using Ki_ADAS.DB;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class Thread_FRCam
    {
        private Thread _frcamThread;
        private int m_frcState = 0;
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

        public Thread_FRCam(VEPBenchClient client, Frm_Main main, Result result)
        {
            _client = client;
            _main = main;
            _result = result;
        }

        public int StartThread()
        {
            try
            {
                if (_frcamThread != null && _frcamThread.IsAlive)
                {
                    StopThread();
                    Thread.Sleep(10);
                }

                m_bRun = true;
                _frcamThread = new Thread(FrCamThread);
                _frcamThread.IsBackground = true;
                _frcamThread.Start();

                return 1;
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStartingFRCamThread", "Error", ex.Message);
                return -1;
            }
        }

        public void StopThread()
        {
            try
            {
                if (_frcamThread != null && _frcamThread.IsAlive)
                {
                    m_bRun = false;
                    _frcamThread.Abort();
                    _frcamThread.Join(500);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStoppingFRCamThread", "Error", ex.Message);
            }
        }

        public bool IsThreadDone()
        {
            return m_bRun;
        }

        public void SetState(int state)
        {
            m_frcState = state;
        }

        private void FrCamThread()
        {
            try
            {
                SetState(TS.STEP_CAM_SEND_INFO);

                while (m_bRun) 
                {
                    Thread.Sleep(10);

                    if (m_frcState == TS.STEP_CAM_SEND_INFO)
                    {
                        _DoSendInfo();
                        _main.AddLogMessage("[FRCam] Send Info Completed");
                        SetState(TS.STEP_CAM_CHECK_OPTION);
                    }
                    else if (m_frcState == TS.STEP_CAM_CHECK_OPTION)
                    {
                        _DoCheckOption();
                        _main.AddLogMessage("[FRCam] Check Option");
                    }
                    else if (m_frcState == TS.STEP_CAM_TARGET_MOVE)
                    {
                        _DoTargetMove();
                        _main.AddLogMessage("[FRCam] Target Move");
                    }
                    else if (m_frcState == TS.STEP_CAM_TARGET_MOVE_COMPLETE)
                    {
                        _DoTargetMoveComplete();
                        _main.AddLogMessage("[FRCam] Target Move Completed");
                        SetState(TS.STEP_CAM_WAIT_SYNC3);
                    }
                    else if (m_frcState == TS.STEP_CAM_WAIT_SYNC3)
                    {
                        _DoWaitSync3();
                        _main.AddLogMessage("[FRCam] WaitSync3");
                    }
                    else if (m_frcState == TS.STEP_CAM_READ_ANGLE)
                    {
                        _DoReadAngle();
                        _main.AddLogMessage("[FRCam] ReadAngle");
                        SetState(TS.STEP_CAM_TARGET_HOME);
                    }
                    else if (m_frcState == TS.STEP_CAM_TARGET_HOME)
                    {
                        _DoTargetHome();
                        _main.AddLogMessage("[FRCam] TargetHome");
                    }
                    else if (m_frcState == TS.STEP_CAM_FINISH)
                    {
                        _DoFinish();
                        _main.AddLogMessage("[FRCam] Finish");
                        m_bRun = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorInFRCamThreadLoop", "Error", ex.Message);
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
                    _main.AddLogMessage("[FRCam] Error: Main form handle not created before Invoke.");
                }

                if (modelInfo == null) 
                {
                    _main.AddLogMessage("[FRCam] Error: ModelInfo is null after Invoke.");
                    return; 
                }

                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_DISTANCE_INDEX, (ushort)(modelInfo.FC_Distance ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_HEIGHT_INDEX, (ushort)(modelInfo.FC_Height ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_INTERDISTANCE_INDEX, (ushort)(modelInfo.FC_InterDistance ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_HTU_INDEX, (ushort)(modelInfo.FC_Htu ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_HTL_INDEX, (ushort)(modelInfo.FC_Htl ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_TS_INDEX, (ushort)(modelInfo.FC_Ts ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_ALLIGNMENTAXEOFFSET_INDEX, (ushort)(modelInfo.FC_AlignmentAxeOffset ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_VV_INDEX, (ushort)(modelInfo.FC_Vv ?? 0));
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.FRONT_CAMERA_STCT_INDEX, (ushort)(modelInfo.FC_StCt ?? 0));

                _client.WriteSynchroZone();

                SetState(TS.STEP_CAM_CHECK_OPTION);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorSendingFRCamInfo", "Error", ex.Message);
            }
        }

        private void _DoCheckOption()
        {
            _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 1); // VEP 서버
            _client.WriteSynchroZone();

            try
            {
                ushort[] readData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 1);

                if (readData == null || readData.Length < 1)
                {
                    return;
                }

                while (true)
                {
                    if (readData[0] == 1)
                    {
                        SetState(TS.STEP_CAM_TARGET_MOVE);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCheckingFRCamOption", "Error", ex.Message);
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
                        SetState(TS.STEP_CAM_TARGET_MOVE_COMPLETE);
                        break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingFRCamTarget", "Error", ex.Message);
            }
        }

        private void _DoTargetMoveComplete()
        {
            try
            {
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_FRONT_CAMERA_INDEX, 1);
                _vepManager.SynchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 20); // VEP 서버
                _client.WriteSynchroZone();

                SetState(TS.STEP_CAM_WAIT_SYNC3);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCompletingFRCamTargetMove", "Error", ex.Message);
            }
        }

        private void _DoWaitSync3()
        {
            try
            {
                ushort[] readData = _client.ReadSynchroZone(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 1);

                if (readData == null || readData.Length < 1)
                {
                    return;
                }

                while (true)
                {
                    if (readData[0] == 20) // OK
                    {
                        SetState(TS.STEP_CAM_READ_ANGLE);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorWaitingForFRCamSync3", "Error", ex.Message);
            }
        }

        private void _DoReadAngle()
        {
            try
            {
                ushort[] angleData = _client.ReadSynchroZone(_vepManager.SynchroZone.FrontCameraAngle1, 3);

                if (angleData == null || angleData.Length < 3)
                {
                    return;
                }

                // 각도값 처리
                ushort roll = angleData[0];
                ushort azimuth = angleData[1];
                ushort elevation = angleData[2];

                SetState(TS.STEP_CAM_TARGET_HOME);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorReadingFRCamAngle", "Error", ex.Message);
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
                        SetState(TS.STEP_CAM_FINISH);
                        break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingFRCamTargetHome", "Error", ex.Message);
            }
        }

        private void _DoFinish()
        {
            try
            {
                // 완료 처리
                if (IsShiftEnterPressed())
                {
                    _result.FC_IsOk = true; // 성공
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFinishingFRCamProcess", "Error", ex.Message);
            }
        }
    }
}
