using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_certificate_temporary
/// </summary>
public class da_gtli_certificate_temporary
{
    private static da_gtli_certificate_temporary mytitle = null;
    public da_gtli_certificate_temporary()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_certificate_temporary();
		}
	}

    //Function to delete gtli certificate temporary
    public static bool DeleteGTLICertificateTemporary(string gtli_premium_id)
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
                myCommand.CommandText = "SP_Delete_GTLI_Certificate_Temporary";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                bolresult = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteGTLICertificateTemporary] in class [da_gtli_certificate_temporary]. Details: " + ex.Message);
        }
        return bolresult;
    }

    //Add new certificate temporary
    public static bool InsertCertificateTemporary(bl_gtli_certificate certificate, string user_id, string gtli_premium_id)
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
                myCommand.CommandText = "SP_Insert_GTLI_Certificate_Temporary";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Certificate_Number", certificate.Certificate_Number);
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", certificate.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", certificate.GTLI_Company_ID);
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
            Log.AddExceptionToLog("Error function [InsertCertificateTemporary] in class [da_gtli_certificate_temporary]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Update certificate temporary
    public static bool UpdateCertificateTemporary(bl_gtli_certificate certificate, string user_id, string gtli_premium_id)
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
                myCommand.CommandText = "SP_Update_GTLI_Certificate_Temporary";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Certificate_Number", certificate.Certificate_Number);
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", certificate.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", certificate.GTLI_Company_ID);
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
            Log.AddExceptionToLog("Error function [InsertCertificateTemporary] in class [da_gtli_certificate_temporary]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Get certificate temporary by id
    public static bl_gtli_certificate GetGTLICertificateTemporayByID(string gtli_certificate_id)
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
                myCommand.CommandText = "SP_Get_GTLI_Certificate_Temporary_By_ID";
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
            Log.AddExceptionToLog("Error function [GetGTLICertificateTemporayByID] in class [da_gtli_certificate_temporary]. Details: " + ex.Message);
        }
        return certificate;
    }
    
     public static int GetLastCertificateTemporayNumber(string companyID)
    {
        //Declare object
        int certificate = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                string strSelect = "SELECT TOP 1 Certificate_Number FROM Ct_GTLI_Certificate_Temporary WHERE GTLI_Company_ID = @GTLI_Company_ID ORDER BY Certificate_Number DESC";
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = strSelect;
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", companyID);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            certificate = myReader.GetInt32(myReader.GetOrdinal("Certificate_Number"));
                        
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
            Log.AddExceptionToLog("Error function [GetLastCertificateTemporayNumber] in class [da_gtli_certificate_temporary]. Details: " + ex.Message);
        }
        return certificate;
    }
   
}