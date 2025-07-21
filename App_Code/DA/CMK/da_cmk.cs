using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_cmk
/// </summary>
public class da_cmk
{
	public da_cmk()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public class Policy
    {
        public Policy() { }
        
        /// <summary>
        /// Get policy by loan id
        /// </summary>
        /// <param name="loan_id"></param>
        /// <returns></returns>
        public static List<bl_cmk.CMK_Policy> GetPolicyByLoanID(string loan_id)
        {
            List<bl_cmk.CMK_Policy> policy_list = new List<bl_cmk.CMK_Policy>();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CMK_POLICY_BY_LOAN_ID", new string[,] { 
                {"@LOAN_ID", loan_id}
                }, "Function [GetPolicyByLoanID(string loan_id), class [da_cmk=>Policy]");

                foreach (DataRow row in tbl.Rows)
                {
                    bl_cmk.CMK_Policy policy = new bl_cmk.CMK_Policy();

                    policy.CMKPolicyID = row["Cmk_Policy_ID"].ToString();
                    policy.CMKCustomerID = row["Cmk_Customer_ID"].ToString();
                    policy.CustomerID = row["Customer_ID"].ToString();
                    policy.CertificateNo = row["Certificate_No"].ToString();
                    policy.LoanID = row["Loan_ID"].ToString();
                    policy.LoanType = row["Loan_Type"].ToString();
                    policy.Group = row["Group"].ToString();
                    policy.OpenedDate = Convert.ToDateTime(row["Opened_Date"].ToString());
                    policy.EffectiveDate = Convert.ToDateTime(row["Effective_Date"].ToString());
                    policy.DateOfEntry = Convert.ToDateTime(row["Date_Of_Entry"].ToString());
                    policy.Currancy = row["Currancy"].ToString();
                    policy.Age = Convert.ToInt32(row["Age"].ToString());
                    policy.LoanDuration = Convert.ToInt32(row["Loan_Duration"].ToString());
                    policy.CoveredYear = Convert.ToInt32(row["Covered_Year"].ToString());
                    policy.CreatedBy = row["Created_By"].ToString();
                    policy.CreatedOn = Convert.ToDateTime(row["Created_On"].ToString());

                    policy_list.Add(policy);
                }

            }
            catch (Exception ex)
            {
                policy_list = new List<bl_cmk.CMK_Policy>(); //return empty object
                Log.AddExceptionToLog("Error function [GetPolicyByLoanID(string loan_id)] in class [da_cmk=>Policy], detail: " + ex.Message);
            }
            return policy_list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer_id"></param>
        /// <returns></returns>
        public static bl_cmk.CMK_Policy GetPolicyByCustomerID(string customer_id)
        {
            bl_cmk.CMK_Policy policy = new bl_cmk.CMK_Policy();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CMK_POLICY_BY_CUSTOMER_ID", new string[,] { 
                {"@CUSTOMER_ID", customer_id}
                }, "Function [GetPolicyByCustomerID(string customer_id), class [da_cmk=>Policy]");
                if (tbl.Rows.Count > 0)
                {
                    var row = tbl.Rows[0];
                    policy.CMKPolicyID = row["Cmk_Policy_ID"].ToString();
                    policy.CMKCustomerID = row["CMK_Customer_ID"].ToString();
                    policy.CustomerID = row["Customer_ID"].ToString();

                }
            }
            catch (Exception ex)
            {
                policy = new bl_cmk.CMK_Policy();//return empty object
                Log.AddExceptionToLog("Error function [GetPolicyByCustomerID(string customer_id)] in class [da_cmk=>Policy], detail: " + ex.Message);
            }
            return policy;
        }


        /// <summary>
        /// </summary>
        /// <param name="policy_prem">Object [bl_cmk.CMK_Policy_Prem]</param>
        /// <param name="report_date">Object [DateTime]</param>
        /// <returns></returns>
        public static bool CheckExistPremiumReport(string cmk_policy_id, DateTime report_date)
        {
            bool result = false;

            bl_ci.Policy policy = new bl_ci.Policy();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CHECK_EXIST_CMK_POLICY_PREMIUM_REPORT", new string[,] { 
                {"@CMK_POLICY_ID", cmk_policy_id},
                {"@REPORT_DATE", report_date+""}
                }, "Function [CheckExistPremiumReport(string cmk_policy_id, DateTime report_date), class [da_cmk=>Policy]");
                if (tbl.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [CheckExistPremiumReport(bl_cmk.CMK_Policy_Prem policy)] in class [da_cmk => Policy], detail:" + ex.Message);
            }
            return result;

        }

        /// <summary>
        /// </summary>
        /// <param name="policy_prem">Object [bl_cmk.CMK_Policy_Prem]</param>
        /// <param name="report_date">Object [DateTime]</param>
        /// <returns></returns>
        public static bool CheckExistPremiumReportDate(DateTime report_date)
        {
            bool result = false;

            bl_ci.Policy policy = new bl_ci.Policy();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_CHECK_EXIST_CMK_PREMIUM_REPORT_DATE", new string[,] { 
                {"@REPORT_DATE", report_date+""}
                }, "Function [CheckExistPremiumReportDate(DateTime report_date), class [da_cmk=>Policy]");
                if (tbl.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [CheckExistPremiumReportDate(DateTime report_date)] in class [da_cmk => Policy], detail:" + ex.Message);
            }
            return result;

        }

        public static bl_cmk.CMK_Policy_Prem VerifyPolicyPremium(string cmk_policy_id)
        {
            bl_cmk.CMK_Policy_Prem pol_prem = new bl_cmk.CMK_Policy_Prem();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CMK_MAIN_PREMIUM_REPORT", new string[,] { 
                {"@CMK_POLICY_ID", cmk_policy_id}
                }, "Function [VerifyPolicyPremium(string cmk_policy_id), class [da_cmk=>Policy]");
                if (tbl.Rows.Count > 0)
                {
                    var row = tbl.Rows[0];
                    pol_prem.CMKPolicyID = row["Cmk_Policy_ID"].ToString();
                    pol_prem.LoanAmount = Convert.ToDouble(row["Loan_Amount"]);
                    pol_prem.LoanAmountRiel = Convert.ToDouble(row["Loan_Amount_Riel"]);
                    pol_prem.OutstandingBalance = Convert.ToDouble(row["Outstanding_Balance"]);
                    pol_prem.OutstandingBalanceRiel = Convert.ToDouble(row["Outstanding_Balance_Riel"]);
                    pol_prem.AssuredAmount = Convert.ToDouble(row["Assured_Amount"]);
                    pol_prem.AssuredAmountRiel = Convert.ToDouble(row["Assured_Amount_Riel"]);
                    pol_prem.MonthlyPremium = Convert.ToDouble(row["Monthly_Premium"]);
                    pol_prem.TotalPremium = Convert.ToDouble(row["Total_Premium"]);

                }
            }
            catch (Exception ex)
            {
                pol_prem = new bl_cmk.CMK_Policy_Prem();//return empty object
                Log.AddExceptionToLog("Error function [VerifyPolicyPremium(string customer_id)] in class [da_cmk=>Policy], detail: " + ex.Message);
            }
            return pol_prem;

        }

        /// <summary>
        /// Save policy into table [ct_cmk_policy]
        /// </summary>
        /// <param name="policy">Object [bl_cmk.CMK_Policy]</param>
        /// <returns></returns>
        public static bool SavePolicy(bl_cmk.CMK_Policy policy)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CMK_POLICY", new string[,]{
                    {"@CMK_POLICY_ID",policy.CMKPolicyID},
                    {"@CMK_CUSTOMER_ID", policy.CMKCustomerID},
                    {"@CUSTOMER_ID", policy.CustomerID},
                    {"@PRODUCT_ID", policy.ProductID},
                    {"@CERTIFICATE_NO", policy.CertificateNo},
                    {"@LOAN_ID", policy.LoanID},
                    {"@LOAN_TYPE", policy.LoanType},
                    {"@GROUP", policy.Group},
                    {"@OPENED_DATE", policy.OpenedDate+""},
                    {"@EFFECTIVE_DATE", policy.EffectiveDate+""},
                    {"@DATE_OF_ENTRY", policy.DateOfEntry+""},
                    {"@EXPIRE_DATE", policy.ExpireDate+""},
                    {"@CURRANCY", policy.Currancy},
                    {"@AGE", policy.Age+""},
                    {"@LOAN_DURATION", policy.LoanDuration+""},
                    {"@COVERED_YEAR", policy.CoveredYear+""},
                    {"@BRANCH", policy.Branch},
                    {"@CHANNEL_LOCATION_ID", policy.ChannelLocationID+""},
                    {"@CHANNEL_CHANNEL_ITEM_ID", policy.ChannelChannelItemID+""},
                    {"@CREATED_BY", policy.CreatedBy},
                    {"@CREATED_ON", policy.CreatedOn+""},
                    {"@CREATED_NOTED", policy.CreatedNoted+""}
                }, "Function [SavePolicy(bl_cmk.CMK_Policy policy)], class [da_cmk => Policy ]");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [SavePolicy(bl_ci.Policy policy)] in class [da_ci => Policy], detail:" + ex.Message);
            }

            return result;
        }
        
        public static bool DeletePolicy(string policy_id)
        {

            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_CI_POLICY_BY_POLICY_ID", new string[,]{
                    {"@POLICY_ID",policy_id}
                }, "Function [DeletePolicy(string policy_id)], class [da_ci => Policy ]");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [DeletePolicy(string policy_id)] in class [da_ci => Policy], detail:" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Save policy Premium into table [ct_cmk_policy_premium]
        /// </summary>
        /// <param name="policy_premium">Object [bl_cmk.CMK_Policy_Prem]</param>
        /// <returns></returns>
        public static bool SavePolicyPremium(bl_cmk.CMK_Policy_Prem policy_prem)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CMK_POLICY_PREMIUM", new string[,]{
                    {"@CMK_POLICY_PREMIUM_ID",policy_prem.CMKPolicyPremiumID},
                    {"@CMK_POLICY_ID",policy_prem.CMKPolicyID},
                    {"@LOAN_AMOUNT", policy_prem.LoanAmount+""},
                    {"@OUTSTANDING_BALANCE", policy_prem.OutstandingBalance+""},
                    {"@ASSURED_AMOUNT", policy_prem.AssuredAmount+""},
                    {"@LOAN_AMOUNT_RIEL", policy_prem.LoanAmountRiel+""},
                    {"@OUTSTANDING_BALANCE_RIEL", policy_prem.OutstandingBalanceRiel+""},
                    {"@ASSURED_AMOUNT_RIEL", policy_prem.AssuredAmountRiel+""},
                    {"@MONTHLY_PREMIUM", policy_prem.MonthlyPremium+""},
                    {"@EXTRA_PREMIUM", policy_prem.ExtraPremium+""},
                    {"@DISCOUNT_AMOUNT", policy_prem.DiscountAmount+""},
                    {"@PREMIUM_AFTER_DISCOUNT", policy_prem.PremiumAfterDiscount+""},
                    {"@TOTAL_PREMIUM", policy_prem.TotalPremium+""},
                    {"@REPORT_DATE", policy_prem.ReportDate+""},
                    {"@PAY_MODE_ID", policy_prem.PaymodeID+""},
                    {"@PAID_OFF_IN_MONTH", policy_prem.PaidOffInMonth+""},
                    {"@TERMINATE_DATE", policy_prem.TerminateDate+""},
                    {"@PAYMENT_BATCH_NO", policy_prem.PaymentBatchNo},
                    {"@CREATED_BY", policy_prem.CreatedBy},
                    {"@CREATED_ON", policy_prem.CreatedOn+""},
                    {"@CREATED_NOTED", policy_prem.CreatedNoted+""},
                    {"@STATUS", policy_prem.Status.Trim().ToUpper()}

                }, "Function [SavePolicyPremium(bl_cmk.CMK_Policy policy_prem)], class [da_cmk => Policy ]");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [SavePolicy(bl_ci.Policy policy)] in class [da_ci => Policy], detail:" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Save Renewal Premium into table [Ct_CMK_Renewal_premium]
        /// </summary>
        /// <param name="policy_premium">Object [bl_cmk.CMK_Policy_Prem]</param>
        /// <returns></returns>
        public static bool SaveRenewalPremium(bl_cmk.CMK_Renewal_Premium renewal_prem)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CMK_RENEWAL_PREMIUM", new string[,]{
                    {"@CMK_POLICY_PREMIUM_ID",renewal_prem.CMK_Policy_Premium_ID},
                    {"@CMK_POLICY_ID",renewal_prem.CMK_Policy_ID},
                    {"@POLICY_NUMBER", renewal_prem.Policy_Number+""},
                    {"@CUSTOMER_ID", renewal_prem.Customer_ID+""},
                    {"@PRODUCT_ID", renewal_prem.Product_ID+""},
                    {"@PRODUCT_NAME", renewal_prem.Product_Name+""},
                    {"@EFFECTIVE_DATE", renewal_prem.Effective_Date+""},
                    {"@SUM_INSURE", renewal_prem.Sum_Insure+""},
                    {"@PREMIUM", renewal_prem.Premium+""},
                    {"@EXTRA_PREMIUM", renewal_prem.Extra_Premium+""},
                    {"@DISCOUNT_AMOUNT", renewal_prem.Discount_Amount+""},
                    {"@TOTAL_PREMIUM", renewal_prem.Total_Premium+""},
                    {"@INVOICE_NO", renewal_prem.Invoice_No+""},
                    {"@PAY_YEAR", renewal_prem.Pay_Year+""},
                    {"@PAY_LOT", renewal_prem.Pay_Lot+""},
                    {"@MODE", renewal_prem.Mode},
                    {"@REPORT_DATE", renewal_prem.Report_Date+""},
                    {"@CREATED_BY", renewal_prem.Created_By},
                    {"@CREATED_ON", renewal_prem.Created_On+""},
                    {"@CREATED_NOTE", renewal_prem.Created_Note+""},

                }, "Function [SaveRenewalPremium(bl_cmk.CMK_Renewal_Premium renewal_prem)], class [da_cmk => Policy ]");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [SaveRenewalPremium(bl_cmk.CMK_Renewal_Premium renewal_prem)] in class [da_cmk => Policy], detail:" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Save Group Premium into table [Ct_CMK_Group_premium]
        /// </summary>
        /// <param name="group_prem">Object [bl_cmk.CMK_Group_Premium]</param>
        /// <returns></returns>
        public static bool SaveGroupPremium(bl_cmk.CMK_Group_Premium group_prem)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CMK_GROUP_PREMIUM", new string[,]{
                    {"@CMK_GROUP_POLICY_ID",group_prem.CMK_Group_Policy_ID},
                    {"@GROUP_CODE", group_prem.Group_Code+""},
                    {"@PRODUCT_ID", group_prem.Product_ID+""},
                    {"@PRODUCT_NAME", group_prem.Product_Name+""},
                    {"@EFFECTIVE_DATE", group_prem.Effective_Date+""},
                    {"@SUM_INSURE", group_prem.Sum_Insure+""},
                    {"@AMOUNT", group_prem.Amount+""},
                    {"@INVOICE_NO", group_prem.Invoice_No+""},
                    {"@PAY_YEAR", group_prem.Pay_Year+""},
                    {"@PAY_LOT", group_prem.Pay_Lot+""},
                    {"@MODE", group_prem.Mode+""},
                    {"@REPORT_DATE", group_prem.Report_Date+""},
                    {"@NUMBER_OF_POLICY", group_prem.Number_Of_Policy+""},
                    {"@CREATED_ON", group_prem.Created_On+""},
                    {"@CREATED_BY", group_prem.Created_By},

                }, "Function [SaveGroupPremium(bl_cmk.CMK_Group_Premium group_prem)], class [da_cmk => Policy ]");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [SaveGroupPremium(bl_cmk.CMK_Group_Premium group_prem)] in class [da_ci => Policy], detail:" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// UPDATE Group Premium into table [Ct_CMK_Group_premium]
        /// </summary>
        /// <param name="group_prem">Object [bl_cmk.CMK_Group_Premium]</param>
        /// <returns></returns>
        public static bool UpdateGroupPremium(bl_cmk.CMK_Group_Premium group_prem)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CMK_GROUP_PREMIUM", new string[,]{
                    {"@CMK_GROUP_POLICY_ID",group_prem.CMK_Group_Policy_ID},
                    {"@SUM_INSURE", group_prem.Sum_Insure+""},
                    {"@AMOUNT", group_prem.Amount+""},
                    {"@INVOICE_NO", group_prem.Invoice_No+""},
                    {"@REPORT_DATE", group_prem.Report_Date+""},
                    {"@NUMBER_OF_POLICY", group_prem.Number_Of_Policy+""},
                    {"@UPDATED_ON", group_prem.Updated_On+""},
                    {"@UPDATED_BY", group_prem.Updated_By},

                }, "Function [UpdateGroupPremium(bl_cmk.CMK_Group_Premium group_prem)], class [da_cmk => Policy ]");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [SaveGroupPremium(bl_cmk.CMK_Group_Premium group_prem)] in class [da_ci => Policy], detail:" + ex.Message);
            }

            return result;
        }

        //View Report List CMK By Params
        public static List<bl_cmk_policy_report> GetPolicyReportListByParams(DateTime from_date, DateTime to_date, int search_by, int policy_status)
        {
            List<bl_cmk_policy_report> policy_report_list = new List<bl_cmk_policy_report>();

            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GET_CMK_POLICY_REPORT";
                cmd.Parameters.AddWithValue("@FROM_DATE", from_date);
                cmd.Parameters.AddWithValue("@TO_DATE", to_date);
                cmd.Parameters.AddWithValue("@SEARCH_BY", search_by);
                cmd.Parameters.AddWithValue("@POLICY_STATUS", policy_status);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        bl_cmk_policy_report policy_report = new bl_cmk_policy_report();

                        policy_report.Branch = rdr.GetString(rdr.GetOrdinal("Branch"));
                        policy_report.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                        policy_report.CMK_Customer_ID = rdr.GetString(rdr.GetOrdinal("CMK_Customer_ID"));
                        policy_report.Loan_ID = rdr.GetString(rdr.GetOrdinal("Loan_ID"));
                        policy_report.Certificate_No = rdr.GetString(rdr.GetOrdinal("Certificate_No"));
                        policy_report.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                        policy_report.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                        policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        policy_report.Age = rdr.GetInt32(rdr.GetOrdinal("Age"));
                        if (rdr.GetInt32(rdr.GetOrdinal("Gender")) == 0)
                            policy_report.Gender = "F";
                        else
                            policy_report.Gender = "M";
                            
                        policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Loan_Type"));
                        policy_report.Opened_Date = rdr.GetDateTime(rdr.GetOrdinal("Opened_Date"));
                        policy_report.LoanDuration = rdr.GetInt32(rdr.GetOrdinal("Loan_Duration"));

                        if (Helper.GetCurrancy(rdr.GetString(rdr.GetOrdinal("Currancy"))) != 0 )
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        }
                        else
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount"));
                        }

                        policy_report.Loan_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                        policy_report.Outstanding_Balance_Riel = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                        policy_report.Date_Of_Entry = rdr.GetDateTime(rdr.GetOrdinal("Date_Of_Entry"));
                        policy_report.Covered_Year = rdr.GetInt32(rdr.GetOrdinal("Covered_Year"));
                        policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                        policy_report.Policy_Status = rdr.GetString(rdr.GetOrdinal("Policy_Status"));
                        policy_report.Assured_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        policy_report.Currancy = rdr.GetString(rdr.GetOrdinal("Currancy"));
                        policy_report.Group = rdr.GetString(rdr.GetOrdinal("Group"));
                        policy_report.Pay_Mode_ID = rdr.GetString(rdr.GetOrdinal("Mode"));
                        policy_report.Monthly_Premium = rdr.GetDouble(rdr.GetOrdinal("Monthly_Premium"));
                        policy_report.Premium_After_Discount = rdr.GetDouble(rdr.GetOrdinal("Premium_After_Discount"));
                        policy_report.Extra_Premium = rdr.GetDouble(rdr.GetOrdinal("Extra_Premium"));
                        policy_report.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                        policy_report.Paid_Off_In_Month = rdr.GetDateTime(rdr.GetOrdinal("Paid_Off_In_Month"));
                        policy_report.Report_Date = rdr.GetDateTime(rdr.GetOrdinal("Report_Date"));
                        policy_report.Terminate_Date = rdr.GetDateTime(rdr.GetOrdinal("Terminate_Date"));
                        policy_report.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Noted"));

                        policy_report_list.Add(policy_report);
                    }
                }/// End loop
            }/// End of ConnectionString
             /// 
            return policy_report_list;
        }

        //View Report List CMK By Params
        public static List<bl_cmk_policy_report> GetPolicyReportListInMonth(DateTime from_date, DateTime to_date, int policy_status)
        {
            List<bl_cmk_policy_report> policy_report_list = new List<bl_cmk_policy_report>();

            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GET_CMK_POLICY_PREMIUM_IN_MONTH";
                cmd.Parameters.AddWithValue("@FROM_DATE", from_date);
                cmd.Parameters.AddWithValue("@TO_DATE", to_date);
                cmd.Parameters.AddWithValue("@POLICY_STATUS", policy_status);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        bl_cmk_policy_report policy_report = new bl_cmk_policy_report();

                        policy_report.Branch = rdr.GetString(rdr.GetOrdinal("Branch"));
                        policy_report.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                        policy_report.CMK_Customer_ID = rdr.GetString(rdr.GetOrdinal("CMK_Customer_ID"));
                        policy_report.Loan_ID = rdr.GetString(rdr.GetOrdinal("Loan_ID"));
                        policy_report.Certificate_No = rdr.GetString(rdr.GetOrdinal("Certificate_No"));
                        policy_report.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                        policy_report.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                        policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        policy_report.Age = rdr.GetInt32(rdr.GetOrdinal("Age"));
                        if (rdr.GetInt32(rdr.GetOrdinal("Gender")) == 0)
                            policy_report.Gender = "F";
                        else
                            policy_report.Gender = "M";
                            
                        policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Loan_Type"));
                        policy_report.Opened_Date = rdr.GetDateTime(rdr.GetOrdinal("Opened_Date"));
                        policy_report.LoanDuration = rdr.GetInt32(rdr.GetOrdinal("Loan_Duration"));

                        if (Helper.GetCurrancy(rdr.GetString(rdr.GetOrdinal("Currancy"))) != 0 )
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        }
                        else
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount"));
                        }

                        policy_report.Loan_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                        policy_report.Outstanding_Balance_Riel = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                        policy_report.Date_Of_Entry = rdr.GetDateTime(rdr.GetOrdinal("Date_Of_Entry"));
                        policy_report.Covered_Year = rdr.GetInt32(rdr.GetOrdinal("Covered_Year"));
                        policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                        policy_report.Policy_Status = rdr.GetString(rdr.GetOrdinal("Policy_Status"));
                        policy_report.Assured_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        policy_report.Currancy = rdr.GetString(rdr.GetOrdinal("Currancy"));
                        policy_report.Group = rdr.GetString(rdr.GetOrdinal("Group"));
                        policy_report.Pay_Mode_ID = rdr.GetString(rdr.GetOrdinal("Mode"));
                        policy_report.Monthly_Premium = rdr.GetDouble(rdr.GetOrdinal("Monthly_Premium"));
                        policy_report.Premium_After_Discount = rdr.GetDouble(rdr.GetOrdinal("Premium_After_Discount"));
                        policy_report.Extra_Premium = rdr.GetDouble(rdr.GetOrdinal("Extra_Premium"));
                        policy_report.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                        policy_report.Paid_Off_In_Month = rdr.GetDateTime(rdr.GetOrdinal("Paid_Off_In_Month"));
                        policy_report.Report_Date = rdr.GetDateTime(rdr.GetOrdinal("Report_Date"));
                        policy_report.Terminate_Date = rdr.GetDateTime(rdr.GetOrdinal("Terminate_Date"));
                        policy_report.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Noted"));

                        policy_report_list.Add(policy_report);
                    }
                }/// End loop
            }/// End of ConnectionString
             /// 
            return policy_report_list;
        }
        

        //View Report List  By Params
        public static List<bl_cmk_policy_report> GetPolicyLastReportByParams(DateTime from_date, DateTime to_date, int policy_status)
        {
            List<bl_cmk_policy_report> policy_report_list = new List<bl_cmk_policy_report>();

            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GET_CMK_POLICY_PREMIUM_LAST_REPORT";
                cmd.Parameters.AddWithValue("@FROM_DATE", from_date);
                cmd.Parameters.AddWithValue("@TO_DATE", to_date);
                cmd.Parameters.AddWithValue("@POLICY_STATUS", policy_status);

                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        bl_cmk_policy_report policy_report = new bl_cmk_policy_report();

                        policy_report.Branch = rdr.GetString(rdr.GetOrdinal("Branch"));
                        policy_report.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                        policy_report.CMK_Customer_ID = rdr.GetString(rdr.GetOrdinal("CMK_Customer_ID"));
                        policy_report.Loan_ID = rdr.GetString(rdr.GetOrdinal("Loan_ID"));
                        policy_report.Certificate_No = rdr.GetString(rdr.GetOrdinal("Certificate_No"));
                        policy_report.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                        policy_report.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                        policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        policy_report.Age = rdr.GetInt32(rdr.GetOrdinal("Age"));
                        if (rdr.GetInt32(rdr.GetOrdinal("Gender")) == 0)
                            policy_report.Gender = "F";
                        else
                            policy_report.Gender = "M";

                        if (Helper.GetCurrancy(rdr.GetString(rdr.GetOrdinal("Currancy"))) != 0)
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        }
                        else
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount"));
                        }

                        policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Loan_Type"));
                        policy_report.Opened_Date = rdr.GetDateTime(rdr.GetOrdinal("Opened_Date"));
                        policy_report.LoanDuration = rdr.GetInt32(rdr.GetOrdinal("Loan_Duration"));
                        policy_report.Loan_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                        policy_report.Outstanding_Balance_Riel = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                        policy_report.Date_Of_Entry = rdr.GetDateTime(rdr.GetOrdinal("Date_Of_Entry"));
                        policy_report.Covered_Year = rdr.GetInt32(rdr.GetOrdinal("Covered_Year"));
                        policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                        policy_report.Policy_Status = rdr.GetString(rdr.GetOrdinal("Policy_Status"));
                        policy_report.Assured_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        policy_report.Currancy = rdr.GetString(rdr.GetOrdinal("Currancy"));
                        policy_report.Group = rdr.GetString(rdr.GetOrdinal("Group"));
                        policy_report.Pay_Mode_ID = rdr.GetString(rdr.GetOrdinal("Mode"));
                        policy_report.Monthly_Premium = rdr.GetDouble(rdr.GetOrdinal("Monthly_Premium"));
                        policy_report.Premium_After_Discount = rdr.GetDouble(rdr.GetOrdinal("Premium_After_Discount"));
                        policy_report.Extra_Premium = rdr.GetDouble(rdr.GetOrdinal("Extra_Premium"));
                        policy_report.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                        policy_report.Paid_Off_In_Month = rdr.GetDateTime(rdr.GetOrdinal("Paid_Off_In_Month"));
                        policy_report.Report_Date = rdr.GetDateTime(rdr.GetOrdinal("Report_Date"));
                        policy_report.Terminate_Date = rdr.GetDateTime(rdr.GetOrdinal("Terminate_Date"));
                        policy_report.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Noted"));

                        policy_report_list.Add(policy_report);
                    }
                }/// End loop
            }/// End of ConnectionString
            /// 
            return policy_report_list;
        }


        //View Report List By Params
        public static List<bl_cmk_policy_report> GetPolicyReportListByParams(string certificate_number_search = "", string last_name_search = "", string first_name_search = "")
        {
            List<bl_cmk_policy_report> policy_report_list = new List<bl_cmk_policy_report>();

            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GET_CMK_POLICY_REPORT_BY_PARAMS";
                cmd.Parameters.AddWithValue("@CERTIFICATE_NUMBER_SEARCH", certificate_number_search);
                cmd.Parameters.AddWithValue("@FIRST_NAME_SEARCH", first_name_search);
                cmd.Parameters.AddWithValue("@LAST_NAME_SEARCH", last_name_search);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        bl_cmk_policy_report policy_report = new bl_cmk_policy_report();

                        policy_report.Branch = rdr.GetString(rdr.GetOrdinal("Branch"));
                        policy_report.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                        policy_report.CMK_Customer_ID = rdr.GetString(rdr.GetOrdinal("CMK_Customer_ID"));
                        policy_report.Loan_ID = rdr.GetString(rdr.GetOrdinal("Loan_ID"));
                        policy_report.Certificate_No = rdr.GetString(rdr.GetOrdinal("Certificate_No"));
                        policy_report.Last_Name = rdr.GetString(rdr.GetOrdinal("Last_Name"));
                        policy_report.First_Name = rdr.GetString(rdr.GetOrdinal("First_Name"));
                        policy_report.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        policy_report.Age = rdr.GetInt32(rdr.GetOrdinal("Age"));
                        if (rdr.GetInt32(rdr.GetOrdinal("Gender")) == 0)
                            policy_report.Gender = "F";
                        else
                            policy_report.Gender = "M";

                        if (Helper.GetCurrancy(rdr.GetString(rdr.GetOrdinal("Currancy"))) != 0)
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        }
                        else
                        {
                            policy_report.Loan_Amount = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount"));
                            policy_report.Outstanding_Balance = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance"));
                            policy_report.Assured_Amount = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount"));
                        }

                        policy_report.Product_ID = rdr.GetString(rdr.GetOrdinal("Loan_Type"));
                        policy_report.Opened_Date = rdr.GetDateTime(rdr.GetOrdinal("Opened_Date"));
                        policy_report.LoanDuration = rdr.GetInt32(rdr.GetOrdinal("Loan_Duration"));
                        policy_report.Loan_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Loan_Amount_Riel"));
                        policy_report.Outstanding_Balance_Riel = rdr.GetDouble(rdr.GetOrdinal("Outstanding_Balance_Riel"));
                        policy_report.Date_Of_Entry = rdr.GetDateTime(rdr.GetOrdinal("Date_Of_Entry"));
                        policy_report.Covered_Year = rdr.GetInt32(rdr.GetOrdinal("Covered_Year"));
                        policy_report.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));
                        policy_report.Policy_Status = rdr.GetString(rdr.GetOrdinal("Policy_Status"));
                        policy_report.Assured_Amount_Riel = rdr.GetDouble(rdr.GetOrdinal("Assured_Amount_Riel"));
                        policy_report.Currancy = rdr.GetString(rdr.GetOrdinal("Currancy"));
                        policy_report.Group = rdr.GetString(rdr.GetOrdinal("Group"));
                        policy_report.Pay_Mode_ID = rdr.GetString(rdr.GetOrdinal("Mode"));
                        policy_report.Monthly_Premium = rdr.GetDouble(rdr.GetOrdinal("Monthly_Premium"));
                        policy_report.Premium_After_Discount = rdr.GetDouble(rdr.GetOrdinal("Premium_After_Discount"));
                        policy_report.Extra_Premium = rdr.GetDouble(rdr.GetOrdinal("Extra_Premium"));
                        policy_report.Total_Premium = rdr.GetDouble(rdr.GetOrdinal("Total_Premium"));
                        policy_report.Paid_Off_In_Month = rdr.GetDateTime(rdr.GetOrdinal("Paid_Off_In_Month"));
                        policy_report.Report_Date = rdr.GetDateTime(rdr.GetOrdinal("Report_Date"));
                        policy_report.Terminate_Date = rdr.GetDateTime(rdr.GetOrdinal("Terminate_Date"));
                        policy_report.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Noted"));

                        policy_report_list.Add(policy_report);
                    }
                }/// End loop
            }/// End of ConnectionString
            /// 
            return policy_report_list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">[INDEX,0]="FEILD_NAME", [INDEX,1]="VALUE"</param>
        /// <returns></returns>
        public static DataTable GetPolicyReportListByUpload(List<string[, ,]> condition)
        {
            DataTable tbl_result = new DataTable();

            string my_condition = "";

            try
            {
                foreach (string[, ,] cond in condition)
                {
                    for (int index = 0; index <= cond.GetUpperBound(0); index++)
                    {
                        /*check operator "IN" */
                        if (cond[index, 0, 1].Trim().ToUpper() == "IN")
                        {
                            /*Make up sql format*/
                            cond[index, 0, 2] = " ('" + cond[index, 0, 2].Trim().Replace(",", "','") + "') ";
                        }
                        else if (cond[index, 0, 1].Trim().ToUpper() == "LIKE")
                        {
                            /*Make up sql format*/
                            cond[index, 0, 2] = " '%" + cond[index, 0, 2].Trim() + "%' ";
                        }
                        else
                        {
                            /*Make up sql format*/
                            cond[index, 0, 2] = " '" + cond[index, 0, 2] + "' ";
                        }
                        cond[index, 0, 1] = " " + cond[index, 0, 1] + " ";
                        my_condition += my_condition.Trim() == "" ? " ) AND " + cond[index, 0, 0] + cond[index, 0, 1] + cond[index, 0, 2] : " AND " + cond[index, 0, 0] + cond[index, 0, 1] + cond[index, 0, 2];
                    }
                }

                tbl_result = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_SEARCH_UPLOAD_CMK_POLICY_REPORT", new string[,] { { "@CONDITION", my_condition } }, "da_cmk => GetPolicyReportListByUpload(List<string[,,]> condition)");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetPolicyReportListByUpload(string[,] condition)] in call [da_cmk], detail:" + ex.Message);
                tbl_result = new DataTable();
            }
            return tbl_result;
        }

        public static DataTable GetPolicyByPolicyID(string policy_id)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_GET_CMK_POLICY_BY_POLICY_ID", new string[,] { { "@Policy_ID", policy_id } }, "da_cmk => GetPolicyProductID(string Policy_ID)");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetPolicyProductID(string Policy_ID)] in call [da_cmk], detail:" + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pay_mode"></param>
        /// <param name="Effective_Date"></param>
        /// <param name="Report_Date"></param>
        /// <returns></returns>
        public static string GetCustomerIDByCertificateNumber(string cmk_customer_id, string certificate_number)
        {
            string customer_id = "";
            string connString = AppConfiguration.GetConnectionString();
            try
            {
                using (SqlConnection myConnection = new SqlConnection())
                {
                    string sql = @"SELECT CUSTOMER_ID FROM Ct_CMK_Policy WHERE CMK_Customer_ID = @CMK_CUSTOMER_ID AND Certificate_No = @CERTIFICATE_NO ";
                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandText = sql;

                    myCommand.Parameters.AddWithValue("@CMK_CUSTOMER_ID", cmk_customer_id);
                    myCommand.Parameters.AddWithValue("@CERTIFICATE_NO", certificate_number);

                    customer_id = myCommand.ExecuteScalar().ToString();
                    myConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetCustomerIDByCertificateNumber(string cmk_customer_id, string certificate_number)] in call [da_cmk], detail:" + ex.Message);
                customer_id = "";
            }
            return customer_id;
        
        }
        
        

        public static string CalPremYearLot(int pay_mode, DateTime Effective_Date, DateTime Report_Date)
        {
            int total_month_all = 12;
            int prem_year = 0;
            int prem_lot = 0;
            int month_cal = 0;

            prem_year = Math.Abs((Report_Date.Year - Effective_Date.Year)) + 1;

            month_cal = (Report_Date.Month - Effective_Date.Month);

            if (pay_mode == 1)
            {
                prem_lot = 1;
            }
            else if (pay_mode == 2)
            {
                prem_lot = 1;
            }
            else if (pay_mode == 3)
            {
                prem_lot = 1;
            }
            else if (pay_mode == 4)
            {
                if (month_cal > 0)
                {
                    for (int i = 1; i <= month_cal; i++)
                    {
                        prem_lot += 1;
                    }
                }
                else
                {
                    prem_year -= 1;

                    prem_lot = (total_month_all - Math.Abs(month_cal));
                }
                
            }
            return prem_year.ToString() + "," + prem_lot.ToString();
        }

        public static bl_cmk.bl_cmk_total_policy GetSumPolicyPremium(DateTime report_date)
        {
            bl_cmk.bl_cmk_total_policy sum_prem = new bl_cmk.bl_cmk_total_policy();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CMK_SUM_PREMIUM_BY_REPORT_DATE", new string[,] { 
                {"@REPORT_DATE", report_date +""}
                }, "Function [GetSumPolicyPremium(DateTime report_date), class [da_cmk=>Policy]");
                if (tbl.Rows.Count > 0)
                {
                    var row = tbl.Rows[0];
                    sum_prem.Number_Of_Policy = Convert.ToInt32(row["Number_Of_Policy"]);
                    sum_prem.Total_Sum_Insure = Convert.ToDouble(row["Total_Sum_Insure"]);
                    sum_prem.Total_Premium = Convert.ToDouble(row["Total_Premium"]);
                }
            }
            catch (Exception ex)
            {
                sum_prem = new bl_cmk.bl_cmk_total_policy();//return empty object
                Log.AddExceptionToLog("Error function [GetSumPolicyPremium(DateTime report_date)] in class [da_cmk=>Policy], detail: " + ex.Message);
            }
            return sum_prem;

        }

        public static string GetGroupPremiumReport(DateTime report_date) 
        {
            string cmk_group_premium_id = "";

            string connString = AppConfiguration.GetConnectionString();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {
                    string sql = @"SELECT CMK_Group_Policy_ID FROM Ct_CMK_Group_Premium  WHERE Report_Date = @REPORT_DATE ORDER BY Report_Date desc ";

                    SqlCommand myCommand = new SqlCommand();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandText = sql;

                    myCommand.Parameters.AddWithValue("@REPORT_DATE", report_date);

                    cmk_group_premium_id = myCommand.ExecuteScalar().ToString();

                    myConnection.Close();
                }
            }
            catch (Exception ex)
            {
                cmk_group_premium_id = "";
                Log.AddExceptionToLog("Error function [GetGroupPremiumReport(DateTime Report_Date)] in class [da_cmk], detail: " + ex.Message);
            }

            return cmk_group_premium_id;

        }
        public static List<bl_cmk.CMK_Group_Premium> GetRenewalPremiumList(DateTime From_Date, DateTime To_Date)
        {
            List<bl_cmk.CMK_Group_Premium> prem_list = new List<bl_cmk.CMK_Group_Premium>();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CMK_RENEWAL_PREM", new string[,] { 
                {"@FROM_DATE", From_Date+""},
                {"@TO_DATE", To_Date+""}
                }, "Function [GetRenewalPremiumList(DateTime Report_Date), class [da_cmk=>Policy]");

                foreach (DataRow row in tbl.Rows)
                {
                    bl_cmk.CMK_Group_Premium premium = new bl_cmk.CMK_Group_Premium();

                    premium.CMK_Group_Policy_ID = row["CMK_Group_Policy_ID"].ToString();
                    premium.Group_Code = row["Group_Code"].ToString();
                    premium.Product_ID = row["Product_ID"].ToString();
                    premium.Product_Name = row["Product_Name"].ToString();
                    premium.Effective_Date = Convert.ToDateTime(row["Effective_Date"].ToString());
                    premium.Sum_Insure = Convert.ToDouble(row["Sum_Insure"].ToString());
                    premium.Amount = Convert.ToDouble(row["Amount"].ToString());
                    premium.Invoice_No = row["Invoice_No"].ToString();
                    premium.Pay_Year = Convert.ToInt32(row["Pay_Year"].ToString());
                    premium.Pay_Lot = Convert.ToInt32(row["Pay_Lot"].ToString());
                    premium.Mode = row["Mode"].ToString();
                    premium.Report_Date = Convert.ToDateTime(row["Report_Date"].ToString());
                    premium.Number_Of_Policy = Convert.ToInt32(row["Number_Of_Policy"].ToString());
                    premium.Created_By = row["Created_By"].ToString();
                    premium.Created_On = Convert.ToDateTime(row["Created_On"].ToString());

                    prem_list.Add(premium);
                }

            }
            catch (Exception ex)
            {
                prem_list = new List<bl_cmk.CMK_Group_Premium>(); //return empty object
                Log.AddExceptionToLog("Error function [GetRenewalPremiumList(DateTime Report_Date)] in class [da_cmk=>Policy], detail: " + ex.Message);
            }
            return prem_list;
        }

        /// <summary>
        /// This function will delete data from table [ct_Customer,ct_cmk_policy, ct_cmk_policy_premium, ct_policy_status, ct_policy_pay_mode]
        /// </summary>
        /// <param name="policy_id"></param>
        /// <returns></returns>
        public static bool RollBack(string policy_id, string customer_id, DateTime rollback_report_date)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_CMK_ROLLBACK", new string[,] { 
            {"@CMK_POLICY_ID", policy_id},
            {"@CUSTOMER_ID", customer_id},
            {"@REPORT_DATE", rollback_report_date +""},
            }, "Function [RollBack(string policy_id, string customer_id, DateTime rollback_report_date)], class [da_cmk]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [RollBack(string policy_id, string customer_id, DateTime rollback_report_date)] in class [da_cmk], detail: " + ex.Message);
            }
            return result;
        }

        public static bool DeletePremiumReport(DateTime Report_Date)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_CMK_PREMIUM_REPORT", new string[,] { 
                {"@REPORT_DATE", Report_Date +""},
                }, "Function [DeletePremiumReport(DateTime Report_Date)], class [da_cmk]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [DeletePremiumReport(DateTime Report_Date)] in class [da_cmk], detail: " + ex.Message);
            }
            return result;
        }
        
    }

}