using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FTPClass
{
    public abstract class FTPClass
    {
        public abstract bool CheckFtpConnectionStatus(out string errmsg);

        public abstract void Connect();

        public abstract void Connect(string url);

        //private abstract void ConnectGetFileSize(string url);

        //private abstract void ConnectGetLastModifyDate(string url);

        public abstract string[] GetFileList();

        public abstract DataTable GetFileList(string folder);


        /// <summary>
        /// Upload
        /// </summary>
        /// <param name="o">传入窗体</param>
        /// <param name="iRow">起始为0</param>
        /// <param name="FtpPathWithoutIP">FTP路径，但不包含[ftp://IP]</param>
        /// <param name="Filename">本地文件全文件名</param>
        /// <param name="initialpath">本地文件起始路径</param>
        public abstract void Upload(object o, int iRow, string FtpPathWithoutIP, string Filename, string initialpath);

        public abstract void Upload(object o, int iRow, string FtpPathWithoutIP, string Filename, string initialpath, int offset);

        public abstract bool CheckFileExist(string FtpFileFullPath, string FileNameWithoutPath);

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="o">传入窗体</param>
        /// <param name="iRow">起始为0</param>
        /// <param name="LocalFilePath">要保存的本地路径，最后一码不能是\</param>
        /// <param name="fileName">FTP上的文件名，包含路径，但不包含[ftp://IP]</param>
        public abstract void Download(object o, int iRow, string LocalFilePath, string fileName);


        /// <summary>
        /// Download
        /// </summary>
        /// <param name="o">传入窗体</param>
        /// <param name="iRow">起始为0</param>
        /// <param name="LocalFilePath">要保存的本地路径，最后一码不能是\</param>
        /// <param name="fileName">文件名，不包含路径</param>
        public abstract void Download(object o, int iRow, string LocalFilePath, 
            string FileNameWithoutPath, string FtpFileFullPath, int offset);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="fileName">FTP上的文件名，包含路径，但不包含[ftp://IP]</param>
        public abstract void Delete(string fileName);

        /// <summary>
        /// Delete folder
        /// </summary>
        /// <param name="folder">FTP上的文件夹名，包含路径，但不包含[ftp://IP]</param>
        public abstract void DeleteFolder(string folder);


        public abstract void MakeFolder(string folder);

        /// <summary>
        /// Make folder
        /// </summary>
        /// <param name="folder">本地文件夹路径</param>
        /// <param name="FtpPathWithoutIP">FTP路径，但不包含[ftp://IP]</param>
        /// <param name="initialpath">本地文件夹起始路径</param>
        public abstract void MakeFolder(string folder, string FtpPathWithoutIP, string initialpath);

        public abstract void Rename(string CurrentFileWithFtpPath, string newFilenameWithoutPath);

        //abstract static class getFormMethod { }
    }
}
