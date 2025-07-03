using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NModbus;

namespace Ki_ADAS
{
    public class VEPProtocol
    {
        private TcpClient _tcpClient;
        private ModbusFactory _modbusFactory;
        private IModbusMaster _modbusMaster;
        private bool _isConnected = false;
        private readonly object _lockObject = new object();

        public event EventHandler<VEPDataReceivedEventArgs> OnDataReceived;
        public event EventHandler<VEPConnectionEventArgs> OnConnectionChanged;

        private const byte SlaveId = 1; // Modbus 슬레이브 ID

        public VEPProtocol()
        {
            _tcpClient = new TcpClient();
            _modbusFactory = new ModbusFactory();
        }

        // VEP 명령어
        public enum VEPCommand
        {
            Unknown = 0,
            SetSynchro = 1,
            GetSynchro = 2,
            CameraCalibration = 3,
            InitCamera = 4,
            AngleMeasurement = 5
        }

        // VEP 응답 클래스
        public class VEPResponse
        {
            public VEPCommand Command { get; }
            public ushort[] Data { get; }
            public VEPResponse(VEPCommand command, ushort[] data)
            {
                Command = command;
                Data = data;
            }
        }

        // Modbus 연결 상태 변경
        public class ModbusConnectionEventArgs : EventArgs
        {
            public bool IsConnected { get; }
            public ModbusConnectionEventArgs(bool isConnected)
            {
                IsConnected = isConnected;
            }
        }

        // VEP 데이터 수신 이벤트
        public class VEPDataReceivedEventArgs : EventArgs
        {
            public VEPResponse Response { get; }
            public VEPDataReceivedEventArgs(VEPResponse response)
            {
                Response = response;
            }
        }

        // VEP 연결 상태 변경 이벤트
        public class VEPConnectionEventArgs : EventArgs
        {
            public bool IsConnected { get; }
            public VEPConnectionEventArgs(bool isConnected)
            {
                IsConnected = isConnected;
            }
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_isConnected)
                        DisConnect();

                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(ipAddress, port);

                    bool connected = _tcpClient.Connected;

                    if (connected)
                    {
                        _modbusMaster = _modbusFactory.CreateMaster(_tcpClient);

                        _modbusMaster.Transport.ReadTimeout = 2000;
                        _modbusMaster.Transport.WriteTimeout = 2000;

                        _isConnected = true;
                        OnConnectionChanged?.Invoke(this, new VEPConnectionEventArgs(true));
                    }

                    return connected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VEP 연결 오류: {ex.Message}");
                return false;
            }
        }

        public void DisConnect()
        {
            lock (_lockObject)
            {
                if (_tcpClient != null && _tcpClient.Connected)
                {
                    _modbusMaster?.Dispose();
                    _tcpClient.Close();
                    _isConnected = false;
                    OnConnectionChanged?.Invoke(this, new VEPConnectionEventArgs(false));
                }
            }
        }

        // 데이터 전송
        public bool SendCommnad(VEPCommand command, ushort[] data)
        {
            try
            {
                if (!_isConnected)
                    return false;

                ushort[] commandData = new ushort[data.Length + 1];
                commandData[0] = (ushort)command;
                Array.Copy(data, 0, commandData, 1, data.Length);

                _modbusMaster.WriteMultipleRegisters(SlaveId, 0, commandData);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VEP 데이터 전송 오류: {ex.Message}");

                return false;
            }
        }

        // 데이터 읽기
        public VEPResponse ReadResponse(ushort startAddress, ushort numberOfPoints)
        {
            try
            {
                if (!_isConnected || _modbusMaster == null)
                    return new VEPResponse(VEPCommand.Unknown, new ushort[0]);

                ushort[] response = _modbusMaster.ReadHoldingRegisters(SlaveId, startAddress, numberOfPoints);

                if (response.Length > 0)
                {
                    VEPCommand command = (VEPCommand)response[0];

                    ushort[] data = new ushort[response.Length - 1];
                    Array.Copy(response, 1, data, 0, data.Length);

                    VEPResponse vepResponse = new VEPResponse(command, data);
                    OnDataReceived?.Invoke(this, new VEPDataReceivedEventArgs(vepResponse));

                    return vepResponse;
                }

                return new VEPResponse(VEPCommand.Unknown, new ushort[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VEP 데이터 읽기 오류: {ex.Message}");

                return new VEPResponse(VEPCommand.Unknown, new ushort[0]);
            }
        }

        // 동기화 값 설정 명령 전송
        public bool SetSynchroValue(int syncNumber, int value)
        {
            ushort[] data = new ushort[2];
            data[0] = (ushort)syncNumber;
            data[1] = (ushort)value;

            return SendCommnad(VEPCommand.SetSynchro, data);
        }

        // 동기화 값 요청 명령 전송
        public bool RequestSynchroValue(int syncNumber)
        {
            ushort[] data= new ushort[1];
            data[0] = (ushort)syncNumber;

            return SendCommnad(VEPCommand.GetSynchro, data);
        }

        // 카메라 켈리브레이션 요청
        public bool RequestCameraCalibration()
        {
            return SendCommnad(VEPCommand.CameraCalibration, new ushort[0]);
        }

        // 카메라 초기화 시작
        public bool InitCamera()
        {
            return SendCommnad(VEPCommand.InitCamera, new ushort[0]);
        }

        // 리소스 해제
        public void Dispose()
        {
            DisConnect();
            _tcpClient?.Dispose();
        }
    }
}