using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_info_contact
/// </summary>
public class bl_app_info_contact
{
	#region "Private Variable"

    private string _App_Register_ID;
    private string _Mobile_Phone1;
    private string _Mobile_Phone2;
    private string _Home_Phone1;
    private string _Home_Phone2;
    private string _Office_Phone1;
    private string _Office_Phone2;
    private string _Fax1;
    private string _Fax2;
    private string _EMail;
 

    #endregion


    #region "Constructor"
    public bl_app_info_contact()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public string Mobile_Phone1
    {
        get { return _Mobile_Phone1; }
        set { _Mobile_Phone1 = value; }
    }

    public string Mobile_Phone2
    {
        get { return _Mobile_Phone2; }
        set { _Mobile_Phone2 = value; }

    }

    public string Home_Phone1
    {
        get { return _Home_Phone1; }
        set { _Home_Phone1 = value; }
    }

    public string Home_Phone2
    {
        get { return _Home_Phone2; }
        set { _Home_Phone2 = value; }
    }

    public string Office_Phone1
    {
        get { return _Office_Phone1; }
        set { _Office_Phone1 = value; }
    }

    public string Office_Phone2
    {
        get { return _Office_Phone2; }
        set { _Office_Phone2 = value; }
    }

    public string Fax1
    {
        get { return _Fax1; }
        set { _Fax1 = value; }
    }

    public string Fax2
    {
        get { return _Fax2; }
        set { _Fax2 = value; }
    }

    public string EMail
    {
        get { return _EMail; }
        set { _EMail = value; }
    }
    public string PolicyID { get; set; }
    #endregion
}