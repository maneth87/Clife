using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_ci
/// </summary>
public class da_ci
{
	public da_ci()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Get premium: [0]=Premium by mode, [1]=Original prmeium or Annual premium
    /// </summary>
    /// <param name="sa">Sum Assured</param>
    /// <param name="product_id">Product id</param>
    /// <param name="pay_mode">Payment mode id</param>
    /// <returns></returns>
    public static double[] GetPremium(double sa, string product_id, int pay_mode)
    {
        double[] premium = new double[] { 0, 0};
        double premium_by_mode = 0.0;
        double premium_annual = 0.0;
        try
        {
            switch (product_id.Trim().ToLower())
            {
                case "ci":
                    premium_annual = (sa * 12) / 1000;
                    switch (pay_mode)
                    {
                        case 1://annual
                            premium_by_mode = premium_annual;
                            break;
                        case 2://semi
                            premium_by_mode = Math.Ceiling(premium_annual / 2);
                            break;
                        case 3://quarter
                            premium_by_mode = Math.Ceiling(premium_annual / 4);
                            break;
                        case 4://monthly
                            premium_by_mode = Math.Ceiling(premium_annual / 12);
                            break;
                    }
                    break;
                case "so":
                    premium_annual = (sa * 12) / 1000;
                    switch (pay_mode)
                    {
                        case 1://annual
                            premium_by_mode = premium_annual;
                            break;
                        case 2://semi
                            premium_by_mode = Math.Ceiling(premium_annual / 2);
                            break;
                        case 3://quarter
                            premium_by_mode = Math.Ceiling(premium_annual / 4);
                            break;
                        case 4://monthly
                            premium_by_mode = Math.Ceiling(premium_annual / 12);
                            break;
                    }
                    break;
            }
            premium = new double[] { premium_by_mode, premium_annual };
        }
        catch (Exception ex)
        {
            premium = new double[] { 0, 0 };
            Log.AddExceptionToLog("Error function [GetPremium(double sa, string product_id, int pay_mode)] in class [], detail:" + ex.Message);
        }
        return premium;
    }
    /// <summary>
    /// This function will delete data from table [ct_Customer,ct_ci_policy, ct_ci_policy_detail, ct_policy_address, ct_policy_contact, ct_policy_status, ct_policy_pay_mode, ct_policy_prem_pay]
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="policy_detail_id"></param>
    /// <param name="policy_prem_pay_id"></param>
    /// <param name="address_id"></param>
    /// <param name="customer_id"></param>
    /// <returns></returns>
    public static bool RollBack(string policy_id, string policy_detail_id, string policy_prem_pay_id, string address_id, string customer_id)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_CI_ROLLBACK", new string[,] { 
            {"@POLICY_ID", policy_id},
            {"@POLICY_DETAIL_ID", policy_detail_id},
            {"@POLICY_PREM_PAY_ID", policy_prem_pay_id},
            {"@ADDRESS_ID", address_id},
            {"@CUSTOMER_ID", customer_id}
            }, "Function [RollBack(string policy_id, string policy_detail_id, string policy_prem_pay, string address_id)], class [da_ci]");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [RollBack(string policy_id)] in class [da_ci], detail: " + ex.Message);
        }
        return result;
    }

    /// <summary>
    /// Return phone number from call center database
    /// </summary>
    /// <param name="phone_number">Phone number format:855</param>
    /// <returns></returns>
    public static string GetCellCardCustomer(string phone_number)
    {
        string phone_no = "";
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetCallCenterConnectionString(), "SP_GET_CELL_CARD_CUSTOMER_PHONE_NUMBER", new string[,] { 
            {"@PHONE_NUMBER", phone_number}
            }, "Function [GetCellCardCustomer(string phone_number)], class [da_ci]");
            if (tbl.Rows.Count > 0)
            {
                var row = tbl.Rows[0];
                phone_no = row["phone_number"].ToString();
            }
        }
        catch (Exception ex)
        {
            phone_no = "";
            Log.AddExceptionToLog("Error function [GetCellCardCustomer(string phone_number)] in class [da_ci], detail: " + ex.Message);
            
        }
        return phone_no;
    }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="from_date"></param>
   /// <param name="to_date"></param>
   /// <param name="customer_name"></param>
   /// <param name="policy_number"></param>
   /// <param name="product_id"></param>
   /// <returns></returns>
    public static DataTable GetPolicyCIDetailReport(DateTime from_date, DateTime to_date, string customer_name, string policy_number, string product_id)
    {
        DataTable tbl = new DataTable();
        try
        {
            tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_POLICY_DETAIL_REPORT", new string[,]{
            {"@FROM_DATE", from_date+""},
            {"@TO_DATE", to_date+""},
            {"@CUSTOMER_NAME" , customer_name.Trim().Replace(" ", "")},
            {"@POLICY_NUMBER", policy_number},
            {"@product_id", product_id}
            },"Function [GetPolicyCIDetailReport(DateTime from_date, DateTime to_date)] class [da_ci]");
        }
        catch (Exception ex)
        {
            tbl = new DataTable();
            Log.AddExceptionToLog("Error function [GetPolicyCIDetailReport(DateTime from_date, DateTime to_date)] in class [da_ci], detail:" + ex.Message);
        }
        return tbl;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from_date"></param>
    /// <param name="to_date"></param>
    /// <param name="customer_name"></param>
    /// <param name="policy_number"></param>
    /// <param name="product_id"></param>
    /// <returns></returns>
    public static DataTable GetPolicyPremiumDetailReport(DateTime from_date, DateTime to_date, string customer_name, string policy_number, string product_id)
    {
        DataTable tbl = new DataTable();
        try
        {
            tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_POLICY_PREMIUM_DETAIL_REPORT", new string[,]{
            {"@FROM_DATE", from_date+""},
            {"@TO_DATE", to_date+""},
            {"@CUSTOMER_NAME" , customer_name},
            {"@POLICY_NUMBER", policy_number},
            {"@product_id", product_id}
            }, "Function [GetPolicyPremiumDetailReport(DateTime from_date, DateTime to_date)] class [da_ci]");
        }
        catch (Exception ex)
        {
            tbl = new DataTable();
            Log.AddExceptionToLog("Error function [GetPolicyPremiumDetailReport(DateTime from_date, DateTime to_date)] in class [da_ci], detail:" + ex.Message);
        }
        return tbl;
    }
    public static DataTable GetTerminatedPolicy(DateTime from_date, DateTime to_date, string customer_name, string policy_number, string product_id)
    {
        DataTable tbl = new DataTable();
        try
        {
            tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_TERMINATED_POLICY_REPORT", new string[,]{
            {"@FROM_DATE", from_date+""},
            {"@TO_DATE", to_date+""},
            {"@CUSTOMER_NAME" , customer_name.Trim().Replace(" ", "")},
            {"@POLICY_NUMBER", policy_number},
            {"@product_id", product_id}
            }, "Function [GetTerminatedPolicy(DateTime from_date, DateTime to_date, string customer_name, string policy_number, string product_id)] class [da_ci]");
        }
        catch (Exception ex)
        {
            tbl = new DataTable();
            Log.AddExceptionToLog("Error function [GetTerminatedPolicy(DateTime from_date, DateTime to_date, string customer_name, string policy_number, string product_id)] in class [da_ci], detail:" + ex.Message);
        }
        return tbl;
    }
    public class Policy
    {
        public Policy() { }
        /// <summary>
        /// Get policy from table [ct_ci_policy] base on policy_id
        /// </summary>
        /// <param name="policy_id"></param>
        /// <returns></returns>
        public static bl_ci.Policy GetPolicy(string policy_id)
        {
            bl_ci.Policy policy = new bl_ci.Policy();
            try
            {
             DataTable tbl=   DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_POLICY_BY_POLICY_ID", new string[,] { 
                {"@POLICY_ID", policy_id}
                }, "Function [bl_ci.Policy GetPolicy(string policy_id)], class [da_ci=>Policy]");
             if (tbl.Rows.Count > 0)
             { 
                 var row = tbl.Rows[0];
                 policy.PolicyID = row["policy_id"].ToString();
                 policy.PolicyNumber = row["policy_number"].ToString();
                 policy.CustomerID = row["customer_id"].ToString();
                 policy.ProductID = row["product_id"].ToString();
                 policy.AddressID = row["address_id"].ToString();
                 policy.AgentCode = row["agent_code"].ToString();
                // policy.PaymentCode = row["payment_code"].ToString();
                 policy.CreatedBy = row["created_by"].ToString();
                 policy.CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString());

             }
            }
            catch (Exception ex)
            {
                policy = new bl_ci.Policy();//return empty object
                Log.AddExceptionToLog("Error function [bl_ci.Policy GetPolicy(string policy_id)] in class [da_ci=>Policy], detail: " + ex.Message);
            }
            return policy;
        }

        /// <summary>
        /// Get policy by policy number
        /// </summary>
        /// <param name="policy_number">Is customer's phone number, ex:[85512363466]</param>
        /// <returns></returns>
        public static bl_ci.Policy GetPolicyByPolicyNumber(string policy_number)
        {
            bl_ci.Policy policy = new bl_ci.Policy();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_POLICY_BY_POLICY_NUMBER", new string[,] { 
                {"@POLICY_NUMBER", policy_number}
                }, "Function [GetPolicyByPolicyNumber(string policy_number), class [da_ci=>Policy]");
                if (tbl.Rows.Count > 0)
                {
                    var row = tbl.Rows[0];
                    policy.PolicyID = row["policy_id"].ToString();
                    policy.PolicyNumber = row["policy_number"].ToString();
                    policy.CustomerID = row["customer_id"].ToString();
                    policy.ProductID = row["product_id"].ToString();
                    policy.AddressID = row["address_id"].ToString();
                    policy.AgentCode = row["agent_code"].ToString();
                    //policy.PaymentCode = row["payment_code"].ToString();
                    policy.CreatedBy = row["created_by"].ToString();
                    policy.CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString());

                }
            }
            catch (Exception ex)
            {
                policy = new bl_ci.Policy();//return empty object
                Log.AddExceptionToLog("Error function [GetPolicyByPolicyNumber(string policy_number)] in class [da_ci=>Policy], detail: " + ex.Message);
            }
            return policy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer_id"></param>
        /// <returns></returns>
        public static bl_ci.Policy GetPolicyByCustomerID(string customer_id)
        {
            bl_ci.Policy policy = new bl_ci.Policy();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_POLICY_BY_CUSTOMER_ID", new string[,] { 
                {"@CUSTOMER_ID", customer_id}
                }, "Function [GetPolicyByCustomerID(string customer_id), class [da_ci=>Policy]");
                if (tbl.Rows.Count > 0)
                {
                    var row = tbl.Rows[0];
                    policy.PolicyID = row["policy_id"].ToString();
                    policy.PolicyNumber = row["policy_number"].ToString();
                    policy.CustomerID = row["customer_id"].ToString();
                    policy.ProductID = row["product_id"].ToString();
                    policy.AddressID = row["address_id"].ToString();
                    policy.AgentCode = row["agent_code"].ToString();
                    //policy.PaymentCode = row["payment_code"].ToString();
                    policy.CreatedBy = row["created_by"].ToString();
                    policy.CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString());

                }
            }
            catch (Exception ex)
            {
                policy = new bl_ci.Policy();//return empty object
                Log.AddExceptionToLog("Error function [GetPolicyByPolicyNumber(string policy_number)] in class [da_ci=>Policy], detail: " + ex.Message);
            }
            return policy;
        }

        /// <summary>
        /// Save policy into table [ct_ci_policy]
        /// </summary>
        /// <param name="policy">Object [bl_ci.Policy]</param>
        /// <returns></returns>
        public static bool SavePolicy(bl_ci.Policy policy)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CI_POLICY", new string[,]{
                    {"@POLICY_ID",policy.PolicyID},
                    {"@POLICY_NUMBER", policy.PolicyNumber},
                    {"@CUSTOMER_ID", policy.CustomerID},
                    {"@PRODUCT_ID", policy.ProductID},
                    {"@ADDRESS_ID", policy.AddressID},
                    {"@AGENT_CODE", policy.AgentCode},
                    {"@CREATED_BY", policy.CreatedBy},
                    {"@CREATED_DATETIME", policy.CreatedDateTime+""}
                }, "Function [SavePolicy(bl_ci.Policy policy)], class [da_ci => Policy ]");
                
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [SavePolicy(bl_ci.Policy policy)] in class [da_ci => Policy], detail:" + ex.Message);
            }

            return result;
        }
        /// <summary>
        /// Update policy in table [ct_ci_policy] base on policy_id
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static bool UpdatePolicy(bl_ci.Policy policy)
        {
            
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CI_POLICY_BY_POLICY_ID", new string[,]{
                    {"@POLICY_ID",policy.PolicyID},
                    {"@POLICY_NUMBER", policy.PolicyNumber},
                    {"@CUSTOMER_ID", policy.CustomerID},
                    {"@PRODUCT_ID", policy.ProductID},
                    {"@ADDRESS_ID", policy.AddressID},
                    {"@AGENT_CODE", policy.AgentCode},
                    //{"@PAYMENT_CODE", policy.PaymentCode},
                    {"@UPDATED_BY", policy.UpdatedBy},
                    {"@UPDATED_DATETIME", policy.UpdatedDateTime+""}
                }, "Function [UpdatePolicy(bl_ci.Policy policy)], class [da_ci => Policy ]");

            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [UpdatePolicy(bl_ci.Policy policy)] in class [da_ci => Policy], detail:" + ex.Message);
            }

            return result;
        }
       /// <summary>
       /// Delete data from table [ct_ci_policy] base on policy_detail_id
       /// </summary>
       /// <param name="policy_id"></param>
       /// <returns></returns>
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
        /// Terminate policy: Update policy status to TER
        /// </summary>
        /// <param name="policy_id"></param>
        /// <param name="updated_by"></param>
        /// <param name="upated_datetime"></param>
        /// <returns></returns>
        public static bool TerminatePolicy(string policy_id, string updated_by, DateTime updated_datetime, string remarks)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_TERMINATE_CI_POLICY_BY_POLICY_ID", new string[,] { 
                { "@policy_id", policy_id } ,{"@updated_by",updated_by}, {"@updated_datetime", updated_datetime +""},{"@remarks", remarks}
                
                }, "function [bool TerminatePolicy(string policy_id)] in class [da_ci]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [bool TerminatePolicy(string policy_id)] in class [da_ci], detail:" + ex.Message);
            }

            return result;
        }
        /// <summary>
        /// Get all policy which has expire date = (current date + one moneth)
        /// </summary>
        /// <param name="current_date"></param>
        /// <returns></returns>
        public static DataTable GetExpirePolicy(DateTime current_date)
        {
            DataTable tbl_policy = new DataTable();
            try
            {
                tbl_policy=DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_POLICY_EXPIRE_BEFORE_1MONTH", new string[,] 
                {
                    {"@CURRENT_DATE", current_date.Date+""}
                }, "da_ci=>policy=>GetExpirePolicy(DateTime current_date)");
            }
            catch (Exception ex)
            {
                tbl_policy = new DataTable();
                Log.AddExceptionToLog("Error function [GetExpirePolicy(DateTime current_date)] in class [da_ci => Policy], detail: " + ex.Message);
            }
            return tbl_policy;
        }
        /// <summary>
        /// Get all SO,CI policies last due
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPolicyLastDue()
        {
            DataTable tbl = new DataTable();

            try
            {
                tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_CI_POLICY_LAST_DUE_DATE", new string[,] { }, "da_ci=>policy=>GetPolicyLastDue()");
            }
            catch (Exception ex)
            {
                tbl = new DataTable();
                Log.AddExceptionToLog("Error function [GetPolicyLastDue()] in class [da_ci => Policy], detail: " + ex.Message);
            }
            return tbl;
        }

        public static DataTable GetPolicyUnpaid60Days(DateTime current_date)
        {
            DataTable tbl = new DataTable();
            try
            {
                DataTable tbl_last_due = GetPolicyLastDue();
                tbl = tbl_last_due.Clone();
                tbl.Columns.Add("Period");
                tbl.Columns.Add("next_due_date");
                DataRow newRow;
                foreach (DataRow row in tbl_last_due.Rows)
                {
                    newRow = tbl.NewRow();
                    DateTime next_due;
                    next_due = Helper.GetDueDateList(Convert.ToDateTime(row["due_date"].ToString()), Convert.ToInt32(row["pay_mode_id"].ToString()))[1];
                    next_due = Calculation.GetNext_Due(next_due, Convert.ToDateTime(row["due_date"].ToString()), Convert.ToDateTime(row["due_date"].ToString()));
                    int period = current_date.Date.Subtract(next_due.Date).Days;
                    if (period == 60)
                    {
                        foreach (DataColumn col in tbl_last_due.Columns)
                        {
                            newRow[col.ColumnName] = row[col.ColumnName];
                        }
                        newRow["Period"] = period;
                        newRow["next_due_date"] = next_due;
                        tbl.Rows.Add(newRow);
                    }
                }

            }
            catch (Exception ex)
            {
                tbl = new DataTable();
                Log.AddExceptionToLog("Error function [GetPolicyUnpaid60Days(DateTime current_date)] in class [da_ci => Policy], detail: " + ex.Message);
            }
            return tbl;
        }
        public static DataTable GetPolicyUnpaid90Days(DateTime current_date)
        {
            DataTable tbl = new DataTable();
            try
            {
                DataTable tbl_last_due = GetPolicyLastDue();
                tbl = tbl_last_due.Clone();
                tbl.Columns.Add("Period");
                DataRow newRow;
                foreach (DataRow row in tbl_last_due.Rows)
                {
                    newRow = tbl.NewRow();
                    DateTime next_due;
                    next_due = Helper.GetDueDateList(Convert.ToDateTime(row["due_date"].ToString()), Convert.ToInt32(row["pay_mode_id"].ToString()))[1];
                    next_due = Calculation.GetNext_Due(next_due, Convert.ToDateTime(row["due_date"].ToString()), Convert.ToDateTime(row["due_date"].ToString()));
                    int period = current_date.Date.Subtract(next_due.Date).Days;
                    if (period == 90)
                    {
                        foreach (DataColumn col in tbl_last_due.Columns)
                        {
                            newRow[col.ColumnName] = row[col.ColumnName];
                        }
                        newRow["Period"] = period;
                        tbl.Rows.Add(newRow);
                    }
                }

            }
            catch (Exception ex)
            {
                tbl = new DataTable();
                Log.AddExceptionToLog("Error function [GetPolicyUnpaid90Days(DateTime current_date)] in class [da_ci => Policy], detail: " + ex.Message);
            }
            return tbl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current_date"></param>
        /// <returns></returns>
        public static DataTable GetOverAgeBeforeRenewal(DateTime current_date)
        {
            DataTable tbl_policy = new DataTable();
            DataTable tbl_overage;
            
            try
            {
                int age = 0;
                string age_remarks = "";
                bl_product product = new bl_product();
                bool isOverAge = false;
                tbl_policy = GetExpirePolicy(current_date.Date);//get policy before expire one month
                tbl_overage = tbl_policy.Clone();// copy table properties from tbl_policy
                tbl_overage.Columns.Add("Age"); // add column age to tbl_overage
                tbl_overage.Columns.Add("Age_Remarks");// add column age_remarks to tbl_overage
                DataRow over_age_row;
                foreach (DataRow row in tbl_policy.Rows)
                {
                    over_age_row = tbl_overage.NewRow();
                   // row["birth_date"] = new DateTime(1950,1,1);

                    // calculate age
                    age = Calculation.Culculate_Customer_Age( Convert.ToDateTime( row["Birth_date"].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(row["MATURITY_DATE"].ToString().Trim()).Date);
                    product = da_product.GetProductByProductID(row["product_id"].ToString().Trim());
                    if (age < product.Age_Min)
                    {
                        age_remarks = " Age is under ["+ product.Age_Min + "].";
                        isOverAge = true;
                    }
                    else if (age > product.Age_Max)
                    {
                        age_remarks = " Age is over [" + product.Age_Max + "].";
                        isOverAge = true;
                    }
                    else
                    {
                        isOverAge = false;
                        age_remarks = "";
                    }
                    if (isOverAge)
                    {
                        //add row value in tbl_overage
                        foreach (DataColumn col in tbl_policy.Columns)
                        {
                            over_age_row[col.ColumnName] = row[col.ColumnName];
                        }
                        over_age_row["Age"] = age + "";
                        over_age_row["age_remarks"] = age_remarks;
                        tbl_overage.Rows.Add(over_age_row);
                    }

                    //reset 
                    isOverAge = false;
                    age_remarks = "";
                }
              

            }
            catch (Exception ex)
            {
                tbl_policy = new DataTable();
                tbl_overage = new DataTable();
                Log.AddExceptionToLog("Error function [GetOverAgeBeforeRenewal(DateTime current_date)] in class [da_ci => policy], detail: " + ex.Message);
            }
            return tbl_overage;
        }
    }

    public class PolicyDetail
    {
        public PolicyDetail() { }
        
        /// <summary>
        /// Save data into table [ct_ci_policy_detail]
        /// </summary>
        /// <param name="policy_detail"></param>
        /// <returns></returns>
        public static bool SavePolicyDetail(bl_ci.PolicyDetail policy_detail)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_CI_POLICY_DETAIL", new string[,]{
                {"@POLICY_DETAIL_ID", policy_detail.PolicyDetailID},
                {"@POLICY_ID", policy_detail.PolicyID},
                {"@EFFECTIVE_DATE", policy_detail.EffectiveDate + ""},
                {"@MATURITY_DATE", policy_detail.MaturityDate+""},
                {"@EXPIRY_DATE", policy_detail.ExpiryDate +""},
                {"@ISSUED_DATE", policy_detail.IssuedDate+""},
                {"@AGE", policy_detail.Age+""},
                {"@SUM_ASSURED", policy_detail.SumAssured+""},
                {"@PAYMODE_ID", policy_detail.PayModeID+""},
                {"@PAYMENT_CODE", policy_detail.PaymentCode},
                {"@PAYMENT_BY", policy_detail.PaymentBy},
                {"@USER_PREMIUM", policy_detail.UserPremium+""},
                {"@PREMIUM", policy_detail.Premium+""},
                {"@RETURN_PREMIUM", policy_detail.RETURN_PREMIUM+""},
                {"@ORIGINAL_PREMIUM", policy_detail.OriginalPremium+""},
                {"@DISCOUNT_AMOUNT", policy_detail.DiscountAmount+""},
                {"@COVER_YEAR", policy_detail.CoverYear+""},
                {"@PAY_YEAR", policy_detail.PayYear+""},
                {"@COVER_UP_TO_AGE", policy_detail.CoverUpToAge+""},
                {"@PAY_UP_TO_AGE", policy_detail.PayUpToAge+""},
                {"@POLICY_STATUS_REMARKS", policy_detail.PolicyStatusRemarks},
                {"@CREATED_BY", policy_detail.CreatedBy},
                {"@CREATED_DATETIME", policy_detail.CreatedDateTime+""}
                }, "Function [SavePolicyDetail(bl_ci.PolicyDetail policy_detail)], class [da_ci=>PolicyDetail]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function[SavePolicyDetail(bl_ci.PolicyDetail policy_detail)], in class [da_ci=>PolicyDetail]], detail:" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// Update data in table [ct_ci_policy_detail] base on policy_detail_id
        /// </summary>
        /// <param name="policy_detail"></param>
        /// <returns></returns>
        public static bool UpdatePolicyDetail(bl_ci.PolicyDetail policy_detail)
        {
            bool result = false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_CI_POLICY_DETAIL_BY_POLICY_DETAIL_ID", new string[,]{
                {"@POLICY_DETAIL_ID", policy_detail.PolicyDetailID},
                {"@POLICY_ID", policy_detail.PolicyID},
                {"@EFFECTIVE_DATE", policy_detail.EffectiveDate + ""},
                {"@MATURITY_DATE", policy_detail.MaturityDate+""},
                {"EXPIRY_DATE", policy_detail.ExpiryDate +""},
                {"ISSUED_DATE", policy_detail.IssuedDate+""},
                {"@AGE", policy_detail.Age+""},
                {"@SUM_ASSUED", policy_detail.SumAssured+""},
                {"@PAYMODE_ID", policy_detail.PayModeID+""},
                {"@PAYMENT_CODE", policy_detail.PaymentCode},
                {"@PAYMENT_BY", policy_detail.PaymentBy},
                {"@USER_PREMIUM", policy_detail.UserPremium+""},
                {"@PREMIUM", policy_detail.Premium+""},
                {"@RETURN_PREMIUM", policy_detail.RETURN_PREMIUM+""},
                {"@ORIGINAL_PREMIUM", policy_detail.OriginalPremium+""},
                {"@DISCOUNT_AMOUNT", policy_detail.DiscountAmount+""},
                {"@COVER_YEAR", policy_detail.CoverYear+""},
                {"@PAY_YEAR", policy_detail.PayYear+""},
                {"@COVER_UP_TO_AGE", policy_detail.CoverUpToAge+""},
                {"@PAY_UP_TO_AGE", policy_detail.PayUpToAge+""},
                {"@POLICY_STATUS_REMARKS", policy_detail.PolicyStatusRemarks},
                {"@CREATED_BY", policy_detail.CreatedBy},
                {"@CREATED_DATETIME", policy_detail.CreatedDateTime+""}
                }, "Function [UpdatePolicyDetail(bl_ci.PolicyDetail policy_detail)], class [da_ci=>PolicyDetail]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function[UpdatePolicyDetail(bl_ci.PolicyDetail policy_detail)], in class [da_ci=>PolicyDetail]], detail:" + ex.Message);
            }
            return result;
        }
        public static bool DeletePolicyDetail(string policy_detail_id)
        {
            bool result =false;
            try
            {
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_CI_POLICY_DETIAL_BY_POLICY_DETAIL_ID", new string[,] { 
                {"@POLICY_DETAIL_ID", policy_detail_id}
                }, "Function [DeletePolicyDetail(string policy_detail_id)], class [da_ci=>PolicyDetail]");
            }
            catch (Exception ex)
            {
                result = false;
                Log.AddExceptionToLog("Error function [DeletePolicyDetail(string policy_detail_id)] in class [da_ic==>PolicyDetail], detail:" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// Get last policy detail by policy number
        /// </summary>
        /// <param name="policy_number"></param>
        /// <returns></returns>
        public static bl_ci.PolicyDetail GetLastPolicyDetail(string policy_number)
        {
            bl_ci.PolicyDetail pol_detail = new bl_ci.PolicyDetail();
            try
            {
                DataTable tbl = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 01, 01), new DateTime(1900, 01, 01), "", policy_number, "");
                if (tbl.Rows.Count > 0)
                { 
                    var row = tbl.Rows[0];
                    pol_detail.PolicyDetailID = row["policy_detail_id"].ToString().Trim();
                    pol_detail.PolicyID = row["policy_id"].ToString().Trim();
                    pol_detail.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString().Trim());
                    pol_detail.MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString().Trim());
                    pol_detail.ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString().Trim());
                    pol_detail.IssuedDate = Convert.ToDateTime(row["issued_date"].ToString().Trim());
                    pol_detail.Age=Convert.ToInt32(row["age"].ToString().Trim());
                    pol_detail.SumAssured = Convert.ToDouble(row["sum_assured"].ToString());
                    pol_detail.PayModeID = Helper.GetPayModeID( row["mode"].ToString().Trim());
                    pol_detail.PaymentCode = row["payment_code"].ToString().Trim();
                    pol_detail.PaymentBy = row["payment_by"].ToString().Trim();
                    pol_detail.UserPremium = Convert.ToDouble(row["user_premium"].ToString().Trim());
                    pol_detail.Premium = Convert.ToDouble(row["premium"].ToString().Trim());
                    pol_detail.RETURN_PREMIUM = Convert.ToDouble(row["return_premium"].ToString().Trim());
                    pol_detail.OriginalPremium = Convert.ToDouble(row["original_premium"].ToString().Trim());
                    pol_detail.DiscountAmount = Convert.ToDouble(row["discount_amount"].ToString().Trim());
                    pol_detail.CoverYear = Convert.ToInt32(row["cover_year"].ToString().Trim());
                    pol_detail.PayYear = Convert.ToInt32(row["pay_year"].ToString().Trim());
                    pol_detail.CoverUpToAge = Convert.ToInt32(row["cover_up_to_age"].ToString().Trim());
                    pol_detail.PayUpToAge = Convert.ToInt32(row["pay_up_to_age"].ToString().Trim());
                    pol_detail.PolicyStatusRemarks = row["policy_status_remarks"].ToString().Trim();
                    //pol_detail.CreatedBy = row["created_by"].ToString().Trim();
                    //pol_detail.CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString().Trim());
                    //pol_detail.UpdatedBy = row["updated_by"].ToString().Trim();
                    //pol_detail.UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString().Trim());
                   
                }
            }
            catch (Exception ex)
            {
                pol_detail = new bl_ci.PolicyDetail();
                Log.AddExceptionToLog("Error function [GetLastPolicyDetail(string policy_number)] in class [da_ci.policy_detail] , detail: " + ex.Message);
            }
            return pol_detail;
        }

    }
}