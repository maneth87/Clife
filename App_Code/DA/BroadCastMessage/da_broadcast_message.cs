using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_broadcast_message
/// </summary>
public class da_broadcast_message
{
	public da_broadcast_message()
	{
		//
		// TODO: Add constructor logic here
		//
	}
   
    /// <summary>
    /// Insert Messages into clife database to send messages out.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool SendMessage(bl_send_message obj)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_InsertIntoMessageOut", new string[,] { {"@MessageTo", obj.MessageTo }, 
                                                                                                                       {"@MessageFrom", obj.MessageFrom}, 
                                                                                                                       {"@MessageText", obj.MessageText}, 
                                                                                                                       {"@MessageType", obj.MessageType}, 
                                                                                                                       {"@Gateway", obj.Gateway},
                                                                                                                       {"@UserId", obj.UserId},
                                                                                                                       {"@UserInfo", obj.UserInfo} ,
                                                                                                                       {"@Priority", obj.Priority+""}, 
                                                                                                                       {"@Scheduled",obj.Scheduled+""}, 
                                                                                                                       {"@IsRead", obj.IsRead+""}, 
                                                                                                                       {"@IsSent", obj.IsSent+""}, 
                                                                                                                       {"@MessageCate",obj.MessageCate}}, "da_broadcast_message => SendMessage");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SaveMessage] in class [da_broadcast_message], Detail: " + ex.Message);
            status = false;
        }

        return status;
    }

    /// <summary>
    /// Get all Prefixed number
    /// </summary>
    /// <returns></returns>
    public static List<bl_prefix_number> GetPrefixNumberList()
    {
        List<bl_prefix_number> list_prefix = new List<bl_prefix_number>();
        try
        {

            DataTable tbl = DataSetGenerator.Get_Data_Soure("select Ct_Prefix_Number.Prefix_Number, Ct_Prefix_Number.MessageFrom, Ct_Company_Phone_Type.Gateway_Name,Ct_Company_Phone_Type.Company_Phone_Type_ID from Ct_Prefix_Number INNER JOIN Ct_Company_Phone_Type " +
                                                                                    "ON Ct_Company_Phone_Type.Company_Phone_Type_ID = Ct_Prefix_Number.Company_Phone_Type_ID");

            foreach (DataRow row in tbl.Rows)
            {
                list_prefix.Add(new bl_prefix_number() { PrefixNumber=row["prefix_number"].ToString(), Gateway= row["Gateway_Name"].ToString(), MessageFrom = row["MessageFrom"].ToString(), CompanyPhoneTypeID= Convert.ToInt32(row["company_phone_type_id"].ToString())});
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPrefixNumberList] in class [da_broadcast_message], Detail: " + ex.Message);
        }
        return list_prefix;
    }
    /// <summary>
    /// Get Prefixed number object by phone number
    /// </summary>
    /// <param name="phone_number"></param>
    /// <returns></returns>
    public static bl_prefix_number PrefixNumber(string phone_number)
    {
        bl_prefix_number obj = new bl_prefix_number();

        foreach(bl_prefix_number prefix in GetPrefixNumberList())
        {
           string pre =phone_number.Trim().Substring(0, 3);
            if (phone_number.Trim().Substring(0, 3) == prefix.PrefixNumber.Trim())
            {
                obj = prefix;
                break;
            }
        }

        return obj;
    }

    /// <summary>
    /// Update MessageFrom in table Ct_Prefix_Number
    /// </summary>
    /// <param name="phone_number"></param>
    /// <returns></returns>
    public static bool UpdateGateway(string old_number, string new_number)
    {
        bool status = false;
        try
        {
            
            status = Helper.ExecuteCommand(AppConfiguration.GetConnectionString(), "UPDATE Ct_Prefix_Number SET MessageFrom = '" + new_number + "' WHERE MessageFrom='" + old_number + "';");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateGateway] in class [da_broadcast_message], Detail: " + ex.Message);
        }
        return status;
    }


    /// <summary>
    /// Insert Messages into clife database to send messages out.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool SaveMessage(bl_send_message obj)
    {
        bool status = false;
        try
        {
           
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_InsertIntoMessageOut", new string[,] { {"@MessageTo", obj.MessageTo }, 
                                                                                                                       {"@MessageFrom", obj.MessageFrom}, 
                                                                                                                       {"@MessageText", obj.MessageText}, 
                                                                                                                       {"@MessageType", obj.MessageType}, 
                                                                                                                       {"@Gateway", obj.Gateway},
                                                                                                                       {"@UserId", obj.UserId},
                                                                                                                       {"@UserInfo", obj.UserInfo} ,
                                                                                                                       {"@Priority", obj.Priority+""}, 
                                                                                                                       {"@Scheduled",obj.Scheduled+""}, 
                                                                                                                       {"@IsRead", obj.IsRead+""}, 
                                                                                                                       {"@IsSent", obj.IsSent+""}, 
                                                                                                                       {"@MessageCate",obj.MessageCate}}, "da_broadcast_message => SaveMessage");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SaveMessage] in class [da_broadcast_message], Detail: " + ex.Message);
            status = false;
        }

        return status;
    }
}