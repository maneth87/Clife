using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_policy_cl24
/// </summary>
public class da_policy_cl24
{
	public da_policy_cl24()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region CL24 By Meas Sun
    public static List<bl_before_due_notification> GetPolicyCL24BeforeDue(DateTime current_date, string updated_by, DateTime updated_on, string updated_note)
    {
        List<bl_before_due_notification> policyList = new List<bl_before_due_notification>();
        try
        {
            policyList = da_policy_cl24.GetPolicyCL24List(current_date);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicyCL24BeforeDue(DateTime current_date)] in class [da_policy_CL24], detail:" + ex.Message);

        }
        return policyList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current_date"></param>
    /// <returns></returns>
    public static List<bl_before_due_notification> GetPolicyCL24List(DateTime current_date)
    {
        List<bl_before_due_notification> list_before_due = new List<bl_before_due_notification>();

        try
        {
            bl_before_due_notification.bl_before_due_notification_sub obj_before_due;
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CL24_POLICY_BEFORE_DUE]", new string[,] { }, "Function [GetPolicyCL24List()] class [da_policy_CL24]");
            foreach (DataRow row in tbl.Rows)
            {
                obj_before_due = new bl_before_due_notification.bl_before_due_notification_sub();
                obj_before_due.DueDate = Convert.ToDateTime(row["due_date"].ToString());
                obj_before_due.CompareDate = current_date;
                obj_before_due.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                obj_before_due.PayMode = Convert.ToInt32(row["pay_mode"].ToString());
                
                if (obj_before_due.NumberOfNextDueDay == 30) //30 Days Before Due Date
                {
                    obj_before_due.CustomerID = row["customer_id"].ToString();
                    obj_before_due.App_Number = row["app_number"].ToString();
                    obj_before_due.PolicyNumber = row["policy_number"].ToString();
                    obj_before_due.NameEN = row["en_name"].ToString();
                    obj_before_due.NameKH = row["kh_name"].ToString();
                    obj_before_due.Gender = row["gender"].ToString();
                    obj_before_due.DOB = Convert.ToDateTime(row["birth_date"].ToString());
                    obj_before_due.PhoneNumber = row["phone"].ToString();
                    obj_before_due.ProductID = row["product_id"].ToString();
                    obj_before_due.Product_Title = row["en_title"].ToString();
                    obj_before_due.Premium = Convert.ToDouble(row["premium"].ToString());

                    //Find Sub Policy
                    if (obj_before_due.App_Number != "" && obj_before_due.CustomerID != "")
                    {
                        DataTable tbl_sub = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_SUB_POLICY_CREDIT_LIFE_BY_APP_NUMBER", new string[,] { 
                        { "@APP_NUMBER", obj_before_due.App_Number }, 
                        { "@CUSTOMER_ID", obj_before_due.CustomerID } 
                        }, "Function [GetPolicyCL24List()] class [da_policy_CL24]");
                        foreach (DataRow sub in tbl_sub.Rows)
                        {
                            obj_before_due.Policy_Number_Sub = "-" + sub["policy_number"].ToString();
                        }
                    }

                    list_before_due.Add(obj_before_due);

                }
            }
        }
        catch (Exception ex)
        {
            list_before_due = new List<bl_before_due_notification>();
            Log.AddExceptionToLog("Error function  [GetPolicyLapList()] in class [da_policy_CL24], detail:" + ex.Message);
        }
        return list_before_due;
    }

    public static DataTable GetRenewalPremiumList(string month, string year)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CL24_RENEWAL_PREMIUM]", new string[,] { 
            { "@NEXT_DUE_MONTH", month }, 
            { "@YEAR", year } 
            }, "Function [GetRenewalPremiumList()] class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetRenewalPremiumList()] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return dt;
        
    }

    public static DataTable Generate_CL24_Renewal_Premium(string month, string year, string created_by)
    {
        DataTable tbl_result = new DataTable();
        try
        {
            tbl_result = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_GENERATE_CL24_RENEWAL_PREMIUM", new string[,] { { "@MONTH", month }, { "@YEAR", year }, { "@CREATED_BY", created_by } }, "da_policy_CL24 => Generate_CL24_Renewal_Premium(string month, string year, string created_by)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Generate_CL24_Renewal_Premium()] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return tbl_result;

    }

    /// <summary>
    /// GET NEW PREMIUM LIST
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public static DataTable GetNewPremiumList(string month, string year)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CL24_NEW_PREMIUM]", new string[,] { 
            { "@MONTH", month }, 
            { "@YEAR", year } 
            }, "Function [GetNewPremiumList()] class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetNewPremiumList()] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return dt;

    }

    public static bool Save_CL24_Renewal_Premium(bl_cl24_renewal_premium renewal_prem_obj)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_SAVE_CL24_RENEWAL_PREMIUM", new string[,]{
                {"@POLICY_ID", renewal_prem_obj.Policy_ID},
                {"@POLICY_NUMBER", renewal_prem_obj.Policy_Number},
                {"@CUSTOMER_ID", renewal_prem_obj.Customer_ID},
                {"@INSURED_NAME", renewal_prem_obj.Insured_Name},
                {"@BIRTH_DATE", renewal_prem_obj.Birth_Date+""},
                {"@GENDER", renewal_prem_obj.Gender+""},
                {"@PHONE_NUMBER", renewal_prem_obj.Phone_Number+""},
                {"@SUM_INSURE", renewal_prem_obj.Sum_Insure+""},
                {"@EFFECTIVE_DATE", renewal_prem_obj.Effective_Date+""},
                {"@ISSUE_DATE", renewal_prem_obj.Issued_Date+""},
                {"@START_DATE", renewal_prem_obj.Start_Date+""},
                {"@END_DATE", renewal_prem_obj.End_Date+""},
                {"@DUE_DATE", renewal_prem_obj.Due_Date+""},
                {"@PRODUCT_ID", renewal_prem_obj.Product_ID+""},
                {"@PRODUCT_NAME", renewal_prem_obj.Product_Name+""},
                {"@PREMIUM", renewal_prem_obj.Premium+""},
                {"@EM_AMOUNT", renewal_prem_obj.EM_Amount+""},
                {"@NEXT_DUE", renewal_prem_obj.Next_Due_Date+""},
                {"@PAY_YEAR", renewal_prem_obj.Pay_Year+""},
                {"@PAY_LOT", renewal_prem_obj.Pay_Lot+""},
                {"@AGENT_CODE", renewal_prem_obj.Agent_Code+""},
                {"@AGENT_NAME", renewal_prem_obj.Agent_Name+""},
                {"@POLICY_YEAR", renewal_prem_obj.Policy_Year+""},
                {"@PAY_MODE_ID", renewal_prem_obj.Pay_Mode_ID+""},
                {"@PAYMENT_MODE", renewal_prem_obj.Paymode_Mode+""},
                {"@FACTOR", renewal_prem_obj.Factor+""},
                {"@TOAL_PREMIUM", renewal_prem_obj.Total_Premium+""},
                {"@CREATED_BY", renewal_prem_obj.Created_By},
                {"@REPORT_DATE", renewal_prem_obj.Report_Date+""},
                {"@REMARK", renewal_prem_obj.Remark}
                }, "Function [Save_CL24_Renewal_Premium(string next_due_month, string year), class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Save_CL24_Renewal_Premium(bl_cl24_renewal_premium renewal_prem_obj)], detail:" + ex.Message);
        }
        return result;    
    }

    public static DataTable GenerateNewIssueFirstFinancePremium(string Policy_Number)
    {
        DataTable tbl_result = new DataTable();
        try
        {
            tbl_result = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_GENERATE_FIRST_ISSUE_CL24_PREMIUM", new string[,] { { "@POLICY_NO", Policy_Number } }, "da_cmk => GenerateNewIssueFirstFinancePremium(string Policy_Number)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GenerateNewIssueFirstFinancePremium(string Policy_Number)] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return tbl_result;

    }

    public static bool InsertNewIssueFirstFinancePremium(DataTable tbl, string user_name)
    {
        bool result = false;
        try
        {
            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow item in tbl.Rows)
                {
                    bl_cl24_renewal_premium renewal_prem;
                    renewal_prem = new bl_cl24_renewal_premium();
                    renewal_prem.Policy_ID = item["Policy_ID"].ToString().Trim();
                    renewal_prem.Policy_Number = item["Policy_Number"].ToString().Trim();
                    renewal_prem.Customer_ID = item["Customer_ID"].ToString().Trim();
                    renewal_prem.Insured_Name = item["Insured_Name"].ToString().Trim();
                    renewal_prem.Birth_Date = Convert.ToDateTime(item["Birth_Date"].ToString().Trim());
                    renewal_prem.Gender = item["Gender"].ToString().Trim();
                    renewal_prem.Phone_Number = item["Phone_Number"].ToString().Trim();
                    renewal_prem.Sum_Insure = item["Sum_Insure"].ToString() != "" ? float.Parse(item["Sum_Insure"].ToString().Trim()) : 0;
                    renewal_prem.Effective_Date = Convert.ToDateTime(item["Effective_Date"].ToString().Trim());
                    renewal_prem.Issued_Date = Convert.ToDateTime(item["Issue_Date"].ToString().Trim());
                    renewal_prem.Start_Date = Convert.ToDateTime(item["Start_Date"].ToString().Trim());
                    renewal_prem.End_Date = Convert.ToDateTime(item["End_Date"].ToString().Trim());
                    renewal_prem.Due_Date = Convert.ToDateTime(item["Due_Date"].ToString().Trim());
                    renewal_prem.Product_ID = item["Product_ID"].ToString().Trim();
                    renewal_prem.Product_Name = item["Product_Name"].ToString().Trim();
                    renewal_prem.Premium = item["Premium"].ToString() != "" ? float.Parse(item["Premium"].ToString().Trim()) : 0;
                    renewal_prem.EM_Amount = item["EM_Amount"].ToString() != "" ? float.Parse(item["EM_Amount"].ToString().Trim()) : 0;
                    renewal_prem.Next_Due_Date = Convert.ToDateTime(item["Next_Due"].ToString().Trim());
                    renewal_prem.Pay_Year = Convert.ToInt16(item["Pay_Year"].ToString().Trim());
                    renewal_prem.Pay_Lot = Convert.ToInt16(item["Pay_Lot"].ToString().Trim());
                    renewal_prem.Agent_Code = item["Agent_Code"].ToString().Trim();
                    renewal_prem.Agent_Name = item["Agent_Name"].ToString().Trim();
                    renewal_prem.Policy_Year = item["Policy_Year"].ToString().Trim();
                    renewal_prem.Pay_Mode_ID = Convert.ToInt16(item["Pay_Mode_ID"].ToString().Trim());
                    renewal_prem.Paymode_Mode = item["Payment_Mode"].ToString().Trim();
                    renewal_prem.Factor = Convert.ToInt16(item["Factor"].ToString().Trim());
                    renewal_prem.Total_Premium = item["Total_Premium"].ToString() != "" ? float.Parse(item["Total_Premium"].ToString().Trim()) : 0;
                    renewal_prem.Created_By = user_name;
                    renewal_prem.Report_Date = Convert.ToDateTime(item["Issue_Date"].ToString().Trim());
                    renewal_prem.Remark = "First Issue policy, New premium";

                    if (da_policy_cl24.Save_CL24_Renewal_Premium(renewal_prem) != false)
                    { result = true; } 

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [InsertNewIssueFirstFinancePremium(DataTable tbl)], detail:" + ex.Message);
        }
        return result;
    }

    public static bool CheckDueMonthGenerate_RenewalPremium(string next_due_month, string year) 
    {
        bool result = false;

        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CHECK_EXIST_CL24_RENEWAL_PREMIUM_IN_MONTH", new string[,] { 
                {"@NEXT_DUE_MONTH", next_due_month},
                {"@YEAR", year}
                }, "Function [CheckDueMonthGenerate_RenewalPremium(string next_due_month, string year), class [da_policy_CL24]");
            if (tbl.Rows.Count > 0)
            {
                result = true;
            }
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [CheckDueMonthGenerate_RenewalPremium(string next_due_month, string year)], detail:" + ex.Message);
        }
        return result;
         
    }

    public static bool InsertRemark(string policy_number, string remark)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_TEMP_SAVE_CL24_REMARK", new string[,]{
                {"@POLICY_NUMBER", policy_number},
                {"@REMARK", remark}
                }, "Function [InsertRemark(string policy_number, string remark), class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [InsertRemark(string policy_number, string remark)], detail:" + ex.Message);
        }
        return result;    
    
    }

    public static bool EditRemark(string policy_number, string remark)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_TEMP_UPDATE_CL24_REMARK", new string[,]{
                {"@POLICY_NUMBER", policy_number},
                {"@REMARK", remark}
                }, "Function [InsertRemark(string policy_number, string remark), class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [EditRemark(string policy_number, string remark)], detail:" + ex.Message);
        }
        return result;

    }

    public static string GetPolicyRemark(string policy_number)
    {
        string Remark = "";

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                string sql = @"SELECT REMARK FROM _TEMP_SAVE_CL24_REMARK 
                             WHERE POLICY_NUMBER = @policy_number AND STATUS IS NULL";

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;

                myCommand.Parameters.AddWithValue("@policy_number", policy_number);

                Remark = myCommand.ExecuteScalar().ToString();

                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
            Remark = "";
        }

        return Remark;
    }

    // CASE PAID OFF POLICY 
    public static bool InsertStatusRemark(string policy_number, int pay_year, string status, string remark, DateTime paid_off_date, string created_by)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_TEMP_SAVE_CL24_STATUS_REMARK", new string[,]{
                {"@POLICY_NUMBER", policy_number},
                {"@PAY_YEAR", pay_year+""},
                {"@STATUS", status},
                {"@REMARK", remark},
                {"@PAID_OFF_DATE", paid_off_date+""},
                {"@CREATED_BY", created_by},
                {"@CREATED_ON", DateTime.Now+""}
                }, "Function [InsertStatusRemark(string policy_number, int pay_year, string status, string remark, DateTime paid_off_date, string created_by), class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [InsertStatusRemark(string policy_number, int pay_year, string status, string remark, DateTime terminate_Date, string created_by)], detail:" + ex.Message);
        }
        return result;

    }

    public static bool EditStatusRemark(string policy_number, int pay_year, string status, string remark, DateTime paid_off_date)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_TEMP_UPDATE_CL24_STATUS_REMARK", new string[,]{
                {"@POLICY_NUMBER", policy_number},
                {"@PAY_YEAR", pay_year+""},
                {"@STATUS", status},
                {"@REMARK", remark},
                {"@PAID_OFF_DATE", paid_off_date+""}
                }, "Function [EditStatusRemark(string policy_number, int pay_year, string status, string remark, DateTime paid_off_date), class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [EditStatusRemark(string policy_number, int pay_year, string status, string remark, DateTime paid_off_date)], detail:" + ex.Message);
        }
        return result;

    }

    public static bool Save_CL24_Paid_Off_Policy(bl_cl24_renewal_premium renewal_prem_obj)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_CL24_PAID_OFF_POLICY", new string[,]{
                {"@POLICY_ID", renewal_prem_obj.Policy_ID},
                {"@PAY_YEAR", renewal_prem_obj.Pay_Year+""},
                {"@STATUS_REMARK", renewal_prem_obj.Status_Remark},
                {"@REMARK", renewal_prem_obj.Remark},
                {"@PAID_OFF_DATE", renewal_prem_obj.Paid_Off_Date+""}
                }, "Function [Save_CL24_Paid_Off_Policy(bl_cl24_renewal_premium renewal_prem_obj), class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Save_CL24_Paid_Off_Policy(bl_cl24_renewal_premium renewal_prem_obj)], detail:" + ex.Message);
        }
        return result;
    }


    public static DataTable GetPolicyStatusRemark(string policy_number, int pay_year)
    {
        DataTable dt = new DataTable();

        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                string sql = @"SELECT * FROM _TEMP_SAVE_CL24_REMARK 
                             WHERE POLICY_NUMBER = @policy_number AND PAY_YEAR = pay_year";

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;

                myCommand.Parameters.AddWithValue("@policy_number", policy_number);

                SqlDataAdapter dap = new SqlDataAdapter(myCommand);

                dap.Fill(dt);

                myConnection.Close();
            }
        }
        catch (Exception ex)
        {
        }

        return dt;
    }

    public static DataTable GetRenewalPremiumByPolicyYearList(string policy_year, string policy_number)
    {
        DataTable dt = new DataTable();

        try
        {
            dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CL24_RENEWAL_PREMIUM_BY_POLICY_YEAR]", new string[,] { 
            { "@POLICY_YEAR", policy_year }, 
            { "@POLICY_NUMBER", policy_number } 
            }, "Function [GetRenewalPremiumByPolicyYearList()] class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetRenewalPremiumByPolicyYearList()] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return dt;

    }

    public static DataTable GetPremiumByDateRangeList(DateTime from_date, DateTime to_date, int search_date)
    {
        DataTable dt = new DataTable();

        try
        {
            dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CL24_PREMIUM_BY_DATE_RANGE]", new string[,] { 
            { "@FROM_DATE", from_date +""}, 
            { "@TO_DATE", to_date +""},
            { "@SEARCH_DATE", search_date +""} 
            }, "Function [GetPremiumByDateRangeList(DateTime from_date, DateTime to_date, int search_date)] class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPremiumByDateRangeList(DateTime from_date, DateTime to_date, int search_date)] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return dt;
    }

    public static DataTable GetPolicyByReportDateList(DateTime report_date)
    {
        DataTable dt = new DataTable();

        try
        {
            dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CL24_POLICY_BY_REPORT_DATE]", new string[,] { 
            { "@REPORT_DATE", report_date +""}
            }, "Function [GetPolicyByReportDateList(DateTime report_date)] class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicyByReportDateList(DateTime report_date)] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return dt;
    }

    public static DataTable GetSubPolicyByMainPolicyID(string Policy_Number)
    {
        DataTable dt = new DataTable();

        try
        {
            dt = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_FIRST_FINANCE_SUB_POLICY_BY_MAIN_POLICY_NUMBER]", new string[,] { 
            { "@POLICY_NUMBER", Policy_Number}, 
            }, "Function [GetSubPolicyByMainPolicyID(string Policy_Number)] class [da_policy_CL24]");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetSubPolicyByMainPolicyID(string Policy_Number)] in class [da_policy_CL24], detail:" + ex.Message);
        }

        return dt;
    }

    // ROLL BACK REMARK
    public static void RemoveStatusRemark(string policy_number, int pay_year)
    {
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                string sql = @"DELETE FROM _TEMP_SAVE_CL24_REMARK WHERE POLICY_NUMBER = @POLICY_NUMBER AND PAY_YEAR = @PAY_YEAR";

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;

                myCommand.Parameters.AddWithValue("@POLICY_NUMBER", policy_number);
                myCommand.Parameters.AddWithValue("@PAY_YEAR", pay_year);

                myCommand.ExecuteNonQuery();
                myConnection.Close();

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [RemoveStatusRemark(string policy_number, int pay_year)], detail:" + ex.Message);
        }
    }

    // ROLL BACK REMARK
    public static void RollBackRemark(string policy_number)
    {
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection myConnection = new SqlConnection(connString))
            {
                string sql = @"DELETE FROM _TEMP_SAVE_CL24_REMARK WHERE POLICY_NUMBER = @POLICY_NUMBER AND PAY_YEAR IS NULL";

                SqlCommand myCommand = new SqlCommand();
                myConnection.Open();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;

                myCommand.Parameters.AddWithValue("@POLICY_NUMBER", policy_number);

                myCommand.ExecuteNonQuery();
                myConnection.Close();

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [RollBackRemark(string policy_number)], detail:" + ex.Message);
        }
    }
    #endregion
}