using System;
using System.Threading.Tasks;

namespace HomePositionSimulator
{
    public class VEP
    {
        private StatusArea statusArea;
        private SyncArea syncArea;
        private bool isWorking;

        public VEP()
        {
            statusArea = new StatusArea();
            syncArea = new SyncArea();
            isWorking = false;
        }

        public async Task<bool> ProcessPJI(string barcodeValue)
        {
            Console.WriteLine($"VEP: {barcodeValue}에 대한 PJI 처리 시작");

            // PJI 처리 시뮬레이션 (실제로는 여기서 외부 시스템과 통신)
            await Task.Delay(2000);

            // PJI 처리 성공 시 상태 변경
            isWorking = true;
            statusArea.VepStatus = 2; // VEP 상태를 2로 설정

            // 랜덤하게 카메라 또는 레이더 선택
            Random random = new Random();
            int option = random.Next(2);

            if (option == 0)
            {
                syncArea.Synchrovalue = 1; // Syncro S1+ (Front Camera)
            }
            else
            {
                syncArea.Synchrovalue = 3; // Syncro S3+ (Rear Radar)
            }

            return true;
        }

        public bool IsWorking()
        {
            return isWorking;
        }

        public int GetVepStatus()
        {
            return statusArea.VepStatus;
        }

        public string GetSelectedDeviceType()
        {
            return syncArea.GetDeviceType();
        }

        public int GetSyncroValue()
        {
            return syncArea.Synchrovalue;
        }

        public void SetCycleValue(int value)
        {
            statusArea.CycleValue = value;
        }
    }
}