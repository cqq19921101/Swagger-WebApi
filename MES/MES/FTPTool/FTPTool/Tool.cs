using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Data;
using System.Collections;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace FTPTool
{
    class Tool
    {
        public static DataTable GetFileListDateTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("File Name");
            dt.Columns.Add("Size");
            dt.Columns.Add("Type");
            dt.Columns.Add("Modify Date");
            dt.Columns.Add("FileFullPath");
            dt.Columns.Add("IsFolder");
            dt.Columns.Add("Icon");

            return dt;
        }

        public static ArrayList GetFTPFileListInfoWithoutSpace(string[] OriginalArray)
        {
            ArrayList al = new ArrayList();
            foreach (string s in OriginalArray)
            {
                if (s != "")
                {
                    al.Add(s);
                }
            }
            return al;
        }

        public static void SendMail(string MailTo, string Subject, string Body, string Attr)
        {
            MailAddressCollection MailList;
            MailList = CheckMailAddress(MailTo);
            if (MailList.Count <= 0) { return; };

            try
            {
                string MailFrom = Para.MAIL_FROM;
                string MailSMTP = Para.MAIL_SMTP;
                //string MailCC = "";
                string MailAttach = Attr;
                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress(MailFrom);
                foreach (MailAddress m in MailList)
                {
                    mailObj.To.Add(m);
                }
                mailObj.Subject = Subject;
                mailObj.Body = Body;
                mailObj.IsBodyHtml = true;
                //mailObj.CC.Add(MailCC);


                if (!String.IsNullOrEmpty(MailAttach))
                {
                    Attachment attchfile = new Attachment(MailAttach);
                    mailObj.Attachments.Add(attchfile);
                }
                SmtpClient SMTPServer = new SmtpClient(MailSMTP);
                if (String.IsNullOrEmpty(Para.MAIL_ACCOUNT))
                {
                    SMTPServer.UseDefaultCredentials = true;

                }
                else
                {
                    SMTPServer.Credentials = new System.Net.NetworkCredential(Para.MAIL_ACCOUNT, Para.MAIL_PWD);
                }
                SMTPServer.Send(mailObj);
            }
            catch (Exception ex)
            {
                throw ex;
                //Tools.SaveErrorLog(ex.Message);
            }
            //MessageBox.Show("Mail already sent.");
        }

        private static MailAddressCollection CheckMailAddress(string strMail)
        {
            MailAddressCollection MailCollection = new MailAddressCollection();
            string errMail = "";
            try
            {
                foreach (string s in strMail.Split(';'))
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        if (IsMail(s))
                        {
                            MailCollection.Add(new MailAddress(s));
                        }
                        else
                        {
                            errMail += s + ";";
                        }
                    }
                }
                return MailCollection;
            }
            catch (Exception e)
            {
                errMail += e.Message;
                return MailCollection;
            }
            finally
            {
                //if (errMail != "")
                //{
                //    Param.ErrorLog(errMail, "CheckMailAddress");
                //}
            }

        }



        private static bool IsMail(string strMail)
        {
            Regex re = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.None);
            MatchCollection mc = re.Matches(strMail);
            if (mc.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 获取文件类型。文件夹和文件图标
        private class Shell32
        {

            public const int MAX_PATH = 256;
            [StructLayout(LayoutKind.Sequential)]
            public struct SHITEMID
            {
                public ushort cb;
                [MarshalAs(UnmanagedType.LPArray)]
                public byte[] abID;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ITEMIDLIST
            {
                public SHITEMID mkid;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct BROWSEINFO
            {
                public IntPtr hwndOwner;
                public IntPtr pidlRoot;
                public IntPtr pszDisplayName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string lpszTitle;
                public uint ulFlags;
                public IntPtr lpfn;
                public int lParam;
                public IntPtr iImage;
            }

            // Browsing for directory.
            public const uint BIF_RETURNONLYFSDIRS = 0x0001;
            public const uint BIF_DONTGOBELOWDOMAIN = 0x0002;
            public const uint BIF_STATUSTEXT = 0x0004;
            public const uint BIF_RETURNFSANCESTORS = 0x0008;
            public const uint BIF_EDITBOX = 0x0010;
            public const uint BIF_VALIDATE = 0x0020;
            public const uint BIF_NEWDIALOGSTYLE = 0x0040;
            public const uint BIF_USENEWUI = (BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
            public const uint BIF_BROWSEINCLUDEURLS = 0x0080;
            public const uint BIF_BROWSEFORCOMPUTER = 0x1000;
            public const uint BIF_BROWSEFORPRINTER = 0x2000;
            public const uint BIF_BROWSEINCLUDEFILES = 0x4000;
            public const uint BIF_SHAREABLE = 0x8000;

            [StructLayout(LayoutKind.Sequential)]
            public struct SHFILEINFO
            {
                public const int NAMESIZE = 80;
                public IntPtr hIcon;
                public int iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
                public string szTypeName;
            };

            public const uint SHGFI_ICON = 0x000000100;     // get icon
            public const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
            public const uint SHGFI_TYPENAME = 0x000000400;     // get type name
            public const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
            public const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
            public const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
            public const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
            public const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
            public const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
            public const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
            public const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
            public const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
            public const uint SHGFI_OPENICON = 0x000000002;     // get open icon
            public const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
            public const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
            public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
            public const uint SHGFI_ADDOVERLAYS = 0x000000020;     // apply the appropriate overlays
            public const uint SHGFI_OVERLAYINDEX = 0x000000040;     // Get the index of the overlay

            public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
            public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

            [DllImport("Shell32.dll")]
            public static extern IntPtr SHGetFileInfo(
                string pszPath,
                uint dwFileAttributes,
                ref SHFILEINFO psfi,
                uint cbFileInfo,
                uint uFlags
                );
        }

        private class User32
        {
            /**/
            /// <summary>
            /// Provides access to function required to delete handle. This method is used internally
            /// and is not required to be called separately.
            /// </summary>
            /// <param name="hIcon">Pointer to icon handle.</param>
            /// <returns>N/A</returns>
            [DllImport("User32.dll")]
            public static extern int DestroyIcon(IntPtr hIcon);
        }

        public static Icon GetFileIcon(string ExtName, bool bSmall)
        {
            try
            {
                Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
                uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

                if (bSmall)
                {
                    flags += Shell32.SHGFI_SMALLICON;
                }
                else
                {
                    flags += Shell32.SHGFI_LARGEICON;
                }

                Shell32.SHGetFileInfo(ExtName, Shell32.FILE_ATTRIBUTE_NORMAL, ref shfi, (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi), flags);

                System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
                User32.DestroyIcon(shfi.hIcon);        // Cleanup
                return icon;
            }
            catch
            {
                return null;
            }
        }

        public static Icon GetDirectoryIcon(bool bSmall)
        {
            try
            {
                uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;

                if (bSmall)
                {
                    flags += Shell32.SHGFI_SMALLICON;
                }
                else
                {
                    flags += Shell32.SHGFI_LARGEICON;
                }

                // Get the folder icon
                Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
                Shell32.SHGetFileInfo(null, Shell32.FILE_ATTRIBUTE_DIRECTORY, ref shfi, (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi), flags);

                System.Drawing.Icon.FromHandle(shfi.hIcon);    // Load the icon from an HICON handle
                // Now clone the icon, so that it can be successfully stored in an ImageList
                System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();

                User32.DestroyIcon(shfi.hIcon);        // Cleanup
                return icon;
            }
            catch
            {
                return null;
            }
        }

        public static string GetFileType(string ExtName)
        {
            try
            {
                string desc = (string)Registry.ClassesRoot.OpenSubKey(ExtName).GetValue(null);
                return (string)Registry.ClassesRoot.OpenSubKey(desc).GetValue(null);

            }
            catch { return ExtName; }
        }
        #endregion


 
        public static void SaveLog(string FileName, string Msg)
        {
            string path = "";
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                if (Directory.Exists(Para.NORMALLOGPATH) == false)
                {
                    Directory.CreateDirectory(Para.NORMALLOGPATH);
                }
                path = Para.NORMALLOGPATH + "\\" + FileName;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.Write(DateTime.Now.ToString("HH:mm:ss\t") + Msg + "\r\n");
                
            }
            catch { }
            finally
            {
                path = null;
                sw.Flush();
                sw.Close();
                fs.Close();
                sw = null;
                fs = null;
            }
        }

        public static void SaveErrorLog(string FileName, string ErrMsg)
        {
            string path = "";
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                if (Directory.Exists(Para.ERRORLOGFILEPATH) == false)
                {
                    Directory.CreateDirectory(Para.ERRORLOGFILEPATH);
                }
                path = Para.ERRORLOGFILEPATH + "\\" + FileName;
                Para.ERRORLOGFILE = path;
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.Write(DateTime.Now.ToString("HH:mm:ss\t") + ErrMsg + "\r\n");
            }
            catch { }
            finally
            {
                path = null;
                sw.Flush();
                sw.Close();
                fs.Close();
                sw = null;
                fs = null;
            }
        }

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

        public static string GetApplicationPath()
        {
            return Application.StartupPath;
        }

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
    }
}
