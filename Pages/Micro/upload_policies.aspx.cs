using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Business_upload_policies : System.Web.UI.Page
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

        }
    }

    //Upload Micro Policies using Excel file
    protected void ImgBtnUpload_Click(object sender, ImageClickEventArgs e)
    {
        bl_banc_card available_card_check = da_banc_card.GetFirstAvailableCard(hdfProduct.Value);
        int available_cards = da_banc_card.GetAvailableCardCount(hdfProduct.Value);
        if (available_card_check.Card_ID == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('No available term one banc card.')", true);
            Clear();
            return;
        }
              
        string new_policy_number = "";

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
        dtfi2.ShortDatePattern = "dd/MM/yyyy";
        dtfi2.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";


        //get sale agent by user id
        string sale_agent_id = hdfMarketingCode.Value;

        hdfSaleAgentID.Value = sale_agent_id;
        hdfSaleAgentName.Value = da_sale_agent.GetSaleAgentNameByID(sale_agent_id);

        //get channel, channel item and channel location
        
        string channel_item_id = ddlCompanyMicro.SelectedValue;
        string channel_channel_item_id = da_channel.GetChannelChannelItemIDByChannelSubIDAndChannelItemID(4, channel_item_id);


        hdfChannelChannelItem.Value = channel_channel_item_id;
        hdfChannelItem.Value = channel_item_id;

        hdfEntryDate.Value += " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":00";
        
        try
        {
            //check if file upload contain any file                      
            if ((FileUploadMicroPolicy.PostedFile != null) && !string.IsNullOrEmpty(FileUploadMicroPolicy.PostedFile.FileName))
            {                

                string save_path = "~/Upload/Micro/";
                string file_name = Path.GetFileName(FileUploadMicroPolicy.PostedFile.FileName);
                string content_type = FileUploadMicroPolicy.PostedFile.ContentType;
                int content_length = FileUploadMicroPolicy.PostedFile.ContentLength;

                FileUploadMicroPolicy.PostedFile.SaveAs(Server.MapPath(save_path + file_name));
                string version = Path.GetExtension(file_name);

                string file_path = Server.MapPath(save_path + file_name).ToString();

                //verify if the file has been save           
                if (version == ".xls")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + file_path + "';Extended Properties=Excel 8.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[Sheet1$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    if (available_cards < dt.Rows.Count)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('available term one banc cards is less than required cards.')", true);
                        Clear();
                        return;
                    }


                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //Birth Certificate
                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input of Birth Certificate # field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //ID Type
                        switch (dt.Rows[i][3].ToString().Trim())
                        {
                            case "I.D Card":
                            case "Passport":
                            case "Visa":
                            case "Birth Certificate":
                            case "Police / Civil Service Card":
                            case "Employment Book":
                            case "Residential Book":
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for ID Type field then try again. Row number: '" + (i + 1) + "";
                                return;
                        }

                        //Last Name English
                        if (dt.Rows[i][7].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Last Name field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }
                        
                        //First Name English
                        if (dt.Rows[i][6].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for First Name field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }
                        
                        //Gender
                        if (dt.Rows[i][9].ToString().Trim().Equals(""))
                        {                           
                                lblMessage.Text = "Please check your input for Gender field then try again. Row number: '" + (i + 1) + "";
                                return;
                            
                        }else{
                            if(!dt.Rows[i][9].ToString().Trim().Equals("M") && !dt.Rows[i][9].ToString().Trim().Equals("F"))
                            {
                                lblMessage.Text = "Please check your input for Gender field then try again. Row number: '" + (i + 1) + "";
                                return;
                            }
                        }

                        //Date of Birth
                        DateTime value;
                        if (!DateTime.TryParse(Convert.ToDateTime(dt.Rows[i][8], dtfi).ToString(), out value))
                        {
                            lblMessage.Text = "Please check your input for Date of Birth field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //Bneficiary
                        if (dt.Rows[i][10].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                         //Relation 
                        if (dt.Rows[i][11].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Relation field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //relation check case ??????????
                        switch (dt.Rows[i][11].ToString().Trim().ToUpper())
                        {
                            case "M":
                            case "F":
                            case "G":
                            
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for Relation field then try again. Row number: '" + (i + 1) + ")";
                                return;                                
                        }
                    }

                    int rows_saved = 0;

                    //loop excel file rows to save policy
                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {
                         
                         string first_name = dt.Rows[k][6].ToString().Trim();
                         string last_name = dt.Rows[k][7].ToString().Trim();
                         string str_genter = dt.Rows[k][9].ToString().Trim();

                         int gender = 0;

                         if (str_genter == "M"){
                             gender = 1;
                         }

                         int id_type = 0;

                         //ID Type
                         switch (dt.Rows[k][3].ToString().Trim())
                         {
                             case "I.D Card":
                                 id_type = 0;
                                 break;
                             case "Passport":
                                 id_type = 1;
                                 break;
                             case "Visa":
                                 id_type = 2;
                                 break;
                             case "Birth Certificate":
                                 id_type = 2;
                                 break;
                             case "Police / Civil Service Card":
                                 id_type = 4;
                                 break;
                             case "Employment Book":
                                 id_type = 5;
                                 break;
                             case "Residential Book":
                                 id_type = 6;
                                 break;                         
                         }

                        DateTime birth_date = Convert.ToDateTime(dt.Rows[k][8].ToString().Trim(), dtfi);

                        //get next available barcode with order by card_number ASC and status == 1 and not in ct_policy_micro_banc_card
                        string customer_micro_id = "";
                        string customer_id = "";

                        //check existing micro customer
                        if (!da_policy_micro.CheckExistingMicroCustomer(first_name, last_name, dt.Rows[k][4].ToString().Trim(), dt.Rows[k][5].ToString().Trim(), gender, birth_date))
                        {

                            //Add new micro customer
                            bl_micro_customer micro_customer = new bl_micro_customer();
                            micro_customer.Birth_Date = birth_date;
                            micro_customer.First_Name = first_name;
                            micro_customer.Created_By = hdfusername.Value;
                            micro_customer.Created_Note = "";
                            micro_customer.Created_On = DateTime.Now;                         
                            micro_customer.Gender = gender;
                            micro_customer.ID_Card = dt.Rows[k][2].ToString().Trim();
                            micro_customer.ID_Type = id_type;
                            micro_customer.Khmer_First_Name = dt.Rows[k][4].ToString().Trim();
                            micro_customer.Khmer_Last_Name = dt.Rows[k][5].ToString().Trim();
                            micro_customer.Last_Name = last_name;

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
                            customer_micro_id = da_policy_micro.GetExistingMicroCustomerID(first_name, last_name, dt.Rows[k][4].ToString().Trim(), dt.Rows[k][5].ToString().Trim(), gender, birth_date);

                        }

                  
                         //successful then save new policy for this row
                          if (customer_micro_id != "")
                          {
                               
                                //Policy Micro
                                bl_policy_micro policy_micro = new bl_policy_micro();

                                policy_micro.Age_Insure =  Calculation.Culculate_Customer_Age_Micro(birth_date, System.DateTime.Now.AddHours(24));
                                policy_micro.Agreement_Date = Convert.ToDateTime(hdfEntryDate.Value, dtfi3);
                                policy_micro.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                                policy_micro.Assure_Up_To_Age = policy_micro.Age_Insure +  policy_micro.Assure_Year;
                                policy_micro.Channel_Channel_Item_ID = hdfChannelChannelItem.Value;
                                policy_micro.Channel_Location_ID = hdfChannelLocation.Value;
                                policy_micro.Created_By = hdfusername.Value;
                                policy_micro.Created_Note = "";
                                policy_micro.Created_On = DateTime.Now;
                                policy_micro.Customer_ID = customer_micro_id;
                                policy_micro.Effective_Date = Convert.ToDateTime(hdfEntryDate.Value, dtfi3).AddHours(24);

                                policy_micro.Issue_Date = DateTime.Now;
                                policy_micro.Maturity_Date = policy_micro.Effective_Date.AddYears(policy_micro.Assure_Year);

                                policy_micro.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                                //case term one product payment is single
                                policy_micro.Pay_Up_To_Age = 0;
                                                            
                                policy_micro.Product_ID = hdfProduct.Value;
                               
                              
                                string policy_micro_id = da_policy_micro.InsertPolicyMicro(policy_micro);

                                if (policy_micro_id != "")
                                {
                                    //successfull
                                    if (!da_policy_micro.InsertPolicyID(policy_micro_id, 3)) //3 = type micro
                                    {
                                        da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);

                                        ShowResult(rows_saved, dt.Rows.Count, dt);
                                        return;
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

                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                            ShowResult(rows_saved, dt.Rows.Count, dt);
                                            return;
                  
                                        }
                                        else
                                        {
                        
                                            //successfull
                                            //Policy Micro and micro Card
                                            bl_policy_micro_banc_card policy_card = new bl_policy_micro_banc_card();

                                            bl_banc_card available_card = da_banc_card.GetFirstAvailableCard(hdfProduct.Value);

                                            policy_card.Card_ID = available_card.Card_ID; //get available card
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


                                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                ShowResult(rows_saved, dt.Rows.Count, dt);
                                                return;
                   
                                            }
                                            else
                                            {
                                                //successfull
                                                //Info Person
                                                bl_policy_micro_info_person person = new bl_policy_micro_info_person();
                                                person.Birth_Date = birth_date;
                                                person.First_Name = first_name;
                                                person.Gender = gender;
                                                person.ID_Card = dt.Rows[k][2].ToString().Trim();
                                                person.ID_Type = id_type;
                                                person.Khmer_First_Name = dt.Rows[k][4].ToString().Trim();
                                                person.Khmer_Last_Name =dt.Rows[k][5].ToString().Trim();
                                                person.Last_Name = last_name;

                                                person.Policy_Micro_ID = policy_micro_id;

                                                if (!da_policy_micro.InsertPolicyMicroInfoPerson(person))
                                                {
                                                    //failed                           
                                                    da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                    da_policy_micro.DeletePolicyID(policy_micro_id);
                                                    da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                    ShowResult(rows_saved, dt.Rows.Count, dt);
                                                    return;
                                                }
                                                else
                                                {
                                                    //successful
                                                    //Info Address
                                                    bl_policy_micro_info_address address = new bl_policy_micro_info_address();
                                                    address.Address1 = "";
                                                    address.Address2 = "";
                                                    address.Address3 = "";
                                                    address.Country_ID = "KH";
                                                    address.Policy_Micro_ID = policy_micro_id;
                                                    address.Province = "";
                                                    address.Zip_Code = "855";

                                                    if (!da_policy_micro.InsertPolicyMicroInfoAddress(address))
                                                    {
                                                        //failed                             
                                                        da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                        da_policy_micro.DeletePolicyID(policy_micro_id);
                                                        da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                        ShowResult(rows_saved, dt.Rows.Count, dt);
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        //successfull
                                                        //Insurance Plan
                                                        bl_policy_micro_life_product product = new bl_policy_micro_life_product();

                                                        product.Age_Insure = Calculation.Culculate_Customer_Age_Micro(birth_date, System.DateTime.Now.AddHours(24));
                                   
                                                        product.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                                                        product.Assure_Up_To_Age =  product.Age_Insure + product.Assure_Year;

                                                        product.Pay_Mode = 0; //Single Payment
                                                                       
                                                        product.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                                                        product.Pay_Up_To_Age = product.Age_Insure;

                                                        product.Policy_Micro_ID = policy_micro_id;
                                                        product.Product_ID = hdfProduct.Value;
                                                        product.System_Premium = 0;
                                                        product.System_Premium_Discount = 0;
                                                        product.System_Sum_Insure = 0;
                                                        product.User_Premium = Convert.ToDouble(hdfPremiumAmount.Value);
                                                        product.User_Sum_Insure = Convert.ToDouble(hdfSumInsured.Value);

                                                        if (!da_policy_micro.InsertPolicyMicroLifeProduct(product))
                                                        {
                                                            //failed 
                                                            da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                            da_policy_micro.DeletePolicyID(policy_micro_id);
                                                            da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                            da_policy_micro.DeletePolicyMicro(policy_micro_id);


                                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                            ShowResult(rows_saved, dt.Rows.Count, dt);
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            //successfull
                                                            //Beneficiary
                                                            bl_policy_micro_benefit_item benefit_item = new bl_policy_micro_benefit_item();
                                                            benefit_item.Seq_Number = 1;
                                                            benefit_item.Address = "";
                                                            benefit_item.Birth_Date = Convert.ToDateTime("01/01/1900", dtfi2); //no benefitciary date of birth supplied
                                                            benefit_item.Full_Name  = dt.Rows[k][10].ToString().Trim();
                                                            benefit_item.Percentage = 100;
                                                            benefit_item.Policy_Micro_ID = policy_micro_id;

                                                            if (dt.Rows[k][11].ToString().Trim().ToUpper() == "F")
                                                            {
                                                                benefit_item.Relationship = "FATHER";
                                                            }
                                                            else if (dt.Rows[k][11].ToString().Trim().ToUpper() == "M")
                                                            {
                                                                benefit_item.Relationship = "MOTHER";
                                                            }
                                                            else if (dt.Rows[k][11].ToString().Trim().ToUpper() == "G")
                                                            {
                                                                benefit_item.Relationship = "GUIDANCE";
                                                            }

                                                            benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);

                                                            benefit_item.Policy_Micro_Benefit_Item_ID = Helper.GetNewGuid("SP_Check_Policy_Micro_Benefit_Item_ID", "@Policy_Micro_Benefit_Item_ID").ToString();
                                      
                                                            if (!da_policy_micro.InsertPolicyMicroBenefitItem(benefit_item))
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

                                                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                                ShowResult(rows_saved, dt.Rows.Count, dt);
                                                                return;
                                                            }
                                                            else
                                                            {
                                                                //successfull
                                                                bl_policy_micro_info_contact contact = new bl_policy_micro_info_contact();
                                                                contact.EMail = "";
                                                                contact.Fax1 = "";
                                                                contact.Fax2 = "";
                                                                contact.Home_Phone1 = "";
                                                                contact.Home_Phone2 = "";
                                                                contact.Mobile_Phone1 = "";
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

                                                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                                    ShowResult(rows_saved, dt.Rows.Count, dt);
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    bl_policy_micro_premium policy_premium = new bl_policy_micro_premium();

                                                                    policy_premium.Created_By = hdfusername.Value;
                                                                    policy_premium.Created_Note = "";
                                                                    policy_premium.Created_On = DateTime.Now;
                                                                    policy_premium.Original_Amount = Convert.ToDouble(hdfPremiumAmount.Value);
                                                                    policy_premium.Premium = Convert.ToDouble(hdfPremiumAmount.Value);
                                                                    policy_premium.Sum_Insure = Convert.ToDouble(hdfSumInsured.Value);
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


                                                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                                        ShowResult(rows_saved, dt.Rows.Count, dt);
                                                                        return;
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


                                                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                                            ShowResult(rows_saved, dt.Rows.Count, dt);
                                                                            return;
                                                                        }
                                                                        else
                                                                        {
                                                                            //successfull                                                    
                                                                            bl_policy_micro_prem_pay micro_prem_pay = new bl_policy_micro_prem_pay();
                                                                            micro_prem_pay.Amount = Convert.ToDouble(hdfPremiumAmount.Value);
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
                                                                                rows_saved = k + 1;

                                                                                //Policy Pay Mode
                                                                                da_policy.InsertPolicyPayMode("", policy_micro.Policy_Micro_ID, 0, policy_micro.Maturity_Date, hdfusername.Value, policy_micro.Created_On);                    

                                                                                //change card status
                                                                                da_banc_card.UpdateCardStatus(policy_card.Card_ID, 0);

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
                            }
                            else
                            {                             
                                //failed insert policy micro
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                ShowResult(rows_saved, dt.Rows.Count, dt);
                                return;
                            }

                    }//end loop

                    //if reach here all save successfully
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy successfull.')", true);

               
                    ShowResult(rows_saved, dt.Rows.Count, dt);
                    return;      
                }
                else if (version == ".xlsx")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + file_path + "';Extended Properties=Excel 12.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[Sheet1$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    if (available_cards < dt.Rows.Count)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('available term one banc cards is less than required cards.')", true);
                        Clear();
                        return;
                    }

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //Birth Certificate
                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input of Birth Certificate # field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //ID Type
                        switch (dt.Rows[i][3].ToString().Trim())
                        {
                            case "I.D Card":
                            case "Passport":
                            case "Visa":
                            case "Birth Certificate":
                            case "Police / Civil Service Card":
                            case "Employment Book":
                            case "Residential Book":
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for ID Type field then try again. Row number: '" + (i + 1) + "";
                                return;
                        }

                        //Last Name English
                        if (dt.Rows[i][7].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Last Name field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //First Name English
                        if (dt.Rows[i][6].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for First Name field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //Gender
                        if (dt.Rows[i][9].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Gender field then try again. Row number: '" + (i + 1) + "";
                            return;

                        }
                        else
                        {
                            if (!dt.Rows[i][9].ToString().Trim().Equals("M") && !dt.Rows[i][9].ToString().Trim().Equals("F"))
                            {
                                lblMessage.Text = "Please check your input for Gender field then try again. Row number: '" + (i + 1) + "";
                                return;
                            }
                        }

                        //Date of Birth
                        DateTime value;
                        if (!DateTime.TryParse(Convert.ToDateTime(dt.Rows[i][8], dtfi).ToString(), out value))
                        {
                            lblMessage.Text = "Please check your input for Date of Birth field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //Bneficiary
                        if (dt.Rows[i][10].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //Relation 
                        if (dt.Rows[i][11].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Relation field then try again. Row number: '" + (i + 1) + "";
                            return;
                        }

                        //relation check case ??????????
                        switch (dt.Rows[i][11].ToString().Trim().ToUpper())
                        {
                            case "M":
                            case "F":
                            case "G":

                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for Relation field then try again. Row number: '" + (i + 1) + ")";
                                return;
                        }
                    }

                    int rows_saved = 0;

                    //loop excel file rows to save policy
                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {

                        string first_name = dt.Rows[k][6].ToString().Trim();
                        string last_name = dt.Rows[k][7].ToString().Trim();
                        string str_genter = dt.Rows[k][9].ToString().Trim();

                        int gender = 0;

                        if (str_genter == "M")
                        {
                            gender = 1;
                        }

                        int id_type = 0;

                        //ID Type
                        switch (dt.Rows[k][3].ToString().Trim())
                        {
                            case "I.D Card":
                                id_type = 0;
                                break;
                            case "Passport":
                                id_type = 1;
                                break;
                            case "Visa":
                                id_type = 2;
                                break;
                            case "Birth Certificate":
                                id_type = 2;
                                break;
                            case "Police / Civil Service Card":
                                id_type = 4;
                                break;
                            case "Employment Book":
                                id_type = 5;
                                break;
                            case "Residential Book":
                                id_type = 6;
                                break;
                        }

                        DateTime birth_date = Convert.ToDateTime(dt.Rows[k][8].ToString().Trim(), dtfi);

                        //get next available barcode with order by card_number ASC and status == 1 and not in ct_policy_micro_banc_card
                        string customer_micro_id = "";
                        string customer_id = "";

                        //check existing micro customer
                        if (!da_policy_micro.CheckExistingMicroCustomer(first_name, last_name, dt.Rows[k][4].ToString().Trim(), dt.Rows[k][5].ToString().Trim(), gender, birth_date))
                        {

                            //Add new micro customer
                            bl_micro_customer micro_customer = new bl_micro_customer();
                            micro_customer.Birth_Date = birth_date;
                            micro_customer.First_Name = first_name;
                            micro_customer.Created_By = hdfusername.Value;
                            micro_customer.Created_Note = "";
                            micro_customer.Created_On = DateTime.Now;
                            micro_customer.Gender = gender;
                            micro_customer.ID_Card = dt.Rows[k][2].ToString().Trim();
                            micro_customer.ID_Type = id_type;
                            micro_customer.Khmer_First_Name = dt.Rows[k][4].ToString().Trim();
                            micro_customer.Khmer_Last_Name = dt.Rows[k][5].ToString().Trim();
                            micro_customer.Last_Name = last_name;

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
                            customer_micro_id = da_policy_micro.GetExistingMicroCustomerID(first_name, last_name, dt.Rows[k][4].ToString().Trim(), dt.Rows[k][5].ToString().Trim(), gender, birth_date);

                        }


                        //successful then save new policy for this row
                        if (customer_micro_id != "")
                        {

                            //Policy Micro
                            bl_policy_micro policy_micro = new bl_policy_micro();

                            policy_micro.Age_Insure = Calculation.Culculate_Customer_Age_Micro(birth_date, System.DateTime.Now.AddHours(24));
                            policy_micro.Agreement_Date = Convert.ToDateTime(hdfEntryDate.Value, dtfi3);
                            policy_micro.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                            policy_micro.Assure_Up_To_Age = policy_micro.Age_Insure + policy_micro.Assure_Year;
                            policy_micro.Channel_Channel_Item_ID = hdfChannelChannelItem.Value;
                            policy_micro.Channel_Location_ID = hdfChannelLocation.Value;
                            policy_micro.Created_By = hdfusername.Value;
                            policy_micro.Created_Note = "";
                            policy_micro.Created_On = DateTime.Now;
                            policy_micro.Customer_ID = customer_micro_id;
                            policy_micro.Effective_Date = Convert.ToDateTime(hdfEntryDate.Value, dtfi3).AddHours(24);

                            policy_micro.Issue_Date = DateTime.Now;
                            policy_micro.Maturity_Date = policy_micro.Effective_Date.AddYears(policy_micro.Assure_Year);

                            policy_micro.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                            //case term one product payment is single
                            policy_micro.Pay_Up_To_Age = 0;

                            policy_micro.Product_ID = hdfProduct.Value;


                            string policy_micro_id = da_policy_micro.InsertPolicyMicro(policy_micro);

                            if (policy_micro_id != "")
                            {
                                //successfull
                                if (!da_policy_micro.InsertPolicyID(policy_micro_id, 3)) //3 = type micro
                                {
                                    da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);

                                    ShowResult(rows_saved, dt.Rows.Count, dt);
                                    return;
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

                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                        ShowResult(rows_saved, dt.Rows.Count, dt);
                                        return;

                                    }
                                    else
                                    {

                                        //successfull
                                        //Policy Micro and micro Card
                                        bl_policy_micro_banc_card policy_card = new bl_policy_micro_banc_card();

                                        bl_banc_card available_card = da_banc_card.GetFirstAvailableCard(hdfProduct.Value);

                                        policy_card.Card_ID = available_card.Card_ID; //get available card
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


                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                            ShowResult(rows_saved, dt.Rows.Count, dt);
                                            return;

                                        }
                                        else
                                        {
                                            //successfull
                                            //Info Person
                                            bl_policy_micro_info_person person = new bl_policy_micro_info_person();
                                            person.Birth_Date = birth_date;
                                            person.First_Name = first_name;
                                            person.Gender = gender;
                                            person.ID_Card = dt.Rows[k][2].ToString().Trim();
                                            person.ID_Type = id_type;
                                            person.Khmer_First_Name = dt.Rows[k][4].ToString().Trim();
                                            person.Khmer_Last_Name = dt.Rows[k][5].ToString().Trim();
                                            person.Last_Name = last_name;

                                            person.Policy_Micro_ID = policy_micro_id;

                                            if (!da_policy_micro.InsertPolicyMicroInfoPerson(person))
                                            {
                                                //failed                           
                                                da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                da_policy_micro.DeletePolicyID(policy_micro_id);
                                                da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                ShowResult(rows_saved, dt.Rows.Count, dt);
                                                return;
                                            }
                                            else
                                            {
                                                //successful
                                                //Info Address
                                                bl_policy_micro_info_address address = new bl_policy_micro_info_address();
                                                address.Address1 = "";
                                                address.Address2 = "";
                                                address.Address3 = "";
                                                address.Country_ID = "KH";
                                                address.Policy_Micro_ID = policy_micro_id;
                                                address.Province = "";
                                                address.Zip_Code = "855";

                                                if (!da_policy_micro.InsertPolicyMicroInfoAddress(address))
                                                {
                                                    //failed                             
                                                    da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                    da_policy_micro.DeletePolicyID(policy_micro_id);
                                                    da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                    da_policy_micro.DeletePolicyMicro(policy_micro_id);

                                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                    ShowResult(rows_saved, dt.Rows.Count, dt);
                                                    return;
                                                }
                                                else
                                                {
                                                    //successfull
                                                    //Insurance Plan
                                                    bl_policy_micro_life_product product = new bl_policy_micro_life_product();

                                                    product.Age_Insure = Calculation.Culculate_Customer_Age_Micro(birth_date, System.DateTime.Now.AddHours(24));

                                                    product.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                                                    product.Assure_Up_To_Age = product.Age_Insure + product.Assure_Year;

                                                    product.Pay_Mode = 0; //Single Payment

                                                    product.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                                                    product.Pay_Up_To_Age = product.Age_Insure;

                                                    product.Policy_Micro_ID = policy_micro_id;
                                                    product.Product_ID = hdfProduct.Value;
                                                    product.System_Premium = 0;
                                                    product.System_Premium_Discount = 0;
                                                    product.System_Sum_Insure = 0;
                                                    product.User_Premium = Convert.ToDouble(hdfPremiumAmount.Value);
                                                    product.User_Sum_Insure = Convert.ToDouble(hdfSumInsured.Value);

                                                    if (!da_policy_micro.InsertPolicyMicroLifeProduct(product))
                                                    {
                                                        //failed 
                                                        da_policy_micro.DeletePolicyMicroInfoAddress(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroInfoPerson(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicroCard(policy_micro_id);
                                                        da_policy_micro.DeletePolicyID(policy_micro_id);
                                                        da_policy_micro.DeletePolicyNumber(policy_micro_id);
                                                        da_policy_micro.DeletePolicyMicro(policy_micro_id);


                                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                        ShowResult(rows_saved, dt.Rows.Count, dt);
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        //successfull
                                                        //Beneficiary
                                                        bl_policy_micro_benefit_item benefit_item = new bl_policy_micro_benefit_item();
                                                        benefit_item.Seq_Number = 1;
                                                        benefit_item.Address = "";
                                                        benefit_item.Birth_Date = Convert.ToDateTime("01/01/1900", dtfi2); //no benefitciary date of birth supplied
                                                        benefit_item.Full_Name = dt.Rows[k][10].ToString().Trim();
                                                        benefit_item.Percentage = 100;
                                                        benefit_item.Policy_Micro_ID = policy_micro_id;

                                                        if (dt.Rows[k][11].ToString().Trim().ToUpper() == "F")
                                                        {
                                                            benefit_item.Relationship = "FATHER";
                                                        }
                                                        else if (dt.Rows[k][11].ToString().Trim().ToUpper() == "M")
                                                        {
                                                            benefit_item.Relationship = "MOTHER";
                                                        }
                                                        else if (dt.Rows[k][11].ToString().Trim().ToUpper() == "G")
                                                        {
                                                            benefit_item.Relationship = "GUIDANCE";
                                                        }

                                                        benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);

                                                        benefit_item.Policy_Micro_Benefit_Item_ID = Helper.GetNewGuid("SP_Check_Policy_Micro_Benefit_Item_ID", "@Policy_Micro_Benefit_Item_ID").ToString();

                                                        if (!da_policy_micro.InsertPolicyMicroBenefitItem(benefit_item))
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

                                                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                            ShowResult(rows_saved, dt.Rows.Count, dt);
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            //successfull
                                                            bl_policy_micro_info_contact contact = new bl_policy_micro_info_contact();
                                                            contact.EMail = "";
                                                            contact.Fax1 = "";
                                                            contact.Fax2 = "";
                                                            contact.Home_Phone1 = "";
                                                            contact.Home_Phone2 = "";
                                                            contact.Mobile_Phone1 = "";
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

                                                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                                ShowResult(rows_saved, dt.Rows.Count, dt);
                                                                return;
                                                            }
                                                            else
                                                            {
                                                                bl_policy_micro_premium policy_premium = new bl_policy_micro_premium();

                                                                policy_premium.Created_By = hdfusername.Value;
                                                                policy_premium.Created_Note = "";
                                                                policy_premium.Created_On = DateTime.Now;
                                                                policy_premium.Original_Amount = Convert.ToDouble(hdfPremiumAmount.Value);
                                                                policy_premium.Premium = Convert.ToDouble(hdfPremiumAmount.Value);
                                                                policy_premium.Sum_Insure = Convert.ToDouble(hdfSumInsured.Value);
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


                                                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                                    ShowResult(rows_saved, dt.Rows.Count, dt);
                                                                    return;
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


                                                                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                                                                        ShowResult(rows_saved, dt.Rows.Count, dt);
                                                                        return;
                                                                    }
                                                                    else
                                                                    {
                                                                        //successfull                                                    
                                                                        bl_policy_micro_prem_pay micro_prem_pay = new bl_policy_micro_prem_pay();
                                                                        micro_prem_pay.Amount = Convert.ToDouble(hdfPremiumAmount.Value);
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
                                                                            rows_saved = k + 1;

                                                                            //Policy Pay Mode
                                                                            da_policy.InsertPolicyPayMode("", policy_micro.Policy_Micro_ID, 0, policy_micro.Maturity_Date, hdfusername.Value, policy_micro.Created_On);

                                                                            //change card status
                                                                            da_banc_card.UpdateCardStatus(policy_card.Card_ID, 0);

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
                        }
                        else
                        {
                            //failed insert policy micro
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy failed. Please try again'" + k + 1 + ")", true);
                            ShowResult(rows_saved, dt.Rows.Count, dt);
                            return;
                        }

                    }//end loop

                    //if reach here all save successfully
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy successfull.')", true);


                    ShowResult(rows_saved, dt.Rows.Count, dt);
                    return;  
             
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please upload an excel file that contains policy micro data.')", true);
                Clear();
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please contact system admin for problem diagnosis.')", true);
            Clear();
            Log.AddExceptionToLog("Error in function [ImgBtnUpload_Click], page [upload_policies]. Details: " + ex.Message);
        }

    }

    private void ShowResult(int rows_saved, int total_rows, DataTable mydt)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";
               
        
        for (int i = 0; i < total_rows; i++)
        {
           
            TableRow row = new TableRow();

            TableCell cell1 = new TableCell();
            cell1.Style.Add("text-align", "center");
            cell1.Text = (i+1).ToString();

            TableCell cell2 = new TableCell();
            cell2.Style.Add("text-align", "left");
            cell2.Style.Add("padding-left", "5px");
            cell2.Text = mydt.Rows[i][4].ToString().Trim();
            
            TableCell cell3 = new TableCell();
            cell3.Style.Add("text-align", "left");
            cell3.Style.Add("padding-left", "5px");
            cell3.Text = mydt.Rows[i][5].ToString().Trim();

            TableCell cell4 = new TableCell();
            cell4.Style.Add("text-align", "left");
            cell4.Style.Add("padding-left", "5px");
            cell4.Text = mydt.Rows[i][6].ToString().Trim();

            TableCell cell5 = new TableCell();
            cell5.Style.Add("text-align", "left");
            cell5.Style.Add("padding-left", "5px");
            cell5.Text = mydt.Rows[i][7].ToString().Trim();

            TableCell cell6 = new TableCell();
            cell6.Style.Add("text-align", "center");           
            cell6.Text = DateTime.Parse(mydt.Rows[i][8].ToString()).ToString("dd-MM-yyyy");

            TableCell cell7 = new TableCell();
            cell7.Style.Add("text-align", "center");          
            cell7.Text = mydt.Rows[i][9].ToString().Trim();

            TableCell cell8= new TableCell();                

            if (rows_saved == 0) //no row saved
            {
                  cell8.Style.Add("color", "red");
                  cell8.Style.Add("text-align", "center");
                  cell8.Text = "X";
            }
            else
            {
                if (i <= rows_saved)//this row saved
                {
                    cell8.Style.Add("color", "green");
                    cell8.Style.Add("text-align", "center");
                    cell8.Text = "\u221A";
                   
                }
                else
                {
                    //this row is not saved
                    cell8.Style.Add("color", "red");
                    cell8.Style.Add("text-align", "center");
                    cell8.Text = "X";
                }
            }

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
            row.Cells.Add(cell4);
            row.Cells.Add(cell5);
            row.Cells.Add(cell6);
            row.Cells.Add(cell7);
            row.Cells.Add(cell8);

            tblResult.Rows.Add(row);
                      

        }

       
    }

    //Clear All
    protected void ImgBtnClear_Click(object sender, ImageClickEventArgs e)
    {
        Clear();
    }

    protected void Clear()
    {
        txtAgentName.Text = "";
        txtMarketingCode.Text = "";
        txtMarketingName.Text = "";
        txtPremiumAmount.Text = "";
        txtSumInsured.Text = "";
        ddlCard.SelectedIndex = 0;
        ddlCompanyMicro.SelectedIndex = 0;

    }
   
}