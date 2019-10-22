using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WindowFramework
{
    public class BLL
    {
        DAL dal = new DAL();

        public UserInfo GetCurrentUserInfo(string uid)
        {
            using(DataTable dt=dal.GetUserInfo(uid))
            {
                if(dt.Rows.Count==0)
                {
                    return null;
                }
                else
                {
                    UserInfo ui = new UserInfo
                    {
                        Uid = dt.Rows[0]["EMP_ID"].ToString(),
                        Emp_No = dt.Rows[0]["EMP_NO"].ToString(),
                        Emp_Name = dt.Rows[0]["EMP_NAME"].ToString(),
                        Dept = dt.Rows[0]["DEPT_NAME"].ToString()
                    };
                    return ui;
                }
            }
            
        }

        public DataTable GetUserMenu(string uid)
        {
            return dal.GetUserMenu(uid);
        }

        public DataTable GetFunctionInfo(string functionID)
        {
            return dal.GetFunctionInfo(functionID);
        }
    }
}
