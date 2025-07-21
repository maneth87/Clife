using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for da_report_sale
/// </summary>
public partial class da_report_sale_policy
{
    public static DataTable GetPolicyBySaleAgentIDAndEffectiveDate(string sale_agent_id, DateTime from_date, DateTime to_date, List<bl_sale_agent> sale_agent_list)
    {
        string connString = AppConfiguration.GetConnectionString(); DataTable dt = new DataTable();
        if (sale_agent_id == "")
        {
            for (int i = 0; i < sale_agent_list.Count; i++)
            {
                //Get obj sale agent of this index[i]
                bl_sale_agent sale_agent = new bl_sale_agent();
                sale_agent = sale_agent_list[i];
                try
                {
                    using (SqlConnection myConnection = new SqlConnection(connString))
                    {
                        TimeSpan start_time = new TimeSpan(00, 00, 00);
                        TimeSpan end_time = new TimeSpan(23, 59, 59);

                        from_date = from_date.Date + start_time;
                        to_date = to_date.Date + end_time;

                        SqlCommand myCommand = new SqlCommand();
                        myConnection.Open();
                        myCommand.Connection = myConnection;
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "SP_Get_Policy_Report_By_Agent_and_Effective";

                        myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent.Sale_Agent_ID);
                        myCommand.Parameters.AddWithValue("@From_Date", from_date);
                        myCommand.Parameters.AddWithValue("@To_Date", to_date);

                        if (from_date.Year != 1900 && to_date.Year != 1900)
                        {
                            myCommand.Parameters.AddWithValue("@Check_Date", 1);
                        }
                        else { myCommand.Parameters.AddWithValue("@Check_Date", 0); }

                        SqlDataAdapter dap = new SqlDataAdapter(myCommand);
                        dap.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [GetPolicyBySaleAgentIDAndEffectiveDate] in class [da_report_sale_policy]. Details: " + ex.Message);

                }
            }
        }
        else
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {
                    TimeSpan start_time = new TimeSpan(00, 00, 00);
                    TimeSpan end_time = new TimeSpan(23, 59, 59);

                    from_date = from_date.Date + start_time;
                    to_date = to_date.Date + end_time;

                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "SP_Get_Policy_Report_By_Agent_and_Effective";

                    myCommand.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
                    myCommand.Parameters.AddWithValue("@From_Date", from_date);
                    myCommand.Parameters.AddWithValue("@To_Date", to_date);

                    if (from_date.Year != 1900 && to_date.Year != 1900)
                    {
                        myCommand.Parameters.AddWithValue("@Check_Date", 1);
                    }
                    else { myCommand.Parameters.AddWithValue("@Check_Date", 0); }

                    SqlDataAdapter dap = new SqlDataAdapter(myCommand);
                    dap.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyBySaleAgentIDAndEffectiveDate] in class [da_report_sale_policy]. Details: " + ex.Message);

            }
        }
        return dt;
    }

    public static DataTable GetCustomerInfoByPolicyID(string policy_ID)
    {
        string connString = AppConfiguration.GetConnectionString(); DataTable dt = new DataTable();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Customer_For_Payment";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_ID);

                SqlDataAdapter dap = new SqlDataAdapter(myCommand);
                dap.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetCustomerInfoByPolicyID] in class [da_report_sale_policy]. Details: " + ex.Message);
        }
        return dt;
    }

    public static DataTable GetPolicyHistoryPaymentByPolicyID(string policy_ID, string from_year, string to_year)
    {
        string connString = AppConfiguration.GetConnectionString(); DataTable dt = new DataTable();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Payment_History";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_ID);

                if (from_year != "" && to_year != "")
                {
                    myCommand.Parameters.AddWithValue("@From_Year", int.Parse(from_year));
                    myCommand.Parameters.AddWithValue("@To_Year", int.Parse(to_year));
                    myCommand.Parameters.AddWithValue("@Check_PayYear", 1);
                    
                }
                else
                { 
                    myCommand.Parameters.AddWithValue("@From_Year", from_year);
                    myCommand.Parameters.AddWithValue("@To_Year", to_year);
                    myCommand.Parameters.AddWithValue("@Check_PayYear", 0);
                }

                SqlDataAdapter dap = new SqlDataAdapter(myCommand);
                dap.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetPolicyScheduleByPolicyID] in class [da_report_sale_policy]. Details: " + ex.Message);

        }
        return dt;
    }

    public static float GetDiscountRateByPremYear(string policy_id, int prem_year)
    {
        string connString = AppConfiguration.GetConnectionString(); float discount_rate=0;
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.CommandText = "SP_Get_DiscountRateByPremYear";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);
                myCommand.Parameters.AddWithValue("@Prem_Year", prem_year);

                discount_rate=float.Parse(myCommand.ExecuteScalar().ToString());
            }
        }
        catch (Exception ex)
        {
            discount_rate = 0;

        }
        return discount_rate;
    }

    public static float GetFactorByPolicyID(string policy_id)
    {
        string connString = AppConfiguration.GetConnectionString(); float factor_amount = 0;
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.CommandText = "SP_Get_Factor_By_Policy_ID";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);

                factor_amount = float.Parse(myCommand.ExecuteScalar().ToString());
            }
        }
        catch (Exception ex)
        {
            factor_amount = 0;

        }
        return factor_amount;
    }

    public static int GetPayYearByPolicyID(string policy_id)
    {
        string connString = AppConfiguration.GetConnectionString(); int pay_year = 1;
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"select Pay_Year from Ct_Policy where Policy_ID=@Policy_ID ";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);

                pay_year = int.Parse(myCommand.ExecuteScalar().ToString());
            }
        }
        catch (Exception ex)
        {
            pay_year = 1;
        }
        return pay_year;
    }

    public static string GetLastPremYearLotPolicyID(string policy_id)
    {
        string connString = AppConfiguration.GetConnectionString(); string pay_year_time = "";
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"select top(1) CONVERT(VARCHAR(10),Prem_Year)  + '/' + CONVERT(VARCHAR(10),Prem_Lot)
                                            from Ct_Policy_Prem_Pay where Policy_ID =@Policy_ID order by Due_Date desc";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);

                pay_year_time =myCommand.ExecuteScalar().ToString();
            }
        }
        catch (Exception ex)
        {
            pay_year_time = "";
        }
        return pay_year_time;
    }

    public static int GetLastPayMode(string policy_id)
    {
        string connString = AppConfiguration.GetConnectionString(); int pay_mode=-1;
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"select Pay_Mode from Ct_Policy_Pay_Mode where Policy_ID =@Policy_ID ";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);

                pay_mode =int.Parse(myCommand.ExecuteScalar().ToString());
            }
        }
        catch (Exception ex)
        {
            pay_mode = -1;
        }
        return pay_mode;
    }

    public static int CalculateNextPremYear(int pay_mode, int last_pay_year, int last_prem_lot)
    {  
        int next_pay_year=0;

        if (pay_mode == 1)
        {
            next_pay_year = last_pay_year + 1;
        }
        else if (pay_mode == 2)
        {
            if (last_prem_lot == 1)
            {
                next_pay_year = last_pay_year;
            }
            else { next_pay_year = last_pay_year + 1; }
        }
        else if (pay_mode == 3)
        {
            if (last_prem_lot < 4)
            {
                next_pay_year = last_pay_year;
            }
            else { next_pay_year = last_pay_year + 1; }
        }
        else if (pay_mode == 4)
        {
            if (last_prem_lot < 12)
            {
                next_pay_year = last_pay_year;
            }
            else { next_pay_year = last_pay_year + 1; }
        }
        return next_pay_year;
    }

    public static int CalculateBasicLotByMode(int pay_mode)
    {
        int basic_lot = 0;

        if (pay_mode == 1)
        {
            basic_lot =  1;
        }
        else if (pay_mode == 2)
        {
            basic_lot = 2;
        }
        else if (pay_mode == 3)
        {
            basic_lot = 4;
        }
        else if (pay_mode == 4)
        {
            basic_lot = 12;
        }
        return basic_lot;
    }

    public static DateTime GetLastDueByPolicy(string policy_id)
    {
        DateTime last_due = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString(); 
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"select TOP(1) Due_Date From Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID order by Due_Date desc";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);

                last_due = DateTime.Parse(myCommand.ExecuteScalar().ToString());
            }
        }
        catch (Exception ex)
        {
            last_due = DateTime.Now;

        }
        return last_due;
    }

    public static DateTime GetEffectiveDate(string policy_id)
    {
        DateTime effective_date = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"select Effective_Date from Ct_Policy where Policy_ID=@Policy_ID ";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);

                effective_date = DateTime.Parse(myCommand.ExecuteScalar().ToString());
            }
        }
        catch (Exception ex)
        {
            effective_date = DateTime.Now;
        }
        return effective_date;
    }

    /// <summary>
    /// Get Next Due by changing Pay Mode, 
    /// we just add month on Due_date base on Pay Mode
    /// </summary>
    public static DateTime GetNextDueByMode(int Pay_Mode_ID, DateTime due_date)
    {
        DateTime next_due = due_date; DateTime next_due_final = DateTime.Now;
        try
        {
            if (Pay_Mode_ID == 1) // Annual
            { next_due = due_date.AddMonths(12); }

            else if (Pay_Mode_ID == 2) // Semi
            { next_due = due_date.AddMonths(6); }

            else if (Pay_Mode_ID == 3) // Quarter
            { next_due = due_date.AddMonths(3); }

            else if (Pay_Mode_ID == 4) // Month
            { next_due = due_date.AddMonths(1); }

        }
        catch (Exception ex)
        {
            //write error to log
            Log.AddExceptionToLog("Error: class [da_report_sale_policy], function [GetNextDueByMode]. Details: " + ex.Message);
        }

        return next_due;
    }

    /// <summary>
    /// Get Next Due base on the calendar whether it is Leap Year or not. 
    /// Example: February: 28/29
    /// </summary>
    public static DateTime GetNextDue(int pay_mode_id, DateTime due_date, string policy_id)
    {
        DateTime result = DateTime.Now;
        try
        {
            DateTime effective_date = GetEffectiveDate(policy_id);

            DateTime next_due = GetNextDueByMode(pay_mode_id, due_date);

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
            Log.AddExceptionToLog("Error: class [da_report_sale_policy], function [GetNextDue]. Details: " + ex.Message);
        }

        return result;
    }

    public static string GetPremiumAndExtra(string policy_id)
    {
        string connString = AppConfiguration.GetConnectionString(); string prem_extra = "";
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"select top(1) CONVERT(VARCHAR(10),Premium)  + '/' + CONVERT(VARCHAR(10),EM_Amount) + '/' + CONVERT(VARCHAR(10),Original_Amount)
                                            from Ct_Policy_Premium where Policy_ID =@Policy_ID ";
                myCommand.Parameters.AddWithValue("@Policy_ID", policy_id);

                prem_extra = myCommand.ExecuteScalar().ToString();
            }
        }
        catch (Exception ex)
        {
            prem_extra = "";
        }
        return prem_extra;
    }
  
}