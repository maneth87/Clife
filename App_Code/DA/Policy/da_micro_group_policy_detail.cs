using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_group_policy_detail
/// </summary>
public class da_micro_group_policy_detail
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
    private static DB db = new DB();
    public da_micro_group_policy_detail()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool Save(bl_micro_group_policy_detail detail)
    {
        bool result = false;
        try
        {
            var a = detail;
            //DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_DETAIL_INSERT", new string[,] {
            {"@POLICY_DETAIL_ID", a.PolicyDetailId},
            {"@POLICY_ID", a.PolicyID}, 
            {"@issued_date",a.IssuedDate+""}, 
            {"@effective_date",  a.EffectivedDate+""},
            {"@expiry_date", a.ExpiryDate+""},
            {"@maturity_date", a.MaturityDate+""}, 
            {"@age", a.Age+""}, 
            {"@sum_assured", a.SumAssured+""}, 
            {"@pay_mode", a.PayMode+""}, 
            {"@payment_code", a.PaymentCode}, 
            {"@premium_rate", a.PremiumRate+""}, 
            {"@premium", a.Premium+""},
            {"@premium_riel", a.PremiumRiel+""},
            {"@annual_premium", a.AnnualPremium+""},
            {"@discount_amount", a.DiscountAmount+""},
            {"@total_amount", a.TotalAmount+""},
            {"@policy_status_remarks", a.PolicyStatusRemarks},
            {"@renewal_from", a.RenewalFrom},
            {"@PAY_PERIOD_TYPE", a.PayPeriodType},
            {"@cover_period_type",a.CoverPeriodType},
            {"@pay_year", a.PayYear+""},
            {"@cover_year", a.CoverYear+""},
            {"@frequency_reduce_year", a.FrequencyReduceYear+""},
            {"@reduce_rate", a.ReduceRate+""},
            {"@created_by", a.CreatedBy},
            {"@created_on", a.CreatedOn+""},
            {"@remarks",a.Remarks}
            }, "da_micro_group_policy_detail=>Save(bl_micro_group_policy_detail detail)");

            if (db.RowEffect == -1) //error
            {
                _MESSAGE = db.Message;
            }
            else
            {
                _MESSAGE = "Success";
            }
            _SUCCESS = result;
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            result = false;
            Log.AddExceptionToLog("Error function [Save(bl_micro_group_policy_detail detail)] in class [da_micro_group_policy_detail], detail: " + ex.Message + "==>" + ex.StackTrace);

        }

        return result;
    }

    public static List<bl_micro_group_policy_detail> GetPolicyDetailList(string customerNumber)
    {
        List<bl_micro_group_policy_detail> dList = new List<bl_micro_group_policy_detail>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_DETAIL_GET_BY_CUS_NO", new string[,] { 
                {"@customer_no", customerNumber}
            
            }, "da_micro_group_policy_detail.GetPolicyDetailList(string customerNumber)");
            if (db.RowEffect == 0)
            {
                _MESSAGE = "No record found.";
                _SUCCESS = true;
                dList = null;
            }
            else if (db.RowEffect < 0)
            {
                _MESSAGE = "Get Policy detail is getting error, please contact your system administrator.";
                _SUCCESS = false;
                dList = null;
            }
            else
            {
                _SUCCESS = true;
                _MESSAGE = tbl.Rows.Count + " Record(s) found.";

                foreach (DataRow r in tbl.Rows)
                {
                    dList.Add(new bl_micro_group_policy_detail() {
                    PolicyDetailId =r["policy_detail_id"].ToString(),
                    PolicyID=r["policy_id"].ToString(),
                    IssuedDate=Convert.ToDateTime(r["issued_date"].ToString()),
                    EffectivedDate=Convert.ToDateTime(r["effectived_date"].ToString()),
                    MaturityDate=Convert.ToDateTime(r["maturity_date"].ToString()),
                     ExpiryDate=Convert.ToDateTime(r["expiry_date"].ToString()),
                     Age=Convert.ToInt32(r["age"].ToString()),
                     SumAssured=Convert.ToDouble(r["sum_assured"].ToString()),
                     PayMode=Convert.ToInt32(r["pay_mode"].ToString()),
                     PaymentCode=r["payment_code"].ToString(),
                     PremiumRate=Convert.ToDouble(r["premium_rate"].ToString()),
                     Premium=Convert.ToDouble(r["premium"].ToString()),
                     PremiumRiel=Convert.ToDouble(r["premium_riel"].ToString()),
                     AnnualPremium=Convert.ToDouble(r["annual_premium"].ToString()),
                     DiscountAmount=Convert.ToDouble(r["discount_amount"].ToString()),
                     TotalAmount=Convert.ToDouble(r["total_amount"].ToString()),
                     PolicyStatusRemarks=r["policy_status_remarks"].ToString(),
                     RenewalFrom=r["renewal_from"].ToString(),
                     PayYear=Convert.ToInt32(r["pay_year"].ToString()),
                     CoverYear=Convert.ToInt32(r["cover_year"].ToString()),
                     PayPeriodType=r["pay_period_type"].ToString(),
                     CoverPeriodType=r["cover_period_type"].ToString()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            dList = null;
            _SUCCESS = false;
            _MESSAGE = ex.Message;

            Log.AddExceptionToLog("Error function [GetPolicyDetailList(string customerNumber)] in class [da_micro_group_policy_detail], detail: " + ex.Message + "==>" + ex.StackTrace);

        }

        return dList;

    }
    public static bool UpdatedAge(string policyId, int age, string updatedBy, DateTime updatedOn, string remarks)
    {
        bool result = false;
        try
        {
          
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_DETAIL_UPDATE_AGE", new string[,] {
            {"@POLICY_ID", policyId},
           
            {"@age", age+""}, 
           
            {"@updated_by", updatedBy},
            {"@updated_on", updatedOn+""},
            {"@remarks",remarks}
            }, "da_micro_group_policy_detail=>UpdatedAge(string policyId, int age, string updatedBy, DateTime updatedOn, string remarks)");

            if (db.RowEffect == -1) //error
            {
                _MESSAGE = db.Message;
            }
            else
            {
                _MESSAGE = "Success";
            }
            _SUCCESS = result;
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            result = false;
            Log.AddExceptionToLog("Error function [UpdatedAge(string policyId, int age, string updatedBy, DateTime updatedOn, string remarks)] in class [da_micro_group_policy_detail], detail: " + ex.Message + "==>" + ex.StackTrace);

        }

        return result;
    }
}