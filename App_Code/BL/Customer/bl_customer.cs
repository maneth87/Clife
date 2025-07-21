using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_customer
/// </summary>
public class bl_customer
{

    #region "Private Variable"

    private string _Customer_ID;
    private string _ID_Card;
    private int _ID_Type;
    private string _First_Name;
    private string _Last_Name;
    private int _Gender;
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
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion


    #region "Constructor"
    public bl_customer()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion


    #region "Public Property"

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


    #region New properties by maneth 22082017
    public string Customer_Type { get; set; }
    public string Customer_Number { get; set; }
    public string Nationality { get; set; }
    public string Address { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Province { get; set; }
    public string Khan { get; set; }
    public string Sangkat { get; set; }
    public string Zip_Code { get; set; }
    public string Tel { get; set; }
    public string Mobile { get; set; }
    public string Fax { get; set; }
    public string Mail { get; set; }
    public string Mother_First_Name_KH { get; set; }
    public string Mother_Last_Name_KH { get; set; }
    public string Father_First_Name_KH { get; set; }
    public string Father_Last_Name_KH { get; set; }
    public string Marital_Status { get; set; }
    public int Status { get; set; }
    public string Sector { get; set; }
    public string Occupation { get; set; }
    public string Register_Name { get; set; }
    public string Register_Number { get; set; }
    public DateTime Register_Date { get; set; }
    public string TIN_NO { get; set; }
    public string Remarks { get; set; }
    public  string Updated_By { get; set; }
    public  DateTime Updated_DateTime { get; set; }
    #endregion

    #endregion
}