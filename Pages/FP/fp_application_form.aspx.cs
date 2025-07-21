using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Collections;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_FP_fp_application_form : System.Web.UI.Page
{
    string user_id="";
    string user_name="";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            MembershipUser myUser = Membership.GetUser();
             user_id = myUser.ProviderUserKey.ToString();
             user_name = myUser.UserName;

            //bind user name and user id to hiddenfield
            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;
            txtDataEntryBy.Text = user_name;

          /*  //save plan test
            if(!da_Policy_FP_Plan.saveFfPolicyPlan(new bl_Policy_FP_Plan(Helper.GetNewGuid("SP_CHECK_FP_PLAN_ID","PLAN_ID"),"SUM_INSURED10000",10000,5000,2500,2500,10,10,DateTime.Now,"System",DateTime.Now,null)))
            {

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save plan fail.')", true);
                return;
            }
            */
        }

        /*
        bl_app_register bl = new bl_app_register();
        bl.App_Register_ID= Helper.GetNewGuid("SP_Check_App_Register_ID", "@App_Register_ID").ToString();
        bl.App_Number="B";
        bl.Original_App_Number="B";
        bl.Created_By="B";
        bl.Created_On=DateTime.Now;
        bl.Office_ID="HQ";
        bl.Channel_ID="B";
        bl.Channel_Item_ID="B";
        bl.Created_Note = "B";
        bl.Payment_Code = "";

        da_application.InsertAppRegister(bl);
         */

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void ImgBtnClear_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!saveFPApplication())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please contact your system administrator.')", true);
        }
    }

    protected void GvQA_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    //save application
    private bool saveFPApplication() {
        bool result = false;
        string app_id = "";
        string app_no = "";
        string note = "";
        DateTime date_entry;
        string entry_by = "";
        DateTime current_date = DateTime.Now;
        int payment_mode;
        int sale_agent_id;

        try
        {
            app_id= Helper.GetNewGuid("SP_Check_FP_App_ID", "@FP_App_ID").ToString();
            app_no = FP_Helper.formatApplicationNumber(Int32.Parse(txtApplicationNumber.Text));
            note = txtNote.Text.Trim();
            sale_agent_id = Convert.ToInt32(hdfMarketingCode.Value.Trim());
            payment_mode = Convert.ToInt32(ddlPaymentMode.SelectedValue.Trim());
            entry_by = txtDataEntryBy.Text.Trim();
            date_entry = Convert.ToDateTime(FP_Helper.newDateFormat(txtDateEntry.Text.Trim()));

            bl_FP_App newApp = new bl_FP_App(app_id, app_no, payment_mode, "0001", note, sale_agent_id, date_entry, entry_by, current_date, "", current_date);

            if (da_FP_App.insertFPApp(newApp))
            {
                result = true;
            }
        }
        catch (Exception ex)
        {
           
            Log.AddExceptionToLog("Error Function [saveFPApplication] in page [fp_application_form]. Detai: " + ex.Message);
        }
        return result;
    }
    //update application
    private bool updateFPApplication()
    {
        bool result = false;
        string app_id = "";
        string app_no = "";
        int payment_mode;
        DateTime date_entry;
        string entry_by = "";
        string created_note = "";
        DateTime current_date = DateTime.Now;

        try
        {
            payment_mode = int.Parse(ddlPaymentMode.SelectedValue.Trim());
            date_entry = Convert.ToDateTime( txtDateEntry.Text);
            entry_by = user_name;
            created_note = txtNote.Text;


            bl_FP_App newApp = new bl_FP_App(app_id, app_no, payment_mode, "000008", created_note, 1, date_entry, user_name, current_date, "", current_date);

            if (da_FP_App.updateFPApp(newApp))
            {
                result = true;
            }
        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error Function [updateFPApplication] in page [fp_application_form]. Detai: " + ex.Message);
        }
        return result;
    }
    //get application by application number
    private void getApplicationByApplicationNumber(string applicationNumber)
    {

        ArrayList myApplication = new ArrayList();
      //  myApplication = da_FP_App.getApplicationList(applicationNumber);
       
               
    }
    //save customer
    private bool saveCustomer() 
    {

        bool result = false;
        try 
        {
            string app_id;
            string customer_id;
            int id_type;
            string id_number;
            string surname_kh;
            string surname_en;
            string firstname_kh;
            string firstname_en;
            int gender;
            DateTime dob;
            string nationality;
            string occupation;
            string created_by;
            DateTime created_on;

            app_id = getApplicationID(Convert.ToInt32(txtApplicationNumber.Text.Trim()));


            if (app_id !="")
            {
               
                customer_id = Helper.GetNewGuid("SP_CHECK_FP_CUSTOMER_ID", "@Customer_ID");
                id_type = Convert.ToInt32(ddlIDType.SelectedValue.Trim());
                id_number = txtIDNumber.Text.Trim();
                surname_kh = txtSurnameKh.Text.Trim();
                surname_en = txtSurnameEng.Text.Trim();
                firstname_kh = txtFirstNameKh.Text.Trim();
                firstname_en = txtFirstNameEng.Text.Trim();
                gender = Convert.ToInt32(ddlGender.SelectedValue.Trim());
                dob = FP_Helper.newDateFormat(txtDateBirth.Text.Trim());
                nationality = ddlNationality.SelectedValue.Trim();
                occupation = "";
                created_by = hdfusername.Value;
                created_on = DateTime.Now;

                bl_FP_Customer customer = new bl_FP_Customer(app_id, customer_id, id_type, id_number, surname_kh, surname_en, firstname_kh, firstname_en, gender, dob, nationality, occupation, created_by, created_on, null, created_on);
                da_FP_Customer.saveCustomer(customer);
                result = true;
            }
            else 
            {
                //Delete data in table [ct_fp_app]
                string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                da_FP_App.deleteFPApp(appNumber);
                result = false;
            }

        }
        catch(Exception ex)
        {
            //Delete data in table [ct_fp_app]
            string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
            da_FP_App.deleteFPApp(appNumber);

           
            Log.AddExceptionToLog("Error function [saveCustomer] in page [fp_application_form], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    //save customer sub
    private bool saveCustomerSub(int step) 
    {

        bool result = false;
        try
        {
            string app_id = "";
            string sub_id = "";
            int sub_number = 0;
            int id_type = 0;
            string id_number = "";
            string surname_kh = "";
            string surname_en = "";
            string firstname_kh = "";
            string firstname_en = "";
            int gender = 0;
            DateTime dob = DateTime.Now;
            string relationship = "";
            string created_by;
            DateTime created_on;

            app_id = getApplicationID(Convert.ToInt32(txtApplicationNumber.Text.Trim()));

            if (app_id != "")
            {
                //loop of life insured 3, 4
                for (int i = 1; i <= step; i++)
                {
                    if (i == 1)//life insured 2
                    {
                        sub_number = i;
                        id_type = Convert.ToInt32(ddlIDTypeLife2.SelectedValue.Trim());
                        id_number = txtIDLife2.Text.Trim();
                        surname_kh = txtSureNameLife2.Text.Trim();
                        surname_en = txtSureNameENLife2.Text.Trim();
                        firstname_kh = txtFirstNameLife2.Text.Trim();
                        firstname_en = txtFirstNameENLife2.Text.Trim();
                        gender = Convert.ToInt32(ddlGenderLife2.SelectedValue.Trim());
                        dob = FP_Helper.newDateFormat(txtDOBLife2.Text.Trim());
                        relationship = rblSpouse.SelectedValue.Trim();
                    
                    }
                   else if (i == 2)//life insured 3
                    {
                        sub_number = i;
                        id_type = Convert.ToInt32(ddlIDTypeLife3.SelectedValue.Trim());
                        id_number = txtIDLife3.Text.Trim();
                        surname_kh = txtSureNameLife3.Text.Trim();
                        surname_en = txtSureNameENLife3.Text.Trim();
                        firstname_kh = txtFirstNameLife3.Text.Trim();
                        firstname_en = txtFirstNameENLife3.Text.Trim();
                        gender = Convert.ToInt32(ddlGenderLife3.SelectedValue.Trim());
                        dob = FP_Helper.newDateFormat(txtDOBLife3.Text.Trim());
                        relationship = rblChildren.SelectedValue.Trim();
                    
                    }
                    else if (i == 3)//life insured 4
                    {
                        sub_number = i;
                        id_type = Convert.ToInt32(ddlIDTypeLife4.SelectedValue.Trim());
                        id_number = txtIDLife4.Text.Trim();
                        surname_kh = txtSureNameLife4.Text.Trim();
                        surname_en = txtSureNameENLife4.Text.Trim();
                        firstname_kh = txtFirstNameLife4.Text.Trim();
                        firstname_en = txtFirstNameENLife4.Text.Trim();
                        gender = Convert.ToInt32(ddlGenderLife4.SelectedValue.Trim());
                        dob = FP_Helper.newDateFormat(txtDOBLife4.Text.Trim());
                        relationship = rblChildren1.SelectedValue.Trim();

                    }

                    sub_id = Helper.GetNewGuid("SP_CHECK_FP_CUSTOMER_SUB_ID", "@Sub_ID");
                    created_on = DateTime.Now;
                    created_by = hdfusername.Value.Trim();

                    bl_FP_Customer_sub customer_sub = new bl_FP_Customer_sub(app_id, sub_id, sub_number, id_type, id_number, surname_kh, surname_en, firstname_kh, firstname_en, gender, dob, relationship, created_by, created_on, null, created_on);
                    da_FP_Customer_Sub.saveCustomerSub(customer_sub);

                    result = true;

                }
            }
            else
            {
                //Delete data in table [ct_fp_app]
                string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                da_FP_App.deleteFPApp(appNumber);
                
                //delete data in table [ct_fp_customer] in case error in loop
                da_FP_Customer.deleteCustomerByApplicationID(app_id);

                //delete data in table [ct_fp_customer_sub] in case error in loop
                da_FP_Customer_Sub.deleteCustomerSubByApplicationID(app_id);
              
                result = false;
                Log.AddExceptionToLog("Function [saveCustomerSub()] in page [fp_application_form], Application ID is nothing ");
            }

            result = true;
        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error function [saveCustomerSub] in class [da_FP_Customer_Sub], Detail: " + ex.Message);
            result = false;
        }

        return result;
    }
    //get application id my application number
    private string getApplicationID(int applicationNumber) 
    {

        string applicationID = "";
        try
        {
            
            // get application id by application number
            List<bl_FP_App> applicationList = new List<bl_FP_App>();
            //format application number A+(8 degits)
            string newAppFormated = FP_Helper.formatApplicationNumber(applicationNumber);
            applicationList = da_FP_App.getApplicationList(newAppFormated);

            if (applicationList.Count > 0)
            {
                bl_FP_App myApp = applicationList[0];
                applicationID = myApp.App_id;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getApplicationID] in page [fp_application_form], Detai: " + ex.Message);
        }

        return applicationID;
        

    }
    // get valid for children

    //check valid life insured 3
    private bool validLifeInsured3()
    {
        // required fields
        // surname in kherm , surname in english, firstname in khmer, firstname in english, nationality, dob
        bool valid = false;
        if (txtSureNameENLife3.Text.Trim() != "" && txtSureNameLife3.Text.Trim() != "" 
            && txtFirstNameENLife3.Text.Trim() != "" && txtFirstNameLife3.Text.Trim() != ""
            && ddlNationalityLife3.SelectedIndex > 0 && txtDOBLife3.Text.Trim() != "" )
        {
            valid = true;
        }

        return valid;
    }
    //check valid life insured 4
    private bool validLifeInsured4()
    {
        // required fields
        // surname in kherm , surname in english, firstname in khmer, firstname in english, nationality, dob
        bool valid = false;
        if (txtSureNameENLife4.Text.Trim() != ""  && txtSureNameLife4.Text.Trim() != ""
            && txtFirstNameENLife4.Text.Trim() != "" && txtFirstNameLife4.Text.Trim() != ""
            && ddlNationalityLife4.SelectedIndex > 0 && txtDOBLife4.Text.Trim() != "")
        {
            valid = true;
        }

        return valid;
    }
    //check valid Life insured 2
    private bool validLifeInsured2()
    {
        bool valid = false;
        if (txtSureNameLife2.Text.Trim() != "" && txtSureNameENLife2.Text.Trim() != "" && txtFirstNameLife2.Text.Trim() != "" && txtFirstNameENLife2.Text.Trim() != ""
            && txtIDLife2.Text.Trim() != "" && ddlNationalityLife2.SelectedIndex > 0 && txtDOBLife2.Text.Trim() != "")
        {

            valid = true;
        }
        return valid;
    }

    //get Customer id my application ID
    private string getCustomerID(string application_id)
    {
        string customer_id = "";
        try
        {

            // get application id by application number
            List<bl_FP_Customer> customerList = new List<bl_FP_Customer>();

            customerList = da_FP_Customer.getCustomerList(application_id);

            if (customerList.Count > 0)
            {
                bl_FP_Customer myCust = customerList[0];
                customer_id = myCust.Customer_id;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [getCustomerID] in page [fp_application_form], Detai: " + ex.Message);
        }

        return customer_id;
    }

    //check valid address
    //save address
    private bool saveAddress()
    {
        bool result = false;
        try
        {
            string address_id = "";
            string customer_id = "";
            string address1 = "";
            string address2 = "";
            string created_by = "";
            DateTime created_on = DateTime.Now;
            string app_id = getApplicationID(Convert.ToInt32(txtApplicationNumber.Text.Trim()));

            address_id = Helper.GetNewGuid("SP_CHECK_FP_ADDRESS_ID", "@Address_ID");
            customer_id = getCustomerID(app_id);
            address1 = txtAddress1.Text.Trim();
            address2 = txtAddress2.Text.Trim();
            created_by = hdfusername.Value.Trim();


            if (!da_FP_Address.saveAddress(new bl_FP_Address(address_id, customer_id, address1, address2, created_by, created_on, null, created_on)))
            {
                result = false;
                //Delete data in table [ct_fp_app]
                string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                da_FP_App.deleteFPApp(appNumber);

                //delete data in table [ct_fp_customer] in case error in loop
                da_FP_Customer.deleteCustomerByApplicationID(app_id);

                //delete data in table [ct_fp_customer_sub] in case error in loop
                da_FP_Customer_Sub.deleteCustomerSubByApplicationID(app_id);

            }
            else {
                result = true;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [saveAddress] in page [fp_applicaton_form], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    //save Contact
    private bool saveContact()
    {
        bool result = false;
        try
        {
            string contact_id = "";
            string customer_id = "";
            string phone1 = "";
            string phone2 = "";
            string email = "";
            string created_by = "";
            DateTime created_on = DateTime.Now;
            string app_id = getApplicationID(Convert.ToInt32(txtApplicationNumber.Text.Trim()));

            contact_id = Helper.GetNewGuid("SP_CHECK_FP_CONTACT_ID", "@Contact_ID");
            customer_id = getCustomerID(app_id);
            phone1 = txtMobilePhone1.Text.Trim();
            phone2 = txtMobilePhone2.Text.Trim();
            email = txtEmail.Text.Trim();
            created_by = hdfusername.Value.Trim();


            if (!da_FP_Contact.saveContact(new bl_FP_Contact(contact_id, customer_id, phone1, phone2, email, created_by, created_on, null, created_on)))
            {
                result = false;
               
                //Delete data in table [ct_fp_app]
                string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                da_FP_App.deleteFPApp(appNumber);

                //delete data in table [ct_fp_customer] in case error in loop
                da_FP_Customer.deleteCustomerByApplicationID(app_id);

                //delete data in table [ct_fp_customer_sub] in case error in loop
                da_FP_Customer_Sub.deleteCustomerSubByApplicationID(app_id);

                //delete data in table [ct_fp_address] in case error in loop
                da_FP_Address.deleteAddress("",customer_id);

            }
            else
            {
                result = true;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [saveAddress] in page [fp_applicaton_form], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }

    //save answer
    private bool saveAnswer(string customer_id) {
        bool result = false;
        string app_id = getApplicationID(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
        try
        {

            List<bl_FP_Customer_sub> subCustomerList = da_FP_Customer_Sub.getSubCustomerList(app_id);

            int row = GvQA.Rows.Count;
            for(int i=0 ;i<=row-1;i++){

                HiddenField qId1 = (HiddenField)GvQA.Rows[i].FindControl("hdfQuestionIDLife1");
                HiddenField qId2 = (HiddenField)GvQA.Rows[i].FindControl("hdfQuestionIDLife2");
                HiddenField qId3 = (HiddenField)GvQA.Rows[i].FindControl("hdfQuestionIDLife3");
                HiddenField qId4 = (HiddenField)GvQA.Rows[i].FindControl("hdfQuestionIDLife4");

                RadioButtonList answer1 = (RadioButtonList)GvQA.Rows[i].FindControl("rbtnlAnswerLife1");
                RadioButtonList answer2 = (RadioButtonList)GvQA.Rows[i].FindControl("rbtnlAnswerLife2");
                RadioButtonList answer3 = (RadioButtonList)GvQA.Rows[i].FindControl("rbtnlAnswerLife3");
                RadioButtonList answer4 = (RadioButtonList)GvQA.Rows[i].FindControl("rbtnlAnswerLife4");

                bl_FP_Answer answer; 
              

               //life insured1
                if (txtSurnameEng.Text.Trim() != "" && txtSurnameKh.Text.Trim() != "" && txtFirstNameEng.Text.Trim() != "" && txtFirstNameKh.Text.Trim() != "" && txtDateBirth.Text.Trim()!="") {
                    answer = new bl_FP_Answer();
                    answer.Customer_id = customer_id;
                    answer.Answer_id = Helper.GetNewGuid("SP_CHECK_FP_ANSWER_ID", "@Answer_ID");
                    answer.Question_id = qId1.Value.Trim();
                    answer.Answer = answer1.SelectedValue.Trim();
                    answer.App_id = app_id;
                    if (!da_FP_Answer.saveAnswer(answer))
                    {
                        result = false;

                        //Delete data in table [ct_fp_app]
                        string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                        da_FP_App.deleteFPApp(appNumber);

                        //delete data in table [ct_fp_customer] in case error in loop
                        da_FP_Customer.deleteCustomerByApplicationID(app_id);

                        //delete data in table [ct_fp_customer_sub] in case error in loop
                        da_FP_Customer_Sub.deleteCustomerSubByApplicationID(app_id);

                        //delete data in table [ct_fp_address] in case error in loop
                        da_FP_Address.deleteAddress("", customer_id);
                        //delete contact
                        da_FP_Contact.deleteContact(customer_id);
                    }
                } 
                //life insured2
                if (txtSureNameENLife2.Text.Trim() != "" &&  txtSureNameLife2.Text.Trim() != "" && txtFirstNameENLife2.Text.Trim() != "" && txtFirstNameLife2.Text.Trim() != "" && txtDOBLife2.Text.Trim() != "")
                {
                    answer = new bl_FP_Answer();
                    answer.Answer_id = Helper.GetNewGuid("SP_CHECK_FP_ANSWER_SUB_ID", "@Answer_ID");
                    answer.Question_id = qId2.Value.Trim();
                    answer.Answer = answer2.SelectedValue.Trim();
                    answer.App_id = app_id;

                    for (int j = 0; j>=subCustomerList.Count - 1; j++) {
                        if (subCustomerList[j].Sub_number == 1) {
                            answer.Customer_id = subCustomerList[j].Sub_id;
                           // break;
                        }
                    }
                    
                    if (!da_FP_Answer.saveAnswerSub(answer))
                    {
                        result = false;
                        //Delete data in table [ct_fp_app]
                        string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                        da_FP_App.deleteFPApp(appNumber);

                        //delete data in table [ct_fp_customer] in case error in loop
                        da_FP_Customer.deleteCustomerByApplicationID(app_id);

                        //delete data in table [ct_fp_customer_sub] in case error in loop
                        da_FP_Customer_Sub.deleteCustomerSubByApplicationID(app_id);

                        //delete data in table [ct_fp_address] in case error in loop
                        da_FP_Address.deleteAddress("", customer_id);
                        //delete contact
                        da_FP_Contact.deleteContact(customer_id);
                        //delete answer
                        da_FP_Answer.deleteAnswer(app_id);

                    }
                }
                //life insured3
                if (txtSureNameENLife3.Text.Trim() != "" && txtSureNameLife3.Text.Trim() != "" && txtFirstNameENLife3.Text.Trim() != "" && txtFirstNameLife3.Text.Trim() != "" && txtDOBLife3.Text.Trim() != "")
                {
                    answer = new bl_FP_Answer();
                    answer.Answer_id = Helper.GetNewGuid("SP_CHECK_FP_ANSWER_SUB_ID", "@Answer_ID");
                    answer.Question_id = qId3.Value.Trim();
                    answer.Answer = answer3.SelectedValue.Trim();
                    answer.App_id = app_id;
                    for (int j = 0; j >= subCustomerList.Count - 1; j++)
                    {
                        if (subCustomerList[j].Sub_number == 2)
                        {
                            answer.Customer_id = subCustomerList[j].Sub_id;
                            //break;
                        }
                    }
                    if (!da_FP_Answer.saveAnswerSub(answer))
                    {
                        result = false;
                        //Delete data in table [ct_fp_app]
                        string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                        da_FP_App.deleteFPApp(appNumber);

                        //delete data in table [ct_fp_customer] in case error in loop
                        da_FP_Customer.deleteCustomerByApplicationID(app_id);

                        //delete data in table [ct_fp_customer_sub] in case error in loop
                        da_FP_Customer_Sub.deleteCustomerSubByApplicationID(app_id);

                        //delete data in table [ct_fp_address] in case error in loop
                        da_FP_Address.deleteAddress("", customer_id);
                        //delete contact
                        da_FP_Contact.deleteContact(customer_id);
                        //delete answer
                        da_FP_Answer.deleteAnswer(app_id);

                        //delete answer sub
                        da_FP_Answer.deleteAnswerSub(app_id);
                    }
                }
                //life insured4
                if (txtSureNameENLife4.Text.Trim() != "" && txtSureNameLife4.Text.Trim() != "" && txtFirstNameENLife4.Text.Trim() != "" && txtFirstNameLife4.Text.Trim() != "" && txtDOBLife4.Text.Trim() != "")
                {
                    answer = new bl_FP_Answer();
                    answer.Answer_id = Helper.GetNewGuid("SP_CHECK_FP_ANSWER_SUB_ID", "@Answer_ID");
                    answer.Question_id = qId4.Value.Trim();
                    answer.Answer = answer4.SelectedValue.Trim();
                    answer.App_id = app_id;
                     for (int j = 0; j>=subCustomerList.Count - 1; j++) {
                        if (subCustomerList[j].Sub_number == 3) {
                            answer.Customer_id = subCustomerList[j].Sub_id;
                           // break;
                        }
                     }
                    if (!da_FP_Answer.saveAnswerSub(answer))
                    {
                        result = false;
                        //Delete data in table [ct_fp_app]
                        string appNumber = FP_Helper.formatApplicationNumber(Convert.ToInt32(txtApplicationNumber.Text.Trim()));
                        da_FP_App.deleteFPApp(appNumber);

                        //delete data in table [ct_fp_customer] in case error in loop
                        da_FP_Customer.deleteCustomerByApplicationID(app_id);

                        //delete data in table [ct_fp_customer_sub] in case error in loop
                        da_FP_Customer_Sub.deleteCustomerSubByApplicationID(app_id);

                        //delete data in table [ct_fp_address] in case error in loop
                        da_FP_Address.deleteAddress("", customer_id);
                        //delete contact
                        da_FP_Contact.deleteContact(customer_id);
                        //delete answer
                        da_FP_Answer.deleteAnswer(app_id);

                        //delete answer sub
                        da_FP_Answer.deleteAnswerSub(app_id);
                    }
                } 
            
            }
            
           
        }
        catch (Exception ex) {
            Log.AddExceptionToLog("Error function [saveAnswer] in page [fp_application_form.aspx.cs], Detail; " + ex.Message);
            result = false;
            da_FP_Answer.deleteAnswer(app_id);//delete by application id
        }
        return result;
    
    }
   
    protected void ibtSave_Click(object sender, ImageClickEventArgs e)
    {

        bool result = true;
        //declared variable step to count life insurd 2,3 and 4
        int step = 0;

        #region //check exist application number
      
            string applicationID="";
            applicationID = getApplicationID(Int16.Parse(txtApplicationNumber.Text.Trim()));
            if (!applicationID.Equals(""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('This application number is already exist.')", true);
                return;
            
            }
      
        #endregion

        #region     //check life insured 2
        if (!validLifeInsured2())
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please input all required fields for Life Insured 2.')", true);
            return;
        }
        else 
        {
            step = step + 1;
        }
        #endregion

        #region     //check life insured 3
        //first check if name !="", it means need to fill all required fields
        if (txtSureNameENLife3.Text.Trim() != "" & txtSureNameLife3.Text.Trim() != "" && txtFirstNameENLife3.Text.Trim()!="" && txtFirstNameLife3.Text.Trim()!="")
        {
            if (!validLifeInsured3())
            {

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please input all required fields for Life Insured 3.')", true);
                return;
            }
            else
            {
                step = step + 1;
            }

        }
        #endregion

        #region      //check life insured 4
        //first check if name !="", it means need to fill all required fields
        if (txtSureNameENLife4.Text.Trim() != "" & txtSureNameLife4.Text.Trim() != "" && txtFirstNameENLife4.Text.Trim() != "" && txtFirstNameLife4.Text.Trim() != "")
        {
            if (!validLifeInsured4())
            {

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please input all required fields for Life Insured 4.')", true);
                return;
            }
            else
            {
                step = step + 1;
            }
        }
        #endregion
        #region //save data

        if (!saveFPApplication())
        {

            result = false;
            return;
        }
        if(!saveCustomer())
        {
            result = false;
            return;
        }
        if (!saveCustomerSub(step))
        {
            result = false;
        }
        if (!saveAddress())
        {
            result = false;
            return;
        }
        if (!saveContact())
        {
            result = false;
            return;
        }
        if (!saveAnswer(applicationID))
        {
            result = false;
            return;
        }
        #endregion

        if (result)
        {
        
              ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Success!.')", true);
        }
        else
        {
           ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saved application fail, Please contact your system administrator.')", true);
        }
        

    }
  /*  protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = ddlPlan.SelectedIndex;
        string planid= ddlPlan.SelectedValue.Trim();

        //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + planid + "')", true);

        if (index > 0)
        {
            List<bl_Policy_FP_Plan> myList = new List<bl_Policy_FP_Plan>();
            myList = da_Policy_FP_Plan.getPlanListByPlanID(planid);

            if (myList.Count > 0)
            {
                bl_Policy_FP_Plan plan = myList[0];

                txtSumInsuredLife1.Text = plan.Sum_insured1.ToString();
                txtSumInsuredLife2.Text = plan.Sum_insured2.ToString();
                txtSumInsuredLife3.Text = plan.Sum_insured3.ToString();
                txtSumInsuredLife4.Text = plan.Sum_insured4.ToString();


                txtCoverageYearLife1.Text = plan.Coverage_peroid.ToString();
                txtCoverageYearLife2.Text = plan.Coverage_peroid.ToString();
                txtCoverageYearLife3.Text = plan.Coverage_peroid.ToString();
                txtCoverageYearLife4.Text = plan.Coverage_peroid.ToString();

                txtPaymentPeroidLife1.Text = plan.Payment_peroid.ToString();
                txtPaymentPeroidLife2.Text = plan.Payment_peroid.ToString();
                txtPaymentPeroidLife3.Text = plan.Payment_peroid.ToString();
                txtPaymentPeroidLife4.Text = plan.Payment_peroid.ToString();

            }

        }
    }*/
    protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPlan.SelectedIndex > 0) {
            List<bl_Policy_FP_Plan> myPlanList = new List<bl_Policy_FP_Plan>();
            myPlanList = da_Policy_FP_Plan.getPlanListByPlanID(ddlPlan.SelectedValue.Trim());
            if (myPlanList.Count > 0)
            {
                var x = myPlanList[0];

                txtSumInsuredLife1.Text = x.Sum_insured1 + "";
                txtSumInsuredLife2.Text = x.Sum_insured2 + "";
                txtSumInsuredLife3.Text = x.Sum_insured3 + "";
                txtSumInsuredLife4.Text = x.Sum_insured4 + "";

                txtCoverageYearLife1.Text = x.Coverage_peroid + "";
                txtCoverageYearLife2.Text = x.Coverage_peroid + "";
                txtCoverageYearLife3.Text = x.Coverage_peroid + "";
                txtCoverageYearLife4.Text = x.Coverage_peroid + "";

                txtPaymentPeroidLife1.Text = x.Payment_peroid + "";
                txtPaymentPeroidLife2.Text = x.Payment_peroid + "";
                txtPaymentPeroidLife3.Text = x.Payment_peroid + "";
                txtPaymentPeroidLife4.Text = x.Payment_peroid + "";

                if (ddlPaymentMode.SelectedIndex > 0) { 
                
                    calCPremium();
                }
                

            }
           
        }
        else
        {
            clsInsurancePlanDetail();
        }
    }
    private void clsInsurancePlanDetail() {
        txtSumInsuredLife1.Text = "";
        txtSumInsuredLife2.Text = "";
        txtSumInsuredLife3.Text = "";
        txtSumInsuredLife4.Text = "";

        txtCoverageYearLife1.Text = "";
        txtCoverageYearLife2.Text = "";
        txtCoverageYearLife3.Text = "";
        txtCoverageYearLife4.Text = "";

        txtPaymentPeroidLife1.Text = "";
        txtPaymentPeroidLife2.Text = "";
        txtPaymentPeroidLife3.Text = "";
        txtPaymentPeroidLife4.Text = "";

        txtAgeLife1.Text = "";
        txtAgeLife2.Text = "";

        txtPremiumLife1.Text = "";
        txtPremiumLife2.Text = "";

        txtOriginalPremiumLife1.Text = "";
        txtOriginalPremiumLife2.Text = "";

        txtTotalOriginalPremium.Text = "";

        txtTotalPremium.Text = "";
    }
    private void calCPremium() {
        int ageLife1 = 0;
        int ageLife2 = 0;
        double premiumLife1 = 0;
        double premiumLife2 = 0;
        int gender = 0;
        int paymentMode = -1;
        string product_id = "FP";
        double original_premium1 = 0.0;
        double original_premium2 = 0.0;
        double total_Original_premium = 0.0;
        double total_premium = 0.0;

        paymentMode = Convert.ToInt32(ddlPaymentMode.SelectedValue.Trim());


        //get age
        //life insured1
        if (txtDateBirth.Text.Trim() != "")
        {
            ageLife1 = Calculation.Culculate_Customer_Age(txtDateBirth.Text.Trim(), Convert.ToDateTime(txtDateEntry.Text.Trim()));
            txtAgeLife1.Text = ageLife1 + "";
        }
        //life insured2
        if (txtDOBLife2.Text.Trim() != "")
        {
            ageLife2 = Calculation.Culculate_Customer_Age(txtDOBLife2.Text.Trim(), Convert.ToDateTime(txtDateEntry.Text.Trim()));
            txtAgeLife2.Text = ageLife2 + "";
        }

        //get premium
        //life insured1
        if (txtAgeLife1.Text.Trim() != "")
        {
            gender = Convert.ToInt32(ddlGender.SelectedValue.Trim());
            original_premium1 = da_FP_Premium.getOriginalPremium(product_id, gender, ageLife1);
            premiumLife1 = da_FP_Premium.getPremium(product_id, gender, ageLife1, paymentMode);
            txtOriginalPremiumLife1.Text = original_premium1 + "";
            txtPremiumLife1.Text = premiumLife1 + "";
        }
        //life insured1
        if (txtAgeLife2.Text.Trim() != "")
        {
            gender = Convert.ToInt32(ddlGenderLife2.SelectedValue.Trim());
            premiumLife2 = da_FP_Premium.getPremium(product_id, gender, ageLife2, paymentMode);
            original_premium2 = da_FP_Premium.getOriginalPremium(product_id, gender, ageLife2);
            txtPremiumLife2.Text = premiumLife2 + "";
            txtOriginalPremiumLife2.Text = original_premium2 + "";
        }

        //Original premium
        total_Original_premium = original_premium1 + original_premium2;
        txtTotalOriginalPremium.Text = total_Original_premium + "";
        total_premium = premiumLife1 + premiumLife2;
        txtTotalPremium.Text = total_premium + "";
    }
    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPlan.SelectedIndex > 0) {
            calCPremium();
        }
    }
    protected void ddlGender_SelectedIndexChanged(object sender, EventArgs e)
    {
        calCPremium();
       
    }
}