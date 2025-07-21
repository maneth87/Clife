using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
[Serializable]
/// <summary>
/// Summary description for bl_group_master_product
/// </summary>
public class bl_group_master_product
{
	public bl_group_master_product()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string GroupMasterID { get; set; }
    public string GroupCode { get; set; }
    public string ProductID { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string Remarks { get; set; }
    public string ChannelItemID { get; set; }
    public DateTime EffectiveDate { get; set; }
}