using Ki_ADAS.DB;
using Ki_ADAS;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;

namespace Ki_ADAS
{
    public partial class HomePositionSimulator : Form
    {
        private Frm_Main mainForm;
        private VEPBenchClient vepClient;
        private ModelRepository _modelRepository;
        private VEPBenchDataManager _dataManager;

        // 시스템 상태 변수
        private bool isVEPWorking = false;
        private bool isTestCompleted = false;
        private int cycleValue = 0;
        private Model selectedModel;
        private string modelName;
        private string barcodeValue = "";
        private DateTime meaDateValue;
        private System.Windows.Forms.Timer processTimer;
        private int timeElapsed = 0;
        private int timeoutLimit = 30; // 30초 타임아웃
        private bool[] isTest = new bool[3]; // 각 센서별 테스트 계획 여부

        private ADASProcess process;

        private enum CalibrationTarget
        {
            None,
            FrontCamera,
            RightRearRadar,
            LeftRearRadar
        }

        private CalibrationTarget currentTarget = CalibrationTarget.None;

        public HomePositionSimulator(VEPBenchClient client, ModelRepository modelRepository, Frm_Main mainForm)
        {
            InitializeComponent();
            vepClient = client;
            this._modelRepository = modelRepository;
            this.mainForm = mainForm;
            this.TopMost = false;
            _dataManager = VEPBenchDataManager.Instance;
            _dataManager.SynchroZone = _dataManager.RefreshSynchroZoneFromVEP(vepClient);
            InitializeCustomComponents();
            SetInitialState();
        }

        private void InitializeCustomComponents()
        {
            // 타이머 초기화
            processTimer = new System.Windows.Forms.Timer();
            processTimer.Interval = 1000;
            processTimer.Tick += ProcessTimer_Tick;

            try
            {
                if (vepClient == null && !vepClient.IsConnected)
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
            isVEPWorking = false;
            cycleValue = 0;
            barcodeValue = "";
            timeElapsed = 0;

            currentTarget = CalibrationTarget.None;

            // UI 업데이트
            lblStatus.Text = "상태: 홈 포지션";
            lblStatus.ForeColor = Color.Green;
            lblPositioningDevice.Text = "포지셔닝 장치: 홈";
            lblCameraTarget.Text = "카메라 타겟: 홈";
            lblVehicleDetect.Text = "차량 감지 센서: 꺼짐";

            // 버튼 상태 업데이트
            btnSetTrafficLight.Enabled = true;
            btnScanBarcode.Enabled = false;
            btnRequestPJI.Enabled = false;
            btnCheckVEPStatus.Enabled = false;
            btnSelectOption.Enabled = false;
            btnReset.Enabled = true;

            try
            {
                if (vepClient != null && vepClient.IsConnected)
                {
                    _dataManager.StatusZone.StartCycle = 0;
                    _dataManager.StatusZone.VepStatus = VEPBenchStatusZone.VepStatus_Undefined;
                    vepClient.WriteStatusZone();

                    for (int i = 0; i < _dataManager.SynchroZone.Size; i++)
                    {
                        _dataManager.SynchroZone.SetValue(i, 0);
                    }

                    vepClient.WriteSynchroZone();
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
            // 단계에 따라 패널 색상 업데이트
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
            lblStatus.Text = "상태: 입구 신호등 초록색";
            UpdateStatusPanels(2);
            btnSetTrafficLight.Enabled = false;
            btnScanBarcode.Enabled = true;
        }

        private void btnScanBarcode_Click(object sender, EventArgs e)
        {
            if (mainForm != null)
            {
                selectedModel = mainForm.SelectedModelInfo;

                if (selectedModel == null)
                {
                    MessageBox.Show($"선택된 모델 '{modelName}'에 대한 설정 정보를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                barcodeValue = selectedModel.Barcode;

                if (string.IsNullOrEmpty(barcodeValue))
                {
                    MessageBox.Show("메인 화면의 테스트 목록에서 항목을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            lblStatus.Text = $"상태: 바코드 스캔 완료 - {barcodeValue}";
            lblVehicleDetect.Text = "차량 감지 센서: 켜짐";
            UpdateStatusPanels(4);
            btnScanBarcode.Enabled = false;

            // 사이클 시작
            cycleValue = 1;

            if (vepClient != null && vepClient.IsConnected)
            {
                _dataManager.StatusZone.StartCycle = (ushort)cycleValue;
                vepClient.WriteStatusZone();
            }

            lblStatus.Text = $"상태: 사이클 시작 값 = {cycleValue}";
            UpdateStatusPanels(5);
            btnRequestPJI.Enabled = true;
        }

        private void btnRequestPJI_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "상태: PJI 요청됨";
            UpdateStatusPanels(6);
            btnRequestPJI.Enabled = false;
            lblStatus.Text = "상태: PJI가 VEP로 전송됨";
            btnCheckVEPStatus.Enabled = true;

            if (vepClient != null && vepClient.IsConnected)
            {
                _dataManager.StatusZone.StartCycle = 1;
                _dataManager.StatusZone.VepStatus = VEPBenchStatusZone.VepStatus_Working;
                vepClient.WriteStatusZone();

                #region 센서별 테스트 계획 여부 설정 - VEP 서버에서 전달해주는 값이기 때문에 테스트 목적으로만 설정 (실제는 필요 없음)
                bool frontCameraTest = selectedModel.Fr_IsTest;
                bool rearRightRadarTest = selectedModel.R_IsTest;
                bool rearLeftRadarTest = selectedModel.L_IsTest;

                isTest[0] = frontCameraTest;
                isTest[1] = rearRightRadarTest;
                isTest[2] = rearLeftRadarTest;

                if (isTest[0] == true)
                {
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 1);
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 0);
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 0);
                }
                else if (isTest[1] == true)
                {
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 0);
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 1);
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 0);
                }
                else if (isTest[2] == true)
                {
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX, 0);
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX, 0);
                    _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX, 1);
                }

                vepClient.WriteSynchroZone();
                #endregion
            }

            // VEP 서버에서 Transmission Zone의 ExchStatus 가 2로 변경되면, 자동으로 VEPBenchClient의 PollAllZones 에서 캐치하여 VEP로 전송됨
            lblStatus.Text = "상태: PJI가 VEP로 전송됨";
            btnCheckVEPStatus.Enabled = true;

            Thread.Sleep(1000);

            // 타이머 시작
            timeElapsed = 0;
            processTimer.Start();
        }

        private void btnCheckVEPStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (vepClient != null && vepClient.IsConnected)
                    isVEPWorking = (_dataManager.StatusZone.VepStatus == VEPBenchStatusZone.VepStatus_Working);
                else
                    isVEPWorking = chkVEPWorking.Checked;

                if (isVEPWorking)
                {
                    lblStatus.Text = "상태: VEP 작업 중";
                    UpdateStatusPanels(7);
                    btnCheckVEPStatus.Enabled = false;
                    btnSelectOption.Enabled = true;
                    processTimer.Stop();
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
                    if (_dataManager.SynchroZone.GetValue(_dataManager.SynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1)
                    {
                        deviceType = "카메라";
                    }
                    else if (_dataManager.SynchroZone.GetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1)
                    {
                        deviceType = "우측 후방 레이더";
                    }
                    else if (_dataManager.SynchroZone.GetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1)
                    {
                        deviceType = "좌측 후방 레이더";
                    }

                    if (isTest[0] == true)
                        lblStatus.Text = $"상태: 전방 카메라 옵션 선택됨 - {deviceType}";
                    else if (isTest[1] == true)
                        lblStatus.Text = $"상태: 우측 후방 레이더 옵션 선택됨 - {deviceType}";
                    else if (isTest[2] == true)
                        lblStatus.Text = $"상태: 좌측 후방 레이더 옵션 선택됨 - {deviceType}";
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

            lblTestStatusValue.Text = "테스트 포지션 설정 완료";
            btnSendToVEP.Enabled = true;
        }

        private void btnSendToVEP_Click(object sender, EventArgs e)
        {
            try
            {
                process = new ADASProcess();

                if (vepClient != null && vepClient.IsConnected)
                {
                    CalibrationTarget detectedTarget = CalibrationTarget.None;

                    if (_dataManager.SynchroZone.GetValue(_dataManager.SynchroZone.DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1)
                    {
                        detectedTarget = CalibrationTarget.FrontCamera;
                        lblTargetValue.Text = "전방 카메라";
                        lblTestStatusValue.Text = "전방 카메라 타겟 설정됨";
                    }
                    else if (_dataManager.SynchroZone.GetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1)
                    {
                        detectedTarget = CalibrationTarget.RightRearRadar;
                        lblTargetValue.Text = "우측 후방 레이더";
                        lblTestStatusValue.Text = "우측 후방 레이더 타겟 설정됨";
                    }
                    else if (_dataManager.SynchroZone.GetValue(_dataManager.SynchroZone.DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1)
                    {
                        detectedTarget = CalibrationTarget.LeftRearRadar;
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
                            _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.SYNC_COMMAND_FRONT_CAMERA_INDEX, 1);
                            lblTargetValue.Text = "전방 카메라";
                            lblTestStatusValue.Text = "전방 카메라 타겟(VEP Synchro 4 = 1) 설정";
                            vepClient.WriteSynchroZone();
                            process.InitializeFrontCameraSteps(_dataManager);
                            break;
                        case CalibrationTarget.RightRearRadar:
                            _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.SYNC_COMMAND_REAR_RIGHT_RADAR_INDEX, 1);
                            lblTargetValue.Text = "우측 후방 레이더";
                            lblTestStatusValue.Text = "우측 후방 레이더 타겟(VEP Synchro 52 = 1) 설정";
                            vepClient.WriteSynchroZone();
                            process.InitializeRightRearRadarSteps(_dataManager);
                            break;
                        case CalibrationTarget.LeftRearRadar:
                            _dataManager.SynchroZone.SetValue(_dataManager.SynchroZone.SYNC_COMMAND_REAR_LEFT_RADAR_INDEX, 1);
                            lblTargetValue.Text = "좌측 후방 레이더";
                            lblTestStatusValue.Text = "좌측 후방 레이더 타겟(VEP Synchro 54 = 1) 설정";
                            vepClient.WriteSynchroZone();
                            process.InitializeLeftRearRadarSteps(_dataManager);
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
                    string resultText = "";
                    double frontCameraAngle1 = 0;
                    double frontCameraAngle2 = 0;
                    double frontCameraAngle3 = 0;
                    double rearRightRadarAngle = 0;
                    double rearLeftRadarAngle = 0;

                    switch (currentTarget)
                    {
                        case CalibrationTarget.FrontCamera:
                            frontCameraAngle1 = _dataManager.SynchroZone.FrontCameraAngle1;
                            frontCameraAngle2 = _dataManager.SynchroZone.FrontCameraAngle2;
                            frontCameraAngle3 = _dataManager.SynchroZone.FrontCameraAngle3;

                            resultText = $"Roll 각도: {frontCameraAngle1:F2}°\nSynchro 110: {_dataManager.SynchroZone.GetValue(110)}\n" +
                                         $"Azimuth 각도: {frontCameraAngle2:F2}°\nSynchro 111: {_dataManager.SynchroZone.GetValue(111)}\n" +
                                         $"Elevation 각도: {frontCameraAngle3:F2}°\nSynchro 112: {_dataManager.SynchroZone.GetValue(112)}";
                            break;

                        case CalibrationTarget.RightRearRadar:
                            rearRightRadarAngle = _dataManager.SynchroZone.RearRightRadarAngle;
                            resultText = $"각도: {rearRightRadarAngle:F2}°\nSynchro 115: {_dataManager.SynchroZone.GetValue(115)}";
                            break;

                        case CalibrationTarget.LeftRearRadar:
                            rearLeftRadarAngle = _dataManager.SynchroZone.RearLeftRadarAngle;
                            resultText = $"각도: {rearLeftRadarAngle:F2}°\nSynchro 116: {_dataManager.SynchroZone.GetValue(116)}";
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
                    _dataManager.SynchroZone.SetValue(1, 20); // 테스트 결과 OK
                else
                    _dataManager.SynchroZone.SetValue(1, 21); // 테스트 결과 NOK

                vepClient.WriteSynchroZone();

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
                int synTestOK = _dataManager.SynchroZone.GetValue(1);
                
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
                    new XElement("Model", modelName),
                    new XElement("Barcode", barcodeValue),
                    new XElement("FrontCameraAngle1", _dataManager.SynchroZone.FrontCameraAngle1),
                    new XElement("FrontCameraAngle2", _dataManager.SynchroZone.FrontCameraAngle2),
                    new XElement("FrontCameraAngle3", _dataManager.SynchroZone.FrontCameraAngle3),
                    new XElement("RearRightRadarAngle", _dataManager.SynchroZone.RearRightRadarAngle),
                    new XElement("RearLeftRadarAngle", _dataManager.SynchroZone.RearLeftRadarAngle)
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
            string pji = barcodeValue;
            double frontCameraAngle1 = _dataManager.SynchroZone?.FrontCameraAngle1 ?? 0;
            double frontCameraAngle2 = _dataManager.SynchroZone?.FrontCameraAngle2 ?? 0;
            double frontCameraAngle3 = _dataManager.SynchroZone?.FrontCameraAngle3 ?? 0;
            double rearRightRadarAngle = _dataManager.SynchroZone?.RearRightRadarAngle ?? 0;
            double rearLeftRadarAngle = _dataManager.SynchroZone?.RearLeftRadarAngle ?? 0;

            var resultForm = new Frm_Result(
                    pji,
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