using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_detail
/// </summary>
public class bl_policy_detail
{

    #region "Constructor"
        public bl_policy_detail()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variable"

    //Policy info
    private string _App_Register_ID;
    private string _App_Number;
    private string _Original_App_Number;
    private DateTime _App_Date;
    private int _Is_Cancelled;
    private int _Is_Underwriting;
    private int _Is_Policy_Issued;
    private int _Is_Clean_Case;
    private int _Result;
    private string _Status_Code;
    private string _Created_By;
    private string _Sale_Agent_ID;
    private string _Sale_Agent_Full_Name;
    private DateTime _Effective_Date;
    private DateTime _Issue_Date;
    private DateTime _Maturity_Date;
    private DateTime _Premium_Payment_Due_Date;
    private DateTime _Expiry_Date;
    private string _Next_Premium_Payment_Due_Date;
    private string _Office_ID;
    private string _Benefit_Note;
    private string _Policy_Number;
    private string _Customer_ID;
    private string _Policy_ID;

    //Customer info
    private string _ID_Card;
    private int _ID_Type;
    private string _First_Name;
    private string _Last_Name;
    private string _Customer_Fullname;
    private string _Gender;
    private DateTime _Birth_Date;
    private string _Nationality;
    private string _Khmer_First_Name;
    private string _Khmer_Last_Name;
    private string _Father_First_Name;
    private string _Father_Last_Name;
    private string _Mother_First_Name;
    private string _Mother_Last_Name;
    private string _Prior_First_Name;
    private string _Prior_Last_Name;
    private string _Customer_Address;
    private string _Country_ID;
    private string _Address1;
    private string _Address2;
    private string _Address3;
    private string _Province;
    private string _Zip_Code;
    private string _Mobile_Phone1;
    private string _Home_Phone1;
    private string _EMail;
    private string _Fax1;


    //Product info  
    private int _Age_Insure;
    private int _Pay_Year;
    private string _Product_ID;
    private int _Pay_Up_To_Age;
    private int _Assure_Year;
    private int _Assure_Up_To_Age;
    private double _User_Sum_Insure;
    private double _System_Sum_Insure;
    private double _User_Premium;
    private double _System_Premium;
    private double _System_Premium_Discount;
    private int _Pay_Mode;
    private string _En_Title;
    private string _Payment_Mode;
    private double _Extra_Premium;
    private double _Total_Premium;
    private double _TPD;
    private string _Kh_Title;

    #endregion

    #region "Public Property"
    //Product info    



    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }

    public string Customer_Address
    {
        get { return _Customer_Address; }
        set { _Customer_Address = value; }
    }


    public string Customer_Fullname
    {
        get { return _Customer_Fullname; }
        set { _Customer_Fullname = value; }
    }

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
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
    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }
    public int Pay_Up_To_Age
    {
        get { return _Pay_Up_To_Age; }
        set { _Pay_Up_To_Age = value; }
    }

    public int Assure_Year
    {
        get { return _Assure_Year; }
        set { _Assure_Year = value; }
    }

    public int Assure_Up_To_Age
    {
        get { return _Assure_Up_To_Age; }
        set { _Assure_Up_To_Age = value; }
    }

    public double User_Sum_Insure
    {
        get { return _User_Sum_Insure; }
        set { _User_Sum_Insure = value; }
    }

    public double System_Sum_Insure
    {
        get { return _System_Sum_Insure; }
        set { _System_Sum_Insure = value; }
    }

    public double User_Premium
    {
        get { return _User_Premium; }
        set { _User_Premium = value; }
    }

    public double System_Premium
    {
        get { return _System_Premium; }
        set { _System_Premium = value; }
    }

    public double System_Premium_Discount
    {
        get { return _System_Premium_Discount; }
        set { _System_Premium_Discount = value; }
    }

    public int Pay_Mode
    {
        get { return _Pay_Mode; }
        set { _Pay_Mode = value; }
    }

    public string En_Title
    {
        get { return _En_Title; }
        set { _En_Title = value; }
    }

    public string Kh_Title
    {
        get { return _Kh_Title; }
        set { _Kh_Title = value; }
    }

    public string Payment_Mode
    {
        get { return _Payment_Mode; }
        set { _Payment_Mode = value; }
    }

    public double Extra_Premium
    {
        get { return _Extra_Premium; }
        set { _Extra_Premium = value; }
    }
    public double Total_Premium
    {
        get { return _Total_Premium; }
        set { _Total_Premium = value; }
    }
    public double TPD
    {
        get { return _TPD; }
        set { _TPD = value; }
    }




    //-------------Customer Info---------------//
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

    public string Nationality
    {
        get { return _Nationality; }
        set { _Nationality = value; }
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

    public string Address1
    {
        get { return _Address1; }
        set { _Address1 = value; }
    }

    public string Address2
    {
        get { return _Address2; }
        set { _Address2 = value; }
    }
    public string Address3
    {
        get { return _Address3; }
        set { _Address3 = value; }
    }

    public string Country_ID
    {
        get { return _Country_ID; }
        set { _Country_ID = value; }
    }

    public string Province
    {
        get { return _Province; }
        set { _Province = value; }
    }

    public string Zip_Code
    {
        get { return _Zip_Code; }
        set { _Zip_Code = value; }
    }
    public string Mobile_Phone1
    {
        get { return _Mobile_Phone1; }
        set { _Mobile_Phone1 = value; }
    }

    public string Home_Phone1
    {
        get { return _Home_Phone1; }
        set { _Home_Phone1 = value; }
    }
    public string EMail
    {
        get { return _EMail; }
        set { _EMail = value; }
    }
    public string Fax1
    {
        get { return _Fax1; }
        set { _Fax1 = value; }
    }






    //-------------App Info---------------//
    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public string App_Number
    {
        get { return _App_Number; }
        set { _App_Number = value; }
    }

    public string Original_App_Number
    {
        get { return _Original_App_Number; }
        set { _Original_App_Number = value; }
    }

    public DateTime App_Date
    {
        get { return _App_Date; }
        set { _App_Date = value; }
    }

    public int Is_Cancelled
    {
        get { return _Is_Cancelled; }
        set { _Is_Cancelled = value; }
    }
    public int Is_Underwriting
    {
        get { return _Is_Underwriting; }
        set { _Is_Underwriting = value; }
    }
    public int Is_Policy_Issued
    {
        get { return _Is_Policy_Issued; }
        set { _Is_Policy_Issued = value; }
    }
    public int Is_Clean_Case
    {
        get { return _Is_Clean_Case; }
        set { _Is_Clean_Case = value; }
    }
    public int Result
    {
        get { return _Result; }
        set { _Result = value; }
    }
    public string Status_Code
    {
        get { return _Status_Code; }
        set { _Status_Code = value; }
    }

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
    }


    public string Sale_Agent_Full_Name
    {
        get { return _Sale_Agent_Full_Name; }
        set { _Sale_Agent_Full_Name = value; }
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

    public DateTime Issue_Date
    {
        get { return _Issue_Date; }
        set { _Issue_Date = value; }
    }

    public DateTime Expiry_Date
    {
        get { return _Expiry_Date; }
        set { _Expiry_Date = value; }
    }

    public DateTime Premium_Payment_Due_Date
    {
        get { return _Premium_Payment_Due_Date; }
        set { _Premium_Payment_Due_Date = value; }
    }

    public string Next_Premium_Payment_Due_Date
    {
        get { return _Next_Premium_Payment_Due_Date; }
        set { _Next_Premium_Payment_Due_Date = value; }
    }

    public string Office_ID
    {
        get { return _Office_ID; }
        set { _Office_ID = value; }
    }


    public string Benefit_Note
    {
        get { return _Benefit_Note; }
        set { _Benefit_Note = value; }
    }

    #endregion

}