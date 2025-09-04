using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Ki_ADAS.DB
{
    public class ModelRepository
    {
        private SettingConfigDb db;

        public ModelRepository(SettingConfigDb database)
        {
            db = database;
        }

        public List<Model> GetAllModels()
        {
            var models = new List<Model>();

            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "SELECT * FROM Model ORDER BY Name";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                models.Add(new Model
                                {
                                    Name = GetSafeString(reader, "Name"),
                                    Barcode = GetSafeString(reader, "Barcode"),
                                    Wheelbase = GetSafeNullableDouble(reader, "Wheelbase"),
                                    Fr_Distance = GetSafeNullableDouble(reader, "Fr_Distance"),
                                    Fr_Height = GetSafeNullableDouble(reader, "Fr_Height"),
                                    Fr_InterDistance = GetSafeNullableDouble(reader, "Fr_InterDistance"),
                                    Fr_Htu = GetSafeNullableDouble(reader, "Fr_Htu"),
                                    Fr_Htl = GetSafeNullableDouble(reader, "Fr_Htl"),
                                    Fr_Ts = GetSafeNullableDouble(reader, "Fr_Ts"),
                                    Fr_AlignmentAxeOffset = GetSafeNullableDouble(reader, "Fr_AlignmentAxeOffset"),
                                    Fr_Vv = GetSafeNullableDouble(reader, "Fr_Vv"),
                                    Fr_StCt = GetSafeNullableDouble(reader, "Fr_StCt"),
                                    Fr_IsTest = GetSafeBool(reader, "Fr_IsTest"),
                                    R_X = GetSafeNullableDouble(reader, "R_X"),
                                    R_Y = GetSafeNullableDouble(reader, "R_Y"),
                                    R_Z = GetSafeNullableDouble(reader, "R_Z"),
                                    R_Angle = GetSafeNullableDouble(reader, "R_Angle"),
                                    R_IsTest = GetSafeBool(reader, "R_IsTest"),
                                    L_X = GetSafeNullableDouble(reader, "L_X"),
                                    L_Y = GetSafeNullableDouble(reader, "L_Y"),
                                    L_Z = GetSafeNullableDouble(reader, "L_Z"),
                                    L_Angle = GetSafeNullableDouble(reader, "L_Angle"),
                                    L_IsTest = GetSafeBool(reader, "L_IsTest")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching models: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return models;
        }

        private string GetSafeString(OleDbDataReader reader, string columnName)
        {
            int ordinal;
            try { ordinal = reader.GetOrdinal(columnName); } catch { return string.Empty; }
            if (!reader.IsDBNull(ordinal)) { return reader.GetValue(ordinal).ToString(); }
            return string.Empty;
        }

        private bool GetSafeBool(OleDbDataReader reader, string columnName)
        {
            int ordinal;
            try { ordinal = reader.GetOrdinal(columnName); } catch { return false; }
            if (!reader.IsDBNull(ordinal)) { return Convert.ToBoolean(reader.GetValue(ordinal)); }
            return false;
        }

        private double? GetSafeNullableDouble(OleDbDataReader reader, string columnName)
        {
            int ordinal;
            try { ordinal = reader.GetOrdinal(columnName); } catch { return null; }
            if (!reader.IsDBNull(ordinal)) { return Convert.ToDouble(reader.GetValue(ordinal)); }
            return null;
        }

        public bool DeleteModel(string modelName)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "DELETE FROM Model WHERE Name = ?";
                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Name", modelName);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting model: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    public Model GetModelDetails(string modelName)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "SELECT * FROM Model WHERE Name = ?";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Name", modelName);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Model
                                {
                                    Name = GetSafeString(reader, "Name"),
                                    Barcode = GetSafeString(reader, "Barcode"),
                                    Wheelbase = GetSafeNullableDouble(reader, "Wheelbase"),
                                    Fr_Distance = GetSafeNullableDouble(reader, "Fr_Distance"),
                                    Fr_Height = GetSafeNullableDouble(reader, "Fr_Height"),
                                    Fr_InterDistance = GetSafeNullableDouble(reader, "Fr_InterDistance"),
                                    Fr_Htu = GetSafeNullableDouble(reader, "Fr_Htu"),
                                    Fr_Htl = GetSafeNullableDouble(reader, "Fr_Htl"),
                                    Fr_Ts = GetSafeNullableDouble(reader, "Fr_Ts"),
                                    Fr_AlignmentAxeOffset = GetSafeNullableDouble(reader, "Fr_AlignmentAxeOffset"),
                                    Fr_Vv = GetSafeNullableDouble(reader, "Fr_Vv"),
                                    Fr_StCt = GetSafeNullableDouble(reader, "Fr_StCt"),
                                    Fr_IsTest = GetSafeBool(reader, "Fr_IsTest"),
                                    R_X = GetSafeNullableDouble(reader, "R_X"),
                                    R_Y = GetSafeNullableDouble(reader, "R_Y"),
                                    R_Z = GetSafeNullableDouble(reader, "R_Z"),
                                    R_Angle = GetSafeNullableDouble(reader, "R_Angle"),
                                    R_IsTest = GetSafeBool(reader, "R_IsTest"),
                                    L_X = GetSafeNullableDouble(reader, "L_X"),
                                    L_Y = GetSafeNullableDouble(reader, "L_Y"),
                                    L_Z = GetSafeNullableDouble(reader, "L_Z"),
                                    L_Angle = GetSafeNullableDouble(reader, "L_Angle"),
                                    L_IsTest = GetSafeBool(reader, "L_IsTest")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching model details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        public bool AddModel(Model model)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    if (IsDuplicateName(model.Name))
                    {
                        MessageBox.Show("Model name already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    const string query = @"INSERT INTO Model (
                                        Name, Barcode, Wheelbase, Fr_Distance, Fr_Height, Fr_InterDistance,
                                        Fr_Htu, Fr_Htl, Fr_Ts, Fr_AlignmentAxeOffset, Fr_Vv, Fr_StCt, Fr_IsTest,
                                        R_X, R_Y, R_Z, R_Angle, R_IsTest,
                                        L_X, L_Y, L_Z, L_Angle, L_IsTest
                                       ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Name", model.Name);
                        cmd.Parameters.AddWithValue("Barcode", model.Barcode);
                        cmd.Parameters.AddWithValue("Wheelbase", (object)model.Wheelbase ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Distance", (object)model.Fr_Distance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Height", (object)model.Fr_Height ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_InterDistance", (object)model.Fr_InterDistance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Htu", (object)model.Fr_Htu ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Htl", (object)model.Fr_Htl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Ts", (object)model.Fr_Ts ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_AlignmentAxeOffset", (object)model.Fr_AlignmentAxeOffset ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Vv", (object)model.Fr_Vv ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_StCt", (object)model.Fr_StCt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_IsTest", model.Fr_IsTest);
                        cmd.Parameters.AddWithValue("R_X", (object)model.R_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_Y", (object)model.R_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_Z", (object)model.R_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_Angle", (object)model.R_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_IsTest", model.R_IsTest);
                        cmd.Parameters.AddWithValue("L_X", (object)model.L_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_Y", (object)model.L_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_Z", (object)model.L_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_Angle", (object)model.L_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_IsTest", model.L_IsTest);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding model: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateModel(Model model, string oldModelName)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    if (oldModelName != model.Name && IsDuplicateName(model.Name, oldModelName))
                    {
                        MessageBox.Show("Model name already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    const string query = @"UPDATE Model SET
                                        Name = ?, Barcode = ?, Wheelbase = ?, Fr_Distance = ?, Fr_Height = ?, Fr_InterDistance = ?,
                                        Fr_Htu = ?, Fr_Htl = ?, Fr_Ts = ?, Fr_AlignmentAxeOffset = ?, Fr_Vv = ?, Fr_StCt = ?, Fr_IsTest = ?,
                                        R_X = ?, R_Y = ?, R_Z = ?, R_Angle = ?, R_IsTest = ?,
                                        L_X = ?, L_Y = ?, L_Z = ?, L_Angle = ?, L_IsTest = ?
                                        WHERE Name = ?";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Name", model.Name);
                        cmd.Parameters.AddWithValue("Barcode", model.Barcode);
                        cmd.Parameters.AddWithValue("Wheelbase", (object)model.Wheelbase ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Distance", (object)model.Fr_Distance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Height", (object)model.Fr_Height ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_InterDistance", (object)model.Fr_InterDistance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Htu", (object)model.Fr_Htu ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Htl", (object)model.Fr_Htl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Ts", (object)model.Fr_Ts ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_AlignmentAxeOffset", (object)model.Fr_AlignmentAxeOffset ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_Vv", (object)model.Fr_Vv ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_StCt", (object)model.Fr_StCt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("Fr_IsTest", model.Fr_IsTest);
                        cmd.Parameters.AddWithValue("R_X", (object)model.R_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_Y", (object)model.R_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_Z", (object)model.R_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_Angle", (object)model.R_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_IsTest", model.R_IsTest);
                        cmd.Parameters.AddWithValue("L_X", (object)model.L_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_Y", (object)model.L_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_Z", (object)model.L_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_Angle", (object)model.L_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("L_IsTest", model.L_IsTest);
                        cmd.Parameters.AddWithValue("OldName", oldModelName);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating model: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool IsDuplicateName(string name, string oldName = null)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    string checkQuery;

                    if (string.IsNullOrEmpty(oldName))
                    {
                        checkQuery = "SELECT COUNT(*) FROM Model WHERE Name = ?";
                    }
                    else
                    {
                        checkQuery = "SELECT COUNT(*) FROM Model WHERE Name = ? AND Name <> ?";
                    }

                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("Name", name);

                        if (!string.IsNullOrEmpty(oldName))
                        {
                            checkCmd.Parameters.AddWithValue("OldName", oldName);
                        }

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for duplicate model name: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }

        public string GetModelNameByBarcode(string modelCode)
        {
            string modelName = null;

            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    string query = "SELECT Name FROM Model WHERE Barcode = ?";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Barcode", modelCode);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            modelName = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching model name by barcode: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return modelName;
        }
    }
}
