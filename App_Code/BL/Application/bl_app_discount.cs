using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_discount
/// </summary>
public class bl_app_discount
{
	#region "Private Variable"
       
    private string _App_Register_ID;
    private double _Annual_Premium;
    private double _Discount_Amount;
    private double _Total_Amount;
    private double _Premium;
    private int _Pay_Mode;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
           
    #endregion

    #region "Constructor"
    public bl_app_discount()
    {

    }
    #endregion

    #region "Public Property"

    public double Annual_Premium
    {
        get { return _Annual_Premium; }
        set { _Annual_Premium = value; }
    }

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public double Discount_Amount
    {
        get { return _Discount_Amount; }
        set { _Discount_Amount = value; }
    }

    public double Total_Amount
    {
        get { return _Total_Amount; }
        set { _Total_Amount = value; }
    }

    public double Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }

    public int Pay_Mode
    {
        get { return _Pay_Mode; }
        set { _Pay_Mode = value; }
    }

    public DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
    }  

    #endregion
}