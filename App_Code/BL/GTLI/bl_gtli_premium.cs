using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_premium
/// </summary>
public class bl_gtli_premium
{
	#region "Private Variable"

    private string _GTLI_Premium_ID;
    private string _GTLI_Policy_ID;
    private DateTime _Effective_Date;
    private DateTime _Expiry_Date;
    private double _Sum_Insured;
    private double _Life_Premium;
    private double _TPD_Premium;
    private double _DHC_Premium;
    private int _Pay_Mode_ID;
    private Int16 _Transaction_Type;
    private int _Transaction_Staff_Number;
    private string _Sale_Agent_ID;
    private int _Policy_Year;
    private string _GTLI_Plan_ID;
    private int _DHC_Option_Value;
    private string _Channel_Location_ID;
    private string _Channel_Channel_Item_ID;
    private string _Created_By;
    private DateTime _Created_On;
    private int _User_Total_Staff_Number;

    private double _Discount;
    private double _Accidental_100Plus_Premium;
    private double _Accidental_100Plus_Premium_Discount;
    private double _Life_Premium_Discount;
    private double _TPD_Premium_Discount;
    private double _DHC_Premium_Discount;

    private double _Original_Accidental_100Plus_Premium;
    private double _Original_Life_Premium;
    private double _Original_TPD_Premium;
    private double _Original_DHC_Premium;
    private double _Accidental_100Plus_Premium_Tax_Amount;
    private double _Life_Premium_Tax_Amount;
    private double _TPD_Premium_Tax_Amount;
    private double _DHC_Premium_Tax_Amount;

    private string _GTLI_Plan;

    //Extra
    private double _Total_Premium;
    private string _Company;
    private string _Policy_Number;

    #endregion

	#region "Constructor"
    public bl_gtli_premium()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Premium_ID
    {
        get { return _GTLI_Premium_ID; }
        set { _GTLI_Premium_ID = value; }
	}

    public string GTLI_Policy_ID
    {
        get { return _GTLI_Policy_ID; }
        set { _GTLI_Policy_ID = value; }
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

    public int Pay_Mode_ID
    {
        get { return _Pay_Mode_ID; }
        set { _Pay_Mode_ID = value; }
    }

    public Int16 Transaction_Type
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

    public string GTLI_Plan_ID
    {
        get { return _GTLI_Plan_ID; }
        set { _GTLI_Plan_ID = value; }

    }

    public int DHC_Option_Value
    {
        get { return _DHC_Option_Value; }
        set { _DHC_Option_Value = value; }

    }

    public string Channel_Location_ID
    {
        get { return _Channel_Location_ID; }
        set { _Channel_Location_ID = value; }

    }

    public string Channel_Channel_Item_ID
    {
        get { return _Channel_Channel_Item_ID; }
        set { _Channel_Channel_Item_ID = value; }

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

    public int User_Total_Staff_Number
    {
        get { return _User_Total_Staff_Number; }
        set { _User_Total_Staff_Number = value; }
    }

    //Extra
    public string Company
    {
        get { return _Company; }
        set { _Company = value; }
    }

    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

    public string GTLI_Plan
    {
        get { return _GTLI_Plan; }
        set { _GTLI_Plan = value; }
    }
    
    public double Accidental_100Plus_Premium
    {
        get { return _Accidental_100Plus_Premium; }
        set { _Accidental_100Plus_Premium = value; }
    }
    
    public double Accidental_100Plus_Premium_Discount
    {
        get { return _Accidental_100Plus_Premium_Discount; }
        set { _Accidental_100Plus_Premium_Discount = value; }
    }

    public double Life_Premium_Discount
    {
        get { return _Life_Premium_Discount; }
        set { _Life_Premium_Discount = value; }
    }

    public double TPD_Premium_Discount
    {
        get { return _TPD_Premium_Discount; }
        set { _TPD_Premium_Discount = value; }
    }

    public double DHC_Premium_Discount
    {
        get { return _DHC_Premium_Discount; }
        set { _DHC_Premium_Discount = value; }
    }

    public double Discount
    {
        get { return _Discount; }
        set { _Discount = value; }
    }

    public double Original_Accidental_100Plus_Premium
    {
        get { return _Original_Accidental_100Plus_Premium; }
        set { _Original_Accidental_100Plus_Premium = value; }
    }

    public double Original_Life_Premium
    {
        get { return _Original_Life_Premium; }
        set { _Original_Life_Premium = value; }
    }

    public double Original_TPD_Premium
    {
        get { return _Original_TPD_Premium; }
        set { _Original_TPD_Premium = value; }
    }

    public double Original_DHC_Premium
    {
        get { return _Original_DHC_Premium; }
        set { _Original_DHC_Premium = value; }
    }

    public double Accidental_100Plus_Premium_Tax_Amount
    {
        get { return _Accidental_100Plus_Premium_Tax_Amount; }
        set { _Accidental_100Plus_Premium_Tax_Amount = value; }
    }

    public double Life_Premium_Tax_Amount
    {
        get { return _Life_Premium_Tax_Amount; }
        set { _Life_Premium_Tax_Amount = value; }
    }

    public double TPD_Premium_Tax_Amount
    {
        get { return _TPD_Premium_Tax_Amount; }
        set { _TPD_Premium_Tax_Amount = value; }
    }

    public double DHC_Premium_Tax_Amount
    {
        get { return _DHC_Premium_Tax_Amount; }
        set { _DHC_Premium_Tax_Amount = value; }
    }

	#endregion

}