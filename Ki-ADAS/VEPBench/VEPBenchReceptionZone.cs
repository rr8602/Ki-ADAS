using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchReceptionZone : IVEPBenchZone
    {
        private static VEPBenchReceptionZone _instance;
        private static readonly object _lock = new object();

        // 주소값
        public const int Offset_Reserved1 = 0;
        public const int Offset_Reserved2 = 1;
        public const int Offset_AddReSize = 2;
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
        public const int FctCode_PJI = 6;
        public const int FctCode_ReportPrint = 15;
        public const int FctCode_WorkInfo = 20;

        // 교환 상태
        public const ushort ExchStatus_Response = 1; // 요청 없음
        public const ushort ExchStatus_Ready = 2; // 요청 가능

        public ushort AddReSize { get; set; }
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

        public static VEPBenchReceptionZone Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new VEPBenchReceptionZone();
                    }
                }
                return _instance;
            }
        }

        private VEPBenchReceptionZone(int dataSize = 48)
        {
            ExchStatus = ExchStatus_Response;
            AddReSize = 0;
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

            if (AddReSize != registers[Offset_AddReSize]) { AddReSize = registers[Offset_AddReSize]; changed = true; }
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
                    if (Data[i] != registers[Offset_DataStart + i])
                    {
                        Data[i] = registers[Offset_DataStart + i];
                        changed = true;
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

            registers[Offset_AddReSize] = AddReSize;
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

        public void SetResponseCompleted()
        {
            ExchStatus = ExchStatus_Response;

        }

        public void SetResponseReady()
        {
            ExchStatus = ExchStatus_Ready;
        }

        public bool IsResponseCompleted => ExchStatus == ExchStatus_Response;

        public bool IsResponseReady => ExchStatus == ExchStatus_Ready;

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
