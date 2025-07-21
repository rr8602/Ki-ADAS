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

        private VEPBenchSynchroZone _synchroValues = new VEPBenchSynchroZone();
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
                    LanguageResource.GetMessage("Step_213_1"),
                    "Synchro 3 = 1",
                    () => SetSynchroValue(3, 1)),

                new ADASProcessStep(
                    214,
                    LanguageResource.GetMessage("Step_214"),
                    "Synchro 4 = 1",
                    () => SetSynchroValue(4, 1)),

                new ADASProcessStep(
                    213,
                    LanguageResource.GetMessage("Step_213_2"),
                    "Synchro 3 = 20",
                    () => SetSynchroValue(3, 20)),

                new ADASProcessStep(
                    213,
                    LanguageResource.GetMessage("Step_213_3"),
                    "Synchro 3 = 21",
                    () => SetSynchroValue(3, 21)),

                new ADASProcessStep(
                    320,
                    LanguageResource.GetMessage("Step_320"),
                    "Synchro 110 = 각도 1",
                    () => SetSynchroValue(110, GetMeasurementAngle(1))),

                new ADASProcessStep(
                    321,
                    LanguageResource.GetMessage("Step_321"),
                    "Synchro 111 = 각도 2",
                    () => SetSynchroValue(111, GetMeasurementAngle(2))),

                new ADASProcessStep(
                    322,
                    LanguageResource.GetMessage("Step_322"),
                    "Synchro 112 = 각도 3",
                    () => SetSynchroValue(112, GetMeasurementAngle(3))),

                new ADASProcessStep(
                    299,
                    LanguageResource.GetMessage("Step_299_1"),
                    "Synchro 89 = 1",
                    () => SetSynchroValue(89, 1)),

                new ADASProcessStep(
                    299,
                    LanguageResource.GetMessage("Step_299_1"),
                    "Synchro 89 = 2",
                    () => ValidateAnglesAndComplete())
            };
        }

        private bool ValidateAnglesAndComplete()
        {
            bool isValid = ValidateCameraAngles();

            if (!isValid)
            {
                NotifyProcessState(LanguageResource.GetMessage("AngleOutOfRange"), ProcessStateType.Error);

                // UI로 재시도 여부 설정으로 수정 필요
                bool retryCalibration = true;

                if (retryCalibration)
                {
                    if (SetSynchroValue(89, 2))
                    {
                        NotifyProcessState(LanguageResource.GetMessage("RetryCalibration"), ProcessStateType.Info);
                        _currentStep = -1;
                        return true;
                    }

                    return false;
                }
                else
                {
                    if (SetSynchroValue(89, 1))
                    {
                        NotifyProcessState(LanguageResource.GetMessage("AbortCalibration"), ProcessStateType.Warning);
                        _isRunning = false;
                        return true;
                    }

                    return false;
                }
            }

            return SetSynchroValue(89, 2);
        }

        private bool ValidateCameraAngles()
        {
            if (Math.Abs(_roll) > _rollThreshold)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("AngleValidationFail", LanguageResource.GetMessage("Roll"), _roll, _rollThreshold),
                    ProcessStateType.Warning);
                return false;
            }

            if (Math.Abs(_azimuth) > _azimuthThreshold)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("AngleValidationFail", LanguageResource.GetMessage("Azimuth"), _azimuth, _azimuthThreshold),
                    ProcessStateType.Warning);
                return false;
            }

            if (Math.Abs(_elevation) > _elevationThreshold)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("AngleValidationFail", LanguageResource.GetMessage("Elevation"), _elevation, _elevationThreshold),
                    ProcessStateType.Warning);
                return false;
            }

            NotifyProcessState(LanguageResource.GetMessage("CameraAngleValidationPass"), ProcessStateType.Success);
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

            return (int)(angle * 100);
        }

        // Synchro 값 설정
        private bool SetSynchroValue(int syncNumber, int value)
        {
            try
            {
                if (_vepBenchClient == null)
                {
                    NotifyProcessState(LanguageResource.GetMessage("VEPBenchNotInitialized"), ProcessStateType.Error);

                    return false;
                }

                if (syncNumber >= 9 && syncNumber < _synchroValues.Size)
                {
                    _synchroValues[syncNumber] = (ushort)value;

                    _vepBenchClient.WriteSynchroZone(_synchroValues);

                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("SynchroSettingComplete", syncNumber, value),
                        ProcessStateType.Info);

                    return true;
                }
                else if (syncNumber == 110 || syncNumber == 111 || syncNumber == 112)
                {
                    switch (syncNumber)
                    {
                        case 110:
                            _synchroValues.Angle1 = value / 100.0;
                            break;
                        case 111:
                            _synchroValues.Angle2 = value / 100.0;
                            break;
                        case 112:
                            _synchroValues.Angle3 = value / 100.0;
                            break;
                    }

                    _vepBenchClient.WriteSynchroZone(_synchroValues);

                    NotifyProcessState(
                        LanguageResource.GetFormattedMessage("SynchroSettingComplete", syncNumber, value),
                        ProcessStateType.Info);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                NotifyProcessState(
                    LanguageResource.GetFormattedMessage("SynchroSettingFail", ex.Message),
                    ProcessStateType.Error);

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

                _vepBenchClient.DisConnect();

                NotifyProcessState(LanguageResource.GetMessage("ProcessStop"), ProcessStateType.Info);
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