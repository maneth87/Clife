using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Collections;

/// <summary>
/// Summary description for da_FP_Customer_Sub
/// </summary>
public class da_FP_Customer_Sub
{
	public da_FP_Customer_Sub()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //saveCustomerSub
    public static bool saveCustomerSub(bl_FP_Customer_sub customerSub) 
    {
        bool result=false;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();

            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_INSERT_FP_CUSTOMER_SUB";
            var x = myCommand.Parameters;
            var y = customerSub;

            x.AddWithValue("@App_ID", y.App_id);
            x.AddWithValue("@Sub_ID", y.Sub_id);
            x.AddWithValue("@Sub_Number", y.Sub_number);
            x.AddWithValue("@ID_Type", y.Id_type);
            x.AddWithValue("@ID_Number", y.Id_number);
            x.AddWithValue("@Surname_Kh", y.Surname_kh);
            x.AddWithValue("@Surname_En", y.Surname_en);
            x.AddWithValue("@Firstname_Kh", y.First_name_kh);
            x.AddWithValue("@Firstname_En", y.First_name_en);
            x.AddWithValue("@Gender", y.Gender);
            x.AddWithValue("@DOB", y.Dob);
            x.AddWithValue("@Relationship", y.Relationship);
            x.AddWithValue("@Created_by", y.Created_by);
            x.AddWithValue("@Created_on", y.Created_on);
                

            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result= true;
        }
        catch (Exception ex)
        {
        
            Log.AddExceptionToLog("Error function [saveCustomerSub] in class [da_FP_Customer_Sub]. Detail: " + ex.Message);
            result=false;
        }
        return result;
    }
    //delete customer sub by application id
    public static bool deleteCustomerSubByApplicationID(string applicationID)
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
            myCommand.CommandText = "SP_DELETE_FP_CUSTOMER_SUB_BY_APP_ID";

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
            Log.AddExceptionToLog("Error function [deleteCustomerSubByApplicationID] in class [da_FP_Customer_Sub]. Detail: " + ex.Message);
        }
        return result;
    }
    //get sub customer list
    public static List<bl_FP_Customer_sub> getSubCustomerList(string application_id) {
        List<bl_FP_Customer_sub> myCustomer = new List<bl_FP_Customer_sub>();
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        SqlDataReader myReader;
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_GET_FP_CUSTOMER_SUB_LIST_BY_APP_ID";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;
            para.AddWithValue("@App_ID", application_id);

            myReader= myCommand.ExecuteReader();
            while (myReader.Read()) {
                bl_FP_Customer_sub customer = new bl_FP_Customer_sub();
                customer.App_id = myReader.GetString(myReader.GetOrdinal("app_id"));
                customer.Sub_id = myReader.GetString(myReader.GetOrdinal("sub_id"));
                customer.Sub_number = myReader.GetInt32(myReader.GetOrdinal("sub_number"));
                customer.Id_type = myReader.GetInt32(myReader.GetOrdinal("id_type"));
                customer.Id_number = myReader.GetString(myReader.GetOrdinal("id_number"));
                customer.Surname_en = myReader.GetString(myReader.GetOrdinal("surnameEN"));
                customer.Surname_kh = myReader.GetString(myReader.GetOrdinal("surnameKH"));
                customer.First_name_en = myReader.GetString(myReader.GetOrdinal("firstnameEn"));
                customer.First_name_kh = myReader.GetString(myReader.GetOrdinal("firstnameKH"));
                customer.Gender = myReader.GetInt32(myReader.GetOrdinal("gender"));
                customer.Dob = myReader.GetDateTime(myReader.GetOrdinal("dob"));
                customer.Relationship = myReader.GetString(myReader.GetOrdinal("relationship"));
                myCustomer.Add(customer);
            }
            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();
           
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error fuction [getSubCustomerList] in class [da_FP_Customer_Sub], Detail: " + ex.Message);
        }
        return myCustomer;
    }
}