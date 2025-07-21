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
using System.Data;
using System.Web.Security;

public partial class Pages_Reports_banca_micro_payment : System.Web.UI.Page
{
    class my_sesstion
    {
        public static string MESSAGE { get; set; }
        public static bool VALID { get; set; }
        public static List<da_banca.PaymentHTB.PaymentHTBReport> DATA { get; set; }
        public static string USER { get; set; }
        public static string ROLE { get; set; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            
            btnExport.Attributes.Add("disabled", "disabled");
            BindChannelItem();
        }
    }
    void BindChannelItem()
    {
        //cooperate id=0152DF80-BA95-46A9-BB7A-E71966A34089
        List<bl_channel_item> ch = da_channel.GetChannelItemListByChannel("0152DF80-BA95-46A9-BB7A-E71966A34089");
        ddlChannelItem.Items.Clear();
        Options.Bind(ddlChannelItem, ch, "Channel_Name", "Channel_Item_ID", 0);
    
    }

    void BindChannelLocation(string CHANNEL_ITEM_ID)
    {
        List<bl_channel_location> ch = da_channel.GetChannelLocationListByChannelItemID(CHANNEL_ITEM_ID);// (ddlChannelItem.SelectedValue);
        ddlChannelLocation.Items.Clear();
        Options.Bind(ddlChannelLocation, ch, "Office_Name", "Channel_Location_ID", 0);
    }
    void BindData()
    {
        List<da_banca.PaymentHTB.PaymentHTBReport> pay = new List<da_banca.PaymentHTB.PaymentHTBReport>();
        pay = da_banca.PaymentHTB.GetPaymentReport(Helper.FormatDateTime(txtDateFrom.Text.Trim()), Helper.FormatDateTime(txtDateTo.Text.Trim()), ddlChannelItem.SelectedValue, ddlChannelLocation.SelectedValue);
        my_sesstion.DATA = pay;
        if (da_banca.SUCCESS)
        {
            gv_valid.DataSource = pay;
            gv_valid.DataBind();
            lblRecords.Text = pay.Count + " Record(s) Found.";
            if (pay.Count > 0)
            {
                btnExport.Attributes.Remove("disabled");

            }
            else
            {
                btnExport.Attributes.Add("disabled", "disabled");
            }
        }
        else
        {
            Alert(true, da_banca.MESSAGE);
        }
        
    }

    void ValidateForm()
    {
        if (txtDateFrom.Text.Trim() == "")
        {
            my_sesstion.VALID = false;
            my_sesstion.MESSAGE = "Payment Date From is requied.";
        }
        else if (txtDateTo.Text.Trim() == "")
        {
            my_sesstion.VALID = false;
            my_sesstion.MESSAGE = "Payment Date To is requied.";
        }
        else if(!Helper.IsDate(txtDateFrom.Text.Trim()))
        {
         my_sesstion.VALID = false;
            my_sesstion.MESSAGE = "Payment Date from is invalid format.";
        }
        else if (!Helper.IsDate(txtDateTo.Text.Trim()))
        {
            my_sesstion.VALID = false;
            my_sesstion.MESSAGE = "Payment Date To is invalid format.";
        }
        else
        {
            my_sesstion.VALID = true;
            my_sesstion.MESSAGE = "";
        }
    }
    void Alert(bool IS_ERROR, string MESSAGE)
    {
        Helper.Alert(IS_ERROR , MESSAGE, lblError);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ValidateForm();
        if (my_sesstion.VALID)
        {
            BindData();
        }
        else
        {
            Alert(true, my_sesstion.MESSAGE);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
            Helper.excel.Sheet = sheet1;
            Helper.excel.HeaderText = new string[] {
            "Company Name","Branch Code", "Brand Name", "Payment Reference No.", "Transaction Type", "Insurance Application Number", "Client Name", "Premium", "Currency","Payment Date"
         };

            Helper.excel.generateHeader();
            int row_no = 0;
            foreach (da_banca.PaymentHTB.PaymentHTBReport pay in my_sesstion .DATA)
            {
                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(pay.CHANNEL_NAME);

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(pay.BranchCode);

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(pay.BranchName);

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(pay.PaymentReferenceNo);

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue(pay.TransactionType);

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(pay.InsuranceApplicationId);

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(pay.ClientNameENG);

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue(pay.Premium);

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                Cell9.SetCellValue(pay.Currency);

                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                Cell10.SetCellValue(pay.PaymentDate.ToString("dd-MM-yyyy"));


            }
            string filename = "banca_micro_payment_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            
            Response.BinaryWrite(file.GetBuffer());

            Response.End();
        }
        catch (Exception ex)
        {
            Alert(true, ex.Message);
        }
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindChannelLocation(((DropDownList)sender).SelectedValue);
    }
}