
using System;


public class bl_micro_application_insurance
{
  private string _ID = "";
  private string _productName = "";
  private string _productNameKh = "";
  private string _productID;

  public bl_micro_application_insurance()
  {
    this._ID = this.GetID();
    if (this.REMARKS != null)
      return;
    this.REMARKS = "";
  }

  private string GetID()
  {
    return Helper.GetNewGuid(new string[2, 2]
    {
      {
        "TABLE",
        "CT_MICRO_APPLICATION_INSURANCE"
      },
      {
        "FIELD",
        "ID"
      }
    });
  }

  private void _GetProduct()
  {
    bl_product productByProductId = da_product.GetProductByProductID(this.PRODUCT_ID);
    if (productByProductId != null)
    {
      this._productName = productByProductId.En_Title;
      this._productNameKh = productByProductId.Kh_Title;
    }
    else
    {
      this._productName = "";
      this._productNameKh = "";
    }
  }

  public string ID
  {
    get {return this._ID;}
    set { this._ID = value;}
  }

  public string APPLICATION_NUMBER { get; set; }

  public string PRODUCT_ID
  {
    get {return this._productID;}
    set
    {
      this._productID = value;
      this._GetProduct();
    }
  }

  public int TERME_OF_COVER { get; set; }

  public int PAYMENT_PERIOD { get; set; }

  public double SUM_ASSURE { get; set; }

  public int PAY_MODE { get; set; }

  public string PAYMENT_CODE { get; set; }

  public double PREMIUM { get; set; }

  public double ANNUAL_PREMIUM { get; set; }

  public double TOTAL_AMOUNT { get; set; }

  public double USER_PREMIUM { get; set; }

  public string PACKAGE { get; set; }

  public string CREATED_BY { get; set; }

  public DateTime CREATED_ON { get; set; }

  public string UPDATED_BY { get; set; }

  public DateTime UPDATED_ON { get; set; }

  public double DISCOUNT_AMOUNT { get; set; }

  public string REMARKS { get; set; }

  public bl_micro_product_config.PERIOD_TYPE COVER_TYPE { get; set; }

  public string PAY_MODE_STRING
  {
    get {return this.PAY_MODE >= 0 ? Helper.GetPaymentModeEnglish(this.PAY_MODE) : "";}
  }

  public string PAY_MODE_STRING_KH
  {
    get {return this.PAY_MODE >= 0 ? Helper.GetPaymentModeInKhmer(this.PAY_MODE) : "";}
  }

  public string PRODUCT_NAME {get{return this._productName;}}

  public string PRODUCT_NAME_KH { get { return this._productNameKh; } }
}
