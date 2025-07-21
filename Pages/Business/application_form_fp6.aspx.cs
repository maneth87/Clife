using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.Security;
using System.Data.SqlClient;
using System.Collections;

public partial class Pages_Business_application_form_fp6 : System.Web.UI.Page
{

    //TODO: Create funtion showApplication for search modal
    private void showApplication(bool status) {
        //show/hide Application controls
        lblApplicationNumberSearch.Visible = status;
        txtApplicationNumberSearch1.Visible = status;

        if (status == false)
        {
            //reset
            txtApplicationNumberSearch1.Text = "";
        }
    }
    //TODO: Create funtion showCustomer for search modal
    private void showCustomer(bool status) {
        lblCustomerFirstNameSearch.Visible = status;
        lblCustomerLastNameSearch.Visible = status;
        txtCustomerFirstNameSearch.Visible = status;
        txtCustomerLastNameSearch.Visible = status;

        if (status == false) {
            //Reset
            txtCustomerFirstNameSearch.Text = "";
            txtCustomerLastNameSearch.Text = "";
        }
    }
    //TODO: Create funtion showIDType for search modal
    private void showIDType(bool status)
    {
        lblIDNoSearch.Visible = status;
        lblIDTypeSearch.Visible = status;
        ddlIDTypeSearch1.Visible = status;
        txtIDNoSearch.Visible = status;

        if (status == false)
        {
            //Reset
            txtIDNoSearch.Text = "";
           
        }
    }
   
    //sub add benifit in to data table
    private void generateBenifitTable()
    {
        //variable
        DataTable tblBenifit = new DataTable();
        var a = tblBenifit.Columns;
        a.Add("ID_Type");
        a.Add("ID_Card");
        a.Add("full_Name");
        a.Add("relationship");
        a.Add("Percentage");
        a.Add("app_benefit_item_id");
        a.Add("remarks");
        ViewState["tblBenifit"] = tblBenifit;
    }
    //sub add Job history in to data table
    private void generateJobHistoryTable()
    {
        //variable
        DataTable tblJob = new DataTable();
        var a = tblJob.Columns;
        a.Add("app_register_id");
        a.Add("job_id");
        a.Add("employer_name");
        a.Add("nature_of_business");
        a.Add("current_position");
        a.Add("job_role");
        a.Add("anual_income");
        a.Add("level");
        ViewState["tblJobHistory"] = tblJob;
    }
    //sub add Job history in to data table
    private void generateBodyTable()
    {
        //variable
        DataTable tblBody = new DataTable();
        var a = tblBody.Columns;
        a.Add("app_register_id");
        a.Add("weight");
        a.Add("height");
        a.Add("reason");
        a.Add("is_weight_changed");
        a.Add("level");
        a.Add("id");
        ViewState["tblBody"] = tblBody;
    }
    //generate rider data table
    private void geneateRiderTable()
    {
        DataTable tblRider = new DataTable();
        var a = tblRider.Columns;
        a.Add("rider_id");
        a.Add("app_register_id");
        a.Add("product_id");
        a.Add("level");
        a.Add("rider_type");
        a.Add("sumInsured");
        a.Add("premium");
        a.Add("discount");
        a.Add("Rate");
        a.Add("rate_id");
        a.Add("original_amount");
        a.Add("rounded_amount");
        a.Add("Age_Insure");
        a.Add("Pay_Year");
        a.Add("Pay_Up_To_Age");
        a.Add("Assure_Year");
        a.Add("Assure_Up_To_Age");
        a.Add("Effective_Date");
        ViewState["tblRider"] = tblRider;
    }

    //calculate premium
    private void CalculatePremium() {
       
        //int count = gvPersonalInfo.Rows.Count;
        int age=0;
        int gender;
        int paymentMode;
        double premium = 0.0;
        double totalPremium = 0.0;
        double originalPremium = 0.0;
        double totalOriginalPremium = 0.0;
        double totalPremiumAfterDiscount = 0.0;
        double sumInsured = 0.0;
        double discount = 0.0;
        string productId = "";
        string dob;
        DateTime entryDate;

        try
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            if (txtDateEntry.Text.Trim() == "")
            {
                lblMessageInsurancePlan.Text = "Date of entry is required.";
                return;
            }
            else
            {
                entryDate = Convert.ToDateTime(txtDateEntry.Text.Trim(), dtfi);
            }

            //incase change effective date
            if (txtEffectiveDate.Text.Trim() != "")
            {
                entryDate = Convert.ToDateTime(txtEffectiveDate.Text.Trim(), dtfi);
            }

            paymentMode = Convert.ToInt32(ddlPremiumMode.SelectedValue.Trim());

            if (txtInsuranceAmountRequired.Text.Trim() != "")
            {
                sumInsured = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim());
            }
            else {

                sumInsured = 0;
            }

            if (ddlTypeInsurancePlan.SelectedItem.Text.Trim() != ".") 
            {
                productId = ddlTypeInsurancePlan.SelectedValue.Trim();
            }

            txtAssureeAgeLife1.Text = "";
            txtAnnualOriginalPremiumAmountSystemLife1.Text ="";
            txtPremiumAmountSystemLife1.Text ="";
            txtPremiumOriginalAmountLife1.Text =  "";

            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];

            int count = tblPersonal.Rows.Count;

            for (int i = 0; i < count; i++)
            {

                dob = tblPersonal.Rows[i]["dob"].ToString().Trim();

                if (tblPersonal.Rows[i]["gender"].ToString().ToLower().Trim() == "male")
                {
                    gender = 1;
                }
                else
                {
                    gender = 0;
                }
               

                if (dob.Trim() != "" && gender+"".Trim() != "" && ddlPremiumMode.SelectedValue.Trim() != "" && sumInsured != 0)
                {
                    //initialize into text box
                    if (tblPersonal.Rows[i]["level"].ToString().Trim()=="1")
                    {//calculation only life insure

                        //get age
                        age = Calculation.Culculate_Customer_Age(dob, entryDate);

                        //get premium

                        int plan_assure_year = 0;

                        plan_assure_year = Convert.ToInt32(txtTermInsurance.Text.Trim());

                        //premium = da_application_fp6.getPremiumFP6(sumInsured, productId, gender, age, paymentMode, plan_assure_year);
                        //originalPremium = da_application_fp6.getAnnualPremiumFP6(sumInsured, productId, gender, age, plan_assure_year);
                        double[,] arr_premium = da_application_fp6.getPremiumFP6(sumInsured, productId, gender, age, paymentMode, plan_assure_year);
                        premium = arr_premium[0, 0];
                        originalPremium = arr_premium[0, 1];

                        txtAssureeAgeLife1.Text = age + "";
                        txtAnnualOriginalPremiumAmountSystemLife1.Text = originalPremium + "";
                        txtPremiumAmountSystemLife1.Text = premium + "";
                        txtPremiumOriginalAmountLife1.Text = originalPremium + "";

                        totalPremium += premium;
                        totalOriginalPremium += originalPremium;

                        //initialize into text box
                        txtTotalPremium.Text = totalPremium + "";
                        txtTotalOriginalPremium.Text = totalOriginalPremium + "";

                        //check discount
                        if (txtDiscountAmount.Text.Trim() != "")
                        {
                            discount = Convert.ToDouble(txtDiscountAmount.Text.Trim());
                        }
                        else
                        {
                            discount = 0;
                        }


                        totalPremiumAfterDiscount = totalPremium - discount;
                        txtTotalPremiumAfterDiscount.Text = totalPremiumAfterDiscount + "";

                        //plus rider
                        txtTotalPremiumAfterDiscount.Text = totalPremiumAfterDiscount + Convert.ToDouble(txtRiderTotalPremiumAfterDiscount.Text.Trim()) + "";
                    }
                }
            }
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function CalculatePremium in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        
        }
    }
    //calculate premium family protection package
    private void CalculatePremiumPackage()
    {

        //int count = gvPersonalInfo.Rows.Count;
        int age = 0;
        int gender;
        int paymentMode;
        double premium = 0.0;
        double totalPremium = 0.0;
        double originalPremium = 0.0;
        double totalOriginalPremium = 0.0;
        double totalPremiumAfterDiscount = 0.0;
        double sumInsured = 0.0;
        double discount = 0.0;
        string productId = "";
        string dob;
        DateTime entryDate;

        try
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            if (txtDateEntry.Text.Trim() == "")
            {
                lblMessageInsurancePlan.Text = "Date of entry is required.";
                return;
            }
            else
            {
                entryDate = Convert.ToDateTime(txtDateEntry.Text.Trim(), dtfi);
            }

            //incase change effective date
            if (txtEffectiveDate.Text.Trim() != "")
            {
                entryDate = Convert.ToDateTime(txtEffectiveDate.Text.Trim(), dtfi);
            }


            paymentMode = Convert.ToInt32(ddlPremiumMode.SelectedValue.Trim());

            if (txtInsuranceAmountRequired.Text.Trim() != "")
            {
                sumInsured = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim());
            }
            else
            {

                sumInsured = 0;
            }

            //check max sum insured
            if (sumInsured != 10000 && sumInsured!=5000)
            {
                lblMessageInsurancePlan.Text = "Sum insured is allow [5,000 $ - 10,000$] only.";
                txtRiderSumInsured.Text = "";
                txtAssureeAgeLife1.Text = "";
                txtPremiumAmount.Text = "";
                txtAnnualOriginalPremiumAmountSystemLife1.Text = "";
                txtTotalPremium.Text = "";
                txtDiscountAmount.Text = "";
                ddlRider.SelectedIndex = 0;
                ddlRiderPerson.SelectedIndex = 0;
                txtRiderPremium.Text = "";
                txtRiderTotalPreium.Text = "";
                txtRiderTotalPreium.Text = "";
                ​​DataTable tblRider​= new DataTable();
                tblRider= (DataTable) ViewState["tblRider"];

                //foreach (DataRow r in tblRider.Rows)
                //{
                //    r.Delete();
                //    tblRider.AcceptChanges();
                //}
                //int r = tblRider.Rows.Count;
                //for (int ind = 0; ind < r; ind++)
                //{
                //    tblRider.Rows[ind].Delete();
                //}
                tblRider.Rows.Clear();
                 ViewState["tblRider"] = tblRider;

                 gvRider.DataSource = tblRider;
                 gvRider.DataBind();

                return;
            }

            else
            {
                lblMessageInsurancePlan.Text = "";
            }

            if (ddlTypeInsurancePlan.SelectedItem.Text.Trim() != ".")
            {
                productId = ddlTypeInsurancePlan.SelectedValue.Trim();
            }


            txtAssureeAgeLife1.Text = "";
            txtAnnualOriginalPremiumAmountSystemLife1.Text = "";
            txtPremiumAmountSystemLife1.Text = "";
            txtPremiumOriginalAmountLife1.Text = "";

            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];

            int count = tblPersonal.Rows.Count;

            for (int i = 0; i < count; i++)
            {

                dob = tblPersonal.Rows[i]["dob"].ToString().Trim();

                if (tblPersonal.Rows[i]["gender"].ToString().ToLower().Trim() == "male")
                {
                    gender = 1;
                }
                else
                {
                    gender = 0;
                }


                if (dob.Trim() != "" && gender + "".Trim() != "" && ddlPremiumMode.SelectedValue.Trim() != "" && sumInsured != 0)
                {
                    //initialize into text box
                    if (tblPersonal.Rows[i]["level"].ToString().Trim() == "1")
                    {//calculation only life insure

                        //get age
                        age = Calculation.Culculate_Customer_Age(dob, entryDate);

                        //get premium
                        int plan_assure_year = 0;
                        plan_assure_year = Convert.ToInt32(txtTermInsurance.Text.Trim());

                        if (age >= 18 && age <= 60)
                        {
                            premium = da_application_fp6.GetPremiumFPPackage(productId, plan_assure_year, gender, age); // default in database all ages are zero
                           
                            if (sumInsured == 5000)
                            {
                                premium = Math.Ceiling( premium / 2);
                            }

                            originalPremium = premium;
                        }


                        txtAssureeAgeLife1.Text = age + "";
                        txtAnnualOriginalPremiumAmountSystemLife1.Text = originalPremium + "";
                        txtPremiumAmountSystemLife1.Text = premium + "";
                        txtPremiumOriginalAmountLife1.Text = originalPremium + "";

                        totalPremium += premium;
                        totalOriginalPremium += originalPremium;

                        //initialize into text box
                        txtTotalPremium.Text = totalPremium + "";
                        txtTotalOriginalPremium.Text = totalOriginalPremium + "";

                        //check discount
                        if (txtDiscountAmount.Text.Trim() != "")
                        {
                            discount = Convert.ToDouble(txtDiscountAmount.Text.Trim());
                        }
                        else
                        {
                            discount = 0;
                        }


                        totalPremiumAfterDiscount = totalPremium - discount;
                        txtTotalPremiumAfterDiscount.Text = totalPremiumAfterDiscount + "";

                        //plus rider
                        txtTotalPremiumAfterDiscount.Text = totalPremiumAfterDiscount + Convert.ToDouble(txtRiderTotalPremiumAfterDiscount.Text.Trim()) + "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function CalculatePremiumPackage in page application_form_fp6.aspx.cs, Detail: " + ex.Message);

        }
    }

  
   
    #region Calculation for Study Save Package
    /// <summary>
    /// Product study save package use study save rate (fix gender:male and age:39)
    /// This condition request by underwriting dept.
    /// </summary>
    /// <param name="product_id"></param>
    private void CalculatePremiumStudySave(string product_id)
    {
        //int count = gvPersonalInfo.Rows.Count;
        int age = 0;
        int gender;
        int paymentMode;
        double[,] premium = new double[,] { { 0, 0 } } ;
        double totalPremium = 0.0;
        double totalOriginalPremium = 0.0;
        double totalPremiumAfterDiscount = 0.0;
        double sumInsured = 0.0;
        double discount = 0.0;
       
        string dob;
        DateTime entryDate;

        try
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            if (txtDateEntry.Text.Trim() == "")
            {
                lblMessageInsurancePlan.Text = "Date of entry is required.";
                return;
            }
            else
            {
                entryDate = Convert.ToDateTime(txtDateEntry.Text.Trim(), dtfi);
            }

            //incase change effective date
            if (txtEffectiveDate.Text.Trim() != "")
            {
                entryDate = Convert.ToDateTime(txtEffectiveDate.Text.Trim(), dtfi);
            }

            paymentMode = Convert.ToInt32(ddlPremiumMode.SelectedValue.Trim());

            if (txtInsuranceAmountRequired.Text.Trim() != "")
            {
                sumInsured = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim());
            }
            else
            {

                sumInsured = 0;
            }

            //rider sum insured
            //txtRiderSumInsured.Text = sumInsured+"";
            txtInsuranceAmountRequired_TextChanged(null, null);

            txtAssureeAgeLife1.Text = "";
            txtAnnualOriginalPremiumAmountSystemLife1.Text = "";
            txtPremiumAmountSystemLife1.Text = "";
            txtPremiumOriginalAmountLife1.Text = "";

            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];

            int count = tblPersonal.Rows.Count;

            for (int i = 0; i < count; i++)
            {
                if (tblPersonal.Rows[i]["level"].ToString().Trim() == "1")
                {//calculation only life insure
                    dob = tblPersonal.Rows[i]["dob"].ToString().Trim();

                    if (tblPersonal.Rows[i]["gender"].ToString().ToLower().Trim() == "male")
                    {
                        gender = 1;
                    }
                    else
                    {
                        gender = 0;
                    }


                    if (dob.Trim() != "" && gender + "".Trim() != "" && ddlPremiumMode.SelectedValue.Trim() != "" && sumInsured != 0)
                    {
                        //initialize into text box


                        //get age
                        age = Calculation.Culculate_Customer_Age(dob, entryDate);

                        //get premium

                        int plan_assure_year = 0;

                        plan_assure_year = Convert.ToInt32(txtTermInsurance.Text.Trim());

                        //check product type
                        string sub_product = "";
                        string[] arr_product;
                        arr_product = product_id.Split('/');

                        if (arr_product.Length == 2)//study save normal
                        {

                            premium = da_application_study_save.study_save.GetPremium(age, gender, product_id, plan_assure_year, paymentMode, sumInsured);

                        }
                        else if (arr_product.Length > 2)//study save package and maturity
                        {
                           // sub_product = "SDS" + arr_product[1].ToString() + "/" + arr_product[1].ToString(); // convert to study save normal
                            //premium = da_application_study_save.GetPremium(39, 1, sub_product, plan_assure_year, paymentMode); ;//use fix gender (male) and age (39)
                            premium = da_application_study_save.study_save_package.GetPremium(age, gender, product_id, plan_assure_year, paymentMode);
                        }

                        txtAssureeAgeLife1.Text = age + "";
                        txtPremiumAmountSystemLife1.Text = premium[0, 0].ToString();
                        txtAnnualOriginalPremiumAmountSystemLife1.Text = premium[0, 1].ToString();
                        txtPremiumOriginalAmountLife1.Text = premium[0, 1].ToString();

                        totalPremium += premium[0, 0];
                        totalOriginalPremium += premium[0, 1];

                        //initialize into text box
                        txtTotalPremium.Text = totalPremium + "";
                        txtTotalOriginalPremium.Text = totalOriginalPremium + "";

                        //check discount
                        if (txtDiscountAmount.Text.Trim() != "")
                        {
                            discount = Convert.ToDouble(txtDiscountAmount.Text.Trim());
                        }
                        else
                        {
                            discount = 0;
                        }


                        totalPremiumAfterDiscount = totalPremium - discount;
                        txtTotalPremiumAfterDiscount.Text = totalPremiumAfterDiscount + "";

                        //plus rider
                        txtTotalPremiumAfterDiscount.Text = totalPremiumAfterDiscount + Convert.ToDouble(txtRiderTotalPremiumAfterDiscount.Text.Trim()) + "";

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function CalculatePremiumStudySavePackage in page application_form_fp6.aspx.cs, Detail: " + ex.Message);

        }
    }
    #endregion

    #region Database transection
    //save application
    private bool insertApplaction() {
        bool result = false;
        bl_app_register appRegister =new bl_app_register();
        try
        {
            string applicationID = Helper.GetNewGuid("SP_Check_App_Register_ID", "@App_Register_ID").ToString();
            //ViewState["applicationID"]=applicationID;//store in viewstate for using in other functions
            ViewState["appID"] = applicationID;//store in viewstate for using in other functions
            appRegister.App_Register_ID = applicationID;
            appRegister.App_Number = txtApplicationNumber.Text.Trim();
            appRegister.Original_App_Number = txtApplicationNumber.Text.Trim();
            appRegister.Channel_ID = ddlChannel.SelectedValue.Trim();
            appRegister.Channel_Item_ID = ddlCompany.SelectedValue.Trim();
            appRegister.Office_ID = "HQ";
            appRegister.Payment_Code = txtPaymentCode.Text.Trim();
            appRegister.Created_Note = txtNote.Text.Trim();
            appRegister.Created_By =txtDataEntryBy.Text.Trim();
            //appRegister.Created_On = DateTime.Now;
            appRegister.Created_On = Helper.FormatDateTime(txtDateSignature.Text.Trim());

            if (da_application_fp6.InsertAppRegister(appRegister))
            {

                result = true;
            }
        }
        catch (Exception ex) {
            result = false;
            Log.AddExceptionToLog("Error insertApplaction function in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
        }
       
        return result;
    }
    //update application
    private bool UpdateApplaction(string appID)
    {
        bool result = false;
        bl_app_register appRegister = new bl_app_register();
        try
        {

            appRegister.App_Register_ID = appID;
            appRegister.App_Number = txtApplicationNumber.Text.Trim();
            appRegister.Created_Note = txtNote.Text.Trim();

            if (da_application_fp6.UpdateAppRegister(appRegister))
            {

                result = true;
            }
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error updateApplaction function in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
        }

        return result;
    }
    //insert app_info
    private bool InsertAppInfo(string app_id) {
        try
        {
            bl_app_info info = new bl_app_info();
            //info.App_Date = Helper.FormatDateTime(txtDateSignature.Text.Trim()); 
             info.App_Date = Helper.FormatDateTime(txtDateEntry.Text.Trim()); 
            info.App_Register_ID = app_id;
            info.Benefit_Note = txtBenefitNote.Text.Trim();
            info.Created_By = txtDataEntryBy.Text.Trim();
            info.Created_Note = txtNote.Text.Trim();
            info.Created_On = DateTime.Now;
            info.Office_ID = "HQ";
            info.Sale_Agent_ID = txtMarketingCode.Text.Trim();
            if (da_application_fp6.InsertAppInfo(info))
            {
                return true;
            }
            else
            {

                Rollback(app_id);
                return false;
            }
        }
        catch (Exception ex) {
            Rollback(app_id);
            Log.AddExceptionToLog("Error function InsertAppInfo in page application_form_fp6.aspx.cs" + ex.Message);
            return false;
           
        }
       
    
    }
    //Insert Ct_App_Prem_Pay
    private bool InsertAppPremPay(string app_id)
    {
        bool status=false;
        try
        {
            string new_guid = Helper.GetNewGuid("SP_Check_App_Prem_Pay_ID", "@App_Prem_Pay_ID").ToString();
            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int rowCount = tblPersonal.Rows.Count;
            DateTime created_on = DateTime.Now;
            da_application_fp6.bl_app_prem_pay_fp6 app_prem_pay = new da_application_fp6.bl_app_prem_pay_fp6();

            app_prem_pay.App_Prem_Pay_ID = new_guid;
            app_prem_pay.App_Register_ID = app_id;

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            DateTime pay_date = Convert.ToDateTime(txtDateSignature.Text, dtfi);

            app_prem_pay.Pay_Date = pay_date;
            app_prem_pay.Is_Init_Payment = 1;//first paid
            app_prem_pay.Created_On = created_on;
            app_prem_pay.Created_By = ViewState["userName"] + "";
            app_prem_pay.Created_Note = txtNote.Text.Trim();

            for (int i = 0; i < rowCount; i++)
            {
                var rowIndex = tblPersonal.Rows[i];

                //string id = rowIndex["id"].ToString();
                //string le = rowIndex["level"].ToString();

                if (rowIndex["id"].ToString().Trim() == "")
                {
                    #region Life insure1
                    
                    if(rowIndex["level"].ToString().Trim()=="1")
                    {
                       
                        if (txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim() == "")
                        {
                            app_prem_pay.Original_Amount = 0.00;
                            app_prem_pay.Rounded_Amount = 0.00;
                        }
                        else 
                        {
                            app_prem_pay.Original_Amount = Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim());
                            app_prem_pay.Rounded_Amount = Math.Ceiling(Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim()));
                        }

                        if(txtPremiumAmountSystemLife1.Text.Trim()=="")
                        {
                    
                            app_prem_pay.Amount = 0.0;
                        }
                        else
                        {
                            app_prem_pay.Amount = Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim());
                        }

                        app_prem_pay.Level = Convert.ToInt32(rowIndex["level"].ToString());

                        //Insertion
                        if (da_application_fp6.InsertAppPremPayFP6(app_prem_pay))
                        {

                            return true;

                        }
                        else
                        {
                            //Rollback
                            Rollback(app_id);
                            return false;
                        }
                    }
#endregion
                   
                }
            }
            status=true;
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppPremPay] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            status=false;
        }
        return status;
    }
    //save person info
    private bool insertPersonalInfo(string app_id)
    {
        bool result = false;
        try
        {
            da_application_fp6.bl_app_info_person_fp6 app_info_person = new da_application_fp6.bl_app_info_person_fp6();

            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int rowCount = tblPersonal.Rows.Count;
           
            for (int i = 0; i < rowCount; i++)
            {
                DataRow rowIndex = tblPersonal.Rows[i];
                int gender = -1;
                if (rowIndex["gender"].ToString().Trim() == "Male")
                {
                    gender = 1;
                }
                else
                {
                    gender = 0;
                }
                if (rowIndex["level"].ToString().Trim() == "1")
                {
                    app_info_person.App_Register_ID = app_id;
                    app_info_person.Birth_Date = Helper.FormatDateTime(rowIndex["dob"].ToString().Trim());

                    app_info_person.Country_ID = rowIndex["nationality"].ToString().Trim();
                    app_info_person.Gender = gender;// Convert.ToInt16(rowIndex["gender"].ToString().Trim());
                    app_info_person.ID_Card = rowIndex["idNumber"].ToString().Trim();
                    app_info_person.ID_Type = Convert.ToInt32( rowIndex["idType"].ToString().Trim());
                    app_info_person.First_Name = rowIndex["firstEnName"].ToString().Trim();
                    app_info_person.Last_Name = rowIndex["surEnName"].ToString().Trim();

                    //Check for null
                    if (rowIndex["fatherFirstName"].ToString().Trim() == "")
                    {
                        app_info_person.Father_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Father_First_Name = rowIndex["fatherFirstName"].ToString().Trim();
                    }

                    if (rowIndex["fatherSurName"].ToString().Trim() == "")
                    {
                        app_info_person.Father_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Father_Last_Name = rowIndex["fatherSurName"].ToString().Trim();
                    }

                    if (rowIndex["firstKhName"].ToString().Trim()== "")
                    {
                        app_info_person.Khmer_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Khmer_First_Name = rowIndex["firstKhName"].ToString().Trim();
                    }

                    if (rowIndex["surKhName"].ToString().Trim() == "")
                    {
                        app_info_person.Khmer_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Khmer_Last_Name = rowIndex["surKhName"].ToString().Trim();

                    }

                    if (rowIndex["motherFirstName"].ToString().Trim() == "")
                    {
                        app_info_person.Mother_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Mother_First_Name = rowIndex["motherFirstName"].ToString().Trim();
                    }

                    if (rowIndex["motherSurName"].ToString().Trim()== "")
                    {
                        app_info_person.Mother_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Mother_Last_Name = rowIndex["motherSurName"].ToString().Trim();
                    }

                    if (rowIndex["previousFirstName"].ToString().Trim() == "")
                    {
                        app_info_person.Prior_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Prior_First_Name = rowIndex["previousFirstName"].ToString().Trim();
                    }

                    if (rowIndex["previousSurName"].ToString().Trim() == "")
                    {
                        app_info_person.Prior_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Prior_Last_Name = rowIndex["previousSurName"].ToString().Trim();
                    }
                    app_info_person.Marital_Status = rowIndex["Marital_Status"].ToString().Trim();
                    app_info_person.Relationship = rowIndex["Relationship"].ToString().Trim();
                    

                    //for policy owner
                    //Insertion
                    if (da_application_fp6.InsertAppInfoPerson(app_info_person))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            #region My Old Code
           /*
            int row = gvPersonalInfo.Rows.Count;


            GridViewRow r = gvPersonalInfo.Rows[0];

            Label lblDob = (Label)r.FindControl("lblDob");
            Label lblCountryID = (Label)r.FindControl("lblCountryId");
            Label lblGender = (Label)r.FindControl("lblGender");
            Label lblIdType = (Label)r.FindControl("lblIdType");
            Label lblIdNumber = (Label)r.FindControl("lblIdNumber");
            Label lblFirstName = (Label)r.FindControl("lblSurNameEn");
            Label lblLastName = (Label)r.FindControl("lblFirstNameEn");
            Label lblFirstNameKh = (Label)r.FindControl("lblSurNameKh");
            Label lblLastNameKh = (Label)r.FindControl("lblFirstNameKh");
            Label lblFatherSurName = (Label)r.FindControl("lblFatherSurName");
            Label lblFatherFirstName = (Label)r.FindControl("lblFatherFirstName");
            Label lblMotherSurName = (Label)r.FindControl("lblMotherSurName");
            Label lblMotherFirstName = (Label)r.FindControl("lblMotherFirstName");
            Label lblPreviousSurName = (Label)r.FindControl("lblPreviousSurName");
            Label lblPreviousFirstName = (Label)r.FindControl("lblPreviousFirstName");
            Label lblNationality = (Label)r.FindControl("lblNationality");

            app_info_person.App_Register_ID = app_id;
            app_info_person.Birth_Date = Helper.FormatDateTime(lblDob.Text.Trim());

            app_info_person.Country_ID = lblNationality.Text.Trim();
            app_info_person.Gender = Convert.ToInt16(lblGender.Text.Trim());
            app_info_person.ID_Card = lblIdNumber.Text.Trim();
            app_info_person.ID_Type = Convert.ToInt32(lblIdType.Text.Trim());
            app_info_person.First_Name = lblFirstName.Text.Trim();
            app_info_person.Last_Name = lblLastName.Text.Trim();

            //Check for null
            if (lblFatherFirstName.Text.Trim() == "")
            {
                app_info_person.Father_First_Name = "";
            }
            else
            {
                app_info_person.Father_First_Name = lblFatherFirstName.Text.Trim();
            }

            if (lblFatherSurName.Text.Trim() == "")
            {
                app_info_person.Father_Last_Name = "";
            }
            else
            {
                app_info_person.Father_Last_Name = lblFatherSurName.Text.Trim();
            }

            if (lblFirstNameKh.Text.Trim() == "")
            {
                app_info_person.Khmer_First_Name = "";
            }
            else
            {
                app_info_person.Khmer_First_Name = lblFirstNameKh.Text.Trim();
            }

            if (lblLastNameKh.Text.Trim() == "")
            {
                app_info_person.Khmer_Last_Name = "";
            }
            else
            {
                app_info_person.Khmer_Last_Name = lblLastNameKh.Text.Trim();

            }

            if (lblMotherFirstName.Text.Trim() == "")
            {
                app_info_person.Mother_First_Name = "";
            }
            else
            {
                app_info_person.Mother_First_Name = lblMotherFirstName.Text.Trim();
            }

            if (lblMotherSurName.Text.Trim() == "")
            {
                app_info_person.Mother_Last_Name = "";
            }
            else
            {
                app_info_person.Mother_Last_Name = lblMotherSurName.Text.Trim();
            }

            if (lblPreviousFirstName.Text.Trim() == "")
            {
                app_info_person.Prior_First_Name = "";
            }
            else
            {
                app_info_person.Prior_First_Name = lblPreviousFirstName.Text.Trim();
            }

            if (lblPreviousSurName.Text.Trim() == "")
            {
                app_info_person.Prior_Last_Name = "";
            }
            else
            {
                app_info_person.Prior_Last_Name = lblPreviousSurName.Text.Trim();
            }

            //for policy owner
            //Insertion
            if (da_application.InsertAppInfoPerson(app_info_person))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            */
            #endregion End My Old Code
            

        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error insertPersonalInfo function in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            result = false;
        }
      return  result;
       
    }
    //save person info
    private bool insertPersonalInfoSub(string app_id)
    {
        bool result = false;
        try
        {
            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int rowCount = tblPersonal.Rows.Count;

            for (int i = 0; i < rowCount; i++)
            {
                
                var row = tblPersonal.Rows[i];
                int gender = -1;
               if(row["gender"].ToString()=="Male")
                {
                    gender=1;
                }
                else if(row["gender"].ToString()=="Female")
                {
                    gender=0;
                }

                if (row["level"].ToString().Trim() == "0")//store only policy owner information
                {
                    da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                    person.App_Register_ID = app_id;
                    person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                    person.Country_ID = row["nationality"].ToString();
                    person.Father_First_Name = row["fatherFirstName"].ToString();
                    person.Father_Last_Name = row["fatherSurName"].ToString();
                    person.First_Name = row["firstEnName"].ToString();
                    person.Last_Name = row["surEnName"].ToString();
                    person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                    person.Khmer_Last_Name = row["surKhName"].ToString();
                    person.Gender = gender;// Convert.ToInt32(row["gender"].ToString());
                    person.ID_Card = row["idNumber"].ToString();
                    person.ID_Type = Convert.ToInt32(row["idtype"].ToString());
                    person.Level = Convert.ToInt32(row["level"].ToString().Trim());
                    person.Mother_First_Name = row["motherFirstName"].ToString();
                    person.Mother_Last_Name = row["motherSurName"].ToString();
                    string id = Helper.GetNewGuid("SP_Check_App_Info_Person_Sub_ID", "Person_ID");
                    person.Person_ID = id;// row["id"].ToString().Trim();
                    person.Prior_First_Name = row["previousFirstName"].ToString();
                    person.Prior_Last_Name = row["previousSurName"].ToString();
                    person.Marital_Status = row["Marital_Status"].ToString();
                    person.Relationship = row["Relationship"].ToString();

                    if (da_application_fp6.InsertAppInfoPersonSub(person))
                    {
                        result = true;
                    }
                    else
                    {
                        Rollback(app_id);
                        result = false;
                    }
                }
                
            }
            #region Old Code
            /*
            int row = gvPersonalInfo.Rows.Count;
            string person_id = "";

            for (int i = 1; i < row; i++)
            {//loop record in gridview personal info

                GridViewRow r = gvPersonalInfo.Rows[i];

                Label lblDob = (Label)r.FindControl("lblDob");
                Label lblCountryID = (Label)r.FindControl("lblCountryId");
                Label lblGender = (Label)r.FindControl("lblGender");
                Label lblIdType = (Label)r.FindControl("lblIdType");
                Label lblIdNumber = (Label)r.FindControl("lblIdNumber");
                Label lblFirstName = (Label)r.FindControl("lblSurNameEn");
                Label lblLastName = (Label)r.FindControl("lblFirstNameEn");
                Label lblFirstNameKh = (Label)r.FindControl("lblSurNameKh");
                Label lblLastNameKh = (Label)r.FindControl("lblFirstNameKh");
                Label lblFatherSurName = (Label)r.FindControl("lblFatherSurName");
                Label lblFatherFirstName = (Label)r.FindControl("lblFatherFirstName");
                Label lblMotherSurName = (Label)r.FindControl("lblMotherSurName");
                Label lblMotherFirstName = (Label)r.FindControl("lblMotherFirstName");
                Label lblPreviousSurName = (Label)r.FindControl("lblPreviousSurName");
                Label lblPreviousFirstName = (Label)r.FindControl("lblPreviousFirstName");
                Label lblNationality = (Label)r.FindControl("lblNationality");

                person_id = Helper.GetNewGuid("SP_Check_App_Info_Person_Sub_ID", "@Person_ID");

                app_info_person.Person_ID = person_id;
                app_info_person.App_Register_ID = app_id;
                app_info_person.Birth_Date = Helper.FormatDateTime(lblDob.Text.Trim());

                app_info_person.Country_ID = lblNationality.Text.Trim();
                app_info_person.Gender = Convert.ToInt16(lblGender.Text.Trim());
                app_info_person.ID_Card = lblIdNumber.Text.Trim();
                app_info_person.ID_Type = Convert.ToInt32(lblIdType.Text.Trim());
                app_info_person.First_Name = lblFirstName.Text.Trim();
                app_info_person.Last_Name = lblLastName.Text.Trim();

                app_info_person.Level = i;

                //Check for null
                if (lblFatherFirstName.Text.Trim() == "")
                {
                    app_info_person.Father_First_Name = "";
                }
                else
                {
                    app_info_person.Father_First_Name = lblFatherFirstName.Text.Trim();
                }

                if (lblFatherSurName.Text.Trim() == "")
                {
                    app_info_person.Father_Last_Name = "";
                }
                else
                {
                    app_info_person.Father_Last_Name = lblFatherSurName.Text.Trim();
                }

                if (lblFirstNameKh.Text.Trim() == "")
                {
                    app_info_person.Khmer_First_Name = "";
                }
                else
                {
                    app_info_person.Khmer_First_Name = lblFirstNameKh.Text.Trim();
                }

                if (lblLastNameKh.Text.Trim() == "")
                {
                    app_info_person.Khmer_Last_Name = "";
                }
                else
                {
                    app_info_person.Khmer_Last_Name = lblLastNameKh.Text.Trim();

                }

                if (lblMotherFirstName.Text.Trim() == "")
                {
                    app_info_person.Mother_First_Name = "";
                }
                else
                {
                    app_info_person.Mother_First_Name = lblMotherFirstName.Text.Trim();
                }

                if (lblMotherSurName.Text.Trim() == "")
                {
                    app_info_person.Mother_Last_Name = "";
                }
                else
                {
                    app_info_person.Mother_Last_Name = lblMotherSurName.Text.Trim();
                }

                if (lblPreviousFirstName.Text.Trim() == "")
                {
                    app_info_person.Prior_First_Name = "";
                }
                else
                {
                    app_info_person.Prior_First_Name = lblPreviousFirstName.Text.Trim();
                }

                if (lblPreviousSurName.Text.Trim() == "")
                {
                    app_info_person.Prior_Last_Name = "";
                }
                else
                {
                    app_info_person.Prior_Last_Name = lblPreviousSurName.Text.Trim();
                }

                //for life insure
                if (da_application_fp6.InsertAppInfoPersonSub(app_info_person))
                {
                    result = true;
                }
                else
                {
                    Rollback(app_id);
                    result = false;
                }
            }
             */
            #endregion End Old Code

        }
        catch (Exception ex)
        {
            Rollback(app_id);
            Log.AddExceptionToLog("Error insert insertPersonalInfoSub function in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            result = false;
        }
        return result;

    }
    //save contact
    private bool insertContact(string app_id)
    {
        try
        {
            bl_app_info_contact app_info_contact = new bl_app_info_contact();

            app_info_contact.App_Register_ID = app_id;

            app_info_contact.Fax1 = "";
            app_info_contact.Fax2 = "";
            app_info_contact.Home_Phone2 = "";
            app_info_contact.Mobile_Phone2 = "";
            app_info_contact.Office_Phone1 = "";
            app_info_contact.Office_Phone2 = "";

            //check for null
            if (txtEmailLife2.Text.Trim() == "")
            {
                app_info_contact.EMail = "";
            }
            else
            {
                app_info_contact.EMail = txtEmailLife2.Text.Trim();
            }

            if (txtTelephoneLife2.Text.Trim() == "")
            {
                app_info_contact.Home_Phone1 = "";
            }
            else
            {
                app_info_contact.Home_Phone1 = txtTelephoneLife2.Text.Trim();
            }

            if (txtMobilePhoneLife2.Text.Trim() == "")
            {
                app_info_contact.Mobile_Phone1 = "";
            }
            else
            {
                app_info_contact.Mobile_Phone1 = txtMobilePhoneLife2.Text.Trim();

            }

            //Insertion
            if (da_application.InsertAppInfoContact(app_info_contact))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_info_contact.App_Register_ID);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoContact] in class [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            return false;

        }
    }
    //save contact sub
    private bool insertContactSub(string app_id)
    {
        try
        {
            bl_app_info_contact app_info_contact = new bl_app_info_contact();

            app_info_contact.App_Register_ID = app_id;

            app_info_contact.Fax1 = "";
            app_info_contact.Fax2 = "";
            app_info_contact.Home_Phone2 = "";
            app_info_contact.Mobile_Phone2 = "";
            app_info_contact.Office_Phone1 = "";
            app_info_contact.Office_Phone2 = "";

            //check for null
            if (txtEmailLife1.Text.Trim() == "")
            {
                app_info_contact.EMail = "";
            }
            else
            {
                app_info_contact.EMail = txtEmailLife1.Text.Trim();
            }

            if (txtTelephoneLife1.Text.Trim() == "")
            {
                app_info_contact.Home_Phone1 = "";
            }
            else
            {
                app_info_contact.Home_Phone1 = txtTelephoneLife1.Text.Trim();
            }

            if (txtMobilePhoneLife1.Text.Trim() == "")
            {
                app_info_contact.Mobile_Phone1 = "";
            }
            else
            {
                app_info_contact.Mobile_Phone1 = txtMobilePhoneLife1.Text.Trim();

            }

            //Insertion
          
            if (da_application_fp6.InsertAppInfoContactSub(app_info_contact))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_info_contact.App_Register_ID);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoContactSub] in class [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            return false;

        }
    }
    //save Address
    private bool InsertAppInfoAddress(string app_id)
    {
        try
        {
            bl_app_info_address app_info_address = new bl_app_info_address();

            app_info_address.App_Register_ID = app_id;

            app_info_address.Country_ID = ddlCountryLife2.SelectedValue;
            app_info_address.Address1 = txtAddress1Life2.Text.Trim();
            app_info_address.Address2 = txtAddress2Life2.Text.Trim();
            app_info_address.Address3 = "";

            app_info_address.Province = txtCityLife2.Text.Trim();

            //check for null
            if (txtZipCodeLife2.Text.Trim() == "")
            {
                app_info_address.Zip_Code = "";
            }
            else
            {
                app_info_address.Zip_Code = txtZipCodeLife2.Text.Trim();
            }

            //Insertion
            if (da_application.InsertAppInfoAddress(app_info_address))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_info_address.App_Register_ID);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoAddress] in class [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            return false;
        }

    }
    //save Address sub , store address of policy owner in table sub
    private bool InsertAppInfoAddressSub(string app_id)
    {
        try
        {
            bl_app_info_address app_info_address = new bl_app_info_address();

            app_info_address.App_Register_ID = app_id;

            app_info_address.Country_ID = ddlCountryLife1.SelectedValue;
            app_info_address.Address1 = txtAddress1Life1.Text.Trim();
            app_info_address.Address2 = txtAddress2Life1.Text.Trim();
            app_info_address.Address3 = "";

            app_info_address.Province = txtCityLife1.Text.Trim();

            //check for null
            if (txtZipCodeLife1.Text.Trim() == "")
            {
                app_info_address.Zip_Code = "";
            }
            else
            {
                app_info_address.Zip_Code = txtZipCodeLife1.Text.Trim();
            }

            //Insertion
            if (da_application_fp6.InsertAppInfoAddressSub(app_info_address))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_info_address.App_Register_ID);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoAddressSub] in class [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            return false;
        }

    }
    //save job history
    /*
    private bool InsertAppJobHistory(string app_id)
    {
        try
        {
            bl_app_job_history app_job_history = new bl_app_job_history();

            app_job_history.App_Register_ID = app_id;

            if (txtAnualIncomeLife1.Text.Trim() == "")
            {
                app_job_history.Anual_Income = 0;
            }
            else
            {
                app_job_history.Anual_Income = Convert.ToDouble(txtAnualIncomeLife1.Text.Trim());
            }

            //Check for null
            if (txtCurrentPositionLife1.Text.Trim() == "")
            {
                app_job_history.Current_Position = "";
            }
            else
            {
                app_job_history.Current_Position = txtCurrentPositionLife1.Text.Trim();
            }

            if (txtNameEmployerLife1.Text.Trim() == "")
            {
                app_job_history.Employer_Name = "";
            }
            else
            {
                app_job_history.Employer_Name = txtNameEmployerLife1.Text.Trim();
            }

            if (txtRoleAndResponsibilityLife1.Text.Trim() == "")
            {
                app_job_history.Job_Role = "";
            }
            else
            {
                app_job_history.Job_Role = txtRoleAndResponsibilityLife1.Text.Trim();
            }

            if (txtNatureBusinessLife1.Text.Trim() == "")
            {
                app_job_history.Nature_Of_Business = "";
            }
            else
            {
                app_job_history.Nature_Of_Business = txtNatureBusinessLife1.Text.Trim();
            }

            //Insertion
            if (da_application.InsertAppJobHistory(app_job_history))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_job_history.App_Register_ID);
                return false;
            }

           


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppJobHistory] in class [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            return false;

        }

    }*/
    //save job history list
    private bool InsertAppJobHistorySubList(string app_id)
    {
        bool status = false;
        try
        {

            DataTable tblJob = (DataTable)ViewState["tblJobHistory"];
            foreach (DataRow row in tblJob.Rows)
            {
                if (row["Level"].ToString().Trim() == "1")
                { //Life insure 1
                    bl_app_job_history app_job_history = new bl_app_job_history();

                    app_job_history.App_Register_ID = app_id;
                    app_job_history.Anual_Income = Convert.ToDouble(row["anual_income"].ToString().Trim());
                    app_job_history.Current_Position = row["current_position"].ToString().Trim();
                    app_job_history.Employer_Name = row["employer_name"].ToString().Trim();
                    app_job_history.Job_Role = row["job_role"].ToString().Trim();
                    app_job_history.Nature_Of_Business = row["nature_of_business"].ToString().Trim();
                    
                    //Insertion
                    if (da_application.InsertAppJobHistory(app_job_history))
                    {
                        status= true;
                    }
                    else
                    {
                        //Rollback
                        Rollback(app_job_history.App_Register_ID);
                        status = false;
                    }
                }
                else
                {//Policy owner

                    da_application_fp6.bl_app_job_history_sub app_job_history = new da_application_fp6.bl_app_job_history_sub();

                    app_job_history.App_Register_ID = app_id;
                    app_job_history.Anual_Income = Convert.ToDouble(row["anual_income"].ToString().Trim());
                    app_job_history.Current_Position = row["current_position"].ToString().Trim();
                    app_job_history.Employer_Name = row["employer_name"].ToString().Trim();
                    app_job_history.Job_Role = row["job_role"].ToString().Trim();
                    app_job_history.Nature_Of_Business = row["nature_of_business"].ToString().Trim();
                    app_job_history.Level = Convert.ToInt32(row["Level"].ToString().Trim());
                    //app_job_history.Job_ID = row["job_id"].ToString().Trim();
                    app_job_history.Job_ID = Helper.GetNewGuid("SP_Check_App_Info_Job_Sub_ID", "@Job_ID");
                    app_job_history.Address = "";

                    //save into database
                    if (da_application_fp6.InsertAppJobHistorySub(app_job_history))
                    {
                        status = true;
                    }
                    else
                    {
                        //Rollback
                        Rollback(app_job_history.App_Register_ID);
                        status = false;
                    }
                }
            }

           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppJobHistorySub] in class [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            status = false;

        }
        return status;
    }
    //Save loan
    private bool InsertAppLoan(string app_id)
    {
        try
        {
            bl_app_loan app_loan = new bl_app_loan();

            app_loan.App_Register_ID = app_id;

            if (txtInterest.Text.Trim() != "" && txtTermLoan.Text.Trim() != "" && txtLoanEffectiveDate.Text.Trim() != "" && txtOutstandingLoanAmount.Text.Trim() != "")
            {

                if (txtInterest.Text.Trim() != "")
                {
                    app_loan.Interest_Rate = Convert.ToDouble(txtInterest.Text.Trim());
                }
                else
                {
                    app_loan.Interest_Rate = 0;
                }

                DateTime loan_effective_date = Helper.FormatDateTime(txtLoanEffectiveDate.Text);

                app_loan.Loan_Effiective_Date = loan_effective_date;
                app_loan.Loan_Type = Convert.ToInt32(ddlLoanType.SelectedValue);

                if (txtOutstandingLoanAmount.Text != "")
                {
                    app_loan.Out_Std_Loan = Convert.ToDouble(txtOutstandingLoanAmount.Text);
                }
                else
                {
                    app_loan.Out_Std_Loan = 0;
                }

                if (txtTermLoan.Text != "")
                {
                    app_loan.Term_Year = Convert.ToInt32(txtTermLoan.Text);
                }
                else
                {
                    app_loan.Term_Year = 0;
                }

                if (da_application.InsertAppLoan(app_loan))
                {
                    return true;
                }
                else
                {
                    //Rollback
                    Rollback(app_loan.App_Register_ID);
                    return false;
                }

            }
            else { //just write log 

               // Log.AddExceptionToLog("No loan save for application ID=" + app_id);
                return true;
            
            }
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppLoan] in class [Pages_Business_application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            return false;
        }
    }
    //save body change
    private bool InsertAppInfoBody(string app_id)
    {
        bool status = false;
        try
        {
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            for (int i = 0; i < tblBody.Rows.Count; i++)
            {
                var drow = tblBody.Rows[i];
                if (drow["level"].ToString().Trim() == "1")
                { //level=1 life insure1
                    bl_app_info_body app_info_body = new bl_app_info_body();

                    app_info_body.App_Register_ID = app_id;

                    app_info_body.Height = Convert.ToInt32(drow["height"].ToString().Trim());
                    app_info_body.Weight_Change = 0;
                    app_info_body.Weight = Convert.ToInt32(drow["weight"].ToString().Trim());
                    app_info_body.Is_Weight_Changed = Convert.ToInt32(drow["is_weight_changed"].ToString().Trim());
                    app_info_body.Reason = drow["reason"].ToString().Trim();

                    if (da_application.InsertAppInfoBody(app_info_body))
                    {
                        status= true;
                    }
                    else
                    {
                        //Rollback
                        Rollback(app_info_body.App_Register_ID);
                        status = false;
                    }
                }
                else
                { //level=0  policy owner
                   da_application_fp6. bl_app_info_body_sub app_info_body = new  da_application_fp6.bl_app_info_body_sub();

                    app_info_body.App_Register_ID = app_id;

                    app_info_body.Height = Convert.ToInt32(drow["height"].ToString().Trim());
                    app_info_body.Weight_Change = 0;
                    app_info_body.Weight = Convert.ToInt32(drow["weight"].ToString().Trim());
                    app_info_body.Is_Weight_Changed = Convert.ToInt32(drow["is_weight_changed"].ToString().Trim());
                    app_info_body.Reason = drow["reason"].ToString().Trim();
                    app_info_body.Level = Convert.ToInt32(drow["level"].ToString().Trim());
                    app_info_body.Id = Helper.GetNewGuid("SP_Check_App_Info_Body_Sub_ID", "@Id");

                    if (da_application_fp6.InsertAppInfoBodySub(app_info_body))
                    {
                        status = true;
                    }
                    else
                    {
                        //Rollback
                        Rollback(app_info_body.App_Register_ID);
                        status = false;
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoBody] in class [Pages_Business_application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            status = false;
        }
        return status;
    }
   
    //save life product
    private bool InsertAppLifeProduct(string app_id)
    {
        try
        {
           
            bl_app_life_product app_life_product = new bl_app_life_product();

            string productID = "";
            productID = ddlTypeInsurancePlan.SelectedValue.Trim();

            app_life_product.App_Register_ID = app_id;

            app_life_product.Age_Insure = Convert.ToInt32(txtAssureeAgeLife1.Text.Trim());

            app_life_product.Assure_Year = Convert.ToInt32(txtTermInsurance.Text.Trim());
            app_life_product.Assure_Up_To_Age = app_life_product.Age_Insure + app_life_product.Assure_Year;
            app_life_product.Pay_Year = Convert.ToInt32(txtPaymentPeriod.Text.Trim());
            app_life_product.Pay_Mode = Convert.ToInt32(ddlPremiumMode.SelectedValue.Trim());
            app_life_product.Pay_Up_To_Age = app_life_product.Age_Insure + app_life_product.Pay_Year;
            app_life_product.Product_ID = productID;
            app_life_product.System_Premium = Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim());
            app_life_product.System_Premium_Discount = 0.00;
            app_life_product.System_Sum_Insure = Convert.ToDouble(txtInsuranceAmountRequired.Text);

            if (txtPremiumAmount.Text == "")
            {
                app_life_product.User_Premium = 0;
            }
            else
            {
                app_life_product.User_Premium = Convert.ToDouble(txtPremiumAmount.Text);
            }

            app_life_product.User_Sum_Insure = Convert.ToDouble(txtInsuranceAmountRequired.Text);

            if (da_application.InsertAppLifeProduct(app_life_product))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_life_product.App_Register_ID);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppLifeProduct] in class [Pages_Business_application_form_fp6]. Details: " + ex.Message);
            Rollback(app_id);
            return false;

        }
    }

    //Save Rider
    private bool InsertAppRider(string app_id)
    {
        bool result = false;
        try
        {

            DataTable tblRider = (DataTable)ViewState["tblRider"];
            string productID = "";
            productID = ddlTypeInsurancePlan.SelectedValue.Trim();

            if (tblRider.Rows.Count > 0)
            { //Save
                da_application_fp6.bl_app_rider rider;
                foreach (DataRow row in tblRider.Rows)
                {//Loop in table rider
                    rider = new da_application_fp6.bl_app_rider();

                    rider.App_Register_ID = app_id;
                    rider.Rider_ID = Helper.GetNewGuid("SP_Check_App_Rider_ID", "@Rider_ID");
                    rider.Product_ID = productID;
                    rider.Rider_Type = row["rider_type"].ToString().Trim();
                    rider.SumInsured = Convert.ToDouble(row["sumInsured"].ToString().Trim());
                    rider.Premium = Convert.ToDouble(row["premium"].ToString().Trim());
                    rider.Discount = Convert.ToDouble(row["discount"].ToString().Trim());
                    rider.Level = Convert.ToInt32(row["Level"].ToString().Trim());
                    rider.Rate = Convert.ToDouble(row["rate"].ToString());
                    rider.Rate_ID = Convert.ToInt32(row["rate_id"].ToString());
                    rider.Original_Amount = Convert.ToDouble(row["original_amount"].ToString().Trim());
                    rider.Rounded_Amount = Convert.ToDouble(row["rounded_amount"].ToString().Trim());
                    rider.Age_Insure = Convert.ToInt32(row["Age_Insure"].ToString().Trim());
                    rider.Pay_Year = Convert.ToInt32(row["Pay_Year"].ToString().Trim());
                    rider.Pay_Up_To_Age = Convert.ToInt32(row["Pay_Up_To_Age"].ToString().Trim());
                    rider.Assure_Year = Convert.ToInt32(row["Assure_Year"].ToString().Trim());
                    rider.Assure_Up_To_Age = Convert.ToInt32(row["Assure_Up_To_Age"].ToString().Trim());
                    rider.Effective_Date = Helper.FormatDateTime (row["Effective_Date"].ToString().Trim());
                   
                    rider.Created_On = DateTime.Now;
                    rider.Action = "new";

                    if (da_application_fp6.InsertAppRider(rider))
                    {
                        result = true;
                    }
                    else
                    {
                        //Rollback
                        Rollback(rider.App_Register_ID);
                        result = false;
                    }
                }
                
            }
            else 
            {//alert message
                //lblMessageApplication.Text = "";
                //set result = true in case no rider
                result = true;
            }
                        
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppRider] in class [Pages_Business_application_form_fp6]. Details: " + ex.Message);
            Rollback(app_id);
            result= false;

        }
        return result;
    }

    private bool UpdateInsuredPlan(string app_id)
    {
       
        SqlConnection myCon = new SqlConnection(AppConfiguration.GetConnectionString());
        SqlCommand myCmd;
       // SqlTransaction myTransaction;
            
        myCon.Open();
       // myTransaction = myCon.BeginTransaction();

        try
        {
            string productID = "";
            productID = ddlTypeInsurancePlan.SelectedValue.Trim();

            #region Update Application Premium Pay

            myCmd = new SqlCommand();
            myCmd.Connection = myCon;
            //myCmd.Transaction = myTransaction;
            myCmd.CommandType= CommandType.StoredProcedure;
            myCmd.CommandText="SP_Update_App_Prem_Pay_Original_Amount_FP6";

            double amount = 0.0;
            double OriginalAmount = 0.0;
            double roundedAmount = 0.0;
          
            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int Count = tblPersonal.Rows.Count;
            #region Old Code
            /*
            for (int i = 0; i < Count; i++)
            {
                var rowIndex = tblPersonal.Rows[i];
                if (rowIndex["id"].ToString().Trim() != "")
                {
                    #region Life insure 1
                    if (rowIndex["level"].ToString().Trim() == "1")
                    {
                        int records = 0;
                        records = da_application_fp6.GetPremiumPayRecords(app_id, 1);

                        if (records > 0)
                        { //update
                            if (txtPremiumAmountSystemLife1.Text.Trim() == "")
                            {
                                amount = 0.00;
                            }
                            else
                            {
                                amount = Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim());
                            }

                            if (txtPremiumOriginalAmountLife1.Text.Trim() == "")
                            {
                                OriginalAmount = 0.00;
                                roundedAmount = 0.00;
                            }
                            else
                            {
                                OriginalAmount = Convert.ToDouble(txtPremiumOriginalAmountLife1.Text.Trim());
                                roundedAmount = Math.Ceiling(Convert.ToDouble(txtPremiumOriginalAmountLife1.Text.Trim()));
                            }

                            var a = myCmd.Parameters;

                            a.AddWithValue("@App_Register_ID", app_id);
                            a.AddWithValue("@Original_Amount", OriginalAmount);
                            a.AddWithValue("@Rounded_Amount", roundedAmount);
                            a.AddWithValue("@Amount", amount);
                            a.AddWithValue("@Level", 1);

                            myCmd.ExecuteNonQuery();
                            myCmd.Parameters.Clear();
                      
                        }
                        else
                        { //save
                            string Created_By = ViewState["userName"] + "";
                            String Created_Note = txtNote.Text.Trim();

                            SavePremPay(Helper.GetNewGuid("SP_Check_App_Prem_Pay_ID", "@App_Prem_Pay_ID"), 
                                        app_id,
                                        Helper.FormatDateTime(txtDateSignature.Text.Trim()), 
                                        1,
                                        Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim()),
                                        Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim()),
                                        Math.Ceiling(Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim())),
                                        Created_By,
                                        Created_Note,
                                        DateTime.Now,
                                        0);
                        }
                        
                       
                    }
                    #endregion
                
                }
            }
           */
            #endregion End Old Code
            for (int i = 0; i < Count; i++)
            {
                var rowIndex = tblPersonal.Rows[i];

                    #region Life insure 1
                    if (rowIndex["level"].ToString().Trim() == "1")
                    {
                        int records = 0;
                        records = da_application_fp6.GetPremiumPayRecords(app_id, 1);

                        if (records > 0)
                        { //update
                            if (txtPremiumAmountSystemLife1.Text.Trim() == "")
                            {
                                amount = 0.00;
                            }
                            else
                            {
                                amount = Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim());
                            }

                            if (txtPremiumOriginalAmountLife1.Text.Trim() == "")
                            {
                                OriginalAmount = 0.00;
                                roundedAmount = 0.00;
                            }
                            else
                            {
                                OriginalAmount = Convert.ToDouble(txtPremiumOriginalAmountLife1.Text.Trim());
                                roundedAmount = Math.Ceiling(Convert.ToDouble(txtPremiumOriginalAmountLife1.Text.Trim()));
                            }

                            var a = myCmd.Parameters;

                            a.AddWithValue("@App_Register_ID", app_id);
                            a.AddWithValue("@Original_Amount", OriginalAmount);
                            a.AddWithValue("@Rounded_Amount", roundedAmount);
                            a.AddWithValue("@Amount", amount);
                            a.AddWithValue("@Level", 1);

                            myCmd.ExecuteNonQuery();
                            myCmd.Parameters.Clear();

                        }
                        else
                        { //save
                            string Created_By = ViewState["userName"] + "";
                            String Created_Note = txtNote.Text.Trim();

                            SavePremPay(Helper.GetNewGuid("SP_Check_App_Prem_Pay_ID", "@App_Prem_Pay_ID"),
                                        app_id,
                                        Helper.FormatDateTime(txtDateSignature.Text.Trim()),
                                        1,
                                        Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim()),
                                        Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim()),
                                        Math.Ceiling(Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim())),
                                        Created_By,
                                        Created_Note,
                                        DateTime.Now,
                                        1);
                        }


                    }
                    #endregion
                
            }
            #endregion

             #region Update Life product
            //policy owner

             DataTable tblPerson = (DataTable)ViewState["tblPersonal"];
             int personCount = tblPerson.Rows.Count;
             for (int i = 0; i < personCount; i++)
             {
                 
                 string id= tblPerson.Rows[i]["id"].ToString().Trim();

                 //count record base on application id and level of life insured sub
                 int level =Convert.ToInt32( tblPerson.Rows[i]["level"].ToString().Trim());
                 int existRecord = da_application_fp6.GetProductLifeSubRecords(app_id, level);

                 if (level == 1)
                 {
                     #region Life insure 1
                     if (txtAssureeAgeLife1.Text.Trim() != "")
                     {
                         if (id == "")
                         {//save
                             bl_app_life_product life = new bl_app_life_product();
                          
                             life.App_Register_ID= app_id;
                             life.Product_ID = productID;
                             life.Age_Insure=Convert.ToInt32(txtAssureeAgeLife1.Text.Trim());
                             life.Pay_Year=Convert.ToInt32(txtPaymentPeriod.Text.Trim());
                             life.Pay_Up_To_Age=Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim());
                             life.Assure_Year = Convert.ToInt32(txtTermInsurance.Text.Trim());
                             life.Assure_Up_To_Age=Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim());
                             life.User_Sum_Insure = Convert.ToDouble(txtInsuranceAmountRequired.Text);
                             life.System_Sum_Insure = Convert.ToDouble(txtInsuranceAmountRequired.Text);
                             life.User_Premium=Convert.ToDouble(txtPremiumAmount.Text.Trim());
                             //life.System_Premium=Convert.ToDouble(txtPremiumOriginalAmountLife1.Text.Trim());
                             life.System_Premium = Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim());
                             life.System_Premium_Discount=0.0;
                             life.Pay_Mode=Convert.ToInt32(ddlPremiumMode.SelectedValue);
                           
                             da_application_fp6.InsertAppLifeProduct(life);

                         }
                         else
                         { //update
                             myCmd = new SqlCommand();
                             myCmd.Connection = myCon;
                             // myCmd.Transaction = myTransaction;
                             myCmd.CommandType = CommandType.StoredProcedure;
                             myCmd.CommandText = "SP_Update_App_Life_Product";
                             var b = myCmd.Parameters;

                             b.AddWithValue("@App_Register_ID", app_id);
                             b.AddWithValue("@Product_ID", productID);
                             b.AddWithValue("@Age_Insure", txtAssureeAgeLife1.Text.Trim());
                             b.AddWithValue("@Pay_Year", txtPaymentPeriod.Text.Trim());
                             b.AddWithValue("@Pay_Up_To_Age", Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                             b.AddWithValue("@Assure_Year", txtTermInsurance.Text.Trim());
                             b.AddWithValue("@Assure_Up_To_Age", Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                             b.AddWithValue("@User_Sum_Insure", Convert.ToDouble(txtInsuranceAmountRequired.Text));
                             b.AddWithValue("@System_Sum_Insure", Convert.ToDouble(txtInsuranceAmountRequired.Text));
                             //b.AddWithValue("@User_Premium", Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim()));
                             b.AddWithValue("@User_Premium", Convert.ToDouble(txtPremiumAmount.Text.Trim()));
                             //b.AddWithValue("@System_Premium", Convert.ToDouble(txtPremiumOriginalAmountLife1.Text.Trim()));
                             b.AddWithValue("@System_Premium", Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim()));
                             b.AddWithValue("@System_Premium_Discount", 0.0);
                             b.AddWithValue("@Pay_Mode", ddlPremiumMode.SelectedValue);

                             myCmd.ExecuteNonQuery();
                             myCmd.Parameters.Clear();
                         }
                         
                     }
                 }
                     #endregion
                
             }
             #endregion
             
            #region Update Rider
             
            DataTable tblRider = (DataTable)ViewState["tblRider"];
            int rowCount=0;
            rowCount=tblRider.Rows.Count;
            //Loop row in table rider
            for (int ii = 0; ii < rowCount; ii++)
            {
                var row = tblRider.Rows[ii];
                if (row["rider_id"].ToString().Trim() == "")
                { //save new record
                  /*
                    string riderID=Helper.GetNewGuid("SP_Check_App_Rider_ID", "@Rider_ID");
                    myCmd = new SqlCommand();
                    myCmd.Connection = myCon;
                    //myCmd.Transaction = myTransaction;
                    myCmd.CommandType = CommandType.StoredProcedure;
                    myCmd.CommandText = "SP_Insert_App_Rider";

                    myCmd.Parameters.AddWithValue("@Rider_ID", riderID);
                    myCmd.Parameters.AddWithValue("@App_Register_ID", row["app_register_id"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Rider_Type", row["rider_type"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Sum_Insured", row["sumInsured"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Premium", row["premium"].ToString().Trim());
                    //myCmd.Parameters.AddWithValue("@Discount", row["discount"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Discount", Convert.ToDouble(txtRiderDiscountAmount.Text.Trim()));
                    myCmd.Parameters.AddWithValue("@Level", row["level"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Product_ID", row["product_id"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Rate",Convert.ToDouble( row["Rate"].ToString()));
                    myCmd.Parameters.AddWithValue("@Rate_ID", Convert.ToInt32(row["rate_id"].ToString()));
                    myCmd.Parameters.AddWithValue("@Original_Amount", Convert.ToDouble(row["original_amount"].ToString()));
                    myCmd.Parameters.AddWithValue("@Rounded_Amount", Convert.ToDouble(row["rounded_amount"].ToString()));

                    myCmd.Parameters.AddWithValue("@Age_Insure",Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Pay_Year",Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Pay_Up_To_Age", Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Assure_Year", Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Assure_Up_To_Age", Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim()));

                    myCmd.Parameters.AddWithValue("@Pay_Year", My_View_State.assure_pay_year.Pay_Year);
                    myCmd.Parameters.AddWithValue("@Pay_Up_To_Age", My_View_State.assure_pay_year.Pay_Up_To_Age);
                    myCmd.Parameters.AddWithValue("@Assure_Year", My_View_State.assure_pay_year.Assure_Year);
                    myCmd.Parameters.AddWithValue("@Assure_Up_To_Age", My_View_State.assure_pay_year.Assure_Up_To_Age);
                    myCmd.Parameters.AddWithValue("@Effective_Date", Helper.FormatDateTime(txtDateEntry.Text.Trim()));

                    myCmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
                    myCmd.Parameters.AddWithValue("@Action", "new");

                    myCmd.ExecuteNonQuery();
                    myCmd.Parameters.Clear();
                    */

                    da_application_fp6.bl_app_rider rider;
                    rider = new da_application_fp6.bl_app_rider();

                    rider.App_Register_ID = app_id;
                    rider.Rider_ID = Helper.GetNewGuid("SP_Check_App_Rider_ID", "@Rider_ID");
                    rider.Product_ID = productID;
                    rider.Rider_Type = row["rider_type"].ToString().Trim();
                    rider.SumInsured = Convert.ToDouble(row["sumInsured"].ToString().Trim());
                    rider.Premium = Convert.ToDouble(row["premium"].ToString().Trim());
                    rider.Discount = Convert.ToDouble(row["discount"].ToString().Trim());
                    rider.Level = Convert.ToInt32(row["Level"].ToString().Trim());
                    rider.Rate = Convert.ToDouble(row["rate"].ToString());
                    rider.Rate_ID = Convert.ToInt32(row["rate_id"].ToString());
                    rider.Original_Amount = Convert.ToDouble(row["original_amount"].ToString().Trim());
                    rider.Rounded_Amount = Convert.ToDouble(row["rounded_amount"].ToString().Trim());
                    rider.Age_Insure = Convert.ToInt32(row["Age_Insure"].ToString().Trim());
                    rider.Pay_Year = Convert.ToInt32(row["Pay_Year"].ToString().Trim());
                    rider.Pay_Up_To_Age = Convert.ToInt32(row["Pay_Up_To_Age"].ToString().Trim());
                    rider.Assure_Year = Convert.ToInt32(row["Assure_Year"].ToString().Trim());
                    rider.Assure_Up_To_Age = Convert.ToInt32(row["Assure_Up_To_Age"].ToString().Trim());
                    rider.Effective_Date = Helper.FormatDateTime(row["Effective_Date"].ToString().Trim());
                    rider.Created_On = DateTime.Now;
                    rider.Action = "new";

                    da_application_fp6.InsertAppRider(rider);


                }
                else
                { //update old record
                    
                    myCmd = new SqlCommand();
                    myCmd.Connection = myCon;
                    //myCmd.Transaction = myTransaction;
                    myCmd.CommandType = CommandType.StoredProcedure;
                    myCmd.CommandText = "SP_Update_App_Rider_By_App_ID";
                   
                    string riderid = row["rider_id"].ToString().Trim();
                    string test = row["rate_id"].ToString();
                    myCmd.Parameters.AddWithValue("@Rider_ID", row["rider_id"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@App_Register_ID", row["app_register_id"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Rider_Type", row["rider_type"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Sum_Insured", row["sumInsured"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Premium", row["premium"].ToString().Trim());
                    //myCmd.Parameters.AddWithValue("@Discount", row["discount"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Discount", Convert.ToDouble(txtRiderDiscountAmount.Text.Trim()));
                    myCmd.Parameters.AddWithValue("@Level", row["level"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Product_ID", row["product_id"].ToString().Trim());
                    myCmd.Parameters.AddWithValue("@Rate", Convert.ToDouble(row["Rate"].ToString()));
                   // string text = row["rate_id"].ToString();
                    myCmd.Parameters.AddWithValue("@Rate_ID", Convert.ToInt32(row["rate_id"].ToString()));
                    myCmd.Parameters.AddWithValue("@Original_Amount", Convert.ToDouble(row["original_amount"].ToString()));
                    myCmd.Parameters.AddWithValue("@Rounded_Amount", Convert.ToDouble(row["rounded_amount"].ToString()));

                    myCmd.Parameters.AddWithValue("@Age_Insure", Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Pay_Year", Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Pay_Up_To_Age", Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Assure_Year", Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Assure_Up_To_Age", Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()) + Convert.ToInt32(txtPaymentPeriod.Text.Trim()));
                    //myCmd.Parameters.AddWithValue("@Effective_Date", Helper.FormatDateTime(txtDateEntry.Text.Trim()));

                    My_View_State.assure_pay_year = new da_application_fpp.Cal_Age_Assure_Pay_Year(Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()),  Convert.ToInt32(txtTermInsurance.Text.Trim()),65, 0);

                    myCmd.Parameters.AddWithValue("@Pay_Year", My_View_State.assure_pay_year.Pay_Year);
                    myCmd.Parameters.AddWithValue("@Pay_Up_To_Age", My_View_State.assure_pay_year.Pay_Up_To_Age);
                    myCmd.Parameters.AddWithValue("@Assure_Year", My_View_State.assure_pay_year.Assure_Year);
                    myCmd.Parameters.AddWithValue("@Assure_Up_To_Age", My_View_State.assure_pay_year.Assure_Up_To_Age);
                    myCmd.Parameters.AddWithValue("@Effective_Date", Helper.FormatDateTime(txtDateEntry.Text.Trim()));

                    myCmd.ExecuteNonQuery();
                    myCmd.Parameters.Clear();
                    
                }
            }
            //Delete record
            if (ViewState["tblRiderID"] != null)
            {
                string riderID = ViewState["tblRiderID"] + "";
                riderID = riderID.Substring(0, riderID.Length - 1);//trim [,] charater
                char[] split = { ',' };
                string[] idDelete = riderID.Split(split);
                for (int j = 0; j < idDelete.Length; j++)
                {
                    myCmd = new SqlCommand();
                    myCmd.Connection = myCon;
                    //myCmd.Transaction = myTransaction;
                    myCmd.CommandType = CommandType.StoredProcedure;
                    myCmd.CommandText = "SP_Delete_App_Rider_By_ID";
                    myCmd.Parameters.AddWithValue("@Rider_ID", idDelete[j]);
                    myCmd.ExecuteNonQuery();
                    myCmd.Parameters.Clear();
                }
                ViewState["tblRiderID"] = null;
            }

            //Reload Data
            LoadRider(app_id);

             #endregion

             #region Update Application Discount
            myCmd = new SqlCommand();
            myCmd.Connection = myCon;
          //  myCmd.Transaction = myTransaction;
            myCmd.CommandType = CommandType.StoredProcedure;
            myCmd.CommandText = "SP_Update_App_Discount";

            double life1 = 0.0;
            double discount = 0.0;
            if (txtDiscountAmount.Text.Trim() != "")
            { 
                discount= Convert.ToDouble(txtDiscountAmount.Text);
            }

            if (txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim() != "")
            {
                life1 = Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim());
            }

            var app=myCmd.Parameters;
            app.AddWithValue("@App_Register_ID", app_id);
            app.AddWithValue("@Annual_Premium", life1 );
            app.AddWithValue("@Discount_Amount", discount);
            app.AddWithValue("@Pay_Mode", Convert.ToInt32(ddlPremiumMode.SelectedValue));
            //app.AddWithValue("@Premium", Convert.ToDouble(txtTotalPremium.Text.Trim()));
            app.AddWithValue("@Premium", Convert.ToDouble(txtTotalPremium.Text.Trim()) + Convert.ToDouble(txtRiderTotalPreium.Text.Trim()));
            app.AddWithValue("@Total_Amount", Convert.ToDouble(txtTotalPremiumAfterDiscount.Text.Trim()));
            app.AddWithValue("@Created_Note", txtInsurancePlanNote.Text);

            myCmd.ExecuteNonQuery();
            myCmd.Parameters.Clear();

 
            #endregion

            #region Update Loan
            if (txtInterest.Text.Trim() != "" && txtTermLoan.Text.Trim() != "" && txtLoanEffectiveDate.Text.Trim() != "" && txtOutstandingLoanAmount.Text.Trim() != "")
             {
                 double Interest_Rate, Out_Std_Loan;
                     int Loan_Type,Term_Year;
                 if (txtInterest.Text.Trim() != "")
                 {
                     Interest_Rate = Convert.ToDouble(txtInterest.Text.Trim());
                 }
                 else
                 {
                    Interest_Rate = 0;
                 }

                 DateTime loan_effective_date = Helper.FormatDateTime(txtLoanEffectiveDate.Text);

                  Loan_Type = Convert.ToInt32(ddlLoanType.SelectedValue);

                 if (txtOutstandingLoanAmount.Text != "")
                 {
                     Out_Std_Loan = Convert.ToDouble(txtOutstandingLoanAmount.Text);
                 }
                 else
                 {
                     Out_Std_Loan = 0;
                 }

                 if (txtTermLoan.Text != "")
                 {
                    Term_Year = Convert.ToInt32(txtTermLoan.Text);
                 }
                 else
                 {
                     Term_Year = 0;
                 }

                 myCmd = new SqlCommand();
                 myCmd.Connection = myCon;
                // myCmd.Transaction = myTransaction;
                 myCmd.CommandType = CommandType.StoredProcedure;
                 myCmd.CommandText = "SP_Update_App_Loan";
                 var h = myCmd.Parameters;

                 h.AddWithValue("@App_Register_ID", app_id);
                 h.AddWithValue("@Interest_Rate", Interest_Rate);
                 h.AddWithValue("@Loan_Effiective_Date", loan_effective_date);
                 h.AddWithValue("@Loan_Type", Loan_Type);
                 h.AddWithValue("@Out_Std_Loan", Out_Std_Loan);
                 h.AddWithValue("@Term_Year", Term_Year);

                 myCmd.ExecuteNonQuery();
                 myCmd.Parameters.Clear();

             }
             #endregion
            
           // myTransaction.Commit();

           
           // myTransaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function UpdateInsuredPlan in page application_form_fp.aspx.cs, Detail: " + ex.Message);
            //myTransaction.Rollback();
            return false;
        }
    }

    bool UpdateUserPremium(string app_id, int user_premium)
    {
        
        return da_application_fp6.UpdateAppUserPremium(app_id, user_premium);
    }
    
    //save discount
    private bool InsertAppDiscount(string app_id)
    {
        try
        {
         
            bl_app_discount app_discount = new bl_app_discount();

            app_discount.App_Register_ID = app_id;

            double life1 = 0.0;

            if (txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim() != "") {
                life1 = Convert.ToDouble(txtAnnualOriginalPremiumAmountSystemLife1.Text.Trim());
            }

            app_discount.Annual_Premium = life1;
            double discount = 0.0;
            if (txtDiscountAmount.Text.Trim() != "")
            {
                discount = Convert.ToDouble(txtDiscountAmount.Text);
            }

            double rider_total_premium = 0;
            if (txtRiderTotalPreium.Text.Trim() != "")
            {
                rider_total_premium = Convert.ToDouble(txtRiderTotalPreium.Text.Trim());
            }
            app_discount.Discount_Amount = discount;
            app_discount.Pay_Mode = Convert.ToInt32(ddlPremiumMode.SelectedValue);
            app_discount.Premium = Convert.ToDouble(txtTotalPremium.Text.Trim()) + rider_total_premium;

            if (txtTotalPremiumAfterDiscount.Text.Trim() == "")
            {
                app_discount.Total_Amount = 0;
            }
            else
            {
                app_discount.Total_Amount = Convert.ToDouble(txtTotalPremiumAfterDiscount.Text.Trim());
            }

            app_discount.Created_On = DateTime.Now;
            app_discount.Created_Note = txtInsurancePlanNote.Text;
            app_discount.Created_By = ViewState["userName"]+"";

            if (da_application.InsertAppDiscount(app_discount))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_discount.App_Register_ID);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppDiscount] in class [Pages_Business_application_form_fp6]. Details: " + ex.Message);
            Rollback(app_id);
            return false;

        }
    }
  
    //save answer
    private bool InsertAppAnswerItem(string app_id)
    {
        try
        {
            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int rowCount= tblPersonal.Rows.Count;
        
            for (int i = 0; i < rowCount; i++) {

                foreach (GridViewRow row in GvQA.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {

                        RadioButtonList rbtnlAnswer1 = (RadioButtonList)row.FindControl("rbtnlAnswerLife1");
                        Label hdfSeqNumber1 = (Label)row.FindControl("lblSeqNumberLife1");
                        Label hdfQuestionID1 = (Label)row.FindControl("lblQuestionIDLife1");

                        RadioButtonList rbtnlAnswer2 = (RadioButtonList)row.FindControl("rbtnlAnswerLife2");
                        Label hdfSeqNumber2 = (Label)row.FindControl("lblSeqNumberLife2");
                        Label hdfQuestionID2 = (Label)row.FindControl("lblQuestionIDLife2");

                        string new_guid = "";

                        da_application_fp6.bl_app_answer_item_fp6 app_answer_item = new da_application_fp6.bl_app_answer_item_fp6();
                       
                        //life insured 1
                        if (i == 0)
                        {
                            int answer = 0;
                            if(rbtnlAnswer1.SelectedIndex>=0){
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID1.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber1.Text.Trim());
                                answer=Convert.ToInt32(rbtnlAnswer1.SelectedValue);
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer1.SelectedValue);
                                app_answer_item.Level = 0;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    //Rollback
                                    Rollback(app_answer_item.App_Register_ID);
                                    return false;
                                }
                            }
                        }

                        //policy owner   
                        else if (i == 1)
                        {
                            int answer = 0;
                            if (rbtnlAnswer2.SelectedIndex >= 0)
                            {
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID2.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber2.Text.Trim());
                                answer = Convert.ToInt32(rbtnlAnswer2.SelectedValue);
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer2.SelectedValue);
                                app_answer_item.Level = 1;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    //Rollback
                                    Rollback(app_answer_item.App_Register_ID);
                                    return false;
                                }
                            }
                        }
                    }
                }
             }

            //if reach here return true
            return true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppAnswerItem] in class [Pages_Business_application_form_fp6]. Details: " + ex.Message);
            Rollback(app_id);
            return false;

        }
    }
    //Save benifit
    public bool SaveBenefiter(string app_id)
    {
        bool result = false;
        try
        {
            int seqNumber=0;
            foreach (GridViewRow row in gvBenifit.Rows) {
                if (row.RowType == DataControlRowType.DataRow) {

                    Label lblIdType = (Label)row.FindControl("lblIDType");
                    Label lblIdNumber = (Label)row.FindControl("lblIDCard");
                    Label lblFullName = (Label)row.FindControl("lblFullName");
                    Label lblRelation = (Label)row.FindControl("lblRelation");
                    Label lblShared = (Label)row.FindControl("lblPercentage");
                    Label lblRemarks = (Label)row.FindControl("lblRemarks");

                    int idType = -1;
                    string idNumber = "";
                    string fullName = "";
                    string relation = "";
                    string remarks = "";
                    int shared = 0;
                  

                    idType =Convert.ToInt16( lblIdType.Text.Trim());
                    idNumber = lblIdNumber.Text.Trim();
                    fullName = lblFullName.Text.Trim();
                    relation = lblRelation.Text.Trim();
                    shared = Convert.ToInt16(lblShared.Text.Trim());
                    remarks = lblRemarks.Text.Trim();

                    //if (idNumber != "" && fullName != "" && shared > 0) {
                    if (idNumber != "" && fullName != "" && shared >= 0)
                    {
                        string new_guid = Helper.GetNewGuid("SP_Check_App_Benefit_Item_ID", "@App_Benefit_Item_ID").ToString();
                        bl_app_benefit_item benefit_item = new bl_app_benefit_item();

                        seqNumber = seqNumber + 1;

                        benefit_item.App_Benefit_Item_ID = new_guid;
                        benefit_item.App_Register_ID = app_id;
                        benefit_item.Full_Name = fullName;
                        benefit_item.ID_Type = idType;
                        benefit_item.ID_Card = idNumber;
                        benefit_item.Percentage = shared;
                        benefit_item.Relationship = relation;
                        benefit_item.Seq_Number = seqNumber;
                        benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);
                        benefit_item.Remarks = remarks;

                        if (!da_application.InsertAppBenefitItem(benefit_item))
                        { //if saving failed rollback
                            Rollback(app_id);
                            result= false;
                        }
                        else {
                            result= true;
                        }
                    
                    }
                }
            
            }

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [SaveBenefiter] in page [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            Rollback(app_id);
            result= false;
        }
        return result;
    }
    //Insert Ct_App_Step_History
    private void InsertAppStepHistory(string app_id)
    {
        try
        {
            bl_app_step_history app_step_history = new bl_app_step_history();

            app_step_history.App_Register_ID = app_id;

            app_step_history.Txn_ID = Helper.GetNewGuid("SP_Check_App_Step_History_ID", "@Txn_ID");
            app_step_history.Step_ID = da_step.GetAppRegisterdStepID();
            app_step_history.Created_By = ViewState["userId"]+"";
            app_step_history.Created_On = System.DateTime.Now;
            app_step_history.Created_Note = "";

            da_step.InsertAppStepHistory(app_step_history);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppStepHistory] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);

        }
    }

    //Insert Ct_App_Step_Next
    private void InsertAppStepNext(string app_id)
    {
        try
        {
            bl_app_step_next app_step_next = new bl_app_step_next();

            app_step_next.App_Register_ID = app_id;
            app_step_next.Step_ID = da_step.GetAppUnderwritingStepID();
            app_step_next.Last_Updated = System.DateTime.Now;

            da_step.InsertAppStepNext(app_step_next);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppStepHistory] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
        }
    }

    //Rollback
    private static void Rollback(string app_id)
    {
        da_application_fp6.DeleteAppInfoPerson(app_id);
        da_application_fp6.DeleteAppInfoPerson_Sub(app_id);

        da_application_fp6.DeleteAppAnswerItem(app_id);

        da_application_fp6.DeleteAppLifeProduct(app_id);
        da_application_fp6.DeleteAppLifeProductSub(app_id);

        da_application_fp6.DeleteAppRider(app_id);

        da_application_fp6.DeleteAppInfoBody(app_id);
        da_application_fp6.DeleteAppInfoBodySub(app_id);

        da_application_fp6.DeleteAppLoan(app_id);

        da_application_fp6.DeleteAppJobHistory(app_id);
        da_application_fp6.DeleteAppJobHistorySub(app_id);

        da_application_fp6.DeleteAppInfoAddress(app_id);
        da_application_fp6.DeleteAppInfoAddressSub(app_id);

        da_application_fp6.DeleteAppInfoContact(app_id);
        da_application_fp6.DeleteAppInfoContactSub(app_id);

        da_application_fp6.DeleteAppPremPay(app_id);

        da_application_fp6.DeleteAppBenefitItem(app_id);

        da_application_fp6.DeleteAppPremiumDiscount(app_id);

        da_application_fp6.DeleteAppDiscount(app_id);

        da_application_fp6.DeleteAppInfo(app_id);
        da_application_fp6.DeleteAppRegister(app_id);


    }

    void BindPersonInDropdownList()
    {

        //add person list into dropdown list
        DataTable tbl =  (DataTable)ViewState["tblPersonal"];
        
        //dropdownlist in job history section
        ddlJobPolicyOwner.Items.Clear();
        ddlJobPolicyOwner.Items.Add(new ListItem(".", ""));

        //dropdownlist in health section
        ddlBodyPerson.Items.Clear();
        ddlBodyPerson.Items.Add(new ListItem(".", ""));

        //dropdownlist in plan section
        ddlRiderPerson.Items.Clear();
        ddlRiderPerson.Items.Add(new ListItem(".", ""));

        string text = "";
        string val = "";
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            text = tbl.Rows[i]["level"].ToString() + "-" + tbl.Rows[i]["surEnName"].ToString() + " " + tbl.Rows[i]["firstEnName"].ToString();
            val = tbl.Rows[i]["level"].ToString();
            //ddlJobPolicyOwner.Items.Add(new ListItem(tbl.Rows[i]["surEnName"].ToString() + " " + tbl.Rows[i]["firstEnName"].ToString(), tbl.Rows[i]["level"].ToString()));
            //ddlBodyPerson.Items.Add(new ListItem(tbl.Rows[i]["surEnName"].ToString() + " " + tbl.Rows[i]["firstEnName"].ToString(), tbl.Rows[i]["level"].ToString()));
            //ddlRiderPerson.Items.Add(new ListItem(tbl.Rows[i]["surEnName"].ToString() + " " + tbl.Rows[i]["firstEnName"].ToString(), tbl.Rows[i]["level"].ToString()));
            ddlJobPolicyOwner.Items.Add(new ListItem(text, val));
            ddlBodyPerson.Items.Add(new ListItem(text, val));
            int level = Convert.ToInt32(tbl.Rows[i]["level"].ToString());
            //bind only life insured into dropdownlist rider person
            if (level > 0)
            {
                ddlRiderPerson.Items.Add(new ListItem(text, val));
            }
            
        }
    }

    //load person
    private  void LoadPerson(string appID) {

        DataTable tbl = new DataTable();
        tbl = da_application_fp6.GetDataTable("SP_Get_App_Info_Person_FP6_By_App_Register_ID", appID);

        ViewState["tblPersonal"] = tbl;
        gvPersonalInfo.DataSource = tbl;
        gvPersonalInfo.DataBind();

        //show all people in to dropdown list
        BindPersonInDropdownList();

    }

    //load underwriting status
    private void LoadUnderWritStatusCode(string app_id)
    {
        DataTable tbl = new DataTable();
        tbl = DataSetGenerator.Get_Data_Soure("select Status_Code from Ct_Underwriting Where App_Register_ID ='" + app_id + "'");
        if (tbl.Rows.Count > 0)
        {
            txtUnderwritingStatus.Text = tbl.Rows[0]["status_code"].ToString().ToUpper();
        }
        else
        {
            txtUnderwritingStatus.Text = "";
        }
    }
    //load policy number
    private void LoadPolicyNumber(string app_id){
        DataTable tbl = new DataTable();
        tbl = DataSetGenerator.Get_Data_Soure("select Ct_Policy_Number.Policy_Number from Ct_App_Policy "+
                                            "INNER JOIN Ct_Policy ON Ct_Policy.Policy_ID = Ct_App_Policy.Policy_ID "+
                                            "INNER JOIN Ct_Policy_Number ON Ct_Policy_Number.Policy_ID = Ct_Policy.Policy_ID WHERE Ct_App_Policy.App_Register_ID='" + app_id + "'");
        if (tbl.Rows.Count > 0)
        {
            txtPolicyNumber.Text = tbl.Rows[0]["Policy_Number"].ToString().ToUpper();
        }
        else
        {
            txtPolicyNumber.Text = "";
        }
    }
    
    //load application
    private void LoadApplication(string appID)
    {
        List<bl_app_register> myList = new List<bl_app_register>();

        myList = da_application_fp6.GetApplication(appID);
        if (myList.Count > 0) {

            var i = myList[0];
            txtApplicationNumber.Text = i.App_Number;
            
            txtPaymentCode.Text = i.Payment_Code;

            txtNote.Text = i.Created_Note;

           
            Helper.SelectedDropDownListIndex("VALUE", ddlChannel,i.Channel_ID);
            Helper.SelectedDropDownListIndex("VALUE", ddlCompany, i.Channel_Item_ID);
           
        }
    }

    //load application info
    private void LoadApplicationInfo(string appID) {

        List<bl_app_info> mylist = new List<bl_app_info>();
        mylist = da_application_fp6.GetApplicationInfo(appID);
        if (mylist.Count > 0) {
            var i = mylist[0];

            txtDateEntry.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(i.App_Date));
            txtDataEntryBy.Text = i.Created_By;
            txtDateSignature.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(i.Created_On));
            
            txtMarketingCode.Text = i.Sale_Agent_ID;
            txtMarketingName.Text = da_sale_agent.GetSaleAgentNameByID(i.Sale_Agent_ID);
            txtBenefitNote.Text = i.Benefit_Note;

        }
       
    }

    //Load address 
    private void LoadAddress(string appID) {

        List<bl_app_info_address> myList = new List<bl_app_info_address>();
        myList = da_application_fp6.GetApplicationAddress(appID);
        if (myList.Count > 0) {
            var i = myList[0];

            txtAddress1Life2.Text = i.Address1;
            txtAddress2Life2.Text = i.Address2;
            txtCityLife2.Text = i.Province;
            txtZipCodeLife2.Text = i.Zip_Code;
            Helper.SelectedDropDownListIndex("VALUE", ddlCountryLife2, i.Country_ID);
        
        }
       
    }


    //Load address sub
    private void LoadAddressSub(string appID) {

        List<bl_app_info_address> myList = new List<bl_app_info_address>();
        myList = da_application_fp6.GetApplicationAddressSub(appID);
        if (myList.Count > 0) {
            var i = myList[0];

            txtAddress1Life1.Text = i.Address1;
            txtAddress2Life1.Text = i.Address2;
            txtCityLife1.Text = i.Province;
            txtZipCodeLife1.Text = i.Zip_Code;
            Helper.SelectedDropDownListIndex("VALUE",ddlCountryLife1, i.Country_ID);
        
        }
    }

    //Load contact
    private void LoadContact(string appID) {
        List<bl_app_info_contact> myList = new List<bl_app_info_contact>();
        myList = da_application_fp6.GetApplicationContact(appID);
        if (myList.Count > 0) {
            var i = myList[0];

            txtTelephoneLife2.Text = i.Home_Phone1;
            txtMobilePhoneLife2.Text = i.Mobile_Phone1;
            txtEmailLife2.Text = i.EMail;

        }
    }


    //load contact sub
    private void LoadContactSub(string appID) {

        List<bl_app_info_contact> myList = new List<bl_app_info_contact>();
        myList = da_application_fp6.GetApplicationContactSub(appID);
        if (myList.Count > 0)
        {
            var i = myList[0];

            txtTelephoneLife1.Text = i.Home_Phone1;
            txtMobilePhoneLife1.Text = i.Mobile_Phone1;
            txtEmailLife1.Text = i.EMail;

        }
    }

    //load loan
    private void loadLoan(string appID) {

        bl_app_loan myLoan = new bl_app_loan();
        myLoan = da_application.GetAppLoan(appID);
        if (!DBNull.Value.Equals(myLoan.App_Register_ID) && myLoan.App_Register_ID!=null)
        {
            txtLoanEffectiveDate.Text = string.Format("{0:dd/MM/yyyy}", myLoan.Loan_Effiective_Date);
            txtInterest.Text = myLoan.Interest_Rate + "";
            txtOutstandingLoanAmount.Text = myLoan.Out_Std_Loan + "";
            txtTermLoan.Text = myLoan.Term_Year + "";
            Helper.SelectedDropDownListIndex("VALUE", ddlLoanType, myLoan.Loan_Type + "");
        }
       

    }

    //Load Rider
    private void LoadRider(string appID)
    {
        double discountAmount = 0.0;
        double riderPremium = 0.0;
        double amountAfterDiscount = 0.0;
        double totalRider = 0.0;
        List<da_application_fp6.bl_app_rider> myRider = new List<da_application_fp6.bl_app_rider>();
        myRider = da_application_fp6.GetAppRiderList(appID);
        if (myRider.Count > 0)
        {
            DataTable tblRider = (DataTable)ViewState["tblRider"];
            tblRider.Clear();


            for (int i = 0; i < myRider.Count; i++)
            {
                var rider = myRider[i];
                DataRow row = tblRider.NewRow();
                row["rider_id"] = rider.Rider_ID;
                row["app_register_id"] = rider.App_Register_ID;
                row["level"] = rider.Level;
                row["rider_type"] = rider.Rider_Type;
                row["sumInsured"] = rider.SumInsured;
                row["premium"] = rider.Premium;
                row["product_id"] = rider.Product_ID;
                row["discount"] = rider.Discount;
             
                row["rate"] = rider.Rate;
                row["rate_id"] = rider.Rate_ID;
                
                row["original_amount"] = rider.Original_Amount;
                row["rounded_amount"] = rider.Rounded_Amount;

                discountAmount = rider.Discount;
                riderPremium = riderPremium + rider.Premium;

                tblRider.Rows.Add(row);
            }

            gvRider.DataSource = tblRider;
            gvRider.DataBind();
            ViewState["tblRider"] = tblRider;
        }

        //total rider premium spouse + all kids + adb + tpd
        //get total rider spouse and kids from life product sub
        totalRider = GetTotalRiderSpouseKids(appID);
        totalRider = totalRider + riderPremium;
        amountAfterDiscount = totalRider - discountAmount;
        txtRiderTotalPreium.Text = totalRider + "";
        txtRiderDiscountAmount.Text = discountAmount + "";
        txtRiderTotalPremiumAfterDiscount.Text = amountAfterDiscount + "";
    }
    //load job history
    private void LoadJobHistory(string appID) {
       

        DataTable tblJobHistory = new DataTable();
        tblJobHistory = da_application_fp6.GetApplicationJobHistory(appID);

        ViewState["tblJobHistory"] = tblJobHistory;
        gvJobHistory.DataSource = tblJobHistory;
        gvJobHistory.DataBind();

       
    }
    
    //load premium
    private void LoadPremium(string appID) {
        //bind product list into ddl plan type
        ddlTypeInsurancePlan.Items.Clear();
        ddlTypeInsurancePlan.Items.Add(new ListItem(".", ""));
        List<da_application_fp6.ProductFP6>fp6Product = da_application_fp6.ProductFP6.GetProductFP6List();

        foreach(da_application_fp6.ProductFP6 pro in fp6Product)
        {
            
            bl_product pro_name = da_product.GetProductByProductID(pro.ProductID);
           
            ddlTypeInsurancePlan.Items.Add(new ListItem( pro_name.En_Title, pro.ProductID));
        }

        //Bind payment mode into ddl premium mode

        ddlPremiumMode.Items.Clear();
        
        List<da_application_fp6.PayMentMode> payMode = da_application_fp6.PayMentMode.GetPaymentModeList();
        foreach (da_application_fp6.PayMentMode mode in payMode)
        { 
            ddlPremiumMode.Items.Add(new ListItem(mode.Mode, mode.PayMentModeID+""));
        }

        List<da_application_fp6.bl_app_life_product_Sub> mylist = new List<da_application_fp6.bl_app_life_product_Sub>();
        mylist = da_application_fp6.GetApplicationPremium(appID);
        
        if (mylist.Count > 0) {
            Helper.SelectedDropDownListIndex("VALUE",ddlTypeInsurancePlan, mylist[0].Product_ID);
            txtTermInsurance.Text = mylist[0].Assure_Year + "";
            txtPaymentPeriod.Text = mylist[0].Pay_Year + "";
            Helper.SelectedDropDownListIndex("VALUE",ddlPremiumMode, mylist[0].Pay_Mode+"");
            txtPremiumAmount.Text = mylist[0].User_Premium + "";

            #region enable/disable insurance amount and pay mode
            string sub_pro = "";
            sub_pro = mylist[0].Product_ID.Substring(0, 3).ToUpper().Trim();
            if (sub_pro == "NFP")
            {
                txtInsuranceAmountRequired.Enabled = true;
                ddlPremiumMode.Enabled = true;
            }
            else if (sub_pro == "FPP")
            {
                txtInsuranceAmountRequired.Enabled = false;
                ddlPremiumMode.Enabled = false;
            }
            #endregion

            txtInsuranceAmountRequired.Text = Helper.FormatDec(Convert.ToDouble( mylist[0].System_Sum_Insure)) + "";

            //assign sum insure to rider
            txtRiderSumInsured.Text = txtInsuranceAmountRequired.Text;

            for (int i = 0; i < mylist.Count; i++) { 
                var k = mylist[i];
                if (k.Level == 1)
                {
                    txtAssureeAgeLife1.Text = k.Age_Insure + "";
                    //txtAnnualOriginalPremiumAmountSystemLife1.Text = k.System_Premium+"";
                    txtPremiumAmountSystemLife1.Text = k.System_Premium + "";
                    txtPremiumOriginalAmountLife1.Text = k.System_Premium + "";
                }
            }
        }

        //premium pay
        List<da_application_fp6.bl_app_prem_pay_fp6> premList = new List<da_application_fp6.bl_app_prem_pay_fp6>();
        premList = da_application_fp6.GetAppPremPay(appID);
        double totalPremium = 0.0;
        if (premList.Count > 0)
        {
            for (int k = 0; k < premList.Count; k++)
            {
                var prem = premList[k];
                
                if (prem.Level == 1)
                {
                    txtAnnualOriginalPremiumAmountSystemLife1.Text = prem.Original_Amount + "";
 
                    totalPremium = totalPremium + prem.Amount;
                }
                
               
            }
            txtTotalPremium.Text = totalPremium + "";
        }


      //discount
        List<bl_app_discount>myDis = new List<bl_app_discount>();
        myDis = da_application_fp6.GetApplicationDiscount(appID);
        if (myDis.Count > 0)
        {

            var i = myDis[0];
            txtInsurancePlanNote.Text = i.Created_Note;
           // txtTotalPremium.Text = i.Premium + "";
            txtDiscountAmount.Text = i.Discount_Amount + "";
            txtTotalPremiumAfterDiscount.Text = i.Total_Amount + "";

        }
    }

    //load benefit
    private void LoadBenefit(string appID) {

        //DataTable tbl = da_application_fp6.GetDataTable("SP_Get_App_Benefit_Item", appID);
        DataTable tbl = da_application_fp6.GetDataTable("SP_Get_App_Benefit_Item_By_App_Register_ID", appID);
        double totalPercentage = 0;
        if (tbl.Rows.Count > 0) {
            for (int i = 0; i < tbl.Rows.Count; i++) {
               totalPercentage +=Convert.ToDouble( tbl.Rows[i]["percentage"].ToString());
            }
         
            txtTotalSharedPercentage.Text = totalPercentage + "";
            ViewState["tblBenifit"] = tbl;
            gvBenifit.DataSource = tbl;
            gvBenifit.DataBind();
        }
    }

    //Load Body
    private void LoadBody(string appID) {

       
        DataTable tblBody = (DataTable)ViewState["tblBody"];
        tblBody.Clear();

        List<bl_app_info_body> myList; 
        //Life insure 1
        myList = new List<bl_app_info_body>();
        myList = da_application_fp6.GetAppInfoBody(appID);
        //check has record or not
        if (myList.Count > 0) 
        {
            var i = myList[0];
            //var row = tblBody.Rows[0];
            DataRow row;
            row = tblBody.NewRow();
            row["level"] = "1";
            row["app_register_id"] = i.App_Register_ID;
            row["weight"] = i.Weight;
            row["height"] = i.Height;
            row["is_weight_changed"] = i.Is_Weight_Changed;
            row["reason"] = i.Reason;
            row["id"] = i.App_Register_ID;

            tblBody.Rows.Add(row);
                      
        }

        //Policy owner 
        List<da_application_fp6.bl_app_info_body_sub>  myList1 = new List<da_application_fp6.bl_app_info_body_sub>();
        myList1 = da_application_fp6.GetAppInfoBodySub(appID);
        //check has record or not
        if (myList1.Count > 0)
        {
            for (int j = 0; j < myList1.Count; j++) 
            {
                var i = myList1[j];
                if (i.Level == 0)//filter only policy owner
                {

                    DataRow row;
                    row = tblBody.NewRow();
                    row["level"] = i.Level;
                    row["app_register_id"] = i.App_Register_ID;
                    row["weight"] = i.Weight;
                    row["height"] = i.Height;
                    row["is_weight_changed"] = i.Is_Weight_Changed;
                    row["reason"] = i.Reason;
                    row["id"] = i.Id;

                    tblBody.Rows.Add(row);
                }
                
            }
        }
        gvBody.DataSource = tblBody;
        gvBody.DataBind();

    }

    //Load Answers
    void LoadAnswers(string appID)
    {
        try
        {
            List<da_application_fp6.bl_app_answer_item_fp6> myList = new List<da_application_fp6.bl_app_answer_item_fp6>();
            myList = da_application_fp6.GetAppAnswerItem(appID);

            if (myList.Count > 0)
            {

                //loop questions in gridview
                int rowCount = GvQA.Rows.Count;

                foreach (GridViewRow gRow in GvQA.Rows)
                {
                    Label lblQuestionID = (Label)gRow.FindControl("lblQuestionIDLife1");

                    string questionID = lblQuestionID.Text.Trim();

                    RadioButtonList rbtnAnswer = (RadioButtonList)gRow.FindControl("rbtnlAnswerLife1");
                    RadioButtonList rbtnAnswer1 = (RadioButtonList)gRow.FindControl("rbtnlAnswerLife2");
                  

                    //loop answers from database
                    for (int i = 0; i < myList.Count; i++)
                    {
                        var answer = myList[i];

                        int answerID = Convert.ToInt32(answer.Answer);//from database

                        if (answer.Level == 0)
                        {
                            if (questionID == answer.Question_ID.Trim())
                            {
                                rbtnAnswer.SelectedIndex = answerID - 1;
                              
                            }
                        }

                        else if (answer.Level == 1)
                        {
                            if (lblQuestionID.Text.Trim() == answer.Question_ID.Trim())
                            {
                                // rbtnAnswer1.SelectedIndex = answerID - 1;
                                Helper.SelectedRadioListIndex(rbtnAnswer1, answerID + "");
                            }
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Load answer Error:"+ex.Message);
        }
        
    }

    //call all load data
    private void LoadData(string appID) {
        try
        {
          //check format application form
           //DataTable tbl =  DataSetGenerator.Get_Data_Soure("select Product_ID from Ct_App_Life_Product where App_Register_ID = '" + appID.Trim().ToString() + "' AND Product_ID like '%NFP%'");
            DataTable tbl = DataSetGenerator.Get_Data_Soure("select Product_ID from Ct_App_Life_Product where App_Register_ID = '" + appID.Trim().ToString() + "'");
           if (tbl.Rows.Count > 0)
           {
               string product_id = "";
               product_id = tbl.Rows[0]["Product_ID"].ToString().Trim().Substring(0, 3).ToUpper();
               if (product_id != "NFP" && product_id != "FPP" && product_id != "SDS")
            {
                lblMessageApplication.Text = "Application is not correct format.";
                return;
            }
           
           }
           else
           {
               lblMessageApplication.Text = "Application is not correct format.";
               return;
           }
            //load application
 
            LoadApplication(appID);

            List<bl_channel_item> channel = new List<bl_channel_item>();

            channel = da_channel.GetChannelItemListByChannel(ddlChannel.SelectedValue);
            ddlCompany.Items.Clear();
            for (int j = 0; j < channel.Count; j++)
            {
                ddlCompany.Items.Add(new ListItem(channel[j].Channel_Name, channel[j].Channel_Item_ID));
            }

            //load application info
            LoadApplicationInfo(appID);

            //load person
            LoadPerson(appID);

            //load Address
            LoadAddress(appID);
            LoadAddressSub(appID);

            //load contact
            LoadContact(appID);
            LoadContactSub(appID);

            //Load loan
            loadLoan(appID);

            //load Job history
            LoadJobHistory(appID);

            //load premium
            LoadPremium(appID);

            //Load Rider
            LoadRider(appID);

            //load benefit
            LoadBenefit(appID);

            //Load Body
            LoadBody(appID);

            //Load Answers
            LoadAnswers(appID);

            

            //load underwriting status
            LoadUnderWritStatusCode(appID);
            //load Policy Number
            LoadPolicyNumber(appID);
            //enable and disable button
            if (txtUnderwritingStatus.Text.Trim() != "" || txtPolicyNumber.Text.Trim() != "")
            {
                if (txtUnderwritingStatus.Text.Trim() != "") {
                    lblMessageApplication.Text = "Application was underwritten already, system is not allow to update.";
                    //enable button update
                    EnableButtonsUpdate(false);
                    //enable button to update address and contact only
                    btnAddressUpdate.Visible = true;
                    btnSaveInsuredPlan.Visible = true;
                }

                if (txtPolicyNumber.Text.Trim() != "")
                {
                    lblMessageApplication.Text = "Application was issued already, system is not allow to update.";
                    //enable button update
                    EnableButtonsUpdate(false);
                    lbtnAddRider.Enabled = true;
                }
               
            }
            else
            {
                //enable button update
                EnableButtonsUpdate(true);
            }
            
        }
        catch (Exception ex)
        {
            //disable button update
           // EnableButtonsUpdate(false);
            Log.AddExceptionToLog("Error page [appliction_form_fp6.aspx.cs while loading data, Detail: " + ex.Message);
        }
        

    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {

            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            ViewState["userId"] = user_id;
            ViewState["userName"] = user_name;

            //POPUP();
            //return;

            txtDateEntry.Text = String.Format("{0:dd/MM/yyyy}", System.DateTime.Today);
            txtDataEntryBy.Text = user_name;

            //disable effective date
            txtEffectiveDate.Enabled = false;

            //hide loan
            dvLoan.Style.Add("display", "none");

            //Disable effective date
            //txtEffectiveDate.Enabled = false;

            //Application search modal

            ddlSearchOption.SelectedIndex = 0;
            //Application
            showApplication(true);
            //Customer
            showCustomer(false);
            //ID Type
            showIDType(false);

            //Marketing search
            divMarketing.Visible = false;
            btnOkMarketing.Visible = false;

            //Bind category dropdown list
            BindCategories(ddlClassRate);
            //Bind position dropdown list
            BindPosition("", ddlPosition);

            //generate personal data table for binding in grid view personal info
            generatePersonalTable();

            //generate benifit table
            generateBenifitTable();

            //Generate job history table
            generateJobHistoryTable();

            //Generate body table
            generateBodyTable();

            //Generate Rider table
            geneateRiderTable();

            //alert to confirm message
            imgSave.Attributes.Add("onclick", "return confirm('Do you want to save this application?');");
            btnSaveJob.Attributes.Add("onclick", "return confirm('Confirm update!');");
            btnUpdateBenefit.Attributes.Add("onclick", "return confirm('Confirm update!');");
            btnAddressUpdate.Attributes.Add("onclick", "return confirm('Confirm update!');");

            //
            ddlTypeInsurancePlan.Items.Clear();
            ddlTypeInsurancePlan.Items.Add(new ListItem(".", ""));
            List<da_application_fp6.ProductFP6> fp6Product = da_application_fp6.ProductFP6.GetProductFP6List();
            foreach (da_application_fp6.ProductFP6 pro in fp6Product)
            {
               
                //get product name by product id
                bl_product pro_name = da_product.GetProductByProductID(pro.ProductID);
               
                ddlTypeInsurancePlan.Items.Add(new ListItem(pro_name.En_Title, pro.ProductID));
               
            }

            //ddlTypeInsurancePlan.Items.Add(new ListItem("A", "A"));
            //ddlTypeInsurancePlan.Items.Add(new ListItem("B", "B"));
            //ddlTypeInsurancePlan.Items.Add(new ListItem("C", "A"));
            //ddlTypeInsurancePlan.Items.Add(new ListItem("D", "A"));

            //ddl paymode
            ddlPremiumMode.Items.Add(new ListItem(".", ""));

            //disable buttons update
            EnableButtonsUpdate(false);

            //DisableControls(Page, false);

            List<da_application_fp6.Channel> myChannel = new List<da_application_fp6.Channel>();
            myChannel = da_application_fp6.GetChannelList();
            ddlChannel.Items.Clear();
            ddlChannel.Items.Add(new ListItem(".", ""));
            for (int i = 0; i < myChannel.Count; i++)
            {

                ddlChannel.Items.Add(new ListItem(myChannel[i].Type, myChannel[i].ChannelID));
            }

            string appID = "";
            appID = Request.Params["application_register_id"];

            if(String.IsNullOrEmpty(appID))
            { 
                appID="";
            }

            if (appID != "" && appID != null)
            {
                ViewState["appID"] = appID;
                LoadData(appID);
            }
        
        }
    }

 
    //button ok on search application form
    protected void btnOk_Click(object sender, EventArgs e)
    {
       
        //Call function Load Data for view exist data
        try
        {
            //Clear form 
            ClearForm();
            txtApplicationNumber.Text = ViewState["appNumber"] + "";
            string appId = ViewState["appID"] + "";

            //fill applcation number to cancel modal
            txtCancelApplicationNumber.Text = ViewState["appNumber"] + ""; 

            //fill data in form
            LoadData(appId);

            mApplicationSearch.Hide();
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error while call function btnOk_Click in page application_fp6.aspx.cs, Detail: " + ex.Message);
        }
        
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
       // mApplicationSearch.Show();
        txtApplicationNumberSearch1.Text = "";
        
    }
    //search button
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
        mApplicationSearch.Show();

        gvApplication.DataSource = null;
        gvApplication.DataBind();

        int index = ddlSearchOption.SelectedIndex;
        if (index == 0) { //search application number
            if (txtApplicationNumberSearch1.Text.Trim() != "") {
                string appNumber = txtApplicationNumberSearch1.Text.Trim();
                List<bl_app_search> applications = new List<bl_app_search>();
                applications = da_application.GetApplicationByAppNo(appNumber);

                gvApplication.DataSource = applications;
                gvApplication.DataBind();

            }
            
        
        }
        else if (index == 1) { //search by customer name
            if (txtCustomerFirstNameSearch.Text.Trim() != "" || txtCustomerLastNameSearch.Text.Trim() != "") {

                List<bl_app_search> applications = new List<bl_app_search>();
                applications = da_application.GetApplicationByCustomerName(txtCustomerLastNameSearch.Text.Trim(), txtCustomerFirstNameSearch.Text.Trim());
                gvApplication.DataSource = applications;
                gvApplication.DataBind();
            }
           
        
        }
        else if (index == 2) { //search by id type

            List<bl_app_search> applications = new List<bl_app_search>();
            applications = da_application_fp6.GetApplicationByIDCard(Convert.ToInt16(ddlIDTypeSearch1.SelectedValue.Trim()), txtIDNoSearch.Text.Trim());
            gvApplication.DataSource = applications;
            gvApplication.DataBind();

        
        }
    }

    protected void gvApplication_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        mApplicationSearch.Show();

        GridViewRow row = gvApplication.Rows[e.NewSelectedIndex];

        Label lbl = (Label)row.FindControl("lblApplicationID");
        Label lbl1 = (Label)row.FindControl("lblApplicationNumber");

        ViewState["appID"] = lbl.Text.Trim();
        ViewState["appNumber"] = lbl1.Text.Trim();

    }
    //search option in application search modal
    protected void ddlSearchOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = ddlSearchOption.SelectedIndex;
        if (index == 0)
        {
            showApplication(true);
            showCustomer(false);
            showIDType(false);
        }
        else if(index==1) {
            showCustomer(true);
            showApplication(false);
            showIDType(false);
        }
        else if (index == 2) {
            showIDType(true);
            showApplication(false);
            showCustomer(false);
        }
        mApplicationSearch.Show();

        gvApplication.DataSource = null;
        gvApplication.DataBind();

    }
    #region Personal information
    //sub add personal information in to data table
    private void generatePersonalTable()
    {
        //variable
        DataTable tblPersonal = new DataTable();
        var a = tblPersonal.Columns;
        //a.Add("ownerType");
        a.Add("level");
        a.Add("idType");
        a.Add("idNumber");
        a.Add("surKhName");
        a.Add("firstKhName");
        a.Add("surEnName");
        a.Add("firstEnName");
        a.Add("fatherSurName");
        a.Add("fatherFirstName");
        a.Add("motherSurName");
        a.Add("motherFirstName");
        a.Add("previousSurName");
        a.Add("previousFirstName");
        a.Add("nationality");
        a.Add("gender");
       // a.Add("genderText");
        a.Add("dob");
        a.Add("id");
        a.Add("Marital_Status");
        a.Add("Relationship");
        ViewState["tblPersonal"] = tblPersonal;
    }
    //clear control in personal information
    private void ClearPerson() {

        ddlPolicyOwner.SelectedIndex = 0;
        ddlIDTypeLife1.SelectedIndex = 0;
        txtIDNumberLife1.Text = "";
        txtSurnameEngLife1.Text = "";
        txtFirstNameEngLife1.Text = "";
        txtSurnameKhLife1.Text = "";
        txtFirstNameKhLife1.Text = "";
       
        ddlNationalityLife1.SelectedIndex = 0;
        ddlGenderLife1.SelectedIndex = 0;
        txtDateBirthLife1.Text = "";

        lblMessagePersonalInfo.Text = "";

        //ckbPolicyowner.Checked = false;

        btnPersonalAdd.Text = "Add";
        ViewState["tblPersonalRowIndex"]= null;
    }

    protected void btnClearPersonInfo_Click(object sender, EventArgs e)
    {
        ClearPerson();
    }

    protected void btnPersonalAdd_Click(object sender, EventArgs e)
    {
        DataTable tblPersonal =(DataTable)ViewState["tblPersonal"];
        DataRow row;
       
        //varlidat required fields

        if (txtApplicationNumber.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "Application number is required.";
            return;
        }

        if (txtDateEntry.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "Date of Entry is required.";
            return;

        }

        if (txtIDNumberLife1.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "I.D NO is required.";
            return;
        }
        else if (txtSurnameEngLife1.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "Surname is required.";
            return;
        }
        else if (txtFirstNameEngLife1.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "First name is required.";
            return;
        }
        else if (ddlNationalityLife1.SelectedIndex == 0)
        {

            lblMessagePersonalInfo.Text = "Nationality is required.";
            return;
        }
        else if (txtDateBirthLife1.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "Date of Birth is required.";
            return;

        }

        
        //Check for selecting person from grid view
        if (ViewState["tblPersonalRowIndex"] == null)
        { //save

            try
            {
                //check owner 
                //int count = gvPersonalInfo.Rows.Count;
                int count = tblPersonal.Rows.Count;
                int level = 0;

                if (ddlPolicyOwner.SelectedIndex == 0)
                { //policy owner
                    if (count > 0)
                    {
                        int limit = 0;
                        for (int i = 0; i < count; i++)
                        {

                            var tr = tblPersonal.Rows[i];
                            //Check age
                            int age = Calculation.Culculate_Customer_Age(txtDateBirthLife1.Text.Trim(), Helper.FormatDateTime(txtDateEntry.Text.Trim()));
                            if(!CheckVarlidAge(age,0))
                            {
                                lblMessagePersonalInfo.Text="Age is over. Age must be in rage [18 - 55].";
                                return;
                            }

                            if (tr["level"].ToString().Trim() == "0")
                            {
                                limit += 1;

                            }
                        }
                        if (limit == 1)
                        {
                            //not allow input policy owner more than one
                            lblMessagePersonalInfo.Text = "Owner is already exist.";
                            return;
                        }
                    }

                }
                else if (ddlPolicyOwner.SelectedIndex == 1)
                { //life insured
                    if (count > 0)
                    {
                        int limit = 0;
                        for (int i = 0; i < count; i++)
                        {

                           // Label lblOwner = (Label)gvPersonalInfo.Rows[i].FindControl("lblOwnerType");
                            var tr = tblPersonal.Rows[i];
                          

                            if (tr["level"].ToString().Trim() == "1")
                            {
                                limit += 1;
                            }
                        }
                        if (limit == 1)
                        {
                            //not allow input policy owner more than 5
                            lblMessagePersonalInfo.Text = "Life is already exist.";
                            return;
                        }

                        /*
                         * 
                         * int rowCount = tblPersonal.Rows.Count;
                        
                        level = rowCount ;
                        if (level == 1)
                        {

                            //Check age
                            int age = Calculation.Culculate_Customer_Age(txtDateBirthLife1.Text.Trim() , Helper.FormatDateTime(txtDateEntry.Text.Trim()));
                            if (!CheckVarlidAge(age, level))
                            {
                                lblMessagePersonalInfo.Text = "Age is over. Age must be in rage [18 - 55].";
                                return;
                            }
                        }
                        else if(level>1)
                        {
                            //Check age
                            int age = Calculation.Culculate_Customer_Age(txtDateBirthLife1.Text.Trim() , Helper.FormatDateTime(txtDateEntry.Text.Trim()));

                            if (!CheckVarlidAge(age, level))
                            {
                                lblMessagePersonalInfo.Text = "Age is over. Age must be in rage [1 - 21].";
                                return;
                            }
                        
                        }
                         */
                       
                    }
                }

                //if policyowner is the life insured auto insert two records
                if (ckbPolicyowner.Checked)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        //initialize value to data row
                        level = i;
                        row = tblPersonal.NewRow();
                        //row["ownerType"] = ddlPolicyOwner.SelectedValue.Trim();
                        row["level"] = level;
                        row["idType"] = ddlIDTypeLife1.SelectedValue.Trim();
                        row["idNumber"] = txtIDNumberLife1.Text.Trim();
                        row["surKhName"] = txtSurnameKhLife1.Text.Trim();
                        row["firstKhName"] = txtFirstNameKhLife1.Text.Trim();
                        row["surEnName"] = txtSurnameEngLife1.Text.Trim().ToUpper();
                        row["firstEnName"] = txtFirstNameEngLife1.Text.Trim().ToUpper();

                        row["nationality"] = ddlNationalityLife1.SelectedValue.Trim();
                        row["gender"] = ddlGenderLife1.SelectedItem.Text;// ddlGenderLife1.SelectedValue.Trim();

                        row["dob"] = txtDateBirthLife1.Text.Trim();
                        row["id"] = "";
                        row["Marital_Status"] = ddlMaritalStatus.SelectedValue;


                        if (ddlRelationShip.SelectedValue.Trim().ToLower() == "others")
                        {
                            if (txtOthers.Text.Trim() != "")
                            {
                                row["Relationship"] = txtOthers.Text.Trim();
                            }
                            else
                            {
                                row["Relationship"] = "";
                            }

                        }
                        else
                        {
                            row["Relationship"] = ddlRelationShip.SelectedValue;
                        }
                        //add row into datatable
                        tblPersonal.Rows.Add(row);
                    }
                    
                }
                else 
                {
                    //initialize value to data row
                    level = Convert.ToInt32(ddlPolicyOwner.SelectedValue);
                    row = tblPersonal.NewRow();
                    //row["ownerType"] = ddlPolicyOwner.SelectedValue.Trim();
                    row["level"] = level;
                    row["idType"] = ddlIDTypeLife1.SelectedValue.Trim();
                    row["idNumber"] = txtIDNumberLife1.Text.Trim();
                    row["surKhName"] = txtSurnameKhLife1.Text.Trim();
                    row["firstKhName"] = txtFirstNameKhLife1.Text.Trim();
                    row["surEnName"] = txtSurnameEngLife1.Text.Trim().ToUpper();
                    row["firstEnName"] = txtFirstNameEngLife1.Text.Trim().ToUpper();

                    row["nationality"] = ddlNationalityLife1.SelectedValue.Trim();
                    row["gender"] = ddlGenderLife1.SelectedItem.Text;// ddlGenderLife1.SelectedValue.Trim();

                    row["dob"] = txtDateBirthLife1.Text.Trim();
                    row["id"] = "";
                    row["Marital_Status"] = ddlMaritalStatus.SelectedValue;


                    if (ddlRelationShip.SelectedValue.Trim().ToLower() == "others")
                    {
                        if (txtOthers.Text.Trim() != "")
                        {
                            row["Relationship"] = txtOthers.Text.Trim();
                        }
                        else
                        {
                            row["Relationship"] = "";
                        }

                    }
                    else
                    {
                        row["Relationship"] = ddlRelationShip.SelectedValue;
                    }
                    //add row into datatable
                    tblPersonal.Rows.Add(row);
                }
               

                gvPersonalInfo.DataSource = tblPersonal;
                gvPersonalInfo.DataBind();

                //if (gvPersonalInfo.Rows.Count > 0)
                //{
                //    //call calculation premium
                //    CalculatePremium();
                //}

                //clear text control
                ClearPerson();

                //show all people in dropdownlist
                BindPersonInDropdownList();
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add Personal information. Detail: " + ex.Message);

                lblMessagePersonalInfo.Text = "Added fail, please contact your system administrator.";

            }

        }
        else { //update data in datatable

            try
            {
                int rowIndex = Convert.ToInt16( ViewState["tblPersonalRowIndex"]);

                //initialize value to data row
                tblPersonal.Rows[rowIndex]["idType"] = ddlIDTypeLife1.SelectedValue.Trim();
                tblPersonal.Rows[rowIndex]["idNumber"] = txtIDNumberLife1.Text.Trim();
                tblPersonal.Rows[rowIndex]["surKhName"] = txtSurnameKhLife1.Text.Trim();
                tblPersonal.Rows[rowIndex]["firstKhName"] = txtFirstNameKhLife1.Text.Trim();
                tblPersonal.Rows[rowIndex]["surEnName"] = txtSurnameEngLife1.Text.Trim().ToUpper();
                tblPersonal.Rows[rowIndex]["firstEnName"] = txtFirstNameEngLife1.Text.Trim().ToUpper();
               
                tblPersonal.Rows[rowIndex]["nationality"] = ddlNationalityLife1.SelectedValue.Trim();
                tblPersonal.Rows[rowIndex]["gender"] = ddlGenderLife1.SelectedItem.Text;// ddlGenderLife1.SelectedValue.Trim();
               
                tblPersonal.Rows[rowIndex]["dob"] = txtDateBirthLife1.Text.Trim();
                //tblPersonal.Rows[rowIndex]["id"] = 0;

                tblPersonal.Rows[rowIndex]["Marital_Status"] = ddlMaritalStatus.SelectedValue;
                //tblPersonal.Rows[rowIndex]["Relationship"] = ddlRelationShip.SelectedValue;

                if (ddlRelationShip.SelectedValue.Trim().ToLower() == "others")
                {
                    if (txtOthers.Text.Trim() != "")
                    {
                        tblPersonal.Rows[rowIndex]["Relationship"] = txtOthers.Text.Trim();
                    }
                    else
                    {
                        tblPersonal.Rows[rowIndex]["Relationship"] = "";
                    }

                }
                else
                {
                    tblPersonal.Rows[rowIndex]["Relationship"] = ddlRelationShip.SelectedValue;
                }


                gvPersonalInfo.DataSource = tblPersonal;
                gvPersonalInfo.DataBind();

                //if (gvPersonalInfo.Rows.Count > 0)
                //{
                //    //call calculation premium
                //    CalculatePremium();
                //}

                //clear text control
                ClearPerson();

                //add person list into dropdown list
                /*
                ddlJobPolicyOwner.Items.Clear();
                ddlJobPolicyOwner.Items.Add(new ListItem(".", ""));
                ddlBodyPerson.Items.Clear();
                ddlBodyPerson.Items.Add(new ListItem(".", ""));
                for (int i = 0; i < tblPersonal.Rows.Count;i++ )
                {
                    ddlJobPolicyOwner.Items.Add(new ListItem(tblPersonal.Rows[i]["surEnName"].ToString() + " " + tblPersonal.Rows[i]["firstEnName"].ToString(), tblPersonal.Rows[i]["level"].ToString()));
                    ddlBodyPerson.Items.Add(new ListItem(tblPersonal.Rows[i]["surEnName"].ToString() + " " + tblPersonal.Rows[i]["firstEnName"].ToString(), tblPersonal.Rows[i]["level"].ToString()));
                
                }*/

                //show all people in dropdownlist
                BindPersonInDropdownList();
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add Personal information. Detail: " + ex.Message);

                lblMessagePersonalInfo.Text = "Update fail, please contact your system administrator.";

            }
        
        }
    }
  
    protected void gvPersonalInfo_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];

        //check data in data table has or not?
        if (tblPersonal.Rows.Count > 0) {

            int rowIndex = e.NewSelectedIndex;
            ViewState["tblPersonalRowIndex"] = rowIndex;

            //change add button to Update
            btnPersonalAdd.Text = "Update";

            ddlPolicyOwner.SelectedIndex = Convert.ToInt16(tblPersonal.Rows[rowIndex]["level"].ToString());// Convert.ToInt16(tblPersonal.Rows[rowIndex]["ownerType"].ToString());
            Helper.SelectedDropDownListIndex("VALUE", ddlIDTypeLife1, tblPersonal.Rows[rowIndex]["idType"].ToString());
            txtIDNumberLife1.Text = tblPersonal.Rows[rowIndex]["idNumber"].ToString();

            txtFirstNameKhLife1.Text = tblPersonal.Rows[rowIndex]["firstKhName"].ToString();
            txtSurnameKhLife1.Text = tblPersonal.Rows[rowIndex]["surKhName"].ToString();


            Helper.SelectedDropDownListIndex("VALUE", ddlNationalityLife1, tblPersonal.Rows[rowIndex]["nationality"].ToString());
            int gender = -1;
            if (tblPersonal.Rows[rowIndex]["gender"].ToString() == "Male")
            {
                gender = 1;
            }
            else 
            {
                gender = 0;
            }
            Helper.SelectedDropDownListIndex("VALUE", ddlGenderLife1, gender+"");

            txtDateBirthLife1.Text = tblPersonal.Rows[rowIndex]["dob"].ToString();

            txtFirstNameEngLife1.Text = tblPersonal.Rows[rowIndex]["firstEnName"].ToString();
            txtSurnameEngLife1.Text = tblPersonal.Rows[rowIndex]["surEnName"].ToString();

           
            string relationship = "";
            relationship = tblPersonal.Rows[rowIndex]["Relationship"].ToString();

            if (relationship != "wife" && relationship !="husband" && relationship !="")
            {
                Helper.SelectedDropDownListIndex("VALUE", ddlRelationShip, "OTHERS");
                txtOthers.Visible = true;
                txtOthers.Text = tblPersonal.Rows[rowIndex]["Relationship"].ToString();
            }
            else
            {
                Helper.SelectedDropDownListIndex("VALUE", ddlRelationShip, tblPersonal.Rows[rowIndex]["Relationship"].ToString());
                txtOthers.Visible = false;
            }

            Helper.SelectedDropDownListIndex("VALUE", ddlMaritalStatus,tblPersonal.Rows[rowIndex]["Marital_Status"].ToString());

        }

    }
    #endregion


    #region Insurance Plan
    protected void txtInsuranceAmountRequired_TextChanged(object sender, EventArgs e)
    {
        //calculate premium
      //  CalculatePremium();
        if (txtInsuranceAmountRequired.Text.Trim() != "")
        {
            txtInsuranceAmountRequired.Text = Helper.FormatDec(Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim())) + "";
            txtRiderSumInsured.Text = Helper.FormatDec(Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim())) + "";
        }
        
    }
   
    protected void txtDiscountAmount_TextChanged(object sender, EventArgs e)
    {
        //calculate premium
      //  CalculatePremium();
        CalcDiscount();

    }
    protected void imgRecalculation_Click(object sender, ImageClickEventArgs e)
    {
        
        string plan ="";
       plan=ddlTypeInsurancePlan.SelectedValue.Trim().Substring(0,3);

       if (plan.ToUpper() == "NFP")//family protection
       {
           CalculatePremium();
       }
       else if(plan.ToUpper()=="FPP")//family protection package
       {
           CalculatePremiumPackage();
       }
       else if (plan.ToUpper() == "SDS")//study save
       {
           CalculatePremiumStudySave(ddlTypeInsurancePlan.SelectedValue.Trim());
       }
    }
    #endregion

    #region Contact
    protected void ddlCountryLife2_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtZipCodeLife2.Text = da_application.GetZipCode(ddlCountryLife2.SelectedValue.Trim());
    }
    protected void ddlCountryLife1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtZipCodeLife1.Text = da_application.GetZipCode(ddlCountryLife1.SelectedValue.Trim());
    }
    #endregion


    #region Address

    #endregion

    #region Benifit
    //click add benifit
    protected void btnAddBenfit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //check validate required field
            int count = gvBenifit.Rows.Count;
            int totalShared = 0;

            //check for inputing application number
            if (txtApplicationNumber.Text.Trim() == "") {

                lblMessageBenifit.Text = "Application Number is riquired.";
                //exit function
                return;

            }

            //if (txtBenefitSharePercentage.Text.Trim() != "" && txtBenefitSharePercentage.Text.Trim() != "0" && txtBenefitIDNo.Text.Trim() != "" && txtBenefitName.Text.Trim() != "")
            if (txtBenefitSharePercentage.Text.Trim() != "" && txtBenefitSharePercentage.Text.Trim() != "" && txtBenefitIDNo.Text.Trim() != "" && txtBenefitName.Text.Trim() != "")
            {

                if (Convert.ToDouble(txtBenefitSharePercentage.Text.Trim()) <= 100)
                {

                    for (int i = 0; i < count; i++)
                    {
                        //sum percentage
                        GridViewRow r = gvBenifit.Rows[i];
                        Label lb = (Label)r.FindControl("lblPercentage");

                        totalShared += Convert.ToInt16(lb.Text.Trim());

                    }

                    int pre_persent = 0;
                    if (ViewState["tblBenifitRowIndex"] != null) 
                    {
                        GridViewRow r = gvBenifit.Rows[Convert.ToInt32(ViewState["tblBenifitRowIndex"])];
                        Label lb = (Label)r.FindControl("lblPercentage");
                        pre_persent = Convert.ToInt16(lb.Text.Trim());
                    }
                    totalShared = totalShared - pre_persent;

                    if (totalShared + Convert.ToDouble(txtBenefitSharePercentage.Text.Trim()) <= 100)
                    {
                        DataTable tblBenifit = (DataTable)ViewState["tblBenifit"];
                       
                        if (ViewState["tblBenifitRowIndex"] == null)
                        { //add new
                            DataRow row;
                            row = tblBenifit.NewRow();
                            row["id_Type"] = ddlBenefitIDType.SelectedValue.Trim();
                            row["id_card"] = txtBenefitIDNo.Text.Trim();
                            row["full_Name"] = txtBenefitName.Text.Trim().ToUpper();
                            row["relationship"] = ddlBenefitRelation.SelectedValue.Trim();
                            row["percentage"] = txtBenefitSharePercentage.Text.Trim();
                            row["remarks"] = txtBenefitRemarks.Text.Trim();
                            row["app_benefit_item_id"] = "";
                            tblBenifit.Rows.Add(row);

                            gvBenifit.DataSource = tblBenifit;
                            gvBenifit.DataBind();

                            //hid error message
                            lblMessageBenifit.Text = "";

                            //show total shared (%) in text box
                            txtTotalSharedPercentage.Text = totalShared + Convert.ToInt16(txtBenefitSharePercentage.Text.Trim()) + "";

                        }
                        else//update
                        {
                            int rowIndex = Convert.ToInt16(ViewState["tblBenifitRowIndex"] + "");

                            //lblMessageBenifit.Text ="Index="+ ViewState["tblBenifitRowIndex"] + "";
                            tblBenifit.Rows[rowIndex]["id_Type"] = ddlBenefitIDType.SelectedValue.Trim();
                            tblBenifit.Rows[rowIndex]["id_card"] = txtBenefitIDNo.Text.Trim();
                            tblBenifit.Rows[rowIndex]["full_Name"] = txtBenefitName.Text.Trim().ToUpper();
                            tblBenifit.Rows[rowIndex]["relationship"] = ddlBenefitRelation.SelectedValue.Trim();
                            tblBenifit.Rows[rowIndex]["percentage"] = txtBenefitSharePercentage.Text.Trim();
                            tblBenifit.Rows[rowIndex]["remarks"] = txtBenefitRemarks.Text.Trim();
                            //tblBenifit.Rows[rowIndex]["app_benefit_item_id"] = "0";

                            gvBenifit.DataSource = tblBenifit;
                            gvBenifit.DataBind();

                            //hid error message
                            lblMessageBenifit.Text = "";

                            //show total shared (%) in text box
                            int totalShared1 = 0;
                            for (int i = 0; i < count; i++)
                            {

                                GridViewRow r = gvBenifit.Rows[i];
                                Label lb = (Label)r.FindControl("lblPercentage");

                                totalShared1 += Convert.ToInt16(lb.Text.Trim());

                            }
                            txtTotalSharedPercentage.Text = totalShared1  + "";
                            ViewState["tblBenifitRowIndex"] = null;
                            gvBenifit.SelectedIndex = -1;

                        }

                      
                    }
                    else
                    {
                        //show message to user
                        lblMessageBenifit.Text = "Maximum total shared is 100%.";
                        return;

                    }

                }
                else
                {//use input grather than 100
                    //show message to user
                    lblMessageBenifit.Text = "Maximum total shared is 100%.";
                    return;
                }
            }
            else
            { //must input required fields
                //show message to user
                lblMessageBenifit.Text = "Required fields [I.D No, Surname and Name, Share (%)].";

            }

            ClearBenefitTextBoxes();
           
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error add benifit, Detail: "+ex.Message);
        
        }

    }

    protected void gvBenifit_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataTable tbl = (DataTable)ViewState["tblBenifit"];
            if (tbl.Rows.Count > 0) {

                int rowIndex = e.NewSelectedIndex;
                ViewState["tblBenifitRowIndex"] = rowIndex;

                var row = tbl.Rows[rowIndex];

                txtBenefitIDNo.Text = row["id_card"].ToString();
                Helper.SelectedDropDownListIndex("VALUE", ddlBenefitIDType, row["id_type"].ToString());
                Helper.SelectedDropDownListIndex("VALUE", ddlBenefitRelation, row["relationship"].ToString());
                txtBenefitName.Text = row["full_name"].ToString();
                txtBenefitSharePercentage.Text = row["percentage"].ToString();
                txtBenefitRemarks.Text = row["remarks"].ToString();                
                //lblMessageBenifit.Text = row["percentage"].ToString();
            
            }

        }
        catch (Exception ex)
        {
            lblMessageBenifit.Text = "Error, please contact your system administrator.";
            Log.AddExceptionToLog("Error select Benefit Item in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }

    }
    protected void gvBenifit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblBenifit = (DataTable)ViewState["tblBenifit"];
            int rowIndex = e.RowIndex;
            Label lblPercentage = (Label)gvBenifit.Rows[rowIndex].FindControl("lblPercentage");

            int percentage = Convert.ToInt32(lblPercentage.Text.Trim());
            string id = "";
            id = tblBenifit.Rows[rowIndex]["app_benefit_item_id"].ToString().Trim();
            if (id != "" && id != "0") {

                ViewState["app_benefit_item_id"] += id + ",";
            }

            tblBenifit.Rows[rowIndex].Delete();
            tblBenifit.AcceptChanges();

            txtTotalSharedPercentage.Text = Convert.ToInt32(txtTotalSharedPercentage.Text.Trim()) - percentage + "";

            ViewState["tblBenifit"] = tblBenifit;
            gvBenifit.DataSource = tblBenifit;
            gvBenifit.DataBind();


           // lblMessageBenifit.Text = "Row index=" + rowIndex + "";
        }
        catch (Exception ex) {

            Log.AddExceptionToLog("Delete benefit error: " + ex.Message);
        }
      
        
    }
    #endregion
    #region Health
    //select changed: weight changed 6 months for life 1
    protected void rblWeightChangeLife1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblWeightChangeLife1.SelectedIndex >= 1)
        {
            lblWeightChangeReasonLife1.Visible = true;
            txtWeightChangeReasonLife1.Visible = true;
        }
        else { //hide control

            lblWeightChangeReasonLife1.Visible = false;
            txtWeightChangeReasonLife1.Visible = false;
        }
    }
   

    #endregion

   
    #region Add New Application
   
    protected void btnOkApplicationNew_Click(object sender, EventArgs e)
    {
        mApplicationNew.Show();
        //clear form
        ClearForm();

        //check application format
        string strApplicationNumber="";
        string strApplicationNumberFormated="";
        strApplicationNumber=txtApplicationNumberNew.Text.Trim();


        if (strApplicationNumber.Trim() != "")
        {
            if (strApplicationNumber.Length == 9)
            {

            }
            else if (strApplicationNumber.Length < 9)
            {

                //clear message
                lblMessageApplicationNew.Text = "";

                //format application number
               strApplicationNumberFormated = da_application_fp6.FormatApplicationNumber(strApplicationNumber.Trim());

                //check exist application nubmer in database


               if (da_application.CheckAppNumber(strApplicationNumberFormated))
               { //exist
                   try
                   {
                       lblMessageApplicationNew.Text = "This application number is aleardy exist.";
                       mApplicationNew.Show();
                       return;
                      
                   }
                   catch (Exception ex) {
                       Log.AddExceptionToLog("Load exist data: " + ex.Message);
                   }
                   
               }
               else { //not exist

                   //pass application number into application form
                   txtApplicationNumber.Text = strApplicationNumberFormated;

                   txtDataEntryBy.Text = ViewState["userName"]+"";
                   txtDateEntry.Text = String.Format("{0:dd/MM/yyyy}", System.DateTime.Today);

                   //hide search modal
                   mApplicationNew.Hide();
               
               }

            }
            else
            {
                lblMessageApplicationNew.Text = "Application number is not valid.";
            }
        }
        else {
            lblMessageApplicationNew.Text = "Application number is required.";
        }
        
    }

   
    //Search marketing name in marketing search modal
    protected void btnSearchMarketingName_Click(object sender, EventArgs e)
    {
        string strAgentName = txtAgentName.Text.Trim();
        
        gvMarketing.DataSource = da_sale_agent.GetAgentList(strAgentName);
        gvMarketing.DataBind();
        if (gvMarketing.Rows.Count > 0)
        {
            divMarketing.Visible = true;
            btnOkMarketing.Visible = true;
        }
        else
        {
            divMarketing.Visible = false;
            btnOkMarketing.Visible = false;
        }

        mMarketingSearch.Show();

    }
    //Modal marketing search click ok
    protected void btnOkMarketing_Click(object sender, EventArgs e)
    {
        txtMarketingCode.Text = ViewState["agentCode"] + "";
        txtMarketingName.Text = ViewState["agentName"] + "";
        // mMarketingSearch.Hide();
    }
    protected void gvMarketing_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Trim() == "select")
        {

            txtAgentName.Text = "";

        }
    }
    //select record in search modal of marketing
    protected void gvMarketing_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

        GridViewRow row = gvMarketing.Rows[e.NewSelectedIndex];

        Label lbl = (Label)row.FindControl("lblAgentCode");
        Label lbl1 = (Label)row.FindControl("lblAgentName");

        ViewState["agentCode"] = lbl.Text.Trim();
        ViewState["agentName"] = lbl1.Text.Trim();
        mMarketingSearch.Show();

    }
    #endregion

    protected void imgSave_Click(object sender, ImageClickEventArgs e)
    {
        string appID = "";
        try
        {
            if (VarlidPage().Trim() != "")
            {
                lblMessageApplication.Text = VarlidPage().Trim();
                return;
            }

            if (txtApplicationNumber.Text.Trim() == "") 
            {
                lblMessageApplication.Text = "Please input all required fields.";
                return;
            }

            if(da_application.CheckAppNumber(txtApplicationNumber.Text.Trim()))
            {
                lblMessageApplication.Text = "Application number is already exist.";
                return;
            }
            else { //save
                if (insertApplaction())//first save application register
                {
                    appID = ViewState["appID"] + "";
                    //save application info
                    if (InsertAppInfo(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application information fail, please contact your system administrator.";
                        return;
                    }
                    //save personal info
                    if (insertPersonalInfo(appID))
                    {
                       lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved personal information fail, please contact your system administrator.";
                        return;
                    }

                    //save personal info sub
                    if (insertPersonalInfoSub(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved personal information [sub] fail, please contact your system administrator.";
                        return;
                    }

                    //save address
                    if (InsertAppInfoAddress(appID))
                    {

                       lblMessageApplication.Text="Saved success.";
                    }
                    else {
                        lblMessageApplication.Text="Saved application address fail, please contact your system administrator.";
                        return;
                    
                    }

                    //save address sub
                    if (InsertAppInfoAddressSub(appID))
                    {

                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application address [sub] fail, please contact your system administrator.";
                        return;

                    }

                    //Save contact
                    if (insertContact(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application contact fail, please contact your system administrator.";
                        return;
                    }
                    //save contact sub
                    if (insertContactSub(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                       lblMessageApplication.Text="Saved application contact [sub] fail, please contact your system administrator.";
                        return;
                    }
                    //save job history

                    if (InsertAppJobHistorySubList(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                       lblMessageApplication.Text="Saved application job history fail, please contact your system administrator.";
                        return;
                    }
                 
                    //save premium life product
                    if (InsertAppLifeProduct(appID))
                    {
                       lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application life product fail, please contact your system administrator.";
                        return;
                    }

                    
                    //save premium pay
                    if (InsertAppPremPay(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                       lblMessageApplication.Text="Saved application premium pay fail, please contact your system administrator.";
                        return;
                    }
                    //save discount
                    if (InsertAppDiscount(appID))
                    {
                       lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application discount fail, please contact your system administrator.";
                        return;
                    }

                    //save Rider
                    if (InsertAppRider(appID))
                    {
                        lblMessageApplication.Text = "Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text = "Saved application rider fail, please contact your system administrator.";
                        return;
                    }

                    //save loan plan
                    if (InsertAppLoan(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application loan fail, please contact your system administrator.";
                        return;
                    }
                    //save benifit
                    if (SaveBenefiter(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved beneficiaries fail, please contact your system administrator.";
                        return;
                    }
                    //save body
                    if (InsertAppInfoBody(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application information body fail, please contact your system administrator.";
                        return;
                    }
                  
                    //save answer
                    if (InsertAppAnswerItem(appID))
                    {
                        lblMessageApplication.Text="Saved success.";
                    }
                    else
                    {
                        lblMessageApplication.Text="Saved application answer fail, please contact your system administrator.";
                        return;
                    }

                    InsertAppStepHistory(appID);
                    InsertAppStepNext(appID);

                    //open rider application form
                    if (lblMessageApplication.Text == "Saved success.")
                    {
                        POPUP();
                    }

                }
            
            }
           
        }
        catch (Exception ex) {
            lblMessageApplication.Text = "Saved fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error click save button in page [application_form_fp6.aspx], Detail: " + ex.Message);
        }
       
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlChannel.SelectedIndex > 0) {

            List<bl_channel_item> channel = new List<bl_channel_item>();
            channel=da_channel.GetChannelItemListByChannel(ddlChannel.SelectedValue);
            ddlCompany.Items.Clear();
            for (int i = 0; i < channel.Count; i++) {

                ddlCompany.Items.Add(new ListItem(channel[i].Channel_Name, channel[i].Channel_Item_ID));
            }
        }
    }
    protected void ddlTypeInsurancePlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region old code
        /*
        if (ddlTypeInsurancePlan.SelectedValue.Trim() != "")
        {

            String[] a = ddlTypeInsurancePlan.SelectedValue.Split(',');
            if (a.Length > 0)
            {
                txtTermInsurance.Text = a[0];
                txtPaymentPeriod.Text = a[1];

            }

            // show product title after user select product plan
            bl_product pro = da_product.GetProductByProductID(ddlTypeInsurancePlan.SelectedItem.Text.Trim());
            lblProductTitle.Text = pro.En_Title;

            // auto select payment mode [only annual for family protection package]
            string substirng = "";
            substirng = ddlTypeInsurancePlan.SelectedItem.Text.Trim().Substring(0, 3);

            if (substirng.ToUpper() == "FPP")
            {
                Helper.SelectedDropDownListIndex("VALUE", ddlPremiumMode, "1"); //1 is annaul
                ddlPremiumMode.Enabled = false;
            }
            else
            {
                ddlPremiumMode.SelectedIndex = 0;
                ddlPremiumMode.Enabled = true;
            }

        }
        else
        {//reset
            txtTermInsurance.Text = "";
            txtPaymentPeriod.Text = "";
            lblProductTitle.Text = "";
        }
         * */
        #endregion

        #region new code
        if (ddlTypeInsurancePlan.SelectedIndex > 0)
        {
            string str_product_id = "";
            str_product_id = ddlTypeInsurancePlan.SelectedValue.Trim();
            // show product title after user select product plan
            //bl_product pro = da_product.GetProductByProductID(str_product_id);
            //lblProductTitle.Text = pro.En_Title;

            bl_product_life pro_life = da_product.GetProductLifeByProductID(str_product_id);
            txtTermInsurance.Text = pro_life.Assure_Year + "";
            txtPaymentPeriod.Text = pro_life.Pay_Year + "";

            // auto select payment mode [only annual for family protection package]
            string substirng = "";
            substirng = str_product_id.Substring(0, 3);

            if (substirng.ToUpper() == "FPP")
            {
                Helper.SelectedDropDownListIndex("VALUE", ddlPremiumMode, "1"); //1 is annaul
                ddlPremiumMode.Enabled = false;
                
            }
            else
            {
                ddlPremiumMode.SelectedIndex = 0;
                ddlPremiumMode.Enabled = true;
                //CalculatePremiumPackage();

                //auto input sum assure
                string[] sub_product_id = str_product_id.Split('/');
                if (sub_product_id.Length > 2)//study save package , maturity and protection
                {
                    txtInsuranceAmountRequired.Text = Helper.FormatDec(Convert.ToDouble( sub_product_id[2].ToString()));
                }
            }

           
            
        }
        else
        {//reset
            txtTermInsurance.Text = "";
            txtPaymentPeriod.Text = "";
            lblProductTitle.Text = "";
        }
        #endregion

    }

    protected void gvPersonalInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable tbl = (DataTable)ViewState["tblPersonal"];
        int rowIndex = e.RowIndex;

        string id = "";
        id = tbl.Rows[rowIndex]["id"].ToString();

        if (id != "0" & id != "") {// store id for deleting from database
            ViewState["personID"] += id+",";
        }
        
        tbl.Rows[rowIndex].Delete();
        tbl.AcceptChanges();

       // lblMessageApplication.Text ="Row index="+ rowIndex;
        
        ViewState["tblPersonal"] = tbl;
        gvPersonalInfo.DataSource = tbl;
        gvPersonalInfo.DataBind();

       // CalculatePremium();    
        

    }

    //update person
    protected void btnUpdatePerson_Click(object sender, EventArgs e)
    {
        //update and delete some rows of person
        DataTable tbl = (DataTable)ViewState["tblPersonal"];
        if (tbl.Rows.Count > 0)
        {
            for (int i = 0; i < tbl.Rows.Count; i++) {
                var row = tbl.Rows[i];
                int gender = -1;
                if (row["gender"].ToString().Trim() == "Male")
                {
                    gender = 1;
                }
                else
                {
                    gender = 0;
                }
                if (row["level"].ToString().Trim() == "1")
                { //Life insured
                    
                    da_application_fp6.bl_app_info_person_fp6 app_info_person = new da_application_fp6.bl_app_info_person_fp6();
                    app_info_person.App_Register_ID = ViewState["appID"] + "";
                    app_info_person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString().Trim());

                    app_info_person.Country_ID = row["nationality"].ToString().Trim();
                    app_info_person.Gender = gender;
                    app_info_person.ID_Card = row["idNumber"].ToString().Trim();
                    app_info_person.ID_Type = Convert.ToInt32(row["idType"].ToString().Trim());
                    app_info_person.First_Name = row["firstEnName"].ToString().Trim();
                    app_info_person.Last_Name = row["surEnName"].ToString().Trim();

                    //Check for null
                    if (row["fatherFirstName"].ToString().Trim() == "")
                    {
                        app_info_person.Father_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Father_First_Name = row["fatherFirstName"].ToString().Trim();
                    }

                    if (row["fatherSurName"].ToString().Trim() == "")
                    {
                        app_info_person.Father_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Father_Last_Name = row["fatherSurName"].ToString().Trim();
                    }

                    if (row["firstKhName"].ToString().Trim() == "")
                    {
                        app_info_person.Khmer_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Khmer_First_Name = row["firstKhName"].ToString().Trim();
                    }

                    if (row["surKhName"].ToString().Trim() == "")
                    {
                        app_info_person.Khmer_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Khmer_Last_Name = row["surKhName"].ToString().Trim();

                    }

                    if (row["motherFirstName"].ToString().Trim() == "")
                    {
                        app_info_person.Mother_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Mother_First_Name = row["motherFirstName"].ToString().Trim();
                    }

                    if (row["motherSurName"].ToString().Trim() == "")
                    {
                        app_info_person.Mother_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Mother_Last_Name = row["motherSurName"].ToString().Trim();
                    }

                    if (row["previousFirstName"].ToString().Trim() == "")
                    {
                        app_info_person.Prior_First_Name = "";
                    }
                    else
                    {
                        app_info_person.Prior_First_Name = row["previousFirstName"].ToString().Trim();
                    }

                    if (row["previousSurName"].ToString().Trim() == "")
                    {
                        app_info_person.Prior_Last_Name = "";
                    }
                    else
                    {
                        app_info_person.Prior_Last_Name = row["previousSurName"].ToString().Trim();
                    }
                    app_info_person.Marital_Status = row["Marital_Status"].ToString().Trim();
                    app_info_person.Relationship = row["Relationship"].ToString().Trim();
                   
                    if (row["id"].ToString().Trim() != "")
                    {//update 
                        try
                        {
                            if (da_application_fp6.UpdateAppInfoPerson(app_info_person))
                            {

                                lblMessagePersonalInfo.Text = "Updated successfully.";
                            }

                            else
                            {
                                lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.AddExceptionToLog("Error update person [btnUpdatePerson_Click] page [application_form_fp6] [update application info person], Detail: " + ex.Message);
                            return;
                        }
                    }
                    else
                    {//save
                        try
                        {

                            if (da_application_fp6.InsertAppInfoPerson(app_info_person))
                            {

                                lblMessagePersonalInfo.Text = "Updated successfully.";
                            }

                            else
                            {
                                lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.AddExceptionToLog("Error update person [btnUpdatePerson_Click] page [application_form_fp6] [Save new application info person], Detail: " + ex.Message);
                            return;
                        }
                    }
                }

                else
                {// Policy owner
                    #region Update into database

                    if (row["id"].ToString().Trim() != "")
                    { //update database
                        try
                        {
                            da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                            person.App_Register_ID = ViewState["appID"] + "";
                            person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                            person.Country_ID = row["nationality"].ToString();
                            person.Father_First_Name = row["fatherFirstName"].ToString();
                            person.Father_Last_Name = row["fatherSurName"].ToString();
                            person.First_Name = row["firstEnName"].ToString();
                            person.Last_Name = row["surEnName"].ToString();
                            person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                            person.Khmer_Last_Name = row["surKhName"].ToString();
                            person.Gender = gender;// Convert.ToInt32(row["gender"].ToString());
                            person.ID_Card = row["idNumber"].ToString();
                            person.ID_Type = Convert.ToInt32(row["idtype"].ToString());

                            person.Level = Convert.ToInt32(row["level"].ToString());
                            person.Mother_First_Name = row["motherFirstName"].ToString();
                            person.Mother_Last_Name = row["motherSurName"].ToString();
                            person.Person_ID = row["id"].ToString().Trim();
                            person.Prior_First_Name = row["previousFirstName"].ToString();
                            person.Prior_Last_Name = row["previousSurName"].ToString();

                            person.Marital_Status = row["Marital_Status"].ToString();
                            person.Relationship = row["Relationship"].ToString();

                            if (da_application_fp6.UpdateAppInfoPersonSub(person))
                            {
                                lblMessagePersonalInfo.Text = "Updated successfully.";
                            }
                            else
                            {
                                lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.AddExceptionToLog("Error update person [btnUpdatePerson_Click] page [application_form_fp6], Detail: " + ex.Message);
                            return;
                        }

                    }
                    #endregion

                    #region Save into database

                    else
                    { //save into database
                        try
                        {
                            da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                            person.App_Register_ID = ViewState["appID"] + "";
                            person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                            person.Country_ID = row["nationality"].ToString();
                            person.Father_First_Name = row["fatherFirstName"].ToString();
                            person.Father_Last_Name = row["fatherSurName"].ToString();
                            person.First_Name = row["firstEnName"].ToString();
                            person.Last_Name = row["surEnName"].ToString();
                            person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                            person.Khmer_Last_Name = row["surKhName"].ToString();
                            person.Gender = gender;// Convert.ToInt32(row["gender"].ToString());
                            person.ID_Card = row["idNumber"].ToString();
                            person.ID_Type = Convert.ToInt32(row["idtype"].ToString());
                            person.Level = Convert.ToInt32(row["level"].ToString());
                            person.Mother_First_Name = row["motherFirstName"].ToString();
                            person.Mother_Last_Name = row["motherSurName"].ToString();

                            string id = Helper.GetNewGuid("SP_Check_App_Info_Person_Sub_ID", "Person_ID");

                            person.Person_ID = id;
                            person.Prior_First_Name = row["previousFirstName"].ToString();
                            person.Prior_Last_Name = row["previousSurName"].ToString();

                            person.Marital_Status = row["Marital_Status"].ToString();
                            person.Relationship = row["Relationship"].ToString();

                            if (da_application_fp6.InsertAppInfoPersonSub(person))
                            {
                                lblMessagePersonalInfo.Text = "Updated successfully.";
                            }
                            else
                            {
                                lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.AddExceptionToLog("Error update person [btnUpdatePerson_Click] page [application_form_fp6], Detail: " + ex.Message);

                        }

                    }
                    #endregion
                }
               
            
            }
            #region Delete data in database base on records deleted from gridview
            try
            {
                string personIDs ="";
                if (ViewState["personID"] != null) {
                    personIDs = ViewState["personID"] + "";

                    personIDs = personIDs.Substring(0, personIDs.Length - 1);

                    //char[] a = {',' };
                    //string[] delete = personIDs.Split(a);
                    //for (int i = 0; i < delete.Length; i++)
                    //{
                    //    if (!da_application_fp6.DeleteAppInfoPersonSubByPersonIDs(delete[i].ToString()))
                    //    {
                    //        lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                    //    }
                    //    else
                    //    {

                    //        lblMessagePersonalInfo.Text = "Updated successfully.";
                    //        ViewState["personID"] = null;
                    //    }
                    //}

                    if (!da_application_fp6.DeleteAppInfoPersonSubByPersonIDs(personIDs))
                    {
                        lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                    }
                    else
                    {

                        lblMessagePersonalInfo.Text = "Updated successfully.";
                        ViewState["personID"] = null;
                    }
                   
                }
            }
            catch (Exception ex) 
            {
                Log.AddExceptionToLog("Delete personal data in function [btnUpdatePerson_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            }
            #endregion
            //reload data
            //call load person

            LoadPerson(ViewState["appID"] + "");
        }
        else {
            lblMessagePersonalInfo.Text = "List of Person is empty cannot be updated.";
        }

       
    }

    //update Benefit
    protected void btnUpdateBenefit_Click(object sender, EventArgs e)
    {
        try
        {
            int seqNumber = 0;

            string appID = "";
            if (ViewState["appID"] != null)
            {
                appID = ViewState["appID"] + "";
            }

            DataTable tblBenifit = (DataTable)ViewState["tblBenifit"];
            int rowCount = 0;
            rowCount = tblBenifit.Rows.Count;
            if (rowCount > 0)
            { 
                //chect total percentage
                int total_percentag = 0;
                foreach (DataRow row in tblBenifit.Rows)
                {
                    total_percentag = total_percentag + Convert.ToInt32(row["Percentage"].ToString());
                }
                if (total_percentag > 100 || total_percentag < 100)
                {
                    lblMessageBenifit.Text = "Total percentage must be equal to 100%.";
                    return;
                }
                foreach(DataRow r in tblBenifit.Rows)
                {
                  
                    int idType = -1;
                    string idNumber = "";
                    string fullName = "";
                    string relation = "";
                    int shared = 0;
                    string id = "";
                    string remarks = "";
                    idType = Convert.ToInt32(r["ID_Type"].ToString());
                    idNumber = r["id_card"].ToString();
                    fullName = r["full_name"].ToString();
                    relation = r["relationship"].ToString();
                    shared = Convert.ToInt32(r["percentage"].ToString());
                    id = r["app_benefit_item_id"].ToString();
                    remarks = r["remarks"].ToString();

                    seqNumber += 1;

                    if (appID != "")
                    {
                        if (id.Trim() == "")
                        {
                            #region Save into database

                            try
                            {
                                string new_guid = Helper.GetNewGuid("SP_Check_App_Benefit_Item_ID", "@App_Benefit_Item_ID").ToString();

                                bl_app_benefit_item benefit_item = new bl_app_benefit_item();

                                benefit_item.App_Benefit_Item_ID = new_guid;
                                benefit_item.App_Register_ID = appID;
                                benefit_item.Full_Name = fullName;
                                benefit_item.ID_Type = idType;
                                benefit_item.ID_Card = idNumber;
                                benefit_item.Percentage = shared;
                                benefit_item.Relationship = relation;
                                benefit_item.Seq_Number = seqNumber;
                                benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);
                                benefit_item.Remarks = remarks;
                                if (!da_application.InsertAppBenefitItem(benefit_item))
                                {

                                    //alert message
                                    lblMessageBenifit.Text = "Updated fail, please contact your system administrator";
                                    return;
                                }
                                else
                                {
                                    lblMessageBenifit.Text = "Updated successfully.";
                                }

                            }
                            catch (Exception ex)
                            {
                                Log.AddExceptionToLog("Save new benefit fail in function [btnUpdateBenefit_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
                                return;
                            }

                            #endregion

                        }
                        else
                        {
                            #region Update into database

                            try
                            {
                                bl_app_benefit_item benefit_item = new bl_app_benefit_item();

                                benefit_item.App_Benefit_Item_ID = id;
                                benefit_item.App_Register_ID = appID;
                                benefit_item.Full_Name = fullName;
                                benefit_item.ID_Type = idType;
                                benefit_item.ID_Card = idNumber;
                                benefit_item.Percentage = shared;
                                benefit_item.Relationship = relation;
                                benefit_item.Seq_Number = seqNumber;
                                benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);
                                benefit_item.Remarks = remarks;

                                if (!da_application_fp6.UpdateAppBenefitItem(benefit_item))
                                {
                                    //alert message
                                    lblMessageBenifit.Text = "Updated fail, please contact your system administrator";
                                    return;

                                }
                                else
                                {
                                    lblMessageBenifit.Text = "Updated successfully.";
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.AddExceptionToLog("Update benefit fail in function [btnUpdateBenefit_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
                                return;
                            }


                            #endregion

                        }
                    }

                }
            }

           

            #region Delete data in database base on records delete from gridview

            try
            {
                
                String app_benefit_item_id = "";

                if (ViewState["app_benefit_item_id"] != null) {
                    app_benefit_item_id = ViewState["app_benefit_item_id"] + "";

                    app_benefit_item_id = app_benefit_item_id.Trim().Substring(0, app_benefit_item_id.Trim().Length - 1);

                      
                    char[] a = { ',' };
                     
                    string[] delete = app_benefit_item_id.Split(a);
                    for (int i = 0; i < delete.Length; i++)
                    {

                        bl_app_benefit_item benefit = new bl_app_benefit_item();
                        benefit.App_Benefit_Item_ID = delete[i];
                        benefit.App_Register_ID = appID;

                        if (!da_application_fp6.DeleteAppBenefitItemByIDs(benefit))
                        {
                            //alert message
                            lblMessageBenifit.Text = "Updated fail, please contact your system administrator";
                            return;
                        }
                        else
                        {
                            lblMessageBenifit.Text = "Updated successfully.";
                            ViewState["app_benefit_item_id"] = null;
                        }
                    }
                }
                   
                
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Delete benefit fail in function [btnUpdateBenefit_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
                return;
            }

            #endregion

            //reload Benefit
            LoadBenefit(appID);

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnUpdateBenefit_Click] in page [application_form_fp6.aspx.cs]. Details: " + ex.Message);
           
        }

    }

    //update job history
    protected void btnSaveJob_Click(object sender, EventArgs e)
    {
        //update job history
        string appID = "";
    
        try
        {
            if (ViewState["appID"] != null)
            {

                appID = ViewState["appID"] + "";
            }


            if (appID.Trim() != "")
            {

                DataTable tblJobHistory =(DataTable) ViewState["tblJobHistory"];
                foreach (DataRow row in tblJobHistory.Rows)
                { //Loop in datatable

                    if (row["Level"].ToString().Trim() == "1")
                    { // if ==1 is life insure
                        if (row["job_id"].ToString().Trim() == "0")
                        { // job_id ==0 is new so save it
                            bl_app_job_history job = new bl_app_job_history();
                            job.App_Register_ID = appID;
                            job.Nature_Of_Business = row["nature_of_business"].ToString().Trim();
                            job.Employer_Name = row["employer_name"].ToString().Trim();
                            job.Current_Position = row["current_position"].ToString().Trim();
                            job.Job_Role = row["job_role"].ToString().Trim();
                            job.Anual_Income = Convert.ToDouble( row["anual_income"].ToString().Trim());
                            

                            if (da_application.InsertAppJobHistory(job))
                            { //inserted success
                                lblMessageJobHistory.Text = "Updated successfully!";
                            }
                            else
                            { //inserted fail
                                lblMessageJobHistory.Text = "Updated fail, please contact your system administrator.";
                                return;
                            }

                        }
                        else
                        { // job_id !=0 is old record so update it 
                            bl_app_job_history job = new bl_app_job_history();

                            job.App_Register_ID = appID;
                            job.Anual_Income = 0;
                            job.Current_Position = txtCurrentPositionLife1.Text.Trim();
                            job.Employer_Name = txtNameEmployerLife1.Text.Trim();
                            job.Job_Role = txtRoleAndResponsibilityLife1.Text.Trim();
                            job.Nature_Of_Business = "";

                            //update into database
                            if (da_application_fp6.UpdateAppJobHistory(job))
                            {
                                //message alert
                                lblMessageJobHistory.Text = "Updated successfully!";
                            }
                            else
                            {
                                lblMessageJobHistory.Text = "Updated fail, please contact your system administrator.";
                                return;
                            }
                        }
                    }
                    else
                    {// if !=0 is life insured 
                        if (row["job_id"].ToString().Trim() == "")
                        { // job_id ==0 is new so save it
                            da_application_fp6.bl_app_job_history_sub job = new da_application_fp6.bl_app_job_history_sub();
                            job.App_Register_ID = appID;
                            job.Nature_Of_Business = row["nature_of_business"].ToString().Trim();
                            job.Employer_Name = row["employer_name"].ToString().Trim();
                            job.Current_Position = row["current_position"].ToString().Trim();
                            job.Job_Role = row["job_role"].ToString().Trim();
                            job.Anual_Income = Convert.ToDouble(row["anual_income"].ToString().Trim());
                            job.Level = Convert.ToInt32(row["level"].ToString().Trim());
                            job.Job_ID = Helper.GetNewGuid("SP_Check_App_Info_Job_Sub_ID","@Job_ID");
                            job.Address = "";

                            if (da_application_fp6.InsertAppJobHistorySub(job))
                            { //inserted success
                                lblMessageJobHistory.Text = "Updated successfully!";
                            }
                            else
                            { //inserted fail
                                lblMessageJobHistory.Text = "Updated fail, please contact your system administrator.";
                                return;
                            }
                        }
                        else
                        { // job_id !=0 is old record so update it 
                            da_application_fp6.bl_app_job_history_sub job = new da_application_fp6.bl_app_job_history_sub();
                            job.App_Register_ID = appID;
                            job.Nature_Of_Business = row["nature_of_business"].ToString().Trim();
                            job.Employer_Name = row["employer_name"].ToString().Trim();
                            job.Current_Position = row["current_position"].ToString().Trim();
                            job.Job_Role = row["job_role"].ToString().Trim();
                            job.Anual_Income = Convert.ToDouble(row["anual_income"].ToString().Trim());
                            job.Level = Convert.ToInt32(row["level"].ToString().Trim());
                            job.Job_ID = row["job_id"].ToString().Trim();
                            job.Address = "";

                            if (da_application_fp6.UpdateAppJobHistorySub(job))
                            { //inserted success
                                lblMessageJobHistory.Text = "Updated successfully!";
                            }
                            else
                            { //inserted fail
                                lblMessageJobHistory.Text = "Updated fail, please contact your system administrator.";
                                return;
                            }
                        }
                    }
                }//end loop

                //delete data
                if (ViewState["jobID"] != null)
                {
                    string jobId = "";
                    jobId = ViewState["jobID"] + "";
                    jobId = jobId.Substring(0, jobId.Length - 1);

                    char[] a = { ','};
                    string[] delete = jobId.Split(a);
                    for (int i = 0; i < delete.Length; i++)
                    {
                        da_application_fp6.bl_app_job_history_sub jobhistory = new da_application_fp6.bl_app_job_history_sub();
                        jobhistory.App_Register_ID = appID;
                        jobhistory.Job_ID = delete[i];
                        if (da_application_fp6.DeleteAppJobHistoryByJobIDs(jobhistory))
                        { //inserted success
                            lblMessageJobHistory.Text = "Updated successfully!";
                        }
                        else
                        { //inserted fail
                            lblMessageJobHistory.Text = "Updated fail, please contact your system administrator.";
                            return;
                        }
                    }
                    
                }

                //reload data
                LoadJobHistory(appID);
            }
        }

        catch (Exception ex)
        {
            Log.AddExceptionToLog("Update Job history for policy owner was fail  in function [btnSaveJob_Click] in page [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            return;
        }
       
    }
    //update contact and address
    protected void btnAddressUpdate_Click(object sender, EventArgs e)
    {
        string appID = "";
        if (ViewState["appID"] != null)
        {
            appID = ViewState["appID"] + "";
        }
        
        #region update address
        //policy owner
        try
        {
            bl_app_info_address app_info_address = new bl_app_info_address();

            app_info_address.App_Register_ID = appID;

            app_info_address.Country_ID = ddlCountryLife1.SelectedValue;
            app_info_address.Address1 = txtAddress1Life1.Text.Trim();
            app_info_address.Address2 = txtAddress2Life1.Text.Trim();
            app_info_address.Address3 = "";

            app_info_address.Province = txtCityLife1.Text.Trim();

            //check for null
            if (txtZipCodeLife1.Text.Trim() == "")
            {
                app_info_address.Zip_Code = "";
            }
            else
            {
                app_info_address.Zip_Code = txtZipCodeLife1.Text.Trim();
            }

            //Update into database
            if (da_application_fp6.UpdateAppInfoAddressSub(app_info_address))
            {
               //message alert
                lblMessageConatact.Text = "Updated successfully.";
            }
            else
            {
                //message alert
                lblMessageConatact.Text = "Updated was fial, please contact your system administrator.";
                
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Updated address for policy owner was fial in function [btnAddressUpdate_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            return;
        }

        //Life insured 1
        try
        {
            bl_app_info_address app_info_address = new bl_app_info_address();

            app_info_address.App_Register_ID = appID;

            app_info_address.Country_ID = ddlCountryLife2.SelectedValue;
            app_info_address.Address1 = txtAddress1Life2.Text.Trim();
            app_info_address.Address2 = txtAddress2Life2.Text.Trim();
            app_info_address.Address3 = "";

            app_info_address.Province = txtCityLife2.Text.Trim();

            //check for null
            if (txtZipCodeLife2.Text.Trim() == "")
            {
                app_info_address.Zip_Code = "";
            }
            else
            {
                app_info_address.Zip_Code = txtZipCodeLife2.Text.Trim();
            }

            //Insertion
            if (da_application_fp6.UpdateAppInfoAddress(app_info_address))
            {
                //message alert
                lblMessageConatact.Text = "Updated successfully.";
            }
            else
            {
                //message alert
                lblMessageConatact.Text = "Updated was fial, please contact your system administrator.";

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Updated address for life insured 1 was fial in function [btnAddressUpdate_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            return;
        }

        #endregion

        #region update contact

        //Policy owner
        try
        {
            bl_app_info_contact app_info_contact = new bl_app_info_contact();

            app_info_contact.App_Register_ID = appID;

            app_info_contact.Fax1 = "";
            app_info_contact.Fax2 = "";
            app_info_contact.Home_Phone2 = "";
            app_info_contact.Mobile_Phone2 = "";
            app_info_contact.Office_Phone1 = "";
            app_info_contact.Office_Phone2 = "";

            //check for null
            if (txtEmailLife1.Text.Trim() == "")
            {
                app_info_contact.EMail = "";
            }
            else
            {
                app_info_contact.EMail = txtEmailLife1.Text.Trim();
            }

            if (txtTelephoneLife1.Text.Trim() == "")
            {
                app_info_contact.Home_Phone1 = "";
            }
            else
            {
                app_info_contact.Home_Phone1 = txtTelephoneLife1.Text.Trim();
            }

            if (txtMobilePhoneLife1.Text.Trim() == "")
            {
                app_info_contact.Mobile_Phone1 = "";
            }
            else
            {
                app_info_contact.Mobile_Phone1 = txtMobilePhoneLife1.Text.Trim();

            }

            //Insertion
            if (da_application_fp6.UpdateAppInfoContactSub(app_info_contact))
            {
                //message alert
                lblMessageConatact.Text = "Updated successfully.";
            }
            else
            {
                //message alert
                lblMessageConatact.Text = "Updated was fial, please contact your system administrator.";

            }
        }
        catch (Exception ex) 
        {
            Log.AddExceptionToLog("Updated address for policy owner was fial in function [btnAddressUpdate_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            return;

        }

        //Life insured 1
        try
        {
            bl_app_info_contact app_info_contact = new bl_app_info_contact();

            app_info_contact.App_Register_ID = appID;

            app_info_contact.Fax1 = "";
            app_info_contact.Fax2 = "";
            app_info_contact.Home_Phone2 = "";
            app_info_contact.Mobile_Phone2 = "";
            app_info_contact.Office_Phone1 = "";
            app_info_contact.Office_Phone2 = "";

            //check for null
            if (txtEmailLife2.Text.Trim() == "")
            {
                app_info_contact.EMail = "";
            }
            else
            {
                app_info_contact.EMail = txtEmailLife2.Text.Trim();
            }

            if (txtTelephoneLife2.Text.Trim() == "")
            {
                app_info_contact.Home_Phone1 = "";
            }
            else
            {
                app_info_contact.Home_Phone1 = txtTelephoneLife2.Text.Trim();
            }

            if (txtMobilePhoneLife2.Text.Trim() == "")
            {
                app_info_contact.Mobile_Phone1 = "";
            }
            else
            {
                app_info_contact.Mobile_Phone1 = txtMobilePhoneLife2.Text.Trim();

            }

            //Insertion

            if (da_application_fp6.UpdateAppInfoContact(app_info_contact))
            {
                //message alert
                lblMessageConatact.Text = "Updated successfully.";
               // AlertMessage("Updated successfully.");
            }
            else
            {
                //message alert
                lblMessageConatact.Text = "Updated was fial, please contact your system administrator.";

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Updated address for life insured 1 was fial in function [btnAddressUpdate_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            return;
        }
        #endregion


    }

   
    void AlertMessage(string message)
    {
        string cleanMessage = message.Replace("'", "\'");
        Page page = HttpContext.Current.CurrentHandler as Page;
        string script = string.Format("alert('{0}');", cleanMessage);
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
        }
    } 

    //Enable buttons for update 
    void EnableButtonsUpdate(bool status) 
    {
        /*
        //benefit 
        btnUpdateBenefit.Enabled = status;
        //personal
        btnUpdatePerson.Enabled = status;
        //job history
        btnSaveJob.Enabled = status;
        //Plan
        btnSaveInsuredPlan.Enabled = status;
        //contact and address
        btnAddressUpdate.Enabled = status;

        //health
        btnBodySave.Enabled = status;
        */

        //benefit 
        btnUpdateBenefit.Visible = status;
        //personal
        btnUpdatePerson.Visible = status;
        //job history
        btnSaveJob.Visible = status;
        //Plan
        btnSaveInsuredPlan.Visible = status;
        //contact and address
        btnAddressUpdate.Visible = status;

        //health
        btnBodySave.Visible = status;
    }

    #region Clear form
    //Clear application detail
    void ClearApplicationDetial()
    {
        lblMessageApplication.Text = "";

        txtApplicationNumber.Text = "";
        ddlChannel.SelectedIndex = 0;
        ddlCompany.Items.Clear();
        txtPaymentCode.Text = "";
        txtPolicyNumber.Text = "";
        txtUnderwritingStatus.Text = "";
        txtDateEntry.Text = "";
        txtDateSignature.Text = "";
        txtDataEntryBy.Text = "";
        txtMarketingCode.Text = "";
        txtMarketingName.Text = "";
        txtNote.Text = "";
    }
    //Clear Personal Information
    void ClearPersonalInformation()
    {
        ClearPerson();
        //Clear record in datatable
        DataTable tbl = (DataTable) ViewState["tblPersonal"];

        tbl.Clear();

        ViewState["tblPersonal"] = tbl;

        gvPersonalInfo.DataSource = tbl;
        gvPersonalInfo.DataBind();

       // btnUpdatePerson.Enabled = false;
    }
    //Clear Mailling Address
    void ClearMaillingAddress()
    {
        lblMessageConatact.Text = "";
        //Policy owner
        txtAddress1Life1.Text = "";
        txtAddress2Life1.Text = "";
        txtCityLife1.Text = "";
        txtZipCodeLife1.Text = "";
        ddlCountryLife1.SelectedIndex = 0;
        txtTelephoneLife1.Text = "";
        txtMobilePhoneLife1.Text = "";
        txtEmailLife1.Text = "";
        //life insured 1
        txtAddress2Life1.Text = "";
        txtAddress2Life2.Text = "";
        txtCityLife2.Text = "";
        txtZipCodeLife2.Text = "";
        ddlCountryLife2.SelectedIndex = 0;
        txtTelephoneLife2.Text = "";
        txtMobilePhoneLife2.Text = "";
        txtEmailLife2.Text = "";
        //button save
     //   btnAddressUpdate.Enabled = false;
    }
    //Clear Job History
    void ClearJobHistory()
    {
        lblMessageJobHistory.Text = "";
        //policy owner
        txtNameEmployerLife1.Text = "";
        txtNatureBusinessLife1.Text = "";
        txtRoleAndResponsibilityLife1.Text = "";
        txtCurrentPositionLife1.Text = "";
        txtAnualIncomeLife1.Text = "";

        DataTable tblJobHistory = (DataTable)ViewState["tblJobHistory"];
        tblJobHistory.Clear();
        gvJobHistory.DataSource = tblJobHistory;
        gvJobHistory.DataBind();

       // btnSaveJob.Enabled = false;
    }

    //Clear Loan
    void ClearLoan()
    {
        ddlLoanType.SelectedIndex = 0;
        txtInterest.Text = "";
        txtTermLoan.Text = "";
        txtLoanEffectiveDate.Text = "";
        txtOutstandingLoanAmount.Text = "";
    }

    //Clear Insured Plan
    void ClearInsuredPlan()
    {
        lblMessageInsurancePlan.Text = "";

        ddlTypeInsurancePlan.SelectedIndex = 0;
        txtTermInsurance.Text = "";
        txtPaymentPeriod.Text = "";
        txtInsuranceAmountRequired.Text = "";
        ddlPremiumMode.SelectedIndex = 0;
        txtAssureeAgeLife1.Text = "";

        txtAnnualOriginalPremiumAmountSystemLife1.Text = "";

        txtPremiumAmountSystemLife1.Text = "";

        txtPremiumOriginalAmountLife1.Text = "";
        
        txtTotalOriginalPremium.Text = "";
        txtTotalPremium.Text = "";
        txtDiscountAmount.Text = "";
        txtTotalPremiumAfterDiscount.Text = "";

        txtInsurancePlanNote.Text = "";

        //btnSaveInsuredPlan.Enabled = false;

    }
    
    //Clear Benefit
    void ClearBenefit()
    {
        lblMessageBenifit.Text = "";

        ddlBenefitIDType.SelectedIndex = 0;
        txtBenefitIDNo.Text = "";
        txtBenefitName.Text = "";
        ddlBenefitRelation.SelectedIndex = 0;
        txtBenefitSharePercentage.Text = "";
        txtBenefitNote.Text = "";
        txtTotalSharedPercentage.Text = "";

        DataTable tbl = (DataTable)ViewState["tblBenifit"];

        tbl.Clear();

        gvBenifit.DataSource = tbl;
        gvBenifit.DataBind();

        //btnUpdateBenefit.Enabled = false;
    }

    //Clear Body
    void ClearBody()
    {
        lblMessageHealth.Text = "";

        txtHeightLife1.Text = "";
        txtWeightLife1.Text = "";
        rblWeightChangeLife1.SelectedIndex = 0;
        txtWeightChangeReasonLife1.Text = "";
        lblWeightChangeReasonLife1.Visible = false;

        DataTable tblBody = (DataTable)ViewState["tblBody"];
        tblBody.Clear();
     
        gvBody.DataSource = tblBody;
        gvBody.DataBind();
       
    }

    //Clear Answers
    void ClearAnswer()
    {
        foreach (GridViewRow row in GvQA.Rows)
        {
            RadioButtonList rbtnlAnswerLife1 = (RadioButtonList)row.FindControl("rbtnlAnswerLife1");
            RadioButtonList rbtnlAnswerLife2 = (RadioButtonList)row.FindControl("rbtnlAnswerLife2");
         
            rbtnlAnswerLife1.SelectedIndex = -1;
            rbtnlAnswerLife2.SelectedIndex = -1;
           
        }
    }


    //Clear All
    void ClearForm()
    {
        ClearApplicationDetial();
        ClearPersonalInformation();
        ClearMaillingAddress();
        ClearJobHistory();
        ClearLoan();
        ClearInsuredPlan();
        ClearRider();
        ClearBenefit();
        ClearBody();
        ClearAnswer();

        

    }
    #endregion

    protected void ImgBtnClear_Click(object sender, ImageClickEventArgs e)
    {
        ClearForm();
        //Clear Viewstate

        ViewState.Clear();
        Response.Redirect("application_form_fp6.aspx");
    }
    protected void btnSaveInsuredPlan_Click(object sender, EventArgs e)
    {
        if (txtUnderwritingStatus.Text.Trim() != "")
        {
            //update user premium
            int user_premium = 0;
            if (txtPremiumAmount.Text.Trim() != "")
            {
                user_premium = Convert.ToInt32(txtPremiumAmount.Text.Trim());
            }

            if (UpdateUserPremium(ViewState["appID"] + "", user_premium))
            {
                lblMessageInsurancePlan.Text = "User premium updated successfully.";
            }
            else
            {
                lblMessageInsurancePlan.Text = "Updated user premium fail, please contact your system administator.";
            }

        }
        else
        {
            if (UpdateInsuredPlan(ViewState["appID"] + ""))
            {
                lblMessageInsurancePlan.Text = "Updated successfully.";
            }
            else
            {
                lblMessageInsurancePlan.Text = "Updated fail, please contact your system administator.";
            }
        }
    }
    //add job history into gridview
    protected void btnAddJobHistory_Click(object sender, EventArgs e)
    {

        try
        {
            if (txtApplicationNumber.Text.Trim() != "")
            {

                DataTable tblJob = (DataTable)ViewState["tblJobHistory"];
                DataRow row;
                string appID = ViewState["appID"] + "";

                if (ViewState["tblJobHistoryRowIndex"] == null)
                {//save record into datatable

                    //check owner 
                    int count = tblJob.Rows.Count;
                    int level = 0;

                    if (ddlJobPolicyOwner.SelectedValue.Trim() == "0")
                    { //policy owner
                        if (count > 0)
                        {
                            int limit = 0;
                            for (int i = 0; i < count; i++)
                            {
                                string strLevel= tblJob.Rows[i]["Level"].ToString().Trim();
                                if (strLevel.Trim() == "0")
                                {
                                    limit += 1;

                                }
                            }
                            if (limit >= 1)
                            {
                                //not allow input policy owner more than one
                                lblMessageJobHistory.Text = "Max life insured is 1.";
                                return;
                            }
                        }

                    }

                    else if (ddlJobPolicyOwner.SelectedValue.Trim() != "0" && ddlJobPolicyOwner.SelectedValue.Trim() != "")
                    { //life insured
                        if (count > 0)
                        {
                            int limit = 0;
                            for (int i = 0; i < count; i++)
                            {
                                string strLevel = tblJob.Rows[i]["Level"].ToString().Trim();
                                if (strLevel.Trim() != "0")
                                {
                                    limit += 1;

                                }
                            }
                            if (limit >= 5)
                            {
                                //not allow input policy owner more than 5
                                lblMessageJobHistory.Text = "Max life insured is 5";
                                return;
                            }

                            int rowCount = tblJob.Rows.Count;

                            level = rowCount;

                        }
                    }

                    //if policy owner is the life insured auto add two records
                    if (ckbPolicyowner.Checked)
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            row = tblJob.NewRow();
                            row["app_register_id"] = appID;
                            row["job_id"] = "";
                            row["employer_name"] = txtNameEmployerLife1.Text.Trim();
                            row["nature_of_business"] = txtNatureBusinessLife1.Text.Trim();
                            row["current_position"] = txtCurrentPositionLife1.Text.Trim();
                            row["job_role"] = txtRoleAndResponsibilityLife1.Text.Trim();
                            row["anual_income"] = txtAnualIncomeLife1.Text.Trim();
                            row["level"] = i;

                            tblJob.Rows.Add(row);
                        }
                       
                    }
                    else
                    {
                        row = tblJob.NewRow();
                        row["app_register_id"] = appID;
                        row["job_id"] = "";
                        row["employer_name"] = txtNameEmployerLife1.Text.Trim();
                        row["nature_of_business"] = txtNatureBusinessLife1.Text.Trim();
                        row["current_position"] = txtCurrentPositionLife1.Text.Trim();
                        row["job_role"] = txtRoleAndResponsibilityLife1.Text.Trim();
                        row["anual_income"] = txtAnualIncomeLife1.Text.Trim();
                        row["level"] = level;

                        tblJob.Rows.Add(row);
                    }
                   
                    ViewState["tblJobHistory"] = tblJob;
                    gvJobHistory.DataSource = tblJob;
                    gvJobHistory.DataBind();

                    //clear message
                    lblMessageJobHistory.Text = "";   

                }
                else// if gridview row seleted update record into datatable
                {
                    try
                    {
                        int rowIndex =Convert.ToInt32( ViewState["tblJobHistoryRowIndex"] + "");
                        var r= tblJob.Rows[rowIndex];

                        r["employer_name"] = txtNameEmployerLife1.Text.Trim();
                        r["nature_of_business"] = txtNatureBusinessLife1.Text.Trim();
                        r["current_position"] = txtCurrentPositionLife1.Text.Trim();
                        r["job_role"] = txtRoleAndResponsibilityLife1.Text.Trim();
                        r["anual_income"] = txtAnualIncomeLife1.Text.Trim();

                        tblJob.AcceptChanges();
                        ViewState["tblJobHistory"] = tblJob;
                        gvJobHistory.DataSource = tblJob;
                        gvJobHistory.DataBind();

                    }
                    catch (Exception ex)
                    {
                        Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add Job History. Detail: " + ex.Message);

                        lblMessagePersonalInfo.Text = "Update fail, please Job History your system administrator.";

                    }
                }

               //Clear text box
                ClearJobHistoryInfo();

            }
            else // if application number is empty
            {
                //alert message
                lblMessageJobHistory.Text = "Application number is required.";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function btnAddJobHistory_Click in page application_form_fp6.aspx, Detail: " + ex.Message);
            lblMessageJobHistory.Text = "Add job history fail, please contact your system administrator.";
        }
        

    }
    void ClearJobHistoryInfo()
    {
        //Clear text box
       
        txtNameEmployerLife1.Text = "";
        txtNatureBusinessLife1.Text = "";
        txtRoleAndResponsibilityLife1.Text = "";
        txtCurrentPositionLife1.Text = "";
        txtAnualIncomeLife1.Text = "";

        btnAddJobHistory.Text = "Add";
        ViewState["tblJobHistoryRowIndex"] = null;
    }
    protected void gvJobHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblJobHistory = (DataTable)ViewState["tblJobHistory"];
            int rowIndex = e.RowIndex;

            string id = "";
            id = tblJobHistory.Rows[rowIndex]["job_id"].ToString().Trim();
            if (id != "" && id != "0")
            {

                ViewState["jobID"] += id + ",";
            }

            tblJobHistory.Rows[rowIndex].Delete();
            tblJobHistory.AcceptChanges();

            ViewState["tblJobHistory"] = tblJobHistory;
            gvJobHistory.DataSource = tblJobHistory;
            gvJobHistory.DataBind();


        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Delete Job history error in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }
    }
    protected void gvJobHistory_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataTable tblJob = (DataTable)ViewState["tblJobHistory"];
        if (tblJob.Rows.Count > 0)
        {
            int row = e.NewSelectedIndex;
            var a = tblJob.Rows[row];

            ViewState["tblJobHistoryRowIndex"] =row;
            btnAddJobHistory.Text = "Update";

            txtNameEmployerLife1.Text = a["employer_name"].ToString().Trim();
            txtNatureBusinessLife1.Text = a["nature_of_business"].ToString().Trim();
            txtCurrentPositionLife1.Text = a["current_position"].ToString().Trim();
            txtRoleAndResponsibilityLife1.Text = a["job_role"].ToString().Trim();
            txtAnualIncomeLife1.Text = a["anual_income"].ToString().Trim();

            //select item in dropdown list
            Helper.SelectedDropDownListIndex("VALUE", ddlJobPolicyOwner, a["level"].ToString().Trim());

        }
        else //if no record in data table
        {
            //alret message
            lblMessageJobHistory.Text = "Record not found.";
        }
    }
    protected void btnAddWeight_Click(object sender, EventArgs e)
    {
       
        try
        {
            if (txtApplicationNumber.Text.Trim() != "")
            {

                DataTable tblBody = (DataTable)ViewState["tblBody"];
                DataRow row;
                string appID = ViewState["appID"] + "";

                if (ViewState["tblBodyRowIndex"] == null)
                {//save record into datatable

                    //check owner 
                    int count = tblBody.Rows.Count;
                    string reason = "";

                   

                    //if policy owner is the life insured auto add two records
                    if (ckbPolicyowner.Checked)
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            if (txtWeightChangeReasonLife1.Text.Trim() != "")
                            {
                                reason = txtWeightChangeReasonLife1.Text.Trim();
                            }

                            row = tblBody.NewRow();
                            row["app_register_id"] = appID;
                            row["weight"] = txtWeightLife1.Text.Trim();
                            row["height"] = txtHeightLife1.Text.Trim();
                            row["is_weight_changed"] = rblWeightChangeLife1.SelectedValue;
                            row["reason"] = reason;
                            row["level"] = i;
                            row["id"] = "";
                            tblBody.Rows.Add(row);
                        }
                    }
                    else
                    {
                        if (txtWeightChangeReasonLife1.Text.Trim() != "")
                        {
                            reason = txtWeightChangeReasonLife1.Text.Trim();
                        }

                        row = tblBody.NewRow();
                        row["app_register_id"] = appID;
                        row["weight"] = txtWeightLife1.Text.Trim();
                        row["height"] = txtHeightLife1.Text.Trim();
                        row["is_weight_changed"] = rblWeightChangeLife1.SelectedValue;
                        row["reason"] = reason;
                        row["level"] = ddlBodyPerson.SelectedValue.Trim();
                        row["id"] = "";
                        tblBody.Rows.Add(row);
                    }

                    ViewState["tblBody"] = tblBody;
                    gvBody.DataSource = tblBody;
                    gvBody.DataBind();

                    //clear message
                    lblMessageJobHistory.Text = "";
                }
                else// if gridview row seleted update record into datatable
                {
                    try
                    {
                        int rowIndex = Convert.ToInt32(ViewState["tblBodyRowIndex"] + "");
                        var r = tblBody.Rows[rowIndex];
                        string reason = "";

                        if (txtWeightChangeReasonLife1.Text.Trim() != "")
                        {
                            reason = txtWeightChangeReasonLife1.Text.Trim();
                        }
                        r["app_register_id"] = appID;
                        r["weight"] = txtWeightLife1.Text.Trim();
                        r["height"] = txtHeightLife1.Text.Trim();
                        r["is_weight_changed"] = rblWeightChangeLife1.SelectedValue;
                        r["reason"] = reason;
                        r["level"] = ddlBodyPerson.SelectedValue.Trim();

                        tblBody.AcceptChanges();
                        ViewState["tblBody"] = tblBody;
                        gvBody.DataSource = tblBody;
                        gvBody.DataBind();

                       

                    }
                    catch (Exception ex)
                    {
                        Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add body. Detail: " + ex.Message);

                        lblMessagePersonalInfo.Text = "Update fail, please contact your system administrator.";

                    }
                }

                //Clear text box
                ClearBodyInfo();

            }
            else // if application number is empty
            {
                //alert message
                lblMessageJobHistory.Text = "Application number is required.";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function btnAddJobHistory_Click in page application_form_fp6.aspx, Detail: " + ex.Message);
            lblMessageJobHistory.Text = "Add job history fail, please contact your system administrator.";
        }
        
    }
  
    protected void gvBody_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            int rowIndex = e.RowIndex;

            string id = "";
            id = tblBody.Rows[rowIndex]["id"].ToString().Trim();
            if (id != "")
            {

                ViewState["id"] += id + ",";
            }

            tblBody.Rows[rowIndex].Delete();
            tblBody.AcceptChanges();

            ViewState["tblBody"] = tblBody;
            gvBody.DataSource = tblBody;
            gvBody.DataBind();

            lblMessageHealth.Text = "table body rows="+ tblBody.Rows.Count+"";

        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Delete Body error in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }
    }
    protected void gvBody_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataTable tblBody = (DataTable)ViewState["tblBody"];
        if (tblBody.Rows.Count > 0)
        {
            int row = e.NewSelectedIndex;
            var a = tblBody.Rows[row];

            ViewState["tblBodyRowIndex"] = row;
            btnAddWeight.Text = "Update";
            txtWeightLife1.Text = a["weight"].ToString().Trim();
            txtHeightLife1.Text = a["height"].ToString().Trim();
            rblWeightChangeLife1.SelectedIndex = Convert.ToInt32(a["is_weight_changed"].ToString().Trim());
            if (Convert.ToInt32(a["is_weight_changed"].ToString().Trim()) > 0)
            { 
                lblWeightChangeReasonLife1.Visible=true;
                txtWeightChangeReasonLife1.Visible = true;
                txtWeightChangeReasonLife1.Text = a["reason"].ToString().Trim();

            }
            else
            {
                lblWeightChangeReasonLife1.Visible = false;
                txtWeightChangeReasonLife1.Visible = false;
                txtWeightChangeReasonLife1.Text ="";

            }
                      
            //select item in dropdown list
            Helper.SelectedDropDownListIndex("VALUE", ddlBodyPerson, a["level"].ToString().Trim());

        }
        else //if no record in data table
        {
            //alret message
            lblMessageHealth.Text = "Record not found.";
        }
    }
    void ClearBodyInfo()
    {
        ViewState["tblBodyRowIndex"] = null;
        btnAddWeight.Text = "Add";
        txtWeightLife1.Text = "";
        txtHeightLife1.Text = "";
        rblWeightChangeLife1.SelectedIndex = 0;
        lblWeightChangeReasonLife1.Visible = false;
        txtWeightChangeReasonLife1.Visible = false;
        txtWeightChangeReasonLife1.Text = "";
    }
    protected void btnClearWeight_Click(object sender, EventArgs e)
    {
        ClearBodyInfo();
        gvBody.SelectedIndex =- 1;
    }
    protected void btnBodySave_Click(object sender, EventArgs e)
    {
        try
        {
            #region Save body
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            int rowCount = tblBody.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                var row = tblBody.Rows[i];

                if (row["level"].ToString().Trim() == "1")
                {
                    if (row["id"].ToString().Trim() == "")
                    {//save
                        bl_app_info_body body = new bl_app_info_body();
                        body.App_Register_ID = row["app_register_id"].ToString().Trim();
                        body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                        body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                        body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                        body.Reason = row["reason"].ToString().Trim();

                        if (da_application.InsertAppInfoBody(body))
                        {
                            lblMessageHealth.Text = "Updated successfully.";
                        }
                        else
                        {
                            lblMessageHealth.Text = "Updated fail, please contact your system administrator.";
                            return;
                        }
                    }
                    else 
                    {//update

                       bl_app_info_body body = new bl_app_info_body();
                        body.App_Register_ID = row["app_register_id"].ToString().Trim();
                        body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                        body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                        body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                        body.Reason = row["reason"].ToString().Trim();

                        if (da_application.UpdateAppInfoBody(body))
                        {
                            lblMessageHealth.Text = "Updated successfully.";
                        }
                        else
                        {
                            lblMessageHealth.Text = "Updated fail, please contact your system administrator.";
                            return;
                        }
                    }
                   
                }
                else
                {// level =0 is policy owner
                    if (row["id"].ToString().Trim() == "")
                    { //id=="" save new

                        da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                        body.App_Register_ID = row["app_register_id"].ToString().Trim();
                        body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                        body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                        body.Level = Convert.ToInt32(row["level"].ToString().Trim());
                        body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                        body.Reason = row["reason"].ToString().Trim();
                        body.Id = Helper.GetNewGuid("SP_Check_App_Info_Body_Sub_ID", "@Id");

                        if (da_application_fp6.InsertAppInfoBodySub(body))
                        {
                            lblMessageHealth.Text = "Updated successfully.";
                        }
                        else
                        {
                            lblMessageHealth.Text = "Updated fail, please contact your system administrator.";
                            return;
                        }
                    }
                    else
                    { //id!="" update
                        da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                        body.App_Register_ID = row["app_register_id"].ToString().Trim();
                        body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                        body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                        body.Level = Convert.ToInt32(row["level"].ToString().Trim());
                        body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                        body.Id = row["id"].ToString().Trim();
                        body.Reason = row["reason"].ToString().Trim();



                        if (da_application_fp6.UpdateAppInfoBodySub(body))
                        {
                            lblMessageHealth.Text = "Updated successfully.";
                        }
                        else
                        {
                            lblMessageHealth.Text = "Updated fail, please contact your system administrator.";
                            return;
                        }

                    }

                }

            }
            #endregion End Save body

            #region Delete some record
            if (ViewState["id"] != null || ViewState["id"] + "" != "")
            {
                string id = "";
                id = ViewState["id"] + "";
                id = id.Substring(0, id.Length - 1);

                char[] a = { ',' };
                string[] delete = id.Split(a);
                for (int i = 0; i < delete.Length; i++)
                {
                    da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                    body.App_Register_ID = ViewState["appID"] + "";
                    body.Id = delete[i];

                    if (da_application_fp6.DeleteAppInfoBodySubByIDs(body))
                    {
                        lblMessageHealth.Text = "Updated successfully.";
                        //ViewState["id"] = null;
                    }
                    else
                    {
                        lblMessageHealth.Text = "Updated fail, please contact your system administrator.";
                    }
                }


            }
            #endregion End Delete some record

            #region Save Answer

            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int Count = tblPersonal.Rows.Count;
            string app_id = "";
            app_id = ViewState["appID"] + "";

            //delete existing answer
            for (int ex = 0; ex < Count; ex++)
            {
                da_application_fp6.DeleteAppAnswerByAppIDAndLevel(app_id, ex);
            }

            for (int i = 0; i < Count; i++)
            {

                foreach (GridViewRow row in GvQA.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {

                        RadioButtonList rbtnlAnswer1 = (RadioButtonList)row.FindControl("rbtnlAnswerLife1");
                        Label hdfSeqNumber1 = (Label)row.FindControl("lblSeqNumberLife1");
                        Label hdfQuestionID1 = (Label)row.FindControl("lblQuestionIDLife1");

                        RadioButtonList rbtnlAnswer2 = (RadioButtonList)row.FindControl("rbtnlAnswerLife2");
                        Label hdfSeqNumber2 = (Label)row.FindControl("lblSeqNumberLife2");
                        Label hdfQuestionID2 = (Label)row.FindControl("lblQuestionIDLife2");

                        RadioButtonList rbtnlAnswer3 = (RadioButtonList)row.FindControl("rbtnlAnswerLife3");
                        Label hdfSeqNumber3 = (Label)row.FindControl("lblSeqNumberLife3");
                        Label hdfQuestionID3 = (Label)row.FindControl("lblQuestionIDLife3");

                        RadioButtonList rbtnlAnswer4 = (RadioButtonList)row.FindControl("rbtnlAnswerLife4");
                        Label hdfSeqNumber4 = (Label)row.FindControl("lblSeqNumberLife4");
                        Label hdfQuestionID4 = (Label)row.FindControl("lblQuestionIDLife4");

                        RadioButtonList rbtnlAnswer5 = (RadioButtonList)row.FindControl("rbtnlAnswerLife5");
                        Label hdfSeqNumber5 = (Label)row.FindControl("lblSeqNumberLife5");
                        Label hdfQuestionID5 = (Label)row.FindControl("lblQuestionIDLife5");

                        RadioButtonList rbtnlAnswer6 = (RadioButtonList)row.FindControl("rbtnlAnswerLife6");
                        Label hdfSeqNumber6 = (Label)row.FindControl("lblSeqNumberLife6");
                        Label hdfQuestionID6 = (Label)row.FindControl("lblQuestionIDLife6");

                        string new_guid = "";

                        da_application_fp6.bl_app_answer_item_fp6 app_answer_item = new da_application_fp6.bl_app_answer_item_fp6();

                        //policy owner
                        if (i == 0)
                        {
                           
                            int answer = 0;
                            if (rbtnlAnswer1.SelectedIndex >= 0)
                            {
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID1.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber1.Text.Trim());
                                answer = Convert.ToInt32(rbtnlAnswer1.SelectedValue);
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer1.SelectedValue);
                                app_answer_item.Level = 0;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    lblMessageHealth.Text = "Saved answer fail.";
                                    return;
                                }
                            }
                           
                        }

                            //life insured 1
                        else if (i == 1)
                        {
                           
                            int answer = 0;
                            if (rbtnlAnswer2.SelectedIndex >= 0)
                            {
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID2.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber2.Text.Trim());
                                answer = Convert.ToInt32(rbtnlAnswer2.SelectedValue);
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer2.SelectedValue);
                                app_answer_item.Level = 1;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    lblMessageHealth.Text = "Saved answer fail.";
                                    return;
                                }
                            }
                           
                        }

                            //life insured 2
                        else if (i == 2)
                        {
                           
                            int answer = 0;
                            if (rbtnlAnswer3.SelectedIndex >= 0)
                            {
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID3.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber3.Text.Trim());
                                answer = Convert.ToInt32(rbtnlAnswer3.SelectedValue);
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer3.SelectedValue);
                                app_answer_item.Level = 2;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    lblMessageHealth.Text = "Saved answer fail.";
                                    return;
                                }
                            }
                           
                        }

                            //life insured 3
                        else if (i == 3)
                        {
                           
                            int answer = 0;
                            if (rbtnlAnswer4.SelectedIndex >= 0)
                            {
                                answer = Convert.ToInt32(rbtnlAnswer4.SelectedValue);
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID4.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber4.Text.Trim());
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer4.SelectedValue);
                                app_answer_item.Level = 3;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    lblMessageHealth.Text = "Saved answer fail.";
                                    return;
                                }
                            }
                           
                        }

                            //life insured 4
                        else if (i == 4)
                        {
                           
                            int answer = 0;
                            if (rbtnlAnswer5.SelectedIndex >= 0)
                            {
                                answer = Convert.ToInt32(rbtnlAnswer5.SelectedValue);
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID5.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber5.Text.Trim());
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer5.SelectedValue);
                                app_answer_item.Level = 4;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    lblMessageHealth.Text = "Saved answer fail.";
                                    return;
                                }
                            }
                           
                        }

                            //life insured 5
                        else if (i == 5)
                        {
                           
                            int answer = 0;
                            if (rbtnlAnswer6.SelectedIndex >= 0)
                            {
                                answer = Convert.ToInt32(rbtnlAnswer6.SelectedValue);
                                new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                app_answer_item.App_Answer_Item_ID = new_guid;
                                app_answer_item.App_Register_ID = app_id;
                                app_answer_item.Question_ID = hdfQuestionID6.Text.Trim();
                                app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber6.Text.Trim());
                                app_answer_item.Answer = answer;// Convert.ToInt32(rbtnlAnswer6.SelectedValue);
                                app_answer_item.Level = 5;
                                //Insert into Ct_App_Answer_Item

                                if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                {

                                }
                                else
                                {
                                    lblMessageHealth.Text = "Saved answer fail.";
                                    return;
                                }
                            }
                            
                        }
                    }
                }

            #endregion End Save Answer

                //reload data
                LoadBody(ViewState["appID"] + "");
                LoadAnswers(ViewState["appID"] + "");
            }
        }

        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function btnBodySave_Click in page application_form_fp6.aspx.cs" + ex.Message);
        }
    }
    protected void btnClearJobHistory_Click(object sender, EventArgs e)
    {
        ClearJobHistoryInfo();
    }
    protected void ddlRider_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if(txtInsuranceAmountRequired.Text.Trim()=="")
            {
                txtRiderSumInsured.Text = txtInsuranceAmountRequired.Text;
            }

            //auto generate sum insured for each rider
            if (txtInsuranceAmountRequired.Text.Trim() != "")
            {
                double life_sum_insured = 0;
                double riderSumInsured = 0.0;
                double premium = 0.0;
                double original_amount = 0.0;
                double rate = 0;

                int age = 0;
                int assure_year = 0;
                int assure_age = 0;
                int assure_plan = 0;
                int gender = -1;
                int pay_mode =Convert.ToInt32( ddlPremiumMode.SelectedValue.Trim());

                string product_id = "";
                string substring_product = "";

                if (txtTermInsurance.Text.Trim() != "")
                {
                    assure_plan = Convert.ToInt32(txtTermInsurance.Text.Trim());
                }

                life_sum_insured = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim());

                product_id = ddlTypeInsurancePlan.SelectedValue.Trim();
                substring_product = product_id.Substring(0, 3).ToUpper();

                age = Convert.ToInt32(txtAssureeAgeLife1.Text.Trim());

                DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
                int rowCount = tblPersonal.Rows.Count;

                if(txtRiderSumInsured.Text.Trim()!="")
                {
                    riderSumInsured = Convert.ToDouble(txtRiderSumInsured.Text);
                }

                //calculate pay year assure year pay up to age assure up to age
               My_View_State.assure_pay_year = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assure_plan, 65, 0);//set policy_insured_year =0 because it's new policy

               #region ADB
               if (ddlRider.SelectedValue.Trim() == "ADB")
                {
                        // ADB Premium
                        if (rowCount > 0)
                        {
                            var row = tblPersonal.Rows[0];
                            
                            if (substring_product == "NFP")
                            {
                                ddlPosition.Enabled = true;
                                ddlClassRate.Enabled = true;
                                BindPosition(ddlClassRate.SelectedValue.Trim(), ddlPosition);
                                txtRoundedAmount.Text = "";
                                txtOriginalAmount.Text = "";
                                rate = GetADBRate();

                                //txtRiderSumInsured.Text = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim()) * 2 + "";
                               
                                if (ddlClassRate.SelectedValue.Trim() != "")
                                {
                                    string premium_original_amount = "";
                                    char[] split = { '/' };
                                    //premium = GetADBPremium(ddlTypeInsurancePlan.SelectedItem.Text, Convert.ToInt32(ddlPremiumMode.SelectedValue), Convert.ToDouble(txtRiderSumInsured.Text), rate);
                                    premium_original_amount = GetADBPremium(product_id, Convert.ToInt32(ddlPremiumMode.SelectedValue), riderSumInsured, rate);
                                    string[] arr_prem = premium_original_amount.Split(split);
                                    if (arr_prem.Length > 0)
                                    {
                                        premium = Convert.ToDouble(arr_prem[0]);
                                        original_amount = Convert.ToDouble(arr_prem[1]);
                                    }
                                }
                            }
                            else if (substring_product == "FPP")
                            {
                                ddlPosition.Enabled = false;
                                ddlClassRate.Enabled = false;
                                rate = 0;
                               
                                ////check age
                                //if (txtAssureeAgeLife1.Text.Trim() != "")
                                //{
                                //    age = Convert.ToInt32(txtAssureeAgeLife1.Text.Trim());
                                //}
                               
                                if (age >= 18 && age <= 60)
                                {
                                    //get adb premium by class

                                    string str_class = "";
                                    string [] str;
                                    str = product_id.Split('/');
                                    str_class = "Class " + str[1] + "/" + str[1];

                                    assure_year = Convert.ToInt32(txtTermInsurance.Text.Trim());
                                    premium = da_application_fp6.GetADBPremiumFPPackage(str_class);
                                    if (life_sum_insured == 5000)
                                    {
                                        premium = Math.Ceiling(premium / 2);
                                    }
                                    original_amount = premium;
                                    
                                }

                            }

                            #region Study save
                            else if (substring_product == "SDS")
                            {
                                if (age >= 18 && age <= 60)
                                {
                                    assure_year = Convert.ToInt32(txtTermInsurance.Text.Trim());
                                    double[,] adb_study = new double[,] { { 0, 0 } };
                                    string[] sub_product = product_id.Split('/');

                                    //int rate_id = da_application_fp6.GetADBRate(ViewState["appID"] + "");
                                    //string class_name = "Class " + rate_id + "/" + product_id;
                                    //double class_rate = da_application_fp6.GetClassRate(class_name);

                                    string class_name = "";
                                    double class_rate=0; 

                                    if (sub_product.Length == 2)//normal study save
                                    {
                                        ddlPosition.Enabled = true;
                                        ddlClassRate.Enabled = true;
                                        BindPosition(ddlClassRate.SelectedValue.Trim(), ddlPosition);
                                        class_rate = GetADBRate();
                                        adb_study = da_application_study_save.study_save.GetADBPremium(product_id, pay_mode, riderSumInsured, class_rate);
                                    }
                                    else if (sub_product.Length > 2)// study save package, package protection, package maturity
                                    {
                                        class_name = "Class " + product_id;
                                        adb_study = da_application_study_save.study_save_package.GetADBPremium(class_name, pay_mode);
                                    }
                                    premium = adb_study[0, 0];
                                    original_amount = adb_study[0, 1];
                                }

                            }
                            #endregion
                           
                        
                    }
                   
                } //
               #endregion
               #region TPD
               else if (ddlRider.SelectedValue.Trim() == "TPD")
                {
                    //txtRiderSumInsured.Text = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim()) * 3 + "";
                    ddlClassRate.Enabled = false;
                    ddlPosition.Enabled = false;
                    txtADBRate.Text = "0";
                    txtRoundedAmount.Text = "";
                    txtOriginalAmount.Text = "";

                    //Check life insured age first
                    if (txtAssureeAgeLife1.Text.Trim() != "")
                    {
                        if (rowCount > 0)
                        {
                            var row = tblPersonal.Rows[0];

                            if (row["gender"].ToString().ToLower().Trim() == "male")
                            {
                                gender = 1;
                            }
                            else
                            {
                                gender = 0;
                            }

                            if (substring_product == "NFP")
                            {
                               
                                //age = Convert.ToInt32(txtAssureeAgeLife1.Text.Trim());

                                //assure_age = age + assure_plan;

                                //if (assure_age <= 65) //maximum age 65
                                //{
                                //    assure_year = assure_plan;

                                //}
                                //else
                                //{
                                //    assure_year = assure_plan - (assure_age - 65);
                                //}


                                //premium = GetTPDPremium("",
                                //                Convert.ToInt32(txtAssureeAgeLife1.Text.Trim()),
                                //                gender,
                                //                Convert.ToDouble(txtRiderSumInsured.Text.Trim()), Convert.ToInt32(ddlPremiumMode.SelectedValue));

                                string premium_original_amount = "";
                                char[] split = { '/' };
                                premium_original_amount = GetTPDPremium( product_id, age, My_View_State.assure_pay_year.Assure_Year, gender, riderSumInsured, Convert.ToInt32(ddlPremiumMode.SelectedValue));
                                string[] arr_prem = premium_original_amount.Split(split);

                                if (arr_prem.Length > 0)
                                {
                                    premium = Convert.ToDouble(arr_prem[0]);
                                    original_amount = Convert.ToDouble(arr_prem[1]);
                                }

                            }
                            else if (substring_product == "FPP")
                            {
                                if (age >= 18 && age <= 60)
                                {
                                    assure_year = Convert.ToInt32(txtTermInsurance.Text.Trim());

                                    //premium = da_application_fp6.GetTPDPremiumFPPackage("", assure_year, gender, 0);
                                    premium = da_application_fp6.GetTPDPremiumFPPackage( product_id, assure_year, gender, 0);
                                    if (life_sum_insured == 5000)
                                    {
                                        premium = Math.Ceiling(premium / 2);
                                    }
                                    original_amount = premium;

                                }
                            }
                            #region Study save
                            else if (substring_product == "SDS")
                            {
                                if (age >= 18 && age <= 60)
                                {
                                    assure_year = Convert.ToInt32(txtTermInsurance.Text.Trim());
                                    double[,] tpd_study = new double[,] { { 0, 0 } };
                                    string[] sub_product = product_id.Split('/');

                                    if (sub_product.Length == 2)//normal study save
                                    {
                                        tpd_study = da_application_study_save.study_save.GetTPDPremium(age, gender, product_id, assure_year, pay_mode, life_sum_insured);
                                    }
                                    else if (sub_product.Length > 2)// study save package, package protection, package maturity
                                    {
                                        tpd_study = da_application_study_save.study_save_package.GetTPDPremium(0, gender, product_id, assure_year, pay_mode);
                                    }
                                    premium = tpd_study[0, 0];
                                    original_amount = tpd_study[0, 1];
                                }
                                
                            }
                            #endregion


                        }
                    }
                    //if life insured age is empty
                    else
                    {
                        lblMessageInsurancePlan.Text = "Assuree Age is required.";
                    }


                }
               #endregion
               else if (ddlRider.SelectedValue.Trim() == "SPOUSE")
                {
                    txtRiderSumInsured.Text = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim()) * 0.5 + "";
                }
                else if (ddlRider.SelectedValue.Trim() == "CHILD")
                {//
                    txtRiderSumInsured.Text = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim()) * 0.25 + "";
                }

                txtADBRate.Text = rate + "";
                txtRiderPremium.Text = premium + "";
                txtOriginalAmount.Text = original_amount + "";
                txtRoundedAmount.Text = Math.Ceiling(original_amount) + "";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error while user select changed on rider dropdownlist, Detail: " + ex.Message);
            lblMessageInsurancePlan.Text = "Select rider error.";
        }
       
    }
    protected void btnRiderAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //check for varlid
            if (ddlRider.SelectedIndex == 0)
            {
                lblMessageInsurancePlan.Text = "Please select Rider.";
                return;
            }
            if (txtRiderSumInsured.Text.Trim() == "" || txtRiderSumInsured.Text.Trim() == "0")
            {
                lblMessageInsurancePlan.Text = "Please input Rider Sum Insured.";
                return;
            }
            if (ddlRider.SelectedValue.Trim() == "ADB" || ddlRider.SelectedValue.Trim() == "TPD")
            {
                if (ddlRiderPerson.SelectedIndex == 0)
                {
                    lblMessageInsurancePlan.Text = "Please select Apply To.";
                    return;
                }
                else
                {
                    if (ddlRiderPerson.SelectedValue.Trim() != "1")
                    {
                        lblMessageInsurancePlan.Text = "Rider [" + ddlRider.SelectedItem.Text.ToString() + "] is not support to [" + ddlRiderPerson.SelectedItem.Text.ToString() + "].";
                        return;
                    }
                }

                string appID = ViewState["appID"] + "";
                DataTable tblRider = (DataTable)ViewState["tblRider"];

                string productID = "";
                productID = ddlTypeInsurancePlan.SelectedValue.Trim();
                string product = "";
                product = productID.Substring(0, 3).ToUpper();

                if (ViewState["tblRiderRowIndex"] != null)
                { //update data in datatable

                    int index = Convert.ToInt32(ViewState["tblRiderRowIndex"] + "");
                    var row = tblRider.Rows[index];

                    row["level"] = ddlRiderPerson.SelectedValue.Trim(); ;
                    row["rider_type"] = ddlRider.SelectedValue.Trim();
                    row["sumInsured"] = txtRiderSumInsured.Text.Trim();
                    row["premium"] = txtRiderPremium.Text.Trim();
                    row["discount"] = "0";

                    if (ddlRider.SelectedValue.Trim() == "ADB")
                    {
                        
                        if (product == "FPP")// family protection package
                        {
                            row["rate"] = 0;
                            row["rate_id"] = 0;
                        }
                        else if (product == "NFP")
                        {
                            row["rate"] = txtADBRate.Text;
                            row["rate_id"] = ddlPosition.SelectedValue.Trim();
                        }
                        else if(product =="SDS")//study save
                        {
                            string[] arr_pro = productID.Split('/');
                            if (arr_pro.Length == 2)//normal
                            {
                                row["rate"] = txtADBRate.Text;
                                row["rate_id"] = ddlPosition.SelectedValue.Trim();
                            }
                            else if (arr_pro.Length == 3)//package
                            {
                                row["rate"] = 0;
                                row["rate_id"] = 0;
                            }
                        }
                        
                    }
                    else
                    {
                        row["rate"] = 0;
                        row["rate_id"] = 0;
                    }

                    if (txtOriginalAmount.Text.Trim() != "")
                    {
                        row["original_amount"] = Convert.ToDouble(txtOriginalAmount.Text);
                    }
                    else
                    {
                        row["original_amount"] = 0;
                    }
                    if (txtRoundedAmount.Text.Trim() != "")
                    {
                        row["rounded_amount"] = Convert.ToDouble(txtRoundedAmount.Text);
                    }
                    else
                    {
                        row["rounded_amount"] = 0;
                    }

                    //add assure, pay year
                    row["Age_Insure"] = My_View_State.assure_pay_year.Age_Insure;
                    row["Pay_Year"] = My_View_State.assure_pay_year.Pay_Year;
                    row["Pay_Up_To_Age"] = My_View_State.assure_pay_year.Pay_Up_To_Age;
                    row["Assure_Year"] = My_View_State.assure_pay_year.Assure_Year;
                    row["Assure_Up_To_Age"] = My_View_State.assure_pay_year.Assure_Up_To_Age;
                    //row["Effective_Date"] = Helper.FormatDateTime(txtDateEntry.Text.Trim());
                    row["Effective_Date"] = txtDateEntry.Text.Trim();

                    tblRider.AcceptChanges();
                    ViewState["tblRiderRowIndex"] = null;
                    btnRiderAdd.Text = "Add";


                }


                else
                { //add new data into datatable

                    #region check duplicate rider
                    int row_count = 0;
                    row_count = tblRider.Rows.Count;
                    bool exist = false;
                    if (row_count > 0)
                    {
                        foreach (DataRow r in tblRider.Rows)
                        {
                            if (r["rider_type"].ToString().Trim().ToUpper() == ddlRider.SelectedValue.Trim().ToUpper())
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (exist)
                        {
                            lblMessageInsurancePlan.Text = "Rider is already exist.";
                            return;
                        }

                    }
                    #endregion

                    DataRow row = tblRider.NewRow();
                    row["rider_id"] = "";
                    row["app_register_id"] = appID;
                    row["product_id"] = productID;
                    row["level"] = ddlRiderPerson.SelectedValue.Trim(); ;
                    row["rider_type"] = ddlRider.SelectedValue.Trim();
                    row["sumInsured"] = txtRiderSumInsured.Text.Trim();
                    row["premium"] = txtRiderPremium.Text.Trim();
                    row["discount"] = "0";


                    if (ddlRider.SelectedValue.Trim() == "ADB")
                    {
                      
                        if (product == "FPP")// family protection package
                        {
                            row["rate"] = 0;
                            row["rate_id"] = 0;
                        }
                        else if (product == "NFP")
                        {
                            row["rate"] = txtADBRate.Text;
                            row["rate_id"] = ddlPosition.SelectedValue.Trim();
                        }
                        else if (product == "SDS")//study save
                        {
                            string[] arr_pro = productID.Split('/');
                            if (arr_pro.Length == 2)//normal
                            {
                                row["rate"] = txtADBRate.Text;
                                row["rate_id"] = ddlPosition.SelectedValue.Trim();
                            }
                            else if (arr_pro.Length == 3)//package
                            {
                                row["rate"] = 0;
                                row["rate_id"] = 0;
                            }
                        }
                    }
                    else
                    {
                        row["rate"] = 0;
                        row["rate_id"] = 0;
                    }

                    if (txtOriginalAmount.Text.Trim() != "")
                    {
                        row["original_amount"] = Convert.ToDouble(txtOriginalAmount.Text);
                    }
                    else
                    {
                        row["original_amount"] = 0;
                    }
                    if (txtRoundedAmount.Text.Trim() != "")
                    {
                        row["rounded_amount"] = Convert.ToDouble(txtRoundedAmount.Text);
                    }
                    else
                    {
                        row["rounded_amount"] = 0;
                    }

                    //add assure, pay year
                    row["Age_Insure"] = My_View_State.assure_pay_year.Age_Insure;
                    row["Pay_Year"] = My_View_State.assure_pay_year.Pay_Year;
                    row["Pay_Up_To_Age"] = My_View_State.assure_pay_year.Pay_Up_To_Age;
                    row["Assure_Year"] = My_View_State.assure_pay_year.Assure_Year;
                    row["Assure_Up_To_Age"] = My_View_State.assure_pay_year.Assure_Up_To_Age;
                    //row["Effective_Date"] = Helper.FormatDateTime(txtDateEntry.Text.Trim());
                    row["Effective_Date"] = txtDateEntry.Text.Trim();

                    tblRider.Rows.Add(row);
                }

                #region Total premium
                /*
                double totalRiderPremium = 0.0;
                double riderDiscountAmount = 0.0;
                double totalPremium = 0.0;

                for (int i = 0; i < tblRider.Rows.Count; i++)
                {
                    var row = tblRider.Rows[i];
                    totalRiderPremium += Convert.ToDouble(row["premium"].ToString());
                }

                txtRiderTotalPreium.Text = totalRiderPremium + "";

                if (txtRiderDiscountAmount.Text.Trim() != "")
                {
                    riderDiscountAmount = Convert.ToDouble(txtRiderDiscountAmount.Text.Trim());
                }

                if (txtTotalPremium.Text.Trim() != "")
                {
                    totalPremium = Convert.ToDouble(txtTotalPremium.Text.Trim());
                }

                txtRiderTotalPremiumAfterDiscount.Text = totalRiderPremium - riderDiscountAmount + "";

                double premiumDiscountAmount = 0.0;
                if(txtDiscountAmount.Text.Trim()!="")
                {
                    premiumDiscountAmount = Convert.ToDouble(txtDiscountAmount.Text);
                }

                txtTotalPremiumAfterDiscount.Text = Convert.ToDouble(txtRiderTotalPremiumAfterDiscount.Text) + totalPremium - premiumDiscountAmount + "";

                ViewState["tblRider"] = tblRider;
                //bind data into gridview
                gvRider.DataSource = tblRider;
                gvRider.DataBind();*/
                #endregion Total premium
                TotalPremiumAndRider();
                lblMessageInsurancePlan.Text = "";
            }
                //Open new page for riders
            else if (ddlRider.SelectedValue.Trim() == "SPOUSE" || ddlRider.SelectedValue.Trim() == "CHILD")
            {
                 List<da_application_fp6.AppPlanInfo> planList = new List<da_application_fp6.AppPlanInfo>();
                   
                planList= da_application_fp6.GetAppPlanInfo(ViewState["appID"]+"");
                if (planList.Count > 0)
                {
                    Response.Redirect("application_form_fp6_rider.aspx?application_register_id=" + planList[0].ApplicationID);
                }
                else
                {
                    lblMessageInsurancePlan.Text = "Please save this application before adding riders (Spouse/Child).";
                    return;
                }
            
            }
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error btnRiderAdd_Click in page application_form_fp6.aspx.cs, Detail: " + ex.Source);
            lblMessageInsurancePlan.Text = "Added rider error.";
        }
        

    }
    protected void btnRiderClear_Click(object sender, EventArgs e)
    {
        ClearRiderInfo();
    }
    private void ClearRiderInfo()
    {
        txtRiderPremium.Text = "";
        //txtRiderSumInsured.Text = "";

        ddlRider.SelectedIndex=0;
        ddlRiderPerson.SelectedIndex = 0;

        ddlClassRate.SelectedIndex = 0;
        txtOriginalAmount.Text = "";
        txtRoundedAmount.Text = "";
       

        btnRiderAdd.Text = "Add";
        gvRider.SelectedIndex = -1;
    }
    private void ClearRider()
    {
        ClearRiderInfo();

        DataTable tblRider = (DataTable)ViewState["tblRider"];
        tblRider.Clear();
        gvRider.DataSource = tblRider;
        gvRider.DataBind();
        ViewState["tblRider"] = tblRider;
    }
    protected void gvRider_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        string appID = ViewState["appID"] + "";
        DataTable tblRider = (DataTable)ViewState["tblRider"];

        if (tblRider.Rows.Count > 0)
        {

            int index = e.NewSelectedIndex;
            if (tblRider.Rows[index]["level"].ToString().Trim() == "1")
            {

                btnRiderAdd.Text = "Update";
                Helper.SelectedDropDownListIndex("VALUE", ddlRider, tblRider.Rows[index]["rider_type"].ToString().Trim());
                Helper.SelectedDropDownListIndex("VALUE", ddlRiderPerson, tblRider.Rows[index]["level"].ToString().Trim());
                txtRiderSumInsured.Text = tblRider.Rows[index]["sumInsured"].ToString().Trim();
                txtRiderPremium.Text = tblRider.Rows[index]["premium"].ToString().Trim();

                txtOriginalAmount.Text = tblRider.Rows[index]["original_amount"].ToString().Trim();
                txtRoundedAmount.Text = tblRider.Rows[index]["rounded_amount"].ToString().Trim();

                string product_id = "";
                product_id = ddlTypeInsurancePlan.SelectedValue.Trim();
                product_id = product_id.Substring(0, 3).ToUpper();

                //ADB
                if (tblRider.Rows[index]["rider_type"].ToString().Trim() == "ADB")
                {
                    string cate = "";
                    int rateID = 0;
                    double rate = 0.0;

                    if (product_id == "NFP")
                    {
                        ddlClassRate.Enabled = true;
                        ddlPosition.Enabled = true;

                        //rateID it acts as position
                        rateID = Convert.ToInt32(tblRider.Rows[index]["rate_id"].ToString().Trim());
                        rate = Convert.ToDouble(tblRider.Rows[index]["rate"].ToString().Trim());
                        cate = da_application_fp6.GetCategoryByNo(rateID);

                        txtADBRate.Text = rate + "";

                        BindPosition(cate, ddlPosition);

                        Helper.SelectedDropDownListIndex("VALUE", ddlClassRate, cate);
                        Helper.SelectedDropDownListIndex("VALUE", ddlPosition, rateID + "");
                    }
                    else if (product_id == "FPP")//family protection package
                    {
                        ddlClassRate.Enabled = false;
                        ddlPosition.Enabled = false;

                        rateID = Convert.ToInt32(tblRider.Rows[index]["rate_id"].ToString().Trim());
                        rate = Convert.ToDouble(tblRider.Rows[index]["rate"].ToString().Trim());
                        cate = da_application_fp6.GetCategoryByNo(rateID);

                        txtADBRate.Text = rate + "";

                        BindPosition(cate, ddlPosition);

                        ddlClassRate.SelectedIndex = 0;
                        ddlPosition.SelectedIndex = 0;
                    }
                    #region Study Save
                    else if (product_id == "SDS")
                    {
                        string[] arr_product = ddlTypeInsurancePlan.SelectedValue.Trim().Split('/');
                        #region study save normal
                        if (arr_product.Length == 2)
                        {
                            ddlClassRate.Enabled = true;
                            ddlPosition.Enabled = true;

                            rateID = Convert.ToInt32(tblRider.Rows[index]["rate_id"].ToString().Trim());
                            rate = Convert.ToDouble(tblRider.Rows[index]["rate"].ToString().Trim());
                            cate = da_application_fp6.GetCategoryByNo(rateID);

                            txtADBRate.Text = rate + "";

                            BindPosition(cate, ddlPosition);

                            Helper.SelectedDropDownListIndex("VALUE", ddlClassRate, cate);
                            Helper.SelectedDropDownListIndex("VALUE", ddlPosition, rateID + "");
                        }
                        #endregion

                        #region study save package
                        else if (arr_product.Length > 2)
                        {
                            ddlClassRate.Enabled = false;
                            ddlPosition.Enabled = false;

                            rateID = Convert.ToInt32(tblRider.Rows[index]["rate_id"].ToString().Trim());
                            rate = Convert.ToDouble(tblRider.Rows[index]["rate"].ToString().Trim());
                            cate = da_application_fp6.GetCategoryByNo(rateID);

                            txtADBRate.Text = rate + "";

                            BindPosition(cate, ddlPosition);

                            ddlClassRate.SelectedIndex = 0;
                            ddlPosition.SelectedIndex = 0;
                        }
                        #endregion
                    }
                    #endregion


                }
                //TPD
                else
                {
                    txtADBRate.Text = "";
                    ddlClassRate.Enabled = false;
                    ddlPosition.Enabled = false;
                    ddlPosition.SelectedIndex = 0;
                    ddlClassRate.SelectedIndex = 0;
                }


                ViewState["tblRiderRowIndex"] = index;
            }
        }
        else
        {
            lblMessageInsurancePlan.Text = "No row selected.";
        }
    }
    protected void gvRider_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string appID = ViewState["appID"] + "";
        DataTable tblRider = (DataTable)ViewState["tblRider"];

        if (tblRider.Rows.Count > 0)
        {
            
            int index = e.RowIndex;
            if (tblRider.Rows[index]["rider_id"].ToString().Trim() != "") 
            {
                ViewState["tblRiderID"] += tblRider.Rows[index]["rider_id"].ToString().Trim() + ",";
            }
            tblRider.Rows[index].Delete();
            tblRider.AcceptChanges();
            ViewState["tblRider"] = tblRider;

           // gvRider.DataSource = tblRider;
            //gvRider.DataBind();
            TotalPremiumAndRider();
        }
        else
        {
            lblMessageInsurancePlan.Text = "No row selected.";
        }
    }
    protected void txtRiderDiscountAmount_TextChanged(object sender, EventArgs e)
    {
        //double totalRiderPremium = 0.0;
        //double discountAmount = 0.0;

        //if (txtRiderTotalPreium.Text.Trim() == "")
        //{
        //    totalRiderPremium = 0.0;
        //}
        //else
        //{
        //    totalRiderPremium = Convert.ToDouble(txtRiderTotalPreium.Text.Trim());
        //}
        //if (txtRiderDiscountAmount.Text.Trim() == "")
        //{
        //    discountAmount = 0.0;
        //}
        //else
        //{
        //    discountAmount = Convert.ToDouble(txtRiderDiscountAmount.Text.Trim());
        //}
        //txtRiderTotalPremiumAfterDiscount.Text = (totalRiderPremium - discountAmount) + "";



        //txtTotalPremiumAfterDiscount.Text = Convert.ToDouble(txtTotalPremium.Text) + Convert.ToDouble(txtRiderTotalPremiumAfterDiscount.Text) - Convert.ToDouble(txtDiscountAmount.Text)  + "";
        CalcDiscount();
    }
    
    private bool SavePremPay( string prem_pay_id, string app_register_id, DateTime pay_date, int is_init_pay, double amount, double original_amount, double rounded_amount, string created_by, string created_note, DateTime created_on, int level)
    {

        bool status = false;
        try
        {
            da_application_fp6.bl_app_prem_pay_fp6 app_prem_pay = new da_application_fp6.bl_app_prem_pay_fp6();

            app_prem_pay.App_Prem_Pay_ID = prem_pay_id;
            app_prem_pay.App_Register_ID = app_register_id;
            app_prem_pay.Pay_Date = pay_date;
            app_prem_pay.Is_Init_Payment = is_init_pay;
            app_prem_pay.Amount = amount;
            app_prem_pay.Original_Amount = original_amount;
            app_prem_pay.Created_By = created_by;
            app_prem_pay.Created_Note = created_note;
            app_prem_pay.Created_On = created_on;
            app_prem_pay.Rounded_Amount = rounded_amount;
            app_prem_pay.Level = level;

            if (da_application_fp6.InsertAppPremPayFP6(app_prem_pay))
            {
                status = true;
            }
            else
            {
                status = false;
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function SavePremPay in page application_form_fp6.aspx. Detail:" + ex.Message);
            status = false;
        }
        
        return status;
    }

    private string GetTPDPremium(string productId, int age, int assure_year, int gender, double sumInsured, int paymode)
    {
       
        string premium_original_amount = "";
        try
        {
            premium_original_amount = da_application_fp6.GetTPDPremium(sumInsured, productId, gender, age, assure_year, paymode);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function GetTPDPremium in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }

        return premium_original_amount;
    }
    private string GetADBPremium(string productID, int payMode, double sumInsured, double rate)
    {
        string  premium_original_amount = "";
        try
        {
            premium_original_amount = da_application_fp6.GetADBPremium(sumInsured, productID, rate, payMode);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function GetADBPremium in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }

        return premium_original_amount;
    }

    private  bool CheckVarlidAge(int age, int level)
    {
        bool varlid =false;

        if (level==0 || level== 1) //policy owner and life 1 (spouse)
        {
            if (age  > 55 && age < 18)
            {
                varlid = false;
            }
            else
            {
                varlid = true;
            }
        }
        else
        {//life 2 - 5 (child)
            if (age  > 21 && age < 1)
            {
                varlid = false;
            }
            else
            {
                varlid = true;
            }
        }
            
        return varlid;
        
    }
    private bool Check_ADB_TPD_Varlid_Age(int age)
    {
        bool varlid = false;

        if (age > 60 && age < 18)
        {
            varlid = false;
        }
        else
        {
            varlid = true;
        }

        return varlid;

    }
    protected void DisableControls(Control parent, bool State)
    {
        foreach (Control c in parent.Controls)
        {
            if (c is DropDownList)
            {
                ((DropDownList)(c)).Enabled = State;
            }
            else if (c is TextBox)
            {
                ((TextBox)(c)).Enabled = State;
            }
            else if (c is Button)
            {
                ((Button)(c)).Enabled = State;
            }

            DisableControls(c, State);
        }
    }
    protected void ddlClassRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRider_SelectedIndexChanged(sender, e);
    }
    protected void btnCancelApplication_Click(object sender, EventArgs e)
    {
        mCancelApplication.Show();
        if (txtCancelApplicationNumber.Text.Trim() == "")
        {
            lblMessageCancelApplication.Text = "Application number is required.";
            return;
        }
        else
        {
            bl_app_register_cancel app_register_cancel = new bl_app_register_cancel();
            app_register_cancel.App_Register_ID = ViewState["appID"] + ""; ;
            app_register_cancel.Created_By = ViewState["userName"] + ""; ;
            app_register_cancel.Created_Note = txtCancelNote.Text.Trim();
            app_register_cancel.Created_On = DateTime.Now;
            if (da_application.InsertAppRegisterCancel(app_register_cancel))
            {
                lblMessageCancelApplication.Text = "Application is canceled successfully.";
            }
            else
            {
                lblMessageCancelApplication.Text = "Application is canceled fail, please contact your system administrator.";
            }
        }
    }

    protected void ddlRelationShip_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRelationShip.SelectedValue.Trim() == "OTHERS")
        {
            txtOthers.Visible = true;
            txtOthers.Focus();
        }
        else
        {
            txtOthers.Visible = false;
        }
    }

    protected string VarlidPage()
    {
        string errorMessage = "";
        try
        {
            #region Application Detail
            if (txtApplicationNumber.Text.Trim() == "")
            {
                errorMessage = "Please fill Application Number.";
                return errorMessage;
            }
            if (ddlChannel.SelectedIndex == 0)
            {
                errorMessage = "Please select Chanel.";
                return errorMessage;
            }
            if (ddlCompany.SelectedValue.Trim() == "")
            {
                errorMessage = "Please select Company.";
                return errorMessage;
            }
            if (txtPaymentCode.Text.Trim() == "")
            {
                errorMessage = "Please fill Payment Code.";
                return errorMessage;
            }
            if (txtDateEntry.Text.Trim() == "")
            {
                errorMessage = "Please fill Date of Entry.";
                return errorMessage;
            }
            if(txtDateSignature.Text.Trim()=="")
            {
                errorMessage = "Please fill Date of Signature.";
                return errorMessage;
            }
            if (txtMarketingCode.Text.Trim() == "")
            {
                errorMessage = "Please fill Marketing Code.";
                return errorMessage;
            }
            if (txtMarketingName.Text.Trim() == "")
            {
                errorMessage = "Please fill Marketing Name.";
                return errorMessage;
            }
            #endregion End Application Detail

            #region Personal Information
            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            if (tblPersonal.Rows.Count == 0)
            {
                return "[Personal Information]: Personal information cannot be blank.";
            }
            #endregion End Personal Information
            #region Mailling and address
            //policy owner
            if (txtAddress1Life1.Text.Trim() == "")
            {
                return "[Mailling and Address], Please fill Address of policy owner.";
            }
            if (txtCityLife1.Text.Trim() == "")
            {
                return "[Mailling and Address], Please fill Province/City of policy owner.";
            }
            if (ddlCountryLife1.SelectedIndex == 0)
            {
                return "[Mailling and Address], Please select Country of policy owner.";
            }

            //if (txtTelephoneLife1.Text.Trim() == "")
            //{
            //    return "[Mailling and Address], Please fill Tel of policy owner.";
            //}

            if (txtMobilePhoneLife1.Text.Trim() == "")
            {
                return "[Mailling and Address], Please fill Mobile Phone of policy owner.";
            }
            //life 1
            if (txtAddress1Life2.Text.Trim() == "")
            {
                return "[Mailling and Address], Please fill Address of life insure 1.";
            }
            if (txtCityLife2.Text.Trim() == "")
            {
                return "[Mailling and Address], Please fill Province/City of life insure 1.";
            }
            if (ddlCountryLife2.SelectedIndex == 0)
            {
                return "[Mailling and Address], Please select Country of life insure 1.";
            }
            //if (txtTelephoneLife2.Text.Trim() == "")
            //{
            //    return "[Mailling and Address], Please fill Tel of life insure 1.";
            //}
            if (txtMobilePhoneLife2.Text.Trim() == "")
            {
                return "[Mailling and Address], Please fill Mobile Phone of life insure 1.";
            }
            #endregion End Mailling and address
            #region Job History
            DataTable tblJobHistory = (DataTable)ViewState["tblJobHistory"];
            if (tblJobHistory.Rows.Count == 0)
            {
                return "[Job History]: Job History cannot be blank.";
            }
            #endregion End Job History
            #region Insurance Plan
            if (ddlTypeInsurancePlan.SelectedIndex == 0)
            {
                return "[Insurance Plan]: please select Type of Insurance Plan.";
            }
            if (txtInsuranceAmountRequired.Text.Trim() == "")
            {
                return "[Insurance Plan]: please fill Insurance Amount Required.";
            }
            if (txtAssureeAgeLife1.Text.Trim() == "")
            {
                return "[Insurance Plan]: please fill Assuree Age.";
            }
            #endregion End Insurance Plan
            #region Beneficiaries
            DataTable tblBenefit = (DataTable)ViewState["tblBenifit"];
            if (tblBenefit.Rows.Count == 0)
            {
                return "[Beneficiaries]: Beneficiaries cannot be blank.";
            }
            else
            { 
                //total percentage
                double total = 0;
                try
                {
                    foreach (DataRow row in tblBenefit.Rows)
                    {
                        total = total+ Convert.ToDouble(row["Percentage"].ToString());
                    }
                    if (total > 100 || total < 100)
                    {
                        return "[Beneficiaries]: Total Percentage must be equal to 100%";
                    }
                }
                catch (Exception ex)
                {
                    Log.AddExceptionToLog("Error fucntion [VarlidPage] in page [application_form_fp6.aspx.cs], system try to total percentage, Detail: " + ex.Message);
                    return "[Beneficiaries]: Beneficiaries cannot be saved, please contact your system administrator.";
                }
               
            }
            #endregion End Beneficiaries
            #region Miscellaneolus and Health Details
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            if (tblBody.Rows.Count == 0)
            {
                return "[Miscellaneolus and Health Details]: Body Details cannot be blank.";
            }
            #endregion End Miscellaneolus and Health Details
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function VarlidPage in page: application_form_fp6.aspx.cs, Detail: " + ex.Message);
            errorMessage = "Application saved fail, please contact your system administrator.";
        }
        return errorMessage;
    }

    //Total rider spouse and all kids
    private double GetTotalRiderSpouseKids(string appID)
    {
        double total = 0.0;
        try
        {
            DataTable tblPremiumDetail = new DataTable();
            tblPremiumDetail = da_application_fp6.GetDataTable("SP_Get_App_Premium_Detail_By_App_Register_ID", appID);
                     
            //total premium 
            double totalPremium = 0.0;
            foreach (DataRow row in tblPremiumDetail.Rows)
            {
                totalPremium = totalPremium + Convert.ToDouble(row["premium"].ToString());
            }
            //initialize to textbox
            total = totalPremium ;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function TotalRiderSpouseKids in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }
        return total;
    }
    protected void lbtnShowRiderDetial_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ViewState["appID"]+""))
        {

            string url = "application_form_fp6_rider.aspx?application_register_id=" + ViewState["appID"] + "";
            Response.Redirect(url);
        }
    }
    private void CalcDiscount()
    {
        double lifeDiscount = 0.0;
        double riderDiscount = 0.0;
        double lifeAfterDiscount = 0.0;
        double riderAfterDiscount = 0.0;

        try
        {
            if (txtDiscountAmount.Text.Trim() != "")
            {
                lifeDiscount = Convert.ToDouble(txtDiscountAmount.Text.Trim());
            }
            if (txtRiderDiscountAmount.Text.Trim() != "")
            {
                riderDiscount = Convert.ToDouble(txtRiderDiscountAmount.Text.Trim());
            }

            lifeAfterDiscount = Convert.ToDouble(txtTotalPremium.Text.Trim()) - lifeDiscount;
            riderAfterDiscount = Convert.ToDouble(txtRiderTotalPreium.Text.Trim()) - riderDiscount;


            txtRiderTotalPremiumAfterDiscount.Text = riderAfterDiscount + "";

            txtTotalPremiumAfterDiscount.Text = lifeAfterDiscount + riderAfterDiscount + "";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [CalcDiscount] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
        }

        
        
    }
    protected void gvPersonalInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#00BFFF'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

        }
    }
    private double GetADBRate()
    {
        double rate = 0.0;
        try
        {
            if (ddlClassRate.SelectedIndex > 0 && ddlPosition.SelectedIndex > 0)
            {
                string class_name = "";
                //get class name
                class_name = "Class " + da_application_fp6.GetADBRate(ddlClassRate.SelectedValue.Trim(), ddlPosition.SelectedItem.Text.Trim());
               //get rate base on class name
                rate = da_application_fp6.GetClassRate(class_name);
            }
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetADBRate] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            lblMessageInsurancePlan.Text = "Get ADB Rate fail, please contact your system administrator.";
        }
       
        return rate;
    }
    private void BindCategories(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(".", ""));
      
        ArrayList arrList=  da_application_fp6.GetADBCategories();
        if (arrList.Count > 0)
        {
            for (int i = 0; i < arrList.Count; i++)
            {
                ddl.Items.Add(new ListItem(arrList[i].ToString(),arrList[i].ToString()));
            }
            
        }
    }

    private void BindPosition(string category, DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(".", ""));

        ArrayList arrList = da_application_fp6.GetPosition(category);
        if (arrList.Count > 0)
        {
            for (int i = 0; i < arrList.Count; i++)
            {
                int value=0;
                string text = "";
                char[] c = new char[] {','};
                string[] str = arrList[i].ToString().Split(c);
                value = Convert.ToInt32(str[1]);
                text = str[0];
                ddl.Items.Add(new ListItem(text, value+""));
            }

        }
    }

    protected void ddlPosition_SelectedIndexChanged(object sender, EventArgs e)
    {
        Double adbRate= GetADBRate();
        double riderSumInsured = 0.0;
        double premium = 0.0;
        double original_amount = 0.0;

        if (txtRiderSumInsured.Text.Trim() != "")
        {
            riderSumInsured = Convert.ToDouble(txtRiderSumInsured.Text);
        }
        string original_premium_amount = "";
        char[] split = { '/'};
    
        original_premium_amount = GetADBPremium(ddlTypeInsurancePlan.SelectedValue.Trim(), Convert.ToInt32(ddlPremiumMode.SelectedValue), riderSumInsured, adbRate);
        string[] arr = original_premium_amount.Split(split);
        if (arr.Length > 0)
        {
            premium = Convert.ToDouble(arr[0]);
            original_amount = Convert.ToDouble(arr[1]);
        }
        txtRiderPremium.Text = "" + premium;
        txtOriginalAmount.Text = "" + original_amount;
        txtRoundedAmount.Text = "" + Math.Ceiling(original_amount);
        txtADBRate.Text = adbRate + "";

      
    }
    private void TotalPremiumAndRider()
    {
        //total premium 
        double totalRiderPremium = 0.0;
        double riderDiscountAmount = 0.0;
        double totalPremium = 0.0;
        DataTable tblRider = (DataTable)ViewState["tblRider"];
        try
        {
            for (int i = 0; i < tblRider.Rows.Count; i++)
            {
                var row = tblRider.Rows[i];
                totalRiderPremium += Convert.ToDouble(row["premium"].ToString());
            }

            

            if (txtRiderDiscountAmount.Text.Trim() != "")
            {
                riderDiscountAmount = Convert.ToDouble(txtRiderDiscountAmount.Text.Trim());
            }

            if (txtTotalPremium.Text.Trim() != "")
            {
                totalPremium = Convert.ToDouble(txtTotalPremium.Text.Trim());
            }

            //spouse and kids
           totalRiderPremium += GetTotalRiderSpouseKids(ViewState["appID"] + "");

           txtRiderTotalPreium.Text = totalRiderPremium + "";
            txtRiderTotalPremiumAfterDiscount.Text = totalRiderPremium - riderDiscountAmount + "";

            double premiumDiscountAmount = 0.0;
            if (txtDiscountAmount.Text.Trim() != "")
            {
                premiumDiscountAmount = Convert.ToDouble(txtDiscountAmount.Text);
            }

            txtTotalPremiumAfterDiscount.Text = Convert.ToDouble(txtRiderTotalPremiumAfterDiscount.Text) + totalPremium - premiumDiscountAmount + "";

            ViewState["tblRider"] = tblRider;
            //bind data into gridview
            gvRider.DataSource = tblRider;
            gvRider.DataBind();
        }
        catch (Exception ex)
        {
            
            Log.AddExceptionToLog("Error function [TotalPremiumAndRider] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
        }
        

    }
    protected void imgbtnPaste_Click(object sender, ImageClickEventArgs e)
    {
        txtAddress1Life2.Text = txtAddress1Life1.Text;
        txtAddress2Life2.Text = txtAddress2Life1.Text;
        txtCityLife2.Text = txtCityLife1.Text;
        if (ddlCountryLife1.SelectedIndex > 0)
        {
            Helper.SelectedDropDownListIndex("TEXT", ddlCountryLife2, ddlCountryLife1.SelectedItem.Text.Trim());
        }
        txtZipCodeLife2.Text = txtZipCodeLife1.Text;
        txtTelephoneLife2.Text = txtTelephoneLife1.Text;
        txtMobilePhoneLife2.Text = txtMobilePhoneLife1.Text;
        txtEmailLife2.Text = txtEmailLife1.Text;
       
    }
    protected void txtRiderSumInsured_TextChanged(object sender, EventArgs e)
    {
        if (txtRiderSumInsured.Text.Trim() != "")
        {
            ddlRider_SelectedIndexChanged(sender, e);
        }
    }
    protected void imgbtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ddlRider_SelectedIndexChanged(sender, e);
    }

    void ClearBenefitTextBoxes()
    {
        txtBenefitIDNo.Text = "";
        txtBenefitName.Text = "";
        ddlBenefitIDType.SelectedIndex = 0;
        ddlBenefitRelation.SelectedIndex = 0;
        txtBenefitSharePercentage.Text = "";


    }
    void POPUP()
    {
        txtMessage.Text = "Do you want to key in rider application form?";
        ScriptManager.RegisterStartupScript(upApplictionNew, upApplictionNew.GetType(), "none",
               "<script> $('#ModalMessage').modal('show'); </script>", false);
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
       // Log.AddExceptionToLog("User [" + ViewState["userName"]+"] did not key in rider application form.");
    }
    protected void btnOpenRiderForm_Click(object sender, EventArgs e)
    {
        if (ViewState["appID"] != null && ViewState["appID"] != "" && ViewState["appID"] != "null")
        {
            string url = "application_form_fp6_rider.aspx?application_register_id=" + ViewState["appID"] + "";
            Response.Redirect(url);
        }
       
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        
        //get policy id by app_id
        string query = "select Policy_ID  from Ct_App_Policy where App_Register_ID='" + ViewState["appID"] + "'";
      DataTable tbl=  DataSetGenerator.Get_Data_Soure(query);
      if (tbl.Rows.Count > 0)
      {
          Session["SS_POLICY_ID"] = tbl.Rows[0][0].ToString();
          Response.Redirect("add_rider.aspx");
      }

       
    }

    class My_View_State
    {
        public static da_application_fpp.Cal_Age_Assure_Pay_Year assure_pay_year { get; set; }
       
    }
    protected void ddlPremiumMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string plan = "";
        //plan = ddlTypeInsurancePlan.SelectedValue.Trim().Substring(0, 3);

        //if (plan.ToUpper() == "NFP")
        //{
        //    CalculatePremium();
        //}
        //else if (plan.ToUpper() == "FPP")
        //{
        //    CalculatePremiumPackage();
        //}

        imgRecalculation_Click(null, null);
    }
}