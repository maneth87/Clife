using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Collections;


/// <summary>
/// Summary description for da_FP_App
/// </summary>
public class da_FP_App
{
	public da_FP_App()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //insert
    public static bool insertFPApp(bl_FP_App app)
    { 
        bool result =false;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();

        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Insert_FP_App";
           
            //initialize parameters to store procedure
           var para = myCommand.Parameters;
           var value = app;

           para.AddWithValue("@App_ID", value.App_id);
           para.AddWithValue("@App_Number", value.App_number);
           para.AddWithValue("@Payment_Code", value.Payment_mode);
           para.AddWithValue("@Policy_Number", value.Policy_number);
           para.AddWithValue("@Entry_Date", value.Entry_date);
           para.AddWithValue("@Sale_Agent_ID", value.Sale_agent_id);
           para.AddWithValue("@Created_Note", value.Created_note);
           para.AddWithValue("@Created_By", value.Created_by);
           para.AddWithValue("@Created_On", value.Created_on);
           //para.AddWithValue("@Updated_By", value.Updated_by);
           //para.AddWithValue("@Updated_On", value.Updated_on);
           myCommand.ExecuteNonQuery();
           myCommand.Parameters.Clear();
           myConnection.Close();
           result = true;

        }catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function [insertFPApp], in class [da_FP_App]. Detail: " + ex.Message);
        }
        return result;
    }
    //update
    public static bool updateFPApp(bl_FP_App app)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();

        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Update_FP_App";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;
            var value = app;

            para.AddWithValue("@App_ID", value.App_id);
            para.AddWithValue("@App_Number", value.App_number);
            para.AddWithValue("@Payment_Code", value.Payment_mode);
            para.AddWithValue("@Policy_Number", value.Policy_number);
            para.AddWithValue("@Entry_Date", value.Entry_date);
            para.AddWithValue("@Sale_Agent_ID", value.Sale_agent_id);
            para.AddWithValue("@Created_Note", value.Created_note);
            para.AddWithValue("@Updated_By", value.Updated_by);
            para.AddWithValue("@Updated_On", value.Updated_on);
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [updateFPApp], in class [da_FP_App]. Detail: " + ex.Message);
        }
        return result;
    }
    //delete application by application ID
    public static bool deleteFPApp(string applicationNumber)
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
            myCommand.CommandText = "SP_DELETE_FP_APP_BY_APP_NUMBER";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;
            para.AddWithValue("@App_number", applicationNumber);

            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function [deleteFPApp] in class [da_FP_App]. Detail: " + ex.Message);
        }
        return result;
    }
    //get application by application number
    public static List<bl_FP_App> getApplicationList(string applicationNumber)
    {

        List<bl_FP_App> applicationList = new List<bl_FP_App>();
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();

        try
        {
             //Open connection
                
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_GET_FP_APP_LIST_BY_APP_NUMBER";
            myCommand.Parameters.AddWithValue("@App_number", applicationNumber);
            SqlDataReader myReader = myCommand.ExecuteReader();
            
            while (myReader.Read())
                 
            {
                if (myReader.HasRows)
                {
                    bl_FP_App myApp = new bl_FP_App();
                   myApp.App_id= myReader.GetString(myReader.GetOrdinal("app_id"));
                   myApp.App_number=myReader.GetString(myReader.GetOrdinal("app_number"));
                   myApp.Policy_number = myReader.GetString(myReader.GetOrdinal("policy_number"));
                   myApp.Entry_date=myReader.GetDateTime(myReader.GetOrdinal("entry_date"));
                   myApp.Sale_agent_id=myReader.GetOrdinal("sale_agent_id");
                   myApp.Created_note = myReader.GetString(myReader.GetOrdinal("created_note"));
                   myApp.Payment_mode=myReader.GetOrdinal("payment_code");
                   myApp.Created_by = myReader.GetString(myReader.GetOrdinal("created_by"));
                   myApp.Created_on=myReader.GetDateTime(myReader.GetOrdinal("created_on"));
                   applicationList.Add(myApp);
                }
                    
            }
            myReader.Close();
            myConnection.Close();
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [getApplicationList] in class [da_FP_App]. Details: " + ex.Message);
        }
       
        return applicationList;

    }
}