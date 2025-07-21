using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_gtli_policy_status
/// </summary>
public class bl_gtli_policy_status
{

    #region "Constructor"
    public bl_gtli_policy_status()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variable"
    private string _GTLI_Policy_ID;
    private string _Policy_Status_Type_ID;    
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion


    #region "Public Properties"

    public string GTLI_Policy_ID
    {
        get { return _GTLI_Policy_ID; }
        set { _GTLI_Policy_ID = value; }
    }
    public string Policy_Status_Type_ID
    {
        get { return _Policy_Status_Type_ID; }
        set { _Policy_Status_Type_ID = value; }
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