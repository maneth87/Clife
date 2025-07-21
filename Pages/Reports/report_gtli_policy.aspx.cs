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

public partial class Pages_Reports_report_gtli_policy : System.Web.UI.Page
{
    List<bl_gtli_policy_search> list_report_policy = new List<bl_gtli_policy_search>();

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

    //get autocomplete for company name
    [WebMethod()]
    public static string[] GetCompanyName()
    {
        ArrayList list_of_company = new ArrayList();
        list_of_company = da_gtli_company.GetListOfCompanyName();

        string[] companyArray = new string[list_of_company.Count];

        for (int i = 0; i <= list_of_company.Count - 1; i++)
        {
            companyArray[i] = list_of_company[i].ToString();
        }

        return companyArray;
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
           

        }
    }

    void ReportPolicy(int search_type)
    {
        //Clear report div
        AppendDivPolicy.Controls.Clear();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        

        if (search_type == 1) /// Form Load
        {
            DateTime now = DateTime.Now;
            DateTime start_date = new DateTime(now.Year, now.Month, 1);
            DateTime end_date = start_date.AddMonths(1).AddDays(-1);

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

            txtFrom_date.Text = start_day + "/" + start_month + "/" + start_date.Year;
            txtTo_date.Text = end_day + "/" + end_month + "/" + end_date.Year;

            list_report_policy = da_gtli_policy_report.GetPolicyReportList(start_date, end_date, 1);
        }
        else
        {
            CheckOrderBy(); 
         
            if (txtFrom_date.Text == "" || txtTo_date.Text == "")
            {
                list_report_policy = da_gtli_policy_report.Get_Policy_Report_By_Condition(DateTime.Parse("01/01/1900", dtfi), DateTime.Parse("01/01/1900", dtfi), order_by, txtPolicyNum.Text, txtCompany.Text.Trim(), txtAgentCode.Text.Trim(), 0);
            }
            else
            {
                list_report_policy = da_gtli_policy_report.Get_Policy_Report_By_Condition(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), order_by, txtPolicyNum.Text, txtCompany.Text.Trim(), txtAgentCode.Text.Trim(), 1);
            }

            

        }

        if (list_report_policy.Count > 0)
        {


            lblfrom1.Text = txtFrom_date.Text;
            lblto1.Text = txtTo_date.Text;

            if (txtFrom_date.Text == "" || txtTo_date.Text == "")
            {
                lblfrom.Text = "<div style='text-align:center;'><h1 style=\"color: blue; text-align:center;\">Policy Report</h1> </div>";
                lblfrom1.Visible = false;
                lblto1.Visible = false;
            }
            else
            {
                lblfrom.Text = "<div style='text-align:center;'><h1 style=\"color: blue; text-align:center;\">Policy Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + "  " + txtFrom_date.Text + "<input type='Label' value=' - ' style=\"color:black; border:none; text-align:right; width:20px; font-size:12px;\"/><input type=\"Label\" value=\"To:  \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";
                lblfrom1.Visible = true;
                lblto1.Visible = true;
            }


            StringBuilder strBuilder = new StringBuilder();

            string strTable = "<table class='gridtable' style='color: black' cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
            strTable += "<tr><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Policy no.</th><th styple=\"text-align: center; \">Company</th><th style=\"text-align: center;\">Sum Insured</th><th style=\"text-align: center; width: 50px\">Cover Period / (Days not cover)</th><th styple=\"text-align: center; \">Type</th><th style=\"text-align: center;\">Effective Date</th><th style=\"text-align: center;\">Maturity Date</th><th style=\"text-align: center;\">Life Premium</th><th style=\"text-align: center;\">TPD Premium</th><th style=\"text-align: center;\">DHC Premium</th><th style=\"text-align: center;\">Total Premium</th><th styple=\"text-align: center; \">Agent</th></tr>";

            strBuilder.Append(strTable);

            strTable = "";

            for (int i = 0; i < list_report_policy.Count; i++) // Loop through List with for
            {
                bl_gtli_policy_search policy_report = new bl_gtli_policy_search();


                policy_report.Policy_Number = list_report_policy[i].Policy_Number.ToString();
                policy_report.Company_Name = list_report_policy[i].Company_Name.ToString();
                policy_report.Effective_Date = list_report_policy[i].Effective_Date;
                policy_report.Expiry_Date = list_report_policy[i].Expiry_Date;
                policy_report.Sum_Insured = list_report_policy[i].Sum_Insured;
                policy_report.Sale_Agent_ID = list_report_policy[i].Sale_Agent_ID.ToString();
                policy_report.Transaction_Type = list_report_policy[i].Transaction_Type;
                policy_report.Transaction_Staff_Number = list_report_policy[i].Transaction_Staff_Number;
                policy_report.TPD_Premium = list_report_policy[i].TPD_Premium;
                policy_report.Life_Premium = list_report_policy[i].Life_Premium;

                policy_report.DHC_Premium = list_report_policy[i].DHC_Premium;
                policy_report.GTLI_Plan = list_report_policy[i].GTLI_Plan;
              
                DateTime maturity_date = list_report_policy[i].Expiry_Date.AddDays(1);
                string agent = da_sale_agent.GetSaleAgentNameByID(list_report_policy[i].Sale_Agent_ID);

                string type = "";
                int days;

                //Get policy row if this transaction
                bl_gtli_policy policy = new bl_gtli_policy();
                policy = da_gtli_policy.GetGTLIPolicyByID(list_report_policy[i].GTLI_Policy_ID);



                strTable += "<tr>";
                strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (i + 1) + "</td>";

                strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + "</td>";

                strTable += "<td style=\"text-align: left;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Company_Name + "</td>";

                if (policy_report.Transaction_Type == 3)
                {
                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\"> - </td>";

                }
                else
                {
                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Sum_Insured.ToString("C0") + "</td>";
                }

                if (policy_report.Transaction_Type == 1)
                {
                    type = "New Policy";
                    days = 356;
                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + days + "</td>";

                }
                else if (policy_report.Transaction_Type == 2)
                {
                    type = "Add Member";
                    //Get Number of Days Used
                    int used_days = Calculation.CalculateUsedDays(policy.Effective_Date, policy_report.Effective_Date);
                    int total_days = 365;

                    int remain_days = total_days - used_days;

                    days = remain_days;

                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + days + "</td>";
                }
                else if (policy_report.Transaction_Type == 3)
                {
                    type = "Resign Member";

                    TimeSpan mytimespan = policy.Expiry_Date.Subtract(policy_report.Effective_Date);
                    int days_not_cover = mytimespan.Days + 1;

                    days = days_not_cover;

                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">(" + days + ")</td>";
                }

                strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + type + "</td>";
                strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";
                strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (maturity_date).ToString("dd-MM-yyyy") + "</td>";

                if (policy_report.Transaction_Type == 3)
                {
                    if (policy_report.Life_Premium > 0)
                    {

                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "(" + policy_report.Life_Premium.ToString("C2") + ")</td>";

                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";

                    }

                    if (policy_report.TPD_Premium > 0)
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "(" + policy_report.TPD_Premium.ToString("C2") + ")</td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";
                    }

                    if (policy_report.DHC_Premium > 0)
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "(" + policy_report.DHC_Premium.ToString("C2") + ")</td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";
                    }


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
                    if (policy_report.Life_Premium > 0)
                    {

                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + policy_report.Life_Premium.ToString("C2") + "</td>";

                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";

                    }

                    if (policy_report.TPD_Premium > 0)
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + policy_report.TPD_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";
                    }

                    if (policy_report.DHC_Premium > 0)
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + policy_report.DHC_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";
                    }


                    if ((policy_report.Life_Premium + policy_report.TPD_Premium + policy_report.DHC_Premium) > 0)
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "" + (policy_report.Life_Premium + policy_report.TPD_Premium + policy_report.DHC_Premium).ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding-right:5px; width=\"0px;\"  class=\"tb-schedule-td-right\"> - </td>";
                    }

                }


                strTable += "<td style=\"text-align: left; padding-left: 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + agent + "</td>";

                strTable += "</tr>";


                strBuilder.Append(strTable);

                strTable = "";
            }

            strTable += "</table>";

            strBuilder.Append(strTable);

            AppendDivPolicy.InnerHtml = strBuilder.ToString();


            /// Clear value 
            RdbOrderBy.SelectedIndex = 0;

            txtFrom_date.Text = "";
            txtTo_date.Text = "";
        }
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