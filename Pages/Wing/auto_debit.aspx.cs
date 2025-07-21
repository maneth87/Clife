using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Xml.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.Web.Security;
public partial class Pages_Wing_auto_debit : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    string user_name = "";
    DateTime transaction_date;
    List<da_report_approver.bl_report_approver> ApproverList;
    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        user_name = System.Web.Security.Membership.GetUser().UserName;
        transaction_date = DateTime.Now;
        ApproverList = da_report_approver.GetApproverList();
        if (!Page.IsPostBack)
        {
            export_excel.Visible = false;
            BindApprover();
        }

    }
    private void BindApprover()
    {
        ddlApprover.Items.Clear();
        ddlApprover.Items.Add(new ListItem(".", ""));
        foreach (da_report_approver.bl_report_approver approver in ApproverList)
        {
            ddlApprover.Items.Add(new ListItem(approver.NameEn, approver.ID + ""));
        }

    }
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        Save();
    }
    protected void ibtnClear_Click(object sender, ImageClickEventArgs e)
    {
        export_excel.Visible = false;
        gv_valid.DataSource = null;
        gv_valid.DataBind();
        gv_invalid.DataSource = null;
        gv_invalid.DataBind();
        Session["AUTO_DEBIT_LIST"] = null;
    }
    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        UploadData();
    }
    protected void export_excel_Click(object sender, EventArgs e)
    {
        Excel();
    }
    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }
    void UploadData()
    {
        try
        {

            if ((flUpload.PostedFile == null) || string.IsNullOrEmpty(flUpload.PostedFile.FileName))
            {
                AlertMessage("Please Browse A File.");
            }
            else
            {
                #region
                string message;
                List<AutoDebit> listUploadData, invalidList;
                if (Validate(out message, out listUploadData, out invalidList) == true)
                {


                    if (listUploadData.Count > 0 && invalidList.Count == 0)
                    {
                        AlertMessage("Consent form list were uploaded successfuly, please view detail in [Success] tab.");
                    }
                    else if (listUploadData.Count == 0 && invalidList.Count > 0)
                    {
                        AlertMessage("Consent form list were uploaded fail, please view detail in [Fail] tab.");
                    }
                    else if (listUploadData.Count > 0 && invalidList.Count > 0)
                    {
                        AlertMessage("Some Consent form were uploaded fail, please view detail in [Fail] tab.");
                    }
                    else
                    {
                        AlertMessage("No data uploaded.");
                    }
                    Session["AUTO_DEBIT_LIST"] = listUploadData;
                    gv_valid.DataSource = listUploadData;
                    gv_valid.DataBind();

                    gv_invalid.DataSource = invalidList;
                    gv_invalid.DataBind();

                    export_excel.Visible = invalidList.Count > 0 ? true : false;
                    if (listUploadData.Count > 0 || invalidList.Count > 0)
                    {
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User uploads temporary auto debit data [Total success:", listUploadData.Count, " Total fail:", invalidList.Count, "]."));
                    }
                }
                else
                {
                    AlertMessage(message);
                }
                #endregion
            }

        }
        catch (Exception ex)
        {
            AlertMessage(ex.Message);
        }


    }
    bool Validate(out string message, out List<AutoDebit> listUploadData, out List<AutoDebit> invalidList)
    {
        bool status = true;
        message = "";
        listUploadData = new List<AutoDebit>();
        invalidList = new List<AutoDebit>();
        string file_path = "";

        try
        {
            //check sheet name
            if ((flUpload.PostedFile != null) && !string.IsNullOrEmpty(flUpload.PostedFile.FileName))
            {
                string save_path = "~/Upload/";
                string file_name = Path.GetFileName(flUpload.PostedFile.FileName);
                string extension = Path.GetExtension(file_name);
                file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
                file_path = save_path + file_name;

                flUpload.SaveAs(Server.MapPath(file_path));//save file 

                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);

                if (my_excel.GetSheetName() != "auto_debit$")
                {
                    message = "File is not correct format, please donwload file template from the system.";
                    //save log
                    UploadTransactionLog("[Validate Execl File] \t [Sheet Name:" + my_excel.GetSheetName().ToString() + "] is not valid.");
                    status = false;
                }
                else
                {
                    my_excel.CommandText = "Select * from [auto_debit$]";
                    DataTable tbl = my_excel.GetData();
                    int col_count = 0;
                    col_count = tbl.Columns.Count;
                    if (col_count > 6 || col_count < 0)//check number of columns
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        //save log
                        UploadTransactionLog("[Validate Execl File] \t [Number of columns] must be equal 6.");
                        status = false;
                    }
                    else
                    {
                        #region //check column name.
                        if (tbl.Columns[0].ColumnName.Trim() != "EffectiveDate")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[0].ColumnName.Trim() + "] is not valid, it must be [EffectiveDate].");
                        }
                        else if (tbl.Columns[1].ColumnName.Trim() != "TransactionDate")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[0].ColumnName.Trim() + "] is not valid, it must be [TransactionDate].");
                        }
                        else if (tbl.Columns[2].ColumnName.Trim() != "TransactionID")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[1].ColumnName.Trim() + "] is not valid, it must be [TransactionID].");

                        }
                        else if (tbl.Columns[3].ColumnName.Trim() != "WingAccount")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[2].ColumnName.Trim() + "] is not valid, it must be [WingAccount].");

                        }
                        else if (tbl.Columns[4].ColumnName.Trim() != "Amount")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[3].ColumnName.Trim() + "] is not valid, it must be [Amount].");

                        }
                        else if (tbl.Columns[5].ColumnName.Trim() != "TransactionStatus")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[4].ColumnName.Trim() + "] is not valid, it must be [TransactionStatus].");

                        }

                        #endregion
                    }

                    if (status == false)
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        listUploadData = new List<AutoDebit>();
                    }
                    else
                    {
                        #region Get upload data from excel file
                        DataTable data_excel = my_excel.GetData();
                        AutoDebit debit;
                        int row_index = -1;
                        AutoDebit invalidData;
                        foreach (DataRow row in data_excel.Rows)
                        {
                            row_index = data_excel.Rows.IndexOf(row);
                            if (validRow(row, row_index, out invalidData))
                            {
                                //store valid data
                                debit = new AutoDebit();
                                debit.TransactionID = row[2].ToString().Trim();
                                debit.WingAccountNumber = row[3].ToString().Trim();
                                debit.Amount = Convert.ToDouble(row[4].ToString().Trim());
                                debit.Status = row[5].ToString().Trim();
                                debit.Remarks = "";
                                debit.EffectiveDate = Helper.FormatDateTime(row[0].ToString().Trim()).Date;
                                debit.TransactionDate = Helper.FormatDateTime(row[1].ToString().Trim());
                                listUploadData.Add(debit);
                            }
                            else
                            {
                                //store invalid data
                                invalidList.Add(invalidData);
                            }
                        }
                        #endregion
                    }
                }
            }
            //delete file
            File.Delete(Server.MapPath(file_path));
        }
        catch (Exception ex)
        {
            listUploadData = new List<AutoDebit>();
            status = false;
            message = "Oooop! something is going wrong, please contact your system administrator.";
            UploadTransactionLog("[Validate excel file] \t [Error]:please check log file for detail.");
            Log.AddExceptionToLog("Error function [ Validate(out string message, out List<AutoDebit> listUploadData, out List<AutoDebit> invalidList)] in page [auto_debit.aspx], Detail:" + ex.Message);
        }
        return status;
    }
    bool validRow(DataRow row, int row_index, out AutoDebit invalidData)
    {
        bool valid = true;
        invalidData = new AutoDebit();

        try
        {
            #region //Check invalid data

            if (row[0].ToString().Trim() == "" || row[0].ToString() == null)//Effective date
            {
                valid = false;
                invalidData.Remarks = "Transaction date is required.";
            }
            else if (row[1].ToString().Trim() == "" || row[1].ToString() == null)//transaction date
            {
                valid = false;
                invalidData.Remarks = "Transaction date is required.";
            }
            else if (Helper.IsDateTime(row[1].ToString().Trim()) == false)//check transaction date format
            {
                valid = false;
                invalidData.Remarks = "Invalid Transaction date.";
            }
            else if (row[2].ToString().Trim() == "")//transaction id
            {
                valid = false;
                invalidData.Remarks = "Transaction id is required.";
            }
            else if (row[3].ToString().Trim() == "")//wing account 
            {
                valid = false;
                invalidData.Remarks = "Wing account is required.";
            }
            else if (!Helper.IsAmount(row[4].ToString().Trim()))//amount
            {
                valid = false;
                invalidData.Remarks = "Amount is required.";
            }

            else if (row[5].ToString().Trim() == "")//transaction status
            {
                valid = false;
                invalidData.Remarks = "Transaction status is required.";
            }

            //store invalid data
            if (!valid)
            {
                invalidData.TransactionDate = Helper.FormatDateTime(row[1].ToString().Trim());
                invalidData.TransactionID = row[2].ToString().Trim();
                invalidData.WingAccountNumber = row[3].ToString().Trim();
                invalidData.Amount = Convert.ToDouble(row[4].ToString().Trim());
                invalidData.Status = row[5].ToString().Trim();
                invalidData.EffectiveDate = Helper.FormatDateTime(row[0].ToString().Trim());
                invalidData.TransactionDate = Helper.FormatDateTime(row[1].ToString().Trim());
            }

            #endregion
        }
        catch (Exception ex)
        {
            valid = false;
            Log.AddExceptionToLog("Error function [validRow(DataRow row, int row_index, out AutoDebit invalidData)] in class [auto_debit.aspx.cs], row index [" + row_index + "], detail:" + ex.Message);
        }
        return valid;
    }

    void Save()
    {
        if (ddlApprover.SelectedIndex > 0)
        {
            if (Session["AUTO_DEBIT_LIST"] != null)
            {
                List<AutoDebit> validList = new List<AutoDebit>();
                List<AutoDebit> invalidList = new List<AutoDebit>();
                List<AutoDebit> debit_list = (List<AutoDebit>)Session["AUTO_DEBIT_LIST"];
                bool isUpdatePolicyRemarks = false;
                bool isUpdatePolicyDetail = false;
                bool isUpdatePolicyStatus = false;
                bool isSavePolicyDetail = false;
                bool isSavePolicyPremPay = false;
                bool isSavePaymentUploading = false; //for payment system
                bool isSaveOfficialReceipt = false;//for payment system
                bool isSaveMethodPayment = false;//for payment system
                bool isSaveReceiptPremPay = false;//for payment system
                bool isSavePaymentWing = false;//for payment system
                DateTime effective_date = new DateTime();
                DateTime maturity_date = new DateTime();
                DateTime expiry_date = new DateTime();
                TimeSpan effective_time = new TimeSpan(23, 59, 0);
                int age = 0;
                int cover_year = 0;
                int pay_year = 0;
                int cover_to_age = 0;
                int pay_to_age = 0;
                bl_wing.policy.PolicyInfo polInfo;
                SENDSMS sms;
                string save_log = "<<Start Saved By[" + user_name + "] [" + transaction_date.ToString("dd MMM yyyy HH:mm:ss") + "]" + Environment.NewLine;
                string status_code = "";
                if (debit_list.Count > 0)
                {

                    string policy_detail_id = "";

                    #region Loop in debit_list
                    foreach (AutoDebit debit in debit_list)
                    {
                        //GET POLICY INFORMATION BY POLICY NUMBER 
                        polInfo = da_wing.Policy.PolicyInfo.GetPolicyInfo(debit.WingAccountNumber);
                        if (polInfo.PolicyID != null && polInfo.PolicyID != "")
                        {
                            bl_product product = da_product.GetProductByProductID(polInfo.ProductID);

                            // effective_date = debit.TransactionDate.Date + effective_time;
                            effective_date = debit.EffectiveDate.Date + effective_time;
                            maturity_date = effective_date.Date.AddMonths(1); //add one month
                            expiry_date = maturity_date.AddDays(-1) + effective_time;
                            age = Calculation.Culculate_Customer_Age(polInfo.DOB.ToString("dd/MM/yyyy"), effective_date.Date);
                            //pay_to_age = Calculation.Culculate_Customer_Age(polInfo.DOB.ToString("dd/MM/yyyy"), effective_date.Date);
                            pay_to_age = age + (product.Age_Max - age);
                            cover_to_age = pay_to_age;
                            #region Check Over age
                            if (age >= product.Age_Min && age <= product.Age_Max)
                            {
                                if (polInfo.EffectiveDate != effective_date)
                                {
                                    status_code = (debit.Status.ToUpper() == "TRANSACTION SUCCESS" ? "IF" : debit.Status.ToUpper() == "TRANSACTION FAIL" ? "LAP" : "N/A");
                                    #region check status "NEW"
                                    if (polInfo.PolicyStatus.Trim().ToUpper() == "NEW")//is the first policy registeration
                                    {
                                        #region Update policy remarks in ct_ci_policy
                                        isUpdatePolicyRemarks = da_wing.Policy.UpdatePolicyRemarks(polInfo.PolicyID, debit.Status.ToUpper(), user_name, transaction_date);
                                        #endregion Update policy remarks in ct_ci_policy
                                        #region Update policy detail in ct_ci_policy_detail
                                        if (isUpdatePolicyRemarks)
                                        {
                                            save_log += "1. Update policy remark in [ct_ci_policy] [SUCCESS]" + Environment.NewLine;
                                            isUpdatePolicyDetail = da_wing.PolicyDetail.UpdatePolicyDetail(new bl_wing.PolicyDetail()
                                            {
                                                PolicyID = polInfo.PolicyID,
                                                PolicyDetailID = polInfo.PolicyDetailID,
                                                EffectiveDate = effective_date,
                                                MaturityDate = maturity_date,
                                                ExpiryDate = expiry_date,
                                                IssuedDate = debit.EffectiveDate,
                                                Age = age,
                                                CoverYear = cover_year,
                                                PayYear = pay_year,
                                                CoverUpToAge = cover_to_age,
                                                PayUpToAge = pay_to_age,
                                                SumAssured = polInfo.SumAssured,
                                                OriginalPremium = polInfo.Originalpremium,
                                                Premium = (status_code == "IF" ? debit.Amount : 0),
                                                UserPremium = (status_code == "IF" ? debit.Amount : 0),
                                                PaymentBy = polInfo.PaymentBy,
                                                PaymentCode = polInfo.PaymentCode,
                                                PayModeID = Helper.GetPayModeID(polInfo.PaymentMode),
                                                RETURN_PREMIUM = 0,
                                                DiscountAmount = 0,
                                                PolicyStatusRemarks = status_code,
                                                TransactionID = debit.TransactionID,
                                                UpdatedBy = user_name,
                                                UpdatedDateTime = transaction_date,
                                                TransactionDate = debit.TransactionDate
                                            }, status_code);


                                            if (isUpdatePolicyDetail)
                                            {
                                                //save log
                                                save_log += "2. Update policy detail in table [CT_CI_POLICY_DETAIL] [SUCCESS]" + Environment.NewLine;


                                                #region Save policy prem pay

                                                bl_policy_prem_pay pol_prem_pay = new bl_policy_prem_pay();
                                                pol_prem_pay.Policy_Prem_Pay_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_POLICY_PREM_PAY" }, { "FIELD", "POLICY_PREM_PAY_ID" } });
                                                pol_prem_pay.Policy_ID = polInfo.PolicyID;
                                                //pol_prem_pay.Due_Date = debit.TransactionDate;
                                                pol_prem_pay.Due_Date = debit.EffectiveDate.Date;
                                                pol_prem_pay.Pay_Date = debit.TransactionDate;
                                                pol_prem_pay.Prem_Year = 1;
                                                pol_prem_pay.Prem_Lot = 1;
                                                pol_prem_pay.Amount = status_code == "IF" ? debit.Amount : 0;
                                                pol_prem_pay.Sale_Agent_ID = polInfo.AgentCode;
                                                pol_prem_pay.Office_ID = "HQ";
                                                pol_prem_pay.Created_By = user_name;
                                                pol_prem_pay.Created_On = transaction_date;
                                                pol_prem_pay.Created_Note = "";
                                                pol_prem_pay.Pay_Mode_ID = Helper.GetPayModeID(polInfo.PaymentMode);
                                                isSavePolicyPremPay = da_policy.InsertPolicyPremiumPay(pol_prem_pay);

                                                #endregion save policy prem pay

                                                if (isSavePolicyPremPay)
                                                {
                                                    save_log += "3. Save policy premium pay in table [ct_policy_prem_pay] [SUCCESS]" + Environment.NewLine;
                                                    //update some information in table ct_policy_prem_pay
                                                    da_policy.UpdatePolicyPremPay(pol_prem_pay.Policy_Prem_Pay_ID, polInfo.ProductID, product.Plan_Code, status_code, polInfo.SumAssured);


                                                    if (status_code == "IF")
                                                    {
                                                        #region save payment uploading record
                                                        string receipt_num = da_policy_prem_pay.Auto_Receipt_Number().ToUpper();
                                                        string official_receipt_id = "";
                                                        official_receipt_id = Helper.GetNewGuid(AppConfiguration.GetConnectionString(), new string[,] { { "TABLE", "Ct_Official_Receipt" }, { "FIELD", "Official_receipt_id" } });
                                                        //delete all existing data
                                                        //da_wing.TruncatePaymentUploading();
                                                        isSavePaymentUploading = da_wing.InsertIntoPaymentUploading(polInfo.PolicyID, polInfo.CustomerID, debit.TransactionID, debit.Amount, polInfo.Premium, 0, 0, debit.TransactionDate, polInfo.AgentCode, "HQ", Helper.GetPayModeID(polInfo.PaymentMode), 0, polInfo.SumAssured, polInfo.PolicyStatus, polInfo.ProductID, 1, polInfo.PolicyNumber);
                                                        #endregion save payment uploading record

                                                        if (isSavePaymentUploading)
                                                        {
                                                            save_log += "4. Save payment uploading in table [Ct_Payment_Uploading] [SUCCESS]" + Environment.NewLine;

                                                            #region Save payment wing
                                                            bl_payment_wing payment_wing = new bl_payment_wing();
                                                            string payment_wing_id = "";
                                                            payment_wing_id = Helper.GetNewGuid(AppConfiguration.GetConnectionString(), new string[,] { { "TABLE", "Ct_Payment_Wing" }, { "FIELD", "PAYMENT_WING_ID" } });
                                                            payment_wing.Payment_Wing_ID = payment_wing_id;
                                                            payment_wing.Policy_ID = polInfo.PolicyID;
                                                            payment_wing.Bill_No = debit.WingAccountNumber;
                                                            payment_wing.Transaction_ID = debit.TransactionID;
                                                            payment_wing.Created_Note = "";
                                                            payment_wing.Received_Amount = (status_code == "IF" ? debit.Amount : 0);
                                                            payment_wing.Received_Date = debit.TransactionDate;
                                                            payment_wing.Created_On = transaction_date;// DateTime.Now;
                                                            payment_wing.Created_By = user_name;
                                                            payment_wing.Status_Paid = 1; //0: not paid or createding OR; 1: has been used for creating OR
                                                            payment_wing.Transaction_Type = 1; // transfer from Wing
                                                            isSavePaymentWing = da_wing.InsertIntoPaymentWing(payment_wing);
                                                            #endregion Save payment wing

                                                            if (isSavePaymentWing)
                                                            {
                                                                save_log += "5. Save payment wing in table [Ct_Payment_Wing] [SUCCESS]" + Environment.NewLine;

                                                                #region save official receipt
                                                                bl_official_receipt official_receipt = new bl_official_receipt();
                                                                official_receipt.Official_Receipt_ID = official_receipt_id;
                                                                official_receipt.Receipt_No = receipt_num;
                                                                official_receipt.Policy_ID = polInfo.PolicyID;
                                                                official_receipt.Customer_ID = polInfo.CustomerID;
                                                                official_receipt.Method_Payment = 2;
                                                                official_receipt.Amount = (status_code == "IF" ? debit.Amount : 0);
                                                                official_receipt.Policy_Type = 1;   // Policy_Individual
                                                                official_receipt.Created_By = user_name;
                                                                official_receipt.Created_On = transaction_date;// debit.TransactionDate;
                                                                official_receipt.Created_Note = "";
                                                                official_receipt.Interest_Amount = 0;
                                                                official_receipt.Entry_Date = DateTime.Now;
                                                                isSaveOfficialReceipt = da_officail_receipt.Insert_Official_Receipt(official_receipt);
                                                                #endregion save official receipt
                                                                if (isSaveOfficialReceipt)
                                                                {
                                                                    save_log += "6. Save Official receipt in table [Ct_Official_Receipt] [SUCCESS]" + Environment.NewLine;
                                                                    #region save method payment
                                                                    bl_method_payment method_payment = new bl_method_payment();

                                                                    method_payment.Method_ID = 2;
                                                                    method_payment.Method_Name = "ផ្សេងៗ/Others";
                                                                    method_payment.Official_Receipt_ID = official_receipt.Official_Receipt_ID;
                                                                    method_payment.Created_By = user_name;
                                                                    method_payment.Created_On = transaction_date;// debit.TransactionDate;
                                                                    method_payment.Created_Note = "Transfer to Cambodia Life's Account Wing (T ID:" + debit.TransactionID + ")";
                                                                    method_payment.Transaction_ID = debit.TransactionID;
                                                                    isSaveMethodPayment = da_method_payment.Insert_Method_Payment(method_payment);
                                                                    #endregion save mothod payment

                                                                    if (isSaveMethodPayment)
                                                                    {
                                                                        save_log += "7. Save method payment in table [Ct_Method_Payment] [SUCCESS]" + Environment.NewLine;
                                                                        #region update payment wing
                                                                        if (da_wing.UpdatePaymentWingByTransactionID(debit.TransactionID, 0, polInfo.PolicyID, official_receipt.Official_Receipt_ID, 0))
                                                                        {
                                                                            save_log += "8. update payment wing in table [Ct_Payment_Wing] [SUCCESS]" + Environment.NewLine;
                                                                        }
                                                                        else
                                                                        {
                                                                            save_log += "8. update payment wing in table [Ct_Payment_Wing] [FAIL]" + Environment.NewLine;
                                                                        }

                                                                        #endregion update payment wing
                                                                        #region save official receipt prem pay
                                                                        bl_official_receipt_prem_pay official_receipt_prem_pay = new bl_official_receipt_prem_pay();
                                                                        official_receipt_prem_pay.Policy_Prem_Pay_ID = pol_prem_pay.Policy_Prem_Pay_ID;
                                                                        official_receipt_prem_pay.Official_Receipt_Prem_Pay_ID = official_receipt.Official_Receipt_ID;
                                                                        official_receipt_prem_pay.Official_Receipt_ID = official_receipt.Official_Receipt_ID;
                                                                        official_receipt_prem_pay.Product_ID = polInfo.ProductID;
                                                                        official_receipt_prem_pay.Sum_Insured = polInfo.SumAssured;
                                                                        official_receipt_prem_pay.Amount = (status_code == "IF" ? debit.Amount : 0);

                                                                        official_receipt_prem_pay.Payment_Type_ID = 2;

                                                                        official_receipt_prem_pay.Created_By = user_name;
                                                                        official_receipt_prem_pay.Created_On = transaction_date;// DateTime.Now;

                                                                        isSaveReceiptPremPay = da_official_receipt_prem_pay.Insert_Official_Receipt_Prem_Pay(official_receipt_prem_pay);
                                                                        #endregion save official receipt prem pay
                                                                        if (isSaveReceiptPremPay)
                                                                        {
                                                                            save_log += "9. Save official receipt prem pay in table [Ct_Official_Receipt_Prem_Pay] [SUCCESS]" + Environment.NewLine;
                                                                        }
                                                                        else
                                                                        {
                                                                            save_log += "9. Save official receipt prem pay in table [Ct_Official_Receipt_Prem_Pay] [FAIL]" + Environment.NewLine;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        save_log += "7. Save method payment in table [Ct_Method_Payment] [FAIL]" + Environment.NewLine;
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    save_log += "6. Save Official receipt in table [Ct_Official_Receipt] [FAIL]" + Environment.NewLine;
                                                                }


                                                            }
                                                            else
                                                            {
                                                                save_log += "5. Save payment wing in table [Ct_Payment_Wing] [FAIL]" + Environment.NewLine;
                                                            }



                                                        }
                                                        else
                                                        {

                                                            save_log += "4. Save payment uploading in table [Ct_Payment_Uploading] [FAIL]" + Environment.NewLine;
                                                        }

                                                    }
                                                    else//policy is not IF
                                                    {
                                                        isSavePaymentWing = true;
                                                        isSaveOfficialReceipt = true;
                                                        isSaveMethodPayment = true;
                                                        isSaveReceiptPremPay = true;
                                                        isSavePaymentUploading = true;
                                                        save_log += "4. Do not save payment uploading in table [Ct_Payment_Uploading]" + Environment.NewLine;
                                                        save_log += "5. Do not save payment wing in table [Ct_Payment_Wing]" + Environment.NewLine;
                                                        save_log += "6. Do not save Official receipt in table [Ct_Official_Receipt]" + Environment.NewLine;
                                                        save_log += "7. Do not save method payment in table [Ct_Method_Payment]" + Environment.NewLine;
                                                        save_log += "8. Do not update payment wing in table [Ct_Payment_Wing]" + Environment.NewLine;
                                                        save_log += "9. Do not save official receipt prem pay in table [Ct_Official_Receipt_Prem_Pay]" + Environment.NewLine;

                                                    }

                                                }
                                                else
                                                {
                                                    save_log += "3. Save policy premium pay in table [ct_policy_prem_pay] [FAIL]" + Environment.NewLine;
                                                }


                                            }
                                            else
                                            {
                                                //save log
                                                save_log += "2. Update policy detail in table [CT_CI_POLICY_DETAIL] [FAIL] [Other transactions were terminated]" + Environment.NewLine;
                                            }

                                        }
                                        else
                                        {
                                            // update policy remark fail
                                            save_log += "1. Update policy remark in [ct_ci_policy] [FAIL] [Other transactions were terminated]" + Environment.NewLine;

                                        }
                                        #endregion Update policy detail in ct_ci_policy_detail



                                        if (isUpdatePolicyRemarks && isUpdatePolicyDetail && isSavePolicyPremPay && isSavePaymentUploading && isSavePaymentWing && isSaveMethodPayment && isSaveOfficialReceipt && isSaveReceiptPremPay)
                                        {
                                            #region Send Message
                                            if (rblSendSMS.SelectedIndex == 0)
                                            {
                                                sms = new SENDSMS();
                                                sms.PhoneNumber = polInfo.PhoneNumber.Replace(" ", "");
                                                sms.MessageCate = "WING";

                                                if (status_code == "IF")
                                                {
                                                    sms.Message = "This is to certify Camlife agreed to insure " + polInfo.FullNameEn + " under Cert. No." + polInfo.PolicyNumber + " from " + effective_date.ToString("dd/MM/yy HH:mm") + ". Visit www.camlife.com.kh/AL to print certificate.";
                                                }
                                                else if (status_code == "LAP")
                                                {
                                                    //fail debit
                                                    sms.Message = "Dear " + polInfo.FullNameEn + ", please deposit to your Wing account to pay premium to get cover from next renewal recurrence. Please call 061 431111 for info.";
                                                }

                                                if (sms.Send())
                                                {
                                                    //save log
                                                    save_log += "10. Message send to [" + polInfo.PhoneNumber + "] [SUCCESS]" + Environment.NewLine;
                                                }
                                                else
                                                {
                                                    //save log
                                                    save_log += "10. Message send to [" + polInfo.PhoneNumber + "] [FAIL]" + Environment.NewLine;
                                                }
                                            }
                                            else
                                            {
                                                //save log
                                                save_log += "10. User does not send message to [" + polInfo.PhoneNumber + "]" + Environment.NewLine;
                                            }


                                            #endregion Send Message

                                            validList.Add(new AutoDebit()
                                            {
                                                TransactionDate = debit.TransactionDate,
                                                TransactionID = debit.TransactionID,
                                                WingAccountNumber = debit.WingAccountNumber,
                                                Amount = debit.Amount,
                                                Remarks = "SUCCESS",
                                                Status = debit.Status,
                                                EffectiveDate = debit.EffectiveDate
                                            });

                                            #region Policy Approver
                                            saveApprovedPolicy(polInfo.PolicyID, Int32.Parse(ddlApprover.SelectedValue), user_name, transaction_date);
                                            #endregion Policy Approver
                                        }
                                        else
                                        {
                                            invalidList.Add(new AutoDebit()
                                            {
                                                TransactionDate = debit.TransactionDate,
                                                TransactionID = debit.TransactionID,
                                                WingAccountNumber = debit.WingAccountNumber,
                                                Amount = debit.Amount,
                                                Remarks = "FAIL",
                                                Status = debit.Status,
                                                EffectiveDate = debit.EffectiveDate
                                            });
                                        }
                                    }

                                    #endregion check status "NEW"

                                    #region check status "IF or LAP"
                                    else if (polInfo.PolicyStatus.Trim().ToUpper() == "IF" || polInfo.PolicyStatus.Trim().ToUpper() == "LAP")
                                    {

                                        #region Update policy remarks in ct_ci_policy
                                        isUpdatePolicyRemarks = da_wing.Policy.UpdatePolicyRemarks(polInfo.PolicyID, debit.Status.ToUpper(), user_name, transaction_date);
                                        #endregion Update policy remarks in ct_ci_policy

                                        #region Save Policy Detail
                                        if (isUpdatePolicyRemarks)
                                        {
                                            save_log += "1. Update policy remark in [ct_ci_policy] [SUCCESS]" + Environment.NewLine;
                                            policy_detail_id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CI_POLICY_DETAIL" }, { "FIELD", "POLICY_DETAIL_ID" } });
                                            isSavePolicyDetail = da_wing.PolicyDetail.SavePolicyDetail(new bl_wing.PolicyDetail()
                                            {

                                                PolicyDetailID = policy_detail_id,// Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CI_POLICY_DETAIL" }, { "FIELD", "POLICY_DETAIL_ID" } }),
                                                PolicyID = polInfo.PolicyID,
                                                EffectiveDate = effective_date,
                                                MaturityDate = maturity_date,
                                                ExpiryDate = expiry_date,
                                                IssuedDate = debit.TransactionDate,
                                                Age = age,
                                                SumAssured = polInfo.SumAssured,
                                                PayModeID = Helper.GetPayModeID(polInfo.PaymentMode),
                                                PaymentBy = polInfo.PaymentBy,
                                                PaymentCode = polInfo.PaymentCode,
                                                UserPremium = (status_code == "IF" ? debit.Amount : 0),
                                                Premium = (status_code == "IF" ? debit.Amount : 0),
                                                RETURN_PREMIUM = (polInfo.UserPremium - polInfo.Premium),
                                                OriginalPremium = polInfo.Originalpremium,
                                                DiscountAmount = 0,
                                                CoverYear = cover_year,
                                                PayYear = pay_year,
                                                CoverUpToAge = cover_to_age,
                                                PayUpToAge = pay_to_age,
                                                PolicyStatusRemarks = status_code,
                                                TransactionID = debit.TransactionID,
                                                CreatedBy = user_name,
                                                CreatedDateTime = transaction_date,
                                                Sequence = da_wing.GetLastSEQ(polInfo.PolicyNumber)
                                            });

                                            if (isSavePolicyDetail)
                                            {
                                                save_log += "2. Save policy detail in [ct_ci_policy_detail] [SUCCESS]" + Environment.NewLine;
                                                isUpdatePolicyStatus = da_wing.PolicyDetail.UpdatePolicyStatus(polInfo.PolicyID, policy_detail_id, status_code, status_code, debit.TransactionID, user_name, transaction_date);
                                                if (isUpdatePolicyStatus)
                                                {
                                                    save_log += "3. Update policy status in [ct_ci_policy_detail & ct_policy_status] [SUCCESS]" + Environment.NewLine;

                                                    #region Save policy prem pay
                                                    int prem_year = da_policy_prem_pay.GetPrem_Year(polInfo.PolicyID);
                                                    prem_year = prem_year == 0 ? 1 : prem_year;
                                                    int prem_lot = da_policy_prem_pay.GetPrem_Lot(polInfo.PolicyID, 4, prem_year);

                                                    bl_policy_prem_pay pol_prem_pay = new bl_policy_prem_pay();
                                                    pol_prem_pay.Policy_Prem_Pay_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_POLICY_PREM_PAY" }, { "FIELD", "POLICY_PREM_PAY_ID" } });
                                                    pol_prem_pay.Policy_ID = polInfo.PolicyID;
                                                    //pol_prem_pay.Due_Date = debit.TransactionDate;
                                                    pol_prem_pay.Due_Date = debit.EffectiveDate.Date;
                                                    pol_prem_pay.Pay_Date = debit.TransactionDate;
                                                    pol_prem_pay.Prem_Year = prem_year;
                                                    pol_prem_pay.Prem_Lot = prem_lot;
                                                    pol_prem_pay.Amount = status_code == "IF" ? debit.Amount : 0;
                                                    pol_prem_pay.Sale_Agent_ID = polInfo.AgentCode;
                                                    pol_prem_pay.Office_ID = "HQ";
                                                    pol_prem_pay.Created_By = user_name;
                                                    pol_prem_pay.Created_On = transaction_date;
                                                    pol_prem_pay.Created_Note = "";
                                                    pol_prem_pay.Pay_Mode_ID = Helper.GetPayModeID(polInfo.PaymentMode);
                                                    isSavePolicyPremPay = da_policy.InsertPolicyPremiumPay(pol_prem_pay);
                                                    if (isSavePolicyPremPay)
                                                    {
                                                        save_log += "4. Save policy premium pay in [ct_policy_prem_pay] [SUCCESS]" + Environment.NewLine;
                                                        //update some information in table ct_policy_prem_pay
                                                        da_policy.UpdatePolicyPremPay(pol_prem_pay.Policy_Prem_Pay_ID, polInfo.ProductID, product.Plan_Code, status_code, polInfo.SumAssured);



                                                        if (status_code == "IF")
                                                        {
                                                            #region save payment uploading record
                                                            string receipt_num = da_policy_prem_pay.Auto_Receipt_Number().ToUpper();
                                                            string official_receipt_id = "";
                                                            official_receipt_id = Helper.GetNewGuid(AppConfiguration.GetConnectionString(), new string[,] { { "TABLE", "Ct_Official_Receipt" }, { "FIELD", "Official_receipt_id" } });
                                                            //delete all existing data
                                                            da_wing.TruncatePaymentUploading();
                                                            isSavePaymentUploading = da_wing.InsertIntoPaymentUploading(polInfo.PolicyID, polInfo.CustomerID, debit.TransactionID, debit.Amount, polInfo.Premium, 0, 0, debit.TransactionDate, polInfo.AgentCode, "HQ", Helper.GetPayModeID(polInfo.PaymentMode), 0, polInfo.SumAssured, polInfo.PolicyStatus, polInfo.ProductID, 1, polInfo.PolicyNumber);
                                                            #endregion save payment uploading record

                                                            if (isSavePaymentUploading)
                                                            {
                                                                save_log += "5. Save payment uploading in table [Ct_Payment_Uploading] [SUCCESS]" + Environment.NewLine;

                                                                #region Save payment wing
                                                                bl_payment_wing payment_wing = new bl_payment_wing();
                                                                string payment_wing_id = "";
                                                                payment_wing_id = Helper.GetNewGuid(AppConfiguration.GetConnectionString(), new string[,] { { "TABLE", "Ct_Payment_Wing" }, { "FIELD", "PAYMENT_WING_ID" } });
                                                                payment_wing.Payment_Wing_ID = payment_wing_id;
                                                                payment_wing.Policy_ID = polInfo.PolicyID;
                                                                payment_wing.Bill_No = debit.WingAccountNumber;
                                                                payment_wing.Transaction_ID = debit.TransactionID;
                                                                payment_wing.Created_Note = "";
                                                                payment_wing.Received_Amount = (status_code == "IF" ? debit.Amount : 0);
                                                                payment_wing.Received_Date = debit.TransactionDate;
                                                                payment_wing.Created_On = transaction_date;// DateTime.Now;
                                                                payment_wing.Created_By = user_name;
                                                                payment_wing.Status_Paid = 1; //0: not paid or createding OR; 1: has been used for creating OR
                                                                payment_wing.Transaction_Type = 1; // transfer from Wing

                                                                isSavePaymentWing = da_wing.InsertIntoPaymentWing(payment_wing);
                                                                #endregion Save payment wing
                                                                if (isSavePaymentWing)
                                                                {
                                                                    save_log += "6. Save payment wing in table [Ct_Payment_Wing] [SUCCESS]" + Environment.NewLine;

                                                                    #region save official receipt
                                                                    bl_official_receipt official_receipt = new bl_official_receipt();
                                                                    official_receipt.Official_Receipt_ID = official_receipt_id;
                                                                    official_receipt.Receipt_No = receipt_num;
                                                                    official_receipt.Policy_ID = polInfo.PolicyID;
                                                                    official_receipt.Customer_ID = polInfo.CustomerID;
                                                                    official_receipt.Method_Payment = 2;
                                                                    official_receipt.Amount = (status_code == "IF" ? debit.Amount : 0);
                                                                    official_receipt.Policy_Type = 1;   // Policy_Individual
                                                                    official_receipt.Created_By = user_name;
                                                                    official_receipt.Created_On = transaction_date;// debit.TransactionDate;
                                                                    official_receipt.Created_Note = "";
                                                                    official_receipt.Interest_Amount = 0;
                                                                    official_receipt.Entry_Date = DateTime.Now;
                                                                    isSaveOfficialReceipt = da_officail_receipt.Insert_Official_Receipt(official_receipt);
                                                                    #endregion save official receipt
                                                                    if (isSaveOfficialReceipt)
                                                                    {
                                                                        save_log += "7. Save Official receipt in table [Ct_Official_Receipt] [SUCCESS]" + Environment.NewLine;
                                                                        #region save method payment
                                                                        bl_method_payment method_payment = new bl_method_payment();

                                                                        method_payment.Method_ID = 2;
                                                                        method_payment.Method_Name = "ផ្សេងៗ/Others";
                                                                        method_payment.Official_Receipt_ID = official_receipt.Official_Receipt_ID;
                                                                        method_payment.Created_By = user_name;
                                                                        method_payment.Created_On = transaction_date;// debit.TransactionDate;
                                                                        method_payment.Created_Note = "Transfer to Cambodia Life's Account Wing (T ID:" + debit.TransactionID + ")";
                                                                        method_payment.Transaction_ID = debit.TransactionID;
                                                                        isSaveMethodPayment = da_method_payment.Insert_Method_Payment(method_payment);
                                                                        #endregion save mothod payment

                                                                        if (isSaveMethodPayment)
                                                                        {
                                                                            save_log += "8. Save method payment in table [Ct_Method_Payment] [SUCCESS]" + Environment.NewLine;
                                                                            #region update payment wing
                                                                            if (da_wing.UpdatePaymentWingByTransactionID(debit.TransactionID, 0, polInfo.PolicyID, official_receipt.Official_Receipt_ID, 0))
                                                                            {
                                                                                save_log += "9. update payment wing in table [Ct_Payment_Wing] [SUCCESS]" + Environment.NewLine;
                                                                            }
                                                                            else
                                                                            {
                                                                                save_log += "9. update payment wing in table [Ct_Payment_Wing] [FAIL]" + Environment.NewLine;
                                                                            }

                                                                            #endregion update payment wing
                                                                            #region save official receipt prem pay
                                                                            bl_official_receipt_prem_pay official_receipt_prem_pay = new bl_official_receipt_prem_pay();
                                                                            official_receipt_prem_pay.Policy_Prem_Pay_ID = pol_prem_pay.Policy_Prem_Pay_ID;
                                                                            official_receipt_prem_pay.Official_Receipt_Prem_Pay_ID = official_receipt.Official_Receipt_ID;
                                                                            official_receipt_prem_pay.Official_Receipt_ID = official_receipt.Official_Receipt_ID;
                                                                            official_receipt_prem_pay.Product_ID = polInfo.ProductID;
                                                                            official_receipt_prem_pay.Sum_Insured = polInfo.SumAssured;
                                                                            official_receipt_prem_pay.Amount = (status_code == "IF" ? debit.Amount : 0);

                                                                            official_receipt_prem_pay.Payment_Type_ID = 2;

                                                                            official_receipt_prem_pay.Created_By = user_name;
                                                                            official_receipt_prem_pay.Created_On = transaction_date;// DateTime.Now;

                                                                            isSaveReceiptPremPay = da_official_receipt_prem_pay.Insert_Official_Receipt_Prem_Pay(official_receipt_prem_pay);
                                                                            #endregion save official receipt prem pay
                                                                            if (isSaveReceiptPremPay)
                                                                            {
                                                                                save_log += "10. Save official receipt prem pay in table [Ct_Official_Receipt_Prem_Pay] [SUCCESS]" + Environment.NewLine;
                                                                            }
                                                                            else
                                                                            {
                                                                                save_log += "10. Save official receipt prem pay in table [Ct_Official_Receipt_Prem_Pay] [FAIL]" + Environment.NewLine;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            save_log += "8. Save method payment in table [Ct_Method_Payment] [FAIL]" + Environment.NewLine;
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        save_log += "7. Save Official receipt in table [Ct_Official_Receipt] [FAIL]" + Environment.NewLine;
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    save_log += "6. Save payment wing in table [Ct_Payment_Wing] [FAIL]" + Environment.NewLine;
                                                                }


                                                            }
                                                            else
                                                            {
                                                                save_log += "5. Save payment uploading in table [Ct_Payment_Uploading] [FAIL]" + Environment.NewLine;

                                                            }

                                                        }
                                                        else //policy is not IF
                                                        {
                                                            isSavePaymentUploading = true;
                                                            isSavePaymentWing = true;
                                                            isSaveOfficialReceipt = true;
                                                            isSaveMethodPayment = true;
                                                            isSaveReceiptPremPay = true;
                                                            save_log += "5. Do not save payment uploading in table [Ct_Payment_Uploading]" + Environment.NewLine;
                                                            save_log += "6. Do not save payment wing in table [Ct_Payment_Wing]" + Environment.NewLine;
                                                            save_log += "7. Do not save Official receipt in table [Ct_Official_Receipt]" + Environment.NewLine;
                                                            save_log += "8. Do not save method payment in table [Ct_Method_Payment]" + Environment.NewLine;
                                                            save_log += "9. Do not update payment wing in table [Ct_Payment_Wing]" + Environment.NewLine;
                                                            save_log += "10. Do not save official receipt prem pay in table [Ct_Official_Receipt_Prem_Pay]" + Environment.NewLine;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        save_log += "4. Save policy premium pay in [ct_policy_prem_pay] [SUCCESS]" + Environment.NewLine;

                                                    }
                                                    #endregion save policy prem pay

                                                }
                                                else
                                                {
                                                    save_log += "3. Update policy status in [ct_ci_policy_detail & ct_policy_status] [FAIL]" + Environment.NewLine;

                                                }
                                            }
                                            else
                                            {
                                                save_log += "2. Save policy detail in [ct_ci_policy_detail] [FAIL]" + Environment.NewLine;
                                            }
                                        }
                                        else
                                        {
                                            save_log += "1. Update policy remark in [ct_ci_policy] [FAIL] [Other transactions were terminated]" + Environment.NewLine;
                                        }
                                        #endregion Save Policy Detail

                                        if (isUpdatePolicyRemarks && isUpdatePolicyStatus && isSavePolicyDetail && isSavePolicyPremPay && isSavePaymentUploading && isSavePaymentWing && isSaveMethodPayment && isSaveOfficialReceipt && isSaveReceiptPremPay)
                                        {
                                            #region Send Message
                                            if (rblSendSMS.SelectedIndex == 0)
                                            {
                                                sms = new SENDSMS();
                                                sms.PhoneNumber = polInfo.PhoneNumber.Replace(" ", "");

                                                sms.MessageCate = "SMS_WING";

                                                if (status_code == "IF")
                                                {
                                                    //Dear [Full Name], your insurance Cert. No.[Wing Acct No] is renewed from [11/MM/YY] 11:59. Visit www.camlife.com.kh/AL to print certificate.
                                                    sms.Message = "Dear " + polInfo.FullNameEn + ", your insurance Cert. No." + polInfo.PolicyNumber + " is renewed from " + effective_date.ToString("dd/MM/yy HH:mm") + ". Visit www.camlife.com.kh/AL to print certificate.";

                                                }
                                                else if (status_code == "LAP")
                                                {
                                                    //fail debit
                                                    //Dear [Full Name], your insurance Cert. No.[Wing Account No] is expired on [11/MM/YY] 11:59. Please pay premium to get cover from next renewal recurrence.
                                                    sms.Message = "Dear " + polInfo.FullNameEn + ", your insurance Cert. No." + polInfo.PolicyNumber + " is expired on " + polInfo.EffectiveDate.ToString("dd/MM/yy HH:mm") + ". Please pay premium to get cover from next renewal recurrence.";
                                                }

                                                if (sms.Send())
                                                {
                                                    //save log
                                                    save_log += "11. Message send to [" + polInfo.PhoneNumber + "] [SUCCESS]" + Environment.NewLine;
                                                }
                                                else
                                                {
                                                    //save log
                                                    save_log += "11. Message send to [" + polInfo.PhoneNumber + "] [FAIL]" + Environment.NewLine;
                                                }
                                            }
                                            else
                                            {
                                                //save log
                                                save_log += "11. User does not send message to [" + polInfo.PhoneNumber + "]" + Environment.NewLine;
                                            }


                                            #endregion Send Message


                                            validList.Add(new AutoDebit()
                                            {
                                                TransactionDate = debit.TransactionDate,
                                                TransactionID = debit.TransactionID,
                                                WingAccountNumber = debit.WingAccountNumber,
                                                Amount = debit.Amount,
                                                Remarks = "SUCCESS",
                                                Status = debit.Status,
                                                EffectiveDate = debit.EffectiveDate
                                            });
                                        }
                                        else
                                        {
                                            invalidList.Add(new AutoDebit()
                                            {
                                                TransactionDate = debit.TransactionDate,
                                                TransactionID = debit.TransactionID,
                                                WingAccountNumber = debit.WingAccountNumber,
                                                Amount = debit.Amount,
                                                Remarks = "FAIL",
                                                Status = debit.Status,
                                                EffectiveDate = debit.EffectiveDate
                                            });
                                        }

                                    }
                                    #endregion check status "IF or LAP"

                                }
                                else
                                {
                                    //store unsaved data
                                    invalidList.Add(new AutoDebit()
                                    {
                                        WingAccountNumber = debit.WingAccountNumber,
                                        TransactionID = debit.TransactionID,
                                        TransactionDate = debit.TransactionDate,
                                        Amount = debit.Amount,
                                        Status = debit.Status,
                                        Remarks = "Duplicate Effective Date.",
                                        EffectiveDate = debit.EffectiveDate
                                    });

                                }
                            }
                            else
                            {
                                // age is not in ranges
                                //store unsaved data
                                invalidList.Add(new AutoDebit()
                                {
                                    WingAccountNumber = debit.WingAccountNumber,
                                    TransactionID = debit.TransactionID,
                                    TransactionDate = debit.TransactionDate,
                                    Amount = debit.Amount,
                                    Status = debit.Status,
                                    Remarks = "Age [" + age + "] not in range.",
                                    EffectiveDate = debit.EffectiveDate
                                });
                            }
                            #endregion Check Over Age
                        }

                        else
                        {
                            //save log file
                            save_log += "System cannot find information of policy number [" + debit.WingAccountNumber + "]" + Environment.NewLine;
                        }

                        save_log += Environment.NewLine; // new line

                        // RESET VARIABLE VALUE
                        effective_date = new DateTime();
                        maturity_date = new DateTime();
                        expiry_date = new DateTime();
                        age = 0;
                        pay_to_age = 0;
                        pay_to_age = 0;
                        cover_to_age = 0;
                        status_code = "";

                    }
                    #endregion Loop in debit_list

                    //SAVE LOG FILE
                    SaveLog(save_log + "End Saved [" + DateTime.Now.ToString("dd MMM yyyy HH:mm:ss") + "]>>");
                    //BIND DATA IN GRID VIEW
                    if (validList.Count > 0 && invalidList.Count > 0)
                    {
                        AlertMessage("Some data cannot be saved, please view detail in [Fail] tab.");
                    }
                    else if (validList.Count > 0 && invalidList.Count == 0)
                    {
                        AlertMessage("Data were saved successfuly, please view detail in [Success] tab.");
                    }
                    else if (validList.Count == 0 && invalidList.Count > 0)
                    {
                        AlertMessage("Data cannot be saved, please view detail in [Fail] tab.");
                    }
                    gv_valid.DataSource = validList;
                    gv_valid.DataBind();
                    gv_invalid.DataSource = invalidList;
                    gv_invalid.DataBind();
                    export_excel.Visible = invalidList.Count > 0 ? true : false;

                    if (validList.Count > 0 || invalidList.Count > 0)
                    {
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User saves auto debit data [Total success:", validList.Count, " Total fail:", invalidList.Count, ", with option send SMS:", rblSendSMS.SelectedItem.Text,"]."));
                    }

                }
                else
                {
                    AlertMessage("No data save.");
                }
                //reset session
                Session["AUTO_DEBIT_LIST"] = null;
            }
        }
        else // not selected approver
        {
            AlertMessage("Please selecte approver.");
        }
    }

    class AutoDebit
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionID { get; set; }
        public string WingAccountNumber { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
    void SaveLog(string log)
    {
        bl_wing.SaveLog("wing_save_auto_debit_transaction", log);
    }
    void UploadTransactionLog(string log)
    {
        bl_wing.SaveLog("wing_upload_auto_debit_transaction", log);
    }

    void Excel()
    {
        int row_count = gv_invalid.Rows.Count;
        if (row_count > 0)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("consent_form_list");
            Helper.excel.Sheet = sheet1;
            //columns name
            Helper.excel.HeaderText = new string[] { "No.", "TransactionDate", "TransactionID", "Wing Account", "Amount", "Status", "Remarks" };
            Helper.excel.generateHeader();
            //disign row
            int row_no = 0;
            foreach (GridViewRow row in gv_invalid.Rows)
            {
                #region //Variable
                Label tran_date = (Label)row.FindControl("lblTransactionDate");
                Label tran_id = (Label)row.FindControl("lblTransactionID");
                Label wing_acc = (Label)row.FindControl("lblWingAccount");
                Label amount = (Label)row.FindControl("lblAmount");
                Label status = (Label)row.FindControl("lblTransactionStatus");
                Label phone_number = (Label)row.FindControl("lblPhoneNumber");
                Label remarks = (Label)row.FindControl("lblRemarks");
                #endregion

                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(row_no);

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(tran_date.Text);
                //HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                //style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy hh:mm:ss");
                //Cell2.CellStyle = style;

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(tran_id.Text);

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(wing_acc.Text);

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue(amount.Text);

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(status.Text);

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(remarks.Text);

            }
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports invalid auto debit data [Total record(S):", gv_invalid.Rows.Count, "]."));
            string filename = "camlife_wing_auto_debit" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            Response.BinaryWrite(file.GetBuffer());

            Response.End();
        }
        else
        {
            AlertMessage("No data to export.");
        }

    }

    bool saveApprovedPolicy(string policy_id, int approver_id, string created_by, DateTime created_on)
    {
        bool result = false;
        da_report_approver.bl_report_approver_policy approver_policy;

        if (approver_id != 0) // new policy approved
        {
            //check existing approved policy
            da_report_approver.bl_report_approver aprov_obj = da_report_approver.GetAproverInfo(policy_id);
            if (aprov_obj.ID == 0)
            {
                approver_policy = new da_report_approver.bl_report_approver_policy();
                approver_policy.Approver_ID = approver_id;
                approver_policy.Policy_ID = policy_id;
                approver_policy.Created_On = created_on;
                approver_policy.Created_By = created_by;
                result = da_report_approver.InsertApproverPolicy(approver_policy);
            }

        }
        return result;
    }
}