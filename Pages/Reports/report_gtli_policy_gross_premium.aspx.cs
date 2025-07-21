using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Reports_report_gtli_policy_gross_premium : System.Web.UI.Page
{
    List<bl_gtli_policy_search> list_report_policy = new List<bl_gtli_policy_search>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime now = DateTime.Now;
            DateTime start_date = new DateTime(now.Year, now.Month, 1);
            DateTime end_date = start_date.AddMonths(1).AddDays(-1);

            //textbox start date and end date
            string start_day = "";
            string start_month = "";
            string end_day = "";
            string end_month = "";

            start_day = start_date.Day.ToString();
            start_month = start_date.Month.ToString();

            end_day = end_date.Day.ToString();
            end_month = end_date.Month.ToString();

            if (start_day.Length == 1)
            {
                start_day = "0" + start_day;
            }

            if (start_month.Length == 1)
            {
                start_month = "0" + start_month;
            }

            if (end_day.Length == 1)
            {
                end_day = "0" + end_day;
            }

            if (end_month.Length == 1)
            {
                end_month = "0" + end_month;
            }

            txtFromDate.Text = start_day + "/" + start_month + "/" + start_date.Year;
            txtToDate.Text = end_day + "/" + end_month + "/" + end_date.Year;
                        
            ReportPolicy();
        }
    }

    protected void ReportPolicy()
    {
        //Clear report div
        AppendDivPolicy.Controls.Clear();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {
            list_report_policy = da_gtli_policy_report.GetPolicyReportList(DateTime.Parse(txtFromDate.Text, dtfi), DateTime.Parse(txtToDate.Text, dtfi), 1);
        }

        if (list_report_policy.Count > 0)
        {
            tblTitle.Visible = true;

            
            if (txtFromDate.Text == "" || txtToDate.Text == "")
            {
                lblfrom.Text = "";
                lblfrom.Visible = false;
                lblto.Visible = false;
            }
            else
            {
                lblfrom.Text = "<input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + "  " + txtFromDate.Text + "<input type='Label' value=' - ' style=\"color:black; border:none; text-align:right; width:20px; font-size:12px;\"/><input type=\"Label\" value=\"To:  \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtToDate.Text;
                lblfrom.Visible = true;
                lblto.Visible = true;
            }


            StringBuilder strBuilder = new StringBuilder();

            string strTable = "<table class='gridtable' style='color: black' cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
            strTable += "<tr><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Policy no.</th><th style=\"text-align: center;\">Pay Date</th><th style=\"text-align: center;\">Return Date</th><th style=\"text-align: center;\">Total Premium</th></tr>";

            strBuilder.Append(strTable);

            strTable = "";
            double total_premium = 0;

            for (int i = 0; i < list_report_policy.Count; i++) // Loop through List with for
            {
                bl_gtli_policy_search policy_report = new bl_gtli_policy_search();


                policy_report.Policy_Number = list_report_policy[i].Policy_Number.ToString();
                
                policy_report.Effective_Date = list_report_policy[i].Effective_Date;               
             
                policy_report.TPD_Premium = list_report_policy[i].TPD_Premium;
                policy_report.Life_Premium = list_report_policy[i].Life_Premium;

                policy_report.DHC_Premium = list_report_policy[i].DHC_Premium;

                total_premium += (policy_report.Life_Premium + policy_report.TPD_Premium + policy_report.DHC_Premium);

                strTable += "<tr>";
                strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (i + 1) + "</td>";

                strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + "</td>";
                                                
               
                if (policy_report.Transaction_Type == 3)
                {
                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\"></td>";
                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";

                  
                    if ((policy_report.Life_Premium + policy_report.TPD_Premium + policy_report.DHC_Premium) > 0)
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "(" + (policy_report.Life_Premium + policy_report.TPD_Premium + policy_report.DHC_Premium).ToString("C2") + ")</td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";
                    }
                }
                else
                {
                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";
                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\"></td>";
                    

                    if ((policy_report.Life_Premium + policy_report.TPD_Premium + policy_report.DHC_Premium) > 0)
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + (policy_report.Life_Premium + policy_report.TPD_Premium + policy_report.DHC_Premium).ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";
                    }

                }

                strTable += "</tr>";


                strBuilder.Append(strTable);

                strTable = "";
            }

            //Total Row
            strTable += "<tr>";
            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\"></td>";
            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\"></td>";
            strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\"></td>";
            strTable += "<td style=\"text-align: center; font-weight: bold;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">Total</td>";

            if (total_premium > 0)
            {
                strTable += "<td style=\"text-align: right; padding-right:5px; font-weight: bold; text-decoration:underline; width=\"0px;\"  class=\"tb-schedule-td-right\">" + total_premium.ToString("C2") + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: right; padding-right:5px; font-weight: bold; text-decoration:underline; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "(" + total_premium.ToString("C2") + ")</td>";
            }

            strTable += "</table>";

            strBuilder.Append(strTable);

            AppendDivPolicy.InnerHtml = strBuilder.ToString();

         
        }
        else
        {
            tblTitle.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        ReportPolicy();

    }
}