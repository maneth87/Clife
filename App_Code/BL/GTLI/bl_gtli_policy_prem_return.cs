using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_policy_prem_return
/// </summary>
public class bl_gtli_policy_prem_return
{
	#region "Private Variable"

    private string _GTLI_Policy_Prem_Return_ID;
    private string _GTLI_Premium_ID;
    private DateTime _Return_Date;   
    private int _Prem_Year;
    private int _Prem_Lot;
    private double _Amount;
    private string _Sale_Agent_ID;
    private string _Office_ID;
    private string _Created_Note;
    private string _Created_By;
    private DateTime _Created_On;
    private int _Pay_Mode_ID;
    private string _Payment_Code;
    
    #endregion

	#region "Constructor"
    public bl_gtli_policy_prem_return()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Policy_Prem_Return_ID
    {
        get { return _GTLI_Policy_Prem_Return_ID; }
        set { _GTLI_Policy_Prem_Return_ID = value; }
	}

    public string GTLI_Premium_ID
    {
        get { return _GTLI_Premium_ID; }
        set { _GTLI_Premium_ID = value; }
	}

    public DateTime Return_Date
    {
        get { return _Return_Date; }
        set { _Return_Date = value; }
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

    public string Office_ID
    {
        get { return _Office_ID; }
        set { _Office_ID = value; }
    }

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
    }

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    public int Pay_Mode_ID
    {
        get { return _Pay_Mode_ID; }
        set { _Pay_Mode_ID = value; }
    }

    public string Payment_Code
    {
        get { return _Payment_Code; }
        set { _Payment_Code = value; }
    }

	#endregion

}