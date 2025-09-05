using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class SettingConfigDb
    {
        public string connectionString { get; set; }

        public void SetupDatabaseConnection()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString;

                AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

                if (string.IsNullOrEmpty(connectionString))
                {
                    string dbPath = Path.Combine(Application.StartupPath, "Ki-ADAS.mdb");

                    if (File.Exists(dbPath))
                    {
                        connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath};";
                    }
                    else
                    {
                        dbPath = Path.Combine(Application.StartupPath, "Ki-ADAS.accdb");

                        if (File.Exists(dbPath))
                        {
                            connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";
                        }
                        else
                        {
                            throw new FileNotFoundException("데이터베이스 파일(Ki-ADAS.mdb 또는 Ki-ADAS.accdb)을 찾을 수 없습니다.");
                        }
                    }
                }

                Console.WriteLine($"데이터베이스 연결 문자열: {connectionString}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to set up database connection: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
