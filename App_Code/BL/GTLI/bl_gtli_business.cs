using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_business
/// </summary>
public class bl_gtli_business
{
	#region "Private Variable"
	
    private string _GTLI_Business_ID;
	private string _Business_Name;
    private string _Created_By;
    private DateTime _Created_On;

    #endregion

	#region "Constructor"
    public bl_gtli_business()
	{
	}
	#endregion
    
	#region "Public Properties"

	public string GTLI_Business_ID {
        get { return _GTLI_Business_ID; }
		set { _GTLI_Business_ID = value; }
	}

    public string Business_Name
    {
        get { return _Business_Name; }
        set { _Business_Name = value; }
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

	#endregion

}