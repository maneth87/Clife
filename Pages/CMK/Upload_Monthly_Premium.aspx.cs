using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CMK_Upload_Monthly_Premium : System.Web.UI.Page
{
    List<bl_cmk_load_data> list_data = new List<bl_cmk_load_data>();

    string err_message = "";
    DateTime rollback_report_date;
    DateTime report_date;
    string sortEx = "";
    string sortCol = "";

    DataTable tbl_fail;
    DataTable tbl_invalid;

    int row_index_fail = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

            if (user_name == "admin")
            {
                DeleteReport.Style["display"] = "block";
            }
            else
            {
                DeleteReport.Style["display"] = "none";
            }

        }
        DIV_UPLOAD_TAB.Style["display"] = "none";
        DIV_RESULT.Style["display"] = "none";    
    }

    List<DataCollection> success_list = new List<DataCollection>();

    List<DataCollection> fail_list = new List<DataCollection>();

    #region PMA REPORT VARIABLE
    DateTime CMK_Effective_Date; string mode = "", product_id = "", product_name = "";
    #endregion


    //Upload Flexi Term Policies using Excel file
    protected void ImgBtnSave_Click(object sender, ImageClickEventArgs e)
    {
        string exist_loan_id = "";
        string rollback_customer_id = "";
        string rollback_policy_id = "";
        int row_count = 1; int saveStatus;
        bool IsReportDate = false;

        List<bl_cmk_load_data> listData = new List<bl_cmk_load_data>();
        saveStatus = Convert.ToInt32(ActionList.SelectedValue);

        try
        {
            if (txtReport_date.Text.Trim() != "")
            {
                IsReportDate = da_cmk.Policy.CheckExistPremiumReportDate(Helper.FormatDateTime(txtReport_date.Text.Trim()));
                if (IsReportDate)
                {
                    if (saveStatus == 1)
                    {
                        AlertMessage("Deny duplication report date!!");
                        return;
                    }
                    else
                    {
                        goto EXECUTION;
                    }
                }
                else
                {
                    if (saveStatus == 2)
                    {
                        AlertMessage("Can't find report date in system!!");
                        return;
                    }
                    else
                    {
                        goto EXECUTION;
                    }
                }

                EXECUTION:
                    listData = (List<bl_cmk_load_data>)Session["cmk_upload_valid_data"];

                    if (listData.Count > 0)
                    {
                        foreach (bl_cmk_load_data cmk_data in listData)
                        {
                            #region //Check EXIST LOAN

                            List<bl_cmk.CMK_Policy> Policy = new List<bl_cmk.CMK_Policy>();
                            Policy = da_cmk.Policy.GetPolicyByLoanID(cmk_data.LoanID);

                            string cmk_policy_id = "";

                            if (Policy.Count > 0)
                            {
                                for (int i = 0; i < Policy.Count; i++)
                                {
                                    if ((Policy[i].CMKCustomerID == cmk_data.CMKCustomerID && Policy[i].CertificateNo == cmk_data.CertificateNo) && (Policy[i].LoanID == cmk_data.LoanID))
                                    {
                                        exist_loan_id = Policy[i].LoanID;
                                        cmk_policy_id = Policy[i].CMKPolicyID;
                                        break;
                                    }
                                    else
                                    {
                                        exist_loan_id = "";
                                    }
                                }
                            }
                            else
                            {
                                exist_loan_id = "";
                            }

                            #endregion

                            if (exist_loan_id != "")
                            {
                                #region // EXIST LOAN THEN SAVE NEW COMING PREMIUM DETAIL
                                bool IsTrue = false;

                                IsTrue = da_cmk.Policy.CheckExistPremiumReport(cmk_policy_id, cmk_data.ReportDate);

                                //CHECK IF REPORT DATE EXIST
                                if (!IsTrue)
                                {
                                    bool IsSave = false;
                                    bl_cmk.CMK_Policy_Prem Premium = da_cmk.Policy.VerifyPolicyPremium(cmk_policy_id);

                                    if ((cmk_data.TotalPremium - Premium.TotalPremium) >= 0.02 && (Premium.TotalPremium <= cmk_data.TotalPremium))
                                    {
                                        #region FAIL LIST

                                        SaveDataFail(cmk_data, "Amount uploaded don't match with system's amount.", row_count, 1);

                                        #endregion
                                    }
                                    else
                                    {
                                        if ((Premium.LoanAmount == cmk_data.LoanAmount) && (Premium.AssuredAmount == cmk_data.AssuredAmount) && (Premium.OutstandingBalance == cmk_data.OutstandingBalance))
                                        {
                                            #region SAVE NEW REPORT OF PREMIUM

                                            IsSave = Save(cmk_data, cmk_policy_id);

                                            if (IsSave)
                                            {
                                                // Prevent when case error occur then rollback only premium
                                                rollback_policy_id = cmk_policy_id;
                                                rollback_report_date = cmk_data.ReportDate;
                                                report_date = cmk_data.ReportDate;
                                            }
                                            else
                                            {
                                                err_message = "[" + Membership.GetUser().UserName + "] Save policy premium fail Certificate No[" + cmk_data.CertificateNo + "]";

                                                goto EXIT;
                                            }

                                            #endregion

                                            #region UPDATE POLICY STATUS

                                            bl_policy_status objPolicy = new bl_policy_status();
                                            objPolicy.Policy_ID = cmk_policy_id;
                                            objPolicy.Policy_Status_Type_ID = cmk_data.PolicyStatus;
                                            objPolicy.Updated_By = Membership.GetUser().UserName;
                                            objPolicy.Updated_On = DateTime.Now;
                                            objPolicy.Updated_Note = cmk_data.CreatedNoted;

                                            if (!da_policy.UpdatePolicyStatus(objPolicy))
                                            {
                                                err_message = "[" + Membership.GetUser().UserName + "] Save policy status fail Certificate No[" + cmk_data.CertificateNo + "]";
                                                goto EXIT;
                                            }

                                            #endregion

                                            #region SUCCESS LIST

                                            SaveDataSuccess(cmk_data);

                                            #endregion
                                        }
                                        else
                                        {
                                            #region FAIL LIST

                                            SaveDataFail(cmk_data, "Amount uploaded don't match with system's amount.", row_count, 1);

                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    #region FAIL LIST

                                    SaveDataFail(cmk_data, "Duplicate Report Date", row_count, 1);

                                    #endregion
                                }

                                #endregion
                            }
                            else
                            {
                                // NOT EXIST POLICY MAIN
                            }

                        EXIT:

                            #region LOG & ROLLBACK

                            if (err_message != "")
                            {
                                //Rollback data
                                Log.CreateLog("CMK_Log", err_message);
                                if (da_cmk.Policy.RollBack(rollback_policy_id, rollback_customer_id, rollback_report_date))
                                {
                                    SaveDataFail(cmk_data, "System error occured!", row_count, 1);
                                    Log.CreateLog("CMK_Log", "System was rollback data successfully.");
                                }
                                else
                                {
                                    SaveDataFail(cmk_data, "System error occured!", row_count, 1);
                                    Log.CreateLog("CMK_Log", "System was rollback data fail.");
                                }
                            }

                            #endregion

                            row_count++;

                            #region CLEAR VARIABLE

                            err_message = "";
                            rollback_policy_id = "";
                            rollback_customer_id = "";
                            rollback_customer_id = "";
                            #endregion

                        }

                        #region // SAVE CMK GROUP PREM - TABLE CMK_GROUP_PREMIUM(PROCESS PMA REPORT) ON 26th-FEBRUARY-2021

                        bl_cmk.bl_cmk_total_policy sum_prem = new bl_cmk.bl_cmk_total_policy();
                        sum_prem = da_cmk.Policy.GetSumPolicyPremium(report_date);

                        if (sum_prem.Number_Of_Policy > 0)
                        {
                            bl_cmk.CMK_Group_Premium group_prem = new bl_cmk.CMK_Group_Premium();

                            if (saveStatus == 1)
                            {
                                group_prem.CMK_Group_Policy_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CMK_GROUP_PREMIUM" }, { "FIELD", "CMK_Group_Policy_ID" } });
                                group_prem.Group_Code = "GR-CR-003";
                                group_prem.Product_ID = product_id;
                                group_prem.Product_Name = product_name;
                                group_prem.Effective_Date = CMK_Effective_Date;
                                group_prem.Sum_Insure = Math.Round(sum_prem.Total_Sum_Insure, 2, MidpointRounding.AwayFromZero);
                                group_prem.Amount = Math.Round(sum_prem.Total_Premium, 2, MidpointRounding.AwayFromZero);
                                group_prem.Invoice_No = "";

                                string CalPremYearLot = da_cmk.Policy.CalPremYearLot(4, CMK_Effective_Date, report_date); // GENERATE PAY YEAR, LOT
                                string[] str_prem_year_lot = CalPremYearLot.Split(',');

                                group_prem.Pay_Year = str_prem_year_lot[0] != "" ? Convert.ToInt32(str_prem_year_lot[0]) : 0;
                                group_prem.Pay_Lot = str_prem_year_lot[1] != "" ? Convert.ToInt32(str_prem_year_lot[1]) : 0;
                                group_prem.Mode = mode;
                                group_prem.Report_Date = report_date;
                                group_prem.Number_Of_Policy = sum_prem.Number_Of_Policy;
                                group_prem.Created_On = DateTime.Now;
                                group_prem.Created_By = Membership.GetUser().UserName;

                                da_cmk.Policy.SaveGroupPremium(group_prem);
                            }
                            else
                            {
                                string cmk_group_policy_id = "";
                                cmk_group_policy_id = da_cmk.Policy.GetGroupPremiumReport(report_date);

                                if (cmk_group_policy_id != "")
                                {
                                    group_prem.CMK_Group_Policy_ID = cmk_group_policy_id;
                                    group_prem.Sum_Insure = Math.Round(sum_prem.Total_Sum_Insure, 2, MidpointRounding.AwayFromZero);
                                    group_prem.Amount = Math.Round(sum_prem.Total_Premium, 2, MidpointRounding.AwayFromZero);
                                    group_prem.Invoice_No = "";
                                    group_prem.Report_Date = report_date;
                                    group_prem.Number_Of_Policy = sum_prem.Number_Of_Policy;
                                    group_prem.Updated_On = DateTime.Now;
                                    group_prem.Updated_By = Membership.GetUser().UserName;

                                    da_cmk.Policy.UpdateGroupPremium(group_prem);
                                }

                            }

                        }

                        #endregion

                        rollback_report_date = new DateTime(1900, 1, 01);
                        report_date = new DateTime(1900, 1, 01);
                        product_id = "";
                        product_name = "";
                        CMK_Effective_Date = new DateTime(1900, 1, 01);
                        mode = "";

                        #region // Display existing customer & Error

                        LoadViewData();

                        if (err_message == "")
                        {
                            if (success_list.Count > 0 && fail_list.Count > 0)
                            {
                                AlertMessage("System saved data successfully, but some datas cannot be saved, please check in [Fail tab].");
                            }
                            else if (success_list.Count > 0 && fail_list.Count <= 0)
                            {
                                AlertMessage("System saved " + success_list.Count + " successfully.");
                            }
                            else
                            {
                                AlertMessage("System save failed!!, please check in [Fail tab].");
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        AlertMessage("There's no any data to save!");
                    }
            }
            else
            {
                AlertMessage("Please choose report date!!");
            }

        }
        catch (Exception ex)
        {
            #region EX

            AlertMessage("System upload data fail.");
            if (da_cmk.Policy.RollBack(rollback_policy_id, rollback_customer_id, rollback_report_date))
            {
                Log.CreateLog("CMK_Log", "System was rollback data successfully.");
            }
            else
            {
                Log.CreateLog("CMK_Log", "System was rollback data fail.");
            }
            Log.AddExceptionToLog("Error fucntion [save_data()] in class [Upload_Policy_Cmk.aspx.cs], detail:" + ex.Message);

            #endregion
        }
    }

    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        try
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";

            if (txtReport_date.Text.Trim() != "")
            {
                if ((FileUploadCmkPolicy.PostedFile != null) && !string.IsNullOrEmpty(FileUploadCmkPolicy.PostedFile.FileName))
                {
                    //check valid
                    string message;
                    if (Validate(out message) != false)
                    {
                        string save_path = "~/Upload/";
                        string file_name = Path.GetFileName(FileUploadCmkPolicy.PostedFile.FileName);
                        string extension = Path.GetExtension(file_name);
                        file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
                        string file_path = save_path + file_name;
                        int row_number = 0;
                        int row_index = 1;

                        FileUploadCmkPolicy.SaveAs(Server.MapPath(file_path));//save file 
                        ExcelConnection my_excel = new ExcelConnection();
                        my_excel.FileName = Server.MapPath(file_path);
                        my_excel.CommandText = "SELECT * FROM [Upload_Format$]";
                        DataTable my_data = new DataTable();
                        my_data = my_excel.GetData();

                        ViewState["my_data"] = my_data;
                        row_number = my_data.Rows.Count;

                        foreach (DataRow row in my_data.Rows)
                        {
                            bl_cmk_load_data cmk_load_data = new bl_cmk_load_data();

                            if (row[1].ToString().Trim() != "" && row[2].ToString().Trim() != "" && row[3].ToString().Trim() != "" && row[4].ToString().Trim() != "" 
                                && row[5].ToString().Trim() != "" && row[6].ToString().Trim() != "" && row[7].ToString().Trim() != "" && row[8].ToString().Trim() != ""
                                && row[9].ToString().Trim() != "" && row[10].ToString().Trim() != "" && row[11].ToString().Trim() != "" && row[13].ToString().Trim() != "")
                            {
                                if (validRow(row, my_data.Rows.IndexOf(row)))
                                {
                                    #region VALID

                                    //Customer Props
                                    cmk_load_data.Row_Number = row_index;

                                    //Policy Props
                                    cmk_load_data.CMKCustomerID = row[1].ToString().Trim();
                                    cmk_load_data.LoanID = row[2].ToString();
                                    cmk_load_data.CertificateNo = row[3].ToString();
                                    cmk_load_data.PolicyStatus = row[6].ToString();
                                    cmk_load_data.Currancy = row[8].ToString();

                                    //Premium Props
                                    if (Helper.GetCurrancy(cmk_load_data.Currancy) != 0) // -> KHR
                                    {
                                        if (Math.Abs(Convert.ToDouble(row[4].ToString().Trim())) >= (500 * 4000)) //Minimum loan cover is >= 2000000 USD
                                        {
                                            cmk_load_data.LoanAmount = row[4].ToString().Trim() != "" && Convert.ToDouble(row[4].ToString().Trim()) > 4000 ? (Convert.ToDouble(row[4].ToString().Trim()) / 4000) : 0.0;
                                            cmk_load_data.OutstandingBalance = row[5].ToString().Trim() != "" && Math.Abs(Convert.ToDouble(row[5].ToString().Trim())) > 4000 ? (Convert.ToDouble(row[5].ToString().Trim()) / 4000) : 0.0;
                                            cmk_load_data.AssuredAmount = row[7].ToString().Trim() != "" && Convert.ToDouble(row[7].ToString().Trim()) > 4000 ? (Convert.ToDouble(row[7].ToString().Trim()) / 4000) : 0.0;

                                            cmk_load_data.LoanAmountRiel = row[4].ToString().Trim() != "" ? Convert.ToDouble(row[4].ToString().Trim()) : 0.0;
                                            cmk_load_data.OutstandingBalanceRiel = row[5].ToString().Trim() != "" ? Convert.ToDouble(row[5].ToString().Trim()) : 0.0;
                                            cmk_load_data.AssuredAmountRiel = row[7].ToString().Trim() != "" ? Convert.ToDouble(row[7].ToString().Trim()) : 0.0;
                                        }
                                        else
                                        {
                                            INVALID_DATA(cmk_load_data, row, row_index_fail);
                                            continue;
                                        }

                                    }
                                    else // -> USD
                                    {
                                        if (Math.Abs(Convert.ToDouble(row[4].ToString().Trim())) >= 500)  //Minimum loan cover is >= 500USD
                                        {
                                            cmk_load_data.LoanAmount = row[4].ToString().Trim() != "" ? Convert.ToDouble(row[4].ToString().Trim()) : 0.0;
                                            cmk_load_data.OutstandingBalance = row[5].ToString().Trim() != "" ? Convert.ToDouble(row[5].ToString().Trim()) : 0.0;
                                            cmk_load_data.AssuredAmount = row[7].ToString().Trim() != "" ? Convert.ToDouble(row[7].ToString().Trim()) : 0.0;

                                            cmk_load_data.LoanAmountRiel = 0.0;
                                            cmk_load_data.OutstandingBalanceRiel = 0.0;
                                            cmk_load_data.AssuredAmountRiel = 0.0;
                                        }
                                        else
                                        {
                                            INVALID_DATA(cmk_load_data, row, row_index_fail);
                                            continue;
                                        }
                                        
                                    }

                                    cmk_load_data.PaymodeID = Helper.GetPayModeID(row[9].ToString().Trim() != "" ? row[9].ToString().Trim() : "monthly");

                                    cmk_load_data.MonthlyPremium = row[10].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[10].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
                                    cmk_load_data.PremiumAfterDiscount = row[11].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[11].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
                                    cmk_load_data.ExtraPremium = row[12].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[12].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
                                    cmk_load_data.TotalPremium = row[13].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[13].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;

                                    cmk_load_data.ReportDate = DateTime.ParseExact(txtReport_date.Text, "dd/MM/yyyy", null);
                                    cmk_load_data.PaymentBatchNo = "";
                                    cmk_load_data.PaidOffInMonth = row[14].ToString();
                                    cmk_load_data.TerminateDate = row[15].ToString();
                                    cmk_load_data.CreatedNoted = row[16].ToString();

                                    list_data.Add(cmk_load_data);

                                    row_index++;

                                    #endregion
                                }

                            }
                            else
                            {
                                #region INVALID DATA

                                INVALID_DATA(cmk_load_data, row, row_index_fail);

                                #endregion
                            }

                        } // end loop statement
                        gv_valid.DataSource = list_data;
                        gv_valid.DataBind();
                        Session["cmk_upload_valid_data"] = list_data;

                        gv_invalid.DataSource = Session["cmk_upload_invalid_data"];
                        gv_invalid.DataBind();

                        if (list_data.Count > 0)
                        {
                            lblCountValid.Text = "Display: " + gv_valid.Rows.Count + " Of " + list_data.Count;
                            ImgBtnSave.Enabled = true;
                        }
                        else
                        {
                            ImgBtnSave.Enabled = false;
                        }

                    }
                    else
                    {
                        AlertMessage(message);
                        return;
                    }

                    DIV_UPLOAD_TAB.Style["display"] = "block";

                    if (gv_invalid.Rows.Count > 0)
                    {
                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(Session["cmk_upload_invalid_data"]);
                        tbl_invalid = JsonConvert.DeserializeObject<DataTable>(json);

                        ViewState["CMK_UPLOAD_INVALID_DATA"] = tbl_invalid;
                        ViewState["FAIL_DATA"] = new DataTable(); // prevent when exporting null value

                        /*show record count*/
                        lblCountInvalid.Text = "Display: " + gv_invalid.Rows.Count + " Of " + tbl_invalid.Rows.Count;
                        export_excel_invalid.Enabled = true;
                    }
                    else
                    {
                        export_excel_invalid.Enabled = false;
                    }

                    Session["cmk_upload_invalid_data"] = null;
                }
                else
                {
                    AlertMessage("Please select your file.");
                }
            }
            else
            {
                AlertMessage("Report Date Is Required.");
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnLoadData_Click] in page [Upload_Policy_Cmk.aspx.cs], detail: " + ex.Message);
            AlertMessage("Internal Error Occured, please contact system administrator");
        }
    }

    void LoadViewData()
    {
        DIV_UPLOAD_TAB.Style["display"] = "none";
        DIV_RESULT.Style["display"] = "block";

        #region CLEAR INVALID
        gv_valid.DataSource = new DataTable();
        gv_invalid.DataSource = new DataTable();

        #endregion

        #region SUCCESS LIST
        gv_success.DataSource = Session["SUCCESS_DATA"];
        gv_success.DataBind();
        #endregion

        #region FAIL LIST

        gv_fail.DataSource = Session["FAIL_DATA"];
        gv_fail.DataBind();
        if (gv_fail.Rows.Count > 0)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Session["FAIL_DATA"]);
            tbl_fail = JsonConvert.DeserializeObject<DataTable>(json);
            ViewState["FAIL_DATA"] = tbl_fail;
            export_excel.Enabled = true;
        }
        else
        {
            ViewState["FAIL_DATA"] = new DataTable();
            export_excel.Enabled = false;
        }

        #endregion

        ViewState["CMK_UPLOAD_INVALID_DATA"] = new DataTable();

        ImgBtnSave.Enabled = false;

    }

    bool Validate(out string message)
    {
        bool status = true;
        message = "";
        string file_path = "";
        //check sheet name
        if ((FileUploadCmkPolicy.PostedFile != null) && !string.IsNullOrEmpty(FileUploadCmkPolicy.PostedFile.FileName))
        {
            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(FileUploadCmkPolicy.PostedFile.FileName);
            string extension = Path.GetExtension(file_name);
            file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
            file_path = save_path + file_name;

            FileUploadCmkPolicy.SaveAs(Server.MapPath(file_path));//save file 

            ExcelConnection my_excel = new ExcelConnection();
            my_excel.FileName = Server.MapPath(file_path);

            if (my_excel.GetSheetName() != "Upload_Format$")
            {
                message = "File is not correct format, please donwload file template from the system.";
                status = false;
            }
            else
            {
                my_excel.CommandText = "Select * from [Upload_Format$]";
                DataTable tbl = my_excel.GetData();
                int col_count = 0;
                col_count = tbl.Columns.Count;
                if (col_count > 30 || col_count < 0)//check number of columns
                {
                    message = "File is not correct format, please donwload file template from the system.";
                    status = false;
                }
                else
                {//check column name.
                    if (tbl.Columns[0].ColumnName.Trim() != "No#" || tbl.Columns[1].ColumnName.Trim() != "Customer ID" || tbl.Columns[2].ColumnName.Trim() != "Loan ID" 
                        || tbl.Columns[3].ColumnName.Trim() != "Certificate No" || tbl.Columns[4].ColumnName.Trim() != "Amount" || tbl.Columns[5].ColumnName.Trim() != "Outstanding Balance"
                        || tbl.Columns[6].ColumnName.Trim() != "Policy Status" || tbl.Columns[7].ColumnName.Trim() != "Assured Amount" || tbl.Columns[8].ColumnName.Trim() != "Currancy"
                        || tbl.Columns[9].ColumnName.Trim() != "Payment Mode" || tbl.Columns[10].ColumnName.Trim() != "Monthly Premium" || tbl.Columns[11].ColumnName.Trim() != "Premium _After Discount"
                        || tbl.Columns[12].ColumnName.Trim() != "Extra Premium" || tbl.Columns[13].ColumnName.Trim() != "Total Premium"
                        || tbl.Columns[14].ColumnName.Trim() != "Paid Off In Month" || tbl.Columns[15].ColumnName.Trim() != "Terminate Date" || tbl.Columns[16].ColumnName.Trim() != "Remarks")
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        status = false;
                    }
                }

            }
        }
        //delete file
        File.Delete(Server.MapPath(file_path));
        return status;
    }

    List<DataCollection> invalid_list = new List<DataCollection>();
    bool validRow(DataRow row, int row_index)
    {
        bool valid = true;

        DataCollection fail_data = new DataCollection();
        try
        {
            #region //Check invalid data
            if (row[1].ToString().Trim() == "") //CMK Customer ID
            {
                valid = false;
                fail_data.Remarks = "Customer ID is required.";
            }
            else if (row[2].ToString().Trim() == "") //Loan ID
            {
                valid = false;
                fail_data.Remarks = "Loan ID is required.";
            }
            else if (row[3].ToString().Trim() == "") // Certificate No
            {
                valid = false;
                fail_data.Remarks = "Certificate No is required.";
            }
            else if (row[6].ToString().Trim() == "") // Policy Status
            {
                valid = false;
                fail_data.Remarks = "Policy Status is required.";
            }
            else if (row[8].ToString().Trim() == "") // Currancy
            {
                valid = false;
                fail_data.Remarks = "Currency is required.";
            }
            else if (row[9].ToString().Trim() == "") // Mode
            {
                valid = false;
                fail_data.Remarks = "Payment Mode is required.";
            }
            else if (Helper.GetCurrancy(row[8].ToString().Trim()) == -1) // Currancy
            {
                valid = false;
                fail_data.Remarks = "Currency is not valid.";
            }

            if (row[4].ToString().Trim() == "") // Loan Amount
            {
                valid = false;
                fail_data.Remarks = "Loan Amount is required.";
            }
            else if (row[5].ToString().Trim() == "") // Outstanding Balance
            {
                valid = false;
                fail_data.Remarks = "Outstanding Balance is required.";
            }
            else if (row[6].ToString().Trim() == "")
            {
                valid = false;
                fail_data.Remarks = "Policy Status is required.";
            }
            else if (row[7].ToString().Trim() == "") // Assure amount
            {
                valid = false;
                fail_data.Remarks = "Assure amount is required.";
            }
            else if (row[10].ToString().Trim() == "") // MonthlyPremium
            {
                valid = false;
                fail_data.Remarks = "Monthly Premium is required.";
            }
            else if (row[11].ToString().Trim() == "") // Prem discou
            {
                valid = false;
                fail_data.Remarks = "Premium After discount is required.";
            }
            else if (row[13].ToString().Trim() == "") // Ttol
            {
                valid = false;
                fail_data.Remarks = "Total Premium is required.";
            }

            if (row[6].ToString().Trim() != "")
            {
                //MAT NEW TER CAN LAP SUR PEN IF
                string policy_status = "";
                policy_status = (row[6].ToString().Trim()).ToUpper();

                switch (policy_status)
                {
                    case "NEW":
                        policy_status = "NEW";
                        break;

                    case "IF":
                        policy_status = "IF";
                        break;

                    case "LAP":
                        policy_status = "LAP";
                        break;

                    case "TER":
                        policy_status = "TER";
                        break;

                    case "SUR":
                        policy_status = "SUR";
                        break;

                    case "CAN":
                        policy_status = "CAN";
                        break;

                    case "MAT":
                        policy_status = "MAT";
                        break;

                    case "PEN":
                        policy_status = "PEN";
                        break;

                    default:
                        valid = false;
                        fail_data.Remarks = "Policy Status is incorrect!!";
                        break;
                }

            }
            
            
            //store invalid data
            if (!valid)
            {
                #region EXIST LOAN TO FAIL LIST

                fail_data.Row_Number = row_index;
                fail_data.CMKCustomerID = row[1].ToString().Trim();
                fail_data.LoanID = row[2].ToString();
                fail_data.CertificateNo = row[3].ToString();
                fail_data.LoanAmount = row[4].ToString().Trim() != "" ? Convert.ToDouble(row[4].ToString().Trim()) : 0.0;
                fail_data.OutstandingBalance = row[5].ToString().Trim() != "" ? Convert.ToDouble(row[5].ToString().Trim()) : 0.0;
                fail_data.AssuredAmount = row[7].ToString().Trim() != "" ? Convert.ToDouble(row[7].ToString().Trim()) : 0.0;
                fail_data.PolicyStatus = row[6].ToString().Trim();
                fail_data.Currancy = row[8].ToString();
                fail_data.PaymodeID = Helper.GetPayModeID(row[9].ToString().Trim() != "" ? row[9].ToString().Trim() : "monthly");
                fail_data.MonthlyPremium = row[10].ToString().Trim() != "" ? Convert.ToDouble(row[10].ToString().Trim()) : 0.0;
                fail_data.PremiumAfterDiscount = row[11].ToString().Trim() != "" ? Convert.ToDouble(row[11].ToString().Trim()) : 0.0;
                fail_data.ExtraPremium = row[12].ToString().Trim() != "" ? Convert.ToDouble(row[12].ToString().Trim()) : 0.0;
                fail_data.TotalPremium = row[13].ToString().Trim() != "" ? Convert.ToDouble(row[13].ToString().Trim()) : 0.0;
                fail_data.ReportDate = DateTime.ParseExact(txtReport_date.Text, "dd/MM/yyyy", null);
                fail_data.PaidOffInMonth = row[14].ToString().Trim();
                fail_data.TerminateDate = row[15].ToString().Trim();
                fail_data.CreatedNoted = fail_data.Remarks;

                fail_list.Add(fail_data);

                Session["cmk_upload_invalid_data"] = fail_list;

                #endregion

            }
            #endregion
        }
        catch (Exception ex)
        {
            valid = false;
            Log.AddExceptionToLog("Error function [validRow(DataRow row)] in class [Upload_Policy_Cmk.aspx.cs], row index [" + row_index + "], detail:" + ex.Message);
        }
        return valid;
    }

    //Clear All
    protected void ImgBtnClear_Click(object sender, ImageClickEventArgs e)
    {
    }

    
    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }

    void SaveDataSuccess(bl_cmk_load_data cmk_data)
    {
        #region SUCCESS LIST

        DataCollection success_data = new DataCollection();

        success_data.Row_Number = cmk_data.Row_Number;
        success_data.CMKCustomerID = cmk_data.CMKCustomerID;
        success_data.LoanID = cmk_data.LoanID;
        success_data.CertificateNo = cmk_data.CertificateNo;
        success_data.LoanAmount = cmk_data.LoanAmount;
        success_data.OutstandingBalance = cmk_data.OutstandingBalance;
        success_data.AssuredAmount = cmk_data.AssuredAmount;
        success_data.Currancy = cmk_data.Currancy;
        success_data.PolicyStatus = cmk_data.PolicyStatus;
        success_data.PaymodeID = cmk_data.PaymodeID;
        success_data.MonthlyPremium = cmk_data.MonthlyPremium;
        success_data.PremiumAfterDiscount = cmk_data.PremiumAfterDiscount;
        success_data.ExtraPremium = cmk_data.ExtraPremium;
        success_data.TotalPremium = cmk_data.TotalPremium;
        success_data.ReportDate = cmk_data.ReportDate;
        success_data.PaidOffInMonth = cmk_data.PaidOffInMonth;
        success_data.TerminateDate = cmk_data.TerminateDate;
        success_data.CreatedNoted = cmk_data.CreatedNoted;

        success_list.Add(success_data);

        Session["SUCCESS_DATA"] = success_list;

        #endregion
    }

    void SaveDataFail(bl_cmk_load_data cmk_data, string remark, int row_count, int action)
    {
        #region EXIST LOAN TO FAIL LIST

        DataCollection fail_data = new DataCollection();

        fail_data.Row_Number = row_count;
        fail_data.CMKCustomerID = cmk_data.CMKCustomerID;
        fail_data.LoanID = cmk_data.LoanID;
        fail_data.CertificateNo = cmk_data.CertificateNo;
        fail_data.LoanAmount = cmk_data.LoanAmount;
        fail_data.OutstandingBalance = cmk_data.OutstandingBalance;
        fail_data.AssuredAmount = cmk_data.AssuredAmount;
        fail_data.PolicyStatus = cmk_data.PolicyStatus;
        fail_data.Currancy = cmk_data.Currancy;
        fail_data.PaymodeID = cmk_data.PaymodeID;
        fail_data.MonthlyPremium = cmk_data.MonthlyPremium;
        fail_data.PremiumAfterDiscount = cmk_data.PremiumAfterDiscount;
        fail_data.ExtraPremium = cmk_data.ExtraPremium;
        fail_data.TotalPremium = cmk_data.TotalPremium;
        fail_data.ReportDate = cmk_data.ReportDate;
        fail_data.PaidOffInMonth = cmk_data.PaidOffInMonth;
        fail_data.TerminateDate = cmk_data.TerminateDate;
        fail_data.CreatedNoted = remark;

        fail_list.Add(fail_data);

        if (action == 0)
        {
            Session["cmk_upload_invalid_data"] = fail_list;
        }
        else
        {
            Session["FAIL_DATA"] = fail_list;
        }


        #endregion
    }

    void SaveSuccessLog(string message)
    {
        Log.CreateLog("Cmk_Log", message);
    }

    void ExportExcel()
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            string filename = "";
            DataTable table_fail = (DataTable)ViewState["FAIL_DATA"];
            DataTable table_invalid = (DataTable)ViewState["CMK_UPLOAD_INVALID_DATA"];

            if (table_fail.Rows.Count > 0)
            {
                filename = "CMK_FAIL_UPLOAD_Records_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                row_count = table_fail.Rows.Count;
            }
            else if (table_invalid.Rows.Count > 0)
            {
                filename = "CMK_INVALID_UPLOAD_Records_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                row_count = table_invalid.Rows.Count;
            }

            if (row_count > 0)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheets = (HSSFSheet)hssfworkbook.CreateSheet("Fail Upload");

                Helper.excel.Sheet = sheets;
                //design row header
                Helper.excel.HeaderText = new string[] {
                                                            "No", "Customer ID", "Loan ID", "Certificate No", "Amount", "Outstanding Balance", "Policy Status",
                                                            "Assured Amount", "Currancy", "Payment Mode", "Monthly Premium", "Premium After Discount", "Extra Premium", "Total Premium", 
                                                            "Report Date", "Paid Off In Month", "Terminate Date", "Remarks"
                                                        };
                Helper.excel.generateHeader();

                // Set Font Bold and Color
                HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();

                IFont font = hssfworkbook.CreateFont();

                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DARK_GREEN.index;

                for (int i = 0; i <= 17; i++)
                {
                    ICell cell = hssfworkbook.GetSheetAt(0).GetRow(0).GetCell(i);
                    cell.CellStyle = style;
                    font.Boldweight = (short)FontBoldWeight.BOLD;

                    cell.CellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.AQUA.index;
                    style.SetFont(font);
                }

                foreach (DataRow row in table_fail.Rows.Count > 0 ? table_fail.Rows : table_invalid.Rows.Count > 0 ? table_invalid.Rows : table_fail.Rows)
                {
                    #region //Variable
                    //Label Customer_ID = (Label)row.FindControl("lblCustomerIDFailed");
                    //Label Loan_ID = (Label)row.FindControl("lblLoanIDFailed");
                    //Label Certificate_No = (Label)row.FindControl("lblCertificateNoFailed");
                    //Label Amount = (Label)row.FindControl("lblAmountFailed");
                    //Label Outstanding_Balance = (Label)row.FindControl("lblOutstandingBalanceFailed");
                    //Label Policy_Status = (Label)row.FindControl("lblPolicyStatusFailed");
                    //Label Assured_Amount = (Label)row.FindControl("lblAssuredAmountFailed");
                    //Label Currancy = (Label)row.FindControl("lblCurrancyFailed");
                    //Label Payment_Mode = (Label)row.FindControl("lblPaymentModeFailed");
                    //Label Monthly_Premium = (Label)row.FindControl("lblMonthlyPremiumFailed");
                    //Label Premium_After_Discount = (Label)row.FindControl("lblPremiumAfterDiscountFailed");
                    //Label Extra_Premium = (Label)row.FindControl("lblExtraPremiumFailed");
                    //Label Total_Premium = (Label)row.FindControl("lblTotalPremiumFailed");
                    //Label Report_Date = (Label)row.FindControl("lblReportDateFailed");
                    //Label Paid_Off_In_Month = (Label)row.FindControl("lblPaidOffDateFailed");
                    //Label Terminate_Date = (Label)row.FindControl("lblTerminateDateFailed");
                    //Label remarks = (Label)row.FindControl("lblRemarkFailed");
                    #endregion

                    #region NEW PROPS 
                    string Customer_ID = row[2].ToString().Trim();
                    string Certificate_No = row[3].ToString().Trim();
                    string Loan_ID = row[4].ToString().Trim();
                    string Currancy = row[15].ToString().Trim();
                    string Policy_Status = row[16].ToString().Trim();
                    string remarks = row[25].ToString().Trim();
                    double Amount = Convert.ToDouble(row[27].ToString().Trim());
                    double Outstanding_Balance = row[29].ToString().Trim() != "" ? Convert.ToDouble(row[29].ToString().Trim()) : 0.0;
                    double Assured_Amount = row[31].ToString().Trim() != "" ? Convert.ToDouble(row[31].ToString().Trim()) : 0.0;
                    double Monthly_Premium = row[33].ToString().Trim() != "" ? Convert.ToDouble(row[33].ToString().Trim()) : 0.0;
                    double Extra_Premium = row[34].ToString().Trim() != "" ? Convert.ToDouble(row[34].ToString().Trim()) : 0.0;
                    double Premium_After_Discount = row[36].ToString().Trim() != "" ? Convert.ToDouble(row[36].ToString().Trim()) : 0.0;
                    double Total_Premium = row[37].ToString().Trim() != "" ? Convert.ToDouble(row[37].ToString().Trim()) : 0.0;
                    string Report_Date = Convert.ToDateTime(row[38]).ToString("dd-MM-yyyy").Trim();
                    string Payment_Mode = row[39].ToString().Trim();
                    string Paid_Off_In_Month = row[40].ToString().Trim() != "" ? Convert.ToDateTime(row[40]).ToString("MM-yyyy").Trim() : "-";
                    string Terminate_Date = row[41].ToString().Trim() != "" ? Convert.ToDateTime(row[41]).ToString("dd-MM-yyyy").Trim() : "-";
                    #endregion 

                    #region set value
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheets.CreateRow(row_no);
                    rowCell.RowStyle = style;

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(Customer_ID);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(Loan_ID);

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(Certificate_No);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(Amount);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(Outstanding_Balance);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(Policy_Status);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(Assured_Amount);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(Currancy);

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Payment_Mode);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Monthly_Premium);

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(Premium_After_Discount);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(Extra_Premium);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(Total_Premium);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(Report_Date);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(Paid_Off_In_Month);

                    HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                    Cell17.SetCellValue(Terminate_Date);

                    HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                    Cell18.SetCellValue(remarks);

                    #endregion

                }

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
                Div_Export.Style["display"] = "none";
            }
        }
        catch (Exception ex)
        {
            AlertMessage("Export data fail, please contact your system administrator.");
            Log.AddExceptionToLog("Error function [ExportExcel()] in page [Upload_Policy_Cmk.aspx.cs], detail:" + ex.Message);
        }
    }

    protected void export_excel_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }

    protected void export_excel_invalid_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }

    //Row Index Changing
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_valid.PageIndex = e.NewPageIndex;
        DIV_UPLOAD_TAB.Style["display"] = "block";

        DataTable tbl_valid;
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(Session["cmk_upload_valid_data"]);
        tbl_valid = JsonConvert.DeserializeObject<DataTable>(json);

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl_valid);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;
        }
        gv_valid.DataSource = dview;
        gv_valid.DataBind();

        ///*show record count*/
        //lblCountValid.Text = "Display: " + gv_valid.Rows.Count + " Of " + tbl_valid.Rows.Count;

    }

    //Row Index Changing
    protected void gv_invalid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_invalid.PageIndex = e.NewPageIndex;
        DIV_UPLOAD_TAB.Style["display"] = "block";

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl_invalid);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_invalid.DataSource = dview;
        gv_invalid.DataBind();

        ///*show record count*/
        //lblCountInvalid.Text = "Display: " + gv_invalid.Rows.Count + " Of " + tbl_invalid.Rows.Count;

    }

    //Row Index Changing
    protected void gv_success_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_success.PageIndex = e.NewPageIndex;
        DIV_UPLOAD_TAB.Style["display"] = "none";
        DIV_RESULT.Style["display"] = "block";

        DataTable tbl_success;
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(Session["SUCCESS_DATA"]);
        tbl_success = JsonConvert.DeserializeObject<DataTable>(json);

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl_success);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;
        }
        gv_success.DataSource = dview;
        gv_success.DataBind();

        ///*show record count*/
        lblCountSuccess.Text = "Display: " + gv_success.Rows.Count + " Of " + tbl_success.Rows.Count;

    }

    //Row Index Changing
    protected void gv_fail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_fail.PageIndex = e.NewPageIndex;
        DIV_UPLOAD_TAB.Style["display"] = "none";
        DIV_RESULT.Style["display"] = "block";

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";

        tbl_fail = (DataTable)ViewState["FAIL_DATA"];

        DataView dview = new DataView(tbl_fail);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_fail.DataSource = dview;
        gv_fail.DataBind();

    }

    class DataCollection : bl_cmk_load_data
    {
        public string Remarks { get; set; }
    }

    private bool Save(bl_cmk_load_data cmk_data, string cmk_policy_id)
    {
        bool status;
        bl_cmk.CMK_Policy_Prem policy_premium = new bl_cmk.CMK_Policy_Prem();
        bl_cmk.CMK_Renewal_Premium renewal_prem = new bl_cmk.CMK_Renewal_Premium();
        bl_cmk.CMK_Group_Premium group_prem = new bl_cmk.CMK_Group_Premium();

        try
        {
            #region // SAVE CMK POLICY PREMIUM

            string CMK_POLICY_PREM_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CMK_POLICY_PREMIUM" }, { "FIELD", "CMK_POLICY_PREMIUM_ID" } });

            policy_premium.CMKPolicyPremiumID = CMK_POLICY_PREM_ID;
            policy_premium.CMKPolicyID = cmk_policy_id;
            policy_premium.LoanAmount = cmk_data.LoanAmount;
            policy_premium.LoanAmountRiel = cmk_data.LoanAmountRiel;
            policy_premium.OutstandingBalance = cmk_data.OutstandingBalance;
            policy_premium.OutstandingBalanceRiel = cmk_data.OutstandingBalanceRiel;
            policy_premium.AssuredAmount = cmk_data.AssuredAmount;
            policy_premium.AssuredAmountRiel = cmk_data.AssuredAmountRiel;
            policy_premium.MonthlyPremium = cmk_data.MonthlyPremium;
            policy_premium.ExtraPremium = cmk_data.ExtraPremium;
            policy_premium.DiscountAmount = 0;
            policy_premium.PremiumAfterDiscount = cmk_data.PremiumAfterDiscount;
            policy_premium.TotalPremium = cmk_data.TotalPremium;
            policy_premium.ReportDate = cmk_data.ReportDate;
            policy_premium.PaymodeID = cmk_data.PaymodeID;

            policy_premium.PaidOffInMonth = cmk_data.PaidOffInMonth != "" ? Convert.ToDateTime(cmk_data.PaidOffInMonth) : new DateTime(1900, 1, 01);
            policy_premium.TerminateDate = cmk_data.TerminateDate != "" ? Convert.ToDateTime(cmk_data.TerminateDate) : new DateTime(1900, 1, 01);
            policy_premium.PaymentBatchNo = cmk_data.PaymentBatchNo;
            policy_premium.CreatedBy = Membership.GetUser().UserName;
            policy_premium.CreatedOn = DateTime.Now;
            policy_premium.CreatedNoted = cmk_data.CreatedNoted;
            policy_premium.Status = cmk_data.PolicyStatus;

            if (da_cmk.Policy.SavePolicyPremium(policy_premium))
                status = true;
            else
                status = false;

            #endregion

            if (status != false)
            {
                #region // SAVE CMK RENEWAL PREM - TABLE CMK_RENEWAL_PREMIUM(PROCESS PAYMENT) ON 26-FEBRUARY-2021

                if (policy_premium.Status != "" && policy_premium.Status == "IF")
                {
                    DataTable dt = new DataTable();

                    renewal_prem.CMK_Policy_Premium_ID = CMK_POLICY_PREM_ID;
                    renewal_prem.CMK_Policy_ID = cmk_policy_id;
                    renewal_prem.Policy_Number = cmk_data.CertificateNo;
                    renewal_prem.Customer_ID = da_cmk.Policy.GetCustomerIDByCertificateNumber(cmk_data.CMKCustomerID, cmk_data.CertificateNo);

                    dt = da_cmk.Policy.GetPolicyByPolicyID(renewal_prem.CMK_Policy_ID);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            product_id = row[0].ToString();
                            renewal_prem.Product_ID = product_id;
                            CMK_Effective_Date = Convert.ToDateTime(row[1].ToString());
                            renewal_prem.CMK_Effective_Date = CMK_Effective_Date;
                            renewal_prem.Effective_Date = Convert.ToDateTime(row[2].ToString());
                            mode = row[3].ToString(); // GET MODE BY POLICY_ID
                            renewal_prem.Mode = mode; // GET MODE BY POLICY_ID
                            product_name = row[4].ToString();
                            renewal_prem.Product_Name = product_name;
                        }

                    }

                    renewal_prem.Sum_Insure = cmk_data.AssuredAmount;
                    renewal_prem.Premium = cmk_data.MonthlyPremium;
                    renewal_prem.Extra_Premium = cmk_data.ExtraPremium;
                    renewal_prem.Discount_Amount = 0;
                    renewal_prem.Total_Premium = cmk_data.TotalPremium;
                    renewal_prem.Invoice_No = ""; // GENERATE INVOICE
                    renewal_prem.Pay_Year = 0; 
                    renewal_prem.Pay_Lot = 0; 
                    renewal_prem.Report_Date = cmk_data.ReportDate;
                    renewal_prem.Created_On = DateTime.Now;
                    renewal_prem.Created_By = Membership.GetUser().UserName;
                    renewal_prem.Created_Note = cmk_data.CreatedNoted;

                    if (da_cmk.Policy.SaveRenewalPremium(renewal_prem))
                    {
                        status = true;
                    }
                    else
                    {
                        da_cmk.Policy.RollBack(cmk_policy_id, cmk_data.CMKCustomerID, cmk_data.ReportDate);
                        status = false; // ROLLBACK PREMIUM
                    }
                }
                
                #endregion
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Save] in page [Upload_Policy_Cmk.aspx.cs], detail: " + ex.Message);
            return false;
        }


        return status;

    }

    protected void btnDelReport_Click(object sender, EventArgs e)
    {
        string report_date = ""; bool result = false; 

        try
        {
            report_date = txtReport_date.Text.Trim();

            string confirmValue = Request.Form["confirm_value"];

            if (confirmValue == "Yes")
            {
                result = da_cmk.Policy.DeletePremiumReport(Helper.FormatDateTime(report_date));

                if (result)
                {
                    AlertMessage("Deleted premium report successfully!!");
                    return;
                }
                else
                {
                    AlertMessage("Delete premium report failed!!");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
        }

    }

    void INVALID_DATA(bl_cmk_load_data cmk_load_data, DataRow row, int row_fail)
    {
        //Customer Props
        cmk_load_data.Row_Number = row_fail;

        //Policy Props
        cmk_load_data.CMKCustomerID = row[1].ToString().Trim();
        cmk_load_data.LoanID = row[2].ToString();
        cmk_load_data.CertificateNo = row[3].ToString();
        cmk_load_data.PolicyStatus = row[6].ToString();
        cmk_load_data.Currancy = row[8].ToString();

        //Premium Props
        if (Helper.GetCurrancy(cmk_load_data.Currancy) != 0) // -> KHR
        {
            cmk_load_data.LoanAmount = row[4].ToString().Trim() != "" ? (Convert.ToDouble(row[4].ToString().Trim())) : 0.0;
            cmk_load_data.OutstandingBalance = row[5].ToString().Trim() != "" ? (Convert.ToDouble(row[5].ToString().Trim()) ) : 0.0;
            cmk_load_data.AssuredAmount = row[7].ToString().Trim() != "" ? (Convert.ToDouble(row[7].ToString().Trim())) : 0.0;

            cmk_load_data.LoanAmountRiel = row[4].ToString().Trim() != "" ? Convert.ToDouble(row[4].ToString().Trim()) : 0.0;
            cmk_load_data.OutstandingBalanceRiel = row[5].ToString().Trim() != "" ? Convert.ToDouble(row[5].ToString().Trim()) : 0.0;
            cmk_load_data.AssuredAmountRiel = row[7].ToString().Trim() != "" ? Convert.ToDouble(row[7].ToString().Trim()) : 0.0;

        }
        else // -> USD
        {
            cmk_load_data.LoanAmount = row[4].ToString().Trim() != "" ? Convert.ToDouble(row[4].ToString().Trim()) : 0.0;
            cmk_load_data.OutstandingBalance = row[5].ToString().Trim() != "" ? Convert.ToDouble(row[5].ToString().Trim()) : 0.0;
            cmk_load_data.AssuredAmount = row[7].ToString().Trim() != "" ? Convert.ToDouble(row[7].ToString().Trim()) : 0.0;

            cmk_load_data.LoanAmountRiel = 0.0;
            cmk_load_data.OutstandingBalanceRiel = 0.0;
            cmk_load_data.AssuredAmountRiel = 0.0;
        }

        cmk_load_data.PaymodeID = Helper.GetPayModeID(row[9].ToString().Trim() != "" ? row[9].ToString().Trim() : "monthly");

        cmk_load_data.MonthlyPremium = row[10].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[10].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
        cmk_load_data.PremiumAfterDiscount = row[11].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[11].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
        cmk_load_data.ExtraPremium = row[12].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[12].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
        cmk_load_data.TotalPremium = row[13].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[13].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;

        cmk_load_data.ReportDate = DateTime.ParseExact(txtReport_date.Text, "dd/MM/yyyy", null);
        cmk_load_data.PaymentBatchNo = "";
        cmk_load_data.PaidOffInMonth = row[14].ToString();
        cmk_load_data.TerminateDate = row[15].ToString();
        cmk_load_data.CreatedNoted = row[16].ToString();

        SaveDataFail(cmk_load_data, "allow valid data, check it again!!", row_index_fail, 0);

        row_index_fail++;
    
    }
}