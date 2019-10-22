using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for IOHelper
/// </summary>
internal class IOHelper
{
    public IOHelper()
    {

    }

    public static bool FileExists(string fileName)
    {
        return File.Exists(fileName);
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="dirName"></param>
    /// <returns></returns>
    public static bool CreateDir(string dirName)
    {
        if (!Directory.Exists(dirName))
        {
            Directory.CreateDirectory(dirName);
        }
        return true;
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool CreateFile(string fileName)
    {
        if (!FileExists(fileName))
        {
            FileStream fs = File.Create(fileName);
            fs.Close();
            fs.Dispose();
        }
        return true;

    }

    /// <summary>
    /// 读文件内容
    /// </summary>
    /// <param name="fileName">文件路徑</param>
    /// <returns></returns>
    public static string Read(string fileName)
    {
        if (!FileExists(fileName))
        {
            throw new NullReferenceException("文件fileName=" + fileName + "不存在!");
        }
        //将文件信息读入流中
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            return new StreamReader(fs, GetFileEncodeType(fileName)).ReadToEnd();
        }
    }

    /// <summary>
    /// 讀取文件所有行
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string[] ReadLines(string fileName)
    {
        if (!FileExists(fileName))
        {
            throw new NullReferenceException("文件fileName=" + fileName + "不存在!");
        }

        return File.ReadAllLines(fileName, GetFileEncodeType(fileName));
    }

    /// <summary>
    /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
    /// </summary>
    /// <param name="FILE_NAME">文件路径</param>
    /// <returns>文件的编码类型</returns>
    public static Encoding GetFileEncodeType(string fileName)
    {
        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        Encoding r = GetType(fs);
        fs.Close();
        return r;
    }

    /// <summary>
    /// 通过给定的文件流，判断文件的编码类型
    /// </summary>
    /// <param name="fs">文件流</param>
    /// <returns>文件的编码类型</returns>
    public static System.Text.Encoding GetType(FileStream fs)
    {
        byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
        byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
        byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
        Encoding reVal = Encoding.Default;

        BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
        int i;
        int.TryParse(fs.Length.ToString(), out i);
        byte[] ss = r.ReadBytes(i);
        if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
        {
            reVal = Encoding.UTF8;
        }
        else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
        {
            reVal = Encoding.BigEndianUnicode;
        }
        else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
        {
            reVal = Encoding.Unicode;
        }
        r.Close();
        return reVal;

    }

    /// <summary>
    /// 判断是否是不带 BOM 的 UTF8 格式
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static bool IsUTF8Bytes(byte[] data)
    {
        int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数
        byte curByte; //当前分析的字节.
        for (int i = 0; i < data.Length; i++)
        {
            curByte = data[i];
            if (charByteCounter == 1)
            {
                if (curByte >= 0x80)
                {
                    //判断当前
                    while (((curByte <<= 1) & 0x80) != 0)
                    {
                        charByteCounter++;
                    }
                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                    if (charByteCounter == 1 || charByteCounter > 6)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //若是UTF-8 此时第一位必须为1
                if ((curByte & 0xC0) != 0x80)
                {
                    return false;
                }
                charByteCounter--;
            }
        }
        if (charByteCounter > 1)
        {
            throw new Exception("非预期的byte格式");
        }
        return true;
    }


    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="content">文件内容</param>
    /// <returns></returns>
    public static bool Write(string fileName, string content)
    {
        if (!FileExists(fileName) || content == null)
        {
            return false;
        }

        //将文件信息读入流中
        using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
        {
            lock (fs)//锁住流
            {
                if (!fs.CanWrite)
                {
                    throw new System.Security.SecurityException("文件fileName=" + fileName + "是只讀文件，不能寫入!");
                }

                byte[] buffer = Encoding.UTF8.GetBytes(content);
                fs.Write(buffer, 0, buffer.Length);
                return true;
            }
        }
    }

    /// <summary>
    /// 写入一行
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="content">内容</param>
    /// <returns></returns>
    public static bool WriteLine(string fileName, string content)
    {
        using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate | FileMode.Append))
        {
            lock (fs)
            {
                if (!fs.CanWrite)
                {
                    throw new System.Security.SecurityException("文件fileName=" + fileName + "是只讀文件，不能寫入!");
                }

                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine(content);
                sw.Dispose();
                sw.Close();
                return true;
            }
        }
    }

    public static bool CopyDir(DirectoryInfo fromDir, string toDir)
    {
        return CopyDir(fromDir, toDir, fromDir.FullName);
    }

    /// <summary>
    /// 复制目录
    /// </summary>
    /// <param name="fromDir">被复制的目录</param>
    /// <param name="toDir">复制到的目录</param>
    /// <returns></returns>
    public static bool CopyDir(string fromDir, string toDir)
    {
        if (fromDir == null || toDir == null)
        {
            throw new NullReferenceException("參數為空!");
        }

        if (fromDir == toDir)
        {
            throw new Exception("兩個目錄都是" + fromDir);
        }

        if (!Directory.Exists(fromDir))
        {
            throw new IOException("目錄fromDir=" + fromDir + "不存在!");
        }

        DirectoryInfo dir = new DirectoryInfo(fromDir);
        return CopyDir(dir, toDir, dir.FullName);
    }

    /// <summary>
    /// 复制目录
    /// </summary>
    /// <param name="fromDir">被复制的目录</param>
    /// <param name="toDir">复制到的目录</param>
    /// <param name="rootDir">被复制的根目录</param>
    /// <returns></returns>
    private static bool CopyDir(DirectoryInfo fromDir, string toDir, string rootDir)
    {
        string filePath = string.Empty;
        foreach (FileInfo f in fromDir.GetFiles())
        {
            filePath = toDir + f.FullName.Substring(rootDir.Length);
            string newDir = filePath.Substring(0, filePath.LastIndexOf("\\"));
            CreateDir(newDir);
            File.Copy(f.FullName, filePath, true);
        }

        foreach (DirectoryInfo dir in fromDir.GetDirectories())
        {
            CopyDir(dir, toDir, rootDir);
        }

        return true;
    }

    /// <summary>
    /// 复制文件
    /// </summary>
    /// <param name="fromDir">被复制的文件</param>
    /// <param name="toDir">复制到的文件</param>
    /// <returns></returns>
    public static bool CopyFile(string fromFile, string toFile)
    {
        if (fromFile == null || toFile == null)
        {
            throw new NullReferenceException("參數為空!");
        }

        if (fromFile == toFile)
        {
            throw new Exception("兩個文件都是" + fromFile);
        }

        if (!FileExists(fromFile))
        {
            throw new IOException("文件fromFile=" + fromFile + "不存在!");
        }

        File.Copy(fromFile, toFile, true);

        return true;
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="fileName">文件的完整路径</param>
    /// <returns></returns>
    public static bool DeleteFile(string fileName)
    {
        if (FileExists(fileName))
        {
            File.Delete(fileName);
            return true;
        }
        return false;
    }


    public static void DeleteDir(DirectoryInfo dir)
    {
        if (dir == null)
        {
            throw new NullReferenceException("目錄不存在!");
        }

        foreach (DirectoryInfo d in dir.GetDirectories())
        {
            DeleteDir(d);
        }

        foreach (FileInfo f in dir.GetFiles())
        {
            DeleteFile(f.FullName);
        }

        dir.Delete();

    }


    /// <summary>
    /// 删除目录
    /// </summary>
    /// <param name="dir">制定目录</param>
    /// <param name="onlyDir">是否只删除目录</param>
    /// <returns></returns>
    public static bool DeleteDir(string dir, bool onlyDir)
    {
        if (dir == null || dir.Trim() == "")
        {
            throw new NullReferenceException("目錄dir=" + dir + "不存在!");
        }

        if (!Directory.Exists(dir))
        {
            return false;
        }

        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        if (dirInfo.GetFiles().Length == 0 && dirInfo.GetDirectories().Length == 0)
        {
            Directory.Delete(dir);
            return true;
        }


        if (!onlyDir)
        {
            return false;
        }
        else
        {
            DeleteDir(dirInfo);
            return true;
        }

    }

    /// <summary>
    /// 移動文件
    /// </summary>
    /// <param name="sourceFileName">文件原始路徑</param>
    /// <param name="destFileName">文件目的路徑</param>
    /// <returns></returns>
    public static bool MoveFile(string sourceFileName, string destFileName)
    {
        if (FileExists(sourceFileName))
        {
            string destFileDirectory = destFileName.Substring(0, destFileName.LastIndexOf(@"\"));
            CreateDir(destFileDirectory);
            DeleteFile(destFileName);
            File.Move(sourceFileName, destFileName);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 在指定的目录中查找文件
    /// </summary>
    /// <param name="dir">目录</param>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public static FileInfo[] FindFiles(string dir, string fileName)
    {
        if (dir == null || dir.Trim() == "" || fileName == null || fileName.Trim() == "" || !Directory.Exists(dir))
        {
            return null;
        }

        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        return FindFiles(dirInfo, fileName);

    }


    public static FileInfo[] FindFiles(DirectoryInfo dir, string fileName)
    {
        return dir.GetFiles(fileName);
    }
}