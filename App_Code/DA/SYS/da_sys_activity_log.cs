using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_sys_activity_log
/// </summary>
public class da_sys_activity_log
{
    public da_sys_activity_log()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// return message 
    /// </summary>
    public static string Message { get { return _message; } }
    /// <summary>
    /// Return transaction status 
    /// </summary>
    public static bool Transaction { get { return _tran; } }

    public static void Save(bl_sys_activity_log objLog)
    {
        try
        {
            db = new DB();
            _tran = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ACTIVITY_USER_LOG_INSERT", new string[,] {
        {"@user_name", objLog.UserName},
        {"@obj_id", objLog.ObjectId},
        {"@activity_type", objLog.ActivityType.ToString()},
        {"@activity_date",objLog.ActivityDate+""},
        {"@description", objLog.Description},
        {"@application_id", objLog.ApplicationId}
        }, "da_sys_activity_log.Save(bl_sys_activity_log objLog)");

            _message = _tran == true ? "Transaction successfully." : "Transaction fail: " + db.Message;
        }
        catch (Exception ex)
        {
            _tran = false;
            _message = ex.Message;
            Log.AddExceptionToLog("Error function [Save(bl_sys_activity_log objLog)] in class [da_sys_activity_log], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
    }

    public static List<bl_sys_activity_log> GetList(DateTime activityDateFrom, DateTime activityDateTo, string userName)
    {
        List<bl_sys_activity_log> lActivity = new List<bl_sys_activity_log>();
        try
        {
            DataTable tbl;
            db = new DB();
            if (userName != "")
            {
                 tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ACTIVITY_LOG_GET_BY_D_USER", new string[,] {
        {"@user_name", userName},
        {"@ACTIVITY_DATE_F",activityDateFrom+""},
        {"@ACTIVITY_DATE_T", activityDateTo+""}
        }, "da_sys_activity_log.GetList(DateTime activityDateFrom, DateTime activityDateTo, string userName)");
            }
            else
            {
                tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ACTIVITY_LOG_GET", new string[,] {
        {"@ACTIVITY_DATE_F",activityDateFrom+""},
        {"@ACTIVITY_DATE_T", activityDateTo+""}
        }, "da_sys_activity_log.GetList(DateTime activityDateFrom, DateTime activityDateTo, string userName)");
            }

            if (db.RowEffect > 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE type;

                    if (Enum.TryParse(r["ACTIVITY_TYPE"].ToString(), out type))
                    {

                    }

                    lActivity.Add(new bl_sys_activity_log()
                    {
                        UserName = r["user_name"].ToString(),
                        ObjectId = r["obj_CODE"].ToString(),
                        ObjectName = r["obj_name"].ToString(),
                       // ActivityType = (bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE)Enum.Parse(typeof(bl_sys_activity_log.ACTIVITY_LOG_TYPE), r["ACTIVITY_TYPE"].ToString(), true),
                       ActivityType = type,
                        ActivityDate = Convert.ToDateTime(r["activity_date"].ToString()),
                        Description = r["description"].ToString(),
                        ApplicationId = r["application_id"].ToString(),
                        ApplicationName = r["application_name"].ToString()
                    });
                }
                _tran = true;
                _message = string.Concat(tbl.Rows.Count, " record(s) found.");
            }
            else if (db.RowEffect == 0)
            {
                _tran = true;
                _message = "No record found.";
            }
            else
            {
                _tran = false;
                _message = "Transaction fail: " + db.Message;
            }
        }
        catch (Exception ex)
        {
            lActivity = null;
            _tran = false;
            _message = ex.Message;
            Log.AddExceptionToLog("Error function [GetList(DateTime activityDateFrom, DateTime activityDateTo, string userName)] in class [da_sys_activity_log], detail:" + ex.Message + "=>" + ex.StackTrace);
        }

        return lActivity;
    }

    #region private function & variables
    private static string _message = "";
    private static bool _tran = false;
    private static DB db = null;
    #endregion
}