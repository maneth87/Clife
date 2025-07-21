using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_business
/// </summary>
public class da_gtli_business
{
    private static da_gtli_business mytitle = null;
    public da_gtli_business()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_business();
		}
	}
    
	//Function to check existing business name
	public static bool CheckExistingBusinessName(string business_name)
	{
		bool result = false;
        string connString = AppConfiguration.GetConnectionString();
		try {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
				
				SqlCommand myCommand = new SqlCommand();
				myConnection.Open();
				myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Check_GTLI_Business";
				myCommand.Parameters.AddWithValue("@Business_Name", business_name);

				using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
					while (myReader.Read()) {
						result = true;
						break; // TODO: might not be correct. Was : Exit While
					}
					myReader.Close();
				}
				myConnection.Open();
				myCommand.ExecuteNonQuery();
				myConnection.Close();
			}
		} catch (Exception ex) {
			//Add error to log 
			Log.AddExceptionToLog("Error in function [CheckExistingBusinessName] in class [da_gtli_business]. Details: " + ex.Message);
		}
		return result;
	}

	//Add new business
	public static bool InsertBusiness(bl_gtli_business business)
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
				myCommand.CommandText = "SP_Insert_GTLI_Business";

				//Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Business_Name", business.Business_Name);
                myCommand.Parameters.AddWithValue("@GTLI_Business_ID", business.GTLI_Business_ID);              

				//Execute the query
				myCommand.ExecuteNonQuery();

				//Close connection
				myConnection.Close();

				//Set result to true to track whether the function is successfully operated
                result = true;
			}

		} catch (Exception ex) {
			//Add error to log for analysis
            Log.AddExceptionToLog("Error function [InsertBusiness] in class [da_gtli_business]. Details: " + ex.Message);
		}
		//Return result to the function caller
		//If <true> means the function has insertd new article successfully
        return result;
	}

	//Function to retrieve list of business name
	public static List<string> GetListOfBusinessName()
	{
        List<string> list_of_business = new List<string>();

		string business_name = "";
        string connString = AppConfiguration.GetConnectionString();
		try {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
				
				//Mysql command
				SqlCommand myCommand = new SqlCommand();
				myConnection.Open();
				myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
				myCommand.CommandText = "SP_Get_GTLI_Business_Name_List";

				using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)) {

					while (myReader.Read()) {
                        business_name = myReader.GetString(myReader.GetOrdinal("Business_Name"));

                        list_of_business.Add(business_name);
					}
					myReader.Close();
				}
				myConnection.Open();
				myCommand.ExecuteNonQuery();
				myConnection.Close();
			}
		} catch (Exception ex) {
			//Add error to log 
			Log.AddExceptionToLog("Error in function [GetListOfBusinessName] in class [da_gtli_business]. Details: " + ex.Message);
		}
		return list_of_business;
	}

	//Update business
	public static bool UpdateBusiness(bl_gtli_business business)
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
				myCommand.CommandText = "SP_Update_GTLI_Business";

                myCommand.Parameters.AddWithValue("@GTLI_Business_ID", business.GTLI_Business_ID);
				myCommand.Parameters.AddWithValue("@Business_Name", business.Business_Name);

				myCommand.ExecuteNonQuery();
                
				//Close connection
				myConnection.Close();
                result = true;
			}

		} catch (Exception ex) {
			//Add error to log for analysis
			Log.AddExceptionToLog("Error in function [UpdateBusiness] in class [da_gtli_business]. Details: " + ex.Message);
		}
        return result;

	}

    //Function to retrieve list of business
    public static List<bl_gtli_business> GetListOfBusiness()
    {
        List<bl_gtli_business> list_of_business = new List<bl_gtli_business>();
               
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                //Mysql command
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Business_List";

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (myReader.Read())
                    {
                        bl_gtli_business business = new bl_gtli_business();
                        business.GTLI_Business_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Business_ID"));
                        business.Business_Name = myReader.GetString(myReader.GetOrdinal("Business_Name"));
                                   
                        list_of_business.Add(business);
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
            Log.AddExceptionToLog("Error in function [GetListOfBusiness] in class [da_gtli_business]. Details: " + ex.Message);
        }
        return list_of_business;
    }

	//Function to delete business
	public static bool DeleteBusiness(string business_id)
	{
		bool result = false;
        string connString = AppConfiguration.GetConnectionString();
		try {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
				
				SqlCommand myCommand = new SqlCommand();
				myConnection.Open();
				myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
				myCommand.CommandText = "SP_Delete_GTLI_Business";
                myCommand.Parameters.AddWithValue("@GTLI_Business_ID", business_id);
				myCommand.ExecuteNonQuery();
				myConnection.Close();
				result = true;
			}
		} catch (Exception ex) {
			//Add error to log 
			Log.AddExceptionToLog("Error in function [DeleteBusiness] in class [da_gtli_business]. Details: " + ex.Message);
		}
		return result;
	}

    
}