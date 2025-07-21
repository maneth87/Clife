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
/// Summary description for da_application_fp6
/// </summary>
public class da_application_fp6 : da_application
{
	public da_application_fp6()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //premium rate for policy owner /life assure
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sumInsured"></param>
    /// <param name="productId"></param>
    /// <param name="gender"></param>
    /// <param name="age"></param>
    /// <param name="paymentMode"></param>
    /// <param name="assure_year"></param>
    /// <returns></returns>
    public static double[,] getPremiumFP6(double sumInsured, string productId, int gender, int age, int paymentMode, int assure_year)
    {
        double premium = 0;
        double annual_premium = 0;
        double rate =0.0;
        double[,] arr_premium = new double[,]{{0,0}};

        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP6_Premium_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);
            para.AddWithValue("@Assure_Year", assure_year);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    rate = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }

            premium = (rate * sumInsured) / 1000;
            annual_premium = premium;
            //round up premium
            premium = Math.Ceiling(premium);

            //0	Single	1
            //1	Annually	1
            //2	Semi-Annual	0.54
            //3	Quarterly	0.27
            //4	Monthly	0.09

            switch (paymentMode)
            {
                case 0:
                    premium = premium * 1;
                    break;
                case 1:
                    premium = premium * 1;
                    break;
                case 2:
                    premium = premium * 0.52;//not used 0.54
                    break;
                case 3:
                    premium = premium * 0.27;
                    break;
                case 4:
                    premium = premium * 0.09;
                    break;

            }

            //Discount base on Sur Assured
            if (sumInsured > 25000) 
            {//discount 0% 
                
            }
            else if (sumInsured >= 25000 && sumInsured < 50000)
            { //discount 5%
                premium = premium - (premium * 5) / 100;
            }
            else if (sumInsured >= 50000 && sumInsured < 100000)
            { //discount 7%
                premium = premium - (premium * 7) / 100;
            }
            else if(sumInsured>=100000)
            { //discount 10%
                premium = premium - (premium * 10) / 100;
            }

            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

            //round up
            //premium = System.Math.Round(premium);
            
            //roundup after discount
            premium = Math.Ceiling(premium);

            arr_premium = new double[,] { { premium, annual_premium } };

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getPremiumFP6] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return arr_premium;

    }
    #region//Premium Family Protection Package
    /// <summary>
    /// 
    /// </summary>
    /// <param name="product_id"></param>
    /// <param name="assure_year"></param>
    /// <param name="gender"></param>
    /// <param name="age">will be set to zero by automaticaly, because this column age in database store 0</param>
    /// <returns></returns>
    public static double GetPremiumFPPackage(string product_id, int assure_year, int gender, int age)
    {
        double premuim = 0;
        try
        {
            //product package set age to zero
            age = 0;

            string connString = AppConfiguration.GetConnectionString();
            //DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FPPackage_Premium", new string[,] { { "@Product_ID", product_id },
            //                                                                                            {"@Gender", gender+""}, 
            //                                                                                            {"@Assure_Year",assure_year +""},
            //                                                                                            {"@AGE",age +""}});

            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FP6_Premium_Rate", new string[,] { { "@Product_ID", product_id },
                                                                                                        {"@Gender", gender+""}, 
                                                                                                        {"@Assure_Year",assure_year +""},
                                                                                                        {"@Age_Band",age +""}});
            if (tbl.Rows.Count > 0)
            {
                premuim = Convert.ToDouble(tbl.Rows[0]["Rate"].ToString());//rate is the real premium
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPremiumFPPackage] in class [da_application_fp6], Detail: " + ex.Message);
            premuim = 0;
        }
        return premuim;

    }

    //TPD Premium for family protection package
    /// <summary>
    /// TPD Premium for family protection package 
    /// *Note: Age=0
    /// </summary>
    /// <param name="product_id"></param>
    /// <param name="assure_year"></param>
    /// <param name="gender"></param>
    /// <param name="age">will set to zero automaticaly, because age in database store as 0</param>
    /// <returns></returns>
    public static double GetTPDPremiumFPPackage(string product_id, int assure_year, int gender, int age)
    {
       
        double premuim = 0;
        //set age to zero
        age = 0;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            //DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FPPackage_TPD_Premium", new string[,] { { "@Product_ID", product_id },
            //                                                                                            {"@Gender", gender+""}, 
            //                                                                                            {"@Assure_Year",assure_year +""},
            //                                                                                            {"@AGE",age +""}});

            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FP6_TPD_Premium_Rate", new string[,] { { "@Product_ID", product_id },
                                                                                                        {"@Gender", gender+""}, 
                                                                                                        {"@Assure_Year",assure_year +""},
                                                                                                        {"@Age_Band",age +""}});
            if (tbl.Rows.Count > 0)
            {
                premuim = Convert.ToDouble(tbl.Rows[0]["Rate"].ToString());//rate is the real premium
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetTPDPremiumFPPackage] in class [da_application_fp6], Detail: " + ex.Message);
            premuim = 0;
        }
        return premuim;

    }
    //ADB Premium for family protection package
    /// <summary>
    /// ADB Premium for family protection package.
    /// ex: str_class = Class 5/5
    /// </summary>
    /// <param name="str_class"></param>
    /// <returns></returns>
    public static double GetADBPremiumFPPackage(string str_class)
    {
        double premuim = 0;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FPPackage_ADB_Premium", new string[,] { { "@Class", str_class }});
            if (tbl.Rows.Count > 0)
            {
                premuim = Convert.ToDouble(tbl.Rows[0]["Rate"].ToString());//rate is the real premium
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetADBPremiumFPPackage] in class [da_application_fp6], Detail: " + ex.Message);
            premuim = 0;
        }
        return premuim;

    }
    #endregion
    //TPD premium rate for policy owner
    /// <summary>
    /// TPD premium rate for Life insure
    /// Note: productId =""
    /// </summary>
    /// <param name="sumInsured"></param>
    /// <param name="productId"></param>
    /// <param name="gender"></param>
    /// <param name="age"></param>
    /// <param name="assure_year"></param>
    /// <param name="paymentMode"></param>
    /// <returns></returns>
    public static string GetTPDPremium(double sumInsured, string productId, int gender, int age, int assure_year, int paymentMode)
    {
        double premium = 0;
        double original_amount = 0.0;
        double rate = 0.0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP6_TPD_Premium_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);
            para.AddWithValue("@Assure_Year", assure_year);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    rate = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }

            premium = (rate * sumInsured) / 1000;
            original_amount = premium;
            //Round up premium before calculating with payment mode
            premium = Math.Ceiling(premium);

            //0	Single	1
            //1	Annually	1
            //2	Semi-Annual	0.54
            //3	Quarterly	0.27
            //4	Monthly	0.09

            switch (paymentMode)
            {
                case 0:
                    premium = premium * 1;
                    break;
                case 1:
                    premium = premium * 1;
                    break;
                case 2:
                    premium = premium * 0.52;//not used 0.54
                    break;
                case 3:
                    premium = premium * 0.27;
                    break;
                case 4:
                    premium = premium * 0.09;
                    break;

            }

            //Discount base on Sur Assured
            if (sumInsured > 25000)
            {//discount 0% 

            }
            else if (sumInsured >= 25000 && sumInsured < 50000)
            { //discount 5%
                premium = premium - (premium * 5) / 100;
            }
            else if (sumInsured >= 50000 && sumInsured < 100000)
            { //discount 7%
                premium = premium - (premium * 7) / 100;
            }
            else if (sumInsured >= 100000)
            { //discount 10%
                premium = premium - (premium * 10) / 100;
            }

            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

            //final round up
            premium = System.Math.Ceiling(premium);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetTPDPremium] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return premium + "/" + original_amount;

    }
    public static string GetADBPremium(double sumInsured, string productId, double rate, int paymentMode)
    {
        double premium = 0;
        double original_amount = 0.0;
       
        try
        {
            premium = (rate * sumInsured) / 1000;
            original_amount = premium;

            //round up premium before calculating with payment mode
            premium = Math.Ceiling(premium);

            //0	Single	1
            //1	Annually	1
            //2	Semi-Annual	0.54
            //3	Quarterly	0.27
            //4	Monthly	0.09

            switch (paymentMode)
            {
                case 0:
                    premium = premium * 1;
                    break;
                case 1:
                    premium = premium * 1;
                    break;
                case 2:
                    premium = premium * 0.52;//not used 0.54
                    break;
                case 3:
                    premium = premium * 0.27;
                    break;
                case 4:
                    premium = premium * 0.09;
                    break;

            }

            //Discount base on Sur Assured
            if (sumInsured > 25000)
            {//discount 0% 

            }
            else if (sumInsured >= 25000 && sumInsured < 50000)
            { //discount 5%
                premium = premium - (premium * 5) / 100;
            }
            else if (sumInsured >= 50000 && sumInsured < 100000)
            { //discount 7%
                premium = premium - (premium * 7) / 100;
            }
            else if (sumInsured >= 100000)
            { //discount 10%
                premium = premium - (premium * 10) / 100;
            }


            //final round up
            premium = System.Math.Ceiling(premium);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetADBPremium] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return premium + "/" + original_amount;

    }
    //TODO : Get ADB Category 26092016
    public static ArrayList GetADBCategories()
    {
        ArrayList arrList = new ArrayList();
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_ADB_Categories";

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    arrList.Add(myReader["categories"].ToString());
                }

            }
            myReader.Close();
            myCommand.Dispose();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetADBCategories] in class [da_application_fp6], Detail: " + ex.Message);
        }

        return arrList;
    }
    //TODO : Get Position base on Category 28092016
    public static ArrayList GetPosition(string category)
    {
        ArrayList arrList = new ArrayList();
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_ADB_Position_By_Categories";
            myCommand.Parameters.AddWithValue("@Category", category);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    arrList.Add(myReader["position"].ToString() + "," + myReader["no"].ToString());
                   
                }

            }
            myReader.Close();
            myCommand.Dispose();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPosition] in class [da_application_fp6], Detail: " + ex.Message);
        }

        return arrList;
    }
    //TODO: Get ADB Rate base on occupation 28092016
    //ADBRate is just the class range
    public static double GetADBRate(string adbCategory, string occupation)
    {
        double rate = 0.0;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_ADB_Rate";
            myCommand.Parameters.AddWithValue("@Category", adbCategory);
            myCommand.Parameters.AddWithValue("@Position", occupation);

            SqlDataReader myReader = myCommand.ExecuteReader();

            if (myReader.Read())
            {
                if (myReader.HasRows)
                {
                   rate= Convert.ToDouble(myReader["Rate"].ToString());
                }

            }
            myReader.Close();
            myCommand.Dispose();
            myConnection.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetADBRate] in class [da_application_fp6, Detail: " + ex.Message);
            rate = 0.0; 
        }
        return rate;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app_register_id"></param>
    /// <param name="rate_id"></param>
    /// <returns></returns>
    public static int GetADBRate(string app_register_id)
    {
        int rate = 0;
        try
        {

            DataTable tbl = DataSetGenerator.Get_Data_Soure("select ct_ADB_Rate.rate from Ct_App_Rider INNER join ct_ADB_Rate ON Ct_App_Rider.Rate_ID = ct_ADB_Rate.no WHERE Ct_App_Rider.Rider_Type='ADB' AND Ct_App_Rider.App_Register_ID='" + app_register_id + "'");

            foreach (DataRow row in tbl.Rows)
            {
                rate = Convert.ToInt32(row["Rate"].ToString());
            }
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetADBRate] in class [da_application_fp6, Detail: " + ex.Message);
            rate = 0;
        }
        return rate;
    }
    //TODO: Get Class Rate base on Class 07112016
    public static double GetClassRate(string class_name)
    {
        double class_rate = 0.0;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection myConnection = new SqlConnection(connString);
            SqlCommand myCommand = new SqlCommand();
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_ADB_Class_Rate";
            myCommand.Parameters.AddWithValue("@Class", class_name);

            SqlDataReader myReader = myCommand.ExecuteReader();

            if (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    class_rate = Convert.ToDouble(myReader["Rate"].ToString());
                }

            }
            myReader.Close();
            myCommand.Dispose();
            myConnection.Close();

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetClassRate] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return class_rate;
        
    }
    //TODO: Get ADB Cagetory by rate ID 04102016
    public static string GetCategoryByNo(int no)
    {
        string cate = "";
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        SqlDataReader myReader;

        try
        {
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_ADB_Category_By_No";
            myCommand.Parameters.AddWithValue("@No", no);

            myReader = myCommand.ExecuteReader();
            if (myReader.Read())
            {
                cate = myReader["categories"].ToString();
            }
            else
            {
                cate = "";
            }
            myReader.Close();

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetCategoryByNo] in class [da_application_fp6], Detail: " + ex.Message);
            cate = "";
        }
        finally
        { 
            //Close connection
          
            myCommand.Parameters.Clear();
            myCommand.Dispose();
            myConnection.Close();
           
        }
        return cate;
    }

    //premium rate for policy owner
    public static double getAnnualPremiumFP6(double sumInsured, string productId, int gender, int age, int assure_year)
    {
        double premium = 0;
        double rate = 0.0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP6_Premium_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);
            para.AddWithValue("@Assure_Year", assure_year);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    rate = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }

            premium = (rate * sumInsured) / 1000;

            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

            //round up
          //  premium = System.Math.Round(premium);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getAnnualPremiumFP6] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return premium;

    }
    //premium rate for Life insured
    public static double[,] getPremiumFP6Sub(double sumInsured, string productId, int gender, int age, int paymentMode, int assuredYear, int life_insured_type)
    {
        double premium =0.0;
        double annual_premium = 0.0;
        double[,] arr_premium = new double[,] { { 0, 0 } };
        double rate = 0.0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP6_Premium_Sub_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);
            para.AddWithValue("@Assure_Year", assuredYear);
                
            para.AddWithValue("@Life_Insured_Type", life_insured_type);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    rate = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }

            annual_premium = (rate * sumInsured) / 1000;

            premium = Math.Ceiling(annual_premium);

            //0	Single	1
            //1	Annually	1
            //2	Semi-Annual	0.54
            //3	Quarterly	0.27
            //4	Monthly	0.09
            switch (paymentMode)
            {
                case 0:
                    premium = premium * 1;
                    break;
                case 1:
                    premium = premium * 1;
                    break;
                case 2:
                    premium = premium * 0.52;//not used 0.54
                    break;
                case 3:
                    premium = premium * 0.27;
                    break;
                case 4:
                    premium = premium * 0.09;
                    break;

            }

            //Discount base on Sur Assured
            if (sumInsured > 25000)
            {//discount 0% 

            }
            else if (sumInsured >= 25000 && sumInsured < 50000)
            { //discount 5%
                premium = premium - (premium * 5) / 100;
            }
            else if (sumInsured >= 50000 && sumInsured < 100000)
            { //discount 7%
                premium = premium - (premium * 7) / 100;
            }
            else if (sumInsured>=100000)
            { //discount 10%
                premium = premium - (premium * 10) / 100;
            }

            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

            //round up
           // premium = System.Math.Round(premium, 2);
            premium = Math.Ceiling(premium);

            arr_premium = new double[,] { { premium, annual_premium } };
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getPremiumFP6Sub] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return arr_premium;

    }
    /// <summary>
    /// *Note: age=0
    ///        life_insured_type: spouse = 0, kid =1
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="gender"></param>
    /// <param name="age"></param>
    /// <param name="assuredYear"></param>
    /// <param name="life_insured_type"></param>
    /// <returns></returns>
    public static double getPremiumSpouseKidsFamilyProtectionPackage(string productId, int gender, int age, int assuredYear, int life_insured_type)
    {
       
        double rate = 0.0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FPP_Premium_Rider_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);
            para.AddWithValue("@Assure_Year", assuredYear);

            para.AddWithValue("@Life_Insured_Type", life_insured_type);

            SqlDataReader myReader = myCommand.ExecuteReader();

            if (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    rate = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getPremiumFP6Sub] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return rate;

    }
    public static double getAnnualPremiumFP6Sub(double sumInsured, string productId, int gender, int age, int assure_year, int life_insured_type)
    {
        double premium = 0;
        double rate = 0.0;
        string connString = AppConfiguration.GetConnectionString();
        SqlConnection myConnection = new SqlConnection(connString);
        SqlCommand myCommand = new SqlCommand();
        try
        {
            //Open connection
            myConnection.Open();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "SP_Get_FP6_Premium_Sub_Rate";

            //initialize parameters to store procedure
            var para = myCommand.Parameters;

            para.AddWithValue("@Age_Band", age);
            para.AddWithValue("@Gender", gender);
            para.AddWithValue("@Product_ID", productId);
            para.AddWithValue("@Life_Insured_Type", life_insured_type);
            para.AddWithValue("@Assure_Year", assure_year);

            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                if (myReader.HasRows)
                {
                    rate = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }

            }

            premium = (rate * sumInsured) / 1000;

            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

            //round up
          //premium = System.Math.Round(premium);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getAnnualPremiumFP6Sub] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return premium;

    }
    // original premium for insured life 1 not calculated with factor and not round up or down
    public static double getOriginalPremium(double sumInsured, string productId, int gender, int age)
    {
        double premium = 0;
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
                    premium = myReader.GetDouble(myReader.GetOrdinal("rate"));
                }
            }

            premium = (premium * sumInsured) / 1000;

            myReader.Close();
            myCommand.Parameters.Clear();
            myConnection.Close();

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getOriginalPremium] in class [da_FP_Premium], Detail: " + ex.Message);
        }
        return premium;

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

            prepium = Math.Ceiling(prepium);
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
            prepium = System.Math.Ceiling(prepium);
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
    //format application number: A+8 degits
    public static string FormatApplicationNumber(string applicationNumber){
    
        string newAppNumber="";
        
        for (int i = 1; i <= 8-applicationNumber.Trim().Length; i++) {
            newAppNumber = newAppNumber + "0";
        }

        return "A"+newAppNumber+applicationNumber;
    }
    //Insert Ct_App_Info_Person_Sub
    public static bool InsertAppInfoPersonSub(bl_app_info_person_sub app_info_person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Person_Sub";

            cmd.Parameters.AddWithValue("@Person_ID", app_info_person.Person_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_person.App_Register_ID);
            cmd.Parameters.AddWithValue("@Level", app_info_person.Level);
            cmd.Parameters.AddWithValue("@ID_Card", app_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", app_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", app_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Last_Name", app_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", app_info_person.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", app_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_person.Country_ID);

            cmd.Parameters.AddWithValue("@Khmer_First_Name", app_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", app_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", app_info_person.Father_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Father_Last_Name", app_info_person.Father_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_First_Name", app_info_person.Mother_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_Last_Name", app_info_person.Mother_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_First_Name", app_info_person.Prior_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_Last_Name", app_info_person.Prior_Last_Name.ToUpper());

            cmd.Parameters.AddWithValue("@Marital_Status", app_info_person.Marital_Status);
            cmd.Parameters.AddWithValue("@Relationship", app_info_person.Relationship);

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
                Log.AddExceptionToLog("Error in function [InsertAppInfoPersonSub] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Update Ct_App_Info_Person_Sub
    public static bool UpdateAppInfoPersonSub(bl_app_info_person_sub app_info_person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Person_Sub";

            cmd.Parameters.AddWithValue("@Person_ID",app_info_person.Person_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_person.App_Register_ID);
            cmd.Parameters.AddWithValue("@ID_Card", app_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", app_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", app_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Last_Name", app_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", app_info_person.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", app_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_person.Country_ID);

            cmd.Parameters.AddWithValue("@Khmer_First_Name", app_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", app_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", app_info_person.Father_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Father_Last_Name", app_info_person.Father_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_First_Name", app_info_person.Mother_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_Last_Name", app_info_person.Mother_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_First_Name", app_info_person.Prior_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_Last_Name", app_info_person.Prior_Last_Name.ToUpper());

            cmd.Parameters.AddWithValue("@Level", app_info_person.Level);
            cmd.Parameters.AddWithValue("@Marital_Status", app_info_person.Marital_Status);
            cmd.Parameters.AddWithValue("@Relationship", app_info_person.Relationship);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoPersonSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Save body sub
    public static bool InsertAppInfoBodySub(bl_app_info_body_sub app_info_body)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Body_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_body.App_Register_ID);
            cmd.Parameters.AddWithValue("@Height", app_info_body.Height);
            cmd.Parameters.AddWithValue("@Weight", app_info_body.Weight);
            cmd.Parameters.AddWithValue("@Is_Weight_Changed", app_info_body.Is_Weight_Changed);
            cmd.Parameters.AddWithValue("@Weight_Change", app_info_body.Weight_Change);
            cmd.Parameters.AddWithValue("@Reason", app_info_body.Reason);
            cmd.Parameters.AddWithValue("@Level", app_info_body.Level);
            cmd.Parameters.AddWithValue("@Id", app_info_body.Id);

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
                Log.AddExceptionToLog("Error in function [InsertAppInfoBodySub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //get body
    public static List<bl_app_info_body> GetAppInfoBody(string appID)
    {
       
        string connString = AppConfiguration.GetConnectionString();
        List<bl_app_info_body> myList = new List<bl_app_info_body>();
        bl_app_info_body body;
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_App_Info_Body";

            cmd.Parameters.AddWithValue("@App_Register_ID", appID);
            cmd.Connection = con;
            con.Open();

            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read()) {
                    body = new bl_app_info_body();
                    body.App_Register_ID = dr.GetString(dr.GetOrdinal("app_register_id"));
                    body.Height = dr.GetInt32(dr.GetOrdinal("height"));
                    body.Weight = dr.GetInt32(dr.GetOrdinal("weight"));
                    body.Weight_Change = dr.GetInt32(dr.GetOrdinal("weight_change"));
                    body.Is_Weight_Changed = dr.GetInt32(dr.GetOrdinal("is_weight_changed"));

                    if (!DBNull.Value.Equals(dr.GetString(dr.GetOrdinal("Reason"))))
                    {
                        body.Reason = dr.GetString(dr.GetOrdinal("Reason"));
                    }
                    else
                    {
                        body.Reason = "";
                    }
                    //body.Reason = dr["Reason"].ToString();
                    myList.Add(body);
                
                }

                dr.Close();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [GetAppInfoBody] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return myList;
    }

    //get body sub
    public static List<bl_app_info_body_sub> GetAppInfoBodySub(string appID)
    {

        string connString = AppConfiguration.GetConnectionString();
        List<bl_app_info_body_sub> myList = new List<bl_app_info_body_sub>();
        bl_app_info_body_sub body;
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_App_Info_Body_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", appID);
            cmd.Connection = con;
            con.Open();

            try
            {
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    body = new bl_app_info_body_sub();
                    body.App_Register_ID = dr.GetString(dr.GetOrdinal("app_register_id"));
                    body.Height = dr.GetInt32(dr.GetOrdinal("height"));
                    body.Weight = dr.GetInt32(dr.GetOrdinal("weight"));
                    body.Weight_Change = dr.GetInt32(dr.GetOrdinal("weight_change"));
                    body.Is_Weight_Changed = dr.GetInt32(dr.GetOrdinal("is_weight_changed"));

                    //if (!DBNull.Value.Equals(dr.GetString(dr.GetOrdinal("Reason"))))
                    //{
                    //    body.Reason = dr.GetString(dr.GetOrdinal("Reason"));
                    //}
                    //else
                    //{
                    //    body.Reason = "";
                    //}

                    if (!DBNull.Value.Equals(dr["Reason"]))
                    {
                        body.Reason = dr.GetString(dr.GetOrdinal("Reason"));
                    }
                    else
                    {
                        body.Reason = "";
                    }

                    body.Level = dr.GetInt32(dr.GetOrdinal("level"));
                    body.Id = dr.GetString(dr.GetOrdinal("id"));
                    myList.Add(body);

                }

                dr.Close();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [GetAppInfoBodySub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return myList;
    }

    //Insert Ct_App_Info_Address_sub
    public static bool InsertAppInfoAddressSub(bl_app_info_address app_info_address)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Address_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_address.App_Register_ID);
            cmd.Parameters.AddWithValue("@Address1", app_info_address.Address1);
            cmd.Parameters.AddWithValue("@Address2", app_info_address.Address2);
            cmd.Parameters.AddWithValue("@Address3", app_info_address.Address3);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_address.Country_ID);
            cmd.Parameters.AddWithValue("@Province", app_info_address.Province);
            cmd.Parameters.AddWithValue("@Zip_Code", app_info_address.Zip_Code);

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
                Log.AddExceptionToLog("Error in function [InsertAppInfoAddressSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Update Ct_App_Info_Address_Sub
    public static bool UpdateAppInfoAddressSub(bl_app_info_address app_info_address)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Address_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_address.App_Register_ID);
            cmd.Parameters.AddWithValue("@Address1", app_info_address.Address1);
            cmd.Parameters.AddWithValue("@Address2", app_info_address.Address2);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_address.Country_ID);
            cmd.Parameters.AddWithValue("@Province", app_info_address.Province);
            cmd.Parameters.AddWithValue("@Zip_Code", app_info_address.Zip_Code);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoAddressSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Update Ct_App_Info_body_sub
    public static bool UpdateAppInfoBodySub(da_application_fp6.bl_app_info_body_sub body)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Body_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", body.App_Register_ID);
            cmd.Parameters.AddWithValue("@Weight", body.Weight);
            cmd.Parameters.AddWithValue("@Height", body.Height);
            cmd.Parameters.AddWithValue("@Is_Weight_Changed", body.Is_Weight_Changed);
            cmd.Parameters.AddWithValue("@Reason",body.Reason );
            cmd.Parameters.AddWithValue("@Level", body.Level);
            cmd.Parameters.AddWithValue("@Id", body.Id);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfBodySub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Insert Ct_App_Job_History
    public static bool InsertAppJobHistorySub(bl_app_job_history_sub app_job_history)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Job_History_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_job_history.App_Register_ID);
            cmd.Parameters.AddWithValue("@Anual_Income", app_job_history.Anual_Income);
            cmd.Parameters.AddWithValue("@Current_Position", app_job_history.Current_Position);
            cmd.Parameters.AddWithValue("@Employer_Name", app_job_history.Employer_Name);
            cmd.Parameters.AddWithValue("@Job_Role", app_job_history.Job_Role);
            cmd.Parameters.AddWithValue("@Nature_Of_Business", app_job_history.Nature_Of_Business);
            cmd.Parameters.AddWithValue("@Level", app_job_history.Level);
            cmd.Parameters.AddWithValue("@Job_ID", app_job_history.Job_ID);
            cmd.Parameters.AddWithValue("@Address", app_job_history.Address);

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
                Log.AddExceptionToLog("Error in function [InsertAppJobHistorySub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Update Ct_App_Job_History_sub
    public static bool UpdateAppJobHistorySub(da_application_fp6.bl_app_job_history_sub app_job_history)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Job_History_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_job_history.App_Register_ID);
            cmd.Parameters.AddWithValue("@Anual_Income", app_job_history.Anual_Income);
            cmd.Parameters.AddWithValue("@Current_Position", app_job_history.Current_Position);
            cmd.Parameters.AddWithValue("@Employer_Name", app_job_history.Employer_Name);
            cmd.Parameters.AddWithValue("@Job_Role", app_job_history.Job_Role);
            cmd.Parameters.AddWithValue("@Nature_Of_Business", app_job_history.Nature_Of_Business);
            cmd.Parameters.AddWithValue("@Level", app_job_history.Level);
            cmd.Parameters.AddWithValue("@Job_ID",app_job_history.Job_ID);
            cmd.Parameters.AddWithValue("@Address", app_job_history.Address);

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
                Log.AddExceptionToLog("Error in function [UpdateAppJobHistorySub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Insert Ct_App_Life_Product
    public static bool InsertAppLifeProductSub(bl_app_life_product_Sub app_life_product)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Life_Product_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_life_product.App_Register_ID);
            cmd.Parameters.AddWithValue("@Age_Insure", app_life_product.Age_Insure);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", app_life_product.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", app_life_product.Assure_Year);
            cmd.Parameters.AddWithValue("@Pay_Mode", app_life_product.Pay_Mode);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", app_life_product.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Pay_Year", app_life_product.Pay_Year);
            cmd.Parameters.AddWithValue("@Product_ID", app_life_product.Product_ID);
            cmd.Parameters.AddWithValue("@System_Premium", app_life_product.System_Premium);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", app_life_product.System_Sum_Insure);
            cmd.Parameters.AddWithValue("@Level", app_life_product.Level);
            cmd.Parameters.AddWithValue("@Effective_Date", app_life_product.EffectiveDate);
            cmd.Parameters.AddWithValue("@Created_On", app_life_product.Created_On);
            cmd.Parameters.AddWithValue("@Action", app_life_product.Action);

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
                Log.AddExceptionToLog("Error in function [InsertAppLifeProductSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Update Ct_App_Life_Product
    public static bool UpdateAppLifeProductSub(bl_app_life_product_Sub app_life_product)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Life_Product_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_life_product.App_Register_ID);
            cmd.Parameters.AddWithValue("@Age_Insure", app_life_product.Age_Insure);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", app_life_product.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", app_life_product.Assure_Year);
            cmd.Parameters.AddWithValue("@Pay_Mode", app_life_product.Pay_Mode);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", app_life_product.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Pay_Year", app_life_product.Pay_Year);
            cmd.Parameters.AddWithValue("@Product_ID", app_life_product.Product_ID);
            cmd.Parameters.AddWithValue("@System_Premium", app_life_product.System_Premium);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", app_life_product.System_Sum_Insure);
            cmd.Parameters.AddWithValue("@Level", app_life_product.Level);
            cmd.Parameters.AddWithValue("@Effective_Date", app_life_product.EffectiveDate);

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
                Log.AddExceptionToLog("Error in function [UpdateAppLifeProductSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete life product sub by app_id and level
    public static bool DeleteAppLifeProductSubByAppIDLevel(string app_id, int level)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            result = Helper.ExecuteProcedure(connString, "SP_Delete_App_Life_Product_Sub_By_App_ID_Level", new string[,] { { " @App_Register_ID", app_id }, { "@Level", level + "" } }, "da_application_fp6 => DeleteAppLifeProductSubByAppIDLevel");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteAppLifeProductSubByAppIDLevel] class [da_application_fp6], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }

    //Count record of Life product sub
    public static int GetProductLifeSubRecords(string appID, int level)
    {
        int record = 0;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_App_Life_Product_Sub_Records";
            cmd.Parameters.AddWithValue("@App_Register_ID", appID);
            cmd.Parameters.AddWithValue("@Level",level);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                record = dr.GetInt32(dr.GetOrdinal("records"));
            }
            dr.Close();
            cmd.Dispose();
            cmd.Parameters.Clear();
            con.Close();


        }
        catch (Exception ex)
        {
           
            Log.AddExceptionToLog("Error function GetProductLifeSubRecords in class da_application_fp6. Detail: " + ex.Message);
            record = 0;
        }
        return record;
    }
    public static int GetPremiumPayRecords(string appID, int level)
    {
        int record = 0;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_App_Premium_Pay_Records";
            cmd.Parameters.AddWithValue("@App_Register_ID", appID);
            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                record = dr.GetInt32(dr.GetOrdinal("records"));
            }
            dr.Close();
            cmd.Dispose();
            cmd.Parameters.Clear();
            con.Close();


        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error function GetPremiumPayRecords in class da_application_fp6. Detail: " + ex.Message);
            record = 0;
        }
        return record;
    }
    //Insert Ct_App_Answer_Item
    public static bool InsertAppAnswerItemFp6(bl_app_answer_item_fp6 app_answer_item)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Answer_Item_fp6";

            cmd.Parameters.AddWithValue("@App_Answer_Item_ID", app_answer_item.App_Answer_Item_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_answer_item.App_Register_ID);
            cmd.Parameters.AddWithValue("@Question_ID", app_answer_item.Question_ID);

            cmd.Parameters.AddWithValue("@Seq_Number", app_answer_item.Seq_Number);
            cmd.Parameters.AddWithValue("@Answer", app_answer_item.Answer);
            cmd.Parameters.AddWithValue("@level", app_answer_item.Level);

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
                result = false;
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [InsertAppAnswerItem] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Insert Ct_App_Info_Contact
    public static bool InsertAppInfoContactSub(bl_app_info_contact app_info_contact)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Contact_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_contact.App_Register_ID);
            cmd.Parameters.AddWithValue("@Mobile_Phone1", app_info_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", app_info_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Home_Phone1", app_info_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", app_info_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", app_info_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", app_info_contact.Office_Phone2);
            cmd.Parameters.AddWithValue("@Fax1", app_info_contact.Fax1);

            cmd.Parameters.AddWithValue("@Fax2", app_info_contact.Fax2);
            cmd.Parameters.AddWithValue("@EMail", app_info_contact.EMail);

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
                Log.AddExceptionToLog("Error in function [InsertAppInfoContactSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Update Ct_App_Info_Contact_Sub
    public static bool UpdateAppInfoContactSub(bl_app_info_contact app_info_contact)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Contact_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_contact.App_Register_ID);
            cmd.Parameters.AddWithValue("@Mobile_Phone1", app_info_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", app_info_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Home_Phone1", app_info_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", app_info_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", app_info_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", app_info_contact.Office_Phone2);
            cmd.Parameters.AddWithValue("@Fax1", app_info_contact.Fax1);

            cmd.Parameters.AddWithValue("@Fax2", app_info_contact.Fax2);
            cmd.Parameters.AddWithValue("@EMail", app_info_contact.EMail);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoContact] in class [da_application]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Info_Person_Sub
    public static bool DeleteAppInfoPersonSub(string app_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Person_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoPersonSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    public static bool DeleteAppInfoPersonSubByPersonIDs(string personIDs)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Person_Sub_By_PersonIDs";

            cmd.Parameters.AddWithValue("@Person_ID", personIDs);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoPersonSubByPersonIDs] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    public static bool DeleteAppLifeProductSubByLevel(int level , string appID)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Life_Product_Sub_By_Level";

            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Parameters.AddWithValue("@App_Register_ID", appID);

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
                Log.AddExceptionToLog("Error in function [DeleteAppLifeProductSubByLevel] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Info_Contact_Sub
    public static bool DeleteAppInfoContactSub(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Contact_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoContactSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Info_Address_Sub
    public static bool DeleteAppInfoAddressSub(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Address_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoAddressSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Delete Ct_App_Job_History
    public static bool DeleteAppJobHistorySub(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Job_History_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppJobHistorySub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Life_Product
    public static bool DeleteAppLifeProductSub(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Life_Product_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppLifeProductSub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Info_Body
    public static bool DeleteAppInfoBodySub(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Body_Sub";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoBodySub] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Info_Body
    public static bool DeleteAppInfoBodySubByIDs(bl_app_info_body_sub body)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Info_Body_Sub_By_IDs";

            cmd.Parameters.AddWithValue("@App_Register_ID", body.App_Register_ID);
            cmd.Parameters.AddWithValue("@Id", body.Id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppInfoBodySubByIDs] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Update Ct_App_Benefit_Item
    public static bool UpdateAppBenefitItem(bl_app_benefit_item app_benefit_item)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Benefit_Item";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_benefit_item.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Benefit_Item_ID", app_benefit_item.App_Benefit_Item_ID);
            cmd.Parameters.AddWithValue("@Full_Name", app_benefit_item.Full_Name);
            cmd.Parameters.AddWithValue("@ID_Type", app_benefit_item.ID_Type);
            cmd.Parameters.AddWithValue("@ID_Card", app_benefit_item.ID_Card);
            cmd.Parameters.AddWithValue("@Percentage", app_benefit_item.Percentage);
            cmd.Parameters.AddWithValue("@Relationship", app_benefit_item.Relationship);
            cmd.Parameters.AddWithValue("@Seq_Number", app_benefit_item.Seq_Number);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", app_benefit_item.Relationship_Khmer);
            cmd.Parameters.AddWithValue("@Remarks", app_benefit_item.Remarks);
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
                Log.AddExceptionToLog("Error in function [UpdateAppBenefitItem] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Benefit_Item
    public static bool DeleteAppBenefitItemByIDs(bl_app_benefit_item app_benefit_item)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Benefit_Item_By_IDs";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_benefit_item.App_Register_ID);
            cmd.Parameters.AddWithValue("@App_Benefit_Item_ID", app_benefit_item.App_Benefit_Item_ID);
           
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
                Log.AddExceptionToLog("Error in function [DeleteAppBenefitItem] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Delete Ct_App_Job_History_sub
    public static bool DeleteAppJobHistoryByJobIDs(bl_app_job_history_sub job_history)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Job_History_Sub_By_JobIDs";

            cmd.Parameters.AddWithValue("@App_Register_ID", job_history.App_Register_ID);
            cmd.Parameters.AddWithValue("@Job_ID", job_history.Job_ID);

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
                Log.AddExceptionToLog("Error in function [DeleteAppJobHistoryByJobIDs] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //create new class which inherit from  bl_app_info_person for Personal information sub
    public class bl_app_info_person_sub : bl_app_info_person{
        //add one more property
        public int Level { get; set; }
        public string Person_ID { get; set; }
        public string Marital_Status { get; set; }
        public string Relationship { get; set; }
    }
    public class bl_app_info_person_fp6 : bl_app_info_person
    {
        public string Marital_Status { get; set; }
        public string Relationship { get; set; }
    }
    public static bool InsertAppInfoPerson(bl_app_info_person_fp6 app_info_person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Info_Person_FP6";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_person.App_Register_ID);
            cmd.Parameters.AddWithValue("@ID_Card", app_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", app_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", app_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Last_Name", app_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", app_info_person.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", app_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_person.Country_ID);

            cmd.Parameters.AddWithValue("@Khmer_First_Name", app_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", app_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", app_info_person.Father_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Father_Last_Name", app_info_person.Father_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_First_Name", app_info_person.Mother_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_Last_Name", app_info_person.Mother_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_First_Name", app_info_person.Prior_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_Last_Name", app_info_person.Prior_Last_Name.ToUpper());

            cmd.Parameters.AddWithValue("@Marital_Status", app_info_person.Marital_Status);
            cmd.Parameters.AddWithValue("@Relationship", app_info_person.Relationship);
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
                Log.AddExceptionToLog("Error in function [InsertAppInfoPerson] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //Update Ct_App_Info_Person
    public static bool UpdateAppInfoPerson(bl_app_info_person_fp6 app_info_person)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Info_Person_FP6";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_info_person.App_Register_ID);
            cmd.Parameters.AddWithValue("@ID_Card", app_info_person.ID_Card);
            cmd.Parameters.AddWithValue("@ID_Type", app_info_person.ID_Type);
            cmd.Parameters.AddWithValue("@First_Name", app_info_person.First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Last_Name", app_info_person.Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Gender", app_info_person.Gender);
            cmd.Parameters.AddWithValue("@Birth_Date", app_info_person.Birth_Date);
            cmd.Parameters.AddWithValue("@Country_ID", app_info_person.Country_ID);

            cmd.Parameters.AddWithValue("@Khmer_First_Name", app_info_person.Khmer_First_Name);
            cmd.Parameters.AddWithValue("@Khmer_Last_Name", app_info_person.Khmer_Last_Name);
            cmd.Parameters.AddWithValue("@Father_First_Name", app_info_person.Father_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Father_Last_Name", app_info_person.Father_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_First_Name", app_info_person.Mother_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Mother_Last_Name", app_info_person.Mother_Last_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_First_Name", app_info_person.Prior_First_Name.ToUpper());
            cmd.Parameters.AddWithValue("@Prior_Last_Name", app_info_person.Prior_Last_Name.ToUpper());

            cmd.Parameters.AddWithValue("@Marital_Status", app_info_person.Marital_Status);
            cmd.Parameters.AddWithValue("@Relationship", app_info_person.Relationship);

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
                Log.AddExceptionToLog("Error in function [UpdateAppInfoPerson] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public class bl_app_life_product_Sub : bl_app_life_product
    {
        //add one more property
        public int Level { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime Created_On { get; set; }
        public string Action { get; set; }
        public string Rider_ID { get; set; }
    }
    public class bl_app_answer_item_fp6 : bl_app_answer_item {
        //add one more property
        public int Level { get; set; }
    
    }

    public class bl_app_job_history_sub : bl_app_job_history
    {
        public int Level{get;set;}
        public string Job_ID{get; set;}
        public string Address { get; set; }
    }

    //retreiv data
    public static DataTable GetDataTable(string sqlProcedureName, String strAppId) {

        DataTable tbl = new DataTable();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sqlProcedureName, con);
                SqlDataAdapter da;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = strAppId;
                cmd.Parameters.Add(paramName);

                con.Open();
                da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                SqlDataReader rdr = cmd.ExecuteReader();
                da.Dispose();
                cmd.Dispose();
                con.Close();
                
            }

        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function GetDataTable in class da_application_fp6, Detail: " + ex.Message);
        }
        //count row
        int count = tbl.Rows.Count;
        return tbl;
    }

    //Get Application
    public static List<bl_app_register> GetApplication(string appID) {

        List<bl_app_register> myList = new List<bl_app_register>();
        try
        {
           

            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_By_App_Register_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_register appRegister = new bl_app_register();

                        appRegister.App_Register_ID = dr.GetString(dr.GetOrdinal("App_Register_ID"));
                        appRegister.App_Number = dr.GetString(dr.GetOrdinal("App_number"));
                        appRegister.Channel_ID = dr.GetString(dr.GetOrdinal("channel_id"));
                        appRegister.Channel_Item_ID = dr.GetString(dr.GetOrdinal("channel_item_id"));
                        appRegister.Created_Note = dr.GetString(dr.GetOrdinal("created_note2"));
                        appRegister.Payment_Code = dr.GetString(dr.GetOrdinal("payment_code"));

                        myList.Add(appRegister);
                    }
                }
                con.Close();
            }
           
        }
        catch (Exception ex) {

            Log.AddExceptionToLog("Error function GetApplication in class da_application_fp6, Detail: "+ex.Message);
        }
        return myList;
    
    }

    //Get Application info
    public static List<bl_app_info> GetApplicationInfo(string appID)
    {
        List<bl_app_info> myList = new List<bl_app_info>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_By_App_Register_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_info appInfo = new bl_app_info();

                        appInfo.App_Register_ID = dr.GetString(dr.GetOrdinal("App_Register_ID"));
                        appInfo.App_Date = dr.GetDateTime(dr.GetOrdinal("app_date"));
                        appInfo.Benefit_Note = dr.GetString(dr.GetOrdinal("Benefit_note"));
                        appInfo.Sale_Agent_ID = dr.GetString(dr.GetOrdinal("sale_agent_id"));
                        appInfo.Created_Note = dr.GetString(dr.GetOrdinal("created_note2"));
                        appInfo.Created_By = dr.GetString(dr.GetOrdinal("created_by"));
                        appInfo.Created_On = dr.GetDateTime(dr.GetOrdinal("created_on"));
                       
                        myList.Add(appInfo);
                    }
                }
                con.Close();
            }

        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error function GetApplicationInfo in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;

    }

    //Get Address
    public static List<bl_app_info_address> GetApplicationAddress(string appID) {

        List<bl_app_info_address> myList = new List<bl_app_info_address>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Info_Address_By_App_Register_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_info_address appAddress = new bl_app_info_address();

                        appAddress.App_Register_ID = dr.GetString(dr.GetOrdinal("App_Register_ID"));
                        appAddress.Country_ID = dr.GetString(dr.GetOrdinal("country_id"));
                        appAddress.Address1 = dr.GetString(dr.GetOrdinal("address1"));
                        appAddress.Address2 = dr.GetString(dr.GetOrdinal("address2"));
                        appAddress.Province = dr.GetString(dr.GetOrdinal("province"));
                        appAddress.Zip_Code = dr.GetString(dr.GetOrdinal("zip_code"));
                        myList.Add(appAddress);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function GetApplicationAddress in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;
    }
    //Get Address Sub
    public static List<bl_app_info_address> GetApplicationAddressSub(string appID)
    {

        List<bl_app_info_address> myList = new List<bl_app_info_address>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Info_Address_Sub_By_App_Register_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_info_address appAddress = new bl_app_info_address();

                        appAddress.App_Register_ID = dr.GetString(dr.GetOrdinal("App_Register_ID"));
                        appAddress.Country_ID = dr.GetString(dr.GetOrdinal("country_id"));
                        appAddress.Address1 = dr.GetString(dr.GetOrdinal("address1"));
                        appAddress.Address2 = dr.GetString(dr.GetOrdinal("address2"));
                        appAddress.Province = dr.GetString(dr.GetOrdinal("province"));
                        appAddress.Zip_Code = dr.GetString(dr.GetOrdinal("zip_code"));
                        myList.Add(appAddress);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationAddressSub in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;
    }
    
    //Get Contact
    public static List<bl_app_info_contact> GetApplicationContact(string appID) {

        List<bl_app_info_contact> myList = new List<bl_app_info_contact>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Info_Contact", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_info_contact appContact = new bl_app_info_contact();

                        appContact.App_Register_ID = dr.GetString(dr.GetOrdinal("App_Register_ID"));
                        appContact.EMail = dr.GetString(dr.GetOrdinal("email"));
                        appContact.Home_Phone1 = dr.GetString(dr.GetOrdinal("home_phone1"));
                        appContact.Mobile_Phone1 = dr.GetString(dr.GetOrdinal("mobile_phone1"));
                        myList.Add(appContact);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationContact in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;
    
    }
    //Get Contact sub
    public static List<bl_app_info_contact> GetApplicationContactSub(string appID)
    {

        List<bl_app_info_contact> myList = new List<bl_app_info_contact>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Info_Contact_Sub", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_info_contact appContact = new bl_app_info_contact();

                        appContact.App_Register_ID = dr.GetString(dr.GetOrdinal("App_Register_ID"));
                        appContact.EMail = dr.GetString(dr.GetOrdinal("email"));
                        appContact.Home_Phone1 = dr.GetString(dr.GetOrdinal("home_phone1"));
                        appContact.Mobile_Phone1 = dr.GetString(dr.GetOrdinal("mobile_phone1"));
                        myList.Add(appContact);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationContactSub in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;

    }
    //Get Job History
#region Old code
    /*
    public static List<bl_app_job_history> GetApplicationJobHistory(string appID) {
        
       // List<bl_app_job_history> myList = new List<bl_app_job_history>();
        try
        {
            
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Info_JobHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_job_history history = new bl_app_job_history();
                        
                        history.Anual_Income = dr.GetDouble(dr.GetOrdinal("anual_income"));
                        history.App_Register_ID = dr.GetString(dr.GetOrdinal("app_register_id"));
                        history.Current_Position = dr.GetString(dr.GetOrdinal("current_position"));
                        history.Employer_Name = dr.GetString(dr.GetOrdinal("employer_name"));
                        history.Job_Role = dr.GetString(dr.GetOrdinal("job_role"));
                        history.Nature_Of_Business = dr.GetString(dr.GetOrdinal("nature_of_business"));

                        myList.Add(history);
                    }
                }
                con.Close();
            }
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationJobHistory in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;
    
    }
    */

#endregion
    public static DataTable GetApplicationJobHistory(string appID)
    {
        DataTable tblJobHistory = new DataTable();
        try
        {
            tblJobHistory = da_application_fp6.GetDataTable("SP_Get_App_Info_JobHistory", appID);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationJobHistory in class da_application_fp6, Detail: " + ex.Message);
        }

        return tblJobHistory;
    }
    public static DataTable GetApplicationJobHistorySub(string appID)
    {
        DataTable tblJobHistory = new DataTable();
        try
        {
            tblJobHistory = da_application_fp6.GetDataTable("SP_Get_App_Info_JobHistory_Sub", appID);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationJobHistorySub in class da_application_fp6, Detail: " + ex.Message);
        }

        return tblJobHistory;
    }
    //Get Premium 
    public static List<da_application_fp6.bl_app_life_product_Sub> GetApplicationPremium(string appID) {

        List<da_application_fp6.bl_app_life_product_Sub> myList = new List<da_application_fp6.bl_app_life_product_Sub>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Premium_By_App_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        da_application_fp6.bl_app_life_product_Sub pre = new da_application_fp6.bl_app_life_product_Sub();

                        pre.Age_Insure = dr.GetInt32(dr.GetOrdinal("age_insure"));
                        pre.App_Register_ID = dr.GetString(dr.GetOrdinal("app_register_id"));
                        pre.Assure_Year = dr.GetInt32(dr.GetOrdinal("assure_year"));
                        pre.Pay_Year = dr.GetInt32(dr.GetOrdinal("pay_year"));
                        pre.Product_ID = dr.GetString(dr.GetOrdinal("product_id"));
                        pre.System_Premium = dr.GetDouble(dr.GetOrdinal("system_premium"));
                        pre.User_Premium = dr.GetDouble(dr.GetOrdinal("user_premium"));
                        pre.System_Sum_Insure = dr.GetDouble(dr.GetOrdinal("system_sum_insure"));
                        pre.User_Sum_Insure = dr.GetDouble(dr.GetOrdinal("user_sum_insure"));
                        pre.Pay_Mode = dr.GetInt32(dr.GetOrdinal("pay_mode"));
                        pre.Level =Convert.ToInt32( dr.GetString(dr.GetOrdinal("level")));

                        myList.Add(pre);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationPremium in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;
    
    }

    //Get Application discount
    public static List<bl_app_discount> GetApplicationDiscount(string appID) {

        List<bl_app_discount> myList = new List<bl_app_discount>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Discount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_discount dis = new bl_app_discount();

                        dis.Annual_Premium = dr.GetDouble(dr.GetOrdinal("annual_premium"));
                        dis.App_Register_ID = dr.GetString(dr.GetOrdinal("app_register_id"));
                        dis.Created_Note = dr.GetString(dr.GetOrdinal("created_note"));
                        dis.Discount_Amount = dr.GetDouble(dr.GetOrdinal("discount_amount"));
                        dis.Pay_Mode = dr.GetInt32(dr.GetOrdinal("pay_mode"));
                        dis.Premium = dr.GetDouble(dr.GetOrdinal("premium"));
                        dis.Total_Amount = dr.GetDouble(dr.GetOrdinal("total_amount"));
                        myList.Add(dis);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            
            Log.AddExceptionToLog("Error function GetApplicationDiscount in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;
    
    }

    //Get Benefit item
    public static List<bl_app_benefit_item> GetApplicationBenefit(string appID) {

        List<bl_app_benefit_item> myList = new List<bl_app_benefit_item>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_App_Benefit_Item", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@App_Register_ID";
                paramName.Value = appID;
                cmd.Parameters.Add(paramName);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        bl_app_benefit_item dis = new bl_app_benefit_item();

                      
                        dis.App_Register_ID = dr.GetString(dr.GetOrdinal("app_register_id"));
                        dis.App_Benefit_Item_ID = dr.GetString(dr.GetOrdinal("app_benefit_item_id"));
                        dis.ID_Type = dr.GetInt32(dr.GetOrdinal("id_type"));
                        dis.ID_Card = dr.GetString(dr.GetOrdinal("id_card"));
                        dis.Percentage = dr.GetDouble(dr.GetOrdinal("percentage"));
                        dis.Relationship = dr.GetString(dr.GetOrdinal("relationship"));
                    
                        myList.Add(dis);
                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationBenefit in class da_application_fp6, Detail: " + ex.Message);
        }
        return myList;
    }

    //Get List of Answer_Items by app_id
    public static List<da_application_fp6.bl_app_answer_item_fp6> GetAppAnswerItem(string app_id)
    {

        List<da_application_fp6.bl_app_answer_item_fp6> answer_items = new List<da_application_fp6.bl_app_answer_item_fp6>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Answer_Item_By_App_Register_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    da_application_fp6.bl_app_answer_item_fp6 answer_item = new da_application_fp6.bl_app_answer_item_fp6();

                    answer_item.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    answer_item.App_Answer_Item_ID = rdr.GetString(rdr.GetOrdinal("App_Answer_Item_ID"));
                    answer_item.Question_ID = rdr.GetString(rdr.GetOrdinal("Question_ID"));
                    answer_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));
                    answer_item.Answer = rdr.GetInt32(rdr.GetOrdinal("Answer"));
                    answer_item.Level = rdr.GetInt32(rdr.GetOrdinal("level"));
                    answer_items.Add(answer_item);
                }
            }
            con.Close();
        }
        return answer_items;
    }

    public class bl_app_info_body_sub : bl_app_info_body
    {
        public int Level { get; set; }
        public string Id { get; set; }
    }

    public class bl_app_rider
    {
        public int Level { get; set; }
        public string Rider_ID { get; set; }
        public string App_Register_ID { get; set; }
        public string Rider_Type { get; set; }
        public double SumInsured { get; set; }
        public double Premium { get; set; }
        public double Discount { get; set; }
        public string Product_ID { get; set; }
        public double Rate { get; set; }
        public int Rate_ID { get; set; }
        public double Original_Amount { get; set; }
        public double Rounded_Amount { get; set; }
        public string Action { get; set; }
        public DateTime Created_On { get; set; }
        public int Age_Insure { get; set; }
        public int Pay_Year{get;set;}
        public int Pay_Up_To_Age { get; set; }
        public int Assure_Year { get; set; }
        public int Assure_Up_To_Age { get; set; }
        public DateTime Effective_Date { get; set; }
    }

    //Insert Rider
    public static bool InsertAppRider(bl_app_rider rider)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Rider";

            cmd.Parameters.AddWithValue("@Rider_ID", rider.Rider_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", rider.App_Register_ID);
            cmd.Parameters.AddWithValue("@Rider_Type", rider.Rider_Type);
            cmd.Parameters.AddWithValue("@Sum_Insured", rider.SumInsured);
            cmd.Parameters.AddWithValue("@Premium", rider.Premium);
            cmd.Parameters.AddWithValue("@Discount", rider.Discount);
            cmd.Parameters.AddWithValue("@Level", rider.Level);
            cmd.Parameters.AddWithValue("@Product_ID", rider.Product_ID);
            cmd.Parameters.AddWithValue("@Rate", rider.Rate);
            cmd.Parameters.AddWithValue("@Rate_ID", rider.Rate_ID);
            cmd.Parameters.AddWithValue("@Original_Amount", rider.Original_Amount);
            cmd.Parameters.AddWithValue("@Rounded_Amount", rider.Rounded_Amount);
            cmd.Parameters.AddWithValue("@Action", rider.Action);
            cmd.Parameters.AddWithValue("@Created_On", rider.Created_On);
            cmd.Parameters.AddWithValue("@Age_Insure", rider.Age_Insure);
            cmd.Parameters.AddWithValue("@Pay_Year", rider.Pay_Year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", rider.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", rider.Assure_Year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", rider.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Effective_Date", rider.Effective_Date);

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
                Log.AddExceptionToLog("Error in function [InsertAppRider] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
   
        return result;
    }

    //Update Rider
    public static bool UpdateAppRider(bl_app_rider rider)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_App_Rider_By_App_ID";

            cmd.Parameters.AddWithValue("@Rider_ID", rider.Rider_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", rider.App_Register_ID);
            cmd.Parameters.AddWithValue("@Rider_Type", rider.Rider_Type);
            cmd.Parameters.AddWithValue("@Sum_Insured", rider.SumInsured);
            cmd.Parameters.AddWithValue("@Premium", rider.Premium);
            cmd.Parameters.AddWithValue("@Discount", rider.Discount);
            cmd.Parameters.AddWithValue("@Level", rider.Level);
            cmd.Parameters.AddWithValue("@Product_ID", rider.Product_ID);
            cmd.Parameters.AddWithValue("@Rate_ID", rider.Rate_ID);
            cmd.Parameters.AddWithValue("@Rate", rider.Rate);
            cmd.Parameters.AddWithValue("@Original_Amount", rider.Original_Amount);
            cmd.Parameters.AddWithValue("@Rounded_Amount", rider.Rounded_Amount);
            cmd.Parameters.AddWithValue("@Age_Insure", rider.Age_Insure);
            cmd.Parameters.AddWithValue("@Pay_Year", rider.Pay_Year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", rider.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", rider.Assure_Year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", rider.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Effective_Date", rider.Effective_Date);

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
                Log.AddExceptionToLog("Error in function [UpdateAppRider] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }

        return result;
    }
    //Delete Rider
    public static bool DeleteAppRider(string app_register_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Rider_By_App_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppRider] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }

        return result;
    }
    public static bool DeleteAppRiderByID(string rider_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Rider_By_ID";

            cmd.Parameters.AddWithValue("@Rider_ID", rider_id);

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
                Log.AddExceptionToLog("Error in function [DeleteAppRiderByID] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }

        return result;
    }
    //Get Rider Liste
    public static List<bl_app_rider> GetAppRiderList(string app_register_id)
    {
        List<bl_app_rider> myRider = new List<bl_app_rider>();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_App_Rider_By_App_ID";
            cmd.CommandTimeout = 60;

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                bl_app_rider rider;
                string sum = "";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                   sum = dr["sum_insured"].ToString();

                    rider = new bl_app_rider();
                    rider.App_Register_ID = dr.GetString(dr.GetOrdinal("app_register_id"));
                    rider.Rider_ID = dr.GetString(dr.GetOrdinal("rider_id"));
                    rider.Rider_Type = dr.GetString(dr.GetOrdinal("rider_type"));
                    rider.SumInsured = Convert.ToDouble(dr["sum_insured"].ToString()); //dr.GetDouble(dr.GetOrdinal("sum_insured"));
                    rider.Premium = Convert.ToDouble(dr["premium"].ToString()); // dr.GetDouble(dr.GetOrdinal("premium"));
                    rider.Product_ID = dr.GetString(dr.GetOrdinal("product_id"));
                    rider.Level = dr.GetInt32(dr.GetOrdinal("level"));
                    rider.Discount = Convert.ToDouble(dr["discount"].ToString());// dr.GetDouble(dr.GetOrdinal("discount"));
                    rider.Rate_ID = dr.GetInt32(dr.GetOrdinal("rate_id"));
                    rider.Action = dr.GetString(dr.GetOrdinal("Action"));
                    rider.Age_Insure = dr.GetInt32(dr.GetOrdinal("Age_Insure"));
                    rider.Pay_Year = dr.GetInt32(dr.GetOrdinal("Pay_Year"));
                    rider.Pay_Up_To_Age= dr.GetInt32(dr.GetOrdinal("Pay_Up_To_Age"));
                    rider.Assure_Year = dr.GetInt32(dr.GetOrdinal("Assure_Year"));
                    rider.Assure_Up_To_Age = dr.GetInt32(dr.GetOrdinal("Assure_Up_To_Age"));
                   

                    if(!DBNull.Value.Equals(dr["Effective_Date"]))
                    {
                        //rider.Effective_Date = Helper.FormatDateTime(dr["Effective_Date"].ToString());
                        rider.Effective_Date = Convert.ToDateTime(dr["Effective_Date"].ToString());
                       
                    }
                   
                    if (!DBNull.Value.Equals( dr["rate"]))
                    {
                        rider.Rate = dr.GetDouble(dr.GetOrdinal("rate"));
                    }
                    else
                    {
                        rider.Rate = 0.0;
                    }

                    if (!DBNull.Value.Equals(dr["original_amount"]))
                    {
                        rider.Original_Amount = dr.GetDouble(dr.GetOrdinal("original_amount"));
                    }
                    else
                    {
                        rider.Original_Amount = 0.0;
                    }
                    if (!DBNull.Value.Equals(dr["rounded_amount"]))
                    {
                        rider.Rounded_Amount = dr.GetDouble(dr.GetOrdinal("rounded_amount"));
                    }
                    else
                    {
                        rider.Rounded_Amount = 0.0;
                    }

                    myRider.Add(rider);

                }

                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [GetAppRiderList] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }

        return myRider;
    }
    public class bl_app_prem_pay_fp6 : bl_app_prem_pay
    {
        public int Level{get;set;}
    }
    public static bool InsertAppPremPayFP6(da_application_fp6.bl_app_prem_pay_fp6 app_prem_pay)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Prem_Pay_Fp6";

            cmd.Parameters.AddWithValue("@App_Prem_Pay_ID", app_prem_pay.App_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_prem_pay.App_Register_ID);
            cmd.Parameters.AddWithValue("@Pay_Date", app_prem_pay.Pay_Date);
            cmd.Parameters.AddWithValue("@Is_Init_Payment", app_prem_pay.Is_Init_Payment);
            cmd.Parameters.AddWithValue("@Amount", app_prem_pay.Amount);
            cmd.Parameters.AddWithValue("@Original_Amount", app_prem_pay.Original_Amount);
            cmd.Parameters.AddWithValue("@Created_By", app_prem_pay.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", app_prem_pay.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", app_prem_pay.Created_On);
            cmd.Parameters.AddWithValue("@Rounded_Amount", app_prem_pay.Rounded_Amount);
            cmd.Parameters.AddWithValue("@Level", app_prem_pay.Level);

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
                Log.AddExceptionToLog("Error in function [InsertAppPremPayFP6] in class [da_application_fp6]. Details: " + ex.Message);
            }
        }
        return result;
    }
    public static List<da_application_fp6.bl_app_prem_pay_fp6> GetAppPremPay(string appID)
    {
        da_application_fp6.bl_app_prem_pay_fp6 appPremPay ;
        List<da_application_fp6.bl_app_prem_pay_fp6> premList = new List<da_application_fp6.bl_app_prem_pay_fp6>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
        
            {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_App_Prem_Pay_FP6";

            cmd.Parameters.AddWithValue("@App_Register_ID", appID);

            cmd.Connection = con;
            con.Open();
           
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    appPremPay = new da_application_fp6.bl_app_prem_pay_fp6();
                    appPremPay.App_Prem_Pay_ID=dr.GetString(dr.GetOrdinal("app_prem_pay_id"));
                    appPremPay.App_Register_ID=dr.GetString(dr.GetOrdinal("app_register_id"));
                    appPremPay.Pay_Date=dr.GetDateTime(dr.GetOrdinal("pay_date"));
                    appPremPay.Amount=dr.GetDouble(dr.GetOrdinal("amount"));
                    appPremPay.Original_Amount=dr.GetDouble(dr.GetOrdinal("original_amount"));
                    appPremPay.Rounded_Amount=dr.GetDouble(dr.GetOrdinal("rounded_amount"));
                    appPremPay.Is_Init_Payment=dr.GetInt32(dr.GetOrdinal("is_init_payment"));
                    appPremPay.Level=dr.GetInt32(dr.GetOrdinal("level"));
                    appPremPay.Created_By=dr.GetString(dr.GetOrdinal("created_by"));
                    appPremPay.Created_On=dr.GetDateTime(dr.GetOrdinal("created_on"));
                    appPremPay.Created_Note=dr.GetString(dr.GetOrdinal("created_note"));

                   premList.Add(appPremPay);

                }
                dr.Close();
                cmd.Parameters.Clear();
                con.Close();
            }
          

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetAppPremPay in class da_application_fp6. Detail: " + ex.Message);

        }
        return premList;
    }

    //Delete answer
    public static bool DeleteAppAnswerByAppIDAndLevel(string appID, int level)
    {
        bool result = false;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
           
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Answer_BY_AppID_And_Level";

            cmd.Parameters.AddWithValue("@App_Register_ID", appID);
            cmd.Parameters.AddWithValue("@Level", level);

            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();

            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error delete answer in function DeleteAppAnswerByAppIDAndLevel in class da_application_fp6, Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    public static string GetApplicationID(string appID)
    {

        string myAppID = "";

        
        try
        {
            myAppID = appID;
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
           
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_App_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", appID);

            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                myAppID = dr.GetString(dr.GetOrdinal("app_register_id"));
            }
            dr.Close();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetApplicationID in class da_application_fp6. Detail: " + ex.Message);
            myAppID = "";
        }
        return myAppID;
    }
    public class AppPlanInfo
    {
        public string ApplicationNumber { get; set; }
        public string ApplicationID { get; set; }
        public double SumInsure { get; set; }
        public int AssureYear { get; set; }
        public int PayYear { get; set; }
        public int PayMode { get; set; }
        public string ProductID { get; set; }
    }

    public static List<AppPlanInfo> GetAppPlanInfo(string appID)
    {
       
        List<AppPlanInfo> planList = new List<AppPlanInfo>();
        AppPlanInfo plan;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_App_Plan_Info";

            cmd.Parameters.AddWithValue("@App_Register_ID", appID);

            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                plan = new AppPlanInfo();
                plan.ApplicationID = dr.GetString(dr.GetOrdinal("app_register_id"));
                plan.ApplicationNumber = dr.GetString(dr.GetOrdinal("app_number"));
                plan.SumInsure = dr.GetDouble(dr.GetOrdinal("system_sum_insure"));
                plan.AssureYear = dr.GetInt32(dr.GetOrdinal("assure_year"));
                plan.PayYear = dr.GetInt32(dr.GetOrdinal("pay_year"));
                plan.PayMode = dr.GetInt32(dr.GetOrdinal("pay_mode"));
                plan.ProductID = dr.GetString(dr.GetOrdinal("product_id"));
                planList.Add(plan);
            }
            dr.Close();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetAppPlanInfo in class da_application_fp6, Detail: " + ex.Message);
        }

        return planList;
    }
    public class Channel 
    {
        public string ChannelID { get; set; }
        public string Type { get; set; }
    }
    public static List<Channel> GetChannelList()
    {
        List<Channel> myChannel = new List<Channel>();
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            Channel channel;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_App_Channel";
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                channel = new Channel();
                channel.ChannelID = dr["channel_id"].ToString();
                channel.Type = dr["type"].ToString();
                myChannel.Add(channel);
            }
            dr.Close();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetChannelList in class da_application_fp6, Detail: " + ex.Message);
        }
        return myChannel;
    }

    public static bool DeleteAppPremPayByLevel(string appID, int level)
    {
        bool status = false;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
        
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_App_Prem_Pay_By_Level";
            cmd.Parameters.AddWithValue("@App_Register_ID", appID);
            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();
            status= true;
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function DeleteAppPremPayByLevel in class da_application_fp6, Detail: " + ex.Message);
            status = false;
        }
        return status;
    }
    public static int GetMaxPremPay(int policyNumber)
    {
        int max = 0;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Max_Prem_Lot";
            cmd.Parameters.AddWithValue("@Policy_Number", policyNumber);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr.HasRows)
                {
                    max = Convert.ToInt32(dr["prem_lot"].ToString());
                }
                else
                {
                    max = 0;
                }
               
            }
            dr.Close();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetMaxPremPay in class application_fp6, Detail: " + ex.Message);
        }
        return max;
    }

    #region Class Family Protection Product
    public class ProductFP6 
    {
        public string ProductID { set; get; }
        public int PayYear { set; get; }
        public int AssureYear { set; get; }

        public static List<ProductFP6> GetProductFP6List()
        {
            List<ProductFP6> myProductList = new List<ProductFP6>();
            try
            {
                string connString = AppConfiguration.GetConnectionString();
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                ProductFP6 pro;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_APP_Product_FP6";
                cmd.Connection = con;
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pro = new ProductFP6();
                    pro.ProductID = dr["product_id"].ToString();
                    pro.AssureYear = Convert.ToInt32(dr["assure_year"].ToString());
                    pro.PayYear = Convert.ToInt32(dr["pay_year"].ToString());
                    myProductList.Add(pro);
                }
                dr.Close();
                cmd.Dispose();
                con.Close();
            }

            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function GetProductFP6List in class da_application_fp6, Detail: " + ex.Message);
            }
            return myProductList;
        }
        
    }
    #endregion Class Family Protection Product
    #region Class Payment Mode
    public class PayMentMode 
    {
        public int PayMentModeID { set; get; }
        public string Mode { set; get; }
        public double Factor { set; get; }

        public static List<PayMentMode> GetPaymentModeList()
        {
            List<PayMentMode> paymodeList = new List<PayMentMode>();
            try
            {
                string connString = AppConfiguration.GetConnectionString();
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                PayMentMode pay;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_APP_PaymentMode_List";
                cmd.Connection = con;
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pay = new PayMentMode();
                    pay.PayMentModeID = Convert.ToInt32(dr["pay_mode_id"].ToString());
                    pay.Mode = dr["mode"].ToString();
                    pay.Factor = Convert.ToDouble(dr["factor"].ToString());
                    paymodeList.Add(pay);
                }
                dr.Close();
                cmd.Dispose();
                con.Close();
            }

            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function GetProductFP6List in class da_application_fp6, Detail: " + ex.Message);
            }
            return paymodeList;
        }
    }
    #endregion Class Payment Mode

    #region Class Premium Detail
    public class PremiumDetail
    {
        public int Level{set;get;}
        public string PersonID{set;get;}
        public string RegisterID{get;set;}
        public string FullName{get;set;}
        public string Gender{get;set;}
        public DateTime BirthDate{get;set;}
        public int AgeInsure {get;set;}
        public int AssureYear{get;set;}
        public int AssureUpToAge{get;set;}
        public int PayYear{get;set;}
        public int PayUpToAge{get;set;}
        public int PayMode{get;set;}
        public double SumInsure{get;set;}
        public double Premium{get;set;}
        public DateTime EffectiveDate{get;set;}

        public List<PremiumDetail> myPremiumList = new List<PremiumDetail>();
        public static void Add(int level, string personId, string registerId, string fullName, string gender, DateTime birthDate, 
                                int ageInsure, int assureYear, int assureUpToAge, int payYear, int payUpToAge, 
                                int payMode, double sumInsure, double premium, DateTime effectiveDate)

        {
           
            PremiumDetail prem = new PremiumDetail();

           prem.Level=level;
           prem.PersonID = personId;
           prem.RegisterID = registerId;
           prem.FullName = fullName;
           prem.Gender = gender;
           prem.BirthDate = birthDate;
           prem.AgeInsure = ageInsure;
           prem.AssureYear = assureYear;
           prem.AssureUpToAge = assureUpToAge;
           prem.PayYear = payYear;
           prem.PayUpToAge = payUpToAge;
           prem.PayMode = payMode;
           prem.SumInsure = sumInsure;
           prem.Premium = premium;
           prem.EffectiveDate = effectiveDate;

                
        }
    }
    #endregion End Class Premium Detail

   //update application discount
    public static bool UpdateApplicationDiscountTotalAmount(string app_register_id)
    {
        bool status = true;
        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand("SP_Update_App_Discount_Total_Amount", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();
            status = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateApplicationDiscountTotalAmount] in class [da_application_fp6], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }
    public static bool DeleteAppRiderForm(string app_register_id)
    {
        bool status = true;
        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand("SP_Delete_App_Rider_Form", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            con.Close();
            status = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteAppRiderForm] in class [da_application_fp6], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }

    //TODO: 01/26/2017 Create new UpdateAppUserPremium function to update user premium after user inforce application
    public static bool UpdateAppUserPremium(string app_register_id, int user_premium)
    {
        bool result = true;
        try
        {
            string  con_string = AppConfiguration.GetConnectionString();
            result = Helper.ExecuteProcedure(con_string, "SP_UPDATE_APP_USER_PREMIUM", new string[,] { { "@App_Register_ID", app_register_id }, { "@User_Premium", user_premium + "" } }, "da_application_fp6 => UpdateAppUserPremium");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdateAppUserPremium] in class [da_application_fp6], Detail: " + ex.Message);
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Spouse's sum insured range [2500USD - 50000USD]
    /// </summary>
    /// <param name="sum_insured"></param>
    /// <returns></returns>
    public static string VarlidSpouseSumInsured(double sum_insured)
    {
        string return_text = "";
        //min sum insured 5000 max 50000
        if (sum_insured < 2500)
        {
            return_text = "Sum insured is less than minimum sum insured (2500 USD), System not allow to process.";
        }
        else if (sum_insured > 50000)
        {
            return_text = "Sum insured is grather than maximum sum insured (50000 USD), System not allow to process.";
        }
        else
        {
            return_text = "";
        }
        return return_text;
    }
    /// <summary>
    /// kid's sum insured range [1250USD - 25000USD]
    /// </summary>
    /// <param name="sum_insured"></param>
    /// <returns></returns>
    public static string VarlidKidSumInsured(double sum_insured)
    {
        string return_text = "";
        //min sum insured 1250 max 25000
        if (sum_insured < 1250)
        {
            return_text = "Sum insured is less than minimum sum insured (1250 USD), System not allow to process.";
        }
        else if (sum_insured > 25000)
        {
            return_text = "Sum insured is grather than maximum sum insured (25000 USD), System not allow to process.";
        }
        else
        {
            return_text = "";
        }
        return return_text;
    }
    /// <summary>
    /// Spouse's sum insure = life's sum insure * 0.5
    /// </summary>
    /// <returns></returns>
    public static double GetSpouseSumInsured(double sum_insure)
    {
        double sum_insured = 0.0;
        try
        {
            double recal_sum_insured = 0.0;
            if (sum_insure>0)
            {
                recal_sum_insured = sum_insure * 0.5; //spouse 50% of life assure
            }
            sum_insured = Math.Ceiling( recal_sum_insured);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetSpouseSumInsured] in page [da_application_fp6_rider], Detail: " + ex.Message);
            sum_insured = 0.0;
        }
        return sum_insured;
    }
    /// <summary>
    /// Kid's sum insure = life's sum insure *0.25
    /// </summary>
    /// <returns></returns>
    public static double GetKidSumInsured(double sum_insure)
    {
        double sum_insured = 0.0;
        try
        {
            double recal_sum_insured = 0.0;
            if (sum_insure>0)
            {
                recal_sum_insured = sum_insure * 0.25; //kid 25% of life assure
            }
            sum_insured = Math.Ceiling( recal_sum_insured);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetKidSumInsured] in page [da_application_fp6_rider], Detail: " + ex.Message);
            sum_insured = 0.0;
        }
        return sum_insured;
    }
    public static List<da_application_fp6.bl_app_life_product_Sub> GetLifeProductSubList(string app_register_id)
    {
        List<da_application_fp6.bl_app_life_product_Sub> myList = new List<bl_app_life_product_Sub>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_APP_LIFE_PRODUCT_SUB_BY_APP_ID", new string[,] { { "@App_Register_ID", app_register_id } });
            foreach (DataRow row in tbl.Rows)
            {
                da_application_fp6.bl_app_life_product_Sub life = new bl_app_life_product_Sub();
                life.Rider_ID = row["rider_id"].ToString();
                life.App_Register_ID = row["app_register_id"].ToString();
                life.Product_ID = row["product_id"].ToString();
                life.Age_Insure = Convert.ToInt32(row["age_insure"].ToString());
                life.Pay_Year = Convert.ToInt32(row["pay_year"].ToString());
                life.Pay_Up_To_Age = Convert.ToInt32(row["pay_up_to_age"].ToString());
                life.Assure_Year = Convert.ToInt32(row["assure_year"].ToString());
                life.Assure_Up_To_Age = Convert.ToInt32(row["assure_up_to_age"].ToString());
                life.System_Sum_Insure = Convert.ToDouble(row["system_sum_insure"].ToString());
                life.System_Premium = Convert.ToDouble(row["system_premium"].ToString());
                life.User_Sum_Insure = Convert.ToDouble(row["system_sum_insure"].ToString());
                life.User_Premium = Convert.ToDouble(row["system_premium"].ToString());
                life.Pay_Mode = Convert.ToInt32(row["pay_mode"].ToString());
                life.Created_On = Convert.ToDateTime(row["created_on"].ToString());
                life.Action = row["action"].ToString();
                life.Level = Convert.ToInt32(row["level"].ToString());

                myList.Add(life);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Erro function [GetLifeProductSubList] in class [da_application_fp6], Detail: " + ex.Message); 
        }
        return myList;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app_id"></param>
    /// <returns></returns>
    public static string GetUWRiderStatus(string app_id)
    {
        string status = "";
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_Underwrite_Rider_Status_Code_by_App_Register_ID", new string[,] { { "@App_Register_ID", app_id } });
            //foreach (DataRow row in tbl.Rows)
            //{
            //    status = row["status_code"].ToString();
            //}
            if (tbl.Rows.Count > 0)
            {
                status = tbl.Rows[0]["status_code"].ToString();
            }
            else
            {
                status = "There's no rider";
            }
        }
        catch (Exception ex)
        {
            status = "";
            Log.AddExceptionToLog("Error function [GetUWRiderStatus] in class [da_application_fp6], Detail: " + ex.Message);
        }
        return status;
    }
}