using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class Pages_Reports_report_policy_payment : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
          //  Search();
        }
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
             dt = da_payment_receipt.Report_Payment(txtPoliNumberSearch.Text, txtFirstName.Text, txtLastName.Text, DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi));
        }
        else
        {
            dt = da_payment_receipt.Report_Payment(txtPoliNumberSearch.Text, txtFirstName.Text, txtLastName.Text, DateTime.Parse("01/01/1900"), DateTime.Parse("01/01/1900"));
        }

        lblfrom.Text = "<div style='text-align:center;' class='PrintPD'>Policy Payment </br> From: " + txtFrom_date.Text + "&nbsp; &nbsp; &nbsp;To:" + txtTo_date.Text + "</div>";

        //lblfrom1.Text = txtFrom_date.Text;
        //lblto1.Text = txtTo_date.Text;

        StringBuilder strBuilder = new StringBuilder(); StringBuilder strBuilder_export = new StringBuilder();

        string strTable = "", check_policy_id = "", check_status = ""; ;

        int i = 0, pay_mode_id; decimal total_prem_lapsed = 0, interest_lapsed = 0, total_interest = 0, total_prem = 0, normal_policy = 0, lapsed_policy = 0; 

        foreach (DataRow item in dt.Rows)
        {
            i = i + 1;

            interest_lapsed =(decimal.Parse(item["Rate_Lapsed"].ToString()));

            if (decimal.Parse(item["Rate_Lapsed"].ToString()) > 0)
            {
                //total_prem_lapsed += decimal.Parse(item["Amount"].ToString());

                if (check_policy_id != item["Policy_ID"].ToString())
                {
                    //if (txtFrom_date.Text != "" && txtTo_date.Text != "")
                    //{
                    //    total_interest += da_payment_receipt.Get_Interest_Lapsed(item["Policy_ID"].ToString(), DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi));
                    //}
                    //else
                    //{
                    //    total_interest += da_payment_receipt.Get_Interest_Lapsed(item["Policy_ID"].ToString(), DateTime.Parse("01/01/1900"), DateTime.Parse("01/01/1900"));
                    //}

                    check_policy_id = item["Policy_ID"].ToString();
                    lapsed_policy = lapsed_policy + 1;
                }

                check_status = "LAP";

            }
            else
            {
                if (check_policy_id != item["Policy_ID"].ToString())
                {
                    normal_policy = normal_policy + 1;
                    check_policy_id = item["Policy_ID"].ToString();
                }

                check_status = item["Policy_Status_Type_ID"].ToString();
            }

            total_interest += interest_lapsed;

            total_prem += decimal.Parse(item["Amount"].ToString());

            strTable += "<tr>";
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + i + "</td>";

            strTable += "<td style='mso-number-format:00000000; text-align: center;'>" + item["Policy_Number"].ToString() + "</td>";

            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Last_Name"].ToString() + " " + item["First_Name"].ToString() + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Gender"].ToString() + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Prem_Year"].ToString() + "/" + item["Prem_Lot"].ToString() + "</td>";

          
            if (item["Mode"].ToString() == "")
            {
                DataTable dt_mode = da_payment_receipt.Get_Pay_Mode(item["Policy_ID"].ToString());

                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + dt_mode.Rows[0][1].ToString() + "</td>";

                pay_mode_id = int.Parse(dt_mode.Rows[0][0].ToString());
            }
            else
            {
                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Mode"].ToString() + "</td>";

                pay_mode_id = int.Parse(item["Pay_Mode_ID"].ToString());
            }

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + item["Sum_Insure"].ToString() + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + item["Amount"].ToString() + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + interest_lapsed + "</td>";

            strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + "$" + (interest_lapsed + decimal.Parse(item["Amount"].ToString())) + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + DateTime.Parse(item["Due_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + DateTime.Parse(item["Pay_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + da_policy_prem_pay.GetNext_Due(pay_mode_id, DateTime.Parse(item["Due_Date"].ToString())).ToString("dd/MM/yyyy") + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["Receipt_Num"].ToString() + "</td>";
           
            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + item["En_Abbr"].ToString() + "</td>";

            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + check_status + "</td>";


            if (i == dt.Rows.Count)
            {
                total_interest = Math.Round(total_interest);

                strTable += "</tr>";

                strTable += "<tr>";

                strTable += "<td style=\"text-align: left; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "Normal/LAP: " + "</td>";
                strTable += "<td style=\"text-align: center; font-weight:bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + normal_policy + "/" + lapsed_policy + "</td>";

                strTable += "<td colspan=5 style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "Total: " + "</td>";
                strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + total_prem + "</td>";
                strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + total_interest + "</td>";

                strTable += "<td style=\"text-align: right; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + (total_prem + total_interest) + "</td>";

                strTable += "<td colspan=6 style=\"text-align: left; font-weight:bold;  padding:5px 5px 5px 5px; width=\"0px;\" >" + "</td>";

                strTable += "</tr>";

                strBuilder.Append(strTable);
            }
            else
            {
                strTable += "</tr>";

                strBuilder.Append(strTable);
            }

            strTable = "";
        }

        if (dt.Rows.Count >0)
        {
            strTable += "</table>";

            hdnData.Value = strBuilder.ToString();

            strBuilder.Append(strTable);
        }
    }

    
    public string getWhileLoopData()
    {
        string htmlStr = hdnData.Value;
        
        return htmlStr;
    }

   
   
}