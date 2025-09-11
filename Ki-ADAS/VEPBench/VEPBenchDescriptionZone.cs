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

        // 속성값
        public ushort ValidityIndicator { get; set; }
        public ushort StatusZoneAddr { get; set; }
        public ushort StatusZoneSize { get; set; }
        public ushort SynchroZoneAddr { get; set; }
        public ushort SynchroZoneSize { get; set; }
        public ushort TransmissionZoneAddr { get; set; }
        public ushort TransmissionZoneSize { get; set; }
        public ushort ReceptionZoneAddr { get; set; }
        public ushort ReceptionZoneSize { get; set; }
        public ushort AdditionalTZAddr { get; set; }
        public ushort AdditionalTZSize { get; set; }
        public ushort AdditionalRZAddr { get; set; }
        public ushort AdditionalRZSize { get; set; }

        private bool _isChanged;
        public bool IsChanged => _isChanged;

        public ushort Length => 18;

        public void ResetChangedState()
        {
            _isChanged = false;
        }

        public VEPBenchDescriptionZone()
        {
            _isChanged = false;
        }

        public void FromRegisters(ushort[] registers)
        {
            bool changed = false;

            if (ValidityIndicator != registers[Addr_ValidityIndicator]) { ValidityIndicator = registers[Addr_ValidityIndicator]; changed = true; }
            if (StatusZoneAddr != registers[Addr_StatusZoneAddr]) { StatusZoneAddr = registers[Addr_StatusZoneAddr]; changed = true; }
            if (StatusZoneSize != registers[Addr_StatusZoneSize]) { StatusZoneSize = registers[Addr_StatusZoneSize]; changed = true; }
            if (SynchroZoneAddr != registers[Addr_SynchroZoneAddr]) { SynchroZoneAddr = registers[Addr_SynchroZoneAddr]; changed = true; }
            if (SynchroZoneSize != registers[Addr_SynchroZoneSize]) { SynchroZoneSize = registers[Addr_SynchroZoneSize]; changed = true; }
            if (TransmissionZoneAddr != registers[Addr_TransmissionZoneAddr]) { TransmissionZoneAddr = registers[Addr_TransmissionZoneAddr]; changed = true; }
            if (TransmissionZoneSize != registers[Addr_TransmissionZoneSize]) { TransmissionZoneSize = registers[Addr_TransmissionZoneSize]; changed = true; }
            if (ReceptionZoneAddr != registers[Addr_ReceptionZoneAddr]) { ReceptionZoneAddr = registers[Addr_ReceptionZoneAddr]; changed = true; }
            if (ReceptionZoneSize != registers[Addr_ReceptionZoneSize]) { ReceptionZoneSize = registers[Addr_ReceptionZoneSize]; changed = true; }
            if (AdditionalTZAddr != registers[Addr_AdditionalTZAddr]) { AdditionalTZAddr = registers[Addr_AdditionalTZAddr]; changed = true; }
            if (AdditionalTZSize != registers[Addr_AdditionalTZSize]) { AdditionalTZSize = registers[Addr_AdditionalTZSize]; changed = true; }
            if (AdditionalRZAddr != registers[Addr_AdditionalRZAddr]) { AdditionalRZAddr = registers[Addr_AdditionalRZAddr]; changed = true; }
            if (AdditionalRZSize != registers[Addr_AdditionalRZSize]) { AdditionalRZSize = registers[Addr_AdditionalRZSize]; changed = true; }

            if (changed)
            {
                _isChanged = true;
            }
        }

        public ushort[] ToRegisters()
        {
            ushort[] registers = new ushort[18];

            registers[Addr_ValidityIndicator] = ValidityIndicator;
            registers[Addr_StatusZoneAddr] = StatusZoneAddr;
            registers[Addr_StatusZoneSize] = StatusZoneSize;
            registers[Addr_SynchroZoneAddr] = SynchroZoneAddr;
            registers[Addr_SynchroZoneSize] = SynchroZoneSize;
            registers[Addr_TransmissionZoneAddr] = TransmissionZoneAddr;
            registers[Addr_TransmissionZoneSize] = TransmissionZoneSize;
            registers[Addr_ReceptionZoneAddr] = ReceptionZoneAddr;
            registers[Addr_ReceptionZoneSize] = ReceptionZoneSize;
            registers[Addr_AdditionalTZAddr] = AdditionalTZAddr;
            registers[Addr_AdditionalTZSize] = AdditionalTZSize;
            registers[Addr_AdditionalRZAddr] = AdditionalRZAddr;
            registers[Addr_AdditionalRZSize] = AdditionalRZSize;

            return registers;
        }
    }
}
