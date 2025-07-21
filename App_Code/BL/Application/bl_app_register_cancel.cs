using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_register_cancel
/// </summary>
public class bl_app_register_cancel
{
	#region "Private Variable"

    private string _App_Register_ID;  
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
        
    #endregion


    #region "Constructor"
    public bl_app_register_cancel()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
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