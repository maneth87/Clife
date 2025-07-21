using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_group_customer_contact
/// </summary>
public class bl_micro_group_customer_contact
{
    private string _id="";
	public bl_micro_group_customer_contact()
	{
		//
		// TODO: Add constructor logic here
		//
        if (PHONE_NUMBER2 == null)
            PHONE_NUMBER2 = "";
        if (PHONE_NUMBER3 == null)
            PHONE_NUMBER3 = "";
        if (EMAIL1 == null)
            EMAIL1 = "";
        if (EMAIL2 == null)
            EMAIL2 = "";
        if (EMAIL3 == null)
            EMAIL3 = "";
        if (REMARKS == null)
            REMARKS = "";
        //generate id
        _id = getID();
	}
    public string CONTACT_ID { get { return _id; } set { _id = value; } }
    public string CUSTOMER_ID { get; set; }
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

    private string getID()
    {
    return Helper.GetNewGuid(new string[,] { { "TABLE", "CT_GROUP_MICRO_CUSTOMER_CONTACT" }, { "FIELD", "CONTACT_ID" } });
    }
}