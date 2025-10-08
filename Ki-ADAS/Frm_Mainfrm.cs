using Ki_ADAS;
using Ki_ADAS.ThreadADAS;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public partial class Frm_Mainfrm : Form
    {
        private int m_nCurrentFrmIdx = Def.FOM_IDX_MAIN;

        public SettingConfigDb _db;
        private Form m_ActiveSubForm;
        public Frm_Main m_frmMain;
        public Frm_Config m_frmConfig;
        public Frm_Calibration m_frmCalibration = new Frm_Calibration();
        public Frm_Manual m_frmManual = new Frm_Manual();
        public Frm_Result m_frmResult;
        public Frm_VEP m_frmVEP;
        public Frm_Operator User_Monitor = null;
        public VEPBenchDescriptionZone _descriptionZone = null;
        private List<Button> m_NavButtons = new List<Button>();
        private BarcodeReader _barcodeReader;
        private GlobalVal gv;

        private IniFile _iniFile;
        public static string ipAddress;
        public static int port;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT = "VepPort";

        public Frm_Mainfrm(SettingConfigDb dbInstance)
        {
            InitializeComponent();
            
            _db = dbInstance;

            // Read network configuration
            string iniPath = System.IO.Path.Combine(Application.StartupPath, "config.ini");
            _iniFile = new IniFile(iniPath);
            ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
            port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

            gv = GlobalVal.Instance;
            gv.InitializeVepSystem(ipAddress, port);

            m_frmMain = new Frm_Main(_db, gv._Client);
            m_frmConfig = new Frm_Config(_db);
            m_frmVEP = new Frm_VEP(gv._Client);
            m_frmResult = new Frm_Result(_db);
            m_frmCalibration = new Frm_Calibration();
            m_frmManual = new Frm_Manual();
        }

        private void Frm_Mainfrm_Load(object sender, EventArgs e)
        {
            try
            {
                m_frmMain.SetParent(this);
                m_frmConfig.SetParent(this);
                m_frmCalibration.SetParent(this);
                m_frmManual.SetParent(this);
                m_frmResult.SetParent(this);
                m_frmVEP.SetParent(this);

                InitializeSubForm(m_frmMain);
                InitializeSubForm(m_frmConfig);
                InitializeSubForm(m_frmCalibration);
                InitializeSubForm(m_frmManual);
                InitializeSubForm(m_frmResult);
                InitializeSubForm(m_frmVEP);

                m_NavButtons.Add(BtnCalibration);
                m_NavButtons.Add(BtnConfig);
                m_NavButtons.Add(BtnCalibration);
                m_NavButtons.Add(BtnManual);
                m_NavButtons.Add(BtnResult);
                m_NavButtons.Add(BtnVEP);

                ShowFrm(Def.FOM_IDX_MAIN);

                if (!this.DesignMode)
                {
                    if (User_Monitor == null || User_Monitor.Text == "")
                    {
                        User_Monitor = new Frm_Operator(m_frmMain);
                        User_Monitor.Show();
                    }
                }
                
                StartBarcode();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorLoadingMainFormFrame", "Error", ex.Message);
            }
        }
        
        private void Frm_Mainfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _barcodeReader?.Disconnect();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDisconnectingBarcodeReader", "Error", ex.Message);
            }
        }

        private void Frm_Mainfrm_Resize(object sender, EventArgs e)
        {
            RepositionSubForm(this.ActiveSubForm);
        }

        private void StartBarcode()
        {
            try
            {
                _barcodeReader = new BarcodeReader(Frm_Main.barcodeIp);
                _barcodeReader.OnBarcodeReceived += BarcodeReceivedHandler;
                _barcodeReader.OnError += BarcodeReader_OnError;
                _barcodeReader.ConnectBarcodeReader();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorConnectingBarcodeReader", "Error", ex.Message);
            }
        }

        private void BarcodeReader_OnError(object sender, Exception e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => MsgBox.ErrorWithFormat("ErrorInBarcodeReadLoop", "Error", e.Message)));
            }
            else
            {
                MsgBox.ErrorWithFormat("ErrorInBarcodeReadLoop", "Error", e.Message);
            }
        }

        private void BarcodeReceivedHandler(object sender, BarcodeReader.BarcodeEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ProcessBarcode(e.BarcodeData)));
            }
            else
            {
                ProcessBarcode(e.BarcodeData);
            }
        }

        private void ProcessBarcode(string barcodeData)
        {
            if (m_frmMain != null && !m_frmMain.IsDisposed)
            {
                m_frmMain.SetBarcodeData(barcodeData);
            }
        }

        private void InitializeSubForm(Form f)
        {
            try
            {
                f.ControlBox = false;
                f.FormBorderStyle = FormBorderStyle.None;
                f.StartPosition = FormStartPosition.Manual;
                f.MdiParent = this;
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorInitializingSubForm", "Error", ex.Message);
            }
        }

        private void RepositionSubForm(Form fSubform)
        {
            try
            {
                if (fSubform == null)
                    return;

                fSubform.Location = new Point(0, 0);
                fSubform.Size = new Size(ClientSize.Width - panelNavBar.Size.Width - 4, ClientSize.Height - 4);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorRepositioningSubForm", "Error", ex.Message);
            }
        }

        public Form ActiveSubForm
        {
            get
            {
                return m_ActiveSubForm;
            }
            set
            {
                m_ActiveSubForm = value;
                if (m_ActiveSubForm == null)
                    return;

                RepositionSubForm(m_ActiveSubForm);
            }
        }

        private void ShowFrm(int nIdx)
        {
            try
            {
                m_nCurrentFrmIdx = nIdx;
                Form f = new Form();

                switch (nIdx)
                {
                    case Def.FOM_IDX_MAIN:
                        f = m_frmMain;
                        break;
                    case Def.FOM_IDX_CONFIG:
                        f = m_frmConfig;
                        break;
                    case Def.FOM_IDX_CALIBRATION:
                        f = m_frmCalibration;
                        break;
                    case Def.FOM_IDX_MANUAL:
                        f = m_frmManual;
                        break;
                    case Def.FOM_IDX_RESULT:
                        f = m_frmResult;
                        break;
                    case Def.FOM_IDX_VEP:
                        f = m_frmVEP;
                        break;
                }

                f.Show();
                f.BringToFront();
                ActiveSubForm = f;
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorShowingForm", "Error", ex.Message);
            }
        }

        private void ChangeButtonColor(Button pButton)
        {
            try
            {
                foreach (Button btn in m_NavButtons)
                {
                    if (pButton.Text == btn.Text)
                    {
                        btn.BackColor = Color.Gray;
                        btn.ForeColor = SystemColors.ControlLightLight;
                    }
                    else
                    {
                        btn.BackColor = Color.Gainsboro;
                        btn.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorChangingButtonColor", "Error", ex.Message);
            }
        }

        private void BtnMain_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeButtonColor((Button)sender);
                ShowFrm(Def.FOM_IDX_MAIN);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMainButtonClick", "Error", ex.Message);
            }
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeButtonColor((Button)sender);
                ShowFrm(Def.FOM_IDX_CONFIG);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorConfigButtonClick", "Error", ex.Message);
            }
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeButtonColor((Button)sender);
                ShowFrm(Def.FOM_IDX_CALIBRATION);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCalibrationButtonClick", "Error", ex.Message);
            }
        }

        private void BtnManual_Click_1(object sender, EventArgs e)
        {
            try
            {
                ChangeButtonColor((Button)sender);
                ShowFrm(Def.FOM_IDX_MANUAL);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorManualButtonClick", "Error", ex.Message);
            }
        }

        private void BtnResult_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeButtonColor((Button)sender);
                ShowFrm(Def.FOM_IDX_RESULT);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorResultButtonClick", "Error", ex.Message);
            }
        }

        private void BtnVEP_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeButtonColor((Button)sender);
                ShowFrm(Def.FOM_IDX_VEP);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorVEPButtonClick", "Error", ex.Message);
            }
        }
    }
}