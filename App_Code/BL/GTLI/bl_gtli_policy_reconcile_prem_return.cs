using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_policy_reconcile_prem_return
/// </summary>
public class bl_gtli_policy_reconcile_prem_return
{
	#region "Private Variable"

    private string _GTLI_Policy_Prem_Return_ID;
    private DateTime _Reconcile_Date;   
    private string _Created_Note;
    private string _Created_By;
    private DateTime _Created_On; 
    
    #endregion

	#region "Constructor"
    public bl_gtli_policy_reconcile_prem_return()
	{
	}
	#endregion
    
	#region "Public Properties"

    public string GTLI_Policy_Prem_Return_ID
    {
        get { return _GTLI_Policy_Prem_Return_ID; }
        set { _GTLI_Policy_Prem_Return_ID = value; }
	}
 
    public DateTime Reconcile_Date
    {
        get { return _Reconcile_Date; }
        set { _Reconcile_Date = value; }
    }         

    public string Created_Note
    {
        get { return _Created_Note; }
        set { _Created_Note = value; }
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