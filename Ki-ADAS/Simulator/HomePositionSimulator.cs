using Ki_ADAS;
using Ki_ADAS.VEPBench;

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace Ki_ADAS
{
    public partial class HomePositionSimulator : Form
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
        private bool isExitPositionReady = false;
        private bool isTestCompleted = false;
        private int cycleValue = 0;
        private string barcodeValue = "";
        private DateTime meaDateValue;
        private Timer processTimer;
        private int timeElapsed = 0;
        private int timeoutLimit = 30; // 30초 타임아웃

        private enum CalibrationTarget
        {
            None,
            FrontCamera,
            RightRearRadar,
            LeftRearRadar
        }

        private CalibrationTarget currentTarget = CalibrationTarget.None;
        private bool isTestPositionReady = false;
        private bool isCalibrationMode = false;

        public HomePositionSimulator(VEPBenchClient client)
        {
            InitializeComponent();
            vepClient = client;
            InitializeCustomComponents();
            SetInitialState();
        }

        private void InitializeCustomComponents()
        {
            // 타이머 초기화
            processTimer = new Timer();
            processTimer.Interval = 1000;
            processTimer.Tick += ProcessTimer_Tick;

            try
            {
                if (vepClient != null && vepClient.IsConnected)
                {
                    statusZone = vepClient.ReadStatusZone();
                    synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));
                }
                else
                {
                    MessageBox.Show("VEP 클라이언트가 연결되어 있지 않습니다. 시뮬레이터를 실행하기 전에 Start 버튼을 눌러주세요.",
                        "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"VEP 클라이언트 초기화 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            currentTarget = CalibrationTarget.None;
            isTestPositionReady = false;
            isCalibrationMode = false;

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

            try
            {
                if (vepClient != null && vepClient.IsConnected)
                {
                    statusZone = vepClient.ReadStatusZone();
                    statusZone.StartCycle = 0;
                    statusZone.VepStatus = VEPBenchStatusZone.VepStatus_Undefined;
                    vepClient.WriteStatusZone(statusZone);

                    synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                    for (int i = 0; i < synchroZone.Size; i++)
                    {
                        synchroZone.SetValue(i, 0);
                    }

                    vepClient.WriteSynchroZone(synchroZone);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"VEP 상태 초기화 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            pnlCheckPJI.BackColor = (currentStep >= 6) ? Color.LightGreen : Color.LightGray;
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

            if (vepClient != null && vepClient.IsConnected)
            {
                statusZone = vepClient.ReadStatusZone();
                statusZone.StartCycle = (ushort)cycleValue;
                vepClient.WriteStatusZone(statusZone);
            }

            lblStatus.Text = $"상태: 사이클 시작 값 = {cycleValue}";
            UpdateStatusPanels(5);
            btnRequestPII.Enabled = true;
        }

        private async void btnRequestPJI_Click(object sender, EventArgs e)
        {
            isPIIRequested = true;
            lblStatus.Text = "상태: PJI 요청됨";
            UpdateStatusPanels(6);
            btnRequestPII.Enabled = false;
            lblStatus.Text = "상태: PJI가 VEP로 전송됨";
            btnCheckVEPStatus.Enabled = true;

            if (vepClient != null && vepClient.IsConnected)
            {
                statusZone.StartCycle = 0;
                statusZone = vepClient.ReadStatusZone();
                statusZone.VepStatus = VEPBenchStatusZone.VepStatus_Working;
                vepClient.WriteStatusZone(statusZone);

                synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));
                Random random = new Random();
                int[] synchroIndices = { VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX };
                int selectedSynchroIndex = synchroIndices[random.Next(synchroIndices.Length)]; // 1: 전방 카메라, 2: 우측 후방 레이더, 3: 좌측 후방 레이더

                synchroZone.SetValue(selectedSynchroIndex, 1);
                vepClient.WriteSynchroZone(synchroZone);
            }

            lblStatus.Text = "상태: PJI가 VEP로 전송됨";
            btnCheckVEPStatus.Enabled = true;

            await Task.Delay(1000);

            // 타이머 시작
            timeElapsed = 0;
            processTimer.Start();
        }

        private void btnCheckVEPStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (vepClient != null && vepClient.IsConnected)
                {
                    statusZone = vepClient.ReadStatusZone();
                    isVEPWorking = (statusZone.VepStatus == VEPBenchStatusZone.VepStatus_Working);
                }
                else
                {
                    isVEPWorking = chkVEPWorking.Checked;
                }

                if (isVEPWorking)
                {
                    lblStatus.Text = "상태: VEP 작업 중";
                    UpdateStatusPanels(7);
                    btnCheckVEPStatus.Enabled = false;
                    btnSelectOption.Enabled = true;
                    processTimer.Stop();

                    synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                    if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1)
                    {
                        radioCamera.Checked = true;
                        radioRearRightRadar.Checked = false;
                        radioRearLeftRadar.Checked = false;
                    }
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1)
                    {
                        radioCamera.Checked = false;
                        radioRearRightRadar.Checked = true;
                        radioRearLeftRadar.Checked = false;
                    }
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1)
                    {
                        radioCamera.Checked = false;
                        radioRearRightRadar.Checked = false;
                        radioRearLeftRadar.Checked = true;
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
            catch (Exception ex)
            {
                MessageBox.Show($"VEP 상태 확인 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSelectOption_Click(object sender, EventArgs e)
        {
            try
            {
                string deviceType = "알 수 없음";

                if (vepClient != null && vepClient.IsConnected)
                {
                    synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                    if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1)
                    {
                        deviceType = "카메라";
                    }
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1)
                    {
                        deviceType = "우측 후방 레이더";
                    }
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1)
                    {
                        deviceType = "좌측 후방 레이더";
                    }

                    if (radioCamera.Checked)
                    {
                        isCameraOptionSelected = true;
                        isRadarOptionSelected = false;
                        lblStatus.Text = $"상태: 전방 카메라 옵션 선택됨 - {deviceType}";
                    }
                    else if (radioRearRightRadar.Checked)
                    {
                        isCameraOptionSelected = false;
                        isRadarOptionSelected = true;
                        lblStatus.Text = $"상태: 우측 후방 레이더 옵션 선택됨 - {deviceType}";
                    }
                    else if (radioRearLeftRadar.Checked)
                    {
                        isCameraOptionSelected = false;
                        isRadarOptionSelected = true;
                        lblStatus.Text = $"상태: 좌측 후방 레이더 옵션 선택됨 - {deviceType}";
                    }
                    else
                    {
                        MessageBox.Show("옵션을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    UpdateStatusPanels(8);
                    btnSelectOption.Enabled = false;
                    lblStatus.Text += " - 프로세스 완료";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"옵션 선택 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SetInitialState();
        }

        private void btnMoveCarToBench_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "상태: 차량이 벤치로 이동 중";
            MessageBox.Show("차량을 벤치로 이동하세요.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            lblTestStatusValue.Text = "테스트 사이클 시작";
            btnSetTestPosition.Enabled = true;
        }

        private void btnSetTestPosition_Click(object sender, EventArgs e)
        {
            lblTestStatusValue.Text = "테스트 포지션 설정 중";
            MessageBox.Show("센터링 장치 이동, 카메라 타겟 하향, 후방 측면 레이더가 이동합니다.", "테스트 포지션", MessageBoxButtons.OK, MessageBoxIcon.Information);

            isTestPositionReady = true;
            lblTestStatusValue.Text = "테스트 포지션 설정 완료";

            radioCalFrontCamera.Enabled = true;
            radioCalRearRightRadar.Enabled = true;
            radioCalRearLeftRadar.Enabled = true;

            btnSendToVEP.Enabled = true;
        }

        private void btnSendToVEP_Click(object sender, EventArgs e)
        {
            ADASProcess process = new ADASProcess(vepClient);

            try
            {
                if (vepClient != null && vepClient.IsConnected)
                {
                    synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));
                    CalibrationTarget detectedTarget = CalibrationTarget.None;

                    if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1)
                    {
                        detectedTarget = CalibrationTarget.FrontCamera;
                        radioCalFrontCamera.Checked = true;
                        radioCalRearRightRadar.Checked = false;
                        radioCalRearLeftRadar.Checked = false;
                        lblTargetValue.Text = "전방 카메라";
                        lblTestStatusValue.Text = "전방 카메라 타겟 설정됨";
                    }
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1)
                    {
                        detectedTarget = CalibrationTarget.RightRearRadar;
                        radioCalFrontCamera.Checked = false;
                        radioCalRearRightRadar.Checked = true;
                        radioCalRearLeftRadar.Checked = false;
                        lblTargetValue.Text = "우측 후방 레이더";
                        lblTestStatusValue.Text = "우측 후방 레이더 타겟 설정됨";
                    }
                    else if (synchroZone.GetValue(VEPBenchSynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1)
                    {
                        detectedTarget = CalibrationTarget.LeftRearRadar;
                        radioCalFrontCamera.Checked = false;
                        radioCalRearRightRadar.Checked = false;
                        radioCalRearLeftRadar.Checked = true;
                        lblTargetValue.Text = "좌측 후방 레이더";
                        lblTestStatusValue.Text = "좌측 후방 레이더 타겟 설정됨";
                    }
                    else
                    {
                        MessageBox.Show("VEP에서 유효한 타겟을 감지할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    currentTarget = detectedTarget;

                    switch (currentTarget)
                    {
                        case CalibrationTarget.FrontCamera:
                            synchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_FRONT_CAMERA_INDEX, 1);
                            lblTargetValue.Text = "전방 카메라";
                            lblTestStatusValue.Text = "전방 카메라 타겟(VEP Synchro 4 = 1) 설정";
                            vepClient.WriteSynchroZone(synchroZone);
                            process.InitializeFrontCameraSteps();
                            break;
                        case CalibrationTarget.RightRearRadar:
                            synchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_RIGHT_RADAR_INDEX, 1);
                            lblTargetValue.Text = "우측 후방 레이더";
                            lblTestStatusValue.Text = "우측 후방 레이더 타겟(VEP Synchro 52 = 1) 설정";
                            vepClient.WriteSynchroZone(synchroZone);
                            process.InitializeRightRearRadarSteps();
                            break;
                        case CalibrationTarget.LeftRearRadar:
                            synchroZone.SetValue(VEPBenchSynchroZone.SYNC_COMMAND_REAR_LEFT_RADAR_INDEX, 1);
                            lblTargetValue.Text = "좌측 후방 레이더";
                            lblTestStatusValue.Text = "좌측 후방 레이더 타겟(VEP Synchro 54 = 1) 설정";
                            vepClient.WriteSynchroZone(synchroZone);
                            process.InitializeLeftRearRadarSteps();
                            break;
                    }

                    btnCheckTestFinish.Enabled = true;
                    timeElapsed = 0;
                    processTimer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"VEP에 데이터 전송 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCheckTestFinish_Click(object sender, EventArgs e)
        {
            // 실제는 VEP에서 상태 확인 필요 (여기서는 5초 이상 경과하면 테스트 완료 처리)
            if (timeElapsed >= 5)
            {
                lblTestStatusValue.Text = "테스트 완료 확인 중...";
                btnReadResults.Enabled = true;
            }
            else
            {
                MessageBox.Show($"테스트가 아직 진행 중입니다. ({timeElapsed}초 경과)", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReadResults_Click(object sender, EventArgs e)
        {
            try
            {
                if (vepClient != null && vepClient.IsConnected)
                {
                    synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));

                    string resultText = "";
                    double frontCameraAngle1 = 0;
                    double frontCameraAngle2 = 0;
                    double frontCameraAngle3 = 0;
                    double rearRightRadarAngle = 0;
                    double rearLeftRadarAngle = 0;

                    switch (currentTarget)
                    {
                        case CalibrationTarget.FrontCamera:
                            frontCameraAngle1 = synchroZone.FrontCameraAngle1;
                            frontCameraAngle2 = synchroZone.FrontCameraAngle2;
                            frontCameraAngle3 = synchroZone.FrontCameraAngle3;

                            resultText = $"Roll 각도: {frontCameraAngle1:F2}°\nSynchro 110: {synchroZone.GetValue(110)}\n" +
                                         $"Azimuth 각도: {frontCameraAngle2:F2}°\nSynchro 111: {synchroZone.GetValue(111)}\n" +
                                         $"Elevation 각도: {frontCameraAngle3:F2}°\nSynchro 112: {synchroZone.GetValue(112)}";
                            break;

                        case CalibrationTarget.RightRearRadar:
                            rearRightRadarAngle = synchroZone.RearRightRadarAngle;
                            resultText = $"각도: {rearRightRadarAngle:F2}°\nSynchro 115: {synchroZone.GetValue(115)}";
                            break;

                        case CalibrationTarget.LeftRearRadar:
                            rearLeftRadarAngle = synchroZone.RearLeftRadarAngle;
                            resultText = $"각도: {rearLeftRadarAngle:F2}°\nSynchro 116: {synchroZone.GetValue(116)}";
                            break;
                    }

                    lblTestResultValue.Text = resultText;
                    lblTestStatusValue.Text = "테스트 결과 읽기 완료";
                    isTestCompleted = true;
                }
                else
                {
                    // 실제에서는 이 부분 실행되면 안됨
                    lblTestStatusValue.Text = "VEP 연결 안됨 - 각도값을 읽을 수 없습니다";
                    lblTestResultValue.Text = "연결 실패";
                    MessageBox.Show("VEP에 연결되어 있지 않아 각도값을 읽을 수 없습니다.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                btnReset.Enabled = true;

                if (isTestCompleted) 
                    synchroZone.SetValue(1, 20); // 테스트 결과 OK
                else
                    synchroZone.SetValue(1, 21); // 테스트 결과 NOK

                vepClient.WriteSynchroZone(synchroZone);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"테스트 결과 읽기 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestFinish_Click(object sender, EventArgs e)
        {
            if (vepClient != null && vepClient.IsConnected)
            {
                synchroZone = VEPBenchSynchroZone.ReadFromVEP((start, count) => vepClient.ReadSynchroZone(start, count));
                int synTestOK = synchroZone.GetValue(1);
                
                if (synTestOK == 20)
                {
                    processTimer.Stop();
                    lblTestStatusValue.Text = "테스트 완료";
                }
                else
                {
                    MessageBox.Show($"VEP가 아직 종료되지 않았습니다. (Synchro 1 = {synTestOK})", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("VEP 연결 상태를 확인할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrintTicket_Click(object sender, EventArgs e)
        {
            MessageBox.Show("티켓이 출력되었습니다.", "티켓 출력", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            meaDateValue = DateTime.Now;
            string dateString = meaDateValue.ToString("yyyyMMdd");
            string xmlFileName = $"test_result_{dateString}.xml";
            string xmlFilePath = Path.Combine(Application.StartupPath, xmlFileName);

            try
            {
                XElement newResult = new XElement("TestResults",
                    new XElement("Timestamp", meaDateValue.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XElement("Barcode", barcodeValue),
                    new XElement("FrontCameraAngle1", synchroZone.FrontCameraAngle1),
                    new XElement("FrontCameraAngle2", synchroZone.FrontCameraAngle2),
                    new XElement("FrontCameraAngle3", synchroZone.FrontCameraAngle3),
                    new XElement("RearRightRadarAngle", synchroZone.RearRightRadarAngle),
                    new XElement("RearLeftRadarAngle", synchroZone.RearLeftRadarAngle)
                );

                XElement root;

                if (File.Exists(xmlFilePath))
                {
                    root = XElement.Load(xmlFilePath);
                    root.Add(newResult);
                }
                else
                {
                    root = new XElement("TestResults", newResult);
                }

                root.Save(xmlFilePath);

                MessageBox.Show("테스트 결과가 XML로 저장되었습니다.", "데이터 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisplayResult_Click(object sender, EventArgs e)
        {
            string barcode = barcodeValue;
            DateTime meaDate = meaDateValue;
            double frontCameraAngle1 = synchroZone?.FrontCameraAngle1 ?? 0;
            double frontCameraAngle2 = synchroZone?.FrontCameraAngle2 ?? 0;
            double frontCameraAngle3 = synchroZone?.FrontCameraAngle3 ?? 0;
            double rearRightRadarAngle = synchroZone?.RearRightRadarAngle ?? 0;
            double rearLeftRadarAngle = synchroZone?.RearLeftRadarAngle ?? 0;

            var resultForm = new Frm_Result(
                    barcode,
                    meaDate,
                    frontCameraAngle1,
                    frontCameraAngle2,
                    frontCameraAngle3,
                    rearRightRadarAngle,
                    rearLeftRadarAngle
                );

            resultForm.MdiParent = this.MdiParent;
            resultForm.WindowState = FormWindowState.Normal;
            resultForm.Show();
        }

        private void btnExitPosition_Click(object sender, EventArgs e)
        {
            lblTestStatusValue.Text = "Exit Poisition: 카메라 타겟 홈, 센터링 장치 홈 복귀";
        }

        private void btnMoveOut_Click(object sender, EventArgs e)
        {
            lblTestStatusValue.Text = "차량이 벤치에서 이동됨 (Move OUt)";
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            lblTestStatusValue.Text = "프로세스 종료 (End)";
            MessageBox.Show("테스트 프로세스가 종료되었습니다.", "종료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}