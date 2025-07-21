using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_group_policy_rider
/// </summary>
public class bl_micro_group_policy_rider
{
	public bl_micro_group_policy_rider()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string PolicyRiderId { get; set; }
    public string PolicyId{get;set;}
    public string ProductId { get; set; }
    public double SumAssured { get; set; }
    public double PremiumRate { get; set; }
    public double Premium { get; set; }
    public double PremiumRiel { get; set; }
    public double AnnualPremium { get; set; }
    public double DiscountAmount { get; set; }
    public double TotalAmount { get; set; }
    public string RiderStatus { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Remarks { get; set; }
}