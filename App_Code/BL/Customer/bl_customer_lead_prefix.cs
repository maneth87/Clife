using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for bl_customer_lead_prefix
/// </summary>
public class bl_customer_lead_prefix
{
	public bl_customer_lead_prefix()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string LeadPrefixId { get; set; }
    public string PREFIX1 { get; set; }
    public string PREFIX2 { get; set; }
    public string DIGITS { get; set; }
    public int STATUS { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Remarks { get; set; }

    public class TableName
    {
        public static string Name { get { return "CT_CUSTOMER_LEAD_PREFIX"; } }
    }

    public class bl_customer_lead_number
    {
        public bl_customer_lead_number()
        {
           
        }
        public string LeadNumberId { get; set; }
        public Int32 LeadNumber { get; set; }
        /// <summary>
        /// LeadNumberVar is combinded Lead Prefix and number
        /// </summary>
        public string LeadNumberVar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Remarks { get; set; }
        public string PrefixYear { get { return GetPrefixYear(); } }
        public class TableName
        {
            public static string Name { get { return "CT_CUSTOMER_LEAD_NUMBER"; } }
        }
        private string GetPrefixYear()
        {
           
            bl_customer_lead_prefix preObj = new bl_customer_lead_prefix();
            preObj = da_customer_lead_prefix.GetLastCustomerLeadPrefix();
            if (string.IsNullOrWhiteSpace(LeadNumberVar))
            {
                return preObj.PREFIX2;
            }
            else
            {
                return LeadNumberVar.Substring(preObj.PREFIX1.Length, preObj.PREFIX2.Length);
            }
        }
    }
}
