using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_product_premium_report
/// </summary>
public class da_product_premium_report
{
    private static da_product_premium_report mytitle = null;
    public da_product_premium_report()
	{
        if (mytitle == null)
        {
            mytitle = new da_product_premium_report();
		}
	}          

    //Get ACC product premium details list between dates
    public static List<bl_product_premium_report> GetACCProductPremiumDetailsByDates(DateTime from_date, DateTime to_date)
    {
        List<bl_product_premium_report> product_premium_list = new List<bl_product_premium_report>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_ACC_Premium_Report_By_Product";
          
                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_product_premium_report product_premium = new bl_product_premium_report();

                        product_premium.Policy_ID = myReader.GetString(myReader.GetOrdinal("Policy_ID"));
                        product_premium.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        product_premium.OR_No = myReader.GetString(myReader.GetOrdinal("Receipt_No"));
                        product_premium.Amount_Paid = myReader.GetDouble(myReader.GetOrdinal("OR_Amount"));
                        product_premium.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        product_premium.Pay_Date = myReader.GetDateTime(myReader.GetOrdinal("OR_Date"));
                        product_premium.Product = myReader.GetString(myReader.GetOrdinal("Product_Name"));
                        product_premium.Pay_Mode = myReader.GetString(myReader.GetOrdinal("Pay_Mode"));
                        product_premium.Product_ID = myReader.GetString(myReader.GetOrdinal("Product_ID"));
                                                                                 

                        product_premium_list.Add(product_premium);

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
            Log.AddExceptionToLog("Error in function [GetACCProductPremiumDetailsByDates] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return product_premium_list;
    }

    //Get ACC product premium pay details list between dates
    public static List<bl_prem_pay_details> GetACCProductPremiumPayDetailsByDates(string official_receipt_number, string policy_number)
    {
        List<bl_prem_pay_details> product_premium_pay_list = new List<bl_prem_pay_details>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_ACC_Premium_Pay";

                myCommand.Parameters.AddWithValue("@Receipt_No", official_receipt_number);
                myCommand.Parameters.AddWithValue("@Policy_Number", policy_number);
              
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_prem_pay_details product_premium_pay = new bl_prem_pay_details();

                        product_premium_pay.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("Prem_SI"));
                        product_premium_pay.Amount = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        product_premium_pay.Prem_Year = myReader.GetInt32(myReader.GetOrdinal("Prem_Year"));
                        product_premium_pay.Prem_Lot = myReader.GetInt32(myReader.GetOrdinal("Prem_Lot"));
                     
                        product_premium_pay_list.Add(product_premium_pay);

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
            Log.AddExceptionToLog("Error in function [GetACCProductPremiumPayDetailsByDates] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return product_premium_pay_list;
    }

    //Get ACC GTLI premium details from prem pay list between dates
    public static List<bl_product_premium_report> GetACCGTLIPremiumDetailsFromPremPayByDates(DateTime from_date, DateTime to_date)
    {
        List<bl_product_premium_report> group_prem_pay_list = new List<bl_product_premium_report>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_ACC_GTLI_Prem_Pay_By_Dates";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_product_premium_report group_prem_pay = new bl_product_premium_report();

                        group_prem_pay.Prem_Pay_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_Prem_Pay_ID"));
                        group_prem_pay.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        group_prem_pay.Amount_Paid = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_pay.AP = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_pay.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        group_prem_pay.Pay_Date = myReader.GetDateTime(myReader.GetOrdinal("Pay_Date"));
                        group_prem_pay.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured")) * myReader.GetInt32(myReader.GetOrdinal("Transaction_Staff_Number")); 
                        group_prem_pay.Product = myReader.GetString(myReader.GetOrdinal("Product"));
                        group_prem_pay.Pay_Mode = myReader.GetString(myReader.GetOrdinal("Mode"));
                        group_prem_pay.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                        group_prem_pay.Prem_Year = myReader.GetInt32(myReader.GetOrdinal("Prem_Year"));
                        group_prem_pay.Prem_Lot = myReader.GetInt32(myReader.GetOrdinal("Prem_Lot"));
                        group_prem_pay_list.Add(group_prem_pay);

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
            Log.AddExceptionToLog("Error in function [GetACCGTLIPremiumDetailsFromPremPayByDates] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return group_prem_pay_list;
    }

    //Get ACC GTLI premium details from prem return list between dates
    public static List<bl_product_premium_report> GetACCGTLIPremiumDetailsFromPremReturnByDates(DateTime from_date, DateTime to_date)
    {
        List<bl_product_premium_report> group_prem_return_list = new List<bl_product_premium_report>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_ACC_GTLI_Prem_Return_By_Dates";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_product_premium_report group_prem_return = new bl_product_premium_report();

                        group_prem_return.Prem_Return_ID = myReader.GetString(myReader.GetOrdinal("GTLI_Policy_Prem_Return_ID"));
                        group_prem_return.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        group_prem_return.Amount_Paid = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_return.AP = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_return.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        group_prem_return.Pay_Date = myReader.GetDateTime(myReader.GetOrdinal("Return_Date"));
                        group_prem_return.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                        group_prem_return.Product = myReader.GetString(myReader.GetOrdinal("Product"));
                        group_prem_return.Pay_Mode = myReader.GetString(myReader.GetOrdinal("Mode"));
                        group_prem_return.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                        group_prem_return.Prem_Year = myReader.GetInt32(myReader.GetOrdinal("Prem_Year"));
                        group_prem_return.Prem_Lot = myReader.GetInt32(myReader.GetOrdinal("Prem_Lot"));
                        group_prem_return_list.Add(group_prem_return);

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
            Log.AddExceptionToLog("Error in function [GetACCGTLIPremiumDetailsFromPremReturnByDates] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return group_prem_return_list;
    }

    //Get PMA product premium details list between dates
    public static List<bl_product_premium_report> GetPMAProductPremiumDetailsByDates(DateTime from_date, DateTime to_date)
    {
        List<bl_product_premium_report> product_premium_list = new List<bl_product_premium_report>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_PMA_Premium_Report_By_Product";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_product_premium_report product_premium = new bl_product_premium_report();
                        product_premium.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        product_premium.Amount_Paid = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        product_premium.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        product_premium.Pay_Date = myReader.GetDateTime(myReader.GetOrdinal("Pay_Date"));
                        product_premium.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("SI"));
                        product_premium.Product = myReader.GetString(myReader.GetOrdinal("Product_Name"));
                        product_premium.Pay_Mode = myReader.GetString(myReader.GetOrdinal("Pay_Mode"));
                        product_premium.Product_ID = myReader.GetString(myReader.GetOrdinal("Product_ID"));
                        product_premium.Prem_Year = myReader.GetInt32(myReader.GetOrdinal("Prem_Year"));
                        product_premium.Prem_Lot = myReader.GetInt32(myReader.GetOrdinal("Prem_Lot"));
                        product_premium.AP = myReader.GetDouble(myReader.GetOrdinal("AP"));
                        product_premium.NumberPolicy = Convert.ToInt32(myReader["number_policy"].ToString());
                        product_premium_list.Add(product_premium);

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
            Log.AddExceptionToLog("Error in function [GetPMAProductPremiumDetailsByDates] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return product_premium_list;
    }


    //Get PMA GTLI premium details from prem pay list between dates
    public static List<bl_product_premium_report> GetPMAGTLIPremiumDetailsFromPremPayByDates(DateTime from_date, DateTime to_date)
    {
        List<bl_product_premium_report> group_prem_pay_list = new List<bl_product_premium_report>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_PMA_GTLI_Prem_Pay_By_Dates";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_product_premium_report group_prem_pay = new bl_product_premium_report();
                        group_prem_pay.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        group_prem_pay.Amount_Paid = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_pay.AP = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_pay.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        group_prem_pay.Pay_Date = myReader.GetDateTime(myReader.GetOrdinal("Pay_Date"));
                        group_prem_pay.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                        group_prem_pay.Product = myReader.GetString(myReader.GetOrdinal("Product"));
                        group_prem_pay.Pay_Mode = myReader.GetString(myReader.GetOrdinal("Mode"));
                        group_prem_pay.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                        group_prem_pay.Prem_Year = myReader.GetInt32(myReader.GetOrdinal("Prem_Year"));
                        group_prem_pay.Prem_Lot = myReader.GetInt32(myReader.GetOrdinal("Prem_Lot"));
                        group_prem_pay_list.Add(group_prem_pay);

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
            Log.AddExceptionToLog("Error in function [GetPMAGTLIPremiumDetailsFromPremPayByDates] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return group_prem_pay_list;
    }

    //Get PMA GTLI premium details from prem return list between dates
    public static List<bl_product_premium_report> GetPMAGTLIPremiumDetailsFromPremReturnByDates(DateTime from_date, DateTime to_date)
    {
        List<bl_product_premium_report> group_prem_return_list = new List<bl_product_premium_report>();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_PMA_GTLI_Prem_Return_By_Dates";

                myCommand.Parameters.AddWithValue("@From_Date", from_date);
                myCommand.Parameters.AddWithValue("@To_Date", to_date);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_product_premium_report group_prem_return = new bl_product_premium_report();
                        group_prem_return.Policy_Number = myReader.GetString(myReader.GetOrdinal("Policy_Number"));
                        group_prem_return.Amount_Paid = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_return.AP = myReader.GetDouble(myReader.GetOrdinal("Amount"));
                        group_prem_return.Effective_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        group_prem_return.Pay_Date = myReader.GetDateTime(myReader.GetOrdinal("Return_Date"));
                        group_prem_return.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("Sum_Insured"));
                        group_prem_return.Product = myReader.GetString(myReader.GetOrdinal("Product"));
                        group_prem_return.Pay_Mode = myReader.GetString(myReader.GetOrdinal("Mode"));
                        group_prem_return.Transaction_Type = myReader.GetInt16(myReader.GetOrdinal("Transaction_Type"));
                        group_prem_return.Prem_Year = myReader.GetInt32(myReader.GetOrdinal("Prem_Year"));
                        group_prem_return.Prem_Lot = myReader.GetInt32(myReader.GetOrdinal("Prem_Lot"));

                        group_prem_return_list.Add(group_prem_return);

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
            Log.AddExceptionToLog("Error in function [GetPMAGTLIPremiumDetailsFromPremReturnByDates] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return group_prem_return_list;
    }

    //Get prem year and prem lot
    public static string GetPremYearAndPremLot(string policy_id, string receipt_no)
    {
        string year_time = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                string sql = @"select Prem_Year,min(Prem_Lot) as min_lot, max(Prem_Lot) as max_lot, Ct_Policy_Prem_Pay.Pay_Mode_ID,Mode from Ct_Policy_Prem_Pay 
                                inner join Ct_Official_Receipt_Prem_Pay on Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID=Ct_Official_Receipt_Prem_Pay.Policy_Prem_Pay_ID
                                inner join Ct_Official_Receipt on Ct_Official_Receipt.Official_Receipt_ID=Ct_Official_Receipt_Prem_Pay.Official_Receipt_ID
                                inner join Ct_Payment_Mode on Ct_Policy_Prem_Pay.Pay_Mode_ID=Ct_Payment_Mode.Pay_Mode_ID
                             where Ct_Policy_Prem_Pay.Policy_ID=@Policy_ID and Receipt_No=@Receipt_No
                             group by Prem_Year,Ct_Policy_Prem_Pay.Pay_Mode_ID,Mode   ";

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;

                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);
                myCommand.Parameters.AddWithValue("@Receipt_No", receipt_no);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        int year_pay = int.Parse(myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString());

                        if (int.Parse(myReader.GetInt32(myReader.GetOrdinal("Pay_Mode_ID")).ToString()) == 0)
                        {
                            year_time += "&" + myReader.GetString(myReader.GetOrdinal("Mode")).ToString();
                        }
                        else if (int.Parse(myReader.GetInt32(myReader.GetOrdinal("Pay_Mode_ID")).ToString()) == 1)
                        {
                            if (year_pay == 1)
                            {
                                year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "st Year";
                            }
                            else if (year_pay == 2)
                            {
                                year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "nd Year";
                            }
                            else if (year_pay == 3)
                            {
                                year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "rd Year";
                            }
                            else
                            {
                                year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + " Year";
                            }
                        }
                        else //if(int.Parse(item["Pay_Mode_ID"].ToString()) == 2)
                        {
                            int min_lot = int.Parse(myReader.GetInt32(myReader.GetOrdinal("min_lot")).ToString()), max_lot = int.Parse(myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString());

                            if (year_pay == 1)
                            {
                                if (min_lot == max_lot)
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "st Year/" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                                else
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "st Year/" + myReader.GetInt32(myReader.GetOrdinal("min_lot")).ToString() + "-" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                            }
                            else if (year_pay == 2)
                            {
                                if (min_lot == max_lot)
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "nd Year/" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                                else
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "nd Year/" + myReader.GetInt32(myReader.GetOrdinal("min_lot")).ToString() + "-" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                            }
                            else if (year_pay == 3)
                            {
                                if (min_lot == max_lot)
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "rd Year/" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                                else
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + "rd Year/" + myReader.GetInt32(myReader.GetOrdinal("min_lot")).ToString() + "-" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                            }
                            else
                            {
                                if (min_lot == max_lot)
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + " Year/" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                                else
                                {
                                    year_time += "&" + myReader.GetInt32(myReader.GetOrdinal("Prem_Year")).ToString() + " Year/" + myReader.GetInt32(myReader.GetOrdinal("min_lot")).ToString() + "-" + myReader.GetInt32(myReader.GetOrdinal("max_lot")).ToString();
                                }
                            }
                        }

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
            Log.AddExceptionToLog("Error in function [GetPremYearAndPremLot] in class [da_product_premium_report]. Details: " + policy_id + receipt_no + ex.Message);

        }

        if (year_time != "")
        {
            year_time = year_time.Remove(0, 1);
        }

        return year_time;
    }

    //Get Official Receipt Number by GTLI Policy Prem Pay ID
    public static string GetOfficialReceiptNumberByGTLIPolicyPremPayID(string gtli_policy_prem_pay_id)
    {
        string official_receipt_number = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Official_Receipt_Number_By_GTLI_Policy_Prem_Pay_ID";

                myCommand.Parameters.AddWithValue("@GTLI_Policy_Prem_Pay_ID", gtli_policy_prem_pay_id);
            

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        official_receipt_number = myReader.GetString(myReader.GetOrdinal("Receipt_No"));
                     
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
            Log.AddExceptionToLog("Error in function [GetOfficialReceiptNumberByGTLIPolicyPremPayID] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return official_receipt_number;
    }

    //Get Official Receipt Number by GTLI Policy Prem Return ID
    public static string GetOfficialReceiptNumberByGTLIPolicyPremReturnID(string gtli_policy_prem_return_id)
    {
        string official_receipt_number = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Official_Receipt_Number_By_GTLI_Policy_Prem_Return_ID";

                myCommand.Parameters.AddWithValue("@GTLI_Policy_Prem_Return_ID", gtli_policy_prem_return_id);

                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        official_receipt_number = myReader.GetString(myReader.GetOrdinal("Receipt_No"));

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
            Log.AddExceptionToLog("Error in function [GetOfficialReceiptNumberByGTLIPolicyPremReturnID] in class [da_product_premium_report]. Details: " + ex.Message);

        }
        return official_receipt_number;
    }

    /// GET RENEWAL PREMIUM REPORT BY ALL PRODUCT
    /// [PRODUCT: // FT013, T10, T10002, T1011, T3, T3002, T5, T5002, // PP15/10, PP200 // W10, W15, W20, W9010, W9015, W9020 // MRTA, MRTA12, MRTA24, MRTA36 
    //   FP,FPP10/10, 'FPP15/15','FP','FPP10/10','FPP15/15','FPP5/5','NFP10/10','NFP15/15','NFP5/5'
    /// SDS10/10','SDS12/12','SDS15/15','SDSPK10/10/5300','SDSPK12/12/6300','SDSPK15/15/6600','SDSPKM10/10/10630','SDSPKM12/12/12560']
    public static bool GetRenewalPremiumReportByAllProduct(DateTime Next_Due_Date, string Send_By)
    {
        bool result = false;

        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Generate_Renewal_Premium_Report", new string[,]{ {"@Next_Due_Date", Next_Due_Date +""}
        }, "[da_product_premium_report] ==> [GetRenewalPremiumReportByAllProduct(DateTime Next_Due_Date)]");

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [GetRenewalPremiumReportByAllProduct(DateTime Next_Due_Date) ] in class [da_product_premium_report], details: " + ex.Message);
        }
        return result;
    
    }

    //Get product premium list between dates
    public static List<bl_product_premium_report> GetRenewalPremiumReport(DateTime from_date, DateTime to_date)
    {
        List<bl_product_premium_report> product_premium_list = new List<bl_product_premium_report>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_Get_Product_Renewal_Premium_Report", new string[,] { 
            {"@From_Date", from_date+""},
            {"@To_Date", to_date+""}
            }, "Function [GetRenewalPremiumReport(DateTime from_date, DateTime to_date), class [da_product_premium_report]");

            foreach (DataRow row in tbl.Rows)
            {
                bl_product_premium_report policy_premium = new bl_product_premium_report();

                policy_premium.Policy_Number = row["Policy_Number"].ToString();
                policy_premium.Amount_Paid = Convert.ToDouble(row["Amount"].ToString());
                policy_premium.Effective_Date = Convert.ToDateTime(row["Effective_Date"].ToString());
                policy_premium.Due_Date = Convert.ToDateTime(row["Due_Date"].ToString());
                policy_premium.Sum_Insure = Convert.ToDouble(row["Sum_Insure"].ToString());
                policy_premium.Product = row["Product_Name"].ToString();
                policy_premium.Pay_Mode = row["Pay_Mode"].ToString();
                policy_premium.Product_ID = row["Product_ID"].ToString();
                policy_premium.Prem_Year = Convert.ToInt32(row["Pay_Year"].ToString());
                policy_premium.Prem_Lot = Convert.ToInt32(row["Pay_Lot"].ToString());
                policy_premium.AP = Convert.ToDouble(row["AP"].ToString());

                product_premium_list.Add(policy_premium);
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [GetRenewalPremiumReport] in class [da_product_premium_report]. Details: " + ex.Message);
        }
        return product_premium_list;
    
    }

}