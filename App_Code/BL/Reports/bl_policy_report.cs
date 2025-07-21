using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_report
/// </summary>
public class bl_policy_report
{
    #region "Private Variable"
   
    private string _Policy_Number;
    private string _customer_name;
    private string _Product_ID;
    private double _Comm_Rate;
    private string _En_Abbr;
    private string _StatusCode;
    private DateTime _Issue_Date;
    private DateTime _Effective_Date;
    private DateTime _Next_Due_Date;
    private DateTime _Last_Payment;
    private string _Mode;
    private double _Sum_Insure;
    private double _Total_Premium;
    private string _Is_Standard;
    private string _Agent;
    private string _Address1;
    private string _Policy_ID;
    private string _Policy_Status_Type_ID;
  
    //new props
    private string _App_Number;
    private string _Customer_ID;
    private string _Product_Name;
    private string _Gender;
    private DateTime _Birth_Date;
    private int _Age_Insure;
    private int _Pay_Year;
    private int _Assured_Year;
    private double _Premium;
    private double _EM_Amount;
    private double _Discount_Amount;
    private double _OLD_Premium;
    private double _OLD_EM_Amount;

    private string _Round_Status_ID;
    private string _Main_Policy;
    private string _Sub_Policy_Number;
    private DateTime _Maturity_Date;

    
    #endregion

    #region "Constructor"
    public bl_policy_report()
    {

    }
    #endregion

    #region "Public Property"
   
    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

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

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public double Comm_Rate
    {
        get { return _Comm_Rate; }
        set { _Comm_Rate = value; }
    }

    public string En_Abbr
    {
        get { return _En_Abbr; }
        set { _En_Abbr = value; }
    }

    public string StatusCode
    {
        get { return _StatusCode; }
        set { _StatusCode = value; }
    }

    public DateTime Issue_Date
    {
        get { return _Issue_Date; }
        set { _Issue_Date = value; }
    }

    public DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }

    public DateTime Next_Due_Date
    {
        get { return _Next_Due_Date; }
        set { _Next_Due_Date = value; }
    }

    public DateTime Last_Payment
    {
        get { return _Last_Payment; }
        set { _Last_Payment = value; }
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

    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }

     public string Is_Standard
    {
        get { return _Is_Standard; }
        set { _Is_Standard = value; }
    }

    public string Agent
    {
        get { return _Agent; }
        set { _Agent = value; }
    }

    public string Address1
    {
        get { return _Address1; }
        set { _Address1 = value; }
    }

    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }


    public string Policy_Status_Type_ID
    {
        get { return _Policy_Status_Type_ID; }
        set { _Policy_Status_Type_ID = value; }
    }

    public string Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public DateTime Birth_Date 
    {
        get { return _Birth_Date; }
        set { _Birth_Date = value; }
    }

    public int Age_Insure
    {
        get { return _Age_Insure; }
        set { _Age_Insure = value; }
    }

    public int Pay_Year
    {
        get { return _Pay_Year; }
        set { _Pay_Year = value; }
    }

    public int Assured_Year
    {
        get { return _Assured_Year; }
        set { _Assured_Year = value; }
    }

    public double Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }

    public double EM_Amount
    {
        get { return _EM_Amount; }
        set { _EM_Amount = value; }
    }

    public double Discount_Amount
    {
        get { return _Discount_Amount; }
        set { _Discount_Amount = value; }
    }

    public double OLD_EM_Amount
    {
        get { return _OLD_EM_Amount; }
        set { _OLD_EM_Amount = value; }
    }

    public double OLD_Premium
    {
        get { return _OLD_Premium; }
        set { _OLD_Premium = value; }

    }

    public string Round_Status_ID
    {
        get { return _Round_Status_ID; }
        set { _Round_Status_ID = value; }
    }

    public string Main_Policy {
        get { return _Main_Policy; }
        set { _Main_Policy = value; }
    }

    public DateTime Maturity_Date {
        get { return _Maturity_Date; }
        set { _Maturity_Date = value; }
    }

    public string Product_Name
    {
        get { return _Product_Name; }
        set { _Product_Name = value; }
    }

    public string Sub_Policy_Number
    {
        get { return _Sub_Policy_Number; }
        set { _Sub_Policy_Number = value; }
    }

    #endregion
}