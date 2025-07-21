using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_group_micro_application
/// </summary>
public class bl_group_micro_application
{
	public bl_group_micro_application()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string Id { get; set; }
    public string ApplicationNumber { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string AgentCode { get; set; }
    public string AgentNameEn { get; set; }
    public string AgentNameKh { get; set; }
    public int IdType { get; set; }
    public string IdEn { get; set; }
    public string IdKh { get; set; }
    public string IdNo { get; set; }
    public string FirstNameInEnglish { get; set; }
    public string LastNameInEnglish { get; set; }
    public string FirstNameInKhmer { get; set; }
    public string LastNameInKhmer { get; set; }
    public int Gender { get; set; }
    public string GenderEn { get; set; }
    public string GenderKh { get; set; }
    public DateTime BirthOfDate { get; set; }
    public string Nationality { get; set; }
    public string MaritalStatus { get; set; }
    public string MaritalStatusKh { get; set; }
    public string Occupation { get; set; }
    public string OccupationKh { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string AddressKh { get; set; }
    public string Province { get; set; }
    public string ProvinceKh { get; set; }
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
    public string BenFullName { get; set; }
    public string BenAge { get; set; }
    public double PercentageShared { get; set; }
    public string BenAddress { get; set; }
    public string Relation { get; set; }
    public string RelationKh { get; set; }
    public string QuestionId { get; set; }
    public int Answer { get; set; }
    public string AnswerRemarks { get; set; }
    public string PaymentCode { get; set; }
    public string ReferrerId { get; set; }
    public string Referrer { get; set; }
}