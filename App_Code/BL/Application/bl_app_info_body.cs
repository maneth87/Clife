using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_info_body
/// </summary>
public class bl_app_info_body
{
	#region "Private Variable"

    private string _App_Register_ID;
    private int _Weight;
    private int _Height;
    private int _Is_Weight_Changed;
    private int _Weight_Change;
    private string _Reason;
    #endregion


    #region "Constructor"
    public bl_app_info_body()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public int Weight
    {
        get { return _Weight; }
        set { _Weight = value; }
    }

    public int Height
    {
        get { return _Height; }
        set { _Height = value; }

    }

    public int Is_Weight_Changed
    {
        get { return _Is_Weight_Changed; }
        set { _Is_Weight_Changed = value; }
    }

    public int Weight_Change
    {
        get { return _Weight_Change; }
        set { _Weight_Change = value; }
    }

    public string Reason
    {
        get { return _Reason; }
        set { _Reason = value; }
    }
    #endregion
}