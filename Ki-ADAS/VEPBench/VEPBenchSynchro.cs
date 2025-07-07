using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchSynchro
    {
        public double Angle1 { get; set; } // roll
        public double Angle2 { get; set; } // azimuth/horizontal
        public double Angle3 { get; set; } // vertical/elevation

        public static VEPBenchSynchro FormUshortArray(ushort[] arr)
        {
            var s = new VEPBenchSynchro();
            s.Angle1 = arr.Length > 0 ? arr[0] / 100.0 : 0.0;
            s.Angle2 = arr.Length > 1 ? arr[1] / 100.0 : 0.0;
            s.Angle3 = arr.Length > 2 ? arr[2] / 100.0 : 0.0;

            return s;
        }

        public ushort[] ToUshortArray()
        {
            ushort[] arr = new ushort[3];
            arr[0] = (ushort)(Angle1 * 100);
            arr[1] = (ushort)(Angle2 * 100);
            arr[2] = (ushort)(Angle3 * 100);

            return arr;
        }
    }
}
