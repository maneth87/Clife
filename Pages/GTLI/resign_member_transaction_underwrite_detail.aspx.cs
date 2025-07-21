using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Pages_GTLI_resign_member_transaction_underwrite_detail : System.Web.UI.Page
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

                //Get Payment Code
                string payment_code = da_gtli_policy_prem_return_temporary.GetGTLPolicyPremReturnByGTLIPremiumID(premium_temporary.GTLI_Premium_ID).Payment_Code.Trim();

                //get gtli_policy temporary by id
                bl_gtli_policy policy_temporary = da_gtli_policy_temporary.GetGTLIPolicyTemporayByID(premium_temporary.GTLI_Policy_ID);

                if (policy_temporary == null)
                {
                    policy_temporary = da_gtli_policy.GetGTLIPolicyByID(premium_temporary.GTLI_Policy_ID);
                }

                //printing date
                lblDate.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
                lblDate2.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");

                lblTitle.Text = "Resigned Member List";
                lblTitle2.Text = "Resigned Member List";

                bl_gtli_company company = da_gtli_company.GetObjCompanyByID(policy_temporary.GTLI_Company_ID);
                string sale_agent_name = da_sale_agent.GetSaleAgentNameByID(premium_temporary.Sale_Agent_ID);
                bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(policy_temporary.GTLI_Company_ID);

                System.DateTime expiry_date = default(System.DateTime);
                System.DateTime effective_date = default(System.DateTime);
                double life_premium = 0;
                double tpd_premium = 0;
                double dhc_premium = 0;
                double accidental_100plus_premium = 0;

                expiry_date = premium_temporary.Expiry_Date;
                effective_date = premium_temporary.Effective_Date;

                lblPolicyNumber.Text = policy_temporary.Policy_Number;
                lblCompanyName.Text = company.Company_Name;
                lblTypeOfBusiness.Text = company.Type_Of_Business;
                lblContactName.Text = contact.Contact_Name;
                lblPhone.Text = contact.Contact_Phone;
                lblEmail.Text = contact.Contact_Email;
                lblAddress.Text = company.Company_Address;
                lblEffectiveDate.Text = effective_date.ToString("d-MMM-yyyy");
                lblPremiumReturn.Text = (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium).ToString("C2");
                lblPaymentCode.Text = payment_code;
                
                //Summary Print
                lblPolicyNumber2.Text = policy_temporary.Policy_Number;
                lblCompanyName2.Text = company.Company_Name;
                lblTypeOfBusiness2.Text = company.Type_Of_Business;
                lblContactPerson2.Text = contact.Contact_Name;
                lblPhoneContact2.Text = contact.Contact_Phone;
                lblContactEmail2.Text = contact.Contact_Email;
                lblCompanyAddress2.Text = company.Company_Address;
                lblEffectiveDate2.Text = effective_date.ToString("d-MMM-yyyy");
                lblPremiumReturn2.Text = (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium).ToString("C2");
                lblPaymentCode2.Text = payment_code;

                //view member resigning list          

                //employee premium for this policy transaction
                ArrayList employee_temporary_list = da_gtli_employee_temporary.GetGTLIEmployeeTemporayListByPremiumID(premium_temporary.GTLI_Premium_ID);

                TimeSpan mytimespan = premium_temporary.Expiry_Date.Subtract(premium_temporary.Effective_Date);
                int days_not_cover = mytimespan.Days + 1;

                int number = 0;
                int count = 0;
                int rows = 0;

                string strTableDetailPrint = "<table class='gridtable' width='98%'>";
                strTableDetailPrint += "<tr><th>No.</th><th style='width:60px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:70px;'>Resigning Date</th><th style='text-align: center; width:70px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Days not Cover</th><th>Sum Insured</th><th>Life Return Premium</th><th>TPD Return Premium</th><th>DHC Return Premium</th><th>Total Return Premium</th></tr>";

                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                strTableDetailPrint = "";

                //loop through employee list
                for (int k = 0; k <= employee_temporary_list.Count - 1; k++)
                {
                    bl_gtli_employee employee_temporary = (bl_gtli_employee)employee_temporary_list[k];

                    string plan_name = da_gtli_plan.GetPlanNameByCertificateID(employee_temporary.GTLI_Certificate_ID);

                    //get employee_premium_life, employee_premium_tpd, employee_premium_dhc
                    life_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Death");
                    tpd_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "TPD");
                    dhc_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "DHC");
                    accidental_100plus_premium = da_gtli_employee_premium_temporary.GetPremiumByCertificateID(employee_temporary.GTLI_Certificate_ID, premium_temporary.GTLI_Premium_ID, "Accidental_100Plus_Premium");

                    double my_sum_insured = 0; //sum_insured for display

                    //get sum insured of this employee             

                    if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                    {
                        my_sum_insured = 0;
                    }
                    else
                    {
                        my_sum_insured = da_gtli_premium.GetGTLPremiumByCerficateID(employee_temporary.GTLI_Certificate_ID).Sum_Insured;
                    }

                    number += 1;
                    count += 1;

                    string str_certificate_no = employee_temporary.Certificate_Number.ToString();

                    while (str_certificate_no.Length < 6)
                    {
                        str_certificate_no = "0" + str_certificate_no;
                    }

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
                    tc_certificate_number.InnerText = str_certificate_no;

                    //Employee Name
                    HtmlTableCell tc_employee_name = new HtmlTableCell();
                    tc_employee_name.Style.Add("text-align", "left");
                    tc_employee_name.Style.Add("padding-left", "5px");
                    tc_employee_name.Style.Add("color", "black");
                    tc_employee_name.InnerText = employee_temporary.Employee_Name;

                    //Plan Name
                    HtmlTableCell tc_plan_name = new HtmlTableCell();
                    tc_plan_name.Style.Add("text-align", "center");
                    tc_plan_name.Style.Add("color", "black");
                    tc_plan_name.InnerText = plan_name;

                    //Effective Date
                    HtmlTableCell tc_effective_date = new HtmlTableCell();
                    tc_effective_date.Style.Add("text-align", "center");
                    tc_effective_date.Style.Add("color", "black");
                    tc_effective_date.InnerText = premium_temporary.Effective_Date.ToString("d-MMM-yyyy");

                    //Expiry Date
                    HtmlTableCell tc_expiry_date = new HtmlTableCell();
                    tc_expiry_date.Style.Add("text-align", "center");
                    tc_expiry_date.Style.Add("color", "black");
                    tc_expiry_date.InnerText = premium_temporary.Expiry_Date.ToString("d-MMM-yyyy");

                    //Cover Period
                    HtmlTableCell tc_covery_period = new HtmlTableCell();
                    tc_covery_period.Style.Add("text-align", "center");
                    tc_covery_period.Style.Add("color", "black");
                    tc_covery_period.InnerText = days_not_cover.ToString();

                    //Sum Insured
                    HtmlTableCell tc_sum_insured = new HtmlTableCell();
                    tc_sum_insured.Style.Add("text-align", "right");
                    tc_sum_insured.Style.Add("color", "black");
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
                    tc_life_premium.Style.Add("color", "black");
                    tc_life_premium.Style.Add("padding-right", "6px");
                    if (life_premium != 0)
                    {
                        tc_life_premium.InnerText = "(" + life_premium.ToString("C2") + ")";
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
                        tc_accidental_100plus_premium.InnerText = "(" + accidental_100plus_premium.ToString("C2") + ")";
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
                        tc_tpd_premium.InnerText = "(" + tpd_premium.ToString("C2") + ")";
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
                        tc_dhc_premium.InnerText = "(" + dhc_premium.ToString("C2") + ")";
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
                        tc_sub_total_premium.InnerText = "(" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + ")";
                    }
                    else
                    {
                        tc_sub_total_premium.InnerText = " - ";
                    }

                    if (employee_temporary.Certificate_Number != 999999)
                    {
                        tc_certificate_number.InnerText = str_certificate_no;
                        tc_plan_name.InnerText = plan_name;
                        tc_effective_date.InnerText = premium_temporary.Effective_Date.ToString("d-MMM-yyyy");
                        tc_expiry_date.InnerText = premium_temporary.Expiry_Date.ToString("d-MMM-yyyy");
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

                    rows += 1;

                    if (rows == 49)
                    {
                        strTableDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                        dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                        rows = 0;

                        //New table
                        strTableDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                        strTableDetailPrint += "<tr><th>No.</th><th style='width:60px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:70px;'>Resigning Date</th><th style='text-align: center; width:70px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Days not Cover</th><th>Sum Insured</th><th>Life Return Premium</th><th>100Plus Return Premium</th><th>TPD Return Premium</th><th>DHC Return Premium</th><th>Total Return Premium</th></tr>";
                    }

                    strTableDetailPrint += "<tr>";
                    if (employee_temporary.Certificate_Number != 999999)
                    {
                        strTableDetailPrint += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                        strTableDetailPrint += "<td style='padding-left: 5px;'>" + str_certificate_no + "</td>";
                        strTableDetailPrint += "<td style='text-align: left; padding-left: 5px;'>" + employee_temporary.Employee_Name + "</td>";
                        strTableDetailPrint += "<td style='text-align: center;'>" + plan_name + "</td>";
                        strTableDetailPrint += "<td style='text-align: center;'>" + premium_temporary.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                        strTableDetailPrint += "<td style='text-align: center;'>" + premium_temporary.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                        strTableDetailPrint += "<td style='text-align: center;'>" + days_not_cover.ToString() + "</td>";
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>(" + my_sum_insured.ToString("C0") + ")</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                        strTableDetailPrint += "<td style='padding-left: 5px;'> - </td>";
                        strTableDetailPrint += "<td style='text-align: left; padding-left: 5px;'>" + employee_temporary.Employee_Name + "</td>";
                        strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                        strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                        strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                        strTableDetailPrint += "<td style='text-align: center;'> - </td>";
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }


                    if (life_premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>(" + life_premium.ToString("C2") + ")</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (accidental_100plus_premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>(" + accidental_100plus_premium.ToString("C2") + ")</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (tpd_premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>(" + tpd_premium.ToString("C2") + ")</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }


                    if (dhc_premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>(" + dhc_premium.ToString("C2") + ")</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }


                    if (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium > 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>(" + (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium).ToString("C2") + ")</td>";

                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    strTableDetailPrint += "</tr>";

                    dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

                    strTableDetailPrint = "";
                }

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
                tc_covery_period_total.InnerText = "Total:";

                //Total Sum Insured
                HtmlTableCell tc_sum_insured_total = new HtmlTableCell();
                tc_sum_insured_total.Style.Add("text-align", "right");
                tc_sum_insured_total.Style.Add("color", "black");
                tc_sum_insured_total.Style.Add("padding-right", "6px");
                tc_sum_insured_total.Style.Add("font-weight", "bold");
                tc_sum_insured_total.Style.Add("text-decoration", "underline");
                tc_sum_insured_total.InnerText = "(" + premium_temporary.Sum_Insured.ToString("C0") + ")";

                //Total Life Premium
                HtmlTableCell tc_life_premium_total = new HtmlTableCell();
                tc_life_premium_total.Style.Add("text-align", "right");
                tc_life_premium_total.Style.Add("color", "black");
                tc_life_premium_total.Style.Add("padding-right", "6px");
                tc_life_premium_total.Style.Add("font-weight", "bold");
                if (premium_temporary.Life_Premium > 0)
                {
                    tc_life_premium_total.Style.Add("text-decoration", "underline");
                    tc_life_premium_total.InnerText = "(" + premium_temporary.Life_Premium.ToString("C2") + ")";
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
                if (premium_temporary.Accidental_100Plus_Premium > 0)
                {
                    tc_accidental_100plus_premium_total.Style.Add("text-decoration", "underline");
                    tc_accidental_100plus_premium_total.InnerText = "(" + premium_temporary.Accidental_100Plus_Premium.ToString("C2") + ")";
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
                if (premium_temporary.TPD_Premium > 0)
                {
                    tc_tpd_premium_total.Style.Add("text-decoration", "underline");
                    tc_tpd_premium_total.InnerText = "(" + premium_temporary.TPD_Premium.ToString("C2") + ")";
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
                if (premium_temporary.DHC_Premium > 0)
                {
                    tc_dhc_premium_total.Style.Add("text-decoration", "underline");
                    tc_dhc_premium_total.InnerText = "(" + premium_temporary.DHC_Premium.ToString("C2") + ")";
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
                if (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium > 0)
                {
                    tc_total_premium.Style.Add("text-decoration", "underline");
                    tc_total_premium.InnerText = "(" + (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium).ToString("C2") + ")";
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
                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + premium_temporary.Sum_Insured.ToString("C0") + ")</td>";


                if (premium_temporary.Life_Premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + premium_temporary.Life_Premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }

                if (premium_temporary.Accidental_100Plus_Premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + premium_temporary.Accidental_100Plus_Premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }

                if (premium_temporary.TPD_Premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + premium_temporary.TPD_Premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }


                if (premium_temporary.DHC_Premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + premium_temporary.DHC_Premium.ToString("C2") + ")</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }


                if (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>(" + (premium_temporary.Life_Premium + premium_temporary.TPD_Premium + premium_temporary.DHC_Premium + premium_temporary.Accidental_100Plus_Premium).ToString("C2") + ")</td>";

                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
                }

                strTableDetailPrint += "</table>";

                dvPrintDetail.Controls.Add(new LiteralControl(strTableDetailPrint));

            }
            catch (Exception ex)
            {

            }

        }
    }

    //Button Cancel Click
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

        Response.Redirect("resign_member.aspx");
    }

    //Button Save Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //get premium id
        string premium_temporay_id = Request.Params["pid"];

        //get gtli premium temporary by id
        bl_gtli_premium premium_temporary = new bl_gtli_premium();
        premium_temporary = da_gtli_premium_temporary.GetGTLIPremiumTemporayByID(premium_temporay_id);

        //get gtli_policy temporary by id
        bl_gtli_policy policy_temporary = da_gtli_policy_temporary.GetGTLIPolicyTemporayByID(premium_temporary.GTLI_Policy_ID);

        if (policy_temporary == null)
        {
            policy_temporary = da_gtli_policy.GetGTLIPolicyByID(premium_temporary.GTLI_Policy_ID);
        }

        string policy_temporary_id = policy_temporary.GTLI_Policy_ID;

        //Insert GTLI Premium

        //Set new gtli policy id to gtli premium
        premium_temporary.GTLI_Policy_ID = policy_temporary.GTLI_Policy_ID;
        premium_temporary.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_ID", "@GTLI_Premium_ID");

        if (!da_gtli_premium.InsertPremium(premium_temporary))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
            return;
        }
        
        //Get Employee Temporary list by premium_id
        ArrayList employee_temporary_list = new ArrayList();
        employee_temporary_list = da_gtli_employee_temporary.GetGTLIEmployeeTemporayListByPremiumID(premium_temporay_id);

        //loop through employee temporary list
        for (int i = 0; i <= employee_temporary_list.Count - 1; i++)
            {
                bl_gtli_employee employee_temporary = new bl_gtli_employee();
                employee_temporary = (bl_gtli_employee)employee_temporary_list[i];

                //set status to 0
                da_gtli_employee.UpdateStatus(employee_temporary.GTLI_Certificate_ID, 0);

                //Get gtli employee premium list by this employee
                List<bl_gtli_employee_premium> employee_premium_list = new List<bl_gtli_employee_premium>();

                employee_premium_list = da_gtli_employee_premium_temporary.GetPremiumList(employee_temporary.GTLI_Certificate_ID, premium_temporay_id);

                for (int j = 0; j <= employee_premium_list.Count - 1; j++)
                {
                    bl_gtli_employee_premium employee_premium = new bl_gtli_employee_premium();
                    employee_premium = employee_premium_list[j];

                    employee_premium.GTLI_Employee_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_ID", "@GTLI_Employee_Premium_ID");
                    employee_premium.GTLI_Certificate_ID = employee_temporary.GTLI_Certificate_ID;
                    employee_premium.GTLI_Premium_ID = premium_temporary.GTLI_Premium_ID;

                    //Insert employee premium
                    if (!da_gtli_employee_premium.InsertEmployeePremium(employee_premium))
                    { //Failed Employee Premium
                        //delete                                          
                        da_gtli_employee_premium.DeleteEmployeePremium(premium_temporary.GTLI_Premium_ID);
                        da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                        return;
                    }
                } //end loop employee premium temporary list
            }


        //Insert gtli prem return

        bl_gtli_policy_prem_return prem_return = new bl_gtli_policy_prem_return();
        prem_return = da_gtli_policy_prem_return_temporary.GetGTLPolicyPremReturnByGTLIPremiumID(premium_temporay_id);
        prem_return.GTLI_Premium_ID = premium_temporary.GTLI_Premium_ID;
        prem_return.GTLI_Policy_Prem_Return_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
        prem_return.Created_On = DateTime.Now;      
      
        if (!da_gtli_policy_prem_return.InsertPolicyPremReturn(prem_return))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
            return;
        }else{

            //Update premium in ct_gtli_policy
            da_gtli_policy.UpdatePremium((premium_temporary.Life_Premium * (-1)), (premium_temporary.TPD_Premium * (-1)), (premium_temporary.DHC_Premium * (-1)), (premium_temporary.Accidental_100Plus_Premium * (-1)), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, premium_temporary.GTLI_Policy_ID);

            Response.Redirect("resign_member_transaction_detail.aspx?pid=" + premium_temporary.GTLI_Premium_ID);
        }

    }
}