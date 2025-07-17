using Ki_ADAS.VEPBench;
using Modbus.Device;
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
        private NModbus.IModbusMaster _modbusMaster;
        private bool _isConnected = false;

        private const ushort Addr_Validity = 0;
        private const ushort Addr_StatusZone = 18;
        private const ushort Addr_StartCycle = 23;
        private const ushort Addr_SynchroZone = 110;
        private const ushort SynchroZoneLength = 3;
        private const ushort Addr_TransmissionZone = 68;
        private const ushort Addr_ReceptionZone = 128;

        public bool IsConnected => _isConnected && _tcpClient != null && _tcpClient.Connected;

        private VEPBenchProcessor _processor = new VEPBenchProcessor();

        // 연결 실패 시 재시도 설정
        private const int MaxRetryCount = 3;
        private const int RetryDelayMs = 1000;

        // 디버그 모드 플래그
        public bool DebugMode { get; set; } = false;

        public VEPBenchClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
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
        
        // VEP 상태 확인
        public ushort ReadValidityIndicator()
        {
            CheckConnection();

            try
            {
                var values = _modbusMaster.ReadHoldingRegisters(1, Addr_Validity, 1);
                LogMessage($"유효성 지시자 읽기: {values[0]}");

                return values[0];
            }
            catch (Exception ex)
            {
                LogMessage($"유효성 지시자 읽기 오류: {ex.Message}");
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

        // status, synchro 등은 읽기/쓰기 모두 가능
        public ushort[] ReadStatusZone(ushort length)
        {
            CheckConnection();
            try
            {
                var result = _modbusMaster.ReadHoldingRegisters(1, Addr_StatusZone, length);
                LogMessage($"상태 영역 읽기: {string.Join(", ", result)}");
                return result;
            }
            catch (Exception ex)
            {
                LogMessage($"상태 영역 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteStatusZone(ushort[] data)
        {
            CheckConnection();

            try
            {
                _modbusMaster.WriteMultipleRegisters(1, Addr_StatusZone, data);
                LogMessage($"상태 영역 쓰기: {string.Join(", ", data)}");
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
        public ushort[] ReadTransmissionZone(int length = 20)
        {
            CheckConnection();

            try
            {
                var result = _modbusMaster.ReadHoldingRegisters(1, Addr_TransmissionZone, (ushort)length);
                LogMessage($"전송 영역 읽기: {string.Join(", ", result)}");
                return result;
            }
            catch (Exception ex)
            {
                LogMessage($"전송 영역 읽기 오류: {ex.Message}");
                throw;
            }
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

                    if (IsVEPRequestAvailable(tx))
                    {
                        LogMessage("유효한 VEP 요청 감지");
                        var request = new VEPBenchRequest(tx);
                        LogMessage($"요청 타입: {request.RequestType}, 함수 코드: {request.FctCode}");
                        
                        var response = _processor.Process(request);
                        LogMessage($"응답 생성 완료: {string.Join(", ", response.Data)}");

                        WriteReceptionZone(response.Data);
                        LogMessage("응답 전송 완료");
                        return;
                    }

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

        public VEPBenchSynchro ReadSynchroZone()
        {
            CheckConnection();

            try
            {
                ushort[] arr = _modbusMaster.ReadHoldingRegisters(1, Addr_SynchroZone, SynchroZoneLength);
                var synchro = VEPBenchSynchro.FormUshortArray(arr);
                LogMessage($"동기화 영역 읽기: Angle1={synchro.Angle1}, Angle2={synchro.Angle2}, Angle3={synchro.Angle3}");
                return synchro;
            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteSynchroZone(VEPBenchSynchro s)
        {
            CheckConnection();

            try
            {
                ushort[] arr = s.ToUshortArray();
                _modbusMaster.WriteMultipleRegisters(1, Addr_SynchroZone, arr);
                LogMessage($"동기화 영역 쓰기: Angle1={s.Angle1}, Angle2={s.Angle2}, Angle3={s.Angle3}");
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