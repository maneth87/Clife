using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
public partial class Pages_Business_banca_micro_cert : System.Web.UI.Page
{
    protected void Page_Load_bak(object sender, EventArgs e)
    {
        try
        {
            string pol_id = Request.QueryString["P_ID"].ToString();
            ReportViewer myReport = new ReportViewer();
            ReportDataSource rdsDetail;


            DataTable tbl_app_detail = new DataTable("tbl_micro_cert");

            tbl_app_detail = da_micro_policy.GetPolicyDetailByPolicyID(pol_id);

            bl_product product = new bl_product();
            bl_micro_product_rider pro_rider = new bl_micro_product_rider();


            if (tbl_app_detail.Rows.Count > 0)
            {

                DataTable tbl = tbl_app_detail.Copy();
                tbl.Columns.Add("QR_CODE", System.Type.GetType("System.Byte[]"));
                tbl.Columns.Add("APPROVER_SIGNATURE", System.Type.GetType("System.Byte[]"));
                tbl.Columns.Add("APPROVER_NAME_EN");
                tbl.Columns.Add("APPROVER_NAME_KH");
                tbl.Columns.Add("APPROVER_POSITION_EN");
                tbl.Columns.Add("APPROVER_POSITION_KH");

                #region//Approver information
                da_report_approver.bl_report_approver obj_approve = new da_report_approver.bl_report_approver();

                obj_approve = da_report_approver.GetAproverInfo(pol_id);
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

                product = da_product.GetProductByProductID(tbl.Rows[0]["product_id"].ToString());
                pro_rider = da_micro_product_rider.GetMicroProductByProductID(tbl.Rows[0]["rider_product_id"].ToString());

                string gender = Helper.GetGenderText(Convert.ToInt32(tbl.Rows[0]["gender"].ToString()), true, true);
                int pay_mode = Convert.ToInt32(tbl.Rows[0]["pay_mode"].ToString());
                string pay_mode_text = Helper.GetPaymentModeInKhmer(pay_mode);
                string address = "";

                string para_expiry_date = "";
                string para_effective_date = "";

                if (tbl.Rows[0]["province_en"].ToString().Trim().ToUpper() == "PHNOM PENH")
                {
                    address = (tbl.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                        (tbl.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "សង្កាត់" + tbl.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                        (tbl.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ខណ្ឌ" + tbl.Rows[0]["district_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ក្រុង" + tbl.Rows[0]["province_kh"].ToString().Trim());
                }
                else
                {
                    address = (tbl.Rows[0]["house_no_kh"].ToString().Trim() == "" ? "" : "ផ្ទះលេខ" + tbl.Rows[0]["house_no_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["street_no_kh"].ToString().Trim() == "" ? "" : "ផ្លូវលេខ" + tbl.Rows[0]["street_no_kh"].ToString().Trim()) + " " +
                        (tbl.Rows[0]["village_kh"].ToString().Trim() == "" ? "" : "ភូមិ" + tbl.Rows[0]["village_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["commune_kh"].ToString().Trim() == "" ? "" : "ឃុំ" + tbl.Rows[0]["commune_kh"].ToString().Trim()) + " " +
                        (tbl.Rows[0]["district_kh"].ToString().Trim() == "" ? "" : "ស្រុក" + tbl.Rows[0]["district_kh"].ToString().Trim()) + " " + (tbl.Rows[0]["province_kh"].ToString().Trim() == "" ? "" : "ខេត្ត" + tbl.Rows[0]["province_kh"].ToString().Trim());

                }


                tbl.Rows[0]["gender"] = gender;
                tbl.Rows[0]["marital_status"] = Helper.GetMaritalStatusInKhmer(tbl.Rows[0]["marital_status"].ToString());
                tbl.Rows[0]["product_name"] = product.Kh_Title;
                tbl.Rows[0]["rider_product_name"] = pro_rider.KH_TITLE;
                tbl.Rows[0]["pay_mode"] = pay_mode_text;
                DateTime effective_date = Convert.ToDateTime(tbl.Rows[0]["effective_date"].ToString());
                int age = Calculation.Culculate_Customer_Age(Convert.ToDateTime(tbl.Rows[0]["date_of_birth"].ToString()).ToString("dd-MM-yyyy"), effective_date);

                int id_type = Convert.ToInt32(tbl_app_detail.Rows[0]["id_type"].ToString());
                string id_type_en = Helper.GetIDCardTypeText(id_type);
                string id_type_kh = Helper.GetIDCardTypeTextKh(id_type);

                string channelItemId = "";
                channelItemId = tbl.Rows[0]["gender"].ToString();

                //  if (product.Product_ID == "SO2022001" || product.Product_ID =="")//HTB
                if (channelItemId == "791D3296-82D0-4F07-AC62-B5C358742E2B" || channelItemId == "A67DCDF1-37AF-4E4A-AAAE-AB7C48F62FF6")
                {
                    para_effective_date = effective_date.ToString("dd-MM-yyyy");
                    para_expiry_date = Convert.ToDateTime(tbl.Rows[0]["expiry_date"].ToString()).ToString("dd-MM-yyyy");
                }
                else
                {
                    para_effective_date = effective_date.ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                    para_expiry_date = Convert.ToDateTime(tbl.Rows[0]["expiry_date"].ToString()).ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
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
                q_r.DATA = new String[] {"This is to certify Camlife agreed to insure", "Full Name: " +tbl.Rows[0]["last_name_in_khmer"] .ToString() + " " +tbl.Rows[0]["first_name_in_khmer"] .ToString()  , 
                                    "Gender: " + gender , 
                                    "Age: " +age, 
                                    "Phone No.: "+tbl.Rows[0]["phone_number"] .ToString() , 
                                    "ID No.: " +tbl.Rows[0]["id_number"] .ToString() , 
                                    "Effective: " +para_effective_date , 
                                    "Expiry: "+ para_expiry_date,
                                    "Basic SA: $"+(tbl.Rows[0]["sum_assure"].ToString()) , 
                                    "Rider DHC: "+ (tbl.Rows[0]["rider_sum_assure"].ToString().Trim()=="0" ? "N/A" : "$" +tbl.Rows[0]["rider_sum_assure"].ToString() + "/day up to 30 days")
                                  
                                    };
                //q_r.DATA = new String[] {"ក្រុមហ៊ុនធានារ៉ាប់រងអាយុជីវិតខ្នាតតូចកម្ពុជា “ខេមឡៃហ្វ” ម.ក", "Tel:023431111/061431111"  , 
                //                    "Email:info@camlife.com.kh" 
                //                    , "Websit: www.camlife.com.kh" 
                //};
                q_r.LogoImagePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo.png";
                tbl.Rows[0]["QR_CODE"] = q_r.generateQRCode();
                myReport.LocalReport.ReportPath = Server.MapPath("micro_cert.rdlc");
                myReport.LocalReport.DataSources.Clear();
                ReportParameter[] paras = new ReportParameter[] { 
                        new ReportParameter("para_age", age+""),
                        new ReportParameter("para_due_date", premium_due_date),
                        new ReportParameter("para_address", address.Trim()),
                        new ReportParameter("para_effective_date", para_effective_date),
                        new ReportParameter("para_expiry_date", para_expiry_date),
                        new ReportParameter("para_id_type_en",id_type_en),
                        new ReportParameter("para_id_type_kh",id_type_kh)

                        };

                //Assign parameters to report
                myReport.LocalReport.SetParameters(paras);

                rdsDetail = new ReportDataSource("ds_micro_cert", tbl);



                myReport.LocalReport.DataSources.Add(rdsDetail);
                myReport.LocalReport.Refresh();

                Report_Generator.ExportToPDF(this.Context, myReport, "micro_cert" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);


            }
            else
            {


            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function[Page_Load(object sender, EventArgs e)] in page [banca_micro_cert.aspx.cs], detail:" + ex.Message);


        }
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


    protected void Page_Load_bak2(object sender, EventArgs e)
    {
        try
        {
            string pol_id = "";// Request.QueryString["P_ID"].ToString();
            string polType = "";// Request.QueryString["P_TYPE"].ToString();

            ReportViewer myReport = new ReportViewer();
            ReportDataSource rdsDetail;


            DataTable tbl_app_detail = new DataTable("tbl_micro_cert");
            if (Request.QueryString.Count == 2)
            {
                pol_id = Request.QueryString["P_ID"].ToString();
                polType = Request.QueryString["P_TYPE"].ToString();
                if (polType == "IND")//individual or banca
                {
                    tbl_app_detail = GetPolicyDetail(pol_id);
                }
                else if (polType == "BDL")//bundle
                {
                    tbl_app_detail = GetBundlePolicyDetail(pol_id);
                }
                bl_product product = new bl_product();
                bl_micro_product_rider pro_rider = new bl_micro_product_rider();


                if (tbl_app_detail.Rows.Count > 0)
                {

                    DataTable tbl = tbl_app_detail.Copy();
                    tbl.Columns.Add("QR_CODE", System.Type.GetType("System.Byte[]"));
                    tbl.Columns.Add("APPROVER_SIGNATURE", System.Type.GetType("System.Byte[]"));
                    tbl.Columns.Add("APPROVER_NAME_EN");
                    tbl.Columns.Add("APPROVER_NAME_KH");
                    tbl.Columns.Add("APPROVER_POSITION_EN");
                    tbl.Columns.Add("APPROVER_POSITION_KH");

                    #region//Approver information
                    da_report_approver.bl_report_approver obj_approve = new da_report_approver.bl_report_approver();

                    obj_approve = da_report_approver.GetAproverInfo(pol_id);
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

                    product = da_product.GetProductByProductID(tbl.Rows[0]["product_id"].ToString());
                    pro_rider = da_micro_product_rider.GetMicroProductByProductID(tbl.Rows[0]["rider_product_id"].ToString());

                    string gender = Helper.GetGenderText(Convert.ToInt32(tbl.Rows[0]["gender"].ToString()), true, true);
                    int pay_mode = Convert.ToInt32(tbl.Rows[0]["pay_mode"].ToString());
                    string pay_mode_text = Helper.GetPaymentModeInKhmer(pay_mode);
                    string address = "";

                    string para_expiry_date = "";
                    string para_effective_date = "";

                    address = tbl.Rows[0]["address"].ToString();
                    tbl.Rows[0]["gender"] = gender;
                    tbl.Rows[0]["marital_status"] = Helper.GetMaritalStatusInKhmer(tbl.Rows[0]["marital_status"].ToString());
                    tbl.Rows[0]["product_name"] = product.Kh_Title;
                    tbl.Rows[0]["rider_product_name"] = pro_rider.KH_TITLE;
                    tbl.Rows[0]["pay_mode"] = pay_mode_text;

                    DateTime effective_date = Convert.ToDateTime(tbl.Rows[0]["effective_date"].ToString());
                    int age = Calculation.Culculate_Customer_Age(Convert.ToDateTime(tbl.Rows[0]["date_of_birth"].ToString()).ToString("dd-MM-yyyy"), effective_date);
                    int id_type = Convert.ToInt32(tbl_app_detail.Rows[0]["id_type"].ToString());
                    string id_type_en = Helper.GetIDCardTypeText(id_type);
                    string id_type_kh = Helper.GetIDCardTypeTextKh(id_type);
                    string channelItemId = "";
                    channelItemId = tbl.Rows[0]["channel_item_id"].ToString();
                    // if (product.Product_ID == "SO2022001")//HTB , jTRUST, WING
                    if (channelItemId == "791D3296-82D0-4F07-AC62-B5C358742E2B" || channelItemId == "A67DCDF1-37AF-4E4A-AAAE-AB7C48F62FF6" || channelItemId == "C6F8A033-548E-45BB-BA16-98D4792B7A62")
                    {
                        para_effective_date = effective_date.ToString("dd-MM-yyyy");
                        para_expiry_date = Convert.ToDateTime(tbl.Rows[0]["expiry_date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        para_effective_date = effective_date.ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                        para_expiry_date = Convert.ToDateTime(tbl.Rows[0]["expiry_date"].ToString()).ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
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
                    q_r.DATA = new String[] {"This is to certify Camlife agreed to insure", "Full Name: " +tbl.Rows[0]["last_name_in_khmer"] .ToString() + " " +tbl.Rows[0]["first_name_in_khmer"] .ToString()  , 
                                    "Gender: " + gender , 
                                    "Age: " +age, 
                                    "Phone No.: "+tbl.Rows[0]["phone_number"] .ToString() , 
                                    "ID No.: " +tbl.Rows[0]["id_number"] .ToString() , 
                                    "Effective: " +para_effective_date , 
                                    "Expiry: "+ para_expiry_date,
                                    "Basic SA: $"+(tbl.Rows[0]["sum_assure"].ToString()) , 
                                    "Rider DHC: "+ (tbl.Rows[0]["rider_sum_assure"].ToString().Trim()=="0" ? "N/A" : "$" +tbl.Rows[0]["rider_sum_assure"].ToString() + "/day up to 30 days")
                                  
                                    };

                    q_r.LogoImagePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo.png";
                    tbl.Rows[0]["QR_CODE"] = q_r.generateQRCode();
                    myReport.LocalReport.ReportPath = Server.MapPath("micro_cert.rdlc");
                    myReport.LocalReport.DataSources.Clear();
                    ReportParameter[] paras = new ReportParameter[] { 
                        new ReportParameter("para_age", age+""),
                        new ReportParameter("para_due_date", premium_due_date),
                        new ReportParameter("para_address", address.Trim()),
                        new ReportParameter("para_effective_date", para_effective_date),
                        new ReportParameter("para_expiry_date", para_expiry_date),
                        new ReportParameter("para_id_type_en",id_type_en),
                        new ReportParameter("para_id_type_kh",id_type_kh)

                        };

                    //Assign parameters to report
                    myReport.LocalReport.SetParameters(paras);

                    rdsDetail = new ReportDataSource("ds_micro_cert", tbl);



                    myReport.LocalReport.DataSources.Add(rdsDetail);
                    myReport.LocalReport.Refresh();
                    Report_Generator.ExportToPDF(this.Context, myReport, "micro_cert" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);
                }
                else
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
            Log.AddExceptionToLog("Error function[Page_Load(object sender, EventArgs e)] in page [banca_micro_cert.aspx.cs], detail:" + ex.Message);

            dvMessage.InnerText = "GENERATE CERFICATE IS GETTING ERROR...!";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string pol_id = "";// Request.QueryString["P_ID"].ToString();
            string polType = "";// Request.QueryString["P_TYPE"].ToString();

            ReportViewer myReport = new ReportViewer();
            ReportDataSource rdsDetail;
            ReportDataSource rdsBen;

            DataTable tblPolDel = new DataTable();
            DataTable tblBen = new DataTable();
            if (Request.QueryString.Count == 2)
            {
                pol_id = Request.QueryString["P_ID"].ToString();
                polType = Request.QueryString["P_TYPE"].ToString();
                DataSet ds = new DataSet();


                ds = da_micro_policy.GetCertificateDataset(pol_id, polType == "IND" ? da_micro_policy.PolicyTypeOption.IND : da_micro_policy.PolicyTypeOption.BDL);
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

                    obj_approve = da_report_approver.GetAproverInfo(pol_id);
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

                    string riderProId = tbl.Rows[0]["rider_product_id"].ToString();

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
                    myReport.LocalReport.ReportPath = Server.MapPath("micro_cert.rdlc");
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
                    //Report_Generator.ExportToPDF(this.Context, myReport, "micro_cert" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;
                    byte[] bytes = myReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    List<byte[]> files = new List<byte[]>();
                    files.Add(bytes);

                    /*add policy insurance wording*/
                    string polInsurePath = "";
                    if (!string.IsNullOrWhiteSpace(riderProId))
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
                    MemoryStream ms = MergePdfForms(files);
                    if (ms != null)
                    {
                       

                      
                            Response.ContentType = "application/pdf";
                            // Set additional headers if needed (e.g., Content-Disposition for file download)
                           // Response.AddHeader("Content-Disposition", "attachment; filename=example.pdf");
                            ms = new MemoryStream(ms.ToArray());
                            //Response.ContentType = "application/pdf";
                            ms.WriteTo(Response.OutputStream);
                            Response.Flush();
                    }
                }
                else
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
            Log.AddExceptionToLog("Error function[Page_Load(object sender, EventArgs e)] in page [banca_micro_cert.aspx.cs], detail:" + ex.Message);

            dvMessage.InnerText = "GENERATE CERFICATE IS GETTING ERROR...!";
        }
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
}