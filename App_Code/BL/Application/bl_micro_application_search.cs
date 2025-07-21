using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_application_search
/// </summary>
public class bl_micro_application_search
{
	public bl_micro_application_search()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int No { get; set; }
    public string ApplicationID { get; set; }
    public string ApplicationNumber { get; set; }
    public string CustomerLastName { get; set; }
    public string CustomerFirstName { get; set; }
    public DateTime CustomerDOB { get; set; }
    public string CustomberGender { get; set; }
    public string AgentCode { get; set; }
    public string AgentName { get; set; }

}