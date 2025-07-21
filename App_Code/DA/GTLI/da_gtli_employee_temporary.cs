using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_employee_temporary
/// </summary>
public class da_gtli_employee_temporary
{
    private static da_gtli_employee_temporary mytitle = null;
    public da_gtli_employee_temporary()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_employee_temporary();
		}
	}

    //Function to delete gtli employee temporary
    public static bool DeleteGTLIEmployeeTemporary(string gtli_premium_id)
    {
        bool bolresult = false;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Delete_GTLI_Employee_Temporary";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                bolresult = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteGTLIEmployeeTemporary] in class [da_gtli_employee_temporary]. Details: " + ex.Message);
        }
        return bolresult;
    }

    //Function to delete gtli employee temporary by certificate id
    public static bool DeleteGTLIEmployeeTemporaryByCertificateID(string gtli_certificate_id)
    {
        bool bolresult = false;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Delete_GTLI_Employee_Temporary_By_Certificate_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                bolresult = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteGTLIEmployeeTemporaryByCertificateID] in class [da_gtli_employee_temporary]. Details: " + ex.Message);
        }
        return bolresult;
    }

    //Add new employee temporary
    public static bool InsertEmployeeTemporary(bl_gtli_employee employee, string user_id, string gtli_premium_id)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
               
                SqlCommand myCommand = new SqlCommand();

                //Open connection
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Insert_GTLI_Employee_Temporary";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", employee.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@Employee_ID", employee.Employee_ID);
                myCommand.Parameters.AddWithValue("@Customer_Status", 1);
                myCommand.Parameters.AddWithValue("@Employee_Name", employee.Employee_Name);
                myCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                myCommand.Parameters.AddWithValue("@DOB", employee.DOB);
                myCommand.Parameters.AddWithValue("@Position", employee.Position);
                myCommand.Parameters.AddWithValue("@User_ID", user_id);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);

                //Execute the query
                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();

                //Set result to true to track whether the function is successfully operated
                result = true;
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [InsertEmployeeTemporary] in class [da_gtli_employee_temporary]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Get employee temporary list by premium id
    public static ArrayList GetGTLIEmployeeTemporayListByPremiumID(string gtli_premium_temporary_id)
    {
        //Declare object
        ArrayList employee_temporary_list = new ArrayList();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Employee_Temporary_List_By_GTLI_Premium_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_temporary_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            bl_gtli_employee employee_temporary = new bl_gtli_employee();
                            employee_temporary.Certificate_Number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                            employee_temporary.Customer_Status = myReader.GetInt16(myReader.GetOrdinal("Customer_Status"));
                            employee_temporary.DOB = myReader.GetDateTime(myReader.GetOrdinal("DOB"));
                            employee_temporary.Employee_ID = myReader.GetString(myReader.GetOrdinal("Employee_ID"));
                            employee_temporary.Employee_Name = myReader.GetString(myReader.GetOrdinal("Employee_Name"));
                            employee_temporary.Gender = myReader.GetString(myReader.GetOrdinal("Gender"));
                            employee_temporary.GTLI_Certificate_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                            employee_temporary.Position = myReader.GetString(myReader.GetOrdinal("Position"));
                           
                            employee_temporary_list.Add(employee_temporary);
                        }
                    }
                    myReader.Close();
                }
                myConnection.Open();

                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [GetGTLEmployeeTemporayListByPremiumID] in class [da_gtli_employee_temporary]. Details: " + ex.Message);
        }
        return employee_temporary_list;
    }

   
}