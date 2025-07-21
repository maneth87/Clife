using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_customer_lead_status_history
/// </summary>
public class bl_customer_lead_status_history
{
    private string _ID = "";
	public bl_customer_lead_status_history()
	{
		//
		// TODO: Add constructor logic here
		//
        _ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_MICRO_LEAD_STATUS_HISTORY" }, { "FIELD", "ID" } });
	}
    public string ID
    {
        get
        {
            return _ID;
        }
        set
        {
            _ID = value;
        }
    }
    public string LeadID { get; set; }
    public string Status { get; set; }
    public string StatusRemarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}