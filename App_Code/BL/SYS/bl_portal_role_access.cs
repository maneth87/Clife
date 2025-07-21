using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for bl_portal_role_access
/// </summary>
public class bl_portal_role_access
{
	public bl_portal_role_access()
	{
		//
		// TODO: Add constructor logic here
		//
        initial();
	}
    public string RoleAccessId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get { return _roleName(); } }
    public int ObjectId { get; set; }
    public string ObjectName { get { return _objectName; } }
    public string ObjectCode { get { return _objectCode; } }
    public string Module { get { return _module; } }
    public bool IsView { get; set; }
    public bool IsAdd { get; set; }
    public bool IsUpdate { get; set; }
    public bool IsDelete { get; set; }
    public bool IsAdmin { get; set; }

    private string _roleName() {
        bl_portal_sys_role objRole = new bl_portal_sys_role();
        if (RoleId > 0)
        {
            bl_portal_sys_role objR = objRole.GetSysRoles(MYSQL_MEMBERSHIP.ApplicationName, RoleId);

            bl_portal_sys_object objSys = new bl_portal_sys_object();

            if (ObjectId > 0)
            {
                bl_portal_sys_object obj = objSys.GetSystObjects(ObjectId);
                _objectCode = obj.ObjCode;
                _objectName = obj.ObjName;
                _module = obj.Module;
            }

            return objR.RoleName;
        }
        else
        {
            return "";
        }
    }
    private string _objectName = "";
    private string _objectCode = "";
    private string _module = "";

    #region private function
    private void initial()
    {
       
        bl_portal_sys_object objSys = new bl_portal_sys_object();
       
        if (ObjectId >0)
        {
            bl_portal_sys_object obj = objSys.GetSystObjects(ObjectId);
            _objectCode = obj.ObjCode;
            _objectName = obj.ObjName;
            _module = obj.Module;
        }
       
    }
    #endregion

    MySqlDB db;
  /// <summary>
  /// 
  /// </summary>
  /// <param name="Obj"></param>
  /// <returns></returns>
    public bool AddSysRolesAccess(bl_portal_role_access Obj)
    {
        bool result = false;
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_insert";
            db.Parameters = new string[,] { 
             {"i_id", Obj.RoleId+""},
            {"i_roleid",Obj.RoleId+""},
            {"i_objectid",Obj.ObjectId+""},
            {"i_isview", Obj.IsView ==true ? "1" : "0"},
            {"i_isadd", Obj.IsAdd==true ? "1" : "0"},
            {"i_isupdate",Obj.IsUpdate==true ? "1" : "0"},
            {"i_isdelete", Obj.IsDelete==true ? "1" : "0"},
            {"i_isadmin", Obj.IsAdmin==true ? "1" : "0"}
            
            };
            db.Execute();
            result = db.ExecuteStatus;
           
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [AddSysRolesAccess(bl_portal_role_access Obj)] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>AddSysRolesAccess( string RoleId, string ObjectId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)");
        }
        return result;
    }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="Obj"></param>
  /// <returns></returns>
    public bool UpdateSysRolesAccess(bl_portal_role_access Obj)
    {
        bool result = false;
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_update";
            db.Parameters = new string[,] { 
             {"i_id", Obj.RoleId+""},
            {"i_roleid",Obj.RoleId+""},
            {"i_objectid",Obj.ObjectId+""},
            {"i_isview", Obj.IsView ==true ? "1" : "0"},
            {"i_isadd", Obj.IsAdd==true ? "1" : "0"},
            {"i_isupdate",Obj.IsUpdate==true ? "1" : "0"},
            {"i_isdelete", Obj.IsDelete==true ? "1" : "0"},
            {"i_isadmin", Obj.IsAdmin==true ? "1" : "0"}
            
            };
            db.Execute();
            result = db.ExecuteStatus;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateSysRolesAccess(bl_portal_role_access Obj)] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>UpdateSysRolesAccess(string RoleAccessId, bool IsView, bool IsAdd, bool IsUpdate, bool IsApprove, bool IsAdmin)");
        }
        return result;
    }

    public List<bl_portal_role_access> GetSysRolesAccess()
    {
        List<bl_portal_role_access> Lobj = new List<bl_portal_role_access>();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_get";
            db.Parameters = new string[,] { };
            db.GenerateData();
            if (db.GenerateDataStatus)
            {
                if (db.DataCount > 0)
                {
                    foreach (DataRow r in db.Data.Rows)
                    {
                        Lobj.Add(new bl_portal_role_access()
                        {
                            RoleAccessId = r["id"].ToString(),
                            RoleId = Convert.ToInt32( r["roleid"].ToString()),
                           // RoleName = r["role_name"].ToString(),
                            ObjectId = Convert.ToInt32(r["objectid"].ToString()),
                           // ObjectCode = r["CODE"].ToString(),
                          //  ObjectName = r["objectName"].ToString(),
                            IsView = Convert.ToInt32(r["isview"].ToString())==1?true:false,
                            IsAdd = Convert.ToInt32(r["isadd"].ToString()) == 1 ? true : false,
                            IsUpdate = Convert.ToInt32(r["isupdate"].ToString()) == 1 ? true : false,
                            IsDelete = Convert.ToInt32(r["isdelete"].ToString()) == 1 ? true : false,
                            IsAdmin = Convert.ToInt32(r["isadmin"].ToString()) == 1 ? true : false
                        });
                    }
                }
                else
                {
                    Lobj = new List<bl_portal_role_access>();
                }
            }
            else
            { Lobj = new List<bl_portal_role_access>(); }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_portal_role_access>();
            Log.AddExceptionToLog("Error function [GetSysRolesAccess()] in class [bl_portal_role_access], detail: " + ex.Message );
        }

        return Lobj;
    }
    /// <summary>
    /// Get system role access by role id
    /// </summary>
    /// <param name="RoleId"></param>
    /// <returns></returns>
    public List<bl_portal_role_access> GetSysRolesAccess(int RoleId)
    {
        List<bl_portal_role_access> Lobj = new List<bl_portal_role_access>();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_get_by_roleid";
            db.Parameters = new string[,] { { "i_roleid",RoleId+"" } };
            db.GenerateData();
            if (db.GenerateDataStatus)
            {
                if (db.DataCount > 0)
                {
                    foreach (DataRow r in db.Data.Rows)
                    {
                        Lobj.Add(new bl_portal_role_access()
                        {
                            RoleAccessId = r["id"].ToString(),
                            RoleId = Convert.ToInt32(r["roleid"].ToString()),
                            // RoleName = r["role_name"].ToString(),
                            ObjectId = Convert.ToInt32(r["objectid"].ToString()),
                            // ObjectCode = r["CODE"].ToString(),
                            //  ObjectName = r["objectName"].ToString(),
                            IsView = Convert.ToInt32(r["isview"].ToString()) == 1 ? true : false,
                            IsAdd = Convert.ToInt32(r["isadd"].ToString()) == 1 ? true : false,
                            IsUpdate = Convert.ToInt32(r["isupdate"].ToString()) == 1 ? true : false,
                            IsDelete = Convert.ToInt32(r["isdelete"].ToString()) == 1 ? true : false,
                            IsAdmin = Convert.ToInt32(r["isadmin"].ToString()) == 1 ? true : false
                        });
                    }
                }
                else
                {
                    Lobj = new List<bl_portal_role_access>();
                }
            }
            else
            { Lobj = new List<bl_portal_role_access>(); }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_portal_role_access>();
            Log.AddExceptionToLog("Error function [GetSystObjects(string RoleId)] in class [bl_portal_role_access], detail: " + ex.Message );
        }

        return Lobj;
    }
    public List<bl_portal_role_access> GetSysRolesAccess(int RoleId, string Module)
    {
        List<bl_portal_role_access> Lobj = new List<bl_portal_role_access>();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_get_by_roleid_module";
            db.Parameters = new string[,] { { "i_roleid", RoleId + "" } ,{"v_module", "%"+Module+"%"}};
            db.GenerateData();
            if (db.GenerateDataStatus)
            {
                if (db.DataCount > 0)
                {
                    foreach (DataRow r in db.Data.Rows)
                    {
                        Lobj.Add(new bl_portal_role_access()
                        {
                            RoleAccessId = r["id"].ToString(),
                            RoleId = Convert.ToInt32(r["roleid"].ToString()),
                            // RoleName = r["role_name"].ToString(),
                            ObjectId = Convert.ToInt32(r["objectid"].ToString()),
                            // ObjectCode = r["CODE"].ToString(),
                            //  ObjectName = r["objectName"].ToString(),
                            IsView = Convert.ToInt32(r["isview"].ToString()) == 1 ? true : false,
                            IsAdd = Convert.ToInt32(r["isadd"].ToString()) == 1 ? true : false,
                            IsUpdate = Convert.ToInt32(r["isupdate"].ToString()) == 1 ? true : false,
                            IsDelete = Convert.ToInt32(r["isdelete"].ToString()) == 1 ? true : false,
                            IsAdmin = Convert.ToInt32(r["isadmin"].ToString()) == 1 ? true : false
                        });
                    }
                }
                else
                {
                    Lobj = new List<bl_portal_role_access>();
                }
            }
            else
            { Lobj = new List<bl_portal_role_access>(); }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_portal_role_access>();
            Log.AddExceptionToLog("Error function [GetSysRolesAccess(int RoleId, string Module)] in class [bl_portal_role_access], detail: " + ex.Message);
        }

        return Lobj;
    }

    //public List<bl_sys_roles_access> GetSysRolesAccess(string RoleId, string RoleName, string Module)
    //{
    //    List<bl_sys_roles_access> Lobj = new List<bl_sys_roles_access>();
    //    try
    //    {
    //        DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_ROLES_ACCESS_GET_BY_COND", new string[,] {
    //        {"@role_id",  "%" + RoleId+"%"},{"@module",  "%"+ Module+"%" }, {"@role_name", "%"+RoleName+"%"}
    //        }, "bl_sys_roles_access=>GetSysRolesAccess(string RoleId, string Module)");
    //        foreach (DataRow r in tbl.Rows)
    //        {
    //            Lobj.Add(new bl_sys_roles_access()
    //            {
    //                RoleAccessId = r["id"].ToString(),
    //                RoleId = r["role_id"].ToString(),
    //                RoleName = r["role_name"].ToString(),
    //                ObjectId = r["obj_id"].ToString(),
    //                ObjectCode = r["obj_code"].ToString(),
    //                ObjectName = r["obj_name"].ToString(),
    //                Module = r["Module"].ToString(),
    //                IsView = Convert.ToInt32(r["is_view"].ToString()),
    //                IsAdd = Convert.ToInt32(r["is_add"].ToString()),
    //                IsUpdate = Convert.ToInt32(r["is_update"].ToString()),
    //                IsApprove = Convert.ToInt32(r["is_approve"].ToString()),
    //                IsAdmin = Convert.ToInt32(r["is_admin"].ToString())
    //            });
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Lobj = new List<bl_sys_roles_access>();
    //        Log.AddExceptionToLog("Error function [GetSystObjects(string RoleId, string Module] in class [bl_sys_roles_access], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_roles_access=>GetSysRolesAccess(string RoleId, string Module)");
    //    }

    //    return Lobj;
    //}


 /// <summary>
 /// 
 /// </summary>
 /// <param name="RoleId"></param>
 /// <returns></returns>
    public bool DeleteSysRolesAccess(int RoleId)
    {
        bool result = false;
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_delete_by_roleid";
            db.Parameters = new string[,] {    {"i_roleid", RoleId+""}       };
            db.Execute();
            result = db.ExecuteStatus;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [DeleteSysRolesAccess(int RoleId)] in class [bl_portal_role_access], detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="RoleId"></param>
    /// <param name="Module"></param>
    /// <returns></returns>
    public bool DeleteSysRolesAccess(int RoleId, string Module)
    {
        bool result = false;
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_delete_by_roleid_module";
            db.Parameters = new string[,] { { "i_roleid", RoleId + "" }, { "v_module", Module } };
            db.Execute();
            result = db.ExecuteStatus;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [DeleteSysRolesAccess(int RoleId, string Module)] in class [bl_portal_role_access], detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public bool DeleteSysRolesAccessById(int Id)
    {
        bool result = false;
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_objectinroles_delete_by_id";
            db.Parameters = new string[,] { { "i_id", Id + "" } };
            db.Execute();
            result = db.ExecuteStatus;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [DeleteSysRolesAccess(int Id)] in class [bl_portal_role_access], detail: " + ex.Message);
        }
        return result;
    }
}