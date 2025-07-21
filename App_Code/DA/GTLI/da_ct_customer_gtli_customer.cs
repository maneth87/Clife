using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_ct_customer_gtli_customer
/// </summary>
public class da_ct_customer_gtli_employee
{
	#region "Constructor"
    private static da_ct_customer_gtli_employee mytitle = null;
    public da_ct_customer_gtli_employee()
        {
            if (mytitle == null)
            {
                mytitle = new da_ct_customer_gtli_employee();
	        }
        }
    #endregion

    #region "Public Functions"
    //Insert new ct_customer_gtli_employee
    public static bool InsertCtCustomerGTLIEmployee(bl_ct_customer_gtli_employee ct_customer_gtli_employee)
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
                myCommand.CommandText = "SP_Insert_Ct_Customer_GTLI_Customer";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", ct_customer_gtli_employee.GTLI_Certificate_ID);
                myCommand.Parameters.AddWithValue("@Customer_ID", ct_customer_gtli_employee.Customer_ID);
              
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
            Log.AddExceptionToLog("Error function [InsertCtCustomerGTLIEmployee] in class [da_ct_customer_gtli_employee]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Function to delete Ct_Customer_GTLI_Employee
    public static bool DeleteCtCustomerGTLIEmployee(string gtli_cerificate_id)
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
                myCommand.CommandText = "SP_Delete_Ct_Customer_GTLI_Employee";
                myCommand.Parameters.AddWithValue("@GTLI_Certificate_ID", gtli_cerificate_id);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [DeleteCtCustomerGTLIEmployee] in class [da_ct_customer_gtli_employee]. Details: " + ex.Message);
        }
        return result;
    }
    #endregion
}