using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_agent_mapping
/// </summary>
public class da_agent_mapping
{
    public da_agent_mapping()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Message or result of transaction
    /// </summary>
    public static string Message { get; set; }
    /// <summary>
    /// Status of transaction
    /// </summary>
    public static bool Transaction { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">Agent Mapping object</param>
    /// <returns></returns>
    public static bool Save(bl_agent_mapping obj)
    {
        bool result = false;
        DB db = new DB();
        try
        {

            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_CHANNEL_LOCATION_INSERT", new string[,] {
            {"@ID", obj.Id}, {"@USER_NAME", obj.UserName}, {"@SALE_AGENT_ID",obj.SaleAgentId}, {"@CHANNEL_LOCATION_ID", obj.ChannelLocationId}, {"@STATUS", obj.Status+""},
            {"@REMARKS",obj.Remarks},{"@CREATED_BY", obj.CreatedBy}, {"@CREATED_ON", obj.CreatedOn+""}
            }, "da_agent_mapping=>Save(bl_agent_mapping obj))");
            Transaction = result;
            Message = Transaction == true ? "Saved record successfully." : "Saved record fail.";
        }
        catch (Exception ex)
        {
            result = false;
            Transaction = false;
            Message = ex.Message;
            Log.AddExceptionToLog("Error function [Save(bl_agent_mapping obj)] in class [da_agent_mapping] detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Update Sale agent channel location by id
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool Update(bl_agent_mapping obj)
    {
        bool result = false;
        DB db = new DB();
        try
        {

            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_CHANNEL_LOCATION_UPDATE", new string[,] {
            {"@ID", obj.Id}, {"@USER_NAME", obj.UserName}, {"@SALE_AGENT_ID",obj.SaleAgentId}, {"@CHANNEL_LOCATION_ID", obj.ChannelLocationId}, {"@STATUS", obj.Status+""},
            {"@REMARKS",obj.Remarks},{"@UPDATED_BY", obj.UpdatedBy}, {"@UPDATED_ON", obj.UpdatedOn+""}
            }, "da_agent_mapping=>Update(bl_agent_mapping obj))");
            Transaction = result;
            Message = Transaction == true ? "Updated record successfully." : "Updated record fail.";
        }
        catch (Exception ex)
        {
            result = false;
            Transaction = false;
            Message = ex.Message;
            Log.AddExceptionToLog("Error function [Update(bl_agent_mapping obj)] in class [da_agent_mapping] detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Delete sale agent mapping by id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public static bool Delete(string Id)
    {
        bool result = false;
        DB db = new DB();
        try
        {

            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_SALE_AGENT_CHANNEL_LOCATION_DELETE", new string[,] {
            {"@ID", Id}
            }, "da_agent_mapping=>Delete(string Id)");
            Transaction = result;
            Message = Transaction == true ? "Deleted record successfully." : "Deleted record fail.";
        }
        catch (Exception ex)
        {
            result = false;
            Transaction = false;
            Message = ex.Message;
            Log.AddExceptionToLog("Error function [Delete(string Id)] in class [da_agent_mapping] detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Generate new id
    /// </summary>
    /// <returns></returns>
    public static string GetId()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_SALE_AGENT_CHANNEL_LOCATION" }, { "FIELD", "ID" } });
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isActive">true get active agent mapping only, false get all agent mapping</param>
    /// <returns></returns>
    public static List<bl_agent_mapping> GetAgentMappingList(bool isActive=true)
    {
        List<bl_agent_mapping> objList = new List<bl_agent_mapping>();
        try
        {
            DB db = new DB();
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), isActive == true ? "SP_CT_SALE_AGENT_CHANNEL_LOCATION_GET_LIST_ACTIVE" : "SP_CT_SALE_AGENT_CHANNEL_LOCATION_GET_LIST", new string[,] {
            }, "da_agent=>GetAgentMappingList(bool isActive=true)");
            if (db.RowEffect > 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    objList.Add(new bl_agent_mapping()
                    {
                        Id = r["id"].ToString(),
                        UserName = r["user_name"].ToString(),
                        SaleAgentId = r["sale_agent_id"].ToString(),
                        ChannelLocationId = r["channel_location_id"].ToString(),
                        Status = Convert.ToInt32(r["status"].ToString()),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                        UpdatedBy = r["Updated_by"].ToString(),
                        UpdatedOn = Convert.ToDateTime(r["updated_on"].ToString()),
                        Remarks = r["remarks"].ToString(),
                        ChannelItemId =r["channel_item_id"].ToString(),
                        ChannelItemName =r["channel_name"].ToString(),
                         SaleAgentName =r["sale_agent_id"].ToString(),
                          OfficeCode =r["office_code"].ToString(),
                           OfficeName=r["office_name"].ToString()
                    });
                }
                Transaction = true;
                Message = "Success.";
            }
            else if (db.RowEffect == 0)
            {
                Transaction = true;
                Message = "No record found.";
            }
            else
            {
                Transaction = false;
                Message = "Get data error.";
            }
        }
        catch (Exception ex)
        {
            Transaction = false;
            Message = "Get data error.";
            Log.AddExceptionToLog("Error function [GetAgentMappingList(bool isActive=true)] in class [da_agent_mapping] detail: " + ex.Message);
        }
        return objList;
    }
    /// <summary>
    /// Get agent mapping object
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bl_agent_mapping GetAgentMapping(string id)
    {
        bl_agent_mapping obj = new bl_agent_mapping();
        try
        {
            List<bl_agent_mapping> objList = new List<bl_agent_mapping>();
            objList = GetAgentMappingList(false);
            foreach (bl_agent_mapping agent in objList.Where(_ => _.Id == id))
            {
                obj = agent ;
                break;
            }
        
        }
        catch (Exception ex)
        {
            Transaction = false;
            Message = "Get data error.";
            Log.AddExceptionToLog("Error function [GetAgentMapping(string id)] in class [da_agent_mapping] detail: " + ex.Message);
        }
        return obj;
    }
}