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

public partial class Pages_BroadCastMessage_MessageIn : System.Web.UI.Page
{
    HSSFWorkbook workbook = new HSSFWorkbook();
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        string result = "<table class='table table-bordered'><tr><th>No.</th><th>Send Time</th><th>Message From</th><th>Message To</th><th>Message Text</th></tr>";
        List<bl_message_in> arr_message = new List<bl_message_in>();
        try
        {
            int records = 0;
            arr_message = da_message_in.GetMessageIn(Helper.FormatDateTime(txtFromDate.Text.Trim()), Helper.FormatDateTime(txtToDate.Text.Trim()));
           
            foreach (bl_message_in message in arr_message)
            {
                records += 1;
                result += "<tr><td>" + message.No + "</td><td>" + message.SendTime + "</td><td>" + message.MessageFrom + "</td><td>" + message.MessageTo + "</td><td>" + message.MessageText + "</td></tr>";
            }
            result += "</table>";
            div_result.InnerHtml = result;
            if (records > 0)
            {
                btnExport.Attributes.CssStyle.Add("display", "block");
            }
            else
            {
                btnExport.Attributes.CssStyle.Add("display", "none");
            }
            Session["SS_MESSAGE_LIST"] = arr_message;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnSearch1_Click] in page [MessageIn.aspx.cs], Detail: " + ex.Message);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        List<bl_message_in> arr_message = new List<bl_message_in>();
        arr_message = (List<bl_message_in>)Session["SS_MESSAGE_LIST"];

        string filename = "Message_Received" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        Response.Clear();

        HSSFSheet sheet1 = (HSSFSheet)workbook.CreateSheet("Sheet 1");
        //make a header row
        HSSFRow row = (HSSFRow)sheet1.CreateRow(0);
        HSSFCell h_cell0 = (HSSFCell)row.CreateCell(0);
        h_cell0.SetCellValue("No.");

        HSSFCell h_cell1 = (HSSFCell)row.CreateCell(1);
        h_cell1.SetCellValue("Send Time");

        HSSFCell h_cell2 = (HSSFCell)row.CreateCell(2);
        h_cell2.SetCellValue("Message From");

        HSSFCell h_cell3 = (HSSFCell)row.CreateCell(3);
        h_cell3.SetCellValue("Message To");

        HSSFCell h_cell4 = (HSSFCell)row.CreateCell(4);
        h_cell4.SetCellValue("Message Text");

        //rows
        int index = 1;
        foreach (bl_message_in message in arr_message)
        {
            //rows
            HSSFRow row1 = (HSSFRow)sheet1.CreateRow(index);

            //cells
            HSSFCell row_cell0 = (HSSFCell)row1.CreateCell(0);
            row_cell0.SetCellValue(message.No);

            HSSFCell row_cell1 = (HSSFCell)row1.CreateCell(1);
           
            row_cell1.SetCellValue(message.SendTime);

            HSSFCell row_cell2 = (HSSFCell)row1.CreateCell(2);
            row_cell2.SetCellValue(message.MessageFrom);

            HSSFCell row_cell3 = (HSSFCell)row1.CreateCell(3);
            row_cell3.SetCellValue(message.MessageTo);

            HSSFCell row_cell4 = (HSSFCell)row1.CreateCell(4);
            row_cell4.SetCellValue(message.MessageText);

            index += 1;
        }
       
        MemoryStream file = new MemoryStream();
        workbook.Write(file);

        Response.BinaryWrite(file.GetBuffer());

        Response.End();
    }
    protected void btnClear1_Click(object sender, EventArgs e)
    {
        Session.Clear();

        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
       div_result.InnerHtml = "";
        btnExport.Attributes.CssStyle.Add("display","none");

        
    }
}