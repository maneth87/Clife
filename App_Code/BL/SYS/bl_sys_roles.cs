using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for bl_sys_roles
/// </summary>
public class bl_sys_roles
{
    public bl_sys_roles()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bl_sys_roles( string RoleId, string RoleName,Int32 IsActive)
    {
        this.RoleId = RoleId;
        this.RoleName = RoleName;
        this.IsActive = IsActive;
    
    }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public Int32 IsActive { get; set; }

    DB db = new DB();
    /// <summary>
    /// GET ALL SYSTEM ROLES
    /// </summary>
    /// <returns></returns>
    public List<bl_sys_roles> GetSysRoles()
    {
        List<bl_sys_roles> Lroles = new List<bl_sys_roles>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_GET_ALL", new string[,] { }, "bl_sys_roles => GetSysRoles()");
            foreach (DataRow r in tbl.Rows)
            {
                Lroles.Add(new bl_sys_roles()
                {
                    RoleId = r["role_id"].ToString(),
                    RoleName = r["role_name"].ToString(),
                    IsActive = Convert.ToInt32(r["is_active"].ToString())
                });
            }
        }
        catch (Exception ex)
        {
            Lroles = new List<bl_sys_roles>();
            Log.AddExceptionToLog("Error function [GetSysRoles] in class [bl_sys_roles], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles => GetSysRoles()");
        }
        return Lroles;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="RoleName"></param>
    /// <returns></returns>
    public List<bl_sys_roles> GetSysRoles(string RoleName)
    {
        List<bl_sys_roles> Lroles = new List<bl_sys_roles>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_GET_BY_ROLE_NAME", new string[,] {
            {"@ROLE_NAME", "%" +RoleName+"%"}
            }, "bl_sys_roles => GetSysRoles(string RoleName)");
            foreach (DataRow r in tbl.Rows)
            {
                Lroles.Add(new bl_sys_roles()
                {
                    RoleId = r["role_id"].ToString(),
                    RoleName = r["role_name"].ToString(),
                    IsActive = Convert.ToInt32(r["is_active"].ToString())
                });
            }
        }
        catch (Exception ex)
        {
            Lroles = new List<bl_sys_roles>();
            Log.AddExceptionToLog("Error function [GetSysRoles(string RoleName)] in class [bl_sys_roles], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles => GetSysRoles(string RoleName)");
        }
        return Lroles;
    }
    /// <summary>
    /// ADD NEW SYSTEM ROLE
    /// </summary>
    /// <param name="SysRole"></param>
    /// <returns></returns>
    public bool AddRole(bl_sys_roles SysRole)
    {
        bool result = false;
        try {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ADD", new string[,] { 
            {"@role_id", SysRole.RoleId},
            {"@role_name",SysRole.RoleName},
            {"@is_active",SysRole.IsActive+""}
            }, "bl_sys_roles => AddRole(bl_sys_roles SysRole)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [AddRole(bl_sys_roles SysRole)] in class [bl_sys_roles], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles => AddRole(bl_sys_roles SysRole)");
        }
        return result;
    }
    /// <summary>
    /// UPDATE EXISTING SYSTEM ROLE
    /// </summary>
    /// <param name="SysRole"></param>
    /// <returns></returns>
    public bool UpdateRole(bl_sys_roles SysRole)
    {
        bool result = false;
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_UPDATE", new string[,] { 
            {"@role_id", SysRole.RoleId},
            {"@role_name",SysRole.RoleName},
            {"@is_active",SysRole.IsActive+""}
            }, "bl_sys_roles => UpdateRole(bl_sys_roles SysRole)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateRole(bl_sys_roles SysRole)] in class [bl_sys_roles], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles => UpdateRole(bl_sys_roles SysRole)");
        }
        return result;
    }
}