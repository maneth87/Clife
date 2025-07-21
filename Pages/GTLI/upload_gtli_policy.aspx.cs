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

public partial class Pages_upload_gtli_policy : System.Web.UI.Page
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

    

    //Clear
    protected void Clear()
    {
        txtCompany.Text = "";
        txtEffectiveDate.Text = "";
        txtTotalNumberOfStaff.Text = "";
        ddlSaleAgent.SelectedIndex = 0;
    }


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
                    Clear();
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
                int policy_year = 2;

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
                    Clear();
                    return;
                }
                else
                {
                    effective_date = Convert.ToDateTime(txtEffectiveDate.Text, dtfi);

                }


                if (version == ".xlsx")
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

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input of employee name field then try again. Row number: '" + (i + 2) + ")";
                            Clear();
                            return;
                        }

                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for gender field then try again. Row number: '" + (i + 2) + ")";
                            Clear();
                            return;
                        }

                        if (!Helper.CheckDateFormatMDY(dt.Rows[i][3].ToString()))
                        {
                            lblMessage.Text = "Please check your input for date field then try again. Row number: '" + (i + 2) + ")";
                            Clear();
                            return;
                        }

                    }


                    bool add_policy = false;

                    //found company, get company ID by name
                    company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompany.Text.Trim());

                    //get contact_id
                    bl_gtli_contact contact = da_gtli_contact.GetContactByCompanyId(company_id);
                    contact_id = contact.GTLI_Contact_ID;

                    string gtli_policy_temporary_id = Helper.GetNewGuid("SP_Check_GTLI_Policy_Temporary_ID", "@GTLI_Policy_ID");

                    //Inforce
                    certificate_number = 0;
                    policy_year = 2;
                    policy_number = "GL00000003";

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

                        add_policy = da_gtli_policy_temporary.InsertPolicyTemporary(gtli_policy_temporary, hdfuserid.Value);

                        if (!add_policy)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear();
                            return;
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
                    gtli_premium.Policy_Year = 2;
                    gtli_premium.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                    gtli_premium.Sum_Insured = plan.Sum_Insured;
                    gtli_premium.TPD_Premium = 0;
                    gtli_premium.Transaction_Staff_Number = dt.Rows.Count;
                    gtli_premium.User_Total_Staff_Number = Convert.ToInt32(txtTotalNumberOfStaff.Text);

                    //365 days add then transaction type = 1                                
                    gtli_premium.Transaction_Type = 1;

                    if (!da_gtli_premium_temporary.InsertPremiumTemporary(gtli_premium, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear();
                        return;

                    }

                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {
                        DateTime dob = Convert.ToDateTime(dt.Rows[k][3], dtfi2);

                        //Insert Certificate
                        bl_gtli_certificate certificate = new bl_gtli_certificate();

                        certificate.GTLI_Company_ID = company_id;
                        certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_Temporary_ID", "@GTLI_Certificate_ID");

                        certificate_number = Convert.ToInt32(dt.Rows[k][8]);

                        certificate.Certificate_Number = certificate_number;


                        if (!da_gtli_certificate_temporary.InsertCertificateTemporary(certificate, hdfuserid.Value, gtli_premium.GTLI_Premium_ID))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear();
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
                            Clear();
                            return;
                        }


                        //calculate premium
                        decimal life_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "8", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);


                        total_life_premium += life_premium;

                        string gtli_employee_premium_temporary_id_life = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium life
                        bl_gtli_employee_premium employee_premium_life = new bl_gtli_employee_premium();
                        employee_premium_life.Premium_Type = "Death";
                        employee_premium_life.Premium = life_premium.ToString();
                        employee_premium_life.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                        employee_premium_life.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_life;
                        employee_premium_life.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                        if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_life, hdfuserid.Value))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                            Clear();
                            return;
                        }

                        //if has TPD
                        if (plan.TPD_Option_Value == 1)
                        {
                            decimal tpd_premium = Calculation.CalculateGTLIPremium(dob, dt.Rows[k][2].ToString(), Convert.ToDecimal(plan.Sum_Insured), "9", Convert.ToInt32(txtTotalNumberOfStaff.Text.Trim()), effective_date);


                            total_tpd_premium += tpd_premium;

                            string gtli_employee_premium_temporary_id_tpd = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                            //Insert employee premium tpd
                            bl_gtli_employee_premium employee_premium_tpd = new bl_gtli_employee_premium();
                            employee_premium_tpd.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;
                            employee_premium_tpd.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_tpd;
                            employee_premium_tpd.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_tpd.Premium = tpd_premium.ToString();
                            employee_premium_tpd.Premium_Type = "TPD";

                            if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_tpd, hdfuserid.Value))
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                Clear();
                                return;
                            }

                        }

                        int customer_age = Calculation.Culculate_Customer_Age_Micro(dob, effective_date);

                        string gtli_employee_premium_temporary_id_dhc = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_Temporary_ID", "@GTLI_Employee_Premium_ID");

                        //Insert employee premium dhc for age within 18 - 59
                        if (customer_age >= 18 && customer_age < 60)
                        {

                            decimal dhc_premium = plan.DHC_Option_Value;

                            total_dhc_premium += dhc_premium;

                            bl_gtli_employee_premium employee_premium_dhc = new bl_gtli_employee_premium();
                            employee_premium_dhc.Premium_Type = "DHC";
                            employee_premium_dhc.Premium = dhc_premium.ToString();
                            employee_premium_dhc.GTLI_Premium_ID = gtli_premium.GTLI_Premium_ID;
                            employee_premium_dhc.GTLI_Employee_Premium_ID = gtli_employee_premium_temporary_id_dhc;
                            employee_premium_dhc.GTLI_Certificate_ID = employee.GTLI_Certificate_ID;

                            if (dhc_premium > 0) //Insert only dhc premium > 0
                            {
                                if (!da_gtli_employee_premium_temporary.InsertEmployeePremiumTemporary(employee_premium_dhc, hdfuserid.Value))
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                                    Clear();
                                    return;
                                }
                            }

                        }

                    }//End loop dt

                    //Insert prem pay
                    bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
                    prem_pay.Sale_Agent_ID = ddlSaleAgent.SelectedValue;
                    prem_pay.Prem_Year = 2;
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
                    prem_pay.Amount = Convert.ToDouble(total_dhc_premium + total_life_premium + total_tpd_premium);


                    if (!da_gtli_policy_prem_pay_temporary.InsertPolicyPremPayTemporary(prem_pay, hdfuserid.Value))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy failed. Please check your inputs again.')", true);
                        Clear();
                        return;
                    }


                    ////update total premium in premium temporary
                    //da_gtli_premium_temporary.UpdatePremium(Convert.ToDouble(total_life_premium), Convert.ToDouble(total_tpd_premium), Convert.ToDouble(total_dhc_premium), gtli_premium.GTLI_Premium_ID);

                    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Insert new gtli policy successfull.')", true);
                    Response.Redirect("add_new_transaction_underwrite_detail.aspx?pid=" + gtli_premium.GTLI_Premium_ID, true);

                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please upload an excel file that contains employee data.')", true);
                Clear();
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
}