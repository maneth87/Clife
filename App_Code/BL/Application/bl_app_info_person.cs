using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_info_person
/// </summary>
public class bl_app_info_person
{
	#region "Private Variable"

    private string _App_Register_ID;
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

    #endregion


    #region "Constructor"
    public bl_app_info_person()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
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
    #endregion
}