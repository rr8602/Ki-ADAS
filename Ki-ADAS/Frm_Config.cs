using Ki_ADAS.VEPBench;

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

            LanguageManager.RegisterForm(this);
        }

        private void Frm_Config_Load(object sender, EventArgs e)
        {
            LoadSettings();
            db = new SettingConfigDb();
            db.SetupDatabaseConnection();


            LoadModelList();
        }

        private void LoadModelList()
        {
            try
            {
                modelList.Items.Clear();

                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    string query = "SELECT Name FROM Model ORDER BY Name";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["Name"].ToString());

                                modelList.Items.Add(item);
                            }
                        }
                    }
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

        public void SaveModelSettings()
        {

        }

        public void SaveConfigSettings()
        {
            _iniFile.WriteValue(CONFIG_SECTION, VEP_IP_KEY, TxtVepIp.Text);
            _iniFile.WriteValue(CONFIG_SECTION, VEP_PORT_KEY, TxtVepPort.Text);
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

        private bool IsDuplicateModelName(OleDbConnection con, string newModelName, string oldModelName = null)
        {
            try
            {
                string checkQuery;

                if (string.IsNullOrEmpty(oldModelName))
                {
                    checkQuery = "SELECT COUNT(*) FROM Model WHERE Name = ?";
                }
                else
                {
                    checkQuery = "SELECT COUNT(*) FROM Model WHERE Name = ? AND Name <> ?";
                }

                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("Name", newModelName);

                    if (!string.IsNullOrEmpty(oldModelName))
                    {
                        checkCmd.Parameters.AddWithValue("OldName", oldModelName);
                    }

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"중복 모델명 검사 중 오류: {ex.Message}");

                return true;
            }
        }

        private void BtnModelSave_Click(object sender, EventArgs e)
        {
            SaveModelData();
        }

        private void BtnConfigSave_Click(object sender, EventArgs e)
        {
            SaveConfigSettings();

            MessageBox.Show(LanguageResource.GetMessage("ConfigSaveSuccess"),
                LanguageResource.GetMessage("Information"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

                _iniFile.WriteValue(LANGUAGE_SECTION, LANGUAGE_KEY, cmb_language.SelectedIndex.ToString());
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtModel.Text))
                {
                    MessageBox.Show(LanguageResource.GetMessage("ModelNameRequired"),
                        LanguageResource.GetMessage("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                string newModelName = txtModel.Text.Trim();

                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    if (IsDuplicateModelName(con, newModelName))
                    {
                        MessageBox.Show(LanguageResource.GetMessage("ModelNameAlreadyExists"),
                            LanguageResource.GetMessage("Warning"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    OleDbTransaction transaction = con.BeginTransaction();

                    try
                    {
                        string query = "INSERT INTO Model (Name) VALUES (@Name)";

                        using (OleDbCommand cmd = new OleDbCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Name", txtModel.Text.Trim());

                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                transaction.Commit();

                                MessageBox.Show(LanguageResource.GetMessage("ModelAddSuccess"),
                                    LanguageResource.GetMessage("Information"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                txtModel.Text = string.Empty;

                                LoadModelList();
                            }
                            else
                            {
                                transaction.Rollback();

                                MessageBox.Show(LanguageResource.GetMessage("ModelAddFailed"),
                                    LanguageResource.GetMessage("Error"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
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

        private void BtnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtModel.Text))
                {
                    MessageBox.Show(LanguageResource.GetMessage("ModelNameRequired"),
                        LanguageResource.GetMessage("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (modelList.SelectedItems.Count == 0)
                {
                    MessageBox.Show(LanguageResource.GetMessage("PleaseSelectModel"),
                        LanguageResource.GetMessage("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                string oldModelName = modelList.SelectedItems[0].Text;
                string newModelName = txtModel.Text.Trim();

                if (oldModelName == newModelName)
                {
                    return;
                }

                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    if (IsDuplicateModelName(con, newModelName, oldModelName))
                    {
                        MessageBox.Show(LanguageResource.GetMessage("ModelNameAlreadyExists"),
                            LanguageResource.GetMessage("Warning"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    OleDbTransaction transaction = con.BeginTransaction();

                    try
                    {
                        string query = "UPDATE Model SET Name = ? WHERE Name = ?";

                        using (OleDbCommand cmd = new OleDbCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("NewName", newModelName);
                            cmd.Parameters.AddWithValue("OldName", oldModelName);

                            int result = cmd.ExecuteNonQuery();
                            
                            if (result > 0)
                            {
                                transaction.Commit();

                                modelList.SelectedItems[0].Text = newModelName;

                                MessageBox.Show(LanguageResource.GetMessage("ModelUpdateSuccess"),
                                    LanguageResource.GetMessage("Information"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                LoadModelList();
                            }
                            else
                            {
                                transaction.Rollback();

                                MessageBox.Show(LanguageResource.GetMessage("ModelUpdateFailed"),
                                    LanguageResource.GetMessage("Error"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
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

                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    OleDbTransaction transaction = con.BeginTransaction();

                    try
                    {
                        string query = "DELETE FROM Model WHERE Name = ?";

                        using (OleDbCommand cmd = new OleDbCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("Name", modelName);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();

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
                                transaction.Rollback();

                                MessageBox.Show(LanguageResource.GetMessage("ModelDeleteFailed"),
                                    LanguageResource.GetMessage("Error"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch
                    {
                        transaction.Rollback();

                        return;
                    }
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

        private void modelList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (modelList.SelectedItems.Count > 0)
            {
                string modelName = modelList.SelectedItems[0].Text;
                txtModel.Text = modelName;

                LoadModelDetails(modelName);
            }
        }

        private void SaveModelData()
        {
            try
            {
                string modelName = modelList.SelectedItems[0].Text;

                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    OleDbTransaction transaction = con.BeginTransaction();

                    try
                    {
                        string query = @"UPDATE Model SET
                                        Barcode = ?,
                                        Wheelbase = ?,
                                        Fr_Distance = ?,
                                        Fr_Height = ?,
                                        Fr_InterDistance = ?,
                                        Fr_Htu = ?,
                                        Fr_Htl = ?,
                                        Fr_Ts = ?,
                                        Fr_AlignmentAxeOffset = ?,
                                        Fr_Vv = ?,
                                        Fr_StCt = ?,
                                        R_X = ?,
                                        R_Y = ?,
                                        R_Z = ?,
                                        R_Angle = ?,
                                        L_X = ?,
                                        L_Y = ?,
                                        L_Z = ?,
                                        L_Angle = ?
                                        WHERE Name = ?";

                        using (OleDbCommand cmd = new OleDbCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("Barcode", txtBarcode.Text.Trim());
                            cmd.Parameters.AddWithValue("Wheelbase", txtWheelbase.Text.Trim());

                            cmd.Parameters.AddWithValue("Fr_Distance", txtDistance.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_Height", txtHeight.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_InterDistance", txtInterDistance.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_Htu", txtHtu.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_Htl", txtHtl.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_Ts", txtTs.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_AlignmentAxeOffset", txtOffset.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_Vv", txtVv.Text.Trim());
                            cmd.Parameters.AddWithValue("Fr_StCt", txtStCt.Text.Trim());

                            cmd.Parameters.AddWithValue("R_X", txtRX.Text.Trim());
                            cmd.Parameters.AddWithValue("R_Y", txtRY.Text.Trim());
                            cmd.Parameters.AddWithValue("R_Z", txtRZ.Text.Trim());
                            cmd.Parameters.AddWithValue("R_Angle", txtRAngle.Text.Trim());

                            cmd.Parameters.AddWithValue("L_X", txtLX.Text.Trim());
                            cmd.Parameters.AddWithValue("L_Y", txtLY.Text.Trim());
                            cmd.Parameters.AddWithValue("L_Z", txtLZ.Text.Trim());
                            cmd.Parameters.AddWithValue("L_Angle", txtLAngle.Text.Trim());

                            cmd.Parameters.AddWithValue("Name", modelName);

                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                transaction.Commit();

                                MessageBox.Show(LanguageResource.GetMessage("ModelSaveSuccess"),
                                    LanguageResource.GetMessage("Information"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }
                            else
                            {
                                transaction.Rollback();

                                MessageBox.Show(LanguageResource.GetMessage("ModelSaveFailed"),
                                    LanguageResource.GetMessage("Error"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
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

        private void LoadModelDetails(string modelName)
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM Model WHERE Name = @Name";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", modelName);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 상단 필드
                                txtBarcode.Text = GetSafeString(reader, "Barcode");
                                txtWheelbase.Text = GetSafeString(reader, "Wheelbase");

                                // Front Camera 섹션
                                txtDistance.Text = GetSafeString(reader, "Fr_Distance");
                                txtHeight.Text = GetSafeString(reader, "Fr_Height");
                                txtInterDistance.Text = GetSafeString(reader, "Fr_InterDistance");
                                txtHtu.Text = GetSafeString(reader, "Fr_Htu");
                                txtHtl.Text = GetSafeString(reader, "Fr_Htl");
                                txtTs.Text = GetSafeString(reader, "Fr_Ts");
                                txtOffset.Text = GetSafeString(reader, "Fr_AlignmentAxeOffset");
                                txtVv.Text = GetSafeString(reader, "Fr_Vv");
                                txtStCt.Text = GetSafeString(reader, "Fr_StCt");

                                // Rear Right Radar 섹션
                                txtRX.Text = GetSafeString(reader, "R_X");
                                txtRY.Text = GetSafeString(reader, "R_Y");
                                txtRZ.Text = GetSafeString(reader, "R_Z");
                                txtRAngle.Text = GetSafeString(reader, "R_Angle");

                                // Rear Left Radar 섹션
                                txtLX.Text = GetSafeString(reader, "L_X");
                                txtLY.Text = GetSafeString(reader, "L_Y");
                                txtLZ.Text = GetSafeString(reader, "L_Z");
                                txtLAngle.Text = GetSafeString(reader, "L_Angle");
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
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageResource.GetMessage("DatabaseError")}: {ex.Message}",
                    LanguageResource.GetMessage("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string GetSafeString(OleDbDataReader reader, string columnName)
        {
            int ordinal;

            try
            {
                ordinal = reader.GetOrdinal(columnName);
            }
            catch
            {
                return string.Empty;
            }

            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetValue(ordinal).ToString();
            }

            return string.Empty;
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

            // Rear Right Radar 섹션
            txtRX.Text = string.Empty;
            txtRY.Text = string.Empty;
            txtRZ.Text = string.Empty;
            txtRAngle.Text = string.Empty;

            // Rear Right Radar 섹션
            txtLX.Text = string.Empty;
            txtLY.Text = string.Empty;
            txtLZ.Text = string.Empty;
            txtLAngle.Text = string.Empty;
        }
    }
}