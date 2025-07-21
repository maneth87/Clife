using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_info
/// </summary>
public class bl_app_info
{
	#region "Private Variable"

    private string _App_Register_ID;
    private DateTime _App_Date;
    private string _Sale_Agent_ID;
    private string _Office_ID;
    private string _Benefit_Note;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;
        
    #endregion


    #region "Constructor"
    public bl_app_info()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public DateTime App_Date
    {
        get { return _App_Date; }
        set { _App_Date = value; }
    }

    public string Sale_Agent_ID
    {
        get { return _Sale_Agent_ID; }
        set { _Sale_Agent_ID = value; }

    }

    public string Office_ID
    {
        get { return _Office_ID; }
        set { _Office_ID = value; }
    }

    public string Benefit_Note
    {
        get { return _Benefit_Note; }
        set { _Benefit_Note = value; }
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