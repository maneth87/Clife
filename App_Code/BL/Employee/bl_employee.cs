using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_employee
/// </summary>
public class bl_employee
{
	 #region "Private Variable"

    private string _Old_Employee_ID;
    private string _Employee_ID;
    private string _ID_Card;
    private int _ID_Type;
    private string _First_Name;
    private string _Last_Name;
    private int _Gender;
    private DateTime _Birth_Date;
    private string _Country_ID;
    private string _Office_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion

    #region "Constructor"
    public bl_employee()
    {

    }
    #endregion

    #region "Public Property"

    public string Old_Employee_ID
    {
        get { return _Old_Employee_ID; }
        set { _Old_Employee_ID = value; }
    }

    public string Employee_ID
    {
        get { return _Employee_ID; }
        set { _Employee_ID = value; }
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

    #endregion
}