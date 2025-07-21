using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da
/// </summary>
public class da_calculate_original_prem
{
    private static da_calculate_original_prem mytitle=null;

    public da_calculate_original_prem()
	{
		if (mytitle == null)
        {
            mytitle = new da_calculate_original_prem();
        }
	}


    /// <summary>
    /// Whole Life 
    /// </summary>
    public static decimal Calculate_Original_Prem_WholeLife(string product_id, int age, int sum_insured_amount, int gender)
    {
        decimal original_prem = 0, rate_wholeLife=0; 

        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Whole_Life_Rate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Product_ID", product_id);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Gender", gender);

                con.Open();

                rate_wholeLife = decimal.Parse(cmd.ExecuteScalar().ToString());

                con.Close();

                //discount
                if (sum_insured_amount >= 2000 & sum_insured_amount < 5000)
                {
                    original_prem = (rate_wholeLife * sum_insured_amount) / 100;
                }
                else if (sum_insured_amount >= 5000 & sum_insured_amount < 40000)
                {
                    original_prem = ((rate_wholeLife - 1) * sum_insured_amount) / 100;
                }
                else if (sum_insured_amount >= 40000)
                {
                    original_prem = ((rate_wholeLife - 2) * sum_insured_amount) / 100;
                }             
            }
        }
        catch 
        {
            original_prem = 0;
        }

        return original_prem;
    }

    /// <summary>
    /// Term 
    /// </summary>
    public static decimal Calculate_Original_Prem_Term(string product_id, int age, int sum_insured_amount, int gender)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        try
        {

            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Term_Life_Rate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Product_ID",product_id);
                cmd.Parameters.AddWithValue("@Age",age);
                cmd.Parameters.AddWithValue("@Gender",gender);

                con.Open();

                myRate = decimal.Parse(cmd.ExecuteScalar().ToString());

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
        }
        catch
        {
            myPremium = 0;
        }

        return myPremium;
    }

    /// <summary>
    /// MRTA
    /// </summary>
    public static decimal Calculate_Original_Prem_MRTA(string product_id, int age, int sum_insured_amount, int coverage_period, int gender)
    {
        decimal myPremium = 0;
        decimal myRate = 0;

        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_MRTA_Rate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Product_ID", product_id);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Assure_Year", coverage_period);
                cmd.Parameters.AddWithValue("@Gender", gender);

                con.Open();

                myRate = decimal.Parse(cmd.ExecuteScalar().ToString());

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

        }
        catch
        {
            myPremium = 0;
        }

        return myPremium;
    }

    public static DataTable GetPolicy_Info()
    {
        string connString = AppConfiguration.GetConnectionString();

        DataTable dt = new DataTable();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = @"select Sum_Insure,Product_ID,Age_Insure,Gender,Assure_Year, Ct_Policy.Policy_ID from Ct_Policy_Premium 
                                    inner join Ct_Policy on Ct_Policy_Premium.Policy_ID=Ct_Policy.Policy_ID
                                    inner join Ct_Customer on Ct_Policy.Customer_ID=Ct_Customer.Customer_ID

		                            where Product_ID not in ('mrta') ";

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = sql;

            cmd.Connection = con;

            con.Open();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            dap.Fill(dt);
            con.Close();

            decimal premium_amount = 0;

            foreach (DataRow item in dt.Rows)
            {
                if (item["Product_ID"].ToString() == "T10" || item["Product_ID"].ToString() == "T5" || item["Product_ID"].ToString() == "T3")
                {
                    premium_amount = Calculate_Original_Prem_Term(item["Product_ID"].ToString(), int.Parse(item["Age_Insure"].ToString()), int.Parse(item["Sum_Insure"].ToString()), int.Parse(item["Gender"].ToString()));
                }
                else if (item["Product_ID"].ToString() == "W10" || item["Product_ID"].ToString() == "W15" || item["Product_ID"].ToString() == "W20")
                {
                    premium_amount = Calculate_Original_Prem_WholeLife(item["Product_ID"].ToString(), int.Parse(item["Age_Insure"].ToString()), int.Parse(item["Sum_Insure"].ToString()), int.Parse(item["Gender"].ToString()));
                }

                /// Update Original Amount in Ct_Policy_Premium
                Update_OriginalAmount(item["Policy_ID"].ToString(), premium_amount);
            }
        }
        return dt;
    }

    /// <summary>
    /// Update Original Amount
    /// </summary>

    public static bool Update_OriginalAmount(string policy_id, decimal original_amount)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("update Ct_Policy_Premium set Original_Amount=CAST(@Original_Amount as decimal(18,2)) where Policy_ID=@Policy_ID ", con);

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Original_Amount", original_amount);

            con.Open();
            try
            {
                cmd.ExecuteNonQuery().ToString();

                result = true;
            }
            catch (Exception)
            {

            }
        }

        return result;
    }




}