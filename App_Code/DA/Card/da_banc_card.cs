using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_banc_card
/// </summary>
public class da_banc_card
{
    private static da_banc_card mytitle = null;
    public da_banc_card()
	{
        if (mytitle == null)
        {
            mytitle = new da_banc_card();
		}
	}

    //Add new banc card
    public static bool InsertBancCard(bl_banc_card banc_card)
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
                myCommand.CommandText = "SP_Insert_Banc_Card";

                //Bind parameter to value received from the function caller
                myCommand.Parameters.AddWithValue("@Card_ID", banc_card.Card_ID);
                myCommand.Parameters.AddWithValue("@Card_Number", banc_card.Card_Number);
                myCommand.Parameters.AddWithValue("@Sum_Insured", banc_card.Sum_Insured);
                myCommand.Parameters.AddWithValue("@Premium", banc_card.Premium);
                myCommand.Parameters.AddWithValue("@Status", banc_card.Status);
                myCommand.Parameters.AddWithValue("@Created_By", banc_card.Created_By);
                myCommand.Parameters.AddWithValue("@Created_On", banc_card.Created_On);
                myCommand.Parameters.AddWithValue("@Url", banc_card.Url);
                myCommand.Parameters.AddWithValue("@Product_ID", banc_card.Product_ID);
                myCommand.Parameters.AddWithValue("@Created_Note", "");

                //Execute the query
                myCommand.ExecuteNonQuery();

                //Close connection
                myConnection.Close();

                //Set result to true to track whether the function is successfully operated
                result = true;
            }

        }
        catch (Exception ex)
        {
            //Add error to log for analysis
            Log.AddExceptionToLog("Error function [InsertBancCard] in class [da_banc_card]. Details: " + ex.Message);
        }
        //Return result to the function caller
        //If <true> means the function has insertd new article successfully
        return result;
    }

    //Function to check existing barcode
    public static bool CheckExistingBarcode(string barcode)
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
                myCommand.CommandText = "SP_Check_Banc_Card_ID";
                myCommand.Parameters.AddWithValue("@Card_ID", barcode);

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
            Log.AddExceptionToLog("Error in function [CheckExistingBarcode] in class [da_banc_card]. Details: " + ex.Message);
        }
        return result;
    }

    //Function to get last card number
    public static string GetLastCardNumber(string product_id)
    {
        string card_number = "0";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Last_Banc_Card_Number";
                myCommand.Parameters.AddWithValue("@Product_ID", product_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        card_number = myReader.GetString(myReader.GetOrdinal("Card_Number"));

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
            Log.AddExceptionToLog("Error in function [GetLastCardNumber] in class [da_banc_card]. Details: " + ex.Message);
        }
        return card_number;
    }

    //Function to card list
    public static List<bl_banc_card> GetCardList(Double sum_insured)
    {
        List<bl_banc_card> card_list = new List<bl_banc_card>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Banc_Card_List";
                myCommand.Parameters.AddWithValue("@Sum_Insured", sum_insured);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_banc_card card = new bl_banc_card();

                        card.Card_ID = myReader.GetString(myReader.GetOrdinal("Card_ID"));
                        card.Card_Number = myReader.GetString(myReader.GetOrdinal("Card_Number"));

                        card_list.Add(card);
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
            Log.AddExceptionToLog("Error in function [GetCardList] in class [da_banc_card]. Details: " + ex.Message);
        }
        return card_list;
    }

    //Update Card Status
    public static bool UpdateCardStatus(string card_id, int status)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Banc_Card_Status";

            cmd.Parameters.AddWithValue("@Card_ID", card_id);
            cmd.Parameters.AddWithValue("@Status", status);
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
                con.Close();
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateCardStatus] in class [da_banc_card]. Details: " + ex.Message);
            }
        }
        return result;
    }


    //Function to get first available card
    public static bl_banc_card GetFirstAvailableCard(string product_id)
    {
        bl_banc_card card = new bl_banc_card();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Available_Banc_Card";
                myCommand.Parameters.AddWithValue("@Product_ID", product_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        card.Card_Number = myReader.GetString(myReader.GetOrdinal("Card_Number"));
                        card.Card_ID = myReader.GetString(myReader.GetOrdinal("Card_ID"));

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
            Log.AddExceptionToLog("Error in function [GetFirstAvailableCard] in class [da_banc_card]. Details: " + ex.Message);
        }
        return card;
    }


    //Function to get available card
    public static int GetAvailableCardCount(string product_id)
    {
        int cards = 0;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Available_Banc_Card_Count";
                myCommand.Parameters.AddWithValue("@Product_ID", product_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        cards = myReader.GetInt32(myReader.GetOrdinal("Card_Count"));

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
            Log.AddExceptionToLog("Error in function [GetAvailableCardCount] in class [da_banc_card]. Details: " + ex.Message);
        }
        return cards;
    }
}