using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Web.Security;
public partial class Pages_Wing_wing_policy_uploaded : System.Web.UI.Page
{

    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (!Page.IsPostBack)
        {
            //hide button export
            btnExcel.Visible = false;
            btnPDF.Visible = false;
        }
    }

    void LoadData(DateTime f_date, DateTime t_date, string policy_number , string policy_status)
    {
        DataTable tbl = da_wing.Policy.GetWingPolicy(f_date, t_date, policy_number, policy_status);
        if (tbl.Rows.Count > 0)
        {
            //show button export
            btnExcel.Visible = true;
            btnPDF.Visible = true;

            Session["AUTO_DEBIT_LIST"] = tbl;

            string desc = string.Concat("Issued Date from:", txtFromDate.Text.Trim(), " to:", txtTodate.Text.Trim());
            desc += txtPolicyNumber.Text.Trim() == "" ? "" : string.Concat(" Pol No:", txtPolicyNumber.Text.Trim());
            desc += ddlPolicyStatus.SelectedIndex == 0 ? "" : string.Concat(" Pol Status:", ddlPolicyStatus.SelectedItem.Text);
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, String.Concat("User inquiries Wing policies uploaded data with criteria [",desc,"]."));
        }
        else
        {
            //hide button export
            btnExcel.Visible = false;
            btnPDF.Visible = false;

        }
        //show data in grid view
        gv_valid.DataSource = tbl;
        gv_valid.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData(txtFromDate.Text.Trim() == "" ? new DateTime(1900, 1, 1) : Helper.FormatDateTime(txtFromDate.Text.Trim()), txtTodate.Text.Trim() == "" ? new DateTime(1900, 1, 1) : Helper.FormatDateTime(txtTodate.Text.Trim()), txtPolicyNumber.Text.Trim() == "" ? "" : txtPolicyNumber.Text.Trim(), ddlPolicyStatus.SelectedValue=="" ? "" : ddlPolicyStatus.SelectedValue.Trim());
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }
    void Clear()
    {
        btnExcel.Visible = false;
        btnPDF.Visible = false;
        txtFromDate.Text = "";
        txtTodate.Text = "";
        ddlPolicyStatus.SelectedIndex = 0;
        txtPolicyNumber.Text = "";
        gv_valid.DataSource = null;
        gv_valid.DataBind();
    }

    void Excel()
    {
        int row_count = gv_valid.Rows.Count;
        if (row_count > 0)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("consent_form_list");
            Helper.excel.Sheet = sheet1;
            //columns name
            Helper.excel.HeaderText = new string[] { "No.", "ConsentNumber", "FullName (EN)", "FullName (KH)", "DOB (DD/MM/YYYY)", "Bank Account", "PhoneNumber", "FactoryName", "Currency", "Gender", "ID_Type", "ID_Card", "Remarks" ,"Status"};
            Helper.excel.generateHeader();
            //disign row
            int row_no = 0;
            foreach (GridViewRow row in gv_valid.Rows)
            {
                #region //Variable
                Label consent_no = (Label)row.FindControl("lblConsentNo");

                Label fullname_en = (Label)row.FindControl("lblFullNameEn");
                Label fullname_kh = (Label)row.FindControl("lblFullNameKh");
                Label dob = (Label)row.FindControl("lblDOB");
                Label policy_number = (Label)row.FindControl("lblPolicyNumber");
                Label phone_number = (Label)row.FindControl("lblPhoneNumber");
                Label factory_name = (Label)row.FindControl("lblFactory");
                Label gender = (Label)row.FindControl("lblGender");
                Label id_type = (Label)row.FindControl("lblIDType");
                Label id = (Label)row.FindControl("lblID");

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
                //Cell13.SetCellValue(remarks.Text);
                Cell13.SetCellValue("");

                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                //Cell13.SetCellValue(remarks.Text);
                Cell14.SetCellValue("");

            }

            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, String.Concat("User exports data to excel file [Total record(s):", row_count, "]."));


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
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Excel();
    }
    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }
    protected void btnPDF_Click(object sender, EventArgs e)
    {
       DataTable tbl= (DataTable) Session["AUTO_DEBIT_LIST"];

       #region //PDF
       ReportViewer report = new ReportViewer();
       ReportDataSource report_source;
       report.Reset();


       DataRow row = tbl.Rows[0];
       report.LocalReport.ReportPath = Server.MapPath("consent_form_list.rdlc");
       report_source = new ReportDataSource("ds_consent_form", tbl);
       report.LocalReport.Refresh();
       report.LocalReport.DataSources.Clear();
        //Get camlife staff information
       bl_camlife_staff camlife_staff = da_camlife_staff.GetCamlifeStaff(System.Web.Security.Membership.GetUser().UserName);
       ReportParameter[] paras = new ReportParameter[] { 
        
                    new ReportParameter("Prepared_Date", "Phnom Penh," + DateTime.Now.DayOfWeek + "," + DateTime.Now.ToString("MMM dd, yyyy")),
                    new ReportParameter("Prepared_by", camlife_staff.EmpCode == null || camlife_staff.EmpCode=="" ? "": (camlife_staff.EmpLastName + " " + camlife_staff.EmpFirstName + Environment.NewLine + camlife_staff.EmpPosition)),
                    new ReportParameter("ReportTitle", "Consent Form List For The Month of " + DateTime.Now.ToString("MMM yyyy"))
                    //new ReportParameter("PrintedBy",System.Web.Security.Membership.GetUser().UserName)
                  
                };

       //Assign parameters to report
      report.LocalReport.SetParameters(paras);
       //add datasource in to report
       report.LocalReport.DataSources.Add(report_source);
       //export to pdf
       Report_Generator.ExportToPDF(this.Context, report, "Policy_Premium_Detail_Report" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
       #endregion

       SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, String.Concat("User view data in pdf file [Total record(s):", tbl.Rows.Count, "]."));

    }
}