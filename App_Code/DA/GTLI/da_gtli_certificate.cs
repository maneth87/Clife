using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_certificate
/// </summary>
public class da_gtli_certificate
{
    private static da_gtli_certificate mytitle = null;
    public da_gtli_certificate()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_certificate();
		}
	}

    //Function to delete gtli certificate by premium id
    public static bool DeleteGTLICertificateByGTLIPremiumID(string gtli_premium_id)
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
                myCommand.CommandText = "SP_Delete_GTLI_Certificate_By_GTLI_Premium_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                bolresult = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteGTLICertificateByGTLIPremiumID] in class [da_gtli_certificate]. Details: " + ex.Message);
        }
        return bolresult;
    }

    //Add new certificate
    public static bool InsertCertificate(bl_gtli_certificate certificate)
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
                myCommand.CommandText = "SP_Insert_GTLI_Certificate";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Certificate_Number", certificate.Certificate_Number);
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", certificate.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", certificate.GTLI_Company_ID);
                  
                         

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
            Log.AddExceptionToLog("Error function [InsertCertificate] in class [da_gtli_certificate]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }


    //Get certificate by id
    public static bl_gtli_certificate GetGTLICertificateByID(string gtli_certificate_id)
    {
        //Declare object
        bl_gtli_certificate certificate = new bl_gtli_certificate();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Certificate_By_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            certificate.Certificate_Number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                            certificate.GTLI_Certificate_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Certificate_ID"));
                            certificate.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                            //certificate.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));                          
                            
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
            Log.AddExceptionToLog("Error function [GetGTLICertificateByID] in class [da_gtli_certificate]. Details: " + ex.Message);
        }
        return certificate;
    }

    //Get last certificate number by company id
    public static int GetGTLILastCertificateNumberByID(string company_id, string policy_number)
    {
        //Declare object
        int certificate_number = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Last_Certificate_Number_By_Company_&_Policy_Number";
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                myCommand.Parameters.AddWithValue("@Policy_Number", policy_number);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            certificate_number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                       
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
            Log.AddExceptionToLog("Error function [GetGTLILastCertificateNumberByID] in class [da_gtli_certificate]. Details: " + ex.Message);
        }
        return certificate_number;

    }

    //Get last certificate number by company id
    public static int GetGTLICertificateNumberByPolicyIDAndEmployeeName(string Gtli_policy_id, string Employee_Name)
    {
        //Declare object
        int certificate_number = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Certificate_Number_By_Policy_ID_Employee_Name";
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", Gtli_policy_id);
                myCommand.Parameters.AddWithValue("@Employee_Name", Employee_Name);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            certificate_number = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));

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
            Log.AddExceptionToLog("Error function [GetGTLICertificateNumberByPolicyIDAndEmployeeName] in class [da_gtli_certificate]. Details: " + ex.Message);
        }
        return certificate_number;
    }
}