using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Collections;
using System.Web.Services;
/// <summary>
/// Summary description for da_FP_Premium
/// </summary>
public class da_FP_Premium
{
	public da_FP_Premium()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //premium rate for policy owner
    public static double getPremium(string productId, int gender, int age, int paymentMode) 
    {
        double prepium = 0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP_Premium_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;
          
            para.AddWithValue("@Age_Band",age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);

           SqlDataReader myReader = myCommand.ExecuteReader();

             while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                   prepium = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }
             //0	Single	1
             //1	Annually	1
             //2	Semi-Annual	0.54
             //3	Quarterly	0.27
             //4	Monthly	0.09
             switch (paymentMode)
            {
                 case 0:
                    prepium = prepium * 1;
                    break;
                 case 1:
                    prepium = prepium * 1;
                    break;
                 case 2:
                    prepium = prepium * 0.52;//not used 0.54
                    break;
                 case 3:
                    prepium = prepium * 0.27;
                    break;
                 case 4:
                    prepium = prepium * 0.09;
                    break;

            }

            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

            //round up
            prepium = System.Math.Round(prepium);
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getPremium] in class [da_FP_Premium], Detail: " + ex.Message);
        }
        return prepium;
    
    }
    // original premium for insured life 1 not calculated with factor and not round up or down
    public static double getOriginalPremium(string productId, int gender, int age)
    {
        double prepium = 0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP_Premium_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    prepium = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }
           
            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getOriginalPremium] in class [da_FP_Premium], Detail: " + ex.Message);
        }
        return prepium;

    }

   //premium rate for Insured life 2
    public static double getLifeInsuredPremium(string productId, int gender, int age, int paymentMode) 
    {
        double prepium = 0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP_LIFE_INSURED_Premium_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    prepium = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }
            //0	Single	1
            //1	Annually	1
            //2	Semi-Annual	0.54
            //3	Quarterly	0.27
            //4	Monthly	0.09
            switch (paymentMode)
            {
                case 0:
                    prepium = prepium * 1;
                    break;
                case 1:
                    prepium = prepium * 1;
                    break;
                case 2:
                    prepium = prepium * 0.52; //not used 0.54
                    break;
                case 3:
                    prepium = prepium * 0.27;
                    break;
                case 4:
                    prepium = prepium * 0.09;
                    break;

            }
            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();
            //round up
            prepium = System.Math.Round(prepium);
        }

        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getLifeInsuredPremium] in class [da_FP_Premium], Detail: " + ex.Message);
        }
        return prepium;
    
    }

    // original premium for insured life 2 not calculated with factor and not round up or down
    public static double getLifeInsuredOriginalPremium(string productId, int gender, int age)
    {
        double prepium = 0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP_LIFE_INSURED_Premium_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    prepium = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }
            
            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();
        }

        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getLifeInsuredOriginalPremium] in class [da_FP_Premium], Detail: " + ex.Message);
        }
        return prepium;

    }
}