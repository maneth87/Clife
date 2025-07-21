using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Report
/// </summary>
public class Report
{
    public static DB db = new DB();
    public Report()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class GroupMicro
    {
        public GroupMicro() { }
        [Serializable]
        public class PremiumDetail
        {
            private double _period;

            public PremiumDetail()
            {
                //_period = GetCoveragePeriod();
            }
            public string ChannelId { get; set; }
            public string ChannelItemId { get; set; }

            public string GroupCode { get; set; }
            public string ChannelName { get; set; }
            public DateTime SubmittedDate { get; set; }
            public string CustomerNo { get; set; }
            public string PolicyNumber { get; set; }
            public string AgreementNumber { get; set; }
            public string CustomerName { get; set; }
            public string Gender { get; set; }
            public DateTime DOB { get; set; }
            public int Age { get; set; }
            public DateTime EffectivedDate { get; set; }
            public DateTime ExpireDate { get; set; }
            public double LoanAmount { get; set; }
            public double ExchangeRate { get; set; }
            public string Currency { get; set; }
            public double LoanAmountUSD { get; set; }
            public double InsuranceCost { get; set; }
            public double Premium { get; set; }
            public double PremiumRate { get; set; }
            public DateTime ReportDate { get; set; }
            //public double CoveragePeriod { get { return GetCoveragePeriod(); } set { _period = value; } }
            public double CoveragePeriod { get { return GetCoveragePeriod(); } }
            public double PremiumKh { get { return GetPremiumKh(); } }
            public string ProductId { get; set; }
            public string PolicyStatus { get; set; }
            public DateTime PolicyStatusDate { get; set; }
            private double GetPremiumKh()
            {

                return Math.Round(Premium * ExchangeRate, 2, MidpointRounding.AwayFromZero);

            }
            private double GetCoveragePeriod()
            {
                double period = 0;
                if (EffectivedDate != null && ExpireDate != null)
                {
                    period = ExpireDate.Subtract(EffectivedDate).TotalDays;
                    period = period >= 364 && period <= 366 ? 365 : period;
                }
                return period;

            }

        }
        [Serializable]
        public class PolicyDetail
        {
            public DateTime ReportDate { get; set; }
            public string ApplicationId { get; set; }
            public string ApplicationNumber { get; set; }
            public string PolicyId { get; set; }
            public string PolicyNumber { get; set; }
            public string CustomerId { get; set; }
            public string ChannelId { get; set; }
            public string ChannelItemId { get; set; }
            public string ChannelLocationId { get; set; }
            public string ChannelName { get; set; }
            public string OfficeCode { get; set; }
            public string OfficeName { get; set; }
            public string SaleAgentId { get; set; }
            public string SaleAgentNameEn { get; set; }
            public string SaleAgentNameKh { get; set; }
            public string CustomerNumber { get; set; }
            public int IdType { get; set; }
            public string IdTypeEn { get { return Helper.GetIDCardTypeText(IdType); } }
            public string IdTypeKh { get { return Helper.GetIDCardTypeTextKh(IdType); } }
            public string IdNumber { get; set; }
            public string FirstNameInEnglish { get; set; }
            public string LastNameInEnglish { get; set; }
            public string LastNameInKhmer { get; set; }
            public string FirstNameInKhmer { get; set; }
            public string FullNameEn { get { return LastNameInEnglish + " " + FirstNameInEnglish; } }
            public string FullNameKh { get { return LastNameInKhmer + " " + FirstNameInKhmer; } }
            public int Gender { get; set; }
            public string GenderEn { get { return Helper.GetGenderText(Gender, true); } }
            public string GenderKh { get { return Helper.GetGenderText(Gender, true, true); } }
            public DateTime DateOfBirth { get; set; }
            public string Nationality { get; set; }
            public string MaritalStatus { get; set; }
            public string Occupation { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string Package { get; set; }
            public double SumAssure { get; set; }
            public int TermOfCoverage { get; set; }
            public int PaymentPeriod { get; set; }
            public int PayMode { get; set; }
            public string PayModeEn { get { return Helper.GetPaymentModeEnglish(PayMode); } }
            public string PayModeKh { get { return Helper.GetPaymentModeInKhmer(PayMode); } }
            public double Premium { get; set; }
            public double AnnualPremium { get; set; }
            public double UserPremium { get; set; }
            public double BasicDiscountAmount { get; set; }
            public double BasicTotalAmount { get; set; }
            public int IssueAge { get; set; }
            public DateTime IssuedDate { get; set; }
            public DateTime EffectiveDate { get; set; }
            public DateTime ExpiryDate { get; set; }
            public DateTime MaturityDate { get; set; }
            public string PolicyStatus { get; set; }
            public DateTime PolicyStatusDate { get; set; }
            public string PolicyStatusRemarks { get; set; }
            public string RiderProductId { get; set; }
            public string RiderId { get; set; }
            public string RiderProductName { get; set; }
            public double RiderSumAssure { get; set; }
            public double RiderPremium { get; set; }
            public double RiderAnnualPremium { get; set; }
            public double RiderDiscountAmount { get; set; }
            public double RiderTotalAmount { get; set; }
            /// <summary>
            /// Basic amount + rider amount
            /// </summary>
            public double Amount { get; set; }
            /// <summary>
            /// Bisic discount amount + rider discount Amount
            /// </summary>
            public double DiscountAmount { get; set; }
            /// <summary>
            /// basic total amount + rider total amount
            /// </summary>
            public double TotalAmount { get; set; }
            public string BenId { get; set; }
            public string BenFullName { get; set; }
            public string BenAddress { get; set; }
            public string BenAge { get; set; }
            public double PercentageShared { get; set; }
            public string Relation { get; set; }
        }
        #region Function
        /// <summary>
        /// Get Premium Detail by Report Date
        /// </summary>
        /// <param name="fDate">Report Date From</param>
        /// <param name="tDate">Report Date To</param>
        /// <param name="reportType">[1=data table, 2= List Object ]</param>
        /// <returns></returns>
        public static Object GetPremiumDetail(DateTime fDate, DateTime tDate, string channelItemId, string productId, int reportType)
        {
            Object obj = new object();
            try
            {
                List<Report.GroupMicro.PremiumDetail> listObject = new List<PremiumDetail>();
                DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_PREMIUM_DETAIL_REPORT", new string[,] { 
            {"@f_date",fDate+"" },
            {"@t_date", tDate+""}, 
            {"@CHANNEL_ITEM_ID",channelItemId},
            {"@PRODUCT_ID", productId}
                
                }, "Report => GroupMicro => GetPremiumDetail(DateTime fDate, DateTime tDate, string channelItemId, string productId, int reportType)");

                if (reportType == 2)
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        listObject.Add(new PremiumDetail()
                        {
                            ChannelId = r["channel_id"].ToString(),
                            ChannelItemId = r["channel_item_id"].ToString(),
                            GroupCode = r["group_code"].ToString(),
                            ChannelName = r["channel_name"].ToString(),
                            SubmittedDate = Convert.ToDateTime(r["submitted_Date"].ToString()),
                            CustomerNo = r["customer_no"].ToString(),
                            PolicyNumber = r["policy_number"].ToString(),
                            AgreementNumber = r["agreement_number"].ToString(),
                            CustomerName = r["full_name_en"].ToString() == "" ? r["full_name_kh"].ToString() : r["full_name_en"].ToString(),
                            DOB = Convert.ToDateTime(r["dob"].ToString()),
                            Age = Convert.ToInt32(r["age"].ToString()),
                            Gender = Helper.GetGenderText(Convert.ToInt32(r["gender"].ToString()), false, false),
                            EffectivedDate = Convert.ToDateTime(r["effectived_date"].ToString()),
                            ExpireDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                            Currency = r["currency"].ToString(),
                            LoanAmount = r["currency"].ToString().ToUpper() != "USD" ? Convert.ToDouble(r["loan_Amount"].ToString()) : Math.Round((Convert.ToDouble(r["loan_Amount"].ToString()) * Convert.ToDouble(r["exchange_rate"].ToString())), 2, MidpointRounding.AwayFromZero),
                            ExchangeRate = Convert.ToDouble(r["exchange_rate"].ToString()),
                            LoanAmountUSD = Convert.ToDouble(r["loan_amount_usd"].ToString()),
                            InsuranceCost = Convert.ToDouble(r["insurance_cost"].ToString()),
                            Premium = Convert.ToDouble(r["premium"].ToString()),
                            PremiumRate = Convert.ToDouble(r["premium_rate"].ToString()),
                            ReportDate = Convert.ToDateTime(r["report_date"].ToString()),
                            PolicyStatus=r["policy_status"].ToString(),
                            PolicyStatusDate=Convert.ToDateTime(r["policy_status_date"].ToString())
                        });
                    }
                    obj = listObject;
                }
                else if (reportType == 1)
                {
                    obj = tbl;
                }
            }
            catch (Exception ex)
            {
                obj = new object();
                Log.AddExceptionToLog("Error function [Report => GroupMicro => GetPremiumDetail(DateTime fDate, DateTime tDate, string channelItemId, string productId, int reportType)], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return obj;
        }
        public enum ReportType { ListObject, DataTable }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fDate"> From Repport Date</param>
        /// <param name="tDate">To Report Date</param>
        /// <param name="customerNumber">[Optional] Customer Number</param>
        /// <param name="idNumber">[Optional] Id Number</param>
        /// <param name="customerName">[Optional] First Name or Last Name</param>
        /// <param name="policyNumber">[Optional] Policy Number</param>
        /// <param name="reportType">Return data base on selected report type</param>
        /// <returns></returns>
        public static Object GetPolicyDetail(DateTime fDate, DateTime tDate, string customerNumber, string idNumber, string customerName, string policyNumber, List< string> policyStatus, List<string>productId, ReportType reportType)
        {
            Object obj = new object();
            try
            {
                string status = "";
                string proId = "";
                foreach (string str in policyStatus)
                {
                    status += status != "" ? "," + str : str;
                }

                foreach (string str in productId)
                {
                    proId += proId != "" ? "," + str : str;
                }

                List<Report.GroupMicro.PolicyDetail> listObject = new List<PolicyDetail>();
                DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_POLICY_DETAIL_REPORT", new string[,] { 
                {"@f_report_date",fDate+"" },{"@t_report_date", tDate+""},
                {"@customer_number",customerNumber},
                {"@id_number",idNumber},
                {"@customer_name",customerName},
                {"@policy_number",policyNumber},
                {"@policy_status", status},
                 {"@product_id", proId}
                }, "Report => GroupMicro =>GetPolicyDetail(DateTime fDate, DateTime tDate, string customerNumber, string idNumber, string customerName, string policyNumber, ReportType reportType)");

                if (reportType == ReportType.ListObject)
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        listObject.Add(new PolicyDetail()
                        {
                            ReportDate = Convert.ToDateTime(r["report_date"].ToString()),
                            ChannelId = r["channel_id"].ToString(),
                            ChannelItemId = r["channel_item_id"].ToString(),
                            ChannelLocationId = r["channel_location_id"].ToString(),
                            ChannelName = r["channel_name"].ToString(),
                            OfficeCode = r["office_code"].ToString(),
                            OfficeName = r["office_name"].ToString(),
                            SaleAgentId = r["sale_agent_id"].ToString(),
                            SaleAgentNameEn = r["sale_agent_name_en"].ToString(),
                            SaleAgentNameKh = r["sale_agent_name_kh"].ToString(),
                            ApplicationId = r["application_id"].ToString(),
                            ApplicationNumber = r["application_number"].ToString(),
                            PolicyId = r["policy_id"].ToString(),
                            PolicyNumber = r["policy_number"].ToString(),
                            CustomerId = r["customer_id"].ToString(),
                            CustomerNumber = r["customer_number"].ToString(),
                            IdType = Convert.ToInt32(r["id_type"].ToString()),
                            IdNumber = r["id_number"].ToString(),
                            FirstNameInEnglish = r["first_name_in_english"].ToString(),
                            LastNameInEnglish = r["last_name_in_english"].ToString(),
                            LastNameInKhmer = r["last_name_in_khmer"].ToString(),
                            FirstNameInKhmer = r["first_name_in_khmer"].ToString(),
                            Gender = Convert.ToInt32(r["gender"].ToString()),
                            DateOfBirth = Convert.ToDateTime(r["date_of_birth"].ToString()),
                            Nationality = r["nationality"].ToString(),
                            MaritalStatus = r["marital_status"].ToString(),
                            Occupation = r["occupation"].ToString(),
                            PhoneNumber = r["phone_number"].ToString(),
                            Email = r["email"].ToString(),
                            Address = r["address"].ToString(),
                            ProductId = r["product_id"].ToString(),
                            ProductName = r["product_name"].ToString(),
                            Package = r["package"].ToString(),
                            RiderId = r["rider_id"].ToString(),
                            RiderProductId = r["rider_product_id"].ToString(),
                            RiderProductName = r["rider_product_name"].ToString(),
                            SumAssure = Convert.ToDouble(r["sum_assure"].ToString()),
                            Premium = Convert.ToDouble(r["premium"].ToString()),
                            AnnualPremium = Convert.ToDouble(r["annual_premium"].ToString()),
                            BasicDiscountAmount=Convert.ToDouble(r["basic_discount_amount"].ToString()),
                            BasicTotalAmount=Convert.ToDouble(r["basic_total_amount"].ToString()),
                            UserPremium = Convert.ToDouble(r["User_premium"].ToString()),
                            RiderPremium = Convert.ToDouble(r["rider_premium"].ToString()),
                            RiderAnnualPremium = Convert.ToDouble(r["rider_annual_premium"].ToString()),
                            RiderSumAssure = Convert.ToDouble(r["rider_sum_assure"].ToString()),
                            RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString()),
                            RiderTotalAmount = Convert.ToDouble(r["rider_total_amount"].ToString()),
                            Amount = Convert.ToDouble(r["amount"].ToString()),
                            DiscountAmount = Convert.ToDouble(r["discount_amount"].ToString()),
                            TotalAmount = Convert.ToDouble(r["total_amount"].ToString()),
                            IssueAge=Convert.ToInt32(r["Issue_Age"].ToString()),
                            IssuedDate=Convert.ToDateTime(r["issued_date"].ToString()),
                            EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                            ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                            MaturityDate = Convert.ToDateTime(r["Maturity_date"].ToString()),
                            TermOfCoverage = Convert.ToInt32(r["term_of_cover"].ToString()),
                            PaymentPeriod = Convert.ToInt32(r["Payment_period"].ToString()),
                            PayMode = Convert.ToInt32(r["pay_mode"].ToString()),
                            PolicyStatus=r["policy_status"].ToString(),
                            PolicyStatusDate=Convert.ToDateTime(r["policy_status_date"].ToString()),
                            PolicyStatusRemarks=r["policy_status_remarks"].ToString(),
                            BenId = r["ben_id"].ToString(),
                            BenFullName = r["ben_full_name"].ToString(),
                            BenAge = r["ben_age"].ToString(),
                            BenAddress = r["ben_address"].ToString(),
                            PercentageShared = Convert.ToDouble(r["percentage_shared"].ToString()),
                            Relation = r["relation"].ToString()
                        });
                    }
                    obj = listObject;
                }
                else if (reportType == ReportType.DataTable)
                {
                    obj = tbl;
                }
            }
            catch (Exception ex)
            {
                obj = new object();
                Log.AddExceptionToLog("Error function [Report => GroupMicro =>GetPolicyDetail(DateTime fDate, DateTime tDate, string customerNumber, string idNumber, string customerName, string policyNumber, ReportType reportType)], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return obj;
        }

        /// <summary>
        /// Get detail policy by policy number
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public static Object GetPolicyDetail(string policyNumber, ReportType reportType)
        {
            Object obj = new object();
            try
            {
                List<Report.GroupMicro.PolicyDetail> listObject = new List<PolicyDetail>();
                DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_DETAIL_POLICY_GET", new string[,] { 
                {"@policy_number",policyNumber}
                
                }, "Report => GroupMicro => GetPolicyDetail(string policyNumber, ReportType reportType)");

                if (reportType == ReportType.ListObject)
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        listObject.Add(new PolicyDetail()
                        {
                            ReportDate = Convert.ToDateTime(r["report_date"].ToString()),
                            ChannelId = r["channel_id"].ToString(),
                            ChannelItemId = r["channel_item_id"].ToString(),
                            ChannelLocationId = r["channel_location_id"].ToString(),
                            ChannelName = r["channel_name"].ToString(),
                            OfficeCode = r["office_code"].ToString(),
                            OfficeName = r["office_name"].ToString(),
                            SaleAgentId = r["sale_agent_id"].ToString(),
                            SaleAgentNameEn = r["sale_agent_name_en"].ToString(),
                            SaleAgentNameKh = r["sale_agent_name_kh"].ToString(),
                            ApplicationId = r["application_id"].ToString(),
                            ApplicationNumber = r["application_number"].ToString(),
                            PolicyId = r["policy_id"].ToString(),
                            PolicyNumber = r["policy_number"].ToString(),
                            CustomerId = r["customer_id"].ToString(),
                            CustomerNumber = r["customer_number"].ToString(),
                            IdType = Convert.ToInt32(r["id_type"].ToString()),
                            IdNumber = r["id_number"].ToString(),
                            FirstNameInEnglish = r["first_name_in_english"].ToString(),
                            LastNameInEnglish = r["last_name_in_english"].ToString(),
                            LastNameInKhmer = r["last_name_in_khmer"].ToString(),
                            FirstNameInKhmer = r["first_name_in_khmer"].ToString(),
                            Gender = Convert.ToInt32(r["gender"].ToString()),
                            DateOfBirth = Convert.ToDateTime(r["date_of_birth"].ToString()),
                            Nationality = r["nationality"].ToString(),
                            MaritalStatus = r["marital_status"].ToString(),
                            Occupation = r["occupation"].ToString(),
                            PhoneNumber = r["phone_number"].ToString(),
                            Email = r["email"].ToString(),
                            Address = r["address"].ToString(),
                            ProductId = r["product_id"].ToString(),
                            ProductName = r["product_name"].ToString(),
                            Package = r["package"].ToString(),
                            RiderId = r["rider_id"].ToString(),
                            RiderProductId = r["rider_product_id"].ToString(),
                            RiderProductName = r["rider_product_name"].ToString(),
                            SumAssure = Convert.ToDouble(r["sum_assure"].ToString()),
                            Premium = Convert.ToDouble(r["premium"].ToString()),
                            AnnualPremium = Convert.ToDouble(r["annual_premium"].ToString()),
                            BasicDiscountAmount = Convert.ToDouble(r["basic_discount_amount"].ToString()),
                            BasicTotalAmount = Convert.ToDouble(r["basic_total_amount"].ToString()),
                            UserPremium = Convert.ToDouble(r["User_premium"].ToString()),
                            RiderPremium = Convert.ToDouble(r["rider_premium"].ToString()),
                            RiderAnnualPremium = Convert.ToDouble(r["rider_annual_premium"].ToString()),
                            RiderSumAssure = Convert.ToDouble(r["rider_sum_assure"].ToString()),
                            RiderDiscountAmount = Convert.ToDouble(r["rider_discount_amount"].ToString()),
                            RiderTotalAmount = Convert.ToDouble(r["rider_total_amount"].ToString()),
                            Amount = Convert.ToDouble(r["amount"].ToString()),
                            DiscountAmount = Convert.ToDouble(r["discount_amount"].ToString()),
                            TotalAmount = Convert.ToDouble(r["total_amount"].ToString()),
                            IssueAge = Convert.ToInt32(r["Issue_Age"].ToString()),
                            IssuedDate = Convert.ToDateTime(r["issued_date"].ToString()),
                            EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                            ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                            MaturityDate = Convert.ToDateTime(r["Maturity_date"].ToString()),
                            TermOfCoverage = Convert.ToInt32(r["term_of_cover"].ToString()),
                            PaymentPeriod = Convert.ToInt32(r["Payment_period"].ToString()),
                            PayMode = Convert.ToInt32(r["pay_mode"].ToString()),
                            PolicyStatus = r["policy_status"].ToString(),
                            PolicyStatusDate = Convert.ToDateTime(r["policy_status_date"].ToString()),
                            PolicyStatusRemarks = r["policy_status_remarks"].ToString(),
                            BenId = r["ben_id"].ToString(),
                            BenFullName = r["ben_full_name"].ToString(),
                            BenAge = r["ben_age"].ToString(),
                            BenAddress = r["ben_address"].ToString(),
                            PercentageShared = Convert.ToDouble(r["percentage_shared"].ToString()),
                            Relation = r["relation"].ToString()
                        });
                    }
                    obj = listObject;
                }
                else if (reportType == ReportType.DataTable)
                {
                    obj = tbl;
                }
            }
            catch (Exception ex)
            {
                obj = new object();
                Log.AddExceptionToLog("Error function [Report => GroupMicro => GetPolicyDetail(string policyNumber, ReportType reportType)], detail:" + ex.Message + "=>" + ex.StackTrace);
            }
            return obj;
        }

        public static DataTable GetBunchSummaryForInvoice()
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_PAYMENT_BUNCH_SUMMARY_FOR_INVOICE", new string[,] { }, "Report => GroupMicro =>GetBunchSummaryForInvoice()");
            return tbl;
        }
        public static DataTable GetBunchDetailForInvoice()
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_PAYMENT_BUNCH_DETAIL_FOR_INVOICE", new string[,] { }, "Report => GroupMicro =>GetBunchDetailForInvoice()");
            return tbl;
        }
        /// <summary>
        /// Get invoice with pending status for issuing official receipt
        /// </summary>
        /// <returns></returns>
        public static DataTable GetInvoiceSummaryForOfficialReceipt()
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_INVOICE_SUMMARY_FOR_OFFICIAL_RECEIPT", new string[,] { }, "Report => GroupMicro =>GetInvoiceSummaryForOfficialReceipt()");
            return tbl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invDateF">Start Invoice Date</param>
        /// <param name="invDateT">End Invoice Date</param>
        /// <param name="chId">Channel Id</param>
        /// <param name="ciId"> Channel Item Id</param>
        /// <param name="invNo">Invoice Number</param>
        /// <returns></returns>
        public static DataTable GetInvoiceSummaryList(DateTime invDateF, DateTime invDateT, string chId, string ciId, string invNo)
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_INVOICE_SUMMARY_LIST", new string[,] {
            {"@invoice_date_from", invDateF+""},{"@invoice_date_to", invDateT+""},
            {"@channel_id", chId},{"@channel_item_id", ciId},{"@invoice_number", invNo}
            }, "Report => GroupMicro =>GetInvoiceSummaryList(DateTime invDateF, DateTime invDateT, string chId, string ciId, string invNo)");
            return tbl;
        }
        public static DataTable GetInvoiceDetailForOfficialReceipt()
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_INVOICE_DETAIL_FOR_OFFICIAL_RECEIPT", new string[,] { }, "Report => GroupMicro =>GetInvoiceDetailForOfficialReceipt()");
            return tbl;
        }

        /// <summary>
        /// Get Invoice by invoice number
        /// </summary>
        /// <param name="invNo">Invoice number</param>
        /// <returns></returns>
        public static DataTable GetInvoice(string invNo)
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_INVOICE_GET", new string[,] {
           {"@invoice_number", invNo}
            }, "Report => GroupMicro =>GetInvoice(string invNo)");
            return tbl;
        }


        #endregion Function


    }
}