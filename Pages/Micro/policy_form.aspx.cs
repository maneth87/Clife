using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;


public partial class Pages_Business_policy_form : System.Web.UI.Page
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

            //diabled enter key on textbox
            txtIDNumber.Attributes.Add("onkeypress", "return event.keyCode!=13");

            txtFirstNameKh.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameEng.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameEng.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameKh.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtDateBirth.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAddress1.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAddress2.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtCity.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtZipCode.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtMobilePhone.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtEmail.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtInsuranceAmountRequired.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPremiumAmount.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBenefitSharePercentage.Attributes.Add("onkeypress", "return event.keyCode!=13");

            //Modal textboxes            
            txtIDCardNoSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtLastNameSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPolicyNumberSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBarcode.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBarcodeNumber.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPolicyNumber.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtMarketingCode.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtMarketingName.Attributes.Add("onkeypress", "return event.keyCode!=13");

            txtBenefitDOB.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBenefitName.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtProduct.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtTermInsurance.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPaymentPeriod.Attributes.Add("onkeypress", "return event.keyCode!=13");

            txtAddress1Edit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAddress2Edit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtAssureeAgeEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBarcodeNumberSearch.Attributes.Add("onkeypress", "return event.keyCode!=13");

            txtBenefitDOBEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBenefitNameEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtBenefitSharePercentageEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtDateBirthEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtEmailEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtFirstNameKhEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtIDNoEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtInsuranceAmountRequiredEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtMobilePhoneEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPaymentPeriodEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPremiumAmountEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtProductEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtProvinceEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameKhEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtSurnameEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtTermInsuranceEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtZipCodeEdit.Attributes.Add("onkeypress", "return event.keyCode!=13");
            txtPaymentCode.Attributes.Add("onkeypress", "return event.keyCode!=13");
        }

    }

    //Save New Policy
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string new_policy_number = "";
        string first_name = txtFirstNameEng.Text.Trim();
        string last_name = txtSurnameEng.Text.Trim();
        int gender = Convert.ToInt32(ddlGender.SelectedValue);

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
        dtfi2.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi2.DateSeparator = "/";


        //get sale agent
        string sale_agent_id = hdfMarketingCode.Value;

        hdfSaleAgentID.Value = sale_agent_id;

        //get channel, channel item and channel location (Sub 4 = Micro)
    
        string channel_item_id = ddlCompanyMicro.SelectedValue;
        string channel_channel_item_id = da_channel.GetChannelChannelItemIDByChannelSubIDAndChannelItemID(4, channel_item_id);

        hdfChannelChannelItem.Value = channel_channel_item_id;
        hdfChannelItem.Value = channel_item_id;      

        DateTime birth_date = Convert.ToDateTime(hdfDateBirth.Value, dtfi);

        string customer_micro_id = "";
        string customer_id = "";

        //check existing micro customer
        if (!da_policy_micro.CheckExistingMicroCustomer(first_name, last_name, txtFirstNameKh.Text.Trim(), txtSurnameKh.Text.Trim(), gender, birth_date))
        {

            //Add new micro customer
            bl_micro_customer micro_customer = new bl_micro_customer();
            micro_customer.Birth_Date = birth_date;
            micro_customer.First_Name = first_name;
            micro_customer.Created_By = hdfusername.Value;
            micro_customer.Created_Note = "";
            micro_customer.Created_On = DateTime.Now;
            micro_customer.Last_Name = last_name;
            micro_customer.Gender = Convert.ToInt32(ddlGender.SelectedValue);
            micro_customer.ID_Card = txtIDNumber.Text;
            micro_customer.ID_Type = Convert.ToInt32(ddlIDType.SelectedValue);
            micro_customer.Khmer_First_Name = txtFirstNameKh.Text.Trim();
            micro_customer.Khmer_Last_Name = txtSurnameKh.Text.Trim();
            micro_customer.Last_Name = txtSurnameEng.Text.Trim();

            customer_micro_id = da_policy_micro.InsertMicroCustomer(micro_customer);

            if (customer_micro_id == "")
            {

                //failed insert micro customer
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                Clear();
                return;
            }

            if (first_name != "" && last_name != "")
            {
                //check customer in Ct_Customer
                if (da_customer.CheckExistingCustomer(first_name, last_name, gender, birth_date))
                {
                    //get existing customer id
                    customer_id = da_customer.GetCustomerIDByNameDOBGender(first_name, last_name, gender, birth_date);

                    if (customer_id == "")
                    {
                        //failed get customer id
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                        Clear();
                        return;
                    }
                    else
                    {
                        bl_ct_customer_micro_customer customer_micro_customer = new bl_ct_customer_micro_customer();
                        customer_micro_customer.Customer_ID = customer_id;
                        customer_micro_customer.Customer_Micro_ID = customer_micro_id;

                        if (!da_policy_micro.InsertCustomerMicroCustomer(customer_micro_customer))
                        {
                            //failed get customer id
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                            Clear();
                            return;
                        }
                    }
                }

            }

        }
        else
        {
            //get customer_micro_id
            customer_micro_id = da_policy_micro.GetExistingMicroCustomerID(first_name, last_name, txtFirstNameKh.Text.Trim(), txtSurnameKh.Text.Trim(), gender, birth_date);

        }

        if (customer_micro_id != "")
        {
            //successful

            //Policy Micro
            bl_policy_micro policy_micro = new bl_policy_micro();

            hdfEntryDate.Value += " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":00";

            policy_micro.Age_Insure = Convert.ToInt32(hdfAssureeAge.Value);
            policy_micro.Agreement_Date = Convert.ToDateTime(hdfEntryDate.Value, dtfi2);         
            policy_micro.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
            policy_micro.Assure_Up_To_Age = policy_micro.Age_Insure + policy_micro.Assure_Year;
            policy_micro.Channel_Channel_Item_ID = hdfChannelChannelItem.Value;
            policy_micro.Channel_Location_ID = hdfChannelLocation.Value;
            policy_micro.Created_By = hdfusername.Value;
            policy_micro.Created_Note = "";
            policy_micro.Created_On = DateTime.Now;
            policy_micro.Customer_ID = customer_micro_id;            

            policy_micro.Effective_Date = Convert.ToDateTime(hdfEntryDate.Value, dtfi2).AddHours(24);

            policy_micro.Issue_Date = Convert.ToDateTime(hdfEntryDate.Value, dtfi2); //today
            policy_micro.Maturity_Date = policy_micro.Effective_Date.AddYears(policy_micro.Assure_Year).AddDays(-1);
          
            //case term one product payment is single
            policy_micro.Pay_Up_To_Age = 0;

            policy_micro.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);
            policy_micro.Product_ID = hdfProduct.Value;         
                       
            string policy_micro_id = da_policy_micro.InsertPolicyMicro(policy_micro);

            if (policy_micro_id != "")
            {
                //successfull
                if (!da_policy_micro.InsertPolicyID(policy_micro_id, 3)) // 3 = type micro
                {
                    da_policy_micro.DeletePolicyMicro(policy_micro_id);

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                    Clear();
                }
                else
                {
                    //sucessfull
                    //New policy number from Ct_Policy_Number

                    string last_policy_number = da_policy_micro.GetLastPolicyNumberMicro();

                    //Convert policy number to int and plus 1
                    int number = Convert.ToInt32(last_policy_number) + 1;

                    new_policy_number = number.ToString();

                    //Concate 0 to the front
                    while (new_policy_number.Length < 8)
                    {
                        new_policy_number = "0" + new_policy_number;
                    }

                    if (!da_policy_micro.InsertPolicyNumber(policy_micro_id, new_policy_number))
                    {
                        da_policy_micro.DeletePolicyID(policy_micro_id);                       
                        da_policy_micro.DeletePolicyMicro(policy_micro_id);
                      
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                        Clear();
                    }
                    else
                    {
                        
                        //successfull
                        //Policy Micro and micro Card
                        bl_policy_micro_banc_card policy_card = new bl_policy_micro_banc_card();

                        policy_card.Card_ID = hdfBarcodeNumber.Value;
                        policy_card.Created_By = hdfusername.Value;
                        policy_card.Created_Note = "";
                        policy_card.Created_On = DateTime.Now;
                        policy_card.Policy_Micro_ID = policy_micro_id;

                        if (!da_policy_micro.InsertPolicyMicroCard(policy_card))
                        {
                            //failed                     
                            da_policy_micro.DeletePolicyID(policy_micro_id);
                            da_policy_micro.DeletePolicyNumber(policy_micro_id);
                            da_policy_micro.DeletePolicyMicro(policy_micro_id);
                         

                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                            Clear();
                        }
                        else
                        {
                            //successfull
                            //Info Person
                            bl_policy_micro_info_person person = new bl_policy_micro_info_person();
                            person.Birth_Date = birth_date;
                            person.First_Name = txtFirstNameEng.Text.Trim();
                            person.Gender = Convert.ToInt32(ddlGender.SelectedValue);
                            person.ID_Card = txtIDNumber.Text.Trim();
                            person.ID_Type = Convert.ToInt32(ddlIDType.SelectedValue);
                            person.Khmer_First_Name = txtFirstNameKh.Text.Trim();
                            person.Khmer_Last_Name = txtSurnameKh.Text.Trim();
                            person.Last_Name = txtSurnameEng.Text.Trim();

                            person.Policy_Micro_ID = policy_micro_id;

                            if (!da_policy_micro.InsertPolicyMicroInfoPerson(person))
                            {
                                //failed                           
                                da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                da_policy_micro.DeletePolicyID(policy_micro_id);
                                da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                da_policy_micro.DeletePolicyMicro(policy_micro_id);
                          

                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                Clear();

                            }
                            else
                            {
                                //successful
                                //Info Address
                                bl_policy_micro_info_address address = new bl_policy_micro_info_address();
                                address.Address1 = txtAddress1.Text.Trim();
                                address.Address2 = txtAddress2.Text.Trim();
                                address.Address3 = "";
                                address.Country_ID = ddlCountry.SelectedValue;
                                address.Policy_Micro_ID = policy_micro_id;
                                address.Province = txtCity.Text.Trim();
                                address.Zip_Code = hdfZipCode.Value;

                                if (!da_policy_micro.InsertPolicyMicroInfoAddress(address))
                                {
                                    //failed                             
                                    da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                    da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                    da_policy_micro.DeletePolicyID(policy_micro_id);
                                    da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                    da_policy_micro.DeletePolicyMicro(policy_micro_id);
                                
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                    Clear();
                                }
                                else
                                {
                                    //successfull
                                    //Insurance Plan
                                    bl_policy_micro_life_product product = new bl_policy_micro_life_product();

                                    product.Age_Insure = Convert.ToInt32(hdfAssureeAge.Value);
                                    
                                    product.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);

                                    product.Assure_Up_To_Age = product.Age_Insure + product.Assure_Year;
                                    
                                    product.Pay_Mode = 0; //Single Payment
                                    product.Pay_Up_To_Age = 0;
                                    product.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);
                                    product.Policy_Micro_ID = policy_micro_id;
                                    product.Product_ID = hdfProduct.Value;
                                    product.System_Premium = 0;
                                    product.System_Premium_Discount = 0;
                                    product.System_Sum_Insure = 0;
                                    product.User_Premium = Convert.ToDouble(txtPremiumAmount.Text.Trim());
                                    product.User_Sum_Insure = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim());

                                    if (!da_policy_micro.InsertPolicyMicroLifeProduct(product))
                                    {
                                        //failed 
                                        da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                        da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                        da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                        da_policy_micro.DeletePolicyID(policy_micro_id);
                                        da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                        da_policy_micro.DeletePolicyMicro(policy_micro_id);
                                  

                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                        Clear();
                                    }
                                    else
                                    {
                                        //successfull
                                        //Beneficiaries
                                        if (!InsertBeneficiaries(policy_micro_id))
                                        {
                                            //failed
                                            da_policy_micro.DeletePolicyMicroBenefitItem(policy_micro_id);
                                            da_policy_micro.DeletePolicyMicroProposedInsuredItem(policy_micro_id);
                                            da_policy_micro.DeletePolicyMicroLifeProduct(policy_micro_id);
                                            da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                            da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                            da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                            da_policy_micro.DeletePolicyID(policy_micro_id);
                                            da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                            da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                            Clear();
                                        }
                                        else
                                        {
                                            //successfull
                                            bl_policy_micro_info_contact contact = new bl_policy_micro_info_contact();
                                            contact.EMail = txtEmail.Text.Trim();
                                            contact.Fax1 = "";
                                            contact.Fax2 = "";
                                            contact.Home_Phone1 = "";
                                            contact.Home_Phone2 = "";
                                            contact.Mobile_Phone1 = txtMobilePhone.Text.Trim();
                                            contact.Mobile_Phone2 = "";
                                            contact.Office_Phone1 = "";
                                            contact.Office_Phone2 = "";
                                            contact.Policy_Micro_ID = policy_micro_id;

                                            if (!da_policy_micro.InsertPolicyMicroInfoContact(contact))
                                            {
                                                //failed
                                                da_policy_micro.DeletePolicyMicroBenefitItem(policy_micro_id);
                                                da_policy_micro.DeletePolicyMicroProposedInsuredItem(policy_micro_id);
                                                da_policy_micro.DeletePolicyMicroLifeProduct(policy_micro_id);
                                                da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                                da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                da_policy_micro.DeletePolicyID(policy_micro_id);
                                                da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                                Clear();
                                            }
                                            else
                                            {
                                                bl_policy_micro_premium policy_premium = new bl_policy_micro_premium();

                                                policy_premium.Created_By = hdfusername.Value;
                                                policy_premium.Created_Note = "";
                                                policy_premium.Created_On = DateTime.Now;
                                                policy_premium.Original_Amount = Convert.ToDouble(txtPremiumAmount.Text.Trim());
                                                policy_premium.Premium = Convert.ToDouble(txtPremiumAmount.Text.Trim());
                                                policy_premium.Sum_Insure = Convert.ToDouble(txtInsuranceAmountRequired.Text.Trim());
                                                policy_premium.Policy_Micro_ID = policy_micro_id;

                                                if (!da_policy_micro.InsertPolicyMicroPremium(policy_premium))
                                                {
                                                    //failed
                                                    da_policy_micro.DeletePolicyMicroBenefitItem(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicroProposedInsuredItem(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicroLifeProduct(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicroInfoContact(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                    da_policy_micro.DeletePolicyID(policy_micro_id);
                                                    da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicro(policy_micro_id);


                                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                                    Clear();
                                                }
                                                else
                                                {

                                                    //successfull
                                                    bl_policy_micro_status policy_status = new bl_policy_micro_status();

                                                    policy_status.Created_By = hdfusername.Value;
                                                    policy_status.Created_Note = "";
                                                    policy_status.Created_On = DateTime.Now;
                                                    policy_status.Policy_Micro_ID = policy_micro_id;
                                                    policy_status.Policy_Status_Type_ID = "IF";

                                                    if (!da_policy_micro.InsertPolicyMicroStatus(policy_status))
                                                    {
                                                        //failed
                                                        da_policy_micro.DeletePolicyMicroBenefitItem(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroProposedInsuredItem(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroLifeProduct(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroInfoContact(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                        da_policy_micro.DeletePolicyID(policy_micro_id);
                                                        da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroPremium(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicro(policy_micro_id);
                                                      

                                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                                        Clear();
                                                    }
                                                    else
                                                    {
                                                        bl_policy_micro_prem_pay micro_prem_pay = new bl_policy_micro_prem_pay();
                                                        micro_prem_pay.Amount = Convert.ToDouble(txtPremiumAmount.Text.Trim());
                                                        micro_prem_pay.Channel_Location_ID = hdfChannelLocation.Value;
                                                        micro_prem_pay.Created_By = hdfusername.Value;
                                                        micro_prem_pay.Created_Note = "";
                                                        micro_prem_pay.Created_On = DateTime.Now;
                                                        micro_prem_pay.Due_Date = policy_micro.Maturity_Date;
                                                        micro_prem_pay.Pay_Date = policy_micro.Effective_Date;
                                                        micro_prem_pay.Policy_Micro_ID = policy_micro.Policy_Micro_ID;
                                                        micro_prem_pay.Prem_Lot = 1;
                                                        micro_prem_pay.Prem_Year = 1;
                                                        micro_prem_pay.Sale_Agent_ID = hdfMarketingCode.Value;
                                                        micro_prem_pay.Policy_Micro_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_Policy_Micro_Prem_Pay_ID", "@Policy_Micro_Prem_Pay_ID");
                                                        micro_prem_pay.Payment_Code = txtPaymentCode.Text.Trim();

                                                        if (!da_policy_micro.InsertPolicyMicroPremPay(micro_prem_pay))
                                                        {
                                                            //failed
                                                            da_policy_micro.DeletePolicyMicroBenefitItem(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroProposedInsuredItem(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroLifeProduct(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroInfoContact(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                            da_policy_micro.DeletePolicyID(policy_micro_id);
                                                            da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroPremium(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                                                            Clear();
                                                        }
                                                        else
                                                        {
                                                            //change card status
                                                            da_banc_card.UpdateCardStatus(policy_card.Card_ID, 0);
                                                            Clear();

                                                            //successfull
                                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy successfull.')", true);
                                                                                                                  

                                                        }

                                                      
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //failed insert policy micro
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again')", true);
                Clear();
            }

        }


    }

    protected bool InsertBeneficiaries(string policy_micro_id)
    {
        try
        {
            string[] benefit_name_arr = hdfBenefitName.Value.Split('}');

            string[] benefit_dob_arr = hdfBenefitDOB.Value.Split('}');

            string[] benefit_relation_arr = hdfBenefitRelation.Value.Split('}');

            string[] benefit_share_arr = hdfBenefitShare.Value.Split('}');

            int seq_number = 0;

            for (int i = 0; i < benefit_name_arr.Count() - 1; i++)
            {
                if (benefit_relation_arr[i].ToString() != "undefined")
                {
                    if (benefit_share_arr[i].ToString() != "")
                    {
                        string name = benefit_name_arr[i];

                        if (name == "N/A")
                        {
                            name = "";
                        }

                        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                        dtfi.ShortDatePattern = "dd/MM/yyyy";
                        dtfi.DateSeparator = "/";

                        DateTime birth_date = Convert.ToDateTime(benefit_dob_arr[i], dtfi);

                        string relation = benefit_relation_arr[i];


                        double share = Convert.ToDouble(benefit_share_arr[i]);
                        seq_number += 1;

                        string new_guid = Helper.GetNewGuid("SP_Check_Policy_Micro_Benefit_Item_ID", "@Policy_Micro_Benefit_Item_ID").ToString();

                        bl_policy_micro_benefit_item benefit_item = new bl_policy_micro_benefit_item();

                        benefit_item.Policy_Micro_ID = policy_micro_id;
                        benefit_item.Policy_Micro_Benefit_Item_ID = new_guid;

                        benefit_item.Birth_Date = birth_date;
                        benefit_item.Full_Name = name.ToUpper();
                        benefit_item.Percentage = share;
                        benefit_item.Relationship = relation;
                        benefit_item.Seq_Number = seq_number;
                        benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);

                        if (!da_policy_micro.InsertPolicyMicroBenefitItem(benefit_item))
                        {
                            //failed
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertBeneficiaries] in class [policy_form.aspx.cs]. Details: " + ex.Message);
            return false;
        }
    }

    protected void Clear()
    {
        hdfPolicyID.Value = "";
        ddlIDType.SelectedIndex = 0;
        txtIDNumber.Text = "";
        txtSurnameKh.Text = "";
        txtFirstNameKh.Text = "";
        txtSurnameEng.Text = "";
        txtFirstNameEng.Text = "";
        ddlGender.SelectedIndex = 0;

        txtDateBirth.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtCity.Text = "";
        txtZipCode.Text = "";
        ddlCountry.SelectedIndex = 0;
        txtMobilePhone.Text = "";
        txtInsuranceAmountRequired.Text = "";
        txtPremiumAmount.Text = "";

        txtMarketingCode.Text = "";
        txtMarketingName.Text = "";
        ddlCompanyMicro.SelectedIndex = 0;

    }

    //Clear Button Click
    protected void ImgBtnClear_Click(object sender, ImageClickEventArgs e)
    {
        Clear();
    }
}