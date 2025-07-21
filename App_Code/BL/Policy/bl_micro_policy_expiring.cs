using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_policy_expiring
/// </summary>
public class bl_micro_policy_expiring
{
	public bl_micro_policy_expiring()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string BranchCode { get; set; }
    public string PolicyID { get; set; }
    public string PolicyNumber { get; set; }
    public string PolicyStatus { get; set; }
    public string CustNo { get; set; }
    public string CustNameEn { get; set; }
    public string CustNamekh { get; set; }
    public string CustGender { get; set; }
    public DateTime CustDob { get; set; }
    public string CustIdNo { get; set; }
    public string CustContactNo { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime NextDueDate { get; set; }
    public string ProductID { get; set; }
    public string ProductName { get; set; }
    public string ProductRemarks { get; set; }
    public double Premium { get; set; }
    public string PaymentMode { get; set; }
    public double SumAssure { get; set; }
    public double DHC { get; set; }
    public double Amount { get; set; }
    public double DiscountAmount { get; set; }
    public double TotalAmount { get; set; }
    public string Channel { get; set; }
    public string Company { get; set; }
    public string AgentCode { get; set; }
    public string AgentName { get; set; }
    public string Referrer { get; set; }
    public DateTime GeneratedDate { get; set; }
    /// <summary>
    /// Expiry date - generated date
    /// </summary>
    public int ExpiryIn { get; set; }
    /// <summary>
    /// Expiry date - current system date
    /// </summary>
    public int ExpiryInCurrent { get; set; }
    public string Status { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Remarks { get; set; }
    public string ChannelItemId { get; set; }
    public string ChannelLocationId { get; set; }
    public string NewApplicationNumber { get; set; }
}