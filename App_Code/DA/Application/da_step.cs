using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_step
/// </summary>
public class da_step
{
	private static da_step mytitle = null;
    //Constructor
    public da_step()
    {
        if (mytitle == null)
        {
            mytitle = new da_step();
        }

    }

    #region "Public Functions"

    //Insert Ct_App_Step_History
    public static bool InsertAppStepHistory(bl_app_step_history app_step_history)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Step_History";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_step_history.App_Register_ID);
            cmd.Parameters.AddWithValue("@Txn_ID", app_step_history.Txn_ID);
            cmd.Parameters.AddWithValue("@Step_ID", app_step_history.Step_ID);           
            cmd.Parameters.AddWithValue("@Created_By", app_step_history.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", app_step_history.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", app_step_history.Created_On);

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
                Log.AddExceptionToLog("Error in function [InsertAppStepHistory] in class [da_step]. Details: " + ex.Message);
            }
        }
        return result;
    }


    //Insert Ct_App_Step_Next
    public static bool InsertAppStepNext(bl_app_step_next app_step_Next)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Step_Next";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_step_Next.App_Register_ID);

            cmd.Parameters.AddWithValue("@Step_ID", app_step_Next.Step_ID);

            cmd.Parameters.AddWithValue("@Last_Updated", app_step_Next.Last_Updated);

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
                Log.AddExceptionToLog("Error in function [InsertAppStepNext] in class [da_step]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get app registered step id
    public static string GetAppRegisterdStepID()
    {
        string step_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Register_Step_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;                   

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    step_id = rdr.GetString(rdr.GetOrdinal("Step_ID"));

                }

            }
            con.Close();
        }
        return step_id;
    }
    
    
    //Function to get app underwriting step id
    public static string GetAppUnderwritingStepID()
    {
        string step_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Underwriting_Step_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;                   

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {
                    step_id = rdr.GetString(rdr.GetOrdinal("Step_ID"));

                }

            }
            con.Close();
        }
        return step_id;
    }
    #endregion
}