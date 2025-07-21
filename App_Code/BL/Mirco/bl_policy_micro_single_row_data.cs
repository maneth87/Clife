using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro_single_row_data
/// </summary>
public class bl_policy_micro_single_row_data
{
	#region "Private Variable"

    private string _Policy_Micro_ID;
    private string _Policy_Number;
    private string _Barcode;
    private string _Card_Number;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private DateTime _Effective_Date;
    private string _Sale_Agent_ID;
 
    private string _Country;
    private string _Address1;
    private string _Address2;
    private string _Province;
    private string _Zip_Code;   
    private string _Mobile_Phone1;


    private string _EMail;
    private string _ID_Card;
    private int _ID_Type;
    private string _First_Name;
    private string _Last_Name;
    private int _Gender;
    private DateTime _Birth_Date;
   
    private string _Khmer_First_Name;
    private string _Khmer_Last_Name;
     
    private string _Product_ID;
    private int _Age_Insure;
    private int _Pay_Year;
   
    private int _Assure_Year;

    private double _User_Sum_Insure;
    
    private double _User_Premium;   
   
    private int _Pay_Mode;
    private int _Marital_Status;
    private DateTime _Issue_Date;
    private string _Channel_Location_ID;
  
    //Extra
    private string _Sale_Agent_Full_Name;
    private string _Product;
    private string _Channel_Item_ID;

    #endregion


    #region "Constructor"
    public bl_policy_micro_single_row_data()
    {

    }
    #endregion

    #region "Public Property"

    public string Policy_Micro_ID
    {
        get { return _Policy_Micro_ID; }
        set { _Policy_Micro_ID = value; }
    }

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

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

    public DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }
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
       
    public string Mobile_Phone1
    {
        get { return _Mobile_Phone1; }
        set { _Mobile_Phone1 = value; }
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
       
    public int Assure_Year
    {
        get { return _Assure_Year; }
        set { _Assure_Year = value; }
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
   
    public int Pay_Mode
    {
        get { return _Pay_Mode; }
        set { _Pay_Mode = value; }
    }

    public int Marital_Status
    {
        get { return _Marital_Status; }
        set { _Marital_Status = value; }
    }

    public DateTime Issue_Date
    {
        get { return _Issue_Date; }
        set { _Issue_Date = value; }
    }

    public string Channel_Location_ID
    {
        get { return _Channel_Location_ID; }
        set { _Channel_Location_ID = value; }
    }

    //Extra

    public string Sale_Agent_Full_Name
    {
        get { return _Sale_Agent_Full_Name; }
        set { _Sale_Agent_Full_Name = value; }
    }

    public string Product
    {
        get { return _Product; }
        set { _Product = value; }
    }

    public string Channel_Item_ID
    {
        get { return _Channel_Item_ID; }
        set { _Channel_Item_ID = value; }
    }

    #endregion
} 