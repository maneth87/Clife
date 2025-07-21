using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_address
/// </summary>
public class bl_policy_address
{
    #region "Private Variable"

    public bl_policy_address()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #endregion


    #region "Private Variable"

    private string _Address_ID;
    private string _Country_ID;
    private string _Address1;
    private string _Address2;
    private string _Address3;
    private string _Province;
    private string _Zip_Code;


    #endregion

    #region "Public Property"

    public string Address_ID
    {
        get { return _Address_ID; }
        set { _Address_ID = value; }
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