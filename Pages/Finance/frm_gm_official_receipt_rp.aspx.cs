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
public partial class Pages_Finance_frm_gm_official_receipt_rp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            tblResult.Attributes.CssStyle.Add("display", "none");
            Helper.BindChannel(ddlChannel);
            ddlChannel.SelectedIndex = 2;
            ddlChannel_SelectedIndexChanged(null, null);
            ddlChannel.Enabled = false;
        }
    }
    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (Helper.IsDate(txtPayDateFrom.Text.Trim()) && Helper.IsDate(txtPayDateTo.Text.Trim()))
        {
            DataTable tbl = da_officail_receipt.GetGroupMicroOfficialReceiptReport(Helper.FormatDateTime(txtPayDateFrom.Text.Trim()), Helper.FormatDateTime(txtPayDateTo.Text.Trim()));

            //FILTER
            string criteria = "";
            criteria = ddlChannel.SelectedValue != "" ? "channel_id='" + ddlChannel.SelectedValue + "'" : "";
            criteria += ddlChannelItem.SelectedValue != "" ? (criteria.Trim() != "" ? " and channel_item_id='" + ddlChannelItem.SelectedValue + "'" : " channel_item_id='" + ddlChannelItem.SelectedValue + "'") : "";
            criteria += txtReceiptNo.Text.Trim() != "" ? (criteria.Trim() != "" ? " and receipt_no like '%" + txtReceiptNo.Text.Trim() + "%'" : " receipt_no like '%" + txtReceiptNo.Text.Trim() + "%'") : "";

            DataTable tblFilter = tbl.Clone();
            foreach (DataRow r in tbl.Select(criteria))
            {
                tblFilter.ImportRow(r);
            }

            gv_valid.DataSource = tblFilter;
            gv_valid.DataBind();
            if (tblFilter.Rows.Count > 0)
            {
                tblResult.Attributes.CssStyle.Remove("display");
                btnExport.Enabled = true;
                Session["SS_DATA001"] =tblFilter;

            }
            else
            {
                btnExport.Enabled = false;
                tblResult.Attributes.CssStyle.Add("display", "none");
                Helper.Alert(false, "No Record(s) found with selected criterias", lblError);
                Session["SS_DATA001"] = null;

            }

           
        }
        else
        {
            Helper.Alert(true, "Pay Date From & To are required with format [DD-MM-YYYY].", lblError);
        }
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["SS_DATA001"] != null)
        {
            DataTable tbl = (DataTable)Session["SS_DATA001"];
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

           
            Helper.excel.Sheet = sheet1;
            Helper.excel.Title = new string[] { "Report Official Receipt " };
            Helper.excel.HeaderText = new string[]
                        {
                        "No","Policy No","Customer ","Customer ID","Product ","Receipt No","API","Amount Paid","Interest","Sum Insured","Date Payment","Period Pay", "Created Note", "Entry Date","Back Date"
                        };

            Helper.excel.generateHeader();

            int row_no = 0;
            row_no = Helper.excel.NewRowIndex - 1;
            double totalAnnual = 0;
            double totalAmount = 0;
            double totalInterest = 0;
            double totalSumInsured = 0;

            foreach (DataRow r in tbl.Rows)
            {
                totalAmount += Convert.ToDouble(r["amount"].ToString());
                totalAnnual += Convert.ToDouble(r["annual_premium"].ToString());
                totalInterest += Convert.ToDouble(r["interest_amount"].ToString());
                totalSumInsured += Convert.ToDouble(r["sum_assured"].ToString());
                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(row_no-1);

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(r["group_code"].ToString());

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(r["customer_name"].ToString());

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(r["customer_id"].ToString());

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue(r["en_title"].ToString());

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(r["receipt_no"].ToString());

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(r["annual_premium"].ToString());

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue(r["amount"].ToString());

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                Cell9.SetCellValue(r["interest_amount"].ToString());

                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                Cell10.SetCellValue(r["sum_assured"].ToString());

                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                Cell11.SetCellValue(Convert.ToDateTime(r["pay_date"].ToString()).ToString("dd-MMM-yyyy"));

                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                Cell12.SetCellValue("");

                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                Cell13.SetCellValue(r["created_note"].ToString());

                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                Cell14.SetCellValue( Convert.ToDateTime( r["entry_date"].ToString()).ToString("dd-MMM-yyyy"));

                HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                Cell15.SetCellValue("");

               

            }

            HSSFRow rowCell1 = (HSSFRow)sheet1.CreateRow(row_no + 1);
            HSSFCell Celltotal = (HSSFCell)rowCell1.CreateCell(5);
            Celltotal.SetCellValue("Total:");

            HSSFCell CellTotalLoanAmountKh = (HSSFCell)rowCell1.CreateCell(6);
            CellTotalLoanAmountKh.SetCellValue(totalAnnual);

            HSSFCell CellTotalLoanAmount = (HSSFCell)rowCell1.CreateCell(7);
            CellTotalLoanAmount.SetCellValue(totalAmount);

            HSSFCell CellTotalPremiumKh = (HSSFCell)rowCell1.CreateCell(8);
            CellTotalPremiumKh.SetCellValue(totalInterest);

            HSSFCell CellTotalPremium = (HSSFCell)rowCell1.CreateCell(9);
            CellTotalPremium.SetCellValue(Convert.ToDouble(totalSumInsured));


            string filename = "Group_Miro_Official_Receipt_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
            Helper.Alert(false, "No data export.", lblError);
        }
    }
}