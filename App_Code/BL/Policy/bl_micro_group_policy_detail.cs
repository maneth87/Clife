using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_group_policy_detail
/// </summary>
/// 
[Serializable]
public class bl_micro_group_policy_detail
{

    private string _id;
	public bl_micro_group_policy_detail()
	{
		//
		// TODO: Add constructor logic here
		//
        _id = GetId();


        if (PremiumRiel == null)
            PremiumRiel = 0;
        if (RenewalFrom == null)
            RenewalFrom = "";
        if (FrequencyReduceYear == null)
            FrequencyReduceYear = 0;
        if (ReduceRate == null)
            ReduceRate = 0;
        if (Remarks == null)
            Remarks = "";
        if (PaymentCode == null)
            PaymentCode = "";
	}

    public string PolicyDetailId { get { return _id; } set { _id = value; } }
    public string PolicyID { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime EffectivedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public Int32 Age { get; set; }
    public double SumAssured { get; set; }
    public Int32 PayMode { get; set; }
    /// <summary>
    /// Purpose to display as text
    /// </summary>
    public string PaymodeText { get { return Helper.GetPaymentModeEnglish(PayMode) ; } }
    public string PaymentCode { get; set; }
   
    public double PremiumRate { get; set; }
    public double Premium { get; set; }
    public double PremiumRiel { get; set; }
    public double AnnualPremium { get; set; }
    public double DiscountAmount { get; set; }
    public double TotalAmount { get; set; }
    public string PolicyStatusRemarks { get; set; }
    /// <summary>
    /// Previous Policy
    /// </summary>
    public string RenewalFrom { get; set; }
    public string PayPeriodType { get; set; }
    public Int32 PayYear { get; set; }
    public string CoverPeriodType { get; set; }
    public Int32 CoverYear { get; set; }
    public Int32 FrequencyReduceYear { get; set; }
    public double ReduceRate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Remarks { get; set; }


    private string GetId()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_DETAIL" }, { "FIELD", "POLICY_DETAIL_ID" } });
    }
}