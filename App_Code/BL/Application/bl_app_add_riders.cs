using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for bl_app_add_riders
/// </summary>
public class bl_app_add_riders
{
	public bl_app_add_riders()
	{
		//
		// TODO: Add constructor logic here
		//
       
	}
    public bl_app_add_riders( string rider_id,  string app_register_id, string product_id, int level, string rider_type, int rate_id, double rate, 
                            double sum_insured, double premium, double discount, double original_amount, double annual_rounded_amount,
                            int age_insure, int pay_year, int assure_year, int pay_up_to_age, int assure_up_to_age, DateTime effective_date)
    {
        this.Rider_ID = rider_id;
        this.App_Register_ID = app_register_id;
        this.Product_ID = product_id;
        this.Level = level;
        this.Rider_Type = rider_type;
        this.Rider_ID = rider_id;
        this.Rate = rate;
        this.Sum_Insured = sum_insured;
        this.Premium = premium;
        this.Discount = discount;
        this.Original_Amount = original_amount;
        this.Annual_Rounded_Amount = annual_rounded_amount;
        this.Age_Insure = age_insure;
        this.Assure_Year = assure_year;
        this.Pay_Year = pay_year;
        this.Pay_Up_To_Age = pay_up_to_age;
        this.Assure_Up_To_Age = assure_up_to_age;
        this.Effective_Date = effective_date;
    }

    public string Rider_ID { get; set; }
    public string App_Register_ID { get; set; }
    public string Product_ID { get; set; }
    public int Level { get; set; }
    public string Rider_Type { get; set; }
    public int Rate_ID { get; set; }
    public double Rate {get;set;}
    public double Sum_Insured { get; set; }
    public double Premium { get; set; }
    public double Discount { get; set; }
    public double Original_Amount { get; set; }
    public double Annual_Rounded_Amount { get; set; }
    public int Age_Insure { get; set; }
    public int Pay_Year { get; set; }
    public int Assure_Year { get; set; }
    public int Pay_Up_To_Age { get; set; }
    public int Assure_Up_To_Age { get; set; }
    public DateTime Effective_Date { get; set; }
}