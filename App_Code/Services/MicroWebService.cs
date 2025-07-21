using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for MicroWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MicroWebService : System.Web.Services.WebService {

    public MicroWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    ////Get last policy number from Ct_Policy_Number
    //[WebMethod]
    //public string GetNewPolicyNumber()
    //{
    //    string new_policy_number = "";
    //    string last_policy_number = da_policy_micro.GetLastPolicyNumberMicro();

    //    //Convert policy number to int and plus 1
    //    int number = Convert.ToInt32(last_policy_number) + 1;

    //    new_policy_number = number.ToString();

    //    //Concate 0 to the front
    //    while (new_policy_number.Length < 8)
    //    {
    //        new_policy_number = "0" + new_policy_number;
    //    }

    //    return new_policy_number;
    //}

    //Get Zip Code
    [WebMethod]
    public string GetZipCode(string country_value)
    {
        string zip_code = da_policy_micro.GetZipCode(country_value);

        return zip_code;
    }

  
    //Check Barcode
    [WebMethod]
    public string CheckBarcode(string card, string barcode)
    {
        switch(card)
        {
            case "1":
                card = "T1011";
                break;
        }

        bool result = da_policy_micro.CheckBarcode(card, barcode);

        if (result)
        {
            return "1";
        }
        else
        {
            return "0";
        }

    }

    //Search Policy By Barcode Number (Internal)
    [WebMethod]
    public List<bl_policy_micro_search> SearchPolicyByBarcodeNoInternal(string barcode_number)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();
        policies = da_policy_micro.GetPolicyByBarcodeNoInternal(barcode_number);

        return policies;

    }

    //Search Policy By Policy Number
    [WebMethod]
    public List<bl_policy_micro_search> SearchPolicyByPolicyNoInternal(string policy_number)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();
        policies = da_policy_micro.GetPolicyByPolicyNoInternal(policy_number);

        return policies;

    }

    //Search Policy By Customer Name
    [WebMethod]
    public List<bl_policy_micro_search> SearchPolicyByCustomerNameInternal(string last_name, string first_name)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();
        policies = da_policy_micro.GetPolicyByCustomerNameInternal(first_name, last_name);

        return policies;

    }

    //Search Policy By ID Card
    [WebMethod]
    public List<bl_policy_micro_search> SearchPolicyByIDCardInternal(int id_type, string id_card_no)
    {
        List<bl_policy_micro_search> policies = new List<bl_policy_micro_search>();
        policies = da_policy_micro.GetPolicyByIDCardInternal(id_type, id_card_no);

        return policies;

    }

    //Get Policy Single Row Data
    [WebMethod]
    public bl_policy_micro_single_row_data GetPolicySingleRow(string policy_id)
    {
        bl_policy_micro_single_row_data policy_single_row_data = new bl_policy_micro_single_row_data();

        policy_single_row_data = da_policy_micro.GetPolicySingleRowData(policy_id);

        return policy_single_row_data;
    }

    //Get Benefits by policy_id
    [WebMethod]
    public List<bl_policy_micro_benefit_item> GetBenefits(string policy_id)
    {
        List<bl_policy_micro_benefit_item> benefits = new List<bl_policy_micro_benefit_item>();

        benefits = da_policy_micro.GetPolicyBenefitItem(policy_id);

        return benefits;
    }

    //Save new benefits
    [WebMethod]
    public string SaveBenefit(string policy_id, string dob, string name, string relation, string share)
    {
        try
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            string[] dob_list = dob.Split('}');           

            string[] name_list = name.Split('}');

            string[] share_list = share.Split('}');           

            string[] relation_list = relation.Split('}');

            for (int i = 0; i < relation_list.Count() - 1; i++)
            {
                if (relation_list[i].ToString() != "undefined")
                {
                    string my_name = name_list[i].Trim();
                    DateTime my_dob = Convert.ToDateTime(dob_list[i], dtfi);
                
                    double my_share = Convert.ToDouble(share_list[i]);
                    int my_seq_number = i + 1;
                    string my_relationship = relation_list[i];

                    if (my_name != "")
                    {
                        string new_guid = Helper.GetNewGuid("SP_Check_Policy_Micro_Benefit_Item_ID", "@Policy_Micro_Benefit_Item_ID").ToString();

                        bl_policy_micro_benefit_item benefit_item = new bl_policy_micro_benefit_item();

                        benefit_item.Policy_Micro_Benefit_Item_ID = new_guid;
                        benefit_item.Policy_Micro_ID = policy_id;
                        benefit_item.Full_Name = my_name.ToUpper();
                      
                        benefit_item.Birth_Date = my_dob;
                        benefit_item.Percentage = my_share;
                        benefit_item.Relationship = my_relationship;
                        benefit_item.Seq_Number = Convert.ToInt32(my_seq_number);
                        benefit_item.Relationship_Khmer = da_relationship.GetRelationshipKhmer(benefit_item.Relationship);

                        if (!da_policy_micro.InsertPolicyMicroBenefitItem(benefit_item))
                        { //if saving failed                           
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
            Log.AddExceptionToLog("Error in function [SaveBenefit] in class [PolicyWebService]. Details: " + ex.Message);          
            return "0";
        }
    }

    //Delete Benefit Items by policy_id
    [WebMethod]
    public int DeleteBenefitItems(string policy_id)
    {
        if (da_policy_micro.DeletePolicyMicroBenefitItem(policy_id))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    //Edit Address and contact
    [WebMethod]
    public string EditAddressAndContact(string policy_id, string address1, string address2, string province, string zip_code, string country, string mobile, string email)
    {        
        //Address
        bl_policy_micro_info_address address = new bl_policy_micro_info_address();

        address.Address1 = address1.Trim();
        address.Address2 = address2.Trim();
        address.Province = province.Trim();
        address.Zip_Code = zip_code;
        address.Country_ID = country.Trim();
        address.Policy_Micro_ID = policy_id;
       
        if (da_policy_micro.UpdatePolicyMicroInfoAddress(address))
        {

        }
        else
        {
            return "0"; // 0 == false
        }

        //Contact
        bl_policy_micro_info_contact contact = new bl_policy_micro_info_contact();
        contact.Policy_Micro_ID = policy_id;
        contact.EMail = email.Trim();   
        contact.Mobile_Phone1 = mobile.Trim();    

        if (da_policy_micro.UpdatePolicyMicroInfoContact(contact))
        {
            return "1"; // 1 == true
        }
        else
        {
            return "0"; //0 == false
        }
    }

    //Edit Personal Details
    [WebMethod]
    public string EditPersonalDetails(string policy_id, string id_type, string id_no, string surname_kh, string first_name_kh, string surname_en, string first_name_en)
    {
        //Personal Details
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        bl_policy_micro_info_person person = new bl_policy_micro_info_person();

        person.Policy_Micro_ID = policy_id;
        person.ID_Type = Convert.ToInt32(id_type);
        person.ID_Card = id_no.Trim();
        person.Last_Name = surname_en.ToUpper().Trim();
        person.First_Name = first_name_en.ToUpper().Trim();
        person.Khmer_First_Name = first_name_kh.Trim();
        person.Khmer_Last_Name = surname_kh.Trim();      
  
       
        if (da_policy_micro.UpdatePolicyMicroInfoPerson(person))
        {
            return "1"; // 1 == true
        }
        else
        {
            return "0"; //0 == false
        }
    }

    //Edit Insurance Plan
    [WebMethod]
    public string EditInsurancePlan(string policy_id, string customer_age, string term_insurance, string pay_year, string sum_insured, string pay_mode, string premium, string dob, string gender)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        bl_policy_micro_life_product policy_life_product = new bl_policy_micro_life_product();
    
        policy_life_product.Policy_Micro_ID = policy_id;
        policy_life_product.Age_Insure = Convert.ToInt32(customer_age);
        policy_life_product.Assure_Year = Convert.ToInt32(term_insurance);
        policy_life_product.Assure_Up_To_Age = policy_life_product.Age_Insure + policy_life_product.Assure_Year;
        policy_life_product.Pay_Year = Convert.ToInt32(pay_year);
        policy_life_product.Pay_Mode = Convert.ToInt32(pay_mode);
        policy_life_product.Pay_Up_To_Age = policy_life_product.Age_Insure + policy_life_product.Pay_Year;
        
        policy_life_product.User_Premium = Convert.ToDouble(premium);
        policy_life_product.User_Sum_Insure = Convert.ToDouble(sum_insured);

        if (da_policy_micro.UpdatePolicyMicroLifeProduct(policy_life_product))
        {
           //do nothing
        }
        else
        {
            return "0";
        }

        //policy premium
        bl_policy_micro_premium policy_premium = new bl_policy_micro_premium();
        policy_premium.Policy_Micro_ID = policy_id;
        policy_premium.Original_Amount = Convert.ToDouble(premium);
        policy_premium.Premium = Convert.ToDouble(premium);
        policy_premium.Sum_Insure = Convert.ToDouble(sum_insured);

        if (da_policy_micro.UpdatePolicyMicroPremium(policy_premium))
        {
            //do nothing
        }
        else
        {
            return "0";
        }

        //policy_prem_pay
        bl_policy_micro_prem_pay prem_pay = new bl_policy_micro_prem_pay();
        prem_pay.Policy_Micro_ID = policy_id;
        prem_pay.Amount = Convert.ToDouble(premium);

        if (da_policy_micro.UpdatePolicyMicroPremPay(prem_pay))
        {

            //do nothing
        }
        else
        {
            return "0";
        }


        if(da_policy_micro.UpdateGenderDOB(policy_id, Convert.ToInt32(gender), Convert.ToDateTime(dob, dtfi))){
            return "1";
        }else{
            return "0";
        }
    }

    //Check if policy already paid
    [WebMethod]
    public string CheckPolicyPremiumPay(string policy_id)
    {
        if(da_policy_micro.CheckPolicyMicroIDInPolicyMicroPremPay(policy_id)){
            return "1";
        }else{
            return "0";
        }
    }
    
}
