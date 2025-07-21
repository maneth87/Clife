
using System;
using System.Collections.Generic;
using System.Data;


public class bl_micro_application
{
  private int _LAST_SEQ;
  private string _LAST_APPLICATION_NUMBER = "";
  private string _LAST_PREFIX = "";
  private string _ID = "";
  private string _sale_agent_id = "";
  private string _agentName = "";
  private string _agentNameKh = "";

  public bl_micro_application()
  {
    this._LAST_SEQ = this.GetLastSEQ();
    this._ID = this.GetID();
    if (this.REFERRER == null)
      this.REFERRER = "";
    if (this.REFERRER_ID == null)
      this.REFERRER_ID = "";
    if (this.RENEW_FROM_POLICY == null)
      this.RENEW_FROM_POLICY = "";
    if (this.MainApplicationNumber == null)
      this.MainApplicationNumber = "";
    if (this.LoanNumber == null)
         this.LoanNumber = "";
  }

  public string APPLICATION_ID
  {
    get {return this._ID;}
    set { this._ID = value;}
  }

  public int SEQ { get; set; }

  public string APPLICATION_NUMBER { get; set; }

  public DateTime APPLICATION_DATE { get; set; }

  public string CHANNEL_ID { get; set; }

  public string CHANNEL_ITEM_ID { get; set; }

  public string CHANNEL_LOCATION_ID { get; set; }

  public string SALE_AGENT_ID
  {
    get{return this._sale_agent_id;}
    set
    {
      this._sale_agent_id = value;
      this._GetAgent();
    }
  }

  public string APPLICATION_CUSTOMER_ID { get; set; }

  public string CREATED_BY { get; set; }

  public DateTime CREATED_ON { get; set; }

  public string UPDATED_BY { get; set; }

  public DateTime UPDATED_ON { get; set; }

  public string REMARKS { get; set; }

  public string REFERRER_ID { get; set; }

  public string REFERRER { get; set; }

  public string RENEW_FROM_POLICY { get; set; }

  public int LAST_SEQ {get{return this._LAST_SEQ;}}

  public string LAST_APPLICATION_NO {get{return this._LAST_APPLICATION_NUMBER;}}

  public string LAST_PREFIX {get{return this._LAST_PREFIX;}}

  public string CLIENT_TYPE { get; set; }

  public string CLIENT_TYPE_REMARKS { get; set; }

  public string CLIENT_TYPE_RELATION { get; set; }

  public string MainApplicationNumber { get; set; }

  public int NumbersOfPurchasingYear { get; set; }

  public int NumbersOfApplicationFirstYear { get; set; }

  public string LoanNumber { get; set; }

  public string PolicyholderName { get; set; }

  public int PolicyholderGender { get; set; }

  public DateTime PolicyholderDOB { get; set; }

  public int PolicyholderIDType { get; set; }

  public string PolicyholderIDNo { get; set; }

  public string PolicyholderPhoneNumber { get; set; }

  public string PolicyholderPhoneNumber2 { get; set; }

  public string PolicyholderEmail { get; set; }

  public string PolicyholderAddress { get; set; }

  public string SALE_AGENT_NAME { get { return this._agentName; } }

  public string SALE_AGENT_NAME_KH { get { return this._agentNameKh; } }

  private int GetLastSEQ()
  {
    DB db = new DB();
    DataTable data = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_GET_LAST_SEQ", new string[0, 0], "bl_micro_application => GetLastSEQ()");
    int lastSeq;
    if (db.RowEffect == -1)
      lastSeq = -1;
    else if (data.Rows.Count > 0)
    {
      lastSeq = Convert.ToInt32(data.Rows[0]["seq"].ToString());
      this._LAST_APPLICATION_NUMBER = data.Rows[0]["application_number"].ToString();
      this._LAST_PREFIX = this._LAST_APPLICATION_NUMBER.Substring(3, 2);
    }
    else
    {
      lastSeq = 0;
      this._LAST_APPLICATION_NUMBER = "";
      this._LAST_PREFIX = "";
    }
    return lastSeq;
  }

  private string GetID()
  {
    return Helper.GetNewGuid(new string[2, 2]
    {
      {
        "TABLE",
        "CT_MICRO_APPLICATION"
      },
      {
        "FIELD",
        "APPLICATION_ID"
      }
    });
  }

  private void _GetAgent()
  {
    bl_sale_agent_micro saleAgentMicro = da_sale_agent.GetSaleAgentMicro(this.SALE_AGENT_ID);
    if (string.IsNullOrWhiteSpace(saleAgentMicro.SaleAgentId))
      return;
    this._agentName = saleAgentMicro.FullNameEn;
    this._agentNameKh = saleAgentMicro.FullNameKh;
  }

  public class ApplicationFilter
  {
    public string ApplicationId { get; set; }

    public string ApplicationNumber { get; set; }

    public string MainApplicationNumber { get; set; }

    public Helper.ApplicationType ApplicationType { get; set; }

    public int NumbersApplication { get; set; }

    public int NumbersPurchasingYear { get; set; }
  }

  public class bl_application_for_issue
  {
    public string PolicyNumber { get; set; }

    public bl_micro_application_customer Customer { get; set; }

    public bl_micro_application Application { get; set; }

    public bl_micro_application_insurance Insurance { get; set; }

    public bl_micro_application_insurance_rider Rider { get; set; }

    public List<bl_micro_application_beneficiary> Beneficiaries { get; set; }

    public bl_micro_application_questionaire Questionaire { get; set; }

    public bl_micro_application_beneficiary.PrimaryBeneciary PrimaryBeneciary { get; set; }
  }
}
