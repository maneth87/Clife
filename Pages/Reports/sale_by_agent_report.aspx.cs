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



public partial class Pages_Reports_sale_by_agent_report : System.Web.UI.Page
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

    protected void ReportSaleByAgent()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";

        DateTime from_date = DateTime.Parse(txtFrom_date.Text , dtfi);
        DateTime to_date = DateTime.Parse(txtTo_date.Text + " 23:59:59", dtfi3);

        List<bl_report_sale_by_agent> report_sale_by_agent_list = new List<bl_report_sale_by_agent>();

        //Get list of all sale agent order by agent code where being active up to now
        #region

            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();

            string sale_agent_team_id = da_sale_agent.GetSaleAgentTeamID(user_id.ToUpper());
            string sale_agent_id = da_sale_agent.GetSaleAgentIDByUserID(user_id.ToUpper());

            string channel_id = da_sale_agent.GetChannelID(user_id.ToUpper());

            //Get list of Sale Agent where being active
            List<bl_sale_agent> sale_agent_list = new List<bl_sale_agent>();

            if (user_id.ToUpper() == "16924F57-A04C-4A73-BFFD-466503FDFFF8" || user_id.ToUpper() == "8D8381E9-DEB7-4123-98B3-1C0BD36356B5") //Admin account -> show all current active sale agent
            {
                sale_agent_list = da_sale_agent.GetActiveSaleAgentList();
            }
            else
            {               
                    ArrayList agent_id_list = new ArrayList();
                    agent_id_list = da_sale_agent.GetAllSaleAgentCodeBySupervisorCode(sale_agent_id);

                    bl_sale_agent sale_agent = new bl_sale_agent();
                    sale_agent.Sale_Agent_ID = sale_agent_id;
                    sale_agent.Full_Name = da_sale_agent.GetSaleAgentName(sale_agent_id);

                    sale_agent_list.Add(sale_agent);

                    //loop agent list
                    for (int ag = 0; ag < agent_id_list.Count; ag++)
                    {
                        string agent_id = agent_id_list[ag].ToString();

                        bl_sale_agent sale_agent2 = new bl_sale_agent();
                        sale_agent2.Sale_Agent_ID = agent_id;
                        sale_agent2.Full_Name = da_sale_agent.GetSaleAgentName(agent_id);

                        sale_agent_list.Add(sale_agent2);

                        ArrayList sub_agent_id_list = new ArrayList();

                        sub_agent_id_list = da_sale_agent.GetAllSaleAgentCodeBySupervisorCode(agent_id);

                        //Loop sub agent list
                        for (int sag = 0; sag < sub_agent_id_list.Count; sag++)
                        {
                            string sub_agent_id = sub_agent_id_list[sag].ToString();
                            agent_id_list.Add(sub_agent_id);
                        }

                    }                    
                
            } 

        #endregion

        //Declare total variables
        #region
            int total_sbmt = 0;
            int total_issue = 0;
            int total_os = 0;
            int total_decline = 0;
            int total_cancel = 0;
            int total_postpone = 0;
            double total_sbmt_fyp = 0;
            double total_api = 0;
            double total_issue_fyp = 0;
        #endregion

        //Draw Header
        #region

            if (sale_agent_list.Count > 0)
            {
                lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Sales Performance Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + txtFrom_date.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";

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
            if (sale_agent_list.Count > 0)
            {

                strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
                strTable += "<tr border=\"1\"><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Manager Name</th><th styple=\"text-align: center; \">Manager Code</th><th style=\"text-align: center;\">Agent Name</th><th styple=\"text-align: center; \">Agent Code</th><th styple=\"text-align: center; \">SBMT</th><th style=\"text-align: center;\">ISSUE</th><th style=\"text-align: center;\">OS</th><th style=\"text-align: center;\">Decline</th><th style=\"text-align: center;\">Postpone</th><th style=\"text-align: center;\">Cancel</th><th style=\"text-align: center; width=\"0px;\" \">API</th><th style=\"text-align: center; width=\"0px;\" \">SBMT FYP</th><th style=\"text-align: center; width=\"0px;\" \">ISSUE FYP</th></tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";
            }
            else
            {
                strTable = "";
            }


        #endregion

        List<bl_report_sale_by_agent> sale_performance_list = new List<bl_report_sale_by_agent>();

        //Loop through sale agent list
        #region
            for (int i = 0; i < sale_agent_list.Count; i++)
            {
                //Get obj sale agent of this index[i]
                bl_sale_agent sale_agent = new bl_sale_agent();
                sale_agent = sale_agent_list[i];

                //Get supervisor
                #region
                
                    string manager_name = "";
                    string manager_code = "";
                    
                    bl_sale_agent supervisor = new bl_sale_agent();
                    supervisor = da_sale_agent.GetSaleAgentSupervisor(sale_agent.Sale_Agent_ID);
                    if (supervisor.Sale_Agent_ID != "")
                    {
                        manager_code = supervisor.Sale_Agent_ID;
                        manager_name = supervisor.Full_Name.ToUpper();
                    }
                    

                #endregion

                bl_report_sale_by_agent sale_performance = new bl_report_sale_by_agent();

                sale_performance.Manager_Code = manager_code;
                sale_performance.Manager_Name = manager_name;
                sale_performance.LIC_Code = sale_agent.Sale_Agent_ID;
                sale_performance.LIC_Name = sale_agent.Full_Name.ToUpper();
              
                //Get report data by this agent and between dates
                #region

                    //Data from applications by sale agent id and between dates
                    #region
                        
                        //Get applications submit count by sale agent id and between dates (SBMT)
                        int sbmt = da_report_sale.GetApplicationCountBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date);

                        //Get applications submit First Year Premium (SBMT FYP) by sale agent id and between dates (SBMT FYP)
                        double sbmt_fyp = da_report_sale.GetSBMTFYPBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date);

                        sale_performance.App_Submit_Count = sbmt;
                        sale_performance.App_Submit_FYP = sbmt_fyp;

                    #endregion

                    //Data from underwriting by sale agent id and between dates
                    #region

                        //Get application outstanding count by sale agent id and between dates (Status = AP, CO, MO)
                        int os = da_report_sale.GetApplicationOSCountBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date); 

                        //Get application decline count by sale agent id and between dates (Status = DC & NT)
                        int decline = da_report_sale.GetApplicationDeclineCountBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date); 

                        //Get application cancel count by sale agent id and between dates (Status = CC)
                        int cancel = da_report_sale.GetApplicationCancelCountBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date);

                        //Get application postpone count by sale agent id and between dates (Status = PP)
                        int postpone = da_report_sale.GetApplicationPostponeCountBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date);

                        sale_performance.Decline = decline;
                        sale_performance.OS = os;
                        sale_performance.Postpone = postpone;
                        sale_performance.Cancel = cancel;
                    #endregion

                    //Data from policy by sale agent id and between dates
                    #region

                        //Get policy issue count by sale agent id and between dates includes only individual policies
                        int policy_issue_count = da_report_sale.GetPolicyIssueCountBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date);

                        //Get API by sale agent id and between dates includes only individual policies
                        double api = da_report_sale.GetPolicyAPIBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date);

                        //Get ISSUE FYP by sale agent id and between dates includes only individual policies
                        double issue_fyp = da_report_sale.GetIssueFYPBySaleAgentIDAndBetweenDates(sale_agent.Sale_Agent_ID, from_date, to_date);

                        sale_performance.API = api;
                        sale_performance.Issue_Count = policy_issue_count;
                        sale_performance.Issue_FYP = issue_fyp;
                    #endregion

                #endregion

                //Get total
                #region

                    total_sbmt += sbmt;
                    total_decline += decline;
                    total_issue += policy_issue_count;
                    total_sbmt_fyp += sbmt_fyp;
                    total_issue_fyp += issue_fyp;
                    total_api += api;
                    total_cancel += cancel;
                    total_os += os;
                    total_postpone += postpone;

                #endregion

                sale_performance_list.Add(sale_performance);

              

            }//End loop sale agent list
        #endregion

        List<bl_report_sale_by_agent> sale_performance_list2 = new List<bl_report_sale_by_agent>();

        sale_performance_list2 = sale_performance_list.OrderBy(x => x.Manager_Name).ToList();

        //Loop through sale_performance_list
        #region
            for (int j = 0; j < sale_performance_list2.Count; j++)
            {
                bl_report_sale_by_agent sale_performance = new bl_report_sale_by_agent();
                sale_performance = sale_performance_list2[j];

                //Draw Row
                #region

                if (sale_performance_list.Count > 0)
                {
                    strTable += "<tr>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (j + 1) + "</td>";

                    if (sale_performance.Manager_Name == "")
                    {
                        strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    }
                    else
                    {
                        strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_performance.Manager_Name + "</td>";
                    }

                    if (sale_performance.Manager_Code == "")
                    {
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
                    }
                    else
                    {
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_performance.Manager_Code + "</td>";
                    }                                       

                    strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  ><a target=\"_blank\" href=\"sale_app_details.aspx?id=" + sale_performance.LIC_Code + "&from=" + from_date.Day + "_" + from_date.Month + "_" + from_date.Year + "&to=" + to_date.Day + "_" + to_date.Month + "_" + to_date.Year + "\">" + sale_performance.LIC_Name + "</a></td>";

                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_performance.LIC_Code + "</td>";

                    if (sale_performance.App_Submit_Count == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_performance.App_Submit_Count + "</td>";
                    }

                    if (sale_performance.Issue_Count == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_performance.Issue_Count + "</td>";
                    }

                    if (sale_performance.OS == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + sale_performance.OS + "</td>";
                    }

                    if (sale_performance.Decline == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" >" + sale_performance.Decline + "</td>";
                    }

                    if (sale_performance.Postpone == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" >" + sale_performance.Postpone + "</td>";
                    }

                    if (sale_performance.Cancel == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" >" + sale_performance.Cancel + "</td>";
                    }

                    if (sale_performance.API == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + "$" + sale_performance.API + "</td>";
                    }

                    if (sale_performance.App_Submit_FYP == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + "$" + sale_performance.App_Submit_FYP + "</td>";
                    }

                    if (sale_performance.Issue_FYP == 0)
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + "$" + sale_performance.Issue_FYP + "</td>";
                    }

                    strTable += "</tr>";

                    dvReportDetail.Controls.Add(new LiteralControl(strTable));

                    strTable = "";
                }

                #endregion
            }
        #endregion
         

        //Draw Total
        #region

            if (sale_agent_list.Count > 0)
            {
                strTable += "<tr>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >&nbsp;</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

                strTable += "<td  style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >Total:</td>";

                if (total_sbmt == 0)
                {
                    strTable += "<td style=\"text-align: right; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_sbmt + "</td>";
                }

                if (total_issue == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + total_issue + "</td>";
                }

                if (total_os == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_os + "</td>";
                }

                if (total_decline == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_decline + "</td>";
                }

                if (total_postpone == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_postpone + "</td>";
                }

                if (total_cancel == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_cancel + "</td>";
                }

                if (total_api == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + "$" + total_api + "</td>";
                }

                if (total_sbmt_fyp == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\" >" + "$" + total_sbmt_fyp + "</td>";
                }

                if (total_issue_fyp == 0)
                {
                    strTable += "<td style=\"text-align: right;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTable += "<td style=\"text-align: right; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + "$" + total_issue_fyp + "</td>";
                }
                
                strTable += "</tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";
            }

        #endregion

        dvReportDetail.Style.Clear();

        if (sale_agent_list.Count > 0)
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
        ReportSaleByAgent();
    }
}