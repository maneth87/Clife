using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for da_wing_account
/// </summary>
public class da_wing_account
{

    private static da_wing_account mytitle = null;

	public da_wing_account()
	{
          if (mytitle == null)
        {
            mytitle = new da_wing_account();
        }
    }

    #region "Public Function"

    //Insert new Wing Account
    public static bool InsertWingAccount(bl_wing_account wing)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Wing_Account";

            cmd.Parameters.AddWithValue("@Sk", wing.Sk);
            cmd.Parameters.AddWithValue("@Wing_Number",wing.Wing_Number);
            cmd.Parameters.AddWithValue("@Date_Request", wing.Date_Request);
            cmd.Parameters.AddWithValue("@Created_On", wing.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", wing.Created_By);
            cmd.Parameters.AddWithValue("@Status", wing.Status);

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
                Log.AddExceptionToLog("Error in function [InsertWingAccount] in class [da_wing_account]. Details: " + ex.Message);
            }
        }

        return result;

    }

    //Function to get all Wing Account
    public static List<bl_wing_account> GetAllWingAccount(string wing_sk, string wing_num)
    {
        List<bl_wing_account> Wing_list = new List<bl_wing_account>();

        string connString = AppConfiguration.GetConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_All_Wing_Account";
                cmd.Parameters.AddWithValue("@wing_sk", wing_sk);
                cmd.Parameters.AddWithValue("@wing_num", wing_num);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        bl_wing_account wing = new bl_wing_account();

                        wing.Sk = rdr.GetString(rdr.GetOrdinal("SK"));
                        wing.Wing_Number = rdr.GetString(rdr.GetOrdinal("Wing_Number"));
                        wing.Date_Request = rdr.GetDateTime(rdr.GetOrdinal("Date_Request"));
                        wing.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                        wing.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                        wing.Status = rdr.GetInt32(rdr.GetOrdinal("Status"));

                        Wing_list.Add(wing);
                      
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetAllWingAccount] in class [da_wing_account]. Details: " + ex.Message);
        }

        return Wing_list;

    }

    //Get wing sk
    public static string GetWingSkAccount(string wing_sk)
    {

        string sk = "";

        string connString = AppConfiguration.GetConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_Wing_SK_Account";
                cmd.Parameters.AddWithValue("@wing_sk", wing_sk);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        sk = rdr.GetString(rdr.GetOrdinal("SK"));

                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetWingSkAccount] in class [da_wing_account]. Details: " + ex.Message);
        }

        return sk;

    }

    //Get wing wing num
    public static string GetWingNumAccount(string wing_num)
    {

        string num = "";

        string connString = AppConfiguration.GetConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_Wing_Num_Account";
                cmd.Parameters.AddWithValue("@wing_num", wing_num);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        num = rdr.GetString(rdr.GetOrdinal("Wing_Number"));

                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetWingNumAccount] in class [da_wing_account]. Details: " + ex.Message);
        }

        return num;

    }
   
    // Function update wing sk
    public static bool UpdateWingSK(string wing_sk, string wing_num)
    {

        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Update_Wing_SK";

            cmd.Parameters.AddWithValue("@Wing_Sk",wing_sk);
            cmd.Parameters.AddWithValue("@Wing_Num",wing_num);
           
            cmd.Connection = con;
            con.Open();
            try
            {

                SqlDataReader rdr = cmd.ExecuteReader();
                result = true;
                con.Close();

            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateWingSK] in class [da_wing_account]. Details: " + ex.Message);
            }
            con.Close();
        }

        return result;
    }


    // Function update wing number
    public static bool UpdateWingNumber(string wing_sk, string wing_num)
    {

        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Update_Wing_Number";

            cmd.Parameters.AddWithValue("@Wing_Sk", wing_sk);
            cmd.Parameters.AddWithValue("@Wing_Num", wing_num);

            cmd.Connection = con;
            con.Open();
            try
            {

                SqlDataReader rdr = cmd.ExecuteReader();
                result = true;
                con.Close();

            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateWingNumber] in class [da_wing_account]. Details: " + ex.Message);
            }
            con.Close();
        }

        return result;
    }

    #endregion

}