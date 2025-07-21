using Ki_ADAS.VEPBench;

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

            string ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
            int port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

            _vepBenchClient = new VEPBenchClient(ipAddress, port);
            _adasProcess = new ADASProcess(_vepBenchClient);

            _adasProcess.OnProcessStepChanged += ADASProcess_OnProcessStepChanged;

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
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _adasProcess.Stop();

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

            string logMessage = $"[{e.Timestamp:HH:mm:ss}] [{e.StateType}] {e.Message}";

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
