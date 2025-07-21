using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_group_micro_application_number
/// </summary>
public class da_group_micro_application_number
{
    public da_group_micro_application_number()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private static string className = "da_group_micro_application_number";
    public static bool Save(bl_group_micro_application_number obj)
    {
        bool result = false;
        try
        {
            DB db = new DB();

            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_NUMBER_INSERT", new string[,] {
                {"@ID", obj.Id}, {"@APPLICATION_NUMBER",obj.ApplicationNumber}, {"@GROUP_CODE", obj.GroupCode}, {"@CHANNEL_ID",obj.ChannelId}, 
                {"@CHANNEL_ITEM_ID", obj.ChannelItemId}, {"@CHANNEL_LOCATION_ID", obj.ChannelLocationId},
                {"@seq", obj.Seq+""},{"@CREATED_BY", obj.CreatedBy},{"@CREATED_ON", obj.CreatedOn+""}
            }, className + "=>Save(bl_group_micro_application_number obj)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [Save(bl_group_micro_application_number obj)] in class[" + className + "], detail: " + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// Get last sequence number by group code
    /// </summary>
    /// <param name="groupCode"></param>
    /// <returns></returns>
    public static bl_group_micro_application_number GetLastSeq(string groupCode)
    {
        bl_group_micro_application_number obj = new bl_group_micro_application_number();
        try
        {
            DB db = new DB();

            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_NUMBER_GET_LAST_SEQ", new string[,] {
                { "@GROUP_CODE",groupCode}
            }, className + "=>GetLastSeq(string groupCode)");
            if (db.RowEffect > 0)
            {
                var r = tbl.Rows[0];
                obj = new bl_group_micro_application_number()
                {
                    Seq = Convert.ToDouble(r["seq"].ToString()),
                    ApplicationNumber = r["application_Number"].ToString(),
                    GroupCode = r["group_code"].ToString()
                };
            }

        }
        catch (Exception ex)
        {
            obj = new bl_group_micro_application_number();
            Log.AddExceptionToLog("Error function [Save(bl_group_micro_application_number obj)] in class[" + className + "], detail: " + ex.Message);
        }
        return obj;
    }

   
}