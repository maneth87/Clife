using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_FP_Contact
/// </summary>
public class bl_FP_Contact
{
    #region 'Local variable'
    private string contact_id;
    private string customer_id;
    private string mobile_phone1;
    private string mobile_phone2;
    private string email;
    private string created_by;
    private DateTime created_on;
    private string updated_by;
    private DateTime updated_on;

    #endregion
    #region 'Properties'

    public DateTime Updated_on
    {
        get { return updated_on; }
        set { updated_on = value; }
    }


    public string Updated_by
    {
        get { return updated_by; }
        set { updated_by = value; }
    }


    public DateTime Created_on
    {
        get { return created_on; }
        set { created_on = value; }
    }


    public string Created_by
    {
        get { return created_by; }
        set { created_by = value; }
    }


    public string Email
    {
        get { return email; }
        set { email = value; }
    }


    public string Mobile_phone1
    {
        get { return mobile_phone1; }
        set { mobile_phone1 = value; }
    }

    public string Mobile_phone2
    {
        get { return mobile_phone2; }
        set { mobile_phone2 = value; }
    }


    public string Customer_id
    {
        get { return customer_id; }
        set { customer_id = value; }
    }


    public string Contact_id
    {
        get { return contact_id; }
        set { contact_id = value; }
    }


    #endregion
    #region 'Constructor'
    public bl_FP_Contact()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_FP_Contact(string contact_id, string customer_id, string mobile_phone1, string mobile_phone2, string email, string created_by, DateTime created_on, string updated_by, DateTime updated_on)
    {
        this.contact_id = contact_id;
        this.customer_id = customer_id;
        this.mobile_phone1 = mobile_phone1;
        this.mobile_phone2 = mobile_phone2;
        this.email = email;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}