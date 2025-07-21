using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_group_policy
/// </summary>
/// 
[Serializable]
public class bl_micro_group_policy
{
    private string _ID;
    private int _LAST_SEQ;
    private string    _LAST_PREFIX;
    private string _LAST_CERTIFICATE_NO;
    //private DB db = new DB();


	public bl_micro_group_policy()
	{
		//
		// TODO: Add constructor logic here
		//
      

        if (Remarks == null)
            Remarks = "";
        
        PolicyStatusDate = new DateTime(1900, 1, 1);

        //if (IsReduceSA == null)
        //    IsReduceSA = 0;
        //if (YearOfReduceSA == null)
        //    YearOfReduceSA = 0;
        if (GroupCode == null)
            GroupCode = "";

        _ID = GetID();

       // _LAST_SEQ = GetLastSEQ();
	}

    public bl_micro_group_policy(string groupCode)
    {
        //
        // TODO: Add constructor logic here
        //

        GroupCode = groupCode;
        if (Remarks == null)
            Remarks = "";

        PolicyStatusDate = new DateTime(1900, 1, 1);

        //if (IsReduceSA == null)
        //    IsReduceSA = 0;
        //if (YearOfReduceSA == null)
        //    YearOfReduceSA = 0;
        if (GroupCode == null)
            GroupCode = "";

        _ID = GetID();
      //  _LAST_SEQ = GetLastSEQ();
    }
    public string PolicyId
    {
        get { return _ID; }
        set { _ID = value; }
    }
    public string GroupMasterId { get; set; }
    public string LoanId { get; set; }
    public string CustomerId { get; set; }
    public string ApplicationId { get; set; }
    public Int32 SEQ { get; set; }
    public string PolicyNumber { get; set; }
    public string ProductId { get; set; }
    public string Currency { get; set; }
    public double ExchangeRate { get; set; }
  
    public string PolicyStatus { get; set; }
    /// <summary>
    /// Policy status date store while policy status not in IF
    /// </summary>
    public DateTime PolicyStatusDate { get; set; }
    public int IsReduceSA { get; set; }
    public int YearOfReduceSA { get; set; }
    public string AgentCode { get; set; }
    public string ChannelId { get; set; }
    public string ChannelItemId { get; set; }
    public string ChannelLocationId { get; set; }
    public string createdBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Remarks { get; set; }

   // public Int32 LAST_SEQ { get { return _LAST_SEQ; } }
   // public string LAST_PREFIX { get { return _LAST_PREFIX; } }
   // public string LAST_CERTIFICATE_NUMBER { get { return _LAST_CERTIFICATE_NO; } }

    public string GroupCode { get; set; }
    public bool IsFirstPolicy { get; set; }
    private string GetID()
    {
        return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_POLICY" }, { "FIELD", "POLICY_ID" } });
      
    }

    //private int GetLastSEQ()
    //{
    //    int seq = 0;
    //    string[] polNo = new string[] { };
    //      DB db = new DB();
    //    DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_GET_LAST_SEQ", new string[,] {
    //    {"@GROUP_CODE",GroupCode}
    //    }, "bl_micro_group_policy => GetLastSEQ()");
    //    if (db.RowEffect == -1)//error
    //    {
    //        seq = -1;
    //    }
    //    else
    //    {
    //        if (tbl.Rows.Count > 0)
    //        {
    //            seq = Convert.ToInt32(tbl.Rows[0]["seq"].ToString());
    //            _LAST_CERTIFICATE_NO = tbl.Rows[0]["POLICY_NUMBER"].ToString();
    //            //_LAST_PREFIX = _LAST_CERTIFICATE_NO.Substring(10, 2);

    //            polNo = _LAST_CERTIFICATE_NO.Split('-');
    //            if (polNo.Length > 0)
    //            {
    //                _LAST_PREFIX = polNo[polNo.Length - 1].ToString() .Substring(0, 2);
    //            }
    //        }
    //        else
    //        {
    //            seq = 0;
    //            _LAST_CERTIFICATE_NO = "";
    //            _LAST_PREFIX = "";
    //        }
    //    }
    //    return seq;
    //}

    public class LastSequence
    {
        public Int32 LastSequenceNumber { get; set; }
        public string Prefix { get; set; }
    }
    


}