using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Web.Security;
using System.IO;
using System.Threading;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;

public partial class Pages_Business_CI_frmCI : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
   private  Helper.FormTransactionType PageTransaction {get { return ( Helper.FormTransactionType)ViewState["VS_PAGE_TRANSACTION"]; } set { ViewState["VS_PAGE_TRANSACTION"] = value; } }
    
    List<bl_ci_load_data> listData = new List<bl_ci_load_data>();
    List<da_report_approver.bl_report_approver> ApproverList;
    string user_login = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        ibtnSave.Enabled = false;
        user_login = Membership.GetUser().UserName;
        export_excel.Enabled = false;
        ApproverList = da_report_approver.GetApproverList();
        if (!Page.IsPostBack)
        {
   
            txtEffectiveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
    List<InvalidData> invalid_list = new List<InvalidData>();
    bool validRow(DataRow row, int row_index)
    {
        bool valid = true;

        InvalidData invalid_data = new InvalidData();
        int customer_age = 0;
        double[] premium = new double[] { 0, 0 };
        bl_product product = new bl_product();
        try
        {
            //product = da_product.GetProductByProductID("CI");
            product = da_product.GetProductByProductID(row[11].ToString().Trim());
            //invalid_list = (List<InvalidData>)Session["INVALID_DATA"];

            #region //Check invalid data
            customer_age = Calculation.Culculate_Customer_Age(row[8].ToString().Trim(), DateTime.Now);
            if (Helper.GetIDCardTypeID(row[1].ToString().Trim()) < 0)//ID Type is emqty
            {
                valid = false;
                invalid_data.Remarks = "Invalid ID type.";

            }
            else if (row[2].ToString().Trim() == "" || row[2].ToString() == null)//ID is emqty
            {
                valid = false;
                invalid_data.Remarks = "ID is required.";
            }
            else if (row[3].ToString().Trim() == "")//Sur Name is emqty
            {
                valid = false;
                invalid_data.Remarks = "Surname (EN) is required.";
            }
            else if (row[4].ToString().Trim() == "")//Given Name is emqty
            {
                valid = false;
                invalid_data.Remarks = "Given name (EN) is required.";
            }
            else if (row[7].ToString().Trim() == "")//Gender
            {
                valid = false;
                invalid_data.Remarks = "Gender is required.";
            }
            else if (row[8].ToString().Trim() == "")//DOB
            {
                valid = false;
                invalid_data.Remarks = "DOB is required.";
            }

            else if (Helper.IsDate(row[8].ToString().Trim()) == false)//check dob date format
            {
                valid = false;
                invalid_data.Remarks = "Invalid DOB";
            }

            else if (customer_age < product.Age_Min || customer_age > product.Age_Max)
            {
                valid = false;
                invalid_data.Remarks = "Age=" + customer_age + " is not in range [" + product.Age_Min + "-" + product.Age_Max + "]";

            }
            //check age range

            else if (row[9].ToString().Trim() == "")//Phone Number
            {
                valid = false;
                invalid_data.Remarks = "Phone number is required.";
            }
            else if (row[9].ToString().Replace(" ", "").Trim().Length < 9 || row[9].ToString().Replace(" ", "").Trim().Length > 10)//phone length
            {
                valid = false;
                invalid_data.Remarks = "Phone number is invalid length.";
            }
            else if (row[10].ToString().Trim() == "")//Province
            {
                valid = false;
                invalid_data.Remarks = "Province is required.";
            }
            else if (row[12].ToString().Trim() == "")//Sum Assured
            {
                valid = false;
                invalid_data.Remarks = "Sum ansured is required.";
            }
            else if (Helper.IsNumeric(row[12].ToString().Trim()) == false)//Check sum assured is number or not
            {
                valid = false;
                invalid_data.Remarks = "Sum ansured is invalid format.";
            }

            else if (Helper.IsNumeric(row[13].ToString().Trim()) == false)//check Premium paid is number or not
            {
                valid = false;
                invalid_data.Remarks = "Invalid premium paid.";
            }

            else if (Helper.GetPayModeID(row[14].ToString().Trim()) < 0)//Payment Mode
            {
                valid = false;
                invalid_data.Remarks = "Mode of payment is required.";
            }
            else if (row[15].ToString().Trim() == "")//Payment by
            {
                valid = false;
                invalid_data.Remarks = "Payment by is required.";
            }
            else if (row[16].ToString().Trim() == "")//Agent Code
            {
                valid = false;
                invalid_data.Remarks = "Agent code is required.";
            }
            else
            {
                #region //Check premium amount
                
                if (Helper.IsNumeric(row[13].ToString().Trim()) == true && row[12].ToString().Trim() != "" && Helper.GetPayModeID(row[14].ToString().Trim()) > 0)
                {
                    premium = da_ci.GetPremium(Convert.ToDouble(row[12].ToString().Trim()), row[11].ToString().Trim(), Helper.GetPayModeID(row[14].ToString().Trim()));
                    if (Convert.ToDouble(row[13].ToString().Trim()) < premium[0])
                    {
                        valid = false;
                        invalid_data.Remarks = "Insufficient premium paid";
                    }
                }
                #endregion
            }
          
            //store invalid data
            if (!valid)
            {
                invalid_data.IDTypeText = row[1].ToString().Trim();
                invalid_data.IDType = Helper.GetIDCardTypeID(invalid_data.IDTypeText);
                invalid_data.ID = row[2].ToString();
                invalid_data.ENFirstName = row[4].ToString();
                invalid_data.ENLastName = row[3].ToString();
                invalid_data.KHFirstName = row[6].ToString();
                invalid_data.KHLastName = row[5].ToString();
                invalid_data.Gender = row[7].ToString();
                invalid_data.DOB = row[8].ToString().Trim();
                invalid_data.PhoneNumber = row[9].ToString().Trim();
                invalid_data.PolicyNumber = "855" + invalid_data.PhoneNumber.Substring(1, invalid_data.PhoneNumber.Length - 1);//Make up phone number format
                invalid_data.Age = Calculation.Culculate_Customer_Age(invalid_data.DOB, DateTime.Now) + "";
                invalid_data.CountryCode = "KH";
                invalid_data.Province = row[10].ToString().Trim();
                invalid_data.ProductName = row[11].ToString().Trim();
                invalid_data.SA = row[12].ToString().Trim();
                invalid_data.UserPremium = row[13].ToString().Trim();
                invalid_data.PayModeText = row[14].ToString().Trim();
                invalid_data.PayMode = Helper.GetPayModeID(invalid_data.PayModeText) + "";
                invalid_data.SystemPremium = premium[0] + "";
                invalid_data.OriginalPremium = premium[1] + "";
                invalid_data.PaymentBy = row[15].ToString().Trim();
                invalid_data.Agent_code = row[16].ToString();
                invalid_data.PaymentCode = GetPaymentCode(invalid_data.PolicyNumber);

                invalid_list.Add(invalid_data);

            }
            Session["INVALID_DATA"] = invalid_list;
            #endregion
        }
        catch (Exception ex)
        {
            valid = false;
            Log.AddExceptionToLog("Error function [validRow(DataRow row)] in class [frmCI.aspx.cs], row index [" + row_index + "], detail:" + ex.Message);
        }
        return valid;
    }
    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnLoadData.Text == "Load Data")
            {
                PageTransaction = Helper.FormTransactionType.SEARCH;
           
            }
            else if (btnLoadData.Text == "Re-Load")
            {
                PageTransaction = Helper.FormTransactionType.RELOAD;
            }

            if (PageTransaction == Helper.FormTransactionType.SEARCH)
            {
                if (txtEffectiveDate.Text.Trim() != "" && Helper.IsDate(txtEffectiveDate.Text.Trim()))
                {
                    if ((flUpload.PostedFile != null) && !string.IsNullOrEmpty(flUpload.PostedFile.FileName))
                    {
                        if (ddlApprover.SelectedValue.Trim() != "")
                        {
                            Session["APPROVER_ID"] = ddlApprover.SelectedValue;
                            //check valid
                            string message;
                            if (Validate(out message) != false)
                            {
                                string save_path = "~/Upload/";
                                string file_name = Path.GetFileName(flUpload.PostedFile.FileName);
                                string extension = Path.GetExtension(file_name);
                                file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
                                string file_path = save_path + file_name;
                                int row_number = 0;

                                flUpload.SaveAs(Server.MapPath(file_path));//save file 

                                ExcelConnection my_excel = new ExcelConnection();
                                my_excel.FileName = Server.MapPath(file_path);
                                my_excel.CommandText = "SELECT * FROM [Upload_Format$]";
                                DataTable my_data = new DataTable();
                                my_data = my_excel.GetData();

                                ViewState["my_data"] = my_data;
                                row_number = my_data.Rows.Count;

                                //Get product detail information
                                //listData = new List<bl_ci_load_data>();
                                foreach (DataRow row in my_data.Rows)
                                {
                                    if (validRow(row, my_data.Rows.IndexOf(row)))
                                    {
                                        if (row[1].ToString().Trim() != "" && row[2].ToString().Trim() != "" && row[7].ToString().Trim() != "" && row[8].ToString().Trim() != ""
                                        && row[9].ToString().Trim() != "" && row[11].ToString().Trim() != "" && row[12].ToString().Trim() != "" && row[13].ToString().Trim() != "")
                                        {
                                            bl_ci_load_data ci_load_data = new bl_ci_load_data();
                                            ci_load_data.IDTypeText = row[1].ToString().Trim();
                                            ci_load_data.IDType = Helper.GetIDCardTypeID(ci_load_data.IDTypeText);
                                            ci_load_data.ID = row[2].ToString();
                                            ci_load_data.ENFirstName = row[4].ToString();
                                            ci_load_data.ENLastName = row[3].ToString();
                                            ci_load_data.KHFirstName = row[6].ToString();
                                            ci_load_data.KHLastName = row[5].ToString();
                                            ci_load_data.Gender = row[7].ToString();
                                            ci_load_data.DOB = row[8].ToString().Trim();
                                            ci_load_data.PhoneNumber = row[9].ToString().Replace(" ", "").Trim();
                                            ci_load_data.PolicyNumber = "855" + ci_load_data.PhoneNumber.Substring(1, ci_load_data.PhoneNumber.Length - 1);//Make up phone number format
                                            /*Modified by Maneth @04July2019*/
                                            //ci_load_data.Age = Calculation.Culculate_Customer_Age(ci_load_data.DOB, DateTime.Now) + "";
                                            ci_load_data.EffectiveDate = Helper.FormatDateTime(txtEffectiveDate.Text.Trim());
                                            ci_load_data.Age = Calculation.Culculate_Customer_Age(ci_load_data.DOB, ci_load_data.EffectiveDate) + "";

                                            ci_load_data.CountryCode = "KH";
                                            ci_load_data.Province = row[10].ToString().Trim();
                                            ci_load_data.ProductName = row[11].ToString().Trim();
                                            ci_load_data.SA = Convert.ToDouble(row[12].ToString().Trim()) + "";
                                            ci_load_data.UserPremium = Convert.ToDouble(row[13].ToString().Trim()) + "";
                                            ci_load_data.PayModeText = row[14].ToString().Trim();
                                            ci_load_data.PayMode = Helper.GetPayModeID(ci_load_data.PayModeText) + "";

                                            double[] premium = new double[] { 0, 0 };
                                            premium = da_ci.GetPremium(Convert.ToDouble(ci_load_data.SA), ci_load_data.ProductName, Convert.ToInt32(ci_load_data.PayMode));

                                            ci_load_data.SystemPremium = premium[0] + "";// da_ci.GetPremium(ci_load_data.SA, ci_load_data.ProductName, ci_load_data.PayMode)[0];
                                            ci_load_data.OriginalPremium = premium[1] + "";// da_ci.GetPremium(ci_load_data.SA, ci_load_data.ProductName, ci_load_data.PayMode)[1];
                                            ci_load_data.PaymentBy = row[15].ToString().Trim();
                                            ci_load_data.Agent_code = row[16].ToString();
                                            ci_load_data.PaymentCode = GetPaymentCode(ci_load_data.PolicyNumber);
                                            listData.Add(ci_load_data);

                                        }

                                    }

                                }
                                gv_valid.DataSource = listData;
                                Session["CI"] = listData;
                                gv_valid.DataBind();

                                //display invalid data
                                LoadInvalidData();

                                if (listData.Count > 0 && gv_invalid.Rows.Count==0)
                                {
                                    //btnSend.Enabled = true;
                                    ibtnSave.Enabled = true;
                                    //ibtnSave.Attributes.Add("Onclick", "return confirm('Confirm to Save!')");
                                    //ibtnSave.Attributes.Add("Onclick", "confirm_save();");
                                    EnableControlls(false);
                                    btnLoadData.Text = "Re-Load";
                                    AlertMessage("All uploaded data are valid.");

                                }
                                else if (listData.Count > 0 && gv_invalid.Rows.Count > 0)
                                {
                                    EnableControlls(false);
                                    btnLoadData.Text = "Re-Load";
                                    AlertMessage("Some uploaded data are invalid. please see detail in Fail TAB.");
                                }
                                else if (listData.Count == 0 && gv_invalid.Rows.Count > 0)
                                {
                                    AlertMessage("All uploaded data are invalid. Please recheck raw data the try again.");
                                }
                                else
                                {
                                    AlertMessage("No record found.");
                                    ibtnSave.Enabled = false;
                                    EnableControlls();
                                }
                               
                                string logDes = string.Concat(" Effective date:", txtEffectiveDate.Text.Trim(), "; approver:", ddlApprover.SelectedItem.Text, "; option send SMS:", ckbSendSMS.Checked == true ? "YES" : "NO");
                                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, String.Concat("User uploads temporary simple one data with criteria [", logDes, "] [Total success:", listData.Count, " fail:", gv_invalid.Rows.Count, "]."));

                            }
                            else
                            {
                                AlertMessage(message);
                                return;
                            }
                        }
                        else //not selected approver
                        {
                            AlertMessage("Please select approver.");
                        }
                    }
                    else // not selected file
                    {
                        AlertMessage("Please select your file.");
                    }
                }
                else
                {
                    AlertMessage("Effective Date Is Required.");
                }
            }
            else if (PageTransaction == Helper.FormTransactionType.RELOAD)
            {
                gv_invalid.DataSource = null;
                gv_invalid.DataBind();
                gv_valid.DataSource = null;
                gv_valid.DataBind();
                ibtnSave.Enabled = false;
                EnableControlls();
                btnLoadData.Text = "Load Data";
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnLoadData_Click] in page [frmCI.aspx.cs], detail: " + ex.Message);
            AlertMessage("Upload data fail please contact your system administrator.");
        }
    }
    void LoadInvalidData()
    {
        gv_invalid.DataSource = (List<InvalidData>)Session["INVALID_DATA"];
        gv_invalid.DataBind();
        if (gv_invalid.Rows.Count > 0)
        {
            export_excel.Enabled = true;
        }
        else
        {
            export_excel.Enabled = false;
        }
    }
    bool Validate(out string message)
    {
        bool status = true;
        message = "";
        string file_path = "";
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
                if (col_count > 17 || col_count < 0)//check number of columns
                {
                    message = "File is not correct format, please donwload file template from the system.";
                    status = false;
                }
                else
                {//check column name.
                    if (tbl.Columns[0].ColumnName.Trim() != "No#" || tbl.Columns[1].ColumnName.Trim() != "ID_Type"
                        || tbl.Columns[2].ColumnName.Trim() != "ID" || tbl.Columns[3].ColumnName.Trim() != "Surname(En)" || tbl.Columns[4].ColumnName.Trim() != "Given Name(En)"
                        || tbl.Columns[5].ColumnName.Trim() != "Surname(Kh)" || tbl.Columns[6].ColumnName.Trim() != "Given Name(Kh)" || tbl.Columns[7].ColumnName.Trim() != "Gender_(M,F)"
                        || tbl.Columns[8].ColumnName.Trim() != "DOB_ (DD-MM-YYYY)" || tbl.Columns[9].ColumnName.Trim() != "Phone Number _(012 XXX XXX)" || tbl.Columns[10].ColumnName.Trim() != "Province"
                        || tbl.Columns[11].ColumnName.Trim() != "Product_ Name" || tbl.Columns[12].ColumnName.Trim() != "Sum Assured_ (USD)" || tbl.Columns[13].ColumnName.Trim() != "Premium_ Paid"
                        || tbl.Columns[14].ColumnName.Trim() != "Mode Of _Payment" || tbl.Columns[15].ColumnName.Trim() != "Payment By" || tbl.Columns[16].ColumnName.Trim() != "Agent Code")
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

    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }

    void SaveSuccessLog(string message)
    {
        Log.CreateLog("CI_Log", message);
    }
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "", "loading();", true);
        save_data();
        //ClientScript.RegisterStartupScript(this.GetType(), "", "Endloading();", true);

    }
    protected void ibtnClear_Click(object sender, ImageClickEventArgs e)
    {
        listData = new List<bl_ci_load_data>();
        Session["CI"] = listData;
        invalid_list = new List<InvalidData>();
        Session["INVALID_DATA"] = invalid_list;

        LoadInvalidData();
        gv_valid.DataSource = listData;
        gv_valid.DataBind();

        Session.Clear();
    }

    void save_data()
    {
        string err_message = "";
        #region //Remmarks Data Write into database
        /*
         *write data into table 
         *1.ct_customer
         *2.ct_policy_address
         *3.ct_ci_policy
         *4.ct_ci_policy_detail
         *5.ct_policy_pay_mode
         *6.ct_policy_status
         *7.ct_policy_prem_pay
         *8.ct_policy_contact
         *9. ct_report_approver_policy
         */
        #endregion
        bl_ci.Policy policy;
        bl_ci.PolicyDetail policy_detail;
        string exist_customer_id = "";
        string exist_cellcard_customer = "";
        string exist_policy_number = "";
        string customer_id = "";
        string rollback_customer_id = "";
        string rollback_policy_id = "";
        string rollback_policy_detail_id = "";
        string rollback_address_id = "";
        string rollback_policy_prem_pay_id = "";
        double return_premium = 0;
        int time_to_pay = 0;
        bool isSaveAble = false;
        List<bl_ci_load_data> listData = new List<bl_ci_load_data>();
        string policy_status = "";
        try
        {
            listData = (List<bl_ci_load_data>)Session["CI"];
            foreach (bl_ci_load_data ci_data in listData)//Loop all valid data
            {

                #region //Check existing policy in call center
                exist_cellcard_customer = da_ci.GetCellCardCustomer(ci_data.PolicyNumber);// i use policy number as phone number cuz policy number format = phone number
                #endregion

                #region //exist_cellcard_customer == ""
                if (exist_cellcard_customer == "")
                {
                    #region //Check exist policy number in ct_ci_policy
                    policy = new bl_ci.Policy();
                    policy = da_ci.Policy.GetPolicyByPolicyNumber(ci_data.PolicyNumber);
                    if (policy.CustomerID != null)
                    {
                        exist_policy_number = policy.PolicyNumber;
                    }
                    #endregion

                    #region //exist_policy_number ==""
                    if (exist_policy_number == "")
                    {
                        //get existing customer in ct_customer
                        exist_customer_id = da_customer.GetCustomerIDByParameters(ci_data.ENFirstName.Trim(), ci_data.ENLastName.Trim(), Helper.GetGender(ci_data.Gender), Helper.FormatDateTime(ci_data.DOB));
                        bl_ci.Policy ci_pol = new bl_ci.Policy();
                        ci_pol = da_ci.Policy.GetPolicyByCustomerID(exist_customer_id);
                        DataTable tbl_ci_detail = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), "", ci_pol.PolicyNumber, "SO");

                        if (tbl_ci_detail.Rows.Count > 0)
                        {
                            //policy_status = tbl_ci_detail.Rows[0]["policy_status"].ToString().Trim();
                            //filter last effective date
                            DataRow[] row = tbl_ci_detail.Select("effective_date = max(effective_date)");

                            policy_status = row[0]["policy_status"].ToString().Trim();
                        }

                        #region //Check existing custom buy ci
                        if (ci_pol.CustomerID != null && ci_pol.CustomerID != "")
                        {
                            if (policy_status.ToUpper() != "TER" && policy_status.ToUpper() != "")
                            {

                                invalid_list = (List<InvalidData>)Session["INVALID_DATA"];
                                invalid_list.Add(new InvalidData()
                                {
                                    ID = ci_data.ID,
                                    IDType = ci_data.IDType,
                                    IDTypeText = ci_data.IDTypeText,
                                    ENFirstName = ci_data.ENFirstName,
                                    ENLastName = ci_data.ENLastName,
                                    KHFirstName = ci_data.KHFirstName,
                                    KHLastName = ci_data.KHLastName,
                                    Gender = ci_data.Gender,
                                    DOB = ci_data.DOB,
                                    PhoneNumber = ci_data.PolicyNumber,
                                    Province = ci_data.Province,
                                    ProductName = ci_data.ProductName,
                                    SA = ci_data.SA,
                                    UserPremium = ci_data.UserPremium,
                                    SystemPremium = ci_data.SystemPremium,
                                    OriginalPremium = ci_data.OriginalPremium,
                                    PayMode = ci_data.PayMode,
                                    PayModeText = ci_data.PayModeText,
                                    PaymentCode = ci_data.PaymentCode,
                                    PaymentBy = ci_data.PaymentBy,
                                    Agent_code = ci_data.Agent_code,
                                    Remarks = "Customer is already exist with policy [" + ci_pol.PolicyNumber + "] status [" + policy_status + "]"
                                });

                                isSaveAble = false;

                            }
                            else
                            {
                                isSaveAble = true;
                                customer_id = ci_pol.CustomerID;
                            }

                        }
                        #endregion
                        #region // new customer ci
                        else
                        {
                            #region //Check exist customer & Get new customer
                            if (exist_customer_id == "")
                            {
                                customer_id = da_customer.GetCustomerID();//generate new customer id
                                #region //Save Customer
                                bl_customer customer = new bl_customer();
                                customer.Customer_ID = customer_id;
                                customer.ID_Type = ci_data.IDType;
                                customer.ID_Card = ci_data.ID;
                                customer.First_Name = ci_data.ENFirstName;
                                customer.Last_Name = ci_data.ENLastName;
                                customer.Khmer_First_Name = ci_data.KHFirstName;
                                customer.Khmer_Last_Name = ci_data.KHLastName;
                                customer.Prior_First_Name = "";
                                customer.Prior_Last_Name = "";
                                customer.Gender = Helper.GetGender(ci_data.Gender);
                                customer.Birth_Date = Helper.FormatDateTime(ci_data.DOB);
                                customer.Country_ID = ci_data.CountryCode;
                                customer.Father_First_Name = "";
                                customer.Father_Last_Name = "";
                                customer.Mother_First_Name = "";
                                customer.Mother_Last_Name = "";
                                customer.Created_By = user_login;
                                customer.Created_On = DateTime.Now;
                                rollback_customer_id = customer_id;//use for rollback data
                                if (da_customer.InsertCustomer(customer, user_login))
                                {
                                    SaveSuccessLog("[" + user_login + "] 1. Save customer success, Customer ID[" + customer.Customer_ID + "]");
                                }
                                else
                                {

                                    err_message = "[" + user_login + "] Save cutomer fail";
                                    goto EXIT;
                                }

                                #endregion
                            }
                            else
                            {
                                customer_id = exist_customer_id;
                                rollback_customer_id = "";// prevent to delete existing customer incase system get error and rollback data.

                            }
                            isSaveAble = true;
                            #endregion

                        }
                        #endregion


                    }
                    #endregion
                    #region //exist_policy_number !=""
                    else
                    {
                        DataTable tbl_ci_detail = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), "", exist_policy_number, policy.ProductID);

                        if (tbl_ci_detail.Rows.Count > 0)
                        {
                            //Get last record base on max effective date
                            DataRow[] row = tbl_ci_detail.Select("effective_date = max(effective_date)");

                            policy_status = row[0]["policy_status"].ToString().Trim();
                        }
                        #region //Policy status is not TER
                        if (policy_status.Trim() != "" && policy_status.ToUpper().Trim() != "TER")
                        {

                            invalid_list = (List<InvalidData>)Session["INVALID_DATA"];
                            invalid_list.Add(new InvalidData()
                            {
                                ID = ci_data.ID,
                                IDType = ci_data.IDType,
                                IDTypeText = ci_data.IDTypeText,
                                ENFirstName = ci_data.ENFirstName,
                                ENLastName = ci_data.ENLastName,
                                KHFirstName = ci_data.KHFirstName,
                                KHLastName = ci_data.KHLastName,
                                Gender = ci_data.Gender,
                                DOB = ci_data.DOB,
                                PhoneNumber = ci_data.PolicyNumber,
                                Province = ci_data.Province,
                                ProductName = policy.ProductID,// ci_data.ProductName,
                                SA = ci_data.SA,
                                UserPremium = ci_data.UserPremium,
                                SystemPremium = ci_data.SystemPremium,
                                OriginalPremium = ci_data.OriginalPremium,
                                PayMode = ci_data.PayMode,
                                PayModeText = ci_data.PayModeText,
                                PaymentCode = ci_data.PaymentCode,
                                PaymentBy = ci_data.PaymentBy,
                                Agent_code = ci_data.Agent_code,
                                Remarks = "Existing policy No.[" + ci_data.PolicyNumber + "]"
                            });
                            isSaveAble = false;
                        }
                        #endregion
                        #region //Policy status is TER
                        else
                        {
                            isSaveAble = true;
                            customer_id = tbl_ci_detail.Rows[0]["customer_id"].ToString(); // 

                        }
                        #endregion
                    }
                    #endregion


                }
                #endregion

                #region //exist_cellcard_customer != ""
                else
                {
                    //customer is already exist in cellcard 
                    invalid_list = (List<InvalidData>)Session["INVALID_DATA"];
                    invalid_list.Add(new InvalidData()
                    {
                        ID = ci_data.ID,
                        IDType = ci_data.IDType,
                        IDTypeText = ci_data.IDTypeText,
                        ENFirstName = ci_data.ENFirstName,
                        ENLastName = ci_data.ENLastName,
                        KHFirstName = ci_data.KHFirstName,
                        KHLastName = ci_data.KHLastName,
                        Gender = ci_data.Gender,
                        DOB = ci_data.DOB,
                        PhoneNumber = ci_data.PolicyNumber,
                        Province = ci_data.Province,
                        ProductName = ci_data.ProductName,
                        SA = ci_data.SA,
                        UserPremium = ci_data.UserPremium,
                        SystemPremium = ci_data.SystemPremium,
                        OriginalPremium = ci_data.OriginalPremium,
                        PayMode = ci_data.PayMode,
                        PayModeText = ci_data.PayModeText,
                        PaymentCode = ci_data.PaymentCode,
                        PaymentBy = ci_data.PaymentBy,
                        Agent_code = ci_data.Agent_code,
                        Remarks = "Exist in callcard (CLI)"
                    });
                    isSaveAble = false;
                }
                #endregion

                if (isSaveAble)
                #region//Save data
                {

                    #region //Save Address
                    bl_policy_address address = new bl_policy_address();
                    address.Address_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_POLICY_ADDRESS" }, { "FIELD", "ADDRESS_ID" } });
                    address.Address1 = "";
                    address.Address2 = "";
                    address.Address3 = "";
                    address.Province = ci_data.Province;
                    address.Zip_Code = "855";
                    address.Country_ID = ci_data.CountryCode;
                    rollback_address_id = address.Address_ID;//use for rollback data
                    if (da_policy.InsertPolicyAddress(address))
                    {
                        SaveSuccessLog("[" + user_login + "] 2. Save address success [address id =" + address.Address_ID + "]");
                    }
                    else
                    {
                        err_message = "[" + user_login + "] Save address fail [address id =" + address.Address_ID + "]";
                        goto EXIT;
                    }

                    #endregion

                    #region //Save CI Policy
                    policy = new bl_ci.Policy();
                    policy.PolicyID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CI_POLICY" }, { "FIELD", "POLICY_ID" } });
                    policy.PolicyNumber = ci_data.PolicyNumber;
                    policy.CustomerID = customer_id;
                    policy.ProductID = ci_data.ProductName;
                    policy.AddressID = address.Address_ID;//address from object address
                    policy.AgentCode = ci_data.Agent_code;
                    policy.CreatedBy = user_login;
                    policy.CreatedDateTime = DateTime.Now;
                    rollback_policy_id = policy.PolicyID;//use for rollback data
                    if (da_ci.Policy.SavePolicy(policy))
                    {
                        SaveSuccessLog("[" + user_login + "] 3. Save policy success [policy id =" + policy.PolicyID + "]");

                    }
                    else
                    {
                        err_message = "[" + user_login + "] Save policy fail [policy id =" + policy.PolicyID + "]";
                        goto EXIT;
                    }

                    #endregion

                    #region //Save CI Policy Detail
                    TimeSpan time = new TimeSpan(23, 59, 00);//time :11:59:00 PM

                    #region //Check customer pay many times
                    if (Convert.ToDouble(ci_data.UserPremium) >= Convert.ToDouble(ci_data.SystemPremium))
                    {
                        time_to_pay = Convert.ToInt32(Convert.ToDouble(ci_data.UserPremium) / Convert.ToDouble(ci_data.SystemPremium));
                        return_premium = Convert.ToDouble(ci_data.UserPremium) - (Convert.ToDouble(ci_data.SystemPremium) * time_to_pay);
                        return_premium = Math.Round(return_premium, 2);
                    }
                    #endregion
                    policy_detail = new bl_ci.PolicyDetail();
                    policy_detail.PolicyDetailID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CI_POLICY_DETAIL" }, { "FIELD", "POLICY_DETAIL_ID" } });
                    policy_detail.PolicyID = policy.PolicyID;
                    /*Modified By Maneth @04July2019*/
                    //policy_detail.EffectiveDate = DateTime.Now.Date + time;
                    policy_detail.EffectiveDate = ci_data.EffectiveDate.Date + time;

                    policy_detail.MaturityDate = policy_detail.EffectiveDate.AddYears(1);
                    policy_detail.ExpiryDate = policy_detail.MaturityDate.Date.AddDays(-1) + time;
                    policy_detail.IssuedDate = DateTime.Now;
                    policy_detail.SumAssured = Convert.ToDouble(ci_data.SA);
                    policy_detail.PayModeID = Convert.ToInt32(ci_data.PayMode);
                    policy_detail.PaymentCode = ci_data.PaymentCode;
                    policy_detail.PaymentBy = ci_data.PaymentBy;
                    policy_detail.UserPremium = Convert.ToDouble(ci_data.UserPremium);
                    policy_detail.Premium = Convert.ToDouble(ci_data.SystemPremium);
                    policy_detail.RETURN_PREMIUM = return_premium;
                    policy_detail.OriginalPremium = Convert.ToDouble(ci_data.OriginalPremium);
                    policy_detail.DiscountAmount = Convert.ToDouble(ci_data.DiscountAmount);
                    policy_detail.Age = Convert.ToInt32(ci_data.Age);
                    policy_detail.CoverYear = 1;
                    policy_detail.PayYear = 1;
                    policy_detail.PayUpToAge = policy_detail.Age + policy_detail.CoverYear;
                    policy_detail.CoverUpToAge = policy_detail.Age + policy_detail.CoverYear;
                    policy_detail.PolicyStatusRemarks = "New";
                    policy_detail.CreatedBy = user_login;
                    policy_detail.CreatedDateTime = DateTime.Now;
                    rollback_policy_detail_id = policy_detail.PolicyDetailID;//use for rollback data
                    // da_ci.PolicyDetail.SavePolicyDetail(policy_detail);
                    if (da_ci.PolicyDetail.SavePolicyDetail(policy_detail))
                    {
                        SaveSuccessLog("[" + user_login + "] 4. Save policy detail success [policy detail id =" + policy_detail.PolicyDetailID + "]");
                    }
                    else
                    {
                        err_message = "[" + user_login + "] Save policy detail fail [policy detail id =" + policy_detail.PolicyDetailID + "]";
                        goto EXIT;
                    }
                    #region //Show return premium to user
                    if (return_premium > 0)
                    {
                        invalid_list = (List<InvalidData>)Session["INVALID_DATA"];
                        invalid_list.Add(new InvalidData()
                        {
                            ID = ci_data.ID,
                            IDType = ci_data.IDType,
                            IDTypeText = ci_data.IDTypeText,
                            ENFirstName = ci_data.ENFirstName,
                            ENLastName = ci_data.ENLastName,
                            KHFirstName = ci_data.KHFirstName,
                            KHLastName = ci_data.KHLastName,
                            Gender = ci_data.Gender,
                            DOB = ci_data.DOB,
                            PhoneNumber = ci_data.PolicyNumber,
                            Province = ci_data.Province,
                            ProductName = ci_data.ProductName,
                            SA = ci_data.SA,
                            UserPremium = return_premium + "",
                            SystemPremium = ci_data.SystemPremium,
                            OriginalPremium = ci_data.OriginalPremium,
                            PayMode = ci_data.PayMode,
                            PayModeText = ci_data.PayModeText,
                            PaymentCode = ci_data.PaymentCode,
                            PaymentBy = ci_data.PaymentBy,
                            Agent_code = ci_data.Agent_code,
                            Remarks = "Return Premium Back"
                        });
                    }
                    #endregion
                    #endregion

                    #region //Save Policy Contact
                    bl_app_info_contact contact = new bl_app_info_contact();
                    contact.PolicyID = policy.PolicyID;
                    contact.Mobile_Phone1 = ci_data.PhoneNumber;
                    contact.Mobile_Phone2 = "";
                    contact.Home_Phone1 = "";
                    contact.Home_Phone2 = "";
                    contact.Office_Phone1 = "";
                    contact.Office_Phone2 = "";
                    contact.Fax1 = "";
                    contact.Fax2 = "";
                    contact.EMail = "";

                    if (da_policy.InsertPolicyContact(contact))
                    {
                        SaveSuccessLog("[" + user_login + "] 5. Save policy contact success [policy id =" + policy.PolicyID + "]");
                    }
                    else
                    {
                        err_message = "[" + user_login + "] Save policy contact fail [policy id =" + policy.PolicyID + "]";
                        goto EXIT;
                    }
                    #endregion

                    #region //Save Policy Status

                    da_policy.InsertPolicyStatus(policy.PolicyID, user_login, DateTime.Now);
                    SaveSuccessLog("[" + user_login + "] 6. Save policy status success [policy id =" + policy.PolicyID + "]");
                    #endregion

                    #region //Save Policy Pay Mode
                    if (da_policy.InsertPolicyPayMode(policy.PolicyID, Convert.ToInt32(ci_data.PayMode), DateTime.Now, user_login, DateTime.Now))
                    {
                        SaveSuccessLog("[" + user_login + "] 7. Save policy pay mode success [policy id =" + policy.PolicyID + "]");
                    }
                    else
                    {
                        err_message = "[" + user_login + "] Save policy pay mode fail [policy id =" + policy.PolicyID + "]";
                        goto EXIT;
                    }
                    #endregion

                    #region //Save Policy Premium Pay
                    DateTime due_date, next_due_date;
                    due_date = DateTime.Now.Date;
                    for (int i = 1; i <= time_to_pay; i++)
                    {

                        if (i > 1)
                        {
                            if (Convert.ToInt32(ci_data.PayMode) == 2)
                            {
                                next_due_date = due_date.AddMonths(6);
                                due_date = Calculation.GetNext_Due(next_due_date, due_date, policy_detail.EffectiveDate);
                            }

                        }
                        bl_policy_prem_pay pol_prem_pay = new bl_policy_prem_pay();
                        pol_prem_pay.Policy_Prem_Pay_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_POLICY_PREM_PAY" }, { "FIELD", "POLICY_PREM_PAY_ID" } });
                        pol_prem_pay.Policy_ID = policy.PolicyID;
                        pol_prem_pay.Due_Date = due_date;
                        pol_prem_pay.Pay_Date = DateTime.Now;
                        pol_prem_pay.Prem_Year = 1;
                        pol_prem_pay.Prem_Lot = i;
                        pol_prem_pay.Amount = Convert.ToDouble(ci_data.SystemPremium);
                        pol_prem_pay.Sale_Agent_ID = ci_data.Agent_code;
                        pol_prem_pay.Office_ID = "HQ";
                        pol_prem_pay.Created_By = user_login;
                        pol_prem_pay.Created_On = DateTime.Now;
                        pol_prem_pay.Created_Note = "";
                        pol_prem_pay.Pay_Mode_ID = Convert.ToInt32(ci_data.PayMode);
                        rollback_policy_prem_pay_id = pol_prem_pay.Policy_Prem_Pay_ID;//use for rollback data
                        if (da_policy.InsertPolicyPremiumPay(pol_prem_pay))
                        {
                            SaveSuccessLog("[" + user_login + "] 8. Save policy premium pay success [policy premium pay id =" + pol_prem_pay.Policy_Prem_Pay_ID + "]");
                        }
                        else
                        {
                            err_message = "[" + user_login + "] Save policy premium pay fail [policy premium pay id =" + pol_prem_pay.Policy_Prem_Pay_ID + "]";
                            goto EXIT;
                        }
                    }
                    #endregion

                    #region //Save Policy ID
                    if (da_policy.InsertPolicyID(policy.PolicyID, 1))
                    {
                        SaveSuccessLog("[" + user_login + "] 9. Save policy ID success [policy id =" + policy.PolicyID + "]");
                    }
                    else
                    {
                        err_message = "[" + user_login + "] Save policy ID fail [policy id =" + policy.PolicyID + "]";
                        goto EXIT;
                    }

                    #endregion

                    #region //Send message
                    if (ckbSendSMS.Checked)
                    {
                        #region Old send SMS
                        /*
                        System.Net.WebClient web = new System.Net.WebClient();
                           web.Headers.Add("cache-control", "no-cache");
                           web.Headers.Add("content-type", "application/x-www-form-urlencoded");
                           //This is to certify Camlife agreed to insure EANG CHANNTO under Cert. No.85592302122 from 14/09/18 23.59 Click www.camlife.com.kh/CI to print Certificate
                           string message = "This is to certify Camlife agreed to insure " + ci_data.ENLastName + " " + ci_data.ENFirstName + " under Cert. No." + ci_data.PolicyNumber + " from " + policy_detail.EffectiveDate.ToString("dd/MM/yy HH.mm") + " Click www.camlife.com.kh/CI to print certificate";
                           //int len = 0;
                           // len = message.Length;
                           string messageCate = Server.UrlEncode("SMS_CI");
                           string phone = "+855" + ci_data.PhoneNumber.Substring(1, ci_data.PhoneNumber.Length - 1);
                           phone = Server.UrlEncode(phone.Trim());
                           message = Server.UrlEncode(message.Trim());
                           string strResponse = "";
                           try
                           {
                               web.Encoding = System.Text.Encoding.UTF8;
                               strResponse = web.UploadString("http://192.168.1.9:12793/HttpPost.asmx/SendMessage", "phoneNumber=" + phone.Trim() + "&messageText=" + message + "&messageCate=" + messageCate);
                               //strResponse = web.UploadString("http://localhost:50334/CallCenter/HttpPost.asmx/SendMessage", "phoneNumber=" + phone.Trim() + "&messageText=" + message + "&messageCate=" + messageCate);

                           }
                           catch (Exception ex)
                           {
                               strResponse = "fail";
                               Log.AddExceptionToLog("Error [Send SMS in function SaveData] in page [frmCI.aspx.cs], detail:" + ex.Message);
                           }
                         */
                        #endregion Old Send SMS
                        #region New Send SMS
                        SENDSMS sms = new SENDSMS();
                        string message = "This is to certify Camlife agreed to insure " + ci_data.ENLastName + " " + ci_data.ENFirstName + " under Cert. No." + ci_data.PolicyNumber + " from " + policy_detail.EffectiveDate.ToString("dd/MM/yy HH.mm") + " Click www.camlife.com.kh/CI to print certificate";
                        sms.Message = message;
                        sms.MessageCate = "SMS_CI";
                        sms.PhoneNumber = ci_data.PhoneNumber;
                        #endregion New Send SMS


                        if (sms.Send())// (strResponse.Trim().ToLower() == "true")
                        {
                            SaveSuccessLog("[" + user_login + "] 10. message was sent to [" + ci_data.PhoneNumber + "] successfully.");
                        }
                        else
                        {
                            SaveSuccessLog("[" + user_login + "] 10. message was sent to [" + ci_data.PhoneNumber + "] fail.");
                        }
                    }
                    else
                    {
                        SaveSuccessLog("[" + user_login + "] 10. message was not sent to [" + ci_data.PhoneNumber + "]");
                    }

                    #endregion

                    #region //Save Approver Policy
                    da_report_approver.bl_report_approver_policy approver_policy;
                    int approver_id = 0;
                    approver_id = Int32.Parse(Session["APPROVER_ID"] + "");
                    if (approver_id != 0) // new policy approved
                    {
                        approver_policy = new da_report_approver.bl_report_approver_policy();
                        approver_policy.Approver_ID = approver_id;
                        approver_policy.Policy_ID = policy.PolicyID;
                        approver_policy.Created_On = DateTime.Now;
                        approver_policy.Created_By = user_login;
                        if (!da_report_approver.InsertApproverPolicy(approver_policy))
                        {
                            err_message = "[" + user_login + "] Save approver policy ID fail [policy id =" + policy.PolicyID + "]";
                            goto EXIT;
                        }


                    }

                    #endregion //Save Approver Policy
                }
                #endregion

                //reset variables value
                exist_cellcard_customer = "";
                exist_customer_id = "";
                exist_policy_number = "";
                time_to_pay = 0;
                return_premium = 0;
                isSaveAble = false;
                rollback_policy_id = "";
                rollback_policy_detail_id = "";
                rollback_policy_prem_pay_id = "";
                rollback_address_id = "";
                rollback_customer_id = "";
            EXIT:
                if (err_message != "")
                {
                    //Rollback data
                    Log.CreateLog("CI_Log", err_message);
                    if (da_ci.RollBack(rollback_policy_id, rollback_policy_detail_id, rollback_policy_prem_pay_id, rollback_address_id, rollback_customer_id))
                    {

                        Log.CreateLog("CI_Log", "System was rollback data successfully.");
                    }
                    else
                    {
                        Log.CreateLog("CI_Log", "System was rollback data fail.");
                    }
                }

            }

            #region//Display existing customer & Error
            //gv_invalid.DataSource = invalid_list;
            //gv_invalid.DataBind();
            LoadInvalidData();
            #endregion

            if (err_message == "")
            {
                if (invalid_list.Count > 0)
                {
                    AlertMessage("System upload data successfully, but some datas cannot be saved, please check in [Fail tab].");
                }
                else
                {

                    AlertMessage("System upload data successfully.");
                }

                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User saves simple one data uploading [Total success:", listData.Count, "; fail:", invalid_list.Count, "]."));
            }
            else
            {
                AlertMessage("System upload data fail.");
            }
        }
        catch (Exception ex)
        {
            AlertMessage("System upload data fail.");
            if (da_ci.RollBack(rollback_policy_id, rollback_policy_detail_id, rollback_policy_prem_pay_id, rollback_address_id, rollback_customer_id))
            {
                Log.CreateLog("CI_Log", "System was rollback data successfully.");
            }
            else
            {
                Log.CreateLog("CI_Log", "System was rollback data fail.");
            }
            Log.AddExceptionToLog("Error fucntion [save_data()] in class [frmCI.aspx.cs], detail:" + ex.Message);
        }

    }
    

    string GetPaymentCode(string phone_number)
    {
        string payment_code = "";
        if (phone_number != "")
        {
            payment_code = phone_number.Substring(3, phone_number.Length - 3);
            if (payment_code.Length == 8)
            {
                payment_code = "8880" + payment_code;
            }
            else if (payment_code.Length == 9)
            {
                payment_code = "888" + payment_code;
            }
        }
        return payment_code;
    }
    class InvalidData : bl_ci_load_data
    {
        public string Remarks { get; set; }
    }
    protected void export_excel_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }

    //export dato to excel
    void ExportExcel()
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            row_count = gv_invalid.Rows.Count;
            if (row_count > 0)
            {

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                Helper.excel.Sheet = sheet1;
                //design row header
                Helper.excel.HeaderText = new string[] {"No", "ID Type","ID", "Surname(En)", "Given Name(En)", "Surname(Kh)", "Given Name(Kh)", "Gender", "DOB", "Age",
                                                "Phone Number", "Province", "Product Name", "Sum Assured", "Premium Paid", "System Premium", "Payment Mode", 
                                                "Payment By", "Agent Code", "Remarks"};
                Helper.excel.generateHeader();
                //disign rows
                foreach (GridViewRow row in gv_invalid.Rows)
                {
                    #region //Variable
                    Label id_type = (Label)row.FindControl("lblTypeIDText");
                    Label id = (Label)row.FindControl("lblID");
                    Label surname_en = (Label)row.FindControl("lblEnlastName");
                    Label given_name_en = (Label)row.FindControl("lblEnFirstName");
                    Label surname_kh = (Label)row.FindControl("lblKhLastName");
                    Label given_name_kh = (Label)row.FindControl("lblKhFirstName");
                    Label gender = (Label)row.FindControl("lblGender");
                    Label dob = (Label)row.FindControl("lblDOB");
                    Label age = (Label)row.FindControl("Age");
                    Label phone_number = (Label)row.FindControl("lblPhoneNumber");
                    Label province = (Label)row.FindControl("lblProvince");
                    Label product_name = (Label)row.FindControl("lblProductName");
                    Label sum_assured = (Label)row.FindControl("lblSumAssured");
                    Label premium_paid = (Label)row.FindControl("lblPremiumPaid");
                    Label system_premium = (Label)row.FindControl("lblSystemPremium");
                    Label payment_mode = (Label)row.FindControl("lblPaymentModeText");
                    Label payment_by = (Label)row.FindControl("lblPaymentBy");
                    Label agent_code = (Label)row.FindControl("lblAgentCode");
                    Label remarks = (Label)row.FindControl("lblRemarks");
                    #endregion
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(id_type.Text);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(id.Text);

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(surname_en.Text);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(given_name_en.Text);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(surname_kh.Text);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(given_name_kh.Text);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(gender.Text);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(dob.Text);
                    HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                    style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy");
                    Cell9.CellStyle = style;

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(age.Text);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(phone_number.Text);

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(province.Text);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(product_name.Text);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(sum_assured.Text);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(premium_paid.Text);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(system_premium.Text);

                    HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                    Cell17.SetCellValue(payment_mode.Text);

                    HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                    Cell18.SetCellValue(payment_by.Text);

                    HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                    Cell19.SetCellValue(agent_code.Text);

                    HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                    Cell20.SetCellValue(remarks.Text);

                }
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports invalid simple one data uploading [Total records:", row_count, "]."));

                string filename = "CI_Records_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                MemoryStream file = new MemoryStream();
                hssfworkbook.Write(file);

                Response.BinaryWrite(file.GetBuffer());

                Response.End();
            }
        }
        catch (Exception ex)
        {
            AlertMessage("Export data fail, please contact your system administrator.");
            Log.AddExceptionToLog("Error function [ExportExcel()] in page [frmCI.aspx.cs], detail:" + ex.Message);
        }
    }

    void EnableControlls(bool t = true)
        {
            txtEffectiveDate.Enabled = t;
            flUpload.Enabled = t;
            ddlApprover.Enabled = t;
            ckbSendSMS.Enabled = t;
    }

}
