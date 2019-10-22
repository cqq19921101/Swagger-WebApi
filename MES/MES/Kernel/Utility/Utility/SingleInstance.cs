using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 單實例
    /// </summary>
    public class SingleInstance
    {
        /// <summary>
        /// 如果有相同進程在運行，則關閉程序
        /// </summary>
        public static bool CloseIfHaveOtherInstanceRuning()
        {
            bool needClose = false;
            try
            {
                int currProcId;

                currProcId = Process.GetCurrentProcess().Id;
                Process[] allProc = System.Diagnostics.Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
                if (allProc.Length > 1)
                {
                    needClose = true;
                    MessageBox.Show("檢測到模塊已經打開，下面會先關閉模塊，請重新開啟", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    foreach (System.Diagnostics.Process proc in allProc)
                    {
                        //MessageBox.Show("檢測到ID:" + proc.Id.ToString());
                        if (proc.Id == currProcId)
                        {
                            continue;
                        }
                        else
                        {
                            proc.Kill();
                        }
                    }
                }
                return needClose;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
