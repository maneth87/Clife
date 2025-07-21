using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_ci
/// </summary>
public class bl_ci
{
	public bl_ci()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public class Policy
    {
        public Policy() { }
        #region
        public string PolicyID { get; set; }
        public string PolicyNumber { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public string AddressID { get; set; }
        public string AgentCode { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        /// <summary>
        /// Required when system update data
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Required when system update data
        /// </summary>
        public DateTime UpdatedDateTime { get; set; }
        #endregion
    }
    public class PolicyDetail
    {
        public PolicyDetail() { }
        #region
        public string PolicyDetailID { get; set; }
        public string PolicyID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public double SumAssured { get; set; }
        public int PayModeID { get; set; }
        public string PaymentCode { get; set; }
        /// <summary>
        /// Bank or something
        /// </summary>
        public string PaymentBy { get; set; }
        public double UserPremium { get; set; }
        public double Premium { get; set; }
        /// <summary>
        /// Return premium is the amount which received from customer more than amount paid
        /// </summary>
        public double RETURN_PREMIUM { get; set; }
        public double OriginalPremium { get; set; }
        public double DiscountAmount { get; set; }
        public int Age { get; set; }
        public int CoverYear { get; set; }
        public int PayYear { get; set; }
        public int CoverUpToAge { get; set; }
        public int PayUpToAge { get; set; }
        /// <summary>
        /// New or Renew
        /// </summary>
        public string PolicyStatusRemarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        #endregion
    }
}