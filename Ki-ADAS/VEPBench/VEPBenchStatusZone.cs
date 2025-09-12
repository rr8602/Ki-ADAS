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

        public const int Offset_VepStatus = 0;
        public const int Offset_VepCycleInterruption = 1;
        public const int Offset_VepCycleEnd = 2;
        public const int Offset_BenchCycleInterruption = 3;
        public const int Offset_BenchCycleEnd = 4;
        public const int Offset_StartCycle = 5;
        public const int ZoneSize = 6;

        public const ushort VepStatus_Undefined = 0;
        public const ushort VepStatus_Waiting = 1;
        public const ushort VepStatus_Working = 2;

        private ushort[] _values;

        public ushort VepStatus
        {
            get => _values[Offset_VepStatus];
            set
            {
                if (_values[Offset_VepStatus] != value)
                {
                    _values[Offset_VepStatus] = value;
                    _isChanged = true;
                }
            }
        }

        public ushort VepCycleInterruption
        {
            get => _values[Offset_VepCycleInterruption];
            set
            {
                if (_values[Offset_VepCycleInterruption] != value)
                {
                    _values[Offset_VepCycleInterruption] = value;
                    _isChanged = true;
                }
            }
        }

        public ushort VepCycleEnd
        {
            get => _values[Offset_VepCycleEnd];
            set
            {
                if (_values[Offset_VepCycleEnd] != value)
                {
                    _values[Offset_VepCycleEnd] = value;
                    _isChanged = true;
                }
            }
        }

        public ushort BenchCycleInterruption
        {
            get => _values[Offset_BenchCycleInterruption];
            set
            {
                if (_values[Offset_BenchCycleInterruption] != value)
                {
                    _values[Offset_BenchCycleInterruption] = value;
                    _isChanged = true;
                }
            }
        }

        public ushort BenchCycleEnd
        {
            get => _values[Offset_BenchCycleEnd];
            set
            {
                if (_values[Offset_BenchCycleEnd] != value)
                {
                    _values[Offset_BenchCycleEnd] = value;
                    _isChanged = true;
                }
            }
        }

        public ushort StartCycle
        {
            get => _values[Offset_StartCycle];
            set
            {
                if (_values[Offset_StartCycle] != value)
                {
                    _values[Offset_StartCycle] = value;
                    _isChanged = true;
                }
            }
        }

        private bool _isChanged;
        public bool IsChanged => _isChanged;

        public void ResetChangedState()
        {
            _isChanged = false;
        }

        public ushort this[int index]
        {
            get
            {
                if (index < 0 || index >= (_values?.Length ?? 0))
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchStatusZone values.");
                return _values[index];
            }
            set
            {
                if (index < 0 || index >= (_values?.Length ?? 0))
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchStatusZone values.");
                if (_values[index] != value)
                {
                    _values[index] = value;
                    _isChanged = true;
                }
            }
        }

        public void SetValue(int index, ushort value)
        {
            this[index] = value;
        }

        public int GetValue(int index)
        {
            return this[index];
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
            _values = new ushort[ZoneSize];
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
            if (registers == null || registers.Length < ZoneSize)
                throw new ArgumentException("Invalid register array.");

            if (_values == null || !_values.SequenceEqual(registers))
            {
                _values = (ushort[])registers.Clone();
                _isChanged = true;
            }
        }

        public ushort[] ToRegisters()
        {
            ushort[] result = new ushort[_values.Length];
            Array.Copy(_values, result, _values.Length);
            return result;
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
