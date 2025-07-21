using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Reporting;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.Script.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Certificate
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Certificate : System.Web.Services.WebService
{

    public Certificate()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone_number">Phone Number: 85512xxxxxx</param>
    /// <param name="dob">DOB: DD-MM-YYYY</param>
    /// <returns></returns>
    [WebMethod]
    public void GetCertificate(string phone_number, string dob)
    {
        try
        {

            eCertificate eCert = new eCertificate();

            eCert = da_eCertificate.GetCertificate(phone_number, dob);

            if (eCert.CertificateNumber == null || eCert.CertificateNumber == "")
            {
                //get SL certificate
                da_sl sl_obj = new da_sl();
                eCertificate sl_cert = new eCertificate();
                sl_cert = sl_obj.GetEcertificate(phone_number, dob);
                if (sl_cert.CertificateNumber == null || sl_cert.CertificateNumber == "")
                {

                }
                else
                {
                    QRCODE q_r = new QRCODE();
                    q_r.DATA = new String[] {"This is to certify Camlife agreed to insure", "Full Name: " + sl_cert.Name  , 
                                    "Gender: " + sl_cert.Gender , 
                                    "Age: " + sl_cert.Age, 
                                    "Phone No.: "+ sl_cert.PhoneNumber, 
                                    "ID No.: " + sl_cert.ID,
                                    "Effective: " + sl_cert.EffectiveDateString , 
                                    "Expiry: "+ sl_cert.ExpiryDateString,
                                    "Basic SA: $"+ sl_cert.SA, 
                                    "Rider DHC: $"+ (sl_cert.RiderSA==0 ? "N/A" : sl_cert.RiderSA + "/day up to 30 days")
                                  
                                    };

                    q_r.LogoImagePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo.png";
                    sl_cert.QRCode = q_r.generateQRCode();
                }



                eCert = sl_cert;

                //Approver information
                da_report_approver.bl_report_approver obj_approve = new da_report_approver.bl_report_approver();
                obj_approve = da_report_approver.GetAproverSL(eCert.EffectiveDate);
                string path = "";
                if (obj_approve.ID != 0)
                {
                    // string path = HttpContext.Current.Server.MapPath("~/Upload/Signature/Kea_Leangsrunn.png");
                    path = HttpContext.Current.Server.MapPath(obj_approve.Signature);
                    eCert.ApproverName = obj_approve.NameEn;// "Kea Leangsrunn";
                    eCert.ApproverNameKh = obj_approve.NameKh;// "គា​ លាងស្រ៊ុន";
                    eCert.ApproverPosition = obj_approve.Remarks;// "Head of Operation";
                    eCert.ApproverPositionKh = obj_approve.PositionKh;// "ប្រធានប្រតិបត្តិការ";
                    eCert.Signature = System.IO.File.ReadAllBytes(path);
                }
                else
                {
                    path = HttpContext.Current.Server.MapPath("~/Upload/Signature/n_a.png");
                    eCert.ApproverName = "";// "Kea Leangsrunn";
                    eCert.ApproverNameKh = "";// "គា​ លាងស្រ៊ុន";
                    eCert.ApproverPosition = "";// "Head of Operation";
                    eCert.ApproverPositionKh = "";// "ប្រធានប្រតិបត្តិការ";
                    eCert.Signature = System.IO.File.ReadAllBytes(path);
                    eCert.ErrorMessage = "Certificate has not approved.";
                }
            }

            string json;
            //if (eCert.Name == null || eCert.Name=="" || eCert.CertificateNumber == null || eCert.CertificateNumber=="" || eCert.ID==null || eCert.ID=="" || eCert.DOB==null || eCert.DOB == Helper.FormatDateTime("01/01/1900") ||eCert.Gender==null || eCert.Gender=="")
            //{
            //    MyResponse myRest = new MyResponse();
            //    myRest.Error = "No record found";
            //    Context.Response.Write(JsonConvert.SerializeObject(myRest));
            //}
            //else
            //{
            //    json = JsonConvert.SerializeObject(eCert);

            //    //eCertificate obj = JsonConvert.DeserializeObject<eCertificate>(json);

            //    Context.Response.Write(json);
            //}
            if (eCert.ErrorMessage != "" && eCert.ErrorMessage != null)
            {
                MyResponse myRest = new MyResponse();
                myRest.Error = eCert.ErrorMessage;
                Context.Response.Write(JsonConvert.SerializeObject(myRest));
            }
            else
            {
                json = JsonConvert.SerializeObject(eCert);

                //eCertificate obj = JsonConvert.DeserializeObject<eCertificate>(json);

                Context.Response.Write(json);
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetCertificate(string phone_number, string dob)] in class [Certificate], detail: " + ex.Message);
            MyResponse myRest = new MyResponse();
            myRest.Error = "Error";
            Context.Response.Write(JsonConvert.SerializeObject(myRest));
        }
    }

    [WebMethod]
    public void GetPolicy(string phone_number)
    {
        try
        {
            //check phone number format
            int pol_length = 0;
            pol_length = phone_number.Length;
            if (pol_length <= 12 && pol_length >= 9)//policy number is in phone number format
            {
                //format 85512xxxxxx
                if (phone_number.Trim().Substring(0, 3) != "855")
                {
                    phone_number = "855" + phone_number.Trim().Substring(1, pol_length - 1);
                }
            }
            else
            {
                //policy number is not in phone number format

            }
            eCertificate eCert = new eCertificate();

            eCert = da_eCertificate.GetPolicy(phone_number);
            if (eCert.CertificateNumber == null || eCert.CertificateNumber == "")
            {
                //get SL certificate
                da_sl sl_obj = new da_sl();
                eCertificate sl_cert = new eCertificate();
                sl_cert = sl_obj.GetPolicy(phone_number);

                eCert = sl_cert;
            }


            string json;
            if (eCert.CertificateNumber == null || eCert.CertificateNumber == "")
            {
                MyResponse myRest = new MyResponse();
                myRest.Error = "No record found";
                Context.Response.Write(JsonConvert.SerializeObject(myRest));
            }
            else
            {
                json = JsonConvert.SerializeObject(eCert);

                //eCertificate obj = JsonConvert.DeserializeObject<eCertificate>(json);

                Context.Response.Write(json);
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicy(string phone_number)] in class [Certificate], detail: " + ex.Message);
            MyResponse myRest = new MyResponse();
            myRest.Error = "Error";
            Context.Response.Write(JsonConvert.SerializeObject(myRest));
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetApplicationByte(string applicationId, string applicationType)
    {
        string strResponse = "";
        try
        {
            ReportViewer myReport = new ReportViewer();
            ReportDataSource rdsDetail;
            ReportDataSource rdsBen;

            DataSet dsApp = da_micro_application.GetAppFormDataSet(applicationId, applicationType == "IND" ? da_micro_application.ApplicationTypeOption.IND : da_micro_application.ApplicationTypeOption.BDL);

            if (dsApp.Tables[0].Rows.Count > 0)
            {
                //DataTable tbl_app_detail = dsApp.Tables[0];
                //DataTable tbl_ben = dsApp.Tables[1];

                var dr = dsApp.Tables[0].Rows[0];

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

                myReport.LocalReport.ReportPath = Server.MapPath("Pages/Business/micro_application.rdlc");
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
                         //new ReportParameter("para_paper_type","A4")
                         new ReportParameter("para_cover_en",coverEn),
                           new ReportParameter("para_cover_kh",coverKh),
                        };

                //Assign parameters to report
                myReport.LocalReport.SetParameters(paras);
                myReport.LocalReport.DataSources.Add(rdsDetail);
                myReport.LocalReport.DataSources.Add(rdsBen);
                myReport.LocalReport.Refresh();

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = myReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                ResponseApplicationForm res = new ResponseApplicationForm() { Status = "Success", Message = "Success", ApplicationForm = bytes };
                strResponse = JsonConvert.SerializeObject(res);
            }
            else
            {

                ResponseApplicationForm res = new ResponseApplicationForm() { Status = "Success", Message = "No record found", ApplicationForm = null };
                strResponse = JsonConvert.SerializeObject(res);
            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[GetApplicationByte(string applicationId, string applicationType)] in class [Certificate], detail:" + ex.Message + "=>" + ex.StackTrace);
            ResponseApplicationForm res = new ResponseApplicationForm() { Status = "Fail", Message = "Generate application form is getting error.", ApplicationForm = null };
            strResponse = JsonConvert.SerializeObject(res);
        }

        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        Context.Response.Write(strResponse);
    }
   
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetApplicationBytes(string applicationId, string applicationType)
    {
        string response = "";
        try
        {
            List<byte[]> files = new List<byte[]>();
            string[] arrAppIds = applicationId.Split(',');
            int appCount = 0;

            Certificate.ResponseApplicationForm responseApplicationForm = new Certificate.ResponseApplicationForm();
            foreach (string appId in arrAppIds)
            {
                ReportViewer reportViewer = new ReportViewer();
                bl_micro_application applicationByApplicationId = da_micro_application.GetApplicationByApplicationID(appId);
                bl_micro_product_config productMicroProduct = da_micro_product_config.ProductConfig.GetProductMicroProduct(da_micro_application_insurance.GetApplicationInsurance(applicationByApplicationId.APPLICATION_NUMBER).PRODUCT_ID);
                if (productMicroProduct.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || productMicroProduct.CreatedOn.Year >= 2025)
                {
                    DB db = new DB();
                    DataTable data1 = db.GetData(AppConfiguration.GetConnectionString(), "SP_MICRO_APPLICATION_FORM_NEW_PRODUC_INFO", new string[1, 2]
          {
            {
              "@application_number",
              applicationByApplicationId.APPLICATION_NUMBER
            }
          }, "GetApplicationBytes(string applicationId, string applicationType)");
                    DataTable data2 = db.GetData(AppConfiguration.GetConnectionString(), "SP_MICRO_APPLICATION_FORM_NEW_PRODUC_BEN", new string[1, 2]
          {
            {
              "@application_number",
              applicationByApplicationId.APPLICATION_NUMBER
            }
          }, "GetApplicationBytes(string applicationId, string applicationType)");
                    DataTable data3 = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_APPLICATION_BENEFICIARY_PRIMARY_GET", new string[1, 2]
          {
            {
              "@application_number",
              applicationByApplicationId.APPLICATION_NUMBER
            }
          }, "GetApplicationBytes(string applicationId, string applicationType)");
                    ReportDataSource reportDataSource1 = new ReportDataSource("tbl_application_form", data1);
                    ReportDataSource reportDataSource2 = new ReportDataSource("tbl_micro_application_ben", data2);
                    ReportDataSource reportDataSource3 = new ReportDataSource("tbl_application_primary_ben", data3);
                    reportViewer.LocalReport.ReportPath = this.Server.MapPath("Pages/Business/micro_application_robang.rdlc");
                    reportViewer.LocalReport.DataSources.Clear();
                    reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                    reportViewer.LocalReport.DataSources.Add(reportDataSource2);
                    reportViewer.LocalReport.DataSources.Add(reportDataSource3);
                    reportViewer.LocalReport.Refresh();
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;
                    byte[] numArray = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    files.Add(numArray);
                    ++appCount;
                }
                else
                {
                    DataSet appFormDataSet = da_micro_application.GetAppFormDataSet(appId, applicationType == "IND" ? da_micro_application.ApplicationTypeOption.IND : da_micro_application.ApplicationTypeOption.BDL);
                    if (appFormDataSet.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = appFormDataSet.Tables[0].Rows[0];
                        string str2 = row["gender"].ToString();
                        string str3 = row["marital_status"].ToString();
                        string str4 = row["pay_mode"].ToString();
                        string str5 = row["rider_sum_assure"].ToString();
                        string str6 = row["answer"].ToString();
                        string str7 = row["answer_remarks"].ToString();
                        int int32_1 = Convert.ToInt32(row["id_type"].ToString());
                        string idCardTypeText = Helper.GetIDCardTypeText(int32_1);
                        string idCardTypeTextKh = Helper.GetIDCardTypeTextKh(int32_1);
                        string str8 = "";
                        string str9 = "";
                        int int32_2 = Convert.ToInt32(row["TERM_OF_COVER"].ToString());
                        row["gender"] = (object)Helper.GetGenderText(Convert.ToInt32(row["gender"].ToString()), true, true);
                        row["marital_status"] = (object)Helper.GetMaritalStatusInKhmer(row["marital_status"].ToString());
                        row["product_name"] = (object)row["product_name_kh"].ToString();
                        row["rider_product_name"] = (object)row["rider_product_name_kh"].ToString();
                        reportViewer.LocalReport.ReportPath = this.Server.MapPath("Pages/Business/micro_application.rdlc");
                        reportViewer.LocalReport.DataSources.Clear();
                        ReportDataSource reportDataSource4 = new ReportDataSource("ds_micro_application", appFormDataSet.Tables[0]);
                        ReportDataSource reportDataSource5 = new ReportDataSource("ds_micro_application_ben", appFormDataSet.Tables[1]);
                        string str10 = row["address"].ToString();
                        string str11 = row["COVER_PERIOD_TYPE"].ToString();
                        if (str11 == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                        {
                            str8 = "Coverage " + int32_2 + (int32_2 > 1 ? " years term" : " year term");
                            str9 = "រយៈពេលនៃការធានា ១\u200B ឆ្នាំ";
                        }
                        else if (str11 == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                        {
                            str8 = "Coverage " + int32_2 + (int32_2 > 1 ? " months term" : " month term");
                            str9 = "រយៈពេលនៃការធានា ១\u200B ខែ";
                        }
                        ReportParameter[] parameters = new ReportParameter[22]
            {
              new ReportParameter("para_male", str2),
              new ReportParameter("para_female", str2),
              new ReportParameter("para_single", str3),
              new ReportParameter("para_married", str3),
              new ReportParameter("para_window", str3),
              new ReportParameter("para_windower", str3),
              new ReportParameter("para_annual", str4),
              new ReportParameter("para_semi", str4),
              new ReportParameter("para_quar", str4),
              new ReportParameter("para_month", str4),
              new ReportParameter("para_dhc10", str5),
              new ReportParameter("para_dhc20", str5),
              new ReportParameter("para_dhc30", str5),
              new ReportParameter("para_anw_yes", str6),
              new ReportParameter("para_anw_no", str6),
              new ReportParameter("para_anw_remarks", str7),
              new ReportParameter("para_address", str10),
              new ReportParameter("para_id_type_en", idCardTypeText),
              new ReportParameter("para_id_type_kh", idCardTypeTextKh),
              new ReportParameter("para_paper_type", "LetterHead"),
              new ReportParameter("para_cover_en", str8),
              new ReportParameter("para_cover_kh", str9)
            };
                        reportViewer.LocalReport.SetParameters((IEnumerable<ReportParameter>)parameters);
                        reportViewer.LocalReport.DataSources.Add(reportDataSource4);
                        reportViewer.LocalReport.DataSources.Add(reportDataSource5);
                        reportViewer.LocalReport.Refresh();
                        Warning[] warnings;
                        string[] streamIds;
                        string mimeType = string.Empty;
                        string encoding = string.Empty;
                        string extension = string.Empty;
                        byte[] numArray = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        files.Add(numArray);
                        ++appCount;
                    }
                    else
                        response = JsonConvert.SerializeObject((object)new Certificate.ResponseApplicationForm()
                        {
                            Status = "Success",
                            Message = "No record found",
                            ApplicationForm = (byte[])null
                        });
                }
            }
            if (appCount > 0)
            {
                MemoryStream memoryStream = this.MergePdfForms(files);
                response = JsonConvert.SerializeObject((object)new Certificate.ResponseApplicationForm()
                {
                    Status = "Success",
                    Message = "Success",
                    ApplicationForm = memoryStream.ToArray(),
                    ApplicationFound = appCount
                });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[GetApplicationByte(string applicationId, string applicationType)] in class [Certificate], detail:" + ex.Message + "=>" + ex.StackTrace);
            response = JsonConvert.SerializeObject((object)new Certificate.ResponseApplicationForm()
            {
                Status = "Fail",
                Message = "Generate application form is getting error.",
                ApplicationForm = (byte[])null
            });
        }
        this.Context.Response.Clear();
        this.Context.Response.ContentType = "application/json";
        this.Context.Response.Write(response);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

    public void GetCertificateByte(string policyId, string policyType)
    {
        string strResponse = "";
        try
        {

            ReportViewer myReport = new ReportViewer();
            ReportDataSource rdsDetail;
            ReportDataSource rdsBen;
            DataTable tblPolDel = new DataTable();
            DataTable tblBen = new DataTable();
            DataSet ds = da_micro_policy.GetCertificateDataset(policyId, policyType == "IND" ? da_micro_policy.PolicyTypeOption.IND : da_micro_policy.PolicyTypeOption.BDL);
            tblPolDel = ds.Tables[0];
            tblBen = ds.Tables[1];

            if (tblPolDel.Rows.Count > 0)
            {


                DataTable tbl = tblPolDel.Copy();
                tbl.Columns.Add("QR_CODE", System.Type.GetType("System.Byte[]"));
                tbl.Columns.Add("APPROVER_SIGNATURE", System.Type.GetType("System.Byte[]"));
                tbl.Columns.Add("APPROVER_NAME_EN");
                tbl.Columns.Add("APPROVER_NAME_KH");
                tbl.Columns.Add("APPROVER_POSITION_EN");
                tbl.Columns.Add("APPROVER_POSITION_KH");

                var dr = tbl.Rows[0];
                #region//Approver information
                da_report_approver.bl_report_approver obj_approve = new da_report_approver.bl_report_approver();

                obj_approve = da_report_approver.GetAproverInfo(policyId);
                string path = "";
                if (obj_approve.ID != 0)
                {
                    // string path = HttpContext.Current.Server.MapPath("~/Upload/Signature/Kea_Leangsrunn.png");
                    path = HttpContext.Current.Server.MapPath(obj_approve.Signature);
                    tbl.Rows[0]["APPROVER_NAME_EN"] = obj_approve.NameEn;
                    tbl.Rows[0]["APPROVER_NAME_KH"] = obj_approve.NameKh;
                    tbl.Rows[0]["APPROVER_POSITION_EN"] = obj_approve.Remarks;
                    tbl.Rows[0]["APPROVER_POSITION_KH"] = obj_approve.PositionKh;

                    tbl.Rows[0]["APPROVER_SIGNATURE"] = System.IO.File.ReadAllBytes(path);
                }
                else
                {
                    path = HttpContext.Current.Server.MapPath("~/Upload/Signature/n_a.png");

                    tbl.Rows[0]["APPROVER_NAME_EN"] = "";
                    tbl.Rows[0]["APPROVER_NAME_KH"] = "";
                    tbl.Rows[0]["APPROVER_POSITION_EN"] = "";
                    tbl.Rows[0]["APPROVER_POSITION_KH"] = "";

                    tbl.Rows[0]["APPROVER_SIGNATURE"] = System.IO.File.ReadAllBytes(path);
                }
                #endregion



                string gender = Helper.GetGenderText(Convert.ToInt32(dr["gender"].ToString()), true, true);
                int pay_mode = Convert.ToInt32(dr["pay_mode"].ToString());
                string pay_mode_text = Helper.GetPaymentModeInKhmer(pay_mode);
                string address = "";
                int payPeriod = 0;
                int coverPeriod = 0;
                string payPeriodType = "";
                string coverPeriodType = "";

                string para_expiry_date = "";
                string para_effective_date = "";

                string paraCoverEn, paraCoverKh, paraPayEn, paraPayKh;

                var row = tbl.Rows[0];
                coverPeriod = Convert.ToInt32(row["TERM_OF_COVER"].ToString());
                payPeriod = Convert.ToInt32(row["PAYMENT_PERIOD"].ToString());
                coverPeriodType = row["cover_period_type"].ToString();
                payPeriodType = row["pay_period_type"].ToString();

                if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                {
                    paraCoverEn = "Term of cover (Year)";
                    paraCoverKh = "រយៈពេលនៃការធានា(ឆ្នាំ)";
                }
                else if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                {
                    paraCoverEn = "Term of cover (Month)";
                    paraCoverKh = "រយៈពេលនៃការធានា(ខែ)";
                }
                else
                { paraCoverEn = ""; paraCoverKh = ""; }

                if (payPeriodType == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                {
                    paraPayEn = "Premium payment period (Year)";
                    paraPayKh = "រយៈពេលបង់បុព្វលាភធានារ៉ាប់រង(ឆ្នាំ)";
                }
                else if (payPeriodType == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                {
                    paraPayEn = "Premium payment period (Month)";
                    paraPayKh = "រយៈពេលបង់បុព្វលាភធានារ៉ាប់រង(ខែ)";
                }
                else { paraPayEn = ""; paraPayKh = ""; }

                address = dr["address"].ToString();
                tbl.Rows[0]["gender"] = gender;
                tbl.Rows[0]["marital_status"] = Helper.GetMaritalStatusInKhmer(dr["marital_status"].ToString());
                tbl.Rows[0]["product_name"] = dr["product_name_kh"].ToString();
                tbl.Rows[0]["rider_product_name"] = dr["rider_product_name_kh"].ToString();
                tbl.Rows[0]["pay_mode"] = pay_mode_text;

                DateTime effective_date = Convert.ToDateTime(dr["effective_date"].ToString());
                int age = Calculation.Culculate_Customer_Age(Convert.ToDateTime(dr["date_of_birth"].ToString()).ToString("dd-MM-yyyy"), effective_date);
                int id_type = Convert.ToInt32(dr["id_type"].ToString());
                string id_type_en = Helper.GetIDCardTypeText(id_type);
                string id_type_kh = Helper.GetIDCardTypeTextKh(id_type);
                string channelItemId = "";
                channelItemId = tbl.Rows[0]["channel_item_id"].ToString();
                // if (product.Product_ID == "SO2022001")//HTB , jTRUST, WING
                if (channelItemId == "791D3296-82D0-4F07-AC62-B5C358742E2B" || channelItemId == "A67DCDF1-37AF-4E4A-AAAE-AB7C48F62FF6" || channelItemId == "C6F8A033-548E-45BB-BA16-98D4792B7A62")
                {
                    para_effective_date = effective_date.ToString("dd-MM-yyyy");
                    para_expiry_date = Convert.ToDateTime(dr["expiry_date"].ToString()).ToString("dd-MM-yyyy");
                }
                else
                {
                    para_effective_date = effective_date.ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                    para_expiry_date = Convert.ToDateTime(dr["expiry_date"].ToString()).ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                }

                #region due date
                string premium_due_date = "";
                if (pay_mode == 5)
                {
                    premium_due_date = "ថ្ងៃបន្ទាប់";
                }
                else if (pay_mode == 4)
                {
                    premium_due_date = "ខែបន្ទាប់";
                }
                else if (pay_mode == 3)
                {
                    premium_due_date = "ត្រីមាសបន្ទាប់";
                }
                else if (pay_mode == 2)
                {
                    premium_due_date = "ឆមាសបន្ទាប់";
                }
                else if (pay_mode == 1)
                {
                    premium_due_date = "ឆ្នាំបន្ទាប់";
                }
                else
                {
                    premium_due_date = "";
                }

                #endregion

                QRCODE q_r = new QRCODE();
                q_r.DATA = new String[] {"This is to certify Camlife agreed to insure", "Full Name: " +dr["last_name_in_khmer"] .ToString() + " " +dr["first_name_in_khmer"] .ToString()  , 
                                    "Gender: " + gender , 
                                    "Age: " +age, 
                                    "Phone No.: "+dr["phone_number"] .ToString() , 
                                    "ID No.: " +dr["id_number"] .ToString() , 
                                    "Effective: " +para_effective_date , 
                                    "Expiry: "+ para_expiry_date,
                                    "Basic SA: $"+(dr["sum_assure"].ToString()) , 
                                    "Rider DHC: "+ (dr["rider_sum_assure"].ToString().Trim()=="0" ? "N/A" : "$" +dr["rider_sum_assure"].ToString() + "/day up to 30 days")
                                  
                                    };

                q_r.LogoImagePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo.png";
                tbl.Rows[0]["QR_CODE"] = q_r.generateQRCode();
                myReport.LocalReport.ReportPath = Server.MapPath("Pages/Business/micro_cert.rdlc");
                myReport.LocalReport.DataSources.Clear();
                ReportParameter[] paras = new ReportParameter[] { 
                        new ReportParameter("para_age", age+""),
                        new ReportParameter("para_due_date", premium_due_date),
                        new ReportParameter("para_address", address.Trim()),
                        new ReportParameter("para_effective_date", para_effective_date),
                        new ReportParameter("para_expiry_date", para_expiry_date),
                        new ReportParameter("para_id_type_en",id_type_en),
                        new ReportParameter("para_id_type_kh",id_type_kh),
                         new ReportParameter("para_coverEn",paraCoverEn),
                          new ReportParameter("para_coverKh",paraCoverKh),
                          new ReportParameter("para_payEn",paraPayEn),
                          new ReportParameter("para_payKh",paraPayKh)

                        };

                //Assign parameters to report
                myReport.LocalReport.SetParameters(paras);

                rdsDetail = new ReportDataSource("ds_micro_cert", tbl);
                rdsBen = new ReportDataSource("ds_micro_ben", tblBen);


                myReport.LocalReport.DataSources.Add(rdsDetail);
                myReport.LocalReport.DataSources.Add(rdsBen);

                myReport.LocalReport.Refresh();

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = myReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                ResponseCertificate res = new ResponseCertificate() { Status = "Success", Message = "Success", Certificate = bytes };
                strResponse = JsonConvert.SerializeObject(res);

            }
            else
            {

                ResponseCertificate res = new ResponseCertificate() { Status = "Success", Message = "No record found", Certificate = null };
                strResponse = JsonConvert.SerializeObject(res);
            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[GetCertificateByte(string policyId, string policyType)] in class [Certificate], detail:" + ex.Message + "=>" + ex.StackTrace);
            ResponseCertificate res = new ResponseCertificate() { Status = "Fail", Message = "Generate certificate is getting error.", Certificate = null };
            strResponse = JsonConvert.SerializeObject(res);
        }

        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        Context.Response.Write(strResponse);
    }


    /*print multi certificate*/
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

    public void GetCertificateBytes(string policyId, string policyType)
    {
        string strResponse = "";
        ResponseCertificate res = new ResponseCertificate();
        try
        {

            List<byte[]> files = new List<byte[]>();

            string[] arrPolId = policyId.Split(',');
            int policyFound = 0;

            foreach (string pol in arrPolId)
            {
                ReportViewer myReport = new ReportViewer();
                ReportDataSource rdsDetail;
                ReportDataSource rdsBen;
                DataTable tblPolDel = new DataTable();
                DataTable tblBen = new DataTable();
                DataSet ds = da_micro_policy.GetCertificateDataset(pol, policyType == "IND" ? da_micro_policy.PolicyTypeOption.IND : da_micro_policy.PolicyTypeOption.BDL);
                tblPolDel = ds.Tables[0];
                tblBen = ds.Tables[1];

                if (tblPolDel.Rows.Count > 0)
                {
                    DataTable tbl = tblPolDel.Copy();
                    tbl.Columns.Add("QR_CODE", System.Type.GetType("System.Byte[]"));
                    tbl.Columns.Add("APPROVER_SIGNATURE", System.Type.GetType("System.Byte[]"));
                    tbl.Columns.Add("APPROVER_NAME_EN");
                    tbl.Columns.Add("APPROVER_NAME_KH");
                    tbl.Columns.Add("APPROVER_POSITION_EN");
                    tbl.Columns.Add("APPROVER_POSITION_KH");

                    var dr = tbl.Rows[0];
                    #region//Approver information
                    da_report_approver.bl_report_approver obj_approve = new da_report_approver.bl_report_approver();

                    obj_approve = da_report_approver.GetAproverInfo(pol);
                    string path = "";
                    if (obj_approve.ID != 0)
                    {
                        // string path = HttpContext.Current.Server.MapPath("~/Upload/Signature/Kea_Leangsrunn.png");
                        path = HttpContext.Current.Server.MapPath(obj_approve.Signature);
                        tbl.Rows[0]["APPROVER_NAME_EN"] = obj_approve.NameEn;
                        tbl.Rows[0]["APPROVER_NAME_KH"] = obj_approve.NameKh;
                        tbl.Rows[0]["APPROVER_POSITION_EN"] = obj_approve.Remarks;
                        tbl.Rows[0]["APPROVER_POSITION_KH"] = obj_approve.PositionKh;
                        tbl.Rows[0]["APPROVER_SIGNATURE"] = System.IO.File.ReadAllBytes(path);
                    }
                    else
                    {
                        path = HttpContext.Current.Server.MapPath("~/Upload/Signature/n_a.png");
                        tbl.Rows[0]["APPROVER_NAME_EN"] = "";
                        tbl.Rows[0]["APPROVER_NAME_KH"] = "";
                        tbl.Rows[0]["APPROVER_POSITION_EN"] = "";
                        tbl.Rows[0]["APPROVER_POSITION_KH"] = "";

                        tbl.Rows[0]["APPROVER_SIGNATURE"] = System.IO.File.ReadAllBytes(path);
                    }
                    #endregion

                    string gender = Helper.GetGenderText(Convert.ToInt32(dr["gender"].ToString()), true, true);
                    int pay_mode = Convert.ToInt32(dr["pay_mode"].ToString());
                    string pay_mode_text = Helper.GetPaymentModeInKhmer(pay_mode);
                    string address = "";
                    int payPeriod = 0;
                    int coverPeriod = 0;
                    string payPeriodType = "";
                    string coverPeriodType = "";

                    string para_expiry_date = "";
                    string para_effective_date = "";
                    string riderProductId = "";

                    string paraCoverEn, paraCoverKh, paraPayEn, paraPayKh;

                    var row = tbl.Rows[0];
                    coverPeriod = Convert.ToInt32(row["TERM_OF_COVER"].ToString());
                    payPeriod = Convert.ToInt32(row["PAYMENT_PERIOD"].ToString());
                    coverPeriodType = row["cover_period_type"].ToString();
                    payPeriodType = row["pay_period_type"].ToString();

                    riderProductId = row["rider_product_id"].ToString().Trim();

                    if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                    {
                        paraCoverEn = "Term of cover (Year)";
                        paraCoverKh = "រយៈពេលនៃការធានា(ឆ្នាំ)";
                    }
                    else if (coverPeriodType == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                    {
                        paraCoverEn = "Term of cover (Month)";
                        paraCoverKh = "រយៈពេលនៃការធានា(ខែ)";
                    }
                    else
                    { paraCoverEn = ""; paraCoverKh = ""; }

                    if (payPeriodType == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                    {
                        paraPayEn = "Premium payment period (Year)";
                        paraPayKh = "រយៈពេលបង់បុព្វលាភធានារ៉ាប់រង(ឆ្នាំ)";
                    }
                    else if (payPeriodType == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                    {
                        paraPayEn = "Premium payment period (Month)";
                        paraPayKh = "រយៈពេលបង់បុព្វលាភធានារ៉ាប់រង(ខែ)";
                    }
                    else { paraPayEn = ""; paraPayKh = ""; }

                    address = dr["address"].ToString();
                    tbl.Rows[0]["gender"] = gender;
                    tbl.Rows[0]["marital_status"] = Helper.GetMaritalStatusInKhmer(dr["marital_status"].ToString());
                    tbl.Rows[0]["product_name"] = dr["product_name_kh"].ToString();
                    tbl.Rows[0]["rider_product_name"] = dr["rider_product_name_kh"].ToString();
                    tbl.Rows[0]["pay_mode"] = pay_mode_text;

                    DateTime effective_date = Convert.ToDateTime(dr["effective_date"].ToString());
                    int age = Calculation.Culculate_Customer_Age(Convert.ToDateTime(dr["date_of_birth"].ToString()).ToString("dd-MM-yyyy"), effective_date);
                    int id_type = Convert.ToInt32(dr["id_type"].ToString());
                    string id_type_en = Helper.GetIDCardTypeText(id_type);
                    string id_type_kh = Helper.GetIDCardTypeTextKh(id_type);
                    string channelItemId = "";
                    channelItemId = tbl.Rows[0]["channel_item_id"].ToString();
                    // if (product.Product_ID == "SO2022001")//HTB , jTRUST, WING
                    if (channelItemId == "791D3296-82D0-4F07-AC62-B5C358742E2B" || channelItemId == "A67DCDF1-37AF-4E4A-AAAE-AB7C48F62FF6" || channelItemId == "C6F8A033-548E-45BB-BA16-98D4792B7A62")
                    {
                        para_effective_date = effective_date.ToString("dd-MM-yyyy");
                        para_expiry_date = Convert.ToDateTime(dr["expiry_date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        para_effective_date = effective_date.ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                        para_expiry_date = Convert.ToDateTime(dr["expiry_date"].ToString()).ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                    }

                    #region due date
                    string premium_due_date = "";
                    if (pay_mode == 5)
                    {
                        premium_due_date = "ថ្ងៃបន្ទាប់";
                    }
                    else if (pay_mode == 4)
                    {
                        premium_due_date = "ខែបន្ទាប់";
                    }
                    else if (pay_mode == 3)
                    {
                        premium_due_date = "ត្រីមាសបន្ទាប់";
                    }
                    else if (pay_mode == 2)
                    {
                        premium_due_date = "ឆមាសបន្ទាប់";
                    }
                    else if (pay_mode == 1)
                    {
                        premium_due_date = "ឆ្នាំបន្ទាប់";
                    }
                    else
                    {
                        premium_due_date = "";
                    }

                    #endregion

                    QRCODE q_r = new QRCODE();
                    q_r.DATA = new String[] {"This is to certify Camlife agreed to insure", "Full Name: " +dr["last_name_in_khmer"] .ToString() + " " +dr["first_name_in_khmer"] .ToString()  , 
                                    "Gender: " + gender , 
                                    "Age: " +age, 
                                    "Phone No.: "+dr["phone_number"] .ToString() , 
                                    "ID No.: " +dr["id_number"] .ToString() , 
                                    "Effective: " +para_effective_date , 
                                    "Expiry: "+ para_expiry_date,
                                    "Basic SA: $"+(dr["sum_assure"].ToString()) , 
                                    "Rider DHC: "+ (dr["rider_sum_assure"].ToString().Trim()=="0" ? "N/A" : "$" +dr["rider_sum_assure"].ToString() + "/day up to 30 days")
                                  
                                    };

                    q_r.LogoImagePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo.png";
                    tbl.Rows[0]["QR_CODE"] = q_r.generateQRCode();
                    myReport.LocalReport.ReportPath = Server.MapPath("Pages/Business/micro_cert.rdlc");
                    myReport.LocalReport.DataSources.Clear();
                    ReportParameter[] paras = new ReportParameter[] { 
                        new ReportParameter("para_age", age+""),
                        new ReportParameter("para_due_date", premium_due_date),
                        new ReportParameter("para_address", address.Trim()),
                        new ReportParameter("para_effective_date", para_effective_date),
                        new ReportParameter("para_expiry_date", para_expiry_date),
                        new ReportParameter("para_id_type_en",id_type_en),
                        new ReportParameter("para_id_type_kh",id_type_kh),
                        new ReportParameter("para_coverEn",paraCoverEn),
                        new ReportParameter("para_coverKh",paraCoverKh),
                        new ReportParameter("para_payEn",paraPayEn),
                        new ReportParameter("para_payKh",paraPayKh)
                        };

                    //Assign parameters to report
                    myReport.LocalReport.SetParameters(paras);

                    rdsDetail = new ReportDataSource("ds_micro_cert", tbl);
                    rdsBen = new ReportDataSource("ds_micro_ben", tblBen);


                    myReport.LocalReport.DataSources.Add(rdsDetail);
                    myReport.LocalReport.DataSources.Add(rdsBen);

                    myReport.LocalReport.Refresh();

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    byte[] bytes = myReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    files.Add(bytes);

                    /*add policy insurance wording*/
                    string polInsurePath = "";
                    if (riderProductId != "")
                    {
                        polInsurePath = AppConfiguration.GetSoDhcPolicyInsuranceFilePath();
                    }
                    else
                    {
                        polInsurePath = AppConfiguration.GetSoPolicyInsuranceFilePath();
                    }

                    if (polInsurePath != "")
                    {
                        polInsurePath = AppDomain.CurrentDomain.BaseDirectory + polInsurePath;
                        if (File.Exists(polInsurePath))
                        {
                            byte[] policyInsurance = File.ReadAllBytes(polInsurePath);
                            files.Add(policyInsurance);
                        }
                    }

                    policyFound += 1;

                }
                else
                {
                    res = new ResponseCertificate() { Status = "Success", Message = "No record found", Certificate = null, PolicyFound = policyFound };
                    //strResponse = JsonConvert.SerializeObject(res);
                }

            }

            if (policyFound > 0)
            {

                MemoryStream ms = MergePdfForms(files);
                res = new ResponseCertificate() { Status = "Success", Message = "Success", Certificate = ms.ToArray(), PolicyFound = policyFound };

                strResponse = JsonConvert.SerializeObject(res);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[GetCertificateByte(string policyId, string policyType)] in class [Certificate], detail:" + ex.Message + "=>" + ex.StackTrace);
            res = new ResponseCertificate() { Status = "Fail", Message = "Generate certificate is getting error.", Certificate = null };
            strResponse = JsonConvert.SerializeObject(res);
        }

        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        Context.Response.Write(strResponse);
    }

    /*print multi certificate*/
  
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetCertificateBytesOption(string policyId, string policyType, string printPolInsurance)
    {
        string response = "";
        Certificate.ResponseCertificate responseCertificate = new Certificate.ResponseCertificate();
        try
        {
            List<byte[]> files = new List<byte[]>();
            string[] strArray = policyId.Split(',');
            int certificateCount = 0;
            DataTable tblPolId = new DataTable();
            DataTable tblPolCustomer = new DataTable();
            tblPolId.Columns.Add("POLICY_ID", typeof(string));
            foreach (string str in strArray)
                tblPolId.Rows.Add((object)str);

            SqlConnection sqlConnection = new SqlConnection(AppConfiguration.GetConnectionString());
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.CommandTimeout = 0;
            selectCommand.CommandText = "SP_CT_MICRO_POLICY_GET_POLICY_CUSTOMER";
            selectCommand.Parameters.AddWithValue("@POL", tblPolId);
            selectCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            sqlDataAdapter.Fill(tblPolCustomer);
            sqlDataAdapter.Dispose();
            selectCommand.Parameters.Clear();
            selectCommand.Dispose();
            sqlConnection.Close();

            /*distinct customer numbers*/
            var distinctCus = tblPolCustomer.AsEnumerable()
               .GroupBy(row => new { CUSTOMER_NUMBER = row["CUSTOMER_NUMBER"] })
               .Select(group => group.First())  // Select first row of each group
               .CopyToDataTable();

            bl_micro_product_config proConfig = new bl_micro_product_config();
            foreach (DataRow rowCuz in distinctCus.Rows)/*loop customer to get policy for printing*/
            {

                var filterPol = tblPolCustomer.AsEnumerable()
                .Where(row => row.Field<string>("CUSTOMER_NUMBER") == rowCuz["CUSTOMER_NUMBER"].ToString());
                foreach (DataRow rowPol in filterPol)
                {
                    ReportViewer reportViewer = new ReportViewer();
                    string polId = rowPol["policy_id"].ToString();
                    proConfig = da_micro_product_config.ProductConfig.GetProductMicroProduct(da_micro_policy.GetPolicyByID(polId).PRODUCT_ID);
                    if (proConfig.ProductType.ToUpper() == bl_micro_product_config.PRODUCT_TYPE.MICRO_LOAN.ToString() || proConfig.CreatedOn.Year >= 2025)
                    {
                        DataTable tblpolInfo = new DataTable();
                        DataTable tblPrimaryBen = new DataTable();
                        DataTable tblBen = new DataTable();
                        DataSet dataSet = new DB().GetDataSet(AppConfiguration.GetConnectionString(), "SP_MICRO_CERTIFICATE_NEW_PRODUCT", new string[1, 2]
                            {
                              {
                                "@policy_id",
                                polId
                              }
                            });
                        if (dataSet.Tables.Count > 0)
                        {
                            tblpolInfo = dataSet.Tables[0];
                            tblPrimaryBen = dataSet.Tables[1];
                            tblBen = dataSet.Tables[2];
                            string signatureFilePath = tblpolInfo.Rows[0]["signature_file"].ToString();
                            tblpolInfo.Columns.Add("APPROVER_SIGNATURE", Type.GetType("System.Byte[]"));
                            string fullPath = HttpContext.Current.Server.MapPath(signatureFilePath);
                            tblpolInfo.Rows[0]["APPROVER_SIGNATURE"] = File.ReadAllBytes(fullPath);

                            ReportDataSource reportDataSource1 = new ReportDataSource("tbl_policy_info", tblpolInfo);
                            ReportDataSource reportDataSource2 = new ReportDataSource("tbl_policy_primary_ben", tblPrimaryBen);
                            ReportDataSource reportDataSource3 = new ReportDataSource("tbl_policy_ben", tblBen);
                            reportViewer.LocalReport.ReportPath = this.Server.MapPath(proConfig.Product_ID == "RSSP2025001" ? "Pages/Business/micro_certificate_raksmey.rdlc" : "Pages/Business/micro_certificate_robang.rdlc");
                            reportViewer.LocalReport.DataSources.Clear();
                            reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                            reportViewer.LocalReport.DataSources.Add(reportDataSource2);
                            reportViewer.LocalReport.DataSources.Add(reportDataSource3);
                            reportViewer.LocalReport.Refresh();
                            Warning[] warnings;
                            string[] streamIds;
                            string mimeType = string.Empty;
                            string encoding = string.Empty;
                            string extension = string.Empty;
                            byte[] numArray = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                            files.Add(numArray);
                            certificateCount++;
                        }
                    }
                    #region/*old simple one*/
                    else
                    {
                        DataTable dataTable7 = new DataTable();
                        DataTable dataTable8 = new DataTable();
                        DataSet certificateDataset = da_micro_policy.GetCertificateDataset(policyId, policyType == "IND" ? da_micro_policy.PolicyTypeOption.IND : da_micro_policy.PolicyTypeOption.BDL);
                        DataTable table4 = certificateDataset.Tables[0];
                        DataTable table5 = certificateDataset.Tables[1];
                        if (table4.Rows.Count > 0)
                        {
                            DataTable dataSourceValue = table4.Copy();
                            dataSourceValue.Columns.Add("QR_CODE", Type.GetType("System.Byte[]"));
                            dataSourceValue.Columns.Add("APPROVER_SIGNATURE", Type.GetType("System.Byte[]"));
                            dataSourceValue.Columns.Add("APPROVER_NAME_EN");
                            dataSourceValue.Columns.Add("APPROVER_NAME_KH");
                            dataSourceValue.Columns.Add("APPROVER_POSITION_EN");
                            dataSourceValue.Columns.Add("APPROVER_POSITION_KH");
                            DataRow row2 = dataSourceValue.Rows[0];
                            da_report_approver.bl_report_approver blReportApprover = new da_report_approver.bl_report_approver();
                            da_report_approver.bl_report_approver aproverInfo = da_report_approver.GetAproverInfo(policyId);
                            if (aproverInfo.ID != 0)
                            {
                                string path = HttpContext.Current.Server.MapPath(aproverInfo.Signature);
                                dataSourceValue.Rows[0]["APPROVER_NAME_EN"] = (object)aproverInfo.NameEn;
                                dataSourceValue.Rows[0]["APPROVER_NAME_KH"] = (object)aproverInfo.NameKh;
                                dataSourceValue.Rows[0]["APPROVER_POSITION_EN"] = (object)aproverInfo.Remarks;
                                dataSourceValue.Rows[0]["APPROVER_POSITION_KH"] = (object)aproverInfo.PositionKh;
                                dataSourceValue.Rows[0]["APPROVER_SIGNATURE"] = (object)File.ReadAllBytes(path);
                            }
                            else
                            {
                                string path = HttpContext.Current.Server.MapPath("~/Upload/Signature/n_a.png");
                                dataSourceValue.Rows[0]["APPROVER_NAME_EN"] = (object)"";
                                dataSourceValue.Rows[0]["APPROVER_NAME_KH"] = (object)"";
                                dataSourceValue.Rows[0]["APPROVER_POSITION_EN"] = (object)"";
                                dataSourceValue.Rows[0]["APPROVER_POSITION_KH"] = (object)"";
                                dataSourceValue.Rows[0]["APPROVER_SIGNATURE"] = (object)File.ReadAllBytes(path);
                            }
                            string genderText = Helper.GetGenderText(Convert.ToInt32(row2["gender"].ToString()), true, true);
                            int int32_1 = Convert.ToInt32(row2["pay_mode"].ToString());
                            string paymentModeInKhmer = Helper.GetPaymentModeInKhmer(int32_1);
                            string productId = "";
                            DataRow row3 = dataSourceValue.Rows[0];
                            Convert.ToInt32(row3["TERM_OF_COVER"].ToString());
                            Convert.ToInt32(row3["PAYMENT_PERIOD"].ToString());
                            string str2 = row3["cover_period_type"].ToString();
                            string str3 = row3["pay_period_type"].ToString();
                            string str4 = row3["rider_product_id"].ToString().Trim();
                            productId = row3["product_id"].ToString().Trim();
                            string str5;
                            string str6;
                            if (str2 == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                            {
                                str5 = "Term of cover (Year)";
                                str6 = "រយៈពេលនៃការធានា(ឆ្នាំ)";
                            }
                            else if (str2 == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                            {
                                str5 = "Term of cover (Month)";
                                str6 = "រយៈពេលនៃការធានា(ខែ)";
                            }
                            else
                            {
                                str5 = "";
                                str6 = "";
                            }
                            string str7;
                            string str8;
                            if (str3 == bl_micro_product_config.PERIOD_TYPE.Y.ToString())
                            {
                                str7 = "Premium payment period (Year)";
                                str8 = "រយៈពេលបង់បុព្វលាភធានារ៉ាប់រង(ឆ្នាំ)";
                            }
                            else if (str3 == bl_micro_product_config.PERIOD_TYPE.M.ToString())
                            {
                                str7 = "Premium payment period (Month)";
                                str8 = "រយៈពេលបង់បុព្វលាភធានារ៉ាប់រង(ខែ)";
                            }
                            else
                            {
                                str7 = "";
                                str8 = "";
                            }
                            string str9 = row2["address"].ToString();
                            dataSourceValue.Rows[0]["gender"] = (object)genderText;
                            dataSourceValue.Rows[0]["marital_status"] = (object)Helper.GetMaritalStatusInKhmer(row2["marital_status"].ToString());
                            dataSourceValue.Rows[0]["product_name"] = (object)row2["product_name_kh"].ToString();
                            dataSourceValue.Rows[0]["rider_product_name"] = (object)row2["rider_product_name_kh"].ToString();
                            dataSourceValue.Rows[0]["pay_mode"] = (object)paymentModeInKhmer;
                            DateTime dateTime = Convert.ToDateTime(row2["effective_date"].ToString());
                            int num2 = Calculation.Culculate_Customer_Age(Convert.ToDateTime(row2["date_of_birth"].ToString()).ToString("dd-MM-yyyy"), dateTime);
                            int int32_2 = Convert.ToInt32(row2["id_type"].ToString());
                            string idCardTypeText = Helper.GetIDCardTypeText(int32_2);
                            string idCardTypeTextKh = Helper.GetIDCardTypeTextKh(int32_2);
                            string str10 = dataSourceValue.Rows[0]["channel_item_id"].ToString();
                            string str11;
                            string str12;
                            if (str10 == "791D3296-82D0-4F07-AC62-B5C358742E2B" || str10 == "A67DCDF1-37AF-4E4A-AAAE-AB7C48F62FF6" || str10 == "C6F8A033-548E-45BB-BA16-98D4792B7A62")
                            {
                                str11 = dateTime.ToString("dd-MM-yyyy");
                                str12 = Convert.ToDateTime(row2["expiry_date"].ToString()).ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                str11 = dateTime.ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                                str12 = Convert.ToDateTime(row2["expiry_date"].ToString()).ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                            }
                            string str13;
                            switch (int32_1)
                            {
                                case 1:
                                    str13 = "ឆ្នាំបន្ទាប់";
                                    break;
                                case 2:
                                    str13 = "ឆមាសបន្ទាប់";
                                    break;
                                case 3:
                                    str13 = "ត្រីមាសបន្ទាប់";
                                    break;
                                case 4:
                                    str13 = "ខែបន្ទាប់";
                                    break;
                                case 5:
                                    str13 = "ថ្ងៃបន្ទាប់";
                                    break;
                                default:
                                    str13 = "";
                                    break;
                            }
                            dataSourceValue.Rows[0]["QR_CODE"] = (object)new QRCODE()
                            {
                                DATA = new string[10]
                {
                  "This is to certify Camlife agreed to insure",
                  "Full Name: "+row2["last_name_in_khmer"].ToString()+" "+row2["first_name_in_khmer"].ToString(),
                  "Gender: " + genderText,
                  "Age: " + (object) num2,
                  "Phone No.: " + row2["phone_number"].ToString(),
                  "ID No.: " + row2["id_number"].ToString(),
                  "Effective: " + str11,
                  "Expiry: " + str12,
                  "Basic SA: $" + row2["sum_assure"].ToString(),
                  "Rider DHC: " + (row2["rider_sum_assure"].ToString().Trim() == "0" ? "N/A" : row2["rider_sum_assure"].ToString()+"/day up to 30 days")
                },
                                LogoImagePath = (AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo.png")
                            }.generateQRCode();
                            reportViewer.LocalReport.ReportPath = this.Server.MapPath("Pages/Business/micro_cert.rdlc");
                            reportViewer.LocalReport.DataSources.Clear();
                            ReportParameter[] parameters = new ReportParameter[11]
              {
                new ReportParameter("para_age", string.Concat((object) num2)),
                new ReportParameter("para_due_date", str13),
                new ReportParameter("para_address", str9.Trim()),
                new ReportParameter("para_effective_date", str11),
                new ReportParameter("para_expiry_date", str12),
                new ReportParameter("para_id_type_en", idCardTypeText),
                new ReportParameter("para_id_type_kh", idCardTypeTextKh),
                new ReportParameter("para_coverEn", str5),
                new ReportParameter("para_coverKh", str6),
                new ReportParameter("para_payEn", str7),
                new ReportParameter("para_payKh", str8)
              };
                            reportViewer.LocalReport.SetParameters((IEnumerable<ReportParameter>)parameters);
                            ReportDataSource reportDataSource4 = new ReportDataSource("ds_micro_cert", dataSourceValue);
                            ReportDataSource reportDataSource5 = new ReportDataSource("ds_micro_ben", table5);
                            reportViewer.LocalReport.DataSources.Add(reportDataSource4);
                            reportViewer.LocalReport.DataSources.Add(reportDataSource5);
                            reportViewer.LocalReport.Refresh();
                            Warning[] warnings;
                            string[] streamIds;
                            string mimeType = string.Empty;
                            string encoding = string.Empty;
                            string extension = string.Empty;
                            byte[] numArray = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                            files.Add(numArray);
                            if (printPolInsurance.Trim().ToLower() == "y")
                            {
                                string str14;
                                if (str4 != "")
                                {
                                    str14 = AppConfiguration.GetSoDhcPolicyInsuranceFilePath();
                                }
                                else
                                {
                                    str14 = AppConfiguration.GetSoPolicyInsuranceFilePath();
                                    foreach (Certificate.PolicyInsuranceFile policyInsuranceFile in JsonConvert.DeserializeObject<List<Certificate.PolicyInsuranceFile>>(str14).Where<Certificate.PolicyInsuranceFile>((System.Func<Certificate.PolicyInsuranceFile, bool>)(_ => _.PRODUCT_ID.ToUpper() == productId.ToUpper())))
                                        str14 = policyInsuranceFile.PATH;
                                }
                                if (str14 != "")
                                {
                                    string path = AppDomain.CurrentDomain.BaseDirectory + str14;
                                    if (File.Exists(path))
                                    {
                                        byte[] numArray2 = File.ReadAllBytes(path);
                                        files.Add(numArray2);
                                    }
                                }
                            }
                            certificateCount++;
                        }
                        else
                            responseCertificate = new Certificate.ResponseCertificate()
                            {
                                Status = "Success",
                                Message = "No record found",
                                Certificate = (byte[])null,
                                PolicyFound = certificateCount
                            };
                    }
                    #endregion Old product
                }
                /*add policy insurance end of certificate of eat customers*/
                if (printPolInsurance.Trim().ToLower() == "y")
                {
                    string polInsuranceFiles = AppConfiguration.GetSoPolicyInsuranceFilePath();/*get string with json format*/
                    string signatureFilePath = "";
                    foreach (Certificate.PolicyInsuranceFile policyInsuranceFile in JsonConvert.DeserializeObject<List<Certificate.PolicyInsuranceFile>>(polInsuranceFiles).Where(_ => _.PRODUCT_ID.ToUpper() == proConfig.Product_ID.ToUpper()))
                        signatureFilePath = policyInsuranceFile.PATH;

                    if (signatureFilePath != "")
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + signatureFilePath;
                        if (File.Exists(path))
                        {
                            byte[] file = File.ReadAllBytes(path);
                            files.Add(file);
                        }
                    }
                }
            }
            if (certificateCount > 0)
            {
                MemoryStream memoryStream = this.MergePdfForms(files);
                response = JsonConvert.SerializeObject((object)new Certificate.ResponseCertificate()
                {
                    Status = "Success",
                    Message = "Success",
                    Certificate = memoryStream.ToArray(),
                    PolicyFound = certificateCount
                });
            }
            else {
                response = JsonConvert.SerializeObject((object)new Certificate.ResponseCertificate()
                {
                    Status = "Success",
                    Message = "Certificate not found.",
                    Certificate = null,
                    PolicyFound = 0
                });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[GetCertificateBytesOption(string policyId, string policyType, string printPolInsurance)] in class [Certificate], detail:" + ex.Message + "=>" + ex.StackTrace);
            response = JsonConvert.SerializeObject((object)new Certificate.ResponseCertificate()
            {
                Status = "Fail",
                Message = "Generate certificate is getting error.",
                Certificate = (byte[])null
            });
        }
        this.Context.Response.Clear();
        this.Context.Response.ContentType = "application/json";
        this.Context.Response.Write(response);
    }


    public MemoryStream MergePdfForms(List<byte[]> files)
    {
        if (files.Count > 1)
        {
            PdfReader pdfFile;
            Document doc;
            PdfWriter pCopy;
            MemoryStream msOutput = new MemoryStream();

            pdfFile = new PdfReader(files[0]);

            doc = new Document();
            pCopy = new PdfSmartCopy(doc, msOutput);

            doc.Open();

            for (int k = 0; k < files.Count; k++)
            {
                pdfFile = new PdfReader(files[k]);
                for (int i = 1; i < pdfFile.NumberOfPages + 1; i++)
                {
                    ((PdfSmartCopy)pCopy).AddPage(pCopy.GetImportedPage(pdfFile, i));
                }
                pCopy.FreeReader(pdfFile);
            }

            pdfFile.Close();
            pCopy.Close();
            doc.Close();

            return msOutput;
        }
        else if (files.Count == 1)
        {
            return new MemoryStream(files[0]);
        }

        return null;
    }

    private DataTable GetBundlePolicyDetail(string policyId)
    {
        return da_micro_group_policy.GetPolicyDetail(policyId);
    }

    private DataTable GetPolicyDetail(string policyId)
    {
        DataTable tbl = da_micro_policy.GetPolicyDetailByPolicyID(policyId);
        tbl.Columns.Add("Address");
        if (tbl.Rows.Count > 0)
        {
            if (tbl.Rows[0]["province_en"].ToString().Trim().ToUpper() == "PHNOM PENH")
            {
                tbl.Rows[0]["address"] = (tbl.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                    (tbl.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "សង្កាត់" + tbl.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                    (tbl.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ខណ្ឌ" + tbl.Rows[0]["district_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ក្រុង" + tbl.Rows[0]["province_kh"].ToString().Trim());
            }
            else
            {
                tbl.Rows[0]["address"] = (tbl.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                    (tbl.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "ឃុំ" + tbl.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                    (tbl.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ស្រុក" + tbl.Rows[0]["district_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ខេត្ត" + tbl.Rows[0]["province_kh"].ToString().Trim());

            }
        }
        return tbl;

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
    /// <summary>
    /// Get Application Detail of Bundle
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    private DataTable GetAppDetailBDL(string appId)
    {
        return da_group_micro_application.GetApplicationDetail(appId);
    }

    public class ResponseCertificate
    {
        public string Status { get; set; }

        public byte[] Certificate { get; set; }

        public int PolicyFound { get; set; }

        public string Message { get; set; }
    }

    public class ResponseApplicationForm
    {
        public string Status { get; set; }

        public byte[] ApplicationForm { get; set; }

        public string Message { get; set; }

        public int ApplicationFound { get; set; }
    }

    private class MyResponse
    {
        public string Error { get; set; }
    }

    public class PolicyInsuranceFile
    {
        public string PRODUCT_ID { get; set; }

        public string PATH { get; set; }
    }
}
