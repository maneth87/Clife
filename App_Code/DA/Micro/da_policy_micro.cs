using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

/// <summary>
/// Summary description for da_policy_micro
/// </summary>
public class da_policy_micro
{
    private static da_policy_micro mytitle = null;

    #region "Constructors"

    public da_policy_micro()
	{
        if (mytitle == null)
        {
            mytitle = new da_policy_micro();
        }
    }

    #endregion

    #region "Public Functions"

    //Insert new policy then return policy ID
    public static string InsertPolicyMicro(bl_policy_micro policy_micro)
    {
        string policy_micro_id = "";

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro";

            //get new primary key for the row to be inserted
            policy_micro.Policy_Micro_ID = Helper.GetNewGuid("SP_Check_Policy_Micro_ID", "@Policy_Micro_ID").ToString();

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Customer_Micro_ID", policy_micro.Customer_ID);               
            cmd.Parameters.AddWithValue("@Product_ID", policy_micro.Product_ID);
            cmd.Parameters.AddWithValue("@Effective_Date", policy_micro.Effective_Date);
            cmd.Parameters.AddWithValue("@Maturity_Date", policy_micro.Maturity_Date);
            cmd.Parameters.AddWithValue("@Agreement_Date", policy_micro.Agreement_Date);
            cmd.Parameters.AddWithValue("@Issue_Date", policy_micro.Issue_Date);
            cmd.Parameters.AddWithValue("@Age_Insure", policy_micro.Age_Insure);
            cmd.Parameters.AddWithValue("@Pay_Year", policy_micro.Pay_Year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", policy_micro.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", policy_micro.Assure_Year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", policy_micro.Assure_Up_To_Age);
           
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", policy_micro.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", "");
            cmd.Parameters.AddWithValue("@Channel_Location_ID", policy_micro.Channel_Location_ID);
            cmd.Parameters.AddWithValue("@Channel_Channel_Item_ID", policy_micro.Channel_Channel_Item_ID);


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();   
                con.Close();
                policy_micro_id = policy_micro.Policy_Micro_ID;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyMicro] in class [da_policy_Micro]. Details: " + ex.Message);
            }
        }
        return policy_micro_id;
    }

    //Insert new micro customer then return ID
    public static string InsertMicroCustomer(bl_micro_customer micro_customer)
    {
        string micro_customer_id = "";

        //get last micro customer number
        string last_micro_customer = da_policy_micro.GetLastMicroCustomerNumber();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Micro_Customer";

            //get new primary key for the row to be inserted
            micro_customer.Customer_Micro_ID = Helper.GetNewGuid("SP_Check_Micro_Customer_ID", "@Customer_Micro_ID").ToString();
            micro_customer.Customer_Micro_Number = last_micro_customer;
            cmd.Parameters.AddWithValue("@Customer_Micro_ID", micro_customer.Customer_Micro_ID);
            cmd.Parameters.AddWithValue("@Customer_Micro_Number", micro_customer.Customer_Micro_Number);
            cmd.Parameters.AddWithValue("@Created_On", micro_customer.Created_On);
            cmd.Parameters.AddWithValue("@Created_Note", micro_customer.Created_Note);
            cmd.Parameters.AddWithValue("@Created_By", micro_customer.Created_By);
            cmd.Parameters.AddWithValue("@Birth_Date", micro_customer.Birth_Date);
            cmd.Parameters.AddWithValue("@First_Name", micro_customer.First_Name);
            cmd.Parameters.AddWithValue("@Gender", micro_customer.Gender);
            cmd.Parameters.AddWithValue("@ID_Card", micro_customer.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", micro_customer.ID_Type);
            cmd.Parameters.AddWithValue("@Khmer_First_Name", micro_customer.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", micro_customer.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Last_Name", micro_customer.Last_Name);
           
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                micro_customer_id = micro_customer.Customer_Micro_ID;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertMicroCustomer] in class [da_policy_Micro]. Details: " + ex.Message);
            }
        }
        return micro_customer_id;
    }

    //Insert new Ct_Customer_Micro_Customer
    public static bool InsertCustomerMicroCustomer(bl_ct_customer_micro_customer customer_micro_customer)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Customer_Micro_Customer";

            cmd.Parameters.AddWithValue("@Customer_ID", customer_micro_customer.Customer_ID);
            cmd.Parameters.AddWithValue("@Customer_Micro_ID", customer_micro_customer.Customer_Micro_ID);
        
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
                Log.AddExceptionToLog("Error in function [InsertCustomerMicroCustomer] in class [da_policy_Micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_Policy_ID
    public static bool InsertPolicyID(string policy_id, int policy_type_id)
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
                Log.AddExceptionToLog("Error in function [InsertPolicyID] in class [da_policy_Micro]. Details: " + ex.Message);
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
                Log.AddExceptionToLog("Error in function [InsertPolicyNumber] in class [da_policy_Micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get last policy number
    public static string GetLastPolicyNumberMicro()
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
            Log.AddExceptionToLog("Error in function [GetLastPolicyNumberMicro] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return last_policy_number;
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

    //Insert new policy micro card
    public static bool InsertPolicyMicroCard(bl_policy_micro_banc_card policy_micro_card)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Card";
        
            cmd.Parameters.AddWithValue("@Card_ID", policy_micro_card.Card_ID);
            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_card.Policy_Micro_ID);

            cmd.Parameters.AddWithValue("@Created_On", policy_micro_card.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy_micro_card.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_micro_card.Created_Note);          

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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroCard] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro Info Person
    public static bool InsertPolicyMicroInfoPerson(bl_policy_micro_info_person policy_micro_info_person)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Info_Person";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_info_person.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Birth_Date", policy_micro_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@First_Name", policy_micro_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", policy_micro_info_person.Gender);
            cmd.Parameters.AddWithValue("@ID_Card", policy_micro_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", policy_micro_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@Khmer_First_Name", policy_micro_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", policy_micro_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Last_Name", policy_micro_info_person.Last_Name.ToUpper());                  

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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroInfoPerson] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro Info Address
    public static bool InsertPolicyMicroInfoAddress(bl_policy_micro_info_address policy_micro_info_address)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Info_Address";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_info_address.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Address1", policy_micro_info_address.Address1);
            cmd.Parameters.AddWithValue("@Address2", policy_micro_info_address.Address2);
            cmd.Parameters.AddWithValue("@Address3", policy_micro_info_address.Address3);
            cmd.Parameters.AddWithValue("@Country_ID", policy_micro_info_address.Country_ID);
            cmd.Parameters.AddWithValue("@Province", policy_micro_info_address.Province);
            cmd.Parameters.AddWithValue("@Zip_Code", policy_micro_info_address.Zip_Code);
          
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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroInfoAddress] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro life product
    public static bool InsertPolicyMicroLifeProduct(bl_policy_micro_life_product policy_micro_life_product)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Life_Product";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_life_product.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Age_Insure", policy_micro_life_product.Age_Insure);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", policy_micro_life_product.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", policy_micro_life_product.Assure_Year);
            cmd.Parameters.AddWithValue("@Pay_Mode", policy_micro_life_product.Pay_Mode);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", policy_micro_life_product.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Pay_Year", policy_micro_life_product.Pay_Year);
            cmd.Parameters.AddWithValue("@Product_ID", policy_micro_life_product.Product_ID);
            cmd.Parameters.AddWithValue("@System_Premium", policy_micro_life_product.System_Premium);
            cmd.Parameters.AddWithValue("@System_Premium_Discount", policy_micro_life_product.System_Premium_Discount);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", policy_micro_life_product.System_Sum_Insure);
            cmd.Parameters.AddWithValue("@User_Premium", policy_micro_life_product.User_Premium);
            cmd.Parameters.AddWithValue("@User_Sum_Insure", policy_micro_life_product.User_Sum_Insure);

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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroLifeProduct] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro Info Contact
    public static bool InsertPolicyMicroInfoContact(bl_policy_micro_info_contact policy_micro_info_contact)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Info_Contact";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_info_contact.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@EMail", policy_micro_info_contact.EMail);
            cmd.Parameters.AddWithValue("@Fax1", policy_micro_info_contact.Fax1);
            cmd.Parameters.AddWithValue("@Fax2", policy_micro_info_contact.Fax2);
            cmd.Parameters.AddWithValue("@Home_Phone1", policy_micro_info_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", policy_micro_info_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Mobile_Phone1", policy_micro_info_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", policy_micro_info_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", policy_micro_info_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", policy_micro_info_contact.Office_Phone2);
         
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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroInfoContact] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro proposed insured item
    public static bool InsertPolicyMicroProposedInsuredItem(bl_policy_micro_proposed_insured_item policy_micro_proposed_insured_item)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Proposed_Insured_Item";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_proposed_insured_item.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", policy_micro_proposed_insured_item.Relationship_Khmer);
            cmd.Parameters.AddWithValue("@Relationship", policy_micro_proposed_insured_item.Relationship);
            cmd.Parameters.AddWithValue("@Policy_Micro_Proposed_Insured_Item_ID", policy_micro_proposed_insured_item.Policy_Micro_Proposed_Insured_Item_ID);
            cmd.Parameters.AddWithValue("@Percentage", policy_micro_proposed_insured_item.Percentage);
            cmd.Parameters.AddWithValue("@Full_Name", policy_micro_proposed_insured_item.Full_Name);
            cmd.Parameters.AddWithValue("@Age", policy_micro_proposed_insured_item.Age);
            cmd.Parameters.AddWithValue("@Address", policy_micro_proposed_insured_item.Address);
            cmd.Parameters.AddWithValue("@Seq_Number", policy_micro_proposed_insured_item.Seq_Number);         

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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroProposedInsuredItem] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro benefit item
    public static bool InsertPolicyMicroBenefitItem(bl_policy_micro_benefit_item policy_micro_benefit_item)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Benefit_Item";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_benefit_item.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", policy_micro_benefit_item.Relationship_Khmer);
            cmd.Parameters.AddWithValue("@Relationship", policy_micro_benefit_item.Relationship);
            cmd.Parameters.AddWithValue("@Policy_Micro_Benefit_Item_ID", policy_micro_benefit_item.Policy_Micro_Benefit_Item_ID);
            cmd.Parameters.AddWithValue("@Percentage", policy_micro_benefit_item.Percentage);
            cmd.Parameters.AddWithValue("@Full_Name", policy_micro_benefit_item.Full_Name);
            cmd.Parameters.AddWithValue("@Birth_Date", policy_micro_benefit_item.Birth_Date);          
            cmd.Parameters.AddWithValue("@Seq_Number", policy_micro_benefit_item.Seq_Number);

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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroBenefitItem] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro status
    public static bool InsertPolicyMicroStatus(bl_policy_micro_status policy_micro_status)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Status";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_status.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", policy_micro_status.Policy_Status_Type_ID);

            cmd.Parameters.AddWithValue("@Created_On", policy_micro_status.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy_micro_status.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_micro_status.Created_Note);

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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroStatus] in class [da_policy_micro]. Details: " + ex.Message);
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
                Log.AddExceptionToLog("Error in function [DeletePolicyID] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Number by policy_id
    public static bool DeletePolicyNumber(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Number";

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
                Log.AddExceptionToLog("Error in function [DeletePolicyNumber] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Micro_Benefit_Item
    public static bool DeletePolicyMicroBenefitItem(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Benefit";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroBenefitItem] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Micro_Proposed_Insured_Item
    public static bool DeletePolicyMicroProposedInsuredItem(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Proposed_Insured";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroProposedInsuredItem] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Micro_Info_Address
    public static bool DeletePolicyMicroInfoAddress(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Info_Address";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroInfoAddress] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Micro_Life_Product
    public static bool DeletePolicyMicroLifeProduct(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Life_Product";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroLifeProduct] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Micro_Info_Person
    public static bool DeletePolicyMicroInfoPerson(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Info_Person";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroInfoPerson] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_Policy_Micro_Info_Contact
    public static bool DeletePolicyMicroInfoContact(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Info_Contact";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroInfoContact] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }


    //Delete Ct_Policy_Micro_Ct_Banc_Card
    public static bool DeletePolicyMicroCard(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Card";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroCard] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Micro
    public static bool DeletePolicyMicro(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicro] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to search policy by barcode_number
    public static List<bl_policy_micro_search> GetPolicyByBarcodeNoInternal(string barcode_number)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();

        string connString = AppConfiguration.GetConnectionString();

        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Micro_By_Barcode_Number_Internal";
                myCommand.Parameters.AddWithValue("@Card_ID", barcode_number);
              
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_policy_micro_search policy = new bl_policy_micro_search();

                        policy.Policy_Micro_ID = myReader.GetString(myReader.GetOrdinal("Policy_Micro_ID"));
                        policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        policy.First_Name = myReader.GetString(myReader.GetOrdinal("First_Name"));

                        int id_type = myReader.GetInt32(myReader.GetOrdinal("ID_Type"));

                        switch (id_type)
                        {
                            case 0:
                                policy.ID_Type = "I.D Card";
                                break;
                            case 1:
                                policy.ID_Type = "Passport";
                                break;
                            case 2:
                                policy.ID_Type = "Visa";
                                break;
                            case 3:
                                policy.ID_Type = "Birth Certificate";
                                break;
                        }

                        policy.Birth_Date = myReader.GetDateTime(myReader.GetOrdinal("Birth_Date"));

                        int gender = myReader.GetInt32(myReader.GetOrdinal("Gender"));

                        if (gender == 1)
                        {
                            policy.Gender = "Male";
                        }
                        else
                        {
                            policy.Gender = "Female";
                        }

                        policy.ID_Card = myReader.GetString(myReader.GetOrdinal("ID_Card"));
                        policy.Last_Name = myReader.GetString(myReader.GetOrdinal("Last_Name"));
                        policy.Barcode = myReader.GetString(myReader.GetOrdinal("Card_ID"));
                        policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        policies.Add(policy);
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
            Log.AddExceptionToLog("Error in function [GetPolicyByBarcodeNoInternal] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return policies;
    }    

    //Function to search policy by policy_number internal
    public static List<bl_policy_micro_search> GetPolicyByPolicyNoInternal(string policy_number)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();

        string connString = AppConfiguration.GetConnectionString();

      try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Micro_By_Policy_Number_Internal";
                myCommand.Parameters.AddWithValue("@Policy_Number", policy_number);
         
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_policy_micro_search policy = new bl_policy_micro_search();

                        policy.Policy_Micro_ID = myReader.GetString(myReader.GetOrdinal("Policy_Micro_ID"));
                        policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        policy.First_Name = myReader.GetString(myReader.GetOrdinal("First_Name"));

                        int id_type = myReader.GetInt32(myReader.GetOrdinal("ID_Type"));

                        switch (id_type)
                        {
                            case 0:
                                policy.ID_Type = "I.D Card";
                                break;
                            case 1:
                                policy.ID_Type = "Passport";
                                break;
                            case 2:
                                policy.ID_Type = "Visa";
                                break;
                            case 3:
                                policy.ID_Type = "Birth Certificate";
                                break;
                        }

                        policy.Birth_Date = myReader.GetDateTime(myReader.GetOrdinal("Birth_Date"));

                        int gender = myReader.GetInt32(myReader.GetOrdinal("Gender"));

                        if (gender == 1)
                        {
                            policy.Gender = "Male";
                        }
                        else
                        {
                            policy.Gender = "Female";
                        }

                        policy.ID_Card = myReader.GetString(myReader.GetOrdinal("ID_Card"));
                        policy.Last_Name = myReader.GetString(myReader.GetOrdinal("Last_Name"));
                        policy.Barcode = myReader.GetString(myReader.GetOrdinal("Card_ID"));
                        policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        policies.Add(policy);
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
            Log.AddExceptionToLog("Error in function [GetPolicyByPolicyNoInternal] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return policies;
    }    

    //Function to search policy by customer name internal
    public static List<bl_policy_micro_search> GetPolicyByCustomerNameInternal(string first_name, string last_name)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Micro_By_Customer_Name_Internal";
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name);
           
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_policy_micro_search policy = new bl_policy_micro_search();

                        policy.Policy_Micro_ID = myReader.GetString(myReader.GetOrdinal("Policy_Micro_ID"));
                        policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        policy.First_Name = myReader.GetString(myReader.GetOrdinal("First_Name"));

                        int id_type = myReader.GetInt32(myReader.GetOrdinal("ID_Type"));

                        switch (id_type)
                        {
                            case 0:
                                policy.ID_Type = "I.D Card";
                                break;
                            case 1:
                                policy.ID_Type = "Passport";
                                break;
                            case 2:
                                policy.ID_Type = "Visa";
                                break;
                            case 3:
                                policy.ID_Type = "Birth Certificate";
                                break;
                        }

                        policy.Birth_Date = myReader.GetDateTime(myReader.GetOrdinal("Birth_Date"));

                        int gender = myReader.GetInt32(myReader.GetOrdinal("Gender"));

                        if (gender == 1)
                        {
                            policy.Gender = "Male";
                        }
                        else
                        {
                            policy.Gender = "Female";
                        }

                        policy.ID_Card = myReader.GetString(myReader.GetOrdinal("ID_Card"));
                        policy.Last_Name = myReader.GetString(myReader.GetOrdinal("Last_Name"));
                        policy.Barcode = myReader.GetString(myReader.GetOrdinal("Card_ID"));
                        policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        policies.Add(policy);
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
            Log.AddExceptionToLog("Error in function [GetPolicyByCustomerNameInternal] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return policies;
    }

    //Function to search policy by ID Card Internal
    public static List<bl_policy_micro_search> GetPolicyByIDCardInternal(int id_type, string id_card)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Micro_By_ID_Card_Internal";
                myCommand.Parameters.AddWithValue("@ID_Type", id_type);
                myCommand.Parameters.AddWithValue("@ID_Card", id_card);
      
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_policy_micro_search policy = new bl_policy_micro_search();

                        policy.Policy_Micro_ID = myReader.GetString(myReader.GetOrdinal("Policy_Micro_ID"));
                        policy.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        policy.First_Name = myReader.GetString(myReader.GetOrdinal("First_Name"));

                        int my_id_type = myReader.GetInt32(myReader.GetOrdinal("ID_Type"));

                        switch (my_id_type)
                        {
                            case 0:
                                policy.ID_Type = "I.D Card";
                                break;
                            case 1:
                                policy.ID_Type = "Passport";
                                break;
                            case 2:
                                policy.ID_Type = "Visa";
                                break;
                            case 3:
                                policy.ID_Type = "Birth Certificate";
                                break;
                        }

                        policy.Birth_Date = myReader.GetDateTime(myReader.GetOrdinal("Birth_Date"));

                        int gender = myReader.GetInt32(myReader.GetOrdinal("Gender"));

                        if (gender == 1)
                        {
                            policy.Gender = "Male";
                        }
                        else
                        {
                            policy.Gender = "Female";
                        }

                        policy.ID_Card = myReader.GetString(myReader.GetOrdinal("ID_Card"));
                        policy.Last_Name = myReader.GetString(myReader.GetOrdinal("Last_Name"));
                        policy.Barcode = myReader.GetString(myReader.GetOrdinal("Card_ID"));
                        policy.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        policies.Add(policy);
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
            Log.AddExceptionToLog("Error in function [GetPolicyByIDCardInternal] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return policies;
    }

    //Function to get policy micro single row data by id
    public static bl_policy_micro_single_row_data GetPolicySingleRowData(string policy_micro_id)
    {

        bl_policy_micro_single_row_data policy_single_row_data = new bl_policy_micro_single_row_data();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Policy_Micro_Single_Row_Data_By_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    if (rdr.HasRows)
                    {
                        policy_single_row_data.Policy_Micro_ID = rdr.GetString(rdr.GetOrdinal("Policy_Micro_ID"));
                        policy_single_row_data.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                        policy_single_row_data.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));                        
                        policy_single_row_data.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                        policy_single_row_data.Barcode = rdr.GetString(rdr.GetOrdinal("Card_ID"));
                        policy_single_row_data.Card_Number = GetCardNumberByCardID(policy_single_row_data.Barcode);//Get Card Number
                        policy_single_row_data.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                        policy_single_row_data.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));

                        //Get sale agent name                                            
                        policy_single_row_data.Sale_Agent_Full_Name = GetSaleAgentNameByAgentCode(policy_single_row_data.Sale_Agent_ID);

                        policy_single_row_data.Country = rdr.GetString(rdr.GetOrdinal("Country"));
                        policy_single_row_data.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                        policy_single_row_data.Address2 = rdr.GetString(rdr.GetOrdinal("Address2"));
                        policy_single_row_data.Province = rdr.GetString(rdr.GetOrdinal("Province"));
                        policy_single_row_data.Zip_Code = rdr.GetString(rdr.GetOrdinal("Zip_Code"));

                        policy_single_row_data.Mobile_Phone1 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone1"));

                        policy_single_row_data.EMail = rdr.GetString(rdr.GetOrdinal("EMail"));
                        policy_single_row_data.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                        policy_single_row_data.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                        policy_single_row_data.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                        policy_single_row_data.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                        policy_single_row_data.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));
                        policy_single_row_data.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));                 
                        policy_single_row_data.Khmer_First_Name = rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));
                        policy_single_row_data.Khmer_Last_Name = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name"));

                        policy_single_row_data.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                        policy_single_row_data.Product = da_product.GetProductByProductID(policy_single_row_data.Product_ID).En_Title;

                        policy_single_row_data.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));
                        policy_single_row_data.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));

                        policy_single_row_data.Assure_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));

                        policy_single_row_data.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("User_Sum_Insure"));

                        policy_single_row_data.User_Premium = rdr.GetDouble(rdr.GetOrdinal("User_Premium"));

                        policy_single_row_data.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));

                        policy_single_row_data.Channel_Location_ID = rdr.GetString(rdr.GetOrdinal("Channel_Location_ID"));

                        policy_single_row_data.Channel_Item_ID = rdr.GetString(rdr.GetOrdinal("Channel_Item_ID"));

                        policy_single_row_data.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));

                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetPolicySingleRowData] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return policy_single_row_data;
    }

    //Get Card_Number by Card_id
    public static string GetCardNumberByCardID(string card_id)
    {

        string card_number = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Card_Number_By_Card_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Card_ID";
                paramName.Value = card_id ;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                      
                        card_number = rdr.GetString(rdr.GetOrdinal("Card_Number"));
                                         
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetCardNumberByCardID] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return card_number;
    }

    //Get List of Benefit_Items by policy_id
    public static List<bl_policy_micro_benefit_item> GetPolicyBenefitItem(string policy_id)
    {

        List<bl_policy_micro_benefit_item> benefit_items = new List<bl_policy_micro_benefit_item>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Policy_Micro_Benefit_Item_By_Policy_Micro_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Policy_Micro_ID";
                paramName.Value = policy_id;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        bl_policy_micro_benefit_item benefit_item = new bl_policy_micro_benefit_item();

                        benefit_item.Policy_Micro_ID = rdr.GetString(rdr.GetOrdinal("Policy_Micro_ID"));
                        benefit_item.Policy_Micro_Benefit_Item_ID = rdr.GetString(rdr.GetOrdinal("Policy_Micro_Benefit_Item_ID"));
                        benefit_item.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                        benefit_item.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        benefit_item.Percentage = rdr.GetDouble(rdr.GetOrdinal("Percentage"));
                        benefit_item.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                        benefit_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));
                    
                        benefit_items.Add(benefit_item);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetPolicyBenefitItem] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return benefit_items;
    }
    //Get List of proposed item by policy_id
    public static List<bl_policy_micro_proposed_insured_item> GetPolicyProposedItem(string policy_id)
    {

        List<bl_policy_micro_proposed_insured_item> proposed_items = new List<bl_policy_micro_proposed_insured_item>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Policy_Micro_Proposed_Insured_Item_By_Policy_Micro_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Policy_Micro_ID";
                paramName.Value = policy_id;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        bl_policy_micro_proposed_insured_item proposed_item = new bl_policy_micro_proposed_insured_item();

                        proposed_item.Policy_Micro_ID = rdr.GetString(rdr.GetOrdinal("Policy_Micro_ID"));
                        proposed_item.Policy_Micro_Proposed_Insured_Item_ID = rdr.GetString(rdr.GetOrdinal("Policy_Micro_Proposed_Insured_Item_ID"));
                        proposed_item.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                        proposed_item.Age = rdr.GetInt32(rdr.GetOrdinal("Age"));
                        proposed_item.Percentage = rdr.GetDouble(rdr.GetOrdinal("Percentage"));
                        proposed_item.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                        proposed_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));
                        proposed_item.Address = rdr.GetString(rdr.GetOrdinal("Address"));

                        proposed_items.Add(proposed_item);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetPolicyProposedItem] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return proposed_items;
    }

    //Update Ct_Policy_Micro_Life_Product
    public static bool UpdatePolicyMicroLifeProduct(bl_policy_micro_life_product product)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Micro_Life_Product";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", product.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Age_Insure", product.Age_Insure);           
            cmd.Parameters.AddWithValue("@Assure_Year", product.Assure_Year);
            cmd.Parameters.AddWithValue("@Pay_Mode", product.Pay_Mode);            
            cmd.Parameters.AddWithValue("@Pay_Year", product.Pay_Year);                  
            cmd.Parameters.AddWithValue("@User_Premium", product.User_Premium);
            cmd.Parameters.AddWithValue("@User_Sum_Insure", product.User_Sum_Insure);

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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdatePolicyMicroLifeProduct] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Gender and DOB
    public static bool UpdateGenderDOB(string policy_id, int gender, DateTime dob)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Micro_Gender";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_id);
            cmd.Parameters.AddWithValue("@Gender", gender);          
            cmd.Parameters.AddWithValue("@DOB", dob);          

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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateGenderDOB] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_Policy_Micro_Info_Address
    public static bool UpdatePolicyMicroInfoAddress(bl_policy_micro_info_address address)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Micro_Info_Address";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", address.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Address1", address.Address1);
            cmd.Parameters.AddWithValue("@address2", address.Address2);
            cmd.Parameters.AddWithValue("@Province", address.Province);
            cmd.Parameters.AddWithValue("@Zip_Code", address.Zip_Code);
            cmd.Parameters.AddWithValue("@Country_ID", address.Country_ID);     

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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdatePolicyMicroInfoAddress] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_Policy_Micro_Info_Contact
    public static bool UpdatePolicyMicroInfoContact(bl_policy_micro_info_contact contact)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Micro_Info_Contact";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", contact.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@EMail", contact.EMail);
            cmd.Parameters.AddWithValue("@Mobile_Phone1", contact.Mobile_Phone1);
           
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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdatePolicymicroInfoContact] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_Policy_Micro_Info_Person
    public static bool UpdatePolicyMicroInfoPerson(bl_policy_micro_info_person person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Micro_Info_Person";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", person.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@ID_Type", person.ID_Type);
            cmd.Parameters.AddWithValue("@ID_Card", person.ID_Card);
            cmd.Parameters.AddWithValue("@Last_Name", person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@First_Name", person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Khmer_First_Name", person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", person.Khmer_Last_Name);         
            
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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdatePolicyMicroInfoPerson] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to check barcode
    public static bool CheckBarcode(string card, string barcode)
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
                myCommand.CommandText = "SP_Check_Banc_Barcode";
                myCommand.Parameters.AddWithValue("@Barcode", barcode);
                myCommand.Parameters.AddWithValue("@Product_ID", card);

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
            Log.AddExceptionToLog("Error in function [CheckBarcode] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return result;
    }     
 
    //Insert new policy micro premium
    public static bool InsertPolicyMicroPremium(bl_policy_micro_premium policy_micro_premium)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Premium";


            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_premium.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Premium", policy_micro_premium.Premium);
            cmd.Parameters.AddWithValue("@Original_Amount", policy_micro_premium.Original_Amount);
            cmd.Parameters.AddWithValue("@Sum_Insure", policy_micro_premium.Sum_Insure);
            cmd.Parameters.AddWithValue("@Created_On", policy_micro_premium.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy_micro_premium.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_micro_premium.Created_Note);

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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroPremium] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert new policy micro prem pay
    public static bool InsertPolicyMicroPremPay(bl_policy_micro_prem_pay policy_micro_prem_pay)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Micro_Prem_Pay";


            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_prem_pay.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Policy_Micro_Prem_Pay_ID", policy_micro_prem_pay.Policy_Micro_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@Amount", policy_micro_prem_pay.Amount);
            cmd.Parameters.AddWithValue("@Channel_Location_ID", policy_micro_prem_pay.Channel_Location_ID);
            cmd.Parameters.AddWithValue("@Created_On", policy_micro_prem_pay.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy_micro_prem_pay.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_micro_prem_pay.Created_Note);
            cmd.Parameters.AddWithValue("@Due_Date", policy_micro_prem_pay.Due_Date);
            cmd.Parameters.AddWithValue("@Pay_Date", policy_micro_prem_pay.Pay_Date);
            cmd.Parameters.AddWithValue("@Prem_Lot", policy_micro_prem_pay.Prem_Lot);
            cmd.Parameters.AddWithValue("@Prem_Year", policy_micro_prem_pay.Prem_Year);
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", policy_micro_prem_pay.Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Payment_Code", policy_micro_prem_pay.Payment_Code); 
            
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
                Log.AddExceptionToLog("Error in function [InsertPolicyMicroPremPay] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_Policy_Micro_Prem_Pay
    public static bool UpdatePolicyMicroPremPay(bl_policy_micro_prem_pay policy_micro_prem_pay)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Micro_Prem_Pay";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_prem_pay.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Amount", policy_micro_prem_pay.Amount);
          
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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdatePolicyMicroPremPay] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }


    //Update Ct_Policy_Micro_Premium
    public static bool UpdatePolicyMicroPremium(bl_policy_micro_premium policy_micro_premium)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_Micro_Premium";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_premium.Policy_Micro_ID);
            cmd.Parameters.AddWithValue("@Sum_Insure", policy_micro_premium.Sum_Insure);
            cmd.Parameters.AddWithValue("@Premium", policy_micro_premium.Premium);
            cmd.Parameters.AddWithValue("@Original_Amount", policy_micro_premium.Original_Amount);         

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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdatePolicyMicroPremium] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_Policy_Micro_Premium
    public static bool DeletePolicyMicroPremium(string policy_micro_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Micro_Premium";

            cmd.Parameters.AddWithValue("@Policy_Micro_ID", policy_micro_id);

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
                Log.AddExceptionToLog("Error in function [DeletePolicyMicroPremium] in class [da_policy_micro]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to check policy micro id in policy micro prem pay
    public static bool CheckPolicyMicroIDInPolicyMicroPremPay(string policy_id)
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
                myCommand.CommandText = "SP_Check_Policy_Micro_ID_In_Policy_Micro_Prem_Pay";
                myCommand.Parameters.AddWithValue("@Policy_Micro_ID", policy_id);
              
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
            Log.AddExceptionToLog("Error in function [CheckPolicyMicroIDInPolicyMicroPremPay] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return result;
    }

    //Function to get last card info by card type (order by card number asc)
    public static bl_banc_card GetLastCardByProduct(string card)
    {
        bl_banc_card last_card = new bl_banc_card();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Last_Banc_Card";             
                myCommand.Parameters.AddWithValue("@Product_ID", card);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        last_card.Card_ID = myReader.GetString(myReader.GetOrdinal("Card_ID"));
                        last_card.Card_Number = myReader.GetString(myReader.GetOrdinal("Card_Number"));
                                              
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
            Log.AddExceptionToLog("Error in function [GetLastCardByProduct] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return last_card;
    }

    //Function to get last micro customer number
    public static string GetLastMicroCustomerNumber()
    {
        string last_micro_customer_number = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Last_Micro_Customer_Number";
           
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        last_micro_customer_number = myReader.GetString(myReader.GetOrdinal("Customer_Micro_Number"));
                        
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
            Log.AddExceptionToLog("Error in function [GetLastMicroCustomerNumber] in class [da_policy_micro]. Details: " + ex.Message);
        }

        last_micro_customer_number = last_micro_customer_number.Substring(1);
        int last_number = Convert.ToInt32(last_micro_customer_number) + 1;

        last_micro_customer_number = last_number.ToString();

        while (last_micro_customer_number.Length < 8)
        {
            last_micro_customer_number = "0" + last_micro_customer_number;
        }

        last_micro_customer_number = "T" + last_micro_customer_number;

        return last_micro_customer_number;
    }

    //Function to check existing micro customer in Ct_Micro_Customer
    public static bool CheckExistingMicroCustomer(string first_name, string last_name, string kh_first_name, string kh_last_name, int gender, DateTime dob)
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
                myCommand.CommandText = "SP_Check_Existing_Micro_Customer";
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name);
                myCommand.Parameters.AddWithValue("@Khmer_First_Name", kh_first_name);
                myCommand.Parameters.AddWithValue("@Khmer_Last_Name", kh_last_name);
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
            Log.AddExceptionToLog("Error in function [CheckExistingMicroCustomer] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return result;
    }

    //Function to get existing micro customer id in Ct_Micro_Customer
    public static string GetExistingMicroCustomerID(string first_name, string last_name, string kh_first_name, string kh_last_name, int gender, DateTime dob)
    {
        string customer_micro_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Existing_Micro_Customer_ID";
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name);
                myCommand.Parameters.AddWithValue("@Khmer_First_Name", kh_first_name);
                myCommand.Parameters.AddWithValue("@Khmer_Last_Name", kh_last_name);
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@Birth_Date", dob);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        customer_micro_id = myReader.GetString(myReader.GetOrdinal("Customer_Micro_ID")); ;
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
            Log.AddExceptionToLog("Error in function [GetExistingMicroCustomerID] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return customer_micro_id;
    }

    //Function to get sale agent by code
    public static string GetSaleAgentNameByAgentCode(string sale_agent_id)
    {
        string sale_agent_name = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Sale_Agent_Name_By_Agent_Code";
                myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        sale_agent_name = myReader.GetString(myReader.GetOrdinal("Full_Name")); ;
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
            Log.AddExceptionToLog("Error in function [GetSaleAgentNameByAgentCode] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return sale_agent_name;
    }

    //Function to get policy_id by number
    public static string GetPolicyIDByNumber(string policy_number)
    {
        string policy_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Micro_ID_By_Policy_Number";
                myCommand.Parameters.AddWithValue("@Policy_Number", policy_number);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        policy_id = myReader.GetString(myReader.GetOrdinal("Policy_Micro_ID")); ;
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
            Log.AddExceptionToLog("Error in function [GetPolicyIDByNumber] in class [da_policy_micro]. Details: " + ex.Message);
        }
        return policy_id;
    }


    #endregion

}