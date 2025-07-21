using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_customer_lead_prefix
/// </summary>
public class da_customer_lead_prefix
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";
    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
	public da_customer_lead_prefix()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bl_customer_lead_prefix GetLastCustomerLeadPrefix()
    {
        bl_customer_lead_prefix prefix = new bl_customer_lead_prefix();
        DB db = new DB();
        try
        {

            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_CUSTOMER_LEAD_PREFIX_GET_LAST_PREFIX", new string[,]{
            
        }, "da_customer_lead_prefix => GetLastCustomerLeadPrefix()");
            if (db.RowEffect == -1)//error get data
            {
                _SUCCESS = false;
                _MESSAGE = db.Message;
            }
            else
            {
                if (tbl.Rows.Count > 0)
                {
                    var row = tbl.Rows[0];
                    prefix = new bl_customer_lead_prefix()
                    {
                        LeadPrefixId = row["LEAD_PREFIX_ID"].ToString(),
                        PREFIX1 = row["PREFIX1"].ToString(),
                        PREFIX2 = row["PREFIX2"].ToString(),
                        DIGITS = row["DIGITS"].ToString(),
                        STATUS = Convert.ToInt32(row["STATUS"].ToString())
                    };

                    _SUCCESS = true;
                    _MESSAGE = "Success";
                }
                else
                {
                    _SUCCESS = true;
                    _MESSAGE = "No records found.";
                }
            }
        }
        catch (Exception ex)
        {
            _MESSAGE = ex.Message;

            prefix = new bl_customer_lead_prefix();
            Log.AddExceptionToLog("Error function [bl_customer_lead_prefix GetLastCustomerLeadPrefix()] in class [da_customer_lead_prefix], detail: " + ex.Message );

        }
        return prefix;
    }

}