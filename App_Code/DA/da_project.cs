using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_project
/// </summary>
public class da_project
{
    private static da_project mytitle = null;
    public da_project()
	{
        if (mytitle == null)
        {
            mytitle = new da_project();
		}
	}
     

    //Function to get application_id by application code
    public static string GetApplicationIDByApplicationCode(string application_code)
    {
        string application_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Application_ID_By_Application_Code";
                myCommand.Parameters.AddWithValue("@Application_Code", application_code);
             
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        application_id = myReader.GetString(myReader.GetOrdinal("Application_ID"));
                        
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
            Log.AddExceptionToLog("Error in function [GetApplicationIDByApplicationCode] in class [da_project]. Details: " + ex.Message);
        }
        return application_id;
    }

}