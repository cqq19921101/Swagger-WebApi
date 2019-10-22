using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Oracle.ManagedDataAccess.Client;
using Liteon.Mes.Utility;

namespace MesInterface
{
    public class DAL : Liteon.Mes.Db.Oracle
    {
        public DAL()
        {
            string sAccount = ConfigurationManager.AppSettings["ACCOUNT"].ToString();
            string sPwd = ConfigurationManager.AppSettings["PWD"].ToString();
            sAccount = Liteon.Mes.Utility.Encrypt.DesDecrypt(sAccount);
            sPwd = Liteon.Mes.Utility.Encrypt.DesDecrypt(sPwd);

            string sServer = ConfigurationManager.AppSettings["SERVER"].ToString();
            string sDB = ConfigurationManager.AppSettings["DB"].ToString();
            Liteon.Mes.Db.Oracle.CreateConnectionString(sServer, sDB, sAccount, sPwd);
         }


    }
}