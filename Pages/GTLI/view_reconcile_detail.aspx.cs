using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_reconcile_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //dvPolicyDetail.Visible = false;
            dvAddMemberDetail.Visible = false;
            dvResignMemberDetail.Visible = false;

            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;
                      
            //get list of premium id from query string
            string query_string = Request.Params["pid"];

            ArrayList list_of_premium_id = new ArrayList();

            list_of_premium_id.AddRange(query_string.Split(','));

            lblDate.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
            lblDate2.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");

            lblTitle.Text = "Total Premium";

            
            double total_life_premium = 0;
            double total_tpd_premium = 0;
            double total_dhc_premium = 0;
            double total_sum_insured = 0;
            double total_accidental_100plus_premium = 0;

            double total_original_life_premium = 0;
            double total_original_accidental_100plus_premium = 0;
            double total_original_tpd_premium = 0;
            double total_original_dhc_premium = 0;

            double total_life_premium_discount = 0;
            double total_accidental_100plus_premium_discount = 0;
            double total_tpd_premium_discount = 0;
            double total_dhc_premium_discount = 0;

            bl_gtli_premium first_premium = da_gtli_premium.GetGTLIPremiumByID(list_of_premium_id[0].ToString());
            bl_gtli_policy policy = da_gtli_policy.GetGTLIPolicyByID(first_premium.GTLI_Policy_ID);


            bl_gtli_company company = da_gtli_company.GetObjCompanyByID(policy.GTLI_Company_ID);
            string sale_agent_name = da_sale_agent.GetSaleAgentNameByID(first_premium.Sale_Agent_ID);
            bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(policy.GTLI_Company_ID);

            int number = 0;
            //int policy_number = 0;
            int add_member_number = 0;
            int resign_member_number = 0;
            int rows = 0;
            //int policy_rows = 0;
            int add_member_rows = 0;
            int resign_member_rows = 0;
            string payment_code = "";
             
            double total_add_member_life_premium = 0;
            double total_add_member_tpd_premium = 0;
            double total_add_member_dhc_premium = 0;
            double total_add_member_sum_insured = 0;
            double total_add_member_accidental_100plus_premium = 0;

            double total_resign_member_life_premium = 0;
            double total_resign_member_tpd_premium = 0;
            double total_resign_member_dhc_premium = 0;
            double total_resign_member_sum_insured = 0;
            double total_resign_member_accidental_100plus_premium = 0;

            //Print Premium Detail Header
            string strTableDetailPrint = "<table class='gridtable' width='98%' border='1'>";
            strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

            dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

            strTableDetailPrint = "";

            //Print Add Member Detail Header
            string strTableAddMemberDetailPrint = "<table class='gridtable' width='98%' border='1'>";
            strTableAddMemberDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

            dvPrintAddMemberDetail.Controls.Add(new LiteralControl(strTableAddMemberDetailPrint));

            strTableAddMemberDetailPrint = "";

            //Print Resign Member Detail Header
            string strTableResignMemberDetailPrint = "<table class='gridtable' width='98%' border='1'>";
            strTableResignMemberDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Days not cover</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

            dvPrintResignMemberDetail.Controls.Add(new LiteralControl(strTableResignMemberDetailPrint));

            strTableResignMemberDetailPrint = "";

            //loop through list_of_premium_id
            for (int i = 0; i <= list_of_premium_id.Count - 1; i++)
            {
                bl_gtli_premium my_premium = new bl_gtli_premium();
                my_premium = da_gtli_premium.GetGTLIPremiumByID(list_of_premium_id[i].ToString());
                bl_gtli_policy my_policy = da_gtli_policy.GetGTLIPolicyByID(my_premium.GTLI_Policy_ID);

                if (my_premium.Transaction_Type == 2)//added
                {
                    #region

                        //Get payment code
                        if (payment_code == "")
                        {
                            payment_code = da_gtli_policy_prem_pay.GetPaymentCode(my_premium.GTLI_Premium_ID);
                        }

                        dvAddMemberDetail.Visible = true;

                        total_accidental_100plus_premium += my_premium.Accidental_100Plus_Premium;
                        total_life_premium += my_premium.Life_Premium;
                        total_tpd_premium += my_premium.TPD_Premium;
                        total_dhc_premium += my_premium.DHC_Premium;

                        total_accidental_100plus_premium_discount += my_premium.Accidental_100Plus_Premium_Discount;
                        total_life_premium_discount += my_premium.Life_Premium_Discount;
                        total_tpd_premium_discount += my_premium.TPD_Premium_Discount;
                        total_dhc_premium_discount += my_premium.DHC_Premium_Discount;

                        total_original_life_premium += my_premium.Original_Life_Premium;
                        total_original_tpd_premium += my_premium.Original_TPD_Premium;
                        total_original_dhc_premium += my_premium.Original_DHC_Premium;
                       // total_sum_insured += (my_premium.Sum_Insured * my_premium.Transaction_Staff_Number);
                        total_sum_insured += my_premium.Sum_Insured;
                        total_original_accidental_100plus_premium += my_premium.Original_Accidental_100Plus_Premium;

                        string plan_name = da_gtli_plan.GetPlan(my_premium.GTLI_Plan_ID).GTLI_Plan;

                        //employee premium for this policy transaction
                        ArrayList this_policy_employee = da_gtli_employee.GetListOfEmployeeByGTLIPremiumID(my_premium.GTLI_Premium_ID);

                  
                        //loop through employee list
                        for (int k = 0; k <= this_policy_employee.Count - 1; k++)
                        {
                            bl_gtli_employee employee = (bl_gtli_employee)this_policy_employee[k];
                            double add_member_detail_life_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "Death");
                            double add_member_detail_tpd_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "TPD");
                            double add_member_detail_dhc_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "DHC");
                            double add_member_detail_accidental_100plus_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "Accidental100Plus");
                            double add_member_detail_sum_insured = 0;

                            //add to total policy detail
                            total_add_member_dhc_premium += add_member_detail_dhc_premium;
                            total_add_member_life_premium += add_member_detail_life_premium;
                            total_add_member_tpd_premium += add_member_detail_tpd_premium;
                            total_add_member_accidental_100plus_premium += add_member_detail_accidental_100plus_premium;

                            //get suminsure for each employee
                            double newSumInsured = da_gtli_premium.getEmployeeSumInsured(employee.GTLI_Certificate_ID);

                            if (add_member_detail_life_premium == 0 && add_member_detail_tpd_premium == 0 && add_member_detail_dhc_premium == 0 && add_member_detail_accidental_100plus_premium == 0)
                            {

                                add_member_detail_sum_insured = 0;
                            }
                            else
                            {
                                total_add_member_sum_insured += newSumInsured;// my_premium.Sum_Insured;
                                add_member_detail_sum_insured =newSumInsured;// my_premium.Sum_Insured;
                            }

                            number += 1;
                            add_member_number += 1;

                            string str_certificate_no = employee.Certificate_Number.ToString();

                            while (str_certificate_no.Length < 6)
                            {
                                str_certificate_no = "0" + str_certificate_no;
                            }

                            TimeSpan mytimespan = my_premium.Expiry_Date.Subtract(my_premium.Effective_Date);
                            int coverage_period = mytimespan.Days + 1;

                            //Add row premium detail
                            //Row
                            HtmlTableRow tr_black = new HtmlTableRow();
                            tr_black.Style.Add("color", "Green");
                            tr_black.Style.Add("font-size", "8pt");

                            //Number
                            HtmlTableCell tc_number = new HtmlTableCell();
                            tc_number.Style.Add("text-align", "center");
                            tc_number.Style.Add("color", "black");
                            tc_number.InnerText = (number).ToString();

                            //Certificate Number
                            HtmlTableCell tc_certificate_number = new HtmlTableCell();
                            tc_certificate_number.Style.Add("color", "Green");
                            tc_certificate_number.Style.Add("padding-left", "5px");

                            //Employee Name
                            HtmlTableCell tc_employee_name = new HtmlTableCell();
                            tc_employee_name.Style.Add("text-align", "left");
                            tc_employee_name.Style.Add("padding-left", "5px");
                            tc_employee_name.Style.Add("color", "Green");
                            tc_employee_name.InnerText = employee.Employee_Name;

                            //Plan Name
                            HtmlTableCell tc_plan_name = new HtmlTableCell();
                            tc_plan_name.Style.Add("text-align", "center");
                            tc_plan_name.Style.Add("color", "Green");


                            //Effective Date
                            HtmlTableCell tc_effective_date = new HtmlTableCell();
                            tc_effective_date.Style.Add("text-align", "center");
                            tc_effective_date.Style.Add("color", "Green");


                            //Expiry Date
                            HtmlTableCell tc_expiry_date = new HtmlTableCell();
                            tc_expiry_date.Style.Add("text-align", "center");
                            tc_expiry_date.Style.Add("color", "Green");


                            //Cover Period
                            HtmlTableCell tc_covery_period = new HtmlTableCell();
                            tc_covery_period.Style.Add("text-align", "center");
                            tc_covery_period.Style.Add("color", "Green");


                            //Sum Insured
                            HtmlTableCell tc_sum_insured = new HtmlTableCell();
                            tc_sum_insured.Style.Add("text-align", "right");
                            tc_sum_insured.Style.Add("color", "Green");
                            tc_sum_insured.Style.Add("padding-right", "6px");

                            if (add_member_detail_sum_insured != 0)
                            {
                                tc_sum_insured.InnerText = add_member_detail_sum_insured.ToString("C0");// my_premium.Sum_Insured.ToString("C0");

                            }
                            else
                            {
                                tc_sum_insured.InnerText = " - ";

                            }

                            //Life Premium
                            HtmlTableCell tc_life_premium = new HtmlTableCell();
                            tc_life_premium.Style.Add("text-align", "right");
                            tc_life_premium.Style.Add("color", "Green");
                            tc_life_premium.Style.Add("padding-right", "6px");
                            if (add_member_detail_life_premium != 0)
                            {
                                tc_life_premium.InnerText = add_member_detail_life_premium.ToString("C2");
                            }
                            else
                            {
                                tc_life_premium.InnerText = " - ";
                            }

                            //Accidental 100Plus Premium
                            HtmlTableCell tc_accidental_100plus_premium = new HtmlTableCell();
                            tc_accidental_100plus_premium.Style.Add("text-align", "right");
                            tc_accidental_100plus_premium.Style.Add("color", "Green");
                            tc_accidental_100plus_premium.Style.Add("padding-right", "6px");
                            if (add_member_detail_accidental_100plus_premium != 0)
                            {
                                tc_accidental_100plus_premium.InnerText = add_member_detail_accidental_100plus_premium.ToString("C2");
                            }
                            else
                            {
                                tc_accidental_100plus_premium.InnerText = " - ";
                            }

                            //TPD Premium
                            HtmlTableCell tc_tpd_premium = new HtmlTableCell();
                            tc_tpd_premium.Style.Add("text-align", "right");
                            tc_tpd_premium.Style.Add("color", "Green");
                            tc_tpd_premium.Style.Add("padding-right", "6px");
                            if (add_member_detail_tpd_premium != 0)
                            {
                                tc_tpd_premium.InnerText = add_member_detail_tpd_premium.ToString("C2");
                            }
                            else
                            {
                                tc_tpd_premium.InnerText = " - ";
                            }

                            //DHC Premium
                            HtmlTableCell tc_dhc_premium = new HtmlTableCell();
                            tc_dhc_premium.Style.Add("text-align", "right");
                            tc_dhc_premium.Style.Add("color", "Green");
                            tc_dhc_premium.Style.Add("padding-right", "6px");
                            if (add_member_detail_dhc_premium != 0)
                            {
                                tc_dhc_premium.InnerText = add_member_detail_dhc_premium.ToString("C2");
                            }
                            else
                            {
                                tc_dhc_premium.InnerText = " - ";
                            }

                            //Sub Total Premium
                            HtmlTableCell tc_sub_total_premium = new HtmlTableCell();
                            tc_sub_total_premium.Style.Add("text-align", "right");
                            tc_sub_total_premium.Style.Add("color", "green");
                            tc_sub_total_premium.Style.Add("padding-right", "6px");
                            if (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium  + add_member_detail_accidental_100plus_premium> 0)
                            {
                                tc_sub_total_premium.InnerText = (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium + add_member_detail_accidental_100plus_premium).ToString("C2");
                            }
                            else
                            {
                                tc_sub_total_premium.InnerText = " - ";
                            }

                            if (employee.Certificate_Number != 999999)
                            {
                                tc_certificate_number.InnerText = str_certificate_no;
                                tc_plan_name.InnerText = plan_name;
                                tc_effective_date.InnerText = my_premium.Effective_Date.ToString("d-MMM-yyyy");
                                tc_expiry_date.InnerText = my_premium.Expiry_Date.ToString("d-MMM-yyyy");
                                tc_covery_period.InnerText = coverage_period.ToString();

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

                            //Add row add member detail
                            //Row
                            HtmlTableRow tr_black2 = new HtmlTableRow();
                            tr_black2.Style.Add("color", "Black");
                            tr_black2.Style.Add("font-size", "8pt");

                            //Number
                            HtmlTableCell tc_number2 = new HtmlTableCell();
                            tc_number2.Style.Add("text-align", "center");
                            tc_number2.Style.Add("color", "black");
                            tc_number2.InnerText = (add_member_number).ToString();

                            //Certificate Number
                            HtmlTableCell tc_certificate_number2 = new HtmlTableCell();
                            tc_certificate_number2.Style.Add("color", "black");
                            tc_certificate_number2.Style.Add("padding-left", "5px");


                            //Employee Name
                            HtmlTableCell tc_employee_name2 = new HtmlTableCell();
                            tc_employee_name2.Style.Add("text-align", "left");
                            tc_employee_name2.Style.Add("padding-left", "5px");
                            tc_employee_name2.Style.Add("color", "black");
                            tc_employee_name2.InnerText = employee.Employee_Name;

                            //Plan Name
                            HtmlTableCell tc_plan_name2 = new HtmlTableCell();
                            tc_plan_name2.Style.Add("text-align", "center");
                            tc_plan_name2.Style.Add("color", "black");


                            //Effective Date
                            HtmlTableCell tc_effective_date2 = new HtmlTableCell();
                            tc_effective_date2.Style.Add("text-align", "center");
                            tc_effective_date2.Style.Add("color", "black");


                            //Expiry Date
                            HtmlTableCell tc_expiry_date2 = new HtmlTableCell();
                            tc_expiry_date2.Style.Add("text-align", "center");
                            tc_expiry_date2.Style.Add("color", "black");


                            //Cover Period
                            HtmlTableCell tc_covery_period2 = new HtmlTableCell();
                            tc_covery_period2.Style.Add("text-align", "center");
                            tc_covery_period2.Style.Add("color", "black");


                            //Sum Insured
                            HtmlTableCell tc_sum_insured2 = new HtmlTableCell();
                            tc_sum_insured2.Style.Add("text-align", "right");
                            tc_sum_insured2.Style.Add("color", "black");
                            tc_sum_insured2.Style.Add("padding-right", "6px");

                            if (add_member_detail_sum_insured != 0)
                            {
                                tc_sum_insured2.InnerText = add_member_detail_sum_insured.ToString("C0");// my_premium.Sum_Insured.ToString("C0");

                            }
                            else
                            {
                                tc_sum_insured2.InnerText = " - ";

                            }

                            //Life Premium
                            HtmlTableCell tc_life_premium2 = new HtmlTableCell();
                            tc_life_premium2.Style.Add("text-align", "right");
                            tc_life_premium2.Style.Add("color", "black");
                            tc_life_premium2.Style.Add("padding-right", "6px");
                            if (add_member_detail_life_premium != 0)
                            {
                                tc_life_premium2.InnerText = add_member_detail_life_premium.ToString("C2");
                            }
                            else
                            {
                                tc_life_premium2.InnerText = " - ";
                            }

                            //Accidental 100Plus Premium
                            HtmlTableCell tc_accidental_100plus_premium2 = new HtmlTableCell();
                            tc_accidental_100plus_premium2.Style.Add("text-align", "right");
                            tc_accidental_100plus_premium2.Style.Add("color", "black");
                            tc_accidental_100plus_premium2.Style.Add("padding-right", "6px");
                            if (add_member_detail_accidental_100plus_premium != 0)
                            {
                                tc_accidental_100plus_premium2.InnerText = add_member_detail_accidental_100plus_premium.ToString("C2");
                            }
                            else
                            {
                                tc_accidental_100plus_premium2.InnerText = " - ";
                            }

                            //TPD Premium
                            HtmlTableCell tc_tpd_premium2 = new HtmlTableCell();
                            tc_tpd_premium2.Style.Add("text-align", "right");
                            tc_tpd_premium2.Style.Add("color", "black");
                            tc_tpd_premium2.Style.Add("padding-right", "6px");
                            if (add_member_detail_tpd_premium != 0)
                            {
                                tc_tpd_premium2.InnerText = add_member_detail_tpd_premium.ToString("C2");
                            }
                            else
                            {
                                tc_tpd_premium2.InnerText = " - ";
                            }

                            //DHC Premium
                            HtmlTableCell tc_dhc_premium2 = new HtmlTableCell();
                            tc_dhc_premium2.Style.Add("text-align", "right");
                            tc_dhc_premium2.Style.Add("color", "black");
                            tc_dhc_premium2.Style.Add("padding-right", "6px");
                            if (add_member_detail_dhc_premium != 0)
                            {
                                tc_dhc_premium2.InnerText = add_member_detail_dhc_premium.ToString("C2");
                            }
                            else
                            {
                                tc_dhc_premium2.InnerText = " - ";
                            }

                            //Sub Total Premium
                            HtmlTableCell tc_sub_total_premium2 = new HtmlTableCell();
                            tc_sub_total_premium2.Style.Add("text-align", "right");
                            tc_sub_total_premium2.Style.Add("color", "black");
                            tc_sub_total_premium2.Style.Add("padding-right", "6px");
                            if (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium + add_member_detail_accidental_100plus_premium> 0)
                            {
                                tc_sub_total_premium2.InnerText = (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium + add_member_detail_accidental_100plus_premium).ToString("C2");
                            }
                            else
                            {
                                tc_sub_total_premium2.InnerText = " - ";
                            }

                            if (employee.Certificate_Number != 999999)
                            {
                                tc_certificate_number2.InnerText = str_certificate_no;
                                tc_plan_name2.InnerText = plan_name;
                                tc_effective_date2.InnerText = my_premium.Effective_Date.ToString("d-MMM-yyyy");
                                tc_expiry_date2.InnerText = my_premium.Expiry_Date.ToString("d-MMM-yyyy");
                                tc_covery_period2.InnerText = coverage_period.ToString();

                            }
                            else
                            {
                                tc_certificate_number2.InnerText = " - ";
                                tc_plan_name2.InnerText = " - ";
                                tc_effective_date2.InnerText = " - ";
                                tc_expiry_date2.InnerText = " - ";
                                tc_covery_period2.InnerText = " - ";

                            }

                            tr_black2.Controls.Add(tc_number2);
                            tr_black2.Controls.Add(tc_certificate_number2);
                            tr_black2.Controls.Add(tc_employee_name2);
                            tr_black2.Controls.Add(tc_plan_name2);
                            tr_black2.Controls.Add(tc_effective_date2);
                            tr_black2.Controls.Add(tc_expiry_date2);
                            tr_black2.Controls.Add(tc_covery_period2);
                            tr_black2.Controls.Add(tc_sum_insured2);
                            tr_black2.Controls.Add(tc_life_premium2);
                            tr_black2.Controls.Add(tc_accidental_100plus_premium2);
                            tr_black2.Controls.Add(tc_tpd_premium2);
                            tr_black2.Controls.Add(tc_dhc_premium2);
                            tr_black2.Controls.Add(tc_sub_total_premium2);
                            tblAddMemberDetail.Controls.Add(tr_black2);

                            //print premium detail green row
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
                            strTableDetailPrint += "<td style='padding-left: 5px; color: green; mso-number-format:\"" + @"\@" + "\"" + ";'>" + str_certificate_no + "</td>";
                            strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: green'>" + employee.Employee_Name + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + plan_name + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + my_premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + my_premium.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: green'>" + coverage_period.ToString() + "</td>";
                            //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + my_premium.Sum_Insured.ToString("C0") + "</td>";
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + add_member_detail_sum_insured.ToString("C0") + "</td>";

                            if (add_member_detail_life_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + add_member_detail_life_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }

                            if (add_member_detail_accidental_100plus_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + add_member_detail_accidental_100plus_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }

                            if (add_member_detail_tpd_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + add_member_detail_tpd_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }


                            if (add_member_detail_dhc_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + add_member_detail_dhc_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }


                            if (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium + add_member_detail_accidental_100plus_premium > 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium + add_member_detail_accidental_100plus_premium).ToString("C2") + "</td>";

                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'> - </td>";
                            }

                            strTableDetailPrint += "</tr>";

                            dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                            strTableDetailPrint = "";

                            //print add member detail row
                            add_member_rows += 1;

                            if (add_member_rows == 49)
                            {
                                strTableAddMemberDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                                dvPrintAddMemberDetail.Controls.Add(new LiteralControl(strTableAddMemberDetailPrint));

                                add_member_rows = 0;

                                //New table
                                strTableAddMemberDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                                strTableAddMemberDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                            }

                            strTableAddMemberDetailPrint += "<tr>";
                            strTableAddMemberDetailPrint += "<td style='text-align: center; color: black' width='30px'>" + (add_member_number).ToString() + "</td>";
                            strTableAddMemberDetailPrint += "<td style='padding-left: 5px; color: black; mso-number-format:\"" + @"\@" + "\"" + ";'>" + str_certificate_no + "</td>";
                            strTableAddMemberDetailPrint += "<td style='text-align: left; padding-left: 5px; color: black'>" + employee.Employee_Name + "</td>";
                            strTableAddMemberDetailPrint += "<td style='text-align: center; color: black'>" + plan_name + "</td>";
                            strTableAddMemberDetailPrint += "<td style='text-align: center; color: black'>" + my_premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableAddMemberDetailPrint += "<td style='text-align: center; color: black'>" + my_premium.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableAddMemberDetailPrint += "<td style='text-align: center; color: black'>" + coverage_period.ToString() + "</td>";
                            //strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + my_premium.Sum_Insured.ToString("C0") + "</td>";
                            strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" +add_member_detail_sum_insured.ToString("C0") + "</td>";

                            if (add_member_detail_life_premium != 0)
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + add_member_detail_life_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            if (add_member_detail_accidental_100plus_premium != 0)
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + add_member_detail_accidental_100plus_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            if (add_member_detail_tpd_premium != 0)
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + add_member_detail_tpd_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }


                            if (add_member_detail_dhc_premium != 0)
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + add_member_detail_dhc_premium.ToString("C2") + "</td>";
                            }
                            else
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }


                            if (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium + add_member_detail_accidental_100plus_premium > 0)
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + (add_member_detail_life_premium + add_member_detail_tpd_premium + add_member_detail_dhc_premium + add_member_detail_accidental_100plus_premium).ToString("C2") + "</td>";

                            }
                            else
                            {
                                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            strTableAddMemberDetailPrint += "</tr>";

                            dvPrintAddMemberDetail.Controls.Add(new LiteralControl(strTableAddMemberDetailPrint));

                            strTableAddMemberDetailPrint = "";
                        }
                    #endregion
                }
                else if (my_premium.Transaction_Type == 3)//resigned
                {
                    #region
                        //Get payment code
                        if (payment_code == "")
                        {
                            payment_code = da_gtli_policy_prem_return.GetPaymentCode(my_premium.GTLI_Premium_ID);
                        }

                        dvResignMemberDetail.Visible = true;

                        total_accidental_100plus_premium -= my_premium.Accidental_100Plus_Premium;
                        total_life_premium -= my_premium.Life_Premium;
                        total_tpd_premium -= my_premium.TPD_Premium;
                        total_dhc_premium -= my_premium.DHC_Premium;
                                          
                        total_original_life_premium -= my_premium.Original_Life_Premium;
                        total_original_tpd_premium -= my_premium.Original_TPD_Premium;
                        total_original_dhc_premium -= my_premium.Original_DHC_Premium;
                        total_sum_insured -= my_premium.Sum_Insured;
                        total_original_accidental_100plus_premium -= my_premium.Original_Accidental_100Plus_Premium;
                        total_resign_member_sum_insured += my_premium.Sum_Insured;

                        //employee premium for this policy transaction
                        ArrayList this_policy_employee = da_gtli_employee.GetListOfEmployeeByGTLIPremiumID(my_premium.GTLI_Premium_ID);

                        TimeSpan mytimespan = my_premium.Expiry_Date.Subtract(my_premium.Effective_Date);
                        int days_not_cover = mytimespan.Days + 1;                  

                        //loop through employee list
                        for (int k = 0; k <= this_policy_employee.Count - 1; k++)
                        {
                            bl_gtli_employee employee = (bl_gtli_employee)this_policy_employee[k];
                            double resign_member_detail_life_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "Death");
                            double resign_member_detail_tpd_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "TPD");
                            double resign_member_detail_dhc_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "DHC");
                            double resign_member_detail_accidental_100plus_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, my_premium.GTLI_Premium_ID, "Accidental100Plus");
                            double resign_member_detail_sum_insured = 0;

                            //get original policy premium for this employee                               
                            bl_gtli_premium original_premium = da_gtli_premium.GetGTLPremiumByCerficateID(employee.GTLI_Certificate_ID);

                            bl_gtli_plan plan = da_gtli_plan.GetPlan(original_premium.GTLI_Plan_ID);

                            //add to total
                            total_resign_member_dhc_premium += resign_member_detail_dhc_premium;
                            total_resign_member_life_premium += resign_member_detail_life_premium;
                            total_resign_member_tpd_premium += resign_member_detail_tpd_premium;
                            total_resign_member_accidental_100plus_premium += resign_member_detail_accidental_100plus_premium;

                            double my_sum_insured = 0; //sum_insured for display

                            //get employee premuim by certificate by maneth
                            double newPremium = 0;
                            newPremium = da_gtli_premium.getEmployeeSumInsured(employee.GTLI_Certificate_ID);
                           
                            //---------

                            if (resign_member_detail_life_premium == 0 && resign_member_detail_tpd_premium == 0 && resign_member_detail_dhc_premium == 0 && resign_member_detail_accidental_100plus_premium == 0)
                            {
                               
                                resign_member_detail_sum_insured = 0;
                            }
                            else
                            {
                                //my_sum_insured = original_premium.Sum_Insured;
                                //resign_member_detail_sum_insured = original_premium.Sum_Insured;
                                my_sum_insured = newPremium;
                                resign_member_detail_sum_insured = newPremium;
                            }

                            resign_member_number += 1;
                            number += 1;

                            string str_certificate_no = employee.Certificate_Number.ToString();

                            while (str_certificate_no.Length < 6)
                            {
                                str_certificate_no = "0" + str_certificate_no;
                            }

                            //Add row to premium detail
                            //Row
                            HtmlTableRow tr_black = new HtmlTableRow();
                            tr_black.Style.Add("color", "red");
                            tr_black.Style.Add("font-size", "8pt");

                            //Number
                            HtmlTableCell tc_number = new HtmlTableCell();
                            tc_number.Style.Add("text-align", "center");
                            tc_number.Style.Add("color", "red");
                            tc_number.InnerText = (number).ToString();

                            //Certificate Number
                            HtmlTableCell tc_certificate_number = new HtmlTableCell();
                            tc_certificate_number.Style.Add("color", "red");
                            tc_certificate_number.Style.Add("padding-left", "5px");

                            //Employee Name
                            HtmlTableCell tc_employee_name = new HtmlTableCell();
                            tc_employee_name.Style.Add("text-align", "left");
                            tc_employee_name.Style.Add("padding-left", "5px");
                            tc_employee_name.Style.Add("color", "red");
                            tc_employee_name.InnerText = employee.Employee_Name;

                            //Plan Name
                            HtmlTableCell tc_plan_name = new HtmlTableCell();
                            tc_plan_name.Style.Add("text-align", "center");
                            tc_plan_name.Style.Add("color", "red");

                            //Resigned Date
                            HtmlTableCell tc_effective_date = new HtmlTableCell();
                            tc_effective_date.Style.Add("text-align", "center");
                            tc_effective_date.Style.Add("color", "red");


                            //Expiry Date
                            HtmlTableCell tc_expiry_date = new HtmlTableCell();
                            tc_expiry_date.Style.Add("text-align", "center");
                            tc_expiry_date.Style.Add("color", "red");


                            //Days not cover
                            HtmlTableCell tc_covery_period = new HtmlTableCell();
                            tc_covery_period.Style.Add("text-align", "center");
                            tc_covery_period.Style.Add("color", "red");


                            //Sum Insured
                            HtmlTableCell tc_sum_insured = new HtmlTableCell();
                            tc_sum_insured.Style.Add("text-align", "right");
                            tc_sum_insured.Style.Add("color", "red");
                            tc_sum_insured.Style.Add("padding-right", "6px");

                            if (my_sum_insured != 0)
                            {
                                tc_sum_insured.InnerText = "(" + my_sum_insured.ToString("C0") + ")";
                            }
                            else
                            {
                                tc_sum_insured.InnerText = " - ";
                            }

                            //Life Premium
                            HtmlTableCell tc_life_premium = new HtmlTableCell();
                            tc_life_premium.Style.Add("text-align", "right");
                            tc_life_premium.Style.Add("color", "red");
                            tc_life_premium.Style.Add("padding-right", "6px");
                            if (resign_member_detail_life_premium != 0)
                            {
                                tc_life_premium.InnerText = "(" + resign_member_detail_life_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_life_premium.InnerText = " - ";
                            }

                            //Accidental 100Plus Premium
                            HtmlTableCell tc_accidental_100plus_premium = new HtmlTableCell();
                            tc_accidental_100plus_premium.Style.Add("text-align", "right");
                            tc_accidental_100plus_premium.Style.Add("color", "red");
                            tc_accidental_100plus_premium.Style.Add("padding-right", "6px");
                            if (resign_member_detail_accidental_100plus_premium != 0)
                            {
                                tc_accidental_100plus_premium.InnerText = "(" + resign_member_detail_accidental_100plus_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_accidental_100plus_premium.InnerText = " - ";
                            }

                            //TPD Return Premium
                            HtmlTableCell tc_tpd_premium = new HtmlTableCell();
                            tc_tpd_premium.Style.Add("text-align", "right");
                            tc_tpd_premium.Style.Add("color", "red");
                            tc_tpd_premium.Style.Add("padding-right", "6px");
                            if (resign_member_detail_tpd_premium != 0)
                            {
                                tc_tpd_premium.InnerText = "(" + resign_member_detail_tpd_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_tpd_premium.InnerText = " - ";
                            }

                            //DHC Return Premium
                            HtmlTableCell tc_dhc_premium = new HtmlTableCell();
                            tc_dhc_premium.Style.Add("text-align", "right");
                            tc_dhc_premium.Style.Add("color", "red");
                            tc_dhc_premium.Style.Add("padding-right", "6px");
                            if (resign_member_detail_dhc_premium != 0)
                            {
                                tc_dhc_premium.InnerText = "(" + resign_member_detail_dhc_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_dhc_premium.InnerText = " - ";
                            }

                            //Sub Total Return Premium
                            HtmlTableCell tc_sub_total_premium = new HtmlTableCell();
                            tc_sub_total_premium.Style.Add("text-align", "right");
                            tc_sub_total_premium.Style.Add("color", "red");
                            tc_sub_total_premium.Style.Add("padding-right", "6px");
                            if (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium > 0)
                            {
                                tc_sub_total_premium.InnerText = "(" + (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium).ToString("C2") + ")";
                            }
                            else
                            {
                                tc_sub_total_premium.InnerText = " - ";
                            }

                            if (employee.Certificate_Number != 999999)
                            {
                                tc_certificate_number.InnerText = str_certificate_no;
                                tc_plan_name.InnerText = plan.GTLI_Plan;
                                tc_effective_date.InnerText = my_premium.Effective_Date.ToString("d-MMM-yyyy");
                                tc_expiry_date.InnerText = my_premium.Expiry_Date.ToString("d-MMM-yyyy");
                                tc_covery_period.InnerText = days_not_cover.ToString();

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

                            //Add row to resign member detail
                            //Row

                            HtmlTableRow tr_black2 = new HtmlTableRow();
                            tr_black2.Style.Add("color", "black");
                            tr_black2.Style.Add("font-size", "8pt");

                            //Number
                            HtmlTableCell tc_number2 = new HtmlTableCell();
                            tc_number2.Style.Add("text-align", "center");
                            tc_number2.Style.Add("color", "black");
                            tc_number2.InnerText = (resign_member_number).ToString();

                            //Certificate Number
                            HtmlTableCell tc_certificate_number2 = new HtmlTableCell();
                            tc_certificate_number2.Style.Add("color", "black");
                            tc_certificate_number2.Style.Add("padding-left", "5px");

                            //Employee Name
                            HtmlTableCell tc_employee_name2 = new HtmlTableCell();
                            tc_employee_name2.Style.Add("text-align", "left");
                            tc_employee_name2.Style.Add("padding-left", "5px");
                            tc_employee_name2.Style.Add("color", "black");
                            tc_employee_name2.InnerText = employee.Employee_Name;

                            //Plan Name
                            HtmlTableCell tc_plan_name2 = new HtmlTableCell();
                            tc_plan_name2.Style.Add("text-align", "center");
                            tc_plan_name2.Style.Add("color", "black");

                            //Resigned Date
                            HtmlTableCell tc_effective_date2 = new HtmlTableCell();
                            tc_effective_date2.Style.Add("text-align", "center");
                            tc_effective_date2.Style.Add("color", "black");


                            //Expiry Date
                            HtmlTableCell tc_expiry_date2 = new HtmlTableCell();
                            tc_expiry_date2.Style.Add("text-align", "center");
                            tc_expiry_date2.Style.Add("color", "black");


                            //Days not cover
                            HtmlTableCell tc_covery_period2 = new HtmlTableCell();
                            tc_covery_period2.Style.Add("text-align", "center");
                            tc_covery_period2.Style.Add("color", "black");


                            //Sum Insured
                            HtmlTableCell tc_sum_insured2 = new HtmlTableCell();
                            tc_sum_insured2.Style.Add("text-align", "right");
                            tc_sum_insured2.Style.Add("color", "black");
                            tc_sum_insured2.Style.Add("padding-right", "6px");

                            if (resign_member_detail_sum_insured != 0)
                            {
                                tc_sum_insured2.InnerText = "(" + resign_member_detail_sum_insured.ToString("C0") + ")";
                            }
                            else
                            {
                                tc_sum_insured2.InnerText = " - ";
                            }

                            //Life Premium
                            HtmlTableCell tc_life_premium2 = new HtmlTableCell();
                            tc_life_premium2.Style.Add("text-align", "right");
                            tc_life_premium2.Style.Add("color", "black");
                            tc_life_premium2.Style.Add("padding-right", "6px");
                            if (resign_member_detail_life_premium != 0)
                            {
                                tc_life_premium2.InnerText = "(" + resign_member_detail_life_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_life_premium2.InnerText = " - ";
                            }

                            //Accidental 100Plus Premium
                            HtmlTableCell tc_accidental_100plus_premium2 = new HtmlTableCell();
                            tc_accidental_100plus_premium2.Style.Add("text-align", "right");
                            tc_accidental_100plus_premium2.Style.Add("color", "black");
                            tc_accidental_100plus_premium2.Style.Add("padding-right", "6px");
                            if (resign_member_detail_accidental_100plus_premium != 0)
                            {
                                tc_accidental_100plus_premium2.InnerText = "(" + resign_member_detail_accidental_100plus_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_accidental_100plus_premium2.InnerText = " - ";
                            }

                            //TPD Return Premium
                            HtmlTableCell tc_tpd_premium2 = new HtmlTableCell();
                            tc_tpd_premium2.Style.Add("text-align", "right");
                            tc_tpd_premium2.Style.Add("color", "black");
                            tc_tpd_premium2.Style.Add("padding-right", "6px");
                            if (resign_member_detail_tpd_premium != 0)
                            {
                                tc_tpd_premium2.InnerText = "(" + resign_member_detail_tpd_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_tpd_premium2.InnerText = " - ";
                            }

                            //DHC Return Premium
                            HtmlTableCell tc_dhc_premium2 = new HtmlTableCell();
                            tc_dhc_premium2.Style.Add("text-align", "right");
                            tc_dhc_premium2.Style.Add("color", "black");
                            tc_dhc_premium2.Style.Add("padding-right", "6px");
                            if (resign_member_detail_dhc_premium != 0)
                            {
                                tc_dhc_premium2.InnerText = "(" + resign_member_detail_dhc_premium.ToString("C2") + ")";
                            }
                            else
                            {
                                tc_dhc_premium2.InnerText = " - ";
                            }

                            //Sub Total Return Premium
                            HtmlTableCell tc_sub_total_premium2 = new HtmlTableCell();
                            tc_sub_total_premium2.Style.Add("text-align", "right");
                            tc_sub_total_premium2.Style.Add("color", "black");
                            tc_sub_total_premium2.Style.Add("padding-right", "6px");
                            if (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium > 0)
                            {
                                tc_sub_total_premium2.InnerText = "(" + (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium).ToString("C2") + ")";
                            }
                            else
                            {
                                tc_sub_total_premium2.InnerText = " - ";
                            }

                            if (employee.Certificate_Number != 999999)
                            {
                                tc_certificate_number2.InnerText = str_certificate_no;
                                tc_plan_name2.InnerText = plan.GTLI_Plan;
                                tc_effective_date2.InnerText = my_premium.Effective_Date.ToString("d-MMM-yyyy");
                                tc_expiry_date2.InnerText = my_premium.Expiry_Date.ToString("d-MMM-yyyy");
                                tc_covery_period2.InnerText = days_not_cover.ToString();

                            }
                            else
                            {
                                tc_certificate_number2.InnerText = " - ";
                                tc_plan_name2.InnerText = " - ";
                                tc_effective_date2.InnerText = " - ";
                                tc_expiry_date2.InnerText = " - ";
                                tc_covery_period2.InnerText = " - ";

                            }

                            tr_black2.Controls.Add(tc_number2);
                            tr_black2.Controls.Add(tc_certificate_number2);
                            tr_black2.Controls.Add(tc_employee_name2);
                            tr_black2.Controls.Add(tc_plan_name2);
                            tr_black2.Controls.Add(tc_effective_date2);
                            tr_black2.Controls.Add(tc_expiry_date2);
                            tr_black2.Controls.Add(tc_covery_period2);
                            tr_black2.Controls.Add(tc_sum_insured2);
                            tr_black2.Controls.Add(tc_life_premium2);
                            tr_black2.Controls.Add(tc_accidental_100plus_premium2);
                            tr_black2.Controls.Add(tc_tpd_premium2);
                            tr_black2.Controls.Add(tc_dhc_premium2);
                            tr_black2.Controls.Add(tc_sub_total_premium2);
                            tblResignMemberDetail.Controls.Add(tr_black2);

                            //print premium detail red row
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
                            strTableDetailPrint += "<td style='text-align: center; color: red' width='30px'>" + (number).ToString() + "</td>";
                            strTableDetailPrint += "<td style='padding-left: 5px; color: red'; mso-number-format:\"" + @"\@" + "\"" + ";>" + str_certificate_no + "</td>";
                            strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: red'>" + employee.Employee_Name + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + plan.GTLI_Plan + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + my_premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + my_premium.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableDetailPrint += "<td style='text-align: center; color: red'>" + days_not_cover.ToString() + "</td>";
                            //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + my_premium.Sum_Insured.ToString("C0") + "</td>";
                            strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + resign_member_detail_sum_insured.ToString("C0") + "</td>";

                            if (resign_member_detail_life_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>(" + resign_member_detail_life_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                            }

                            if (resign_member_detail_accidental_100plus_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>(" + resign_member_detail_accidental_100plus_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                            }

                            if (resign_member_detail_tpd_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>(" + resign_member_detail_tpd_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                            }


                            if (resign_member_detail_dhc_premium != 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>(" + resign_member_detail_dhc_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                            }


                            if (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium > 0)
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>(" + (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium).ToString("C2") + ")</td>";

                            }
                            else
                            {
                                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'> - </td>";
                            }

                            strTableDetailPrint += "</tr>";

                            dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                            strTableDetailPrint = "";

                            //print resign member detail row
                            resign_member_rows += 1;

                            if (resign_member_rows == 49)
                            {
                                strTableResignMemberDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                                dvPrintResignMemberDetail.Controls.Add(new LiteralControl(strTableResignMemberDetailPrint));

                                resign_member_rows = 0;

                                //New table
                                strTableResignMemberDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                                strTableResignMemberDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Days not cover</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                            }

                            strTableResignMemberDetailPrint += "<tr>";
                            strTableResignMemberDetailPrint += "<td style='text-align: center; color: black' width='30px'>" + (resign_member_number).ToString() + "</td>";
                            strTableResignMemberDetailPrint += "<td style='padding-left: 5px; color: black; mso-number-format:\"" + @"\@" + "\"" + ";'>" + str_certificate_no + "</td>";
                            strTableResignMemberDetailPrint += "<td style='text-align: left; padding-left: 5px; color: black'>" + employee.Employee_Name + "</td>";
                            strTableResignMemberDetailPrint += "<td style='text-align: center; color: black'>" + plan.GTLI_Plan + "</td>";
                            strTableResignMemberDetailPrint += "<td style='text-align: center; color: black'>" + my_premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableResignMemberDetailPrint += "<td style='text-align: center; color: black'>" + my_premium.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                            strTableResignMemberDetailPrint += "<td style='text-align: center; color: black'>" + days_not_cover.ToString() + "</td>";
                            strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>(" + my_premium.Sum_Insured.ToString("C0") + ")</td>";

                            if (resign_member_detail_life_premium != 0)
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>(" + resign_member_detail_life_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            if (resign_member_detail_accidental_100plus_premium != 0)
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>(" + resign_member_detail_accidental_100plus_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            if (resign_member_detail_tpd_premium != 0)
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>(" + resign_member_detail_tpd_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }


                            if (resign_member_detail_dhc_premium != 0)
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>(" + resign_member_detail_dhc_premium.ToString("C2") + ")</td>";
                            }
                            else
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }


                            if (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium > 0)
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>(" + (resign_member_detail_life_premium + resign_member_detail_tpd_premium + resign_member_detail_dhc_premium + resign_member_detail_accidental_100plus_premium).ToString("C2") + ")</td>";

                            }
                            else
                            {
                                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'> - </td>";
                            }

                            strTableResignMemberDetailPrint += "</tr>";

                            dvPrintResignMemberDetail.Controls.Add(new LiteralControl(strTableResignMemberDetailPrint));

                            strTableResignMemberDetailPrint = "";
                        }
                    #endregion
                }

            }//End Loop

                        
            //Summary data
                       
            lblPolicyNumber.Text = policy.Policy_Number;
            lblCompanyName.Text = company.Company_Name;
            lblTypeOfBusiness.Text = company.Type_Of_Business;
            lblContactName.Text = contact.Contact_Name;
            lblPhone.Text = contact.Contact_Phone;
            lblEmail.Text = contact.Contact_Email;
            lblAddress.Text = company.Company_Address;
            lblSumInsured.Text = total_sum_insured.ToString("C0");

            lblExpiryDate.Text = policy.Expiry_Date.ToString("d-MMM-yyyy");
            lblEffectiveDate.Text = policy.Effective_Date.ToString("d-MMM-yyyy");
            lblCreatedDate.Text = DateTime.Today.ToString("d-MMM-yyyy");
            lblPremiumAfterDiscount.Text = (total_life_premium + total_accidental_100plus_premium + total_tpd_premium + total_dhc_premium).ToString("C2");
            lblPremiumDiscount.Text = (total_life_premium_discount + total_accidental_100plus_premium_discount + total_tpd_premium_discount + total_dhc_premium_discount).ToString("C2");

            //Print Summary
            lblPaymentCode2.Text = payment_code;
            lblPolicyNumber2.Text = policy.Policy_Number;
            lblCompanyName2.Text = company.Company_Name;
            lblTypeOfBusiness2.Text = company.Type_Of_Business;
            lblContactName2.Text = contact.Contact_Name;
            lblPhone2.Text = contact.Contact_Phone;
            lblEmail2.Text = contact.Contact_Email;
            lblAddress2.Text = company.Company_Address;
            lblSumInsured2.Text = total_sum_insured.ToString("C0");

            lblExpiryDate2.Text = policy.Expiry_Date.ToString("d-MMM-yyyy");
            lblEffectiveDate2.Text = policy.Effective_Date.ToString("d-MMM-yyyy");
            lblCreatedDate2.Text = DateTime.Today.ToString("d-MMM-yyyy");
            lblPremiumAfterDiscount2.Text = (total_life_premium + total_accidental_100plus_premium + total_tpd_premium + total_dhc_premium).ToString("C2");
            lblPremiumDiscount2.Text = (total_life_premium_discount + total_accidental_100plus_premium_discount + total_tpd_premium_discount + total_dhc_premium_discount).ToString("C2");

            if ((total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium) < 0)
            {
                lblPremiumPayment.Text = "(" + (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2") + ")";
                lblPremiumPayment2.Text = "(" + (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2") + ")";
            } 
            else
            {
                lblPremiumPayment.Text = (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2");
                lblPremiumPayment2.Text = (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2");
            }

            //Total Premium Detail
            #region
                //Total Row Premium Detail            
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

                if (total_sum_insured < 0)
                {
                    tc_sum_insured_total.InnerText = "(" + total_sum_insured.ToString("C0") + ")";
                }
                else
                {
                    tc_sum_insured_total.InnerText = total_sum_insured.ToString("C0");
                }

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
                else if (total_life_premium == 0)
                {
                    tc_life_premium_total.InnerText = " - ";
                }
                else
                {
                    tc_life_premium_total.Style.Add("text-decoration", "underline");
                    tc_life_premium_total.InnerText = "(" + total_life_premium.ToString("C2") + ")";
                }

                //Total Accidental 100Plus Premium
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
                else if (total_accidental_100plus_premium == 0)
                {
                    tc_accidental_100plus_premium_total.InnerText = " - ";
                }
                else
                {
                    tc_accidental_100plus_premium_total.Style.Add("text-decoration", "underline");
                    tc_accidental_100plus_premium_total.InnerText = "(" + total_accidental_100plus_premium.ToString("C2") + ")";
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
                else if (total_tpd_premium == 0)
                {
                    tc_tpd_premium_total.InnerText = " - ";
                }
                else
                {
                    tc_tpd_premium_total.Style.Add("text-decoration", "underline");
                    tc_tpd_premium_total.InnerText = "(" + total_tpd_premium.ToString("C2") + ")";
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
                else if (total_dhc_premium == 0)
                {
                    tc_dhc_premium_total.InnerText = " - ";
                }
                else
                {
                    tc_dhc_premium_total.Style.Add("text-decoration", "underline");
                    tc_dhc_premium_total.InnerText = "(" + total_dhc_premium.ToString("C2") + ")";
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
                else if (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium == 0)
                {
                    tc_total_premium.InnerText = " - ";
                }
                else
                {
                    tc_total_premium.Style.Add("text-decoration", "underline");
                    tc_total_premium.InnerText = "(" + (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2") + ")";
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

                if (total_sum_insured < 0)
                {

                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + total_sum_insured.ToString("C0") + ")</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_sum_insured.ToString("C0") + "</td>";
                }

                if (total_life_premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_life_premium.ToString("C2") + "</td>";
                }
                else if (total_life_premium == 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + total_life_premium.ToString("C2") + ")</td>";
                }

                if (total_accidental_100plus_premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_accidental_100plus_premium.ToString("C2") + "</td>";
                }
                else if (total_accidental_100plus_premium == 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + total_accidental_100plus_premium.ToString("C2") + ")</td>";
                }

                if (total_tpd_premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_tpd_premium.ToString("C2") + "</td>";
                }
                else if (total_tpd_premium == 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + total_tpd_premium.ToString("C2") + ")</td>";
                }


                if (total_dhc_premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_dhc_premium.ToString("C2") + "</td>";
                }
                else if (total_dhc_premium == 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + total_dhc_premium.ToString("C2") + ")</td>";
                }


                if (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2") + "</td>";

                }
                else if (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium == 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium).ToString("C2") + ")</td>";
                }

                strTableDetailPrint += "</table>";

                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));


            #endregion        

            //Total Add Member Detail
            #region
                //Total Row Add Member Detail              
                HtmlTableRow tr_total3 = new HtmlTableRow();
                tr_total3.Style.Add("color", "Black");
                tr_total3.Style.Add("font-size", "8pt");

                //Total Number
                HtmlTableCell tc_number_total3 = new HtmlTableCell();
                tc_number_total3.Style.Add("text-align", "center");
                tc_number_total3.Style.Add("color", "black");
                tc_number_total3.InnerText = "";

                //Total Certificate Number
                HtmlTableCell tc_certificate_number_total3 = new HtmlTableCell();
                tc_certificate_number_total3.Style.Add("color", "black");
                tc_certificate_number_total3.InnerText = "";

                //Total Employee Name 
                HtmlTableCell tc_employee_name_total3 = new HtmlTableCell();
                tc_employee_name_total3.Style.Add("text-align", "left");
                tc_employee_name_total3.Style.Add("padding-left", "5px");
                tc_employee_name_total3.Style.Add("color", "black");
                tc_employee_name_total3.InnerText = "";

                //Total Plan Name
                HtmlTableCell tc_plan_name_total3 = new HtmlTableCell();
                tc_plan_name_total3.Style.Add("text-align", "center");
                tc_plan_name_total3.Style.Add("color", "black");
                tc_plan_name_total3.InnerText = "";

                //Total Effective Date
                HtmlTableCell tc_effective_date_total3 = new HtmlTableCell();
                tc_effective_date_total3.Style.Add("text-align", "center");
                tc_effective_date_total3.Style.Add("color", "black");
                tc_effective_date_total3.InnerText = "";

                //Total Expiry Date
                HtmlTableCell tc_expiry_date_total3 = new HtmlTableCell();
                tc_expiry_date_total3.Style.Add("text-align", "center");
                tc_expiry_date_total3.Style.Add("color", "black");
                tc_expiry_date_total3.InnerText = "";

                //Total Cover Period
                HtmlTableCell tc_covery_period_total3 = new HtmlTableCell();
                tc_covery_period_total3.Style.Add("text-align", "center");
                tc_covery_period_total3.Style.Add("color", "black");
                tc_covery_period_total3.Style.Add("font-weight", "bold");
                tc_covery_period_total3.InnerText = "Total:";

                //Total Sum Insured
                HtmlTableCell tc_sum_insured_total3 = new HtmlTableCell();
                tc_sum_insured_total3.Style.Add("text-align", "right");
                tc_sum_insured_total3.Style.Add("color", "black");
                tc_sum_insured_total3.Style.Add("padding-right", "6px");
                tc_sum_insured_total3.Style.Add("font-weight", "bold");
                tc_sum_insured_total3.Style.Add("text-decoration", "underline");
                tc_sum_insured_total3.InnerText = total_add_member_sum_insured.ToString("C0");

                //Total Life Premium
                HtmlTableCell tc_life_premium_total3 = new HtmlTableCell();
                tc_life_premium_total3.Style.Add("text-align", "right");
                tc_life_premium_total3.Style.Add("color", "black");
                tc_life_premium_total3.Style.Add("padding-right", "6px");
                tc_life_premium_total3.Style.Add("font-weight", "bold");

                if (total_add_member_life_premium > 0)
                {
                    tc_life_premium_total3.Style.Add("text-decoration", "underline");
                    tc_life_premium_total3.InnerText = total_add_member_life_premium.ToString("C2");
                }
                else
                {
                    tc_life_premium_total3.InnerText = " - ";
                }

                //Total Accidental 100Plus Premium
                HtmlTableCell tc_accidental_100plus_premium_total3 = new HtmlTableCell();
                tc_accidental_100plus_premium_total3.Style.Add("text-align", "right");
                tc_accidental_100plus_premium_total3.Style.Add("color", "black");
                tc_accidental_100plus_premium_total3.Style.Add("padding-right", "6px");
                tc_accidental_100plus_premium_total3.Style.Add("font-weight", "bold");

                if (total_add_member_accidental_100plus_premium > 0)
                {
                    tc_accidental_100plus_premium_total3.Style.Add("text-decoration", "underline");
                    tc_accidental_100plus_premium_total3.InnerText = total_add_member_accidental_100plus_premium.ToString("C2");
                }
                else
                {
                    tc_accidental_100plus_premium_total3.InnerText = " - ";
                }

                //Total TPD Premium
                HtmlTableCell tc_tpd_premium_total3 = new HtmlTableCell();
                tc_tpd_premium_total3.Style.Add("text-align", "right");
                tc_tpd_premium_total3.Style.Add("color", "black");
                tc_tpd_premium_total3.Style.Add("padding-right", "6px");
                tc_tpd_premium_total3.Style.Add("font-weight", "bold");

                if (total_add_member_tpd_premium > 0)
                {
                    tc_tpd_premium_total3.Style.Add("text-decoration", "underline");
                    tc_tpd_premium_total3.InnerText = total_add_member_tpd_premium.ToString("C2");
                }
                else
                {
                    tc_tpd_premium_total3.InnerText = " - ";
                }

                //Total DHC Premium
                HtmlTableCell tc_dhc_premium_total3 = new HtmlTableCell();
                tc_dhc_premium_total3.Style.Add("text-align", "right");
                tc_dhc_premium_total3.Style.Add("color", "black");
                tc_dhc_premium_total3.Style.Add("padding-right", "6px");
                tc_dhc_premium_total3.Style.Add("font-weight", "bold");

                if (total_add_member_dhc_premium > 0)
                {
                    tc_dhc_premium_total3.Style.Add("text-decoration", "underline");
                    tc_dhc_premium_total3.InnerText = total_add_member_dhc_premium.ToString("C2");
                }
                else
                {
                    tc_dhc_premium_total3.InnerText = " - ";
                }

                //Total Premium
                HtmlTableCell tc_total_premium3 = new HtmlTableCell();
                tc_total_premium3.Style.Add("text-align", "right");
                tc_total_premium3.Style.Add("color", "black");
                tc_total_premium3.Style.Add("padding-right", "6px");
                tc_total_premium3.Style.Add("font-weight", "bold");

                if (total_add_member_life_premium + total_add_member_tpd_premium + total_add_member_dhc_premium + total_add_member_accidental_100plus_premium > 0)
                {
                    tc_total_premium3.Style.Add("text-decoration", "underline");
                    tc_total_premium3.InnerText = (total_add_member_life_premium + total_add_member_tpd_premium + total_add_member_dhc_premium + total_add_member_accidental_100plus_premium).ToString("C2");
                }
                else
                {
                    tc_total_premium3.InnerText = " - ";
                }

                tr_total3.Controls.Add(tc_number_total3);
                tr_total3.Controls.Add(tc_certificate_number_total3);
                tr_total3.Controls.Add(tc_employee_name_total3);
                tr_total3.Controls.Add(tc_plan_name_total3);
                tr_total3.Controls.Add(tc_effective_date_total3);
                tr_total3.Controls.Add(tc_expiry_date_total3);
                tr_total3.Controls.Add(tc_covery_period_total3);
                tr_total3.Controls.Add(tc_sum_insured_total3);
                tr_total3.Controls.Add(tc_life_premium_total3);
                tr_total3.Controls.Add(tc_accidental_100plus_premium_total3);
                tr_total3.Controls.Add(tc_tpd_premium_total3);
                tr_total3.Controls.Add(tc_dhc_premium_total3);
                tr_total3.Controls.Add(tc_total_premium3);
                tblAddMemberDetail.Controls.Add(tr_total3);

                //Total Print Add Member Detail Row       
                strTableAddMemberDetailPrint = "<tr>";
                strTableAddMemberDetailPrint += "<td></td>";
                strTableAddMemberDetailPrint += "<td></td>";
                strTableAddMemberDetailPrint += "<td></td>";
                strTableAddMemberDetailPrint += "<td></td>";
                strTableAddMemberDetailPrint += "<td></td>";
                strTableAddMemberDetailPrint += "<td></td>";
                strTableAddMemberDetailPrint += "<td style='text-align: center; font-weight: bold;'>Total:</td>";
                strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_add_member_sum_insured.ToString("C0") + "</td>";

                if (total_add_member_life_premium > 0)
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_add_member_life_premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }


                if (total_add_member_accidental_100plus_premium > 0)
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_add_member_accidental_100plus_premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }

                if (total_add_member_tpd_premium > 0)
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_add_member_tpd_premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }
            
                if (total_add_member_dhc_premium > 0)
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_add_member_dhc_premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }


                if (total_add_member_life_premium + total_add_member_tpd_premium + total_add_member_dhc_premium + total_add_member_accidental_100plus_premium > 0)
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + (total_add_member_life_premium + total_add_member_tpd_premium + total_add_member_dhc_premium + total_add_member_accidental_100plus_premium).ToString("C2") + "</td>";

                }
                else
                {
                    strTableAddMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }

                strTableAddMemberDetailPrint += "</table>";

                dvPrintAddMemberDetail.Controls.Add(new LiteralControl(strTableAddMemberDetailPrint));
           #endregion

            //Total Resign Member Detail
            #region
                //Total Row Resign Member Detail              
                HtmlTableRow tr_total4 = new HtmlTableRow();
                tr_total4.Style.Add("color", "black");
                tr_total4.Style.Add("font-size", "8pt");

                //Total Number
                HtmlTableCell tc_number_total4 = new HtmlTableCell();
                tc_number_total4.Style.Add("text-align", "center");
                tc_number_total4.Style.Add("color", "black");
                tc_number_total4.InnerText = "";

                //Total Certificate Number
                HtmlTableCell tc_certificate_number_total4 = new HtmlTableCell();
                tc_certificate_number_total4.Style.Add("color", "black");
                tc_certificate_number_total4.InnerText = "";

                //Total Employee Name 
                HtmlTableCell tc_employee_name_total4 = new HtmlTableCell();
                tc_employee_name_total4.Style.Add("text-align", "left");
                tc_employee_name_total4.Style.Add("padding-left", "5px");
                tc_employee_name_total4.Style.Add("color", "black");
                tc_employee_name_total4.InnerText = "";

                //Total Plan Name
                HtmlTableCell tc_plan_name_total4 = new HtmlTableCell();
                tc_plan_name_total4.Style.Add("text-align", "center");
                tc_plan_name_total4.Style.Add("color", "black");
                tc_plan_name_total4.InnerText = "";

                //Total Effective Date
                HtmlTableCell tc_effective_date_total4 = new HtmlTableCell();
                tc_effective_date_total4.Style.Add("text-align", "center");
                tc_effective_date_total4.Style.Add("color", "black");
                tc_effective_date_total4.InnerText = "";

                //Total Expiry Date
                HtmlTableCell tc_expiry_date_total4 = new HtmlTableCell();
                tc_expiry_date_total4.Style.Add("text-align", "center");
                tc_expiry_date_total4.Style.Add("color", "black");
                tc_expiry_date_total4.InnerText = "";

                //Total Cover Period
                HtmlTableCell tc_covery_period_total4 = new HtmlTableCell();
                tc_covery_period_total4.Style.Add("text-align", "center");
                tc_covery_period_total4.Style.Add("color", "black");
                tc_covery_period_total4.Style.Add("font-weight", "bold");
                tc_covery_period_total4.InnerText = "Total:";

                //Total Sum Insured
                HtmlTableCell tc_sum_insured_total4 = new HtmlTableCell();
                tc_sum_insured_total4.Style.Add("text-align", "right");
                tc_sum_insured_total4.Style.Add("color", "black");
                tc_sum_insured_total4.Style.Add("padding-right", "6px");
                tc_sum_insured_total4.Style.Add("font-weight", "bold");
                tc_sum_insured_total4.Style.Add("text-decoration", "underline");
                tc_sum_insured_total4.InnerText = "(" + total_resign_member_sum_insured.ToString("C0") + ")";

                //Total Life Premium
                HtmlTableCell tc_life_premium_total4 = new HtmlTableCell();
                tc_life_premium_total4.Style.Add("text-align", "right");
                tc_life_premium_total4.Style.Add("color", "black");
                tc_life_premium_total4.Style.Add("padding-right", "6px");
                tc_life_premium_total4.Style.Add("font-weight", "bold");

                if (total_resign_member_life_premium > 0)
                {
                    tc_life_premium_total4.Style.Add("text-decoration", "underline");
                    tc_life_premium_total4.InnerText = "(" + total_resign_member_life_premium.ToString("C2") + ")";
                }
                else
                {
                    tc_life_premium_total4.InnerText = " - ";
                }

                //Total Accidental 100Plus Premium
                HtmlTableCell tc_accidental_100plus_premium_total4 = new HtmlTableCell();
                tc_accidental_100plus_premium_total4.Style.Add("text-align", "right");
                tc_accidental_100plus_premium_total4.Style.Add("color", "black");
                tc_accidental_100plus_premium_total4.Style.Add("padding-right", "6px");
                tc_accidental_100plus_premium_total4.Style.Add("font-weight", "bold");

                if (total_resign_member_accidental_100plus_premium > 0)
                {
                    tc_accidental_100plus_premium_total4.Style.Add("text-decoration", "underline");
                    tc_accidental_100plus_premium_total4.InnerText = "(" + total_resign_member_accidental_100plus_premium.ToString("C2") + ")";
                }
                else
                {
                    tc_accidental_100plus_premium_total4.InnerText = " - ";
                }

                //Total TPD Premium
                HtmlTableCell tc_tpd_premium_total4 = new HtmlTableCell();
                tc_tpd_premium_total4.Style.Add("text-align", "right");
                tc_tpd_premium_total4.Style.Add("color", "black");
                tc_tpd_premium_total4.Style.Add("padding-right", "6px");
                tc_tpd_premium_total4.Style.Add("font-weight", "bold");

                if (total_resign_member_tpd_premium > 0)
                {
                    tc_tpd_premium_total4.Style.Add("text-decoration", "underline");
                    tc_tpd_premium_total4.InnerText = "(" + total_resign_member_tpd_premium.ToString("C2") + ")";
                }
                else
                {
                    tc_tpd_premium_total4.InnerText = " - ";
                }

                //Total DHC Premium
                HtmlTableCell tc_dhc_premium_total4 = new HtmlTableCell();
                tc_dhc_premium_total4.Style.Add("text-align", "right");
                tc_dhc_premium_total4.Style.Add("color", "black");
                tc_dhc_premium_total4.Style.Add("padding-right", "6px");
                tc_dhc_premium_total4.Style.Add("font-weight", "bold");

                if (total_resign_member_dhc_premium > 0)
                {
                    tc_dhc_premium_total4.Style.Add("text-decoration", "underline");
                    tc_dhc_premium_total4.InnerText = "(" + total_resign_member_dhc_premium.ToString("C2") + ")";
                }
                else
                {
                    tc_dhc_premium_total4.InnerText = " - ";
                }

                //Total Premium
                HtmlTableCell tc_total_premium4 = new HtmlTableCell();
                tc_total_premium4.Style.Add("text-align", "right");
                tc_total_premium4.Style.Add("color", "black");
                tc_total_premium4.Style.Add("padding-right", "6px");
                tc_total_premium4.Style.Add("font-weight", "bold");

                if (total_resign_member_life_premium + total_resign_member_tpd_premium + total_resign_member_dhc_premium + total_resign_member_accidental_100plus_premium > 0)
                {
                    tc_total_premium4.Style.Add("text-decoration", "underline");
                    tc_total_premium4.InnerText = "(" + (total_resign_member_life_premium + total_resign_member_tpd_premium + total_resign_member_dhc_premium + total_resign_member_accidental_100plus_premium).ToString("C2") + ")";
                }
                else
                {
                    tc_total_premium4.InnerText = " - ";
                }

                tr_total4.Controls.Add(tc_number_total4);
                tr_total4.Controls.Add(tc_certificate_number_total4);
                tr_total4.Controls.Add(tc_employee_name_total4);
                tr_total4.Controls.Add(tc_plan_name_total4);
                tr_total4.Controls.Add(tc_effective_date_total4);
                tr_total4.Controls.Add(tc_expiry_date_total4);
                tr_total4.Controls.Add(tc_covery_period_total4);
                tr_total4.Controls.Add(tc_sum_insured_total4);
                tr_total4.Controls.Add(tc_life_premium_total4);
                tr_total4.Controls.Add(tc_accidental_100plus_premium_total4);
                tr_total4.Controls.Add(tc_tpd_premium_total4);
                tr_total4.Controls.Add(tc_dhc_premium_total4);
                tr_total4.Controls.Add(tc_total_premium4);
                tblResignMemberDetail.Controls.Add(tr_total4);

                //Total Print Resign Member Detail Row       
                strTableResignMemberDetailPrint = "<tr>";
                strTableResignMemberDetailPrint += "<td></td>";
                strTableResignMemberDetailPrint += "<td></td>";
                strTableResignMemberDetailPrint += "<td></td>";
                strTableResignMemberDetailPrint += "<td></td>";
                strTableResignMemberDetailPrint += "<td></td>";
                strTableResignMemberDetailPrint += "<td></td>";
                strTableResignMemberDetailPrint += "<td style='text-align: center; font-weight: bold; color: black'>Total:</td>";
                strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline; color: black'>(" + total_resign_member_sum_insured.ToString("C0") + ")</td>";

                if (total_resign_member_life_premium > 0)
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline; color: black'>(" + total_resign_member_life_premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; color: black'> - </td>";
                }

                if (total_resign_member_accidental_100plus_premium > 0)
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline; color: black'>(" + total_resign_member_accidental_100plus_premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; color: black'> - </td>";
                }

                if (total_resign_member_tpd_premium > 0)
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline; color: black'>(" + total_resign_member_tpd_premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; color: black'> - </td>";
                }

                if (total_resign_member_dhc_premium > 0)
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline; color: black'(>" + total_resign_member_dhc_premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; color: black'> - </td>";
                }


                if (total_resign_member_life_premium + total_resign_member_tpd_premium + total_resign_member_dhc_premium + total_resign_member_accidental_100plus_premium > 0)
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline; color: black'>(" + (total_resign_member_life_premium + total_resign_member_tpd_premium + total_resign_member_dhc_premium + total_resign_member_accidental_100plus_premium).ToString("C2") + ")</td>";

                }
                else
                {
                    strTableResignMemberDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; color: black'> - </td>";
                }

                strTableResignMemberDetailPrint += "</table>";

                dvPrintResignMemberDetail.Controls.Add(new LiteralControl(strTableResignMemberDetailPrint));

           #endregion
        }
    }
}