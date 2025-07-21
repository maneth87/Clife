using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_certificate
/// </summary>
public class bl_gtli_certificate
{
	#region "Private Variable"

    private string _GTLI_Certificate_ID;
    private int _Certificate_Number;
    private string _GTLI_Company_ID;
    private string _GTLI_Policy_ID;

    #endregion

	#region "Constructor"
    public bl_gtli_certificate()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Certificate_ID
    {
        get { return _GTLI_Certificate_ID; }
        set { _GTLI_Certificate_ID = value; }
	}

    public int Certificate_Number
    {
        get { return _Certificate_Number; }
        set { _Certificate_Number = value; }
	}

    public string GTLI_Company_ID
    {
        get { return _GTLI_Company_ID; }
        set { _GTLI_Company_ID = value; }
    }

    public string GTLI_Policy_ID
    {
        get { return _GTLI_Policy_ID; }
        set { _GTLI_Policy_ID = value; }
    }

	#endregion

}