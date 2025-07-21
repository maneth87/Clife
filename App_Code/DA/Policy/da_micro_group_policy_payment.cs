using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_micro_group_policy_payment
/// </summary>
public class da_micro_group_policy_payment
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }

    private static DB db = new DB();
    public da_micro_group_policy_payment()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool Save(bl_micro_group_policy_payment payment)
    {
        bool result = false;
        try
        {
            var a = payment;
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_INSERT", new string[,] {
            {"@policy_payment_id", a.PolicyPaymentId},
            {"@POLICY_DETAIL_ID", a.PolicyDetailId}, 
            {"@pay_mode", a.PayMode+""},
            {"@user_premium", a.UserPremium+""},
            {"@amount", a.Amount+""},
            {"@amount_riel", a.AmountRiel+""},
            {"@discount_amount", a.DiscountAmount+""},
            {"@total_amount",a.TotalAmount+""},
            {"@return_amount", a.ReturnAmount+""},
           {"@POLICY_STATUS", a.PolicyStatus},
           {"@due_date",a.DueDate+""},{"@pay_date",a.PayDate+""}, {"@next_due_date", a.NextDueDate+""},
           {"@premium_year", a.PremiumYear+""},{"@premium_lot",a.PremiumLot+""},{"@office_id",a.OfficeId},{"@transaction_type",a.TransactionType},{"@transaction_ref", a.TransactionRef},
           {"@pay_status", a.PayStatus+""},{"@report_date", a.ReportDate+""},
            {"@CREATED_BY", a.CreatedBy},
            {"@CREATED_ON", a.CreatedOn+""},
            {"@REMARKS",a.Remarks }
            }, "da_micro_group_policy_payment=>Save(bl_micro_group_policy_payment payment)");


            if (result)
            {
                _MESSAGE = "Success";
                _SUCCESS = true;
            }
            else
            {
                _MESSAGE = db.Message;
                _SUCCESS = false;
            }

        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            result = false;
            Log.AddExceptionToLog("Error function [Save(bl_micro_group_policy_payment payment)] in class [da_micro_group_policy_payment], detail: " + ex.Message + "==>" + ex.StackTrace);
        }
        return result;
    }

    /// <summary>
    /// Update Payment Date by bunch id
    /// </summary>
    /// <param name="bunchId"></param>
    /// <param name="newPaydate"></param>
    /// <returns></returns>
    public static bool UpdatePaymentDate(string bunchId, DateTime newPaydate)
    {
        bool result = false;
        try
        {
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_UPDATE_PAYMENT_DATE", new string[,] {
            {"@BUNCH_ID", bunchId},
            {"@NEW_PAY_DATE", newPaydate+""}
            }, "da_micro_group_policy_payment=>UpdatePaymentDate(string bunchId, DateTime newPaydate)");


            if (result)
            {
                _MESSAGE = "Success";
                _SUCCESS = true;
            }
            else
            {
                _MESSAGE = db.Message;
                _SUCCESS = false;
            }

        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            result = false;

            Log.AddExceptionToLog("Error function [UpdatePaymentDate(string bunchId, DateTime newPaydate)] in class [da_micro_group_policy_payment], detail: " + ex.Message);

        }
        return result;
    }

}