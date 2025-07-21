using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.Security;

using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;

public partial class Pages_Card_print_policy_cards_rp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string policy_type = "";
        string policy_id = "";
        string customer_id = "";
        List<string> list_policy_id = new List<string>();
        List<string> list_customer_id = new List<string>();
        string data_source_type = "";

        if (!Page.IsPostBack)
        {
            try
            {
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                data_source_type = Session["SS_DATA_SOURCE_TYPE"] + "";
                policy_type = Request.Params["policy_type"];
                //policy_id = Request.Params["policy_id"];
                //customer_id = Request.Params["customer_id"];


                list_policy_id = (List<string>) Session["SS_POLICY_ID"];
                list_customer_id = (List<string>)Session["SS_CUSTOMER_ID"];
                DataTable tbl = new DataTable();
  
                    #region New code
                    if (data_source_type.ToUpper() == "INTERNAL")//data in database
                    {
                        foreach (string str in list_policy_id)
                        {
                            policy_id = policy_id + str + ",";
                        }
                        policy_id = policy_id.Substring(0, policy_id.Length - 1);
                        if (policy_id != "" && policy_id != null && policy_type != "" && policy_type != null)
                        {
                            if (policy_type.Trim().ToUpper() == "IND")
                            {
                                tbl = DataSetGenerator.Get_Data_Soure("SP_PRINT_POLICY_CARDS", new string[,] { { "@Policy_Type", policy_type.Trim().ToUpper() }, { "@Policy_ID", policy_id }, { "@Customer_ID", "" }, { "@Policy_Number", "" }, { "@From", "" }, { "@To", "" } });

                            }
                            else if (policy_type.Trim().ToUpper() == "GROUP")
                            {
                                foreach (string str in list_customer_id)
                                {
                                    customer_id = customer_id + str + ",";
                                }
                                customer_id = customer_id.Substring(0, customer_id.Length - 1);
                                // customer_id = customer_id.Replace(",", "','");
                                tbl = DataSetGenerator.Get_Data_Soure("SP_PRINT_POLICY_CARDS", new string[,] { { "@Policy_Type", policy_type.Trim().ToUpper() }, { "@Policy_ID", policy_id }, { "@Customer_ID", customer_id }, { "@Policy_Number", "" }, { "@From", "" }, { "@To", "" } });
                            }
                        }
                    }
                    else if(data_source_type.ToUpper() == "EXTERNAL")//Data import from excel
                    {
                        tbl = DataSetGenerator.Get_Data_Soure( AppConfiguration.GetConnectionString(), "Select policy_number, customer_number, kh_name, en_name, ISNULL(effective_date, expiry_date) as 'effective_date', expiry_date from tbl_temp_policy_cards_records where created_by='" + user_name + "'");
                    }
                    
                    

                    #endregion

                    //DataTable tbl = new DataTable();
                    //tbl = DataSetGenerator.Get_Data_Soure(query);
                    if (tbl.Rows.Count > 0)
                    {
                        ReportDataSource report_source = new ReportDataSource("card", tbl);
                        ReportViewer my_viewer = new ReportViewer();
                        my_viewer.Reset();
                        my_viewer.LocalReport.DataSources.Clear();
                        my_viewer.LocalReport.DataSources.Add(report_source);
                        
                        //my_report_view.Reset();
                        //my_report_view.LocalReport.DataSources.Clear();
                        //my_report_view.LocalReport.DataSources.Add(report_source);

                        if (policy_type.Trim().ToUpper() == "IND")
                        {

                            //my_report_view.LocalReport.ReportPath = Server.MapPath("card.rdlc");
                            my_viewer.LocalReport.ReportPath = Server.MapPath("card.rdlc");
                            my_viewer.LocalReport.Refresh();

                        }
                        else if (policy_type.Trim().ToUpper() == "GROUP")
                        {

                            my_viewer.LocalReport.ReportPath = Server.MapPath("card_group.rdlc");
                           
                            //crystal report

                            //ReportDocument rpt = new ReportDocument();
                            //rpt.Load(Server.MapPath("group_policy_cards.rpt"));
                            //rpt.SetDataSource(tbl);
                            //Report_Generator.CrystallToPDF(this.Context,rpt);

                        }

                        //my_report_view.LocalReport.Refresh();

                        my_viewer.LocalReport.Refresh();

                        Report_Generator.ExportToPDF(this.Context, my_viewer, "Policy_cards" + DateTime.Now.ToString("yyyyMMddhhmmss"), true);
                    }
                    else
                    {
                        div_message.InnerText = "Record(s) Not Found.";
                    }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error page [print_policy_cards_rp.aspx.cs], on page load, Detail:" + ex.Message);
            }
        }
        
       
        
    }
}