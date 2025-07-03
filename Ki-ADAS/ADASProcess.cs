using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static Ki_ADAS.VEPProtocol;

namespace Ki_ADAS
{
    public class ADASProcess
    {
        private VEPProtocol _vepProtocol;
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

        public ADASProcess(VEPProtocol vepProtocol)
        {
            _vepProtocol = vepProtocol ?? throw new ArgumentNullException(nameof(vepProtocol));

            _vepProtocol.OnDataReceived += VEPProtocol_OnDataReceived;
            _vepProtocol.OnConnectionChanged += VEPProtocol_OnConnectionChanged;

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
                    () => _vepProtocol.SetSynchroValue(3, 1)),

                new ADASProcessStep(
                    214,
                    "전면 카메라 타겟 위치 확인",
                    "Synchro 4 = 1",
                    () => _vepProtocol.SetSynchroValue(4, 1)),

                new ADASProcessStep(
                    213,
                    "타켓을 홈 위치로 이동",
                    "Synchro 3 = 20",
                    () => _vepProtocol.SetSynchroValue(3, 20)),

                new ADASProcessStep(
                    213,
                    "타켓을 홈 위치로 이동 2단계",
                    "Synchro 3 = 21",
                    () => _vepProtocol.SetSynchroValue(3, 21)),

                new ADASProcessStep(
                    320,
                    "각도 1 측정값 전송",
                    "Synchro 110 = 각도 1",
                    () => _vepProtocol.SetSynchroValue(110, GetMeasurementAngle(1))),

                new ADASProcessStep(
                    321,
                    "각도 2 측정값 전송",
                    "Synchro 111 = 각도 2",
                    () => _vepProtocol.SetSynchroValue(111, GetMeasurementAngle(2))),

                new ADASProcessStep(
                    322,
                    "각도 3 측정값 전송",
                    "Synchro 112 = 각도 3",
                    () => _vepProtocol.SetSynchroValue(112, GetMeasurementAngle(3))),

                new ADASProcessStep(
                    299,
                    "Synchro 89 첫번째 시도",
                    "Synchro 89 = 1",
                    () => _vepProtocol.SetSynchroValue(89, 1)),

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

            return _vepProtocol.SetSynchroValue(89, 2);
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

        // VEP 프로토콜 데이터 수신 처리
        private void VEPProtocol_OnDataReceived(object sender, VEPDataReceivedEventArgs e)
        {
            switch (e.Response.Command)
            {
                case VEPCommand.CameraCalibration:
                    if (e.Response.Data.Length > 0)
                    {
                        if (e.Response.Data[0] == 1)
                        {
                            NotifyProcessState("캘리브레이션 완료", ProcessStateType.Success);
                        }
                        else
                        {
                            NotifyProcessState("캘리브레이션 실패", ProcessStateType.Error);
                        }
                    }

                    break;

                case VEPCommand.SetSynchro:
                    if (e.Response.Data.Length > 0)
                    {
                        bool success = e.Response.Data[0] == 1;

                        if (success)
                        {
                            NotifyProcessState("동기화 값 설정 성공", ProcessStateType.Success);
                        }
                        else
                        {
                            NotifyProcessState("동기화 값 설정 실패", ProcessStateType.Error);
                        }
                    }
                    
                    break;

                case VEPCommand.GetSynchro:
                    if (e.Response.Data.Length > 1)
                    {
                        int syncNumber = e.Response.Data[0];
                        int syncValue = e.Response.Data[1];

                        NotifyProcessState($"동기화 값 수신: Synchro {syncNumber} = {syncValue}", ProcessStateType.Info);
                    }

                    break;

                case VEPCommand.AngleMeasurement:
                    if (e.Response.Data.Length > 0)
                    {
                        NotifyProcessState($"각도 측정 결과: {e.Response.Data[0]}", ProcessStateType.Info);
                    }

                    break;
            }
        }

        // VEP 프로토콜 연결 상태 변경 처리
        private void VEPProtocol_OnConnectionChanged(object sender, VEPConnectionEventArgs e)
        {
            if (!e.IsConnected && _isRunning)
            {
                Stop();
                NotifyProcessState("VEP 서버 연결 끊김", ProcessStateType.Info);
            }
        }

        public bool Start(string ipAddress, int port)
        {
            try
            {
                if (_isRunning)
                    return false;

                bool connected = _vepProtocol.Connect(ipAddress, port);

                if (!connected)
                {
                    NotifyProcessState("VEP 서버 연결 실패", ProcessStateType.Error);
                    return false;
                }

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

                _vepProtocol.DisConnect();
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