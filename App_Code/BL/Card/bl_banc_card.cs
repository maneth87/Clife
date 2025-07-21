using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_banc_card
/// </summary>
public class bl_banc_card
{
	#region "Private Variable"
	
    private string _Card_ID;
    private string _Product_ID;
	private string _Card_Number;
    private double _Sum_Insured;
    private double _Premium;
    private string _Created_By;
    private DateTime _Created_On;
    private string _Created_Note;
    private int _Status;
    private string _Url;
   
  
    #endregion

	#region "Constructor"
    public bl_banc_card()
	{
	}
	#endregion
    
	#region "Public Properties"

	public string Card_ID {
        get { return _Card_ID; }
        set { _Card_ID = value; }
	}

    public string Product_ID
    {
        get { return _Product_ID; }
        set { _Product_ID = value; }
	}

    public string Card_Number
    {
        get { return _Card_Number; }
        set { _Card_Number = value; }
    }

    public double Sum_Insured
    {
        get { return _Sum_Insured; }
        set { _Sum_Insured = value; }
    }
    
    public double Premium
    {
        get { return _Premium; }
        set { _Premium = value; }
    }
    
    public int Status
    {
        get { return _Status; }
        set { _Status = value; }
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

    public string Url
    {
        get { return _Url; }
        set { _Url = value; }
    }

	#endregion
}