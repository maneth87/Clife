using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro_search
/// </summary>
public class bl_policy_micro_search
{
	#region "Private Variable"

    private string _Policy_Micro_ID;
    private string _Policy_Number;
    private string _ID_Type;
    private string _ID_Card;
    private string _Last_Name;
    private string _First_Name;
    private string _Gender;
    private DateTime _Birth_Date;
    private string _Barcode;
    private DateTime _Issue_Date;
    private DateTime _Effective_Date;

    #endregion


    #region "Constructor"
    public bl_policy_micro_search()
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

    public string Barcode
    {
        get { return _Barcode; }
        set { _Barcode = value; }
    }

    public DateTime Issue_Date
    {
        get { return _Issue_Date; }
        set { _Issue_Date = value; }
    }

    public DateTime Effective_Date
    {
        get { return _Effective_Date; }
        set { _Effective_Date = value; }
    }


    #endregion
}