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
    public static class TS
    {
        // Main Thread
        public const int STEP_MAIN_WAIT = 0;
        public const int STEP_MAIN_BARCODE_WAIT = 1;
        public const int STEP_MAIN_CHECK_DETECTION_SENSOR = 2;
        public const int STEP_MAIN_PRESS_START_BUTTON = 3;
        public const int STEP_MAIN_CENTERING_ON = 4;
        public const int STEP_MAIN_CHECK_OPTION = 5;
        
        public const int STEP_MAIN_PEV_START_CYCLE = 110;
        public const int STEP_MAIN_PEV_SEND_PJI = 111;
        public const int STEP_MAIN_PEV_READY = 112;

        public const int STEP_MAIN_START_EACH_THREAD = 120;  // 각 스레드 동작 시킴
        public const int STEP_MAIN_WAIT_TEST_COMPLETE = 121; // 완료 신호를 기다림
        public const int STEP_MAIN_CENTERING_HOME = 122; //
        public const int STEP_MAIN_WAIT_TARGET_HOME = 123; // 완료 신호를 기다림
        public const int STEP_MAIN_DATA_SAVE = 124; // 데이터 저장
        public const int STEP_MAIN_TICKET_PRINT = 125; // 데이터 저장
        public const int STEP_MAIN_GRET_COMM = 126; // GRET 전송
        public const int STEP_MAIN_WAIT_GO_OUT = 127; // 디텍션 센서 빠지는지 확인
        public const int STEP_MAIN_CYCLE_FINISH = 128; // cycle 끝내고 초기화

        // Front Camera
        public const int STEP_CAM_SEND_INFO = 200; // VEP 서버로 Model 데이터 전송
        public const int STEP_CAM_CHECK_OPTION = 201; // RECV Sync 3 = 1
        public const int STEP_CAM_TARGET_MOVE = 202;
        public const int STEP_CAM_TARGET_MOVE_COMPLETE = 203; // SEND Sync4 = 1
        public const int STEP_CAM_WAIT_SYNC3 = 204; // Sync3 =20 or 21
        public const int STEP_CAM_READ_ANGLE = 205; // Sync 110 111 112 
        public const int STEP_CAM_TARGET_HOME = 206;
        public const int STEP_CAM_FINISH = 207;

        // Front Radar
        public const int STEP_FRADAR_SEND_INFO = 300;
        public const int STEP_FRADAR_CHECK_OPTION = 301; // RECV Sync 55 = 1 or Sync 57 = 1
        public const int STEP_FRADAR_TARGET_MOVE = 302;
        public const int STEP_FRADAR_TARGET_MOVE_COMPLETE = 303; // SEND Sync 56 = 1 or Sync 58 = 1
        public const int STEP_FRADAR_WAIT_SYNC = 304; // Sync55 = 20 or 21 | Sync57 = 20 or 21
        public const int STEP_FRADAR_READ_ANGLE = 305; // Sync 117 or Sync 118
        public const int STEP_FRADAR_TARGET_HOME = 306;
        public const int STEP_FRADAR_FINISH = 307;

        // Rear Radar
        public const int STEP_RRADAR_SEND_INFO = 400;
        public const int STEP_RRADAR_CHECK_OPTION = 401; // RECV Sync 51 = 1 or Sync 53 = 1
        public const int STEP_RRADAR_TARGET_MOVE = 402;
        public const int STEP_RRADAR_TARGET_MOVE_COMPLETE = 403; // SEND Sync 52 = 1 or Sync 54 = 1
        public const int STEP_RRADAR_WAIT_SYNC = 404; // Sync51 = 20 or 21 | Sync53 = 20 or 21
        public const int STEP_RRADAR_READ_ANGLE = 405; // Sync 115 or Sync 116
        public const int STEP_RRADAR_TARGET_HOME = 406;
        public const int STEP_RRADAR_FINISH = 407;
    }
}


