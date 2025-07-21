using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_group_policy_payment
/// </summary>
public class bl_micro_group_policy_payment
{

    private string _id;

	public bl_micro_group_policy_payment()
	{
		//
		// TODO: Add constructor logic here
		//
        _id = GetId();

        if (Remarks == null)
            Remarks = "";
        if (TransactionRef == null)
            TransactionRef = "";
        if (AmountRiel == null)
            AmountRiel = 0;
        if(PayStatus==null)
            PayStatus=0;

        if (UpdatedBy == null)
            UpdatedBy = "";
        if (UpdatedOn == null)
            UpdatedOn = new DateTime(1900, 1, 1);
	}
    public string PolicyPaymentId { get { return _id; } set { _id = value; } }
    public string PolicyDetailId { get; set; }
    public Int32 PayMode { get; set; }
    public string PayModeText{get{return Helper.GetPaymentModeEnglish(PayMode);}}
    public double UserPremium { get; set; }
    public double Amount { get; set; }
    public double AmountRiel { get; set; }
    public double DiscountAmount { get; set; }
    public double TotalAmount { get; set; }
    public double ReturnAmount { get; set; }
    public string PolicyStatus { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime PayDate { get; set; }
    public DateTime NextDueDate { get; set; }
    public Int32 PremiumYear { get; set; }
    public Int32 PremiumLot { get; set; }
    public string OfficeId { get; set; }
    public string TransactionType { get; set; }
    public string TransactionRef { get; set; }
    public Int32 PayStatus { get; set; }// 0 unpaid, 1 paid
    public DateTime ReportDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public String Remarks { get; set; }


    private string GetId()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY_PAYMENT" }, { "FIELD", "POLICY_PAYMENT_ID" } });
    }



}