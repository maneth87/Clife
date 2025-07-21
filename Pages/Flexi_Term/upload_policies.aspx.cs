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


public partial class Pages_Flexi_Term_upload_policies : System.Web.UI.Page
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

    //Upload Flexi Term Policies using Excel file
    protected void ImgBtnUpload_Click(object sender, ImageClickEventArgs e)
    {
        bl_banc_card available_card_check = da_banc_card.GetFirstAvailableCard(hdfProduct.Value);
        int available_cards = da_banc_card.GetAvailableCardCount(hdfProduct.Value);
        if (available_card_check.Card_ID == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('No available flexi term banc card.')", true);
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
        
        //get channel, channel item and channel location
        string channel_item_id = ddlBank.SelectedValue;
        string channel_channel_item_id = da_channel.GetChannelChannelItemIDByChannelSubIDAndChannelItemID(3, channel_item_id);
        
        hdfChannelChannelItem.Value = channel_channel_item_id;
        hdfChannelItem.Value = channel_item_id;
        
        try
        {
            //check if file upload contain any file                      
            if ((FileUploadFlexiTermPolicy.PostedFile != null) && !string.IsNullOrEmpty(FileUploadFlexiTermPolicy.PostedFile.FileName))
            {

                string save_path = "~/Upload/FlexiTerm/";
                string file_name = Path.GetFileName(FileUploadFlexiTermPolicy.PostedFile.FileName);
                string content_type = FileUploadFlexiTermPolicy.PostedFile.ContentType;
                int content_length = FileUploadFlexiTermPolicy.PostedFile.ContentLength;

                FileUploadFlexiTermPolicy.PostedFile.SaveAs(Server.MapPath(save_path + file_name));
                string version = Path.GetExtension(file_name);

                string file_path = Server.MapPath(save_path + file_name).ToString();

                //verify if the file has been save           
                if (version == ".xls")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + file_path + "';Extended Properties=Excel 8.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Worksheet$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[Worksheet$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    if (available_cards < dt.Rows.Count)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('available flexi term cards is less than required cards.')", true);
                        Clear();
                        return;
                    }

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //Branch
                        if (dt.Rows[i][0].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input of Branch field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Bank Account Number
                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Bank Account Number field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Surname
                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Surname field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //First Name
                        if (dt.Rows[i][3].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for First Name field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Date of Birth
                        DateTime value;
                        if (!DateTime.TryParse(Convert.ToDateTime(dt.Rows[i][4], dtfi).ToString(), out value))
                        {
                            lblMessage.Text = "Please check your input for Date of Birth field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Gender
                        if (dt.Rows[i][5].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for First Name field then try again. Row number: '" + (i + 2) + "";
                            return;

                        }
                        else
                        {
                            if (!dt.Rows[i][5].ToString().Trim().Equals("M") && !dt.Rows[i][5].ToString().Trim().Equals("F"))
                            {
                                lblMessage.Text = "Please check your input for Gender field then try again. Row number: '" + (i + 2) + "";
                                return;
                            }
                        }
                        
                        //ID Number 
                        if (dt.Rows[i][6].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for ID Number field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //ID Type 
                        if (dt.Rows[i][7].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for ID Type field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Case ID Type
                        switch (dt.Rows[i][7].ToString().Trim())
                        {
                            case "I.D Card":
                            case "Passport":
                            case "Visa":
                            case "Birth Certificate":
                            case "Police / Civil Service Card":
                            case "Employment Book":
                            case "Residential Book":
                            case "Family Book":
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for ID Type field then try again. Row number: '" + (i + 2) + "";
                                return;
                        }

                        //Resident 
                        if (dt.Rows[i][8].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Resident field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }
                        else
                        {
                            if (!dt.Rows[i][8].ToString().Trim().Equals("Y") && !dt.Rows[i][8].ToString().Trim().Equals("N"))
                            {
                                lblMessage.Text = "Please check your input for Residential field then try again. Row number: '" + (i + 2) + "";
                                return;
                            }
                        }

                        //Beneficiary Surname 
                        if (dt.Rows[i][9].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary Surname field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Beneficiary First Name 
                        if (dt.Rows[i][10].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary First Name field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Beneficiary ID Number 
                        if (dt.Rows[i][11].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary ID Number field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Beneficiary ID Type 
                        if (dt.Rows[i][12].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary ID Type field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Case Beneficiary ID Type
                        switch (dt.Rows[i][12].ToString().Trim())
                        {
                            case "I.D Card":
                            case "Passport":
                            case "Visa":
                            case "Birth Certificate":
                            case "Police / Civil Service Card":
                            case "Employment Book":
                            case "Residential Book": 
                            case "Family_Book":
                                break;
                            default:                              
                                //Wrong input
                                lblMessage.Text = "Please check your input for Beneficiary ID Type field then try again. Row number: '" + (i + 2) + "";
                                return;
                        }

                        //Beneficiary Relation 
                        if (dt.Rows[i][13].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Relationship field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //relation check case
                        switch (dt.Rows[i][13].ToString().Trim().ToUpper())
                        {
                            case "BROTHER":
                            case "BROTHER (YOUNGER)":
                            case "CREDITOR":
                            case "DAUGHTER":
                            case "FATHER":
                            case "GRANDAUGHTER":
                            case "GRANDMOTHER":
                            case "GUIDANCE":
                            case "HUSBAND":
                            case "MOTHER":
                            case "NEPHEW":
                            case "NIECE":
                            case "OTHERS":
                            case "SISTER":
                            case "SISTER (YOUNGER)":
                            case "SON":
                            case "WIFE":
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for Relationship field then try again. Row number: '" + (i + 2) + "";
                                return;
                        }

                    }

                    List<bl_flexi_term_upload_result> row_save = new List<bl_flexi_term_upload_result>();

                    //loop excel file rows to save policy
                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {
                        string card_id = "";

                        //Bank Number column
                        string bank_number = dt.Rows[k][1].ToString().Trim();

                        //Branch column
                        string branch = dt.Rows[k][0].ToString().Trim();

                        //Surname column
                        string surname = dt.Rows[k][2].ToString().Trim().ToUpper();

                        //First Name column
                        string first_name = dt.Rows[k][3].ToString().Trim().ToUpper();

                        //Gender column
                        string str_genter = dt.Rows[k][5].ToString().Trim().ToUpper();
                        int gender = 0;

                        if (str_genter == "M")
                        {
                            gender = 1;
                        }

                        //ID Type column
                        int id_type = 0;
                        switch (dt.Rows[k][7].ToString().Trim())
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
                                id_type = 3;
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
                            case "Family Book":
                                id_type = 7;
                                break;
                        }

                        //Date of Birth column
                        DateTime birth_date = Convert.ToDateTime(dt.Rows[k][4].ToString().Trim(), dtfi);

                        //ID Number column
                        string id_card = dt.Rows[k][6].ToString().Trim();

                        //Resident column
                        string str_resident = dt.Rows[k][8].ToString().Trim();
                        int resident = 0;

                        if (str_resident == "Y")
                        {
                            resident = 1;
                        }

                        //Beneficiary's Surname column
                        string beneficiary_surname = dt.Rows[k][9].ToString().Trim();

                        //Beneficiary's First Name column
                        string beneficiary_first_name = dt.Rows[k][10].ToString().Trim();

                        //Beneficiary's ID Card
                        string beneficiary_id_card = dt.Rows[k][11].ToString().Trim();

                        //Beneficiary's ID Type
                        string str_beneficiary_id_type = dt.Rows[k][12].ToString().Trim();

                        int beneficiary_id_type = 0;

                        switch (dt.Rows[k][12].ToString().Trim())
                        {
                            case "I.D Card":
                                beneficiary_id_type = 0;
                                break;
                            case "Passport":
                                beneficiary_id_type = 1;
                                break;
                            case "Visa":
                                beneficiary_id_type = 2;
                                break;
                            case "Birth Certificate":
                                beneficiary_id_type = 3;
                                break;
                            case "Police / Civil Service Card":
                                beneficiary_id_type = 4;
                                break;
                            case "Employment Book":
                                beneficiary_id_type = 5;
                                break;
                            case "Residential Book":
                                beneficiary_id_type = 6;
                                break;
                            case "Family Book":
                                id_type = 7;
                                break;
                        }

                        //Beneficiary's Relationship
                        string beneficiary_relationship = dt.Rows[k][13].ToString().Trim();

                        //Family Book
                        string str_family_book = dt.Rows[k][14].ToString().Trim();

                        int family_book = 0;

                        if (str_family_book == "Y")
                        {
                            family_book = 1;
                        }

                        bool is_save = true;
                        bool error = false;

                        bl_flexi_term_upload_result upload_result = new bl_flexi_term_upload_result();
                        upload_result.Branch = dt.Rows[k][0].ToString().Trim();
                        upload_result.Bank_Number = dt.Rows[k][1].ToString().Trim();
                        upload_result.Last_Name = dt.Rows[k][2].ToString().Trim();
                        upload_result.First_Name = dt.Rows[k][3].ToString().Trim();
                        upload_result.DOB = dt.Rows[k][4].ToString().Trim();
                        upload_result.Gender = dt.Rows[k][5].ToString().Trim();
                        upload_result.ID_Card = dt.Rows[k][6].ToString().Trim();
                        upload_result.ID_Type = dt.Rows[k][7].ToString().Trim();
                        upload_result.Application_Resident = dt.Rows[k][8].ToString().Trim();
                        upload_result.Beneficiary_Last_Name = dt.Rows[k][9].ToString().Trim();
                        upload_result.Beneficiary_First_Name = dt.Rows[k][10].ToString().Trim();

                        //Check bank number for this channel item in flexi term policy (if exist don't save)
                        if (da_flexi_term_policy.CheckExistingFlexiTermBankNumberByChannelItemID(bank_number, hdfChannelItem.Value))
                        {
                            //not saved (Reason: Bank Number already exist or saved in policy)
                            upload_result.Result = "0";
                            upload_result.Reason = "Policy already created";
                            row_save.Add(upload_result);

                            //Don't save this row
                            is_save = false;

                        }

                        //Check in Flexi Term Temp table (Primary Data)
                        if (is_save == true)
                        {
                            if (!da_flexi_term_policy.CheckFlexiTermDataInTemp(branch, bank_number, surname, first_name, birth_date, gender, id_card, id_type, resident, beneficiary_surname, beneficiary_first_name, beneficiary_id_card, beneficiary_id_type, beneficiary_relationship, family_book))
                            {
                                //not saved (Reason: upload row not exist in temp table 'Ct_Flexi_Term_Primary_Data')
                                upload_result.Result = "0";
                                upload_result.Reason = "Not exist in temp";
                                row_save.Add(upload_result);

                                //Don't save this row
                                is_save = false;
                            }

                            //Check Status in Temp table (Primary Data)
                            string status_code = da_flexi_term_policy.GetFlexiTermStatusInTemp(branch, bank_number, surname, first_name, birth_date, gender, id_card, id_type, resident, beneficiary_surname, beneficiary_first_name, beneficiary_id_card, beneficiary_id_type, beneficiary_relationship, family_book);

                            if (status_code != "Approved")
                            {
                                upload_result.Result = "0";
                                upload_result.Reason = "Not yet approved";
                                row_save.Add(upload_result);

                                //Don't save this row
                                is_save = false;
                            }
                        }

                        bl_flexi_term_primary_data flexi_term_primary_data = new bl_flexi_term_primary_data();

                        if (is_save == true)
                        {
                            //Get flexi term primary data                         
                            flexi_term_primary_data = da_flexi_term_policy.GetFlexiTermPrimaryDataByParams(branch, bank_number, surname, first_name, birth_date, gender);

                           

                            //Check age > 60 not save
                            int age = da_flexi_term_policy.GetAge(birth_date.Day + "/" + birth_date.Month + "/" + birth_date.Year, flexi_term_primary_data.Effective_Date.Day + "/" + flexi_term_primary_data.Effective_Date.Month + "/" + flexi_term_primary_data.Effective_Date.Year);

                            if (age > 60)
                            {
                                upload_result.Result = "0";
                                upload_result.Reason = "Customer Age > 60";
                                row_save.Add(upload_result);

                                //Don't save this row
                                is_save = false;
                            }

                        
                        }

                        //If is_save = true -> start save policy
                        if (is_save == true)
                        {

                            string customer_flexi_term_id = "";
                            string customer_id = "";

                            //check existing customer flexi term
                            if (!da_flexi_term_policy.CheckExistingCustomerFlexiTerm(first_name, surname, gender, birth_date))
                            {
                                //Add new customer flexi term
                                bl_customer_flexi_term customer_flexi_term = new bl_customer_flexi_term();
                                customer_flexi_term.Birth_Date = birth_date;
                                customer_flexi_term.First_Name = first_name;
                                customer_flexi_term.Created_By = hdfusername.Value;
                                customer_flexi_term.Created_Note = "";
                                customer_flexi_term.Created_On = DateTime.Now;
                                customer_flexi_term.Gender = gender;
                                customer_flexi_term.ID_Card = id_card;
                                customer_flexi_term.ID_Type = id_type;
                                customer_flexi_term.Khmer_First_Name = "";
                                customer_flexi_term.Khmer_Last_Name = "";
                                customer_flexi_term.Last_Name = surname;

                                customer_flexi_term_id = da_flexi_term_policy.InsertCustomerFlexiTerm(customer_flexi_term);

                                if (customer_flexi_term_id == "")
                                {
                                    //failed insert customer flexi term
                                    upload_result.Result = "0";
                                    upload_result.Reason = "Save Failed";
                                    row_save.Add(upload_result);

                                    error = true;
                                }
                                else
                                {
                                    //check customer in Ct_Customer
                                    if (da_customer.CheckExistingCustomer(first_name, surname, gender, birth_date))
                                    {
                                        //get existing customer id
                                        customer_id = da_customer.GetCustomerIDByNameDOBGender(first_name, surname, gender, birth_date);

                                        if (customer_id == "")
                                        {
                                            //failed get customer id
                                            upload_result.Result = "0";
                                            upload_result.Reason = "Save failed";

                                            error = true;
                                        }
                                        else
                                        {
                                            //Start Insert customer_flexi_term_customer
                                            bl_customer_flexi_term_customer customer_flexi_term_customer = new bl_customer_flexi_term_customer();
                                            customer_flexi_term_customer.Customer_ID = customer_id;
                                            customer_flexi_term_customer.Customer_Flexi_Term_ID = customer_flexi_term_id;

                                            if (!da_flexi_term_policy.InsertCustomerFlexiTermCustomer(customer_flexi_term_customer))
                                            {
                                                //failed insert customer_flexi_term_customer                                          
                                                upload_result.Result = "0";
                                                upload_result.Reason = "Save Failed";
                                                row_save.Add(upload_result);

                                                error = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Case exiting customer flexi term -> get flexi_term_customer_id
                                customer_flexi_term_id = da_flexi_term_policy.GetCustomerFlexiTermID(first_name, surname, gender, birth_date);

                                if (customer_flexi_term_id == "")
                                {
                                    //failed insert customer flexi term
                                    upload_result.Result = "0";
                                    upload_result.Reason = "Save Failed";
                                    row_save.Add(upload_result);

                                    error = true;
                                }
                            }

                            //Not save error continue
                            if (error == false)
                            {
                               
                                //Insert flexi term policy
                                bl_flexi_term_policy flexi_term_policy = new bl_flexi_term_policy();

                                flexi_term_policy.Age_Insure = flexi_term_primary_data.Age_Insured;
                                flexi_term_policy.Agreement_Date = flexi_term_primary_data.Agreement_Date;
                                flexi_term_policy.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                                flexi_term_policy.Assure_Up_To_Age = flexi_term_policy.Age_Insure + flexi_term_policy.Assure_Year;
                                flexi_term_policy.Channel_Channel_Item_ID = hdfChannelChannelItem.Value;
                                flexi_term_policy.Channel_Location_ID = da_channel.GetChannelLocationIDByLocationName(branch);
                                flexi_term_policy.Created_By = hdfusername.Value;
                                flexi_term_policy.Created_Note = "";
                                flexi_term_policy.Created_On = DateTime.Now;
                                flexi_term_policy.Customer_Flexi_Term_ID = customer_flexi_term_id;
                                flexi_term_policy.Effective_Date = flexi_term_primary_data.Effective_Date;

                                flexi_term_policy.Issue_Date = DateTime.Now;
                                flexi_term_policy.Maturity_Date = flexi_term_primary_data.Maturity_Date;

                                flexi_term_policy.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                                //case flexi term product payment is single
                                flexi_term_policy.Pay_Up_To_Age = 0;

                                flexi_term_policy.Product_ID = hdfProduct.Value;

                                string flexi_term_policy_id = da_flexi_term_policy.InsertFlexiTermPolicy(flexi_term_policy);

                                if (flexi_term_policy_id == "")
                                {
                                    //failed insert flexi term policy
                                    upload_result.Result = "0";
                                    upload_result.Reason = "Save Failed";
                                    row_save.Add(upload_result);

                                    error = true;
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert Policy ID
                                    if (!da_flexi_term_policy.InsertPolicyID(flexi_term_policy_id, "4")) //4 = type flexi term
                                    {
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);

                                        //failed insert policy id
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //New policy number from Ct_Policy_Number
                                    string last_policy_number = da_flexi_term_policy.GetLastPolicyNumberFlexiTerm();

                                    //Convert policy number to int and plus 1
                                    int number = Convert.ToInt32(last_policy_number) + 1;

                                    new_policy_number = number.ToString();

                                    //Concate 0 to the front
                                    while (new_policy_number.Length < 8)
                                    {
                                        new_policy_number = "0" + new_policy_number;
                                    }

                                    if (!da_flexi_term_policy.InsertPolicyNumber(flexi_term_policy_id, new_policy_number))
                                    {

                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert policy number
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;

                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {

                                    //Insert flexi term policy banc card
                                    bl_flexi_term_policy_banc_card flexi_term_policy_banc_card = new bl_flexi_term_policy_banc_card();

                                    bl_banc_card available_card = da_banc_card.GetFirstAvailableCard(hdfProduct.Value);

                                    card_id = available_card.Card_ID;

                                    flexi_term_policy_banc_card.Card_ID = available_card.Card_ID; //get available card
                                    flexi_term_policy_banc_card.Created_By = hdfusername.Value;
                                    flexi_term_policy_banc_card.Created_Note = "";
                                    flexi_term_policy_banc_card.Created_On = DateTime.Now;
                                    flexi_term_policy_banc_card.Flexi_Term_Policy_ID = flexi_term_policy_id;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyCard(flexi_term_policy_banc_card))
                                    {
                                        //failed                     
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term policy banc card
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;

                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term info person
                                    bl_flexi_term_policy_info_person flexi_term_policy_info_person = new bl_flexi_term_policy_info_person();
                                    flexi_term_policy_info_person.Birth_Date = birth_date;
                                    flexi_term_policy_info_person.First_Name = first_name;
                                    flexi_term_policy_info_person.Gender = gender;
                                    flexi_term_policy_info_person.ID_Card = id_card;
                                    flexi_term_policy_info_person.ID_Type = id_type;
                                    flexi_term_policy_info_person.Khmer_First_Name = "";
                                    flexi_term_policy_info_person.Khmer_Last_Name = "";
                                    flexi_term_policy_info_person.Last_Name = surname;
                                    flexi_term_policy_info_person.Resident = resident;
                                    flexi_term_policy_info_person.Branch = branch;
                                    flexi_term_policy_info_person.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    flexi_term_policy_info_person.Bank_Number = bank_number;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyInfoPerson(flexi_term_policy_info_person))
                                    {
                                        //failed                           
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term info person
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term insurance plan
                                    bl_flexi_term_policy_life_product flexi_term_policy_life_product = new bl_flexi_term_policy_life_product();

                                    flexi_term_policy_life_product.Age_Insure = flexi_term_primary_data.Age_Insured;

                                    flexi_term_policy_life_product.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                                    flexi_term_policy_life_product.Assure_Up_To_Age = flexi_term_policy_life_product.Age_Insure + flexi_term_policy_life_product.Assure_Year;

                                    flexi_term_policy_life_product.Pay_Mode = 0; //Single Payment

                                    flexi_term_policy_life_product.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                                    flexi_term_policy_life_product.Pay_Up_To_Age = 0;

                                    flexi_term_policy_life_product.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    flexi_term_policy_life_product.Product_ID = hdfProduct.Value;
                                    flexi_term_policy_life_product.System_Premium = 0;
                                    flexi_term_policy_life_product.System_Premium_Discount = 0;
                                    flexi_term_policy_life_product.System_Sum_Insure = 0;
                                    flexi_term_policy_life_product.User_Premium = flexi_term_primary_data.Premium;
                                    flexi_term_policy_life_product.User_Sum_Insure = flexi_term_primary_data.Sum_Insured;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyLifeProduct(flexi_term_policy_life_product))
                                    {
                                        //failed insert flexi term policy life product                                      
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term life product
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;

                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term beneficiary item
                                    bl_flexi_term_policy_benefit_item benefit_item = new bl_flexi_term_policy_benefit_item();
                                    benefit_item.Seq_Number = 1;
                                    benefit_item.Address = "";
                                    benefit_item.Birth_Date = Convert.ToDateTime("01/01/1900", dtfi2); //no benefitciary date of birth supplied
                                    benefit_item.Last_Name = beneficiary_surname.Trim().ToUpper();
                                    benefit_item.First_Name = beneficiary_first_name.Trim().ToUpper();
                                    benefit_item.Percentage = 100;
                                    benefit_item.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    benefit_item.Relationship = flexi_term_primary_data.Beneficiary_Relationship.Trim().ToUpper();
                                    benefit_item.Family_Book = flexi_term_primary_data.Family_Book;
                                    benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);
                                    benefit_item.Flexi_Term_Policy_Benefit_Item_ID = Helper.GetNewGuid("SP_Check_Flexi_Term_Policy_Benefit_Item_ID", "@Flexi_Term_Policy_Benefit_Item_ID").ToString();
                                    benefit_item.ID_Card = beneficiary_id_card;
                                    benefit_item.ID_Type = beneficiary_id_type;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyBenefitItem(benefit_item))
                                    {
                                        //failed insert flexi term policy benefit item
                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term policy benefit item
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term policy premium
                                    bl_flexi_term_policy_premium policy_premium = new bl_flexi_term_policy_premium();

                                    policy_premium.Created_By = hdfusername.Value;
                                    policy_premium.Created_Note = "";
                                    policy_premium.Created_On = DateTime.Now;
                                    policy_premium.Original_Amount = flexi_term_primary_data.Premium;
                                    policy_premium.Premium = flexi_term_primary_data.Premium;
                                    policy_premium.Sum_Insure = flexi_term_primary_data.Sum_Insured;
                                    policy_premium.Flexi_Term_Policy_ID = flexi_term_policy_id;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyPremium(policy_premium))
                                    {
                                        //failed
                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);


                                        //failed insert flexi term policy premium
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term policy status
                                    bl_flexi_term_policy_status policy_status = new bl_flexi_term_policy_status();

                                    policy_status.Created_By = hdfusername.Value;
                                    policy_status.Created_Note = "";
                                    policy_status.Created_On = DateTime.Now;
                                    policy_status.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    policy_status.Policy_Status_Type_ID = "IF";

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyStatus(policy_status))
                                    {
                                        //failed flexi term policy status
                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyPremium(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);


                                        //failed insert flexi term policy status
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {

                                    //Insert flexi term policy prem pay
                                    bl_flexi_term_policy_prem_pay policy_prem_pay = new bl_flexi_term_policy_prem_pay();

                                    policy_prem_pay.Amount = flexi_term_primary_data.Premium;
                                    policy_prem_pay.Channel_Location_ID = da_channel.GetChannelLocationIDByLocationName(branch);
                                    policy_prem_pay.Created_By = hdfusername.Value;
                                    policy_prem_pay.Created_Note = "";
                                    policy_prem_pay.Created_On = DateTime.Now;
                                    policy_prem_pay.Due_Date = flexi_term_primary_data.Maturity_Date;
                                    policy_prem_pay.Pay_Date = flexi_term_primary_data.Effective_Date;
                                    policy_prem_pay.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    policy_prem_pay.Prem_Lot = 1;
                                    policy_prem_pay.Prem_Year = 1;
                                    policy_prem_pay.Sale_Agent_ID = "";
                                    policy_prem_pay.Flexi_Term_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_Flexi_Term_Policy_Prem_Pay_ID", "@Flexi_Term_Policy_Prem_Pay_ID");

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyPremPay(policy_prem_pay))
                                    {

                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyPremium(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyStatus(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term policy prem pay
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                    else
                                    {
                                        //Policy Pay Mode
                                        da_policy.InsertPolicyPayMode("", flexi_term_policy_id, 0, flexi_term_primary_data.Maturity_Date, hdfusername.Value, DateTime.Now);

                                        //change card status
                                        da_banc_card.UpdateCardStatus(card_id, 0);

                                        upload_result.Result = "1";
                                        upload_result.Reason = "Save successfull";
                                        row_save.Add(upload_result);
                                    }
                                }
                            }

                        }


                    }//end loop

                    ////if reach here all save successfully
                    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy successfull.')", true);

                    ShowResult(row_save);

                    Clear();
                
                }
                else if (version == ".xlsx")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + file_path + "';Extended Properties=Excel 12.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Worksheet$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[Worksheet$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    if (available_cards < dt.Rows.Count)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('available flexi term cards is less than required cards.')", true);
                        Clear();
                        return;
                    }

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //Branch
                        if (dt.Rows[i][0].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input of Branch field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Bank Account Number
                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Bank Account Number field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Surname
                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Surname field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //First Name
                        if (dt.Rows[i][3].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for First Name field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Date of Birth
                        DateTime value;
                        if (!DateTime.TryParse(Convert.ToDateTime(dt.Rows[i][4], dtfi).ToString(), out value))
                        {
                            lblMessage.Text = "Please check your input for Date of Birth field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Gender
                        if (dt.Rows[i][5].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for First Name field then try again. Row number: '" + (i + 2) + "";
                            return;

                        }
                        else
                        {
                            if (!dt.Rows[i][5].ToString().Trim().Equals("M") && !dt.Rows[i][5].ToString().Trim().Equals("F"))
                            {
                                lblMessage.Text = "Please check your input for Gender field then try again. Row number: '" + (i + 2) + "";
                                return;
                            }
                        }

                        //ID Number 
                        if (dt.Rows[i][6].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for ID Number field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //ID Type 
                        if (dt.Rows[i][7].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for ID Type field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Case ID Type                      
                        switch (dt.Rows[i][7].ToString().Trim())
                        {
                            case "I.D Card":
                            case "Passport":
                            case "Visa":
                            case "Birth Certificate":
                            case "Police / Civil Service Card":
                            case "Employment Book":
                            case "Residential Book":
                            case "Family Book":
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for ID Type field then try again. Row number: '" + (i + 2) + "";
                                return;
                        }

                        //Resident 
                        if (dt.Rows[i][8].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Resident field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }
                        else
                        {
                            if (!dt.Rows[i][8].ToString().Trim().Equals("Y") && !dt.Rows[i][8].ToString().Trim().Equals("N"))
                            {
                                lblMessage.Text = "Please check your input for Residential field then try again. Row number: '" + (i + 2) + "";
                                return;
                            }
                        }

                        //Beneficiary Surname 
                        if (dt.Rows[i][9].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary Surname field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Beneficiary First Name 
                        if (dt.Rows[i][10].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary First Name field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Beneficiary ID Number 
                        if (dt.Rows[i][11].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary ID Number field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Beneficiary ID Type 
                        if (dt.Rows[i][12].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Beneficiary ID Type field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //Case Beneficiary ID Type
                        switch (dt.Rows[i][12].ToString().Trim())
                        {
                            case "I.D Card":
                            case "Passport":
                            case "Visa":
                            case "Birth Certificate":
                            case "Police / Civil Service Card":
                            case "Employment Book":
                            case "Residential Book":
                            case "Family Book":
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for Beneficiary ID Type field then try again. Row number: '" + (i + 2) + "";
                                return;
                        }

                        //Beneficiary Relation 
                        if (dt.Rows[i][13].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Relationship field then try again. Row number: '" + (i + 2) + "";
                            return;
                        }

                        //relation check case
                        switch (dt.Rows[i][13].ToString().Trim().ToUpper())
                        {
                            case "BROTHER":
                            case "BROTHER (YOUNGER)":
                            case "CREDITOR":
                            case "DAUGHTER":
                            case "FATHER":
                            case "GRANDAUGHTER":
                            case "GRANDMOTHER":
                            case "GUIDANCE":
                            case "HUSBAND":
                            case "MOTHER":
                            case "NEPHEW":
                            case "NIECE":
                            case "OTHERS":
                            case "SISTER":
                            case "SISTER (YOUNGER)":
                            case "SON":
                            case "WIFE":
                                break;
                            default:
                                //Wrong input
                                lblMessage.Text = "Please check your input for Relationship field then try again. Row number: '" + (i + 2) + "";
                                return;
                        }

                    }

                    List<bl_flexi_term_upload_result> row_save = new List<bl_flexi_term_upload_result>();

                    //loop excel file rows to save policy
                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {
                        string card_id = "";

                        //Bank Number column
                        string bank_number = dt.Rows[k][1].ToString().Trim();

                        //Branch column
                        string branch = dt.Rows[k][0].ToString().Trim();

                        //Surname column
                        string surname = dt.Rows[k][2].ToString().Trim().ToUpper();

                        //First Name column
                        string first_name = dt.Rows[k][3].ToString().Trim().ToUpper();

                        //Gender column
                        string str_genter = dt.Rows[k][5].ToString().Trim().ToUpper();
                        int gender = 0;

                        if (str_genter == "M")
                        {
                            gender = 1;
                        }

                        //ID Type column
                        int id_type = 0;
                        switch (dt.Rows[k][7].ToString().Trim())
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
                                id_type = 3;
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
                            case "Family Book":
                                id_type = 7;
                                break;
                        }

                        //Date of Birth column
                        DateTime birth_date = Convert.ToDateTime(dt.Rows[k][4].ToString().Trim(), dtfi);

                        //ID Number column
                        string id_card = dt.Rows[k][6].ToString().Trim();

                        //Resident column
                        string str_resident = dt.Rows[k][8].ToString().Trim();
                        int resident = 0;

                        if (str_resident == "Y")
                        {
                            resident = 1;
                        }

                        //Beneficiary's Surname column
                        string beneficiary_surname = dt.Rows[k][9].ToString().Trim();

                        //Beneficiary's First Name column
                        string beneficiary_first_name = dt.Rows[k][10].ToString().Trim();

                        //Beneficiary's ID Card
                        string beneficiary_id_card = dt.Rows[k][11].ToString().Trim();

                        //Beneficiary's ID Type
                        string str_beneficiary_id_type = dt.Rows[k][12].ToString().Trim();

                        int beneficiary_id_type = 0;

                        switch (dt.Rows[k][12].ToString().Trim())
                        {
                            case "I.D Card":
                                beneficiary_id_type = 0;
                                break;
                            case "Passport":
                                beneficiary_id_type = 1;
                                break;
                            case "Visa":
                                beneficiary_id_type = 2;
                                break;
                            case "Birth Certificate":
                                beneficiary_id_type = 3;
                                break;
                            case "Police / Civil Service Card":
                                beneficiary_id_type = 4;
                                break;
                            case "Employment Book":
                                beneficiary_id_type = 5;
                                break;
                            case "Residential Book":
                                beneficiary_id_type = 6;
                                break;
                            case "Family Book":
                                id_type = 7;
                                break;
                        }

                        //Beneficiary's Relationship
                        string beneficiary_relationship = dt.Rows[k][13].ToString().Trim();

                        //Family Book
                        string str_family_book = dt.Rows[k][14].ToString().Trim();

                        int family_book = 0;

                        if (str_family_book == "Y")
                        {
                            family_book = 1;
                        }

                        bool is_save = true;
                        bool error = false;

                        bl_flexi_term_upload_result upload_result = new bl_flexi_term_upload_result();
                        upload_result.Branch = dt.Rows[k][0].ToString().Trim();
                        upload_result.Bank_Number = dt.Rows[k][1].ToString().Trim();
                        upload_result.Last_Name = dt.Rows[k][2].ToString().Trim();
                        upload_result.First_Name = dt.Rows[k][3].ToString().Trim();
                        upload_result.DOB = dt.Rows[k][4].ToString().Trim();
                        upload_result.Gender = dt.Rows[k][5].ToString().Trim();
                        upload_result.ID_Card = dt.Rows[k][6].ToString().Trim();
                        upload_result.ID_Type = dt.Rows[k][7].ToString().Trim();
                        upload_result.Application_Resident = dt.Rows[k][8].ToString().Trim();
                        upload_result.Beneficiary_Last_Name = dt.Rows[k][9].ToString().Trim();
                        upload_result.Beneficiary_First_Name = dt.Rows[k][10].ToString().Trim();

                        //Check bank number for this channel item in flexi term policy (if exist don't save)
                        if (da_flexi_term_policy.CheckExistingFlexiTermBankNumberByChannelItemID(bank_number, hdfChannelItem.Value))
                        {
                            //not saved (Reason: Bank Number already exist or saved in policy)
                            upload_result.Result = "0";
                            upload_result.Reason = "Policy already created";
                            row_save.Add(upload_result);

                            //Don't save this row
                            is_save = false;

                        }
                          
                        //Check in Flexi Term Temp table (Primary Data)
                        if (is_save == true)
                        {
                            if (!da_flexi_term_policy.CheckFlexiTermDataInTemp(branch, bank_number, surname, first_name, birth_date, gender, id_card, id_type, resident, beneficiary_surname, beneficiary_first_name, beneficiary_id_card, beneficiary_id_type, beneficiary_relationship, family_book))
                            {
                                //not saved (Reason: upload row not exist in temp table 'Ct_Flexi_Term_Primary_Data')
                                upload_result.Result = "0";
                                upload_result.Reason = "Not exist in temp";
                                row_save.Add(upload_result);

                                //Don't save this row
                                is_save = false;
                            }

                            //Check Status in Temp table (Primary Data)
                            string status_code = da_flexi_term_policy.GetFlexiTermStatusInTemp(branch, bank_number, surname, first_name, birth_date, gender, id_card, id_type, resident, beneficiary_surname, beneficiary_first_name, beneficiary_id_card, beneficiary_id_type, beneficiary_relationship, family_book);

                            if (status_code != "Approved")
                            {
                                upload_result.Result = "0";
                                upload_result.Reason = "Not yet approved";
                                row_save.Add(upload_result);

                                //Don't save this row
                                is_save = false;
                            }
                        }

                        bl_flexi_term_primary_data flexi_term_primary_data = new bl_flexi_term_primary_data();

                        if (is_save == true)
                        {
                            //Get flexi term primary data                         
                            flexi_term_primary_data = da_flexi_term_policy.GetFlexiTermPrimaryDataByParams(branch, bank_number, surname, first_name, birth_date, gender);



                            //Check age > 60 not save
                            int age = da_flexi_term_policy.GetAge(birth_date.Day + "/" + birth_date.Month + "/" + birth_date.Year, flexi_term_primary_data.Effective_Date.Day + "/" + flexi_term_primary_data.Effective_Date.Month + "/" + flexi_term_primary_data.Effective_Date.Year);

                            if (age > 60)
                            {
                                upload_result.Result = "0";
                                upload_result.Reason = "Customer Age > 60";
                                row_save.Add(upload_result);

                                //Don't save this row
                                is_save = false;
                            }


                        }

                        //If is_save = true -> start save policy
                        if (is_save == true)
                        {

                            string customer_flexi_term_id = "";
                            string customer_id = "";

                            //check existing customer flexi term
                            if (!da_flexi_term_policy.CheckExistingCustomerFlexiTerm(first_name, surname, gender, birth_date))
                            {
                                //Add new customer flexi term
                                bl_customer_flexi_term customer_flexi_term = new bl_customer_flexi_term();
                                customer_flexi_term.Birth_Date = birth_date;
                                customer_flexi_term.First_Name = first_name;
                                customer_flexi_term.Created_By = hdfusername.Value;
                                customer_flexi_term.Created_Note = "";
                                customer_flexi_term.Created_On = DateTime.Now;
                                customer_flexi_term.Gender = gender;
                                customer_flexi_term.ID_Card = id_card;
                                customer_flexi_term.ID_Type = id_type;
                                customer_flexi_term.Khmer_First_Name = "";
                                customer_flexi_term.Khmer_Last_Name = "";
                                customer_flexi_term.Last_Name = surname;

                                customer_flexi_term_id = da_flexi_term_policy.InsertCustomerFlexiTerm(customer_flexi_term);

                                if (customer_flexi_term_id == "")
                                {
                                    //failed insert customer flexi term
                                    upload_result.Result = "0";
                                    upload_result.Reason = "Save Failed";
                                    row_save.Add(upload_result);

                                    error = true;
                                }
                                else
                                {
                                    //check customer in Ct_Customer
                                    if (da_customer.CheckExistingCustomer(first_name, surname, gender, birth_date))
                                    {
                                        //get existing customer id
                                        customer_id = da_customer.GetCustomerIDByNameDOBGender(first_name, surname, gender, birth_date);

                                        if (customer_id == "")
                                        {
                                            //failed get customer id
                                            upload_result.Result = "0";
                                            upload_result.Reason = "Save failed";

                                            error = true;
                                        }
                                        else
                                        {
                                            //Start Insert customer_flexi_term_customer
                                            bl_customer_flexi_term_customer customer_flexi_term_customer = new bl_customer_flexi_term_customer();
                                            customer_flexi_term_customer.Customer_ID = customer_id;
                                            customer_flexi_term_customer.Customer_Flexi_Term_ID = customer_flexi_term_id;

                                            if (!da_flexi_term_policy.InsertCustomerFlexiTermCustomer(customer_flexi_term_customer))
                                            {
                                                //failed insert customer_flexi_term_customer                                          
                                                upload_result.Result = "0";
                                                upload_result.Reason = "Save Failed";
                                                row_save.Add(upload_result);

                                                error = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Case exiting customer flexi term -> get flexi_term_customer_id
                                customer_flexi_term_id = da_flexi_term_policy.GetCustomerFlexiTermID(first_name, surname, gender, birth_date);

                                if (customer_flexi_term_id == "")
                                {
                                    //failed insert customer flexi term
                                    upload_result.Result = "0";
                                    upload_result.Reason = "Save Failed";
                                    row_save.Add(upload_result);

                                    error = true;
                                }
                            }

                            //Not save error continue
                            if (error == false)
                            {
                                
                                //Insert flexi term policy
                                bl_flexi_term_policy flexi_term_policy = new bl_flexi_term_policy();

                                flexi_term_policy.Age_Insure = flexi_term_primary_data.Age_Insured;
                                flexi_term_policy.Agreement_Date = flexi_term_primary_data.Agreement_Date;
                                flexi_term_policy.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                                flexi_term_policy.Assure_Up_To_Age = flexi_term_policy.Age_Insure + flexi_term_policy.Assure_Year;
                                flexi_term_policy.Channel_Channel_Item_ID = hdfChannelChannelItem.Value;
                                flexi_term_policy.Channel_Location_ID = da_channel.GetChannelLocationIDByLocationName(branch);
                                flexi_term_policy.Created_By = hdfusername.Value;
                                flexi_term_policy.Created_Note = "";
                                flexi_term_policy.Created_On = DateTime.Now;
                                flexi_term_policy.Customer_Flexi_Term_ID = customer_flexi_term_id;
                                flexi_term_policy.Effective_Date = flexi_term_primary_data.Effective_Date;

                                flexi_term_policy.Issue_Date = DateTime.Now;
                                flexi_term_policy.Maturity_Date = flexi_term_primary_data.Maturity_Date;

                                flexi_term_policy.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                                //case flexi term product payment is single
                                flexi_term_policy.Pay_Up_To_Age = 0;

                                flexi_term_policy.Product_ID = hdfProduct.Value;

                                string flexi_term_policy_id = da_flexi_term_policy.InsertFlexiTermPolicy(flexi_term_policy);

                                if (flexi_term_policy_id == "")
                                {
                                    //failed insert flexi term policy
                                    upload_result.Result = "0";
                                    upload_result.Reason = "Save Failed";
                                    row_save.Add(upload_result);

                                    error = true;
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert Policy ID
                                    if (!da_flexi_term_policy.InsertPolicyID(flexi_term_policy_id, "4")) //4 = type flexi term
                                    {
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);

                                        //failed insert policy id
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //New policy number from Ct_Policy_Number
                                    string last_policy_number = da_flexi_term_policy.GetLastPolicyNumberFlexiTerm();

                                    //Convert policy number to int and plus 1
                                    int number = Convert.ToInt32(last_policy_number) + 1;

                                    new_policy_number = number.ToString();

                                    //Concate 0 to the front
                                    while (new_policy_number.Length < 8)
                                    {
                                        new_policy_number = "0" + new_policy_number;
                                    }

                                    if (!da_flexi_term_policy.InsertPolicyNumber(flexi_term_policy_id, new_policy_number))
                                    {

                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert policy number
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;

                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {

                                    //Insert flexi term policy banc card
                                    bl_flexi_term_policy_banc_card flexi_term_policy_banc_card = new bl_flexi_term_policy_banc_card();

                                    bl_banc_card available_card = da_banc_card.GetFirstAvailableCard(hdfProduct.Value);

                                    card_id = available_card.Card_ID;

                                    flexi_term_policy_banc_card.Card_ID = available_card.Card_ID; //get available card
                                    flexi_term_policy_banc_card.Created_By = hdfusername.Value;
                                    flexi_term_policy_banc_card.Created_Note = "";
                                    flexi_term_policy_banc_card.Created_On = DateTime.Now;
                                    flexi_term_policy_banc_card.Flexi_Term_Policy_ID = flexi_term_policy_id;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyCard(flexi_term_policy_banc_card))
                                    {
                                        //failed                     
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term policy banc card
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;

                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term info person
                                    bl_flexi_term_policy_info_person flexi_term_policy_info_person = new bl_flexi_term_policy_info_person();
                                    flexi_term_policy_info_person.Birth_Date = birth_date;
                                    flexi_term_policy_info_person.First_Name = first_name;
                                    flexi_term_policy_info_person.Gender = gender;
                                    flexi_term_policy_info_person.ID_Card = id_card;
                                    flexi_term_policy_info_person.ID_Type = id_type;
                                    flexi_term_policy_info_person.Khmer_First_Name = "";
                                    flexi_term_policy_info_person.Khmer_Last_Name = "";
                                    flexi_term_policy_info_person.Last_Name = surname;
                                    flexi_term_policy_info_person.Resident = resident;
                                    flexi_term_policy_info_person.Branch = branch;
                                    flexi_term_policy_info_person.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    flexi_term_policy_info_person.Bank_Number = bank_number;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyInfoPerson(flexi_term_policy_info_person))
                                    {
                                        //failed                           
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term info person
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term insurance plan
                                    bl_flexi_term_policy_life_product flexi_term_policy_life_product = new bl_flexi_term_policy_life_product();

                                    flexi_term_policy_life_product.Age_Insure = flexi_term_primary_data.Age_Insured;

                                    flexi_term_policy_life_product.Assure_Year = Convert.ToInt32(hdfTermInsurance.Value);
                                    flexi_term_policy_life_product.Assure_Up_To_Age = flexi_term_policy_life_product.Age_Insure + flexi_term_policy_life_product.Assure_Year;

                                    flexi_term_policy_life_product.Pay_Mode = 0; //Single Payment

                                    flexi_term_policy_life_product.Pay_Year = Convert.ToInt32(hdfPaymentPeriod.Value);

                                    flexi_term_policy_life_product.Pay_Up_To_Age = 0;

                                    flexi_term_policy_life_product.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    flexi_term_policy_life_product.Product_ID = hdfProduct.Value;
                                    flexi_term_policy_life_product.System_Premium = 0;
                                    flexi_term_policy_life_product.System_Premium_Discount = 0;
                                    flexi_term_policy_life_product.System_Sum_Insure = 0;
                                    flexi_term_policy_life_product.User_Premium = flexi_term_primary_data.Premium;
                                    flexi_term_policy_life_product.User_Sum_Insure = flexi_term_primary_data.Sum_Insured;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyLifeProduct(flexi_term_policy_life_product))
                                    {
                                        //failed insert flexi term policy life product                                      
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term life product
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;

                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term beneficiary item
                                    bl_flexi_term_policy_benefit_item benefit_item = new bl_flexi_term_policy_benefit_item();
                                    benefit_item.Seq_Number = 1;
                                    benefit_item.Address = "";
                                    benefit_item.Birth_Date = Convert.ToDateTime("01/01/1900", dtfi2); //no benefitciary date of birth supplied
                                    benefit_item.Last_Name = beneficiary_surname.Trim().ToUpper(); 
                                    benefit_item.First_Name = beneficiary_first_name.Trim().ToUpper();
                                    benefit_item.Percentage = 100;
                                    benefit_item.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    benefit_item.Relationship = flexi_term_primary_data.Beneficiary_Relationship.Trim().ToUpper();
                                    benefit_item.Family_Book = flexi_term_primary_data.Family_Book;
                                    benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);
                                    benefit_item.Flexi_Term_Policy_Benefit_Item_ID = Helper.GetNewGuid("SP_Check_Flexi_Term_Policy_Benefit_Item_ID", "@Flexi_Term_Policy_Benefit_Item_ID").ToString();
                                    benefit_item.ID_Card = beneficiary_id_card;
                                    benefit_item.ID_Type = beneficiary_id_type;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyBenefitItem(benefit_item))
                                    {
                                        //failed insert flexi term policy benefit item
                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term policy benefit item
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term policy premium
                                    bl_flexi_term_policy_premium policy_premium = new bl_flexi_term_policy_premium();

                                    policy_premium.Created_By = hdfusername.Value;
                                    policy_premium.Created_Note = "";
                                    policy_premium.Created_On = DateTime.Now;
                                    policy_premium.Original_Amount = flexi_term_primary_data.Premium;
                                    policy_premium.Premium = flexi_term_primary_data.Premium;
                                    policy_premium.Sum_Insure = flexi_term_primary_data.Sum_Insured;
                                    policy_premium.Flexi_Term_Policy_ID = flexi_term_policy_id;

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyPremium(policy_premium))
                                    {
                                        //failed
                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);


                                        //failed insert flexi term policy premium
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }

                                //Not save error continue
                                if (error == false)
                                {
                                    //Insert flexi term policy status
                                    bl_flexi_term_policy_status policy_status = new bl_flexi_term_policy_status();

                                    policy_status.Created_By = hdfusername.Value;
                                    policy_status.Created_Note = "";
                                    policy_status.Created_On = DateTime.Now;
                                    policy_status.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    policy_status.Policy_Status_Type_ID = "IF";

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyStatus(policy_status))
                                    {
                                        //failed flexi term policy status
                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyPremium(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);


                                        //failed insert flexi term policy status
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                }
                                   
                                //Not save error continue
                                if (error == false)
                                {
                                     
                                    //Insert flexi term policy prem pay
                                    bl_flexi_term_policy_prem_pay policy_prem_pay = new bl_flexi_term_policy_prem_pay();

                                    policy_prem_pay.Amount = flexi_term_primary_data.Premium;
                                    policy_prem_pay.Channel_Location_ID = da_channel.GetChannelLocationIDByLocationName(branch);
                                    policy_prem_pay.Created_By = hdfusername.Value;
                                    policy_prem_pay.Created_Note = "";
                                    policy_prem_pay.Created_On = DateTime.Now;
                                    policy_prem_pay.Due_Date = flexi_term_primary_data.Maturity_Date;
                                    policy_prem_pay.Pay_Date = flexi_term_primary_data.Effective_Date;
                                    policy_prem_pay.Flexi_Term_Policy_ID = flexi_term_policy_id;
                                    policy_prem_pay.Prem_Lot = 1;
                                    policy_prem_pay.Prem_Year = 1;
                                    policy_prem_pay.Sale_Agent_ID = "";
                                    policy_prem_pay.Flexi_Term_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_Flexi_Term_Policy_Prem_Pay_ID", "@Flexi_Term_Policy_Prem_Pay_ID");

                                    if (!da_flexi_term_policy.InsertFlexiTermPolicyPremPay(policy_prem_pay))
                                    {

                                        da_flexi_term_policy.DeleteFlexiTermPolicyBenefitItem(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyLifeProduct(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyInfoPerson(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyCard(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyID(flexi_term_policy_id);
                                        da_flexi_term_policy.DeletePolicyNumber(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyPremium(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicyStatus(flexi_term_policy_id);
                                        da_flexi_term_policy.DeleteFlexiTermPolicy(flexi_term_policy_id);

                                        //failed insert flexi term policy prem pay
                                        upload_result.Result = "0";
                                        upload_result.Reason = "Save Failed";
                                        row_save.Add(upload_result);

                                        error = true;
                                    }
                                    else
                                    {
                                        //Policy Pay Mode
                                        da_policy.InsertPolicyPayMode("", flexi_term_policy_id, 0, flexi_term_primary_data.Maturity_Date, hdfusername.Value, DateTime.Now);

                                        //change card status
                                        da_banc_card.UpdateCardStatus(card_id, 0);

                                        upload_result.Result = "1";
                                        upload_result.Reason = "Save successfull";
                                        row_save.Add(upload_result);
                                    }
                                }
                            }

                        }


                    }//end loop

                    ////if reach here all save successfully
                    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new policy successfull.')", true);

                    ShowResult(row_save);
                    Clear();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please upload an excel file that contains policy flexi term data.')", true);
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

    private void ShowResult(List<bl_flexi_term_upload_result> row_save_result_list)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";

      
        for (int i = 0; i < row_save_result_list.Count; i++)
        {
            bl_flexi_term_upload_result row_save_result = new bl_flexi_term_upload_result();
            row_save_result = row_save_result_list[i];

            TableRow row = new TableRow();

            //No.
            TableCell cell1 = new TableCell();
            cell1.Style.Add("text-align", "center");
            cell1.Text = (i + 1).ToString();

            //Back Account No.
            TableCell cell2 = new TableCell();
            cell2.Style.Add("text-align", "center");  
            cell2.Text = row_save_result.Bank_Number;

            //First Name
            TableCell cell3 = new TableCell();
            cell3.Style.Add("text-align", "left");
            cell3.Style.Add("padding-left", "5px");
            cell3.Text = row_save_result.First_Name;

            //Last Name
            TableCell cell4 = new TableCell();
            cell4.Style.Add("text-align", "left");
            cell4.Style.Add("padding-left", "5px");
            cell4.Text = row_save_result.Last_Name;

            //Gender
            TableCell cell5 = new TableCell();
            cell5.Style.Add("text-align", "center");       
            cell5.Text = row_save_result.Gender;

            //Date of Birth
            TableCell cell6 = new TableCell();
            cell6.Style.Add("text-align", "center");
            cell6.Text = row_save_result.DOB;

            //ID No.
            TableCell cell7 = new TableCell();
            cell7.Style.Add("text-align", "center");
            cell7.Text = row_save_result.ID_Card;

            //ID Type
            TableCell cell8 = new TableCell();           
            cell8.Style.Add("text-align", "center");
            cell8.Text = row_save_result.ID_Type;

            //Result
            TableCell cell9 = new TableCell();
            cell9.Style.Add("text-align", "center");
            
            
            //Reason
            TableCell cell10 = new TableCell();
            cell10.Style.Add("text-align", "center");
            cell10.Text = row_save_result.Reason;

            if (row_save_result.Result == "0")
            {
                cell1.ForeColor = System.Drawing.Color.Red;
                cell2.ForeColor = System.Drawing.Color.Red;
                cell3.ForeColor = System.Drawing.Color.Red;
                cell4.ForeColor = System.Drawing.Color.Red;
                cell5.ForeColor = System.Drawing.Color.Red;
                cell6.ForeColor = System.Drawing.Color.Red;
                cell7.ForeColor = System.Drawing.Color.Red;
                cell8.ForeColor = System.Drawing.Color.Red;
                cell9.ForeColor = System.Drawing.Color.Red;
                cell10.ForeColor = System.Drawing.Color.Red;

                cell1.BorderColor = System.Drawing.Color.Black;
                cell2.BorderColor = System.Drawing.Color.Black;
                cell3.BorderColor = System.Drawing.Color.Black;
                cell4.BorderColor = System.Drawing.Color.Black;
                cell5.BorderColor = System.Drawing.Color.Black;
                cell6.BorderColor = System.Drawing.Color.Black;
                cell7.BorderColor = System.Drawing.Color.Black;
                cell8.BorderColor = System.Drawing.Color.Black;
                cell9.BorderColor = System.Drawing.Color.Black;
                cell10.BorderColor = System.Drawing.Color.Black;
                cell9.Text = "X";
            }
            else
            {
                cell1.ForeColor = System.Drawing.Color.Black;
                cell2.ForeColor = System.Drawing.Color.Black;
                cell3.ForeColor = System.Drawing.Color.Black;
                cell4.ForeColor = System.Drawing.Color.Black;
                cell5.ForeColor = System.Drawing.Color.Black;
                cell6.ForeColor = System.Drawing.Color.Black;
                cell7.ForeColor = System.Drawing.Color.Black;
                cell8.ForeColor = System.Drawing.Color.Black;
                cell9.ForeColor = System.Drawing.Color.Black;
                cell10.ForeColor = System.Drawing.Color.Black;

                cell1.BorderColor = System.Drawing.Color.Black;
                cell2.BorderColor = System.Drawing.Color.Black;
                cell3.BorderColor = System.Drawing.Color.Black;
                cell4.BorderColor = System.Drawing.Color.Black;
                cell5.BorderColor = System.Drawing.Color.Black;
                cell6.BorderColor = System.Drawing.Color.Black;
                cell7.BorderColor = System.Drawing.Color.Black;
                cell8.BorderColor = System.Drawing.Color.Black;
                cell9.BorderColor = System.Drawing.Color.Black;
                cell10.BorderColor = System.Drawing.Color.Black;
                cell9.Text = "\u221A";
            }

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
            row.Cells.Add(cell4);
            row.Cells.Add(cell5);
            row.Cells.Add(cell6);
            row.Cells.Add(cell7);
            row.Cells.Add(cell8);
            row.Cells.Add(cell9);
            row.Cells.Add(cell10);

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
        ddlCard.SelectedIndex = 0;
        txtPaymentPeriod.Text = "";
        txtTermInsurance.Text = "";
        ddlBank.SelectedIndex = 0;        
       
    }
}