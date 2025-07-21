using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_report_policy_chart
/// </summary>
public class da_report_policy_chart
{
    private static da_report_policy_chart mytitle = null;
    public da_report_policy_chart()
	{
        if (mytitle == null)
        {
            mytitle = new da_report_policy_chart();
		}
	}

    //Get Policy Report Chart Data By Conditions (Pie or Line Chart)
    public static List<bl_policy_report_chart> GetPolicyReportChartByConditions(DateTime from_date, DateTime to_date,
      string policy_status, string policy_number, string product_id)
    {
        List<bl_policy_report_chart> policy_report_chart_list = new List<bl_policy_report_chart>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "", group_by = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            switch (product_id) //select by product ID
            {
                case "MRTA":
                case "MRTA12":
                case "MRTA24":
                case "MRTA36":
                case "PP200":
                case "PP15/10":
                case "T3002":
                case "T5002":
                case "T10002":
                case "T3":
                case "T5":
                case "T10":
                case "T1011":
                case "W10":
                case "W15":
                case "W20":
                case "W9010":
                case "W9015":
                case "W9020":
                case "FT013":
                    if (policy_status != "")
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "'";

                    }
                    break;
                case "-1": //select from_date and to_date only
                    if (policy_status != "")
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium,  Count(Policy_ID) as Total_Policy FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    }

                    break;
                default:
                    sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    break;

            }

            group_by = " group by V_Policy_Complete.Product_ID ";

            cmd.CommandText = sql + group_by;
          
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    bl_policy_report_chart policy_report_chart = new bl_policy_report_chart();

                    policy_report_chart.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));                  
                    policy_report_chart.Total_Policy = rdr.GetInt32(rdr.GetOrdinal("Total_Policy"));
                    policy_report_chart.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                    policy_report_chart.Total_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Total_Sum_Insure"));

                    policy_report_chart.Product = da_product.GetProductByProductID(policy_report_chart.Product_ID).En_Title;

                    policy_report_chart_list.Add(policy_report_chart);
                }
            }/// End loop

            con.Close();

        }/// End of ConnectionString

        return policy_report_chart_list;
    }

    //Get Policy Report Chart Data By Conditions (Combo Chart)
    public static List<bl_policy_report_chart> GetPolicyReportComboChartByConditions(DateTime from_date, DateTime to_date,
      string policy_status, string policy_number, string product_id)
    {
        List<bl_policy_report_chart> policy_report_chart_list = new List<bl_policy_report_chart>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "", group_by = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            switch (product_id) //select by product ID
            {
                case "MRTA":
                case "MRTA12":
                case "MRTA24":
                case "MRTA36":
                case "PP200":
                case "PP15/10":
                case "T3002":
                case "T5002":
                case "T10002":
                case "T3":
                case "T5":
                case "T10":
                case "T1011":
                case "W10":
                case "W15":
                case "W20":
                case "W9010":
                case "W9015":
                case "W9020":
                case "FT013":
                    if (policy_status != "")
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy, YEAR(Effective_Date) as My_Year, MONTH(V_Policy_Complete.Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy, YEAR(Effective_Date) as My_Year, MONTH(V_Policy_Complete.Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "'";

                    }
                    break;
                case "-1": //select from_date and to_date only
                    if (policy_status != "")
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium,  Count(Policy_ID) as Total_Policy, YEAR(Effective_Date) as My_Year, MONTH(V_Policy_Complete.Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy, YEAR(Effective_Date) as My_Year, MONTH(V_Policy_Complete.Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    }

                    break;
                default:
                    sql = @"SELECT Product_ID, SUM(Sum_Insure) as Total_Sum_Insure, SUM(Premium_Paid) as Total_Premium, Count(Policy_ID) as Total_Policy, YEAR(Effective_Date) as My_Year, MONTH(V_Policy_Complete.Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    break;

            }

            group_by = " group by YEAR(Effective_Date), MONTH(V_Policy_Complete.Effective_Date), V_Policy_Complete.Product_ID ";

            cmd.CommandText = sql + group_by;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    bl_policy_report_chart policy_report_chart = new bl_policy_report_chart();

                    policy_report_chart.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    policy_report_chart.Total_Policy = rdr.GetInt32(rdr.GetOrdinal("Total_Policy"));
                    policy_report_chart.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                    policy_report_chart.Total_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Total_Sum_Insure"));
                    policy_report_chart.Year = rdr.GetInt32(rdr.GetOrdinal("My_Year"));
                    policy_report_chart.Month = rdr.GetInt32(rdr.GetOrdinal("My_Month"));
                    policy_report_chart.Product = da_product.GetProductByProductID(policy_report_chart.Product_ID).En_Title;

                    policy_report_chart_list.Add(policy_report_chart);
                }
            }/// End loop

            con.Close();

        }/// End of ConnectionString

        return policy_report_chart_list;
    }

    //Get Distinct Product By Conditions (Combo Chart)
    public static List<string> GetDistinctProductByConditions(DateTime from_date, DateTime to_date,
      string policy_status, string policy_number, string product_id)
    {
        List<string> product_list = new List<string>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            switch (product_id) //select by product ID
            {
                case "MRTA":
                case "MRTA12":
                case "MRTA24":
                case "MRTA36":
                case "PP200":
                case "PP15/10":
                case "T3002":
                case "T5002":
                case "T10002":
                case "T3":
                case "T5":
                case "T10":
                case "T1011":
                case "W10":
                case "W15":
                case "W20":
                case "W9010":
                case "W9015":
                case "W9020":
                case "FT013":
                    if (policy_status != "")
                    {
                        sql = @"SELECT Distinct Product_ID FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT Distinct Product_ID  FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "'";

                    }
                    break;
                case "-1": //select from_date and to_date only
                    if (policy_status != "")
                    {
                        sql = @"SELECT Distinct Product_ID FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT Distinct Product_ID FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    }

                    break;
                default:
                    sql = @"SELECT Distinct Product_ID FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    break;

            }                      

            cmd.CommandText = sql;
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {                    
                    
                   string product = da_product.GetProductByProductID(rdr.GetString(rdr.GetOrdinal("Product_ID"))).En_Title;

                   product_list.Add(product);
                }
            }/// End loop

            con.Close();

        }/// End of ConnectionString
         
        //Sort ASC
        product_list.Sort();

        return product_list;
    }

    //Get Distinct Month By Conditions (Combo Chart)
    public static List<string> GetDistinctMonthByConditions(DateTime from_date, DateTime to_date,
      string policy_status, string policy_number, string product_id)
    {
        List<string> Month_list = new List<string>();
        
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            switch (product_id) //select by product ID
            {
                case "MRTA":
                case "MRTA12":
                case "MRTA24":
                case "MRTA36":
                case "PP200":
                case "PP15/10":
                case "T3002":
                case "T5002":
                case "T10002":
                case "T3":
                case "T5":
                case "T10":
                case "T1011":
                case "W10":
                case "W15":
                case "W20":
                case "W9010":
                case "W9015":
                case "W9020":
                case "FT013":
                    if (policy_status != "")
                    {
                        sql = @"SELECT DISTINCT YEAR(V_Policy_Complete.Effective_Date) as My_Year, MONTH(Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT DISTINCT YEAR(V_Policy_Complete.Effective_Date) as My_Year, MONTH(Effective_Date) as My_Month  FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Product_ID = '" + product_id + "'";

                    }
                    break;
                case "-1": //select from_date and to_date only
                    if (policy_status != "")
                    {
                        sql = @"SELECT DISTINCT YEAR(V_Policy_Complete.Effective_Date) as My_Year, MONTH(Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "' AND Status = '" + policy_status + "'";
                    }
                    else
                    {
                        sql = @"SELECT DISTINCT YEAR(V_Policy_Complete.Effective_Date) as My_Year, MONTH(Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    }

                    break;
                default:
                    sql = @"SELECT DISTINCT YEAR(V_Policy_Complete.Effective_Date) as My_Year, MONTH(Effective_Date) as My_Month FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date + "' AND '" + to_date + "'";
                    break;

            }

            cmd.CommandText = sql + " order by My_Year ASC, My_Month ASC";
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    int month = rdr.GetInt32(rdr.GetOrdinal("My_Month"));
                    int year = rdr.GetInt32(rdr.GetOrdinal("My_Year"));
                    Month_list.Add(year + "/" + month);
                   
                }
            }/// End loop

            con.Close();

        }/// End of ConnectionString

        //Sort ASC            
        return Month_list;
    }
      
}