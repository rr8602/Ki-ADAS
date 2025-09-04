using System;

namespace Ki_ADAS.VEPBench
{
    public interface IVEPBenchZone
    {
        bool IsChanged { get; }
        void ResetChangedState();
        ushort[] ToRegisters();
        void FromRegisters(ushort[] registers);
    }
}
