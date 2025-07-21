using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for da_customer
/// </summary>
public class da_customer
{	

    private static da_customer mytitle = null;
    
    public da_customer()
	{
        if (mytitle == null)
        {
            mytitle = new da_customer();
        }
    }

    #region "Public Functions"


    //Insert new customer into database then upon successful insert, return customer ID
    public static string InsertCustomer(string app_register_id, string created_by, DateTime created_on)
    {
        
        string customer_id = "";

        //Get a record from table app_info_person by app_register_id
        bl_app_info_person my_app_info_person = GetAppInfoPerson(app_register_id);

        //Check existing customer
        string check_existing_customer = CheckCustomerByParameters(my_app_info_person.First_Name, my_app_info_person.Last_Name, my_app_info_person.Gender, my_app_info_person.Birth_Date); //search to get customer id

        if (check_existing_customer == "")
        {
            string temp_id = "";

            //Create new customer and get Customer_ID        
            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Insert_Customer";

                temp_id = GetCustomerID();

                cmd.Parameters.AddWithValue("@Customer_ID", temp_id);
                cmd.Parameters.AddWithValue("@ID_Card", my_app_info_person.ID_Card);
                cmd.Parameters.AddWithValue("@ID_Type", my_app_info_person.ID_Type);
                cmd.Parameters.AddWithValue("@First_Name", my_app_info_person.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", my_app_info_person.Last_Name);
                cmd.Parameters.AddWithValue("@Gender", my_app_info_person.Gender);
                cmd.Parameters.AddWithValue("@Birth_Date", my_app_info_person.Birth_Date);
                cmd.Parameters.AddWithValue("@Country_ID", my_app_info_person.Country_ID);
                cmd.Parameters.AddWithValue("@Khmer_First_Name", my_app_info_person.Khmer_First_Name);
                cmd.Parameters.AddWithValue("@Khmer_Last_Name", my_app_info_person.Khmer_Last_Name);
                cmd.Parameters.AddWithValue("@Father_First_Name", my_app_info_person.Father_First_Name);
                cmd.Parameters.AddWithValue("@Father_Last_Name", my_app_info_person.Father_Last_Name);
                cmd.Parameters.AddWithValue("@Mother_First_Name", my_app_info_person.Mother_First_Name);
                cmd.Parameters.AddWithValue("@Mother_Last_Name", my_app_info_person.Mother_Last_Name);
                cmd.Parameters.AddWithValue("@Prior_First_Name", my_app_info_person.Prior_First_Name);
                cmd.Parameters.AddWithValue("@Prior_Last_Name", my_app_info_person.Prior_Last_Name);

                cmd.Parameters.AddWithValue("@Created_On", DateTime.Now); //use created_on when inserting data; real time use datetime.now
                cmd.Parameters.AddWithValue("@Created_By", created_by);
                cmd.Parameters.AddWithValue("@Created_Note", "");

                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    customer_id = temp_id;
                }

                catch (Exception ex)
                {
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [InsertCustomer] in class [da_customer]. Details: " + ex.Message);
                }
                con.Close();
            }

        }
        else
        {
            customer_id = check_existing_customer; // search and get latest + 1 
        }

        
        return customer_id;
    }

    /// <summary>
    /// This function usefull for CI Product
    /// </summary>
    /// <param name="customer">Customer object</param>
    /// <param name="created_by">User Login</param>
    /// <returns></returns>
    public static bool InsertCustomer(bl_customer customer, string created_by)
    {

        bool result = false;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Insert_Customer";

                cmd.Parameters.AddWithValue("@Customer_ID", customer.Customer_ID);
                cmd.Parameters.AddWithValue("@ID_Card", customer.ID_Card);
                cmd.Parameters.AddWithValue("@ID_Type", customer.ID_Type);
                cmd.Parameters.AddWithValue("@First_Name", customer.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", customer.Last_Name);
                cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                cmd.Parameters.AddWithValue("@Birth_Date", customer.Birth_Date);
                cmd.Parameters.AddWithValue("@Country_ID", customer.Country_ID);
                cmd.Parameters.AddWithValue("@Khmer_First_Name", customer.Khmer_First_Name);
                cmd.Parameters.AddWithValue("@Khmer_Last_Name", customer.Khmer_Last_Name);
                cmd.Parameters.AddWithValue("@Father_First_Name", customer.Father_First_Name);
                cmd.Parameters.AddWithValue("@Father_Last_Name", customer.Father_Last_Name);
                cmd.Parameters.AddWithValue("@Mother_First_Name", customer.Mother_First_Name);
                cmd.Parameters.AddWithValue("@Mother_Last_Name", customer.Mother_Last_Name);
                cmd.Parameters.AddWithValue("@Prior_First_Name", customer.Prior_First_Name);
                cmd.Parameters.AddWithValue("@Prior_Last_Name", customer.Prior_Last_Name);

                cmd.Parameters.AddWithValue("@Created_On", DateTime.Now); //use created_on when inserting data; real time use datetime.now
                cmd.Parameters.AddWithValue("@Created_By", created_by);
                cmd.Parameters.AddWithValue("@Created_Note", "");

                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                }

                catch (Exception ex)
                {
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [InsertCustomer] in class [da_customer]. Details: " + ex.Message);
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Eror function [InsertCustomer(bl_customer customer, string created_by)] in class [], detail: " + ex.Message);
        }
        
        return result;
    }

    public static string InsertCustomerReserved(string app_register_id, string customer_id, string created_by, DateTime created_on)
    {
        //Get a record from table app_info_person by app_register_id
        bl_app_info_person my_app_info_person = GetAppInfoPerson(app_register_id);

        //Check existing customer
        string check_existing_customer = CheckCustomerByParameters(my_app_info_person.First_Name, my_app_info_person.Last_Name, my_app_info_person.Gender, my_app_info_person.Birth_Date);

        if (check_existing_customer == "")
        {
            //Create new customer and get Customer_ID        
            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Insert_Customer";

                cmd.Parameters.AddWithValue("@Customer_ID", customer_id);
                cmd.Parameters.AddWithValue("@ID_Card", my_app_info_person.ID_Card);
                cmd.Parameters.AddWithValue("@ID_Type", my_app_info_person.ID_Type);
                cmd.Parameters.AddWithValue("@First_Name", my_app_info_person.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", my_app_info_person.Last_Name);
                cmd.Parameters.AddWithValue("@Gender", my_app_info_person.Gender);
                cmd.Parameters.AddWithValue("@Birth_Date", my_app_info_person.Birth_Date);
                cmd.Parameters.AddWithValue("@Country_ID", my_app_info_person.Country_ID);
                cmd.Parameters.AddWithValue("@Khmer_First_Name", my_app_info_person.Khmer_First_Name);
                cmd.Parameters.AddWithValue("@Khmer_Last_Name", my_app_info_person.Khmer_Last_Name);
                cmd.Parameters.AddWithValue("@Father_First_Name", my_app_info_person.Father_First_Name);
                cmd.Parameters.AddWithValue("@Father_Last_Name", my_app_info_person.Father_Last_Name);
                cmd.Parameters.AddWithValue("@Mother_First_Name", my_app_info_person.Mother_First_Name);
                cmd.Parameters.AddWithValue("@Mother_Last_Name", my_app_info_person.Mother_Last_Name);
                cmd.Parameters.AddWithValue("@Prior_First_Name", my_app_info_person.Prior_First_Name);
                cmd.Parameters.AddWithValue("@Prior_Last_Name", my_app_info_person.Prior_Last_Name);

                cmd.Parameters.AddWithValue("@Created_On", DateTime.Now); //use created_on when inserting data; real time use datetime.now
                cmd.Parameters.AddWithValue("@Created_By", created_by);
                cmd.Parameters.AddWithValue("@Created_Note", "");

                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    //Add error to log 
                    customer_id = "";
                    Log.AddExceptionToLog("Error in function [InsertCustomerReserved] in class [da_customer]. Details: " + ex.Message);
                }
                con.Close();
            }

        }
        else
        {
            customer_id = check_existing_customer;
        }


        return customer_id;
    }

    //Get app info person record by app_register_id
    public static bl_app_info_person GetAppInfoPerson(string app_register_id)
    {
        bl_app_info_person my_app_info_person = new bl_app_info_person();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_App_Info_Person_By_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);        

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                    my_app_info_person.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    my_app_info_person.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                    my_app_info_person.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                    my_app_info_person.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    my_app_info_person.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));
                    my_app_info_person.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                    my_app_info_person.Country_ID = rdr.GetString(rdr.GetOrdinal("Country_ID"));
                    my_app_info_person.Khmer_First_Name = rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));
                    my_app_info_person.Khmer_Last_Name = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name"));
                    my_app_info_person.Father_First_Name = rdr.GetString(rdr.GetOrdinal("Father_First_Name"));
                    my_app_info_person.Father_Last_Name = rdr.GetString(rdr.GetOrdinal("Father_Last_Name"));
                    my_app_info_person.Mother_First_Name = rdr.GetString(rdr.GetOrdinal("Mother_First_Name"));
                    my_app_info_person.Mother_Last_Name = rdr.GetString(rdr.GetOrdinal("Mother_Last_Name"));
                    my_app_info_person.Prior_First_Name = rdr.GetString(rdr.GetOrdinal("Prior_First_Name"));
                    my_app_info_person.Prior_Last_Name = rdr.GetString(rdr.GetOrdinal("Prior_Last_Name"));
                    my_app_info_person.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    
                       
                    }
                }
            }
            
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerID] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }

        return my_app_info_person;
    }


    //Get customer id by param: first name, last name, gender, dob
    public static string GetCustomerIDByParameters(string first_name, string last_name, int gender, DateTime birth_date)
    {
        string customer_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Last_Customer_ID_By_Params";

            cmd.Parameters.AddWithValue("@First_Name", first_name);
            cmd.Parameters.AddWithValue("@Last_Name", last_name);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Birth_Date", birth_date);        

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        customer_id = rdr.GetString(rdr.GetOrdinal("Customer_ID"));                        
                    }
                    else
                    {
                        customer_id = rdr.GetString(rdr.GetOrdinal("Customer_ID"));

                        int strConvert = Convert.ToInt16(customer_id) + 1;
                        customer_id = strConvert.ToString("D8");
                    }
                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerID] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return customer_id;
    }
    

    //Check if customer is exisiting customer
    public static string CheckCustomerByParameters(string first_name, string last_name, int gender, DateTime birth_date)
    {        

        string customer_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Check_Existing_Customer_ID_By_Params";

            cmd.Parameters.AddWithValue("@First_Name", first_name);
            cmd.Parameters.AddWithValue("@Last_Name", last_name);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Birth_Date", birth_date);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        customer_id = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                    }
                        
                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [CheckCustomerByParameters] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return customer_id;
    }


    //Check if customer already have WING account
    public static bool CheckCustomerWINGAccount(string customer_id)
    {

        bool check_customer = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Check_WING_Account_for_Customer";

            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);


            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        check_customer = true;
                    }

                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [CheckCustomerWINGAccount] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return check_customer;
    }


    //GEt WING SK from Ct_Policy_WING for existing customer
    public static string GetCustomerWINGSK(string customer_id)
    {
        string wing_sk = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Customer_WING_SK";

            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);
            

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        wing_sk = rdr.GetString(rdr.GetOrdinal("WING_SK"));
                    }
                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerWINGSK] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return wing_sk;
    }

    //Get last WING SK
    public static string GetLastUsableWINGSK()
    {
        string wing_sk = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_WING_SK";

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        wing_sk = rdr.GetString(rdr.GetOrdinal("SK"));

                    }
                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetLastUsableWINGSK] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return wing_sk;
    }


    //Get last customer number
    public static string GetCustomerID()
    {
        string customer_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Last_Customer_ID";

            cmd.Connection = con;
            con.Open();
            try
            {                
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        customer_id = rdr.GetString(rdr.GetOrdinal("Customer_ID"));

                        int strConvert = Convert.ToInt16(customer_id) + 1;
                        customer_id = strConvert.ToString("D8");
                       
                    }
                }
            }
            
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerID] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return customer_id;
    }


    //Get customer by policy ID
    public static bl_customer GetCustomerByPolicyID(string policy_id)
    {
        bl_customer my_customer = new bl_customer();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Customer_By_Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_customer.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                        my_customer.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                        my_customer.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                        my_customer.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                        my_customer.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));
                        my_customer.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        my_customer.Country_ID = rdr.GetString(rdr.GetOrdinal("Country_ID"));
                        my_customer.Khmer_First_Name = rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));
                        my_customer.Khmer_Last_Name = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name"));
                        my_customer.Father_First_Name = rdr.GetString(rdr.GetOrdinal("Father_First_Name"));
                        my_customer.Father_Last_Name = rdr.GetString(rdr.GetOrdinal("Father_Last_Name"));
                        my_customer.Mother_First_Name = rdr.GetString(rdr.GetOrdinal("Mother_First_Name"));
                        my_customer.Mother_Last_Name = rdr.GetString(rdr.GetOrdinal("Mother_Last_Name"));
                        my_customer.Prior_First_Name = rdr.GetString(rdr.GetOrdinal("Prior_First_Name"));
                        my_customer.Prior_Last_Name = rdr.GetString(rdr.GetOrdinal("Prior_Last_Name"));

                    }
                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerByPolicyID] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }

        return my_customer;
    }

    //Get customer by customer ID
    public static bl_customer GetCustomerByCustomerID(string customer_id)
    {
        bl_customer my_customer = new bl_customer();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Customer_By_Customer_ID";

            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_customer.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                        my_customer.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                        my_customer.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                        my_customer.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                        my_customer.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));
                        my_customer.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        my_customer.Country_ID = rdr.GetString(rdr.GetOrdinal("Country_ID"));
                        my_customer.Khmer_First_Name = rdr["Khmer_First_Name"] == DBNull.Value ? "" : rdr["Khmer_First_Name"].ToString(); // rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));
                        my_customer.Khmer_Last_Name = rdr["Khmer_Last_Name"] == DBNull.Value ? "" : rdr["Khmer_Last_Name"].ToString(); //rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name"));
                        my_customer.Father_First_Name = rdr["Father_First_Name"] == DBNull.Value ? "" : rdr["Father_First_Name"].ToString();// rdr.GetString(rdr.GetOrdinal("Father_First_Name"));
                        my_customer.Father_Last_Name = rdr["Father_Last_Name"] == DBNull.Value ? "" : rdr["Father_Last_Name"].ToString(); //rdr.GetString(rdr.GetOrdinal("Father_Last_Name"));
                        my_customer.Mother_First_Name = rdr["Mother_First_Name"] == DBNull.Value ? "" : rdr["Mother_First_Name"].ToString(); //rdr.GetString(rdr.GetOrdinal("Mother_First_Name"));
                        my_customer.Mother_Last_Name = rdr["Mother_Last_Name"] == DBNull.Value ? "" : rdr["Mother_Last_Name"].ToString(); //rdr.GetString(rdr.GetOrdinal("Mother_Last_Name"));
                        my_customer.Prior_First_Name = rdr["Prior_First_Name"] == DBNull.Value ? "" : rdr["Prior_First_Name"].ToString(); //rdr.GetString(rdr.GetOrdinal("Prior_First_Name"));
                        my_customer.Prior_Last_Name = rdr["Prior_Last_Name"] == DBNull.Value ? "" : rdr["Prior_Last_Name"].ToString(); //rdr.GetString(rdr.GetOrdinal("Prior_Last_Name"));

                        my_customer.Customer_ID = rdr["customer_id"].ToString();

                    }
                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerByPolicyID] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }

        return my_customer;
    }


    //Update customer by customer id
    public static bool UpdateCustomerByID(bl_customer my_customer)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Update_Customer_By_Customer_ID";

            cmd.Parameters.AddWithValue("@Customer_ID", my_customer.Customer_ID);
            cmd.Parameters.AddWithValue("@ID_Card", my_customer.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", my_customer.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", my_customer.First_Name);
            cmd.Parameters.AddWithValue("@Last_Name", my_customer.Last_Name);
            cmd.Parameters.AddWithValue("@Country_ID", my_customer.Country_ID);
            cmd.Parameters.AddWithValue("@Khmer_First_Name", my_customer.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", my_customer.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", my_customer.Father_First_Name);
            cmd.Parameters.AddWithValue("@Father_Last_Name", my_customer.Father_Last_Name);
            cmd.Parameters.AddWithValue("@Mother_First_Name", my_customer.Mother_First_Name);
            cmd.Parameters.AddWithValue("@Mother_Last_Name", my_customer.Mother_Last_Name);
            cmd.Parameters.AddWithValue("@Prior_First_Name", my_customer.Prior_First_Name);
            cmd.Parameters.AddWithValue("@Prior_Last_Name", my_customer.Prior_Last_Name);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateCustomerByID] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return result;

    }

    //Function to check existing customer
    public static bool CheckExistingCustomer(string first_name, string last_name, int genter, DateTime dob)
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
                myCommand.CommandText = "SP_Check_Existing_Customer_ID_By_Params";
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name);
                myCommand.Parameters.AddWithValue("@Gender", genter);
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
            Log.AddExceptionToLog("Error in function [CheckExistingCustomer] in class [da_customer]. Details: " + ex.Message);
        }
        return result;
    }

    //Get customer id (private key) by first_name, last_name, gender, dob    
    public static string GetCustomerIDByNameDOBGender(string first_name, string last_name, int genter, DateTime dob)
    {
        string customer_id = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Ct_Customer_ID_By_Name_DOB_Gender";
                myCommand.Parameters.AddWithValue("@First_Name", first_name);
                myCommand.Parameters.AddWithValue("@Last_Name", last_name);
                myCommand.Parameters.AddWithValue("@Gender", genter);
                myCommand.Parameters.AddWithValue("@DOB", dob);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        customer_id = myReader.GetString(myReader.GetOrdinal("Customer_ID")); ;

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
            Log.AddExceptionToLog("Error in function [GetCustomerIDByNameDOBGender] in class [da_customer]. Details: " + ex.Message);
        }
        return customer_id;
    }

    #endregion

    #region Family protection
    public class bl_customer_rider : bl_customer
    {
        public int Level { set; get; }
    }
    public static List<da_application_fp6.bl_app_info_person_sub> GetAppInfoPersonSub(string app_register_id)
    {
        List<da_application_fp6.bl_app_info_person_sub> listPerson = new List<da_application_fp6.bl_app_info_person_sub>();
        DataTable tbl = new DataTable();
        try
        {
            tbl = da_application_fp6.GetDataTable("SP_Get_App_Info_Person_Sub_FP6_By_App_Register_ID", app_register_id);
            foreach (DataRow row in tbl.Rows)
            {
                da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                person.App_Register_ID = app_register_id;
                person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                person.Country_ID = row["nationality"].ToString();
                person.Father_First_Name = row["fatherFirstName"].ToString();
                person.Father_Last_Name = row["fatherSurName"].ToString();
                person.First_Name = row["firstEnName"].ToString();
                person.Last_Name = row["surEnName"].ToString();
                person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                person.Khmer_Last_Name = row["surKhName"].ToString();
                if (row["gender"].ToString().Trim().ToUpper() == "MALE")
                {
                    person.Gender = 1;
                }
                else
                {
                    person.Gender = 0;
                }
                //person.Gender = Convert.ToInt32(row["gender"].ToString());
                person.ID_Card = row["idNumber"].ToString();
                person.ID_Type = Convert.ToInt32(row["idtype"].ToString());

                person.Level = Convert.ToInt32(row["level"].ToString().Trim());
                person.Mother_First_Name = row["motherFirstName"].ToString();
                person.Mother_Last_Name = row["motherSurName"].ToString();
                person.Person_ID = row["id"].ToString().Trim();
                person.Prior_First_Name = row["previousFirstName"].ToString();
                person.Prior_Last_Name = row["previousSurName"].ToString();

                person.Marital_Status = row["Marital_Status"].ToString();
                person.Relationship = row["Relationship"].ToString();

                listPerson.Add(person);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetAppInfoPersonSub] in class [da_customer], Detail: " + ex.Message);
        }
               
        return listPerson;
    }

    public static string InsertCustomerRider(string app_register_id, string created_by)
    {
        string temp_id;
        string customer_id="";
        string connString = AppConfiguration.GetConnectionString();
            
            try
            {
                List<da_application_fp6.bl_app_info_person_sub> list_my_app_info_person = new List<da_application_fp6.bl_app_info_person_sub>();
                list_my_app_info_person = GetAppInfoPersonSub(app_register_id);
                foreach (da_application_fp6.bl_app_info_person_sub my_app_info_person in list_my_app_info_person)
                {
                    String existCustomerId = "";
                    existCustomerId = GetCustomerIDRiderByParameters(my_app_info_person.First_Name, my_app_info_person.Last_Name, my_app_info_person.Gender, my_app_info_person.Birth_Date);
                    if (existCustomerId == "")
                    {
                        temp_id = GetLastCustomerIDRider();
                        using (SqlConnection con = new SqlConnection(connString))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_Insert_Customer_Rider";

                            cmd.Parameters.AddWithValue("@Customer_ID", temp_id);
                            cmd.Parameters.AddWithValue("@ID_Card", my_app_info_person.ID_Card);
                            cmd.Parameters.AddWithValue("@ID_Type", my_app_info_person.ID_Type);
                            cmd.Parameters.AddWithValue("@First_Name", my_app_info_person.First_Name);
                            cmd.Parameters.AddWithValue("@Last_Name", my_app_info_person.Last_Name);
                            cmd.Parameters.AddWithValue("@Gender", my_app_info_person.Gender);
                            cmd.Parameters.AddWithValue("@Birth_Date", my_app_info_person.Birth_Date);
                            cmd.Parameters.AddWithValue("@Country_ID", my_app_info_person.Country_ID);
                            cmd.Parameters.AddWithValue("@Khmer_First_Name", my_app_info_person.Khmer_First_Name);
                            cmd.Parameters.AddWithValue("@Khmer_Last_Name", my_app_info_person.Khmer_Last_Name);
                            cmd.Parameters.AddWithValue("@Father_First_Name", my_app_info_person.Father_First_Name);
                            cmd.Parameters.AddWithValue("@Father_Last_Name", my_app_info_person.Father_Last_Name);
                            cmd.Parameters.AddWithValue("@Mother_First_Name", my_app_info_person.Mother_First_Name);
                            cmd.Parameters.AddWithValue("@Mother_Last_Name", my_app_info_person.Mother_Last_Name);
                            cmd.Parameters.AddWithValue("@Prior_First_Name", my_app_info_person.Prior_First_Name);
                            cmd.Parameters.AddWithValue("@Prior_Last_Name", my_app_info_person.Prior_Last_Name);
                            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now); //use created_on when inserting data; real time use datetime.now
                            cmd.Parameters.AddWithValue("@Created_By", created_by);
                            cmd.Parameters.AddWithValue("@Created_Note", "");
                            cmd.Parameters.AddWithValue("@Level", my_app_info_person.Level);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            customer_id = temp_id;
                            con.Close();
                        }
                        
                    }
                    else if (existCustomerId != "" && existCustomerId != "err")
                    {
                        customer_id = existCustomerId;
                    }
                    else // err mean that error while get existing customer id
                    {
                        customer_id = "";
                    }

                }
               
                
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertCustomerRider] in class [da_customer]. Details: " + ex.Message);
            }
            return customer_id; 
        
    }
    //TODO: Get last customer number for ride 12102016
    public static string GetLastCustomerIDRider()
    {
        string customer_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Last_Customer_ID_Rider";

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    customer_id = rdr["Customer_ID"].ToString();
                    if (customer_id != "")
                    {
                        int strConvert = Convert.ToInt16(customer_id) + 1;
                        customer_id = strConvert.ToString("D8");
                    }
                    else
                    {
                        customer_id = 1.ToString("D8");
                    }
                    

                }
                else
                {
                    customer_id = 1.ToString("D8");
                }
               
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerIDRider] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return customer_id;
    }
    //TODO: Get exist customer rider 12102016
    public static string GetCustomerIDRiderByParameters(string first_name, string last_name, int gender, DateTime birth_date)
    {
        string customer_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Customer_ID_By_Params";

            cmd.Parameters.AddWithValue("@First_Name", first_name);
            cmd.Parameters.AddWithValue("@Last_Name", last_name);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Birth_Date", birth_date);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        customer_id = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                    }
                    else
                    {
                        customer_id = "";
                    }
                }
            }

            catch (Exception ex)
            {
                customer_id = "err";
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCustomerIDByParameters] in class [da_customer]. Details: " + ex.Message);
            }
            con.Close();
        }
        return customer_id;
    }
    #endregion Family protection

    #region Save all customer types
    public static bool SaveCustomer(bl_customer customer)
    {
        bool status=false;
        try{
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CUSTOMER_ALL_TYPES", 
                new string[,] { {"@CUST_NO", customer.Customer_Number},
                                {"@FIRST_NAME_EN", customer.First_Name},
                                {"@LAST_NAME_EN", customer.Last_Name},
                                {"@LAST_NAME_KH",customer.Khmer_Last_Name},
                                {"@FIRST_NAME_KH",customer.Khmer_First_Name},
                                {"@GENDER", customer.Gender+""} ,
                                {"@ID_TYPE", customer.ID_Type+""},
                                {"@ID_CARD", customer.ID_Card},
                                {"@BIRTH_DATE", customer.Birth_Date+""},
                                {"@NATIONALITY", customer.Nationality},
                                {"@MOTHER_FIRST_NAME_EN", customer.Mother_First_Name},
                                {"@MOTHER_LAST_NAME_EN", customer.Mother_Last_Name},
                                {"@MOTHER_FIRST_NAME_KH", customer.Mother_First_Name_KH},
                                {"@MOTHER_LAST_NAME_KH", customer.Mother_Last_Name_KH},
                                {"@FATHER_FIRST_NAME_EN", customer.Father_First_Name},
                                {"@FATHER_LAST_NAME_EN", customer.Father_Last_Name},
                                {"@FATHER_FIRST_NAME_KH", customer.Father_First_Name_KH},
                                {"@FATHER_LAST_NAME_KH", customer.Father_Last_Name_KH},
                                {"@CREATED_BY", customer.Created_By},
                                {"@CREATED_ON", customer.Created_On+""}}, "da_customer => SaveCustomer");
            
        }
        catch(Exception ex){
        Log.AddExceptionToLog("Error function [SaveCustomer] in class [da_customer], Detail: " +ex.Message);

        }
        return status;
    }

  
    public static bool UpdateCustomer(bl_customer customer){
    
        bool status=false;
        try{
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CUSTOMER_ALL_TYPES_BY_CUSTOMER_ID", 
                new string[,] {{"@CUST_NO", customer.Customer_Number},
                                {"@FIRST_NAME_KH",customer.Khmer_First_Name},
                                {"@LAST_NAME_KH",customer.Khmer_Last_Name},
                                {"@FIRST_NAME_EN", customer.First_Name},
                                {"@LAST_NAME_EN", customer.Last_Name},
                                {"@MOTHER_FIRST_NAME_KH", customer.Mother_First_Name_KH},
                                {"@MOTHER_LAST_NAME_KH", customer.Mother_Last_Name_KH},
                                {"@MOTHER_FIRST_NAME_EN", customer.Mother_First_Name},
                                {"@MOTHER_LAST_NAME_EN", customer.Mother_Last_Name},
                                {"@FATHER_FIRST_NAME_KH", customer.Father_First_Name_KH},
                                {"@FATHER_LAST_NAME_KH", customer.Father_Last_Name_KH},
                                {"@FATHER_FIRST_NAME_EN", customer.Father_First_Name},
                                {"@FATHER_LAST_NAME_EN", customer.Father_Last_Name},
                                {"@BIRTH_DATE", customer.Birth_Date+""},
                                {"@ID_TYPE", customer.ID_Type+""},
                                {"@ID_CARD", customer.ID_Card},
                                {"@NATIONALITY", customer.Nationality},
                                {"@GENDER", customer.Gender+""} ,
                                //{"@CREATED_BY", customer.Updated_By}, 
                                {"@CREATED_ON", customer.Created_On+"" }}, "da_customer => UpdateCustomer");
            
        }
        catch(Exception ex){
        Log.AddExceptionToLog("Error function [UpdateCustomer] in class [da_customer], Detail: " +ex.Message);

        }
        return status;
    }

    /// <summary>
    /// Get an existing customer by first_name_en, last_name_en, gender, id_card and birth_date
    /// </summary>
    /// <param name="first_name_en"></param>
    /// <param name="last_name_en"></param>
    /// <param name="gender"></param>
    /// <param name="id_card"></param>
    /// <param name="birth_date"></param>
    /// <returns></returns>
    public static string GetExistingCustomer(string first_name_en, string last_name_en, int gender, string id_card, DateTime birth_date) {
        string customer_id = "";
        try
        {
            DataTable tbl_customer = DataSetGenerator.Get_Data_Soure("SP_CHECK_EXISTING_CUSTOMER_All_TYPE", new string[,] { {"@FIRST_NAME_EN", first_name_en.Trim() }, 
                                                                                                                    {"@LAST_NAME_EN", last_name_en.Trim()}, 
                                                                                                                    {"@GENDER", gender+""}, 
                                                                                                                    {"@ID_CARD", id_card.Trim()}, 
                                                                                                                    {"@BIRTH_DATE", birth_date+""} });
            foreach (DataRow row in tbl_customer.Rows) {
                customer_id = row["cust_id"].ToString();
            }
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function [GetExistingCustomer] in class [da_customer], Detail: " + ex.Message);
        }

        return customer_id;
    }

    /// <summary>
    /// This function is focus on Policy flat rate
    /// </summary>
    /// <param name="customer_name">First name + Last name or last name + first name</param>
    /// <param name="customer_type_id"></param>
    /// <param name="id_card"></param>
    /// <returns></returns>
    public static List<bl_customer> GetCustomerList(string customer_name, string customer_type_id, string id_card, string customer_id, string customer_number)
    {
        List<bl_customer> customer_list = new List<bl_customer>();
        try
        {
            DataTable tbl_customer = DataSetGenerator.Get_Data_Soure("SP_GET_CUSTOMER_ALL_TYPE", new string[,] { {"@FULL_NAME",customer_name },
                                                                                                                  {"@ID_CARD", id_card}, 
                                                                                                                  {"@CUSTOMER_ID", customer_id}, 
                                                                                                                  {"@CUSTOMER_NUMBER", customer_number} });
            foreach(DataRow row in tbl_customer.Rows)
            {
                customer_list.Add(new bl_customer()
                {
                    Customer_Number = row["customer_id"].ToString(),
                    First_Name = row["first_name"].ToString(),
                    Last_Name = row["last_name"].ToString(),
                    ID_Card = row["id_card"].ToString(),
                    ID_Type = Convert.ToInt32(row["id_type"].ToString()),
                    Gender = Convert.ToInt32(row["gender"].ToString()),
                    Birth_Date = Convert.ToDateTime(row["birth_date"].ToString()),
                    Nationality = row["country_id"].ToString(),
                    Khmer_First_Name = row["first_name_kh"].ToString(),
                    Khmer_Last_Name = row["last_name_kh"].ToString(),
                    Mother_First_Name = row["mother_first_name"].ToString(),
                    Mother_Last_Name = row["mother_last_name"].ToString(),
                    Tel = row["tel"].ToString(),
                    Mobile = row["mobile"].ToString(),   
                    Fax = row["fax"].ToString(),   
                    Mail = row["email"].ToString(),
                    Country_ID = row["country"].ToString(),
                    Address1 = row["address_#1"].ToString(),
                    Address2 = row["address_#2"].ToString(),
                    Province = row["province"].ToString(),
                    Zip_Code = row["zip_code"].ToString(),
                    Created_By = row["created_by"].ToString(),
                    Created_On = Convert.ToDateTime(row["created_on"].ToString()),
                    Remarks = row["remarks"].ToString()

                });
            }
            return customer_list;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetCustomerList] in class [da_customer], Detail: " + ex.Message);
        }
        return customer_list;
    }
    public static List<bl_customer> GetCustomerList(string customer_id, string full_name, DateTime dob, int gender, string id_card)
    {
        List<bl_customer> custList = new List<bl_customer>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_CUSTOMER_GET", new string[,] { 
                {"@customer_number", customer_id},
                {"@full_name", full_name}, 
                {"@gender", gender+""},
                {"@birth_date", dob+""},
                {"@id_card", id_card}
            }, "da_customer => GetCustomerList(string customer_id, string full_name, DateTime dob, int gender, string id_card)");

            foreach (DataRow row in tbl.Rows)
            {
                custList.Add(new bl_customer() {
                Customer_ID= row["customer_id"].ToString().Trim(),
                ID_Type= Int32.Parse(row["id_type"].ToString().Trim()),
                ID_Card = row["id_card"].ToString().Trim(),
                First_Name = row["first_name"].ToString().Trim(),
                Last_Name=row["last_name"].ToString().Trim(),
                Khmer_First_Name= row["khmer_first_name"].ToString().Trim(),
                Khmer_Last_Name=row["khmer_last_name"].ToString().Trim(),
                Gender=Int32.Parse(row["gender"].ToString().Trim()),
                Birth_Date = DateTime.Parse(row["birth_date"].ToString().Trim()).Date,
                Country_ID = row["country_id"].ToString().Trim()
                });
            }
        }
        catch (Exception ex)
        {
            custList = new List<bl_customer>();
            Log.AddExceptionToLog(Log.GenerateLog(ex));
        }
        return custList;
    }
    public static string GenerateNewCustomerNumber()
    {
        string new_cust_no = "1";
        try
        {
            foreach (DataRow row in DataSetGenerator.Get_Data_Soure("SP_GENERATE_NEW_CUSTOMER_NUMBER", new string[,] { }).Rows)
            {
                new_cust_no = row["CUST_NO"].ToString();
            }
            if (new_cust_no.Trim() == "")
            {
                new_cust_no = "1";
            }
        }
        catch (Exception ex)
        {
            new_cust_no = "1";
            Log.AddExceptionToLog("Error function [GenerateNewCustomerNumber] in class [da_customer], detail: " + ex.Message);
        }
        return new_cust_no;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="customer_name"></param>
    /// <param name="customer_type_id"></param>
    /// <param name="id_card"></param>
    /// <param name="customer_id"></param>
    /// <param name="customer_number"></param>
    /// <returns></returns>
    public static List<bl_customer> GetCustomerListByParameters(string customer_name, string customer_type_id, string id_card, string customer_id, string customer_number)
    {
        List<bl_customer> customer_list = new List<bl_customer>();
        try
        {
            DataTable tbl_customer = DataSetGenerator.Get_Data_Soure("SP_GET_CUSTOMER_ALL_TYPE_BY_PARAMETERS", new string[,] { {"@FULL_NAME",customer_name },
                                                                                                                    {"@CUSTOMER_TYPE_ID", customer_type_id}, 
                                                                                                                    {"@ID_CARD", id_card}, 
                                                                                                                    {"@CUSTOMER_ID", customer_id}, 
                                                                                                                    {"@CUSTOMER_NUMBER", customer_number} });
            foreach (DataRow row in tbl_customer.Rows)
            {
                customer_list.Add(new bl_customer()
                {
                    Customer_ID = row["cust_id"].ToString(),
                    Customer_Type = row["customer_type_id"].ToString(),
                    Customer_Number = row["cust_no"].ToString(),
                    First_Name = row["first_name_en"].ToString(),
                    Last_Name = row["last_name_en"].ToString(),
                    Khmer_First_Name = row["first_name_kh"].ToString(),
                    Khmer_Last_Name = row["last_name_kh"].ToString(),
                    Birth_Date = Convert.ToDateTime(row["birth_date"].ToString()),
                    Gender = Convert.ToInt32(row["gender"].ToString()),
                    ID_Card = row["id_card"].ToString(),
                    ID_Type = Convert.ToInt32(row["id_type_id"].ToString()),
                    Marital_Status = row["marital_status"].ToString(),
                    Status = Convert.ToInt32(row["status_id"].ToString()),
                    //Mother
                    Mother_First_Name = row["mother_first_name_en"].ToString(),
                    Nationality = row["nationality"].ToString(),
                    //address
                    Address = row["address"].ToString(),
                    Country_ID = row["country_id"].ToString(),
                    Zip_Code = row["zip_code"].ToString(),
                    Province = row["province_id"].ToString(),
                    Khan = row["khan_id"].ToString(),
                    Sangkat = row["sangkat_id"].ToString(),
                    //occupation
                    Sector = row["Sector"].ToString(),
                    Occupation = row["occupation"].ToString(),
                    //company
                    Register_Name = row["register_name"].ToString(),
                    Register_Number = row["register_number"].ToString(),
                    Register_Date = Convert.ToDateTime(row["register_date"].ToString()),
                    TIN_NO = row["tinno"].ToString(),
                    Created_By = row["created_by"].ToString(),
                    Created_On = Convert.ToDateTime(row["created_datetime"].ToString()),
                    Remarks = row["remarks"].ToString()

                });
            }
            return customer_list;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetCustomerList] in class [da_customer], Detail: " + ex.Message);
        }
        return customer_list;
    }

    public static bool DeleteCustomer(string customer_id)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_CUSTOMER_BY_CUSTOMER_ID", new string[,] { { "@CUSTOMER_ID", customer_id } }, "da_customer => DeleteCustomer");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteCustomer] in class [da_customer], Detail: " + ex.Message);
        }
        return status;
    }

    public static bool SaveContact(bl_contact contact)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CONTACT", new string[,] { {"@CUST_ID", contact.Cust_ID }, 
                                                                                                                          {"@NAME", contact.Name.Trim()},
                                                                                                                          {"@MOBILE", contact.Mobile},
                                                                                                                          {"@TEL", contact.Tel},
                                                                                                                          {"@FAX", contact.Fax},
                                                                                                                          {"@MAIL",contact.Mail},
                                                                                                                          {"@RESPONSIBILITY", contact.Responsibility},
                                                                                                                          {"@REMARKS",contact.Remarks},
                                                                                                                          {"@ADDRESS",contact.Address},
                                                                                                                          {"@CREATED_BY", contact.Created_By},
                                                                                                                          {"@CREATED_DATETIME",contact.Created_DateTime+""}}, "da_customer => SaveContact");
            
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function [SaveCotact] in class [da_customer], Detail: " + ex.Message);
        }
        return status;
    }
    public static bool UpdateContact(bl_contact contact)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CONTACT_BY_ID", new string[,] {{"@ID", contact.ID+""},
                                                                                                                          {"@CUST_ID", contact.Cust_ID }, 
                                                                                                                          {"@NAME", contact.Name.Trim()},
                                                                                                                          {"@MOBILE", contact.Mobile},
                                                                                                                          {"@TEL", contact.Tel},
                                                                                                                          {"@FAX", contact.Fax},
                                                                                                                          {"@MAIL",contact.Mail},
                                                                                                                          {"@RESPONSIBILITY", contact.Responsibility},
                                                                                                                          {"@REMARKS",contact.Remarks},
                                                                                                                          {"@ADDRESS",contact.Address},
                                                                                                                          {"@UPDATED_BY", contact.Updated_By},
                                                                                                                          {"@UPDATED_DATETIME",contact.Updated_DateTime+""}}, "da_policy => UpdateContact");

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateContact] in class [da_customer], Detail: " + ex.Message);
        }
        return status;
    }

    /// <summary>
    /// Function focus on flat rate
    /// </summary>
    /// <param name="cust_id"></param>
    /// <returns></returns>
    public static List<bl_contact> GetContactList(string cust_id)
    {
        List<bl_contact> contact_list = new List<bl_contact>();
        try
        {
            DataTable tbl_contact = DataSetGenerator.Get_Data_Soure("SP_GET_CONTACT_BY_CUST_ID", new string[,] { { "@CUST_ID", cust_id} });
            foreach (DataRow row in tbl_contact.Rows)
            {
                contact_list.Add(new bl_contact() { ID=Convert.ToInt32(row["id"].ToString()), 
                                                    Name=row["name"].ToString(), 
                                                    Mobile=row["mobile"].ToString(), 
                                                    Tel=row["tel"].ToString(), 
                                                    Fax=row["fax"].ToString(), 
                                                    Mail=row["mail"].ToString(), 
                                                    Responsibility=row["responsibility"].ToString(), 
                                                    Remarks=row["remarks"].ToString(), 
                                                    Address=row["address"].ToString(),
                                                    Created_By= row["created_by"].ToString(), 
                                                    Created_DateTime = Convert.ToDateTime(row["created_datetime"].ToString()), 
                                                    Updated_DateTime = Convert.ToDateTime(row["updated_datetime"].ToString()), 
                                                    Updated_By=row["updated_by"].ToString()
                                                    });
            }
        }
        catch (Exception ex) 
        {
            Log.AddExceptionToLog("Error function [GetContactList] in class [da_customer], Detail: " + ex.Message);
            
        }
        return contact_list;
    }

    /// <summary>
    /// Get all khan list
    /// </summary>
    /// <returns></returns>
    public static List<bl_khan> GetAllKhanList()
    {
        List<bl_khan> khan_list = new List<bl_khan>();
        try
        {
            DataTable tbl_khan = DataSetGenerator.Get_Data_Soure("SP_GET_KHAN", new string[,] {});
            foreach (DataRow row in tbl_khan.Rows)
            {
                khan_list.Add(new bl_khan() { 
                 Pro_ID = row["pro_id"].ToString(),
                 Khan_ID=row["khan_id"].ToString(),
                 Khan_Name=row["khan_name"].ToString(),
                 Khan_Name_Kh=row["khan_name_kh"].ToString(),
                 Khan_Post_Code=row["khan_post_code"].ToString(),
                 Remarks=row["remarks"].ToString(),
                 Created_By=row["created_by"].ToString(),
                 Created_DateTime=Convert.ToDateTime(row["created_datetime"].ToString()),
                Updated_By=row["updated_by"].ToString(),
                Updated_DateTime=Convert.ToDateTime(row["updated_datetime"].ToString())
              
                });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetAllKhanList] in class [da_customer], Detail: " + ex.Message);
        }
        return khan_list;
    }
    /// <summary>
    /// Get Khan List filter by province id
    /// </summary>
    /// <param name="pro_id">Province id</param>
    /// <returns></returns>
    public static List<bl_khan> GetKhanListByProID(string pro_id)
    {
        List<bl_khan> khan_list = new List<bl_khan>();
        try
        {
            foreach(bl_khan khan in GetAllKhanList().Where(khan =>khan.Pro_ID==pro_id))//filter by province id
            {
                khan_list.Add(khan);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetKhanListByProID] in class [da_customer], Detail: " + ex.Message);
        }

        return khan_list;
    }

    /// <summary>
    /// Get all sangkat list
    /// </summary>
    /// <returns></returns>
    public static List<bl_sangkat> GetAllSangkatList()
    {
        List<bl_sangkat> sangkat_list = new List<bl_sangkat>();
        try
        {
            DataTable tbl_sangkat = DataSetGenerator.Get_Data_Soure("SP_GET_SANGKAT", new string[,] { });
            foreach (DataRow row in tbl_sangkat.Rows)
            {
                sangkat_list.Add(new bl_sangkat()
                {
                    Khan_ID = row["khan_id"].ToString(),
                    Sangkat_ID = row["sangkat_id"].ToString(),
                    Sangkat_Name = row["sangkat_name"].ToString(),
                    Sangkat_Name_Kh = row["sangkat_name_kh"].ToString(),
                    Sangkat_Post_Code = row["sangkat_post_code"].ToString(),
                    Remarks = row["remarks"].ToString(),
                    Created_By = row["created_by"].ToString(),
                    Created_DateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                    Updated_By = row["updated_by"].ToString(),
                    Updated_DateTime = Convert.ToDateTime(row["updated_datetime"].ToString())

                });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetAllSangkatList] in class [da_customer], Detail: " + ex.Message);
        }
        return sangkat_list;
    }
    /// <summary>
    /// Get sangkat list filter by khan id
    /// </summary>
    /// <param name="khan_id"></param>
    /// <returns></returns>
    public static List<bl_sangkat> GetSangkatListByKhanID(string khan_id)
    {
        List<bl_sangkat> sangkat_list = new List<bl_sangkat>();
        try
        {
            foreach (bl_sangkat sangkat in GetAllSangkatList().Where(sangkat => sangkat.Khan_ID == khan_id))//filter by khan id
            {
                sangkat_list.Add(sangkat);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetSangkatListByKhanID] in class [da_customer], Detail: " + ex.Message);
        }
        return sangkat_list;
    }

    /// <summary>
    /// Get Khan Name By Khan ID
    /// </summary>
    /// <param name="khan_id"></param>
    /// <returns></returns>
    public static string GetKhanName(string khan_id)
    {
        //get khan name.
        string khan_name = "";
        try
        {
            foreach (bl_khan khan in da_customer.GetAllKhanList().Where(_ => _.Khan_ID == khan_id))
            {
                if (khan.Khan_Name_Kh.Trim() != "" && khan.Khan_Name_Kh != null)
                {
                    khan_name = khan.Khan_Name_Kh;
                }
                else
                {
                    khan_name = khan.Khan_Name;
                }
                break;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [GetKhanName] in class [da_customer], Detail: " + ex.Message);
            khan_name = "";
        }
        return khan_name;
    }
    public static string GetSangkatName(string sangkat_id)
    {
        string sangkat_name = "";
        try
        {
            foreach (bl_sangkat sangkat in da_customer.GetAllSangkatList().Where(_ => _.Sangkat_ID.Trim() == sangkat_id.Trim()))
            {
                if (sangkat.Sangkat_Name_Kh.Trim() != "" && sangkat.Sangkat_Name_Kh != null)
                {
                    sangkat_name = sangkat.Sangkat_Name_Kh;
                }
                else
                {
                    sangkat_name = sangkat.Sangkat_Name;
                }
                break;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetSangKatName] in class [da_customer], Detail: " + ex.Message);
            sangkat_name = "";
        }
        return sangkat_name;
    }
    public static bool DeleteContact(int contact_id)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_CONTACT_BY_ID", new string[,] { { "@ID", contact_id + "" } }, "da_customer => DeleteContact");
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteContact] in class [da_customer], Detail: " + ex.Message);
        }

        return status;
    }

    /// <summary>
    /// Roll back while system save error.
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bool RollBack(string customer_id)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_CUSTOMER_ROLLBACK", new string[,] { { "@CUSTOMER_ID", customer_id } }, "da_customer => RollBack");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [RollBack] in class [da_customer], Detail: " + ex.Message);
        }
        return status;
    }

    /// <summary>
    /// Roll back while system save error.
    /// </summary>
    /// <param name="address_id"></param>
    /// <returns></returns>
    public static bool RollBackAddress(string address_id)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_ADDRESS_ROLLBACK", new string[,] { { "@ADDRESS_ID", address_id } }, "da_customer => RollBackAddress");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [RollBackAddress] in class [da_customer], Detail: " + ex.Message);
        }
        return status;
    }
   
    #endregion
}