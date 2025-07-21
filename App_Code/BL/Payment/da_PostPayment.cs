
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


public class da_PostPayment
{
   
    public bool SUCCESS;
    public string MESSAGE;
   
    public da_PostPayment() {
        SUCCESS = false;
        MESSAGE = "";
      
    }
    

    public bool SavePostPayment(bl_PostPayment obj)
    {
        bool result = false;
        try
        {
            DB db = new DB();
          result=  db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_BANK_PAYMENT_TRANSACTION_REALTIME_INSERT", new string[,] {
            {"@payment_code", obj.PaymentCode},
            {"@Bill_No", obj.BillNo },
            {"@Biller_id", obj.BillerId },
            {"@Biller_name",obj.BillerName}, 
            {"@bill_amount",obj.BillAmount+""},
            {"@fee_charge", obj.FeeCharge+"" },
            {"@total_amount", obj.TotalAmount+"" },
            {"@transaction_amount",obj.TransactionAmount+"" },
            {"@transaction_type", obj.TransactionType },
            { "@transaction_reference_number", obj.TransactionReferenceNumber},
            {"@transaction_date",obj.TransactionDate+"" },
            {"@bank_name", obj.BankName },
            {"@created_by", obj.CreatedBy },
            {"@created_on", obj.CreatedOn+"" },
            {"@sys_remarks", obj.SysRemarks }
            }, "SavePostPayment(bl_PostPayment obj)");

            SUCCESS = result;
            MESSAGE = db.Message;
          
        }
        catch (Exception ex)
        {
         
            MESSAGE = ex.Message;
            SUCCESS = false;
            CallError("SavePostPayment(bl_PostPayment obj)", "da_PostPayment", ex);
        }
        return result;
    }

    public string GetExistingTransactionNumber(string transactionNumber)
    {
        string tranNo = "";
        try
        {
            DB db = new DB();
            DataTable dt = new DataTable();
            dt = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_BANK_PAYMENT_TRANSACTION_REALTIME_GET_BY_TRAN_REF_NO", new string[,] {
            {"@TRANSACTION_REFERENCE_NUMBER",transactionNumber }
            }, "da_PostPayment=>GetExistingTransactionNumber(string transactionNumber)");

            if(db.RowEffect>=0)
            {
               if(dt.Rows.Count>0)
                {
                    tranNo = dt.Rows[0]["TRANSACTION_REFERENCE_NUMBER"].ToString();
                }
               else
                {
                    tranNo = "";
                }
                MESSAGE = db.Message;
                SUCCESS = true;
            }
            else
            {
               
                MESSAGE = db.Message;
                tranNo = transactionNumber;
            }
        }
        catch(Exception ex)
        {
            tranNo = transactionNumber;
            //CODE = "0";
            //MESSAGE = ex.Message;
            //SUCCESS = false;
            //Log.AddExceptionToLog("Error function [SavePostPayment(bl_PostPayment obj)] in class [da_PostPayment], detail: " + ex.Message + " ==> " + ex.StackTrace);
            CallError("GetExistingTransactionNumber(string transactionNumber)", "da_PostPayment", ex);
        }
        return tranNo;
    }


    public bl_PostPayment GetPostPayment(string paymentCode)
    {
        bl_PostPayment postPayment = new bl_PostPayment();
        try
        {
            DB db = new DB();
            DataTable dt = new DataTable();
            dt = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_BANK_PAYMENT_TRANSACTION_REALTIME_GET_BY_PAYMENT_CODE", new string[,] {
            {"@PAYMENT_CODE",paymentCode }
            }, "da_PostPayment=> GetPostPayment(string paymentCode)");

            if (db.RowEffect >= 0)
            {
                if (dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    postPayment.Id = r["id"].ToString();
                    postPayment.BillNo = r["bill_no"].ToString();
                    postPayment.BillerId = r["biller_id"].ToString();
                    postPayment.BillerName = r["biller_name"].ToString();
                    postPayment.BillAmount = Convert.ToDouble(r["bill_amount"].ToString());
                    postPayment.FeeCharge = Convert.ToDouble(r["fee_charge"].ToString());
                    postPayment.TotalAmount = Convert.ToDouble(r["total_amount"].ToString());
                    postPayment.TransactionAmount = Convert.ToDouble(r["transaction_amount"].ToString());
                    postPayment.TransactionType = r["transaction_type"].ToString();
                    postPayment.TransactionReferenceNumber = r["TRANSACTION_REFERENCE_NUMBER"].ToString();
                    postPayment.TransactionDate = Convert.ToDateTime(r["transaction_date"].ToString());
                    postPayment.BankName = r["bank_name"].ToString();
                    postPayment.AmountUsedFlag = Convert.ToInt32(r["amount_used_flag"].ToString());
                    postPayment.AmountUsedBy = r["amount_used_by"].ToString();
                    postPayment.AmountUsedDate = Convert.ToDateTime(r["amount_used_date"].ToString());
                }
                else
                {
                    postPayment = new bl_PostPayment();
                }
                MESSAGE = db.Message;
                SUCCESS = true;
            }
            else
            {

                MESSAGE = db.Message;
                postPayment = new bl_PostPayment();
            }
        }
        catch (Exception ex)
        {
            postPayment = new bl_PostPayment();
            CallError(" GetPostPayment(string paymentCode)", "da_PostPayment", ex);
        }
        return postPayment;
    }
    /// <summary>
    /// Get Post payment by payment code and amount used flag = null or zero
    /// </summary>
    /// <param name="paymentCode"></param>
    /// <returns></returns>
    public bl_PostPayment GetPostPaymentAmountNotUse(string paymentCode)
    {
        bl_PostPayment postPayment = new bl_PostPayment();
        try
        {
            DB db = new DB();
            DataTable dt = new DataTable();
            dt = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_BANK_PAYMENT_TRANSACTION_REALTIME_GET_BY_PAYMENT_CODE_AMOUNT_NOT_USED", new string[,] {
            {"@PAYMENT_CODE",paymentCode }
            }, "da_PostPayment=> GetPostPaymentAmountNotUse(string paymentCode)");

            if (db.RowEffect >= 0)
            {
                if (dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    postPayment.Id = r["id"].ToString();
                    postPayment.BillNo = r["bill_no"].ToString();
                    postPayment.BillerId = r["biller_id"].ToString();
                    postPayment.BillerName = r["biller_name"].ToString();
                    postPayment.BillAmount = Convert.ToDouble(r["bill_amount"].ToString());
                    postPayment.FeeCharge = Convert.ToDouble(r["fee_charge"].ToString());
                    postPayment.TotalAmount = Convert.ToDouble(r["total_amount"].ToString());
                    postPayment.TransactionAmount = Convert.ToDouble(r["transaction_amount"].ToString());
                    postPayment.TransactionType = r["transaction_type"].ToString();
                    postPayment.TransactionReferenceNumber = r["TRANSACTION_REFERENCE_NUMBER"].ToString();
                    postPayment.TransactionDate = Convert.ToDateTime(r["transaction_date"].ToString());
                    postPayment.BankName = r["bank_name"].ToString();
                    postPayment.AmountUsedFlag = Convert.ToInt32(r["amount_used_flag"].ToString());
                    postPayment.AmountUsedBy = r["amount_used_by"].ToString();
                    postPayment.AmountUsedDate = Convert.ToDateTime(r["amount_used_date"].ToString());
                }
                else
                {
                    postPayment = null;// new bl_PostPayment();
                }
                MESSAGE = db.Message;
                SUCCESS = true;

               
            }
            else
            {

                MESSAGE = "Get Postpayment fail, please contact your system administrator.";
                postPayment = null;// new bl_PostPayment();
            }
        }
        catch (Exception ex)
        {
            SUCCESS = false;
            MESSAGE = "Get Postpayment fail, please contact your system administrator.";
            postPayment = null;// new bl_PostPayment();
            CallError("GetPostPaymentAmountNotUse(string paymentCode)", "da_PostPayment", ex);
        }
        return postPayment;
    }
    /// <summary>
    /// Update amount used flag by payment code
    /// </summary>
    /// <param name="paymentCode"></param>
    /// <param name="amountUsedFlag"></param>
    /// <param name="amountUsedBy"></param>
    /// <param name="amountUsedDate"></param>
    /// <returns></returns>
    public bool UpdateAmountUsed(string paymentCode, int amountUsedFlag, string amountUsedBy, DateTime amountUsedDate)
    {
        bool updated = false;
        try
        {
            DB db = new DB();
            updated = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_BANK_PAYMENT_TRANSACTION_REALTIME_UPDATE_USAGE_AMOUNT", new string[,] { 
            {"@payment_code",paymentCode},
            {"@amount_used_flag", amountUsedFlag+""},
            {"@amount_used_by", amountUsedBy},
            {"@amount_used_date", amountUsedDate+""}
            }, "da_PostPayment=>UpdateAmountUsed(string paymentCode, int amountUsedFlag, string amountUsedBy, DateTime amountUsedDate)");
           
            if (db.RowEffect > 0)
            {
                MESSAGE = "Updated successfully.";
            }
            else if (db.RowEffect == 0)
            {

                MESSAGE = "No record found for updating.";
            }
            else
            {
                MESSAGE = "Updated fail. Please contact your system administrator.";
            }
            SUCCESS = updated;
        }
        catch (Exception ex)
        {
            updated = false;
            CallError("UpdateAmountUsed(string paymentCode, int amountUsedFlag, string amountUsedBy, DateTime amountUsedDate)", "da_PostPayment", ex);
        }
        return updated;
    }
    public bool DeletePostpayment(string id)
    {
        bool updated = false;
        try
        {
            DB db = new DB();
            updated = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_BANK_PAYMENT_TRANSACTION_REALTIME_DEL_BY_ID", new string[,] { 
            {"@id",id}
            }, "da_PostPayment=>DeletePostpayment(string id)");

            if (db.RowEffect > 0)
            {
                MESSAGE = "Deleted successfully.";
            }
            else if (db.RowEffect == 0)
            {

                MESSAGE = "No record found for deleting.";
            }
            else
            {
                MESSAGE = "Deleted fail. Please contact your system administrator.";
            }
            SUCCESS = updated;
        }
        catch (Exception ex)
        {
            updated = false;
            CallError("DeletePostpayment(string id)", "da_PostPayment", ex);
        }
        return updated;
    }
    private void CallError(string functionName, string className, Exception errorMessage)
    {
     
        MESSAGE = errorMessage.Message;
        SUCCESS = false;
        Log.AddExceptionToLog("Error function ["+ functionName +"] in class ["+  className +"], detail: " + errorMessage.Message + " ==> " + errorMessage.StackTrace);

    }
}