using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_flexi_term_policy_report
/// </summary>
public class bl_flexi_term_policy_report
{
    #region "Private Variable"

    private string _Card_ID;
    private string _Card_Number;
    private string _Policy_Number;
    private string _Customer_Flexi_Term_Number;
    private string _First_Name;
    private string _Last_Name;
    private string _Product_ID;   
    private string _En_Abbr;
    private string _Status_Code;
    private DateTime _Issue_Date;
    private DateTime _Effective_Date;
    private DateTime _Maturity_Date;
    private string  _Pay_Mode;
    private double _Sum_Insure;
    private double _Premium;
    private string _Flexi_Term_Policy_ID;
    private string _Policy_Status_Type_ID;
    private string _Branch;
    private string _Beneficiary_First_Name;
    private string _Beneficiary_Last_Name;
    private string _ID_Card;
    private string _Bank_Number;
    private int _Age_Insure;
    private DateTime _Birth_Date;
    private int _Gender;
    private int _ID_Type;
    private int _Resident;
    private int _Beneficiary_ID_Type;
    private string _Beneficiary_ID_Card;
    private string _Relationship;
    private int _Family_Book;
    private DateTime _Expiry_Date;

    //Extra
    private string _Str_ID_Type;
    private string _Str_Gender;

    #endregion

    #region "Constructor"
    public bl_flexi_term_policy_report()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion

    #region "Public Property"

    public string Card_ID
    {
        get { return _Card_ID; }
        set { _Card_ID = value; }
    }
    
    public string Card_Number
    {
        get { return _Card_Number; }
        set { _Card_Number = value; }
    }


    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

    public string Customer_Flexi_Term_Number
    {
        get { return _Customer_Flexi_Term_Number; }
        set { _Customer_Flexi_Term_Number = value; }
    }


    public string First_Name
    {
        get { return _First_Name; }
        set { _First_Name = value; }
    }

    public string Last_Name
    {
        get { return _Last_Name; }
        set { _Last_Name = value; }
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

    public DateTime Expiry_Date
    {
        get { return _Expiry_Date; }
        set { _Expiry_Date = value; }
    }

    public string Pay_Mode
    {
        get { return _Pay_Mode; }
        set { _Pay_Mode = value; }
    }

    public double Sum_Insure
    {
        get { return _Sum_Insure; }
        set { _Sum_Insure = value; }
    }

    public double Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }

       
    public string Flexi_Term_Policy_ID
    {
        get { return _Flexi_Term_Policy_ID; }
        set { _Flexi_Term_Policy_ID = value; }
    }

    public string Policy_Status_Type_ID
    {
        get { return _Policy_Status_Type_ID; }
        set { _Policy_Status_Type_ID = value; }
    }

    public string Branch
    {
        get { return _Branch; }
        set { _Branch = value; }
    }

    public string Beneficiary_First_Name
    {
        get { return _Beneficiary_First_Name; }
        set { _Beneficiary_First_Name = value; }
    }  

     public string Beneficiary_Last_Name
    {
        get { return _Beneficiary_Last_Name; }
        set { _Beneficiary_Last_Name = value; }
    }  

    public string ID_Card
    {
        get { return _ID_Card; }
        set { _ID_Card = value; }
    }  

     public int ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }
    }  

     public string Bank_Number
    {
        get { return _Bank_Number; }
        set { _Bank_Number = value; }
    }  

    public int Age_Insure
    {
        get { return _Age_Insure; }
        set { _Age_Insure = value; }
    }  

    public DateTime Birth_Date
    {
        get { return _Birth_Date; }
        set { _Birth_Date = value; }
    }  

     public int Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }  

      public int Resident
    {
        get { return _Resident; }
        set { _Resident = value; }
    }  

    public int Beneficiary_ID_Type
    {
        get { return _Beneficiary_ID_Type; }
        set { _Beneficiary_ID_Type = value; }
    }
  
     public string Beneficiary_ID_Card
    {
        get { return _Beneficiary_ID_Card; }
        set { _Beneficiary_ID_Card = value; }
    }  

    public string Relationship
    {
        get { return _Relationship; }
        set { _Relationship = value; }
    }  

     public int Family_Book
    {
        get { return _Family_Book; }
        set { _Family_Book = value; }
    } 

    //Extra
     public string Str_ID_Type
     {
         get { return _Str_ID_Type; }
         set { _Str_ID_Type = value; }
     }

     public string Str_Gender
     {
         get { return _Str_Gender; }
         set { _Str_Gender = value; }
     } 
    #endregion
}