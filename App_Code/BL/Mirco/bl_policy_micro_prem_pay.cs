using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro_prem_pay
/// </summary>
public class bl_policy_micro_prem_pay
{
	 #region "Constructor"

    public bl_policy_micro_prem_pay()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #endregion


    #region "Private Variable"
    private string _Policy_Micro_Prem_Pay_ID;
    private string _Policy_Micro_ID;
    private DateTime _Due_Date;
    private DateTime _Pay_Date;
    private int _Prem_Year;
    private int _Prem_Lot;
    private double _Amount;
    private string _Sale_Agent_ID;
    private string _Channel_Location_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private string _Payment_Code;

    #endregion

    #region "Public Property"

    public string Policy_Micro_Prem_Pay_ID
    {
        get { return _Policy_Micro_Prem_Pay_ID; }
        set { _Policy_Micro_Prem_Pay_ID = value; }
    }

    public string Policy_Micro_ID
    {
        get { return _Policy_Micro_ID; }
        set { _Policy_Micro_ID = value; }
    }

    public DateTime Due_Date
    {
        get { return _Due_Date; }
        set { _Due_Date = value; }
    }

    public DateTime Pay_Date
    {
        get { return _Pay_Date; }
        set { _Pay_Date = value; }

    }

    public int Prem_Year
    {
        get { return _Prem_Year; }
        set { _Prem_Year = value; }
    }

    public int Prem_Lot
    {
        get { return _Prem_Lot; }
        set { _Prem_Lot = value; }
    }

    public double Amount
    {
        get { return _Amount; }
        set { _Amount = value; }
    }

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
    }

    public string Channel_Location_ID
    {
        get { return _Channel_Location_ID; }
        set { _Channel_Location_ID = value; }
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

    public string Payment_Code
    {
        get { return _Payment_Code; }
        set { _Payment_Code = value; }
    }

    #endregion
}