using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
[Serializable]
public class bl_sys_user_role
{

	public bl_sys_user_role()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string UserName { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string ObjectId { get; set; }
    public string ObjectCode { get; set; }
    public string ObjectName { get; set; }
    public Int32 IsView { get; set; }
    public Int32 IsAdd { get; set; }
    public Int32 IsUpdate { get; set; }
    public Int32 IsApprove { get; set; }
    public Int32 IsAdmin { get; set; }


    public bool AddSysUserRole(string UserName, string RoleId)
    {
        bool result = false;
        DB db = new DB();
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USERS_ROLE_ADD", new string[,] { 
            {"@role_id", RoleId},
            {"@user_name",UserName}
          
            }, "bl_sys_user_role => AddSysUserRole(string UserName, string RoleId)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [AddSysUserRole(string UserName, string RoleId)] in class [bl_sys_user_role], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_user_role=>AddSysUserRole(string UserName, string RoleId)");
        }
        return result;
    }
    public bool DeleteSysUserRole(string UserName)
    {
        bool result = false;
        DB db = new DB();

        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USERS_ROLE_DELETE", new string[,] { 
           
            {"@user_name",UserName}
          
            }, "bl_sys_user_role => DeleteSysUserRole(string UserName)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [DeleteSysUserRole(string UserName)] in class [bl_sys_user_role], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_user_role=>DeleteSysUserRole(string UserName)");
        }
        return result;
    }
  
    public List<bl_sys_user_role> GetSysUserRole(string UserName)
    {
        DB db = new DB();
        List<bl_sys_user_role> Lobj = new List<bl_sys_user_role>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USERS_ROLE_GET_BY_USER", new string[,] {
            {"@user_name", UserName}
            }, "bl_sys_user_role=>GetSysUserRole(string UserName)");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_user_role()
                {
                    UserName = r["user_name"].ToString(),
                    RoleId = r["role_id"].ToString(),
                    RoleName = r["role_name"].ToString(),
                    ObjectId = r["obj_id"].ToString(),
                    ObjectCode = r["obj_code"].ToString(),
                    ObjectName = r["obj_name"].ToString(),
                    IsView = Convert.ToInt32(r["is_view"].ToString()),
                    IsAdd = Convert.ToInt32(r["is_add"].ToString()),
                    IsUpdate = Convert.ToInt32(r["is_update"].ToString()),
                    IsApprove = Convert.ToInt32(r["is_approve"].ToString()),
                    IsAdmin = Convert.ToInt32(r["is_admin"].ToString())
                });
            }
          
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_sys_user_role>();
            Log.AddExceptionToLog("Error function [GetSysUserRole(string UserName)] in class [bl_sys_user_role], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_user_role=>GetSysUserRole(string UserName)");
        }
        return Lobj;
    }
 
    public List<bl_sys_user_role> GetSysRole(string UserName)
    {
        DB db = new DB();
        List<bl_sys_user_role> Lobj = new List<bl_sys_user_role>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USERS_ROLE_GET_ROLE_BY_USER", new string[,] {
            {"@user_name", UserName}
            }, "bl_sys_user_role=>GetSysRole(string UserName)");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_user_role()
                {
                    UserName = r["user_name"].ToString(),
                    RoleId = r["role_id"].ToString(),
                    RoleName = r["role_name"].ToString()
                  
                });
            }

        }
        catch (Exception ex)
        {
            Lobj = new List<bl_sys_user_role>();
            Log.AddExceptionToLog("Error function [GetSysRole(string UserName)] in class [bl_sys_user_role], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_user_role=>GetSysRole(string UserName)");
        }
        return Lobj;
    }

    /// <summary>
    /// Get system user role by object code
    /// </summary>
    /// <param name="UserRole">List of user role</param>
    /// <param name="ObjectCode"></param>
    /// <returns></returns>
    /// 

    public bl_sys_user_role GetSysUserRole(List<bl_sys_user_role> UserRole, string ObjectCode)
    {
      
        bl_sys_user_role obj = new bl_sys_user_role();
        try
        {
            foreach (bl_sys_user_role role in UserRole.Where(_ => _.ObjectCode ==ObjectCode))
            {
                obj = role;
                break;
            }
        }
        catch (Exception ex)
        {
            obj = new bl_sys_user_role();
            Log.AddExceptionToLog("Error function [GetSysUserRole(List<bl_sys_user_role> UserRole, string ObjectCode)] in class [bl_sys_user_role], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_user_role=>GetSysUserRole(List<bl_sys_user_role> UserRole, string ObjectCode)");

        }
        return obj;
    }
}