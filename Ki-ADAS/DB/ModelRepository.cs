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

                                    // Front Camera
                                    FC_Distance = GetSafeNullableDouble(reader, "FC_Distance"),
                                    FC_Height = GetSafeNullableDouble(reader, "FC_Height"),
                                    FC_InterDistance = GetSafeNullableDouble(reader, "FC_InterDistance"),
                                    FC_Htu = GetSafeNullableDouble(reader, "FC_Htu"),
                                    FC_Htl = GetSafeNullableDouble(reader, "FC_Htl"),
                                    FC_Ts = GetSafeNullableDouble(reader, "FC_Ts"),
                                    FC_AlignmentAxeOffset = GetSafeNullableDouble(reader, "FC_AlignmentAxeOffset"),
                                    FC_Vv = GetSafeNullableDouble(reader, "FC_Vv"),
                                    FC_StCt = GetSafeNullableDouble(reader, "FC_StCt"),
                                    FC_IsTest = GetSafeBool(reader, "FC_IsTest"),

                                    // Frpmt Radar
                                    FR_X = GetSafeNullableDouble(reader, "FR_X"),
                                    FR_Y = GetSafeNullableDouble(reader, "FR_Y"),
                                    FR_Z = GetSafeNullableDouble(reader, "FR_Z"),
                                    FR_Angle = GetSafeNullableDouble(reader, "FR_Angle"),
                                    FL_X = GetSafeNullableDouble(reader, "FL_X"),
                                    FL_Y = GetSafeNullableDouble(reader, "FL_Y"),
                                    FL_Z = GetSafeNullableDouble(reader, "FL_Z"),
                                    FL_Angle = GetSafeNullableDouble(reader, "FL_Angle"),
                                    F_IsTest = GetSafeBool(reader, "F_IsTest"),

                                    // Rear Radar
                                    RR_X = GetSafeNullableDouble(reader, "RR_X"),
                                    RR_Y = GetSafeNullableDouble(reader, "RR_Y"),
                                    RR_Z = GetSafeNullableDouble(reader, "RR_Z"),
                                    RR_Angle = GetSafeNullableDouble(reader, "RR_Angle"),
                                    RL_X = GetSafeNullableDouble(reader, "RL_X"),
                                    RL_Y = GetSafeNullableDouble(reader, "RL_Y"),
                                    RL_Z = GetSafeNullableDouble(reader, "RL_Z"),
                                    RL_Angle = GetSafeNullableDouble(reader, "RL_Angle"),
                                    R_IsTest = GetSafeBool(reader, "R_IsTest")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFetchingModels", "DatabaseError", ex.Message);
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
                MsgBox.ErrorWithFormat("ErrorDeletingModel", "DatabaseError", ex.Message);
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
                                    FC_Distance = GetSafeNullableDouble(reader, "FC_Distance"),
                                    FC_Height = GetSafeNullableDouble(reader, "FC_Height"),
                                    FC_InterDistance = GetSafeNullableDouble(reader, "FC_InterDistance"),
                                    FC_Htu = GetSafeNullableDouble(reader, "FC_Htu"),
                                    FC_Htl = GetSafeNullableDouble(reader, "FC_Htl"),
                                    FC_Ts = GetSafeNullableDouble(reader, "FC_Ts"),
                                    FC_AlignmentAxeOffset = GetSafeNullableDouble(reader, "FC_AlignmentAxeOffset"),
                                    FC_Vv = GetSafeNullableDouble(reader, "FC_Vv"),
                                    FC_StCt = GetSafeNullableDouble(reader, "FC_StCt"),
                                    FC_IsTest = GetSafeBool(reader, "FC_IsTest"),

                                    FR_X = GetSafeNullableDouble(reader, "FR_X"),
                                    FR_Y = GetSafeNullableDouble(reader, "FR_Y"),
                                    FR_Z = GetSafeNullableDouble(reader, "FR_Z"),
                                    FR_Angle = GetSafeNullableDouble(reader, "FR_Angle"),
                                    FL_X = GetSafeNullableDouble(reader, "FL_X"),
                                    FL_Y = GetSafeNullableDouble(reader, "FL_Y"),
                                    FL_Z = GetSafeNullableDouble(reader, "FL_Z"),
                                    FL_Angle = GetSafeNullableDouble(reader, "FL_Angle"),
                                    F_IsTest = GetSafeBool(reader, "F_IsTest"),

                                    RR_X = GetSafeNullableDouble(reader, "RR_X"),
                                    RR_Y = GetSafeNullableDouble(reader, "RR_Y"),
                                    RR_Z = GetSafeNullableDouble(reader, "RR_Z"),
                                    RR_Angle = GetSafeNullableDouble(reader, "RR_Angle"),
                                    RL_X = GetSafeNullableDouble(reader, "RL_X"),
                                    RL_Y = GetSafeNullableDouble(reader, "RL_Y"),
                                    RL_Z = GetSafeNullableDouble(reader, "RL_Z"),
                                    RL_Angle = GetSafeNullableDouble(reader, "RL_Angle"),
                                    R_IsTest = GetSafeBool(reader, "R_IsTest")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFetchingModelDetails", "DatabaseError", ex.Message);
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
                        MsgBox.Warn("ModelNameAlreadyExists");
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

                        // Front Camera
                        cmd.Parameters.AddWithValue("FC_Distance", (object)model.FC_Distance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Height", (object)model.FC_Height ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_InterDistance", (object)model.FC_InterDistance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Htu", (object)model.FC_Htu ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Htl", (object)model.FC_Htl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Ts", (object)model.FC_Ts ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_AlignmentAxeOffset", (object)model.FC_AlignmentAxeOffset ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Vv", (object)model.FC_Vv ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_StCt", (object)model.FC_StCt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_IsTest", model.FC_IsTest);

                        // Front Radar
                        cmd.Parameters.AddWithValue("FR_X", (object)model.FR_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FR_Y", (object)model.FR_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FR_Z", (object)model.FR_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FR_Angle", (object)model.FR_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_X", (object)model.FL_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_Y", (object)model.FL_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_Z", (object)model.FL_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_Angle", (object)model.FL_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("F_IsTest", model.F_IsTest);

                        // Rear Radar
                        cmd.Parameters.AddWithValue("RR_X", (object)model.RR_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RR_Y", (object)model.RR_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RR_Z", (object)model.RR_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RR_Angle", (object)model.RR_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RL_X", (object)model.RL_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RRL_Y", (object)model.RL_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RL_Z", (object)model.RL_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RL_Angle", (object)model.RL_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_IsTest", model.R_IsTest);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorAddingModel", "DatabaseError", ex.Message);
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
                        MsgBox.Warn("ModelNameAlreadyExists");
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

                        // Front Camera
                        cmd.Parameters.AddWithValue("FC_Distance", (object)model.FC_Distance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Height", (object)model.FC_Height ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_InterDistance", (object)model.FC_InterDistance ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Htu", (object)model.FC_Htu ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Htl", (object)model.FC_Htl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Ts", (object)model.FC_Ts ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_AlignmentAxeOffset", (object)model.FC_AlignmentAxeOffset ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_Vv", (object)model.FC_Vv ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_StCt", (object)model.FC_StCt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FC_IsTest", model.FC_IsTest);
                        cmd.Parameters.AddWithValue("OldName", oldModelName);

                        // Front Radar
                        cmd.Parameters.AddWithValue("FR_X", (object)model.FR_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FR_Y", (object)model.FR_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FR_Z", (object)model.FR_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FR_Angle", (object)model.FR_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_X", (object)model.FL_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_Y", (object)model.FL_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_Z", (object)model.FL_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("FL_Angle", (object)model.FL_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("F_IsTest", model.F_IsTest);
                        cmd.Parameters.AddWithValue("OldName", oldModelName);

                        // Rear Radar
                        cmd.Parameters.AddWithValue("RR_X", (object)model.RR_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RR_Y", (object)model.RR_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RR_Z", (object)model.RR_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RR_Angle", (object)model.RR_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RL_X", (object)model.RL_X ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RL_Y", (object)model.RL_Y ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RL_Z", (object)model.RL_Z ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("RL_Angle", (object)model.RL_Angle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("R_IsTest", model.R_IsTest);
                        cmd.Parameters.AddWithValue("OldName", oldModelName);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingModel", "DatabaseError", ex.Message);
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
                MsgBox.ErrorWithFormat("ErrorCheckingDuplicateModelName", "DatabaseError", ex.Message);
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
                MsgBox.ErrorWithFormat("ErrorFetchingModelNameByBarcode", "DatabaseError", ex.Message);
            }

            return modelName;
        }
    }
}
