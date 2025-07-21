using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for da_micro_group_load_upload
/// </summary>
public class da_micro_group_loan_upload
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }

    //private static DB db = new DB();
    public da_micro_group_loan_upload()
    {
        //
        // TODO: Add constructor logic here
        //
        _SUCCESS = false;
        _MESSAGE = "";
    }
    public enum POJECT_TYPE_OPTIONS { LOAN, INTERNATIONAL_MONEY_TRANSFER, PAYROLL, DIGITAL_LOAN }

    public static List<bl_micro_group_customer> ExistingCustomerList { get { return _existingCustomerList; } }
    private static List<bl_micro_group_customer> _existingCustomerList = new List<bl_micro_group_customer>();

    public static List<FirstPolicyList> GroupFirstPolicyList { get { return _firstPolicyList; } }
    private static List<FirstPolicyList> _firstPolicyList = new List<FirstPolicyList>();

    public static DataTable PolicyInforce { get { return _tblPolicyInforce; } }
    private static DataTable _tblPolicyInforce = new DataTable();
    /// <summary>
    /// Upload in temp table 
    /// </summary>
    /// <param name="tbl"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdOn"></param>
    /// <param name="channel_id"></param>
    /// <param name="channel_itme_id"></param>
    /// <param name="channel_location_id"></param>
    /// <param name="remarks"></param>
    /// <param name="bundleName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool UploadLoan(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_LOAN_UPLOAD";
            cmd.Parameters.AddWithValue("@TBL", tbl);
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ID", channel_id);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Parameters.AddWithValue("@CHANNEL_LOCATION_ID", channel_location_id);
            cmd.Parameters.AddWithValue("@REMARKS", remarks);
            cmd.Parameters.AddWithValue("@Bundle_Name", bundleName);
            cmd.Parameters.AddWithValue("@file_path", filePath);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [UploadLoan(DataTable tbl, string createdBy, DateTime createdOn)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [UploadLoan(DataTable tbl, string createdBy, DateTime createdOn)] in class [da_micro_group_loan_upload], detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return result;
    }
    public static bool UploadMoneyTransfer(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_LOAN_UPLOAD_MONEY_TRANSFER";
            cmd.Parameters.AddWithValue("@TBL", tbl);
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ID", channel_id);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Parameters.AddWithValue("@CHANNEL_LOCATION_ID", channel_location_id);
            cmd.Parameters.AddWithValue("@REMARKS", remarks);
            cmd.Parameters.AddWithValue("@Bundle_Name", bundleName);
            cmd.Parameters.AddWithValue("@file_path", filePath);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [UploadMoneyTransfer(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [UploadMoneyTransfer(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)] in class [da_micro_group_loan_upload], detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return result;
    }
    public static bool UploadPayroll(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_LOAN_UPLOAD_WING_PAYROLL";
            cmd.Parameters.AddWithValue("@TBL", tbl);
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ID", channel_id);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Parameters.AddWithValue("@CHANNEL_LOCATION_ID", channel_location_id);
            cmd.Parameters.AddWithValue("@REMARKS", remarks);
            cmd.Parameters.AddWithValue("@Bundle_Name", bundleName);
            cmd.Parameters.AddWithValue("@file_path", filePath);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [UploadPayroll(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [UploadPayroll(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)] in class [da_micro_group_loan_upload], detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return result;
    }

    public static bool UploadDigitalLoan(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string loanPeriodType, string bundleName, string filePath)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_WING_DIGITAL_LOAN_UPLOAD";
            cmd.Parameters.AddWithValue("@TBL", tbl);
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ID", channel_id);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Parameters.AddWithValue("@CHANNEL_LOCATION_ID", channel_location_id);
            cmd.Parameters.AddWithValue("@LOAN_PERIOD_TYPE", loanPeriodType);
            cmd.Parameters.AddWithValue("@Bundle_Name", bundleName);
            cmd.Parameters.AddWithValue("@file_path", filePath);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [UploadDigitalLoan(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string loanPeriodType, string bundleName, string filePath)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [UploadDigitalLoan(DataTable tbl, string createdBy, DateTime createdOn, string channel_id, string channel_itme_id, string channel_location_id, string remarks, string bundleName, string filePath)] in class [da_micro_group_loan_upload], detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return result;
    }

    /// <summary>
    /// Copy loan record from temp table to real table
    /// </summary>
    /// <param name="createdBy"></param>
    /// <param name="createdOn"></param>
    /// <param name="channel_itme_id"></param>
    /// <returns></returns>
    public static bool SaveLoan(string createdBy, DateTime createdOn, string channel_itme_id)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_LOAN_RECORD_INSERT";
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [SaveLoan(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [SaveLoan(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return result;
    }
    public static bool SaveMoneyTransfer(string createdBy, DateTime createdOn, string channel_itme_id)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_LOAN_RECORD_MONEY_TRANSFER_INSERT";
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [SaveMoneyTransfer(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [SaveMoneyTransfer(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + ex.Message);

        }
        return result;
    }

    public static bool SaveWingPayroll(string createdBy, DateTime createdOn, string channel_itme_id)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_LOAN_RECORD_WING_PAYROLL_INSERT";
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [SaveWingPayroll(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [SaveWingPayroll(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + ex.Message);

        }
        return result;
    }

    public static bool SaveWingDigitalLoan(string createdBy, DateTime createdOn, string channel_itme_id)
    {
        bool result = false;

        try
        {
            SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "SP_CT_GROUP_MICRO_LOAN_RECORD_WING_DIGITAL_LOAN_INSERT";
            cmd.Parameters.AddWithValue("@CREATED_BY", createdBy);
            cmd.Parameters.AddWithValue("@CREATED_ON", createdOn);
            cmd.Parameters.AddWithValue("@CHANNEL_ITEM_ID", channel_itme_id);
            cmd.Connection = con;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString().ToUpper() == "SUCCESS")
                {
                    _SUCCESS = true;
                    _MESSAGE = dr[1].ToString();
                }
                if (dr[0].ToString().ToUpper() == "WARNNING")
                {
                    _SUCCESS = false;
                    _MESSAGE = dr[1].ToString();
                }
                else if (dr[0].ToString().ToUpper() == "FAIL")
                {
                    _SUCCESS = false;
                    _MESSAGE = "Import Fail, please contact your system administrator.";
                    Log.AddExceptionToLog("Error function [SaveWingDigitalLoan(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + dr[1].ToString());

                }
            }
            result = SUCCESS;

            con.Close();
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = "Import Fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error function [SaveWingDigitalLoan(string createdBy, DateTime createdOn, string channel_itme_id)] in class [da_micro_group_loan_upload], detail:" + ex.Message);

        }
        return result;
    }


    public static List<bl_loan_upload> GetLoanList(string createdBy, DateTime createdOn)
    {
        List<bl_loan_upload> loanList = new List<bl_loan_upload>();
        bl_loan_upload loan;
        DB db = new DB();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_LOAN_RECORD_GET", new string[,] { 
            { "@created_by", createdBy},{"@created_on", createdOn+""}
            }, "da_micro_group_loan_upload=>List<bl_loan_upload> GetLoanList(string createdBy, DateTime cratedOn)");

            if (db.RowEffect >= 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    loan = new bl_loan_upload()
                    {
                        SubmmitedDate = Convert.ToDateTime(r["submitted_date"].ToString()),
                        Branch = r["branch"].ToString(),
                        LoanID = r["loan_id"].ToString(),
                        ClientName = r["client_name"].ToString(),
                        Gender = Convert.ToInt32(r["gender"].ToString()),
                        DOB = Convert.ToDateTime(r["dob"].ToString()),
                        IdType = Convert.ToInt32(r["id_type"].ToString()),
                        IdNumber = r["id_number"].ToString(),
                        AgreementNumber = r["agreement_number"].ToString(),
                        Occupation = r["occupation"].ToString(),
                        Address = r["address"].ToString(),
                        ContactNumber = r["contact_number"].ToString(),
                        Currency = r["currency"].ToString(),
                        ExchangeRate = Convert.ToDouble(r["exchange_rate_usd"].ToString()),
                        LoanAmount = Convert.ToDouble(r["loan_amount"].ToString()),
                        DisbursementDate = Convert.ToDateTime(r["disbursement_date"].ToString()),
                        LoanPeriod = Convert.ToInt32(r["loan_period"].ToString()),
                        InsuranceCost = Convert.ToDouble(r["insurance_cost"].ToString()),
                        ChannelId = r["channel_id"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ChannelLocationId = r["channel_location_id"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                        BeneficiaryName = r["Beneficiary_Name"].ToString(),
                        Relation = r["relation"].ToString()
                    };

                    loanList.Add(loan);

                }
                _MESSAGE = "Success.";
                _SUCCESS = true;
            }
            else if (db.RowEffect == -1)
            {
                _MESSAGE = db.Message;
                _SUCCESS = false;
            }
        }
        catch (Exception ex)
        {
            _MESSAGE = ex.Message;
            _SUCCESS = false;

            loanList = new List<bl_loan_upload>();
            Log.AddExceptionToLog("Error function [List<bl_loan_upload> GetLoanList(string createdBy, DateTime cratedOn) in class [da_micro_group_loan_upload], detail: " + ex.Message + "=>" + ex.StackTrace);
        }
        return loanList;
    }

    public static List<bl_money_transfer_upload> GetMoneyTransferList(string createdBy, DateTime createdOn)
    {
        List<bl_money_transfer_upload> tranList = new List<bl_money_transfer_upload>();
        bl_money_transfer_upload tran;
        DB db = new DB();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_LOAN_RECORD_GET_MONEY_TRANSFER", new string[,] { 
            { "@created_by", createdBy},{"@created_on", createdOn+""}
            }, "da_micro_group_loan_upload=>GetMoneyTransferList(string createdBy, DateTime createdOn)");

            if (db.RowEffect >= 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    tran = new bl_money_transfer_upload()
                    {
                        IssuedDate = Convert.ToDateTime(r["Issued_date"].ToString()),
                        ID = r["Id"].ToString(),
                        ClientName = r["client_name"].ToString(),
                        Gender = Convert.ToInt32(r["gender"].ToString()),
                        DOB = Convert.ToDateTime(r["dob"].ToString()),
                        IdType = Convert.ToInt32(r["id_type"].ToString()),
                        IdNumber = r["id_number"].ToString(),
                        AccountNumber = r["account_number"].ToString(),
                        ContactNumber = r["contact_number"].ToString(),
                        Currency = r["currency"].ToString(),
                        ExchangeRate = Convert.ToDouble(r["exchange_rate_usd"].ToString()),
                        SumAssured = Convert.ToDouble(r["sum_assured"].ToString()),
                        EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                        Premium = Convert.ToDouble(r["Premium"].ToString()),
                        ChannelId = r["channel_id"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ChannelLocationId = r["channel_location_id"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString())
                    };

                    tranList.Add(tran);

                }
                _MESSAGE = "Success.";
                _SUCCESS = true;
            }
            else if (db.RowEffect == -1)
            {
                _MESSAGE = db.Message;
                _SUCCESS = false;
            }
        }
        catch (Exception ex)
        {
            _MESSAGE = ex.Message;
            _SUCCESS = false;

            tranList = new List<bl_money_transfer_upload>();
            Log.AddExceptionToLog("Error function [GetMoneyTransferList(string createdBy, DateTime createdOn)] in class [da_micro_group_loan_upload], detail: " + ex.Message);
        }
        return tranList;
    }

    public static List<bl_wing_payroll_upload> GetWingPayrollList(string createdBy, DateTime createdOn)
    {
        List<bl_wing_payroll_upload> wingList = new List<bl_wing_payroll_upload>();
        bl_wing_payroll_upload wing;
        DB db = new DB();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_LOAN_RECORD_GET_WING_PAYROLL", new string[,] { 
            { "@created_by", createdBy},{"@created_on", createdOn+""}
            }, "da_micro_group_loan_upload=>GetWingPayrollList(string createdBy, DateTime createdOn)");

            if (db.RowEffect >= 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    wing = new bl_wing_payroll_upload()
                    {
                        IssuedDate = Convert.ToDateTime(r["Issued_date"].ToString()),
                        ID = r["Id"].ToString(),
                        ClientName = r["client_name"].ToString(),
                        Gender = Convert.ToInt32(r["gender"].ToString()),
                        DOB = Convert.ToDateTime(r["dob"].ToString()),
                        IdType = Convert.ToInt32(r["id_type"].ToString()),
                        IdNumber = r["id_number"].ToString(),
                        PhoneNumber = r["contact_number"].ToString(),
                        Currency = r["currency"].ToString(),
                        ExchangeRate = Convert.ToDouble(r["exchange_rate_usd"].ToString()),
                        SumAssured = Convert.ToDouble(r["sum_assured"].ToString()),
                        EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                        Premium = Convert.ToDouble(r["Premium"].ToString()),
                        ChannelId = r["channel_id"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ChannelLocationId = r["channel_location_id"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                        SubCompanyName = r["sub_company_name"].ToString()
                    };

                    wingList.Add(wing);

                }
                _MESSAGE = "Success.";
                _SUCCESS = true;
            }
            else if (db.RowEffect == -1)
            {
                _MESSAGE = db.Message;
                _SUCCESS = false;
            }
        }
        catch (Exception ex)
        {
            _MESSAGE = ex.Message;
            _SUCCESS = false;

            wingList = null;
            Log.AddExceptionToLog("Error function [GetWingPayrollList(string createdBy, DateTime createdOn)] in class [da_micro_group_loan_upload], detail: " + ex.Message);
        }
        return wingList;
    }

    public static List<bl_wing_digital_loan_upload> GetWingDigitalLoanlList(string createdBy, DateTime createdOn)
    {
        List<bl_wing_digital_loan_upload> wingList = new List<bl_wing_digital_loan_upload>();
        bl_wing_digital_loan_upload wing;
        DB db = new DB();
        try
        {
            //DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_LOAN_RECORD_GET_WING_DIGITAL_LOAN", new string[,] { 
            //{ "@created_by", createdBy},{"@created_on", createdOn+""}
            // }, "da_micro_group_loan_upload=>GetWingDigitalLoanlList(string createdBy, DateTime createdOn)");

            DataSet ds = db.GetDataSet(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_LOAN_RECORD_GET_WING_DIGITAL_LOAN", new string[,] { 
            { "@created_by", createdBy},{"@created_on", createdOn+""}
            });

            DataTable tbl = ds.Tables[0]; /*RECENTLY UPLOADED RECORD*/
            DataTable tblCust = ds.Tables[1]; /*EXISTING CUSTOMER WHICH COMPARE WITH RECENTLY UPLOADED RECORD AND GROUP CUSTOMER*/
            DataTable tblFirstPol = ds.Tables[2]; /*LIST OF FIRST POLICY WITH STATUS INFORCE*/

            if (db.RowEffect >= 0)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    wing = new bl_wing_digital_loan_upload()
                    {
                        AccountNumber = r["account_number"].ToString(),
                        AppliedDate = Convert.ToDateTime(r["Applied_Date"].ToString()),
                        MaturityDate = Convert.ToDateTime(r["Maturity_Date"].ToString()),
                        ID = r["Id"].ToString(),
                        ClientName = r["client_name"].ToString(),
                        Gender = Convert.ToInt32(r["gender"].ToString()),
                        DOB = Convert.ToDateTime(r["dob"].ToString()),
                        IdType = Convert.ToInt32(r["id_type"].ToString()),
                        IdNumber = r["id_number"].ToString(),
                        Province = r["province"].ToString(),
                        District = r["district"].ToString(),
                        Commune = r["commune"].ToString(),
                        Village = r["village"].ToString(),
                        Address = r["address"].ToString(),
                        PhoneNumber = r["phone_number"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ChannelLocationId = r["channel_location_id"].ToString(),
                        Remarks = r["remarks"].ToString(),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                        ChannelId = r["channel_id"].ToString(),
                        Occupation = r["occupation"].ToString(),
                        BeneficiaryName = r["beneficiary_name"].ToString(),
                        Relation = r["relation"].ToString(),
                        LoanPeriod = Convert.ToInt32(r["loan_period"].ToString()),
                        LoanPeriodType = r["loan_period_type"].ToString(),
                        BundleName = r["bundle_name"].ToString(),
                        Seq=Convert.ToInt32(r["seq"].ToString())
                    };

                    wingList.Add(wing);

                }

                /*get existing customer*/
                foreach (DataRow r in tblCust.Rows)
                {
                    _existingCustomerList.Add(new bl_micro_group_customer
                    {
                        CUSTOMER_NUMBER = r["customer_no"].ToString(),
                        ID = r["customer_id"].ToString(),
                        FIRST_NAME_EN = r["first_name_en"].ToString(),
                        LAST_NAME_EN = r["last_name_en"].ToString(),
                        FIRST_NAME_KH = r["first_name_kh"].ToString(),
                        LAST_NAME_KH = r["last_name_kh"].ToString(),
                        GENDER = Convert.ToInt32(r["gender"].ToString()),
                        DOB = Convert.ToDateTime(r["dob"].ToString()),
                        ID_TYPE = Convert.ToInt32(r["id_type"].ToString()),
                        ID_NUMBER = r["id_number"].ToString(),
                        NATIONALITY = r["nationality"].ToString(),
                        OCCUPATION = r["occupation"].ToString(),
                        GroupCode = r["group_code"].ToString()
                    });
                }

                _firstPolicyList = new List<FirstPolicyList>();
                foreach (DataRow r in tblFirstPol.Rows)
                {
                    _firstPolicyList.Add(new FirstPolicyList()
                    {
                        CustomerId = r["customer_id"].ToString(),
                        CustomerNumber = r["customer_no"].ToString(),
                        IdType = Convert.ToInt32(r["id_type"].ToString()),
                        IdNumber = r["id_number"].ToString(),
                        PolicyId = r["policy_id"].ToString(),
                        PolicyNumber = r["policy_number"].ToString(),
                        PolicyStatus = r["policy_status"].ToString(),
                        IsFirstPolicy = Convert.ToInt32(r["is_first_policy"].ToString()) == 1 ? true : false,
                        GroupCode = r["group_code"].ToString()
                    });
                }

                _tblPolicyInforce = new DataTable();
                _tblPolicyInforce = ds.Tables[3];

                _MESSAGE = "Success.";
                _SUCCESS = true;
            }
            else if (db.RowEffect == -1)
            {
                _MESSAGE = db.Message;
                _SUCCESS = false;
            }
        }
        catch (Exception ex)
        {
            _MESSAGE = ex.Message;
            _SUCCESS = false;

            wingList = null;
            Log.AddExceptionToLog("Error function [GetWingDigitalLoanlList(string createdBy, DateTime createdOn)] in class [da_micro_group_loan_upload], detail: " + ex.Message);
        }
        return wingList;
    }


    /// <summary>
    /// Delete temp record from temp loan upload by user and date
    /// </summary>
    /// <param name="createdBy"></param>
    /// <param name="createdOn"></param>
    /// <returns></returns>
    public static bool DeleteTempRecords(string createdBy, DateTime createdOn)
    {
        bool result = false;
        DB db = new DB();
        try
        {

            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_LOAN_RECORD_TEMP_UPLOAD_DELETE", new string[,] {
           
            {"@created_by", createdBy},
            {"@created_on", createdOn+""},
         
            }, "da_micro_group_loan_upload=>DeleteTempRecords(string createdBy, DateTime createdOn)");


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
            Log.AddExceptionToLog("Error function [DeleteTempRecords(string createdBy, DateTime createdOn)] in class [da_micro_group_loan_upload], detail: " + ex.Message + "==>" + ex.StackTrace);
        }
        return result;
    }

    [Serializable]
    public class FirstPolicyList
    {
        public string GroupCode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerNumber { get; set; }
        public Int32 IdType { get; set; }
        public string IdNumber { get; set; }
        public string PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyStatus { get; set; }
        public bool IsFirstPolicy { get; set; }
    }

}