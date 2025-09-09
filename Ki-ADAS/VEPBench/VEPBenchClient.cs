using Ki_ADAS.VEPBench;

using Microsoft.Win32;

using NModbus;
using NModbus.Extensions;
using NModbus.Extensions.Enron;

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
        private ushort Addr_Validity = VEPBenchDataManager.Instance.DescriptionZone.ValidityIndicator;
        private ushort Addr_StatusZone = VEPBenchDataManager.Instance.DescriptionZone.StatusZoneAddr;
        private ushort Addr_SynchroZone = VEPBenchDataManager.Instance.DescriptionZone.SynchroZoneAddr;
        private ushort Addr_TransmissionZone = VEPBenchDataManager.Instance.DescriptionZone.TransmissionZoneAddr;
        private ushort Addr_ReceptionZone = VEPBenchDataManager.Instance.DescriptionZone.ReceptionZoneAddr;

        public bool IsConnected => _isConnected && _tcpClient != null && _tcpClient.Connected;

        // 연결 실패 시 재시도 설정
        private const int MaxRetryCount = 3;
        private const int RetryDelayMs = 1000;

        private Task _pollingTask = null;
        private CancellationTokenSource _cancellationTokenSouce = null;
        private int _pollingIntervalMs = 1000;

        private VEPBenchDataManager _vepManager = GlobalVal.Instance._VEP;

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
                OnDescriptionZoneRead();

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

                        var connectTask = _tcpClient.ConnectAsync(_ip, _port);

                        if (!connectTask.Wait(5000))
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
                    InitializeAndReadDescriptionZone();
                    PollAllZones();

                    while (!token.IsCancellationRequested)
                    {
                        Thread.Sleep(_pollingIntervalMs);

                        if (token.IsCancellationRequested)
                            break;

                        if (IsConnected)
                        {
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
                _vepManager.UpdateAllZonesFromRegisters(ReadAllRegisters);

                if (_vepManager.DescriptionZone.IsChanged)
                {
                    OnDescriptionZoneRead();
                    _vepManager.DescriptionZone.ResetChangedState();
                }

                if (_vepManager.StatusZone.IsChanged)
                {
                    OnStatusZoneChanged();
                    _vepManager.StatusZone.ResetChangedState();
                }

                if (_vepManager.SynchroZone.IsChanged)
                {
                    OnSynchroZoneChanged();
                    _vepManager.SynchroZone.ResetChangedState();
                }

                if (_vepManager.TransmissionZone.IsChanged)
                {
                    OnTransmissionZoneChanged();
                    _vepManager.TransmissionZone.ResetChangedState();
                    ProcessTransmissionRequest();
                }

                if (_vepManager.ReceptionZone.IsChanged)
                {
                    OnReceptionZoneChanged();
                    _vepManager.ReceptionZone.ResetChangedState();
                }
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

        private ushort[] ReadAllRegisters(int address, int count)
        {
            CheckConnection();

            try
            {
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, (ushort)address, (ushort)count);
                
                return registers;
            }
            catch (Exception ex)
            {
                LogMessage($"ReadAllRegisters 오류 발생: Addr={address}, Count={count}, Error={ex.Message}");
                throw;
            }
        }

        public ushort ReadValidityIndicator()
        {
            CheckConnection();

            try
            {
                var values = _modbusMaster.ReadHoldingRegisters(1, Addr_Validity, 1); // VEP 서버

                if (values[0] == 0)
                {
                    throw new Exception("VEP 소프트웨어가 준비되지 않았습니다.");
                }

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
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, _vepManager.DescriptionZone.ValidityIndicator, _vepManager.DescriptionZone.Length);

                _vepManager.DescriptionZone.FromRegisters(registers);

                LogMessage($"Description Zone 읽기 성공: ValidityIndicator={_vepManager.DescriptionZone.ValidityIndicator}, " +
                  $"StatusZoneAddr={_vepManager.DescriptionZone.StatusZoneAddr}, StatusZoneSize={_vepManager.DescriptionZone.StatusZoneSize}");

                return _vepManager.DescriptionZone;
            }
            catch (Exception ex)
            {
                LogMessage($"Description Zone 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteStatusZone()
        {
            CheckConnection();

            try
            {
                ushort[] registers = _vepManager.StatusZone.ToRegisters();
                _modbusMaster.WriteMultipleRegisters(1, Addr_StatusZone, registers);

                LogMessage($"Status Zone 쓰기 성공: VepStatus={_vepManager.StatusZone.GetVepStatusString()}, " +
                 $"StartCycle={_vepManager.StatusZone.StartCycle}, " +
                 $"VepCycleEnd={_vepManager.StatusZone.VepCycleEnd}, " +
                 $"BenchCycleEnd={_vepManager.StatusZone.BenchCycleEnd}");
            }
            catch (Exception ex)
            {
                LogMessage($"상태 영역 쓰기 오류: {ex.Message}");
                throw;
            }
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

        public void WriteSynchroZone()
        {
            CheckConnection();

            try
            {
                ushort[] arr = _vepManager.SynchroZone.ToRegisters();

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

                LogMessage($"동기화 영역 쓰기: FrontCameraAngle (Roll) = {_vepManager.SynchroZone.FrontCameraAngle1}, " +
                        $"FrontCameraAngle (Azimuth) ={_vepManager.SynchroZone.FrontCameraAngle2}, " +
                        $"FrontCameraAngle (Elevation) ={_vepManager.SynchroZone.FrontCameraAngle3}, " +
                        $"RearRightRadarAngle={_vepManager.SynchroZone.RearRightRadarAngle}, " +
                        $"RearLeftRadarAngle={_vepManager.SynchroZone.RearLeftRadarAngle}");
            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        // Bench -> VEP에 응답 Write
        public void WriteReceptionZone()
        {
            CheckConnection();

            try
            {
                ushort[] data = _vepManager.ReceptionZone.ToRegisters();
                _modbusMaster.WriteMultipleRegisters(1, Addr_ReceptionZone, data);
                LogMessage($"수신 영역 쓰기: {string.Join(", ", data)}");
            }
            catch (Exception ex)
            {
                LogMessage($"수신 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteTransmissionZone()
        {
            CheckConnection();

            try
            {
                ushort[] registers = _vepManager.TransmissionZone.ToRegisters();
                _modbusMaster.WriteMultipleRegisters(1, Addr_TransmissionZone, registers);

                LogMessage($"전송 영역 쓰기 성공: " +
                  $"AddTSize={_vepManager.TransmissionZone.AddTzSize}, " +
                  $"ExchStatus={_vepManager.TransmissionZone.ExchStatus}, " +
                  $"FctCode={_vepManager.TransmissionZone.FctCode}, " +
                  $"PCNum={_vepManager.TransmissionZone.PCNum}, " +
                  $"DataSize={_vepManager.TransmissionZone.Data.Length}");
            }
            catch (Exception ex)
            {
                LogMessage($"전송 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        private void ProcessTransmissionRequest()
        {
            var txZone = _vepManager.TransmissionZone;

            if (txZone.ExchStatus == VEPBenchTransmissionZone.ExchStatus_Request)
            {
                LogMessage($"Transmission Zone 요청 감지: FctCode={txZone.GetFctCodeString()}, PCNum={txZone.PCNum}");

                txZone.ExchStatus = VEPBenchTransmissionZone.ExchStatus_Response;
                WriteTransmissionZone();

                _vepManager.ReceptionZone.ExchStatus = VEPBenchReceptionZone.ExchStatus_Ready;
                WriteReceptionZone();
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

        protected virtual void OnDescriptionZoneRead()
        {
            DescriptionZoneRead?.Invoke(this, _vepManager.DescriptionZone);
        }

        protected virtual void OnStatusZoneChanged()
        {
            StatusZoneChanged?.Invoke(this, _vepManager.StatusZone);
        }

        protected virtual void OnSynchroZoneChanged()
        {
            SynchroZoneChanged?.Invoke(this, _vepManager.SynchroZone);
        }

        protected virtual void OnTransmissionZoneChanged()
        {
            TransmissionZoneChanged?.Invoke(this, _vepManager.TransmissionZone);
        }

        protected virtual void OnReceptionZoneChanged()
        {
            ReceptionZoneChanged?.Invoke(this, _vepManager.ReceptionZone);
        }

        // 로깅
        private void LogMessage(string message)
        {
            Console.WriteLine($"[Modbus Client] {DateTime.Now:HH:mm:ss.fff} - {message}");
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