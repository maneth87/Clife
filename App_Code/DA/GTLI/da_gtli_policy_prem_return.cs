using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_policy_prem_return
/// </summary>
public class da_gtli_policy_prem_return
{
    private static da_gtli_policy_prem_return mytitle = null;
    public da_gtli_policy_prem_return()
	{
        if (mytitle == null)
        {
            mytitle = new da_gtli_policy_prem_return();
		}
	}

    //Function to delete gtli policy prem return
    public static bool DeleteGTLIPolicyPremReturn(string user_id)
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
                myCommand.CommandText = "SP_Delete_GTLI_Policy_Prem_Return";
                myCommand.Parameters.AddWithValue("@User_ID", user_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                bolresult = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteGTLIPolicyPremReturn] in class [da_gtli_policy_prem_return]. Details: " + ex.Message);
        }
        return bolresult;
    }

    //Add new policy prem return
    public static bool InsertPolicyPremReturn(bl_gtli_policy_prem_return prem_return)
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
                myCommand.CommandText = "SP_Insert_GTLI_Policy_Prem_Return";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Amount", prem_return.Amount);
                myCommand.Parameters.AddWithValue("@Created_By", prem_return.Created_By);
                myCommand.Parameters.AddWithValue("@Created_Note", prem_return.Created_Note);
                myCommand.Parameters.AddWithValue("@Created_On", prem_return.Created_On);
                myCommand.Parameters.AddWithValue("@Return_Date", prem_return.Return_Date);
                myCommand.Parameters.AddWithValue("@GTLI_Policy_Prem_Return_ID", prem_return.GTLI_Policy_Prem_Return_ID);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", prem_return.GTLI_Premium_ID);
                myCommand.Parameters.AddWithValue("@Office_ID", prem_return.Office_ID);          
                myCommand.Parameters.AddWithValue("@Pay_Mode_ID", prem_return.Pay_Mode_ID);
                myCommand.Parameters.AddWithValue("@Prem_Lot", prem_return.Prem_Lot);
                myCommand.Parameters.AddWithValue("@Prem_Year", prem_return.Prem_Year);
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", prem_return.Sale_Agent_ID);
                myCommand.Parameters.AddWithValue("@Payment_Code", prem_return.Payment_Code);     

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
            Log.AddExceptionToLog("Error function [InsertPolicyPremReturn] in class [da_gtli_policy_prem_return]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Update status by premium_id
    public static bool UpdateGTLIPolicyPremReturnStatus(int status, string payment_code, string gtli_premium_id)
    {
        //Declare object
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
                myCommand.CommandText = "SP_Update_GTLI_Policy_Prem_Return_Status";
                myCommand.Parameters.AddWithValue("@Status", status);
                myCommand.Parameters.AddWithValue("@Payment_Code", payment_code);
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
           
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = true;
            
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [UpdateGTLPolicyPremReturnStatus] in class [da_gtli_policy_prem_return]. Details: " + ex.Message);
        }
        return result;
    }

    //Get payment code
    public static string GetPaymentCode(string gtli_premium_id)
    {
        //Declare object
        string payment_code = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Policy_Prem_Return_Payment_Code";
                myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            payment_code = myReader.GetString(myReader.GetOrdinal("Payment_Code"));

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
            Log.AddExceptionToLog("Error function [GetPaymentCode] in class [da_gtli_policy_prem_return]. Details: " + ex.Message);
        }
        return payment_code;
    }

    //Get payment code by policy id and status != 3
    public static string GetPaymentCodeByPolicyID(string gtli_policy_id)
    {
        //Declare object
        string payment_code = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Payment_Code_From_Prem_Return_By_Policy_ID_And_Status";
                myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        //If found row, return true & do the statement
                        if (myReader.HasRows)
                        {

                            payment_code = myReader.GetString(myReader.GetOrdinal("Payment_Code"));

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
            Log.AddExceptionToLog("Error function [GetPaymentCodeByPolicyID] in class [da_gtli_policy_prem_return]. Details: " + ex.Message);
        }
        return payment_code;
    }
}