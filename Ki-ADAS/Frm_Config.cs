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
    public partial class Frm_Config : MultiLanguageForm
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

            LanguageManager.RegisterForm(this);
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
                MessageBox.Show($"{LanguageResource.GetMessage("DatabaseError")}: {ex.Message}",
                   LanguageResource.GetMessage("Error"),
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

                MessageBox.Show(LanguageResource.GetMessage("LanguageChangeSuccess"),
                    LanguageResource.GetMessage("Information"),
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

            MessageBox.Show(LanguageResource.GetMessage("ConfigSaveSuccess"),
                LanguageResource.GetMessage("Information"),
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
                MessageBox.Show(LanguageResource.GetMessage("ModelNameRequired"),
                                LanguageResource.GetMessage("Warning"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            var newModel = new Model
            {
                Name = txtModel.Text.Trim(),
                Barcode = txtBarcode.Text.Trim(),
                Wheelbase = ParseNullableDouble(txtWheelbase.Text),
                Fr_Distance = ParseNullableDouble(txtDistance.Text),
                Fr_Height = ParseNullableDouble(txtHeight.Text),
                Fr_InterDistance = ParseNullableDouble(txtInterDistance.Text),
                Fr_Htu = ParseNullableDouble(txtHtu.Text),
                Fr_Htl = ParseNullableDouble(txtHtl.Text),
                Fr_Ts = ParseNullableDouble(txtTs.Text),
                Fr_AlignmentAxeOffset = ParseNullableDouble(txtOffset.Text),
                Fr_Vv = ParseNullableDouble(txtVv.Text),
                Fr_StCt = ParseNullableDouble(txtStCt.Text),
                Fr_IsTest = chkIsFrontCameraTest.Checked,
                R_X = ParseNullableDouble(txtRX.Text),
                R_Y = ParseNullableDouble(txtRY.Text),
                R_Z = ParseNullableDouble(txtRZ.Text),
                R_Angle = ParseNullableDouble(txtRAngle.Text),
                R_IsTest = chkIsRearRightRadar.Checked,
                L_X = ParseNullableDouble(txtLX.Text),
                L_Y = ParseNullableDouble(txtLY.Text),
                L_Z = ParseNullableDouble(txtLZ.Text),
                L_Angle = ParseNullableDouble(txtLAngle.Text),
                L_IsTest = chkIsRearLeftRadar.Checked
            };

            if (_modelRepository.AddModel(newModel))
            {
                MessageBox.Show(LanguageResource.GetMessage("ModelAddSuccess"),
                                LanguageResource.GetMessage("Information"),
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
                MessageBox.Show(LanguageResource.GetMessage("PleaseSelectModel"),
                                LanguageResource.GetMessage("Warning"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show(LanguageResource.GetMessage("ModelNameRequired"),
                                LanguageResource.GetMessage("Warning"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string oldModelName = modelList.SelectedItems[0].Text;

            var updatedModel = new Model
            {
                Name = txtModel.Text.Trim(),
                Barcode = txtBarcode.Text.Trim(),
                Wheelbase = ParseNullableDouble(txtWheelbase.Text),
                Fr_Distance = ParseNullableDouble(txtDistance.Text),
                Fr_Height = ParseNullableDouble(txtHeight.Text),
                Fr_InterDistance = ParseNullableDouble(txtInterDistance.Text),
                Fr_Htu = ParseNullableDouble(txtHtu.Text),
                Fr_Htl = ParseNullableDouble(txtHtl.Text),
                Fr_Ts = ParseNullableDouble(txtTs.Text),
                Fr_AlignmentAxeOffset = ParseNullableDouble(txtOffset.Text),
                Fr_Vv = ParseNullableDouble(txtVv.Text),
                Fr_StCt = ParseNullableDouble(txtStCt.Text),
                Fr_IsTest = chkIsFrontCameraTest.Checked,
                R_X = ParseNullableDouble(txtRX.Text),
                R_Y = ParseNullableDouble(txtRY.Text),
                R_Z = ParseNullableDouble(txtRZ.Text),
                R_Angle = ParseNullableDouble(txtRAngle.Text),
                R_IsTest = chkIsRearRightRadar.Checked,
                L_X = ParseNullableDouble(txtLX.Text),
                L_Y = ParseNullableDouble(txtLY.Text),
                L_Z = ParseNullableDouble(txtLZ.Text),
                L_Angle = ParseNullableDouble(txtLAngle.Text),
                L_IsTest = chkIsRearLeftRadar.Checked
            };

            if (_modelRepository.UpdateModel(updatedModel, oldModelName))
            {
                MessageBox.Show(LanguageResource.GetMessage("ModelUpdateSuccess"),
                                LanguageResource.GetMessage("Information"),
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
                    MessageBox.Show(LanguageResource.GetMessage("PleaseSelectModel"),
                        LanguageResource.GetMessage("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                string modelName = modelList.SelectedItems[0].Text;

                DialogResult result = MessageBox.Show(
                    string.Format(LanguageResource.GetMessage("ConfirmDeleteModel"), modelName),
                    LanguageResource.GetMessage("Confirmation"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                if (_modelRepository.DeleteModel(modelName))
                {
                    modelList.SelectedItems[0].Remove();
                    txtModel.Text = string.Empty;
                    ClearAllFields();

                    MessageBox.Show(LanguageResource.GetMessage("ModelDeleteSuccess"),
                        LanguageResource.GetMessage("Information"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadModelList();
                }
                else
                {
                    MessageBox.Show(LanguageResource.GetMessage("ModelDeleteFailed"),
                        LanguageResource.GetMessage("Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageResource.GetMessage("DatabaseError")}: {ex.Message}",
                    LanguageResource.GetMessage("Error"),
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

            // Rear Right Radar 섹션
            txtRX.Text = string.Empty;
            txtRY.Text = string.Empty;
            txtRZ.Text = string.Empty;
            txtRAngle.Text = string.Empty;
            chkIsRearRightRadar.Checked = false;

            // Rear Left Radar 섹션
            txtLX.Text = string.Empty;
            txtLY.Text = string.Empty;
            txtLZ.Text = string.Empty;
            txtLAngle.Text = string.Empty;
            chkIsRearLeftRadar.Checked = false;
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
                    txtDistance.Text = selectedModel.Fr_Distance?.ToString();
                    txtHeight.Text = selectedModel.Fr_Height?.ToString();
                    txtInterDistance.Text = selectedModel.Fr_InterDistance?.ToString();
                    txtHtu.Text = selectedModel.Fr_Htu?.ToString();
                    txtHtl.Text = selectedModel.Fr_Htl?.ToString();
                    txtTs.Text = selectedModel.Fr_Ts?.ToString();
                    txtOffset.Text = selectedModel.Fr_AlignmentAxeOffset?.ToString();
                    txtVv.Text = selectedModel.Fr_Vv?.ToString();
                    txtStCt.Text = selectedModel.Fr_StCt?.ToString();
                    chkIsFrontCameraTest.Checked = selectedModel.Fr_IsTest;
                    txtRX.Text = selectedModel.R_X?.ToString();
                    txtRY.Text = selectedModel.R_Y?.ToString();
                    txtRZ.Text = selectedModel.R_Z?.ToString();
                    txtRAngle.Text = selectedModel.R_Angle?.ToString();
                    chkIsRearRightRadar.Checked = selectedModel.R_IsTest;
                    txtLX.Text = selectedModel.L_X?.ToString();
                    txtLY.Text = selectedModel.L_Y?.ToString();
                    txtLZ.Text = selectedModel.L_Z?.ToString();
                    txtLAngle.Text = selectedModel.L_Angle?.ToString();
                    chkIsRearLeftRadar.Checked = selectedModel.L_IsTest;
                }
                else
                {
                    ClearAllFields();
                    MessageBox.Show(LanguageResource.GetMessage("NoModelDetailsFound"),
                                        LanguageResource.GetMessage("Information"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                }
            }
        }
    }
}