using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    string query = "SELECT MAX(AcceptNo) FROM Info WHERE AcceptNo LIKE ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("AcceptNo", todayStr + "%");
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string lastAcceptNo = result.ToString();

                            if (lastAcceptNo.Length == 12)
                            {
                                int lastSeq = int.Parse(lastAcceptNo.Substring(8));
                                int nextSeq = lastSeq + 1;
                                nextAcceptNo = $"{todayStr}{nextSeq:D4}";
                            }
                        }
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

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("AcceptNo", newVehicle.AcceptNo);
                        cmd.Parameters.AddWithValue("PJI", newVehicle.PJI);
                        cmd.Parameters.AddWithValue("Model", newVehicle.Model);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
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
            var vehicles = new List<Info>();
            string todayStr = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    const string query = "SELECT AcceptNo, PJI, Model FROM Info WHERE Mid(AcceptNo, 1, 8) = ? ORDER BY AcceptNo DESC";

                    using (var cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Today", todayStr);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vehicles.Add(new Info
                                {
                                    AcceptNo = reader["AcceptNo"].ToString(),
                                    PJI = reader["PJI"].ToString(),
                                    Model = reader["Model"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFetchingRegisteredVehicles", "DatabaseError", ex.Message);
            }

            return vehicles;
        }
    }
}
