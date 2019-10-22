using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Printing;

namespace Liteon.Mes.Utility
{
    public sealed class PrintLabelBk
    {
        ////#region 使用默認打印機打印相關的準備方法

        ////class Win32API
        ////{
        ////    [DllImport("winspool.drv")]
        ////    public static extern bool SetDefaultPrinter(string Name);

        ////    [DllImport("winspool.drv", CharSet = CharSet.Auto)]
        ////    public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

        ////    [DllImport("winspool.drv", CharSet = CharSet.Auto)]
        ////    public static extern bool AddJob(IntPtr ptrPrinter, Int32 iLevel, IntPtr ptrJob, Int32 iSize, out Int32 iCpSize);

        ////    [DllImport("winspool.drv", CharSet = CharSet.Auto)]
        ////    public static extern bool ScheduleJob(IntPtr ptrPrinter, Int32 JobID);

        ////    [DllImport("winspool.drv", CharSet = CharSet.Auto)]
        ////    public static extern bool ClosePrinter(IntPtr ptrPrinter);
        ////}

        ////[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        ////private struct ADDJOB_INFO_1
        ////{
        ////    [MarshalAs(UnmanagedType.LPTStr)]
        ////    public string lpPath;
        ////    public Int32 JobID;
        ////}

        ////public void WinApiPrintByte(string p_PrintName, byte[] p_Byte)
        ////{
        ////    if (p_PrintName != null && p_PrintName.Length > 0)
        ////    {
        ////        IntPtr _PrintHandle;
        ////        IntPtr _JobHandle = Marshal.AllocHGlobal(2000);

        ////        if (Win32API.OpenPrinter(p_PrintName, out _PrintHandle, IntPtr.Zero))
        ////        {
        ////            ADDJOB_INFO_1 _JobInfo = new ADDJOB_INFO_1();
        ////            int _Size;
        ////            Win32API.AddJob(_PrintHandle, 1, _JobHandle, 2000, out _Size);
        ////            _JobInfo = (ADDJOB_INFO_1)Marshal.PtrToStructure(_JobHandle, typeof(ADDJOB_INFO_1));
        ////            //System.IO.File.WriteAllBytes(p_PrintName, p_Byte);
        ////            System.IO.File.WriteAllBytes(_JobInfo.lpPath, p_Byte);
        ////            Win32API.ScheduleJob(_PrintHandle, _JobInfo.JobID);
        ////            Win32API.ClosePrinter(_PrintHandle);
        ////            Marshal.FreeHGlobal(_JobHandle);
        ////        }
        ////    }
        ////}

        ////public static void WinApiPrintByte(byte[] p_Byte)
        ////{
        ////    string sDefPrinter = GetDefaultPrinterName();
        ////    if (sDefPrinter != null && sDefPrinter.Length > 0)
        ////    {
        ////        IntPtr _PrintHandle;
        ////        IntPtr _JobHandle = Marshal.AllocHGlobal(2000);

        ////        if (Win32API.OpenPrinter(sDefPrinter, out _PrintHandle, IntPtr.Zero))
        ////        {
        ////            ADDJOB_INFO_1 _JobInfo = new ADDJOB_INFO_1();
        ////            int _Size;
        ////            Win32API.AddJob(_PrintHandle, 1, _JobHandle, 2000, out _Size);

        ////            _JobInfo = (ADDJOB_INFO_1)Marshal.PtrToStructure(_JobHandle, typeof(ADDJOB_INFO_1));
        ////            System.IO.File.WriteAllBytes(_JobInfo.lpPath, p_Byte);
        ////            //System.IO.File.WriteAllBytes(sDefPrinter, p_Byte);
        ////            Win32API.ScheduleJob(_PrintHandle, _JobInfo.JobID);
        ////            Win32API.ClosePrinter(_PrintHandle);
        ////            Marshal.FreeHGlobal(_JobHandle);
        ////        }
        ////    }
        ////}

        /////// <summary>
        /////// 獲得本地安裝的默認打印機
        /////// </summary>
        /////// <returns>返回打印機名稱</returns>
        ////public static string GetDefaultPrinterName()
        ////{
        ////    PrintDocument pd = new PrintDocument();
        ////    string strDdefaultPrinter = pd.PrinterSettings.PrinterName;
        ////    return strDdefaultPrinter == null ? "" : strDdefaultPrinter;
        ////}

        /////// <summary>
        /////// 獲得本地安裝的所有打印機
        /////// </summary>
        /////// <returns>返回字符串數組</returns>
        ////public static string[] GetInstalledPrinterList()
        ////{
        ////    PrintDocument pd = new PrintDocument();
        ////    string[] Printers = new string[PrinterSettings.InstalledPrinters.Count];
        ////    int i = 0;
        ////    foreach (string sOnePrinter in PrinterSettings.InstalledPrinters)
        ////    {
        ////        Printers[i] = sOnePrinter;
        ////        i++;
        ////    }
        ////    return Printers;
        ////}

        /////// <summary>
        /////// 將選定的打印機設置為默認打印機
        /////// </summary>
        /////// <param name="printerName">打印機名稱</param>
        /////// <returns></returns>
        ////public static bool SetDefaultPrinter(string printerName)
        ////{
        ////    if (string.IsNullOrEmpty(printerName))
        ////    {
        ////        return true;
        ////    }
        ////    try
        ////    {
        ////        if (Win32API.SetDefaultPrinter(printerName))
        ////        {
        ////            return true;
        ////        }
        ////        else
        ////        {
        ////            return false;
        ////        }
        ////    }
        ////    catch { return false; }
        ////}

        ////#endregion

        ////#region 替換字符串方法
        /////// <summary>
        /////// 將TXT模板進行替換
        /////// </summary>
        /////// <param name="allContent">模板內容</param>
        /////// <param name="dt">DataTable</param>
        /////// <returns>返回替換好的字符串</returns>
        ////private static string ReplaceContentValue(string allContent, DataTable dt)
        ////{
        ////    for (int i = 1; i < dt.Columns.Count; i++)
        ////    {
        ////        if (allContent.IndexOf(dt.Columns[i].Caption.ToString()) > -1)
        ////        {
        ////            allContent = allContent.Replace(dt.Columns[i].Caption.ToString(), dt.Rows[0][i].ToString());
        ////        }
        ////    }
        ////    return allContent;
        ////}

        /////// <summary>
        /////// 移除掉NA行
        /////// </summary>
        /////// <param name="content">替換后的字符串</param>
        /////// <returns>返回移除掉NA行后的字符串</returns>
        ////private static string RemoveNARow(string content)
        ////{
        ////    string allContent = "";
        ////    string[] line = System.Text.RegularExpressions.Regex.Split(content, "\r\n");  //利用正则表达式来分解
        ////    int j = 1;
        ////    foreach (string ss in line)
        ////    {
        ////        if (ss.IndexOf("<NA>") > -1)
        ////        {
        ////            continue;
        ////        }
        ////        if (j == 1)
        ////        {
        ////            allContent = ss;
        ////            j = j + 1;
        ////        }
        ////        else
        ////        {
        ////            allContent = allContent + "\r\n" + ss;
        ////            j = j + 1;
        ////        }
        ////    }
        ////    return allContent;
        ////}

        ////#endregion

        ////#region 使用默認打印機打印（USB直連線，需安裝打印機驅動）

        /////// <summary>
        /////// 使用默認打印機來打印DataTable的數據
        /////// </summary>
        /////// <param name="dt">第一列：文件名（例如123.txt），不含路徑，文件需放在程序目錄下，第二列開始時變量，DataTable列頭以&lt;變量名&gt;為格式，替換txt文檔中相同的地方，若變量值是NA，則變量所在行不打印</param>
        ////public static void PrintByDef(DataTable dt)
        ////{
        ////    StreamReader sr;
        ////    string AllContent;
        ////    string strFilePath = Application.StartupPath + "\\" + dt.Rows[0][0].ToString();
        ////    if (!File.Exists(strFilePath))
        ////    {
        ////        throw new Exception("Can not find the templet:" + @strFilePath);
        ////    }

        ////    sr = new StreamReader(strFilePath);
        ////    AllContent = sr.ReadToEnd();
        ////    sr.Close();
        ////    try
        ////    {
        ////        //用dt來替換內容
        ////        AllContent = ReplaceContentValue(AllContent, dt);
        ////        //去掉<NA>行
        ////        AllContent = RemoveNARow(AllContent);
             
        ////        byte[] b = Encoding.UTF8.GetBytes(AllContent);

        ////        WinApiPrintByte(b);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}


        /////// <summary>
        /////// 使用默認打印機打印Zebra的TXT內容
        /////// </summary>
        /////// <param name="content">要打印的TXT中的所有內容</param>
        ////public static void PrintByDef(string content)
        ////{
        ////    string AllContent;
        ////    AllContent = content;
        ////    try
        ////    {
        ////        AllContent = RemoveNARow(AllContent);

        ////        byte[] b = Encoding.UTF8.GetBytes(AllContent);//Encoding.GetEncoding("big5").GetBytes(AllContent);

        ////        WinApiPrintByte(b);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}

        /////// <summary>
        /////// 測試默認打印機是否可以打印（打印一張測試標籤出來）
        /////// </summary>
        ////public static void TestDefPrinter()
        ////{
        ////    string sTxt = "^XA"
        ////                + "^EG"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^MCY"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^FWN^CFD,24^PW631^LH0,0"
        ////                + "^CI0^PR3^MNY^MTT^MMC^MD12^PON^PMN^LRN"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^MCY"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^DFR:TEMP_FMT.ZPL"
        ////                + "^LRN"
        ////                + "^CWZ,E:ARIAL.FNT^AZN,19,19^FO20,37^FDCOM Printer Test^FS"
        ////                + "^CWZ,E:ARIAL.FNT^AZN,19,19^FO20,64^FDTest OK^FS"
        ////                + "^CWZ,E:ARIAL.FNT^AZN,19,19^FO20,92^FDLiteOn CZ^FS"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^XFR:TEMP_FMT.ZPL"
        ////                + "^MMC,N^PQ1,1,1,Y"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^IDR:TEMP_FMT.ZPL"
        ////                + "^XZ";

        ////    try
        ////    {
        ////        PrintByDef(sTxt);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}

        ////#endregion

        ////#region 用COM口打印

        /////// <summary>
        /////// 打開指定端口，如果採用的是打開端口-->打印-->關閉端口這種模式，會在方法中自動調用本方法
        /////// </summary>
        /////// <param name="serialPort">打印要使用的SerialPort</param>
        /////// <param name="com">計算機的COM口，格式是COM數字，比如COM1</param>
        /////// <param name="rate">打印機波特率</param>
        ////public static void OpenSerialPort(SerialPort serialPort, string com, int rate)
        ////{
        ////    try
        ////    {
        ////        if (serialPort.IsOpen == false)
        ////        {
        ////            serialPort.PortName = com;
        ////            serialPort.BaudRate = rate;
        ////            serialPort.DataBits = 8;
        ////            serialPort.Parity = Parity.None;
        ////            serialPort.StopBits = StopBits.One;
        ////            serialPort.Open();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}

        /////// <summary>
        /////// 關閉打印機端口
        /////// </summary>
        /////// <param name="serialPort">打印調用的SerialPort</param>
        ////public static void CloseSerialPort(SerialPort serialPort)
        ////{
        ////    try
        ////    {
        ////        if (serialPort.IsOpen)
        ////        {
        ////            serialPort.Close();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}

        /////// <summary>
        /////// 測試打印端口是否可以打印（打印一張測試標籤出來）
        /////// </summary>
        /////// <param name="serialPort">打印調用的SerialPort</param>
        /////// <param name="com">要測試的端口</param>
        /////// <param name="rate">打印機波特率</param>
        ////public static void TestSerialPort(SerialPort serialPort, string com, int rate)
        ////{
        ////    string sTxt = "^XA"
        ////                + "^EG"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^MCY"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^FWN^CFD,24^PW631^LH0,0"
        ////                + "^CI0^PR3^MNY^MTT^MMC^MD12^PON^PMN^LRN"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^MCY"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^DFR:TEMP_FMT.ZPL"
        ////                + "^LRN"
        ////                + "^CWZ,E:ARIAL.FNT^AZN,19,19^FO20,37^FDCOM Printer Test^FS"
        ////                + "^CWZ,E:ARIAL.FNT^AZN,19,19^FO20,64^FDTest OK^FS"
        ////                + "^CWZ,E:ARIAL.FNT^AZN,19,19^FO20,92^FDLiteOn CZ^FS"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^XFR:TEMP_FMT.ZPL"
        ////                + "^MMC,N^PQ1,1,1,Y"
        ////                + "^XZ"
        ////                + "^XA"
        ////                + "^IDR:TEMP_FMT.ZPL"
        ////                + "^XZ";

        ////    try
        ////    {
        ////        OpenSerialPort(serialPort, com, rate);
        ////        serialPort.WriteLine(sTxt);
        ////        CloseSerialPort(serialPort);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}


        /////// <summary>
        /////// 用SerialPort來打印DataTable中的數據，端口將一直開啟，直到調用CloseSerialPort才關閉
        /////// </summary>
        /////// <param name="dt">第一列：文件名（例如123.txt），不含路徑，文件需放在程序目錄下，第二列開始時變量，DataTable列頭以&lt;變量名&gt;為格式，替換txt文檔中相同的地方，若變量值是NA，則變量所在行不打印</param>
        /////// <param name="serialPort">打印調用的SerialPort</param>
        ////public static void PrintByPort(DataTable dt, SerialPort serialPort)
        ////{
        ////    if (serialPort.IsOpen == false)
        ////    {
        ////        throw new Exception("SerialPort is close, please open it at first!");
        ////    }

        ////    StreamReader sr;
        ////    string AllContent;
        ////    string strFilePath = Application.StartupPath + "\\" + dt.Rows[0][0].ToString();
        ////    if (!File.Exists(strFilePath))
        ////    {
        ////        throw new Exception("Can not find the templet:" + @strFilePath);
        ////    }

        ////    sr = new StreamReader(strFilePath);
        ////    AllContent = sr.ReadToEnd();
        ////    sr.Close();
        ////    try
        ////    {
        ////        AllContent = ReplaceContentValue(AllContent, dt);
        ////        AllContent = RemoveNARow(AllContent);

        ////        serialPort.WriteLine(AllContent);
        ////        serialPort.DiscardOutBuffer();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }

        ////}

        /////// <summary>
        /////// 用SerialPort來打印Zebra的TXT膜拜，端口將一直開啟，直到調用CloseSerialPort才關閉
        /////// </summary>
        /////// <param name="Context">要打印的TXT中的所有內容</param>
        /////// <param name="serialPort">打印調用的SerialPort</param>
        ////public static void PrintByPort(string Context, SerialPort serialPort)
        ////{
        ////    if (serialPort.IsOpen == false)
        ////    {
        ////        throw new Exception("SerialPort is close, please open it at first!");
        ////    }

        ////    string AllContent;
        ////    AllContent = Context;
        ////    try
        ////    {
        ////        AllContent = RemoveNARow(AllContent);
               
        ////        serialPort.WriteLine(AllContent);

        ////        serialPort.DiscardOutBuffer();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////}

        /////// <summary>
        /////// 用SerialPort來打印DataTable中的數據，端口將在打印前開啟，打印后關閉
        /////// /// </summary>
        /////// <param name="dt">第一列：文件名（例如123.txt），不含路徑，文件需放在程序目錄下，第二列開始時變量，DataTable列頭以&lt;變量名&gt;為格式，替換txt文檔中相同的地方，若變量值是NA，則變量所在行不打印</param>
        /////// <param name="serialPort">打印調用的SerialPort</param>
        /////// <param name="com">要調用的端口</param>
        /////// <param name="rate">打印機波特率</param>
        ////public static void PrintByPort(DataTable dt, SerialPort serialPort, string com, int rate)
        ////{
        ////    if (serialPort.IsOpen == false)
        ////    {
        ////        OpenSerialPort(serialPort, com, rate);
        ////    }

        ////    StreamReader sr;
        ////    string AllContent;
        ////    string strFilePath = Application.StartupPath + "\\" + dt.Rows[0][0].ToString();
        ////    if (!File.Exists(strFilePath))
        ////    {
        ////        throw new Exception("Can not find the templet:" + @strFilePath);
        ////    }

        ////    sr = new StreamReader(strFilePath);
        ////    AllContent = sr.ReadToEnd();
        ////    sr.Close();
        ////    try
        ////    {
        ////        AllContent = ReplaceContentValue(AllContent, dt);
        ////        AllContent = RemoveNARow(AllContent);
               
        ////        serialPort.WriteLine(AllContent);

        ////        serialPort.DiscardOutBuffer();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////    finally
        ////    {
        ////        CloseSerialPort(serialPort);
        ////    }
        ////}

        /////// <summary>
        /////// 用SerialPort來打印Zebra的TXT膜拜，端口将在打印前开启，端口將在打印前開啟，打印后關閉
        /////// </summary>
        /////// <param name="content">要打印的TXT模板中的所有內容</param>
        /////// <param name="serialPort">打印調用的SerialPort</param>
        /////// <param name="com">要調用的端口</param>
        /////// <param name="rate">打印機波特率</param>
        ////public static void PrintByPort(string content, SerialPort serialPort, string com, int rate)
        ////{
        ////    if (serialPort.IsOpen == false)
        ////    {
        ////        OpenSerialPort(serialPort, com, rate);
        ////    }

        ////    string AllContent;
        ////    AllContent = content;
        ////    try
        ////    {
        ////        AllContent = RemoveNARow(AllContent);

        ////        serialPort.WriteLine(AllContent);

        ////        serialPort.DiscardOutBuffer();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw new Exception(ex.Message);
        ////    }
        ////    finally
        ////    {
        ////        CloseSerialPort(serialPort);
        ////    }
        ////}

        ////#endregion

    }
}
