using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_cmk
/// </summary>
public class bl_cmk
{
	public bl_cmk()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public class CMK_Policy
    {

        #region POLICY

        public string CMKPolicyID { get; set; }
        public string CMKCustomerID { get; set; }
        public string CustomerID { get; set; }
        public string CertificateNo { get; set; }
        public string LoanID { get; set; }
        public string LoanType { get; set; }
        public string Group { get; set; }
        public string ProductID { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime DateOfEntry { get; set; }
        public DateTime ExpireDate { get; set; }

        public string Currancy { get; set; }
        public int Age { get; set; }
        public int LoanDuration { get; set; }
        public int CoveredYear { get; set; }
        public string PolicyStatus { get; set; }
        public string Branch { get; set; }
        public string ChannelLocationID { get; set; }
        public string ChannelChannelItemID { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedNoted { get; set; }

        #endregion
        
    }

    public class CMK_Policy_Prem
    {
        #region POLICY Premium
        public string CMKPolicyPremiumID { get; set; }
        public string CMKPolicyID { get; set; }
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
        public DateTime PaidOffInMonth { get; set; }
        public DateTime TerminateDate { get; set; }
        public string PaymentBatchNo { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedNoted { get; set; }
        public string Status { get; set; }

        #endregion
    }

    public class CMK_Renewal_Premium
    {
        #region RENEWAL PREMIUM
        public string CMK_Policy_Premium_ID { get; set; }
        public string CMK_Policy_ID { get; set; }
        public string Policy_Number { get; set; }
        public string Customer_ID { get; set; }
        public string Product_ID { get; set; }
        public string Product_Name { get; set; }
        public DateTime Effective_Date { get; set; }
        public DateTime CMK_Effective_Date { get; set; }
        public double Sum_Insure { get; set; }
        public DateTime Due_Date { get; set; }
        public double Premium { get; set; }
        public double Extra_Premium { get; set; }
        public double Discount_Amount { get; set; }
        public double Total_Premium { get; set; }
        public DateTime Next_Due_Date { get; set; }
        public string Payment_Receipt_No { get; set; }
        public string Invoice_No { get; set; }
        public int Pay_Year { get; set; }
        public int Pay_Lot { get; set; }
        public string Mode { get; set; }
        public DateTime Report_Date { get; set; }
        public DateTime Created_On { get; set; }
        public string Created_By { get; set; }
        public DateTime Updated_On { get; set; }
        public string Updated_By { get; set; }
        public string Created_Note { get; set; }
        #endregion

    }

    public class CMK_Group_Premium
    {
        #region GROUP PREMIUM
        public string CMK_Group_Policy_ID { get; set; }
        public string Group_Code { get; set; }
        public string Product_ID { get; set; }
        public string Product_Name { get; set; }
        public DateTime Effective_Date { get; set; }
        public double Sum_Insure { get; set; }
        public double Amount { get; set; }
        public string Invoice_No { get; set; }
        public int Pay_Year { get; set; }
        public int Pay_Lot { get; set; }
        public string Mode { get; set; }
        public DateTime Report_Date { get; set; }
        public DateTime Created_On { get; set; }
        public int Number_Of_Policy { get; set; }
        public string Created_By { get; set; }
        public DateTime Updated_On { get; set; }
        public string Updated_By { get; set; }
        public string Created_Note { get; set; }
        #endregion

    }

    public class bl_cmk_total_policy
    {
        public int Number_Of_Policy { get; set; }
        public double Total_Sum_Insure { get; set; }
        public double Total_Premium { get; set; }
        public DateTime Report_Date { get; set; }

    
    }
}
