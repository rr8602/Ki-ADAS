using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ki_ADAS.DB;

namespace Ki_ADAS
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LanguageManager.Initialize();

            SettingConfigDb db = new SettingConfigDb();
            db.SetupDatabaseConnection();

            Frm_Mainfrm mainForm = new Frm_Mainfrm(db);

            Application.Run(mainForm);
        }
    }
}
