using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using LiteOn.ea.GZ.PPM.MFGUtility.Common;
using LiteOn.ea.GZ.PPM.MFGUtility.DBUtility;
using System.Security.Permissions;
using System.IO;

/// <summary>
/// Summary description for DataExchangeService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class DataExchangeService : System.Web.Services.WebService
{

    public DataExchangeService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool UpdateSFCPacking(string DNNo, string startTime, string endTime, out string errorMessge)
    {
        //Util.WriteToLog(string.Format("DN No:{0}   BeginLadingTime:{1}   EndLadingTime:{2}", DNNo, startTime, endTime), "DPCService-UpdateSFCPacking");
        WriteLog("UpdateSFCPacking", "Start.");
        if (string.IsNullOrEmpty(DNNo)||string.IsNullOrEmpty(startTime)||string.IsNullOrEmpty(endTime))
        {
            errorMessge = "請提供呼叫參數!";
            return false;
        }
        DateTime beginLadingTime;
        if (!DateTime.TryParse(startTime, out beginLadingTime))
        {
            errorMessge = "參數【startTime】值有誤!";
            return false;
        }
        DateTime endLadingTime;
        if (!DateTime.TryParse(endTime, out endLadingTime))
        {
            errorMessge = "參數【endTime】值有誤!";
            return false;
        }

        SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@DNNo", SqlDbType.VarChar, 10),
                    new SqlParameter("@beginLadingTime", SqlDbType.DateTime),
					new SqlParameter("@endLadingTime", SqlDbType.DateTime),
                    new SqlParameter("@ExecuteTime", SqlDbType.DateTime)
            };

        parms[0].Value = DNNo;
        parms[1].Value = beginLadingTime;
        parms[2].Value = endLadingTime;
        parms[3].Value = DateTime.Now;

        string sql = @"select count(*) from [tblDPCSFCComplete] where [DNNo]=@DNNo";
        object n = SqlHelper.ExecuteScalar(Util.getConnStr(), CommandType.Text, sql, parms);

        if (Convert.ToInt32(n) > 0)
        {
            sql = @"update [dbo].[tblDPCSFCComplete] set BeginLadingTime=ISNULL(BeginLadingTime,@beginLadingTime),EndLadingTime=ISNULL(EndLadingTime,@endLadingTime) WHERE DNNo=@DNNo";
        }
        else
        {
            sql = @"INSERT INTO [dbo].[tblDPCSFCComplete]
                               ([DNNo]
                               ,[BeginLadingTime]
                               ,[EndLadingTime]
                               ,[ExecuteTime])
                         VALUES
                               (@DNNo
                               ,@beginLadingTime
                               ,@endLadingTime
                               ,@executeTime)";
        }
        sql += @" 
                update tblDPCBillOfLadingDetail 
                set BeginLadingTime=ISNULL(BeginLadingTime,@beginLadingTime),EndLadingTime=ISNULL(EndLadingTime,@endLadingTime) 
                WHERE DNNo=@DNNo and EndLadingTime is null";

        try
        {
            SqlHelper.ExecuteNonQuery(Util.getConnStr(), CommandType.Text, sql, parms);

            errorMessge = "";
            return true;
        }
        catch(Exception ex)
        {
            errorMessge = "SQL執行錯誤：" + ex.Message;
            WriteLog("UpdateSFCPacking", "SQL執行錯誤: " + ex.ToString());
        }

        WriteLog("UpdateSFCPacking", "End.");
        return false;
    }
    [WebMethod]
    public string GetVehicleNo(string factoryID, string DNNo, out string vehicleno)
    {
        WriteLog("GetVehicleNo", "Start.");
        vehicleno = "";
        string sql = @"select top 1 M.VehicleNo from tblDPCBillOfLadingMaster M  
                       left join tblDPCBillOfLadingDetail D on D.FactoryID=M.FactoryID and D.ShipmentNumber=M.ShipmentNumber 
                       where D.FactoryID=@factoryID and D.DNNo=@DNNo";

        using (SqlConnection lConn = new SqlConnection(Util.getConnStr()))
        {
            try
            {
                lConn.Open();
                SqlCommand lCmd = new SqlCommand(sql, lConn);
                lCmd.Parameters.AddWithValue("@FactoryID", factoryID);
                lCmd.Parameters.AddWithValue("@DNNo", DNNo);
                SqlDataReader dr = lCmd.ExecuteReader();
                while (dr.Read())
                {
                    vehicleno = dr["VehicleNo"].ToString();
                }
            }
            catch (Exception ex)
            {
                WriteLog("GetVehicleNo", "Error: " + ex.ToString());
                throw new Exception("車輛信息不存在！"+ex.Message);
            }
            finally
            {
                if (lConn.State == ConnectionState.Open)
                    lConn.Close();
            }
        }
        WriteLog("GetVehicleNo", "End.");
        return vehicleno;
    }

    private void WriteLog(string method, string message)
    {
        string filePath = HttpContext.Current.Request.PhysicalApplicationPath +
                "\\Log\\" + DateTime.Today.ToString("yyyyMM") +
                "\\";

        if (!Directory.Exists(filePath))
        {
            (new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, filePath)).Demand();
            Directory.CreateDirectory(filePath);
        }

        using (FileStream fileStream = File.Open(filePath + DateTime.Today.ToString("yyyyMMdd") + "DataExchangeService.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.WriteLine("LogDateTime : " + DateTime.Now.ToString());
                writer.WriteLine("Method : " + method);
                writer.WriteLine("LogMessage");
                writer.WriteLine(message);
                writer.WriteLine("----------------------------------------------------------------------------------------");
            }
        }
    }
}

