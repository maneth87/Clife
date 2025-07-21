using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
using System.Web.Security;

public partial class Pages_Reports_group_micro_premium_detail_req : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));
    }

    private List<Report.GroupMicro.PremiumDetail> PremiumDetail { get { return (List<Report.GroupMicro.PremiumDetail>)ViewState["VS_PREM_LIST"]; } set { ViewState["VS_PREM_LIST"] = value; } }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (!Page.IsPostBack)
        {
     
            btnExport.Enabled = false;
            if (!Helper.BindChannel(ddlChannel))
            {
                Helper.Alert(true, "Bind Channel Error", lblError);
            }
            else
            {
                ddlChannel.SelectedIndex = 2;
                ddlChannel_SelectedIndexChanged(null, null);
                ddlChannel.Enabled = false;
            }
        }
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

        try
        {
            
                double totalLoanAmountKh = 0;
                double totalLoanAmount = 0;
                double totalPremiumkh = 0;
                double totalPremium = 0;
                double totalLoanAmountUSD = 0;
                List<Report.GroupMicro.PremiumDetail> filterObj = new List<Report.GroupMicro.PremiumDetail>();
                filterObj = PremiumDetail;
                if (filterObj.Count > 0)
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                    Response.Clear();
                    HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                    var pre = filterObj[0];

                    Helper.excel.Sheet = sheet1;
                    Helper.excel.Title = new string[] { "Customer Premium Details", pre.ChannelName, pre.GroupCode };
                    Helper.excel.HeaderText = new string[]
                        {
                        "Issued Date", "Customer ID", "Policy Number",  "Agreement Number", "Customer Name","Gender", "DOB", "Age",  
                        "Effective Date","Expiry Date", "Period (Days)", "Loan Amount", "Currency", "Loan Amount USD","Rate", "Premium Amount KH", "Premium Amount USD", "Policy Status", "Policy Status Date"
                        };

                    Helper.excel.generateHeader();

                    int row_no = 0;
                    row_no = Helper.excel.NewRowIndex - 1;
                    foreach (Report.GroupMicro.PremiumDetail obj in filterObj)//foreach (DataRow r in my_session.DATA.Rows)
                    {
                        totalLoanAmount += obj.LoanAmountUSD;
                        totalLoanAmountKh += obj.LoanAmount;
                        totalPremium += obj.Premium;
                        totalPremiumkh += obj.ExchangeRate>0 ?  obj.Premium * obj.ExchangeRate : 0;//  obj.Premium * obj.ExchangeRate;
                        totalLoanAmountUSD += obj.LoanAmountUSD;
                        row_no += 1;
                        HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                        HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                        Cell1.SetCellValue(obj.SubmittedDate.ToString("dd-MMM-yyyy"));

                        HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                        Cell2.SetCellValue(obj.CustomerNo);

                        HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                        Cell3.SetCellValue(obj.PolicyNumber);

                        HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                        Cell4.SetCellValue(obj.AgreementNumber);

                        HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                        Cell5.SetCellValue(obj.CustomerName);

                        HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                        Cell6.SetCellValue(obj.Gender);

                        HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                        Cell7.SetCellValue(obj.DOB.ToString("dd-MMM-yyyy"));

                        HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                        Cell8.SetCellValue(obj.Age);

                        HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                        Cell9.SetCellValue(obj.EffectivedDate.ToString("dd-MMM-yyyy"));

                        HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                        Cell10.SetCellValue(obj.ExpireDate.ToString("dd-MMM-yyyy"));

                        HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                        Cell11.SetCellValue(obj.CoveragePeriod);

                        HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                        Cell12.SetCellValue(obj.LoanAmount);

                        HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                        Cell13.SetCellValue(obj.Currency);

                        HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                        Cell14.SetCellValue(obj.LoanAmountUSD);

                        HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                        Cell15.SetCellValue(obj.PremiumRate);

                        HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                        Cell16.SetCellValue(obj.PremiumKh);// Math.Round(obj.Premium * obj.ExchangeRate, MidpointRounding.AwayFromZero));

                        HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                        Cell17.SetCellValue(obj.Premium);

                        HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                        Cell18.SetCellValue(obj.PolicyStatus);

                        HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                        Cell19.SetCellValue(obj.PolicyStatusDate.Year==2000? "" :obj.PolicyStatusDate.ToString("dd-MMM-yyyy"));

                        //HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                        //Cell17.SetCellValue(obj.ReportDate.ToString("dd-MMM-yyyyy"));

                    }

                    HSSFRow rowCell1 = (HSSFRow)sheet1.CreateRow(row_no + 1);
                    HSSFCell Celltotal = (HSSFCell)rowCell1.CreateCell(12);
                    Celltotal.SetCellValue("Total Premium");

                    //HSSFCell CellTotalLoanAmountKh = (HSSFCell)rowCell1.CreateCell(11);
                    //CellTotalLoanAmountKh.SetCellValue(totalLoanAmountKh);

                    HSSFCell CellTotalLoanAmount = (HSSFCell)rowCell1.CreateCell(13);
                    CellTotalLoanAmount.SetCellValue(totalLoanAmount);

                    HSSFCell CellTotalPremiumKh = (HSSFCell)rowCell1.CreateCell(15);
                    CellTotalPremiumKh.SetCellValue(totalPremiumkh);

                    HSSFCell CellTotalPremium = (HSSFCell)rowCell1.CreateCell(16);
                    CellTotalPremium.SetCellValue(Convert.ToDouble(totalPremium));

                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports premium detial [Total record(s):", filterObj.Count, "]."));
                    string filename = "Group_Miro_Premium_Detail_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
                    Helper.Alert(false, "No data to export.", lblError);
                }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }

    }
   
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            int countRow = 0;
            
            gv_valid.PageIndex = e.NewPageIndex;
            gv_valid.DataSource = PremiumDetail;
           
            gv_valid.DataBind();
            if (gv_valid.PageCount == e.NewPageIndex + 1)//last page
            {
                countRow = gv_valid.PageSize * (e.NewPageIndex) + gv_valid.Rows.Count;
            }
            else
            {
                countRow = gv_valid.PageSize * (e.NewPageIndex + 1);
            }
            lblRecords.Text = "Record(s): " + countRow + " of " + PremiumDetail.Count;
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error:" + ex.Message, lblError);
        }
    }
    protected void gv_valid_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtReportDateFrom.Text.Trim() == "" || !(Helper.IsDate(txtReportDateFrom.Text.Trim())))
            {
                Helper.Alert(true, "Report Date From is required with format [DD-MM-YYYY].", lblError);
            }
            else if (txtReportDateFrom.Text.Trim() == "" || !(Helper.IsDate(txtReportDateFrom.Text.Trim())))
            {
                Helper.Alert(true, "Report Date To is required with format [DD-MM-YYYY].", lblError);
            }
            else if (ddlChannel.SelectedValue == "")
            {
                Helper.Alert(true, "Channel is required.", lblError);
            }
            else
            {
              
                List<Report.GroupMicro.PremiumDetail> listObj = new List<Report.GroupMicro.PremiumDetail>();
                List<Report.GroupMicro.PremiumDetail> filterObj = new List<Report.GroupMicro.PremiumDetail>();
                listObj = (List<Report.GroupMicro.PremiumDetail>)Report.GroupMicro.GetPremiumDetail(Helper.FormatDateTime(txtReportDateFrom.Text.Trim()), Helper.FormatDateTime(txtReportDateTo.Text.Trim()),ddlChannelItem.SelectedValue, ddlProduct.SelectedValue , 2);

                //Filter by channel & channel item
                //foreach (Report.GroupMicro.PremiumDetail obj in listObj.Where(_ => _.ChannelId == ddlChannel.SelectedValue && (String.IsNullOrEmpty(ddlChannelItem.SelectedValue) || _.ChannelItemId == ddlChannelItem.SelectedValue) ))
                //{
                //    filterObj.Add(obj);
                //}

                filterObj = listObj;

                if (filterObj.Count > 0)
                {
                    PremiumDetail = filterObj;
                    gv_valid.DataSource = filterObj;
                    gv_valid.DataBind();
                    btnExport.Enabled = true;
                    lblRecords.Text = "Record(s): " + gv_valid.Rows.Count + " of " + PremiumDetail.Count;

                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries premium detail with criteria [Payment Date From:", txtReportDateFrom.Text.Trim(), " To:", txtReportDateTo.Text.Trim(), "; Company:", ddlChannelItem.SelectedItem.Text, "; Product Id:", ddlProduct.SelectedValue, "]."));

                }
                else
                {
                    gv_valid.DataSource = null;
                    gv_valid.DataBind();

                    lblRecords.Text = "No Record Found";
                    btnExport.Enabled = false;
                    Helper.Alert(false, "No Record Found.", lblError);
                }
            }
        }
        catch (Exception ex)
        {
            PremiumDetail = null;
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            Helper.Alert(true, "Search Data is getting error: "+ ex.Message,lblError);
        }
    }
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*bind saleable product list */
        ddlProduct.Items.Clear();
        List<bl_micro_product_config> proConList = da_micro_product_config.ProductConfig.GetMicroProductConfigListByChannelItemId(ddlChannelItem.SelectedValue, true);
        Options.Bind(ddlProduct, proConList, bl_micro_product_config.NAME.MarketingName, bl_micro_product_config.NAME.Product_ID, 0, "--- Select ---");

        if (ddlProduct.Items.Count == 2)
        {
            ddlProduct.SelectedIndex = 1;
            ddlProduct.Enabled = false;

        }
        else
        {
            ddlProduct.Enabled = true;
            ddlProduct.SelectedIndex = 0;
        }
    }
}