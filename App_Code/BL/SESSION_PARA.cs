using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SESSION_PARA
/// </summary>
[Serializable]
public class SESSION_PARA
{
	public SESSION_PARA()
	{
		//
		// TODO: Add constructor logic here
		//
      
           
	}
    public string BranchName { get; set; }
    public string BranchCode { get; set; }
    public string ChannelItemId { get; set; }
    public string ChannelLocationId { get; set; }
    public string AgentCode { get; set; }
    public string AgentName { get; set; }
    public string OldApplicationNumber { get; set; }
    /// <summary>
    /// The new application number of repayment policy
    /// </summary>
    public string NewApplicationNumber { get; set; }
    public string CustomerId { get; set; }
    public string OldPolicyId { get; set; }
    public string OldPolicyNumber { get; set; }
    public string OldPolicyStatus { get; set; }
    public DateTime OldPolicyExpiryDate { get; set; }
    public DateTime PolicyNewEffectiveDate { get { return GetNewEffectiveDate(); } set { _newEffectiveDate = value; } }
    /// <summary>
    /// New or Repayment
    /// </summary>
    public string ApplicationType { get; set; }
    private DateTime _newEffectiveDate;
    private DateTime GetNewEffectiveDate()
    {
        return OldPolicyExpiryDate.AddDays(1);
    }


}