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

public partial class Pages_BroadCastMessage_MessageHistory : System.Web.UI.Page
{
    HSSFWorkbook workbook = new HSSFWorkbook();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnExport1_Click(object sender, EventArgs e)
    {
        List<bl_message_history> message_list = new List<bl_message_history>();
        message_list = da_message_history.GetMessageHistory(Helper.FormatDateTime(txtFromDate.Text.Trim()), Helper.FormatDateTime(txtToDate.Text.Trim()), ddlMessageCate.SelectedValue.Trim(), ddlStatusCode.SelectedValue.Trim());
        
        string filename = "Message_History" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        Response.Clear();

        HSSFSheet sheet1 = (HSSFSheet)workbook.CreateSheet("Data");

        //header 
        HSSFRow row = (HSSFRow)sheet1.CreateRow(0);
        HSSFCell h_cell0 = (HSSFCell)row.CreateCell(0);
        h_cell0.SetCellValue("No.");

        HSSFCell h_cell1 = (HSSFCell)row.CreateCell(1);
        h_cell1.SetCellValue("Message Cate.");

        HSSFCell h_cell2 = (HSSFCell)row.CreateCell(2);
        h_cell2.SetCellValue("Message From");

        HSSFCell h_cell3 = (HSSFCell)row.CreateCell(3);
        h_cell3.SetCellValue("Message To");

        HSSFCell h_cell4 = (HSSFCell)row.CreateCell(4);
        h_cell4.SetCellValue("Message Text");

        HSSFCell h_cell5 = (HSSFCell)row.CreateCell(5);
        h_cell5.SetCellValue("Send Time");

        HSSFCell h_cell6 = (HSSFCell)row.CreateCell(6);
        h_cell6.SetCellValue("Status");

        //rows
        int index = 1;
        foreach (bl_message_history message in message_list)
        {
            HSSFRow row1 = (HSSFRow)sheet1.CreateRow(index);

            //cells
            HSSFCell row_cell0 = (HSSFCell)row1.CreateCell(0);
            row_cell0.SetCellValue(message.No);

            HSSFCell row_cell1 = (HSSFCell)row1.CreateCell(1);
            row_cell1.SetCellValue(message.MessageCate);

            HSSFCell row_cell2 = (HSSFCell)row1.CreateCell(2);
            row_cell2.SetCellValue(message.MessageFrom);

            HSSFCell row_cell3 = (HSSFCell)row1.CreateCell(3);
            row_cell3.SetCellValue(message.MessageTo);

            HSSFCell row_cell4 = (HSSFCell)row1.CreateCell(4);
            row_cell4.SetCellValue(message.MessageText);

            HSSFCell row_cell5 = (HSSFCell)row1.CreateCell(5);
            row_cell5.SetCellValue(message.SendTime);
           
            HSSFCell row_cell6 = (HSSFCell)row1.CreateCell(6);
            row_cell6.SetCellValue(message.StatusCode);
            

            index += 1;
        }

        MemoryStream file = new MemoryStream();
        workbook.Write(file);

        Response.BinaryWrite(file.GetBuffer());

        Response.End();

    }
}