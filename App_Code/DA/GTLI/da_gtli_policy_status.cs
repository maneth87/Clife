using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_policy_status
/// </summary>
public class da_gtli_policy_status
{
    private static da_gtli_policy_status mytitle = null;
    public da_gtli_policy_status()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_policy_status();
		}
	}
    


	//Add new gtli policy status
	public static bool InsertGTLIPolicyStatus(bl_gtli_policy_status policy_status)
	{
		bool result = false;

        string connString = AppConfiguration.GetConnectionString();
		try {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
				
				SqlCommand myCommand = new SqlCommand();

				//Open connection
				myConnection.Open();
				myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
				myCommand.CommandText = "SP_Insert_GTLI_Policy_Status";

				//Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Policy_Status_Type_ID", policy_status.Policy_Status_Type_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", policy_status.GTLI_Policy_ID);
                myCommand.Parameters.AddWithValue("@Created_On", policy_status.Created_On);
                myCommand.Parameters.AddWithValue("@Created_Note", policy_status.Created_Note);
                myCommand.Parameters.AddWithValue("@Created_By", policy_status.Created_By);   

				//Execute the query
				myCommand.ExecuteNonQuery();

				//Close connection
				myConnection.Close();

				//Set result to true to track whether the function is successfully operated
                result = true;
			}

		} catch (Exception ex) {
			//Add error to log for analysis
            Log.AddExceptionToLog("Error function [InsertGTLIPolicyStatus] in class [da_gtli_policy_status]. Details: " + ex.Message);
		}
		//Return result to the function caller
		//If <true> means the function has insertd new article successfully
        return result;
	}


    
}