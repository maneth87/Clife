using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Security;

public partial class Pages_Business_Reports_co_inform_letter : System.Web.UI.Page
{
    ReportViewer rpt = new ReportViewer();
    ReportDataSource report_source;
    MYSESSION mysession;
    DateTime DeadLine = new DateTime();
    double TotalPremium = 0;
    double co_amount = 0;
    double premium = 0;
    string customer_name = "";
    string Address = "";
    string Phone = "";


    int DocDay = 0;
    int DocYear = 0;
    string DocMonth = "";

    int DeadDay = 0;
    int DeadYear = 0;
    string DeadMonth = "";
    string AgentInfo = "";
    string title = "";

    DataTable tbl;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {

                // MYSESSION mysession = new MYSESSION();
                mysession = new MYSESSION();
                //Session["SS_APP_ID"] = "BA865A72-CF59-432E-8634-4ACADFB05A10";
                //Session["SS_DOCUMENT_DATE"] = DateTime.Today;
                //Session["SS_WRITER_NAME"] = "រិច​ ដារ៉ង់";
                //Session["SS_WRITER_POSITION"] = "មន្ដ្រីប្រតិបត្ដិវិភាគហានិភ័យមិនមែនវេជ្ជសាស្រ្ដ";
                //Session["SS_DOCUMENT_NUMBER"] = "UW-CO005/2017";

                mysession.DocumentDate = Convert.ToDateTime(Session["SS_DOCUMENT_DATE"]);
                mysession.WriterName = Session["SS_WRITER_NAME"] + "";
                mysession.WriterPosition = Session["SS_WRITER_POSITION"] + "";
                mysession.DocumentNumber = Session["SS_DOCUMENT_NUMBER"] + "";
                mysession.AppID = Session["SS_APP_ID"] + "";
                mysession.Reason = Session["SS_REASON"] + "";
                mysession.AppNumber = Session["SS_APP_NUMBER"] + "";



                //document date
                DocDay = mysession.DocumentDate.Day;
                DocMonth = Helper.Get_month_in_khmer(mysession.DocumentDate.Month);
                DocYear = mysession.DocumentDate.Year;

                //dead line
                DeadLine = mysession.DocumentDate.AddDays(30);
                DeadDay = DeadLine.Day;
                DeadMonth = Helper.Get_month_in_khmer(DeadLine.Month);
                DeadYear = DeadLine.Year;



                tbl = new DataTable();
                tbl.Columns.Add("App_Number");
                tbl.Columns.Add("Co_Number");
                tbl.Columns.Add("system_premium");
                tbl.Columns.Add("em_amount");
                tbl.Columns.Add("coverage_year");
                tbl.Columns.Add("pay_year");
                tbl.Columns.Add("Product_Name_Kh");
                tbl.Columns.Add("sum_insured");


                rpt.Reset();
                rpt.LocalReport.DataSources.Clear();
                report_source = new ReportDataSource("co_inform_letter", tbl);


                rpt.LocalReport.DataSources.Add(report_source);
                rpt.LocalReport.ReportPath = Server.MapPath("co_inform_letter_rr.rdlc");
                //test font content.ttf
                // rpt.LocalReport.ReportPath = Server.MapPath("co_inform_letter_rr_content.rdlc");
                if (mysession.AppID.Trim()!="")
                {
                #region premium

                bl_underwriting_co objCO = da_underwriting.GetUnderwritingCOByAppID(mysession.AppID);

                bl_app_single_row_data app = da_application.GetAppSingleRowData(mysession.AppID);

                co_amount = objCO.EM_Amount;//da_underwriting.GetUWCOExtraAmount(AppID);
                premium = objCO.System_Premium;
                TotalPremium = co_amount + premium - app.Discount_Amount;

                DataRow row = tbl.NewRow();
                row[0] = app.App_Number;
                row[1] = "";
                row[2] = premium - app.Discount_Amount ;
                row[3] = co_amount;
                row[4] = app.Assure_Year;
                row[5] = app.Pay_Year;
                row[6] = da_product.GetProductByProductID(app.Product_ID).Kh_Title.Trim();
                row["sum_insured"] = app.System_Sum_Insure;
                tbl.Rows.Add(row);

                #endregion

                    #region customer info
                    if (app.Khmer_First_Name != "" && app.Khmer_Last_Name != "" && app.Khmer_Last_Name != null && app.Khmer_First_Name != null)
                    {
                        customer_name = app.Khmer_Last_Name + " " + app.Khmer_First_Name;

                    }
                    else
                    {
                        customer_name = app.Last_Name + " " + app.First_Name;
                    }


                    if (app.Gender == 1)
                    {
                        title = "លោក";
                    }
                    else
                    {
                        //get marital status from application form only life assure.
                        DataTable tbl_person = new DataTable();
                        tbl_person = da_application_fp6.GetDataTable("SP_Get_App_Info_Person_FP6_By_App_Register_ID", mysession.AppID);
                        string marital_status = "";

                        foreach (DataRow r_person in tbl_person.Select("Level=1"))
                        {
                            //if (r_person["level"].ToString().Trim() == "1")
                            //{
                            marital_status = r_person["marital_status"].ToString().Trim().ToUpper();
                            //    break;
                            //}
                        }

                        if (marital_status == "SINGLE")
                        {
                            title = "កញ្ញា";
                        }
                        else
                        {
                            title = "លោកស្រី";
                        }
                    }

                    customer_name = title + " " + customer_name;

                    Address = app.Address1 + System.Environment.NewLine;

                    if (app.Address2 == "")
                    {
                        Address += app.Province;
                    }
                    else
                    {
                        Address += app.Address2 + " " + app.Province;
                    }

                    Phone = app.Mobile_Phone1;
                    #endregion

                    #region agent info


                    foreach (bl_sale_agent_all agent in da_sale_agent.GetAllSaleAgent().Where(agent => agent.Sale_Agent_ID == app.Sale_Agent_ID))
                    {
                        //AgentInfo = "ភ្នាក់ងារឈ្មោះ ៖ " + agent.Full_Name_EN + "         លេខទូរសព្ទ ៖ " + agent.Mobile_Phone;
                        AgentInfo = "ភ្នាក់ងារឈ្មោះ ៖ " + agent.Full_Name_KH + "         លេខទូរសព្ទ ៖ " + agent.Mobile_Phone;
                    }


                    #endregion

                    string str = DocDay.ToString("00");

                    ReportParameter[] paras = new ReportParameter[] { 
        //"ដោយសារតែភាពមិនប្រក្រតីនៃទម្ងន់ធៀបជាមួយនឹងគីឡូរបស់លោកអ្នក (abnormal of Body Mass Index)។"
            new ReportParameter("Reason", mysession.Reason + "។" ),
            new ReportParameter ("TotalPremium", TotalPremium+""),
            new ReportParameter("CustomerName",customer_name),
            new ReportParameter("AgentInfo",AgentInfo),
            new ReportParameter("Doc_Number",mysession.DocumentNumber),
            new ReportParameter("Address",Address),
            new ReportParameter("Phone",Phone),
            new ReportParameter("DocDay",DocDay.ToString("00")),
            new ReportParameter("DocMonth",DocMonth),
            new ReportParameter("DocYear",DocYear+""),
            new ReportParameter("DeadDay",DeadDay.ToString("00")),
            new ReportParameter("DeadMonth",DeadMonth),
            new ReportParameter("DeadYear",DeadYear+""),
            new ReportParameter("WriterName",mysession.WriterName),
            new ReportParameter("WriterPosition",mysession.WriterPosition)
            };

                    rpt.LocalReport.SetParameters(paras);
                    rpt.LocalReport.Refresh();
                    Report_Generator.ExportToPDF(this.Context, rpt, "", false);

                    //save printed records: for co letter column policy id store app id 
                    da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(mysession.AppID, DateTime.Now, Membership.GetUser().UserName, "COLetter"));

                }
                else
                {
                    FlatRateCO();
                }

            }
           
            catch (Exception ex)
            {
                message.InnerHtml = "Report load error.";
                // throw ex;
                Log.AddExceptionToLog("Error [Page_Load] in page [co_inform_letter.aspx.cs], Detail: " + ex.Message);

            }
        }

        }
   

    void FlatRateCO()
    { 
        try
        {

            //document date
            DocDay = mysession.DocumentDate.Day;
            DocMonth = Helper.Get_month_in_khmer(mysession.DocumentDate.Month);
            DocYear = mysession.DocumentDate.Year;

            //dead line
            DeadLine = mysession.DocumentDate.AddDays(30);
            DeadDay = DeadLine.Day;
            DeadMonth = Helper.Get_month_in_khmer(DeadLine.Month);
            DeadYear = DeadLine.Year;
            
            rpt.Reset();
            rpt.LocalReport.DataSources.Clear();
            report_source = new ReportDataSource("co_inform_letter", tbl);


            rpt.LocalReport.DataSources.Add(report_source);
            rpt.LocalReport.ReportPath = Server.MapPath("co_inform_letter_rr.rdlc");
            //test font content.ttf
            // rpt.LocalReport.ReportPath = Server.MapPath("co_inform_letter_rr_content.rdlc");

            #region premium
           List<bl_policy_flat_rate> arr_obj= da_policy_flat_rate.GetPolicyByParameters(" APP_NUMBER='" + mysession.AppNumber + "'");
           foreach (bl_policy_flat_rate obj in arr_obj)
           {
               co_amount = obj.ExtraPremiumByMode;
               premium = obj.PremiumByMode;
               TotalPremium = co_amount + premium - obj.Discount;

               DataRow row = tbl.NewRow();
               row[0] = mysession.AppNumber;
               row[1] = "";
               row[2] = premium;
               row[3] = co_amount;
               row[4] = obj.AssuredYear;
               row[5] = obj.PayYear;
               row[6] = da_product.GetProductByProductID(obj.ProductID).Kh_Title.Trim();
               row["sum_insured"] = obj.SumInsured;
               tbl.Rows.Add(row);
               //get customer infomation
               foreach (bl_customer customer in da_customer.GetCustomerListByParameters("", "", "", obj.CustomerID.Trim(), ""))
               {
                   if (customer.Khmer_First_Name.Trim() != "" && customer.Khmer_Last_Name.Trim() != "" && customer.Khmer_First_Name != null && customer.Khmer_Last_Name != null)
                   {
                       customer_name = customer.Khmer_Last_Name + " " + customer.Khmer_First_Name;
                   }
                   else
                   {
                       customer_name = customer.Last_Name + " " + customer.First_Name;
                   }
                   if (customer.Gender == 1)
                   {

                       title = "លោក";
                   }
                   else
                   {

                       if (customer.Marital_Status.ToUpper() == "SINGLE")
                       {
                           title = "កញ្ញា";
                       }
                       else
                       {
                           title = "លោកស្រី";
                       }
                   }

                   customer_name = title + " " + customer_name; // full title and name
                   Address = customer.Address + System.Environment.NewLine;
                  

                   string khan_name = "";
                   string sangkat_name = "";
                   string province_name = "";
                   //get khan name.
                   khan_name = da_customer.GetKhanName(customer.Khan);
                  //get sangkat name
                   sangkat_name = da_customer.GetSangkatName(customer.Sangkat);

                   da_province pro = new da_province();
                   province_name = pro.GetProvinceName(customer.Province);
                   
                   Address += khan_name + " " + sangkat_name + " " + province_name;
                   //get contact list
                   foreach (bl_contact contact in da_customer.GetContactList(obj.CustomerID))
                   {
                       Phone = contact.Mobile;
                       break;
                   }

               }

               #region agent info
               foreach (bl_sale_agent_all agent in da_sale_agent.GetAllSaleAgent().Where(agent => agent.Sale_Agent_ID == obj.SaleAgentID))
               {
                   //AgentInfo = "ភ្នាក់ងារឈ្មោះ ៖ " + agent.Full_Name_EN + "         លេខទូរសព្ទ ៖ " + agent.Mobile_Phone;
                   AgentInfo = "ភ្នាក់ងារឈ្មោះ ៖ " + agent.Full_Name_KH + "         លេខទូរសព្ទ ៖ " + agent.Mobile_Phone;
               }


               #endregion
               break;
           }

            #endregion
           

            string str = DocDay.ToString("00");

            ReportParameter[] paras = new ReportParameter[] { 
        //"ដោយសារតែភាពមិនប្រក្រតីនៃទម្ងន់ធៀបជាមួយនឹងគីឡូរបស់លោកអ្នក (abnormal of Body Mass Index)។"
            new ReportParameter("Reason", mysession.Reason + "។" ),
            new ReportParameter ("TotalPremium", TotalPremium+""),
            new ReportParameter("CustomerName",customer_name),
            new ReportParameter("AgentInfo",AgentInfo),
            new ReportParameter("Doc_Number",mysession.DocumentNumber),
            new ReportParameter("Address",Address),
            new ReportParameter("Phone",Phone),
            new ReportParameter("DocDay",DocDay.ToString("00")),
            new ReportParameter("DocMonth",DocMonth),
            new ReportParameter("DocYear",DocYear+""),
            new ReportParameter("DeadDay",DeadDay.ToString("00")),
            new ReportParameter("DeadMonth",DeadMonth),
            new ReportParameter("DeadYear",DeadYear+""),
            new ReportParameter("WriterName",mysession.WriterName),
            new ReportParameter("WriterPosition",mysession.WriterPosition)
            };

            rpt.LocalReport.SetParameters(paras);
            rpt.LocalReport.Refresh();
            Report_Generator.ExportToPDF(this.Context, rpt, "", false);

            //save printed records: for co letter column policy id store app id 
            da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(mysession.AppID, DateTime.Now, Membership.GetUser().UserName, "COLetter"));

        }
        catch (Exception ex)
        {
            message.InnerHtml = "Report load error.";
            // throw ex;
            Log.AddExceptionToLog("Error function [FlatRateCO] in page [co_inform_letter.aspx.cs], Detail: " + ex.Message);

        }
    }

}
class MYSESSION
{
    public string WriterName { get; set; }
    public string WriterPosition { get; set; }
    public DateTime DocumentDate { get; set; }
    public string DocumentNumber { get; set; }
    public string AppID { get; set; }
    public string Reason { get; set; }
    public string AppNumber { get; set; }
}