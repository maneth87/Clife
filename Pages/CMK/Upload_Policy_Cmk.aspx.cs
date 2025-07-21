using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Security.Cryptography;
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
using Newtonsoft.Json;

public partial class Pages_CMK_Upload_Policy_Cmk : System.Web.UI.Page
{
    List<bl_cmk_load_data> list_data = new List<bl_cmk_load_data>();
    List<da_report_approver.bl_report_approver> ApproverList;

    string err_message = "";
    DateTime rollback_report_date;
    string sortEx = "";
    string sortCol = "";

    DataTable tbl_fail;
    DataTable tbl_invalid;

    int row_index_fail = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        ApproverList = da_report_approver.GetApproverList();

        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

            BindddlFullName();
        }
        DIV_UPLOAD_TAB.Style["display"] = "none";
        DIV_RESULT.Style["display"] = "none";    
    }

    List<DataCollection> success_list = new List<DataCollection>();

    List<DataCollection> fail_list = new List<DataCollection>();

    //Upload Flexi Term Policies using Excel file
    protected void ImgBtnSave_Click(object sender, ImageClickEventArgs e)
    {
        bl_cmk.CMK_Policy policy;
        string exist_customer_id = "";
        string exist_loan_id = "";
        string new_customer_id = "";
        string rollback_customer_id = "";
        string rollback_policy_id = "";
        int row_count = 1;

        bool isSaveAble = false;
        List<bl_cmk_load_data> listData = new List<bl_cmk_load_data>();
        try
        {
            listData = (List<bl_cmk_load_data>)Session["cmk_upload_valid_data"];

            if (listData.Count > 0)
            {
                foreach (bl_cmk_load_data cmk_data in listData)
                {
                    #region //Check EXIST LOAN

                    List<bl_cmk.CMK_Policy> Policy = new List<bl_cmk.CMK_Policy>();
                    Policy = da_cmk.Policy.GetPolicyByLoanID(cmk_data.LoanID);

                    DateTime expiry_date;
                    string cmk_policy_id = "";

                    if (Policy.Count > 0)
                    {
                        for (int i = 0; i < Policy.Count; i++)
                        {
                            if (Policy[i].CMKCustomerID == cmk_data.CMKCustomerID && Policy[i].CertificateNo == cmk_data.CertificateNo && Policy[i].LoanID == cmk_data.LoanID)
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

                    if (exist_loan_id == "")
                    {
                        #region // NEW LOAN

                        #region // CHECK EXIST CUSTOMER & GET NEW CUSTOMER

                        exist_customer_id = da_customer.GetCustomerIDByParameters(cmk_data.FirstName.Trim(), cmk_data.LastName.Trim(), Helper.GetGender(cmk_data.Gender), Convert.ToDateTime(cmk_data.DateOfBirth));

                        if (exist_customer_id == "")
                        {
                            new_customer_id = da_customer.GetCustomerID(); //generate NEW CUSTOMER ID

                            #region // SAVE CUSTOMER

                            bl_customer customer = new bl_customer();
                            customer.Customer_ID = new_customer_id;
                            customer.ID_Type = 0;
                            customer.ID_Card = "";
                            customer.First_Name = cmk_data.FirstName;
                            customer.Last_Name = cmk_data.LastName;
                            customer.Khmer_First_Name = "";
                            customer.Khmer_Last_Name = "";
                            customer.Prior_First_Name = "";
                            customer.Prior_Last_Name = "";
                            customer.Gender = Helper.GetGender(cmk_data.Gender);
                            customer.Birth_Date = Convert.ToDateTime(cmk_data.DateOfBirth);
                            customer.Country_ID = "KH";
                            customer.Father_First_Name = "";
                            customer.Father_Last_Name = "";
                            customer.Mother_First_Name = "";
                            customer.Mother_Last_Name = "";
                            customer.Created_By = Membership.GetUser().UserName;
                            customer.Created_On = DateTime.Now;
                            rollback_customer_id = new_customer_id; //use for rollback data

                            if (!da_customer.InsertCustomer(customer, Membership.GetUser().UserName))
                            {
                                err_message = "[" + Membership.GetUser().UserName + "] Save Customer fail Customer ID[" + customer.Customer_ID + "]";
                                goto EXIT;
                            }

                            #endregion
                        }
                        else
                        {
                            new_customer_id = exist_customer_id;
                            rollback_customer_id = "";
                        }

                        isSaveAble = true;

                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region FAIL LIST
                        SaveDataFail(cmk_data, "Policy is existed!!", row_count, 1);
                        #endregion

                        isSaveAble = false;

                    }

                    #region // SAVE NEW POLICY

                    if (isSaveAble)
                    {
                        #region // SAVE CMK POLICY

                        policy = new bl_cmk.CMK_Policy();
                        policy.CMKPolicyID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CMK_POLICY" }, { "FIELD", "CMK_POLICY_ID" } });
                        policy.CMKCustomerID = cmk_data.CMKCustomerID;
                        policy.CustomerID = new_customer_id;
                        policy.ProductID = hdfProductID.Value.Trim(); // Camlife Product (credit life plan c)
                        policy.CertificateNo = cmk_data.CertificateNo;
                        policy.LoanID = cmk_data.LoanID;
                        policy.LoanType = cmk_data.LoanType; // CMK Product ID
                        policy.Group = cmk_data.Group;
                        policy.CoveredYear = cmk_data.CoveredYear;
                        policy.OpenedDate = Convert.ToDateTime(cmk_data.OpenedDate);
                        policy.EffectiveDate = Convert.ToDateTime(cmk_data.EffectiveDate);
                        policy.DateOfEntry = Convert.ToDateTime(cmk_data.DateOfEntry);
                        policy.PolicyStatus = cmk_data.PolicyStatus;
                        expiry_date = (policy.EffectiveDate.AddYears(policy.CoveredYear)).AddDays(-1);
                        policy.ExpireDate = expiry_date;
                        policy.Currancy = cmk_data.Currancy;
                        policy.Age = cmk_data.Age;
                        policy.LoanDuration = cmk_data.LoanDuration;
                        policy.Branch = cmk_data.Branch;
                        policy.ChannelLocationID = hdfChannelLocationID.Value;
                        policy.ChannelChannelItemID = da_channel.GetChannelChannelItemIDByChannelSubIDAndChannelItemID(Convert.ToInt32(hdfChannelSubID.Value), hdfChannelItemID.Value);
                        policy.CreatedBy = Membership.GetUser().UserName;
                        policy.CreatedOn = DateTime.Now;
                        policy.CreatedNoted = cmk_data.CreatedNoted;
                        rollback_policy_id = policy.CMKPolicyID; //use for rollback data

                        if (!da_cmk.Policy.SavePolicy(policy))
                        {
                            err_message = "[" + Membership.GetUser().UserName + "] Save policy fail Certificate No[" + cmk_data.CertificateNo + "]";
                            goto EXIT;
                        }

                        #endregion

                        #region // SAVE CMK POLICY PREMIUM

                        bl_cmk.CMK_Policy_Prem policy_premium = new bl_cmk.CMK_Policy_Prem();

                        policy_premium.CMKPolicyPremiumID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CMK_POLICY_PREMIUM" }, { "FIELD", "CMK_POLICY_PREMIUM_ID" } });
                        policy_premium.CMKPolicyID = policy.CMKPolicyID;
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
                        policy_premium.Status = policy.PolicyStatus;

                        if (!da_cmk.Policy.SavePolicyPremium(policy_premium)){
                            err_message = "[" + Membership.GetUser().UserName + "] Save policy premium failed Certificate No[" + cmk_data.CertificateNo + "]";
                            rollback_report_date = new DateTime(1900, 1, 01);
                            goto EXIT;
                        }

                        #endregion

                        #region // SAVE POLICY ID

                        if (!da_policy.InsertPolicyID(policy.CMKPolicyID, 1))
                        {
                            err_message = "[" + Membership.GetUser().UserName + "] Save policy ID fail Certificate No[" + cmk_data.CertificateNo + "]";
                            goto EXIT;
                        }

                        #endregion

                        #region // SAVE POLICY STATUS

                        if (!da_policy.InsertPolicyStatus(policy.CMKPolicyID, policy.PolicyStatus, Membership.GetUser().UserName, DateTime.Now)) 
                        {
                            err_message = "[" + Membership.GetUser().UserName + "] Save policy status fail Certificate No[" + cmk_data.CertificateNo + "]";
                            goto EXIT;
                        }
                       
                        #endregion

                        #region // SAVE POLICY PAY MODE
                        if (!da_policy.InsertPolicyPayMode(policy.CMKPolicyID, Convert.ToInt32(cmk_data.PaymodeID), DateTime.Now, Membership.GetUser().UserName, DateTime.Now))
                        {
                            err_message = "[" + Membership.GetUser().UserName + "] Save policy pay mode fail Certificate No[" + cmk_data.CertificateNo + "]";
                            goto EXIT;
                        }

                        #endregion

                        #region SAVE APPROVER

                        foreach (da_report_approver.bl_report_approver approve in ApproverList)
                        {
                            if (approve.NameEn.Trim() == ddlFullName.SelectedItem.Text.Trim() && (approve.ID).ToString().Trim() == ddlFullName.SelectedValue.Trim())
                            {
                                da_report_approver.bl_report_approver_policy approver_policy = new da_report_approver.bl_report_approver_policy();
                                approver_policy.Approver_ID = approve.ID;
                                approver_policy.Policy_ID = policy.CMKPolicyID;
                                approver_policy.Created_On = DateTime.Now;
                                approver_policy.Created_By = Membership.GetUser().UserName;
                                da_report_approver.InsertApproverPolicy(approver_policy);
                                break;
                            }

                        }

                        #endregion

                        #region SUCCESS LIST

                        SaveDataSuccess(cmk_data, row_count);

                        #endregion

                        
                    }
                    #endregion

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
                rollback_report_date = new DateTime(1900, 1, 01); 
                #endregion

                }

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
                        AlertMessage("System Saved All successfully.");
                    }
                    else 
                    {
                        AlertMessage("System Save Failed!!, please check in [Fail tab].");
                    }
                }

                #endregion
            }
            else
            {
                AlertMessage("There's no any data to save!");
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

            if (txtReport_date.Text.Trim() != "" && ddlChannel.Text.Trim() != "" && hdfProductID.Value.Trim() != "" && ddlChannel.Text.Trim() != "0" && hdfProductID.Value.Trim() != "0" && ddlFullName.SelectedValue.Trim() != ".")
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
                        //Short Loan_ID and Customer_ID

                        foreach (DataRow row in my_data.Rows)
                        {
                            bl_cmk_load_data cmk_load_data = new bl_cmk_load_data();

                            if (row[2].ToString().Trim() != "" && row[3].ToString().Trim() != "" && row[4].ToString().Trim() != "" && row[6].ToString().Trim() != "" 
                                && row[7].ToString().Trim() != "" && row[8].ToString().Trim() != "" && row[9].ToString().Trim() != "" && row[10].ToString().Trim() != ""
                                && row[11].ToString().Trim() != "" && row[12].ToString().Trim() != "" && row[13].ToString().Trim() != "" && row[14].ToString().Trim() != ""
                                && row[15].ToString().Trim() != "" && row[16].ToString().Trim() != "" && row[17].ToString().Trim() != "" && row[18].ToString().Trim() != "" 
                                && row[19].ToString().Trim() != "" && row[20].ToString().Trim() != "" && row[21].ToString().Trim() != "" && row[22].ToString().Trim() != "" 
                                && row[23].ToString().Trim() != "" && row[24].ToString().Trim() != "" && row[26].ToString().Trim() != "")
                            {
                                if (validRow(row, my_data.Rows.IndexOf(row)))
                                {
                                    #region VALID DATA
                                    //Customer Props
                                    cmk_load_data.Row_Number = row_index;
                                    cmk_load_data.LastName = row[5].ToString().Trim();
                                    cmk_load_data.FirstName = row[6].ToString().Trim();
                                    cmk_load_data.Gender = row[9].ToString().Trim();
                                    cmk_load_data.DateOfBirth = row[7].ToString().Trim();
                                    
                                    //Policy Props
                                    cmk_load_data.CMKCustomerID = row[2].ToString().Trim();
                                    cmk_load_data.LoanID = row[3].ToString().Trim();
                                    cmk_load_data.CertificateNo = row[4].ToString().Trim();
                                    cmk_load_data.LoanType = row[10].ToString().Trim();
                                    cmk_load_data.Group = row[21].ToString().Trim();
                                    cmk_load_data.OpenedDate = row[11].ToString().Trim();
                                    cmk_load_data.EffectiveDate = row[17].ToString().Trim();
                                    cmk_load_data.DateOfEntry = row[15].ToString().Trim();
                                    cmk_load_data.PolicyStatus = row[18].ToString().Trim();
                                    cmk_load_data.Currancy = row[20].ToString().Trim();
                                    cmk_load_data.Age = row[8].ToString().Trim() != "" ? Convert.ToInt32(row[8].ToString().Trim()) : 0;
                                    cmk_load_data.LoanDuration = row[12].ToString().Trim() != "" ? Convert.ToInt32(row[12].ToString().Trim()) : 0;
                                    cmk_load_data.CoveredYear = row[16].ToString().Trim() != "" ? Convert.ToInt32(row[16].ToString().Trim()) : 0;
                                    cmk_load_data.Branch = row[1].ToString().Trim();
                                    cmk_load_data.CreatedNoted = row[29].ToString().Trim() != "" ? row[29].ToString().Trim() : "";  

                                    //Premium Props
                                    if (Helper.GetCurrancy(cmk_load_data.Currancy) != 0) // -> KHR
                                    {
                                        if (Math.Abs(Convert.ToDouble(row[4].ToString().Trim())) >= (500 * 4000)) //Minimum loan cover is >= 2000000 USD
                                        {
                                            cmk_load_data.LoanAmount = row[13].ToString().Trim() != "" && Convert.ToDouble(row[13].ToString().Trim()) > 4000 ? (Convert.ToDouble(row[13].ToString().Trim()) / 4000) : 0.0;
                                            cmk_load_data.OutstandingBalance = row[14].ToString().Trim() != "" && Math.Abs(Convert.ToDouble(row[14].ToString().Trim())) > 4000 ? (Convert.ToDouble(row[14].ToString().Trim()) / 4000) : 0.0;
                                            cmk_load_data.AssuredAmount = row[19].ToString().Trim() != "" && Convert.ToDouble(row[19].ToString().Trim()) > 4000 ? (Convert.ToDouble(row[19].ToString().Trim()) / 4000) : 0.0;

                                            cmk_load_data.LoanAmountRiel = row[13].ToString().Trim() != "" ? Convert.ToDouble(row[13].ToString().Trim()) : 0.0;
                                            cmk_load_data.OutstandingBalanceRiel = row[14].ToString().Trim() != "" ? Convert.ToDouble(row[14].ToString().Trim()) : 0.0;
                                            cmk_load_data.AssuredAmountRiel = row[19].ToString().Trim() != "" ? Convert.ToDouble(row[19].ToString().Trim()) : 0.0;

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
                                            cmk_load_data.LoanAmount = row[13].ToString().Trim() != "" ? Convert.ToDouble(row[13].ToString().Trim()) : 0.0;
                                            cmk_load_data.OutstandingBalance = row[14].ToString().Trim() != "" ? Convert.ToDouble(row[14].ToString().Trim()) : 0.0;
                                            cmk_load_data.AssuredAmount = row[19].ToString().Trim() != "" ? Convert.ToDouble(row[19].ToString().Trim()) : 0.0;

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

                                    cmk_load_data.PaymodeID = Helper.GetPayModeID(row[22].ToString().Trim() != "" ? row[22].ToString().Trim() : "monthly");
                                    cmk_load_data.MonthlyPremium = row[23].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[23].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
                                    cmk_load_data.PremiumAfterDiscount = row[24].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[24].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
                                    cmk_load_data.ExtraPremium = row[25].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[25].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
                                    cmk_load_data.TotalPremium = row[26].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[26].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
                                    cmk_load_data.ReportDate = DateTime.ParseExact(txtReport_date.Text, "dd/MM/yyyy", null);
                                    cmk_load_data.PaymentBatchNo = "";
                                    cmk_load_data.PaidOffInMonth = row[27].ToString().Trim();
                                    cmk_load_data.TerminateDate = row[28].ToString().Trim();

                                    list_data.Add(cmk_load_data);

                                    #endregion

                                    row_index++;

                                }

                            }
                            else
                            {
                                #region INVALID DATA

                                INVALID_DATA(cmk_load_data, row, row_index_fail);

                                #endregion
                                
                            }


                        }
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
            export_excel.Enabled = false;
        }

        #endregion
        ViewState["CMK_UPLOAD_INVALID_DATA"] = new DataTable(); 
        //Session["SUCCESS_DATA"] = null;
        //Session["FAIL_DATA"] = null;

        ImgBtnSave.Enabled = false;

    }

    private void BindddlFullName()
    {
        ddlFullName.Items.Clear();
        ddlFullName.Items.Add(".");
        foreach (da_report_approver.bl_report_approver approver in ApproverList)
        {
            ddlFullName.Items.Add(new ListItem(approver.NameEn, Convert.ToString(approver.ID)));
        }
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
                    if (tbl.Columns[0].ColumnName.Trim() != "No#" || tbl.Columns[1].ColumnName.Trim() != "Branch"
                        || tbl.Columns[2].ColumnName.Trim() != "Customer ID" || tbl.Columns[3].ColumnName.Trim() != "Loan ID" || tbl.Columns[4].ColumnName.Trim() != "Certificate No"
                        || tbl.Columns[5].ColumnName.Trim() != "Last Name" || tbl.Columns[6].ColumnName.Trim() != "First Name" || tbl.Columns[7].ColumnName.Trim() != "DOB_ (DD-MM-YYYY)"
                        || tbl.Columns[8].ColumnName.Trim() != "Age" || tbl.Columns[9].ColumnName.Trim() != "Gender" || tbl.Columns[10].ColumnName.Trim() != "Product"  || tbl.Columns[11].ColumnName.Trim() != "Opened Date"
                        || tbl.Columns[12].ColumnName.Trim() != "Duration" || tbl.Columns[13].ColumnName.Trim() != "Amount" || tbl.Columns[14].ColumnName.Trim() != "Outstanding Balance"
                        || tbl.Columns[15].ColumnName.Trim() != "Date Of Entry" || tbl.Columns[16].ColumnName.Trim() != "Cover Year" || tbl.Columns[17].ColumnName.Trim() != "Effective Date"
                        || tbl.Columns[18].ColumnName.Trim() != "Policy Status" || tbl.Columns[19].ColumnName.Trim() != "Assured Amount" || tbl.Columns[20].ColumnName.Trim() != "Currancy"
                        || tbl.Columns[21].ColumnName.Trim() != "Group" || tbl.Columns[22].ColumnName.Trim() != "Payment Mode" || tbl.Columns[23].ColumnName.Trim() != "Monthly Premium" 
                        || tbl.Columns[24].ColumnName.Trim() != "Premium _After Discount" || tbl.Columns[25].ColumnName.Trim() != "Extra Premium" || tbl.Columns[26].ColumnName.Trim() != "Total Premium"
                        || tbl.Columns[27].ColumnName.Trim() != "Paid Off In Month" || tbl.Columns[28].ColumnName.Trim() != "Terminate Date" || tbl.Columns[29].ColumnName.Trim() != "Remarks")
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
            if (row[2].ToString().Trim() == "") //CMK Customer ID
            {
                valid = false;
                fail_data.Remarks = "Customer ID is required.";
            }
            else if (row[3].ToString().Trim() == "") //Loan ID
            {
                valid = false;
                fail_data.Remarks = "Loan ID is required.";
            }
            else if (row[4].ToString().Trim() == "") // Certificate No
            {
                valid = false;
                fail_data.Remarks = "Certificate No is required.";
            }
            else if (row[5].ToString().Trim() == "") // Last Name
            {
                valid = false;
                fail_data.Remarks = "Last Name is required.";
            }
            else if (row[6].ToString().Trim() == "") // First Name
            {
                valid = false;
                fail_data.Remarks = "First Name is required.";
            }
            
            else if (row[7].ToString().Trim() == "") // DOB
            {
                valid = false;
                fail_data.Remarks = "DOB is required.";
            }
            else if (row[8].ToString().Trim() == "") // Age
            {
                valid = false;
                fail_data.Remarks = "Age is required.";
            }
            else if (row[9].ToString().Trim() == "") // Gender
            {
                valid = false;
                fail_data.Remarks = "Gender is required.";
            }
            else if (row[13].ToString().Trim() == "") // Loan Amount
            {
                valid = false;
                fail_data.Remarks = "Loan Amount is required.";
            }
            else if (row[14].ToString().Trim() == "") // Outstanding Balance
            {
                valid = false;
                fail_data.Remarks = "Outstanding Balance is required.";
            }
            else if (row[15].ToString().Trim() == "") // Date Of Entry
            {
                valid = false;
                fail_data.Remarks = "Date Of Entry is required.";
            }
            else if (row[16].ToString().Trim() == "") // Cover year
            {
                valid = false;
                fail_data.Remarks = "Cover Year is required.";
            }
            else if (row[17].ToString().Trim() == "") // Effective Date
            {
                valid = false;
                fail_data.Remarks = "Effective Date is required.";
            }
            else if (row[19].ToString().Trim() == "") // Assure amount
            {
                valid = false;
                fail_data.Remarks = "Assure amount is required.";
            }
            else if (row[20].ToString().Trim() == "" ) // Currancy
            {
                valid = false;
                fail_data.Remarks = "Currency is required.";
            }
            else if (Helper.GetCurrancy(row[20].ToString().Trim()) == -1) // Currancy
            {
                valid = false;
                fail_data.Remarks = "Currency is not valid.";
            }
            
            else if (row[23].ToString().Trim() == "") // MonthlyPremium
            {
                valid = false;
                fail_data.Remarks = "Monthly Premium is required.";
            }

            //store invalid data
            if (!valid)
            {
                #region EXIST LOAN TO FAIL LIST

                fail_data.Row_Number = row_index;
                fail_data.Branch = row[1].ToString().Trim();
                fail_data.CMKCustomerID = row[2].ToString().Trim();
                fail_data.LoanID = row[3].ToString();
                fail_data.CertificateNo = row[4].ToString();
                fail_data.LastName = row[5].ToString();
                fail_data.FirstName = row[6].ToString();
                fail_data.Gender = row[9].ToString();
                fail_data.DateOfBirth = row[7].ToString();
                fail_data.LoanType = row[10].ToString();
                fail_data.OpenedDate = row[11].ToString();
                fail_data.DateOfEntry = row[15].ToString();
                fail_data.EffectiveDate = row[17].ToString().Trim();
                fail_data.LoanDuration = row[12].ToString().Trim() != "" ? Convert.ToInt32(row[12].ToString().Trim()) : 0;
                fail_data.CoveredYear = row[16].ToString().Trim() != "" ? Convert.ToInt32(row[16].ToString().Trim()) : 0;
                fail_data.LoanAmount = row[13].ToString().Trim() != "" ? Convert.ToDouble(row[13].ToString().Trim()) : 0.0;
                fail_data.OutstandingBalance = row[14].ToString().Trim() != "" ? Convert.ToDouble(row[14].ToString().Trim()) : 0.0;
                fail_data.AssuredAmount = row[19].ToString().Trim() != "" ? Convert.ToDouble(row[19].ToString().Trim()) : 0.0;
                fail_data.PolicyStatus = row[18].ToString().Trim();
                fail_data.Currancy = row[20].ToString();
                fail_data.Group = row[21].ToString();
                fail_data.Age = row[8].ToString().Trim() != "" ? Convert.ToInt32(row[8].ToString()) : 0;
                fail_data.PaymodeID = Helper.GetPayModeID(row[22].ToString().Trim() != "" ? row[22].ToString().Trim() : "monthly");
                fail_data.MonthlyPremium = row[23].ToString().Trim() != "" ? Convert.ToDouble(row[23].ToString().Trim()) : 0.0;
                fail_data.PremiumAfterDiscount = row[24].ToString().Trim() != "" ? Convert.ToDouble(row[24].ToString().Trim()) : 0.0;
                fail_data.ExtraPremium = row[25].ToString().Trim() != "" ? Convert.ToDouble(row[25].ToString().Trim()) : 0.0;
                fail_data.TotalPremium = row[26].ToString().Trim() != "" ? Convert.ToDouble(row[26].ToString().Trim()) : 0.0;
                fail_data.ReportDate = DateTime.ParseExact(txtReport_date.Text, "dd/MM/yyyy", null);
                fail_data.PaidOffInMonth = row[27].ToString().Trim();
                fail_data.TerminateDate = row[28].ToString().Trim();


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

        /*show record count*/
        lblCountValid.Text = "Display: " + gv_valid.Rows.Count + " Of " + tbl_valid.Rows.Count;

    }

    //Row Index Changing
    protected void gv_invalid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_invalid.PageIndex = e.NewPageIndex;
        DIV_UPLOAD_TAB.Style["display"] = "block";
        tbl_invalid = (DataTable)ViewState["CMK_UPLOAD_INVALID_DATA"];

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl_invalid);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_invalid.DataSource = dview;
        gv_invalid.DataBind();

        /*show record count*/
        lblCountInvalid.Text = "Display: " + gv_invalid.Rows.Count + " Of " + tbl_invalid.Rows.Count;

    }

    //Row Index Changing
    protected void gv_success_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_success.PageIndex = e.NewPageIndex;
        DIV_RESULT.Style["display"] = "block";

        DataTable tbl_success;
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(Session["SUCCESS_DATA"]);
        tbl_success = JsonConvert.DeserializeObject<DataTable>(json);

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        //div_success.Attributes["class"] = "active";
        //div_fail.Attributes["class"] = "";  

        DataView dview = new DataView(tbl_success);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;
        }
        gv_success.DataSource = dview;
        gv_success.DataBind();

        /*show record count*/
        lblCountSuccess.Text = "Display: " + gv_success.Rows.Count + " Of " + tbl_success.Rows.Count;

    }

    //Row Index Changing
    protected void gv_fail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_fail.PageIndex = e.NewPageIndex;
        DIV_RESULT.Style["display"] = "block";
        tbl_fail = (DataTable)ViewState["FAIL_DATA"];

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        success.Attributes["class"] = "active";
        fail.Attributes["class"] = "active";
        

        DataView dview = new DataView(tbl_fail);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_fail.DataSource = dview;
        gv_fail.DataBind();

        /*show record count*/
        lblCountFail.Text = "Display: " + gv_fail.Rows.Count + " Of " + tbl_fail.Rows.Count;

    }

    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }

    void SaveDataSuccess(bl_cmk_load_data cmk_data, int row_count)
    {
        #region SUCCESS LIST

        DataCollection success_data = new DataCollection();

        success_data.Row_Number = row_count;
        success_data.Branch = cmk_data.Branch;
        success_data.CMKCustomerID = cmk_data.CMKCustomerID;
        success_data.LoanID = cmk_data.LoanID;
        success_data.CertificateNo = cmk_data.CertificateNo;
        success_data.LastName = cmk_data.LastName;
        success_data.FirstName = cmk_data.FirstName;
        success_data.Gender = cmk_data.Gender;
        success_data.DateOfBirth = cmk_data.DateOfBirth;
        success_data.LoanType = cmk_data.LoanType;
        success_data.OpenedDate = cmk_data.OpenedDate;
        success_data.DateOfEntry = cmk_data.DateOfEntry;
        success_data.EffectiveDate = cmk_data.EffectiveDate;
        success_data.LoanDuration = cmk_data.LoanDuration;
        success_data.CoveredYear = cmk_data.CoveredYear;
        success_data.LoanAmount = cmk_data.LoanAmount;
        success_data.OutstandingBalance = cmk_data.OutstandingBalance;
        success_data.AssuredAmount = cmk_data.AssuredAmount;
        success_data.Currancy = cmk_data.Currancy;
        success_data.Group = cmk_data.Group;
        success_data.Age = cmk_data.Age;
        success_data.PaymodeID = cmk_data.PaymodeID;
        success_data.MonthlyPremium = cmk_data.MonthlyPremium;
        success_data.PremiumAfterDiscount = cmk_data.PremiumAfterDiscount;
        success_data.ExtraPremium = cmk_data.ExtraPremium;
        success_data.TotalPremium = cmk_data.TotalPremium;
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
        fail_data.Branch = cmk_data.Branch;
        fail_data.CMKCustomerID = cmk_data.CMKCustomerID;
        fail_data.LoanID = cmk_data.LoanID;
        fail_data.CertificateNo = cmk_data.CertificateNo;
        fail_data.LastName = cmk_data.LastName;
        fail_data.FirstName = cmk_data.FirstName;
        fail_data.Gender = cmk_data.Gender;
        fail_data.DateOfBirth = cmk_data.DateOfBirth;
        fail_data.LoanType = cmk_data.LoanType;
        fail_data.OpenedDate = cmk_data.OpenedDate;
        fail_data.DateOfEntry = cmk_data.DateOfEntry;
        fail_data.EffectiveDate = cmk_data.EffectiveDate;
        fail_data.LoanDuration = cmk_data.LoanDuration;
        fail_data.CoveredYear = cmk_data.CoveredYear;
        fail_data.LoanAmount = cmk_data.LoanAmount;
        fail_data.OutstandingBalance = cmk_data.OutstandingBalance;
        fail_data.AssuredAmount = cmk_data.AssuredAmount;
        fail_data.PolicyStatus = cmk_data.PolicyStatus;
        fail_data.Currancy = cmk_data.Currancy;
        fail_data.Group = cmk_data.Group;
        fail_data.Age = cmk_data.Age;
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

    void INVALID_DATA(bl_cmk_load_data cmk_load_data, DataRow row, int row_fail)
    {
        //Customer Props
        cmk_load_data.Row_Number = row_index_fail;
        cmk_load_data.LastName = row[5].ToString().Trim();
        cmk_load_data.FirstName = row[6].ToString().Trim();
        cmk_load_data.Gender = row[9].ToString().Trim();
        cmk_load_data.DateOfBirth = row[7].ToString().Trim() != "" ? row[7].ToString().Trim() : "-";

        //Policy Props
        cmk_load_data.CMKCustomerID = row[2].ToString().Trim();
        cmk_load_data.LoanID = row[3].ToString().Trim();
        cmk_load_data.CertificateNo = row[4].ToString().Trim();
        cmk_load_data.LoanType = row[10].ToString().Trim();
        cmk_load_data.Group = row[21].ToString().Trim();
        cmk_load_data.OpenedDate = row[11].ToString().Trim();
        cmk_load_data.EffectiveDate = row[17].ToString().Trim();
        cmk_load_data.DateOfEntry = row[15].ToString().Trim();
        cmk_load_data.PolicyStatus = row[18].ToString().Trim();
        cmk_load_data.Currancy = row[20].ToString().Trim();
        cmk_load_data.Age = row[8].ToString().Trim() != "" ? Convert.ToInt32(row[8].ToString().Trim()) : 0;
        cmk_load_data.LoanDuration = row[12].ToString().Trim() != "" ? Convert.ToInt32(row[12].ToString().Trim()) : 0;
        cmk_load_data.CoveredYear = row[16].ToString().Trim() != "" ? Convert.ToInt32(row[16].ToString().Trim()) : 0;
        cmk_load_data.Branch = row[1].ToString().Trim();
        cmk_load_data.CreatedNoted = row[29].ToString().Trim() != "" ? row[29].ToString().Trim() : "";

        //Premium Props
        if (Helper.GetCurrancy(cmk_load_data.Currancy) != 0) // -> KHR
        {
            cmk_load_data.LoanAmount = row[13].ToString().Trim() != "" ? Convert.ToDouble(row[13].ToString().Trim()) : 0.0;
            cmk_load_data.OutstandingBalance = row[14].ToString().Trim() != "" ? Convert.ToDouble(row[14].ToString().Trim()) : 0.0;
            cmk_load_data.AssuredAmount = row[19].ToString().Trim() != "" ? Convert.ToDouble(row[19].ToString().Trim()) : 0.0;

            cmk_load_data.LoanAmountRiel = row[13].ToString().Trim() != "" ? Convert.ToDouble(row[13].ToString().Trim()) : 0.0;
            cmk_load_data.OutstandingBalanceRiel = row[14].ToString().Trim() != "" ? Convert.ToDouble(row[14].ToString().Trim()) : 0.0;
            cmk_load_data.AssuredAmountRiel = row[19].ToString().Trim() != "" ? Convert.ToDouble(row[19].ToString().Trim()) : 0.0;
        }
        else // -> USD
        {
            cmk_load_data.LoanAmount = row[13].ToString().Trim() != "" ? Convert.ToDouble(row[13].ToString().Trim()) : 0.0;
            cmk_load_data.OutstandingBalance = row[14].ToString().Trim() != "" ? Convert.ToDouble(row[14].ToString().Trim()) : 0.0;
            cmk_load_data.AssuredAmount = row[19].ToString().Trim() != "" ? Convert.ToDouble(row[19].ToString().Trim()) : 0.0;

            cmk_load_data.LoanAmountRiel = 0.0;
            cmk_load_data.OutstandingBalanceRiel = 0.0;
            cmk_load_data.AssuredAmountRiel = 0.0;
        }

        cmk_load_data.PaymodeID = Helper.GetPayModeID(row[22].ToString().Trim() != "" ? row[22].ToString().Trim() : "monthly");
        cmk_load_data.MonthlyPremium = row[23].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[23].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
        cmk_load_data.PremiumAfterDiscount = row[24].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[24].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
        cmk_load_data.ExtraPremium = row[25].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[25].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
        cmk_load_data.TotalPremium = row[26].ToString().Trim() != "" ? Math.Round(Convert.ToDouble(row[26].ToString().Trim()), 2, MidpointRounding.AwayFromZero) : 0.0;
        cmk_load_data.ReportDate = DateTime.ParseExact(txtReport_date.Text, "dd/MM/yyyy", null);
        cmk_load_data.PaymentBatchNo = "";
        cmk_load_data.PaidOffInMonth = row[27].ToString().Trim();
        cmk_load_data.TerminateDate = row[28].ToString().Trim();

        SaveDataFail(cmk_load_data, "allow valid data, check it again!!", row_index_fail, 0);


        row_index_fail++;

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
                                                            "No", "Branch","Customer ID", "Loan ID", "Certificate No", "Last Name", "First Name", "DOB", "Age", "Gender",  "Product",
                                                            "Opened Date", "Duration", "Amount", "Outstanding Balance", "Date Of Entry", "Cover Year", "Effective Date", "Policy Status",
                                                            "Assured Amount", "Currancy", "Group", "Payment Mode", "Monthly Premium", "Premium After Discount", "Extra Premium", "Total Premium", 
                                                            "Paid Off In Month", "Terminate Date", "Remarks"
                                                        };
                Helper.excel.generateHeader();

                // Set Font Bold and Color
                HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();

                IFont font = hssfworkbook.CreateFont();

                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DARK_GREEN.index;

                for (int i = 0; i <= 29; i++)
                {
                    ICell cell = hssfworkbook.GetSheetAt(0).GetRow(0).GetCell(i);
                    cell.CellStyle = style;
                    font.Boldweight = (short)FontBoldWeight.BOLD;

                    cell.CellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.AQUA.index;
                    style.SetFont(font);
                }

                foreach (DataRow row in table_fail.Rows.Count > 0 ? table_fail.Rows : table_invalid.Rows.Count > 0 ? table_invalid.Rows : table_fail.Rows)
                {
                   
                    #region VARIABLE

                    string Customer_ID = row[2].ToString().Trim();
                    string Certificate_No = row[3].ToString().Trim();
                    string Loan_ID = row[4].ToString().Trim();
                    string Product = row[6].ToString().Trim();
                    string Last_Name = row[7].ToString().Trim();
                    string First_Name = row[8].ToString().Trim();
                    string Gender = row[9].ToString().Trim();
                    string DOB = row[10].ToString().Trim() != "-" ? Convert.ToDateTime(row[10]).ToString("dd-MM-yyyy").Trim() : "invalid";
                    string Group = row[11].ToString().Trim();
                    string Effective_Date = row[12].ToString().Trim() != "" ? Convert.ToDateTime(row[12]).ToString("dd-MM-yyyy").Trim() : "-";
                    string Opened_Date = row[13].ToString().Trim() != "" ? Convert.ToDateTime(row[13]).ToString("dd-MM-yyyy").Trim() : "-";
                    string Date_Of_Entry = row[14].ToString().Trim() != "" ? Convert.ToDateTime(row[14]).ToString("dd-MM-yyyy").Trim() : "-";
                    string Currancy = row[15].ToString().Trim();
                    string Policy_Status = row[16].ToString().Trim();
                    string Age = row[17].ToString().Trim();
                    string Duration = row[18].ToString().Trim();
                    string Cover_Year = row[19].ToString().Trim();
                    string Branch = row[20].ToString().Trim();
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

                    #region //Variable
                    //Label Branch = (Label)row.FindControl("lblBranch");
                    //Label Customer_ID = (Label)row.FindControl("lblCustomerID");
                    //Label Loan_ID = (Label)row.FindControl("lblLoanID");
                    //Label Certificate_No = (Label)row.FindControl("lblCertificateNo");
                    //Label Last_Name = (Label)row.FindControl("lblLastName");
                    //Label First_Name = (Label)row.FindControl("lblFirstName");
                    //Label DOB = (Label)row.FindControl("lblDateOfBirth");
                    //Label Age = (Label)row.FindControl("lblAge");
                    //Label Gender = (Label)row.FindControl("lblGender");
                    //Label Product = (Label)row.FindControl("lblProduct");
                    //Label Opened_Date = (Label)row.FindControl("lblOpenedDate");
                    //Label Duration = (Label)row.FindControl("lblDuration");
                    //Label Amount = (Label)row.FindControl("lblAmount");
                    //Label Outstanding_Balance = (Label)row.FindControl("lblOutstandingBalance");
                    //Label Date_Of_Entry = (Label)row.FindControl("lblDateOfEntry");
                    //Label Cover_Year = (Label)row.FindControl("lbCoverYear");
                    //Label Effective_Date = (Label)row.FindControl("lblEffectiveDate");
                    //Label Policy_Status = (Label)row.FindControl("lblPolicyStatus");
                    //Label Assured_Amount = (Label)row.FindControl("lblAssuredAmount");
                    //Label Currancy = (Label)row.FindControl("lblCurrancy");
                    //Label Group = (Label)row.FindControl("lblGroup");
                    //Label Payment_Mode = (Label)row.FindControl("lblPaymentMode");
                    //Label Monthly_Premium = (Label)row.FindControl("lblMonthlyPremium");
                    //Label Premium_After_Discount = (Label)row.FindControl("lblPremiumAfterDiscount");
                    //Label Extra_Premium = (Label)row.FindControl("lblExtraPremium");
                    //Label Total_Premium = (Label)row.FindControl("lblTotalPremium");
                    //Label Paid_Off_In_Month = (Label)row.FindControl("lblPaid");
                    //Label Terminate_Date = (Label)row.FindControl("lblTerminateDate");
                    //Label remarks = (Label)row.FindControl("lblRemarks");
                    #endregion

                    #region set value
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheets.CreateRow(row_no);
                    rowCell.RowStyle = style;

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(Branch);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(Customer_ID);

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(Loan_ID);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(Certificate_No);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(Last_Name);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(First_Name);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(DOB);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(Age);

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Gender);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Product);

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(Opened_Date);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(Duration);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(Amount);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(Outstanding_Balance);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(Date_Of_Entry);

                    HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                    Cell17.SetCellValue(Cover_Year);

                    HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                    Cell18.SetCellValue(Effective_Date);

                    HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                    Cell19.SetCellValue(Policy_Status);

                    HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                    Cell20.SetCellValue(Assured_Amount);

                    HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);
                    Cell21.SetCellValue(Currancy);

                    HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(21);
                    Cell22.SetCellValue(Group);

                    HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(22);
                    Cell23.SetCellValue(Payment_Mode);

                    HSSFCell Cell24 = (HSSFCell)rowCell.CreateCell(23);
                    Cell24.SetCellValue(Monthly_Premium);

                    HSSFCell Cell25 = (HSSFCell)rowCell.CreateCell(24);
                    Cell25.SetCellValue(Premium_After_Discount);

                    HSSFCell Cell26 = (HSSFCell)rowCell.CreateCell(25);
                    Cell26.SetCellValue(Extra_Premium);

                    HSSFCell Cell27 = (HSSFCell)rowCell.CreateCell(26);
                    Cell27.SetCellValue(Total_Premium);

                    HSSFCell Cell28 = (HSSFCell)rowCell.CreateCell(27);
                    Cell28.SetCellValue(Paid_Off_In_Month);

                    HSSFCell Cell29 = (HSSFCell)rowCell.CreateCell(28);
                    Cell29.SetCellValue(Terminate_Date);

                    HSSFCell Cell30 = (HSSFCell)rowCell.CreateCell(29);
                    Cell30.SetCellValue(remarks);

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

    class DataCollection : bl_cmk_load_data
    {
        public string Remarks { get; set; }
    }

}