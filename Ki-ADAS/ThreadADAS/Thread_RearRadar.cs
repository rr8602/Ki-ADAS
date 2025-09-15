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
    public class Thread_RearRadar
    {
        private Thread _rearRadarThread;
        private int m_rearRadarState = 0;
        private GlobalVal _GV;
        private VEPBenchClient _client;
        private Frm_Main _main;
        private Result _result;
        private VEPBenchDataManager _vepManager = GlobalVal.Instance._VEP;

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

        public Thread_RearRadar(VEPBenchClient client, Frm_Main main, Result result)
        {
            _GV = GlobalVal.Instance;
            _client = client;
            _main = main;
            _result = result;
        }

        public int StartThread()
        {
            try
            {
                if (_rearRadarThread != null && _rearRadarThread.IsAlive)
                {
                    StopThread();
                    Thread.Sleep(10);
                }

                m_bRun = true;
                _rearRadarThread = new Thread(RrRadarThread);
                _rearRadarThread.IsBackground = true;
                _rearRadarThread.Start();

                return 1;
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStartingRearRadarThread", "Error", ex.Message);
                return -1;
            }
        }

        public void StopThread()
        {
            try
            {
                if (_rearRadarThread != null && _rearRadarThread.IsAlive)
                {
                    m_bRun = false;
                    _rearRadarThread.Abort();
                    _rearRadarThread.Join(500);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStoppingRearRadarThread", "Error", ex.Message);
            }
        }

        public bool IsThreadDone()
        {
            return m_bRun;
        }

        public void SetRRState(int state)
        {
            m_rearRadarState = state;
        }

        private void RrRadarThread()
        {
            try
            {
                SetRRState(TS.STEP_RRADAR_SEND_INFO);

                while (m_bRun) 
                {
                    Thread.Sleep(10);

                    if (m_rearRadarState == TS.STEP_RRADAR_SEND_INFO)
                    {
                        _DoSendInfo();
                        _main.AddLogMessage("[RearRadar] Send Info Completed");
                        SetRRState(TS.STEP_RRADAR_CHECK_OPTION);
                    }
                    else if (m_rearRadarState == TS.STEP_RRADAR_CHECK_OPTION)
                    {
                        _DoCheckOption();
                        _main.AddLogMessage("[RearRadar] Check Option");
                    }
                    else if (m_rearRadarState == TS.STEP_RRADAR_TARGET_MOVE)
                    {
                        _DoTargetMove();
                        _main.AddLogMessage("[RearRadar] Target Move");
                    }
                    else if (m_rearRadarState == TS.STEP_RRADAR_TARGET_MOVE_COMPLETE)
                    {
                        _DoTargetMoveComplete();
                        _main.AddLogMessage("[RearRadar] Target Move Completed");
                        SetRRState(TS.STEP_RRADAR_WAIT_SYNC);
                    }
                    else if (m_rearRadarState == TS.STEP_RRADAR_WAIT_SYNC)
                    {
                        _DoWaitSync();
                        _main.AddLogMessage("[RearRadar] WaitSync");
                    }
                    else if (m_rearRadarState == TS.STEP_RRADAR_READ_ANGLE)
                    {
                        _DoReadAngle();
                        _main.AddLogMessage("[RearRadar] ReadAngle");
                        SetRRState(TS.STEP_RRADAR_TARGET_HOME);
                    }
                    else if (m_rearRadarState == TS.STEP_RRADAR_TARGET_HOME)
                    {
                        _DoTargetHome();
                        _main.AddLogMessage("[RearRadar] TargetHome");
                    }
                    else if (m_rearRadarState == TS.STEP_RRADAR_FINISH)
                    {
                        _DoFinish();
                        _main.AddLogMessage("[RearRadar] Finish");
                        m_bRun = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorInRearRadarThreadLoop", "Error", ex.Message);
            }
        }

        private void _DoSendInfo()
        {
            try
            {
                Model modelInfo = null;

                if (_main.IsHandleCreated)
                {
                    _main.Invoke(new Action(() => {
                        modelInfo = _main.SelectedModelInfo;
                    }));
                }
                else
                {
                    _main.AddLogMessage("[RearRadar] Error: Main form handle not created before Invoke.");
                }

                if (modelInfo == null) 
                {
                    _main.AddLogMessage("[RearRadar] Error: ModelInfo is null after Invoke.");
                    return; 
                }

                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_RH_XPOSITION_INDEX, (ushort)(modelInfo.RR_X ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_RH_YPOSITION_INDEX, (ushort)(modelInfo.RR_Y ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_RH_ZPOSITION_INDEX, (ushort)(modelInfo.RR_Z ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_RH_ANGLE_INDEX, (ushort)(modelInfo.RR_Angle ?? 0));

                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_LH_XPOSITION_INDEX, (ushort)(modelInfo.RL_X ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_LH_YPOSITION_INDEX, (ushort)(modelInfo.RL_Y ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_LH_ZPOSITION_INDEX, (ushort)(modelInfo.RL_Z ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.REAR_RADAR_LH_ANGLE_INDEX, (ushort)(modelInfo.RL_Angle ?? 0));

                _client.WriteSynchroZone();

                SetRRState(TS.STEP_FRADAR_CHECK_OPTION);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorSendingRearRadarInfo", "Error", ex.Message);
            }
        }

        private void _DoCheckOption()
        {
            _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 1); // VEP 서버
            _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 1);  // VEP 서버
            _client.WriteSynchroZone();

            try
            {
                ushort[] readRhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 1);
                ushort[] readLhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 1);

                if ((readRhData == null || readRhData.Length < 1) || (readLhData == null || readLhData.Length < 1))
                {
                    return;
                }

                while (true)
                {
                    if (readRhData[0] == 1 && readLhData[0] == 1)
                    {
                        SetRRState(TS.STEP_RRADAR_TARGET_MOVE);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCheckingRearRadarOption", "Error", ex.Message);
            }
        }

        private void _DoTargetMove()
        {
            try
            {
                while (true)
                {
                    if (IsShiftEnterPressed())
                    {
                        SetRRState(TS.STEP_RRADAR_TARGET_MOVE_COMPLETE);
                        break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingRearRadarTarget", "Error", ex.Message);
            }
        }

        private void _DoTargetMoveComplete()
        {
            try
            {
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_RIGHT_RADAR_INDEX, 1);
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_LEFT_RADAR_INDEX, 1);
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 20); // VEP 서버
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 20);  // VEP 서버
                _client.WriteSynchroZone();

                SetRRState(TS.STEP_RRADAR_WAIT_SYNC);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCompletingRearRadarTargetMove", "Error", ex.Message);
            }
        }

        private void _DoWaitSync()
        {
            try
            {
                ushort[] readRhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 1);
                ushort[] readLhData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 1);

                if ((readRhData == null || readRhData.Length < 1) || (readLhData == null || readLhData.Length < 1))
                {
                    return;
                }

                while (true)
                {
                    if (readRhData[0] == 20 && readLhData[0] == 20) // OK
                    {
                        SetRRState(TS.STEP_RRADAR_READ_ANGLE);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorWaitingForRearRadarSync", "Error", ex.Message);
            }
        }

        private void _DoReadAngle()
        {
            try
            {
                ushort[] angleData = _client.ReadSynchroZone(_vepManager.SynchroZone.RearRightRadarAngle, 2);

                if (angleData == null || angleData.Length < 2)
                {
                    return;
                }

                // 각도값 처리
                ushort rhAngle = angleData[0];
                ushort lhAngle = angleData[1];

                SetRRState(TS.STEP_RRADAR_TARGET_HOME);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorReadingRearRadarAngle", "Error", ex.Message);
            }
        }

        private void _DoTargetHome()
        {
            try
            {
                while (true)
                {
                    if (IsShiftEnterPressed())
                    {
                        SetRRState(TS.STEP_RRADAR_FINISH);
                        break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingRearRadarTargetHome", "Error", ex.Message);
            }
        }

        private void _DoFinish()
        {
            try
            {
                // 완료 처리
                if (IsShiftEnterPressed())
                {
                    _result.RR_IsOk = true; // 성공
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFinishingRearRadarProcess", "Error", ex.Message);
            }
        }
    }
}