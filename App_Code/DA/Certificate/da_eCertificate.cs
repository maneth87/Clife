using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_Certificate
/// </summary>
public class da_eCertificate 
{
	public da_eCertificate()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Get certificate information by phone number and birth date.
    /// </summary>
    /// <param name="phone_number">Customer Phone Number/ Policy number</param>
    /// <param name="dob">Format: DD-MM-YYYY</param>
    /// <returns></returns>
    public static eCertificate GetCertificate(string phone_number, string dob)
    {
        eCertificate cert = new eCertificate();
        try
        {
            #region Not Used
            /*
            if (phone_number == "85578987799" && dob == "19/03/1975") //Mr.Tondy data
            {
                #region //Sample Data

                cert.Name = "តុនឌី សូរ៉ាឌីរីឌចារ្យ / Tondy Suradiredja";
                cert.DOB = Helper.FormatDateTime("19/03/1975");
                cert.ID = "1273039";
                cert.Gender = "M";
                cert.Address = "Phnom Penh";
                cert.CustomerNumber = "M" + cert.ID;
                cert.CertificateNumber = "85578987799";
                cert.EffectiveDate = DateTime.Parse("10/04/2018") + new TimeSpan(23, 59, 00);

                cert.Age = Calculation.Culculate_Customer_Age(cert.DOB.ToString("dd/MM/yyyy"), cert.EffectiveDate.Date);

                cert.MaturityDate = cert.EffectiveDate.Date.AddYears(1);
                cert.ExpiryDate = cert.MaturityDate.Date.AddDays(-1);
                cert.PaymentModeEn = "Monthly";
                cert.PaymentMethod = "Cash";

                List<DateTime> dueDateList1 = Helper.GetDueDateList(cert.EffectiveDate, 4);
                string premium_due_date1 = "";
                if (dueDateList1.Count > 0)
                {
                    var due = dueDateList1[0];

                    premium_due_date1 += "ថ្ងៃ " + due.Day + da_policy.GetMonthName(due.Month);
                }
                cert.PremiumDueDate = premium_due_date1;
                cert.PaymentMode = "ប្រចាំខែ";

                cert.IssuedDate = DateTime.Parse("10/04/2018");
                bl_product product1 = da_product.GetProductByProductID("CI");
                cert.InsurancePlan = product1.Kh_Title + " " + product1.En_Title;
                cert.Premium = 3;
                cert.SA = 3000;
                cert.BenefitFullName = "មរតកសាស្រ្ត";
                cert.BenefitRelationship = "មរតកជន";
                cert.BenefitShare = 100;
                cert.BenefitRemarks = "";
                cert.SaleAgentCode = "00000A";
                cert.ProductID = "CI";
                cert.PhoneNumber = phone_number;
                #endregion

            }
            else if (phone_number == "85512401333" && dob == "03/10/1982") //Mr.somony data
            {
                #region //Sample Data
                cert.Name = "Prim Somony";
                cert.DOB = Helper.FormatDateTime("03/10/1982");
                cert.ID = "010920751";
                cert.Gender = "M";
                cert.Address = "Phnom Penh";
                cert.CustomerNumber = "M" + cert.ID;// "00000088";
                cert.CertificateNumber = "85512401333";
                cert.EffectiveDate = DateTime.Now.Date + (new TimeSpan(23, 59, 00));

                cert.Age = Calculation.Culculate_Customer_Age(cert.DOB.ToString("dd/MM/yyyy"), cert.EffectiveDate.Date);

                cert.MaturityDate = cert.EffectiveDate.Date.AddYears(1);
                cert.ExpiryDate = cert.MaturityDate.Date.AddDays(-1);


                List<DateTime> dueDateList1 = Helper.GetDueDateList(cert.EffectiveDate, 1);
                string premium_due_date1 = "";
                if (dueDateList1.Count > 0)
                {
                    var due = dueDateList1[0];

                    premium_due_date1 += "ថ្ងៃ " + due.Day + da_policy.GetMonthName(due.Month);
                }
                cert.PremiumDueDate = premium_due_date1;
                cert.PaymentMode = "ប្រចាំឆ្នាំ";
                cert.PaymentModeEn = "Annually";
                cert.PaymentMethod = "Cash";

                cert.IssuedDate = DateTime.Now;
                bl_product product1 = da_product.GetProductByProductID("CI");
                cert.InsurancePlan = product1.Kh_Title + " " + product1.En_Title;
                cert.Premium = 36;
                cert.SA = 3000;
                cert.BenefitFullName = "មរតកសាស្រ្ត";
                cert.BenefitRelationship = "មរតកជន";
                cert.BenefitShare = 100;
                cert.BenefitRemarks = "";
                cert.SaleAgentCode = "20146";
                cert.ProductID = "CI";
                cert.PhoneNumber = phone_number;
                #endregion
            }
            else
            {
                */
            #endregion
            #region Old SO/AL
            bl_ci.Policy policy = da_ci.Policy.GetPolicyByPolicyNumber(phone_number);
            bl_product product;
            DataTable tbl = new DataTable();
            string premium_due_date = "";
            if (policy.PolicyID != null && policy.PolicyID != "")
            {
                #region //Real data
                 tbl = new DataTable();
                tbl = da_ci.GetPolicyCIDetailReport(Helper.FormatDateTime("01-01-1900"), Helper.FormatDateTime("01-01-1900"), "", phone_number, policy.ProductID);

               

                #region //SO Product
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Select("Birth_Date ='" + Helper.FormatDateTime(dob) + "' and policy_status='IF'"))
                    {
                        cert.Name = row["last_name"].ToString() + " " + row["first_name"].ToString();
                        cert.DOB = Convert.ToDateTime(row["birth_date"].ToString());
                        cert.ID = row["id_card"].ToString();
                        cert.Gender = (row["gender"].ToString() == "0") ? "F" : "M";

                        //cert.Address = row["province"].ToString();
                        cert.Address = row["address"].ToString();
                        cert.CustomerNumber = "M" + cert.ID;// row["customer_id"].ToString();
                        cert.CertificateNumber = row["policy_number"].ToString();
                        cert.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());

                        cert.Age = Convert.ToInt32(row["age"].ToString());

                        cert.MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString());
                        cert.ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString());

                        cert.PaymentMode = row["MODE"].ToString();
                        cert.PaymentModeEn = cert.PaymentMode;
                        cert.PaymentMethod = row["Payment_by"].ToString().ToUpper();
                        string cust_contact = row["Mobile_Phone1"].ToString().Replace(" ", "");
                        cert.PhoneNumber = "855" + cust_contact.Substring(1, cust_contact.Length - 1);

                        List<DateTime> dueDateList = Helper.GetDueDateList(cert.EffectiveDate, Helper.GetPayModeID(cert.PaymentMode));
                       // string premium_due_date = "";
                        if (cert.PaymentMode.ToLower() == "daily")
                        {
                            cert.PremiumDueDate = "រៀងរាល់ថ្ងៃ";
                            cert.PaymentMode = "ប្រចាំថ្ងៃ";
                        }
                        else if (cert.PaymentMode.ToLower() == "monthly")
                        {
                            cert.PremiumDueDate = "រៀងរាល់ខែ";
                            cert.PaymentMode = "ប្រចាំខែ";
                        }
                        else if (cert.PaymentMode.ToLower() == "semi-annual")
                        {
                            foreach (DateTime due in dueDateList)
                            {
                                premium_due_date += "ថ្ងៃ " + due.Day + da_policy.GetMonthName(due.Month) + ", ";
                            }
                            if (premium_due_date.Length > 0)
                            {
                                premium_due_date = premium_due_date.Substring(0, premium_due_date.Length - 2);//cut ", "
                            }

                            cert.PremiumDueDate = premium_due_date;
                            cert.PaymentMode = "ប្រចាំឆមាស";
                        }
                        else if (cert.PaymentMode.ToLower() == "annually")
                        {

                            if (dueDateList.Count > 0)
                            {
                                var due = dueDateList[0];

                                premium_due_date += "ថ្ងៃ " + due.Day + da_policy.GetMonthName(due.Month);
                            }
                            cert.PremiumDueDate = premium_due_date;
                            cert.PaymentMode = "ប្រចាំឆ្នាំ";
                        }
                        else
                        {
                            cert.PremiumDueDate = "";
                            cert.PaymentMode = "";
                        }

                        cert.IssuedDate = Convert.ToDateTime(row["issued_date"].ToString());
                        product = new bl_product();
                        product=da_product.GetProductByProductID(row["product_id"].ToString());
                        cert.InsurancePlan = product.Kh_Title + " " + product.En_Title;
                        cert.Premium = Convert.ToDouble(row["premium"].ToString());
                        cert.SA = Convert.ToDouble(row["sum_assured"].ToString());
                        cert.BenefitFullName = "មរតកសាស្ត្រ";
                        cert.BenefitRelationship = "មរតកជន";
                        cert.BenefitShare = 100;
                        cert.BenefitRemarks = "";
                        cert.SaleAgentCode = row["AGENT_CODE"].ToString();
                        cert.ProductID = product.Product_ID;

                        //Approver information
                        da_report_approver.bl_report_approver obj_approve = new da_report_approver.bl_report_approver();
                       
                        obj_approve = da_report_approver.GetAproverInfo(policy.PolicyID);
                        string path = "";
                        if (obj_approve.ID != 0)
                        {
                            // string path = HttpContext.Current.Server.MapPath("~/Upload/Signature/Kea_Leangsrunn.png");
                            path = HttpContext.Current.Server.MapPath(obj_approve.Signature);
                            cert.ApproverName = obj_approve.NameEn;// "Kea Leangsrunn";
                            cert.ApproverNameKh = obj_approve.NameKh;// "គា​ លាងស្រ៊ុន";
                            cert.ApproverPosition = obj_approve.Remarks;// "Head of Operation";
                            cert.ApproverPositionKh = obj_approve.PositionKh;// "ប្រធានប្រតិបត្តិការ";
                            cert.Signature = System.IO.File.ReadAllBytes(path);
                        }
                        else
                        {
                            path = HttpContext.Current.Server.MapPath("~/Upload/Signature/n_a.png");
                            cert.ApproverName = "";// "Kea Leangsrunn";
                            cert.ApproverNameKh = "";// "គា​ លាងស្រ៊ុន";
                            cert.ApproverPosition = "";// "Head of Operation";
                            cert.ApproverPositionKh = "";// "ប្រធានប្រតិបត្តិការ";
                            cert.Signature = System.IO.File.ReadAllBytes(path);
                            cert.ErrorMessage = "Certificate has not approved.";
                        }
                       
                    }
                }
                #endregion //SO Product
                #region //SL Product
                /*
                else
                {
                    da_sl sl_obj = new da_sl();
                    eCertificate sl_cert= new eCertificate();
                  sl_cert=  sl_obj.GetEcertificate(phone_number, dob);
                  if (sl_cert.CertificateNumber != null && sl_cert.CertificateNumber != "")
                  {
                      cert = sl_cert;
                  }
                  else
                  {
                      cert = new eCertificate();
                  }
                }
                     */
                #endregion//SL Product
                #endregion
            }
            else
            {
                #region //new SO Product
                tbl = new DataTable();

                product = new bl_product();
                bl_micro_product_rider pro_rider = new bl_micro_product_rider();
                tbl = da_micro_policy.GetPolicyDetailByPolicyNumber(phone_number);


                string gender = "";
                int pay_mode = -1;
                string pay_mode_kh = "";
                string address = "";
                string pay_mode_en = "";

                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Select("date_of_birth ='" + Helper.FormatDateTime(dob) + "'"))
                    {
                      gender=  Helper.GetGenderText(Convert.ToInt32(tbl.Rows[0]["gender"].ToString()), true, true);
                        pay_mode = Convert.ToInt32(tbl.Rows[0]["pay_mode"].ToString());
                        pay_mode_kh = Helper.GetPaymentModeInKhmer(pay_mode);
                        pay_mode_en = Helper.GetPaymentModeEnglish(pay_mode);

                        product = da_product.GetProductByProductID(tbl.Rows[0]["product_id"].ToString());
                        pro_rider = da_micro_product_rider.GetMicroProductByProductID(tbl.Rows[0]["rider_product_id"].ToString());

                        cert.Name = row["last_name_in_khmer"].ToString() + " " + row["first_name_in_khmer"].ToString();
                        cert.DOB = Convert.ToDateTime(row["date_of_birth"].ToString());
                        cert.ID = row["id_number"].ToString();
                        cert.Gender = gender;// (row["gender"].ToString() == "0") ? "ស្រី" : "ប្រុស";

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
                        cert.Address = address;
                        cert.CustomerNumber = row["customer_number"].ToString();
                        cert.CertificateNumber = row["policy_number"].ToString();
                        cert.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());
                        cert.ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString());
                        if (product.Product_ID == "SO2022001")//HTB
                        {

                            cert.EffectiveDateString = cert.EffectiveDate.ToString("dd-MM-yyyy");
                            cert.ExpiryDateString = cert.ExpiryDate.ToString("dd-MM-yyyy");
                        }
                        else
                        {
                            cert.EffectiveDateString = cert.EffectiveDate.ToString("dd-MM-yyyy") + " ម៉ោង 23:59";
                            cert.ExpiryDateString = cert.ExpiryDate.ToString("dd-MM-yyyy") + " ម៉ោង 23:59";

                        }



                        cert.Age = Convert.ToInt32(row["age"].ToString());

                        cert.MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString());


                        cert.PaymentMode = pay_mode_kh;// row["MODE"].ToString();
                        cert.PaymentModeEn = pay_mode_en;
                        cert.PaymentMethod = "";
                        string cust_contact = row["phone_number"].ToString().Replace(" ", "");
                        cert.PhoneNumber = cust_contact;// "855" + cust_contact.Substring(1, cust_contact.Length - 1);

                        #region due date

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

                        cert.PremiumDueDate = premium_due_date;
                        cert.IssuedDate = Convert.ToDateTime(row["issued_date"].ToString());

                        cert.InsurancePlan = product.Kh_Title;// +" " + product.En_Title;
                        cert.Premium = Convert.ToDouble(row["total_amount"].ToString());
                        cert.SA = Convert.ToDouble(row["sum_assure"].ToString());
                        cert.BenefitFullName = row["ben_full_name"].ToString();
                        cert.BenefitRelationship = row["relation"].ToString();
                        cert.BenefitShare = Convert.ToInt32(row["percentage_of_share"].ToString());
                        cert.BeneficiaryAddress = row["ben_address"].ToString();
                        cert.BenefitRemarks = "";
                        cert.SaleAgentCode = row["sale_AGENT_ID"].ToString();
                        cert.ProductID = product.Product_ID;
                        cert.RiderProduct = pro_rider.KH_TITLE;
                        cert.ProductName = product.Kh_Title;
                        cert.RiderSA = Convert.ToDouble(row["rider_sum_assure"].ToString());
                        cert.RiderPremium = Convert.ToDouble(row["rider_total_amount"].ToString());
                        cert.CoverageYear = Convert.ToInt32(row["term_of_cover"].ToString());
                        cert.PayYear = Convert.ToInt32(row["payment_period"].ToString());
                        QRCODE q_r = new QRCODE();
                        q_r.DATA = new String[] {"This is to certify Camlife agreed to insure", "Full Name: " +tbl.Rows[0]["last_name_in_khmer"] .ToString() + " " +tbl.Rows[0]["first_name_in_khmer"] .ToString()  , 
                                    "Gender: " + gender , 
                                    "Age: " +cert.Age, 
                                    "Phone No.: "+tbl.Rows[0]["phone_number"] .ToString() , 
                                    "ID No.: " +tbl.Rows[0]["id_number"] .ToString() , 
                                    "Effective: " + cert.EffectiveDateString , 
                                    "Expiry: "+ cert.EffectiveDateString,
                                    "Basic SA: $"+(tbl.Rows[0]["sum_assure"].ToString()) , 
                                    "Rider DHC: $"+ (tbl.Rows[0]["rider_sum_assure"].ToString().Trim()=="0" ? "N/A" : tbl.Rows[0]["rider_sum_assure"].ToString() + "/day up to 30 days")
                                  
                                    };

                        q_r.LogoImagePath = AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo.png";
                        cert.QRCode = q_r.generateQRCode();

                        int id_type = Convert.ToInt32(row["id_type"].ToString());
                        string id_type_en = Helper.GetIDCardTypeText(id_type);
                        string id_type_kh = Helper.GetIDCardTypeTextKh(id_type);
                        cert.IDType = id_type_kh + " / " + id_type_en;
                        //Approver information
                        da_report_approver.bl_report_approver obj_approve = new da_report_approver.bl_report_approver();

                        obj_approve = da_report_approver.GetAproverInfo(row["policy_id"].ToString());
                        string path = "";
                        if (obj_approve.ID != 0)
                        {
                            // string path = HttpContext.Current.Server.MapPath("~/Upload/Signature/Kea_Leangsrunn.png");
                            path = HttpContext.Current.Server.MapPath(obj_approve.Signature);
                            cert.ApproverName = obj_approve.NameEn;// "Kea Leangsrunn";
                            cert.ApproverNameKh = obj_approve.NameKh;// "គា​ លាងស្រ៊ុន";
                            cert.ApproverPosition = obj_approve.Remarks;// "Head of Operation";
                            cert.ApproverPositionKh = obj_approve.PositionKh;// "ប្រធានប្រតិបត្តិការ";
                            cert.Signature = System.IO.File.ReadAllBytes(path);
                        }
                        else
                        {
                            path = HttpContext.Current.Server.MapPath("~/Upload/Signature/n_a.png");
                            cert.ApproverName = "";// "Kea Leangsrunn";
                            cert.ApproverNameKh = "";// "គា​ លាងស្រ៊ុន";
                            cert.ApproverPosition = "";// "Head of Operation";
                            cert.ApproverPositionKh = "";// "ប្រធានប្រតិបត្តិការ";
                            cert.Signature = System.IO.File.ReadAllBytes(path);
                            cert.ErrorMessage = "Certificate has not approved.";
                        }

                    }
                }
                else
                {

                    cert = new eCertificate();
                    cert.ErrorMessage = "Policy is not found.";
                }


                #endregion New SO

            }
            // }
            #endregion old so/al

           
        }
        catch (Exception ex)
        {
            cert = new eCertificate();
            cert.ErrorMessage = "Ooop! something is going wrong.";
            Log.AddExceptionToLog("Error function [GetCertificate(string phone_number, DateTime dob)] in class [da_eCertificate], Detail: " + ex.InnerException + " ==> " + ex.Message + " ==> " + ex.StackTrace);
        }
        return cert;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone_number"></param>
    /// <returns></returns>
    public static eCertificate GetPolicy(string phone_number)
    {
        eCertificate cert = new eCertificate();
        try
        {
            bl_ci.Policy policy = da_ci.Policy.GetPolicyByPolicyNumber(phone_number);
                #region //Real data
                DataTable tbl = new DataTable();
                tbl = da_ci.GetPolicyCIDetailReport(Helper.FormatDateTime("01-01-1900"), Helper.FormatDateTime("01-01-1900"), "", phone_number, policy.ProductID);

                foreach (DataRow row in tbl.Rows)
                {
                    cert.Name = row["last_name"].ToString() + " " + row["first_name"].ToString();
                    cert.DOB = Convert.ToDateTime(row["birth_date"].ToString());
                    cert.ID = row["id_card"].ToString();
                    cert.Gender = (row["gender"].ToString() == "0") ? "F" : "M";

                    cert.Address = row["province"].ToString();
                    cert.CustomerNumber = "M" + cert.ID;// row["customer_id"].ToString();
                    cert.CertificateNumber = row["policy_number"].ToString();
                    cert.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString());

                    cert.Age = Convert.ToInt32(row["age"].ToString());

                    cert.MaturityDate = Convert.ToDateTime(row["maturity_date"].ToString());
                    cert.ExpiryDate = Convert.ToDateTime(row["expiry_date"].ToString());

                    cert.PaymentMode = row["MODE"].ToString();
                    cert.PaymentModeEn = cert.PaymentMode;
                    cert.PaymentMethod = row["Payment_by"].ToString().ToUpper();
                    List<DateTime> dueDateList = Helper.GetDueDateList(cert.EffectiveDate, Helper.GetPayModeID(cert.PaymentMode));
                    string premium_due_date = "";
                    if (cert.PaymentMode.ToLower() == "daily")
                    {
                        cert.PremiumDueDate = "រៀងរាល់ថ្ងៃ";
                        cert.PaymentMode = "ប្រចាំថ្ងៃ";
                    }
                    else if (cert.PaymentMode.ToLower() == "monthly")
                    {
                        cert.PremiumDueDate = "រៀងរាល់ខែ";
                        cert.PaymentMode = "ប្រចាំខែ";
                    }
                    else if (cert.PaymentMode.ToLower() == "semi-annual")
                    {
                        foreach (DateTime due in dueDateList)
                        {
                            premium_due_date += "ថ្ងៃ " + due.Day + da_policy.GetMonthName(due.Month) + ", ";
                        }
                        if (premium_due_date.Length > 0)
                        {
                            premium_due_date = premium_due_date.Substring(0, premium_due_date.Length - 2);//cut ", "
                        }

                        cert.PremiumDueDate = premium_due_date;
                        cert.PaymentMode = "ប្រចាំឆមាស";
                    }
                    else if (cert.PaymentMode.ToLower() == "annually")
                    {

                        if (dueDateList.Count > 0)
                        {
                            var due = dueDateList[0];

                            premium_due_date += "ថ្ងៃ " + due.Day + da_policy.GetMonthName(due.Month);
                        }
                        cert.PremiumDueDate = premium_due_date;
                        cert.PaymentMode = "ប្រចាំឆ្នាំ";
                    }
                    else
                    {
                        cert.PremiumDueDate = "";
                        cert.PaymentMode = "";
                    }

                    cert.IssuedDate = Convert.ToDateTime(row["issued_date"].ToString());
                    bl_product product = da_product.GetProductByProductID(row["product_id"].ToString());
                    cert.InsurancePlan = product.Kh_Title + " " + product.En_Title;
                    cert.Premium = Convert.ToDouble(row["premium"].ToString());
                    cert.SA = Convert.ToDouble(row["sum_assured"].ToString());
                    cert.BenefitFullName = "មរតកសាស្រ្ត";
                    cert.BenefitRelationship = "មរតកជន";
                    cert.BenefitShare = 100;
                    cert.BenefitRemarks = "";
                    cert.SaleAgentCode = row["AGENT_CODE"].ToString();
                    cert.ProductID = product.Product_ID;
                    cert.PhoneNumber = phone_number;
                }
                #endregion

            
        }
        catch (Exception ex)
        {
            cert = new eCertificate();
            Log.AddExceptionToLog("Error function [GetPolicy(string phone_number)] in class [da_eCertificate], Detail: " + ex.InnerException + " ==> " + ex.Message + " ==> " + ex.StackTrace);
        }
        return cert;
    }

}