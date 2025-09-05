using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS
{
    public static class Def
    {
        public const int FOM_IDX_MAIN = 0;
        public const int FOM_IDX_CONFIG = 1;
        public const int FOM_IDX_CALIBRATION = 2;
        public const int FOM_IDX_MANUAL = 3;
        public const int FOM_IDX_RESULT = 4;
        public const int FOM_IDX_VEP = 5;
    }
    public static class ThreadStep
    {

        public const int STEP_MAIN_WAIT = 0;
        public const int STEP_MAIN_BARCODE_ENTER = 1;
        public const int STEP_MAIN_CHECK_DETECTION_SENSOR = 2;
        public const int STEP_MAIN_PRESS_START_BUTTON = 3; //
        public const int STEP_MAIN_CENTERING_ON = 4; //
        public const int STEP_MAIN_CHECK_OPTION = 5;
        
        public const int STEP_MAIN_PEV_START_CYCLE = 110;
        public const int STEP_MAIN_PEV_SEND_PJI = 111;
        public const int STEP_MAIN_PEV_READY = 112;
        
        public const int STEP_MAIN_START_EACH_THREAD = 120;  // 각 스레드 동작 시킴
        public const int STEP_MAIN_WAIT_TEST_COMPLEATE = 121; // 완료 신호를 기다림
        public const int STEP_MAIN_CENTERING_HOME = 122; //
        public const int STEP_MAIN_WAIT_TARGET_HOME = 123; // 완료 신호를 기다림
        public const int STEP_MAIN_DATA_SAVE = 124; // 데이터 저장
        public const int STEP_MAIN_TICKET_PRINT = 125; // 데이터 저장
        public const int STEP_MAIN_GRET_COMM = 126; // GRET 전송
        public const int STEP_MAIN_WAIT_GO_OUT = 127; // 디텍션 센서 빠지는지 확인
        public const int STEP_MAIN_CYCLE_FINISH = 128; // cycle 끝내고 초기화


        public const int STEP_CAM_SEND_INFO = 200;
        public const int STEP_CAM_CHECK_OPTION = 201;
        public const int STEP_CAM_TARGET_MOVE = 202;
        public const int STEP_CAM_TARGET_MOVE_COMPLATE = 203; // SEND Sync4 = 1
        public const int STEP_CAM_WAIT_SYNC3 = 204; // Sync3 =20 or 21
        public const int STEP_CAM_READ_ANGLE = 205; // Sync 110 111 112 
        public const int STEP_CAM_TARGET_HOME = 206; // Sync3 =110 111 112 
        public const int STEP_CAM_FINISH = 207; // Sync3 =110 111 112 

        public const int STEP_FRADAR_SEND_INFO = 300;
        public const int STEP_FRADAR_TARGET_MOVE_COMPLATE = 301; // SEND Sync 56 = 1
        public const int STEP_FRADAR_WAIT_SYNC3 = 203; // Sync50 =20 or 21
        public const int STEP_FRADAR_READ_ANGLE = 204; // Sync 110 111 112 
        public const int STEP_FRADAR_FINISH = 205; // Sync3 =110 111 112 

    }

}


