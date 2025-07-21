using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_cmk_policy_report
/// </summary>
public class bl_cmk_policy_report
{
    #region Constructor
    public bl_cmk_policy_report()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #endregion

    #region "Private Variable"

    #region Customer 
    private string _Customer_ID;
    private string _ID_Card;
    private int _ID_Type;
    private string _First_Name;
    private string _Last_Name;
    private string _Gender;
    private DateTime _Birth_Date;
    private string _Country_ID;
    private string _Khmer_First_Name;
    private string _Khmer_Last_Name;
    private string _Father_First_Name;
    private string _Father_Last_Name;
    private string _Mother_First_Name;
    private string _Mother_Last_Name;
    private string _Prior_First_Name;
    private string _Prior_Last_Name;
    #endregion

    #region Policy
    private string _CMK_Policy_ID;
    private string _CMK_Customer_ID;
    private string _Certificate_No;
    private string _Loan_ID;
    private string _Loan_Type;
    private string _Group;
    private string _Product_ID;
    private DateTime _Opened_Date;
    private DateTime _Effective_Date;
    private DateTime _Date_Of_Entry;
    private DateTime _Expire_Date;
    private string _Policy_Status;
    private string _Currancy;
    private int _Age;
    private int _LoanDuration;
    private int _Covered_Year;
    private string _Branch;
    private string _Channel_Location_ID;
    private string _Channel_Channel_Item_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion

    #region POLICY Premium

    private string _CMK_Policy_PremiumID;
    private double _Loan_Amount;
    private double _Loan_Amount_Riel;
    private double _Outstanding_Balance;
    private double _Outstanding_Balance_Riel;
    private double _Assured_Amount;
    private double _Assured_Amount_Riel;
    private double _Monthly_Premium;
    private double _Extra_Premium;
    private double _Discount_Amount;
    private double _Premium_After_Discount;
    private double _Total_Premium;
    private DateTime _Report_Date;
    private string _Pay_Mode_ID;
    private DateTime _Paid_Off_In_Month;
    private DateTime _Terminate_Date;
    private string _Payment_Batch_No;

    #endregion

    #endregion

    #region "Public Property"

    #region Customer 
    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
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

    public string Country_ID
    {
        get { return _Country_ID; }
        set { _Country_ID = value; }
    }

    public string Khmer_First_Name
    {
        get { return _Khmer_First_Name; }
        set { _Khmer_First_Name = value; }
    }

    public string Khmer_Last_Name
    {
        get { return _Khmer_Last_Name; }
        set { _Khmer_Last_Name = value; }
    }

    public string Father_First_Name
    {
        get { return _Father_First_Name; }
        set { _Father_First_Name = value; }
    }

    public string Father_Last_Name
    {
        get { return _Father_Last_Name; }
        set { _Father_Last_Name = value; }
    }

    public string Mother_First_Name
    {
        get { return _Mother_First_Name; }
        set { _Mother_First_Name = value; }
    }

    public string Mother_Last_Name
    {
        get { return _Mother_Last_Name; }
        set { _Mother_Last_Name = value; }
    }

    public string Prior_First_Name
    {
        get { return _Prior_First_Name; }
        set { _Prior_First_Name = value; }
    }

    public string Prior_Last_Name
    {
        get { return _Prior_Last_Name; }
        set { _Prior_Last_Name = value; }
    }

    #endregion

    #region Policy
    public string CMK_Policy_ID
    {
        get { return _CMK_Policy_ID; }
        set { _CMK_Policy_ID = value; }
    }

    public string CMK_Customer_ID
    {
        get { return _CMK_Customer_ID; }
        set { _CMK_Customer_ID = value; }
    }

    public string Certificate_No
    {
        get { return _Certificate_No; }
        set { _Certificate_No = value; }
    }

    public string Loan_ID
    {
        get { return _Loan_ID; }
        set { _Loan_ID = value; }
    }

    public string Loan_Type
    {
        get { return _Loan_Type; }
        set { _Loan_Type = value; }
    }

    public string Group
    {
        get { return _Group; }
        set { _Group = value; }
    }

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public DateTime Opened_Date
    {
        get { return _Opened_Date; }
        set { _Opened_Date = value; }
    }

    public DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }

    public string Policy_Status
    {
        get { return _Policy_Status; }
        set { _Policy_Status = value; }
    }

    public DateTime Date_Of_Entry
    {
        get { return _Date_Of_Entry; }
        set { _Date_Of_Entry = value; }
    }

    public DateTime Expire_Date
    {
        get { return _Expire_Date; }
        set { _Expire_Date = value; }
    }

    public string Currancy
    {
        get { return _Currancy; }
        set { _Currancy = value; }
    }

    public int Age
    {
        get { return _Age; }
        set { _Age = value; }
    }

    public int LoanDuration
    {
        get { return _LoanDuration; }
        set { _LoanDuration = value; }
    }

    public int Covered_Year
    {
        get { return _Covered_Year; }
        set { _Covered_Year = value; }
    }

    public string Branch
    {
        get { return _Branch; }
        set { _Branch = value; }
    }

    public DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
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

    #endregion

    #region Policy Premium

    public string CMK_Policy_PremiumID
    {
        get { return _CMK_Policy_PremiumID; }
        set { _CMK_Policy_PremiumID = value; }
    }

    public double Loan_Amount
    {
        get { return _Loan_Amount; }
        set { _Loan_Amount = value; }
    }

    public double Loan_Amount_Riel
    {
        get { return _Loan_Amount_Riel; }
        set { _Loan_Amount_Riel = value; }
    }

    public double Outstanding_Balance
    {
        get { return _Outstanding_Balance; }
        set { _Outstanding_Balance = value; }
    }

    public double Outstanding_Balance_Riel
    {
        get { return _Outstanding_Balance_Riel; }
        set { _Outstanding_Balance_Riel = value; }
    }

    public double Assured_Amount
    {
        get { return _Assured_Amount; }
        set { _Assured_Amount = value; }
    }

    public double Assured_Amount_Riel
    {
        get { return _Assured_Amount_Riel; }
        set { _Assured_Amount_Riel = value; }
    }

    public double Monthly_Premium
    {
        get { return _Monthly_Premium; }
        set { _Monthly_Premium = value; }
    }

    public double Extra_Premium
    {
        get { return _Extra_Premium; }
        set { _Extra_Premium = value; }
    }

    public double Discount_Amount
    {
        get { return _Discount_Amount; }
        set { _Discount_Amount = value; }
    }

    public double Premium_After_Discount
    {
        get { return _Premium_After_Discount; }
        set { _Premium_After_Discount = value; }
    }

    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }

    public DateTime Report_Date
    {
        get { return _Report_Date; }
        set { _Report_Date = value; }
    }

    public string Pay_Mode_ID
    {
        get { return _Pay_Mode_ID; }
        set { _Pay_Mode_ID = value; }
    }

    public DateTime Paid_Off_In_Month
    {
        get { return _Paid_Off_In_Month; }
        set { _Paid_Off_In_Month = value; }
    }

    public DateTime Terminate_Date
    {
        get { return _Terminate_Date; }
        set { _Terminate_Date = value; }
    }

    public string Payment_Batch_No
    {
        get { return _Payment_Batch_No; }
        set { _Payment_Batch_No = value; }
    }

    #endregion

    #endregion
}