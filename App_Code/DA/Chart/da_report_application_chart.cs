using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_report_application_chart
/// </summary>
public class da_report_application_chart
{
    private static da_report_application_chart mytitle = null;
    public da_report_application_chart()
	{
        if (mytitle == null)
        {
            mytitle = new da_report_application_chart();
		}
	}

    //Get Application Report Chart Data By Conditions (Pie or Line Chart)
    public static List<bl_application_report_chart> GetApplicationReportChartByConditions(DateTime from_date, DateTime to_date,
      string policy_status)
    {
        List<bl_application_report_chart> application_report_chart_list = new List<bl_application_report_chart>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "", group_by = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            if (policy_status == "")
            {//convert(varchar(10),dbo.Ct_App_Info.App_Date,103)
                sql = @"SELECT Ct_Product.Product_ID, Count(Ct_App_Register.App_Register_ID) as Total_Application,
                         SUM(dbo.Ct_App_Life_Product.User_Sum_Insure) AS Total_Sum_Insure, SUM(dbo.Ct_App_Life_Product.User_Premium) AS Total_Premium                       
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"SELECT Ct_Product.Product_ID, Count(Ct_App_Register.App_Register_ID) as Total_Application, 
                         SUM(dbo.Ct_App_Life_Product.User_Sum_Insure) AS Total_Sum_Insure, SUM(dbo.Ct_App_Life_Product.User_Premium) AS Total_Premium
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (dbo.Ct_Underwrite_Table.Status_Code in (" + policy_status + "))";

            }

            group_by = " group by Ct_Product.Product_ID ";

            cmd.CommandText = sql + group_by;          

            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    bl_application_report_chart application_report_chart = new bl_application_report_chart();

                    application_report_chart.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    application_report_chart.Total_Application = rdr.GetInt32(rdr.GetOrdinal("Total_Application"));
                    application_report_chart.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                    application_report_chart.Total_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Total_Sum_Insure"));

                    application_report_chart.Product = da_product.GetProductByProductID(application_report_chart.Product_ID).En_Title;

                    application_report_chart_list.Add(application_report_chart);
                }
            }/// End loop

            con.Close();

        }/// End of ConnectionString

        return application_report_chart_list;
    }

    //Get Application Report Chart Data By Conditions (Combo Chart)
    public static List<bl_application_report_chart> GetApplicationReportComboChartByConditions(DateTime from_date, DateTime to_date,
      string policy_status)
    {
        List<bl_application_report_chart> application_report_chart_list = new List<bl_application_report_chart>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "", group_by = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            if (policy_status == "")
            {//convert(varchar(10),dbo.Ct_App_Info.App_Date,103)
                sql = @"SELECT Ct_Product.Product_ID, Count(Ct_App_Register.App_Register_ID) as Total_Application, YEAR(Ct_App_Register.Created_On) as My_Year, MONTH(Ct_App_Register.Created_On) as My_Month, 
                         SUM(dbo.Ct_App_Life_Product.User_Sum_Insure) AS Total_Sum_Insure, SUM(dbo.Ct_App_Life_Product.User_Premium) AS Total_Premium                       
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"SELECT Ct_Product.Product_ID, Count(Ct_App_Register.App_Register_ID) as Total_Application, YEAR(Ct_App_Register.Created_On) as My_Year, MONTH(Ct_App_Register.Created_On) as My_Month, 
                         SUM(dbo.Ct_App_Life_Product.User_Sum_Insure) AS Total_Sum_Insure, SUM(dbo.Ct_App_Life_Product.User_Premium) AS Total_Premium
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (dbo.Ct_Underwrite_Table.Status_Code in (" + policy_status + "))";

            }

            group_by = " group by YEAR(Ct_App_Register.Created_On), MONTH(Ct_App_Register.Created_On), Ct_Product.Product_ID ";

            cmd.CommandText = sql + group_by;

            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    bl_application_report_chart application_report_chart = new bl_application_report_chart();

                    application_report_chart.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    application_report_chart.Total_Application = rdr.GetInt32(rdr.GetOrdinal("Total_Application"));
                    application_report_chart.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                    application_report_chart.Total_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Total_Sum_Insure"));
                    application_report_chart.Year = rdr.GetInt32(rdr.GetOrdinal("My_Year"));
                    application_report_chart.Month = rdr.GetInt32(rdr.GetOrdinal("My_Month"));
                    application_report_chart.Product = da_product.GetProductByProductID(application_report_chart.Product_ID).En_Title;

                    application_report_chart_list.Add(application_report_chart);
                }
            }/// End loop

            con.Close();

        }/// End of ConnectionString

        return application_report_chart_list;
    }

    //Get Distinct Product By Conditions (Combo Chart)
    public static List<string> GetDistinctProductByConditions(DateTime from_date, DateTime to_date,
      string policy_status)
    {
        List<string> product_list = new List<string>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            if (policy_status == "")
            {//convert(varchar(10),dbo.Ct_App_Info.App_Date,103)
                sql = @"SELECT Distinct Ct_Product.Product_ID                    
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"SELECT Distinct Ct_Product.Product_ID
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (dbo.Ct_Underwrite_Table.Status_Code in (" + policy_status + "))";

            }

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));
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
      string policy_status)
    {
        List<string> Month_list = new List<string>();
        
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "";

            SqlCommand cmd = new SqlCommand(sql, con);

            if (policy_status == "")
            {//convert(varchar(10),dbo.Ct_App_Info.App_Date,103)
                sql = @"SELECT DISTINCT YEAR(Ct_App_Register.Created_On) as My_Year, MONTH(Ct_App_Register.Created_On) as My_Month                    
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"SELECT DISTINCT YEAR(Ct_App_Register.Created_On) as My_Year, MONTH(Ct_App_Register.Created_On) as My_Month 
                         FROM dbo.Ct_App_Info INNER JOIN
                         dbo.Ct_App_Register ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Register.App_Register_ID INNER JOIN
                         dbo.Ct_Underwriting ON dbo.Ct_App_Register.App_Register_ID = dbo.Ct_Underwriting.App_Register_ID INNER JOIN
                         dbo.Ct_Underwrite_Table ON dbo.Ct_Underwriting.Status_Code = dbo.Ct_Underwrite_Table.Status_Code INNER JOIN
                         dbo.Ct_App_Life_Product ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Life_Product.App_Register_ID INNER JOIN
                         dbo.Ct_Product ON dbo.Ct_App_Life_Product.Product_ID = dbo.Ct_Product.Product_ID INNER JOIN
                         dbo.Ct_Sale_Agent ON dbo.Ct_App_Info.Sale_Agent_ID = dbo.Ct_Sale_Agent.Sale_Agent_ID INNER JOIN
                         dbo.Ct_Sale_Agent_Ordinary ON dbo.Ct_Sale_Agent.Sale_Agent_ID = dbo.Ct_Sale_Agent_Ordinary.Sale_Agent_ID INNER JOIN
                         dbo.Ct_App_Info_Person ON dbo.Ct_App_Info.App_Register_ID = dbo.Ct_App_Info_Person.App_Register_ID
                                where 
                                (CONVERT(nvarchar(10),dbo.Ct_App_Info.App_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (dbo.Ct_Underwrite_Table.Status_Code in (" + policy_status + "))";

            }                     

            cmd.CommandText = sql + " order by My_Year ASC, My_Month ASC";
            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));
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