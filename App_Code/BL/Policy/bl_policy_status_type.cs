using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_policy_status_type
/// </summary>
public class bl_policy_status_type
{
	#region "Private Variable"

    private string _Policy_Status_Type_ID;
    private string _Policy_Status_Code;
    private string _Detail;
    private bool _Terminated;
    private bool _Disabled;
    private bool _Is_Reserved;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion


    #region "Constructor"
    public bl_policy_status_type()
    {

    }
    #endregion

    #region "Public Property"

    public string Policy_Status_Type_ID
    {
        get { return _Policy_Status_Type_ID; }
        set { _Policy_Status_Type_ID = value; }
    }

    public string Policy_Status_Code
    {
        get { return _Policy_Status_Code; }
        set { _Policy_Status_Code = value; }
    }

    public string Detail
    {
        get { return _Detail; }
        set { _Detail = value; }
    }

    public bool Terminated
    {
        get { return _Terminated; }
        set { _Terminated = value; }
    }

    public bool Disabled
    {
        get { return _Disabled; }
        set { _Disabled = value; }
    }

    public bool Is_Reserved
    {
        get { return _Is_Reserved; }
        set { _Is_Reserved = value; }
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