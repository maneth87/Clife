
using System;

[Serializable]
public class bl_micro_application_beneficiary
{
  private string _ID = "";

  public bl_micro_application_beneficiary()
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
        "CT_MICRO_APPLICATION_BENEFICIARY"
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

  public string NewID()
  {
      return GetID();
  }
  public string APPLICATION_NUMBER { get; set; }

  public string FULL_NAME { get; set; }

  public string AGE { get; set; }

  public string RELATION { get; set; }

  public double PERCENTAGE_OF_SHARE { get; set; }

  public string ADDRESS { get; set; }

  public string CREATED_BY { get; set; }

  public DateTime CREATED_ON { get; set; }

  public string UPDATED_BY { get; set; }

  public DateTime UPDATED_ON { get; set; }

  public string REMARKS { get; set; }

  public DateTime DOB { get; set; }

  public int Gender { get; set; }

  public int IdType { get; set; }

  public string IdNo { get; set; }

  public string GenderString {get {return Helper.GetGenderText(this.Gender, false);}}

  public string IdTypeString {get{return Helper.GetIDCardTypeText(this.IdType);}}

  public class PrimaryBeneciary
  {
    private string _id;

    public string Id
    {
      get
      {
        if (this._id == null)
          this._id = Helper.GetNewGuid(new string[2, 2]
          {
            {
              "TABLE",
              "CT_MICRO_APPLICATION_BENEFICIARY_PRIMARY"
            },
            {
              "FIELD",
              "ID"
            }
          });
        return this._id;
      }
        set { this._id = value; }
    }

    public string ApplicationNumber { get; set; }

    public string FullName { get; set; }

    public string LoanNumber { get; set; }

    public string Address { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedRemarks { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string UpdatedRemarks { get; set; }
  }
}
