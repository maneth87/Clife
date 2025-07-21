using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_underwrite_info
/// </summary>
public class bl_app_underwrite_info
{
	#region "Private Variable"

    private string _Policy_Number;
   
    private string _Underwrting_Status;
         
    #endregion


    #region "Constructor"

    public bl_app_underwrite_info()
    {

    }
    #endregion

    #region "Public Property"

    public string Policy_Number
    {
        get { return _Policy_Number; }
        set { _Policy_Number = value; }
    }
    
    public string Underwrting_Status
    {
        get { return _Underwrting_Status; }
        set { _Underwrting_Status = value; }

    }            
  
    #endregion
}