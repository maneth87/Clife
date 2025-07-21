using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
public partial class Pages_CI_frmLoadTerminateReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string[] para = new string[] { };
            para = (string[])Session["PARAS"];
            if (para != null)
            {
                if (para.Length > 0)
                {
                    if (para[0] == "")
                    {
                        para[0] = "01/01/1900";//set default date
                    }
                    if (para[1] == "")
                    {
                        para[1] = "01/01/1900";//set default date
                    }
                    //Filter policy status = IF or Lap in datatable
                    DataTable tbl;

                    try
                    {
                        tbl = da_ci.GetTerminatedPolicy(Helper.FormatDateTime(para[0]), Helper.FormatDateTime(para[1]), para[2], para[3], para[5]);
                    }
                    catch (Exception ex)
                    {
                        tbl = new DataTable();
                        Log.CreateLog("CI_Log", "Error page[frmLoadTerminatedReport.aspx.cs], detail:" + ex.Message);
                    }
                    if (tbl.Rows.Count > 0)
                    {

                        if (para[4] == "pdf")
                        {
                            #region //PDF
                            ReportViewer report = new ReportViewer();
                            ReportDataSource report_source;
                            report.Reset();
                            DataRow row = tbl.Rows[0];
                            report.LocalReport.ReportPath = Server.MapPath("terminated_policy_RR.rdlc");
                            report_source = new ReportDataSource("TERMINATED_POLICY", tbl);
                            report.LocalReport.Refresh();
                            report.LocalReport.DataSources.Clear();
                            ReportParameter[] paras = new ReportParameter[] { 
                        new ReportParameter("Title", "Terminated Policy Report"),
                        new ReportParameter("From_Date", Helper.FormatDateTime(para[0])+""),
                        new ReportParameter("To_Date", Helper.FormatDateTime( para[1])+""),
                        new ReportParameter("Printed_By",System.Web.Security.Membership.GetUser().UserName)
                        };

                            //Assign parameters to report
                            report.LocalReport.SetParameters(paras);
                            //add datasource in to report
                            report.LocalReport.DataSources.Add(report_source);
                            //export to pdf
                            Report_Generator.ExportToPDF(this.Context, report, "Terminated_Policy_Report" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
                            #endregion
                        }
                        else if (para[4] == "excel")
                        {
                            #region //EXCEL
                            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                            Response.Clear();
                            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                            Helper.excel.Sheet = sheet1;
                            //design row header
                            Helper.excel.HeaderText = new string[] { "No", "PolicyNo", "CustomerID", "FullName", "Gender", "BirthDate", "Age", 
                                                                    "Province", "EffeciveDate", "ExpiryDate","IssuedDate", "SA","Product","PaymentMode","Premium","PaymentBy","AgentCode", "TerminatedDate", "TerminatedBy", "TerminateRemarks"};
                            Helper.excel.generateHeader();
                            int row_no = 0;
                            foreach (DataRow row in tbl.Rows)
                            {

                                row_no += 1;
                                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                                //formate date
                                //HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                                //style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MMM/yyyy hh:mm:ss");
                                ////formate date
                                //HSSFCellStyle style_short = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                                //style_short.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd/MMM/yyyy");

                                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);//NO
                                Cell1.SetCellValue(row_no);

                                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);//PolicyNo
                                Cell2.SetCellValue(row["policy_number"].ToString());

                                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);//customerID
                                Cell3.SetCellValue(row["customer_id"].ToString());

                                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);//fullname
                                Cell4.SetCellValue(row["last_name"].ToString() + " " + row["first_name"].ToString());
                                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);//Gender
                                Cell5.SetCellValue(row["gender"].ToString() == "0" ? "F" : "M");
                                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);//BrithDate
                                Cell6.SetCellValue(Convert.ToDateTime(row["birth_date"].ToString()).ToString("dd/MM/yyyy"));
                                //Cell6.SetCellValue(row["birth_date"].ToString());
                                //Cell6.CellStyle = style;

                                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);//age
                                Cell7.SetCellValue(row["age"].ToString());
                                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);//province
                                Cell8.SetCellValue(row["province"].ToString());


                                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);//effectivedate
                                Cell9.SetCellValue(Convert.ToDateTime(row["effective_date"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"));
                                // Cell9.CellStyle = style;

                                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);//expirydate
                                Cell10.SetCellValue(Convert.ToDateTime(row["expiry_date"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"));
                                //Cell10.CellStyle = style;

                                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);//issueddate
                                Cell11.SetCellValue(Convert.ToDateTime(row["issued_date"].ToString()).ToString("dd/MM/yyyy"));
                                //Cell11.CellStyle = style_short;

                                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);//SA
                                Cell12.SetCellValue(row["sum_assured"].ToString());
                                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);//Product
                                Cell13.SetCellValue(row["Product_ID"].ToString());
                                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);//PaymentMode
                                Cell14.SetCellValue(row["mode"].ToString());
                                HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);//Premium
                                Cell15.SetCellValue(row["Premium"].ToString());
                                HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);//PaymentBy
                                Cell16.SetCellValue(row["payment_by"].ToString());
                                HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);//AgentCOde
                                Cell17.SetCellValue(row["agent_code"].ToString());

                                HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);//Terminated on
                                Cell18.SetCellValue(Convert.ToDateTime(row["terminated_on"].ToString()).ToString("dd/MM/yyyy"));
                                HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);//Terminated by
                                Cell19.SetCellValue(row["terminated_by"].ToString());
                                HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);//Terminated by
                                Cell20.SetCellValue(row["terminate_remarks"].ToString());
                            }
                            string filename = "terminated_policy_report" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                            Response.Clear();
                            Response.ContentType = "application/vnd.ms-excel";
                            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                            System.IO.MemoryStream file = new System.IO.MemoryStream();
                            hssfworkbook.Write(file);


                            Response.BinaryWrite(file.GetBuffer());

                            Response.End();
                            #endregion
                        }


                    }
                    else
                    {
                        div_message.InnerText = "No record found.";
                    }
                }
                else
                {
                    div_message.InnerText = "Ooop! something wrong with this page, please contact your system administrator.";
                    Log.CreateLog("CI_Log", "Session[PARAS] in page [frmLoadTerminatedReport.aspx.cs] is expired or empty.");//save CI log
                }
            }
            else
            {
                div_message.InnerText = "Ooop! something wrong with this page, please contact your system administrator.";
                Log.CreateLog("CI_Log", "Session[PARAS] in page [frmLoadTerminatedReport.aspx.cs] is expired or empty.");//save CI log
            }
        }
        catch (Exception ex)
        {
            div_message.InnerText = "Ooop! something wrong with this page, please contact your system administrator.";
            Log.AddExceptionToLog("Error page [frmLoadTerminatedReport.aspx.cs] while page load, detail:" + ex.Message);
        }
    }
}