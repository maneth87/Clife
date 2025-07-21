using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_product_premium_report
/// </summary>
public class bl_product_premium_report
{
	#region "Private Variable"

        private string _Policy_ID;
        private string _Policy_Number;
        private DateTime _Effective_Date;
        private DateTime _Pay_Date;
        private DateTime _Due_Date;
        private double _Sum_Insure; 
        private string _Product;
        private string _Pay_Mode;
        private double _Amount_Paid;
        private string _Product_ID;
        private int _Prem_Year;
        private int _Prem_Lot;
        private int _Transaction_Type;
        private double _AP; //Annual Premium
        private string _OR_No; //Official Receipt Number
        private string _Prem_Pay_ID;
        private string _Prem_Return_ID;
    #endregion

	#region "Constructor"
        public bl_product_premium_report()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
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

    public DateTime Pay_Date
    {
        get { return _Pay_Date; }
        set { _Pay_Date = value; }
    }

    public DateTime Due_Date
    {
        get { return _Due_Date; }
        set { _Due_Date = value; }
    }

    public double Sum_Insure
    {
        get { return _Sum_Insure; }
        set { _Sum_Insure = value; }
    }

    public string Product
    {
        get { return _Product; }
        set { _Product = value; }
    }

    public string Pay_Mode
    {
        get { return _Pay_Mode; }
        set { _Pay_Mode = value; }
    }

    public double Amount_Paid
    {
        get { return _Amount_Paid; }
        set { _Amount_Paid = value; }
    }

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public int Prem_Year
    {
        get { return _Prem_Year; }
        set { _Prem_Year = value; }
    }

    public int Prem_Lot
    {
        get { return _Prem_Lot; }
        set { _Prem_Lot = value; }
    }

    public int Transaction_Type
    {
        get { return _Transaction_Type; }
        set { _Transaction_Type = value; }
    }

    public double AP
    {
        get { return _AP; }
        set { _AP = value; }
    }

    public string OR_No
    {
        get { return _OR_No; }
        set { _OR_No = value; }
    }

    public string Prem_Return_ID
    {
        get { return _Prem_Return_ID; }
        set { _Prem_Return_ID = value; }
    }

    public string Prem_Pay_ID
    {
        get { return _Prem_Pay_ID; }
        set { _Prem_Pay_ID = value; }
    }

    public int NumberPolicy { get; set; }
	#endregion
}