using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_employee_premium
/// </summary>
public class bl_gtli_employee_premium
{
	#region "Private Variable"

    private string _GTLI_Employee_Premium_ID;
    private string _GTLI_Certificate_ID;
    private string _GTLI_Premium_ID;
    private string _Premium;
    private string _Premium_Type;
    private string _Sum_Insured;

    #endregion

	#region "Constructor"
    public bl_gtli_employee_premium()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Employee_Premium_ID
    {
        get { return _GTLI_Employee_Premium_ID; }
        set { _GTLI_Employee_Premium_ID = value; }
	}

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

    public string Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }

    public string Premium_Type
    {
        get { return _Premium_Type; }
        set { _Premium_Type = value; }
    }

    public string Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
    }

	#endregion

}