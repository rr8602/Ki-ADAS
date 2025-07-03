using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator
{
    public partial class Frm_CameraSimulator : Form
    {
        private Ki_ADAS.VEPProtocol _vepProtocol;
        private Ki_ADAS.IniFile _iniFile;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT = "VepPort";
        private Timer _statusTimer;

        // 카메라 캘리브레이션 상태
        private enum CalibrationStatus
        {
            NotStarted,
            Requested,
            InProgress,
            Success,
            Failed
        }

        private CalibrationStatus _currentStatus = CalibrationStatus.NotStarted;

        // 각도 측정값
        private double _rollAngle = 0;
        private double _azimuthAngle = 0;
        private double _elevationAngle = 0;

        // 각도 임계값 (허용범위)
        private const double ROLL_THRESHOLD = 1.0;
        private const double AZIMUTH_THRESHOLD = 1.0;
        private const double ELEVATION_THRESHOLD = 1.0;

        // 시뮬레이션 목적으로 사용할 synchro 값 저장
        private Dictionary<int, int> _synchroValues = new Dictionary<int, int>();

        public Frm_CameraSimulator()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // 설정 파일 로드
            string iniPath = System.IO.Path.Combine(Application.StartupPath, "config.ini");
            _iniFile = new Ki_ADAS.IniFile(iniPath);

            // VEP 프로토콜 초기화
            _vepProtocol = new Ki_ADAS.VEPProtocol();
            _vepProtocol.OnDataReceived += VepProtocol_OnDataReceived;
            _vepProtocol.OnConnectionChanged += VepProtocol_OnConnectionChanged;

            // 상태 확인 타이머 설정
            _statusTimer = new Timer();
            _statusTimer.Interval = 1000; // 1초마다 체크
            _statusTimer.Tick += StatusTimer_Tick;

            // 각도 설정 초기화
            tbRollAngle.Text = "0.0";
            tbAzimuthAngle.Text = "0.0";
            tbElevationAngle.Text = "0.0";

            // 시뮬레이션 상태 설정
            UpdateStatus(CalibrationStatus.NotStarted);
        }

        private void UpdateStatus(CalibrationStatus status)
        {
            _currentStatus = status;

            switch (status)
            {
                case CalibrationStatus.NotStarted:
                    lblStatus.Text = "준비됨";
                    lblStatus.BackColor = Color.LightGray;
                    break;
                case CalibrationStatus.Requested:
                    lblStatus.Text = "캘리브레이션 요청됨";
                    lblStatus.BackColor = Color.LightBlue;
                    break;
                case CalibrationStatus.InProgress:
                    lblStatus.Text = "캘리브레이션 진행 중";
                    lblStatus.BackColor = Color.Yellow;
                    break;
                case CalibrationStatus.Success:
                    lblStatus.Text = "캘리브레이션 성공";
                    lblStatus.BackColor = Color.Green;
                    break;
                case CalibrationStatus.Failed:
                    lblStatus.Text = "캘리브레이션 실패";
                    lblStatus.BackColor = Color.Red;
                    break;
            }
        }

        private void VepProtocol_OnConnectionChanged(object sender, Ki_ADAS.VEPProtocol.VEPConnectionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => VepProtocol_OnConnectionChanged(sender, e)));
                return;
            }

            btnConnect.Enabled = !e.IsConnected;
            btnDisconnect.Enabled = e.IsConnected;

            if (e.IsConnected)
            {
                AddLogMessage("VEP 서버에 연결됨");
                _statusTimer.Start();
            }
            else
            {
                AddLogMessage("VEP 서버와 연결 끊김");
                _statusTimer.Stop();
                UpdateStatus(CalibrationStatus.NotStarted);
            }
        }

        private void VepProtocol_OnDataReceived(object sender, Ki_ADAS.VEPProtocol.VEPDataReceivedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => VepProtocol_OnDataReceived(sender, e)));
                return;
            }

            string message = $"명령 수신: {e.Response.Command}";

            if (e.Response.Data.Length > 0)
            {
                message += $", 데이터: {string.Join(", ", e.Response.Data)}";
            }

            AddLogMessage(message);

            ProcessVepCommand(e.Response);
        }

        private void ProcessVepCommand(Ki_ADAS.VEPProtocol.VEPResponse response)
        {
            switch (response.Command)
            {
                case Ki_ADAS.VEPProtocol.VEPCommand.SetSynchro:
                    if (response.Data.Length >= 2)
                    {
                        int synchroNumber = response.Data[0];
                        int synchroValue = response.Data[1];

                        // Synchro 값 저장
                        _synchroValues[synchroNumber] = synchroValue;
                        AddLogMessage($"Synchro {synchroNumber} = {synchroValue}");

                        // Synchro 상태에 따른 처리
                        ProcessSynchroUpdate(synchroNumber, synchroValue);
                    }
                    break;

                case Ki_ADAS.VEPProtocol.VEPCommand.CameraCalibration:
                    AddLogMessage("카메라 캘리브레이션 요청 수신");
                    UpdateStatus(CalibrationStatus.Requested);
                    break;

                case Ki_ADAS.VEPProtocol.VEPCommand.InitCamera:
                    AddLogMessage("카메라 초기화 요청 수신");
                    break;
            }
        }

        private void ProcessSynchroUpdate(int synchroNumber, int value)
        {
            // 이미지 표시된 캘리브레이션 프로세스에 따른 시뮬레이션 처리
            switch (synchroNumber)
            {
                case 3:
                    if (value == 1)
                    {
                        AddLogMessage("전면 카메라 캘리브레이션 요청됨");
                        UpdateStatus(CalibrationStatus.Requested);
                    }
                    else if (value == 20)
                    {
                        AddLogMessage("타겟을 홈 위치로 이동");
                    }
                    else if (value == 21)
                    {
                        AddLogMessage("타겟을 홈 위치로 이동 2단계");
                    }
                    break;

                case 4:
                    if (value == 1)
                    {
                        AddLogMessage("전면 카메라 타겟 위치 확인");
                        UpdateStatus(CalibrationStatus.InProgress);
                    }
                    break;

                case 110: // 각도 1 (Roll)
                    AddLogMessage($"Roll 각도 값 요청됨");
                    // 시뮬레이션으로 입력된 Roll 값 전송
                    int rollValue = ConvertAngleToRawValue(_rollAngle);
                    AddLogMessage($"Roll 각도 값 전송: {_rollAngle} (raw: {rollValue})");
                    break;

                case 111: // 각도 2 (Azimuth)
                    AddLogMessage($"Azimuth 각도 값 요청됨");
                    // 시뮬레이션으로 입력된 Azimuth 값 전송
                    int azimuthValue = ConvertAngleToRawValue(_azimuthAngle);
                    AddLogMessage($"Azimuth 각도 값 전송: {_azimuthAngle} (raw: {azimuthValue})");
                    break;

                case 112: // 각도 3 (Elevation)
                    AddLogMessage($"Elevation 각도 값 요청됨");
                    // 시뮬레이션으로 입력된 Elevation 값 전송
                    int elevationValue = ConvertAngleToRawValue(_elevationAngle);
                    AddLogMessage($"Elevation 각도 값 전송: {_elevationAngle} (raw: {elevationValue})");
                    break;

                case 89:
                    if (value == 1)
                    {
                        AddLogMessage("Synchro 89 첫번째 시도");
                    }
                    else if (value == 2)
                    {
                        AddLogMessage("Synchro 89 두번째 시도");
                        // 각도 검증 처리
                        ValidateCalibration();
                    }
                    break;
            }
        }

        private int ConvertAngleToRawValue(double angle)
        {
            // 실제 각도를 Raw 데이터 값으로 변환 (예: 100을 곱함)
            return (int)(angle * 100);
        }

        private void ValidateCalibration()
        {
            bool isValid = true;
            string message = "캘리브레이션 검증 결과:\n";

            // Roll 각도 검증
            if (Math.Abs(_rollAngle) > ROLL_THRESHOLD)
            {
                message += $"Roll 각도 오류: {_rollAngle}, 허용범위: ±{ROLL_THRESHOLD}\n";
                isValid = false;
            }
            else
            {
                message += $"Roll 각도 정상: {_rollAngle}\n";
            }

            // Azimuth 각도 검증
            if (Math.Abs(_azimuthAngle) > AZIMUTH_THRESHOLD)
            {
                message += $"Azimuth 각도 오류: {_azimuthAngle}, 허용범위: ±{AZIMUTH_THRESHOLD}\n";
                isValid = false;
            }
            else
            {
                message += $"Azimuth 각도 정상: {_azimuthAngle}\n";
            }

            // Elevation 각도 검증
            if (Math.Abs(_elevationAngle) > ELEVATION_THRESHOLD)
            {
                message += $"Elevation 각도 오류: {_elevationAngle}, 허용범위: ±{ELEVATION_THRESHOLD}\n";
                isValid = false;
            }
            else
            {
                message += $"Elevation 각도 정상: {_elevationAngle}\n";
            }

            AddLogMessage(message);

            // 최종 결과 업데이트
            if (isValid)
            {
                UpdateStatus(CalibrationStatus.Success);
                AddLogMessage("캘리브레이션 성공");
            }
            else
            {
                UpdateStatus(CalibrationStatus.Failed);
                AddLogMessage("캘리브레이션 실패");
            }
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            // 현재 상태 값 갱신
            lblSynchro3.Text = _synchroValues.ContainsKey(3) ? _synchroValues[3].ToString() : "-";
            lblSynchro4.Text = _synchroValues.ContainsKey(4) ? _synchroValues[4].ToString() : "-";
            lblSynchro89.Text = _synchroValues.ContainsKey(89) ? _synchroValues[89].ToString() : "-";
            lblSynchro110.Text = _synchroValues.ContainsKey(110) ? _synchroValues[110].ToString() : "-";
            lblSynchro111.Text = _synchroValues.ContainsKey(111) ? _synchroValues[111].ToString() : "-";
            lblSynchro112.Text = _synchroValues.ContainsKey(112) ? _synchroValues[112].ToString() : "-";
        }

        private void AddLogMessage(string message)
        {
            lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            lstLog.SelectedIndex = lstLog.Items.Count - 1;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
                int port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

                AddLogMessage($"VEP 서버에 연결 시도 중: {ipAddress}:{port}");
                bool connected = _vepProtocol.Connect(ipAddress, port);

                if (!connected)
                {
                    AddLogMessage("VEP 서버 연결 실패");
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"연결 오류: {ex.Message}");
            }
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _vepProtocol.DisConnect();
                AddLogMessage("VEP 서버 연결 종료");
            }
            catch (Exception ex)
            {
                AddLogMessage($"연결 종료 오류: {ex.Message}");
            }
        }

        private void BtnApplyAngles_Click(object sender, EventArgs e)
        {
            try
            {
                _rollAngle = double.Parse(tbRollAngle.Text);
                _azimuthAngle = double.Parse(tbAzimuthAngle.Text);
                _elevationAngle = double.Parse(tbElevationAngle.Text);

                AddLogMessage($"각도 설정 변경됨: Roll={_rollAngle}, Azimuth={_azimuthAngle}, Elevation={_elevationAngle}");
            }
            catch (Exception ex)
            {
                AddLogMessage($"각도 설정 오류: {ex.Message}");
            }
        }

        private void BtnSimulateSuccess_Click(object sender, EventArgs e)
        {
            tbRollAngle.Text = "0.5";
            tbAzimuthAngle.Text = "0.5";
            tbElevationAngle.Text = "0.5";
            BtnApplyAngles_Click(sender, e);
        }

        private void BtnSimulateFail_Click(object sender, EventArgs e)
        {
            tbRollAngle.Text = "1.5";
            tbAzimuthAngle.Text = "0.5";
            tbElevationAngle.Text = "0.5";
            BtnApplyAngles_Click(sender, e);
        }
    }
}