using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_add_member_form : System.Web.UI.Page
{
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

            DataTable agent_list = da_sale_agent.GetSaleAgentList();

            ddlSaleAgent.DataSource = agent_list;
            ddlSaleAgent.DataTextField = "Full_Name";
            ddlSaleAgent.DataValueField = "Sale_Agent_ID";
            ddlSaleAgent.DataBind();


            string this_policy_id = Request.Params["pid"];

            bl_gtli_policy this_policy = da_gtli_policy.GetGTLIPolicyByID(this_policy_id);

            string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(this_policy.GTLI_Policy_ID);

          

            if (policy_status != "IF")
            {
                //disable resign and add new button
                btnAddEmployee.Enabled = false;

            }
            else
            {
                //Enable resign and add new button

                btnAddEmployee.Enabled = true;
            }

            //disable button add new member and resign member if this policy is already expire
            int policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(this_policy_id);

            //get last policy year by policy number
            int last_policy_year = da_gtli_premium.GetGTLILastPolicyYearByPolicyNumber(this_policy.Policy_Number);

            if (last_policy_year > policy_year)
            {

                btnAddEmployee.Enabled = false;
            }
            else
            {

                btnAddEmployee.Enabled = true;
            }

            double life_premium = 0;
            double tpd_premium = 0;
            double dhc_premium = 0;
            double accidental_100plus_premium = 0;

            //get list of active employee for this sale transaction
            ArrayList list_of_active_employee = new ArrayList();
            list_of_active_employee = da_gtli_employee.GetListOfActiveEmployee(this_policy_id);

            ArrayList list_of_employee = new ArrayList();


            int number = 1;
            //int rows = 0;

            double total_sum_insured = 0;
            double total_life_premium = 0;
            double total_tpd_premium = 0;
            double total_dhc_premium = 0;
            double total_accidental_100plus_premium = 0;

            //loop through active employee list
            for (int k = 0; k <= list_of_active_employee.Count - 1; k++)
            {
                bl_gtli_employee myemployee = (bl_gtli_employee)list_of_active_employee[k];
                life_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "Death");
                tpd_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "TPD");
                dhc_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "DHC");
                accidental_100plus_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "Accidental100Plus");

                //set values
                string str_certificate_no = myemployee.Certificate_Number.ToString();

                while (str_certificate_no.Length < 6)
                {
                    str_certificate_no = "0" + str_certificate_no;
                }

                bl_gtli_premium premium = new bl_gtli_premium();
                premium = da_gtli_premium.GetGTLIPremiumByID(myemployee.GTLI_Premium_ID);

                TimeSpan mytimespan = premium.Expiry_Date.Subtract(premium.Effective_Date);
                int coverage_period = mytimespan.Days + 1;

                bl_gtli_plan my_plan = da_gtli_plan.GetPlan(premium.GTLI_Plan_ID);

                bl_gtli_member_list active_employee = new bl_gtli_member_list();
                active_employee.Certificate_Number = str_certificate_no;
                active_employee.Days = coverage_period;
                active_employee.DHC_Premium = dhc_premium;
                active_employee.Effective_Date = premium.Effective_Date;
                active_employee.Employee_Name = myemployee.Employee_Name;
                active_employee.Expiry_Date = premium.Expiry_Date;
                active_employee.GTLI_Plan = my_plan.GTLI_Plan;
                active_employee.Life_Premium = life_premium;
                active_employee.Total_Premium = (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium);
                active_employee.TPD_Premium = tpd_premium;
                active_employee.Sum_Insured = Convert.ToDouble(myemployee.Sum_Insured);
                active_employee.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                active_employee.GTLI_Premium_ID = myemployee.GTLI_Premium_ID;
                active_employee.Accidental_100Plus_Premium = accidental_100plus_premium;

                list_of_employee.Add(active_employee);


                number += 1;

                //total
                total_sum_insured += active_employee.Sum_Insured;
                total_dhc_premium += active_employee.DHC_Premium;
                total_life_premium += active_employee.Life_Premium;
                total_tpd_premium += active_employee.TPD_Premium;
                total_accidental_100plus_premium += accidental_100plus_premium;
            }


            //get list of plan
            string company_name = null;

            company_name = da_gtli_company.GetCompanyNameByID(this_policy.GTLI_Company_ID);

            List<bl_gtli_plan> plan_list = new List<bl_gtli_plan>();
            plan_list = da_gtli_plan.GetPlanListByCompanyName(company_name);

            if (plan_list.Count > 0)
            {
                ddlPlan.Items.Clear();
                ListItem item = new ListItem();
                item.Text = ".";
                item.Value = "0";

                ddlPlan.Items.Add(item);
                ddlPlan.AppendDataBoundItems = true;
                ddlPlan.DataSource = plan_list;
                ddlPlan.DataTextField = "GTLI_Plan";
                ddlPlan.DataValueField = "GTLI_Plan_ID";
                ddlPlan.DataBind();


            }
            else
            {
                ddlPlan.Items.Clear();

            }


            //Get Premium Summary and all Detail
            PopulatePremium();
        }
    }

    //View Premium Info
    private void PopulatePremium()
    {
        //get policy
        string policy_id = Request.Params["pid"];
        bl_gtli_policy policy = da_gtli_policy.GetGTLIPolicyByID(policy_id);

        //printing date
        lblDate.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
        lblDate2.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");

        lblTitle.Text = "Master List";
        lblTitle2.Text = "Master List";

        //Get customer info
        bl_gtli_company company = da_gtli_company.GetObjCompanyByID(policy.GTLI_Company_ID);

        bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(company.GTLI_Company_ID);

        //Get list of premium by policy id
        List<bl_gtli_premium> premium_list = new List<bl_gtli_premium>();
        premium_list = da_gtli_premium.GetGTLPremiumListByPolicyID(policy_id);

        double life_premium = 0;
        double tpd_premium = 0;
        double dhc_premium = 0;
        double accidental_100plus_premium = 0;

        DateTime expiry_date, effective_date;

        expiry_date = policy.Expiry_Date;
        effective_date = policy.Effective_Date;

        //get list of plans
        ArrayList list_of_plan = da_gtli_policy.GetListOfPlanByPolicyID(policy_id);

        string myplan = "";

        for (int i = 0; i <= list_of_plan.Count - 1; i++)
        {
            myplan += list_of_plan[i] + "<br />";
        }

        double total_sum_insured = 0;
        double total_life_premium = 0;
        double total_dhc_premium = 0;
        double total_tpd_premium = 0;
        double total_accidental_100plus_premium = 0;


        string strTableDetailPrint = "<table class='gridtable' width='98%'>";
        strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

        strTableDetailPrint = "";
        int number = 0;
        int rows = 0;
        string agentname = "";
        //Loop through premium list
        for (int i = 0; i <= premium_list.Count - 1; i++)
        {
            bl_gtli_premium premium = premium_list[i];
            string agent_name = da_sale_agent.GetSaleAgentNameByID(premium.Sale_Agent_ID);
            agentname = da_sale_agent.GetSaleAgentNameByID(premium.Sale_Agent_ID);

            TimeSpan mytimespan = premium.Expiry_Date.Subtract(premium.Effective_Date);
            int days = mytimespan.Days + 1;

            ArrayList list_employee = new ArrayList();
            list_employee = da_gtli_employee.GetListOfEmployeeByGTLIPremiumID(premium.GTLI_Premium_ID);


            //loop through employee list
            for (int k = 0; k <= list_employee.Count - 1; k++)
            {
                bl_gtli_employee employee = new bl_gtli_employee();
                employee = (bl_gtli_employee)list_employee[k];

                life_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "Death");
                tpd_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "TPD");
                dhc_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "DHC");
                accidental_100plus_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "Accidental100Plus");

                double employee_sum_insure = da_gtli_employee_premium.GetSumInsuredByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "Death");

                number += 1;
                rows += 1;

                string str_certificate_no = employee.Certificate_Number.ToString();

                while (str_certificate_no.Length < 6)
                {
                    str_certificate_no = "0" + str_certificate_no;
                }

                //view content according to transaction type (1 = original, 2 = new plan, 3 = resign member)
                switch (premium.Transaction_Type)
                {
                    case 1:
                    case 2:
                        //case original and add 
                        total_dhc_premium += dhc_premium;
                        total_life_premium += life_premium;
                        total_tpd_premium += tpd_premium;
                        total_accidental_100plus_premium += accidental_100plus_premium;

                        string plan_name = da_gtli_plan.GetPlan(premium.GTLI_Plan_ID).GTLI_Plan;

                        if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                        {
                            total_sum_insured += 0;

                        }
                        else
                        {
                            total_sum_insured += employee_sum_insure;

                        }

                        if (premium.Transaction_Type == 2)
                        {
                            //Color green
                            //Row
                            HtmlTableRow tr_black = new HtmlTableRow();
                            tr_black.Style.Add("color", "Green");
                            tr_black.Style.Add("font-size", "8pt");

                            //Number
                            HtmlTableCell tc_number = new HtmlTableCell();
                            tc_number.Style.Add("text-align", "center");

                            tc_number.InnerText = (number).ToString();

                            //Certificate Number
                            HtmlTableCell tc_certificate_number = new HtmlTableCell();

                            tc_certificate_number.Style.Add("padding-left", "5px");

                            //Employee Name
                            HtmlTableCell tc_employee_name = new HtmlTableCell();
                            tc_employee_name.Style.Add("text-align", "left");
                            tc_employee_name.Style.Add("padding-left", "5px");

                            tc_employee_name.InnerText = employee.Employee_Name;

                            //Plan Name
                            HtmlTableCell tc_plan_name = new HtmlTableCell();
                            tc_plan_name.Style.Add("text-align", "center");


                            //Effective Date
                            HtmlTableCell tc_effective_date = new HtmlTableCell();
                            tc_effective_date.Style.Add("text-align", "center");


                            //Expiry Date
                            HtmlTableCell tc_expiry_date = new HtmlTableCell();
                            tc_expiry_date.Style.Add("text-align", "center");


                            //Cover Period
                            HtmlTableCell tc_covery_period = new HtmlTableCell();
                            tc_covery_period.Style.Add("text-align", "center");

                            //Sum Insured
                            HtmlTableCell tc_sum_insured = new HtmlTableCell();
                            tc_sum_insured.Style.Add("text-align", "right");

                            tc_sum_insured.Style.Add("padding-right", "6px");

                            if (employee_sum_insure != 0)
                            {
                                tc_sum_insured.InnerText = employee_sum_insure.ToString("C0");

                            }
                            else
                            {
                                tc_sum_insured.InnerText = " - ";

                            }

                            //Life Premium
                            HtmlTableCell tc_life_premium = new HtmlTableCell();
                            tc_life_premium.Style.Add("text-align", "right");

                            tc_life_premium.Style.Add("padding-right", "6px");
                            if (life_premium != 0)
                            {
                                tc_life_premium.InnerText = life_premium.ToString("C2");
                            }
                            else
                            {
                                tc_life_premium.InnerText = " - ";
                            }

                            //Accidental 100Plus Premium
                            HtmlTableCell tc_accidental_100plus_premium = new HtmlTableCell();
                            tc_accidental_100plus_premium.Style.Add("text-align", "right");

                            tc_accidental_100plus_premium.Style.Add("padding-right", "6px");
                            if (accidental_100plus_premium != 0)
                            {
                                tc_accidental_100plus_premium.InnerText = accidental_100plus_premium.ToString("C2");
                            }
                            else
                            {
                                tc_accidental_100plus_premium.InnerText = " - ";
                            }

                            //TPD Premium
                            HtmlTableCell tc_tpd_premium = new HtmlTableCell();
                            tc_tpd_premium.Style.Add("text-align", "right");

                            tc_tpd_premium.Style.Add("padding-right", "6px");
                            if (tpd_premium != 0)
                            {
                                tc_tpd_premium.InnerText = tpd_premium.ToString("C2");
                            }
                            else
                            {
                                tc_tpd_premium.InnerText = " - ";
                            }

                            //DHC Premium
                            HtmlTableCell tc_dhc_premium = new HtmlTableCell();
                            tc_dhc_premium.Style.Add("text-align", "right");

                            tc_dhc_premium.Style.Add("padding-right", "6px");
                            if (dhc_premium != 0)
                            {
                                tc_dhc_premium.InnerText = dhc_premium.ToString("C2");
                            }
                            else
                            {
                                tc_dhc_premium.InnerText = " - ";
                            }

                            //Sub Total Premium
                            HtmlTableCell tc_sub_total_premium = new HtmlTableCell();
                            tc_sub_total_premium.Style.Add("text-align", "right");

                            tc_sub_total_premium.Style.Add("padding-right", "6px");
                            if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                            {
                                tc_sub_total_premium.InnerText = (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2");
                            }
                            else
                            {
                                tc_sub_total_premium.InnerText = " - ";
                            }

                            if (employee.Certificate_Number != 0)
                            {
                                tc_certificate_number.InnerText = str_certificate_no;
                                tc_plan_name.InnerText = plan_name;
                                tc_effective_date.InnerText = premium.Effective_Date.ToString("d-MMM-yyyy");
                                tc_expiry_date.InnerText = premium.Expiry_Date.ToString("d-MMM-yyyy");
                                tc_covery_period.InnerText = days.ToString();

                            }
                            else
                            {
                                tc_certificate_number.InnerText = " - ";
                                tc_plan_name.InnerText = " - ";
                                tc_effective_date.InnerText = " - ";
                                tc_expiry_date.InnerText = " - ";
                                tc_covery_period.InnerText = " - ";

                            }

                            tr_black.Controls.Add(tc_number);
                            tr_black.Controls.Add(tc_certificate_number);
                            tr_black.Controls.Add(tc_employee_name);
                            tr_black.Controls.Add(tc_plan_name);
                            tr_black.Controls.Add(tc_effective_date);
                            tr_black.Controls.Add(tc_expiry_date);
                            tr_black.Controls.Add(tc_covery_period);
                            tr_black.Controls.Add(tc_sum_insured);
                            tr_black.Controls.Add(tc_life_premium);
                            tr_black.Controls.Add(tc_accidental_100plus_premium);
                            tr_black.Controls.Add(tc_tpd_premium);
                            tr_black.Controls.Add(tc_dhc_premium);
                            tr_black.Controls.Add(tc_sub_total_premium);
                            tblPremiumDetail.Controls.Add(tr_black);

                            rows += 1;

                            if (rows == 49)
                            {
                                strTableDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                                rows = 0;

                                //New table
                                strTableDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                                strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                            }

                            strTableDetailPrint += "<tr>";
                            strTableDetailPrint += "<td style='text-align: center; color: green' width='30px'>" + (number).ToString() + "</td>";
                            strTableDetailPrint += "<td style='padding-left: 5px; color: green'>" + str_certificate_no + "</td>";
                            strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: green'>" + employee.Employee_Name + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + plan_name + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + premium.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + days.ToString() + "</td>";
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + employee_sum_insure.ToString("C0") + "</td>";

                            if (life_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + life_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }

                            if (accidental_100plus_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + accidental_100plus_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }

                            if (tpd_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + tpd_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }


                            if (dhc_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + dhc_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }


                            if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + "</td>";

                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }

                            strTableDetailPrint += "</tr>";

                            dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                            strTableDetailPrint = "";
                        }
                        else
                        {
                            //Color black
                            //Row
                            HtmlTableRow tr_black = new HtmlTableRow();
                            tr_black.Style.Add("color", "Black");
                            tr_black.Style.Add("font-size", "8pt");

                            //Number
                            HtmlTableCell tc_number = new HtmlTableCell();
                            tc_number.Style.Add("text-align", "center");
                            tc_number.Style.Add("color", "black");
                            tc_number.InnerText = (number).ToString();

                            //Certificate Number
                            HtmlTableCell tc_certificate_number = new HtmlTableCell();
                            tc_certificate_number.Style.Add("color", "black");
                            tc_certificate_number.Style.Add("padding-left", "5px");


                            //Employee Name
                            HtmlTableCell tc_employee_name = new HtmlTableCell();
                            tc_employee_name.Style.Add("text-align", "left");
                            tc_employee_name.Style.Add("padding-left", "5px");
                            tc_employee_name.Style.Add("color", "black");
                            tc_employee_name.InnerText = employee.Employee_Name;

                            //Plan Name
                            HtmlTableCell tc_plan_name = new HtmlTableCell();
                            tc_plan_name.Style.Add("text-align", "center");
                            tc_plan_name.Style.Add("color", "black");


                            //Effective Date
                            HtmlTableCell tc_effective_date = new HtmlTableCell();
                            tc_effective_date.Style.Add("text-align", "center");
                            tc_effective_date.Style.Add("color", "black");


                            //Expiry Date
                            HtmlTableCell tc_expiry_date = new HtmlTableCell();
                            tc_expiry_date.Style.Add("text-align", "center");
                            tc_expiry_date.Style.Add("color", "black");


                            //Cover Period
                            HtmlTableCell tc_covery_period = new HtmlTableCell();
                            tc_covery_period.Style.Add("text-align", "center");
                            tc_covery_period.Style.Add("color", "black");


                            //Sum Insured
                            HtmlTableCell tc_sum_insured = new HtmlTableCell();
                            tc_sum_insured.Style.Add("text-align", "right");
                            tc_sum_insured.Style.Add("color", "black");
                            tc_sum_insured.Style.Add("padding-right", "6px");

                            if (employee_sum_insure != 0)
                            {
                                tc_sum_insured.InnerText = employee_sum_insure.ToString("C0");
                            }
                            else
                            {
                                tc_sum_insured.InnerText = " - ";
                            }

                            //Life Premium
                            HtmlTableCell tc_life_premium = new HtmlTableCell();
                            tc_life_premium.Style.Add("text-align", "right");
                            tc_life_premium.Style.Add("color", "black");
                            tc_life_premium.Style.Add("padding-right", "6px");
                            if (life_premium != 0)
                            {
                                tc_life_premium.InnerText = life_premium.ToString("C2");
                            }
                            else
                            {
                                tc_life_premium.InnerText = " - ";
                            }

                            //Accidental 100Plus Premium
                            HtmlTableCell tc_accidental_100plus_premium = new HtmlTableCell();
                            tc_accidental_100plus_premium.Style.Add("text-align", "right");
                            tc_accidental_100plus_premium.Style.Add("color", "black");
                            tc_accidental_100plus_premium.Style.Add("padding-right", "6px");

                            if (accidental_100plus_premium != 0)
                            {
                                tc_accidental_100plus_premium.InnerText = accidental_100plus_premium.ToString("C2");
                            }
                            else
                            {
                                tc_accidental_100plus_premium.InnerText = " - ";
                            }

                            //TPD Premium
                            HtmlTableCell tc_tpd_premium = new HtmlTableCell();
                            tc_tpd_premium.Style.Add("text-align", "right");
                            tc_tpd_premium.Style.Add("color", "black");
                            tc_tpd_premium.Style.Add("padding-right", "6px");
                            if (tpd_premium != 0)
                            {
                                tc_tpd_premium.InnerText = tpd_premium.ToString("C2");
                            }
                            else
                            {
                                tc_tpd_premium.InnerText = " - ";
                            }

                            //DHC Premium
                            HtmlTableCell tc_dhc_premium = new HtmlTableCell();
                            tc_dhc_premium.Style.Add("text-align", "right");
                            tc_dhc_premium.Style.Add("color", "black");
                            tc_dhc_premium.Style.Add("padding-right", "6px");
                            if (dhc_premium != 0)
                            {
                                tc_dhc_premium.InnerText = dhc_premium.ToString("C2");
                            }
                            else
                            {
                                tc_dhc_premium.InnerText = " - ";
                            }


                            //Sub Total Premium
                            HtmlTableCell tc_sub_total_premium = new HtmlTableCell();
                            tc_sub_total_premium.Style.Add("text-align", "right");
                            tc_sub_total_premium.Style.Add("color", "black");
                            tc_sub_total_premium.Style.Add("padding-right", "6px");
                            if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                            {
                                tc_sub_total_premium.InnerText = (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2");
                            }
                            else
                            {
                                tc_sub_total_premium.InnerText = " - ";
                            }

                            if (employee.Certificate_Number != 0)
                            {
                                tc_certificate_number.InnerText = str_certificate_no;
                                tc_plan_name.InnerText = plan_name;
                                tc_effective_date.InnerText = premium.Effective_Date.ToString("d-MMM-yyyy");
                                tc_expiry_date.InnerText = premium.Expiry_Date.ToString("d-MMM-yyyy");
                                tc_covery_period.InnerText = days.ToString();

                            }
                            else
                            {
                                tc_certificate_number.InnerText = " - ";
                                tc_plan_name.InnerText = " - ";
                                tc_effective_date.InnerText = " - ";
                                tc_expiry_date.InnerText = " - ";
                                tc_covery_period.InnerText = " - ";
                            }

                            tr_black.Controls.Add(tc_number);
                            tr_black.Controls.Add(tc_certificate_number);
                            tr_black.Controls.Add(tc_employee_name);
                            tr_black.Controls.Add(tc_plan_name);
                            tr_black.Controls.Add(tc_effective_date);
                            tr_black.Controls.Add(tc_expiry_date);
                            tr_black.Controls.Add(tc_covery_period);
                            tr_black.Controls.Add(tc_sum_insured);
                            tr_black.Controls.Add(tc_life_premium);
                            tr_black.Controls.Add(tc_accidental_100plus_premium);
                            tr_black.Controls.Add(tc_tpd_premium);
                            tr_black.Controls.Add(tc_dhc_premium);
                            tr_black.Controls.Add(tc_sub_total_premium);
                            tblPremiumDetail.Controls.Add(tr_black);

                            rows += 1;

                            if (rows == 49)
                            {
                                strTableDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                                rows = 0;

                                //New table
                                strTableDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                                strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                            }

                            strTableDetailPrint += "<tr>";

                            if (employee.Certificate_Number != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: center; color: black' width='30px'>" + (number).ToString() + "</td>";
                                strTableDetailPrint += "<td style='padding-left: 5px; color: black'>" + str_certificate_no + "</td>";
                                strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: black'>" + employee.Employee_Name + "</td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'>" + plan_name + "</td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'>" + premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'>" + premium.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'>" + days.ToString() + "</td>";
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + employee_sum_insure.ToString("C0") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: center; color: black' width='30px'>" + (number).ToString() + "</td>";
                                strTableDetailPrint += "<td style='padding-left: 5px; color: black'> - </td>";
                                strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: black'>" + employee.Employee_Name + "</td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'> - </td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'> - </td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'> - </td>";
                                strTableDetailPrint += "<td style='text-align: center; color: black'> - </td>";
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }


                            if (life_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + life_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            if (accidental_100plus_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + accidental_100plus_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            if (tpd_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + tpd_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }


                            if (dhc_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + dhc_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }


                            if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + "</td>";

                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            strTableDetailPrint += "</tr>";

                            dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                            strTableDetailPrint = "";

                        }

                        break;

                    case 3:
                        //case resign
                        total_dhc_premium -= dhc_premium;
                        total_life_premium -= life_premium;
                        total_tpd_premium -= tpd_premium;
                        total_accidental_100plus_premium -= accidental_100plus_premium;

                        bl_gtli_plan plan_resign = da_gtli_policy.GetPlanByPolicyIDAndCertificateID(policy_id, employee.GTLI_Certificate_ID);

                        if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                        {
                            total_sum_insured -= 0;

                        }
                        else
                        {
                            total_sum_insured -= employee_sum_insure;

                        }

                        //Color red
                        //Row
                        HtmlTableRow tr_red = new HtmlTableRow();
                        tr_red.Style.Add("color", "red");
                        tr_red.Style.Add("font-size", "8pt");

                        //Number
                        HtmlTableCell tc_number_resign = new HtmlTableCell();
                        tc_number_resign.Style.Add("text-align", "center");

                        tc_number_resign.InnerText = (number).ToString();

                        //Certificate Number
                        HtmlTableCell tc_certificate_number_resign = new HtmlTableCell();

                        tc_certificate_number_resign.Style.Add("padding-left", "5px");


                        //Employee Name
                        HtmlTableCell tc_employee_name_resign = new HtmlTableCell();
                        tc_employee_name_resign.Style.Add("text-align", "left");
                        tc_employee_name_resign.Style.Add("padding-left", "5px");

                        tc_employee_name_resign.InnerText = employee.Employee_Name;

                        //Plan Name
                        HtmlTableCell tc_plan_name_resign = new HtmlTableCell();
                        tc_plan_name_resign.Style.Add("text-align", "center");


                        //Effective Date
                        HtmlTableCell tc_effective_date_resign = new HtmlTableCell();
                        tc_effective_date_resign.Style.Add("text-align", "center");



                        //Expiry Date
                        HtmlTableCell tc_expiry_date_resign = new HtmlTableCell();
                        tc_expiry_date_resign.Style.Add("text-align", "center");



                        //Cover Period
                        HtmlTableCell tc_covery_period_resign = new HtmlTableCell();
                        tc_covery_period_resign.Style.Add("text-align", "center");


                        //Sum Insured
                        HtmlTableCell tc_sum_insured_resign = new HtmlTableCell();
                        tc_sum_insured_resign.Style.Add("text-align", "right");

                        tc_sum_insured_resign.Style.Add("padding-right", "6px");

                        if (employee_sum_insure != 0)
                        {
                            tc_sum_insured_resign.InnerText = employee_sum_insure.ToString("C0");
                        }
                        else
                        {
                            tc_sum_insured_resign.InnerText = " - ";
                        }

                        //Life Premium
                        HtmlTableCell tc_life_premium_resign = new HtmlTableCell();
                        tc_life_premium_resign.Style.Add("text-align", "right");

                        tc_life_premium_resign.Style.Add("padding-right", "6px");

                        if (life_premium != 0)
                        {

                            tc_life_premium_resign.InnerText = life_premium.ToString("C2");

                        }
                        else
                        {
                            tc_life_premium_resign.InnerText = " - ";
                        }

                        //Accidental 100Plus Premium
                        HtmlTableCell tc_accidental_100plus_premium_resign = new HtmlTableCell();

                        tc_accidental_100plus_premium_resign.Style.Add("text-align", "right");

                        tc_accidental_100plus_premium_resign.Style.Add("padding-right", "6px");

                        if (accidental_100plus_premium != 0)
                        {
                            tc_accidental_100plus_premium_resign.InnerText = accidental_100plus_premium.ToString("C2");
                        }
                        else
                        {
                            tc_accidental_100plus_premium_resign.InnerText = " - ";
                        }

                        //TPD Premium
                        HtmlTableCell tc_tpd_premium_resign = new HtmlTableCell();
                        tc_tpd_premium_resign.Style.Add("text-align", "right");

                        tc_tpd_premium_resign.Style.Add("padding-right", "6px");

                        if (tpd_premium != 0)
                        {
                            tc_tpd_premium_resign.InnerText = tpd_premium.ToString("C2");
                        }
                        else
                        {
                            tc_tpd_premium_resign.InnerText = " - ";
                        }

                        //DHC Premium
                        HtmlTableCell tc_dhc_premium_resign = new HtmlTableCell();

                        tc_dhc_premium_resign.Style.Add("text-align", "right");

                        tc_dhc_premium_resign.Style.Add("padding-right", "6px");

                        if (dhc_premium != 0)
                        {
                            tc_dhc_premium_resign.InnerText = dhc_premium.ToString("C2");
                        }
                        else
                        {
                            tc_dhc_premium_resign.InnerText = " - ";
                        }


                        //Sub Total Premium
                        HtmlTableCell tc_sub_total_premium_resign = new HtmlTableCell();

                        tc_sub_total_premium_resign.Style.Add("text-align", "right");

                        tc_sub_total_premium_resign.Style.Add("padding-right", "6px");

                        if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                        {
                            tc_sub_total_premium_resign.InnerText = (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2");
                        }
                        else
                        {
                            tc_sub_total_premium_resign.InnerText = " - ";
                        }

                        if (employee.Certificate_Number != 0)
                        {
                            tc_certificate_number_resign.InnerText = str_certificate_no;
                            tc_plan_name_resign.InnerText = plan_resign.GTLI_Plan;
                            tc_effective_date_resign.InnerText = premium.Effective_Date.ToString("d-MMM-yyyy");
                            tc_expiry_date_resign.InnerText = premium.Expiry_Date.ToString("d-MMM-yyyy");
                            tc_covery_period_resign.InnerText = days.ToString();

                        }
                        else
                        {
                            tc_certificate_number_resign.InnerText = " - ";
                            tc_plan_name_resign.InnerText = " - ";
                            tc_effective_date_resign.InnerText = " - ";
                            tc_expiry_date_resign.InnerText = " - ";
                            tc_covery_period_resign.InnerText = " - ";

                        }

                        tr_red.Controls.Add(tc_number_resign);
                        tr_red.Controls.Add(tc_certificate_number_resign);
                        tr_red.Controls.Add(tc_employee_name_resign);
                        tr_red.Controls.Add(tc_plan_name_resign);
                        tr_red.Controls.Add(tc_effective_date_resign);
                        tr_red.Controls.Add(tc_expiry_date_resign);
                        tr_red.Controls.Add(tc_covery_period_resign);
                        tr_red.Controls.Add(tc_sum_insured_resign);
                        tr_red.Controls.Add(tc_life_premium_resign);
                        tr_red.Controls.Add(tc_accidental_100plus_premium_resign);
                        tr_red.Controls.Add(tc_tpd_premium_resign);
                        tr_red.Controls.Add(tc_dhc_premium_resign);
                        tr_red.Controls.Add(tc_sub_total_premium_resign);
                        tblPremiumDetail.Controls.Add(tr_red);

                        rows += 1;

                        if (rows == 49)
                        {
                            strTableDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                            dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                            rows = 0;

                            //New table
                            strTableDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                            strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                        }

                        strTableDetailPrint += "<tr>";

                        if (employee.Certificate_Number != 0)
                        {
                            strTableDetailPrint += "<td style='text-align: center; color: red' width='30px'>" + (number).ToString() + "</td>";
                            strTableDetailPrint += "<td style='padding-left: 5px; color: red'>" + str_certificate_no + "</td>";
                            strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: red'>" + employee.Employee_Name + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + plan_resign.GTLI_Plan + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + premium.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + days.ToString() + "</td>";
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + employee_sum_insure.ToString("C0") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint += "<td style='text-align: center; color: red' width='30px'>" + (number).ToString() + "</td>";
                            strTableDetailPrint += "<td style='padding-left: 5px; color: red'> - </td>";
                            strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: red'>" + employee.Employee_Name + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                        }


                        if (life_premium != 0)
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + life_premium.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                        }

                        if (accidental_100plus_premium != 0)
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + accidental_100plus_premium.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                        }

                        if (tpd_premium != 0)
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + tpd_premium.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                        }


                        if (dhc_premium != 0)
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + dhc_premium.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                        }


                        if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + "</td>";

                        }
                        else
                        {
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                        }

                        strTableDetailPrint += "</tr>";

                        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                        strTableDetailPrint = "";

                        break;



                } //end switch premium type        

            } //end loop employee list



        } //end loop premium list

        lblPolicyNumber.Text = policy.Policy_Number;
        lblCreatedDate.Text = policy.Created_On.ToString("d-MMM-yyyy");
        lblAgentName.Text = agentname;
        

        lblCompanyName.Text = company.Company_Name;
        lblTypeOfBusiness.Text = company.Type_Of_Business;
        lblContactName.Text = contact.Contact_Name;
        lblPhone.Text = contact.Contact_Phone;
        lblEmail.Text = contact.Contact_Email;
        lblAddress.Text = company.Company_Address;
        lblSumInsured.Text = total_sum_insured.ToString("C0");

        lblPremiumPayment.Text = (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2");
        lblEffectiveDate.Text = policy.Effective_Date.ToString("d-MMM-yyyy");
        lblExpiryDate.Text = policy.Expiry_Date.ToString("d-MMM-yyyy");

        //Div plan innerhtml
        dvPlan.InnerHtml = myplan;

        //Summary Print
        lblPolicyNumber2.Text = policy.Policy_Number;
        lblCreatedDate2.Text = policy.Created_On.ToString("d-MMM-yyyy");
        lblAgentName2.Text = agentname;

        lblCompanyName2.Text = company.Company_Name;
        lblTypeOfBusiness2.Text = company.Type_Of_Business;
        lblContactName2.Text = contact.Contact_Name;
        lblPhone2.Text = contact.Contact_Phone;
        lblEmail2.Text = contact.Contact_Email;
        lblAddress2.Text = company.Company_Address;
        lblSumInsured2.Text = total_sum_insured.ToString("C0");

        lblPremiumPayment2.Text = (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2");
        lblEffectiveDate2.Text = policy.Effective_Date.ToString("d-MMM-yyyy");
        lblExpiryDate2.Text = policy.Expiry_Date.ToString("d-MMM-yyyy");

        //Div plan innerhtml
        dvPlan2.InnerHtml = myplan;

        //Total Row               
        HtmlTableRow tr_total = new HtmlTableRow();
        tr_total.Style.Add("color", "Black");
        tr_total.Style.Add("font-size", "8pt");

        //Total Number
        HtmlTableCell tc_number_total = new HtmlTableCell();
        tc_number_total.Style.Add("text-align", "center");
        tc_number_total.Style.Add("color", "black");
        tc_number_total.InnerText = "";

        //Total Certificate Number
        HtmlTableCell tc_certificate_number_total = new HtmlTableCell();
        tc_certificate_number_total.Style.Add("color", "black");
        tc_certificate_number_total.InnerText = "";

        //Total Employee Name 
        HtmlTableCell tc_employee_name_total = new HtmlTableCell();
        tc_employee_name_total.Style.Add("text-align", "left");
        tc_employee_name_total.Style.Add("padding-left", "5px");
        tc_employee_name_total.Style.Add("color", "black");
        tc_employee_name_total.InnerText = "";

        //Total Plan Name
        HtmlTableCell tc_plan_name_total = new HtmlTableCell();
        tc_plan_name_total.Style.Add("text-align", "center");
        tc_plan_name_total.Style.Add("color", "black");
        tc_plan_name_total.InnerText = "";

        //Total Effective Date
        HtmlTableCell tc_effective_date_total = new HtmlTableCell();
        tc_effective_date_total.Style.Add("text-align", "center");
        tc_effective_date_total.Style.Add("color", "black");
        tc_effective_date_total.InnerText = "";

        //Total Expiry Date
        HtmlTableCell tc_expiry_date_total = new HtmlTableCell();
        tc_expiry_date_total.Style.Add("text-align", "center");
        tc_expiry_date_total.Style.Add("color", "black");
        tc_expiry_date_total.InnerText = "";

        //Total Cover Period
        HtmlTableCell tc_covery_period_total = new HtmlTableCell();
        tc_covery_period_total.Style.Add("text-align", "center");
        tc_covery_period_total.Style.Add("color", "black");
        tc_covery_period_total.Style.Add("font-weight", "bold");
        tc_covery_period_total.InnerText = "Total:";

        //Total Sum Insured
        HtmlTableCell tc_sum_insured_total = new HtmlTableCell();
        tc_sum_insured_total.Style.Add("text-align", "right");
        tc_sum_insured_total.Style.Add("color", "black");
        tc_sum_insured_total.Style.Add("padding-right", "6px");
        tc_sum_insured_total.Style.Add("font-weight", "bold");
        tc_sum_insured_total.Style.Add("text-decoration", "underline");
        tc_sum_insured_total.InnerText = total_sum_insured.ToString("C0");

        //Total Life Premium
        HtmlTableCell tc_life_premium_total = new HtmlTableCell();
        tc_life_premium_total.Style.Add("text-align", "right");
        tc_life_premium_total.Style.Add("color", "black");
        tc_life_premium_total.Style.Add("padding-right", "6px");
        tc_life_premium_total.Style.Add("font-weight", "bold");

        if (total_life_premium > 0)
        {
            tc_life_premium_total.Style.Add("text-decoration", "underline");
            tc_life_premium_total.InnerText = total_life_premium.ToString("C2");
        }
        else
        {
            tc_life_premium_total.InnerText = " - ";
        }

        //Total Accidental_100plus Premium
        HtmlTableCell tc_accidental_100plus_premium_total = new HtmlTableCell();
        tc_accidental_100plus_premium_total.Style.Add("text-align", "right");
        tc_accidental_100plus_premium_total.Style.Add("color", "black");
        tc_accidental_100plus_premium_total.Style.Add("padding-right", "6px");
        tc_accidental_100plus_premium_total.Style.Add("font-weight", "bold");

        if (total_accidental_100plus_premium > 0)
        {
            tc_accidental_100plus_premium_total.Style.Add("text-decoration", "underline");
            tc_accidental_100plus_premium_total.InnerText = total_accidental_100plus_premium.ToString("C2");
        }
        else
        {
            tc_accidental_100plus_premium_total.InnerText = " - ";
        }

        //Total TPD Premium
        HtmlTableCell tc_tpd_premium_total = new HtmlTableCell();
        tc_tpd_premium_total.Style.Add("text-align", "right");
        tc_tpd_premium_total.Style.Add("color", "black");
        tc_tpd_premium_total.Style.Add("padding-right", "6px");
        tc_tpd_premium_total.Style.Add("font-weight", "bold");

        if (total_tpd_premium > 0)
        {
            tc_tpd_premium_total.Style.Add("text-decoration", "underline");
            tc_tpd_premium_total.InnerText = total_tpd_premium.ToString("C2");
        }
        else
        {
            tc_tpd_premium_total.InnerText = " - ";
        }

        //Total DHC Premium
        HtmlTableCell tc_dhc_premium_total = new HtmlTableCell();
        tc_dhc_premium_total.Style.Add("text-align", "right");
        tc_dhc_premium_total.Style.Add("color", "black");
        tc_dhc_premium_total.Style.Add("padding-right", "6px");
        tc_dhc_premium_total.Style.Add("font-weight", "bold");

        if (total_dhc_premium > 0)
        {
            tc_dhc_premium_total.Style.Add("text-decoration", "underline");
            tc_dhc_premium_total.InnerText = total_dhc_premium.ToString("C2");
        }
        else
        {
            tc_dhc_premium_total.InnerText = " - ";
        }


        //Total Premium
        HtmlTableCell tc_total_premium = new HtmlTableCell();
        tc_total_premium.Style.Add("text-align", "right");
        tc_total_premium.Style.Add("color", "black");
        tc_total_premium.Style.Add("padding-right", "6px");
        tc_total_premium.Style.Add("font-weight", "bold");

        if (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium > 0)
        {
            tc_total_premium.Style.Add("text-decoration", "underline");
            tc_total_premium.InnerText = (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2");
        }
        else
        {
            tc_total_premium.InnerText = " - ";
        }

        tr_total.Controls.Add(tc_number_total);
        tr_total.Controls.Add(tc_certificate_number_total);
        tr_total.Controls.Add(tc_employee_name_total);
        tr_total.Controls.Add(tc_plan_name_total);
        tr_total.Controls.Add(tc_effective_date_total);
        tr_total.Controls.Add(tc_expiry_date_total);
        tr_total.Controls.Add(tc_covery_period_total);
        tr_total.Controls.Add(tc_sum_insured_total);
        tr_total.Controls.Add(tc_life_premium_total);
        tr_total.Controls.Add(tc_accidental_100plus_premium_total);
        tr_total.Controls.Add(tc_tpd_premium_total);
        tr_total.Controls.Add(tc_dhc_premium_total);
        tr_total.Controls.Add(tc_total_premium);
        tblPremiumDetail.Controls.Add(tr_total);

        //Total Print Row       
        strTableDetailPrint = "<tr>";
        strTableDetailPrint += "<td></td>";
        strTableDetailPrint += "<td></td>";
        strTableDetailPrint += "<td></td>";
        strTableDetailPrint += "<td></td>";
        strTableDetailPrint += "<td></td>";
        strTableDetailPrint += "<td></td>";
        strTableDetailPrint += "<td style='text-align: center; font-weight: bold;'>Total:</td>";
        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'>" + total_sum_insured.ToString("C0") + "</td>";

        if (total_life_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_life_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }


        if (total_accidental_100plus_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_accidental_100plus_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }

        if (total_tpd_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_tpd_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }


        if (total_dhc_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_dhc_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }

        if (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2") + "</td>";

        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }

        strTableDetailPrint += "</table>";

        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

    }

    //Add new member button click
    protected void btnAddEmployee_Click(object sender, EventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
        dtfi2.ShortDatePattern = "MM/dd/yyyy";
        dtfi2.DateSeparator = "/";

        //check date of add format        
        if (!Helper.CheckDateFormat(txtEffectiveDate.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Effective date invalid.')", true);
            return;
        }

        string channel_location_id = "0D696111-2590-4FA2-BCE6-C8B2D46648C9"; //Camlife HQ
        string channel_channel_item_id = "016AE1FC-CF77-461A-92F8-6C7605D0A648";

        //Effective date is add_date in db
        DateTime add_effective_date = Convert.ToDateTime(txtEffectiveDate.Text, dtfi);

        string policy_id = Request.QueryString.Get(0);

        bl_gtli_policy policy = da_gtli_policy.GetGTLIPolicyByID(policy_id);


        //get plan by plan_id
        bl_gtli_plan my_plan = da_gtli_plan.GetPlan(Convert.ToString(ddlPlan.SelectedValue));

        //Get last certificate number by this company and policy number
        int Certificate_Number = da_gtli_employee.GetLastCertificateNoByPolicyID(policy.GTLI_Policy_ID);

        int policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(policy_id);

        double discount = 0;

        double total_sum_insure = 0;

        if (txtDiscountAmount.Text.Trim() != "")
        {
            discount = Convert.ToDouble(txtDiscountAmount.Text.Trim());
        }

        try
        {
            //check if file upload contain any file
            if ((uploadedDoc.PostedFile != null) & !string.IsNullOrEmpty(uploadedDoc.PostedFile.FileName))
            {
                string savePath = "~/Upload/GTLI/CustomerAddData/";
                dynamic postedFile = uploadedDoc.PostedFile;
                string filename = Path.GetFileName(postedFile.FileName);
                string contentType = postedFile.ContentType;
                int contentLength = postedFile.ContentLength;
                string filepath = null;

                postedFile.SaveAs(Server.MapPath(savePath + filename));

                string version = Path.GetExtension(filename);
                filepath = Server.MapPath(savePath + filename).ToString();

                //verify if the file has been save
                #region 
                
                if (version == ".xls")
                {
                    bool invalid_input = false;

                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + filepath + "';Extended Properties=Excel 8.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [GClientData$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[GClientData$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    //Validate correct Data from file

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessageAdd.Text = "Please check your input of employee name field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessageAdd.Text = "Please check your input for gender field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                        {
                            lblMessageAdd.Text = "Please check your input for date field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (my_plan.Sum_Insured == 0)
                        {
                            if (!Helper.IsNumeric(dt.Rows[i][5].ToString()))
                            {
                                lblMessageAdd.Text = "Please check your input for sum insure field then try again. Row number: " + (i + 2) + "";
                                return;
                            }
                        }

                    }

                    //if all valid input then continue update for (upload input option)
                    if (invalid_input == false)
                    {
                        //Start adding new employees, recalculating total premium

                        //check add date with expiry date
                        if (policy.Expiry_Date.CompareTo(add_effective_date) != 1) //before effective date
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Effective date must not be later than or equal to expiry date. Please check again.')", true);
                            return;
                        }

                        //GTLI Premium
                        bl_gtli_premium gtli_premium = new bl_gtli_premium();
                        gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                        gtli_premium.Channel_Location_ID = channel_location_id;
                        gtli_premium.Created_By = hdfusername.Value;
                        gtli_premium.Created_On = DateTime.Now;
                        gtli_premium.DHC_Option_Value = Convert.ToInt32(my_plan.DHC_Option_Value);
                        gtli_premium.DHC_Premium = 0;
                        gtli_premium.Effective_Date = add_effective_date;
                        gtli_premium.Expiry_Date = policy.Expiry_Date;
                        gtli_premium.GTLI_Plan_ID = my_plan.GTLI_Plan_ID;
                        gtli_premium.GTLI_Policy_ID = policy.GTLI_Policy_ID;
                        gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                        gtli_premium.Life_Premium = 0;
                        gtli_premium.Pay_Mode_ID = 1;
                        gtli_premium.Policy_Year = policy_year;
                        gtli_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                        gtli_premium.Sum_Insured = my_plan.Sum_Insured;
                        gtli_premium.TPD_Premium = 0;
                        gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                        gtli_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);
                        gtli_premium.Accidental_100Plus_Premium = 0;
                        gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                        gtli_premium.DHC_Premium_Discount = 0;
                        gtli_premium.Life_Premium_Discount = 0;
                        gtli_premium.TPD_Premium_Discount = 0;
                        gtli_premium.Discount = Convert.ToDouble(discount);
                        gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_premium.Life_Premium_Tax_Amount = 0;
                        gtli_premium.TPD_Premium_Tax_Amount = 0;
                        gtli_premium.DHC_Premium_Tax_Amount = 0;
                        gtli_premium.Original_Accidental_100Plus_Premium = 0;
                        gtli_premium.Original_DHC_Premium = 0;
                        gtli_premium.Original_Life_Premium = 0;
                        gtli_premium.Original_TPD_Premium = 0;
                        gtli_premium.Transaction_Type = 2;

                        if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                            return;

                        }

                        //Get Number of Days Used
                        int used_days = CalculateUsedDays(policy.Effective_Date, add_effective_date);
                        int total_days = 365;

                        int remain_days = total_days - used_days;

                        decimal total_new_original_life_premium = 0;
                        decimal total_new_original_tpd_premium = 0;
                        decimal total_new_original_dhc_premium = 0;
                        decimal total_new_original_accidental_100plus_premium = 0;

                        decimal total_new_life_premium = 0;
                        decimal total_new_tpd_premium = 0;
                        decimal total_new_dhc_premium = 0;
                        decimal total_new_accidental_100plus_premium = 0;

                        decimal total_new_life_premium_discount = 0;
                        decimal total_new_tpd_premium_discount = 0;
                        decimal total_new_dhc_premium_discount = 0;
                        decimal total_new_accidental_100plus_premium_discount = 0;

                        decimal total_new_life_premium_tax_amount;
                        decimal total_new_tpd_premium_tax_amount;
                        decimal total_new_dhc_premium_tax_amount;
                        decimal total_new_accidental_100plus_tax_amount;

                        for (int j = 0; j <= dt.Rows.Count - 1; j++)
                        {

                            if (my_plan.Sum_Insured == 0)
                            {
                                total_sum_insure += Convert.ToDouble(dt.Rows[j][5].ToString().Trim());
                            }
                            else
                            {
                                total_sum_insure += my_plan.Sum_Insured;
                            }

                            //Column 0 = employee ID; 1 = employee name; 2 = gender; 3 = dob; 4 = position                           

                            DateTime dob = Convert.ToDateTime(dt.Rows[j][3].ToString().Trim(), dtfi2);

                            //Insert Certificate
                            bl_gtli_certificate certificate = new bl_gtli_certificate();
                            certificate.GTLI_Policy_ID = policy.GTLI_Policy_ID;
                            certificate.GTLI_Company_ID = policy.GTLI_Company_ID;
                            certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");

                            Certificate_Number += 1;

                            certificate.Certificate_Number = Certificate_Number;

                            if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            //Insert Employee
                            bl_gtli_employee employee = new bl_gtli_employee();
                            employee.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;
                            employee.Position = dt.Rows[j][4].ToString().Trim();
                            employee.Gender = dt.Rows[j][2].ToString().Trim();
                            employee.Employee_Name = dt.Rows[j][1].ToString().Trim();
                            employee.Employee_ID = dt.Rows[j][0].ToString().Trim();
                            employee.DOB = dob;
                            employee.Customer_Status = 1;

                            if (!da_gtli_employee_temporary.InsertEmployeeTemporary(employee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                return;
                            }


                            //calculate premium

                            decimal original_life_premium = 0;

                            if (policy.Policy_Number == "GL00000001" || policy.Policy_Number == "GL00000002" || policy.Policy_Number == "GL00000003" || policy.Policy_Number == "GL00000004" || policy.Policy_Number == "GL00000005" || policy.Policy_Number == "GL00000006" || policy.Policy_Number == "GL00000007" || policy.Policy_Number == "GL00000008")
                            {
                                original_life_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                            }
                            else if (policy.Policy_Number == "GL00000009" || policy.Policy_Number == "GL00000010")
                            {
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                            }
                            else if (policy.Policy_Number == "GL00000011" || policy.Policy_Number == "GL00000012" || policy.Policy_Number == "GL00000013")
                            {
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                            }
                            else
                            {
                                //New rate for new policy which effactive date start from 12/april/2016
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "11", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                           
                            }

                            //calculate premium for remaining days
                            decimal original_life_remain_premium = Math.Ceiling((original_life_premium * remain_days) / total_days);

                            total_new_original_life_premium += original_life_remain_premium;

                            string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium life
                            bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                            employee_premium_life.Premium_Type = "Death";
                            employee_premium_life.Premium = original_life_remain_premium.ToString();
                            employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                            employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                            if (my_plan.Sum_Insured == 0)
                            {
                                employee_premium_life.Sum_Insured = dt.Rows[j][5].ToString();
                            }
                            else
                            {
                                employee_premium_life.Sum_Insured = my_plan.Sum_Insured.ToString();
                            }


                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            //if has TPD
                            if (my_plan.TPD_Option_Value == 1)
                            {

                                //calculate premium TPD
                                decimal original_tpd_premium = 0;

                                if (policy.Policy_Number == "GL00000001" || policy.Policy_Number == "GL00000002" || policy.Policy_Number == "GL00000003" || policy.Policy_Number == "GL00000004" || policy.Policy_Number == "GL00000005" || policy.Policy_Number == "GL00000006" || policy.Policy_Number == "GL00000007" || policy.Policy_Number == "GL00000008")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000009" || policy.Policy_Number == "GL00000010")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000011" || policy.Policy_Number == "GL00000012" || policy.Policy_Number == "GL00000013")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else 
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "13", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }

                                //calculate premium TPD for remaining days
                                decimal original_tpd_remain_premium = Math.Ceiling((original_tpd_premium * remain_days) / total_days);

                                total_new_original_tpd_premium += original_tpd_remain_premium;

                                string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                                //Insert employee premium tpd
                                bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                                employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                                employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                                employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                                employee_premium_tpd.Premium = original_tpd_remain_premium.ToString();
                                employee_premium_tpd.Premium_Type = "TPD";


                                if (my_plan.Sum_Insured == 0)
                                {
                                    employee_premium_tpd.Sum_Insured = dt.Rows[j][5].ToString();
                                }
                                else
                                {
                                    employee_premium_tpd.Sum_Insured = my_plan.Sum_Insured.ToString();
                                }

                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                    return;
                                }

                            }

                            //if has Accidental 100Plus
                            if (my_plan.Accidental_100Plus_Option_Value == 1)
                            {

                                //calculate premium TPD
                                decimal original_accidental_100plus_premium = 0;

                                if (policy.Policy_Number == "GL00000001" || policy.Policy_Number == "GL00000002" || policy.Policy_Number == "GL00000003" || policy.Policy_Number == "GL00000004" || policy.Policy_Number == "GL00000005" || policy.Policy_Number == "GL00000006" || policy.Policy_Number == "GL00000007" || policy.Policy_Number == "GL00000008")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000009" || policy.Policy_Number == "GL00000010")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000011" || policy.Policy_Number == "GL00000012" || policy.Policy_Number == "GL00000013")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else 
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "13", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }

                                //calculate premium accidental 100plus for remaining days
                                decimal original_accidental_100plus_remain_premium = Math.Ceiling((original_accidental_100plus_premium * remain_days) / total_days);

                                total_new_original_accidental_100plus_premium += original_accidental_100plus_remain_premium;

                                string gtli_employee_premium_temporary_id_accidental_100plus = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                                //Insert employee premium accidental 100plus
                                bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                                employee_premium_accidental_100plus.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                                employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_accidental_100plus;
                                employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                                employee_premium_accidental_100plus.Premium = original_accidental_100plus_remain_premium.ToString();
                                employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                                if (my_plan.Sum_Insured == 0)
                                {
                                    employee_premium_accidental_100plus.Sum_Insured = dt.Rows[j][5].ToString();
                                }
                                else
                                {
                                    employee_premium_accidental_100plus.Sum_Insured = my_plan.Sum_Insured.ToString();
                                }


                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                    return;
                                }
                              
                            }

                            int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, add_effective_date);

                            string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium dhc for age within 18 - 59
                            if (customer_age >= 18 && customer_age < 60)
                            {
                                //dhc premium for remaining days
                                decimal original_dhc_premium = my_plan.DHC_Option_Value;

                                decimal original_dhc_remain_premium = Math.Ceiling((original_dhc_premium * remain_days) / total_days);

                                total_new_original_dhc_premium += original_dhc_remain_premium;

                                bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                                employee_premium_dhc.Premium_Type = "DHC";
                                employee_premium_dhc.Premium = original_dhc_remain_premium.ToString();
                                employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                                employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                                employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                                if (my_plan.Sum_Insured == 0)
                                {
                                    employee_premium_dhc.Sum_Insured = dt.Rows[j][5].ToString();
                                }
                                else
                                {
                                    employee_premium_dhc.Sum_Insured = my_plan.Sum_Insured.ToString();
                                }


                                if (original_dhc_premium > 0) //Insert only dhc premium > 0
                                {
                                    if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                                    {
                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                        return;
                                    }

                                }
                            }



                        }//End loop dt


                        //Get discount for each premium type
                        #region
                        //if discount > 0
                        //if discount > 0
                        if (discount > 0)
                        {

                            //calculate discount for accidental 100 plus
                            if (my_plan.Accidental_100Plus_Option_Value == 1)
                            {
                                total_new_accidental_100plus_premium_discount = Math.Floor((total_new_original_accidental_100plus_premium * Convert.ToDecimal(discount)) / 100);

                            }

                            //calculate discount for tpd
                            if (my_plan.TPD_Option_Value == 1)
                            {
                                total_new_tpd_premium_discount = Math.Floor((total_new_original_tpd_premium * Convert.ToDecimal(discount)) / 100);

                            }

                            //calculate discount for dhc
                            if (my_plan.DHC_Option_Value > 0)
                            {
                                total_new_dhc_premium_discount = Math.Floor((total_new_original_dhc_premium * Convert.ToDecimal(discount)) / 100);

                            }


                            total_new_life_premium_discount = Math.Floor((total_new_original_life_premium * Convert.ToDecimal(discount)) / 100);


                        }
                        #endregion

                        //Get tax amount on premium after discount
                        #region

                        total_new_accidental_100plus_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_accidental_100plus_premium, total_new_accidental_100plus_premium_discount);
                        total_new_life_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_life_premium, total_new_life_premium_discount);
                        total_new_tpd_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_tpd_premium, total_new_tpd_premium_discount);
                        total_new_dhc_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_dhc_premium, total_new_dhc_premium_discount);

                        #endregion

                        //Premium after discount
                        total_new_accidental_100plus_premium = total_new_original_accidental_100plus_premium - total_new_accidental_100plus_premium_discount;
                        total_new_life_premium = total_new_original_life_premium - total_new_life_premium_discount;
                        total_new_tpd_premium = total_new_original_tpd_premium - total_new_tpd_premium_discount;
                        total_new_dhc_premium = total_new_original_dhc_premium - total_new_dhc_premium_discount;

                        //Insert prem pay
                        bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
                        prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                        prem_pay.Prem_Year = policy_year;
                        prem_pay.Prem_Lot = 1;
                        prem_pay.Pay_Mode_ID = 1;
                        prem_pay.Pay_Date = add_effective_date;
                        prem_pay.Office_ID = channel_location_id;
                        prem_pay.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                        prem_pay.Due_Date = policy.Expiry_Date;
                        prem_pay.Created_On = DateTime.Now;
                        prem_pay.Created_Note = "";
                        prem_pay.Created_By = hdfusername.Value;
                        prem_pay.Status = 1;
                        prem_pay.Payment_Code = "";

                        prem_pay.Amount = Convert.ToDouble(total_new_dhc_premium + total_new_life_premium + total_new_tpd_premium + total_new_accidental_100plus_premium);

                        if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(prem_pay, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            return;
                        }


                        //update total premium in premium temporary
                        da_gtli_premium_temporary.UpdatePremium(total_sum_insure, Convert.ToDouble(total_new_life_premium), Convert.ToDouble(total_new_tpd_premium),
                            Convert.ToDouble(total_new_dhc_premium), Convert.ToDouble(total_new_accidental_100plus_premium),
                            Convert.ToDouble(total_new_accidental_100plus_premium_discount), Convert.ToDouble(total_new_life_premium_discount),
                            Convert.ToDouble(total_new_tpd_premium_discount), Convert.ToDouble(total_new_dhc_premium_discount),
                            Convert.ToDouble(total_new_original_accidental_100plus_premium), Convert.ToDouble(total_new_original_life_premium),
                            Convert.ToDouble(total_new_original_tpd_premium), Convert.ToDouble(total_new_original_dhc_premium),
                            Convert.ToDouble(total_new_accidental_100plus_tax_amount), Convert.ToDouble(total_new_life_premium_tax_amount),
                            Convert.ToDouble(total_new_tpd_premium_tax_amount), Convert.ToDouble(total_new_dhc_premium_tax_amount), gtli_premium.GTLI_Premium_ID);

                        //da_gtli_premium.UpdatePremium(total_sum_insure, Convert.ToDouble(total_new_life_premium), Convert.ToDouble(total_new_tpd_premium),
                        //   Convert.ToDouble(total_new_dhc_premium), Convert.ToDouble(total_new_accidental_100plus_premium),
                        //   Convert.ToDouble(total_new_accidental_100plus_premium_discount), Convert.ToDouble(total_new_life_premium_discount),
                        //   Convert.ToDouble(total_new_tpd_premium_discount), Convert.ToDouble(total_new_dhc_premium_discount),
                        //   Convert.ToDouble(total_new_original_accidental_100plus_premium), Convert.ToDouble(total_new_original_life_premium),
                        //   Convert.ToDouble(total_new_original_tpd_premium), Convert.ToDouble(total_new_original_dhc_premium),
                        //   Convert.ToDouble(total_new_accidental_100plus_tax_amount), Convert.ToDouble(total_new_life_premium_tax_amount),
                        //   Convert.ToDouble(total_new_tpd_premium_tax_amount), Convert.ToDouble(total_new_dhc_premium_tax_amount), gtli_premium.GTLI_Premium_ID);

                        Response.Redirect("add_new_transaction_underwrite_detail.aspx?pid=" + gtli_premium.GTLI_Premium_ID);

                    }
                }

                #endregion

                if (version == ".xlsx")
                {
                    bool invalid_input = false;
                    int old_certificate_number = 0;

                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filepath + "';Extended Properties=Excel 12.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [GClientData$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[GClientData$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    //Validate correct Data from file

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessageAdd.Text = "Please check your input of employee name field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessageAdd.Text = "Please check your input for gender field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                        {
                            lblMessageAdd.Text = "Please check your input for date field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (my_plan.Sum_Insured == 0)
                        {

                            if (!Helper.IsNumeric(dt.Rows[i][5].ToString()))
                            {
                                lblMessageAdd.Text = "Please check your input for sum insure field then try again. Row number: " + (i + 2) + "";
                                return;
                            }
                        }

                    }

                    //if all valid input then continue update for (upload input option)
                    if (invalid_input == false)
                    {
                        //Start adding new employees, recalculating total premium

                        //check add date with expiry date
                        if (policy.Expiry_Date.CompareTo(add_effective_date) != 1) //before effective date
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Effective date must not be later than or equal to expiry date. Please check again.')", true);
                            return;
                        }

                        //GTLI Premium
                        bl_gtli_premium gtli_premium = new bl_gtli_premium();
                        gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                        gtli_premium.Channel_Location_ID = channel_location_id;
                        gtli_premium.Created_By = hdfusername.Value;
                        gtli_premium.Created_On = DateTime.Now;
                        gtli_premium.DHC_Option_Value = Convert.ToInt32(my_plan.DHC_Option_Value);
                        gtli_premium.DHC_Premium = 0;
                        gtli_premium.Effective_Date = add_effective_date;
                        gtli_premium.Expiry_Date = policy.Expiry_Date;
                        gtli_premium.GTLI_Plan_ID = my_plan.GTLI_Plan_ID;
                        gtli_premium.GTLI_Policy_ID = policy.GTLI_Policy_ID;


                        gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                       
                        gtli_premium.Life_Premium = 0;
                        gtli_premium.Pay_Mode_ID = 1;
                        gtli_premium.Policy_Year = policy_year;
                        gtli_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                        gtli_premium.Sum_Insured = my_plan.Sum_Insured;
                        
                        gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                        gtli_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);
                        gtli_premium.TPD_Premium = 0;
                        gtli_premium.Accidental_100Plus_Premium = 0;
                        gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                        gtli_premium.DHC_Premium_Discount = 0;
                        gtli_premium.Life_Premium_Discount = 0;
                        gtli_premium.TPD_Premium_Discount = 0;
                        gtli_premium.Discount = Convert.ToDouble(discount);
                        gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_premium.Life_Premium_Tax_Amount = 0;
                        gtli_premium.TPD_Premium_Tax_Amount = 0;
                        gtli_premium.DHC_Premium_Tax_Amount = 0;
                        gtli_premium.Original_Accidental_100Plus_Premium = 0;
                        gtli_premium.Original_DHC_Premium = 0;
                        gtli_premium.Original_Life_Premium = 0;
                        gtli_premium.Original_TPD_Premium = 0;

                        //Transaction_Type 1=new 
                        //Transaction_Type 2=add member
                        //Transaction_Type 3=resigned

                        gtli_premium.Transaction_Type = 2;

                        //if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                        //{
                        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                        //    return;

                        //}
                        //===============================================

                        //Get Number of Days Used
                        int used_days = CalculateUsedDays(policy.Effective_Date, add_effective_date);
                        int total_days = 365;

                        int remain_days = total_days - used_days;

                        decimal total_new_original_life_premium = 0;
                        decimal total_new_original_tpd_premium = 0;
                        decimal total_new_original_dhc_premium = 0;
                        decimal total_new_original_accidental_100plus_premium = 0;

                        decimal total_new_life_premium = 0;
                        decimal total_new_tpd_premium = 0;
                        decimal total_new_dhc_premium = 0;
                        decimal total_new_accidental_100plus_premium = 0;

                        decimal total_new_life_premium_discount = 0;
                        decimal total_new_tpd_premium_discount = 0;
                        decimal total_new_dhc_premium_discount = 0;
                        decimal total_new_accidental_100plus_premium_discount = 0;

                        decimal total_new_life_premium_tax_amount;
                        decimal total_new_tpd_premium_tax_amount;
                        decimal total_new_dhc_premium_tax_amount;
                        decimal total_new_accidental_100plus_tax_amount;

                        for (int j = 0; j <= dt.Rows.Count - 1; j++)
                        {

                            if (my_plan.Sum_Insured == 0)
                            {
                                total_sum_insure += Convert.ToDouble(dt.Rows[j][5].ToString().Trim());
                            }
                            else
                            {
                                total_sum_insure += my_plan.Sum_Insured;
                            }

                            //Column 0 = employee ID; 1 = employee name; 2 = gender; 3 = dob; 4 = position                           

                            DateTime dob = Convert.ToDateTime(dt.Rows[j][3].ToString().Trim(), dtfi2);

                            bl_gtli_certificate certificate = new bl_gtli_certificate();
                            
                            //get old certificate number for old employee
                            old_certificate_number = da_gtli_certificate.GetGTLICertificateNumberByPolicyIDAndEmployeeName(policy.GTLI_Policy_ID, dt.Rows[j][1].ToString().Trim());

                            if (old_certificate_number != 0)
                            {
                                Certificate_Number = old_certificate_number;
                            }
                            else
                            {
                              
                                //last certificate
                                int lastCertificate = 0;
                                int lastCertificateTemporary = 0;

                                lastCertificate = da_gtli_employee.GetLastCertificateNoByCompanyID(policy.GTLI_Company_ID);
                                //lastCertificateTemporary from temp table
                                lastCertificateTemporary = da_gtli_certificate_temporary.GetLastCertificateTemporayNumber(policy.GTLI_Company_ID);

                                if (lastCertificate > lastCertificateTemporary)
                                {

                                    Certificate_Number = lastCertificate + 1;

                                }
                                else {

                                    Certificate_Number = lastCertificateTemporary + 1;

                                }
                                
                            }
                            
                            certificate.GTLI_Policy_ID = policy.GTLI_Policy_ID;
                            certificate.GTLI_Company_ID = policy.GTLI_Company_ID;
                            certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");
                            certificate.Certificate_Number = Certificate_Number;
                            
                            //Insert Certificate
                            if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            //Insert Employee
                            bl_gtli_employee employee = new bl_gtli_employee();
                            employee.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;
                            employee.Position = dt.Rows[j][4].ToString().Trim();
                            employee.Gender = dt.Rows[j][2].ToString().Trim();
                            employee.Employee_Name = dt.Rows[j][1].ToString().Trim();
                            employee.Employee_ID = dt.Rows[j][0].ToString().Trim();
                            employee.DOB = dob;
                            employee.Customer_Status = 1;

                            if (!da_gtli_employee_temporary.InsertEmployeeTemporary(employee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            //calculate premium
                            decimal original_life_premium = 0;

                            if (policy.Policy_Number == "GL00000001" || policy.Policy_Number == "GL00000002" || policy.Policy_Number == "GL00000003" || policy.Policy_Number == "GL00000004" || policy.Policy_Number == "GL00000005" || policy.Policy_Number == "GL00000006" || policy.Policy_Number == "GL00000007" || policy.Policy_Number == "GL00000008")
                            {
                               // original_life_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                            }
                            else if (policy.Policy_Number == "GL00000009" || policy.Policy_Number == "GL00000010")
                            {
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                            }
                            else if (policy.Policy_Number == "GL00000011" || policy.Policy_Number == "GL00000012" || policy.Policy_Number == "GL00000013")
                            {
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                            }
                            else 
                            //new rate effected from 4/12/2016
                            //life:productid=11, TPD:productid=13, 100 plus:productid=12
                            {
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[j][2].ToString(), Convert.ToDecimal(my_plan.Sum_Insured), "11", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                            }

                            //calculate premium for remaining days
                            decimal original_life_remain_premium = Math.Ceiling((original_life_premium * remain_days) / total_days);

                            total_new_original_life_premium += original_life_remain_premium;

                            string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");
                          
                            //Insert employee premium life
                            bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                            employee_premium_life.Premium_Type = "Death";
                            employee_premium_life.Premium = original_life_remain_premium.ToString();
                            employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                            employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                            if (my_plan.Sum_Insured == 0)
                            {
                                employee_premium_life.Sum_Insured = dt.Rows[j][2].ToString();
                            }
                            else
                            {
                                employee_premium_life.Sum_Insured = my_plan.Sum_Insured.ToString();
                            }


                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            //if has TPD
                            if (my_plan.TPD_Option_Value == 1)
                            {

                                //calculate premium TPD
                                decimal original_tpd_premium = 0;

                                if (policy.Policy_Number == "GL00000001" || policy.Policy_Number == "GL00000002" || policy.Policy_Number == "GL00000003" || policy.Policy_Number == "GL00000004" || policy.Policy_Number == "GL00000005" || policy.Policy_Number == "GL00000006" || policy.Policy_Number == "GL00000007" || policy.Policy_Number == "GL00000008")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000009" || policy.Policy_Number == "GL00000010")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000011" || policy.Policy_Number == "GL00000012" || policy.Policy_Number == "GL00000013")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }

                                  else
                                    //new rate effected from 12/04/2015
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "13", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }

                                //calculate premium TPD for remaining days
                                decimal original_tpd_remain_premium = Math.Ceiling((original_tpd_premium * remain_days) / total_days);

                                total_new_original_tpd_premium += original_tpd_remain_premium;

                                string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");
                               
                                //Insert employee premium tpd
                                bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                                employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                                employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                                employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                                employee_premium_tpd.Premium = original_tpd_remain_premium.ToString();
                                employee_premium_tpd.Premium_Type = "TPD";

                                if (my_plan.Sum_Insured == 0)
                                {
                                    employee_premium_tpd.Sum_Insured = dt.Rows[j][2].ToString();
                                }
                                else
                                {
                                    employee_premium_tpd.Sum_Insured = my_plan.Sum_Insured.ToString();
                                }

                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                    return;
                                }


                            }

                            //if has Accidental 100Plus
                            if (my_plan.Accidental_100Plus_Option_Value == 1)
                            {

                                //calculate premium TPD
                                decimal original_accidental_100plus_premium = 0;

                                if (policy.Policy_Number == "GL00000001" || policy.Policy_Number == "GL00000002" || policy.Policy_Number == "GL00000003" || policy.Policy_Number == "GL00000004" || policy.Policy_Number == "GL00000005" || policy.Policy_Number == "GL00000006" || policy.Policy_Number == "GL00000007" || policy.Policy_Number == "GL00000008")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000009" || policy.Policy_Number == "GL00000010")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else if (policy.Policy_Number == "GL00000011" || policy.Policy_Number == "GL00000012" || policy.Policy_Number == "GL00000013")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                }
                                else 
                                    //new rate effected from 12/04/2016
                                {

                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[j][2].ToString().Trim(), Convert.ToDecimal(my_plan.Sum_Insured), "12", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), add_effective_date);
                                
                                }


                                //calculate premium accidental 100plus for remaining days
                                decimal original_accidental_100plus_remain_premium = Math.Ceiling((original_accidental_100plus_premium * remain_days) / total_days);

                                total_new_original_accidental_100plus_premium += original_accidental_100plus_remain_premium;

                                string gtli_employee_premium_temporary_id_accidental_100plus = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");
                              
                                //Insert employee premium accidental 100plus
                                bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                                employee_premium_accidental_100plus.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                                employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_accidental_100plus;
                                employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                                employee_premium_accidental_100plus.Premium = original_accidental_100plus_remain_premium.ToString();
                                employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                                if (my_plan.Sum_Insured == 0)
                                {
                                    employee_premium_accidental_100plus.Sum_Insured = dt.Rows[j][2].ToString();
                                }
                                else
                                {
                                    employee_premium_accidental_100plus.Sum_Insured = my_plan.Sum_Insured.ToString();
                                }

                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                    return;
                                }


                            }

                            int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, add_effective_date);

                            string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");
                           
                            //Insert employee premium dhc for age within 18 - 59
                            if (customer_age >= 18 && customer_age < 60)
                            {
                                //dhc premium for remaining days
                                decimal original_dhc_premium = my_plan.DHC_Option_Value;

                                decimal original_dhc_remain_premium = Math.Ceiling((original_dhc_premium * remain_days) / total_days);

                                total_new_original_dhc_premium += original_dhc_remain_premium;

                                bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                                employee_premium_dhc.Premium_Type = "DHC";
                                employee_premium_dhc.Premium = original_dhc_remain_premium.ToString();
                                employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                                employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                                employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                                if (my_plan.Sum_Insured == 0)
                                {
                                    employee_premium_dhc.Sum_Insured = dt.Rows[j][2].ToString();
                                }
                                else
                                {
                                    employee_premium_dhc.Sum_Insured = my_plan.Sum_Insured.ToString();
                                }

                                if (original_dhc_premium > 0) //Insert only dhc premium > 0
                                {
                                    if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                                    {
                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                                        return;
                                    }

                                }
                            }



                        }//End loop dt


                        //Get discount for each premium type
                        #region
                        //if discount > 0
                        //if discount > 0
                        if (discount > 0)
                        {

                            //calculate discount for accidental 100 plus
                            if (my_plan.Accidental_100Plus_Option_Value == 1)
                            {
                                total_new_accidental_100plus_premium_discount = Math.Floor((total_new_original_accidental_100plus_premium * Convert.ToDecimal(discount)) / 100);

                            }

                            //calculate discount for tpd
                            if (my_plan.TPD_Option_Value == 1)
                            {
                                total_new_tpd_premium_discount = Math.Floor((total_new_original_tpd_premium * Convert.ToDecimal(discount)) / 100);

                            }

                            //calculate discount for dhc
                            if (my_plan.DHC_Option_Value > 0)
                            {
                                total_new_dhc_premium_discount = Math.Floor((total_new_original_dhc_premium * Convert.ToDecimal(discount)) / 100);

                            }


                            total_new_life_premium_discount = Math.Floor((total_new_original_life_premium * Convert.ToDecimal(discount)) / 100);


                        }
                        #endregion

                        //Get tax amount on premium after discount
                        #region

                        total_new_accidental_100plus_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_accidental_100plus_premium, total_new_accidental_100plus_premium_discount);
                        total_new_life_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_life_premium, total_new_life_premium_discount);
                        total_new_tpd_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_tpd_premium, total_new_tpd_premium_discount);
                        total_new_dhc_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_new_original_dhc_premium, total_new_dhc_premium_discount);

                        #endregion

                        //Premium after discount
                        total_new_accidental_100plus_premium = total_new_original_accidental_100plus_premium - total_new_accidental_100plus_premium_discount;
                        total_new_life_premium = total_new_original_life_premium - total_new_life_premium_discount;
                        total_new_tpd_premium = total_new_original_tpd_premium - total_new_tpd_premium_discount;
                        total_new_dhc_premium = total_new_original_dhc_premium - total_new_dhc_premium_discount;

                        //Premium in temp table

                        gtli_premium.Life_Premium = Convert.ToDouble(total_new_life_premium);
                        gtli_premium.TPD_Premium = Convert.ToDouble(total_new_tpd_premium);
                        gtli_premium.DHC_Premium = Convert.ToDouble(total_new_dhc_premium);
                        gtli_premium.Accidental_100Plus_Premium = Convert.ToDouble(total_new_accidental_100plus_premium);
                        gtli_premium.Accidental_100Plus_Premium_Discount = Convert.ToDouble(total_new_accidental_100plus_premium_discount);
                        gtli_premium.DHC_Premium_Discount = Convert.ToDouble(total_new_dhc_premium_discount);
                        gtli_premium.Life_Premium_Discount = Convert.ToDouble(total_new_life_premium_discount);
                        gtli_premium.TPD_Premium_Discount = Convert.ToDouble(total_new_tpd_premium_discount);
                        gtli_premium.Discount = Convert.ToDouble(discount);
                        gtli_premium.Accidental_100Plus_Premium_Tax_Amount = Convert.ToDouble(total_new_accidental_100plus_tax_amount);
                        gtli_premium.Life_Premium_Tax_Amount = Convert.ToDouble(total_new_life_premium_tax_amount);
                        gtli_premium.TPD_Premium_Tax_Amount = Convert.ToDouble(total_new_tpd_premium_tax_amount);
                        gtli_premium.DHC_Premium_Tax_Amount = Convert.ToDouble(total_new_dhc_premium_tax_amount);
                        gtli_premium.Original_Accidental_100Plus_Premium = Convert.ToDouble(total_new_original_accidental_100plus_premium);
                        gtli_premium.Original_DHC_Premium = Convert.ToDouble(total_new_original_dhc_premium);
                        gtli_premium.Original_Life_Premium = Convert.ToDouble(total_new_original_life_premium);
                        gtli_premium.Original_TPD_Premium = Convert.ToDouble(total_new_original_tpd_premium);
                       

                        //total sum insured, sum all insured.
                        gtli_premium.Sum_Insured = total_sum_insure;


                        if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Add gtli employee failed. Please check your inputs again.')", true);
                            return;

                        }

                        //Insert prem pay
                        bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
                        prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                        prem_pay.Prem_Year = policy_year;
                        prem_pay.Prem_Lot = 1;
                        prem_pay.Pay_Mode_ID = 1;
                        prem_pay.Pay_Date = add_effective_date;
                        prem_pay.Office_ID = channel_location_id;
                        prem_pay.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                        prem_pay.Due_Date = policy.Expiry_Date;
                        prem_pay.Created_On = DateTime.Now;
                        prem_pay.Created_Note = "";
                        prem_pay.Created_By = hdfusername.Value;
                        prem_pay.Status = 1;
                        prem_pay.Payment_Code = "";

                        prem_pay.Amount = Convert.ToDouble(total_new_dhc_premium + total_new_life_premium + total_new_tpd_premium + total_new_accidental_100plus_premium);

                        if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(prem_pay, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            return;
                        }

                        //update total premium in premium temporary
                        //da_gtli_premium_temporary.UpdatePremium(total_sum_insure, Convert.ToDouble(total_new_life_premium),
                        //    Convert.ToDouble(total_new_tpd_premium), Convert.ToDouble(total_new_dhc_premium),
                        //    Convert.ToDouble(total_new_accidental_100plus_premium), Convert.ToDouble(total_new_accidental_100plus_premium_discount),
                        //    Convert.ToDouble(total_new_life_premium_discount), Convert.ToDouble(total_new_tpd_premium_discount),
                        //    Convert.ToDouble(total_new_dhc_premium_discount), Convert.ToDouble(total_new_original_accidental_100plus_premium),
                        //    Convert.ToDouble(total_new_original_life_premium), Convert.ToDouble(total_new_original_tpd_premium),
                        //    Convert.ToDouble(total_new_original_dhc_premium), Convert.ToDouble(total_new_accidental_100plus_tax_amount),
                        //    Convert.ToDouble(total_new_life_premium_tax_amount), Convert.ToDouble(total_new_tpd_premium_tax_amount),
                        //    Convert.ToDouble(total_new_dhc_premium_tax_amount), gtli_premium.GTLI_Premium_ID);

                        //da_gtli_premium.UpdatePremium(total_sum_insure, Convert.ToDouble(total_new_life_premium),
                        //   Convert.ToDouble(total_new_tpd_premium), Convert.ToDouble(total_new_dhc_premium),
                        //   Convert.ToDouble(total_new_accidental_100plus_premium), Convert.ToDouble(total_new_accidental_100plus_premium_discount),
                        //   Convert.ToDouble(total_new_life_premium_discount), Convert.ToDouble(total_new_tpd_premium_discount),
                        //   Convert.ToDouble(total_new_dhc_premium_discount), Convert.ToDouble(total_new_original_accidental_100plus_premium),
                        //   Convert.ToDouble(total_new_original_life_premium), Convert.ToDouble(total_new_original_tpd_premium),
                        //   Convert.ToDouble(total_new_original_dhc_premium), Convert.ToDouble(total_new_accidental_100plus_tax_amount),
                        //   Convert.ToDouble(total_new_life_premium_tax_amount), Convert.ToDouble(total_new_tpd_premium_tax_amount),
                        //   Convert.ToDouble(total_new_dhc_premium_tax_amount), gtli_premium.GTLI_Premium_ID);

                        Response.Redirect("add_new_transaction_underwrite_detail.aspx?pid=" + gtli_premium.GTLI_Premium_ID);
                    }

                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Require file upload!')", true);

            }

        }
        catch (ThreadAbortException th)
        {
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [btnAddEmployee_Click], page [Pages_GTLI_gtli_master_detail]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please contact system admin for problem diagnosis.')", true);

        }
    }

    //Find Used Days
    private static int CalculateUsedDays(System.DateTime sale_date, System.DateTime date_of_modify)
    {
        TimeSpan mytimespan = date_of_modify.Subtract(sale_date);
        int used_days = mytimespan.Days;

        return used_days;
    }
}
