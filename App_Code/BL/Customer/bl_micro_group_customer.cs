using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
[Serializable]
/// <summary>
/// Summary description for bl_micro_group_customer
/// </summary>
///
public class bl_micro_group_customer
{
	public bl_micro_group_customer()
	{
		//
		// TODO: Add constructor logic here
		//

    

        if (FIRST_NAME_EN == null)
            FIRST_NAME_EN = "";
        if (FIRST_NAME_KH == null)
            FIRST_NAME_KH = "";
        if (LAST_NAME_EN == null)
            LAST_NAME_EN = "";
        if (LAST_NAME_KH == null)
            LAST_NAME_KH = "";
        if (FULL_NAME_KH == null)
            FULL_NAME_KH = "";
        if (NATIONALITY == null)
            NATIONALITY = "";
        if (REMARKS == null)
            REMARKS = "";
        if (PLACE_OF_BIRTH == null)
            PLACE_OF_BIRTH = "";
        if (GroupCode == null)
            GroupCode = "";

        _ID = GetID();
     //   _LAST_SEQ = GetLastSEQ();
        genderText = getGenderText();
        idTypeText = getIdTypeText();
	}

    public bl_micro_group_customer(string groupCode)
    {
        //
        // TODO: Add constructor logic here
        //
        GroupCode = groupCode;
        _ID = GetID();
     //   _LAST_SEQ = GetLastSEQ();
        genderText = getGenderText();
        idTypeText = getIdTypeText();

        if (FIRST_NAME_EN == null)
            FIRST_NAME_EN = "";
        if (FIRST_NAME_KH == null)
            FIRST_NAME_KH = "";
        if (LAST_NAME_EN == null)
            LAST_NAME_EN = "";
        if (LAST_NAME_KH == null)
            LAST_NAME_KH = "";
        if (FULL_NAME_KH == null)
            FULL_NAME_KH = "";
        if (NATIONALITY == null)
            NATIONALITY = "";
        if (REMARKS == null)
            REMARKS = "";
        if (PLACE_OF_BIRTH == null)
            PLACE_OF_BIRTH = "";
    }

    private string _ID = "";
    //private Int32 _LAST_SEQ = 0;
    //private string _LAST_PREFIX = "";
    //private string _LAST_CUSTOMER_NO = "";

   // private DB db = new DB();
    private string idTypeText="";
    private string genderText="";

    public string ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    public Int32 SEQ { get; set; }
    public string CUSTOMER_NUMBER { get; set; }
    public int ID_TYPE { get; set; }
    public string ID_NUMBER { get; set; }
    public string FIRST_NAME_EN { get; set; }
    public string LAST_NAME_EN{ get; set; }
    public string FULL_NAME_EN { get; set; }
    public string FIRST_NAME_KH { get; set; }
    public string LAST_NAME_KH { get; set; }
    public string FULL_NAME_KH { get; set; }
    public int GENDER { get; set; }
    public DateTime DOB { get; set; }
    public string NATIONALITY { get; set; }
    public string MARITAL_STATUS { get; set; }
    public string OCCUPATION { get; set; }
    public string PLACE_OF_BIRTH { get; set; }
   
  
    public string CREATED_BY { get; set; }
    public DateTime CREATED_ON { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_ON { get; set; }
    public string REMARKS { get; set; }
    public int STATUS { get; set; }
   // public int LAST_SEQ { get { return _LAST_SEQ; } }
    /// <summary>
    /// Return 2 digits of year
    /// </summary>
   // public string LAST_PREFIX { get { return _LAST_PREFIX; } }
   // public string LAST_CUSTOMER_NUMBER { get { return _LAST_CUSTOMER_NO; } }

    public string GenderText { get { return genderText; } }
    public string IdTypeText { get { return idTypeText; } }

    public string GroupCode { get; set; }

    private string getGenderText()
    {
        return Helper.GetGenderText(GENDER, false, false);
    }
    private string getIdTypeText()
    {
        return Helper.GetIDCardTypeText(ID_TYPE);
    }

    //private int GetLastSEQ()
    //{
    //    DB db = new DB();
    //    int seq = 0;
    //    string[] cus = new string[] { };
    //    string currentseq = "";
    //    DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CUSTOMER_GET_LAST_SEQ", new string[,] { 
    //    {"@GROUP_CODE", GroupCode}
    //    }, "bl_group_micro_customer => GetLastSEQ()");
    //    if (db.RowEffect == -1)//error
    //    {
    //        seq = -1;
    //    }
    //    else
    //    {
    //        if (tbl.Rows.Count > 0)
    //        {
    //            seq = Convert.ToInt32(tbl.Rows[0]["seq"].ToString());
    //            _LAST_CUSTOMER_NO = tbl.Rows[0]["customer_NO"].ToString();
    //           // _LAST_PREFIX = _LAST_CUSTOMER_NO.Substring(7, 2);
    //            cus = _LAST_CUSTOMER_NO.Split('-');
    //            if (cus.Length > 0)
    //            {
    //                currentseq = cus[cus.Length - 1].ToString();
    //                _LAST_PREFIX = currentseq.Substring(0, 2);
    //            }
               
    //        }
    //        else
    //        {
    //            seq = 0;
    //            _LAST_CUSTOMER_NO = "";
    //            _LAST_PREFIX = "";
    //        }
    //    }
    //    return seq;
    //}
    private string GetID()
    {
        string id = "";
        id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CUSTOMER" }, { "FIELD", "CUSTOMER_ID" } });
        return id;
    }

    public class LastSequence {
        public double SequenceNumber { get; set; }
        public string Prefix { get; set; }
    }
}