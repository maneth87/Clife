using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_job_history
/// </summary>
public class bl_app_job_history
{
	#region "Private Variable"

    private string _App_Register_ID;
    private string _Employer_Name;
    private string _Nature_Of_Business;
    private string _Current_Position;
    private string _Job_Role;
    private double _Anual_Income;  
   
    #endregion


    #region "Constructor"
    public bl_app_job_history()
    {

    }
    #endregion

    #region "Public Property"

    public string App_Register_ID
    {
        get { return _App_Register_ID; }
        set { _App_Register_ID = value; }
    }

    public string Employer_Name
    {
        get { return _Employer_Name; }
        set { _Employer_Name = value; }
    }

    public string Nature_Of_Business
    {
        get { return _Nature_Of_Business; }
        set { _Nature_Of_Business = value; }

    }

    public string Current_Position
    {
        get { return _Current_Position; }
        set { _Current_Position = value; }
    }

    public string Job_Role
    {
        get { return _Job_Role; }
        set { _Job_Role = value; }
    }

    public double Anual_Income
    {
        get { return _Anual_Income; }
        set { _Anual_Income = value; }
    }
                  
    #endregion
}