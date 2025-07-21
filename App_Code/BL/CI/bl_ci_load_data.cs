using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_ci_load_data
/// </summary>
public class bl_ci_load_data
{
    private string __PaymentCode = "";

   
    public bl_ci_load_data()
	{
		//
		// TODO: Add constructor logic here
		//
      
    }
    #region //Properties
    /// <summary>
    /// ID Type: 0=ID Card, 1=Passpord, 2=Visa, 3=Birth Certificate
    /// </summary>
    public int IDType { get; set; }
    /// <summary>
    /// ID type Text
    /// </summary>
    public string IDTypeText { get; set; }
    public string ID { get; set; }
    public string ENFirstName { get; set; }
    public string ENLastName { get; set; }
    public string KHFirstName { get; set; }
    public string KHLastName { get; set; }
    public string Gender { get; set; }
    public string DOB { get; set; }
    public string Age { get; set; }
    public string PhoneNumber { get; set; }
    public string PolicyNumber { get; set; }
    public string CountryCode { get; set; }
    public string Province { get; set; }
    public string SA { get; set; }
    /// <summary>
    /// 0=Single, 1=Annual, 2=Semi, 3=Quarterly, 4=Monthly
    /// </summary>
    public string PayMode { get; set; }
    /// <summary>
    /// Payment mode text
    /// </summary>
    public string PayModeText { get; set; }
    public string Agent_code { get; set; }
    public string PaymentCode { get; set; }
    public string PaymentBy { get; set; }
    public string ProductName { get; set; }
    public string UserPremium { get; set; }
    public string SystemPremium { get; set; }
    public string OriginalPremium { get; set; }
    public string DiscountAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    #endregion
}