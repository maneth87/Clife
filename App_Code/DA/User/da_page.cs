using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_page
/// </summary>
public class da_page
{
	public da_page()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static List<bl_page> GetPage(string page_name)
    {
        List<bl_page> page_list = new List<bl_page>();
        try
        {
            DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString_User(), "SP_GET_PAGE", new string[,] { { "@PAGE_NAME", page_name } }, "da_page => GetPage(string page_name)");
            foreach (DataRow row in tbl.Rows)
            {
                page_list.Add(new bl_page() { PageID = row["id"].ToString().Trim(), PageName = row["page_name"].ToString().Trim(), Remarks = row["remarks"].ToString().Trim() });
            }
        }
        catch (Exception ex)
        {
            page_list = new List<bl_page>();
            Log.AddExceptionToLog("Error function [GetPage(string page_name)] in class [da_page], detail:" + ex.Message);
        }

        return page_list;
    }

    public static bool AddPage(bl_page page_obj)
    {
        bool status = false;
        try
        {
            status = new DB().Execute(AppConfiguration.GetConnectionString_User(), "SP_INSERT_PAGE", new string[,]{
                {"@app_id", page_obj.ApplicatoinID},
                {"@page_id", page_obj.PageID},
                {"@page_name",page_obj.PageName},
                {"@created_by",page_obj.CreatedBy},
                {"@created_on",page_obj.CreatedOn+""},
                {"@remarks",page_obj.Remarks}
            });
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [Addpage(bl_page page_obj)] in class [da_page], detail:" + ex.Message);
        }

        return status;
    }

    public static DataTable GetPageByID(string page_id)
    {
        DataTable tbl = new DataTable();
        try
        {
            tbl = new DB().GetData(AppConfiguration.GetConnectionString_User(), "SP_GET_PAGE_BY_ID", new string[,] { { "@page_id", page_id } }, "da_page => GetPageByID(string page_id)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPageByID(string page_id)] in class [da_page], detail:" + ex.Message);
        }

        return tbl;
    }

    public static bool EditPage(string page_id, string page_name, string remarks)
    {
        bool status = false;
        try
        {
            status = new DB().Execute(AppConfiguration.GetConnectionString_User(), "SP_EDIT_PAGE", new string[,]{
                {"@PAGE_ID", page_id},
                {"@PAGE_NAME",page_name},
                {"@REMARK",remarks}
            });
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [EditPage(string page_id,string page_name, string remarks)] in class [da_page], detail:" + ex.Message);
        }

        return status;
    }

    public static bool DeletePage(string page_id)
    {
        bool status = false;
        try
        {
            status = new DB().Execute(AppConfiguration.GetConnectionString_User(), "SP_DELETE_PAGE", new string[,]{
                {"@PAGE_ID", page_id}
            });
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeletePage(string page_id)] in class [da_page], detail:" + ex.Message);
        }

        return status;
    }

    public static List<bl_page> GetPageByUserID(string user_id)
    {
        List<bl_page> page_list = new List<bl_page>();
        try
        {
            DataTable tbl = new DB().GetData(AppConfiguration.GetConnectionString_User(), "SP_GET_PAGE_BY_USERID", new string[,] { { "@USER_ID", user_id } }, "da_page => GetPageByUserID(string user_id)");
            foreach (DataRow row in tbl.Rows)
            {
                page_list.Add(new bl_page() { PageID = row["id"].ToString().Trim(), PageName = row["page_name"].ToString().Trim()});
            }
        }
        catch (Exception ex)
        {
            page_list = new List<bl_page>();
            Log.AddExceptionToLog("Error function [GetPageByUserID(string user_id)] in class [da_page], detail:" + ex.Message);
        }

        return page_list;
    }
}