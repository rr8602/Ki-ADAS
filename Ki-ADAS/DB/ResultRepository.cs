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
            List<Result> results = new List<Result>();
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    string todayDate = DateTime.Now.ToString("yyyyMMdd");
                    const string query = "SELECT * FROM Result WHERE LEFT(AcceptNo, 8) = ? ORDER BY AcceptNo";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("todayDate", todayDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Result
                                {
                                    AcceptNo = reader["AcceptNo"].ToString(),
                                    PJI = reader["PJI"].ToString(),
                                    Model = reader["Model"].ToString(),
                                    StartTime = Convert.ToDateTime(reader["StartTime"]), 
                                    EndTime = Convert.ToDateTime(reader["EndTime"]), 
                                    FC_IsOk = Convert.ToBoolean(reader["FC_IsOk"]), 
                                    FR_IsOk = Convert.ToBoolean(reader["FR_IsOk"]), 
                                    RR_IsOk = Convert.ToBoolean(reader["RR_IsOk"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorRetrievingResultInfo", "DatabaseError", ex.Message);
                return null;
            }

            return results;
        }

        public List<Result> GetResultInfoByDate(string date) 
        {
            List<Result> results = new List<Result>();
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "SELECT * FROM Result WHERE LEFT(AcceptNo, 8) = ? ORDER BY AcceptNo";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("date", date);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Result
                                {
                                    AcceptNo = reader["AcceptNo"].ToString(),
                                    PJI = reader["PJI"].ToString(),
                                    Model = reader["Model"].ToString(),
                                    StartTime = Convert.ToDateTime(reader["StartTime"]), 
                                    EndTime = Convert.ToDateTime(reader["EndTime"]), 
                                    FC_IsOk = Convert.ToBoolean(reader["FC_IsOk"]), 
                                    FR_IsOk = Convert.ToBoolean(reader["FR_IsOk"]), 
                                    RR_IsOk = Convert.ToBoolean(reader["RR_IsOk"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorRetrievingResultInfoByDate", "DatabaseError", ex.Message);
                return null;
            }

            return results;
        }

        public List<Result> GetResultInfoByPji(string pji)
        {
            List<Result> results = new List<Result>();
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "SELECT * FROM Result WHERE PJI = ? ORDER BY AcceptNo";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("pji", pji);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Result
                                {
                                    AcceptNo = reader["AcceptNo"].ToString(),
                                    PJI = reader["PJI"].ToString(),
                                    Model = reader["Model"].ToString(),
                                    StartTime = Convert.ToDateTime(reader["StartTime"]), 
                                    EndTime = Convert.ToDateTime(reader["EndTime"]), 
                                    FC_IsOk = Convert.ToBoolean(reader["FC_IsOk"]), 
                                    FR_IsOk = Convert.ToBoolean(reader["FR_IsOk"]), 
                                    RR_IsOk = Convert.ToBoolean(reader["RR_IsOk"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorRetrievingResultInfoByPJI", "DatabaseError", ex.Message);
                return null;
            }

            return results;
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
                            cmd.Parameters.AddWithValue("StartTime", result.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("EndTime", result.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("FC_IsOk", result.FC_IsOk ? 1 : 0);
                            cmd.Parameters.AddWithValue("FR_IsOk", result.FR_IsOk ? 1 : 0);
                            cmd.Parameters.AddWithValue("RR_IsOk", result.RR_IsOk ? 1 : 0);
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
                            cmd.Parameters.AddWithValue("StartTime", result.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("EndTime", result.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("FC_IsOk", result.FC_IsOk ? 1 : 0);
                            cmd.Parameters.AddWithValue("FR_IsOk", result.FR_IsOk ? 1 : 0);
                            cmd.Parameters.AddWithValue("RR_IsOk", result.RR_IsOk ? 1 : 0);

                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorSavingModel", "DatabaseError", ex.Message);
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
                MsgBox.ErrorWithFormat("ErrorCheckingDuplicateModelName", "DatabaseError", ex.Message);
                return true;
            }
        }
    }
}
