using Dapper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace Ki_ADAS.DB
{
    public class InfoRepository
    {
        private SettingConfigDb db;

        public InfoRepository(SettingConfigDb database)
        {
            db = database;
        }

        public string GetNextAcceptNo()
        {
            string todayStr = DateTime.Now.ToString("yyyyMMdd");
            string nextAcceptNo = $"{todayStr}0001";

            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string query = "SELECT MAX(AcceptNo) FROM Info WHERE AcceptNo LIKE ?";

                    var lastAcceptNo = con.QueryFirstOrDefault<string>(query, new { p1 = todayStr + "%" });

                    if (!string.IsNullOrEmpty(lastAcceptNo) && lastAcceptNo.Length == 12)
                    {
                        int lastSeq = int.Parse(lastAcceptNo.Substring(8));
                        int nextSeq = lastSeq + 1;
                        nextAcceptNo = $"{todayStr}{nextSeq:D4}";
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorGeneratingAcceptNo", "DatabaseError", ex.Message);
                return null;
            }

            return nextAcceptNo;
        }

        public bool SaveVehicleInfo(Info newVehicle)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "INSERT INTO Info (AcceptNo, PJI, Model) VALUES (?, ?, ?)";

                    int rowsAffected = con.Execute(query, new { newVehicle.AcceptNo, newVehicle.PJI, newVehicle.Model });
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorInsertingInfoTable", "DatabaseError", ex.Message);
                return false;
            }
        }

        public List<Info> GetRegisteredVehicles()
        {
            string todayStr = DateTime.Now.ToString("yyyyMMdd");
            const string query = "SELECT AcceptNo, PJI, Model FROM Info WHERE Mid(AcceptNo, 1, 8) = ? ORDER BY AcceptNo DESC";

            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    return con.Query<Info>(query, new { Today = todayStr }).ToList();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFetchingRegisteredVehicles", "DatabaseError", ex.Message);
                return new List<Info>();
            }
        }

        public bool PjiExists(string pji)
        {
            const string query = "SELECT COUNT(*) FROM Info WHERE PJI = ?";
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    int count = con.ExecuteScalar<int>(query, new { pji });
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCheckingPjiExists", "Error", ex.Message);
                return false;
            }
        }
    }
}