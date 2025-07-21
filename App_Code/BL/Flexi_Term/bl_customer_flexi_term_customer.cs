using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_customer_flexi_term_customer
/// </summary>
public class bl_customer_flexi_term_customer
{
	#region "Private Variable"
    private string _Customer_ID;
    private string _Customer_Flexi_Term_ID;  

    #endregion
    #region "Constructor"
    public bl_customer_flexi_term_customer()
    {

    }
    #endregion

    #region "Public Properties"

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }

    public string Customer_Flexi_Term_ID
    {
        get { return _Customer_Flexi_Term_ID; }
        set { _Customer_Flexi_Term_ID = value; }
    }  

    #endregion
}