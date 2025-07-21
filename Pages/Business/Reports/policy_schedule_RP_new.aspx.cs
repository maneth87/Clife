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

public partial class Pages_Business_Reports_policy_schedule_RP : System.Web.UI.Page
{
    
    string policy_id = "";
    string name_kh = "";
    string name_en = "";
    string position_kh = "";
    string position_en = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            policy_id = Request.QueryString["policy_id"];
           // policy_id = "7AA14279-3471-486E-8B43-BA7E8C9EEF0D";
            //approver info
            name_en = Session["NAME_EN"].ToString();
            name_kh = Session["NAME_KH"].ToString();
            position_en = Session["POSITION_EN"].ToString();
            position_kh = Session["POSITION_KH"].ToString();

            //Response.Write("name en=" + name_en + " name kh =" + name_kh + " position kh=" + position_kh + " position en=" + position_en);
            //return;
            if (policy_id != null && policy_id != "null" && policy_id != "")
            {


                show_report();
                //FlatRateReport();

                //save printed report
                da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(policy_id, DateTime.Now, Membership.GetUser().UserName, "PolicySchedule"));
            }
            else
            {
                message.InnerHtml = "Report is not found.";

            }
        }
        catch (Exception ex)
        {
            message.InnerHtml = "Page load error.";
            Log.AddExceptionToLog("Error in page [policy_schedule_RP.aspx.cs], Detail: " + ex.Message);
        }
    }
     
    private string GetPaymentModeInKhmer(int pay_mode)
    {
        string pay_mode_khmer = "";
        try
        {
            if (pay_mode == 0)//single
            {
                pay_mode_khmer = "បង់តែម្តង";
               
            }
            else if (pay_mode == 1)//annually
            {
                pay_mode_khmer = "ប្រចាំឆ្នាំ";
                
            }
            else if (pay_mode == 2)//semi-annul
            {
                pay_mode_khmer = "ប្រចាំឆមាស";
               
            }
            else if (pay_mode == 3)//quarterly
            {
                pay_mode_khmer = "ប្រចាំត្រីមាស";
               
            }
            else if (pay_mode == 4)//monthly
            {
                pay_mode_khmer = "ប្រចាំខែ";
                
            }
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPaymentModeInKhmer] in page [policy_schedule_RP.aspx.cs], Detail: " + ex.Message);
        }
        return pay_mode_khmer;
    }
    private string GetNewDateFormat(DateTime date)
    {
        //format date to dd-mm-yyyy
        string date_formated = "";
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";
        date = Convert.ToDateTime(date, dtfi);

        try
        {
            string[] str;
            char a = '/';
            str = date.ToShortDateString().Split(a);
            if (str.Length > 0)
            {
                date_formated = str[1] + "-" + str[0] + "-" + str[2];
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetNewDateFormat] in page [policy_schedule_RP.aspx.cs], Datail: " + ex.Message);
        }
        return date_formated;
    }

    void show_report()
    {
    #region block code
    /*
            try
            {
            
                //reset report
                my_report_view.Reset();

                //datasource
                DataTable my_data = new DataTable();
                DataTable tbl_benefit = new DataTable();

                my_data = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID", new string[,] { { "@PolicyID", policy_id } });
                ReportDataSource report_source = new ReportDataSource("policy_schedule", my_data);

                my_report_view.LocalReport.DataSources.Clear();
                my_report_view.LocalReport.DataSources.Add(report_source);

                //benefit datasource
            
                tbl_benefit = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Beneficiary", new string[,] { { "@PolicyID", policy_id } });
                ReportDataSource ds = new ReportDataSource("DataSet1", tbl_benefit);
                my_report_view.LocalReport.DataSources.Add(ds);

                //report path
                my_report_view.LocalReport.ReportPath = Server.MapPath("policy_schedule_main.rdlc");

                //parameters
                string maturity_date, effective_date, mode_of_payment, premium_due_date, plan_name;
                maturity_date = "";
                effective_date = "";
                mode_of_payment = "";
                premium_due_date = "";
                plan_name = "";
                double maturity_value = 0;
                int policy_owner_age = 0;

                if (my_data.Rows.Count > 0)
                {
                    int day, month;
                    DataRow row = my_data.Rows[0];
                    int pay_mode = -1;
                    DateTime effective;
                    int assure_year = 0;

                    pay_mode = Convert.ToInt32(row["pay_mode"].ToString().Trim());
                    mode_of_payment = Helper.GetPaymentModeInKhmer(pay_mode);
                    assure_year = Convert.ToInt32(row["assure_year"].ToString());

                    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                    dtfi.ShortDatePattern = "MM/dd/yyyy";
                    dtfi.DateSeparator = "/";

                    effective = Convert.ToDateTime(row["effective_date"].ToString(), dtfi);

                    effective_date = effective.ToShortDateString();
                    maturity_date = effective.AddYears(assure_year).ToShortDateString();

                    //Response.Write(effective + "   " + DateTime.Now.ToShortDateString());
                    //return;

                    day = effective.Day;
                    month = effective.Month;

                    DateTime policy_owner_birth_date;
                    policy_owner_birth_date = Convert.ToDateTime(row["Birth_Date1"].ToString());
                    policy_owner_age = Calculation.Culculate_Customer_Age(policy_owner_birth_date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), Convert.ToDateTime(row["effective_date"].ToString(), dtfi));


                    string day_format = day.ToString("00");

                    //year = effective_date.Year;
                    //maturity date = effective date + assured year
                    if (pay_mode == 0)//single
                    {
                        //premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month);
                        premium_due_date = "-";
                        //pay_mode = "បង់តែម្តង";
                    }
                    else if (pay_mode == 1)// annually
                    {
                        premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month);

                        //pay_mode = "ប្រចាំឆ្នាំ";
                    }
                    else if (pay_mode == 2)//semi
                    {
                        premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6);

                        //pay_mode = "ប្រចាំឆមាស";
                    }
                    else if (pay_mode == 3)//quarterly
                    {
                        //premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ​ " + day_format + " " + da_policy.GetMonthName(month + 3) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 9) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 12);
                        premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ​ " + day_format + " " + da_policy.GetMonthName(month + 3) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 9);

                        //pay_mode = "ប្រចាំត្រីមាស";
                    }
                    else if (pay_mode == 4)//monthly
                    {
                        premium_due_date = "ថ្ងៃ " + day_format + " នៃខែនីមួយៗ";
                        //pay_mode = "ប្រចាំខែ";
                    }

                    //plan_name = "គម្រោងសុវត្ថិភាពគ្រួសារខ្ញុំ";

                    bl_product pro = da_product.GetProductByProductID(row["product_id"].ToString());
                    plan_name = pro.Kh_Title;

                    #region Maturity Calculation
                
                    bl_product_type pro_typ = new bl_product_type();
                    pro_typ = da_product.GetProductTypeByProductID(row["product_id"].ToString());

                    if (pro_typ.Product_Type.ToUpper().Trim() == "SAVINGS")
                    {
                        //fomular maturity_value = (annual basic life premium without discount * 115% * coverage period)
                        DataTable tbl_premium_detail = new DataTable();
                        tbl_premium_detail = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Benefits_Premium", new string[,] { { "@PolicyID", policy_id } });
                        foreach (DataRow r in tbl_premium_detail.Rows)
                        {
                            if (r["level"].ToString() == "1") //basic life
                            {
                                //<0606207 by maneth>
                                // percent_val base on coverage year
                                //10 years = 110%
                                //12 years = 112%
                                //15 years = 115%
                                double percent_val = 0.0;
                                int assure_y = 0;
                                assure_y = Convert.ToInt32(r["assure_year"].ToString());
                                if (assure_y == 10)
                                {
                                    percent_val = 1.1;//110%
                                }
                                else if (assure_y == 12)
                                {
                                    percent_val = 1.12;//112%
                                }
                                else if (assure_y == 15)
                                {
                                    percent_val = 1.15;//115%
                                }
                                maturity_value = (Math.Ceiling(Convert.ToDouble(r["original_amount"].ToString())) * percent_val * Convert.ToInt32(r["assure_year"].ToString()));
                                //maturity_value = (Math.Ceiling( Convert.ToDouble(r["original_amount"].ToString())) * 1.15 * Convert.ToInt32(r["assure_year"].ToString()));
                                // //<End 0606207 by maneth>

                                maturity_value = Math.Round(maturity_value, 0, MidpointRounding.AwayFromZero);
                                break;
                            }
                        }
                    }
                    else
                    {
                        maturity_value = 0.0;
                    }

                    #endregion
                }
                else
                {
                    FlatRateReport();
                    return;
                }

                //plan_name = "គម្រោងសុវត្ថិភាពគ្រួសារខ្ញុំ";
                //Get issue date
                DateTime issue_date;
                issue_date = da_policy.GetIssueDate(policy_id);
           
                ReportParameter[] paras = new ReportParameter[] { 
        
                new ReportParameter("Report_Title1","តារាងបណ្ណសន្យារ៉ាប់រង POLICY SCHEDULE"),
                 new ReportParameter("Report_Title2",""),
                  new ReportParameter("Report_Title3","លេខបណ្ណសន្យារ៉ាប់រង Policy Number"),
                  new ReportParameter("Report_Footer","LO-IN-PS-01-03"),
                new ReportParameter("Plan_Name",plan_name),
                new ReportParameter("Maturity_Date",maturity_date),
                new ReportParameter("Due_Date",premium_due_date),
                new ReportParameter("Mode_Of_Payment", mode_of_payment),
                new ReportParameter("Issued_Date",issue_date+""),
                new ReportParameter("Name_Kh",name_kh),
                new ReportParameter("Name_En",name_en),
                new ReportParameter("Position_En",position_en),
                new ReportParameter("Position_Kh",position_kh),
                //policyowner age
                new ReportParameter("Policyowner_Age", policy_owner_age+""),
                new ReportParameter("Maturity_Value",maturity_value+"")


            };

                //Assign parameters to report
                my_report_view.LocalReport.SetParameters(paras);

                // call subreport
                my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Data_Check_List_Premium_Detail);

                //my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Data_Check_List_benefit);

                //refresh
                my_report_view.LocalReport.Refresh();

                //export to pdf

                Report_Generator.ExportToPDF(this.Context, my_report_view, "Policy_payment_schedule" + DateTime.Now.ToString("yyyyMMddhhmmss"), true);
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [show_report] in page [policy_schedule_RP_new.aspx.cs], Detail: " + ex.Message);
                message.InnerHtml = "Report is error.";
            }
    */
    #endregion
        //reset report
        try{
            my_report_view.Reset();

            //datasource
            DataTable my_data = new DataTable();
            DataTable tbl_benefit = new DataTable();
            ReportDataSource report_source;
            ReportDataSource report_source2;
            DataTable tbl_premium_detail = new DataTable();

            //use for asigning value to report parameter.
            DateTime issue_date;
            string report_title1 = "";
            string report_title2 = "";
            string report_title3 = "";
            string report_footer = "";

            //first data table for main report
            my_data = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID", new string[,] { { "@PolicyID", policy_id } });
            if (my_data.Rows.Count > 0)
            {
                //report path
                my_report_view.LocalReport.ReportPath = Server.MapPath("policy_schedule_main.rdlc");

                //second data table for main report
                tbl_benefit = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Beneficiary", new string[,] { { "@PolicyID", policy_id } });

                //this datatable use for calculate maturity values.
                tbl_premium_detail = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Benefits_Premium", new string[,] { { "@PolicyID", policy_id } });

                //report parameters values
                issue_date = da_policy.GetIssueDate(policy_id);
                report_title1 = "តារាងបណ្ណសន្យារ៉ាប់រង POLICY SCHEDULE";
                report_title2 = "";
                report_title3 = "លេខបណ្ណសន្យារ៉ាប់រង Policy Number";
                report_footer = "LO-IN-PS-01-03";

                // call subreport
                my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Data_Check_List_Premium_Detail);

            }
            else//flate rate and credit life
            {
                //Report path
                my_report_view.LocalReport.ReportPath = Server.MapPath("policy_schedule_fr_main.rdlc");

                //Credit life
                myDataSet = DataSetGenerator.GetDataSet("SP_GENERATE_POLICY_CREDIT_LIFE_REPORT", new string[,] { { "@POLICY_ID", policy_id } });

                List<bl_group_master_product> gMaster = da_group_master_product.GetGroupMasterProductList(myDataSet.Tables[0].Rows[0]["product_id"].ToString().Trim(), myDataSet.Tables[0].Rows[0]["channel_item_id"].ToString());
                //report parameters values
                issue_date = da_policy.GetIssueDate(policy_id);
                report_title1 = "តារាងបណ្ណសន្យារ៉ាប់រង POLICY SCHEDULE";
                report_title2 = "";
                report_title3 = "លេខសម្គាល់អតិថិជន Customer ID No";
                report_footer = "LO-IN-PS-01-03";

                //first data table for main report
                my_data = myDataSet.Tables[0];

                //second data table for main report
                tbl_benefit = myDataSet.Tables[1];

                //this datatable use for calculate maturity values.
                tbl_premium_detail = myDataSet.Tables[2];

                // call subreport
                my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(FlatRateSubReport);

            }

            //first dataset
            report_source = new ReportDataSource("policy_schedule", my_data);

            //clear datasource in report
            my_report_view.LocalReport.DataSources.Clear();

            //add datasource in to report
            my_report_view.LocalReport.DataSources.Add(report_source);

           //second dataset
            report_source2 = new ReportDataSource("DataSet1", tbl_benefit);

            //add second dataset into report
            my_report_view.LocalReport.DataSources.Add(report_source2);

            ////report path
            //my_report_view.LocalReport.ReportPath = Server.MapPath("policy_schedule_main.rdlc");

            //parameters
            string maturity_date, effective_date, mode_of_payment, premium_due_date, plan_name;
            maturity_date = "";
            effective_date = "";
            mode_of_payment = "";
            premium_due_date = "";
            plan_name = "";
            double maturity_value = 0;
            int policy_owner_age = 0;

            if (my_data.Rows.Count > 0)
            {
                //int day, month;
                DataRow row = my_data.Rows[0];
                int pay_mode = -1;
                DateTime effective;
                int assure_year = 0;

                pay_mode = Convert.ToInt32(row["pay_mode"].ToString().Trim());
                mode_of_payment = Helper.GetPaymentModeInKhmer(pay_mode);
                assure_year = Convert.ToInt32(row["assure_year"].ToString());

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "MM/dd/yyyy";
                dtfi.DateSeparator = "/";

                effective = Convert.ToDateTime(row["effective_date"].ToString(), dtfi);

                effective_date = effective.ToShortDateString();
                maturity_date = effective.AddYears(assure_year).ToShortDateString();

                //day = effective.Day;
                //month = effective.Month;
                //DateTime  = Convert.ToDateTime(txtDateEffectiveDate.Text, dtfi); ////To be deleted after inserting data

                //da_application.GetApplicationByAppNo(row["App_Number"].ToString());


                DateTime policy_owner_birth_date;
                policy_owner_birth_date = Convert.ToDateTime(row["Birth_Date1"].ToString());
                policy_owner_age = Calculation.Culculate_Customer_Age(policy_owner_birth_date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), Convert.ToDateTime(row["effective_date"].ToString(), dtfi));
                
                
                //string day_format = day.ToString("00");

                List<DateTime> dueDateList = Helper.GetDueDateList(effective, pay_mode);
               

                //year = effective_date.Year;
                //maturity date = effective date + assured year
                if (pay_mode == 0)//single
                {
                    //premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month);
                    premium_due_date = "-";
                    //pay_mode = "បង់តែម្តង";
                }
                else if (pay_mode == 1)// annually
                {
                    //premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month);
                    //pay_mode = "ប្រចាំឆ្នាំ";

                    if (dueDateList.Count > 0)
                    {
                        var due = dueDateList[0];
                       
                         premium_due_date += "រៀងរាល់ថ្ងៃទី " + due.Day + " " + da_policy.GetMonthName(due.Month);
                    }

                }
                else if (pay_mode == 2 || pay_mode == 3)//semi & quarter
                {
                   // premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6);

                    //pay_mode = "ប្រចាំឆមាស";
                    if (dueDateList.Count > 0)
                    {

                        foreach (DateTime due in dueDateList)
                        {
                            premium_due_date += "ថ្ងៃ " + due.Day + da_policy.GetMonthName(due.Month) + ", ";
                        }
                        if (premium_due_date.Length > 0)
                        {
                            premium_due_date = premium_due_date.Substring(0, premium_due_date.Length - 2);//cut ", "
                        }
                    }
                }
                //else if (pay_mode == 3)//quarterly
                //{
                //    //premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ​ " + day_format + " " + da_policy.GetMonthName(month + 3) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 9) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 12);
                //    premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ​ " + day_format + " " + da_policy.GetMonthName(month + 3) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 9);

                //    //pay_mode = "ប្រចាំត្រីមាស";
                //}
                else if (pay_mode == 4)//monthly
                {
                   // premium_due_date = "ថ្ងៃ " + day_format + " នៃខែនីមួយៗ";
                    //pay_mode = "ប្រចាំខែ";
                    if (dueDateList.Count > 0)
                    {
                        var due = dueDateList[0];

                        premium_due_date += "ថ្ងៃ " + due.Day + " នៃខែនីមួយៗ";
                    }
                }

                //plan_name = "គម្រោងសុវត្ថិភាពគ្រួសារខ្ញុំ";

                bl_product pro = da_product.GetProductByProductID(row["product_id"].ToString());
                plan_name = pro.Kh_Title;

                #region Maturity Calculation
                
                bl_product_type pro_typ = new bl_product_type();
                pro_typ = da_product.GetProductTypeByProductID(row["product_id"].ToString());

                if (pro_typ.Product_Type.ToUpper().Trim() == "SAVINGS")
                {
                    //fomular maturity_value = (annual basic life premium without discount * 115% * coverage period)
                    
                    foreach (DataRow r in tbl_premium_detail.Rows)
                    {
                        if (r["level"].ToString() == "1") //basic life
                        {
                            //<0606207 by maneth>
                            // percent_val base on coverage year
                            //10 years = 110%
                            //12 years = 112%
                            //15 years = 115%
                            double percent_val = 0.0;
                            int assure_y = 0;
                            assure_y = Convert.ToInt32(r["assure_year"].ToString());
                            if (assure_y == 10)
                            {
                                percent_val = 1.1;//110%
                            }
                            else if (assure_y == 12)
                            {
                                percent_val = 1.12;//112%
                            }
                            else if (assure_y == 15)
                            {
                                percent_val = 1.15;//115%
                            }
                            maturity_value = (Math.Ceiling(Convert.ToDouble(r["original_amount"].ToString())) * percent_val * Convert.ToInt32(r["assure_year"].ToString()));
                            //maturity_value = (Math.Ceiling( Convert.ToDouble(r["original_amount"].ToString())) * 1.15 * Convert.ToInt32(r["assure_year"].ToString()));
                            // //<End 0606207 by maneth>

                            maturity_value = Math.Round(maturity_value, 0, MidpointRounding.AwayFromZero);
                            break;
                        }
                    }
                }
                else
                {
                    maturity_value = 0.0;
                }

                #endregion

                ReportParameter[] paras = new ReportParameter[] { 
        
                   new ReportParameter("Report_Title1", report_title1),
                   new ReportParameter("Report_Title2",report_title2),
                   new ReportParameter("Report_Title3",report_title3),
                   new ReportParameter("Report_Footer", report_footer),
                    new ReportParameter("Plan_Name",plan_name),
                    new ReportParameter("Maturity_Date",maturity_date),
                    new ReportParameter("Due_Date",premium_due_date),
                    new ReportParameter("Mode_Of_Payment", mode_of_payment),
                    new ReportParameter("Issued_Date",issue_date+""),
                    new ReportParameter("Name_Kh",name_kh),
                    new ReportParameter("Name_En",name_en),
                    new ReportParameter("Position_En",position_en),
                    new ReportParameter("Position_Kh",position_kh),
                    //policyowner age
                    new ReportParameter("Policyowner_Age", policy_owner_age+""),
                    new ReportParameter("Maturity_Value",maturity_value+"")
                };

                //Assign parameters to report
                my_report_view.LocalReport.SetParameters(paras);

                // call subreport
                //my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Data_Check_List_Premium_Detail);

                //my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Data_Check_List_benefit);

                //refresh
                my_report_view.LocalReport.Refresh();

                //export to pdf

                Report_Generator.ExportToPDF(this.Context, my_report_view, "Policy_payment_schedule" + DateTime.Now.ToString("yyyyMMddhhmmss"), true);

            }
            else
            {
                message.InnerHtml = "Report is not found.";
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [show_report] in page [policy_schedule_RP_new.aspx.cs], Detail: " + ex.InnerException + "==>"+ ex.Message + "==>" + ex.StackTrace);
            message.InnerHtml = "Report is error.";
        }

    }

    void Data_Check_List_Premium_Detail(object sender, SubreportProcessingEventArgs e)
    {
       DataTable tbl_premium_detail = new DataTable();
       tbl_premium_detail = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Benefits_Premium", new string[,] { { "@PolicyID", policy_id } });

       //add black riders
       int intkid = 0;
       int intspouse = 0;
       //double total_premium = 0.0;
       //double total_premium_rider = 0.0;
       //double grand_total = 0.0;
       //double basic_premium = 0.0;

       foreach (DataRow row in tbl_premium_detail.Rows)
       {
           int level = 0;
           level = Convert.ToInt32(row["Level"].ToString().Trim());
           //basic_premium = Convert.ToDouble(row["Premium"].ToString()) + Convert.ToDouble(row["EM_Amount"].ToString()) - Convert.ToDouble(row["Discount_Amount"].ToString());
           if (level == 1)
           {
               //premium for life insured
               //total_premium = total_premium + Convert.ToDouble(row["Premium_Rider"].ToString()) + Convert.ToDouble(row["EM_Amount_Rider"].ToString()) - Convert.ToDouble(row["Discount_Amount_Rider"].ToString());
           }
           if (level == 2)
           {//spouse
               intspouse = intspouse + 1;
               //sum premium for rider
               //total_premium_rider = Convert.ToDouble(row["Premium_Rider"].ToString()) + Convert.ToDouble(row["EM_Amount_Rider"].ToString()) - Convert.ToDouble(row["Discount_Amount_Rider"].ToString());
           }
           else if (level > 2)
           {
               intkid = intkid + 1;
               //sum premium for rider
               //total_premium_rider = total_premium_rider + Convert.ToDouble(row["Premium_Rider"].ToString()) + Convert.ToDouble(row["EM_Amount_Rider"].ToString()) - Convert.ToDouble(row["Discount_Amount_Rider"].ToString());
           }

       }
       //grand total 
       //grand_total = basic_premium + total_premium + total_premium_rider;

       #region  add blank spouse
       //if (intspouse == 0)
       //{
       //    DataRow rowSpouse = tbl_premium_detail.NewRow();
       //    rowSpouse["Rider_Type"] = "Spouse";
       //    rowSpouse["Sum_Insure"] = 0;
       //    rowSpouse["Rider_Sum_Insured"] = 0;
       //    rowSpouse["Premium_Rider"] = 0;
       //    rowSpouse["EM_Amount"] = 0;
       //    rowSpouse["EM_Amount_Rider"] = 0;
       //    rowSpouse["Level"] = 2;
       //    rowSpouse["Discount_Amount"] = 0;
       //    rowSpouse["Discount_Amount_Rider"] = 0;
       //    tbl_premium_detail.Rows.Add(rowSpouse);
       //}

       #endregion

       #region add blank kids
       ////int kidno=0;
       ////kidno = 4 - intkid;
       //if (intkid < 4)
       //{
       //    intkid = intkid + 1;
       //    for (int i = intkid; i <= 4; i++)
       //    {
       //        DataRow rowKid = tbl_premium_detail.NewRow();
       //        rowKid["Rider_Type"] = "Kid " + i;
       //        rowKid["Sum_Insure"] = 0;
       //        rowKid["Rider_Sum_Insured"] = 0;
       //        rowKid["Premium_Rider"] = 0;
       //        rowKid["EM_Amount"] = 0;
       //        rowKid["EM_Amount_Rider"] = 0;
       //        rowKid["Level"] = i;
       //        rowKid["Discount_Amount"] = 0;
       //        rowKid["Discount_Amount_Rider"] = 0;

       //        tbl_premium_detail.Rows.Add(rowKid);
       //    }
       //}
        #endregion
       ReportDataSource ds = new ReportDataSource("policy_schedule", tbl_premium_detail);
       e.DataSources.Add(ds);

        //Data_Check_List_benefit(sender, e);

    }

    void Data_Check_List_benefit(object sender, SubreportProcessingEventArgs e)
    {
        DataTable tbl_benefit = new DataTable();

        tbl_benefit = DataSetGenerator.Get_Data_Soure("SP_GetPolicyScheduleByPolicyID_Sub_Report_Beneficiary", new string[,] { { "@PolicyID", policy_id } });

        ReportDataSource ds = new ReportDataSource("policy_schedule", tbl_benefit);
        e.DataSources.Add(ds);
    }

    #region policy flat rate
    DataSet myDataSet = new DataSet();
    void FlatRateReport()
    {
        try
        {
            my_report_view.Reset();
            //datasource
            DataTable tbl_policy_schedule = new DataTable();
            int num_tables = 0;
        
            myDataSet = DataSetGenerator.GetDataSet("SP_GENERATE_POLICY_CREDIT_LIFE_REPORT", new string[,] { { "@POLICY_ID", policy_id } });

            num_tables = myDataSet.Tables.Count;
            tbl_policy_schedule = myDataSet.Tables[0];
        
            ReportDataSource report_source = new ReportDataSource("policy_schedule", tbl_policy_schedule);

            my_report_view.LocalReport.DataSources.Clear();
            my_report_view.LocalReport.DataSources.Add(report_source);
    
            //benefit datasource
            DataTable tbl_benefit = new DataTable();
            tbl_benefit = myDataSet.Tables[1];
            ReportDataSource ds = new ReportDataSource("DataSet1", tbl_benefit);
            my_report_view.LocalReport.DataSources.Add(ds);

            //report path
            my_report_view.LocalReport.ReportPath = Server.MapPath("policy_schedule_main.rdlc");

            //parameters
            string maturity_date, effective_date, mode_of_payment, premium_due_date, plan_name;
            maturity_date = "";
            effective_date = "";
            mode_of_payment = "";
            premium_due_date = "";
            plan_name = "";
            double maturity_value = 0;
            int policy_owner_age = 0;

            if (tbl_policy_schedule.Rows.Count > 0)
            {
                int day, month;
                DataRow row = tbl_policy_schedule.Rows[0];
                int pay_mode = -1;
                DateTime effective;
                int assure_year = 0;

                pay_mode = Convert.ToInt32(row["pay_mode"].ToString().Trim());
                mode_of_payment = Helper.GetPaymentModeInKhmer(pay_mode);
                assure_year = Convert.ToInt32(row["assure_year"].ToString());

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "MM/dd/yyyy";
                dtfi.DateSeparator = "/";

                effective = Convert.ToDateTime(row["effective_date"].ToString(), dtfi);

                effective_date = effective.ToShortDateString();
                maturity_date = effective.AddYears(assure_year).ToShortDateString();

                //Response.Write(effective + "   " + DateTime.Now.ToShortDateString());
                //return;

                day = effective.Day;
                month = effective.Month;

                DateTime policy_owner_birth_date;
                policy_owner_birth_date = Convert.ToDateTime(row["Birth_Date1"].ToString());
                policy_owner_age = Calculation.Culculate_Customer_Age(policy_owner_birth_date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), Convert.ToDateTime(row["effective_date"].ToString(), dtfi));

                string day_format = day.ToString("00");

                //year = effective_date.Year;
                //maturity date = effective date + assured year
                if (pay_mode == 0)//single
                {
                    //premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month);
                    premium_due_date = "-";
                    //pay_mode = "បង់តែម្តង";
                }
                else if (pay_mode == 1)// annually
                {
                    premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month);

                    //pay_mode = "ប្រចាំឆ្នាំ";
                }
                else if (pay_mode == 2)//semi
                {
                    premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6);

                    //pay_mode = "ប្រចាំឆមាស";
                }
                else if (pay_mode == 3)//quarterly
                {
                    premium_due_date = "ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month) + ", ថ្ងៃ​ " + day_format + " " + da_policy.GetMonthName(month + 3) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 6) + ", ថ្ងៃ " + day_format + " " + da_policy.GetMonthName(month + 9);

                    //pay_mode = "ប្រចាំត្រីមាស";
                }
                else if (pay_mode == 4)//monthly
                {
                    premium_due_date = "ថ្ងៃ " + day_format + " នៃខែនីមួយៗ";
                    //pay_mode = "ប្រចាំខែ";
                }

                bl_product pro = da_product.GetProductByProductID(row["product_id"].ToString());
                plan_name = pro.Kh_Title;

                #region Maturity Calculation

                bl_product_type pro_typ = new bl_product_type();
                pro_typ = da_product.GetProductTypeByProductID(row["product_id"].ToString());

                if (pro_typ.Product_Type.ToUpper().Trim() == "SAVINGS")
                {
                    //fomular maturity_value = (annual basic life premium without discount * 115% * coverage period)
                    DataTable tbl_premium_detail = new DataTable();
                    tbl_premium_detail = myDataSet.Tables[2];
                    if (tbl_premium_detail.Rows.Count > 0)
                    {
                        //<0606207 by maneth>
                        // percent_val base on coverage year
                        //10 years = 110%
                        //12 years = 112%
                        //15 years = 115%
                        double percent_val = 0.0;
                        int assure_y = 0;
                        assure_y = Convert.ToInt32(tbl_premium_detail.Rows[0]["assure_year"].ToString());
                        if (assure_y == 10)
                        {
                            percent_val = 1.1;//110%
                        }
                        else if (assure_y == 12)
                        {
                            percent_val = 1.12;//112%
                        }
                        else if (assure_y == 15)
                        {
                            percent_val = 1.15;//115%
                        }
                        maturity_value = (Math.Ceiling(Convert.ToDouble(tbl_premium_detail.Rows[0]["annual_premium"].ToString()) * percent_val * assure_y));
                        //maturity_value = (Math.Ceiling( Convert.ToDouble(r["original_amount"].ToString())) * 1.15 * Convert.ToInt32(r["assure_year"].ToString()));
                        // //<End 0606207 by maneth>

                        maturity_value = Math.Round(maturity_value, 0, MidpointRounding.AwayFromZero);
                    }
              
                }
                else
                {
                    maturity_value = 0.0;
                }
            
                #endregion

        
            DateTime issue_date;
            string report_title1 = "";
            string report_title2 = "";
            string report_title3 = "";
            string report_footer = "";

                //check credit life
               List<bl_group_master_product> gMaster= da_group_master_product.GetGroupMasterProductList(row["product_id"].ToString().Trim(),"");
               if (gMaster.Count > 0)
               {
                   report_title1 = "វិញ្ញបនប័ត្រធានារ៉ាប់រង INSURANCE CERTIFICATE";
                   report_title2 = "លេខបណ្ណសន្យារ៉ាប់រងក្រុម Group Policy No.: " + gMaster[0].GroupCode;
                   report_title3 = "លេខ No.";
                   report_footer = "LO-IN-PS-01-04";
                   //Get issue date
                   issue_date = da_policy.GetIssueDate(policy_id);
               }
               else
               {
                   report_title1 = "តារាងបណ្ណសន្យារ៉ាប់រង POLICY SCHEDULE";
                   report_title2 = "";
                   report_title3 = "លេខបណ្ណសន្យារ៉ាប់រង Policy Number";
                   report_footer = "LO-IN-PS-01-03";
                   issue_date = Convert.ToDateTime(tbl_policy_schedule.Rows[0]["Issued_Date"].ToString());
               }

                ReportParameter[] paras = new ReportParameter[] { 
        
                new ReportParameter("Report_Title1",report_title1),
                new ReportParameter("Report_Title2",report_title2),
                new ReportParameter("Report_Title3",report_title3),
                new ReportParameter("Report_Footer",report_footer),
                new ReportParameter("Plan_Name",plan_name),
                new ReportParameter("Maturity_Date",maturity_date),
                new ReportParameter("Due_Date",premium_due_date),
                new ReportParameter("Mode_Of_Payment", mode_of_payment),
                new ReportParameter("Issued_Date",issue_date+""),
                new ReportParameter("Name_Kh",name_kh),
                new ReportParameter("Name_En",name_en),
                new ReportParameter("Position_En",position_en),
                new ReportParameter("Position_Kh",position_kh),
                //policyowner age
                new ReportParameter("Policyowner_Age", policy_owner_age+""),
                new ReportParameter("Maturity_Value",maturity_value+"")


            };

            //Assign parameters to report
            my_report_view.LocalReport.SetParameters(paras);

            // call subreport
            my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(FlatRateSubReport);

            //my_report_view.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Data_Check_List_benefit);

            //refresh
            my_report_view.LocalReport.Refresh();

            //export to pdf

            Report_Generator.ExportToPDF(this.Context, my_report_view, "Policy_payment_schedule" + DateTime.Now.ToString("yyyyMMddhhmmss"), true);
            }
            else
            {
                message.InnerHtml = "Record(S) Not Found.";
                return;
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [FlatRateReport] in page [policy_schedule_RP_new.aspx.cs], Detail: " + ex.Message + "--->" + ex.InnerException + Environment.NewLine + ex.StackTrace);
            message.InnerHtml = "Report is error.";
        }
    }
    void FlatRateSubReport(object sender, SubreportProcessingEventArgs e)
    {
        ReportDataSource ds = new ReportDataSource("policy_schedule", myDataSet.Tables[2]);
        e.DataSources.Add(ds);
    }
    #endregion

}