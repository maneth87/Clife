
using System;

public class bl_micro_policy_detail
{
  private string _PAY_MODE_TEXT = "";
  private static DB db = new DB();
  private string _ID = "";

  public bl_micro_policy_detail()
  {
    this._ID = this.GetID();
    double discountAmount = this.DISCOUNT_AMOUNT;
    double referralFee = this.REFERRAL_FEE;
    double referralIncentive = this.REFERRAL_INCENTIVE;
    if (this.UPDATED_BY == null)
      this.UPDATED_BY = "";
    DateTime updatedOn = this.UPDATED_ON;
    if (this.REMARKS == null)
      this.REMARKS = "";
    int payMode = this.PAY_MODE;
    this._PAY_MODE_TEXT = Helper.GetPaymentModeEnglish(this.PAY_MODE);
  }

  public string POLICY_DETAIL_ID
  {
    get {return this._ID;}
    set { this._ID = value;}
  }

  public string POLICY_ID { get; set; }

  public DateTime EFFECTIVE_DATE { get; set; }

  public DateTime MATURITY_DATE { get; set; }

  public DateTime EXPIRY_DATE { get; set; }

  public DateTime ISSUED_DATE { get; set; }

  public int AGE { get; set; }

  public string CURRANCY { get; set; }

  public double SUM_ASSURE { get; set; }

  public int PAY_MODE { get; set; }

  public string PAY_MODE_TEXT { get { return this._PAY_MODE_TEXT; } }

  public string PAYMENT_CODE { get; set; }

  public double PREMIUM { get; set; }

  public double ANNUAL_PREMIUM { get; set; }

  public double DISCOUNT_AMOUNT { get; set; }

  public double TOTAL_AMOUNT { get; set; }

  public double REFERRAL_FEE { get; set; }

  public double REFERRAL_INCENTIVE { get; set; }

  public int COVER_YEAR { get; set; }

  public int PAY_YEAR { get; set; }

  public int COVER_UP_TO_AGE { get; set; }

  public int PAY_UP_TO_AGE { get; set; }

  public bl_micro_product_config.PERIOD_TYPE COVER_TYPE { get; set; }

  public string POLICY_STATUS_REMARKS { get; set; }

  public string CREATED_BY { get; set; }

  public DateTime CREATED_ON { get; set; }

  public string UPDATED_BY { get; set; }

  public DateTime UPDATED_ON { get; set; }

  public string REMARKS { get; set; }

  private string GetID()
  {
    return Helper.GetNewGuid(new string[,]
    {
      {
        "TABLE",
        "CT_MICRO_POLICY_DETAIL"
      },
      {
        "FIELD",
        "POLICY_DETAIL_ID"
      }
    });
  }
}
