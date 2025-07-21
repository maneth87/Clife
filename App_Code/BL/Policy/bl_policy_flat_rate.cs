using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_flat_rate
/// By: Maneth
/// Date: 26092017
/// </summary>
public class bl_policy_flat_rate
{
    #region Public properties
    public string PolicyID { get; set; }
    public string PolicyNumber { get; set; }
    public string ProductID { get; set; }
    public string ApplicationNumber { get; set; }
    public string ApplicationOriginNumber { get; set; }
    public string CustomerID { get; set; }
    public string PaymentCode { get; set; }
    public DateTime ApplicationDate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public DateTime AgreementDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime ApprovedDate { get; set; }
    public string ApprovedBy { get; set; }
    public double SumInsured { get; set; }
    public int PayModeID { get; set; }
    public int PolicyType { get; set; }
    public double Premium { get; set; }
    public double AnnualPremium { get; set; }
    public double AnnualOriginPremium { get; set; }
    public double PremiumByMode { get; set; }
    public double ActualPremium { get; set; }
    public double ReturnPremium { get; set; }
    public double Discount { get; set; }
    public double ExtraAnnualPremium { get; set; }
    public double ExtraAmount { get; set; }
    public double ExtraPremiumByMode { get; set; }
    public int PolicyStatusID { get; set; }
    public string UnderWritingStatusID { get; set; }
    public string PolicyStatus { get; set; }
    public string PolicyRemarks { get; set; }
    public string ApplicationRemarks { get; set; }
    public string Remarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public int PayYear { get; set; }
    public int AssuredYear { get; set; }
    public int Age_Insure { get; set; }    
    public int PayUpToAge { get; set; }
    public int AssuredUpToAge { get; set; }
    public string CustomerName { get; set; }
    public string CustomerTypeID { get; set; }
    public string CustomerNumber { get; set; }
    public string CustomerNameKh { get; set; }
    public string Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string SaleAgentID { get; set; }
    public string SaleAgentName { get; set; }
    public string ChannelID { get; set; }
    public string CompanyID { get; set; }
    public string ChannelItemID { get; set; }

    #endregion
    public bl_policy_flat_rate()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// 
    /// </summary>
    //[Flags]
    public enum Status { New = 1, Approved = 2, Decline = 3, Deleted = 4 };
   
    /// <summary>
    /// By: Maneth
    /// Date: 26092017
    /// </summary>
    public class bl_member
    {
        public string PolicyMemberID { get; set; }
        public string PolicyID { get; set; }
        public string CustomerID { get; set; }
        public int Age { get; set; }
        public int PayYear { get; set; }
        public int PayUpToAge { get; set; }
        public int AssuredYear { get; set; }
        public int AssuredUpToAge { get; set; }
        public double SumInsured { get; set; }
        public double AnnualPremium { get; set; }
        public double AnnualOriginPremium { get; set; }
        public double PremiumByMode { get; set; }
        public int PayModeID { get; set; }
        public double Discount { get; set; }
        public double ExtraPercentage { get; set; }
        public double ExtraRate { get; set; }
        public double ExtraAnnualPremium { get; set; }
        public double ExtraPremiumByMode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StatusID { get; set; }
        public double ReturnPremium { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public string CustomerName { get; set; }
       
        public string Gender { get; set; }
        public DateTime BirthDate{get;set;}

    }

    /// <summary>
    /// 
    /// </summary>
    public class bl_cover
    {
        public string CoverID { get; set; }
        public string Cover { get; set; }
        public string Remarks { get; set; }
    }
    /// <summary>
    /// By: Maneth
    /// Date: 26092017 
    /// </summary>
    public class bl_policy_cover
    {
        public string PolicyCoverID { get; set; }
        public string PolicyID { get; set; }
        public string CustomerID { get; set; }
        public string CoverType { get; set; }
        public string CoverTypeID { get; set; }
        public int Age { get; set; }
        public int PayYear { get; set; }
        public int PayUpToAge { get; set; }
        public int AssuredYear { get; set; }
        public int AssuredUpToAge { get; set; }
        public double AnnualPremium { get; set; }
        public double AnnualOriginPremium { get; set; }
        public double PremiumByMode { get; set; }
        public int PayModeID { get; set; }
        public double SumInsured { get; set; }
        public string Remarks { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
    /// <summary>
    /// By: Maneth
    /// Date: 26092017
    /// </summary>
    /// 
    ////
    public class bl_riders
    {
        public string PolicyRiderID { get; set; }
        public string PolicyID { get; set; }
        public string CustomerID { get; set; }
        public string FirstNameKh { get; set; }
        public string LastNameKh { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameEn { get; set; }
        public int IDTypeID { get; set; }
        public string IDCard { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; }
        public int Age { get; set; }
        public int PayYear { get; set; }
        public int PayUpToAge { get; set; }
        public int AssuredYear { get; set; }
        public int AssuredUpToAge { get; set; }
        public string Relationship { get; set; }
        public int RiderType { get; set; }
        public double SumInsured { get; set; }
        public int PayModeID { get; set; }
        public double AnnualPremium { get; set; }
        public double AnnualOriginPremium { get; set; }
        public double PremiumByMode { get; set; }
        public double Discount { get; set; }
        public double ExtraPercentage { get; set; }
        public double ExtraRate { get; set; }
        public double ExtraPremium { get; set; }//annual premium
        public double ExtraPremiumByMode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
    /// <summary>
    /// By: Maneth
    /// Date: 26092017 
    /// </summary>
    public class bl_beneficiary
    {
        public string PolicyBenID { get; set; }
        public string PolicyID { get; set; }
        public string CustomerID { get; set; }
        public string FirstNameKh { get; set; }
        public string LastNameKh { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameEn { get; set; }
        public int IDTypeID { get; set; }
        public string IDCard { get; set; }
        public string Nationality { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Relationship { get; set; }
        public double Percentage { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
    public class bl_loan
    {
        public string PolicyID { get; set; }
        public int LoanType { get; set; }
        public double InterestRate { get; set; }
        public int TermYear { get; set; }
        public DateTime LoanEffectiveDate { get; set; }
        public double Loan { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
    /// <summary>
    /// Class for searching policy + customer
    /// </summary>
    public class bl_policy_customer_search : bl_customer
    {
        public string PolicyID { get; set; }
        public string PolicyNumber { get; set; }
    }


}