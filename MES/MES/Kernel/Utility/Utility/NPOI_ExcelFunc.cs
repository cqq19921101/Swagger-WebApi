using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 使用NPOI進行EXCEL導出和讀取
    /// </summary>
    public class NPOI_ExcelFunc
    {
        /// <summary>
        /// 查詢Excel中有多少個sheet，并返回名稱
        /// </summary>
        /// <param name="fileName">Excel文件，完整路徑</param>
        /// <returns>返回字符串數組，包含所有的sheet名稱</returns>
        public static string[] GetSheetNameFromExcel(string fileName)
        {
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileName);
                int sheetQty = xssfworkbook.NumberOfSheets;
                string[] sheetNames = new string[sheetQty];
                for (int i = 0; i < sheetQty; i++)
                {
                    sheetNames[i] = xssfworkbook.GetSheetName(i);
                }
                xssfworkbook.Close();
                xssfworkbook = null;
                return sheetNames;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 寫Excel模塊
        /// </summary>
        /// <param name="xssfworkbook"></param>
        /// <param name="sheet"></param>
        /// <param name="dt"></param>
        private static void WriteExcelModule(XSSFWorkbook xssfworkbook, ISheet sheet, DataTable dt)
        {
            //创建一行
            IRow firstRow = sheet.CreateRow(0);

            //创建一个单元格
            ICell cell = null;

            //字体
            IFont font = xssfworkbook.CreateFont();
            font.FontName = "Verdana";
            font.FontHeightInPoints = 10;

            //创建单元格样式
            ICellStyle cellStyleTitle = xssfworkbook.CreateCellStyle();
            cellStyleTitle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
            //cellStyleTitle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
            cellStyleTitle.FillPattern = FillPattern.SolidForeground;

            cellStyleTitle.SetFont(font);
            cellStyleTitle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyleTitle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            cellStyleTitle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleTitle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleTitle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleTitle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            ICellStyle cellStyle = xssfworkbook.CreateCellStyle();
            cellStyle.SetFont(font);

            //创建格式
            IDataFormat dataFormat = xssfworkbook.CreateDataFormat();

            //设置为文本格式，也可以为 text，即 dataFormat.GetFormat("text");
            cellStyle.DataFormat = dataFormat.GetFormat("@");

            //设置列名
            foreach (DataColumn col in dt.Columns)
            {
                //创建单元格并设置单元格内容
                firstRow.CreateCell(col.Ordinal).SetCellValue(col.Caption);

                //设置单元格格式
                firstRow.Cells[col.Ordinal].CellStyle = cellStyleTitle;
            }

            //写入数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //跳过第一行，第一行为列名
                IRow row = sheet.CreateRow(i + 1);

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    cell = row.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                    cell.CellStyle = cellStyle;
                }
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        /// <summary>
        /// DataTable導出到Excel
        /// </summary>
        /// <param name="dt">要導出的DataTable</param>
        /// <param name="fileName">指定的文件名，不帶路徑，放到桌面上</param>
        public static void ExportToExcel(DataTable dt,string fileName)
        {
            ExportToExcel(dt, "", fileName);
        }

        /// <summary>
        /// DataTable導出到Excel，可以指定SheetName、文件名和是否要保存
        /// </summary>
        /// <param name="dt">要導出的DataTable</param>
        /// <param name="sheetName">指定的SheetName，輸入空白或者null則使用Result作為SheetName</param>
        /// <param name="fileName">指定的文件名，不帶路徑，放到桌面上</param>
        public static void ExportToExcel(DataTable dt, string sheetName, string fileName)
        {
            ExportToExcel(dt, sheetName, fileName, "");
        }

        /// <summary>
        /// DataTable導出到Excel，可以指定SheetName、文件名和是否要保存，也可以指定保存后是否自動退出
        /// </summary>
        /// <param name="dt">要導出的DataTable</param>
        /// <param name="sheetName">指定的SheetName，輸入空白或者null則使用Result作為SheetName</param>
        /// <param name="fileName">指定的文件名，不帶路徑，如果savePath是空或null則放到桌面上</param>
        /// <param name="savePath">文件存放的路徑，最後不要帶\\</param>
        public static void ExportToExcel(DataTable dt, string sheetName, string fileName, string savePath)
        {
            if (dt == null)
            {
                return;
            }
            try
            {
                //创建一个工作簿
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                if (String.IsNullOrEmpty(sheetName))
                {
                    sheetName = "Result";
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = "Result.xlsx";
                }
                //创建一个 sheet 表
                ISheet sheet = xssfworkbook.CreateSheet(sheetName);

                WriteExcelModule(xssfworkbook, sheet, dt);
                
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

                //创建文件
                FileStream file = new FileStream(savePath+"\\"+fileName, FileMode.CreateNew, FileAccess.Write);
                xssfworkbook.Write(file);

                file.Close();
                file.Dispose();

                xssfworkbook.Close();
                sheet = null;
                xssfworkbook = null;

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// DataSet導出到Excel，可以指定SheetName、文件名和是否要保存
        /// </summary>
        /// <param name="ds">要導出的DataSet</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        public static void ExportToExcel(DataSet ds, string fileName)
        {
            ExportToExcel(ds, null, fileName, "");
        }

        /// <summary>
        /// DataSet導出到Excel，可以指定SheetName、文件名和是否要保存
        /// </summary>
        /// <param name="ds">要導出的DataSet</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        /// <param name="savePath">文件存放的路徑，最後不要帶\\</param>
        public static void ExportToExcel(DataSet ds, string fileName,string savePath)
        {
            ExportToExcel(ds, null, fileName, savePath);
        }

        /// <summary>
        /// DataSet導出到Excel，可以指定SheetName、文件名和是否要保存
        /// </summary>
        /// <param name="ds">要導出的DataSet</param>
        /// <param name="sheetNames">字符串數組，用來存放SheetName，數量需要和DataSet中的Table數一致，輸入null則自動給sheet編號</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        public static void ExportToExcel(DataSet ds, string[] sheetNames, string fileName)
        {
            ExportToExcel(ds, sheetNames, fileName,  "");
        }

        /// <summary>
        /// DataSet導出到Excel，可以指定SheetName、文件名和是否要保存，也可以指定保存后是否自動退出
        /// </summary>
        /// <param name="ds">要導出的DataSet</param>
        /// <param name="sheetNames">字符串數組，用來存放SheetName，數量需要和DataSet中的Table數一致，輸入null則自動給sheet編號</param>
        /// <param name="fileName">指定的文件名，不帶路徑</param>
        /// <param name="savePath">文件存放的路徑，最後不要帶\\</param>
        public static void ExportToExcel(DataSet ds, string[] sheetNames, string fileName, string savePath)
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
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "Result.xlsx";
            }
            try
            {
                //创建一个工作簿
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                for (int i = 1; i <= ds.Tables.Count; i++)
                {
                    //创建一个 sheet 表
                    string sheetName = (sheetNames[i - 1] != null) ? sheetNames[i - 1].ToString() : "Sheet" + i.ToString();
                    ISheet sheet = xssfworkbook.CreateSheet(sheetName);
                    WriteExcelModule(xssfworkbook, sheet, ds.Tables[i - 1]);
                    sheet = null;
                }
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

                //创建文件
                FileStream file = new FileStream(savePath + "\\" + fileName, FileMode.CreateNew, FileAccess.Write);
                xssfworkbook.Write(file);

                file.Close();
                file.Dispose();

                xssfworkbook.Close();
                xssfworkbook = null;
            }
            catch (Exception ex)
            {

            }            
        }

        /// <summary>
        /// 獲取Excel單元格內容
        /// </summary>
        /// <param name="sheet">Sheet</param>
        /// <param name="iRow">行，起始為1</param>
        /// <param name="iCol">列，起始為1</param>
        /// <returns></returns>
        public static string GetExcelCellValue(ISheet sheet, int iRow, int iCol)
        {
            try
            {
                return sheet.GetRow(iRow - 1).GetCell(iCol - 1).ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取Excel單元格內容
        /// </summary>
        /// <param name="fileName">Excel文件，帶路徑</param>
        /// <param name="sheetIndex">Sheet Index，起始為0</param>
        /// <param name="iRow">行，起始為1</param>
        /// <param name="iCol">列，起始為1</param>
        /// <returns></returns>
        public static string GetExcelCellValue(string fileName,int sheetIndex,int iRow,int iCol)
        {
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileName);
                ISheet sheet = xssfworkbook.GetSheetAt(sheetIndex);
                string result = GetExcelCellValue(sheet, iRow, iCol);
                xssfworkbook.Close();
                sheet = null;
                xssfworkbook = null;
                return result;
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 獲取Excel單元格內容
        /// </summary>
        /// <param name="fileName">Excel文件，帶路徑</param>
        /// <param name="sheetName">Sheet Name，起始為0</param>
        /// <param name="iRow">行，起始為1</param>
        /// <param name="iCol">列，起始為1</param>
        /// <returns></returns>
        public static string GetExcelCellValueBySheetName(string fileName, string sheetName, int iRow, int iCol)
        {
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileName);
                ISheet sheet = xssfworkbook.GetSheet(sheetName);
                string result = GetExcelCellValue(sheet, iRow, iCol);
                xssfworkbook.Close();
                sheet = null;
                xssfworkbook = null;
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 讀取Sheet內容到DataTable
        /// </summary>
        /// <param name="fileName">文件名，帶路徑</param>
        /// <param name="sheetIndex">Sheet Index，起始為0</param>
        /// <param name="hasTitle">首行是否是列頭</param>
        /// <returns></returns>
        public static DataTable ReadSheetToDataTable(string fileName, int sheetIndex, bool hasTitle)
        {
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileName);
                ISheet sheet = xssfworkbook.GetSheetAt(sheetIndex);

                DataTable dt = ReadExcelModule(sheet, hasTitle);

                xssfworkbook.Close();
                sheet = null;
                xssfworkbook = null;
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 讀取Sheet內容到DataTable
        /// </summary>
        /// <param name="fileName">文件名，帶路徑</param>
        /// <param name="sheetName">Sheet Name</param>
        /// <param name="hasTitle">首行是否是列頭</param>
        /// <returns></returns>
        public static DataTable ReadSheetToDataTableBySheetName(string fileName,string sheetName,bool hasTitle)
        {
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileName);
                ISheet sheet = xssfworkbook.GetSheet(sheetName);

                DataTable dt = ReadExcelModule(sheet, hasTitle);

                xssfworkbook.Close();
                sheet = null;
                xssfworkbook = null;
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 獲取EXCEL的行和列數
        /// </summary>
        /// <param name="fileName">文件名，帶路徑</param>
        /// <param name="sheetIndex">Sheet Index，起始為0</param>
        /// <returns>數字，第一個元素是行數，第二個元素是列數</returns>
        public static int[] GetSheetRowCountAndColumnCount(string fileName, int sheetIndex)
        {
            int[] qtys = new int[2];
            qtys[0] = 0;
            qtys[1] = 0;
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileName);
                ISheet sheet = xssfworkbook.GetSheetAt(sheetIndex);
                IRow firstRow = sheet.GetRow(0);
                qtys[0] = sheet.PhysicalNumberOfRows;
                qtys[1] = firstRow.PhysicalNumberOfCells;

                xssfworkbook.Close();
                sheet = null;
                xssfworkbook = null;
                return qtys;
            }
            catch (Exception ex)
            {
                return qtys;
            }
        }

        /// <summary>
        /// 獲取EXCEL的行和列數
        /// </summary>
        /// <param name="fileName">文件名，帶路徑</param>
        /// <param name="sheetName">Sheet Name</param>
        /// <returns>數字，第一個元素是行數，第二個元素是列數</returns>
        public static int[] GetSheetRowCountAndColumnCountBySheetName(string fileName, string sheetName)
        {
            int[] qtys = new int[2];
            qtys[0] = 0;
            qtys[1] = 0;
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileName);
                ISheet sheet = xssfworkbook.GetSheet(sheetName);
                IRow firstRow = sheet.GetRow(0);
                qtys[0] = sheet.PhysicalNumberOfRows;
                qtys[1] = firstRow.PhysicalNumberOfCells;

                xssfworkbook.Close();
                sheet = null;
                xssfworkbook = null;
                return qtys;
            }
            catch (Exception ex)
            {
                return qtys;
            }
        }

        private static DataTable ReadExcelModule(ISheet sheet,bool hasTitle)
        {
            IRow firstRow = sheet.GetRow(0);
            int columnQty = firstRow.PhysicalNumberOfCells;
            DataTable dt = new DataTable();
            for (int i = 0; i < columnQty; i++)
            {
                string colName = hasTitle ? firstRow.GetCell(i).ToString() : "Column" + (i + 1).ToString();
                if (String.IsNullOrEmpty(colName))
                {
                    colName = "Column" + (i + 1).ToString();
                }
                dt.Columns.Add(colName);
            }

            int iStart = hasTitle ? 2 : 1;
            for (int i = iStart; i <= sheet.PhysicalNumberOfRows; i++)
            {
                DataRow dr = dt.NewRow();

                for (int j = 1; j <= columnQty; j++)
                {
                    dr[j - 1] = GetExcelCellValue(sheet, i, j);
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
