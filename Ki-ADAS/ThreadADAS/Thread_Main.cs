using Ki_ADAS.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_ADAS
{
    public class Thread_Main
    {
        private Thread _testThread = null;

        private Thread_FRCam _Thread_FRCam = null;
        private Thread_FRCam _Thread_FrontRadar = null;
        private Thread_FRCam _Thread_RearRadar = null;

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

        public Thread_Main()
        {
            
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
                    }
                    else if (m_nState == TS.STEP_MAIN_BARCODE_WAIT)
                    {
                        _DoMainBarcodeWait();
                    }
                    else if (m_nState == TS.STEP_MAIN_CHECK_DETECTION_SENSOR)
                    {
                        _DoMainCheck_Detect();
                    }
                    else if (m_nState == TS.STEP_MAIN_PRESS_START_BUTTON)
                    {
                        _DoMainPressCycle();
                    }
                    else if (m_nState == TS.STEP_MAIN_CYCLE_FINISH) 
                    {
                        _DoMainFinishCycle();
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
                UI_Update_Message("_DoMainInitial");
                
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
                UI_Update_Message("_DoMainBarcode");
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    if (m_bBarcode)
                    {
                        break;
                    }
                    Thread.Sleep(10);
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
                UI_Update_Message("_DoMainCheck_Detect");
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    //차량 진입 시
                    //if (PLC.DI.FDetect &&  PLC.DI.RDetect) break;
                    Thread.Sleep(10);
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
                UI_Update_Message("_DoMainPressCycle");
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    // 시작버튼 클릭시
                    // if (PLC.DI.PressStart) break;
                    Thread.Sleep(10);
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
                UI_Update_Message("_DoMainCenteringOn");
                // 루프 들어가기전에 센터링 신호 전송
                // PLC.DO.CenterOn = true ;
                while (true)
                {
                    if (CheckLoopExit())
                        break;

                    // 시작버튼 클릭시
                    //if (PLC.DI.CenteringOn) break;
                    Thread.Sleep(10);
                }
                SetState(TS.STEP_MAIN_CYCLE_FINISH);
            }
            catch (Exception ex)
            {

            }
        }
        private void _DoMainFinishCycle()
        {
            try
            {
                UI_Update_Message("_DoMainFinishCycle");
                // 사이클 종료시 처리 하는 함수
                Thread.Sleep(10);
                SetState(TS.STEP_MAIN_WAIT);
            }
            catch (Exception ex)
            {

            }
        }



    }
}
