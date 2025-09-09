using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchDataManager
    {
        private static VEPBenchDataManager _instance;
        private static readonly object _lock = new object();

        public VEPBenchDescriptionZone DescriptionZone { get; private set; }
        public VEPBenchReceptionZone ReceptionZone { get; private set; }
        public VEPBenchStatusZone StatusZone { get; private set; }
        public VEPBenchSynchroZone SynchroZone { get; internal set; }
        public VEPBenchTransmissionZone TransmissionZone { get; private set; }

        public static VEPBenchDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new VEPBenchDataManager();
                    }
                }

                return _instance;
            }
        }

        public VEPBenchDataManager()
        {
            DescriptionZone = new VEPBenchDescriptionZone();
            ReceptionZone = VEPBenchReceptionZone.Instance;
            StatusZone = VEPBenchStatusZone.Instance;
            SynchroZone = VEPBenchSynchroZone.Instance;
            TransmissionZone = VEPBenchTransmissionZone.Instance;
        }

        public VEPBenchSynchroZone RefreshSynchroZoneFromVEP(VEPBenchClient client)
        {
            if (client != null && client.IsConnected)
            {
                return VEPBenchSynchroZone.ReadFromVEP((start, count) => client.ReadSynchroZone(start, count));
            }

            return null;
        }

        public void UpdateAllZonesFromRegisters(Func<int, int, ushort[]> readRegistersFunc)
        {
            StatusZone.FromRegisters(readRegistersFunc(DescriptionZone.StatusZoneAddr, DescriptionZone.StatusZoneSize));

            ushort[] synchroPart1 = readRegistersFunc(DescriptionZone.SynchroZoneAddr, VEPBenchSynchroZone.SYNCHRO_SIZE_PART1);
            ushort[] synchroPart2 = readRegistersFunc(DescriptionZone.SynchroZoneAddr + VEPBenchSynchroZone.SYNCHRO_SIZE_PART1, VEPBenchSynchroZone.SYNCHRO_SIZE_PART2);
            ushort[] allSynchroRegisters = new ushort[VEPBenchSynchroZone.DEFAULT_SYNCHRO_SIZE];
            Array.Copy(synchroPart1, 0, allSynchroRegisters, 0, VEPBenchSynchroZone.SYNCHRO_SIZE_PART1);
            Array.Copy(synchroPart2, 0, allSynchroRegisters, VEPBenchSynchroZone.SYNCHRO_SIZE_PART1, VEPBenchSynchroZone.SYNCHRO_SIZE_PART2);
            SynchroZone.FromRegisters(allSynchroRegisters);

            TransmissionZone.FromRegisters(readRegistersFunc(DescriptionZone.TransmissionZoneAddr, DescriptionZone.TransmissionZoneSize));
            ReceptionZone.FromRegisters(readRegistersFunc(DescriptionZone.ReceptionZoneAddr, DescriptionZone.ReceptionZoneSize));
        }

        public void WriteAllZonesToRegisters(Action<int, ushort[]> writeRegistersFunc)
        {
            writeRegistersFunc(DescriptionZone.StatusZoneAddr, StatusZone.ToRegisters());

            ushort[] synchroRegisters = SynchroZone.ToRegisters();
            writeRegistersFunc(DescriptionZone.SynchroZoneAddr, synchroRegisters.Take(VEPBenchSynchroZone.SYNCHRO_SIZE_PART1).ToArray());
            writeRegistersFunc(DescriptionZone.SynchroZoneAddr + VEPBenchSynchroZone.SYNCHRO_SIZE_PART1, synchroRegisters.Skip(VEPBenchSynchroZone.SYNCHRO_SIZE_PART1).ToArray());

            writeRegistersFunc(DescriptionZone.TransmissionZoneAddr, TransmissionZone.ToRegisters());
        }
    }
}
