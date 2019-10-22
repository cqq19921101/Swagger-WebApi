using System;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Management;
using System.IO;
using System.Diagnostics;
namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 獲取本地計算機的一些信息
    /// </summary>
    public class GetLocalInfo
    {
        static ManagementObjectSearcher mos_os = new ManagementObjectSearcher("select * from win32_operatingsystem");
        static ManagementClass mc_cpu = new ManagementClass("win32_processor");
        static ManagementObjectSearcher mos_mb = new ManagementObjectSearcher("select * from win32_baseboard");
        static ManagementObjectSearcher mos_video = new ManagementObjectSearcher("select * from win32_videocontroller");

        /// <summary>
        /// 獲取文件(當前自學的exe)的版本信息
        /// </summary>
        /// <param name="filePath">文件路徑</param>
        /// <returns></returns>
        public static Version GetFileVersion()
        {
            try
            {
                string exeName = Process.GetCurrentProcess().ProcessName;
                return AssemblyName.GetAssemblyName(Application.StartupPath + "\\" + exeName + ".exe").Version;
                //Assembly assembly = Assembly.LoadFile(Application.StartupPath + "\\" + exeName + ".exe");
                //AssemblyName assemblyName = assembly.GetName();
                //return assemblyName.Version;
            }
            catch
            {
                Version version = new Version("0.0.0.0");
                return version;
            }
        }

        /// <summary>
        /// 指定文件（exe或dll）的版本信息
        /// </summary>
        /// <param name="filePath">文件路徑</param>
        /// <returns></returns>
        public static Version GetFileVersion(string fullFileName)
        {
            try
            {
                string exeName = Process.GetCurrentProcess().ProcessName;
                return AssemblyName.GetAssemblyName(fullFileName).Version;
                //Assembly assembly = Assembly.LoadFile(Application.StartupPath + "\\" + exeName + ".exe");
                //AssemblyName assemblyName = assembly.GetName();
                //return assemblyName.Version;
            }
            catch
            {
                Version version = new Version("0.0.0.0");
                return version;
            }
        }

        #region User and computer

        /// <summary>
        /// 獲取當前計算機登陸用戶名
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetCurrentUserName()
        {
            try
            {
                return Environment.UserName;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取當前登陸用戶權名
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetCurrentFullUserName()
        {
            try
            {
                ManagementClass mcUser = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection mocUser = mcUser.GetInstances();
                int k = 0;
                foreach (ManagementObject mo in mocUser)
                {
                    k = k + 1;
                }
                string[] resultUser = new string[k];
                k = 0;
                foreach (ManagementObject mo in mocUser)
                {
                    return mo["UserName"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取當前計算機的Domain
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetCurrentDomainName()
        {
            try
            {
                return Environment.UserDomainName;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 判斷當前用戶是否是管理員
        /// </summary>
        /// <returns>返回Boolean值</returns>
        public static bool IsCurrentUserAdmin()
        {
            try
            {
                AppDomain domain = System.Threading.Thread.GetDomain();
                domain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal);
                System.Security.Principal.WindowsPrincipal principal = (System.Security.Principal.WindowsPrincipal)System.Threading.Thread.CurrentPrincipal;
                bool user = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
                if (user)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 獲取System路徑
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetSystemPath()
        {
            try
            {
                return Environment.SystemDirectory;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 獲取當前計算機名稱
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetComputerName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region IP and Mac
        /// <summary>
        /// 獲取計算機IP地址
        /// </summary>
        /// <returns>返回字符串，如果有多個IP，以;隔開</returns>
        public static string GetLocalIP()
        {
            try
            {
                IPHostEntry IPHE = new IPHostEntry();
                IPHE = Dns.GetHostEntry(Dns.GetHostName());
                string ips = "";
                for (int i = 0; i < IPHE.AddressList.Length; i++)
                {
                    if (IPHE.AddressList[i].ToString().Length < 7)
                    {
                        continue;
                    }
                    if (IPHE.AddressList[i].ToString().IndexOf(":") > 0)
                    {
                        continue;
                    }
                    ips = ips + IPHE.AddressList[i].ToString() + ";";
                    return ips.Substring(0, ips.Length - 1);
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 返回計算機IP地址
        /// </summary>
        /// <returns>返回一個數組</returns>
        public static string[] GetLocalIPArray()
        {
            try
            {
                IPHostEntry IPHE = new IPHostEntry();
                IPHE = Dns.GetHostEntry(Dns.GetHostName());
                string[] resultIP = new string[IPHE.AddressList.Length];
                for (int i = 0; i < IPHE.AddressList.Length; i++)
                {
                    resultIP[i] = IPHE.AddressList[i].ToString();
                }
                return resultIP;
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        /// 獲取Mac地址，只返回第一個
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetLoaclMac()
        {
            try
            {
                ManagementClass mc;
                ManagementObjectCollection moc;
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                moc = mc.GetInstances();
                int j = 0;
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        j = j + 1;
                    }
                }
                string[] resultMac = new string[j];
                j = 0;
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        return mo["MacAddress"].ToString();//.Replace(":", "");
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取Mac地址
        /// </summary>
        /// <returns>返回一個數組</returns>
        public static string[] GetLoaclMacArray()
        {
            try
            {
                ManagementClass mc;
                ManagementObjectCollection moc;
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                moc = mc.GetInstances();
                int j = 0;
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        j = j + 1;
                    }
                }
                string[] resultMac = new string[j];
                j = 0;
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        resultMac[j] = mo["MacAddress"].ToString();//.Replace(":", "");
                        j = j + 1;
                    }
                }
                return resultMac;
            }
            catch
            {
                return new string[0];
            }
        }
        #endregion

        #region OS
        /// <summary>
        /// 獲取計算機OS版本
        /// </summary>
        /// <returns>返回一個數組</returns>
        public static string GetOSVsersion()
        {
            try
            {
                ManagementObjectCollection moc_os = mos_os.Get();
                foreach (ManagementObject mo in moc_os)
                {
                    return mo["Version"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取計算機OS序列號
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetOSSerialNuber()
        {
            try
            {
                ManagementObjectCollection moc_os = mos_os.Get();
                foreach (ManagementObject mo in moc_os)
                {
                    return mo["Serialnumber"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取OS名稱
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetOSName()
        {
            try
            {
                ManagementObjectCollection moc_os = mos_os.Get();
                foreach (ManagementObject mo in moc_os)
                {
                    return mo["Caption"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取OS的ServicePack版本
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetOSServicePack()
        {
            try
            {
                ManagementObjectCollection moc_os = mos_os.Get();
                foreach (ManagementObject mo in moc_os)
                {
                    return mo["CSDVersion"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取OS廠商
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetOSManufacturer()
        {
            try
            {
                ManagementObjectCollection moc_os = mos_os.Get();
                foreach (ManagementObject mo in moc_os)
                {
                    return mo["Manufacturer"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取OS安裝路徑
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetOSPath()
        {
            try
            {
                ManagementObjectCollection moc_os = mos_os.Get();
                foreach (ManagementObject mo in moc_os)
                {
                    return mo["WindowsDirectory"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region CPU
        /// <summary>
        /// 獲取CPU ID
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetCPUID()
        {
            try
            {
                ManagementObjectCollection moc_cpu = mc_cpu.GetInstances();
                foreach (ManagementObject mo in moc_cpu)
                {
                    return mo["processorid"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取CPU名稱
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetCPUName()
        {
            try
            {
                ManagementObjectCollection moc_cpu = mc_cpu.GetInstances();
                foreach (ManagementObject mo in moc_cpu)
                {
                    return mo["Name"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取CPU製造商
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetCPUManufacturer()
        {
            try
            {
                ManagementObjectCollection moc_cpu = mc_cpu.GetInstances();
                foreach (ManagementObject mo in moc_cpu)
                {
                    return mo["Manufacturer"].ToString(); ;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取CPU版本
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetCPUVersion()
        {
            try
            {
                ManagementObjectCollection moc_cpu = mc_cpu.GetInstances();
                foreach (ManagementObject mo in moc_cpu)
                {
                    return mo["Version"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region MB
        /// <summary>
        /// 獲取MB廠商
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetMBManufacturer()
        {
            try
            {
                ManagementObjectCollection moc_mb = mos_mb.Get();
                foreach (ManagementObject mo in moc_mb)
                {
                    return mo["Manufacturer"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取MB序列號
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetMBSerialNumber()
        {
            try
            {
                ManagementObjectCollection moc_mb = mos_mb.Get();
                foreach (ManagementObject mo in moc_mb)
                {
                    return mo["SerialNumber"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取MB型號
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetMBModel()
        {
            try
            {
                ManagementObjectCollection moc_mb = mos_mb.Get();
                foreach (ManagementObject mo in moc_mb)
                {
                    return mo["Product"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Driver
        /// <summary>
        /// 獲取計算機驅動器信息
        /// </summary>
        /// <returns>返回DataTable，包含DriverName,FileSystem,Size,FreeSzie</returns>
        public static DataTable GetDriverInfo()
        {
            try
            {
                DriveInfo[] drive = DriveInfo.GetDrives();
                DataTable dtDriver = new DataTable();
                dtDriver.Columns.Add("DriverName", typeof(string));
                dtDriver.Columns.Add("FileSystem", typeof(string));
                dtDriver.Columns.Add("Size", typeof(string));
                dtDriver.Columns.Add("FreeSize", typeof(string));

                for (int i = 0; i < drive.Length; i++)
                {
                    DataRow dr = dtDriver.NewRow();
                    dr["DriverName"] = drive[i].Name;
                    if (drive[i].DriveType == DriveType.Fixed)
                    {
                        dr["Size"] = Convert.ToString(drive[i].TotalSize / 1024 / 1024 / 1024) + "G";
                        dr["FreeSize"] = Convert.ToString(drive[i].TotalFreeSpace / 1024 / 1024 / 1024) + "G";
                        dr["FileSystem"] = drive[i].DriveFormat;
                    }
                    else
                    {
                        dr["Size"] = "0G";
                        dr["FreeSize"] = "0G";
                        dr["FileSystem"] = "NA";
                    }
                    dtDriver.Rows.Add(dr);
                }

                return dtDriver;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Video

        /// <summary>
        /// 獲取顯存容量
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayRAM()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    return Convert.ToString(Convert.ToInt32(mo["AdapterRAM"].ToString()) / 1024 / 1024) + "MB";
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取顯示器分辨率
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayPixel()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                string Resolution = "";
                foreach (ManagementObject mo in moc_video)
                {
                    if (mo["CurrentHorizontalResolution"] != null && mo["CurrentVerticalResolution"] != null)
                    {
                        Resolution = mo["CurrentHorizontalResolution"].ToString() + "℅" + mo["CurrentVerticalResolution"].ToString();
                        return Resolution;
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取顯示器刷新率
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayRefreshRate()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    if (mo["CurrentRefreshRate"] != null)
                    {
                        return mo["CurrentRefreshRate"].ToString() + "Hz";
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 獲取顯示卡描述
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayDescription()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    return mo["Description"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 獲取顯卡廠商
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayManufacutrer()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    return mo["AdapterCompatibility"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 獲取顯示位寬
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayBitPixel()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    if (mo["CurrentBitsPerPixel"] != null)
                    {
                        return mo["CurrentBitsPerPixel"].ToString() + "bit";
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取顯示器刷新率
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayMaxRefreshRate()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    if (mo["MaxRefreshRate"] != null)
                    {
                        return mo["MaxRefreshRate"].ToString() + "Hz";
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取顯示芯片名
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayChip()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    return mo["VideoProcessor"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取顯卡驅動版本
        /// </summary>
        /// <returns>返回字符串</returns>
        public static string GetDisplayDriverVersion()
        {
            try
            {
                ManagementObjectCollection moc_video = mos_video.Get();
                foreach (ManagementObject mo in moc_video)
                {
                    return mo["DriverVersion"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }
}
