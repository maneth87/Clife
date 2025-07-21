using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CL24_detail_report : System.Web.UI.Page
{
    List<bl_policy_report> list_daily_report = new List<bl_policy_report>();

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

        }

    }

    void LoadReport()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        DateTime fromDate, toDate; int search_date = -1;

        try
        {
            if (txtFrom_date.Text.Trim() != "" && txtTo_date.Text.Trim() != "")
            {
                fromDate = DateTime.Parse(txtFrom_date.Text, dtfi); toDate = DateTime.Parse(txtTo_date.Text, dtfi);
            }
            else
            {
                fromDate = new DateTime(1900, 1, 01); toDate = DateTime.Now;
            }

            if (txtPolicyNumberSearch.Text != "")
            {
                fromDate = new DateTime(1900, 1, 01); toDate = new DateTime(1900, 1, 01); txtLastnameSearch.Text = ""; txtFirstnameSearch.Text = "";
                list_daily_report = da_report_policy.GetPolicyDetailReportLisByParams(txtPolicyNumberSearch.Text, "", "");

            }
            else if (txtLastnameSearch.Text != "" || txtFirstnameSearch.Text != "")
            {
                fromDate = new DateTime(1900, 1, 01); toDate = new DateTime(1900, 1, 01); txtPolicyNumberSearch.Text = "";
                list_daily_report = da_report_policy.GetPolicyDetailReportLisByParams("", txtLastnameSearch.Text, txtFirstnameSearch.Text);
            }
            else
            {
                txtPolicyNumberSearch.Text = ""; txtLastnameSearch.Text = ""; txtFirstnameSearch.Text = "";

                if (rbtnlData.SelectedValue != "")
                {
                    search_date = int.Parse(rbtnlData.SelectedValue);
                    list_daily_report = da_report_policy.GetPolicyDetailReportList(fromDate, toDate, search_date, int.Parse(rbtnlPolicyStatus.SelectedValue), 1);
                }
                else
                {
                    AlertMessage("Search by date is required!!");
                    return;
                }
            }

            lblfrom1.Text = fromDate.ToString("dd-MM-yyyy");
            lblto1.Text = toDate.ToString("dd-MM-yyyy");

            lblfrom.Text = "<div style='text-align:center;'><input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + "  " + lblfrom1.Text + "<input type='Label' value=' - ' style=\"color:black; border:none; text-align:right; width:20px; font-size:12px;\"/><input type=\"Label\" value=\"To:  \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + lblto1.Text + "</div>";

            StringBuilder strBuilder = new StringBuilder();

            string strTable = "<table class='grid-layout' style='color: black; border: 1pt thin #9f9f9f;' cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";

            if (ckb_split.Checked)
                strTable += "<tr style=\"background-color: #23afc5c7; \"><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Policy Number</th><th styple=\"text-align: center; \">App Number</th><th styple=\"text-align: center; \">Customer ID</th><th styple=\"text-align: center; \">Customer Name</th><th styple=\"text-align: center; \">Gender</th><th styple=\"text-align: center; \">DOB</th><th styple=\"text-align: center; \">Product Name</th><th style=\"text-align: center;\">Age Insure</th><th style=\"text-align: center;\">Coverage Period</th><th style=\"text-align: center;\">Payment Period</th><th style=\"text-align: center;\">Sum Insured</th><th style=\"text-align: center;\">Premium</th><th styple=\"text-align: center; \">Policy Remarks</th><th styple=\"text-align: center; \">Policy Status</th><th style=\"text-align: center;\">Effective Date</th><th style=\"text-align: center;\">Issue Date</th><th style=\"text-align: center;\">Maturity Date</th></tr>";
            else
                strTable += "<tr style=\"background-color: #23afc5c7; \"><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Policy Number</th><th styple=\"text-align: center; \">App Number</th><th styple=\"text-align: center; \">Customer ID</th><th styple=\"text-align: center; \">Customer Name</th><th styple=\"text-align: center; \">Gender</th><th styple=\"text-align: center; \">DOB</th><th styple=\"text-align: center; \">Product Name</th><th style=\"text-align: center;\">Age Insure</th><th style=\"text-align: center;\">Coverage Period</th><th style=\"text-align: center;\">Payment Period</th><th style=\"text-align: center;\">Sum Insured</th><th style=\"text-align: center;\">Premium</th><th style=\"text-align: center;\">Extra Premium</th><th style=\"text-align: center;\">Discount</th><th style=\"text-align: center;\">Total Premium</th><th styple=\"text-align: center; \">Policy Status</th><th style=\"text-align: center;\">Effective Date</th><th style=\"text-align: center;\">Issue Date</th><th style=\"text-align: center;\">Maturity Date</th></tr>";

            strBuilder.Append(strTable);

            strTable = "";

            if (ckb_split.Checked)
            {
                #region split table
                if (list_daily_report.Count > 0)
                {
                    for (int i = 0; i < list_daily_report.Count; i++) // Loop through List with for
                    {
                        bl_policy_report policy_report = new bl_policy_report();
                        double display_premium = 0.0;

                        policy_report.Policy_Number = list_daily_report[i].Policy_Number.ToString();
                        policy_report.App_Number = list_daily_report[i].App_Number.ToString();
                        policy_report.Customer_ID = list_daily_report[i].Customer_ID.ToString();
                        policy_report.customer_name = list_daily_report[i].customer_name.ToString();
                        policy_report.Gender = list_daily_report[i].Gender.ToString();
                        policy_report.Birth_Date = list_daily_report[i].Birth_Date;
                        policy_report.Product_Name = list_daily_report[i].Product_Name.ToString();
                        policy_report.Age_Insure = list_daily_report[i].Age_Insure;
                        policy_report.Pay_Year = list_daily_report[i].Pay_Year;
                        policy_report.Assured_Year = list_daily_report[i].Assured_Year;
                        policy_report.Sum_Insure = list_daily_report[i].Sum_Insure;
                        policy_report.Premium = list_daily_report[i].Premium;
                        policy_report.EM_Amount = list_daily_report[i].EM_Amount;
                        policy_report.Discount_Amount = list_daily_report[i].Discount_Amount;
                        if (list_daily_report[i].Main_Policy.ToString() == "Main")
                        {
                            policy_report.Main_Policy = list_daily_report[i].Main_Policy.ToString();
                            //main_policy = list_daily_report[i].Policy_Number.ToString();
                        }
                        else
                        {
                            //policy_report.Main_Policy = main_policy;
                            policy_report.Main_Policy = "Sub";
                        }
                        policy_report.Policy_Status_Type_ID = list_daily_report[i].Policy_Status_Type_ID;
                        policy_report.Effective_Date = list_daily_report[i].Effective_Date;
                        policy_report.Issue_Date = list_daily_report[i].Issue_Date;
                        policy_report.Maturity_Date = list_daily_report[i].Maturity_Date;
                        display_premium = (policy_report.Premium + policy_report.EM_Amount) - policy_report.Discount_Amount;

                        strTable += "<tr>";
                        strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (i + 1) + "</td>";

                        strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + "</td>";

                        strTable += "<td style=\"text-align: center;  mso-number-format: 0000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.App_Number + "</td>";

                        strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Customer_ID + "</td>";

                        strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.customer_name + "</td>";

                        strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Gender + "</td>";

                        strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Birth_Date).ToString("dd-MM-yyyy") + "</td>";

                        strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Product_Name + "</td>";

                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Age_Insure + "</td>";

                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Pay_Year + "</td>";

                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Assured_Year + "</td>";

                        if (policy_report.Sum_Insure > 0)
                            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Sum_Insure + "</td>";
                        else
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " - " + "</td>";

                        if (policy_report.Premium > 0)
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + display_premium + "</td>";
                        else
                            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " - " + "</td>";

                        //strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Total_Premium + "</td>";
                        strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Main_Policy + "</td>";

                        strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Policy_Status_Type_ID + "</td>";

                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Issue_Date).ToString("dd-MM-yyyy") + "</td>";

                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Maturity_Date).ToString("dd-MM-yyyy") + "</td>";


                        strTable += "</tr>";


                        strBuilder.Append(strTable);

                        strTable = "";
                    }
                }


                strTable += "</table>";

                strBuilder.Append(strTable);

                AppendDivPolicy.InnerHtml = strBuilder.ToString();

                #endregion
            }
            else
            {
                #region non-split (Main Policy)
                bl_policy_report policy_report = new bl_policy_report();
                int index = 0;
                int count = 1;
                int num = 0;
                string[] Sub_PolNumber_Arr;

                if (list_daily_report.Count > 0)
                {
                    for (int i = 0; i < list_daily_report.Count; i++)
                    {
                        if (list_daily_report[i].App_Number.ToString() == list_daily_report[index].App_Number.ToString() && list_daily_report[i].Customer_ID.ToString() == list_daily_report[index].Customer_ID.ToString() && list_daily_report[i].customer_name.ToString() == list_daily_report[index].customer_name.ToString())
                        {
                            if (list_daily_report[i].Main_Policy.ToString() == "Main")
                            {
                                policy_report.Policy_Number = list_daily_report[index].Policy_Number.ToString();
                                policy_report.App_Number = list_daily_report[index].App_Number.ToString();
                                policy_report.Customer_ID = list_daily_report[index].Customer_ID.ToString();
                                policy_report.customer_name = list_daily_report[index].customer_name.ToString();
                                policy_report.Gender = list_daily_report[index].Gender.ToString();
                                policy_report.Birth_Date = list_daily_report[index].Birth_Date;
                                policy_report.Product_Name = list_daily_report[index].Product_Name.ToString();
                                policy_report.Age_Insure = list_daily_report[index].Age_Insure;
                                policy_report.Pay_Year = list_daily_report[index].Pay_Year;
                                policy_report.Assured_Year = list_daily_report[index].Assured_Year;
                                //policy_report.Main_Policy = list_daily_report[index].Main_Policy.ToString();
                                policy_report.Policy_Status_Type_ID = list_daily_report[index].Policy_Status_Type_ID.ToString();
                                policy_report.Effective_Date = list_daily_report[index].Effective_Date;
                                policy_report.Issue_Date = list_daily_report[index].Issue_Date;
                                policy_report.Maturity_Date = list_daily_report[index].Maturity_Date;

                                policy_report.Sum_Insure += list_daily_report[index].Sum_Insure;
                                policy_report.Premium += list_daily_report[index].Premium;
                                policy_report.OLD_Premium = list_daily_report[index].OLD_Premium;
                                policy_report.OLD_EM_Amount = list_daily_report[index].OLD_EM_Amount;
                                policy_report.Discount_Amount = list_daily_report[index].Discount_Amount;
                                policy_report.Round_Status_ID = list_daily_report[index].Round_Status_ID;
                            }
                            else
                            {
                                policy_report.Sum_Insure += list_daily_report[i].Sum_Insure;
                                policy_report.Premium += list_daily_report[i].Premium;
                                policy_report.EM_Amount += list_daily_report[index].EM_Amount;

                                Sub_PolNumber_Arr = list_daily_report[i].Policy_Number.ToString().Split('-');
                                policy_report.Sub_Policy_Number = "-" + Sub_PolNumber_Arr[3];
                            }

                        }
                        else
                        {
                            // SUM TOTAL PREM
                            if (policy_report.Round_Status_ID == "0")
                            {
                                policy_report.Total_Premium = Math.Ceiling((policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount);
                            }
                            else if (policy_report.Round_Status_ID == "1") // Rounding Midpoint
                            {
                                policy_report.Total_Premium = Math.Round((policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount, MidpointRounding.AwayFromZero);
                            }
                            else if (policy_report.Round_Status_ID == "2") // In line 
                            {
                                policy_report.Total_Premium = (policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount;
                            }
                            else // Round UP
                            {
                                policy_report.Total_Premium = Math.Ceiling((policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount);
                            }

                            strTable += "<tr>";
                            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (count) + "</td>";

                            strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + policy_report.Sub_Policy_Number + "</td>";

                            strTable += "<td style=\"text-align: center;  mso-number-format: 0000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.App_Number + "</td>";

                            strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Customer_ID + "</td>";

                            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.customer_name + "</td>";

                            strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Gender + "</td>";

                            strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Birth_Date).ToString("dd-MM-yyyy") + "</td>";

                            strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Product_Name + "</td>";

                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Age_Insure + "</td>";

                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Pay_Year + "</td>";

                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Assured_Year + "</td>";

                            if (policy_report.Sum_Insure > 0)
                                strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Sum_Insure + "</td>";
                            else
                                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " - " + "</td>";

                            if (policy_report.Premium > 0)
                                strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.OLD_Premium + "</td>";
                            else
                                strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " - " + "</td>";

                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.OLD_EM_Amount + "</td>";

                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Discount_Amount + "</td>";

                            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + "$" + policy_report.Total_Premium + "</td>";
                            //strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Main_Policy + "</td>";
                            strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Policy_Status_Type_ID + "</td>";

                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Issue_Date).ToString("dd-MM-yyyy") + "</td>";

                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Maturity_Date).ToString("dd-MM-yyyy") + "</td>";

                            strTable += "</tr>";

                            strBuilder.Append(strTable);
                            strTable = "";

                            policy_report.Policy_Number = "";
                            policy_report.App_Number = "";
                            policy_report.Customer_ID = "";
                            policy_report.customer_name = "";
                            policy_report.Gender = "";
                            policy_report.Product_Name = "";
                            policy_report.Age_Insure = 0;
                            policy_report.Main_Policy = "";
                            policy_report.Sub_Policy_Number = "";

                            policy_report.Sum_Insure = 0.0;
                            policy_report.Premium = 0.0;

                            num += index;
                            index = (index + i) - num;  //increase for next policy
                            count += 1;
                            num = 0;
                            i -= 1;
                        }

                    }
                    #region LAST Insert

                    // SUM TOTAL PREM
                    if (policy_report.Round_Status_ID == "0")
                    {
                        policy_report.Total_Premium = Math.Ceiling((policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount);
                    }
                    else if (policy_report.Round_Status_ID == "1") // Rounding Midpoint
                    {
                        policy_report.Total_Premium = Math.Round((policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount, MidpointRounding.AwayFromZero);
                    }
                    else if (policy_report.Round_Status_ID == "2") // In line 
                    {
                        policy_report.Total_Premium = (policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount;
                    }
                    else // Round UP
                    {
                        policy_report.Total_Premium = Math.Ceiling((policy_report.OLD_Premium - policy_report.Discount_Amount) + policy_report.OLD_EM_Amount);
                    }

                    strTable += "<tr>";
                    strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + (count) + "</td>";

                    strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Policy_Number + policy_report.Sub_Policy_Number + "</td>";

                    strTable += "<td style=\"text-align: center;  mso-number-format: 0000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.App_Number + "</td>";

                    strTable += "<td style=\"text-align: center;  mso-number-format: 00000000; padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.Customer_ID + "</td>";

                    strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"   class=\"tb-schedule-td-right\">" + policy_report.customer_name + "</td>";

                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Gender + "</td>";

                    strTable += "<td style=\"text-align: center;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Birth_Date).ToString("dd-MM-yyyy") + "</td>";

                    strTable += "<td style=\"text-align: left;   padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Product_Name + "</td>";

                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Age_Insure + "</td>";

                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Pay_Year + "</td>";

                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + policy_report.Assured_Year + "</td>";

                    if (policy_report.Sum_Insure > 0)
                        strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Sum_Insure + "</td>";
                    else
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " - " + "</td>";

                    if (policy_report.Premium > 0)
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.OLD_Premium + "</td>";
                    else
                        strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + " - " + "</td>";

                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.OLD_EM_Amount + "</td>";

                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + "$" + policy_report.Discount_Amount + "</td>";

                    strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + "$" + policy_report.Total_Premium + "</td>";

                    //strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Main_Policy + "</td>";
                    strTable += "<td style=\"text-align: center;  padding:5px 5px 5px 5px; width=\"0px;\" class=\"tb-schedule-td-right\">" + policy_report.Policy_Status_Type_ID + "</td>";

                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Effective_Date).ToString("dd-MM-yyyy") + "</td>";
                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Issue_Date).ToString("dd-MM-yyyy") + "</td>";

                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  class=\"tb-schedule-td-right\">" + (policy_report.Maturity_Date).ToString("dd-MM-yyyy") + "</td>";


                    strTable += "</tr>";

                    #endregion
                }

                #endregion

                strTable += "</table>";
                strBuilder.Append(strTable);
                AppendDivPolicy.InnerHtml = strBuilder.ToString();

            }

            txtFrom_date.Text = "";
            txtTo_date.Text = "";
            txtPolicyNumberSearch.Text = ""; txtLastnameSearch.Text = ""; txtFirstnameSearch.Text = "";
        }
        catch (Exception ex)
        {

        }

        

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadReport();
    }

    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }

}