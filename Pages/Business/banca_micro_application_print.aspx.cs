using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
public partial class Pages_Business_banca_micro_application_print : System.Web.UI.Page
{
    protected void Page_Load_bak(object sender, EventArgs e)
    {

        try
        {
            string app_id = Request.QueryString["APP_ID"].ToString();
            ReportViewer myReport = new ReportViewer();
            ReportDataSource rdsDetail;


            DataTable tbl_app_detail = new DataTable("tbl_micro_application_form");

            tbl_app_detail = da_micro_application.GetApplicationDetailByApplicationID(app_id);

            bl_product product = new bl_product();
            bl_micro_product_rider pro_rider = new bl_micro_product_rider();
            if (tbl_app_detail.Rows.Count > 0)
            {
                product = da_product.GetProductByProductID(tbl_app_detail.Rows[0]["product_id"].ToString());
                pro_rider = da_micro_product_rider.GetMicroProductByProductID(tbl_app_detail.Rows[0]["rider_product_id"].ToString());
                string gender = tbl_app_detail.Rows[0]["gender"].ToString();
                string marital_status = tbl_app_detail.Rows[0]["marital_status"].ToString();
                string pay_mode = tbl_app_detail.Rows[0]["pay_mode"].ToString();
                string dhc = tbl_app_detail.Rows[0]["rider_sum_assure"].ToString();
                string answer = tbl_app_detail.Rows[0]["answer"].ToString();
                string answer_remarks = tbl_app_detail.Rows[0]["answer_remarks"].ToString();

                int id_type = Convert.ToInt32(tbl_app_detail.Rows[0]["id_type"].ToString());
                string id_type_en = Helper.GetIDCardTypeText(id_type);
                string id_type_kh = Helper.GetIDCardTypeTextKh(id_type);

                string address = "";
                tbl_app_detail.Rows[0]["gender"] = Helper.GetGenderText(Convert.ToInt32(tbl_app_detail.Rows[0]["gender"].ToString()), true, true);
                tbl_app_detail.Rows[0]["marital_status"] = Helper.GetMaritalStatusInKhmer(tbl_app_detail.Rows[0]["marital_status"].ToString());
                tbl_app_detail.Rows[0]["product_name"] = product.Kh_Title;
                tbl_app_detail.Rows[0]["rider_product_name"] = pro_rider.KH_TITLE;
                myReport.LocalReport.ReportPath = Server.MapPath("micro_application.rdlc");
                myReport.LocalReport.DataSources.Clear();
                rdsDetail = new ReportDataSource("ds_micro_application", tbl_app_detail);


                if (tbl_app_detail.Rows[0]["province_en"].ToString().Trim().ToUpper() == "PHNOM PENH")
                {
                    address = (tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                        (tbl_app_detail.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl_app_detail.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "សង្កាត់" + tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                        (tbl_app_detail.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ខណ្ឌ" + tbl_app_detail.Rows[0]["district_kh"].ToString().Trim());// +" " + (tbl_app_detail.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ក្រុង " + tbl_app_detail.Rows[0]["province_kh"].ToString().Trim());
                }
                else
                {
                    address = (tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                        (tbl_app_detail.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl_app_detail.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "ឃុំ" + tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                        (tbl_app_detail.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ស្រុក" + tbl_app_detail.Rows[0]["district_kh"].ToString().Trim());// +" " + (tbl_app_detail.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ខេត្ត " + tbl_app_detail.Rows[0]["province_kh"].ToString().Trim());

                }
                //ReportParameter1


                ReportParameter[] paras = new ReportParameter[] { 
                        new ReportParameter("para_male", gender),
                        new ReportParameter("para_female", gender),
                        new ReportParameter("para_single", marital_status),
                       new ReportParameter("para_married", marital_status),
                      new ReportParameter("para_window", marital_status),
                      new ReportParameter("para_windower", marital_status),
                       new ReportParameter("para_annual", pay_mode),
                       new ReportParameter("para_semi", pay_mode),
                        new ReportParameter("para_quar", pay_mode),
                        new ReportParameter("para_month", pay_mode),
                        new ReportParameter("para_dhc10", dhc),
                        new ReportParameter("para_dhc20", dhc),
                        new ReportParameter("para_dhc30", dhc),
                        new ReportParameter("para_anw_yes", answer),
                        new ReportParameter("para_anw_no", answer),
                        new ReportParameter("para_anw_remarks", answer_remarks),
                        new ReportParameter("para_address", address),
                        new ReportParameter("para_id_type_en",id_type_en),
                        new ReportParameter("para_id_type_kh",id_type_kh),

                        new ReportParameter("para_paper_type","LetterHead")
                        //new ReportParameter("para_paper_type","A4")
                        
                        };


                //Assign parameters to report
                myReport.LocalReport.SetParameters(paras);
                myReport.LocalReport.DataSources.Add(rdsDetail);
                myReport.LocalReport.Refresh();
                Report_Generator.ExportToPDF(this.Context, myReport, "micro_application" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
                //Report_Generator.Export(this.Context, myReport, "micro_application" + DateTime.Now.ToString("yyyyMMddhhmmss"), false,"EXCEL");
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[Page_Load(object sender, EventArgs e)] in page [banca_micro_application_print.aspx.cs], detail:" + ex.Message);


        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            string app_id = "";// Request.QueryString["APP_ID"].ToString();
            string appType = "";// Request.QueryString["AP_TYPE"].ToString();
            ReportViewer myReport = new ReportViewer();
            ReportDataSource rdsDetail;
            ReportDataSource rdsBen;
            if (Request.QueryString.Count == 2)
            {
                app_id = Request.QueryString["APP_ID"].ToString();
                appType = Request.QueryString["A_TYPE"].ToString();
                // DataTable tbl_app_detail = new DataTable("tbl_micro_application_form");
                // DataTable tbl_app_ben = new DataTable("tbl_micro_application_form_ben");
                DataSet dsApp = new DataSet();
                //if (appType == "IND")
                //{
                //    dsApp = GetAppForm(app_id, da_micro_application.ApplicationTypeOption.IND);
                //}
                //else if (appType == "BDL")
                //{
                //    dsApp = GetAppForm(app_id, da_micro_application.ApplicationTypeOption.BDL);
                //}

                dsApp = da_micro_application.GetAppFormDataSet(app_id, appType == "IND" ? da_micro_application.ApplicationTypeOption.IND : da_micro_application.ApplicationTypeOption.BDL);

                bl_product product = new bl_product();
                bl_micro_product_rider pro_rider = new bl_micro_product_rider();
                if (dsApp.Tables[0].Rows.Count > 0)
                {
                    var dr = dsApp.Tables[0].Rows[0];
                    product = da_product.GetProductByProductID(dr["product_id"].ToString());
                    pro_rider = da_micro_product_rider.GetMicroProductByProductID(dr["rider_product_id"].ToString());
                    string gender = dr["gender"].ToString();
                    string marital_status = dr["marital_status"].ToString();
                    string pay_mode = dr["pay_mode"].ToString();
                    string dhc = dr["rider_sum_assure"].ToString();
                    string answer = dr["answer"].ToString();
                    string answer_remarks = dr["answer_remarks"].ToString();

                    int id_type = Convert.ToInt32(dr["id_type"].ToString());
                    string id_type_en = Helper.GetIDCardTypeText(id_type);
                    string id_type_kh = Helper.GetIDCardTypeTextKh(id_type);

                    string address = "";

                    string coverEn = "";
                    string coverKh = "";
                    string coverPeriodType = "";

                    int termOfCover = Convert.ToInt32(dr["TERM_OF_COVER"].ToString());

                    dr["gender"] = Helper.GetGenderText(Convert.ToInt32(dr["gender"].ToString()), true, true);
                    dr["marital_status"] = Helper.GetMaritalStatusInKhmer(dr["marital_status"].ToString());
                    dr["product_name"] = dr["product_name_kh"].ToString();
                    dr["rider_product_name"] = dr["rider_product_name_kh"].ToString();
                    myReport.LocalReport.ReportPath = Server.MapPath("micro_application.rdlc");
                    myReport.LocalReport.DataSources.Clear();
                    rdsDetail = new ReportDataSource("ds_micro_application", dsApp.Tables[0]);
                    rdsBen = new ReportDataSource("ds_micro_application_ben", dsApp.Tables[1]);
                    address = dr["address"].ToString();

                    coverPeriodType = dr["COVER_PERIOD_TYPE"].ToString();
                    if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                    {
                        coverEn = "Coverage " + termOfCover + (termOfCover > 1 ? " years term" : " year term");
                        coverKh = "រយៈពេលនៃការធានា ១​ ឆ្នាំ";
                    }
                    else if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                    {
                        coverEn = "Coverage " + termOfCover + (termOfCover > 1 ? " months term" : " month term");
                        coverKh = "រយៈពេលនៃការធានា ១​ ខែ";
                    }

                    //ReportParameter1
                    ReportParameter[] paras = new ReportParameter[] { 
                        new ReportParameter("para_male", gender),
                        new ReportParameter("para_female", gender),
                        new ReportParameter("para_single", marital_status),
                       new ReportParameter("para_married", marital_status),
                      new ReportParameter("para_window", marital_status),
                      new ReportParameter("para_windower", marital_status),
                       new ReportParameter("para_annual", pay_mode),
                       new ReportParameter("para_semi", pay_mode),
                        new ReportParameter("para_quar", pay_mode),
                        new ReportParameter("para_month", pay_mode),
                        new ReportParameter("para_dhc10", dhc),
                        new ReportParameter("para_dhc20", dhc),
                        new ReportParameter("para_dhc30", dhc),
                        new ReportParameter("para_anw_yes", answer),
                        new ReportParameter("para_anw_no", answer),
                        new ReportParameter("para_anw_remarks", answer_remarks),
                        new ReportParameter("para_address", address),
                        new ReportParameter("para_id_type_en",id_type_en),
                        new ReportParameter("para_id_type_kh",id_type_kh),
                        new ReportParameter("para_paper_type","LetterHead"),
                        //new ReportParameter("para_paper_type","A4")
                         new ReportParameter("para_cover_en",coverEn),
                           new ReportParameter("para_cover_kh",coverKh),
                        };


                    //Assign parameters to report
                    myReport.LocalReport.SetParameters(paras);
                    myReport.LocalReport.DataSources.Add(rdsDetail);
                    myReport.LocalReport.DataSources.Add(rdsBen);
                    myReport.LocalReport.Refresh();
                    Report_Generator.ExportToPDF(this.Context, myReport, "micro_application" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
                    //Report_Generator.Export(this.Context, myReport, "micro_application" + DateTime.Now.ToString("yyyyMMddhhmmss"), false,"EXCEL");
                }
                else// no record found
                {
                    dvMessage.InnerText = "NO RECORD FOUND...!";
                    dvMessage.Attributes.CssStyle.Add("background-color", "black");
                }
            }
            else
            {
                dvMessage.InnerText = "BAD URL...!";

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[Page_Load(object sender, EventArgs e)] in page [banca_micro_application_print.aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);

            dvMessage.InnerText = "GENERATE APPLICATION FORM IS GETTING ERROR...!";
        }

    }
    /// <summary>
    /// Get application Id of Individual or Banca
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    private DataTable GetAppDetailIND(string appId)
    {
        DataTable tbl_app_detail = da_micro_application.GetApplicationDetailByApplicationID(appId);
        tbl_app_detail.Columns.Add("Address");

        if (tbl_app_detail.Rows.Count > 0)
        {
            if (tbl_app_detail.Rows[0]["province_en"].ToString().Trim().ToUpper() == "PHNOM PENH")
            {
                tbl_app_detail.Rows[0]["Address"] = (tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                    (tbl_app_detail.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl_app_detail.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "សង្កាត់" + tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                    (tbl_app_detail.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ខណ្ឌ" + tbl_app_detail.Rows[0]["district_kh"].ToString().Trim());// +" " + (tbl_app_detail.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ក្រុង " + tbl_app_detail.Rows[0]["province_kh"].ToString().Trim());
            }
            else
            {
                tbl_app_detail.Rows[0]["Address"] = (tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl_app_detail.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl_app_detail.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                    (tbl_app_detail.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl_app_detail.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "ឃុំ" + tbl_app_detail.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                    (tbl_app_detail.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ស្រុក" + tbl_app_detail.Rows[0]["district_kh"].ToString().Trim());// +" " + (tbl_app_detail.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ខេត្ត " + tbl_app_detail.Rows[0]["province_kh"].ToString().Trim());

            }
        }
        return tbl_app_detail;
    }


    private DataSet GetAppForm(string appId, da_micro_application.ApplicationTypeOption appType)
    {
        DataSet returnDs = new DataSet();
        DataSet ds = da_micro_application.GetApplicationFormByApplicationID(appId, appType);
        DataTable tbl_app_detail = new DataTable();
        DataTable tbl_app_ben = new DataTable();
        tbl_app_detail = ds.Tables[0];
        tbl_app_ben = ds.Tables[1];

        if (tbl_app_detail.Rows.Count > 0)
        {
            if (appType.ToString() == da_micro_application.ApplicationTypeOption.IND.ToString())
            {
                tbl_app_detail.Columns.Add("Address");
                var dr = tbl_app_detail.Rows[0];
                if (dr["province_en"].ToString().Trim().ToUpper() == "PHNOM PENH")
                {
                    dr["Address"] = (dr["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + dr["house_no_kh"].ToString().Trim()) + " " + (dr["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + dr["street_no_kh"].ToString().Trim()) + " " +
                        (dr["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + dr["village_kh"].ToString().Trim()) + " " + (dr["commune_kh"].ToString().Trim() == "" ? "" : "សង្កាត់" + dr["commune_kh"].ToString().Trim()) + " " +
                        (dr["district_kh"].ToString().Trim() == "" ? "" : "ខណ្ឌ" + dr["district_kh"].ToString().Trim());// +" " + (dr["province_kh"].ToString().Trim() == "" ? "" : "ក្រុង " + dr["province_kh"].ToString().Trim());
                }
                else
                {
                    dr["Address"] = (dr["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + dr["house_no_kh"].ToString().Trim()) + " " + (dr["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + dr["street_no_kh"].ToString().Trim()) + " " +
                        (dr["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + dr["village_kh"].ToString().Trim()) + " " + (dr["commune_kh"].ToString().Trim() == "" ? "" : "ឃុំ" + dr["commune_kh"].ToString().Trim()) + " " +
                        (dr["district_kh"].ToString().Trim() == "" ? "" : "ស្រុក" + dr["district_kh"].ToString().Trim());// +" " + (dr["province_kh"].ToString().Trim() == "" ? "" : "ខេត្ត " + dr["province_kh"].ToString().Trim());

                }
            }
        }
        DataTable tbl1 = new DataTable("tbl_micro_application_form");
        tbl1 = tbl_app_detail.Copy();
        DataTable tbl2 = new DataTable("tbl_micro_application_form_ben");
        tbl2 = tbl_app_ben.Copy();

        returnDs.Tables.Add(tbl1);
        returnDs.Tables.Add(tbl2);

        return returnDs;
    }

    /// <summary>
    /// Get Application Detail of Bundle
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    private DataTable GetAppDetailBDL(string appId)
    {
        return da_group_micro_application.GetApplicationDetail(appId);
    }
}