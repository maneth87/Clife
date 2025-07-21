using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.Text;
using System.Globalization;
using System.Data;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;


public partial class Pages_Reports_report_application : System.Web.UI.Page
{
    List<bl_report_application> list_report_application = new List<bl_report_application>();
    HSSFWorkbook hssfworkbook = new HSSFWorkbook();

    int check_form_load_or_search = 0, order_by = 0;
  
    //Public property for passing data to other pages
    #region
    public string Check_Form_Load_Or_Search
    {
        get
        {
            return hdfCheckFormLoadOrSearch.Value;
        }
    }

    public string Status_Code
    {
        get
        {
            return hdfPolicyStatus.Value;
        }
    }

    public string Order_By
    {
        get
        {
            return hdfOrderBy.Value;
        }
    }

    public string From_Date
    {
        get
        {
            return hdfFromDate.Value;
        }
    }

    public string To_Date
    {
        get
        {
            return hdfToDate.Value;
        }
    }

    public string Chart_Type
    {
        get
        {
            return hdfChartType.Value;
        }
    }

    public string Chart_Data
    {
        get
        {
            return hdfChartData.Value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (txtFrom_date.Text == "" && txtTo_date.Text == "")
        {
            check_form_load_or_search = 1;
            ReportApplication();
        }
        else
        {
            check_form_load_or_search = 2;
            ReportApplication();
        }
    }

    void CheckOrderBy()
    {
        if (RdbOrderBy.SelectedValue != "")
        {
            if (int.Parse(RdbOrderBy.SelectedValue) == 0)
            {
                order_by = 1;

            }
            else
            {
                order_by = 2;
            }
        }
    }

    void ReportApplication()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        string by_status_code = "";

        if (check_form_load_or_search == 1)
        {
            list_report_application = da_report_application.GetAppReportList(DateTime.Now, DateTime.Now, "", 1);
        }
        else
        {
            CheckOrderBy();

            foreach (GridViewRow item in GvoApplicationStatus.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.CheckBox rbtnlAnswer = (System.Web.UI.WebControls.CheckBox)item.FindControl("ckb1");
                    if (rbtnlAnswer.Checked == true)
                    {
                        HiddenField hdfSeqNumber = (HiddenField)item.FindControl("hdfApplicationCode");

                        by_status_code += "'" + hdfSeqNumber.Value + "',";

                        //by_status_code += hdfSeqNumber.Value + ",";

                        rbtnlAnswer.Checked = false;
                    }
                }
            }

            if (by_status_code.Length > 0)
            {
                by_status_code = by_status_code.Remove(by_status_code.Length - 1, 1);
            }

            list_report_application = da_report_application.GetAppReportList(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), by_status_code, order_by);

            if (list_report_application.Count > 0)
            {
                lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Application Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + txtFrom_date.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";

                lblfrom1.Text = txtFrom_date.Text;
                lblto1.Text = txtTo_date.Text;
            }
            else
            {
                lblfrom.Text = "";
                lblfrom1.Text = "";
                lblto1.Text = "";
            }
            
        }

        StringBuilder strDeviceList = new StringBuilder();


        string strTable = "";

        if (list_report_application.Count > 0)
        {
            strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
            strTable += "<tr><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">App.no.</th><th styple=\"text-align: center; \">Applicant's name</th><th style=\"text-align: center;\">Reg.date</th><th styple=\"text-align: center; \">S.A.</th><th styple=\"text-align: center; \">Premium</th><th style=\"text-align: center;\">Plan</th><th style=\"text-align: center;\">Status</th><th style=\"text-align: center;\">Status date</th><th style=\"text-align: center; width=\"0px;\" \">Agent</th></tr>";
        }
        else
        {
            strTable = "";
        }

        for (int i = 0; i < list_report_application.Count; i++) // Loop through List with for
        {
            bl_report_application app_report = new bl_report_application();

            app_report.App_Number = list_report_application[i].App_Number.ToString();
            app_report.customer_name = list_report_application[i].customer_name.ToString();
            app_report.App_Date =list_report_application[i].App_Date.ToString();
            app_report.User_Sum_Insure =float.Parse(list_report_application[i].User_Sum_Insure.ToString());
            app_report.User_Premium = float.Parse(list_report_application[i].User_Premium.ToString());
            app_report.En_Abbr = list_report_application[i].En_Abbr.ToString();
            app_report.Status_Code = list_report_application[i].Status_Code.ToString();
            app_report.Status_date =list_report_application[i].Status_date.ToString();
            app_report.agent_name = list_report_application[i].agent_name.ToString();

            strTable += "<tr>";
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (i+1) + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + app_report.App_Number + "</td>";

            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + app_report.customer_name + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + app_report.App_Date + "</td>";

            strTable += "<td style=\"text-align: center; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + app_report.User_Sum_Insure + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + app_report.User_Premium + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + app_report.En_Abbr + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + app_report.Status_Code + "</td>";

            strTable += "<td style=\"text-align: center; width=\"0px;\"  class=\"tb-schedule-td-right\">" + app_report.Status_date + "</td>";

            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + app_report.agent_name + "</td>";

            strTable += "</tr>";
        }

        StringBuilder strBuilder = new StringBuilder();

        divExport.Style.Clear();

        if (list_report_application.Count > 0)
        {
            strTable += "</table>";
            strBuilder.Append(strTable);
            divExport.InnerHtml = strBuilder.ToString();
        }
        else
        {
            strBuilder.Append("Please filter your search....");            
            divExport.InnerHtml = strBuilder.ToString();
            divExport.Style.Add("color", "#3399ff");
            divExport.Style.Add("Font-Weight", "bold");
        }

        //Set value to hidden fields for passing data to other pages
        hdfFromDate.Value = txtFrom_date.Text.Trim();
        hdfToDate.Value = txtTo_date.Text.Trim();
    
        hdfOrderBy.Value = RdbOrderBy.SelectedValue;
        hdfPolicyStatus.Value = by_status_code;
        hdfCheckFormLoadOrSearch.Value = check_form_load_or_search.ToString();
    }

    private MemoryStream WriteToStream()
    {
        //Write the stream data of workbook to the root directory
        MemoryStream file = new MemoryStream();
        hssfworkbook.Write(file);
        return file;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        //Label1.Visible = true;
        //Label2.Visible = true;
    }

    protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        string by_status_code = "";

        if (lblfrom.Text != "")
        {
            string filename = "AppReport.xls";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

            //make a header row

            HSSFRow row1 = (HSSFRow)sheet1.CreateRow(0);

            //Puts in headers (these are table row headers, omit if you

            //just need a straight data dump
            DataTable dt = new DataTable();
            dt = da_report_application.Get_Dt_AppReportList(DateTime.Parse(lblfrom1.Text, dtfi), DateTime.Parse(lblto1.Text, dtfi), by_status_code, order_by);


            for (int j = 0; j < dt.Columns.Count; j++)
            {

                HSSFCell cell = (HSSFCell)row1.CreateCell(j);

                String columnName = dt.Columns[j].ToString();

                cell.SetCellValue(columnName);
            }

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

            //writing the data to binary from memory

            Response.BinaryWrite(WriteToStream().GetBuffer());

            Response.End();
        } 
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('There is no data, please check it again.')", true);
        }
    }

    //Create Chart
    protected void btnCreateChart_Click(object sender, EventArgs e)
    {
        hdfChartType.Value = ddlType.SelectedValue;
        hdfChartData.Value = rbtnlData.SelectedValue;

        Server.Transfer("~/Pages/Chart/application_chart.aspx");
    }

    //protected void ImgPrintPDF_Click(object sender, ImageClickEventArgs e)
    //{
    //    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
    //    dtfi.ShortDatePattern = "dd/MM/yyyy";
    //    dtfi.DateSeparator = "/";
    //    string by_status_code = "";

    //    //just need a straight data dump

    //    if (lblfrom.Text != "")
    //    {
    //        DataTable dt = dt = da_report_application.Get_Dt_AppReportList(DateTime.Parse(lblfrom.Text, dtfi), DateTime.Parse(lblto.Text, dtfi), by_status_code, order_by);
    //        if (dt.Rows.Count > 0)
    //        {
    //            Document pdfDoc = new Document(PageSize.A4.Rotate()); // Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
    //            pdfDoc.SetMargins(-80, -80, 10, 0);
    //            PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
    //            pdfDoc.Open();
    //            string title_report = "Report Policy" + " " + (DateTime.Parse(lblfrom.Text, dtfi).ToString("yyyy-MM-dd")) + " to " + DateTime.Parse(lblto.Text, dtfi).ToString("yyyy-MM-dd");
    //            Chunk c = new Chunk
    //                ("" + title_report + "",
    //                FontFactory.GetFont("Verdana", 15));
    //            Paragraph p = new Paragraph();
    //            p.Alignment = Element.ALIGN_CENTER;
    //            p.Add(c);
    //            pdfDoc.Add(p);
    //            try
    //            {
    //                Font fnt1 = FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.BOLD);
    //                Font fnt = FontFactory.GetFont("Times New Roman", 9);
    //                if (dt != null)
    //                {
    //                    PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
    //                    PdfPCell PdfPCell = null;
    //                    for (int rows = 0; rows < dt.Rows.Count; rows++)
    //                    {
    //                        if (rows == 0)
    //                        {
    //                            for (int column = 0; column < dt.Columns.Count; column++)
    //                            {

    //                                PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[column].ColumnName.ToString(), fnt1)));
    //                                PdfTable.AddCell(PdfPCell);
    //                            }
    //                        }
    //                    }
    //                    for (int rows = 0; rows < dt.Rows.Count; rows++)
    //                    {
    //                        for (int column = 0; column < dt.Columns.Count; column++)
    //                        {
    //                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), fnt)));
    //                            PdfTable.AddCell(PdfPCell);
    //                        }
    //                    }
    //                    pdfDoc.Add(PdfTable);
    //                }
    //                pdfDoc.Close();
    //                Response.ContentType = "application/pdf";
    //                Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");
    //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //                Response.Flush();
    //                Response.End();
    //            }
    //            catch (Exception ex)
    //            {
    //                Response.Write(ex.ToString());
    //            }
    //        }
    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('There is no data, please check it again.')", true);
    //    }
    //}

}