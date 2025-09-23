using Keyence.AutoID.SDK;

using Ki_ADAS.DB;
using Ki_ADAS.ThreadADAS;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        private delegate void delegateUserControl(string str);

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
                    Thread.Sleep(100);
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
                MsgBox.ErrorWithFormat("ErrorStartingMainThread", "Error", ex.Message);
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
                MsgBox.ErrorWithFormat("ErrorStoppingMainThread", "Error", ex.Message);
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
                MsgBox.ErrorWithFormat("ErrorUpdatingUIMessage", "Error", ex.Message);
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
                    /*else if (m_nState == TS.STEP_MAIN_BARCODE_WAIT)
                    {
                        _DoMainBarcodeWait();
                        _main.AddLogMessage("[Main] Main Barcode Wait");
                    }*/
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
                        _main.AddLogMessage("[Main] Starting Test Threads...");
                    }
                    else if (m_nState == TS.STEP_MAIN_WAIT_TEST_COMPLETE)
                    {
                        _DoMainWaitTestComplete();
                        _main.AddLogMessage("[Main] All Test Threads Complete");
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
                MsgBox.ErrorWithFormat("ErrorInMainThreadLoop", "Error", ex.Message);
            }

        }

        private void _DoMainInitial()
        {
            try
            {
                //변수들 초기화
                m_bBarcode = false;
                m_bPassNext = false;

                SetState(TS.STEP_MAIN_CHECK_DETECTION_SENSOR);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorInitializingMainVariables", "Error", ex.Message);
            }
        }

        /*private void _DoMainBarcodeWait()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescScanBarcode");

                if (_main.IsHandleCreated)
                {
                    Info info = null;
                    Model model = null;

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
                MsgBox.ErrorWithFormat("ErrorWaitingForBarcode", "Error", ex.Message);
            }
        }*/

        private void _DoMainCheck_Detect()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescMoveVehicle");

                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);

                    //차량 진입 시
                    //if (PLC.DI.FDetect &&  PLC.DI.RDetect) break;
                }

                SetState(TS.STEP_MAIN_PRESS_START_BUTTON);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCheckingDetectionSensor", "Error", ex.Message);
            }
        }
        private void _DoMainPressCycle()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescPressStart");

                while (true)
                {
                    if (CheckLoopExit())
                    {
                        _main.m_frmParent.User_Monitor.StartInspectionTimer();
                        _result.StartTime = DateTime.Now;
                        break;
                    }

                    Thread.Sleep(100);
                    // 시작버튼 클릭시
                    // if (PLC.DI.PressStart) break;
                }

                SetState(TS.STEP_MAIN_CENTERING_ON);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorPressingStartButton", "Error", ex.Message);
            }
        }

        private void _DoMainCenteringOn()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescCentering");

                // 루프 들어가기전에 센터링 신호 전송
                // PLC.DO.CenterOn = true ;
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);

                    // 시작버튼 클릭시
                    //if (PLC.DI.CenteringOn) break;
                }

                SetState(TS.STEP_MAIN_PEV_START_CYCLE);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDuringCenteringOn", "Error", ex.Message);
            }
        }

        private void _DoMainPEVStartCycle()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescCommVep");
                _vepManager.StatusZone.StartCycle = 1;
                _client.WriteStatusZone();

                SetState(TS.STEP_MAIN_PEV_SEND_PJI);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStartingPEVCycle", "Error", ex.Message);
            }
        }

        private void _DoMainPEVSendPJI()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescSendPji");
                _vepManager.TransmissionZone.SetValue(VEPBenchTransmissionZone.Offset_ExchStatus, 2); // VEP 서버
                _client.WriteTransmissionZone();

                // PJI 정보 전송
                if (Cur_Model != null &&
                    _vepManager.TransmissionZone.ExchStatus == VEPBenchTransmissionZone.ExchStatus_Request &&
                    _vepManager.TransmissionZone.AddTzSize == 0 &&
                    _vepManager.TransmissionZone.FctCode == 6 &&
                    _vepManager.TransmissionZone.PCNum == 1 &&
                    _vepManager.TransmissionZone.ProcessCode == 1 &&
                    _vepManager.TransmissionZone.SubFctCode == 0)
                {
                    _vepManager.TransmissionZone.SetValue(VEPBenchTransmissionZone.Offset_ExchStatus, VEPBenchTransmissionZone.ExchStatus_Response);

                    if (Cur_Info != null && !string.IsNullOrEmpty(Cur_Info.PJI))
                    {
                        _vepManager.ReceptionZone.SetValue(VEPBenchReceptionZone.Offset_FctAndPCNum, 0x0106); // FCT 6, PC 1
                        _vepManager.ReceptionZone.SetValue(VEPBenchReceptionZone.Offset_ProcessAndSubFct, 0x0001); // Process 1, SubFct 0
                        _vepManager.ReceptionZone.SetValue(VEPBenchReceptionZone.Offset_DataStart, 0x0701);

                        byte[] bytes = Encoding.ASCII.GetBytes(Cur_Info.PJI);
                        ushort[] pjiData = new ushort[(bytes.Length + 1) / 2];

                        for (int i = 0; i < bytes.Length; i += 2)
                            pjiData[i / 2] = (ushort)(bytes[i] | ((i + 1 < bytes.Length ? bytes[i + 1] : 0) << 8));

                        _vepManager.ReceptionZone.Data = pjiData;
                    }
                    else
                    {
                        _vepManager.ReceptionZone.Data = new ushort[0];
                    }

                    _vepManager.StatusZone.SetValue(VEPBenchStatusZone.Offset_VepStatus, 2); // VEP 서버
                    _vepManager.ReceptionZone.SetValue(VEPBenchReceptionZone.Offset_ExchStatus, VEPBenchReceptionZone.ExchStatus_Ready);
                    _vepManager.StatusZone.SetValue(VEPBenchStatusZone.Offset_StartCycle, 0);

                    _client.WriteTransmissionZone();
                    _client.WriteReceptionZone();
                    _client.WriteStatusZone();
                }

                SetState(TS.STEP_MAIN_PEV_READY);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorSendingPJI", "Error", ex.Message);
            }
        }

        // Check VEP Status
        private void _DoMainPEVReady()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescVepReady");

                while (true)
                {
                    if (_vepManager.StatusZone.VepStatus == 2)
                    {
                        SetState(TS.STEP_MAIN_START_EACH_THREAD);
                        break;
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCheckingPEVStatus", "Error", ex.Message);
            }
        }

        private void _DoMainStartEachThread()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescStartSensorTest");

                while (true)
                {
                    if (Cur_Model.FC_IsTest == true)
                    {
                        _frCam.StartThread();
                    }

                    if (Cur_Model.F_IsTest == true)
                    {
                        _frontRadar.StartThread();
                    }

                    if (Cur_Model.R_IsTest == true)
                    {
                        _rearRadar.StartThread();
                    }

                    SetState(TS.STEP_MAIN_WAIT_TEST_COMPLETE);
                    break;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStartingSubThreads", "Error", ex.Message);
            }
        }

        private void _DoMainWaitTestComplete()
        {
            try
            {
                Thread.Sleep(5000); // 각 검사 쓰레드 종료될 때까지 대기

                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescWaitTestComplete");

                bool fcDone = !Cur_Model.FC_IsTest || (Cur_Model.FC_IsTest && _frCam.IsThreadDone());
                bool frDone = !Cur_Model.F_IsTest || (Cur_Model.F_IsTest && _frontRadar.IsThreadDone());
                bool rrDone = !Cur_Model.R_IsTest || (Cur_Model.R_IsTest && _rearRadar.IsThreadDone());

                while (true)
                {
                    if (fcDone || frDone || rrDone)
                    {
                        _main.m_frmParent.User_Monitor.StopInspectionTimer();
                        _result.EndTime = DateTime.Now;
                        SetState(TS.STEP_MAIN_CENTERING_HOME);

                        break;
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorWaitingForTestCompletion", "Error", ex.Message);
            }
        }

        private void _DoMainCenteringHome()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescCenteringHome");

                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);
                }

                SetState(TS.STEP_MAIN_WAIT_TARGET_HOME);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDuringCenteringHome", "Error", ex.Message);
            }
        }

        private void _DoMainWaitTargetHome()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescWaitTargetHome");

                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);
                }

                SetState(TS.STEP_MAIN_DATA_SAVE);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorWaitingForTargetHome", "Error", ex.Message);
            }
        }

        private void _DoMainDataSave()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescSaveResult");

                // DB에 결과 저장
                Result result = CreateResultInfo();
                bool isSavedToDb = _resultRepository.SaveResult(result);

                if (isSavedToDb)
                {
                    _main.AddLogMessage("[Main] Test results saved to database.");
                }
                else
                {
                    _main.AddLogMessage("[Main] Failed to save test results to database.");
                }

                // XML 파일 생성
                if (string.IsNullOrEmpty(result.AcceptNo) || Cur_Model == null)
                {
                    _main.AddLogMessage("[Main] VIN or Model is empty, skipping XML save.");
                    SetState(TS.STEP_MAIN_TICKET_PRINT);
                    return;
                }

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string xmlFileName = $"{timestamp}-{result.PJI}-1.xml";
                string sPath = Path.Combine(Application.StartupPath, xmlFileName);

                using (System.Xml.XmlTextWriter textWriter = new System.Xml.XmlTextWriter(sPath, Encoding.UTF8))
                {
                    textWriter.Formatting = System.Xml.Formatting.Indented;
                    textWriter.WriteStartDocument();

                    textWriter.WriteStartElement("STATION_SETTING");

                    textWriter.WriteStartElement("DataModel");
                    textWriter.WriteAttributeString("version", "1.2.B");
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("FileFormat");
                    textWriter.WriteAttributeString("version", "1.2.B");
                    textWriter.WriteEndElement();

                    textWriter.WriteElementString("MainPart_ID", result.AcceptNo);
                    textWriter.WriteElementString("Site", "UGB");
                    textWriter.WriteElementString("TopStartCyclePart", "true");
                    textWriter.WriteElementString("TopPart", "true");

                    textWriter.WriteStartElement("ADASCalibrationProcessType");

                    bool allSensorsOk = Cur_Model.FC_IsTest ? result.FC_IsOk : true && Cur_Model.F_IsTest ? result.FR_IsOk : true && Cur_Model.R_IsTest ? result.RR_IsOk : true;
                    textWriter.WriteElementString("VerdictOK", allSensorsOk.ToString().ToLower());
                    textWriter.WriteElementString("BenchNumber", "2");
                    string cycleTime = (_result.EndTime - _result.StartTime).TotalSeconds.ToString("F6");
                    textWriter.WriteElementString("CycleTime", cycleTime);

                    textWriter.WriteStartElement("ADASSensorToolTypesList");

                    // FRC - FRONT CAMERA
                    textWriter.WriteStartElement("ADASSensorToolType");
                    textWriter.WriteAttributeString("ADASSensorType", "CAM");
                    textWriter.WriteAttributeString("Name", "FRC");
                    textWriter.WriteAttributeString("Description", "FRONT CAMERA");
                    textWriter.WriteAttributeString("LateralPositionType", "CENTRAL");
                    textWriter.WriteAttributeString("LongitudinalPositionType", "FRONT");
                    textWriter.WriteElementString("VerdictOK", Cur_Model.FC_IsTest ? result.FC_IsOk.ToString().ToLower() : "false");
                    textWriter.WriteElementString("IsPresent", Cur_Model.FC_IsTest.ToString().ToLower());
                    textWriter.WriteElementString("FrictionResult", null);
                    textWriter.WriteElementString("FinalFrictionTorque", null);

                    if (Cur_Model.FC_IsTest)
                    {
                        textWriter.WriteElementString("MeasureCount", "1");
                        textWriter.WriteElementString("FinalAngleX", _frCam.FinalAngleX.ToString("F6"));
                        textWriter.WriteElementString("FinalAngleY", _frCam.FinalAngleY.ToString("F6"));
                        textWriter.WriteElementString("FinalAngleZ", _frCam.FinalAngleZ.ToString("F6"));
                        textWriter.WriteElementString("ShapeType", "2");
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", (Cur_Model.FC_Distance ?? 0).ToString("F6"));
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", (Cur_Model.FC_AlignmentAxeOffset ?? 0).ToString("F6"));
                        textWriter.WriteElementString("CmdPositionTargetHeight", (Cur_Model.FC_Height ?? 0).ToString("F6"));
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", (Cur_Model.FC_Htu ?? 0).ToString("F6"));
                        textWriter.WriteElementString("CmdTargetEntrax", (Cur_Model.FC_InterDistance ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalCycleTime", cycleTime);
                    }
                    else
                    {
                        textWriter.WriteElementString("MeasureCount", null);
                        textWriter.WriteElementString("FinalAngleX", null);
                        textWriter.WriteElementString("FinalAngleY", null);
                        textWriter.WriteElementString("FinalAngleZ", null);
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", null);
                    }

                    textWriter.WriteEndElement();

                    // FRR - FRONT RADAR RIGHT
                    textWriter.WriteStartElement("ADASSensorToolType");
                    textWriter.WriteAttributeString("ADASSensorType", "RAD");
                    textWriter.WriteAttributeString("Name", "FRR");
                    textWriter.WriteAttributeString("Description", "FRONT RADAR RIGHT");
                    textWriter.WriteAttributeString("LateralPositionType", "RIGHT");
                    textWriter.WriteAttributeString("LongitudinalPositionType", "FRONT");
                    textWriter.WriteElementString("VerdictOK", Cur_Model.F_IsTest ? result.FR_IsOk.ToString().ToLower() : "false");
                    textWriter.WriteElementString("IsPresent", Cur_Model.F_IsTest.ToString().ToLower());
                    textWriter.WriteElementString("FrictionResult", null);
                    textWriter.WriteElementString("FinalFrictionTorque", null);

                    if (Cur_Model.F_IsTest)
                    {
                        textWriter.WriteElementString("MeasureCount", "1");
                        textWriter.WriteElementString("FinalAngleX", (Cur_Model.FR_X ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleY", (Cur_Model.FR_Y ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleZ", (Cur_Model.FR_Z ?? 0).ToString("F6"));
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", cycleTime);
                    }
                    else
                    {
                        textWriter.WriteElementString("MeasureCount", null);
                        textWriter.WriteElementString("FinalAngleX", null);
                        textWriter.WriteElementString("FinalAngleY", null);
                        textWriter.WriteElementString("FinalAngleZ", null);
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", null);
                    }

                    textWriter.WriteEndElement();

                    // FRR - FRONT RADAR LEFT
                    textWriter.WriteStartElement("ADASSensorToolType");
                    textWriter.WriteAttributeString("ADASSensorType", "RAD");
                    textWriter.WriteAttributeString("Name", "FRR");
                    textWriter.WriteAttributeString("Description", "FRONT RADAR LEFT");
                    textWriter.WriteAttributeString("LateralPositionType", "LEFT");
                    textWriter.WriteAttributeString("LongitudinalPositionType", "FRONT");
                    textWriter.WriteElementString("VerdictOK", Cur_Model.F_IsTest ? result.FR_IsOk.ToString().ToLower() : "false");
                    textWriter.WriteElementString("IsPresent", Cur_Model.F_IsTest.ToString().ToLower());
                    textWriter.WriteElementString("FrictionResult", null);
                    textWriter.WriteElementString("FinalFrictionTorque", null);

                    if (Cur_Model.F_IsTest)
                    {
                        textWriter.WriteElementString("MeasureCount", "1");
                        textWriter.WriteElementString("FinalAngleX", (Cur_Model.FL_X ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleY", (Cur_Model.FL_Y ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleZ", (Cur_Model.FL_Z ?? 0).ToString("F6"));
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", cycleTime);
                    }
                    else
                    {
                        textWriter.WriteElementString("MeasureCount", null);
                        textWriter.WriteElementString("FinalAngleX", null);
                        textWriter.WriteElementString("FinalAngleY", null);
                        textWriter.WriteElementString("FinalAngleZ", null);
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", null);
                    }

                    textWriter.WriteEndElement();

                    // RSR - REAR RADAR RIGHT
                    textWriter.WriteStartElement("ADASSensorToolType");
                    textWriter.WriteAttributeString("ADASSensorType", "RAD");
                    textWriter.WriteAttributeString("Name", "RSR");
                    textWriter.WriteAttributeString("Description", "REAR RADAR RIGHT");
                    textWriter.WriteAttributeString("LateralPositionType", "RIGHT");
                    textWriter.WriteAttributeString("LongitudinalPositionType", "REAR");
                    textWriter.WriteElementString("VerdictOK", Cur_Model.R_IsTest ? result.RR_IsOk.ToString().ToLower() : "false");
                    textWriter.WriteElementString("IsPresent", Cur_Model.R_IsTest.ToString().ToLower());
                    textWriter.WriteElementString("FrictionResult", null);
                    textWriter.WriteElementString("FinalFrictionTorque", null);

                    if (Cur_Model.R_IsTest)
                    {
                        textWriter.WriteElementString("MeasureCount", "1");
                        textWriter.WriteElementString("FinalAngleX", (Cur_Model.RR_X ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleY", (Cur_Model.RR_Y ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleZ", (Cur_Model.RR_Z ?? 0).ToString("F6"));
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", cycleTime);
                    }
                    else
                    {
                        textWriter.WriteElementString("MeasureCount", null);
                        textWriter.WriteElementString("FinalAngleX", null);
                        textWriter.WriteElementString("FinalAngleY", null);
                        textWriter.WriteElementString("FinalAngleZ", null);
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", null);
                    }

                    textWriter.WriteEndElement();

                    // RSL - REAR RADAR LEFT
                    textWriter.WriteStartElement("ADASSensorToolType");
                    textWriter.WriteAttributeString("ADASSensorType", "RAD");
                    textWriter.WriteAttributeString("Name", "RSL");
                    textWriter.WriteAttributeString("Description", "REAR RADAR LEFT");
                    textWriter.WriteAttributeString("LateralPositionType", "LEFT");
                    textWriter.WriteAttributeString("LongitudinalPositionType", "REAR");
                    textWriter.WriteElementString("VerdictOK", Cur_Model.R_IsTest ? result.RR_IsOk.ToString().ToLower() : "false");
                    textWriter.WriteElementString("IsPresent", Cur_Model.R_IsTest.ToString().ToLower());
                    textWriter.WriteElementString("FrictionResult", null);
                    textWriter.WriteElementString("FinalFrictionTorque", null);

                    if (Cur_Model.R_IsTest)
                    {
                        textWriter.WriteElementString("MeasureCount", "1");
                        textWriter.WriteElementString("FinalAngleX", (Cur_Model.RL_X ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleY", (Cur_Model.RL_Y ?? 0).ToString("F6"));
                        textWriter.WriteElementString("FinalAngleZ", (Cur_Model.RL_Z ?? 0).ToString("F6"));
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", cycleTime);
                    }
                    else
                    {
                        textWriter.WriteElementString("MeasureCount", null);
                        textWriter.WriteElementString("FinalAngleX", null);
                        textWriter.WriteElementString("FinalAngleY", null);
                        textWriter.WriteElementString("FinalAngleZ", null);
                        textWriter.WriteElementString("ShapeType", null);
                        textWriter.WriteElementString("CmdPositionTargetWheelAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetSensor", null);
                        textWriter.WriteElementString("CmdPositionTargetCarAxis", null);
                        textWriter.WriteElementString("CmdPositionTargetHeight", null);
                        textWriter.WriteElementString("CmdPositionTargetAngle", null);
                        textWriter.WriteElementString("CmdTargetShapeHeight", null);
                        textWriter.WriteElementString("CmdTargetEntrax", null);
                        textWriter.WriteElementString("FinalCycleTime", null);
                    }

                    textWriter.WriteEndElement();

                    textWriter.WriteEndElement();

                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("PEVProcessType");
                    textWriter.WriteElementString("VerdictOK", "true");
                    textWriter.WriteEndElement();

                    textWriter.WriteEndElement();

                    textWriter.WriteEndDocument();
                }
                _main.AddLogMessage($"[Main] XML data saved to {sPath}");
            }
            catch (Exception ex)
            {
                _main.AddLogMessage($"[Main] XML Save Error: {ex.Message}");
            }
            finally
            {
                sw.Stop();
                _main.AddLogMessage($"[Main] Data Save Time : {sw.ElapsedMilliseconds}ms");
                SetState(TS.STEP_MAIN_TICKET_PRINT);
            }
        }

        private void _DoMainTicketPrint()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescPrintTicket");

                Result resultData = CreateResultInfo();
                bool isOk = resultData.FC_IsOk && resultData.FR_IsOk && resultData.RR_IsOk;

                var adasData = new Zebra420T.ADASString
                {
                    Identification = resultData.PJI,
                    Description = resultData.Model,
                    Results = isOk ? "OK" : "NG",
                    BankNumber = "ADAS NR",
                    Data = resultData.StartTime.ToString("yyyy/MM/dd"),
                    Time = resultData.StartTime.ToString("HH:mm:ss"),
                    Duration = (resultData.EndTime - resultData.StartTime).TotalSeconds.ToString() + "    segun"
                };

                _main.Invoke(new Action(() =>
                {
                    using (var printForm = new Zebra420T.ZebraForm(adasData))
                    {
                        printForm.ShowDialog();
                    }
                }));

                SetState(TS.STEP_MAIN_GRET_COMM);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorPrintingTicket", "Error", ex.Message);
            }
        }

        private void _DoMainGRETComm()
        {
            try
            {
                SetState(TS.STEP_MAIN_WAIT_GO_OUT);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDuringGRETCommunication", "Error", ex.Message);
            }
        }

        private void _DoMainWaitGoOut()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescMoveOut");
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    Thread.Sleep(100);
                }

                SetState(TS.STEP_MAIN_CYCLE_FINISH);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorWaitingForGoOut", "Error", ex.Message);
            }
        }

        private void _DoMainFinishCycle()
        {
            try
            {
                _main.m_frmParent.User_Monitor.UpdateStepDescription("StepDescFinishCycle");
                Thread.Sleep(100);
                SetState(TS.STEP_MAIN_WAIT);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFinishingCycle", "Error", ex.Message);
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