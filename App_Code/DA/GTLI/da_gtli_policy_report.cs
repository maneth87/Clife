using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_gtli_policy_report
/// </summary>
public class da_gtli_policy_report
{
	 private static da_gtli_policy_report mytitle = null;
     public da_gtli_policy_report()
	 {
	  if (mytitle == null)
        {
            mytitle = new da_gtli_policy_report();
        }
	 }

     public static List<bl_gtli_policy_search> GetPolicyReportList(DateTime from_date, DateTime to_date, int order_by)
     {
         List<bl_gtli_policy_search> policy_report_list = new List<bl_gtli_policy_search>();

         string connString = AppConfiguration.GetConnectionString();

         using (SqlConnection con = new SqlConnection(connString))
         {

             string sql = "", Order_by = "";
            
             sql = @"select * from V_Report_Policy_GTLI where 
                                (CONVERT(nvarchar(10),V_Report_Policy_GTLI.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
             
            

             if (order_by == 1) // Policy number
             {
                 Order_by = " order by V_Report_Policy_GTLI.Policy_Number desc";
             }
             else if (order_by == 2) // Created date
             {
                 Order_by = " order by V_Report_Policy_GTLI.Effective_Date desc";
             }
            


             SqlCommand cmd = new SqlCommand(sql, con);


             cmd.CommandText = sql + Order_by;

             cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
             cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

             con.Open();
             SqlDataReader rdr = cmd.ExecuteReader();
             while (rdr.Read())
             {
                 if (rdr.HasRows)
                 {

                     bl_gtli_policy_search policy_report = new bl_gtli_policy_search();

                     policy_report.GTLI_Policy_ID = rdr.GetString(rdr.GetOrdinal("GTLI_Policy_ID"));
                     policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                     policy_report.GTLI_Plan_ID = rdr.GetString(rdr.GetOrdinal("GTLI_Plan_ID"));
                     policy_report.Life_Premium = rdr.GetDouble(rdr.GetOrdinal("Life_Premium"));

                     policy_report.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));

                     policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                     policy_report.Sum_Insured = rdr.GetDouble(rdr.GetOrdinal("Sum_Insured"));
                     policy_report.TPD_Premium = rdr.GetDouble(rdr.GetOrdinal("TPD_Premium"));
                     policy_report.Transaction_Staff_Number = rdr.GetInt32(rdr.GetOrdinal("Transaction_Staff_Number"));

                     policy_report.Transaction_Type = rdr.GetInt16(rdr.GetOrdinal("Transaction_Type"));

                     policy_report.DHC_Premium = rdr.GetDouble(rdr.GetOrdinal("DHC_Premium"));
                     policy_report.Company_Name = rdr.GetString(rdr.GetOrdinal("Company_Name"));

                     if (policy_report.GTLI_Plan_ID != null && policy_report.GTLI_Plan_ID != "0")
                     {
                         policy_report.GTLI_Plan = da_gtli_plan.GetPlan(policy_report.GTLI_Plan_ID).GTLI_Plan;
                     }
                     else
                     {
                         policy_report.GTLI_Plan = "";
                     }

                     policy_report_list.Add(policy_report);
                 }
             }/// End loop
         }/// End of ConnectionString

         return policy_report_list;
     }

     public static List<bl_gtli_policy_search> Get_Policy_Report_By_Condition(DateTime from_date, DateTime to_date, int order_by, string policy_number, string company, string sale_agent_id, int use_date)
     {
         List<bl_gtli_policy_search> policy_report_list = new List<bl_gtli_policy_search>();
         
         string connString = AppConfiguration.GetConnectionString();

         using (SqlConnection con = new SqlConnection(connString))
         {
             string sql = "", Order_by = "";

             SqlCommand cmd = new SqlCommand(sql, con);

             sql = "select * from V_Report_Policy_GTLI where V_Report_Policy_GTLI.GTLI_Policy_ID != '' ";

            if (use_date != 0)
            {
                sql += @" AND (CONVERT(nvarchar(10),V_Report_Policy_GTLI.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                            and CONVERT(nvarchar(10),@to_date,111))";
            }
                 

             if (order_by == 1) // Policy number
             {
                 Order_by = " order by V_Report_Policy_GTLI.Policy_Number desc";
             }
             else if (order_by == 2) // Created date
             {
                 Order_by = " order by V_Report_Policy_GTLI.Effective_Date desc";
             }
              
             
             if (policy_number != "")
             {
                 sql += "  and V_Report_Policy_GTLI.Policy_Number Like '%' + @Policy_Number + '%' ";
                 cmd.Parameters.AddWithValue("@Policy_Number", policy_number);
             }

             if (company != "")
             {
                 sql += " and V_Report_Policy_GTLI.Company_Name = @Company_Name   ";
                 cmd.Parameters.AddWithValue("@Company_Name", company);
             }

             if (sale_agent_id != "0" && sale_agent_id != "")
             {
                 sql += " and V_Report_Policy_GTLI.Sale_Agent_ID = @Sale_Agent_ID   ";
                 cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
             }          
           

             cmd.CommandText = sql + Order_by;

             if (use_date != 0)
             {
                 cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
                 cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));
             }

             con.Open();
             SqlDataReader rdr = cmd.ExecuteReader();
             while (rdr.Read())
             {
                 if (rdr.HasRows)
                 {

                     bl_gtli_policy_search policy_report = new bl_gtli_policy_search();

                     policy_report.GTLI_Policy_ID = rdr.GetString(rdr.GetOrdinal("GTLI_Policy_ID"));
                     policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                     policy_report.GTLI_Plan_ID = rdr.GetString(rdr.GetOrdinal("GTLI_Plan_ID"));
                     policy_report.Life_Premium = rdr.GetDouble(rdr.GetOrdinal("Life_Premium"));

                     policy_report.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));

                     policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                     policy_report.Sum_Insured = rdr.GetDouble(rdr.GetOrdinal("Sum_Insured"));
                     policy_report.TPD_Premium = rdr.GetDouble(rdr.GetOrdinal("TPD_Premium"));
                     policy_report.Transaction_Staff_Number = rdr.GetInt32(rdr.GetOrdinal("Transaction_Staff_Number"));

                     policy_report.Transaction_Type = rdr.GetInt16(rdr.GetOrdinal("Transaction_Type"));
                     policy_report.Company_Name = rdr.GetString(rdr.GetOrdinal("Company_Name"));
                     policy_report.DHC_Premium = rdr.GetDouble(rdr.GetOrdinal("DHC_Premium"));

                     if (policy_report.GTLI_Plan_ID != null && policy_report.GTLI_Plan_ID != "0")
                     {
                         policy_report.GTLI_Plan = da_gtli_plan.GetPlan(policy_report.GTLI_Plan_ID).GTLI_Plan;
                     }
                     else
                     {
                         policy_report.GTLI_Plan = "";
                     }

                   
                     policy_report_list.Add(policy_report);
                 }
             }/// End loop
         }/// End of ConnectionString

         return policy_report_list;

     }
}