using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_micro_customer
/// </summary>
public class bl_micro_customer
{
    #region "Constructor"
    public bl_micro_customer()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #endregion


    #region "Private Variable"

    private string _Customer_Micro_ID;
    private string _Customer_Micro_Number;
    private string _ID_Card;
    private int _ID_Type;
    private string _First_Name;
    private string _Last_Name;
    private string _Khmer_First_Name;
    private string _Khmer_Last_Name;
    private int _Gender;
    private DateTime _Birth_Date;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;  

    #endregion

    #region "Public Property"

    public string Customer_Micro_ID
    {
        get { return _Customer_Micro_ID; }
        set { _Customer_Micro_ID = value; }
    }

    public string Customer_Micro_Number
    {
        get { return _Customer_Micro_Number; }
        set { _Customer_Micro_Number = value; }
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
    
    public int Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }


    public DateTime Birth_Date
    {
        get { return _Birth_Date; }
        set { _Birth_Date = value; }
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
       

    #endregion


}