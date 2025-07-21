using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchDescriptionZone
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

        public VEPBenchDescriptionZone()
        {
            ValidityIndicator = 0;
            StatusZoneAddr = 18;
            StatusZoneSize = 10;
            SynchroZoneAddr = 28;
            SynchroZoneSize = 40;
            TransmissionZoneAddr = 68;
            TransmissionZoneSize = 60;
            ReceptionZoneAddr = 128;
            ReceptionZoneSize = 60;
            AdditionalTZAddr = 1000;
            AdditionalTZSize = 2000;
            AdditionalRZAddr = 3000;
            AdditionalRZSize = 2000;
        }

        public static VEPBenchDescriptionZone FromRegisters(ushort[] registers)
        {
            return new VEPBenchDescriptionZone
            {
                ValidityIndicator = registers[Addr_ValidityIndicator],
                StatusZoneAddr = registers[Addr_StatusZoneAddr],
                StatusZoneSize = registers[Addr_StatusZoneSize],
                SynchroZoneAddr = registers[Addr_SynchroZoneAddr],
                SynchroZoneSize = registers[Addr_SynchroZoneSize],
                TransmissionZoneAddr = registers[Addr_TransmissionZoneAddr],
                TransmissionZoneSize = registers[Addr_TransmissionZoneSize],
                ReceptionZoneAddr = registers[Addr_ReceptionZoneAddr],
                ReceptionZoneSize = registers[Addr_ReceptionZoneSize],
                AdditionalTZAddr = registers[Addr_AdditionalTZAddr],
                AdditionalTZSize = registers[Addr_AdditionalTZSize],
                AdditionalRZAddr = registers[Addr_AdditionalRZAddr],
                AdditionalRZSize = registers[Addr_AdditionalRZSize]
            };
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
