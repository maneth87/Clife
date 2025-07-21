using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_plan
/// </summary>
public class bl_gtli_plan
{
	#region "Private Variable"
	private string _GTLI_Plan_ID;
	private string _GTLI_Plan;
	private double _Sum_Insured;
	private double _TPD;
    private double _Accidental_100Plus;
	private int _DHC_Option_Value;
    private int _TPD_Option_Value;
    private int _Accidental_100Plus_Option_Value;

	#endregion

	#region "Constructor"
    public bl_gtli_plan()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Plan_ID
    {
        get { return _GTLI_Plan_ID; }
        set { _GTLI_Plan_ID = value; }
	}

    public string GTLI_Plan
    {
        get { return _GTLI_Plan; }
        set { _GTLI_Plan = value; }
	}

    public double Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
	}      

    public int DHC_Option_Value
    {
        get { return _DHC_Option_Value; }
        set { _DHC_Option_Value = value; }
	}

    public int TPD_Option_Value
    {
        get { return _TPD_Option_Value; }
        set { _TPD_Option_Value = value; }
    }

    public double TPD
    {
        get { return _TPD; }
        set { _TPD = value; }
	}

    public double Accidental_100Plus
    {
        get { return _Accidental_100Plus; }
        set { _Accidental_100Plus = value; }
    }

    public int Accidental_100Plus_Option_Value
    {
        get { return _Accidental_100Plus_Option_Value; }
        set { _Accidental_100Plus_Option_Value = value; }
    }

	#endregion
}