using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for bl_sys_objects
/// </summary>
public class bl_sys_objects
{
    DB db = new DB();
	public bl_sys_objects()
	{
		//
		// TODO: Add constructor logic here
		//
        if (Remarks == null)
            Remarks = "";
        
	}
    public  bl_sys_objects(string ObjId, string ObjCode, string ObjName, string Module, string Path, Int32 IsActive, string TransactionBy, DateTime TransactionDate, String Remarks)
    {
        this.ObjId = ObjId;
        this.ObjCode = ObjCode;
        this.ObjName = ObjName;
        this.Module = Module;
        this.Path = Path;
        this.IsActive = IsActive;
        this.TransactionBy = TransactionBy;
        this.TransactionDate = TransactionDate;
    }
    /// <summary>
    /// Properties
    /// </summary>
    public string ObjId { get; set; }
    public string ObjCode { get; set; }
    public string ObjName { get; set; }
    public string Module { get; set; }
    public string Path { get; set; }
    public Int32 IsActive { get; set; }
    public string TransactionBy { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Remarks { get; set; }

    /// <summary>
    /// Gell all system objects
    /// </summary>
    /// <param name="?"></param>
    public List<bl_sys_objects> GetSystObjects()
    {
        List<bl_sys_objects>Lobj=new List<bl_sys_objects>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_OBJECTS_GET_ALL", new string[,] { }, "bl_sys_objects=>GetSystObjects()");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_objects(r["obj_id"].ToString(), r["obj_code"].ToString(), r["obj_name"].ToString(), r["module"].ToString(), r["path"].ToString(), Convert.ToInt32(r["is_active"].ToString()),"", DateTime.Now,""));
            }
        }
        catch(Exception ex)
        {
            Lobj=new List<bl_sys_objects>();
            Log.AddExceptionToLog("Error function [GetSystObjects] in class [bl_sys_objects], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_objects=>GetSystObjects()");
        }
        
        return Lobj;
    }
    /// <summary>
    /// Get system object by module
    /// </summary>
    /// <param name="Module"></param>
    /// <returns></returns>
    public List<bl_sys_objects> GetSystObjects(string Module)
    {
        List<bl_sys_objects> Lobj = new List<bl_sys_objects>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_OBJECTS_GET_BY_MODULE", new string[,] {
            
            {
                "@module", "%" +Module + "%"
            }
            }, "bl_sys_objects=>GetSystObjects(string Module)");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_objects(r["obj_id"].ToString(), r["obj_code"].ToString(), r["obj_name"].ToString(), r["module"].ToString(), r["path"].ToString(), Convert.ToInt32(r["is_active"].ToString()), "", DateTime.Now, ""));
            }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_sys_objects>();
            Log.AddExceptionToLog("Error function [GetSystObjects(string Module)] in class [bl_sys_objects], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_objects=>GetSystObjects(string Module)");
        }

        return Lobj;
    }
    public List<bl_sys_objects> GetSystObjects(string Module, string ObjectName)
    {
        List<bl_sys_objects> Lobj = new List<bl_sys_objects>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_OBJECTS_GET_BY_COND", new string[,] {
            {"@MODULE", ""+Module+"%"},{"@OBJ_NAME","%"+ObjectName+"%"}
            }, "bl_sys_objects=>GetSystObjects(string Module, string ObjectName)");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_objects(r["obj_id"].ToString(), r["obj_code"].ToString(), r["obj_name"].ToString(), r["module"].ToString(), r["path"].ToString(), Convert.ToInt32(r["is_active"].ToString()), "", DateTime.Now, ""));
            }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_sys_objects>();
            Log.AddExceptionToLog("Error function [GetSystObjects(string Module, string ObjectName)] in class [bl_sys_objects], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_objects=>GetSystObjects(string Module, string ObjectName)");
        }

        return Lobj;
    }
    /// <summary>
    /// Get Module
    /// </summary>
    /// <returns></returns>
    public List<bl_sys_objects> GetSystObjectsModule()
    {
        List<bl_sys_objects> Lobj = new List<bl_sys_objects>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_OBJECTS_GET_MODULE", new string[,] { }, "bl_sys_objects=>GetSystObjectsModule()");
            foreach (DataRow r in tbl.Rows)
            {
                Lobj.Add(new bl_sys_objects()
                {
                     Module = r["module"].ToString()
                });
            }
        }
        catch (Exception ex)
        {
            Lobj = new List<bl_sys_objects>();
            Log.AddExceptionToLog("Error function [GetSystObjectsModule()] in class [bl_sys_objects], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_objects=>GetSystObjectsModule()");
        }

        return Lobj;
    }

    /// <summary>
    /// Add new system object
    /// </summary>
    /// <param name="SysObject"></param>
    /// <returns></returns>
    public bool AddObject(bl_sys_objects SysObject)
    {
        bool result = false;
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_OBJECT_ADD", new string[,] { 
            {"@obj_id", SysObject.ObjId},
            {"@obj_code",SysObject.ObjCode},
            {"@obj_name", SysObject.ObjName},
            {"@module", SysObject.Module},
            {"@path",SysObject.Path},
            {"@is_active",SysObject.IsActive+""},
            {"@created_by", SysObject.TransactionBy},
            {"@created_on", SysObject.TransactionDate+""},
            {"@remarks", SysObject.Remarks}
            }, "bl_sys_objects => AddObject(bl_sys_objects SysObject)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [AddObject(bl_sys_objects SysObject)] in class [bl_sys_objects], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_objects=>AddObject(bl_sys_objects SysObject)");
        }
        return result;
    }
    /// <summary>
    /// Update existing system object
    /// </summary>
    /// <param name="SysObject"></param>
    /// <returns></returns>
    public bool UpdateObject(bl_sys_objects SysObject)
    {
        bool result = false;
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_TBL_SYS_OBJECT_UPDATE", new string[,] { 
            {"@obj_id", SysObject.ObjId},
            {"@obj_code",SysObject.ObjCode},
            {"@obj_name", SysObject.ObjName},
            {"@module", SysObject.Module},
            {"@path",SysObject.Path},
            {"@is_active",SysObject.IsActive+""},
            {"@updated_by", SysObject.TransactionBy},
            {"@updated_on", SysObject.TransactionDate+""},
            {"@remarks", SysObject.Remarks}
            }, "bl_sys_objects => UpdateObject(bl_sys_objects SysObject)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateObject(bl_sys_objects SysObject)] in class [bl_sys_objects], detail: " + ex.Message + "=>" + ex.StackTrace, "bl_sys_objects=>UpdateObject(bl_sys_objects SysObject)");
        }
        return result;
    }
}