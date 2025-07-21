using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro_premium
/// </summary>
public class bl_policy_micro_premium
{
	 #region "Constructor"

    public bl_policy_micro_premium()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #endregion


    #region "Private Variable"
   
    private string _Policy_Micro_ID;
    private double _Sum_Insure;  
    private double _Premium;
    private double _Original_Amount; 
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion

    #region "Public Property"

  
    public string Policy_Micro_ID
    {
        get { return _Policy_Micro_ID; }
        set { _Policy_Micro_ID = value; }
    }

    public double Sum_Insure
    {
        get { return _Sum_Insure; }
        set { _Sum_Insure = value; }
    }

    public double Premium
    {
        get { return _Premium; }
        set { _Premium = value; }

    }

    public double Original_Amount
    {
        get { return _Original_Amount; }
        set { _Original_Amount = value; }
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