using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_ct_customer_micro_customer
/// </summary>
public class bl_ct_customer_micro_customer
{
	#region "Private Variable"
    private string _Customer_ID;
    private string _Customer_Micro_ID;  

    #endregion
    #region "Constructor"
    public bl_ct_customer_micro_customer()
    {

    }
    #endregion

    #region "Public Properties"

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }

    public string Customer_Micro_ID
    {
        get { return _Customer_Micro_ID; }
        set { _Customer_Micro_ID = value; }
    }  

    #endregion
}