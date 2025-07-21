using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_employee_cancel
/// </summary>
public class bl_employee_cancel
{
	#region "Private Variable"

    private string _Employee_ID;
    private DateTime _Created_On;
    private string _Created_By;
    private string _Created_Note;

    #endregion

    #region "Constructor"
    public bl_employee_cancel()
    {

    }
    #endregion

    #region "Public Property"

    public string Employee_ID
    {
        get { return _Employee_ID; }
        set { _Employee_ID = value; }
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