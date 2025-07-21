using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_camlife_staff
/// </summary>
public class da_camlife_staff
{
	public da_camlife_staff()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static bl_camlife_staff GetCamlifeStaff(string user_name)
    {
        bl_camlife_staff staff = new bl_camlife_staff();

        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetCamlifeIntranetConnectionString(), "CPR_SP_EMPLOYEE_GET_BY_USERNAME", new string[,]{
             {"@emp_username",user_name}}, "Function [GetCamlifeStaff(string user_name)] in class [da_camlife_staff]");
            if (tbl.Rows.Count > 0)
            {
                var row = tbl.Rows[0];
                staff.EmpCode = row["emp_code"].ToString();
                staff.EmpLastName = row["emp_lastname"].ToString();
                staff.EmpFirstName = row["emp_firstname"].ToString();
                staff.EmpSex = row["emp_sex"].ToString();
                staff.EmpPosition = row["emp_position"].ToString();
            }
            else
            {
                staff = new bl_camlife_staff();
            }
        }
        catch (Exception ex)
        {
            staff = new bl_camlife_staff();
            Log.AddExceptionToLog("Error function [GetCamlifeStaff(string user_name)] in class [da_camlife_staff], detail:" + ex.Message);
        }
        return staff;
    }
}