using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_member_list
/// </summary>
public class bl_gtli_member_list
{
	#region "Private Variable"

    private string _GTLI_Certificate_ID;
	private string _Certificate_Number;
    private string _GTLI_Premium_ID;
	private string _Employee_Name;
    //by maneth
    private string _Gender;
    private System.DateTime _DOB;
    private string _Position;
    private string _EmployeeID;
    //-------
    private string _GTLI_Plan;
    private System.DateTime _Effective_Date;
	private System.DateTime _Delete_Date;
	private System.DateTime _Expiry_Date;
	private int _Days_Not_Cover;
    private int _Days;

    private double _Life_Premium;
    private double _TPD_Premium;
    private double _DHC_Premium;
    private double _Accidental_100Plus_Premium;

	private double _Life_Return_Premium;
	private double _TPD_Return_Premium;
	private double _DHC_Return_Premium;

	private double _Total_Return_Premium;
    private double _Total_Premium;
    private double _Sum_Insured;


	#endregion

	#region "Constructor"
    public bl_gtli_member_list()
	{
	}
	#endregion


	#region "Public Properties"

    public string GTLI_Certificate_ID
    {
        get { return _GTLI_Certificate_ID; }
        set { _GTLI_Certificate_ID = value; }
	}

    public string GTLI_Premium_ID
    {
        get { return _GTLI_Premium_ID; }
        set { _GTLI_Premium_ID = value; }
    }

    public string Certificate_Number
    {
        get { return _Certificate_Number; }
        set { _Certificate_Number = value; }
	}

    public string Employee_Name
    {
        get { return _Employee_Name; }
        set { _Employee_Name = value; }
	}

    //by maneth
    public string Gender {

        get { return _Gender; }
        set { _Gender = value; }
    }
    public DateTime DOB {

        get { return _DOB; }
        set { _DOB = value; }
    }
    public string Position {

        get { return _Position; }
        set { _Position = value; }
    }
    public string EmployeeID {

        get { return _EmployeeID; }
        set { _EmployeeID = value; }
    }
    //--------

    public string GTLI_Plan
    {
        get { return _GTLI_Plan; }
        set { _GTLI_Plan = value; }
	}

    public System.DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }

    public System.DateTime Delete_Date
    {
        get { return _Delete_Date; }
        set { _Delete_Date = value; }
	}

    public System.DateTime Expiry_Date
    {
        get { return _Expiry_Date; }
        set { _Expiry_Date = value; }
	}

    public int Days_Not_Cover
    {
        get { return _Days_Not_Cover; }
        set { _Days_Not_Cover = value; }
	}

    public int Days
    {
        get { return _Days; }
        set { _Days = value; }
    }

    public double Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
    }

    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }

    public double Total_Return_Premium
    {
        get { return _Total_Return_Premium; }
        set { _Total_Return_Premium = value; }
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

    public double Life_Return_Premium
    {
        get { return _Life_Return_Premium; }
        set { _Life_Return_Premium = value; }
    }

    public double TPD_Return_Premium
    {
        get { return _TPD_Return_Premium; }
        set { _TPD_Return_Premium = value; }
    }

    public double DHC_Return_Premium
    {
        get { return _DHC_Return_Premium; }
        set { _DHC_Return_Premium = value; }
    }

    public double Accidental_100Plus_Premium
    {
        get { return _Accidental_100Plus_Premium; }
        set { _Accidental_100Plus_Premium = value; }
    }

	#endregion
}