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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Pages_GTLI_gtli_master_detail : System.Web.UI.Page
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

            string this_policy_id = Request.Params["pid"];      

            bl_gtli_policy this_policy = da_gtli_policy.GetGTLIPolicyByID(this_policy_id);

            string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(this_policy.GTLI_Policy_ID);
                       
            //disable button add new member and resign member if this policy is already expire
            int policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(this_policy_id);
            
            //get last policy year by policy number
            int last_policy_year = da_gtli_premium.GetGTLILastPolicyYearByPolicyNumber(this_policy.Policy_Number);             

            //get list of active employee for this sale transaction
            ArrayList list_of_active_employee = new ArrayList();
            list_of_active_employee = da_gtli_employee.GetListOfActiveEmployee(this_policy_id);

            ArrayList list_of_employee = new ArrayList();                   
                                
            //get list of plan
            string company_name = null;

            company_name = da_gtli_company.GetCompanyNameByID(this_policy.GTLI_Company_ID);

            List<bl_gtli_plan> plan_list = new List<bl_gtli_plan>();
            plan_list = da_gtli_plan.GetPlanListByCompanyName(company_name);

            if (plan_list.Count > 0)
            {
              
                ListItem item = new ListItem();
                item.Text = ".";
                item.Value = "0";

              
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
        //lblDate.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
        //lblDate2.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
        lblDate.Text = policy.Issue_Date.ToString("dd-MMM-yyyy");
        lblDate2.Text = policy.Issue_Date.ToString("dd-MMM-yyyy");

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

        double total_original_life_premium = 0;
        double total_original_accidental_100plus_premium = 0;
        double total_original_tpd_premium = 0;
        double total_original_dhc_premium = 0;

        double total_life_premium_discount = 0;
        double total_accidental_100plus_premium_discount = 0;
        double total_tpd_premium_discount = 0;
        double total_dhc_premium_discount = 0;

        string strTableDetailPrint = "<table class='gridtable' width='98%'>";
        strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

        strTableDetailPrint = "";
        int number = 0;
        int rows = 0;
      
         //Loop through premium list
        for (int i = 0; i <= premium_list.Count - 1; i++)
        {
            bl_gtli_premium premium = premium_list[i];
            string agent_name = da_sale_agent.GetSaleAgentNameByID(premium.Sale_Agent_ID);
          
            TimeSpan mytimespan = premium.Expiry_Date.Subtract(premium.Effective_Date);
            int days = mytimespan.Days + 1;

            ArrayList list_employee = new ArrayList();
            list_employee = da_gtli_employee.GetListOfEmployeeByGTLIPremiumID(premium.GTLI_Premium_ID);

            if (premium.Transaction_Type == 3)
            {
                total_accidental_100plus_premium -= premium.Accidental_100Plus_Premium;
                total_life_premium -= premium.Life_Premium;
                total_tpd_premium -= premium.TPD_Premium;
                total_dhc_premium -= premium.DHC_Premium;
            }
            else
            {
                total_accidental_100plus_premium += premium.Accidental_100Plus_Premium;
                total_life_premium += premium.Life_Premium;
                total_tpd_premium += premium.TPD_Premium;
                total_dhc_premium += premium.DHC_Premium;

                total_accidental_100plus_premium_discount += premium.Accidental_100Plus_Premium_Discount;
                total_life_premium_discount += premium.Life_Premium_Discount;
                total_tpd_premium_discount += premium.TPD_Premium_Discount;
                total_dhc_premium_discount += premium.DHC_Premium_Discount;
            }
                                  
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
                        total_original_dhc_premium += dhc_premium;
                        total_original_life_premium += life_premium;
                        total_original_tpd_premium += tpd_premium;
                        total_original_accidental_100plus_premium += accidental_100plus_premium;

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
                            if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium> 0)
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

                                if (premium.Transaction_Type == 1)
                                {
                                    tc_covery_period.InnerText = "365";
                                }
                                else
                                {
                                    tc_covery_period.InnerText = days.ToString();
                                }

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
                        total_original_dhc_premium -= dhc_premium;
                        total_original_life_premium -= life_premium;
                        total_original_tpd_premium -= tpd_premium;
                        total_original_accidental_100plus_premium -= accidental_100plus_premium;

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

        lblFirstPagePolicyNumber.Text = policy.Policy_Number;
        lblFirstPageCompanyName.Text = company.Company_Name;
        //lblFirstPageEffectiveDate.Text = policy.Effective_Date.ToString("d-MMM-yyyy");
        lblFirstPageEffectiveDate.Text = policy.Issue_Date.ToString("d-MMM-yyyy");

        //lblSecondPageCreateDate.Text = System.DateTime.Today.ToString("dd MMMM yyyy");
        lblSecondPageCreateDate.Text = policy.Issue_Date.ToString("dd MMMM yyyy");
        lblSecondPageContactName.Text = contact.Contact_Name;
        lblSecondPageCompanyName.Text = company.Company_Name;
        lblSecondPageAddress.Text = company.Company_Address;
        lblSecondPageContactPhone.Text = "Tel: " + contact.Contact_Phone;
        lblSecondPageDearContact.Text = "Dear " + contact.Contact_Name;
        lblSecondPagePolicyNumber.Text = policy.Policy_Number;

        lblForthPagePolicyNo.Text = policy.Policy_Number;
        //lblForthPagePolicyAnnual.Text = policy.Issue_Date.ToString("dd MMMM") + ",Annual";
        lblForthPagePolicyAnnual.Text = policy.Effective_Date.ToString("dd MMMM") + ",Annual";
        lblForthPageIssueDate.Text = policy.Issue_Date.ToString("dd MMMM yyyy");
        lblForthPageEffectiveDate.Text = policy.Effective_Date.ToString("dd MMMM yyyy");
        lblForthPageDueDate.Text = policy.Maturity_Date.ToString("dd MMMM yyyy");

        bl_gtli_plan plan = da_gtli_plan.GetLastPlanByCompanyID(company.GTLI_Company_ID);
        
        //by maneth: 3 Aug 2016, Show dynamic product list
        string strProductList = "";
       
        if (plan.TPD_Option_Value != 0) 
        {
            strProductList += "Group Total Permanent Disability (GTPD), ";
        }
        if(plan.DHC_Option_Value!=0)
        {
             strProductList += "Group Daily Hospital Cash (GDGC), ";
        }
        if (plan.Accidental_100Plus_Option_Value != 0)
        {
            strProductList += "100 plus, ";
        }

        if (strProductList.Trim() != "") 
        {
            strProductList = ", " + strProductList.Trim().Substring(0, strProductList.Trim().Length - 1);
        }
        
        lblProductList.Text = strProductList ;
        //for printing
        lblPrintProductList.Text = strProductList;

        //End my maneth

        dvSumInsured.InnerHtml = getPlanDescription(plan);
        lblForthPageFreeCoverLimit.Text = "USD" + plan.Sum_Insured.ToString("###,##0");

        lblFifthPagePolicyNumber.Text = policy.Policy_Number;
        //lblFiftPageCreateDate.Text = System.DateTime.Today.ToString("dd MMMM yyyy");
        lblFiftPageCreateDate.Text = policy.Issue_Date.ToString("dd MMMM yyyy");

        lblPolicyNumber.Text = policy.Policy_Number;
        lblCreatedDate.Text = policy.Created_On.ToString("d-MMM-yyyy");

        lblCompanyName.Text = company.Company_Name;
        lblTypeOfBusiness.Text = company.Type_Of_Business;
        lblContactName.Text = contact.Contact_Name;
        lblPhone.Text = contact.Contact_Phone;
        lblEmail.Text = contact.Contact_Email;
        lblAddress.Text = company.Company_Address;
        lblSumInsured.Text = total_sum_insured.ToString("C0");

        lblPremiumPayment.Text = (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2");
        lblPremiumDiscount.Text = (total_life_premium_discount + total_tpd_premium_discount + total_dhc_premium_discount + total_accidental_100plus_premium_discount).ToString("C2");
        lblPremiumAfterDiscount.Text = (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2");
        lblEffectiveDate.Text = policy.Effective_Date.ToString("d-MMM-yyyy");
        lblExpiryDate.Text = policy.Expiry_Date.ToString("d-MMM-yyyy");

        //Div plan innerhtml
        dvPlan.InnerHtml = myplan;

        //Summary Print
        lblPrintFirstPagePolicyNumber.Text = policy.Policy_Number;
        lblPrintFirstPageCompanyName.Text = company.Company_Name;

        //lblPrintFirstPageEffectiveDate.Text = policy.Effective_Date.ToString("d-MMM-yyyy");
        lblPrintFirstPageEffectiveDate.Text = policy.Issue_Date.ToString("d-MMM-yyyy");

        //lblPrintSecondPageCreateDate.Text = System.DateTime.Today.ToString("dd MMMM yyyy");
        lblPrintSecondPageCreateDate.Text =policy.Issue_Date.ToString("dd MMMM yyyy");

        lblPrintSecondPageContactName.Text = contact.Contact_Name;
        lblPrintSecondPageCompanyName.Text = company.Company_Name;
        lblPrintSecondPageAddress.Text = company.Company_Address;
        lblPrintSecondPageContactPhone.Text = "Tel: " + contact.Contact_Phone;
        lblPrintSecondPageDearContact.Text = "Dear " + contact.Contact_Name;
        lblPrintSecondPagePolicyNumber.Text = policy.Policy_Number;

        lblForthPagePrintPolicyNo.Text = policy.Policy_Number;
        //lblForthPagePrintPolicyAnnual.Text = policy.Issue_Date.ToString("dd MMMM") + ",Annual";
        lblForthPagePrintPolicyAnnual.Text = policy.Effective_Date.ToString("dd MMMM") + ",Annual";
        lblForthPagePrintIssueDate.Text = policy.Issue_Date.ToString("dd MMMM yyyy");

        //lblForthPagePrintEffectiveDate.Text = policy.Effective_Date.ToString("dd MMMM yyyy");
        lblForthPagePrintEffectiveDate.Text = policy.Issue_Date.ToString("dd MMMM yyyy");

        lblForthPagePrintDueDate.Text = policy.Maturity_Date.ToString("dd MMMM yyyy");

        plan = da_gtli_plan.GetLastPlanByCompanyID(company.GTLI_Company_ID);

        dvPrintSumInsured.InnerHtml = getPlanDescription(plan);
        lblForthPageFreeCoverLimit.Text = "USD" + plan.Sum_Insured.ToString("###,##0");

        lblFifthPagePrintPolicyNumber.Text = policy.Policy_Number;

        //lblFiftPagePrintCreateDate.Text = System.DateTime.Today.ToString("dd MMMM yyyy");
        lblFiftPagePrintCreateDate.Text =policy.Issue_Date.ToString("dd MMMM yyyy");

        lblPolicyNumber2.Text = policy.Policy_Number;
        lblCreatedDate2.Text = policy.Created_On.ToString("d-MMM-yyyy");

        lblCompanyName2.Text = company.Company_Name;
        lblTypeOfBusiness2.Text = company.Type_Of_Business;
        lblContactName2.Text = contact.Contact_Name;
        lblPhone2.Text = contact.Contact_Phone;
        lblEmail2.Text = contact.Contact_Email;
        lblAddress2.Text = company.Company_Address;
        lblSumInsured2.Text = total_sum_insured.ToString("C0");

        lblPremiumPayment2.Text = (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2");
        lblPremiumDiscount2.Text = (total_life_premium_discount + total_tpd_premium_discount + total_dhc_premium_discount + total_accidental_100plus_premium_discount).ToString("C2");
        lblPremiumAfterDiscount2.Text = (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2");
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

        if (total_original_life_premium > 0)
        {
            tc_life_premium_total.Style.Add("text-decoration", "underline");
            tc_life_premium_total.InnerText = total_original_life_premium.ToString("C2");
        }
        else
        {
            tc_life_premium_total.InnerText = " - ";
        }

        //Total Accidental 100Plus Premium
        HtmlTableCell tc_accidental_100plus_premium_total = new HtmlTableCell();
        tc_accidental_100plus_premium_total.Style.Add("text-align", "right");
        tc_accidental_100plus_premium_total.Style.Add("color", "black");
        tc_accidental_100plus_premium_total.Style.Add("padding-right", "6px");
        tc_accidental_100plus_premium_total.Style.Add("font-weight", "bold");

        if (total_original_life_premium > 0)
        {
            tc_accidental_100plus_premium_total.Style.Add("text-decoration", "underline");
            //tc_accidental_100plus_premium_total.InnerText = total_original_life_premium.ToString("C2");
            tc_accidental_100plus_premium_total.InnerText = total_original_accidental_100plus_premium.ToString("C2");
            
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

        if (total_original_tpd_premium > 0)
        {
            tc_tpd_premium_total.Style.Add("text-decoration", "underline");
            tc_tpd_premium_total.InnerText = total_original_tpd_premium.ToString("C2");
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

        if (total_original_dhc_premium > 0)
        {
            tc_dhc_premium_total.Style.Add("text-decoration", "underline");
            tc_dhc_premium_total.InnerText = total_original_dhc_premium.ToString("C2");
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

        //if (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium > 0)
        //{
        //    tc_total_premium.Style.Add("text-decoration", "underline");
        //    tc_total_premium.InnerText = (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_accidental_100plus_premium).ToString("C2");
        //}

        if (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium > 0)
        {
            tc_total_premium.Style.Add("text-decoration", "underline");
            tc_total_premium.InnerText = (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2");
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

        if (total_original_life_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_life_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }

        if (total_original_accidental_100plus_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_accidental_100plus_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }

        if (total_original_tpd_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_tpd_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }


        if (total_original_dhc_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_dhc_premium.ToString("C2") + "</td>";
        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }


        if (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium > 0)
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2") + "</td>";

        }
        else
        {
            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
        }

        strTableDetailPrint += "</table>";

        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

    }

 
    //Find Used Days
    private static int CalculateUsedDays(System.DateTime sale_date, System.DateTime date_of_modify)
    {
        TimeSpan mytimespan = date_of_modify.Subtract(sale_date);
        int used_days = mytimespan.Days;

        return used_days;
    }

    private string getPlanDescription(bl_gtli_plan plan)
    {
        string strPlanDescription = "<table>" +
                                "<tr>" +
                                "    <td style=\"text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px\">" +
                                "        Classification and plan:" +
                                "    </td>" +
                                "    <td style=\"text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px\">" +
                                "        Amount of Insurance/Benefit" +
                                "    </td>" +
                                "</tr>" +
                                "<tr>" +
                                "    <td>        " +
                                "    </td>" +
                                "    <td style=\"text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-weight: bold; font-size: 12px\">" +
                                "        Plan A" +
                                "    </td>" +
                                "</tr>" +
                                "<tr>" +
                                "    <td style=\"text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                                "        1.	Group Term Life Insurance " +
                                "    </td>" +
                                "    <td style=\"text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                                "USD " + plan.Sum_Insured +
                                "    </td>" +
                                "</tr>" ;
       
        
        if( plan.TPD_Option_Value != 0 || plan.DHC_Option_Value != 0 || plan.Accidental_100Plus_Option_Value !=0)
        {
            strPlanDescription += "<tr>" +
                                "    <td style=\"text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                                "        2.	Supplementary Contract: " +
                                "    </td>" +
                                "    <td>" +
                                "    </td>" +
                                "</tr>";
            //by maneth 3 Aug 2016 : show 100plus
            if (plan.Accidental_100Plus_Option_Value != 0)
            {

                strPlanDescription += "<tr>" +
                               "    <td style=\"text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                               "          -	100 Plus " +
                               "    </td>" +
                               "    <td style=\"text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">USD" + plan.Sum_Insured +
                               "    </td></tr>";
            }
            //end by maneth
            if (plan.TPD_Option_Value != 0)
            {
                strPlanDescription += "<tr>" +
                                        "    <td style=\"text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                                        "        -	Group Total Permanent Disability" +
                                        "    </td>" +
                                        "    <td style=\"text-align: center; font-family: Arial; font-size: 12px\">" +
                                        "USD " + plan.Sum_Insured +
                                        "    </td>" +
                                        "</tr>";
            }
            if (plan.DHC_Option_Value != 0)
            {
                //strPlanDescription += "<tr>" +
                //                "    <td style=\"text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                //                "        -	Group Daily Hospital Cash (pay " + plan.DHC_Option_Value + "$ per day maximum to)" +
                //                "    </td>" +
                //                "    <td style=\"text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                //                "        90 days per year " +
                //                "    </td>" +
                //                "</tr> ";
                //by maneth 3 Aug 2016 : show dynamic days per year
                strPlanDescription += "<tr>" +
                               "    <td style=\"text-align: left; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">" +
                               "        -	Group Daily Hospital Cash (pay 40$ per day maximum to)" +
                               "    </td>" +
                               "    <td style=\"text-align: center; font-family: Arial;padding-left:50px;padding-right:50px; font-size: 12px\">";

                               if(plan.DHC_Option_Value==10)
                               {

                                   strPlanDescription += "30 days per year     </td></tr>";
                               }
                               else if (plan.DHC_Option_Value == 20)
                               {

                                   strPlanDescription += "60 days per year     </td></tr>";
                               }
                               else if (plan.DHC_Option_Value == 30)
                               {

                                   strPlanDescription += "90 days per year     </td></tr>";
                               }
            }               
        }

        strPlanDescription += "</table>";

        return strPlanDescription;
    }
  
}