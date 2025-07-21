using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_message_history
/// </summary>
public class da_message_history
{
	public da_message_history()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from_date">Schedualed Date From</param>
    /// <param name="to_date">Schedualed Date To</param>
    /// <returns></returns>
    public static List<bl_message_history> GetMessageHistory(DateTime from_date, DateTime to_date, string message_cate, string status_code)
    {
        List<bl_message_history> message_list = new List<bl_message_history>();
        try
        {
           
            DataTable tbl;
            tbl = DataSetGenerator.Get_Data_Soure("SP_GET_MESSAGE_HISTORY", new string[,] { {"@FROM", from_date+""}, {"@TO", to_date+""}, {"@MESSAGE_CATE", message_cate},{"@STATUS_CODE", status_code}});
            int no=0;
            foreach (DataRow row in tbl.Rows)
            { 
                no +=1;
                message_list.Add(new bl_message_history(){ No=no, MessageCate=row["messageCate"].ToString(), MessageFrom=row["messageFrom"].ToString(),
                                                            MessageTo=row["messageTo"].ToString(), MessageText= row["messageText"].ToString(), 
                                                            Scheduled=Convert.ToDateTime(row["scheduled"].ToString()), SendTime=Convert.ToDateTime(row["sendTime"].ToString()), 
                                                            StatusCode=row["statusCode"].ToString(), UserId= row["userId"].ToString(), HistoryRecordID=Convert.ToInt32(row["recordId"].ToString()), LogID=Convert.ToInt32(row["id"].ToString())});
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GeMessageHistory] in class [da_message_history], Detial: " + ex.Message);
        }
        return message_list;
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
}