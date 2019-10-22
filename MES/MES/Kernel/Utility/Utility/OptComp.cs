using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Data;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 包含一些常用的處理控件的方法
    /// </summary>
    public class OptComp
    {
        /// <summary>
        /// 使用DataTable填充ListView，ListView要設置成Detail模式，如果ListView有CheckBox列，並且當前列的值是Y，則勾選，否則不勾選，此方法會調整列寬
        /// </summary>
        /// <param name="lv">Listview</param>
        /// <param name="dt">DataTable</param>
        public static void FillListView(ListView lv, DataTable dt)
        {
            FillListView(lv, dt, true);
        }

        /// <summary>
        /// 使用DataTable填充ListView，ListView要設置成Detail模式，如果ListView有CheckBox列，並且當前列的值是Y，則勾選，否則不勾選
        /// </summary>
        /// <param name="lv">Listview</param>
        /// <param name="dt">DataTable</param>
        /// <param name="autoResize">是否要自動調整列寬，輸入null或false則不調整</param>
        public static void FillListView(ListView lv, DataTable dt, bool autoResize)
        {
            ListViewItem lvi;
            lv.Items.Clear();
            lv.Columns.Clear();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                lv.Columns.Add(dt.Columns[i].Caption.ToString());
            }
            foreach (DataRow dr in dt.Rows)
            {
                lvi = lv.Items.Add(dr[0].ToString());
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    lvi.SubItems.Add(dr[i].ToString());
                    if (lv.CheckBoxes == true && dr[i].ToString() == "Y")
                    {
                        lvi.Checked = true;
                    }
                }
            }
            if (autoResize == null)
            {
                autoResize = false;
            }
            if (autoResize == true)
            {
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }

        }

        /// <summary>
        /// 使用DataTable填充ComobBox
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="cbo">ComboBox</param>
        /// <param name="columnIndex">要填充的DataTable的列索引，以0開始算第一列</param>
        public static void FillComboBox(DataTable dt, ComboBox cbo, int columnIndex)
        {
            cbo.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cbo.Items.Add(dr[columnIndex].ToString());
            }
        }

        /// <summary>
        /// 使用DataTable填充ComobBox
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="cbo">ComboBox</param>
        /// <param name="columnName">要填充的DataTable的列名</param>
        public static void FillComboBox(DataTable dt, ComboBox cbo, string columnName)
        {
            cbo.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cbo.Items.Add(dr[columnName].ToString());
            }
        }

        /// <summary>
        /// 獲取選中的ListView中一行的數據，使用\t分割每一列
        /// </summary>
        /// <param name="lv">Listview</param>
        /// <returns >返回一個字符串</returns>
        public static string GetListViewData(ListView lv)
        {
            string tmpstr = "";

            if (lv.SelectedItems.Count != 0)
            {
                for (int i = 0; i < lv.Columns.Count; i++)
                {
                    tmpstr = tmpstr + "\t" + lv.SelectedItems[0].SubItems[i].Text;
                }
            }
            return tmpstr;
        }

        /// <summary>
        /// 使用DataTable填充ListBox
        /// </summary>
        /// <param name="targetBox">ListBox</param>
        /// <param name="dt">DataTable</param>
        /// <param name="columnIndex">要填充的DataTable的列索引，以0開始算第一列</param>
        public static void FillListBox(ListBox targetBox, DataTable dt, int columnIndex)
        {
            targetBox.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                targetBox.Items.Add(dr[columnIndex].ToString());
            }
        }

        /// <summary>
        /// 使用DataTable填充ListBox
        /// </summary>
        /// <param name="targetBox">ListBox</param>
        /// <param name="dt">DataTable</param>
        /// <param name="columnName">要填充的DataTable的列名</param>
        public static void FillListBox(ListBox targetBox, DataTable dt, string columnName)
        {
            targetBox.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                targetBox.Items.Add(dr[columnName].ToString());
            }
        }

        /// <summary>
        /// 將一個ListBox中所有內容移動到另外一個ListBox中，可作為全選按鈕的代碼使用
        /// </summary>
        /// <param name="sourceBox">源ListBox</param>
        /// <param name="targetBox">目的ListBox</param>
        public static void MoveListBoxAll(ListBox sourceBox, ListBox targetBox)
        {
            string tmpStr = "";
            for (int i = sourceBox.Items.Count; i > 0; i--)
            {
                tmpStr = sourceBox.Items[i - 1].ToString();
                targetBox.Items.Add(tmpStr);
                sourceBox.Items.Remove(tmpStr);
            }
        }

        /// <summary>
        /// 將選中的ListBox的Item移動到另外一個ListBox中
        /// </summary>
        /// <param name="sourceBox">源ListBox</param>
        /// <param name="targetBox">目的ListBox</param>
        public static void MoveListBoxBySelected(ListBox sourceBox, ListBox targetBox)
        {
            int i = sourceBox.SelectedItems.Count;
            if (i == 0)
            {
                return;
            }
            string[] tmpStr = new string[i];
            int j = 0;
            foreach (string lbitem in sourceBox.SelectedItems)
            {
                tmpStr[j] = lbitem;
                j = j + 1;
            }

            foreach (string lbitem in tmpStr)
            {
                targetBox.Items.Add(lbitem);
                sourceBox.Items.Remove(lbitem);
            }
        }

        /// <summary>
        /// 將選中的ListBox的Item進行上移或者下移
        /// </summary>
        /// <param name="targetBox">ListBox</param>
        /// <param name="upOrDown">UP or DOWN</param>
        public static void UpDownListBoxItem(ListBox targetBox, string upOrDown)
        {
            if (targetBox.Items.Count == 0)
            {
                return;
            }
            if (targetBox.SelectedItems.Count != 1)
            {
                return;
            }
            int i = targetBox.SelectedIndex;
            if (upOrDown.Trim().ToUpper() == "UP")
            {
                if (i == 0)
                {
                    return;
                }
                else
                {
                    object obj = targetBox.Items[i];
                    targetBox.Items.RemoveAt(i);
                    targetBox.Items.Insert(i - 1, obj);
                    targetBox.SelectedIndex = i - 1;
                }
            }
            else if (upOrDown.Trim().ToUpper() == "DOWN")
            {
                if (i == targetBox.Items.Count - 1)
                {
                    return;
                }
                else
                {
                    object obj = targetBox.Items[i];
                    targetBox.Items.RemoveAt(i);
                    targetBox.Items.Insert(i + 1, obj);
                    targetBox.SelectedIndex = i + 1;
                }
            }
        }

        /// <summary>
        /// 將選中的DataGridView的Row進行上移或者下移
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        /// <param name="upOrDown">UP or DOWN</param>
        public static void UpDownDataGridViewRow(DataGridView dgv,string upOrDown)
        {
            if (dgv.Rows.Count == 0)
            {
                return;
            }
            if (dgv.SelectedRows.Count == 0)
            {
                return;
            }
            DataGridViewRow dgvr = dgv.SelectedRows[0];
            if(upOrDown.Trim().ToUpper() == "UP")
            {
                if (dgvr.Index == 0)
                {
                    return;
                }
                try
                {
                    int idx = dgvr.Index;
                    dgv.Rows.Remove(dgvr);
                    idx--;
                    dgv.Rows.Insert(idx, dgvr);
                    dgvr.Selected = true;
                }
                catch { }
            }
            else
            {
                if (dgvr.Index == dgv.Rows.Count - 1)
                {
                    return;
                }
                try
                {
                    int idx = dgvr.Index;
                    dgv.Rows.Remove(dgvr);
                    idx++;
                    dgv.Rows.Insert(idx, dgvr);
                    dgvr.Selected = true;
                }
                catch { }
            }
        }
    }
}
