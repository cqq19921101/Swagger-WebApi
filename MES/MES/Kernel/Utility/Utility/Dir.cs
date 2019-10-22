using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 文件夾處理
    /// </summary>
    public class Dir
    {
        /// <summary>
        /// 創建文件夾
        /// </summary>
        /// <param name="path"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static bool CreateFolders(string path, out string returnMsg)
        {
            int iPosistion = 0;
            iPosistion = path.LastIndexOf("\\");
            if (iPosistion >= 0)
            {
                path = path.Substring(0, iPosistion);
            }
            try
            {
                Directory.CreateDirectory(path);
                returnMsg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                returnMsg = "Error:" + ex.Message.ToString();
                return false;
            }
        }
    }
}
