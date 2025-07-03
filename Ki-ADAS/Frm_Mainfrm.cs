using Ki_ADAS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public partial class Frm_Mainfrm : Form
    {

        private int m_nCurrentFrmIdx = Def.FOM_IDX_MAIN;

        private Form m_ActiveSubForm;
        public Frm_Main m_frmMain = new Frm_Main();
        public Frm_Config m_frmConfig = new Frm_Config();
        public Frm_Calibration m_frmCalibration = new Frm_Calibration();
        public Frm_Manual m_frmManual = new Frm_Manual();
        public Frm_Result m_frmResult = new Frm_Result();
        public Frm_VEP m_frmVEP = new Frm_VEP();

        private List<Button> m_NavButtons = new List<Button>();

        public Frm_Mainfrm()
        {
            InitializeComponent();
        }
        private void Frm_Mainfrm_Load(object sender, EventArgs e)
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
        }

        private void InitializeSubForm(Form f)
        {
            f.ControlBox = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.StartPosition = FormStartPosition.Manual;
            f.MdiParent = this;
        }


        private void RepositionSubForm(Form fSubform)
        {
            if (fSubform == null)
                return;
            fSubform.Location = new Point(0, 0);
            fSubform.Size = new Size(ClientSize.Width - panelNavBar.Size.Width - 4, ClientSize.Height - 4);

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

        private void ChangeButtonColor(Button pButton)
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

        private void BtnMain_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_MAIN);
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_CONFIG);
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_CALIBRATION);
        }

        private void BtnManual_Click_1(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_MANUAL);
        }

        private void BtnResult_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_RESULT);
        }

        private void BtnVEP_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_VEP);
        }
    }
}
