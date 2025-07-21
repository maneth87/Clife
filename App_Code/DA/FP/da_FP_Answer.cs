using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for da_FP_Answer
/// </summary>
public class da_FP_Answer
{
	public da_FP_Answer()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bool saveAnswer(bl_FP_Answer answer) {
       bool result=false;

        try{
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();

            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_INSERT_FP_ANSWER";
            var x = myCommand.Parameters;
            var y = answer;
            x.AddWithValue("@Answer_ID", y.Answer_id);
            x.AddWithValue("@Question_ID", y.Question_id);
            x.AddWithValue("@App_ID", y.App_id);
            x.AddWithValue("@Answer", y.Answer);
            x.AddWithValue("@Customer_ID", y.Customer_id);
            x.AddWithValue("@Remarks", y.Remarks);
           
            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        }catch(Exception ex){
            Log.AddExceptionToLog("Error funtion [saveAnswer] in class [da_FP_Answer], Detail: " + ex.Message);
            result = false;
        }

        return result;
    }
    //save answer for sub
    public static bool saveAnswerSub(bl_FP_Answer answer)
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
            myCommand.CommandText = "SP_INSERT_FP_ANSWER_SUB";
            var x = myCommand.Parameters;
            var y = answer;
            x.AddWithValue("@Answer_ID", y.Answer_id);
            x.AddWithValue("@Question_ID", y.Question_id);
            x.AddWithValue("@App_ID", y.App_id);
            x.AddWithValue("@Answer", y.Answer);
            x.AddWithValue("@Customer_ID", y.Customer_id);
            x.AddWithValue("@Remarks", y.Remarks);

            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [saveAnswerSub] in class [da_FP_Answer], Detail: " + ex.Message);
            result = false;
        }

        return result;
    }
    public static bool updateAnswer(bl_FP_Answer answer)
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
            myCommand.CommandText = "SP_UPDATE_FP_ANSWER_BY_APP_ID";

            var x = myCommand.Parameters;
            var y = answer;
            x.AddWithValue("@App_ID", y.App_id);
            x.AddWithValue("@Answer", y.Answer);

            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [updateAnswer] in class [da_FP_Answer], Detail: " + ex.Message);
            result = false;
        }

        return result;
    }
    public static bool deleteAnswer(string app_id)
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
            myCommand.CommandText = "SP_DELETE_FP_ANSWER_BY_APP_ID";

            var x = myCommand.Parameters;
         
            x.AddWithValue("@App_ID", app_id);

            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [DeleteAnswer] in class [da_FP_Answer], Detail: " + ex.Message);
            result = false;
        }

        return result;
    }
    //delete answer sub
    public static bool deleteAnswerSub(string app_id)
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
            myCommand.CommandText = "SP_DELETE_FP_ANSWER_SUB_BY_APP_ID";

            var x = myCommand.Parameters;

            x.AddWithValue("@App_ID", app_id);

            myCommand.ExecuteNonQuery();
            myCommand.Parameters.Clear();
            myConnection.Close();
            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error funtion [DeleteAnswerSub] in class [da_FP_Answer], Detail: " + ex.Message);
            result = false;
        }

        return result;
    }
    public static List<bl_FP_Answer>getAnswerByAppID(string app_id) {
        List<bl_FP_Answer> myAnswerList = new List<bl_FP_Answer>();
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_GET_FP_ANSWER_BY_APP_ID";
            myCommand.Parameters.AddWithValue("@App_ID", app_id);
            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    bl_FP_Answer myAnswer = new bl_FP_Answer();
                    var a = myAnswer;

                    a.Answer_id = myReader.GetString(myReader.GetOrdinal("answer_id"));
                    a.App_id = myReader.GetString(myReader.GetOrdinal("app_id"));
                    a.Answer = myReader.GetString(myReader.GetOrdinal("answer"));
                    a.Question_id = myReader.GetString(myReader.GetOrdinal("question_id"));

                    myAnswerList.Add(myAnswer);
                }

            }
            myReader.Close();
            myConnection.Close();
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function [getAnswerByAppID] in class [da_FP_Answer], Detail: " + ex.Message);
        }

        return myAnswerList;
    }

}