using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Calculation
/// </summary>
public class Calculation
{
    #region "Constructor(s)"

    private static Calculation mytitle = null;
    public Calculation()
    {
        if (mytitle == null)
        {
            mytitle = new Calculation();
        }

    }
    #endregion

    #region "Public Functions"
    //Calculate premium Whole Life
    public static string CalculatePremiumWholeLife(string product_id, int applicant_age, int sum_insured_amount, int gender, int pay_mode)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Whole_Life_Rate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName1 = new SqlParameter();
            paramName1.ParameterName = "@Product_ID";
            paramName1.Value = product_id;
            cmd.Parameters.Add(paramName1);

            SqlParameter paramName2 = new SqlParameter();
            paramName2.ParameterName = "@Age";
            paramName2.Value = applicant_age;
            cmd.Parameters.Add(paramName2);

            SqlParameter paramName3 = new SqlParameter();
            paramName3.ParameterName = "@Gender";
            paramName3.Value = gender;
            cmd.Parameters.Add(paramName3);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {

                    myRate = Convert.ToDecimal(rdr.GetDouble(rdr.GetOrdinal("Rate")));

                }

            }
            con.Close();
        }

       
            //discount
            if (sum_insured_amount >= 2000 & sum_insured_amount < 5000)
            {
                myPremium = (myRate * sum_insured_amount) / 100;
            }
            else if (sum_insured_amount >= 5000 & sum_insured_amount < 40000)
            {
                myPremium = ((myRate - 1) * sum_insured_amount) / 100;
            }
            else if (sum_insured_amount >= 40000)
            {
                myPremium = ((myRate - 2) * sum_insured_amount) / 100;
            }
       

        //Premium Calculation by payment mode
        if (product_id == "W10" || product_id == "W15" || product_id == "W20")
        {
            if (pay_mode == 2) //Semi-annual old = 0.52
            {
                decimal old_factor = Convert.ToDecimal(0.52);
                decimal annual_premium = myPremium;
                myPremium = Convert.ToInt32(Math.Ceiling(myPremium * old_factor));
                return myPremium.ToString() + ',' + String.Format("{0:#0.##}", annual_premium);
            }
            else
            {
                return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);
            }
        }
        else
        {
            return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);
        }

        
    }

    //Calculate premium Term Life
    public static string CalculatePremiumTermLife(string product_id, int applicant_age, int sum_insured_amount, int gender, int pay_mode)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Term_Life_Rate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName1 = new SqlParameter();
            paramName1.ParameterName = "@Product_ID";
            paramName1.Value = product_id;
            cmd.Parameters.Add(paramName1);

            SqlParameter paramName2 = new SqlParameter();
            paramName2.ParameterName = "@Age";
            paramName2.Value = applicant_age;
            cmd.Parameters.Add(paramName2);

            SqlParameter paramName3 = new SqlParameter();
            paramName3.ParameterName = "@Gender";
            paramName3.Value = gender;
            cmd.Parameters.Add(paramName3);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {

                    myRate = Convert.ToDecimal(rdr.GetDouble(rdr.GetOrdinal("Rate")));

                }

            }
            con.Close();

        }

      
            //discount
            if (sum_insured_amount >= 5000 & sum_insured_amount < 10000)
            {
                myPremium = (myRate * sum_insured_amount) / 100;
            }
            else if (sum_insured_amount >= 10000 & sum_insured_amount < 40000)
            {
                decimal m = Convert.ToDecimal(1 - 0.1);
                myPremium = (myRate * m) * sum_insured_amount / 100;
            }
            else if (sum_insured_amount >= 40000)
            {
                decimal m = Convert.ToDecimal(1 - 0.3);
                myPremium = (myRate * m) * sum_insured_amount / 100;
            }
       

        //Premium Calculation by payment mode

        if (product_id == "T10" || product_id == "T3" || product_id == "T5")
        {
            if (pay_mode == 2) //Semi-annual old = 0.52
            {
                decimal old_factor = Convert.ToDecimal(0.52);
                decimal annual_premium = myPremium;
                myPremium = Convert.ToInt32(Math.Ceiling(myPremium * old_factor));
                return myPremium.ToString() + ',' + String.Format("{0:#0.##}", annual_premium);
            }
            else
            {
                return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);
            }
        }
        else
        {
            return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);
        }
       

    }

    //Calculate premium MRTA
    public static string CalculatePremiumMRTA(string product_id, int applicant_age, int sum_insured_amount, int coverage_period, int gender, int pay_mode)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_MRTA_Rate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName1 = new SqlParameter();
            paramName1.ParameterName = "@Product_ID";
            paramName1.Value = product_id;
            cmd.Parameters.Add(paramName1);

            SqlParameter paramName2 = new SqlParameter();
            paramName2.ParameterName = "@Age";
            paramName2.Value = applicant_age;
            cmd.Parameters.Add(paramName2);

            SqlParameter paramName3 = new SqlParameter();
            paramName3.ParameterName = "@Assure_Year";
            paramName3.Value = coverage_period;
            cmd.Parameters.Add(paramName3);

            SqlParameter paramName4 = new SqlParameter();
            paramName4.ParameterName = "@Gender";
            paramName4.Value = gender;
            cmd.Parameters.Add(paramName4);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {

                    myRate = Convert.ToDecimal(rdr.GetDouble(rdr.GetOrdinal("Rate")));

                }

            }
            con.Close();

        }

            //discount    
            if (sum_insured_amount >= 7500 & sum_insured_amount < 15000)
            {
                myPremium = (myRate * sum_insured_amount) / 100;
            }
            else if (sum_insured_amount >= 15000 & sum_insured_amount < 40000)
            {
                decimal m = Convert.ToDecimal(1 - 0.05);
                myPremium = ((myRate * m) * sum_insured_amount) / 100;
            }
            else if (sum_insured_amount >= 40000)
            {
                decimal m = Convert.ToDecimal(1 - 0.1);
                myPremium = ((myRate * m) * sum_insured_amount) / 100;
            }
      

        //Premium Calculation by payment mode
        if (product_id == "MRTA")
        {
            if (pay_mode == 2) //Semi-annual old = 0.52
            {
                decimal old_factor = Convert.ToDecimal(0.52);
                decimal annual_premium = myPremium;
                myPremium = Convert.ToInt32(Math.Ceiling(myPremium * old_factor));
                return myPremium.ToString() + ',' + String.Format("{0:#0.##}", annual_premium);
            }
            else
            {
                return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);
            }
        }
        else
        {
            return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);
        }

    }

    //Calculate premium type 1
    public static string CalculatePremiumTypeOne(string product_id, int applicant_age, int sum_insured_amount, int gender, int pay_mode, string plan_block)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Type_One_Rate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName1 = new SqlParameter();
            paramName1.ParameterName = "@Product_ID";
            paramName1.Value = product_id;
            cmd.Parameters.Add(paramName1);

            SqlParameter paramName2 = new SqlParameter();
            paramName2.ParameterName = "@Age";
            paramName2.Value = applicant_age;
            cmd.Parameters.Add(paramName2);

            SqlParameter paramName3 = new SqlParameter();
            paramName3.ParameterName = "@Gender";
            paramName3.Value = gender;
            cmd.Parameters.Add(paramName3);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    myRate = Convert.ToDecimal(rdr.GetDouble(rdr.GetOrdinal("Rate")));
                }
            }
            con.Close();
        }

       
        //Get Premium
        //discount for plan block 001 (Whole Life New), 002 (Term Life New)
        switch (plan_block)
        {
            //Case Term Life New
            case "002":
               
                    if (sum_insured_amount >= 10000 & sum_insured_amount < 40000)
                    {
                        decimal m = Convert.ToDecimal(1 - 0.05);
                        myPremium = ((myRate * m) * sum_insured_amount) / 1000;
                    }
                    else if (sum_insured_amount >= 40000)
                    {
                        decimal m = Convert.ToDecimal(1 - 0.1);
                        myPremium = ((myRate * m) * sum_insured_amount) / 1000;
                    }
                    else if (sum_insured_amount < 10000)
                    {
                        myPremium = (myRate * sum_insured_amount) / 1000; // no discount
                    }
                
                break;
            //Case whole Life New
            case "001":
               
                    if (sum_insured_amount >= 5000 & sum_insured_amount < 40000)
                    {
                        myPremium = ((myRate - 10) * sum_insured_amount) / 1000;
                    }
                    else if (sum_insured_amount >= 40000)
                    {
                        myPremium = ((myRate - 20) * sum_insured_amount) / 1000;
                    }
                    else if (sum_insured_amount < 5000)
                    {
                        myPremium = (myRate * sum_insured_amount) / 1000; // no discount
                    }
               
                break;
            default:
                myPremium = (myRate * sum_insured_amount) / 1000;
                break;
                
        }
        

        //Premium Calculation by payment mode
        return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);

    }

    //Calculate premium type two
    public static string CalculatePremiumTypeTwo(string product_id, int applicant_age, int sum_insured_amount, int coverage_period, int gender, int pay_mode, string plan_block)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Type_Two_Rate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName1 = new SqlParameter();
            paramName1.ParameterName = "@Product_ID";
            paramName1.Value = product_id;
            cmd.Parameters.Add(paramName1);

            SqlParameter paramName2 = new SqlParameter();
            paramName2.ParameterName = "@Age";
            paramName2.Value = applicant_age;
            cmd.Parameters.Add(paramName2);

            SqlParameter paramName3 = new SqlParameter();
            paramName3.ParameterName = "@Assure_Year";
            paramName3.Value = coverage_period;
            cmd.Parameters.Add(paramName3);

            SqlParameter paramName4 = new SqlParameter();
            paramName4.ParameterName = "@Gender";
            paramName4.Value = gender;
            cmd.Parameters.Add(paramName4);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                if (rdr.HasRows)
                {

                    myRate = Convert.ToDecimal(rdr.GetDouble(rdr.GetOrdinal("Rate")));

                }

            }
            con.Close();

        }


        //discount MRTA 12, 24, 36 
        switch (plan_block)
        {
            //case "006": //MRTA 12
            //case "007": //MRTA 24
            //case "008": //MRTA 32
            //    if (sum_insured_amount >= 15000 & sum_insured_amount < 40000)
            //    {
            //        decimal m = Convert.ToDecimal(1 - 0.05);
            //        myPremium = ((myRate * m) * sum_insured_amount) / 1000;
            //    }
            //    else if (sum_insured_amount >= 40000)
            //    {
            //        decimal m = Convert.ToDecimal(1 - 0.1);
            //        myPremium = ((myRate * m) * sum_insured_amount) / 1000;
            //    }
            //    else if (sum_insured_amount < 15000){ //no discount
            //        myPremium = (myRate * sum_insured_amount) / 10000;
            //    }
            //break;
            default: // Case others products
                myPremium = (myRate * sum_insured_amount) / 1000;
            break;
        }
        


        //Premium Calculation by payment mode
        return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);


    }

    //Calculate premium type 3 (ep: PP15/10) divide by 100
    public static string CalculatePremiumTypeThree(string product_id, int applicant_age, int sum_insured_amount, int gender, int pay_mode)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Type_One_Rate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName1 = new SqlParameter();
            paramName1.ParameterName = "@Product_ID";
            paramName1.Value = product_id;
            cmd.Parameters.Add(paramName1);

            SqlParameter paramName2 = new SqlParameter();
            paramName2.ParameterName = "@Age";
            paramName2.Value = applicant_age;
            cmd.Parameters.Add(paramName2);

            SqlParameter paramName3 = new SqlParameter();
            paramName3.ParameterName = "@Gender";
            paramName3.Value = gender;
            cmd.Parameters.Add(paramName3);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    myRate = Convert.ToDecimal(rdr.GetDouble(rdr.GetOrdinal("Rate")));
                }
            }
            con.Close();
        }

        //Get Premium
        myPremium = (myRate * sum_insured_amount) / 100;

        //Premium Calculation by payment mode
        return GetPremiumByPaymentMode(myPremium, pay_mode).ToString() + ',' + String.Format("{0:#0.##}", myPremium);

    }

    //Get premium by payment mode
    public static int GetPremiumByPaymentMode(decimal myPremium, int pay_mode)
    {
        int premium = 0;
        decimal factor = 0;

        //get factor by pay_mode
        bl_payment_mode payment_mode = new bl_payment_mode();

        payment_mode = da_payment_mode.GetPaymentModeByPayModeID(pay_mode);

        factor = Convert.ToDecimal(payment_mode.Factor);

        premium = Convert.ToInt32(Math.Ceiling(myPremium * factor));

        return premium;
    }

    //Calculate GTLI premium old rate (effective date < 01-02-2015)
    public static decimal CalculateGTLIPremium(System.DateTime dob, string gender, decimal sum_insured, string product_id, int number_of_staff, System.DateTime effective_date)
    {
        decimal premium = 0;
        decimal myRate = default(decimal);

        string connString = AppConfiguration.GetConnectionString();

        TimeSpan mytimespan = effective_date.Subtract(dob);
        int no_of_day = mytimespan.Days;

        //Get leap year count
        int number_of_leap_year = Helper.Get_Number_Of_Leap_Year(dob.Year, effective_date.Year);

        int age_band = Convert.ToInt32(Math.Floor(Convert.ToDecimal((no_of_day - number_of_leap_year) / 365)));

        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                //Get premium rate by age and product ID

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_Premium_Rate";
                myCommand.Parameters.AddWithValue("@Age_Band", age_band);
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@Product_ID", product_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        myRate = Convert.ToDecimal(myReader.GetString(myReader.GetOrdinal("Rate")));
                    }
                    myReader.Close();
                }

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();

                //Normal rate for any number of staffs

                if (number_of_staff < 20)
                {
                    premium = ((myRate * sum_insured) / 100) * Convert.ToDecimal(2.07); // 2.07 factor
                }
                else if (number_of_staff >= 20 & number_of_staff <= 29)
                {
                    premium = ((myRate * sum_insured) / 100) * Convert.ToDecimal(2.07);// 2.07 factor
                }
                else if (number_of_staff >= 30 & number_of_staff <= 39)
                {
                    premium = ((myRate * sum_insured) / 100) * Convert.ToDecimal(1.46);// 1.46 factor
                }
                else if (number_of_staff >= 40 & number_of_staff <= 49)
                {
                    premium = ((myRate * sum_insured) / 100) * Convert.ToDecimal(1.13);// 1.13 factor
                }
                else if (number_of_staff >= 50)
                {
                    premium = (myRate * sum_insured) / 100;
                    //Normal rate for 50 staffs or more
                }

                ////over age get new rate by: maneth
                //if (myRate == 0) {
                //    string newProductID = "";
                   
                //    //premium life
                //    if(product_id=="8") {
                 
                //        newProductID = "11";
                //    }
                //        //100plus
                //    else if (product_id == "10") {

                //        newProductID = "12";
                //    }
                //        //tpd
                //    else if (product_id == "9") {

                //        newProductID = "13";
                //    }

                //    premium = Calculation.CalculateGTLINewPremiumRate3(dob, gender, sum_insured, newProductID, number_of_staff, effective_date);
                    
                //}//end over age

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error calculating GTLI premium. Function [CalculateGTLIPremium], class [Calculation]. Details: " + ex.Message);
        }

        return Math.Ceiling(premium);
    }

    //Calculate GTLI premium New Rate
    public static decimal CalculateGTLINewPremiumRate(System.DateTime dob, string gender, decimal sum_insured, string product_id, int number_of_staff, System.DateTime effective_date)
    {
        decimal premium = 0;
        decimal myRate = default(decimal);

        string connString = AppConfiguration.GetConnectionString();

        TimeSpan mytimespan = effective_date.Subtract(dob);
        int no_of_day = mytimespan.Days;

        //Get leap year count
        int number_of_leap_year = Helper.Get_Number_Of_Leap_Year(dob.Year, effective_date.Year);

        int age_band = Convert.ToInt32(Math.Floor(Convert.ToDecimal((no_of_day - number_of_leap_year) / 365)));

        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                //Get premium rate by age and product ID

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_New_Premium_Rate";
                myCommand.Parameters.AddWithValue("@Age_Band", age_band);
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@Product_ID", product_id);
                myCommand.Parameters.AddWithValue("@Effective_Date", effective_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        myRate = Convert.ToDecimal(myReader.GetString(myReader.GetOrdinal("Rate")));
                    }
                    myReader.Close();
                }

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();

                //Normal rate for any number of staffs

                if (number_of_staff < 20)
                {
                    premium = ((myRate * sum_insured) / 1000) * Convert.ToDecimal(2.07);
                }
                else if (number_of_staff >= 20 & number_of_staff <= 29)
                {
                    premium = ((myRate * sum_insured) / 1000) * Convert.ToDecimal(2.07);
                }
                else if (number_of_staff >= 30 & number_of_staff <= 39)
                {
                    premium = ((myRate * sum_insured) / 1000) * Convert.ToDecimal(1.46);
                }
                else if (number_of_staff >= 40 & number_of_staff <= 49)
                {
                    premium = ((myRate * sum_insured) / 1000) * Convert.ToDecimal(1.13);
                }
                else if (number_of_staff >= 50)
                {
                    premium = (myRate * sum_insured) / 1000;
                    //Normal rate for 50 staffs or more
                }

                ////over age get new rate by: maneth
                //if (myRate == 0)
                //{
                //    string newProductID = "";

                //    //premium life
                //    if (product_id == "8")
                //    {

                //        newProductID = "11";
                //    }
                //    //100plus
                //    else if (product_id == "10")
                //    {

                //        newProductID = "12";
                //    }
                //    //tpd
                //    else if (product_id == "9")
                //    {

                //        newProductID = "13";
                //    }

                //    premium = Calculation.CalculateGTLINewPremiumRate3(dob, gender, sum_insured, newProductID, number_of_staff, effective_date);

                //}//end over age
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error calculating GTLI premium. Function [CalculateGTLINewPremiumRate], class [Calculation]. Details: " + ex.Message);
        }

        return Math.Ceiling(premium);
    }

    //Calculate GTLI premium New Rate 2
    public static decimal CalculateGTLINewPremiumRate2(System.DateTime dob, string gender, decimal sum_insured, string product_id, int number_of_staff, System.DateTime effective_date)
    {
        decimal premium = 0;
        decimal myRate = default(decimal);

        string connString = AppConfiguration.GetConnectionString();

        TimeSpan mytimespan = effective_date.Subtract(dob);
        int no_of_day = mytimespan.Days;

        //Get leap year count
        int number_of_leap_year = Helper.Get_Number_Of_Leap_Year(dob.Year, effective_date.Year);

        int age_band = Convert.ToInt32(Math.Floor(Convert.ToDecimal((no_of_day - number_of_leap_year) / 365)));

        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                //Get premium rate by age and product ID

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_New_Premium_Rate_2";
                myCommand.Parameters.AddWithValue("@Age_Band", age_band);
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@Product_ID", product_id);
                myCommand.Parameters.AddWithValue("@Effective_Date", effective_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        myRate = Convert.ToDecimal(myReader.GetString(myReader.GetOrdinal("Rate")));
                    }
                    myReader.Close();
                }

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();


                //Discount for number of staffs
                //if (number_of_staff >= 10 && number_of_staff < 51)
                //{
                //    //Factor
                //    if (number_of_staff < 15)
                //    {
                //        premium = ((myRate * sum_insured) / 1000) * Convert.ToDecimal(1.5);
                //    }
                //    else
                //    {
                //        premium = ((myRate * sum_insured) / 1000);
                //    }
                //}
                //else if (number_of_staff >= 51 & number_of_staff <= 100)
                //{
                //    premium = ((myRate * sum_insured) / 1000) - (((myRate * sum_insured) / 1000) * Convert.ToDecimal(0.05));

                //    //No Discount
                //   // premium = ((myRate * sum_insured) / 1000); 
                //}
                //else if (number_of_staff > 100)
                //{
                //    premium = ((myRate * sum_insured) / 1000) - (((myRate * sum_insured) / 1000) * Convert.ToDecimal(0.1));
                //}

                if (number_of_staff >= 10 && number_of_staff < 51)
                {
                   
                        premium = ((myRate * sum_insured) / 1000);
                  
                }
                else if (number_of_staff >= 51 & number_of_staff <= 100)
                {
                    premium = ((myRate * sum_insured) / 1000);

                    //No Discount
                    // premium = ((myRate * sum_insured) / 1000); 
                }
                else if (number_of_staff > 100)
                {
                    premium = ((myRate * sum_insured) / 1000);
                }

                ////over age get new rate by: maneth
                //if (myRate == 0)
                //{
                //    string newProductID = "";

                //    //premium life
                //    if (product_id == "8")
                //    {

                //        newProductID = "11";
                //    }
                //    //100plus
                //    else if (product_id == "10")
                //    {

                //        newProductID = "12";
                //    }
                //    //tpd
                //    else if (product_id == "9")
                //    {

                //        newProductID = "13";
                //    }

                //    premium = Calculation.CalculateGTLINewPremiumRate3(dob, gender, sum_insured, newProductID, number_of_staff, effective_date);

                //}//end over age
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error calculating GTLI premium. Function [CalculateGTLINewPremiumRate2], class [Calculation]. Details: " + ex.Message);
        }

        return Math.Ceiling(premium);
    }

    //Calculate GTLI premium New Rate 3: effective from 12/april/2016
    public static decimal CalculateGTLINewPremiumRate3(System.DateTime dob, string gender, decimal sum_insured, string product_id, int number_of_staff, System.DateTime effective_date)
    {
        decimal premium = 0;
        decimal myRate = default(decimal);

        string connString = AppConfiguration.GetConnectionString();

        TimeSpan mytimespan = effective_date.Subtract(dob);
        int no_of_day = mytimespan.Days;

        //Get leap year count
        int number_of_leap_year = Helper.Get_Number_Of_Leap_Year(dob.Year, effective_date.Year);

        int age_band = Convert.ToInt32(Math.Floor(Convert.ToDecimal((no_of_day - number_of_leap_year) / 365)));

        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                //Get premium rate by age and product ID

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_GTLI_New_Premium_Rate_3";
                myCommand.Parameters.AddWithValue("@Age_Band", age_band);
                myCommand.Parameters.AddWithValue("@Gender", gender);
                myCommand.Parameters.AddWithValue("@Product_ID", product_id);
                myCommand.Parameters.AddWithValue("@Effective_Date", effective_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        myRate = Convert.ToDecimal(myReader.GetString(myReader.GetOrdinal("Rate")));
                    }
                    myReader.Close();
                }

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myConnection.Close();


                if (number_of_staff >= 20 && number_of_staff < 51)
                {

                    premium = ((myRate * sum_insured) / 1000);

                }
                else if (number_of_staff >= 51 & number_of_staff <= 100)
                {
                    premium = ((myRate * sum_insured) / 1000);

                }
                else if (number_of_staff > 100)
                {
                    premium = ((myRate * sum_insured) / 1000);
                }

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error calculating GTLI premium. Function [CalculateGTLINewPremiumRate3], class [Calculation]. Details: " + ex.Message);
        }

        return Math.Ceiling(premium);
    }

    //Get Leap Year Count
    public static int Get_Number_Of_Leap_Year(int dob_year, int this_year)
    {

        int number_of_year = 0;
        int i = dob_year;


        while ((i <= this_year))
        {
            if (((i % 4 == 0) && (i % 100 != 0) || (i % 400 == 0)))
            {
                number_of_year += 1;
            }

            i += 1;
        }

        return number_of_year;
    }

    //Get Customer Age
    public static int Culculate_Customer_Age(string strdob, DateTime compare_date)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        int customer_age = 0;
        try
        {
            DateTime dob = Convert.ToDateTime(strdob, dtfi);

            TimeSpan mytimespan = compare_date.Subtract(dob);
            int no_of_day = mytimespan.Days;

            //Get leap year count
            int number_of_leap_year = Get_Number_Of_Leap_Year(dob.Year, compare_date.Year);

            double result = (Convert.ToDouble(no_of_day) - Convert.ToDouble(number_of_leap_year)) / 365;

            if (dob.Month.Equals(compare_date.Month) && dob.Day.Equals(compare_date.Day))
            {
                //round .99 age
                double round_result = Math.Ceiling(result);

                //minus rould result 0.1
                double sub_result = round_result - 0.02;

                //if result ~ #.99 then round up
                if (result >= sub_result)
                {
                    result = Math.Ceiling(result);
                }
            }
            else
            {
                result = Math.Floor(result);
            }


            customer_age = Convert.ToInt32(result);
        }
        catch (Exception ex)
        {
            customer_age = 0;
            Log.AddExceptionToLog("Error function [Culculate_Customer_Age(string strdob, DateTime compare_date)] in class [Calculation], detail: " + ex.Message);
        }
        return customer_age;


    }

    //Get MRTA Assure Year
    public static int Culculate_MRTA_Assure_Year(DateTime current_date, DateTime loan_effective_date, int term_loan)
    {
        int assure_year = 0;
        int no_of_day = 0;

        //Get leap year count
        if (current_date.CompareTo(loan_effective_date) == 1)
        {
            TimeSpan mytimespan = current_date.Subtract(loan_effective_date);

            no_of_day = mytimespan.Days;

            decimal result = (no_of_day) / 365;

            int substract_year = Convert.ToInt32(Math.Floor(result));

            assure_year = term_loan - substract_year;
        }
        else
        {
            assure_year = term_loan;
        }


        return assure_year;

    }

    //Function to find Used Days (GTLI)
    public static int CalculateUsedDays(System.DateTime sale_date, System.DateTime date_of_modify)
    {
        TimeSpan mytimespan = date_of_modify.Subtract(sale_date);
        int used_days = mytimespan.Days;
        return used_days;
    }

    //Function to calculate total points for Point Reward
    public static double CalculateTotalPoint(double premium, int point)
    {
        double total_point = (premium * point) / 100;
        return Convert.ToDouble(String.Format("{0:#0.##}", total_point));
    }

    //Function to calculate total cash premium from points for Point Reward
    public static double CalculateTotalPremium(double total_point, int point)
    {
        double total_premium = (total_point * 100) / point;
        return Convert.ToDouble(String.Format("{0:#0.##}", total_point));
    }

    //Get Customer Age Micro
    public static int Culculate_Customer_Age_Micro(DateTime dob, DateTime compare_date)
    {

        TimeSpan mytimespan = compare_date.Subtract(dob);
        int no_of_day = mytimespan.Days;

        //Get leap year count
        int number_of_leap_year = Get_Number_Of_Leap_Year(dob.Year, compare_date.Year);

        double result = (Convert.ToDouble(no_of_day) - Convert.ToDouble(number_of_leap_year)) / 365;

        if (dob.Month.Equals(compare_date.Month) && dob.Day.Equals(compare_date.Day))
        {
            //round .99 age
            double round_result = Math.Ceiling(result);

            //minus rould result 0.1
            double sub_result = round_result - 0.02;

            //if result ~ #.99 then round up
            if (result >= sub_result)
            {
                result = Math.Ceiling(result);
            }
        }
        else
        {
            result = Math.Floor(result);
        }

        int customer_age = Convert.ToInt32(result);

        

        return customer_age;

    }

    //Get GTLI Premium tax amount
    public static decimal Calculate_GTLI_Premium_Tax(decimal original_premium, decimal discount)
    {
        decimal tax_amount = Convert.ToDecimal(((original_premium - discount) * Convert.ToDecimal(0.05)).ToString("N2"));

        return tax_amount;
    }

    //Get GTLI Premium After Discount & Tax
    public static decimal Calculate_GTLI_Premium_After_Discount_And_Tax(decimal original_premium, decimal discount, decimal tax_amount)
    {              
        decimal premium_after_tax = (original_premium - discount) + tax_amount;

        return premium_after_tax;
    }

    public static DateTime GetNext_Due(DateTime next_due, DateTime due_date, DateTime effective_date)
    {
        DateTime result = DateTime.Now;
        try
        {

            result = next_due;

            int i = next_due.Year, checking_day_per_month = 0, add_days = 0;

            if (((i % 4 == 0) && (i % 100 != 0) || (i % 400 == 0))) // Leap Year, 29 last day of Feb
            {
                if (next_due.Month == 2)
                {
                    if (next_due.Day >= 29)
                    {
                        if (due_date.Day == 28)
                        {
                            result = next_due.AddDays(1);
                        }
                        else { result = next_due; }
                    }
                    else { result = next_due; }
                }
                else if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12 || next_due.Month == 10)
                {
                    if (next_due.Day >= 31)
                    {
                        if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-1);
                        }
                        else { result = next_due; }
                    }
                    else
                    {
                        checking_day_per_month = DateTime.DaysInMonth(next_due.Year, next_due.Month);

                        if (effective_date.Day > next_due.Day)
                        {
                            add_days = effective_date.Day - next_due.Day;

                            if (checking_day_per_month == next_due.Day)
                            {
                                result = next_due;
                            }
                            else
                            {
                                result = next_due.AddDays(add_days); //due_date = next_due.AddDays(1);
                            }
                        }
                        else if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-(next_due.Day - effective_date.Day));
                        }
                        else
                        {
                            result = next_due;
                        }
                    }
                }
                else
                {
                    result = next_due;
                }
            }
            else
            {
                if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12 || next_due.Month == 10)
                {
                    if (next_due.Day >= 31)
                    {
                        if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-1);
                        }
                        else { result = next_due; }
                    }
                    else
                    {
                        checking_day_per_month = DateTime.DaysInMonth(next_due.Year, next_due.Month);

                        if (effective_date.Day > next_due.Day)
                        {
                            add_days = effective_date.Day - next_due.Day;

                            if (checking_day_per_month == next_due.Day)
                            {
                                result = next_due;
                            }
                            else
                            {
                                result = next_due.AddDays(add_days); //due_date = next_due.AddDays(1);
                            }
                        }
                        else if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-(next_due.Day - effective_date.Day));
                        }
                        else
                        {
                            result = next_due;
                        }
                    }
                }
                else
                {
                    result = next_due;
                }
            }
        }
        catch (Exception ex)
        {
            //write error to log
            Log.AddExceptionToLog("Error: class [da_Calculation], function [GetNext_Due]. Details: " + ex.Message);
        }

        return result;
    }

#region miscro product SO
    /// <summary>
    /// Return [annualy premium , premium by pay mode]
    /// </summary>
    /// <param name="productID"></param>
    /// <param name="gender"></param>
    /// <param name="age"></param>
    /// <param name="SumAssured"></param>
    /// <param name="paymode"></param>
    /// <returns></returns>
    public static double[] GetMicroProducPremium(string productID, int gender, int age, double SumAssured, int paymode)
    {
        #region old calc
        /*
        double[] result = new double[] { 0, 0 };
        int divide = -1;
        bl_micro_product_rate basic;
        string[] splitBasic = new string[] { };
        string[] splitAnnual = new string[] { };
        double basic1, basic2,  basic2LastDegitCheck;
        double annualPremium, PremiumByPaymode;
        string basic2stDegit, basic2LastDegit;
        try
        {
            switch (paymode)
            {
                case 1:
                    divide = 1;
                    break;
                case 2:
                    divide = 2;
                    break;
                case 3:
                    divide = 4;
                    break;
                case 4:
                    divide = 12;
                    break;
                default:
                    divide = -1;
                    break;

            }
            basic = da_micro_product_rate.GetProductRate(productID, gender, age, SumAssured);
            if (basic.PRODUCT_ID != null)
            {
                if (divide > 0)
                {
                    if (basic.RATE_TYPE.ToUpper() == "VALUE")
                    {
                       // annualPremium = basic.RATE;
                        if (SumAssured > basic.RATE_PER)
                        {
                            int t = Convert.ToInt32(SumAssured / basic.RATE_PER);
                            annualPremium = basic.RATE * t;
                        }
                        else
                        {
                            annualPremium = basic.RATE;
                        }
                    }
                    else
                    {
                        annualPremium = (basic.RATE * SumAssured) / basic.RATE_PER;
                    }
                   

                    if (divide != 1)
                    {
                        // basic_premium = Math.Round((rateByPaymode * SA) / basic.RATE_PER, 3);
                        PremiumByPaymode = Math.Round(annualPremium / divide, 3, MidpointRounding.AwayFromZero);
                        splitBasic = PremiumByPaymode.ToString().Split('.');
                        basic1 = Convert.ToDouble(splitBasic[0]);
                        basic2 = Convert.ToDouble(splitBasic[1]);
                        basic2stDegit = basic2.ToString().Substring(0, 1);
                        basic2LastDegit = basic2.ToString().Substring(basic2stDegit.Length, basic2.ToString().Length - basic2stDegit.Length);
                        basic2LastDegitCheck = Convert.ToDouble("0.0" + basic2LastDegit.ToString());

                        if (basic2LastDegitCheck >= 0.05)
                        {
                            PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.10;
                        }
                        else
                        {
                            PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.05;
                        }
                        PremiumByPaymode = Math.Round(PremiumByPaymode, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        PremiumByPaymode = annualPremium;
                    }
                    result = new double[] { annualPremium, PremiumByPaymode };
                }
            }

            
        }
        catch (Exception ex)
        {
            result = new double[] { 0, 0 };
            Log.AddExceptionToLog("Error function [GetMicroProducPremium(string productID, int gender, int age, double SumAssured)] in class [Calculation], detail:" + ex.Message + "=>" + ex.StackTrace);
        }

        return result;
        */
        #endregion

        double[] result = new double[] { 0, 0 };
        int divide = -1;
        bl_micro_product_rate basic;
        string[] splitBasic = new string[] { };
        string[] splitAnnual = new string[] { };
        double basic1, basic2, basic2LastDegitCheck;
        double annualPremium, PremiumByPaymode;
        string basic2stDegit, basic2LastDegit;
        try
        {
            switch (paymode)
            {
                case 1:
                    divide = 1;
                    break;
                case 2:
                    divide = 2;
                    break;
                case 3:
                    divide = 4;
                    break;
                case 4:
                    divide = 12;
                    break;
                default:
                    divide = -1;
                    break;

            }
            basic = da_micro_product_rate.GetProductRate(productID, gender, age, SumAssured, paymode);
            if (basic.PRODUCT_ID != null)
            {
                if (divide > 0)
                {
                    if (basic.RATE_TYPE.ToUpper() == "VALUE")
                    {
                        // annualPremium = basic.RATE;
                        if (SumAssured > basic.RATE_PER)
                        {
                            int t = Convert.ToInt32(SumAssured / basic.RATE_PER);
                            PremiumByPaymode = basic.RATE * t;

                        }
                        else
                        {
                            PremiumByPaymode = basic.RATE;
                        }
                        annualPremium = PremiumByPaymode * divide;
                    }
                    else if (basic.RATE_TYPE.ToString() == "RATE")
                    {
                        PremiumByPaymode = (basic.RATE * SumAssured) / basic.RATE_PER;

                        if (divide != 1)
                        {

                            PremiumByPaymode = Math.Round(PremiumByPaymode, 3, MidpointRounding.AwayFromZero);
                            splitBasic = PremiumByPaymode.ToString().Split('.');
                            basic1 = Convert.ToDouble(splitBasic[0]);
                            basic2 = Convert.ToDouble(splitBasic[1]);
                            basic2stDegit = basic2.ToString().Substring(0, 1);
                            basic2LastDegit = basic2.ToString().Substring(basic2stDegit.Length, basic2.ToString().Length - basic2stDegit.Length);
                            basic2LastDegitCheck = Convert.ToDouble("0.0" + basic2LastDegit.ToString());

                            if (basic2LastDegitCheck >= 0.05)
                            {
                                PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.10;
                            }
                            else
                            {
                                PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.05;
                            }
                            PremiumByPaymode = Math.Round(PremiumByPaymode, 2, MidpointRounding.AwayFromZero);
                        }

                        annualPremium = Math.Round(PremiumByPaymode, 2, MidpointRounding.AwayFromZero) * divide;
                    }else/*percentage*/
                    {
                        PremiumByPaymode = Math.Round(SumAssured * (basic.RATE / basic.RATE_PER), 3, MidpointRounding.AwayFromZero);
                        annualPremium = Math.Round(PremiumByPaymode * (double)divide, 3, MidpointRounding.AwayFromZero);
                    }

                    result = new double[] { annualPremium, PremiumByPaymode };
                }
            }


        }
        catch (Exception ex)
        {
            result = new double[] { 0, 0 };
            Log.AddExceptionToLog("Error function [GetMicroProducPremium(string productID, int gender, int age, double SumAssured)] in class [Calculation], detail:" + ex.Message + "=>" + ex.StackTrace);
        }

        return result;

    }
    public static double[] GetMicroProductRiderPremium(string productID, int gender, int age, double SumAssured, int paymode)
    {
#region old calc
        /*
        double[] result = new double[] { 0, 0 };
        int divide = -1;
      
        string[] splitBasic = new string[] { };
        
        double basic1, basic2, basic2LastDegitCheck;
        double annualPremium, PremiumByPaymode;
        string basic2stDegit, basic2LastDegit;
        bl_micro_product_rider_rate rate;
        try
        {
          
            switch (paymode)
            {
                case 1:
                    divide = 1;
                    break;
                case 2:
                    divide = 2;
                    break;
                case 3:
                    divide = 4;
                    break;
                case 4:
                    divide = 12;
                    break;
                default:
                    divide = -1;
                    break;

            }
            rate = da_micro_product_rider_rate.GetProductRate(productID, gender, age, SumAssured);
            if (rate.PRODUCT_ID != null)
            {
                if (divide > 0)
                {
                    if (rate.RATE_TYPE == "VALUE")
                    {
                        if (SumAssured > rate.RATE_PER)
                        {
                            int t = Convert.ToInt32(SumAssured / rate.RATE_PER);
                            annualPremium = rate.RATE * t;
                        }
                        else
                        {
                            annualPremium = rate.RATE;
                        }

                       // annualPremium = rate.RATE;
                    }
                    else
                    {
                        annualPremium = (rate.RATE * SumAssured) / rate.RATE_PER;
                    }
                    if (divide != 1)
                    {
                        // basic_premium = Math.Round((rateByPaymode * SA) / basic.RATE_PER, 3);
                        PremiumByPaymode = Math.Round(annualPremium / divide, 3, MidpointRounding.AwayFromZero);
                        splitBasic = PremiumByPaymode.ToString().Split('.');
                        basic1 = Convert.ToDouble(splitBasic[0]);
                        basic2 = Convert.ToDouble(splitBasic[1]);
                        basic2stDegit = basic2.ToString().Substring(0, 1);
                        basic2LastDegit = basic2.ToString().Substring(basic2stDegit.Length, basic2.ToString().Length - basic2stDegit.Length);
                        basic2LastDegitCheck = Convert.ToDouble("0.0" + basic2LastDegit.ToString());

                        if (basic2LastDegitCheck >= 0.05)
                        {
                            PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.10;
                        }
                        else
                        {
                            PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.05;
                        }
                        PremiumByPaymode = Math.Round(PremiumByPaymode, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        PremiumByPaymode = annualPremium;
                    }
                    result = new double[] { annualPremium, PremiumByPaymode };
                }
            }

        }
        catch (Exception ex)
        {
            result = new double[] { 0, 0 };
            Log.AddExceptionToLog("Error function [GetMicroProductRiderPremium(string productID, int gender, int age, double SumAssured)] in class [Calculation], detail:" + ex.Message + "=>" + ex.StackTrace);
        }

        return result;
        */
#endregion

        double[] result = new double[] { 0, 0 };
        int divide = -1;

        string[] splitBasic = new string[] { };

        double basic1, basic2, basic2LastDegitCheck;
        double annualPremium, PremiumByPaymode;
        string basic2stDegit, basic2LastDegit;
        bl_micro_product_rider_rate rate;
        try
        {

            switch (paymode)
            {
                case 1:
                    divide = 1;
                    break;
                case 2:
                    divide = 2;
                    break;
                case 3:
                    divide = 4;
                    break;
                case 4:
                    divide = 12;
                    break;
                default:
                    divide = -1;
                    break;

            }
            rate = da_micro_product_rider_rate.GetProductRate(productID, gender, age, SumAssured, paymode);
            if (rate.PRODUCT_ID != null)
            {
                if (divide > 0)
                {
                    if (rate.RATE_TYPE == "VALUE")
                    {
                        if (SumAssured > rate.RATE_PER)
                        {
                            int t = Convert.ToInt32(SumAssured / rate.RATE_PER);
                            PremiumByPaymode = rate.RATE * t;
                        }
                        else
                        {
                            PremiumByPaymode = rate.RATE;
                        }
                        annualPremium = PremiumByPaymode * divide;
                    }
                    else if (rate.RATE_TYPE.ToUpper() == "RATE")
                    {
                        PremiumByPaymode = (rate.RATE * SumAssured) / rate.RATE_PER;
                        if (divide != 1)
                        {
                            PremiumByPaymode = Math.Round(PremiumByPaymode, 3, MidpointRounding.AwayFromZero);
                            splitBasic = PremiumByPaymode.ToString().Split('.');
                            basic1 = Convert.ToDouble(splitBasic[0]);
                            basic2 = Convert.ToDouble(splitBasic[1]);
                            basic2stDegit = basic2.ToString().Substring(0, 1);
                            basic2LastDegit = basic2.ToString().Substring(basic2stDegit.Length, basic2.ToString().Length - basic2stDegit.Length);
                            basic2LastDegitCheck = Convert.ToDouble("0.0" + basic2LastDegit.ToString());

                            if (basic2LastDegitCheck >= 0.05)
                            {
                                PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.10;
                            }
                            else
                            {
                                PremiumByPaymode = Convert.ToDouble(basic1.ToString() + "." + basic2stDegit.ToString()) + 0.05;
                            }
                            PremiumByPaymode = Math.Round(PremiumByPaymode, 2, MidpointRounding.AwayFromZero);
                        }

                        annualPremium = Math.Round(PremiumByPaymode, 2, MidpointRounding.AwayFromZero) * divide;
                    }
                    else {
                        PremiumByPaymode = Math.Round(SumAssured * (rate.RATE / rate.RATE_PER), 3, MidpointRounding.AwayFromZero);
                        annualPremium = Math.Round(PremiumByPaymode * (double)divide, 3, MidpointRounding.AwayFromZero);
                    }
                    result = new double[] { annualPremium, PremiumByPaymode };
                }
            }

        }
        catch (Exception ex)
        {
            result = new double[] { 0, 0 };
            Log.AddExceptionToLog("Error function [GetMicroProductRiderPremium(string productID, int gender, int age, double SumAssured)] in class [Calculation], detail:" + ex.Message + "=>" + ex.StackTrace);
        }

        return result;
    }
#endregion

    #endregion
    #region
    public static DateTime CheckDayNextDueWithEffectiveDay(DateTime next_due, string policy_id)
    {
        DateTime result = DateTime.Now;
        try
        {
            DateTime effective_date = da_effective_date.getEffectiveDate(policy_id);

            result = next_due;

            int i = next_due.Year, checking_day_per_month = 0, add_days = 0;

            if (((i % 4 == 0) && (i % 100 != 0) || (i % 400 == 0))) // Leap Year, 29 last day of Feb
            {
                if (next_due.Month == 2)
                {
                    if (next_due.Day >= 29)
                    {
                        if (next_due.Day == 28)
                        {
                            result = next_due.AddDays(1);
                        }
                        else { result = next_due; }
                    }
                    else { result = next_due; }
                }
                else if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12 || next_due.Month == 10)
                {
                    if (next_due.Day >= 31)
                    {
                        if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-1);
                        }
                        else { result = next_due; }
                    }
                    else
                    {
                        checking_day_per_month = DateTime.DaysInMonth(next_due.Year, next_due.Month);

                        if (effective_date.Day > next_due.Day)
                        {
                            add_days = effective_date.Day - next_due.Day;

                            if (checking_day_per_month == next_due.Day)
                            {
                                result = next_due;
                            }
                            else
                            {
                                result = next_due.AddDays(add_days); //due_date = next_due.AddDays(1);
                            }
                        }
                        else if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-(next_due.Day - effective_date.Day));
                        }
                        else
                        {
                            result = next_due;
                        }
                    }
                }
                else
                {
                    result = next_due;
                }
            }
            else
            {
                if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12 || next_due.Month == 10)
                {
                    if (next_due.Day >= 31)
                    {
                        if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-1);
                        }
                        else { result = next_due; }
                    }
                    else
                    {
                        checking_day_per_month = DateTime.DaysInMonth(next_due.Year, next_due.Month);

                        if (effective_date.Day > next_due.Day)
                        {
                            add_days = effective_date.Day - next_due.Day;

                            if (checking_day_per_month == next_due.Day)
                            {
                                result = next_due;
                            }
                            else
                            {
                                result = next_due.AddDays(add_days); //due_date = next_due.AddDays(1);
                            }
                        }
                        else if (effective_date.Day < next_due.Day)
                        {
                            result = next_due.AddDays(-(next_due.Day - effective_date.Day));
                        }
                        else
                        {
                            result = next_due;
                        }
                    }
                }
                else
                {
                    result = next_due;
                }
            }
        }
        catch (Exception ex)
        {
            //write error to log
            Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [CheckDayNextDueWithEffectiveDay]. Details: " + ex.Message);
        }

        return result;
    }

    public static DateTime Culculate_Maturity_Date(DateTime effective_date, DateTime policy_pay_mode_date)
    {
        DateTime result = DateTime.Now;
        try
        {


        }
        catch (Exception ex)
        {
            //write error to log
            Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [CheckDayNextDueWithEffectiveDay]. Details: " + ex.Message);
        }

        return result;
    }

    public static string CalculatePremiumCreditLife24(string product_id, int applicant_age, int sum_insured_amount, int gender, int pay_mode, int assured_year)
    {
        double premium = 0;
        try
        {
            int calculation_age = 0;
            double rate = 0;
            //age band
            if (applicant_age >= 18 && applicant_age <= 39)
            {
                calculation_age = 18;
            }
            else if (applicant_age >= 40 && applicant_age <= 60)
            {
                calculation_age = 40;
            }

            //get rate
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_CREDIT_LIFE_24_RATE", new string[,] { { "@PRODUCT_ID", product_id},
                                                                                                           {"@ASSURED_YEAR", assured_year+ ""},
                                                                                                           {"@GENDER", gender+""}, 
                                                                                                           {"@PAY_MODE", pay_mode+""}, 
                                                                                                           {"@AGE", calculation_age+""} });
            foreach (DataRow row in tbl.Rows)
            {
                rate = Convert.ToDouble(row["rate"].ToString());
            }

            //Discount base on Sum Assured
            double discount = 0;
            if (sum_insured_amount >= 25000 && sum_insured_amount <= 49999)
            { 
                //discount 5%
                
                //premium = (rate * sum_insured_amount) / 1000 ;
                //discount = premium * 0.05;
                //premium = premium - discount;
                /*Modified by maneth @24Jun2020*/
                premium = (rate * sum_insured_amount) / 1000;
                premium = Math.Ceiling(premium);
                discount = Math.Floor( premium * 0.05);
                premium = premium - discount;

            }
            else if (sum_insured_amount >= 50000 && sum_insured_amount <= 150000)
            { 
                //discount 10%
                //premium = (rate * sum_insured_amount) / 1000;
                //discount = premium * 0.01;
                //premium = premium - discount;
                /*Modified by maneth @24Jun2020*/
                premium = (rate * sum_insured_amount) / 1000;
                premium = Math.Ceiling(premium);
                discount = Math.Floor( premium * 0.1);
                premium = premium - discount;
            }
            else 
            { 
                //discount 0%
                premium = (rate * sum_insured_amount) / 1000;
               
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [CalculatePremiumCreditLife24] in class [Calculation], Detial: " + ex.Message);
        }

        return GetPremiumByPaymentMode(Convert.ToDecimal( premium), pay_mode).ToString() + "," + String.Format("{0:#0.##}",premium);

    }

    #endregion
}