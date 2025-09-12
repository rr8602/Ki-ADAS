using Snap7;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchDescriptionZone : IVEPBenchZone
    {
        // 주소값
        public const int Addr_ValidityIndicator = 0;
        public const int Addr_StatusZoneAddr = 1;
        public const int Addr_StatusZoneSize = 2;
        public const int Addr_SynchroZoneAddr = 3;
        public const int Addr_SynchroZoneSize = 4;
        public const int Addr_TransmissionZoneAddr = 10;
        public const int Addr_TransmissionZoneSize = 11;
        public const int Addr_ReceptionZoneAddr = 12;
        public const int Addr_ReceptionZoneSize = 13;
        public const int Addr_AdditionalTZAddr = 14;
        public const int Addr_AdditionalTZSize = 15;
        public const int Addr_AdditionalRZAddr = 16;
        public const int Addr_AdditionalRZSize = 17;

        private ushort[] _values;

        // 속성값
        public ushort ValidityIndicator => _values[Addr_ValidityIndicator];
        public ushort StatusZoneAddr => _values[Addr_StatusZoneAddr];
        public ushort StatusZoneSize => _values[Addr_StatusZoneSize];
        public ushort SynchroZoneAddr => _values[Addr_SynchroZoneAddr];
        public ushort SynchroZoneSize => _values[Addr_SynchroZoneSize];
        public ushort TransmissionZoneAddr => _values[Addr_TransmissionZoneAddr];
        public ushort TransmissionZoneSize => _values[Addr_TransmissionZoneSize];
        public ushort ReceptionZoneAddr => _values[Addr_ReceptionZoneAddr];
        public ushort ReceptionZoneSize => _values[Addr_ReceptionZoneSize];
        public ushort AdditionalTZAddr => _values[Addr_AdditionalTZAddr];
        public ushort AdditionalTZSize => _values[Addr_AdditionalTZSize];
        public ushort AdditionalRZAddr => _values[Addr_AdditionalRZAddr];
        public ushort AdditionalRZSize => _values[Addr_AdditionalRZSize];

        private bool _isChanged;
        public bool IsChanged => _isChanged;

        public ushort Length => 18;

        public void ResetChangedState()
        {
            _isChanged = false;
        }

        public VEPBenchDescriptionZone()
        {
            _values = new ushort[Length];
        }

        public void FromRegisters(ushort[] registers)
        {
            if (registers == null)
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
    }
}
