using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_payment
/// </summary>
public class bl_policy_payment
{
    #region "Private Variable"

    private string _Policy_Number;
    private string _Last_Name;
    private string _First_Name;
    private string _Gender;
    private int _Prem_Year;
    private int _Prem_Lot;
    private string _Mode;
    private double _Sum_Insure;
    private double _Amount;
    private DateTime _Due_Date;
    private string _Sale_Agent_ID;
    private string _Office_ID;
    private string _Policy_ID;
    private double _Original_Prem;
    private int _Pay_Mode_ID;
    private string _En_Abbr;
    private double _Normal_Prem;
    private DateTime _Normal_Due_Date;

    private string _Policy_Status_Type_ID;
    private int _Product_Type_ID;
    private string _Policy_Prem_Pay_ID;
    private DateTime _Pay_Date;
    private string _Receipt_Num;
    private string _Customer_ID;

    #endregion

    #region "Constructor"
    public bl_policy_payment()
	{

    }
    #endregion "Constructor"

    #region "Public Property"

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

    public int Pay_Mode_ID
    {
        get { return _Pay_Mode_ID; }
        set { _Pay_Mode_ID = value; }
    }

    public double Amount
    {
        get { return _Amount; }
        set { _Amount = value; }
    }

    public DateTime Due_Date
    {
        get { return _Due_Date; }
        set { _Due_Date = value; }
    }

    public string Office_ID
    {
        get { return _Office_ID; }
        set { _Office_ID = value; }
    }

    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
    }

    public double Original_Prem
    {
        get { return _Original_Prem; }
        set { _Original_Prem = value; }
    }

    public string Last_Name
    {
        get { return _Last_Name; }
        set { _Last_Name = value; }
    }

    public string First_Name
    {
        get { return _First_Name; }
        set { _First_Name = value; }
    }

    public string Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
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

    public string Mode
    {
        get { return _Mode; }
        set { _Mode = value; }
    }

    public double Sum_Insure
    {
        get { return _Sum_Insure; }
        set { _Sum_Insure = value; }
    }

    public string En_Abbr
    {
        get { return _En_Abbr; }
        set { _En_Abbr = value; }
    }

    public double Normal_Prem
    {
        get { return _Normal_Prem; }
        set { _Normal_Prem = value; }
    }

    public DateTime Normal_Due_Date
    {
        get { return _Normal_Due_Date; }
        set { _Normal_Due_Date = value; }
    }

    public string Policy_Status_Type_ID
    {
        get { return _Policy_Status_Type_ID; }
        set { _Policy_Status_Type_ID = value; }
    }

    public int Product_Type_ID
    {
        get { return _Product_Type_ID; }
        set { _Product_Type_ID = value; }
    }

    public string Policy_Prem_Pay_ID
    {
        get { return _Policy_Prem_Pay_ID; }
        set { _Policy_Prem_Pay_ID = value; }
    }
    public DateTime Pay_Date
    {
        get { return _Pay_Date; }
        set { _Pay_Date = value; }
    }
    public string Receipt_Num
    {
        get { return _Receipt_Num; }
        set { _Receipt_Num = value; }
    }

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }

    #endregion

}