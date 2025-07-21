using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_premium_temporary
/// </summary>
public class da_gtli_premium_temporary
{
	 private static da_gtli_premium_temporary mytitle = null;
     public da_gtli_premium_temporary()
	 {
        if (mytitle == null)
        {
            mytitle = new da_gtli_premium_temporary();
		}
	 }  

     //Add new premium temporary
     public static bool InsertPremiumTemporary(bl_gtli_premium premium_temporary, string user_id)
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
                 myCommand.CommandText = "SP_Insert_GTLI_Premium_Temporary";

                 //Bind parameter to value received from the function caller
                 myCommand.Parameters.AddWithValue("@GTLI_Policy_ID", premium_temporary.GTLI_Policy_ID);
                 myCommand.Parameters.AddWithValue("@Channel_Channel_Item_ID", premium_temporary.Channel_Channel_Item_ID);
                 myCommand.Parameters.AddWithValue("@Created_By", premium_temporary.Created_By);
                 myCommand.Parameters.AddWithValue("@Channel_Location_ID", premium_temporary.Channel_Location_ID);
                 myCommand.Parameters.AddWithValue("@Created_On", premium_temporary.Created_On);
                 myCommand.Parameters.AddWithValue("@Effective_Date", premium_temporary.Effective_Date);
                 myCommand.Parameters.AddWithValue("@Expiry_Date", premium_temporary.Expiry_Date);
                 myCommand.Parameters.AddWithValue("@DHC_Option_Value", premium_temporary.DHC_Option_Value);
                 myCommand.Parameters.AddWithValue("@DHC_Premium", premium_temporary.DHC_Premium);
                 myCommand.Parameters.AddWithValue("@GTLI_Plan_ID", premium_temporary.GTLI_Plan_ID);
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", premium_temporary.GTLI_Premium_ID);
                 myCommand.Parameters.AddWithValue("@Life_Premium", premium_temporary.Life_Premium);
                 myCommand.Parameters.AddWithValue("@Pay_Mode_ID", premium_temporary.Pay_Mode_ID);
                 myCommand.Parameters.AddWithValue("@Policy_Year", premium_temporary.Policy_Year);
                 myCommand.Parameters.AddWithValue("@Sale_Agent_ID", premium_temporary.Sale_Agent_ID);
                 myCommand.Parameters.AddWithValue("@Sum_Insured", premium_temporary.Sum_Insured);
                 myCommand.Parameters.AddWithValue("@TPD_Premium", premium_temporary.TPD_Premium);
                 myCommand.Parameters.AddWithValue("@Transaction_Staff_Number", premium_temporary.Transaction_Staff_Number);
                 myCommand.Parameters.AddWithValue("@Transaction_Type", premium_temporary.Transaction_Type);
                 myCommand.Parameters.AddWithValue("@User_ID", user_id);
                 myCommand.Parameters.AddWithValue("@User_Total_Staff_Number", premium_temporary.User_Total_Staff_Number);
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium", premium_temporary.Accidental_100Plus_Premium);
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Discount", premium_temporary.Accidental_100Plus_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Discount", premium_temporary.DHC_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Discount", premium_temporary.Life_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Discount", premium_temporary.TPD_Premium_Discount);
                 myCommand.Parameters.AddWithValue("@Discount", premium_temporary.Discount);

                 myCommand.Parameters.AddWithValue("@Original_TPD_Premium", premium_temporary.Original_TPD_Premium);
                 myCommand.Parameters.AddWithValue("@Original_Life_Premium", premium_temporary.Original_Life_Premium);
                 myCommand.Parameters.AddWithValue("@Original_DHC_Premium", premium_temporary.Original_DHC_Premium);
                 myCommand.Parameters.AddWithValue("@Original_Accidental_100Plus_Premium", premium_temporary.Original_Accidental_100Plus_Premium);
                 myCommand.Parameters.AddWithValue("@Life_Premium_Tax_Amount", premium_temporary.Life_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@DHC_Premium_Tax_Amount", premium_temporary.DHC_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@Accidental_100Plus_Premium_Tax_Amount", premium_temporary.Accidental_100Plus_Premium_Tax_Amount);
                 myCommand.Parameters.AddWithValue("@TPD_Premium_Tax_Amount", premium_temporary.TPD_Premium_Tax_Amount);
               

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
             Log.AddExceptionToLog("Error function [InsertPremiumTemporary] in class [da_gtli_premium_temporary]. Details: " + ex.Message);
         }
         //Return result to the function caller

         return result;
     }
           

     //Function to delete gtli premium temporary
     public static bool DeleteGTLIPremiumTemporary(string gtli_premium_id)
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
                 myCommand.CommandText = "SP_Delete_GTLI_Premium_Temporary";
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_id);
                 myCommand.ExecuteNonQuery();
                 myConnection.Close();
                 result = true;
             }
         }
         catch (Exception ex)
         {
             //Add error to log 
             Log.AddExceptionToLog("Error in function [DeleteGTLIPremiumTemporary] in class [da_gtli_premium_temporary]. Details: " + ex.Message);
         }
         return result;
     }
        

     //Get premium by ID
     public static bl_gtli_premium GetGTLIPremiumTemporayByID(string gtli_premium_temporary_id)
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
                 myCommand.CommandText = "SP_Get_GTLI_Premium_Temporary_By_ID";
                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_temporary_id);

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

                             premium.Accidental_100Plus_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Tax_Amount"));
                             premium.Life_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Tax_Amount"));
                             premium.TPD_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Tax_Amount"));
                             premium.DHC_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Tax_Amount"));

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
             Log.AddExceptionToLog("Error function [GetGTLPremiumTemporayByID] in class [da_gtli_premium_temporary]. Details: " + ex.Message);
         }
         return premium;
     }

     //Update Premium in premium temporary
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
                 myCommand.CommandText = "SP_Update_GTLI_Premium_Temporary_Premium";

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

     //Update sum insured in premium temporary
     public static void UpdateSumInsured(double sum_insured, string gtli_premium_temporary_id)
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
                 myCommand.CommandText = "SP_Update_GTLI_Premium_Temporary_Sum_Insured";

                 myCommand.Parameters.AddWithValue("@GTLI_Premium_ID", gtli_premium_temporary_id);
                 myCommand.Parameters.AddWithValue("@Sum_Insured", sum_insured);
                

                 myCommand.ExecuteNonQuery();

                 //Close connection
                 myConnection.Close();
             }

         }
         catch (Exception ex)
         {
             //Add error to log for analysis
             Log.AddExceptionToLog("Error in function [UpdateSumInsured] in class [da_gtli_premium_temporary]. Details: " + ex.Message);
         }

     }


     //Get premium list
     public static List<bl_gtli_premium> GetGTLPremiumTemporayList()
     {
         //Declare object
         List<bl_gtli_premium> list_of_gtli_premium = new List<bl_gtli_premium>();

         string connString = AppConfiguration.GetConnectionString();
         try
         {
             using (SqlConnection myConnection = new SqlConnection(connString))
             {

                 SqlCommand myCommand = new SqlCommand();
                 myConnection.Open();
                 myCommand.Connection = myConnection;
                 myCommand.CommandType = CommandType.StoredProcedure;
                 myCommand.CommandText = "SP_Get_GTLI_Premium_Temporary_List";
               
                 using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                 {
                     while (myReader.Read())
                     {
                         //If found row, return true & do the statement
                         if (myReader.HasRows)
                         {
                             bl_gtli_premium premium = new bl_gtli_premium();
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
                             premium.GTLI_Plan = da_gtli_plan.GetPlan(premium.GTLI_Plan_ID).GTLI_Plan;

                             premium.Total_Premium = premium.Life_Premium + premium.TPD_Premium + premium.DHC_Premium + premium.Accidental_100Plus_Premium;

                             premium.Original_Accidental_100Plus_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Accidental_100Plus_Premium"));
                             premium.Original_Life_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_Life_Premium"));
                             premium.Original_TPD_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_TPD_Premium"));
                             premium.Original_DHC_Premium = myReader.GetDouble(myReader.GetOrdinal("Original_DHC_Premium"));

                             premium.Accidental_100Plus_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Accidental_100Plus_Premium_Tax_Amount"));
                             premium.Life_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("Life_Premium_Tax_Amount"));
                             premium.TPD_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("TPD_Premium_Tax_Amount"));
                             premium.DHC_Premium_Tax_Amount = myReader.GetDouble(myReader.GetOrdinal("DHC_Premium_Tax_Amount"));

                             if (da_gtli_policy_temporary.CheckPolicyID(premium.GTLI_Policy_ID))
                             {
                                 //Get policy temporary from Ct_GTLI_Policy_Temporary
                                 bl_gtli_policy policy_temporary = new bl_gtli_policy();
                                 policy_temporary = da_gtli_policy_temporary.GetGTLIPolicyTemporayByID(premium.GTLI_Policy_ID);

                                 premium.Company = da_gtli_company.GetCompanyNameByID(policy_temporary.GTLI_Company_ID);
                             }
                             else
                             {
                                 //Get policy from Ct_GTLI_Policy
                                 bl_gtli_policy policy = new bl_gtli_policy();
                                 policy = da_gtli_policy.GetGTLIPolicyByID(premium.GTLI_Policy_ID);

                                 premium.Company = da_gtli_company.GetCompanyNameByID(policy.GTLI_Company_ID);
                             }                

                             list_of_gtli_premium.Add(premium);
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
             Log.AddExceptionToLog("Error function [GetGTLPremiumTemporayList] in class [da_gtli_premium_temporary]. Details: " + ex.Message);
         }
         return list_of_gtli_premium;
     }
}