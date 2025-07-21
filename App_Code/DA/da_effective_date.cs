using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_effective_date
/// </summary>
public class da_effective_date
{
    
    private static da_effective_date mytitle = null;
	public da_effective_date()
	{
        if (mytitle == null)
        {
            mytitle = new da_effective_date();
        }
	}
 
    public static DateTime getEffectiveDate(string policy_id)
    {
        DateTime effective_date=DateTime.Now; 
        string connString = AppConfiguration.GetConnectionString();
        string strSelect;
        strSelect = "select effective_date from Ct_Policy where policy_id=@policy_id";
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText =strSelect;
                myCommand.Parameters.AddWithValue("@policy_id", policy_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        effective_date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
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
            Log.AddExceptionToLog("Error in function [GetEffectiveDate] in class [da_effective_date]. Details: " + ex.Message);
        }
        return effective_date;
    }
}