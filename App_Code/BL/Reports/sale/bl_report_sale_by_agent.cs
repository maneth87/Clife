using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_report_sale_by_agent
/// </summary>
public class bl_report_sale_by_agent
{
	#region "Private Variable"

    private string _Sale_Agent_ID;
    private string _Manager_Name;
    private string _Manager_Code;
    private string _LIC_Name;
    private string _LIC_Code;
    private int _App_Submit_Count; //Monthly SBMT (SBMT = Submit)
    private int _Issue_Count; //Monthly ISSUE
    private double _API; //Monthly API (API = Annual Premium Income)
    private double _App_Submit_FYP; //Monthly SBMT FYP (FYP = First Year Policy)
    private double _Issue_FYP; //Monthly ISSUE FYP (FYP = First Year Policy)  
    private int _OS; // App Count in Underwriting Process
    private int _Decline; // App Decline Count
    private int _Postpone; // App Postpone Count
    private int _Cancel; // App Cancel Count
    #endregion

	#region "Constructor"
    public bl_report_sale_by_agent()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
	}

    public string Manager_Name
    {
        get { return _Manager_Name; }
        set { _Manager_Name = value; }
	}

    public string Manager_Code
    {
        get { return _Manager_Code; }
        set { _Manager_Code = value; }
    }

    public string LIC_Name
    {
        get { return _LIC_Name; }
        set { _LIC_Name = value; }
    }

    public string LIC_Code
    {
        get { return _LIC_Code; }
        set { _LIC_Code = value; }
    }

    public int App_Submit_Count
    {
        get { return _App_Submit_Count; }
        set { _App_Submit_Count = value; }
    }

    public int Issue_Count
    {
        get { return _Issue_Count; }
        set { _Issue_Count = value; }
    }

    public double API
    {
        get { return _API; }
        set { _API = value; }
    }

    public double App_Submit_FYP
    {
        get { return _App_Submit_FYP; }
        set { _App_Submit_FYP = value; }
    }

    public double Issue_FYP
    {
        get { return _Issue_FYP; }
        set { _Issue_FYP = value; }
    }

    public int Postpone
    {
        get { return _Postpone; }
        set { _Postpone = value; }
    }

    public int OS
    {
        get { return _OS; }
        set { _OS = value; }
    }

    public int Decline
    {
        get { return _Decline; }
        set { _Decline = value; }
    }

    public int Cancel
    {
        get { return _Cancel; }
        set { _Cancel = value; }
    }

	#endregion
}