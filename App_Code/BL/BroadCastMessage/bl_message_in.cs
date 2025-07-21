using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_message_in
/// </summary>
public class bl_message_in
{
	public bl_message_in()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_message_in(int no, DateTime send_time, DateTime receive_time, string message_from, string message_to, string message_text)
    {
        this.No = no;
        this.SendTime = send_time;
        this.ReceiveTime = receive_time;
        this.MessageFrom = message_from;
        this.MessageTo = message_to;
        this.MessageText = message_text;
    }
    #region Properties
    public int No { get; set; }
    public DateTime SendTime { get; set; }
    public DateTime ReceiveTime { get; set; }
    public string MessageFrom { get; set; }
    public string MessageTo { get; set; }
    public string MessageText { get; set; }

    #endregion
}