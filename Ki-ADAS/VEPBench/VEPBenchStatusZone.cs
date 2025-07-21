using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchStatusZone
    {
        public const int Offset_VepStatus = 0;
        public const int Offset_VepCycleInterruption = 1;
        public const int Offset_VepCycleEnd = 2;
        public const int Offset_BenchCycleInterruption = 3;
        public const int Offset_BenchCycleEnd = 4;
        public const int Offset_StartCycle = 5;

        public const ushort VepStatus_Undefined = 0;
        public const ushort VepStatus_Waiting = 1;
        public const ushort VepStatus_Working = 2;

        public ushort VepStatus { get; set; }
        public ushort VepCycleInterruption { get; set; }
        public ushort VepCycleEnd { get; set; }
        public ushort BenchCycleInterruption { get; set; }
        public ushort BenchCycleEnd { get; set; }
        public ushort StartCycle { get; set; }

        public VEPBenchStatusZone()
        {
            VepStatus = VepStatus_Waiting;
            VepCycleInterruption = 0;
            VepCycleEnd = 0;
            BenchCycleInterruption = 0;
            BenchCycleEnd = 0;
            StartCycle = 0;
        }

        public static VEPBenchStatusZone FromRegisters(ushort[] registers)
        {
            return new VEPBenchStatusZone
            {
                VepStatus = registers[Offset_VepStatus],
                VepCycleInterruption = registers[Offset_VepCycleInterruption],
                VepCycleEnd = registers[Offset_VepCycleEnd],
                BenchCycleInterruption = registers[Offset_BenchCycleInterruption],
                BenchCycleEnd = registers[Offset_BenchCycleEnd],
                StartCycle = registers[Offset_StartCycle]
            };
        }

        public ushort[] ToRegisters()
        {
            ushort[] registers = new ushort[6];

            registers[Offset_VepStatus] = VepStatus;
            registers[Offset_VepCycleInterruption] = VepCycleInterruption;
            registers[Offset_VepCycleEnd] = VepCycleEnd;
            registers[Offset_BenchCycleInterruption] = BenchCycleInterruption;
            registers[Offset_BenchCycleEnd] = BenchCycleEnd;
            registers[Offset_StartCycle] = StartCycle;

            return registers;
        }

        public string GetVepStatusString()
        {
            switch (VepStatus)
            {
                case VepStatus_Undefined:
                    return "Undefined";
                case VepStatus_Waiting:
                    return "Waiting";
                case VepStatus_Working:
                    return "Working";
                default:
                    return $"Unknown({VepStatus})";
            }
        }
    }
}
