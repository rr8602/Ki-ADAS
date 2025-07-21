using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchTransmissionZone
    {
        // 상대 주소값
        public const int Offset_Reserved1 = 0;
        public const int Offset_Reserved2 = 1;
        public const int Offset_AddTSize = 2;
        public const int Offset_ExchStatus = 3;
        public const int Offset_Reserved3 = 4;
        public const int Offset_Reserved4 = 5;
        public const int Offset_FctAndPCNum = 6;
        public const int Offset_Reserver5 = 7;
        public const int Offset_ProcessAndSubFct = 8;
        public const int Offset_Reserved6 = 9;
        public const int Offset_Reserved7 = 10;
        public const int Offset_Reserved8 = 11;
        public const int Offset_DataStart = 12;

        // 기능 코드
        public const byte FctCode_PJI = 6;
        public const byte FctCode_ReportSend = 15;
        public const byte FctCode_DiagnosticInfo = 20;
        public const byte FctCode_VirtualDiag = 21;

        // 교환 상태
        public const ushort ExchStatus_Response = 1; // 요청 없음
        public const ushort ExchStatus_Request = 2; // 요청 가능

        public ushort AddTzSize { get; set; }
        public ushort ExchStatus { get; set; }
        public byte FctCode { get; set; }
        public byte PCNum { get; set; }
        public byte ProcessCode { get; set; }
        public byte SubFctCode { get; set; }
        public ushort[] Data { get; set; }
        public int TotalSize => Offset_DataStart + (Data?.Length ?? 0);

        public VEPBenchTransmissionZone(int dataSize = 48)
        {
            ExchStatus = ExchStatus_Response;
            AddTzSize = 0;
            FctCode = 0;
            PCNum = 0;
            ProcessCode = 0;
            SubFctCode = 0;
            Data = new ushort[dataSize];
        }

        public static VEPBenchTransmissionZone FromRegisters(ushort[] registers)
        {
            if (registers == null || registers.Length < Offset_DataStart)
                throw new ArgumentException("Invalid register array.");

            int dataSize = registers.Length - Offset_DataStart;
            var zone = new VEPBenchTransmissionZone(dataSize);

            zone.AddTzSize = registers[Offset_AddTSize];
            zone.ExchStatus = registers[Offset_ExchStatus];

            zone.FctCode = (byte)(registers[Offset_FctAndPCNum] & 0xFF);
            zone.PCNum = (byte)((registers[Offset_FctAndPCNum] >> 8) & 0xFF);

            zone.ProcessCode = (byte)(registers[Offset_ProcessAndSubFct] & 0xFF);
            zone.SubFctCode = (byte)((registers[Offset_ProcessAndSubFct] >> 8) & 0xFF);

            if (dataSize > 0)
            {
                for (int i = 0; i < dataSize; i++)
                {
                    if (Offset_DataStart + i < registers.Length)
                        zone.Data[i] = registers[Offset_DataStart + i];
                }
            }

            return zone;
        }

        public ushort[] ToRegisters()
        {
            int totalSize = TotalSize;
            ushort[] registers = new ushort[totalSize];

            registers[Offset_AddTSize] = AddTzSize;
            registers[Offset_ExchStatus] = ExchStatus;

            registers[Offset_FctAndPCNum] = (ushort)((PCNum << 8) | FctCode);
            registers[Offset_ProcessAndSubFct] = (ushort)((SubFctCode << 8) | ProcessCode);

            if (Data != null && Data.Length > 0)
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    if (Offset_DataStart + i < totalSize)
                        registers[Offset_DataStart + i] = Data[i];
                }
            }

            return registers;
        }

        public void SetData(ushort[] data, int startIndex = 0)
        {
            if (data == null)
                return;

            int copyLength = Math.Min(data.Length, Data.Length - startIndex);

            if (copyLength > 0 && startIndex >= 0 && startIndex < Data.Length)
            {
                Array.Copy(data, 0, Data, startIndex, copyLength);
            }
        }

        public void SetRequestMode()
        {
            ExchStatus = ExchStatus_Request;
        }

        public void SetResponseMode()
        {
            ExchStatus = ExchStatus_Response;
        }

        public bool IsRequest => ExchStatus == ExchStatus_Request;
        public bool IsResponse => ExchStatus == ExchStatus_Response;

        public string GetFctCodeString()
        {
            switch (FctCode)
            {
                case FctCode_PJI:
                    return "PJI";
                case FctCode_ReportSend:
                    return "Report Send";
                case FctCode_DiagnosticInfo:
                    return "Diagnostic Info";
                case FctCode_VirtualDiag:
                    return "Virtual Diagnostic";
                default:
                    return "Unknown Function Code";
            }
        }
    }
}
