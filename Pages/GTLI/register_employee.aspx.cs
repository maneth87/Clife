using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;
using System.Globalization;


public partial class Pages_GTLI_register_employee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            //bind user name and user id to hiddenfield
            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

            txtCompany.Attributes.Add("onchange", "return GetPlans()");

            ddlSaleAgent.DataSource = da_sale_agent.GetSaleAgentList();
            ddlSaleAgent.DataTextField = "Full_Name";
            ddlSaleAgent.DataValueField = "Sale_Agent_ID";
            ddlSaleAgent.DataBind();

            //set ddlOption attribute
            ddlOption.Attributes.Add("onChange", "return OnSelectedIndexChange();");
            //hide button search
            btnSearch.Style.Clear();
            btnSearch.Style.Add("display", "none");
            
        }
    }

    //get autocomplete for company name
    [WebMethod()]
    public static string[] GetCompanyName()
    {
        ArrayList list_of_company = new ArrayList();
        list_of_company = da_gtli_company.GetListOfCompanyName();

        string[] companyArray = new string[list_of_company.Count];

        for (int i = 0; i <= list_of_company.Count - 1; i++)
        {
            companyArray[i] = list_of_company[i].ToString();
        }

        return companyArray;
    }

    #region old script for register employees to plan.
    /*
    protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
    {
        //'1. read excel file
        //'2. save new gtli policy for this company (new policy number)
        //'3. save all employee data into employee table
        //'4. calculate premium for each into then save into gtli_employee_premium table
        //'5. get total premium then save to premium table

        string channel_location_id = "0D696111-2590-4FA2-BCE6-C8B2D46648C9"; //Camlife HQ
        string channel_channel_item_id = "016AE1FC-CF77-461A-92F8-6C7605D0A648";

        try
        {
            //check if file upload contain any file                      
            if ((uploadedDoc.PostedFile != null) && !string.IsNullOrEmpty(uploadedDoc.PostedFile.FileName))
            {
                //Check company
                bool check_company = da_gtli_company.CheckCompany(txtCompany.Text.Trim());

                if (!check_company)
                {
                    //Company not found
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This company is not registered.')", true);
                    Clear("", "");
                    return;
                }

                //Get plan by plan id
                bl_gtli_plan plan = new bl_gtli_plan();
                plan = da_gtli_plan.GetPlan(hdfPlanID.Value);

                string save_path = "~/Upload/GTLI/CustomerData/";
                string file_name = Path.GetFileName(uploadedDoc.PostedFile.FileName);
                string content_type = uploadedDoc.PostedFile.ContentType;
                int content_length = uploadedDoc.PostedFile.ContentLength;

                //Save file upload
                uploadedDoc.PostedFile.SaveAs(Server.MapPath(save_path + file_name));
                string version = Path.GetExtension(file_name);

                string file_path = Server.MapPath(save_path + file_name).ToString();

                DateTime effective_date;
                DateTime expiry_date;
                int certificate_number = 0;
                string policy_number = "";
                int policy_year = 1;
                double total_sum_insure = 0;

                string payment_code = txtPaymentCode.Text.Trim();

                int dhc_option_value = plan.DHC_Option_Value;

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd/MM/yyyy";
                dtfi.DateSeparator = "/";

                DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
                dtfi2.ShortDatePattern = "MM/dd/yyyy";
                dtfi2.DateSeparator = "/";

                //check effective date
                if (!Helper.CheckDateFormat(txtEffectiveDate.Text.Trim()))
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Invalid Effective Date Format.')", true);
                    Clear("", "");
                    return;
                }
                else
                {
                    effective_date = Convert.ToDateTime(txtEffectiveDate.Text, dtfi);

                }

                if (version == ".xls")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + file_path + "';Extended Properties=Excel 8.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [GClientData$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[GClientData$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    string contact_id = "0";
                    string company_id = "0";
                    decimal total_life_premium = 0;
                    decimal total_tpd_premium = 0;
                    decimal total_dhc_premium = 0;
                    decimal total_accidental_100plus_premium = 0;
                    decimal discount = Convert.ToDecimal(txtDiscountAmount.Text.Trim());

                    decimal total_original_life_premium = 0;
                    decimal total_original_tpd_premium = 0;
                    decimal total_original_dhc_premium = 0;
                    decimal total_original_accidental_100plus_premium = 0;

                    decimal total_accidental_100plus_premium_tax_amount = 0;
                    decimal total_life_premium_tax_amount = 0;
                    decimal total_tpd_premium_tax_amount = 0;
                    decimal total_dhc_premium_tax_amount = 0;

                    decimal total_accidental_100plus_premium_discount = 0;
                    decimal total_life_premium_discount = 0;
                    decimal total_tpd_premium_discount = 0;
                    decimal total_dhc_premium_discount = 0;

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input of employee name field then try again. Row number: '" + (i + 2) + ")";
                            Clear("", "");
                            return;
                        }

                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for gender field then try again. Row number: '" + (i + 2) + ")";
                            Clear("", "");
                            return;
                        }


                        if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                        {
                            lblMessage.Text = "Please check your input for date field then try again. Row number: '" + (i + 2) + ")";
                            Clear("", "");
                            return;
                        }

                        if (plan.Sum_Insured == 0) // 0 == dynamic sum insure
                        {
                            if (!Helper.IsNumeric(dt.Rows[i][5].ToString()))
                            {
                                lblMessage.Text = "Please check your input for sum insure field then try again. Row number: '" + (i + 2) + ")";
                                Clear("", "");
                                return;
                            }
                        }

                    }
                    int remain_days = 365;

                    bool add_policy = false;

                    //found company, get company ID by name
                    company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompany.Text.Trim());

                    //get contact_id
                    bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(company_id);
                    contact_id = contact.GTLI_Contact_ID;

                    string gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                    //Check gtli policy by company id
                    if (da_gtli_policy.CheckCompanyId(company_id))
                    {
                        //Case This company already has policy -> check last policy status by this company
                        bl_gtli_policy last_policy = new bl_gtli_policy();
                        last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(company_id);

                        string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                        if (policy_status == "IF")
                        {

                            gtli_policy_temporary_id = last_policy.GTLI_Policy_ID;

                            if (da_gtli_policy.CheckPlanIdByPlanIDAndPolicyID(hdfPlanID.Value, last_policy.GTLI_Policy_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This plan is already registered.')", true);
                                Clear("", "");
                                return;
                            }
                            else
                            {
                                policy_number = last_policy.Policy_Number;
                                policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(last_policy.GTLI_Policy_ID);
                                certificate_number = da_gtli_certificate.GetGTLILastCertificateNumberByID(company_id, last_policy.Policy_Number);
                                expiry_date = last_policy.Expiry_Date;

                                TimeSpan mytimespan = last_policy.Expiry_Date.Subtract(effective_date);

                                remain_days = mytimespan.Days + 1;
                            }

                        }
                        else if (policy_status == "TER")
                        {
                            //Terminate -> add new policy with different policy number
                            certificate_number = 0;
                            policy_year = 1;
                            policy_number = "";
                            gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                            //Expiry date for 365 days
                            expiry_date = effective_date.AddYears(1);

                            //substract one day
                            expiry_date = expiry_date.AddDays(-1);

                            //Insert GTLI Policy
                            bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                            gtli_policy_temporary.Expiry_Date = expiry_date;
                            gtli_policy_temporary.Effective_Date = effective_date;
                            gtli_policy_temporary.Agreement_date = DateTime.Now;
                            gtli_policy_temporary.Issue_Date = DateTime.Now;
                            gtli_policy_temporary.Created_By = hdfusername.Value;
                            gtli_policy_temporary.Created_Note = "";
                            gtli_policy_temporary.Created_On = DateTime.Now;
                            gtli_policy_temporary.DHC_Premium = 0;
                            gtli_policy_temporary.GTLI_Company_ID = company_id;
                            gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                            gtli_policy_temporary.Life_Premium = 0;
                            gtli_policy_temporary.Policy_Number = "";
                            gtli_policy_temporary.TPD_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Original_DHC_Premium = 0;
                            gtli_policy_temporary.Original_Life_Premium = 0;
                            gtli_policy_temporary.Original_TPD_Premium = 0;
                            gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                            gtli_policy_temporary.Life_Premium_Discount = 0;
                            gtli_policy_temporary.TPD_Premium_Discount = 0;
                            gtli_policy_temporary.DHC_Premium_Discount = 0;


                            add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                            if (!add_policy)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear("", "");
                                return;
                            }


                        }
                        else if (policy_status == "MAT")
                        {
                            //Maturity -> renew policy instead
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please renew policy instead. Or Terminate current policy first.')", true);
                            Clear("", "");
                            return;
                        }
                        else
                        {
                            //For Other policy status
                            certificate_number = 0;
                            policy_year = 1;
                            policy_number = "";
                            gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                            //Expiry date for 365 days
                            expiry_date = effective_date.AddYears(1);

                            //substract one day
                            expiry_date = expiry_date.AddDays(-1);

                            //Insert GTLI Policy
                            bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                            gtli_policy_temporary.Expiry_Date = expiry_date;
                            gtli_policy_temporary.Effective_Date = effective_date;
                            gtli_policy_temporary.Agreement_date = DateTime.Now;
                            gtli_policy_temporary.Issue_Date = DateTime.Now;
                            gtli_policy_temporary.Created_By = hdfusername.Value;
                            gtli_policy_temporary.Created_Note = "";
                            gtli_policy_temporary.Created_On = DateTime.Now;
                            gtli_policy_temporary.DHC_Premium = 0;
                            gtli_policy_temporary.GTLI_Company_ID = company_id;
                            gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                            gtli_policy_temporary.Life_Premium = 0;
                            gtli_policy_temporary.Policy_Number = "";
                            gtli_policy_temporary.TPD_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Original_DHC_Premium = 0;
                            gtli_policy_temporary.Original_Life_Premium = 0;
                            gtli_policy_temporary.Original_TPD_Premium = 0;
                            gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                            gtli_policy_temporary.Life_Premium_Discount = 0;
                            gtli_policy_temporary.TPD_Premium_Discount = 0;
                            gtli_policy_temporary.DHC_Premium_Discount = 0;

                            add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                            if (!add_policy)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear("", "");
                                return;
                            }
                        }

                    }
                    else //Case First Policy for this company
                    {
                        certificate_number = 0;
                        policy_year = 1;
                        policy_number = "";
                        gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                        //Expiry date for 365 days
                        expiry_date = effective_date.AddYears(1);

                        //substract one day
                        expiry_date = expiry_date.AddDays(-1);

                        //Insert GTLI Policy
                        bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                        gtli_policy_temporary.Expiry_Date = expiry_date;
                        gtli_policy_temporary.Effective_Date = effective_date;
                        gtli_policy_temporary.Agreement_date = DateTime.Now;
                        gtli_policy_temporary.Issue_Date = DateTime.Now;
                        gtli_policy_temporary.Created_By = hdfusername.Value;
                        gtli_policy_temporary.Created_Note = "";
                        gtli_policy_temporary.Created_On = DateTime.Now;
                        gtli_policy_temporary.DHC_Premium = 0;
                        gtli_policy_temporary.GTLI_Company_ID = company_id;
                        gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                        gtli_policy_temporary.Life_Premium = 0;
                        gtli_policy_temporary.Policy_Number = "";
                        gtli_policy_temporary.TPD_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Original_DHC_Premium = 0;
                        gtli_policy_temporary.Original_Life_Premium = 0;
                        gtli_policy_temporary.Original_TPD_Premium = 0;
                        gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                        gtli_policy_temporary.Life_Premium_Discount = 0;
                        gtli_policy_temporary.TPD_Premium_Discount = 0;
                        gtli_policy_temporary.DHC_Premium_Discount = 0;


                        add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                        if (!add_policy)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear("", "");
                            return;
                        }
                    }


                    //GTLI Premium
                    bl_gtli_premium gtli_premium = new bl_gtli_premium();
                    gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                    gtli_premium.Channel_Location_ID = channel_location_id;
                    gtli_premium.Created_By = hdfusername.Value;
                    gtli_premium.Created_On = DateTime.Now;
                    gtli_premium.DHC_Option_Value = Convert.ToInt32(dhc_option_value);
                    gtli_premium.DHC_Premium = 0;
                    gtli_premium.Effective_Date = effective_date;
                    gtli_premium.Expiry_Date = expiry_date;
                    gtli_premium.GTLI_Plan_ID = hdfPlanID.Value;
                    gtli_premium.GTLI_Policy_ID = gtli_policy_temporary_id;
                    gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                    gtli_premium.Life_Premium = 0;
                    gtli_premium.Pay_Mode_ID = 1;
                    gtli_premium.Policy_Year = 1;
                    gtli_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                    gtli_premium.Sum_Insured = plan.Sum_Insured;
                    gtli_premium.TPD_Premium = 0;
                    gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                    gtli_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);
                    gtli_premium.Accidental_100Plus_Premium = 0;
                    gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                    gtli_premium.DHC_Premium_Discount = 0;
                    gtli_premium.Life_Premium_Discount = 0;
                    gtli_premium.TPD_Premium_Discount = 0;
                    gtli_premium.Discount = Convert.ToDouble(discount);
                    gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                    gtli_premium.Life_Premium_Tax_Amount = 0;
                    gtli_premium.TPD_Premium_Tax_Amount = 0;
                    gtli_premium.DHC_Premium_Tax_Amount = 0;
                    gtli_premium.Original_Accidental_100Plus_Premium = 0;
                    gtli_premium.Original_DHC_Premium = 0;
                    gtli_premium.Original_Life_Premium = 0;
                    gtli_premium.Original_TPD_Premium = 0;

                    //365 days add then transaction type = 1                                
                    gtli_premium.Transaction_Type = 1;

                    if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear(gtli_policy_temporary_id, "");
                        return;

                    }

                    //Loop excel upload data
                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {
                        DateTime dob = Convert.ToDateTime(dt.Rows[k][3], dtfi2);

                        //Insert Certificate
                        bl_gtli_certificate certificate = new bl_gtli_certificate();

                        certificate.GTLI_Company_ID = company_id;
                        certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");

                        certificate_number += 1;

                        certificate.Certificate_Number = certificate_number;


                        if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }

                        //Insert Employee
                        bl_gtli_employee employee = new bl_gtli_employee();
                        employee.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;
                        employee.Position = dt.Rows[k][4].ToString().Trim();
                        employee.Gender = dt.Rows[k][2].ToString().Trim();
                        employee.Employee_Name = dt.Rows[k][1].ToString().Trim();
                        employee.Employee_ID = dt.Rows[k][0].ToString().Trim();
                        employee.DOB = dob;
                        employee.Customer_Status = 1;

                        if (!da_gtli_employee_temporary.InsertEmployeeTemporary(employee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }


                        //calculate premium new rate                        
                        decimal original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                        //if remain days < 365
                        if (remain_days < 365)
                        {
                            //calculate premium for remaining days
                            original_life_premium = Math.Ceiling((original_life_premium * remain_days) / 365);
                        }

                        total_original_life_premium += original_life_premium;

                        string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium life
                        bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                        employee_premium_life.Premium_Type = "Death";
                        employee_premium_life.Premium = original_life_premium.ToString();
                        employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                        employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                        if (plan.Sum_Insured == 0)
                        {
                            employee_premium_life.Sum_Insured = dt.Rows[k][5].ToString();

                        }
                        else
                        {
                            employee_premium_life.Sum_Insured = plan.Sum_Insured.ToString();
                        }

                        total_sum_insure += Convert.ToDouble(employee_premium_life.Sum_Insured.Trim());

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }

                        //if has Accicental 100Plus
                        if (plan.Accidental_100Plus_Option_Value == 1)
                        {
                            decimal original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                            //if remain days < 365
                            if (remain_days < 365)
                            {
                                //calculate premium TPD for remaining days
                                original_accidental_100plus_premium = Math.Ceiling((original_accidental_100plus_premium * remain_days) / 365);
                            }

                            total_original_accidental_100plus_premium += original_accidental_100plus_premium;

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                            employee_premium_accidental_100plus.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                            employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_accidental_100plus.Premium = original_accidental_100plus_premium.ToString();
                            employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_accidental_100plus.Sum_Insured = dt.Rows[k][5].ToString();
                            }
                            else
                            {
                                employee_premium_accidental_100plus.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                                return;
                            }

                        }

                        //if has TPD
                        if (plan.TPD_Option_Value == 1)
                        {
                            decimal original_tpd_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                            //if remain days < 365
                            if (remain_days < 365)
                            {
                                //calculate premium TPD for remaining days
                                original_tpd_premium = Math.Ceiling((original_tpd_premium * remain_days) / 365);
                            }

                            total_original_tpd_premium += original_tpd_premium;

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                            employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                            employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_tpd.Premium = original_tpd_premium.ToString();
                            employee_premium_tpd.Premium_Type = "TPD";

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_tpd.Sum_Insured = dt.Rows[k][5].ToString();
                            }
                            else
                            {
                                employee_premium_tpd.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                                return;
                            }

                        }

                        int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, effective_date);

                        string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium dhc for age within 18 - 59
                        if (customer_age >= 18 && customer_age < 60)
                        {
                            decimal original_dhc_premium = plan.DHC_Option_Value;

                            //if remain days < 365
                            if (remain_days < 365)
                            {
                                original_dhc_premium = Math.Ceiling((original_dhc_premium * remain_days) / 365);
                            }

                            total_original_dhc_premium += original_dhc_premium;

                            bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                            employee_premium_dhc.Premium_Type = "DHC";
                            employee_premium_dhc.Premium = original_dhc_premium.ToString();
                            employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                            employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_dhc.Sum_Insured = dt.Rows[k][5].ToString();
                            }
                            else
                            {
                                employee_premium_dhc.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (original_dhc_premium > 0) //Insert only dhc premium > 0
                            {
                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                    Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                                    return;
                                }
                            }

                        }

                    }//End loop dt

                    //Get discount for each premium type

                    //if discount > 0
                    if (discount > 0)
                    {

                        //calculate discount for accidental 100 plus
                        if (plan.Accidental_100Plus_Option_Value == 1)
                        {
                            total_accidental_100plus_premium_discount = Math.Floor((total_original_accidental_100plus_premium * discount) / 100);

                        }

                        //calculate discount for tpd
                        if (plan.TPD_Option_Value == 1)
                        {
                            total_tpd_premium_discount = Math.Floor((total_original_tpd_premium * discount) / 100);

                        }

                        //calculate discount for dhc
                        if (plan.DHC_Option_Value > 0)
                        {
                            total_dhc_premium_discount = Math.Floor((total_original_dhc_premium * discount) / 100);

                        }


                        total_life_premium_discount = Math.Floor((total_original_life_premium * discount) / 100);


                    }

                    //Get Tax Amount
                    total_accidental_100plus_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_accidental_100plus_premium, total_accidental_100plus_premium_discount);
                    total_life_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_life_premium, total_life_premium_discount);
                    total_tpd_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_tpd_premium, total_tpd_premium_discount);
                    total_dhc_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_dhc_premium, total_dhc_premium_discount);

                    //Premium after discount
                    total_accidental_100plus_premium = total_original_accidental_100plus_premium - total_accidental_100plus_premium_discount;
                    total_life_premium = total_original_life_premium - total_life_premium_discount;
                    total_tpd_premium = total_original_tpd_premium - total_tpd_premium_discount;
                    total_dhc_premium = total_original_dhc_premium - total_dhc_premium_discount;

                    //Insert prem pay
                    bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
                    prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                    prem_pay.Prem_Year = 1;
                    prem_pay.Prem_Lot = 1;
                    prem_pay.Pay_Mode_ID = 1;
                    prem_pay.Pay_Date = effective_date;
                    prem_pay.Office_ID = channel_location_id;
                    prem_pay.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                    prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                    prem_pay.Due_Date = expiry_date.AddDays(1);
                    prem_pay.Created_On = DateTime.Now;
                    prem_pay.Created_Note = "";
                    prem_pay.Created_By = hdfusername.Value;
                    prem_pay.Amount = Convert.ToDouble(total_dhc_premium + total_life_premium + total_tpd_premium + total_accidental_100plus_premium);
                    prem_pay.Payment_Code = payment_code;

                    if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(prem_pay, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                        return;
                    }

                    //update total premium in premium temporary
                    da_gtli_premium_temporary.UpdatePremium(total_sum_insure, Convert.ToDouble(total_life_premium), Convert.ToDouble(total_tpd_premium), Convert.ToDouble(total_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium), Convert.ToDouble(total_accidental_100plus_premium_discount), Convert.ToDouble(total_life_premium_discount), Convert.ToDouble(total_tpd_premium_discount), Convert.ToDouble(total_dhc_premium_discount), Convert.ToDouble(total_original_accidental_100plus_premium), Convert.ToDouble(total_original_life_premium), Convert.ToDouble(total_original_tpd_premium), Convert.ToDouble(total_original_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium_tax_amount), Convert.ToDouble(total_life_premium_tax_amount), Convert.ToDouble(total_tpd_premium_tax_amount), Convert.ToDouble(total_dhc_premium_tax_amount), gtli_premium.GTLI_Premium_ID);

                    Clear("", "");

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy successfull.')", true);


                }
                else if (version == ".xlsx")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + file_path + "';Extended Properties=Excel 12.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [GClientData$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[GClientData$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    string contact_id = "0";
                    string company_id = "0";
                    decimal total_life_premium = 0;
                    decimal total_tpd_premium = 0;
                    decimal total_dhc_premium = 0;
                    decimal total_accidental_100plus_premium = 0;
                    decimal discount = Convert.ToDecimal(txtDiscountAmount.Text.Trim());

                    decimal total_original_life_premium = 0;
                    decimal total_original_tpd_premium = 0;
                    decimal total_original_dhc_premium = 0;
                    decimal total_original_accidental_100plus_premium = 0;

                    decimal total_accidental_100plus_premium_tax_amount = 0;
                    decimal total_life_premium_tax_amount = 0;
                    decimal total_tpd_premium_tax_amount = 0;
                    decimal total_dhc_premium_tax_amount = 0;

                    decimal total_accidental_100plus_premium_discount = 0;
                    decimal total_life_premium_discount = 0;
                    decimal total_tpd_premium_discount = 0;
                    decimal total_dhc_premium_discount = 0;

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input of employee name field then try again. Row number: '" + (i + 2) + ")";
                            Clear("", "");
                            return;
                        }

                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for gender field then try again. Row number: '" + (i + 2) + ")";
                            Clear("", "");
                            return;
                        }


                        if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                        {
                            lblMessage.Text = "Please check your input for date field then try again. Row number: '" + (i + 2) + ")";
                            Clear("", "");
                            return;
                        }

                        if (plan.Sum_Insured == 0) // 0 == dynamic sum insure
                        {
                            if (!Helper.IsNumeric(dt.Rows[i][5].ToString()))
                            {
                                lblMessage.Text = "Please check your input for sum insure field then try again. Row number: '" + (i + 2) + ")";
                                Clear("", "");
                                return;
                            }
                        }
                    }
                    int remain_days = 365;

                    bool add_policy = false;

                    //found company, get company ID by name
                    company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompany.Text.Trim());

                    //get contact_id
                    bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(company_id);
                    contact_id = contact.GTLI_Contact_ID;

                    string gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                    //Check gtli policy by company id
                    if (da_gtli_policy.CheckCompanyId(company_id))
                    {
                        //Case This company already has policy -> check last policy status by this company
                        bl_gtli_policy last_policy = new bl_gtli_policy();
                        last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(company_id);

                        string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                        if (policy_status == "IF")
                        {

                            gtli_policy_temporary_id = last_policy.GTLI_Policy_ID;

                            if (da_gtli_policy.CheckPlanIdByPlanIDAndPolicyID(hdfPlanID.Value, last_policy.GTLI_Policy_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This plan is already registered.')", true);
                                Clear("", "");
                                return;
                            }
                            else
                            {
                                policy_number = last_policy.Policy_Number;
                                policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(last_policy.GTLI_Policy_ID);
                                certificate_number = da_gtli_certificate.GetGTLILastCertificateNumberByID(company_id, last_policy.Policy_Number);
                                expiry_date = last_policy.Expiry_Date;

                                TimeSpan mytimespan = last_policy.Expiry_Date.Subtract(effective_date);

                                remain_days = mytimespan.Days + 1;
                            }

                        }
                        else if (policy_status == "TER")
                        {
                            //Terminate -> add new policy with different policy number
                            certificate_number = 0;
                            policy_year = 1;
                            policy_number = "";
                            gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                            //Expiry date for 365 days
                            expiry_date = effective_date.AddYears(1);

                            //substract one day
                            expiry_date = expiry_date.AddDays(-1);

                            //Insert GTLI Policy
                            bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                            gtli_policy_temporary.Expiry_Date = expiry_date;
                            gtli_policy_temporary.Effective_Date = effective_date;
                            gtli_policy_temporary.Agreement_date = DateTime.Now;
                            gtli_policy_temporary.Issue_Date = DateTime.Now;
                            gtli_policy_temporary.Created_By = hdfusername.Value;
                            gtli_policy_temporary.Created_Note = "";
                            gtli_policy_temporary.Created_On = DateTime.Now;
                            gtli_policy_temporary.DHC_Premium = 0;
                            gtli_policy_temporary.GTLI_Company_ID = company_id;
                            gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                            gtli_policy_temporary.Life_Premium = 0;
                            gtli_policy_temporary.Policy_Number = "";
                            gtli_policy_temporary.TPD_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Original_DHC_Premium = 0;
                            gtli_policy_temporary.Original_Life_Premium = 0;
                            gtli_policy_temporary.Original_TPD_Premium = 0;
                            gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                            gtli_policy_temporary.Life_Premium_Discount = 0;
                            gtli_policy_temporary.TPD_Premium_Discount = 0;
                            gtli_policy_temporary.DHC_Premium_Discount = 0;


                            add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                            if (!add_policy)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear("", "");
                                return;
                            }


                        }
                        else if (policy_status == "MAT")
                        {
                            //Maturity -> renew policy instead
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please renew policy instead. Or Terminate current policy first.')", true);
                            Clear("", "");
                            return;
                        }
                        else
                        {
                            //For Other policy status
                            certificate_number = 0;
                            policy_year = 1;
                            policy_number = "";
                            gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                            //Expiry date for 365 days
                            expiry_date = effective_date.AddYears(1);

                            //substract one day
                            expiry_date = expiry_date.AddDays(-1);

                            //Insert GTLI Policy
                            bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                            gtli_policy_temporary.Expiry_Date = expiry_date;
                            gtli_policy_temporary.Effective_Date = effective_date;
                            gtli_policy_temporary.Agreement_date = DateTime.Now;
                            gtli_policy_temporary.Issue_Date = DateTime.Now;
                            gtli_policy_temporary.Created_By = hdfusername.Value;
                            gtli_policy_temporary.Created_Note = "";
                            gtli_policy_temporary.Created_On = DateTime.Now;
                            gtli_policy_temporary.DHC_Premium = 0;
                            gtli_policy_temporary.GTLI_Company_ID = company_id;
                            gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                            gtli_policy_temporary.Life_Premium = 0;
                            gtli_policy_temporary.Policy_Number = "";
                            gtli_policy_temporary.TPD_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                            gtli_policy_temporary.Original_DHC_Premium = 0;
                            gtli_policy_temporary.Original_Life_Premium = 0;
                            gtli_policy_temporary.Original_TPD_Premium = 0;
                            gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                            gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                            gtli_policy_temporary.Life_Premium_Discount = 0;
                            gtli_policy_temporary.TPD_Premium_Discount = 0;
                            gtli_policy_temporary.DHC_Premium_Discount = 0;

                            add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                            if (!add_policy)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear("", "");
                                return;
                            }
                        }

                    }
                    else //Case First Policy for this company
                    {
                        certificate_number = 0;
                        policy_year = 1;
                        policy_number = "";
                        gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                        //Expiry date for 365 days
                        expiry_date = effective_date.AddYears(1);

                        //substract one day
                        expiry_date = expiry_date.AddDays(-1);

                        //Insert GTLI Policy
                        bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                        gtli_policy_temporary.Expiry_Date = expiry_date;
                        gtli_policy_temporary.Effective_Date = effective_date;
                        gtli_policy_temporary.Agreement_date = DateTime.Now;
                        gtli_policy_temporary.Issue_Date = DateTime.Now;
                        gtli_policy_temporary.Created_By = hdfusername.Value;
                        gtli_policy_temporary.Created_Note = "";
                        gtli_policy_temporary.Created_On = DateTime.Now;
                        gtli_policy_temporary.DHC_Premium = 0;
                        gtli_policy_temporary.GTLI_Company_ID = company_id;
                        gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                        gtli_policy_temporary.Life_Premium = 0;
                        gtli_policy_temporary.Policy_Number = "";
                        gtli_policy_temporary.TPD_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Original_DHC_Premium = 0;
                        gtli_policy_temporary.Original_Life_Premium = 0;
                        gtli_policy_temporary.Original_TPD_Premium = 0;
                        gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                        gtli_policy_temporary.Life_Premium_Discount = 0;
                        gtli_policy_temporary.TPD_Premium_Discount = 0;
                        gtli_policy_temporary.DHC_Premium_Discount = 0;


                        add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                        if (!add_policy)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear("", "");
                            return;
                        }
                    }


                    //GTLI Premium
                    bl_gtli_premium gtli_premium = new bl_gtli_premium();
                    gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                    gtli_premium.Channel_Location_ID = channel_location_id;
                    gtli_premium.Created_By = hdfusername.Value;
                    gtli_premium.Created_On = DateTime.Now;
                    gtli_premium.DHC_Option_Value = Convert.ToInt32(dhc_option_value);
                    gtli_premium.DHC_Premium = 0;
                    gtli_premium.Effective_Date = effective_date;
                    gtli_premium.Expiry_Date = expiry_date;
                    gtli_premium.GTLI_Plan_ID = hdfPlanID.Value;
                    gtli_premium.GTLI_Policy_ID = gtli_policy_temporary_id;
                    gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                    gtli_premium.Life_Premium = 0;
                    gtli_premium.Pay_Mode_ID = 1;
                    gtli_premium.Policy_Year = 1;
                    gtli_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                    gtli_premium.Sum_Insured = plan.Sum_Insured;
                    gtli_premium.TPD_Premium = 0;
                    gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                    gtli_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);
                    gtli_premium.Accidental_100Plus_Premium = 0;
                    gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                    gtli_premium.DHC_Premium_Discount = 0;
                    gtli_premium.Life_Premium_Discount = 0;
                    gtli_premium.TPD_Premium_Discount = 0;
                    gtli_premium.Discount = Convert.ToDouble(discount);
                    gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                    gtli_premium.Life_Premium_Tax_Amount = 0;
                    gtli_premium.TPD_Premium_Tax_Amount = 0;
                    gtli_premium.DHC_Premium_Tax_Amount = 0;
                    gtli_premium.Original_Accidental_100Plus_Premium = 0;
                    gtli_premium.Original_DHC_Premium = 0;
                    gtli_premium.Original_Life_Premium = 0;
                    gtli_premium.Original_TPD_Premium = 0;

                    //365 days add then transaction type = 1                                
                    gtli_premium.Transaction_Type = 1;

                    if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear(gtli_policy_temporary_id, "");
                        return;

                    }

                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {
                        DateTime dob = Convert.ToDateTime(dt.Rows[k][3], dtfi2);

                        //Insert Certificate
                        bl_gtli_certificate certificate = new bl_gtli_certificate();

                        certificate.GTLI_Company_ID = company_id;
                        certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");

                        certificate_number += 1;

                        certificate.Certificate_Number = certificate_number;


                        if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }

                        //Insert Employee
                        bl_gtli_employee employee = new bl_gtli_employee();
                        employee.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;
                        employee.Position = dt.Rows[k][4].ToString().Trim();
                        employee.Gender = dt.Rows[k][2].ToString().Trim();
                        employee.Employee_Name = dt.Rows[k][1].ToString().Trim();
                        employee.Employee_ID = dt.Rows[k][0].ToString().Trim();
                        employee.DOB = dob;
                        employee.Customer_Status = 1;

                        if (!da_gtli_employee_temporary.InsertEmployeeTemporary(employee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }


                        //calculate premium new rate                        
                        decimal original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                        //if remain days < 365
                        if (remain_days < 365)
                        {
                            //calculate premium for remaining days
                            original_life_premium = Math.Ceiling((original_life_premium * remain_days) / 365);
                        }

                        total_original_life_premium += original_life_premium;

                        string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium life
                        bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                        employee_premium_life.Premium_Type = "Death";
                        employee_premium_life.Premium = original_life_premium.ToString();
                        employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                        employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                        if (plan.Sum_Insured == 0)
                        {
                            employee_premium_life.Sum_Insured = dt.Rows[k][5].ToString();
                        }
                        else
                        {
                            employee_premium_life.Sum_Insured = plan.Sum_Insured.ToString();
                        }

                        total_sum_insure += Convert.ToDouble(employee_premium_life.Sum_Insured.Trim());

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }

                        //if has Accicental 100Plus
                        if (plan.Accidental_100Plus_Option_Value == 1)
                        {
                            decimal original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                            //if remain days < 365
                            if (remain_days < 365)
                            {
                                //calculate premium TPD for remaining days
                                original_accidental_100plus_premium = Math.Ceiling((original_accidental_100plus_premium * remain_days) / 365);
                            }

                            total_original_accidental_100plus_premium += original_accidental_100plus_premium;

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                            employee_premium_accidental_100plus.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                            employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_accidental_100plus.Premium = original_accidental_100plus_premium.ToString();
                            employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_accidental_100plus.Sum_Insured = dt.Rows[k][5].ToString();
                            }
                            else
                            {
                                employee_premium_accidental_100plus.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                                return;
                            }

                        }

                        //if has TPD
                        if (plan.TPD_Option_Value == 1)
                        {
                            decimal original_tpd_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                            //if remain days < 365
                            if (remain_days < 365)
                            {
                                //calculate premium TPD for remaining days
                                original_tpd_premium = Math.Ceiling((original_tpd_premium * remain_days) / 365);
                            }

                            total_original_tpd_premium += original_tpd_premium;

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                            employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                            employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_tpd.Premium = original_tpd_premium.ToString();
                            employee_premium_tpd.Premium_Type = "TPD";

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_tpd.Sum_Insured = dt.Rows[k][5].ToString();
                            }
                            else
                            {
                                employee_premium_tpd.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                                return;
                            }

                        }

                        int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, effective_date);

                        string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium dhc for age within 18 - 59
                        if (customer_age >= 18 && customer_age < 60)
                        {
                            decimal original_dhc_premium = plan.DHC_Option_Value;

                            //if remain days < 365
                            if (remain_days < 365)
                            {
                                original_dhc_premium = Math.Ceiling((original_dhc_premium * remain_days) / 365);
                            }

                            total_original_dhc_premium += original_dhc_premium;

                            bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                            employee_premium_dhc.Premium_Type = "DHC";
                            employee_premium_dhc.Premium = original_dhc_premium.ToString();
                            employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                            employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_dhc.Sum_Insured = dt.Rows[k][5].ToString();
                            }
                            else
                            {
                                employee_premium_dhc.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (original_dhc_premium > 0) //Insert only dhc premium > 0
                            {
                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                    Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                                    return;
                                }
                            }

                        }

                    }//End loop dt

                    //Get discount for each premium type

                    //if discount > 0
                    if (discount > 0)
                    {

                        //calculate discount for accidental 100 plus
                        if (plan.Accidental_100Plus_Option_Value == 1)
                        {
                            total_accidental_100plus_premium_discount = Math.Floor((total_original_accidental_100plus_premium * discount) / 100);

                        }

                        //calculate discount for tpd
                        if (plan.TPD_Option_Value == 1)
                        {
                            total_tpd_premium_discount = Math.Floor((total_original_tpd_premium * discount) / 100);

                        }

                        //calculate discount for dhc
                        if (plan.DHC_Option_Value > 0)
                        {
                            total_dhc_premium_discount = Math.Floor((total_original_dhc_premium * discount) / 100);

                        }


                        total_life_premium_discount = Math.Floor((total_original_life_premium * discount) / 100);


                    }

                    //Get Tax Amount
                    total_accidental_100plus_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_accidental_100plus_premium, total_accidental_100plus_premium_discount);
                    total_life_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_life_premium, total_life_premium_discount);
                    total_tpd_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_tpd_premium, total_tpd_premium_discount);
                    total_dhc_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_dhc_premium, total_dhc_premium_discount);

                    //Premium after discount
                    total_accidental_100plus_premium = total_original_accidental_100plus_premium - total_accidental_100plus_premium_discount;
                    total_life_premium = total_original_life_premium - total_life_premium_discount;
                    total_tpd_premium = total_original_tpd_premium - total_tpd_premium_discount;
                    total_dhc_premium = total_original_dhc_premium - total_dhc_premium_discount;

                    //Insert prem pay
                    bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
                    prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                    prem_pay.Prem_Year = 1;
                    prem_pay.Prem_Lot = 1;
                    prem_pay.Pay_Mode_ID = 1;
                    prem_pay.Pay_Date = effective_date;
                    prem_pay.Office_ID = channel_location_id;
                    prem_pay.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                    prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                    prem_pay.Due_Date = expiry_date.AddDays(1);
                    prem_pay.Created_On = DateTime.Now;
                    prem_pay.Created_Note = "";
                    prem_pay.Created_By = hdfusername.Value;
                    prem_pay.Amount = Convert.ToDouble(total_dhc_premium + total_life_premium + total_tpd_premium + total_accidental_100plus_premium);
                    prem_pay.Payment_Code = payment_code;

                    if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(prem_pay, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                        return;
                    }

                    //update total premium in premium temporary
                    da_gtli_premium_temporary.UpdatePremium(total_sum_insure, Convert.ToDouble(total_life_premium), Convert.ToDouble(total_tpd_premium), Convert.ToDouble(total_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium), Convert.ToDouble(total_accidental_100plus_premium_discount), Convert.ToDouble(total_life_premium_discount), Convert.ToDouble(total_tpd_premium_discount), Convert.ToDouble(total_dhc_premium_discount), Convert.ToDouble(total_original_accidental_100plus_premium), Convert.ToDouble(total_original_life_premium), Convert.ToDouble(total_original_tpd_premium), Convert.ToDouble(total_original_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium_tax_amount), Convert.ToDouble(total_life_premium_tax_amount), Convert.ToDouble(total_tpd_premium_tax_amount), Convert.ToDouble(total_dhc_premium_tax_amount), gtli_premium.GTLI_Premium_ID);

                    Clear("", "");

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy successfull.')", true);

                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please upload an excel file that contains employee data.')", true);
                Clear("", "");
            }


        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please contact system admin for problem diagnosis.')", true);
            Log.AddExceptionToLog("Error in function [btnCreate_Click], page [register_employee]. Details: " + ex.Message);
        }

    }
    */
    #endregion

    //Register employees to plan
    protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
    {
       
        //'1. read excel file
        //'2. save new gtli policy for this company (new policy number)
        //'3. save all employee data into employee table
        //'4. calculate premium for each into then save into gtli_employee_premium table
        //'5. get total premium then save to premium table

        string channel_location_id = "0D696111-2590-4FA2-BCE6-C8B2D46648C9"; //Camlife HQ
        string channel_channel_item_id = "016AE1FC-CF77-461A-92F8-6C7605D0A648";
        try
        {
            
            //check if file upload contain any file                      
            if ((uploadedDoc.PostedFile != null) && !string.IsNullOrEmpty(uploadedDoc.PostedFile.FileName))
            {
                //Check company
                bool check_company = da_gtli_company.CheckCompany(txtCompany.Text.Trim());

                if (!check_company)
                {
                    //Company not found
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This company is not registered.')", true);
                    Clear("", "");
                    return;
                }

                //Get plan by plan id
                bl_gtli_plan plan = new bl_gtli_plan();
                plan = da_gtli_plan.GetPlan(hdfPlanID.Value);

                string save_path = "~/Upload/GTLI/CustomerData/";
                string file_name = Path.GetFileName(uploadedDoc.PostedFile.FileName);
                string content_type = uploadedDoc.PostedFile.ContentType;
                int content_length = uploadedDoc.PostedFile.ContentLength;

                //Save file upload
                uploadedDoc.PostedFile.SaveAs(Server.MapPath(save_path + file_name));
                string version = Path.GetExtension(file_name);

                string file_path = Server.MapPath(save_path + file_name).ToString();

                DateTime effective_date;
                DateTime expiry_date;
                int certificate_number = 0;
                int old_certificate_number = 0;
                string policy_number = "";
                int policy_year = 1;
                double total_sum_insure = 0;

                //should be automatically get from quotation system.
                string payment_code = txtPaymentCode.Text.Trim();

                int dhc_option_value = plan.DHC_Option_Value;

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd/MM/yyyy";
                dtfi.DateSeparator = "/";

                DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
                dtfi2.ShortDatePattern = "MM/dd/yyyy";
                dtfi2.DateSeparator = "/";

                //check effective date
                if (!Helper.CheckDateFormat(txtEffectiveDate.Text.Trim()))
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Invalid Effective Date Format.')", true);
                    Clear("", "");
                    return;
                }
                else
                {
                    effective_date = Convert.ToDateTime(txtEffectiveDate.Text, dtfi);

                }


                DataTable dt = null;

                if (version == ".xls")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + file_path + "';Extended Properties=Excel 8.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [GClientData$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[GClientData$]");
                    MyConnection.Close();

                    dt = DtSet.Tables[0];
                }
                else if (version == ".xlsx")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + file_path + "';Extended Properties=Excel 12.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [GClientData$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[GClientData$]");
                    MyConnection.Close();

                    dt = DtSet.Tables[0];
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please upload an excel file that contains employee data.')", true);
                    Clear("", "");
                    return;
                }

                string contact_id = "0";
                string company_id = "0";
                decimal total_life_premium = 0;
                decimal total_tpd_premium = 0;
                decimal total_dhc_premium = 0;
                decimal total_accidental_100plus_premium = 0;
                decimal discount = Convert.ToDecimal(txtDiscountAmount.Text.Trim());

                decimal total_original_life_premium = 0;
                decimal total_original_tpd_premium = 0;
                decimal total_original_dhc_premium = 0;
                decimal total_original_accidental_100plus_premium = 0;

                decimal total_accidental_100plus_premium_tax_amount = 0;
                decimal total_life_premium_tax_amount = 0;
                decimal total_tpd_premium_tax_amount = 0;
                decimal total_dhc_premium_tax_amount = 0;

                decimal total_accidental_100plus_premium_discount = 0;
                decimal total_life_premium_discount = 0;
                decimal total_tpd_premium_discount = 0;
                decimal total_dhc_premium_discount = 0;

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    if (dt.Rows[i][1].ToString().Trim().Equals(""))
                    {
                        lblMessage.Text = "Please check your input of employee name field then try again. Row number: '" + (i + 2) + ")";
                        Clear("", "");
                        return;
                    }

                    if (dt.Rows[i][2].ToString().Trim().Equals(""))
                    {
                        lblMessage.Text = "Please check your input for gender field then try again. Row number: '" + (i + 2) + ")";
                        Clear("", "");
                        return;
                    }


                    if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                    {
                        lblMessage.Text = "Please check your input for date field then try again. Row number: '" + (i + 2) + ")";
                        Clear("", "");
                        return;
                    }

                    if (plan.Sum_Insured == 0) // 0 == dynamic sum insure
                    {
                        if (!Helper.IsNumeric(dt.Rows[i][5].ToString()))
                        {
                            lblMessage.Text = "Please check your input for sum insure field then try again. Row number: '" + (i + 2) + ")";
                            Clear("", "");
                            return;
                        }
                    }

                }
                int remain_days = 365;

                bool add_policy = false;

                //found company, get company ID by name
                company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompany.Text.Trim());

                //get contact_id
                bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(company_id);
                contact_id = contact.GTLI_Contact_ID;

                string gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                //Check gtli policy by company id
                if (da_gtli_policy.CheckCompanyId(company_id))
                {
                    //Case This company already has policy -> check last policy status by this company
                    bl_gtli_policy last_policy = new bl_gtli_policy();
                    last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(company_id);

                    string policy_status = "";
                    if (ckbRenew.Checked)
                    {

                        policy_status = "MAT";

                    }
                    else {

                      policy_status=  da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);
                    }

                    if (policy_status == "IF")
                    {

                        gtli_policy_temporary_id = last_policy.GTLI_Policy_ID;

                        if (da_gtli_policy.CheckPlanIdByPlanIDAndPolicyID(hdfPlanID.Value, last_policy.GTLI_Policy_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This plan is already registered.')", true);
                            Clear("", "");
                            return;
                        }
                        else
                        {
                            policy_number = last_policy.Policy_Number;
                            policy_year = da_gtli_premium.GetGTLIPolicyYearByPolicyID(last_policy.GTLI_Policy_ID);
                            certificate_number = da_gtli_certificate.GetGTLILastCertificateNumberByID(company_id, last_policy.Policy_Number);
                            expiry_date = last_policy.Expiry_Date;

                            TimeSpan mytimespan = last_policy.Expiry_Date.Subtract(effective_date);

                            remain_days = mytimespan.Days + 1;
                        }
                    }
                    else if (policy_status == "TER")
                    {
                        //Terminate -> add new policy with different policy number
                        certificate_number = 0;
                        policy_year = 1;
                        policy_number = "";
                        gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                        //Expiry date for 365 days
                        expiry_date = effective_date.AddYears(1);

                        //substract one day
                        expiry_date = expiry_date.AddDays(-1);

                        //Insert GTLI Policy
                        bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                        gtli_policy_temporary.Expiry_Date = expiry_date;
                        gtli_policy_temporary.Effective_Date = effective_date;
                        gtli_policy_temporary.Agreement_date = DateTime.Now;
                        gtli_policy_temporary.Issue_Date = DateTime.Now;
                        gtli_policy_temporary.Created_By = hdfusername.Value;
                        gtli_policy_temporary.Created_Note = "";
                        gtli_policy_temporary.Created_On = DateTime.Now;
                        gtli_policy_temporary.DHC_Premium = 0;
                        gtli_policy_temporary.GTLI_Company_ID = company_id;
                        gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                        gtli_policy_temporary.Life_Premium = 0;
                        gtli_policy_temporary.Policy_Number = "";
                        gtli_policy_temporary.TPD_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Original_DHC_Premium = 0;
                        gtli_policy_temporary.Original_Life_Premium = 0;
                        gtli_policy_temporary.Original_TPD_Premium = 0;
                        gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                        gtli_policy_temporary.Life_Premium_Discount = 0;
                        gtli_policy_temporary.TPD_Premium_Discount = 0;
                        gtli_policy_temporary.DHC_Premium_Discount = 0;


                        add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                        if (!add_policy)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear("", "");
                            return;
                        }


                    }
                    else if (policy_status == "MAT")
                    {
                        //Maturity -> renew policy instead
                        //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please renew policy instead. Or Terminate current policy first.')", true);
                        //Clear("", "");
                        //return;
                        certificate_number = da_gtli_certificate.GetGTLILastCertificateNumberByID(last_policy.GTLI_Company_ID, last_policy.Policy_Number);
                        policy_year = da_gtli_premium.GetGTLILastPolicyYearByPolicyNumber(last_policy.Policy_Number);
                        policy_number = "";
                        gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                        //Expiry date for 365 days
                        expiry_date = effective_date.AddYears(1);

                        //substract one day
                        expiry_date = expiry_date.AddDays(-1);

                        //Insert GTLI Policy
                        bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                        gtli_policy_temporary.Expiry_Date = expiry_date;
                        gtli_policy_temporary.Effective_Date = effective_date;
                        gtli_policy_temporary.Agreement_date = DateTime.Now;
                        gtli_policy_temporary.Issue_Date = DateTime.Now;
                        gtli_policy_temporary.Created_By = hdfusername.Value;
                        gtli_policy_temporary.Created_Note = "";
                        gtli_policy_temporary.Created_On = DateTime.Now;
                        gtli_policy_temporary.DHC_Premium = 0;
                        gtli_policy_temporary.GTLI_Company_ID = company_id;
                        gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                        gtli_policy_temporary.Life_Premium = 0;
                        gtli_policy_temporary.Policy_Number = last_policy.Policy_Number;
                        gtli_policy_temporary.TPD_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Original_DHC_Premium = 0;
                        gtli_policy_temporary.Original_Life_Premium = 0;
                        gtli_policy_temporary.Original_TPD_Premium = 0;
                        gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                        gtli_policy_temporary.Life_Premium_Discount = 0;
                        gtli_policy_temporary.TPD_Premium_Discount = 0;
                        gtli_policy_temporary.DHC_Premium_Discount = 0;


                        add_policy = da_gtli_policy_temporary.InsertReNewPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                        if (!add_policy)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear("", "");
                            return;
                        }

                        //GTLI Premium for Renew Policy
                        bl_gtli_premium gtli_renew_premium = new bl_gtli_premium();
                        gtli_renew_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                        gtli_renew_premium.Channel_Location_ID = channel_location_id;
                        gtli_renew_premium.Created_By = hdfusername.Value;
                        gtli_renew_premium.Created_On = DateTime.Now;
                        gtli_renew_premium.DHC_Option_Value = Convert.ToInt32(dhc_option_value);
                        gtli_renew_premium.DHC_Premium = 0;
                        gtli_renew_premium.Effective_Date = effective_date;
                        gtli_renew_premium.Expiry_Date = expiry_date;
                        gtli_renew_premium.GTLI_Plan_ID = hdfPlanID.Value;
                        gtli_renew_premium.GTLI_Policy_ID = gtli_policy_temporary_id;
                        gtli_renew_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                        gtli_renew_premium.Life_Premium = 0;
                        gtli_renew_premium.Pay_Mode_ID = 1;
                        gtli_renew_premium.Policy_Year = policy_year;
                        gtli_renew_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                        gtli_renew_premium.Sum_Insured = plan.Sum_Insured;
                        gtli_renew_premium.TPD_Premium = 0;
                        gtli_renew_premium.Transaction_Staff_Number = dt.Rows.Count;
                        gtli_renew_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);
                        gtli_renew_premium.Accidental_100Plus_Premium = 0;
                        gtli_renew_premium.Accidental_100Plus_Premium_Discount = 0;
                        gtli_renew_premium.DHC_Premium_Discount = 0;
                        gtli_renew_premium.Life_Premium_Discount = 0;
                        gtli_renew_premium.TPD_Premium_Discount = 0;
                        gtli_renew_premium.Discount = Convert.ToDouble(discount);
                        gtli_renew_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_renew_premium.Life_Premium_Tax_Amount = 0;
                        gtli_renew_premium.TPD_Premium_Tax_Amount = 0;
                        gtli_renew_premium.DHC_Premium_Tax_Amount = 0;
                        gtli_renew_premium.Original_Accidental_100Plus_Premium = 0;
                        gtli_renew_premium.Original_DHC_Premium = 0;
                        gtli_renew_premium.Original_Life_Premium = 0;
                        gtli_renew_premium.Original_TPD_Premium = 0;

                        //365 days add then transaction type = 1                                
                        gtli_renew_premium.Transaction_Type = 1;

                        if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_renew_premium, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, "");
                            return;

                        }

                        //Loop excel upload data
                        for (int k = 0; k <= dt.Rows.Count - 1; k++)
                        {
                            DateTime dob = Convert.ToDateTime(dt.Rows[k][3], dtfi2);

                            ////calculate premium new rate                        
                            //decimal original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                            decimal original_life_premium = 0;

                            if (last_policy.Policy_Number == "GL00000001" || last_policy.Policy_Number == "GL00000002" || last_policy.Policy_Number == "GL00000003" || last_policy.Policy_Number == "GL00000004" || last_policy.Policy_Number == "GL00000005" || last_policy.Policy_Number == "GL00000006" || last_policy.Policy_Number == "GL00000007" || last_policy.Policy_Number == "GL00000008")
                            {
                                original_life_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else if (last_policy.Policy_Number == "GL00000009" || last_policy.Policy_Number == "GL00000010")
                            {
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else if (last_policy.Policy_Number == "GL00000011" || last_policy.Policy_Number == "GL00000012" || last_policy.Policy_Number == "GL00000013")
                            {
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else 
                            {
                                //new rate effected from 12/04/2016
                                original_life_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "11", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }

                            //if remain days < 365
                            if (remain_days < 365)
                            {
                                //calculate premium for remaining days
                                original_life_premium = Math.Ceiling((original_life_premium * remain_days) / 365);
                            }

                            //Insert Certificate
                            bl_gtli_certificate certificate = new bl_gtli_certificate();
                             
                            certificate.GTLI_Company_ID = company_id;
                            
                            certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");

                            //get old certificate number for old employee
                            old_certificate_number = da_gtli_certificate.GetGTLICertificateNumberByPolicyIDAndEmployeeName(last_policy.GTLI_Policy_ID, dt.Rows[k][1].ToString().Trim());

                            if (old_certificate_number != 0)
                            {
                                certificate.Certificate_Number = old_certificate_number;
                            }
                            else
                            {
                                if (original_life_premium != 0)
                                {
                                    //certificate_number += 1;
                                    //certificate.Certificate_Number = certificate_number;

                                    //last certificate
                                    int lastCertificate = 0;
                                    int lastCertificateTemporary = 0;

                                    lastCertificate = da_gtli_employee.GetLastCertificateNoByCompanyID(company_id);
                                    //lastCertificateTemporary from temp table
                                    lastCertificateTemporary = da_gtli_certificate_temporary.GetLastCertificateTemporayNumber(company_id);
                                    
                                    if (lastCertificate > lastCertificateTemporary)
                                    {

                                        certificate.Certificate_Number = lastCertificate + 1;

                                    }
                                    else
                                    {

                                        certificate.Certificate_Number = lastCertificateTemporary + 1;

                                    }
                                    
                                }
                                else
                                {
                                   
                                    certificate.Certificate_Number = 0;
                                }
                            }



                            if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_renew_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                return;
                            }

                            //Insert Employee
                            bl_gtli_employee employee = new bl_gtli_employee();
                            employee.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;
                            employee.Position = dt.Rows[k][4].ToString().Trim();
                            employee.Gender = dt.Rows[k][2].ToString().Trim();
                            employee.Employee_Name = dt.Rows[k][1].ToString().Trim();
                            employee.Employee_ID = dt.Rows[k][0].ToString().Trim();
                            employee.DOB = dob;
                            employee.Customer_Status = 1;

                            if (!da_gtli_employee_temporary.InsertEmployeeTemporary(employee, hdfuserid.Value, gtli_renew_premium.GTLI_Premium_ID))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                return;
                            }


                            total_original_life_premium += original_life_premium;

                            string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium life
                            bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                            employee_premium_life.Premium_Type = "Death";
                            employee_premium_life.Premium = original_life_premium.ToString();
                            employee_premium_life.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                            employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                            employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                            if (original_life_premium != 0)
                            {
                                if (plan.Sum_Insured == 0)
                                {
                                    employee_premium_life.Sum_Insured = dt.Rows[k][5].ToString();

                                }
                                else
                                {
                                    employee_premium_life.Sum_Insured = plan.Sum_Insured.ToString();
                                }
                            }
                            else
                            {
                                employee_premium_life.Sum_Insured = "0";
                            }

                            total_sum_insure += Convert.ToDouble(employee_premium_life.Sum_Insured.Trim());

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                return;
                            }

                            //if has Accicental 100Plus
                            if (plan.Accidental_100Plus_Option_Value == 1)
                            {
                                //decimal original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                //calculate premium TPD
                                decimal original_accidental_100plus_premium = 0;

                                if (last_policy.Policy_Number == "GL00000001" || last_policy.Policy_Number == "GL00000002" || last_policy.Policy_Number == "GL00000003" || last_policy.Policy_Number == "GL00000004" || last_policy.Policy_Number == "GL00000005" || last_policy.Policy_Number == "GL00000006" || last_policy.Policy_Number == "GL00000007" || last_policy.Policy_Number == "GL00000008")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                }
                                else if (last_policy.Policy_Number == "GL00000009" || last_policy.Policy_Number == "GL00000010")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                }
                                else if (last_policy.Policy_Number == "GL00000011" || last_policy.Policy_Number == "GL00000012" || last_policy.Policy_Number == "GL00000013")
                                {
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                }
                                else
                                { 
                                    //new rate effected from 12/04/2016
                                    original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "12", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                                }


                                //if remain days < 365
                                if (remain_days < 365)
                                {
                                    //calculate premium TPD for remaining days
                                    original_accidental_100plus_premium = Math.Ceiling((original_accidental_100plus_premium * remain_days) / 365);
                                }

                                total_original_accidental_100plus_premium += original_accidental_100plus_premium;

                                string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                                //Insert employee premium tpd
                                bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                                employee_premium_accidental_100plus.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                                employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                                employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                                employee_premium_accidental_100plus.Premium = original_accidental_100plus_premium.ToString();
                                employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                                if (plan.Sum_Insured == 0)
                                {
                                    employee_premium_accidental_100plus.Sum_Insured = dt.Rows[k][5].ToString();
                                }
                                else
                                {
                                    employee_premium_accidental_100plus.Sum_Insured = plan.Sum_Insured.ToString();
                                }

                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                                    Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                    return;
                                }

                            }

                            //if has TPD
                            if (plan.TPD_Option_Value == 1)
                            {
                                //calculate premium TPD
                                decimal original_tpd_premium = 0;

                                if (last_policy.Policy_Number == "GL00000001" || last_policy.Policy_Number == "GL00000002" || last_policy.Policy_Number == "GL00000003" || last_policy.Policy_Number == "GL00000004" || last_policy.Policy_Number == "GL00000005" || last_policy.Policy_Number == "GL00000006" || last_policy.Policy_Number == "GL00000007" || last_policy.Policy_Number == "GL00000008")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                }
                                else if (last_policy.Policy_Number == "GL00000009" || last_policy.Policy_Number == "GL00000010")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                }
                                else if (last_policy.Policy_Number == "GL00000011" || last_policy.Policy_Number == "GL00000012" || last_policy.Policy_Number == "GL00000013")
                                {
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                }
                                else
                                { 
                                
                                    //new rate effected from 12/04/2016
                                    original_tpd_premium = Calculation.CalculateGTLINewPremiumRate3(dob, dt.Rows[k][2].ToString().Trim(), Convert.ToDecimal(plan.Sum_Insured), "13", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                                }
                                //if remain days < 365
                                if (remain_days < 365)
                                {
                                    //calculate premium TPD for remaining days
                                    original_tpd_premium = Math.Ceiling((original_tpd_premium * remain_days) / 365);
                                }

                                total_original_tpd_premium += original_tpd_premium;

                                string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                                //Insert employee premium tpd
                                bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                                employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                                employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                                employee_premium_tpd.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                                employee_premium_tpd.Premium = original_tpd_premium.ToString();
                                employee_premium_tpd.Premium_Type = "TPD";

                                if (plan.Sum_Insured == 0)
                                {
                                    employee_premium_tpd.Sum_Insured = dt.Rows[k][5].ToString();
                                }
                                else
                                {
                                    employee_premium_tpd.Sum_Insured = plan.Sum_Insured.ToString();
                                }

                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                                    Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                    return;
                                }

                            }

                            int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, effective_date);

                            string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium dhc for age within 18 - 59
                            if (customer_age >= 18 && customer_age < 60)
                            {
                                decimal original_dhc_premium = plan.DHC_Option_Value;

                                //if remain days < 365
                                if (remain_days < 365)
                                {
                                    original_dhc_premium = Math.Ceiling((original_dhc_premium * remain_days) / 365);
                                }

                                total_original_dhc_premium += original_dhc_premium;

                                bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                                employee_premium_dhc.Premium_Type = "DHC";
                                employee_premium_dhc.Premium = original_dhc_premium.ToString();
                                employee_premium_dhc.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                                employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                                employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                                if (plan.Sum_Insured == 0)
                                {
                                    employee_premium_dhc.Sum_Insured = dt.Rows[k][5].ToString();
                                }
                                else
                                {
                                    employee_premium_dhc.Sum_Insured = plan.Sum_Insured.ToString();
                                }

                                if (original_dhc_premium > 0) //Insert only dhc premium > 0
                                {
                                    if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                                    {
                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                                        Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                        return;
                                    }
                                }

                            }

                        }//End loop dt

                        //Get discount for each premium type

                        //if discount > 0
                        if (discount > 0)
                        {

                            //calculate discount for accidental 100 plus
                            if (plan.Accidental_100Plus_Option_Value == 1)
                            {
                                total_accidental_100plus_premium_discount = Math.Floor((total_original_accidental_100plus_premium * discount) / 100);

                            }

                            //calculate discount for tpd
                            if (plan.TPD_Option_Value == 1)
                            {
                                total_tpd_premium_discount = Math.Floor((total_original_tpd_premium * discount) / 100);

                            }

                            //calculate discount for dhc
                            if (plan.DHC_Option_Value > 0)
                            {
                                total_dhc_premium_discount = Math.Floor((total_original_dhc_premium * discount) / 100);

                            }


                            total_life_premium_discount = Math.Floor((total_original_life_premium * discount) / 100);


                        }

                        //Get Tax Amount
                        total_accidental_100plus_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_accidental_100plus_premium, total_accidental_100plus_premium_discount);
                        total_life_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_life_premium, total_life_premium_discount);
                        total_tpd_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_tpd_premium, total_tpd_premium_discount);
                        total_dhc_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_dhc_premium, total_dhc_premium_discount);

                        //Premium after discount
                        total_accidental_100plus_premium = total_original_accidental_100plus_premium - total_accidental_100plus_premium_discount;
                        total_life_premium = total_original_life_premium - total_life_premium_discount;
                        total_tpd_premium = total_original_tpd_premium - total_tpd_premium_discount;
                        total_dhc_premium = total_original_dhc_premium - total_dhc_premium_discount;

                        //Insert renew prem pay

                        //Get payment code

                        string policy_id = da_gtli_policy.GetGTLIPolicyIDByPremiumID(gtli_renew_premium.GTLI_Premium_ID);

                        string prem_pay_payment_code = da_gtli_policy_prem_pay.GetPaymentCodeByPolicyID(policy_id);
                        string prem_return_payment_code = da_gtli_policy_prem_return.GetPaymentCodeByPolicyID(policy_id);

                        if (prem_pay_payment_code == "" && prem_return_payment_code == "")
                        {
                            payment_code = da_payment_code.GetNewPaymentCode();
                        }
                        else
                        {
                            payment_code = prem_pay_payment_code;
                        }
                        //End Get payment code


                        bl_gtli_policy_prem_pay renew_prem_pay = new bl_gtli_policy_prem_pay();
                        renew_prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                        renew_prem_pay.Prem_Year = 1;
                        renew_prem_pay.Prem_Lot = 1;
                        renew_prem_pay.Pay_Mode_ID = 1;
                        renew_prem_pay.Pay_Date = effective_date;
                        renew_prem_pay.Office_ID = channel_location_id;
                        renew_prem_pay.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                        renew_prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                        renew_prem_pay.Due_Date = expiry_date.AddDays(1);
                        renew_prem_pay.Created_On = DateTime.Now;
                        renew_prem_pay.Created_Note = "";
                        renew_prem_pay.Created_By = hdfusername.Value;
                        renew_prem_pay.Amount = Convert.ToDouble(total_dhc_premium + total_life_premium + total_tpd_premium + total_accidental_100plus_premium);
                        renew_prem_pay.Payment_Code = payment_code;

                        if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(renew_prem_pay, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                            return;
                        }

                        //update total premium in premium temporary
                        da_gtli_premium_temporary.UpdatePremium(total_sum_insure, Convert.ToDouble(total_life_premium), Convert.ToDouble(total_tpd_premium), Convert.ToDouble(total_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium), Convert.ToDouble(total_accidental_100plus_premium_discount), Convert.ToDouble(total_life_premium_discount), Convert.ToDouble(total_tpd_premium_discount), Convert.ToDouble(total_dhc_premium_discount), Convert.ToDouble(total_original_accidental_100plus_premium), Convert.ToDouble(total_original_life_premium), Convert.ToDouble(total_original_tpd_premium), Convert.ToDouble(total_original_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium_tax_amount), Convert.ToDouble(total_life_premium_tax_amount), Convert.ToDouble(total_tpd_premium_tax_amount), Convert.ToDouble(total_dhc_premium_tax_amount), gtli_renew_premium.GTLI_Premium_ID);

                        Clear("", "");

                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy successfull.')", true);
                        return;
                    }
                   
                    else
                    {
                        //For Other policy status
                        certificate_number = 0;
                        policy_year = 1;
                        policy_number = "";
                        gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                        //Expiry date for 365 days
                        expiry_date = effective_date.AddYears(1);

                        //substract one day
                        expiry_date = expiry_date.AddDays(-1);

                        //Insert GTLI Policy
                        bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                        gtli_policy_temporary.Expiry_Date = expiry_date;
                        gtli_policy_temporary.Effective_Date = effective_date;
                        gtli_policy_temporary.Agreement_date = DateTime.Now;
                        gtli_policy_temporary.Issue_Date = DateTime.Now;
                        gtli_policy_temporary.Created_By = hdfusername.Value;
                        gtli_policy_temporary.Created_Note = "";
                        gtli_policy_temporary.Created_On = DateTime.Now;
                        gtli_policy_temporary.DHC_Premium = 0;
                        gtli_policy_temporary.GTLI_Company_ID = company_id;
                        gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                        gtli_policy_temporary.Life_Premium = 0;
                        gtli_policy_temporary.Policy_Number = "";
                        gtli_policy_temporary.TPD_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                        gtli_policy_temporary.Original_DHC_Premium = 0;
                        gtli_policy_temporary.Original_Life_Premium = 0;
                        gtli_policy_temporary.Original_TPD_Premium = 0;
                        gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                        gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                        gtli_policy_temporary.Life_Premium_Discount = 0;
                        gtli_policy_temporary.TPD_Premium_Discount = 0;
                        gtli_policy_temporary.DHC_Premium_Discount = 0;

                        add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                        if (!add_policy)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear("", "");
                            return;
                        }
                    }

                }

                else //Case First Policy for this company
                {
                    certificate_number = 0;
                    policy_year = 1;
                    policy_number = "";
                    gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                    //Expiry date for 365 days
                    expiry_date = effective_date.AddYears(1);

                    //substract one day
                    expiry_date = expiry_date.AddDays(-1);

                    //Insert GTLI Policy
                    bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();

                    gtli_policy_temporary.Expiry_Date = expiry_date;
                    gtli_policy_temporary.Effective_Date = effective_date;
                    gtli_policy_temporary.Agreement_date = DateTime.Now;
                    gtli_policy_temporary.Issue_Date = DateTime.Now;
                    gtli_policy_temporary.Created_By = hdfusername.Value;
                    gtli_policy_temporary.Created_Note = "";
                    gtli_policy_temporary.Created_On = DateTime.Now;
                    gtli_policy_temporary.DHC_Premium = 0;
                    gtli_policy_temporary.GTLI_Company_ID = company_id;
                    gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                    gtli_policy_temporary.Life_Premium = 0;
                    gtli_policy_temporary.Policy_Number = "";
                    gtli_policy_temporary.TPD_Premium = 0;
                    gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                    gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                    gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                    gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                    gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                    gtli_policy_temporary.Original_DHC_Premium = 0;
                    gtli_policy_temporary.Original_Life_Premium = 0;
                    gtli_policy_temporary.Original_TPD_Premium = 0;
                    gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                    gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                    gtli_policy_temporary.Life_Premium_Discount = 0;
                    gtli_policy_temporary.TPD_Premium_Discount = 0;
                    gtli_policy_temporary.DHC_Premium_Discount = 0;


                    add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                    if (!add_policy)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear("", "");
                        return;
                    }
                }


                //GTLI Premium
                bl_gtli_premium gtli_premium = new bl_gtli_premium();
                gtli_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                gtli_premium.Channel_Location_ID = channel_location_id;
                gtli_premium.Created_By = hdfusername.Value;
                gtli_premium.Created_On = DateTime.Now;
                gtli_premium.DHC_Option_Value = Convert.ToInt32(dhc_option_value);
                gtli_premium.DHC_Premium = 0;
                gtli_premium.Effective_Date = effective_date;
                gtli_premium.Expiry_Date = expiry_date;
                gtli_premium.GTLI_Plan_ID = hdfPlanID.Value;
                gtli_premium.GTLI_Policy_ID = gtli_policy_temporary_id;
                gtli_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                gtli_premium.Life_Premium = 0;
                gtli_premium.Pay_Mode_ID = 1;
                gtli_premium.Policy_Year = 1;
                gtli_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                gtli_premium.Sum_Insured = plan.Sum_Insured;
                gtli_premium.TPD_Premium = 0;
                gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                gtli_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);
                gtli_premium.Accidental_100Plus_Premium = 0;
                gtli_premium.Accidental_100Plus_Premium_Discount = 0;
                gtli_premium.DHC_Premium_Discount = 0;
                gtli_premium.Life_Premium_Discount = 0;
                gtli_premium.TPD_Premium_Discount = 0;
                gtli_premium.Discount = Convert.ToDouble(discount);
                gtli_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                gtli_premium.Life_Premium_Tax_Amount = 0;
                gtli_premium.TPD_Premium_Tax_Amount = 0;
                gtli_premium.DHC_Premium_Tax_Amount = 0;
                gtli_premium.Original_Accidental_100Plus_Premium = 0;
                gtli_premium.Original_DHC_Premium = 0;
                gtli_premium.Original_Life_Premium = 0;
                gtli_premium.Original_TPD_Premium = 0;

                //365 days add then transaction type = 1                                
                gtli_premium.Transaction_Type = 1;

                if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                    Clear(gtli_policy_temporary_id, "");
                    return;

                }

                //Loop excel upload data
                for (int k = 0; k <= dt.Rows.Count - 1; k++)
                {
                    DateTime dob = Convert.ToDateTime(dt.Rows[k][3], dtfi2);

                    //Insert Certificate
                    bl_gtli_certificate certificate = new bl_gtli_certificate();

                    certificate.GTLI_Company_ID = company_id;
                    certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");

                    certificate_number += 1;

                    certificate.Certificate_Number = certificate_number;


                    if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                        return;
                    }

                    //Insert Employee
                    bl_gtli_employee employee = new bl_gtli_employee();
                    employee.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;
                    employee.Position = dt.Rows[k][4].ToString().Trim();
                    employee.Gender = dt.Rows[k][2].ToString().Trim();
                    employee.Employee_Name = dt.Rows[k][1].ToString().Trim();
                    employee.Employee_ID = dt.Rows[k][0].ToString().Trim();
                    employee.DOB = dob;
                    employee.Customer_Status = 1;

                    if (!da_gtli_employee_temporary.InsertEmployeeTemporary(employee, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                        return;
                    }


                    //calculate premium new rate                        
                   // decimal original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                   
                    decimal original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "11", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                    //if remain days < 365
                    if (remain_days < 365)
                    {
                        //calculate premium for remaining days
                        original_life_premium = Math.Ceiling((original_life_premium * remain_days) / 365);
                    }

                    total_original_life_premium += original_life_premium;

                    string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                    //Insert employee premium life
                    bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                    employee_premium_life.Premium_Type = "Death";
                    employee_premium_life.Premium = original_life_premium.ToString();
                    employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                    employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                    employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                    if (plan.Sum_Insured == 0)
                    {
                        employee_premium_life.Sum_Insured = dt.Rows[k][5].ToString();

                    }
                    else
                    {
                        employee_premium_life.Sum_Insured = plan.Sum_Insured.ToString();
                    }

                    total_sum_insure += Convert.ToDouble(employee_premium_life.Sum_Insured.Trim());

                    if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                        return;
                    }

                    //if has Accicental 100Plus
                    if (plan.Accidental_100Plus_Option_Value == 1)
                    {
                        decimal original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "12", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                        //if remain days < 365
                        if (remain_days < 365)
                        {
                            //calculate premium TPD for remaining days
                            original_accidental_100plus_premium = Math.Ceiling((original_accidental_100plus_premium * remain_days) / 365);
                        }

                        total_original_accidental_100plus_premium += original_accidental_100plus_premium;

                        string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium tpd
                        bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                        employee_premium_accidental_100plus.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                        employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                        employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_accidental_100plus.Premium = original_accidental_100plus_premium.ToString();
                        employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                        if (plan.Sum_Insured == 0)
                        {
                            employee_premium_accidental_100plus.Sum_Insured = dt.Rows[k][5].ToString();
                        }
                        else
                        {
                            employee_premium_accidental_100plus.Sum_Insured = plan.Sum_Insured.ToString();
                        }

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }

                    }

                    //if has TPD
                    if (plan.TPD_Option_Value == 1)
                    {
                        decimal original_tpd_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "13", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                        //if remain days < 365
                        if (remain_days < 365)
                        {
                            //calculate premium TPD for remaining days
                            original_tpd_premium = Math.Ceiling((original_tpd_premium * remain_days) / 365);
                        }

                        total_original_tpd_premium += original_tpd_premium;

                        string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium tpd
                        bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                        employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                        employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                        employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_tpd.Premium = original_tpd_premium.ToString();
                        employee_premium_tpd.Premium_Type = "TPD";

                        if (plan.Sum_Insured == 0)
                        {
                            employee_premium_tpd.Sum_Insured = dt.Rows[k][5].ToString();
                        }
                        else
                        {
                            employee_premium_tpd.Sum_Insured = plan.Sum_Insured.ToString();
                        }

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                            return;
                        }

                    }

                    int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, effective_date);

                    string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                    //Insert employee premium dhc for age within 18 - 59
                    if (customer_age >= 18 && customer_age < 60)
                    {
                        decimal original_dhc_premium = plan.DHC_Option_Value;

                        //if remain days < 365
                        if (remain_days < 365)
                        {
                            original_dhc_premium = Math.Ceiling((original_dhc_premium * remain_days) / 365);
                        }

                        total_original_dhc_premium += original_dhc_premium;

                        bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                        employee_premium_dhc.Premium_Type = "DHC";
                        employee_premium_dhc.Premium = original_dhc_premium.ToString();
                        employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                        employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                        if (plan.Sum_Insured == 0)
                        {
                            employee_premium_dhc.Sum_Insured = dt.Rows[k][5].ToString();
                        }
                        else
                        {
                            employee_premium_dhc.Sum_Insured = plan.Sum_Insured.ToString();
                        }

                        if (original_dhc_premium > 0) //Insert only dhc premium > 0
                        {
                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                                return;
                            }
                        }

                    }

                }//End loop dt

                //Get discount for each premium type

                //if discount > 0
                if (discount > 0)
                {

                    //calculate discount for accidental 100 plus
                    if (plan.Accidental_100Plus_Option_Value == 1)
                    {
                        total_accidental_100plus_premium_discount = Math.Floor((total_original_accidental_100plus_premium * discount) / 100);

                    }

                    //calculate discount for tpd
                    if (plan.TPD_Option_Value == 1)
                    {
                        total_tpd_premium_discount = Math.Floor((total_original_tpd_premium * discount) / 100);

                    }

                    //calculate discount for dhc
                    if (plan.DHC_Option_Value > 0)
                    {
                        total_dhc_premium_discount = Math.Floor((total_original_dhc_premium * discount) / 100);

                    }


                    total_life_premium_discount = Math.Floor((total_original_life_premium * discount) / 100);


                }

                //Get Tax Amount
                total_accidental_100plus_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_accidental_100plus_premium, total_accidental_100plus_premium_discount);
                total_life_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_life_premium, total_life_premium_discount);
                total_tpd_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_tpd_premium, total_tpd_premium_discount);
                total_dhc_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_dhc_premium, total_dhc_premium_discount);

                //Premium after discount
                total_accidental_100plus_premium = total_original_accidental_100plus_premium - total_accidental_100plus_premium_discount;
                total_life_premium = total_original_life_premium - total_life_premium_discount;
                total_tpd_premium = total_original_tpd_premium - total_tpd_premium_discount;
                total_dhc_premium = total_original_dhc_premium - total_dhc_premium_discount;

                //Insert prem pay
                bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
                prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                prem_pay.Prem_Year = 1;
                prem_pay.Prem_Lot = 1;
                prem_pay.Pay_Mode_ID = 1;
                prem_pay.Pay_Date = effective_date;
                prem_pay.Office_ID = channel_location_id;
                prem_pay.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                prem_pay.Due_Date = expiry_date.AddDays(1);
                prem_pay.Created_On = DateTime.Now;
                prem_pay.Created_Note = "";
                prem_pay.Created_By = hdfusername.Value;
                prem_pay.Amount = Convert.ToDouble(total_dhc_premium + total_life_premium + total_tpd_premium + total_accidental_100plus_premium);
                prem_pay.Payment_Code = payment_code;

                if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(prem_pay, hdfuserid.Value))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                    Clear(gtli_policy_temporary_id, gtli_premium.GTLI_Premium_ID);
                    return;
                }

                //update total premium in premium temporary
                da_gtli_premium_temporary.UpdatePremium(total_sum_insure, Convert.ToDouble(total_life_premium), Convert.ToDouble(total_tpd_premium), Convert.ToDouble(total_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium), Convert.ToDouble(total_accidental_100plus_premium_discount), Convert.ToDouble(total_life_premium_discount), Convert.ToDouble(total_tpd_premium_discount), Convert.ToDouble(total_dhc_premium_discount), Convert.ToDouble(total_original_accidental_100plus_premium), Convert.ToDouble(total_original_life_premium), Convert.ToDouble(total_original_tpd_premium), Convert.ToDouble(total_original_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium_tax_amount), Convert.ToDouble(total_life_premium_tax_amount), Convert.ToDouble(total_tpd_premium_tax_amount), Convert.ToDouble(total_dhc_premium_tax_amount), gtli_premium.GTLI_Premium_ID);

                Clear("", "");

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy successfull.')", true);
            }

        }
        catch (System.Threading.ThreadAbortException)
        {
            // ignore it
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please contact system admin for problem diagnosis.')", true);
            Log.AddExceptionToLog("Error in function [btnCreate_Click], page [register_employee]. Details: " + ex.Message);
        }
    }

    protected void btnReNewPolicy_Click(object sender, EventArgs e)
    {

    }

    //Find Used Days
    private static int CalculateUsedDays(System.DateTime sale_date, System.DateTime date_of_modify)
    {
        TimeSpan mytimespan = date_of_modify.Subtract(sale_date);
        int used_days = mytimespan.Days;

        return used_days;
    }

    //Clear
    protected void Clear(string policy_id, string premium_id)
    {
        txtCompany.Text = "";
        txtEffectiveDate.Text = "";
        txtTotalNumberOfStaff.Text = "";
        ddlSaleAgent.SelectedIndex = 0;

        //ddlOption.SelectedIndex = 0;
        //uploadedDoc.Visible = true;
        //btnSearch.Visible = false;

        //clear gridview
        this.gvActiveMember.DataSource = null;
        this.gvActiveMember.DataBind();
        

        //Delete temporary data
        if (policy_id != "")
        {
            da_gtli_policy_temporary.DeleteGTLIPolicyTemporary(policy_id);
        }

        if (premium_id != "")
        {

            da_gtli_employee_premium_temporary.DeleteEmployeePremiumTemporary(premium_id);
            da_gtli_employee_temporary.DeleteGTLIEmployeeTemporary(premium_id);

            da_gtli_certificate_temporary.DeleteGTLICertificateTemporary(premium_id);
            da_gtli_premium_temporary.DeleteGTLIPremiumTemporary(premium_id);
            da_gtli_policy_prem_pay_temporary.DeleteGTLIPolicyPremPayTemporary(premium_id);
            da_gtli_policy_prem_return_temporary.DeleteGTLIPolicyPremReturnTemporary(premium_id);
        }
    }

    //show employee list
    void showEmployee(bool status) {
        gvActiveMember.Visible = status;
    }
    protected void reNewPolicy()
    {
        string channel_location_id = "0D696111-2590-4FA2-BCE6-C8B2D46648C9"; //Camlife HQ
        string channel_channel_item_id = "016AE1FC-CF77-461A-92F8-6C7605D0A648";
        string company_id = "0";
        int numberOfStaff = 0;
        decimal discount = 0;

        decimal total_original_life_premium = 0;
        decimal total_original_tpd_premium = 0;
        decimal total_original_dhc_premium = 0;
        decimal total_original_accidental_100plus_premium = 0;

        decimal total_accidental_100plus_premium_tax_amount = 0;
        decimal total_life_premium_tax_amount = 0;
        decimal total_tpd_premium_tax_amount = 0;
        decimal total_dhc_premium_tax_amount = 0;

        decimal total_accidental_100plus_premium_discount = 0;
        decimal total_life_premium_discount = 0;
        decimal total_tpd_premium_discount = 0;
        decimal total_dhc_premium_discount = 0;

        decimal total_accidental_100plus_premium = 0;
        decimal total_life_premium = 0;
        decimal total_tpd_premium = 0;
        decimal total_dhc_premium = 0;

        double total_sum_insure = 0;
        string payment_code = "";
        payment_code = txtPaymentCode.Text.Trim();
     

        numberOfStaff = this.numberStaffSelected();
        discount = Convert.ToDecimal(txtDiscountAmount.Text);

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        try
        {

            if (ckbRenew.Checked)
            {

                //Get plan by plan id
                bl_gtli_plan plan = new bl_gtli_plan();
                plan = da_gtli_plan.GetPlan(hdfPlanID.Value);
                int dhc_option_value = plan.DHC_Option_Value;

                int certificate_number = 0;
                int policy_year = 0;
                string gtli_policy_temporary_id = "";
                DateTime effective_date;
                DateTime expiry_date;

                effective_date = Convert.ToDateTime(txtEffectiveDate.Text.Trim(), dtfi);

                //get company ID by name
                company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompany.Text.Trim());

                bl_gtli_policy last_policy = new bl_gtli_policy();
                last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(company_id);

                certificate_number = da_gtli_certificate.GetGTLILastCertificateNumberByID(last_policy.GTLI_Company_ID, last_policy.Policy_Number);
                policy_year = da_gtli_premium.GetGTLILastPolicyYearByPolicyNumber(last_policy.Policy_Number);

                gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                //Expiry date for 365 days
                expiry_date = effective_date.AddYears(1);

                //substract one day
                expiry_date = expiry_date.AddDays(-1);

                //Insert GTLI Policy
                bl_gtli_policy gtli_policy_temporary = new bl_gtli_policy();
                gtli_policy_temporary.Expiry_Date = expiry_date;
                gtli_policy_temporary.Effective_Date = effective_date;
                gtli_policy_temporary.Agreement_date = DateTime.Now;
                gtli_policy_temporary.Issue_Date = DateTime.Now;
                gtli_policy_temporary.Created_By = hdfusername.Value;
                gtli_policy_temporary.Created_Note = "";
                gtli_policy_temporary.Created_On = DateTime.Now;
                gtli_policy_temporary.DHC_Premium = 0;
                gtli_policy_temporary.GTLI_Company_ID = company_id;
                gtli_policy_temporary.GTLI_Policy_ID = gtli_policy_temporary_id;
                gtli_policy_temporary.Life_Premium = 0;
                gtli_policy_temporary.Policy_Number = last_policy.Policy_Number;
                gtli_policy_temporary.TPD_Premium = 0;
                gtli_policy_temporary.Accidental_100Plus_Premium = 0;
                gtli_policy_temporary.Accidental_100Plus_Premium_Tax_Amount = 0;
                gtli_policy_temporary.DHC_Premium_Tax_Amount = 0;
                gtli_policy_temporary.Life_Premium_Tax_Amount = 0;
                gtli_policy_temporary.Original_Accidental_100Plus_Premium = 0;
                gtli_policy_temporary.Original_DHC_Premium = 0;
                gtli_policy_temporary.Original_Life_Premium = 0;
                gtli_policy_temporary.Original_TPD_Premium = 0;
                gtli_policy_temporary.TPD_Premium_Tax_Amount = 0;
                gtli_policy_temporary.Accidental_100Plus_Premium_Discount = 0;
                gtli_policy_temporary.Life_Premium_Discount = 0;
                gtli_policy_temporary.TPD_Premium_Discount = 0;
                gtli_policy_temporary.DHC_Premium_Discount = 0;

                //status of save data into database
                bool add_policy = false;
                add_policy = da_gtli_policy_temporary.InsertReNewPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);


                if (!add_policy)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                    Clear("", "");
                    return;
                }

                //GTLI Premium for Renew Policy
                bl_gtli_premium gtli_renew_premium = new bl_gtli_premium();
                gtli_renew_premium.Channel_Channel_Item_ID = channel_channel_item_id;
                gtli_renew_premium.Channel_Location_ID = channel_location_id;
                gtli_renew_premium.Created_By = hdfusername.Value;
                gtli_renew_premium.Created_On = DateTime.Now;
                gtli_renew_premium.DHC_Option_Value = Convert.ToInt32(dhc_option_value);
                gtli_renew_premium.DHC_Premium = 0;
                gtli_renew_premium.Effective_Date = effective_date;
                gtli_renew_premium.Expiry_Date = expiry_date;
                gtli_renew_premium.GTLI_Plan_ID = hdfPlanID.Value;
                gtli_renew_premium.GTLI_Policy_ID = gtli_policy_temporary_id;
                gtli_renew_premium.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_Temporary_ID", "@GTLI_Premium_ID");
                gtli_renew_premium.Life_Premium = 0;
                gtli_renew_premium.Pay_Mode_ID = 1;
                gtli_renew_premium.Policy_Year = policy_year;
                gtli_renew_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                gtli_renew_premium.Sum_Insured = plan.Sum_Insured;
                gtli_renew_premium.TPD_Premium = 0;
                gtli_renew_premium.Transaction_Staff_Number = numberOfStaff;
                gtli_renew_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);//user input in textbox
                gtli_renew_premium.Accidental_100Plus_Premium = 0;
                gtli_renew_premium.Accidental_100Plus_Premium_Discount = 0;
                gtli_renew_premium.DHC_Premium_Discount = 0;
                gtli_renew_premium.Life_Premium_Discount = 0;
                gtli_renew_premium.TPD_Premium_Discount = 0;
                gtli_renew_premium.Discount = Convert.ToDouble(discount);
                gtli_renew_premium.Accidental_100Plus_Premium_Tax_Amount = 0;
                gtli_renew_premium.Life_Premium_Tax_Amount = 0;
                gtli_renew_premium.TPD_Premium_Tax_Amount = 0;
                gtli_renew_premium.DHC_Premium_Tax_Amount = 0;
                gtli_renew_premium.Original_Accidental_100Plus_Premium = 0;
                gtli_renew_premium.Original_DHC_Premium = 0;
                gtli_renew_premium.Original_Life_Premium = 0;
                gtli_renew_premium.Original_TPD_Premium = 0;

                //365 days add then transaction type = 1                                
                gtli_renew_premium.Transaction_Type = 1;//1=renew

                if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_renew_premium , hdfuserid.Value))
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                    Clear(gtli_policy_temporary_id, "");
                    return;

                }

                //loop employee in grid view
                foreach (GridViewRow row in gvActiveMember.Rows)
                {
                    //check checked box
                    CheckBox ckEmployee;
                    ckEmployee = (CheckBox)row.Cells[0].FindControl("chk1");
                    if (ckEmployee.Checked)
                    {

                        Label lblEmployeeName;
                        lblEmployeeName = (Label)row.Cells[3].FindControl("lblEmployeeName");
                        Label lblEmployeeID;
                        lblEmployeeID = (Label)row.Cells[4].FindControl("lblEmployeeID");
                        Label lblPosition;
                        lblPosition = (Label)row.Cells[5].FindControl("lblPosition");
                        Label lblCertificateNumber;
                        lblCertificateNumber = (Label)row.Cells[0].FindControl("lblCertificateNumber");
                        Label lblGender;
                        lblGender = (Label)row.Cells[6].FindControl("lblGender");
                        Label lblDOB;
                        lblDOB = (Label)row.Cells[7].FindControl("lblDOB");
                        Label lblSumInsure;
                        lblSumInsure = (Label)row.Cells[12].FindControl("lblSumInsure");
                        DateTime dob = Convert.ToDateTime(lblDOB.Text.Trim(), dtfi);

                        ////calculate premium new rate                        
                        //decimal original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                        decimal original_life_premium = 0;

                        if (last_policy.Policy_Number == "GL00000001" || last_policy.Policy_Number == "GL00000002" || last_policy.Policy_Number == "GL00000003" || last_policy.Policy_Number == "GL00000004" || last_policy.Policy_Number == "GL00000005" || last_policy.Policy_Number == "GL00000006" || last_policy.Policy_Number == "GL00000007" || last_policy.Policy_Number == "GL00000008")
                        {
                            original_life_premium = Calculation.CalculateGTLIPremium(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                        }
                        else if (last_policy.Policy_Number == "GL00000009" || last_policy.Policy_Number == "GL00000010")
                        {
                            original_life_premium = Calculation.CalculateGTLINewPremiumRate(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                        }
                        else if (last_policy.Policy_Number == "GL00000011" || last_policy.Policy_Number == "GL00000012" || last_policy.Policy_Number == "GL00000013")
                        {
                            original_life_premium = Calculation.CalculateGTLINewPremiumRate2(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                        }
                        else {
                            //product_id: 13=tpd, 12=100plus, 11=life_premium
                            original_life_premium = Calculation.CalculateGTLINewPremiumRate3(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "11", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                        }


                        //Insert Certificate
                        bl_gtli_certificate certificate = new bl_gtli_certificate();

                        certificate.GTLI_Company_ID = company_id;
                        certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");
                        certificate.Certificate_Number = Convert.ToInt32(lblCertificateNumber.Text.Trim());

                        if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_renew_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                            return;
                        }

                        //Insert Employee
                        bl_gtli_employee employee = new bl_gtli_employee();
                        employee.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;
                        employee.Position = lblPosition.Text.Trim();
                        employee.Gender = lblGender.Text.Trim();
                        employee.Employee_Name = lblEmployeeName.Text.Trim();
                        employee.Employee_ID = lblEmployeeID.Text.Trim();
                        employee.DOB = dob;
                        employee.Customer_Status = 1;

                        if (!da_gtli_employee_temporary.InsertEmployeeTemporary(employee, hdfuserid.Value, gtli_renew_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                            return;
                        }


                        total_original_life_premium += original_life_premium;

                        string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium life
                        bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                        employee_premium_life.Premium_Type = "Death";
                        employee_premium_life.Premium = original_life_premium.ToString();
                        employee_premium_life.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                        employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                        employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                        if (original_life_premium != 0)
                        {
                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_life.Sum_Insured = lblSumInsure.Text.Trim();

                            }
                            else
                            {
                                employee_premium_life.Sum_Insured = plan.Sum_Insured.ToString();
                            }
                        }
                        else
                        {
                            employee_premium_life.Sum_Insured = "0";
                        }

                        total_sum_insure += Convert.ToDouble(employee_premium_life.Sum_Insured.Trim());

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                            Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                            return;
                        }

                        //if has Accicental 100Plus
                        if (plan.Accidental_100Plus_Option_Value == 1)
                        {
                            //decimal original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            //calculate premium TPD
                            decimal original_accidental_100plus_premium = 0;

                            if (last_policy.Policy_Number == "GL00000001" || last_policy.Policy_Number == "GL00000002" || last_policy.Policy_Number == "GL00000003" || last_policy.Policy_Number == "GL00000004" || last_policy.Policy_Number == "GL00000005" || last_policy.Policy_Number == "GL00000006" || last_policy.Policy_Number == "GL00000007" || last_policy.Policy_Number == "GL00000008")
                            {
                                original_accidental_100plus_premium = Calculation.CalculateGTLIPremium(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else if (last_policy.Policy_Number == "GL00000009" || last_policy.Policy_Number == "GL00000010")
                            {
                                original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else if (last_policy.Policy_Number == "GL00000011" || last_policy.Policy_Number == "GL00000012" || last_policy.Policy_Number == "GL00000013")
                            {
                                original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate2(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "10", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else{
                            
                                //product_id: 13=tpd, 12=100plus, 11=life_premium
                                original_accidental_100plus_premium = Calculation.CalculateGTLINewPremiumRate3(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "12", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                            }

                            total_original_accidental_100plus_premium += original_accidental_100plus_premium;

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_accidental_100plus = new bl_gtli_employee_premium();
                            employee_premium_accidental_100plus.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                            employee_premium_accidental_100plus.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_accidental_100plus.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                            employee_premium_accidental_100plus.Premium = original_accidental_100plus_premium.ToString();
                            employee_premium_accidental_100plus.Premium_Type = "Accidental100Plus";

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_accidental_100plus.Sum_Insured = lblSumInsure.Text.Trim();
                            }
                            else
                            {
                                employee_premium_accidental_100plus.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_accidental_100plus, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                return;
                            }

                        }

                        //if has TPD
                        if (plan.TPD_Option_Value == 1)
                        {
                            //calculate premium TPD
                            decimal original_tpd_premium = 0;

                            if (last_policy.Policy_Number == "GL00000001" || last_policy.Policy_Number == "GL00000002" || last_policy.Policy_Number == "GL00000003" || last_policy.Policy_Number == "GL00000004" || last_policy.Policy_Number == "GL00000005" || last_policy.Policy_Number == "GL00000006" || last_policy.Policy_Number == "GL00000007" || last_policy.Policy_Number == "GL00000008")
                            {
                                original_tpd_premium = Calculation.CalculateGTLIPremium(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else if (last_policy.Policy_Number == "GL00000009" || last_policy.Policy_Number == "GL00000010")
                            {
                                original_tpd_premium = Calculation.CalculateGTLINewPremiumRate(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                            else if (last_policy.Policy_Number == "GL00000011" || last_policy.Policy_Number == "GL00000012" || last_policy.Policy_Number == "GL00000013")
                            {
                                original_tpd_premium = Calculation.CalculateGTLINewPremiumRate2(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);
                            }
                             else{
                            
                                //product_id: 13=tpd, 12=100plus, 11=life_premium
                                //New policy
                                 original_tpd_premium = Calculation.CalculateGTLINewPremiumRate3(dob, lblGender.Text.Trim(), Convert.ToDecimal(plan.Sum_Insured), "13", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);

                             }

                            total_original_tpd_premium += original_tpd_premium;

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                            employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                            employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_tpd.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                            employee_premium_tpd.Premium = original_tpd_premium.ToString();
                            employee_premium_tpd.Premium_Type = "TPD";

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_tpd.Sum_Insured = lblSumInsure.Text.Trim();
                            }
                            else
                            {
                                employee_premium_tpd.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                                Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                return;
                            }

                        }

                        int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, effective_date);

                        string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium dhc for age within 18 - 59
                        if (customer_age >= 18 && customer_age < 60)
                        {
                            decimal original_dhc_premium = plan.DHC_Option_Value;

                            total_original_dhc_premium += original_dhc_premium;

                            bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                            employee_premium_dhc.Premium_Type = "DHC";
                            employee_premium_dhc.Premium = original_dhc_premium.ToString();
                            employee_premium_dhc.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                            employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                            employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                            if (plan.Sum_Insured == 0)
                            {
                                employee_premium_dhc.Sum_Insured = lblSumInsure.Text.Trim();
                            }
                            else
                            {
                                employee_premium_dhc.Sum_Insured = plan.Sum_Insured.ToString();
                            }

                            if (original_dhc_premium > 0) //Insert only dhc premium > 0
                            {
                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                                    Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                                    return;
                                }
                            }

                        }
                    }//end check checked box employee

                }//end loop employee in grid view

                //Get discount for each premium type
                if (discount > 0)
                {

                    //calculate discount for accidental 100 plus
                    if (plan.Accidental_100Plus_Option_Value == 1)
                    {
                        total_accidental_100plus_premium_discount = Math.Floor((total_original_accidental_100plus_premium * discount) / 100);

                    }

                    //calculate discount for tpd
                    if (plan.TPD_Option_Value == 1)
                    {
                        total_tpd_premium_discount = Math.Floor((total_original_tpd_premium * discount) / 100);

                    }

                    //calculate discount for dhc
                    if (plan.DHC_Option_Value > 0)
                    {
                        total_dhc_premium_discount = Math.Floor((total_original_dhc_premium * discount) / 100);

                    }

                    total_life_premium_discount = Math.Floor((total_original_life_premium * discount) / 100);

                }

                //Get Tax Amount
                total_accidental_100plus_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_accidental_100plus_premium, total_accidental_100plus_premium_discount);
                total_life_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_life_premium, total_life_premium_discount);
                total_tpd_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_tpd_premium, total_tpd_premium_discount);
                total_dhc_premium_tax_amount = Calculation.Calculate_GTLI_Premium_Tax(total_original_dhc_premium, total_dhc_premium_discount);

                //Premium after discount
                total_accidental_100plus_premium = total_original_accidental_100plus_premium - total_accidental_100plus_premium_discount;
                total_life_premium = total_original_life_premium - total_life_premium_discount;
                total_tpd_premium = total_original_tpd_premium - total_tpd_premium_discount;
                total_dhc_premium = total_original_dhc_premium - total_dhc_premium_discount;

                //Insert renew prem pay

                //Get payment code

                string policy_id = da_gtli_policy.GetGTLIPolicyIDByPremiumID(gtli_renew_premium.GTLI_Premium_ID);

                string prem_pay_payment_code = da_gtli_policy_prem_pay.GetPaymentCodeByPolicyID(policy_id);
                string prem_return_payment_code = da_gtli_policy_prem_return.GetPaymentCodeByPolicyID(policy_id);

                if (prem_pay_payment_code == "" && prem_return_payment_code == "")
                {
                    payment_code = da_payment_code.GetNewPaymentCode();
                }
                else
                {
                    payment_code = prem_pay_payment_code;
                }
                //End Get payment code

                bl_gtli_policy_prem_pay renew_prem_pay = new bl_gtli_policy_prem_pay();
                renew_prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                renew_prem_pay.Prem_Year = 1;
                renew_prem_pay.Prem_Lot = 1;
                renew_prem_pay.Pay_Mode_ID = 1;
                renew_prem_pay.Pay_Date = effective_date;
                renew_prem_pay.Office_ID = channel_location_id;
                renew_prem_pay.GTLI_Premium_ID = gtli_renew_premium.GTLI_Premium_ID;
                renew_prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Return_ID", "@GTLI_Policy_Prem_Return_ID");
                renew_prem_pay.Due_Date = expiry_date.AddDays(1);
                renew_prem_pay.Created_On = DateTime.Now;
                renew_prem_pay.Created_Note = "";
                renew_prem_pay.Created_By = hdfusername.Value;
                renew_prem_pay.Amount = Convert.ToDouble(total_dhc_premium + total_life_premium + total_tpd_premium + total_accidental_100plus_premium);
                renew_prem_pay.Payment_Code = payment_code;

                if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(renew_prem_pay, hdfuserid.Value))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy failed. Please check your inputs again.')", true);
                    Clear(gtli_policy_temporary_id, gtli_renew_premium.GTLI_Premium_ID);
                    return;
                }

                //update total premium in premium temporary
                da_gtli_premium_temporary.UpdatePremium(total_sum_insure, Convert.ToDouble(total_life_premium), Convert.ToDouble(total_tpd_premium), Convert.ToDouble(total_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium), Convert.ToDouble(total_accidental_100plus_premium_discount), Convert.ToDouble(total_life_premium_discount), Convert.ToDouble(total_tpd_premium_discount), Convert.ToDouble(total_dhc_premium_discount), Convert.ToDouble(total_original_accidental_100plus_premium), Convert.ToDouble(total_original_life_premium), Convert.ToDouble(total_original_tpd_premium), Convert.ToDouble(total_original_dhc_premium), Convert.ToDouble(total_accidental_100plus_premium_tax_amount), Convert.ToDouble(total_life_premium_tax_amount), Convert.ToDouble(total_tpd_premium_tax_amount), Convert.ToDouble(total_dhc_premium_tax_amount), gtli_renew_premium.GTLI_Premium_ID);

                Clear("", "");

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert renew gtli policy successfull.')", true);
                return;


            }//End checked renew 
        }
        catch (Exception ex) {

            Log.AddExceptionToLog("Error function [reNewPolicy] in page [register_employee.aspx.cs]. Detail: " + ex.Message);
        }
        
    }//end void renew policy

    protected void loadEmployeeList(string companyName)
    {
        try
        {

            this.gvActiveMember.DataSource = null;
            this.gvActiveMember.DataBind();

            //get policy by company
            string companyID = "";
            companyID = da_gtli_company.GetCompanyIDByCompanyName(companyName.Trim());
            bl_gtli_policy last_policy = new bl_gtli_policy();
            last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(companyID);


            //list all employee status=1
            string policyID = "";
            policyID = last_policy.GTLI_Policy_ID;

            ArrayList list_of_active_employee = new ArrayList();
            list_of_active_employee = da_gtli_employee.GetListOfActiveEmployee(policyID);
            ArrayList listEmployeeWithPremium = new ArrayList();

            //Declaration
            double life_premium = 0;
            double tpd_premium = 0;
            double dhc_premium = 0;
            double accidental_100plus_premium = 0;

            //employee calculation premium
            for (int i = 0; i <= list_of_active_employee.Count - 1; i++)
            {

                bl_gtli_employee myemployee = (bl_gtli_employee)list_of_active_employee[i];
                life_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "Death");
                tpd_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "TPD");
                dhc_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "DHC");
                accidental_100plus_premium = da_gtli_employee_premium.GetPremiumByGTLICertificateID(myemployee.GTLI_Certificate_ID, myemployee.GTLI_Premium_ID, "Accidental100Plus");

                string str_certificate_no = myemployee.Certificate_Number.ToString();
                double sumInsure = 0;
                sumInsure = Convert.ToDouble(myemployee.Sum_Insured);
                //show only certificate >0
                if (str_certificate_no != "0" && sumInsure > 0)
                {

                    while (str_certificate_no.Length < 6)
                    {
                        str_certificate_no = "0" + str_certificate_no;
                    }

                    bl_gtli_premium premium = new bl_gtli_premium();
                    premium = da_gtli_premium.GetGTLIPremiumByID(myemployee.GTLI_Premium_ID);

                    TimeSpan mytimespan = premium.Expiry_Date.Subtract(premium.Effective_Date);
                    int coverage_period = mytimespan.Days + 1;

                    bl_gtli_plan my_plan = da_gtli_plan.GetPlan(premium.GTLI_Plan_ID);
                    bl_gtli_member_list active_employee = new bl_gtli_member_list();
                    active_employee.Certificate_Number = str_certificate_no;
                    active_employee.Days = coverage_period;
                    active_employee.DHC_Premium = dhc_premium;
                    active_employee.Effective_Date = premium.Effective_Date;
                    active_employee.Employee_Name = myemployee.Employee_Name;
                    active_employee.Gender = myemployee.Gender;
                    active_employee.DOB = myemployee.DOB;
                    active_employee.Position = myemployee.Position;
                    active_employee.EmployeeID = myemployee.Employee_ID;
                    active_employee.Expiry_Date = premium.Expiry_Date;
                    active_employee.GTLI_Plan = my_plan.GTLI_Plan;
                    active_employee.Life_Premium = life_premium;
                    active_employee.Accidental_100Plus_Premium = accidental_100plus_premium;
                    active_employee.Total_Premium = (life_premium + tpd_premium + dhc_premium + accidental_100plus_premium);
                    active_employee.TPD_Premium = tpd_premium;
                    active_employee.Sum_Insured = Convert.ToDouble(myemployee.Sum_Insured);
                    active_employee.GTLI_Certificate_ID = myemployee.GTLI_Certificate_ID;
                    active_employee.GTLI_Premium_ID = myemployee.GTLI_Premium_ID;

                    listEmployeeWithPremium.Add(active_employee);

                }

            }

            this.gvActiveMember.DataSource = listEmployeeWithPremium;
            this.gvActiveMember.DataBind();
        }
        catch (Exception ex) {

            Log.AddExceptionToLog("Error Function [loadEmployeeList] in page [register_employee.aspx.cs], Detail: " + ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!this.validate()) {

            this.loadEmployeeList(this.txtCompany.Text.Trim());
            ddlOption.SelectedIndex = 1;
            //hide uploadedDoc
            uploadedDoc.Style.Clear();
            uploadedDoc.Style.Add("display", "none");
            //show button search
            btnSearch.Style.Clear();
            btnSearch.Style.Add("display", "relative");
            gvActiveMember.Style.Clear();
            gvActiveMember.Style.Add("display", "relative");

            txtCompany.Attributes.Add("onchange", "return GetPlans()");
        }
    }

    protected bool validate() {
        bool error = false;
        //check company
        bool check_company = da_gtli_company.CheckCompany(txtCompany.Text.Trim());
        if (!check_company)
        {
            error = true;
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This company is not registered.')", true);
            //Clear("", "");
            return error;
        }

        //check effective date format
        //if(txtEffectiveDate.Text.Trim()!=""){

        //    if (!Helper.CheckDateFormat(txtEffectiveDate.Text.Trim()))
        //    {
        //        error = true;
        //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Invalid Effective Date Format.')", true);
        //       // Clear("", "");
        //        return error;
        //    }
        //}
            
        //else {

        //    error = true;
        //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Effective Date Is Required.')", true);
        //       // Clear("", "");
        //        return error;
        //}

        return error;
    }
    protected int numberStaffSelected() {
        int intStaff = 0;
        foreach (GridViewRow row in gvActiveMember.Rows) {

            CheckBox ckb;
            ckb = (CheckBox)row.Cells[0].FindControl("chk1");
            if (ckb.Checked) {

                intStaff += 1;
            }
        }
        return intStaff;
    }
    protected DateTime getEmployeeDOB(string companyName, string certificateNumber, string employeeName) {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime DOB;
        DOB =DateTime.Now;
        DOB=Convert.ToDateTime(DOB,dtfi);

        try{

              string query = "select distinct e.Customer_Status, c.Certificate_Number, e.gtli_certificate_id, e.Employee_Name, e.Gender, " +
            "e.DOB, com.Company_Name from ct_gtli_employee e "+
            "inner join Ct_GTLI_Certificate c on c.GTLI_Certificate_ID=e.GTLI_Certificate_ID "+
            "inner join Ct_GTLI_Company com on com.GTLI_Company_ID=c.GTLI_Company_ID "+
            "where e.Customer_Status='1' and com.Company_Name='" + companyName.Trim() + "'" +
            "and c.Certificate_Number=" + certificateNumber +
            " and e.Employee_Name='" + employeeName.Trim() + "'";

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(AppConfiguration.GetConnectionString());
            System.Data.SqlClient.SqlDataReader dr;
            System.Data.SqlClient.SqlCommand cmd;

            cmd = new System.Data.SqlClient.SqlCommand(query, con);
            con.Open();
            dr =  cmd.ExecuteReader();
            if (dr.Read()) {

                DOB = Convert.ToDateTime(dr["DOB"],dtfi);
            }

        }
        catch (Exception ex){
        
            Log.AddExceptionToLog("Error function [getEmployeeDOB] in page [register_employee.aspx.cs]. Detail: " + ex.Message);
        }
       
        return DOB;
    }
    protected void btnRenew_Click(object sender, EventArgs e)
    {

        validate();

        if(txtTotalNumberOfStaff.Text.Trim()==""){
        
             ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Input Number Of Staff.')", true);
                // Clear("", "");
                return;
        }

        if (ddlSaleAgent.SelectedIndex == 0) {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Select Sale Agent.')", true);
            // Clear("", "");
            return;
        }

        if (txtEffectiveDate.Text.Trim() != "")
        {

            if (!Helper.CheckDateFormat(txtEffectiveDate.Text.Trim()))
            {
               
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Invalid Effective Date Format.')", true);
                // Clear("", "");
                return;
            }
        }

        else
        {

           
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Effective Date Is Required.')", true);
            // Clear("", "");
            return ;
        }

        int staffNum = 0;
        staffNum = numberStaffSelected();
        //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + staffNum + "')", true);

        if (staffNum == 0)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please select record(s).')", true);
            return;
        }
        else if (ckbRenew.Checked == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Checke Renew Policy.')", true);
            return;
        }
        else if (ddlSaleAgent.SelectedIndex == 0)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Select Sale Agent.')", true);
            return;
        }

        else
        {

            if (ddlOption.SelectedIndex == 0)
            {
                //upload file
                ImageButton img;

            }
            else { 
                //gridview
                //renew policy
                this.reNewPolicy();
            
            }
            

        }

        
    }
    protected void imgBtnRenew_Click(object sender, ImageClickEventArgs e)
    {
        validate();

        if (txtTotalNumberOfStaff.Text.Trim() == "")
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Input Number Of Staff.')", true);
            // Clear("", "");
            return;
        }

        if (ddlSaleAgent.SelectedIndex == 0)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Select Sale Agent.')", true);
            // Clear("", "");
            return;
        }

        if (txtEffectiveDate.Text.Trim() != "")
        {

            if (!Helper.CheckDateFormat(txtEffectiveDate.Text.Trim()))
            {

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Invalid Effective Date Format.')", true);
                // Clear("", "");
                return;
            }
        }

        else
        {


            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Effective Date Is Required.')", true);
            // Clear("", "");
            return;
        }

       
        if (ckbRenew.Checked == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Checke Renew Policy.')", true);
            return;
        }
        else if (ddlSaleAgent.SelectedIndex == 0)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please Select Sale Agent.')", true);
            return;
        }
        else//Save data
        {
            //check exist renew policy
            int a = existPolicyByCompanyName(txtCompany.Text.Trim());
            if (a == 1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This Policy was already renew." + "')", true);
                return;
            }
            else if (a == 2)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "System got error, pleasce contact your system administrator." + "')", true);
                return;
            }
            else
            {

                if (ddlOption.SelectedIndex == 0)
                {
                    //upload file
                    //call Add new button
                    ImgBtnAdd_Click(sender, e);
                }
                else
                {
                    //gridview
                    //renew policy
                    int staffNum = 0;
                    staffNum = numberStaffSelected();
                    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + staffNum + "')", true);

                    if (staffNum == 0)
                    {

                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please select record(s).')", true);
                        return;
                    }
                    else {
                        reNewPolicy();
                    }
                
                }
            }
        }
    }
    //Fuction check existing policy
    protected int existPolicyByCompanyName(string companyname) {
        int exist = 0;
        string strSelect = "";
        DateTime effective_date;
        string company_id = "";
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        try
        {
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";
            effective_date = Convert.ToDateTime(txtEffectiveDate.Text.Trim(), dtfi);
            //get company ID by name
            company_id = da_gtli_company.GetCompanyIDByCompanyName(companyname);

            //get last policy of company
            bl_gtli_policy last_policy = new bl_gtli_policy();
            last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(company_id);

            System.Data.SqlClient.SqlDataReader dr;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(AppConfiguration.GetConnectionString());
            System.Data.SqlClient.SqlCommand cmd;

            strSelect = "select Gtli_policy_id, policy_number, effective_date from Ct_GTLI_Policy_Temporary where policy_number='" + last_policy.Policy_Number +
                "' and effective_date='" + effective_date + "'";
            conn.Open();
            cmd = new System.Data.SqlClient.SqlCommand(strSelect, conn);
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {

                exist = 1;
            }
            dr.Close();
            cmd.Dispose();
            conn.Close();
        }
        catch (Exception e) {
            exist = 2;//Error
            Log.AddExceptionToLog("Error fucntion [existPolicyByCompanyName] in page [register_employee.aspx.cs], Detail: " + e.Message);
        }
               
        return exist;
    }
}