using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Web.Security;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;

public partial class Pages_Business_Wing_import_data : System.Web.UI.Page
{
    string user_name = "";
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        user_name = Membership.GetUser().UserName;
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (!Page.IsPostBack)
        {
            /*set default effective date in text box*/

            export_excel.Visible = false;
        }
    }
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        Save();
    }
    protected void ibtnClear_Click(object sender, ImageClickEventArgs e)
    {
        Session["WING_DATA_UPLOAD"] = null;
        gv_invalid.DataSource = null;
        gv_invalid.DataBind();
        gv_valid.DataSource = null;
        gv_valid.DataBind();
        export_excel.Visible = false;
    }
    protected void btnLoadData_Click(object sender, EventArgs e)
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
                List<bl_wing.DataUpload> listUploadData, invalidList;
                if (Validate(out message, out listUploadData, out invalidList) == true)
                {
                    gv_valid.DataSource = listUploadData;
                    gv_valid.DataBind();

                    gv_invalid.DataSource = invalidList;
                    gv_invalid.DataBind();

                    Session["WING_DATA_UPLOAD"] = listUploadData;

                    export_excel.Visible = invalidList.Count > 0 ? true : false;

                    if (listUploadData.Count > 0)
                    {
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User uploads temporary new wing account [Total record(s):", listUploadData.Count, "]."));
                    }

                    if (listUploadData.Count > 0 && invalidList.Count == 0)
                    {
                        AlertMessage("Data were imported successfuly.");
                    }
                    else if (listUploadData.Count == 0 && invalidList.Count > 0)
                    {
                        AlertMessage("Data were imported fail, view detail in [Fail] tab.");
                    }
                    else
                    {
                        AlertMessage("Few data cannot import, view detail in [Success] and [Fail] tab.");
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
            Log.AddExceptionToLog("Error function [btnLoadData_Click(object sender, EventArgs e)] in class [import_data.aspx.cs], detail:"+ ex.Message + "=>"+ ex.StackTrace);
            AlertMessage(ex.Message);
        }

    }
    protected void export_excel_Click(object sender, EventArgs e)
    {
        Excel();
    }

    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }

    bool Validate(out string message, out List<bl_wing.DataUpload> listUploadData, out List<bl_wing.DataUpload> invalidList)
    {
        bool status = true;
        message = "";
        listUploadData = new List<bl_wing.DataUpload>();
        invalidList = new List<bl_wing.DataUpload>();
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

                if (my_excel.GetSheetName() != "Upload_Format$")
                {
                    message = "File is not correct format, please donwload file template from the system.";
                    //save log
                    UploadTransactionLog("[Validate Execl File] \t [Sheet Name:" + my_excel.GetSheetName().ToString() + "] is not valid.");
                    status = false;
                }
                else
                {
                    my_excel.CommandText = "Select * from [Upload_Format$]";
                    DataTable tbl = my_excel.GetData();
                    int col_count = 0;
                    col_count = tbl.Columns.Count;
                    if (col_count > 21 || col_count < 0)//check number of columns
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        //save log
                        UploadTransactionLog("[Validate Execl File] \t [Number of columns] must be equal 20.");
                        status = false;
                    }
                    else
                    {
                        #region //check column name.

                        if (tbl.Columns[0].ColumnName.Trim() != "No#")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[0].ColumnName.Trim() + "] is not valid, it must be [No.].");
                        }
                        else if (tbl.Columns[1].ColumnName.Trim() != "ID_Type")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[1].ColumnName.Trim() + "] is not valid, it must be [ID_Type].");

                        }
                        else if (tbl.Columns[2].ColumnName.Trim() != "ID")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[2].ColumnName.Trim() + "] is not valid, it must be [ID].");

                        }
                        else if (tbl.Columns[3].ColumnName.Trim() != "Surname(En)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[3].ColumnName.Trim() + "] is not valid, it must be [Surname(En)].");

                        }
                        else if (tbl.Columns[4].ColumnName.Trim() != "Given Name(En)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[4].ColumnName.Trim() + "] is not valid, it must be [Given Name(En)].");

                        }
                        else if (tbl.Columns[5].ColumnName.Trim() != "Surname(Kh)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[5].ColumnName.Trim() + "] is not valid, it must be [Surname(Kh)].");

                        }
                        else if (tbl.Columns[6].ColumnName.Trim() != "Given Name(Kh)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[6].ColumnName.Trim() + "] is not valid, it must be [Given Name(Kh)].");

                        }
                        else if (tbl.Columns[7].ColumnName.Trim() != "Gender(M,F)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[7].ColumnName.Trim() + "] is not valid, it must be [Gender(M,F)].");

                        }
                        else if (tbl.Columns[8].ColumnName.Trim() != "DOB(DD-MM-YYYY)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[8].ColumnName.Trim() + "] is not valid, it must be [DOB(DD-MM-YYYY)].");

                        }
                        else if (tbl.Columns[9].ColumnName.Trim() != "Phone Number(012 XXX XXX)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[9].ColumnName.Trim() + "] is not valid, it must be [Phone Number(012 XXX XXX)].");

                        }
                        else if (tbl.Columns[10].ColumnName.Trim() != "Province")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[10].ColumnName.Trim() + "] is not valid, it must be [Province].");

                        }
                        else if (tbl.Columns[11].ColumnName.Trim() != "Product Name")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[11].ColumnName.Trim() + "] is not valid, it must be [Product Name].");

                        }
                        else if (tbl.Columns[12].ColumnName.Trim() != "Sum Assured(USD)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[12].ColumnName.Trim() + "] is not valid, it must be [Sum Assured(USD)].");

                        }
                        else if (tbl.Columns[13].ColumnName.Trim() != "Premium Paid(USD)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[13].ColumnName.Trim() + "] is not valid, it must be [Premium Paid(USD)].");

                        }
                        else if (tbl.Columns[14].ColumnName.Trim() != "Mode Of Payment")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[14].ColumnName.Trim() + "] is not valid, it must be [Mode Of Payment].");

                        }
                        else if (tbl.Columns[15].ColumnName.Trim() != "Payment By")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[15].ColumnName.Trim() + "] is not valid, it must be [Payment By].");

                        }

                        else if (tbl.Columns[16].ColumnName.Trim() != "Wing Account No#")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[16].ColumnName.Trim() + "] is not valid, it must be [Wing Account No.].");

                        }
                        else if (tbl.Columns[17].ColumnName.Trim() != "Consent Number")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[17].ColumnName.Trim() + "] is not valid, it must be [Consent Number].");

                        }
                        else if (tbl.Columns[18].ColumnName.Trim() != "Factor/Company Name")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[18].ColumnName.Trim() + "] is not valid, it must be [Factor/Company Name].");

                        }
                        else if (tbl.Columns[19].ColumnName.Trim() != "Agent Code")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[19].ColumnName.Trim() + "] is not valid, it must be [Agent Code].");

                        }
                        else if (tbl.Columns[20].ColumnName.Trim() != "Remarks")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[20].ColumnName.Trim() + "] is not valid, it must be [Remarks].");

                        }
                        #endregion
                    }

                    if (status == false)
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        listUploadData = new List<bl_wing.DataUpload>();
                    }
                    else
                    {
                        #region Get upload data from excel file
                        DataTable data_excel = my_excel.GetData();
                        bl_wing.DataUpload wing;
                        int row_index = -1;
                        bl_wing.DataUpload invalidData;
                        foreach (DataRow row in data_excel.Rows)
                        {
                            row_index = data_excel.Rows.IndexOf(row);
                            if (validRow(row, row_index, out invalidData))
                            {
                                wing = new bl_wing.DataUpload();
                                wing.IDTypeText = row[1].ToString().Trim();
                                wing.IDType = Helper.GetIDCardTypeID(wing.IDTypeText);
                                wing.ID = row[2].ToString().Trim();
                                wing.ENLastName = row[3].ToString().Trim();
                                wing.ENFirstName = row[4].ToString().Trim();
                                wing.KHLastName = row[5].ToString().Trim();
                                wing.KHFirstName = row[6].ToString().Trim();
                                wing.Gender = row[7].ToString().Trim();
                                wing.DOB = row[8].ToString().Trim();
                                wing.PhoneNumber = row[9].ToString().Trim().Replace(" ", "");
                                wing.Province = row[10].ToString().Trim();
                                wing.ProductName = row[11].ToString().Trim();
                                wing.SA = row[12].ToString().Trim();
                                wing.UserPremium = row[13].ToString().Trim();
                                wing.PayModeText = row[14].ToString().Trim();
                                wing.PayMode = Helper.GetPayModeID(wing.PayModeText) + "";
                                wing.PaymentBy = row[15].ToString().Trim();
                                wing.WingAccNumber = row[16].ToString().Trim();
                                wing.ConsentNumber = row[17].ToString().Trim();

                                wing.FactoryName = row[18].ToString().Trim();
                                wing.Agent_code = row[19].ToString().Trim();
                                wing.Remarks = row[20].ToString().Trim() == "" ? "Processing consent form" : row[20].ToString().Trim();
                                wing.CountryCode = "KH";
                                wing.PolicyNumber = wing.WingAccNumber;
                                wing.EffectiveDate = DateTime.Now.Date;
                                wing.Age = Calculation.Culculate_Customer_Age(wing.DOB, DateTime.Now.Date) + "";

                                double[] prem = new double[] { 0, 0 };
                                prem = da_wing.GetPremium(wing.ProductName, Convert.ToInt32(wing.SA), Convert.ToInt32(wing.PayMode));
                                wing.SystemPremium = prem[0].ToString();
                                wing.OriginalPremium = prem[1].ToString();

                                wing.PaymentCode = wing.WingAccNumber;

                                listUploadData.Add(wing);
                            }
                            else
                            {
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
            listUploadData = new List<bl_wing.DataUpload>();
            status = false;
            message = "Oooop! something is going wrong, please contact your system administrator.";
            UploadTransactionLog("[Validate excel file] \t [Error]:please check log file for detail.");
            Log.AddExceptionToLog("Error function [Validate(out string message, List<bl_wing.DataUpload> listUploadData)] in page [import_data.aspx], Detail:" + ex.Message);
        }
        return status;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    /// <param name="row_index"></param>
    /// <param name="invalidData">[Return bl_wing_DataUpload]</param>
    /// <returns></returns>
    bool validRow(DataRow row, int row_index, out bl_wing.DataUpload invalidData)
    {
        bool valid = true;
        invalidData = new bl_wing.DataUpload();

        int customer_age = 0;
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
                invalidData.Remarks = "Invalid ID type.";

            }
            else if (row[2].ToString().Trim() == "" || row[2].ToString() == null)//ID is emqty
            {
                valid = false;
                invalidData.Remarks = "ID is required.";
            }
            else if (row[3].ToString().Trim() == "")//Sur Name is emqty
            {
                valid = false;
                invalidData.Remarks = "Surname (EN) is required.";
            }
            else if (row[4].ToString().Trim() == "")//Given Name is emqty
            {
                valid = false;
                invalidData.Remarks = "Given name (EN) is required.";
            }
            else if (row[7].ToString().Trim() == "")//Gender
            {
                valid = false;
                invalidData.Remarks = "Gender is required.";
            }
            else if (row[8].ToString().Trim() == "")//DOB
            {
                valid = false;
                invalidData.Remarks = "DOB is required.";
            }

            else if (Helper.IsDate(row[8].ToString().Trim()) == false)//check dob date format
            {
                valid = false;
                invalidData.Remarks = "Invalid DOB";
            }

            else if (Helper.IsDate(row[8].ToString().Trim()) == true)
            {
                if (customer_age < product.Age_Min || customer_age > product.Age_Max)
                {
                    valid = false;
                    invalidData.Remarks = "Age=" + customer_age + " is not in range [" + product.Age_Min + "-" + product.Age_Max + "]";

                }
            }

            //check age range

            if (row[9].ToString().Trim() == "")//Phone Number
            {
                valid = false;
                invalidData.Remarks = "Phone number is required.";
            }
            else if (row[9].ToString().Replace(" ", "").Trim().Length < 9 || row[9].ToString().Replace(" ", "").Trim().Length > 10)//phone length
            {
                valid = false;
                invalidData.Remarks = "Phone number is invalid length.";
            }
            else if (row[10].ToString().Trim() == "")//Province
            {
                valid = false;
                invalidData.Remarks = "Province is required.";
            }
            else if (row[12].ToString().Trim() == "")//Sum Assured
            {
                valid = false;
                invalidData.Remarks = "Sum ansured is required.";
            }
            else if (Helper.IsNumeric(row[12].ToString().Trim()) == false)//Check sum assured is number or not
            {
                valid = false;
                invalidData.Remarks = "Sum ansured is invalid format.";
            }

            else if (Helper.IsNumeric(row[13].ToString().Trim()) == false)//check Premium paid is number or not
            {
                valid = false;
                invalidData.Remarks = "Invalid premium paid.";
            }

            else if (Helper.GetPayModeID(row[14].ToString().Trim()) < 0)//Payment Mode
            {
                valid = false;
                invalidData.Remarks = "Mode of payment is required.";
            }
            
            else if (row[15].ToString().Trim() == "")//Payment by
            {
                valid = false;
                invalidData.Remarks = "Payment by is required.";
            }
            else if (row[16].ToString().Trim() == "")//Payment by
            {
                valid = false;
                invalidData.Remarks = "Wing account No. is required.";
            }
            else if (row[17].ToString().Trim() == "")//Payment by
            {
                valid = false;
                invalidData.Remarks = "Consent No. is required.";
            }
            else if (row[18].ToString().Trim() == "")//Payment by
            {
                valid = false;
                invalidData.Remarks = "Company/Factory is required.";
            }
            else if (row[19].ToString().Trim() == "")//Agent Code
            {
                valid = false;
                invalidData.Remarks = "Agent code is required.";
            }

            #region //Check premium amount

            double[] premium = new double[] { 0, 0 };
            if (Helper.IsNumeric(row[13].ToString().Trim()) == true && row[12].ToString().Trim() != "" && Helper.GetPayModeID(row[14].ToString().Trim()) > 0)
            {
                premium = da_wing.GetPremium(row[11].ToString().Trim(), Convert.ToInt32(row[12].ToString().Trim()), Helper.GetPayModeID(row[14].ToString().Trim()));

                if (premium[0]<=0)
                {
                    valid = false;
                    invalidData.Remarks = "Premium is required.";
                }

            }
            #endregion
            //store invalid data
            if (!valid)
            {
                invalidData.IDTypeText = row[1].ToString().Trim();
                invalidData.IDType = Helper.GetIDCardTypeID(invalidData.IDTypeText);
                invalidData.ID = row[2].ToString();
                invalidData.ENFirstName = row[4].ToString();
                invalidData.ENLastName = row[3].ToString();
                invalidData.KHFirstName = row[6].ToString();
                invalidData.KHLastName = row[5].ToString();
                invalidData.Gender = row[7].ToString();
                invalidData.DOB = row[8].ToString().Trim();
                invalidData.PhoneNumber = row[9].ToString().Trim();
                invalidData.PolicyNumber = "855" + invalidData.PhoneNumber.Substring(1, invalidData.PhoneNumber.Length - 1);//Make up phone number format
                invalidData.Age = Calculation.Culculate_Customer_Age(invalidData.DOB, DateTime.Now) + "";
                invalidData.CountryCode = "KH";
                invalidData.Province = row[10].ToString().Trim();
                invalidData.ProductName = row[11].ToString().Trim();
                invalidData.SA = row[12].ToString().Trim();
                invalidData.UserPremium = row[13].ToString().Trim();
                invalidData.PayModeText = row[14].ToString().Trim();
                invalidData.PayMode = Helper.GetPayModeID(invalidData.PayModeText) + "";
                invalidData.SystemPremium = premium[0] + "";
                invalidData.OriginalPremium = premium[1] + "";
                invalidData.PaymentBy = row[15].ToString().Trim();
                invalidData.WingAccNumber = row[16].ToString().Trim();
                invalidData.ConsentNumber = row[17].ToString().Trim();
                invalidData.FactoryName = row[18].ToString().Trim();
                invalidData.Agent_code = row[19].ToString();
                invalidData.PaymentCode = invalidData.WingAccNumber;

            }

            #endregion
        }
        catch (Exception ex)
        {
            valid = false;
            Log.AddExceptionToLog("Error function [validRow(DataRow row)] in class [import_data.aspx.cs], row index [" + row_index + "], detail:" + ex.Message);
        }
        return valid;
    }

    void Save()
    {
        if (Session["WING_DATA_UPLOAD"] != null)
        {

            List<bl_wing.DataUpload> savedList = new List<bl_wing.DataUpload>();
            List<bl_wing.DataUpload> invalidList = new List<bl_wing.DataUpload>();
            List<bl_wing.DataUpload> wingList = (List<bl_wing.DataUpload>)Session["WING_DATA_UPLOAD"];
            bool isSaveWing = false;
            bool isSavePolicyDetail = false;
            bool isSaveCustomer = false;
           
            bool isSaveContact = false;
            bool isSaveAble = true;
            string address_id = "";
            string policy_id = "";
            string existing_policy = "";
            string customer_id = "";
            string exist_customer_id = "";
            bl_ci.Policy exist_policy_obj;
            string policy_status = "";
            string rollback_customer_id = "";
            string rollback_policy_id = "";
            string rollback_policy_detail_id = "";
            string rollback_address_id = "";
            string rollback_policy_prem_pay_id = "";
            string err_message = "";
            DateTime transaction_date = DateTime.Now;
            bl_wing.PolicyDetail pol_detail;

            #region //Loop
            foreach (bl_wing.DataUpload wing in wingList)
            {
                try
                {
                    #region Check existing policy Number
                    exist_policy_obj = new bl_ci.Policy();
                    exist_policy_obj = da_ci.Policy.GetPolicyByPolicyNumber(wing.PolicyNumber);
                    if (exist_policy_obj.CustomerID != null)
                    {
                        existing_policy = exist_policy_obj.PolicyID;
                    }
                    #endregion
                    exist_customer_id = da_customer.GetCustomerIDByParameters(wing.ENFirstName.Trim(), wing.ENLastName.Trim(), Helper.GetGender(wing.Gender), Helper.FormatDateTime(wing.DOB));
                    if (existing_policy == "")//New policy
                    {
                        #region New Policy
                        #region check exist customer already bought policy or not

                        exist_policy_obj = new bl_ci.Policy();
                        exist_policy_obj = da_ci.Policy.GetPolicyByCustomerID(exist_customer_id);
                        if (exist_policy_obj.CustomerID != null && exist_policy_obj.CustomerID != "")
                        {
                            exist_customer_id = exist_policy_obj.CustomerID;
                            DataTable tbl_ci_detail = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), "", exist_policy_obj.PolicyNumber, wing.ProductName);
                            if (tbl_ci_detail.Rows.Count > 0)
                            {
                                //filter last effective date
                                DataRow[] row = tbl_ci_detail.Select("effective_date = max(effective_date)");

                                policy_status = row[0]["policy_status"].ToString().Trim();//get last policy status
                            }
                        }

                        if (exist_customer_id != "")
                        {
                            #region Exist Customer
                            if (policy_status.ToUpper() != "TER" && policy_status.ToUpper() != "")
                            {
                                //
                                isSaveAble = false;
                                invalidList.Add(new bl_wing.DataUpload()
                                {
                                    PolicyNumber = wing.PolicyNumber,
                                    ENFirstName = wing.ENFirstName,
                                    ENLastName = wing.ENLastName,
                                    KHFirstName = wing.KHFirstName,
                                    KHLastName = wing.KHLastName,
                                    Age=wing.Age,
                                    Gender = wing.Gender,
                                    DOB = wing.DOB,
                                    IDType = wing.IDType,
                                    IDTypeText = wing.IDTypeText,
                                    ID = wing.ID,
                                    WingAccNumber = wing.WingAccNumber,
                                    ConsentNumber = wing.ConsentNumber,
                                    FactoryName = wing.FactoryName,
                                    SA = wing.SA,
                                    UserPremium = wing.UserPremium,
                                    SystemPremium = wing.SystemPremium,
                                    OriginalPremium = wing.OriginalPremium,
                                    Agent_code = wing.Agent_code,
                                    PaymentBy = wing.PaymentBy,
                                    PayMode = wing.PayMode,
                                    PayModeText = wing.PayModeText,
                                    Remarks = "Customer is already exist with policy [" + exist_policy_obj.PolicyNumber + "] status [" + policy_status + "]"
                                });

                            }
                            else
                            {
                                isSaveAble = true;

                            }
                            customer_id = exist_customer_id;
                            rollback_customer_id = ""; //// prevent to delete existing customer incase system get error and rollback data.
                            #endregion //Exist Customer

                        }
                        else
                        {
                            #region //New customer
                            customer_id = da_customer.GetCustomerID();//generate new customer id
                            rollback_customer_id = customer_id;
                            isSaveAble = true;
                            #endregion //New customer
                        }
                        #endregion //check exist customer already bought policy or not
                        #endregion //New Policy
                    }
                    else
                    {
                        #region //Policy is exist
                        //check last policy status
                        DataTable tbl_ci_detail = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), "", exist_policy_obj.PolicyNumber, wing.ProductName);
                        if (tbl_ci_detail.Rows.Count > 0)
                        {
                            //filter last effective date
                            DataRow[] row = tbl_ci_detail.Select("effective_date = max(effective_date)");

                            policy_status = row[0]["policy_status"].ToString().Trim();//get last policy status
                            if (policy_status.ToUpper() != "TER" && policy_status.ToUpper() != "")
                            {
                                //
                                isSaveAble = false;
                                invalidList.Add(new bl_wing.DataUpload()
                                {
                                    PolicyNumber = wing.PolicyNumber,
                                    ENFirstName = wing.ENFirstName,
                                    ENLastName = wing.ENLastName,
                                    KHFirstName = wing.KHFirstName,
                                    KHLastName = wing.KHLastName,
                                    Gender = wing.Gender,
                                    PhoneNumber = wing.PhoneNumber,
                                    Province = wing.Province,
                                    ProductName = wing.ProductName,
                                    DOB = wing.DOB,
                                    IDType = wing.IDType,
                                    IDTypeText = wing.IDTypeText,
                                    ID = wing.ID,
                                    WingAccNumber = wing.WingAccNumber,
                                    ConsentNumber = wing.ConsentNumber,
                                    FactoryName = wing.FactoryName,
                                    SA = wing.SA,
                                    UserPremium = wing.UserPremium,
                                    SystemPremium = wing.SystemPremium,
                                    OriginalPremium = wing.OriginalPremium,
                                    Agent_code = wing.Agent_code,
                                    PaymentBy = wing.PaymentBy,
                                    PayMode = wing.PayMode,
                                    PayModeText = wing.PayModeText,
                                    Remarks = "Existing policy No.[" + exist_policy_obj.PolicyNumber + "]"
                                });
                            }
                            else
                            {
                                isSaveAble = true;
                                customer_id = exist_policy_obj.CustomerID;
                                rollback_customer_id = ""; //// prevent to delete existing customer incase system get error and rollback data.
                            }
                        }

                        #endregion
                    }

                    #region Save Data into database
                    // generate address & customer id
                    address_id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_POLICY_ADDRESS" }, { "FIELD", "ADDRESS_ID" } }); ;
                    policy_id = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CI_POLICY" }, { "FIELD", "POLICY_ID" } });

                    if (isSaveAble)
                    {
                        #region //Save Address
                        bl_policy_address address = new bl_policy_address();
                        address.Address_ID = address_id;
                        address.Address1 = "";
                        address.Address2 = "";
                        address.Address3 = "";
                        address.Province = wing.Province;
                        address.Zip_Code = "855";
                        address.Country_ID = wing.CountryCode;
                        rollback_address_id = address.Address_ID;//use for rollback data

                        if (da_policy.InsertPolicyAddress(address))
                        {
                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 1. Save address success [address id =" + address.Address_ID + "]");
                        }
                        else
                        {
                            err_message = "[SAVE]\t[" + user_name + "] \t 1. Save address fail [address id =" + address.Address_ID + "]";
                            goto EXIT;
                        }

                        #endregion
                        #region Save data in table ct_ci_policy
                        isSaveWing = da_wing.Policy.SavePolicy(new bl_wing.policy()
                        {
                            PolicyID = policy_id,
                            PolicyNumber = wing.PolicyNumber,
                            CustomerID = customer_id,
                            ProductID = wing.ProductName,
                            AddressID = address_id,
                            ConsentNumber = wing.ConsentNumber,
                            ConsentForm = wing.ConsentNumber + ".pdf",
                            Categories = "WING",
                            FactoryName = wing.FactoryName,
                            Remarks = wing.Remarks,
                            AgentCode = wing.Agent_code,
                            CreatedBy = user_name,
                            CreatedDateTime = transaction_date
                        });
                        rollback_policy_id = policy_id;

                        if (isSaveWing)
                        {
                            

                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 2. Save policy success [policy id =" + policy_id + "]");
                        }
                        else
                        {
                            err_message = "[SAVE]\t[" + user_name + "] \t 2. Save policy fail [policy id =" + policy_id + "]";
                            goto EXIT;
                        }
                        #endregion
                        #region Save policy detail
                        TimeSpan time = new TimeSpan(23, 59, 00);//time :11:59:00 PM
                        pol_detail = new bl_wing.PolicyDetail();
                        pol_detail.PolicyDetailID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_CI_POLICY_DETAIL" }, { "FIELD", "POLICY_DETAIL_ID" } });
                        pol_detail.PolicyID = policy_id;
                        pol_detail.EffectiveDate = wing.EffectiveDate;
                        pol_detail.MaturityDate = pol_detail.EffectiveDate.AddMonths(1);
                        pol_detail.ExpiryDate = pol_detail.MaturityDate.Date.AddDays(-1) + time;
                        pol_detail.IssuedDate = transaction_date;
                        pol_detail.Age = Convert.ToInt32(wing.Age);
                        pol_detail.SumAssured = Convert.ToDouble(wing.SA);
                        pol_detail.PayModeID = Convert.ToInt32(wing.PayMode);
                        pol_detail.PaymentBy = wing.PaymentBy;
                        pol_detail.PaymentCode = wing.PaymentCode;
                        pol_detail.UserPremium = Convert.ToDouble(wing.UserPremium);
                        pol_detail.Premium = Convert.ToDouble(wing.SystemPremium);
                        pol_detail.RETURN_PREMIUM = (pol_detail.UserPremium - pol_detail.Premium);
                        pol_detail.OriginalPremium = Convert.ToDouble(wing.OriginalPremium);
                        pol_detail.DiscountAmount = Convert.ToDouble(wing.DiscountAmount);
                        pol_detail.CoverYear = 0;
                        pol_detail.PayYear = 0;
                        pol_detail.CoverUpToAge = 0;
                        pol_detail.PayUpToAge = 0;
                        pol_detail.PolicyStatusRemarks = "PEN";
                        pol_detail.TransactionID = "";
                        pol_detail.CreatedBy = user_name;
                        pol_detail.CreatedDateTime = transaction_date;
                        pol_detail.Sequence = da_wing.GetLastSEQ(wing.PolicyNumber);
                        rollback_policy_detail_id = pol_detail.PolicyDetailID;

                        isSavePolicyDetail = da_wing.PolicyDetail.SavePolicyDetail(pol_detail);
                        if (isSavePolicyDetail)
                        {
                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 3. Save policy detail success [policy detail id =" + pol_detail.PolicyDetailID + "]");
                        }
                        else
                        {
                            err_message = "[SAVE]\t[" + user_name + "] \t 3. Save policy detail fail [policy detail id =" + pol_detail.PolicyDetailID + "]";
                            goto EXIT;
                        }
                        #endregion
                        #region Save New customer
                        if (exist_customer_id == "")
                        {
                            bl_customer customer = new bl_customer();
                            customer.Customer_ID = customer_id;
                            customer.ID_Type = wing.IDType;
                            customer.ID_Card = wing.ID;
                            customer.First_Name = wing.ENFirstName;
                            customer.Last_Name = wing.ENLastName;
                            customer.Khmer_First_Name = wing.KHFirstName;
                            customer.Khmer_Last_Name = wing.KHLastName;
                            customer.Prior_First_Name = "";
                            customer.Prior_Last_Name = "";
                            customer.Gender = Helper.GetGender(wing.Gender);
                            customer.Birth_Date = Helper.FormatDateTime(wing.DOB);
                            customer.Country_ID = wing.CountryCode;
                            customer.Father_First_Name = "";
                            customer.Father_Last_Name = "";
                            customer.Mother_First_Name = "";
                            customer.Mother_Last_Name = "";
                            customer.Created_By = user_name;
                            customer.Created_On = transaction_date;
                            rollback_customer_id = customer_id;//use for rollback data
                            isSaveCustomer = da_customer.InsertCustomer(customer, user_name);
                            if (isSaveCustomer)
                            {
                                UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 4. Save customer success [customer id =" + customer_id + "]");

                            }
                            else
                            {
                                err_message = "[SAVE]\t[" + user_name + "] \t 4. Save customer fail [customer id =" + customer_id + "]";
                                goto EXIT;
                            }
                        }
                        else
                        {
                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 4. Not saved exist customer [customer id =" + customer_id + "]");

                        }
                        #endregion
                        #region //Save Policy Contact
                        bl_app_info_contact contact = new bl_app_info_contact();
                        contact.PolicyID = policy_id;
                        contact.Mobile_Phone1 = wing.PhoneNumber;
                        contact.Mobile_Phone2 = "";
                        contact.Home_Phone1 = "";
                        contact.Home_Phone2 = "";
                        contact.Office_Phone1 = "";
                        contact.Office_Phone2 = "";
                        contact.Fax1 = "";
                        contact.Fax2 = "";
                        contact.EMail = "";
                        isSaveContact = da_policy.InsertPolicyContact(contact);

                        if (isSaveContact)
                        {
                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 5. Saved contact success [policy id =" + policy_id + "]");
                        }
                        else
                        {
                            err_message = "[SAVE]\t[" + user_name + "] \t 5. Saved contact fail [policy id =" + policy_id + "]";
                            goto EXIT;
                        }
                        #endregion

                        #region //Save Policy Status

                        if (da_policy.InsertPolicyStatus(policy_id, "PEN", user_name, transaction_date))
                        {
                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 6. Saved policy status success [policy id =" + policy_id + "]");
                        }
                        else
                        {
                            err_message = "[SAVE]\t[" + user_name + "] \t 6. Saved policy status fail [policy id =" + policy_id + "]";
                            goto EXIT;
                        }
                        #endregion
                        #region //Save Policy Pay Mode
                        if (da_policy.InsertPolicyPayMode(policy_id, Convert.ToInt32(wing.PayMode), transaction_date, user_name, transaction_date))
                        {
                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 7. Saved policy pay mode success [policy id =" + policy_id + "]");
                        }
                        else
                        {
                            err_message = "[SAVE]\t[" + user_name + "] \t 7. Saved policy pay mode fail [policy id =" + policy_id + "]";
                            goto EXIT;
                        }
                        #endregion
                        #region //Save Policy ID
                        if (da_policy.InsertPolicyID(policy_id,1))
                        {
                            UploadTransactionLog("[SAVE]\t[" + user_name + "] \t 8. Saved policy id success [policy id =" + policy_id + "]");
                        }
                        else
                        {
                            err_message = "[SAVE]\t[" + user_name + "] \t 8. Saved policy id fail [policy id =" + policy_id + "]";
                            goto EXIT;
                        }
                        #endregion
                        //Store saved success data
                        savedList.Add(new bl_wing.DataUpload()
                        {
                            PolicyNumber = wing.PolicyNumber,
                            ENFirstName = wing.ENFirstName,
                            ENLastName = wing.ENLastName,
                            KHFirstName = wing.KHFirstName,
                            KHLastName = wing.KHLastName,
                            Gender = wing.Gender,
                            DOB = wing.DOB,
                            IDType = wing.IDType,
                            IDTypeText = wing.IDTypeText,
                            ID = wing.ID,
                            WingAccNumber = wing.WingAccNumber,
                            ConsentNumber = wing.ConsentNumber,
                            FactoryName = wing.FactoryName,
                            SA = wing.SA,
                            UserPremium = wing.UserPremium,
                            SystemPremium = wing.SystemPremium,
                            OriginalPremium = wing.OriginalPremium,
                            Agent_code = wing.Agent_code,
                            PaymentBy = wing.PaymentBy,
                            PayMode = wing.PayMode,
                            PayModeText = wing.PayModeText,
                            Remarks = "success",
                            Age=wing.Age,
                            PhoneNumber =wing.PhoneNumber,
                            ProductName=wing.ProductName,
                            Province=wing.Province
                        });
                    }
                    else
                    {
                        // CANNOT BE SAVED 
                    }
                    #endregion
                    #region //Label Exit
                EXIT:
                    if (err_message != "")
                    {
                        
                        //Rollback data

                        UploadTransactionLog(err_message);
                        if (da_ci.RollBack(rollback_policy_id, rollback_policy_detail_id, rollback_policy_prem_pay_id, rollback_address_id, rollback_customer_id))
                        {
                            UploadTransactionLog("[RollBack] \t [policy number:"+ wing.PolicyNumber +"] \t System was rollback data successfully.");
                        }
                        else
                        {
                            UploadTransactionLog("[RollBack] \t [policy number:" + wing.PolicyNumber + "] \t System was rollback data fail.");
                        }

                        invalidList.Add(new bl_wing.DataUpload()
                        {
                            PolicyNumber = wing.PolicyNumber,
                            ENFirstName = wing.ENFirstName,
                            ENLastName = wing.ENLastName,
                            KHFirstName = wing.KHFirstName,
                            KHLastName = wing.KHLastName,
                            Gender = wing.Gender,
                            PhoneNumber = wing.PhoneNumber,
                            Province = wing.Province,
                            ProductName = wing.ProductName,
                            DOB = wing.DOB,
                            IDType = wing.IDType,
                            IDTypeText = wing.IDTypeText,
                            ID = wing.ID,
                            WingAccNumber = wing.WingAccNumber,
                            ConsentNumber = wing.ConsentNumber,
                            FactoryName = wing.FactoryName,
                            SA = wing.SA,
                            UserPremium = wing.UserPremium,
                            SystemPremium = wing.SystemPremium,
                            OriginalPremium = wing.OriginalPremium,
                            Agent_code = wing.Agent_code,
                            PaymentBy = wing.PaymentBy,
                            PayMode = wing.PayMode,
                            PayModeText = wing.PayModeText,
                            Remarks = "fail"
                        });
                    }
                   
                    #endregion //End Label Exit
                }
                catch (Exception ex)
                {

                    Log.AddExceptionToLog("Error function [Save()] in page [import_data.aspx.cs], detail: " + ex.Message);
                    if (da_ci.RollBack(rollback_policy_id, rollback_policy_detail_id, rollback_policy_prem_pay_id, rollback_address_id, rollback_customer_id))
                    {
                        UploadTransactionLog("[RollBack] \t [policy number:" + wing.PolicyNumber + "] \t System was rollback data successfully.");
                    }
                    else
                    {
                        UploadTransactionLog("[RollBack] \t [policy number:" + wing.PolicyNumber + "] \t System was rollback data fail.");
                    }
                    invalidList.Add(new bl_wing.DataUpload()
                    {
                        PolicyNumber = wing.PolicyNumber,
                        ENFirstName = wing.ENFirstName,
                        ENLastName = wing.ENLastName,
                        KHFirstName = wing.KHFirstName,
                        KHLastName = wing.KHLastName,
                        Gender = wing.Gender,
                        PhoneNumber = wing.PhoneNumber,
                        Province = wing.Province,
                        ProductName = wing.ProductName,
                        DOB = wing.DOB,
                        IDType = wing.IDType,
                        IDTypeText = wing.IDTypeText,
                        ID = wing.ID,
                        WingAccNumber = wing.WingAccNumber,
                        ConsentNumber = wing.ConsentNumber,
                        FactoryName = wing.FactoryName,
                        SA = wing.SA,
                        UserPremium = wing.UserPremium,
                        SystemPremium = wing.SystemPremium,
                        OriginalPremium = wing.OriginalPremium,
                        Agent_code = wing.Agent_code,
                        PaymentBy = wing.PaymentBy,
                        PayMode = wing.PayMode,
                        PayModeText = wing.PayModeText,
                        Remarks = "error"
                    });
                }
                //reset variable value
                exist_customer_id = "";
                existing_policy = "";
            }
#endregion //End loop

            //Bind saved success data
            gv_valid.DataSource = savedList;
            gv_valid.DataBind();

            //Bind invalid data
            gv_invalid.DataSource = invalidList;
            gv_invalid.DataBind();

            //Clear session
            Session["WING_DATA_UPLOAD"] = null;
            int intSave = 0;
            int intFail = 0;
            intSave = savedList.Count;
            intFail = invalidList.Count;

            if (intSave > 0)
            {
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User adds new wing account [Total record(s):", intSave, "]."));

            }
            if (intSave == 0)
            {
                AlertMessage("No data saved. View detail in [Fail] tab.");
            }
            //else if (intSave > 0 & intSave != intFail)
            //{
            //    AlertMessage("Some data were not saved. View detail in [Success/Fial] tab.");
            //}
            else if (intSave > 0 && intFail == 0)
            {
                AlertMessage("Data were saved. View datail in [Success] tab.");
            }
            else
            {
                AlertMessage("Some data were not saved. View detail in [Success/Fial] tab.");
            }
        }
            
        else
        {
            AlertMessage("Please upload data.");
        }

    }
    void UploadTransactionLog(string log)
    {
        bl_wing.SaveLog("wing_upload_transaction", log);
    }

    void Excel()
    {
        try
        {
            int row_count = gv_invalid.Rows.Count;
            if (row_count > 0)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Upload_Format");
                Helper.excel.Sheet = sheet1;
                //columns name
                Helper.excel.HeaderText = new string[] { "No.", "ID Type", "ID", "Surname(EN)", "GinvenName(EN)","Surname(KH)", "GivenName(KH)", 
                "Gender", "DOB (DD/MM/YYYY)", "PhoneNumber","Province","Product", "SumAssured","Premium","Mode of payment","Payment By","Wing Account", 
                "Consent Number","Factory","Agent Code","Remarks" };
                Helper.excel.generateHeader();
                //disign row
                int row_no = 0;
                foreach (GridViewRow row in gv_invalid.Rows)
                {
                    #region //Variable
                    Label id_type = (Label)row.FindControl("lblTypeIDText");
                    Label id = (Label)row.FindControl("lblID");
                    Label lastnameen = (Label)row.FindControl("lblENLastName");
                    Label firstnameen = (Label)row.FindControl("lblENFirstName");
                    Label lastnamekh = (Label)row.FindControl("lblKHLastName");
                    Label firstnamekh = (Label)row.FindControl("lblKHFirstName");
                    Label gender = (Label)row.FindControl("lblGender");
                    Label dob = (Label)row.FindControl("lblDOB");
                    Label phonenumber = (Label)row.FindControl("lblPhoneNumber");
                    Label province = (Label)row.FindControl("lblProvince");
                    Label product = (Label)row.FindControl("lblProductName");
                    Label SA = (Label)row.FindControl("lblSumAssured");
                    Label premium = (Label)row.FindControl("lblPremiumPaid");
                    Label paymode = (Label)row.FindControl("lblPaymentMode");
                    Label paymentby = (Label)row.FindControl("lblPaymentBy");
                    Label wingacc = (Label)row.FindControl("lblWingAccNumber");
                    Label consentNo = (Label)row.FindControl("lblConsentNo");
                    Label factory = (Label)row.FindControl("lblFactory");
                    Label agentcode = (Label)row.FindControl("lblAgentCode");
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
                    Cell4.SetCellValue(lastnameen.Text);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(firstnameen.Text);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(lastnamekh.Text);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(firstnamekh.Text);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(gender.Text);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(dob.Text);
                    HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                    style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy");
                    Cell9.CellStyle = style;

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(phonenumber.Text);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(province.Text);

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(product.Text);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(SA.Text);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(premium.Text);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(paymode.Text);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(paymentby.Text);

                    HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                    Cell17.SetCellValue(wingacc.Text);

                    HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                    Cell18.SetCellValue(consentNo.Text);

                    HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                    Cell19.SetCellValue(factory.Text);

                    HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                    Cell20.SetCellValue(agentcode.Text);

                    HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);
                    Cell21.SetCellValue(remarks.Text);
                }

                string filename = "camlife_wing_account" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
        catch (Exception ex)
        {
            AlertMessage("Oooop! something is going wrong, please check with your system administrator.");
            Log.AddExceptionToLog("Error function [Excel()] in page [import_data], detail:" + ex.Message);
        }

    }

}

