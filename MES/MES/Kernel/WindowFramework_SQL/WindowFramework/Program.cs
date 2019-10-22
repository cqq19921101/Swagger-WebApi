using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace WindowFramework
{
    /// <summary>
    /// 窗體框架，給編譯后的EXE文件重命名成需要的模塊名稱，比如DataCenter，即可加載這個PROGRAM對應的菜單
    /// Programer:Kimi
    /// Date:2017-06-08
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string sArgs = "";

            if (args.Length == 0)
            {
                //sArgs = "沒有參數傳入";
                MessageBox.Show("請用Sajet Mamanger打開");
                Application.Exit();
                return;
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    sArgs += args[i].ToString();
                }
                Para.UID = sArgs;
            }

            try
            {
                if (Liteon.Mes.Utility.SingleInstance.CloseIfHaveOtherInstanceRuning())
                {
                    Application.Exit();
                    return;
                }
                Para.PROGRAM = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                Para.VERSION = Liteon.Mes.Utility.GetLocalInfo.GetFileVersion().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Liteon.Mes.Db.Oracle.CreateConnectionStringByXMLFile("DBConnection.xml");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
