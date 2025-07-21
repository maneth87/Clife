using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Printing;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using System.Web.Security;

using CrystalDecisions.CrystalReports.Design;
using CrystalDecisions.Web.Design;
using CrystalDecisions.Reporting.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
public partial class Pages_Business_Reports_policy_payment_schedule_rp : System.Web.UI.Page
{
    #region Global variables declaration
    //string policy_id = "";
    string policy_number = "";
    string str_year = "";
    string report_type = "";
    ReportViewer my_report;
    ReportDataSource rpt_datasource;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                policy_number = Session["SS_POLICY_NUMBER"] + "";
                str_year = Session["SS_YEAR"] + "";
                report_type = Session["SS_REPORT_TYPE"] + "";

                #region Design table
                DataTable tbl = new DataTable("policy_payment_schedule");
                var col = tbl.Columns;
                col.Add("no");
                col.Add("policy_number");
                col.Add("insurance_plan");
                col.Add("insured_name");
                col.Add("sum_insured");
                col.Add("effective_date");
                col.Add("mode_of_payment");
                col.Add("year");
                col.Add("time");
                col.Add("due_date");
                col.Add("premium");
                col.Add("discount");
                col.Add("premium_after_discount");
                col.Add("extra_premium");
                col.Add("total_premium");
                col.Add("remarks");
                #endregion

                #region bind data into report
                my_report = new ReportViewer();
                my_report.Reset();
                my_report.LocalReport.DataSources.Clear();

                //report data source
                List<bl_policy_payment_schedule> policy_payment_schedule_list = new List<bl_policy_payment_schedule>();
                policy_payment_schedule_list = da_policy_payment_schedule.GetPolicyPaymentScheduleDetails(policy_number, str_year);

                if (policy_payment_schedule_list.Count > 0)
                {
                    bl_policy_detail policy = new bl_policy_detail();
                    var payment_schedule = policy_payment_schedule_list[0];
                  
                    policy = da_policy.GetPolicyDetail(payment_schedule.Policy_ID.ToString());
                   
                    #region Rider
                    //get assure year and premium for riders
                    rider myrider = new rider();
                    int level = 0;
                    int assure_year = 0;
                    double premium = 0;

                    DataTable my_data = new DataTable();
                    my_data = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Benefits_Premium", new string[,] { { "@PolicyID", payment_schedule.Policy_ID.ToString() } });
                    DataSet dsFlatRate = new DataSet();
                    if (my_data.Rows.Count == 0)
                    {
                        //get flat rate data
                        dsFlatRate = DataSetGenerator.GetDataSet("SP_GENERATE_POLICY_FLAT_RATE_REPORT", new string[,] { { "@POLICY_ID", payment_schedule.Policy_ID.ToString() }, { "@POLICY_STATUS_ID", "2" } });
                        my_data = dsFlatRate.Tables[2];
                    }

                    foreach(DataRow r in my_data.Select("level >1"))//filter riders
                    {
                        level = Convert.ToInt32(r["level"].ToString());
                        assure_year = Convert.ToInt32(r["assure_year_rider"].ToString());
                        premium = Convert.ToDouble(r["premium_rider"].ToString());
                        myrider.AddRider(new RiderPro() { AssuredYear = assure_year, Level = level, Premium = premium });
                        
                    }
                    #endregion

                    if (policy.Policy_Number == "" || policy.Policy_Number==null)
                    {
                        if (my_data.Rows[0]["policy_number"].ToString().Trim() != "")
                        {
                            var row = dsFlatRate.Tables[0].Rows[0];//policy owner detial
                            policy.Policy_Number = row["policy_number"].ToString();
                            policy.Effective_Date = Convert.ToDateTime( row["effective_date"].ToString());
                            policy.Last_Name = row["last_name"].ToString();
                            policy.First_Name = row["first_name"].ToString();

                            foreach (DataRow premium_row in my_data.Select("level =1"))// sum_insure life assure
                            {
                                policy.User_Sum_Insure =Convert.ToDouble( premium_row["sum_insure"].ToString());
                                break;
                            }
                                 

                        }
                        else
                        {
                            policy.Policy_Number = policy_number;//credit life policy number
                        }
                    }

                    //string sale_agent_id = da_policy.GetSaleAgentIDByPolicyID(payment_schedule.Policy_ID.ToString());

                    int no = 0;
                    string insurance_plan = da_product.GetProductByProductID(payment_schedule.Product_ID).En_Title.Trim();
                    string pay_mode = "";

                    #region agent info
                    string agent_info = "";
                    string agent_id = "";

                    // agent_id = da_policy.GetSaleAgentIDByPolicyID(policy.Product_ID);
                    //agent_id = policy.Sale_Agent_ID;
                    if (policy.Sale_Agent_ID == "" || policy.Sale_Agent_ID == null)
                    {
                        agent_id = dsFlatRate.Tables[0].Rows[0]["Sale_Agent_ID"].ToString();
                    }
                    else
                    {
                        agent_id = policy.Sale_Agent_ID;
                    }
                    int count = da_sale_agent.GetAllSaleAgent().Count;
                    foreach (bl_sale_agent_all obj_agent in da_sale_agent.GetAllSaleAgent().Where(obj_agent => obj_agent.Sale_Agent_ID == agent_id))
                    {
                        //agent_info = "Agent's name: " + obj_agent.Full_Name_KH + "                            Tel: " + obj_agent.Mobile_Phone;
                        //display english name instead of khmer name <updated: 05072017 by: maneth requested by:UW>
                        agent_info = "Agent's name: " + obj_agent.Full_Name_EN + "                            Tel: " + obj_agent.Mobile_Phone;
                        break;
                    }

                    //switch (da_policy.GetPolicyPayMode(payment_schedule.Policy_ID.ToString()).Pay_Mode)
                    //{
                    //    case 0: //Single
                    //        pay_mode = "Single";
                    //        break;
                    //    case 1: //Annually
                    //        pay_mode = "Annually";
                    //        break;
                    //    case 2:
                    //        pay_mode = "Semi-annually";
                    //        break;
                    //    case 3:
                    //        pay_mode = "Quarterly";
                    //        break;
                    //    case 4:
                    //        pay_mode = "Monthly";
                    //        break;
                    //}

                    pay_mode = da_policy.GetPaymentModeText(payment_schedule.Pay_Mode);

                    #endregion

                    #region Add data into datatable 
                    foreach (bl_policy_payment_schedule sch in policy_payment_schedule_list)
                    {
                        DataRow row;
                        row = tbl.NewRow();
                        row["no"] = no + 1;
                        row["policy_number"] = policy.Policy_Number;
                        row["insurance_plan"] = insurance_plan;
                        row["insured_name"] = policy.Last_Name + " " + policy.First_Name;
                        row["sum_insured"] = policy.User_Sum_Insure;
                        row["effective_date"] = policy.Effective_Date;
                        row["mode_of_payment"] = pay_mode;
                        row["year"] = sch.Year;
                        row["time"] = sch.Time;
                        row["due_date"] = sch.Due_Date;
                        row["premium"]=sch.Premium;
                        row["discount"] = sch.Discount;
                        row["premium_after_discount"] = sch.Premium_After_Discount;
                        row["extra_premium"] = sch.Extra_Premium;
                        row["total_premium"] = sch.Total_Premium;
                        row["remarks"] = sch.Created_Note;

                        tbl.Rows.Add(row);
                        no += 1;
                    }
                    #endregion

                    //minus rider premium if assure year of rider is less than assure plan
                    if (myrider.ListRider().Count > 0)
                    {
                        foreach (RiderPro b in myrider.ListRider())
                        {
                            foreach(DataRow r in tbl.Rows)
                            {
                                if (Convert.ToInt32(r["year"].ToString()) > b.AssuredYear)
                                {
                                    r["premium"] = Convert.ToDouble(r["premium"].ToString()) - b.Premium;
                                    r["premium_after_discount"] = Convert.ToDouble(r["premium_after_discount"].ToString()) - b.Premium;
                                    r["total_premium"] = Convert.ToDouble(r["total_premium"].ToString()) - b.Premium;

                                }
                            }
                        }
                    }

                    #region //Microsoft report
                    if (report_type == "1")
                    {
                        rpt_datasource = new ReportDataSource("ds_policy_payment_schedule", tbl);
                        my_report.LocalReport.DataSources.Add(rpt_datasource);

                        //report path
                        my_report.LocalReport.ReportPath = Server.MapPath("policy_payment_schedule_rr.rdlc");

                        #region initialize parameter into report
                        ReportParameter[] paras = new ReportParameter[]{
                        new ReportParameter ("Report_Title", "Payment Schedule"),
                        new ReportParameter ("Position","Life Operation Department"),
                        new ReportParameter("Agent_Info",agent_info)
                    };

                        my_report.LocalReport.SetParameters(paras);
                        #endregion
                        //refresh
                        my_report.LocalReport.Refresh();

                        //export to pdf
                        Report_Generator.ExportToPDF(this.Context, my_report, "Policy_payment_schedule" + DateTime.Now.ToString("yyyyMMddhhmmss"), true);

                    }
                    #endregion
                    #region//crystal report
                    else if (report_type == "2")
                    {
                        ReportDocument rpt = new ReportDocument();
                        rpt.Load(Server.MapPath("policy_payment_schedule.rpt"));

                        rpt.SetDataSource(tbl);

                        rpt.SetParameterValue("ReportTitle", "Payment Schedule");
                        rpt.SetParameterValue("Position", "Life Operation Department");
                        rpt.SetParameterValue("Agent_Info", agent_info);

                        string path = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
                        string fileName = DateTime.Now.ToString("yyyymmddhhmmss");
                        string fullPath = path + fileName + ".pdf";
                        Report_Generator.CrystallToPDF(rpt, fullPath);

                        Report_Generator.ReadPDF(this.Context, fullPath);
                    }
                    #endregion
                    //save printed report
                    da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(policy_payment_schedule_list[0].Policy_ID, DateTime.Now, Membership.GetUser().UserName, "PolicySchedule"));
                }
                else
                {
                    message.InnerHtml = "Record(s) not found.";
                    return;
                }

               
                #endregion

            }

        }
        catch(Exception ex)
        {
            message.InnerHtml = "Report load fail." + ex.Message;
        }
    }
}

class rider
{
    private  List<RiderPro> riderList = new List<RiderPro>();

    public void AddRider(RiderPro rider)
    {
        riderList.Add(rider);
    }

    public  List<RiderPro> ListRider()
    {
        return riderList;
    }
}
class RiderPro
{
    public int Level { get; set; }
    public double Premium { get; set; }
    public int AssuredYear { get; set; }
}
