using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using System.Text;
using System.IO;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Pages_Reports_report_policy : System.Web.UI.Page
{
    List<bl_policy_report> list_report_policy = new List<bl_policy_report>();
    HSSFWorkbook hssfworkbook = new HSSFWorkbook();

    int check_form_load_or_search = 0, order_by = 0, payment_mode = 0;
    string policy_number = "", product_id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (txtFrom_date.Text == "" && txtTo_date.Text == "")
        //{
        //    check_form_load_or_search = 1;
        //    ReportPolicy();
        //}
        //else
        //{
        //    check_form_load_or_search = 2;
        //    ReportPolicy();
        //}

       
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        if (DateTime.Parse(txtFrom_date.Text, dtfi).Date > DateTime.Parse(txtTo_date.Text, dtfi).Date)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('From Date (" + txtFrom_date.Text + ") cannot be bigger than To Date (" + txtTo_date.Text + "), please check it again.')", true);
            txtFrom_date.Text = "";
            txtTo_date.Text = "";
            
        }
        else
        {
            check_form_load_or_search = 2;
            
            ReportPolicy();
            txtFrom_date.Text = "";
            txtTo_date.Text = "";
            //Label1.Visible = true;
            //Label2.Visible = true;
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
            else if (int.Parse(RdbOrderBy.SelectedValue) == 1)
            {
                order_by = 2;
            }
            else if (int.Parse(RdbOrderBy.SelectedValue) == 2)
            {
                order_by = 3;
            }
            else
            {
                order_by = 4;
            }
        }
    }

    void ReportPolicy()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        string by_status_code = "";

        if (check_form_load_or_search == 1) /// Form Load
        {
            list_report_policy = da_report_policy.GetPolicyReportList(DateTime.Now, DateTime.Now, "", 1);
        }
        else
        {
            CheckOrderBy();

            foreach (GridViewRow item in GvoPolicyStatus.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.CheckBox rbtnlAnswer = (System.Web.UI.WebControls.CheckBox)item.FindControl("ckb1");
                    if (rbtnlAnswer.Checked == true)
                    {
                        HiddenField hdfSeqNumber = (HiddenField)item.FindControl("hdfPolicy_Status_ID");

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

            list_report_policy = da_report_policy.test_report_policy(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), by_status_code, order_by, int.Parse(ddlPaymentMod.SelectedValue), txtPolicyNumber.Text, ddlProduct.SelectedValue);

            lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Policy Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + txtFrom_date.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";

            lblfrom1.Text = txtFrom_date.Text;
            lblto1.Text = txtTo_date.Text;
        }

        StringBuilder strDeviceList = new StringBuilder();
       // string strTable = "<table class=\"grid-layout\" width=\"100%\" border=\"1\">";
        string strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
        strTable += "<tr><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Pol.no.</th><th styple=\"text-align: center; \">Insured's name</th><th style=\"text-align: center;\">Product</th><th style=\"text-align: center;\">Comm Rate</th><th style=\"text-align: center;\">Plan</th><th styple=\"text-align: center; \">Stat.</th><th styple=\"text-align: center; \">Issued date</th><th style=\"text-align: center;\">Eff.date</th><th style=\"text-align: center;\"> Next due</th><th style=\"text-align: center;\">Mode</th><th style=\"text-align: center; width=\"0px;\" \">S.A.</th><th style=\"text-align: center;\">Total Premium</th><th style=\"text-align: center;\">Premium rate</th><th style=\"text-align: center;\">Last payment</th><th style=\"text-align: center; padding-left:5px \">Agent</th></tr>"; //<th style=\"text-align: center;\">Address</th>

        for (int i = 0; i < list_report_policy.Count; i++) // Loop through List with for
        {
            bl_policy_report policy_report = new bl_policy_report();

            policy_report.Policy_Number = list_report_policy[i].Policy_Number.ToString();
            policy_report.customer_name = list_report_policy[i].customer_name.ToString();
            policy_report.En_Abbr = list_report_policy[i].En_Abbr.ToString();
            policy_report.StatusCode = list_report_policy[i].StatusCode.ToString();
            policy_report.Issue_Date = DateTime.Parse(list_report_policy[i].Issue_Date.ToString());
            policy_report.Effective_Date = DateTime.Parse(list_report_policy[i].Effective_Date.ToString());
            policy_report.Next_Due_Date = DateTime.Parse(list_report_policy[i].Next_Due_Date.ToString());
            policy_report.Mode = list_report_policy[i].Mode.ToString();
            policy_report.Sum_Insure = float.Parse(list_report_policy[i].Sum_Insure.ToString());
            policy_report.Total_Premium = float.Parse(list_report_policy[i].Total_Premium.ToString());
            policy_report.Is_Standard = list_report_policy[i].Is_Standard.ToString();
            policy_report.Last_Payment = DateTime.Parse(list_report_policy[i].Last_Payment.ToString());
            policy_report.Agent = list_report_policy[i].Agent.ToString();
            policy_report.Address1 = list_report_policy[i].Address1.ToString();
            policy_report.Product_ID = list_report_policy[i].Product_ID.ToString();
            policy_report.Comm_Rate = float.Parse(list_report_policy[i].Comm_Rate.ToString());

            strTable += "<tr>";
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (i + 1) + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + "</td>";

            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.customer_name + "</td>";

            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Product_ID + "</td>";

            if (policy_report.Comm_Rate > 0)
            {
                strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Comm_Rate + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "---" + "</td>";
            }

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.En_Abbr + "</td>";

            strTable += "<td style=\"text-align: center; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.StatusCode + "</td>";

            //if (DateTime.Parse(policy_report.Issue_Date.ToString()).ToString("yyyy/MM/dd") != DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy/MM/dd"))
            //{
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Issue_Date).ToString("dd-MM-yyyy") + "</td>";
            //}
            //else
            //{
            //    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " " + "</td>";
            //}

            //if (DateTime.Parse(policy_report.Effective_Date.ToString()).ToString("yyyy/MM/dd") != DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy/MM/dd"))
            //{
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";
            //}
            //else
            //{
            //    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + "</td>";
            //}

            //if (DateTime.Parse(policy_report.Next_Due_Date.ToString()).ToString("yyyy/MM/dd") != DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy/MM/dd"))
            //{
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Next_Due_Date).ToString("dd-MM-yyyy") + "</td>";
            //}
            //else
            //{
            //    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " " + "</td>";
            //}


            strTable += "<td style=\"text-align: center; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Mode + "</td>";

            if (policy_report.Sum_Insure > 0)
            {
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Sum_Insure + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " " + "</td>";
            }

            if (policy_report.Total_Premium > 0)
            {
                strTable += "<td style=\"text-align: center; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Total_Premium + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: center; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + "</td>";
            }

            strTable += "<td style=\"text-align: center; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Is_Standard + "</td>";

            //if (DateTime.Parse(policy_report.Next_Due_Date.ToString()).ToString("yyyy/MM/dd") != DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy/MM/dd"))
            //{
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Last_Payment).ToString("dd-MM-yyyy") + "</td>";
            //}
            //else
            //{
            //    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + "</td>";
            //}

            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px;  width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Agent + "</td>";

            // strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px;  width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Address1 + "</td>";

            strTable += "</tr>";
        }

        strTable += "</table>";

        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append(strTable);

        AppendDivPolicy.InnerHtml = strBuilder.ToString();

        if (check_form_load_or_search != 1)
        {
            hdfPaymentMode.Value = ddlPaymentMod.SelectedValue;
            hdfPolicyNum.Value = txtPolicyNumber.Text;
            hdfProduct.Value = ddlProduct.SelectedValue;
        }

        /// Clear value 
        RdbOrderBy.SelectedIndex = 0;
        ddlProduct.SelectedIndex = 0;
        ddlPaymentMod.SelectedIndex = 0;
        txtPolicyNumber.Text = "";
    }

    private MemoryStream WriteToStream()
    {
        //Write the stream data of workbook to the root directory
        MemoryStream file = new MemoryStream();
        hssfworkbook.Write(file);
        return file;
    }

    protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        string by_status_code = "";

        //just need a straight data dump

        if (lblfrom.Text != "")
        {
            DataTable dt2 = new DataTable();

            policy_number = hdfPolicyNum.Value;
            product_id = hdfProduct.Value;
            payment_mode = int.Parse(hdfPaymentMode.Value);

            DataRow r2;

            dt2.Columns.Add("Policy Number");
            dt2.Columns.Add("Insured's name");
            dt2.Columns.Add("Plan");
            dt2.Columns.Add("Stat.");
            dt2.Columns.Add("Issue date");
            dt2.Columns.Add("Effective date");
            dt2.Columns.Add("Next due");
            dt2.Columns.Add("Mode");
            dt2.Columns.Add("S.A.");
            dt2.Columns.Add("Total Premium");
            dt2.Columns.Add("Premium Rate");
            dt2.Columns.Add("Last Payment");
            dt2.Columns.Add("Agent");
            //da_report_policy.Report_Policy_Without_Next_Due(DateTime.Parse(lblfrom1.Text, dtfi), DateTime.Parse(lblto1.Text, dtfi), by_status_code, order_by, payment_mode, policy_number, product_id);
            list_report_policy = da_report_policy.test_report_policy(DateTime.Parse(lblfrom1.Text, dtfi), DateTime.Parse(lblto1.Text, dtfi), by_status_code, 4, payment_mode, policy_number, product_id);
            ///
            for (int i = 0; i < list_report_policy.Count; i++) // Loop through List with for
            {
                r2 = dt2.NewRow();

                r2["Policy Number"] = list_report_policy[i].Policy_Number.ToString();
                r2["Insured's name"] = list_report_policy[i].customer_name.ToString();
                r2["Plan"] = list_report_policy[i].En_Abbr.ToString();
                r2["Stat."] = list_report_policy[i].StatusCode.ToString();
                r2["Issue date"] = DateTime.Parse(list_report_policy[i].Issue_Date.ToString());
                r2["Effective date"] = DateTime.Parse(list_report_policy[i].Effective_Date.ToString());
                r2["Next due"] = DateTime.Parse(list_report_policy[i].Next_Due_Date.ToString());
                r2["Mode"] = list_report_policy[i].Mode.ToString();
                r2["S.A."] = float.Parse(list_report_policy[i].Sum_Insure.ToString());
                r2["Total Premium"] = float.Parse(list_report_policy[i].Total_Premium.ToString());
                r2["Premium Rate"] = list_report_policy[i].Is_Standard.ToString();
                r2["Last Payment"] = DateTime.Parse(list_report_policy[i].Last_Payment.ToString());
                r2["Agent"] = list_report_policy[i].Agent.ToString();

                dt2.Rows.Add(r2);
            }
            ///

            DataView dt_view = dt2.DefaultView;
            dt_view.Sort = "Policy Number ASC";
            DataTable dt = dt_view.ToTable(); //da_report_policy.Report_Policy_Without_Next_Due(DateTime.Parse(lblfrom1.Text, dtfi), DateTime.Parse(lblto1.Text, dtfi), by_status_code, order_by, payment_mode, policy_number, product_id);
            if (dt.Rows.Count > 0)
            {
                string filename = "PolicyReport.xls";
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                Response.Clear();

                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                //make a header row

                HSSFRow row1 = (HSSFRow)sheet1.CreateRow(0);

                //Puts in headers (these are table row headers, omit if you

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
        }
        else {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('There is no data, please check it again.')", true);
        }
    }

    //protected void ImagPrintPDF_Click(object sender, ImageClickEventArgs e)
    //{
    //    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
    //    dtfi.ShortDatePattern = "dd/MM/yyyy";
    //    dtfi.DateSeparator = "/";
    //    string by_status_code = "";

    //    //just need a straight data dump

    //    if (lblfrom.Text != "")
    //    {
    //        policy_number = hdfPolicyNum.Value;
    //        product_id = hdfProduct.Value;
    //        payment_mode = int.Parse(hdfPaymentMode.Value);

    //        DataTable dt = da_report_policy.Report_Policy_Without_Next_Due(DateTime.Parse(lblfrom.Text, dtfi), DateTime.Parse(lblto.Text, dtfi), by_status_code, order_by, payment_mode, policy_number, product_id);
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