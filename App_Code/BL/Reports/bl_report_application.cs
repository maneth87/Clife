using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for report_application
/// </summary>
public class bl_report_application
{
    private string _App_Number;
    private string _customer_name;
    private string _App_Date;
    private double _User_Sum_Insure;
    private double _User_Premium;
    private string _En_Abbr;
    private string _Status_Code;
    private string _Status_date;
    private string _agent_name;

    #region "Constructor"
    public bl_report_application()
	{

    }
    #endregion

    #region "Public Property"

    public string App_Number
    {
        get { return _App_Number; }
        set { _App_Number = value; }
    }

    public string customer_name
    {
        get { return _customer_name; }
        set { _customer_name = value; }
    }

    public string App_Date
    {
        get { return _App_Date; }
        set { _App_Date = value; }
    }

    public double User_Sum_Insure
    {
        get { return _User_Sum_Insure; }
        set { _User_Sum_Insure = value; }
    }

    public double User_Premium
    {
        get { return _User_Premium; }
        set { _User_Premium = value; }
    }

    public string En_Abbr
    {
        get { return _En_Abbr; }
        set { _En_Abbr = value; }
    }

    public string Status_Code
    {
        get { return _Status_Code; }
        set { _Status_Code = value; }
    }

    public string Status_date
    {
        get { return _Status_date; }
        set { _Status_date = value; }
    }

    public string agent_name
    {
        get { return _agent_name; }
        set { _agent_name = value; }
    }

    #endregion
}