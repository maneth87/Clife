using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_micro_group_invoice
/// </summary>
public class da_micro_group_invoice
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }

    private static DB db = new DB();


	public da_micro_group_invoice()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public class summary
    {
       
        public summary()
        { 
        
        }
        public static bool Save(bl_micro_group_invoice.summary invoice)
        {
            bool result = false;
            try
            {
                var a = invoice;
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_INVOICE_SUMMARY_INSERT", new string[,] {
              {"@invoice_id", a.InvoiceId},
            {"@invoice_number", a.InvoiceNumber},
             {"@group_master_code", a.GroupMasterCode},
            {"@amount", a.Amount+""}, 
            {"@discount_amount", a.DiscountAmount+""},
            {"@total_amount",a.TotalAmount+""},{"@number_policy", a.NumberPolicy+""},
            {"@invoice_date",a.InvoiceDate+""},
            {"@exchange_rate_tax", a.ExchangeRateTax+""},
            {"@total_amount_kh", a.TotalAmountKh+""},
            {"@CREATED_BY", a.CreatedBy},
            {"@CREATED_ON", a.CreatedOn+""},
            {"@REMARKS",a.Remarks }
            }, "da_micro_group_invoice=>summary=>Save(bl_micro_group_invioce.summary bunch)");

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
                Log.AddExceptionToLog("Error function [Save(bl_micro_group_invioce.summary bunch)] in class [da_micro_group_invoice => summary], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Update Invoice summary by invoice number
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public static bool Update(bl_micro_group_invoice.summary invoice)
        {
            bool result = false;
            try
            {
                var a = invoice;
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_INVOICE_SUMMARY_UPDATE", new string[,] {
              {"@invoice_id", a.InvoiceId},
            {"@invoice_number", a.InvoiceNumber+""},
            // {"@bunch_id", a.BunchId},
             //{"@group_master_code", a.GroupMasterCode},
            {"@amount", a.Amount+""}, 
            {"@discount_amount", a.DiscountAmount+""},
            {"@total_amount",a.TotalAmount+""},
              {"@exchange_rate_tax", a.ExchangeRateTax+""},
            {"@total_amount_kh", a.TotalAmountKh+""},
            {"@UPDATED_BY", a.UpdateBy},
            {"@updated_on", a.UpdatedOn+""},
            {"@REMARKS",a.Remarks }
            }, "da_micro_group_invoice=>summary=>Update(bl_micro_group_invioce.summary bunch)");

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
                Log.AddExceptionToLog("Error function [Update(bl_micro_group_invioce.summary bunch)] in class [da_micro_group_invoice => summary], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invId">Invoice Id</param>
        ///<param name="official_receipt_id">Official Receipt Id to be insert</param> 
        /// <param name="remarks"></param>
        /// <param name="updatedBy"></param>
        /// <param name="updatedOn"></param>
        /// <returns></returns>
        public static bool UpdateOfficialReceipt(string invId, string official_receipt_id, string remarks, string updatedBy, DateTime updatedOn)
        {
            bool result = false;
            try
            {
              
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_INVOICE_SUMMARY_UPDATE_OFFICIAL_RECEIPT", new string[,] {
              {"@invoice_id", invId},
              {"@official_receipt_id", official_receipt_id},
            {"@UPDATED_BY", updatedBy},
            {"@updated_on", updatedOn+""},
            {"@REMARKS",remarks }
            }, "da_micro_group_invoice=>summary=>UpdateOfficialReceipt(string invId, string remarks, string updatedBy, DateTime updatedOn)");

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
                Log.AddExceptionToLog("Error function [UpdateOfficialReceipt(string invId, string remarks, string updatedBy, DateTime updatedOn)] in class [da_micro_group_invoice => summary], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }

        public static List<bl_micro_group_invoice.summary> GetInvoice()
        {
            return new List<bl_micro_group_invoice.summary>();
        }


       

    }
    public class detail
    {
        public detail()
        {

        }
        public static bool Save(bl_micro_group_invoice.detail detail)
        {
            bool result = false;
            try
            {
                var a = detail;
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_INVOICE_DETAIL_INSERT", new string[,] {
                    {"@invoice_detail_id", a.InvoiceDetailId},
                    {"@invoice_id", a.InvoiceId},
                    {"@bunch_detail_id", a.BunchDetailId},
                     {"@BUNCH_ID", a.BunchId},
                  
            }, "da_micro_group_invoice=>detail=>Save(bl_micro_group_invioce.detail detail)");

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
                Log.AddExceptionToLog("Error function [Save(bl_micro_group_invioce.detail detail)] in class [da_micro_group_invoice => detail], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }

    }
}