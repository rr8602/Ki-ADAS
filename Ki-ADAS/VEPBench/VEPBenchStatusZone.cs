using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchStatusZone : IVEPBenchZone
    {
        private static VEPBenchStatusZone _instance;
        private static readonly object _lock = new object();

        public static int Offset_VepStatus = 0;
        public static int Offset_VepCycleInterruption = 1;
        public static int Offset_VepCycleEnd = 2;
        public static int Offset_BenchCycleInterruption = 3;
        public static int Offset_BenchCycleEnd = 4;
        public static int Offset_StartCycle = 5;

        public const ushort VepStatus_Undefined = 0;
        public const ushort VepStatus_Waiting = 1;
        public const ushort VepStatus_Working = 2;

        public ushort VepStatus { get; set; }
        public ushort VepCycleInterruption { get; set; }
        public ushort VepCycleEnd { get; set; }
        public ushort BenchCycleInterruption { get; set; }
        public ushort BenchCycleEnd { get; set; }
        public ushort StartCycle { get; set; }

        private bool _isChanged;
        public bool IsChanged => _isChanged;

        public void ResetChangedState()
        {
            _isChanged = false;
        }

        private ushort[] _values;

        public ushort this[int index]
        {
            get
            {
                if (index < 0 || index >= (_values?.Length ?? 0))
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchSynchro values.");

                return _values[index];
            }
            set
            {
                if (index < 0 || index >= (_values?.Length ?? 0))
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchSynchro values.");

                _values[index] = value;
            }
        }

        public void SetValue(int index, ushort value)
        {
            this[index] = value;
        }

        public static VEPBenchStatusZone Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new VEPBenchStatusZone();
                    }
                }

                return _instance;
            }
        }

        public VEPBenchStatusZone()
        {
            VepStatus = VepStatus_Waiting;
            VepCycleInterruption = 0;
            VepCycleEnd = 0;
            BenchCycleInterruption = 0;
            BenchCycleEnd = 0;
            StartCycle = 0;
            _isChanged = false;
        }

        public void FromRegisters(ushort[] registers)
        {
            if (registers == null || registers.Length < 6)
                throw new ArgumentException("Invalid register array.");

            bool changed = false;

            if (VepStatus != registers[Offset_VepStatus]) { VepStatus = registers[Offset_VepStatus]; changed = true; }
            if (VepCycleInterruption != registers[Offset_VepCycleInterruption]) { VepCycleInterruption = registers[Offset_VepCycleInterruption]; changed = true; }
            if (VepCycleEnd != registers[Offset_VepCycleEnd]) { VepCycleEnd = registers[Offset_VepCycleEnd]; changed = true; }
            if (BenchCycleInterruption != registers[Offset_BenchCycleInterruption]) { BenchCycleInterruption = registers[Offset_BenchCycleInterruption]; changed = true; }
            if (BenchCycleEnd != registers[Offset_BenchCycleEnd]) { BenchCycleEnd = registers[Offset_BenchCycleEnd]; changed = true; }
            if (StartCycle != registers[Offset_StartCycle]) { StartCycle = registers[Offset_StartCycle]; changed = true; }

            if (changed)
            {
                _isChanged = true;
            }
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