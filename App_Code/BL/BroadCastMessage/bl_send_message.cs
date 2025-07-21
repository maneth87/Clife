using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_send_message
/// </summary>
public class bl_send_message
{
	public bl_send_message()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string MessageTo { get; set; }
    public string MessageFrom { get; set; }
    public string MessageText { get; set; }
    public string MessageType { get; set; }
    public string Gateway { get; set; }
    public string UserId { get; set; }
    public string UserInfo { get; set; }
    public int Priority { get; set; }
    public DateTime Scheduled { get; set; }
    public int IsRead { get; set; }
    public int IsSent { get; set; }
    public string MessageCate { get; set; }
}