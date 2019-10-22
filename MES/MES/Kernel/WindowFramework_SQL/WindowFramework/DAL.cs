using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace WindowFramework
{
    public class DAL : Liteon.Mes.Db.SQLServer
    {
        public DataTable GetUserInfo(string uid)
        {
            string sql = "SELECT a.EMP_NO,a.EMP_ID,a.EMP_NAME,b.DEPT_NAME FROM SYS_EMP a,SYS_DEPT b WHERE a.EMP_ID=" + uid + " AND a.DEPT_ID=b.DEPT_ID";
            using (DataTable dt = this.ExecSQLReturnDataTable(sql))
            {
                return dt;
            }
        }

        public DataTable GetUserMenu(string uid)
        {
             
            //查詢當前用戶是不是有此程序菜單的權限，如果有，且最小的ID是-1，說明是管理員，顯示所有菜單
            //如果不是-1，則顯示有權限的菜單，如果查詢無結果，說明沒有權限
            string sql = "SELECT MIN(a.FUNCTION_ID) AS FUNCTION_ID FROM SYS_PROGRAM_USERRIGHT a, "
                    + "(SELECT FUNCTION_ID FROM SYS_PROGRAM_LIST WHERE PROGRAM=@v_prog UNION SELECT -1 FROM DUAL)b "
                    + "WHERE a.FUNCTION_ID=b.FUNCTION_ID AND a.EMP_ID=@v_uid";
            string[,] param = { { "v_prog", Para.PROGRAM }, { "v_uid", uid } };
            using (DataTable dt = this.ExecSQLReturnDataTable(sql,param))
            {
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (dt.Rows[0]["FUNCTION_ID"].ToString() == "-1") //管理員
                    {
                        sql = "SELECT MENU,FUNCTION,FUNCTION_ID FROM SYS_PROGRAM_LIST WHERE PROGRAM=@v_prog ORDER BY MENU_INDEX,SEQ_IN_MENU,FUNCTION_ID";
                        string[,] param1 = { { "v_prog", Para.PROGRAM } };
                        using (DataTable dt1 = this.ExecSQLReturnDataTable(sql, param1))
                        {
                            return dt1;
                        }
                    }
                    else
                    {
                        sql = "SELECT MENU,FUNCTION,a.FUNCTION_ID FROM SYS_PROGRAM_USERRIGHT a,SYS_PROGRAM_LIST b"
                            + " WHERE b.PROGRAM=@v_prog AND a.EMP_ID=:v_uid AND a.FUNCTION_ID=b.FUNCTION_ID ORDER BY b.MENU_INDEX,b.SEQ_IN_MENU,a.FUNCTION_ID";
                        using (DataTable dt2 = this.ExecSQLReturnDataTable(sql, param))
                        {
                            return dt2;
                        }
                    }
                }
            }
        }

        public DataTable GetFunctionInfo(string functionID)
        {
            string sql = "SELECT ASSEMBLY_NAME,NAMESPACE,FORM_NAME,DLL_NAME,HOST_NAME,SERVICE_NAME,ACCOUNT,PASSWORD "
                + " FROM SYS_PROGRAM_LIST a,SYS_PROGRAM_LIST_ACCOUNT b WHERE a.FUNCTION_ID=@v_id AND a.ACCOUNT_ID=b.Account_Id";
            string[,] param = { { "v_id", functionID } };
            using (DataTable dt = this.ExecSQLReturnDataTable(sql, param))
            {
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                
                return dt;
            }

        }
    }
}
