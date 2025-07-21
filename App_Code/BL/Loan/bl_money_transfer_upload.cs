using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_money_transfer_upload
/// </summary>
public class bl_money_transfer_upload
{
	public bl_money_transfer_upload()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Field Name Submitted_date
    /// </summary>
    public DateTime IssuedDate { get; set; }
    /// <summary>
    /// Field Name Loan_ID
    /// </summary>
    public string ID { get; set; }
  
    public string ClientName { get; set; }
    public int Gender { get; set; }
    public DateTime DOB { get; set; }
    public int IdType { get; set; }
    public string IdNumber { get; set; }
    /// <summary>
    /// Field Name Agreement_number
    /// </summary>
    public string AccountNumber { get; set; }
   
    public string ContactNumber { get; set; }
    public string Currency { get; set; }
    public double ExchangeRate { get; set; }
    /// <summary>
    /// Field Name Loan_Amount
    /// </summary>
    public double SumAssured { get; set; }
    /// <summary>
    /// Field Name Disbursement_Date
    /// </summary>
    public DateTime EffectiveDate { get; set; }
   
    /// <summary>
    /// Field Name Insurance_Cost
    /// </summary>
    public double Premium { get; set; }
    public string ChannelId { get; set; }
    public string ChannelItemId { get; set; }
    public string ChannelLocationId { get; set; }
    public string Remarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string BeneficiaryName { get; set; }
    public string Relation { get; set; }
    public string GenderText
    {
        get
        {
            return getGender();
        }
    }
    public string IdTypeText
    {
        get
        {
            return getIdTypeText();
        }
    }

    #region private function
    private string getGender()
    {
        if (!string.IsNullOrWhiteSpace(Gender.ToString()))
        {
            return Helper.GetGenderText(Gender, false, false);
        }
        else
        {
            return "";
        }
    }
    private string getIdTypeText()
    {
        if (!string.IsNullOrWhiteSpace(IdType.ToString()))
        {
            return Helper.GetIDCardTypeText(IdType);
        }
        else
        {
            return "";
        }
    }
    #endregion private function
}