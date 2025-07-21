using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class Pages_Business_application_form_fp : System.Web.UI.Page
{
    //Page Load Event
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

            hdfDateEntry.Value = String.Format("{0:dd/MM/yyyy}", System.DateTime.Today);

            //diabled enter key on textbox
            txtIDNumberLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtDateSignature.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameKhLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameEngLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameEngLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameFatherLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameKhLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameFatherLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameMotherLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameMotherLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPreviousSurnameLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPreviousFirstnameLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtDateBirthLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAddress1Life1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAddress2Life1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtCityLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtZipCodeLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtTelephoneLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtMobilePhoneLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtEmailLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtNameEmployerLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtNatureBusinessLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtRoleAndResponsibilityLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtCurrentPositionLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAnualIncomeLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtInsuranceAmountRequired.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPremiumAmount.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBenefitNote.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtHeightLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtWeightLife1.Attributes.Add("onkeypress", "return event.keyCode!=13");

            txtHeightLife2.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtWeightLife2.Attributes.Add("onkeypress", "return event.keyCode!=13");

            txtBenefitName.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBenefitIDNo.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBenefitSharePercentage.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtLoanEffectiveDate.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtInterest.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtTermLoan.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtOutstandingLoanAmount.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPaymentCode.Attributes.Add("onkeypress", "return event.keyCode!=13");

            //Modal textboxes
            txtCancelNote.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtIDCardNoSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtLastNameSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtApplicationNumberSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAgentName.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtApplicationNumberModal.Attributes.Add("onkeypress", "return event.keyCode!=13");


        }
    }

    //Rollback
    private static void Rollback(string app_id)
    {
        da_application.DeleteAppAnswerItem(app_id);
        da_application.DeleteAppLifeProduct(app_id);
        da_application.DeleteAppInfoBody(app_id);
        da_application.DeleteAppLoan(app_id);
        da_application.DeleteAppJobHistory(app_id);
        da_application.DeleteAppInfoAddress(app_id);
        da_application.DeleteAppInfoContact(app_id);

        da_application.DeleteAppInfoPerson(app_id);
        da_application.DeleteAppPremPay(app_id);
        da_application.DeleteAppBenefitItem(app_id);
        da_application.DeleteAppPremiumDiscount(app_id);
        da_application.DeleteAppDiscount(app_id);
        da_application.DeleteAppInfo(app_id);
        da_application.DeleteAppRegister(app_id);
    }

    //Insert Ct_App_Prem_Pay
    private bool InsertAppPremPay(string app_id)
    {
        try
        {
            string new_guid = Helper.GetNewGuid("SP_Check_App_Prem_Pay_ID", "@App_Prem_Pay_ID").ToString();

            bl_app_prem_pay app_prem_pay = new bl_app_prem_pay();
            app_prem_pay.App_Prem_Pay_ID = new_guid;

            app_prem_pay.App_Register_ID = app_id;

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            DateTime pay_date = Convert.ToDateTime(txtDateSignature.Text, dtfi);
            app_prem_pay.Pay_Date = pay_date;

            app_prem_pay.Is_Init_Payment = 1;

            if (txtPremiumAmount.Text == "")
            {
                app_prem_pay.Amount = 0.00;
            }
            else
            {
                app_prem_pay.Amount = Convert.ToDouble(txtPremiumAmount.Text);
            }

            if (hdfOriginalPremiumAmountSystem.Value == "0")
            {
                app_prem_pay.Original_Amount = 0.00;
                app_prem_pay.Rounded_Amount = 0.00;
            }
            else
            {
                app_prem_pay.Original_Amount = Convert.ToDouble(hdfOriginalPremiumAmountSystem.Value);
                app_prem_pay.Rounded_Amount = Math.Ceiling(Convert.ToDouble(hdfOriginalPremiumAmountSystem.Value));
            }

            DateTime created_on = Convert.ToDateTime(hdfDateEntry.Value, dtfi);

            app_prem_pay.Created_On = created_on;
            app_prem_pay.Created_By = hdfusername.Value;
            app_prem_pay.Created_Note = txtNote.Text.Trim();

            //Insertion
            if (da_application.InsertAppPremPay(app_prem_pay))
            {

                return true;

            }
            else
            {
                //Rollback
                Rollback(app_id);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppPremPay] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;
        }
    }

    //Insert Ct_App_Info_Person
    private bool InsertAppInfoPerson(string app_id)
    {
        try
        {
            bl_app_info_person app_info_person = new bl_app_info_person();

            app_info_person.App_Register_ID = app_id;

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            DateTime dob = Convert.ToDateTime(txtDateBirthLife1.Text, dtfi);
            app_info_person.Birth_Date = dob;

            app_info_person.Country_ID = ddlNationalityLife1.SelectedValue;
            app_info_person.Gender = Convert.ToInt32(ddlGenderLife1.SelectedValue);
            app_info_person.ID_Card = txtIDNumberLife1.Text.Trim();
            app_info_person.ID_Type = Convert.ToInt32(ddlIDTypeLife1.SelectedValue);
            app_info_person.First_Name = txtFirstNameEngLife1.Text.Trim();
            app_info_person.Last_Name = txtSurnameEngLife1.Text;

            //Check for null
            if (txtFirstNameFatherLife1.Text.Trim() == "")
            {
                app_info_person.Father_First_Name = "";
            }
            else
            {
                app_info_person.Father_First_Name = txtFirstNameFatherLife1.Text.Trim();
            }

            if (txtSurnameFatherLife1.Text.Trim() == "")
            {
                app_info_person.Father_Last_Name = "";
            }
            else
            {
                app_info_person.Father_Last_Name = txtSurnameFatherLife1.Text.Trim();
            }

            if (txtFirstNameKhLife1.Text.Trim() == "")
            {
                app_info_person.Khmer_First_Name = "";
            }
            else
            {
                app_info_person.Khmer_First_Name = txtFirstNameKhLife1.Text.Trim();
            }

            if (txtSurnameKhLife1.Text.Trim() == "")
            {
                app_info_person.Khmer_Last_Name = "";
            }
            else
            {
                app_info_person.Khmer_Last_Name = txtSurnameKhLife1.Text.Trim();

            }

            if (txtFirstNameMotherLife1.Text.Trim() == "")
            {
                app_info_person.Mother_First_Name = "";
            }
            else
            {
                app_info_person.Mother_First_Name = txtFirstNameMotherLife1.Text.Trim();
            }

            if (txtSurnameMotherLife1.Text.Trim() == "")
            {
                app_info_person.Mother_Last_Name = "";
            }
            else
            {
                app_info_person.Mother_Last_Name = txtSurnameMotherLife1.Text.Trim();
            }

            if (txtPreviousFirstnameLife1.Text.Trim() == "")
            {
                app_info_person.Prior_First_Name = "";
            }
            else
            {
                app_info_person.Prior_First_Name = txtPreviousFirstnameLife1.Text.Trim();
            }

            if (txtPreviousSurnameLife1.Text.Trim() == "")
            {
                app_info_person.Prior_Last_Name = "";
            }
            else
            {
                app_info_person.Prior_Last_Name = txtPreviousSurnameLife1.Text.Trim();
            }

            //Insertion
            if (da_application.InsertAppInfoPerson(app_info_person))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_id);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoPerson] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }

    //Insert Ct_App_Info_Person_Sub
    private bool InsertAppInfoPersonSub(string app_id)
    {
        try
        {
            bl_app_info_person app_info_person = new bl_app_info_person();

            app_info_person.App_Register_ID = app_id;

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            DateTime dob = Convert.ToDateTime(txtDateBirthLife1.Text, dtfi);
            app_info_person.Birth_Date = dob;

            app_info_person.Country_ID = ddlNationalityLife1.SelectedValue;
            app_info_person.Gender = Convert.ToInt32(ddlGenderLife1.SelectedValue);
            app_info_person.ID_Card = txtIDNumberLife1.Text.Trim();
            app_info_person.ID_Type = Convert.ToInt32(ddlIDTypeLife1.SelectedValue);
            app_info_person.First_Name = txtFirstNameEngLife1.Text.Trim();
            app_info_person.Last_Name = txtSurnameEngLife1.Text;

            //Check for null
            if (txtFirstNameFatherLife1.Text.Trim() == "")
            {
                app_info_person.Father_First_Name = "";
            }
            else
            {
                app_info_person.Father_First_Name = txtFirstNameFatherLife1.Text.Trim();
            }

            if (txtSurnameFatherLife1.Text.Trim() == "")
            {
                app_info_person.Father_Last_Name = "";
            }
            else
            {
                app_info_person.Father_Last_Name = txtSurnameFatherLife1.Text.Trim();
            }

            if (txtFirstNameKhLife1.Text.Trim() == "")
            {
                app_info_person.Khmer_First_Name = "";
            }
            else
            {
                app_info_person.Khmer_First_Name = txtFirstNameKhLife1.Text.Trim();
            }

            if (txtSurnameKhLife1.Text.Trim() == "")
            {
                app_info_person.Khmer_Last_Name = "";
            }
            else
            {
                app_info_person.Khmer_Last_Name = txtSurnameKhLife1.Text.Trim();

            }

            if (txtFirstNameMotherLife1.Text.Trim() == "")
            {
                app_info_person.Mother_First_Name = "";
            }
            else
            {
                app_info_person.Mother_First_Name = txtFirstNameMotherLife1.Text.Trim();
            }

            if (txtSurnameMotherLife1.Text.Trim() == "")
            {
                app_info_person.Mother_Last_Name = "";
            }
            else
            {
                app_info_person.Mother_Last_Name = txtSurnameMotherLife1.Text.Trim();
            }

            if (txtPreviousFirstnameLife1.Text.Trim() == "")
            {
                app_info_person.Prior_First_Name = "";
            }
            else
            {
                app_info_person.Prior_First_Name = txtPreviousFirstnameLife1.Text.Trim();
            }

            if (txtPreviousSurnameLife1.Text.Trim() == "")
            {
                app_info_person.Prior_Last_Name = "";
            }
            else
            {
                app_info_person.Prior_Last_Name = txtPreviousSurnameLife1.Text.Trim();
            }

            //Insertion
            if (da_application.InsertAppInfoPerson(app_info_person))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_id);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoPerson] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }



    //Insert Ct_App_Info_Contact
    private bool InsertAppInfoContact(string app_id)
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
            if (da_application.InsertAppInfoContact(app_info_contact))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_info_contact.App_Register_ID);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoContact] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }

    //Insert Ct_App_Info_Contact_Sub
    private bool InsertAppInfoContactSub(string app_id)
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
            if (da_application.InsertAppInfoContact(app_info_contact))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_info_contact.App_Register_ID);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoContactSub] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }

    //Insert Ct_App_Info_Address
    private bool InsertAppInfoAddress(string app_id)
    {
        try
        {
            bl_app_info_address app_info_address = new bl_app_info_address();

            app_info_address.App_Register_ID = app_id;

            app_info_address.Country_ID = ddlCountryLife1.SelectedValue.Trim();
            app_info_address.Address1 = txtAddress1Life1.Text.Trim();
            app_info_address.Address2 = txtAddress2Life1.Text.Trim();
            app_info_address.Address3 = "";

            app_info_address.Province = txtCityLife1.Text.Trim();

            //check for null
         
            if (txtZipCodeLife1.Text.Trim()=="")
            {
                app_info_address.Zip_Code = "";
            }
            else
            {
                app_info_address.Zip_Code = txtZipCodeLife1.Text.Trim();
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
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoAddress] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }

    }

    //Insert Ct_App_Job_History
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
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppJobHistory] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }

    }

    //Insert Ct_App_Loan
    private bool InsertAppLoan(string app_id)
    {
        try
        {
            bl_app_loan app_loan = new bl_app_loan();

            app_loan.App_Register_ID = app_id;

            if (txtInterest.Text.Trim() != "")
            {
                app_loan.Interest_Rate = Convert.ToDouble(txtInterest.Text.Trim());
            }
            else
            {
                app_loan.Interest_Rate = 0;
            }


            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            DateTime loan_effective_date = Convert.ToDateTime(txtLoanEffectiveDate.Text, dtfi);

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
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppLoan] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }

    }

    //Insert Ct_App_Info_Body
    private bool InsertAppInfoBody(string app_id)
    {
       
        try
        {

            bl_app_info_body app_info_body= new bl_app_info_body();
            bl_app_info_body app_info_body1;
            app_info_body.App_Register_ID = app_id;

            app_info_body.Height = Convert.ToInt32(txtHeightLife1.Text.Trim());
            app_info_body.Weight_Change = 0;
            app_info_body.Weight = Convert.ToInt32(txtWeightLife1.Text);
            app_info_body.Is_Weight_Changed = Convert.ToInt32(rblWeightChangeLife1.SelectedValue);
            app_info_body.Reason =txtWeightChangeReasonLife1.Text.Trim();

            if (da_application.InsertAppInfoBody(app_info_body))
            {
                //return true;
                //life2


                app_info_body1 = new bl_app_info_body();
                app_info_body1.App_Register_ID = app_id;

                app_info_body1.Height = Convert.ToInt32(txtHeightLife2.Text.Trim());
                app_info_body1.Weight_Change = 0;
                app_info_body1.Weight = Convert.ToInt32(txtWeightLife2.Text);
                app_info_body1.Is_Weight_Changed = Convert.ToInt32(rblWeightChangeLife2.SelectedValue);
                app_info_body1.Reason = txtWeightChangeReasonLife2.Text.Trim();

                if (da_application.InsertAppInfoBody(app_info_body1))
                {
                    return true;
                }
                else
                {
                    //Rollback
                    Rollback(app_info_body.App_Register_ID);
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                    return false;
                }
            }
            else
            {
                //Rollback
                Rollback(app_info_body.App_Register_ID);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }

           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoBody] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }

    //Insert Ct_App_Life_Product
    private bool InsertAppLifeProduct(string app_id)
    {
        try
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            bl_app_life_product app_life_product = new bl_app_life_product();

            app_life_product.App_Register_ID = app_id;

            app_life_product.Age_Insure = Convert.ToInt32(txtAssureeAgeLife1.Text.Trim());


            app_life_product.Assure_Year = Convert.ToInt32(txtTermInsurance.Text.Trim());
            app_life_product.Assure_Up_To_Age = app_life_product.Age_Insure + app_life_product.Assure_Year;
            app_life_product.Pay_Year = Convert.ToInt32(txtPaymentPeriod.Text.Trim());
            app_life_product.Pay_Mode = Convert.ToInt32(ddlPremiumMode.SelectedValue.Trim());
            app_life_product.Pay_Up_To_Age = app_life_product.Age_Insure + app_life_product.Pay_Year;
            app_life_product.Product_ID = hdfInsurancePlan.Value.ToString();
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
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppLifeProduct] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }

    //Insert Ct_App_Discount
    private bool InsertAppDiscount(string app_id)
    {
        try
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            bl_app_discount app_discount = new bl_app_discount();

            app_discount.App_Register_ID = app_id;

            app_discount.Annual_Premium = Convert.ToDouble(hdfOriginalPremiumAmountSystem.Value);
            app_discount.Discount_Amount = Convert.ToDouble(txtDiscountAmount.Text);
            app_discount.Pay_Mode = Convert.ToInt32(ddlPremiumMode.SelectedValue);
            app_discount.Premium = Convert.ToDouble(txtPremiumAmountSystemLife1.Text.Trim());

            if (txtPremiumAmount.Text == "")
            {
                app_discount.Total_Amount = 0;
            }
            else
            {
                app_discount.Total_Amount = Convert.ToDouble(txtPremiumAmount.Text);
            }

            app_discount.Created_On = Convert.ToDateTime(hdfDateEntry.Value, dtfi);
            app_discount.Created_Note = txtInsurancePlanNote.Text;
            app_discount.Created_By = hdfusername.Value;

            if (da_application.InsertAppDiscount(app_discount))
            {
                return true;
            }
            else
            {
                //Rollback
                Rollback(app_discount.App_Register_ID);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppDiscount] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }

    //Loop gridview to insert Q&A to Ct_App_Answer_Item
    private bool InsertAppAnswerItem(string app_id)
    {
        try
        {
            foreach (GridViewRow row in GvQA.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    RadioButtonList rbtnlAnswer = (RadioButtonList)row.FindControl("rbtnlAnswer");
                    HiddenField hdfSeqNumber = (HiddenField)row.FindControl("hdfSeqNumber");
                    HiddenField hdfQuestionID = (HiddenField)row.FindControl("hdfQuestionID");

                    string new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();

                    bl_app_answer_item app_answer_item = new bl_app_answer_item();

                    app_answer_item.App_Answer_Item_ID = new_guid;
                    app_answer_item.App_Register_ID = app_id;
                    app_answer_item.Question_ID = hdfQuestionID.Value;
                    app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber.Value);
                    app_answer_item.Answer = Convert.ToInt32(rbtnlAnswer.SelectedValue);

                    //Insert into Ct_App_Answer_Item
                    if (da_application.InsertAppAnswerItem(app_answer_item))
                    {
                        //do nothing
                    }
                    else
                    {
                        //Rollback
                        Rollback(app_answer_item.App_Register_ID);
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
                        return false;
                    }
                }
            }

            //if reach here return true
            return true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppAnswerItem] in class [Pages_Business_application_form]. Details: " + ex.Message);
            Rollback(app_id);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Failed. Please check it again.')", true);
            return false;

        }
    }

    //Function to clear content
    protected void ImgBtnClear_Click(object sender, ImageClickEventArgs e)
    {
        Clear();
    }

    //Clear all
    private void Clear()
    {
        //clear Textboxes
        txtAddress1Life1.Text = "";
        txtAddress2Life1.Text = "";
        txtAgentName.Text = "";
        txtAnualIncomeLife1.Text = "";
        txtPaymentCode.Text = "";
        txtApplicationNumber.Text = "";
        txtApplicationNumberModal.Text = "";


        txtAssureeAgeLife1.Text = "";
        txtAssureeAgeLife2.Text = "";

        txtBenefitName.Text = "";
        txtBenefitIDNo.Text = "";
        txtBenefitNote.Text = "";
        txtBenefitSharePercentage.Text = "";
        txtCityLife1.Text = "";
        txtCurrentPositionLife1.Text = "";
        txtDataEntryBy.Text = "";
        txtDateBirthLife1.Text = "";
        txtDateBirthLife2.Text = "";
        txtDateEntry.Text = "";
        txtDateSignature.Text = "";
        txtEmailLife1.Text = "";
        txtFirstNameEngLife1.Text = "";
        txtFirstNameFatherLife1.Text = "";
        txtFirstNameKhLife1.Text = "";
        txtFirstNameMotherLife1.Text = "";

        txtFirstNameEngLife2.Text = "";
        txtFirstNameFatherLife2.Text = "";
        txtFirstNameKhLife2.Text = "";
        txtFirstNameMotherLife2.Text = "";

        txtHeightLife1.Text = "";
        txtHeightLife2.Text = "";
        txtIDNumberLife1.Text = "";
        txtIDNumberLife2.Text = "";
        txtInsuranceAmountRequired.Text = "";
        txtInterest.Text = "";
        txtLoanEffectiveDate.Text = "";
        txtMarketingCode.Text = "";
        txtMarketingName.Text = "";
        txtMobilePhoneLife1.Text = "";
        txtNameEmployerLife1.Text = "";
        txtNatureBusinessLife1.Text = "";
        txtNote.Text = "";
        txtOutstandingLoanAmount.Text = "";
        txtPaymentPeriod.Text = "";
        txtPolicyNumber.Text = "";
        txtPremiumAmount.Text = "";

        txtPremiumAmountSystemLife1.Text = "";
        txtPremiumAmountSystemLife2.Text = "";

        txtPreviousFirstnameLife1.Text = "";
        txtPreviousSurnameLife1.Text = "";

        txtPreviousFirstNameLife2.Text = "";
        txtPreviousSurNameLife2.Text = "";

        txtRoleAndResponsibilityLife1.Text = "";
        txtSurnameEngLife1.Text = "";
        txtSurnameFatherLife1.Text = "";
        txtSurnameKhLife1.Text = "";
        txtSurnameMotherLife1.Text = "";

        txtSurNameEngLife2.Text = "";
        txtSurNameFatherLife2.Text = "";
        txtSurNameKhLife2.Text = "";
        txtSurNameMotherLife2.Text = "";

        txtTelephoneLife1.Text = "";
        txtTermInsurance.Text = "";
        txtTermLoan.Text = "";
        txtUnderwritingStatus.Text = "";
        txtWeightLife1.Text = "";
        txtWeightLife2.Text = "";
        txtZipCodeLife1.Text = "";
        txtDiscountAmount.Text = "0";

        //Clear hidden fields
       
        hdfBenefitCount.Value = "0";
        hdfDataEntryBy.Value = "";
        hdfInsurancePlan.Value = "0";
        hdfMarketingCode.Value = "";
        hdfMarketingName.Value = "";
        txtPaymentPeriod.Text = "0";

        txtPremiumAmountSystemLife1.Text = "0";
        txtPremiumAmountSystemLife2.Text = "0";

        txtTermInsurance.Text = "0";
        hdfTotalAgentRow.Value = "0";
        hdfAppRegisterID.Value = "";
      
        //ddlPremiumMode.Value = "1";

        //Set default ddl selection
        ddlCountryLife1.SelectedIndex = 0;
        ddlNationalityLife1.SelectedIndex = 0;
        ddlGenderLife1.SelectedIndex = 0;
        ddlBenefitRelation.SelectedIndex = 0;
        ddlBenefitIDType.SelectedIndex = 0;
        ddlIDTypeLife1.SelectedIndex = 0;
        ddlLoanType.SelectedIndex = 0;
        ddlPremiumMode.SelectedIndex = 0;
        ddlProductTypeModal.SelectedIndex = 0;
        ddlChannel.SelectedIndex = 0;

        //Set default selection of radiobuttonlist in gridview
        foreach (GridViewRow row in GvQA.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                RadioButtonList rbtnlAnswer = (RadioButtonList)row.FindControl("rbtnlAnswer");

                rbtnlAnswer.SelectedValue = "0";

            }
        }
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
            app_step_history.Created_By = hdfusername.Value;
            app_step_history.Created_On = System.DateTime.Now;
            app_step_history.Created_Note = "";

            da_step.InsertAppStepHistory(app_step_history);

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppStepHistory] in class [Pages_Business_application_form]. Details: " + ex.Message);

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

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        string user_id = myUser.ProviderUserKey.ToString();
        string user_name = myUser.UserName;

        //Continue Registration
        string app_id = da_application.GetAppRegisterIDByUserID(user_id);

        da_application.DeleteAppIDTemp(user_id);

        //Check if sucessful insertions
        if (da_application.CheckAppRegisterID(app_id))
        {
            if (!InsertAppPremPay(app_id))
            {
                return;
            }

            if (!InsertAppInfoPerson(app_id))
            {
                return;
            }

            if (!InsertAppInfoContact(app_id))
            {
                return;
            }

            if (!InsertAppInfoAddress(app_id))
            {
                return;
            }

            if (!InsertAppJobHistory(app_id))
            {
                return;
            }

            //MRTA Save Ct_App_Loan
            if (hdfProductType.Value == "2")
            {
                if (!InsertAppLoan(app_id))
                {
                    return;
                }
            }

            if (!InsertAppInfoBody(app_id))
            {
                return;
            }

            if (!InsertAppLifeProduct(app_id))
            {
                return;
            }

            if (!InsertAppDiscount(app_id))
            {
                return;
            }


            if (!InsertAppAnswerItem(app_id))
            {
                return;
            }

            InsertAppStepHistory(app_id);
            InsertAppStepNext(app_id);

            //if reach here 
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Registration Successful!')", true);

            Clear();
        }
    }

    //Cancel Application..................................................................................................................
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (hdfAppRegisterID.Value != "")
        {
            if (hdfUnderwritingStatus.Value != "")
            {
                bl_app_register_cancel app_register_cancel = new bl_app_register_cancel();

                app_register_cancel.App_Register_ID = hdfAppRegisterID.Value;
                app_register_cancel.Created_By = hdfusername.Value;
                app_register_cancel.Created_Note = txtCancelNote.Text.Trim();
                app_register_cancel.Created_On = System.DateTime.Now;

                if (da_application.InsertAppRegisterCancel(app_register_cancel))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Cancel application Successful.')", true);
                    //Update app_number to app_register_id
                    da_application.UpdateAppNumberToAppRegisterID(hdfAppRegisterID.Value);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Cancel application failed.')", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Can not cancel this application. Status: IF')", true);
            }

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('No application to cancel.')", true);
        }
        Clear();

    }

    //Grid Answer Item mouse over highliht
    protected void GvQA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


        }

    }

    protected void rblWeightChangeLife1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblWeightChangeLife1.SelectedIndex > 0) {
            showWeightReasonChangeLife1(true);
           
        }
        else
        {
            showWeightReasonChangeLife1(false);
            txtWeightChangeReasonLife1.Text = "";
        }
    }

    protected void rblWeightChangeLife2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblWeightChangeLife2.SelectedIndex > 0)
        {
            showWeightReasonChangeLife2(true);

        }
        else {
            showWeightReasonChangeLife2(false);
        }
    }
    #region //Additional functions
    //TODO: additional function by maneth
    void showWeightReasonChangeLife1(bool status) {
        lblWeightChangeReasonLife1.Visible = status;
        txtWeightChangeReasonLife1.Visible = status;
    }
    void showWeightReasonChangeLife2(bool status)
    {
        lblWeightChangeReasonLife2.Visible = status;
        txtWeightChangeReasonLife2.Visible = status;
    }
    void alertMessage(string message) {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('"+message+"')", true);
    }


    #endregion
    #region //Calculate premium
    void CalculatePremium() { 
        //variable declaration
        double premiumLife1 = 0.0;
        double premiumLife2 = 0.0;
        int ageLife1 = 0; 
        int ageLife2 = 0;
        string entryDate;
        string dob;
        int gender;
        int paymentMode;
        string productId;
        double discount=0.0;
        double discountAmountLife1 = 00;
        double discountAmountLife2 = 0.0;
        double premiumAfterDiscountLife1=0.0; 
        double premiumAfterDiscountLife2 = 0.0;
        double totalPremium = 0.0;
        double sumInsured=0.0;
        double annualPremiumLife1 = 0.0;
        double annualPremiumLife2 = 0.0;


        //initialize value

       // entryDate = txtDateEntry.Text.Trim();
        entryDate = hdfDateEntry.Value.Trim();
        alertMessage(entryDate);
        productId = "FP";
        paymentMode = Convert.ToInt32(ddlPremiumMode.SelectedValue.Trim());
        sumInsured=Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim());

        //get discount value from user input
        if (txtDiscountAmount.Text.Trim() != "") {
            discount = Convert.ToDouble(txtDiscountAmount.Text.Trim());
        }


        CalculationWebService objCalc = new CalculationWebService();
        da_application_fp6 objPremium = new da_application_fp6();

        //life insured 1
       

        dob= txtDateBirthLife1.Text.Trim();
        gender = Convert.ToInt32(ddlGenderLife1.SelectedValue.Trim());

        //get age
        if(dob!="" && entryDate!="" && gender>=0){
            ageLife1 = objCalc.GetCustomerAge(dob, entryDate);
            txtAssureeAgeLife1.Text = ageLife1+"";

            //get premium
            annualPremiumLife1 = da_application_fp6.getLifeInsuredPremium(productId, gender, ageLife1, 1);

            premiumLife1 = da_application_fp6.getLifeInsuredPremium(productId, gender, ageLife1, paymentMode);
            premiumLife1 = (premiumLife1 * sumInsured) / 1000;

            txtAnnualOriginalPremiumAmountSystemLife1.Text = annualPremiumLife1 + "";
            txtPremiumAmountSystemLife1.Text = premiumLife1+"";

            premiumAfterDiscountLife1 = premiumLife1 - discountAmountLife1;

        
        }

        //reset 
        dob = "";
        gender = -1;
       

        //Life insured 2

        dob = txtDateBirthLife2.Text.Trim();
        gender = Convert.ToInt32(ddlGenderLife2.SelectedValue.Trim());

        //get age
        if (dob != "" && entryDate != "" && gender>=0)
        {
            ageLife2 = objCalc.GetCustomerAge(dob, entryDate);
            txtAssureeAgeLife2.Text = ageLife2 + "";

            //get premium
            annualPremiumLife2 = da_application_fp6.getLifeInsuredPremium(productId, gender, ageLife2, 1);

            premiumLife2 = da_application_fp6.getLifeInsuredPremium(productId, gender, ageLife2, paymentMode);
            premiumLife2 = (premiumLife2 * sumInsured) / 1000;

            txtAnnualOriginalPremiumAmountSystemLife2.Text = annualPremiumLife2 + "";
            txtPremiumAmountSystemLife2.Text = premiumLife2 + "";

            premiumAfterDiscountLife2 = premiumLife2 - discountAmountLife2;
        }
        //reset
        dob = "";
        gender = -1;

        //total premium
        totalPremium = premiumLife1 + premiumLife2;
        txtTotalPremium.Text = totalPremium + "";
    
    }
    #endregion



    protected void ddlPremiumMode_SelectedIndexChanged(object sender, EventArgs e)
    {
       CalculatePremium();
        
    }
    protected void txtInsuranceAmountRequired_TextChanged(object sender, EventArgs e)
    {
        CalculatePremium();
    }
}