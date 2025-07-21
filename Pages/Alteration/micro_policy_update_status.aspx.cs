using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.IO;
using System.Data.SqlClient;

using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;

public partial class Pages_Alteration_micro_policy_update_status : System.Web.UI.Page
{
    private string MyPolicyType { get { return ViewState["VS_POL_TYPE"] + ""; } set { ViewState["VS_POL_TYPE"] = value; } }
    private enum MyPolicyTypeOption { IND, BDL }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        string userId = Membership.GetUser().ProviderUserKey.ToString();
        if (!Page.IsPostBack)
        {
            
            //BIND STATUS CODE
            DataTable tblStatus = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_GET_POLICY_STATUS", new string[,] { }, "micro_policy_update_status.aspx => Page load");
            Options.Bind(ddlStatus, tblStatus, "Detail", "Policy_Status_Code", 0);

            txtApplicationNo.Enabled = false;
            txtCustomerName.Enabled = false;
            txtCustomerNo.Enabled = false;
            txtPolicyNumber.Enabled = false;
            TxtEffectiveDate.Enabled = false;
            txtIssueDate.Enabled = false;
            txtcurrentstatus.Enabled = false;
            btnUpdate.Enabled = false;
            ddlStatus.Enabled = false;
            txtPlicystatusRemark.Enabled = false;
            txtpolicystatusdate.Enabled = false;
            
            if (Request.QueryString.Count > 0)
            {
                try
                {
                    MyPolicyType = Request.QueryString["P_TYPE"].ToString();
                    EnableSeach();
                }
                catch (Exception ex)
                {
                    EnableSeach(false);
                    Helper.Alert(true, "Invalid parameters", lblError);
                    
                }
            }
            else
            {
                EnableSeach(false);
                Helper.Alert(true, "Invalid parameters", lblError);
            }

            btnExport.Enabled = false;
        }

    }

    void EnableSeach(bool t = true)
    {
        txtPol.Enabled = t;
        btnSearch.Enabled = t;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string policyNo = txtPol.Text.Trim();
        if (policyNo.Trim() == "")
        {
            //alert 
            Helper.Alert(true, "Please input Policy Number.", lblError);
        }
        else//policy number is not blank.
        {
            //cleare old data in form
            txtApplicationNo.Text = "";
            txtCustomerName.Text = "";
            txtCustomerNo.Text = "";
            txtPolicyNumber.Text = "";
            TxtEffectiveDate.Text = "";
            txtIssueDate.Text = "";
            hdfPolicyID.Value = "";
            txtcurrentstatus.Text = "";
            string appNumber = "";

            if (MyPolicyType == MyPolicyTypeOption.IND.ToString())
            {

                DataTable tbl = da_micro_policy.GetPolicyDetailByPolicyNumber(policyNo);
                if (da_micro_policy.SUCCESS)
                {

                    if (tbl.Rows.Count > 0)
                    {
                        EnableUpdate();

                        #region v1
                        //foreach (DataRow row in tbl.Rows)
                        //{
                        //    hdfPolicyID.Value = row["policy_id"].ToString();
                        //    txtPolicyNumber.Text = row["policy_number"].ToString();
                        //    txtCustomerNo.Text = row["customer_number"].ToString();
                        //    txtCustomerName.Text = row["last_name_in_khmer"].ToString() + " " + row["first_name_in_khmer"].ToString() + " / " + row["last_name_in_english"].ToString() + " " + row["first_name_in_english"].ToString();
                        //    txtIssueDate.Text = Convert.ToDateTime(row["issued_date"].ToString()).ToString("dd/MM/yyyy");
                        //    TxtEffectiveDate.Text = Convert.ToDateTime(row["effective_date"].ToString()).ToString("dd/MM/yyyy");
                        //    txtApplicationNo.Text = row["application_number"].ToString();
                        //    txtpolicystatusdate.Text = Convert.ToDateTime(row["policy_status_date"].ToString()).ToString("dd/MM/yyyy");
                        //    txtPlicystatusRemark.Text = row["policy_status_remarks"].ToString();
                        //    appNumber = row["application_number"].ToString();
                        //}
                        #endregion
                        #region v2
                        DataRow row = tbl.Rows[0];
                        hdfPolicyID.Value = row["policy_id"].ToString();
                        txtPolicyNumber.Text = row["policy_number"].ToString();
                        txtCustomerNo.Text = row["customer_number"].ToString();
                        txtCustomerName.Text = row["last_name_in_khmer"].ToString() + " " + row["first_name_in_khmer"].ToString() + " / " + row["last_name_in_english"].ToString() + " " + row["first_name_in_english"].ToString();
                        txtIssueDate.Text = Convert.ToDateTime(row["issued_date"].ToString()).ToString("dd/MM/yyyy");
                        TxtEffectiveDate.Text = Convert.ToDateTime(row["effective_date"].ToString()).ToString("dd/MM/yyyy");
                        txtApplicationNo.Text = row["application_number"].ToString();
                        txtpolicystatusdate.Text = Convert.ToDateTime(row["policy_status_date"].ToString()).Year == 1900 ? DateTime.Now.ToString("dd/MM/yyyy") : Convert.ToDateTime(row["policy_status_date"].ToString()).ToString("dd/MM/yyyy");
                        txtPlicystatusRemark.Text = row["policy_status_remarks"].ToString();
                        appNumber = row["application_number"].ToString();
                        #endregion

                        //get application infomation by application number
                        bl_micro_application objApp = da_micro_application.GetApplication(appNumber);

                        //get policy information by application id
                        bl_micro_policy objPolicy = da_micro_policy.GetPolicyByApplicationID(objApp.APPLICATION_ID);

                        //alert message
                        //Helper.Alert(false, " Current Status= " + objPolicy.POLICY_STATUS + ", Policy numbe=" + objPolicy.POLICY_NUMBER, lblError);
                        txtcurrentstatus.Text = objPolicy.POLICY_STATUS;
                    }

                    else
                    {

                        //alert
                        Helper.Alert(false, "No record found.", lblError);
                        EnableUpdate(false);
                    }
                }
                else
                {
                    ddlStatus.Enabled = false;
                    txtPlicystatusRemark.Enabled = false;
                    btnUpdate.Enabled = false;
                    txtpolicystatusdate.Enabled = false;
                    //alert
                    Helper.Alert(true, da_micro_policy.MESSAGE, lblError);
                }
            }
            else if (MyPolicyType == MyPolicyTypeOption.BDL.ToString())
            {
                var obj = (List<Report.GroupMicro.PolicyDetail>)Report.GroupMicro.GetPolicyDetail(policyNo, Report.GroupMicro.ReportType.ListObject);
                if (obj.Count > 0)
                {
                    var pol = obj[0];
                    hdfPolicyID.Value = pol.PolicyId;
                    txtPolicyNumber.Text = pol.PolicyNumber;
                    txtCustomerNo.Text = pol.CustomerNumber;
                    txtCustomerName.Text = pol.FullNameKh + " / " + pol.FullNameEn;
                    txtIssueDate.Text = pol.IssuedDate.ToString("dd/MM/yyyy");
                    TxtEffectiveDate.Text = pol.EffectiveDate.ToString("dd/MM/yyyy");
                    txtApplicationNo.Text = pol.ApplicationNumber;
                    txtpolicystatusdate.Text = pol.PolicyStatusDate.Year == 1900 ? DateTime.Now.ToString("dd/MM/yyyy") : pol.PolicyStatusDate.ToString("dd/MM/yyyy");
                    txtPlicystatusRemark.Text = pol.PolicyStatusRemarks;
                    appNumber = pol.ApplicationNumber;
                    txtcurrentstatus.Text = pol.PolicyStatus;

                    EnableUpdate();
                }
                else
                {
                    Helper.Alert(false, "No record found.", lblError);
                    EnableUpdate(false);
                }
            }

        }
    }
    void EnableUpdate(bool t = true)
    {
        btnUpdate.Enabled = t;
        ddlStatus.Enabled = t;
        txtPlicystatusRemark.Enabled = t;
        txtpolicystatusdate.Enabled = t;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //string msg = "";
        //bool isValid = true;
        if (hdfPolicyID.Value == "")
        {
            Helper.Alert(true, "Policy ID is required.", lblError);
        }
        else if (ddlStatus.SelectedValue.Trim() == "")
        {
            Helper.Alert(true, "Policy New  Status is required.", lblError);
        }
        else if (ddlStatus.SelectedValue.ToUpper() != "IF" && txtPlicystatusRemark.Text.Trim() == "")
        {
            Helper.Alert(true, "Policy Status Remarks is required.", lblError);
        }
        else if (!Helper.IsDate(txtpolicystatusdate.Text))
        {
            Helper.Alert(true, "Policy Status Date is required with the format [DD/MM/YYYY].", lblError);
        }
        else if (ddlStatus.SelectedValue == txtcurrentstatus.Text.Trim())
        {
            Helper.Alert(true, "New Policy Status is required.", lblError);
        }
        else if ( txtcurrentstatus.Text.Trim()=="EXP")
        {
            Helper.Alert(true, "Policy was already expired, system not allowed to change.", lblError);
        }
        else // abled to update
        {
            //code to update in database
            bool result = false;
            if (MyPolicyType == MyPolicyTypeOption.IND.ToString())
            {
                result = da_micro_policy.UpdatePolicyStatus(new bl_micro_policy()
                {
                    POLICY_ID = hdfPolicyID.Value,
                    POLICY_STATUS = ddlStatus.SelectedValue,
                    POLICY_STATUS_REMARKS = txtPlicystatusRemark.Text,
                    POLICY_STATUS_DATE = Helper.FormatDateTime(txtpolicystatusdate.Text.ToString()),
                    UPDATED_ON = DateTime.Now,
                    UPDATED_BY = Membership.GetUser().UserName,
                });
            }
            else if (MyPolicyType == MyPolicyTypeOption.BDL.ToString())
            {
                result = da_micro_group_policy.UpdatePolicyStatus(hdfPolicyID.Value, ddlStatus.SelectedValue, Helper.FormatDateTime(txtpolicystatusdate.Text.Trim()), txtPlicystatusRemark.Text, Membership.GetUser().UserName, DateTime.Now);
            }

            if (result)
            {
                Helper.Alert(false, "Updated successfully.", lblError);
                btnUpdate.Enabled = false;
                btnSearch_Click(null, null);
            }
            else
            {
                Helper.Alert(true, "Updated fail, Detail:" + da_micro_policy.MESSAGE, lblError);
            }
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if ((fdlPolicy.PostedFile != null) && !string.IsNullOrEmpty(fdlPolicy.PostedFile.FileName))
        {
            #region file is selected
            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(fdlPolicy.PostedFile.FileName);
           string  extention = Path.GetExtension(file_name);
           file_name = file_name.Replace(extention, DateTime.Now.ToString("yyyymmddhhmmss") + extention);
            string file_path = "";

            if (extention.Trim().ToLower() == ".xls" || extention.Trim().ToLower() == ".xlsx")
            {
                file_path = save_path + file_name;
                fdlPolicy.SaveAs(Server.MapPath(file_path));//save file 
                DataTable myData = new DataTable();
                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "select * from [Sheet1$]";
                if (my_excel.GetSheetName().ToLower() == "sheet1$")
                {
                    DataTable tblResult = new DataTable();
                    DataRow dRow;  
     
                    var col =tblResult.Columns;
                    col.Add("No");
                    col.Add("Account_Number");
                    col.Add("Policy_Number");
                    col.Add("Policy_Status");
                    col.Add("Policy_Status_Date");
                    col.Add("Policy_Status_Remarks");

                    myData = my_excel.GetData();
                    if (my_excel.Status)
                    {
                        SqlConnection con = new SqlConnection(AppConfiguration.GetConnectionString());
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader dr;
                        SqlDataAdapter da;
                        DataTable tbl = new DataTable();
                        try
                        {
                          
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.CommandText = "SP_CT_GROUP_MICRO_POLICY_TERMINATE_UPLOAD_TEMP_UPLOAD";
                            cmd.Parameters.AddWithValue("@TBL", myData);
                            cmd.Parameters.AddWithValue("@PROJECT_TYPE", "Wing Digital Loan");
                            cmd.Parameters.AddWithValue("@CREATED_BY", Membership.GetUser().UserName);
                            cmd.Parameters.AddWithValue("@CREATED_ON", DateTime.Now);

                            cmd.Connection = con;
                            con.Open();
                          da  = new SqlDataAdapter(cmd);
                          da.Fill(tbl);
                          cmd.Dispose();
                          da.Dispose();
                          con.Close();
                          //  dr = cmd.ExecuteReader();
                            try
                            {
                               
                                foreach (DataRow r in tbl.Rows)
                                {
                                    dRow = tblResult.NewRow();
                                  dRow["No"]=  tbl.Rows.IndexOf(r) + 1;
                                    dRow["Account_Number"] = r["Account_number"].ToString();
                                    dRow["Policy_Number"] = r["Policy_Number"].ToString();
                                    dRow["Policy_Status"] = r["policy_status"].ToString();
                                    dRow["Policy_Status_Date"] = r["Policy_Status_Date"].ToString();
                                    dRow["Policy_Status_Remarks"] = r["Policy_Status_Remarks"];
                                    tblResult.Rows.Add(dRow);
                                }
                                Session["SS_DATA_POLICY_TER"] = tblResult;
                                gvResult.DataSource = tblResult;
                                gvResult.DataBind();
                                Helper.Alert(false, string.Concat("Uploaded successfully"), lblError);

                                btnExport.Enabled = true;
                            }
                            catch (Exception ex)
                            {
                                btnExport.Enabled = false;
                                Log.AddExceptionToLog(string.Concat("Error function [btnUpload_Click(object sender, EventArgs e)] in class [ micro_policy_update_status.aspx.cs], detail:", ex.ToString(), "=>", ex.StackTrace));
                                Helper.Alert(true, string.Concat("Uploaded successfully , but retrieve result is getting error. Detail: ", ex.Message), lblError);
                            }

                           
                        }
                        catch (Exception ex)
                        {
                            btnExport.Enabled = false;
                            Log.AddExceptionToLog(string.Concat("Error function [btnUpload_Click(object sender, EventArgs e)] in class [ micro_policy_update_status.aspx.cs], detail:", ex.ToString(), "=>", ex.StackTrace));
                            Helper.Alert(true, string.Concat("Upload file is getting error, detail: ", ex.Message, lblError), lblError);
                        }
                    }
                    else
                    {
                        Helper.Alert(true, my_excel.Message, lblError);

                    }
                }
                else
                {
                    Helper.Alert(true, string.Concat("[",my_excel.GetSheetName().ToLower(),"]"), lblError);
                }

                if(File.Exists(file_path))
                    File.Delete(file_path);
            }
            #endregion file is selected
        }
        else
        {
            #region file is empty
            Helper.Alert(true, "Please select a file.", lblError);
            #endregion file is empty
        }

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["SS_DATA_POLICY_TER"] != null)
        {
            DataTable tbl = (DataTable)Session["SS_DATA_POLICY_TER"];
            List<string> colName = new List<string>();

            foreach (DataColumn col in tbl.Columns)
            {
                colName.Add(col.ColumnName);
            }

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
            Helper.excel.Sheet = sheet1;
            Helper.excel.Title = new string[] { "Terminated Policy List" };
            Helper.excel.HeaderText = colName.ToArray();
            Helper.excel.generateHeader();

            int row_no = 0;
            row_no = Helper.excel.NewRowIndex - 1;
            int colIndex = -1;
            foreach (DataRow r in tbl.Rows)
            {

                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);
                foreach (DataColumn col in tbl.Columns)
                {
                    colIndex += 1;
                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(colIndex);
                    Cell1.SetCellValue(r[colIndex].ToString());
                }
                colIndex = -1;

            }
            string filename = "Terminated_Policy_List_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);


            Response.BinaryWrite(file.GetBuffer());

            Response.End();
        }
    }
}