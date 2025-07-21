using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for bl_role
/// </summary>
public class bl_role
{
    private string app_name = "";
	public bl_role()
	{
		//
		// TODO: Add constructor logic here
		//

        if (ApplicationID != null && ApplicationID != "")
        {
            app_name = getAppName(ApplicationID);
        }
        else
        {
            app_name = "";
        }
	}
    public string ApplicationID { get; set; }
    public string ApplicationName {
        get {
            return "";
        }
    }
    public string RoleID { get; set; }
    public string RoleName { get; set; }
    public string LoweredRoleName {
        get {
            return RoleName.ToLower().Trim();
        }
    }
    public string Description { get; set; }

    private string getAppName(string application_id)
    {
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetAccountConnectionString(), "select ApplicationName from aspnet_Applications where ApplicationId='" + application_id + "';");
            foreach (DataRow row in tbl.Rows)
            {
                app_name = row["applicationName"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            app_name = "";
            Log.AddExceptionToLog("Error local function [getAppName(string application_id)] in class [bl_role]. Detail: " + ex.Message);
        }

        return app_name;
    }
}