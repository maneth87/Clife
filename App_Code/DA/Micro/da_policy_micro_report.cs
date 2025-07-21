using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for policy_micro_report
/// </summary>
public class da_policy_micro_report
{
     private static da_policy_micro_report mytitle = null;
     public da_policy_micro_report()
	 {
	  if (mytitle == null)
        {
            mytitle = new da_policy_micro_report();
        }
	 }

     public static List<bl_policy_micro_report> GetPolicyReportList(DateTime from_date, DateTime to_date, string check_policy_status_code, int order_by, string sale_agent_id)
    {
        List<bl_policy_micro_report> policy_report_list = new List<bl_policy_micro_report>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            string sql = "", Order_by = "";

            if (check_policy_status_code == "")
            {
                sql = @"select * from V_Report_Policy_Micro where 
                                (CONVERT(nvarchar(10),V_Report_Policy_Micro.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"select * from v_report_policy where 
                                (CONVERT(nvarchar(10),V_Report_Policy_Micro.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (lower(V_Report_Policy_Micro.Policy_Status_Type_ID) in (" + check_policy_status_code + "))";

            }

            if (order_by == 1) // Policy number
            {
                Order_by = " order by V_Report_Policy_Micro.Policy_Number asc";
            }
            else if (order_by == 2) // Issued date
            {
                Order_by = " order by V_Report_Policy_Micro.Issue_Date asc";
            }
            else if (order_by == 3)
            {
                Order_by = " order by V_Report_Policy_Micro.Effective_Date asc";
            }
                    

            SqlCommand cmd = new SqlCommand(sql, con);

            if (sale_agent_id != "")
            {
                sql += " and V_Report_Policy_Micro.Sale_Agent_ID = @Sale_Agent_ID   ";
                cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
            }

            cmd.CommandText = sql + Order_by;

            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {

                    bl_policy_micro_report policy_report = new bl_policy_micro_report();

                    policy_report.Barcode = rdr.GetString(rdr.GetOrdinal("Card_ID"));
                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.Card_Number = rdr.GetString(rdr.GetOrdinal("Card_Number"));

                    policy_report.Customer_Name = rdr.GetString(rdr.GetOrdinal("Last_Name")) + ' ' +  rdr.GetString(rdr.GetOrdinal("First_Name"));

                    policy_report.Customer_Name_Khmer = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name")) + ' ' + rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));

                    policy_report.En_Abbr = rdr.GetString(rdr.GetOrdinal("En_Abbr"));
                  
                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));

                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                    policy_report.Maturity_Date = rdr.GetDateTime(rdr.GetOrdinal("Maturity_Date"));

                    policy_report.Mode = da_payment_mode.GetPaymentModeByPayModeID(rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"))).Mode;

                    policy_report.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.User_Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));
                                   
                    policy_report.Policy_Micro_ID = rdr.GetString(rdr.GetOrdinal("Policy_Micro_ID"));

                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    policy_report.Agent = rdr.GetString(rdr.GetOrdinal("Full_Name"));

                    policy_report.Channel_Location_ID = rdr.GetString(rdr.GetOrdinal("Channel_Location_ID"));

                    policy_report.Customer_Number = rdr.GetString(rdr.GetOrdinal("Customer_Micro_Number"));

                    policy_report.Payment_Code = rdr.GetString(rdr.GetOrdinal("Payment_Code"));

                    policy_report_list.Add(policy_report);
                }
            }/// End loop
        }/// End of ConnectionString
               
            return policy_report_list;     
    }

     public static List<bl_policy_micro_report> Get_Policy_Report_By_Condition(DateTime from_date, DateTime to_date, string check_policy_status_code, int order_by, int pay_mode_id, string policy_number, string product_id, string barcode, string view_type_id, string view_type, int use_date)
    {
        List<bl_policy_micro_report> policy_report_list = new List<bl_policy_micro_report>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "", Order_by = "";

            SqlCommand cmd = new SqlCommand(sql, con);



            if (check_policy_status_code == "")
            {
                sql = "select * from V_Report_Policy_Micro where V_Report_Policy_Micro.Policy_Micro_ID != '' ";

                if (use_date != 0)
                {
                    sql += @" AND (CONVERT(nvarchar(10),V_Report_Policy_Micro.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
                }

            }
            else
            {
                sql = @"select * from V_Report_Policy_Micro where lower(V_Report_Policy_Micro.Policy_Status_Type_ID) in (" + check_policy_status_code + ") ";
                if (use_date != 0)
                {
                    sql += @" AND (CONVERT(nvarchar(10),V_Report_Policy_Micro.Effective_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) ";
                }

            }

            if (order_by == 1) // Policy number
            {
                Order_by = " order by V_Report_Policy_Micro.Policy_Number asc";
            }
            else if (order_by == 2) // Issued date
            {
                Order_by = " order by V_Report_Policy_Micro.Issue_Date asc";
            }
            else if (order_by == 3)
            {
                Order_by = " order by V_Report_Policy_Micro.Effective_Date asc";
            }


            if (pay_mode_id != -1)
            {
                sql += "  and V_Report_Policy_Micro.Pay_Mode = @Pay_Mode_ID";
                cmd.Parameters.AddWithValue("@Pay_Mode_ID", pay_mode_id);
            }

            if (policy_number != "")
            {
                sql += "  and V_Report_Policy_Micro.Policy_Number Like '%' + @Policy_Number + '%' ";
                cmd.Parameters.AddWithValue("@Policy_Number", policy_number);
            }

            if (product_id != "" && product_id != "-1")
            {
                sql += " and V_Report_Policy_Micro.Product_ID = @Product_ID   ";
                cmd.Parameters.AddWithValue("@Product_ID", product_id);
            }

            if (view_type == "0" && view_type_id != "")
            {
                sql += " and V_Report_Policy_Micro.Sale_Agent_ID = @Sale_Agent_ID   ";
                cmd.Parameters.AddWithValue("@Sale_Agent_ID", view_type_id);
            }
            else if (view_type == "1" && view_type_id != "")
            {
                sql += " and V_Report_Policy_Micro.Channel_Location_ID = @Channel_Location_ID   ";
                cmd.Parameters.AddWithValue("@Channel_Location_ID", view_type_id);
            }

            if (barcode != "")
            {
                sql += " and V_Report_Policy_Micro.Card_ID = @Card_ID   ";
                cmd.Parameters.AddWithValue("@Card_ID", barcode);
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

                    bl_policy_micro_report policy_report = new bl_policy_micro_report();


                    policy_report.Barcode = rdr.GetString(rdr.GetOrdinal("Card_ID"));
                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.Card_Number = rdr.GetString(rdr.GetOrdinal("Card_Number"));

                    policy_report.Customer_Name = rdr.GetString(rdr.GetOrdinal("Last_Name")) + ' ' + rdr.GetString(rdr.GetOrdinal("First_Name"));

                    policy_report.Customer_Name_Khmer = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name")) + ' ' + rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));

                    policy_report.En_Abbr = rdr.GetString(rdr.GetOrdinal("En_Abbr"));

                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));

                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                    policy_report.Maturity_Date = rdr.GetDateTime(rdr.GetOrdinal("Maturity_Date"));

                    policy_report.Mode = da_payment_mode.GetPaymentModeByPayModeID(rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"))).Mode;

                    policy_report.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.User_Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));

                    policy_report.Policy_Micro_ID = rdr.GetString(rdr.GetOrdinal("Policy_Micro_ID"));

                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    policy_report.Agent = rdr.GetString(rdr.GetOrdinal("Full_Name"));

                    policy_report.Channel_Location_ID = rdr.GetString(rdr.GetOrdinal("Channel_Location_ID"));

                    policy_report.Customer_Number = rdr.GetString(rdr.GetOrdinal("Customer_Micro_Number"));

                    policy_report.Payment_Code = rdr.GetString(rdr.GetOrdinal("Payment_Code"));


                    policy_report_list.Add(policy_report);
                }
            }/// End loop
        }/// End of ConnectionString
             
            return policy_report_list;
     
    }

}