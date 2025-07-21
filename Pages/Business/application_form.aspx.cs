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


public partial class Pages_Business_application_form : System.Web.UI.Page
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
           txtIDNumber.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtDateSignature.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtFirstNameKh.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtSurnameEng.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtFirstNameEng.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtSurnameFather.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtSurnameKh.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtFirstNameFather.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtSurnameMother.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtFirstNameMother.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtPreviousSurname.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtPreviousFirstname.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtDateBirth.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtAddress1.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtAddress2.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtCity.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtZipCode.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtTelephone.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtMobilePhone.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtEmail.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtNameEmployer.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtNatureBusiness.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtRoleAndResponsibility.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtCurrentPosition.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtAnualIncome.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtInsuranceAmountRequired.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtPremiumAmount.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtBenefitNote.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtHeight.Attributes.Add("onkeypress", "return event.keyCode!=13");
           txtWeight.Attributes.Add("onkeypress", "return event.keyCode!=13");
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

            DateTime dob = Convert.ToDateTime(txtDateBirth.Text, dtfi);
            app_info_person.Birth_Date = dob;

            app_info_person.Country_ID = ddlNationality.SelectedValue;
            app_info_person.Gender = Convert.ToInt32(ddlGender.SelectedValue);
            app_info_person.ID_Card = txtIDNumber.Text.Trim();
            app_info_person.ID_Type = Convert.ToInt32(ddlIDType.SelectedValue);
            app_info_person.First_Name = txtFirstNameEng.Text.Trim();
            app_info_person.Last_Name = txtSurnameEng.Text;

            //Check for null
            if (txtFirstNameFather.Text.Trim() == "")
            {
                app_info_person.Father_First_Name = "";
            }
            else
            {
                app_info_person.Father_First_Name = txtFirstNameFather.Text.Trim();
            }

            if (txtSurnameFather.Text.Trim() == "")
            {
                app_info_person.Father_Last_Name = "";
            }
            else
            {
                app_info_person.Father_Last_Name = txtSurnameFather.Text.Trim();
            }

            if (txtFirstNameKh.Text.Trim() == "")
            {
                app_info_person.Khmer_First_Name = "";
            }
            else
            {
                app_info_person.Khmer_First_Name = txtFirstNameKh.Text.Trim();
            }

            if (txtSurnameKh.Text.Trim() == "")
            {
                app_info_person.Khmer_Last_Name = "";
            }
            else
            {
                app_info_person.Khmer_Last_Name = txtSurnameKh.Text.Trim();
           
            }

            if (txtFirstNameMother.Text.Trim() == "")
            {
                app_info_person.Mother_First_Name = "";
            }
            else
            {
                app_info_person.Mother_First_Name = txtFirstNameMother.Text.Trim();
            }

            if (txtSurnameMother.Text.Trim() == "")
            {
                app_info_person.Mother_Last_Name = "";
            }
            else
            {
                app_info_person.Mother_Last_Name = txtSurnameMother.Text.Trim();
            }

            if (txtPreviousFirstname.Text.Trim() == "")
            {
                app_info_person.Prior_First_Name = "";
            }
            else
            {
                app_info_person.Prior_First_Name = txtPreviousFirstname.Text.Trim();
            }

            if (txtPreviousSurname.Text.Trim() == "")
            {
                app_info_person.Prior_Last_Name = "";
            }
            else
            {
                app_info_person.Prior_Last_Name = txtPreviousSurname.Text.Trim();
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
            if (txtEmail.Text.Trim() == "")
            {
                app_info_contact.EMail = "";
            }
            else
            {
                app_info_contact.EMail = txtEmail.Text.Trim();
            }

            if (txtTelephone.Text.Trim() == "")
            {
                app_info_contact.Home_Phone1 = "";
            }
            else
            {
                app_info_contact.Home_Phone1 = txtTelephone.Text.Trim();
            }

            if (txtMobilePhone.Text.Trim() == "")
            {
                app_info_contact.Mobile_Phone1 = "";
            }
            else
            {
                app_info_contact.Mobile_Phone1 = txtMobilePhone.Text.Trim();
           
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
    
    //Insert Ct_App_Info_Address
    private bool InsertAppInfoAddress(string app_id)
    {
        try
        {
            bl_app_info_address app_info_address = new bl_app_info_address();

            app_info_address.App_Register_ID = app_id;

            app_info_address.Country_ID = ddlCountry.SelectedValue;
            app_info_address.Address1 = txtAddress1.Text.Trim();
            app_info_address.Address2 = txtAddress2.Text.Trim();
            app_info_address.Address3 = "";

            app_info_address.Province = txtCity.Text.Trim();

            //check for null
            if (hdfZipCode.Value == "")
            {
                app_info_address.Zip_Code = "";
            }
            else
            {
                app_info_address.Zip_Code = hdfZipCode.Value;
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

            if (txtAnualIncome.Text.Trim() == "")
            {
                app_job_history.Anual_Income = 0;
            }
            else
            {
                app_job_history.Anual_Income = Convert.ToDouble(txtAnualIncome.Text.Trim());
            }                       

            //Check for null
            if (txtCurrentPosition.Text.Trim() == "")
            {
                app_job_history.Current_Position = "";
            }
            else
            {
                app_job_history.Current_Position = txtCurrentPosition.Text.Trim();
            }

            if (txtNameEmployer.Text.Trim() == "")
            {
                app_job_history.Employer_Name = "";
            }
            else
            {
                app_job_history.Employer_Name = txtNameEmployer.Text.Trim();
            }

            if (txtRoleAndResponsibility.Text.Trim() == "")
            {
                app_job_history.Job_Role = "";
            }
            else
            {
                app_job_history.Job_Role = txtRoleAndResponsibility.Text.Trim();
            }

            if (txtNatureBusiness.Text.Trim() == "")
            {
                app_job_history.Nature_Of_Business = "";
            }
            else
            {
                app_job_history.Nature_Of_Business = txtNatureBusiness.Text.Trim();
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
            bl_app_info_body app_info_body = new bl_app_info_body();

            app_info_body.App_Register_ID = app_id;

            app_info_body.Height = Convert.ToInt32(txtHeight.Text);
            app_info_body.Weight_Change = 0;
            app_info_body.Weight = Convert.ToInt32(txtWeight.Text);
            app_info_body.Is_Weight_Changed = Convert.ToInt32(rbtnlWeightChange.SelectedValue);
            app_info_body.Reason = txtWeightChangeReason.Text.Trim();

            if (da_application.InsertAppInfoBody(app_info_body))
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

            app_life_product.Age_Insure = Convert.ToInt32(hdfAssureeAge.Value);
                  

            app_life_product.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
            app_life_product.Assure_Up_To_Age = app_life_product.Age_Insure + app_life_product.Assure_Year;
            app_life_product.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);
            app_life_product.Pay_Mode = Convert.ToInt32(hdfPremiumMode.Value);
            app_life_product.Pay_Up_To_Age = app_life_product.Age_Insure + app_life_product.Pay_Year;
            app_life_product.Product_ID = hdfInsurancePlan.Value.ToString();
            app_life_product.System_Premium = Convert.ToDouble(hdfPremiumAmountSystem.Value);
            app_life_product.System_Premium_Discount = 0.00;
            app_life_product.System_Sum_Insure = Convert.ToDouble(txtInsuranceAmountRequired.Text);

            //if(ckbPrePremiumDiscount.Checked){
            //    app_life_product.Is_Pre_Premium_Discount = 1;
            //}
            //else
            //{
            //    app_life_product.Is_Pre_Premium_Discount = 0;
            //}
          

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
            app_discount.Premium = Convert.ToDouble(hdfPremiumAmountSystem.Value);

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
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtAgentName.Text = "";
        txtAnualIncome.Text = "";
        txtPaymentCode.Text = "";
        txtApplicationNumber.Text = "";
        txtApplicationNumberModal.Text = "";
        txtAssureeAge.Text = "";
        txtBenefitName.Text = "";
        txtBenefitIDNo.Text = "";
        txtBenefitNote.Text = "";
        txtBenefitSharePercentage.Text = "";
        txtCity.Text = "";
        txtCurrentPosition.Text = "";
        txtDataEntryBy.Text = "";
        txtDateBirth.Text = "";
        txtDateEntry.Text = "";
        txtDateSignature.Text = "";
        txtEmail.Text = "";
        txtFirstNameEng.Text = "";
        txtFirstNameFather.Text = "";
        txtFirstNameKh.Text = "";
        txtFirstNameMother.Text = "";
        txtHeight.Text = "";
        txtIDNumber.Text = "";
        txtInsuranceAmountRequired.Text = "";
        txtInterest.Text = "";
        txtLoanEffectiveDate.Text = "";
        txtMarketingCode.Text = "";
        txtMarketingName.Text = "";
        txtMobilePhone.Text = "";
        txtNameEmployer.Text = "";
        txtNatureBusiness.Text = "";
        txtNote.Text = "";
        txtOutstandingLoanAmount.Text = "";
        txtPaymentPeriod.Text = "";
        txtPolicyNumber.Text = "";
        txtPremiumAmount.Text = "";
        txtPremiumAmountSystem.Text = "";
        txtPreviousFirstname.Text = "";
        txtPreviousSurname.Text = "";
        txtRoleAndResponsibility.Text = "";
        txtSurnameEng.Text = "";
        txtSurnameFather.Text = "";
        txtSurnameKh.Text = "";
        txtSurnameMother.Text = "";
        txtTelephone.Text = "";
        txtTermInsurance.Text = "";
        txtTermLoan.Text = "";
        txtUnderwritingStatus.Text = "";
        txtWeight.Text = "";
        txtZipCode.Text = "";
        txtDiscountAmount.Text = "0";

        //Clear hidden fields
        hdfAssureeAge.Value = "0";
        hdfBenefitCount.Value = "0";
        hdfDataEntryBy.Value = "";
        hdfInsurancePlan.Value = "0";
        hdfMarketingCode.Value = "";
        hdfMarketingName.Value = "";
        hdfPaymentPeriod.Value = "0";
        hdfPremiumAmountSystem.Value = "0";
        hdfTermInsurance.Value = "0";
        hdfTotalAgentRow.Value = "0";
        hdfAppRegisterID.Value = "";
        hdfPremiumMode.Value = "1";

        //Set default ddl selection
        ddlCountry.SelectedIndex = 0;
        ddlNationality.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        ddlBenefitRelation.SelectedIndex = 0;
        ddlBenefitIDType.SelectedIndex = 0;
        ddlIDType.SelectedIndex = 0;
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

    protected void btnOk_Click(object sender, EventArgs e)
    {

    }

}