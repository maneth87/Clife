using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_single_row_data
/// </summary>
public class bl_app_single_row_data
{
	#region "Private Variable"

    private string _App_Register_ID;
    private string _App_Number;
    private string _Payment_Code;
    private string _Channel_ID;
    private string _Channel_Item_ID;
    private string _Office_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private DateTime _App_Date;
    private string _Sale_Agent_ID;
    private string _Benefit_Note;
    private string _Country;
    private string _Address1;
    private string _Address2;
    private string _Province;
    private string _Zip_Code;
    private int _Weight;
    private int _Height;
    private int _Is_Weight_Changed;
    private string _Reason;
    private int _Weight_Change;
    private string _Mobile_Phone1;
    private string _Home_Phone1;
    private string _Fax1;
    private string _EMail;
    private string _ID_Card;
    private int _ID_Type;
    private string _First_Name;
    private string _Last_Name;
    private int _Gender;
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
    private string _Employer_Name;
    private string _Nature_Of_Business;
    private string _Current_Position;
    private string _Job_Role;
    private double _Anual_Income;
    private string _Product_ID;
    private int _Age_Insure;
    private int _Pay_Year;
    private int _Pay_Up_To_Age;
    private int _Assure_Year;
    private int _Assure_Up_To_Age;
    private double _User_Sum_Insure;
    private double _System_Sum_Insure;
    private double _User_Premium;
    private double _System_Premium;
    private double _System_Premium_Discount;
    private int _Pay_Mode;
    private double _Original_Amount;
    private string _Policy_Number;
    private string _Underwriting_Status;

    private double _Interest_Rate;
    private DateTime _Loan_Effiective_Date;
    private int _Loan_Type;
    private double _Out_Std_Loan;
    private int _Term_Year;

    private int _Is_Pre_Premium_Discount;

    //Discount on mode
    private double _Discount_Amount;
    private string _Insurance_Plan_Note;
  
    //Extra
    private string _Sale_Agent_Full_Name;
    private int _Product_Type_ID;
    #endregion


    #region "Constructor"
    public bl_app_single_row_data()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public string Payment_Code
    {
        get { return _Payment_Code; }
        set { _Payment_Code = value; }
    }

    public string Channel_ID
    {
        get { return _Channel_ID; }
        set { _Channel_ID = value; }
    }

    public string Channel_Item_ID
    {
        get { return _Channel_Item_ID; }
        set { _Channel_Item_ID = value; }
    }

    public string App_Number
    {
        get { return _App_Number; }
        set { _App_Number = value; }
    }
   
    public string Office_ID
    {
        get { return _Office_ID; }
        set { _Office_ID = value; }
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

    public DateTime App_Date
    {
        get { return _App_Date; }
        set { _App_Date = value; }
    }

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
    }

    public string Benefit_Note
    {
        get { return _Benefit_Note; }
        set { _Benefit_Note = value; }
    }

    public string Country
    {
        get { return _Country; }
        set { _Country = value; }
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

    public int Weight
    {
        get { return _Weight; }
        set { _Weight = value; }
    }

    public int Height
    {
        get { return _Height; }
        set { _Height = value; }
    }

    public int Is_Weight_Changed
    {
        get { return _Is_Weight_Changed; }
        set { _Is_Weight_Changed = value; }
    }

    public string Reason
    {
        get { return _Reason; }
        set { _Reason = value; }
    }

    public int Weight_Change
    {
        get { return _Weight_Change; }
        set { _Weight_Change = value; }
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

    public string Fax1
    {
        get { return _Fax1; }
        set { _Fax1 = value; }
    }

    public string EMail
    {
        get { return _EMail; }
        set { _EMail = value; }
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

    public int Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public DateTime  Birth_Date
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

    public string Employer_Name
    {
        get { return _Employer_Name; }
        set { _Employer_Name = value; }
    }

    public string Nature_Of_Business
    {
        get { return _Nature_Of_Business; }
        set { _Nature_Of_Business = value; }
    }

    public string Current_Position
    {
        get { return _Current_Position; }
        set { _Current_Position = value; }
    }

    public string Job_Role
    {
        get { return _Job_Role; }
        set { _Job_Role = value; }
    }

    public double Anual_Income
    {
        get { return _Anual_Income; }
        set { _Anual_Income = value; }
    }

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
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

    public double Original_Amount
    {
        get { return _Original_Amount; }
        set { _Original_Amount = value; }
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

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

    public string Underwriting_Status
    {
        get { return _Underwriting_Status; }
        set { _Underwriting_Status = value; }
    }

    public double Out_Std_Loan
    {
        get { return _Out_Std_Loan; }
        set { _Out_Std_Loan = value; }
    }

    public int Loan_Type
    {
        get { return _Loan_Type; }
        set { _Loan_Type = value; }
    }

    public DateTime Loan_Effiective_Date
    {
        get { return _Loan_Effiective_Date; }
        set { _Loan_Effiective_Date = value; }
    }

    public double Interest_Rate
    {
        get { return _Interest_Rate; }
        set { _Interest_Rate = value; }
    }

    public int Term_Year
    {
        get { return _Term_Year; }
        set { _Term_Year = value; }
    }

    public double Discount_Amount
    {
        get { return _Discount_Amount; }
        set { _Discount_Amount = value; }
    }

    public string Insurance_Plan_Note
    {
        get { return _Insurance_Plan_Note; }
        set { _Insurance_Plan_Note = value; }
    }

    public int Is_Pre_Premium_Discount
    {
        get { return _Is_Pre_Premium_Discount; }
        set { _Is_Pre_Premium_Discount = value; }
    }

    //Extra

    public string Sale_Agent_Full_Name
    {
        get { return _Sale_Agent_Full_Name; }
        set { _Sale_Agent_Full_Name = value; }
    }

    public int Product_Type_ID
    {
        get { return _Product_Type_ID; }
        set { _Product_Type_ID = value; }
    }
    #endregion
} 