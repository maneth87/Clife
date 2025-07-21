using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Data;
using System.Web.Security;



public partial class Pages_Reports_sale_app_details : System.Web.UI.Page
{
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ReportApplicationsByStatus();
        }
    }       

    protected void ReportApplicationsByStatus()
    {
        dvReportDetail.Style.Clear();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";

        string sale_agent_id = Request.QueryString[0];
        lblfrom1.Text = Request.QueryString[1].Replace('_', '/');
        lblto1.Text = Request.QueryString[2].Replace('_', '/');

        DateTime from_date = DateTime.Parse(lblfrom1.Text, dtfi);
        DateTime to_date = DateTime.Parse(lblto1.Text + " 23:59:59", dtfi3);
                
        List<bl_report_sale_app_by_status> report_sale_app_by_status_list = new List<bl_report_sale_app_by_status>();

        report_sale_app_by_status_list = da_report_sale.GetApplicationBySaleAgentIDAndDates(sale_agent_id, from_date, to_date);
     
        
        //Draw Header
        #region

        if (report_sale_app_by_status_list.Count > 0)
            {
                lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Sales Application</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + lblfrom1.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + lblto1.Text + "</div>";
                          
            }
            else
            {
                lblfrom.Text = "";
                lblfrom1.Text = "";
                lblto1.Text = "";
            }

            string strTable = "";

            //Draw Header
            if (report_sale_app_by_status_list.Count > 0)
            {

                strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
                strTable += "<tr border=\"1\"><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">App No</th><th styple=\"text-align: center; \">App Date</th><th styple=\"text-align: center; \">Premium</th><th style=\"text-align: center;\">Agent Name</th><th styple=\"text-align: center; \">Status</th><th styple=\"text-align: center; \">Policy No</th></tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";
            }
            else
            {
                strTable = "";
            }


        #endregion

        //Loop through sale app by status list
        #region
            for (int i = 0; i < report_sale_app_by_status_list.Count; i++)
            {
                //Get obj sale app by status of this index[i]
                bl_report_sale_app_by_status sale_app_by_status = new bl_report_sale_app_by_status();
                sale_app_by_status = report_sale_app_by_status_list[i];
                              
                //Draw Row
                #region

                    if (report_sale_app_by_status_list.Count > 0)
                    {
                        strTable += "<tr>";
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (i + 1) + "</td>";

                        if (sale_app_by_status.App_No == "")
                        {
                            strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                        }
                        else
                        {
                            strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.App_No + "</td>";
                        }
                                              
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.App_Date.ToString("d-MMM-yyyy") + "</td>";

                        strTable += "<td  style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" >$" + sale_app_by_status.Premium + "</td>";

                        strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.Agent_Name + "</td>";
                                             
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.Last_Status + "</td>";

                        if (sale_app_by_status.Policy_No == "")
                        {
                            strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                        }
                        else
                        {
                            strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.Policy_No + "</td>";
                        }               
                        
                        strTable += "</tr>";

                        dvReportDetail.Controls.Add(new LiteralControl(strTable));

                        strTable = "";
                    }

                #endregion
                
            }//End loop sale app by status list
        #endregion

      
        dvReportDetail.Style.Clear();

        if (report_sale_app_by_status_list.Count > 0)
        {
            strTable += "</table>";
            dvReportDetail.Controls.Add(new LiteralControl(strTable));
            strTable = "";
        }
        else
        {
            dvReportDetail.Controls.Add(new LiteralControl("No record found...."));

            strTable = "";

            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }

      
    }

     
    protected void btnOk_Click(object sender, EventArgs e)
    {
        ReportApplicationsByStatus();
    }
}