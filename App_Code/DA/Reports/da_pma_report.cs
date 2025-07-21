using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_pma_report
/// </summary>
public class da_pma_report
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
    private static DB db = new DB();
	public da_pma_report()
	{
		//
		// TODO: Add constructor logic here
		//
        _SUCCESS = false;
        _MESSAGE = "";
	}

    public static bool Save(bl_pma_report pma)
    {
        try
        {
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PMA_REPORT_INSERT", new string[,] {
            {"@ID", pma.ID},
            {"@POLICY_NUMBER", pma.PolicyNumber}, 
            {"@effective_date",pma.EffectiveDate+""}, 
            {"@pay_date",pma.PayDate+""}, 
            {"@sa", pma.SA+""}, 
            {"@annual_premium", pma.AnnualPremium+""}, 
            {"@premium", pma.Premium+""},
            {"@discount_amount", pma.DiscountAmount+""},
            {"@em", pma.EM+""},
            {"@amount", pma.Amount+""},
            {"@product_id", pma.ProductId},
            {"@PRODUCT_CODE", pma.ProductCode},
            {"@product_name", pma.ProductName},
            {"@PAY_MODE", pma.PayMode},
            {"@pay_year", pma.PayYear+""},
            {"@pay_lot", pma.PayLot+""},
            {"@report_date", pma.ReportDate+""},
            {"@CREATED_BY", pma.CreatedBy}, 
            {"@CREATED_ON", pma.CreatedOn+""},
            {"@number_policy", pma.NumberPolicy+""},
            {"@policy_range", pma.PolicyRange}
            }, "da_pma_report=>Save(bl_pma_report pma)");

            if (db.RowEffect == -1) //error
            {
                _MESSAGE = db.Message;
            }
            else
            {
                _MESSAGE = "Success";
            }
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;

            Log.AddExceptionToLog("Error function [Save(bl_pma_report pma)] in class [da_pma_report], detail: " + ex.Message + "==>" + ex.StackTrace);

        }

        return _SUCCESS;
    }
}