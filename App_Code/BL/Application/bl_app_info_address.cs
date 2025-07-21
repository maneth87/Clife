using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_info_address
/// </summary>
public class bl_app_info_address
{
	#region "Private Variable"

    private string _App_Register_ID;
    private string _Country_ID;
    private string _Address1;
    private string _Address2;
    private string _Address3;
    private string _Province;
    private string _Zip_Code;
   

    #endregion


    #region "Constructor"
    public bl_app_info_address()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public string Country_ID
    {
        get { return _Country_ID; }
        set { _Country_ID = value; }
    }

    public string Address1
    {
        get { return _Address1; }
        set { _Address1 = value; }

    }

    public string Address2
    {
        get { return _Address2; }
        set { _Address2 = value; }
    }

    public string Address3
    {
        get { return _Address3; }
        set { _Address3 = value; }
    }

    public string Province
    {
        get { return _Province; }
        set { _Province = value; }
    }

    public string Zip_Code
    {
        get { return _Zip_Code; }
        set { _Zip_Code = value; }
    }
              
    #endregion
}