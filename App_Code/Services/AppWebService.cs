using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Globalization;
using System.Reflection;

/// <summary>
/// Summary description for ApplicationWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AppWebService : System.Web.Services.WebService {

    public string MYNAME = "AppWebService";
    
    //Constructor
    public AppWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //Save new application register
    [WebMethod]
    public string RegisterApplication(string app_number, string sale_agent_id, string date_signature, string created_by, string created_note, string created_on, string benefit_note, string user_id, string payment_code, string channel_id, string channel_item_id)
    {

        try
        {
            int count = 0;

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            //Get Office_ID
            string office_id = "HQ"; //Important Note: Office_ID set fixed to HQ for current version
            string new_guid = Helper.GetNewGuid("SP_Check_App_Register_ID", "@App_Register_ID").ToString();
          
            bl_app_register app_register = new bl_app_register();
            app_register.App_Register_ID = new_guid;
            app_register.App_Number = app_number;
            app_register.Original_App_Number = app_number;
            app_register.Office_ID = office_id;
            app_register.Created_On = Convert.ToDateTime(System.DateTime.Now, dtfi);
            app_register.Created_By = created_by;
            app_register.Payment_Code = payment_code;
            app_register.Channel_ID = channel_id;
            app_register.Channel_Item_ID = channel_item_id;
           
            //Check for null
            if (created_note == "")
            {
                app_register.Created_Note = "";
            }
            else
            {
                app_register.Created_Note = created_note.Trim();
            }

            //Save Ct_App_Register
            if (da_application.InsertAppRegister(app_register))
            {
                count += 1;
            }

            bl_app_info app_info = new bl_app_info();
            app_info.App_Register_ID = new_guid;
            app_info.App_Date = Convert.ToDateTime(date_signature, dtfi);
            app_info.Office_ID = office_id;
            app_info.Sale_Agent_ID = sale_agent_id;
           
            app_info.Created_By = created_by;

            app_info.Created_On = Convert.ToDateTime(System.DateTime.Now, dtfi);

            //Check for null
            if (benefit_note == "")
            {
                app_info.Benefit_Note = "";
            }
            else
            {
                app_info.Benefit_Note = benefit_note.Trim();
            }

            if (created_note == "")
            {
                app_info.Created_Note = "";
            }
            else
            {
                app_info.Created_Note = created_note.Trim();
            }

            //Save Ct_App_Info
            if (da_application.InsertAppInfo(app_info))
            {
                count += 1;
            }
            else
            {
                //Rollback
                da_application.DeleteAppInfo(new_guid);
                da_application.DeleteAppRegister(new_guid);
            }

            if (count < 2)
            {
                return "0"; //registration failed return 0
            }
            else
            {
                if (SaveAppIDToTempTable(new_guid, user_id))
                {
                    return new_guid; //registration success return app_register_id to continue inserting info
                }
                else
                {
                    return "0";
                }
               
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [RegisterApplication] in class [AppWebService]. Details: " + ex.Message);
            return "0";
        }
    }

    //Save new benefits
    [WebMethod]
    public string SaveBenefiter(string app_id, string id_type, string id_no, string name, string relation, string share, string seq_number, string remarks)
    {
        try
        {

            string[] id_type_list = id_type.Split(',');

            string[] id_no_list = id_no.Split(',');

            string[] name_list = name.Split(',');

            string[] share_list = share.Split(',');

            string[] seq_number_list = seq_number.Split(',');

            string[] relation_list = relation.Split(',');
            string[] remarks_list = remarks.Split(',');

            for (int i = 0; i < id_type_list.Count() - 1; i++)
            {
                if (id_type_list[i].ToString() != "undefined")
                {
                    string my_name = name_list[i];
                    int my_id_type = Convert.ToInt32(id_type_list[i]);
                    string my_id_no = id_no_list[i];
                    double my_share = Convert.ToDouble(share_list[i]);
                    int my_seq_number = i + 1;
                    string my_relationship = relation_list[i];
                    string my_remarks = remarks_list[i];

                    if (my_name != "")
                    {
                        string new_guid = Helper.GetNewGuid("SP_Check_App_Benefit_Item_ID", "@App_Benefit_Item_ID").ToString();

                        bl_app_benefit_item benefit_item = new bl_app_benefit_item();

                        benefit_item.App_Benefit_Item_ID = new_guid;
                        benefit_item.App_Register_ID = app_id;
                        benefit_item.Full_Name = my_name.Trim();
                        benefit_item.ID_Type = my_id_type;
                        benefit_item.ID_Card = my_id_no.Trim();
                        benefit_item.Percentage = my_share;
                        benefit_item.Relationship = my_relationship;
                        benefit_item.Seq_Number = Convert.ToInt32(my_seq_number);
                        benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);
                        benefit_item.Remarks = my_remarks;

                        if (!da_application.InsertAppBenefitItem(benefit_item))
                        { //if saving failed rollback
                            Rollback(app_id);
                            return "0";
                        }
                    }
                }
            }
           
                return "1"; //successful         
                       
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [SaveBenefiter] in class [AppWebService]. Details: " + ex.Message);
            Rollback(app_id);
            return "0";
        }
    }

    //Save new premium discount
    [WebMethod]
    public string SavePremiumDiscount(string app_id, string discount_year, string discount_rate, string premium_discount, string premium_after_discount)
    {
        try
        {

            string[] discount_year_list = discount_year.Split(',');

            string[] discount_rate_list = discount_rate.Split(',');

            string[] premium_discount_list = premium_discount.Split(',');

            string[] premium_after_discount_list = premium_after_discount.Split(',');

            for (int i = 0; i < discount_year_list.Count() - 1; i++)
            {
                if (discount_year_list[i].ToString() != "undefined")
                {
                    int my_discount_year = Convert.ToInt32(discount_year_list[i]);
                    double my_discount_rate = Convert.ToDouble(discount_rate_list[i]);
                    double my_premium_discount = Convert.ToDouble(premium_discount_list[i]);
                    double my_premium_after_discount = Convert.ToDouble(premium_after_discount_list[i]);

                    if (my_discount_year > 0)
                    {
                        string new_guid = Helper.GetNewGuid("SP_Check_App_Premium_Discount_ID", "@App_Premium_Discount_ID").ToString();

                        bl_app_premium_discount app_premium_discount = new bl_app_premium_discount();

                        app_premium_discount.App_Premium_Discount_ID = new_guid;
                        app_premium_discount.App_Register_ID = app_id;
                        app_premium_discount.Discount_Rate = my_discount_rate;
                        app_premium_discount.Premium_After_Discount = my_premium_after_discount;
                        app_premium_discount.Premium_Discount = my_premium_discount;
                        app_premium_discount.Year = my_discount_year;

                        if (!da_application.InsertAppPremiumDiscount(app_premium_discount))
                        { //if saving failed rollback
                            Rollback(app_id);
                            return "0";
                        }
                    }
                }
            }

            return "1"; //successful         

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [SavePremiumDiscount] in class [AppWebService]. Details: " + ex.Message);
            Rollback(app_id);
            return "0";
        }
        }

    //Save App_Register_ID to Ct_App_ID_Temp
    private bool SaveAppIDToTempTable(string app_id, string user_id)
    {
        try
        {          
            if (da_application.InsertAppIDToTempTable(app_id, user_id))
            {
                return true;
            }
            else
            {
                return false;

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [SaveAppIDToTempTable] in class [AppWebService]. Details: " + ex.Message);
            Rollback(app_id);
            return false;
        }
    }

    //Rollback
    [WebMethod]
    public void Rollback(string app_id)
    {
        da_application.DeleteAppLifeProduct(app_id);
        da_application.DeleteAppInfo(app_id);
        da_application.DeleteAppRegister(app_id);
        da_application.DeleteAppBenefitItem(app_id);
        da_application.DeleteAppPremiumDiscount(app_id);
    }

    //Check Existing Applicaiton Number
    [WebMethod]
    public string CheckApplicationNumber (string app_number)
    {
        if (da_application.CheckAppNumber(app_number))
        {
            return "1";

        }
        else
        {
            return "0";
        }
    }

    //Search Application By App Number
    [WebMethod]
    public List<bl_app_search> SearchApplicationByAppNo(string app_number)
    {            
        List<bl_app_search> applications = new List<bl_app_search>();
        applications = da_application.GetApplicationByAppNo(app_number);

        return applications;

    }

    //Search Application By Customer Name
    [WebMethod]
    public List<bl_app_search> SearchApplicationByCustomerName(string last_name, string first_name)
    {
        List<bl_app_search> applications = new List<bl_app_search>();
        applications = da_application.GetApplicationByCustomerName(last_name, first_name);
              
        return applications;

    }

    //Search Application By ID Card
    [WebMethod]
    public List<bl_app_search> SearchApplicationByIDCard(string id_type, string id_card_no)
    {
        int id_type_number = Convert.ToInt32(id_type);

        List<bl_app_search> applications = new List<bl_app_search>();
        applications = da_application.GetApplicationByIDCard(id_type_number, id_card_no);

        return applications;

    }

    //Get Application Single Row Data
    [WebMethod]
    public bl_app_single_row_data GetAppSingleRow(string app_id)
    {
        bl_app_single_row_data app_single_row_data = new bl_app_single_row_data();

        app_single_row_data = da_application.GetAppSingleRowData(app_id);

        return app_single_row_data;
    }

    //Get Premium Discount by app_id
    [WebMethod]
    public List<bl_app_premium_discount> GetPremiumDiscount(string app_id)
    {
        List<bl_app_premium_discount> discounts = new List<bl_app_premium_discount>();

        discounts = da_application.GetAppPremiumDiscount(app_id);

        return discounts;
    }

    //Get Benefits by app_id
    [WebMethod]
    public List<bl_app_benefit_item> GetBenefits(string app_id)
    {
        List<bl_app_benefit_item> benefits = new List<bl_app_benefit_item>();

        benefits = da_application.GetAppBenefitItem(app_id);

        return benefits;
    }

    //Get Answers by app_id
    [WebMethod]
    public List<bl_app_answer_item> GetAnswers(string app_id)
    {
        List<bl_app_answer_item> answers = new List<bl_app_answer_item>();

        answers = da_application.GetAppAnswerItem(app_id);

        return answers;
    }

    //Delete Premium Discount by app_id
    [WebMethod]
    public int DeletePremiumDiscount(string app_id)
    {
        if (da_application.DeleteAppPremiumDiscount(app_id))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    //Delete Benefit Items by app_id
    [WebMethod]
    public int DeleteBenefitItems(string app_id)
    {
        if(da_application.DeleteAppBenefitItem(app_id)){
            return 1;
        }else{
            return 0;
        }        
    }

    //Save back Previous Benefits
    [WebMethod]
    public string SavePreviousBenefiter(string app_id, string id_type, string id_no, string name, string relation, string share)
    {
        try
        {

            string[] id_type_list = id_type.Split(',');

            string[] id_no_list = id_no.Split(',');

            string[] name_list = name.Split(',');

            string[] share_list = share.Split(',');

            string[] relation_list = relation.Split(',');
			int my_seq_number = 0;
            for (int i = 0; i < id_type_list.Count() - 1; i++)
            {
                if (id_type_list[i].ToString() != "undefined")
                {
                    string my_name = name_list[i];
                    int my_id_type = Convert.ToInt32(id_type_list[i]);
                    string my_id_no = id_no_list[i];
                    double my_share = Convert.ToDouble(share_list[i]);
                    my_seq_number += 1;
                    string my_relationship = relation_list[i];

                    if (my_name != "")
                    {
                        string new_guid = Helper.GetNewGuid("SP_Check_App_Benefit_Item_ID", "@App_Benefit_Item_ID").ToString();

                        bl_app_benefit_item benefit_item = new bl_app_benefit_item();

                        benefit_item.App_Benefit_Item_ID = new_guid;
                        benefit_item.App_Register_ID = app_id;
                        benefit_item.Full_Name = my_name.Trim();
                        benefit_item.ID_Type = my_id_type;
                        benefit_item.ID_Card = my_id_no.Trim();
                        benefit_item.Percentage = my_share;
                        benefit_item.Relationship = my_relationship;
                        benefit_item.Seq_Number = Convert.ToInt32(my_seq_number);
                        benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);

                        da_application.InsertAppBenefitItem(benefit_item);
                    }
                }
            }

            return "1"; //successful
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [SavePreviousBenefiter] in class [AppWebService]. Details: " + ex.Message);
            return "0";
        }
    }


    //Get Zip Code
    [WebMethod]
    public string GetZipCode(string country_value)
    {
       string zip_code = da_application.GetZipCode(country_value);

       return zip_code;
    }

    //Edit Address and contact
    [WebMethod]
    public string EditAddressAndContact(string app_id, string address1, string address2, string province, string zip_code, string country, string tel, string mobile, string email){
           

        //Address
        bl_app_info_address address = new bl_app_info_address();

        address.Address1 = address1.Trim();
        address.Address2 = address2.Trim();
        address.Province = province.Trim();
        address.Zip_Code = zip_code;
        address.Country_ID = country.Trim();
        address.App_Register_ID = app_id;

        if (da_application.UpdateAppInfoAddress(address))
        {
            
        }else{
            return "0"; // 0 == false
        }

        //Contact
        bl_app_info_contact contact = new bl_app_info_contact();
        contact.App_Register_ID = app_id;
        contact.EMail = email.Trim();
        contact.Home_Phone1 = tel.Trim();
        contact.Mobile_Phone1 = mobile.Trim();
        contact.Fax1 = "";
        contact.Fax2 = "";
        contact.Home_Phone2 = "";
        contact.Office_Phone1 = "";
        contact.Office_Phone2 = "";
        contact.Mobile_Phone2 = "";

        if (da_application.UpdateAppInfoContact(contact))
        {
            return "1"; // 1 == true
        }
        else
        {
            return "0"; //0 == false
        }
    }

    //Edit Job History
    [WebMethod]
    public string EditJobHistory(string app_id, string employer_name, string nature_business, string role_response, string current_position, double annual_income)
    {
        //Address
        bl_app_job_history job_history = new bl_app_job_history();

        job_history.Employer_Name = employer_name.Trim();
        job_history.Nature_Of_Business = nature_business.Trim();
        job_history.Job_Role = role_response.Trim();
        job_history.Current_Position = current_position.Trim();
        job_history.Anual_Income = annual_income;

        job_history.App_Register_ID = app_id;

        if (da_application.UpdateAppJobHistory(job_history))
        {
            return "1"; // 1 == true
        }
        else
        {
            return "0"; // 0 == false
        }

    }

    //Edit Personal Details
    [WebMethod]
    public string EditPersonalDetails(string app_id, string id_type, string id_no, string surname_kh, string first_name_kh, string surname_en, string first_name_en, string father_surname, string father_first_name, string Mother_surname, string Mother_first_name, string previous_surname, string previous_first_name, string nationality, string dob, string gender)
    {
        //Personal Details
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        bl_app_info_person person = new bl_app_info_person();

        person.App_Register_ID = app_id;
        person.ID_Type = Convert.ToInt32(id_type);
        person.ID_Card = id_no.Trim();
        person.Last_Name = surname_en.Trim();
        person.First_Name = first_name_en.Trim();
        person.Khmer_First_Name = first_name_kh.Trim();
        person.Khmer_Last_Name = surname_kh.Trim();
        person.Father_First_Name = father_first_name.Trim();
        person.Father_Last_Name = father_surname.Trim();
        person.Mother_First_Name = Mother_first_name.Trim();
        person.Mother_Last_Name = Mother_surname.Trim();
        person.Prior_First_Name = previous_first_name.Trim();
        person.Prior_Last_Name = previous_surname.Trim();
        person.Country_ID = nationality;
        person.Birth_Date = Convert.ToDateTime(dob, dtfi);
        person.Gender = Convert.ToInt32(gender);
     
        if (da_application.UpdateAppInfoPerson(person))
        {
            return "1"; // 1 == true
        }
        else
        {
            return "0"; //0 == false
        }
    }

    #region Inserting App_Life_Product by Meas SUN ON 22-01-2020
    [WebMethod]
    public bool InsertAppLifeProduct(string app_id, string customer_age, string product_id, string assure_year, string pay_year, string sum_insured, string premium_system, string pay_mode, string premium, string discount_amount, string annual_premium)
    {
        bl_app_life_product app_life_product = new bl_app_life_product();

        app_life_product.App_Register_ID = app_id;
        app_life_product.Age_Insure = Convert.ToInt32(customer_age);
        app_life_product.Assure_Year = Convert.ToInt32(assure_year);
        app_life_product.Assure_Up_To_Age = app_life_product.Age_Insure + app_life_product.Assure_Year;
        app_life_product.Pay_Year = Convert.ToInt32(pay_year);
        app_life_product.Pay_Mode = Convert.ToInt32(pay_mode);
        app_life_product.Pay_Up_To_Age = app_life_product.Age_Insure + app_life_product.Pay_Year;
        app_life_product.Product_ID = product_id;
        app_life_product.System_Premium_Discount = 0.00;
        app_life_product.System_Sum_Insure = Convert.ToDouble(sum_insured);
        app_life_product.User_Premium = Convert.ToDouble(premium);
        app_life_product.System_Premium = Convert.ToDouble(premium_system);
        app_life_product.User_Sum_Insure = Convert.ToDouble(sum_insured);
        
        if (da_application.InsertAppLifeProduct(app_life_product))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    #endregion

    //Edit Insurance Plan
    [WebMethod]
    public string EditInsurancePlan(string app_id, string dob, string gender, string customer_age, string product_id, string term_insurance, string pay_year, string sum_insured, string premium_system, string pay_mode, string premium, string discount_amount, string annual_premium, string insurance_plan_note)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        bl_app_life_product app_life_product = new bl_app_life_product();

        app_life_product.App_Register_ID = app_id;

        app_life_product.Age_Insure = Convert.ToInt32(customer_age);
             
        app_life_product.Assure_Year = Convert.ToInt32(term_insurance);
        app_life_product.Assure_Up_To_Age = app_life_product.Age_Insure + app_life_product.Assure_Year;
               

        app_life_product.Pay_Year = Convert.ToInt32(pay_year);
        app_life_product.Pay_Mode = Convert.ToInt32(pay_mode);
        app_life_product.Pay_Up_To_Age = app_life_product.Age_Insure + app_life_product.Pay_Year;
        app_life_product.Product_ID = product_id;
        app_life_product.System_Premium = Convert.ToDouble(premium_system);
        app_life_product.System_Premium_Discount = 0.00;
        app_life_product.System_Sum_Insure = Convert.ToDouble(sum_insured);
        app_life_product.User_Premium = Convert.ToDouble(premium);
        app_life_product.User_Sum_Insure = Convert.ToDouble(sum_insured);
        //app_life_product.Is_Pre_Premium_Discount = Convert.ToInt32(is_pre_premium_discount);

        if (da_application.UpdateAppLifeProduct(app_life_product))
        {
            //do nothing
        }
        else
        {
            return "0";
        }

        //update gener and dob
        if (da_application.UpdateGenderDOB(app_id, Convert.ToInt32(gender), Convert.ToDateTime(dob, dtfi)))
        {
            
        }
        else
        {
            return "0";
        }
         
        //update discount on pay mode
        bl_app_discount app_discount = new bl_app_discount();
        app_discount.Total_Amount = Convert.ToDouble(premium);
        app_discount.Premium = Convert.ToDouble(premium_system);
        app_discount.Pay_Mode = Convert.ToInt32(pay_mode);
        app_discount.Discount_Amount = Convert.ToDouble(discount_amount);
        app_discount.Annual_Premium = Convert.ToDouble(annual_premium);
        app_discount.App_Register_ID = app_id;
        app_discount.Created_Note = insurance_plan_note;

        if (da_application.UpdateAppDiscount(app_discount))
        {
            return "1";
        }
        else
        {
            return "0";
        }

    }

    //Edit Original Amount
    [WebMethod]
    public string EditOriginalAmount(string app_id, string original_amount,  string amount)
    {

        if (original_amount == "")
        {
            original_amount = "0";
        }      

        if (amount == "")
        {
            amount = "0";
        }

        if (da_application.UpdateOriginalAmount(app_id, Convert.ToDouble(original_amount), Math.Ceiling(Convert.ToDouble(original_amount)), Convert.ToDouble(amount)))
        {
            //do nothing
            return "1";
        }
        else
        {
            return "0";
        }       

    }
    [WebMethod]
   
    public List<bl_micro_application_search> SearchApplication(string applicationNumber, string customerName, string customerGender, string customerDOB, string agentName, string agentCode)
    {
        List<bl_micro_application_search> listSearch = new List<bl_micro_application_search>();
        try
        {
            int gender = -1;
            DateTime dob;
            gender = Convert.ToInt32( customerGender=="" ? "-1" : customerGender);
            dob =Helper.FormatDateTime( customerDOB=="" ? "01-01-1900": customerDOB);
            listSearch = da_micro_application.SearchApplication(applicationNumber, customerName, dob, gender, agentCode, agentName);
        }
        catch (Exception ex)
        {
            listSearch = new List<bl_micro_application_search>();

            Log.AddExceptionToLog("Error function [SearchApplication(string applicationNumber, string customerName, string customerGender, string customerDOB, string agentname, string agentCode)] in class [AppWebService], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
        return listSearch;
    }

    [WebMethod]
    public List<bl_referral> SearchReferral(string channelItemId, string channelLocationId, string referralInfo)
    {
        try
        {
            List<bl_referral> reList = new List<bl_referral>();
            reList = da_referral.GetActiveReferral(channelItemId, channelLocationId, referralInfo, "WEB SERVICE");
            return reList;
        }
        catch (Exception ex)
        {
           
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = "WEB SERVICE"
            });
            return new List<bl_referral>();
        }
    }
}
