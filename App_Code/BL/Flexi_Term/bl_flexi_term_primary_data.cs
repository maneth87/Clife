using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_flexi_term_primary_data
/// </summary>
public class bl_flexi_term_primary_data
{
	#region "Private Variable"

    private string _Flexi_Term_Primary_Data_ID;
    private string _Branch;
    private string _Bank_Number;
    private string _First_Name;
    private string _Last_Name;
    private int _ID_Type;
    private string _ID_Card;
    private DateTime _DOB;
    private double _Sum_Insured;
    private double _Premium;
    private int _Age_Insured;
    private DateTime _Effective_Date;
    private DateTime _Expiry_Date;
    private DateTime _Agreement_Date;
    private DateTime _Maturity_Date;
    private DateTime _Issue_Date;
    private int _Pay_Year;
    private int _Pay_Up_To_Age;
    private int _Assure_Year;
    private int _Assure_Up_To_Age;   
    private string _Created_By;
    private DateTime _Created_On;
    private string _Created_Note;
    private string _Channel_Location_ID;
    private string _Channel_Channel_Item_ID;
    private string _Status_Code;

    private DateTime _Updated_On;
    private DateTime _Approved_On;
    private int _Is_Update;
    private string _Updated_By;
    private string _Approved_By;

    private int _Gender;
    private int _Application_Resident;
    private string _Beneficiary_First_Name;
    private string _Beneficiary_Last_Name;
    private int _Beneficiary_ID_Type;
    private string _Beneficiary_ID_Card;
    private string _Beneficiary_Relationship;
    private int _Family_Book;

    //Extra
    private string _Str_ID_Type;
    private string _Company;
    private string _Str_Gender;
    private string _Str_Application_Resident;
    private string _Str_Family_Book;
    private string _Str_Beneficiary_ID_Type;

    #endregion

	#region "Constructor"
    public bl_flexi_term_primary_data()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string Flexi_Term_Primary_Data_ID
    {
        get { return _Flexi_Term_Primary_Data_ID; }
        set { _Flexi_Term_Primary_Data_ID = value; }
	}

    public string Branch
    {
        get { return _Branch; }
        set { _Branch = value; }
    }

    public string Bank_Number
    {
        get { return _Bank_Number; }
        set { _Bank_Number = value; }
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

    public int ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }
    }

    public string ID_Card
    {
        get { return _ID_Card; }
        set { _ID_Card = value; }
    }

    public double Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
    }

    public double Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }

    public DateTime DOB
    {
        get { return _DOB; }
        set { _DOB = value; }
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

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
    }

    public int Age_Insured
    {
        get { return _Age_Insured; }
        set { _Age_Insured = value; }
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

    public DateTime Agreement_Date
    {
        get { return _Agreement_Date; }
        set { _Agreement_Date = value; }
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

    public string Status_Code
    {
        get { return _Status_Code; }
        set { _Status_Code = value; }
    }

    public DateTime Updated_On
    {
        get { return _Updated_On; }
        set { _Updated_On = value; }
    }

    public DateTime Approved_On
    {
        get { return _Approved_On; }
        set { _Approved_On = value; }
    }


    public string Updated_By
    {
        get { return _Updated_By; }
        set { _Updated_By = value; }
    }


    public string Approved_By
    {
        get { return _Approved_By; }
        set { _Approved_By = value; }
    }

    public int Is_Update
    {
        get { return _Is_Update; }
        set { _Is_Update = value; }
    }

    public int Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public int Application_Resident
    {
        get { return _Application_Resident; }
        set { _Application_Resident = value; }
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

    public string Beneficiary_Relationship
    {
        get { return _Beneficiary_Relationship; }
        set { _Beneficiary_Relationship = value; }
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

    public string Company
    {
        get { return _Company; }
        set { _Company = value; }
    }

    public string Str_Gender
    {
        get { return _Str_Gender; }
        set { _Str_Gender = value; }
    }

    public string Str_Application_Resident
    {
        get { return _Str_Application_Resident; }
        set { _Str_Application_Resident = value; }
    }

    public string Str_Family_Book
    {
        get { return _Str_Family_Book; }
        set { _Str_Family_Book = value; }
    }

    public string Str_Beneficiary_ID_Type
    {
        get { return _Str_Beneficiary_ID_Type; }
        set { _Str_Beneficiary_ID_Type = value; }
    }

   

	#endregion
}