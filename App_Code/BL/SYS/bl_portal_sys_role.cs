using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for bl_portal_sys_role
/// </summary>
public class bl_portal_sys_role
{
	public bl_portal_sys_role()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public bl_portal_sys_role(string connectionString)
    {
        _connectionString = connectionString;
    }
    public bl_portal_sys_role(int RoleId, string RoleName, Int32 IsActive, Int32 ApplicationId)
    {
        this.RoleId = RoleId;
        this.RoleName = RoleName;
        this.IsActive = IsActive;
        this.ApplicationId = ApplicationId;
    }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public Int32 IsActive { get; set; }
    public Int32 ApplicationId { get; set; }
    public string ApplicationName { get { return _applicationName; } }

    private string _applicationName = "";
    private string _connectionString = "";

    MySqlDB db = new MySqlDB();
 /// <summary>
 /// Get roles by application name
 /// </summary>
 /// <param name="appName"></param>
 /// <returns></returns>
    public List<bl_portal_sys_role> GetSysRoles(string applicationName)
    {
        List<bl_portal_sys_role> Lroles = new List<bl_portal_sys_role>();
        try
        {
            db.ConnectionString =  AppConfiguration.GetCamlifePortalConnectionString();
            db.Parameters = new string[,] { { "v_app_name", applicationName } };
            db.ProcedureName = "SP_my_aspnet_roles_get_all";
            db.GenerateData();
        
            if (db.GenerateDataStatus == true)
            {
                if (db.DataCount > 0)
                {
                    _applicationName = applicationName;
                    foreach (DataRow r in db.Data.Rows)
                    {
                        Lroles.Add(new bl_portal_sys_role()
                        {
                            RoleId = Convert.ToInt32(r["id"].ToString()),
                            RoleName = r["role_name"].ToString(),
                          //  IsActive = Convert.ToInt32(r["is_active"].ToString()),
                           ApplicationId =Convert.ToInt32(r["application_id"].ToString())
                        });
                       
                    }
                }
                else
                {
                    Lroles = new List<bl_portal_sys_role>();
                }

            }
          
        }
        catch (Exception ex)
        {
            Lroles = new List<bl_portal_sys_role>();
            Log.AddExceptionToLog("Error function [GetSysRoles(string applicationName)] in class [bl_portal_sys_role], detail: " + ex.Message);
        }
        return Lroles;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="RoleName"></param>
    /// <returns></returns>
    public List<bl_portal_sys_role> GetSysRoles(string ApplicationName, string RoleName)
    {
        List<bl_portal_sys_role> Lroles = new List<bl_portal_sys_role>();
        try
        {
            db.ConnectionString = _connectionString;// AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "v_app_name", ApplicationName }, { "v_role_name", '%' + RoleName + '%' } };
            db.ProcedureName = "SP_my_aspnet_roles_get_by_role_name";
            db.GenerateData();
         
            if (db.GenerateDataStatus == true)
            {
                if (db.DataCount > 0)
                {
                    _applicationName = ApplicationName;
                    foreach (DataRow r in db.Data.Rows)
                    {
                        Lroles.Add(new bl_portal_sys_role()
                        {
                            RoleId = Convert.ToInt32(r["id"].ToString()),
                            RoleName = r["role_name"].ToString(),
                           // IsActive = Convert.ToInt32(r["is_active"].ToString()),
                            ApplicationId = Convert.ToInt32(r["application_id"].ToString())
                        });
                    }
                }
                else
                {
                    Lroles = new List<bl_portal_sys_role>();
                }

            }
        }
        catch (Exception ex)
        {
            Lroles = new List<bl_portal_sys_role>();
            Log.AddExceptionToLog("Error function [GetSysRoles(string ApplicationName, string RoleName)] in class [bl_portal_sys_role], detail: " + ex.Message);
        }
        return Lroles;
    }

    public bl_portal_sys_role GetSysRoles(string ApplicationName, int RoleId)
    {
        bl_portal_sys_role Lroles = new bl_portal_sys_role();
        try
        {
            db.ConnectionString = _connectionString;// AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "v_app_name", ApplicationName }, { "i_role_id", RoleId + "" } };
            db.ProcedureName = "SP_my_aspnet_roles_get_by_role_id";
            db.GenerateData();

            if (db.GenerateDataStatus == true)
            {
                if (db.DataCount > 0)
                {
                    _applicationName = ApplicationName;
                    foreach (DataRow r in db.Data.Rows)
                    {
                        Lroles=new bl_portal_sys_role()
                        {
                            RoleId = Convert.ToInt32(r["id"].ToString()),
                            RoleName = r["role_name"].ToString(),
                            ApplicationId = Convert.ToInt32(r["application_id"].ToString())
                        };
                    }
                }
                else
                {
                    Lroles = new bl_portal_sys_role();
                }

            }
        }
        catch (Exception ex)
        {
            Lroles = new bl_portal_sys_role();
            Log.AddExceptionToLog("Error function [GetSysRoles(string ApplicationName, int RoleId)] in class [bl_portal_sys_role], detail: " + ex.Message);
        }
        return Lroles;
    }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="SysRole"></param>
   /// <returns></returns>
    public bool AddRole(bl_portal_sys_role SysRole)
    {
        bool result = false;
        try {


            db.ConnectionString = _connectionString;// AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "i_application_id", SysRole.ApplicationId + "" }, { "v_role_name", SysRole.RoleName} };

            db.ProcedureName = "SP_my_aspnet_roles_insert";
            db.Execute();
            result = db.ExecuteStatus;
            

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [AddRole(bl_portal_sys_role SysRole)] in class [bl_portal_sys_role], detail: " + ex.Message );
        }
        return result;
    }

    /// <summary>
    /// UPDATE EXISTING SYSTEM ROLE
    /// </summary>
    /// <param name="SysRole"></param>
    /// <returns></returns>
    public bool UpdateRole(int RoleId, string NewRoleName)
    {
        bool result = false;
        try
        {
            db.ConnectionString = _connectionString;// AppConfiguration.GetCamlifeMidleWareConnectionString();
            db.Parameters = new string[,] { { "i_role_id", RoleId + "" }, { "v_role_name", NewRoleName } };
            db.ProcedureName = "SP_my_aspnet_roles_update";
            db.Execute();
            result = db.ExecuteStatus;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateRole(int RoleId, string NewRoleName)] in class [bl_portal_sys_role], detail: " + ex.Message);
        }
        return result;
    }
}