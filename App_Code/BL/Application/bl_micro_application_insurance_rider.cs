
using System;


public class bl_micro_application_insurance_rider
{
  private string _ID = "";
  private string _productID;
  private string _productName = "";
  private string _productNameKh = "";

  public bl_micro_application_insurance_rider()
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
        "CT_MICRO_APPLICATION_INSURANCE_RIDER"
      },
      {
        "FIELD",
        "ID"
      }
    });
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

  public double SUM_ASSURE { get; set; }

  public double PREMIUM { get; set; }

  public double ANNUAL_PREMIUM { get; set; }

  public double DISCOUNT_AMOUNT { get; set; }

  public double TOTAL_AMOUNT { get; set; }

  public string CREATED_BY { get; set; }

  public DateTime CREATED_ON { get; set; }

  public string UPDATED_BY { get; set; }

  public DateTime UPDATED_ON { get; set; }

  public string REMARKS { get; set; }

  public string PRODUCT_NAME {get{return this._productName;}}

  public string PRODUCT_NAME_KH { get { return this._productNameKh; } }

  private void _GetProduct()
  {
    bl_micro_product_rider productByProductId = da_micro_product_rider.GetMicroProductByProductID(this.PRODUCT_ID);
    da_product.GetProductByProductID(this.PRODUCT_ID);
    if (productByProductId != null)
    {
      this._productName = productByProductId.EN_TITLE;
      this._productNameKh = productByProductId.KH_TITLE;
    }
    else
    {
      this._productName = "";
      this._productNameKh = "";
    }
  }
}
