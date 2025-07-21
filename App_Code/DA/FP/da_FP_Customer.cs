using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Collections;

/// <summary>
/// Summary description for da_FP_Customer
/// </summary>
public class da_FP_Customer
{
    #region 'Construtor'
    public da_FP_Customer()
	{
		//
		// TODO: Add constructor logic here
		//
    }
   
    #endregion
    //insert customer
    public static bool saveCustomer(bl_FP_Customer customer) 
    {

        bool result = false;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();

            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_INSERT_FP_CUSTOMER";

            //initialize parameters to store procedure
            var p = myCommand.Parameters;
            var c = customer;

            p.AddWithValue("@App_ID", c.App_ID);
            p.AddWithValue("@Customer_ID", c.Customer_id);
            p.AddWithValue("@ID_Type", c.Id_type);
            p.AddWithValue("@ID_Number", c.Id_number);
            p.AddWithValue("@Surname_Kh", c.Surname_kh);
            p.AddWithValue("@Surname_En", c.Surname_en);
            p.AddWithValue("@Firstname_Kh",c.First_name_kh);
            p.AddWithValue("@Firstname_En",c.First_name_en);
            p.AddWithValue("@Gender",c.Gender);
            p.AddWithValue("@DOB",c.Dob);
            p.AddWithValue("@Nationality",c.Nationality);
            p.AddWithValue("@Occupation",c.Occupation);
            p.AddWithValue("@Created_By", c.Created_by);
            p.AddWithValue("@Created_On", c.Created_on);
           
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;

        }
        catch (Exception ex) 
        {
            result = false;
            Log.AddExceptionToLog("Error function [saveCustomer()] in class [da_FP_Customer]. Detail: " + ex.Message);
        }

        return result;
    }
    //delete customer by application id
    public static bool deleteCustomerByApplicationID(string applicationID)
    {
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        bool result = false;
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_DELETE_FP_CUSTOMER_BY_APP_ID";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;
            para.AddWithValue("@App_ID", applicationID);

            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [deleteCustomerByApplicationID] in class [da_FP_Customer]. Detail: " + ex.Message);
        }
        return result;
    }
//get customer list
    public static List<bl_FP_Customer> getCustomerList(string applicationID)
    {

        List<bl_FP_Customer> customerList = new List<bl_FP_Customer>();
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();

        try
        {
            //Open connection

            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_GET_FP_CUSTOMER_LIST_BY_APP_ID";
            myCommand.Parameters.AddWithValue("@App_ID", applicationID);
            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    bl_FP_Customer myCustomer = new bl_FP_Customer();
                    myCustomer.App_ID= myReader.GetString(myReader.GetOrdinal("app_id"));
                    myCustomer.Customer_id = myReader.GetString(myReader.GetOrdinal("customer_id"));
                    myCustomer.Id_type= myReader.GetOrdinal("id_type");
                    myCustomer.Id_number = myReader.GetString(myReader.GetOrdinal("id_number"));
                    myCustomer.Surname_kh = myReader.GetString(myReader.GetOrdinal("surnamekh"));
                    myCustomer.First_name_kh = myReader.GetString(myReader.GetOrdinal("firstnamekh"));
                    myCustomer.Surname_en = myReader.GetString(myReader.GetOrdinal("surnameen"));
                    myCustomer.First_name_en = myReader.GetString(myReader.GetOrdinal("firstnameen"));
                    myCustomer.Gender = myReader.GetOrdinal("gender");
                    myCustomer.Nationality = myReader.GetString(myReader.GetOrdinal("nationality"));
                    myCustomer.Dob = myReader.GetDateTime(myReader.GetOrdinal("dob"));
                 
                    customerList.Add(myCustomer);
                }

            }
            myReader.Close();
            myConnection.Close();

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [getApplicationList] in class [da_FP_App]. Details: " + ex.Message);
        }

        return customerList;
    }
   
}