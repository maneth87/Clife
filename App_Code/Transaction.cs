using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Transaction
/// </summary>
public class Transaction
{
    private static DB db = new DB();
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";
    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }

    public enum TransactionType { INSERT, UPDATE, DELETE }

    public enum PageTransactionType { FIRST_LOAD, RE_LOAD, SEARCH, ADD, UPDATE, DELETE }
    public Transaction()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public class GroupMirco
    {
        public GroupMirco() { }
        public static bool RollBackUpload(string tranBy, DateTime tranDate)
        {
            bool result = false;
            try
            {
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_ROLEBACK", new string[,] {
            {"@tran_by", tranBy},{"@tran_date",tranDate+""}
            }, "Transaction => GroupMicro =>RollBackUpload(string tranBy, DateTime tranDate)");
                if (db.RowEffect >= 0)
                {
                    _SUCCESS = true;
                }
                else
                {
                    _SUCCESS = false;
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {

                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [RollBackUpload(string createdBy, DateTime createdOn) in class [Transaction => GroupMicro], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;
        }
        public static bool RollBackIssueInvoice(string tranBy, DateTime tranDate)
        {
            bool result = false;
            try
            {
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_ISSUE_INVOICE_ROLLBACK", new string[,] {
            {"@tran_by", tranBy},{"@tran_date",tranDate+""}
            }, "Transaction => GroupMicro =>RollBackIssueInvoice(string tranBy, DateTime tranDate)");
                if (db.RowEffect > 0)
                {
                    _SUCCESS = true;
                }
                else
                {
                    _SUCCESS = false;
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {

                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [RollBackIssueInvoice(string createdBy, DateTime createdOn) in class [Transaction => GroupMicro], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;
        }
        public static bool RollBackIssueOfficialReceipt(string tranBy, DateTime tranDate)
        {
            bool result = false;
            try
            {
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_ISSUE_OFFICIAL_RECEIPT_ROLLBACK", new string[,] {
            {"@tran_by", tranBy},{"@tran_date",tranDate+""}
            }, "Transaction => GroupMicro =>RollBackIssueOfficialReceipt(string tranBy, DateTime tranDate)");
                if (db.RowEffect > 0)
                {
                    _SUCCESS = true;
                }
                else
                {
                    _SUCCESS = false;
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {

                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [RollBackIssueOfficialReceipt(string createdBy, DateTime createdOn) in class [Transaction => GroupMicro], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;
        }
        public static bool ClearBackupTransactionIssuePolicyRecords(string tranBy, DateTime tranDate, string tranType)
        {
            bool result = false;
            try
            {
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_CLEAR_ISSUE_POLICY_BACKUP_TRAN", new string[,] {
            {"@tran_by", tranBy},{"@tran_date",tranDate+""},{"@tran_type",tranType}
            }, "Transaction => GroupMicro =>ClearTransactionIssuePolicyRecords(string tranBy, DateTime tranDate, string tranType)");
                if (db.RowEffect > 0)
                {
                    _SUCCESS = true;
                }
                else
                {
                    _SUCCESS = false;
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {

                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [ClearTransactionIssuePolicyRecords(string tranBy, DateTime tranDate, string tranType) in class [Transaction => GroupMicro], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;
        }
        public static bool ClearBackupTransactionIssueInvoiceRecords(string tranBy, DateTime tranDate, string tranType)
        {
            bool result = false;
            try
            {
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_CLEAR_ISSUE_INVOICE_BACKUP_TRAN", new string[,] {
            {"@tran_by", tranBy},{"@tran_date",tranDate+""},{"@tran_type",tranType}
            }, "Transaction => GroupMicro =>ClearTransactionIssuePolicyRecords(string tranBy, DateTime tranDate, string tranType)");
                if (db.RowEffect > 0)
                {
                    _SUCCESS = true;
                }
                else
                {
                    _SUCCESS = false;
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {

                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [ClearTransactionIssuePolicyRecords(string tranBy, DateTime tranDate, string tranType) in class [Transaction => GroupMicro], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;
        }

        public static bool ClearBackupTransactionIssueOfficialReceiptRecords(string tranBy, DateTime tranDate, string tranType)
        {
            bool result = false;
            try
            {
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_CLEAR_ISSUE_OFFICIAL_RECEIPT_BACKUP_TRAN", new string[,] {
            {"@tran_by", tranBy},{"@tran_date",tranDate+""},{"@tran_type",tranType}
            }, "Transaction => GroupMicro =>ClearBackupTransactionIssueOfficialReceiptRecords(string tranBy, DateTime tranDate, string tranType)");
                if (db.RowEffect > 0)
                {
                    _SUCCESS = true;
                }
                else
                {
                    _SUCCESS = false;
                    _MESSAGE = db.Message;
                }
            }
            catch (Exception ex)
            {

                _SUCCESS = false;
                _MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [ClearBackupTransactionIssueOfficialReceiptRecords(string tranBy, DateTime tranDate, string tranType) in class [Transaction => GroupMicro], Detail:" + ex.Message + "=>" + ex.StackTrace);

            }
            return _SUCCESS;
        }

        #region policy detail


        public class PolicyDetail
        {
            public PolicyDetail() { }
            public class Tran
            {
                public Tran() { }
                public string PolicyDetailId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool Backup(Tran tran)
            {
                try
                {
                    var a = tran;
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_DETAIL_BACKUP", new string[,] {
            {"@policy_detail_id", a.PolicyDetailId},
           {"@tran_by", a.TranBy},{"@tran_date", a.TranDate+""},{"@tran_type", a.TranType}
            }, "Transaction=>GroupMicro=>PolicyPayment=> Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>PolicyDetail], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;

            }
        }
        #endregion policy detail
        #region policy
        public class Policy
        {
            public Policy() { }
            public class Tran
            {
                public Tran() { }
                public string PolicyId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool Backup(Tran tran)
            {
                try
                {
                    var a = tran;
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_BACKUP", new string[,] {
            {"@policy_id", a.PolicyId},
           {"@tran_by", a.TranBy},{"@tran_date", a.TranDate+""},{"@tran_type", a.TranType}
            }, "Transaction=>GroupMicro=>Policy=> Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>Policy], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;

            }
        }
        #endregion policy

        #region Beneficiary
        public class Beneficiary
        {
            public Beneficiary() { }
            public class Tran
            {
                public Tran() { }
                public string BeneficiaryId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool Backup(Tran tran)
            {

                try
                {
                    var a = tran;
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_BENEFICIARY_BACKUP", new string[,] {
            {"@beneficiary_ID", a.BeneficiaryId},
           {"@tran_by", a.TranBy},{"@tran_date", a.TranDate+""},{"@tran_type", a.TranType}
            }, "Transaction=>GroupMicro=>Beneficiary=> Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>Beneficiary], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion Beneficiary

        #region Policy payment summary

        #endregion Policy payment summary

        #region Policy Payment
        public class PolicyPayment
        {
            public PolicyPayment()
            {

            }
            public class Tran
            {
                public Tran() { }
                public string PolicyPaymentId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }

            public static bool Backup(Tran tran)
            {

                try
                {
                    var a = tran;
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BACKUP", new string[,] {
            {"@policy_payment_id", a.PolicyPaymentId},
           {"@tran_by", a.TranBy},{"@tran_date", a.TranDate+""},{"@tran_type", a.TranType}
            }, "Transaction=>GroupMicro=>PolicyPayment=> Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>PolicyPayment], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
            /// <summary>
            /// Backup policy payment by bunch id
            /// </summary>
            /// <param name="bunchId"></param>
            /// <param name="tranBy"></param>
            /// <param name="tranDate"></param>
            /// <param name="tranType"></param>
            /// <returns></returns>
            public static bool Backup(string bunchId, string tranBy, DateTime tranDate, TransactionType tranType)
            {

                try
                {

                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BACKUP_BY_BUNCH_ID", new string[,] {
                 {"@bunch_id", bunchId},
                {"@tran_by", tranBy},{"@tran_date", tranDate+""},{"@tran_type", tranType.ToString()}
            }, "Transaction=>GroupMicro=>PolicyPayment=>Backup(string bunchId, string tranBy, DateTime tranDate, TransactionType tranType )");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(string bunchId, string tranBy, DateTime tranDate, TransactionType tranType )] in class [Transaction=>GroupMicro=>PolicyPayment], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion Policy Payment

        #region payment bunch summary
        public class PaymentBunchSummary
        {
            public PaymentBunchSummary() { }
            public class Tran
            {
                public Tran() { }
                public string BunchId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }

            public static bool Backup(Tran tran)
            {

                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_SUMMARY_BACKUP", new string[,] {
                    {"@bunch_id", tran.BunchId},{"@tran_by", tran.TranBy},{"@tran_date", tran.TranDate+""},{"@tran_type", tran.TranType}
            
                        }, "Transaction=>GroupMicro=>PaymentBunchSummary=>TranSummary=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>PaymentBunchSummary], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }

        }
        #endregion payment bunch summary

        #region payment bunch detail
        public class PaymentBunchDetail
        {
            public PaymentBunchDetail() { }
            public class Tran
            {
                public Tran() { }
                public string BunchDetailId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }

            public static bool Backup(Tran tran)
            {

                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_DETAIL_BACKUP", new string[,] {
             {"@bunch_detail_id", tran.BunchDetailId},
            {"@tran_by", tran.TranBy},{"@tran_date", tran.TranDate+""},{"@tran_type", tran.TranType}
            }, "Transaction=>GroupMicro=>PaymentBunchDetail=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>PaymentBunchDetail], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }

        }
        #endregion payment bunch detail

        #region InvoiceSummary
        public class InvoiceSummary
        {
            public InvoiceSummary() { }
            public class Tran
            {
                public Tran() { }
                public string InvoiceId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }

            public static bool Backup(Tran tran)
            {

                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_INVOICE_SUMMARY_BACKUP", new string[,] {
            {"@Invoice_id", tran.InvoiceId},{"@tran_by", tran.TranBy},{"@tran_date", tran.TranDate+""},{"@tran_type", tran.TranType}
            }, "Transaction=>GroupMicro=>InvoiceSummary=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>InvoiceSummary], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion InvoiceSummary

        #region InvoiceDetail
        public class InvoiceDetail
        {
            public InvoiceDetail() { }
            public class Tran
            {
                public Tran() { }
                public string InvoiceDetailId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }

            public static bool Backup(Tran tran)
            {

                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_INVOICE_DETAIL_BACKUP", new string[,] {
                        {"@invoice_detail_id", tran.InvoiceDetailId},
            {"@tran_by", tran.TranBy},{"@tran_date", tran.TranDate+""},{"@tran_type", tran.TranType}
            }, "Transaction=>GroupMicro=>InvoiceDetail=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>InvoiceDetail], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion InvoiceDetail

        #region Official Receipt
        public class Official_receipt
        {
            public Official_receipt() { }
            public class Tran
            {
                public Tran() { }
                public string OfficialReceiptId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool Backup(Tran tran)
            {

                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_OFFICIAL_RECEIPT_BACKUP", new string[,] {
                        {"@OFFICIAL_RECEIPT_ID", tran.OfficialReceiptId},
            {"@tran_by", tran.TranBy},{"@tran_date", tran.TranDate+""},{"@tran_type", tran.TranType}
            }, "Transaction=>GroupMicro=>Official_receipt=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>Official_receipt], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion Official Receipt

        #region Method Payment
        public class MethodPayment
        {
            public MethodPayment() { }
            public class Tran
            {
                public Tran() { }
                public string OfficialReceiptId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool Backup(Tran tran)
            {

                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_METHOD_PAYMENT_BACKUP", new string[,] {
                        {"@OFFICIAL_RECEIPT_ID", tran.OfficialReceiptId},
            {"@tran_by", tran.TranBy},{"@tran_date", tran.TranDate+""},{"@tran_type", tran.TranType}
            }, "Transaction=>GroupMicro=>MethodPayment=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>MethodPayment], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion Method Payment


        #region ApplicationNumber
        public class ApplicationNumber
        {
            public class Tran
            {
                public Tran() { }
                public string ApplicationNumber { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool BackUp(Tran objTran)
            {
                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_NUMBER_BACKUP", new string[,] {
                        {"@APPLICATION_NUMBER", objTran.ApplicationNumber},
            {"@tran_by", objTran.TranBy},{"@tran_date", objTran.TranDate+""},{"@tran_type", objTran.TranType}
            }, "Transaction=>GroupMicro=>ApplicationNumber=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>ApplicationNumber], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion ApplicationNumber

        #region Application
        public class Application
        {
            public class Tran
            {
                public Tran() { }
                public string ApplicationNumber { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool BackUp(Tran objTran)
            {
                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_APPLICATION_BACKUP", new string[,] {
                        {"@APPLICATION_NUMBER", objTran.ApplicationNumber},
            {"@tran_by", objTran.TranBy},{"@tran_date", objTran.TranDate+""},{"@tran_type", objTran.TranType}
            }, "Transaction=>GroupMicro=>Application=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>Application], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }
        }
        #endregion Application


        #region Certificate
        public class Certificate
        {
            public class Tran
            {
                public Tran() { }
                public string PolicyNumber { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool BackUp(Tran objTran)
            {
                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_CERTIFICATE_BACKUP", new string[,] {
                        {"@POLICY_NUMBER", objTran.PolicyNumber},
            {"@tran_by", objTran.TranBy},{"@tran_date", objTran.TranDate+""},{"@tran_type", objTran.TranType}
            }, "Transaction=>GroupMicro=>Certificate=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>Certificate], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }

        }
        #endregion Certificate

        #region Policy Rider
        public class PolicyRider
        {
            public class Tran
            {
                public Tran() { }
                public string PolicyRiderId { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool BackUp(Tran objTran)
            {
                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_RIDER_BACKUP", new string[,] {
                        {"@POLICY_RIDER_ID", objTran.PolicyRiderId},
            {"@tran_by", objTran.TranBy},{"@tran_date", objTran.TranDate+""},{"@tran_type", objTran.TranType}
            }, "Transaction=>GroupMicro=>PolicyRider=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>PolicyRider], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }

        }
        #endregion Policy Rider

        #region Policy Rider
        public class PMA
        {
            public class Tran
            {
                public Tran() { }
                public string ID { get; set; }
                public string TranBy { get; set; }
                public DateTime TranDate { get; set; }
                public string TranType { get; set; }
            }
            public static bool BackUp(Tran objTran)
            {
                try
                {
                    db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PMA_REPORT_BACKUP", new string[,] {
                        {"@id", objTran.ID},
            {"@tran_by", objTran.TranBy},{"@tran_date", objTran.TranDate+""},{"@tran_type", objTran.TranType}
            }, "Transaction=>GroupMicro=>PMA=>Backup(Tran tran)");

                    if (db.RowEffect > 0)
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

                    Log.AddExceptionToLog("Error function [Backup(Tran tran)] in class [Transaction=>GroupMicro=>PMA], detail: " + ex.Message + "==>" + ex.StackTrace);
                }
                return _SUCCESS;
            }

        }
        #endregion Policy Rider
    }
}