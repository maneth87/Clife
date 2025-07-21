using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_employee
/// </summary>
public class bl_gtli_employee
{
	#region "Private Variable"

    private string _GTLI_Certificate_ID;
    private string _Employee_ID;
    private string _Employee_Name;
    private string _Gender;
    private DateTime _DOB;
    private string _Position;
    private Int16 _Customer_Status;
       

    //Extra
    private int _Certificate_Number;
    private string _GTLI_Premium_ID;
    private string _Sum_Insured;

    #endregion

	#region "Constructor"
    public bl_gtli_employee()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Certificate_ID
    {
        get { return _GTLI_Certificate_ID; }
        set { _GTLI_Certificate_ID = value; }
	}

    public string Employee_ID
    {
        get { return _Employee_ID; }
        set { _Employee_ID = value; }
	}

    public string Employee_Name
    {
        get { return _Employee_Name; }
        set { _Employee_Name = value; }
    }

    public string Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public DateTime DOB
    {
        get { return _DOB; }
        set { _DOB = value; }
    }

    public string Position
    {
        get { return _Position; }
        set { _Position = value; }
    }

    public Int16 Customer_Status
    {
        get { return _Customer_Status; }
        set { _Customer_Status = value; }
    }

    //Extra
    public int Certificate_Number
    {
        get { return _Certificate_Number; }
        set { _Certificate_Number = value; }
    }

    public string GTLI_Premium_ID
    {
        get { return _GTLI_Premium_ID; }
        set { _GTLI_Premium_ID = value; }
    }
    
    public string Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
    }

	#endregion

}