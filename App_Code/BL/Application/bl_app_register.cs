using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_register
/// </summary>
public class bl_app_register
{
	#region "Private Variable"

    private string _App_Register_ID;
    private string _App_Number;
    private string _Original_App_Number;
    private string _Office_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
    private string _Payment_Code;
    private string _Channel_ID;
    private string _Channel_Item_ID;    

    #endregion


    #region "Constructor"
    public bl_app_register()
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

    public string Original_App_Number
    {
        get { return _Original_App_Number; }
        set { _Original_App_Number = value; }

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

    public string Payment_Code
    {
        get { return _Payment_Code; }
        set { _Payment_Code = value; }
    }

    public string Channel_ID
    {
        get { return _Channel_ID; }
        set { _Channel_ID = value; }
    }

    public string Channel_Item_ID
    {
        get { return _Channel_Item_ID; }
        set { _Channel_Item_ID = value; }
    }

    #endregion
}