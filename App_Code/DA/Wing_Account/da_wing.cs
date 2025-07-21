using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_wing
/// </summary>
public class da_wing
{
	public da_wing()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// Get premium: [0]=Premium by mode, [1]=Original prmeium or Annual premium
    /// </summary>
    /// <param name="product_id">Product ID</param>
    /// <param name="sum_assured">Sum Assured</param>
    /// <param name="pay_mode">[1:Annual, 2:Semi-Annaul, 3:Quarter, 4:Monthly]</param>
    /// <returns></returns>
    public static double[] GetPremium(string product_id, int sum_assured, int pay_mode)
    {
        double[] premium = new double[]{ 0.0,0.0};
        try
        {
            double prem_by_mode = 0.0;
            double prem_annual = 0.0;
            if (pay_mode == 4)//Monthly
            {
                prem_by_mode = (sum_assured * 0.25) / 1000;
                prem_annual = prem_by_mode * 12;
                premium = new double[] { prem_by_mode, prem_annual };
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPremium(string product_id, int sum_assured, int pay_mode)] in class [da_wing.cs], detail: " + ex.Message);
        }
        return premium;
    }
    /// <summary>
    /// Get last sequence number from table CT_CI_POLICY_DETAIL
    /// </summary>
    /// <param name="policy_number"></param>
    /// <returns></returns>
    public static Int32 GetLastSEQ(string policy_number)
    {
        Int32 last_seq = 0;
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_WING_LAST_SEQ", new string[,] { { "@POLICY_NUMBER", policy_number } }, "Call [da_wing] =>GetLastSEQ(string policy_number)");
            if (tbl.Rows.Count > 0)
            {
                last_seq = Convert.ToInt32(tbl.Rows[0][0].ToString());
            }
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetLastSEQ(string policy_number) in class [da_wing.cs], detail: " + ex.Message);
        }
        return last_seq;
    }

    public static bool InsertIntoPaymentUploading(string policy_id, string customer_id, string transaction_id, double received_amount, double prem_amount, double left_over, double interest_amount, DateTime received_date,
           string Sale_Agent_ID, string Office_ID, int Pay_Mode_ID, double Discount_Amount, double Prem_SI, string policy_status, string product_id, int period_payment, string policy_number)
    {
        bool result = false;

        try
        {
             string sql = @"Insert into Ct_Payment_Uploading (Policy_ID, Customer_ID, Transaction_ID, Received_Amount,Prem_Amount, 
                                                Interest_Amount, Left_Over_Amount, Received_Date,Sale_Agent_ID,Office_ID,Pay_Mode_ID,Discount_Amount,Prem_EM_Amount,Prem_Original_Amount,Prem_SI,Policy_Status,Product_ID,Period_Payment,Policy_Number)
                                        Values(@Policy_ID, @Customer_ID, @Transaction_ID, @Received_Amount, @Prem_Amount, 
                                                @Interest_Amount, @Left_Over_Amount, @Received_Date,@Sale_Agent_ID, @Office_ID, @Pay_Mode_ID, @Discount_Amount, @Prem_EM_Amount, @Prem_Original_Amount, @Prem_SI,@Policy_Status,@Product_ID,@Period_Payment,@Policy_Number)";

            result = Helper.ExecuteCommand(AppConfiguration.GetConnectionString(),sql, new string[,] {
            {"@Policy_ID", policy_id},
            {"@Customer_ID", customer_id},
            {"@Transaction_ID", transaction_id},
            {"@Received_Amount", received_amount+""},
            {"@Prem_Amount", prem_amount+""},
            {"@Interest_Amount", interest_amount+""},
            {"@Left_Over_Amount", left_over+""},
            {"@Received_Date", received_date+""},
            {"@Sale_Agent_ID", Sale_Agent_ID},
            {"@Office_ID", Office_ID},
            {"@Pay_Mode_ID", Pay_Mode_ID+""},
            {"@Discount_Amount", Discount_Amount+""},
            {"@Prem_EM_Amount", "0"},
            {"@Prem_Original_Amount", "0"},
            {"@Prem_SI", Prem_SI+""},
            {"@Policy_Status", policy_status},
            {"@Product_ID", product_id},
            {"@Period_Payment", period_payment+""},
            {"@Policy_Number", policy_number}
             }, "Function [InsertIntoPaymentUploading] in class [da_wing]");
        }
        catch (Exception ex)
        {
            result = false;
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertIntoPaymentUploading] in class [da_wing]. Details: " + ex.Message);
        }

        return result;
    }
    public static bool InsertIntoPaymentWing(bl_payment_wing payment_wing)
    {
        bool result = false;

        try
        {

            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Insert_Payment_Wing", new string[,] { 
                {"@Payment_Wing_ID", payment_wing.Payment_Wing_ID},
                {"@Policy_ID", payment_wing.Policy_ID},
                {"@Bill_No", payment_wing.Bill_No},
                {"@Transaction_ID", payment_wing.Transaction_ID},
                {"@Received_Amount", payment_wing.Received_Amount+""},
                {"@Received_Date", payment_wing.Received_Date+""},
                {"@Created_On", payment_wing.Created_On+""},
                {"@Created_By", payment_wing.Created_By},
                {"@Status_Paid", payment_wing.Status_Paid+""},
                {"@Check_Status", payment_wing.Check_Status+""},
                {"@Transaction_Type", payment_wing.Transaction_Type+""},
                {"@Created_Note", payment_wing.Created_Note}

                }, "Function [InsertIntoPaymentWing(bl_payment_wing payment_wing)] class [da_wing]");
        }
        catch (Exception ex)
        {
            result = false;
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertIntoPaymentWing] in class [da_wing]. Details: " + ex.Message);
        }

        return result;
    }
    public static bool UpdatePaymentWingByTransactionID(string transaction_id, double returrn_amount, string policy_id, string official_receipt_id, double left_over_amount)
    {
        bool result = false;

        try
        {
           

           string sql = @"update Ct_Payment_Wing set Status_Paid=1, Return_Amount=@Return_Amount, Policy_ID=@Policy_ID,
                                    Official_Receipt_ID=@Official_Receipt_ID, Left_Over_Amount=@Left_Over_Amount where Transaction_ID=@Transaction_ID";
            result=Helper.ExecuteCommand(AppConfiguration.GetConnectionString(),sql, new string[,]{
            {"@Transaction_ID", transaction_id},
            {"@Return_Amount", returrn_amount+""},
            {"@Policy_ID", policy_id},
            {"@Official_Receipt_ID", official_receipt_id},
            {"@Left_Over_Amount", left_over_amount+""}
        },"function [UpdatePaymentWingByTransactionID] class [da_wing]");
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [UpdatePaymentWingByTransactionID] in class [da_wing]. Details: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Delete all record in table Ct_Payment_Uploading
    /// </summary>
    /// <returns></returns>
    public static bool TruncatePaymentUploading()
    {
        bool result = false;

        try
        {

            result = Helper.ExecuteCommand(AppConfiguration.GetConnectionString(), "Truncate Table Ct_Payment_Uploading", new string[,] { }, "Function [TruncatePaymentUploading()] class [da_wing]");
        }
        catch (Exception ex)
        {
            result = false;
            //Add error to log 
            Log.AddExceptionToLog("Error in function [TruncatePaymentUploading] in class [da_wing]. Details: " + ex.Message);
        }

        return result;
    }
    public class Policy
    {
        public Policy() { }
        public static bool SavePolicy(bl_wing.policy policy)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_WING_CI_POLICY", new string[,] { 
                {"@POLICY_ID",policy.PolicyID},
                {"@POLICY_NUMBER", policy.PolicyNumber},
                {"@CUSTOMER_ID", policy.CustomerID},
                {"@PRODUCT_ID", policy.ProductID},
                {"@ADDRESS_ID", policy.AddressID},
                {"@AGENT_CODE",policy.AgentCode},
                {"@CREATED_BY",policy.CreatedBy},
                {"@CREATED_DATETIME",policy.CreatedDateTime +""},
                {"@CONSENT_NUMBER",policy.ConsentNumber},
                {"@CONSENT_FORM",policy.ConsentForm},
                {"@CATEGORIES",policy.Categories},
                {"@FACTORY_NAME", policy.FactoryName},
                {"@REMARKS", policy.Remarks}
            }, "Function [SavePolicy(bl_wing.policy policy)] in class [da_wing.cs => Policy]");
            }
            catch (Exception ex)
            {

            }

            return result;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="issued_date_f">[not searched by date please input: 1900-01-01]</param>
        /// <param name="issued_date_t">[not searched by date please input: 1900-01-01]</param>
       /// <param name="policy_number">[seach many policies please input: pol1,pol2, pol3]</param>
        /// <param name="policy_status">[To Skip this condition input blank value]</param> 
       /// <returns></returns>
        public static DataTable GetWingPolicy(DateTime issued_date_f, DateTime issued_date_t, string policy_number, string policy_status)
        {
            DataTable tbl = new DataTable();
            try
            {
                tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_WING_UPLOADED_POLICY", new string[,] {
                {"@issued_date_f", issued_date_f+""}, 
                {"@issued_date_t", issued_date_t+""},
                {"@policy_number", policy_number},
                 {"@policy_status", policy_status}
                }, "Function [GetWingPolicy(DateTime issued_date_f, DateTime issured_date_t, string policy_number)] in class [da_wing => policy]");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetWingPolicy(DateTime issued_date_f, DateTime issured_date_t, string policy_number)] in class [da_wing => policy], detail:" + ex.Message);
                tbl = new DataTable();
            }
            return tbl;
        }
        
        /// <summary>
        /// Pending policy means that it's waiting for approving consent form by wing
        /// </summary>
        /// <param name="policy_number"></param>
        /// <param name="policy_id">[out put string policy id]</param>
        /// <param name="policy_detail_id">[out put string policy detail id]</param>
        /// <returns></returns>
        public static bool isPending(string policy_number, out string policy_id, out string policy_detail_id)
        {
            bool result = false;
            policy_id = "";
            policy_detail_id = "";
            try
            {
                DataTable tbl = GetWingPolicy(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), policy_number, "PEN");
                if (tbl.Rows.Count > 0)
                {
                    result = true;
                    policy_id = tbl.Rows[0]["POLICY_ID"].ToString().Trim();
                    policy_detail_id = tbl.Rows[0]["POLICY_DETAIL_ID"].ToString().Trim();
                }
                else
                {
                    result = false;
                    policy_id = "";
                    policy_detail_id = "";
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [isPending(string policy_number, out string policy_id, out string policy_detail_id)] in class [da_wing=>policy], detail:" + ex.Message);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy_number"></param>
        /// <param name="policy_id">Out put policy id</param>
        /// <param name="policy_detail_id">Out put policy detail id</param>
        /// <returns></returns>
        public static bool isNew(string policy_number, out string policy_id, out string policy_detail_id)
        {
            bool result = false;
            policy_id = "";
            policy_detail_id = "";
            try
            {
                DataTable tbl = GetWingPolicy(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), policy_number, "NEW");
                if (tbl.Rows.Count > 0)
                {
                    result = true;
                    policy_id = tbl.Rows[0]["POLICY_ID"].ToString().Trim();
                    policy_detail_id = tbl.Rows[0]["POLICY_DETAIL_ID"].ToString().Trim();
                }
                else
                {
                    result = false;
                    policy_id = "";
                    policy_detail_id = "";
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [isNew(string policy_number, out string policy_id, out string policy_detail_id)] in class [da_wing=>policy], detail:" + ex.Message);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Update policy remark by policy_id
        /// </summary>
        /// <param name="policy_id"></param>
        /// <param name="remarks"></param>
        /// <param name="updated_by"></param>
        /// <param name="updated_datetime"></param>
        /// <returns></returns>
        public static bool UpdatePolicyRemarks(string policy_id, string remarks, string updated_by, DateTime updated_datetime)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CI_WING_POLICY_REMARKS", new string[,] { 
                {"@POLICY_ID", policy_id},
                {"@REMARKS", remarks},
                {"@UPDATED_BY", updated_by},
                {"@UPDATED_DATETIME", updated_datetime+""}
                }, "Function [UpdatePolicyRemarks(string policy_id, string remarks, string updated_by, DateTime updated_datetime)] in class [bl_wing=>policy]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [UpdatePolicyRemarks(string policy_id, string remarks, string updated_by, DateTime updated_datetime)] in class [bl_wing=>policy], detail:" + ex.Message);
            }

            return result;
        }
        /// <summary>
        /// Get policy Info by policy id
        /// </summary>
        /// <param name="policy_id"></param>
        /// <returns></returns>
        public static bl_wing.policy.PolicyInfo GetPolicyInfo(string policy_id)
        {
            bl_wing.policy.PolicyInfo polInfo = new bl_wing.policy.PolicyInfo();
            try
            {
                
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetPolicyInfo(string policy_id)] in class [da_wing], detail:" + ex.Message);
                polInfo = new bl_wing.policy.PolicyInfo();
            }
            return polInfo;
        }

        public class PolicyInfo
        {
            public PolicyInfo() { }
            /// <summary>
            /// Get policy infomation by policy id
            /// </summary>
            /// <param name="policy_number"></param>
            /// <returns></returns>
            public static bl_wing.policy.PolicyInfo GetPolicyInfo(string policy_number)
            {
                bl_wing.policy.PolicyInfo info = new bl_wing.policy.PolicyInfo();
                try
                {
                    DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_WING_POLICY_INFO", new string[,] {
                    {"@Policy_Number",policy_number}
                    }, "Function [GetPolicyInfo(string policy_number) in class da_wing=>policy=>policyinfo]");
                    if (tbl.Rows.Count > 0)
                    {
                        var row = tbl.Rows[0];
                        info.PolicyID = row["policy_id"].ToString();
                        info.PolicyNumber = row["policy_number"].ToString();
                        info.CustomerID = row["customer_id"].ToString();
                        info.FirstNameEN = row["first_name"].ToString();
                        info.LastNameEn = row["last_name"].ToString();
                        info.FirstNameKH = row["khmer_first_name"].ToString();
                        info.LastnameKH = row["khmer_last_name"].ToString();
                        info.Gender = Helper.GetGenderText(Convert.ToInt32(row["gender"].ToString()), false);
                        info.DOB = Convert.ToDateTime(row["birth_date"].ToString());
                        info.IDType = Helper.GetIDCardTypeText(Convert.ToInt32(row["id_type"].ToString()));
                        info.IDNumber = row["id_card"].ToString();
                        info.PhoneNumber = row["mobile_phone"].ToString();
                        info.Address = row["address1"].ToString() + " " + row["Province"].ToString();
                        info.PaymentMode = row["mode"].ToString();
                        info.ProductID = row["product_id"].ToString();
                        info.Premium = Convert.ToDouble(row["premium"].ToString());
                        info.SumAssured = Convert.ToInt32(row["sum_assured"].ToString());
                        info.Originalpremium = Convert.ToDouble(row["original_premium"].ToString());
                        info.PaymentBy = row["payment_by"].ToString();
                        info.PaymentCode = row["payment_code"].ToString();
                        info.UserPremium = Convert.ToDouble(row["user_premium"].ToString());
                        info.PolicyStatus = row["policy_status_type_id"].ToString();
                        info.PolicyDetailID = row["policy_detail_id"].ToString();
                        info.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                        info.AgentCode = row["agent_code"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log.AddExceptionToLog("Error function [GetPolicyInfo(string policy_number)] in class [da_wing=>policy=>policyinfo], detail:" + ex.Message);
                    info = new bl_wing.policy.PolicyInfo();
                }
                return info;
            }
        }

        public static DataTable GetRenewPolicy(int year, int month)
        {
            DataTable tbl = new DataTable();
            DataTable tbl_final = new DataTable();
            try
            {
                tbl = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), "", "", "AL");
                if (tbl.Rows.Count > 0)
                {
                    // tbl_final = tbl.Clone();
                    //  DataRow new_row;
                    //filter year, month and policy status IF or Lap
                    tbl_final = tbl.AsEnumerable().Where(r => r.Field<DateTime>("expiry_date").Year == year && r.Field<DateTime>("expiry_date").Month == month && (r.Field<string>("policy_status") == "IF" || r.Field<string>("policy_status") == "LAP")).CopyToDataTable();
                    //foreach (DataRow row in tbl.Select("EXPIRY_DATE='2019-09-06 23:59:00.000' and policy_status in ('IF','LAP')"))
                    //{
                    //    new_row = tbl_final.NewRow();
                    //    foreach (DataColumn col in tbl.Columns)
                    //    {
                    //        new_row[col.ColumnName] = row[col.ColumnName];
                    //    }
                    //    tbl_final.Rows.Add(new_row);
                    //}
                }
                else
                {
                    tbl_final = new DataTable();
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetExpiredPolicy(int a)] in class [da_wing], detail:" + ex.Message);
                tbl_final = new DataTable();
            }
            return tbl_final;
        }
    }
    public class PolicyDetail
    {
        public PolicyDetail() { }

        public static bool SavePolicyDetail(bl_wing.PolicyDetail policy_detail)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_WING_CI_POLICY_DETAIL", new string[,] { 
                {"@POLICY_DETAIL_ID",policy_detail.PolicyDetailID},
                {"@POLICY_ID",policy_detail.PolicyID},
                {"@EFFECTIVE_DATE",policy_detail.EffectiveDate+""},
                {"@MATURITY_DATE",policy_detail.MaturityDate+""},
                {"@EXPIRY_DATE",policy_detail.ExpiryDate+""}, 
                {"@ISSUED_DATE", policy_detail.IssuedDate+""},
                {"@AGE", policy_detail.Age+""},
                {"@SUM_ASSURED",policy_detail.SumAssured+""},
                {"@PAYMODE_ID", policy_detail.PayModeID +""},
                {"@PAYMENT_CODE",policy_detail.PaymentCode},
                {"@PAYMENT_BY", policy_detail.PaymentBy},
                {"@USER_PREMIUM",policy_detail.UserPremium+""},
                {"@PREMIUM", policy_detail.Premium +""},
                {"@RETURN_PREMIUM",policy_detail.RETURN_PREMIUM+""},
                {"@ORGINAL_PREMIUM", policy_detail.OriginalPremium+""},
                {"@DISCOUNT_AMOUNT", policy_detail.DiscountAmount+""}, 
                {"@COVER_YEAR", policy_detail.CoverYear+""},
                {"@PAY_YEAR", policy_detail.PayYear+""},
                {"@COVER_UP_TO_AGE", policy_detail.CoverUpToAge+""},
                {"@PAY_UP_TO_AGE", policy_detail.PayUpToAge+""},
                {"@POLICY_STATUS_REMARKS", policy_detail.PolicyStatusRemarks},
                {"@CREATED_BY", policy_detail.CreatedBy},
                {"@CREATED_DATETIME", policy_detail.CreatedDateTime+""},
                {"@SEQ", policy_detail.Sequence+""},
                {"@TRANSACTION_ID", policy_detail.TransactionID}
                }, "Function [SavePolicyDetail(bl_wing.PolicyDetail policy_detail)] in class [da_wing.cs => PolicyDetail]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [SavePolicyDetail(bl_wing.PolicyDetail policy_detail)] in class [da_wing=>policy_detail], detail:" + ex.Message);
            }
            return result;
        }

        

        public static bool UpdatePolicyStatus(string policy_id, string policy_detail_id, string policy_status_code, string remarks, string transaction_id, string updated_by, DateTime updated_datetime)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CI_WING_POLICY_STATUS", new string[,] { 
                {"@policy_id", policy_id},
                {"@policy_detail_id", policy_detail_id},
                {"@policy_status", policy_status_code},
                {"@remarks", remarks},
                 {"@transaction_id", transaction_id},
                {"@updated_by", updated_by},
                {"@updated_datetime", updated_datetime+""}
                }, "Function [UpdatePolicyStatus(string policy_id, string policy_detail_id, string policy_status_code, string remarks, string transaction_id, string updated_by, DateTime updated_datetime)] in class [da_wing=>policy_detail]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [ UpdatePolicyStatus(string policy_id, string policy_detail_id, string policy_status_code, string remarks, string transaction_id, string updated_by, DateTime updated_datetime)] in class [da_wing=>policy_detail], detail:" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy_detail">Policy detail object</param>
        /// <param name="policy_status_code">Status code [IF, Lap, ....]</param>
        /// <returns></returns>
        public static bool UpdatePolicyDetail(bl_wing.PolicyDetail policy_detail, string policy_status_code)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CI_WING_POLICY_DETAIL", new string[,] { 
                {"@POLICY_DETAIL_ID", policy_detail.PolicyDetailID}, 
                {"@POLICY_ID", policy_detail.PolicyID},
                {"@EFFECTIVE_DATE", policy_detail.EffectiveDate+""}, 
                {"@MATURITY_DATE", policy_detail.MaturityDate+""}, 
                {"@EXPIRY_DATE",policy_detail.ExpiryDate+""}, 
                {"@ISSUED_DATE",policy_detail.IssuedDate+""}, 
                {"@AGE",policy_detail.Age+""}, 
                {"@SUM_ASSURED", policy_detail.SumAssured+""},
                {"@PAYMODE_ID",policy_detail.PayModeID+""}, 
                {"@PAYMENT_CODE",policy_detail.PaymentCode},
                {"@PAYMENT_BY",policy_detail.PaymentBy}, 
                {"@USER_PREMIUM", policy_detail.UserPremium+""}, 
                {"@PREMIUM",policy_detail.Premium+""}, 
                {"@RETURN_PREMIUM",policy_detail.RETURN_PREMIUM+""},
                {"@ORIGINAL_PREMIUM",policy_detail.OriginalPremium+""}, 
                {"@DISCOUNT",policy_detail.DiscountAmount+""},
                {"@COVER_YEAR",policy_detail.CoverYear+""}, 
                {"@PAY_YEAR",policy_detail.PayYear+""},
                {"@COVER_UP_TO_AGE",policy_detail.CoverUpToAge+""},
                {"@PAY_UP_TO_AGE",policy_detail.PayUpToAge+""},
                {"@POLICY_STATUS_CODE",policy_status_code}, 
                {"@POLICY_STATUS_REMARKS",policy_detail.PolicyStatusRemarks},
                {"@TRANSACTION_ID", policy_detail.TransactionID},
                {"@UPDATED_BY",policy_detail.UpdatedBy},
                {"@UPDATED_DATETIME",policy_detail.UpdatedDateTime+""},
                 {"@TRANSACTION_DATE",policy_detail.TransactionDate+""}
                }, "Function [UpdatePolicyDetail(bl_wing.PolicyDetail policy_detail, string policy_status_code)] in class [da_wing=>Policy_Detail]");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [UpdatePolicyDetail(bl_wing.PolicyDetail policy_detail, string policy_status_code)] in class [da_wing=>policy_detail], detail" + ex.Message);
                result = false;
            }
            return result;
        }
    }
}