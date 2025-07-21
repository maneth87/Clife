using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Report_policy_micro_report : System.Web.UI.Page
{
    List<bl_policy_micro_report> list_report_policy = new List<bl_policy_micro_report>();

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

            list_report_policy = da_policy_micro_report.GetPolicyReportList(start_date, end_date, "", 1, "");
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

            if (ddlViewType.SelectedValue == "0")
            {
                if (txtFrom_date.Text == "" || txtTo_date.Text == "")
                {
                    list_report_policy = da_policy_micro_report.Get_Policy_Report_By_Condition(DateTime.Parse("01/01/1900", dtfi), DateTime.Parse("01/01/1900", dtfi), by_status_code, order_by, int.Parse(ddlPaymentMod.SelectedValue), txtPolicyNum.Text, ddlProduct.SelectedValue, txtBarcode.Text, txtAgentCode.Text, ddlViewType.SelectedValue, 0);
                }
                else
                {
                    list_report_policy = da_policy_micro_report.Get_Policy_Report_By_Condition(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), by_status_code, order_by, int.Parse(ddlPaymentMod.SelectedValue), txtPolicyNum.Text, ddlProduct.SelectedValue, txtBarcode.Text, txtAgentCode.Text, ddlViewType.SelectedValue, 1);
                }
                
            }
            else
            {
                string channel_location_id = da_channel.GetChannelLocationIDBySaleAgentID(txtAgentCode.Text);

                if (txtFrom_date.Text == "" || txtTo_date.Text == "")
                {
                    list_report_policy = da_policy_micro_report.Get_Policy_Report_By_Condition(DateTime.Parse("01/01/1900", dtfi), DateTime.Parse("01/01/1900", dtfi), by_status_code, order_by, int.Parse(ddlPaymentMod.SelectedValue), txtPolicyNum.Text, ddlProduct.SelectedValue, txtBarcode.Text, channel_location_id, ddlViewType.SelectedValue, 0);
                }
                else
                {
                    list_report_policy = da_policy_micro_report.Get_Policy_Report_By_Condition(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), by_status_code, order_by, int.Parse(ddlPaymentMod.SelectedValue), txtPolicyNum.Text, ddlProduct.SelectedValue, txtBarcode.Text, channel_location_id, ddlViewType.SelectedValue, 1);
                }
                
            }            
          
        }

        lblfrom1.Text = txtFrom_date.Text;
        lblto1.Text = txtTo_date.Text;

        if (txtFrom_date.Text == "" || txtTo_date.Text == "")
        {
            lblfrom.Text = ""; 
            lblfrom1.Visible = false;
            lblto1.Visible = false;
        }
        else
        {
            lblfrom.Text = "<div style='text-align:center;'><input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + "  " + txtFrom_date.Text + "<input type='Label' value=' - ' style=\"color:black; border:none; text-align:right; width:20px; font-size:12px;\"/><input type=\"Label\" value=\"To:  \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>"; 
            lblfrom1.Visible = true;
            lblto1.Visible = true;
        }

                     
        StringBuilder strBuilder = new StringBuilder();

        string strTable = "<table class='grid-layout' style='color: black; border: 1pt thin #9f9f9f;' cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
        strTable += "<tr><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Barcode</th><th styple=\"text-align: center; \">Card no.</th><th styple=\"text-align: center; \">Policy no.</th><th styple=\"text-align: center; \">Payment Code</th><th styple=\"text-align: center; \">Customer ID</th><th styple=\"text-align: center; \">Insured's name</th><th style=\"text-align: center;\">Product</th><th style=\"text-align: center;\">Sum Insured</th><th style=\"text-align: center;\">Premium</th><th styple=\"text-align: center; \">Status</th><th style=\"text-align: center;\">Effective Date</th><th style=\"text-align: center;\">Expiry Date</th><th style=\"text-align: center;\">Pay Mode</th><th styple=\"text-align: center; \">Agent</th></tr>";

        strBuilder.Append(strTable);

        strTable = "";

        for (int i = 0; i < list_report_policy.Count; i++) // Loop through List with for
        {
            bl_policy_micro_report policy_report = new bl_policy_micro_report();

            policy_report.Barcode = list_report_policy[i].Barcode.ToString();
            policy_report.Card_Number = list_report_policy[i].Card_Number.ToString();
            policy_report.Policy_Number = list_report_policy[i].Policy_Number.ToString();
            policy_report.Customer_Number = list_report_policy[i].Customer_Number.ToString();
            policy_report.Customer_Name = list_report_policy[i].Customer_Name.ToString();
            policy_report.Customer_Name_Khmer = list_report_policy[i].Customer_Name_Khmer.ToString();
            policy_report.En_Abbr = list_report_policy[i].En_Abbr.ToString();
            policy_report.Policy_Status_Type_ID = list_report_policy[i].Policy_Status_Type_ID.ToString();           
            policy_report.Effective_Date = list_report_policy[i].Effective_Date;
            policy_report.Mode = list_report_policy[i].Mode.ToString();
            policy_report.User_Sum_Insure = list_report_policy[i].User_Sum_Insure;
            policy_report.User_Premium = list_report_policy[i].User_Premium;          
            policy_report.Agent = list_report_policy[i].Agent.ToString();
            policy_report.Product_ID = list_report_policy[i].Product_ID.ToString();
            policy_report.Maturity_Date = list_report_policy[i].Maturity_Date;
            policy_report.Payment_Code = list_report_policy[i].Payment_Code;
          
            strTable += "<tr>";
            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (i + 1) + "</td>";

            strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Barcode + "</td>";

            strTable += "<td style=\"text-align: center;  mso-number-format: 0000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Card_Number + "</td>";

            strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + "</td>";

            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Payment_Code + "</td>";

            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Customer_Number + "</td>";
            
            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Customer_Name + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.En_Abbr + "</td>";

            if (policy_report.User_Sum_Insure > 0)
            {
                strTable += "<td style=\"text-align: right;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.User_Sum_Insure + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " " + "</td>";
            }

            if (policy_report.User_Premium > 0)
            {
                strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.User_Premium + "</td>";
            }
            else
            {
                strTable += "<td style=\"text-align: right;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + "</td>";
            }

            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Policy_Status_Type_ID + "</td>";
                      
           
            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";

            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Maturity_Date.AddDays(-1)).ToString("dd-MM-yyyy") + "</td>";

            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Mode + "</td>";


            strTable += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\" width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Agent + "</td>";
           
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