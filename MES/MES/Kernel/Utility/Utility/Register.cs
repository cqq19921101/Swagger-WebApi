using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 讀取和寫入註冊表
    /// </summary>
    public class Register
    {
        /// <summary>
        /// 寫註冊表值(HKEY_CURRENT_USER)
        /// </summary>
        /// <param name="key">註冊表鍵名字</param>
        /// <param name="value">值</param>
        public static void WriteRegKey(string key, string value)
        {
            try
            {
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\LITEON_MES");
                regKey.SetValue(key, value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 從註冊表讀取值(HKEY_CURRENT_USER)
        /// </summary>
        /// <param name="key">Key name</param>
        /// <returns></returns>
        public static string ReadRegKey(string key)
        {
            RegistryKey regKey;
            try
            {
                regKey = Registry.CurrentUser.OpenSubKey("Software\\LITEON_MES");
                return regKey.GetValue(key, "").ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
