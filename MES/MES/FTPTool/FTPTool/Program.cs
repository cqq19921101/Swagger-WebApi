using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

namespace FTPTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Para.dsSiteProfile = new DataSet();
            //Para.dsSiteProfile.ReadXml(Application.StartupPath + "\\siteprofile.xml");
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in ps)
            {
                System.Diagnostics.Debug.WriteLine(p.ProcessName);
                if (p.Id == current.Id)
                {
                    continue;
                }
                if (p.ProcessName == "FTPTool")
                {
                    
                    MessageBox.Show("FTPTool is running now!", "FTPTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            if (System.IO.File.Exists(Application.StartupPath + "\\SysMailLoop.xml"))
            {
                Para.dsSysMailLoop = new DataSet();
                Para.dsSysMailLoop.ReadXml(Application.StartupPath + "\\SysMailLoop.xml");
            }


            Para.NORMALLOGPATH = Application.StartupPath + "\\Logs\\Normal";
            Para.ERRORLOGFILEPATH = Application.StartupPath + "\\Logs\\Error";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
