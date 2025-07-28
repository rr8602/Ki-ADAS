using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomePositionSimulator
{
    public class SyncArea
    {
        private int synchroValue;

        public SyncArea()
        {
            synchroValue = 0;
        }

        public int Synchrovalue
        {
            get { return synchroValue; }
            set
            {
                synchroValue = value;

                Console.WriteLine($"동기화영역: 동기화 값이 {value}로 설정됨");
            }
        }

        public string GetDeviceType()
        {
            switch (synchroValue)
            {
                case 1:
                    return "Synchro S1+ (Front Camera)";

                case 3:
                    return "Synchro S3 (Rear Radar)";

                default:
                    return "Unknown Device";
            }
        }
    }
}
