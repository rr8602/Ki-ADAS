using Ki_ADAS;
using Ki_ADAS.VEPBench;

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
        private VEPBenchClient _vepBenchClient;
        private IniFile _iniFile;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT = "VepPort";
        private Timer _statusTimer;
        private string _ipAddress;
        private int _port;

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
        private VEPBenchSynchroZone _synchroZone = new VEPBenchSynchroZone();

        public Frm_CameraSimulator()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // 설정 파일 로드
            string iniPath = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\..\Excute\config.ini");
            _iniFile = new Ki_ADAS.IniFile(iniPath);

            _ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
            _port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

            // VEP 프로토콜 초기화
            _vepBenchClient = new VEPBenchClient(_ipAddress, _port);
            _vepBenchClient.DebugMode = true;

            // 상태 확인 타이머 설정
            _statusTimer = new Timer();
            _statusTimer.Interval = 1000;
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

        private void VepProtocol_OnConnectionChanged(object sender, VEPProtocol.VEPConnectionEventArgs e)
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
                message += $"Roll 각도 오류: {_rollAngle}, 허용범위: ±{ROLL_THRESHOLD} (실패)\n";
                isValid = false;
            }
            else
            {
                message += $"Roll 각도 정상: {_rollAngle} (성공)\n";
            }

            // Azimuth 각도 검증
            if (Math.Abs(_azimuthAngle) > AZIMUTH_THRESHOLD)
            {
                message += $"Azimuth 각도 오류: {_azimuthAngle}, 허용범위: ±{AZIMUTH_THRESHOLD} (실패)\n";
                isValid = false;
            }
            else
            {
                message += $"Azimuth 각도 정상: {_azimuthAngle} (성공)\n";
            }

            // Elevation 각도 검증
            if (Math.Abs(_elevationAngle) > ELEVATION_THRESHOLD)
            {
                message += $"Elevation 각도 오류: {_elevationAngle}, 허용범위: ±{ELEVATION_THRESHOLD} (실패)\n";
                isValid = false;
            }
            else
            {
                message += $"Elevation 각도 정상: {_elevationAngle} (성공)\n";
            }

            AddLogMessage(message);

            // 최종 결과 업데이트
            if (isValid)
            {
                UpdateStatus(CalibrationStatus.Success);
                AddLogMessage("캘리브레이션 성공: 모든 각도가 허용 범위 내에 있습니다.");

                ushort[] data= new ushort[3];
                data[0] = 20;
                _vepBenchClient.WriteReceptionZone(data);
                AddLogMessage("응답: Synchro 3 = 20 (캘리브레이션 완료)");
            }
            else
            {
                UpdateStatus(CalibrationStatus.Failed);
                AddLogMessage("캘리브레이션 실패: 하나 이상의 각도가 허용 범위를 벗어났습니다.");

                ushort[] data = new ushort[3];
                data[0] = 1;
                _vepBenchClient.WriteStatusZone(data);
                AddLogMessage("응답: Synchro 3 = 1 (캘리브레이션 재시작)");
            }
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (_vepBenchClient != null && _vepBenchClient.IsConnected)
            {
                try
                {
                    ushort[] statusData = _vepBenchClient.ReadStatusZone(6);

                    if (statusData != null && statusData.Length >= 3)
                    {
                        lblSynchro3.Text = statusData[0].ToString();
                        lblSynchro4.Text = statusData[1].ToString();
                        lblSynchro89.Text = statusData[2].ToString();
                    }

                    VEPBenchSynchroZone synchro = _vepBenchClient.ReadSynchroZone();
                    lblSynchro110.Text = synchro.Angle1.ToString();
                    lblSynchro111.Text = synchro.Angle2.ToString();
                    lblSynchro112.Text = synchro.Angle3.ToString();

                    ProcessStatusZone(statusData);
                }
                catch (Exception ex)
                {
                    AddLogMessage($"상태 업데이트 오류: {ex.Message}");
                }
            }
        }

        private void ProcessStatusZone(ushort[] statusData)
        {
            if (statusData == null || statusData.Length < 3)
                return;

            int currentSynchro3Value = 0;
            _synchroValues.TryGetValue(3, out currentSynchro3Value);

            if (statusData[0] == 1 && currentSynchro3Value != 1)
            {
                _synchroValues[3] = 1;
                AddLogMessage("전면 카메라 캘리브레이션 요청됨 (Synchro 3 = 1)");
                UpdateStatus(CalibrationStatus.Requested);

                RespondToSynchro3_1();
            }
            else if (statusData[0] == 20 && currentSynchro3Value != 20)
            {
                _synchroValues[3] = 20;
                AddLogMessage("타겟을 홈 위치로 이동 (Synchro 3 = 20)");

                RespondToSynchro3_20();
            }
            else if (statusData[0] == 21 && currentSynchro3Value != 21)
            {
                _synchroValues[3] = 21;
                AddLogMessage("타겟을 홈 위치로 이동 2단계 (Synchro 3 = 21)");

                SendAngleMeausrements();
            }

            int currnetSynchro89value = 0;
            _synchroValues.TryGetValue(89, out currnetSynchro89value);

            if (statusData[2] == 1 && currnetSynchro89value != 1)
            {
                _synchroValues[89] = 1;
                AddLogMessage("Synchro 89 첫번째 시도 (Synchro 89 = 1)");
            }
            else if (statusData[2] == 2&& currnetSynchro89value != 2)
            {
                _synchroValues[89] = 2;
                AddLogMessage("Synchro 89 두번째 시도 (Synchro 89 = 2)");

                ValidateCalibration();
            }
        }

        private void RespondToSynchro3_1()
        {
            try
            {
                ushort[] data = new ushort[3];
                data[1] = 1;
                _vepBenchClient.WriteStatusZone(data);
                AddLogMessage("응답: Synchro 4 = 1 (타겟 위치 확인)");

                UpdateStatus(CalibrationStatus.InProgress);
            }
            catch (Exception ex)
            {
                AddLogMessage($"응답 오류: {ex.Message}");
            }
        }

        private void RespondToSynchro3_20()
        {
            try
            {
                ushort[] data = new ushort[3];
                data[0] = 21;
                _vepBenchClient.WriteStatusZone(data);
                AddLogMessage("응답: Synchro 3 = 21 (타겟을 홈 위치로 이동)");
            }
            catch (Exception ex)
            {
                AddLogMessage($"응답 오류: {ex.Message}");
            }
        }

        private void SendAngleMeausrements()
        {
            try
            {
                _synchroZone = new VEPBenchSynchroZone();
                _synchroZone.Angle1 = ConvertAngleToRawValue(_rollAngle);
                _synchroZone.Angle2 = ConvertAngleToRawValue(_azimuthAngle);
                _synchroZone.Angle3 = ConvertAngleToRawValue(_elevationAngle);

                _vepBenchClient.WriteSynchroZone(_synchroZone);
                AddLogMessage($"각도 측정값 전송: Roll={_rollAngle}, Azimuth={_azimuthAngle}, Elevation={_elevationAngle}");

                ushort[] data = new ushort[3];
                data[2] = 1;
                _vepBenchClient.WriteStatusZone(data);
                AddLogMessage("응답: Synchro 89 = 1 (각도 측정 완료)");
            }
            catch (Exception ex)
            {
                AddLogMessage($"각도 전송 오류: {ex.Message}");
            }
        }

        private void SetSynchro89_2()
        {
            try
            {
                ushort[] data = new ushort[3];
                data[2] = 2;
                _vepBenchClient.WriteStatusZone(data);
                AddLogMessage("응답: Synchro 89 = 2 (검증 단계)");
            }
            catch (Exception ex)
            {
                AddLogMessage($"Synchro 89 = 2 설정 오류: {ex.Message}");
            }
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
                AddLogMessage($"VEP 서버에 연결 시도 중: {_ipAddress}:{_port}");
                _vepBenchClient.Connect();

                if (_vepBenchClient.IsConnected)
                {
                    AddLogMessage("VEP 서버 연결 실패");
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                    _statusTimer.Start();
                }
                else
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
                _vepBenchClient.DisConnect();
                AddLogMessage("VEP 서버 연결 종료");
                _statusTimer.Stop();
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                UpdateStatus(CalibrationStatus.NotStarted);
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

        private void btnStartCalibration_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_vepBenchClient.IsConnected)
                {
                    AddLogMessage("VEP 서버에 연결되어 있지 않습니다.");
                    return;
                }

                AddLogMessage("카메라 캘리브레이션 프로세스 시작");

                // Synchro 3 = 1 설정 (전면 카메라 캘리브레이션 요청)
                ushort[] data = new ushort[3];
                data[0] = 1;
                _vepBenchClient.WriteStatusZone(data);

                _synchroValues[3] = 1;
                UpdateStatus(CalibrationStatus.Requested);
            }
            catch (Exception ex)
            {
                AddLogMessage($"캘리브레이션 시작 오류: {ex.Message}");
            }
        }
    }
}