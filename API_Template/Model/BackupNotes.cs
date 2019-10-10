using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Template.Model
{
    public class BackupNotes
    {
    }

    #region TECO
    //public static string GetTECO(TECO_Input Parameter)
    //{
    //    try
    //    {
    //        var FuncTeco = new DP_TECO();
    //        DataTable dt = new DataTable();
    //        dt = FuncTeco.ReturnTECOByCZOPS(Parameter.PlantNo);
    //        //dt == dt.Select("");
    //        return JsonConvert.SerializeObject(dt);
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    #endregion

    #region NSB
    //public static string GetNSB(GetNSB_Input Parameter)
    //{
    //    StringBuilder sb = new StringBuilder();

    //    switch (Parameter.PRODH.ToUpper())
    //    {
    //        case "ALL":
    //            sb.Append(@"
    //                        SELECT T1.SPMON,'ALL' AS PRODH, Sum(convert(bigint,T1.NETWR)) as currentNSB, 
    //                        Convert(bigint,T2.LineCode) as targetNSB
    //                        FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
    //                        where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
    //                        and T2.Line = 'NSB' AND  T2.Value1 = 'ALL'
    //                        group by T2.LineCode,T1.SPMON");
    //            opc.Clear();
    //            break;
    //        default:
    //            sb.Append(@"SELECT T1.SPMON, T1.PRODH,Sum(convert(bigint,T1.NETWR)) as currentNSB, Convert(bigint,T2.LineCode) as targetNSB
    //                FROM [SAPTEST].[dbo].[ZTVDSS913] T1,ICM657.GreenPower.dbo.TB_Line_Param T2
    //                where  T1.SPMON = REPLACE(convert(varchar(7),GETDATE(),121),'-','') and T1.WERKS = @WERKS
    //                and T2.Line = 'NSB'  and T2.Value1 = @PRODH
    //                ");
    //            opc.Clear();
    //            sb.Append(" and T1.PRODH = @PRODH");
    //            sb.Append(" Group by T2.LineCode,T1.SPMON,T1.PRODH");
    //            opc.Add(DataPara.CreateDataParameter("@PRODH", SqlDbType.NVarChar, Parameter.PRODH));
    //            break;
    //    }
    //    opc.Add(DataPara.CreateDataParameter("@WERKS", SqlDbType.NVarChar, Parameter.WERKS));
    //    DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
    //    //string NETWR = sdb.GetRowString(sb.ToString(), opc, "NETWR");
    //    //m.Add(NETWR);
    //    return JsonConvert.SerializeObject(dt);

    //}
    #endregion

    #region NUB
    //暫時注釋非異步處理 GetNUB
    //public static ReturnMessage GetNUB(GetNUB_Input item)
    //{
    //    ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
    //    string Result = string.Empty;
    //    try
    //    {
    //        Result = GetNUB_Helper.GetNUB(item);
    //        JArray jArray = JArray.Parse(Result);
    //        rm.Success = true;
    //        rm.Status = "success";
    //        rm.Command = "GetNUB";
    //        rm.Array = jArray;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error("Got error. " + ex.Message);
    //        rm.Success = false;
    //        rm.Status = "Error";
    //    }

    //    return rm;
    //}
    #endregion

    #region SmartMeter
    //public static string GetMeterKWHAlert(SmartMeterKWHAlert_Input Parameter)
    //{
    //    int Limit = GetLimitReal(Parameter.did);//根據did抓取對應的上限值
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append(@"select Top 1 T2.Line, T1.did, T1.type, convert(varchar(16), T1.dt, 121) as dt, Convert(int,T1.total)  AS ActValue, Convert(int,T3.VALUE1) AS TargetValue
    //                from KWH T1, dbo.TB_Line_Param T2, dbo.TB_APPLICATION_PARAM T3
    //                where T3.VALUE2 = @did and T1.did = @did and T2.LineCode = @did
    //                and T1.type = 'R'
    //                and DateDiff(dd, T1.dt, getdate()) = 0 and total > @total
    //                order by dt desc");
    //    opc.Clear();
    //    opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
    //    opc.Add(DataPara.CreateDataParameter("@total", SqlDbType.Int, Limit));
    //    DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
    //    return JsonConvert.SerializeObject(dt);

    //}

    //public static string GetMeterKWH(SmartMeterKWH_Input Parameter)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append(@" select T3.Line, T1.did, SUM(convert(int,T1.total)) as ActValue, Convert(int,T2.VALUE1) as TargetValue 
    //                 from dbo.KWH T1, TB_APPLICATION_PARAM T2, dbo.TB_Line_Param T3
    //                 where  T1.type = 'R' 
    //                 and T2.PARAME_ITEM = 'upper'  ");
    //    sb.Append(" AND T2.VALUE2 = @VALUE2 and T1.did = @did and T3.LineCode = @LineCode");
    //    opc.Clear();
    //    switch (Parameter.functiontype)
    //    {
    //        case "Day":
    //            sb.Append(" and DateDiff(dd,dt,getdate())=0 and T2.VALUE5 = 'Day'");
    //            break;
    //        case "Month":
    //            sb.Append(" and DateDiff(MM,dt,getdate())=0 and T2.VALUE5 = 'Month'");
    //            break;
    //        default:
    //            sb.Append(" T1.did = ''");
    //            break;
    //    }
    //    sb.Append(" group by did, VALUE1, PARAME_NAME, Line");
    //    switch (Parameter.did)
    //    {
    //        //OSD車間 
    //        case "190160":
    //        case "190161":
    //        case "190174":
    //        case "190175":
    //            opc.Add(DataPara.CreateDataParameter("@VALUE2", SqlDbType.NVarChar, "190160"));
    //            opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
    //            opc.Add(DataPara.CreateDataParameter("@LineCode", SqlDbType.NVarChar, "190160"));
    //            break;
    //        default:
    //            opc.Add(DataPara.CreateDataParameter("@VALUE2", SqlDbType.NVarChar, Parameter.did));
    //            opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
    //            opc.Add(DataPara.CreateDataParameter("@LineCode", SqlDbType.NVarChar, Parameter.did));
    //            break;
    //    }
    //    DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
    //    return JsonConvert.SerializeObject(dt);

    //    //opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
    //    //DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
    //    //return JsonConvert.SerializeObject(dt);

    //}

    //public static string GetMeterUTS(SmartMeterUTS_Input Parameter)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append(@"select  PLANTNO,PRODUCTLINECODE, convert(varchar(10),GR_DATE,121) as GR_DATE,QTY from UTS where PRODUCTLINECODE = @PRODUCTLINECODE and PLANTNO = @PLANTNO   order by GR_DATE desc");
    //    opc.Clear();
    //    opc.Add(DataPara.CreateDataParameter("@PRODUCTLINECODE", SqlDbType.NVarChar, Parameter.PRODUCTLINECODE));
    //    opc.Add(DataPara.CreateDataParameter("@PLANTNO", SqlDbType.NVarChar, Parameter.PLANTNO));
    //    DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
    //    return JsonConvert.SerializeObject(dt);
    //}

    #endregion

    #region SmartWater
    //public static string GetWaterAlert(SmartWaterAlert_Input Parameter)
    //{
    //    int Limit = GetLimitDay();//上限值
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append(@"select Top 1  T1.did, T1.type, convert(varchar(16), T1.dt, 121) as dt,T1.UPDATE_TIME, Convert(int,T1.total)  AS ActValue, 
    //       Convert(Float,T3.VALUE1) AS TargetValue
    //                from Water T1, dbo.TB_Line_Param T2, dbo.TB_APPLICATION_PARAM T3
    //                where T3.VALUE2 = 'A1' and T1.did = @did and T2.LineCode = @did
    //                and T1.type = 'R'
    //                and DateDiff(dd, T1.dt, getdate()) = 0 
    //                and total > @total
    //                order by dt desc ");
    //    opc.Clear();
    //    opc.Add(DataPara.CreateDataParameter("@did", SqlDbType.NVarChar, Parameter.did));
    //    opc.Add(DataPara.CreateDataParameter("@total", SqlDbType.Int, Limit));
    //    DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
    //    return JsonConvert.SerializeObject(dt);

    //}


    //public static string GetWater(SmartWater_Input Parameter)
    //{
    //    SqlConnection cn = new SqlConnection(conn);
    //    cn.Open();
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append(@" select T1.did,SUM(convert(int,T1.total)) as ActValue,Convert(int,T2.VALUE1) as TargetValue 
    //                 from dbo.[Water] T1,TB_APPLICATION_PARAM T2
    //                 where T1.type = 'R' 
    //                 and T2.PARAME_ITEM = 'upper'  AND T2.VALUE2 = 'A1' and T1.did = @did
    //                  ");
    //    opc.Clear();
    //    switch (Parameter.functiontype)
    //    {
    //        case "Day":
    //            sb.Append("  and DateDiff(dd,T1.dt,getdate())=0 and T2.VALUE5 = 'Day'");
    //            break;
    //        case "Month":
    //            sb.Append("  and DateDiff(dd,T1.dt,getdate())=0 and T2.VALUE5 = 'Month'");
    //            break;
    //        default:
    //            sb.Append(" T1.did = ''");
    //            break;
    //    }
    //    sb.Append(" group by VALUE1,did");
    //    DataTable dt = new DataTable();
    //    using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
    //    {
    //        cmd.Parameters.AddWithValue("@did", Parameter.did);
    //        using (SqlDataReader dr = cmd.ExecuteReader())
    //        {
    //            dt.Load(dr);
    //        }
    //    }
    //    return JsonConvert.SerializeObject(dt);

    //}

    #endregion

    #region TECO
    //public static async Task<TECO_Output> GetTECO(TECO_Input Parameter)
    //{
    //    TECO_Output rm = new TECO_Output();
    //    try
    //    {
    //        await Task.Run(() =>
    //        {
    //            var FuncTeco = new DP_TECO();
    //            DataTable dt = new DataTable();
    //            dt = FuncTeco.ReturnTECOByCZOPS(Parameter.PlantNo,Parameter.Line);
    //            JArray jArray = JArray.Parse(JsonConvert.SerializeObject(dt));
    //            rm.Success = true;
    //            rm.Status = "success";
    //            rm.Command = "GetTECO";
    //            rm.Array = jArray;
    //        });

    //    }
    //    catch (Exception ex)
    //    {
    //        rm.Success = false;
    //        rm.Status = "Error";
    //        rm.Command = "GetTECO";
    //    }
    //    return rm;
    //}    
    #endregion

    #region HC
    //public ReturnMessage GetHC_QueryDLBuffer(GetHCDLBuffer_Input item)
    //{
    //    ReturnMessage rm = new ReturnMessage();//new 一個返回的請求狀態類
    //    string Result = "";
    //    try
    //    {
    //        Result = GetHC_Helper.GetHC_QueryDLBuffer(item);
    //        JArray jArray = JArray.Parse(Result);
    //        rm.Success = true;
    //        rm.Status = "success";
    //        rm.Command = "GetHC_QueryDLBuffer";
    //        rm.Array = jArray;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error("Got error. " + ex.Message);
    //        rm.Success = false;
    //        rm.Status = "Error";
    //    }

    //    return rm;
    //}

    //public static async Task<GetHCDLBuffer_Output> GetHC_DLBuffer(GetHCDLBuffer_Input Parameter)
    //{
    //    GetHCDLBuffer_Output rm = new GetHCDLBuffer_Output();

    //    try
    //    {
    //        await Task.Run(() =>
    //        {
    //            opc.Clear();
    //            string Date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
    //            opc.Add(DataPara.CreateProcParameter("@P_BU", SqlDbType.VarChar, 10, ParameterDirection.Input, Parameter.BU));
    //            opc.Add(DataPara.CreateProcParameter("@P_DATE", SqlDbType.VarChar, 10, ParameterDirection.Input, Date));
    //            opc.Add(DataPara.CreateProcParameter("@P_DEPTID", SqlDbType.VarChar, 10, ParameterDirection.Input, Parameter.DEPT_ID));
    //            DataTable dt = sdb.RunProc2("P_DailyReprot_QueryDLBuffer_API", opc);
    //            JArray jArray = JArray.Parse(JsonConvert.SerializeObject(dt));
    //            rm.Success = true;
    //            rm.Status = "success";
    //            rm.Command = "GetHC";
    //            rm.Array = jArray;
    //        });


    //    }
    //    catch (Exception ex)
    //    {
    //        rm.Success = false;
    //        rm.Status = "Error";
    //        rm.Command = "GetHC";
    //    }
    //    return rm;
    //}

    #endregion

}