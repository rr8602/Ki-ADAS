using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchTransmissionZone : IVEPBenchZone
    {
        private static VEPBenchTransmissionZone _instance;
        private static readonly object _lock = new object();
        private static int TZ_Addr = VEPBenchDataManager.Instance.DescriptionZone.TransmissionZoneAddr;

        // 상대 주소값
        public int Offset_Reserved1 = TZ_Addr;
        public int Offset_Reserved2 = TZ_Addr + 1;
        public int Offset_AddTSize = TZ_Addr + 2;
        public int Offset_ExchStatus = TZ_Addr + 3;
        public int Offset_Reserved3 = TZ_Addr + 4;
        public int Offset_Reserved4 = TZ_Addr + 5;
        public int Offset_FctAndPCNum = TZ_Addr + 6;
        public int Offset_Reserver5 = TZ_Addr + 7;
        public int Offset_ProcessAndSubFct = TZ_Addr + 8;
        public int Offset_Reserved6 = TZ_Addr + 9;
        public int Offset_Reserved7 = TZ_Addr + 10;
        public int Offset_Reserved8 = TZ_Addr + 11;
        public int Offset_DataStart = TZ_Addr + 12;

        // 기능 코드
        public const int FctCode_PJI = 6;
        public const int FctCode_ReportPrint = 15;
        public const int FctCode_WorkInfo = 20;

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

        private bool _isChanged;
        public bool IsChanged => _isChanged;

        public void ResetChangedState()
        {
            _isChanged = false;
        }

        public static VEPBenchTransmissionZone Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new VEPBenchTransmissionZone();
                    }
                }

                return _instance;
            }
        }

        private VEPBenchTransmissionZone(int dataSize = 48)
        {
            ExchStatus = ExchStatus_Response;
            AddTzSize = 0;
            FctCode = 0;
            PCNum = 1; // 고정값
            ProcessCode = 0;
            SubFctCode = 0;
            Data = new ushort[dataSize];
            _isChanged = false;
        }

        public void FromRegisters(ushort[] registers)
        {
            if (registers == null || registers.Length < Offset_DataStart)
                throw new ArgumentException("Invalid register array.");

            bool changed = false;

            if (AddTzSize != registers[Offset_AddTSize]) { AddTzSize = registers[Offset_AddTSize]; changed = true; }
            if (ExchStatus != registers[Offset_ExchStatus]) { ExchStatus = registers[Offset_ExchStatus]; changed = true; }

            byte newFctCode = (byte)(registers[Offset_FctAndPCNum] & 0xFF);
            byte newPCNum = (byte)((registers[Offset_FctAndPCNum] >> 8) & 0xFF);
            if (FctCode != newFctCode) { FctCode = newFctCode; changed = true; }
            if (PCNum != newPCNum) { PCNum = newPCNum; changed = true; }

            byte newProcessCode = (byte)(registers[Offset_ProcessAndSubFct] & 0xFF);
            byte newSubFctCode = (byte)((registers[Offset_ProcessAndSubFct] >> 8) & 0xFF);
            if (ProcessCode != newProcessCode) { ProcessCode = newProcessCode; changed = true; }
            if (SubFctCode != newSubFctCode) { SubFctCode = newSubFctCode; changed = true; }

            int dataSize = registers.Length - Offset_DataStart;

            if (Data.Length != dataSize)
            {
                Data = new ushort[dataSize];
                changed = true;
            }

            if (dataSize > 0)
            {
                for (int i = 0; i < dataSize; i++)
                {
                    if (Offset_DataStart + i < registers.Length)
                    {
                        if (Data[i] != registers[Offset_DataStart + i])
                        {
                            Data[i] = registers[Offset_DataStart + i];
                            changed = true;
                        }
                    }
                }
            }

            if (changed)
            {
                _isChanged = true;
            }
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
                case FctCode_ReportPrint:
                    return "Report Print";
                case FctCode_WorkInfo:
                    return "Work Info";
                default:
                    return "Unknown Function Code";
            }
        }
    }
}
