using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_loan_upload
/// </summary>
public class bl_loan_upload
{
    public bl_loan_upload()
    {
        //
        // TODO: Add constructor logic here
        //


    }


    public DateTime SubmmitedDate { get; set; }
    public string LoanID { get; set; }
    public string Branch { get; set; }
    public string ClientName { get; set; }
    public int Gender { get; set; }
    public DateTime DOB { get; set; }
    public int IdType { get; set; }
    public string IdNumber { get; set; }
    public string AgreementNumber { get; set; }
    public string Occupation { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string Currency { get; set; }
    public double ExchangeRate { get; set; }
    public double LoanAmount { get; set; }
    public DateTime DisbursementDate { get; set; }
    public int LoanPeriod { get; set; }
    public int CoverableYear { get; set; }
    public double InsuranceCost { get; set; }
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