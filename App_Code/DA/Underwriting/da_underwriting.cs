using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

/// <summary>
/// Summary description for da_underwriting
/// </summary>
public class da_underwriting
{
	private static da_underwriting mytitle = null;
    
    public da_underwriting()
	{
		if (mytitle == null)
        {
            mytitle = new da_underwriting();
        }
    }

    #region "Public Functions"

    //Add new row underwriting table
    public static bool AddUnderwritingRecord(string app_register_id, int result, string status_code, string updated_by, string updated_note, DateTime updated_on)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name            
            cmd.CommandText = "SP_Insert_Underwriting_Record";

            //bind parameter
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Underwrite_Doc_Number", GetUWDocNumber());
            cmd.Parameters.AddWithValue("@Is_Clean_Case", 0);
            cmd.Parameters.AddWithValue("@Result", result);
            cmd.Parameters.AddWithValue("@Status_Code", status_code);
            cmd.Parameters.AddWithValue("@Updated_On", updated_on);
            cmd.Parameters.AddWithValue("@Updated_By", updated_by);
            cmd.Parameters.AddWithValue("@Updated_Note", updated_note);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUnderwritingRecord] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }


    //Update underwriting record
    public static bool UpdateUnderwritingRecord(string app_register_id, int result, string status_code, string updated_by, string updated_note, DateTime updated_on)
    {
        bool result_query = false;        

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            
            //call store procedure by name
            cmd.CommandText = "SP_UpdateUW_Status_Code_By_App_Register_ID";

            //bind parameter
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Result", result);
            cmd.Parameters.AddWithValue("@Status_Code", status_code);
            cmd.Parameters.AddWithValue("@Updated_By", updated_by);
            cmd.Parameters.AddWithValue("@Updated_On", updated_on); //Change to datetime.now after inserting data
            cmd.Parameters.AddWithValue("@Updated_Note", updated_note);
            
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result_query = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateUnderwritingRecord] in class [da_underwriting]. Details: " + ex.Message);                
            }

        }
        //return true if successful
        return result_query;
    }

    //Update data in uw_life_product when user click on inforce button. Check if age is different. if it's different, calculate everything again then update... if not, no update.
    public static bool UpdateUWLifeProductRecord(string app_register_id, string product_id, int age_insure, int pay_year, int pay_up_to_age, int assure_year, int assure_up_to_age, double user_sum_insure, double system_sum_insure, double user_premium, double system_premium, double system_premium_discount, int pay_mode, DateTime created_on, string created_by, string created_note, double original_amount, double rounded_amount, double user_premium_discount, string dob, int gender, int coverage_period)
    {
        bool AddRecord = false;

        //Get age insure by effective date
        int my_age_insure = GetCustomerAge(dob, created_on.ToString());

        //If age increase, get new premium calculation based on new age
        if (my_age_insure > age_insure)
        {
            bl_product product = da_product.GetProductByProductID(product_id);
            string premium = "0,0";

            switch (product.Plan_Block)
            {
                case "A": //Whole Life Old
                    premium = Calculation.CalculatePremiumWholeLife(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode);
                    break;
                case "M": //MRTA descreasing
                    premium = Calculation.CalculatePremiumMRTA(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), coverage_period, gender, pay_mode);
                    break;

                case "T": //Term Life Old
                    premium = Calculation.CalculatePremiumTermLife(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode);
                    break;

                case "001": //Whole Life New (CAM Whole)
                case "002": //Term Life New (CAM Protection)
                case "010": //Savings PP200          

                    //calculate with product_id, age, SA, gender, pay mode and no factor discount (call it type 1)
                    premium = Calculation.CalculatePremiumTypeOne(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode, product.Plan_Block);

                    break;

                case "006": //MRTA 12
                case "007": //MRTA 24
                case "008": //MRTA 36
                    premium = Calculation.CalculatePremiumTypeTwo(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), coverage_period, gender, pay_mode, product.Plan_Block);
                    break;
                case "004": //Premium Payback 15/10
                    //calculate with product_id, age, SA, gender, pay mode and no factor discount (call it type 3)
                    premium = Calculation.CalculatePremiumTypeThree(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode);
                    break;
                case "NFP15/15": //Premium NFP15/15
                    premium = da_application_fp6.getPremiumFP6(Convert.ToDouble(user_sum_insure), product_id, gender, my_age_insure, pay_mode, assure_year)+"";
                    break;
                default:
                    break;
            }

            //Premium returns 1. premium by pay mode which is already rounded 2. original premium separated by (,)
            original_amount = Convert.ToDouble(premium.Split(',')[1]);
            rounded_amount = Math.Ceiling(original_amount);

            //Assign the answer to premium before passing to db table
            //user_premium = Convert.ToDouble(premium.Split(',')[0]);
            system_premium = Convert.ToDouble(premium.Split(',')[0]);

            age_insure = my_age_insure;

            //Get new assure year for whole life product 
            if (product.Product_ID == "W10" || product.Product_ID == "W15" || product.Product_ID == "W20" || product.Product_ID == "W9010" || product.Product_ID == "W9015" || product.Product_ID == "W9020")
            {
                assure_year = GetAssureYear(product_id, my_age_insure.ToString());
            }

            //other product, assure year remains the same
            assure_up_to_age = my_age_insure + assure_year;
            pay_up_to_age = my_age_insure + pay_year;


            //Get EM object
            bl_underwriting_co my_uw_co = GetUnderwritingCOByAppID(app_register_id);

            //Re-calculate EM Rate
            my_uw_co.EM_Rate = GetEMRate(product_id, gender, my_age_insure, my_uw_co.EM_Percent, assure_year);

            //Re-calculate EM Premium
            my_uw_co.EM_Premium = (system_sum_insure * my_uw_co.EM_Rate) / 1000;

            //Re-calculate EM Amount by pay_mode_id
            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * GetPaymentFactor(pay_mode));

            //Update UW_Co table
            bool update_co = UpdateUWCO(my_uw_co);


            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                //call store procedure by name
                cmd.CommandText = "SP_Update_UW_Life_Product_Record";

                //get new primary key for the row to be inserted
                string new_guid = Helper.GetNewGuid("SP_Check_UW_Life_Product_ID", "@UW_Life_Product_ID").ToString();

                //bind parameter
               
                cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);                
                cmd.Parameters.AddWithValue("@Product_ID", product_id);
                cmd.Parameters.AddWithValue("@Age_Insure", age_insure);
                cmd.Parameters.AddWithValue("@Pay_Year", pay_year);
                cmd.Parameters.AddWithValue("@Pay_Up_To_Age", pay_up_to_age);
                cmd.Parameters.AddWithValue("@Assure_Year", assure_year);
                cmd.Parameters.AddWithValue("@Assure_Up_To_Age", assure_up_to_age);
                cmd.Parameters.AddWithValue("@User_Sum_Insure", user_sum_insure);
                cmd.Parameters.AddWithValue("@System_Sum_Insure", system_sum_insure);
                cmd.Parameters.AddWithValue("@User_Premium", user_premium);
                cmd.Parameters.AddWithValue("@System_Premium", system_premium);
                cmd.Parameters.AddWithValue("@System_Premium_Discount", system_premium_discount);
                cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);
                cmd.Parameters.AddWithValue("@Original_Amount", original_amount);
                cmd.Parameters.AddWithValue("@Rounded_Amount", rounded_amount);
                cmd.Parameters.AddWithValue("@User_Premium_Discount", user_premium_discount);

                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    AddRecord = true;
                }
                catch (Exception ex)
                {
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [UpdateUWLifeProductRecord] in class [da_underwriting]. Details: " + ex.Message);
                }

                

            }

        }
        
        //return true if successful
        return AddRecord;
    }

    //Update data in uw_life_product when user click on inforce button. Check if age is different. if it's different, calculate everything again then update... if not, no update.
    public static bool UpdateRiderUWLifeProductRecord(string app_register_id, int level, string product_id, int age_insure, int pay_year, int pay_up_to_age, int assure_year, int assure_up_to_age, double user_sum_insure, double system_sum_insure, double user_premium, double system_premium, double system_premium_discount, int pay_mode, DateTime created_on, string created_by, string created_note, double original_amount, double rounded_amount, double user_premium_discount, string dob, int gender, int coverage_period)
    {
        bool AddRecord = false;
        string sub_product = "";
        sub_product = product_id.Substring(0, 3).ToUpper().Trim();

        //Get age insure by effective date
        int my_age_insure = GetCustomerAge(dob, created_on.ToString());

        #region If age increase, get new premium calculation based on new age
        if (my_age_insure > age_insure)
        {

            bl_product product = da_product.GetProductByProductID(product_id);
            double new_premium = 0.0;
            double new_original_amount = 0.0;

            //get plan assure from life product
            da_application_fp6.ProductFP6 life_product = new da_application_fp6.ProductFP6();
            List<da_application_fp6.ProductFP6> list_product = new List<da_application_fp6.ProductFP6>();
            list_product = da_application_fp6.ProductFP6.GetProductFP6List();

            //get plan assure year
            int assure_plan = 0;
            for (int i = 0; i < list_product.Count; i++)
            {
                var life = list_product[i];
                if (product_id == life.ProductID)
                {
                    assure_plan = life.AssureYear;
                    break;
                }
            }

            int new_assure_age = 0;

            if (sub_product == "NFP")
            {
                #region Family Protection
                //Life insured 1
                if (level == 1)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 70)
                    {
                        assure_year = assure_plan;
                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 70);
                    }

                    //new_premium = da_application_fp6.getPremiumFP6(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6(user_sum_insure, product_id, gender, my_age_insure, assure_year);
                    double[,] arr_premium = da_application_fp6.getPremiumFP6(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year);
                    new_premium = arr_premium[0, 0];
                    new_original_amount = arr_premium[0, 1];


                }

                //Spouse
                else if (level == 2)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 70)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 70);
                    }

                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 0);
                    double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                    new_premium = arr_premium[0, 0];
                    new_original_amount = arr_premium[0, 1];

                }
                //Kids
                else if (level > 2 & level < 5)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 21)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 21);
                    }

                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);

                    double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    new_premium = arr_premium[0, 0];
                    new_original_amount = arr_premium[0, 1];

                }

                //TPD  
                else if (level >= 12)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 65);
                    }

                   

                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);

                    string str_premium = da_application_fp6.GetTPDPremium(user_sum_insure, product_id, gender, my_age_insure, assure_year, pay_mode);
                    string[] arr_premium = str_premium.Split('/');
                    new_premium = Convert.ToDouble(arr_premium[0].ToString());
                    new_original_amount = Convert.ToDouble(arr_premium[1].ToString());
                }
                //ADB
                else if (level >= 13)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 65);
                    }

                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);

                    int rate_id = da_application_fp6.GetADBRate(app_register_id);
                    string class_rate = "";
                    class_rate = "Class " + rate_id;
                    double rate = da_application_fp6.GetClassRate(class_rate);
                    string str_premium = da_application_fp6.GetADBPremium(user_sum_insure, product_id, rate, pay_mode);
                    string[] arr_premium = str_premium.Split('/');
                    new_premium = Convert.ToDouble(arr_premium[0].ToString());
                    new_original_amount = Convert.ToDouble(arr_premium[1].ToString());

                }

                original_amount = new_original_amount;
                rounded_amount = Math.Ceiling(original_amount);//round up

                system_premium = new_premium;
                age_insure = my_age_insure;

                assure_up_to_age = my_age_insure + assure_year;
                pay_up_to_age = my_age_insure + assure_year;

                //Get EM object
                bl_underwriting_co my_uw_co = GetUnderwritingCOByAppID(app_register_id);
                GetCORiderByAppID_Level(app_register_id, level);

                //Re-calculate EM Rate
                //life insured 1
                if (level == 1)
                {
                    my_uw_co.EM_Rate = GetEMRate(product_id, gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }

                else if (level == 2)//spouse
                {
                    my_uw_co.EM_Rate = GetEMRiderRate(0, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }
                else if (level > 2 && level < 5)//kids
                {
                    my_uw_co.EM_Rate = GetEMRiderRate(1, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }
                else if (level == 12) //TPD
                {
                    my_uw_co.EM_Rate = GetTPDEMRate("", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }

                //Re-calculate EM Premium
                my_uw_co.EM_Premium = (system_sum_insure * my_uw_co.EM_Rate) / 1000;

                //Re-calculate EM Amount by pay_mode_id
                switch (pay_mode)
                {
                    case 0:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                        break;
                    case 1:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                        break;
                    case 2:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.52);
                        break;
                    case 3:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.27);
                        break;
                    case 4:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.09);
                        break;

                }

                //Update UW_Co table
                bool update_co = UpdateUWCO(my_uw_co);

                #endregion
            }
            else if (sub_product == "FPP")
            {
                #region Family protection package
                //Life insured 1
                if (level == 1)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65)
                    {
                        assure_year = assure_plan;
                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 65);
                    }

                    new_premium = da_application_fp6.GetPremiumFPPackage(product_id, assure_year, gender, my_age_insure);
                    new_original_amount = new_premium;
                }

                //Spouse
                else if (level == 2)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 65);
                    }

                    new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 1);
                    new_original_amount = new_premium;
                }
                //Kids
                else if (level > 2 & level < 5)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 21)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 21);
                    }
                    new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 0);
                    new_original_amount = new_premium;
                }

                //TPD
                else if (level == 12)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 65);
                    }
                    new_premium = da_application_fp6.GetTPDPremiumFPPackage(product_id, assure_year, gender, 0);
                    new_original_amount = new_premium;
                }
                //ADB
                else if (level == 13)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65)
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        //assure_year = assure_plan - my_age_insure;
                        assure_year = assure_plan - (new_assure_age - 65);
                    }
                    string str_class="";
                    str_class="Class " + assure_plan + "/" + assure_plan;
                    new_premium = da_application_fp6.GetADBPremiumFPPackage(str_class);
                    new_original_amount = new_premium;
                }

                original_amount = new_original_amount;
                rounded_amount = Math.Ceiling(original_amount);//round up

                system_premium = new_premium;
                age_insure = my_age_insure;

                assure_up_to_age = my_age_insure + assure_year;
                pay_up_to_age = my_age_insure + assure_year;

                //Get EM object
                bl_underwriting_co my_uw_co = GetUnderwritingCOByAppID(app_register_id);
                GetCORiderByAppID_Level(app_register_id, level);

                //Re-calculate EM Rate
                //life insured 1
                if (level == 1)
                {
                    my_uw_co.EM_Rate = GetEMRate(product_id, gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }

                else if (level == 2)//spouse
                {
                    my_uw_co.EM_Rate = GetEMRiderRate(0, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }
                else if (level > 2 && level < 5)//kids
                {
                    my_uw_co.EM_Rate = GetEMRiderRate(1, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }
                else if (level == 12) //TPD
                {
                    my_uw_co.EM_Rate = GetTPDEMRate("", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                }

                //Re-calculate EM Premium
                my_uw_co.EM_Premium = (system_sum_insure * my_uw_co.EM_Rate) / 1000;

                //Re-calculate EM Amount by pay_mode_id
                switch (pay_mode)
                {
                    case 0:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                        break;
                    case 1:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                        break;
                    case 2:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.52);
                        break;
                    case 3:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.27);
                        break;
                    case 4:
                        my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.09);
                        break;

                }

                //Update UW_Co table
                bool update_co = UpdateUWCO(my_uw_co);

                #endregion
            }
            else if (sub_product == "SDS")
            {
                string[] arr_product = product_id.Split('/');
                #region Study save
                if (arr_product.Length == 2)
                {
                    #region Study save normal
                    if (level == 1)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;
                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }

                        //new_premium = da_application_fp6.GetPremiumFPPackage(product_id, assure_year, gender, my_age_insure);
                        //new_original_amount = new_premium;
                        double[,] arr_premium = da_application_study_save.study_save.GetPremium(my_age_insure, gender, product_id, assure_year, pay_mode,user_sum_insure);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1]; ;
                    }

               //Spouse
                    else if (level == 2)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }

                        //new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 1);
                        //new_original_amount = new_premium;

                        double[,] arr_premium = da_application_study_save.study_save. GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }
                    //Kids
                    else if (level > 2 & level < 5)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 21)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 21);
                        }
                        //new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 0);
                        //new_original_amount = new_premium;

                        double[,] arr_premium = da_application_study_save.study_save.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }

                    //TPD
                    else if (level == 12)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }
                        double[,] arr_premium = da_application_study_save.study_save.GetTPDPremium(my_age_insure, gender, product_id, assure_year, pay_mode, user_sum_insure);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }
                    //ADB
                    else if (level == 13)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }
                        string str_class = "";
                        int rate_id = da_application_fp6.GetADBRate(app_register_id);
                        str_class = "Class " + rate_id + "/" + product_id;
                        double rate = da_application_fp6.GetClassRate(str_class);

                        double[,] arr_premium = da_application_study_save.study_save.GetADBPremium(product_id, pay_mode, user_sum_insure, rate) ;
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }

                    original_amount = new_original_amount;
                    rounded_amount = Math.Ceiling(original_amount);//round up

                    system_premium = new_premium;
                    age_insure = my_age_insure;

                    assure_up_to_age = my_age_insure + assure_year;
                    pay_up_to_age = my_age_insure + assure_year;

                    //Get EM object
                    bl_underwriting_co my_uw_co = GetUnderwritingCOByAppID(app_register_id);
                    GetCORiderByAppID_Level(app_register_id, level);

                    //Re-calculate EM Rate
                    //life insured 1
                    if (level == 1)
                    {
                        my_uw_co.EM_Rate = GetEMRate(product_id, gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }

                    else if (level == 2)//spouse
                    {
                        my_uw_co.EM_Rate = GetEMRiderRate(0, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }
                    else if (level > 2 && level < 5)//kids
                    {
                        my_uw_co.EM_Rate = GetEMRiderRate(1, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }
                    else if (level == 12) //TPD
                    {
                        my_uw_co.EM_Rate = GetTPDEMRate("", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }

                    //Re-calculate EM Premium
                    my_uw_co.EM_Premium = (system_sum_insure * my_uw_co.EM_Rate) / 1000;

                    //Re-calculate EM Amount by pay_mode_id
                    switch (pay_mode)
                    {
                        case 0:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                            break;
                        case 1:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                            break;
                        case 2:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.52);
                            break;
                        case 3:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.27);
                            break;
                        case 4:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.09);
                            break;

                    }

                    //Update UW_Co table
                    bool update_co = UpdateUWCO(my_uw_co);
                    #endregion
                }
                else if (arr_product.Length > 2)
                {
                    #region Study save package, package protection, maturiy
                    if (level == 1)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;
                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }

                        //new_premium = da_application_fp6.GetPremiumFPPackage(product_id, assure_year, gender, my_age_insure);
                        //new_original_amount = new_premium;
                        double[,] arr_premium = da_application_study_save.study_save_package.GetPremium(my_age_insure, gender, product_id, assure_year, pay_mode);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1]; ;
                    }

               //Spouse
                    else if (level == 2)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }

                        //new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 1);
                        //new_original_amount = new_premium;

                        double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }
                    //Kids
                    else if (level > 2 & level < 5)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 21)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 21);
                        }
                        //new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 0);
                        //new_original_amount = new_premium;

                        double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }

                    //TPD
                    else if (level == 12)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }
                        double[,] arr_premium = da_application_study_save.study_save_package.GetTPDPremium(my_age_insure, gender, product_id, assure_year, pay_mode);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }
                    //ADB
                    else if (level == 13)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65)
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            //assure_year = assure_plan - my_age_insure;
                            assure_year = assure_plan - (new_assure_age - 65);
                        }
                        string str_class = "";
                        int rate_id = da_application_fp6.GetADBRate(app_register_id);
                        str_class = "Class "  + product_id;

                        double[,] arr_premium = da_application_study_save.study_save_package.GetADBPremium(str_class, pay_mode);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                    }

                    original_amount = new_original_amount;
                    rounded_amount = Math.Ceiling(original_amount);//round up

                    system_premium = new_premium;
                    age_insure = my_age_insure;

                    assure_up_to_age = my_age_insure + assure_year;
                    pay_up_to_age = my_age_insure + assure_year;

                    //Get EM object
                    bl_underwriting_co my_uw_co = GetUnderwritingCOByAppID(app_register_id);
                    GetCORiderByAppID_Level(app_register_id, level);

                    //Re-calculate EM Rate
                    //life insured 1
                    if (level == 1)
                    {
                        my_uw_co.EM_Rate = GetEMRate(product_id, gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }

                    else if (level == 2)//spouse
                    {
                        my_uw_co.EM_Rate = GetEMRiderRate(0, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }
                    else if (level > 2 && level < 5)//kids
                    {
                        my_uw_co.EM_Rate = GetEMRiderRate(1, "", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }
                    else if (level == 12) //TPD
                    {
                        my_uw_co.EM_Rate = GetTPDEMRate("", gender, my_age_insure, my_uw_co.EM_Percent, assure_year);
                    }

                    //Re-calculate EM Premium
                    my_uw_co.EM_Premium = (system_sum_insure * my_uw_co.EM_Rate) / 1000;

                    //Re-calculate EM Amount by pay_mode_id
                    switch (pay_mode)
                    {
                        case 0:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                            break;
                        case 1:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 1);
                            break;
                        case 2:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.52);
                            break;
                        case 3:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.27);
                            break;
                        case 4:
                            my_uw_co.EM_Amount = Math.Ceiling(my_uw_co.EM_Premium * 0.09);
                            break;

                    }

                    //Update UW_Co table
                    bool update_co = UpdateUWCO(my_uw_co);
                    #endregion
                }

                
            }
#endregion
            
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                //call store procedure by name
                cmd.CommandText = "SP_Update_UW_Rider_Life_Product_Record";

                //get new primary key for the row to be inserted
                string new_guid = Helper.GetNewGuid("SP_Check_UW_Life_Product_ID", "@UW_Life_Product_ID").ToString();

                //bind parameter

                cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd.Parameters.AddWithValue("@Product_ID", product_id);
                cmd.Parameters.AddWithValue("@Level", level);
                cmd.Parameters.AddWithValue("@Age_Insure", age_insure);
                cmd.Parameters.AddWithValue("@Pay_Year", pay_year);
                cmd.Parameters.AddWithValue("@Pay_Up_To_Age", pay_up_to_age);
                cmd.Parameters.AddWithValue("@Assure_Year", assure_year);
                cmd.Parameters.AddWithValue("@Assure_Up_To_Age", assure_up_to_age);
                cmd.Parameters.AddWithValue("@User_Sum_Insure", user_sum_insure);
                cmd.Parameters.AddWithValue("@System_Sum_Insure", system_sum_insure);
                cmd.Parameters.AddWithValue("@User_Premium", user_premium);
                cmd.Parameters.AddWithValue("@System_Premium", system_premium);
                cmd.Parameters.AddWithValue("@System_Premium_Discount", system_premium_discount);
                cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);
                cmd.Parameters.AddWithValue("@Original_Amount", original_amount);
                cmd.Parameters.AddWithValue("@Rounded_Amount", rounded_amount);
                cmd.Parameters.AddWithValue("@User_Premium_Discount", user_premium_discount);

                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    AddRecord = true;
                }
                catch (Exception ex)
                {
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [UpdateRiderUWLifeProductRecord] in class [da_underwriting]. Details: " + ex.Message);
                }
            }

        }
        //nothing changed
        else
        {
            //default true
            AddRecord = true;
        }
        #endregion  If age increase, get new premium calculation based on new age

        //return true if successful
        return AddRecord;
    }

    //Check underwriting
    public static bool CheckUnderwriting(string app_register_id)
    {
        bool result = false;       

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Check_Underwriting";

            //bind parameter
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {                        
                        result = true;                        
                    }
                }                
                
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [CheckUnderwriting] in class [da_underwriting]. Details: " + ex.Message);
            }
            con.Close();
        }
        return result;
    }


    //Get E.M. rate
    /// <summary>
    /// Get extra premium rate by: product_id, gender, age, percentage, assure year
    /// </summary>
    /// <param name="product_id"></param>
    /// <param name="gender"></param>
    /// <param name="customer_age"></param>
    /// <param name="em_percentage"></param>
    /// <param name="assure_year"></param>
    /// <returns></returns>
    public static double GetEMRate(string product_id, int gender, int customer_age, double em_percentage, int assure_year, string pay_mode = "")
    {
        double em_rate = default (double);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_EM_Rate";
            #region Check for family protection package
            //customer's (male and female) extra premium for family protection package use extra premium of family protection rate (gender = male, age = 39)  
            if (product_id.Substring(0, 3).ToUpper() == "FPP")
            {
                gender = 1;//male
                customer_age = 39;
                product_id = "NFP" + assure_year + "/" + assure_year; // family protection 
            }
            #endregion
            #region Study Save Package 
            else if (product_id.Substring(0, 4).ToUpper() == "SDSP")
            { 
            //product id package format Abbreviation/Assured Year/Sum Insured
            //product id normal format Abbreviation/Assured Year
            //this case for package product will be used by fixed condition age=39 and gender=1(male) to calculate extra premium
                gender = 1;//male
                customer_age = 39;
                product_id = "SDS" + assure_year + "/" + assure_year;
           
            #endregion
            }
            
            cmd.Parameters.AddWithValue("@Product_ID", product_id);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Age", customer_age);
            cmd.Parameters.AddWithValue("@EM_Percent", em_percentage);
            cmd.Parameters.AddWithValue("@Assure_Year", assure_year);
            cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        em_rate = rdr.GetDouble(rdr.GetOrdinal("EM_Permue"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetEMRate] in class [da_underwriting]. Details: " + ex.Message);
            }            

        }
        return em_rate;
    }

    //Get E M Rate for rider
    public static double GetEMRiderRate(int insured_type, string product_id, int gender, int customer_age, double em_percentage, int assure_year)
    {
        double em_rate = default(double);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_EM_Rider_Rate";
            #region Check for family protection package
            //customer's (male and female) extra premium rider (TPD) for family protection package use extra premium rider (TPD) rate (gender = male, age = 39)  
            if (product_id.Substring(0, 3).ToUpper() == "FPP")
            {
                gender = 1;//male
                customer_age = 39;
            }
            
            #endregion
            #region Study save
            else if (product_id.Substring(0, 3).ToUpper() == "SDS")
            {
                string[] arr_product = product_id.Split('/');
                if (arr_product.Length > 2)
                {
                    gender = 1;
                    customer_age = 39;
                }
            }
            #endregion

            cmd.Parameters.AddWithValue("@Product_ID", product_id);
            cmd.Parameters.AddWithValue("@Insured_Type", insured_type);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Age", customer_age);
            cmd.Parameters.AddWithValue("@EM_Percent", em_percentage);
            cmd.Parameters.AddWithValue("@Assure_Year", assure_year);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        em_rate = rdr.GetDouble(rdr.GetOrdinal("EM_Permue"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetEMRiderRate] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return em_rate;
    }
    //Get E M Rate for rider
    public static double GetTPDEMRate(string product_id, int gender, int customer_age, double em_percentage, int assure_year)
    {
        double em_rate = default(double);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_TPD_EM_Rate";
           

            cmd.Parameters.AddWithValue("@Product_ID", product_id);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Age", customer_age);
            cmd.Parameters.AddWithValue("@EM_Percent", em_percentage);
            cmd.Parameters.AddWithValue("@Assure_Year", assure_year);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        em_rate = rdr.GetDouble(rdr.GetOrdinal("EM_Permue"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetTPDEMRate] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return em_rate;
    }

    //Get E.M. rate for MRTA for 2013
    public static double GetEMRateMRTA2013(string product_id, int gender, int customer_age, double em_percentage, int assure_year)
    {
        double em_rate = default(double);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_EM_Rate_MRTA_2013";

            cmd.Parameters.AddWithValue("@Product_ID", product_id);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Age", customer_age);
            cmd.Parameters.AddWithValue("@EM_Percent", em_percentage);
            cmd.Parameters.AddWithValue("@Assure_Year", assure_year);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        em_rate = rdr.GetDouble(rdr.GetOrdinal("EM_Permue"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetEMRateMRTA2013] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return em_rate;
    }

    //Get last underwriting doc number
    public static string GetUWDocNumber()
    {
        string uw_doc_number = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Last_Underwrite_Doc_Number";

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        uw_doc_number = rdr.GetString(rdr.GetOrdinal("Underwrite_Doc_Number"));

                        int strConvert = Convert.ToInt16(uw_doc_number) + 1;
                        uw_doc_number = strConvert.ToString("D8");

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetWUDocNumber] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return uw_doc_number;
    }


    //Get last CO doc number
    public static string GetCODocNumber()
    {
        string co_doc_number = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Last_CO_Doc_Number";

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        co_doc_number = rdr.GetString(rdr.GetOrdinal("CO_Doc_Number"));

                        int strConvert = Convert.ToInt16(co_doc_number) + 1;
                        co_doc_number = strConvert.ToString("D8");

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCODocNumber] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return co_doc_number;
    }


    //Get detail of application that has status = memo
    public static string GetMemoStatusDetail(string app_register_id)
    {
        string memo_detail = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Underwrite_Updated_Note_by_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        memo_detail = rdr.GetString(rdr.GetOrdinal("Updated_Note"));
                        
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetMemoStatusDetail] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return memo_detail;
    }



    //Get detail of application that has status = memo
    public static string GetStatusCode(string app_register_id)
    {
        string status_code = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Underwrite_Status_Code_by_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        status_code = rdr.GetString(rdr.GetOrdinal("Status_Code"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetStatusCode] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return status_code;
    }


    //Get customer age by effective date
    public static int GetCustomerAge(string dob, string effective_date)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";

        DateTime my_effective_date = System.DateTime.Now;

        if (effective_date != "")
        {
            my_effective_date = Convert.ToDateTime(effective_date, dtfi);
        }

        int customer_age = Culculate_Customer_Age(dob, my_effective_date);
        return customer_age;
    }


    public static int Culculate_Customer_Age(string strdob, DateTime compare_date)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy/";
        dtfi.DateSeparator = "/";

        DateTime dob = Convert.ToDateTime(strdob, dtfi);

        TimeSpan mytimespan = compare_date.Subtract(dob);
        int no_of_day = mytimespan.Days;

        //Get leap year count
        int number_of_leap_year = Get_Number_Of_Leap_Year(dob.Year, compare_date.Year);

        decimal result = (no_of_day - number_of_leap_year) / 365;

        int customer_age = Convert.ToInt32(Math.Floor(result));

        return customer_age;

    }

    //Get Leap Year Count
    public static int Get_Number_Of_Leap_Year(int dob_year, int this_year)
    {

        int number_of_year = 0;
        int i = dob_year;


        while ((i <= this_year))
        {
            if (((i % 4 == 0) && (i % 100 != 0) || (i % 400 == 0)))
            {
                number_of_year += 1;
            }

            i += 1;
        }

        return number_of_year;
    }

    //Add new row to UW Life Product table
    public static bool AddUWLifeProductRecord(string app_register_id, string product_id, int age_insure, int pay_year, int pay_up_to_age, int assure_year, int assure_up_to_age, double user_sum_insure, double system_sum_insure, double user_premium, double system_premium, double system_premium_discount, int pay_mode, DateTime created_on, string created_by, string created_note, double original_amount, double rounded_amount, double user_premium_discount, string dob, int gender, int coverage_period)
    {
        bool AddRecord = false;

        //Get age insure by effective date
        int my_age_insure = GetCustomerAge(dob, created_on.ToString());

       

        //If age increase, get new premium calculation based on new age
        if (my_age_insure > age_insure)
        {
            bl_product product = da_product.GetProductByProductID(product_id);
            string premium = "0,0";

            switch (product.Plan_Block)
            {
                case "A": //Whole Life Old
                    premium = Calculation.CalculatePremiumWholeLife(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode);
                    break;
                case "M": //MRTA descreasing
                    premium = Calculation.CalculatePremiumMRTA(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), coverage_period, gender, pay_mode);
                    break;

                case "T": //Term Life Old
                    premium = Calculation.CalculatePremiumTermLife(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode);
                    break;

                case "001": //Whole Life New (CAM Whole)
                case "002": //Term Life New (CAM Protection)
                case "010": //Savings PP200          

                    //calculate with product_id, age, SA, gender, pay mode and no factor discount (call it type 1)
                    premium = Calculation.CalculatePremiumTypeOne(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode, product.Plan_Block);

                    break;

                case "006": //MRTA 12
                case "007": //MRTA 24
                case "008": //MRTA 36
                    premium = Calculation.CalculatePremiumTypeTwo(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), coverage_period, gender, pay_mode, product.Plan_Block);
                    break;
                case "004": //Premium Payback 15/10
                    //calculate with product_id, age, SA, gender, pay mode and no factor discount (call it type 3)
                    premium = Calculation.CalculatePremiumTypeThree(product_id, my_age_insure, Convert.ToInt32(user_sum_insure), gender, pay_mode);
                    break;
                default:
                    break;
            }

            //Premium returns 1. premium by pay mode which is already rounded 2. original premium separated by (,)
            original_amount = Convert.ToDouble(premium.Split(',')[1]);
            rounded_amount = Math.Ceiling(original_amount);

            //Assign the answer to premium before passing to db table
            //user_premium = Convert.ToDouble(premium.Split(',')[0]);
            system_premium = Convert.ToDouble(premium.Split(',')[0]);

            age_insure = my_age_insure;

            //Get new assure year for whole life product 
            if (product.Product_ID == "W10" || product.Product_ID == "W15" || product.Product_ID == "W20" || product.Product_ID == "W9010" || product.Product_ID == "W9015" || product.Product_ID == "W9020")
            {
                assure_year = GetAssureYear(product_id, my_age_insure.ToString()); 
            }

            //other product, assure year remains the same
            assure_up_to_age = my_age_insure + assure_year;
            pay_up_to_age = my_age_insure + pay_year;

        }


        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UW_Life_Product_Record";

            //get new primary key for the row to be inserted
            string new_guid = Helper.GetNewGuid("SP_Check_UW_Life_Product_ID", "@UW_Life_Product_ID").ToString();

            //bind parameter
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", new_guid);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Seq_Number", 0);
            cmd.Parameters.AddWithValue("@Product_ID", product_id);
            cmd.Parameters.AddWithValue("@Age_Insure", age_insure);
            cmd.Parameters.AddWithValue("@Pay_Year", pay_year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", pay_up_to_age);
            cmd.Parameters.AddWithValue("@Assure_Year", assure_year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", assure_up_to_age);
            cmd.Parameters.AddWithValue("@User_Sum_Insure", user_sum_insure);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", system_sum_insure);
            cmd.Parameters.AddWithValue("@User_Premium", user_premium);
            cmd.Parameters.AddWithValue("@System_Premium", system_premium);
            cmd.Parameters.AddWithValue("@System_Premium_Discount", system_premium_discount);
            cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);
            cmd.Parameters.AddWithValue("@Created_On", created_on);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@created_Note", created_note);

            cmd.Parameters.AddWithValue("@Original_Amount", original_amount);
            cmd.Parameters.AddWithValue("@Rounded_Amount", rounded_amount);
            cmd.Parameters.AddWithValue("@User_Premium_Discount", user_premium_discount);  

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWLifeProduct] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }

    //Get number of assure year by product ID and customer ID
    public static int GetAssureYear(string product_id, string customer_age)
    {
        int assure_year = 0;

        assure_year = da_product.GetAssureYearByProductID(product_id);

        //Get Product information
        bl_product product = new bl_product();
        product = da_product.GetProductByProductID(product_id);

        switch (product.Plan_Block)
        {

            case "001"://New Whole Life
            case "A": //Old Whole Life
                assure_year = assure_year - Convert.ToInt32(customer_age); // 90 - Age_Insure

                break;            
        }

        return assure_year;
    }


    //Update UW Life product record (not using yet)
    public static bool UpdateUWLifeProductRecord(string app_register_id, string product_id, int age_insure, int pay_year, int pay_up_to_age, int assure_year, int assure_up_to_age, double user_sum_insure, double system_sum_insure, double user_premium, double system_premium, double system_premium_discount, int pay_mode, DateTime created_on, string created_by, string created_note, double original_amount, double rounded_amount, double user_premium_discount)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UW_Life_Product_Record";

            //get new primary key for the row to be inserted
            string new_guid = Helper.GetNewGuid("SP_Check_UW_Life_Product_ID", "@UW_Life_Product_ID").ToString();

            //bind parameter
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", new_guid);

            cmd.Parameters.AddWithValue("@System_Premium", system_premium);

            cmd.Parameters.AddWithValue("@Original_Amount", original_amount);
            cmd.Parameters.AddWithValue("@Rounded_Amount", rounded_amount);

            cmd.Parameters.AddWithValue("@User_Premium_Discount", user_premium_discount);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWLifeProduct] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }

    //Get life product ID by App_Register_ID
    public static string GetUWLifeProductID(string app_register_id)
    {
        string uw_life_product_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_Life_Product_ID_By_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        uw_life_product_id = rdr.GetString(rdr.GetOrdinal("UW_Life_Product_ID"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUWLifeProductID] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return uw_life_product_id;
    }

    //TODO: Get life product ID by App_Register_ID and Level for rider
    public static string GetUWLifeProductID(string app_register_id, int level)
    {
        string uw_life_product_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_Life_Product_ID_By_App_Register_ID_Level";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    uw_life_product_id = rdr["UW_Life_Product_ID"].ToString();
                }
               
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUWLifeProductID (" + app_register_id + ", " + level + ")] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return uw_life_product_id;
    }
    //Add new row to table UW Life Prod Cancel
    public static bool AddUWLifeProductCancel(string app_register_id, string created_by, string created_note)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UW_Life_Product_Cancel";          

            //bind parameter            
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", GetUWLifeProductID(app_register_id));           
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@created_Note", created_note);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWLifeProductCancel] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }


    //Add new row to table UW CO
    public static bool AddUWCO(string app_register_id, double system_sum_insure, double system_premium, double em_percent, double em_rate, double em_premium, double em_amount, double ef_rate, double ef_premium, double total_ef_year, string created_by, string created_note, string round_status_id ="")
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UW_CO_Record";

            //bind parameter            
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", GetUWLifeProductID(app_register_id));
            cmd.Parameters.AddWithValue("@CO_Doc_Number", GetCODocNumber());
            cmd.Parameters.AddWithValue("@Requested_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Doc_Schedule", 0);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", system_sum_insure);
            cmd.Parameters.AddWithValue("@System_Premium", system_premium);
            cmd.Parameters.AddWithValue("@EM_Percent", em_percent);
            cmd.Parameters.AddWithValue("@EM_Premium", em_premium);
            cmd.Parameters.AddWithValue("@EM_Amount", em_amount);
            cmd.Parameters.AddWithValue("@EM_Rate", em_rate);
            cmd.Parameters.AddWithValue("@EF_Rate", ef_rate);
            cmd.Parameters.AddWithValue("@EF_Premium", ef_premium);
            cmd.Parameters.AddWithValue("@Total_EF_Year", total_ef_year);
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@created_Note", created_note);
            cmd.Parameters.AddWithValue("@Round_Status_ID", round_status_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWCO] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }
    //TODO: Add new row to table UW CO with new parameter uw_life_product_id
    public static bool AddUWCO(string uw_life_product_id, string app_register_id, double system_sum_insure, double system_premium, double em_percent, double em_rate, double em_premium, double em_amount, double ef_rate, double ef_premium, double total_ef_year, string created_by, string created_note, string round_status_id = "")
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UW_CO_Record";

            //bind parameter            
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);
            cmd.Parameters.AddWithValue("@CO_Doc_Number", GetCODocNumber());
            cmd.Parameters.AddWithValue("@Requested_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Doc_Schedule", 0);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", system_sum_insure);
            cmd.Parameters.AddWithValue("@System_Premium", system_premium);
            cmd.Parameters.AddWithValue("@EM_Percent", em_percent);
            cmd.Parameters.AddWithValue("@EM_Premium", em_premium);
            cmd.Parameters.AddWithValue("@EM_Amount", em_amount);
            cmd.Parameters.AddWithValue("@EM_Rate", em_rate);
            cmd.Parameters.AddWithValue("@EF_Rate", ef_rate);
            cmd.Parameters.AddWithValue("@EF_Premium", ef_premium);
            cmd.Parameters.AddWithValue("@Total_EF_Year", total_ef_year);
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@created_Note", created_note);
            cmd.Parameters.AddWithValue("@Round_Status_ID", round_status_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWCO] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }

    //Add new row to table UW CO
    public static bool UpdateUWCO(bl_underwriting_co my_uw_co)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Update_UW_CO_Record";

            //bind parameter            
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", my_uw_co.UW_Life_Product_ID);            
            cmd.Parameters.AddWithValue("@EM_Premium", my_uw_co.EM_Premium);
            cmd.Parameters.AddWithValue("@EM_Amount", my_uw_co.EM_Amount);
            cmd.Parameters.AddWithValue("@EM_Rate", my_uw_co.EM_Rate);            


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateUWCO] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }

    //Update row table UW CO
    public static bool UpdateUWCORider(bl_underwriting_co my_uw_co)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Update_UW_CO_Rider_Record";

            //bind parameter            
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", my_uw_co.UW_Life_Product_ID);
            cmd.Parameters.AddWithValue("@EM_Premium", my_uw_co.EM_Premium);
            cmd.Parameters.AddWithValue("@EM_Amount", my_uw_co.EM_Amount);
            cmd.Parameters.AddWithValue("@EM_Rate", my_uw_co.EM_Rate);
            cmd.Parameters.AddWithValue("@EM_Percent", my_uw_co.EM_Percent);


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateUWCORider] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }

    //Get payment mode by paymode ID
    public static string GetPaymentMode(string pay_mode_id)
    {
        string payment_mode = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Payment_Mode_By_Pay_Mode_ID";

            cmd.Parameters.AddWithValue("@PayModeID", pay_mode_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        payment_mode = rdr.GetString(rdr.GetOrdinal("Mode"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPaymentMode] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return payment_mode;
    }


    //Get extra premium amount from table uw_co by app_register_id
    public static string GetEMAmount(string app_register_id)
    {
        double extra_amount = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_EM_Amount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        extra_amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetEMAmount] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return extra_amount.ToString();
    }


    //Get discount by app_register_id
    public static string GetDiscount(string app_register_id)
    {
        double discount = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Discount_By_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        discount = rdr.GetDouble(rdr.GetOrdinal("Discount_Amount"));
                       
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetDiscount] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return discount.ToString();
    }


    //Get UW CO object
    public static bl_underwriting_co GetUnderwritingCO(string app_register_id, string product_id, string gender, string customer_age)
    {
        bl_underwriting_co my_uw_co = new bl_underwriting_co();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_By_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_uw_co.UW_Life_Product_ID = rdr.GetString(rdr.GetOrdinal("UW_Life_Product_ID"));
                        my_uw_co.CO_Doc_Number = rdr.GetString(rdr.GetOrdinal("CO_Doc_Number"));
                        my_uw_co.Requested_On = rdr.GetDateTime(rdr.GetOrdinal("Requested_On"));
                        my_uw_co.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                        my_uw_co.System_Premium = rdr.GetDouble(rdr.GetOrdinal("System_Premium"));
                        my_uw_co.EM_Percent = rdr.GetDouble(rdr.GetOrdinal("EM_Percent"));
                        my_uw_co.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        my_uw_co.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                        my_uw_co.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                        my_uw_co.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                        my_uw_co.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));                       
                        my_uw_co.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));

                        my_uw_co.EM_Rate = GetEMRate(product_id, Convert.ToInt32(gender), Convert.ToInt32(customer_age), Convert.ToDouble(my_uw_co.EM_Percent), rdr.GetInt32(rdr.GetOrdinal("Pay_Mode")));
                        
                        my_uw_co.Benefit_Note = rdr.GetString(rdr.GetOrdinal("CO_Note"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUnderwritingCO] in class [da_underwriting]. Details: " + ex.Message);
            }

        }

        return my_uw_co;
    }


    //New function to get CO object
    public static bl_underwriting_co GetUnderwritingCOByParams(string app_register_id, string product_id, string gender, string customer_age, string assure_year)
    {
        bl_underwriting_co my_uw_co = new bl_underwriting_co();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_By_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_uw_co.UW_Life_Product_ID = rdr.GetString(rdr.GetOrdinal("UW_Life_Product_ID"));
                        my_uw_co.CO_Doc_Number = rdr.GetString(rdr.GetOrdinal("CO_Doc_Number"));
                        my_uw_co.Requested_On = rdr.GetDateTime(rdr.GetOrdinal("Requested_On"));
                        my_uw_co.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                        my_uw_co.System_Premium = rdr.GetDouble(rdr.GetOrdinal("System_Premium"));
                        my_uw_co.EM_Percent = rdr.GetDouble(rdr.GetOrdinal("EM_Percent"));
                        my_uw_co.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        my_uw_co.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                        my_uw_co.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                        my_uw_co.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                        my_uw_co.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));
                        my_uw_co.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));

                        my_uw_co.EM_Rate = rdr.GetDouble(rdr.GetOrdinal("EM_Rate"));

                        my_uw_co.Benefit_Note = rdr.GetString(rdr.GetOrdinal("CO_Note"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUnderwritingCO] in class [da_underwriting]. Details: " + ex.Message);
            }

        }

        return my_uw_co;
    }

    //TODO : New function to get CO Rider
    public static bl_underwriting_co GetCORiderByAppID_Level(string app_register_id, int level)
    {
        bl_underwriting_co my_uw_co = new bl_underwriting_co();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (level == 12)//tpd
            {
                //call store procedure by name
                cmd.CommandText = "SP_Get_UW_CO_Rider_TPD_By_App_Register_ID_Level";

                cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd.Parameters.AddWithValue("@Level", level);
            }
            else
            {
                //call store procedure by name
                cmd.CommandText = "SP_Get_UW_CO_Rider_By_App_Register_ID_Level";

                cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd.Parameters.AddWithValue("@Level", level);
            }
            

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_uw_co.UW_Life_Product_ID = rdr.GetString(rdr.GetOrdinal("UW_Life_Product_ID"));
                        my_uw_co.CO_Doc_Number = rdr.GetString(rdr.GetOrdinal("CO_Doc_Number"));
                        my_uw_co.Requested_On = rdr.GetDateTime(rdr.GetOrdinal("Requested_On"));
                        my_uw_co.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                        my_uw_co.System_Premium = rdr.GetDouble(rdr.GetOrdinal("System_Premium"));
                        my_uw_co.EM_Percent = rdr.GetDouble(rdr.GetOrdinal("EM_Percent"));
                        my_uw_co.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        my_uw_co.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                        my_uw_co.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                        my_uw_co.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                        my_uw_co.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));
                        my_uw_co.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));

                        my_uw_co.EM_Rate = rdr.GetDouble(rdr.GetOrdinal("EM_Rate"));

                        my_uw_co.Benefit_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetCORiderByAppID_Level] in class [da_underwriting]. Details: " + ex.Message);
            }

        }

        return my_uw_co;
    }
    //Get UW CO object by passing only app_register_id
    public static bl_underwriting_co GetUnderwritingCOByAppID(string app_register_id)
    {
        bl_underwriting_co my_uw_co = new bl_underwriting_co();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_By_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_uw_co.UW_Life_Product_ID = rdr.GetString(rdr.GetOrdinal("UW_Life_Product_ID"));
                        my_uw_co.CO_Doc_Number = rdr.GetString(rdr.GetOrdinal("CO_Doc_Number"));
                        my_uw_co.Requested_On = rdr.GetDateTime(rdr.GetOrdinal("Requested_On"));
                        my_uw_co.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                        my_uw_co.System_Premium = rdr.GetDouble(rdr.GetOrdinal("System_Premium"));
                        my_uw_co.EM_Percent = rdr.GetDouble(rdr.GetOrdinal("EM_Percent"));
                        my_uw_co.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        my_uw_co.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                        my_uw_co.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                        my_uw_co.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                        my_uw_co.Created_Note = rdr.GetString(rdr.GetOrdinal("Created_Note"));
                        my_uw_co.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));
                        my_uw_co.Original_Amount = rdr.GetDouble(rdr.GetOrdinal("Original_Amount"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUnderwritingCOByAppID] in class [da_underwriting]. Details: " + ex.Message);
            }

        }

        return my_uw_co;
    }


    //Get EM_Premium (total annual extra premium) and EM_Amount (extra premium based on paymode) from uw_co table
    public static bl_underwriting_co GetUWCOSingleRow(string app_register_id)
    {        
        bl_underwriting_co my_co = new bl_underwriting_co();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_Single_Row";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_co.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        my_co.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUWCOSingleRow] in class [da_underwriting]. Details: " + ex.Message);
            }

        }

        return my_co;
    }

    //Get extra premium in amount digit
    public static double GetUWCOExtraAmount(string app_register_id)
    {       
        double my_extra_amount = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_Extra_Amount";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_extra_amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));                        
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUWCOExtraAmount] in class [da_underwriting]. Details: " + ex.Message);
            }

        }

        return my_extra_amount;
    }


    //Delete rows from table underwriting, uw_life_product, uw_co
    public static void UndoUnderwriting(string app_register_id)
    {
        string uw_life_product_id = GetUWLifeProductID(app_register_id);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            try
            {
                //Get uw_life_product_id from ct_uw_life_product table                
                //Check and if exist, delete row in table ct_uw_co
                //Delete row in table uw_life_product
                //Delete row in table underwriting
                //Refresh gridview

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                //Delete record in table UW_CO
                cmd.CommandText = "SP_Delete_UW_CO";
                cmd.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();

                //Delete record in table UW_Life_Prod_Cancel SP_Delete_UW_Life_Product_Cancel
                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "SP_Delete_UW_Life_Product_Cancel";
                cmd3.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);
                cmd3.Connection = con;
                cmd3.ExecuteNonQuery();

                //Delete record in table UW_Life_Product
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_Delete_UW_Life_Product";
                cmd1.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);
                cmd1.Connection = con;
                cmd1.ExecuteNonQuery();

                //TODO:04012017 Delete record in table Ct_UW_Effective_Date by app_register_id
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "SP_Delete_UW_Effective_Date";
                cmd2.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd2.Connection = con;
                cmd2.ExecuteNonQuery();
                cmd2.Parameters.Clear();
                cmd2.Dispose();
                //con.Close();

                //Delete record in table Underwriting
                cmd2 = new SqlCommand();
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "SP_Delete_Underwriting";
                cmd2.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd2.Connection = con;
                cmd2.ExecuteNonQuery();
                //con.Close();

                //TODO: 22092016 Delete record in talbe CO base on app_register_id
                cmd2 = new SqlCommand();
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "SP_Delete_UW_CO_BY_App_ID";
                cmd2.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd2.Connection = con;
                cmd2.ExecuteNonQuery();
                cmd2.Parameters.Clear();
                cmd2.Dispose();
                con.Close();
           
            }
           
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UndoUnderwriting] in class [da_underwriting]. Details: " + ex.Message);
            }
        }

    }
    //TODO: 22092016 Delete rows from table underwriting rider, uw_life_product, uw_co
    public static void UndoUnderwriting(string app_register_id, int level)
    {
        string uw_life_product_id = GetUWLifeProductID(app_register_id, level);

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                //Delete record in table UW_CO
                cmd.CommandText = "SP_Delete_UW_CO";
                cmd.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();

                //Delete record in table UW_Life_Prod_Cancel SP_Delete_UW_Life_Product_Cancel
                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "SP_Delete_UW_Life_Product_Cancel";
                cmd3.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);
                cmd3.Connection = con;
                cmd3.ExecuteNonQuery();

                //Delete record in table UW_Life_Product
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_Delete_UW_Life_Product";
                cmd1.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);
                cmd1.Connection = con;
                cmd1.ExecuteNonQuery();

                //Delete record in table Ct_UW_Rider_Effective_Date
                cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_Delete_UW_Rider_Effective_Date_By_AppID_Level";
                cmd1.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd1.Parameters.AddWithValue("@Level", level);
                cmd1.Connection = con;
                cmd1.ExecuteNonQuery();

                //TODO: Delete record in talbe underwriting rider 22092016 
                da_underwriting.DeleteUnderwritingRiderByAppID(app_register_id, level);
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UndoUnderwriting] in class [da_underwriting]. Details: " + ex.Message);
            }
        }

    }

    public static void UndoUnderwritingAll(string app_register_id)
    {

        string connString = AppConfiguration.GetConnectionString();
       
            try
            {

                Helper.ExecuteProcedure(connString, "SP_Delete_UW_ALL", new string[,] { { "@app_id", app_register_id } }, "da_underwriting => UndoUnderwritingAll");

            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UndoUnderwritingAll] in class [da_underwriting]. Details: " + ex.Message);
            }
        

    }


    //Add new row to table UW Effective Date
    public static bool AddUWEffectiveDate(string app_register_id, DateTime effective_date, string created_by)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UW_Effective_Date";

            //bind parameter            
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Effective_Date", effective_date);
            cmd.Parameters.AddWithValue("@Is_Standard", 1);
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWEffectiveDate] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }


    //Get data from view: basic underwriting
    public static bl_underwriting GetUnderwritingObject(string app_register_id)
    {
        bl_underwriting my_UW_view = new bl_underwriting();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_Details_By_App_Register_ID";

            //bind parameter            
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);            

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_UW_view.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                        my_UW_view.App_Date = rdr.GetDateTime(rdr.GetOrdinal("App_Date"));

                        //Application
                        my_UW_view.App_Number = rdr.GetString(rdr.GetOrdinal("App_Number"));
                        my_UW_view.Created_By = rdr.GetString(rdr.GetOrdinal("Created_By"));
                        my_UW_view.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));
                        my_UW_view.Sale_Agent_Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                        my_UW_view.Office_ID = rdr.GetString(rdr.GetOrdinal("Office_ID"));
                        my_UW_view.Benefit_Note = rdr.GetString(rdr.GetOrdinal("Benefit_Note"));
                        //Customer info
                        my_UW_view.First_Name = rdr.GetString(rdr.GetOrdinal("Khmer_First_Name")).ToUpper();
                        my_UW_view.Last_Name = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name")).ToUpper();
                        my_UW_view.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                        my_UW_view.Nationality = rdr.GetString(rdr.GetOrdinal("Country_ID"));
                        my_UW_view.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        my_UW_view.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));

                        my_UW_view.Pay_Up_To_Age = rdr.GetInt32(rdr.GetOrdinal("Pay_Up_To_Age"));
                        my_UW_view.Assure_Up_To_Age = rdr.GetInt32(rdr.GetOrdinal("Assure_Up_To_Age"));

                        //maneth
                        //if ((rdr.GetInt32(rdr.GetOrdinal("Gender")) == 1))
                        //    my_UW_view.Gender = "ប្រុស";
                        //else
                        //    my_UW_view.Gender = "ស្រី";

                        my_UW_view.Gender = rdr.GetString(rdr.GetOrdinal("Gender"));
                        
                        my_UW_view.Father_First_Name = rdr.GetString(rdr.GetOrdinal("Father_First_Name")).ToUpper();
                        my_UW_view.Father_Last_Name = rdr.GetString(rdr.GetOrdinal("Father_Last_Name")).ToUpper();
                        my_UW_view.Mother_First_Name = rdr.GetString(rdr.GetOrdinal("Mother_First_Name")).ToUpper();
                        my_UW_view.Mother_Last_Name = rdr.GetString(rdr.GetOrdinal("Mother_Last_Name")).ToUpper();

                        //Contact
                        my_UW_view.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                        my_UW_view.Address2 = rdr.GetString(rdr.GetOrdinal("Address2"));
                        my_UW_view.Address3 = rdr.GetString(rdr.GetOrdinal("Address3"));
                        my_UW_view.Province = rdr.GetString(rdr.GetOrdinal("Province"));
                        my_UW_view.Mobile_Phone1 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone1"));
                        my_UW_view.EMail = rdr.GetString(rdr.GetOrdinal("EMail"));

                        //Product
                        my_UW_view.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                        if(!DBNull.Value.Equals(rdr["Kh_Title"]))
                        {
                             my_UW_view.Kh_Title = rdr.GetString(rdr.GetOrdinal("Kh_Title"));
                        }
                        else
                        {
                             my_UW_view.Kh_Title = "";
                        }
                        if (!DBNull.Value.Equals(rdr["en_title"]))
                        {
                            my_UW_view.En_Title = rdr.GetString(rdr.GetOrdinal("En_Title"));
                        }
                        else
                        {
                            my_UW_view.En_Title = "";
                        }
                        
                        my_UW_view.Assure_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                        my_UW_view.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));
                        my_UW_view.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                        my_UW_view.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));
                        my_UW_view.Payment_Mode = GetPaymentMode(my_UW_view.Pay_Mode.ToString());
                        my_UW_view.System_Premium = rdr.GetDouble(rdr.GetOrdinal("System_Premium"));

                        
                        //To be edited
                        my_UW_view.Original_Amount = rdr.GetDouble(rdr.GetOrdinal("Original_Amount"));
                        my_UW_view.Rounded_Amount = Math.Ceiling(my_UW_view.Original_Amount);


                        my_UW_view.User_Premium_Discount = rdr.GetDouble(rdr.GetOrdinal("User_Premium_Discount"));

                        //Get EM_Premium (Total extra premium) and EM_Amount (Extra premium based on payment method)
                        bl_underwriting_co my_co =  GetUWCOSingleRow(my_UW_view.App_Register_ID);

                        my_UW_view.Extra_Premium = Math.Ceiling(my_co.EM_Premium);
                        my_UW_view.Extra_Amount = my_co.EM_Amount;

                        //Total yearly premium = Total yearly premium + total yearly extra premium
                        my_UW_view.Total_Yearly_Premium = (my_UW_view.Rounded_Amount + my_UW_view.Extra_Premium);

                        //Total premium = premium based on paymode + extra premium based on paymode
                        my_UW_view.Total_Premium = (my_UW_view.System_Premium + my_UW_view.Extra_Amount) - my_UW_view.User_Premium_Discount;
                        
                        my_UW_view.TPD = 0;

                        my_UW_view.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                        dtfi.ShortDatePattern = "dd/MM/yyyy";
                        dtfi.DateSeparator = "/";

                        my_UW_view.Maturity_Date = Convert.ToDateTime(my_UW_view.Effective_Date.Day.ToString() + "/" + my_UW_view.Effective_Date.Month.ToString() + "/" + Convert.ToString(my_UW_view.Effective_Date.Year + my_UW_view.Assure_Year), dtfi);


                        my_UW_view.Premium_Payment_Due_Date = my_UW_view.Effective_Date;
                        my_UW_view.Due_Day_Month = my_UW_view.Premium_Payment_Due_Date.Day + " " + GetMonthName(my_UW_view.Premium_Payment_Due_Date.Month);

                       
                        switch (my_UW_view.Payment_Mode)
                        {
                            case "Annually":
                                my_UW_view.Complete_Due_Day_Month = my_UW_view.Premium_Payment_Due_Date.Day + " ខែ " + GetKhmerMonthName(my_UW_view.Premium_Payment_Due_Date.Month) + " រៀងរាល់ឆ្នាំ";
                                break;
                            case "Semi-Annual":
                                my_UW_view.Complete_Due_Day_Month = my_UW_view.Premium_Payment_Due_Date.Day + " ខែ " + GetKhmerMonthName(my_UW_view.Premium_Payment_Due_Date.Month) + ", " + my_UW_view.Premium_Payment_Due_Date.Day + " ខែ " + GetKhmerMonthName(my_UW_view.Premium_Payment_Due_Date.Month + 6);
                                break;
                            case "Quarterly":
                                my_UW_view.Complete_Due_Day_Month = my_UW_view.Premium_Payment_Due_Date.Day + " ខែ " + GetKhmerMonthName(my_UW_view.Premium_Payment_Due_Date.Month) + ", " + my_UW_view.Premium_Payment_Due_Date.Day + " ខែ " + GetKhmerMonthName(my_UW_view.Premium_Payment_Due_Date.Month + 3) + ", " + my_UW_view.Premium_Payment_Due_Date.Day + "  ខែ " + GetKhmerMonthName(my_UW_view.Premium_Payment_Due_Date.Month + 6) + ", " + my_UW_view.Premium_Payment_Due_Date.Day + " ខែ " + GetKhmerMonthName(my_UW_view.Premium_Payment_Due_Date.Month + 9);
                                break;
                            case "Monthly":
                                my_UW_view.Complete_Due_Day_Month = "ថ្ងៃទី " + my_UW_view.Premium_Payment_Due_Date.Day + " នៃខែនីមួយៗ";
                                break;
                            case "Single":
                                my_UW_view.Complete_Due_Day_Month = "-";
                                break;
                            default:
                                my_UW_view.Complete_Due_Day_Month = "N/A";
                                break;
                        }

                       

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUnderwritingView] in class [da_underwriting]. Details: " + ex.Message);
            }

        }


        return my_UW_view;
    }


    //Get name of the month
    public static string GetMonthName(int month)
    {
        string month_name = "";

        switch (month)
        {
            case 1:
                month_name = "Jan";
                break;
            case 2:
                month_name = "Feb";
                break;
            case 3:
                month_name = "Mar";
                break;
            case 4:
                month_name = "Apr";
                break;
            case 5:
                month_name = "May";
                break;
            case 6:
                month_name = "Jun";
                break;
            case 7:
                month_name = "Jul";
                break;
            case 8:
                month_name = "Aug";
                break;
            case 9:
                month_name = "Sep";
                break;
            case 10:
                month_name = "Oct";
                break;
            case 11:
                month_name = "Nov";
                break;
            default:
                month_name = "Dec";
                break;

        }

        return month_name;

    }


    //Get khmer month name
    public static string GetKhmerMonthName(int month)
    {
        string month_name = "";

        switch (month)
        {
                // Just edit by Sok Thavy on 19-Nov-2015
            case 13:
                month = 1;
                break;
            case 14:
                month = 2;
                break;
            case 15:
                month = 3;
                break;
            case 16:
                month = 4;
                break;
            case 17:
                month = 5;
                break;
            case 18:
                month = 6;
                break;
            case 19:
                month = 7;
                break;
            case 20:
                month = 8;
                break;
            case 21:
                month = 9;
                break;
            case 22:
                month = 10;
                break;
            case 23:
                month = 11;
                break;
            case 24:
                month = 12;
                break;
        }


        switch (month)
        {
            case 1:
                month_name = "មករា";
                break;
            case 2:
                month_name = "កុម្ភៈ";
                break;
            case 3:
                month_name = "មីនា";
                break;
            case 4:
                month_name = "មេសា";
                break;
            case 5:
                month_name = "ឧសភា";
                break;
            case 6:
                month_name = "មិថុនា";
                break;
            case 7:
                month_name = "កក្កដា";
                break;
            case 8:
                month_name = "សីហា";
                break;
            case 9:
                month_name = "កញ្ញា";
                break;
            case 10:
                month_name = "តុលា";
                break;
            case 11:
                month_name = "វិច្ឆិកា";
                break;
            default:
                month_name = "ធ្នូ";
                break;

        }

        return month_name;

    }

    //Get List of Benefit_Items by app_id
    public static List<bl_app_benefit_item> GetAppBenefitItem(string app_id)
    {

        List<bl_app_benefit_item> benefit_items = new List<bl_app_benefit_item>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand("SP_Get_App_Benefit_Item_By_App_Register_ID_Khmer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@App_Register_ID";
            paramName.Value = app_id;
            cmd.Parameters.Add(paramName);        
        
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_app_benefit_item benefit_item = new bl_app_benefit_item();

                    benefit_item.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                    benefit_item.App_Benefit_Item_ID = rdr.GetString(rdr.GetOrdinal("App_Benefit_Item_ID"));
                    benefit_item.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                    benefit_item.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    benefit_item.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                    benefit_item.Percentage = rdr.GetDouble(rdr.GetOrdinal("Percentage"));                    
                    benefit_item.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship_Khmer"));
                    benefit_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));

                    benefit_items.Add(benefit_item);
                }
            }
            con.Close();
        }
        return benefit_items;
    }



    //Get factor by pay_mode_id
    public static double GetPaymentFactor(int pay_mode_id)
    {
        double payment_factor = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Payment_Mode_By_Pay_Mode_ID";

            cmd.Parameters.AddWithValue("@PayModeID", pay_mode_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        payment_factor = rdr.GetDouble(rdr.GetOrdinal("Factor"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPaymentFactor] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return payment_factor;
    }

    #endregion

    #region Family Protection
    //Check underwriting Rider
    public static bool CheckUnderwritingRider(string app_register_id, int level)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Check_Underwriting_Rider";

            //bind parameter
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [CheckUnderwritingRider] in class [da_underwriting]. Details: " + ex.Message);
            }
            con.Close();
        }
        return result;
    }
    //Check underwriting Rider by app id
    public static bool CheckUnderwritingRider(string app_register_id)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Check_Underwriting_Rider_By_App_ID";

            //bind parameter
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [CheckUnderwritingRider] in class [da_underwriting]. Details: " + ex.Message);
            }
            con.Close();
        }
        return result;
    }
    //Add new row underwriting Rider table
    public static bool AddUnderwritingRecordRider(string app_register_id, int level, int result, string status_code, string updated_by, string updated_note, DateTime updated_on)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name            
            cmd.CommandText = "SP_Insert_Underwriting_Rider_Record";
            string uw_rider_id = "";
            uw_rider_id = Helper.GetNewGuid("SP_Check_UW_Rider_ID", "@UW_Rider_ID");
            //bind parameter
            cmd.Parameters.AddWithValue("@UW_Rider_ID", uw_rider_id);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Parameters.AddWithValue("@Underwrite_Doc_Number", GetUWDocNumber());
            cmd.Parameters.AddWithValue("@Is_Clean_Case", 0);
            cmd.Parameters.AddWithValue("@Result", result);
            cmd.Parameters.AddWithValue("@Status_Code", status_code);
            cmd.Parameters.AddWithValue("@Updated_On", updated_on);
            cmd.Parameters.AddWithValue("@Updated_By", updated_by);
            cmd.Parameters.AddWithValue("@Updated_Note", updated_note);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUnderwritingRecordRider] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }
    public static bool DeleteUnderwritingRiderByAppID(string app_register_id , int level)
    {
        bool success = false;
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //call store procedure by name            
                cmd.CommandText = "SP_Delete_Underwriting_Rider_Record_By_App_ID";
                //bind parameter
                cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
                cmd.Parameters.AddWithValue("@Level", level);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();
                con.Close();
                success = true;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteUnderwritingRiderByAppID] in class [da_underwriting], Detail: " + ex.Message);
            success = false;
        }


        return success;
    }
    
    //Get status code for rider
    public static string GetRiderStatusCode(string app_register_id, int level)
    {
        string status_code = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Rider_Underwrite_Status_Code_by_App_Register_ID";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        status_code = rdr.GetString(rdr.GetOrdinal("Status_Code"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetRiderStatusCode] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return status_code;
    }
    //Get update status note for rider by app_register_id and level
    public static string GetRiderMemoStatusDetail(string app_register_id, int level)
    {
        string memo_detail = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Underwrite_Rider_Updated_Note_by_App_Register_ID_Level";

            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        memo_detail = rdr.GetString(rdr.GetOrdinal("Updated_Note"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetRiderMemoStatusDetail] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return memo_detail;
    }

    //Update underwriting rider record by app_register_id and level
    public static bool UpdateUnderwritingRiderRecord(string app_register_id, int level, int result, string status_code, string updated_by, string updated_note, DateTime updated_on)
    {
        bool result_query = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_UpdateUWRider_Status_Code_By_App_Register_ID_Level";

            //bind parameter
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Parameters.AddWithValue("@Result", result);
            cmd.Parameters.AddWithValue("@Status_Code", status_code);
            cmd.Parameters.AddWithValue("@Updated_By", updated_by);
            cmd.Parameters.AddWithValue("@Updated_On", updated_on); //Change to datetime.now after inserting data
            cmd.Parameters.AddWithValue("@Updated_Note", updated_note);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result_query = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateUnderwritingRiderRecord] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return result_query;
    }
   
    //Add new row to UW Life Product table for rider
    public static bool AddUWRiderLifeProductRecord(string app_register_id, int level, string product_id, int age_insure, int pay_year, int pay_up_to_age, int assure_year, int assure_up_to_age, double user_sum_insure, double system_sum_insure, double user_premium, double system_premium, double system_premium_discount, int pay_mode, DateTime created_on, string created_by, string created_note, double original_amount, double rounded_amount, double user_premium_discount, string dob, int gender, int coverage_period)
    {
        bool AddRecord = false;
        string sub_product = "";
        sub_product = product_id.Substring(0, 3).ToUpper().Trim();

        //Get age insure by effective date
        int my_age_insure = GetCustomerAge(dob, created_on.ToString());

        //If age increase, get new premium calculation based on new age
        if (my_age_insure > age_insure)
        {
            bl_product product = da_product.GetProductByProductID(product_id);
            double new_premium = 0.0;
            double new_original_amount = 0.0;

            //get plan assure from life product
            da_application_fp6.ProductFP6 life_product = new da_application_fp6.ProductFP6();
            List<da_application_fp6.ProductFP6> list_product = new List<da_application_fp6.ProductFP6>();
            list_product = da_application_fp6.ProductFP6.GetProductFP6List();
            int assure_plan = 0;

            for (int i = 0; i < list_product.Count; i++)
            {
                var life = list_product[i];
                if (product_id == life.ProductID)
                {
                    assure_plan = life.AssureYear;
                    break;
                }
            }
            int new_assure_age = 0;

            if (sub_product == "NFP")
            {
                #region Family protection
                //life insured 1
                if (level == 1)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 70) //max age = 70
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age-70);
                    }

                   

                    #region By maneth 29032017
                    //<>
                    //new_premium = da_application_fp6.getPremiumFP6(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6(user_sum_insure, product_id, gender, my_age_insure, assure_year);
                    double[,] arr_premium = da_application_fp6.getPremiumFP6(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year);
                    new_premium = arr_premium[0, 0];
                    new_original_amount = arr_premium[0, 1];
                    #endregion
                }
                //Spouse
                else if (level == 2)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 70)//max age = 70
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age-70);
                    }
                    #region by maneth 29032017
                    //<>

                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 0);
                    double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                    new_premium = arr_premium[0, 0];
                    new_original_amount = arr_premium[0, 1];
                    #endregion
                }
                //Kids 4
                else if (level > 2 && level < 5)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 21)//max age = 21
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age-21);
                    }
                   
                    #region by maneth 29032017
                    //<>

                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);
                    double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    new_premium = arr_premium[0, 0];
                    new_original_amount = arr_premium[0, 1];
                    #endregion
                }
                #region TPD
                else if (level == 12)//TPD
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65) //max age = 65
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - my_age_insure;
                    }

                    #region 
                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);

                    string str_premium = da_application_fp6.GetTPDPremium(user_sum_insure, product_id, gender, my_age_insure, assure_year, pay_mode);
                    string[] arr_premium = str_premium.Split('/');
                    new_premium = Convert.ToDouble(arr_premium[0].ToString());
                    new_original_amount = Convert.ToDouble(arr_premium[1].ToString());

                    #endregion
                }
                #endregion

                #region ADB
                else if (level == 13)//ADB
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65) //max age = 65
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - my_age_insure;
                    }

                    #region
                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);
                   

                    int rate = 0;
                    double class_rate = 0.0;

                    rate = da_application_fp6.GetADBRate(app_register_id);
                    class_rate = da_application_fp6.GetClassRate("Class " + rate);

                    string str_premium = da_application_fp6.GetADBPremium(user_sum_insure, product_id, class_rate, pay_mode);

                    string[] arr_premium = str_premium.Split('/');
                    new_premium = Convert.ToDouble(arr_premium[0].ToString());
                    new_original_amount = Convert.ToDouble(arr_premium[1].ToString());

                    #endregion
                }
                #endregion
                #endregion
            }

            else if (sub_product == "FPP")
            {
                #region Family protection package
                //life insured 1
                if (level == 1)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65) //max expired age = 65
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age - 65);
                    }
                    new_premium = da_application_fp6.GetPremiumFPPackage(product_id, assure_year, gender, my_age_insure);
                    new_original_amount = new_premium;
                }
                //Spouse
                else if (level == 2)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65)//max age = 65
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age - 65);
                    }

                    new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 0);
                    new_original_amount = new_premium;
                }
                //Kids 4
                else if (level > 2 && level < 5)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 21)//max age = 21
                    {
                        assure_year = assure_plan;
                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age - 21);
                    }

                    new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(product_id, gender, 0, assure_year, 1);
                    new_original_amount = new_premium;
                }
                //TPD level=12 & ADB level=13
                else if (level == 12)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65) //max age = 65
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age-65);
                    }

                    new_premium = da_application_fp6.GetTPDPremiumFPPackage(product_id, assure_year, gender, my_age_insure);
                    new_original_amount = new_premium;
                }
                else if (level == 13)
                {
                    new_assure_age = my_age_insure + assure_plan;
                    if (new_assure_age <= 65) //max age = 65
                    {
                        assure_year = assure_plan;

                    }
                    else
                    {
                        assure_year = assure_plan - (new_assure_age - 65);
                    }

                    //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                    //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);

                    string str_class = "";
                    str_class = "Class " + assure_plan + "/" + assure_plan;
                    new_premium = da_application_fp6.GetADBPremiumFPPackage(str_class);
                    new_original_amount = new_premium;
                }
                #endregion
               
            }

            #region Study Save
            else if (sub_product == "SDS")
            {
                string[] arr_product = product_id.Split('/');
                if (arr_product.Length == 2)
                {
                    #region Study save normal
                    if (level == 1)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max expiry age = 65
                        {
                            assure_year = assure_plan;
                        }
                        else
                        {
                            assure_year = assure_plan - (new_assure_age - 65);
                        }

                        #region By maneth 29032017
                        //<>
                        double[,] arr_premium = da_application_study_save.study_save.GetPremium(my_age_insure, gender, product_id, assure_year, pay_mode, user_sum_insure);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                        #endregion
                    }
                    //Spouse
                    else if (level == 2)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max expiry age = 65
                        {
                            assure_year = assure_plan;
                        }
                        else
                        {
                            assure_year = assure_plan - (new_assure_age - 65);
                        }
                        #region by maneth 29032017
                        //<>

                        double[,] arr_premium = da_application_study_save.study_save.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                        #endregion
                    }
                    //Kids 4
                    else if (level > 2 && level < 5)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 21)//max age = 21
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            assure_year = assure_plan - (new_assure_age - 21);
                        }

                        #region by maneth 29032017
                        //<>

                        double[,] arr_premium = da_application_study_save.study_save.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                        #endregion
                    }
                    #region TPD
                    else if (level == 12)//TPD
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max age = 65
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            assure_year = assure_plan - my_age_insure;
                        }

                        #region
                       
                        double[,] arr_premium = da_application_study_save.study_save.GetTPDPremium(my_age_insure, gender, product_id, assure_year, pay_mode, user_sum_insure);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];

                        #endregion
                    }
                    #endregion
                    #region ADB
                    else if (level == 13)//ADB
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max age = 65
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            assure_year = assure_plan - my_age_insure;
                        }

                        #region
                        //new_premium = da_application_fp6.getPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                        //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(user_sum_insure, product_id, gender, my_age_insure, assure_year, 1);


                        int rate = 0;
                        double class_rate = 0.0;

                        rate = da_application_fp6.GetADBRate(app_register_id);//is the ID
                        string format_class_name = "";
                        format_class_name = "Class " + rate + "/" + product_id;//format:Class 1/SDS10/10
                        class_rate = da_application_fp6.GetClassRate(format_class_name);//is the rate for calculate premium

                        double[,] arr_premium =da_application_study_save.study_save.GetADBPremium(product_id, pay_mode,user_sum_insure, class_rate);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];

                        #endregion
                    }
                    #endregion
                }
                    #endregion
                else if (arr_product.Length > 2)
                {
                    #region Study save package, package protection, maturity
                    if (level == 1)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max expiry age = 65
                        {
                            assure_year = assure_plan;
                        }
                        else
                        {
                            assure_year = assure_plan - (new_assure_age - 65);
                        }

                        #region By maneth 29032017
                        //<>
                       
                        //double[,] arr_premium = da_application_study_save.study_save_package.GetPremium(my_age_insure, gender, product_id, assure_year, pay_mode);
                        double[,] arr_premium = da_application_study_save.study_save_package.GetPremium(my_age_insure, gender, product_id, assure_plan, pay_mode);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                        #endregion
                    }
                    //Spouse
                    else if (level == 2)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max expiry age = 65
                        {
                            assure_year = assure_plan;
                        }
                        else
                        {
                            assure_year = assure_plan - (new_assure_age - 65);
                        }
                        #region by maneth 29032017
                        //<>

                        //double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 0);
                        double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_plan, 0);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                        #endregion
                    }
                    //Kids 4
                    else if (level > 2 && level < 5)
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 21)//max age = 21
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            assure_year = assure_plan - (new_assure_age - 21);
                        }

                        #region by maneth 29032017
                        //<>

                        //double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_year, 1);
                        double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(user_sum_insure, product_id, gender, my_age_insure, pay_mode, assure_plan, 1);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];
                        #endregion
                    }
                    #region TPD
                    else if (level == 12)//TPD
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max age = 65
                        {
                            assure_year = assure_plan;

                        }
                        else
                        {
                            assure_year = assure_plan - my_age_insure;
                        }

                        #region

                        double[,] arr_premium = da_application_study_save.study_save_package.GetTPDPremium(my_age_insure, gender, product_id, assure_year, pay_mode);
                        new_premium = arr_premium[0, 0];
                        new_original_amount = arr_premium[0, 1];

                        #endregion
                    }
                    #endregion

                    #region ADB
                    else if (level == 13)//ADB
                    {
                        new_assure_age = my_age_insure + assure_plan;
                        if (new_assure_age <= 65) //max age = 65
                        {
                            assure_year = assure_plan;
                        }
                        else
                        {
                            assure_year = assure_plan - my_age_insure;
                        }

                        #region
                       
                        string format_class_name = "";
                        format_class_name = "Class " + product_id; //Class Class SDSPK10/10/5300
                  
                        double[,] arr_premium = da_application_study_save.study_save_package.GetADBPremium(format_class_name, pay_mode);
                        new_premium = arr_premium[0,0];
                        new_original_amount = arr_premium[0, 1];

                        #endregion
                    }
                    #endregion
                }

#endregion
            }
            
#endregion

            //Premium returns 1. premium by pay mode which is already rounded 2. original premium separated by (,)
            original_amount = new_original_amount;
            rounded_amount = Math.Ceiling(original_amount);

            system_premium = new_premium;

            age_insure = my_age_insure;
          
            assure_up_to_age = my_age_insure + assure_year;
            pay_up_to_age = my_age_insure + assure_year;
            pay_year =  assure_year;


        }

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UWRider_Life_Product_Record";

            //get new primary key for the row to be inserted
            string new_guid = Helper.GetNewGuid("SP_Check_UW_Life_Product_ID", "@UW_Life_Product_ID").ToString();

            //bind parameter
            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", new_guid);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Parameters.AddWithValue("@Seq_Number", 0);
            cmd.Parameters.AddWithValue("@Product_ID", product_id);
            cmd.Parameters.AddWithValue("@Age_Insure", age_insure);
            cmd.Parameters.AddWithValue("@Pay_Year", pay_year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", pay_up_to_age);
            cmd.Parameters.AddWithValue("@Assure_Year", assure_year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", assure_up_to_age);
            cmd.Parameters.AddWithValue("@User_Sum_Insure", user_sum_insure);
            cmd.Parameters.AddWithValue("@System_Sum_Insure", system_sum_insure);
            cmd.Parameters.AddWithValue("@User_Premium", user_premium);
            cmd.Parameters.AddWithValue("@System_Premium", system_premium);
            cmd.Parameters.AddWithValue("@System_Premium_Discount", system_premium_discount);
            cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);
            cmd.Parameters.AddWithValue("@Created_On", created_on);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@created_Note", created_note);

            cmd.Parameters.AddWithValue("@Original_Amount", original_amount);
            cmd.Parameters.AddWithValue("@Rounded_Amount", rounded_amount);
            cmd.Parameters.AddWithValue("@User_Premium_Discount", user_premium_discount);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                AddRecord = false;
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWRiderLifeProductRecord] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }
    public static bool DeleteCORiderByAppIdAndLevel(string app_register_id, int level)
    {
        bool result = false;
       
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Delete_UW_CO_BY_App_ID_AND_Level";

            //bind parameter
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Level", level);
            

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                //Add error to log 
                Log.AddExceptionToLog("Error in function [DeleteCORiderByAppIdAndLevel] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        return result;
    }

    //TODO: Get ADB by Application_id 11102016
    public static double GetADB_TPD(string app_register_id, string rider_type)
    {
        double result = 0.0;
        string connString = AppConfiguration.GetConnectionString();
        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                //call store procedure by name
                cmd.CommandText = "SP_Get_ADB_TPD_Premium";

                cmd.Parameters.AddWithValue("@Rider_Type", rider_type);
                cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

                cmd.Connection = con;
                con.Open();


                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        result = Convert.ToDouble(rdr["premium"].ToString());

                    }
                    else
                    {
                        result = 0.0;
                    }
                }
            } 
           
        }
        catch (Exception ex)
        {
            result = 0.0;
            Log.AddExceptionToLog("Error function[GetADB_TPD] in class [da_underwriting], Detail: " + ex.Message);
        }

       
        return result;
    }

    //TODO: Save effective Date for rider 11102016
    public static bool AddUWRiderEffectiveDate(string app_register_id, DateTime effective_date, string created_by, int level)
    {
        bool AddRecord = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Insert_UW_Rider_Effective_Date";

            //bind parameter            
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Parameters.AddWithValue("@Effective_Date", effective_date);
            cmd.Parameters.AddWithValue("@Is_Standard", 1);
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@Level", level);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                AddRecord = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [AddUWRiderEffectiveDate] in class [da_underwriting]. Details: " + ex.Message);
            }

        }
        //return true if successful
        return AddRecord;
    }
            #endregion
    
}