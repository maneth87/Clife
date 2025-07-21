using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_report_sale_app_by_status
/// </summary>
public class bl_report_sale_app_by_status
{
	#region "Private Variable"

    private string _App_Register_ID;
    private string _App_No;
    private string _Sale_Agent_ID;
    private string _Agent_Name; 
    private DateTime _App_Date;
    private string _Last_Status;
    private string _Policy_No;
    private double _Premium;
    #endregion

	#region "Constructor"
    public bl_report_sale_app_by_status()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
	}

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
	}

    public string App_No
    {
        get { return _App_No; }
        set { _App_No = value; }
    }

    public string Policy_No
    {
        get { return _Policy_No; }
        set { _Policy_No = value; }
    }

    public string Agent_Name
    {
        get { return _Agent_Name; }
        set { _Agent_Name = value; }
    }

    public DateTime App_Date
    {
        get { return _App_Date; }
        set { _App_Date = value; }
    }

    public string Last_Status
    {
        get { return _Last_Status; }
        set { _Last_Status = value; }
    }

    public double Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }
	#endregion
}