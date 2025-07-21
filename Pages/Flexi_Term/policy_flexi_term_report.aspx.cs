using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Report_policy_flexi_term_report : System.Web.UI.Page
{
    List<bl_flexi_term_policy_report> list_report_policy = new List<bl_flexi_term_policy_report>();

    int order_by = 0;   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            //bind user name and user id to hiddenfield
            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

            if (txtFrom_date.Text == "" && txtTo_date.Text == "")
            {
                ReportPolicy(1);
            }
        }

    }

  
    void CheckOrderBy()
    {
        if (RdbOrderBy.SelectedValue != "")
        {
            if (int.Parse(RdbOrderBy.SelectedValue) == 1)
            {
                order_by = 1;

            }
            else if (int.Parse(RdbOrderBy.SelectedValue) == 2)
            {
                order_by = 2;
            }
            else if (int.Parse(RdbOrderBy.SelectedValue) == 3)
            {
                order_by = 3;
            }

        }
    }

    void ReportPolicy(int search_type)
    {
        //Clear report div
        AppendDivPolicy.Controls.Clear();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        string by_status_code = "";

      
        if (search_type == 1) /// Form Load
        {
            DateTime now = DateTime.Now;
            DateTime start_date = new DateTime(now.Year, now.Month, 1);
            DateTime end_date = start_date.AddMonths(1).AddDays(-1);

            string start_day = "";
            string start_month = "";
            string end_day = "";
            string end_month = "";

            start_day =  start_date.Day.ToString();
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

            txtFrom_date.Text = start_day + "/" + start_month + "/" + start_date.Year;
            txtTo_date.Text = end_day + "/" + end_month + "/" + end_date.Year;

            list_report_policy = da_flexi_term_policy.GetPolicyReportList(start_date, end_date, "", 1, "");
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

                        rbtnlAnswer.Checked = false;
                    }
                }
            }

            if (by_status_code.Length > 0)
            {
                by_status_code = by_status_code.Remove(by_status_code.Length - 1, 1);
            }

          
            if (txtFrom_date.Text == "" || txtTo_date.Text == "")
            {
                list_report_policy = da_flexi_term_policy.GetPolicyReportListByParams(DateTime.Parse("01/01/1900", dtfi), DateTime.Parse("01/01/1900", dtfi), by_status_code, order_by, int.Parse(ddlPaymentMod.SelectedValue), txtPolicyNum.Text, ddlProduct.SelectedValue, txtBarcode.Text, ddlBank.SelectedValue, 0);
            }
            else
            {
                list_report_policy = da_flexi_term_policy.GetPolicyReportListByParams(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), by_status_code, order_by, int.Parse(ddlPaymentMod.SelectedValue), txtPolicyNum.Text, ddlProduct.SelectedValue, txtBarcode.Text, ddlBank.SelectedValue, 1);
            }
                           
          
        }

        lblfrom1.Text = txtFrom_date.Text;
        lblto1.Text = txtTo_date.Text;

        if (txtFrom_date.Text == "" || txtTo_date.Text == "")
        {
            lblfrom.Text = "<div style='text-align:center;'><h1 style=\"text-align:center;\">Insurance Premium List</h1><h2 style=\"text-align:center;\">Flexi-Term Insurance</h2> </div>"; 
            lblfrom1.Visible = false;
            lblto1.Visible = false;
        }
        else
        {
            lblfrom.Text = "<div style='text-align:center;'><h1 style=\"text-align:center;\">Insurance Premium List</h1><h2 style=\"text-align:center;\">Flexi-Term Insurance</h2> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + "  " + txtFrom_date.Text + "<input type='Label' value=' - ' style=\"color:black; border:none; text-align:right; width:20px; font-size:12px;\"/><input type=\"Label\" value=\"To:  \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>"; 
            lblfrom1.Visible = true;
            lblto1.Visible = true;
        }

                     
        StringBuilder strBuilder = new StringBuilder();

        string strTable = "<table class='gridtable' style='color: black' cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
        strTable += "<tr><th styple=\"text-align: center; \">No.</th><th styple=\"text-align: center; \">Brank's Branch</th><th styple=\"text-align: center; \">Applicant's Account Number</th><th styple=\"text-align: center; \">Applicant's Surname</th><th styple=\"text-align: center; \">Applicant's First Name</th><th styple=\"text-align: center; \">Applicant's ID Number</th><th style=\"text-align: center;\">Applicant's ID Type</th><th style=\"text-align: center;\">Applicant's Date of Birth (dd/mm/yy)</th><th style=\"text-align: center;\">Applicant's Age</th><th styple=\"text-align: center; \">Applicant's Gender (Male/Female)</th><th style=\"text-align: center;\">Sum Insured</th><th style=\"text-align: center;\">Premium</th><th style=\"text-align: center;\">Effective Date</th><th styple=\"text-align: center; \">Expiry Date</th><th styple=\"text-align: center; \">Policy Number</th><th styple=\"text-align: center; \">Barcode</th><th styple=\"text-align: center; \">Card Number</th><th styple=\"text-align: center; \">Customer Number</th></tr>";

        strBuilder.Append(strTable);

        strTable = "";

        for (int i = 0; i < list_report_policy.Count; i++) // Loop through List with for
        {
            bl_flexi_term_policy_report policy_report = new bl_flexi_term_policy_report();

            policy_report.Card_ID = list_report_policy[i].Card_ID.ToString();
            policy_report.Card_Number = list_report_policy[i].Card_Number.ToString();
            policy_report.Policy_Number = list_report_policy[i].Policy_Number.ToString();
            policy_report.First_Name = list_report_policy[i].First_Name.ToString();
            policy_report.Last_Name = list_report_policy[i].Last_Name.ToString();
            policy_report.Customer_Flexi_Term_Number = list_report_policy[i].Customer_Flexi_Term_Number.ToString();
            policy_report.En_Abbr = list_report_policy[i].En_Abbr.ToString();
            policy_report.Policy_Status_Type_ID = list_report_policy[i].Policy_Status_Type_ID.ToString();           
            policy_report.Effective_Date = list_report_policy[i].Effective_Date;
            policy_report.Pay_Mode = list_report_policy[i].Pay_Mode.ToString();
            policy_report.Sum_Insure = list_report_policy[i].Sum_Insure;
            policy_report.Premium = list_report_policy[i].Premium;
            policy_report.ID_Card = list_report_policy[i].ID_Card.ToString();
            policy_report.Product_ID = list_report_policy[i].Product_ID.ToString();
            policy_report.Maturity_Date = list_report_policy[i].Maturity_Date;
            policy_report.Expiry_Date = list_report_policy[i].Expiry_Date;
            policy_report.Birth_Date = list_report_policy[i].Birth_Date;
            policy_report.Gender = list_report_policy[i].Gender;
            policy_report.Age_Insure = list_report_policy[i].Age_Insure;
            policy_report.Bank_Number = list_report_policy[i].Bank_Number;
            policy_report.Branch = list_report_policy[i].Branch;
            policy_report.Str_ID_Type = list_report_policy[i].Str_ID_Type;
            policy_report.Str_Gender = list_report_policy[i].Str_Gender;

            strTable += "<tr>";

            //No.
            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (i + 1) + "</td>";

            //Branch
            strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Branch + "</td>";

            //Bank Number
            strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Bank_Number + "</td>";

            //Surname
            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Last_Name + "</td>";

            //First Name
            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.First_Name + "</td>";

            //ID Number
            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.ID_Card + "</td>";

            //ID Type            
            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Str_ID_Type + "</td>";

            //Birth Date
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Birth_Date).ToString("dd-MM-yyyy") + "</td>";

            //Age            
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Age_Insure + "</td>";

            //Gender           
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Str_Gender + "</td>";

            //Sum Insured
            if (policy_report.Sum_Insure > 0)
            {
                strTable += "<td style=\"text-align: right; padding-right:5px; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Sum_Insure + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "-" + "</td>";
            }
            
            //Premium
            if (policy_report.Premium > 0)
            {
                strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Premium + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: right;  padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "-" + "</td>";
            }                      
       
            //Effective Date
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";

            //Expiry Date
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Expiry_Date).ToString("dd-MM-yyyy") + "</td>";

            //Policy Number
            strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + "</td>";
            
            //Barcode
            strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Card_ID + "</td>";

            //Card Number
            strTable += "<td style=\"text-align: center;  mso-number-format: 0000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Card_Number + "</td>";

            //Customer Number
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Customer_Flexi_Term_Number + "</td>";
           
            strTable += "</tr>";


            strBuilder.Append(strTable);

            strTable = "";
        }

        strTable += "</table>";

        strBuilder.Append(strTable);

        AppendDivPolicy.InnerHtml = strBuilder.ToString();

        if (search_type != 1)
        {
            hdfPaymentMode.Value = ddlPaymentMod.SelectedValue;
          
            hdfProduct.Value = ddlProduct.SelectedValue;
        }

        /// Clear value 
        RdbOrderBy.SelectedIndex = 0;
        ddlProduct.SelectedIndex = 0;
        ddlPaymentMod.SelectedIndex = 0;
        
        txtFrom_date.Text = "";
        txtTo_date.Text = "";

    }

   
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";


        if (txtFrom_date.Text == "" || txtTo_date.Text == "")
        {


            ReportPolicy(2);
            txtFrom_date.Text = "";
            txtTo_date.Text = "";

        }
        else
        {
            if (DateTime.Parse(txtFrom_date.Text, dtfi).Date > DateTime.Parse(txtTo_date.Text, dtfi).Date)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('From Date (" + txtFrom_date.Text + ") cannot be bigger than To Date (" + txtTo_date.Text + "), please check it again.')", true);
                txtFrom_date.Text = "";
                txtTo_date.Text = "";

            }
            else
            {

                ReportPolicy(2);
                txtFrom_date.Text = "";
                txtTo_date.Text = "";

            }
        }
    }
}