using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_policy_search
/// </summary>
public class bl_gtli_policy_search
{
	#region "Private Variable"
    private string _GTLI_Policy_ID;
    private string _Policy_Number;
    private System.DateTime _Effective_Date;
    private System.DateTime _Expiry_Date;
    private System.DateTime _Created_On;
    private double _Sum_Insured;
    private double _Life_Premium;
    private double _TPD_Premium;
    private double _DHC_Premium;	
    private string _Mode_Of_Payment;
    private string _Product_ID;
    private string _GTLI_Company_ID;
    private string _Created_By;	
	private int _Total_Staff;
    private int _Transaction_Type;
    private int _Transaction_Staff_Number;
    private string _Sale_Agent_ID;
    private int _Policy_Year;
    private string _GTLI_Contact_ID;
    private int _DHC_Option_Value;
    private string _GTLI_Plan_ID;
    private string _GTLI_Premium_ID;
    private double _Accidental_100Plus_Premium;

	//Extra
    private string _Employee_Name;
    private string _Company_Name;
    private string _Type_Of_Business;
    private string _DHC_Option;
    private string _GTLI_Plan;
	private System.DateTime _From_Date;
	private System.DateTime _To_Date;
    private double _Life_Premium_Return;
    private double _TPD_Premium_Return;
    private double _DHC_Premium_Return;
    private double _Total_Premium;
    private int _Date_Type;

	#endregion

	#region "Constructor"
    public bl_gtli_policy_search()
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

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
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

    public double Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
    }

    public double DHC_Premium
    {
        get { return _DHC_Premium; }
        set { _DHC_Premium = value; }
    }

    public double TPD_Premium
    {
        get { return _TPD_Premium; }
        set { _TPD_Premium = value; }
    }

    public double Life_Premium
    {
        get { return _Life_Premium; }
        set { _Life_Premium = value; }
    }
    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
	}

    public string GTLI_Company_ID
    {
        get { return _GTLI_Company_ID; }
        set { _GTLI_Company_ID = value; }
	}

    public string GTLI_Plan_ID
    {
        get { return _GTLI_Plan_ID; }
        set { _GTLI_Plan_ID = value; }
	}

    public string GTLI_Contact_ID
    {
        get { return _GTLI_Contact_ID; }
        set { _GTLI_Contact_ID = value; }
	}

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
	}

    public string Mode_Of_Payment
    {
        get { return _Mode_Of_Payment; }
        set { _Mode_Of_Payment = value; }
	}     		   

    public int Total_Staff
    {
        get { return _Total_Staff; }
        set { _Total_Staff = value; }
	}

    public int Transaction_Type
    {
        get { return _Transaction_Type; }
        set { _Transaction_Type = value; }
	}

    public int Transaction_Staff_Number
    {
        get { return _Transaction_Staff_Number; }
        set { _Transaction_Staff_Number = value; }
	}

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
	}

    public int Policy_Year
    {
        get { return _Policy_Year; }
        set { _Policy_Year = value; }
	}

    public int DHC_Option_Value
    {
        get { return _DHC_Option_Value; }
        set { _DHC_Option_Value = value; }
	}

    public double Accidental_100Plus_Premium
    {
        get { return _Accidental_100Plus_Premium; }
        set { _Accidental_100Plus_Premium = value; }
    }

	//Extra 
    public double Life_Premium_Return
    {
        get { return _Life_Premium_Return; }
        set { _Life_Premium_Return = value; }
    }

    public double TPD_Premium_Return
    {
        get { return _TPD_Premium_Return; }
        set { _TPD_Premium_Return = value; }
    }

    public double DHC_Premium_Return
    {
        get { return _DHC_Premium_Return; }
        set { _DHC_Premium_Return = value; }
    }

    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }
    
    public string Employee_Name
    {
        get { return _Employee_Name; }
        set { _Employee_Name = value; }
    }

    public string Company_Name
    {
        get { return _Company_Name; }
        set { _Company_Name = value; }
    }

    public string Type_Of_Business
    {
        get { return _Type_Of_Business; }
        set { _Type_Of_Business = value; }
    }

    public string DHC_Option
    {
        get { return _DHC_Option; }
        set { _DHC_Option = value; }
    }

	private string _SortColum;
	public string SortColum {
		get { return _SortColum; }
		set { _SortColum = value; }
	}

	private string _SortDir;
	public string SortDir {
		get { return _SortDir; }
		set { _SortDir = value; }
	}

	public System.DateTime From_Date {
		get { return _From_Date; }
		set { _From_Date = value; }
	}

	public System.DateTime To_Date {
		get { return _To_Date; }
		set { _To_Date = value; }
	}

	public string GTLI_Plan {
		get { return _GTLI_Plan; }
		set { _GTLI_Plan = value; }
	}

    public int Date_Type
    {
        get { return _Date_Type; }
        set { _Date_Type = value; }
    }

	#endregion
}