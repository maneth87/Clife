using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_transaction_history
/// </summary>
public class bl_transaction_history
{
	#region "Private Variable"
	private string _GTLI_Policy_ID;
    private string _GTLI_Premium_ID;
	private string _Company_Name;
	private string _GTLI_Plan;
	private int _Transaction_Staff_Number;
	private Int16 _Transaction_Type;
	private double _Sum_Insured;
	private double _Life_Premium;
	private double _TPD_Premium;
	private double _DHC_Premium;
	private double _Total_Premium;
	private System.DateTime _Effective_Date;
	private System.DateTime _Expiry_Date;	
	private System.DateTime _Created_On;
    private double _Accidental_100Plus_Premium;

    #endregion

    #region "Constructor"
    public bl_transaction_history()
	{
	}
	#endregion


	#region "Public Properties"

    public string GTLI_Policy_ID
    {
        get { return _GTLI_Policy_ID; }
        set { _GTLI_Policy_ID = value; }
	}

    public string GTLI_Premium_ID
    {
        get { return _GTLI_Premium_ID; }
        set { _GTLI_Premium_ID = value; }
    }

    public string GTLI_Plan
    {
        get { return _GTLI_Plan; }
        set { _GTLI_Plan = value; }
	}

    public string Company_Name
    {
        get { return _Company_Name; }
        set { _Company_Name = value; }
	}

    public int Transaction_Staff_Number
    {
        get { return _Transaction_Staff_Number; }
        set { _Transaction_Staff_Number = value; }
	}

    public Int16 Transaction_Type
    {
        get { return _Transaction_Type; }
        set { _Transaction_Type = value; }
	}

    public double Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
	}

    public double Life_Premium
    {
        get { return _Life_Premium; }
        set { _Life_Premium = value; }
	}

    public double TPD_Premium
    {
        get { return _TPD_Premium; }
        set { _TPD_Premium = value; }
	}

    public double DHC_Premium
    {
        get { return _DHC_Premium; }
        set { _DHC_Premium = value; }
	}

    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
	}

    public System.DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
	}

    public System.DateTime Expiry_Date
    {
        get { return _Expiry_Date; }
        set { _Expiry_Date = value; }
	}

    public System.DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
	}

    public double Accidental_100Plus_Premium
    {
        get { return _Accidental_100Plus_Premium; }
        set { _Accidental_100Plus_Premium = value; }
    }

	#endregion
}