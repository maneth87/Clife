using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_application
/// </summary>
public class da_application
{
	private static da_application mytitle = null;
    //Constructor
    public da_application()
    {
        if (mytitle == null)
        {
            mytitle = new da_application();
        }

    }

    #region "Public Functions"

    //Insert Ct_App_Register
    public static bool InsertAppRegister(bl_app_register app_register)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Register";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Number", app_register.App_Number);
            cmd.Parameters.AddWithValue("@Original_App_Number", app_register.Original_App_Number);
            cmd.Parameters.AddWithValue("@Office_ID", app_register.Office_ID);
            cmd.Parameters.AddWithValue("@Created_By", app_register.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", app_register.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", app_register.Created_On);
            cmd.Parameters.AddWithValue("@Payment_Code", app_register.Payment_Code);
            cmd.Parameters.AddWithValue("@Channel_ID", app_register.Channel_ID);
            cmd.Parameters.AddWithValue("@Channel_Item_ID", app_register.Channel_Item_ID); 
            
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
                Log.AddExceptionToLog("Error in function [InsertAppRegister] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Register
    public static bool UpdateAppRegister(bl_app_register app_register)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Register";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Number", app_register.App_Number);      
            cmd.Parameters.AddWithValue("@Created_Note", app_register.Created_Note);
            
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
                Log.AddExceptionToLog("Error in function [UpdateAppRegister] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }
      
    //Insert Ct_App_Info
    public static bool InsertAppInfo(bl_app_info app_info)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Date", app_info.App_Date);
            cmd.Parameters.AddWithValue("@Benefit_Note", app_info.Benefit_Note);
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", app_info.Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Office_ID", app_info.Office_ID);
            cmd.Parameters.AddWithValue("@Created_By", app_info.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", app_info.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", app_info.Created_On);

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
                Log.AddExceptionToLog("Error in function [InsertAppInfo] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Info
    public static bool UpdateAppInfo(bl_app_info app_info)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Date", app_info.App_Date);
            cmd.Parameters.AddWithValue("@Benefit_Note", app_info.Benefit_Note);
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", app_info.Sale_Agent_ID);                    
            cmd.Parameters.AddWithValue("@Created_Note", app_info.Created_Note);            

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfo] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Info
    public static bool DeleteAppInfo(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);          

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfo] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }    

    //Delete Ct_App_Register
    public static bool DeleteAppRegister(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Register";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteAppRegister] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

  
    //Insert Ct_App_Benefit_Item
    public static bool InsertAppBenefitItem(bl_app_benefit_item app_benefit_item)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Benefit_Item";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_benefit_item.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Benefit_Item_ID", app_benefit_item.App_Benefit_Item_ID);
            cmd.Parameters.AddWithValue("@Full_Name", app_benefit_item.Full_Name);
            cmd.Parameters.AddWithValue("@ID_Type", app_benefit_item.ID_Type);
            cmd.Parameters.AddWithValue("@ID_Card", app_benefit_item.ID_Card);
            cmd.Parameters.AddWithValue("@Percentage", app_benefit_item.Percentage);
            cmd.Parameters.AddWithValue("@Relationship", app_benefit_item.Relationship);
            cmd.Parameters.AddWithValue("@Seq_Number", app_benefit_item.Seq_Number);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", app_benefit_item.Relationship_Khmer);
            cmd.Parameters.AddWithValue("@Remarks", app_benefit_item.Remarks);
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
                Log.AddExceptionToLog("Error in function [InsertAppBenefitItem] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Get List of Benefit_Items by app_id
    public static List<bl_app_benefit_item> GetAppBenefitItem(string app_id)
    {

        List<bl_app_benefit_item> benefit_items = new List<bl_app_benefit_item>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Benefit_Item_By_App_Register_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_id;
            cmd.Parameters.Add(paramName);        
        
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_app_benefit_item benefit_item = new bl_app_benefit_item();

                    benefit_item.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    benefit_item.App_Benefit_Item_ID = rdr.GetString(rdr.GetOrdinal("App_Benefit_Item_ID"));
                    benefit_item.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                    benefit_item.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    benefit_item.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                    benefit_item.Percentage = rdr.GetDouble(rdr.GetOrdinal("Percentage"));
                    benefit_item.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                    benefit_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));
                    benefit_item.Remarks = rdr["remarks"].ToString();

                    benefit_items.Add(benefit_item);
                }
            }
            con.Close();
        }
        return benefit_items;
    }

    //Delete Ct_App_Benefit_Item
    public static bool DeleteAppBenefitItem(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Benefit_Item";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppBenefitItem] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Prem_Pay
    public static bool InsertAppPremPay(bl_app_prem_pay app_prem_pay)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Prem_Pay";

            cmd.Parameters.AddWithValue("@App_Prem_Pay_ID", app_prem_pay.App_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_prem_pay.App_Register_ID);
            cmd.Parameters.AddWithValue("@Pay_Date", app_prem_pay.Pay_Date);
            cmd.Parameters.AddWithValue("@Is_Init_Payment", app_prem_pay.Is_Init_Payment);
            cmd.Parameters.AddWithValue("@Amount", app_prem_pay.Amount);
            cmd.Parameters.AddWithValue("@Original_Amount", app_prem_pay.Original_Amount);
            cmd.Parameters.AddWithValue("@Created_By", app_prem_pay.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", app_prem_pay.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", app_prem_pay.Created_On);
            cmd.Parameters.AddWithValue("@Rounded_Amount", app_prem_pay.Rounded_Amount);

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
                Log.AddExceptionToLog("Error in function [InsertAppPremPay] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Prem_Pay
    public static bool UpdateAppPremInitialPay(bl_app_prem_pay app_prem_pay)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Prem_Initial_Pay";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_prem_pay.App_Register_ID);
            cmd.Parameters.AddWithValue("@Pay_Date", app_prem_pay.Pay_Date);         
            cmd.Parameters.AddWithValue("@Created_Note", app_prem_pay.Created_Note);
            cmd.Parameters.AddWithValue("@Amount", app_prem_pay.Amount);
            cmd.Parameters.AddWithValue("@Original_Amount", app_prem_pay.Original_Amount);

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
                Log.AddExceptionToLog("Error in function [UpdateAppPremInitialPay] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Prem_Pay
    public static bool DeleteAppPremPay(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Prem_Pay";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteAppPremPay] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Info_Person
    public static bool InsertAppInfoPerson(bl_app_info_person app_info_person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Person";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_person.App_Register_ID);
            cmd.Parameters.AddWithValue("@ID_Card", app_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", app_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", app_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Last_Name", app_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", app_info_person.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", app_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_person.Country_ID);

            cmd.Parameters.AddWithValue("@Khmer_First_Name", app_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", app_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", app_info_person.Father_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Father_Last_Name", app_info_person.Father_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_First_Name", app_info_person.Mother_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_Last_Name", app_info_person.Mother_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_First_Name", app_info_person.Prior_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_Last_Name", app_info_person.Prior_Last_Name.ToUpper());
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
                Log.AddExceptionToLog("Error in function [InsertAppInfoPerson] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //create new class which inherit from  bl_app_info_person for Personal information sub
    public class bl_app_info_person_sub : bl_app_info_person
    {
        //add one more property
        public int Level { get; set; }
        public string Person_ID { get; set; }
        public string Marital_Status { get; set; }
        public string Relationship { get; set; }
    }

    //Insert Ct_App_Info_Person_Sub
    public static bool InsertAppInfoPersonSub(bl_app_info_person_sub app_info_person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Person_Sub";

            cmd.Parameters.AddWithValue("@Person_ID", app_info_person.Person_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_person.App_Register_ID);
            cmd.Parameters.AddWithValue("@Level", app_info_person.Level);
            cmd.Parameters.AddWithValue("@ID_Card", app_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", app_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", app_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Last_Name", app_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", app_info_person.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", app_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_person.Country_ID);

            cmd.Parameters.AddWithValue("@Khmer_First_Name", app_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", app_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", app_info_person.Father_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Father_Last_Name", app_info_person.Father_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_First_Name", app_info_person.Mother_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_Last_Name", app_info_person.Mother_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_First_Name", app_info_person.Prior_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_Last_Name", app_info_person.Prior_Last_Name.ToUpper());

            cmd.Parameters.AddWithValue("@Marital_Status", app_info_person.Marital_Status);
            cmd.Parameters.AddWithValue("@Relationship", app_info_person.Relationship);

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
                Log.AddExceptionToLog("Error in function [InsertAppInfoPersonSub] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }
   
    //Update Ct_App_Info_Person
    public static bool UpdateAppInfoPerson(bl_app_info_person app_info_person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Person";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_person.App_Register_ID);
            cmd.Parameters.AddWithValue("@ID_Card", app_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", app_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", app_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Last_Name", app_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", app_info_person.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", app_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_person.Country_ID);

            cmd.Parameters.AddWithValue("@Khmer_First_Name", app_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", app_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", app_info_person.Father_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Father_Last_Name", app_info_person.Father_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_First_Name", app_info_person.Mother_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_Last_Name", app_info_person.Mother_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_First_Name", app_info_person.Prior_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_Last_Name", app_info_person.Prior_Last_Name.ToUpper());

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoPerson] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Get Ct_App_Info_Person By Meas Sun on 05-February 2020
    public static bl_app_info_person GetAppInfoPerson(string App_Register_ID)
    {
        bl_app_info_person app_info_person = new bl_app_info_person();
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Info_Person_By_App_Register_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = App_Register_ID;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    app_info_person.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    app_info_person.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                }
            }
            con.Close();
        }
        return app_info_person;
    }
    //Delete Ct_App_Info_Person
    public static bool DeleteAppInfoPerson(string app_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Person";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoPerson] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Info_Person_Sub
    public static bool DeleteAppInfoPerson_Sub(string app_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Person_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoPersonSub] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }      
  
    //Insert Ct_App_Info_Contact
    public static bool InsertAppInfoContact(bl_app_info_contact app_info_contact)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Contact";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_contact.App_Register_ID);
            cmd.Parameters.AddWithValue("@Mobile_Phone1", app_info_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", app_info_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Home_Phone1", app_info_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", app_info_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", app_info_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", app_info_contact.Office_Phone2);
            cmd.Parameters.AddWithValue("@Fax1", app_info_contact.Fax1);

            cmd.Parameters.AddWithValue("@Fax2", app_info_contact.Fax2);
            cmd.Parameters.AddWithValue("@EMail", app_info_contact.EMail);
          
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
                Log.AddExceptionToLog("Error in function [InsertAppInfoContact] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Info_Contact
    public static bool UpdateAppInfoContact(bl_app_info_contact app_info_contact)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Contact";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_contact.App_Register_ID);
            cmd.Parameters.AddWithValue("@Mobile_Phone1", app_info_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", app_info_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Home_Phone1", app_info_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", app_info_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", app_info_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", app_info_contact.Office_Phone2);
            cmd.Parameters.AddWithValue("@Fax1", app_info_contact.Fax1);

            cmd.Parameters.AddWithValue("@Fax2", app_info_contact.Fax2);
            cmd.Parameters.AddWithValue("@EMail", app_info_contact.EMail);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoContact] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Info_Contact
    public static bool DeleteAppInfoContact(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Contact";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoContact] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Info_Address
    public static bool InsertAppInfoAddress(bl_app_info_address app_info_address)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Address";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_address.App_Register_ID);
            cmd.Parameters.AddWithValue("@Address1", app_info_address.Address1);
            cmd.Parameters.AddWithValue("@Address2", app_info_address.Address2);
            cmd.Parameters.AddWithValue("@Address3", app_info_address.Address3);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_address.Country_ID);
            cmd.Parameters.AddWithValue("@Province", app_info_address.Province);
            cmd.Parameters.AddWithValue("@Zip_Code", app_info_address.Zip_Code);
           
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
                Log.AddExceptionToLog("Error in function [InsertAppInfoAddress] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Info_Address
    public static bool UpdateAppInfoAddress(bl_app_info_address app_info_address)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Address";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_address.App_Register_ID);
            cmd.Parameters.AddWithValue("@Address1", app_info_address.Address1);
            cmd.Parameters.AddWithValue("@Address2", app_info_address.Address2);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_address.Country_ID);
            cmd.Parameters.AddWithValue("@Province", app_info_address.Province);
            cmd.Parameters.AddWithValue("@Zip_Code", app_info_address.Zip_Code);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoAddress] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Info_Address
    public static bool DeleteAppInfoAddress(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Address";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoAddress] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Job_History
    public static bool InsertAppJobHistory(bl_app_job_history app_job_history)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Job_History";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_job_history.App_Register_ID);
            cmd.Parameters.AddWithValue("@Anual_Income", app_job_history.Anual_Income);
            cmd.Parameters.AddWithValue("@Current_Position", app_job_history.Current_Position);
            cmd.Parameters.AddWithValue("@Employer_Name", app_job_history.Employer_Name);
            cmd.Parameters.AddWithValue("@Job_Role", app_job_history.Job_Role);
            cmd.Parameters.AddWithValue("@Nature_Of_Business", app_job_history.Nature_Of_Business);
           
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
                Log.AddExceptionToLog("Error in function [InsertAppJobHistory] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Job_History
    public static bool UpdateAppJobHistory(bl_app_job_history app_job_history)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Job_History";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_job_history.App_Register_ID);
            cmd.Parameters.AddWithValue("@Anual_Income", app_job_history.Anual_Income);
            cmd.Parameters.AddWithValue("@Current_Position", app_job_history.Current_Position);
            cmd.Parameters.AddWithValue("@Employer_Name", app_job_history.Employer_Name);
            cmd.Parameters.AddWithValue("@Job_Role", app_job_history.Job_Role);
            cmd.Parameters.AddWithValue("@Nature_Of_Business", app_job_history.Nature_Of_Business);

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
                Log.AddExceptionToLog("Error in function [UpdateAppJobHistory] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Job_History
    public static bool DeleteAppJobHistory(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Job_History";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppJobHistory] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Loan
    public static bool InsertAppLoan(bl_app_loan app_loan)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Loan";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_loan.App_Register_ID);
            cmd.Parameters.AddWithValue("@Interest_Rate", app_loan.Interest_Rate);
            cmd.Parameters.AddWithValue("@Loan_Effiective_Date", app_loan.Loan_Effiective_Date);
            cmd.Parameters.AddWithValue("@Loan_Type", app_loan.Loan_Type);
            cmd.Parameters.AddWithValue("@Out_Std_Loan", app_loan.Out_Std_Loan);
            cmd.Parameters.AddWithValue("@Term_Year", app_loan.Term_Year);

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
                Log.AddExceptionToLog("Error in function [InsertAppLoan] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Loan
    public static bool UpdateAppLoan(bl_app_loan app_loan)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Loan";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_loan.App_Register_ID);
            cmd.Parameters.AddWithValue("@Interest_Rate", app_loan.Interest_Rate);
            cmd.Parameters.AddWithValue("@Loan_Effiective_Date", app_loan.Loan_Effiective_Date);
            cmd.Parameters.AddWithValue("@Loan_Type", app_loan.Loan_Type);
            cmd.Parameters.AddWithValue("@Out_Std_Loan", app_loan.Out_Std_Loan);
            cmd.Parameters.AddWithValue("@Term_Year", app_loan.Term_Year);
         
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
                Log.AddExceptionToLog("Error in function [UpdateAppLoan] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Loan
    public static bool DeleteAppLoan(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Loan";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppLoan] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Info_Body
    public static bool InsertAppInfoBody(bl_app_info_body app_info_body)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Body";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_body.App_Register_ID);
            cmd.Parameters.AddWithValue("@Height", app_info_body.Height);
            cmd.Parameters.AddWithValue("@Weight", app_info_body.Weight);
            cmd.Parameters.AddWithValue("@Is_Weight_Changed", app_info_body.Is_Weight_Changed);
            cmd.Parameters.AddWithValue("@Weight_Change", app_info_body.Weight_Change);
            cmd.Parameters.AddWithValue("@Reason", app_info_body.Reason);
           
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
                Log.AddExceptionToLog("Error in function [InsertAppInfoBody] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Life_Body
    public static bool UpdateAppInfoBody(bl_app_info_body app_info_body)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Body";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_body.App_Register_ID);
            cmd.Parameters.AddWithValue("@Height", app_info_body.Height);
            cmd.Parameters.AddWithValue("@Is_Weight_Changed", app_info_body.Is_Weight_Changed);
            cmd.Parameters.AddWithValue("@Weight", app_info_body.Weight);
            cmd.Parameters.AddWithValue("@Weight_Change", app_info_body.Weight_Change);
            cmd.Parameters.AddWithValue("@Reason", app_info_body.Reason);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoBody] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Info_Body
    public static bool DeleteAppInfoBody(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Body";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoBody] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Life_Product
    public static bool InsertAppLifeProduct(bl_app_life_product app_life_product)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Life_Product";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_life_product.App_Register_ID);
            cmd.Parameters.AddWithValue("@Age_Insure", app_life_product.Age_Insure);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", app_life_product.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", app_life_product.Assure_Year);
            cmd.Parameters.AddWithValue("@Pay_Mode", app_life_product.Pay_Mode);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", app_life_product.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Pay_Year", app_life_product.Pay_Year);
            cmd.Parameters.AddWithValue("@Product_ID", app_life_product.Product_ID);
            cmd.Parameters.AddWithValue("@System_Premium", app_life_product.System_Premium);
            cmd.Parameters.AddWithValue("@System_Premium_Discount", app_life_product.System_Premium_Discount);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", app_life_product.System_Sum_Insure);
            cmd.Parameters.AddWithValue("@User_Premium", app_life_product.User_Premium);
            cmd.Parameters.AddWithValue("@User_Sum_Insure", app_life_product.User_Sum_Insure);
            //cmd.Parameters.AddWithValue("@Is_Pre_Premium_Discount", app_life_product.Is_Pre_Premium_Discount);
            
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
                Log.AddExceptionToLog("Error in function [InsertAppLifeProduct] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Life_Product
    public static bool UpdateAppLifeProduct(bl_app_life_product app_life_product)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Life_Product";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_life_product.App_Register_ID);
            cmd.Parameters.AddWithValue("@Product_ID", app_life_product.Product_ID);
            cmd.Parameters.AddWithValue("@Age_Insure", app_life_product.Age_Insure);
            cmd.Parameters.AddWithValue("@Pay_Year", app_life_product.Pay_Year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", app_life_product.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", app_life_product.Assure_Year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", app_life_product.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@User_Sum_Insure", app_life_product.User_Sum_Insure);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", app_life_product.System_Sum_Insure);
            cmd.Parameters.AddWithValue("@User_Premium", app_life_product.User_Premium);
            cmd.Parameters.AddWithValue("@System_Premium", app_life_product.System_Premium);
            cmd.Parameters.AddWithValue("@System_Premium_Discount", app_life_product.System_Premium_Discount);
            cmd.Parameters.AddWithValue("@Pay_Mode", app_life_product.Pay_Mode);
            //cmd.Parameters.AddWithValue("@Is_Pre_Premium_Discount", app_life_product.Is_Pre_Premium_Discount);
        
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
                 Log.AddExceptionToLog("Error in function [UpdateAppLifeProduct] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Life_Product
    public static bool DeleteAppLifeProduct(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Life_Product";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppLifeProduct] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Answer_Item
    public static bool InsertAppAnswerItem(bl_app_answer_item app_answer_item)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Answer_Item";

            cmd.Parameters.AddWithValue("@App_Answer_Item_ID", app_answer_item.App_Answer_Item_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_answer_item.App_Register_ID);
            cmd.Parameters.AddWithValue("@Question_ID", app_answer_item.Question_ID);

            cmd.Parameters.AddWithValue("@Seq_Number", app_answer_item.Seq_Number);
            cmd.Parameters.AddWithValue("@Answer", app_answer_item.Answer);          

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
                Log.AddExceptionToLog("Error in function [InsertAppAnswerItem] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Get List of Answer_Items by app_id
    public static List<bl_app_answer_item> GetAppAnswerItem(string app_id)
    {

        List<bl_app_answer_item> answer_items = new List<bl_app_answer_item>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Answer_Item_By_App_Register_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_app_answer_item answer_item = new bl_app_answer_item();

                    answer_item.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    answer_item.App_Answer_Item_ID = rdr.GetString(rdr.GetOrdinal("App_Answer_Item_ID"));
                    answer_item.Question_ID = rdr.GetString(rdr.GetOrdinal("Question_ID"));
                    answer_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));
                    answer_item.Answer = rdr.GetInt32(rdr.GetOrdinal("Answer"));
                    answer_items.Add(answer_item);
                }
            }
            con.Close();
        }
        return answer_items;
    }

    //Update Ct_App_Answer_Item
    public static bool UpdateAppAnswerItem(bl_app_answer_item app_answer_item)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Answer_Item";
           
            cmd.Parameters.AddWithValue("@App_Register_ID", app_answer_item.App_Register_ID);
            cmd.Parameters.AddWithValue("@Question_ID", app_answer_item.Question_ID);
            cmd.Parameters.AddWithValue("@Seq_Number", app_answer_item.Seq_Number);
            cmd.Parameters.AddWithValue("@Answer", app_answer_item.Answer);
         
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
                 Log.AddExceptionToLog("Error in function [UpdateAppAnswerItem] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Answer_Item
    public static bool DeleteAppAnswerItem(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Answer_Item";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppAnswerItem] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert App_ID_Temp (App_Register_ID store here to be used for registration continue in application_form code behind)
    public static bool InsertAppIDToTempTable(string app_id, string user_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_ID_Temp";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);
            cmd.Parameters.AddWithValue("@User_ID", user_id);   
          
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
                Log.AddExceptionToLog("Error in function [InsertAppIDToTempTable] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_ID_Temp
    public static bool DeleteAppIDTemp(string user_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_ID_Temp";

            cmd.Parameters.AddWithValue("@User_ID", user_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppIDTemp] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get app_id by user_id
    public static string GetAppRegisterIDByUserID(string user_id)
    {
        string app_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_ID_Temp", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@User_ID";
            paramName.Value = user_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    app_id = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));

                }

            }
            con.Close();
        }
        return app_id;
    }

    //Function to check app_id
    public static bool CheckAppRegisterID(string app_id)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Check_App_Register_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                   result = true;

                }

            }
            con.Close();
        }
        return result;
    }

    //Function to check app_number
    public static bool CheckAppNumber(string app_number)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Check_App_Number", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Number";
            paramName.Value = app_number;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    result = true;

                }

            }
            con.Close();
        }
        return result;
    }

    //Function to check app_number
    public static bool CheckAppRegisterIDInAppLoan(string app_register_id)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Check_App_Register_ID_In_App_Loan", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_register_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    result = true;

                }

            }
            con.Close();
        }
        return result;
    }

    //Function to search application by app_number
    public static List<bl_app_search> GetApplicationByAppNo(string app_number)
    {
        
        List<bl_app_search> applications = new List<bl_app_search>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_By_App_Number", con);
            cmd.CommandType = CommandType.StoredProcedure;
                           
            cmd.Parameters.AddWithValue("@App_Number", app_number);                 
        
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    bl_app_search application = new bl_app_search();

                    application.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    application.App_Number = rdr.GetString(rdr.GetOrdinal("Original_App_Number"));
                    application.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));

                    int id_type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                                       
                    switch (id_type)
                    {
                        case 0:
                            application.ID_Type = "I.D Card";
                            break;
                        case 1:
                            application.ID_Type = "Passport";
                            break;
                        case 2:
                            application.ID_Type = "Visa";
                            break;
                        case 3:
                            application.ID_Type = "Birth Certificate";
                            break;
                    }
                    
                    application.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));

                    int gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));

                    if (gender == 1)
                    {
                        application.Gender = "Male";
                    }
                    else
                    {
                        application.Gender = "Female";
                    }
                   
                    application.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    application.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    application.Nationality = rdr.GetString(rdr.GetOrdinal("Country_ID"));
                   
                    applications.Add(application);
                }
            }
            con.Close();
        }
        return applications;
    }

    //Function to search application by customer_name
    public static List<bl_app_search> GetApplicationByCustomerName(string last_name, string first_name)
    {

        List<bl_app_search> applications = new List<bl_app_search>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_By_Customer_Name", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Last_Name", last_name);
            cmd.Parameters.AddWithValue("@First_Name", first_name);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    bl_app_search application = new bl_app_search();

                    application.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    application.App_Number = rdr.GetString(rdr.GetOrdinal("Original_App_Number"));
                    application.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));

                    int id_type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));

                    switch (id_type)
                    {
                        case 0:
                            application.ID_Type = "I.D Card";
                            break;
                        case 1:
                            application.ID_Type = "Passport";
                            break;
                        case 2:
                            application.ID_Type = "Visa";
                            break;
                        case 3:
                            application.ID_Type = "Birth Certificate";
                            break;
                    }

                    application.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));

                    int gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));

                    if (gender == 1)
                    {
                        application.Gender = "Male";
                    }
                    else
                    {
                        application.Gender = "Female";
                    }

                    application.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    application.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    application.Nationality = rdr.GetString(rdr.GetOrdinal("Country_ID"));

                    applications.Add(application);
                }
            }
            con.Close();
        }
        return applications;
    }

    //Function to search application by app_number
    public static List<bl_app_search> GetApplicationByIDCard(int id_type, string id_card)
    {

        List<bl_app_search> applications = new List<bl_app_search>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_By_ID_Card", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID_Type", id_type);
            cmd.Parameters.AddWithValue("@ID_Card", id_card);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    bl_app_search application = new bl_app_search();

                    application.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    application.App_Number = rdr.GetString(rdr.GetOrdinal("Original_App_Number"));
                    application.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));

                    int id_type_number = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));

                    switch (id_type_number)
                    {
                        case 0:
                            application.ID_Type = "I.D Card";
                            break;
                        case 1:
                            application.ID_Type = "Passport";
                            break;
                        case 2:
                            application.ID_Type = "Visa";
                            break;
                        case 3:
                            application.ID_Type = "Birth Certificate";
                            break;
                    }

                    application.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));

                    int gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));

                    if (gender == 1)
                    {
                        application.Gender = "Male";
                    }
                    else
                    {
                        application.Gender = "Female";
                    }

                    application.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    application.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    application.Nationality = rdr.GetString(rdr.GetOrdinal("Country_ID"));

                    applications.Add(application);
                }
            }
            con.Close();
        }
        return applications;
    }

    //Function to get app single row data by id
    public static bl_app_single_row_data GetAppSingleRowData(string app_id)
    {

        bl_app_single_row_data app_single_row_data = new bl_app_single_row_data();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Single_Row_Data_By_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);
           
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {

                    app_single_row_data.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    app_single_row_data.App_Number = rdr.GetString(rdr.GetOrdinal("Original_App_Number"));
                    app_single_row_data.Payment_Code = rdr.GetString(rdr.GetOrdinal("Payment_Code"));
                    app_single_row_data.Channel_ID = rdr.GetString(rdr.GetOrdinal("Channel_ID"));
                    app_single_row_data.Channel_Item_ID = rdr.GetString(rdr.GetOrdinal("Channel_Item_ID"));
                    app_single_row_data.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                    app_single_row_data.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));
                    app_single_row_data.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                    app_single_row_data.Office_ID = rdr.GetString(rdr.GetOrdinal("Office_ID"));
                    app_single_row_data.App_Date = rdr.GetDateTime(rdr.GetOrdinal("App_Date"));
                   
                    app_single_row_data.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));

                    //Get sale agent name                   
                    app_single_row_data.Sale_Agent_Full_Name = da_sale_agent.GetSaleAgentNameByID(app_single_row_data.Sale_Agent_ID);
                    
                    
                    app_single_row_data.Benefit_Note = rdr.GetString(rdr.GetOrdinal("Benefit_Note"));
                    app_single_row_data.Country = rdr.GetString(rdr.GetOrdinal("Country"));
                    app_single_row_data.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                    app_single_row_data.Address2 = rdr.GetString(rdr.GetOrdinal("Address2"));
                    app_single_row_data.Province = rdr.GetString(rdr.GetOrdinal("Province"));
                    app_single_row_data.Zip_Code = rdr.GetString(rdr.GetOrdinal("Zip_Code"));
                    app_single_row_data.Weight = rdr.GetInt32(rdr.GetOrdinal("Weight"));
                    app_single_row_data.Height = rdr.GetInt32(rdr.GetOrdinal("Height"));
                    app_single_row_data.Reason = rdr.GetString(rdr.GetOrdinal("Reason"));
                    app_single_row_data.Is_Weight_Changed = rdr.GetInt32(rdr.GetOrdinal("Is_Weight_Changed"));
                    app_single_row_data.Weight_Change = rdr.GetInt32(rdr.GetOrdinal("Weight_Change"));
                    app_single_row_data.Mobile_Phone1 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone1"));
                    app_single_row_data.Home_Phone1 = rdr.GetString(rdr.GetOrdinal("Home_Phone1"));
                    app_single_row_data.Fax1 = rdr.GetString(rdr.GetOrdinal("Fax1"));
                    app_single_row_data.EMail = rdr.GetString(rdr.GetOrdinal("EMail"));
                    app_single_row_data.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    app_single_row_data.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                    app_single_row_data.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                    app_single_row_data.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    app_single_row_data.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));
                    app_single_row_data.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                    app_single_row_data.Nationality = rdr.GetString(rdr.GetOrdinal("Nationality"));
                    app_single_row_data.Khmer_First_Name = rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));
                    app_single_row_data.Khmer_Last_Name = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name"));
                    app_single_row_data.Father_First_Name = rdr.GetString(rdr.GetOrdinal("Father_First_Name"));
                    app_single_row_data.Father_Last_Name = rdr.GetString(rdr.GetOrdinal("Father_Last_Name"));
                    app_single_row_data.Mother_First_Name = rdr.GetString(rdr.GetOrdinal("Mother_First_Name"));
                    app_single_row_data.Mother_Last_Name = rdr.GetString(rdr.GetOrdinal("Mother_Last_Name"));
                    app_single_row_data.Prior_First_Name = rdr.GetString(rdr.GetOrdinal("Prior_First_Name"));
                    app_single_row_data.Prior_Last_Name = rdr.GetString(rdr.GetOrdinal("Prior_Last_Name"));
                    app_single_row_data.Employer_Name = rdr.GetString(rdr.GetOrdinal("Employer_Name"));
                    app_single_row_data.Nature_Of_Business = rdr.GetString(rdr.GetOrdinal("Nature_Of_Business"));
                    app_single_row_data.Current_Position = rdr.GetString(rdr.GetOrdinal("Current_Position"));
                    app_single_row_data.Job_Role = rdr.GetString(rdr.GetOrdinal("Job_Role"));
                    app_single_row_data.Anual_Income = rdr.GetDouble(rdr.GetOrdinal("Anual_Income"));

                    app_single_row_data.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));

                    //Get Product_Type
                    bl_product_type product_type = new bl_product_type();
                    product_type = da_product.GetProductTypeByProductID(app_single_row_data.Product_ID);
                    app_single_row_data.Product_Type_ID = product_type.Product_Type_ID;

                    app_single_row_data.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));
                    app_single_row_data.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));
                    app_single_row_data.Pay_Up_To_Age = rdr.GetInt32(rdr.GetOrdinal("Pay_Up_To_Age"));
                    app_single_row_data.Assure_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                    app_single_row_data.Assure_Up_To_Age = rdr.GetInt32(rdr.GetOrdinal("Assure_Up_To_Age"));
                    app_single_row_data.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("User_Sum_Insure"));
                    app_single_row_data.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                    app_single_row_data.User_Premium = rdr.GetDouble(rdr.GetOrdinal("User_Premium"));
                    app_single_row_data.System_Premium = rdr.GetDouble(rdr.GetOrdinal("System_Premium"));
                    app_single_row_data.System_Premium_Discount = rdr.GetDouble(rdr.GetOrdinal("System_Premium_Discount"));
                    app_single_row_data.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));
                    app_single_row_data.Original_Amount = rdr.GetDouble(rdr.GetOrdinal("Original_Amount"));

                    app_single_row_data.Discount_Amount = rdr.GetDouble(rdr.GetOrdinal("Discount_Amount"));
                    app_single_row_data.Insurance_Plan_Note = rdr.GetString(rdr.GetOrdinal("Insurance_Plan_Note"));

                    //app_single_row_data.Is_Pre_Premium_Discount = rdr.GetInt32(rdr.GetOrdinal("Is_Pre_Premium_Discount"));

                    //underwriting status and policy number
                    bl_app_underwrite_info app_underwriting_info = new bl_app_underwrite_info();
                    app_underwriting_info = GetAppUnderwritingInfo(app_single_row_data.App_Register_ID);

                    app_single_row_data.Policy_Number = app_underwriting_info.Policy_Number;
                    app_single_row_data.Underwriting_Status = app_underwriting_info.Underwrting_Status;

                    //app_loan                   
                    if (da_application.CheckAppRegisterIDInAppLoan(app_single_row_data.App_Register_ID))
                    {
                        //get app_loan info
                        bl_app_loan app_loan = new bl_app_loan();
                        app_loan = da_application.GetAppLoan(app_single_row_data.App_Register_ID);

                        app_single_row_data.Interest_Rate = app_loan.Interest_Rate;
                        app_single_row_data.Loan_Effiective_Date = app_loan.Loan_Effiective_Date;
                        app_single_row_data.Loan_Type = app_loan.Loan_Type;
                        app_single_row_data.Out_Std_Loan = app_loan.Out_Std_Loan;
                        app_single_row_data.Term_Year = app_loan.Term_Year;
                    }
                    else
                    {
                        app_single_row_data.Interest_Rate = 0;
                        app_single_row_data.Loan_Effiective_Date = System.DateTime.Now;
                        app_single_row_data.Loan_Type = 0;
                        app_single_row_data.Out_Std_Loan = 0;
                        app_single_row_data.Term_Year = 0;
                    }
                }
            }
            con.Close();
        }
        return app_single_row_data;
    }

    //Insert Ct_App_Register_Cancel
    public static bool InsertAppRegisterCancel(bl_app_register_cancel app_register_cancel)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Register_Cancel";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_cancel.App_Register_ID);           
            cmd.Parameters.AddWithValue("@Created_By", app_register_cancel.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", app_register_cancel.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", app_register_cancel.Created_On);

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
                Log.AddExceptionToLog("Error in function [InsertAppRegisterCancel] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
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

    //Update App_Number to App_Register_ID
    public static bool UpdateAppNumberToAppRegisterID(string app_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Number_To_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);
          
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
                Log.AddExceptionToLog("Error in function [UpdateAppNumberToAppRegisterID] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get app underwriting info
    public static bl_app_underwrite_info GetAppUnderwritingInfo(string app_id)
    {
        bl_app_underwrite_info underwrite = new bl_app_underwrite_info();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Underwrite_Status_And_Policy_Number", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                   
                    underwrite.Underwrting_Status = rdr.GetString(rdr.GetOrdinal("Status_Code"));
                    int Result =  rdr.GetInt32(rdr.GetOrdinal("Result"));
                  
                    underwrite.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    
                
                }

            }
            con.Close();
        }
        return underwrite;
    }

    //Function to get app_loan
    public static bl_app_loan GetAppLoan(string app_register_id)
    {
        bl_app_loan app_loan = new bl_app_loan();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Loan", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_register_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    app_loan.Interest_Rate = rdr.GetDouble(rdr.GetOrdinal("Interest_Rate"));
                    app_loan.Loan_Effiective_Date = rdr.GetDateTime(rdr.GetOrdinal("Loan_Effiective_Date"));
                    app_loan.Loan_Type = rdr.GetInt32(rdr.GetOrdinal("Loan_Type"));
                    app_loan.Out_Std_Loan = rdr.GetDouble(rdr.GetOrdinal("Out_Std_Loan"));
                    app_loan.Term_Year = rdr.GetInt32(rdr.GetOrdinal("Term_Year"));
                    app_loan.App_Register_ID = rdr["app_register_id"].ToString();
                }

            }
            con.Close();
        }
        return app_loan;
    }

    //Update Gender by App_Register_ID
    public static bool UpdateGenderDOB(string app_id, int gender, DateTime dob)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Gender_DOB";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);
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
                Log.AddExceptionToLog("Error in function [UpdateGenderDOB] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Premium_Discount
    public static bool InsertAppPremiumDiscount(bl_app_premium_discount app_premium_discount)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Premium_Discount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_premium_discount.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Premium_Discount_ID", app_premium_discount.App_Premium_Discount_ID);
            cmd.Parameters.AddWithValue("@Discount_Rate", app_premium_discount.Discount_Rate);
            cmd.Parameters.AddWithValue("@Premium_After_Discount", app_premium_discount.Premium_After_Discount);
            cmd.Parameters.AddWithValue("@Premium_Discount", app_premium_discount.Premium_Discount);
            cmd.Parameters.AddWithValue("@Year", app_premium_discount.Year);

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
                Log.AddExceptionToLog("Error in function [InsertAppPremiumDiscount] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Premium_Discount
    public static bool DeleteAppPremiumDiscount(string app_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Premium_Discount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppPremiumDiscount] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Get List of premium discount by app_id
    public static List<bl_app_premium_discount> GetAppPremiumDiscount(string app_id)
    {

        List<bl_app_premium_discount> premium_discounts = new List<bl_app_premium_discount>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Premium_Discount_By_App_Register_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_app_premium_discount premium_discount = new bl_app_premium_discount();

                    premium_discount.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    premium_discount.App_Premium_Discount_ID = rdr.GetString(rdr.GetOrdinal("App_Premium_Discount_ID"));
                    premium_discount.Discount_Rate = rdr.GetDouble(rdr.GetOrdinal("Discount_Rate"));
                    premium_discount.Premium_After_Discount = rdr.GetDouble(rdr.GetOrdinal("Premium_After_Discount"));
                    premium_discount.Premium_Discount = rdr.GetDouble(rdr.GetOrdinal("Premium_Discount"));
                    premium_discount.Year = rdr.GetInt32(rdr.GetOrdinal("Year"));
                   
                    premium_discounts.Add(premium_discount);
                }
            }
            con.Close();
        }
        return premium_discounts;
    }

    //Update Original Amount
    public static bool UpdateOriginalAmount(string app_id, double original_amount, double rounded_amount, double amount)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Prem_Pay_Original_Amount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);
            cmd.Parameters.AddWithValue("@Original_Amount", original_amount);
            cmd.Parameters.AddWithValue("@Rounded_Amount", rounded_amount);
            cmd.Parameters.AddWithValue("@Amount", amount);
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
                Log.AddExceptionToLog("Error in function [UpdateOriginalAmount] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Discount
    public static bool InsertAppDiscount(bl_app_discount app_discount)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Discount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_discount.App_Register_ID);
            cmd.Parameters.AddWithValue("@Discount_Amount", app_discount.Discount_Amount);
            cmd.Parameters.AddWithValue("@Annual_Premium", app_discount.Annual_Premium);
            cmd.Parameters.AddWithValue("@Total_Amount", app_discount.Total_Amount);
            cmd.Parameters.AddWithValue("@Premium", app_discount.Premium);
            cmd.Parameters.AddWithValue("@Pay_Mode", app_discount.Pay_Mode);
            cmd.Parameters.AddWithValue("@Created_By", app_discount.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", app_discount.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", app_discount.Created_On);

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
                Log.AddExceptionToLog("Error in function [InsertAppDiscount] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Discount
    public static bool UpdateAppDiscount(bl_app_discount app_discount)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Discount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_discount.App_Register_ID);
            cmd.Parameters.AddWithValue("@Annual_Premium", app_discount.Annual_Premium);
            cmd.Parameters.AddWithValue("@Discount_Amount", app_discount.Discount_Amount);
            cmd.Parameters.AddWithValue("@Pay_Mode", app_discount.Pay_Mode);
            cmd.Parameters.AddWithValue("@Premium", app_discount.Premium);
            cmd.Parameters.AddWithValue("@Total_Amount", app_discount.Total_Amount);
            cmd.Parameters.AddWithValue("@Created_Note", app_discount.Created_Note);

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
                Log.AddExceptionToLog("Error in function [UpdateAppDiscount] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Discount
    public static bool DeleteAppDiscount(string app_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Discount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppDiscount] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get app_register ID by policy_id
    public static string GetAppRegisterID(string policy_id)
    {
        string app_register_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Register_By_Policy_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@PolicyID";
            paramName.Value = policy_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    app_register_id = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));

                }

            }
            con.Close();
        }
        return app_register_id;
    }

    //Function to get app life product by app_register_ID
    public static bl_underwriting GetAppLifeProductInfo(string app_register_id)
    {
        bl_underwriting app_life_product = new bl_underwriting();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM CT_APP_LIFE_PRODUCT WHERE App_Register_ID = '" + app_register_id + "' ";
            cmd.Connection = con;
            con.Open();
            cmd.Dispose();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    app_life_product.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    app_life_product.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    app_life_product.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));
                    app_life_product.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));
                    app_life_product.Pay_Up_To_Age = rdr.GetInt32(rdr.GetOrdinal("Pay_Up_To_Age"));
                    app_life_product.Assure_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                    app_life_product.Assure_Up_To_Age = rdr.GetInt32(rdr.GetOrdinal("Assure_Up_To_Age"));
                    app_life_product.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("User_Sum_Insure"));
                    app_life_product.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                    app_life_product.User_Premium = rdr.GetDouble(rdr.GetOrdinal("User_Premium"));
                    app_life_product.System_Premium = rdr.GetDouble(rdr.GetOrdinal("System_Premium"));
                    app_life_product.System_Premium_Discount = rdr.GetDouble(rdr.GetOrdinal("System_Premium_Discount"));
                    app_life_product.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));

                }

            }
            con.Close();
        }
        return app_life_product;
    }

    //Get All Reserve Data List 
    public static DataTable GetReservedDataList(List<string[,,]> condition)
    {
        DataTable tbl_result = new DataTable();
        string my_condition = "";

        try
        {
            foreach (string[, ,] cond in condition)
            {
                for (int index = 0; index <= cond.GetUpperBound(0); index++)
                {
                    /*check operator "IN" */
                    if (cond[index, 0, 1].Trim().ToUpper() == "IN")
                    {
                        /*Make up sql format*/
                        cond[index, 0, 2] = " ('" + cond[index, 0, 2].Trim().Replace(",", "','") + "') ";
                    }
                    else if (cond[index, 0, 1].Trim().ToUpper() == "LIKE")
                    {
                        /*Make up sql format*/
                        cond[index, 0, 2] = " '%" + cond[index, 0, 2].Trim() + "%' ";
                    }
                    else if (cond[index, 0, 1].Trim().ToUpper() == "BETWEEN")
                    {
                        /*Make up sql format*/
                        cond[index, 0, 2] = " '" + cond[index, 0, 2].Trim().Replace(",", "' AND '") + "' ";
                        cond[index, 0, 0] = " CAST(" + cond[index, 0, 0] + " AS DATE) ";
                    }
                    else
                    {
                        /*Make up sql format*/
                        cond[index, 0, 2] = " '" + cond[index, 0, 2] + "' ";
                    }
                    cond[index, 0, 1] = " " + cond[index, 0, 1] + " ";
                    my_condition += my_condition.Trim() == "" ? " WHERE " + cond[index, 0, 0] + cond[index, 0, 1] + cond[index, 0, 2] : " AND " + cond[index, 0, 0] + cond[index, 0, 1] + cond[index, 0, 2];
                }
            }

            tbl_result = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_GET_RESERVED_POLICY_BY_CONDITION", new string[,] { { "@CONDITION", my_condition } }, "da_application => GetReservedDataList(string[,] condition)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetReservedDataList(string[,] condition)] in call [da_application], detail:" + ex.Message);
            tbl_result = new DataTable();

        }
        return tbl_result;
    }

    //Insert Reserve Policy for Credit Life 24 On 17-06-2020
    public static bool InsertReservePolicy(bl_app_reserve_policy reserve_policy) 
    {
        bool status = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Reserve_Policy";

            cmd.Parameters.AddWithValue("@App_Number", reserve_policy.App_Number);
            cmd.Parameters.AddWithValue("@Customer_ID", reserve_policy.Customer_ID);
            cmd.Parameters.AddWithValue("@Policy_Number", reserve_policy.Policy_Number);
            cmd.Parameters.AddWithValue("@Created_On", reserve_policy.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", reserve_policy.Created_By);

            cmd.Connection = con;
            con.Open();

            try
            {
                cmd.ExecuteNonQuery(); //sql query execution
                status = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [InsertReservePolicy] in class [da_application]. Details: " + ex.Message);  
            }

        }

        return status;
    }

    //Get Reserve Policy 
    public static bl_app_reserve_policy GetReservePolicyByAppNumber(string App_Number)
    {
        bl_app_reserve_policy Reserve_list = new bl_app_reserve_policy();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Reserve_Policy_By_App_Number";
            cmd.Parameters.AddWithValue("@App_Number", App_Number);
            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        Reserve_list.Reserve_Policy_ID = rdr.GetString(rdr.GetOrdinal("Reserve_Policy_ID"));
                        Reserve_list.App_Number = rdr.GetString(rdr.GetOrdinal("App_Number"));
                        Reserve_list.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                        Reserve_list.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [GetReservePolicyByAppNumber] in class [da_application]. Details: " + ex.Message); 
            }

        }

        return Reserve_list;
    }

    //Delete ReservePolicyNumber
    public static bool DeleteReservePolicy(string id)
    {
        bool status = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Reserve_Policy";

            cmd.Parameters.AddWithValue("@App_Reserve_Policy_ID", id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                status = true;
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeleteReservePolicy] in class [da_application]. Details: " + ex.Message);
            }
        }
        return status;
    }

    //Get complete format of application number with A + 8 digits number
    public static string GetCompleteAppNumber(string app_number)
    {
        string complete_number = "A";
        int count = app_number.Length; //app_number.Length;

        if (count <= 8)
        {
            for (int i = count; i < 8; i++)
            {
                complete_number += "0";
            }
        }
        else
        {
            return "";
        }


        complete_number += app_number;

        return complete_number;
    }


    #endregion
}