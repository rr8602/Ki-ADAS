using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomePositionSimulator
{
    public class StatusArea
    {
        private int cycleValue;
        private int vepStatus;

        public StatusArea()
        {
            cycleValue = 0;
            vepStatus = 0; // 초기 상태는 0 (정지)
        }

        public int CycleValue
        {
            get { return cycleValue; }
            set
            {
                cycleValue = value;

                Console.WriteLine($"상태영역: 시작주기 값이 {value}로 설정됨");
            }
        }

        public int VepStatus
        {
            get { return vepStatus; }
            set
            {
                vepStatus = value;

                Console.WriteLine($"상태영역: VEP 상태가 {value}로 설정됨");
            }
        }
    }
}
