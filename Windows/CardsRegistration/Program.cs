using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardsRegistration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSelect());
        }

        public static string user = "";
        public static string token = "";
        public static string building_id = "";
        public static string level_id = "";
        public static bool validToken = false;

#if DEBUG
        public static string host = "http://localhost/";
#else
        public static string host = "http://sm.kbs.com.vn/";
#endif
    }
}
