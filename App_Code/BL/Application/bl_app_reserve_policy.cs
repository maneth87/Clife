using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_reserve_policy
/// </summary>
public class bl_app_reserve_policy
{
    #region Private Properties

    private string _Reserve_Policy_ID;
    private string _App_Number;
    private string _Customer_ID;
    private string _Policy_Number;
    private DateTime _Created_On;
    private string _Created_By;

    #endregion

    #region "Constructor"

    public bl_app_reserve_policy()
	{
		//
		// TODO: Add constructor logic here
		//

    }

    #endregion


    #region "Public Properties"

    public string Reserve_Policy_ID
    {
        get { return _Reserve_Policy_ID; }
        set { _Reserve_Policy_ID = value; }
    }

    public string App_Number
    {
        get { return _App_Number; }
        set { _App_Number = value; }
    }

    public string Customer_ID
    {
        get { return _Customer_ID; }
        set { _Customer_ID = value; }
    }

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
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
    #endregion
}