using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class Digital_Input
    {
        public bool bFrontDetect = false;
        public bool bRearDetect = false;
    }
    public class Digital_Output
    {
        public bool bCenterOn = false;
    }

    public class PLC
    {
        Digital_Output DO = new Digital_Output();
        Digital_Input DI = new Digital_Input();
    }


    internal class GlobalVal
    {
        private static readonly Lazy<GlobalVal> _instance = new Lazy<GlobalVal>(() => new GlobalVal());
        public static GlobalVal Instance => _instance.Value;
        private GlobalVal()
        {
            _PLC = new PLC();
        }

        public VEPBenchDataManager _VEP;
        public VEPBenchClient _Client;
        public PLC _PLC;

        public void InitializeVepSystem(string ip, int port)
        {
            _VEP = new VEPBenchDataManager();
            _Client = new VEPBenchClient(ip, port, _VEP);

            _Client.InitializeAndReadDescriptionZone();
        }
    }
}
