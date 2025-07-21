using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_wing_digital_loan_upload
/// </summary>
public class bl_wing_digital_loan_upload
{
	public bl_wing_digital_loan_upload()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// Field Name Agreement_Number
    /// </summary>
    public string AccountNumber { get; set; }
    /// <summary>
    /// Field Name Submitted_date
    /// </summary>
    /// 
    public DateTime AppliedDate { get; set; }
    public DateTime MaturityDate { get; set; }
    /// <summary>
    /// Field Name Loan_ID
    /// </summary>
    public string ID { get; set; }

    public string ClientName { get; set; }
    public string FirstName { get { return getFirstName(); } }
    public string LastName { get { return getLastName(); } }
    public int Gender { get; set; }
    public DateTime DOB { get; set; }
    public int IdType { get; set; }
    public string IdNumber { get; set; }


    public string PhoneNumber { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Commune { get; set; }
    public string Village { get; set; }
    public string Address { get; set; }
 
    public string ChannelId { get; set; }
    public string ChannelItemId { get; set; }
    public string ChannelLocationId { get; set; }
    public string Remarks { get; set; }
    public string Occupation { get; set; }
    public string BeneficiaryName { get; set; }
    public string Relation { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LoanPeriodType { get; set; }
    public int LoanPeriod { get; set; }
    public string BundleName { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsFirstPolicy { get; set; }
    public double Premium { get; set; }
    public double SumAssure { get; set; }
    public string PolicyStatusRemarks { get; set; }

    public int Seq { get; set; }

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
    private string getLastName()
    {
        if (!string.IsNullOrWhiteSpace(ClientName))
        {
            return Helper.GetBreakName(ClientName, 0);
        }
        else
        { return ""; }
    }
    private string getFirstName()
    {
        if (!string.IsNullOrWhiteSpace(ClientName))
        {
            return Helper.GetBreakName(ClientName, 1);
        }
        else
        { return ""; }
    }

    #endregion private function
}