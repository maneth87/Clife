using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_application_search
/// </summary>
public class bl_app_search
{
	#region "Private Variable"

    private string _App_Register_ID;
    private string _App_Number;
    private string _ID_Type;
    private string _ID_Card;
    private string _Last_Name;
    private string _First_Name;
    private string _Gender;
    private DateTime _Birth_Date;
    private string _Nationality;
        
    #endregion


    #region "Constructor"
    public bl_app_search()
    {

    }
    #endregion

    #region "Public Property"

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

    public string ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }

    }

    public string ID_Card
    {
        get { return _ID_Card; }
        set { _ID_Card = value; }
    }

    public string Last_Name
    {
        get { return _Last_Name; }
        set { _Last_Name = value; }
    }

    public string First_Name
    {
        get { return _First_Name; }
        set { _First_Name = value; }
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
    #endregion
}