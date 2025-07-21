using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Collections;

/// <summary>
/// Summary description for da_FP_Contact
/// </summary>
public class da_FP_Contact
{
	public da_FP_Contact()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //save Contact
    public static bool saveContact(bl_FP_Contact contact)
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
            myCommand.CommandText = "SP_INSERT_FP_CONTACT";
            var x = myCommand.Parameters;
            var y = contact;

            x.AddWithValue("@Contact_ID", y.Contact_id);
            x.AddWithValue("@Customer_ID", y.Customer_id);
            x.AddWithValue("@Mobile_Phone1", y.Mobile_phone1);
            x.AddWithValue("@Mobile_Phone2", y.Mobile_phone2);
            x.AddWithValue("@Email", y.Email);
            x.AddWithValue("@Created_By", y.Created_by);
            x.AddWithValue("@Created_On", y.Created_on);
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [saveContact] in class [da_FP_Contact], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }

    //update Contact by customer id
    public static bool updateContact(bl_FP_Contact contact)
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
            myCommand.CommandText = "SP_UPDATE_FP_CONTACT";
            var x = myCommand.Parameters;
            var y = contact;

            x.AddWithValue("@Customer_ID", y.Customer_id);
            x.AddWithValue("@Mobile_Phone1", y.Mobile_phone1);
            x.AddWithValue("@Mobile_Phone2", y.Mobile_phone2);
            x.AddWithValue("@Email", y.Email);
            x.AddWithValue("@Updated_by", y.Updated_by);
            x.AddWithValue("@Updated_On", y.Updated_on);
         
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [saveContact] in class [da_FP_Contact], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    //delete Contact by customer_id
    public static bool deleteContact(string customer_id)
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
            myCommand.CommandText = "SP_DELETE_FP_CONTACT";
            var x = myCommand.Parameters;

            x.AddWithValue("@Customer_ID",customer_id);
          
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [deleteContact] in class [da_FP_Contact], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }

    //TODO get customer list
    public static List<bl_FP_Contact> getContactList(string customer_id) {
        List<bl_FP_Contact> myContactList= new List<bl_FP_Contact>();
        try
        {
            bl_FP_Contact contact;
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();
            SqlDataReader myReader;

            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_GET_FP_CONTACT_BY_CUSTOMER_ID";

            myReader = myCommand.ExecuteReader();
            while (myReader.Read()){
                contact = new bl_FP_Contact();
                contact.Mobile_phone1=myReader.GetString(myReader.GetOrdinal("mobile_phone1"));
                contact.Mobile_phone2 = myReader.GetString(myReader.GetOrdinal("mobile_phone2"));
                contact.Email = myReader.GetString(myReader.GetOrdinal("email"));
                contact.Contact_id = myReader.GetString(myReader.GetOrdinal("contact_id"));
                contact.Customer_id = myReader.GetString(myReader.GetOrdinal("customer_id"));
                myContactList.Add(contact);
            }
            myReader.Close();
            myCommand.Dispose();
            myConnection.Close();

        }
        catch(Exception ex) {
            Log.AddExceptionToLog("Error function [getContactList] in class [da_FP_Contact], Detail: " + ex.Message);
        }
        return myContactList;
    }
}