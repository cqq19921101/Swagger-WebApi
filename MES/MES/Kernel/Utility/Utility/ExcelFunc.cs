using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// Excel讀取和導出的類
    /// </summary>
    public class ExcelFunc
    {

        /// <summary>
        /// 查詢Excel中有多少個sheet，并返回名稱，使用完后請手動調用GC.Collect()
        /// </summary>
        /// <param name="fileName">Excel文件，完整路徑</param>
        /// <returns>返回字符串數組，包含所有的sheet名稱</returns>
        public static string[] GetSheetNameFromExcel(string fileName)
        {
            try
            {
                Missing Miss = Missing.Value;
                DataTable dtSheet = new DataTable();
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlBook = xlApp.Workbooks.Open(fileName, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss);
                dtSheet.Columns.Add("SheetName");
                for (int i = 1; i <= xlBook.Sheets.Count; i++)
                {
                    dtSheet.Rows.Add(((Excel.Worksheet)xlBook.Worksheets[i]).Name.ToString());
                }

                xlApp.Quit();
                xlBook = null;
                xlApp = null;
                string[] sheetList = new string[dtSheet.Rows.Count];
                for (int i = 0; i < sheetList.Length; i++)
                {
                    sheetList[i] = dtSheet.Rows[i]["SheetName"].ToString();
                }
                dtSheet = null;
                return sheetList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 寫Excel模塊
        /// </summary>
        /// <param name="xlSheet"></param>
        /// <param name="dt"></param>
        private static void WriteExcelModule(Excel.Worksheet xlSheet, DataTable dt)
        {
            int i = 1;
            string[,] x;
            int iRow = dt.Rows.Count;
            int iCol = dt.Columns.Count;
            Excel.Range ra;
            foreach (DataColumn dc in dt.Columns)
            {
                xlSheet.Cells[1, i] = dt.Columns[i - 1].Caption.ToString();

                x = new string[iRow, 1];
                int j = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    x[j, 0] = dr[i - 1].ToString();
                    j = j + 1;
                }
                ra = xlSheet.get_Range(xlSheet.Cells[2, i], xlSheet.Cells[iRow + 1, i]);
                ra.Value2 = x;
                ra.EntireColumn.AutoFit();
                i = i + 1;
            }
            ra = xlSheet.get_Range(xlSheet.Cells[1, 1], xlSheet.Cells[dt.Rows.Count + 1, dt.Columns.Count]);
            ra.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            ra.Font.Name = "Arial";
            ra.Font.Size = 10;
            ra = xlSheet.get_Range(xlSheet.Cells[1, 1], xlSheet.Cells[1, dt.Columns.Count]);
            ra.Interior.ColorIndex = 45;
            ra.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ra = null;
        }

        /// <summary>
        /// DataTable導出到Excel
        /// </summary>
        /// <param name="dt">要導出的DataTable</param>
        public static void ExportToExcel(DataTable dt)
        {
            if (dt == null)
            {
                return;
            }
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlBook = xlApp.Application.Workbooks.Add(true);
            Excel.Worksheet xlSheet = (Excel.Worksheet)xlBook.ActiveSheet;
            xlSheet.Name = "Result";
            xlApp.Visible = true;
            xlApp.UserControl = false;
            xlApp.DisplayAlerts = false;

            WriteExcelModule(xlSheet, dt);

            xlSheet = null;
            xlBook = null;
            xlApp = null;
            GC.Collect();
        }

        /// <summary>
        /// DataTable導出到Excel，可以指定SheetName、文件名和是否要保存
        /// </summary>
        /// <param name="dt">要導出的DataTable</param>
        /// <param name="sheetName">指定的SheetName，輸入空白或者null則使用Result作為SheetName</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        /// <param name="save">是否要保存，默認存放路徑是桌面上</param>
        public static void ExportToExcel(DataTable dt, string sheetName, string fileName, bool save)
        {
            ExportToExcel(dt, sheetName, fileName, save, "", false);
        }

        /// <summary>
        /// DataTable導出到Excel，可以指定SheetName、文件名和是否要保存，也可以指定保存后是否自動退出
        /// </summary>
        /// <param name="dt">要導出的DataTable</param>
        /// <param name="sheetName">指定的SheetName，輸入空白或者null則使用Result作為SheetName</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        /// <param name="save">是否要保存，默認存放路徑是桌面上</param>
        /// <param name="savePath">文件存放的路徑，最後不要帶\\</param>
        /// <param name="quitAfterSave">保存以後是否自動退出</param>
        public static void ExportToExcel(DataTable dt, string sheetName, string fileName, bool save, string savePath, bool quitAfterSave)
        {
            if (dt == null)
            {
                return;
            }
            Missing Miss = Missing.Value;

            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Result";
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "Result.xlsx";
            }
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlBook = xlApp.Application.Workbooks.Add(true);
            Excel.Worksheet xlSheet = (Excel.Worksheet)xlBook.ActiveSheet;
            xlSheet.Name = sheetName;
            xlApp.Visible = true;
            xlApp.UserControl = false;
            xlApp.DisplayAlerts = false;
            xlApp.Caption = fileName;

            WriteExcelModule(xlSheet, dt);

            if (save)
            {
                if (savePath != "")
                {
                    if (System.IO.Directory.Exists(savePath) == false)
                    {
                        System.IO.Directory.CreateDirectory(savePath);
                    }
                }
                else
                {
                    savePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                }
                xlBook.SaveAs(savePath + "\\" + fileName, Miss, null, null, null, null, Excel.XlSaveAsAccessMode.xlNoChange,
                    null, null, null, null, null);

                if (quitAfterSave)
                {
                    xlApp.Quit();
                }
            }

            xlSheet = null;
            xlBook = null;
            xlApp = null;
            GC.Collect();
        }

        /// <summary>
        /// DataSet導出到Excel，可以指定SheetName、文件名和是否要保存
        /// </summary>
        /// <param name="ds">要導出的DataSet</param>
        /// <param name="sheetNames">字符串數組，用來存放SheetName，數量需要和DataSet中的Table數一致，輸入null則自動給sheet編號</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        /// <param name="save">是否要保存，默認存放路徑是桌面上</param>
        public static void ExportToExcel(DataSet ds, string[] sheetNames, string fileName, bool save)
        {
            ExportToExcel(ds, sheetNames, fileName, save, "", false);
        }

        /// <summary>
        /// DataSet導出到Excel，可以指定SheetName、文件名和是否要保存，也可以指定保存后是否自動退出
        /// </summary>
        /// <param name="ds">要導出的DataSet</param>
        /// <param name="sheetNames">字符串數組，用來存放SheetName，數量需要和DataSet中的Table數一致，輸入null則自動給sheet編號</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        /// <param name="save">是否要保存，默認存放在桌面上</param>
        /// <param name="savePath">文件存放的路徑，最後不要帶\\</param>
        /// <param name="quitAfterSave">導出完成后是否要自動退出</param>
        public static void ExportToExcel(DataSet ds, string[] sheetNames, string fileName, bool save, string savePath, bool quitAfterSave)
        {
            if (ds == null)
            {
                return;
            }
            if (sheetNames == null)
            {
                sheetNames = new string[256];
            }
            System.Reflection.Missing Miss = System.Reflection.Missing.Value;
            string SheetName = "";
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "Result.xlsx";
            }

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlBook = xlApp.Application.Workbooks.Add(true);
            Excel.Worksheet xlSheet = (Excel.Worksheet)xlBook.ActiveSheet;
            Excel.Range ra;
            xlApp.Visible = true;
            xlApp.DisplayAlerts = true;
            xlApp.Caption = fileName;
            SheetName = (sheetNames[0] != null) ? sheetNames[0].ToString() : "Sheet1";
            xlSheet.Name = SheetName;

            //寫第一個DataTable
            WriteExcelModule(xlSheet, ds.Tables[0]);

            //如果DataTable數量大於1，循環寫
            if (ds.Tables.Count > 1)
            {
                for (int z = 1; z < ds.Tables.Count; z++)
                {
                    xlBook.Worksheets.Add(Miss, xlBook.ActiveSheet, Miss, Miss);
                    xlSheet = (Excel.Worksheet)xlBook.Worksheets[z + 1];
                    SheetName = (sheetNames[z] != null) ? sheetNames[z].ToString() : "Sheet" + Convert.ToString(z + 1);
                    xlSheet.Name = SheetName;
                    xlSheet.Activate();

                    WriteExcelModule(xlSheet, ds.Tables[z]);
                }
            }

            xlSheet = (Excel.Worksheet)xlBook.Worksheets[1];
            xlSheet.Activate();

            if (save)
            {
                if (savePath != "")
                {
                    if (System.IO.Directory.Exists(savePath) == false)
                    {
                        System.IO.Directory.CreateDirectory(savePath);
                    }
                }
                else
                {
                    savePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                }
                xlBook.SaveAs(savePath + "\\" + fileName, Miss, null, null, null, null, Excel.XlSaveAsAccessMode.xlNoChange,
                    null, null, null, null, null);

                if (quitAfterSave)
                {
                    xlApp.Quit();
                }
            }

            xlSheet = null;
            xlBook = null;
            xlApp = null;
            GC.Collect();
        }

        /// <summary>
        /// 獲取Excel單元格內容，使用完后請手動調用GC.Collect()
        /// </summary>
        /// <param name="fileName">Excel文件，帶路徑</param>
        /// <param name="sheetIndex">Sheet Index，起始為0</param>
        /// <param name="iRow">行，起始為1</param>
        /// <param name="iCol">列，起始為1</param>
        /// <returns></returns>
        public static string GetExcelCellValue(string fileName, int sheetIndex, int iRow, int iCol)
        {
            Missing Miss = Missing.Value;
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlBook = xlApp.Workbooks.Open(fileName, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss);
            Excel.Worksheet xlSheet = (Excel.Worksheet)xlApp.Worksheets[sheetIndex + 1];
            try
            {
                string value = GetExcelCellValue(xlSheet, iRow, iCol);
                return value;
            }
            catch(Exception ex)
            {
                return "";
            }
            finally
            {                
                xlApp.Quit();
                xlSheet = null;
                xlBook = null;
                xlApp = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// 獲取Excel單元格內容，使用完后請手動調用GC.Collect()
        /// </summary>
        /// <param name="fileName">Excel文件，帶路徑</param>
        /// <param name="sheetName">Sheet Name</param>
        /// <param name="iRow">行，起始為1</param>
        /// <param name="iCol">列，起始為1</param>
        /// <returns></returns>
        public static string GetExcelCellValueBySheetName(string fileName, string sheetName, int iRow, int iCol)
        {
            Missing Miss = Missing.Value;
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlBook = xlApp.Workbooks.Open(fileName, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss);
            Excel.Worksheet xlSheet = (Excel.Worksheet)xlApp.Worksheets[sheetName];
            try
            {
                string value = GetExcelCellValue(xlSheet, iRow, iCol);
                return value;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {                
                xlApp.Quit();
                xlSheet = null;
                xlBook = null;
                xlApp = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// 獲取Excel單元格內容
        /// </summary>
        /// <param name="xlSheet"></param>
        /// <param name="iRow"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public static string GetExcelCellValue(Excel.Worksheet xlSheet, int iRow, int iCol)
        {
            return (((Excel.Range)xlSheet.Cells[iRow, iCol]).Value2 != null) ? ((Excel.Range)xlSheet.Cells[iRow, iCol]).Value2.ToString().Trim() : "";
        }
        
        /// <summary>
        /// 獲取Excel單元格內容
        /// </summary>
        /// <param name="xlSheet"></param>
        /// <param name="range">範圍字符串，比如M13，M13:N13</param>
        /// <returns></returns>
        public static object[,] GetExcelCellValue(Excel.Worksheet xlSheet, string range)
        {
            object obj = (((Excel.Range)xlSheet.get_Range(range)).Value2 != null) ? ((Excel.Range)xlSheet.get_Range(range)).Value2 : null;
            if (obj != null)
            {
                return (object[,])obj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 讀取Sheet內容到DataTable，使用完后請手動調用GC.Collect()
        /// </summary>
        /// <param name="fileName">文件名，帶路徑</param>
        /// <param name="sheetIndex">Sheet Index，起始為0</param>
        /// <param name="hasTitle">首行是否是列頭</param>
        /// <returns></returns>
        public static DataTable ReadSheetToDataTable(string fileName, int sheetIndex, bool hasTitle)
        {
            Missing Miss = Missing.Value;
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlBook = xlApp.Workbooks.Open(fileName, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss);
            Excel.Worksheet xlSheet = (Excel.Worksheet)xlApp.Worksheets[sheetIndex + 1];
            try
            {
                DataTable dt = ReadExcelModule(xlSheet, hasTitle);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {                
                xlApp.Quit();
                xlSheet = null;
                xlBook = null;
                xlApp = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// 讀取Sheet內容到DataTable，使用完后請手動調用GC.Collect()
        /// </summary>
        /// <param name="fileName">文件名，帶路徑</param>
        /// <param name="sheetName">Sheet Name</param>
        /// <param name="hasTitle">首行是否是列頭</param>
        /// <returns></returns>
        public static DataTable ReadSheetToDataTableBySheetName(string fileName, string sheetName, bool hasTitle)
        {
            Missing Miss = Missing.Value;
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlBook = xlApp.Workbooks.Open(fileName, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss, Miss);
            Excel.Worksheet xlSheet = (Excel.Worksheet)xlApp.Worksheets[sheetName];
            try
            {
                DataTable dt = ReadExcelModule(xlSheet, hasTitle);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                xlSheet = null;
                xlApp.Quit();
                xlBook = null;
                xlApp = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// 讀取Sheet內容到DataTable
        /// </summary>
        /// <param name="xlSheet">Excel Sheet</param>
        /// <param name="hasTitle">首行是否是列頭</param>
        /// <returns></returns>
        public static DataTable ReadSheetToDataTable(Excel.Worksheet xlSheet, bool hasTitle)
        {
            try
            {
                DataTable dt = ReadExcelModule(xlSheet, hasTitle);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }

        private static DataTable ReadExcelModule(Excel.Worksheet xlSheet, bool hasTitle)
        {
            int columnQty = xlSheet.UsedRange.Columns.Count;
            DataTable dt = new DataTable();
            for (int i = 1; i <= columnQty; i++)
            {
                string colName = hasTitle ? GetExcelCellValue(xlSheet,1,i) : "Column" + i.ToString();
                if (String.IsNullOrEmpty(colName))
                {
                    colName = "Column" + (i + 1).ToString();
                }
                dt.Columns.Add(colName);
            }

            int iStart = hasTitle ? 2 : 1;
            for (int i = iStart; i <= xlSheet.UsedRange.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();

                for (int j = 1; j <= columnQty; j++)
                {
                    dr[j - 1] = GetExcelCellValue(xlSheet, i, j);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 獲取單元格名字，比如A1，AA1
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static string GetCellName(int rowIndex, int columnIndex)
        {
            string characters = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
            string[] array = characters.Split(' ');
            int mod = columnIndex / 26;
            if (mod == 0)
            {
                return array[columnIndex - 1] + rowIndex.ToString();
            }
            else
            {
                try
                {
                    string first = array[mod - 1];
                    string second = array[columnIndex - mod * 26 - 1];
                    return first + second + rowIndex.ToString();
                }
                catch { return ""; }
            }

        }
    }
}
