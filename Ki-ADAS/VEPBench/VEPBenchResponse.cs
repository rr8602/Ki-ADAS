using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchResponse
    {
        public ushort[] Data { get; set; }

        public VEPBenchResponse()
        {
            Data = new ushort[20];
        }

        public static VEPBenchResponse CreatePJIResponse(string pji)
        {
            var resp = new VEPBenchResponse();
            resp.Data[0] = (ushort)(1 | pji.Length << 8); // ex) 0000 1100 0000 0001

            for (int i = 0; i < pji.Length / 2; i++)
            {
                resp.Data[1 + i] = (ushort)((byte)pji[i * 2] | ((byte)pji[i * 2 + 1] << 8));
            }

            return resp;
        }

        public static VEPBenchResponse CreateSimpleAck()
        {
            var resp = new VEPBenchResponse();
            resp.Data[3] = 2;

            return resp;
        }
    }
}
