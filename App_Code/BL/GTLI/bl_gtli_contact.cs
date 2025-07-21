using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_contact
/// </summary>
public class bl_gtli_contact
{
    #region "Private Variable"
    private string _GTLI_Contact_ID;
    private string _Contact_Name;
    private string _Contact_Phone;
    private string _Contact_Email;
    private string _GTLI_Company_ID;
    private System.DateTime _Created_On;
    private int _Contact_Status;    
    private string _Created_By;

    #endregion
    #region "Constructor"
    public bl_gtli_contact()
    {
    }
    #endregion

    #region "Public Properties"

    public string GTLI_Contact_ID
    {
        get { return _GTLI_Contact_ID; }
        set { _GTLI_Contact_ID = value; }
    }

    public string Contact_Email
    {
        get { return _Contact_Email; }
        set { _Contact_Email = value; }
    }

    public string Contact_Phone
    {
        get { return _Contact_Phone; }
        set { _Contact_Phone = value; }
    }

    public string Contact_Name
    {
        get { return _Contact_Name; }
        set { _Contact_Name = value; }
    }

    public string GTLI_Company_ID
    {
        get { return _GTLI_Company_ID; }
        set { _GTLI_Company_ID = value; }
    }

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public System.DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    public int Contact_Status
    {
        get { return _Contact_Status; }
        set { _Contact_Status = value; }
    }

    #endregion
}