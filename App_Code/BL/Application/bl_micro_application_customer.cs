
using System;

public class bl_micro_application_customer
{
  private string _ID = "";

  public bl_micro_application_customer()
  {
    this._ID = this.GetID();
    if (this.PHONE_NUMBER2 == null)
      this.PHONE_NUMBER2 = "";
    if (this.PHONE_NUMBER3 == null)
      this.PHONE_NUMBER3 = "";
    if (this.EMAIL1 == null)
      this.EMAIL1 = "";
    if (this.EMAIL2 == null)
      this.EMAIL2 = "";
    if (this.EMAIL3 == null)
      this.EMAIL3 = "";
    if (this.REMARKS == null)
      this.REMARKS = "";
    if (this.PLACE_OF_BIRTH != null)
      return;
    this.PLACE_OF_BIRTH = "";
  }

  private string GetID()
  {
    return Helper.GetNewGuid(new string[2, 2]
    {
      {
        "TABLE",
        "CT_MICRO_APPLICATION_CUSTOMER"
      },
      {
        "FIELD",
        "CUSTOMER_ID"
      }
    });
  }

  public string CUSTOMER_ID
  {
    get {return this._ID;}
    set { this._ID = value;}
  }

  public int SEQ { get; set; }

  public string CUSTOMER_NUMBER { get; set; }

  public string ID_TYPE { get; set; }

  public string ID_NUMBER { get; set; }

  public string FIRST_NAME_IN_ENGLISH { get; set; }

  public string LAST_NAME_IN_ENGLISH { get; set; }

  public string FIRST_NAME_IN_KHMER { get; set; }

  public string LAST_NAME_IN_KHMER { get; set; }

  public string GENDER { get; set; }

  public DateTime DATE_OF_BIRTH { get; set; }

  public string NATIONALITY { get; set; }

  public string MARITAL_STATUS { get; set; }

  public string OCCUPATION { get; set; }

  public string PLACE_OF_BIRTH { get; set; }

  public string HOUSE_NO_KH { get; set; }

  public string STREET_NO_KH { get; set; }

  public string VILLAGE_KH { get; set; }

  public string COMMUNE_KH { get; set; }

  public string DISTRICT_KH { get; set; }

  public string PROVINCE_KH { get; set; }

  public string HOUSE_NO_EN { get; set; }

  public string STREET_NO_EN { get; set; }

  public string VILLAGE_EN { get; set; }

  public string COMMUNE_EN { get; set; }

  public string DISTRICT_EN { get; set; }

  public string PROVINCE_EN { get; set; }

  public string PHONE_NUMBER1 { get; set; }

  public string PHONE_NUMBER2 { get; set; }

  public string PHONE_NUMBER3 { get; set; }

  public string EMAIL1 { get; set; }

  public string EMAIL2 { get; set; }

  public string EMAIL3 { get; set; }

  public string CREATED_BY { get; set; }

  public DateTime CREATED_ON { get; set; }

  public string UPDATED_BY { get; set; }

  public DateTime UPDATED_ON { get; set; }

  public string REMARKS { get; set; }

  public int STATUS { get; set; }

  public string VILLAGE_CODE { get; set; }

  public string COMMUNE_CODE { get; set; }

  public string DISTRICT_CODE { get; set; }

  public string PROVINCE_CODE { get; set; }

  public string ID_TYPE_STRING
  {
    get {return !(this.ID_TYPE != "-1") ? "" : Helper.GetIDCardTypeText(Convert.ToInt32(this.ID_TYPE));}
  }

  public string ID_TYPE_STRING_KH
  {
    get {return !(this.ID_TYPE != "-1") ? "" : Helper.GetIDCardTypeTextKh(Convert.ToInt32(this.ID_TYPE));}
  }

  public string FULL_NAME {get{return this.LAST_NAME_IN_ENGLISH.Trim()+" "+this.FIRST_NAME_IN_ENGLISH.Trim();}}

  public string FULL_NAME_KH {get{return this.LAST_NAME_IN_KHMER.Trim()+" "+this.FIRST_NAME_IN_KHMER.Trim();}}

  public string GENDER_STRING
  {
      get { return !(this.GENDER != "-1") ? "" : Helper.GetGenderText(Convert.ToInt32(this.GENDER), false); }
  }

  public string GENDER_STRING_KH
  {
    get
    {
      return !(this.GENDER != "-1") ? "" : Helper.GetGenderText(Convert.ToInt32(this.GENDER), false, true);
    }
  }

  public string ADDRESS_KH
  {
    get
    {
      return (string.IsNullOrWhiteSpace(this.VILLAGE_KH) ? "" : this.VILLAGE_KH + " ") + (string.IsNullOrWhiteSpace(this.COMMUNE_KH) ? "" : this.COMMUNE_KH + " ") + (string.IsNullOrWhiteSpace(this.COMMUNE_KH) ? "" : this.DISTRICT_KH + " ") + this.PROVINCE_KH;
    }
  }
}
