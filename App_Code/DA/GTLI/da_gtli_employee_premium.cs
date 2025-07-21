using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_employee_premium
/// </summary>
public class da_gtli_employee_premium
{
	private static da_gtli_employee_premium mytitle = null;
    public da_gtli_employee_premium()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_employee_premium();
		}
	}

    //Insert employee premium record
    public static bool InsertEmployeePremium(bl_gtli_employee_premium employee_premium)
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
                myCommand.CommandText = "SP_Insert_GTLI_Employee_Premium";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Employee_Premium_ID", employee_premium.GTLI_Employee_Premium_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", employee_premium.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", employee_premium.GTLI_Premium_ID);

                myCommand.Parameters.AddWithValue("@Premium", employee_premium.Premium);
                myCommand.Parameters.AddWithValue("@Premium_Type", employee_premium.Premium_Type);
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
            Log.AddExceptionToLog("Error in function [InsertEmployeePremium] in class [da_gtli_employee_premium]. Details: " + ex.Message);
        }
        //Return result to the function caller

        return result;
    }

    //Get premium
    public static double GetPremiumByGTLICertificateID(string gtli_certificate_id, string gtli_premium_id, string premium_type)
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
                myCommand.CommandText = "SP_Get_GTLI_Employee_Premium";

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
            Log.AddExceptionToLog("Error in function [GetPremiumByGTLICertificateID] in class [da_gtli_employee_premium]. Details: " + ex.Message);
        }
        return premium;
    }

    //Get premium case don't know gtli_premium_id
    public static double GetPremiumOfResignEmployeeByGTLICertificateID(string gtli_certificate_id, string premium_type)
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
                myCommand.CommandText = "SP_Get_GTLI_Premium_Of_Resign_Employee";

                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);
                myCommand.Parameters.AddWithValue("@Premium_Type", premium_type);
           
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
            Log.AddExceptionToLog("Error in function [GetPremiumOfResignEmployeeByGTLICertificateID] in class [da_gtli_employee_premium]. Details: " + ex.Message);
        }
        return premium;
    }

    //Function to delete employee premium
    public static bool DeleteEmployeePremium(string gtli_premium_id)
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
                myCommand.CommandText = "SP_Delete_GTLI_Employee_Premium";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteEmployeePremium] in class [da_gtli_employee_premium]. Details: " + ex.Message);
        }
        return result;
    }

    //Get certifcate id list by premium id and premium type
    public static ArrayList GetGTLICertificateIDListByPremiumID(string gtli_premium_id, string premium_type)
    {
        ArrayList certificate_id_list = new ArrayList();

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
                myCommand.CommandText = "SP_Get_GTLI_certificate_ID_List_By_GTLI_Premium_ID";

                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                myCommand.Parameters.AddWithValue("@Premium_Type", premium_type);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            string certificate_id = "";
                            certificate_id = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                            certificate_id_list.Add(certificate_id);
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
            Log.AddExceptionToLog("Error in function [GetGTLICertificateIDListByPremiumID] in class [da_gtli_employee_premium]. Details: " + ex.Message);
        }
        return certificate_id_list;
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
                myCommand.CommandText = "SP_Get_GTLI_Employee_Premium_Sum_Insured";

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
            Log.AddExceptionToLog("Error in function [GetSumInsuredByGTLICertificateID] in class [da_gtli_employee_premium]. Details: " + ex.Message);
        }
        return sum_insured;
    }
    public static bool InsertEmployeePremiumNew(bl_gtli_employee_premium employee_premium)
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
                myCommand.CommandText = "SP_Insert_GTLI_Employee_Premium_New";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", employee_premium.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Employee_Premium_ID", employee_premium.GTLI_Employee_Premium_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", employee_premium.GTLI_Premium_ID);
                myCommand.Parameters.AddWithValue("@Premium", employee_premium.Premium);
                myCommand.Parameters.AddWithValue("@Premium_Type", employee_premium.Premium_Type);
                myCommand.Parameters.AddWithValue("@Sum_Insured", employee_premium.Sum_Insured);
                myCommand.Parameters.AddWithValue("@User_ID", "");


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
            Log.AddExceptionToLog("Error in function [InsertEmployeePremiumNew] in class [da_gtli_employee_premium]. Details: " + ex.Message);
        }
        //Return result to the function caller

        return result;
    }
}