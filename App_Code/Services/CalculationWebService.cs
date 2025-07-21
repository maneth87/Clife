using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for CalculationWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CalculationWebService : System.Web.Services.WebService {

    public CalculationWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public int GetCustomerAge(string dob, string date_of_entry) 
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime my_date_of_entry = System.DateTime.Now;
        int customer_age = 0;

        if (date_of_entry != "")
        {
            my_date_of_entry = Convert.ToDateTime(date_of_entry, dtfi);
            customer_age = Calculation.Culculate_Customer_Age(dob, my_date_of_entry);
        }
        else
        {
            my_date_of_entry = Convert.ToDateTime(my_date_of_entry, dtfi);
            customer_age = Calculation.Culculate_Customer_Age(dob, my_date_of_entry);
        }

        
        return customer_age;
    }

    [WebMethod]
    public string GetPremium(string product_id, string sum_insured, string customer_age, string coverage_period, string gender, string product_type, string pay_mode)
    {
        string premium = "0,0";
        int mysum_insured = Convert.ToInt32(sum_insured);
        int mycustomer_age = Convert.ToInt32(customer_age);
        int mygender = Convert.ToInt32(gender);
        int myproduct_type = Convert.ToInt32(product_type);
        int mycoverage_period = Convert.ToInt32(coverage_period);
        int mypay_mode = Convert.ToInt32(pay_mode);
        //int my_is_pre_premium_discount = Convert.ToInt32(is_pre_premium_discount);
        bl_product product = da_product.GetProductByProductID(product_id);

        switch (product.Plan_Block)
        {
          
            case "A": //Whole Life Old
                premium = Calculation.CalculatePremiumWholeLife(product_id, mycustomer_age, mysum_insured, mygender, mypay_mode);
                break;
            case "M": //MRTA descreasing
                premium = Calculation.CalculatePremiumMRTA(product_id, mycustomer_age, mysum_insured, mycoverage_period, mygender, mypay_mode);
                break;

            case "T": //Term Life Old
                premium = Calculation.CalculatePremiumTermLife(product_id, mycustomer_age, mysum_insured, mygender, mypay_mode);
                break;

            case "001": //Whole Life New
            case "002": //Term Life New
            case "010": //Savings PP200          
                             
                    //calculate with product_id, age, SA, gender, pay mode and no factor discount (call it type 1)
                premium = Calculation.CalculatePremiumTypeOne(product_id, mycustomer_age, mysum_insured, mygender, mypay_mode, product.Plan_Block);
                               
                break;
          
            case "006": //MRTA 12
            case "007": //MRTA 24
            case "008": //MRTA 36
                premium = Calculation.CalculatePremiumTypeTwo(product_id, mycustomer_age, mysum_insured, mycoverage_period, mygender, mypay_mode, product.Plan_Block);
                break;
            case "004": //Premium Payback 15/10
                //calculate with product_id, age, SA, gender, pay mode and no factor discount (call it type 3)
                premium = Calculation.CalculatePremiumTypeThree(product_id, mycustomer_age, mysum_insured, mygender, mypay_mode);
                break;

            case "CL24"://Credit life 24
                premium = Calculation.CalculatePremiumCreditLife24(product_id, Convert.ToInt32( mycustomer_age), Convert.ToInt32( mysum_insured), Convert.ToInt32(mygender), Convert.ToInt32(mypay_mode), Convert.ToInt32(coverage_period));
                break;

            default:
                break;
        }


        return premium;
    }


    [WebMethod]
    public string GetPremiumDiscount(string annual_premium, string discount)
    {
        if (discount == "")
        {
            discount = "0";
        }

        decimal premium_discount = (Convert.ToDecimal(annual_premium) * Convert.ToDecimal(discount)) / 100;

        return String.Format("{0:#0.##}", premium_discount);

    }

    [WebMethod]
    public string GetPremiumAfterDiscount(string annual_premium, string premium_discount)
    {
        if (premium_discount == "")
        {
            premium_discount = "0";
        }

        decimal premium_after_discount = Convert.ToDecimal(annual_premium) - Convert.ToDecimal(premium_discount);

        return String.Format("{0:#0.##}", premium_after_discount);

    }

    [WebMethod]
    public string GetDiscountPercentage(string annual_premium, string discount_amount)
    {
        if (discount_amount == "")
        {
            discount_amount = "0";
        }

        decimal discount_rate = (Convert.ToDecimal(discount_amount) * 100) / Convert.ToDecimal(annual_premium);

        return String.Format("{0:#0.##}", discount_rate);

    }

    [WebMethod]
    public string GetDiscountAfterDiscountByPayMode(string system_premium, string discount_amount)
    {
        if (discount_amount == "")
        {
            discount_amount = "0";
        }

        if (system_premium == "")
        {
            system_premium = "0";
        }

        decimal premium_after_discount = Convert.ToDecimal(system_premium) - Convert.ToDecimal(discount_amount);

        return String.Format("{0:#0.##}", premium_after_discount);

    }

    //Micro
    [WebMethod]
    public int GetCustomerAgeMicro(string dob)
    {

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime date_of_birth = Convert.ToDateTime(dob, dtfi);

        int customer_age = Calculation.Culculate_Customer_Age_Micro(date_of_birth, System.DateTime.Now.AddHours(24));
        return customer_age;
    }

    //Micro
    [WebMethod]
    public string GetPremiumMicro(string amount)
    {
        if (amount == "")
        {
            return "";
        }
        else
        {
            double premium = Convert.ToDouble(amount) * 0.01;
            return premium.ToString();
        }

    }
    [WebMethod]
    public DateTime GetMaturityDateByMode(string effective_date, string cover_year)
    {
        DateTime maturity_date = DateTime.Now;

        if (effective_date != "")
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime effectiveDate = DateTime.ParseExact(effective_date, "dd/MM/yyyy", provider);
            maturity_date = effectiveDate;
            try
            {
                if (cover_year != "")
                {
                    maturity_date = Convert.ToDateTime(effectiveDate).AddYears(Convert.ToInt32(cover_year));
                }
                else
                {
                    maturity_date = effectiveDate;
                }
                //if (cover_year == 0)
                //{
                //    maturity_date = Convert.ToDateTime(effectiveDate).AddMonths(12);
                //}
                //else if (cover_year == 1) // Annual
                //{
                //    maturity_date = Convert.ToDateTime(effectiveDate).AddMonths(12); 
                //}

                //else if (cover_year == 2) // Semi
                //{
                //    maturity_date = Convert.ToDateTime(effectiveDate).AddMonths(6); 
                //}

                //else if (cover_year == 3) // Quarter
                //{
                //    maturity_date = Convert.ToDateTime(effectiveDate).AddMonths(3); 
                //}

                //else if (cover_year == 4) // Month
                //{
                //    maturity_date = Convert.ToDateTime(effectiveDate).AddMonths(1); 
                //}

            }
            catch (Exception ex)
            {
                //write error to log
                Log.AddExceptionToLog("Error: class [da_report_sale_policy], function [GetMaturityDateByMode]. Details: " + ex.Message);
            }
        }
        
        return maturity_date;
    }
}
