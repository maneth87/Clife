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
using System.Windows.Forms;
using System.Security.Cryptography;


public partial class Pages_Reports_sale_policy: System.Web.UI.Page
{
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ReportApplicationsByStatus();
        }
    }

    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    protected void ReportApplicationsByStatus()
    {
        dvReportDetail.Style.Clear();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        //Get list of all sale agent order by agent code where being active up to now
        #region

        MembershipUser myUser = Membership.GetUser();
        string user_id = myUser.ProviderUserKey.ToString();

        string sale_agent_team_id = da_sale_agent.GetSaleAgentTeamID(user_id.ToUpper());
        string sale_agent_id = da_sale_agent.GetSaleAgentIDByUserID(user_id.ToUpper());
        string channel_id = da_sale_agent.GetChannelID(user_id.ToUpper());

        //Get list of Sale Agent where being active up
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

        DataTable report_sale_policy_list = new DataTable();

        if (txtFrom_date.Text == "" && txtTo_date.Text == "")
        {

            report_sale_policy_list = da_report_sale_policy.GetPolicyBySaleAgentIDAndEffectiveDate(txtAgent_Code.Text, DateTime.Parse("1900/01/01"), DateTime.Parse("1900/01/01"), sale_agent_list);
        }
        else
        {
            report_sale_policy_list = da_report_sale_policy.GetPolicyBySaleAgentIDAndEffectiveDate(txtAgent_Code.Text, DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), sale_agent_list);
        }

        //Draw Header
        #region

        if (report_sale_policy_list.Rows.Count > 0)
            {
                if (txtFrom_date.Text != "" && txtTo_date.Text != "")
                {
                    lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Sales Policy</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + DateTime.Parse(txtFrom_date.Text, dtfi).ToString("dd/MM/yyyy") + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + DateTime.Parse(txtTo_date.Text, dtfi).ToString("dd/MM/yyyy") + "</div>";
                }
            }
            else
            {
                lblfrom.Text = "";
                lblfrom1.Text = "";
                lblto1.Text = "";
            }

            string strTable = "";

            //Draw Header
            if (report_sale_policy_list.Rows.Count > 0)
            {

                strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
                strTable += "<tr border=\"1\"><th styple=\"text-align: center; \">No</th><th styple=\"text-align: center; \">Policy No</th><th styple=\"text-align: center; \">Agent. Name</th><th styple=\"text-align: center; \">Cust. Name</th><th style=\"text-align: center;\">Cust. Contact</th><th styple=\"text-align: center; \">Product</th><th styple=\"text-align: center; \">Mode</th><th styple=\"text-align: center; \">SI</th><th styple=\"text-align: center; \">Prem.</th><th styple=\"text-align: center; \">Due Date</th><th styple=\"text-align: center; \">Effective Date</th><th styple=\"text-align: center; \">Maturity Date</th><th styple=\"text-align: center; \">Status</th></tr>";

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
            int i = 0;

            foreach (DataRow item in report_sale_policy_list.Rows)
            {
                //Draw Row
                #region
                   
                strTable += "<tr>";

                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width:0px;\"  >" + (i + 1) + "</td>";

                if(int.Parse(item["Policy_Type_ID"].ToString()) == 1)
                {
                    if (item["Status"].ToString() == "IF" || item["Status"].ToString() == "LAP")
                    {
                        if (int.Parse(item["Pay_Mode"].ToString()) == 0)
                        {
                            strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Policy_Number"].ToString() + "</td>";
                        }
                        else
                        {
                            strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width:0px;\"  ><a target=\"_blank\" href=\"policy_schedule.aspx?id=" + Encrypt(HttpUtility.UrlDecode(item["Policy_ID"].ToString())) + "\">" + item["Policy_Number"].ToString() + "</a></td>";
                        }
                    }
                    else
                    {
                        strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Policy_Number"].ToString() + "</td>";
                    }
                }
                else{
                    strTable += "<td  style=\"text-align: left; mso-number-format:\\@; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Policy_Number"].ToString() + "</td>";
                }

                strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Sale_Agent"].ToString() + "</td>";

                strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width:0px;\" >" + item["Last_Name"].ToString() + " " + item["First_Name"].ToString() + "</td>";

                strTable += "<td  style=\"text-align: right; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Customer_Contact"].ToString() + "</td>";

                strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Product_Name"].ToString() + "</td>";

                strTable += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width:0px;\"  >" + item["Mode"].ToString() + "</td>";

                strTable += "<td  style=\"text-align: right; padding:5px 5px 5px 5px; width:0px;\"  >" + float.Parse(item["Sum_Insure"].ToString()).ToString("c2") + "</td>";

                strTable += "<td  style=\"text-align: right; padding:5px 5px 5px 5px; width:0px;\"  >" + float.Parse(item["Premium"].ToString()).ToString("c2") + "</td>";

                strTable += "<td  style=\"text-align: right; padding:5px 5px 5px 5px; width:0px;\"  >" +  DateTime.Parse(item["Due_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

                strTable += "<td  style=\"text-align: right;  padding:5px 5px 5px 5px; width:0px;\"  >" + DateTime.Parse(item["Effective_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

                strTable += "<td  style=\"text-align: right;  padding:5px 5px 5px 5px; width:0px;\"  >" + DateTime.Parse(item["Maturity_Date"].ToString()).ToString("dd/MM/yyyy") + "</td>";

                strTable += "<td  style=\"text-align: left;  padding:5px 5px 5px 5px; width:0px;\"  >" + item["Status"].ToString() + "</td>";
                                       
                strTable += "</tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTable));

                strTable = "";

                i = i + 1;

                #endregion
                
            }//End loop sale app by status list
        #endregion

      
        dvReportDetail.Style.Clear();

        if (report_sale_policy_list.Rows.Count > 0)
        {
            strTable += "</table>";
            dvReportDetail.Controls.Add(new LiteralControl(strTable));
            strTable = "";
        }
        else
        {
            dvReportDetail.Controls.Add(new LiteralControl("No record found...."));

            strTable = "";

            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }
    }

     
    protected void btnOk_Click(object sender, EventArgs e)
    {
        ReportApplicationsByStatus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ReportApplicationsByStatus();
    }
}