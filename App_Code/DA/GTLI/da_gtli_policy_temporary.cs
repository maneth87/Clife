using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_policy_temporary
/// </summary>
public class da_gtli_policy_temporary
{
	 private static da_gtli_policy_temporary mytitle = null;
     public da_gtli_policy_temporary()
	 {
        if (mytitle == null)
        {
            mytitle = new da_gtli_policy_temporary();
		}
	 }  

     //Add new policy
     public static bool InsertPolicyTemporary(bl_gtli_policy policy_temporary, string user_id)
     {
         bool result = false;
         string policy_number = null;
         if (string.IsNullOrEmpty(policy_number))
         {
             //Get last gtli policy number
             policy_number = da_gtli_policy.GetPolicyNumber();
         }

         policy_temporary.Policy_Number = policy_number;
       
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
                 myCommand.CommandText = "SP_Insert_GTLI_Policy_Temporary";

                 //Bind parameter to value received from the function caller
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", policy_temporary.GTLI_Policy_ID);
                 myCommand.Parameters.AddWithValue("@Policy_Number", policy_temporary.Policy_Number);
                 myCommand.Parameters.AddWithValue("@Created_By", policy_temporary.Created_By);
                 myCommand.Parameters.AddWithValue("@Created_Note", policy_temporary.Created_Note);
                 myCommand.Parameters.AddWithValue("@Created_On", policy_temporary.Created_On);
                 myCommand.Parameters.AddWithValue("@Life_Premium", 0);
                 myCommand.Parameters.AddWithValue("@TPD_Premium", 0);
                 myCommand.Parameters.AddWithValue("@DHC_Premium", 0);
                 myCommand.Parameters.AddWithValue("@Effective_Date", policy_temporary.Effective_Date);
                 myCommand.Parameters.AddWithValue("@Expiry_Date", policy_temporary.Expiry_Date);
                 myCommand.Parameters.AddWithValue("@Agreement_date", policy_temporary.Agreement_date);
                 myCommand.Parameters.AddWithValue("@Issue_Date", policy_temporary.Issue_Date);
                 myCommand.Parameters.AddWithValue("@GTLI_Company_ID", policy_temporary.GTLI_Company_ID);
                 myCommand.Parameters.AddWithValue("@User_ID", user_id);
                 
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", policy_temporary.Accidental_100Plus_Premium);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Tax_Amount", policy_temporary.Accidental_100Plus_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Tax_Amount", policy_temporary.DHC_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Tax_Amount", policy_temporary.Life_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Tax_Amount", policy_temporary.TPD_Premium_Tax_Amount);

                 myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", policy_temporary.Original_Accidental_100Plus_Premium);
                 myCommand.Parameters.AddWithValue("@Original_DHC_Premium", policy_temporary.Original_DHC_Premium);
                 myCommand.Parameters.AddWithValue("@Original_Life_Premium", policy_temporary.Original_Life_Premium);
                 myCommand.Parameters.AddWithValue("@Original_TPD_Premium", policy_temporary.Original_TPD_Premium);               

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", policy_temporary.Accidental_100Plus_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Discount", policy_temporary.Life_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", policy_temporary.TPD_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", policy_temporary.DHC_Premium_Discount);
                

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
             Log.AddExceptionToLog("Error function [InsertPolicyTemporary] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }
         //Return result to the function caller

         return result;
     }
     //Add new policy renew
     public static bool InsertReNewPolicyTemporary(bl_gtli_policy policy_temporary, string user_id)
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
                 myCommand.CommandText = "SP_Insert_GTLI_Policy_Temporary";

                 //Bind parameter to value received from the function caller
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", policy_temporary.GTLI_Policy_ID);
                 myCommand.Parameters.AddWithValue("@Policy_Number", policy_temporary.Policy_Number);
                 myCommand.Parameters.AddWithValue("@Created_By", policy_temporary.Created_By);
                 myCommand.Parameters.AddWithValue("@Created_Note", policy_temporary.Created_Note);
                 myCommand.Parameters.AddWithValue("@Created_On", policy_temporary.Created_On);
                 myCommand.Parameters.AddWithValue("@Life_Premium", 0);
                 myCommand.Parameters.AddWithValue("@TPD_Premium", 0);
                 myCommand.Parameters.AddWithValue("@DHC_Premium", 0);
                 myCommand.Parameters.AddWithValue("@Effective_Date", policy_temporary.Effective_Date);
                 myCommand.Parameters.AddWithValue("@Expiry_Date", policy_temporary.Expiry_Date);
                 myCommand.Parameters.AddWithValue("@Agreement_date", policy_temporary.Agreement_date);
                 myCommand.Parameters.AddWithValue("@Issue_Date", policy_temporary.Issue_Date);
                 myCommand.Parameters.AddWithValue("@GTLI_Company_ID", policy_temporary.GTLI_Company_ID);
                 myCommand.Parameters.AddWithValue("@User_ID", user_id);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", policy_temporary.Accidental_100Plus_Premium);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Tax_Amount", policy_temporary.Accidental_100Plus_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Tax_Amount", policy_temporary.DHC_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Tax_Amount", policy_temporary.Life_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Tax_Amount", policy_temporary.TPD_Premium_Tax_Amount);

                 myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", policy_temporary.Original_Accidental_100Plus_Premium);
                 myCommand.Parameters.AddWithValue("@Original_DHC_Premium", policy_temporary.Original_DHC_Premium);
                 myCommand.Parameters.AddWithValue("@Original_Life_Premium", policy_temporary.Original_Life_Premium);
                 myCommand.Parameters.AddWithValue("@Original_TPD_Premium", policy_temporary.Original_TPD_Premium);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", policy_temporary.Accidental_100Plus_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Discount", policy_temporary.Life_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", policy_temporary.TPD_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", policy_temporary.DHC_Premium_Discount);


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
             Log.AddExceptionToLog("Error function [InsertPolicyTemporary] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }
         //Return result to the function caller

         return result;
     }

     //Check existing user id
     public static bool CheckUseID(string user_id)
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
                 myCommand.CommandText = "SP_Check_GTLI_Premium_Temporary_By_User_ID";
                 myCommand.Parameters.AddWithValue("@User_ID", user_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             result = true;
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
             Log.AddExceptionToLog("Error in function [CheckUseID] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }
         return result;
     }

     //get list of gtli_policy_id by user_id
     public static ArrayList GetGTLIPolicyTemporaryIDList(string user_id)
     {
         ArrayList gtli_policy_temporary_id_list = new ArrayList();
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
                 myCommand.CommandText = "SP_Get_GTLI_Policy_Temporary_List_By_User_ID";
                 myCommand.Parameters.AddWithValue("@User_ID", user_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             string gtli_policy_temporary_id = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));

                             gtli_policy_temporary_id_list.Add(gtli_policy_temporary_id);
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
             Log.AddExceptionToLog("Error in function [GetGTLIPolicyTemporaryIDList] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }
         return gtli_policy_temporary_id_list;
     }

     //Function to delete gtli policy temporary
     public static bool DeleteGTLIPolicyTemporary(string gtli_policy_id)
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
                 myCommand.CommandText = "SP_Delete_GTLI_Policy_Temporary";
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);
                 myCommand.ExecuteNonQuery();
                 myConnection.Close();
                 result = true;
             }
         }
         catch (Exception ex)
         {
             //Add error to log 
             Log.AddExceptionToLog("Error in function [DeleteGTLIPolicyTemporary] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }
         return result;
     }
        

     //Get policy by ID
     public static bl_gtli_policy GetGTLIPolicyTemporayByID(string gtli_policy_temporary_id)
     {
         //Declare object
         bl_gtli_policy policy = null;

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {
                 
                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Policy_Temporary_By_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_temporary_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             policy = new bl_gtli_policy();
                             policy.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                             
                             policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                             policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                             policy.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));                            
                             policy.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                             policy.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                             policy.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium")); 
                             policy.GTLI_Company_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Company_ID"));
                             policy.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));                             
                             policy.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                             policy.Issue_Date = myReader.GetDateTime(myReader.GetOrdinal("Issue_Date"));
                             policy.Agreement_date = myReader.GetDateTime(myReader.GetOrdinal("Agreement_date"));

                             policy.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                             policy.Accidental_100Plus_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Discount"));
                             policy.Accidental_100Plus_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Tax_Amount"));

                             policy.DHC_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Discount"));
                             policy.DHC_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Tax_Amount"));
                             policy.Life_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Discount"));
                             policy.Life_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Tax_Amount"));

                             policy.Original_Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Accidental_100Plus_Premium"));
                             policy.Original_DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_DHC_Premium"));
                             policy.Original_Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Life_Premium"));
                             policy.Original_TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_TPD_Premium"));
                                                         
                             policy.TPD_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Discount"));
                             policy.TPD_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Tax_Amount"));
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
             Log.AddExceptionToLog("Error function [GetGTLPolicyTemporayByID] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }
         return policy;
     }

     //Update Premium in policy temporary
     public static void UpdatePremium(double life_premium, double tpd_premium, double dhc_premium, double Accidental_100Plus_Premium, double Accidental_100Plus_Premium_Discount, double Life_Premium_Discount, double TPD_Premium_Discount, double DHC_Premium_Discount, double Original_Accidental_100Plus_Premium, double Original_Life_Premium, double Original_TPD_Premium, double Original_DHC_Premium, string gtli_policy_temporary_id)
     {
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
                 myCommand.CommandText = "SP_Update_GTLI_Policy_Temporary_Premium";

                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_temporary_id);
                 myCommand.Parameters.AddWithValue("@Life_Premium", life_premium);
                 myCommand.Parameters.AddWithValue("@TPD_Premium", tpd_premium);
                 myCommand.Parameters.AddWithValue("@DHC_Premium", dhc_premium);
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", Accidental_100Plus_Premium);
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", Accidental_100Plus_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Discount", Life_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", TPD_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", DHC_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", Original_Accidental_100Plus_Premium);
                 myCommand.Parameters.AddWithValue("@Original_Life_Premium", Original_Life_Premium);
                 myCommand.Parameters.AddWithValue("@Original_TPD_Premium", Original_TPD_Premium);
                 myCommand.Parameters.AddWithValue("@Original_DHC_Premium", Original_DHC_Premium);
              
                 myCommand.ExecuteNonQuery();

                 //Close connection
                 myConnection.Close();
             }

         }
         catch (Exception ex)
         {
             //Add error to log for analysis
             Log.AddExceptionToLog("Error in function [UpdatePremium] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }

     }

     //Check
     public static bool CheckPolicyID(string gtli_policy_temporary_id)
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
                 myCommand.CommandText = "SP_Check_GTLI_Policy_Temporary_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_temporary_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             result = true;
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
             Log.AddExceptionToLog("Error function [CheckPolicyID] in class [da_gtli_policy_temporary]. Details: " + ex.Message);
         }
         return result;
     }
    //TODO : Get Payment code 12/08/2017
     public static string GetPaymentCode(string premiumID)
     {
         string paymentCode = "";
         try
         {
             string connString = AppConfiguration.GetConnectionString();
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_PaymentCode_Temporary_By_Premium_ID";
                 myCommand.Parameters.AddWithValue("@Premium_ID", premiumID);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             paymentCode = myReader.GetString(myReader.GetOrdinal("payment_Code")) ;
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
             Log.AddExceptionToLog("Error function GetPaymentCode in class da_gtli_policy_temporary, Detail: " + ex.Message);
             paymentCode = "";
         }
         return paymentCode;
     }
}