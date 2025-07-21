using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro
/// </summary>
public class bl_policy_micro
{
    #region "Constructor"
    public bl_policy_micro()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion


    #region "Private Variable"

    private string _Policy_Micro_ID;
    private string _Customer_ID;
    private string _Product_ID;  
    private DateTime _Effective_Date;
    private DateTime _Maturity_Date;
    private DateTime _Agreement_Date;
    private DateTime _Issue_Date;
    private int _Age_Insure;
    private int _Pay_Year;
    private int _Pay_Up_To_Age;
    private int _Assure_Year;
    private int _Assure_Up_To_Age;  
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private string _Channel_Location_ID;
    private string _Channel_Channel_Item_ID;

    #endregion

    #region "Public Property"
      
    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
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
    
    public int Assure_Up_To_Age
    {
        get { return _Assure_Up_To_Age; }
        set { _Assure_Up_To_Age = value; }
    }

    public int Assure_Year
    {
        get { return _Assure_Year; }
        set { _Assure_Year = value; }
    }


    public int Pay_Up_To_Age
    {
        get { return _Pay_Up_To_Age; }
        set { _Pay_Up_To_Age = value; }
    }


    public int Pay_Year
    {
        get { return _Pay_Year; }
        set { _Pay_Year = value; }
    }

    public int Age_Insure
    {
        get { return _Age_Insure; }
        set { _Age_Insure = value; }
    }
        
    public DateTime Issue_Date
    {
        get { return _Issue_Date; }
        set { _Issue_Date = value; }
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
    
    public DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
    }

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }

    public string Policy_Micro_ID
    {
        get { return _Policy_Micro_ID; }
        set { _Policy_Micro_ID = value; }
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


}