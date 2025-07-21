using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_cellcard_message
/// </summary>
public class da_cellcard_message
{
	public da_cellcard_message()
	{
		//
		// TODO: Add constructor logic here
		//
	}

   // public bool STATUS = false;

    public bool SaveMessage(bl_cellcard_message messageObj)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetCellCardMessageDbConnectionString(), "SP_INSERT_BROADCAST_MESSAGE",
                new string[,] {
                    {"@ID", messageObj.ID},
                    {"@SENDER_NAME", messageObj.SenderName},
                    {"@MESSAGE_FROM", messageObj.MessageFrom},
                    {"@MESSAGE_TO", messageObj.MessageTo},
                    {"@MESSAGE_TEXT",messageObj.MessageText},
                    {"@MESSAGE_FROM_API", messageObj.MessageFromAPI},
                    {"@MESSAGE_TO_API", messageObj.MessageToAPI},
                    {"@MESSAGE_TEXT_API", messageObj.MessageTextAPI},
                    {"@SEND_DATETIME", messageObj.SendDateTime+""},
                    {"@STATUS", messageObj.Status},
                    {"@REMARKS", messageObj.Remarks},
                    {"@CREATED_BY", messageObj.CreatedBy},
                    {"@CREATED_DATETIME", messageObj.CreatedDateTime+""},
                    {"@CLIENT_CORRELATOR", messageObj.ClientCorrelator},
                    {"@MESSAGE_CATE", messageObj.MessageCate}
                }, 
                "[da_cellcard_message ==> SaveMessage]");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [SaveMessage] in class [da_cellcard_message], Detial:" + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return status;
    }
    public List<bl_cellcard_message> GetMessageList(DateTime fromDate, DateTime toDate, string Cate, string Status, string gate_way)
    {
        List<bl_cellcard_message> messageList = new List<bl_cellcard_message>();
        bl_cellcard_message cellCard;
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetCellCardMessageDbConnectionString(), "SP_GET_BROADCAST_MESSAGE", new string[,] 
            { 
                {"@FROM", fromDate +""},
                {"@TO", toDate +""},
                {"@STATUS", Status}, 
                {"@MESSAGE_CATE", Cate},
                {"@Gateway", gate_way}
            },

            "[da_cellcard_message ==> GetMessageList]");
            foreach (DataRow row in tbl.Rows)
            {
                cellCard = new bl_cellcard_message();
                cellCard.MessageTo = row["MessageTo"].ToString();
                cellCard.MessageText = row["MessageText"].ToString();
                cellCard.Status = row["Status"].ToString();
                cellCard.MessageCate = row["MessageCate"].ToString();
                cellCard.SendDateTime = Convert.ToDateTime(row["CreatedDateTime"].ToString());
                cellCard.IDForUpdated = row["id"].ToString();

                messageList.Add(cellCard);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetMessageList] class [da_cellcard_message], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return messageList;
    }

    /// <summary>
    /// Delete message by id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public bool DeleteMessage(string ID)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetCellCardMessageDbConnectionString(), "SP_DELETE_BROADCAST_MESSAGE_BY_ID", new string[,] {
                {"@ID", ID}
                
            }, "[da_cellcard_message ==> DeleteMessage]");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateMessageStatus] class [da_cellcard_message], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return result;
    }

    public bool UpdateMessageStatus(string messageID, string status, string remarks, string updatedBy, DateTime updatedDateTime)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetCellCardMessageDbConnectionString(), "SP_UPDATE_BROADCAST_MESSAGE_STATUS", new string[,] {
                {"@STATUS", status},
                 {"@REMARKS", remarks},
                {"@MESSAGE_ID", messageID},
                {"@UPDATED_BY", updatedBy},
                {"@UPDATED_DATETIME", updatedDateTime+""}
                
            }, "[da_cellcard_message ==> UpdateMessageStatus]");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [UpdateMessageStatus] class [da_cellcard_message], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return result;
    }
}