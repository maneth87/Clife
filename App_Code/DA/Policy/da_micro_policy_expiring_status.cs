using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// Summary description for da_micro_policy_expiring_status
/// </summary>
public class da_micro_policy_expiring_status
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
    private static DB db = new DB();
    /// <summary>
    /// Class Name
    /// </summary>
    private static string MYNAME = "da_micro_policy_expiring_status";
	public da_micro_policy_expiring_status()
	{
		//
		// TODO: Add constructor logic here
		//

	}
    public static bool Save(bl_micro_policy_expiring_status polStatus, string userName="")
    {
        try
        {
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_STATUS_INSERT", new string[,] { 
            {"@policy_id", polStatus.PolicyId},
            {"@status", polStatus.Status},
            {"@remarks", polStatus.Remarks},
            {"@created_by", polStatus.CreatedBy},
            {"@created_on", polStatus.CreatedOn+""}
            }, MYNAME+ " => Save(bl_micro_policy_expiring_status polStatus, string userName)");

            _MESSAGE = _SUCCESS == true ? "Update status successfully." : "Update status fail.";
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE ="Update status fail, detail:" + ex.Message;
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            }); 
        }
        return _SUCCESS;
    }
    public static bool SaveHistory(bl_micro_policy_expiring_status polStatus, string userName = "")
    {
        try
        {
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_STATUS_HIS_INSERT", new string[,] { 
            {"@policy_id", polStatus.PolicyId},
            {"@status", polStatus.Status},
            {"@remarks", polStatus.Remarks},
            {"@created_by", polStatus.CreatedBy},
            {"@created_on", polStatus.CreatedOn+""}
            }, MYNAME + " => SaveHistory(bl_micro_policy_expiring_status polStatus, string userName)");

            _MESSAGE = _SUCCESS == true ? "Update status successfully." : "Update status fail.";
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Update status fail, detail:" + ex.Message;
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return _SUCCESS;
    }
    public static bool Update(bl_micro_policy_expiring_status polStatus, string userName = "")
    {
        try
        {
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_STATUS_UPDATE", new string[,] { 
            {"@policy_id", polStatus.PolicyId},
            {"@status", polStatus.Status},
            {"@remarks", polStatus.Remarks},
            {"@updated_by", polStatus.UpdatedBy},
            {"@updated_on", polStatus.UpdatedOn+""}
            }, MYNAME + " => Update(bl_micro_policy_expiring_status polStatus, string userName)");
            _MESSAGE = _SUCCESS == true ? "Update status successfully." : "Update status fail.";
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Update status fail, detail:" + ex.Message;
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return _SUCCESS;
    }
    public static bool UpdateHistory(bl_micro_policy_expiring_status polStatus, string userName = "")
    {
        try
        {
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_STATUS_HIS_UPDATE", new string[,] { 
            {"@policy_id", polStatus.PolicyId},
            {"@status", polStatus.Status},
            {"@remarks", polStatus.Remarks},
            {"@updated_by", polStatus.UpdatedBy},
            {"@updated_on", polStatus.UpdatedOn+""}
            }, MYNAME + " => UpdateHistory(bl_micro_policy_expiring_status polStatus, string userName)");
            _MESSAGE = _SUCCESS == true ? "Update status successfully." : "Update status fail.";
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Update status fail, detail:" + ex.Message;
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return _SUCCESS;
    }
}