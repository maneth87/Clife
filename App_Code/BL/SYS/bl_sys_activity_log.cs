using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// Summary description for bl_sys_activity_log
/// </summary>
/// 
[Serializable]
public class bl_sys_activity_log
{
	public bl_sys_activity_log()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public bl_sys_activity_log(string userName, string objId, ACTIVITY_LOG_TYPE.TYPE activityType, DateTime activityDate, string description, string applicationName) {
       
        ObjectId = objId;
        UserName = userName;
        ActivityType = activityType;
        ActivityDate = activityDate;
        Description = description;
        ApplicationName = applicationName;
        ApplicationId = _getApplicationID();
    }
    public string LogId { get { return _generateId(); } }
    public string UserName { get; set; }
    public string ObjectId { get; set; }
    public string ObjectName { get; set; }
    public ACTIVITY_LOG_TYPE.TYPE ActivityType { get; set; }
    public DateTime ActivityDate { get; set; }
    public string Description { get; set; }
    public string ApplicationId { get; set; }
    public string ApplicationName { get; set; }

   


   
    #region Private function & variables
  
    
    private string _generateId() {
        
      return  Helper.GetNewGuid(new string[,] { { "TABLE", "TBL_SYS_ACTIVITY_LOG" }, { "FIELD", "ACTIVITY_LOG_ID" } });
    
    }

    private string _getApplicationID()
    {
        if (ApplicationName != null)
        {
            try {
                DB db = new DB();
                System.Data.DataTable tbl = db.GetData(AppConfiguration.GetAccountConnectionString(), "SP_GET_APPLICATION_ID", new string[,] { 
                {"@application_name",ApplicationName}
                }, "bl_sy_activity_log._getApplicationID()");
                if (db.RowEffect <= 0)
                {
                    return "";
                }
                else {
                    return tbl.Rows[0]["applicationId"].ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }
    #endregion

    public class ACTIVITY_LOG_TYPE
    {
        public enum TYPE { SAVE, UPDATE, DELETE, GENERATE, EXPORT, INQUIRY,VIEW, PRINT, LOGIN, LOGOUT,CANCEL,RENEW,UPLOAD }
    }
}