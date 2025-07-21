using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_group_micro_certificate
/// </summary>
public class bl_group_micro_certificate
{
	public bl_group_micro_certificate()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string Id { get; set; }
   
    public string CustomerNo { get; set; }
    public string PolicyNumber { get; set; }
    public string AgentCode { get; set; }
    public string AgentNameEn { get; set; }
    public string AgentNameKh { get; set; }
    public int IdType { get; set; }
    public string IdEn { get; set; }
    public string IdKh { get; set; }
    public string IdNo { get; set; }
    public string FullName { get; set; }
   
    public int Gender { get; set; }
    public string GenderEn { get; set; }
    public string GenderKh { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string Nationality { get; set; }
    public string Address { get; set; }
    public string Province { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductNameKh { get; set; }
    public double SumAssure { get; set; }
    public string CoverPeriodType { get; set; }
    public string PayPeriodType { get; set; }
    public int TermOfCover { get; set; }
    public int PaymentPeriod { get; set; }
    public int PayMode { get; set; }
    public string PayModeEn { get; set; }
    public string PayModeKh { get; set; }
    public double Premium { get; set; }
    public double AnnualPremium { get; set; }
    public double UserPremium { get; set; }
    public double DiscountAmount { get; set; }
    public double TotalAmount { get; set; }
    public string RiderProductId { get; set; }
    public string RiderProductName { get; set; }
    public string RiderProductNameKh { get; set; }
    public double RiderSumAssure { get; set; }
    public double RiderPremium { get; set; }
    public double RiderAnnualPremium { get; set; }
    public double RiderDiscountAmount { get; set; }
    public double RiderTotalAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime NextDueDate { get; set; }
}