using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_FP_Address
/// </summary>
public class bl_FP_Address
{
    #region 'local variable'

    private string address_id;
    private string customer_id;
    private string address1;
    private string address2;
    private string created_by;
    private DateTime created_on;
    private string updated_by;
    private DateTime updated_on;

#endregion
    #region 'properties'

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


    public string Address1
    {
        get { return address1; }
        set { address1 = value; }
    }

    public string Address2
    {
        get { return address2; }
        set { address2 = value; }
    }

    public string Customer_id
    {
        get { return customer_id; }
        set { customer_id = value; }
    }


    public string Address_id
    {
        get { return address_id; }
        set { address_id = value; }
    }


    #endregion
    #region 'constructor'
    public bl_FP_Address()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public bl_FP_Address(string address_id, string customer_id, string address1, string address2, string created_by, DateTime created_on, string updated_by, DateTime updated_on)
    {
        this.address_id = address_id;
        this.customer_id = customer_id;
        this.address1 = address1;
        this.address2 = address2;
        this.created_by = created_by;
        this.created_on = created_on;
        this.updated_by = updated_by;
        this.updated_on = updated_on;
    }
    #endregion
}