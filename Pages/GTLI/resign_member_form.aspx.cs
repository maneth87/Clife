using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_resign_member_form : System.Web.UI.Page
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

           
            ddlSaleAgentReturn.DataSource = agent_list;
            ddlSaleAgentReturn.DataTextField = "Full_Name";
            ddlSaleAgentReturn.DataValueField = "Sale_Agent_ID";
            ddlSaleAgentReturn.DataBind();

            string this_policy_id = Request.Params["pid"];

            bl_gtli_policy this_policy = da_gtli_policy.GetGTLIPolicyByID(this_policy_id);

            string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(this_policy.GTLI_Policy_ID);

            if (policy_status != "IF")
            {
                //disable resign and add new button
               
                btnResignMember.Enabled = false;
            }
            else
            {
                //Enable resign and add new button
                btnResignMember.Enabled = true;
               
            }

            //disable button add new member and resign member if this policy is already expire
            int policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(this_policy_id);

            //get last policy year by policy number
            int last_policy_year = da_gtli_premium.GetGTLILastPolicyYearByPolicyNumber(this_policy.Policy_Number);

            if (last_policy_year > policy_year)
            {
                btnResignMember.Enabled = false;
                
            }
            else
            {
                btnResignMember.Enabled = true;
              
            }

            double life_premium = 0;
            double tpd_premium = 0;
            double dhc_premium = 0;
            double accidental_100plus_premium = 0;

            //get list of active employee for this sale transaction
            ArrayList list_of_active_employee = new ArrayList();
            list_of_active_employee = da_gtli_employee.GetListOfActiveEmployee(this_policy_id);

            ArrayList list_of_employee = new ArrayList();

            //Print Active Member Hearder
            string strTableDetailPrint = "<table class='gridtable' width='98%'>";
            strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

            dvPrintActiveMember.Controls.Add(new LiteralControl(strTableDetailPrint));

            strTableDetailPrint = "";

            //Export Excel Active Member Header
            string strTableDetailExportExcel = "<table class='gridtable' width='98%'>";
            strTableDetailExportExcel += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>Life Premium</th><th>100Plus Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";

            dvExportExcelActiveMember.Controls.Add(new LiteralControl(strTableDetailExportExcel));

            strTableDetailExportExcel = "";

            int number = 1;
            int rows = 0;

            double total_sum_insured = 0;
            double total_life_premium = this_policy.Life_Premium;
            double total_tpd_premium = this_policy.TPD_Premium;
            double total_dhc_premium = this_policy.DHC_Premium;
            double total_accidental_100plus_premium = this_policy.Accidental_100Plus_Premium;

            double total_original_accidental_100plus_premium = 0;
            double total_original_life_premium = 0;
            double total_original_tpd_premium = 0;
            double total_original_dhc_premium = 0;

            double total_accidental_100plus_premium_discount = this_policy.Accidental_100Plus_Premium_Discount;
            double total_life_premium_discount = this_policy.Life_Premium_Discount;
            double total_tpd_premium_discount = this_policy.TPD_Premium_Discount;
            double total_dhc_premium_discount = this_policy.DHC_Premium_Discount;


            //loop through active employee list
            for (int k = 0; k <= list_of_active_employee.Count - 1; k++)
            {
                #region Old code
                /*
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
                active_employee.Accidental_100Plus_Premium = accidental_100plus_premium;
                active_employee.Total_Premium = (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium);
                active_employee.TPD_Premium = tpd_premium;
                active_employee.Sum_Insured = Convert.ToDouble(myemployee.Sum_Insured);
                active_employee.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                active_employee.GTLI_Premium_ID = myemployee.GTLI_Premium_ID;

                list_of_employee.Add(active_employee);

                //table print active member

                rows += 1;

                if (rows == 49)
                {
                    strTableDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                    dvPrintActiveMember.Controls.Add(new LiteralControl(strTableDetailPrint));
                    rows = 0;
                    //New table
                    strTableDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                    strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>100Plus Premium</th><th>Life Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                }

                //Row Print Active Member
                strTableDetailPrint += "<tr>";

                strTableDetailPrint += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                strTableDetailPrint += "<td style='padding-left: 5px'>" + str_certificate_no + "</td>";
                strTableDetailPrint += "<td style='text-align: left; padding-left: 5px;'>" + active_employee.Employee_Name + "</td>";
                strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.GTLI_Plan + "</td>";
                strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.Days + "</td>";


                if (active_employee.Sum_Insured != 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Sum_Insured.ToString("C0") + "</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                if (active_employee.Life_Premium != 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Life_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";

                }

                if (active_employee.Accidental_100Plus_Premium != 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Accidental_100Plus_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                if (active_employee.TPD_Premium != 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.TPD_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                if (active_employee.DHC_Premium != 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.DHC_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }


                if (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium > 0)
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium).ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                strTableDetailPrint += "</tr>";

                dvPrintActiveMember.Controls.Add(new LiteralControl(strTableDetailPrint));

                strTableDetailPrint = "";


                //Row Export Excel Active Member
                strTableDetailExportExcel += "<tr>";

                strTableDetailExportExcel += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                strTableDetailExportExcel += "<td style='padding-left: 5px'>" + str_certificate_no + "</td>";
                strTableDetailExportExcel += "<td style='text-align: left; padding-left: 5px;'>" + active_employee.Employee_Name + "</td>";
                strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.GTLI_Plan + "</td>";
                strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.Days + "</td>";

                if (active_employee.Sum_Insured != 0)
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Sum_Insured.ToString("C0") + "</td>";
                }
                else
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                if (active_employee.Life_Premium != 0)
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Life_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                if (active_employee.Accidental_100Plus_Premium != 0)
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Accidental_100Plus_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                if (active_employee.TPD_Premium != 0)
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.TPD_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                if (active_employee.DHC_Premium != 0)
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.DHC_Premium.ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }



                if (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium > 0)
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium).ToString("C2") + "</td>";
                }
                else
                {
                    strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                }

                strTableDetailExportExcel += "</tr>";

                dvExportExcelActiveMember.Controls.Add(new LiteralControl(strTableDetailExportExcel));

                strTableDetailExportExcel = "";

                number += 1;

                //total
                total_sum_insured += active_employee.Sum_Insured;
                total_original_dhc_premium += active_employee.DHC_Premium;
                total_original_life_premium += active_employee.Life_Premium;
                total_original_tpd_premium += active_employee.TPD_Premium;
                total_original_accidental_100plus_premium += active_employee.Accidental_100Plus_Premium;
                 */
                #endregion
                bl_gtli_employee myemployee = (bl_gtli_employee)list_of_active_employee[k];
                life_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "Death");
                tpd_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "TPD");
                dhc_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "DHC");
                accidental_100plus_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "Accidental100Plus");

                if (life_premium > 0)
                {
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
                    active_employee.Accidental_100Plus_Premium = accidental_100plus_premium;
                    active_employee.Total_Premium = (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium);
                    active_employee.TPD_Premium = tpd_premium;
                    active_employee.Sum_Insured = Convert.ToDouble(myemployee.Sum_Insured);
                    active_employee.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                    active_employee.GTLI_Premium_ID = myemployee.GTLI_Premium_ID;

                    list_of_employee.Add(active_employee);

                    //table print active member

                    rows += 1;

                    if (rows == 49)
                    {
                        strTableDetailPrint = "</table><div style='display: block; page-break-before: always;'></div>";
                        dvPrintActiveMember.Controls.Add(new LiteralControl(strTableDetailPrint));
                        rows = 0;
                        //New table
                        strTableDetailPrint = "<br /><br /><br /><table class='gridtable' width='98%'>";
                        strTableDetailPrint += "<tr><th>No.</th><th style='width:70px;'>Certificate No.</th><th style='width:130px; text-align: left; padding-left: 5px;'>Employee Name</th><th>Plan</th><th style='width:80px;'>Effective Date</th><th style='text-align: center; width:80px; padding-left: 5px;'>Expiry Date</th><th style='text-align: center; width:50px; padding-left: 5px;'>Period (Days)</th><th>Life Coverage (USD)</th><th>100Plus Premium</th><th>Life Premium</th><th>TPD Premium</th><th>DHC Premium</th><th>Total Premium</th></tr>";
                    }

                    //Row Print Active Member
                    strTableDetailPrint += "<tr>";

                    strTableDetailPrint += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                    strTableDetailPrint += "<td style='padding-left: 5px'>" + str_certificate_no + "</td>";
                    strTableDetailPrint += "<td style='text-align: left; padding-left: 5px;'>" + active_employee.Employee_Name + "</td>";
                    strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.GTLI_Plan + "</td>";
                    strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                    strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                    strTableDetailPrint += "<td style='text-align: center;'>" + active_employee.Days + "</td>";


                    if (active_employee.Sum_Insured != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Sum_Insured.ToString("C0") + "</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (active_employee.Life_Premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Life_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";

                    }

                    if (active_employee.Accidental_100Plus_Premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Accidental_100Plus_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (active_employee.TPD_Premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.TPD_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (active_employee.DHC_Premium != 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.DHC_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }


                    if (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium > 0)
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'>" + (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium).ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailPrint += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    strTableDetailPrint += "</tr>";

                    dvPrintActiveMember.Controls.Add(new LiteralControl(strTableDetailPrint));

                    strTableDetailPrint = "";


                    //Row Export Excel Active Member
                    strTableDetailExportExcel += "<tr>";

                    strTableDetailExportExcel += "<td style='text-align: center;' width='30px'>" + (number).ToString() + "</td>";
                    strTableDetailExportExcel += "<td style='padding-left: 5px'>" + str_certificate_no + "</td>";
                    strTableDetailExportExcel += "<td style='text-align: left; padding-left: 5px;'>" + active_employee.Employee_Name + "</td>";
                    strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.GTLI_Plan + "</td>";
                    strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.Effective_Date.ToString("d-MMM-yyyy") + "</td>";
                    strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.Expiry_Date.ToString("d-MMM-yyyy") + "</td>";
                    strTableDetailExportExcel += "<td style='text-align: center;'>" + active_employee.Days + "</td>";

                    if (active_employee.Sum_Insured != 0)
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Sum_Insured.ToString("C0") + "</td>";
                    }
                    else
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (active_employee.Life_Premium != 0)
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Life_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (active_employee.Accidental_100Plus_Premium != 0)
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.Accidental_100Plus_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (active_employee.TPD_Premium != 0)
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.TPD_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    if (active_employee.DHC_Premium != 0)
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + active_employee.DHC_Premium.ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }



                    if (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium > 0)
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'>" + (active_employee.Life_Premium + active_employee.TPD_Premium + active_employee.DHC_Premium + active_employee.Accidental_100Plus_Premium).ToString("C2") + "</td>";
                    }
                    else
                    {
                        strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px;'> - </td>";
                    }

                    strTableDetailExportExcel += "</tr>";

                    dvExportExcelActiveMember.Controls.Add(new LiteralControl(strTableDetailExportExcel));

                    strTableDetailExportExcel = "";

                    number += 1;

                    //total
                    total_sum_insured += active_employee.Sum_Insured;
                    total_original_dhc_premium += active_employee.DHC_Premium;
                    total_original_life_premium += active_employee.Life_Premium;
                    total_original_tpd_premium += active_employee.TPD_Premium;
                    total_original_accidental_100plus_premium += active_employee.Accidental_100Plus_Premium;
                }

                
               
            }

            gvActiveMember.DataSource = list_of_employee;
            gvActiveMember.DataBind();

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



            if (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium> 0)
            {
                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2") + "</td>";
            }
            else
            {
                strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
            }

            strTableDetailPrint += "</table>";

            dvPrintActiveMember.Controls.Add(new LiteralControl(strTableDetailPrint));

            //Total Print Row       
            strTableDetailExportExcel = "<tr>";
            strTableDetailExportExcel += "<td></td>";
            strTableDetailExportExcel += "<td></td>";
            strTableDetailExportExcel += "<td></td>";
            strTableDetailExportExcel += "<td></td>";
            strTableDetailExportExcel += "<td></td>";
            strTableDetailExportExcel += "<td></td>";
            strTableDetailExportExcel += "<td style='text-align: center; font-weight: bold;'>Total:</td>";
            strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_sum_insured.ToString("C0") + "</td>";


            if (total_original_life_premium > 0)
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_life_premium.ToString("C2") + "</td>";
            }
            else
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
            }


            if (total_original_accidental_100plus_premium > 0)
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_accidental_100plus_premium.ToString("C2") + "</td>";
            }
            else
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
            }

            if (total_original_tpd_premium > 0)
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_tpd_premium.ToString("C2") + "</td>";
            }
            else
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
            }


            if (total_original_dhc_premium > 0)
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + total_original_dhc_premium.ToString("C2") + "</td>";
            }
            else
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
            }



            if (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium> 0)
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold; text-decoration: underline;'>" + (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2") + "</td>";
            }
            else
            {
                strTableDetailExportExcel += "<td style='text-align: right; padding-right: 6px; font-weight: bold;'> - </td>";
            }

            strTableDetailExportExcel += "</table>";

            dvExportExcelActiveMember.Controls.Add(new LiteralControl(strTableDetailExportExcel));



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

            

                ddlPlan2.Items.Add(item);
                ddlPlan2.AppendDataBoundItems = true;
                ddlPlan2.DataSource = plan_list;
                ddlPlan2.DataTextField = "GTLI_Plan";
                ddlPlan2.DataValueField = "GTLI_Plan_ID";
                ddlPlan2.DataBind();
            }
            else
            {
              
                ddlPlan2.Items.Clear();

            }


            //hide upload div
            upload.Style.Clear();
            upload.Style.Add("display", "none");

            gvActiveMember.Style.Clear();
            gvActiveMember.Style.Add("display", "box");

            //set ddlOption attribute
            ddlOption.Attributes.Add("onChange", "return OnSelectedIndexChange();");

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

        double total_original_life_premium = 0;
        double total_original_accidental_100plus_premium = 0;
        double total_original_tpd_premium = 0;
        double total_original_dhc_premium = 0;

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

            double my_sum_insured = 0; //sum_insured for display

            TimeSpan mytimespan = premium.Expiry_Date.Subtract(premium.Effective_Date);
            int days = mytimespan.Days + 1;

            ArrayList list_employee = new ArrayList();
            list_employee = da_gtli_employee.GetListOfEmployeeByGTLIPremiumID(premium.GTLI_Premium_ID);


            //loop through employee list
            for (int k = 0; k <= list_employee.Count - 1; k++)
            {
                #region Old code
                /*
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

                        //if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0)
                        //{
                        //    total_sum_insured += 0;
                        //    my_sum_insured = 0;
                        //}
                        //else
                        //{
                        //    total_sum_insured += premium.Sum_Insured;
                        //    my_sum_insured = premium.Sum_Insured;
                        //}


                        if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0)
                        {
                            total_sum_insured += 0;
                            my_sum_insured = 0;
                        }
                        else
                        {
                            total_sum_insured += employee_sum_insure;
                            my_sum_insured = employee_sum_insure;
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

                            //if (my_sum_insured != 0)
                            //{
                            //    tc_sum_insured.InnerText = premium.Sum_Insured.ToString("C0");

                            //}
                            //else
                            //{
                            //    tc_sum_insured.InnerText = " - ";

                            //}

                            if (my_sum_insured != 0)
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
                            //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + premium.Sum_Insured.ToString("C0") + "</td>";
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

                            //if (my_sum_insured != 0)
                            //{
                            //    tc_sum_insured.InnerText = premium.Sum_Insured.ToString("C0");
                            //}
                            //else
                            //{
                            //    tc_sum_insured.InnerText = " - ";
                            //}

                            if (my_sum_insured != 0)
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
                                //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + premium.Sum_Insured.ToString("C0") + "</td>";
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

                        //if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                        //{
                        //    total_sum_insured -= 0;
                        //    my_sum_insured = 0;
                        //}
                        //else
                        //{
                        //    total_sum_insured -= plan_resign.Sum_Insured;
                        //    my_sum_insured = plan_resign.Sum_Insured;
                        //}


                        if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                        {
                            total_sum_insured -= 0;
                            my_sum_insured = 0;
                        }
                        else
                        {
                            total_sum_insured -= employee_sum_insure;
                            my_sum_insured = employee_sum_insure;
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

                        //if (my_sum_insured != 0)
                        //{
                        //    tc_sum_insured_resign.InnerText = plan_resign.Sum_Insured.ToString("C0");
                        //}
                        //else
                        //{
                        //    tc_sum_insured_resign.InnerText = " - ";
                        //}

                        if (my_sum_insured != 0)
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
                            //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + plan_resign.Sum_Insured.ToString("C0") + "</td>";
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
                    */
                #endregion Old code
                #region New code by Maneth
                bl_gtli_employee employee = new bl_gtli_employee();
                employee = (bl_gtli_employee)list_employee[k];

                life_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "Death");
                tpd_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "TPD");
                dhc_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "DHC");
                accidental_100plus_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(employee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "Accidental100Plus");
                if(life_premium>0)
                {
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

                        //if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0)
                        //{
                        //    total_sum_insured += 0;
                        //    my_sum_insured = 0;
                        //}
                        //else
                        //{
                        //    total_sum_insured += premium.Sum_Insured;
                        //    my_sum_insured = premium.Sum_Insured;
                        //}


                        if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0)
                        {
                            total_sum_insured += 0;
                            my_sum_insured = 0;
                        }
                        else
                        {
                            total_sum_insured += employee_sum_insure;
                            my_sum_insured = employee_sum_insure;
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

                            //if (my_sum_insured != 0)
                            //{
                            //    tc_sum_insured.InnerText = premium.Sum_Insured.ToString("C0");

                            //}
                            //else
                            //{
                            //    tc_sum_insured.InnerText = " - ";

                            //}

                            if (my_sum_insured != 0)
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
                            //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: green'>" + premium.Sum_Insured.ToString("C0") + "</td>";
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

                            //if (my_sum_insured != 0)
                            //{
                            //    tc_sum_insured.InnerText = premium.Sum_Insured.ToString("C0");
                            //}
                            //else
                            //{
                            //    tc_sum_insured.InnerText = " - ";
                            //}

                            if (my_sum_insured != 0)
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
                                //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: black'>" + premium.Sum_Insured.ToString("C0") + "</td>";
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

                        //if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                        //{
                        //    total_sum_insured -= 0;
                        //    my_sum_insured = 0;
                        //}
                        //else
                        //{
                        //    total_sum_insured -= plan_resign.Sum_Insured;
                        //    my_sum_insured = plan_resign.Sum_Insured;
                        //}


                        if (life_premium == 0 && tpd_premium == 0 && dhc_premium == 0 && accidental_100plus_premium == 0)
                        {
                            total_sum_insured -= 0;
                            my_sum_insured = 0;
                        }
                        else
                        {
                            total_sum_insured -= employee_sum_insure;
                            my_sum_insured = employee_sum_insure;
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

                        //if (my_sum_insured != 0)
                        //{
                        //    tc_sum_insured_resign.InnerText = plan_resign.Sum_Insured.ToString("C0");
                        //}
                        //else
                        //{
                        //    tc_sum_insured_resign.InnerText = " - ";
                        //}

                        if (my_sum_insured != 0)
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
                            //strTableDetailPrint += "<td style='text-align: right; padding-right: 6px; color: red'>" + plan_resign.Sum_Insured.ToString("C0") + "</td>";
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
                }
                



                } //end switch premium type    
                #endregion New code by Maneth

            } //end loop employee list



        } //end loop premium list

        lblPolicyNumber.Text = policy.Policy_Number;
        lblCreatedDate.Text = policy.Created_On.ToString("d-MMM-yyyy");
        lblAgent.Text = agentname;

        lblCompanyName.Text = company.Company_Name;
        lblTypeOfBusiness.Text = company.Type_Of_Business;
        lblContactName.Text = contact.Contact_Name;
        lblPhone.Text = contact.Contact_Phone;
        lblEmail.Text = contact.Contact_Email;
        lblAddress.Text = company.Company_Address;
        lblSumInsured.Text = total_sum_insured.ToString("C0");

        lblPremiumPayment.Text = (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2");
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

        lblPremiumPayment2.Text = (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium + total_original_accidental_100plus_premium).ToString("C2");
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

        if (total_original_accidental_100plus_premium > 0)
        {
            tc_accidental_100plus_premium_total.Style.Add("text-decoration", "underline");
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

        if (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium > 0)
        {
            tc_total_premium.Style.Add("text-decoration", "underline");
            tc_total_premium.InnerText = (total_original_life_premium + total_original_tpd_premium + total_original_dhc_premium).ToString("C2");
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

    //Resign Member Click
    protected void btnResignMember_Click(object sender, EventArgs e)
    {
        string channel_location_id = "0D696111-2590-4FA2-BCE6-C8B2D46648C9"; //Camlife HQ
        string channel_channel_item_id = "016AE1FC-CF77-461A-92F8-6C7605D0A648";

        double total_sum_insured = 0;

        string this_policy_id = Request.QueryString.Get(0);

        bl_gtli_policy this_policy = da_gtli_policy.GetGTLIPolicyByID(this_policy_id);

        int policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(this_policy_id);

        //get company
        bl_gtli_company company = da_gtli_company.GetObjCompanyByID(this_policy.GTLI_Company_ID);

        bl_gtli_plan plan = new bl_gtli_plan();
        plan = da_gtli_plan.GetPlan(ddlPlan2.SelectedValue);

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
        dtfi2.ShortDatePattern = "MM/dd/yyyy";
        dtfi2.DateSeparator = "/";

        //check date of removed format
        System.DateTime date_of_removed = default(System.DateTime);

        if (!Helper.CheckDateFormat(txtResignDate.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Date of removal invalid.')", true);
            return;
        }
        else
        {
            date_of_removed = Convert.ToDateTime(txtResignDate.Text, dtfi);

        }        

        double total_premium_return_death = 0;
        double total_premium_return_tpd = 0;
        double total_premium_return_dhc = 0;
        double total_premium_return_accidental_100plus = 0;

        //get total days
        int total_days = 365;

        if (ddlOption.SelectedValue == "0")
        {
            //Upload file
            //check if file upload contain any file
            if ((uploadedDoc2.PostedFile != null) & !string.IsNullOrEmpty(uploadedDoc2.PostedFile.FileName))
            {
                //get plan by plan_id
                bl_gtli_plan myplan = da_gtli_plan.GetPlan(ddlPlan2.SelectedValue);

                string savePath = "~/Upload/GTLI/CustomerResignData/";
                dynamic postedFile = uploadedDoc2.PostedFile;
                string filename = Path.GetFileName(postedFile.FileName);
                string contentType = postedFile.ContentType;
                int contentLength = postedFile.ContentLength;
                string filepath = null;

                postedFile.SaveAs(Server.MapPath(savePath + filename));

                string version = Path.GetExtension(filename);
                filepath = Server.MapPath(savePath + filename).ToString();


                if (version == ".xls")
                {
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

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessageResign.Text = "Please check your input of employee name field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessageResign.Text = "Please check your input for gender field then try again. Row number: " + (i + 2) + "";
                            return;
                        }


                        if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                        {
                            lblMessageResign.Text = "Please check your input for date field then try again. Row number: " + (i + 2) + "";
                            return;
                        }
                    }


                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //check employee by company_id, plan_id, sale_group_number,  employee_name, gender, dob
                        if (!da_gtli_employee.CheckGTLIEmployeeForResign(dt.Rows[i][1].ToString().Trim(), dt.Rows[i][2].ToString().Trim(), Convert.ToDateTime(dt.Rows[i][3], dtfi), this_policy.GTLI_Company_ID, myplan.GTLI_Plan_ID, this_policy.GTLI_Policy_ID))
                        {
                            //not found employee in this plan
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Employee is not in this plan. Please check again. Row number: " + (i + 2) + "')", true);

                            return;
                        }

                    }

                    //GTLI Premium
                    bl_gtli_premium gtli_premium = new bl_gtli_premium();
                    gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                    gtli_premium.Channel_Location_ID = channel_location_id;
                    gtli_premium.Created_By = hdfusername.Value;
                    gtli_premium.Created_On = DateTime.Now;
                    gtli_premium.DHC_Option_Value = 0;
                    gtli_premium.DHC_Premium = 0;
                    gtli_premium.Effective_Date = date_of_removed;
                    gtli_premium.Expiry_Date = this_policy.Expiry_Date;
                    gtli_premium.GTLI_Plan_ID = "0";
                    gtli_premium.GTLI_Policy_ID = this_policy.GTLI_Policy_ID;
                    gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                    gtli_premium.Life_Premium = 0;
                    gtli_premium.Accidental_100Plus_Premium = 0;
                    gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                    gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                    gtli_premium.DHC_Premium_Discount = 0;
                    gtli_premium.DHC_Premium_Tax_Amount = 0;
                    gtli_premium.Life_Premium_Discount = 0;
                    gtli_premium.Life_Premium_Tax_Amount = 0;
                    gtli_premium.Original_Accidental_100Plus_Premium = 0;
                    gtli_premium.Original_DHC_Premium = 0;
                    gtli_premium.Original_Life_Premium = 0;
                    gtli_premium.Original_TPD_Premium = 0;
                    gtli_premium.TPD_Premium_Discount = 0;
                    gtli_premium.TPD_Premium_Tax_Amount = 0;
                    gtli_premium.Pay_Mode_ID = 1;
                    gtli_premium.Policy_Year = policy_year;
                    gtli_premium.Sale_Agent_ID = ddlSaleAgentReturn.SelectedValue;
                    //gtli_premium.Sum_Insured = plan.Sum_Insured * dt.Rows.Count;
                   // gtli_premium.Sum_Insured = plan.Sum_Insured;
                    gtli_premium.TPD_Premium = 0;
                    gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                    gtli_premium.User_Total_Staff_Number = 0;
                    gtli_premium.Discount = 0;

                    gtli_premium.Transaction_Type = 3;


                    //if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign gtli employee failed. Please check your inputs again.')", true);
                    //    return;

                    //}

                  
                    //Insert all resign employee in upload file for this plan 
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //get employee
                        bl_gtli_employee myemployee = da_gtli_employee.GetGTLIEmployeeForResign(dt.Rows[i][1].ToString().Trim(), dt.Rows[i][2].ToString().Trim(), Convert.ToDateTime(dt.Rows[i][3], dtfi2), this_policy.GTLI_Company_ID, ddlPlan2.SelectedValue, this_policy.GTLI_Policy_ID);

                        //get certificate
                        bl_gtli_certificate mycertificate = da_gtli_certificate.GetGTLICertificateByID(myemployee.GTLI_Certificate_ID);

                        //get employee premium life_premium, tpd_premium, dhc_premium
                        decimal life_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "Death"));
                        decimal tpd_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "TPD"));
                        decimal hdc_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "DHC"));
                        decimal accidental_100plus_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "Accidental100plus"));

                        //get premium obj of this employee
                        bl_gtli_premium premium = da_gtli_premium.GetGTLPremiumByCerficateID(mycertificate.GTLI_Certificate_ID);

                        double employee_sum_insure = da_gtli_employee_premium.GetSumInsuredByGTLICertificateID(myemployee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "Death");

                        total_sum_insured += employee_sum_insure;

                        System.DateTime add_date = premium.Effective_Date;

                        int used_days = 0;

                        //Get Number of Days Used
                        used_days = CalculateUsedDays(add_date, date_of_removed);
                                              
                        //Find Premium Return

                        decimal premium_charged_Death = 0;
                        decimal premium_return_Death = 0;

                        if(premium.Discount > 0){
                            premium_charged_Death  = ((life_premium - (Math.Floor((life_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                            premium_return_Death = (life_premium - (Math.Floor((life_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_Death);
                        }
                        else{
                            premium_charged_Death = (life_premium * used_days) / total_days;
                            premium_return_Death = life_premium  - Math.Round(premium_charged_Death);
                        }
                                               

                        //Insert Certficate temporary
                        //Column 0 = employee ID; 1 = employee name; 2 = gender; 3 = dob; 4 = position                           

                        DateTime dob = Convert.ToDateTime(dt.Rows[i][3].ToString().Trim(), dtfi2);

                        //Insert Certificate   
                        if (!da_gtli_certificate_temporary.InsertCertificateTemporary(mycertificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        //Delete employee temporary by certificate id
                        da_gtli_employee_temporary.DeleteGTLIEmployeeTemporaryByCertificateID(myemployee.GTLI_Certificate_ID);

                        //Insert Employee
                        myemployee.GTLI_Certificate_ID = mycertificate.GTLI_Certificate_ID;

                        if (!da_gtli_employee_temporary.InsertEmployeeTemporary(myemployee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        //Insert employee premium life
                        string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                        employee_premium_life.Premium_Type = "Death";
                        employee_premium_life.Premium = premium_return_Death.ToString();
                        employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                        employee_premium_life.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        total_premium_return_death += Convert.ToDouble(premium_return_Death);

                        decimal premium_charged_TPD = default(decimal);
                        decimal premium_charged_DHC = default(decimal);
                        decimal premium_return_TPD = default(decimal);
                        decimal premium_return_DHC = default(decimal);
                        decimal premium_charged_accidental_100plus = default(decimal);
                        decimal premium_return_accidental_100plus = default(decimal);

                        //Insert employee premium accidental 100plus
                        if (accidental_100plus_premium != 0 | Convert.ToDouble(accidental_100plus_premium) != 0.0)
                        {
                            if (premium.Discount > 0)
                            {
                                premium_charged_accidental_100plus = ((accidental_100plus_premium - (Math.Floor((accidental_100plus_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_accidental_100plus = (accidental_100plus_premium - (Math.Floor((accidental_100plus_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_accidental_100plus);
                            }
                            else
                            {
                                premium_charged_accidental_100plus = (accidental_100plus_premium * used_days) / total_days;
                                premium_return_accidental_100plus = accidental_100plus_premium - Math.Round(premium_charged_accidental_100plus);
                            }                                                       
                           

                            string gtli_employee_premium_temporary_id_accidental_100plus = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium accidental 100plus
                            bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                            employee_premium_accidental_100plus.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                            employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_accidental_100plus;
                            employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_accidental_100plus.Premium = premium_return_TPD.ToString();
                            employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_accidental_100plus += Convert.ToDouble(premium_return_accidental_100plus);
                        }

                        //Insert employee premium tpd
                        if (tpd_premium != 0 | Convert.ToDouble(tpd_premium) != 0.0)
                        {
                            if (premium.Discount > 0)
                            {
                                
                                premium_charged_TPD = ((tpd_premium - (Math.Floor((tpd_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_TPD = (tpd_premium - (Math.Floor((tpd_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_TPD);
                            }
                            else
                            {
                                premium_charged_TPD = (tpd_premium * used_days) / total_days;
                                premium_return_TPD = tpd_premium - Math.Round(premium_charged_TPD);
                            }                          
                            

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                            employee_premium_tpd.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                            employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_tpd.Premium = premium_return_TPD.ToString();
                            employee_premium_tpd.Premium_Type = "TPD";

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_tpd += Convert.ToDouble(premium_return_TPD);
                        }

                        //Insert employee premium dhc
                        if (hdc_premium != 0 | Convert.ToDouble(hdc_premium) != 0.0)
                        {
                            if (premium.Discount > 0)
                            {
                                premium_charged_DHC = ((hdc_premium - (Math.Floor((hdc_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_DHC = (hdc_premium - (Math.Floor((hdc_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_DHC);
                            }
                            else
                            {
                                premium_charged_DHC = (hdc_premium * used_days) / total_days;
                                premium_return_DHC = hdc_premium - Math.Round(premium_charged_DHC);
                            }
                          

                            string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                            employee_premium_dhc.Premium_Type = "DHC";
                            employee_premium_dhc.Premium = premium_return_DHC.ToString();
                            employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                            employee_premium_dhc.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_dhc += Convert.ToDouble(premium_return_DHC);
                        }



                    }

                   //total sum insured
                    gtli_premium.Sum_Insured = total_sum_insured;

                    if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign gtli employee failed. Please check your inputs again.')", true);
                        return;

                    }

                    //Insert gtli prem return

                    bl_gtli_policy_prem_return prem_return = new bl_gtli_policy_prem_return();
                    prem_return.Sale_Agent_ID = ddlSaleAgentReturn.SelectedValue;
                    prem_return.Prem_Year = policy_year;
                    prem_return.Prem_Lot = 1;
                    prem_return.Pay_Mode_ID = 1;
                    prem_return.Return_Date = date_of_removed;
                    prem_return.Office_ID = channel_location_id;
                    prem_return.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                    prem_return.GTLI_Policy_Prem_Return_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                    prem_return.Created_On = DateTime.Now;
                    prem_return.Created_Note = "";
                    prem_return.Created_By = hdfusername.Value;
                    prem_return.Amount = Convert.ToDouble(total_premium_return_death + total_premium_return_dhc + total_premium_return_tpd + total_premium_return_accidental_100plus);
                    prem_return.Payment_Code = "";

                    if (!da_gtli_policy_prem_return_temporary.InsertPolicyPremReturnTemporary(prem_return, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                        return;
                    }

                    //update total premium in premium temporary
                    da_gtli_premium_temporary.UpdatePremium(total_sum_insured,Convert.ToDouble(total_premium_return_death), Convert.ToDouble(total_premium_return_tpd), Convert.ToDouble(total_premium_return_dhc), Convert.ToDouble(total_premium_return_accidental_100plus), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, gtli_premium.GTLI_Premium_ID);
                    da_gtli_premium_temporary.UpdateSumInsured(total_sum_insured, gtli_premium.GTLI_Premium_ID);

                    Response.Redirect("resign_member_transaction_underwrite_detail.aspx?pid=" + gtli_premium.GTLI_Premium_ID);

                }
                else if (version == ".xlsx")
                {
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

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessageResign.Text = "Please check your input of employee name field then try again. Row number: " + (i + 2) + "";
                            return;
                        }

                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessageResign.Text = "Please check your input for gender field then try again. Row number: " + (i + 2) + "";
                            return;
                        }


                        if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                        {
                            lblMessageResign.Text = "Please check your input for date field then try again. Row number: " + (i + 2) + "";
                            return;
                        }
                    }


                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //check employee by company_id, plan_id, sale_group_number,  employee_name, gender, dob
                        if (!da_gtli_employee.CheckGTLIEmployeeForResign(dt.Rows[i][1].ToString().Trim(), dt.Rows[i][2].ToString().Trim(), Convert.ToDateTime(dt.Rows[i][3], dtfi), this_policy.GTLI_Company_ID, myplan.GTLI_Plan_ID, this_policy.GTLI_Policy_ID))
                        {
                            //not found employee in this plan
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Employee is not in this plan. Please check again. Row number: " + (i + 2) + "')", true);
                            return;
                        }

                    }

                    //GTLI Premium
                    bl_gtli_premium gtli_premium = new bl_gtli_premium();
                    gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                    gtli_premium.Channel_Location_ID = channel_location_id;
                    gtli_premium.Created_By = hdfusername.Value;
                    gtli_premium.Created_On = DateTime.Now;
                    gtli_premium.DHC_Option_Value = 0;
                    gtli_premium.DHC_Premium = 0;
                    gtli_premium.Effective_Date = date_of_removed;
                    gtli_premium.Expiry_Date = this_policy.Expiry_Date;
                    gtli_premium.GTLI_Plan_ID = "0";
                    gtli_premium.GTLI_Policy_ID = this_policy.GTLI_Policy_ID;
                    gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                    gtli_premium.Life_Premium = 0;
                    gtli_premium.Accidental_100Plus_Premium = 0;
                    gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                    gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                    gtli_premium.DHC_Premium_Discount = 0;
                    gtli_premium.DHC_Premium_Tax_Amount = 0;
                    gtli_premium.Life_Premium_Discount = 0;
                    gtli_premium.Life_Premium_Tax_Amount = 0;
                    gtli_premium.Original_Accidental_100Plus_Premium = 0;
                    gtli_premium.Original_DHC_Premium = 0;
                    gtli_premium.Original_Life_Premium = 0;
                    gtli_premium.Original_TPD_Premium = 0;
                    gtli_premium.TPD_Premium_Discount = 0;
                    gtli_premium.TPD_Premium_Tax_Amount = 0;
                    gtli_premium.Pay_Mode_ID = 1;
                    gtli_premium.Policy_Year = policy_year;
                    gtli_premium.Sale_Agent_ID = ddlSaleAgentReturn.SelectedValue;
                    //gtli_premium.Sum_Insured = plan.Sum_Insured * dt.Rows.Count;
                    gtli_premium.TPD_Premium = 0;
                    gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                    gtli_premium.User_Total_Staff_Number = 0;
                    gtli_premium.Discount = 0;
                    gtli_premium.Transaction_Type = 3;


                    //if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign gtli employee failed. Please check your inputs again.')", true);
                    //    return;

                    //}

                    //Insert all resign employee in upload file for this plan 
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //get employee
                        bl_gtli_employee myemployee = da_gtli_employee.GetGTLIEmployeeForResign(dt.Rows[i][1].ToString().Trim(), dt.Rows[i][2].ToString().Trim(), Convert.ToDateTime(dt.Rows[i][3], dtfi2), this_policy.GTLI_Company_ID, ddlPlan2.SelectedValue, this_policy.GTLI_Policy_ID);

                        //get certificate
                        bl_gtli_certificate mycertificate = da_gtli_certificate.GetGTLICertificateByID(myemployee.GTLI_Certificate_ID);

                        //get employee premium life_premium, tpd_premium, dhc_premium
                        decimal life_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "Death"));
                        decimal tpd_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "TPD"));
                        decimal hdc_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "DHC"));
                        decimal accidental_100plus_premium = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumOfResignEmployeeByGTLICertificateID(mycertificate.GTLI_Certificate_ID, "Accidental100plus"));

                        //get premium obj of this employee
                        bl_gtli_premium premium = da_gtli_premium.GetGTLPremiumByCerficateID(mycertificate.GTLI_Certificate_ID);

                        double employee_sum_insure = da_gtli_employee_premium.GetSumInsuredByGTLICertificateID(myemployee.GTLI_Certificate_ID, premium.GTLI_Premium_ID, "Death");

                        total_sum_insured += employee_sum_insure;

                        System.DateTime add_date = premium.Effective_Date;

                        int used_days = 0;

                        //Get Number of Days Used
                        used_days = CalculateUsedDays(add_date, date_of_removed);

                        //Find Premium Return

                        decimal premium_charged_Death = 0;
                        decimal premium_return_Death = 0;

                        if (premium.Discount > 0)
                        {
                            premium_charged_Death = ((life_premium - (Math.Floor((life_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                            premium_return_Death = (life_premium - (Math.Floor((life_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_Death);
                        }
                        else
                        {
                            premium_charged_Death = (life_premium * used_days) / total_days;
                            premium_return_Death = life_premium - Math.Round(premium_charged_Death);
                        }


                        //Insert Certficate temporary
                        //Column 0 = employee ID; 1 = employee name; 2 = gender; 3 = dob; 4 = position                           

                        DateTime dob = Convert.ToDateTime(dt.Rows[i][3].ToString().Trim(), dtfi2);

                        //Insert Certificate   
                        if (!da_gtli_certificate_temporary.InsertCertificateTemporary(mycertificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        //Delete employee temporary by certificate id
                        da_gtli_employee_temporary.DeleteGTLIEmployeeTemporaryByCertificateID(myemployee.GTLI_Certificate_ID);

                        //Insert Employee
                        myemployee.GTLI_Certificate_ID = mycertificate.GTLI_Certificate_ID;

                        if (!da_gtli_employee_temporary.InsertEmployeeTemporary(myemployee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        //Insert employee premium life
                        string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                        employee_premium_life.Premium_Type = "Death";
                        employee_premium_life.Premium = premium_return_Death.ToString();
                        employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                        employee_premium_life.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        total_premium_return_death += Convert.ToDouble(premium_return_Death);

                        decimal premium_charged_TPD = default(decimal);
                        decimal premium_charged_DHC = default(decimal);
                        decimal premium_return_TPD = default(decimal);
                        decimal premium_return_DHC = default(decimal);
                        decimal premium_charged_accidental_100plus = default(decimal);
                        decimal premium_return_accidental_100plus = default(decimal);

                        //Insert employee premium accidental 100plus
                        if (accidental_100plus_premium != 0 | Convert.ToDouble(accidental_100plus_premium) != 0.0)
                        {
                            if (premium.Discount > 0)
                            {
                                premium_charged_accidental_100plus = ((accidental_100plus_premium - (Math.Floor((accidental_100plus_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_accidental_100plus = (accidental_100plus_premium - (Math.Floor((accidental_100plus_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_accidental_100plus);
                            }
                            else
                            {
                                premium_charged_accidental_100plus = (accidental_100plus_premium * used_days) / total_days;
                                premium_return_accidental_100plus = accidental_100plus_premium - Math.Round(premium_charged_accidental_100plus);
                            }


                            string gtli_employee_premium_temporary_id_accidental_100plus = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium accidental 100plus
                            bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                            employee_premium_accidental_100plus.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                            employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_accidental_100plus;
                            employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_accidental_100plus.Premium = premium_return_TPD.ToString();
                            employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_accidental_100plus += Convert.ToDouble(premium_return_accidental_100plus);
                        }

                        //Insert employee premium tpd
                        if (tpd_premium != 0 | Convert.ToDouble(tpd_premium) != 0.0)
                        {
                            if (premium.Discount > 0)
                            {
                                premium_charged_TPD = ((tpd_premium - (Math.Floor((tpd_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_TPD = (tpd_premium - (Math.Floor((tpd_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_TPD);
                            }
                            else
                            {
                                premium_charged_TPD = (tpd_premium * used_days) / total_days;
                                premium_return_TPD = tpd_premium - Math.Round(premium_charged_TPD);
                            }


                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                            employee_premium_tpd.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                            employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_tpd.Premium = premium_return_TPD.ToString();
                            employee_premium_tpd.Premium_Type = "TPD";

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_tpd += Convert.ToDouble(premium_return_TPD);
                        }

                        //Insert employee premium dhc
                        if (hdc_premium != 0 | Convert.ToDouble(hdc_premium) != 0.0)
                        {
                            if (premium.Discount > 0)
                            {
                                premium_charged_DHC = ((hdc_premium - (Math.Floor((hdc_premium * Convert.ToDecimal(premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_DHC = (hdc_premium - (Math.Floor((hdc_premium * Convert.ToDecimal(premium.Discount)) / 100))) - Math.Round(premium_charged_DHC);
                            }
                            else
                            {
                                premium_charged_DHC = (hdc_premium * used_days) / total_days;
                                premium_return_DHC = hdc_premium - Math.Round(premium_charged_DHC);
                            }


                            string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                            employee_premium_dhc.Premium_Type = "DHC";
                            employee_premium_dhc.Premium = premium_return_DHC.ToString();
                            employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                            employee_premium_dhc.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_dhc += Convert.ToDouble(premium_return_DHC);
                        }



                    }

                    //total sum insured
                    gtli_premium.Sum_Insured = total_sum_insured;

                    if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign gtli employee failed. Please check your inputs again.')", true);
                        return;

                    }


                    //Insert gtli prem return

                    bl_gtli_policy_prem_return prem_return = new bl_gtli_policy_prem_return();
                    prem_return.Sale_Agent_ID = ddlSaleAgentReturn.SelectedValue;
                    prem_return.Prem_Year = policy_year;
                    prem_return.Prem_Lot = 1;
                    prem_return.Pay_Mode_ID = 1;
                    prem_return.Return_Date = date_of_removed;
                    prem_return.Office_ID = channel_location_id;
                    prem_return.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                    prem_return.GTLI_Policy_Prem_Return_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                    prem_return.Created_On = DateTime.Now;
                    prem_return.Created_Note = "";
                    prem_return.Created_By = hdfusername.Value;
                    prem_return.Amount = Convert.ToDouble(total_premium_return_death + total_premium_return_dhc + total_premium_return_tpd + total_premium_return_accidental_100plus);
                    prem_return.Payment_Code = "";

                    if (!da_gtli_policy_prem_return_temporary.InsertPolicyPremReturnTemporary(prem_return, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                        return;
                    }

                    //update total premium in premium temporary
                    da_gtli_premium_temporary.UpdatePremium(total_sum_insured, Convert.ToDouble(total_premium_return_death), Convert.ToDouble(total_premium_return_tpd), Convert.ToDouble(total_premium_return_dhc), Convert.ToDouble(total_premium_return_accidental_100plus), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, gtli_premium.GTLI_Premium_ID);
                    da_gtli_premium_temporary.UpdateSumInsured(total_sum_insured, gtli_premium.GTLI_Premium_ID);

                    Response.Redirect("resign_member_transaction_underwrite_detail.aspx?pid=" + gtli_premium.GTLI_Premium_ID);

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please upload an excel file that contains employee data.')", true);

                }
            }
        }
        else
        {
            //Select from table
            ArrayList list_of_resign_certificate_id = new ArrayList();

            int number_of_delete_staff = 0;

            //find select checkboxes
            SelectedCheckBoxes();

            if (!string.IsNullOrEmpty(hfSaleID.Value))
            {
                //Retrieve data from the hidden fields
                string[] hiddenIDs = { };

                if (!string.IsNullOrEmpty(hfSaleID.Value))
                {
                    hiddenIDs = hfSaleID.Value.Split(new char[] { '|' });
                }

                ArrayList arrIDs = new ArrayList();

                if (hiddenIDs.Length != 0)
                {
                    arrIDs.AddRange(hiddenIDs);
                }

                foreach (string item in arrIDs)
                {
                    list_of_resign_certificate_id.Add(item);
                    number_of_delete_staff += 1;
                }

                if (list_of_resign_certificate_id.Count > 0)
                {

                    //GTLI Premium
                    bl_gtli_premium gtli_premium = new bl_gtli_premium();
                    gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                    gtli_premium.Channel_Location_ID = channel_location_id;
                    gtli_premium.Created_By = hdfusername.Value;
                    gtli_premium.Created_On = DateTime.Now;
                    gtli_premium.DHC_Option_Value = 0;
                    gtli_premium.DHC_Premium = 0;
                    gtli_premium.Effective_Date = date_of_removed;
                    gtli_premium.Expiry_Date = this_policy.Expiry_Date;
                    gtli_premium.GTLI_Plan_ID = "0";
                    gtli_premium.GTLI_Policy_ID = this_policy.GTLI_Policy_ID;
                    gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                    gtli_premium.Life_Premium = 0;
                    gtli_premium.Pay_Mode_ID = 1;
                    gtli_premium.Policy_Year = policy_year;
                    gtli_premium.Sale_Agent_ID = ddlSaleAgentReturn.SelectedValue;
                   // gtli_premium.Sum_Insured = 0;
                    gtli_premium.TPD_Premium = 0;
                    gtli_premium.Transaction_Staff_Number = list_of_resign_certificate_id.Count;
                    gtli_premium.User_Total_Staff_Number = 0;
                    gtli_premium.Accidental_100Plus_Premium = 0;
                    gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                    gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                    gtli_premium.DHC_Premium_Discount = 0;
                    gtli_premium.DHC_Premium_Tax_Amount = 0;
                    gtli_premium.Life_Premium_Discount = 0;
                    gtli_premium.Life_Premium_Tax_Amount = 0;
                    gtli_premium.Original_Accidental_100Plus_Premium = 0;
                    gtli_premium.Original_DHC_Premium = 0;
                    gtli_premium.Original_Life_Premium = 0;
                    gtli_premium.Original_TPD_Premium = 0;
                    gtli_premium.TPD_Premium_Discount = 0;
                    gtli_premium.TPD_Premium_Tax_Amount = 0;
                    gtli_premium.Discount = 0;

                    gtli_premium.Transaction_Type = 3;


                    //if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign gtli employee failed. Please check your inputs again.')", true);
                    //    return;

                    //}

                    //loop through resign certificate id list
                    for (int j = 0; j <= list_of_resign_certificate_id.Count - 1; j++)
                    {
                        //keys = certificate_id and premium_id
                        string keys = list_of_resign_certificate_id[j].ToString().Trim();

                        //get row premium
                        bl_gtli_premium row_premium = da_gtli_premium.GetGTLIPremiumByID(keys.Substring(keys.IndexOf(",") + 1));

                        bl_gtli_certificate row_certificate = da_gtli_certificate.GetGTLICertificateByID(keys.Substring(0, keys.IndexOf(",")));

                        //get employee
                        bl_gtli_employee myemployee = da_gtli_employee.GetEmployeeByID(row_certificate.GTLI_Certificate_ID);


                        //sum total sum insured
                       // total_sum_insured += row_premium.Sum_Insured;

                        //get row sale date
                        System.DateTime add_date = row_premium.Effective_Date;

                        int used_days = 0;

                        //Get Number of Days Used
                        used_days = CalculateUsedDays(add_date, date_of_removed);

                        decimal premium_Death = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumByGTLICertificateID(row_certificate.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "Death"));
                        decimal premium_TPD = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumByGTLICertificateID(row_certificate.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "TPD"));
                        decimal premium_DHC = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumByGTLICertificateID(row_certificate.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "DHC"));
                        decimal premium_accidental_100plus = Convert.ToDecimal(da_gtli_employee_premium.GetPremiumByGTLICertificateID(row_certificate.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "Accidental100Plus"));

                        double employee_sum_insure = da_gtli_employee_premium.GetSumInsuredByGTLICertificateID(myemployee.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "Death");

                        total_sum_insured += employee_sum_insure;

                        //maneth
                        //check exist certificate id in [Ct_GTLI_Certificate_Temporary]
                        string strselect = "";
                        bool exist = false;
                        strselect = "select GTLI_Certificate_ID from Ct_GTLI_Certificate_Temporary where GTLI_Certificate_ID='" + row_certificate.GTLI_Certificate_ID + "'";
                        System.Data.SqlClient.SqlCommand cmd;
                        System.Data.SqlClient.SqlDataReader dr;
                        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(AppConfiguration.GetConnectionString());
                        cmd = new System.Data.SqlClient.SqlCommand(strselect, con);
                        con.Open();
                        dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            exist = true;
                        }
                        dr.Close();
                        cmd.Dispose();
                        con.Close();


                        if (exist == true)
                        {
                            //update certificate
                            if(!da_gtli_certificate_temporary.UpdateCertificateTemporary(row_certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }
                        }
                        else
                        {
                            //Insert Certificate   
                            if (!da_gtli_certificate_temporary.InsertCertificateTemporary(row_certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }
                        }
                       

                        //Delete employee temporary by certificate id
                        da_gtli_employee_temporary.DeleteGTLIEmployeeTemporaryByCertificateID(myemployee.GTLI_Certificate_ID);
                        
                        //Insert Employee
                        if (!da_gtli_employee_temporary.InsertEmployeeTemporary(myemployee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        //Find Premium Return
                        decimal premium_charged_Death = 0;
                        decimal premium_return_Death = 0;

                        if (row_premium.Discount > 0)
                        {
                             premium_charged_Death = ((premium_Death - (Math.Floor(((premium_Death * Convert.ToDecimal(row_premium.Discount)) / 100)))) * used_days) / total_days;
                             premium_return_Death = (premium_Death - (Math.Floor(((premium_Death * Convert.ToDecimal(row_premium.Discount)) / 100)))) - Math.Round(premium_charged_Death);
                        }
                        else
                        {
                             premium_charged_Death = (premium_Death * used_days) / total_days;
                             premium_return_Death = premium_Death - Math.Round(premium_charged_Death);
                        }
                     
                        //Insert employee premium life
                        string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                        employee_premium_life.Premium_Type = "Death";
                        employee_premium_life.Premium = premium_return_Death.ToString();
                        employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                        employee_premium_life.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                        employee_premium_life.Sum_Insured = employee_sum_insure.ToString();

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                            return;
                        }

                        total_premium_return_death += Convert.ToDouble(premium_return_Death);

                        decimal premium_charged_TPD = default(decimal);
                        decimal premium_charged_DHC = default(decimal);
                        decimal premium_return_TPD = default(decimal);
                        decimal premium_return_DHC = default(decimal);
                        decimal premium_charged_accidental_100plus = default(decimal);
                        decimal premium_return_accidental_100plus = default(decimal);

                        //Insert employee premium accidental 100plus
                        if (premium_accidental_100plus != 0 | Convert.ToDouble(premium_accidental_100plus) != 0.0)
                        {
                            if (row_premium.Discount > 0)
                            {
                                premium_charged_accidental_100plus = ((premium_accidental_100plus - (Math.Floor((premium_accidental_100plus * Convert.ToDecimal(row_premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_accidental_100plus = (premium_accidental_100plus - (Math.Floor((premium_accidental_100plus * Convert.ToDecimal(row_premium.Discount)) / 100))) - Math.Round(premium_charged_accidental_100plus);
                            }
                            else
                            {
                                premium_charged_accidental_100plus = (premium_accidental_100plus * used_days) / total_days;
                                premium_return_accidental_100plus = premium_accidental_100plus - Math.Round(premium_charged_accidental_100plus);
                            }


                            string gtli_employee_premium_temporary_id_accidental_100plus = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium accidental 100plus
                            bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                            employee_premium_accidental_100plus.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                            employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_accidental_100plus;
                            employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_accidental_100plus.Premium = premium_return_TPD.ToString();
                            employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";
                            //maneth
                            double employee_premium_accidental_100plus_sum_insured=da_gtli_employee_premium.GetSumInsuredByGTLICertificateID(myemployee.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "Accidental100Plus");
                            employee_premium_accidental_100plus.Sum_Insured = employee_premium_accidental_100plus_sum_insured.ToString();

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_accidental_100plus += Convert.ToDouble(premium_return_accidental_100plus);
                        }

                        if (premium_TPD != 0 | Convert.ToDouble(premium_TPD) != 0.0)
                        {
                            if (row_premium.Discount > 0)
                            {
                                premium_charged_TPD = ((premium_TPD - (Math.Floor((premium_TPD * Convert.ToDecimal(row_premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_TPD = (premium_TPD - (Math.Floor((premium_TPD * Convert.ToDecimal(row_premium.Discount)) / 100))) - Math.Round(premium_charged_TPD);
                            }
                            else
                            {
                                premium_charged_TPD = (premium_TPD * used_days) / total_days;
                                premium_return_TPD = premium_TPD - Math.Round(premium_charged_TPD);
                            }
                        
                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                            employee_premium_tpd.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                            employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_tpd.Premium = premium_return_TPD.ToString();
                            employee_premium_tpd.Premium_Type = "TPD";
                            //maneth
                            double  employee_premium_tpd_Sum_Insure = da_gtli_employee_premium.GetSumInsuredByGTLICertificateID(myemployee.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "TPD");
                            employee_premium_tpd.Sum_Insured = employee_premium_tpd_Sum_Insure.ToString();

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_tpd += Convert.ToDouble(premium_return_TPD);
                        }

                        if (premium_DHC != 0 | Convert.ToDouble(premium_DHC) != 0.0)
                        {
                            if (row_premium.Discount > 0)
                            {
                                premium_charged_DHC = ((premium_DHC - (Math.Floor((premium_DHC * Convert.ToDecimal(row_premium.Discount)) / 100))) * used_days) / total_days;
                                premium_return_DHC = (premium_DHC - (Math.Floor((premium_DHC * Convert.ToDecimal(row_premium.Discount)) / 100))) - Math.Round(premium_charged_DHC);
                            }
                            else
                            {
                                premium_charged_DHC = (premium_DHC * used_days) / total_days;
                                premium_return_DHC = premium_DHC - Math.Round(premium_charged_DHC);
                            }


                            string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                            employee_premium_dhc.Premium_Type = "DHC";
                            employee_premium_dhc.Premium = premium_return_DHC.ToString();
                            employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                            employee_premium_dhc.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;

                            //maneth
                            employee_premium_dhc.Sum_Insured = myemployee.Sum_Insured;
                            double employee_premium_dhc_Sum_Insure = da_gtli_employee_premium.GetSumInsuredByGTLICertificateID(myemployee.GTLI_Certificate_ID, row_premium.GTLI_Premium_ID, "DHC");
                            employee_premium_dhc.Sum_Insured = employee_premium_dhc_Sum_Insure.ToString();

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                                return;
                            }

                            total_premium_return_dhc += Convert.ToDouble(premium_return_DHC);
                        }


                    }
                    //total suminsured
                    gtli_premium.Sum_Insured = total_sum_insured;
                    if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign gtli employee failed. Please check your inputs again.')", true);
                        return;

                    }

                    //Insert gtli prem return
                    bl_gtli_policy_prem_return prem_return = new bl_gtli_policy_prem_return();
                    prem_return.Sale_Agent_ID = ddlSaleAgentReturn.SelectedValue;
                    prem_return.Prem_Year = policy_year;
                    prem_return.Prem_Lot = 1;
                    prem_return.Pay_Mode_ID = 1;
                    prem_return.Return_Date = date_of_removed;
                    prem_return.Office_ID = channel_location_id;
                    prem_return.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                    prem_return.GTLI_Policy_Prem_Return_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                    prem_return.Created_On = DateTime.Now;
                    prem_return.Created_Note = "";
                    prem_return.Created_By = hdfusername.Value;
                    prem_return.Amount = Convert.ToDouble(total_premium_return_death + total_premium_return_dhc + total_premium_return_tpd);
                    prem_return.Payment_Code = "";

                    if (!da_gtli_policy_prem_return_temporary.InsertPolicyPremReturnTemporary(prem_return, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Resign GTLI Employee failed. Please check your inputs again.')", true);
                        return;
                    }

                    //update total premium in premium temporary
                    da_gtli_premium_temporary.UpdatePremium(total_sum_insured, Convert.ToDouble(total_premium_return_death), Convert.ToDouble(total_premium_return_tpd), Convert.ToDouble(total_premium_return_dhc), Convert.ToDouble(total_premium_return_accidental_100plus), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, gtli_premium.GTLI_Premium_ID);
                    da_gtli_premium_temporary.UpdateSumInsured(total_sum_insured, gtli_premium.GTLI_Premium_ID);


                    Response.Redirect("resign_member_transaction_underwrite_detail.aspx?pid=" + gtli_premium.GTLI_Premium_ID);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('No employee selection.')", true);

                }
            }
        }

        if (ddlOption.SelectedValue == "1")
        {
            gvActiveMember.Visible = true;
            upload.Visible = false;
        }
        else
        {
            gvActiveMember.Visible = false;
            upload.Visible = true;
        }
    }

    //Selected Checkbox and store in hiddenfield
    public void SelectedCheckBoxes()
    {
        string[] hiddenIDs = { };
        if (!string.IsNullOrEmpty(hfSaleID.Value))
        {
            hiddenIDs = hfSaleID.Value.Split(new char[] { '|' });
        }

        ArrayList arrIDs = new ArrayList();
        string certificate_id = null;
        string premium_id = null;
        if (hiddenIDs.Length != 0)
        {
            arrIDs.AddRange(hiddenIDs);
        }

        foreach (GridViewRow rowItem in gvActiveMember.Rows)
        {
            CheckBox chk = default(CheckBox);

            chk = (CheckBox)rowItem.Cells[0].FindControl("chk1");
            certificate_id = gvActiveMember.DataKeys[rowItem.RowIndex]["GTLI_Certificate_ID"].ToString();
            premium_id = gvActiveMember.DataKeys[rowItem.RowIndex]["GTLI_Premium_ID"].ToString();
            if (chk.Checked)
            {
                if (!arrIDs.Contains(certificate_id))
                {
                    arrIDs.Add(certificate_id +  "," + premium_id);
                }
            }
            else
            {
                if ((arrIDs.Contains(certificate_id + "," + premium_id)))
                {
                    arrIDs.Remove(certificate_id + "," + premium_id);
                }
            }
        }

        hiddenIDs = (String[])arrIDs.ToArray(Type.GetType("System.String"));

        hfSaleID.Value = string.Join("|", hiddenIDs);
    }


    protected void gvActiveMember_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[7].Text == "$0.00")
            {
                e.Row.Cells[7].Text = " - ";
            }

            if (e.Row.Cells[8].Text == "$0.00")
            {
                e.Row.Cells[8].Text = " - ";
            }

            if (e.Row.Cells[9].Text == "$0.00")
            {
                e.Row.Cells[9].Text = " - ";
            }

            if (e.Row.Cells[10].Text == "$0.00")
            {
                e.Row.Cells[10].Text = " - ";
            }

            if (e.Row.Cells[11].Text == "$0.00")
            {
                e.Row.Cells[11].Text = " - ";
            }

            if (e.Row.Cells[12].Text == "$0.00")
            {
                e.Row.Cells[12].Text = " - ";
            }

            if (e.Row.Cells[13].Text == "$0.00")
            {
                e.Row.Cells[13].Text = " - ";
            }
        }
    }
}