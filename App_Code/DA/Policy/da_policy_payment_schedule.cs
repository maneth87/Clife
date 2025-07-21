using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_policy_payment_schedule
/// </summary>
public class da_policy_payment_schedule
{
    private static da_policy_payment_schedule mytitle = null;
    public da_policy_payment_schedule()
    {
        if (mytitle == null)
        {
            mytitle = new da_policy_payment_schedule();
        }
    }

    #region "Public Functions"

    /// <summary>
    /// Insert into Ct_Policy_Payment_Schedule
    /// </summary>
    public static bool InsertPolicyPaymentSchedule(bl_policy_payment_schedule policy_payment_schedule)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Payment_Schedule";

            cmd.Parameters.AddWithValue("@Created_By", policy_payment_schedule.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy_payment_schedule.Created_Note);
            cmd.Parameters.AddWithValue("@Created_On", policy_payment_schedule.Created_On);
            cmd.Parameters.AddWithValue("@Discount", policy_payment_schedule.Discount);
            cmd.Parameters.AddWithValue("@Due_Date", policy_payment_schedule.Due_Date);
            cmd.Parameters.AddWithValue("@Extra_Premium", policy_payment_schedule.Extra_Premium);
            cmd.Parameters.AddWithValue("@Pay_Mode", policy_payment_schedule.Pay_Mode);
            cmd.Parameters.AddWithValue("@Policy_ID", policy_payment_schedule.Policy_ID);
            cmd.Parameters.AddWithValue("@Policy_Payment_Schedule_ID", policy_payment_schedule.Policy_Payment_Schedule_ID);
            cmd.Parameters.AddWithValue("@Premium", policy_payment_schedule.Premium);
            cmd.Parameters.AddWithValue("@Premium_After_Discount", policy_payment_schedule.Premium_After_Discount);
            cmd.Parameters.AddWithValue("@Product_ID", policy_payment_schedule.Product_ID);
            cmd.Parameters.AddWithValue("@Sum_Insure", policy_payment_schedule.Sum_Insure);
            cmd.Parameters.AddWithValue("@_Time", policy_payment_schedule.Time);
            cmd.Parameters.AddWithValue("@Total_Premium", policy_payment_schedule.Total_Premium);
            cmd.Parameters.AddWithValue("@_Year", policy_payment_schedule.Year);

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
                Log.AddExceptionToLog("Error in function [InsertPolicyStatus] in class [da_policy_payment_schedule]. Details: " + ex.Message);
            }
        }
        return result;
    }

   
    /// <summary>
    /// Check Policy Number in Ct_Policy_Payment_Schedule
    /// </summary>
    public static bool CheckPolicyNumber(string policy_number)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Check_Policy_Number_In_Policy_Payment_Schedule";
            cmd.Parameters.AddWithValue("@Policy_Number", policy_number);
            
            cmd.Connection = con;
            DataTable dt = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(cmd);

            con.Open();
            try
            {
                dap.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [CheckPolicyNumber] in class [da_policy_payment_schedule]. Details: " + ex.Message);
            }
        }
        return result;
    }


    //Function to get policy
    public static bl_policy_payment_schedule GetPolicyDetails(string policy_Number)
    {
        bl_policy_payment_schedule policy = new bl_policy_payment_schedule();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Detail_For_Payment_Schedule_By_Policy_Number";
                myCommand.Parameters.AddWithValue("@Policy_Number", policy_Number);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        policy.Due_Date = myReader.GetDateTime(myReader.GetOrdinal("Effective_Date"));
                        policy.Discount = myReader.GetDouble(myReader.GetOrdinal("Discount_Amount"));
                        policy.Year = myReader.GetInt32(myReader.GetOrdinal("Pay_Year"));
                        policy.Policy_ID = myReader.GetString(myReader.GetOrdinal("Policy_ID"));
                        policy.Product_ID = myReader.GetString(myReader.GetOrdinal("Product_ID"));
                        policy.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("Sum_Insure"));
                        policy.Premium = System.Math.Ceiling(myReader.GetDouble(myReader.GetOrdinal("Premium")));
                        policy.Extra_Premium =System.Math.Ceiling(myReader.GetDouble(myReader.GetOrdinal("EM_Premium")));
                        policy.Pay_Mode = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode"));

                        //maneth
                        //policy.Original_Amount = myReader.GetDouble(myReader.GetOrdinal("Original_Amount"));
                        //policy.Original_Amount=System.Math.Round(myReader.GetDouble(myReader.GetOrdinal("Original_Amount")),0,MidpointRounding.AwayFromZero);
                        policy.Original_Amount = System.Math.Ceiling(myReader.GetDouble(myReader.GetOrdinal("Original_Amount")));
                        //end maneth
                        
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
            Log.AddExceptionToLog("Error in function [GetPolicyDetails] in class [da_policy_payment_schedule]. Details: " + ex.Message);
        }
        return policy;
    }

    /// <summary>
    /// Delete Policy Payment Schedule By Policy ID
    /// </summary>
    public static bool DeletePolicyPaymentScheduleByPolicyID(string policy_id)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Policy_Payment_Schedule_By_Policy_ID";
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                con.Close();
                
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [DeletePolicyPaymentScheduleByPolicyID] in class [da_policy_payment_schedule]. Details: " + ex.Message);
            }
        }
        return result;
    }

    //Function to get policy payment schedule
    public static List<bl_policy_payment_schedule> GetPolicyPaymentScheduleDetails(string policy_Number, string year)
    {
        
        List<bl_policy_payment_schedule> policy_payment_schedule_list = new List<bl_policy_payment_schedule>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Payment_Schedule_By_Policy_Number";
                myCommand.Parameters.AddWithValue("@Policy_Number", policy_Number);
                myCommand.Parameters.AddWithValue("@_Year", Convert.ToInt32(year));
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        bl_policy_payment_schedule policy_payment_schedule = new bl_policy_payment_schedule();

                        //policy_payment_schedule.Due_Date = myReader.GetDateTime(myReader.GetOrdinal("Due_Date"));
                        //policy_payment_schedule.Discount = myReader.GetDouble(myReader.GetOrdinal("Discount"));
                        //policy_payment_schedule.Year = myReader.GetInt32(myReader.GetOrdinal("Year"));
                        //policy_payment_schedule.Policy_ID = myReader.GetString(myReader.GetOrdinal("Policy_ID"));
                        //policy_payment_schedule.Product_ID = myReader.GetString(myReader.GetOrdinal("Product_ID"));
                        //policy_payment_schedule.Sum_Insure = myReader.GetDouble(myReader.GetOrdinal("Sum_Insure"));
                        //policy_payment_schedule.Premium = myReader.GetDouble(myReader.GetOrdinal("Premium"));
                        //policy_payment_schedule.Extra_Premium = myReader.GetDouble(myReader.GetOrdinal("Extra_Premium"));
                        //policy_payment_schedule.Pay_Mode = myReader.GetInt32(myReader.GetOrdinal("Pay_Mode"));
                        //policy_payment_schedule.Premium_After_Discount = myReader.GetDouble(myReader.GetOrdinal("Premium_After_Discount"));
                        //policy_payment_schedule.Time = myReader.GetInt32(myReader.GetOrdinal("Time"));
                        //policy_payment_schedule.Total_Premium = myReader.GetDouble(myReader.GetOrdinal("Total_Premium"));
                        //policy_payment_schedule.Created_Note = myReader.GetString(myReader.GetOrdinal("Created_Note"));

                        policy_payment_schedule.Due_Date = Convert.ToDateTime(myReader["Due_Date"].ToString());
                        policy_payment_schedule.Discount = Convert.ToDouble(myReader["Discount"].ToString());
                        policy_payment_schedule.Year = Convert.ToInt32(myReader["Year"].ToString());
                        policy_payment_schedule.Policy_ID = (myReader["Policy_ID"].ToString());
                        policy_payment_schedule.Product_ID = myReader["Product_ID"].ToString();
                        policy_payment_schedule.Sum_Insure = Convert.ToDouble(myReader["Sum_Insure"].ToString());
                        policy_payment_schedule.Premium = Convert.ToDouble( myReader["Premium"].ToString());
                        policy_payment_schedule.Extra_Premium = Convert.ToDouble( myReader["Extra_Premium"].ToString());
                        policy_payment_schedule.Pay_Mode = Convert.ToInt32( myReader["Pay_Mode"].ToString());
                        policy_payment_schedule.Premium_After_Discount = Convert.ToDouble( myReader["Premium_After_Discount"].ToString());
                        policy_payment_schedule.Time = Convert.ToInt32( myReader["Time"].ToString());
                        policy_payment_schedule.Total_Premium = Convert.ToDouble( myReader["Total_Premium"].ToString());
                        policy_payment_schedule.Created_Note = myReader["Created_Note"].ToString();

                        policy_payment_schedule_list.Add(policy_payment_schedule);
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
            Log.AddExceptionToLog("Error in function [GetPolicyPaymentScheduleDetails] in class [da_policy_payment_schedule]. Details: " + ex.Message);
        }
        return policy_payment_schedule_list;
    }

    //Function to get years
    public static List<bl_policy_payment_schedule> GetYear(string policy_Number)
    {
        List<bl_policy_payment_schedule> years = new List<bl_policy_payment_schedule>();
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "SP_Get_Policy_Payment_Schedule_Years";
                myCommand.Parameters.AddWithValue("@Policy_Number", policy_Number);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {

                        bl_policy_payment_schedule year = new bl_policy_payment_schedule();
                        year.Year = myReader.GetInt32(myReader.GetOrdinal("Year"));
                        years.Add(year);
                      
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
            Log.AddExceptionToLog("Error in function [GetYear] in class [da_policy_payment_schedule]. Details: " + ex.Message);
        }
        return years;
    }

    //Get exist policy payment schedule
    public static DataTable GetExistPolicyPaymentSchedule(string policy_Number)
    {
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();

        //string connString = AppConfiguration.GetConnectionString();
        try
        {
            //using (SqlConnection myConnection = new SqlConnection(connString))
            //{
            //    string strSelect;
            //    strSelect = "SELECT	Ct_Policy.Policy_ID, Ct_Policy.Product_ID, Ct_Policy_Premium.Premium," +
            //                " Ct_Policy_Premium.Sum_Insure, Ct_Policy_Premium.EM_Premium as 'Extra_Premium', Ct_Policy_Pay_Mode.Pay_Mode, " +
            //                " Ct_Policy_Premium.Original_Amount, schedule.discount_amount, schedule.Year, schedule.Created_Note" +
            //                " FROM dbo.Ct_Policy INNER JOIN dbo.Ct_Policy_Number ON Ct_Policy.Policy_ID = Ct_Policy_Number.Policy_ID " +
            //                " INNER JOIN Ct_Policy_Premium on Ct_Policy.Policy_ID = Ct_Policy_Premium.Policy_ID" +
            //                " INNER JOIN Ct_Policy_Pay_Mode on Ct_Policy.Policy_ID = Ct_Policy_Pay_Mode.Policy_ID" +
            //                " INNER JOIN (select policy_id, pay_mode, year, Sum(Discount) as 'discount_amount', Created_Note from Ct_Policy_Payment_Schedule" +
            //                " group by  policy_id, year, Created_Note, pay_mode) as schedule" +
            //                " on schedule.Policy_ID=ct_policy.policy_id and schedule.Pay_Mode=Ct_Policy_Pay_Mode.Pay_Mode" +
            //                " WHERE CAST(CAST(Ct_Policy_Number.Policy_Number AS BIGINT) AS VARCHAR(255)) = @Policy_Number";

            //    SqlCommand myCommand = new SqlCommand();
            //    myConnection.Open();
            //    myCommand.Connection = myConnection;
            //    myCommand.CommandType = CommandType.Text;
            //    myCommand.CommandText = strSelect;
            //    myCommand.Parameters.AddWithValue("@Policy_Number", policy_Number);

            //    SqlDataAdapter cmd;
            //    cmd = new SqlDataAdapter(myCommand);
            //    cmd.Fill(ds);
            //    cmd.Dispose();
            //    tbl = ds.Tables[0];
            //    ds.Dispose();
            //    myCommand.ExecuteNonQuery();
            //    myCommand.Parameters.Clear();
            //    myConnection.Close();

            //}

            tbl = DataSetGenerator.Get_Data_Soure("SP_GET_EXIST_POLICY_PAYMENT_SCHEDULE_BY_POLICY_NUMBER", new string[,] { { "@Policy_Number", policy_Number } });
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetExistPolicyPaymentSchedule] in class [da_policy_payment_schedule]. Details: " + ex.Message);
        }
        return tbl;
    }
    public static bool updatePolicyPaymentSchedule1(bl_policy_payment_schedule update_policy)
    {
        bool resutl = false;
        try
        {

            string strUpdate = "";
            strUpdate = "Update Ct_Policy_Payment_Schedule set discount=@discount_amount, premium=@premium, " +
                        " premium_after_discount=@premium_after_discount," +
                        " extra_premium=@extra_premium, total_premium=@total_premium, created_note=@remark " +
                        " where policy_id=@policy_id and product_id=@product_id and year=@year and pay_mode=@pay_mode";

            string connString = AppConfiguration.GetConnectionString();
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(strUpdate,conn);

            cmd.Parameters.AddWithValue("@discount_amount", update_policy.Discount);
            cmd.Parameters.AddWithValue("@premium", update_policy.Premium);
            cmd.Parameters.AddWithValue("@premium_after_discount", update_policy.Premium_After_Discount);
            cmd.Parameters.AddWithValue("@extra_premium", update_policy.Extra_Premium);
            cmd.Parameters.AddWithValue("@total_premium", update_policy.Total_Premium);
            cmd.Parameters.AddWithValue("@policy_id", update_policy.Policy_ID);
            cmd.Parameters.AddWithValue("@product_id", update_policy.Product_ID);
            cmd.Parameters.AddWithValue("@year", update_policy.Year);
            cmd.Parameters.AddWithValue("@pay_mode", update_policy.Pay_Mode);
            cmd.Parameters.AddWithValue("@remark", update_policy.Created_Note);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            conn.Close();
            resutl = true;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [updatePolicyPaymentSchedule] in class [da_policy_payment_schedule]. Details: " + ex.Message);
        }
        return resutl;
    }

    public static string getPolicyNumber(string policy_id)
    {
        string policy_number = "";
        string str = "";
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                string strselect;

               // strselect = "select policy_number from Ct_Policy_Number where policy_id=@policy_id";

                strselect = "select policy_number from Ct_Policy_Number where policy_id= @policy_id " +
                            " union all " +
                            " select (ct_group_master_product.group_code + '-'  + ct_group_master_policy.seq_number) as 'policy_number'  from ct_group_master_policy inner join ct_group_master_product on ct_group_master_policy.group_master_id = ct_group_master_product.group_master_id " +
                            " where ct_group_master_policy.policy_id = @policy_id";
                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = strselect;
                myCommand.Parameters.AddWithValue("@policy_id", policy_id);
                using (SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (myReader.Read())
                    {
                        str = (string)myReader["policy_number"];
                    }
                    myReader.Close();
                }
                myConnection.Open();
                myCommand.ExecuteReader();
                myConnection.Close();
                string str1 = "";
                for (int i = 0; i <= str.Length - 1; i++)
                {
                    //delete zero
                    str1 = str.Substring(i, 1);
                    if (str1.Trim() != "0")
                    {
                        policy_number = policy_number + str1;
                    }
                    else
                    {
                        if (policy_number.Trim() != "")
                        {
                            policy_number = policy_number + str1;
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [getPolicyNumber] in class [PolicyPaymentScheduleWebService]. Details: " + ex.Message);
        }
        return policy_number;
    }

    public static bool getPayStatus(string policy_id, string year)
    {
        bool result = true;
        try
        {
            string strselect = "select Policy_ID from [Ct_Policy_Prem_Pay] where Policy_ID=@policy_id and prem_year=@year and Prem_Lot=1";
            SqlConnection conn = new SqlConnection(AppConfiguration.GetConnectionString());
            conn.Open();
            SqlCommand cmd = new SqlCommand(strselect, conn);
            cmd.Parameters.AddWithValue("@policy_id", policy_id);
            cmd.Parameters.AddWithValue("@year", year);
            SqlDataReader dr;

            dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                result = false;
            }
            dr.Close();
            cmd.Parameters.Clear();
            cmd.Dispose();
            conn.Close();
           }

        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error in function [getPayStatus] in class [da_policy_payment_schedule]. Datail: " + ex.Message);
        }
        return result;
    }
    #endregion
}