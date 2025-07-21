using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_user_access
/// </summary>
public class da_user_access
{
    List<bl_user_access> list_user_access = new List<bl_user_access>();
	public da_user_access()
	{
		//
		// TODO: Add constructor logic here
		//
        //GET ALL USERS ACCESS
        try
        {
            DataTable tbl_user_access = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetAccountConnectionString(), "SELECT TBL_PAGE.PAGE_NAME, TBL_PAGE.ID, TBL_USER_ACCESS.user_id, TBL_USER_ACCESS.access FROM TBL_PAGE " +
                                                                                            "INNER JOIN TBL_USER_ACCESS ON TBL_PAGE.id = TBL_USER_ACCESS.page_id;");
            foreach (DataRow row in tbl_user_access.Rows)
            {
                list_user_access.Add(new bl_user_access()
                {
                    UserId = row["user_id"].ToString(),
                    PageId = row["id"].ToString(),
                    PageName = row["page_name"].ToString(),
                    Access = Convert.ToInt32(row["access"].ToString())
                });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [da_user_access] in class [da_user_access], Detail: " + ex.Message);
        }

        
	}

    //public List<bl_user_access> GetAllUserAccess()
    //{
    //    return list_user_access;
    //}
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //public List<bl_user_access> GetActiveUserAccess()
    //{
    //    List<bl_user_access> my_user_access_list = new List<bl_user_access>();
    //    try
    //    {
    //        foreach(bl_user_access obj in list_user_access.Where(a => a.Access==1))
    //        {
    //            my_user_access_list.Add(obj);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.AddExceptionToLog("Error function [da_user_access] in class [GetActiveUserAccess], Detail: " + ex.Message);
    //    }
    //    return my_user_access_list;
    //}
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //public bl_user_access GetActiveUserAccessByUserId( string user_id)
    //{
    //    bl_user_access my_user_access = new bl_user_access();
    //    try
    //    {
    //        foreach (bl_user_access obj in list_user_access.Where(a => a.Access == 1 && a.UserId==user_id.Trim()))
    //        {
    //            my_user_access = obj;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.AddExceptionToLog("Error function [da_user_access] in class [GetActiveUserAccessByUserId], Detail: " + ex.Message);
    //    }
    //    return my_user_access;
    //}
    //public bl_user_access GetActiveUserAccessPage(string page_name, string user_id)
    //{
    //    bl_user_access my_user_access = new bl_user_access();
    //    try
    //    {
    //        foreach (bl_user_access obj in list_user_access.Where(a => a.Access == 1 && a.PageName.ToUpper().Trim() == page_name.ToUpper().Trim() && a.UserId.ToUpper().Trim() == user_id.Trim().ToUpper()))
    //        {
    //            my_user_access = obj;
    //            break;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Log.AddExceptionToLog("Error function [da_user_access] in class [GetActiveUserAccessByUserId], Detail: " + ex.Message);
    //    }
    //    return my_user_access;
    //}

    //public static bool SaveUserAccess(bl_user_access user_obj)
    //{
    //    bool status = false;
    //    try
    //    {
    //        status = new DB().Execute(AppConfiguration.GetAccountConnectionString(), "SP_INSERT_USER_ACCESS", new string[,] { 
            
    //            {"@ID",user_obj.ID}, 
    //            {"@USER_ID", user_obj.UserId},
    //                {"@PAGE_ID", user_obj.PageId},
    //                {"@ACCESS",user_obj.Access+""}, 
    //                {"@CREATED_BY",user_obj.CreatedBy},
    //                {"@CREATED_DATETIME", user_obj.CreatedDateTime+""}
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.AddExceptionToLog("Error functin [SaveUserAccess(bl_user_access user_obj)] in page [da_user_access], detail:" + ex.Message);
    //        status = false;
    //    }
    //    return status;
    //}
}