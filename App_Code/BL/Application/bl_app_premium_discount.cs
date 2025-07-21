using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_premium_discount
/// </summary>
public class bl_app_premium_discount
{
	#region "Private Variable"

    private string _App_Premium_Discount_ID;
    private string _App_Register_ID;
    private int _Year;
    private double _Discount_Rate;
    private double _Premium_Discount;
    private double _Premium_After_Discount;
           
    #endregion

    #region "Constructor"
    public bl_app_premium_discount()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Premium_Discount_ID
    {
        get { return _App_Premium_Discount_ID; }
        set { _App_Premium_Discount_ID = value; }
    }

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public int Year
    {
        get { return _Year; }
        set { _Year = value; }
    }

    public double Discount_Rate
    {
        get { return _Discount_Rate; }
        set { _Discount_Rate = value; }
    }

    public double Premium_Discount
    {
        get { return _Premium_Discount; }
        set { _Premium_Discount = value; }
    }

    public double Premium_After_Discount
    {
        get { return _Premium_After_Discount; }
        set { _Premium_After_Discount = value; }
    }       
  
    #endregion
}