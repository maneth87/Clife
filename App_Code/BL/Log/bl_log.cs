using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_log
/// </summary>
public class bl_log
{
	public bl_log()
	{
		//
		// TODO: Add constructor logic here
		//

       
	}
    private string buildDesc()
    {
       return  "User: " + UserName +
               System.Environment.NewLine + "Date: " + LogDate.ToString("dd-MMM-yyyy HH:mm:ss") +
               System.Environment.NewLine + "Class: " + Class +
               System.Environment.NewLine + "Function Name: " + FunctionName +
               System.Environment.NewLine + "Error Line: " + ErrorLine +
               System.Environment.NewLine + "Message: " + Message;
       
    }
    private string description = "";    public string LogId { get; set; }
    public string LogType { get; set; }
    public DateTime LogDate { get; set; }
    public string Class { get; set; }
    public string FunctionName { get; set; }
    public string Message { get; set; }
    public Int32 ErrorLine { get; set; }
    public string Description { get { return buildDesc(); } }
    public string UserName { get; set; }
}