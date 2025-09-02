using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class ADASProcess
    {
        private Frm_Main mainForm;
        private Frm_Config config;
        private VEPBenchClient vepClient;
        private VEPBenchStatusZone statusZone;
        private VEPBenchSynchroZone synchroZone;
        private int _currentStep = 0;
        private List<ADASProcessStep> _processSteps;
        private List<ADASProcessStep> _frontCameraSteps;
        private List<ADASProcessStep> _rightRearRadarSteps;
        private List<ADASProcessStep> _leftRearRadarSteps;
        private List<ADASProcessStep> _currentSensorSteps;
        private int _currentSensorStepIndex;
        private bool _isRunning = false;
        private Thread _processThread;
        private DataRow selectedModel;
        private string modelName;
        private string barcodeValue;

        // 각도 측정값과 허용 범위 저장 변수
        private double _frontCameraAngle1; // Roll
        private double _frontCameraAngle2; // Azimuth
        private double _frontCameraAngle3; // Elevation
        private double _rightRearRadarAngle; // RH Rear Corner Radar
        private double _leftRearRadarAngle; // LH Rear Corner Radar
        private const double _rollThreshold = 1.0; // Roll 허용 범위
        private const double _azimuthThreshold = 1.0; // Azimuth 허용 범위
        private const double _elevationThreshold = 1.0; // Elevation 허용 범위

        private bool[] isTest = new bool[3]; // 각 센서별 테스트 계획 여부

        public event EventHandler<ADASProcessEventArgs> OnProcessStepChanged;

        // ADAS 프로세스 상태 타입
        public enum ProcessStateType
        {
            Info,
            Progress,
            Success,
            Error,
            Warning
        }

        // ADAS 프로세스 단계 정의
        public class ADASProcessStep
        {
            public int StepId { get; }
            public string Description { get; }
            public string SynchroState { get; }
            private readonly Func<bool> _action;

            public ADASProcessStep(int stepId, string description, string synchroState, Func<bool> action)
            {
                StepId = stepId;
                Description = description;
                SynchroState = synchroState;
                _action = action;
            }

            public bool Execute()
            {
                return _action();
            }
        }

        // ADAS 프로세스 이벤트
        public class ADASProcessEventArgs : EventArgs
        {
            public string Message { get; }
            public ProcessStateType StateType { get; }
            public ADASProcessStep Step { get; }
            public DateTime Timestamp { get; }

            public ADASProcessEventArgs(string message, ProcessStateType stateType, ADASProcessStep step = null)
            {
                Message = message;
                StateType = stateType;
                Step = step;
                Timestamp = DateTime.Now;
            }
        }

        public ADASProcess() { }

        public ADASProcess(VEPBenchClient vepBenchClient, Frm_Config configForm, Frm_Main mainForm)
        {
            this.config = configForm;
            this.mainForm = mainForm;
            vepClient = vepBenchClient ?? throw new ArgumentNullException(nameof(vepBenchClient));
            InitializeProcessSteps();
        }

        // 프로세스 단계 초기화
        public void InitializeProcessSteps()
        {
            _processSteps = new List<ADASProcessStep>();

            // 1. 홈 포지션/초기화
            _processSteps.Add(new ADASProcessStep(
                1, "홈 포지션 및 초기화", "초기화", () => {
                    try
                    {
                        var statusZone = vepClient.ReadStatusZone();
                        statusZone.StartCycle = 0;
                        statusZone.VepStatus = VEPBenchStatusZone.VepStatus_Undefined;
                        vepClient.WriteStatusZone(statusZone);

                        var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                        // SynchroZone 초기화
                        for (int i = 0; i < synchroZone.Size; i++)
                            synchroZone.SetValue(i, 0);

                        vepClient.WriteSynchroZone(synchroZone);

                        NotifyProcessState("홈 포지션 및 초기화 완료", ProcessStateType.Info);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        NotifyProcessState($"초기화 실패: {ex.Message}", ProcessStateType.Error);
                        return false;
                    }
                }));

            // 2. 신호등(입구) 제어 (시뮬레이션)
            _processSteps.Add(new ADASProcessStep(
                2, "입구 신호등 초록색", "신호등", () => {
                    NotifyProcessState("입구 신호등 초록색", ProcessStateType.Info);
                    return true;
                }));

            // 3. 차량 감지 및 바코드 스캔 (시뮬레이션)
            _processSteps.Add(new ADASProcessStep(
                3, "차량 감지 및 바코드 스캔", "바코드", () => {

                    if (mainForm != null)
                    {
                        Random random = new Random();
                        DataTable modelData = config.GetModelData();

                        int randomIndex = random.Next(modelData.Rows.Count);
                        selectedModel = modelData.Rows[randomIndex];
                        modelName = selectedModel["Name"].ToString();
                        barcodeValue = mainForm.SelectedBarcode;

                        if (string.IsNullOrEmpty(barcodeValue))
                        {
                            MessageBox.Show("메인 화면의 테스트 목록에서 항목을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (selectedModel == null)
                        {
                            MessageBox.Show($"선택된 모델 '{modelName}'에 대한 설정 정보를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    NotifyProcessState("차량 감지 및 바코드 스캔 완료", ProcessStateType.Info);
                    return true;
                }));

            // 4. 사이클 시작
            _processSteps.Add(new ADASProcessStep(
                4, "사이클 시작", "StartCycle", () => {
                    try
                    {
                        var statusZone = vepClient.ReadStatusZone();
                        statusZone.StartCycle = 1;
                        vepClient.WriteStatusZone(statusZone);
                        NotifyProcessState("사이클 시작", ProcessStateType.Info);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        NotifyProcessState($"사이클 시작 실패: {ex.Message}", ProcessStateType.Error);
                        return false;
                    }
                }));

            // 5. PJI(PII) 요청
            _processSteps.Add(new ADASProcessStep(
                5, "PJI 요청", "PJI", () => {
                    try
                    {
                        var statusZone = vepClient.ReadStatusZone();
                        statusZone.StartCycle = 0;
                        statusZone.VepStatus = VEPBenchStatusZone.VepStatus_Working;
                        vepClient.WriteStatusZone(statusZone);

                        var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                        if (isTest[0] == true)
                        {
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 1);
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 0);
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 0);
                        }
                        else if (isTest[1] == true)
                        {
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 0);
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 1);
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 0);
                        }
                        else if (isTest[2] == true)
                        {
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 0);
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 0);
                            synchroZone.SetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 1);
                        }

                        vepClient.WriteSynchroZone(synchroZone);

                        NotifyProcessState("PJI 요청 및 타겟 선택", ProcessStateType.Info);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        NotifyProcessState($"PJI 요청 실패: {ex.Message}", ProcessStateType.Error);
                        return false;
                    }
                }));

            // 6. VEP 상태 확인
            _processSteps.Add(new ADASProcessStep(
                6, "VEP 상태 확인", "VEPStatus", () => {
                    var statusZone = vepClient.ReadStatusZone();

                    if (statusZone.VepStatus == VEPBenchStatusZone.VepStatus_Working)
                    {
                        NotifyProcessState("VEP 작업 중", ProcessStateType.Info);
                        return true;
                    }

                    NotifyProcessState("VEP 미작동", ProcessStateType.Warning);
                    return false;
                }));

            // 7. 타겟(FrontCamera/RearRadar) 선택 및 테스트 포지션 이동
            _processSteps.Add(new ADASProcessStep(
                7, "타겟 선택 및 테스트 포지션 이동", "타겟", () => {
                    var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));
                    string target = "알 수 없음";

                    if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1)
                        target = "전방 카메라";
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1)
                        target = "우측 후방 레이더";
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1)
                        target = "좌측 후방 레이더";

                    NotifyProcessState($"{target} 타겟 선택 및 테스트 포지션 이동", ProcessStateType.Info);
                    return true;
                }));

            // 8. 센서별 보정/측정 단계
            _processSteps.Add(new ADASProcessStep(
                8, "센서별 보정/측정 단계", "SensorType", () =>
                {
                    int sensorType = DetermineSensorType();
                    var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                    switch (sensorType)
                    {
                        case VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX:
                            synchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_FRONT_CAMERA_INDEX, 1);
                            vepClient.WriteSynchroZone(synchroZone);
                            NotifyProcessState("전방 카메라 타겟(VEP Synchro 4 = 1) 설정", ProcessStateType.Info);
                            InitializeFrontCameraSteps();
                            _currentSensorSteps = _frontCameraSteps;
                            break;
                        case VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX:
                            synchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_RIGHT_RADAR_INDEX, 1);
                            vepClient.WriteSynchroZone(synchroZone);
                            NotifyProcessState("우측 후방 레이더 타겟(VEP Synchro 52 = 1) 설정", ProcessStateType.Info);
                            InitializeRightRearRadarSteps();
                            _currentSensorSteps = _rightRearRadarSteps;
                            break;
                        case VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX:
                            synchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_LEFT_RADAR_INDEX, 1);
                            vepClient.WriteSynchroZone(synchroZone);
                            NotifyProcessState("좌측 후방 레이더 타겟(VEP Synchro 54 = 1) 설정", ProcessStateType.Info);
                            InitializeLeftRearRadarSteps();
                            _currentSensorSteps = _leftRearRadarSteps;
                            break;
                        default:
                            synchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_FRONT_CAMERA_INDEX, 1);
                            vepClient.WriteSynchroZone(synchroZone);
                            NotifyProcessState("기본값(전방 카메라) 타겟(VEP Synchro 4 = 1) 설정", ProcessStateType.Info);
                            InitializeFrontCameraSteps();
                            _currentSensorSteps = _frontCameraSteps;
                            break;
                    }

                    _currentSensorStepIndex = 0;

                    return true;
                }));

            // 9. 결과 판정 및 완료
            _processSteps.Add(new ADASProcessStep(
                99, "테스트 결과 판정 및 완료", "결과", () => {
                    var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));
                    int result = synchroZone.GetValue(1);

                    if (result == 20)
                    {
                        NotifyProcessState("테스트 완료 (OK)", ProcessStateType.Success);
                        return true;
                    }
                    else if (result == 21)
                    {
                        NotifyProcessState("테스트 실패 (NOK)", ProcessStateType.Error);
                        return false;
                    }
                    else
                    {
                        NotifyProcessState($"테스트 결과 미정 (Synchro 1 = {result})", ProcessStateType.Warning);
                        return false;
                    }
                }));
        }

        private int DetermineSensorType()
        {
            try
            {
                var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                if (synchroZone.GetValue(3) == 1)
                    return 3;

                if (synchroZone.GetValue(51) == 1)
                    return 51;

                if (synchroZone.GetValue(53) == 1)
                    return 53;

                return 3;
            }
            catch (Exception ex)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("SensorTypeDetectionFail", ex.Message),
                    ProcessStateType.Error);

                return 3; // 기본값으로 Front Camera 사용
            }
        }

        public void InitializeFrontCameraSteps()
        {
            _frontCameraSteps = new List<ADASProcessStep>();

            _frontCameraSteps.Add(new ADASProcessStep(
                213,
                "전방 카메라 - 초기화",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX} = 1",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1));

            _frontCameraSteps.Add(new ADASProcessStep(
                214,
                "전방 카메라 - 준비 완료",
                $"Synchro {VEPBenchSynchroZone.SYNC_COMMAND_FRONT_CAMERA_INDEX} = 1",
                () => ReadSynchroValue(VEPBenchSynchroZone.SYNC_COMMAND_FRONT_CAMERA_INDEX) == 1));

            _frontCameraSteps.Add(new ADASProcessStep(
                213,
                "전방 카메라 - 보정 완료",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX} = 20",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 20));

            _frontCameraSteps.Add(new ADASProcessStep(
                213,
                "전방 카메라 - 보정 실패",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX} = 21",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 21));

            _frontCameraSteps.Add(new ADASProcessStep(
                320,
                "Roll 각도 측정",
                $"Synchro {VEPBenchSynchroZone.FRONT_CAMERA_ANGLE1_INDEX} = FrontCameraAngle",
                () => {
                    _frontCameraAngle1 = ReadSynchroValue(VEPBenchSynchroZone.FRONT_CAMERA_ANGLE1_INDEX) / 100.0;
                    return true;
                }));

            _frontCameraSteps.Add(new ADASProcessStep(
                321,
                "Azimuth 각도 측정",
                $"Synchro {VEPBenchSynchroZone.FRONT_CAMERA_ANGLE2_INDEX} = FrontCameraAngle",
                () => {
                    _frontCameraAngle2 = ReadSynchroValue(VEPBenchSynchroZone.FRONT_CAMERA_ANGLE2_INDEX) / 100.0;
                    return true;
                }));

            _frontCameraSteps.Add(new ADASProcessStep(
                322,
                "Elevation 각도 측정",
                $"Synchro {VEPBenchSynchroZone.FRONT_CAMERA_ANGLE3_INDEX} = FrontCameraAngle",
                () => {
                    _frontCameraAngle3 = ReadSynchroValue(VEPBenchSynchroZone.FRONT_CAMERA_ANGLE3_INDEX) / 100.0;
                    return true;
                }));

            _frontCameraSteps.Add(new ADASProcessStep(
                299,
                "추가 시도 확인 1",
                "Synchro 89 = 1",
                () =>
                {
                    NotifyProcessState("보정 각도 검증 완료", ProcessStateType.Success);

                    return ReadSynchroValue(VEPBenchSynchroZone.TRY_FRONT_CAMERA_INDEX) == 1;
                }));

            _frontCameraSteps.Add(new ADASProcessStep(
                299,
                "추가 시도 확인 2",
                "Synchro 89 = 2",
                () => {
                    _currentSensorStepIndex = 0;
                    NotifyProcessState("보정 재시도 요청됨", ProcessStateType.Info);

                    return ReadSynchroValue(VEPBenchSynchroZone.TRY_FRONT_CAMERA_INDEX) == 2;
                }));
        }

        public void InitializeRightRearRadarSteps()
        {
            _rightRearRadarSteps = new List<ADASProcessStep>();

            _rightRearRadarSteps.Add(new ADASProcessStep(
                261,
                "우측 후방 레이더 - 초기화",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX} = 1",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1));

            _rightRearRadarSteps.Add(new ADASProcessStep(
                262,
                "우측 후방 레이더 - 보정 위치 확인",
                $"Synchro {VEPBenchSynchroZone.SYNC_COMMAND_REAR_RIGHT_RADAR_INDEX} = 1",
                () => ReadSynchroValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_RIGHT_RADAR_INDEX) == 1));

            _rightRearRadarSteps.Add(new ADASProcessStep(
                261,
                "우측 후방 레이더 - 보정 완료",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX} = 20",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 20));

            _rightRearRadarSteps.Add(new ADASProcessStep(
                261,
                "우측 후방 레이더 - 보정 실패",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX} = 21",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 21));

            _rightRearRadarSteps.Add(new ADASProcessStep(
                293,
                "추가 시도 확인 1",
                "Synchro 83 = 1",
                () =>
                {
                    NotifyProcessState("보정 각도 검증 완료", ProcessStateType.Success);

                    return ReadSynchroValue(VEPBenchSynchroZone.TRY_REAR_RIGHT_RADAR_INDEX) == 1;
                }));

            _rightRearRadarSteps.Add(new ADASProcessStep(
                293,
                "추가 시도 확인 2",
                "Synchro 83 = 2",
                () => {
                    _currentSensorStepIndex = 0;
                    NotifyProcessState("보정 재시도 요청됨", ProcessStateType.Info);

                    return ReadSynchroValue(VEPBenchSynchroZone.TRY_REAR_RIGHT_RADAR_INDEX) == 2;
                }));

            _rightRearRadarSteps.Add(new ADASProcessStep(
                325,
                "우측 후방 레이더 각도 측정",
                $"Synchro {VEPBenchSynchroZone.REAR_RIGHT_RADAR_ANGLE_INDEX} = RearRightRadarAngle",
                () => {
                    _rightRearRadarAngle = ReadSynchroValue(VEPBenchSynchroZone.REAR_RIGHT_RADAR_ANGLE_INDEX) / 100.0;
                    return true;
                }));
        }

        public void InitializeLeftRearRadarSteps()
        {
            _leftRearRadarSteps = new List<ADASProcessStep>();

            _leftRearRadarSteps.Add(new ADASProcessStep(
                263,
                "좌측 후방 레이더 - 초기화",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX} = 1",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1));

            _leftRearRadarSteps.Add(new ADASProcessStep(
                264,
                "좌측 후방 레이더 - 보정 위치 확인",
                $"Synchro {VEPBenchSynchroZone.SYNC_COMMAND_REAR_LEFT_RADAR_INDEX} = 1",
                () => ReadSynchroValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_LEFT_RADAR_INDEX) == 1));

            _leftRearRadarSteps.Add(new ADASProcessStep(
                263,
                "좌측 후방 레이더 - 보정 완료",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX} = 20",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 20));

            _leftRearRadarSteps.Add(new ADASProcessStep(
                263,
                "좌측 후방 레이더 - 보정 실패",
                $"Synchro {VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX} = 21",
                () => ReadSynchroValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 21));

            _leftRearRadarSteps.Add(new ADASProcessStep(
                292,
                "추가 시도 확인 1",
                "Synchro 82 = 1",
                () =>
                {
                    NotifyProcessState("보정 각도 검증 완료", ProcessStateType.Success);

                    return ReadSynchroValue(VEPBenchSynchroZone.TRY_REAR_LEFT_RADAR_INDEX) == 1;
                }));

            _leftRearRadarSteps.Add(new ADASProcessStep(
                292,
                "추가 시도 확인 2",
                "Synchro 82 = 2",
                () => {
                    _currentSensorStepIndex = 0;
                    NotifyProcessState("보정 재시도 요청됨", ProcessStateType.Info);

                    return ReadSynchroValue(VEPBenchSynchroZone.TRY_REAR_LEFT_RADAR_INDEX) == 2;
                }));

            _leftRearRadarSteps.Add(new ADASProcessStep(
                326,
                "좌측 후방 레이더 각도 측정",
                $"Synchro {VEPBenchSynchroZone.REAR_LEFT_RADAR_ANGLE_INDEX} = RearLeftRadarAngle",
                () => {
                    _leftRearRadarAngle = ReadSynchroValue(VEPBenchSynchroZone.REAR_LEFT_RADAR_ANGLE_INDEX) / 100.0;
                    return true;
                }));
        }

        // Synchro 값 설정
        private int ReadSynchroValue(int syncNumber)
        {
            try
            {
                if (vepClient == null)
                {
                    NotifyProcessState(LanguageResource.GetMessage("VEPBenchNotInitialized"), ProcessStateType.Error);

                    return -1;
                }

                var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                if (syncNumber == VEPBenchSynchroZone.FRONT_CAMERA_ANGLE1_INDEX ||
                    syncNumber == VEPBenchSynchroZone.FRONT_CAMERA_ANGLE2_INDEX ||
                    syncNumber == VEPBenchSynchroZone.FRONT_CAMERA_ANGLE3_INDEX)
                {
                    double angle = 0;
                    string angleName = "";

                    switch (syncNumber)
                    {
                        case VEPBenchSynchroZone.FRONT_CAMERA_ANGLE1_INDEX: // Roll
                            angle = synchroZone.FrontCameraAngle1;
                            _frontCameraAngle1 = angle;
                            angleName = "Roll";
                            break;

                        case VEPBenchSynchroZone.FRONT_CAMERA_ANGLE2_INDEX: // Azimuth
                            angle = synchroZone.FrontCameraAngle2;
                            _frontCameraAngle2 = angle;
                            angleName = "Azimuth";
                            break;

                        case VEPBenchSynchroZone.FRONT_CAMERA_ANGLE3_INDEX: // Elevation
                            angle = synchroZone.FrontCameraAngle3;
                            _frontCameraAngle3 = angle;
                            angleName = "Elevation";
                            break;
                    }

                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("AngleReadComplete", angleName, angle),
                        ProcessStateType.Info);

                    return (int)(angle * 100);
                }
                // 우측 후방 레이더 각도 처리
                else if (syncNumber == VEPBenchSynchroZone.REAR_RIGHT_RADAR_ANGLE_INDEX)
                {
                    double angle = synchroZone.RearRightRadarAngle;
                    _rightRearRadarAngle = angle;

                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("AngleReadComplete", "우측 후방 레이더", angle),
                        ProcessStateType.Info);

                    return (int)(angle * 100);
                }
                // 좌측 후방 레이더 각도 처리
                else if (syncNumber == VEPBenchSynchroZone.REAR_LEFT_RADAR_ANGLE_INDEX)
                {
                    double angle = synchroZone.RearLeftRadarAngle;
                    _leftRearRadarAngle = angle;

                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("AngleReadComplete", "좌측 후방 레이더", angle),
                        ProcessStateType.Info);

                    return (int)(angle * 100);
                }

                // 일반 Synchro 값 처리
                if (syncNumber < synchroZone.Size)
                {
                    ushort value = synchroZone.GetValue(syncNumber);

                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("SynchroReadComplete", syncNumber, value),
                        ProcessStateType.Info);

                    return value;
                }

                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("SynchroReadFail", syncNumber, "Index out of range"),
                    ProcessStateType.Warning);

                return -1;
            }
            catch (Exception ex)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("SynchroReadFail", ex.Message),
                    ProcessStateType.Error);

                return -1;
            }
        }

        public bool Start(string ipAddress, int port)
        {
            try
            {
                if (_isRunning)
                    return false;

                if (vepClient == null)
                {
                    vepClient = new VEPBenchClient(ipAddress, port);
                }

                vepClient.Connect();

                _currentStep = 0;
                _isRunning = true;

                NotifyProcessState(LanguageResource.GetMessage("ProcessStart"), ProcessStateType.Info);

                _processThread = new Thread(ProcessThread);
                _processThread.IsBackground = true;
                _processThread.Start();

                return true;
            }
            catch (Exception ex)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("ProcessStartFail", ex.Message),
                    ProcessStateType.Error);

                return false;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;

                if (_processThread != null && _processThread.IsAlive)
                {
                    try
                    {
                        _processThread.Join(1000);
                    }
                    catch { }
                }

                vepClient.DisConnect();

                NotifyProcessState(LanguageResource.GetMessage("ProcessStop"), ProcessStateType.Info);
            }
        }

        // 프로세스 실행
        private void ProcessThread()
        {
            try
            {
                while (_isRunning)
                {
                    if (_currentStep < _processSteps.Count)
                    {
                        if (_processSteps[_currentStep].StepId == 8 && _currentSensorSteps != null)
                        {
                            while (_isRunning && _currentSensorStepIndex < _currentSensorSteps.Count)
                            {
                                ProcessSensorStep();

                                if (!_isRunning) break;
                                Thread.Sleep(1000);
                            }

                            _currentStep++;
                        }
                        else
                        {
                            ProcessNextStep();
                        }
                    }
                    else
                    {
                        break;
                    }

                    if (!_isRunning)
                        break;

                    Thread.Sleep(1000);
                }

                if (_currentStep >= _processSteps.Count)
                {
                    NotifyProcessState(LanguageResource.GetMessage("ProcessComplete"), ProcessStateType.Success);
                }
            }
            catch (Exception ex)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("ProcessError", ex.Message),
                    ProcessStateType.Error);
            }
            finally
            {
                _isRunning = false;
            }
        }

        private void ProcessSensorStep()
        {
            if (!_isRunning || _currentSensorSteps == null || _currentSensorStepIndex >= _currentSensorSteps.Count)
                return;

            ADASProcessStep currentStep = _currentSensorSteps[_currentSensorStepIndex];

            NotifyProcessState(
                LanguageResource.GetFormattedMessage("StepProgress", _currentSensorStepIndex + 1, _currentSensorSteps.Count, currentStep.Description),
                ProcessStateType.Progress,
                currentStep);

            try
            {
                bool success = currentStep.Execute();

                if (!success)
                {
                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("StepFail", _currentSensorStepIndex + 1, currentStep.Description),
                        ProcessStateType.Error,
                        currentStep);

                    _isRunning = false;
                }
                else
                {
                    _currentSensorStepIndex++;
                }
            }
            catch (Exception ex)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("StepError", _currentSensorStepIndex + 1, ex.Message),
                    ProcessStateType.Error,
                    currentStep);

                _isRunning = false;
            }
        }

        private void ProcessNextStep()
        {
            if (!_isRunning || _currentStep >= _processSteps.Count)
                return;

            ADASProcessStep currentStep = _processSteps[_currentStep];

            NotifyProcessState(
                LanguageResource.GetFormattedMessage("StepProgress", _currentStep + 1, _processSteps.Count, currentStep.Description),
                ProcessStateType.Progress,
                currentStep);

            try
            {
                bool success = currentStep.Execute();

                if (!success)
                {
                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("StepFail", _currentStep + 1, currentStep.Description),
                        ProcessStateType.Error,
                        currentStep);

                    _isRunning = false;
                }
                else
                {
                    _currentStep++;
                }
            }
            catch (Exception ex)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("StepError", _currentStep + 1, ex.Message),
                    ProcessStateType.Error,
                    currentStep);

                _isRunning = false;
            }
        }

        // 현재 진행 중인 단계 정보 반환
        public ADASProcessStep GetCurrentStep()
        {
            return _currentStep < _processSteps.Count ? _processSteps[_currentStep] : null;
        }

        // 프로세스 상태 변경 알림
        private void NotifyProcessState(string message, ProcessStateType stateType, ADASProcessStep step = null)
        {
            OnProcessStepChanged?.Invoke(this, new ADASProcessEventArgs(message, stateType, step));
        }
    }
}