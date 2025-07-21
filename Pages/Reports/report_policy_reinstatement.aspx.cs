using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Reports_report_policy_reinstatement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           // Search();
        }
    }

    public string getWhileLoopData()
    {
        string htmlStr = hdnData.Value;

        return htmlStr;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    void Search()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DataTable dt = new DataTable();

        if (txtFrom_date.Text != "" && txtTo_date.Text != "")
        {
            dt = da_payment_receipt.Report_Policy_Reinstatement(txtPoliNumberSearch.Text, txtFirstName.Text, txtLastName.Text, DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi));
        }
        else
        {
            dt = da_payment_receipt.Report_Policy_Reinstatement(txtPoliNumberSearch.Text, txtFirstName.Text, txtLastName.Text, DateTime.Parse("01/01/1900"), DateTime.Parse("01/01/1900"));
        }

        DataTable uniqueCols = dt.DefaultView.ToTable(true, "Product_ID");

        lblfrom.Text = "<div style='text-align:center;' class='PrintPD'>Reinstatement Report </br> From: " + txtFrom_date.Text + "&nbsp; &nbsp; &nbsp;To:" + txtTo_date.Text + "</div>";

        StringBuilder strBuilder = new StringBuilder(); StringBuilder strBuilder_export = new StringBuilder();

        string strTable = "";

        int i = 0; decimal total_prem_lapsed = 0, total_prem=0;

        foreach (DataRow item in dt.Rows)
        {
            decimal amount = decimal.Parse(item["Prem_Interest"].ToString());

            total_prem += decimal.Parse(item["Amount"].ToString());

            amount = Math.Round(amount);

            i = i + 1; total_prem_lapsed += amount;

            strTable += "<tr>";
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + i + "</td>";

            strTable += "<td style='mso-number-format:00000000; text-align: center;'>" + item["Policy_Number"].ToString() + "</td>";

            strTable += "<td style='mso-number-format:00000000; text-align: center; padding:5px 5px 5px 5px; width=0px;  class=tb-schedule-td-right; '>" + item["Customer_ID"].ToString() + "</td>";

            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["insured_name"].ToString() + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Product_ID"].ToString() + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + item["Sum_Insure"].ToString() + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + item["Amount"].ToString() + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">"  + item["Mode"].ToString() + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + DateTime.Parse(item["Effective_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + DateTime.Parse(item["Due_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + DateTime.Parse(item["Pay_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["duration_day"].ToString() + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + amount + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"40px;\"   class=\"tb-schedule-td-right\">"  + item["Full_Name"].ToString() + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Sale_Agent_ID"].ToString() + "</td>";

            if (i == dt.Rows.Count)
            {
                strTable += "</tr>";

                strTable += "<tr>";
                strTable += "<td style=\"text-align: center; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "Total Policy: " + "</td>";
                strTable += "<td style=\"text-align: center;  font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + i + "</td>";
                strTable += "<td colspan=4 style=\"text-align: right; font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + "Total Premium: " + "</td>";
                strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + total_prem + "</td>";
                strTable += "<td colspan=5 style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "Total Reinstatement Premium: " + "</td>";
                strTable += "<td style=\"text-align: right;  font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + total_prem_lapsed + "</td>";
                strTable += "<td  colspan=2 style=\"text-align: right; font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + "</td>";

                strTable += "</tr>";

                strTable += "<tr>";
                strTable += "<td colspan=15 style=\"text-align: left;  font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + "Lapsed Policy is mostly on Product:" + "</td>";
                strTable += "</tr>";

                strBuilder.Append(strTable);

                foreach (DataRow dr in uniqueCols.Rows)
                {
                    strTable = "";

                    int numberOfRecords = 0;
                    DataRow[] rows;

                    rows = dt.Select("Product_ID ='" + dr["Product_ID"].ToString() + "'");
                    numberOfRecords = rows.Length;

                    strTable += "<tr>";
                    strTable += "<td style=\"text-align: center;  font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + dr["Product_ID"].ToString() + "</td>";
                    strTable += "<td style=\"text-align: center;  font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + numberOfRecords + "</td>";
                    strTable += "<td colspan=13 style=\"text-align: center;  font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + "</td>";
                    strTable += "</tr>";

                    strBuilder.Append(strTable);
                }
            }
            else
            {
                strTable += "</tr>";

                strBuilder.Append(strTable);
            }

            strTable = "";
        }


        if (dt.Rows.Count > 0)
        {
            hdnData.Value = strBuilder.ToString();

            strBuilder.Append(strTable);
        }
        else {
            hdnData.Value = "";

            strBuilder.Append(strTable);
        }
    }

}