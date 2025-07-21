using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Globalization;

/// <summary>
/// Summary description for UnderwritingWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class UnderwritingWebService : System.Web.Services.WebService {

    public UnderwritingWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

     static List<bl_sale_agent> sale_agent_list = new List<bl_sale_agent> { };
    
    //Get status of application with memo UW status
    [WebMethod]
    public string GetAppStatusDetails(string app_register_id, string status_code)
    {
        string app_status_details = da_underwriting.GetMemoStatusDetail(app_register_id);
        return app_status_details;
    }

    //Get status code by app_register_id
    [WebMethod]
    public string GetAppStatusCode(string app_register_id)
    {
        string app_status_code = da_underwriting.GetStatusCode(app_register_id);
        return app_status_code;
    }

    //Get EM rate with complete params
    [WebMethod]
    public double GetEMRate(string product_id, string gender, string customer_age, string em_percentage, string assure_year, string app_year, string pay_mode = "")
    {
        double em_rate = 0;
        
        if (product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36")
        {
            if (app_year == "2013")
            {
                em_rate = da_underwriting.GetEMRateMRTA2013(product_id, Convert.ToInt32(gender), Convert.ToInt32(customer_age), Convert.ToDouble(em_percentage), Convert.ToInt32(assure_year));
            }
            else //2014
            {
                em_rate = da_underwriting.GetEMRate(product_id, Convert.ToInt32(gender), Convert.ToInt32(customer_age), Convert.ToDouble(em_percentage), Convert.ToInt32(assure_year));
            }
            //em_rate = da_underwriting.GetEMRate(product_id, Convert.ToInt32(gender), Convert.ToInt32(customer_age), Convert.ToDouble(em_percentage), Convert.ToInt32(assure_year));  
        }

        else
        {
            em_rate = da_underwriting.GetEMRate(product_id, Convert.ToInt32(gender), Convert.ToInt32(customer_age), Convert.ToDouble(em_percentage), Convert.ToInt32(assure_year), pay_mode);        
        }
        
        
        return em_rate;
    }


    [WebMethod]
    public string GetDiscountAmount(string app_register_id)
    {
        string discount = da_underwriting.GetDiscount(app_register_id);
        return discount;
    }


    //Get underwriting CO
    [WebMethod]
    public bl_underwriting_co GetUWCO(string app_register_id, string product_id, string gender, string customer_age, string assure_year)
    {
        bl_underwriting_co my_new_co = da_underwriting.GetUnderwritingCOByParams(app_register_id, product_id, gender, customer_age, assure_year);        
        return my_new_co;
    }


    //Get complete underwriting detail
    [WebMethod]
    public bl_underwriting GetUWObject(string app_register_id)
    {
        bl_underwriting my_uw_obj = da_underwriting.GetUnderwritingObject(app_register_id);

        return my_uw_obj;
    }


    //Get Benefits by app_id
    [WebMethod]
    public List<bl_app_benefit_item> GetBenefits(string app_id)
    {
        List<bl_app_benefit_item> benefits = new List<bl_app_benefit_item>();

        benefits = da_underwriting.GetAppBenefitItem(app_id);

        return benefits;
    }

    //add IF row to table underwriting 
    [WebMethod]
    public bool UnderwritingRecord(List<string> UWRecords, string App_Register_ID)
    {
        bool status = true;
        int index = 0;
        try
        {
            if (UWRecords.Count > 0)
            {
                string user_name = System.Web.Security.Membership.GetUser().UserName;
                bl_underwriting obj_uw = new bl_underwriting();

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd/MM/yyyy";
                dtfi.DateSeparator = "/";
                DateTime effective_date = new DateTime();
                double em_amount = 0.00;
                DateTime Birth_Date = new DateTime(); //birth of life_insure

                //Life Insured Rider Level
                bl_underwriting life_insure = new bl_underwriting();
                life_insure = da_application.GetAppLifeProductInfo(App_Register_ID);
                bl_app_info_person app_info_person = da_application.GetAppInfoPerson(App_Register_ID);
                Birth_Date = app_info_person.Birth_Date;

                int Level = 0;

                foreach (string record in UWRecords)
                {
                    string[] arr_record = record.Split(';');

                    Level = Convert.ToInt32(arr_record[14].ToString());
                    effective_date = Convert.ToDateTime(arr_record[17].ToString(), dtfi); 
                    em_amount = Convert.ToDouble(arr_record[18].ToString());

                    if (Level != 12 && Level != 13)
                    {
                        obj_uw.Product_ID = arr_record[1].ToString();
                        obj_uw.Age_Insure = Convert.ToInt32(arr_record[2].ToString());
                        obj_uw.Pay_Year = Convert.ToInt32(arr_record[3].ToString());
                        obj_uw.Pay_Up_To_Age = Convert.ToInt32(arr_record[4].ToString());
                        obj_uw.Assure_Year = Convert.ToInt32(arr_record[5].ToString());
                        obj_uw.Assure_Up_To_Age = Convert.ToInt32(arr_record[6].ToString());
                        obj_uw.System_Sum_Insure = Convert.ToDouble(arr_record[7].ToString());
                        obj_uw.User_Premium = Convert.ToDouble(arr_record[8].ToString());
                        obj_uw.System_Premium = Convert.ToDouble(arr_record[9].ToString());
                        obj_uw.Pay_Mode = Convert.ToInt16(arr_record[10].ToString());
                        obj_uw.Original_Amount = Convert.ToDouble(arr_record[11].ToString());
                        obj_uw.Rounded_Amount = 0.00;
                        obj_uw.Extra_Amount = Convert.ToDouble(arr_record[12].ToString());
                        obj_uw.System_Premium_Discount = Convert.ToDouble(arr_record[13].ToString());
                        obj_uw.User_Premium_Discount = Convert.ToDouble(arr_record[13].ToString());
                        obj_uw.Level = Level;
                        obj_uw.Birth_Date = Helper.FormatDateTime(arr_record[15].ToString()); 
                        obj_uw.Gender = arr_record[16].ToString();

                        //add IF row to table underwriting 
                        if (index == 0)
                        {
                            if (!da_underwriting.AddUnderwritingRecord(App_Register_ID, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date))
                            {
                                return false;
                            }

                            index++;
                        }

                        //Insert With each level of (Spouse, kid1, 2, 3, 4, 5)
                        if (!da_underwriting.AddUWRiderLifeProductRecord(App_Register_ID, obj_uw.Level, obj_uw.Product_ID, obj_uw.Age_Insure, obj_uw.Pay_Year, obj_uw.Pay_Up_To_Age, obj_uw.Assure_Year, obj_uw.Assure_Up_To_Age, obj_uw.System_Sum_Insure, obj_uw.System_Sum_Insure, obj_uw.User_Premium, obj_uw.System_Premium, 0, obj_uw.Pay_Mode, effective_date, user_name, "", obj_uw.Original_Amount, obj_uw.Rounded_Amount, obj_uw.User_Premium_Discount, Convert.ToString(obj_uw.Birth_Date), Convert.ToInt32(obj_uw.Gender), obj_uw.Assure_Year))
                        {
                            return false;
                        }
                    }
                    else //Insert With each level of AD & TBD
                    {
                        //add IF row to table underwriting 
                        if (index == 0)
                        {
                            if (!da_underwriting.AddUnderwritingRecord(App_Register_ID, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date))
                            {
                                return false;
                            }

                            index++;
                        }

                        if (!da_underwriting.AddUWRiderLifeProductRecord(App_Register_ID, Level, life_insure.Product_ID, life_insure.Age_Insure, life_insure.Pay_Year, life_insure.Pay_Up_To_Age, life_insure.Assure_Year, life_insure.Assure_Up_To_Age, life_insure.System_Sum_Insure, life_insure.System_Sum_Insure, life_insure.User_Premium, life_insure.System_Premium, 0, life_insure.Pay_Mode, effective_date, user_name, "", life_insure.Original_Amount, life_insure.Rounded_Amount, life_insure.User_Premium_Discount, Convert.ToString(Birth_Date), Convert.ToInt32(life_insure.Gender), life_insure.Assure_Year))
                        {
                            return false;
                        }
                    }

                }

                if (!da_underwriting.AddUWRiderLifeProductRecord(App_Register_ID, 1, life_insure.Product_ID, life_insure.Age_Insure, life_insure.Pay_Year, life_insure.Pay_Up_To_Age, life_insure.Assure_Year, life_insure.Assure_Up_To_Age, life_insure.System_Sum_Insure, life_insure.System_Sum_Insure, life_insure.User_Premium, life_insure.System_Premium, 0, life_insure.Pay_Mode, effective_date, user_name, "", life_insure.Original_Amount, life_insure.Rounded_Amount, life_insure.User_Premium_Discount, Convert.ToString(Birth_Date), Convert.ToInt32(life_insure.Gender), life_insure.Assure_Year))
                {
                    return false;
                }

                if (em_amount > 0) //policy extra amount > 0
                {
                    status = da_underwriting.AddUWCO(App_Register_ID, obj_uw.System_Sum_Insure, obj_uw.System_Premium, 0.00, 0.00, 0.00, em_amount, 0.00, 0.00, 0.00, user_name, "");
                }

                if (!da_underwriting.AddUWEffectiveDate(App_Register_ID, effective_date, user_name))
                {
                    return false;
                }
            }

            index = 0;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UnderwritingRecord] in class [UnderwritingWebService], Detail: " + ex.Message);
            return false;
        }
        
        return status;
        
    }
    
}
