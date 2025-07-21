using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Web;

/// <summary>
/// Summary description for da_payment_code
/// </summary>
public class da_payment_code
{
    private static da_payment_code mytitle = null;
    public da_payment_code()
    {
        if (mytitle == null)
        {
            mytitle = new da_payment_code();
        }
    }

    #region "Public Functions"

    /// <summary>
    /// Insert into Quotation DB table quotation_payment_code
    /// </summary>
    public static bool InsertPaymentCode(bl_payment_code payment_code)
    {
        bool result = false;
        string connString = AppConfiguration.GetQuotationConnectionString();
        using (MySqlConnection con = new MySqlConnection(connString))
        {
            string sql = "Insert into quotation_payment_code (payment_code,quotation_date,quotation_number) values (@payment_code,@quotation_date,@quotation_number)";

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@payment_code", payment_code.payment_code);
            cmd.Parameters.AddWithValue("@quotation_date", payment_code.quotation_date);
            cmd.Parameters.AddWithValue("@quotation_number", payment_code.quotation_number);
         
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
                Log.AddExceptionToLog("Error in function [InsertPaymentCode] in class [da_payment_code]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Get new payment code
    public static string GetNewPaymentCode()
    {
        string payment_code = "";
        string front_digits = GetFrontDigits();

        do
        {
           
            Random random = new Random();
            string num = random.Next(1, 99999999).ToString();

            while (num.Length < 8)
            {
                num = "0" + num;
            }

            payment_code = front_digits + num;

        } while (CheckPaymentCode(payment_code));
        return payment_code;
    }

    //Get front digit payment code
    private static string GetFrontDigits()
    {

        string front_digits = "";
        string connString = AppConfiguration.GetQuotationConnectionString();
        try
        {
            using (MySqlConnection myConnection = new MySqlConnection(connString))
            {
               // string sql = "select code_number_for_fist_payment from code_number_for_first_payment where status = 1";
                //By maneth
                string sql = "select code_number_for_first_payment from code_number_for_first_payment where status = 1";
                MySqlCommand myCommand = new MySqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;

                using (MySqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        front_digits = myReader.GetString(myReader.GetOrdinal("code_number_for_first_payment"));

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
            Log.AddExceptionToLog("Error in function [GetFrontDigits] in class [da_payment_code]. Details: " + ex.Message);
        }
        return front_digits;

    }

    //Check payment code
    private static bool CheckPaymentCode(string payment_code)
    {

        bool result = false;
        string connString = AppConfiguration.GetQuotationConnectionString();
        try
        {
            using (MySqlConnection myConnection = new MySqlConnection(connString))
            {
                string sql = "select payment_code from quotation_payment_code where payment_code = @payment_code";

                MySqlCommand myCommand = new MySqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;
                myCommand.Parameters.AddWithValue("@payment_code", payment_code);

                using (MySqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        result = true;
                        break;

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
            Log.AddExceptionToLog("Error in function [CheckPaymentCode] in class [da_payment_code]. Details: " + ex.Message);
        }
        return result;

    }

   

    #endregion
}