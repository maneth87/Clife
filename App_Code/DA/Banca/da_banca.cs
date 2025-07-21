

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


public class da_banca
{
    public static bool SUCCESS;
    public static string MESSAGE;
    private static DB db = new DB();

    public da_banca()
    {
        da_banca.SUCCESS = false;
        da_banca.MESSAGE = "";
    }

    public static List<bl_daily_insurance_booking_htb> GetDailyInsuranceBookingHTB(DateTime START_DATE, DateTime TO_DATE)
    {
        List<bl_daily_insurance_booking_htb> insuranceBookingHtb = new List<bl_daily_insurance_booking_htb>();
        try
        {
            DataTable dataTable = new DataTable();
            dataTable = da_banca.db.GetData(AppConfiguration.GetConnectionString(), "SP_MICRO_POLICY_GET_INSURANCE_BOOKING", new string[2, 2]
      {
        {
          "@ISSUED_DATE_F",START_DATE+""
        },
        {
          "@ISSUED_DATE_T", TO_DATE+""
        }
      }, "da_banca=>GetDailyInsuranceBookingHTB(DateTime  START_DATE, DateTime TO_DATE)");

            bl_daily_insurance_booking_htb obj = new bl_daily_insurance_booking_htb();
           
            foreach (DataRow row in dataTable.Rows)
            {
               obj.  BranchCode = row["office_code"].ToString();
                   obj. BranchName = row["office_name"].ToString();
                  obj.  ApplicationID = row["application_id"].ToString();
                   obj. ClientType = row["client_type"].ToString();
                  obj.  InsuranceApplicationId = row["application_number"].ToString();
                  obj.  CertificateNumber = row["policy_number"].ToString();
                  obj.  PaymentReferenceNo = row["payment_reference_no"].ToString();
                   obj. ReferralStaffId = row["referral_staff_id"].ToString();
                   obj. ReferralStaffName = row["referral_staff_name"].ToString();
                   obj. ReferralStaffPosition = row["referral_staff_position"].ToString();
                   obj. ClientCIF = row["cif"].ToString();
                   obj. ClientNameENG = row["last_name_in_english"].ToString() + " " + row["first_name_in_english"].ToString();
                   obj. ClientNameKHM = string.Concat(row["last_name_in_khmer"].ToString()+ " ", row["first_name_in_khmer"].ToString());
                   obj. ClientDoB = Convert.ToDateTime(row["date_of_birth"].ToString()).ToString("dd-MMM-yyyy");
                   obj. ClientPhoneNumber = row["phone_number"].ToString();
                   obj. ClientGender = row["gender"].ToString() == "1" ? "MALE" : "Female";
                   obj. DocumentType = row["id_type_text"].ToString();
                   obj. DocumentId = row["id_number"].ToString();
                   obj. ClientProvince = row["province_en"].ToString();
                   obj. ClientDistrict = row["district_en"].ToString();
                    obj.ClientCommune = row["commune_en"].ToString();
                    obj.ClientVillage = row["village_en"].ToString();
                    obj.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()).ToString("dd-MMM-yyyy");
                   obj. MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString()).ToString("dd-MMM-yyyy");
                    obj.InsuranceTenor = Convert.ToInt32(row["TERM_OF_COVER"].ToString());
                    obj.Premium = Convert.ToDouble(row["amount"].ToString());
                    obj.Currency = row["currency"].ToString();
                    obj.InsuranceType = "Micro Insurance";
                   obj. PackageType = row["package"].ToString();
                   obj. InsuranceStatus = row["policy_status"].ToString() == "IF" ? "Approved" : "";
                    obj.ReferralFee = Convert.ToDouble(row["referral_fee"].ToString());
                   obj. ReferralIncentive = Convert.ToDouble(row["referral_incentive"].ToString());
                   obj. IAName = row["agent_name_en"].ToString();
                    obj.ClientStatus = row["Policy_status_remarks"].ToString();
                    obj.ReferredDate = Convert.ToDateTime(row["referred_date"].ToString()).ToString("dd-MMM-yyyy");
                   obj. IssuedDate = Convert.ToDateTime(row["issued_date"].ToString()).ToString("dd-MMM-yyyy");
               insuranceBookingHtb.Add(obj);
            //    insuranceBookingHtb.Add(new bl_daily_insurance_booking_htb()
            //    {
            //        BranchCode = row["office_code"].ToString(),
            //        BranchName = row["office_name"].ToString(),
            //        ApplicationID = row["application_id"].ToString(),
            //        ClientType = row["client_type"].ToString(),
            //        InsuranceApplicationId = row["application_number"].ToString(),
            //        CertificateNumber = row["policy_number"].ToString(),
            //        PaymentReferenceNo = row["payment_reference_no"].ToString(),
            //        ReferralStaffId = row["referral_staff_id"].ToString(),
            //        ReferralStaffName = row["referral_staff_name"].ToString(),
            //        ReferralStaffPosition = row["referral_staff_position"].ToString(),
            //        ClientCIF = row["cif"].ToString(),
            //        ClientNameENG = row["last_name_in_english"].ToString() + " " + row["first_name_in_english"].ToString(),
            //        ClientNameKHM = string.Concat(row["last_name_in_khmer"].ToString(), " ", row["first_name_in_khmer"].ToString(),
            //        ClientDoB = Convert.ToDateTime(row["date_of_birth"].ToString()).ToString("dd-MMM-yyyy"),
            //        ClientPhoneNumber = row["phone_number"].ToString(),
            //        ClientGender = row["gender"].ToString() == "1" ? "MALE" : "Female",
            //        DocumentType = row["id_type_text"].ToString(),
            //        DocumentId = row["id_number"].ToString(),
            //        ClientProvince = row["province_en"].ToString(),
            //        ClientDistrict = row["district_en"].ToString(),
            //        ClientCommune = row["commune_en"].ToString(),
            //        ClientVillage = row["village_en"].ToString(),
            //        EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()).ToString("dd-MMM-yyyy"),
            //        MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString()).ToString("dd-MMM-yyyy"),
            //        InsuranceTenor = Convert.ToInt32(row["TERM_OF_COVER"].ToString()),
            //        Premium = Convert.ToDouble(row["amount"].ToString()),
            //        Currency = row["currency"].ToString(),
            //        InsuranceType = "Micro Insurance",
            //        PackageType = row["package"].ToString(),
            //        InsuranceStatus = row["policy_status"].ToString() == "IF" ? "Approved" : "",
            //        ReferralFee = Convert.ToDouble(row["referral_fee"].ToString()),
            //        ReferralIncentive = Convert.ToDouble(row["referral_incentive"].ToString()),
            //        IAName = row["agent_name_en"].ToString(),
            //        ClientStatus = row["Policy_status_remarks"].ToString(),
            //        ReferredDate = Convert.ToDateTime(row["referred_date"].ToString()).ToString("dd-MMM-yyyy"),
            //        IssuedDate = Convert.ToDateTime(row["issued_date"].ToString()).ToString("dd-MMM-yyyy"))
            //    });
            }
            da_banca.SUCCESS = true;
            da_banca.MESSAGE = "Success";
        }
        catch (Exception ex)
        {
            insuranceBookingHtb = new List<bl_daily_insurance_booking_htb>();
            da_banca.SUCCESS = false;
            da_banca.MESSAGE = ex.Message;
        }
        return insuranceBookingHtb;
    }

    public static DataTable GetDataInsuranceBookingHTB(DateTime START_DATE, DateTime TO_DATE)
    {
        DataTable dataTable = new DataTable();
        DataTable insuranceBookingHtb;
        try
        {
            insuranceBookingHtb = da_banca.db.GetData(AppConfiguration.GetConnectionString(), "SP_MICRO_POLICY_GET_INSURANCE_BOOKING", new string[2, 2]
      {
        {
          "@ISSUED_DATE_F",
          string.Concat((object) START_DATE)
        },
        {
          "@ISSUED_DATE_T",
          string.Concat((object) TO_DATE)
        }
      }, "da_banca=>GetDailyInsuranceBookingHTB(DateTime  START_DATE, DateTime TO_DATE)");
            da_banca.SUCCESS = true;
            da_banca.MESSAGE = "Success";
        }
        catch (Exception ex)
        {
            insuranceBookingHtb = new DataTable();
            da_banca.SUCCESS = false;
            da_banca.MESSAGE = ex.Message;
        }
        return insuranceBookingHtb;
    }

    public static DataTable GetDataInsuranceBookingHTBByPolicyStatus(
      DateTime START_DATE,
      DateTime TO_DATE)
    {
        DataTable dataTable = new DataTable();
        DataTable htbByPolicyStatus;
        try
        {
            htbByPolicyStatus = da_banca.db.GetData(AppConfiguration.GetConnectionString(), "SP_MICRO_POLICY_GET_INSURANCE_BOOKING_V1", new string[2, 2]
      {
        {
          "@ISSUED_DATE_F",
          string.Concat((object) START_DATE)
        },
        {
          "@ISSUED_DATE_T",
          string.Concat((object) TO_DATE)
        }
      }, "da_banca=>GetDailyInsuranceBookingHTB(DateTime  START_DATE, DateTime TO_DATE)");
            da_banca.SUCCESS = true;
            da_banca.MESSAGE = "Success";
        }
        catch (Exception ex)
        {
            htbByPolicyStatus = new DataTable();
            da_banca.SUCCESS = false;
            da_banca.MESSAGE = ex.Message;
        }
        return htbByPolicyStatus;
    }

    public static DataTable GetIncentiveReferral(
      DateTime F_DATE,
      DateTime T_DATE,
      string CHANNEL_ID,
      string CHANNEL_ITEM_ID,
      string CHANNEL_LOCATION_ID)
    {
        DataTable dataTable = new DataTable();
        DataTable incentiveReferral;
        try
        {
            incentiveReferral = da_banca.db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_GET_INCENTIVE_REFERAL", new string[5, 2]
      {
        {
          "@F_DATE",
          string.Concat((object) F_DATE)
        },
        {
          "@T_DATE",
          string.Concat((object) T_DATE)
        },
        {
          "@CHANNEL_ID",
          CHANNEL_ID
        },
        {
          "@CHANNEL_ITEM_ID",
          CHANNEL_ITEM_ID
        },
        {
          "@CHANNEL_LOCATION_ID",
          CHANNEL_LOCATION_ID
        }
      }, "da_banca=>GetIncentiveReferral(DateTime F_DATE, DateTime T_DATE, string CHANNEL_ID, string CHANNEL_ITEM_ID, string CHANNEL_LOCATION_ID)");
            da_banca.SUCCESS = true;
            da_banca.MESSAGE = "Success";
        }
        catch (Exception ex)
        {
            incentiveReferral = new DataTable();
            da_banca.SUCCESS = false;
            da_banca.MESSAGE = ex.Message;
        }
        return incentiveReferral;
    }

    public static DataTable GetLeadApplicationPolicy(string CIF)
    {
        DataTable dataTable = new DataTable();
        DataTable applicationPolicy;
        try
        {
            applicationPolicy = da_banca.db.GetData(AppConfiguration.GetConnectionString(), "SP_V_CT_MICRO_APPLICATION_LEAD_POLICY_GET", new string[1, 2]
      {
        {
          "@cif",
          CIF
        }
      }, "da_banca=>GetLeadApplicationPolicy(string CIF)");
            if (da_banca.db.RowEffect < 0)
            {
                da_banca.SUCCESS = false;
                da_banca.MESSAGE = da_banca.db.Message;
            }
            else
            {
                da_banca.SUCCESS = true;
                da_banca.MESSAGE = "Success";
            }
        }
        catch (Exception ex)
        {
            applicationPolicy = new DataTable();
            da_banca.SUCCESS = false;
            da_banca.MESSAGE = ex.Message;
        }
        return applicationPolicy;
    }

    public static DataTable GetLeadApplicationPolicy(
      string clientName,
      string clientGender,
      DateTime clientDob,
      string clientIdType,
      string clientIdNumber)
    {
        DataTable dataTable = new DataTable();
        DataTable applicationPolicy;
        try
        {
            applicationPolicy = da_banca.db.GetData(AppConfiguration.GetConnectionString(), "SP_V_CT_MICRO_APPLICATION_LEAD_POLICY_GET_BY_NAME", new string[5, 2]
      {
        {
          "@client_name_en",
          clientName
        },
        {
          "@client_gender",
          clientGender
        },
        {
          "@client_dob",
          string.Concat((object) clientDob)
        },
        {
          "@client_id_type",
          clientIdType
        },
        {
          "@client_id_number",
          clientIdNumber
        }
      }, "da_banca=>GetLeadApplicationPolicy(string clientName, string clientGender, DateTime clientDob, string clientIdType, string clientIdNumber)");
            if (da_banca.db.RowEffect < 0)
            {
                da_banca.SUCCESS = false;
                da_banca.MESSAGE = da_banca.db.Message;
            }
            else
            {
                da_banca.SUCCESS = true;
                da_banca.MESSAGE = "Success";
            }
        }
        catch (Exception ex)
        {
            applicationPolicy = new DataTable();
            da_banca.SUCCESS = false;
            da_banca.MESSAGE = ex.Message;
        }
        return applicationPolicy;
    }

    public static bool RoleBackIssuePolicy(
      string CUSTOMER_ID,
      string POLICY_ID,
      string POLICY_DETAIL_ID,
      string POLICY_RIDER_ID,
      string POLICY_PAYMENT_ID,
      string POLICY_BENEFICIARY_ID,
      string POLICY_ADDRESS_ID)
    {
        bool flag;
        try
        {
            flag = new DB().Execute(AppConfiguration.GetConnectionString(), "SP_MICRO_ROLEBACK_ISSUE", new string[,]
      {
        {
          "@CUSTOMER_ID",
          CUSTOMER_ID
        },
        {
          "@POLICY_ID",
          POLICY_ID
        },
        {
          "@POLICY_DETAIL_ID",
          POLICY_DETAIL_ID
        },
        {
          "@POLICY_RIDER_ID",
          POLICY_RIDER_ID
        },
        {
          "@POLICY_PAYMENT_ID",
          POLICY_PAYMENT_ID
        },
        {
          "@POLICY_BENEFICIARY_ID",
          POLICY_BENEFICIARY_ID
        },
        {
          "@POLICY_ADDRESS_ID",
          POLICY_ADDRESS_ID
        }
      }, "da_banca=>RoleBackIssuePolicy(string CUSTOMER_ID, string POLICY_ID, string POLICY_DETAIL_ID, string POLICY_RIDER_ID, string POLICY_PAYMENT_ID, string POLICY_BENEFICIARY_ID, string POLICY_ADDRESS_ID)");
        }
        catch (Exception ex)
        {
            da_banca.SUCCESS = false;
            flag = false;
            da_banca.MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [RoleBackIssuePolicy(string CUSTOMER_ID, string POLICY_ID, string POLICY_DETAIL_ID, string POLICY_PAYMENT_ID, string POLICY_BENEFICIARY_ID, string POLICY_ADDRESS_ID)] in class [da_banca], detail: " + ex.Message + " ==>" + ex.StackTrace);
        }
        return flag;
    }

    public static bool RoleBackIssuePolicy(string CUSTOMER_ID, string POLICY_ID)
    {
        bool flag;
        try
        {
            flag = new DB().Execute(AppConfiguration.GetConnectionString(), "SP_MICRO_ROLEBACK_ISSUE_CUST_ID_POL_ID", new string[2, 2]
      {
        {
          "@CUSTOMER_ID",
          CUSTOMER_ID
        },
        {
          "@POLICY_ID",
          POLICY_ID
        }
      }, "da_banca=>RoleBackIssuePolicy(string CUSTOMER_ID, string POLICY_ID)");
        }
        catch (Exception ex)
        {
            da_banca.SUCCESS = false;
            flag = false;
            da_banca.MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [RoleBackIssuePolicy(string CUSTOMER_ID, string POLICY_ID)] in class [da_banca], detail:" + ex.Message + "==>" + ex.StackTrace);
        }
        return flag;
    }

    public class ApplicationConsumer
    {
        public string InsuranceApplicationId { get; set; }

        public string ClientNameENG { get; set; }

        public double Premium { get; set; }

        public string Currency { get; set; }

        public static da_banca.ApplicationConsumer GetApplicationConsumer(string APPLICATION_NUMBER)
        {
            da_banca.MESSAGE = "Success";
            da_banca.SUCCESS = true;
            bl_micro_application microApplication = new bl_micro_application();
            bl_micro_application application = da_micro_application.GetApplication(APPLICATION_NUMBER);
            if (application.APPLICATION_NUMBER == null)
                return new da_banca.ApplicationConsumer();
            DataTable detailByApplicationId = da_micro_application.GetApplicationDetailByApplicationID(application.APPLICATION_ID);
            if (detailByApplicationId.Rows.Count <= 0)
                return new da_banca.ApplicationConsumer();
            DataRow row = detailByApplicationId.Rows[0];
            return new da_banca.ApplicationConsumer()
            {
                InsuranceApplicationId = row["application_number"].ToString(),
                ClientNameENG = string.Concat(row["last_name_in_english"].ToString(), " ", row["first_name_in_english"].ToString()),
                Currency = "USD",
                Premium = Convert.ToDouble(row["total_amount"].ToString())
            };
        }
    }

    public class PaymentHTB
    {
        private string _ID;

        public PaymentHTB()
        {
            this._ID = Helper.GetNewGuid(new string[,]
      {
        {
          "TABLE",
          "CT_MICRO_PAYMENT_HTB"
        },
        {
          "FIELD",
          "ID"
        }
      });
        }

        public string ID
        {
            get { return this._ID; }
            set { this._ID = value; }
        }

        public string BranchCode { get; set; }

        public string BranchName { get; set; }

        public string PaymentReferenceNo { get; set; }

        public string TransactionType { get; set; }

        public string InsuranceApplicationId { get; set; }

        public string ClientNameENG { get; set; }

        public string Currency { get; set; }

        public double Premium { get; set; }

        public DateTime PaymentDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Remarks { get; set; }

        public static bool SavePaymentHTB(da_banca.PaymentHTB OBJ_PAYMENT)
        {
            DB db = new DB();
            bool flag;
            try
            {
                flag = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PAYMENT_HTB_INSERT", new string[,]
        {
          {
            "@ID",
            OBJ_PAYMENT.ID
          },
          {
            "@BRANCH_CODE",
            OBJ_PAYMENT.BranchCode
          },
          {
            "@BRANCH_NAME",
            OBJ_PAYMENT.BranchName
          },
          {
            "@PAYMENT_REFERENCE_NO",
            OBJ_PAYMENT.PaymentReferenceNo
          },
          {
            "@TRANSACTION_TYPE",
            OBJ_PAYMENT.TransactionType
          },
          {
            "@INSURANCE_APPLICATION_NUMBER",
            OBJ_PAYMENT.InsuranceApplicationId
          },
          {
            "@CLIENT_NAME",
            OBJ_PAYMENT.ClientNameENG
          },
          {
            "@PREMIUM",
            string.Concat((object) OBJ_PAYMENT.Premium)
          },
          {
            "@PAYMENT_DATE",
            string.Concat((object) OBJ_PAYMENT.PaymentDate)
          },
          {
            "@CREATED_BY",
            OBJ_PAYMENT.CreatedBy
          },
          {
            "@CREATED_ON",
            string.Concat((object) OBJ_PAYMENT.CreatedOn)
          },
          {
            "@REMARKS",
            OBJ_PAYMENT.Remarks
          }
        }, "da_banca => PaymentHTB => SavePaymentHTB(PaymentHTB OBJ_PAYMENT)");
                da_banca.SUCCESS = flag;
                da_banca.MESSAGE = db.Message;
            }
            catch (Exception ex)
            {
                da_banca.SUCCESS = false;
                flag = false;
                da_banca.MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [SavePaymentHTB(PaymentHTB OBJ_PAYMENT)] in class [da_banca => PaymentHTB], detail: " + ex.Message + " ==>" + ex.StackTrace);
            }
            return flag;
        }

        public static List<da_banca.PaymentHTB.PaymentHTBReport> GetPaymentReport(
          DateTime F_DATE,
          DateTime T_DATE,
          string CHANNEL_ITEM_ID,
          string CHANNEL_LOCATION_ID)
        {
            List<da_banca.PaymentHTB.PaymentHTBReport> paymentReport = new List<da_banca.PaymentHTB.PaymentHTBReport>();
            try
            {
                DB db = new DB();
                DataTable data = db.GetData(AppConfiguration.GetConnectionString(), "CT_MICRO_PAYMENT_HTB_GET", new string[,]
        {
          {
            "@F_DATE",
            string.Concat((object) F_DATE)
          },
          {
            "@T_DATE",
            string.Concat((object) T_DATE)
          },
          {
            "@CHANNEL_ITEM_ID",
            CHANNEL_ITEM_ID
          },
          {
            "@CHANNEL_LOCATION_ID",
            CHANNEL_LOCATION_ID
          }
        }, "da_banca => PaymentHTB => GetPaymentReport(DateTime F_DATE, DateTime T_DATE)");
                if (db.RowEffect == -1)
                {
                    da_banca.MESSAGE = db.Message;
                    da_banca.SUCCESS = false;
                }
                else if (data.Rows.Count > 0)
                {
                    da_banca.SUCCESS = true;
                    da_banca.MESSAGE = "Success";
                    foreach (DataRow row in (InternalDataCollectionBase)data.Rows)
                    {
                        List<da_banca.PaymentHTB.PaymentHTBReport> paymentHtbReportList = paymentReport;
                        da_banca.PaymentHTB.PaymentHTBReport paymentHtbReport1 = new da_banca.PaymentHTB.PaymentHTBReport();
                        paymentHtbReport1.ID = row["id"].ToString();
                        paymentHtbReport1.BranchCode = row["branch_code"].ToString();
                        paymentHtbReport1.BranchName = row["branch_name"].ToString();
                        paymentHtbReport1.ClientNameENG = row["client_name"].ToString();
                        paymentHtbReport1.InsuranceApplicationId = row["insurance_application_number"].ToString();
                        paymentHtbReport1.PaymentDate = Convert.ToDateTime(row["payment_date"].ToString());
                        paymentHtbReport1.PaymentReferenceNo = row["payment_reference_no"].ToString();
                        paymentHtbReport1.Premium = Convert.ToDouble(row["premium"].ToString());
                        paymentHtbReport1.Currency = row["currency"].ToString();
                        paymentHtbReport1.TransactionType = row["transaction_type"].ToString();
                        paymentHtbReport1.CreatedBy = row["created_by"].ToString();
                        paymentHtbReport1.CreatedOn = Convert.ToDateTime(row["created_on"].ToString());
                        paymentHtbReport1.UpdatedBy = row["updated_by"].ToString();
                        paymentHtbReport1.UpdatedOn = Convert.ToDateTime(row["updated_on"].ToString());
                        paymentHtbReport1.Remarks = row["remarks"].ToString();
                        paymentHtbReport1.CHANNEL_NAME = row["channel_name"].ToString();
                        paymentHtbReport1.CHANNEL_ITEM_ID = row["channel_item_id"].ToString();
                        paymentHtbReport1.CHANNEL_LOCATION_ID = row["channel_location_id"].ToString();
                        paymentHtbReport1.OFFICE_CODE = row["office_code"].ToString();
                        paymentHtbReport1.OFFICE_NAME = row["office_name"].ToString();
                        da_banca.PaymentHTB.PaymentHTBReport paymentHtbReport2 = paymentHtbReport1;
                        paymentHtbReportList.Add(paymentHtbReport2);
                    }
                }
                else
                {
                    da_banca.MESSAGE = "No Record Found";
                    da_banca.SUCCESS = true;
                }
            }
            catch (Exception ex)
            {
                da_banca.MESSAGE = ex.Message;
                da_banca.SUCCESS = false;
                Log.AddExceptionToLog("Error function [GetPaymentReport(DateTime F_DATE, DateTime T_DATE)] in class [da_banca => PaymentHTB], detail: " + ex.Message + "=>" + ex.StackTrace);
                paymentReport = new List<da_banca.PaymentHTB.PaymentHTBReport>();
            }
            return paymentReport;
        }

        public class PaymentHTBReport : da_banca.PaymentHTB
        {
            public string CHANNEL_NAME { get; set; }

            public string CHANNEL_ITEM_ID { get; set; }

            public string CHANNEL_LOCATION_ID { get; set; }

            public string OFFICE_CODE { get; set; }

            public string OFFICE_NAME { get; set; }
        }
    }

    public class PaymentHTBObjectString
    {
        public string BranchCode { get; set; }

        public string BranchName { get; set; }

        public string PaymentReferenceNo { get; set; }

        public string TransactionType { get; set; }

        public string InsuranceApplicationId { get; set; }

        public string ClientNameENG { get; set; }

        public string Currency { get; set; }

        public string Premium { get; set; }

        public string PaymentDate { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedOn { get; set; }

        public string Remarks { get; set; }
    }

    public class Payment
    {
        public static bool SUCCESS;
        public static string MESSAGE;

        public Payment()
        {
            da_banca.Payment.SUCCESS = false;
            da_banca.Payment.MESSAGE = "";
        }

        public static bool UploadPaymentList(
          DataTable paymentList,
          string company,
          string userName,
          DateTime tranDate,
          string channelItemId)
        {
            bool flag;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(AppConfiguration.GetConnectionString());
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "SP_CT_MICRO_PAYMENT_LIST_IMPORT";
                sqlCommand.Parameters.AddWithValue("@TBL", (object)paymentList);
                sqlCommand.Parameters.AddWithValue("@COMPANY", (object)company);
                sqlCommand.Parameters.AddWithValue("@CREATED_BY", (object)userName);
                sqlCommand.Parameters.AddWithValue("@CREATED_ON", (object)tranDate);
                sqlCommand.Parameters.AddWithValue("@CHANNEL_ITEM_ID", (object)channelItemId);
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    if (sqlDataReader[0].ToString().ToUpper() == "SUCCESS")
                    {
                        da_banca.Payment.SUCCESS = true;
                        da_banca.Payment.MESSAGE = sqlDataReader[1].ToString();
                    }
                    if (sqlDataReader[0].ToString().ToUpper() == "WARNNING")
                    {
                        da_banca.Payment.SUCCESS = false;
                        da_banca.Payment.MESSAGE = sqlDataReader[1].ToString();
                    }
                    else if (sqlDataReader[0].ToString().ToUpper() == "FAIL")
                    {
                        da_banca.Payment.SUCCESS = false;
                        da_banca.Payment.MESSAGE = "Import Fail, please contact your system administrator.";
                        Log.AddExceptionToLog("Error function [UploadPaymentList(string company, string userName, DateTime tranDate)] in class [da_banca=>Payment], detail:" + sqlDataReader[1].ToString());
                    }
                }
                flag = da_banca.Payment.SUCCESS;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                flag = false;
                da_banca.Payment.MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [UploadPaymentList(string company, string userName, DateTime tranDate)] in class [da_banca=>Payment], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return flag;
        }

        public static DataTable GetTempPaymentList(
          string createdBY,
          DateTime createdOn,
          string channelItemId)
        {
            DB db = new DB();
            DataTable tempPaymentList = new DataTable();
            try
            {
                tempPaymentList = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_PAYMENT_LIST_TEMP_GET", new string[3, 2]
        {
          {
            "@CREATED_BY",
            createdBY ?? ""
          },
          {
            "@CREATED_ON",
            string.Concat((object) createdOn)
          },
          {
            "@CHANNEL_ITEM_ID",
            channelItemId
          }
        }, "da_banca => Payment => GetTempPaymentList(string createdBY, DateTime createdOn, string channelItemId)");
                if (db.RowEffect == -1)
                {
                    da_banca.Payment.MESSAGE = db.Message;
                    da_banca.Payment.SUCCESS = false;
                }
                else
                {
                    da_banca.Payment.SUCCESS = true;
                    da_banca.Payment.MESSAGE = "Success";
                }
            }
            catch (Exception ex)
            {
                da_banca.Payment.SUCCESS = false;
                da_banca.Payment.MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [GetTempPaymentList(string createdBY, DateTime createdOn, string channelItemId)] in class [da_banca=>Payment], detail:" + ex.Message + "==>" + ex.StackTrace);
            }
            return tempPaymentList;
        }

        public static DataTable GetPolicyPaymentComparison(
          DateTime issuedDateF,
          DateTime issuedDateT,
          string createdBY,
          string channelItemId)
        {
            DB db = new DB();
            DataTable paymentComparison = new DataTable();
            try
            {
                paymentComparison = db.GetData(AppConfiguration.GetConnectionString(), "SP_MICRO_POLICY_PAYMENT_COMPARISON_GET", new string[4, 2]
        {
          {
            "@ISSUED_DATE_F",
            string.Concat((object) issuedDateF)
          },
          {
            "@ISSUED_DATE_T",
            string.Concat((object) issuedDateT)
          },
          {
            "@CREATED_BY",
            createdBY
          },
          {
            "@CHANNEL_ITEM_ID",
            channelItemId
          }
        }, "da_banca => Payment => GetPolicyPaymentComparison(DateTime issuedDateF, DateTime issuedDateT, string createdBY, string channelItemId)");
                if (db.RowEffect == -1)
                {
                    da_banca.Payment.MESSAGE = db.Message;
                    da_banca.Payment.SUCCESS = false;
                }
                else
                {
                    da_banca.Payment.SUCCESS = true;
                    da_banca.Payment.MESSAGE = "Success";
                }
            }
            catch (Exception ex)
            {
                da_banca.Payment.SUCCESS = false;
                da_banca.Payment.MESSAGE = ex.Message;
                Log.AddExceptionToLog("Error function [GetPolicyPaymentComparison(DateTime issuedDateF, DateTime issuedDateT, string createdBY, string channelItemId)] in class [da_banca=>Payment], detail: " + ex.Message + " ==>" + ex.StackTrace);
            }
            return paymentComparison;
        }
    }
}
