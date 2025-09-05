using System;
using System.Drawing;
using System.Windows.Forms;

using Ki_ADAS;
using Ki_ADAS.VEPBench;

namespace HomePositionSimulator
{
    public partial class Form1 : Form
    {
        private VEPBenchClient vepClient;
        private VEPBenchStatusZone statusZone;
        private VEPBenchSynchroZone synchroZone;

        // 시스템 상태 변수
        private bool isHomePosition = true;
        private bool isTrafficLightGreen = false;
        private bool isVehicleDetected = false;
        private bool isPIIRequested = false;
        private bool isVEPWorking = false;
        private bool isCameraOptionSelected = false;
        private bool isRadarOptionSelected = false;
        private int cycleValue = 0;
        private string barcodeValue = "";
        private Timer processTimer;
        private int timeElapsed = 0;
        private int timeoutLimit = 30; // 30초 타임아웃

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetInitialState();
        }

        private void InitializeCustomComponents()
        {
            // 타이머 초기화
            processTimer = new Timer();
            processTimer.Interval = 1000;
            processTimer.Tick += ProcessTimer_Tick;

            vepManager = new VEP();
        }

        private void ProcessTimer_Tick(object sender, EventArgs e)
        {
            timeElapsed++;
            lblTimeElapsed.Text = $"경과 시간: {timeElapsed}초";

            // 타임아웃 체크
            if (timeElapsed > timeoutLimit && !isVEPWorking)
            {
                lblStatus.Text = "상태: 타임아웃 - 프로세스 종료";
                lblStatus.ForeColor = Color.Red;
                processTimer.Stop();
                btnReset.Enabled = true;
            }
        }

        private void SetInitialState()
        {
            // 초기 상태 설정 - Home Position
            isHomePosition = true;
            isTrafficLightGreen = false;
            isVehicleDetected = false;
            isPIIRequested = false;
            isVEPWorking = false;
            isCameraOptionSelected = false;
            isRadarOptionSelected = false;
            cycleValue = 0;
            barcodeValue = "";
            timeElapsed = 0;

            // UI 업데이트
            lblStatus.Text = "상태: 홈 포지션";
            lblStatus.ForeColor = Color.Green;
            lblPositioningDevice.Text = "포지셔닝 장치: 홈";
            lblCameraTarget.Text = "카메라 타겟: 홈";
            lblVehicleDetect.Text = "차량 감지 센서: 꺼짐";

            // 버튼 상태 업데이트
            btnSetTrafficLight.Enabled = true;
            btnScanBarcode.Enabled = false;
            btnRequestPII.Enabled = false;
            btnCheckVEPStatus.Enabled = false;
            btnSelectOption.Enabled = false;
            btnReset.Enabled = true;

            // 상태 패널 초기화
            UpdateStatusPanels(1);

            // 타이머 초기화
            processTimer.Stop();
        }

        private void UpdateStatusPanels(int currentStep)
        {
            // 순서도의 단계에 따라 패널 색상 업데이트
            pnlHomePosition.BackColor = (currentStep >= 1) ? Color.LightGreen : Color.LightGray;
            pnlTrafficLight.BackColor = (currentStep >= 2) ? Color.LightGreen : Color.LightGray;
            pnlVehicleEntrance.BackColor = (currentStep >= 3) ? Color.LightGreen : Color.LightGray;
            pnlScanBarcode.BackColor = (currentStep >= 4) ? Color.LightGreen : Color.LightGray;
            pnlStartCycle.BackColor = (currentStep >= 5) ? Color.LightGreen : Color.LightGray;
            pnlCheckPII.BackColor = (currentStep >= 6) ? Color.LightGreen : Color.LightGray;
            pnlVEPStatus.BackColor = (currentStep >= 7) ? Color.LightGreen : Color.LightGray;
            pnlCameraRadarOption.BackColor = (currentStep >= 8) ? Color.LightGreen : Color.LightGray;
        }

        private void btnSetTrafficLight_Click(object sender, EventArgs e)
        {
            isTrafficLightGreen = true;
            lblStatus.Text = "상태: 입구 신호등 초록색";
            UpdateStatusPanels(2);
            btnSetTrafficLight.Enabled = false;
            btnScanBarcode.Enabled = true;
        }

        private void btnScanBarcode_Click(object sender, EventArgs e)
        {
            // 바코드 스캔 시뮬레이션
            Random random = new Random();
            barcodeValue = $"VIN{random.Next(10000, 99999)}";
            lblStatus.Text = $"상태: 바코드 스캔 완료 - {barcodeValue}";
            isVehicleDetected = true;
            lblVehicleDetect.Text = "차량 감지 센서: 켜짐";
            UpdateStatusPanels(4);
            btnScanBarcode.Enabled = false;

            // 사이클 시작
            cycleValue = 1;
            vepManager.SetCycleValue(cycleValue);
            lblStatus.Text = $"상태: 사이클 시작 값 = {cycleValue}";
            UpdateStatusPanels(5);
            btnRequestPII.Enabled = true;
        }

        private async void btnRequestPII_Click(object sender, EventArgs e)
        {
            isPIIRequested = true;
            lblStatus.Text = "상태: PJI 요청됨";
            UpdateStatusPanels(6);
            btnRequestPII.Enabled = false;
            lblStatus.Text = "상태: PJI가 VEP로 전송됨";
            btnCheckVEPStatus.Enabled = true;

            // VEP로 PJI 전송
            await vepManager.ProcessPJI(barcodeValue);

            // 타이머 시작
            timeElapsed = 0;
            processTimer.Start();
        }

        private void btnCheckVEPStatus_Click(object sender, EventArgs e)
        {
            // VEP 상태 확인
            isVEPWorking = chkVEPWorking.Checked;
            int vepStatus = vepManager.GetVepStatus();

            if (isVEPWorking)
            {
                lblStatus.Text = "상태: VEP 작동 중";
                UpdateStatusPanels(7);
                btnCheckVEPStatus.Enabled = false;
                btnSelectOption.Enabled = true;
                processTimer.Stop();

                int synchroValue = vepManager.GetSyncroValue();

                if (synchroValue == 1)
                {
                    radioCamera.Checked = true;
                }
                else if (synchroValue == 3)
                {
                    radioRadar.Checked = true;
                }
            }
            else if (timeElapsed > timeoutLimit)
            {
                lblStatus.Text = "상태: VEP 타임아웃 - 프로세스 종료";
                lblStatus.ForeColor = Color.Red;
                processTimer.Stop();
            }
            else
            {
                lblStatus.Text = $"상태: VEP 미작동, 대기 중... ({timeElapsed}초)";
            }
        }

        private void btnSelectOption_Click(object sender, EventArgs e)
        {
            string deviceType = vepManager.GetSelectedDeviceType();

            if (radioCamera.Checked)
            {
                isCameraOptionSelected = true;
                isRadarOptionSelected = false;
                lblStatus.Text = $"상태: 카메라 옵션 선택됨 - {deviceType}";
            }
            else if (radioRadar.Checked)
            {
                isCameraOptionSelected = false;
                isRadarOptionSelected = true;
                lblStatus.Text = $"상태: 레이더 옵션 선택됨 - {deviceType}";
            }
            else
            {
                MessageBox.Show("Please select an option.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UpdateStatusPanels(8);
            btnSelectOption.Enabled = false;
            lblStatus.Text += " - 프로세스 완료";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SetInitialState();
        }

        private void btnMoveCarToBench_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "상태: 차량이 벤치로 이동 중";
            MessageBox.Show("Move the vehicle to the bench.", "Guidance", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}