using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for bl_portal_sys_object
/// </summary>
public class bl_portal_sys_object
{
    public bl_portal_sys_object()
    {
        //
        // TODO: Add constructor logic here
        //
        if (Remarks == null)
            Remarks = "";
    }
    /// <summary>
    /// Properties
    /// </summary>
    public int ObjId { get; set; }
    public string ObjCode { get; set; }
    public string ObjName { get; set; }
    public string Module { get; set; }
    public Int32 IsActive { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Remarks { get; set; }

    MySqlDB db = new MySqlDB();

    /// <summary>
    /// Gell all system objects
    /// </summary>
    /// <param name="?"></param>
    public List<bl_portal_sys_object> GetSystObjects()
    {
        List<bl_portal_sys_object> Lobj = new List<bl_portal_sys_object>();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_object_get";
            db.Parameters = new string[,] {};
            db.GenerateData();

            if (db.DataCount > 0)
            {
                foreach (DataRow r in db.Data.Rows)
                {
                    Lobj.Add(new bl_portal_sys_object()
                    {
                        ObjId = Convert.ToInt32(r["id"].ToString()),
                        ObjCode = r["code"].ToString(),
                        ObjName = r["name"].ToString(),
                        Module = r["module"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        IsActive = Convert.ToInt32(r["isActive"].ToString()),
                        CreatedBy = r["createdby"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["createdon"].ToString())
                    });
                }
            }
            else
            {
                Lobj = new List<bl_portal_sys_object>();
            }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_portal_sys_object>();
            Log.AddExceptionToLog("Error function [GetSystObjects] in class [bl_portal_sys_object], detail: " + ex.Message );
        }

        return Lobj;
    }
    /// <summary>
    /// Get system object by module
    /// </summary>
    /// <param name="Module"></param>
    /// <returns></returns>
    public List<bl_portal_sys_object> GetSystObjects(string Module)
    {
        List<bl_portal_sys_object> Lobj = new List<bl_portal_sys_object>();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_object_get_by_module";
            db.Parameters = new string[,] { { "v_module", "%" + Module + "%" } };
            db.GenerateData();

            if (db.DataCount > 0)
            {
                foreach (DataRow r in db.Data.Rows)
                {
                    Lobj.Add(new bl_portal_sys_object()
                    {
                        ObjId = Convert.ToInt32(r["id"].ToString()),
                        ObjCode = r["code"].ToString(),
                        ObjName = r["name"].ToString(),
                        Module = r["module"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        IsActive = Convert.ToInt32(r["isActive"].ToString()),
                        CreatedBy = r["createdby"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["createdon"].ToString())
                    });
                }
            }
            else
            {
                Lobj = new List<bl_portal_sys_object>();
            }

        }
        catch (Exception ex)
        {
            Lobj = new List<bl_portal_sys_object>();
            Log.AddExceptionToLog("Error function [GetSystObjects(string Module)] in class [bl_portal_sys_object], detail: " + ex.Message);
        }

        return Lobj;
    }

    /// <summary>
    /// Get system object by id
    /// </summary>
    /// <param name="objectId"></param>
    /// <returns></returns>
    public bl_portal_sys_object GetSystObjects(int objectId)
    {
        bl_portal_sys_object Lobj = new bl_portal_sys_object();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_object_get_by_id";
            db.Parameters = new string[,] { { "i_id", objectId+""} };
            db.GenerateData();

            if (db.DataCount > 0)
            {
                foreach (DataRow r in db.Data.Rows)
                {
                    Lobj=new bl_portal_sys_object()
                    {
                        ObjId = Convert.ToInt32(r["id"].ToString()),
                        ObjCode = r["code"].ToString(),
                        ObjName = r["name"].ToString(),
                        Module = r["module"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        IsActive = Convert.ToInt32(r["isActive"].ToString()),
                        CreatedBy = r["createdby"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["createdon"].ToString())
                    };
                }
            }
            else
            {
                Lobj = new bl_portal_sys_object();
            }

        }
        catch (Exception ex)
        {
            Lobj = new bl_portal_sys_object();
            Log.AddExceptionToLog("Error function [GetSystObjects(string Module)] in class [bl_portal_sys_object], detail: " + ex.Message);
        }

        return Lobj;
    }

    public List<bl_portal_sys_object> GetSystObjects(string Module, string ObjectName)
    {
        List<bl_portal_sys_object> Lobj = new List<bl_portal_sys_object>();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_object_get_by_module_objname";
            db.Parameters = new string[,] { { "v_module", "%"+Module+"%" }, { "v_obj_name", "%"+ObjectName+"%" } };
            db.GenerateData();

            if (db.DataCount > 0)
            {
                foreach (DataRow r in db.Data.Rows)
                {
                    Lobj.Add(new bl_portal_sys_object()
                    {
                        ObjId = Convert.ToInt32(r["id"].ToString()),
                        ObjCode = r["code"].ToString(),
                        ObjName = r["name"].ToString(),
                        Module = r["module"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        IsActive = Convert.ToInt32(r["isActive"].ToString()),
                        CreatedBy = r["createdby"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["createdon"].ToString())
                    });
                }
            }
            else
            {
                Lobj = new List<bl_portal_sys_object>();
            }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_portal_sys_object>();
            Log.AddExceptionToLog("Error function [GetSystObjects(string Module, string ObjectName)] in class [bl_portal_sys_object], detail: " + ex.Message );
        }

        return Lobj;
    }
    /// <summary>
    /// Get Module
    /// </summary>
    /// <returns></returns>
    public List<bl_portal_sys_object> GetSystObjectsModule()
    {
        List<bl_portal_sys_object> Lobj = new List<bl_portal_sys_object>();
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_object_get_module";
            db.Parameters = new string[,] { };
            db.GenerateData();

            if (db.DataCount > 0)
            {
                foreach (DataRow r in db.Data.Rows)
                {
                    Lobj.Add(new bl_portal_sys_object()
                    {
                       
                        Module = r["module"].ToString(),
                       
                    });
                }
            }
            else
            {
                Lobj = new List<bl_portal_sys_object>();
            }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_portal_sys_object>();
            Log.AddExceptionToLog("Error function [GetSystObjectsModule()] in class [bl_portal_sys_object], detail: " + ex.Message );
        }

        return Lobj;
    }

    /// <summary>
    /// Add new system object
    /// </summary>
    /// <param name="SysObject"></param>
    /// <returns></returns>
    public bool AddObject(bl_portal_sys_object SysObject)
    {
        bool result = false;
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_object_insert";
            db.Parameters = new string[,] { {"v_code", SysObject.ObjCode},
                {"v_name", SysObject.ObjName},
                {"v_module",SysObject.Module},
                {"i_isActive", SysObject.IsActive+""},
                {"v_createdBy", SysObject.CreatedBy},
                {"d_createdOn",SysObject.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") } };
            db.Execute();
            result= db.ExecuteStatus;
                      
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [AddObject(bl_portal_sys_object SysObject)] in class [bl_sys_objects], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_objects=>AddObject(bl_sys_objects SysObject)");
        }
        return result;
    }
    /// <summary>
    /// Update existing system object
    /// </summary>
    /// <param name="SysObject"></param>
    /// <returns></returns>
    public bool UpdateObject(bl_portal_sys_object SysObject)
    {
        bool result = false;
        try
        {
            db = new MySqlDB(AppConfiguration.GetCamlifePortalConnectionString());
            db.ProcedureName = "SP_my_aspnet_object_update";
            db.Parameters = new string[,] { 
                {"i_id", SysObject.ObjId+""},
                {"v_code", SysObject.ObjCode},
                {"v_name", SysObject.ObjName},
                {"v_module",SysObject.Module},
                {"i_isActive", SysObject.IsActive+""},
                {"v_updatedBy", SysObject.CreatedBy},
                {"d_updatedOn",SysObject.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") } };
            db.Execute();
            result = db.ExecuteStatus;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateObject(bl_portal_sys_object SysObject)] in class [bl_portal_sys_object], detail: " + ex.Message );
        }
        return result;
    }
}