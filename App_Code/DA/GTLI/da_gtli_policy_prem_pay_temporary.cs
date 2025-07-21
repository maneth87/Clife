using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_policy_prem_pay_temporary
/// </summary>
public class da_gtli_policy_prem_pay_temporary
{
    private static da_gtli_policy_prem_pay_temporary mytitle = null;
    public da_gtli_policy_prem_pay_temporary()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_policy_prem_pay_temporary();
		}
	}

    //Function to delete gtli policy prem pay temporary
    public static bool DeleteGTLIPolicyPremPayTemporary(string gtli_premium_id)
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
                myCommand.CommandText = "SP_Delete_GTLI_Policy_Prem_Pay_Temporary";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                bolresult = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteGTLIPolicyPremPayTemporary] in class [da_gtli_policy_prem_pay_temporary]. Details: " + ex.Message);
        }
        return bolresult;
    }

    //Add new policy prem pay temporary
    public static bool InsertPolicyPremPayTemporary(bl_gtli_policy_prem_pay prem_pay, string user_id)
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
                myCommand.CommandText = "SP_Insert_GTLI_Policy_Prem_Pay_Temporary";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Amount", prem_pay.Amount);
                myCommand.Parameters.AddWithValue("@Created_By", prem_pay.Created_By);
                myCommand.Parameters.AddWithValue("@Created_Note", prem_pay.Created_Note);
                myCommand.Parameters.AddWithValue("@Created_On", prem_pay.Created_On);
                myCommand.Parameters.AddWithValue("@Due_Date", prem_pay.Due_Date);
                myCommand.Parameters.AddWithValue("@GTLI_Policy_Prem_Pay_ID", prem_pay.GTLI_Policy_Prem_Pay_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", prem_pay.GTLI_Premium_ID);
                myCommand.Parameters.AddWithValue("@Office_ID", prem_pay.Office_ID);
                myCommand.Parameters.AddWithValue("@Pay_Date", prem_pay.Pay_Date);
                myCommand.Parameters.AddWithValue("@Pay_Mode_ID", prem_pay.Pay_Mode_ID);
                myCommand.Parameters.AddWithValue("@Prem_Lot", prem_pay.Prem_Lot);
                myCommand.Parameters.AddWithValue("@Prem_Year", prem_pay.Prem_Year);
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", prem_pay.Sale_Agent_ID);             
                myCommand.Parameters.AddWithValue("@User_ID", user_id);
                myCommand.Parameters.AddWithValue("@Payment_Code", prem_pay.Payment_Code);     

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
            Log.AddExceptionToLog("Error function [InsertPolicyPremPayTemporary] in class [da_gtli_policy_prem_pay_temporary]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Get policy prem pay by premium id
    public static bl_gtli_policy_prem_pay GetGTLIPolicyPremPayByGTLIPremiumID(string gtli_premium_id)
    {
        //Declare object
        bl_gtli_policy_prem_pay prem_pay = null;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Policy_Prem_Pay_Temporary_By_GTLI_Premium_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {
                            prem_pay = new bl_gtli_policy_prem_pay();
                            prem_pay.Amount = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                            prem_pay.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                            prem_pay.Created_Note = myReader.GetString(myReader.GetOrdinal("Created_Note"));
                            prem_pay.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                            prem_pay.Due_Date = myReader.GetDateTime(myReader.GetOrdinal("Due_Date"));
                            prem_pay.GTLI_Policy_Prem_Pay_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_Prem_Pay_ID"));
                            prem_pay.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                            prem_pay.Office_ID = myReader.GetString(myReader.GetOrdinal("Office_ID"));
                            prem_pay.Pay_Date = myReader.GetDateTime(myReader.GetOrdinal("Pay_Date"));
                            prem_pay.Pay_Mode_ID = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode_ID"));
                            prem_pay.Prem_Lot = myReader.GetInt32(myReader.GetOrdinal("Prem_Lot"));
                            prem_pay.Prem_Year = myReader.GetInt32(myReader.GetOrdinal("Prem_Year"));
                            prem_pay.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                            prem_pay.Payment_Code = myReader.GetString(myReader.GetOrdinal("Payment_Code"));
                           
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
            Log.AddExceptionToLog("Error function [GetGTLPolicyPremPayByGTLIPremiumID] in class [da_gtli_policy_prem_pay_temporary]. Details: " + ex.Message);
        }
        return prem_pay;
    }
}