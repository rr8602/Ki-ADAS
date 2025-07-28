using Ki_ADAS.VEPBench;
using Ki_ADAS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Ki_ADAS
{
    public partial class Frm_Main : Form
    {
        private Frm_Mainfrm m_frmParent = null;
        private Frm_VEP _vep;
        private VEPBenchClient _vepBenchClient;
        private ADASProcess _adasProcess;
        private IniFile _iniFile;
        private string ipAddress;
        private int port;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT = "VepPort";

        public Frm_Main()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            string iniPath = Path.Combine(Application.StartupPath, "config.ini");
            _iniFile = new IniFile(iniPath);

            ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
            port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

            _vepBenchClient = new VEPBenchClient(ipAddress, port);

            BtnStop.Enabled = false;
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                /*_adasProcess = new ADASProcess(_vepBenchClient);
                _adasProcess.OnProcessStepChanged += ADASProcess_OnProcessStepChanged;
                _adasProcess.InitializeProcessSteps();

                bool success = _adasProcess.Start(ipAddress, port);

                // VEP 클라이언트가 없거나 연결이 끊어진 경우 새로 생성
                if (_vepBenchClient == null || !_vepBenchClient.IsConnected)
                {
                    if (_vepBenchClient != null)
                    {
                        _vepBenchClient.DisConnect();
                    }

                    _vepBenchClient = new VEPBenchClient(ipAddress, port);
                    _vepBenchClient.Connect();

                    // ADAS 프로세스 재초기화
                    _adasProcess = new ADASProcess(_vepBenchClient);
                    _adasProcess.OnProcessStepChanged += ADASProcess_OnProcessStepChanged;
                }
*/
                // 임시 HomePositionSimulator 실행 부분 (나중에 삭제)
                bool connected = _vepBenchClient.TestConnection();

                if (connected)
                {
                    BtnStart.Enabled = false;
                    BtnStop.Enabled = true;

                    AddLogMessage("VEP 연결 성공 - 시뮬레이터를 통해 ADAS 프로세스 시작 가능");
                    AddLogMessage("'홈 포지션 시뮬레이터' 버튼을 클릭하여 테스트를 시작하세요.");

                    _vepBenchClient.DebugMode = false;
                    _vepBenchClient.StartMonitoring();

                    seqList.Items.Clear();

                    if (MessageBox.Show("홈 포지션 시뮬레이터를 지금 실행하시겠습니까?",
                        "시뮬레이터 실행", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        HomePositionSimulator simulator = new HomePositionSimulator(_vepBenchClient);
                        simulator.ShowDialog();
                    }
                }
                else
                {
                    AddLogMessage("VEP 연결 실패 - IP 주소와 포트를 확인해주세요");
                    MessageBox.Show($"VEP 연결에 실패했습니다. IP 주소({ipAddress})와 포트({port})를 확인하세요.",
                        "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"오류 발생: {ex.Message}");

                MessageBox.Show($"ADAS 프로세스 시작 중 오류가 발생했습니다: {ex.Message}",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
            }

            /*try
            {
                string ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
                int port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

                bool started = _adasProcess.Start(ipAddress, port);

                if (started)
                {
                    BtnStart.Enabled = false;
                    BtnStop.Enabled = true;

                    AddLogMessage("ADAS  프로세스 시작");
                }
                else
                {
                    AddLogMessage("ADAS 프로세스 시작 실패");
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"오류 발생: {ex.Message}");
            }*/
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _adasProcess.Stop();
                _vepBenchClient.StopMonitoring();
                _vepBenchClient.DisConnect();

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;

                AddLogMessage("ADAS 프로세스 중지");
            }
            catch (Exception ex)
            {
                AddLogMessage($"오류 발생: {ex.Message}");
            }
        }

        private void ADASProcess_OnProcessStepChanged(object sender, ADASProcess.ADASProcessEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ADASProcess_OnProcessStepChanged(sender, e)));

                return;
            }
            
            if (m_frmParent != null && m_frmParent.User_Monitor != null)
            {
                if (e.Step != null)
                {
                    m_frmParent.User_Monitor.UpdateStepDescription(e.Step.Description);
                }

                if (e.StateType == ADASProcess.ProcessStateType.Success && e.Message == "프로세스 완료")
                {
                    m_frmParent.User_Monitor.UpdateStepDescription("테스트 완료");
                }
            }

            string logMessage = $"[{e.StateType}] {e.Message}";

            AddLogMessage(logMessage);

            UpdateProcessStepDisplay(e);

            if (e.StateType == ADASProcess.ProcessStateType.Success && e.Message == "프로세스 완료")
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
                AddLogMessage("ADAS 프로세스가 성공적으로 완료되었습니다.");
            }
            else if (e.StateType == ADASProcess.ProcessStateType.Error)
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
                AddLogMessage("ADAS 프로세스가 오류로 인해 중지되었습니다.");
            }
        }

        private void AddLogMessage(string message)
        {
            lb_Message.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            lb_Message.SelectedIndex = lb_Message.Items.Count - 1;
        }

        private void UpdateProcessStepDisplay(ADASProcess.ADASProcessEventArgs e)
        {
            if (e.Step != null)
            {
                seqList.Items.Add(new ListViewItem(new string[] { e.Step.StepId.ToString(), e.Step.Description, e.Step.SynchroState }));
            }
        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_adasProcess != null)
            {
                _adasProcess.Stop();
            }

            base.OnClosing(e);
        }

        private void BtnTestModbus_Click(object sender, EventArgs e)
        {
            try
            {
                string ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
                int port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

                if (_vepBenchClient != null)
                {
                    _vepBenchClient.DisConnect();
                }

                _vepBenchClient = new VEPBenchClient(ipAddress, port);
                _adasProcess = new ADASProcess(_vepBenchClient);
                _adasProcess.OnProcessStepChanged += ADASProcess_OnProcessStepChanged;

                bool success = _adasProcess.Start(ipAddress, port);

                if (success)
                {
                    BtnTestModbus.Enabled = false;
                }
                else
                {
                    MessageBox.Show("ADAS 프로세스 시작에 실패했습니다.", "시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}", "예외 발생", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
