using Ki_ADAS.DB;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_ADAS
{
    public class Thread_FrontRadar
    {
        private Thread _frontRadarThread;
        private int m_frRadarState = 0;
        private VEPBenchClient _client;
        private Frm_Main _main;
        private Result _result;
        private VEPBenchDataManager _vepManager = GlobalVal.Instance._VEP;
        private Model _model;
        private readonly ManualResetEvent _completionEvent;

        private bool m_bRun = false;

        public Result Result => _result;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private bool IsShiftEnterPressed()
        {
            // 좌쉬프트 + 플러스
            return (GetAsyncKeyState(0xA0) & 0x8000) != 0 &&
                   (GetAsyncKeyState(0x6B) & 0x8000) != 0;
        }

        public Thread_FrontRadar(VEPBenchClient client, Frm_Main main, Result result, ManualResetEvent completionEvent)
        {
            _client = client;
            _main = main;
            _result = result;
            _completionEvent = completionEvent;
        }

        public int StartThread(Model modelToTest)
        {
            try
            {
                if (_frontRadarThread != null && _frontRadarThread.IsAlive)
                {
                    StopThread();
                    Thread.Sleep(10);
                }

                _model = modelToTest;
                if (_model == null)
                {
                    _main.AddLogMessage("[FrontRadar] Error: Model data is null. Aborting thread start.");
                    return -1;
                }

                m_bRun = true;
                _frontRadarThread = new Thread(FrRadarThread);
                _frontRadarThread.IsBackground = true;
                _frontRadarThread.Start();

                return 1;
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStartingFrontRadarThread", "Error", ex.Message);
                return -1;
            }
        }

        public void StopThread()
        {
            try
            {
                if (_frontRadarThread != null && _frontRadarThread.IsAlive)
                {
                    m_bRun = false;
                    _frontRadarThread.Abort();
                    _frontRadarThread.Join(500);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStoppingFrontRadarThread", "Error", ex.Message);
            }
        }

        public void SetState(int state)
        {
            m_frRadarState = state;
        }

        private void FrRadarThread()
        {
            try
            {
                SetState(TS.STEP_FRADAR_SEND_INFO);

                while (m_bRun) 
                {
                    Thread.Sleep(50);

                    if (m_frRadarState == TS.STEP_FRADAR_SEND_INFO)
                    {
                        _DoSendInfo();
                        _main.AddLogMessage("[FrontRadar] Send Info Completed");
                        SetState(TS.STEP_FRADAR_CHECK_OPTION);
                    }
                    else if (m_frRadarState == TS.STEP_FRADAR_CHECK_OPTION)
                    {
                        _DoCheckOption();
                        _main.AddLogMessage("[FrontRadar] Check Option");
                    }
                    else if (m_frRadarState == TS.STEP_FRADAR_TARGET_MOVE)
                    {
                        _DoTargetMove();
                        _main.AddLogMessage("[FrontRadar] Target Move");
                    }
                    else if (m_frRadarState == TS.STEP_FRADAR_TARGET_MOVE_COMPLETE)
                    {
                        _DoTargetMoveComplete();
                        _main.AddLogMessage("[FrontRadar] Target Move Completed");
                        SetState(TS.STEP_FRADAR_WAIT_SYNC);
                    }
                    else if (m_frRadarState == TS.STEP_FRADAR_WAIT_SYNC)
                    {
                        _DoWaitSync();
                        _main.AddLogMessage("[FrontRadar] WaitSync");
                    }
                    else if (m_frRadarState == TS.STEP_FRADAR_READ_ANGLE)
                    {
                        _DoReadAngle();
                        _main.AddLogMessage("[FrontRadar] ReadAngle");
                        SetState(TS.STEP_FRADAR_TARGET_HOME);
                    }
                    else if (m_frRadarState == TS.STEP_FRADAR_TARGET_HOME)
                    {
                        _DoTargetHome();
                        _main.AddLogMessage("[FrontRadar] TargetHome");
                    }
                    else if (m_frRadarState == TS.STEP_FRADAR_FINISH)
                    {
                        _DoFinish();
                        _main.AddLogMessage("[FrontRadar] Finish");
                        m_bRun = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorInFrontRadarThreadLoop", "Error", ex.Message);
            }
            finally
            {
                _completionEvent?.Set();
            }
        }

        private void _DoSendInfo()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarSendInfo");
            try
            {
                if (_model == null)
                {
                    _main.AddLogMessage("[FrontRadar] Error: ModelInfo is null in _DoSendInfo.");
                    SetState(TS.STEP_FRADAR_FINISH); // Abort the sequence
                    return;
                }

                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_RH_XPOSITION_INDEX, (ushort)(_model.FR_X ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_RH_YPOSITION_INDEX, (ushort)(_model.FR_Y ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_RH_ZPOSITION_INDEX, (ushort)(_model.FR_Z ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_RH_ANGLE_INDEX, (ushort)(_model.FR_Angle ?? 0));

                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_LH_XPOSITION_INDEX, (ushort)(_model.FL_X ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_LH_YPOSITION_INDEX, (ushort)(_model.FL_Y ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_LH_ZPOSITION_INDEX, (ushort)(_model.FL_Z ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_RADAR_LH_ANGLE_INDEX, (ushort)(_model.FL_Angle ?? 0));

                _client.WriteSynchroZone();

                SetState(TS.STEP_FRADAR_CHECK_OPTION);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorSendingFrontRadarInfo", "Error", ex.Message);
            }
        }

        private void _DoCheckOption()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarCheckOption");
            _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_RIGHT_RADAR_INDEX, 1); // VEP 서버
            _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_LEFT_RADAR_INDEX, 1);  // VEP 서버
            _client.WriteSynchroZone();

            try
            {
                ushort[] readRhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_RIGHT_RADAR_INDEX, 1);
                ushort[] readLhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_LEFT_RADAR_INDEX, 1);

                if ((readRhData == null || readRhData.Length < 1) || (readLhData == null || readLhData.Length < 1))
                {
                    return;
                }

                while (true)
                {
                    if (readRhData[0] == 1 && readLhData[0] == 1)
                    {
                        SetState(TS.STEP_FRADAR_TARGET_MOVE);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCheckingFrontRadarOption", "Error", ex.Message);
            }
        }

        private void _DoTargetMove()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarTargetMove");

            try
            {
                while (true)
                {
                    if (IsShiftEnterPressed())
                    {
                        SetState(TS.STEP_FRADAR_TARGET_MOVE_COMPLETE);
                        break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingFrontRadarTarget", "Error", ex.Message);
            }
        }

        private void _DoTargetMoveComplete()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarTargetMoveComplete");

            try
            {
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_FRONT_RIGHT_RADAR_INDEX, 1);
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_FRONT_LEFT_RADAR_INDEX, 1);
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_RIGHT_RADAR_INDEX, 20); // VEP 서버
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_LEFT_RADAR_INDEX, 20);  // VEP 서버
                _client.WriteSynchroZone();

                SetState(TS.STEP_FRADAR_WAIT_SYNC);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCompletingFrontRadarTargetMove", "Error", ex.Message);
            }
        }

        private void _DoWaitSync()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarWaitSync");

            try
            {
                ushort[] readRhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_RIGHT_RADAR_INDEX, 1);
                ushort[] readLhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_LEFT_RADAR_INDEX, 1);

                if ((readRhData == null || readRhData.Length < 1) || (readLhData == null || readLhData.Length < 1))
                {
                    return;
                }

                while (true)
                {
                    if (readRhData[0] == 20 && readLhData[0] == 20) // OK
                    {
                        SetState(TS.STEP_FRADAR_READ_ANGLE);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorWaitingForFrontRadarSync", "Error", ex.Message);
            }
        }

        private void _DoReadAngle()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarReadAngle");

            try
            {
                ushort[] angleData = _client.ReadSynchroZone(_vepManager.SynchroZone.FrontRightRadarAngle, 2);

                if (angleData == null || angleData.Length < 2)
                {
                    return;
                }

                // 각도값 처리
                ushort rhAngle = angleData[0];
                ushort lhAngle = angleData[1];

                SetState(TS.STEP_FRADAR_TARGET_HOME);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorReadingFrontRadarAngle", "Error", ex.Message);
            }
        }

        private void _DoTargetHome()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarTargetHome");

            try
            {
                while (true)
                {
                    if (IsShiftEnterPressed())
                    {
                        SetState(TS.STEP_FRADAR_FINISH);
                        break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingFrontRadarTargetHome", "Error", ex.Message);
            }
        }

        private void _DoFinish()
        {
            _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFrontRadarFinish");

            try
            {
                _result.FR_IsOk = true; // 성공
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFinishingFrontRadarProcess", "Error", ex.Message);
            }
        }
    }
}
