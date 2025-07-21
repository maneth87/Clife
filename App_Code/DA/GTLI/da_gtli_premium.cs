using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_premium
/// </summary>
public class da_gtli_premium
{
	 private static da_gtli_premium mytitle = null;
     public da_gtli_premium()
	 {
        if (mytitle == null)
        {
            mytitle = new da_gtli_premium();
		}
	 }  

     //Add new premium
     public static bool InsertPremium(bl_gtli_premium premium)
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
                 myCommand.CommandText = "SP_Insert_GTLI_Premium";

                 //Bind parameter to value received from the function caller
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", premium.GTLI_Policy_ID);
                 myCommand.Parameters.AddWithValue("@Channel_Channel_Item_ID", premium.Channel_Channel_Item_ID);
                 myCommand.Parameters.AddWithValue("@Created_By", premium.Created_By);
                 myCommand.Parameters.AddWithValue("@Channel_Location_ID", premium.Channel_Location_ID);
                 myCommand.Parameters.AddWithValue("@Created_On", premium.Created_On);
                 myCommand.Parameters.AddWithValue("@Effective_Date", premium.Effective_Date);
                 myCommand.Parameters.AddWithValue("@Expiry_Date", premium.Expiry_Date);
                 myCommand.Parameters.AddWithValue("@DHC_Option_Value", premium.DHC_Option_Value);
                 myCommand.Parameters.AddWithValue("@DHC_Premium", premium.DHC_Premium);
                 myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", premium.GTLI_Plan_ID);
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", premium.GTLI_Premium_ID);
                 myCommand.Parameters.AddWithValue("@Life_Premium", premium.Life_Premium);
                 myCommand.Parameters.AddWithValue("@Pay_Mode_ID", premium.Pay_Mode_ID);
                 myCommand.Parameters.AddWithValue("@Policy_Year", premium.Policy_Year);
                 myCommand.Parameters.AddWithValue("@Sale_Agent_ID", premium.Sale_Agent_ID);
                 myCommand.Parameters.AddWithValue("@Sum_Insured", premium.Sum_Insured);
                 myCommand.Parameters.AddWithValue("@TPD_Premium", premium.TPD_Premium);
                 myCommand.Parameters.AddWithValue("@Transaction_Staff_Number", premium.Transaction_Staff_Number);
                 myCommand.Parameters.AddWithValue("@Transaction_Type", premium.Transaction_Type);
                 myCommand.Parameters.AddWithValue("@User_Total_Staff_Number", premium.User_Total_Staff_Number);
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", premium.Accidental_100Plus_Premium);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", premium.Accidental_100Plus_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", premium.DHC_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Discount", premium.Life_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", premium.TPD_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Discount", premium.Discount);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Tax_Amount", premium.Accidental_100Plus_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Tax_Amount", premium.DHC_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Tax_Amount", premium.Life_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Tax_Amount", premium.TPD_Premium_Tax_Amount);

                 myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", premium.Original_Accidental_100Plus_Premium);
                 myCommand.Parameters.AddWithValue("@Original_DHC_Premium", premium.Original_DHC_Premium);
                 myCommand.Parameters.AddWithValue("@Original_Life_Premium", premium.Original_Life_Premium);
                 myCommand.Parameters.AddWithValue("@Original_TPD_Premium", premium.Original_TPD_Premium);
                 
              

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
             Log.AddExceptionToLog("Error function [InsertPremium] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         //Return result to the function caller

         return result;
     }           

     //Get premium by ID
     public static bl_gtli_premium GetGTLIPremiumByID(string gtli_premium_id)
     {
         //Declare object
         bl_gtli_premium premium = null;

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {
                 
                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Premium_By_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             premium = new bl_gtli_premium();
                             premium.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                             premium.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                             premium.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                             premium.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                             premium.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                             premium.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                             premium.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                             premium.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                             premium.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                             premium.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                             premium.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                             premium.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                             premium.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
                             premium.Pay_Mode_ID = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode_ID"));
                             premium.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                             premium.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                             premium.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC_Option_Value"));
                             premium.Channel_Location_ID = myReader.GetString(myReader.GetOrdinal("Channel_Location_ID"));
                             premium.Channel_Channel_Item_ID = myReader.GetString(myReader.GetOrdinal("Channel_Channel_Item_ID"));
                             premium.User_Total_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("User_Total_Staff_Number"));
                             premium.TPD_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Discount"));
                             premium.Life_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Discount"));
                             premium.DHC_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Discount"));
                             premium.Accidental_100Plus_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Discount"));
                             premium.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                             premium.Discount = myReader.GetDouble(myReader.GetOrdinal("Discount"));
                             premium.Original_Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Accidental_100Plus_Premium"));
                             premium.Original_Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Life_Premium"));
                             premium.Original_TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_TPD_Premium"));
                             premium.Original_DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_DHC_Premium"));
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
             Log.AddExceptionToLog("Error function [GetGTLPremiumByID] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return premium;
     }

     //Get premium by ID
     public static bl_gtli_premium GetGTLPremiumByCerficateID(string gtli_certificate_id)
     {
         //Declare object
         bl_gtli_premium premium = null;

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Premium_By_Certificate_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_certificate_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             premium = new bl_gtli_premium();
                             premium.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                             premium.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                             premium.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                             premium.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                             premium.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                             premium.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                             premium.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                             premium.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                             premium.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                             premium.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                             premium.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                             premium.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                             premium.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
                             premium.Pay_Mode_ID = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode_ID"));
                             premium.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                             premium.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                             premium.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC_Option_Value"));
                             premium.Channel_Location_ID = myReader.GetString(myReader.GetOrdinal("Channel_Location_ID"));
                             premium.Channel_Channel_Item_ID = myReader.GetString(myReader.GetOrdinal("Channel_Channel_Item_ID"));
                             premium.User_Total_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("User_Total_Staff_Number"));
                             premium.TPD_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Discount"));
                             premium.Life_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Discount"));
                             premium.DHC_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Discount"));
                             premium.Accidental_100Plus_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Discount"));
                             premium.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                             premium.Discount = myReader.GetDouble(myReader.GetOrdinal("Discount"));
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
             Log.AddExceptionToLog("Error function [GetGTLPremiumByCerficateID] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return premium;
     }

     //Function to delete gtli premium
     public static bool DeleteGTLIPremium(string gtli_premium_id)
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
                 myCommand.CommandText = "SP_Delete_GTLI_Premium";
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                 myCommand.ExecuteNonQuery();
                 myConnection.Close();
                 result = true;
             }
         }
         catch (Exception ex)
         {
             //Add error to log 
             Log.AddExceptionToLog("Error in function [DeleteGTLIPremium] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return result;
     }

     //Get total sum insured by policy id
     public static double GetGTLTotalSumInsuredByPolicyID(string gtli_policy_id)
     {
         //Declare object
         double tottal_sum_insured = 0;

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Total_Sum_Insured_By_Policy_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {

                             tottal_sum_insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured_New")) + myReader.GetDouble(myReader.GetOrdinal("Sum_Insured_Add")) - myReader.GetDouble(myReader.GetOrdinal("Sum_Insured_Resign"));
                           
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
             Log.AddExceptionToLog("Error function [GetGTLTotalSumInsuredByPolicyID] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return tottal_sum_insured;
     }

     //Get premium list by policy id
     public static List<bl_gtli_premium> GetGTLPremiumListByPolicyID(string gtli_policy_id)
     {
         //Declare object
         bl_gtli_premium premium = null;
         List<bl_gtli_premium> premium_list = new List<bl_gtli_premium>();

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Premium_List_By_Policy_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             premium = new bl_gtli_premium();
                             premium.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                             premium.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                             premium.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                             premium.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                             premium.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                             premium.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                             premium.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                             premium.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                             premium.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                             premium.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                             premium.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                             premium.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                             premium.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
                             premium.Pay_Mode_ID = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode_ID"));
                             premium.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                             premium.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                             premium.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC_Option_Value"));
                             premium.Channel_Location_ID = myReader.GetString(myReader.GetOrdinal("Channel_Location_ID"));
                             premium.Channel_Channel_Item_ID = myReader.GetString(myReader.GetOrdinal("Channel_Channel_Item_ID"));
                             premium.User_Total_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("User_Total_Staff_Number"));
                             premium.TPD_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Discount"));
                             premium.Life_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Discount"));
                             premium.DHC_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Discount"));
                             premium.Accidental_100Plus_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Discount"));
                             premium.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                             premium.Discount = myReader.GetDouble(myReader.GetOrdinal("Discount"));

                             premium_list.Add(premium);
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
             Log.AddExceptionToLog("Error function [GetGTLPremiumListByPolicyID] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return premium_list;
     }

     //Function to check existing plan id in ct_gtli_premium
     public static bool CheckExistingPlanInPremium(string gtli_plan_id)
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
                 myCommand.CommandText = "SP_Check_GTLI_Plan_ID_In_Premium";

                 myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", gtli_plan_id);
                
                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         result = true;
                         break; // TODO: might not be correct. Was : Exit While
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
             Log.AddExceptionToLog("Error in function [CheckExistingPlanInPremium] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return result;
     }

     //Get policy year by policy id
     public static int GetGTLIPolicyYearByPolicyID(string gtli_policy_id)
     {
         //Declare object
         int policy_year = 0;
         List<bl_gtli_premium> premium_list = new List<bl_gtli_premium>();

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Policy_Year_By_Policy_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", gtli_policy_id);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             policy_year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));                          
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
             Log.AddExceptionToLog("Error function [GetGTLIPolicyYearByPolicyID] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return policy_year;
     }

     //Get last policy year by policy number
     public static int GetGTLILastPolicyYearByPolicyNumber(string policy_number)
     {
         //Declare object
         int policy_year = 0;
         List<bl_gtli_premium> premium_list = new List<bl_gtli_premium>();

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Last_Policy_Year_By_Policy_Number";
                 myCommand.Parameters.AddWithValue("@Policy_Number", policy_number);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             policy_year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
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
             Log.AddExceptionToLog("Error function [GetGTLILastPolicyYearByPolicyNumber] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return policy_year;
     }

    

     //Get premium list by status and company id
     public static List<bl_gtli_premium> GetGTLIPremiumListByStatusAndCompanyID(int status, string company_id, DateTime to_date, string transaction_type)
     {
         //Declare object
         bl_gtli_premium premium = null;
         List<bl_gtli_premium> premium_list = new List<bl_gtli_premium>();

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Premium_List_By_Status_And_Company_ID";
                 myCommand.Parameters.AddWithValue("@Status", status);
                 myCommand.Parameters.AddWithValue("@GTLI_Company_ID", company_id);
                 myCommand.Parameters.AddWithValue("@Created_On", to_date.AddDays(1));
                 myCommand.Parameters.AddWithValue("@Transaction_Type", transaction_type);

                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             premium = new bl_gtli_premium();
                             premium.GTLI_Policy_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_ID"));
                             premium.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                             premium.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                             premium.Expiry_Date = myReader.GetDateTime(myReader.GetOrdinal("Expiry_Date"));
                             premium.Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Life_Premium"));
                             premium.TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium"));
                             premium.DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium"));
                             premium.Transaction_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number"));
                             premium.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                             premium.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                             premium.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                             premium.Sale_Agent_ID = myReader.GetString(myReader.GetOrdinal("Sale_Agent_ID"));
                             premium.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));
                             premium.Pay_Mode_ID = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode_ID"));
                             premium.GTLI_Premium_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Premium_ID"));
                             premium.GTLI_Plan_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Plan_ID"));
                             premium.DHC_Option_Value = myReader.GetInt32(myReader.GetOrdinal("DHC_Option_Value"));
                             premium.Channel_Location_ID = myReader.GetString(myReader.GetOrdinal("Channel_Location_ID"));
                             premium.Channel_Channel_Item_ID = myReader.GetString(myReader.GetOrdinal("Channel_Channel_Item_ID"));
                             premium.User_Total_Staff_Number = myReader.GetInt32(myReader.GetOrdinal("User_Total_Staff_Number"));
                             premium.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                             premium.Policy_Year = myReader.GetInt32(myReader.GetOrdinal("Policy_Year"));

                             premium.TPD_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Discount"));
                             premium.Life_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Discount"));
                             premium.DHC_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Discount"));
                             premium.Accidental_100Plus_Premium_Discount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Discount"));
                             premium.Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium"));
                             premium.Discount = myReader.GetDouble(myReader.GetOrdinal("Discount"));

                             premium_list.Add(premium);
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
             Log.AddExceptionToLog("Error function [GetGTLPremiumListByStatusAndCompanyID] in class [da_gtli_premium]. Details: " + ex.Message);
         }
         return premium_list;
     }

     public static void UpdatePremium(double sum_insure, double life_premium, double tpd_premium, double dhc_premium, double accidental_100plus_premium, double accidental_100plus_premium_discount, double life_premium_discount, double tpd_premium_discount, double dhc_premium_discount, double original_accidental_100plus_premium, double original_life_premium, double original_tpd_premium, double original_dhc_premium, double accidental_100plus_premium_tax_amount, double life_premium_tax_amount, double tpd_premium_tax_amount, double dhc_premium_tax_amount, string gtli_premium_temporary_id)
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
                 myCommand.CommandText = "SP_Update_GTLI_Premium";

                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_temporary_id);

                 myCommand.Parameters.AddWithValue("@Sum_Insured", sum_insure);
                 myCommand.Parameters.AddWithValue("@Life_Premium", life_premium);
                 myCommand.Parameters.AddWithValue("@TPD_Premium", tpd_premium);
                 myCommand.Parameters.AddWithValue("@DHC_Premium", dhc_premium);
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", accidental_100plus_premium);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", accidental_100plus_premium_discount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Discount", life_premium_discount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", tpd_premium_discount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", dhc_premium_discount);

                 myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", original_accidental_100plus_premium);
                 myCommand.Parameters.AddWithValue("@Original_Life_Premium", original_life_premium);
                 myCommand.Parameters.AddWithValue("@Original_TPD_Premium", original_tpd_premium);
                 myCommand.Parameters.AddWithValue("@Original_DHC_Premium", original_dhc_premium);

                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Tax_Amount", accidental_100plus_premium_tax_amount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Tax_Amount", life_premium_tax_amount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Tax_Amount", tpd_premium_tax_amount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Tax_Amount", dhc_premium_tax_amount);

                 myCommand.ExecuteNonQuery();

                 //Close connection
                 myConnection.Close();
             }

         }
         catch (Exception ex)
         {
             //Add error to log for analysis
             Log.AddExceptionToLog("Error in function [UpdatePremium] in class [da_gtli_premium_temporary]. Details: " + ex.Message);
         }

     }
     public static double getEmployeeSumInsured(string empCertificateID)
     {
        double sumInsured=0;
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
                myCommand.CommandText = "SP_Get_GTLI_Employee_SumInsured_By_Certificate_ID";
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", empCertificateID);

                SqlDataReader myReader;

                myReader = myCommand.ExecuteReader();
                while (myReader.Read()) {
                    sumInsured = Convert.ToDouble(myReader.GetString(0));

                }

                //using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                //{
                //    while (myReader.Read())
                //    {
                //        //If found row, return true & do the statement
                //        if (myReader.HasRows)
                //        {
                //            //sumInsured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                //            sumInsured = myReader.GetDouble(0);
                //        }
                //    }
                //    myReader.Close();
                //}
                myReader.Close();
                myCommand.Dispose();
                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [getEmployeePremium] in class [da_gtli_premium]. Details: " + ex.Message);
        }
        return sumInsured;
    }

}