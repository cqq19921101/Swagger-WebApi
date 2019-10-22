using System;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Threading;
using System.Net;
using System.Configuration;

namespace MesInterface
{
    public partial class go : System.Web.UI.Page
    {
        BLL_Other bllOther = new BLL_Other();

        string programName = "MesInterface";
        string sUrlContent = "";
        string sLocalIP = "";
        string LogQueryRecord = "";
        string LogAddRecord = "";
        string LogErrorMsg = "";
        string factoryId = "";
        
        string FuncType = "";
        string CommandType = "";
        string CommandCode = "";

        string sn = "";

        int ParaQty = 0;

        string FORBIDGET = "N";
        string currentReqType = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, object> ParaDict = null;

            if (!Page.IsPostBack)
            {
                string sRequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sResponseTime = "";
                sUrlContent = "";
                sLocalIP = Request.UserHostAddress.ToString();

                Response.ContentType = "text/plain";

                ParaDict = new Dictionary<string, object>();
                try
                {
                    sUrlContent = Request.Url.ToString();

                    #region 获取WEB CONFIG配置

                    LogAddRecord = ConfigurationManager.AppSettings["LOGADDRECORD"].ToString().ToUpper();
                    LogQueryRecord = ConfigurationManager.AppSettings["LOGQUERYRECORD"].ToString().ToUpper();
                    LogErrorMsg = ConfigurationManager.AppSettings["SAVEERRORLOG"].ToString().ToUpper();
                    FORBIDGET = ConfigurationManager.AppSettings["FORBIDGET"].ToString().ToUpper().Trim();
                    factoryId = ConfigurationManager.AppSettings["FACTORY_ID"].ToString().ToUpper().Trim();

                    #endregion

                    #region 从POST或GET中得到数据
                    if (Request.RequestType.ToUpper() == "GET")
                    {
                        currentReqType = "GET";
                        ParaQty = Request.QueryString.Count;
                        for (int i = 0; i < Request.QueryString.Count; i++)
                        {
                            try
                            {
                                ParaDict.Add(Request.QueryString.AllKeys[i].ToString(), Request.QueryString[i].ToString());
                            }
                            catch
                            {//重复值异常忽略
                            }
                        }
                    }
                    else if (Request.RequestType.ToUpper() == "POST")
                    {
                        currentReqType = "POST";
                        sUrlContent = sUrlContent + "?";
                        ParaQty = Request.Form.Count;
                        for (int i = 0; i < Request.Form.Count; i++)
                        {
                            try
                            {
                                ParaDict.Add(Request.Form.AllKeys[i].ToString(), Request.Form[i].ToString());
                            }
                            catch
                            {//重复值异常忽略
                            }

                            sUrlContent += Request.Form.AllKeys[i].ToString() + "=" + Request.Form[i].ToString() + "&";
                        }
                        sUrlContent = sUrlContent.Substring(0, sUrlContent.Length - 1);
                    }
                    else
                    {
                        Response.Write("ERROR: Request not in GET,POST");
                        return;
                    }
                    #endregion

                    #region 将URL中的参数赋值给变量

                    InitialParaValue(ParaDict);

                    #endregion

                    #region CommandType判断

                    if (CommandType != "ADD" && CommandType != "QUERY")
                    {
                        if (LogErrorMsg == "Y")
                        {
                            bllOther.SaveErrorLog(factoryId, CommandType, FuncType, sn, "ERROR: Command name not in ADD, QUERY", sUrlContent, sLocalIP);
                        }
                        Response.Write("ERROR: Command name not in ADD,QUERY");
                        return;
                    }

                    #endregion

                    #region 如果Type是空，返回错误
                    if (FuncType == "")
                    {
                        if (LogErrorMsg == "Y")
                        {
                            bllOther.SaveErrorLog(factoryId, CommandType, FuncType, sn, "ERROR: Can not get function type(f)", sUrlContent, sLocalIP);
                        }
                        Response.Write("ERROR: Can not get function type(f)");
                        return;
                    }
                    #endregion

                    #region 根据CommandType来判断参数是不是完整

                    string sCheckCommType = CheckParaByCommondType();

                    if (sCheckCommType != "OK")
                    {
                        if (LogErrorMsg == "Y")
                        {
                            bllOther.SaveErrorLog(factoryId, CommandType, FuncType, sn, sCheckCommType, sUrlContent, sLocalIP);
                        }
                        Response.Write(sCheckCommType);
                        sCheckCommType = null;
                        return;
                    }
                    else
                    {
                        sCheckCommType = null;
                    }
                    #endregion

                    #region 根据CommandType来记录信息

                    if (CommandType == "ADD")
                    {
                        if (FORBIDGET == "Y" && currentReqType == "GET")
                        {
                            if (LogErrorMsg == "Y")
                            {
                                bllOther.SaveErrorLog(factoryId, CommandType, FuncType, sn, "ERROR: Can not use GET type to upload data", sUrlContent, sLocalIP);
                            }
                            Response.Write("ERROR: Can not use GET type to upload data");
                            return;
                        }
                        BLL bll = GetBllClass();
                        using (DataTable dtAddResult = bll.DoAdd(CommandCode,factoryId,ParaDict,sUrlContent))
                        {
                            if (dtAddResult.Rows[0][0].ToString().ToUpper() == "FAIL")
                            {
                                string sErrmsg = dtAddResult.Rows[0][1].ToString();
                                if (LogErrorMsg == "Y")
                                {
                                    bllOther.SaveErrorLog(factoryId, CommandType, FuncType, sn, "ERROR: " + sErrmsg, sUrlContent, sLocalIP);
                                }
                                Response.Write("ERROR: " + sErrmsg);
                            }
                            else
                            {
                                if (dtAddResult.Columns.Count == 1)
                                {
                                    Response.Write("OK");
                                }
                                else
                                {
                                    if (dtAddResult.Rows[0][1].ToString() == "")
                                    {
                                        Response.Write("OK");
                                    }
                                    else
                                    {
                                        Response.Write("OK;" + dtAddResult.Rows[0][1].ToString());
                                    }
                                }
                            }
                        }
                        if (LogAddRecord == "Y")
                        {
                            sResponseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            bllOther.SaveLog(factoryId, CommandType, FuncType, sn, sRequestTime, sResponseTime, sUrlContent, sLocalIP);
                        }
                    }

                    if (CommandType == "QUERY")
                    {
                        BLL bll = GetBllClass();
                        using (DataSet dsQR = bll.DoQuery(CommandCode,factoryId,ParaDict,sUrlContent))
                        {
                            if (dsQR.Tables[0].Rows[0][0].ToString().ToUpper() == "OK" || dsQR.Tables[0].Rows[0][0].ToString().ToUpper() == "PASS")
                            {
                                Response.Write("OK\u000a");
                                if (dsQR.Tables.Count > 1)
                                {
                                    for (int i = 0; i < dsQR.Tables[1].Rows.Count; i++)
                                    {
                                        if (i < dsQR.Tables[1].Rows.Count - 1)
                                        {
                                            Response.Write(dsQR.Tables[1].Rows[i]["Category"].ToString() + "=" + dsQR.Tables[1].Rows[i]["Value"].ToString() + "\u000a");
                                        }
                                        else
                                        {
                                            Response.Write(dsQR.Tables[1].Rows[i]["Category"].ToString() + "=" + dsQR.Tables[1].Rows[i]["Value"].ToString());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LogErrorMsg == "Y")
                                {
                                    bllOther.SaveErrorLog(factoryId, CommandType, FuncType, sn, "ERROR: " + dsQR.Tables[0].Rows[0][1].ToString(), sUrlContent, sLocalIP);
                                }
                                Response.Write("ERROR: " + dsQR.Tables[0].Rows[0][1].ToString());
                            }
                        }
                        if (LogQueryRecord == "Y")
                        {
                            sResponseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            bllOther.SaveLog(factoryId, CommandType, FuncType, sn, sRequestTime, sResponseTime, sUrlContent, sLocalIP);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    Liteon.Mes.Db.SaveProgramExecLog.DoSaveErrorLog(Liteon.Mes.Db.DbType.Oracle,
                        programName, "Page_Load", ex.ToString(), ex.Message, sUrlContent, false);
                    if (LogErrorMsg == "Y")
                    {
                        bllOther.SaveErrorLog(factoryId, CommandType, FuncType, sn, "ERROR: " + ex.Message, sUrlContent, sLocalIP);
                    }
                    Response.Write("ERROR: " + ex.Message);
                }
                finally
                {

                }
            }
        }


        private void InitialParaValue(Dictionary<string, object> dict)
        {
            CommandType = bllOther.GetParaValueReturnString(dict, "c").ToUpper();
            CommandCode = bllOther.GetParaValueReturnString(dict, "code").ToUpper();
            if (CommandCode == "")
            {
                CommandCode = "1";
            }
            FuncType = bllOther.GetParaValueReturnString(dict, "f").ToUpper();
            sn = bllOther.GetParaValueReturnString(dict, "sn").ToUpper();
        }

        

        private string CheckParaByCommondType()
        {
            if (CommandType == "ADD" || CommandType == "QUERY")
            {
                if (ParaQty < 3)
                {
                    return "ERROR: Command type " + CommandType + " need at list 3 commands";
                }
            }
            if (FuncType == "")
            {
                return "ERROR: f(func type) is required";
            }
           
            return "OK";
        }

        private bool TimeFormatCheck(string Time)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) ([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
            if (regex.IsMatch(Time))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private BLL GetBllClass()
        {
            switch (FuncType)
            {
                case "STD":
                    return new BLL_STD();                   
                case "SMTALERT":
                    return new BLL_SmtAlert();
                case "CHECKSFC":
                    return new BLL_CheckSFCDll();
                case "AGVCALLMATERIAL":
                    return new BLL_AGVCallMaterial();
                case "LORA":
                    return new BLL_LORA();
                case "SMTSPI":
                    return new BLL_SMTSPI();
                case "SMTAOI":
                    return new BLL_SMTSPI();
                case "EASYLINKIN":
                    return new BLL_Easylinkin();
                case "PICKING":
                    return new BLL_Picking();
                case "SOLDER":
                    return new BLL_Solder();
                case "HOLLEY":
                    return new BLL_Holley();
                case "EASYLINKINGWEUI":
                    return new BLL_EasylinkGWEUI();
                case "ALI":
                    return new BLL_ALi();
                case "REFLOWSTATUS":
                    return new BLL_ReflowStatus();
                case "CASTLENET":
                    return new BLL_Castlenet();
                case "EASYLINKIN_ALI":
                    return new BLL_EasylinkinAli();
                case "ATTR":
                    return new BLL_ATTR();
                default:
                    return null;
            }
        }
    }
}