using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_customer_lead_status_history
/// </summary>
public class da_customer_lead_status_history
{
    public static bool SUCCESS;
    public static string MESSAGE;
	public da_customer_lead_status_history()
	{
		//
		// TODO: Add constructor logic here
		//
        SUCCESS = false;
        MESSAGE = "";
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">Lead status history object</param>
    /// <returns></returns>
    public static bool InsertLeadStatus(bl_customer_lead_status_history obj)
    {
       
        bool result = false;
        try
        {
            DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_LEAD_STATUS_HISTORY_INSERT", new string[,] {
            {"@ID",obj.ID},
            {"@LEAD_ID", obj.LeadID},
	        {"@STATUS", obj.Status},
            {"@STATUS_REMARKS", obj.StatusRemarks},
	        {"@Created_By", obj.CreatedBy} ,
	        {"@Created_On", obj.CreatedOn+""} 
            }, "da_customer_lead ==> InsertLeadStatus(bl_customer_lead_status_history obj)");
            
            SUCCESS = result;
            MESSAGE = db.Message;

        }
        catch (Exception ex)
        {
            MESSAGE = ex.Message;
            SUCCESS = false;
            Log.AddExceptionToLog("Error function [InsertCustomerLead(bl_customer_lead obj_customer_lead)] in class [da_customer_lead_status_history], detail: " + ex.Message + " ==> " + ex.StackTrace);
        }

        return SUCCESS;

    }

}