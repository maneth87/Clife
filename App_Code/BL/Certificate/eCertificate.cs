using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Certificate
/// </summary>
public class eCertificate
{
   
	public eCertificate()
	{
		//
		// TODO: Add constructor logic here
		//
        DOB = Helper.FormatDateTime("01/01/1900");

    }
   
    #region //Properties
    public string CustomerNumber { get; set; }
    public string CertificateNumber { get; set; }
    public string Name { get; set; }
    public string ID { get; set; }
    public DateTime DOB{get;set;}
    public string Gender { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string PaymentMode { get; set; }
    public string PremiumDueDate { get; set; }
    public DateTime IssuedDate { get; set; }
    public string InsurancePlan { get; set; }
    public double SA { get; set; }
    public double Premium { get; set; }
    public string SaleAgentCode { get; set; }
    public string BenefitFullName { get; set; }
    public string BenefitRelationship { get; set; }
    public int BenefitShare { get; set; }
    public string BenefitRemarks { get; set; }
    public string ProductID { get; set; }
    /*Added @05 March 2018 by maneth*/
    public string PaymentModeEn { get; set; }
    public string PaymentMethod { get; set; }
    //Add @08 August 2019
    public string PhoneNumber { get; set; }
        
    //Add @05 October 2020
    public string ApproverName { get; set; }
    public string ApproverNameKh { get; set; }
    public string ApproverPosition { get; set; }
    public string ApproverPositionKh { get; set; }
    public Byte[] Signature { get; set; }
    public string ErrorMessage { get; set; }

    //ADD @04MARCH2022
  
    public string ProductName { get; set; }
    public string RiderProduct { get; set; }
    public double RiderSA { get; set; }
    public double RiderPremium { get; set; }
    public int CoverageYear { get; set; }
    public int PayYear { get; set; }
    public string EffectiveDateString { get; set; }
    public string ExpiryDateString { get; set; }
    public string BeneficiaryAddress { get; set; }
    public string IDType { get; set; }
    public Byte[] QRCode { get; set; }
    
    #endregion
}