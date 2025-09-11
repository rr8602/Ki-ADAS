using Ki_ADAS.DB;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Ki_ADAS
{
    public class Thread_Main
    {
        private Thread _testThread = null;
        private VEPBenchDataManager _vepManager = GlobalVal.Instance._VEP;
        private VEPBenchClient _client;
        private Thread_FRCam _frCam = null;
        private Thread_FrontRadar _frontRadar = null;
        private Thread_RearRadar _rearRadar = null;
        private Result _result;
        private ResultRepository _resultRepository;
        private Frm_Main _main;

        private bool m_bRun = false;
        private bool m_bExitStep = false;
        private int m_nState = 0;
        public bool m_bPassNext = false;

        private bool m_bBarcode = false;
        private readonly object _lock = new object();

        private bool _bFRCamComplate = false;
        private bool _bFornt_RadarCamComplate = false;
        private bool _bRear_RadarCamComplate = false;

        private Info Cur_Info = null;
        private Model Cur_Model = null;

        public static event Action<string> OnStatusUpdate;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private bool IsShiftEnterPressed()
        {
            // 좌쉬프트 + 플러스
            return (GetAsyncKeyState(0xA0) & 0x8000) != 0 &&
                   (GetAsyncKeyState(0x6B) & 0x8000) != 0;
        }

        public Thread_Main(Frm_Main main, VEPBenchClient client, SettingConfigDb db)
        {
            _result = new Result();
            _resultRepository = new ResultRepository(db);
            _client = client;
            _frCam = new Thread_FRCam(_client, main, _result);
            _frontRadar = new Thread_FrontRadar(_client, main, _result);
            _rearRadar = new Thread_RearRadar(_client, main, _result);
            _main = main;
        }

        public void SetBarcode(Info pInfo, Model pModel)
        {
            Cur_Info = pInfo;
            Cur_Model = pModel;
            m_bBarcode = true;
        }

        public int StartThread()
        {
            try
            {
                if (m_bRun) return 0;

                if (_testThread != null)
                {
                    StopThread();
                    Thread.Sleep(1000);
                }


                m_bRun = true;
                m_bExitStep = false;

                _testThread = new Thread(Main_Thread);
                _testThread.IsBackground = true;
                _testThread.Start(this);

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void StopThread()
        {
            try
            {
                lock (_lock)
                {
                    m_bRun = false;
                }
                if (_testThread != null && _testThread.IsAlive)
                {
                    if (!_testThread.Join(3000))
                    {
                        _testThread.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void UI_Update_Message(string Message)
        {
            try
            {
                OnStatusUpdate?.Invoke(Message);
            }
            catch (Exception ex)
            {
            }
        }

        private bool CheckLoopExit()
        {
            return !m_bRun || m_bExitStep || IsShiftEnterPressed() || m_bPassNext;
        }

        public void SetState(int state)
        {
            m_nState = state;
            m_bPassNext = false;
        }

        private void Main_Thread(Object obj)
        {
            try
            {
                UI_Update_Message("Main Thread Start");

                SetState(TS.STEP_MAIN_WAIT);

                while (true)
                {
                    if (!m_bRun) break;
                    Thread.Sleep(100);

                    if (m_nState == TS.STEP_MAIN_WAIT)
                    {
                        _DoMainInitial();
                        _main.AddLogMessage("[Main] Main Wait");
                    }
                    else if (m_nState == TS.STEP_MAIN_BARCODE_WAIT)
                    {
                        _DoMainBarcodeWait();
                        _main.AddLogMessage("[Main] Main Barcode Wait");
                    }
                    else if (m_nState == TS.STEP_MAIN_CHECK_DETECTION_SENSOR)
                    {
                        _DoMainCheck_Detect();
                        _main.AddLogMessage("[Main] Main Check Detection Sensor");
                    }
                    else if (m_nState == TS.STEP_MAIN_PRESS_START_BUTTON)
                    {
                        _DoMainPressCycle();
                        _main.AddLogMessage("[Main] Main Press Start Button");
                    }
                    else if (m_nState == TS.STEP_MAIN_CENTERING_ON)
                    {
                        _DoMainCenteringOn();
                        _main.AddLogMessage("[Main] Main Centering On");
                    }
                    else if (m_nState == TS.STEP_MAIN_PEV_START_CYCLE)
                    {
                        _DoMainPEVStartCycle();
                        _main.AddLogMessage("[Main] Main PEV Start Cycle");
                    }
                    else if (m_nState == TS.STEP_MAIN_PEV_SEND_PJI)
                    {
                        _DoMainPEVSendPJI();
                        _main.AddLogMessage("[Main] Main PEV Send PJI");
                    }
                    else if (m_nState == TS.STEP_MAIN_PEV_READY)
                    {
                        _DoMainPEVReady();
                        _main.AddLogMessage("[Main] Main PEV Ready");
                    }
                    else if (m_nState == TS.STEP_MAIN_START_EACH_THREAD)
                    {
                        _DoMainStartEachThread();
                        _main.AddLogMessage("[Main] Main Start Each Thread");
                    }
                    else if (m_nState == TS.STEP_MAIN_WAIT_TEST_COMPLETE)
                    {
                        _DoMainWaitTestComplete();
                        _main.AddLogMessage("[Main] Main Wait Test Complete");
                    }
                    else if (m_nState == TS.STEP_MAIN_CENTERING_HOME)
                    {
                        _DoMainCenteringHome();
                        _main.AddLogMessage("[Main] Main Centering Home");
                    }
                    else if (m_nState == TS.STEP_MAIN_WAIT_TARGET_HOME)
                    {
                        _DoMainWaitTargetHome();
                        _main.AddLogMessage("[Main] Main Wait Target Home");
                    }
                    else if (m_nState == TS.STEP_MAIN_DATA_SAVE)
                    {
                        _DoMainDataSave();
                        _main.AddLogMessage("[Main] Main Data Save");
                    }
                    else if (m_nState == TS.STEP_MAIN_TICKET_PRINT)
                    {
                        _DoMainTicketPrint();
                        _main.AddLogMessage("[Main] Main Ticket Print");
                    }
                    else if (m_nState == TS.STEP_MAIN_GRET_COMM)
                    {
                        _DoMainGRETComm();
                        _main.AddLogMessage("[Main] Main Gret Comm");
                    }
                    else if (m_nState == TS.STEP_MAIN_WAIT_GO_OUT)
                    {
                        _DoMainWaitGoOut();
                        _main.AddLogMessage("[Main] Main Wait Go Out");
                    }
                    else if (m_nState == TS.STEP_MAIN_CYCLE_FINISH)
                    {
                        _DoMainFinishCycle();
                        _main.AddLogMessage("[Main] Main Cycle Finish");
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }
        private void _DoMainInitial()
        {
            try
            {
                //변수들 초기화
                m_bBarcode = false;
                m_bPassNext = false;

                SetState(TS.STEP_MAIN_BARCODE_WAIT);
            }
            catch (Exception ex)
            {

            }
        }
        private void _DoMainBarcodeWait()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    if (m_bBarcode)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }

                if (_main.IsHandleCreated)
                {
                    var info = (Info)null;
                    var model = (Model)null;

                    _main.Invoke(new Action(() => {
                        info = _main.SelectedVehicleInfo;
                        model = _main.SelectedModelInfo;
                    }));

                    SetBarcode(info, model);
                }

                SetState(TS.STEP_MAIN_CHECK_DETECTION_SENSOR);
            }
            catch (Exception ex)
            {

            }
        }
        private void _DoMainCheck_Detect()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    //차량 진입 시
                    //if (PLC.DI.FDetect &&  PLC.DI.RDetect) break;
                    Thread.Sleep(100);
                }

                SetState(TS.STEP_MAIN_PRESS_START_BUTTON);
            }
            catch (Exception ex)
            {

            }
        }
        private void _DoMainPressCycle()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    // 시작버튼 클릭시
                    // if (PLC.DI.PressStart) break;
                    Thread.Sleep(100);
                }
                SetState(TS.STEP_MAIN_CENTERING_ON);
            }
            catch (Exception ex)
            {

            }
        }

        private void _DoMainCenteringOn()
        {
            try
            {
                // 루프 들어가기전에 센터링 신호 전송
                // PLC.DO.CenterOn = true ;
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    // 시작버튼 클릭시
                    //if (PLC.DI.CenteringOn) break;
                    Thread.Sleep(100);
                }
                SetState(TS.STEP_MAIN_PEV_START_CYCLE);
            }
            catch (Exception ex)
            {

            }
        }

        private void _DoMainPEVStartCycle()
        {
            try
            {
                _vepManager.StatusZone.StartCycle = 1;
                _client.WriteStatusZone();

                SetState(TS.STEP_MAIN_PEV_SEND_PJI);
            }
            catch { }
        }

        private void _DoMainPEVSendPJI()
        {
            try
            {
                _vepManager.TransmissionZone.ExchStatus = 2; // VEP 서버

                // PJI 정보 전송
                if (Cur_Model != null &&
                    _vepManager.TransmissionZone.ExchStatus == VEPBenchTransmissionZone.ExchStatus_Request &&
                    _vepManager.TransmissionZone.AddTzSize == 0 &&
                    _vepManager.TransmissionZone.FctCode == 6 &&
                    _vepManager.TransmissionZone.PCNum == 1 &&
                    _vepManager.TransmissionZone.ProcessCode == 1 &&
                    _vepManager.TransmissionZone.SubFctCode == 0)
                {
                    _vepManager.TransmissionZone.ExchStatus = VEPBenchTransmissionZone.ExchStatus_Response;

                    if (Cur_Info != null && !string.IsNullOrEmpty(Cur_Info.PJI))
                    {
                        byte[] bytes = Encoding.Unicode.GetBytes(Cur_Info.PJI);
                        ushort[] pjiData = new ushort[(bytes.Length + 1) / 2];
                        Buffer.BlockCopy(bytes, 0, pjiData, 0, bytes.Length);
                        _vepManager.ReceptionZone.Data = pjiData;
                    }
                    else
                    {
                        _vepManager.ReceptionZone.Data = new ushort[0];
                    }

                    _vepManager.StatusZone.VepStatus = 2; // VEP 서버
                    _vepManager.ReceptionZone.ExchStatus = VEPBenchReceptionZone.ExchStatus_Ready;
                    _vepManager.StatusZone.StartCycle = 0;

                    _client.WriteTransmissionZone();
                    _client.WriteReceptionZone();
                    _client.WriteStatusZone();
                }

                SetState(TS.STEP_MAIN_PEV_READY);
            }
            catch { }
        }

        // Check VEP Status
        private void _DoMainPEVReady()
        {
            try
            {
                if (_vepManager.StatusZone.VepStatus == 2)
                {
                    SetState(TS.STEP_MAIN_START_EACH_THREAD);
                }
            }
            catch { }
        }

        private void _DoMainStartEachThread()
        {
            try
            {
                if (_frCam.StartThread() == 1 && _frontRadar.StartThread() == 1 && _rearRadar.StartThread() == 1)
                {
                    _main.m_frmParent.User_Monitor.StartInspectionTimer();
                    _result.StartTime = DateTime.Now;
                    SetState(TS.STEP_MAIN_WAIT_TEST_COMPLETE);
                }
            }
            catch { }
        }

        private void _DoMainWaitTestComplete()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);
                }

                _main.m_frmParent.User_Monitor.StopInspectionTimer();
                _result.EndTime = DateTime.Now;
                SetState(TS.STEP_MAIN_CENTERING_HOME);
            }
            catch { }
        }

        private void _DoMainCenteringHome()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);
                }

                SetState(TS.STEP_MAIN_WAIT_TARGET_HOME);
            }
            catch { }
        }

        private void _DoMainWaitTargetHome()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);
                }

                SetState(TS.STEP_MAIN_DATA_SAVE);
            }
            catch { }
        }

        private void _DoMainDataSave()
        {
            try
            {
                Result result = CreateResultInfo();
                bool isSavedToDb = _resultRepository.SaveResult(result);

                if (isSavedToDb)
                {
                    MessageBox.Show(LanguageManager.GetString("TestResultsSavedToDatabase"),
                                    LanguageManager.GetString("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(LanguageManager.GetString("FailedToSaveTestResultsToDatabase"),
                                    LanguageManager.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string xmlFileName = $"test_result_{DateTime.Now.ToString("yyyyMMdd")}.xml";
                string xmlFilePath = Path.Combine(Application.StartupPath, xmlFileName);

                try
                {
                    XElement newResult = new XElement("TestResults",
                        new XElement("AcceptNo", result.AcceptNo),
                        new XElement("PJI", result.PJI),
                        new XElement("Model", result.Model),
                        new XElement("StartTime", result.StartTime),
                        new XElement("EndTime", result.EndTime),
                        new XElement("FC_IsOk", result.FC_IsOk),
                        new XElement("FR_IsOk", result.FR_IsOk),
                        new XElement("RR_IsOk", result.RR_IsOk),
                        new XElement("Angle_Roll", _vepManager.SynchroZone.FrontCameraAngle1),
                        new XElement("Angle_Azimuth", _vepManager.SynchroZone.FrontCameraAngle2),
                        new XElement("Angle_Elevation", _vepManager.SynchroZone.FrontCameraAngle3),
                        new XElement("Angle_RearRight", _vepManager.SynchroZone.RearRightRadarAngle),
                        new XElement("Angle_RearLeft", _vepManager.SynchroZone.RearLeftRadarAngle),
                        new XElement("Angle-FrontRight", _vepManager.SynchroZone.FrontRightRadarAngle),
                        new XElement("Angle-FrontLeft", _vepManager.SynchroZone.FrontLeftRadarAngle)
                    );

                    XElement root;

                    if (File.Exists(xmlFilePath))
                    {
                        root = XElement.Load(xmlFilePath);
                        root.Add(newResult);
                    }
                    else
                    {
                        root = new XElement("TestResults", newResult);
                    }

                    root.Save(xmlFilePath);

                    MessageBox.Show(LanguageManager.GetString("TestResultsSavedAsXML"),
                                    LanguageManager.GetString("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(LanguageManager.GetFormattedString("ErrorSavingData", ex.Message),
                                    LanguageManager.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SetState(TS.STEP_MAIN_TICKET_PRINT);
            }
            catch { }
        }

        private void _DoMainTicketPrint()
        {
            try
            {
                SetState(TS.STEP_MAIN_GRET_COMM);
            }
            catch { }
        }

        private void _DoMainGRETComm()
        {
            try
            {
                SetState(TS.STEP_MAIN_WAIT_GO_OUT);
            }
            catch { }
        }

        private void _DoMainWaitGoOut()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;
                    Thread.Sleep(100);
                }

                SetState(TS.STEP_MAIN_CYCLE_FINISH);
            }
            catch { }
        }

        private void _DoMainFinishCycle()
        {
            try
            {
                // 사이클 종료시 처리 하는 함수
                Thread.Sleep(10);
                SetState(TS.STEP_MAIN_WAIT);
            }
            catch (Exception ex)
            {

            }
        }

        private Result CreateResultInfo()
        {
            return new Result
            {
                AcceptNo = Cur_Info?.AcceptNo ?? string.Empty,
                PJI = Cur_Info?.PJI ?? string.Empty,
                Model = Cur_Model?.Name ?? string.Empty,
                StartTime = _result.StartTime,
                EndTime = _result.EndTime,
                FC_IsOk = _frCam?.Result.RR_IsOk ?? false,
                FR_IsOk = _frontRadar?.Result.RR_IsOk ?? false,
                RR_IsOk = _rearRadar?.Result.RR_IsOk ?? false,
            };
        }
    }
}
