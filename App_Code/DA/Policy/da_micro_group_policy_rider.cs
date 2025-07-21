using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_group_policy_rider
/// </summary>
public class da_micro_group_policy_rider
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }

    private static DB db = new DB();
    private static string className = "da_micro_group_policy_rider";
	public da_micro_group_policy_rider()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bool Save(bl_micro_group_policy_rider payment)
    {
        bool result = false;
        try
        {
            var a = payment;
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_RIDER_INSERT", new string[,] {
            {"@policy_rider_id", a.PolicyRiderId},
            {"@POLICY_ID", a.PolicyId}, 
            {"@product_id", a.ProductId},
            {"@sum_assured", a.SumAssured+""},
            {"@premium_rate", a.PremiumRate+""},
            {"@premium", a.Premium+""},
            {"@premium_riel",a.PremiumRiel+""},
            {"@annual_premium", a.AnnualPremium+""},
           {"@discount_amount", a.DiscountAmount+""},
           {"@total_amount",a.TotalAmount+""},
           {"@rider_status",a.RiderStatus}, 
            {"@CREATED_BY", a.CreatedBy},
            {"@CREATED_ON", a.CreatedOn+""},
            {"@REMARKS",a.Remarks }
            }, className + "=>Save(bl_micro_group_policy_rider payment)");

            if (db.RowEffect == -1)
            {

            }
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
            Log.AddExceptionToLog("Error function [Save(bl_micro_group_policy_rider payment)] in class ["+className+"], detail: " + ex.Message + "==>" + ex.StackTrace);
        }
        return result;
    }
}