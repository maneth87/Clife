using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_micro_status
/// </summary>
public class bl_policy_micro_status
{

    #region "Constructor"
    public bl_policy_micro_status()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region "Private Variable"
    private string _Policy_Micro_ID;
    private string _Policy_Status_Type_ID;    
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion


    #region "Public Properties"

    public string Policy_Micro_ID
    {
        get { return _Policy_Micro_ID; }
        set { _Policy_Micro_ID = value; }
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