using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS
{
    public class VEP_Data
    {
        public string aa = "";
    }

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
            _VEP = new VEP_Data();
            _PLC = new PLC();
        }
        public VEP_Data _VEP;
        public PLC _PLC;


    }
}
