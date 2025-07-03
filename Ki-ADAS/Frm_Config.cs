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
    public partial class Frm_Config : Form
    {
        private Frm_Mainfrm m_frmParent = null;
        private IniFile _iniFile;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT_KEY = "VepPort";

        public Frm_Config()
        {
            InitializeComponent();
            InitializeConfig();
        }
        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void InitializeConfig()
        {
            string iniPath = Path.Combine(Application.StartupPath, "config.ini");
            _iniFile = new IniFile(iniPath);
        }

        private void Frm_Config_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            TxtVepIp.Text = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
            TxtVepPort.Text = _iniFile.ReadValue(CONFIG_SECTION, VEP_PORT_KEY);
        }

        public void SaveSettings()
        {
            _iniFile.WriteValue(CONFIG_SECTION, VEP_IP_KEY, TxtVepIp.Text);
            _iniFile.WriteValue(CONFIG_SECTION, VEP_PORT_KEY, TxtVepPort.Text);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MessageBox.Show("설정이 저장되었습니다.", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string GetVepIp()
        {
            return TxtVepIp.Text;
        }

        public int GetVepPort()
        {
            if (int.TryParse(TxtVepPort.Text, out int port))
                return port;

            return 502;
        }
    }
}
