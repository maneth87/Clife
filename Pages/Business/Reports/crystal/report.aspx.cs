using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Design;
using CrystalDecisions.Web.Design;
using CrystalDecisions.Reporting.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.IO;
using System.Web.Security;

public partial class Pages_Business_Reports_crystal_report : System.Web.UI.Page
{
    string policy_number = "";
    string str_year = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            policy_number = "1796";// Session["SS_POLICY_NUMBER"] + "";
            str_year = "1";// Session["SS_YEAR"] + "";

            /*
            DataTable tbl = new DataTable("tbl_Test");
            var col = tbl.Columns;
            col.Add("No");
            col.Add("Name");
            DataRow row;
            row = tbl.NewRow();
            row["No"] = "1";
            row["Name"] = "Maneth";
            tbl.Rows.Add(row);

            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("report1.rpt"));
            rd.SetDataSource(tbl);
            
            ////rd.ExportToDisk(ExportFormatType.PortableDocFormat,Server.MapPath(DateTime.Now.ToString("yyyymmddhhmmss")+ "test.pdf"));
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/PDF";
            //Response.AddHeader("Content-Disposition", "inline; filename=list.pdf");
            //MemoryStream mem;// = new MemoryStream();
            //Stream s = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //var sr = new BinaryReader(s);

            //mem = new MemoryStream (sr.ReadBytes((int) s.Length));
       
            //Response.BinaryWrite(mem.ToArray());
            //Response.Flush();
            //Response.Close();

           // Report_Generator.CrystallToPDF(this.Context, rd, true, DateTime.Now.ToString("yyyymmddhhmmss"));

          //  Report_Generator.CrystallToPDF(this.Context, rd);
           // Report_Generator.CrystallToPDF(this.Context, rd, true, "testing.pdf");

            //CrystalReportViewer2.ReportSource = rd;
            //CrystalReportViewer2.Visible = true;
            string path = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
            string fileName = DateTime.Now.ToString("yyyymmddhhmmss");
            string fullPath= path + fileName + ".pdf";
           // Response.Write(path + "\\test.pdf");
            Report_Generator.CrystallToPDF( rd, fullPath);

            Report_Generator.ReadPDF(this.Context, fullPath);

            */



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

            //report data source
            List<bl_policy_payment_schedule> policy_payment_schedule_list = new List<bl_policy_payment_schedule>();
            policy_payment_schedule_list = da_policy_payment_schedule.GetPolicyPaymentScheduleDetails(policy_number, str_year);

            if (policy_payment_schedule_list.Count > 0)
            {
                bl_policy_detail policy = new bl_policy_detail();
                policy = da_policy.GetPolicyDetail(policy_payment_schedule_list[0].Policy_ID.ToString());
                string sale_agent_id = da_policy.GetSaleAgentIDByPolicyID(policy_payment_schedule_list[0].Policy_ID.ToString());
                int no = 0;
                string insurance_plan = da_product.GetProductByProductID(policy.Product_ID).En_Title.Trim();
                string pay_mode = "";

                #region agent info
                string agent_info = "";
                string agent_id = "";

                // agent_id = da_policy.GetSaleAgentIDByPolicyID(policy.Product_ID);
                agent_id = policy.Sale_Agent_ID;

                int count = da_sale_agent.GetAllSaleAgent().Count;
                foreach (bl_sale_agent_all obj_agent in da_sale_agent.GetAllSaleAgent().Where(obj_agent => obj_agent.Sale_Agent_ID == agent_id))
                {
                    //agent_info = "Agent's name: " + obj_agent.Full_Name_KH + "                            Tel: " + obj_agent.Mobile_Phone;
                    //display english name instead of khmer name <updated: 05072017 by: maneth requested by:UW>
                    agent_info = "Agent's name: " + obj_agent.Full_Name_EN + "                            Tel: " + obj_agent.Mobile_Phone;
                }

                #endregion

                switch (da_policy.GetPolicyPayMode(policy_payment_schedule_list[0].Policy_ID.ToString()).Pay_Mode)
                {
                    case 0: //Single
                        pay_mode = "Single";
                        break;
                    case 1: //Annually
                        pay_mode = "Annually";
                        break;
                    case 2:
                        pay_mode = "Semi-annually";
                        break;
                    case 3:
                        pay_mode = "Quarterly";
                        break;
                    case 4:
                        pay_mode = "Monthly";
                        break;
                }
                #region Rider
                //get assure year and premium for riders
                rider myrider = new rider();
                int level = 0;
                int assure_year = 0;
                double premium = 0;

                DataTable my_data = new DataTable();
                my_data = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Benefits_Premium", new string[,] { { "@PolicyID", policy_payment_schedule_list[0].Policy_ID.ToString() } });

                foreach (DataRow r in my_data.Select("level >1"))
                {
                    level = Convert.ToInt32(r["level"].ToString());
                    assure_year = Convert.ToInt32(r["assure_year_rider"].ToString());
                    premium = Convert.ToDouble(r["premium_rider"].ToString());
                    myrider.AddRider(new RiderPro() { AssuredYear = assure_year, Level = level, Premium = premium });

                }


                #endregion
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
                    row["premium"] = sch.Premium;
                    row["discount"] = sch.Discount;
                    row["premium_after_discount"] = sch.Premium_After_Discount;
                    row["extra_premium"] = sch.Extra_Premium;
                    row["total_premium"] = sch.Total_Premium;
                    row["remarks"] = sch.Created_Note;

                    tbl.Rows.Add(row);
                    no += 1;
                }

                //minus rider premium if assure year of rider is less than assure plan
                if (myrider.ListRider().Count > 0)
                {
                    foreach (RiderPro b in myrider.ListRider())
                    {
                        foreach (DataRow r in tbl.Rows)
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
                //save printed report
                da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(policy_payment_schedule_list[0].Policy_ID, DateTime.Now, Membership.GetUser().UserName, "PolicySchedule"));

                ReportDocument rd = new ReportDocument();
                rd.Load(Server.MapPath("report1.rpt"));

                rd.SetDataSource(tbl);

                rd.SetParameterValue("ReportTitle", "Payment Schedule");
                rd.SetParameterValue("Position", "Life Operation Department");
                rd.SetParameterValue("Agent_Info", agent_info);

                string path = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
                string fileName = DateTime.Now.ToString("yyyymmddhhmmss");
                string fullPath = path + fileName + ".pdf";
                Report_Generator.CrystallToPDF(rd, fullPath);

                Report_Generator.ReadPDF(this.Context, fullPath);
            
            }
            else
            {
              //  message.InnerHtml = "Record(s) not found.";
                return;
            }



           

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
