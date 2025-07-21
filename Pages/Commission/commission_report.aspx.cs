using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;

public partial class Pages_Report_commission_report : System.Web.UI.Page
{
    string user_name = "";
    string user_id = "";
    //First Page Load Event    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            System.Web.Security.MembershipUser myUser = System.Web.Security.Membership.GetUser();
            user_id = myUser.ProviderUserKey.ToString();
            user_name = myUser.UserName;

            //string page_name = Path.GetFileName(Request.Url.AbsolutePath);
            //da_user_access user_acc = new da_user_access();
            //if (user_acc.GetActiveUserAccessPage(page_name, user_id).UserId != user_id)
            //{
            //    div_main_Toolbar.Attributes.CssStyle.Add("display", "none");
            //    div_main.Attributes.CssStyle.Add("display", "none");
            //    showMessage("No permission to access this page!", "1");
            //}
            //else
            //{
            //    showMessage("", "");
            //    //Default Table Detail
            //    dvReportDetail.Style.Clear();
            //    dvReportDetail.Controls.Add(new LiteralControl("Please filter your search...."));
            //    dvReportDetail.Style.Add("color", "#3399ff");
            //    dvReportDetail.Style.Add("Font-Weight", "bold");
            //}


            showMessage("", "");
            //Default Table Detail
            dvReportDetail.Style.Clear();
            dvReportDetail.Controls.Add(new LiteralControl("Please filter your search...."));
            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        GetPolicyDetailForReport();
    }

    //Generate Product Premium Report
    protected void GetPolicyDetailForReport()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";

        DateTime from_date = DateTime.Parse(txtFrom_date.Text, dtfi);
        DateTime to_date = DateTime.Parse(txtTo_date.Text + " 23:59:59", dtfi3);

        List<bl_commission_report> commission_list = new List<bl_commission_report>();
        commission_list = da_commission_report.GetPolicyDetailForReport(from_date, to_date);
        //Draw Header
        #region

        if (commission_list.Count > 0)
        {
            lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Commission Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + txtFrom_date.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";

            lblfrom1.Text = txtFrom_date.Text;
            lblto1.Text = txtTo_date.Text;
        }
        else
        {
            lblfrom.Text = "";
            lblfrom1.Text = "";
            lblto1.Text = "";
        }

        string strTable = "";

        //Draw Header
        if (commission_list.Count > 0)
        {
            strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
            strTable += "<tr border=\"1\"><th styple=\"text-align: center; width:90px; \">No</th><th styple=\"text-align: center; \">Insured Name</th><th styple=\"text-align: center; \">Policy Number</th><th styple=\"text-align: center; \">Issue Date</th><th style=\"text-align: center;\">Year</th><th styple=\"text-align: center; \">Product</th><th style=\"text-align: center;\">Sum Insured</th><th styple=\"text-align: center; \">Std Prem</th><th styple=\"text-align: center; \">EM Amount</th><th style=\"text-align: center;\">Mode</th><th style=\"text-align: center;\">Year Time</th><th styple=\"text-align: center; \">Sale Agent ID</th><th style=\"text-align: center;\">Agent</th><th style=\"text-align: center;\">Location</th></tr>";

            dvReportDetail.Controls.Add(new LiteralControl(strTable));

            strTable = "";
        }
        else
        {
            //Default Text in Table Detail
            dvReportDetail.Controls.Add(new LiteralControl("No data found. Please filter your search...."));

            strTable = "";

            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }

        //Declare total Std Prem & EM Amount variables
        #region
        double total_std_prem = 0; 
        double total_em_amount = 0;
        #endregion

        #endregion
        int row_no = 1;
        try
        {
            if (commission_list.Count > 0)
            {
                foreach (var item in commission_list)
                {
                    if (int.Parse((item.Policy_Type).ToString()) == 1)
                    {
                        int count_payment = da_commission_report.CountNumPayment((item.Receipt_No).ToString(), from_date, to_date);
                        DataTable dt_policy_individual = da_commission_report.GetPolcyPremInfo((item.Policy_Number).ToString(), int.Parse((item.Policy_Type).ToString()));

                        if (dt_policy_individual.Rows.Count > 0)
                        {
                            DataTable dt_get_sum_mode = da_commission_report.GetSIMode((item.Receipt_No).ToString(), from_date, to_date, int.Parse((item.Policy_Type).ToString()));
                            if (dt_get_sum_mode.Rows.Count > 0)
                            {
                                foreach (DataRow dr_gt_prem in dt_get_sum_mode.Rows)
                                {
                                    string strTableGroupPremPay = "<tr>";
                                    strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (row_no) + "</td>";

                                    //Insured Name Column
                                    DataTable dt_app = da_commission_report.GetAppInfo((item.Policy_ID).ToString());
                                    if (dt_app.Rows.Count > 0)
                                        strTableGroupPremPay += "<td  style=\"text-align: center; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dt_app.Rows[0]["Customer"].ToString() + "</td>";
                                    else
                                        strTableGroupPremPay += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                                    //Policy Number Column
                                    strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (item.Policy_Number).ToString() + "</td>";

                                    //Issued Date Column
                                    strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + DateTime.Parse(dt_policy_individual.Rows[0]["Issue_Date"].ToString()).ToString("dd-MM-yyyy") + "</td>";

                                    //Prem year Column
                                    strTableGroupPremPay += "<td style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dr_gt_prem["Prem_Year"].ToString() + "</td>";

                                    //Product Column
                                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dt_policy_individual.Rows[0]["En_Title"].ToString() + "</td>";

                                    ////Sum Insure Column
                                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dt_get_sum_mode.Rows[0]["Sum_Insure"].ToString() +"</td>";

                                    ////Std Prem Column
                                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dr_gt_prem["Std_Prem"].ToString() + "</td>";
                                    total_std_prem += double.Parse(dr_gt_prem["Std_Prem"].ToString());
                                    ////EM Amount Column
                                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dr_gt_prem["Prem_EM_Amount"].ToString() + "</td>";
                                    total_em_amount += double.Parse(dr_gt_prem["Prem_EM_Amount"].ToString());
                                    ////Pay Mode Column
                                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dt_get_sum_mode.Rows[0]["Mode"].ToString() + "</td>";

                                    ////Year Time Column
                                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + dr_gt_prem["Prem_Year"].ToString() + "Year" + "/" + dr_gt_prem["Prem_Lot"].ToString() + "</td>";

                                    ////Sale Agent ID Column
                                    DataTable sale_agent = da_commission_report.GetSaleAgent((item.Policy_Number).ToString(), DateTime.Parse((item.Created_On).ToString()));
                                    if (sale_agent.Rows.Count > 0)
                                    {
                                        strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_agent.Rows[0]["Sale_Agent_ID"].ToString() + "</td>";
                                        strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_agent.Rows[0]["full_Name"].ToString() + "</td>";
                                    }
                                    else
                                    {
                                        strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                                        strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                                    }

                                    ////Location Column
                                    string channel_name = da_commission_report.GetChannelNameByPolicyNum((item.Policy_Number).ToString(), int.Parse((item.Policy_Type).ToString()));
                                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + channel_name + "</td>";
                                    
                                    
                                    strTableGroupPremPay += "</tr>";

                                    dvReportDetail.Controls.Add(new LiteralControl(strTableGroupPremPay));

                                    strTableGroupPremPay = "";

                                    row_no += 1;
                                }
                            }
                        }
                    }
                }

                //Draw Total
                #region

                if (commission_list.Count > 0)
                {
                    strTable += "<tr>";

                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\" >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >Total:</td>";


                    if ((total_std_prem) == 0)
                    {
                        strTable += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        //Negative
                        if ((total_std_prem) < 0)
                        {
                            strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_std_prem) + ")</td>";
                        }
                        else //Positive
                        {
                            strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_std_prem) + "</td>";
                        }
                    }

                    if ((total_em_amount) == 0)
                    {
                        strTable += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        if ((total_em_amount) < 0)
                        {
                            strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_em_amount) + ")</td>";
                        }
                        else
                        {
                            strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_em_amount) + "</td>";
                        }
                    }
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\" >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

                    strTable += "</tr>";

                    dvReportDetail.Controls.Add(new LiteralControl(strTable));

                    strTable = "";
                }

                #endregion
            }
            
        }
        catch (Exception)
        {
            dvReportDetail.Controls.Add(new LiteralControl("Error...."));
        }



        if (commission_list.Count > 0)
        {
            //Table Detail End
            strTable += "</table>";
            dvReportDetail.Controls.Add(new LiteralControl(strTable));
            dvReportDetail.Style.Clear();

            strTable = "";
        }
        else
        {
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">Text message to show user</param>
    /// /// <param name="type">0=Success, 1=Error, 2=warning</param>
    void showMessage(string message, string type)
    {
        if (message.Trim() != "")
        {
            if (type == "0")
            {

                div_message.Attributes.CssStyle.Add("background-color", "#228B22");
            }
            else if (type == "1")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#f00");

            }
            else if (type == "2")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#ffcc00");
            }
            div_message.Attributes.CssStyle.Add("display", "block");
            div_message.InnerHtml = message;
        }
        else
        {
            div_message.Attributes.CssStyle.Add("display", "none");
        }
    }
}
