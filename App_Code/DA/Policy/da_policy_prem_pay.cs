using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Windows.Forms;


/// <summary>
/// Summary description for da_policy_payment
/// </summary>
public class da_policy_prem_pay
{
    private static da_policy_prem_pay mytitle = null;

    public da_policy_prem_pay()
    {
        if (mytitle == null)
        {
            mytitle = new da_policy_prem_pay();
        }
    }

    /// <summary>
    /// Insert into Ct_Policy_Prem_Pay
    /// </summary>
    public static bool InsertPolicy_Pre_Pay(bl_policy_prem_pay policy_prem_pay)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Policy_Pre_Pay";

            cmd.Parameters.AddWithValue("@Policy_Prem_Pay_ID", policy_prem_pay.Policy_Prem_Pay_ID);
            cmd.Parameters.AddWithValue("@Policy_ID", policy_prem_pay.Policy_ID);
            cmd.Parameters.AddWithValue("@Due_Date", policy_prem_pay.Due_Date);
            cmd.Parameters.AddWithValue("@Pay_Date", policy_prem_pay.Pay_Date);
            cmd.Parameters.AddWithValue("@Prem_Year", policy_prem_pay.Prem_Year);
            cmd.Parameters.AddWithValue("@Prem_Lot", policy_prem_pay.Prem_Lot);
            cmd.Parameters.AddWithValue("@Amount", policy_prem_pay.Amount);
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", policy_prem_pay.Sale_Agent_ID);
            cmd.Parameters.AddWithValue("@Office_ID", policy_prem_pay.Office_ID);
            cmd.Parameters.AddWithValue("@Created_On", policy_prem_pay.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy_prem_pay.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_prem_pay.Created_Note);
            cmd.Parameters.AddWithValue("@Pay_Mode_ID", policy_prem_pay.Pay_Mode_ID);

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
                Log.AddExceptionToLog("Error in function [InsertPolicy_Pre_Pay] in class [bl_policy_prem_pay]. Details: " + ex.Message);
            }
        }
        return result;
    }

    public static bl_policy_payment GetPreList(string policy_num, string customer_id)
    {
       bl_policy_payment policy_payment = new bl_policy_payment();

       string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_GetPre_by_PolicyID_CusID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);

            if (policy_num != "" && customer_id=="")
            {
                cmd.Parameters.AddWithValue("@check_type", 0);
            }
            if (customer_id != "" && policy_num=="")
            {
                cmd.Parameters.AddWithValue("@check_type", 1);
            }
            if (policy_num != "" && customer_id != "")
            { cmd.Parameters.AddWithValue("@check_type", 2); }


            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    policy_payment.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                    policy_payment.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                    policy_payment.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                    policy_payment.Gender = rdr.GetString(rdr.GetOrdinal("Gender"));
                    policy_payment.Prem_Year = rdr.GetInt32(rdr.GetOrdinal("Prem_Year"));
                    policy_payment.Mode = rdr.GetString(rdr.GetOrdinal("Mode"));
                    policy_payment.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    policy_payment.Amount = rdr.GetDouble(rdr.GetOrdinal("Amount"));
                    policy_payment.Due_Date = rdr.GetDateTime(rdr.GetOrdinal("Due_Date"));
                    policy_payment.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));
                    policy_payment.Office_ID = rdr.GetString(rdr.GetOrdinal("Office_ID"));
                    policy_payment.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                    policy_payment.Original_Prem = rdr.GetDouble(rdr.GetOrdinal("Original_Prem"));
                    policy_payment.Pay_Mode_ID = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode_ID"));

                }
            }
        }

        return policy_payment;
    }

    public static DataTable Get_Prem_Interest(string policy_prem_pay_id)
    {
        DataTable dt = new DataTable(); string receipt_no = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("select Receipt_Num from Ct_Payment_Receipt where Policy_Prem_Pay_ID=@Policy_Prem_Pay_ID", con);
            cmd.Parameters.AddWithValue("@Policy_Prem_Pay_ID", policy_prem_pay_id);

            con.Open();

            dt.Columns.Add("total_prem");
            dt.Columns.Add("total_interest");

            DataRow r;
            try
            {
                receipt_no = cmd.ExecuteScalar().ToString();

                r = dt.NewRow();

                r["total_prem"] = Get_Total_Prem(receipt_no);
                r["total_interest"] = Total_Prem_interest(receipt_no);

                dt.Rows.Add(r);
            }
            catch (Exception ex)
            {

            }
        }

        return dt;
    }

    public static decimal Total_Prem_interest(string receipt_num)
    {
        decimal total_prem = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("select sum(Rate_Lapsed) from Ct_Payment_Receipt where Receipt_Num=@Receipt_Num ", con);
            cmd.Parameters.AddWithValue("@Receipt_Num", receipt_num);

            con.Open();

            total_prem = decimal.Parse(cmd.ExecuteScalar().ToString());
        }

        return total_prem;
    }

    public static decimal Get_Total_Prem(string receipt_num)
    {
        decimal prem_total = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(@"select sum(Amount) from Ct_Policy_Prem_Pay where Policy_Prem_Pay_ID in 
                                                (select Policy_Prem_Pay_ID from Ct_Payment_Receipt where Receipt_Num=@Receipt_Num)", con);
            cmd.Parameters.AddWithValue("@Receipt_Num", receipt_num);

            con.Open();

            prem_total = decimal.Parse(cmd.ExecuteScalar().ToString());
        }

        return prem_total;
    }

    public static List<bl_policy_payment> GetPolicy_NO(string policy_num, string customer_id, string last_name, string first_name, int check_type)
    {
        List<bl_policy_payment> policy = new List<bl_policy_payment>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("SP_GetPre_by_PolicyID_CusID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);
            cmd.Parameters.AddWithValue("@Last_Name", last_name);
            cmd.Parameters.AddWithValue("@First_Name", first_name);
            cmd.Parameters.AddWithValue("@check_type", check_type);

            con.Open();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            dap.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    double total_prem = 0, interest_amount = 0, i = 0;

                    bl_policy_payment policy_payment = new bl_policy_payment();

                    policy_payment.Policy_Number = item["Policy_Number"].ToString();
                    policy_payment.Last_Name = item["Last_Name"].ToString();
                    policy_payment.First_Name = item["First_Name"].ToString();
                    policy_payment.Gender = item["Gender"].ToString();
                    policy_payment.Prem_Year = int.Parse(item["Prem_Year"].ToString());
                    policy_payment.Prem_Lot = int.Parse(item["Prem_Lot"].ToString());
                    policy_payment.Mode = item["Mode"].ToString();
                    policy_payment.Sum_Insure = double.Parse(item["Sum_Insure"].ToString());
                    //policy_payment.Amount = double.Parse(item["Amount"].ToString());
                    policy_payment.Sale_Agent_ID = item["Sale_Agent_ID"].ToString();
                    policy_payment.Office_ID = item["Office_ID"].ToString();
                    policy_payment.Policy_ID = item["Policy_ID"].ToString();
                    policy_payment.Original_Prem = double.Parse(item["Original_Prem"].ToString());
                    policy_payment.Pay_Mode_ID = int.Parse(item["Pay_Mode_ID"].ToString());
                    policy_payment.En_Abbr = item["En_Abbr"].ToString();
                    policy_payment.Normal_Prem = double.Parse(item["Amount"].ToString());
                    policy_payment.Normal_Due_Date = GetLast_Due_Date(item["Policy_ID"].ToString());// GetNext_Due(int.Parse(item["Pay_Mode_ID"].ToString()), GetLast_Due_Date(item["Policy_ID"].ToString())); //
                    policy_payment.Policy_Status_Type_ID = item["Policy_Status_Type_ID"].ToString();
                    policy_payment.Product_Type_ID = int.Parse(item["Product_Type_ID"].ToString());
                    policy_payment.Policy_Prem_Pay_ID = item["Policy_Prem_Pay_ID"].ToString();
                    policy_payment.Receipt_Num = item["Receipt_Num"].ToString();
                    policy_payment.Pay_Date = DateTime.Parse(item["Pay_Date"].ToString());
                    policy_payment.Customer_ID = item["Customer_ID"].ToString();

                    DataTable dt_prem_interest = Get_Prem_Interest(item["Policy_Prem_Pay_ID"].ToString());

                    //DataTable dt_due = GetLast_Due(item["Policy_ID"].ToString(), int.Parse(item["Pay_Mode_ID"].ToString()), DateTime.Now, decimal.Parse(item["Amount"].ToString()));

                    //if (dt_due.Rows.Count > 0 && item["Policy_Status_Type_ID"].ToString() == "LAP")
                    //{
                    //    //foreach (DataRow dr in dt_due.Rows)
                    //    //{
                    //    //    i = i + 1;

                    //    //    if (int.Parse(dr["duration"].ToString()) >= 0)
                    //    //    {
                    //    //        if (i <= dt_due.Rows.Count)
                    //    //        {
                    //    //            total_prem += float.Parse(dr["prem_amount"].ToString());

                    //    //            interest_amount += float.Parse(dr["interest_amount"].ToString());

                    //    //            if (i == dt_due.Rows.Count)
                    //    //            {
                    //    //                policy_payment.Due_Date = DateTime.Parse(dr["due_date"].ToString());
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //}

                    //    if (DateTime.Parse(policy_payment.Due_Date.ToString()).ToString("dd/MM/yyyy") == "01/01/0001")
                    //    {
                    //        policy_payment.Due_Date = policy_payment.Normal_Due_Date;
                    //    }

                    //    total_prem = total_prem + interest_amount;
                    //    total_prem = Math.Round(total_prem);
                    //    policy_payment.Amount = total_prem;
                    //}
                    //else
                    //{
                    //    policy_payment.Amount = double.Parse(item["Amount"].ToString());
                    //    policy_payment.Due_Date = policy_payment.Normal_Due_Date ;

                    //}

                    //policy_payment.Due_Date = GetLast_Due(item["Policy_ID"].ToString(), int.Parse(item["Pay_Mode_ID"].ToString()));

                    if (dt_prem_interest.Rows.Count > 0)
                    {
                        total_prem = double.Parse(dt_prem_interest.Rows[0][0].ToString());
                        interest_amount = double.Parse(dt_prem_interest.Rows[0][1].ToString());
                    }
                    else
                    {
                        total_prem = policy_payment.Normal_Prem;
                        interest_amount = 0;
                    }

                    total_prem = total_prem + interest_amount;
                    total_prem = Math.Round(total_prem);
                    policy_payment.Amount = total_prem;

                    policy_payment.Due_Date = policy_payment.Normal_Due_Date;

                    policy.Add(policy_payment);
                }
            }
            con.Close();
        }
        return policy;
    }

    public static List<bl_policy_payment> GetPolicy_Check_Lapsed(string policy_num, string customer_id, string last_name, string first_name, int check_type)
    {
        List<bl_policy_payment> policy = new List<bl_policy_payment>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("SP_GetPre_by_PolicyID_CusID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);
            cmd.Parameters.AddWithValue("@Last_Name", last_name);
            cmd.Parameters.AddWithValue("@First_Name", first_name);
            cmd.Parameters.AddWithValue("@check_type", check_type);

            con.Open();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            dap.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    bl_policy_payment policy_payment = new bl_policy_payment();
                    policy_payment.Policy_ID = item["Policy_ID"].ToString();
                    policy_payment.Pay_Mode_ID = int.Parse(item["Pay_Mode_ID"].ToString());
                    policy_payment.Amount = double.Parse(item["Amount"].ToString());
                    policy_payment.Policy_Status_Type_ID = item["Policy_Status_Type_ID"].ToString();

                    policy.Add(policy_payment);
                }
            }
            con.Close();
        }
        return policy;
    }

    public static DataTable GetPolicy_Info(string policy_num, string last_name, string first_name)
    {
        string connString = AppConfiguration.GetConnectionString();

        DataTable dt = new DataTable(); 

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = @"select Ct_Policy_Number.Policy_Number,Ct_Customer.Last_Name,Ct_Customer.First_Name from Ct_Policy 
		                    inner join Ct_Policy_Number on Ct_Policy.Policy_ID=Ct_Policy_Number.Policy_ID
		                    inner join Ct_Customer on Ct_Policy.Customer_ID=Ct_Customer.Customer_ID ";

            SqlCommand cmd = new SqlCommand();

            string condition_search = "";

            if (policy_num != "")
            {
                cmd.Parameters.AddWithValue("@Policy_Number", policy_num);

                condition_search += " and Policy_Number=@Policy_Number";
            }

            if (first_name != "")
            {
                condition_search += " and First_Name like '%" + first_name + "%'";
            }

            if (last_name != "")
            {
                condition_search += " and Last_Name like '%" + last_name + "%'";
            }

            if (condition_search != "")
            {
                condition_search = StringExtensions.SubstringAfter(condition_search, " and");

                sql = sql + " where" + condition_search;
            }

            cmd.CommandText = sql;

            cmd.Connection = con;

            con.Open();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            dap.Fill(dt);
            con.Close();

       }
        return dt;
    }

    public static DataTable GetPolicy_Lapsed(string policy_num, string last_name, string first_name, DateTime from_date)
    {
        #region V1
        /*
        string connString = AppConfiguration.GetConnectionString();

        if (from_date == DateTime.Parse("01/01/1900"))
        { from_date = DateTime.Now; }

        DataTable dt = new DataTable(); DataTable dt_final = new DataTable();

        using (SqlConnection con = new SqlConnection(connString))
        {
            string sql = @"select top(1) Ct_Policy_Number.Policy_Number,Ct_Customer.Last_Name,Ct_Customer.First_Name,
					case when Ct_Customer.Gender =0 then 'F'
					else 'M' end as 'Gender',Ct_Policy_Prem_Pay.Prem_Year,Ct_Policy_Prem_Pay.Prem_Lot,Ct_Payment_Mode.Mode,
					Ct_Policy_Premium.Sum_Insure,Ct_Policy_Prem_Pay.Amount,Due_Date,Ct_Policy_Prem_Pay.Sale_Agent_ID,
					Ct_Policy_Prem_Pay.Office_ID,Ct_Policy.Policy_ID,
					case when Ct_Policy_Premium.Original_Amount IS NULL then 0
					else Ct_Policy_Premium.Original_Amount end as 'Original_Amount',Ct_Payment_Mode.Pay_Mode_ID,
				    case when Ct_Product.En_Abbr is null then '' 
					else Ct_Product.En_Abbr end as 'En_Abbr',Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID,
					case when Ct_Payment_Receipt.Receipt_Num is null then ''
					else Ct_Payment_Receipt.Receipt_Num end as 'Receipt_Num',Ct_Policy_Status.Policy_Status_Type_ID,Ct_Product.Product_Type_ID from Ct_Policy 
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

                    where Ct_Policy_Status.Policy_Status_Type_ID not in ('MAT')";

            SqlCommand cmd = new SqlCommand();

            if (policy_num != "")
            {

                sql = sql + " and Policy_Number like '%" + policy_num + "%'";
            }

            if (first_name != "")
            {
                sql = sql + " and First_Name like '%" + first_name + "%'";
            }

            if (last_name != "")
            {
                sql = sql + " and Last_Name like '%" + last_name + "%'";
            }

            sql = sql + " order by Due_Date desc";

            cmd.CommandText = sql;

            cmd.Connection = con;

            con.Open();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            dap.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                

                dt_final.Columns.Add("Policy_ID");
                dt_final.Columns.Add("Mode_ID");
                dt_final.Columns.Add("Customer");
                dt_final.Columns.Add("Gender");
                dt_final.Columns.Add("Year");
                dt_final.Columns.Add("Times");
                dt_final.Columns.Add("Mode");
                dt_final.Columns.Add("SumIn");
                dt_final.Columns.Add("Amount");
                dt_final.Columns.Add("Due_Date");
                dt_final.Columns.Add("Interest");
                dt_final.Columns.Add("duration_day");
                dt_final.Columns.Add("month");
                dt_final.Columns.Add("Product");
                dt_final.Columns.Add("Pay_Date");
                dt_final.Columns.Add("Polno");

                DataRow r;

                foreach (DataRow dr in dt.Rows)
                {
                    DataTable dt1 = da_policy_prem_pay.GetLast_Due(dr["Policy_ID"].ToString(), int.Parse(dr["Pay_Mode_ID"].ToString()), from_date, decimal.Parse(dr["Amount"].ToString()), "", GetFirst_Due_Date(dr["Policy_ID"].ToString()));

                    foreach (DataRow item in dt1.Rows)
                    {
                        r = dt_final.NewRow();

                        r["Policy_ID"] = dr["Policy_ID"].ToString();
                        r["Mode_ID"] = dr["Pay_Mode_ID"].ToString();
                        r["Customer"] = dr["Last_Name"].ToString() + " " + dr["First_Name"].ToString();
                        r["Gender"] = dr["Gender"].ToString();
                        r["Year"] = dr["Prem_Year"].ToString();
                        r["Times"] = dr["Prem_Lot"].ToString();
                        r["Mode"] = dr["Mode"].ToString();
                        r["SumIn"] = dr["Sum_Insure"].ToString();
                        r["Due_Date"] = DateTime.Parse(item["due_date"].ToString());

                        // Calculate Day
                        TimeSpan ts = from_date.Subtract(DateTime.Parse(r["Due_Date"].ToString()));
                        int totalday = (int)Math.Ceiling(ts.TotalDays);

                        r["Amount"] = dr["Amount"].ToString();
                       
                        r["Interest"] = item["interest_amount"].ToString();
                        r["duration_day"] = item["duration"].ToString();
                        r["month"] = item["duration_month"].ToString();
                        r["Product"] = dr["En_Abbr"].ToString(); ;
                        r["Pay_Date"] = item["due_date"].ToString();
                        r["Polno"] = dr["Policy_Number"].ToString(); ;

                        if (int.Parse(r["duration_day"].ToString()) > 0)
                        {
                            dt_final.Rows.Add(r);
                        }
                    }
                }

            }
        }
        return dt_final;
         */
#endregion

        /*V2 by maneth 31 May 2019*/
        #region V2
        if (from_date == DateTime.Parse("01/01/1900"))
        { 
            from_date = DateTime.Now;
        }

        DataTable dt = new DataTable(); 
        DataTable dt_final = new DataTable();

        // init connection element
        SqlCommand cmd = new SqlCommand();
              
        string sql = @"select top(1) Ct_Policy_Number.Policy_Number,Ct_Customer.Last_Name,Ct_Customer.First_Name,
					case when Ct_Customer.Gender =0 then 'F'
					else 'M' end as 'Gender',Ct_Policy_Prem_Pay.Prem_Year,Ct_Policy_Prem_Pay.Prem_Lot,Ct_Payment_Mode.Mode,
					Ct_Policy_Premium.Sum_Insure,((Ct_Policy_Premium.Premium + Ct_Policy_Premium.EM_Amount) - Ct_Policy_Premium.Discount_Amount) as Amount,Due_Date,Ct_Policy_Prem_Pay.Sale_Agent_ID,
					Ct_Policy_Prem_Pay.Office_ID,Ct_Policy.Policy_ID,
					case when Ct_Policy_Premium.Original_Amount IS NULL then 0
					else Ct_Policy_Premium.Original_Amount end as 'Original_Amount',Ct_Payment_Mode.Pay_Mode_ID,
				    case when Ct_Product.En_Abbr is null then '' 
					else Ct_Product.En_Abbr end as 'En_Abbr',Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID,
					case when Ct_Payment_Receipt.Receipt_Num is null then ''
					else Ct_Payment_Receipt.Receipt_Num end as 'Receipt_Num',Ct_Policy_Status.Policy_Status_Type_ID,Ct_Product.Product_Type_ID from Ct_Policy 
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

                    where Ct_Policy_Status.Policy_Status_Type_ID not in ('MAT') ";



        if (policy_num != "")
        {
           
            sql = sql + " and Policy_Number=@Policy_Number";
        }

        if (first_name != "")
        {
            sql = sql + " and First_Name like '%" + first_name + "%'";
        }

        if (last_name != "")
        {
            sql = sql + " and Last_Name like '%" + last_name + "%'";
        }

        sql = sql + " order by due_date desc";

        dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), sql, policy_num != "" ? new string[,] { { "@Policy_Number", policy_num } } : new string[,] { });

        if (dt.Rows.Count > 0)
        {
            #region Combind with sub policies <<By:Maneth, Date: 02 Aug 2018>>
            //total main policy and sub policy
            bl_sub_policy_premium subPolicies = GetSubPoliciesTotalPremium(dt.Rows[0]["policy_id"].ToString());
            //update premium, original_amount, em_amount, em_premium, discount_amount and SI in datatable
            foreach (DataRow row in dt.Rows)
            {
                row["amount"] = Convert.ToDouble(row["amount"].ToString()) + subPolicies.Premium;
                row["original_amount"] = Convert.ToDouble(row["original_amount"].ToString()) + subPolicies.OriginalAmount;
                //row["em_amount"] = Convert.ToDouble(row["em_amount"].ToString()) + subPolicies.EmAmount;
                //row["em_premium"] = Convert.ToDouble(row["em_premium"].ToString()) + subPolicies.EmPremium;
                //row["discount_amount"] = Convert.ToDouble(row["discount_amount"].ToString()) + subPolicies.DiscountAmount;
                row["sum_insure"] = Convert.ToDouble(row["sum_insure"].ToString()) + subPolicies.SumInsure;

                dt.AcceptChanges();
            }
            #endregion


            dt_final.Columns.Add("Policy_ID");
            dt_final.Columns.Add("Mode_ID");
            dt_final.Columns.Add("Customer");
            dt_final.Columns.Add("Gender");
            dt_final.Columns.Add("YearTimes");
            dt_final.Columns.Add("Mode");
            dt_final.Columns.Add("SumIn");
            dt_final.Columns.Add("Amount");
            dt_final.Columns.Add("Due_Date");
            dt_final.Columns.Add("Interest");
            dt_final.Columns.Add("duration_day");
            dt_final.Columns.Add("month");
            dt_final.Columns.Add("Product");
            dt_final.Columns.Add("Pay_Date");
            dt_final.Columns.Add("Polno");

            DataRow r;

            foreach (DataRow dr in dt.Rows)
            {

                int prem_year = int.Parse(dt.Rows[0]["Prem_Year"].ToString()), prem_lot = int.Parse(dt.Rows[0]["Prem_Lot"].ToString());
                int total_remaining_month = 0, total_month = 0;

                DataTable dt1 = da_policy_prem_pay.GetLast_Due(dr["Policy_ID"].ToString(), int.Parse(dr["Pay_Mode_ID"].ToString()), from_date, decimal.Parse(dr["Amount"].ToString()), "");

                DateTime due_date = DateTime.Parse(dr["due_date"].ToString());

                /// Get last Pay Mode from Prem_Pay
                int last_pay_mode = GetLastPayModeFromPremPay(dr["Policy_ID"].ToString());

                /// Check Last Due Date by Pay Mode
                if (last_pay_mode != int.Parse(dr["Pay_Mode_ID"].ToString().ToString()))
                {
                    total_remaining_month = da_policy_prem_pay.Check_Remaining_Month(dt.Rows[0]["Policy_ID"].ToString(), last_pay_mode, 1);

                    if (last_pay_mode == 1)
                    {
                        due_date = due_date.AddYears(1);
                        due_date = due_date.AddMonths(-1);
                    }
                    else if (last_pay_mode == 2)
                    {
                        due_date = due_date.AddMonths(6);
                        due_date = due_date.AddMonths(-1);
                    }
                    else if (last_pay_mode == 3)
                    {
                        due_date = due_date.AddMonths(3);
                        due_date = due_date.AddMonths(-1);
                    }

                    due_date = CheckDayNextDueWithEffectiveDay(last_pay_mode, due_date, dt.Rows[0]["Policy_ID"].ToString());
                }
                else
                {
                    total_remaining_month = da_policy_prem_pay.Check_Remaining_Month(dt.Rows[0]["Policy_ID"].ToString(), int.Parse(dr["Pay_Mode_ID"].ToString().ToString()), 1);
                }/// End of checking Due Date by Pay Mode

                if (total_remaining_month == 0) { total_remaining_month = 12; }

                total_month = 12 - total_remaining_month;

                foreach (DataRow item in dt1.Rows)
                {
                    string year_time = CalPremYearLot(dt.Rows[0]["Policy_ID"].ToString(), int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()), prem_year, prem_lot, total_month);

                    if (year_time != "")
                    {
                        string[] w_policy_num = year_time.Split(',');

                        prem_year = int.Parse(w_policy_num[0].ToString().Trim()); prem_lot = int.Parse(w_policy_num[1].ToString().Trim());
                    }

                    r = dt_final.NewRow();

                    r["Policy_ID"] = dr["Policy_ID"].ToString();
                    r["Mode_ID"] = dr["Pay_Mode_ID"].ToString();
                    r["Customer"] = dr["Last_Name"].ToString() + " " + dr["First_Name"].ToString();
                    r["Gender"] = dr["Gender"].ToString();
                    r["YearTimes"] = prem_year.ToString() + "/" + prem_lot.ToString();
                    r["Mode"] = dr["Mode"].ToString();
                    r["SumIn"] = dr["Sum_Insure"].ToString();
                    r["Amount"] = dr["Amount"].ToString();
                    r["Due_Date"] = GetNext_Due(int.Parse(dr["Pay_Mode_ID"].ToString()), due_date, dr["Policy_ID"].ToString());
                    r["Interest"] = item["interest_amount"].ToString();
                    r["duration_day"] = item["duration"].ToString();
                    r["month"] = item["duration_month"].ToString();
                    r["Product"] = dr["En_Abbr"].ToString(); ;
                    r["Pay_Date"] = item["due_date"].ToString();
                    r["Polno"] = dr["Policy_Number"].ToString(); ;

                    if (dt1.Rows.Count > 1)
                    {
                        dt_final.Rows.Add(r);

                        due_date = DateTime.Parse(r["Due_Date"].ToString());

                        /// Check total month
                        if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 1)
                        {
                            total_remaining_month = 12;
                        }
                        else if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 2)
                        {
                            total_remaining_month = total_remaining_month + 6;
                        }
                        else if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 3)
                        {
                            total_remaining_month = total_remaining_month + 3;
                        }
                        else if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 4)
                        {
                            total_remaining_month = total_remaining_month + 1;
                        }
                    }
                    else
                    {
                        if (int.Parse(item["duration"].ToString()) != 0)
                        {
                            dt_final.Rows.Add(r);

                            due_date = DateTime.Parse(r["Due_Date"].ToString());

                            /// Check total month
                            if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 1)
                            {
                                total_remaining_month = 12;
                            }
                            else if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 2)
                            {
                                total_remaining_month = total_remaining_month + 6;
                            }
                            else if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 3)
                            {
                                total_remaining_month = total_remaining_month + 3;
                            }
                            else if (int.Parse(dt.Rows[0]["Pay_Mode_ID"].ToString()) == 4)
                            {
                                total_remaining_month = total_remaining_month + 1;
                            }
                        }
                    }
                }
            }
        }

        return dt_final;
        #endregion
    }
    /// <summary>
    /// Get Next Due base on the calendar whether it is Leap Year or not. 
    /// Example: February: 28/29
    /// </summary>
    public static DateTime GetNext_Due(int pay_mode_id, DateTime due_date, string policy_id)
    {
        DateTime result = DateTime.Now;
        try
        {
         
            DateTime effective_date = da_officail_receipt.GetEffectiveDate(policy_id);

            DateTime next_due = Get_Next_Due_by(pay_mode_id, due_date);

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
            Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [GetNext_Due]. Details: " + ex.Message);
        }

        return result;
    }

    private static string CalPremYearLot(string policy_id, int pay_mode, int prem_year, int prem_lot, int total_month_all)
    {
        string year_lot = "";

        if (pay_mode == 1)
        {
            if (total_month_all == 12)
            {
                prem_year = prem_year + 1;
                prem_lot = 1;
            }
            else
            {
                prem_year = prem_year + 1;
                prem_lot = 1;
            }
        }
        else if (pay_mode == 2)
        {
            if (total_month_all == 12)
            {
                prem_year = prem_year + 1;
                prem_lot = 1;
            }
            else
            {
                if (total_month_all < 12)
                {
                    prem_lot = prem_lot + 1;
                }
                else
                {
                    prem_year = prem_year + 1;
                    prem_lot = 1;
                }
            }
        }
        else if (pay_mode == 3)
        {
            if (total_month_all == 12)
            {
                prem_year = prem_year + 1;
                prem_lot = 1;
            }
            else
            {
                if (total_month_all < 12)
                {
                    prem_lot = prem_lot + 1;
                }
                else
                {
                    prem_year = prem_year + 1;
                    prem_lot = 1;
                }
            }
        }
        else if (pay_mode == 4)
        {
            if (total_month_all == 12)
            {
                prem_year = prem_year + 1;
                prem_lot = 1;
            }
            else
            {
                if (prem_lot < 12)
                {
                    prem_lot = prem_lot + 1;
                }
                else
                {
                    prem_lot = 1;
                    prem_year = prem_year + 1;
                }
            }
        }
        return year_lot = prem_year.ToString() + "," + prem_lot.ToString();
    }

    public static DateTime CheckDayNextDueWithEffectiveDay(int pay_mode_id, DateTime next_due, string policy_id)
    {
        DateTime result = DateTime.Now;
        try
        {
            DateTime effective_date = da_officail_receipt.GetEffectiveDate(policy_id);

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

    public static DateTime Get_Next_Due_by(int Pay_Mode_ID, DateTime due_date)
    {
        DateTime next_due = DateTime.Now;

        if (Pay_Mode_ID == 1) // Annual
        { next_due = due_date.AddMonths(12); }

        else if (Pay_Mode_ID == 2) // Semi
        { next_due = due_date.AddMonths(6); }

        else if (Pay_Mode_ID == 3) // Quarter
        { next_due = due_date.AddMonths(3); }

        else if (Pay_Mode_ID == 4) // Month
        { next_due = due_date.AddMonths(1); }

        return next_due;
    }

    public static DateTime GetNext_Due ( int pay_mode_id,DateTime due_date)
    {
        DateTime result = DateTime.Now;
        try
        {

            DateTime next_due =Get_Next_Due_by(pay_mode_id,due_date);

            int i = next_due.Year;

            if (due_date.Day != next_due.Day)
            {
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
                    else if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                    {
                        if (next_due.Day >= 31)
                        {
                            if (due_date.Day == 30)
                            {
                                result = next_due.AddDays(1);
                            }
                            else { result = next_due; }
                        }
                        else
                        {
                            if (due_date.Day > next_due.Day)
                            {
                                due_date = next_due.AddDays(1);
                            }
                            else { due_date = next_due; }
                        }
                    }
                    else
                    {
                        due_date = next_due;
                    }
                }
                else
                {
                    if (next_due.Month == 1 || next_due.Month == 3 || next_due.Month == 5 || next_due.Month == 7 || next_due.Month == 8 || next_due.Month == 11 || next_due.Month == 12)
                    {
                        if (next_due.Day >= 31)
                        {
                            if (due_date.Day == 30)
                            {
                                result = next_due.AddDays(1);
                            }
                            else { result = next_due; }
                        }
                        else
                        {
                            if (due_date.Day > next_due.Day)
                            {
                                result = next_due.AddDays(due_date.Day - next_due.Day);
                            }
                            else { result = next_due; }
                        }
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
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetNext_Due] in class [bl_policy_payment]. Details: " + ex.Message);
        }
        
        return result;
    }

    public static string GetPre_by_Poli_Num_Mode(string policy_num, int Pay_Mode_ID)
    {
        string policy_pre = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_Prem_by_Poli_Num_PayMode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
            cmd.Parameters.AddWithValue("@Pay_Mode_ID", Pay_Mode_ID);

            con.Open();

            decimal pre =decimal.Parse(cmd.ExecuteScalar().ToString());

            pre =Math.Round(pre);

            /// we have to calculate Original Prem, if it is Zero. But be careful with Old and New Product

            policy_pre = pre.ToString();
        }

        return policy_pre;
    }

    public static int GetLast_PayMode(string policy_id)
    {
        int pay_mode_id = -1;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(@"select top(1) Prem_Year from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID order by Due_Date desc", con);
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            con.Open();

            try
            {
                pay_mode_id = int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception)
            {

                pay_mode_id = -1;
            }
             
        }

        return pay_mode_id;
    }

    public static int Check_Remaining_Month(string policy_id, int new_pay_mode, int Product_Type)
    {
        int remaining_month = -1;

        int prem_year = GetLast_PayMode(policy_id), total_month=0;

        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(@"select Pay_Mode_ID from Ct_Policy_Prem_Pay 
                                                where Policy_ID=@Policy_ID and Prem_Year=@Prem_Year", con);

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Prem_Year", prem_year);

            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();

            dap.Fill(dt);

            if (dt.Rows.Count != 0 && Product_Type !=2)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["Pay_Mode_ID"].ToString() != "")
                    {
                        if (int.Parse(item["Pay_Mode_ID"].ToString()) == 1)
                        {
                            total_month += 12;
                        }
                        else if (int.Parse(item["Pay_Mode_ID"].ToString()) == 2)
                        {
                            total_month += 6;
                        }
                        else if (int.Parse(item["Pay_Mode_ID"].ToString()) == 3)
                        {
                            total_month += 3;
                        }
                        else if (int.Parse(item["Pay_Mode_ID"].ToString()) == 4)
                        {
                            total_month += 1;
                        }
                    }
                    else
                    {
                        int pay_mode = GetPay_Mode_by_PolicyID(policy_id);

                        if (pay_mode == 1)
                        {
                            total_month += 12;
                        }
                        else if (pay_mode == 2)
                        {
                            total_month += 6;
                        }
                        else if (pay_mode == 3)
                        {
                            total_month += 3;
                        }
                        else if (pay_mode == 4)
                        {
                            total_month += 1;
                        }
                    }
                }
            }

            remaining_month = 12 - total_month;
        }

        return remaining_month;
    }

    public static int GetPay_Mode_by_PolicyID(string policy_id)
    {
        int pay_mode =-1;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("select Pay_Mode from Ct_Policy_Pay_Mode where Policy_ID=@Policy_ID", con);

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            con.Open();

            pay_mode =int.Parse(cmd.ExecuteScalar().ToString());
       }

        return pay_mode;
    }

    public static bool Update_Pay_Mode(string policy_id, int pay_mode)
    {
        bool result = false;    

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("update Ct_Policy_Pay_Mode set Pay_Mode=@Pay_Mode where Policy_ID=@Policy_ID", con);

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);

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

    /// <summary>
    /// Get total Months by Pre_Year
    /// </summary>

    //public static int GetRemaining_Left_Month (string policy_id)
    //{
    //    int total_month = 0, pay_mode=-1;

    //    string connString = AppConfiguration.GetConnectionString();

    //    using (SqlConnection con = new SqlConnection(connString))
    //    {
    //        try
    //        {
    //            SqlCommand cmd = new SqlCommand(@"select Pay_Mode from Ct_Policy_Pay_Mode where Policy_ID=@Policy_ID", con);

    //            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

    //            con.Open();

    //            pay_mode = int.Parse(cmd.ExecuteScalar().ToString());

    //            if (pay_mode == 0 || pay_mode == 1)
    //            {
    //                total_month = 12;
    //            }
    //            else if (pay_mode == 2)
    //            {
    //                total_month = 6;
    //            }
    //            else if (pay_mode == 3)
    //            {
    //                total_month = 3;
    //            }
    //            else { total_month = 1; }
    //        }
    //        catch 
    //        {
    //            total_month = 0;
    //        }
    //    }

    //    return total_month;
    //}
    /// <summary>
    /// Generate new policy prem year
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static int GetLast_Prem_Year(string policy_id)
    {
        int prem_year = 0;
        int total_month = Check_Remaining_Month(policy_id,0,1);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Prem_Year from Ct_Policy_Prem_Pay 
                                                        where Policy_ID=@Policy_ID order by Due_Date desc
                                                        ", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                con.Open();

                prem_year = int.Parse(cmd.ExecuteScalar().ToString());

                if (total_month == 0)
                {
                    prem_year=prem_year + 1;
                }
            }
            catch
            {
                prem_year = 0;
            }
        }

        return prem_year;
    }
   
    /// <summary>
    /// Get last policy prem year
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static int GetLast_Policy_Prem_Year(string policy_id)
    {
        int prem_year = 0;
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Prem_Year from Ct_Policy_Prem_Pay 
                                                        where Policy_ID=@Policy_ID order by Due_Date desc
                                                        ", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                con.Open();

                prem_year = int.Parse(cmd.ExecuteScalar().ToString());

               
            }
            catch
            {
                prem_year = 0;
            }
        }

        return prem_year;
    }
    public static string GetReceipt_No_Due(string Receipt_Num, string Policy_Prem_Pay_ID, DateTime Due_Date, int save_edit, string policy_id)
    {
        string receipNo = ""; int yes_no = GetLast_Due_by_Due(Due_Date,policy_id);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand();

                string sql="";
                if(save_edit==1)
                {
                    sql=@"select Receipt_Num from Ct_Payment_Receipt where Receipt_Num=@Receipt_Num" ;
                    cmd.Parameters.AddWithValue("@Receipt_Num", Receipt_Num);
                }
                else{
                    sql = @"select Receipt_Num from Ct_Payment_Receipt where Receipt_Num=@Receipt_Num and Policy_Prem_Pay_ID <>@Policy_Prem_Pay_ID";
                    cmd.Parameters.AddWithValue("@Receipt_Num", Receipt_Num);
                    cmd.Parameters.AddWithValue("@Policy_Prem_Pay_ID", Policy_Prem_Pay_ID);

                }

                cmd.CommandText=sql;
                cmd.Connection = con;
                con.Open();

                receipNo = cmd.ExecuteScalar().ToString();

                if (receipNo != "")
                {
                    receipNo = "Receipt No (" + Receipt_Num + ")" ;
                }

                if (yes_no == 1)
                {
                    receipNo += "Due Date (" + Due_Date + ")";
                }

                if (receipNo != "") { receipNo += " have duplicated, please check again"; }
            }
            catch
            {
                receipNo="";
            }
        }

        return receipNo;
    }

    public static int GetLast_Due_by_Due(DateTime due,string policy_id)
    {
        int yes_no=0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select Due_Date from Ct_Policy_Prem_Pay where Convert(nvarchar(10),Due_Date,111)=Convert(nvarchar(10),@Due_Date,111) and Policy_ID=@Policy_ID", con);

                cmd.Parameters.AddWithValue("@Due_Date", due.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                con.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter dat = new SqlDataAdapter(cmd);

                dat.Fill(dt);


                if (dt.Rows.Count > 0) { yes_no = 1; }
            }
            catch
            { }
        }

        return yes_no;
    }

    public static int Cal_Month(DateTime due_date, DateTime pay_date)
    {
        //int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30,31 };

        int year_cal = 0, month_cal=0, day_cal=0, increment=0;

        //if (due_date.Day > pay_date.Day)
        //{ 
        //    increment = monthDay [due_date.Month - 1]; 
        //}

        ///// Increment
        //if (increment == -1)
        //{
        //    if (DateTime.IsLeapYear(due_date.Year))
        //    {
        //        increment = 29;
        //    }
        //    else
        //    {
        //        increment = 28;
        //    }
        //}


        //try
        //{
        //    /// Cal Day
        //    if (increment != 0)
        //    {
        //        day_cal = (pay_date.Day + increment) - due_date.Day;
        //        increment = 1;
        //    }
        //    else
        //    {
        //        day_cal = pay_date.Day - due_date.Day;
        //    }

        //    /// Cal Month
        //    if ((due_date.Month + increment) > pay_date.Month)
        //    {
        //        month_cal = (pay_date.Month + 12) - (due_date.Month + increment);
        //        increment = 1;
        //    }
        //    else
        //    {
        //        month_cal = (pay_date.Month) - (due_date.Month + increment);
        //        increment = 0;
        //    }

        //    /// Cal year
        //    year_cal=pay_date.Year - (due_date.Year + increment);
        //}
        //catch
        //{ }

        int M = Math.Abs((pay_date.Year - due_date.Year));


        TimeSpan ts_day = pay_date.Subtract(due_date);
        int totalday = (int)Math.Ceiling(ts_day.TotalDays);

        if (totalday > 30)
        {
            month_cal = ((M * 12) + (pay_date.Month - due_date.Month));
        }
        else
        {
            month_cal = 0;
        }



        return month_cal;
    }

    public static DateTime GetLast_Due_Date_Edit(string policy_id, string receipt_num)
    {
        DateTime last_due = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Due_Date from Ct_Policy_Prem_Pay where 
                                                    Policy_Prem_Pay_ID not in (select Policy_Prem_Pay_ID from Ct_Payment_Receipt where Receipt_Num=@Receipt_Num)
                                                    and Policy_ID=@Policy_ID
                                                    order by Due_Date desc ", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
                cmd.Parameters.AddWithValue("@Receipt_Num", receipt_num);

                con.Open();

                last_due = DateTime.Parse(cmd.ExecuteScalar().ToString());
            }
            catch
            { }
        }

        return last_due;
    }

    public static DataTable GetLast_Due(string policy_id, int pay_mode, DateTime pay_date, decimal prem_paid, string receipt_num, DateTime due_date_original)
    {
        DataTable dt = new DataTable();
        DataTable get_dt = new DataTable();

        DateTime last_due = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString();

        DataTable dt1 = new DataTable();
        dt1.Columns.Add("due_date");
        dt1.Columns.Add("interest_amount");
        dt1.Columns.Add("prem_amount");
        dt1.Columns.Add("duration");
        dt1.Columns.Add("duration_month");
        dt1.Columns.Add("policy_status");

        DataRow r1;

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Due_Date, Policy_Status_Type_ID from Ct_Policy_Prem_Pay 
                                                    inner join Ct_Policy_Status on Ct_Policy_Prem_Pay.Policy_ID=Ct_Policy_Status.Policy_ID

                                                    where Ct_Policy_Prem_Pay.Policy_ID=@Policy_ID order by Due_Date desc", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);

                con.Open();

                dap.Fill(get_dt);

                if (receipt_num == "")
                {
                    last_due = DateTime.Parse(get_dt.Rows[0][0].ToString());
                }
                else
                {
                    last_due = GetLast_Due_Date_Edit(policy_id, receipt_num);
                }

                string policy_status = get_dt.Rows[0][1].ToString();

                TimeSpan start_time = new TimeSpan(00, 00, 00);
                TimeSpan end_time = new TimeSpan(00, 00, 00);

                /// Check Original Due Day and Last Due Day
                if (due_date_original.Day > last_due.Day)
                {
                    last_due = last_due.AddDays(due_date_original.Day - last_due.Day);
                }

                /// Day LAP
                DateTime day_LAP = GetNext_Due(pay_mode, last_due);

                pay_date = pay_date.Date + start_time;
                last_due = last_due.Date + start_time;
                day_LAP = day_LAP.Date + start_time;

                // Calculate Day
                TimeSpan ts = pay_date.Subtract(day_LAP);

                int totalday = (int)Math.Ceiling(ts.TotalDays);

                int i = 0;

                /// Calculate Month
                int M = Math.Abs((pay_date.Year - day_LAP.Year));

                int months = ((M * 12) + (pay_date.Month - day_LAP.Month));

                if (totalday > 30)
                {
                    i = 0;

                    ///// Add table
                    //r1 = dt1.NewRow();
                    //r1["due_date"] = last_due;

                    //r1["duration_month"] = Cal_Month(last_due, pay_date);

                    ////if (policy_status == "LAP")
                    ////{
                    ////    r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));
                    ////}
                    ////else { r1["interest_amount"] = "0"; }

                    //r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                    //r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();
                    //r1["duration"] = totalday;
                    //r1["policy_status"] = policy_status;
                    //dt1.Rows.Add(r1);
                    ///

                    if (pay_mode == 1)
                    {
                        while (i <= months)
                        {
                            r1 = dt1.NewRow();

                            //last_due = last_due.AddYears(1);

                            last_due = GetNext_Due(pay_mode, last_due);


                            TimeSpan ts_annaull = pay_date.Subtract(last_due);
                            int totalday_annull = (int)Math.Ceiling(ts_annaull.TotalDays);

                            if (totalday_annull >= 0)
                            {
                                r1["due_date"] = last_due;

                                r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                                r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();

                                r1["duration"] = totalday_annull;

                                r1["duration_month"] = Cal_Month(last_due, pay_date);

                                r1["policy_status"] = policy_status;

                                dt1.Rows.Add(r1);
                            }

                            i = i + 12;
                        }
                    }
                    else if (pay_mode == 2)
                    {
                        while (i <= months)
                        {
                            r1 = dt1.NewRow();

                            //last_due = last_due.AddMonths(6);

                            last_due = GetNext_Due(pay_mode, last_due);

                            r1["duration_month"] = Cal_Month(last_due, pay_date);

                            TimeSpan ts_sime = pay_date.Subtract(last_due);
                            int totalday_sime = (int)Math.Ceiling(ts_sime.TotalDays);

                            r1["due_date"] = last_due;

                            //if (policy_status == "LAP")
                            //{
                            //    r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));
                            //}
                            //else { r1["interest_amount"] = "0"; }

                            r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                            r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();
                            r1["policy_status"] = policy_status;

                            if (totalday_sime >= 0)
                            {
                                r1["duration"] = totalday_sime;

                                dt1.Rows.Add(r1);
                            }

                            i = i + 6;
                        }
                    }
                    else if (pay_mode == 3)
                    {
                        while (i <= months)
                        {
                            r1 = dt1.NewRow();

                            //last_due = last_due.AddMonths(3);

                            last_due = GetNext_Due(pay_mode, last_due);

                            r1["duration_month"] = Cal_Month(last_due, pay_date);

                            TimeSpan ts_quarter = pay_date.Subtract(last_due);
                            int totalday_quarter = (int)Math.Ceiling(ts_quarter.TotalDays);

                            if (totalday_quarter >= 0)
                            {
                                r1["due_date"] = last_due;

                                //if (policy_status == "LAP")
                                //{
                                //    r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));
                                //}
                                //else { r1["interest_amount"] = "0"; }

                                r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                                r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();

                                r1["duration"] = totalday_quarter;
                                r1["policy_status"] = policy_status;

                                dt1.Rows.Add(r1);
                            }

                            i = i + 3;
                        }
                    }
                    else if (pay_mode == 4)
                    {
                        while (i <= months)
                        {
                            r1 = dt1.NewRow();

                            //last_due = last_due.AddMonths(1);

                            last_due = GetNext_Due(pay_mode, last_due);

                            r1["duration_month"] = Cal_Month(last_due, pay_date);

                            TimeSpan ts_month = pay_date.Subtract(last_due);
                            int totalday_month = (int)Math.Ceiling(ts_month.TotalDays);

                            if (totalday_month >= 0)
                            {
                                r1["due_date"] = last_due;

                                //if (policy_status == "LAP")
                                //{
                                //    r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));
                                //}
                                //else { r1["interest_amount"] = "0"; }

                                r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                                r1["prem_amount"] = prem_paid; //(decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();

                                r1["duration"] = totalday_month;
                                r1["policy_status"] = policy_status;

                                dt1.Rows.Add(r1);
                            }

                            i = i + 1;
                        }
                    }
                }
                else
                {
                    r1 = dt1.NewRow();
                    r1["due_date"] = last_due;
                    r1["interest_amount"] = 0;
                    r1["prem_amount"] = prem_paid;
                    r1["duration"] = 0;
                    r1["duration_month"] = 0;
                    r1["policy_status"] = policy_status;

                    dt1.Rows.Add(r1);
                }

                //if (dt1.Rows.Count > 1)
                //{
                //    double total_prem = 0;

                //    foreach (DataRow item in dt1.Rows)
                //    {
                //        total_prem += double.Parse(item["prem_amount"].ToString());
                //    }
                //    total_prem = Math.Round(total_prem);

                //    r1 = dt1.NewRow();
                //    r1["due_date"] = "";
                //    r1["interest_amount"] = "";
                //    r1["prem_amount"] = total_prem;
                //    r1["duration"] = "";
                //    dt1.Rows.Add(r1);

                //}
            }
            catch
            { }
        }

        return dt1;
    }
    public static DataTable GetLast_Due(string policy_id, int pay_mode, DateTime pay_date, decimal prem_paid, string receipt_num)
    {
        DataTable dt = new DataTable();
        DataTable get_dt = new DataTable();

        DateTime last_due = DateTime.Now;
        DataTable dt1 = new DataTable();
        dt1.Columns.Add("due_date");
        dt1.Columns.Add("interest_amount");
        dt1.Columns.Add("prem_amount");
        dt1.Columns.Add("duration");
        dt1.Columns.Add("duration_month");
        dt1.Columns.Add("policy_status");

        DataRow r1;

        try
        {
            string sql = @"select top(1) Due_Date, Policy_Status_Type_ID from Ct_Policy_Prem_Pay 
                                                    inner join Ct_Policy_Status on Ct_Policy_Prem_Pay.Policy_ID=Ct_Policy_Status.Policy_ID
                                                    where Ct_Policy_Prem_Pay.Policy_ID=@Policy_ID order by Due_Date desc";

           
            get_dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), sql, new string[,] { { "@Policy_ID", policy_id } });

            if (receipt_num == "")
            {
                last_due = DateTime.Parse(get_dt.Rows[0][0].ToString());
            }
            else
            {
                last_due = GetLast_Due_Date_Edit(policy_id, receipt_num);
            }

            string policy_status = get_dt.Rows[0][1].ToString();

            TimeSpan start_time = new TimeSpan(00, 00, 00);
            TimeSpan end_time = new TimeSpan(00, 00, 00);

            /// Day LAP
            DateTime day_LAP = GetNext_Due(pay_mode, last_due, policy_id);
            last_due = day_LAP;

            /// Get last Pay Mode from Prem_Pay
            int last_pay_mode = GetLastPayModeFromPremPay(policy_id);
            pay_date = pay_date.Date + start_time;
            last_due = last_due.Date + start_time;

            // Calculate Day
            TimeSpan ts = pay_date.Subtract(last_due);

            int totalday = (int)Math.Ceiling(ts.TotalDays);

            int i = 0;

            /// Calculate Month
            int M = Math.Abs((pay_date.Year - last_due.Year));

            int months = ((M * 12) + (pay_date.Month - last_due.Month));

            if (totalday > 30)
            {
                i = 0;

                /// Add table
                r1 = dt1.NewRow();
                r1["due_date"] = last_due;

                r1["duration_month"] = Calculate_Month(last_due, pay_date);

                r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                if (r1["interest_amount"].ToString() == "")
                {
                    r1["interest_amount"] = "0";
                }

                r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();
                r1["duration"] = totalday;
                r1["policy_status"] = policy_status;
                dt1.Rows.Add(r1);
                ///

                if (pay_mode == 1)
                {
                    while (i <= months)
                    {
                        r1 = dt1.NewRow();

                        last_due = last_due.AddYears(1);

                        r1["duration_month"] = Calculate_Month(last_due, pay_date);

                        TimeSpan ts_annaull = pay_date.Subtract(last_due);
                        int totalday_annull = (int)Math.Ceiling(ts_annaull.TotalDays);

                        if (totalday_annull >= 0)
                        {
                            r1["due_date"] = last_due;

                            if (receipt_num == "")
                            {
                                if (policy_status == "LAP")
                                {
                                    r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                                    if (r1["interest_amount"] == "")
                                    {
                                        r1["interest_amount"] = "0";
                                    }
                                }
                                else { r1["interest_amount"] = "0"; }
                            }
                            else
                            {
                                r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                                if (r1["interest_amount"] == "")
                                {
                                    r1["interest_amount"] = "0";
                                }
                            }

                            r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();

                            r1["duration"] = totalday_annull;
                            r1["policy_status"] = policy_status;

                            dt1.Rows.Add(r1);
                        }

                        i = i + 12;
                    }
                }
                else if (pay_mode == 2)
                {
                    while (i <= months)
                    {
                        r1 = dt1.NewRow();

                        last_due = last_due.AddMonths(6);

                        last_due = CheckDayNextDueWithEffectiveDay(pay_mode, last_due, policy_id);

                        r1["duration_month"] = Calculate_Month(last_due, pay_date);

                        TimeSpan ts_sime = pay_date.Subtract(last_due);
                        int totalday_sime = (int)Math.Ceiling(ts_sime.TotalDays);

                        r1["due_date"] = last_due;

                        if (policy_status == "LAP")
                        {
                            r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                            if (r1["interest_amount"] == "")
                            {
                                r1["interest_amount"] = "0";
                            }
                        }
                        else { r1["interest_amount"] = "0"; }

                        r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();
                        r1["policy_status"] = policy_status;

                        if (totalday_sime >= 0)
                        {
                            r1["duration"] = totalday_sime;

                            dt1.Rows.Add(r1);
                        }

                        i = i + 6;
                    }
                }
                else if (pay_mode == 3)
                {
                    while (i <= months)
                    {
                        r1 = dt1.NewRow();

                        last_due = last_due.AddMonths(3);

                        last_due = CheckDayNextDueWithEffectiveDay(pay_mode, last_due, policy_id);

                        r1["duration_month"] = Calculate_Month(last_due, pay_date);

                        TimeSpan ts_quarter = pay_date.Subtract(last_due);
                        int totalday_quarter = (int)Math.Ceiling(ts_quarter.TotalDays);

                        if (totalday_quarter >= 0)
                        {
                            r1["due_date"] = last_due;

                            if (policy_status == "LAP")
                            {
                                r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                                if (r1["interest_amount"] == "")
                                {
                                    r1["interest_amount"] = "0";
                                }
                            }
                            else { r1["interest_amount"] = "0"; }

                            r1["prem_amount"] = prem_paid; // (decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();

                            r1["duration"] = totalday_quarter;
                            r1["policy_status"] = policy_status;

                            dt1.Rows.Add(r1);
                        }

                        i = i + 3;
                    }
                }
                else if (pay_mode == 4)
                {
                    while (i <= months)
                    {
                        r1 = dt1.NewRow();

                        last_due = last_due.AddMonths(1);

                        last_due = CheckDayNextDueWithEffectiveDay(pay_mode, last_due, policy_id);

                        r1["duration_month"] = Calculate_Month(last_due, pay_date);

                        TimeSpan ts_month = pay_date.Subtract(last_due);
                        int totalday_month = (int)Math.Ceiling(ts_month.TotalDays);

                        if (totalday_month >= 0)
                        {
                            r1["due_date"] = last_due;

                            if (policy_status == "LAP")
                            {
                                r1["interest_amount"] = string.Format("{0:#,#.####}", Calculate_Lapsed_Prem(policy_id, pay_date, last_due, prem_paid));

                                if (r1["interest_amount"] == "")
                                {
                                    r1["interest_amount"] = "0";
                                }
                            }
                            else { r1["interest_amount"] = "0"; }

                            r1["prem_amount"] = prem_paid; //(decimal.Parse(r1["interest_amount"].ToString()) + prem_paid).ToString();

                            r1["duration"] = totalday_month;
                            r1["policy_status"] = policy_status;

                            dt1.Rows.Add(r1);
                        }

                        i = i + 1;
                    }
                }
            }
            else
            {
                r1 = dt1.NewRow();
                r1["due_date"] = last_due;
                r1["interest_amount"] = 0;
                r1["prem_amount"] = prem_paid;
                r1["duration"] = 0;
                r1["duration_month"] = 0;
                r1["policy_status"] = policy_status;

                dt1.Rows.Add(r1);
            }

          
        }
        catch (Exception ex)
        {
            //write error to log
            Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [GetLast_Due]. Details: " + ex.Message);
        }
        return dt1;
    }
    public static int Calculate_Month(DateTime due_date, DateTime pay_date)
    {
        //int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30,31 };

        int month_cal = 0;

        try
        {
            int M = Math.Abs((pay_date.Year - due_date.Year));

            month_cal = ((M * 12) + (pay_date.Month - due_date.Month));

        }
        catch (Exception ex)
        {
            //write error to log
            Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [Calculate_Month]. Details: " + ex.Message);
        }

        return month_cal;
    }

    public static DataTable Get_Interest_Rate(string policy_id, DateTime pay_date)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from Ct_Interest_Amount where policy_id=@policy_id and date_rate in (@date_rate) order by date_rate,duration_day desc ", con);

                cmd.Parameters.AddWithValue("@policy_id", policy_id);
                cmd.Parameters.AddWithValue("@date_rate", pay_date);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);

                con.Open();

                dap.Fill(dt);
            }
            catch
            { }
        }

        return dt;
    }

    public static DateTime GetLast_Due_Date(string policy_id)
    {
        DateTime last_due = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Due_Date from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID order by Due_Date desc ", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                con.Open();

                last_due = DateTime.Parse(cmd.ExecuteScalar().ToString());
            }
            catch
            { }
        }

        return last_due;
    }

    public static int GetPrem_Year(string policy_id)
    {
        int total_month = Check_Remaining_Month(policy_id,0,1);

        int pre_year = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Prem_Year from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID
                                                  order by Prem_Year desc", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                con.Open();

                pre_year = int.Parse(cmd.ExecuteScalar().ToString());


                if (total_month == 0)
                { pre_year = pre_year + 1; }
            }
            catch
            {
                pre_year = 0;
            }
        }

        return pre_year;
    }
    /// <summary>
    /// generate new policy prem lot
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="pay_mode_id"></param>
    /// <param name="prem_year"></param>
    /// <returns></returns>
    public static int GetPrem_Lot(string policy_id, int pay_mode_id,int prem_year)
    {
        int total_month = Check_Remaining_Month(policy_id,0,1);

        int pre_lot = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Prem_Lot from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID and
                                                  Pay_Mode_ID=@Pay_Mode_ID and Prem_Year=@Prem_Year order by Due_Date desc", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
                cmd.Parameters.AddWithValue("@Pay_Mode_ID", pay_mode_id);
                cmd.Parameters.AddWithValue("@Prem_Year", prem_year);

                con.Open();

                pre_lot = int.Parse(cmd.ExecuteScalar().ToString());

                if (total_month == 12)
                { pre_lot = 1; }
                else { pre_lot = pre_lot + 1; }
            }
            catch(Exception ex)
            {
                pre_lot =1;
            }
        }

        return pre_lot;
    }
    /// <summary>
    /// Get last policy prem lot
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="pay_mode_id"></param>
    /// <param name="prem_year"></param>
    /// <returns></returns>
    public static int GetLast_Policy_Prem_Lot(string policy_id, int pay_mode_id, int prem_year)
    {
      
        int pre_lot = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Prem_Lot from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID and
                                                  Pay_Mode_ID=@Pay_Mode_ID and Prem_Year=@Prem_Year order by Prem_Lot desc", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
                cmd.Parameters.AddWithValue("@Pay_Mode_ID", pay_mode_id);
                cmd.Parameters.AddWithValue("@Prem_Year", prem_year);

                con.Open();

                pre_lot = int.Parse(cmd.ExecuteScalar().ToString());

            }
            catch (Exception ex)
            {
                pre_lot = 1;
            }
        }

        return pre_lot;
    }
    public static decimal Calculate_Lapsed_Prem(string Policy_ID, DateTime Pay_Date, DateTime Due_Date, decimal Pre_Paid)
    {
        decimal lapsed_prem_paid = 0;

        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(00, 00, 00);

        Pay_Date = Pay_Date.Date + start_time;
        Due_Date = Due_Date.Date + end_time;

        // Calculate Day
        TimeSpan ts = Pay_Date.Subtract(Due_Date); //Pay_Date.Date + start_time - Due_Date.Date + end_time;
        int totalday = (int)Math.Ceiling(ts.TotalDays);

        //// Calculate Month
        //int M = Math.Abs((Pay_Date.Year - Due_Date.Year));

        //int months = ((M * 12) + Math.Abs((Pay_Date.Month - Due_Date.Month)));

        //if (totalday <= 30)
        //{
        //    // add nothing to Prem
        //    lapsed_prem_paid = Pre_Paid;
        //}
        //else
        //{ 
        //   // Add interest lapsed of Prem
        //    lapsed_prem_paid = Lapsed_Payment(Pre_Paid, totalday);
        //}

        lapsed_prem_paid = Lapsed_Payment(Pre_Paid, totalday);

        return lapsed_prem_paid;
    }

    public static decimal Lapsed_Payment(decimal prem, int total_day)
    {
        decimal lapsed_amount = 0;

        lapsed_amount =((prem * (decimal.Parse("0.1")))/365) * total_day; // 0.1 is rate of lapsed per year

        //lapsed_amount = prem + lapsed_amount;

        //lapsed_amount = Math.Round(lapsed_amount); /// Round up >=0.5, Round down <0.5

        
        return lapsed_amount;
    }

    public static decimal Total_Interest_Rate(string array_string_rate)
    {
        decimal lapsed_amount = 0;

        string[] interest_value = array_string_rate.Split(',');

        if (interest_value.Count() > 0)
        {
            lapsed_amount = decimal.Parse(interest_value[0]);

            lapsed_amount = Math.Round(lapsed_amount);
        }

        return lapsed_amount;
    }

    public static int Calculate_Period(DateTime pay_date, DateTime due_date, int pay_mode)
    {
        int period = 0;

        if (pay_mode == 1)
        {
            while (pay_date >=due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddYears(-1);
            }
        }
        else if (pay_mode == 2)
        {
            while (pay_date >= due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddMonths(-6);
            }
        }
        else if (pay_mode == 3)
        {
            while (pay_date >= due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddMonths(-3);
            }
        }
        else if (pay_mode ==4)
        {
            while (pay_date >= due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddMonths(-1);
            }
        }
        return period;
    }

    public static DateTime Next_Dub_by_PayDate(DateTime pay_date, DateTime due_date, int pay_mode)
     {
        int period = 0;
        DateTime next_due = DateTime.Now;

        if (pay_mode == 1)
        {
            while (pay_date >= due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddYears(-1);
            }
            if (period == 0) { period = 1; }
            next_due = due_date.AddYears(period);
        }
        else if (pay_mode == 2)
        {
            while (pay_date >= due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddMonths(-6);
            }

            if (period == 0) { period = 1; }
            next_due = due_date.AddMonths(period * 6);
        }
        else if (pay_mode == 3)
        {
            while (pay_date >= due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddMonths(-3);
            }
            if (period == 0) { period = 1; }
            next_due = due_date.AddMonths(period * 3);
        }
        else if (pay_mode == 4)
        {
            while (pay_date >= due_date)
            {
                period = period + 1;
                pay_date = pay_date.AddMonths(-1);
            }
            if (period == 0) { period = 1; }
            next_due = due_date.AddMonths(period);
        }
        return next_due;
    }

    /// <summary>
    /// Update policy status
    /// </summary>
    public static bool Update_Policy_Status(string policy_id, string status, string note_text)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("update Ct_Policy_Status set Policy_Status_Type_ID =@Policy_Status_Type_ID, Created_Note=@Created_Note where Policy_ID=@Policy_ID", con);

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", status);
            cmd.Parameters.AddWithValue("@Created_Note", note_text);

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

    /// <summary>
    /// Policy List
    /// </summary>
    public static DataTable GetPolicy_List(DateTime from_date, DateTime to_date, string last_name, string first_name, string policy_num, int check_edit)
    {
        string sql = "", condition_search="";

        DataTable dt = new DataTable(); DataTable dt1 = new DataTable(); 

        string From_date = DateTime.Parse(from_date.ToString()).ToString("dd/MM/yyyy");
        string To_date = DateTime.Parse(to_date.ToString()).ToString("dd/MM/yyyy");
   
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                if (from_date.ToString("dd/MM/yyyy") == ("01/01/1900") && last_name == "" && first_name == "" && policy_num == "")
                {
                    sql = @" SELECT top(100)  dbo.Ct_Policy_Number.Policy_Number, dbo.Ct_Customer.Last_Name, dbo.Ct_Customer.First_Name, 
                CASE WHEN Ct_Customer.Gender = 0 THEN 'F' ELSE 'M' END AS Gender, dbo.Ct_Payment_Mode.Mode, dbo.Ct_Policy_Premium.Sum_Insure, 
				dbo.Ct_Policy.Policy_ID, CASE WHEN Ct_Product.En_Abbr IS NULL 
                THEN '' ELSE Ct_Product.En_Abbr END AS En_Abbr, dbo.Ct_Policy_Status.Policy_Status_Type_ID, Ct_Payment_Mode.Pay_Mode_ID
                FROM    dbo.Ct_Policy INNER JOIN
                        dbo.Ct_Policy_Pay_Mode ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Pay_Mode.Policy_ID INNER JOIN
                        dbo.Ct_Payment_Mode ON dbo.Ct_Policy_Pay_Mode.Pay_Mode = dbo.Ct_Payment_Mode.Pay_Mode_ID INNER JOIN
                        dbo.Ct_Policy_Number ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Number.Policy_ID INNER JOIN
                        dbo.Ct_Policy_Premium ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Premium.Policy_ID left JOIN
                        dbo.Ct_Policy_Status ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Status.Policy_ID left JOIN
                        dbo.Ct_Policy_Status_Type ON dbo.Ct_Policy_Status.Policy_Status_Type_ID = dbo.Ct_Policy_Status_Type.Policy_Status_Type_ID left JOIN
                        dbo.Ct_Customer ON dbo.Ct_Policy.Customer_ID = dbo.Ct_Customer.Customer_ID left JOIN
                        dbo.Ct_Product ON dbo.Ct_Policy.Product_ID = dbo.Ct_Product.Product_ID 
                    
                 ";
                }
                else
                {

                    sql = @" SELECT  dbo.Ct_Policy_Number.Policy_Number, dbo.Ct_Customer.Last_Name, dbo.Ct_Customer.First_Name, 
                CASE WHEN Ct_Customer.Gender = 0 THEN 'F' ELSE 'M' END AS Gender, dbo.Ct_Payment_Mode.Mode, dbo.Ct_Policy_Premium.Sum_Insure, 
				dbo.Ct_Policy.Policy_ID, CASE WHEN Ct_Product.En_Abbr IS NULL 
                THEN '' ELSE Ct_Product.En_Abbr END AS En_Abbr, dbo.Ct_Policy_Status.Policy_Status_Type_ID, Ct_Payment_Mode.Pay_Mode_ID
                FROM    dbo.Ct_Policy INNER JOIN
                        dbo.Ct_Policy_Pay_Mode ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Pay_Mode.Policy_ID INNER JOIN
                        dbo.Ct_Payment_Mode ON dbo.Ct_Policy_Pay_Mode.Pay_Mode = dbo.Ct_Payment_Mode.Pay_Mode_ID INNER JOIN
                        dbo.Ct_Policy_Number ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Number.Policy_ID INNER JOIN
                        dbo.Ct_Policy_Premium ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Premium.Policy_ID left JOIN
                        dbo.Ct_Policy_Status ON dbo.Ct_Policy.Policy_ID = dbo.Ct_Policy_Status.Policy_ID left JOIN
                        dbo.Ct_Policy_Status_Type ON dbo.Ct_Policy_Status.Policy_Status_Type_ID = dbo.Ct_Policy_Status_Type.Policy_Status_Type_ID left JOIN
                        dbo.Ct_Customer ON dbo.Ct_Policy.Customer_ID = dbo.Ct_Customer.Customer_ID left JOIN
                        dbo.Ct_Product ON dbo.Ct_Policy.Product_ID = dbo.Ct_Product.Product_ID 
                    
                 ";
                }
                SqlCommand cmd = new SqlCommand();

                if (policy_num != "")
                {
                    condition_search += " and Policy_Number=@Policy_Number";
                    cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
                }

                if (first_name != "")
                {
                    condition_search += " and First_Name like '%" + first_name + "%'";
                }

                if (last_name != "")
                {
                    condition_search += " and Last_Name like '%" + last_name + "%'";
                }

                if (condition_search != "")
                {
                    condition_search =StringExtensions.SubstringAfter(condition_search, " and");

                    sql = sql + " where" + condition_search + " order by Policy_Number asc";
                }

                cmd.CommandText = sql;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);

                cmd.Connection = con;

                con.Open();

                dap.Fill(dt1);

                DataRow dr;

                dt.Columns.Add("No");
                dt.Columns.Add("Policy_Number");
                dt.Columns.Add("Last_Name");
                dt.Columns.Add("First_Name");
                dt.Columns.Add("Gender");
                dt.Columns.Add("Mode");
                dt.Columns.Add("Sum_Insure");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Due_Date");
                dt.Columns.Add("Next_Due");
                dt.Columns.Add("Policy_ID");
                dt.Columns.Add("En_Abbr");
                dt.Columns.Add("Policy_Status_Type_ID");
                
                DateTime next_due = DateTime.Now;

                int i = 0;

                foreach (DataRow item in dt1.Rows)
                {
                    dr = dt.NewRow();

                    DataTable dt2 = Get_Prem(item["Policy_ID"].ToString());

                    if (dt2.Rows.Count > 0)
                    {
                        next_due = GetNext_Due(int.Parse(item["Pay_Mode_ID"].ToString()), DateTime.Parse(dt2.Rows[0][0].ToString()));

                        dr["Policy_Number"] = item["Policy_Number"].ToString();
                        dr["Last_Name"] = item["Last_Name"].ToString();
                        dr["First_Name"] = item["First_Name"].ToString();
                        dr["Gender"] = item["Gender"].ToString();
                        dr["Mode"] = item["Mode"].ToString();
                        dr["Sum_Insure"] = item["Sum_Insure"].ToString();
                        dr["Amount"] = dt2.Rows[0][1].ToString();
                        //dr["Due_Date"] =DateTime.Parse(dt2.Rows[0][0].ToString()).ToString("dd-MM-yyyy");
                        //dr["Next_Due"] =DateTime.Parse(next_due.ToString()).ToString("dd-MM-yyyy");
                        dr["Policy_ID"] = item["Policy_ID"].ToString();
                        dr["En_Abbr"] = item["En_Abbr"].ToString();
                        dr["Policy_Status_Type_ID"] = item["Policy_Status_Type_ID"].ToString();

                        if (check_edit == 0)
                        {
                            if (from_date != DateTime.Parse("01/01/1900"))
                            {
                                if ((DateTime.Parse(dt2.Rows[0][0].ToString()) >= from_date) && (DateTime.Parse(dt2.Rows[0][0].ToString()) <= to_date))
                                {
                                    dr["Due_Date"] = DateTime.Parse(dt2.Rows[0][0].ToString()).ToString("dd-MM-yyyy");
                                    dr["Next_Due"] = DateTime.Parse(next_due.ToString()).ToString("dd-MM-yyyy");

                                    i = i + 1;

                                    dr["No"] = i;

                                    dt.Rows.Add(dr);
                                }
                            }
                            else
                            {
                                dr["Due_Date"] = DateTime.Parse(dt2.Rows[0][0].ToString()).ToString("dd-MM-yyyy");
                                dr["Next_Due"] = DateTime.Parse(next_due.ToString()).ToString("dd-MM-yyyy");

                                i = i + 1;

                                dr["No"] = i;

                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dr["Due_Date"] = DateTime.Parse(dt2.Rows[0][0].ToString()).ToString("dd-MM-yyyy");
                            dr["Next_Due"] = DateTime.Parse(next_due.ToString()).ToString("dd-MM-yyyy");

                            i = i + 1;

                            dr["No"] = i;

                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch
            {
                
            }
        }

        DataView view = dt.DefaultView;
        view.Sort = "Policy_Number ASC";
        DataTable dt11 = view.ToTable();

        return dt11;
    }

    public static DataTable Get_Prem(string policy_id)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Due_Date,Amount from Ct_Policy_Prem_Pay 
                                                  where Policy_ID=@Policy_ID order by Due_Date desc
                                                        ", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                con.Open();

                dap.Fill(dt);
            }
            catch
            {
                
            }
        }

        return dt;
    }

    public static List<bl_check_policy_lapsed> Check_Lapsed(string policy_num, string customer_id, string last_name, string first_name, DateTime from_date)
    {
        List<bl_check_policy_lapsed> policy = new List<bl_check_policy_lapsed>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            DataTable dt = new DataTable();

            string sql = @"select Ct_Policy_Number.Policy_Number,Ct_Customer.Last_Name,Ct_Customer.First_Name,
					case when Ct_Customer.Gender =0 then 'F'
					else 'M' end as 'Gender',Ct_Policy_Prem_Pay.Prem_Year,Ct_Policy_Prem_Pay.Prem_Lot,Ct_Payment_Mode.Mode,
					Ct_Policy_Premium.Sum_Insure,Ct_Policy_Prem_Pay.Amount,Due_Date,Ct_Policy_Prem_Pay.Sale_Agent_ID,
					Ct_Policy_Prem_Pay.Office_ID,Ct_Policy.Policy_ID,
					case when Ct_Policy_Premium.Original_Amount IS NULL then 0
					else Ct_Policy_Premium.Original_Amount end as 'Original_Prem',Ct_Payment_Mode.Pay_Mode_ID,
				    case when Ct_Product.En_Abbr is null then '' 
					else Ct_Product.En_Abbr end as 'En_Abbr',Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID,
					case when Ct_Payment_Receipt.Receipt_Num is null then ''
					else Ct_Payment_Receipt.Receipt_Num end as 'Receipt_Num',Ct_Policy_Status.Policy_Status_Type_ID,Ct_Product.Product_Type_ID from Ct_Policy 
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

                    where Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID in (select max(Ct_Policy_Prem_Pay.Policy_Prem_Pay_ID) from Ct_Policy_Prem_Pay 
					inner join Ct_Policy_Number on Ct_Policy_Prem_Pay.Policy_ID=Ct_Policy_Number.Policy_ID group by Policy_Number ) 
					and Ct_Policy_Status.Policy_Status_Type_ID not in ('MAT')";

            SqlCommand cmd = new SqlCommand();

            if (policy_num != "")
            {
                cmd.Parameters.AddWithValue("@Policy_Number", policy_num);
                sql = sql + " and Policy_Number=@Policy_Number";
            }

            if (first_name != "")
            {
                sql = sql + " and First_Name like '%" + first_name + "%'";
            }

            if (last_name != "")
            {
                sql = sql + " and Last_Name like '%" + last_name + "%'";
            }

            if (customer_id != "")
            {
                cmd.Parameters.AddWithValue("@Customer_ID", customer_id);
                sql = sql + " and Customer_ID =@Customer_ID";
            }

            cmd.CommandText = sql;

            cmd.Connection = con;

            con.Open();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            dap.Fill(dt);
            con.Close();

            DataTable dt_final = new DataTable();

            if (dt.Rows.Count > 0)
            {

                dt_final.Columns.Add("Policy_ID");
                dt_final.Columns.Add("Mode_ID");
                dt_final.Columns.Add("Customer");
                dt_final.Columns.Add("Gender");
                dt_final.Columns.Add("YearTimes");
                dt_final.Columns.Add("Mode");
                dt_final.Columns.Add("SumIn");
                dt_final.Columns.Add("Amount");
                dt_final.Columns.Add("Due_Date");
                dt_final.Columns.Add("Interest");
                dt_final.Columns.Add("duration_day");
                dt_final.Columns.Add("month");
                dt_final.Columns.Add("Product");
                dt_final.Columns.Add("Pay_Date");
                dt_final.Columns.Add("Polno");

                DataRow r;

                foreach (DataRow dr in dt.Rows)
                {
                    DataTable dt1 = da_policy_prem_pay.GetLast_Due(dr["Policy_ID"].ToString(), int.Parse(dr["Pay_Mode_ID"].ToString()), from_date, decimal.Parse(dr["Amount"].ToString()), "", DateTime.Parse(dr["Due_Date"].ToString()));

                    foreach (DataRow item in dt1.Rows)
                    {
                        r = dt_final.NewRow();

                        r["Policy_ID"] = dr["Policy_ID"].ToString();
                        r["Mode_ID"] = dr["Pay_Mode_ID"].ToString();
                        r["Customer"] = dr["Last_Name"].ToString() + " " + dr["First_Name"].ToString();
                        r["Gender"] = dr["Gender"].ToString();
                        r["YearTimes"] = dr["Prem_Year"].ToString() + "/" + dr["Prem_Lot"].ToString();
                        r["Mode"] = dr["Mode"].ToString();
                        r["SumIn"] = dr["Sum_Insure"].ToString();
                        r["Amount"] = dr["Amount"].ToString();
                        r["Due_Date"] = item["due_date"].ToString();
                        r["Interest"] = item["interest_amount"].ToString();
                        r["duration_day"] = item["duration"].ToString();
                        r["month"] = item["duration_month"].ToString();
                        r["Product"] = dr["En_Abbr"].ToString(); ;
                        r["Pay_Date"] = item["due_date"].ToString();
                        r["Polno"] = dr["Policy_Number"].ToString();

                        dt_final.Rows.Add(r);
                    }
                }
            }

            foreach (DataRow item in dt_final.Rows)
            {
                if (dt_final.Rows.Count > 0)
                {
                    bl_check_policy_lapsed policy_payment = new bl_check_policy_lapsed();

                    policy_payment.policy_num = item["polno"].ToString();
                    policy_payment.customer = item["Customer"].ToString();
                    policy_payment.gender = item["Gender"].ToString();
                    policy_payment.year_time = int.Parse(item["YearTimes"].ToString());
                    policy_payment.mode = item["Mode"].ToString();
                    policy_payment.sum_insure = decimal.Parse(item["SumIn"].ToString());
                    policy_payment.premium = decimal.Parse(item["Amount"].ToString());
                    policy_payment.due_date = DateTime.Parse(item["Due_Date"].ToString());
                    policy_payment.interest = decimal.Parse(item["Interest"].ToString());
                    policy_payment.prem_interest = policy_payment.premium + policy_payment.interest;
                    policy_payment.dur_days = int.Parse(item["duration_day"].ToString());
                    policy_payment.dur_months = int.Parse(item["month"].ToString());
                    policy_payment.next_due = DateTime.Parse(item["Policy_Status_Type_ID"].ToString());
                    policy_payment.product = item["Product"].ToString();

                    policy.Add(policy_payment);
                }
            }
            con.Close();
        }
        return policy;
    }
    public static int GetLastPayModeFromPremPay(string policy_id)
    {
        int pay_mode = -1;

        try
        {
           string sql = @"select Pay_Mode_ID from Ct_Policy_Prem_Pay where Policy_ID =@Policy_ID order by due_date desc";
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), sql, new string[,] { { "@Policy_ID", policy_id} });
            if (tbl.Rows.Count > 0)
            {
                pay_mode = Convert.ToInt32(tbl.Rows[0]["Pay_Mode_ID"].ToString());
            }
        }
        catch (Exception ex)
        {
            //write error to log
            //Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [GetLastPayModeFromPremPay]. Details: " + ex.Message);
        }

        return pay_mode;
    }

    public static DateTime GetFirst_Due_Date(string policy_id)
    {
        DateTime last_due = DateTime.Now;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select top(1) Due_Date from Ct_Policy_Prem_Pay where Policy_ID=@Policy_ID order by Due_Date asc ", con);

                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

                con.Open();

                last_due = DateTime.Parse(cmd.ExecuteScalar().ToString());
            }
            catch
            {

            }
        }

        return last_due;
    }
    public static DataTable GetPolicyStatusPayMode(string policy_number)
    {

        DataTable dt = new DataTable();

        try
        {


            string sql = @"select top(1) Ct_Policy_Number.Policy_ID,Due_Date,Ct_Policy_Status.Policy_Status_Type_ID,Pay_Mode from Ct_Policy_Status 
                                    inner join Ct_Policy_Pay_Mode on Ct_Policy_Status.Policy_ID=Ct_Policy_Pay_Mode.Policy_ID
                                    inner join Ct_Policy_Number on Ct_Policy_Number.Policy_ID=Ct_Policy_Status.Policy_ID
                                    inner join Ct_Policy_Prem_Pay on Ct_Policy_Number.Policy_ID=Ct_Policy_Prem_Pay.Policy_ID
                                    where Policy_Number =@Policy_Number
                                    order by Due_Date desc
                                ";


            dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), sql, new string[,]{
            {"@Policy_Number",policy_number}
            });


        }
        catch (Exception ex)
        {
            //write error to log
            Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [GetPolicyStatusPayMode]. Details: " + ex.Message);
        }
        return dt;
    }
    /// <summary>
    /// This function return sub policy premium object, but premium, em, original_amount and SI, and discount show as total
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bl_sub_policy_premium GetSubPoliciesTotalPremium(string policy_id)
    {
        bl_sub_policy_premium objSub = new bl_sub_policy_premium();
        try
        {
            List<bl_sub_policy_premium> listSubPrem = GetSubPolicyPremiumList(policy_id);
            if (listSubPrem.Count > 0)
            {
                foreach (bl_sub_policy_premium obj in listSubPrem)
                {
                    objSub.Premium += obj.Premium;
                    objSub.OriginalAmount += obj.OriginalAmount;
                    objSub.EmAmount += obj.EmAmount;
                    objSub.EmPremium += obj.EmPremium;
                    objSub.DiscountAmount += obj.DiscountAmount;
                    objSub.SumInsure += obj.SumInsure;
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in class [da_policy_prem_pay], function [GetSubPoliciesTotalPremium(string policy_id)], Detail: " + ex.Message);
        }
        return objSub;
    }
    /// <summary>
    /// Get all sub policies premium by policy id
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static List<bl_sub_policy_premium> GetSubPolicyPremiumList(string policy_id)
    {
        List<bl_sub_policy_premium> preList = new List<bl_sub_policy_premium>();
        try
        {
            DataTable tbl =  DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_SUB_POLICY_PREMIUM_BY_POLICY_ID", new string[,] { { "@POLICY_ID", policy_id } }, "[da_policy_prem_pay ==> GetSubPolicyPremiumList(string policy_id)]");
            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    //Add sub policies premium in list
                    preList.Add(new bl_sub_policy_premium()
                    {
                        PolicyID = row["policy_id"].ToString(),
                        PolicyNumber = row["policy_number"].ToString(),
                        Premium = Convert.ToDouble(row["premium"].ToString()),
                        OriginalAmount = Convert.ToDouble(row["original_amount"].ToString()),
                        EmPremium = Convert.ToDouble(row["em_premium"].ToString()),
                        EmAmount = Convert.ToDouble(row["em_amount"].ToString()),
                        DiscountAmount = Convert.ToDouble(row["discount_amount"].ToString()),
                        SumInsure = Convert.ToDouble(row["sum_insure"].ToString())

                    });
                }
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error: class [da_policy_prem_pay], function [GetSubPolicyPremiumList(string policy_id)]. Details: " + ex.Message);
        }
        return preList;
    }
    /// <summary>
    /// Get auto receipt number
    /// </summary>
    /// <returns></returns>
    public static string Auto_Receipt_Number()
    {
        string receipt_no = "";

      
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_Auto_Receipt_Number", new string[,] { },"Funcion [Auto_Receipt_Number()] in class [da_policy_prem_pay]");
            if (tbl.Rows.Count > 0)
            {
                receipt_no = tbl.Rows[0][0].ToString();
            }
            else
            {
                receipt_no = "";
            }
        }
        catch(Exception ex)
        {
            receipt_no = "";
            Log.AddExceptionToLog("Error function [Auto_Receipt_Number()] in class [policy_prem_pay], detail:" + ex.Message);
        }

        return receipt_no;
    }
}

public static class StringExtensions
{
    public static string SubstringAfter(this string source, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return source;
        }
        CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        int index = compareInfo.IndexOf(source, value, CompareOptions.Ordinal);
        if (index < 0)
        {
            //No such substring
            return string.Empty;
        }
        return source.Substring(index + value.Length);
    }


}