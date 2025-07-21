using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for bl_user_access
/// </summary>
public class bl_sys_user_access
{
    public bl_sys_user_access()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Constructor for insert and update
    /// </summary>
    public bl_sys_user_access(string id, string userName, string roleId, string objId, int isView, int isAdd, int isUpdate, int isApprove, int isAdmin)
    {
        Id = id;
        UserName = userName;
        RoleId = roleId;
        ObjId = objId;
        IsView = isView;
        IsAdd = isAdd;
        IsUpdate = isUpdate;
        IsApprove = isApprove;
        IsAdmin = isAdmin;
    }
    public string Id { get; set; }
    public string UserName { get; set; }
    public string RoleId { get; set; }
    public string ObjId { get; set; }
    public Int32 IsView { get; set; }
    public Int32 IsAdd { get; set; }
    public Int32 IsUpdate { get; set; }
    public Int32 IsApprove { get; set; }
    public Int32 IsAdmin { get; set; }

    /// <summary>
    /// Display only
    /// </summary>
    public string Module { get; set; }
    /// <summary>
    /// Display only
    /// </summary>
    public string RoleName { get; set; }
    /// <summary>
    /// Display only
    /// </summary>
    public string ObjName { get; set; }
    /// <summary>
    /// Display only
    /// </summary>
    public string ObjCode{ get; set; }

    DB db;
    #region public function
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">Object user access</param>
    /// <returns></returns>
    public bool Save(bl_sys_user_access obj)
    {
        bool result = false;
        try
        {
            db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USER_ACCESS_INSERT", new string[,] {
                {"@ID", obj.Id}, {"@USER_NAME", obj.UserName},{"@ROLE_ID", obj.RoleId},{"@OBJ_ID", obj.ObjId}, {"@IS_VIEW", obj.IsView+""}, {"@IS_ADD", obj.IsAdd+""}, {"@IS_UPDATE", obj.IsUpdate+""},{"@IS_APPROVE", obj.IsApprove+""},{"@IS_ADMIN", obj.IsAdmin+""}
            }, "Save(bl_sys_user_access obj)=>bl_sys_user_access");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Erorr fuction [Save(bl_sys_user_access obj)] in class [bl_sys_user_access], detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Update user access by id
    /// </summary>
    /// <param name="obj">Object user access</param>
    /// <returns></returns>
    public bool Update(bl_sys_user_access obj)
    {
        bool result = false;
        try
        {
            db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USER_ACCESS_UPDATE", new string[,] {
                {"@ID", obj.Id}, {"@USER_NAME", obj.UserName},{"@ROLE_ID", obj.RoleId},{"@OBJ_ID", obj.ObjId}, {"@IS_VIEW", obj.IsView+""}, {"@IS_ADD", obj.IsAdd+""}, {"@IS_UPDATE", obj.IsUpdate+""},{"@IS_APPROVE", obj.IsApprove+""},{"@IS_ADMIN", obj.IsAdmin+""}
            }, "Update(bl_user_access obj)=>bl_user_access");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Erorr fuction [Update(bl_sys_user_access obj)] in class [bl_sys_user_access], detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Delete user access by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Delete(string id)
    {
        bool result = false;
        try
        {
            db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USER_ACCESS_DELETE", new string[,] {
                {"@ID", id}
            }, "Delete(id string)=>bl_sys_user_access");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Erorr fuction [Delete(string id)] in class [bl_sys_user_access], detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Delete user access by user name and object id
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="objId"></param>
    /// <returns></returns>
    public bool Delete(string userName, string objId)
    {
        bool result = false;
        try
        {
            db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USER_ACCESS_DELETE_BY_USER_NAME_OBJ_ID", new string[,] {
                {"@USER_NAME", userName},{"@OBJ_ID", objId}

            }, "Delete(string userName, string objId)=>bl_sys_user_access");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Erorr fuction [Delete(string userName, string objId)] in class [bl_sys_user_access], detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Get All user access list
    /// </summary>
    /// <returns></returns>
    public List<bl_sys_user_access> GetUserAccessList()
    {

        List<bl_sys_user_access> uList = new List<bl_sys_user_access>();
        try
        {
            DataTable tbl = new DataTable();
            db = new DB();
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USER_ACCESS_GET_LIST_ALL", new string[,] { }, "GetUserAccessList()=>bl_sys_user_access");
            foreach (DataRow r in tbl.Rows)
            {
                uList.Add(new bl_sys_user_access() { Id = r["id"].ToString(), UserName = r["user_name"].ToString(), RoleId = r["role_id"].ToString(), RoleName = r["role_name"].ToString(), ObjId = r["obj_id"].ToString(), ObjName = r["obj_name"].ToString(), ObjCode=r["obj_code"].ToString(), Module=r["module"].ToString(), IsView = Convert.ToInt32(r["is_view"].ToString()), IsAdd = Convert.ToInt32(r["is_add"].ToString()), IsUpdate = Convert.ToInt32(r["is_update"].ToString()), IsApprove = Convert.ToInt32(r["is_approve"].ToString()), IsAdmin = Convert.ToInt32(r["is_admin"].ToString()) });
            }
        }
        catch (Exception ex)
        {
            uList = new List<bl_sys_user_access>();
            Log.AddExceptionToLog("Error function [GetUserAccessList()] in class [bl_sys_user_access], detail: " + ex.Message);
        }
        return uList;
    }
    /// <summary>
    /// Get user access list by role id, module & user name
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="module"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<bl_sys_user_access> GetUserAccessList(string roleId, string module, string userName)
    {

        List<bl_sys_user_access> uList = new List<bl_sys_user_access>();
        try
        {
            DataTable tbl = new DataTable();
            db = new DB();
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_USER_ACCESS_GET_LIST_BY_CONDI", new string[,] { 
            {"@ROLE_ID", roleId},{"@MODULE", module},{"@USER_NAME",userName}
            }, "GetUserAccessList(string roleId, string module, string userName)=>bl_sys_user_access");

            foreach (DataRow r in tbl.Rows)
            {
                uList.Add(new bl_sys_user_access() { Id = r["id"].ToString(), UserName = r["user_name"].ToString(), RoleId = r["role_id"].ToString(), RoleName = r["role_name"].ToString(), ObjId = r["obj_id"].ToString(), ObjName = r["obj_name"].ToString(), ObjCode = r["obj_code"].ToString(), Module = r["module"].ToString(), IsView = Convert.ToInt32(r["is_view"].ToString()), IsAdd = Convert.ToInt32(r["is_add"].ToString()), IsUpdate = Convert.ToInt32(r["is_update"].ToString()), IsApprove = Convert.ToInt32(r["is_approve"].ToString()), IsAdmin = Convert.ToInt32(r["is_admin"].ToString()) });
            }
        }
        catch (Exception ex)
        {
            uList = new List<bl_sys_user_access>();
            Log.AddExceptionToLog("Error function [GetUserAccessList(string roleId, string module, string userName)] in class [bl_sys_user_access], detail: " + ex.Message);
        }
        return uList;
    }

    public string GetNewId()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "TBL_SYS_USER_ACCESS" }, { "FIELD", "ID" } });
    }
    #endregion public function
}