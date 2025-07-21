using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_step_next
/// </summary>
public class bl_app_step_next
{
	#region "Private Variable"

    private string _App_Register_ID;
   
    private string _Step_ID;

    private DateTime _Last_Updated;

        
    #endregion


    #region "Constructor"
    public bl_app_step_next()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

  
    public string Step_ID
    {
        get { return _Step_ID; }
        set { _Step_ID = value; }

    }   
    
    public DateTime Last_Updated
    {
        get { return _Last_Updated; }
        set { _Last_Updated = value; }
    }

  
  
    #endregion
}