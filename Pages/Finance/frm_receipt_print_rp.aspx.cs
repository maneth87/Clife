using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;

public partial class Pages_Finance_rm_receipt_print_rp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Params.Count > 0)
            {
                string rec_id = Request.QueryString["rec_id"].ToString();
                ReportViewer myReport = new ReportViewer();
                ReportDataSource rdsDetail;

                DataTable tbl = new DataTable("dt_official_receipt");
                tbl = da_officail_receipt.GetGroupMicroOfficialReceipt(rec_id);

                myReport.LocalReport.ReportPath = Server.MapPath("rtp_official.rdlc");
                myReport.LocalReport.DataSources.Clear();

                rdsDetail = new ReportDataSource("ds_report", tbl);

                myReport.LocalReport.DataSources.Add(rdsDetail);
                myReport.LocalReport.Refresh();
                Report_Generator.ExportToPDF(this.Context, myReport, "official_receipt" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
            }
            else
            {
                Helper.Alert(true, "Bad link is not allow.", lblError);
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
}