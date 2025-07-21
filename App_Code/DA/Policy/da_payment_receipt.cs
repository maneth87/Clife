using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for da_payment_receipt
/// </summary>
public class da_payment_receipt
{
    private static da_payment_receipt mytitle = null;
    public da_payment_receipt()
    {
        if (mytitle == null)
        {
            mytitle = new da_payment_receipt();
        }
    }


     /// <summary>
    /// Insert into Ct_Payment_Receipt
    /// </summary>
    public static bool Insert_Payment_Receipt(bl_payment_receipt payment_receipt)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"insert into Ct_Payment_Receipt(Receipt_ID,Policy_Prem_Pay_ID,Receipt_Num,Rate_Lapsed,Payment_Mode,Created_By,Created_On)
                                       values(@Receipt_ID,@Policy_Prem_Pay_ID,@Receipt_Num,@Rate_Lapsed,@Payment_Mode,@Created_By,@Created_On) ";

            cmd.Parameters.AddWithValue("@Receipt_ID", payment_receipt.Receipt_ID);
            cmd.Parameters.AddWithValue("@Policy_Prem_Pay_ID", payment_receipt.Policy_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@Receipt_Num", payment_receipt.Receipt_Num);
            cmd.Parameters.AddWithValue("@Rate_Lapsed", payment_receipt.Rate_Lapsed);
            cmd.Parameters.AddWithValue("@Payment_Mode", payment_receipt.Payment_Mode);
            cmd.Parameters.AddWithValue("@Created_On", payment_receipt.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", payment_receipt.Created_By);
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Insert_Payment_Receipt] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update Ct_Payment_Receipt
    /// </summary>
    public static bool Update_Payment_Receipt(bl_payment_receipt payment_receipt)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"update Ct_Payment_Receipt set
                                       Policy_Prem_Pay_ID=@Policy_Prem_Pay_ID,
                                       Receipt_Num=@Receipt_Num,
                                       Payment_Mode=@Payment_Mode,
                                       Created_By=@Created_By,
                                       Created_On=@Created_On
                                       where Receipt_ID=@Receipt_ID";

            cmd.Parameters.AddWithValue("@Receipt_ID", payment_receipt.Receipt_ID);
            cmd.Parameters.AddWithValue("@Policy_Prem_Pay_ID", payment_receipt.Policy_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@Receipt_Num", payment_receipt.Receipt_Num);
            cmd.Parameters.AddWithValue("@Payment_Mode", payment_receipt.Payment_Mode);
            cmd.Parameters.AddWithValue("@Created_On", payment_receipt.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", payment_receipt.Created_By);
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Update_Payment_Receipt] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// Update Ct_Payment_Receipt
    /// </summary>
    public static bool Update_Interest_Amount(string Policy_Prem_Pay_ID, decimal interest_amount)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"update Ct_Payment_Receipt set
                                       Rate_Lapsed=@Rate_Lapsed
                                      
                                       where Policy_Prem_Pay_ID=@Policy_Prem_Pay_ID";

     
            cmd.Parameters.AddWithValue("@Policy_Prem_Pay_ID", Policy_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@Rate_Lapsed", interest_amount);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Update_Interest_Amount] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static string Check_Duplicate_Payment_Receipt(string receipt_num)
    {
        string result = "";
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select Receipt_Num from Ct_Payment_Receipt where Receipt_Num=@Receipt_Num";

            cmd.Parameters.AddWithValue("@Receipt_Num", receipt_num);

            cmd.Connection = con;
            con.Open();
            try
            {
                result = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Duplicate_Payment_Receipt] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static bool  Check_Policy_Status(string policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select * from Ct_Policy_Status where Policy_ID=@Policy_ID and Policy_Status_Type_ID='LAP'";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();
            try
            {
                dap.Fill(dt);

                if (dt.Rows.Count > 0)
                { result = true; }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Check_Policy_Status] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static bool Update_Policy_Status(string policy_id, string Policy_Status_Type_ID)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"update Ct_Policy_Status set Policy_Status_Type_ID=@Policy_Status_Type_ID where Policy_ID=@Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", Policy_Status_Type_ID);

            cmd.Connection = con;

            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true; 
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Update_Policy_Status] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static bool Check_Pay_Year(string policy_id)
    {
        bool result = false;
        int pay_year_product = 0, pay_year_prem=0;
        
        try
        {
            pay_year_product = Pay_Year_Product(policy_id);

            pay_year_prem = Pay_Year_Prem(policy_id);

            if (pay_year_product == pay_year_prem)
            {
                result = true;
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [Check_Pay_Year] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        
        return result;
    }

    public static int Pay_Year_Product(string policy_id)
    {
        int result = 0;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"select Ct_Product_Life.Pay_Year from Ct_Product_Life 
                                inner join Ct_Policy on Ct_Product_Life.Product_ID=Ct_Policy.Product_ID
                                where Policy_ID=@Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;

            con.Open();
            try
            {

                result =int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Pay_Year_Product] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static int Pay_Year_Prem(string policy_id)
    {
        int result = 0;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
         
            string sql = @"select top(1) Prem_Year,Pay_Mode_ID from Ct_Policy_Prem_Pay
                                where Policy_ID=@Policy_ID order by Due_Date desc";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.CommandText = sql;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();

            DataTable dt=new DataTable();

            try
            {
                dap.Fill(dt);

                if (int.Parse(dt.Rows[0][1].ToString()) > 1)
                {
                    result = Pay_Year_Prem_By_Pay_Year(int.Parse(dt.Rows[0][0].ToString()));
                }
                else
                {
                    result = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Pay_Year_Prem] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static DataTable Report_Payment(string policy_num, string first_name, string last_name, DateTime from_date, DateTime to_date)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = "";

            SqlCommand cmd = new SqlCommand();

            sql = @"select Ct_Policy_Number.Policy_Number,Ct_Customer.Last_Name,Ct_Customer.First_Name,
		            case when Ct_Customer.Gender =0 then 'F'
		            else 'M' end as 'Gender',Ct_Policy_Prem_Pay.Prem_Year,Ct_Policy_Prem_Pay.Prem_Lot,
					case when Ct_Policy_Prem_Pay.Pay_Mode_ID =0 then 'Singal'
						 when Ct_Policy_Prem_Pay.Pay_Mode_ID = 1 then 'Annually'
						 when Ct_Policy_Prem_Pay.Pay_Mode_ID = 2 then 'Semi' 
						 when Ct_Policy_Prem_Pay.Pay_Mode_ID = 3 then 'Quarterly'
						 when Ct_Policy_Prem_Pay.Pay_Mode_ID = 4 then 'Monthly'
					else '' end as 'Mode',
		            Ct_Policy_Premium.Sum_Insure,Ct_Policy_Prem_Pay.Amount,
                    case when Ct_Payment_Receipt.Rate_Lapsed is null then 0
					else Ct_Payment_Receipt.Rate_Lapsed end as 'Rate_Lapsed',Due_Date,Ct_Policy_Prem_Pay.Pay_Date,Ct_Policy_Prem_Pay.Sale_Agent_ID,
		            Ct_Policy.Policy_ID,
		            Ct_Policy_Prem_Pay.Pay_Mode_ID,
		            case when Ct_Product.En_Abbr is null then '' 
		            else Ct_Product.En_Abbr end as 'En_Abbr',Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID,
		            case when Ct_Payment_Receipt.Receipt_Num is null then ''
		            else Ct_Payment_Receipt.Receipt_Num end as 'Receipt_Num',Ct_Policy_Status.Policy_Status_Type_ID from Ct_Policy 
		            inner join Ct_Policy_Pay_Mode on Ct_Policy.Policy_ID=Ct_Policy_Pay_Mode.Policy_ID
		            inner join Ct_Payment_Mode on Ct_Policy_Pay_Mode.Pay_Mode=Ct_Payment_Mode.Pay_Mode_ID
		            inner join Ct_Policy_Prem_Pay on Ct_Policy_Prem_Pay.Policy_ID=Ct_Policy.Policy_ID
		            inner join Ct_Policy_Number on Ct_Policy.Policy_ID=Ct_Policy_Number.Policy_ID
		            inner join Ct_Policy_Premium on Ct_Policy.Policy_ID=Ct_Policy_Premium.Policy_ID
		            inner join Ct_Policy_Status on Ct_Policy.Policy_ID=Ct_Policy_Status.Policy_ID
		            inner join Ct_Policy_Status_Type on Ct_Policy_Status.Policy_Status_Type_ID=Ct_Policy_Status_Type.Policy_Status_Type_ID
		            inner join Ct_Customer on Ct_Policy.Customer_ID=Ct_Customer.Customer_ID
		            inner join Ct_Product on Ct_Policy.Product_ID=Ct_Product.Product_ID
		            left join Ct_Payment_Receipt on Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID= Ct_Payment_Receipt.Policy_Prem_Pay_ID
                
                  ";

            string check_condition = "";

            if (policy_num != "")
            {
                cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
                check_condition = check_condition + " and Policy_Number=@Policy_Number";
            }

            if (first_name != "")
            {
                check_condition = check_condition + " and First_Name like '%" + first_name + "%'";
            }

            if (last_name != "")
            {
                check_condition = check_condition + " and Last_Name like '%" + last_name + "%'";
            }

            if (from_date != DateTime.Parse("01/01/1900"))
            {
               check_condition += " and  (Ct_Policy_Prem_Pay.Pay_Date between @From_Date and @To_Date)" ;

                cmd.Parameters.AddWithValue("@From_Date", from_date);
                cmd.Parameters.AddWithValue("@To_Date", to_date);
            }

            if (check_condition != "")
            {
                check_condition = check_condition.Remove(0, 4);
                sql = sql + " where " + check_condition;
            }

            cmd.CommandText = sql; 
            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();
            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Report_Payment] in class [da_payment_receipt]. Details: " + ex.Message);
            }
        }

        
        if (last_name != "" || first_name != "" || policy_num != "")
        {
            DataView view = dt.DefaultView;
            if (dt.Rows.Count > 0)
            {
                view.Sort = "Due_Date, Policy_Number ASC";
            }
            DataTable dt11 = view.ToTable();

            return dt11;
        }
        else
        {
            DataView view = dt.DefaultView;
            if (dt.Rows.Count > 0)
            {
                view.Sort = "Policy_Number, Due_Date ASC";
            }
            DataTable dt11 = view.ToTable();

            return dt11;
            
        }
    }

    public static DataTable Report_Policy_Reinstatement(string policy_num, string first_name, string last_name, DateTime from_date, DateTime to_date)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            dt.Columns.Add("Policy_Number");
            dt.Columns.Add("Customer_ID");
            dt.Columns.Add("insured_name");
            dt.Columns.Add("Product_ID");
            dt.Columns.Add("Sum_Insure");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Mode");
            dt.Columns.Add("Effective_Date");
            dt.Columns.Add("Due_Date");
            dt.Columns.Add("Pay_Date");
            dt.Columns.Add("duration_day");
            dt.Columns.Add("Prem_Interest");
            dt.Columns.Add("Full_Name");
            dt.Columns.Add("Sale_Agent_ID");

            DataRow r;

            string sql = "", check_condition="";

            SqlCommand cmd = new SqlCommand();

            sql = @"select distinct Ct_Policy.Policy_ID, Ct_Policy_Number.Policy_Number, Ct_Customer.Customer_ID, 
                        case when Gender=0 then 'Ms.'+' '+ Ct_Customer.Last_Name + ' ' + Ct_Customer.First_Name else
                        'Mr.'+' '+ Ct_Customer.Last_Name + '' + Ct_Customer.First_Name end 'insured_name',
                        Product_ID, Ct_Policy_Premium.Sum_Insure,Ct_Payment_Mode.Mode,
                        Ct_Policy.Effective_Date,
                        Ct_Sale_Agent.Full_Name, Ct_Sale_Agent.Sale_Agent_ID from Ct_Policy  
                        inner join Ct_Customer on Ct_Policy.Customer_ID=Ct_Customer.Customer_ID
                        inner join Ct_Policy_Number on Ct_Policy.Policy_ID=Ct_Policy_Number.Policy_ID
                        inner join Ct_Policy_Premium on Ct_Policy.Policy_ID=Ct_Policy_Premium.Policy_ID
                        inner join Ct_Policy_Pay_Mode on Ct_Policy.Policy_ID=Ct_Policy_Pay_Mode.Policy_ID
                        inner join Ct_Payment_Mode on Ct_Policy_Pay_Mode.Pay_Mode=Ct_Payment_Mode.Pay_Mode_ID
                        inner join Ct_Policy_Prem_Pay on Ct_Policy.Policy_ID=Ct_Policy_Prem_Pay.Policy_ID
                        inner join Ct_Sale_Agent on Ct_Policy_Prem_Pay.Sale_Agent_ID=Ct_Sale_Agent.Sale_Agent_ID";
   
            if (policy_num != "")
            {
                cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
                check_condition += " and Policy_Number=@Policy_Number";
            }

            if (first_name != "")
            {
                check_condition +=" and First_Name like '%" + first_name + "%'";
            }

            if (last_name != "")
            {
                check_condition +=" and Last_Name like '%" + last_name + "%'";
            }

            if (check_condition != "")
            {
                check_condition = check_condition.Remove(0,4);
                
                sql += " where " + check_condition;
            }

            cmd.CommandText = sql;
            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();
            try
            {

                DataTable dt_policy=new DataTable();

                dap.Fill(dt_policy);

                foreach (DataRow item in dt_policy.Rows)
                {
                    /// Get data from Ct_Interest_Amount

                    DataTable dt_interest_amount = Get_Interest_Amount(item["Policy_ID"].ToString(), from_date, to_date);

                    foreach (DataRow item_interest in dt_interest_amount.Rows)
                    {
                        string pay_mode = "";

                        r = dt.NewRow();

                        r["Policy_Number"] = item["Policy_Number"].ToString();
                        r["Customer_ID"] = item["Customer_ID"].ToString();
                        r["insured_name"] = item["insured_name"].ToString();
                        r["Product_ID"] = item["Product_ID"].ToString();
                        r["Sum_Insure"] = item["Sum_Insure"].ToString();
                        r["Effective_Date"] = item["Effective_Date"].ToString();
                        r["Full_Name"] = item["Full_Name"].ToString();
                        r["Sale_Agent_ID"] = item["Sale_Agent_ID"].ToString();

                        if (int.Parse(item_interest["pay_mode"].ToString()) == 0)
                        { pay_mode = "Single"; }
                        else if (int.Parse(item_interest["pay_mode"].ToString()) == 1)
                        { pay_mode = "Annually"; }
                        else if (int.Parse(item_interest["pay_mode"].ToString()) == 2)
                        { pay_mode = "Semi"; }
                        else if (int.Parse(item_interest["pay_mode"].ToString()) == 3)
                        { pay_mode = "Quarterly"; }
                        else
                        { pay_mode = "Monthly"; }

                        r["Mode"] = pay_mode;

                        decimal total_prem = Get_Prem_Lapsed_by_PolicyID(item["Policy_ID"].ToString(), from_date, to_date, int.Parse(item_interest["pay_mode"].ToString())), total_interest = decimal.Parse(item_interest["interest_amount"].ToString());

                        r["Amount"] = item_interest["amount_prem"].ToString();

                        r["Pay_Date"] = item_interest["date_rate"].ToString();

                        r["duration_day"] = item_interest["duration_day"].ToString();

                        r["Prem_Interest"] = (total_prem + total_interest).ToString();

                        r["Due_Date"] = item_interest["due_date"].ToString();

                        if (dt_policy.Rows.Count > 0 && dt_interest_amount.Rows.Count > 0)
                        {
                            dt.Rows.Add(r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Report_Policy_Reinstatement] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }

        if (last_name != "" || first_name != "" || policy_num != "")
        {
            DataView view = dt.DefaultView;
            view.Sort = "Policy_Number, Due_Date ASC";
            DataTable dt11 = view.ToTable();

            return dt11;
        }
        else
        {
            DataView view = dt.DefaultView;
            view.Sort = "Policy_Number,Due_Date  ASC";
            DataTable dt11 = view.ToTable();

            return dt11;
        }
    }

    public static int Pay_Year_Prem_By_Pay_Year(int Pay_Year)
    {
        int result = 0, total_pay_year=0, i=0;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql = @"select Prem_Year,Pay_Mode_ID from Ct_Policy_Prem_Pay
                                where Prem_Year=@Prem_Year order by Due_Date desc";

            cmd.Parameters.AddWithValue("@Prem_Year", Pay_Year);

            cmd.CommandText = sql;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();

            DataTable dt = new DataTable();

            try
            {
                dap.Fill(dt);

                if (int.Parse(dt.Rows[0][1].ToString()) > 1)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (int.Parse(item["Pay_Mode_ID"].ToString()) == 2)
                        {
                            total_pay_year += 6;
                        }
                        else if (int.Parse(item["Pay_Mode_ID"].ToString()) == 3)
                        {
                            total_pay_year += 4;
                        }
                        else if (int.Parse(item["Pay_Mode_ID"].ToString()) == 4)
                        {
                            total_pay_year += 1;
                        }
                    }

                    i = total_pay_year / 12;

                    if (i > 1)
                    {
                        i = Pay_Year + 1;
                    }
                    else if(i==1)
                    {
                        result = Pay_Year;
                    }
                    else if (i == 0)
                    { result = Pay_Year -1; }
                }
                else
                {
                    result = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Pay_Year_Prem_By_Pay_Year] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static DataTable Prem_For_LAP(string policy_id)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql = @"select Due_Date,Pay_Date,Pay_Mode_ID,Amount from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID and
                            Policy_Prem_Pay_ID in (select Policy_Prem_Pay_ID from Ct_Payment_Receipt where Rate_Lapsed >0) order by Due_Date desc";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.CommandText = sql;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();

            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Prem_For_LAP] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return dt;
    }

    public static DataTable Get_Interest_Amount(string policy_id, DateTime from_date, DateTime to_date)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql = @"select  date_rate,interest_amount,duration_day,due_date,pay_mode,amount_prem from Ct_Interest_Amount 
                            where date_rate in (select Pay_Date from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID) and
                            policy_id=@Policy_ID and 
                            (CONVERT(nvarchar(10),date_rate,111) between CONVERT(nvarchar(10),@From_Date,111) and 
                             CONVERT(nvarchar(10),@To_Date,111)) order by date_rate desc";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@From_Date", from_date);
            cmd.Parameters.AddWithValue("@To_Date", to_date);
            //cmd.Parameters.AddWithValue("@due_date", due_date);
            

            cmd.CommandText = sql;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();

            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Get_Interest_Amount] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return dt;
    }

    public static decimal Get_Interest_Lapsed(string policy_id, DateTime from_date, DateTime to_date)
    {
        decimal result = 0;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql="";

            if(from_date == DateTime.Parse("01/01/1900"))
            {
                sql = @"select interest_amount from Ct_Interest_Amount where policy_id=@Policy_ID
                            and  date_rate in (select Pay_Date from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID)";
            }
            else{
                sql = @"select interest_amount from Ct_Interest_Amount where policy_id=@Policy_ID
                            and  date_rate in (select Pay_Date from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID) and
                            (CONVERT(nvarchar(10),date_rate,111) between CONVERT(nvarchar(10),@from_date,111) and 
                             CONVERT(nvarchar(10),@to_date,111))";

                cmd.Parameters.AddWithValue("@from_date", from_date.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@to_date", to_date.ToString("yyyy/MM/dd"));
            }
							  
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
          
            cmd.CommandText = sql;

            cmd.Connection = con;

            DataTable dt = new DataTable();

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();

            try
            {
                dap.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        result += Math.Round(decimal.Parse(item["interest_amount"].ToString()));
                    }
                }
            }
            catch 
            {
                result = 0;
            }
        }
        return result;
    }

    public static decimal Get_Prem_Lapsed_by_PolicyID(string policy_id, DateTime from_date, DateTime to_date, int pay_mode)
    {
        decimal total_prem_lapsed = 0;

        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql = @"select Amount from Ct_Policy_Prem_Pay where Policy_Prem_Pay_ID in (select Policy_Prem_Pay_ID from Ct_Payment_Receipt where Rate_Lapsed >0)

                            and Policy_ID=@Policy_ID and 
                              (CONVERT(nvarchar(10),Pay_Date,111) between CONVERT(nvarchar(10),@from_date,111) and 
                             CONVERT(nvarchar(10),@to_date,111)) and Pay_Mode_ID=@pay_mode";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@from_date", from_date.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@to_date", to_date.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@pay_mode", pay_mode);

            cmd.CommandText = sql;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();

            try
            {
                dap.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        total_prem_lapsed += decimal.Parse(item["Amount"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                total_prem_lapsed = 0;
            }
        }
        return total_prem_lapsed;
    }

    public static DataTable Get_Pay_Mode(string policy_id)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            string sql = @"select Pay_Mode,case when Pay_Mode =0 then 'Singal'
						 when Pay_Mode = 1 then 'Annually'
						 when Pay_Mode = 2 then 'Semi' 
						 when Pay_Mode = 3 then 'Quarterly'
						 when Pay_Mode = 4 then 'Monthly'
					else '' end as 'Mode' from Ct_Policy_Pay_Mode where Policy_ID=@Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.CommandText = sql;

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            cmd.Connection = con;

            con.Open();

            try
            {
                dap.Fill(dt);
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [Get_Pay_Mode] in class [bl_payment_receipt]. Details: " + ex.Message);
            }
        }
        return dt;
    }
}