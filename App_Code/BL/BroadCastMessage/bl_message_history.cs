using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_message_history
/// </summary>
public class bl_message_history : bl_send_message
{
	public bl_message_history()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int No{get;set;}
    public string StatusCode { get; set; }
    public DateTime SendTime { get; set; }
    public int HistoryRecordID { get; set; }
    public int LogID { get; set; }

    /// <summary>
    /// Add Proterty for Export Excel
    /// </summary> 
    public string Customer_Name { get; set; }
    public string Gender { get; set; }
    public string Phone_Number { get; set; }
    public string Send_Status { get; set; }
    public string Customer_No { get; set; }
    public string Policy_No { get; set; }
    public string Product { get; set; }
    public DateTime Due_Date { get; set; }
    public string Mail_Address { get; set; } 
    public DateTime Remind_Date { get; set; }
    public string Date_Before_Due_Date { get; set; }
    public string Remark { get; set; }
    
    public bl_message_history(int no, string message_cate, string message_from, string message_to, string message_text, DateTime scheduled, DateTime send_time, string status_code, string user_id, int history_record_id, int log_id )
    {
        this.No = no;
        this.MessageCate = message_cate;
        this.MessageFrom = message_from;
        this.MessageTo = message_to;
        this.MessageText = message_text;
        this.Scheduled = scheduled;
        this.SendTime = send_time;
        this.StatusCode = status_code;
        this.UserId = user_id;
        this.HistoryRecordID = history_record_id;
        this.LogID = log_id;
    }
}