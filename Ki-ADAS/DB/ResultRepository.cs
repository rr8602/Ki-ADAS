using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS.DB
{
    public class ResultRepository
    {
        private SettingConfigDb db;

        public ResultRepository(SettingConfigDb database)
        {
            db = database;
        }

        public List<Result> GetResultInfo()
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "SELECT * FROM Result";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error retrieving Result info: {ex.Message}", "Database Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }

            return new List<Result>();
        }

        public bool SaveResult(Result result)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    // AcceptNo가 DB에 존재하는지 확인
                    bool exists;
                    const string checkQuery = "SELECT COUNT(*) FROM Result WHERE AcceptNo = ?";

                    using (var checkCmd = new OleDbCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("AcceptNo", result.AcceptNo);
                        exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                    }

                    if (exists)
                    {
                        // Update
                        const string updateQuery = @"UPDATE Result SET
                            PJI = ?, Model = ?, StartTime = ?, EndTime = ?, FC_IsOk = ?, FR_IsOk = ?, RR_IsOk = ?
                            WHERE AcceptNo = ?";

                        using (var cmd = new OleDbCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("PJI", result.PJI);
                            cmd.Parameters.AddWithValue("Model", result.Model);
                            cmd.Parameters.AddWithValue("StartTime", result.StartTime);
                            cmd.Parameters.AddWithValue("EndTime", result.EndTime);
                            cmd.Parameters.AddWithValue("FC_IsOk", result.FC_IsOk);
                            cmd.Parameters.AddWithValue("FR_IsOk", result.FR_IsOk);
                            cmd.Parameters.AddWithValue("RR_IsOk", result.RR_IsOk);
                            cmd.Parameters.AddWithValue("AcceptNo", result.AcceptNo);

                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                    else
                    {
                        // Add
                        const string insertQuery = @"INSERT INTO Result (
                            AcceptNo, PJI, Model, StartTime, EndTime, FC_IsOk, FR_IsOk, RR_IsOk)
                            VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

                        using (var cmd = new OleDbCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("AcceptNo", result.AcceptNo);
                            cmd.Parameters.AddWithValue("PJI", result.PJI);
                            cmd.Parameters.AddWithValue("Model", result.Model);
                            cmd.Parameters.AddWithValue("StartTime", result.StartTime);
                            cmd.Parameters.AddWithValue("EndTime", result.EndTime);
                            cmd.Parameters.AddWithValue("FC_IsOk", result.FC_IsOk);
                            cmd.Parameters.AddWithValue("FR_IsOk", result.FR_IsOk);
                            cmd.Parameters.AddWithValue("RR_IsOk", result.RR_IsOk);

                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving model: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool IsDuplicateAceeptNo(string name, string oldAcceptNo = null)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    string checkQuery;

                    if (string.IsNullOrEmpty(oldAcceptNo))
                    {
                        checkQuery = "SELECT COUNT(*) FROM Result WHERE AcceptNo = ?";
                    }
                    else
                    {
                        checkQuery = "SELECT COUNT(*) FROM Result WHERE AcceptNo = ? AND AcceptNo <> ?";
                    }

                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("Name", name);

                        if (!string.IsNullOrEmpty(oldAcceptNo))
                        {
                            checkCmd.Parameters.AddWithValue("OldName", oldAcceptNo);
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
    }
}
