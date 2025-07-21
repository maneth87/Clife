using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_policy_expiring_status
/// </summary>
public class bl_micro_policy_expiring_status
{
	public bl_micro_policy_expiring_status()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string PolicyId { get; set; }
    public string Status { get; set; }
    public string Remarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }

}