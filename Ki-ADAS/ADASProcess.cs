using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Ki_ADAS.VEPBench;

namespace Ki_ADAS
{
    public class ADASProcess
    {
        private VEPBenchClient _vepBenchClient;
        private int _currentStep = 0;
        private List<ADASProcessStep> _processSteps;
        private bool _isRunning = false;
        private Thread _processThread;

        // 각도 측정값과 허용 범위 저장 변수
        private double _roll;
        private double _azimuth;
        private double _elevation;
        private const double _rollThreshold = 1.0; // Roll 허용 범위
        private const double _azimuthThreshold = 1.0; // Azimuth 허용 범위
        private const double _elevationThreshold = 1.0; // Elevation 허용 범위

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

        public ADASProcess(VEPBenchClient vepBenchClient)
        {
            _vepBenchClient = vepBenchClient ?? throw new ArgumentNullException(nameof(vepBenchClient));
            InitializeProcessSteps();
        }

        // 프로세스 단계 초기화
        private void InitializeProcessSteps()
        {
            _processSteps = new List<ADASProcessStep>
            {
                new ADASProcessStep(
                    213,
                    "카메라 캘리브레이션 요청",
                    "Synchro 3 = 1",
                    () => SetSynchroValue(3, 1)),

                new ADASProcessStep(
                    214,
                    "전면 카메라 타겟 위치 확인",
                    "Synchro 4 = 1",
                    () => SetSynchroValue(4, 1)),

                new ADASProcessStep(
                    213,
                    "타켓을 홈 위치로 이동",
                    "Synchro 3 = 20",
                    () => SetSynchroValue(3, 20)),

                new ADASProcessStep(
                    213,
                    "타켓을 홈 위치로 이동 2단계",
                    "Synchro 3 = 21",
                    () => SetSynchroValue(3, 21)),

                new ADASProcessStep(
                    320,
                    "각도 1 측정값 전송",
                    "Synchro 110 = 각도 1",
                    () => SetSynchroValue(110, GetMeasurementAngle(1))),

                new ADASProcessStep(
                    321,
                    "각도 2 측정값 전송",
                    "Synchro 111 = 각도 2",
                    () => SetSynchroValue(111, GetMeasurementAngle(2))),

                new ADASProcessStep(
                    322,
                    "각도 3 측정값 전송",
                    "Synchro 112 = 각도 3",
                    () => SetSynchroValue(112, GetMeasurementAngle(3))),

                new ADASProcessStep(
                    299,
                    "Synchro 89 첫번째 시도",
                    "Synchro 89 = 1",
                    () => SetSynchroValue(89, 1)),

                new ADASProcessStep(
                    299,
                    "Synchro 89 두번째 시도",
                    "Synchro 89 = 2",
                    () => ValidateAnglesAndComplete())
            };
        }

        private bool ValidateAnglesAndComplete()
        {
            bool isValid = ValidateCameraAngles();

            if (!isValid)
            {
                NotifyProcessState("카메라 각도 측정값이 허용 범위를 벗어났습니다.", ProcessStateType.Error);
                _currentStep = 0;

                return false;
            }

            return SetSynchroValue(89, 2);
        }

        private bool ValidateCameraAngles()
        {
            if (Math.Abs(_roll) > _rollThreshold)
            {
                NotifyProcessState($"Roll 각도 검증 실패: {_roll}, 허용범위: ±{_rollThreshold}", ProcessStateType.Warning);
                return false;
            }

            if (Math.Abs(_azimuth) > _azimuthThreshold)
            {
                NotifyProcessState($"Azimuth 각도 검증 실패: {_azimuth}, 허용범위: ±{_azimuthThreshold}", ProcessStateType.Warning);
                return false;
            }

            if (Math.Abs(_elevation) > _elevationThreshold)
            {
                NotifyProcessState($"Elevation 각도 검증 실패: {_elevation}, 허용범위: ±{_elevationThreshold}", ProcessStateType.Warning);
                return false;
            }

            NotifyProcessState("전면 카메라 각도 검증 통과", ProcessStateType.Success);
            return true;
        }

        // 테스트를 위한 임의의 각도 생성
        private int GetMeasurementAngle(int angleType)
        {
            Random random = new Random();
            double angle = 0;

            switch (angleType)
            {
                case 1:
                    _roll = (random.NextDouble() * 2 - 1) * 1.5; // -1.5 ~ 1.5
                    angle = _roll;
                    break;

                case 2:
                    _azimuth = (random.NextDouble() * 2 - 1) * 1.5; // -1.5 ~ 1.5
                    angle = _azimuth;
                    break;

                case 3:
                    _elevation = (random.NextDouble() * 2 - 1) * 1.5; // -1.5 ~ 1.5
                    angle = _elevation;
                    break;
            }

            return random.Next(0, 100);
        }

        // Synchro 값 설정
        private bool SetSynchroValue(int syncNumber, int value)
        {
            try
            {
                var synchro = new VEPBenchSynchro();

                // 동기화 값에 따라 알맞은 위치에 설정
                switch (syncNumber)
                {
                    case 110:
                        synchro.Angle1 = value;
                        break;

                    case 111:
                        synchro.Angle2 = value;
                        break;

                    case 112:
                        synchro.Angle3 = value;
                        break;

                    default:
                        ushort[] data = new ushort[3];
                        data[syncNumber % 3] = (ushort)value;
                        _vepBenchClient.WriteStatusZone(data);

                        return true;
                }

                _vepBenchClient.WriteSynchroZone(synchro);
                NotifyProcessState($"Synchro {syncNumber} = {value} 설정 완료", ProcessStateType.Info);
                return true;
            }
            catch (Exception ex)
            {
                NotifyProcessState($"Synchro 값 설정 실패: {ex.Message}", ProcessStateType.Error);
                return false;
            }
        }

        public bool Start(string ipAddress, int port)
        {
            try
            {
                if (_isRunning)
                    return false;

                if (_vepBenchClient == null)
                {
                    _vepBenchClient = new VEPBenchClient(ipAddress, port);
                }

                _vepBenchClient.Connect();

                _currentStep = 0;
                _isRunning = true;

                NotifyProcessState("프로세스 시작", ProcessStateType.Info);

                _processThread = new Thread(ProcessThread);
                _processThread.IsBackground = true;
                _processThread.Start();

                return true;
            }
            catch (Exception ex)
            {
                NotifyProcessState($"프로세스 시작 실패: {ex.Message}", ProcessStateType.Error);
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

                _vepBenchClient.DisConnect();
                NotifyProcessState("프로세스 중지", ProcessStateType.Info);
            }
        }

        // 프로세스 실행
        private void ProcessThread()
        {
            try
            {
                while (_isRunning && _currentStep < _processSteps.Count)
                {
                    ProcessNextStep();

                    if (!_isRunning)
                        break;

                    Thread.Sleep(1000);
                }

                if (_currentStep >= _processSteps.Count)
                {
                    NotifyProcessState("프로세스 완료", ProcessStateType.Success);
                }
            }
            catch (Exception ex)
            {
                NotifyProcessState($"프로세스 실행 중 오류: {ex.Message}", ProcessStateType.Error);
            }
            finally
            {
                _isRunning = false;
            }
        }

        private void ProcessNextStep()
        {
            if (!_isRunning || _currentStep >= _processSteps.Count)
                return;

            ADASProcessStep currentStep = _processSteps[_currentStep];

            NotifyProcessState(
                $"단계 {_currentStep + 1} / {_processSteps.Count}: {currentStep.Description}",
                ProcessStateType.Progress,
                currentStep);

            try
            {
                bool success = currentStep.Execute();

                if (!success)
                {
                    NotifyProcessState(
                        $"단계 {_currentStep + 1} 실패: {currentStep.Description}",
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
                    $"단계 {_currentStep + 1} 오류 발생: {ex.Message}",
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