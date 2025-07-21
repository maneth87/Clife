using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_receipt_payment
/// </summary>
public class bl_payment_receipt
{
    #region "Private Variable"

    private string _Receipt_ID;
    private string _Policy_Prem_Pay_ID;
    private string _Receipt_Num;
    private int _Payment_Mode;
    private DateTime _Created_On;
    private string _Created_By;
    private float _Rate_Lapsed;

    /// <summary>
    /// For Lapsed Policy
    /// </summary>
    private DateTime _due_date;
    private decimal _interest_amount;
    private decimal _prem_amount;
    private int _duration;
    private int _duration_month;
    private string _policy_status;


    #endregion

	public bl_payment_receipt()
	{
		// TODO: Add constructor logic here
	}

    #region "Public Property"

    public string Receipt_ID
    {
        get { return _Receipt_ID; }
        set { _Receipt_ID = value; }
    }

    public string Policy_Prem_Pay_ID
    {
        get { return _Policy_Prem_Pay_ID; }
        set { _Policy_Prem_Pay_ID = value; }
    }

    public string Receipt_Num
    {
        get { return _Receipt_Num; }
        set { _Receipt_Num = value; }
    }

    public float Rate_Lapsed
    {
        get { return _Rate_Lapsed; }
        set { _Rate_Lapsed = value; }
    }

    public int Payment_Mode
    {
        get { return _Payment_Mode; }
        set { _Payment_Mode = value; }
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

    /// <summary>
    /// For Lapsed Policy
    /// </summary>

    public DateTime due_date
    {
        get { return _due_date; }
        set { _due_date = value; }
    }

    public decimal interest_amount
    {
        get { return _interest_amount; }
        set { _interest_amount = value; }
    }

    public decimal prem_amount
    {
        get { return _prem_amount; }
        set { _prem_amount = value; }
    }

    public int duration
    {
        get { return _duration; }
        set { _duration = value; }
    }

    public int duration_month
    {
        get { return _duration_month; }
        set { _duration_month = value; }
    }

    public string policy_status
    {
        get { return _policy_status; }
        set { _policy_status = value; }
    }

    #endregion
}