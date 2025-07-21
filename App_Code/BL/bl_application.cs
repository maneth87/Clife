using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_application
/// </summary>
public class bl_application
{
	#region "Private Variable"

    private string _Application_ID;
    private string _Application;
    private string _Application_Code; 
    private string _Created_By;
    private DateTime _Created_On;
    private string _Created_Note;
       
    #endregion

	#region "Constructor"
    public bl_application()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string Application_ID
    {
        get { return _Application_ID; }
        set { _Application_ID = value; }
	}

    public string Application
    {
        get { return _Application; }
        set { _Application = value; }
	}

    public string Application_Code
    {
        get { return _Application_Code; }
        set { _Application_Code = value; }
    }
     

    public string Created_By
    {
        get { return _Created_By; }
        set { _Created_By = value; }
    }

    public DateTime Created_On
    {
        get { return _Created_On; }
        set { _Created_On = value; }
    }

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
    }
	#endregion
}