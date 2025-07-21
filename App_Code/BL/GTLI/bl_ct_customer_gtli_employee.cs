using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_ct_customer_gtli_employee
/// </summary>
public class bl_ct_customer_gtli_employee
{
	#region "Private Variable"
    private string _Customer_ID;
    private string _GTLI_Certificate_ID;  

    #endregion
    #region "Constructor"
    public bl_ct_customer_gtli_employee()
    {

    }
    #endregion

    #region "Public Properties"

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }

    public string GTLI_Certificate_ID
    {
        get { return _GTLI_Certificate_ID; }
        set { _GTLI_Certificate_ID = value; }
    }  

    #endregion
}