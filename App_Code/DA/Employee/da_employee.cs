using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for da_employee
/// </summary>
public class da_employee
{
    private static da_employee mytitle = null;
	public da_employee()
	{
	  if (mytitle == null)
        {
            mytitle = new da_employee();
        }
	}

    /// <summary>
    /// Insert into Ct_Employee
    /// </summary>
    public static bool InsertEmployee(bl_employee employee)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Employee";

            cmd.Parameters.AddWithValue("@Employee_ID", employee.Employee_ID);
            cmd.Parameters.AddWithValue("@ID_Card", employee.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", employee.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", employee.First_Name);
            cmd.Parameters.AddWithValue("@Last_Name", employee.Last_Name);
            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", employee.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", employee.Country_ID);
            cmd.Parameters.AddWithValue("@Office_ID", employee.Office_ID);
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
                Log.AddExceptionToLog("Error in function [InsertEmployee] in class [bl_employee]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update in Ct_Employee
    /// </summary>
    public static bool UpdateEmployee(bl_employee employee)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Employee";

            //cmd.Parameters.AddWithValue("@Old_Employee_ID", employee.Old_Employee_ID);
            cmd.Parameters.AddWithValue("@Employee_ID", employee.Employee_ID);
            cmd.Parameters.AddWithValue("@ID_Card", employee.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", employee.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", employee.First_Name);
            cmd.Parameters.AddWithValue("@Last_Name", employee.Last_Name);
            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", employee.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", employee.Country_ID);
            cmd.Parameters.AddWithValue("@Office_ID", employee.Office_ID);
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
                Log.AddExceptionToLog("Error in function [UpdateEmployee] in class [bl_employee]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Delete Employee ID from Ct_Employee
    /// </summary>
    public static bool DeleteEmployee_by_Employee_ID(bl_employee employee)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Employee_By_Employee_ID";
            cmd.Parameters.AddWithValue("@Employee_ID", employee.Office_ID);
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
                Log.AddExceptionToLog("Error in function [DeleteEmployee_by_Employee_ID] in class [bl_employee]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Cancel Employee from Ct_Employee , then Insert into Ct_Employee_Cancel
    /// </summary>
    public static bool CancelEmployee_by_Employee_ID(bl_employee employee)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Employee_Cancel";
            cmd.Parameters.AddWithValue("@Employee_ID", employee.Office_ID);
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
                Log.AddExceptionToLog("Error in function [CancelEmployee_by_Employee_ID] in class [bl_employee]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Check Duplicate from Ct_Employee
    /// </summary>
    public static bool Check_Duplicate_ID_Card_ID(bl_employee employee,int check_id_card_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_EmployeeCode_CardID";
            cmd.Parameters.AddWithValue("@Employee_ID", employee.Employee_ID);
            cmd.Parameters.AddWithValue("@ID_Card", employee.ID_Card);
            cmd.Parameters.AddWithValue("@check_id_card_id", check_id_card_id);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();
            
            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Duplicate_ID_Card_ID] in class [bl_employee]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string Check_Duplicate_ID_Card_ID(string Employee_ID, string ID_Card, int check_id_card_id)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_EmployeeCode_CardID";
            cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
            cmd.Parameters.AddWithValue("@ID_Card", ID_Card);
            cmd.Parameters.AddWithValue("@check_id_card_id", 0);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result += "Employee Code (" + Employee_ID.ToUpper() + ")";
                }

                result += Check_Duplicate_Card_ID(Employee_ID, ID_Card, check_id_card_id);

                if (result != "")
                {
                    result += " have already existed";
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Duplicate_ID_Card_ID] in class [bl_employee]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string Check_Duplicate_Card_ID(string Employee_ID, string ID_Card, int check_id_card_id)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Duplicate_EmployeeCode_CardID";
            cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
            cmd.Parameters.AddWithValue("@ID_Card", ID_Card);
            cmd.Parameters.AddWithValue("@check_id_card_id", check_id_card_id);
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (check_id_card_id == 1)
                    { result += "ID Card (" + ID_Card.ToUpper() + ")"; }
                    else { result = "ID Card (" + ID_Card.ToUpper() + ") has already existed"; }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Duplicate_Card_ID] in class [bl_employee]. Details: " + ex.Message);
            }
        }
        return result;
    }
}