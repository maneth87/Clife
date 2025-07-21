using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

/// <summary>
/// Summary description for da_gtli_employee_premium_temporary
/// </summary>
public class da_gtli_employee_premium_temporary
{
    private static da_gtli_employee_premium_temporary mytitle = null;
    public da_gtli_employee_premium_temporary()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_employee_premium_temporary();
		}
	}

    //Function to delete premium
    public static bool DeleteEmployeePremiumTemporary(string gtli_premium_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
               
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Delete_GTLI_Employee_Premium_Temporary";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteEmployeePremiumTemporary] in class [da_gtli_employee_premium_temporary]. Details: " + ex.Message);
        }
        return result;
    }

    //Add premium record
    public static bool InsertEmployeePremiumTemporary(bl_gtli_employee_premium employee_premium, string user_id)
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
                myCommand.CommandText = "SP_Insert_GTLI_Employee_Premium_Temporary";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", employee_premium.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Employee_Premium_ID", employee_premium.GTLI_Employee_Premium_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", employee_premium.GTLI_Premium_ID);

                myCommand.Parameters.AddWithValue("@Premium", employee_premium.Premium);
                myCommand.Parameters.AddWithValue("@Premium_Type", employee_premium.Premium_Type);
                myCommand.Parameters.AddWithValue("@User_ID", user_id);
                myCommand.Parameters.AddWithValue("@Sum_Insured", employee_premium.Sum_Insured);
              

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
            Log.AddExceptionToLog("Error in function [InsertEmployeePremiumTemporary] in class [da_gtli_employee_premium_temporary]. Details: " + ex.Message);
        }
        //Return result to the function caller

        return result;
    }

    //Get premium
    public static double GetPremiumByCertificateID(string gtli_certificate_temporary_id, string gtli_premium_temporary_id, string premium_type)
    {
        double premium = 0;
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
                myCommand.CommandText = "SP_Get_GTLI_Employee_Premium_Temporary";

                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_temporary_id);
                myCommand.Parameters.AddWithValue("@Premium_Type", premium_type);
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_temporary_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            premium = Convert.ToDouble(myReader.GetString(myReader.GetOrdinal("premium")));
                        }
                    }
                    myReader.Close();
                }
                myConnection.Open();

                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [GetPremiumByCertificateID] in class [da_gtli_employee_premium_temporary]. Details: " + ex.Message);
        }
        return premium;
    }

    //Get premium list
    public static List<bl_gtli_employee_premium> GetPremiumList(string gtli_certificate_temporary_id, string gtli_premium_temporary_id)
    {

        List<bl_gtli_employee_premium> premium_list = new List<bl_gtli_employee_premium>();
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
                myCommand.CommandText = "SP_Get_GTLI_Employee_Premium_Temporary_List";

                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_temporary_id);
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_temporary_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            bl_gtli_employee_premium premium = new bl_gtli_employee_premium();
                            premium.GTLI_Employee_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Employee_Premium_ID"));
                            premium.Premium = myReader.GetString(myReader.GetOrdinal("Premium"));
                            premium.Premium_Type = myReader.GetString(myReader.GetOrdinal("Premium_Type"));
                            premium.GTLI_Certificate_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                            premium.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                            premium.Sum_Insured = myReader.GetString(myReader.GetOrdinal("Sum_Insured"));

                            premium_list.Add(premium);
                        }
                    }
                    myReader.Close();
                }
                myConnection.Open();

                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [GetPremiumList] in class [da_gtli_employee_premium_temporary]. Details: " + ex.Message);
        }
        return premium_list;
    }

    //Get premium
    public static double GetSumInsuredByGTLICertificateID(string gtli_certificate_id, string gtli_premium_id, string premium_type)
    {
        double sum_insured = 0;
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
                myCommand.CommandText = "SP_Get_GTLI_Employee_Premium_Sum_Insured_Temporary";

                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);
                myCommand.Parameters.AddWithValue("@Premium_Type", premium_type);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            sum_insured = Convert.ToDouble(myReader.GetString(myReader.GetOrdinal("Sum_Insured")));
                        }
                    }
                    myReader.Close();
                }
                myConnection.Open();

                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [GetSumInsuredByGTLICertificateID] in class [da_gtli_employee_premium_Temporary]. Details: " + ex.Message);
        }
        return sum_insured;
    }
}