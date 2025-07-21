using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_wing
/// </summary>
public class bl_policy_wing
{

    #region "Constructor"
    public bl_policy_wing()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variables"

    private string _Policy_WING_ID;
    private string _Policy_ID;
    private string _Policy_Number;
    private string _Customer_ID;
    private string _Customer_Name;
    private string _Gender;
    private string _ID_Type;
    private string _ID_Number;
    private DateTime _Birth_Date;
    private string _Contact_Number;
    private string _WING_SK;
    private string _WING_Number;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion



    #region "Public Properties"

    public string Policy_WING_ID
    {
        get { return _Policy_WING_ID; }
        set { _Policy_WING_ID = value; }
    }

    public string Policy_ID
    {
        get { return _Policy_ID; }
        set { _Policy_ID = value; }
    }

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }
    
    public string Customer_Name
    {
        get { return _Customer_Name; }
        set { _Customer_Name = value; }
    }

    public string Gender
    {
        get { return _Gender; }
        set { _Gender = value; }
    }

    public string ID_Type
    {
        get { return _ID_Type; }
        set { _ID_Type = value; }
    }

    public string ID_Number
    {
        get { return _ID_Number; }
        set { _ID_Number = value; }
    }

    public DateTime Birth_Date
    {
        get { return _Birth_Date; }
        set { _Birth_Date = value; }
    }

    public string Contact_Number
    {
        get { return _Contact_Number; }
        set { _Contact_Number = value; }
    }

    public string WING_SK
    {
        get { return _WING_SK; }
        set { _WING_SK = value; }
    }

    public string WING_Number
    {
        get { return _WING_Number; }
        set { _WING_Number = value; }
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