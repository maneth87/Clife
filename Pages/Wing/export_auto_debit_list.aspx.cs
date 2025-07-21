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
using System.Web.Security;
public partial class Pages_Wing_export_consent_form_list : System.Web.UI.Page
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
            btnExcel.Visible = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (Helper.IsDate(txtPaymentInputDate.Text))
        {
            LoadData();
        }
        else
        {
            AlertMessage("Please select a valid format date.");
        }
    }

    void LoadData()
    {
        DataTable tbl = da_wing.Policy.GetWingPolicy(new DateTime(1900,1,1),new DateTime(1900,1,1),"","New");
        DataTable tbl_final = tbl.Clone();
        DataRow new_row;
        tbl_final.Columns.Add("PaymentInputDate");
        //Loop all rows in table tbl
        foreach (DataRow row in tbl.Rows)
        {
            new_row = tbl_final.NewRow();
        //Loop column in table tbl
            foreach (DataColumn col in tbl.Columns)
            {
                new_row[col.ColumnName] = row[col.ColumnName];
            }
            new_row["PaymentInputDate"] = txtPaymentInputDate.Text.Trim();

            tbl_final.Rows.Add(new_row);
        }

        if (tbl_final.Rows.Count > 0)
        {
            //show button export
            btnExcel.Visible = true;
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries consent form list with criteria [Payment input date:", txtPaymentInputDate.Text.Trim(),"]."));
        }
        else
        {
            //hide button export
            btnExcel.Visible = false;
        }
       
        //show data in grid view
        gv_valid.DataSource = tbl_final;
        gv_valid.DataBind();


    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        gv_valid.DataSource = null;
        gv_valid.DataBind();
        txtPaymentInputDate.Text = "";
        btnExcel.Visible = false;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Excel();
    }

    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
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
            Helper.excel.HeaderText = new string[] { "No.", "ConsentNumber","WingAccount","Debit Amount","Currency","Wing Remark","PaymentInputDate(dd/MM/yyyy)","PaymentReference" };
            Helper.excel.generateHeader();
            //disign row
            int row_no = 0;
            foreach (GridViewRow row in gv_valid.Rows)
            {
                #region //Variable
                Label consent_no = (Label)row.FindControl("lblConsentNo");
                Label policy_number = (Label)row.FindControl("lblPolicyNumber");
                Label premium = (Label)row.FindControl("lblPremiumPaid");
                Label paymentinputdate = (Label)row.FindControl("lblPaymentInputDate");
               
                #endregion

                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(row_no+"");

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(consent_no.Text);

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(policy_number.Text);

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(premium.Text);

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue("USD");
                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(row_no-1+"");

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(paymentinputdate.Text);
                HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy");
                Cell7.CellStyle = style;

                HSSFCell Cell8= (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue("0");

            }
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports consent form list total record:[", gv_valid.Rows.Count, "]."));
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

}