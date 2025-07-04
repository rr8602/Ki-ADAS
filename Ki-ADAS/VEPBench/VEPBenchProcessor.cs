using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchProcessor
    {
        public VEPBenchResponse Process(VEPBenchRequest request)
        {
            if (request.RequestType == VEPRequestType.PJI)
            {
                return ProcessPJI(request);
            }
            else if (request.RequestType == VEPRequestType.Report_ANSI || request.RequestType == VEPRequestType.Report_Unicode)
            {
                return ProcessReport(request);
            }
            else if (request.RequestType == VEPRequestType.OperatorMessage_ANSI || request.RequestType == VEPRequestType.OperatorMessage_Unicode)
            {
                return ProcessOperatorMessage(request);
            }
            else
            {
                Console.WriteLine("[Bench] 알 수 없는 요청 타입: " + request.RequestType);
                return new VEPBenchResponse();
            }
        }

        private VEPBenchResponse ProcessPJI(VEPBenchRequest req)
        {
            string pji = "123456789012";
            Console.WriteLine("[Bench] PJI 요청 수신 -> 응답 : " + pji);
            
            return VEPBenchResponse.CreatePJIResponse(pji);
        }

        private VEPBenchResponse ProcessReport(VEPBenchRequest req)
        {
            Console.WriteLine("[Bench] Report 요청 수신 -> 단순 Ack 응답");

            return VEPBenchResponse.CreateSimpleAck();
        }

        private VEPBenchResponse ProcessOperatorMessage(VEPBenchRequest req)
        {
            int msgLen = req.Raw[0] & 0xFF;
            char[] msgChars = new char[msgLen];

            for (int i = 0; i < msgLen; i++)
            {
                int wordIdx = 12 + (i / 2);
                bool isLow = (i % 2 == 0);
                msgChars[i] = (char)(isLow ? (req.Raw[wordIdx] & 0xFF) : (req.Raw[wordIdx] >> 8));
            }

            string message = new string(msgChars);
            Console.WriteLine("[Bench] 오퍼레이터 메시지 수신 : " + message);

            return VEPBenchResponse.CreateSimpleAck();
        }
    }
}
