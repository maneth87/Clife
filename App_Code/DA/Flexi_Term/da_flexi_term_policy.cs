using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

/// <summary>
/// Summary description for da_flexi_term_policy
/// </summary>
public class da_flexi_term_policy
{
    private static da_flexi_term_policy mytitle = null;

    #region "Constructors"

    public da_flexi_term_policy()
	{
        if (mytitle == null)
        {
            mytitle = new da_flexi_term_policy();
        }
    }

    #endregion

    #region "Public Functions"

    //Insert new flexi term policy then return policy ID
    public static string InsertFlexiTermPolicy(bl_flexi_term_policy flexi_term_policy)
    {
        string flexi_term_policy_id = "";

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy";

            //get new primary key for the row to be inserted
            flexi_term_policy.Flexi_Term_Policy_ID = Helper.GetNewGuid("SP_Check_Flexi_Term_Policy_ID", "@Flexi_Term_Policy_ID").ToString();

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy.Flexi_Term_Policy_ID);
            cmd.Parameters.AddWithValue("@Customer_Flexi_Term_ID", flexi_term_policy.Customer_Flexi_Term_ID);
            cmd.Parameters.AddWithValue("@Product_ID", flexi_term_policy.Product_ID);
            cmd.Parameters.AddWithValue("@Effective_Date", flexi_term_policy.Effective_Date);
            cmd.Parameters.AddWithValue("@Maturity_Date", flexi_term_policy.Maturity_Date);
            cmd.Parameters.AddWithValue("@Agreement_Date", flexi_term_policy.Agreement_Date);
            cmd.Parameters.AddWithValue("@Issue_Date", flexi_term_policy.Issue_Date);
            cmd.Parameters.AddWithValue("@Age_Insure", flexi_term_policy.Age_Insure);
            cmd.Parameters.AddWithValue("@Pay_Year", flexi_term_policy.Pay_Year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", flexi_term_policy.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", flexi_term_policy.Assure_Year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", flexi_term_policy.Assure_Up_To_Age);
           
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", flexi_term_policy.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", "");
            cmd.Parameters.AddWithValue("@Channel_Location_ID", flexi_term_policy.Channel_Location_ID);
            cmd.Parameters.AddWithValue("@Channel_Channel_Item_ID", flexi_term_policy.Channel_Channel_Item_ID);


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();   
                con.Close();
                flexi_term_policy_id = flexi_term_policy.Flexi_Term_Policy_ID;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicy] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return flexi_term_policy_id;
    }

    //Insert new flexi term customer then return ID
    public static string InsertCustomerFlexiTerm(bl_customer_flexi_term customer_flexi_term)
    {
        string customer_flexi_term_id = "";

        //get last customer flexi term number
        string last_customer_flexi_term = da_flexi_term_policy.GetLastCustomerFlexiTermNumber();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Customer_Flexi_Term";

            //get new primary key for the row to be inserted
            customer_flexi_term.Customer_Flexi_Term_ID = Helper.GetNewGuid("SP_Check_Customer_Flexi_Term_ID", "@Customer_Flexi_Term_ID").ToString();
            customer_flexi_term.Customer_Flexi_Term_Number = last_customer_flexi_term;
            cmd.Parameters.AddWithValue("@Customer_Flexi_Term_ID", customer_flexi_term.Customer_Flexi_Term_ID);
            cmd.Parameters.AddWithValue("@Customer_Flexi_Term_Number", customer_flexi_term.Customer_Flexi_Term_Number);
            cmd.Parameters.AddWithValue("@Created_On", customer_flexi_term.Created_On);
            cmd.Parameters.AddWithValue("@Created_Note", customer_flexi_term.Created_Note);
            cmd.Parameters.AddWithValue("@Created_By", customer_flexi_term.Created_By);
            cmd.Parameters.AddWithValue("@Birth_Date", customer_flexi_term.Birth_Date);        
            cmd.Parameters.AddWithValue("@Gender", customer_flexi_term.Gender);
            cmd.Parameters.AddWithValue("@ID_Card", customer_flexi_term.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", customer_flexi_term.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", customer_flexi_term.First_Name);
            cmd.Parameters.AddWithValue("@Last_Name", customer_flexi_term.Last_Name);
            cmd.Parameters.AddWithValue("@Khmer_First_Name", customer_flexi_term.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", customer_flexi_term.Khmer_Last_Name);
                     
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                customer_flexi_term_id = customer_flexi_term.Customer_Flexi_Term_ID;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertCustomerFlexiTerm] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return customer_flexi_term_id;
    }      

    //Insert Ct_Policy_ID
    public static bool InsertPolicyID(string policy_id, string policy_type_id)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Type_ID", policy_type_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyID] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_Policy_Number
    public static bool InsertPolicyNumber(string policy_id, string policy_number)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Number";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Number", policy_number);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyNumber] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get last customer flexi term number
    public static string GetLastCustomerFlexiTermNumber()
    {
        string last_customer_flexi_term_number = "0";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Last_Customer_Flexi_Term_Number";                

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_channel_location channel_location = new bl_channel_location();

                        last_customer_flexi_term_number = myReader.GetString(myReader.GetOrdinal("Customer_Flexi_Term_Number"));
                        
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
            Log.AddExceptionToLog("Error in function [GetLastCustomerFlexiTermNumber] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }

        if (last_customer_flexi_term_number != "0")
        {
            last_customer_flexi_term_number = last_customer_flexi_term_number.Substring(2);

        }

        int last_number = Convert.ToInt32(last_customer_flexi_term_number) + 1;

        last_customer_flexi_term_number = last_number.ToString();

        while (last_customer_flexi_term_number.Length < 8)
        {
            last_customer_flexi_term_number = "0" + last_customer_flexi_term_number;
        }        

        last_customer_flexi_term_number = "FT" + last_customer_flexi_term_number;

        return last_customer_flexi_term_number;
        
    }

    //Function to get zip code
    public static string GetZipCode(string country_id)
    {
        string zip_code = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Zip_Code", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@Country_ID";
            paramName.Value = country_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    zip_code = rdr.GetString(rdr.GetOrdinal("Num_Country"));

                }

            }
            con.Close();
        }
        return zip_code;
    }

    //Insert new policy flexi term card
    public static bool InsertFlexiTermPolicyCard(bl_flexi_term_policy_banc_card flexi_term_policy_card)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy_Banc_Card";

            cmd.Parameters.AddWithValue("@Card_ID", flexi_term_policy_card.Card_ID);
            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_card.Flexi_Term_Policy_ID);

            cmd.Parameters.AddWithValue("@Created_On", flexi_term_policy_card.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", flexi_term_policy_card.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", flexi_term_policy_card.Created_Note);          

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicyCard] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new flexi term policy Info Person
    public static bool InsertFlexiTermPolicyInfoPerson(bl_flexi_term_policy_info_person flexi_term_policy_info_person)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy_Info_Person";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_info_person.Flexi_Term_Policy_ID);
            cmd.Parameters.AddWithValue("@Birth_Date", flexi_term_policy_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@First_Name", flexi_term_policy_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", flexi_term_policy_info_person.Gender);
            cmd.Parameters.AddWithValue("@ID_Card", flexi_term_policy_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", flexi_term_policy_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@Last_Name", flexi_term_policy_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Resident", flexi_term_policy_info_person.Resident);
            cmd.Parameters.AddWithValue("@Branch", flexi_term_policy_info_person.Branch);
            cmd.Parameters.AddWithValue("@Bank_Number", flexi_term_policy_info_person.Bank_Number);
            cmd.Parameters.AddWithValue("@Khmer_First_Name", flexi_term_policy_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", flexi_term_policy_info_person.Khmer_Last_Name);        

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicyInfoPerson] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }
        
    //Insert new flexi term policy life product
    public static bool InsertFlexiTermPolicyLifeProduct(bl_flexi_term_policy_life_product flexi_term_policy_life_product)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy_Life_Product";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_life_product.Flexi_Term_Policy_ID);
            cmd.Parameters.AddWithValue("@Age_Insure", flexi_term_policy_life_product.Age_Insure);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", flexi_term_policy_life_product.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", flexi_term_policy_life_product.Assure_Year);
            cmd.Parameters.AddWithValue("@Pay_Mode", flexi_term_policy_life_product.Pay_Mode);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", flexi_term_policy_life_product.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Pay_Year", flexi_term_policy_life_product.Pay_Year);
            cmd.Parameters.AddWithValue("@Product_ID", flexi_term_policy_life_product.Product_ID);
            cmd.Parameters.AddWithValue("@System_Premium", flexi_term_policy_life_product.System_Premium);
            cmd.Parameters.AddWithValue("@System_Premium_Discount", flexi_term_policy_life_product.System_Premium_Discount);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", flexi_term_policy_life_product.System_Sum_Insure);
            cmd.Parameters.AddWithValue("@User_Premium", flexi_term_policy_life_product.User_Premium);
            cmd.Parameters.AddWithValue("@User_Sum_Insure", flexi_term_policy_life_product.User_Sum_Insure);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicyLifeProduct] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new flexi term policy benefit item
    public static bool InsertFlexiTermPolicyBenefitItem(bl_flexi_term_policy_benefit_item flexi_term_policy_benefit_item)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy_Benefit_Item";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_Benefit_Item_ID", flexi_term_policy_benefit_item.Flexi_Term_Policy_Benefit_Item_ID);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", flexi_term_policy_benefit_item.Relationship_Khmer);
            cmd.Parameters.AddWithValue("@Relationship", flexi_term_policy_benefit_item.Relationship);
            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_benefit_item.Flexi_Term_Policy_ID);
            cmd.Parameters.AddWithValue("@Percentage", flexi_term_policy_benefit_item.Percentage);
            cmd.Parameters.AddWithValue("@First_Name", flexi_term_policy_benefit_item.First_Name);
            cmd.Parameters.AddWithValue("@Last_Name", flexi_term_policy_benefit_item.Last_Name);
            cmd.Parameters.AddWithValue("@Birth_Date", flexi_term_policy_benefit_item.Birth_Date);
            cmd.Parameters.AddWithValue("@Seq_Number", flexi_term_policy_benefit_item.Seq_Number);
            cmd.Parameters.AddWithValue("@Family_Book", flexi_term_policy_benefit_item.Family_Book);
            cmd.Parameters.AddWithValue("@ID_Type", flexi_term_policy_benefit_item.ID_Type);
            cmd.Parameters.AddWithValue("@ID_Card", flexi_term_policy_benefit_item.ID_Card);
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicyBenefitItem] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new flexi term policy status
    public static bool InsertFlexiTermPolicyStatus(bl_flexi_term_policy_status flexi_term_policy_status)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy_Status";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_status.Flexi_Term_Policy_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", flexi_term_policy_status.Policy_Status_Type_ID);

            cmd.Parameters.AddWithValue("@Created_On", flexi_term_policy_status.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", flexi_term_policy_status.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", flexi_term_policy_status.Created_Note);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicyStatus] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new flexi term policy premium
    public static bool InsertFlexiTermPolicyPremium(bl_flexi_term_policy_premium flexi_term_policy_premium)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy_Premium";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_premium.Flexi_Term_Policy_ID);
            cmd.Parameters.AddWithValue("@Original_Amount", flexi_term_policy_premium.Original_Amount);
            cmd.Parameters.AddWithValue("@Premium", flexi_term_policy_premium.Premium);
            cmd.Parameters.AddWithValue("@Sum_Insure", flexi_term_policy_premium.Sum_Insure);
       
            cmd.Parameters.AddWithValue("@Created_On", flexi_term_policy_premium.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", flexi_term_policy_premium.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", flexi_term_policy_premium.Created_Note);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicyPremium] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_ID
    public static bool DeletePolicyID(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_micro_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeletePolicyID] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Number by policy_id
    public static bool DeletePolicyNumber(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Number";

            cmd.Parameters.AddWithValue("@Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeletePolicyNumber] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Flexi_Term_Policy_Benefit_Item
    public static bool DeleteFlexiTermPolicyBenefitItem(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Flexi_Term_Policy_Benefit_Item";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteFlexiTermPolicyBenefitItem] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Flexi_Term_Policy_Premium
    public static bool DeleteFlexiTermPolicyPremium(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Flexi_Term_Policy_Premium";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteFlexiTermPolicyPremium] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Flexi_Term_Policy_Life_Product
    public static bool DeleteFlexiTermPolicyLifeProduct(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Flexi_Term_Policy_Life_Product";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteFlexiTermPolicyLifeProduct] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Flexi_Term_Policy_Info_Person
    public static bool DeleteFlexiTermPolicyInfoPerson(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Flexi_Term_Policy_Info_Person";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteFlexiTermPolicyInfoPerson] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }
       
    //Delete Ct_Flexi_Term_Policy_Ct_Banc_Card
    public static bool DeleteFlexiTermPolicyCard(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Flexi_Term_Policy_Banc_Card";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteFlexiTermPolicyCard] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Flexi_Term_Policy
    public static bool DeleteFlexiTermPolicy(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Flexi_Term_Policy";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteFlexiTermPolicy] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Flexi_Term_Policy_Status
    public static bool DeleteFlexiTermPolicyStatus(string flexi_term_policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Flexi_Term_Policy_Status";

            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteFlexiTermPolicyStatus] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to check existing customer flexi term in Ct_Customer_flexi_term
    public static bool CheckExistingCustomerFlexiTerm(string first_name, string last_name, int gender, DateTime dob)
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
                myCommand.CommandText = "SP_Check_Existing_Customer_Flexi_Term";
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name); 
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@Birth_Date", dob);

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
            Log.AddExceptionToLog("Error in function [CheckExistingCustomerFlexiTerm] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }
        return result;
    }

    //Function to check data in flexi term temp table (primary data)
    public static bool CheckFlexiTermDataInTemp(string branch, string bank_account_number, string surname, string first_name, DateTime dob, int gender, string id_number, int id_type, int resident, string beneficiary_surname, string beneficiary_first_name, string beneficiary_id_number, int beneficiary_id_type, string relationship, int family_book)
    {
        bool result = false;
        string connString = AppConfiguration.GetFlexiTermConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Check_Flexi_Term_Primary_Data";

                myCommand.Parameters.AddWithValue("@Branch", branch);
                myCommand.Parameters.AddWithValue("@Bank_Number", bank_account_number);
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", surname);
                myCommand.Parameters.AddWithValue("@DOB", dob);
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@ID_Type", id_type);
                myCommand.Parameters.AddWithValue("@ID_Card", id_number);
                myCommand.Parameters.AddWithValue("@Application_Resident", resident);
                myCommand.Parameters.AddWithValue("@Beneficiary_First_Name", beneficiary_first_name);
                myCommand.Parameters.AddWithValue("@Beneficiary_Last_Name", beneficiary_surname);
                myCommand.Parameters.AddWithValue("@Beneficiary_ID_Type", beneficiary_id_type);
                myCommand.Parameters.AddWithValue("@Beneficiary_ID_Card", beneficiary_id_number);
                myCommand.Parameters.AddWithValue("@Beneficiary_Relationship", relationship);
                myCommand.Parameters.AddWithValue("@Family_Book", family_book);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                       
                        result = true;

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
            Log.AddExceptionToLog("Error in function [CheckFlexiTermDataInTemp] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }
        return result;
    }

    //Function to get status in flexi term temp table (primary data)
    public static string GetFlexiTermStatusInTemp(string branch, string bank_account_number, string surname, string first_name, DateTime dob, int gender, string id_number, int id_type, int resident, string beneficiary_surname, string beneficiary_first_name, string beneficiary_id_number, int beneficiary_id_type, string relationship, int family_book)
    {
        string status = "";
        string connString = AppConfiguration.GetFlexiTermConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Flexi_Term_Primary_Data_Status_Code";

                myCommand.Parameters.AddWithValue("@Branch", branch);
                myCommand.Parameters.AddWithValue("@Bank_Number", bank_account_number);
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", surname);
                myCommand.Parameters.AddWithValue("@DOB", dob);
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@ID_Type", id_type);
                myCommand.Parameters.AddWithValue("@ID_Card", id_number);
                myCommand.Parameters.AddWithValue("@Application_Resident", resident);
                myCommand.Parameters.AddWithValue("@Beneficiary_First_Name", beneficiary_first_name);
                myCommand.Parameters.AddWithValue("@Beneficiary_Last_Name", beneficiary_surname);
                myCommand.Parameters.AddWithValue("@Beneficiary_ID_Type", beneficiary_id_type);
                myCommand.Parameters.AddWithValue("@Beneficiary_ID_Card", beneficiary_id_number);
                myCommand.Parameters.AddWithValue("@Beneficiary_Relationship", relationship);
                myCommand.Parameters.AddWithValue("@Family_Book", family_book);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        status = myReader.GetString(myReader.GetOrdinal("Status_Code"));

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
            Log.AddExceptionToLog("Error in function [GetFlexiTermStatusInTemp] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }
        return status;
    }

    //Insert new Ct_Customer_Flexi_Term_Customer
    public static bool InsertCustomerFlexiTermCustomer(bl_customer_flexi_term_customer customer_flexi_term_customer)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Customer_Flexi_Term_Customer";

            cmd.Parameters.AddWithValue("@Customer_ID", customer_flexi_term_customer.Customer_ID);
            cmd.Parameters.AddWithValue("@Customer_Flexi_Term_ID", customer_flexi_term_customer.Customer_Flexi_Term_ID);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertCustomerFlexiTermCustomer] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get Customer_Flexi_Term_ID
    public static string GetCustomerFlexiTermID(string first_name, string last_name, int gender, DateTime dob)
    {
        string customer_flexi_term_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Customer_Flexi_Term_ID";
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name);
          
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@Birth_Date", dob);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        customer_flexi_term_id = myReader.GetString(myReader.GetOrdinal("Customer_Flexi_Term_ID"));
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
            Log.AddExceptionToLog("Error in function [GetCustomerFlexiTermID] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }
        return customer_flexi_term_id;
    }


    //Function to check existing bank number in flexi term policy for this channel item
    public static bool CheckExistingFlexiTermBankNumberByChannelItemID(string bank_number, string channel_item_id)
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
                myCommand.CommandText = "SP_Check_Existing_Flexi_Term_Bank_Number_By_Channel_Item_ID";
                myCommand.Parameters.AddWithValue("@Bank_Number", bank_number);
                myCommand.Parameters.AddWithValue("@Channel_Item_ID", channel_item_id);
             
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
            Log.AddExceptionToLog("Error in function [CheckExistingFlexiTermBankNumberByChannelItemID] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }
        return result;
    }

    //Function to get flexi term primary data by params
    public static bl_flexi_term_primary_data GetFlexiTermPrimaryDataByParams(string branch, string bank_number, string last_name, string first_name, DateTime dob, int gender)
    {
        bl_flexi_term_primary_data flexi_term_primary_data = new bl_flexi_term_primary_data();

        string connString = AppConfiguration.GetFlexiTermConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Flexi_Term_Primary_Data_By_Params";

                myCommand.Parameters.AddWithValue("@Branch", branch);
                myCommand.Parameters.AddWithValue("@Bank_Number", bank_number);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name);
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@DOB", dob);
                myCommand.Parameters.AddWithValue("@Gender", gender);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        flexi_term_primary_data.Flexi_Term_Primary_Data_ID = myReader.GetString(myReader.GetOrdinal("Flexi_Term_Primary_Data_ID"));
                        flexi_term_primary_data.Bank_Number = myReader.GetString(myReader.GetOrdinal("Bank_Number"));
                        flexi_term_primary_data.Branch = myReader.GetString(myReader.GetOrdinal("Branch"));
                        flexi_term_primary_data.Channel_Location_ID = myReader.GetString(myReader.GetOrdinal("Channel_Location_ID"));
                        flexi_term_primary_data.Channel_Channel_Item_ID = myReader.GetString(myReader.GetOrdinal("Channel_Channel_Item_ID"));
                                               
                        flexi_term_primary_data.Created_On = myReader.GetDateTime(myReader.GetOrdinal("Created_On"));
                        flexi_term_primary_data.Created_By = myReader.GetString(myReader.GetOrdinal("Created_By"));
                        flexi_term_primary_data.DOB = myReader.GetDateTime(myReader.GetOrdinal("DOB"));
                        flexi_term_primary_data.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        flexi_term_primary_data.Maturity_Date = myReader.GetDateTime(myReader.GetOrdinal("Maturity_Date"));
                        flexi_term_primary_data.Issue_Date = myReader.GetDateTime(myReader.GetOrdinal("Issue_Date"));
                        flexi_term_primary_data.Status_Code = myReader.GetString(myReader.GetOrdinal("Status_Code"));
                        flexi_term_primary_data.ID_Type = myReader.GetInt32(myReader.GetOrdinal("ID_Type"));
                        
                        flexi_term_primary_data.Updated_On = myReader.GetDateTime(myReader.GetOrdinal("Updated_On"));
                        flexi_term_primary_data.Updated_By = myReader.GetString(myReader.GetOrdinal("Updated_By"));
                        flexi_term_primary_data.Is_Update = myReader.GetInt32(myReader.GetOrdinal("Is_Update"));
                        flexi_term_primary_data.Approved_On = myReader.GetDateTime(myReader.GetOrdinal("Approved_On"));
                        flexi_term_primary_data.Approved_By = myReader.GetString(myReader.GetOrdinal("Approved_By"));

                        flexi_term_primary_data.First_Name = myReader.GetString(myReader.GetOrdinal("First_Name"));
                        flexi_term_primary_data.ID_Card = myReader.GetString(myReader.GetOrdinal("ID_Card"));
                        flexi_term_primary_data.Last_Name = myReader.GetString(myReader.GetOrdinal("Last_Name"));
                        flexi_term_primary_data.Premium = myReader.GetDouble(myReader.GetOrdinal("Premium"));
                        flexi_term_primary_data.Sum_Insured = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));

                        flexi_term_primary_data.Gender = myReader.GetInt32(myReader.GetOrdinal("Gender"));
                          
                        flexi_term_primary_data.Application_Resident = myReader.GetInt32(myReader.GetOrdinal("Application_Resident"));
                        
                        flexi_term_primary_data.Family_Book = myReader.GetInt32(myReader.GetOrdinal("Family_Book"));
                        
                        flexi_term_primary_data.Beneficiary_First_Name = myReader.GetString(myReader.GetOrdinal("Beneficiary_First_Name"));
                        flexi_term_primary_data.Beneficiary_Last_Name = myReader.GetString(myReader.GetOrdinal("Beneficiary_Last_Name"));

                        flexi_term_primary_data.Beneficiary_ID_Type = myReader.GetInt32(myReader.GetOrdinal("Beneficiary_ID_Type"));
                        flexi_term_primary_data.Beneficiary_ID_Card = myReader.GetString(myReader.GetOrdinal("Beneficiary_ID_Card"));
                        
                        flexi_term_primary_data.Beneficiary_Relationship = myReader.GetString(myReader.GetOrdinal("Beneficiary_Relationship"));

                        flexi_term_primary_data.Age_Insured = myReader.GetInt32(myReader.GetOrdinal("Age_Insured"));
                        flexi_term_primary_data.Agreement_Date = myReader.GetDateTime(myReader.GetOrdinal("Agreement_Date"));
                      

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
            Log.AddExceptionToLog("Error in function [GetFlexiTermPrimaryDataByParams] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }
        

        return flexi_term_primary_data;

    }

    //Function to get last policy number
    public static string GetLastPolicyNumberFlexiTerm()
    {
        string last_policy_number = "0";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Last_Policy_Number";

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_channel_location channel_location = new bl_channel_location();

                        last_policy_number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));

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
            Log.AddExceptionToLog("Error in function [GetLastPolicyNumberFlexiTerm] in class [da_flexi_term_policy]. Details: " + ex.Message);
        }
        return last_policy_number;
    }

    //Insert new flexi term policy prem pay
    public static bool InsertFlexiTermPolicyPremPay(bl_flexi_term_policy_prem_pay flexi_term_policy_prem_pay)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Flexi_Term_Policy_Prem_Pay";


            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_ID", flexi_term_policy_prem_pay.Flexi_Term_Policy_ID);
            cmd.Parameters.AddWithValue("@Flexi_Term_Policy_Prem_Pay_ID", flexi_term_policy_prem_pay.Flexi_Term_Policy_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@Amount", flexi_term_policy_prem_pay.Amount);
            cmd.Parameters.AddWithValue("@Channel_Location_ID", flexi_term_policy_prem_pay.Channel_Location_ID);
            cmd.Parameters.AddWithValue("@Created_On", flexi_term_policy_prem_pay.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", flexi_term_policy_prem_pay.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", flexi_term_policy_prem_pay.Created_Note);
            cmd.Parameters.AddWithValue("@Due_Date", flexi_term_policy_prem_pay.Due_Date);
            cmd.Parameters.AddWithValue("@Pay_Date", flexi_term_policy_prem_pay.Pay_Date);
            cmd.Parameters.AddWithValue("@Prem_Lot", flexi_term_policy_prem_pay.Prem_Lot);
            cmd.Parameters.AddWithValue("@Prem_Year", flexi_term_policy_prem_pay.Prem_Year);
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", flexi_term_policy_prem_pay.Sale_Agent_ID);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertFlexiTermPolicyPremPay] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Get age
    public static int GetAge(string dob, string date_of_entry)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime my_date_of_entry = System.DateTime.Now;

        if (date_of_entry != "")
        {
            my_date_of_entry = Convert.ToDateTime(date_of_entry, dtfi);
        }

        int customer_age = Calculation.Culculate_Customer_Age(dob, my_date_of_entry);

        if (customer_age < 0)
        {
            customer_age = 0;
        }
        return customer_age;
    }

    //View Report List Flexi Term
    public static List<bl_flexi_term_policy_report> GetPolicyReportList(DateTime from_date, DateTime to_date, string check_policy_status_code, int order_by, string channel_item_id)
    {
        List<bl_flexi_term_policy_report> policy_report_list = new List<bl_flexi_term_policy_report>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            string sql = "", Order_by = "";

            if (check_policy_status_code == "")
            {
                sql = @"select * from V_Report_Policy_Flexi_Term_Complete where 
                                (CONVERT(nvarchar(10),V_Report_Policy_Flexi_Term_Complete.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"select * from V_Report_Policy_Flexi_Term where 
                                (CONVERT(nvarchar(10),V_Report_Policy_Flexi_Term_Complete.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (lower(V_Report_Policy_Flexi_Term_Complete.Policy_Status_Type_ID) in (" + check_policy_status_code + "))";

            }

            if (order_by == 1) // Policy number
            {
                Order_by = " order by V_Report_Policy_Flexi_Term_Complete.Policy_Number asc, V_Report_Policy_Flexi_Term_Complete.Last_Name asc";
            }
            else if (order_by == 2) // Issued date
            {
                Order_by = " order by V_Report_Policy_Flexi_Term_Complete.Issue_Date asc, V_Report_Policy_Flexi_Term_Complete.Last_Name asc";
            }
            else if (order_by == 3)
            {
                Order_by = " order by V_Report_Policy_Flexi_Term_Complete.Effective_Date asc, V_Report_Policy_Flexi_Term_Complete.Last_Name asc";
            }
            
            SqlCommand cmd = new SqlCommand(sql, con);

            if (channel_item_id != "")
            {
                sql += " and V_Report_Policy_Flexi_Term_Complete.Channel_Item_ID = @Channel_Item_ID  ";
                cmd.Parameters.AddWithValue("@Channel_Item_ID", channel_item_id);
            }

            cmd.CommandText = sql + Order_by;

            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    bl_flexi_term_policy_report policy_report = new bl_flexi_term_policy_report();

                    policy_report.Card_ID = rdr.GetString(rdr.GetOrdinal("Card_ID"));
                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.Card_Number = rdr.GetString(rdr.GetOrdinal("Card_Number"));
                    policy_report.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    policy_report.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                                       
                    policy_report.En_Abbr = rdr.GetString(rdr.GetOrdinal("En_Abbr"));

                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));

                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                    policy_report.Maturity_Date = rdr.GetDateTime(rdr.GetOrdinal("Maturity_Date"));
                    policy_report.Expiry_Date = policy_report.Maturity_Date.AddDays(-1);
                    policy_report.Pay_Mode = da_payment_mode.GetPaymentModeByPayModeID(rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"))).Mode;

                    policy_report.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));

                    policy_report.Flexi_Term_Policy_ID = rdr.GetString(rdr.GetOrdinal("Flexi_Term_Policy_ID"));

                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    policy_report.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    policy_report.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));

                    policy_report.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));

                    policy_report.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));
                    policy_report.Bank_Number = rdr.GetString(rdr.GetOrdinal("Bank_Number"));
                    policy_report.Beneficiary_First_Name = rdr.GetString(rdr.GetOrdinal("Beneficiary_First_Name"));
                    policy_report.Beneficiary_Last_Name = rdr.GetString(rdr.GetOrdinal("Beneficiary_Last_Name"));
                    policy_report.Beneficiary_ID_Card = rdr.GetString(rdr.GetOrdinal("Beneficiary_ID_Card"));
                    policy_report.Beneficiary_ID_Type = rdr.GetInt32(rdr.GetOrdinal("Beneficiary_ID_Type"));
                    policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                    policy_report.Branch = rdr.GetString(rdr.GetOrdinal("Branch"));
                    policy_report.Customer_Flexi_Term_Number = rdr.GetString(rdr.GetOrdinal("Customer_Flexi_Term_Number"));
                    policy_report.Family_Book = rdr.GetInt32(rdr.GetOrdinal("Family_Book"));
                    policy_report.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));


                    //Case ID Type
                    switch (policy_report.ID_Type)
                    {
                        case 0:
                            policy_report.Str_ID_Type = "I.D Card";
                            break;
                        case 1:
                            policy_report.Str_ID_Type = "Passport";
                            break;
                        case 2:
                            policy_report.Str_ID_Type = "Visa";
                            break;
                        case 3:
                            policy_report.Str_ID_Type = "Birth Certificate";
                            break;
                        case 4:
                            policy_report.Str_ID_Type = "Police / Civil Service Card";
                            break;
                        case 5:
                            policy_report.Str_ID_Type = "Employment Book";
                            break;
                        case 6:
                            policy_report.Str_ID_Type = "Residential Book";
                            break;
                        case 7:                            
                            policy_report.Str_ID_Type = "Family Book";
                            break;                      
                    }

                    if (policy_report.Gender == 1)
                    {
                        policy_report.Str_Gender = "M";
                    }
                    else
                    {
                        policy_report.Str_Gender = "F";
                    }

                    policy_report_list.Add(policy_report);
                }
            }/// End loop
        }/// End of ConnectionString

        return policy_report_list;
    }


    //View Report List Flexi Term By Params
    public static List<bl_flexi_term_policy_report> GetPolicyReportListByParams(DateTime from_date, DateTime to_date, string check_policy_status_code, int order_by, int pay_mode, string policy_number, string poduct_id, string barcode,  string channel_item_id, int use_date)
    {
        List<bl_flexi_term_policy_report> policy_report_list = new List<bl_flexi_term_policy_report>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            string sql = "", Order_by = "";

            if (check_policy_status_code == "")
            {
                sql = @"select * from V_Report_Policy_Flexi_Term_Complete where V_Report_Policy_Flexi_Term_Complete.Flexi_Term_Policy_ID != '' ";

                if (use_date != 0)
                {
                    sql += @" AND (CONVERT(nvarchar(10),V_Report_Policy_Flexi_Term_Complete.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) ";
                }   
            }
            else
            {
                sql = @"select * from V_Report_Policy_Flexi_Term_Complete where lower(V_Report_Policy_Flexi_Term_Complete.Policy_Status_Type_ID) in (" + check_policy_status_code + ") ";
               
                if (use_date != 0)
                {
                    sql += @" AND (CONVERT(nvarchar(10),V_Report_Policy_Flexi_Term_Complete.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) ";
                }               
            }

            if (order_by == 1) // Policy number
            {
                Order_by = " order by V_Report_Policy_Flexi_Term_Complete.Policy_Number asc, V_Report_Policy_Flexi_Term_Complete.Last_Name asc";
            }
            else if (order_by == 2) // Issued date
            {
                Order_by = " order by V_Report_Policy_Flexi_Term_Complete.Issue_Date asc, V_Report_Policy_Flexi_Term_Complete.Last_Name asc";
            }
            else if (order_by == 3)
            {
                Order_by = " order by V_Report_Policy_Flexi_Term_Complete.Effective_Date asc, V_Report_Policy_Flexi_Term_Complete.Last_Name asc";
            }

            SqlCommand cmd = new SqlCommand(sql, con);

            
            if (channel_item_id != "")
            {
                sql += " and V_Report_Policy_Flexi_Term_Complete.Channel_Item_ID = @Channel_Item_ID  ";
                cmd.Parameters.AddWithValue("@Channel_Item_ID", channel_item_id);
            }

            if (pay_mode != -1)
            {
                sql += " and V_Report_Policy_Flexi_Term_Complete.Pay_Mode = @Pay_Mode  ";
                cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);
            }

            if (policy_number != "")
            {
                sql += " and V_Report_Policy_Flexi_Term_Complete.Policy_Number = @Policy_Number  ";
                cmd.Parameters.AddWithValue("@Policy_Number", policy_number);
            }

            if (poduct_id != "-1")
            {
                sql += " and V_Report_Policy_Flexi_Term_Complete.Product_ID = @Product_ID  ";
                cmd.Parameters.AddWithValue("@Product_ID", poduct_id);
            }

            if (barcode != "")
            {
                sql += " and V_Report_Policy_Flexi_Term_Complete.Product_ID = @Card_ID  ";
                cmd.Parameters.AddWithValue("@Card_ID", barcode);
            }

            if (barcode != "")
            {
                sql += " and V_Report_Policy_Flexi_Term_Complete.Product_ID = @Card_ID  ";
                cmd.Parameters.AddWithValue("@Card_ID", barcode);
            }

            cmd.CommandText = sql + Order_by;

            if (use_date != 0)
            {
                cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));
            }
                                  
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    bl_flexi_term_policy_report policy_report = new bl_flexi_term_policy_report();

                    policy_report.Card_ID = rdr.GetString(rdr.GetOrdinal("Card_ID"));
                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.Card_Number = rdr.GetString(rdr.GetOrdinal("Card_Number"));
                    policy_report.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    policy_report.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));

                    policy_report.En_Abbr = rdr.GetString(rdr.GetOrdinal("En_Abbr"));

                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));

                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                    policy_report.Maturity_Date = rdr.GetDateTime(rdr.GetOrdinal("Maturity_Date"));
                    policy_report.Expiry_Date = policy_report.Maturity_Date.AddDays(-1);

                    policy_report.Pay_Mode = da_payment_mode.GetPaymentModeByPayModeID(rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"))).Mode;

                    policy_report.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));

                    policy_report.Flexi_Term_Policy_ID = rdr.GetString(rdr.GetOrdinal("Flexi_Term_Policy_ID"));

                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    policy_report.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    policy_report.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));

                    policy_report.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));

                    policy_report.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));
                    policy_report.Bank_Number = rdr.GetString(rdr.GetOrdinal("Bank_Number"));
                    policy_report.Beneficiary_First_Name = rdr.GetString(rdr.GetOrdinal("Beneficiary_First_Name"));
                    policy_report.Beneficiary_Last_Name = rdr.GetString(rdr.GetOrdinal("Beneficiary_Last_Name"));
                    policy_report.Beneficiary_ID_Card = rdr.GetString(rdr.GetOrdinal("Beneficiary_ID_Card"));
                    policy_report.Beneficiary_ID_Type = rdr.GetInt32(rdr.GetOrdinal("Beneficiary_ID_Type"));
                    policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                    policy_report.Branch = rdr.GetString(rdr.GetOrdinal("Branch"));
                    policy_report.Customer_Flexi_Term_Number = rdr.GetString(rdr.GetOrdinal("Customer_Flexi_Term_Number"));
                    policy_report.Family_Book = rdr.GetInt32(rdr.GetOrdinal("Family_Book"));
                    policy_report.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                                        

                    //Case ID Type
                    switch (policy_report.ID_Type)
                    {
                        case 0:
                            policy_report.Str_ID_Type = "I.D Card";
                            break;
                        case 1:
                            policy_report.Str_ID_Type = "Passport";
                            break;
                        case 2:
                            policy_report.Str_ID_Type = "Visa";
                            break;
                        case 3:
                            policy_report.Str_ID_Type = "Birth Certificate";
                            break;
                        case 4:
                            policy_report.Str_ID_Type = "Police / Civil Service Card";
                            break;
                        case 5:
                            policy_report.Str_ID_Type = "Employment Book";
                            break;
                        case 6:
                            policy_report.Str_ID_Type = "Residential Book";
                            break;
                        case 7:
                            policy_report.Str_ID_Type = "Family Book";
                            break;
                    }

                    if (policy_report.Gender == 1)
                    {
                        policy_report.Str_Gender = "M";
                    }
                    else
                    {
                        policy_report.Str_Gender = "F";
                    }

                    policy_report_list.Add(policy_report);
                }
            }/// End loop
        }/// End of ConnectionString

        return policy_report_list;
    }


    //Function to get udate premium company
    public static string GetPremium(double sum_insured, int age)
    {
        string premium = "";
        if (age > 60)
        {
            age = 59;
        }
        string connString = AppConfiguration.GetFlexiTermConnectionString();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {

                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "SP_Get_Flexi_Term_Premium";
                    myCommand.Parameters.AddWithValue("@Sum_Insured", sum_insured);
                    myCommand.Parameters.AddWithValue("@Age", age);
                    using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (myReader.Read())
                        {
                            premium = myReader.GetString(myReader.GetOrdinal("Premium"));
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
                Log.AddExceptionToLog("Error in function [GetFlexiTermPrimaryData] in class [da_flexi_term_policy]. Details: " + ex.Message);
            }
        
        return premium;
      
    }

    

    #endregion

}