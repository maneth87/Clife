using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Data.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;

/// <summary>
/// Summary description for PolicyWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PolicyWebService : System.Web.Services.WebService
{
    string user_name = "";//System.Web.Security.Membership.GetUser().UserName;
    DateTime action_date = DateTime.Now;

    public PolicyWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //Get complete underwriting detail
    [WebMethod]
    public bl_policy_detail GetPolicyDetail(string policy_id)
    {
        bl_policy_detail my_policy = da_policy.GetPolicyDetail(policy_id);
        return my_policy;
    }

    [WebMethod]
    public bl_policy CheckPolicyByPolicyNumber(string policy_number)
    {
        bl_policy my_policy = da_policy.GetPolicyByPolicyNumber(policy_number);

        return my_policy;
    }

    //Get Benefits by policy_id
    [WebMethod]
    public List<bl_policy_benefit_item> GetBenefits(string policy_id)
    {
        List<bl_policy_benefit_item> benefits = new List<bl_policy_benefit_item>();

        benefits = da_policy.GetPolicyBenefitItem(policy_id);

        return benefits;
    }


    //GetCustomerByPolicyID
    //Get complete underwriting detail
    [WebMethod]
    public bl_customer GetCustomerByPolicyID(string policy_id)
    {
        bl_customer my_customer = da_customer.GetCustomerByPolicyID(policy_id);
        return my_customer;
    }

    //GetSurrenderValue   
    [WebMethod]
    public List<bl_prod_life_cv_item> GetSurrenderValue(string product_id, int gender, int customer_age, double sum_insured, int assure_year)
    {
        List<bl_prod_life_cv_item> cv_items = new List<bl_prod_life_cv_item>();

        cv_items = da_policy.GetProdLifeCVItem(product_id, gender, customer_age, sum_insured, assure_year);

        return cv_items;
    }

    /// Thavy Block

    /// <summary>
    /// Get Pre
    /// </summary>
    [WebMethod]
    public bl_policy_payment GetPreValue(string Policy_Number, string Customer_ID)
    {
        bl_policy_payment pre_payment = new bl_policy_payment();

        pre_payment = da_policy_prem_pay.GetPreList(Policy_Number, Customer_ID);

        return pre_payment;
    }

    [WebMethod]
    public List<bl_policy_payment> SearchPolicy_Num(string Policy_Number, string Customer_ID, string Last_Name, string First_Name)
    {
        List<bl_policy_payment> policy = new List<bl_policy_payment>();

        if (Policy_Number != "")
        {
            policy = da_policy_prem_pay.GetPolicy_NO(Policy_Number, Customer_ID, Last_Name, First_Name, 0);
        }
        else if (Customer_ID != "")
        {
            policy = da_policy_prem_pay.GetPolicy_NO(Policy_Number, Customer_ID, Last_Name, First_Name, 1);
        }
        else { policy = da_policy_prem_pay.GetPolicy_NO(Policy_Number, Customer_ID, Last_Name, First_Name, 2); }

        return policy;
    }

    [WebMethod]
    public List<bl_payment_receipt> Search_Lapsed_Policy(string Policy_Number, string Customer_ID, string Last_Name, string First_Name)
    {
        List<bl_payment_receipt> policy_lapsed = new List<bl_payment_receipt>();

        ///
        List<bl_policy_payment> policy = new List<bl_policy_payment>();

        if (Policy_Number != "")
        {
            policy = da_policy_prem_pay.GetPolicy_Check_Lapsed(Policy_Number, Customer_ID, Last_Name, First_Name, 0);
        }
        else if (Customer_ID != "")
        {
            policy = da_policy_prem_pay.GetPolicy_Check_Lapsed(Policy_Number, Customer_ID, Last_Name, First_Name, 1);
        }
        else { policy = da_policy_prem_pay.GetPolicy_Check_Lapsed(Policy_Number, Customer_ID, Last_Name, First_Name, 2); }

        if (policy.Count > 0)
        {
            DataTable dt = da_policy_prem_pay.GetLast_Due(policy[0].Policy_ID, int.Parse(policy[0].Pay_Mode_ID.ToString()), DateTime.Now, decimal.Parse(policy[0].Amount.ToString()), "", DateTime.Parse(policy[0].Due_Date.ToString()));
            ///

            foreach (DataRow item in dt.Rows)
            {
                bl_payment_receipt payment_receipt = new bl_payment_receipt();

                if (int.Parse(item["duration"].ToString()) > 0 && item["policy_status"].ToString() == "LAP")
                {
                    payment_receipt.due_date = DateTime.Parse(item["due_date"].ToString());
                    payment_receipt.interest_amount = decimal.Parse(item["interest_amount"].ToString());
                    payment_receipt.prem_amount = decimal.Parse(item["prem_amount"].ToString());
                    payment_receipt.duration = int.Parse(item["duration"].ToString());
                    payment_receipt.duration_month = int.Parse(item["duration_month"].ToString());
                    payment_receipt.policy_status = item["policy_status"].ToString();

                    policy_lapsed.Add(payment_receipt);
                }
            }
        }

        return policy_lapsed;
    }

    /// <summary>
    /// Get Pre by Policy Num & Pay_Mode_ID
    /// </summary>
    [WebMethod]
    public string GetPreValue_Poli_Num_Pay_Mode(string Policy_Number, int Pay_Mode_ID)
    {
        string pre_payment = da_policy_prem_pay.GetPre_by_Poli_Num_Mode(Policy_Number, Pay_Mode_ID);

        return pre_payment;
    }

    /// <summary>
    /// Get Month Paid 
    /// </summary>
    [WebMethod]
    public int GetMonth_Paid(string Policy_ID)
    {
        int total_month = 0;

        total_month = da_policy_prem_pay.Check_Remaining_Month(Policy_ID, 0, 1);

        return total_month;
    }

    /// <summary>
    /// Get Available Pay Mode
    /// </summary>
    [WebMethod]
    public int Result_Remaining_Month(string Policy_ID, int Pay_Mode_ID, int Product_Type)
    {
        int remaining_month = 0;

        if (Product_Type == 2 && Pay_Mode_ID != 0)
        {
            remaining_month = -1;
        }
        else
        {
            remaining_month = da_policy_prem_pay.Check_Remaining_Month(Policy_ID, Pay_Mode_ID, Product_Type);
        }

        return remaining_month;
    }

    /// <summary>
    /// Get Prem_Year 
    /// </summary>
    [WebMethod]
    public int GetPrem_Year(string Policy_ID)
    {
        int prem_year = 0;

        prem_year = da_policy_prem_pay.GetLast_Prem_Year(Policy_ID);

        return prem_year;
    }

    /// <summary>
    /// Get Receipt No, save
    /// </summary>
    [WebMethod]
    public string GetReceiptNo(string Receipt_Num, string Due_Date, string Policy_ID)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime due_date = DateTime.Parse(Due_Date, dtfi);

        string receipt = "";

        receipt = da_policy_prem_pay.GetReceipt_No_Due(Receipt_Num, "", due_date, 1, Policy_ID);

        return receipt;
    }

    /// <summary>
    /// Get Receipt No, Edit
    /// </summary>
    [WebMethod]
    public string GetReceiptNo_Edit(string Receipt_Num, string Policy_Prem_Pay_ID, string Due_Date, string Policy_ID)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime due_date = DateTime.Parse(Due_Date, dtfi);

        string receipt = "";

        receipt = da_policy_prem_pay.GetReceipt_No_Due(Receipt_Num, Policy_Prem_Pay_ID, due_date, 2, Policy_ID);

        return receipt;
    }

    /// <summary>
    /// Round Up & Down Interest
    /// </summary>
    [WebMethod]
    public string RounInterestUp_Down(string interest_total)
    {
        string total_interest = "";

        decimal interest_value = 0;
        interest_value = decimal.Parse(interest_total);
        interest_value = Math.Round(interest_value, 0, MidpointRounding.AwayFromZero);

        total_interest = interest_value.ToString();

        return total_interest;
    }

    /// <summary>
    /// Check Update Status
    /// </summary>
    [WebMethod]
    public string Check_Status(string due_date)
    {
        string check_update_status = "";

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(due_date, dtfi));

        int totalday = (int)Math.Ceiling(ts.TotalDays);

        if (totalday > 30)
        {
            check_update_status = "lap";
        }

        return check_update_status;
    }

    /// <summary>
    /// GetPolicy_Info
    /// </summary>
    [WebMethod]
    public string GetPolicy(string Policy_Number, string First_Name, string Last_Name)
    {
        string policy_info = "";

        DataTable dt = da_policy_prem_pay.GetPolicy_Info(Policy_Number, Last_Name, First_Name);

        policy_info = dt.Rows[0][0].ToString() + "," + dt.Rows[0][1].ToString() + "," + dt.Rows[0][2].ToString();


        return policy_info;
    }

    /// <summary>
    /// Calculate Lapsed amount
    /// </summary>
    [WebMethod]
    public string Lapsed_Amount(string Policy_Number, string Current_Date)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime currentDate = DateTime.Now;

        if (Current_Date != "")
        {
            currentDate = DateTime.Parse(Current_Date, dtfi);
        }

        string total_amount = ""; decimal total_prem = 0, total_interest = 0;

        DataTable dt = da_policy_prem_pay.GetPolicy_Lapsed(Policy_Number, "", "", currentDate);

        foreach (DataRow item in dt.Rows)
        {
            if (decimal.Parse(item["duration_day"].ToString()) > 0)
            {
                total_interest += decimal.Parse(item["Interest"].ToString());

                total_prem += decimal.Parse(item["Amount"].ToString());
            }
        }


        if (total_prem > 0)
        {
            if (total_interest >= 1)
            {
                total_interest = Math.Round(total_interest, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                total_interest = 0;
            }

            total_prem = (total_prem + total_interest);

            total_amount = total_prem.ToString();
        }
        else
        {

            total_amount = "0".ToString();
        }

        return total_amount;
    }
    /// End my block

    [WebMethod]
    public List<bl_check_policy_lapsed> SearchPolicy_Num_LAP(string Policy_Number, string Customer_ID, string Last_Name, string First_Name, string Current_Date)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTime currentDate = DateTime.Parse(Current_Date, dtfi);

        List<bl_check_policy_lapsed> policy = new List<bl_check_policy_lapsed>();

        if (Policy_Number != "")
        {
            policy = da_policy_prem_pay.Check_Lapsed(Policy_Number, Customer_ID, Last_Name, First_Name, currentDate);
        }
        else if (Customer_ID != "")
        {
            policy = da_policy_prem_pay.Check_Lapsed(Policy_Number, Customer_ID, Last_Name, First_Name, currentDate);
        }
        else { policy = da_policy_prem_pay.Check_Lapsed(Policy_Number, Customer_ID, Last_Name, First_Name, currentDate); }

        return policy;
    }

    #region Maneth 17032017
    /// <summary>
    /// Get obect inform letter by policy id and report_type
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="report_type"></param>
    /// <returns></returns>
    [WebMethod]
    public List<bl_inform_letter_printed_records> GetInformLetter(string policy_id, string report_type)
    {
        string[] arr_policy = policy_id.Split(',');
        return da_inform_letter_printed_records.GetPrintedInformLetter(arr_policy, report_type);
    }
    #endregion
    #region <Maneth 06062017>
    [WebMethod]
    public bool insertPrintedRecord(string policy_number, string printed_by, string report_type)
    {
        policy_number = (Convert.ToDouble(policy_number).ToString("00000000"));

        return da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(da_policy.GetPolicyByPolicyNumber(policy_number).Policy_ID, DateTime.Now, printed_by, report_type));
    }
    #endregion

    #region By Meas Sun Modified On 2019-11-18
    [WebMethod]
    public List<string> InsertPolicyFlatRate(List<string> policy)
    {
        List<string> list_return = new List<string>();

        string str_return = "";
        string App_Register_ID = "";
        string Policy_ID = "";
        string policy_number = "";
        string ChannelItemID = "";
        string GroupPolicy_ID = "";
        string main_policy_number = "";

        try
        {
            bl_policy obj_pol = new bl_policy();
            bl_policy_premium obj_prem = new bl_policy_premium();
            bl_policy_prem_pay obj_prem_pay = new bl_policy_prem_pay();

            bl_policy_status objPolicyStatus = new bl_policy_status();

            if (policy.Count > 0)
            {
                App_Register_ID = policy[21].ToString();

                #region Save Policy

                obj_pol.Customer_ID = policy[0].ToString();
                obj_pol.Product_ID = policy[1].ToString();
                obj_pol.Effective_Date = Helper.FormatDateTime(policy[2].ToString());
                obj_pol.Maturity_Date = Helper.FormatDateTime(policy[3].ToString());
                obj_pol.Agreement_Date = Helper.FormatDateTime(policy[4].ToString());
                obj_pol.Issue_Date = Helper.FormatDateTime(policy[5].ToString());
                obj_pol.Age_Insure = Convert.ToInt32(policy[6].ToString());
                obj_pol.Pay_Year = Convert.ToInt32(policy[7].ToString());
                obj_pol.Pay_Up_To_Age = Convert.ToInt32(policy[8].ToString());
                obj_pol.Assure_Year = Convert.ToInt32(policy[9].ToString());
                obj_pol.Assure_Up_To_Age = Convert.ToInt32(policy[10].ToString());
                obj_pol.Address_ID = policy[11].ToString();
                obj_pol.Created_On = DateTime.Now;
                obj_pol.Created_By = System.Web.Security.Membership.GetUser().UserName;
                obj_pol.ChannelItem_ID = policy[12].ToString();
                obj_pol.Created_Note = policy[22].ToString();
                #endregion

                #region Save Policy Premium

                obj_prem.Sum_Insure = Convert.ToInt32(policy[14].ToString());
                obj_prem.Premium = Convert.ToInt32(policy[15].ToString());
                obj_prem.Original_Amount = Convert.ToInt32(policy[19].ToString());
                obj_prem.EM_Amount = Convert.ToInt32(policy[20].ToString());
                obj_prem.Created_On = DateTime.Now;
                obj_prem.Created_By = System.Web.Security.Membership.GetUser().UserName;
                
                #endregion

                #region Save Policy Prem Pay

                obj_prem_pay.Policy_Prem_Pay_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_POLICY_PREM_PAY" }, { "FIELD", "POLICY_PREM_PAY_ID" } }); ;
                obj_prem_pay.Due_Date = Helper.FormatDateTime(policy[2].ToString()); // modified
                obj_prem_pay.Pay_Date = Helper.FormatDateTime(policy[25].ToString());  //get signature_date
                obj_prem_pay.Prem_Year = 1;
                obj_prem_pay.Prem_Lot = 1;
                obj_prem_pay.Office_ID = "HQ";
                obj_prem_pay.Created_On = Helper.FormatDateTime(policy[5].ToString()); //Further Change => issued_Date
                obj_prem_pay.Created_By = Membership.GetUser().UserName;
                obj_prem_pay.Created_Note = policy[22].ToString(); 
                obj_prem_pay.Amount = Convert.ToInt32(policy[15].ToString());
                obj_prem_pay.Sale_Agent_ID = policy[23].ToString();
                obj_prem_pay.Pay_Mode_ID = Convert.ToInt32(policy[16].ToString());
                #endregion

                policy_number = policy[13].ToString();

                if (policy_number == "")
                {
                    policy_number = da_policy.GetPolicyNumber();
                }

                //1.Add new policy then return policy ID
                Policy_ID = da_policy.InsertPolicy(obj_pol);
                str_return = Policy_ID;

                //2.Add new policy Number
                if (obj_pol.Product_ID == "CL24")
                {
                    #region Policy of credit life <By: Meassun Date:15-04-2020>

                    GroupPolicy_ID = Helper.GetNewGuid("SP_Check_GroupPolicy_ID", "@Group_Policy_ID").ToString();
                    da_policy.InsertGroupMasterPolicy(obj_pol.Product_ID, Policy_ID, GroupPolicy_ID, main_policy_number, ChannelItemID);

                    #endregion
                }
                else
                {
                    if (!da_policy.InsertPolicyNumber(Policy_ID, policy_number)) //Save policy number in table ct_policy_number.
                    {
                        str_return = "";
                    }
                }

                //2.Add new policy premium then return policy ID
                obj_prem.Policy_ID = Policy_ID;
                if (!da_policy.InsertPolicyPremium(obj_prem))
                {
                    str_return = "";
                }

                //3.Insert Policy_Prem_Pay
                obj_prem_pay.Policy_ID = Policy_ID;

                //4.policy premium pay
                if (!da_policy.InsertPolicyPremiumPay(obj_prem_pay))
                {
                    str_return = "";
                }

                //4.Insert Policy_ID
                if (!da_policy.InsertPolicyID(Policy_ID, Convert.ToInt32(policy[17].ToString())))
                {
                    str_return = "";
                }

                //5.Insert Policy Status 
                da_policy.InsertPolicyStatus(Policy_ID, Membership.GetUser().UserName, DateTime.Now);

                objPolicyStatus.Policy_ID = Policy_ID;
                objPolicyStatus.Policy_Status_Type_ID = policy[24].ToString();
                objPolicyStatus.Updated_By = Membership.GetUser().UserName;
                objPolicyStatus.Updated_On = DateTime.Now;
                objPolicyStatus.Updated_Note = "System update policy status to LAP";

                if (objPolicyStatus.Policy_Status_Type_ID == "LAP")
                {
                    if (!da_policy.UpdatePolicyStatus(objPolicyStatus))
                    {
                        str_return = "";
                    }
                }

                //6.Insert Policy Pay Mode
                if (!da_policy.InsertPolicyPayMode(Policy_ID, Convert.ToInt32(policy[16].ToString()), obj_pol.Effective_Date, obj_pol.Created_By, obj_pol.Created_On))
                {
                    str_return = "";
                }

                //7. Insert App Policy 
                if (!da_policy.InsertAppPolicy(App_Register_ID, Policy_ID))
                {
                    str_return = "";
                }

                #region Save Person
                //Insert only policy owner information
                da_application.bl_app_info_person_sub person = new da_application.bl_app_info_person_sub();
                bl_policy_detail policy_detail = new bl_policy_detail();
                policy_detail = da_policy.GetPolicyDetailByPolicyID(Policy_ID);

                person.Person_ID = Helper.GetNewGuid("SP_Check_App_Info_Person_Sub_ID", "@Person_ID").ToString();
                person.App_Register_ID = App_Register_ID;
                person.ID_Card = policy_detail.ID_Card;
                person.ID_Type = policy_detail.ID_Type;
                person.First_Name = policy_detail.First_Name;
                person.Last_Name = policy_detail.Last_Name;
                person.Khmer_First_Name = policy_detail.Khmer_First_Name;
                person.Khmer_Last_Name = policy_detail.Khmer_Last_Name;
                person.Gender = Convert.ToInt32(policy_detail.Gender);
                person.Birth_Date = Convert.ToDateTime(policy_detail.Birth_Date.ToString());
                person.Country_ID = policy_detail.Country_ID;
                person.Level = 0;
                person.Father_First_Name = "";
                person.Father_Last_Name = "";
                person.Mother_First_Name = "";
                person.Mother_Last_Name = "";
                person.Prior_First_Name = "";
                person.Prior_Last_Name = "";
                person.Marital_Status = "";
                person.Relationship = "";
                if (!da_application.InsertAppInfoPersonSub(person))
                {
                    str_return = "";
                }

                //Insert new row info of riders 
                if (!da_application.InsertAppInfoPerson(person))
                {
                    str_return = "";
                }
                #endregion

                if (str_return == "")
                    list_return.Add(str_return);
                else
                    list_return.Add(str_return);

                list_return.Add(Policy_ID);
            }
            else
            {
                list_return.Add(str_return);
                list_return.Add(Policy_ID);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [InsertPolicyFlatRate] in class [PolicyWebService], Detail: " + ex.Message);
            list_return.Add(str_return);
            list_return.Add(Policy_ID);
        }
        return list_return;
    }
    [WebMethod]
    public bool UpdatePolicyFlatRate(List<string> policy)
    {
        bool status = false;
        //try
        //{
        //    bl_policy_flat_rate obj = new bl_policy_flat_rate();
        //    if (policy.Count > 0)
        //    {

        //        obj.PolicyID = policy[0].ToString();
        //        obj.CustomerID = policy[1].ToString();
        //        obj.ProductID = policy[2].ToString();

        //        obj.AssuredYear = Convert.ToInt32(policy[3].ToString());
        //        obj.PayYear = Convert.ToInt32(policy[4].ToString());

        //        obj.PolicyNumber = policy[5].ToString();
        //        obj.ApplicationNumber = policy[6].ToString();
        //        obj.ApplicationOriginNumber = policy[7].ToString();
        //        obj.PaymentCode = policy[8].ToString();
        //        obj.ApplicationDate = Helper.FormatDateTime(policy[9].ToString());
        //        obj.EffectiveDate = Helper.FormatDateTime(policy[10].ToString());
        //        obj.SumInsured = Convert.ToDouble(policy[11].ToString());
        //        obj.PayModeID = Convert.ToInt32(policy[12].ToString());
        //        obj.PremiumByMode = Convert.ToDouble(policy[13].ToString());
        //        obj.AnnualOriginPremium = Convert.ToDouble(policy[14].ToString());
        //        obj.AnnualPremium = Convert.ToDouble(policy[15].ToString());
        //        obj.ActualPremium = Convert.ToDouble(policy[16].ToString());
        //        obj.Discount = Convert.ToDouble(policy[17].ToString());
        //        obj.ExtraAnnualPremium = Convert.ToDouble(policy[18].ToString());
        //        obj.ExtraPremiumByMode = Convert.ToDouble(policy[19].ToString());
        //        obj.ReturnPremium = Convert.ToDouble(policy[20].ToString());

        //        obj.MaturityDate = obj.EffectiveDate.AddYears(obj.AssuredYear);
        //        obj.ExpiryDate = obj.MaturityDate.AddDays(-1);

        //        obj.ApprovedBy = ""; // SQL WILL CONVERT "" TO NULL VALUE
        //        obj.PolicyRemarks = "";
        //        obj.ApplicationRemarks = "";
        //        obj.Remarks = "";

        //        obj.PolicyStatusID= 1;
        //        obj.UnderWritingStatusID = "";

        //        obj.UpdatedBy = System.Web.Security.Membership.GetUser().UserName;
        //        obj.UpdatedDateTime = DateTime.Now;

        //        obj.SaleAgentID = policy[21].ToString();

        //        obj.ChannelID = policy[22].ToString();
        //        obj.ChannelItemID = policy[23].ToString();

        //        if (da_policy_flat_rate.UpdatePolicy(obj))
        //        {
        //            status = true;
        //        }
        //        else
        //        {
        //            status = false;
        //        }
        //    }
        //    else                                 
        //    {
        //        status = false;
        //    }
        //    //status = da_policy_flat_rate.SavePolicy(obj);
        //}
        //catch (Exception ex)
        //{
        //    Log.AddExceptionToLog("Error function [UpdatePolicyFlatRate] in class [PolicyWebService], Detail: " + ex.Message);
        //}
        return status;
    }
    [WebMethod]
    public bl_policy_flat_rate GetPolicyFlatRate(string policy_id)
    {
        bl_policy_flat_rate obj = new bl_policy_flat_rate();
        try
        {
            obj = da_policy_flat_rate.GetPolicy(policy_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPoicy] in class [PolicyWebService], Detail: " + ex.Message);
        }
        return obj;
    }
    [WebMethod]
    public List<string> GetPay_AssuredYear(string product_id)
    {
        List<string> str = new List<string>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("select Pay_Year, Assure_Year from Ct_Product_Life where Product_ID='" + product_id.Trim() + "'");
            foreach (DataRow row in tbl.Rows)
            {
                str.Add(row["pay_year"].ToString());
                str.Add(row["assure_year"].ToString());
            }

        }
        catch (Exception ex)
        {
            str.Add("0");
            str.Add("0");
            Log.AddExceptionToLog("Error function [GetPay_AssuredYear] in class [PolicyWebService], Detail: " + ex.Message);
        }
        return str;
    }

    [WebMethod]
    public bool SaveRiders(List<string> riders)
    {
        bool status = true;
        try
        {
            if (riders.Count > 0)
            {
                da_application.bl_app_info_person_sub person = new da_application.bl_app_info_person_sub();
                bl_policy_premium_riders prem_rider = new bl_policy_premium_riders();
                string App_Register_ID = "";
                string Relationship = "";

                foreach (string rider in riders)
                {
                    string[] arr_riders = rider.Split(';');

                    if (arr_riders[6].ToString() != "12" && arr_riders[6].ToString() != "13")
                    {
                        App_Register_ID = arr_riders[17].ToString();
                        prem_rider.Policy_ID = arr_riders[0].ToString();
                        prem_rider.Sum_Insure = Convert.ToDouble(arr_riders[1].ToString());
                        prem_rider.Premium = Convert.ToDouble(arr_riders[2].ToString());
                        prem_rider.EM_Percent = 0;
                        prem_rider.EM_Premium = 0;
                        prem_rider.EF_Rate = 0;
                        prem_rider.EF_Premium = 0;
                        prem_rider.Total_EF_Year = 0;
                        prem_rider.EM_Amount = Convert.ToDouble(arr_riders[3].ToString());
                        prem_rider.Discount_Amount = Convert.ToDouble(arr_riders[4].ToString());
                        prem_rider.Original_Amount = Convert.ToDouble(arr_riders[5].ToString());
                        prem_rider.Created_On = DateTime.Now;
                        prem_rider.Created_By = System.Web.Security.Membership.GetUser().UserName;
                        prem_rider.Level = Convert.ToInt32(arr_riders[6].ToString());

                        person.Person_ID = Helper.GetNewGuid("SP_Check_App_Info_Person_Sub_ID", "@Person_ID").ToString();
                        person.App_Register_ID = App_Register_ID;
                        person.Level = Convert.ToInt32(arr_riders[6].ToString());
                        person.ID_Card = arr_riders[7].ToString();
                        person.ID_Type = Convert.ToInt32(arr_riders[8].ToString());
                        person.First_Name = arr_riders[9].ToString();
                        person.Last_Name = arr_riders[10].ToString();
                        person.Gender = Convert.ToInt32(arr_riders[11].ToString());
                        person.Birth_Date = Helper.FormatDateTime(arr_riders[12].ToString());
                        person.Country_ID = arr_riders[13].ToString();
                        person.Khmer_First_Name = arr_riders[14].ToString();
                        person.Khmer_Last_Name = arr_riders[15].ToString();
                        person.Father_First_Name = "";
                        person.Father_Last_Name = "";
                        person.Mother_First_Name = "";
                        person.Mother_Last_Name = "";
                        person.Prior_First_Name = "";
                        person.Prior_Last_Name = "";
                        person.Marital_Status = "";
                        Relationship = arr_riders[16].ToString();
                        person.Relationship = Relationship;

                        //9. Insert new row policy premium riders
                        if (!da_policy.InsertPolicyPremiumRider(prem_rider))
                        {
                            return false;
                        }

                        //10. Insert new row info of riders to app info person sub
                        if (!da_application.InsertAppInfoPersonSub(person))
                        {
                            return false;
                        }

                    }
                    else
                    {
                        person.App_Register_ID = arr_riders[17].ToString();
                        prem_rider.Policy_ID = arr_riders[0].ToString();
                        prem_rider.Sum_Insure = Convert.ToDouble(arr_riders[1].ToString());
                        prem_rider.Premium = Convert.ToDouble(arr_riders[2].ToString());
                        prem_rider.EM_Percent = 0;
                        prem_rider.EM_Premium = 0;
                        prem_rider.EF_Rate = 0;
                        prem_rider.EF_Premium = 0;
                        prem_rider.Total_EF_Year = 0;
                        prem_rider.EM_Amount = Convert.ToDouble(arr_riders[3].ToString());
                        prem_rider.Discount_Amount = Convert.ToDouble(arr_riders[4].ToString());
                        prem_rider.Original_Amount = Convert.ToDouble(arr_riders[5].ToString());
                        prem_rider.Created_On = DateTime.Now;
                        prem_rider.Created_By = System.Web.Security.Membership.GetUser().UserName;
                        prem_rider.Level = Convert.ToInt32(arr_riders[6].ToString());
                        Relationship = arr_riders[16].ToString();
                        //9. Insert new row policy premium riders
                        if (!da_policy.InsertPolicyPremiumRider(prem_rider))
                        {
                            return false;
                        }
                    }

                }

            }
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [SaveRiders] in class [PolicyWebService], Detail: " + ex.Message);
        }
        return status;
    }

    [WebMethod]
    public List<bl_policy_flat_rate.bl_riders> GetRider(string policy_id)
    {
        List<bl_policy_flat_rate.bl_riders> rider_list = new List<bl_policy_flat_rate.bl_riders>();
        try
        {
            rider_list = da_policy_flat_rate.da_rider.GetRiderByPolicyID(policy_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetRider] in class [PolicyWebService], Detail: " + ex.Message);
        }

        return rider_list;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rider_info"></param>
    /// <returns></returns>
    /// 
    [WebMethod]
    public bool DeleteRiders(string riders)
    {
        bool status = false;
        try
        {
            string rider_id = "";

            string[] arr = riders.Split(';');
            foreach (string rider in arr)
            {
                if (rider.Trim() != "")
                {
                    rider_id += rider.Trim() + ',';
                }
            }

            rider_id = rider_id.Substring(0, rider_id.Length - 1);//.Replace(",","','");
            //rider_id = "'" + rider_id + "'";

            status = da_policy_flat_rate.da_rider.DeleteRiders(rider_id, user_name);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteRider] in class [PolicyWebService], Detail: " + ex.Message);
        }
        return status;
    }

    [WebMethod]
    public bool SaveBeneficiary(List<string> beneficiaries)
    {
        bool status = false;
        try
        {
            bl_policy_benefit_item obj = new bl_policy_benefit_item();
            foreach (string ben in beneficiaries)
            {
                if (ben.Trim() != "")
                {
                    string[] arr_str = ben.Split(';');
                    {
                        obj.Policy_ID = arr_str[0].ToString();
                        obj.Full_Name = arr_str[1].ToString();
                        obj.Relationship = arr_str[2].ToString();
                        obj.ID_Type = Convert.ToInt32(arr_str[3].ToString());
                        obj.ID_Card = arr_str[4].ToString();
                        obj.Percentage = Convert.ToInt32(arr_str[5].ToString());
                        obj.Seq_Number = 1;
                    }
                }

                //1. Insert new row into table policy benefit
                da_policy.InsertPolicyBenefit("", obj.Policy_ID, Membership.GetUser().UserName, DateTime.Now);

                //2. Insert new row into table policy benefit item
                status = da_policy.InsertPolicyBenefitItem(obj);
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SaveBeneficiary] in class [PolicyWebService], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }
    [WebMethod]
    public bool SaveLoan(List<string> loan)
    {
        bool status = false;
        bl_policy_flat_rate.bl_loan loanObj;
        try
        {
            if (loan.Count > 0)
            {
                string option = "";
                loanObj = new bl_policy_flat_rate.bl_loan();
                //loanObj.PolicyID = Helper.GetNewGuid(new string[,] { { "TABLE", "TBL_LOAN" }, { "FIELD", "POLICYLOANID" } });
                loanObj.LoanType = Convert.ToInt32(loan[0]);
                loanObj.InterestRate = Convert.ToDouble(loan[1]);
                loanObj.TermYear = Convert.ToInt32(loan[2]);
                loanObj.LoanEffectiveDate = Helper.FormatDateTime(loan[3]);
                loanObj.Loan = Convert.ToDouble(loan[4]);
                option = loan[5];
                if (option.ToLower() == "save")
                {
                    option = "insert";
                }
                else if (option == "update")
                {

                    option = "update";
                }
                loanObj.PolicyID = loan[6];
                loanObj.CreatedBy = user_name;
                loanObj.CreatedDateTime = DateTime.Now;
                loanObj.UpdatedBy = user_name;
                loanObj.UpdatedDateTime = DateTime.Now;
                status = da_policy_flat_rate.da_loan.SaveLoan(option, loanObj);
            }
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [SaveLoan] in class [PolicyWebService], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return status;
    }
    [WebMethod]
    public bl_policy_flat_rate.bl_loan GetLoan(string policyID)
    {
        return da_policy_flat_rate.da_loan.GetLoan(policyID);
    }
    [WebMethod]
    public List<bl_policy_flat_rate.bl_beneficiary> GetBeneficiary(string policy_id)
    {
        List<bl_policy_flat_rate.bl_beneficiary> arr_obj = new List<bl_policy_flat_rate.bl_beneficiary>();
        try
        {
            arr_obj = da_policy_flat_rate.da_beneficiary.GetBeneficiaryByPolicyID(policy_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetBeneficiary] in class [PolicyWebService], Detail: " + ex.Message);
        }

        return arr_obj;
    }
    [WebMethod]
    public bool DeleteBeneficiary(string beneficiary)
    {
        bool status = false;
        try
        {
            string ben_id = "";

            string[] arr = beneficiary.Split(';');
            foreach (string ben in arr)
            {
                if (ben.Trim() != "")
                {
                    ben_id += ben.Trim() + ',';
                }
            }

            ben_id = ben_id.Substring(0, ben_id.Length - 1);//.Replace(",","','");
            //rider_id = "'" + rider_id + "'";

            status = da_policy_flat_rate.da_beneficiary.DeleteBeneficiary(ben_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteBeneficiary] in class [PolicyWebService], Detail: " + ex.Message);
        }
        return status;
    }

    [WebMethod]
    public bool RollBack(string policy_id)
    {
        bool status = false;
        try
        {
            status = da_policy_flat_rate.RollBack(policy_id);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [RollBack] in class [PolicyWebService], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }

    [WebMethod]
    public List<bl_policy_flat_rate.bl_policy_customer_search> GetSearchPolicyCustomer(string policy_number, string app_number, string name, int gender, string id_card)
    {
        List<bl_policy_flat_rate.bl_policy_customer_search> obj_arr = new List<bl_policy_flat_rate.bl_policy_customer_search>();
        try
        {
            obj_arr = da_policy_flat_rate.GetPolicyCustomerSearch(policy_number, app_number, name, gender, id_card, "");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetSearchPolicyCustomer] in class [PolicyWebService], Detail: " + ex.Message);
        }
        return obj_arr;
    }
    [WebMethod]
    public string GetProductType(string productId)
    {
        string productType = "";
        try
        {
            bl_product product = da_product.GetProductByProductID(productId);
            productType = product.Product_Type_ID + "";
        }
        catch (Exception ex)
        {
            productType = "";
            Log.AddExceptionToLog("Error function [GetProductType] in class [PolicyWebService], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return productType;
    }
    #endregion

    #region //CI_Calculate_Renew_Policy
    [WebMethod]
    public List<string> CI_Calculate_Renew_Policy(string policy_id, string new_effecive_date)
    {

        List<string> result = new List<string>();
        int age = 0;
        DateTime maturity_date = new DateTime();
        DateTime expiry_date = new DateTime();
        DateTime effective_date = new DateTime();
        effective_date = Helper.FormatDateTime(new_effecive_date).Date;
        bl_ci.PolicyDetail pol_detail = new bl_ci.PolicyDetail();
        bl_ci.Policy pol = new bl_ci.Policy();
        bl_customer cust = new bl_customer();

        pol = da_ci.Policy.GetPolicy(policy_id);
        pol_detail = da_ci.PolicyDetail.GetLastPolicyDetail(pol.PolicyNumber);
        cust = da_customer.GetCustomerByCustomerID(pol.CustomerID);
        age = Calculation.Culculate_Customer_Age(cust.Birth_Date.ToString("dd/MM/yyyy"), effective_date.Date);
        maturity_date = effective_date.Date.AddYears(1);
        expiry_date = maturity_date.AddDays(-1);



        result.Add(age + "");
        result.Add(maturity_date.ToString("dd/MM/yyyy"));
        result.Add(expiry_date.ToString("dd/MM/yyyy"));
        return result;
    }

    #endregion

    #region // Send policies list which will be renew next month
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current_date">[dd/MM/yyyy]</param>
    /// <returns></returns>
    [WebMethod]
    public string SendRenewPolicy(string current_date)
    {
        string result = "";
        try
        {

            string body = "";
            DataTable tbl = da_ci.Policy.GetExpirePolicy(Helper.FormatDateTime(current_date));
            int index = 0;
            if (tbl.Rows.Count > 0)
            {
                body = "Below list is the policies which will be expired on " + Convert.ToDateTime(tbl.Rows[0]["expiry_date"].ToString()).ToString("dd/MM/yyyy") + "." + Environment.NewLine + "<table border=1><th>No.</th><th>Policy No.</th><th>Customer ID</th><th>Customer Name</th><th>Customer Name (KH)</th><th>Gender</th><th>DOB</th><th>Expire Date</th>";
                //Send sms
                SENDSMS sms;
                string title = "";
                string sms_log = "";
                string phone_number = "";
                foreach (DataRow row in tbl.Rows)
                {
                    index += 1;

                    /* Temporary disalbed send sms
                    if (row["gender"].ToString().Trim() == "0")
                    {
                        title = "Mrs.";
                    }
                    else
                    {
                        title = "Mr.";
                    }
                    phone_number = row["mobile_phone1"].ToString().Trim();
                    //Send SMS to inform customers
                    sms = new SENDSMS();
                    sms.Message = "Dear " + title + row["last_name"].ToString().Trim() + " " + row["first_name"].ToString().Trim() + ", your policy " + row["policy_number"].ToString().Trim() + " will be expired on " + Convert.ToDateTime(row["expiry_date"].ToString().Trim()).ToString("dd/mm/yyyy") + ". Please contact 061431111 for renewal.";
                    sms.MessageCate = "SO_RENEWAL";
                    sms.PhoneNumber =  phone_number;
                    try
                    {
                        if (sms.Send())
                        {
                            sms_log = "[Send Renewal]\t" + index + ". [SUCCESSFULLY]\t PHONE:[" + phone_number + "]";
                        }
                        else
                        {
                            sms_log = "[Send Renewal]\t" + index + ". [FAIL]\t PHONE:[" + phone_number + "]";
                        }
                    }
                    catch (Exception ex)
                    {
                        sms_log = "[Send Renewal]\t" + index + ". [FAIL]\t PHONE:[" + phone_number + "]\t ERROR:[" + ex.Message + "]";
                    }
                */

                    //makeup summary email body
                    body += "<tr><td>" + index + "</td><td>" + row["policy_number"].ToString() + "</td><td>" + row["customer_id"].ToString() + "</td><td>" + (row["last_name"].ToString() + " " + row["first_name"].ToString()) + "</td><td>" + (row["khmer_last_name"].ToString() + " " + row["Khmer_first_name"].ToString()) + "</td><td>" + (row["gender"].ToString() == "0" ? "F" : "M") + "</td><td>" +
                        Convert.ToDateTime(row["birth_date"].ToString()).ToString("dd/MM/yyyy") + "</td><td>" + Convert.ToDateTime(row["expiry_date"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>";
                    //save log sms
                    Log.CreateLog("SendCustomerNotificationSMS", sms_log);
                    //reset variable
                    phone_number = "";
                    sms_log = "";
                }
                body += "</table>";

                /*send summary email*/
                EmailSender mail = new EmailSender();
                mail.From = "";
                //mail.From = "policyholder@camlife.com.kh";
                mail.From = "camlifesys@camlife.com.kh";
                //mail.To = "maneth.som@camlife.com.kh";
                mail.To = "underwriting@camlife.com.kh";
                // mail.BCC = "maneth.som@camlife.com.kh";
                mail.Subject = "List Of Policies Will Be Expired In Next Month";
                mail.Message = body;
                mail.Host = "mail.camlife.com.kh";

                mail.Port = 587;
                mail.Password = "admin1!2cdef";

                string str_log = "";
                if (mail.SendMail(mail))
                {

                    str_log = "[List Of Policies Will Be Expired In Next Month] was sent to " + mail.To + " successfully.";
                    result = "Successfully";
                }
                else
                {
                    str_log = "[List Of Policies Will Be Expired In Next Month] was sent to " + mail.To + " fail.";
                    result = "Fail";
                }

                //save log file
                if (str_log.Trim() != "")
                {
                    Log.CreateLog("Email_Log", str_log);
                }


            }
            else
            {
                result = "There is no expired policy in next month.";
                Log.CreateLog("Email_Log", "[Send Renewal]\tThere is no expired policy in next month.");
            }
        }
        catch (Exception ex)
        {
            result = "Error";
            Log.AddExceptionToLog("Error function [SendRenewPolicy(string current_date)] in class [PolicyWebService], detail: " + ex.Message);
        }
        return result;
    }
    #endregion
    #region //Send over age of next month renewal
    [WebMethod]
    public string SendRenewalOverAge(string current_date)
    {
        string result = "";
        try
        {
            DataTable tbl = da_ci.Policy.GetOverAgeBeforeRenewal(Helper.FormatDateTime(current_date));
            string body = "";

            int index = 0;
            if (tbl.Rows.Count > 0)
            {
                body = "Below list is the policies which will be over ages and renew on " + Convert.ToDateTime(tbl.Rows[0]["MATURITY_DATE"].ToString().Trim()).ToString("dd/MM/yyyy") + "." + Environment.NewLine + "<table border=1><th>No.</th><th>Policy No.</th><th>Customer ID</th><th>Customer Name</th><th>Customer Name (KH)</th><th>Gender</th><th>DOB</th><th>Expire Date</th><th>Age</th><th>Remarks</th>";
                foreach (DataRow row in tbl.Rows)
                {
                    index += 1;

                    body += "<tr><td>" + index + "</td><td>" + row["policy_number"].ToString() + "</td><td>" + row["customer_id"].ToString() + "</td><td>" + (row["last_name"].ToString() + " " + row["first_name"].ToString()) + "</td><td>" + (row["khmer_last_name"].ToString() + " " + row["Khmer_first_name"].ToString()) + "</td><td>" + (row["gender"].ToString() == "0" ? "F" : "M") + "</td><td>" +
                        Convert.ToDateTime(row["birth_date"].ToString()).ToString("dd/MM/yyyy") + "</td><td>" + Convert.ToDateTime(row["expiry_date"].ToString()).ToString("dd/MM/yyyy") + "</td><td>" + row["Age"].ToString() + "</td><td>" + row["age_remarks"].ToString() + "</td></tr>";

                }
                body += "</table>";

                /*send summary email*/
                EmailSender mail = new EmailSender();
                mail.From = "";
                //mail.From = "policyholder@camlife.com.kh";
                mail.From = "camlifesys@camlife.com.kh";
                //mail.To = "maneth.som@camlife.com.kh";
                mail.To = "underwriting@camlife.com.kh";
                // mail.BCC = "maneth.som@camlife.com.kh";
                mail.Subject = "List Of Policies Will Be Over Age In Next Month";
                mail.Message = body;
                mail.Host = "mail.camlife.com.kh";

                mail.Port = 587;
                mail.Password = "admin1!2cdef";

                string str_log = "";
                if (mail.SendMail(mail))
                {

                    str_log = "[List Of Policies Will Be Over Age In Next Month] was sent to " + mail.To + " successfully.";
                    result = "Successfully";
                }
                else
                {
                    str_log = "[List Of Policies Will Be Over Age In Next Month] was sent to " + mail.To + " fail.";
                    result = "Fail";
                }

                //save log file
                if (str_log.Trim() != "")
                {
                    Log.CreateLog("Email_Log", str_log);
                }
            }
            else
            {
                result = "There is no over ages policies for renewal in next month.";
                Log.CreateLog("Email_Log", "[Send Over Ages]\tThere is no over ages policies for renewal in next month.");
            }
        }
        catch (Exception ex)
        {
            result = "Error";
            Log.AddExceptionToLog("Error function [SendRenewalOverAge(string current_date)] in class [PolicyWebService], detail:" + ex.Message);
        }
        return result;
    }
    #endregion

    [WebMethod]
    public string SendRenewPolicy_RenewalOverAge()
    {
        string result = "";
        result = SendRenewPolicy(DateTime.Now.ToString("dd/MM/yyyy"));
        result += SendRenewalOverAge(DateTime.Now.ToString("dd/MM/yyyy"));
        return result;
    }

    [WebMethod]
    public string EDITRemark(string policy_number)
    {
        string editRemark = "";
        if (policy_number != "")
        {
            editRemark = da_policy_cl24.GetPolicyRemark(policy_number);
        }

        return editRemark;

    }

    [WebMethod]
    public string EDITStatusRemark(string policy_number, string pay_year)
    {
        string status = "", remark = "", paid_off_date = "";
        if (policy_number != "" && pay_year != "")
        {
            DataTable dt = da_policy_cl24.GetPolicyStatusRemark(policy_number, pay_year != "" ? Convert.ToInt16(pay_year) : 0);
            if (dt.Rows.Count > 0)
            {
                status = dt.Rows[0]["Status"].ToString();
                remark = dt.Rows[0]["Remark"].ToString();
                paid_off_date = dt.Rows[0]["PAID_OFF_DATE"].ToString();
            }
        }

        return status + "," + remark + "," + paid_off_date;
    }


    #region Unpaid 60 days
    [WebMethod]
    public string SendUnpaid60Days()
    {
        DataTable tbl = new DataTable();
        string result = "";
        try
        {
            tbl = da_ci.Policy.GetPolicyUnpaid60Days(DateTime.Now.Date);// (new DateTime(2020, 3, 1));
            int index = 0;
            string body = "";
            if (tbl.Rows.Count > 0)
            {
                body = "Below list is the policies which are unpaid for " + Environment.NewLine +
                    "<table border=1><th>No.</th><th>Policy No.</th><th>Product ID</th><th>Customer ID</th><th>Customer Name</th><th>Customer Name (KH)</th><th>Gender</th><th>DOB</th><th>Pay Mode</th><th>Last Due Date</th><th>Next Due Date</th><th>Unpaid Period</th><th>Premium</th><th>Agent Code</th><th>Agent Name</th>";
                foreach (DataRow row in tbl.Rows)
                {
                    index += 1;

                    body += "<tr><td>" + index + "</td><td>" + row["policy_number"].ToString() + "</td><td>" + row["product_id"].ToString() + "</td><td>" + row["customer_id"].ToString() + "</td><td>" + (row["last_name"].ToString() + " " + row["first_name"].ToString()) + "</td><td>" + (row["khmer_last_name"].ToString() + " " + row["Khmer_first_name"].ToString()) + "</td><td>" + (row["gender"].ToString() == "0" ? "F" : "M") + "</td><td>" +
                        Convert.ToDateTime(row["birth_date"].ToString()).ToString("dd/MM/yyyy") + "</td><td>" + Helper.GetPaymentModeEnglish(Convert.ToInt32(row["pay_mode_id"].ToString())) + "</td><td>" + Convert.ToDateTime(row["due_date"].ToString()).ToString("dd/MM/yyyy") + "</td><td>" + Convert.ToDateTime(row["next_due_date"].ToString()).ToString("dd/MM/yyyy") + "</td><td>" + row["period"].ToString() + " Days </td><td>" + row["premium"] + " USD</td><td>" + row["agent_code"] + "</td><td>" + row["agent_name"] + "</td></tr>";

                }
                body += "</table>";

                /*send summary email*/
                EmailSender mail = new EmailSender();
                mail.From = "";
                //mail.From = "policyholder@camlife.com.kh";
                mail.From = "camlifesys@camlife.com.kh";
                // mail.To = "maneth.som@camlife.com.kh";
                mail.To = "underwriting@camlife.com.kh";
                //mail.BCC = "ict@camlife.com.kh";
                mail.Subject = "Policies Unpaid 60 Days.";
                mail.Message = body;
                mail.Host = "mail.camlife.com.kh";

                mail.Port = 587;
                mail.Password = "admin1!2cdef";

                string str_log = "";
                if (mail.SendMail(mail))
                {

                    str_log = "[List Of Policies Unpaid 60 Days.] was sent to " + mail.To + " successfully.";
                    result = "Successfully";
                }
                else
                {
                    str_log = "[List Of Policies Unpaid 60 Days.] was sent to " + mail.To + " fail.";
                    result = "Fail";
                }

                //save log file
                if (str_log.Trim() != "")
                {
                    Log.CreateLog("Email_Log", str_log);
                }
            }
            else
            {
                result = "There is no policies unpaid 60 days.";
                Log.CreateLog("Email_Log", "[Send Policies Unpaid 60 Days]\tThere is no policies unpaid 60 days.");
            }
        }
        catch (Exception ex)
        {
            result = "Ooop! System is getting something wrong.";
            Log.AddExceptionToLog("Error function [SendUnpaid60Days()], in class [PolicyWebService], detail:" + ex.Message);
        }
        return result;
    }
    #endregion
}
