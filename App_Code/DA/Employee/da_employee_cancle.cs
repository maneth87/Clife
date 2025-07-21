using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_employee_cancle
/// </summary>
public class da_employee_cancle
{
    private static da_employee_cancle mytitle = null;
    public da_employee_cancle()
	{
	  if (mytitle == null)
        {
            mytitle = new da_employee_cancle();
        }
	}

    /// <summary>
    /// Insert into Ct_Emp_Cancel
    /// </summary>
    public static bool InsertEmployee(bl_employee_cancel employee)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Employee_Cancel";

            cmd.Parameters.AddWithValue("@Employee_ID", employee.Employee_ID);
            cmd.Parameters.AddWithValue("@Created_On", employee.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", employee.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", employee.Created_Note);
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertEmployee] in class [bl_employee_cancel]. Details: " + ex.Message);
            }
        }
        return result;
    }
}