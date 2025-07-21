using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro_report
/// </summary>
public class bl_policy_micro_report
{
    #region "Private Variable"

    private string _Barcode;
    private string _Card_Number;
    private string _Payment_Code;
    private string _Policy_Number;
    private string _Customer_Number;
    private string _Customer_Name;
    private string _Product_ID;   
    private string _En_Abbr;
    private string _Status_Code;
    private DateTime _Issue_Date;
    private DateTime _Effective_Date;
    private DateTime _Maturity_Date;
    private string _Mode;
    private double _User_Sum_Insure;
    private double _User_Premium;
    private string _Is_Standard;
    private string _Policy_Micro_ID;
    private string _Policy_Status_Type_ID;
    private string _Agent;
    private string _Channel_Location_ID;
    private string _Channel_Location;
    private string _Customer_Name_Khmer;

    #endregion

    #region "Constructor"
    public bl_policy_micro_report()
    {

    }
    #endregion

    #region "Public Property"

    public string Barcode
    {
        get { return _Barcode; }
        set { _Barcode = value; }
    }
    
    public string Card_Number
    {
        get { return _Card_Number; }
        set { _Card_Number = value; }
    }

    public string Payment_Code
    {
        get { return _Payment_Code; }
        set { _Payment_Code = value; }
    }

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

    public string Customer_Number
    {
        get { return _Customer_Number; }
        set { _Customer_Number = value; }
    }


    public string Customer_Name
    {
        get { return _Customer_Name; }
        set { _Customer_Name = value; }
    }

    public string Customer_Name_Khmer
    {
        get { return _Customer_Name_Khmer; }
        set { _Customer_Name_Khmer = value; }
    }

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }
       
    public string En_Abbr
    {
        get { return _En_Abbr; }
        set { _En_Abbr = value; }
    }

    public string Status_Code
    {
        get { return _Status_Code; }
        set { _Status_Code = value; }
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

    public DateTime Maturity_Date
    {
        get { return _Maturity_Date; }
        set { _Maturity_Date = value; }
    }
   
    public string Mode
    {
        get { return _Mode; }
        set { _Mode = value; }
    }

    public double User_Sum_Insure
    {
        get { return _User_Sum_Insure; }
        set { _User_Sum_Insure = value; }
    }

    public double User_Premium
    {
        get { return _User_Premium; }
        set { _User_Premium = value; }
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
      
    public string Policy_Micro_ID
    {
        get { return _Policy_Micro_ID; }
        set { _Policy_Micro_ID = value; }
    }

    public string Policy_Status_Type_ID
    {
        get { return _Policy_Status_Type_ID; }
        set { _Policy_Status_Type_ID = value; }
    }

    public string Channel_Location_ID
    {
        get { return _Channel_Location_ID; }
        set { _Channel_Location_ID = value; }
    }

    public string Channel_Location
    {
        get { return _Channel_Location; }
        set { _Channel_Location = value; }
    }  
    #endregion
}