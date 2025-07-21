using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_group_master_policy
/// </summary>
public class bl_group_master_policy
{
	public bl_group_master_policy()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string PolicyID { get; set; }
    public string GroupPolicyID { get; set; }
    public string GroupMasterID { get; set; }
    public string SeqNumber { get; set; }
    public string Remarks { get; set; }
    public string CreatedBY { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDatetime { get; set; }
    public string MainPolicyNumber { get; set; }
}