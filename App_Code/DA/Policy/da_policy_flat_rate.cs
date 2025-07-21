using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_policy_flat_rate
/// </summary>
public class da_policy_flat_rate
{
    public da_policy_flat_rate()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /*
    /// <summary>
    /// Get All Policy with status active
    /// </summary>
    /// <returns></returns>
    public static List<bl_policy_flat_rate> GetActivePolicyList()
    {
        List<bl_policy_flat_rate> obj_arr = new List<bl_policy_flat_rate>();

        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("Pro_Name", new string[,] { });
            foreach (DataRow row in tbl.Rows)
            {
                obj_arr.Add(new bl_policy_flat_rate()
                {
                    PolicyID = row["Policy_ID"].ToString(),
                    PolicyNumber = row["policy_number"].ToString(),
                    CustomerID = row["cust_id"].ToString(),
                    ApplicationNumber = row["app_number"].ToString(),
                    ApplicationOriginNumber = row["app_origin_number"].ToString(),
                    ApplicationDate = Convert.ToDateTime(row["app_date"].ToString()),
                    ApplicationRemarks = row["app_remarks"].ToString(),
                    PolicyRemarks=row["policy_remarks"].ToString(),
                    PaymentCode = row["payment_code"].ToString(),
                    PayModeID = Convert.ToInt32(row["pay_mode_id"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                    MaturityDate = Convert.ToDateTime(row["marturity_date"].ToString()),
                    ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                    IssuedDate = Convert.ToDateTime(row["issured_date"].ToString()),
                    ApprovedDate = Convert.ToDateTime(row["approved_date"].ToString()),
                    ApprovedBy = row["approved_by"].ToString(),
                    ProductID = row["product_id"].ToString(),
                    SumInsured = Convert.ToDouble(row["sum_insured"].ToString()),
                    AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString()),
                    AnnualOriginPremium=Convert.ToDouble(row["annual_origin_premium"].ToString()),
                    PremiumByMode=Convert.ToDouble(row["premium_by_mode"].ToString()),
                    ActualPremium = Convert.ToDouble(row["actual_premium"].ToString()),
                    ReturnPremiunm=Convert.ToDouble(row["return_premium"].ToString()),
                    Discount = Convert.ToDouble(row["discount"].ToString()),
                    ExtraAnnualPremium = Convert.ToDouble(row["extra_annual_premium"].ToString()),
                    ExtraPremiumByMode = Convert.ToDouble(row["Extra_premium_by_mode"].ToString()),
                    PolicyStatusID = Convert.ToInt32(row["policy_status_id"].ToString()),
                    UnderWritingStatusID = row["uw_status_id"].ToString(),
                    Remarks = row["remarks"].ToString(),
                    CreatedBy = row["created_by"].ToString(),
                    CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                    UpdatedBy = row["updated_by"].ToString(),
                    UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString())

                });
            }
                
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetActivePolicyList] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return obj_arr;
    }
    */
    /*
    /// <summary>
    /// Get Policy with status active filter by policy 
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public bl_policy_flat_rate GetActivePolicyByPolicyID(string policy_id)
    {
        bl_policy_flat_rate obj_policy = new bl_policy_flat_rate();

        try
        {
            List<bl_policy_flat_rate> obj_arr = new List<bl_policy_flat_rate>();
            obj_arr = GetActivePolicyList();
            foreach (bl_policy_flat_rate obj in obj_arr.Where(obj => obj.PolicyID == policy_id))//filter by linq
            {
                obj_policy = obj;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetActivePolicyByPolicyID] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return obj_policy;
    }
    */

    public static bl_policy_flat_rate GetPolicy(string policy_id)
    {
        bl_policy_flat_rate obj = new bl_policy_flat_rate();

        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_BY_POLICY_ID", new string[,] { { "@POLICY_ID", policy_id } });
            foreach (DataRow row in tbl.Rows)
            {
                obj = (new bl_policy_flat_rate()
                {
                    PolicyID = row["Policy_ID"].ToString(),
                    PolicyNumber = row["policy_number"].ToString(),
                    CustomerID = row["cust_id"].ToString(),
                    ApplicationNumber = row["app_number"].ToString(),
                    ApplicationOriginNumber = row["app_origin_number"].ToString(),
                    ApplicationDate = Convert.ToDateTime(row["app_date"].ToString()),
                    ApplicationRemarks = row["app_remarks"].ToString(),
                    PolicyRemarks = row["policy_remarks"].ToString(),
                    PaymentCode = row["payment_code"].ToString(),
                    PayModeID = Convert.ToInt32(row["pay_mode_id"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                    MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString()),
                    ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                    IssuedDate = Convert.ToDateTime(row["issued_date"].ToString()),
                    ApprovedDate = Convert.ToDateTime(row["approved_date"].ToString()),
                    ApprovedBy = row["approved_by"].ToString(),
                    ProductID = row["product_id"].ToString(),
                    SumInsured = Convert.ToDouble(row["sum_insured"].ToString()),
                    AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString()),
                    AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString()),
                    PremiumByMode = Convert.ToDouble(row["premium_by_mode"].ToString()),
                    ActualPremium = Convert.ToDouble(row["actual_premium"].ToString()),
                    ReturnPremium = Convert.ToDouble(row["return_premium"].ToString()),
                    Discount = Convert.ToDouble(row["discount"].ToString()),
                    ExtraAnnualPremium = Convert.ToDouble(row["extra_annual_premium"].ToString()),
                    ExtraPremiumByMode = Convert.ToDouble(row["Extra_premium_by_mode"].ToString()),
                    PolicyStatusID = Convert.ToInt32(row["policy_status_id"].ToString()),
                    CustomerName = row["customer_Name"].ToString(),
                    CustomerTypeID = row["customer_type_id"].ToString(),
                    AssuredYear = Convert.ToInt32(row["assured_year"].ToString()),
                    PayYear = Convert.ToInt32(row["pay_year"].ToString()),
                    Gender = row["gender"].ToString(),
                    BirthDate = Convert.ToDateTime(row["birth_date"].ToString()),
                    UnderWritingStatusID = row["uw_status_id"].ToString(),
                    Remarks = row["remarks"].ToString(),
                    CreatedBy = row["created_by"].ToString(),
                    CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                    UpdatedBy = row["updated_by"].ToString(),
                    UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString()),
                    SaleAgentID = row["sale_agent_id"].ToString(),
                    SaleAgentName = row["sale_agent_name"].ToString(),
                    PolicyStatus = GetPolicyStatus(Convert.ToInt32(row["policy_status_id"].ToString())),
                    ChannelID = row["CHANNEL_ID"].ToString(),
                    ChannelItemID = row["CHANNEL_ITEM_ID"].ToString()

                });

               
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicy] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return obj;
    }
    public static string GetPolicyStatus(int policy_status_id)
    {
        string str_status = "";
        try
        {
            foreach (int status in Enum.GetValues(typeof(bl_policy_flat_rate.Status)))
            {
                if (status == policy_status_id)
                {
                    str_status = Enum.GetName(typeof(bl_policy_flat_rate.Status), status);
                    break;
                }

                //str += status + "_";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicyStatus] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return str_status;
    }

    public static List<bl_policy_flat_rate> GetPolicyByParameters(string parameters)
    {
        bl_policy_flat_rate obj = new bl_policy_flat_rate();
        List<bl_policy_flat_rate> arr_obj = new List<bl_policy_flat_rate>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_BY_PARAMETERS", new string[,] { { "@PARAMETERS", parameters } });
            foreach (DataRow row in tbl.Rows)
            {
                obj = (new bl_policy_flat_rate()
                {
                    PolicyID = row["Policy_ID"].ToString(),
                    PolicyNumber = row["policy_number"].ToString(),
                    CustomerID = row["cust_id"].ToString(),
                    ApplicationNumber = row["app_number"].ToString(),
                    ApplicationOriginNumber = row["app_origin_number"].ToString(),
                    ApplicationDate = Convert.ToDateTime(row["app_date"].ToString()),
                    ApplicationRemarks = row["app_remarks"].ToString(),
                    PolicyRemarks = row["policy_remarks"].ToString(),
                    PaymentCode = row["payment_code"].ToString(),
                    PayModeID = Convert.ToInt32(row["pay_mode_id"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                    MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString()),
                    ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                    IssuedDate = Convert.ToDateTime(row["issued_date"].ToString()),
                    ApprovedDate = Convert.ToDateTime(row["approved_date"].ToString()),
                    ApprovedBy = row["approved_by"].ToString(),
                    ProductID = row["product_id"].ToString(),
                    SumInsured = Convert.ToDouble(row["sum_insured"].ToString()),
                    AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString()),
                    AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString()),
                    PremiumByMode = Convert.ToDouble(row["premium_by_mode"].ToString()),
                    ActualPremium = Convert.ToDouble(row["actual_premium"].ToString()),
                    ReturnPremium = Convert.ToDouble(row["return_premium"].ToString()),
                    Discount = Convert.ToDouble(row["discount"].ToString()),
                    ExtraAnnualPremium = Convert.ToDouble(row["extra_annual_premium"].ToString()),
                    ExtraPremiumByMode = Convert.ToDouble(row["Extra_premium_by_mode"].ToString()),
                    PolicyStatusID = Convert.ToInt32(row["policy_status_id"].ToString()),
                    CustomerName = row["customer_Name"].ToString(),
                    CustomerNameKh = row["customer_name_kh"].ToString(),
                    CustomerTypeID = row["customer_type_id"].ToString(),
                    CustomerNumber= row["CUST_NO"].ToString(),
                    AssuredYear = Convert.ToInt32(row["assured_year"].ToString()),
                    PayYear = Convert.ToInt32(row["pay_year"].ToString()),
                    Gender = row["gender"].ToString(),
                    BirthDate = Convert.ToDateTime(row["birth_date"].ToString()),
                    UnderWritingStatusID = row["uw_status_id"].ToString(),
                    Remarks = row["remarks"].ToString(),
                    CreatedBy = row["created_by"].ToString(),
                    CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                    UpdatedBy = row["updated_by"].ToString(),
                    UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString()),
                    SaleAgentID = row["sale_agent_id"].ToString(),
                    SaleAgentName = row["sale_agent_name"].ToString(),
                     ChannelID = row["CHANNEL_ID"].ToString(),
                    ChannelItemID = row["CHANNEL_ITEM_ID"].ToString()
                });

                arr_obj.Add(obj);
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicyByParameters] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return arr_obj;
    }
    public static bool SavePolicy(bl_policy_flat_rate policy)
    {
        bool status = false;
        try
        {
            string a = (int)bl_policy_flat_rate.Status.New+"";

            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_FLAT_RATE", new string[,] { 
            
                {"@POLICY_ID", policy.PolicyID},
                {"@CUST_ID", policy.CustomerID},
                {"@PRODUCT_ID", policy.ProductID},
                {"@POLICY_NUMBER", policy.PolicyNumber},
                {"@POLICY_TYPE", policy.PolicyType+""},
                {"@EFFECTIVE_DATE", policy.EffectiveDate+""},
                {"@MATURITY_DATE", policy.MaturityDate+""},
                {"@ISSUED_DATE", policy.IssuedDate+""},
                {"@AGREEMENT_DATE", policy.IssuedDate+""},
                {"@SUM_INSURED", policy.SumInsured+""},
                {"@PREMIUM", policy.Premium+""},
                {"@PAY_MODE_ID", policy.PayModeID+""},
                {"@ANNUAL_PREMIUM", policy.AnnualPremium+""},
                {"@DISCOUNT", policy.Discount+""},
                {"@AGE_INSURE", policy.Age_Insure+""},
                {"@PAY_UP_TO_AGE", policy.PayUpToAge+""},
                {"@ASSURE_UP_TO_AGE", policy.AssuredUpToAge+""},
                {"@PAY_YEAR", policy.PayYear+""},
                {"@ASSURED_YEAR", policy.AssuredYear+""},
                {"@CREATED_BY", policy.CreatedBy},
                {"@CREATED_DATETIME", policy.CreatedDateTime+""},
                {"@CHANNEL_ITEM_ID", policy.ChannelItemID},

                {"@EM_PERCENT", policy.ChannelItemID},
                {"@EF_RATE", policy.ChannelItemID},
                {"@EF_PREMIUM", policy.ChannelItemID},
                {"@EM_PREMIUM", policy.ChannelItemID},
                {"@TOTAL_EF_YEAR", policy.ChannelItemID}
                //{"@ORIGINAL_AMOUNT", policy.AnnualOriginPremium},
                //{"@EM_AMOUNT", policy.ExtraAmount}

            }, "da_policy_flat_rate => SavePolicy");
            
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SavePolicy] in class [da_policy_flat_rate], Detail: " + ex.InnerException + "==>"  + ex.Message + "==>" + ex.StackTrace);
            status = false;
        }
        return status;
    }

    public static bool UpdatePolicy(bl_policy_flat_rate policy)
    {
        bool status = false;
        try
        {
            //store old data
            bl_policy_flat_rate old_pol = GetPolicy(policy.PolicyID);

            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_POLICY_FLAT_RATE_BY_POLICY_ID", new string[,] { 
            
                {"@POLICY_ID", policy.PolicyID},
                {"@CUST_ID", policy.CustomerID},
                {"@PRODUCT_ID", policy.ProductID},
                {"@POLICY_NUMBER", policy.PolicyNumber},
                {"@APP_NUMBER", policy.ApplicationNumber},
                {"@APP_ORIGIN_NUMBER", policy.ApplicationOriginNumber},
                {"@PAYMENT_CODE", policy.PaymentCode},
                {"@APP_DATE", policy.ApplicationDate+""},
                {"@EFFECTIVE_DATE", policy.EffectiveDate+""},
                {"@MATURITY_DATE", policy.MaturityDate+""},
                {"@EXPIRY_DATE", policy.ExpiryDate+""}, 
                {"@ISSUED_DATE", policy.IssuedDate+""},
                {"@APPROVED_DATE", policy.ApprovedDate+""}, 
                {"@APPROVED_BY", policy.ApprovedBy},
                {"@SUM_INSURED", policy.SumInsured+""},
                {"@PAY_MODE_ID", policy.PayModeID+""},
                {"@ANNUAL_PREMIUM", policy.AnnualPremium+""},
                {"@ANNUAL_ORIGIN_PREMIUM", policy.AnnualOriginPremium+""},
                {"@PREMIUM_BY_MODE", policy.PremiumByMode+""},
                {"@ACTUAL_PREMIUM", policy.ActualPremium+""},
                {"@RETURN_PREMIUM", policy.ReturnPremium+""},
                {"@DISCOUNT", policy.Discount+""},
                {"@EXTRA_ANNUAL_PREMIUM", policy.ExtraAnnualPremium+""},
                {"@EXTRA_PREMIUM_BY_MODE", policy.ExtraPremiumByMode+""},
                //{"@POLICY_STATUS_ID", policy.PolicyStatusID+""},
                {"@POLICY_STATUS_ID", (int)bl_policy_flat_rate.Status.New+""},
                {"@UW_STATUS_ID", policy.UnderWritingStatusID},
                {"@POLICY_REMARKS", policy.PolicyRemarks},
                {"@APP_REMARKS", policy.ApplicationRemarks},
                {"@REMARKS", policy.Remarks},
                {"@UPDATED_BY", policy.UpdatedBy},
                {"@UPDATED_DATETIME", policy.UpdatedDateTime+""},
                {"@PAY_YEAR", policy.PayYear+""},
                {"@ASSURED_YEAR", policy.AssuredYear+""},
                {"@SALE_AGENT_ID", policy.SaleAgentID},
                {"@CHANNEL_ID", policy.ChannelID},
                {"@CHANNEL_ITEM_ID", policy.ChannelItemID}
            }, "da_policy_flat_rate => UpdatePolicy");

            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdatePolicy] in class [da_policy_flat_rate], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }

    public static bool DeletePolicy(string policy_id, string user_login)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_POLICY_FLAT_RATE_BY_POLICY_ID", new string[,] { 
                {"@POLICY_ID", policy_id },
                {"@USER", user_login}
            },"da_policy_flat_rate => DeletePolicy");
            
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [DeletePolicy] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return status;
    }
    /// <summary>
    /// Approve policy by policy id.
    /// This function will automatically generate policy number and insert policy number
    /// </summary>
    /// <param name="option">[Approve] or [Decline]</param>
    /// <param name="policy_id"></param>
    /// <param name="approved_by"></param>
    /// <param name="approved_date"></param>
    /// <returns></returns>
    public static bool ApprovedPolicy(string option, string policy_id, string policy_status_id, string approved_by, DateTime approved_date, DateTime issued_date, string remarks)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_APPROVED_POLICY_FLAT_RATE_BY_POLICY_ID", new string[,] {{"@OPTION",option}, { "@POLICY_ID", policy_id }, { "@POLICY_STATUS_ID", policy_status_id }, { "@APPROVED_BY", approved_by }, { "@APPROVED_DATE", approved_date + "" }, {"@ISSUED_DATE", issued_date + ""}, {"@REMARKS", remarks} }, "da_policy_flat_rate => ApprovedPolicy");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ApprovedPolicy] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return status;
    }

    /// <summary>
    /// Roll back while system save error.
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bool RollBack(string policy_id)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_POLICY_FLAT_RATE_ROLL_BACK", new string[,] { { "@POLICY_ID", policy_id } }, "da_policy_flat_rate => RollBack");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [RollBack] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return status;
    }

    /// <summary>
    /// By: Maneth
    /// Date: 27 09 2017
    /// </summary>
    public class da_member
    {
        public da_member() { }

        public static List<bl_policy_flat_rate.bl_member> GetMemberByPolicyID(string policy_id)
        {
            List<bl_policy_flat_rate.bl_member> arr_obj = new List<bl_policy_flat_rate.bl_member>();

            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure("pro_name", new string[,] { });
                foreach (DataRow row in tbl.Rows)
                {
                    arr_obj.Add(new bl_policy_flat_rate.bl_member()
                    {
                        PolicyID = row["policy_id"].ToString(),
                        PolicyMemberID = row["policy_member_id"].ToString(),
                        CustomerID = row["cust_id"].ToString(),
                        Age = Convert.ToInt32(row["age"].ToString()),
                        PayYear = Convert.ToInt32(row["pay_year"].ToString()),
                        PayUpToAge = Convert.ToInt32(row["pay_up_to_age"].ToString()),
                        AssuredYear = Convert.ToInt32(row["assured_year"].ToString()),
                        AssuredUpToAge = Convert.ToInt32(row["assured_up_to_age"].ToString()),
                        SumInsured = Convert.ToDouble(row["sum_insured"].ToString()),
                        AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString()),
                        AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString()),
                        PremiumByMode = Convert.ToDouble(row["premium_by_mode"].ToString()),
                        ExtraAnnualPremium = Convert.ToDouble(row["extra_annual_premium"].ToString()),
                        ExtraPremiumByMode = Convert.ToDouble(row["extra_premium_by_mode"].ToString()),
                        ExtraPercentage = Convert.ToDouble(row["extra_percentage"].ToString()),
                        ExtraRate = Convert.ToDouble(row["extra_rate"].ToString()),
                        ReturnPremium = Convert.ToDouble(row["return_premium"].ToString()),
                        Discount = Convert.ToDouble(row["discount"].ToString()),
                        EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                        PayModeID = Convert.ToInt32(row["pay_mode_id"].ToString()),
                        StatusID = Convert.ToInt32(row["status_id"].ToString()),
                        Remarks = row["remarks"].ToString(),
                        CreatedBy = row["created_by"].ToString(),
                        CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                        UpdatedBy = row["updated_by"].ToString(),
                        UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetMemberByPolicyID] in class [da_policy_flat_rate => da_member], Detail: " + ex.Message);
            }
            return arr_obj;
        }

        public static bool SaveMember(bl_policy_flat_rate.bl_member member)
        {
            bool status = false;
            try
            {
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_FLAT_RATE_MEMBER", new string[,] { 
                
                    {"@POLICY_ID", member.PolicyID},
                    {"@POLICY_MEMBER_ID", member.PolicyMemberID},
                    {"@CUST_ID", member.CustomerID},
                    {"@AGE", member.Age+""},
                    {"@PAY_YEAR", member.PayYear+""},
                    {"@PAY_UP_TO_AGE", member.PayUpToAge+""},
                    {"@ASSURED_YEAR", member.AssuredYear+""},
                    {"@ASSURED_UP_TO_AGE", member.AssuredUpToAge+""},
                    {"@SUM_INSURED", member.SumInsured+""},
                    {"@ANNUAL_PREMIUM", member.AnnualPremium+""},
                    {"@ANNUAL_ORIGIN_PREMIUM", member.AnnualOriginPremium+""},
                    {"@PREMIUM_BY_MODE", member.PremiumByMode+""},
                    {"@EXTRA_ANNUAL_PREMIUM", member.ExtraAnnualPremium+""},
                    {"@EXTRA_PREMIUM_BY_MODE", member.ExtraPremiumByMode+""},
                    {"@EXTRA_PERCENTAGE", member.ExtraPercentage+""},
                    {"@EXTRA_RATE", member.ExtraRate+""},
                    {"@RETURN_PREMIUM", member.ReturnPremium+""},
                    {"@DISCOUNT", member.Discount+""},
                    {"@EFFECTIVE_DATE", member.EffectiveDate+""},
                    {"@EXPIRY_DATE", member.ExpiryDate+""},
                    {"@PAY_MODE_ID", member.PayModeID+""},
                    {"@STATUS_ID", member.StatusID+""},
                    {"@REMARKS", member.Remarks},
                    {"@CREATED_BY", member.CreatedBy},
                    {"@CREATED_DATETIME", member.CreatedDateTime+""}
                  
                }, "da_policy_flat_rate => SaveMember");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [SaveMember] in class [da_policy_flat_rate => da_member], Detail: " + ex.Message);
            }
            return status;
        }

        public static bool UpdateMember(bl_policy_flat_rate.bl_member member)
        {
            bool status = false;
            try
            {
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_POLICY_FLAT_RATE_MEMBER_BY_MEMBER_ID", new string[,] { 
                
                    {"@POLICY_ID", member.PolicyID},
                    {"@POLICY_MEMBER_ID", member.PolicyMemberID},
                    {"@CUST_ID", member.CustomerID},
                    {"@AGE", member.Age+""},
                    {"@PAY_YEAR", member.PayYear+""},
                    {"@PAY_UP_TO_AGE", member.PayUpToAge+""},
                    {"@ASSURED_YEAR", member.AssuredYear+""},
                    {"@ASSURED_UP_TO_AGE", member.AssuredUpToAge+""},
                    {"@SUM_INSURED", member.SumInsured+""},
                    {"@ANNUAL_PREMIUM", member.AnnualPremium+""},
                    {"@ANNUAL_ORIGIN_PREMIUM", member.AnnualOriginPremium+""},
                    {"@PREMIUM_BY_MODE", member.PremiumByMode+""},
                    {"@EXTRA_ANNUAL_PREMIUM", member.ExtraAnnualPremium+""},
                    {"@EXTRA_PREMIUM_BY_MODE", member.ExtraPremiumByMode+""},
                    {"@EXTRA_PERCENTAGE", member.ExtraPercentage+""},
                    {"@EXTRA_RATE", member.ExtraRate+""},
                    {"@RETURN_PREMIUM", member.ReturnPremium+""},
                    {"@DISCOUNT", member.Discount+""},
                    {"@EFFECTIVE_DATE", member.EffectiveDate+""},
                    {"@EXPIRY_DATE", member.ExpiryDate+""},
                    {"@PAY_MODE_ID", member.PayModeID+""},
                    {"@STATUS_ID", member.StatusID+""},
                    {"@REMARKS", member.Remarks},
                    {"@UPDATED_BY", member.UpdatedBy},
                    {"@UPDATED_DATETIME", member.UpdatedDateTime+""}
                }, "da_policy_flat_rate => UpdateMember");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [UpdateMember] in class [da_policy_flat_rate => da_member], Detail: " + ex.Message);
            }
            return status;
        }

        public static List<bl_policy_flat_rate.bl_member> GetMemberList(string policy_id)
        {
            //SP_GET_POLICY_FLAT_RATE_MEMBER_BY_POLICY_ID
            List<bl_policy_flat_rate.bl_member> member_list = new List<bl_policy_flat_rate.bl_member>();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_MEMBER_BY_POLICY_ID", new string[,] { { "@POLICY_ID", policy_id } });
                foreach (DataRow row in tbl.Rows)
                {
                    bl_policy_flat_rate.bl_member member = new bl_policy_flat_rate.bl_member();
                    member.CustomerID = row["cust_id"].ToString();
                    member.PolicyID = row["policy_id"].ToString();
                    member.PolicyMemberID = row["policy_member_id"].ToString();
                    member.Age = Convert.ToInt32(row["age"].ToString());
                    member.PayYear = Convert.ToInt32(row["pay_year"].ToString());
                    member.PayUpToAge = Convert.ToInt32(row["pay_up_to_age"].ToString());
                    member.AssuredYear = Convert.ToInt32(row["assured_year"].ToString());
                    member.AssuredUpToAge = Convert.ToInt32(row["assured_up_to_age"].ToString());
                    member.SumInsured = Convert.ToDouble(row["sum_insured"].ToString());
                    member.AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString());
                    member.AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString());//rounded
                    member.PremiumByMode = Convert.ToDouble(row["premium_by_mode"].ToString());
                    member.PayModeID = Convert.ToInt32(row["pay_mode_id"].ToString());
                    member.Discount = Convert.ToDouble(row["discount"].ToString());
                    member.ExtraPercentage = Convert.ToDouble(row["extra_percentage"].ToString());
                    member.ExtraRate = Convert.ToDouble(row["extra_rate"].ToString());
                    member.ExtraAnnualPremium = Convert.ToDouble(row["extra_annual_premium"].ToString());
                    member.ExtraPremiumByMode = Convert.ToDouble(row["extra_premium_by_mode"].ToString());
                    member.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                    member.ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString());
                    member.StatusID = Convert.ToInt32(row["status_id"].ToString());
                    member.Remarks = row["remarks"].ToString();
                    member.CustomerName = row["cust_name"].ToString();
                    member.Gender = row["gender"].ToString();
                    member.BirthDate = Convert.ToDateTime(row["birth_date"].ToString());
                    //add member to list
                    member_list.Add(member);
                }

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetMemberList] in class [da_policy_flat_rate => da_member], Detail: " + ex.Message);
            }
            return member_list;
        }

        public static bool DeleteMembers(string member_id, string user_login)
        {
            bool status = false;
            try
            {
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_POLICY_FLAT_RATE_MEMBER_BY_MEMBER_ID", new string[,] { { "@POLICY_MEMBER_ID", member_id },{"@USER", user_login} }, "da_policy_flat_rate => DeleteMembers");
            }
            catch (Exception ex)
            {
                status = false;
                Log.AddExceptionToLog("Error function [DeleteMembers] in class [da_policy_flat_rate => da_member], Detail: " + ex.Message);
            }

            return status;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class da_cover
    {
        public da_cover() { }
        /// <summary>
        /// Get all covers master
        /// </summary>
        /// <returns></returns>
        public static List<bl_policy_flat_rate.bl_cover> CoverList()
        {
            List<bl_policy_flat_rate.bl_cover> arr_obj = new List<bl_policy_flat_rate.bl_cover>();

            try
            {
                foreach (DataRow row in DataSetGenerator.Get_Data_Soure("SP_GET_COVER", new string[,] { }).Rows)
                {
                    arr_obj.Add(new bl_policy_flat_rate.bl_cover()
                    {
                        CoverID = row["cover_id"].ToString(),
                        Cover = row["cover"].ToString(),
                        Remarks = row["remarks"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [CoverList] in class [da_policy_flat_rate => da_cover], Detail: " + ex.Message);
            }
            return arr_obj;
        }

        public static bl_policy_flat_rate.bl_cover GetCover(string cover_id)
        {
            bl_policy_flat_rate.bl_cover obj = new bl_policy_flat_rate.bl_cover();

            try
            {
                //use linq to filter cover by cover_id
                foreach (bl_policy_flat_rate.bl_cover cover in CoverList().Where(_ => _.CoverID.Trim().ToUpper() == cover_id.Trim().ToUpper()))
                {
                    obj = cover;
                    break;
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetCover] in class [da_policy_flat_rate => da_cover], Detail: " + ex.Message);
            }
            return obj;
        }
    }


    /// <summary>
    /// By: Maneth
    /// Date: 27 09 2017 
    /// </summary>
    public class da_policy_cover
    {
        public da_policy_cover() { }

        public static List<bl_policy_flat_rate.bl_policy_cover> GetCoverByPolicyID(string policy_id)
        {
            List<bl_policy_flat_rate.bl_policy_cover> arr_obj = new List<bl_policy_flat_rate.bl_policy_cover>();
            try
            {
                bl_policy_flat_rate.bl_policy_cover obj;
                DataTable tbl = DataSetGenerator.Get_Data_Soure("GET_POLICY_FLAT_RATE_COVER_BY_POLICY_ID", new string[,] { { "@POLICY_ID", policy_id } });
                foreach (DataRow row in tbl.Rows)
                {
                    obj = new bl_policy_flat_rate.bl_policy_cover();
                    obj.PolicyCoverID = row["policy_cover_id"].ToString();
                    obj.PolicyID = row["policy_id"].ToString();
                    obj.CustomerID = row["cust_id"].ToString();
                    obj.PremiumByMode = Convert.ToInt32(row["premium_by_mode"].ToString());
                    obj.AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString());
                    obj.AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString());
                    obj.Age = Convert.ToInt32(row["age"].ToString());
                    obj.PayYear = Convert.ToInt32(row["pay_year"].ToString());
                    obj.PayUpToAge = Convert.ToInt32(row["pay_up_to_age"].ToString());
                    obj.AssuredUpToAge = Convert.ToInt32(row["assured_up_to_age"].ToString());
                    obj.AssuredYear = Convert.ToInt32(row["assured_year"].ToString());
                    obj.CoverTypeID = row["cover_type"].ToString();
                    //get cover name
                    bl_policy_flat_rate.bl_cover cover = da_policy_flat_rate.da_cover.GetCover(obj.CoverTypeID);
                    obj.CoverType = cover.Cover;

                    obj.SumInsured = Convert.ToDouble(row["sum_insured"].ToString());
                    obj.Remarks = row["remarks"].ToString();
                    obj.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                    obj.ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString());
                    obj.CreatedBy = row["created_by"].ToString();
                    obj.CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString());
                    //obj.UpdatedBy = row["updated_by"].ToString();
                    //obj.UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString());
                    arr_obj.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetCoverByPolicyID] in class [da_policy_flat_rate => da_cover], Detail: " + ex.Message);
            }
            return arr_obj;
        }
        public static List<bl_policy_flat_rate.bl_policy_cover> GetCoverByPolicyCoverID(string policy_cover_id)
        {
            List<bl_policy_flat_rate.bl_policy_cover> arr_obj = new List<bl_policy_flat_rate.bl_policy_cover>();
            try
            {
                bl_policy_flat_rate.bl_policy_cover obj;
                DataTable tbl = DataSetGenerator.Get_Data_Soure("GET_POLICY_FLAT_RATE_COVER_BY_POLICY_COVER_ID", new string[,] { { "@POLICY_COVER_ID", policy_cover_id } });
                foreach (DataRow row in tbl.Rows)
                {
                    obj = new bl_policy_flat_rate.bl_policy_cover();
                    obj.PolicyCoverID = row["policy_cover_id"].ToString();
                    obj.PolicyID = row["policy_id"].ToString();
                    obj.CustomerID = row["cust_id"].ToString();
                    obj.PremiumByMode = Convert.ToInt32(row["premium_by_mode"].ToString());
                    obj.AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString());
                    obj.AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString());
                    obj.Age = Convert.ToInt32(row["age"].ToString());
                    obj.PayYear = Convert.ToInt32(row["pay_year"].ToString());
                    obj.PayUpToAge = Convert.ToInt32(row["pay_up_to_age"].ToString());
                    obj.AssuredUpToAge = Convert.ToInt32(row["assured_up_to_age"].ToString());
                    obj.AssuredYear = Convert.ToInt32(row["assured_year"].ToString());
                    obj.CoverTypeID = row["cover_type"].ToString();
                    //get cover name
                    bl_policy_flat_rate.bl_cover cover = da_policy_flat_rate.da_cover.GetCover(obj.CoverTypeID);
                    obj.CoverType = cover.Cover;

                    obj.SumInsured = Convert.ToDouble(row["sum_insured"].ToString());
                    obj.Remarks = row["remarks"].ToString();
                    obj.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                    obj.ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString());
                    obj.CreatedBy = row["created_by"].ToString();
                    obj.CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString());
                    //obj.UpdatedBy = row["updated_by"].ToString();
                    //obj.UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString());
                    arr_obj.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetCoverByPolicyCoverID] in class [da_policy_flat_rate => da_cover], Detail: " + ex.Message);
            }
            return arr_obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cover">Cover Object</param>
        /// <param name="option">save = add new record, update = update old record</param>
        /// <returns></returns>
        public static bool SaveCover(bl_policy_flat_rate.bl_policy_cover cover, string option)
        {
            bool status = false;
            try
            {
                if (option.Trim().ToLower() == "save")
                {

                    status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_FLAT_RATE_COVER", new string[,] { 
                    {"@POLICY_COVER_ID", cover.PolicyCoverID }, 
                    {"@POLICY_ID", cover.PolicyID},
                    {"@CUST_ID", cover.CustomerID},
                    {"@COVER_TYPE", cover.CoverType},
                    {"@AGE", cover.Age+""},
                    {"@PAY_YEAR", cover.PayYear+""},
                    {"@PAY_UP_TO_AGE", cover.PayUpToAge+""},
                    {"@ASSURED_YEAR", cover.AssuredYear+""},
                    {"@ASSURED_UP_TO_AGE", cover.AssuredUpToAge+""},
                    {"@ANNUAL_PREMIUM" , cover.AnnualPremium+""},
                    {"@ANNUAL_ORIGIN_PREMIUM", cover.AnnualOriginPremium+""},
                    {"@PREMIUM_BY_MODE", cover.PremiumByMode+""},
                    {"@PAY_MODE_ID", cover.PayModeID+""},
                    {"@SUM_INSURED", cover.SumInsured+""},
                    {"@REMARKS", cover.Remarks},
                    {"@EFFECTIVE_DATE", cover.EffectiveDate+""},
                    {"@EXPIRY_DATE", cover.ExpiryDate+""},
                    {"@CREATED_BY", cover.CreatedBy},
                    {"@CREATED_DATETIME", cover.CreatedDateTime+""}
                            
                }, "da_policy_flat_rate => SaveCover[Insert]");
                }
                else if (option.Trim().ToLower() == "update")
                {//update

                    status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_POLICY_FLAT_RATE_COVER_BY_COVER_ID", new string[,] { 
                    {"@POLICY_COVER_ID", cover.PolicyCoverID }, 
                    {"@POLICY_ID", cover.PolicyID},
                    {"@CUST_ID", cover.CustomerID},
                    {"@COVER_TYPE", cover.CoverType},
                    {"@AGE", cover.Age+""},
                    {"@PAY_YEAR", cover.PayYear+""},
                    {"@PAY_UP_TO_AGE", cover.PayUpToAge+""},
                    {"@ASSURED_YEAR", cover.AssuredYear+""},
                    {"@ASSURED_UP_TO_AGE", cover.AssuredUpToAge+""},
                    {"@ANNUAL_PREMIUM" , cover.AnnualPremium+""},
                    {"@ANNUAL_ORIGIN_PREMIUM", cover.AnnualOriginPremium+""},
                    {"@PREMIUM_BY_MODE", cover.PremiumByMode+""},
                    {"@PAY_MODE_ID", cover.PayModeID+""},
                    {"@SUM_INSURED", cover.SumInsured+""},
                    {"@REMARKS", cover.Remarks},
                    {"@EFFECTIVE_DATE", cover.EffectiveDate+""},
                    {"@EXPIRY_DATE", cover.ExpiryDate+""},
                    {"@UPDATED_BY", cover.UpdatedBy},
                    {"@UPDATED_DATETIME", cover.UpdatedDateTime+""}
                 }, "da_policy_flat_rate => SaveCover[Update]");
                }

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [SaveCover] in class [da_policy_flat_rate => da_cover], Detail: " + ex.Message);
                status = false;
            }
            return status;

        }

        public static bool DeleteCover(string cover_id, string user_login)
        {
            bool status = false;
            try
            {
                List<bl_policy_flat_rate.bl_policy_cover> arr_cover = GetCoverByPolicyCoverID(cover_id);
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_POLICY_FLAT_RATE_COVER_BY_COVER_ID", new string[,] { { "@POLICY_COVER_ID", cover_id }, {"@USER", user_login} }, "da_policy_flat_rate => DeleteCover");

                #region Save cover history
                /*
                if (status)
                {
                    foreach (bl_policy_flat_rate.bl_policy_cover cover in arr_cover)
                    {
                        da_policy_cover obj = new da_policy_cover();
                        obj.SaveCoverHistory(new bl_policy_flat_rate.bl_policy_cover_history() {
                            PolicyCoverID = cover.PolicyCoverID,
                            PolicyID = cover.PolicyID,
                            CoverType = cover.CoverTypeID,
                            CustomerID = cover.CustomerID,
                            Age = cover.Age,
                            PayYear = cover.PayYear,
                            PayUpToAge = cover.PayUpToAge,
                            AssuredYear = cover.AssuredYear,
                            AssuredUpToAge = cover.AssuredUpToAge,
                            SumInsured = cover.SumInsured,
                            PayModeID = cover.PayModeID,
                            PremiumByMode = cover.PremiumByMode,
                            AnnualOriginPremium = cover.AnnualOriginPremium,
                            AnnualPremium = cover.AnnualPremium,
                            EffectiveDate = cover.EffectiveDate,
                            ExpiryDate = cover.ExpiryDate,
                            Remarks = cover.Remarks,
                            CreatedBy = cover.CreatedBy,
                            CreatedDateTime = cover.CreatedDateTime,
                            UpdatedBy = cover.UpdatedBy,
                            UpdatedDateTime = cover.UpdatedDateTime,
                            Action = "DELETE",
                            ActionBy = cover.CreatedBy,
                            ActionDateTime = cover.CreatedDateTime
                        });
                    }
                }
                 * */
                #endregion
            }
            catch (Exception ex)
            {
                status = true;
                Log.AddExceptionToLog("Error function [DeleteCover] in class [da_policy_flat_rate => DeleteCover], Detail: " + ex.Message);
            }
            return status;
        }
    }

    /// <summary>
    /// By: Maneth
    /// Date: 27 09 2017 
    /// </summary>
    public class da_rider
    {
        public da_rider() { }

        public static List<bl_policy_flat_rate.bl_riders> GetRiderByPolicyID(string policy_id)
        {
            List<bl_policy_flat_rate.bl_riders> arr_obj = new List<bl_policy_flat_rate.bl_riders>();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_RIDERS_BY_POLICY_ID", new string[,] { { "@POLICY_ID", policy_id } });
                foreach (DataRow row in tbl.Rows)
                {
                    arr_obj.Add(new bl_policy_flat_rate.bl_riders()
                    {
                        PolicyID = row["policy_id"].ToString(),
                        PolicyRiderID = row["policy_rider_id"].ToString(),
                        CustomerID = row["cust_id"].ToString(),
                        FirstNameKh = row["first_name_kh"].ToString(),
                        LastNameKh = row["last_name_kh"].ToString(),
                        FirstNameEn = row["first_name_en"].ToString(),
                        LastNameEn = row["last_name_en"].ToString(),
                        IDTypeID = Convert.ToInt32(row["id_type_id"].ToString()),
                        IDCard = row["id_card"].ToString(),
                        Gender = Convert.ToInt32(row["gender"].ToString()),
                        BirthDate = Convert.ToDateTime(row["birth_date"].ToString()),
                        RiderType = Convert.ToInt32(row["rider_type"].ToString()),
                        Nationality = row["nationality"].ToString(),
                        Age = Convert.ToInt32(row["age"].ToString()),
                        PayYear = Convert.ToInt32(row["pay_year"].ToString()),
                        PayUpToAge = Convert.ToInt32(row["pay_up_to_age"].ToString()),
                        AssuredUpToAge = Convert.ToInt32(row["assured_up_to_age"].ToString()),
                        AssuredYear = Convert.ToInt32(row["assured_year"].ToString()),
                        Relationship = row["relationship"].ToString(),
                        SumInsured = Convert.ToDouble(row["sum_insured"].ToString()),
                        PayModeID = Convert.ToInt32(row["pay_mode_id"].ToString()),
                        AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString()),
                        AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString()),
                        PremiumByMode = Convert.ToDouble(row["premium_by_mode"].ToString()),
                        Discount = Convert.ToDouble(row["discount"].ToString()),
                        ExtraPercentage = Convert.ToDouble(row["extra_percentage"].ToString()),
                        ExtraPremium = Convert.ToDouble(row["extra_annual_premium"].ToString()),
                        ExtraPremiumByMode = Convert.ToDouble(row["extra_premium"].ToString()),
                        ExtraRate = Convert.ToDouble(row["extra_rate"].ToString()),
                        EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                        Remarks = row["remarks"].ToString(),
                        CreatedBy = row["created_by"].ToString(),
                        CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                        UpdatedBy = row["updated_by"].ToString(),
                        UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetRiderByPolicyID] in class [da_policy_flat_rate => da_rider], Detail: " + ex.Message);
            }

            return arr_obj;
        }
        public static List<bl_policy_flat_rate.bl_riders> GetRiderByPolicyRiderID(string policy_rider_id)
        {
            List<bl_policy_flat_rate.bl_riders> arr_obj = new List<bl_policy_flat_rate.bl_riders>();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_RIDERS_BY_POLICY_RIDER_ID", new string[,] { { "@POLICY_RIDER_ID", policy_rider_id } });
                foreach (DataRow row in tbl.Rows)
                {
                    arr_obj.Add(new bl_policy_flat_rate.bl_riders()
                    {
                        PolicyID = row["policy_id"].ToString(),
                        PolicyRiderID = row["policy_rider_id"].ToString(),
                        CustomerID = row["cust_id"].ToString(),
                        FirstNameKh = row["first_name_kh"].ToString(),
                        LastNameKh = row["last_name_kh"].ToString(),
                        FirstNameEn = row["first_name_en"].ToString(),
                        LastNameEn = row["last_name_en"].ToString(),
                        IDTypeID = Convert.ToInt32(row["id_type_id"].ToString()),
                        IDCard = row["id_card"].ToString(),
                        Gender = Convert.ToInt32(row["gender"].ToString()),
                        BirthDate = Convert.ToDateTime(row["birth_date"].ToString()),
                        RiderType = Convert.ToInt32(row["rider_type"].ToString()),
                        Nationality = row["nationality"].ToString(),
                        Age = Convert.ToInt32(row["age"].ToString()),
                        PayYear = Convert.ToInt32(row["pay_year"].ToString()),
                        PayUpToAge = Convert.ToInt32(row["pay_up_to_age"].ToString()),
                        AssuredUpToAge = Convert.ToInt32(row["assured_up_to_age"].ToString()),
                        AssuredYear = Convert.ToInt32(row["assured_year"].ToString()),
                        Relationship = row["relationship"].ToString(),
                        SumInsured = Convert.ToDouble(row["sum_insured"].ToString()),
                        PayModeID = Convert.ToInt32(row["pay_mode_id"].ToString()),
                        AnnualOriginPremium = Convert.ToDouble(row["annual_origin_premium"].ToString()),
                        AnnualPremium = Convert.ToDouble(row["annual_premium"].ToString()),
                        PremiumByMode = Convert.ToDouble(row["premium_by_mode"].ToString()),
                        Discount = Convert.ToDouble(row["discount"].ToString()),
                        ExtraPercentage = Convert.ToDouble(row["extra_percentage"].ToString()),
                        ExtraPremium = Convert.ToDouble(row["extra_annual_premium"].ToString()),
                        ExtraPremiumByMode = Convert.ToDouble(row["extra_premium"].ToString()),
                        ExtraRate = Convert.ToDouble(row["extra_rate"].ToString()),
                        EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()),
                        ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString()),
                        Remarks = row["remarks"].ToString(),
                        CreatedBy = row["created_by"].ToString(),
                        CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                        UpdatedBy = row["updated_by"].ToString(),
                        UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetRiderByPolicyRiderID] in class [da_policy_flat_rate => da_rider], Detail: " + ex.Message);
            }

            return arr_obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj_rider"></param>
        /// <param name="option">To insert [INSERT] or to update [UPDATE]</param>
        /// <returns></returns>
        public static bool SaveRiders(bl_policy_flat_rate.bl_riders obj_rider, string option)
        {
            bool status = false;
            try
            {
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_FLAT_RATE_RIDER", new string[,] { 
                    {"@OPTION", option.Trim().ToUpper()}, 
                    {"@POLICY_RIDER_ID", obj_rider.PolicyRiderID},
                    {"@POLICY_ID", obj_rider.PolicyID},
                    {"@CUST_ID", obj_rider.CustomerID},
                    {"@FIRST_NAME_KH", obj_rider.FirstNameKh},
                    {"@LAST_NAME_KH", obj_rider.LastNameKh},
                    {"@FIRST_NAME_EN", obj_rider.FirstNameEn},
                    {"@LAST_NAME_EN", obj_rider.LastNameEn},
                    {"@ID_TYPE_ID", obj_rider.IDTypeID+""},
                    {"@ID_CARD", obj_rider.IDCard},
                    {"@GENDER", obj_rider.Gender +""},
                    {"@BIRTH_DATE", obj_rider.BirthDate + ""}, 
                    {"@RIDER_TYPE", obj_rider.RiderType + ""},
                    {"@NATIONALITY", obj_rider.Nationality},
                    {"@AGE", obj_rider.Age + ""}, 
                    {"@PAY_YEAR", obj_rider.PayYear + ""},
                    {"@PAY_UP_TO_AGE", obj_rider.PayUpToAge + ""},
                    {"@ASSURED_YEAR", obj_rider.AssuredYear + ""},
                    {"@ASSURED_UP_TO_AGE", obj_rider.AssuredUpToAge + ""},
                    {"@RELATIONSHIP", obj_rider.Relationship + ""},
                    {"@SUM_INSURED", obj_rider.SumInsured + ""}, 
                    {"@PAY_MODE_ID", obj_rider.PayModeID +""},
                    {"@ANNUAL_PREMIUM", obj_rider.AnnualPremium + ""},
                    {"@ANNUAL_ORIGIN_PREMIUM", obj_rider.AnnualOriginPremium + ""},
                    {"@PREMIUM_BY_MODE", obj_rider.PremiumByMode + ""}, 
                    {"@DISCOUNT", obj_rider.Discount + ""}, 
                    {"@EXTRA_PERCENTAGE", obj_rider.ExtraPercentage + ""},
                    {"@EXTRA_RATE", obj_rider.ExtraRate + ""}, 
                    {"@EXTRA_PREMIUM", obj_rider.ExtraPremiumByMode + ""},
                    {"@EXTRA_ANNUAL_PREMIUM", obj_rider.ExtraPremium + ""},
                    {"@EFFECTIVE_DATE", obj_rider.EffectiveDate+""},
                    {"@EXPIRY_DATE", obj_rider.ExpiryDate + ""},
                    {"@REMARKS", obj_rider.Remarks}, 
                    {"@CREATED_BY", obj_rider.CreatedBy},
                    {"@CREATED_DATETIME", obj_rider.CreatedDateTime + ""},
                    {"@UPDATED_BY", obj_rider.UpdatedBy}, 
                    {"@UPDATED_DATETIME", obj_rider.UpdatedDateTime +""}

                
                }, "da_policy_flat_rate => SaveRiders[" + option + "]");


            }
            catch (Exception ex)
            {
                status = false;
                Log.AddExceptionToLog("Error function [SaveRiders] in class [da_policy_flat_rate => da_rider], Detail: " + ex.Message);
            }
            return status;
        }

        public static bool DeleteRiders(string rider_id, string user_login)
        {
            bool status = false;
            try
            {
               
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_POLICY_FLAT_RATE_RIDER_BY_RIDER_ID", new string[,] { { "@POLICY_RIDER_ID", rider_id }, {"@USER", user_login} }, "da_policy_flat_rate => DeleteRiders");
            }
            catch (Exception ex)
            {
                status = false;
                Log.AddExceptionToLog("Error function [DeleteRiders] in class [da_policy_flat_rate => da_rider], Detail: " + ex.Message);
            }

            return status;
        }

    }

    /// <summary>
    ///By: Maneth
    ///Date: 27 09 2017
    /// </summary>
    public class da_beneficiary
    {
        public da_beneficiary() { }

        public static List<bl_policy_flat_rate.bl_beneficiary> GetBeneficiaryByPolicyID(string policy_id)
        {
            List<bl_policy_flat_rate.bl_beneficiary> arr_obj = new List<bl_policy_flat_rate.bl_beneficiary>();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_BENEFICIARY_BY_POLICY_ID", new string[,] { {"@POLICY_ID", policy_id} });
                foreach (DataRow row in tbl.Rows)
                {
                    arr_obj.Add(new bl_policy_flat_rate.bl_beneficiary()
                    {
                        PolicyBenID = row["policy_ben_id"].ToString().Trim(),
                        PolicyID = row["policy_id"].ToString().Trim(),
                        CustomerID = row["cust_id"].ToString().Trim(),
                        FirstNameKh = row["first_name_kh"].ToString(),
                        LastNameKh = row["last_name_kh"].ToString(),
                        FirstNameEn = row["first_name_en"].ToString(),
                        LastNameEn = row["last_name_en"].ToString(),
                        Gender = Convert.ToInt32(row["gender"].ToString()),
                        Nationality = row["nationality"].ToString(),
                        IDTypeID = Convert.ToInt32(row["id_type_id"].ToString()),
                        IDCard = row["id_card"].ToString(),
                        Relationship = row["relationship"].ToString(),
                        BirthDate = Convert.ToDateTime(row["birth_date"].ToString()),
                        Percentage = Convert.ToDouble(row["percentage"].ToString()),
                        Remarks = row["remarks"].ToString(),
                        CreatedDateTime = Convert.ToDateTime(row["created_datetime"].ToString()),
                        CreatedBy = row["created_by"].ToString(),
                        UpdatedBy = row["updated_by"].ToString(),
                        UpdatedDateTime = Convert.ToDateTime(row["updated_datetime"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetBeneficiaryByPolicyID] in class [da_policy_flat_rate => da_beneficiary], Detail: " + ex.Message);
            }
            return arr_obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beneficiary"></param>
        /// <param name="option">[INSERT] OR [UPDATE]</param>
        /// <returns></returns>
        public static bool SaveBeneficiaries(bl_policy_flat_rate.bl_beneficiary beneficiary, string option)
        {
            bool status = false;
            try
            {
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_FLAT_RATE_BENEFICIARY", new string[,] { 
                    {"@OPTION", option},
                    {"@POLICY_BEN_ID", beneficiary.PolicyBenID.Trim() }, 
                    {"@POLICY_ID", beneficiary.PolicyID.Trim()},
                    {"@CUST_ID", beneficiary.CustomerID.Trim()},
                    {"@FIRST_NAME_KH", beneficiary.FirstNameKh},
                    {"@LAST_NAME_KH", beneficiary.LastNameKh},
                    {"@FIRST_NAME_EN", beneficiary.FirstNameEn},
                    {"@LAST_NAME_EN", beneficiary.LastNameEn},
                    {"@ID_TYPE_ID", beneficiary.IDTypeID+""},
                    {"@ID_CARD", beneficiary.IDCard},
                    {"@GENDER", beneficiary.Gender+""},
                    {"@NATIONALITY", beneficiary.Nationality},
                    {"@BIRTH_DATE", beneficiary.BirthDate+""},
                    {"@RELATIONSHIP", beneficiary.Relationship},
                    {"@PERCENTAGE", beneficiary.Percentage+""},
                    {"@REMARKS", beneficiary.Remarks},
                    {"@CREATED_BY", beneficiary.CreatedBy},
                    {"@CREATED_DATETIME", beneficiary.CreatedDateTime+""},
                    {"@UPDATED_BY", beneficiary.UpdatedBy},
                    {"@UPDATED_DATETIME", beneficiary.UpdatedDateTime+""}
                }, "da_policy_flat_rate => da_beneficiary => SaveBeneficiaries");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [SaveBeneficiaries] in class [da_policy_flat_rate => da_beneficiary], Detail: " + ex.Message);
                status = false;
            }
            return status;
        }

        public static bool DeleteBeneficiary(string policy_ben_id)
        {
            bool status = false;
            try
            {
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_DELETE_POLICY_FLAT_RATE_BENEFICIARY_BY_BENEFICIARY_ID",
                    new string[,] { { "@POLICY_BEN_ID" , policy_ben_id} }, 
                    "da_policy_flat_rate => da_beneficiary => DeleteBeneficiary");
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [DeleteBeneficiary] in class [da_policy_flat_rate => da_beneficiary], Detail: " + ex.Message);
                status = false;
            }
            return status;
        }
    }

    public class da_loan
    {
        public da_loan() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">[Save] to save record, [Update] to update record</param>
        /// <param name="loanObj">loan object</param>
        /// <returns></returns>
        public static bool SaveLoan(string option, bl_policy_flat_rate.bl_loan loanObj)
        {
            bool status=false;  
            try
            {
                status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_FLAT_RATE_LOAN",
                    new string[,] { 
                        {"@POLICY_ID", loanObj.PolicyID},
                        {"@LOAN_TYPE", loanObj.LoanType+""},
                        {"@INTEREST_RATE", loanObj.InterestRate + ""},
                        {"@TERM_YEAR", loanObj.TermYear+""},
                        {"@LOAN_EFFECTIVE_DATE", loanObj.LoanEffectiveDate + ""},
                        {"@LOAN", loanObj.Loan+""},
                        {"@OPTION", option.ToUpper()},
                        {"@CREATED_BY", loanObj.CreatedBy},
                        {"@CREATED_DATETIME", loanObj.CreatedDateTime +""},
                        {"@UPDATED_BY", loanObj.UpdatedBy},
                        {"@UPDATED_DATETIME", loanObj.UpdatedDateTime +""}
                    },
                    "da_policy_flat_rate => da_loan => SaveLoan");
            }
            catch(Exception ex)
            {
                Log.AddExceptionToLog("Error function [SaveLoan] in class [da_policy_flat_rate => SaveLoan], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
                status=false;
            }
            return status;
        }

        public static bl_policy_flat_rate.bl_loan GetLoan(string policyID)
        {
            bl_policy_flat_rate.bl_loan loanObj = new bl_policy_flat_rate.bl_loan();
            try
            {
                DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_LOAN_BY_POLICY_ID", new string[,] { { "@POLICY_ID", policyID } });
                if (tbl.Rows.Count>0)
                { 
                    var row = tbl.Rows[0];
                    loanObj.PolicyID = row["policy_ID"].ToString();
                    loanObj.LoanType = Convert.ToInt32(row["Loan_Type"].ToString());
                    loanObj.InterestRate = Convert.ToDouble(row["Interest_Rate"].ToString());
                    loanObj.TermYear = Convert.ToInt32(row["Term_Year"].ToString());
                    loanObj.LoanEffectiveDate = Convert.ToDateTime(row["Loan_Effective_Date"].ToString());
                    loanObj.Loan = Convert.ToDouble(row["loan"].ToString());
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetLoan] in class [da_policy_flat_rate => GetLoan(string policyID)], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
            }
            return loanObj;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="policy_number"></param>
    /// <param name="app_number"></param>
    /// <param name="name"></param>
    /// <param name="gender">[0:Female, 1:Male]</param>
    /// <param name="id_card"></param>
    /// <returns></returns>
    public static List<bl_policy_flat_rate.bl_policy_customer_search> GetPolicyCustomerSearch(string policy_number, string app_number, string name, int gender, string id_card , string policy_status)
    {
        List<bl_policy_flat_rate.bl_policy_customer_search> obj_arr = new List<bl_policy_flat_rate.bl_policy_customer_search>();
        try
        {
            foreach (DataRow row in DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_SEARCH_POLICY_BY_PARAMETERS", new string[,] { { "@POLICY_NUMBER", policy_number }, { "@NAME", name }, { "@APP_NUMBER", app_number }, { "@GENDER", gender + "" }, { "@ID_CARD", id_card }, {"@STATUS", policy_status} }).Rows)
            {
                obj_arr.Add(new bl_policy_flat_rate.bl_policy_customer_search() { 
                    PolicyID = row["policy_id"].ToString(),
                    PolicyNumber = row["policy_number"].ToString(),
                    First_Name = row["first_name_en"].ToString(),
                    Last_Name = row["last_name_en"].ToString(),
                    Gender = Convert.ToInt32(row["gender"].ToString()),
                    Birth_Date = Convert.ToDateTime(row["Birth_date"].ToString()),
                    ID_Card = row["id_card"].ToString()
                });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicyCustomerSearch] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return obj_arr;
    }
    /// <summary>
    /// Return as DataTable
    /// </summary>
    /// <param name="policy_number"></param>
    /// <param name="app_number"></param>
    /// <param name="name"></param>
    /// <param name="gender"></param>
    /// <param name="id_card"></param>
    /// <param name="policy_status"></param>
    /// <returns></returns>
    public static DataTable GetPolicyCustomerSearch(string table_name, string policy_number, string app_number, string name, int gender, string id_card, string policy_status)
    {
        DataTable tbl = new DataTable();
        try
        {
            tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_FLAT_RATE_SEARCH_POLICY_BY_PARAMETERS", new string[,] { { "@POLICY_NUMBER", policy_number }, { "@NAME", name }, { "@APP_NUMBER", app_number }, { "@GENDER", gender + "" }, { "@ID_CARD", id_card }, { "@STATUS", policy_status } });
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicyCustomerSearch] in class [da_policy_flat_rate], Detail: " + ex.Message);
        }
        return tbl;
    }
   
}