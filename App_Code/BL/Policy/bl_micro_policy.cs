
using System;
using System.Collections.Generic;
using System.Data;

public class bl_micro_policy
{
    private static DB db = new DB();
    private string _ID = "";
    private int _LAST_SEQ;
    private string _LAST_CERTIFICATE_NO = "";
    private string _LAST_PREFIX = "";
    private string _LAST_PREFIX1 = "";

    public bl_micro_policy()
    {
        this._ID = this.GetID();
        if (this.REMARKS == null)
            this.REMARKS = "";
        this.RenewFromPolicy = this.RenewFromPolicy == null ? "" : this.RenewFromPolicy;
        this._LAST_SEQ = this.GetLastSEQ();
    }
    public bl_micro_policy(string productId)
    {
        _ProductID = productId;
       PRODUCT_ID = _ProductID;
        _ID = GetID();
        //_LAST_SEQ = this.GetLastSEQ();

        if (REMARKS == null)
        {
            REMARKS = "";
        }
        RenewFromPolicy = RenewFromPolicy == null ? "" : RenewFromPolicy;
    }
    private string GetID()
    {
        return Helper.GetNewGuid(new string[,]
    {
      {
        "TABLE",
        "CT_MICRO_POLICY"
      },
      {
        "FIELD",
        "POLICY_ID"
      }
    });
    }

    public string NewID() { return GetID(); }

    private int GetLastSEQ()
    {
        if (string.IsNullOrWhiteSpace(PRODUCT_ID))
        {
            this._LAST_CERTIFICATE_NO = "";
            this._LAST_PREFIX = "";
            this._LAST_PREFIX1 = "";
            return 0;
        }
        DataTable data = bl_micro_policy.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_GET_LAST_SEQ", new string[,]{
    {"@product_id",PRODUCT_ID}
    }, "bl_micro_policy => GetLastSEQ()");
        int lastSeq;
        if (bl_micro_policy.db.RowEffect == -1)
            lastSeq = -1;
        else if (data.Rows.Count > 0)
        {
            if (data.Rows[0]["POLICY_NUMBER"].ToString().Length <= 1)
            {
                lastSeq = 0;
                this._LAST_CERTIFICATE_NO = "";
                this._LAST_PREFIX = ""; 
                this._LAST_PREFIX1 = "";
            }
            else {
                lastSeq = Convert.ToInt32(data.Rows[0]["seq"].ToString());
                this._LAST_CERTIFICATE_NO = data.Rows[0]["POLICY_NUMBER"].ToString();
                this._LAST_PREFIX = this._LAST_CERTIFICATE_NO.Substring(5, 2);
                this._LAST_PREFIX1 = this._LAST_CERTIFICATE_NO.Substring(0, 5);
            }
           
        }
        else
        {
            lastSeq = 0;
            this._LAST_CERTIFICATE_NO = "";
            this._LAST_PREFIX = "";
            this._LAST_PREFIX1 = "";
        }
        return lastSeq;
    }

    public string POLICY_ID
    {
        get { return this._ID; }
        set { this._ID = value; }
    }

    public string POLICY_TYPE { get; set; }

    public string APPLICATION_ID { get; set; }

    public int SEQ { get; set; }

    public string POLICY_NUMBER { get; set; }

    public string CUSTOMER_ID { get; set; }
    string _ProductID = "";
    public string PRODUCT_ID
    {
        get { return _ProductID; }
        set
        {
            _ProductID = value;
            if (_ProductID != null)
                _LAST_SEQ = this.GetLastSEQ();
        }
    }

    public string AGENT_CODE { get; set; }

    public string CHANNEL_ID { get; set; }

    public string CHANNEL_ITEM_ID { get; set; }

    public string CHANNEL_LOCATION_ID { get; set; }

    public string POLICY_STATUS { get; set; }

    public string CREATED_BY { get; set; }

    public DateTime CREATED_ON { get; set; }

    public string UPDATED_BY { get; set; }

    public DateTime UPDATED_ON { get; set; }

    public string REMARKS { get; set; }

    public int LAST_SEQ { get { return this._LAST_SEQ; } }

    public string LAST_PREFIX1 { get { return this._LAST_PREFIX1; } }

    public string LAST_PREFIX { get { return this._LAST_PREFIX; } }

    public string LAST_CERTIFICATE_NUMBER { get { return this._LAST_CERTIFICATE_NO; } }

    public DateTime POLICY_STATUS_DATE { get; set; }

    public string POLICY_STATUS_REMARKS { get; set; }

    public string RenewFromPolicy { get; set; }

    [Serializable]
    public class SavedIssuePolicy
    {
        public string PolicyId { get; set; }

        public string PolicyNumber { get; set; }

        public string ApplicationNumber { get; set; }

        public string CustomerId { get; set; }

        public bool IsExistingCustomer { get; set; }

        public string PolicyDetailID { get; set; }
        public string PolicyPaymentID { get; set; }
        public List<string> PolicyBeneficiaryID { get; set; }
        public string PolicyPrimaryBenenficiaryID { get; set; }
        public string PolicyAddressID { get; set; }
        public string PolicyRiderID { get; set; }
    }
}
