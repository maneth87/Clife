using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_prem_pay
/// </summary>
public class bl_app_prem_pay
{
	#region "Private Variable"

    private string _App_Prem_Pay_ID;
    private string _App_Register_ID;
    private DateTime _Pay_Date;
    private double _Amount;
    private int _Is_Init_Payment;   
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private double _Original_Amount;
    private double _Rounded_Amount;

    #endregion


    #region "Constructor"
    public bl_app_prem_pay()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Prem_Pay_ID
    {
        get { return _App_Prem_Pay_ID; }
        set { _App_Prem_Pay_ID = value; }
    }

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public DateTime Pay_Date
    {
        get { return _Pay_Date; }
        set { _Pay_Date = value; }
    }

    public double Amount
    {
        get { return _Amount; }
        set { _Amount = value; }

    }

    public double Original_Amount
    {
        get { return _Original_Amount; }
        set { _Original_Amount = value; }

    }

    public double Rounded_Amount
    {
        get { return _Rounded_Amount; }
        set { _Rounded_Amount = value; }

    }

    public int Is_Init_Payment
    {
        get { return _Is_Init_Payment; }
        set { _Is_Init_Payment = value; }
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