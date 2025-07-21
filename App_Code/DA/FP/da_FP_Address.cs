using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Collections;

/// <summary>
/// Summary description for da_FP_Address
/// </summary>
public class da_FP_Address
{
	public da_FP_Address()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //save address
    public static bool saveAddress(bl_FP_Address address)
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
            myCommand.CommandText = "SP_INSERT_FP_ADDRESS";
            var x = myCommand.Parameters;
            var y = address;
            x.AddWithValue("@Address_ID", y.Address_id);
            x.AddWithValue("@Address1", y.Address1);
            x.AddWithValue("@Address2", y.Address2);
            x.AddWithValue("@Customer_ID", y.Customer_id);
            x.AddWithValue("@Created_By", y.Created_by);
            x.AddWithValue("@Created_On", y.Created_on);
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        
        }catch(Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [saveAddress] in class [da_FP_Address], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    //update address
    public static bool updateAddress(bl_FP_Address address)
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
            myCommand.CommandText = "SP_UPDATE_FP_ADDRESS";
            var x = myCommand.Parameters;
            var y = address;
            x.AddWithValue("@Address_ID", y.Address_id);
            x.AddWithValue("@Address1", y.Address1);
            x.AddWithValue("@Address2", y.Address2);
            x.AddWithValue("@Customer_ID", y.Customer_id);
            x.AddWithValue("@Updated_By", y.Updated_by);
            x.AddWithValue("@Updated_On", y.Updated_on);
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [updateAddress] in class [da_FP_Address], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    //Delete address
    public static bool deleteAddress(string address_id, string customer_id)
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
            myCommand.CommandText = "SP_DELETE_FP_ADDRESS";
            var x = myCommand.Parameters;
        
            x.AddWithValue("@Address_ID", address_id);
            x.AddWithValue("@Customer_ID", customer_id);
         
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [deleteAddress] in class [da_FP_Address], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
}