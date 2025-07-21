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

public partial class Pages_Wing_wing_register_consent_form : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }

    string user_name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        user_name = Membership.GetUser().UserName;
    }
    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        UploadData();
    }
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        SaveData();
    }
    protected void ibtnClear_Click(object sender, ImageClickEventArgs e)
    {
        Session["CONSENT_FORM_LIST"] = null;
        gv_invalid.DataSource = null;
        gv_invalid.DataBind();
        gv_valid.DataSource = null;
        gv_valid.DataBind();
        export_excel.Visible = false;
    }
    protected void export_excel_Click(object sender, EventArgs e)
    {

        Excel();
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
                List<consentFormList> listUploadData, invalidList;
                if (Validate(out message, out listUploadData, out invalidList) == true)
                {
                    gv_valid.DataSource = listUploadData;
                    gv_valid.DataBind();

                    gv_invalid.DataSource = invalidList;
                    gv_invalid.DataBind();

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
                    Session["CONSENT_FORM_LIST"] = listUploadData;

                    export_excel.Visible = invalidList.Count > 0 ? true : false;
                    if (listUploadData.Count > 0|| invalidList.Count>0)
                    {
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User uploads temporary censent form list [Total success:", listUploadData.Count, " Total fail:", invalidList.Count,"]."));
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
    void SaveData()
    {

        if (Session["CONSENT_FORM_LIST"] != null)
        {

            List<consentFormList> consent_form_list = (List<consentFormList>)Session["CONSENT_FORM_LIST"];
            List<consentFormList> invalidList = new List<consentFormList>();
            List<consentFormList> validList = new List<consentFormList>();
            DateTime transaction_date = DateTime.Now;
            string policy_id = "";
            string policy_detail_id = "";
            string save_log = "<<Start saving\t" + transaction_date.ToString("dd MMM yyyy HH:mm:ss") + "\tBy[" + user_name + "]" + Environment.NewLine;
            bool isUpdatePolicyRemarks = false;
            bool isUpdatedPolicyStatus = false;
            try
            {
                if (consent_form_list.Count > 0)
                {
                    foreach (consentFormList consent in consent_form_list)
                    {
                        if (da_wing.Policy.isPending(consent.WingAccNumber, out policy_id, out policy_detail_id))
                        {
                            #region //check status

                            isUpdatePolicyRemarks = da_wing.Policy.UpdatePolicyRemarks(policy_id, consent.Remarks, user_name, transaction_date);
                            if (isUpdatePolicyRemarks)
                            {
                                #region store save success to display
                                validList.Add(consent);
                                #endregion
                                #region update policy status
                                //store save log
                                save_log += "1. upadate policy remarks [ct_ci_policy]:SUCCESS\t [policy id:" + policy_id + "]" + Environment.NewLine;
                                if (consent.Status.ToUpper() == "SUCCESS")
                                {
                                    isUpdatedPolicyStatus = da_wing.PolicyDetail.UpdatePolicyStatus(policy_id, policy_detail_id, "NEW", "NEW", "", user_name, transaction_date);
                                    if (isUpdatedPolicyStatus)
                                    {
                                        save_log += "2. update policy status [ct_ci_policy_detail]:SUCCESS\t [policy id:" + policy_id + "]\t [policy detail id: " + policy_detail_id + "]" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        save_log += "2. update policy status [ct_ci_policy_detail]:FAIL\t [policy id:" + policy_id + "]\t [policy detail id: " + policy_detail_id + "]" + Environment.NewLine;

                                    }
                                }
                                else
                                {
                                    // just save log 
                                    save_log += "2. update policy status [ct_ci_policy_detail]:FAIL\t [policy id:" + policy_id + "]\t [policy detail id: " + policy_detail_id + "]" + Environment.NewLine;

                                }

                                #endregion
                            }
                            else
                            {
                                save_log += "1. update policy remarks [ct_ci_policy]:FAIL\t [policy id:" + policy_id + "]\t" + "[Other transactions were terminated.]" + Environment.NewLine;
                            }

                            if (isUpdatePolicyRemarks != true && isUpdatedPolicyStatus != true)
                            {
                                invalidList.Add(new consentFormList()
                                {
                                    ConsentNumber = consent.ConsentNumber,
                                    WingAccNumber = consent.WingAccNumber,
                                    FullNameEN = consent.FullNameEN,
                                    FullNameKH = consent.FullNameKH,
                                    PhoneNumber = consent.PhoneNumber,
                                    DOB = consent.DOB,
                                    FactoryName = consent.FactoryName,
                                    Currency = consent.Currency,
                                    Gender = consent.Gender,
                                    IDTypeText = consent.IDTypeText,
                                    ID = consent.ID,
                                    Status = consent.Status,
                                    Remarks = "Save fail"
                                });
                            }

                        }
                        else
                        {
                            invalidList.Add(new consentFormList()
                            {
                                ConsentNumber = consent.ConsentNumber,
                                WingAccNumber = consent.WingAccNumber,
                                FullNameEN = consent.FullNameEN,
                                FullNameKH = consent.FullNameKH,
                                PhoneNumber = consent.PhoneNumber,
                                DOB = consent.DOB,
                                FactoryName = consent.FactoryName,
                                Currency = consent.Currency,
                                Gender = consent.Gender,
                                IDTypeText = consent.IDTypeText,
                                ID = consent.ID,
                                Status = consent.Status,
                                Remarks = "consent number [" + consent.ConsentNumber + "] is not in pending status."
                            });
                            #endregion //end check status
                        }

                    }
                    save_log += "End saving\t" + DateTime.Now.ToString(" dd MMM yyyy HH:mm:ss") + ">>";
                    SaveLog(save_log);
                    //show data in page
                    gv_valid.DataSource = validList;
                    gv_valid.DataBind();
                    gv_invalid.DataSource = invalidList;
                    gv_invalid.DataBind();
                    if (validList.Count > 0 || invalidList.Count > 0)
                    {
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User saves consent form list [Total success:", validList.Count, " Total fail:", invalidList.Count, "]."));
                    }
                    if (validList.Count > 0 && invalidList.Count == 0)
                    {
                        AlertMessage("Data were saved successfuly.");
                    }
                    else if (validList.Count == 0 && invalidList.Count > 0)
                    {
                        AlertMessage("Data were saved fail, view datail in [Fail] tab.");
                    }
                    else
                    {
                        AlertMessage("Few data were saved fail, view datail in [Fail] tab.");
                    }

                   
                }
                else
                {
                    AlertMessage("No data to save.");
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [SaveData()] in page [wing_register_consent_form], detail: " + ex.Message + "=>" + ex.StackTrace);
            }
        }
        else
        {
            AlertMessage("No data to save.");
        }
        Session["CONSENT_FORM_LIST"] = null;
    }

    bool Validate(out string message, out List<consentFormList> listUploadData, out List<consentFormList> invalidList)
    {
        bool status = true;
        message = "";
        listUploadData = new List<consentFormList>();
        invalidList = new List<consentFormList>();
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

                if (my_excel.GetSheetName() != "consent_form_list$")
                {
                    message = "File is not correct format, please donwload file template from the system.";
                    //save log
                    UploadTransactionLog("[Validate Execl File] \t [Sheet Name:" + my_excel.GetSheetName().ToString() + "] is not valid.");
                    status = false;
                }
                else
                {
                    my_excel.CommandText = "Select * from [consent_form_list$]";
                    DataTable tbl = my_excel.GetData();
                    int col_count = 0;
                    col_count = tbl.Columns.Count;
                    if (col_count > 14 || col_count < 0)//check number of columns
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        //save log
                        UploadTransactionLog("[Validate Execl File] \t [Number of columns] must be equal 14.");
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
                        else if (tbl.Columns[1].ColumnName.Trim() != "ConsentNumber")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[1].ColumnName.Trim() + "] is not valid, it must be [ConsentNumber].");

                        }
                        else if (tbl.Columns[2].ColumnName.Trim() != "FullName (EN)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[2].ColumnName.Trim() + "] is not valid, it must be [FullName (EN)].");

                        }
                        else if (tbl.Columns[3].ColumnName.Trim() != "FullName (KH)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[3].ColumnName.Trim() + "] is not valid, it must be [FullName (KH)].");

                        }
                        else if (tbl.Columns[4].ColumnName.Trim() != "DOB (DD/MM/YYYY)")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[4].ColumnName.Trim() + "] is not valid, it must be [DOB (DD/MM/YYYY)].");

                        }
                        else if (tbl.Columns[5].ColumnName.Trim() != "Bank Account")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[5].ColumnName.Trim() + "] is not valid, it must be [Bank Account].");

                        }
                        else if (tbl.Columns[6].ColumnName.Trim() != "PhoneNumber")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[6].ColumnName.Trim() + "] is not valid, it must be [PhoneNumber].");

                        }
                        else if (tbl.Columns[7].ColumnName.Trim() != "FactoryName")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[7].ColumnName.Trim() + "] is not valid, it must be [FactoryName].");

                        }
                        else if (tbl.Columns[8].ColumnName.Trim() != "Currency")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[8].ColumnName.Trim() + "] is not valid, it must be [Currency].");

                        }
                        else if (tbl.Columns[9].ColumnName.Trim() != "Gender")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[9].ColumnName.Trim() + "] is not valid, it must be [Gender].");

                        }
                        else if (tbl.Columns[10].ColumnName.Trim() != "ID_Type")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[10].ColumnName.Trim() + "] is not valid, it must be [ID_Type].");

                        }
                        else if (tbl.Columns[11].ColumnName.Trim() != "ID_Card")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[11].ColumnName.Trim() + "] is not valid, it must be [ID_Card].");

                        }
                        else if (tbl.Columns[12].ColumnName.Trim() != "Remarks")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[12].ColumnName.Trim() + "] is not valid, it must be [Remarks].");

                        }
                        else if (tbl.Columns[13].ColumnName.Trim() != "Status")
                        {
                            status = false;
                            UploadTransactionLog("[Validate Excel File] \t [Column Name:" + tbl.Columns[13].ColumnName.Trim() + "] is not valid, it must be [Status].");

                        }

                        #endregion
                    }

                    if (status == false)
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        listUploadData = new List<consentFormList>();
                    }
                    else
                    {
                        #region Get upload data from excel file
                        DataTable data_excel = my_excel.GetData();
                        consentFormList wing;
                        int row_index = -1;
                        consentFormList invalidData;
                        foreach (DataRow row in data_excel.Rows)
                        {
                            row_index = data_excel.Rows.IndexOf(row);
                            if (validRow(row, row_index, out invalidData))
                            {
                                wing = new consentFormList();
                                wing.ConsentNumber = row[1].ToString().Trim();
                                wing.FullNameEN = row[2].ToString().Trim();
                                wing.FullNameKH = row[3].ToString().Trim();
                                wing.DOB = row[4].ToString().Trim();
                                wing.WingAccNumber = row[5].ToString().Trim();
                                wing.PhoneNumber = row[6].ToString().Trim().Replace(" ", "");
                                wing.FactoryName = row[7].ToString().Trim();
                                wing.Currency = row[8].ToString().Trim();
                                wing.Gender = row[9].ToString().Trim();
                                wing.IDTypeText = row[10].ToString().Trim();
                                wing.IDType = Helper.GetIDCardTypeID(wing.IDTypeText);
                                wing.ID = row[11].ToString().Trim();
                                wing.Remarks = row[12].ToString().Trim();
                                wing.Status = row[13].ToString().Trim();


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
            listUploadData = new List<consentFormList>();
            status = false;
            message = "Oooop! something is going wrong, please contact your system administrator.";
            UploadTransactionLog("[Validate excel file] \t [Error]:please check log file for detail.");
            Log.AddExceptionToLog("Error function [Validate(out string message, List<bl_wing.DataUpload> listUploadData)] in page [import_data.aspx], Detail:" + ex.Message);
        }
        return status;
    }
    bool validRow(DataRow row, int row_index, out consentFormList invalidData)
    {
        bool valid = true;
        invalidData = new consentFormList();

        int customer_age = 0;
        bl_product product = new bl_product();
        try
        {
            //product = da_product.GetProductByProductID("CI");
            product = da_product.GetProductByProductID("AL");
            //invalid_list = (List<InvalidData>)Session["INVALID_DATA"];

            #region //Check invalid data


            if (row[1].ToString().Trim() == "" || row[1].ToString() == null)//Consent number is emqty
            {
                valid = false;
                invalidData.Remarks = "Consent Number is required.";
            }
            else if (row[2].ToString().Trim() == "")//Full Name EN is emqty
            {
                valid = false;
                invalidData.Remarks = "Full Name (EN) is required.";
            }
            else if (Helper.IsDate(row[4].ToString().Trim()) == false)//check dob date format
            {
                valid = false;
                invalidData.Remarks = "Invalid DOB";
            }


            else if (row[5].ToString().Trim() == "")//Bank Account
            {
                valid = false;
                invalidData.Remarks = "Bank Account is required.";
            }

            else if (row[6].ToString().Trim() == "")//Phone Number
            {
                valid = false;
                invalidData.Remarks = "Phone number is required.";
            }
            else if (row[6].ToString().Replace(" ", "").Trim().Length < 9 || row[6].ToString().Replace(" ", "").Trim().Length > 10)//phone length
            {
                valid = false;
                invalidData.Remarks = "Phone number is invalid length.";
            }
            else if (row[7].ToString().Trim() == "")//factory name
            {
                valid = false;
                invalidData.Remarks = "Factory Name is required.";
            }
            else if (row[8].ToString().Trim() == "")//Currency
            {
                valid = false;
                invalidData.Remarks = "Currency is required.";
            }

            else if (row[9].ToString().Trim() == "")//Gender
            {
                valid = false;
                invalidData.Remarks = "Gender is required.";
            }

            else if (Helper.GetIDCardTypeID(row[10].ToString().Trim()) < 0)//ID Type is emqty
            {
                valid = false;
                invalidData.Remarks = "Invalid ID type.";

            }

            else if (row[11].ToString().Trim() == "")//ID Card
            {
                valid = false;
                invalidData.Remarks = "ID Card is required.";
            }
            else if (row[13].ToString().Trim() == "")//Status
            {
                valid = false;
                invalidData.Remarks = "Status is required.";
            }
            if (Helper.IsDate(row[4].ToString().Trim()) == true)
            {
                customer_age = Calculation.Culculate_Customer_Age(row[4].ToString().Trim(), DateTime.Now);
                if (customer_age < product.Age_Min || customer_age > product.Age_Max)
                {
                    valid = false;
                    invalidData.Remarks = "Age=" + customer_age + " is not in range [" + product.Age_Min + "-" + product.Age_Max + "]";

                }
            }
            //store invalid data
            if (!valid)
            {
                invalidData.IDTypeText = row[10].ToString().Trim();
                invalidData.IDType = Helper.GetIDCardTypeID(invalidData.IDTypeText);
                invalidData.ID = row[11].ToString();
                invalidData.FullNameEN = row[2].ToString();
                invalidData.FullNameKH = row[3].ToString();
                invalidData.ConsentNumber = row[1].ToString().Trim();
                invalidData.Gender = row[9].ToString();
                invalidData.DOB = row[4].ToString().Trim();
                invalidData.PhoneNumber = row[6].ToString().Trim();
                invalidData.WingAccNumber = row[5].ToString().Trim();
                invalidData.FactoryName = row[7].ToString().Trim();
                invalidData.Currency = row[8].ToString().Trim();

                invalidData.Status = row[13].ToString().Trim();
            }

            #endregion
        }
        catch (Exception ex)
        {
            valid = false;
            Log.AddExceptionToLog("Error function [validRow(DataRow row)] in class [wing_register_consent_form.aspx.cs], row index [" + row_index + "], detail:" + ex.Message);
        }
        return valid;
    }

    class consentFormList : bl_wing.DataUpload
    {

        public string Status { get; set; }
        public string FullNameEN
        {
            get;
            set;
        }
        public string FullNameKH
        {
            get;
            set;
        }
        public string Currency { get; set; }
    }
    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }
    void UploadTransactionLog(string log)
    {
        bl_wing.SaveLog("wing_upload_consent_form_list_transaction", log);
    }
    void SaveLog(string log)
    {
        bl_wing.SaveLog("wing_save_consent_form_list_transaction", log);
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
            Helper.excel.HeaderText = new string[] { "No.", "ConsentNumber", "FullName (EN)", "FullName (KH)", "DOB (DD/MM/YYYY)", "Bank Account", "PhoneNumber", "FactoryName", "Currency", "Gender", "ID_Type", "ID_Card", "Status", "Remarks" };
            Helper.excel.generateHeader();
            //disign row
            int row_no = 0;
            foreach (GridViewRow row in gv_invalid.Rows)
            {
                #region //Variable
                Label consent_no = (Label)row.FindControl("lblConsentNo");

                Label fullname_en = (Label)row.FindControl("lblFullNameEn");
                Label fullname_kh = (Label)row.FindControl("lblFullNameKh");
                Label dob = (Label)row.FindControl("lblDOB");
                Label policy_number = (Label)row.FindControl("lblBankAccount");
                Label phone_number = (Label)row.FindControl("lblPhoneNumber");
                Label factory_name = (Label)row.FindControl("lblFactory");
                Label gender = (Label)row.FindControl("lblGender");
                Label id_type = (Label)row.FindControl("lblIDType");
                Label id = (Label)row.FindControl("lblID");
                Label status = (Label)row.FindControl("lblStatus");
                Label remarks = (Label)row.FindControl("lblRemarks");
                #endregion

                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(row_no);

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(consent_no.Text);

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(fullname_en.Text);

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(fullname_kh.Text);

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue(dob.Text);
                HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy");
                Cell5.CellStyle = style;

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(policy_number.Text);

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(phone_number.Text);

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue(factory_name.Text);

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                Cell9.SetCellValue("USD");


                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                Cell10.SetCellValue(gender.Text);

                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                Cell11.SetCellValue(id_type.Text);

                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                Cell12.SetCellValue(id.Text);
                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                Cell13.SetCellValue(status.Text);
                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                Cell14.SetCellValue(remarks.Text);


            }
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports invalid consent form list [Total record(s):", row_count, "]."));

            string filename = "camlife_wing_consentform" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
}