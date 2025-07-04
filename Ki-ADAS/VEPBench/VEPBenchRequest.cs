using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public enum VEPRequestType
    {
        Unknown = 0,
        PJI = 6,
        Report_ANSI = 15,
        Report_Unicode = 16,
        OperatorMessage_ANSI = 20,
        OperatorMessage_Unicode = 21
    }

    public class VEPBenchRequest
    {
        public VEPRequestType RequestType { get; private set; }
        public ushort[] Raw { get; private set; }
        public byte FctCode { get; private set; }
        public byte ProcessCode { get; private set; }
        public byte subFctCode { get; private set; }

        public VEPBenchRequest(ushort[] tx)
        {
            Raw = tx;
            FctCode = (byte)(tx[6] & 0xFF);
            ProcessCode = (byte)(tx[8] & 0xFF);
            subFctCode = (byte)((tx[8] >> 8) & 0xFF);

            switch (FctCode)
            {
                case 6:
                    RequestType = VEPRequestType.PJI;
                    break;

                case 15:
                    RequestType = VEPRequestType.Report_ANSI;
                    break;

                case 16:
                    RequestType = VEPRequestType.Report_Unicode;
                    break;

                case 20:
                    RequestType = VEPRequestType.OperatorMessage_ANSI;
                    break;

                case 21:
                    RequestType = VEPRequestType.OperatorMessage_Unicode;
                    break;

                default:
                    RequestType = VEPRequestType.Unknown;
                    break;
            }
        }
    }
}
