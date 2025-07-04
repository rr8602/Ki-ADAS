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

        private const ushort Addr_Validity = 0;
        private const ushort Addr_StatusZone = 18;
        private const ushort Addr_StartCycle = 23;
        private const ushort Addr_SynchroZone = 28;
        private const ushort Addr_TransmissionZone = 68;
        private const ushort Addr_ReceptionZone = 128;

        private VEPBenchProcessor _processor = new VEPBenchProcessor();

        public VEPBenchClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public void Connect()
        {
            _tcpClient = new TcpClient(_ip, _port);
            _tcpClient.Connect(_ip, _port);
            var factory = new ModbusFactory();
            _modbusMaster = factory.CreateMaster(_tcpClient);
            Console.WriteLine("[Bench] Connected to VEP Slave.");
        }

        public void DisConnect()
        {
            _tcpClient?.Close();
            Console.WriteLine("[Bench] Disconnected.");
        }
        
        // VEP 상태 확인
        public ushort ReadValidityIndicator()
        {
            var values = _modbusMaster.ReadHoldingRegisters(1, Addr_Validity, 1);

            return values[0];
        }

        // 벤치가 사이클 시작/정지 신호를 VEP에 전달
        public void WriteStartCycle(bool start)
        {
            ushort value = (ushort)(start ? 1 : 0);
            _modbusMaster.WriteSingleRegister(1, Addr_StartCycle, value);
        }

        // status, synchro 등은 읽기/쓰기 모두 가능
        public ushort[] ReadStatusZone(ushort length)
        {
            return _modbusMaster.ReadHoldingRegisters(1, Addr_SynchroZone, length);
        }

        public void WriteStatusZone(ushort[] data)
        {
            _modbusMaster.WriteMultipleRegisters(1, Addr_SynchroZone, data);
        }

        // Bench -> VEP에 응답 Write
        public void WriteReceptionZone(ushort[] data)
        {
            _modbusMaster.WriteMultipleRegisters(1, Addr_ReceptionZone, data);
        }

        // VEP -> Bench에 요청 Read
        public ushort[] ReadTransmissionZone(int length = 20)
        {
            return _modbusMaster.ReadHoldingRegisters(1, Addr_TransmissionZone, (ushort)length);
        }

        public void WriteSynchroZone(ushort[] data)
        {
            _modbusMaster.WriteMultipleRegisters(1, Addr_SynchroZone, data);
        }

        // Test
        public void RunBenchRoop()
        {
            // 1. VEP 정상동작 확인
            if (ReadValidityIndicator() != 1)
            {
                Console.WriteLine("[Bench] VEP 소프트웨어가 준비되지 않았습니다.");

                return;
            }

            // 2. Bench에서 사이클 Start (1초 후 0으로 복귀)
            WriteStartCycle(true);
            Thread.Sleep(1000);
            WriteStartCycle(false);

            Console.WriteLine("[Bench] StartCycle 신호 전송");

            // 3. TransmissionZone 폴링하면서 요청 대기
            while (true)
            {
                var tx = ReadTransmissionZone();

                if (IsVEPRequestAvailable(tx))
                {
                    var request = new VEPBenchRequest(tx);
                    var response = _processor.Process(request);

                    WriteReceptionZone(response.Data);

                    break;
                }

                Thread.Sleep(1000);
            }
        }

        // 요청 감지: ExchStatus가 2일 때
        private bool IsVEPRequestAvailable(ushort[] tx)
        {
            return tx != null && tx.Length > 3 && tx[3] == 2;
        }
    }
}
