using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for report_policy
/// </summary>
public class da_report_policy
{
     private static da_report_policy mytitle = null;
     public da_report_policy()
	{
	  if (mytitle == null)
        {
            mytitle = new da_report_policy();
        }
	}

    public static DataTable Report_Policy_Without_Next_Due(DateTime from_date, DateTime to_date, string check_policy_status_code, int order_by, int pay_mode_id, string policy_number, string product_id)
    {
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();

        DataRow r2;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "SP_Report_Policy_Without_Next_Due";

            string sql = "", Order_by = "";

            if (check_policy_status_code == "")
            {
                sql = @"select * from v_report_policy where 
                                (CONVERT(nvarchar(10),v_report_policy.Issue_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"select * from v_report_policy where 
                                (CONVERT(nvarchar(10),v_report_policy.Issue_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (lower(v_report_policy.Policy_Status_Type_ID) in (" + check_policy_status_code + "))";

            }

            if (order_by == 1) // Policy number
            {
                Order_by = " order by v_report_policy.Policy_Number asc";
            }
            else if (order_by == 2) // Issued date
            {
                Order_by = " order by v_report_policy.Issue_Date asc";
            }
            else if (order_by == 3)
            {
                Order_by = " order by v_report_policy.Effective_Date asc";
            }

            cmd.CommandText = sql + Order_by;
            cmd.Parameters.AddWithValue("@from_date", DateTime.Parse(from_date.ToString()).ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", DateTime.Parse(to_date.ToString()).ToString("yyyy/MM/dd"));

            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                dap.Fill(dt);

                dt2.Columns.Add("Policy Number");
                dt2.Columns.Add("Insured's name");
                dt2.Columns.Add("Plan");
                dt2.Columns.Add("Stat.");
                dt2.Columns.Add("Issue date");
                dt2.Columns.Add("Effective date");
                dt2.Columns.Add("Next due");
                dt2.Columns.Add("Mode");
                dt2.Columns.Add("S.A.");
                dt2.Columns.Add("Total Premium");
                dt2.Columns.Add("Premium Rate");
                dt2.Columns.Add("Last Payment");
                dt2.Columns.Add("Agent");
               // dt2.Columns.Add("Address1");

                DateTime next_due = DateTime.Now;

                foreach (DataRow r in dt.Rows)
                {
                    r2 = dt2.NewRow();

                    r2["Policy Number"] = r["Policy_Number"];
                    r2["Insured's name"] = r["customer_name"];
                    r2["Plan"] = r["En_Abbr"];
                    r2["Stat."] = r["StatusCode"];
                    r2["Issue date"] = DateTime.Parse(r["Issue_Date"].ToString()).ToString("yyyy/MM/dd");
                    r2["Effective date"] = DateTime.Parse(r["Effective_Date"].ToString()).ToString("yyyy/MM/dd");

                    next_due = Get_Next_Due(r["Policy_ID"].ToString());

                    int i = next_due.Year;

                    if (DateTime.Parse(r["Effective_date"].ToString()).Day != next_due.Day)
                    {
                        if (((i % 4 == 0) && (i % 100 != 0) || (i % 400 == 0))) // Leap Year, 29 last day of Feb
                        {
                            if (next_due.Month == 2)
                            {
                                if (next_due.Day >= 29)
                                {
                                    if (DateTime.Parse(r["Effective_date"].ToString()).Day == 28)
                                    {
                                        r2["Next due"] = next_due.AddDays(1);
                                    }
                                    else { r2["Next due"] = next_due; }
                                }
                                else { r2["Next due"] = next_due; }
                            }
                            else if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                            {
                                if (next_due.Day >= 31)
                                {
                                    if (DateTime.Parse(r["Effective_date"].ToString()).Day == 30)
                                    {
                                        r2["Next due"] = next_due.AddDays(1);
                                    }
                                    else { r2["Next due"] = next_due; }
                                }
                                else
                                {
                                    if (DateTime.Parse(r["Effective_date"].ToString()).Day > next_due.Day)
                                    {
                                        r2["Next due"] = next_due.AddDays(1);
                                    }
                                    else { r2["Next due"] = next_due; }
                                }
                            }
                            else
                            {
                                r2["Next due"] = next_due;
                            }
                        }
                        else
                        {
                            if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                            {
                                if (next_due.Day >= 31)
                                {
                                    if (DateTime.Parse(r["Effective_date"].ToString()).Day == 30)
                                    {
                                        r2["Next due"] = next_due.AddDays(1);
                                    }
                                    else { r2["Next due"] = next_due; }
                                }
                                else
                                {
                                    if (DateTime.Parse(r["Effective_date"].ToString()).Day > next_due.Day)
                                    {
                                        r2["Next due"] = next_due.AddDays(DateTime.Parse(r["Effective_date"].ToString()).Day - next_due.Day);
                                    }
                                    else { r2["Next due"] = next_due; }
                                }
                            }
                            else
                            {
                                r2["Next due"] = next_due;
                            }
                        }
                    }
                    else
                    {
                        r2["Next due"] = next_due.ToString("yyyy/MM/dd");
                    }

                    r2["Mode"] = r["Mode"];
                    r2["S.A."] = r["Sum_Insure"];
                    r2["Total Premium"] = float.Parse(r["Premium"].ToString()) + float.Parse(r["EM_Premium"].ToString());
                    r2["Premium Rate"] = r["Is_Standard"];
                    r2["Last Payment"] = DateTime.Parse(Get_Last_Pay_Date(r["Policy_ID"].ToString()).ToString()).ToString("yyyy/MM/dd");
                    r2["Agent"] = Get_Agent_Name_By_Policy_ID(r["Policy_ID"].ToString());
                    //r2["Address1"] = r["Address1"];

                    dt2.Rows.Add(r2);

                } /// End of loop from dt
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Report_Policy_Without_Next_Due] in class ['']. Details: " + ex.Message);
            }
        }
        return dt2;
    }

    public static List<bl_policy_report> GetPolicyDetailReportList(DateTime from_date, DateTime to_date, int search_date, int status, int in_month)
    {
        List<bl_policy_report> policy_detail_report_list = new List<bl_policy_report>(); 

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CL24_DETAIL_REPORT";
            if (search_date == 1)
		        cmd.Parameters.AddWithValue("@SEARCH_DATE", 1);
            else if (search_date == 2)
		        cmd.Parameters.AddWithValue("@SEARCH_DATE", 2);
            else if (search_date == 3)
                cmd.Parameters.AddWithValue("@SEARCH_DATE", 3);
            else
                cmd.Parameters.AddWithValue("@SEARCH_DATE", 0);

            cmd.Parameters.AddWithValue("@FROM_DATE", from_date);
            cmd.Parameters.AddWithValue("@TO_DATE", to_date);
            cmd.Parameters.AddWithValue("@STATUS", status);
            cmd.Parameters.AddWithValue("@IN_MONTH", in_month);
            cmd.Connection = con;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_policy_report policy_report = new bl_policy_report();

                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.App_Number = rdr.GetString(rdr.GetOrdinal("App_Number"));
                    policy_report.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                    policy_report.customer_name = rdr.GetString(rdr.GetOrdinal("Customer_Name"));
                    policy_report.Gender = rdr.GetString(rdr.GetOrdinal("Gender"));
                    policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    policy_report.Product_Name = rdr.GetString(rdr.GetOrdinal("En_Title"));
                    policy_report.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));
                    policy_report.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));
                    policy_report.Assured_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                    policy_report.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));

                    policy_report.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));

                    policy_report.Discount_Amount = rdr.GetDouble(rdr.GetOrdinal("Discount_Amount"));

                    policy_report.OLD_EM_Amount = rdr.GetDouble(rdr.GetOrdinal("OLD_EM_Amount"));

                    if (rdr.GetString(rdr.GetOrdinal("Main_Policy")) == "")
                    {
                        policy_report.Main_Policy = "Main";
                        policy_report.OLD_Premium = rdr.GetDouble(rdr.GetOrdinal("OLD_PREMIUM"));
                    }
                    else
                    {
                        policy_report.Main_Policy = rdr.GetString(rdr.GetOrdinal("Main_Policy"));
                    }

                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status"));
                    policy_report.Round_Status_ID = rdr.GetString(rdr.GetOrdinal("Round_Status_ID"));
                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));
                    policy_report.Maturity_Date = rdr.GetDateTime(rdr.GetOrdinal("Maturity_Date"));

                    policy_detail_report_list.Add(policy_report);
                }
            }/// End loop

        }

        return policy_detail_report_list;
    }

    /// <summary>
    /// Get Next Due
    /// </summary>
    public static DateTime Get_Next_Due(string policy_id)
    {
        DateTime Due_Date = Get_Due_Date(policy_id);

        DateTime next_due = DateTime.Now;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Calculate_Next_Due";
            cmd.Parameters.AddWithValue("@due_date", Due_Date);
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                next_due = DateTime.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [Get_Next_Due] in class ['']. Details: " + ex.Message);
            }
        }
        return next_due;
    }


    /// <summary>
    /// Get Due Date
    /// </summary>
    public static DateTime Get_Due_Date(string policy_id)
    {
        DateTime due_date = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select Due_Date from Ct_Policy_Prem_Pay where Policy_ID =@Policy_ID order by Due_Date desc";
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                due_date = DateTime.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [Get_Due_Date] in class ['']. Details: " + ex.Message);
            }
        }
        return due_date;
    }

    /// <summary>
    /// Get Last Pay Date
    /// </summary>
    public static DateTime Get_Last_Pay_Date(string policy_id)
    {
        DateTime last_pay_date = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Last_Pay_Date";
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                last_pay_date = DateTime.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [Get_Last_Pay_Date] in class ['']. Details: " + ex.Message);
            }
        }
        return last_pay_date;
    }
    
    /// <summary>
    /// Get Agent Name
    /// </summary>
    public static string Get_Agent_Name_By_Policy_ID(string policy_id)
    {
        string agnet_name = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Agent_Name_By_Policy_ID";
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                agnet_name =cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [Get_Agent_Name_By_Policy_ID] in class ['']. Details: " + ex.Message);
            }
        }
        return agnet_name;
    }

    /// <summary>
    /// Get Comm rate
    /// </summary>
    public static double GetComm_Rate_by_product_id(string product_id)
    {
        double comm_rate = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = " ";
            cmd.Parameters.AddWithValue("@Product_ID", product_id);
            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();

            try
            {
                comm_rate =double.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [GetComm_Rate_by_product_id] in class ['']. Details: " + ex.Message);
            }
        }
        return comm_rate;
    }

    public static List<bl_policy_report> test_report_policy(DateTime from_date, DateTime to_date,
        string check_policy_status_code, int order_by, int pay_mode_id, string policy_number, string product_id)
    {
        List<bl_policy_report> policy_report_list = new List<bl_policy_report>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "", Order_by = "";

            SqlCommand cmd = new SqlCommand(sql, con);

          

            if (check_policy_status_code == "")
            {
                sql = @"select * from v_report_policy where 
                                (CONVERT(nvarchar(10),v_report_policy.Issue_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"select * from v_report_policy where 
                                (CONVERT(nvarchar(10),v_report_policy.Issue_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (lower(v_report_policy.Policy_Status_Type_ID) in (" + check_policy_status_code + "))";

            }

            if (order_by == 1) // Policy number
            {
                Order_by = " order by v_report_policy.Policy_Number asc";
            }
            else if (order_by == 2) // Issued date
            {
                Order_by = " order by v_report_policy.Issue_Date asc";
            }
            else if (order_by == 3)
            {
                Order_by = " order by v_report_policy.Effective_Date asc";
            }
            
            if (pay_mode_id != 0 && pay_mode_id !=-1)
            {
                sql += "  and v_report_policy.Pay_Mode_ID=@Pay_Mode_ID";
                cmd.Parameters.AddWithValue("@Pay_Mode_ID", pay_mode_id);
            } 

            if (policy_number != "")
            {
                sql += "  and v_report_policy.Policy_Number=@Policy_Number";
                cmd.Parameters.AddWithValue("@Policy_Number", policy_number);
            }

            if (product_id != "" && product_id !="-1")
            {
                sql += " and v_report_policy.Product_ID=@Product_ID   ";
                cmd.Parameters.AddWithValue("@Product_ID", product_id);
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
                    DateTime next_due = DateTime.Now;
                    bl_policy_report policy_report = new bl_policy_report();

                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.customer_name = rdr.GetString(rdr.GetOrdinal("customer_name"));
                    policy_report.En_Abbr = rdr.GetString(rdr.GetOrdinal("En_Abbr"));
                    policy_report.StatusCode = rdr.GetString(rdr.GetOrdinal("StatusCode"));
                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));
                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                    policy_report.Mode = rdr.GetString(rdr.GetOrdinal("Mode"));
                    policy_report.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Premium")) + rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                    policy_report.Is_Standard = rdr.GetString(rdr.GetOrdinal("Is_Standard"));
                    policy_report.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                    policy_report.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));

                    policy_report.Last_Payment = Get_Last_Pay_Date(policy_report.Policy_ID);
                    policy_report.Agent = Get_Agent_Name_By_Policy_ID(policy_report.Policy_ID);
                    /// Calculate Next Due
                    next_due = Get_Next_Due(policy_report.Policy_ID);

                    int i = next_due.Year;

                    if ((policy_report.Effective_Date).Day != next_due.Day)
                    {
                        if (((i % 4 == 0) && (i % 100 != 0) || (i % 400 == 0))) // Leap Year, 29 last day of Feb
                        {
                            if (next_due.Month == 2)
                            {
                                if (next_due.Day >= 29)
                                {
                                    if ((policy_report.Effective_Date).Day == 28)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                                else { policy_report.Next_Due_Date = next_due; }
                            }
                            else if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                            {
                                if (next_due.Day >= 31)
                                {
                                    if ((policy_report.Effective_Date).Day == 30)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                                else
                                {
                                    if ((policy_report.Effective_Date).Day > next_due.Day)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                            }
                            else
                            {
                                policy_report.Next_Due_Date = next_due;
                            }
                        }
                        else
                        {
                            if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                            {
                                if (next_due.Day >= 31)
                                {
                                    if ((policy_report.Effective_Date).Day == 30)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                                else
                                {
                                    if ((policy_report.Effective_Date).Day > next_due.Day)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays((policy_report.Effective_Date).Day - next_due.Day);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                            }
                            else
                            {
                                policy_report.Next_Due_Date = next_due;
                            }
                        }
                    }
                    else
                    {
                        policy_report.Next_Due_Date = next_due;
                    }
                    /// End of Calculate Next Due
                    policy_report_list.Add(policy_report);
                }
            }/// End loop
        }/// End of ConnectionString

        if (order_by != 4)
        {
            return policy_report_list;
        }
        else
        {
            List<bl_policy_report> sortedByNextDue = policy_report_list.OrderBy(s => s.Next_Due_Date).ToList();
            return sortedByNextDue;
        }
    }

    #region DAILY REPORT - Meas Sun ON 29-03-2020

    public static List<bl_policy_report> GetPolicyReportList(DateTime from_date, DateTime to_date, string check_policy_status_code, int order_by)
    {
        List<bl_policy_report> policy_report_list = new List<bl_policy_report>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            string sql = "", Order_by = "";

            if (check_policy_status_code == "")
            {
                sql = @"select * from v_report_policy where 
                                (CONVERT(nvarchar(10),v_report_policy.Issue_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111))";
            }
            else
            {
                sql = @"select * from v_report_policy where 
                                (CONVERT(nvarchar(10),v_report_policy.Issue_Date,111) between CONVERT(nvarchar(10),@from_date,111) 
                                 and CONVERT(nvarchar(10),@to_date,111)) and (lower(v_report_policy.Policy_Status_Type_ID) in (" + check_policy_status_code + "))";

            }

            if (order_by == 1) // Policy number
            {
                Order_by = " order by v_report_policy.Policy_Number asc";
            }
            else if (order_by == 2) // Issued date
            {
                Order_by = " order by v_report_policy.Issue_Date asc";
            }
            else if (order_by == 3)
            {
                Order_by = " order by v_report_policy.Effective_Date asc";
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
                    DateTime next_due = DateTime.Now;
                    bl_policy_report policy_report = new bl_policy_report();

                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.customer_name = rdr.GetString(rdr.GetOrdinal("customer_name"));
                    policy_report.En_Abbr = rdr.GetString(rdr.GetOrdinal("En_Abbr"));
                    policy_report.StatusCode = rdr.GetString(rdr.GetOrdinal("StatusCode"));
                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));
                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                    policy_report.Mode = rdr.GetString(rdr.GetOrdinal("Mode"));
                    policy_report.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Premium")) + rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                    policy_report.Is_Standard = rdr.GetString(rdr.GetOrdinal("Is_Standard"));
                    policy_report.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                    policy_report.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));

                    policy_report.Last_Payment = Get_Last_Pay_Date(policy_report.Policy_ID);
                    policy_report.Agent = Get_Agent_Name_By_Policy_ID(policy_report.Policy_ID);
                    /// Calculate Next Due
                    next_due = Get_Next_Due(policy_report.Policy_ID);

                    int i = next_due.Year;

                    if ((policy_report.Effective_Date).Day != next_due.Day)
                    {
                        if (((i % 4 == 0) && (i % 100 != 0) || (i % 400 == 0))) // Leap Year, 29 last day of Feb
                        {
                            if (next_due.Month == 2)
                            {
                                if (next_due.Day >= 29)
                                {
                                    if ((policy_report.Effective_Date).Day == 28)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                                else { policy_report.Next_Due_Date = next_due; }
                            }
                            else if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                            {
                                if (next_due.Day >= 31)
                                {
                                    if ((policy_report.Effective_Date).Day == 30)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                                else
                                {
                                    if ((policy_report.Effective_Date).Day > next_due.Day)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                            }
                            else
                            {
                                policy_report.Next_Due_Date = next_due;
                            }
                        }
                        else
                        {
                            if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                            {
                                if (next_due.Day >= 31)
                                {
                                    if ((policy_report.Effective_Date).Day == 30)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays(1);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                                else
                                {
                                    if ((policy_report.Effective_Date).Day > next_due.Day)
                                    {
                                        policy_report.Next_Due_Date = next_due.AddDays((policy_report.Effective_Date).Day - next_due.Day);
                                    }
                                    else { policy_report.Next_Due_Date = next_due; }
                                }
                            }
                            else
                            {
                                policy_report.Next_Due_Date = next_due;
                            }
                        }
                    }
                    else
                    {
                        policy_report.Next_Due_Date = next_due;
                    }
                    /// End of Calculate Next Due
                    policy_report_list.Add(policy_report);
                }
            }/// End loop
        }/// End of ConnectionString

        if (order_by != 4)
        {
            return policy_report_list;
        }
        else
        {
            List<bl_policy_report> sortedByNextDue = policy_report_list.OrderBy(s => s.Next_Due_Date).ToList();

            return sortedByNextDue;
        }
    }


    #endregion


    public static List<bl_policy_report> GetPolicyDetailReportLisByParams(string policy_number_search = "", string last_name_search = "", string first_name_search = "")
    {
        List<bl_policy_report> policy_detail_report_list = new List<bl_policy_report>();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GET_CL24_POLICY_REPORT_BY_PARAMS";
            cmd.Parameters.AddWithValue("@POLICY_NUMBER_SEARCH", policy_number_search);
            cmd.Parameters.AddWithValue("@FIRST_NAME_SEARCH", first_name_search);
            cmd.Parameters.AddWithValue("@LAST_NAME_SEARCH", last_name_search);
            cmd.Connection = con;
            con.Open();

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_policy_report policy_report = new bl_policy_report();

                    policy_report.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_report.App_Number = rdr.GetString(rdr.GetOrdinal("App_Number"));
                    policy_report.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                    policy_report.customer_name = rdr.GetString(rdr.GetOrdinal("Customer_Name"));
                    policy_report.Gender = rdr.GetString(rdr.GetOrdinal("Gender"));
                    policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                    policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                    policy_report.Product_Name = rdr.GetString(rdr.GetOrdinal("En_Title"));
                    policy_report.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));
                    policy_report.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));
                    policy_report.Assured_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                    policy_report.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_report.Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));

                    policy_report.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));

                    policy_report.Discount_Amount = rdr.GetDouble(rdr.GetOrdinal("Discount_Amount"));

                    policy_report.OLD_EM_Amount = rdr.GetDouble(rdr.GetOrdinal("OLD_EM_Amount"));

                    if (rdr.GetString(rdr.GetOrdinal("Main_Policy")) == "")
                    {
                        policy_report.Main_Policy = "Main";
                        policy_report.OLD_Premium = rdr.GetDouble(rdr.GetOrdinal("OLD_PREMIUM"));
                    }
                    else
                    {
                        policy_report.Main_Policy = rdr.GetString(rdr.GetOrdinal("Main_Policy"));
                    }

                    policy_report.Policy_Status_Type_ID = rdr.GetString(rdr.GetOrdinal("Policy_Status"));
                    policy_report.Round_Status_ID = rdr.GetString(rdr.GetOrdinal("Round_Status_ID"));
                    policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                    policy_report.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));
                    policy_report.Maturity_Date = rdr.GetDateTime(rdr.GetOrdinal("Maturity_Date"));

                    policy_detail_report_list.Add(policy_report);
                }
            }/// End loop

        }

        return policy_detail_report_list;
        
    }
}