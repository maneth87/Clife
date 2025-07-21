using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for bl_sys_roles_access
/// </summary>
public class bl_sys_roles_access
{
    DB db = new DB();

	public bl_sys_roles_access()
	{
		//
		// TODO: Add constructor logic here
		//
      
	}
    public string RoleAccessId { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string ObjectId{get;set;}
    public string ObjectName { get; set; }
    public string ObjectCode { get; set; }
    public string Module { get; set; }
    public Int32 IsView { get; set; }
    public Int32 IsAdd { get; set; }
    public Int32 IsUpdate { get; set; }
    public Int32 IsApprove { get; set; }
    public Int32 IsAdmin { get; set; }


 /// <summary>
 /// Add new system role access
 /// </summary>
 /// <param name="RoleId"></param>
 /// <param name="ObjectId"></param>
 /// <param name="IsView"></param>
 /// <param name="IsAdd"></param>
 /// <param name="IsUpdate"></param>
 /// <param name="IsApprove"></param>
 /// <param name="IsAdmin"></param>
 /// <returns></returns>
    public bool AddSysRolesAccess( string RoleId, string ObjectId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)
    {
        bool result = false;
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ACCESS_ADD", new string[,] { 
            {"@role_id", RoleId},
            {"@obj_id",ObjectId},
            {"@is_view", IsView ==true ? "1" : "0"},
            {"@is_add", IsAdd==true ? "1" : "0"},
            {"@is_update",IsUpdate==true ? "1" : "0"},
            {"@is_approve", IsApprove==true ? "1" : "0"},
            {"@is_admin", IsAdmin==true ? "1" : "0"}
            }, "bl_sys_roles_access => AddSysRolesAccess( string RoleId, string ObjectId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [AddSysRolesAccess( string RoleId, string ObjectId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>AddSysRolesAccess( string RoleId, string ObjectId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)");
        }
        return result;
    }
    /// <summary>
    /// Update existing system role access
    /// </summary>
    /// <param name="RoleAccessId"></param>
    /// <param name="IsView"></param>
    /// <param name="IsAdd"></param>
    /// <param name="IsUpdate"></param>
    /// <param name="IsApprove"></param>
    /// <param name="IsAdmin"></param>
    /// <returns></returns>
    public bool UpdateSysRolesAccess(string RoleAccessId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)
    {
        bool result = false;
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ACCESS_UPDATE", new string[,] { 
            {"@role_access_id", RoleAccessId},
            {"@obj_id",ObjectId},
            {"@is_view", IsView+""},
            {"@is_add", IsAdd+""},
            {"@is_update",IsUpdate+""},
            {"@is_approve", IsApprove+""},
            {"@is_admin", IsAdmin+""}
            }, "bl_sys_roles_access => AUpdateSysRolesAccess(string RoleAccessId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateSysRolesAccess(string RoleAccessId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>UpdateSysRolesAccess(string RoleAccessId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)");
        }
        return result;
    }

    public List<bl_sys_roles_access> GetSysRolesAccess()
    {
        List<bl_sys_roles_access> Lobj = new List<bl_sys_roles_access>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ACCESS_GET_ALL", new string[,] { }, "bl_sys_roles_access=>GetSysRolesAccess()");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_roles_access()
                {
                    RoleAccessId = r["id"].ToString(),
                    RoleId = r["role_id"].ToString(),
                    RoleName = r["role_name"].ToString(),
                    ObjectId = r["obj_id"].ToString(),
                    ObjectCode = r["obj_code"].ToString(),
                    ObjectName = r["obj_name"].ToString(),
                    Module=r["module"].ToString(),
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
            Lobj = new List<bl_sys_roles_access>();
            Log.AddExceptionToLog("Error function [GetSysRolesAccess()] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>GetSysRolesAccess()");
        }

        return Lobj;
    }
    /// <summary>
    /// Get system role access by role id
    /// </summary>
    /// <param name="RoleId"></param>
    /// <returns></returns>
    public List<bl_sys_roles_access> GetSysRolesAccess(string RoleId)
    {
        List<bl_sys_roles_access> Lobj = new List<bl_sys_roles_access>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ACCESS_GET_BY_ROLE", new string[,] {
            {"@role_id", RoleId}
            }, "bl_sys_roles_access=>GetSysRolesAccess(string RoleId)");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_roles_access()
                {
                    RoleAccessId = r["id"].ToString(),
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
            Lobj = new List<bl_sys_roles_access>();
            Log.AddExceptionToLog("Error function [GetSystObjects(string RoleId] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>GetSysRolesAccess(string RoleId)");
        }

        return Lobj;
    }
    public List<bl_sys_roles_access> GetSysRolesAccess(string RoleId, string RoleName, string Module)
    {
        List<bl_sys_roles_access> Lobj = new List<bl_sys_roles_access>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ACCESS_GET_BY_COND", new string[,] {
            {"@role_id",  "%" + RoleId+"%"},{"@module",  "%"+ Module+"%" }, {"@role_name", "%"+RoleName+"%"}
            }, "bl_sys_roles_access=>GetSysRolesAccess(string RoleId, string Module)");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_roles_access()
                {
                    RoleAccessId = r["id"].ToString(),
                    RoleId = r["role_id"].ToString(),
                    RoleName = r["role_name"].ToString(),
                    ObjectId = r["obj_id"].ToString(),
                    ObjectCode = r["obj_code"].ToString(),
                    ObjectName = r["obj_name"].ToString(),
                    Module = r["Module"].ToString(),
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
            Lobj = new List<bl_sys_roles_access>();
            Log.AddExceptionToLog("Error function [GetSystObjects(string RoleId, string Module] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>GetSysRolesAccess(string RoleId, string Module)");
        }

        return Lobj;
    }
    /// <summary>
    /// Delete System Roles Access by Role Id and Module
    /// </summary>
    /// <param name="RoleId"></param>
    /// <param name="Module"></param>
    /// <returns></returns>
    public bool DeleteSysRolesAccess(string RoleId, string Module)
    {
        bool result = false;
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ACCESS_DELETE_BY_MODULE", new string[,] { 
            {"@role_id", RoleId},
            {"@module",Module},
           
            }, "bl_sys_roles_access => DeleteSysRolesAccess(string RoleId, string Module)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [DeleteSysRolesAccess(string RoleId, string Module)] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>DeleteSysRolesAccess(string RoleId, string Module)");
        }
        return result;
    }
}