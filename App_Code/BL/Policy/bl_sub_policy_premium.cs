using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_sub_policy
/// </summary>
public class bl_sub_policy_premium
{
	public bl_sub_policy_premium()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region Properties
    public string PolicyID { get; set; }
    public string PolicyNumber { get; set; }
    public double Premium { get; set; }
    public double OriginalAmount { get; set; }
    public double EmAmount { get; set; }
    public double EmPremium { get; set; }
    public double DiscountAmount { get; set; }
    public double SumInsure { get; set; }
    #endregion
}