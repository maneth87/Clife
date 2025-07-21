using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
public partial class Pages_Reports_policy_commission : System.Web.UI.Page
{
    HSSFWorkbook hssfworkbook = new HSSFWorkbook();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Convertor my_convert = new Convertor();
            //DateTime f = Helper.FormatDateTime(txtFromDate.Text);
            //DateTime t = Helper.FormatDateTime(txtToDate.Text);
            //DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_TO_DO_COMMISSION", new string[,] { { "@From_Date", f + "" }, { "@To_Date", t + "" } });
            //my_convert.ToExcel("text", "MY EXCEL", tbl, this.Context);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
    {

        if(txtFromDate.Text.Trim()=="" || txtToDate.Text.Trim()=="")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('From Date and To Date are required.')", true);
            return;
        }
        else if (txtEntryDate.Text.Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Entry Date is required.')", true);
            return;
        }

        string filename = "Policy_List_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        Response.Clear();

        HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

        DateTime f = Helper.FormatDateTime(txtFromDate.Text);
        DateTime t = Helper.FormatDateTime(txtToDate.Text);
        DateTime entry_date = Helper.FormatDateTime(txtEntryDate.Text);
        DataTable dt = new DataTable();
        dt = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_TO_DO_COMMISSION", new string[,] { { "@From_Date", f + "" }, { "@To_Date", t + "" }, { "@Entry_Date", entry_date+"" } });

        
#region Columns header
        //make a header row
        HSSFRow row1 = (HSSFRow)sheet1.CreateRow(0);
        for (int j = 0; j < dt.Columns.Count; j++)
        {

            HSSFCell cell = (HSSFCell)row1.CreateCell(j);

            String columnName = dt.Columns[j].ToString();

            cell.SetCellValue(columnName);
        }
#endregion

        #region rows
        //loops through data
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 1);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                HSSFCell cell = (HSSFCell)row.CreateCell(j);

                String columnName = dt.Columns[j].ToString();

                cell.SetCellValue(dt.Rows[i][columnName].ToString());
            }
        }
        #endregion

        MemoryStream file = new MemoryStream();
        hssfworkbook.Write(file);

        Response.BinaryWrite(file.GetBuffer());

        Response.End();
    
    }
}