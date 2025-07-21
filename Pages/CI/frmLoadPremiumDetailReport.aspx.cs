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
using System.Web.Security;
public partial class Pages_CI_frmLoadPremiumDetailReport : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] para = new string[] { };
        para = (string[])Session["PARAS"];
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
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
                //DataTable tbl = da_ci.GetPolicyPremiumDetailReport(Helper.FormatDateTime(para[0]), Helper.FormatDateTime(para[1]), para[2], para[3], para[5]);
                //Filter policy status = IF or Lap in datatable

                string logDesc = string.Concat("Issued Date from: ", para[0], " to:", para[1]);
                logDesc += para[2] == "" ? "" : string.Concat(", Customer Name:", para[2]);
                logDesc += para[3] == "" ? "" : string.Concat(", Pol No:", para[3]);
                DataTable tbl;
                try
                {
                    tbl = da_ci.GetPolicyPremiumDetailReport(Helper.FormatDateTime(para[0]), Helper.FormatDateTime(para[1]), para[2], para[3], para[5]).AsEnumerable().Where(r => r.Field<string>("policy_status") == "IF" || r.Field<string>("policy_status") == "LAP").CopyToDataTable();
                }
                catch (Exception ex)
                {
                    tbl = new DataTable();
                    Log.CreateLog("CI_Log", "Error function [da_ci.GetPolicyPremiumDetailReport], detail:" + ex.Message);
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
                        report.LocalReport.ReportPath = Server.MapPath("frmCIPremiumDetailReport_RR.rdlc");
                        report_source = new ReportDataSource("CI_POLICY_PREMIUM_DETAIL_REPORT", tbl);
                        report.LocalReport.Refresh();
                        report.LocalReport.DataSources.Clear();
                        ReportParameter[] paras = new ReportParameter[] { 
        
                    new ReportParameter("Title", "Policy Premium Detail Report"),
                    new ReportParameter("FromDate", Helper.FormatDateTime( para[0])+""),
                    new ReportParameter("ToDate", Helper.FormatDateTime(para[1])+""),
                    new ReportParameter("PrintedBy",System.Web.Security.Membership.GetUser().UserName)
                  
                };
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views premium detail report in PDF with criteria [", logDesc, "]."));

                        //Assign parameters to report
                        report.LocalReport.SetParameters(paras);
                        //add datasource in to report
                        report.LocalReport.DataSources.Add(report_source);
                        //export to pdf
                        Report_Generator.ExportToPDF(this.Context, report, "Policy_Premium_Detail_Report" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
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
                        Helper.excel.HeaderText = new string[] { "No", "PolicyNo", "CustomerID", "FullNameEN","FullNameKH", "Gender",  
                                                                    "SA","Product","Year", "Time","Premium","PaymentMode","PaymentCode","AgentCode"};
                        Helper.excel.generateHeader();
                        int row_no = 0;
                        foreach (DataRow row in tbl.Rows)
                        {
                            row_no += 1;
                            HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);
                                                      

                            HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);//NO
                            Cell1.SetCellValue(row_no);

                            HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);//PolicyNo
                            Cell2.SetCellValue(row["policy_number"].ToString());

                            HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);//customerID
                            Cell3.SetCellValue(row["customer_id"].ToString());

                            HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);//fullname
                            Cell4.SetCellValue(row["last_name"].ToString() + " " + row["first_name"].ToString());
                            HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);//fullnameKH
                            Cell5.SetCellValue(row["khmer_last_name"].ToString() + " " + row["khmer_first_name"].ToString());
                            HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);//Gender
                            Cell6.SetCellValue(row["gender"].ToString() == "0" ? "F" : "M");
                            
                            HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);//SA
                            Cell7.SetCellValue(row["sum_assured"].ToString());
                            HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);//Product
                            Cell8.SetCellValue(row["Product_ID"].ToString());
                            HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);//year
                            Cell9.SetCellValue(row["prem_year"].ToString());
                            HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);//time
                            Cell10.SetCellValue(row["prem_lot"].ToString());
                            HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);//premium
                            Cell11.SetCellValue(row["amount"].ToString());
                            HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);//PaymentMode
                            Cell12.SetCellValue(row["payment_mode"].ToString());
                            HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);//PaymentCode
                            Cell13.SetCellValue(row["payment_code"].ToString());
                            HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);//Agentcode
                            Cell14.SetCellValue(row["agent_code"].ToString());

                        }
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports premium detail report to excel with criteria [", logDesc, "]."));

                        string filename = "policy_premium_detail_report" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
                Log.CreateLog("CI_Log", "Session[PARAS] in page [frmLoadPremiumDetailReport.aspx.cs] is expired or empty.");//save CI log
            }
        }
        else
        {
            div_message.InnerText = "Ooop! something wrong with this page, please contact your system administrator.";
            Log.CreateLog("CI_Log", "Session[PARAS] in page [frmLoadPremiumDetailReport.aspx.cs] is expired or empty.");//save CI log
        }
    }
}