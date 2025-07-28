using Ki_ADAS.VEPBench;

using Microsoft.Win32;

using NModbus;
using NModbus.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_ADAS
{
    public class VEPBenchClient
    {
        private readonly string _ip;
        private readonly int _port;

        private TcpClient _tcpClient;
        private IModbusMaster _modbusMaster;
        private bool _isConnected = false;

        // 시작 주소 값
        private const ushort Addr_Validity = 0;
        private const ushort Addr_StatusZone = 18;
        private const ushort Addr_StartCycle = 23;
        private const ushort Addr_SynchroZone = 210;
        private const ushort Addr_TransmissionZone = 400;
        private const ushort Addr_ReceptionZone = 460;

        public bool IsConnected => _isConnected && _tcpClient != null && _tcpClient.Connected;

        private VEPBenchProcessor _processor = new VEPBenchProcessor();

        // 연결 실패 시 재시도 설정
        private const int MaxRetryCount = 3;
        private const int RetryDelayMs = 1000;

        private Task _pollingTask = null;
        private CancellationTokenSource _cancellationTokenSouce = null;
        private int _pollingIntervalMs = 1000;

        // 마지막으로 읽은 Zone 값
        private VEPBenchStatusZone _lastStatusZone = null;
        private VEPBenchSynchroZone _lastSynchroZone = null;
        private VEPBenchTransmissionZone _lastTransmissionZone = null;
        private VEPBenchReceptionZone _lastReceptionZone = null;

        // 폴링 주기 설정
        public int PollingIntervalMs
        {
            get { return _pollingIntervalMs; }
            set { _pollingIntervalMs = Math.Max(100, value); }
        }

        // Zone 변경 이벤트
        public EventHandler<VEPBenchDescriptionZone> DescriptionZoneRead;
        public EventHandler<VEPBenchStatusZone> StatusZoneChanged;
        public EventHandler<VEPBenchSynchroZone> SynchroZoneChanged;
        public EventHandler<VEPBenchTransmissionZone> TransmissionZoneChanged;
        public EventHandler<VEPBenchReceptionZone> ReceptionZoneChanged;

        // 디버그 모드 플래그
        public bool DebugMode { get; set; } = false;

        public VEPBenchClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public void InitializeAndReadDescriptionZone()
        {
            try
            {
                if (!IsConnected)
                    Connect();

                var descriptionZone = ReadDescriptionZone();
                OnDescriptionZoneRead(descriptionZone);

                LogMessage("Description Zone 초기 읽기 완료");
            }
            catch (Exception ex)
            {
                LogMessage($"Description Zone 초기 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void Connect()
        {
            try
            {
                if (_isConnected || (_tcpClient != null && _tcpClient.Connected))
                {
                    DisConnect();
                }

                int retryCount = 0;
                bool connected = false;

                while (!connected && retryCount < MaxRetryCount)
                {
                    try
                    {
                        _tcpClient = new TcpClient();
                        
                        // 연결 타임아웃 설정
                        var connectTask = Task.Run(() => _tcpClient.Connect(_ip, _port));
                        bool completed = connectTask.Wait(5000); // 5초 타임아웃
                        
                        if (!completed)
                        {
                            throw new TimeoutException("연결 시간이 초과되었습니다.");
                        }

                        var factory = new ModbusFactory();
                        _modbusMaster = factory.CreateMaster(_tcpClient);
                        
                        // 타임아웃 설정
                        _modbusMaster.Transport.ReadTimeout = 3000;  // 읽기 타임아웃 3초
                        _modbusMaster.Transport.WriteTimeout = 3000; // 쓰기 타임아웃 3초
                        
                        connected = true;
                        _isConnected = true;
                        LogMessage("Modbus 서버에 연결되었습니다.");
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        LogMessage($"연결 시도 {retryCount}/{MaxRetryCount} 실패: {ex.Message}");
                        
                        if (_tcpClient != null)
                        {
                            _tcpClient.Dispose();
                            _tcpClient = null;
                        }
                        
                        if (retryCount < MaxRetryCount)
                        {
                            Thread.Sleep(RetryDelayMs);
                        }
                        else
                        {
                            throw new Exception($"최대 재시도 횟수({MaxRetryCount})를 초과했습니다. 연결 실패: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"연결 오류: {ex.Message}");
                _isConnected = false;
                throw;
            }
        }

        public void DisConnect()
        {
            try
            {
                if (_modbusMaster != null)
                {
                    _modbusMaster.Dispose();
                    _modbusMaster = null;
                }

                if (_tcpClient != null)
                {
                    if (_tcpClient.Connected)
                    {
                        _tcpClient.Close();
                    }

                    _tcpClient.Dispose();
                    _tcpClient = null;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"연결 해제 오류: {ex.Message}");
            }
            finally
            {
                _isConnected = false;
                LogMessage("Modbus 연결이 해제되었습니다.");
            }
        }

        // 주기적 Read 시작
        public void StartMonitoring()
        {
            if (_pollingTask != null && !_pollingTask.IsCompleted)
            {
                LogMessage("이미 모니터링이 실행 중입니다.");

                return;
            }

            _cancellationTokenSouce = new CancellationTokenSource();
            var token = _cancellationTokenSouce.Token;

            _pollingTask = Task.Run(() =>
            {
                LogMessage("모니터링 시작");

                try
                {
                    PollAllZones();

                    while (!token.IsCancellationRequested)
                    {
                        Thread.Sleep(_pollingIntervalMs);

                        if (token.IsCancellationRequested)
                            break;

                        if (IsConnected)
                        {
                            InitializeAndReadDescriptionZone();
                            PollAllZones();
                        }
                        else
                        {
                            try
                            {
                                LogMessage("연결이 끊어졌습니다. 재연결 시도 중...");
                                Connect();
                            }
                            catch (Exception ex)
                            {
                                LogMessage($"재연결 시도 중 오류 발생: {ex.Message}");
                                Thread.Sleep(5000);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"모니터링 중 오류 발생: {ex.Message}");
                }

                LogMessage("모니터링 종료");
            }, token);
        }

        // Read 중지
        public void StopMonitoring()
        {
            if (_cancellationTokenSouce != null)
            {
                _cancellationTokenSouce.Cancel();

                try
                {
                    if (_pollingTask != null)
                    {
                        bool completed = _pollingTask.Wait(3000);

                        if (!completed)
                        {
                            LogMessage("모니터링 작업이 시간 내에 완료되지 않았습니다. 강제 종료합니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"모니터링 중지 오류: {ex.Message}");
                }
                finally
                {
                    _pollingTask = null;
                    _cancellationTokenSouce.Dispose();
                    _cancellationTokenSouce = null;

                    LogMessage("모니터링이 중지되었습니다.");
                }
            }
        }

        // 모든 Zone 폴링
        private void PollAllZones()
        {
            try
            {
                PollValidityIndicator();
                PollStatusZone();
                PollSynchroZone();
                PollTransmissionZone();
                PollReceptionZone();
            }
            catch (Exception ex)
            {
                LogMessage($"모든 Zone 폴링 중 오류 발생: {ex.Message}");
            }
        }

        private void PollValidityIndicator()
        {
            try
            {
                ushort validity = ReadValidityIndicator();

                if (validity == 0)
                {
                    LogMessage("경고: VEP 소프트웨어가 준비되지 않았습니다.");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"유효성 표시기 폴링 오류: {ex.Message}");
            }
        }

        private void PollStatusZone()
        {
            try
            {
                var statusZone = ReadStatusZone();

                bool isChanged = _lastStatusZone == null ||
                    _lastStatusZone.VepStatus != statusZone.VepStatus ||
                    _lastStatusZone.VepCycleInterruption != statusZone.VepCycleInterruption ||
                    _lastStatusZone.VepCycleEnd != statusZone.VepCycleEnd;

                if (isChanged)
                {
                    _lastStatusZone = statusZone;
                    OnStatusZoneChanged(statusZone);
                    LogMessage($"Status Zone 변경 감지: VepStatus={statusZone.GetVepStatusString()}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"상태 영역 폴링 오류: {ex.Message}");
            }
        }

        private void PollSynchroZone()
        {
            try
            {
                var synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => ReadSynchroZone(start, count));

                bool isChanged = _lastSynchroZone == null ||
                    _lastSynchroZone.FrontCameraAngle1 != synchroZone.FrontCameraAngle1 ||
                    _lastSynchroZone.FrontCameraAngle2 != synchroZone.FrontCameraAngle2 ||
                    _lastSynchroZone.FrontCameraAngle3 != synchroZone.FrontCameraAngle3 ||
                    _lastSynchroZone.RearRightRadarAngle != synchroZone.RearRightRadarAngle ||
                    _lastSynchroZone.RearLeftRadarAngle != synchroZone.RearLeftRadarAngle;

                if (isChanged)
                {
                    _lastSynchroZone = synchroZone;
                    OnSynchroZoneChanged(synchroZone);
                    LogMessage($"동기화 영역 변경 감지: FrontCameraAngle (Roll) = {synchroZone.FrontCameraAngle1}, " +
                        $"FrontCameraAngle (Azimuth) ={ synchroZone.FrontCameraAngle2}, " +
                        $"FrontCameraAngle (Elevation) ={synchroZone.FrontCameraAngle3}, " +
                        $"RearRightRadarAngle={synchroZone.RearRightRadarAngle}, " +
                        $"RearLeftRadarAngle={synchroZone.RearLeftRadarAngle}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 폴링 오류: {ex.Message}");
            }
        }

        private void PollTransmissionZone()
        {
            try
            {
                var txZone = ReadTransmissionZone();

                bool isChanged = _lastTransmissionZone == null ||
                    _lastTransmissionZone.AddTzSize != txZone.AddTzSize ||
                    _lastTransmissionZone.ExchStatus != txZone.ExchStatus ||
                    !_lastTransmissionZone.Data.SequenceEqual(txZone.Data);

                if (isChanged)
                {
                    _lastTransmissionZone = txZone;
                    OnTransmissionZoneChanged(txZone);

                    if (txZone.IsRequest)
                    {
                        LogMessage($"Transmission Zone 요청 감지: FctCode={txZone.GetFctCodeString()}, PCNum={txZone.PCNum}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"전송 영역 폴링 오류: {ex.Message}");
            }
        }

        private void PollReceptionZone()
        {
            try
            {
                var receptionZone = ReadReceptionZone();

                bool isChanged = _lastReceptionZone == null ||
                    _lastReceptionZone.AddReSize != receptionZone.AddReSize ||
                    _lastReceptionZone.ExchStatus != receptionZone.ExchStatus ||
                    !_lastReceptionZone.Data.SequenceEqual(receptionZone.Data);

                if (isChanged)
                {
                    _lastReceptionZone = receptionZone;
                    OnReceptionZoneChanged(receptionZone);
                    LogMessage($"수신 영역 변경 감지: AddReSize={receptionZone.AddReSize}, ExchStatus={receptionZone.ExchStatus}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"수신 영역 폴링 오류: {ex.Message}");
            }
        }

        public ushort ReadValidityIndicator()
        {
            CheckConnection();

            try
            {
                var values = _modbusMaster.ReadHoldingRegisters(1, Addr_Validity, 1);

                LogMessage($"유효성 표시기 읽기: {values[0]} " +
                    $"(0=VEP 소프트웨어 미준비, 1=VEP 소프트웨어 준비됨)");

                return values[0];
            }
            catch (Exception ex)
            {
                LogMessage($"유효성 표시기 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public VEPBenchDescriptionZone ReadDescriptionZone()
        {
            CheckConnection();

            try
            {
                const ushort descriptionZoneStartAddress = 0;
                const ushort descriptionZoneLength = 18;

                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, descriptionZoneStartAddress, descriptionZoneLength);

                var descriptionZone = VEPBenchDescriptionZone.FromRegisters(registers);

                LogMessage($"Description Zone 읽기 성공: ValidityIndicator={descriptionZone.ValidityIndicator}, " +
                  $"StatusZoneAddr={descriptionZone.StatusZoneAddr}, StatusZoneSize={descriptionZone.StatusZoneSize}");

                return descriptionZone;
            }
            catch (Exception ex)
            {
                LogMessage($"Description Zone 읽기 오류: {ex.Message}");
                throw;
            }
        }

        // 벤치가 사이클 시작/정지 신호를 VEP에 전달
        public void WriteStartCycle(bool start)
        {
            CheckConnection();

            try
            {
                ushort value = (ushort)(start ? 1 : 0);
                _modbusMaster.WriteSingleRegister(1, Addr_StartCycle, value);
                LogMessage($"사이클 시작/정지 신호 전송: {(start ? "시작" : "정지")}");
            }
            catch (Exception ex)
            {
                LogMessage($"사이클 시작/정지 신호 전송 오류: {ex.Message}");
                throw;
            }
        }

        public VEPBenchStatusZone ReadStatusZone()
        {
            CheckConnection();

            try
            {
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, Addr_StatusZone, 6);
                var statusZone = VEPBenchStatusZone.FromRegisters(registers);

                LogMessage($"Status Zone 읽기 성공: VepStatus={statusZone.GetVepStatusString()}, " +
                  $"StartCycle={statusZone.StartCycle}, " +
                  $"VepCycleEnd={statusZone.VepCycleEnd}, " +
                  $"BenchCycleEnd={statusZone.BenchCycleEnd}");

                return statusZone;
            }
            catch (Exception ex)
            {
                LogMessage($"상태 영역 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteStatusZone(VEPBenchStatusZone statusZone)
        {
            CheckConnection();

            try
            {
                ushort[] registers = statusZone.ToRegisters();
                _modbusMaster.WriteMultipleRegisters(1, Addr_StatusZone, registers);

                LogMessage($"Status Zone 쓰기 성공: VepStatus={statusZone.GetVepStatusString()}, " +
                 $"StartCycle={statusZone.StartCycle}, " +
                 $"VepCycleEnd={statusZone.VepCycleEnd}, " +
                 $"BenchCycleEnd={statusZone.BenchCycleEnd}");
            }
            catch (Exception ex)
            {
                LogMessage($"상태 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        // Bench -> VEP에 응답 Write
        public void WriteReceptionZone(ushort[] data)
        {
            CheckConnection();

            try
            {
                _modbusMaster.WriteMultipleRegisters(1, Addr_ReceptionZone, data);
                LogMessage($"수신 영역 쓰기: {string.Join(", ", data)}");
            }
            catch (Exception ex)
            {
                LogMessage($"수신 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        // VEP -> Bench에 요청 Read
        public VEPBenchTransmissionZone ReadTransmissionZone(int length = 20)
        {
            CheckConnection();

            try
            {
                var registers = _modbusMaster.ReadHoldingRegisters(1, Addr_TransmissionZone, (ushort)length);
                var transmissionZone = VEPBenchTransmissionZone.FromRegisters(registers);

                LogMessage($"전송 영역 읽기 성공: " +
                  $"AddTSize={transmissionZone.AddTzSize}, " +
                  $"ExchStatus={transmissionZone.ExchStatus}, " +
                  $"FctCode={transmissionZone.FctCode}, " +
                  $"PCNum={transmissionZone.PCNum}, " +
                  $"DataSize={transmissionZone.Data.Length}");

                return transmissionZone;
            }
            catch (Exception ex)
            {
                LogMessage($"전송 영역 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public VEPBenchReceptionZone ReadReceptionZone(int length = 20)
        {
            CheckConnection();

            try
            {
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, Addr_ReceptionZone, (ushort)length);
                var receptionZone = VEPBenchReceptionZone.FromRegisters(registers);

                LogMessage($"수신 영역 읽기 성공: " +
                  $"AddReSize={receptionZone.AddReSize}, " +
                  $"ExchStatus={receptionZone.ExchStatus}, " +
                  $"DataSize={receptionZone.Data.Length}");

                return receptionZone;
            }
            catch (Exception ex)
            {
                LogMessage($"수신 영역 읽기 오류: {ex.Message}");
                throw;
            }
        }

        protected virtual void OnDescriptionZoneRead(VEPBenchDescriptionZone descriptionZone)
        {
            DescriptionZoneRead?.Invoke(this, descriptionZone);
        }

        protected virtual void OnStatusZoneChanged(VEPBenchStatusZone statusZone)
        {
            StatusZoneChanged?.Invoke(this, statusZone);
        }

        protected virtual void OnSynchroZoneChanged(VEPBenchSynchroZone synchroZone)
        {
            SynchroZoneChanged?.Invoke(this, synchroZone);
        }

        protected virtual void OnTransmissionZoneChanged(VEPBenchTransmissionZone transmissionZone)
        {
            TransmissionZoneChanged?.Invoke(this, transmissionZone);
        }

        protected virtual void OnReceptionZoneChanged(VEPBenchReceptionZone receptionZone)
        {
            ReceptionZoneChanged?.Invoke(this, receptionZone);
        }

        // Test
        public void RunBenchRoop()
        {
            CheckConnection();
            try
            {
                // 1. VEP 정상동작 확인
                LogMessage("VEP 정상동작 확인 중...");
                if (ReadValidityIndicator() != 1)
                {
                    LogMessage("VEP 소프트웨어가 준비되지 않았습니다.");
                    return;
                }

                // 2. Bench에서 사이클 Start (1초 후 0으로 복귀)
                LogMessage("사이클 시작 신호 전송...");
                WriteStartCycle(true);
                Thread.Sleep(1000);
                WriteStartCycle(false);

                LogMessage("StartCycle 신호 전송 완료");

                // 3. TransmissionZone 폴링하면서 요청 대기
                LogMessage("전송 영역 폴링 시작...");
                int pollCount = 0;
                const int maxPollCount = 30; // 최대 30초 대기
                
                while (pollCount < maxPollCount)
                {
                    var tx = ReadTransmissionZone();

                    /*if (IsVEPRequestAvailable(tx))
                    {
                        LogMessage("유효한 VEP 요청 감지");
                        var request = new VEPBenchRequest(tx);
                        LogMessage($"요청 타입: {request.RequestType}, 함수 코드: {request.FctCode}");
                        
                        var response = _processor.Process(request);
                        LogMessage($"응답 생성 완료: {string.Join(", ", response.Data)}");

                        WriteReceptionZone(response.Data);
                        LogMessage("응답 전송 완료");
                        return;
                    }*/

                    LogMessage($"폴링 {++pollCount}/{maxPollCount}: 유효한 요청 없음");
                    Thread.Sleep(1000);
                }
                
                LogMessage("최대 폴링 횟수 초과: 유효한 요청을 받지 못했습니다.");
            }
            catch (Exception ex)
            {
                LogMessage($"벤치 루프 실행 오류: {ex.Message}");
                throw;
            }
        }

        // 요청 감지: ExchStatus가 2일 때
        private bool IsVEPRequestAvailable(ushort[] tx)
        {
            bool result = tx != null && tx.Length > 3 && tx[3] == 2;
            LogMessage($"요청 상태 확인: {(result ? "유효한 요청 있음" : "유효한 요청 없음")}");
            return result;
        }

        public ushort[] ReadSynchroZone(int startIndex, int count)
        {
            CheckConnection();

            ushort modbusStartAddr = (ushort)(Addr_SynchroZone + startIndex);

            if (count < 1 || count > 123)
                throw new ArgumentOutOfRangeException(nameof(count), "numberOfPoints must be between 1 and 123 inclusive.");

            try
            {
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, modbusStartAddr, (ushort)count);
                LogMessage($"동기화 영역 부분 읽기: Start={modbusStartAddr}, Count={count}, Data={string.Join(",", registers)}");

                return registers;
            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteSynchroZone(VEPBenchSynchroZone s)
        {
            CheckConnection();

            try
            {
                ushort[] arr = s.ToUshortArray();

                if (arr.Length >= VEPBenchSynchroZone.SYNCHRO_SIZE_PART1)
                {
                    ushort[] part1 = new ushort[VEPBenchSynchroZone.SYNCHRO_SIZE_PART1];
                    Array.Copy(arr, 0, part1, 0, VEPBenchSynchroZone.SYNCHRO_SIZE_PART1);
                    _modbusMaster.WriteMultipleRegisters(1, Addr_SynchroZone, part1);
                }

                if (VEPBenchSynchroZone.SYNCHRO_SIZE_PART2 > 0 && arr.Length > VEPBenchSynchroZone.SYNCHRO_SIZE_PART1)
                {
                    ushort[] part2 = new ushort[VEPBenchSynchroZone.SYNCHRO_SIZE_PART2];
                    Array.Copy(arr, VEPBenchSynchroZone.SYNCHRO_SIZE_PART1, part2, 0, VEPBenchSynchroZone.SYNCHRO_SIZE_PART2);
                    _modbusMaster.WriteMultipleRegisters(1, (ushort)(Addr_SynchroZone + VEPBenchSynchroZone.SYNCHRO_SIZE_PART1), part2);
                }

                LogMessage($"동기화 영역 쓰기: FrontCameraAngle (Roll) = {s.FrontCameraAngle1}, " +
                        $"FrontCameraAngle (Azimuth) ={s.FrontCameraAngle2}, " +
                        $"FrontCameraAngle (Elevation) ={s.FrontCameraAngle3}, " +
                        $"RearRightRadarAngle={s.RearRightRadarAngle}, " +
                        $"RearLeftRadarAngle={s.RearLeftRadarAngle}");
            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        // 연결 상태 확인
        private void CheckConnection()
        {
            if (!IsConnected)
            {
                LogMessage("Modbus 서버에 연결되어 있지 않습니다. 연결 시도 중...");
                Connect();
            }
        }

        // 로깅
        private void LogMessage(string message)
        {
            if (DebugMode)
            {
                Console.WriteLine($"[Modbus Client] {DateTime.Now:HH:mm:ss.fff} - {message}");
            }
        }

        // Modbus 서버 테스트 - 단순히 연결 확인
        public bool TestConnection()
        {
            try
            {
                CheckConnection();
                // 단순히 레지스터 하나를 읽어서 연결이 잘 되는지 확인
                _modbusMaster.ReadHoldingRegisters(1, 0, 1);
                LogMessage("Modbus 서버 연결 테스트 성공");
                return true;
            }
            catch (Exception ex)
            {
                LogMessage($"Modbus 서버 연결 테스트 실패: {ex.Message}");
                return false;
            }
        }
    }
}