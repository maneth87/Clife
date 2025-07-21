using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for PolicyPaymentScheduleWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PolicyPaymentScheduleWebService : System.Web.Services.WebService {

    public PolicyPaymentScheduleWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
   
    //public string SavePolicyPaymentSchedule(string policy_id, string product_id, string pay_mode, string sum_insure, string due_date, string year, string time, string premium, string discount, string premium_after_discount, string em_premium, string total_premium, string remark, string user_name)
    //{
    //    string result = "1";

    //    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
    //    dtfi.ShortDatePattern = "dd/MM/yyyy";
    //    dtfi.DateSeparator = "/";
        
    //    string[] arr_due_date = due_date.Split(',');
    //    string[] arr_year = year.Split(',');
    //    string[] arr_time = time.Split(',');
    //    string[] arr_premium = premium.Split(',');
    //    string[] arr_discount = discount.Split(',');
    //    string[] arr_premium_after_discount = premium_after_discount.Split(',');
    //    string[] arr_em_premium = em_premium.Split(',');
    //    string[] arr_total_premium = total_premium.Split(',');
    //    string[] arr_remark = remark.Split(',');

    //    //Delete By Policy ID
    //    da_policy_payment_schedule.DeletePolicyPaymentScheduleByPolicyID(policy_id);

    //    for (int i = 0; i < arr_year.Count() - 1; i++)
    //    {
    //        try
    //        {
    //            string new_guid = Helper.GetNewGuid("SP_Check_Policy_Payment_Schedule_ID", "@Policy_Payment_Schedule_ID").ToString();

    //            bl_policy_payment_schedule policy_payment_schedule = new bl_policy_payment_schedule();

    //            policy_payment_schedule.Created_By = user_name;
    //            policy_payment_schedule.Created_Note = arr_remark[i];
    //            policy_payment_schedule.Created_On = DateTime.Now;
    //            policy_payment_schedule.Discount = Convert.ToDouble(arr_discount[i]);
    //            policy_payment_schedule.Due_Date = Convert.ToDateTime(arr_due_date[i], dtfi);
    //            policy_payment_schedule.Extra_Premium = Convert.ToDouble(arr_em_premium[i]);
    //            policy_payment_schedule.Pay_Mode = Convert.ToInt32(pay_mode);
    //            policy_payment_schedule.Policy_ID = policy_id;
    //            policy_payment_schedule.Policy_Payment_Schedule_ID = new_guid;
    //            policy_payment_schedule.Premium = Convert.ToDouble(arr_premium[i]);
    //            policy_payment_schedule.Premium_After_Discount = Convert.ToDouble(arr_premium_after_discount[i]);
    //            policy_payment_schedule.Product_ID = product_id;
    //            policy_payment_schedule.Sum_Insure = Convert.ToDouble(sum_insure);
    //            policy_payment_schedule.Time = Convert.ToInt32(arr_time[i]);
    //            policy_payment_schedule.Total_Premium = Convert.ToDouble(arr_total_premium[i]);
    //            policy_payment_schedule.Year = Convert.ToInt32(arr_year[i]);

    //            if (!da_policy_payment_schedule.InsertPolicyPaymentSchedule(policy_payment_schedule))
    //            {
    //                result = "0";
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            ////Delete By Policy ID
    //            //da_policy_payment_schedule.DeletePolicyPaymentScheduleByPolicyID(policy_id);
    //            result = "0";
    //            //Add error to log 
    //            Log.AddExceptionToLog("Error in function [SavePolicyPaymentSchedule] in class [PolicyPaymentScheduleWebService]. Details: " + ex.Message);
    //        }
    //    }

    //    return result;
    //}
    // modified by maneth
    #region
     [WebMethod]
    public string SavePolicyPaymentSchedule(string policy_id, string product_id, string pay_mode, 
                                            string sum_insure, string year, string time, string premium, string discount, 
                                            string premium_after_discount, string em_premium, string total_premium, 
                                            string remark, string user_name, string disPeriod)
    {
        string result = "1";

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        string[] arr_year = year.Split(',');
        string[] arr_time = time.Split(',');
        string[] arr_premium = premium.Split(',');
        string[] arr_discount = discount.Split(',');
        string[] arr_premium_after_discount = premium_after_discount.Split(',');
        string[] arr_em_premium = em_premium.Split(',');
        string[] arr_total_premium = total_premium.Split(',');
        string[] arr_remark = remark.Split(',');
        string[] arr_disPeriod = disPeriod.Split(',');

        //tototal premium rider
         double total_premium_riders=0;
        foreach(bl_policy_premium_riders riders in da_policy_premium_riders.GetPolicyPremiumRidersList(policy_id))
        {
            total_premium_riders = total_premium_riders + riders.Premium;
        }

        int n = 0;
        int _pay_mode = Convert.ToInt32(pay_mode);

        switch (_pay_mode)
        {
            case 0:
            case 1:
                n = 1;
                break;
            case 2:
                n = 2;
                break;
            case 3:
                n = 4;
                break;
            case 4:
                n = 12;
                break;
        }

       string policy_number = da_policy_payment_schedule.getPolicyNumber(policy_id);
       
        bl_policy_payment_schedule policy = da_policy_payment_schedule.GetPolicyDetails(policy_number);

        DateTime effective_date = da_effective_date.getEffectiveDate(policy_id);
        //loop years
        for (int i = 0; i < arr_year.Count() - 1; i++)
        {
            //loop times
            for (int t = 1; t <= n; t++)
            {
                DateTime due_date = effective_date;
                DateTime my_date = new DateTime();

                #region
                if (i == 0 & t==1)
                {
                    //first year and first paid due date = effective date
                    my_date = effective_date;
                }
                else
                {
                    switch (_pay_mode)
                    {
                        case 0:
                        case 1:
                            effective_date = effective_date.AddYears(1);
                            break;
                        case 2:
                            effective_date = effective_date.AddMonths(6);
                            break;
                        case 3:
                            effective_date = effective_date.AddMonths(3);
                            break;
                        case 4:
                            effective_date = effective_date.AddMonths(1);
                            break;
                    }
                    due_date = effective_date;
                    my_date = Calculation.CheckDayNextDueWithEffectiveDay(due_date, policy_id);
                    effective_date = my_date;
                }
                #endregion
                
               // my_date = Calculation.GetNext_Due(policy.Due_Date, due_date, effective_date);
               // my_date = Calculation.CheckDayNextDueWithEffectiveDay(due_date, policy_id);

                try
                {
                    string new_guid = Helper.GetNewGuid("SP_Check_Policy_Payment_Schedule_ID", "@Policy_Payment_Schedule_ID").ToString();

                    bl_policy_payment_schedule policy_payment_schedule = new bl_policy_payment_schedule();

                    //int a=0;
                    //switch (policy.Pay_Mode)
                    //{
                    //    case 0:
                    //    case 1:
                    //       a = 1;
                    //        break;
                    //    case 2:
                    //        a = 2;
                    //        break;
                    //    case 3:
                    //        a = 4;
                    //        break;
                    //    case 4:
                    //        a = 12;
                    //        break;
                    //}

                    Double monthdiscount = 0;
                    Double monthexpremium = 0;
                    Double monthpreafterdiscount = 0;
                    Double totalpremium = 0;

                    monthdiscount=Convert.ToDouble(arr_discount[i])/n;
                    monthdiscount = Math.Floor(monthdiscount);

                    monthexpremium=Convert.ToDouble(policy.Extra_Premium)/n;
                    monthexpremium = Math.Ceiling(monthexpremium);

                    monthpreafterdiscount = Convert.ToDouble(policy.Premium) - monthdiscount + total_premium_riders;
                    totalpremium = monthpreafterdiscount + monthexpremium ;

                    string created_note = "";
                    //if (t == 1)
                    //{
                    //    created_note = arr_remark[i];
                    //}
                    //else
                    //{
                    //    created_note = "";
                    //}

                    //show create note by period
                    if (t <= Convert.ToInt32(arr_disPeriod[i]))
                    {
                        created_note = arr_remark[i];
                    }
                    else
                    {
                        created_note = "";
                    }

                    policy_payment_schedule.Created_By = user_name;
                    policy_payment_schedule.Created_Note = created_note;// arr_remark[i];
                    policy_payment_schedule.Created_On = DateTime.Now;
                    policy_payment_schedule.Discount =  monthdiscount;
                    policy_payment_schedule.Due_Date = Convert.ToDateTime(my_date);
                    policy_payment_schedule.Extra_Premium = monthexpremium; //Convert.ToDouble(arr_em_premium[i]);
                    policy_payment_schedule.Pay_Mode =  Convert.ToInt32(pay_mode);
                    policy_payment_schedule.Policy_ID =  policy_id;
                    policy_payment_schedule.Policy_Payment_Schedule_ID = new_guid;
                    policy_payment_schedule.Premium = Convert.ToDouble(policy.Premium + total_premium_riders); //Convert.ToDouble(arr_premium[i]);
                    policy_payment_schedule.Premium_After_Discount = monthpreafterdiscount;// Convert.ToDouble(arr_premium_after_discount[i]);
                    policy_payment_schedule.Product_ID =  product_id;
                    policy_payment_schedule.Sum_Insure =  Convert.ToDouble(sum_insure);
                    policy_payment_schedule.Time = t;
                    policy_payment_schedule.Total_Premium = totalpremium;// Convert.ToDouble(arr_total_premium[i]);
                    policy_payment_schedule.Year = i + 1;

                    if (!da_policy_payment_schedule.InsertPolicyPaymentSchedule(policy_payment_schedule))
                    {
                        result = "0";
                    }

                }
                catch (Exception ex)
                {
                    ////Delete By Policy ID
                    //da_policy_payment_schedule.DeletePolicyPaymentScheduleByPolicyID(policy_id);
                    result = "0";
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [SavePolicyPaymentSchedule] in class [PolicyPaymentScheduleWebService]. Details: " + ex.InnerException + ">>" + ex.StackTrace + ">>" + ex.Message);
                }
            }
           
        }

        return result;
    }
    
    [WebMethod]
    public List<bl_policy_payment_schedule> GetYear(string policy_number)
    {
        List<bl_policy_payment_schedule> years = new List<bl_policy_payment_schedule>();

        years = da_policy_payment_schedule.GetYear(policy_number);

        return years;
    }

    [WebMethod]
    public string UpdatePolicyPaymentSchedule(string policy_id, string product_id, string pay_mode, string year, string premium,
                                               string discount, string premium_after_discount,
                                               string extra_premium, string total_premium, string remark)
    {
        string result = "1";
        try
        {
            bool pay_status;
            pay_status = da_policy_payment_schedule.getPayStatus(policy_id, year);
            bl_policy_payment_schedule policy = new bl_policy_payment_schedule();
            if (pay_status)
            {
                result = "2"; //Current Year is in using.
            }
            else
            {
                string policy_number = da_policy_payment_schedule.getPolicyNumber(policy_id);
                bl_policy_payment_schedule policy_old = da_policy_payment_schedule.GetPolicyDetails(policy_number);
                
                int a = 0;
                switch (Convert.ToInt32(policy_old.Pay_Mode))
                {
                    case 0:
                    case 1:
                        a = 1;
                        break;
                    case 2:
                        a = 2;
                        break;
                    case 3:
                        a = 4;
                        break;
                    case 4:
                        a = 12;
                        break;
                }

                Double Dpremium = Convert.ToDouble(policy_old.Premium);
                Double Ddiscount = Convert.ToDouble(discount) / a;
                Double Dafter_discount = Convert.ToDouble(policy_old.Premium) - Convert.ToDouble(discount) / a;
                Double Dextra_premium = Convert.ToDouble(extra_premium);
                Double Dtotal_premium = Dafter_discount + Dextra_premium;

                policy.Policy_ID = policy_id;
                policy.Premium = Dpremium;
                policy.Product_ID = product_id;
                policy.Pay_Mode = Convert.ToInt32(pay_mode);
                policy.Year = Convert.ToInt32(year);
                policy.Discount = Ddiscount;
                policy.Premium_After_Discount = Dafter_discount; // Convert.ToDouble(premium_after_discount);
                policy.Extra_Premium = Dextra_premium;
                policy.Total_Premium = Dtotal_premium;
                policy.Created_Note = remark;

                if (!da_policy_payment_schedule.updatePolicyPaymentSchedule1(policy))
                {
                    result = "0";
                }
            }
        }
        catch (Exception ex)
        {
            result = "0";
            Log.AddExceptionToLog("Error in function [UpdatePolicyPaymentSchedule] in class [PolicyPaymentScheduleWebService]. Details: " + ex.Message);
        }
      //  Log.AddExceptionToLog("Result: " + result);
        return result;
    }

     [WebMethod]
     public string SaveTest(string f1, string f2)
     {
      string result = "1";
         try
         {
             string strinsert = "insert into tbl_test (field1, field2) values (@field1, @field2)";
             string connString = AppConfiguration.GetConnectionString();
             SqlConnection conn = new SqlConnection(connString);
             SqlCommand cmd = new SqlCommand(strinsert, conn);
             conn.Open();
             cmd.Parameters.AddWithValue("@field1", f1);
             cmd.Parameters.AddWithValue("@field2", f2);
             cmd.ExecuteNonQuery();
             cmd.Parameters.Clear();
             cmd.Dispose();
             conn.Close();
         }
         catch (Exception ex)
         {
             Log.AddExceptionToLog("Error in function SaveTest " + ex.Message);
             result = "0";
         }
   
         return result;
     }
    #endregion
}
