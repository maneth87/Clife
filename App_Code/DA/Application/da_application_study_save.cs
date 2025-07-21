using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_application_study_save_package
/// </summary>
public class da_application_study_save
{
	public da_application_study_save()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    #region Public function for both individual and package study save


    #region Class Cash Value
    public class CashValue
    {
        public string Product_ID { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public int Assure_Year { get; set; }
        public int Policy_Year { get; set; }
        public double Cash_Value { get; set; }
    }
    #endregion

    #region Class ETPU
    public class ETPU
    {
        public string Product_ID { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public int Assure_Year { get; set; }
        public int Policy_Year { get; set; }
        public double PU_Sum { get; set; }
        public double PU_Immediate_Payment { get; set; }
        public double ET_Year { get; set; }
        public double ET_Day { get; set; }
        public double ET_Immediate_Payment { get; set; }
        public double ET_Maturity_Payment { get; set; }
    }
    #endregion

    /// <summary>
 /// This function return list of cash value rate.
 /// </summary>
 /// <param name="customer_age">If study save package please set age = 39</param>
    /// <param name="gender">If study save package please set gender = 1</param>
 /// <param name="product_id"></param>
  /// <returns></returns>
    public static List<CashValue> GetCashValueRateList(int customer_age, int gender, string product_id)
    {

        List<CashValue> List_cash_value = new List<CashValue>();
        CashValue cash_value;
        try
        {
            DataTable tbl = new DataTable();
            tbl = DataSetGenerator.Get_Data_Soure("SP_Get_Product_Life_CV_Item", new string[,] { { "@Age",customer_age+""}, {"@Gender", gender+""}, {"@Product_ID", product_id}});
            foreach (DataRow row in tbl.Rows)
            {
                cash_value = new CashValue();
                cash_value.Product_ID = row["product_id"].ToString();
                cash_value.Gender = Convert.ToInt32(row["gender"].ToString());
                cash_value.Assure_Year = Convert.ToInt32(row["assure_year"].ToString());
                cash_value.Age = Convert.ToInt32(row["age"].ToString());
                cash_value.Policy_Year = Convert.ToInt32(row["policy_year"].ToString());
                cash_value.Cash_Value = Convert.ToDouble(row["cash_value"].ToString());

                List_cash_value.Add(cash_value);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetCashValueRateLish] in class [da_application_study_save], Detail: " + ex.Message);
            
        }

        return List_cash_value;
    }
    /// <summary>
    /// * For study save: PU_Immediate_Payment = PU_SurPlus [column in data base], 
    /// ET_Immediate_Payment = ET_SurPlus [column in data base]
    /// </summary>
    /// <param name="customer_age">If study save package please set age = 39</param>
    /// <param name="gender">If study save package please set gender = 1</param>
    /// <param name="product_id"></param>
    /// <param name="assure_year"></param>
    /// <returns></returns>
    public static List<ETPU> GetETPUList(int customer_age, int gender, string product_id)
    {
        List<ETPU> etpu_list = new List<ETPU>();
        ETPU etpu;
        try
        {
            DataTable tbl = new DataTable();
            tbl = DataSetGenerator.Get_Data_Soure("SP_Get_Product_Life_ETPU_Item", new string[,] { { "@Age",customer_age+""}, {"@Gender", gender+""}, {"@Product_ID", product_id} });
            foreach (DataRow row in tbl.Rows)
            {
                etpu = new ETPU();
                etpu.Product_ID = row["product_id"].ToString();
                etpu.Gender = Convert.ToInt32(row["gender"].ToString());
                etpu.Policy_Year = Convert.ToInt32(row["policy_year"].ToString());
                etpu.Age = Convert.ToInt32(row["age"].ToString());
                etpu.Assure_Year = Convert.ToInt32(row["assure_year"].ToString());
                etpu.PU_Sum = Convert.ToDouble(row["pu_sum"].ToString());
                etpu.PU_Immediate_Payment = Convert.ToDouble(row["pu_surplus"].ToString());
                etpu.ET_Year = Convert.ToDouble(row["et_year"].ToString());
                etpu.ET_Day = Convert.ToDouble(row["et_day"].ToString());
                etpu.ET_Immediate_Payment = Convert.ToDouble(row["et_surplus"].ToString());
                etpu.ET_Maturity_Payment = Convert.ToDouble(row["et_mature"].ToString());

                etpu_list.Add(etpu);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetETPUList] in class [da_application_study_save], Detail: " + ex.Message);
        }

        return etpu_list;
    }

    public static List<bl_nonforfiet_value> GetNonforfietValueList(string product_id, int customer_age, int assure_year, int gender, double sum_insured ,string policy_id, string policy_number )
    {
       
        List<bl_nonforfiet_value> nonlist = new List<bl_nonforfiet_value>();
        try
        {
            bl_nonforfiet_value value;
            DataTable tbl = new DataTable();

            List<ETPU> etpu_list = GetETPUList(customer_age, gender, product_id);

            #region Get Cash value list
            List<CashValue> cash_value_list = GetCashValueRateList(customer_age, gender, product_id);

            #region Blocked Code
            //foreach (CashValue cash_value in cash_value_list)
            //{
            //    value = new bl_nonforfiet_value();

            //    value.Policy_ID = "";
            //    value.Policy_Number = "00000001";
            //    value.Cash_Value = (sum_insured * cash_value.Cash_Value)/1000 ;
            //    value.Policy_Year = cash_value.Policy_Year;
            //    value.PU_IME_Payment = 0;
            //    value.PU_SI = 0;
            //    //value.ET_Year = 0;
            //    //value.ET_Day = 0;
            //    value.ET_IME_Payment = 0;
            //    value.ET_Maturity = 0;
              
            //        value.ET_Day = 246;
            //        value.ET_Year = 0;
               
            //    nonlist.Add(value);
            //}
            #endregion

            for (int i = 0; i < cash_value_list.Count; i++)
            {
                var cash_value = cash_value_list[i];
                var etpu = etpu_list[i];

                value = new bl_nonforfiet_value();

                value.Policy_ID = policy_id;
                value.Policy_Number = policy_number;
                value.Cash_Value =  (sum_insured * cash_value.Cash_Value) / 1000;
                value.Policy_Year = cash_value.Policy_Year;
              

                if (cash_value.Assure_Year == etpu.Assure_Year && cash_value.Product_ID == etpu.Product_ID && cash_value.Age == etpu.Age && cash_value.Policy_Year == etpu.Policy_Year)
                {
                    value.PU_IME_Payment = (etpu.ET_Immediate_Payment * sum_insured) / 1000;
                    value.PU_SI = (etpu.PU_Sum * sum_insured) / 1000; ;
                    value.ET_Maturity = (etpu.ET_Maturity_Payment * sum_insured) / 1000;
                    value.ET_Day = etpu.ET_Day;
                    value.ET_Year = etpu.ET_Year;
                }

                nonlist.Add(value);
            }

            #endregion

            #region Blocked code
            //for (int i = 1; i <= assure_year; i++)
            //{
            //    value = new bl_nonforfiet_value();

            //    value.Policy_ID = "";
            //    value.Policy_Number = "00000001";
            //    value.Cash_Value = 0;
            //    value.Policy_Year = i;
            //    value.PU_IME_Payment = 0;
            //    value.PU_SI = 0;
            //    //value.ET_Year = 0;
            //    //value.ET_Day = 0;
            //    value.ET_IME_Payment = 0;
            //    value.ET_Maturity = 0;
            //    if (i == 1)//it's the first year
            //    {
            //        value.ET_Day = 246;
            //        value.ET_Year = 0;
            //    }
            //    //else if (i == 2)
            //    //{
            //    //    //value.ET_Day = 0;
            //    //    value.ET_Year = assure_year - 2;
            //    //}
            //    else
            //    {
            //        value.ET_Year = assure_year - value.Policy_Year;
            //    }

                

            //    nonlist.Add(value);
            //}
            #endregion
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetNonforfietValueList] in class [da_application_studay_save], Detail: " + ex.Message);
        }
        return nonlist;
    }
    #endregion

    /// <summary>
    /// Abbreviation: 
    /// PU = Paid UP
    /// ET = Extended Term
    /// IME = Immediate
    /// </summary>
    public class bl_nonforfiet_value
    {

        #region variables declaration

        #endregion
        #region Properties
        public string Policy_ID { get; set; }
        public string Policy_Number { get; set; }
        public int Policy_Year { get; set; }
        public double Cash_Value { get; set; }
        public double PU_IME_Payment { get; set; }
        public double PU_SI { get; set; }
        public double ET_Year { get; set; }
        public double ET_Day { get; set; }
        public double ET_IME_Payment { get; set; }
        public double ET_Maturity { get; set; }

        #endregion
    }
    /// <summary>
    /// This class is individual study save
    /// </summary>
    public class study_save
    {
        /// <summary>
        /// Get premuim of study save by payment mode
        /// Return Value [@A1, @A2] => @A1 is premium by pay mode, @A2 is annaul premium
        /// </summary>
        /// <param name="customer_age"></param>
        /// <param name="gender"></param>
        /// <param name="product_id"></param>
        /// <param name="assure_year"></param>
        /// <returns>My return values</returns>
        public static double[,] GetPremium(int customer_age, int gender, string product_id, int assure_year, int pay_mode, double sum_insured)
        {
            double[,] arr_premium = new double[,] { { 0, 0 } };
            double premium_by_pay_mode = 0;
            double annual_premium = 0;
            double rate = 0.0;
            try
            {
                DataTable tbl = new DataTable();
                //tbl = DataSetGenerator.Get_Data_Soure("SP_GET_STUDY_SAVE_PREMIUM", new string[,] { { "@Customer_Age",customer_age+""}, {"@Gender", gender+""},
                //                                                                                    {"@Product_ID", product_id},{"@Assure_Year",assure_year+""} });\
                tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FP6_Premium_Rate", new string[,] { { "@Age_Band",customer_age+""}, {"@Gender", gender+""},
                                                                                                {"@Product_ID", product_id},{"@Assure_Year",assure_year+""} });
                foreach (DataRow row in tbl.Rows)
                {
                    rate = Convert.ToDouble(row["rate"].ToString());
                    // annual_premium = Math.Ceiling(annual_premium);

                    
                }
                annual_premium = (sum_insured * rate) / 1000;

                premium_by_pay_mode = Math.Ceiling(annual_premium);

                //0	Single	1
                //1	Annually	1
                //2	Semi-Annual	0.54
                //3	Quarterly	0.27
                //4	Monthly	0.09
                switch (pay_mode)
                {
                    case 0:
                        premium_by_pay_mode = annual_premium * 1;
                        break;
                    case 1:
                        premium_by_pay_mode = annual_premium * 1;
                        break;
                    case 2:
                        premium_by_pay_mode = annual_premium * 0.52;//not used 0.54
                        break;
                    case 3:
                        premium_by_pay_mode = annual_premium * 0.27;
                        break;
                    case 4:
                        premium_by_pay_mode = annual_premium * 0.09;
                        break;

                }

                //Discount base on Sur Assured
                if (sum_insured > 25000)
                {//discount 0% 

                }
                else if (sum_insured >= 25000 && sum_insured < 50000)
                { //discount 5%
                    premium_by_pay_mode = premium_by_pay_mode - (premium_by_pay_mode * 5) / 100;
                }
                else if (sum_insured >= 50000 && sum_insured < 100000)
                { //discount 7%
                    premium_by_pay_mode = premium_by_pay_mode - (premium_by_pay_mode * 7) / 100;
                }
                else if (sum_insured >= 100000)
                { //discount 10%
                    premium_by_pay_mode = premium_by_pay_mode - (premium_by_pay_mode * 10) / 100;
                }

                //round up
                premium_by_pay_mode = Math.Ceiling(premium_by_pay_mode);
                arr_premium = new double[,] { { premium_by_pay_mode, annual_premium } };
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetPrmium] in class [da_application_study_save_package], Detail: " + ex.Message);
                arr_premium = new double[,] { { 0, 0 } };
            }

            return arr_premium;
        }

        /// <summary>
        /// GetTPDPremium of study save
        /// Return Value [@1,@2]: @1 is premium by pay mode, @2 is annual premium
        /// </summary>
        /// <param name="customer_age"></param>
        /// <param name="gender"></param>
        /// <param name="product_id"></param>
        /// <param name="assure_year"></param>
        /// <param name="pay_mode"></param>
        /// <returns></returns>
        public static double[,] GetTPDPremium(int customer_age, int gender, string product_id, int assure_year, int pay_mode, double sum_insured)
        {
            double[,] arr_premium = new double[,] { { 0, 0 } };
            double premium_by_pay_mode = 0;
            double annual_premium = 0;
            try
            {
                //for stuy save normal use rate of family protection normal.
                string str_premium = da_application_fp6.GetTPDPremium(sum_insured, product_id, gender, customer_age,assure_year, pay_mode);

                string[] arr_pre = str_premium.Split('/');
                annual_premium = Convert.ToDouble(arr_pre[0].ToString());
                premium_by_pay_mode = Convert.ToDouble(arr_pre[1].ToString());
               
                arr_premium = new double[,] { { premium_by_pay_mode, annual_premium } };

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetTPDPrmium] in class [da_application_study_save=>study_save], Detail: " + ex.Message);
                arr_premium = new double[,] { { 0, 0 } };
                
            }

            return arr_premium;
        }


        /// <summary>
        /// This class it uses same rate as family protection normal
        /// </summary>
        /// <param name="product_id"></param>
        /// <param name="pay_mode"></param>
        /// <param name="sum_insured"></param>
        /// <param name="rate"></param>
        /// <returns>premium[premium_by_pay_mode, annual_premium]</returns>
        public static double[,] GetADBPremium(string product_id, int pay_mode, double sum_insured, double rate)
        {
            double[,] arr_premium = new double[,] { { 0, 0 } };
            double premium_by_pay_mode = 0;
            double annual_premium = 0;
            try
            {
                //for stuy save normal use rate of family protection normal.
                string str_premium = da_application_fp6.GetADBPremium(sum_insured, product_id, rate, pay_mode);

                string[] arr_pre = str_premium.Split('/');
                annual_premium = Convert.ToDouble(arr_pre[0].ToString());
                premium_by_pay_mode = Convert.ToDouble(arr_pre[1].ToString());

                arr_premium = new double[,] { { premium_by_pay_mode, annual_premium } };

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetADBPremium] in class [da_application_study_save=>study_save], Detail: " + ex.Message);
                arr_premium = new double[,] { { 0, 0 } };
            }

            return arr_premium;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sumInsured"></param>
        /// <param name="productId"></param>
        /// <param name="gender"></param>
        /// <param name="age"></param>
        /// <param name="paymentMode"></param>
        /// <param name="assuredYear"></param>
        /// <param name="life_insured_type">0 = Spouse, 1 = kids</param>
        /// <returns></returns>
        public static double[,] GetPremiumRider(double sumInsured, string productId, int gender, int age, int paymentMode, int assuredYear, int life_insured_type)
        {
            double[,] arr_premium = new double[,] { { 0, 0 } };
            arr_premium = da_application_fp6.getPremiumFP6Sub(sumInsured, productId, gender, age, paymentMode, assuredYear, life_insured_type);
            return arr_premium;
        }


    }


    /// <summary>
    /// This class is study save package
    /// </summary>
    public class study_save_package
    {
        /// <summary>
        /// Get premuim of study save  
        /// Return Value [@A1, @A2] => @A1 is premium by pay mode, @A2 is annaul premium
        /// </summary>
        /// <param name="customer_age">will set to zero automaticaly</param>
        /// <param name="gender"></param>
        /// <param name="product_id"></param>
        /// <param name="assure_year"></param>
        /// <returns>My return values</returns>
        public static double[,] GetPremium(int customer_age, int gender, string product_id, int assure_year, int pay_mode)
        {
            double[,] arr_premium = new double[,] { { 0, 0 } };
            double premium_by_pay_mode = 0;
            double annual_premium = 0;
            try
            {
                //reset age to zero
                customer_age = 0;
                DataTable tbl = new DataTable();
                //tbl = DataSetGenerator.Get_Data_Soure("SP_GET_STUDY_SAVE_PREMIUM", new string[,] { { "@Customer_Age",customer_age+""}, {"@Gender", gender+""},
                //                                                                                    {"@Product_ID", product_id},{"@Assure_Year",assure_year+""} });\
                tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FP6_Premium_Rate", new string[,] { { "@Age_Band",customer_age+""}, {"@Gender", gender+""},
                                                                                                {"@Product_ID", product_id},{"@Assure_Year",assure_year+""} });
                foreach (DataRow row in tbl.Rows)
                {
                    annual_premium = Convert.ToDouble(row["rate"].ToString());
                    // annual_premium = Math.Ceiling(annual_premium);

                    //0	Single	1
                    //1	Annually	1
                    //2	Semi-Annual	0.54
                    //3	Quarterly	0.27
                    //4	Monthly	0.09
                    switch (pay_mode)
                    {
                        case 0:
                            premium_by_pay_mode = annual_premium * 1;
                            break;
                        case 1:
                            premium_by_pay_mode = annual_premium * 1;
                            break;
                        case 2:
                            premium_by_pay_mode = annual_premium * 0.52;//not used 0.54
                            break;
                        case 3:
                            premium_by_pay_mode = annual_premium * 0.27;
                            break;
                        case 4:
                            premium_by_pay_mode = annual_premium * 0.09;
                            break;

                    }
                    //round up
                    premium_by_pay_mode = Math.Ceiling(premium_by_pay_mode);
                    arr_premium = new double[,] { { premium_by_pay_mode, annual_premium } };
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetPrmium] in class [da_application_study_save_package], Detail: " + ex.Message);
                arr_premium = new double[,] { { 0, 0 } };
            }

            return arr_premium;
        }

        /// <summary>
        /// Get TPD Premium base on product_id, assure_year and gender
        /// </summary>
        /// <param name="customer_age">[put age =0, because in database store age 0 only.]</param>
        /// <param name="gender"></param>
        /// <param name="product_id"></param>
        /// <param name="assure_year"></param>
        /// <param name="pay_mode"></param>
        /// <returns></returns>
        public static double[,] GetTPDPremium(int customer_age, int gender, string product_id, int assure_year, int pay_mode)
        {
            double[,] arr_premium = new double[,] { { 0, 0 } };
            double premium_by_pay_mode = 0;
            double annual_premium = 0;
            try
            {
                //set age =0 
               // customer_age = 0;
               // annual_premium = da_application_fp6.GetTPDPremiumFPPackage(product_id, assure_year, gender, customer_age);
                DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_Study_Save_Package_TPD_Premium_Rate", new string[,] { { "@Product_ID", product_id },
                                                                                                        {"@Gender", gender+""}, 
                                                                                                        {"@Assure_Year",assure_year +""},
                                                                                                        {"@Age_Band",customer_age +""}});
                annual_premium = Convert.ToDouble(tbl.Rows[0][0].ToString());
                //calculate premium by pay mode
                //0	Single	1
                //1	Annually	1
                //2	Semi-Annual	0.54
                //3	Quarterly	0.27
                //4	Monthly	0.09
                switch (pay_mode)
                {
                    case 0:
                        premium_by_pay_mode = annual_premium * 1;
                        break;
                    case 1:
                        premium_by_pay_mode = annual_premium * 1;
                        break;
                    case 2:
                        premium_by_pay_mode = annual_premium * 0.52;//not used 0.54
                        break;
                    case 3:
                        premium_by_pay_mode = annual_premium * 0.27;
                        break;
                    case 4:
                        premium_by_pay_mode = annual_premium * 0.09;
                        break;

                }
                //round up
                premium_by_pay_mode = Math.Ceiling(premium_by_pay_mode);
                arr_premium = new double[,] { { premium_by_pay_mode, annual_premium } };

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetTPDPrmium] in class [da_application_study_save_package => study_save_package], Detail: " + ex.Message);
                arr_premium = new double[,] { { 0, 0 } };
            }

            return arr_premium;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product_id"></param>
        /// <param name="pay_mode"></param>
        /// <param name="sum_insured"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
          public static double[,] GetADBPremium(string class_name, int pay_mode)
        {
            double[,] arr_premium = new double[,] { { 0, 0 } };
            double premium_by_pay_mode = 0;
            double annual_premium = 0;
            try
            {
                //for stuy save normal use rate of family protection normal.
                double premium=da_application_fp6.GetADBPremiumFPPackage(class_name);
                annual_premium = premium;
                //calculate premium by pay mode
                //0	Single	1
                //1	Annually	1
                //2	Semi-Annual	0.54
                //3	Quarterly	0.27
                //4	Monthly	0.09
                switch (pay_mode)
                {
                    case 0:
                        premium_by_pay_mode = annual_premium * 1;
                        break;
                    case 1:
                        premium_by_pay_mode = annual_premium * 1;
                        break;
                    case 2:
                        premium_by_pay_mode = annual_premium * 0.52;//not used 0.54
                        break;
                    case 3:
                        premium_by_pay_mode = annual_premium * 0.27;
                        break;
                    case 4:
                        premium_by_pay_mode = annual_premium * 0.09;
                        break;

                }
               
                //annual_premium = premium;
                premium_by_pay_mode = Math.Ceiling(premium_by_pay_mode) ;

                arr_premium = new double[,] { { premium_by_pay_mode, annual_premium } };

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function [GetADBPremium] in class [da_application_study_save=>study_save_package], Detail: " + ex.Message);
                arr_premium = new double[,] { { 0, 0 } };
            }

            return arr_premium;
        
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="sumInsured"></param>
          /// <param name="productId"></param>
          /// <param name="gender"></param>
          /// <param name="age">will set to zero automaticaly</param>
          /// <param name="paymentMode"></param>
          /// <param name="assuredYear"></param>
          /// <param name="life_insured_type">0 = Spouse, 1 = kids</param>
          /// <returns></returns>
          public static double[,] GetPremiumRider(double sumInsured, string productId, int gender, int age, int paymentMode, int assuredYear, int life_insured_type)
          {
              double[,] arr_premium = new double[,] { { 0, 0 } };

              double premium_by_pay_mode = 0;
              double annual_premium = 0;

              //reset age to zero
              age = 0;

              try
              {
                  DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_FP6_Premium_Sub_Rate", new string[,] { { "@Age_Band", age+"" }, { "@Gender", gender+"" }, 
                                                                                                                {"@Product_ID", productId}, {"@Assure_Year", assuredYear+""} , 
                                                                                                                {"@Life_Insured_Type", life_insured_type+""}});


                  foreach (DataRow row in tbl.Rows)
                    {
                        annual_premium = Convert.ToDouble(row["rate"].ToString());
                       
                  }
                  //calculate premium by pay mode
                  //0	Single	1
                  //1	Annually	1
                  //2	Semi-Annual	0.54
                  //3	Quarterly	0.27
                  //4	Monthly	0.09
                  switch (paymentMode)
                  {
                      case 0:
                          premium_by_pay_mode = annual_premium * 1;
                          break;
                      case 1:
                          premium_by_pay_mode = annual_premium * 1;
                          break;
                      case 2:
                          premium_by_pay_mode = annual_premium * 0.52;//not used 0.54
                          break;
                      case 3:
                          premium_by_pay_mode = annual_premium * 0.27;
                          break;
                      case 4:
                          premium_by_pay_mode = annual_premium * 0.09;
                          break;

                  }
                  premium_by_pay_mode = Math.Ceiling(premium_by_pay_mode);
                 arr_premium = new double[,] { { premium_by_pay_mode, annual_premium } };
                  
              }
              catch (Exception ex)
              {
                  Log.AddExceptionToLog("Error function [GetPremiumRider] in class [da_application_study_save=>study_save_pack], Detail: " + ex.Message);
                  arr_premium = new double[,] { { 0, 0 } };
              }
              return arr_premium;
          }


    }

    
}
    

