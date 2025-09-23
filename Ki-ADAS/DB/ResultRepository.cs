using Dapper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

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
                    string todayDate = DateTime.Now.ToString("yyyyMMdd");
                    const string query = "SELECT * FROM Result WHERE LEFT(AcceptNo, 8) = ? ORDER BY AcceptNo";
                    return con.Query<Result>(query, new { todayDate }).ToList();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorRetrievingResultInfo", "DatabaseError", ex.Message);
                return new List<Result>();
            }
        }

        public List<Result> GetResultInfoByDate(string date)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string query = "SELECT * FROM Result WHERE LEFT(AcceptNo, 8) = ? ORDER BY AcceptNo";
                    return con.Query<Result>(query, new { date }).ToList();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorRetrievingResultInfoByDate", "DatabaseError", ex.Message);
                return new List<Result>();
            }
        }

        public List<Result> GetResultInfoByPji(string pji)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string query = "SELECT * FROM Result WHERE PJI = ? ORDER BY AcceptNo";
                    return con.Query<Result>(query, new { pji }).ToList();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorRetrievingResultInfoByPJI", "DatabaseError", ex.Message);
                return new List<Result>();
            }
        }

        public bool SaveResult(Result result)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string checkQuery = "SELECT COUNT(*) FROM Result WHERE AcceptNo = ?";
                    bool exists = con.ExecuteScalar<int>(checkQuery, new { result.AcceptNo }) > 0;

                    if (exists)
                    {
                        // Update
                        const string updateQuery = @"UPDATE Result SET
                            PJI = @PJI, Model = @Model, StartTime = @StartTime, EndTime = @EndTime, 
                            FC_IsOk = @FC_IsOk, FR_IsOk = @FR_IsOk, RR_IsOk = @RR_IsOk
                            WHERE AcceptNo = @AcceptNo";
                        return con.Execute(updateQuery, result) > 0;
                    }
                    else
                    {
                        // Add
                        const string insertQuery = @"INSERT INTO Result (
                            AcceptNo, PJI, Model, StartTime, EndTime, FC_IsOk, FR_IsOk, RR_IsOk)
                            VALUES (@AcceptNo, @PJI, @Model, @StartTime, @EndTime, @FC_IsOk, @FR_IsOk, @RR_IsOk)";
                        return con.Execute(insertQuery, result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorSavingModel", "DatabaseError", ex.Message);
                return false;
            }
        }

        public bool IsDuplicateAcceptNo(string acceptNo, string oldAcceptNo = null)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    if (string.IsNullOrEmpty(oldAcceptNo))
                    {
                        const string query = "SELECT COUNT(*) FROM Result WHERE AcceptNo = ?";
                        int count = con.ExecuteScalar<int>(query, new { AcceptNo = acceptNo });
                        return count > 0;
                    }
                    else
                    {
                        const string query = "SELECT COUNT(*) FROM Result WHERE AcceptNo = ? AND AcceptNo <> ?";
                        int count = con.ExecuteScalar<int>(query, new { NewAcceptNo = acceptNo, OldAcceptNo = oldAcceptNo });
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