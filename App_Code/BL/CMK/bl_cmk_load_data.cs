using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_cmk_load_data
/// </summary>
public class bl_cmk_load_data
{
	public bl_cmk_load_data()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    #region //Properties
    public int Row_Number { get; set; }
    public string CMKCustomerID { get; set; }
    public string CertificateNo { get; set; }
    public string LoanID { get; set; }
    public string LoanType { get; set; }
    public string ProductID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Gender { get; set; }
    public string DateOfBirth { get; set; }
    public string Group { get; set; }
    public string EffectiveDate { get; set; }

    public string OpenedDate { get; set; }
    public string DateOfEntry { get; set; }
    public string Currancy { get; set; }
    public string PolicyStatus { get; set; }
    public int Age { get; set; }
    public int LoanDuration { get; set; }
    public int CoveredYear { get; set; }
    public string Branch { get; set; }
    public string ChannelLocationID { get; set; }
    public string ChannelChannelItemID { get; set; }

    public string CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedNoted { get; set; }

    #endregion

    #region POLICY Premium
    public string CMKPolicyPremiumID { get; set; }
    public double LoanAmount { get; set; }
    public double LoanAmountRiel { get; set; }
    public double OutstandingBalance { get; set; }
    public double OutstandingBalanceRiel { get; set; }
    public double AssuredAmount { get; set; }
    public double AssuredAmountRiel { get; set; }
    public double MonthlyPremium { get; set; }
    public double ExtraPremium { get; set; }
    public double DiscountAmount { get; set; }
    public double PremiumAfterDiscount { get; set; }
    public double TotalPremium { get; set; }
    public DateTime ReportDate { get; set; }
    public int PaymodeID { get; set; }
    public string PaidOffInMonth { get; set; }
    public string TerminateDate { get; set; }
    public string PaymentBatchNo { get; set; }

    #endregion
}
