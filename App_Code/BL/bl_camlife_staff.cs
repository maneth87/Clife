using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_camlife_staff
/// </summary>
public class bl_camlife_staff
{
	public bl_camlife_staff()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string EmpCode { get; set; }
    public string EmpTitle { get; set; }
    public string EmpFirstName { get; set; }
    public string EmpLastName { get; set; }
    public string EmpSex { get; set; }
    public DateTime EmpDOB { get; set; }
    public string EmpPosition { get; set; }
}