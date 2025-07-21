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
public partial class Pages_Wing_wing_renew_policy_report : System.Web.UI.Page
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
            loadYear();
            loadMonth();

            btnExcel.Visible = false;

        }
    }

    void loadYear()
    {
        int currentYear = DateTime.Now.Year;
        ddlYear.Items.Add(new ListItem(currentYear-1+"", currentYear-1+""));
        ddlYear.Items.Add(new ListItem(currentYear  + "", currentYear  + ""));
        ddlYear.Items.Add(new ListItem(currentYear + 1 + "", currentYear + 1 + ""));
        //ddlYear.Items.Add(new ListItem("2050","2050"));
       // ddlYear.SelectedIndex = ddlYear.Items.Count - 2;
      
        for (int i = 0; i < ddlYear.Items.Count; i++)
        {

            if (ddlYear.Items[i].Value == currentYear + "")
            {
                ddlYear.SelectedIndex = i;
                break;
            }
        }
       
    }
    void loadMonth()
    {
        for (int i = 1; i <= 12; i++)
        {
            ddlMonth.Items.Add(new ListItem(Helper.Get_month_in_english(i), i + ""));
        }
        for (int i = 0; i < ddlMonth.Items.Count; i++)
        {

            if (ddlMonth.Items[i].Value.ToUpper() == DateTime.Now.Month+"")
            {
                ddlMonth.SelectedIndex = i;
                break;
            }
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

    }
    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tbl = da_wing.Policy.GetRenewPolicy(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));

            DataTable tbl_final = tbl.Clone();
            tbl_final.Columns.Add("Remarks");
            bl_product product;
            DataRow newRow;
            foreach (DataRow row in tbl.Rows)
            {
                string remarks = "";
                product = new bl_product();
                product = da_product.GetProductByProductID(row["product_id"].ToString().Trim());
                int age = Calculation.Culculate_Customer_Age(Convert.ToDateTime(row["birth_date"].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(row["expiry_date"].ToString()).Date.AddDays(1));
                if (age >= product.Age_Min && age <= product.Age_Max)
                {
                    remarks = "";
                }
                else
                {
                    remarks = "[" + age + "] is over age.";
                }
                newRow = tbl_final.NewRow();
                foreach (DataColumn col in tbl.Columns)
                {

                    newRow[col.ColumnName] = row[col.ColumnName];

                }
                newRow["Age"] = age;
                newRow["remarks"] = remarks;

                tbl_final.Rows.Add(newRow);
            }

            string logDec = string.Concat( "Expiry year:", ddlYear.SelectedItem.Text,", month:", ddlMonth.SelectedItem.Text);
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries renew policies with criteria [", logDec,"] [Total record(s):", tbl_final.Rows.Count,"]."));

            gv_valid.DataSource = tbl_final;
            gv_valid.DataBind();

            btnExcel.Visible = tbl_final.Rows.Count > 0 ? true : false;
        }
        catch (Exception ex)
        {
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            btnExcel.Visible = false;
            Log.AddExceptionToLog("Error function [btnSearch_Click(object sender, EventArgs e)] in page [wing_renew_policy_report.aspx.cs], detail:" + ex.Message);

        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Excel();
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
            Helper.excel.HeaderText = new string[] { "No.", "ConsentNumber", "WingAccount", "Debit Amount", "Currency", "Wing Remark", "PaymentInputDate(dd/MM/yyyy)", "PaymentReference" ,"Remarks"};
            Helper.excel.generateHeader();
            //disign row
            int row_no = 0;
            foreach (GridViewRow row in gv_valid.Rows)
            {
                #region //Variable
                Label consent_no = (Label)row.FindControl("lblConsentNo");
                Label policy_number = (Label)row.FindControl("lblPolicyNumber");
                Label premium = (Label)row.FindControl("lblPremiumPaid");
                Label expiry_date = (Label)row.FindControl("lblExpiryDate");
                Label remarks = (Label)row.FindControl("lblRemarks");
                #endregion

                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(row_no + "");

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(consent_no.Text);

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(policy_number.Text);

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(premium.Text);

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue("USD");
                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(row_no - 1 + "");

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                //convert expire date from text to date format
                expiry_date.Text = Helper.FormatDateTime(expiry_date.Text.Trim()).Date.AddDays(1).ToString("dd/MM/yyyy");
                //Cell7.SetCellValue( Convert.ToDateTime( expiry_date.Text).Date.AddDays(1).ToString("dd/MM/yyyy"));
                Cell7.SetCellValue(expiry_date.Text);
                //HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                //style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MM/yyyy");
                //Cell7.CellStyle = style;

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue("0");

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                Cell9.SetCellValue(remarks.Text);

            }

            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports renew policies in excel file [Total record(s):",row_count, "]."));

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