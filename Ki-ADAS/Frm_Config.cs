using Ki_ADAS.VEPBench;
using Ki_ADAS.DB;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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
        private const string LANGUAGE_KEY = "System";
        private const string LANGUAGE_SECTION = "Language";

        private SettingConfigDb db;
        private ModelRepository _modelRepository;

        public Frm_Config(SettingConfigDb dbInstance)
        {
            InitializeComponent();
            InitializeConfig();
            this.db = dbInstance;
            _modelRepository = new ModelRepository(dbInstance);
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
            LoadModelList();
        }

        private void LoadModelList()
        {
            try
            {
                modelList.Items.Clear();
                var models = _modelRepository.GetAllModels();

                foreach (var model in models)
                {
                    ListViewItem item = new ListViewItem(model.Name);
                    modelList.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.GetString("DatabaseError")}: {ex.Message}",
                                LanguageManager.GetString("Error"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        public void LoadSettings()
        {
            TxtVepIp.Text = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
            TxtVepPort.Text = _iniFile.ReadValue(CONFIG_SECTION, VEP_PORT_KEY);

            string languageStr = _iniFile.ReadValue(LANGUAGE_SECTION, LANGUAGE_KEY, "0");
            int languageIndex = 0;

            if (int.TryParse(languageStr, out languageIndex) && languageIndex >= 0 && languageIndex < cmb_language.Items.Count)
            {
                cmb_language.SelectedIndex = languageIndex;
            }
            else
            {
                cmb_language.SelectedIndex = 0; // 기본값으로 영어 선택
            }
        }

        public void SaveLanguageSettings()
        {
            if (cmb_language.SelectedItem != null)
            {
                Language selectedLanguage = Language.English;

                switch (cmb_language.SelectedIndex)
                {
                    case 0:
                        selectedLanguage = Language.English;
                        break;
                    case 1:
                        selectedLanguage = Language.Portuguese;
                        break;
                    case 2:
                        selectedLanguage = Language.Korean;
                        break;
                }

                LanguageManager.ChangeLanguage(selectedLanguage);

                _iniFile.WriteValue(LANGUAGE_SECTION, LANGUAGE_KEY, cmb_language.SelectedIndex.ToString());

                MessageBox.Show(LanguageManager.GetString("LanguageChangeSuccess"),
                    LanguageManager.GetString("Information"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void BtnConfigSave_Click(object sender, EventArgs e)
        {
            _iniFile.WriteValue(CONFIG_SECTION, VEP_IP_KEY, TxtVepIp.Text);
            _iniFile.WriteValue(CONFIG_SECTION, VEP_PORT_KEY, TxtVepPort.Text);

            Frm_Main.ipAddress = TxtVepIp.Text;
            Frm_Main.port = int.Parse(TxtVepPort.Text);

            MessageBox.Show(LanguageManager.GetString("ConfigSaveSuccess"),
                    LanguageManager.GetString("Information"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void BtnLanSave_Click(object sender, EventArgs e)
        {
            SaveLanguageSettings();
        }

        private void cmb_language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_language.SelectedItem != null)
            {
                Language selectedLanguage = Language.English;

                switch (cmb_language.SelectedIndex)
                {
                    case 0:
                        selectedLanguage = Language.English;
                        break;
                    case 1:
                        selectedLanguage = Language.Portuguese;
                        break;
                    case 2:
                        selectedLanguage = Language.Korean;
                        break;
                }

                LanguageManager.ChangeLanguage(selectedLanguage);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show(LanguageManager.GetString("ModelNameRequired"),
                                LanguageManager.GetString("Warning"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (_modelRepository.AddModel(_modelRepository.GetModelDetails(txtModel.Text)))
            {
                MessageBox.Show(LanguageManager.GetString("ModelAddSuccess"),
                                LanguageManager.GetString("Information"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                txtModel.Text = string.Empty;
                LoadModelList();
                ClearAllFields();
            }
        }

        private void BtnModify_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedItems.Count == 0)
            {
                MessageBox.Show(LanguageManager.GetString("PleaseSelectModel"),
                                LanguageManager.GetString("Warning"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show(LanguageManager.GetString("ModelNameRequired"),
                                LanguageManager.GetString("Warning"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string oldModelName = modelList.SelectedItems[0].Text;

            if (_modelRepository.UpdateModel(_modelRepository.GetModelDetails(txtModel.Text), oldModelName))
            {
                MessageBox.Show(LanguageManager.GetString("ModelUpdateSuccess"),
                                LanguageManager.GetString("Information"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                LoadModelList();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (modelList.SelectedItems.Count == 0)
                {
                    MessageBox.Show(LanguageManager.GetString("PleaseSelectModel"),
                                LanguageManager.GetString("Warning"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                    return;
                }

                string modelName = modelList.SelectedItems[0].Text;

                DialogResult result = MessageBox.Show(
                    string.Format(LanguageManager.GetFormattedString("ConfirmDeleteModel", modelName),
                                LanguageManager.GetString("Information"),
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question));

                if (result != DialogResult.Yes)
                    return;

                if (_modelRepository.DeleteModel(modelName))
                {
                    modelList.SelectedItems[0].Remove();
                    txtModel.Text = string.Empty;
                    ClearAllFields();

                    MessageBox.Show(LanguageManager.GetString("ModelDeleteSuccess"),
                                LanguageManager.GetString("Information"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                    LoadModelList();
                }
                else
                {
                    MessageBox.Show(LanguageManager.GetString("ModelDeleteFailed"),
                                LanguageManager.GetString("Error"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.GetString("DatabaseError")}: {ex.Message}",
                    LanguageManager.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }   
        }

        private double? ParseNullableDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return null;
        }

        private void ClearAllFields()
        {
            // 상단 필드
            txtBarcode.Text = string.Empty;
            txtWheelbase.Text = string.Empty;

            // Front Camera 섹션
            txtDistance.Text = string.Empty;
            txtHeight.Text = string.Empty;
            txtInterDistance.Text = string.Empty;
            txtHtu.Text = string.Empty;
            txtHtl.Text = string.Empty;
            txtTs.Text = string.Empty;
            txtOffset.Text = string.Empty;
            txtVv.Text = string.Empty;
            txtStCt.Text = string.Empty;
            chkIsFrontCameraTest.Checked = false;

            // Rear Radar 섹션
            txtRLX.Text = string.Empty;
            txtRLY.Text = string.Empty;
            txtRLZ.Text = string.Empty;
            txtRLAngle.Text = string.Empty;
            txtRRX.Text = string.Empty;
            txtRRY.Text = string.Empty;
            txtRRZ.Text = string.Empty;
            txtRRAngle.Text = string.Empty;
            chkIsRearRadar.Checked = false;

            // Front Radar 섹션
            txtFLX.Text = string.Empty;
            txtFLY.Text = string.Empty;
            txtFLZ.Text = string.Empty;
            txtFLAngle.Text = string.Empty;
            txtFRX.Text = string.Empty;
            txtFRY.Text = string.Empty;
            txtFRZ.Text = string.Empty;
            txtFRAngle.Text = string.Empty;
            chkIsFrontRadar.Checked = false;
        }

        private void modelList_MouseClick(object sender, MouseEventArgs e)
        {
            if (modelList.SelectedItems.Count > 0)
            {
                string modelName = modelList.SelectedItems[0].Text;
                txtModel.Text = modelName;

                var selectedModel = _modelRepository.GetModelDetails(modelName);

                if (selectedModel != null)
                {
                    txtBarcode.Text = selectedModel.Barcode;
                    txtWheelbase.Text = selectedModel.Wheelbase?.ToString();

                    // Front Camera
                    txtDistance.Text = selectedModel.FC_Distance?.ToString();
                    txtHeight.Text = selectedModel.FC_Height?.ToString();
                    txtInterDistance.Text = selectedModel.FC_InterDistance?.ToString();
                    txtHtu.Text = selectedModel.FC_Htu?.ToString();
                    txtHtl.Text = selectedModel.FC_Htl?.ToString();
                    txtTs.Text = selectedModel.FC_Ts?.ToString();
                    txtOffset.Text = selectedModel.FC_AlignmentAxeOffset?.ToString();
                    txtVv.Text = selectedModel.FC_Vv?.ToString();
                    txtStCt.Text = selectedModel.FC_StCt?.ToString();
                    chkIsFrontCameraTest.Checked = selectedModel.FC_IsTest;

                    // Front Radar
                    txtFLX.Text = selectedModel.FR_X?.ToString();
                    txtFLY.Text = selectedModel.FR_Y?.ToString();
                    txtFLZ.Text = selectedModel.FR_Z?.ToString();
                    txtFLAngle.Text = selectedModel.FR_Angle?.ToString();
                    txtFRX.Text = selectedModel.FL_X?.ToString();
                    txtFRY.Text = selectedModel.FL_Y?.ToString();
                    txtFRZ.Text = selectedModel.FL_Z?.ToString();
                    txtFRAngle.Text = selectedModel.FL_Angle?.ToString();
                    chkIsFrontRadar.Checked = selectedModel.F_IsTest;

                    // Rear Radar
                    txtRLX.Text = selectedModel.RR_X?.ToString();
                    txtRLY.Text = selectedModel.RR_Y?.ToString();
                    txtRLZ.Text = selectedModel.RR_Z?.ToString();
                    txtRLAngle.Text = selectedModel.RR_Angle?.ToString();
                    txtRRX.Text = selectedModel.RL_X?.ToString();
                    txtRRY.Text = selectedModel.RL_Y?.ToString();
                    txtRRZ.Text = selectedModel.RL_Z?.ToString();
                    txtRRAngle.Text = selectedModel.RL_Angle?.ToString();
                    chkIsRearRadar.Checked = selectedModel.R_IsTest;
                }
                else
                {
                    ClearAllFields();
                    MessageBox.Show(LanguageManager.GetString("NoModelDetailsFound"),
                                LanguageManager.GetString("Information"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
            }
        }
    }
}