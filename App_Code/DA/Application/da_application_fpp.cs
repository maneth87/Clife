using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_application_fpp
/// </summary>
public class da_application_fpp
{
	public da_application_fpp()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// Check varlid age range of life insured
    /// Age range: [18 - 50]
    /// Expire age = 65
    /// </summary>
    /// <returns></returns>
    public static string Varlid_Life_Insured_Age_Range(int age)
    {
        string message = "";
        try
        {
            if (age >= 18 && age <= 50)
            {
                message = "";
            }
            else
            {
                message = "Invarlid age, life insured's age range [18-50].";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error functioin [Varlid_Life_Insured_Age_Range] in class [da_application_fpp], Detail: " + ex.Message);
            message = "Ooop! something wrong in function Varlid_Life_Insured_Age_Range.";
        }
        return message;
    }
    /// <summary>
    /// Check varlid age range of spouse
    /// Age range: [18 - 50]
    /// Expire age = 65
    /// </summary>
    /// <returns></returns>
    public static string Varlid_Spouse_Age_Range(int age)
    {
        string message = "";
        try
        {
            if (age >= 18 && age <= 50)
            {
                message = "";
            }
            else
            {
                message = "Invarlid age, spouse's age range [18-50].";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error functioin [Varlid_Spouse_Age_Range] in class [da_application_fpp], Detail: " + ex.Message);
            message = "Ooop! something wrong in function Varlid_Spouse_Age_Range.";
        }
        return message;
    }
    /// <summary>
    /// Check varlid age range of kid
    /// Age range: [1 - 17]
    /// Expire age = 21
    /// </summary>
    /// <returns></returns>
    public static string Varlid_Kid_Age_Range(int age)
    {
        string message = "";
        try
        {
            if (age >= 1 && age <= 17)
            {
                message = "";
            }
            else
            {
                message = "Invarlid age, kid's age range [1-17].";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error functioin [Varlid_Kid_Age_Range] in class [da_application_fpp], Detail: " + ex.Message);
            message = "Ooop! something wrong in function Varlid_Kid_Age_Range.";
        }
        return message;
    }
    /// <summary>
    /// Class Cal_Age_Assure_Pay_Year
    /// Use to Calculate Age Insure, Pay year, Pay up to age, Assure year and Assure up to age
    /// </summary>
    public class Cal_Age_Assure_Pay_Year
    {
        public int Age_Insure { get; set; }
        public int Pay_Year { get; set; }
        public int Pay_Up_To_Age { get; set; }
        public int Assure_Year { get; set; }
        public int Assure_Up_To_Age { get; set; }
        /// <summary>
        /// Constactors of Class Cal_Age_Assure_Pay_Year
        /// This fuction use to assign the value to class
        /// </summary>
        /// <param name="age_insure"></param>
        /// <param name="pay_year"></param>
        /// <param name="pay_up_to_age"></param>
        /// <param name="assure_year"></param>
        /// <param name="assure_up_to_age"></param>
        public Cal_Age_Assure_Pay_Year(int age_insure, int pay_year, int pay_up_to_age, int assure_year, int assure_up_to_age)
        {
            this.Age_Insure = age_insure;
            this.Pay_Year = pay_year;
            this.Pay_Up_To_Age = pay_up_to_age;
            this.Assure_Up_To_Age = assure_year;
            this.Assure_Up_To_Age = assure_up_to_age;
        }
        /// <summary>
        /// Constactors of Class Cal_Age_Assure_Pay_Year
        /// This fuction use to assign the value to class by system automaitically calculation
        /// policy_insured_year is the number of years which was already insured
        /// </summary>
        /// <param name="customer_age"></param>
        /// <param name="plan_assure_year"></param>
        /// <param name="expiry_age"></param>
        /// <param name="policy_insured_year"></param>
        public Cal_Age_Assure_Pay_Year(int customer_age, int plan_assure_year , int expiry_age, int policy_insured_year)
        {
            int insure_age = 0;
            int assure_year = 0;
           
            insure_age = customer_age + plan_assure_year;
            if (insure_age <= expiry_age)
            {
                assure_year = plan_assure_year;
            }
            else
            { 
                assure_year = plan_assure_year -(insure_age - expiry_age);
            }

            Age_Insure = customer_age;
            Pay_Year = assure_year - policy_insured_year;
            Pay_Up_To_Age = assure_year + customer_age - policy_insured_year;
            Assure_Year = assure_year - policy_insured_year;
            Assure_Up_To_Age = assure_year + customer_age - policy_insured_year;
        }

    }
    /// <summary>
    /// This function get underwriting status of rider by application id and level
    /// Level: 2. Spouse, 
    ///        3. kid1, 
    ///        4. kid2, 
    ///        5. kid3, 
    ///        6. kid6, 
    ///        12. TPD, 
    ///        13. ADB 
    /// </summary>
    /// <param name="app_register_id"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static string GetUWRiderStatus(string app_register_id, int level)
    {
        string status = "";
        try
        {
            string query = "";
            query = "select Status_Code from Ct_Underwriting_Rider where  App_Register_ID='" + app_register_id + "' AND [Level] =" + level;
            DataTable tbl = DataSetGenerator.Get_Data_Soure(query);
            if (tbl.Rows.Count > 0)
            {
                status = tbl.Rows[0][0].ToString().Trim();
            }
            else
            {
                status = "";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetUWRiderStatus] in class [da_application_fpp], Detail: " + ex.Message);
            status = "fail";
        }
        return status;
    }
}