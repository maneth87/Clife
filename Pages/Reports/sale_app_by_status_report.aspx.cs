using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Data;
using System.Web.Security;



public partial class Pages_Reports_sale_app_by_status_report : System.Web.UI.Page
{
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dvReportDetail.Style.Clear();
            dvReportDetail.Controls.Add(new LiteralControl("Please filter your search...."));
            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }
    }       

    protected void ReportApplicationsByStatus()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";

        DateTime from_date = DateTime.Parse(txtFrom_date.Text , dtfi);
        DateTime to_date = DateTime.Parse(txtTo_date.Text + " 23:59:59", dtfi3);

        MembershipUser myUser = Membership.GetUser();
        string user_id = myUser.ProviderUserKey.ToString();

        string sale_agent_team_id = da_sale_agent.GetSaleAgentTeamID(user_id.ToUpper());
        string sale_agent_id = da_sale_agent.GetSaleAgentIDByUserID(user_id.ToUpper());

        string channel_id = da_sale_agent.GetChannelID(user_id.ToUpper());


        List<bl_report_sale_app_by_status> report_sale_app_by_status_list = new List<bl_report_sale_app_by_status>();

        if (user_id.ToUpper() == "16924F57-A04C-4A73-BFFD-466503FDFFF8" || user_id.ToUpper() == "8D8381E9-DEB7-4123-98B3-1C0BD36356B5") //Admin account -> show all applications
        {
            report_sale_app_by_status_list = da_report_sale.GetApplicationByDates(from_date, to_date);
        }
        else
        {
            
                ArrayList agent_id_list = new ArrayList();
                agent_id_list = da_sale_agent.GetAllSaleAgentCodeBySupervisorCode(sale_agent_id);

                //loop agent list
                for (int ag = 0; ag < agent_id_list.Count; ag++)
                {
                    string agent_id = agent_id_list[ag].ToString();
                   
                    ArrayList sub_agent_id_list = new ArrayList();

                    sub_agent_id_list = da_sale_agent.GetAllSaleAgentCodeBySupervisorCode(agent_id);
                    
                    //Loop sub agent list
                    for (int sag = 0; sag < sub_agent_id_list.Count; sag++)
                    {
                        string sub_agent_id = sub_agent_id_list[sag].ToString();
                        agent_id_list.Add(sub_agent_id);
                    }

                    List<bl_report_sale_app_by_status> sub_report_sale_app_by_status_list = new List<bl_report_sale_app_by_status>();
                    sub_report_sale_app_by_status_list = da_report_sale.GetApplicationBySaleAgentIDAndDates(agent_id, from_date, to_date);
                
                    //loop sub report list
                    for (int sr = 0; sr < sub_report_sale_app_by_status_list.Count; sr++)
                    {
                        bl_report_sale_app_by_status report_app_status = new bl_report_sale_app_by_status();
                        report_app_status = sub_report_sale_app_by_status_list[sr];

                        report_sale_app_by_status_list.Add(report_app_status);
                    }
                }
                
            
        }
        
        //Draw Header
        #region

        if (report_sale_app_by_status_list.Count > 0)
            {
                lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Application Status Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + txtFrom_date.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";

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
            if (report_sale_app_by_status_list.Count > 0)
            {

                strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
                strTable += "<tr border=\"1\"><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">App No</th><th styple=\"text-align: center; \">App Date</th><th style=\"text-align: center;\">Agent Name</th><th styple=\"text-align: center; \">Status</th><th styple=\"text-align: center; \">Policy No</th></tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";
            }
            else
            {
                strTable = "";
            }


        #endregion

        //Loop through sale app by status list
        #region
            for (int i = 0; i < report_sale_app_by_status_list.Count; i++)
            {
                //Get obj sale app by status of this index[i]
                bl_report_sale_app_by_status sale_app_by_status = new bl_report_sale_app_by_status();
                sale_app_by_status = report_sale_app_by_status_list[i];
                              
                //Draw Row
                #region

                    if (report_sale_app_by_status_list.Count > 0)
                    {
                        strTable += "<tr>";
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (i + 1) + "</td>";

                        if (sale_app_by_status.App_No == "")
                        {
                            strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                        }
                        else
                        {
                            strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.App_No + "</td>";
                        }
                                              
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.App_Date.ToString("d-MMM-yyyy") + "</td>";

                        strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.Agent_Name + "</td>";

                     
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.Last_Status + "</td>";

                        if (sale_app_by_status.Policy_No == "")
                        {
                            strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                        }
                        else
                        {
                            strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_app_by_status.Policy_No + "</td>";
                        }               
                        
                        strTable += "</tr>";

                        dvReportDetail.Controls.Add(new LiteralControl(strTable));

                        strTable = "";
                    }

                #endregion
                
            }//End loop sale app by status list
        #endregion

      
        dvReportDetail.Style.Clear();

        if (report_sale_app_by_status_list.Count > 0)
        {
            strTable += "</table>";
            dvReportDetail.Controls.Add(new LiteralControl(strTable));
            strTable = "";
        }
        else
        {
            dvReportDetail.Controls.Add(new LiteralControl("Please filter your search...."));

            strTable = "";

            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }

      
    }

     
    protected void btnOk_Click(object sender, EventArgs e)
    {
        ReportApplicationsByStatus();
    }
}