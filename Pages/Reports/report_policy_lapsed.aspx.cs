using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Reports_report_policy_lapsed : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtPremLapsed.Text == "") { txtPremLapsed.Text = "0"; }

        if (txtFrom_date.Text != "" || decimal.Parse(txtPremLapsed.Text) >0)
        {
            Search();
        }
    }

    void Search()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DataTable dt = new DataTable();


        if (txtFrom_date.Text != "")
        {
            dt = da_policy_prem_pay.GetPolicy_Lapsed(txtPoliNumberSearch.Text, txtLastNameSearch.Text, txtFirstNameSearch.Text, DateTime.Parse(txtFrom_date.Text, dtfi));
        }
        else
        {
            dt = da_policy_prem_pay.GetPolicy_Lapsed(txtPoliNumberSearch.Text, txtLastNameSearch.Text, txtFirstNameSearch.Text, DateTime.Parse("01/01/1900"));
        }



        StringBuilder strBuilder = new StringBuilder(); StringBuilder strBuilder_export = new StringBuilder();

        string strTable = "", check_policy_id = "";

        int i = 0; decimal total_prem_lapsed = 0, interest_lapsed = 0, total_interest = 0, total_prem = 0, normal_policy = 0, lapsed_policy = 0;

        txtDue_Date.Text = hdfDue_Date.Value;
        txtPoliNumber.Text = hdfPoli.Value;
        txtFirstName.Text = hdffirst.Value;
        txtLastName.Text = hdflast.Value;
        txtPay_Date.Text = hdfPay_Date.Value;
        txtCustomer_ID.Text = hdfCustomer_ID.Value;

        if (dt.Rows.Count > 0)
        {

            strTable = "<table id=\"tblExport\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
            strTable += "<tr><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Pol.no.</th><th styple=\"text-align: center; \">Customer</th><th style=\"text-align: center;\">Gender</th><th style=\"text-align: center;\">Years/Times</th><th style=\"text-align: center;\">Mode</th><th styple=\"text-align: center; \">Sum Insure</th><th styple=\"text-align: center; \">Amount</th><th style=\"text-align: center;\">Due Date</th><th style=\"text-align: center;\">Interest</th><th style=\"text-align: center;\">Prem & Interest</th><th style=\"text-align: center; width=\"0px;\">Dur.days</th><th style=\"text-align: center;\">Product</th></tr>";

            /// Check Year/Time
            int prem_year = 0, pay_lot = 0;

            if (dt.Rows.Count > 0)
            {
                prem_year = int.Parse(dt.Rows[0]["Year"].ToString()); pay_lot = pay_lot = int.Parse(dt.Rows[0]["Times"].ToString());
            }

            foreach (DataRow item in dt.Rows)
            {
                i = i + 1;

                if (pay_lot >= 12)
                {
                    pay_lot = 1;

                    prem_year = prem_year + 1;
                }
                else { pay_lot = pay_lot + 1; }

                lapsed_policy = lapsed_policy + 1;

                interest_lapsed = (decimal.Parse(item["Interest"].ToString()));

                if (decimal.Parse(item["Interest"].ToString()) > 0)
                {
                    ///Count LAP base on Policy Number
                    //if (check_policy_id != item["Policy_ID"].ToString())
                    //{
                    //    check_policy_id = item["Policy_ID"].ToString();

                    //    lapsed_policy = lapsed_policy + 1;
                    //}

                    total_interest += interest_lapsed;

                    lapsed_policy = lapsed_policy + 1;
                }
                else
                {
                    normal_policy = normal_policy + 1;
                }


                total_prem += decimal.Parse(item["Amount"].ToString());

                string format_text =@"mso-number-format:\@;";

                strTable += "<tr>";
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + i + "</td>";

                strTable += "<td style='mso-number-format:00000000; text-align: center;'>" + item["Polno"].ToString() + "</td>";

                strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Customer"].ToString() + "</td>";

                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Gender"].ToString() + "</td>";

                strTable += "<td style=" + format_text + " padding:5px 5px 5px 5px; width=0px;  class=tb-schedule-td-right>" + prem_year.ToString() + "/" + pay_lot.ToString() + "</td>";

                //strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + pay_lot + "</td>";

                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Mode"].ToString() + "</td>";

                strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + item["SumIn"].ToString() + "</td>";

                strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + item["Amount"].ToString() + "</td>";

                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + DateTime.Parse(item["Due_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

                //strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + DateTime.Parse(txtFrom_date.Text, dtfi).ToString("dd/MM/yyyy") + "</td>";

                strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + interest_lapsed + "</td>";

                strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + (interest_lapsed + decimal.Parse(item["Amount"].ToString())) + "</td>";

                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["duration_day"].ToString() + "</td>";

                // strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["month"].ToString() + "</td>";

                //strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + da_policy_prem_pay.GetNext_Due(int.Parse(item["Mode_ID"].ToString()), DateTime.Parse(item["Due_Date"].ToString())).ToString("dd/MM/yyyy") + "</td>";

                strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Product"].ToString() + "</td>";

                if (i == dt.Rows.Count)
                {
                    if (total_interest >= 1)
                    {
                        total_interest = Math.Round(total_interest, 0, MidpointRounding.AwayFromZero); 
                    }
                    else
                    {
                        total_interest = 0;
                    }

                    strTable += "</tr>";

                    strTable += "<tr>";

                    //strTable += "<td style=\"text-align: left; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "LAP: " + "</td>";
                    //strTable += "<td style=\"text-align: center; font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + lapsed_policy + "</td>";

                    strTable += "<td colspan=7 style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "Total: " + "</td>";
                    strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + total_prem + "</td>";
                    strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "</td>";
                    strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + total_interest + "</td>";
                    strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + (total_prem + total_interest).ToString() + "</td>";

                    strTable += "<td colspan= style=\"text-align: left; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "</td>";

                    strTable += "</tr>";

                    if (lapsed_policy > 0)
                    {
                        strBuilder.Append(strTable);
                        txtPremLapsed.Text = "$" + (total_prem + total_interest).ToString();
                    }
                    else
                    {
                        txtPremLapsed.Text = "$" + "0";
                    }
                }
                else
                {
                    strTable += "</tr>";

                    if (lapsed_policy > 0)
                    {
                        strBuilder.Append(strTable);
                    }
                }

                strTable = "";

            }

            if (dt.Rows.Count > 0)
            {
                if (lapsed_policy > 0)
                {
                    // hdnData.Value = strBuilder.ToString();
                    strTable += "</table>";

                    strBuilder.Append(strTable);

                    PrintPD.InnerHtml = strBuilder.ToString();
                }
            }
            else
            {
                txtPremLapsed.Text = "$" + total_prem_lapsed.ToString();
                PrintPD.InnerHtml = strBuilder.ToString();

            }
        }// Check dt
        else { 
            txtPremLapsed.Text = "$" + "0";

            //No data, set table to 0 row
            PrintPD.InnerHtml = "";
            
        
        }

    }

    protected void btnOriginalPrem_Click(object sender, EventArgs e)
    {
        da_calculate_original_prem.GetPolicy_Info();
    }
}