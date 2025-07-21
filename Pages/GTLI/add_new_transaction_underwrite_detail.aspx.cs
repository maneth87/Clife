using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_add_new_transaction_underwrite_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                //get premium id
                string premium_temporay_id = Request.Params["pid"];

                //get gtli premium temporary by id
                bl_gtli_premium premium_temporary = new bl_gtli_premium();
                premium_temporary = da_gtli_premium_temporary.GetGTLIPremiumTemporayByID(premium_temporay_id);

                string payment_code = da_gtli_policy_prem_pay_temporary.GetGTLIPolicyPremPayByGTLIPremiumID(premium_temporary.GTLI_Premium_ID).Payment_Code.Trim();

                //Get plan by id
                bl_gtli_plan plan = new bl_gtli_plan();
                plan = da_gtli_plan.GetPlan(premium_temporary.GTLI_Plan_ID);

                //get gtli_policy temporary by id
                bl_gtli_policy policy_temporary = da_gtli_policy_temporary.GetGTLIPolicyTemporayByID(premium_temporary.GTLI_Policy_ID);

                if (policy_temporary == null)
                {
                    policy_temporary = da_gtli_policy.GetGTLIPolicyByID(premium_temporary.GTLI_Policy_ID);
                }

                //printing date
                lblDate.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
                lblDate2.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");

                switch (premium_temporary.Transaction_Type)
                {
                    case 1:
                        lblTitle.Text = "Insurance Premium";
                        lblTitle2.Text = "Insurance Premium";
                        break;
                    case 2:
                        lblTitle.Text = "New Member Premium";
                        lblTitle2.Text = "New Member Premium";
                        break;
                }

                bl_gtli_company company = da_gtli_company.GetObjCompanyByID(policy_temporary.GTLI_Company_ID);
                string sale_agent_name = da_sale_agent.GetSaleAgentNameByID(premium_temporary.Sale_Agent_ID);
                bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(policy_temporary.GTLI_Company_ID);

                DateTime expiry_date, effective_date;

                expiry_date = premium_temporary.Expiry_Date;
                effective_date = premium_temporary.Effective_Date;

                lblPolicyNumber.Text = policy_temporary.Policy_Number;
                lblCreatedDate.Text = premium_temporary.Created_On.ToString("d-MMM-yyyy");
                lblAgentName.Text = sale_agent_name;
                lblCompanyName.Text = company.Company_Name;
                lblTypeOfBusiness.Text = company.Type_Of_Business;
                lblContactName.Text = contact.Contact_Name;
                lblPhone.Text = contact.Contact_Phone;
                lblEmail.Text = contact.Contact_Email;
                lblAddress.Text = company.Company_Address;
               
                lblPremiumPayment.Text = (premium_temporary.Original_Life_Premium + premium_temporary.Original_TPD_Premium + premium_temporary.Original_DHC_Premium + premium_temporary.Original_Accidental_100Plus_Premium).ToString("C2");
                lblPremiumDiscount.Text = (premium_temporary.Life_Premium_Discount + premium_temporary.TPD_Premium_Discount + premium_temporary.DHC_Premium_Discount + premium_temporary.Accidental_100Plus_Premium_Discount).ToString("C2");
                lblPremiumAfterDiscount.Text = (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium).ToString("C2");
                lblEffectiveDate.Text = effective_date.ToString("d-MMM-yyyy");
                lblExpiryDate.Text = expiry_date.ToString("d-MMM-yyyy");

                lblPaymentCode.Text = payment_code;

                //Summary Print
                lblPolicyNumber2.Text = policy_temporary.Policy_Number;
                lblCreatedDate2.Text = premium_temporary.Created_On.ToString("d-MMM-yyyy");
                lblSaleAgent2.Text = sale_agent_name;
                lblCompanyName2.Text = company.Company_Name;
                lblTypeOfBusiness2.Text = company.Type_Of_Business;
                lblContactPerson2.Text = contact.Contact_Name;
                lblPhoneContact2.Text = contact.Contact_Phone;
                lblContactEmail2.Text = contact.Contact_Email;
                lblCompanyAddress2.Text = company.Company_Address;            
                lblPremiumPayment2.Text = (premium_temporary.Original_Life_Premium + premium_temporary.Original_TPD_Premium + premium_temporary.Original_DHC_Premium + premium_temporary.Original_Accidental_100Plus_Premium).ToString("C2");
                lblPremiumDiscount2.Text = (premium_temporary.Life_Premium_Discount + premium_temporary.TPD_Premium_Discount + premium_temporary.DHC_Premium_Discount + premium_temporary.Accidental_100Plus_Premium_Discount).ToString("C2");
                lblPremiumAfterDiscount2.Text = (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium).ToString("C2");
                lblEffectiveDate2.Text = effective_date.ToString("d-MMM-yyyy");
                lblExpiryDate2.Text = expiry_date.ToString("d-MMM-yyyy");

                lblPaymentCode2.Text = payment_code;

                if (plan.Sum_Insured == 0)
                {
                    lblSumInsured.Text = "Dynamic";
                    lblSumInsured2.Text = "Dynamic";
                }
                else
                {
                    lblSumInsured.Text = premium_temporary.Sum_Insured.ToString("C0");
                    lblSumInsured2.Text = premium_temporary.Sum_Insured.ToString("C0");
                }

                //Premium Detail

                //view content according to transaction type (1 = original sale, 2 = new plan, 3 = add to existing plan, 4 = resign member)
                Double life_premium, tpd_premium, dhc_premium, accidental_100plus_premium;
                switch (premium_temporary.Transaction_Type)
                {
                    case 1:

                        //view original policy premium detail                 

                        string plan_name = da_gtli_plan.GetPlan(premium_temporary.GTLI_Plan_ID).GTLI_Plan;

                        //employee premium for this policy temporary transaction
                        ArrayList employee_temporary_list = da_gtli_employee_temporary.GetGTLIEmployeeTemporayListByPremiumID(premium_temporary.GTLI_Premium_ID);

                        double total_sum_insured = 0;
                        double total_life_premium = 0;
                        double total_dhc_premium = 0;
                        double total_tpd_premium = 0;
                        double total_accidental_100plus_premium = 0;

                        int number = 0;
                        int rows = 0;

                        string strTableDetailPrint = "<table class='gridtable' width='98%'>";
                        strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

                        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                        strTableDetailPrint = "";

                        //loop through employee list
                        for (int k = 0; k <= employee_temporary_list.Count - 1; k++)
                        {
                            bl_gtli_employee employee_temporary = (bl_gtli_employee)employee_temporary_list[k];
                            life_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Death");
                            tpd_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "TPD");
                            dhc_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "DHC");
                            accidental_100plus_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Accidental100Plus");

                            double employee_sum_insure = da_gtli_employee_premium_temporary.GetSumInsuredByGTLICertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Death");

                            //add to total
                            total_dhc_premium += dhc_premium;
                            total_life_premium += life_premium;
                            total_tpd_premium += tpd_premium;
                            total_accidental_100plus_premium += accidental_100plus_premium;

                          
                            if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                            {
                                total_sum_insured += 0;
                              
                            }
                            else
                            {
                                total_sum_insured += employee_sum_insure;
                               
                            }

                            number += 1;

                            string str_certificate_no = employee_temporary.Certificate_Number.ToString();
                            while (str_certificate_no.Length < 6)
                            {
                                str_certificate_no = "0" + str_certificate_no;
                            }

                            //Check existing active employee and double employee in same file upload for this company

                            //loop this_policy_temporary_employee to check employee appear double
                            bool double_employee = false;
                            for (int l = 0; l <= employee_temporary_list.Count - 1; l++)
                            {
                                bl_gtli_employee employee2 = (bl_gtli_employee)employee_temporary_list[l];
                                if (employee_temporary.Employee_Name.Equals(employee2.Employee_Name) & employee_temporary.DOB.Equals(employee2.DOB) & employee_temporary.Gender.Equals(employee2.Gender) & k != l)
                                {
                                    double_employee = true;
                                    //Same employee in another row found
                                }
                            }

                            if (da_gtli_employee.CheckGTLIEmployee(employee_temporary.Employee_Name, employee_temporary.Gender, employee_temporary.DOB, policy_temporary.GTLI_Company_ID) | double_employee == true | (life_premium + tpd_premium + dhc_premium) <= 0)
                            {
                                //Apply red color to this row
                                //Row
                                HtmlTableRow tr_red = new HtmlTableRow();
                                tr_red.Style.Add("font-size", "8pt");
                                tr_red.Style.Add("color", "red");

                                //Number
                                HtmlTableCell tc_number = new HtmlTableCell();
                                tc_number.Width = "30px";
                                tc_number.Style.Add("text-align", "center");
                                tc_number.InnerText = (number).ToString();

                                //Certificate Number
                                HtmlTableCell tc_certificate_number = new HtmlTableCell();
                                tc_certificate_number.Style.Add("padding-left", "5px");



                                //Employee Name
                                HtmlTableCell tc_employee_name = new HtmlTableCell();
                                tc_employee_name.Style.Add("text-align", "left");
                                tc_employee_name.Style.Add("padding-left", "5px");
                                tc_employee_name.InnerText = employee_temporary.Employee_Name;

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

                                if (employee_temporary.Certificate_Number != 0)
                                {
                                    tc_certificate_number.InnerText = str_certificate_no;
                                    tc_plan_name.InnerText = plan_name;
                                    tc_effective_date.InnerText = premium_temporary.Effective_Date.ToString("d-MMM-yyyy");
                                    tc_expiry_date.InnerText = premium_temporary.Expiry_Date.ToString("d-MMM-yyyy");
                                    tc_covery_period.InnerText = "365";

                                }
                                else
                                {
                                    tc_certificate_number.InnerText = " - ";
                                    tc_plan_name.InnerText = " - ";
                                    tc_effective_date.InnerText = " - ";
                                    tc_expiry_date.InnerText = " - ";
                                    tc_covery_period.InnerText = " - ";

                                }

                                tr_red.Controls.Add(tc_number);
                                tr_red.Controls.Add(tc_certificate_number);
                                tr_red.Controls.Add(tc_employee_name);
                                tr_red.Controls.Add(tc_plan_name);
                                tr_red.Controls.Add(tc_effective_date);
                                tr_red.Controls.Add(tc_expiry_date);
                                tr_red.Controls.Add(tc_covery_period);
                                tr_red.Controls.Add(tc_sum_insured);
                                tr_red.Controls.Add(tc_life_premium);
                                tr_red.Controls.Add(tc_accidental_100plus_premium);
                                tr_red.Controls.Add(tc_tpd_premium);
                                tr_red.Controls.Add(tc_dhc_premium);
                                tr_red.Controls.Add(tc_sub_total_premium);
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
                                strTableDetailPrint += "<td style='text-align: center; color: red' width='30px'>" + (number).ToString() + "</td>";

                                if (employee_temporary.Certificate_Number != 0)
                                {
                                    strTableDetailPrint += "<td style='padding-left: 5px; color: red'>" + str_certificate_no + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='padding-left: 5px; color: red'> - </td>";
                                }

                                strTableDetailPrint += "<td style='text-align: left; padding-left: 5px; color: red'>" + employee_temporary.Employee_Name + "</td>";

                                if (employee_temporary.Certificate_Number != 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: center; color: red'>" + plan_name + "</td>";
                                    strTableDetailPrint += "<td style='text-align: center; color: red'>" + premium_temporary.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint += "<td style='text-align: center; color: red'>" + premium_temporary.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint += "<td style='text-align: center; color: red'> 365 </td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: center; color: red'> - </td>";
                                }

                                if (employee_sum_insure != 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + employee_sum_insure.ToString("C0") + "</td>";
                                }
                                else
                                {
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


                                if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium> 0)
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

                            }
                            else
                            {
                                //Apply black color
                                //Row
                                HtmlTableRow tr_black = new HtmlTableRow();
                                tr_black.Style.Add("color", "Black");
                                tr_black.Style.Add("font-size", "8pt");

                                //Number
                                HtmlTableCell tc_number = new HtmlTableCell();
                                tc_number.Width = "30px";
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

                                if (employee_temporary.Certificate_Number != 0)
                                {
                                    tc_certificate_number.InnerText = str_certificate_no;
                                    tc_employee_name.InnerText = employee_temporary.Employee_Name;
                                    tc_effective_date.InnerText = premium_temporary.Effective_Date.ToString("d-MMM-yyyy");
                                    tc_expiry_date.InnerText = premium_temporary.Expiry_Date.ToString("d-MMM-yyyy");
                                    tc_covery_period.InnerText = "365";
                                    tc_plan_name.InnerText = plan_name;
                                }
                                else
                                {
                                    tc_certificate_number.InnerText = " - ";
                                    tc_employee_name.InnerText = " - ";
                                    tc_effective_date.InnerText = " - ";
                                    tc_expiry_date.InnerText = " - ";
                                    tc_covery_period.InnerText = " - ";
                                    tc_plan_name.InnerText = " - ";
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

                                if (employee_temporary.Certificate_Number != 999999)
                                {
                                    strTableDetailPrint += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                                    strTableDetailPrint += "<td style='padding-left: 5px'>" + str_certificate_no + "</td>";
                                    strTableDetailPrint += "<td style='text-align: left; padding-left: 5px;'>" + employee_temporary.Employee_Name + "</td>";
                                    strTableDetailPrint += "<td style='text-align: center;'>" + plan_name + "</td>";
                                    strTableDetailPrint += "<td style='text-align: center;'>" + premium_temporary.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint += "<td style='text-align: center;'>" + premium_temporary.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint += "<td style='text-align: center;'> 365 </td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                                    strTableDetailPrint += "<td style='padding-left: 5px'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: left; padding-left: 5px;'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                                    strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                                }

                                if (employee_sum_insure != 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + employee_sum_insure.ToString("C0") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                if (life_premium != 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + life_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                if (accidental_100plus_premium != 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + accidental_100plus_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                if (tpd_premium != 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + tpd_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }


                                if (dhc_premium != 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + dhc_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }


                                if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + "</td>";

                                }
                                else
                                {
                                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                strTableDetailPrint += "</tr>";

                                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                                strTableDetailPrint = "";

                            }

                        }

                        //Total Row               
                        HtmlTableRow tr_total = new HtmlTableRow();
                        tr_total.Style.Add("color", "Black");
                        tr_total.Style.Add("font-size", "8pt");

                        //Total Number
                        HtmlTableCell tc_number_total = new HtmlTableCell();
                        tc_number_total.Width = "30px";
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
                        tc_sum_insured_total.Style.Add("font-decoration", "underline");
                        tc_sum_insured_total.InnerText = total_sum_insured.ToString("C0");

                        //Total Life Premium
                        HtmlTableCell tc_life_premium_total = new HtmlTableCell();
                        tc_life_premium_total.Style.Add("text-align", "right");
                        tc_life_premium_total.Style.Add("color", "black");
                        tc_life_premium_total.Style.Add("padding-right", "6px");
                        tc_life_premium_total.Style.Add("font-weight", "bold");
                        if (total_life_premium > 0)
                        {
                            tc_life_premium_total.Style.Add("font-decoration", "underline");
                            tc_life_premium_total.InnerText = total_life_premium.ToString("C2");
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
                        if (total_accidental_100plus_premium > 0)
                        {
                            tc_accidental_100plus_premium_total.Style.Add("font-decoration", "underline");
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
                            tc_tpd_premium_total.Style.Add("font-decoration", "underline");
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
                            tc_dhc_premium_total.Style.Add("font-decoration", "underline");
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
                        if (total_life_premium + total_tpd_premium + total_dhc_premium + total_accidental_100plus_premium> 0)
                        {
                            tc_total_premium.Style.Add("font-decoration", "underline");
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
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_sum_insured.ToString("C0") + "</td>";


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

                        break;

                    case 2:
                        //view add new member premium detail to (new plan, existing plan)                  

                        string plan_name2 = da_gtli_plan.GetPlan(premium_temporary.GTLI_Plan_ID).GTLI_Plan;

                        TimeSpan mytimespan = premium_temporary.Expiry_Date.Subtract(premium_temporary.Effective_Date);
                        int coverage_period = mytimespan.Days + 1;

                        //employee premium for this policy temporary transaction
                        ArrayList employee2_temporary_list = da_gtli_employee_temporary.GetGTLIEmployeeTemporayListByPremiumID(premium_temporary.GTLI_Premium_ID);

                        double total_sum_insured2 = 0;
                        double total_life_premium2 = 0;
                        double total_dhc_premium2 = 0;
                        double total_tpd_premium2 = 0;
                        double total_accidental_100plus_premium2 = 0;

                        int number2 = 0;
                        int rows2 = 0;

                        string strTableDetailPrint2 = "<table class='gridtable' width='98%'>";
                        strTableDetailPrint2 += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

                        //loop through employee list
                        for (int k = 0; k <= employee2_temporary_list.Count - 1; k++)
                        {
                            bl_gtli_employee employee = (bl_gtli_employee)employee2_temporary_list[k];
                            life_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Death");
                            tpd_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "TPD");
                            dhc_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "DHC");
                            accidental_100plus_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Accidental100Plus");

                            double employee_sum_insure2 = da_gtli_employee_premium_temporary.GetSumInsuredByGTLICertificateID(employee.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Death");

                            //add to total
                            total_dhc_premium2 += dhc_premium;
                            total_life_premium2 += life_premium;
                            total_tpd_premium2 += tpd_premium;
                            total_accidental_100plus_premium2 += accidental_100plus_premium;

                         
                            if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                            {
                                total_sum_insured2 += 0;
                               
                            }
                            else
                            {
                               // total_sum_insured2 += premium_temporary.Sum_Insured;
                                //maneth
                                total_sum_insured2 += employee_sum_insure2;

                            }

                            number2 += 1;

                            string str_certificate_no = employee.Certificate_Number.ToString();

                            while (str_certificate_no.Length < 6)
                            {
                                str_certificate_no = "0" + str_certificate_no;
                            }

                            //Check existing active employee and double employee in same file upload for this company

                            //loop this_policy_temporary_employee to check double employee
                            bool double_employee = false;
                            for (int l = 0; l <= employee2_temporary_list.Count - 1; l++)
                            {
                                bl_gtli_employee employee2 = (bl_gtli_employee)employee2_temporary_list[l];
                                if (employee.Employee_Name.Equals(employee2.Employee_Name) & employee.DOB.Equals(employee2.DOB) & employee.Gender.Equals(employee2.Gender) & k != l)
                                {

                                    //Same employee in another row found
                                    double_employee = true;
                                }
                            }

                            if (da_gtli_employee.CheckGTLIEmployee(employee.Employee_Name, employee.Gender, employee.DOB, policy_temporary.GTLI_Company_ID) | double_employee == true | (life_premium + tpd_premium + dhc_premium) <= 0)
                            {
                                //Apply red color to this row
                                //Row
                                HtmlTableRow tr_red = new HtmlTableRow();
                                tr_red.Style.Add("font-size", "8pt");
                                tr_red.Style.Add("color", "red");

                                //Number
                                HtmlTableCell tc_number = new HtmlTableCell();
                                tc_number.Width = "30px";
                                tc_number.Style.Add("text-align", "center");
                                tc_number.InnerText = (number2).ToString();

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

                                if (employee_sum_insure2 != 0)
                                {
                                    tc_sum_insured.InnerText = employee_sum_insure2.ToString("C0");
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
                                    tc_plan_name.InnerText = plan_name2;
                                    tc_effective_date.InnerText = premium_temporary.Effective_Date.ToString("d-MMM-yyyy");
                                    tc_expiry_date.InnerText = premium_temporary.Expiry_Date.ToString("d-MMM-yyyy");
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

                                tr_red.Controls.Add(tc_number);
                                tr_red.Controls.Add(tc_certificate_number);
                                tr_red.Controls.Add(tc_employee_name);
                                tr_red.Controls.Add(tc_plan_name);
                                tr_red.Controls.Add(tc_effective_date);
                                tr_red.Controls.Add(tc_expiry_date);
                                tr_red.Controls.Add(tc_covery_period);
                                tr_red.Controls.Add(tc_sum_insured);
                                tr_red.Controls.Add(tc_life_premium);
                                tr_red.Controls.Add(tc_accidental_100plus_premium);
                                tr_red.Controls.Add(tc_tpd_premium);
                                tr_red.Controls.Add(tc_dhc_premium);
                                tr_red.Controls.Add(tc_sub_total_premium);
                                tblPremiumDetail.Controls.Add(tr_red);

                                rows2 += 1;

                                if (rows2 == 49)
                                {
                                    strTableDetailPrint2 = "</table><div style='display: block; page-break-before: always;'></div>";
                                    dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint2));

                                    rows2 = 0;

                                    //New table
                                    strTableDetailPrint2 = "<br /><br /><br /><table class='gridtable' width='98%'>";
                                    strTableDetailPrint2 += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                                }

                                strTableDetailPrint2 += "<tr>";

                                if (employee.Certificate_Number != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;' width='30px'>" + (number2).ToString() + "</td>";
                                    strTableDetailPrint2 += "<td style='padding-left: 5px; color: red;'>" + str_certificate_no + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: left; padding-left: 5px; color: red;'>" + employee.Employee_Name + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'>" + plan_name2 + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'>" + premium_temporary.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'>" + premium_temporary.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'>" + coverage_period.ToString() + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;' width='30px'>" + (number2).ToString() + "</td>";
                                    strTableDetailPrint2 += "<td style='padding-left: 5px; color: red;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: left; padding-left: 5px; color: red;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center; color: red;'> - </td>";
                                }


                                if (employee_sum_insure2 != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'>" + employee_sum_insure2.ToString("C0") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'> - </td>";
                                }

                                if (life_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'>" + life_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'> - </td>";
                                }

                                 if (accidental_100plus_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'>" + accidental_100plus_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'> - </td>";
                                }

                                if (tpd_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'>" + tpd_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'> - </td>";
                                }


                                if (dhc_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'>" + dhc_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'> - </td>";
                                }


                                if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'>" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + "</td>";

                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; color: red;'> - </td>";
                                }

                                strTableDetailPrint2 += "</tr>";

                                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint2));

                                strTableDetailPrint2 = "";


                            }
                            else
                            {
                                //Apply black color
                                //Row
                                HtmlTableRow tr_black = new HtmlTableRow();
                                tr_black.Style.Add("color", "Black");
                                tr_black.Style.Add("font-size", "8pt");

                                //Number
                                HtmlTableCell tc_number = new HtmlTableCell();
                                tc_number.Width = "30px";
                                tc_number.Style.Add("text-align", "center");
                                tc_number.Style.Add("color", "black");
                                tc_number.InnerText = (number2).ToString();

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

                                if (employee_sum_insure2 != 0)
                                {
                                    tc_sum_insured.InnerText = employee_sum_insure2.ToString("C0");
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
                                    tc_plan_name.InnerText = plan_name2;
                                    tc_effective_date.InnerText = premium_temporary.Effective_Date.ToString("d-MMM-yyyy");
                                    tc_expiry_date.InnerText = premium_temporary.Expiry_Date.ToString("d-MMM-yyyy");
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

                                rows2 += 1;

                                if (rows2 == 49)
                                {
                                    strTableDetailPrint2 = "</table><div style='display: block; page-break-before: always;'></div>";
                                    dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint2));

                                    rows2 = 0;

                                    //New table
                                    strTableDetailPrint2 = "<br /><br /><br /><table class='gridtable' width='98%'>";
                                    strTableDetailPrint2 += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                                }

                                strTableDetailPrint2 += "<tr>";

                                if (employee.Certificate_Number != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: center;' width='30px'>" + (number2).ToString() + "</td>";
                                    strTableDetailPrint2 += "<td style='padding-left: 5px'>" + str_certificate_no + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: left; padding-left: 5px;'>" + employee.Employee_Name + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'>" + plan_name2 + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'>" + premium_temporary.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'>" + premium_temporary.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'>" + coverage_period.ToString() + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: center;' width='30px'>" + (number2).ToString() + "</td>";
                                    strTableDetailPrint2 += "<td style='padding-left: 5px'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: left; padding-left: 5px;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'> - </td>";
                                    strTableDetailPrint2 += "<td style='text-align: center;'> - </td>";
                                }


                                if (employee_sum_insure2 != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'>" + employee_sum_insure2.ToString("C0") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                if (life_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'>" + life_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                if (accidental_100plus_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'>" + accidental_100plus_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                if (tpd_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'>" + tpd_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }


                                if (dhc_premium != 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'>" + dhc_premium.ToString("C2") + "</td>";
                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }


                                if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'>" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + "</td>";

                                }
                                else
                                {
                                    strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                                }

                                strTableDetailPrint2 += "</tr>";

                                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint2));

                                strTableDetailPrint2 = "";
                            }

                        }

                        //Total Row               
                        HtmlTableRow tr_total2 = new HtmlTableRow();
                        tr_total2.Style.Add("color", "Black");
                        tr_total2.Style.Add("font-size", "8pt");

                        //Total Number
                        HtmlTableCell tc_number_total2 = new HtmlTableCell();
                        tc_number_total2.Width = "30px";
                        tc_number_total2.Style.Add("text-align", "center");
                        tc_number_total2.Style.Add("color", "black");
                        tc_number_total2.InnerText = "";

                        //Total Certificate Number
                        HtmlTableCell tc_certificate_number_total2 = new HtmlTableCell();
                        tc_certificate_number_total2.Style.Add("color", "black");
                        tc_certificate_number_total2.InnerText = "";

                        //Total Employee Name 
                        HtmlTableCell tc_employee_name_total2 = new HtmlTableCell();
                        tc_employee_name_total2.Style.Add("text-align", "left");
                        tc_employee_name_total2.Style.Add("padding-left", "5px");
                        tc_employee_name_total2.Style.Add("color", "black");
                        tc_employee_name_total2.InnerText = "";

                        //Total Plan Name
                        HtmlTableCell tc_plan_name_total2 = new HtmlTableCell();
                        tc_plan_name_total2.Style.Add("text-align", "center");
                        tc_plan_name_total2.Style.Add("color", "black");
                        tc_plan_name_total2.InnerText = "";

                        //Total Effective Date
                        HtmlTableCell tc_effective_date_total2 = new HtmlTableCell();
                        tc_effective_date_total2.Style.Add("text-align", "center");
                        tc_effective_date_total2.Style.Add("color", "black");
                        tc_effective_date_total2.InnerText = "";

                        //Total Expiry Date
                        HtmlTableCell tc_expiry_date_total2 = new HtmlTableCell();
                        tc_expiry_date_total2.Style.Add("text-align", "center");
                        tc_expiry_date_total2.Style.Add("color", "black");
                        tc_expiry_date_total2.InnerText = "";

                        //Total Cover Period
                        HtmlTableCell tc_covery_period_total2 = new HtmlTableCell();
                        tc_covery_period_total2.Style.Add("text-align", "center");
                        tc_covery_period_total2.Style.Add("color", "black");
                        tc_covery_period_total2.Style.Add("font-weight", "bold");
                        tc_covery_period_total2.InnerText = "Total:";

                        //Total Sum Insured
                        HtmlTableCell tc_sum_insured_total2 = new HtmlTableCell();
                        tc_sum_insured_total2.Style.Add("text-align", "right");
                        tc_sum_insured_total2.Style.Add("color", "black");
                        tc_sum_insured_total2.Style.Add("padding-right", "6px");
                        tc_sum_insured_total2.Style.Add("font-weight", "bold");
                        tc_sum_insured_total2.Style.Add("font-decoration", "underline");
                        tc_sum_insured_total2.InnerText = total_sum_insured2.ToString("C0");

                        //Total Life Premium
                        HtmlTableCell tc_life_premium_total2 = new HtmlTableCell();
                        tc_life_premium_total2.Style.Add("text-align", "right");
                        tc_life_premium_total2.Style.Add("color", "black");
                        tc_life_premium_total2.Style.Add("padding-right", "6px");
                        tc_life_premium_total2.Style.Add("font-weight", "bold");
                        if (total_life_premium2 > 0)
                        {
                            tc_life_premium_total2.Style.Add("font-decoration", "underline");
                            tc_life_premium_total2.InnerText = total_life_premium2.ToString("C2");
                        }
                        else
                        {
                            tc_life_premium_total2.InnerText = " - ";
                        }

                        //Total Accidental 100Plus Premium
                        HtmlTableCell tc_accidental_100plus_premium_total2 = new HtmlTableCell();
                        tc_accidental_100plus_premium_total2.Style.Add("text-align", "right");
                        tc_accidental_100plus_premium_total2.Style.Add("color", "black");
                        tc_accidental_100plus_premium_total2.Style.Add("padding-right", "6px");
                        tc_accidental_100plus_premium_total2.Style.Add("font-weight", "bold");
                        if (total_accidental_100plus_premium2 > 0)
                        {
                            tc_accidental_100plus_premium_total2.Style.Add("font-decoration", "underline");
                            tc_accidental_100plus_premium_total2.InnerText = total_accidental_100plus_premium2.ToString("C2");
                        }
                        else
                        {
                            tc_accidental_100plus_premium_total2.InnerText = " - ";
                        }

                        //Total TPD Premium
                        HtmlTableCell tc_tpd_premium_total2 = new HtmlTableCell();
                        tc_tpd_premium_total2.Style.Add("text-align", "right");
                        tc_tpd_premium_total2.Style.Add("color", "black");
                        tc_tpd_premium_total2.Style.Add("padding-right", "6px");
                        tc_tpd_premium_total2.Style.Add("font-weight", "bold");
                        if (total_tpd_premium2 > 0)
                        {
                            tc_tpd_premium_total2.Style.Add("font-decoration", "underline");
                            tc_tpd_premium_total2.InnerText = total_tpd_premium2.ToString("C2");
                        }
                        else
                        {
                            tc_tpd_premium_total2.InnerText = " - ";
                        }

                        //Total DHC Premium
                        HtmlTableCell tc_dhc_premium_total2 = new HtmlTableCell();
                        tc_dhc_premium_total2.Style.Add("text-align", "right");
                        tc_dhc_premium_total2.Style.Add("color", "black");
                        tc_dhc_premium_total2.Style.Add("padding-right", "6px");
                        tc_dhc_premium_total2.Style.Add("font-weight", "bold");
                        if (total_dhc_premium2 > 0)
                        {
                            tc_dhc_premium_total2.Style.Add("font-decoration", "underline");
                            tc_dhc_premium_total2.InnerText = total_dhc_premium2.ToString("C2");
                        }
                        else
                        {
                            tc_dhc_premium_total2.InnerText = " - ";
                        }

                        //Total Premium
                        HtmlTableCell tc_total_premium2 = new HtmlTableCell();
                        tc_total_premium2.Style.Add("text-align", "right");
                        tc_total_premium2.Style.Add("color", "black");
                        tc_total_premium2.Style.Add("padding-right", "6px");
                        tc_total_premium2.Style.Add("font-weight", "bold");
                        if (total_life_premium2 + total_tpd_premium2 + total_dhc_premium2 + total_accidental_100plus_premium2 > 0)
                        {
                            tc_total_premium2.Style.Add("font-decoration", "underline");
                            tc_total_premium2.InnerText = (total_life_premium2 + total_tpd_premium2 + total_dhc_premium2 + total_accidental_100plus_premium2).ToString("C2");
                        }
                        else
                        {
                            tc_total_premium2.InnerText = " - ";
                        }

                        tr_total2.Controls.Add(tc_number_total2);
                        tr_total2.Controls.Add(tc_certificate_number_total2);
                        tr_total2.Controls.Add(tc_employee_name_total2);
                        tr_total2.Controls.Add(tc_plan_name_total2);
                        tr_total2.Controls.Add(tc_effective_date_total2);
                        tr_total2.Controls.Add(tc_expiry_date_total2);
                        tr_total2.Controls.Add(tc_covery_period_total2);
                        tr_total2.Controls.Add(tc_sum_insured_total2);
                        tr_total2.Controls.Add(tc_life_premium_total2);
                        tr_total2.Controls.Add(tc_accidental_100plus_premium_total2);
                        tr_total2.Controls.Add(tc_tpd_premium_total2);
                        tr_total2.Controls.Add(tc_dhc_premium_total2);
                        tr_total2.Controls.Add(tc_total_premium2);
                        tblPremiumDetail.Controls.Add(tr_total2);

                        //Total Print Row       
                        strTableDetailPrint2 = "<tr>";
                        strTableDetailPrint2 += "<td></td>";
                        strTableDetailPrint2 += "<td></td>";
                        strTableDetailPrint2 += "<td></td>";
                        strTableDetailPrint2 += "<td></td>";
                        strTableDetailPrint2 += "<td></td>";
                        strTableDetailPrint2 += "<td></td>";
                        strTableDetailPrint2 += "<td style='text-align: center; font-weight: bold;'>Total:</td>";
                        strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'>" + total_sum_insured2.ToString("C0") + "</td>";

                        if (total_life_premium2 > 0)
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_life_premium2.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                        }

                        if (total_accidental_100plus_premium2 > 0)
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_accidental_100plus_premium2.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                        }

                        if (total_tpd_premium2 > 0)
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_tpd_premium2.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                        }


                        if (total_dhc_premium2 > 0)
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_dhc_premium2.ToString("C2") + "</td>";
                        }
                        else
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                        }


                        if (total_life_premium2 + total_tpd_premium2 + total_dhc_premium2 + total_accidental_100plus_premium2 > 0)
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + (total_life_premium2 + total_tpd_premium2 + total_dhc_premium2 + total_accidental_100plus_premium2).ToString("C2") + "</td>";

                        }
                        else
                        {
                            strTableDetailPrint2 += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                        }

                        strTableDetailPrint2 += "</table>";

                        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint2));

                        break;

                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    //Cancel click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //get premium id
        string premium_temporay_id = Request.Params["pid"];

        //get gtli premium temporary by id
        bl_gtli_premium premium_temporary = new bl_gtli_premium();
        premium_temporary = da_gtli_premium_temporary.GetGTLIPremiumTemporayByID(premium_temporay_id);


        //Delete temporary data
        da_gtli_employee_premium_temporary.DeleteEmployeePremiumTemporary(premium_temporay_id);
        da_gtli_employee_temporary.DeleteGTLIEmployeeTemporary(premium_temporay_id);
        da_gtli_policy_temporary.DeleteGTLIPolicyTemporary(premium_temporary.GTLI_Policy_ID);
        da_gtli_certificate_temporary.DeleteGTLICertificateTemporary(premium_temporay_id);
        da_gtli_premium_temporary.DeleteGTLIPremiumTemporary(premium_temporay_id);
        da_gtli_policy_prem_pay_temporary.DeleteGTLIPolicyPremPayTemporary(premium_temporay_id);
        da_gtli_policy_prem_return_temporary.DeleteGTLIPolicyPremReturnTemporary(premium_temporay_id);

        Response.Redirect("add_member.aspx");
    }

    //Save click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        bool is_new_policy = false;

        //temporary variables
        string policy_temporary_id = "";
        string certificate_temporary_id = "";

        //get premium id
        string premium_temporay_id = Request.Params["pid"];

        //get gtli premium temporary by id
        bl_gtli_premium premium_temporary = new bl_gtli_premium();
        premium_temporary = da_gtli_premium_temporary.GetGTLIPremiumTemporayByID(premium_temporay_id);

        //get gtli_policy temporary by id
        bl_gtli_policy policy_temporary = da_gtli_policy_temporary.GetGTLIPolicyTemporayByID(premium_temporary.GTLI_Policy_ID);

        if (policy_temporary == null)
        {
            //get existing policy
            policy_temporary = da_gtli_policy.GetGTLIPolicyByID(premium_temporary.GTLI_Policy_ID);

            policy_temporary_id = policy_temporary.GTLI_Policy_ID;
        }
        else
        {
            policy_temporary_id = policy_temporary.GTLI_Policy_ID; //Save id of temporary for later use

            //Insert new policy
            policy_temporary.GTLI_Policy_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_ID", "@GTLI_Policy_ID");
            policy_temporary.Maturity_Date = policy_temporary.Expiry_Date.AddDays(1);

            if (!da_gtli_policy.InsertPolicy(policy_temporary))
            {
                
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                return;
            }

            is_new_policy = true;
        }

        //Insert GTLI Premium

        //Set new gtli policy id to gtli premium
        premium_temporary.GTLI_Policy_ID = policy_temporary.GTLI_Policy_ID;
        premium_temporary.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_ID", "@GTLI_Premium_ID");

        if (da_gtli_premium.InsertPremium(premium_temporary))
        {
            //Get Employee Temporary list by premium_id
            ArrayList employee_temporary_list = new ArrayList();
            employee_temporary_list = da_gtli_employee_temporary.GetGTLIEmployeeTemporayListByPremiumID(premium_temporay_id);

            //loop through employee temporary list
            for (int i = 0; i <= employee_temporary_list.Count - 1; i++)
            {
                bl_gtli_employee employee_temporary = new bl_gtli_employee();
                employee_temporary = (bl_gtli_employee)employee_temporary_list[i];

                // modifed by maneth: Description: check exist employee before insert into database
                System.Globalization.DateTimeFormatInfo dtfi2 = new System.Globalization.DateTimeFormatInfo();
                dtfi2.ShortDatePattern = "MM/dd/yyyy";
                dtfi2.DateSeparator = "/";
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(AppConfiguration.GetConnectionString());
                bool exist = false;
              

               
                    //Get Certificate by certificate_id
                    bl_gtli_certificate certificate = new bl_gtli_certificate();
                    certificate = da_gtli_certificate_temporary.GetGTLICertificateTemporayByID(employee_temporary.GTLI_Certificate_ID);

                    //temporary certificate_id
                    string certificate_id = certificate.GTLI_Certificate_ID;

                    //Get last certificate number by this company
                    //certificate.Certificate_Number = da_gtli_certificate.GetGTLILastCertificateNumberByID(policy_temporary.GTLI_Company_ID, policy_temporary.Policy_Number);

                    //certificate.Certificate_Number = certificate.Certificate_Number + 1;
                    //use certificate number from temp table
                    certificate.Certificate_Number = certificate.Certificate_Number;

                    certificate_temporary_id = certificate.GTLI_Certificate_ID; //Save id of temporary for later use

                    certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_ID", "@GTLI_Certificate_ID");


                    //Insert Certificate
                    if (da_gtli_certificate.InsertCertificate(certificate))
                    {
                        //Insert Employee   

                        //Set new gtli certificate id to gtli employee
                        employee_temporary.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;

                        if (da_gtli_employee.InsertGTLIEmployee(employee_temporary))
                        {

                            //check if this customer exist in general policy customer (Clife)
                            char[] space = new char[] { ' ' };
                            string[] name = employee_temporary.Employee_Name.Split(space);

                            //string first_name = name[1];
                            //string last_name = name[0];
                            string first_name = "";
                            string last_name = "";
                            if (name.Length > 1)
                            {
                                first_name = name[1];
                                last_name = name[0];
                            }
                            else
                            {
                                first_name = "";
                                last_name = name[0];

                            }

                            if (da_gtli_employee.CheckGTLIEmployeeInCtCustomer(first_name, last_name, employee_temporary.Gender, employee_temporary.DOB))
                            {
                                //Insert into Ct_Customer_GTLI_Customer table
                                string ct_customer_id = da_gtli_employee.GetCtCustomerID(first_name, last_name, employee_temporary.Gender, employee_temporary.DOB);
                                bl_ct_customer_gtli_employee ct_customer_gtli_employee = new bl_ct_customer_gtli_employee();

                                ct_customer_gtli_employee.Customer_ID = ct_customer_id;
                                ct_customer_gtli_employee.GTLI_Certificate_ID = employee_temporary.GTLI_Certificate_ID;

                                if (!da_ct_customer_gtli_employee.InsertCtCustomerGTLIEmployee(ct_customer_gtli_employee))
                                { //Failed ct_customer_gtli_employee
                                    da_gtli_employee.DeleteCtCustomerGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee.DeleteGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee_premium.DeleteEmployeePremium(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                                    //delete just added policy transaction
                                    if (is_new_policy)
                                    {
                                        da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                                    }

                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                                    return;
                                }
                            }

                            //Get gtli employee premium list by this employee
                            List<bl_gtli_employee_premium> employee_premium_list = new List<bl_gtli_employee_premium>();

                            employee_premium_list = da_gtli_employee_premium_temporary.GetPremiumList(certificate_id, premium_temporay_id);

                            for (int j = 0; j <= employee_premium_list.Count - 1; j++)
                            {
                                bl_gtli_employee_premium premium = new bl_gtli_employee_premium();
                                premium = employee_premium_list[j];

                                premium.GTLI_Employee_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_ID", "@GTLI_Employee_Premium_ID");
                                premium.GTLI_Certificate_ID = employee_temporary.GTLI_Certificate_ID;
                                premium.GTLI_Premium_ID = premium_temporary.GTLI_Premium_ID;

                                //Insert employee premium
                                if (!da_gtli_employee_premium.InsertEmployeePremium(premium))
                                { //Failed Employee Premium
                                    //delete
                                    da_gtli_employee.DeleteCtCustomerGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee.DeleteGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee_premium.DeleteEmployeePremium(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                                    //delete just added policy transaction
                                    if (is_new_policy)
                                    {
                                        da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                                    }

                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                                    return;
                                }
                
                        } //end loop employee premium temporary list
                    }

                    else

                    { //Failed Employee
                        //delete
                        da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                        da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);
                        
                        //delete just added policy transaction
                        if (is_new_policy)
                        {
                            da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                        }

                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                        return;
                    }


                }
                else
                { //Failed Certificate

                    //delete
                    da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                    //delete just added policy transaction
                    if (is_new_policy)
                    {
                        da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                    return;
                }

           

            } //End loop employee temporary list



            //Get policy prem pay temporary
            bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
            prem_pay = da_gtli_policy_prem_pay_temporary.GetGTLIPolicyPremPayByGTLIPremiumID(premium_temporay_id);
            prem_pay.Status = 1;

            //Insert policy prem pay
            prem_pay.GTLI_Premium_ID = premium_temporary.GTLI_Premium_ID;
            prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Pay_ID", "@GTLI_Policy_Prem_Pay_ID");

            if (da_gtli_policy_prem_pay.InsertPolicyPremPay(prem_pay))
            {
                //Update premium in ct_gtli_policy
                da_gtli_policy.UpdatePremium(premium_temporary.Life_Premium, premium_temporary.TPD_Premium, premium_temporary.DHC_Premium, premium_temporary.Accidental_100Plus_Premium, premium_temporary.Accidental_100Plus_Premium_Discount, premium_temporary.Life_Premium_Discount, premium_temporary.TPD_Premium_Discount, premium_temporary.DHC_Premium_Discount, premium_temporary.Original_Accidental_100Plus_Premium, premium_temporary.Original_Life_Premium, premium_temporary.Original_TPD_Premium, premium_temporary.Original_DHC_Premium, premium_temporary.Accidental_100Plus_Premium_Tax_Amount, premium_temporary.Life_Premium_Tax_Amount, premium_temporary.TPD_Premium_Tax_Amount, premium_temporary.DHC_Premium_Tax_Amount, premium_temporary.GTLI_Policy_ID);

                //Reach here all save successful
                Response.Redirect("add_new_transaction_detail.aspx?pid=" + premium_temporary.GTLI_Premium_ID);
            }
            else
            { //Failed Prem Pay
                //delete
                da_gtli_employee.DeleteCtCustomerGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                da_gtli_employee.DeleteGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                da_gtli_employee_premium.DeleteEmployeePremium(premium_temporary.GTLI_Premium_ID);
                da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                //delete just added policy transaction
                if (is_new_policy)
                {
                    da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                }

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                return;

            }
        }
        else
        { //Failed Premium
                    
            //delete just added policy transaction
            if (is_new_policy)
            {
                da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
            }
                       
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
            return;
        }


    }
}