using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_master_list
/// </summary>
public class bl_gtli_master_list
{
	#region "Private Variable"

    private string _GTLI_Policy_ID;
    private string _Policy_Number;
    private DateTime _Effective_Date;
    private DateTime _Expiry_Date;
    private double _Sum_Insured;
    private double _Life_Premium;
    private double _TPD_Premium;
    private double _DHC_Premium;
    private string _GTLI_Company;
    private string _Created_Note;
    private string _Created_By;
    private DateTime _Created_On;
    private DateTime _Agreement_date;
    private DateTime _Issue_Date;
    private string _SortDir;
    private string _SortColum;
    private double _Accidental_100Plus_Premium;

    #endregion

	#region "Constructor"
    public bl_gtli_master_list()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Policy_ID
    {
        get { return _GTLI_Policy_ID; }
        set { _GTLI_Policy_ID = value; }
	}

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
	}

    public DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }

    public DateTime Expiry_Date
    {
        get { return _Expiry_Date; }
        set { _Expiry_Date = value; }
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

    public string GTLI_Company
    {
        get { return _GTLI_Company; }
        set { _GTLI_Company = value; }
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

    public DateTime Agreement_date
    {
        get { return _Agreement_date; }
        set { _Agreement_date = value; }
    }

    public DateTime Issue_Date
    {
        get { return _Issue_Date; }
        set { _Issue_Date = value; }
    }


    public string SortColum
    {
        get { return _SortColum; }
        set { _SortColum = value; }
    }

    public string SortDir
    {
        get { return _SortDir; }
        set { _SortDir = value; }
    }

    public double Accidental_100Plus_Premium
    {
        get { return _Accidental_100Plus_Premium; }
        set { _Accidental_100Plus_Premium = value; }
    }

	#endregion

}