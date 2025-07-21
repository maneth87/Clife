using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for da_gtli_contact
/// </summary>
public class da_gtli_contact
{
	private static da_gtli_contact mytitle = null;
    public da_gtli_contact()
    {
        if (mytitle == null)
        {
            mytitle = new da_gtli_contact();
	    }
    }

    //Function to retrieve active contact by company_id
    public static bl_gtli_contact GetContactByCompanyId(string gtli_company_id)
    {

        bl_gtli_contact contact = null;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
             
                //sql command
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Contact_By_Company_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", gtli_company_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        contact = new bl_gtli_contact();
                        contact.GTLI_Contact_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Contact_ID"));
                        contact.Contact_Name = myReader.GetString(myReader.GetOrdinal("Contact_Name"));
                        contact.Contact_Email = myReader.GetString(myReader.GetOrdinal("Contact_Email"));
                        contact.Contact_Phone = myReader.GetString(myReader.GetOrdinal("Contact_Phone"));
                        contact.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                        contact.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));                        
                        contact.Contact_Status = myReader.GetInt32(myReader.GetOrdinal("Contact_Status"));
                        contact.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));

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
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetContactByCompanyId] in class [da_gtli_contact]. Details: " + ex.Message);
        }
        return contact;
    }

    //Insert new contact
    public static bool InsertContact(bl_gtli_contact contact)
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
                myCommand.CommandText = "SP_Insert_GTLI_Contact";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Contact_ID", contact.GTLI_Contact_ID);
                myCommand.Parameters.AddWithValue("@Contact_Name", contact.Contact_Name);
                myCommand.Parameters.AddWithValue("@Contact_Phone", contact.Contact_Phone);
                myCommand.Parameters.AddWithValue("@Contact_Email", contact.Contact_Email);
                myCommand.Parameters.AddWithValue("@Created_On", contact.Created_On);
                myCommand.Parameters.AddWithValue("@GTLI_Company_ID", contact.GTLI_Company_ID);
                myCommand.Parameters.AddWithValue("@Created_By", contact.Created_By);
                //Execute the query                   
                myCommand.ExecuteNonQuery();
                //Close connection
                myConnection.Close();
                result = true;
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [InsertContact] in class [da_gtli_contact]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Function to retrieve active contact by contact_id
    public static bl_gtli_contact GetContactByContactId(string gtli_contact_id)
    {

        bl_gtli_contact contact = null;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                //sql command
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Contact_By_Contact_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Contact_ID", gtli_contact_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        contact = new bl_gtli_contact();
                        contact.GTLI_Contact_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Contact_ID"));
                        contact.Contact_Name = myReader.GetString(myReader.GetOrdinal("Contact_Name"));
                        contact.Contact_Email = myReader.GetString(myReader.GetOrdinal("Contact_Email"));
                        contact.Contact_Phone = myReader.GetString(myReader.GetOrdinal("Contact_Phone"));
                        contact.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                        contact.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                        contact.Contact_Status = myReader.GetInt32(myReader.GetOrdinal("Contact_Status"));
                        contact.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));

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
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetContactByContactId] in class [da_gtli_contact]. Details: " + ex.Message);
        }
        return contact;
    }

    //Update contact
    public static bool UpdateContact(bl_gtli_contact contact)
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
                myCommand.CommandText = "SP_Update_GTLI_Contact";

                myCommand.Parameters.AddWithValue("@GTLI_Contact_ID", contact.GTLI_Contact_ID);
                myCommand.Parameters.AddWithValue("@Contact_Name", contact.Contact_Name);
                myCommand.Parameters.AddWithValue("@Contact_Phone", contact.Contact_Phone);
                myCommand.Parameters.AddWithValue("@Contact_Email", contact.Contact_Email);
                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();
                result = true;
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error in function [UpdateContact] in class [da_gtli_contact]. Details: " + ex.Message);
        }
        return result;

    }
}