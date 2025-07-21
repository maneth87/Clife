using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using System.Data;

public partial class Pages_Business_Reports_non_forfeiture_rp : System.Web.UI.Page
{
    #region variables declaration
    ReportViewer report = new ReportViewer();
    ReportDataSource report_data_source;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            #region Variable Declaration
            DataTable tbl_non_forfieture = new DataTable();
            var t = tbl_non_forfieture.Columns;
            t.Add("policy_id");
            t.Add("policy_number");
            t.Add("policy_year");
            t.Add("cash_value");
            t.Add("pu_ime_payment");
            t.Add("pu_ime_sum_insured");
            t.Add("et_year");
            t.Add("et_day");
            t.Add("et_ime_payment");
            t.Add("et_mty_payment");

            DataRow row;
            int coverage = 10;
            double sum_insured = 0;
            string product_id = "";
            int customer_age = 0;
            int gender = -1;
            string policy_id = Request.QueryString["policy_id"] + "";
            string policy_number = "";
            string agent_name = "";
            string agent_phone = "";

            #endregion
            try
            {
                #region Get policy detail
                bl_policy_detail policy = da_policy.GetPolicyDetail(policy_id);
                sum_insured = policy.User_Sum_Insure;// policy.System_Sum_Insure;
                coverage = policy.Assure_Year;
                product_id = policy.Product_ID;
                policy_number = policy.Policy_Number;
                customer_age = policy.Age_Insure;
                if (policy.Gender == "ប្រុស")
                {
                    gender = 1;
                }
                else
                {
                    gender = 0;
                }

                da_sale_agent.GetSaleAgent_By_SaleAgentCode(policy.Sale_Agent_ID);

                #endregion

                #region Sale agent
                //get sale agent info
                DataTable tbl_agent = da_sale_agent.GetSaleAgent_By_SaleAgentCode(policy.Sale_Agent_ID);
                var r = tbl_agent.Rows[0];

                agent_name = r["last_name"].ToString() + " " + r["first_name"].ToString();

                if (r["mobile_phone1"].ToString().Trim() == "")
                {
                    agent_phone = r["home_phone1"].ToString();
                }
                else
                {
                    agent_phone = r["mobile_phone1"].ToString();
                }
                #endregion

                #region Study save package
                string[] arr_pro = product_id.Split('/');
                if (arr_pro.Length == 3)
                {
                    //all study save package use rate of normal study save by gender= male, age=39
                    gender = 1;
                    customer_age = 39;
                    //convert product id study save package to study save normal
                    product_id = "SDS" + coverage + "/" + coverage;
                }
                #endregion

                List<da_application_study_save.bl_nonforfiet_value> list_non = da_application_study_save.GetNonforfietValueList(product_id, customer_age, coverage, gender, sum_insured, policy_id, policy_number);
                if (list_non.Count > 0)
                {
                    #region Have Records
                    foreach (da_application_study_save.bl_nonforfiet_value sds in list_non)
                    {
                        row = tbl_non_forfieture.NewRow();
                        row["policy_id"] = sds.Policy_ID;
                        row["policy_number"] = sds.Policy_Number;
                        row["policy_year"] = sds.Policy_Year;
                        row["cash_value"] = sds.Cash_Value;
                        row["pu_ime_payment"] = sds.PU_IME_Payment;
                        row["pu_ime_sum_insured"] = sds.PU_SI;
                        row["et_year"] = sds.ET_Year;
                        row["et_day"] = sds.ET_Day;
                        row["et_ime_payment"] = sds.ET_IME_Payment;
                        row["et_mty_payment"] = sds.ET_Maturity;
                        tbl_non_forfieture.Rows.Add(row);
                    }

                    ReportParameter[] paras = new ReportParameter[] { 
        
            new ReportParameter("Header1","Non-forfeiture value Table"),
            new ReportParameter("Header2","Study Save (Package Protection USD " + Helper.FormatDec(sum_insured) + ")"),
            new ReportParameter("Header3", coverage + " Years Coverage"),
            new ReportParameter("Agent_Name",agent_name),
            new ReportParameter("Agent_Phone",agent_phone)


        };

                    report.Reset();

                    report_data_source = new ReportDataSource("ds_non_forfeiture", tbl_non_forfieture);
                    report.LocalReport.DataSources.Add(report_data_source);
                    report.LocalReport.ReportPath = Server.MapPath("non_forfeiture_rr.rdlc");
                    report.LocalReport.SetParameters(paras);
                    report.LocalReport.Refresh();
                    Report_Generator.ExportToPDF(this.Context, report, "", false);

                    //save printed reports
                    da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(policy_id, DateTime.Now, System.Web.Security.Membership.GetUser().UserName, "NonForfeiture"));
                }
                    #endregion


                else
                {
                    div_message.InnerText = "Record(s) not found.";
                }

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [page load] in page [non_forfeiture_rep.aspx.cs], Detail: " + ex.Message);
                div_message.InnerText = "Load report error.";
            }
        }
       
    }
}