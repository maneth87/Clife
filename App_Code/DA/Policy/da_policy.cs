using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

/// <summary>
/// Summary description for da_policy
/// </summary>
public class da_policy
{
    private static da_policy mytitle = null;

    #region "Constructors"

    public da_policy()
    {
        if (mytitle == null)
        {
            mytitle = new da_policy();
        }
    }

    #endregion

    #region "Public Functions"

    //Insert new policy then return policy ID
    public static string InsertPolicy(string app_register_id, string customer_id, string address_id, string created_by, DateTime created_on)
    {

        string policy_id = "";
        string temp_id = "";

        bl_underwriting my_data = da_underwriting.GetUnderwritingObject(app_register_id);

        //Create new customer and get Customer_ID        
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy";

            //get new primary key for the row to be inserted
            temp_id = Helper.GetNewGuid("SP_Check_Policy_ID", "@Policy_ID").ToString();

            cmd.Parameters.AddWithValue("@Policy_ID", temp_id);
            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);
            cmd.Parameters.AddWithValue("@Product_ID", my_data.Product_ID);
            cmd.Parameters.AddWithValue("@Effective_Date", my_data.Effective_Date);
            cmd.Parameters.AddWithValue("@Maturity_Date", my_data.Maturity_Date);
            cmd.Parameters.AddWithValue("@Agreement_Date", my_data.App_Date);
            cmd.Parameters.AddWithValue("@Issue_Date", created_on); //change to datetime.now use created_on when inserting data
            cmd.Parameters.AddWithValue("@Age_Insure", my_data.Age_Insure);
            cmd.Parameters.AddWithValue("@Pay_Year", my_data.Pay_Year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", my_data.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", my_data.Assure_Year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", my_data.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Address_ID", address_id);
            cmd.Parameters.AddWithValue("@Is_Standard", 1); //modify code to select from table uw_co to verify if the case is std or em

            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now); //change to datetime.now use created_on when inserting data
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@Created_Note", "");


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                policy_id = temp_id;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicy(string app_register_id, string customer_id, string address_id, string created_by, DateTime created_on)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return policy_id;
    }

    #region By Meas Sun On 2019/11/28
    //Insert new policy then return policy ID
    public static string InsertPolicy(bl_policy policy)
    {
        string policy_id = "";
        string temp_id = "";

        //Create new customer and get Customer_ID        
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy";

            //get new primary key for the row to be inserted
            temp_id = Helper.GetNewGuid("SP_Check_Policy_ID", "@Policy_ID").ToString();

            cmd.Parameters.AddWithValue("@Policy_ID", temp_id);
            cmd.Parameters.AddWithValue("@Customer_ID", policy.Customer_ID);
            cmd.Parameters.AddWithValue("@Product_ID", policy.Product_ID);
            cmd.Parameters.AddWithValue("@Effective_Date", policy.Effective_Date);
            cmd.Parameters.AddWithValue("@Maturity_Date", policy.Maturity_Date);
            cmd.Parameters.AddWithValue("@Agreement_Date", policy.Agreement_Date);
            cmd.Parameters.AddWithValue("@Issue_Date", policy.Issue_Date);
            cmd.Parameters.AddWithValue("@Age_Insure", policy.Age_Insure);
            cmd.Parameters.AddWithValue("@Pay_Year", policy.Pay_Year);
            cmd.Parameters.AddWithValue("@Pay_Up_To_Age", policy.Pay_Up_To_Age);
            cmd.Parameters.AddWithValue("@Assure_Year", policy.Assure_Year);
            cmd.Parameters.AddWithValue("@Assure_Up_To_Age", policy.Assure_Up_To_Age);
            cmd.Parameters.AddWithValue("@Address_ID", policy.Address_ID);
            cmd.Parameters.AddWithValue("@Is_Standard", 1);
            //cmd.Parameters.AddWithValue("@Channel_Item_ID", policy.ChannelItem_ID);
            cmd.Parameters.AddWithValue("@Created_On", policy.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", policy.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", policy.Created_Note);


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                policy_id = temp_id;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicy(bl_policy policy)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return policy_id;
    }
    #endregion
    //Get app info address record by app_register_id
    public static bl_app_info_address GetAppInfoAddress(string app_register_id)
    {
        bl_app_info_person my_app_info_person = new bl_app_info_person();

        bl_app_info_address my_app_info_address = new bl_app_info_address();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_App_Info_Address_By_App_Register_ID";

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
                        my_app_info_address.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                        my_app_info_address.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                        my_app_info_address.Address2 = rdr.GetString(rdr.GetOrdinal("Address2"));
                        my_app_info_address.Address3 = rdr.GetString(rdr.GetOrdinal("Address3"));
                        my_app_info_address.Province = rdr.GetString(rdr.GetOrdinal("Province"));
                        my_app_info_address.Country_ID = rdr.GetString(rdr.GetOrdinal("Country_ID"));
                        my_app_info_address.Zip_Code = rdr.GetString(rdr.GetOrdinal("Zip_Code"));
                    }
                }
            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetAppInfoAddress(string app_register_id)] in class [da_policy]. Details: " + ex.Message);
            }
            con.Close();
        }

        return my_app_info_address;
    }

    //Insert new policy address and return adddress ID
    public static string InsertPolicyAddress(string app_register_id)
    {

        string temp_id = "";
        string policy_address_id = "";

        //Get a row from table app_info_address by passing app_register_id parameter
        bl_app_info_address my_app_info_address = GetAppInfoAddress(app_register_id);

        //Check whether address already exist
        //bool check_address = CheckExistingPolicyAddress(my_app_info_address.Address1, my_app_info_address.Address2, my_app_info_address.Address3);

        bool check_address = false;

        //If not exist, add new row
        if (check_address == false)
        {
            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Insert_Policy_Address";

                //get new primary key for the row to be inserted
                temp_id = Helper.GetNewGuid("SP_Check_Policy_Address_ID", "@Address_ID").ToString();

                cmd.Parameters.AddWithValue("@Address_ID", temp_id);
                cmd.Parameters.AddWithValue("@Country_ID", my_app_info_address.Country_ID);
                cmd.Parameters.AddWithValue("@Address1", my_app_info_address.Address1);
                cmd.Parameters.AddWithValue("@Address2", my_app_info_address.Address2);
                cmd.Parameters.AddWithValue("@Address3", my_app_info_address.Address3);
                cmd.Parameters.AddWithValue("@Province", my_app_info_address.Province);
                cmd.Parameters.AddWithValue("@Zip_Code", my_app_info_address.Zip_Code);

                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                    policy_address_id = temp_id;
                }
                catch (Exception ex)
                {
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [InsertPolicyAddress(string app_register_id)] in class [da_policy]. Details: " + ex.Message);
                }
            }

        }
        else //If address already exist
        {
            //Get exisitng address_id
            policy_address_id = GetExistingPolicyAddress(my_app_info_address.Address1, my_app_info_address.Address2, my_app_info_address.Address3);

        }

        return policy_address_id;
    }

    #region By Meas Sun On 2019/11/20
    public static bool InsertPolicyAddress(bl_policy_address address)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Insert_Policy_Address", new string[,]{
        {"@Address_ID", address.Address_ID},
        {"@Country_ID", address.Country_ID},
        {"@Address1", address.Address1},
        {"@Address2", address.Address2},
        {"@Address3", address.Address3},
        {"@Province", address.Province},
        {"@Zip_Code", address.Zip_Code}
        }, "Function [InsertPolicyAddress(bl_policy_address address)], class [da_policy]");


        }
        catch (Exception ex)
        {
            result = false;
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertPolicyAddress(bl_policy_address address)] in class [da_policy]. Details: " + ex.Message);
        }
        return result;
    }
    #endregion
    /// <summary>
    /// Update policy contact by address ID
    /// </summary>
    /// <param name="address">Address class object</param>
    /// <returns></returns>
    public static bool UpdatePolicyAddress(bl_policy_address address)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_POLICY_ADDRESS_UPDATE", new string[,]{
        {"@Address_ID", address.Address_ID},
        {"@Country_ID", address.Country_ID},
        {"@Address1", address.Address1},
        {"@Address2", address.Address2},
        {"@Address3", address.Address3},
        {"@Province", address.Province},
        {"@Zip_Code", address.Zip_Code}
        }, "da_policy=>UpdatePolicyAddress(bl_policy_address address)");


        }
        catch (Exception ex)
        {
            result = false;
            //Add error to log 
            Log.AddExceptionToLog("Error in function [UpdatePolicyAddress(bl_policy_address address)], details: " + ex.Message + "=>" + ex.StackTrace + "=>"+ ex.InnerException);
        }
        return result;
    }
    public static bl_policy_address GetAddress(string address_id)
    {
        bl_policy_address add = new bl_policy_address();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_POLICY_ADDRESS_GET", new string[,] {
            {"@address_id", address_id}
            }, "da_policy=>GetAddress(string address_id)");
            foreach (DataRow row in tbl.Rows)
            {
                add.Address_ID = row["address_id"].ToString();
                add.Country_ID = row["country_id"].ToString();
                add.Address1 = row["address1"].ToString();
                add.Address2 = row["address2"].ToString();
                add.Address3 = row["address3"].ToString();
                add.Province = row["province"].ToString();
                add.Zip_Code = row["zip_code"].ToString();
                break;
            }
        }
        catch (Exception ex)
        {
            add = new bl_policy_address();
            Log.AddExceptionToLog("Error in function [GetAddress(string address_id)], details: " + ex.Message + "=>" + ex.StackTrace + "=>" + ex.InnerException);

        }
        return add;
    }

    //Insert new policy details: policy premium, policy premium pay, policy contact, policy pay mode, policy number, app policy
    public static bool InsertPolicyDetails(string app_register_id, string policy_id, string created_by, DateTime created_on)
    {
        bool insert = false;

        bl_underwriting my_data = da_underwriting.GetUnderwritingObject(app_register_id);

        try
        {
            if (my_data.Product_ID.Substring(0, 3).ToUpper() == "NFP" || my_data.Product_ID.Substring(0, 3).ToUpper() == "FPP" || my_data.Product_ID.Substring(0, 3).ToUpper() == "SDS") // New product, family protection, study save
            {
                //1.policy premium params: policy_id, sum_id, premium, em_percent, em_premium, ef_rate, ef_premium, total_ef_year, created_on, created_by, created_note, original_amount, em_amount
                InsertPolicyPremium(my_data, policy_id, created_by, created_on);

                // insert policy premium rider & rider status in table ct_policy_status_rider
                InsertPolicyPremiumRider(app_register_id, policy_id, created_by, created_on);

                //2.policy premium pay
                InsertPolicyPremiumPay(app_register_id, policy_id, my_data.Sale_Agent_ID, my_data.Office_ID, created_by, created_on);

                //3.policy contact
                InsertPolicyContact(app_register_id, policy_id);

                //4.policy number
                InsertPolicyNumber(policy_id);

                //5.app policy
                InsertAppPolicy(app_register_id, policy_id);

                //6.policy pay mode - first due date & month is the extract of policy effective date
                InsertPolicyPayMode(app_register_id, policy_id, my_data.Pay_Mode, my_data.Effective_Date, created_by, created_on);

                //7.policy benefit
                InsertPolicyBenefit(my_data.Benefit_Note, policy_id, created_by, created_on);

                //8.policy benefit item
                InsertPolicyBenefitItem(app_register_id, policy_id);  //change relationship status from english to khmer in app_benefit_item

                //9.policy status
                InsertPolicyStatus(policy_id, created_by, created_on);
            }
            //Old product
            else
            {
                //Old 
                my_data.Old_Sum_Insure = my_data.System_Sum_Insure;
                my_data.Old_Premium = my_data.System_Premium;
                my_data.Old_Original_Amount = my_data.Original_Amount;

                //1.policy premium params: policy_id, sum_id, premium, em_percent, em_premium, ef_rate, ef_premium, total_ef_year, created_on, created_by, created_note, original_amount, em_amount
                InsertPolicyPremium(my_data, policy_id, created_by, created_on);

                ////2.policy premium pay
                //InsertPolicyPremiumPay(app_register_id, policy_id, my_data.Sale_Agent_ID, my_data.Office_ID, created_by, created_on);

                //3.policy contact
                InsertPolicyContact(app_register_id, policy_id);

                #region check credit life 24% <By: Maneth Date:20122017>
                //if product is credit life not insert insert policy number into ct_policy_number table
                //but insert into ct_group_master_policy
                List<bl_group_master_product> gMasterProductList = da_group_master_product.GetGroupMasterProductList(my_data.Product_ID.Trim(), "");
                string group_code = ""; string main_policy_number = "";

                if (gMasterProductList.Count > 0)
                {
                    var gMasterProduct = gMasterProductList[0];
                    bl_group_master_policy gMasterPolicy = new bl_group_master_policy();
                    gMasterPolicy.PolicyID = policy_id;
                    gMasterPolicy.GroupPolicyID = Helper.GetNewGuid("SP_Check_GroupPolicy_ID", "@Group_Policy_ID").ToString(); 
                    gMasterPolicy.GroupMasterID = gMasterProduct.GroupMasterID;
                    gMasterPolicy.Remarks = "";
                    gMasterPolicy.CreatedBY = System.Web.Security.Membership.GetUser().UserName;
                    gMasterPolicy.CreatedDateTime = DateTime.Now;
                    gMasterPolicy.MainPolicyNumber = ""; 

                    //Check Reserve Policy Number
                    #region Reserve Policy
                    bl_app_reserve_policy reserve_policy = new bl_app_reserve_policy();
                    reserve_policy = da_application.GetReservePolicyByAppNumber(my_data.App_Number);

                    #endregion

                    if (reserve_policy.Policy_Number != null && reserve_policy.Customer_ID != null)
                        gMasterPolicy.SeqNumber = reserve_policy.Policy_Number;
                    else
                        gMasterPolicy.SeqNumber = da_group_master_policy.GetNewSeqNumber(gMasterProduct.GroupMasterID);

                    da_group_master_policy.InsertGroupMasterPolicy(gMasterPolicy);
                    main_policy_number = gMasterPolicy.SeqNumber; group_code = gMasterProduct.GroupCode;

                }
                else
                {
                    //4.policy number
                    InsertPolicyNumber(policy_id);
                }
                #endregion

                //5.app policy
                InsertAppPolicy(app_register_id, policy_id);

                //6.policy pay mode - first due date & month is the extract of policy effective date
                InsertPolicyPayMode(app_register_id, policy_id, my_data.Pay_Mode, my_data.Effective_Date, created_by, created_on);

                //7.policy benefit
                InsertPolicyBenefit(my_data.Benefit_Note, policy_id, created_by, created_on);

                //8.policy benefit item
                InsertPolicyBenefitItem(app_register_id, policy_id);  //change relationship status from english to khmer in app_benefit_item

                //9.policy status
                InsertPolicyStatus(policy_id, created_by, created_on);

                //10.GENERATE NEW ISSUE FF PREMIUM
                string my_policy_number = ""; DataTable tbl = new DataTable();

                my_policy_number = group_code + "-" + main_policy_number;
                tbl = da_policy_cl24.GenerateNewIssueFirstFinancePremium(my_policy_number);

                if (tbl.Rows.Count > 0)
                {
                    //12.INSERT NEW TO CT_CL24_RENEWAL_PREMIUM (1st ISSUE)
                    da_policy_cl24.InsertNewIssueFirstFinancePremium(tbl, created_by);
                }

            }


        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertPolicyDetails(string app_register_id, string policy_id, string created_by, DateTime created_on)] in class [da_policy]. Details: " + ex.Message);
        }


        return insert;
    }

    //Insert Group Master Policy
    
    public static string InsertGroupMasterPolicy(string ProductID, string PolicyID, string GPolicy_ID, string main_policy_num, string ChannelItemID)
    {
        string result = "";
        List<bl_group_master_product> gMasterProductList = da_group_master_product.GetGroupMasterProductList(ProductID.Trim(), ChannelItemID);
        if (gMasterProductList.Count > 0)
        {
            var gMasterProduct = gMasterProductList[0];
            bl_group_master_policy gMasterPolicy = new bl_group_master_policy();
            gMasterPolicy.PolicyID = PolicyID;
            gMasterPolicy.GroupPolicyID = GPolicy_ID;
            gMasterPolicy.GroupMasterID = gMasterProduct.GroupMasterID;
            gMasterPolicy.Remarks = "";
            gMasterPolicy.CreatedBY = System.Web.Security.Membership.GetUser().UserName;
            gMasterPolicy.CreatedDateTime = DateTime.Now;
            gMasterPolicy.SeqNumber = da_group_master_policy.GetNewSeqNumber(gMasterProduct.GroupMasterID);
            gMasterPolicy.MainPolicyNumber = main_policy_num; 

            if (da_group_master_policy.InsertGroupMasterPolicy(gMasterPolicy))
                result = gMasterPolicy.SeqNumber;
        }

        return result;
    }

    public static string InsertGroupMasterPolicyReserved(string ProductID, string PolicyID, string GPolicyID, string seq_number, string main_policy_num, string ChannelItemID)
    {
        string result = "";
        List<bl_group_master_product> gMasterProductList = da_group_master_product.GetGroupMasterProductList(ProductID.Trim(), ChannelItemID);
        if (gMasterProductList.Count > 0)
        {
            var gMasterProduct = gMasterProductList[0];
            bl_group_master_policy gMasterPolicy = new bl_group_master_policy();
            gMasterPolicy.PolicyID = PolicyID;
            gMasterPolicy.GroupPolicyID = GPolicyID;
            gMasterPolicy.GroupMasterID = gMasterProduct.GroupMasterID;
            gMasterPolicy.Remarks = "";
            gMasterPolicy.CreatedBY = System.Web.Security.Membership.GetUser().UserName;
            gMasterPolicy.CreatedDateTime = DateTime.Now;
            gMasterPolicy.SeqNumber = seq_number;
            gMasterPolicy.MainPolicyNumber = main_policy_num;

            if (da_group_master_policy.InsertGroupMasterPolicy(gMasterPolicy))
            {
                result = da_group_master_policy.GetSubNewSeqNumber(seq_number);
            }
        }

        return result;
    }

    public static string InsertGroupMasterPolicyForSub(string ProductID, string PolicyID, string GPolicy_ID, string main_policy_num, string ChannelItemID)
    {
        string result = "";
        List<bl_group_master_product> gMasterProductList = da_group_master_product.GetGroupMasterProductList(ProductID.Trim(), ChannelItemID);
        if (gMasterProductList.Count > 0)
        {
            var gMasterProduct = gMasterProductList[0];
            bl_group_master_policy gMasterPolicy = new bl_group_master_policy();
            gMasterPolicy.PolicyID = PolicyID;
            gMasterPolicy.GroupPolicyID = GPolicy_ID;
            gMasterPolicy.GroupMasterID = gMasterProduct.GroupMasterID;
            gMasterPolicy.Remarks = "";
            gMasterPolicy.CreatedBY = System.Web.Security.Membership.GetUser().UserName;
            gMasterPolicy.CreatedDateTime = DateTime.Now;
            gMasterPolicy.SeqNumber = da_group_master_policy.GetNewSeqNumber(gMasterProduct.GroupMasterID);
            gMasterPolicy.MainPolicyNumber = main_policy_num;

            if (da_group_master_policy.InsertGroupMasterPolicy(gMasterPolicy))
                result = gMasterPolicy.SeqNumber;
        }

        return result;
    }

    public static bool CheckExistingAppNumber(string App_Number)
    {
        bool status = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CHECK_EXIST_APP_NUMBER";
            cmd.Parameters.AddWithValue("@App_Number", App_Number);
            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [CheckExistingAppNumber(string App_Number)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return status;
    }

    public static bool CheckExistingGroupMasterPolicy(string Seq_Number)
    {
        bool status = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CHECK_EXIST_GROUP_MASTER_POLICY";
            cmd.Parameters.AddWithValue("@Seq_Number", Seq_Number);
            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [CheckExistingMasterPolicy(string policy_number)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return status;
    }

    public static bool CheckExistingCustomerID(string Customer_ID)
    {
        bool status = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CHECK_EXIST_CUSTOMER_ID";
            cmd.Parameters.AddWithValue("@Customer_ID", Customer_ID);
            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [CheckExistingCustomerID(string Customer_ID)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return status;
    }

    public static bool CheckExistingReservePolicy(string Seq_Number)
    {
        bool status = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CHECK_EXIST_RESERVE_POLICY";
            cmd.Parameters.AddWithValue("@Seq_Number", Seq_Number);
            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in function [CheckExistingReservePolicy(string Seq_Number)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return status;
    }

    //Get discount
    public static string GetDiscount(string app_register_id)
    {
        string discount = da_underwriting.GetDiscount(app_register_id);
        return discount.ToString();
    }


    public static string GetDiscountFromPolicyPremium(string policy_id)
    {
        double discount = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Discount_By_Policy_ID";

            cmd.Parameters.AddWithValue("@policy_id", policy_id);

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
                Log.AddExceptionToLog("Error in function [GetDiscountFromPolicyPremium(string policy_id)] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return discount.ToString();
    }


    //Insert wing account for existing customer
    public static bool InsertPolicyWingAccount(string policy_id, string policy_number, string customer_id, string customer_name, string gender, string id_type, string id_number, DateTime birth_date, string contact_number, string wing_sk, string created_by)
    {
        bool result = false;

        string policy_wing_id;

        string type_of_id = "N/A";

        switch (id_type)
        {
            case "0":
                type_of_id = "ID Card";
                break;
            case "1":
                type_of_id = "Passport";
                break;
            case "2":
                type_of_id = "Visa";
                break;
            case "3":
                type_of_id = "Birth Certificate";
                break;
        }
        //Create new customer and get Customer_ID        
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_WING";

            //get new primary key for the row to be inserted
            policy_wing_id = Helper.GetNewGuid("SP_Check_Policy_WING_ID", "@Policy_WING_ID").ToString();

            cmd.Parameters.AddWithValue("@Policy_WING_ID", policy_wing_id);
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Number", policy_number);

            cmd.Parameters.AddWithValue("@Customer_ID", customer_id);
            cmd.Parameters.AddWithValue("@Customer_Name", customer_name);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@ID_Type", type_of_id);
            cmd.Parameters.AddWithValue("@ID_Number", id_number);
            cmd.Parameters.AddWithValue("@Birth_Date", birth_date);
            cmd.Parameters.AddWithValue("@Contact_Number", contact_number);


            cmd.Parameters.AddWithValue("@WING_SK", wing_sk);
            cmd.Parameters.AddWithValue("@WING_Number", GetWingNumberBySK(wing_sk));

            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyWingAccount(string policy_id, string policy_number, string customer_id, string customer_name, string gender, string id_type, string id_number, DateTime birth_date, string contact_number, string wing_sk, string created_by)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return result;

    }


    public static bool UpdatePolicyWingAccount(bl_policy_wing my_policy)
    {
        bool result = false;

        //Create new customer and get Customer_ID        
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_Policy_WING";

            cmd.Parameters.AddWithValue("@Policy_WING_ID", my_policy.Policy_WING_ID);
            cmd.Parameters.AddWithValue("@Policy_ID", my_policy.Policy_ID);
            cmd.Parameters.AddWithValue("@Policy_Number", my_policy.Policy_Number);

            cmd.Parameters.AddWithValue("@Customer_Name", my_policy.Customer_Name);
            cmd.Parameters.AddWithValue("@Gender", my_policy.Gender);
            cmd.Parameters.AddWithValue("@ID_Type", my_policy.ID_Type);
            cmd.Parameters.AddWithValue("@ID_Number", my_policy.ID_Number);
            cmd.Parameters.AddWithValue("@Birth_Date", my_policy.Birth_Date);
            cmd.Parameters.AddWithValue("@Contact_Number", my_policy.Contact_Number);


            cmd.Parameters.AddWithValue("@WING_SK", my_policy.WING_SK);
            cmd.Parameters.AddWithValue("@WING_Number", my_policy.WING_Number);

            cmd.Parameters.AddWithValue("@Created_On", my_policy.Created_On);
            cmd.Parameters.AddWithValue("@Created_By", my_policy.Created_By);


            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdatePolicyWingAccount(bl_policy_wing my_policy)] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return result;

    }

    //Set status of wing account to 0
    public static void UpdateWINGAccountStatus(string wing_sk)
    {

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Update_Wing_Status";

            cmd.Parameters.AddWithValue("@Wing_SK", wing_sk);

            cmd.Connection = con;
            con.Open();
            try
            {

                SqlDataReader rdr = cmd.ExecuteReader();
                con.Close();

            }

            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [UpdateWINGAccountStatus(string wing_sk)] in class [da_policy]. Details: " + ex.Message);
            }
            con.Close();
        }

    }


    //Get wing number by wing SK
    public static string GetWingNumberBySK(string wing_sk)
    {

        string num = "";

        string connString = AppConfiguration.GetConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Get_Wing_Number_By_SK";

                cmd.Parameters.AddWithValue("@WING_SK", wing_sk);
                cmd.Connection = con;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {

                        num = rdr.GetString(rdr.GetOrdinal("Wing_Number"));

                    }
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetWingNumberBySK(string wing_sk)] in class [da_policy]. Details: " + ex.Message);
        }

        return num;

    }

    //Insert new row into table policy premium (1)
    public static void InsertPolicyPremium(bl_underwriting my_data, string policy_id, string created_by, DateTime created_on, int count = 1)
    {
        //get underwriting counter offer object
        //bl_underwriting_co my_data =  da_underwriting.GetUnderwritingCOByAppID(app_register_id);

        bl_underwriting_co my_uw_co = GetUnderwritingCOData(my_data.App_Register_ID);

        //Total EM_Prem
        double EM_Premium = Math.Round(my_uw_co.EM_Premium,2);
        double EM_Amount = my_uw_co.EM_Amount;

        //Get discount
        double discount_amount = Convert.ToDouble(da_underwriting.GetDiscount(my_data.App_Register_ID));

        if (my_data.Product_ID == "CL24") //First Finance
        {
            my_uw_co.EM_Premium = Math.Round(my_uw_co.EM_Premium / count, 2);
            my_uw_co.EM_Amount = Math.Round(my_uw_co.EM_Amount / count, 2); 
            discount_amount = discount_amount / count;
        }

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Premium";

            //params: policy_id, sum_id, premium, em_percent, em_premium, ef_rate, ef_premium, total_ef_year, created_on, created_by, created_note, original_amount, em_amount, discount

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Sum_Insure", my_data.System_Sum_Insure);
            cmd.Parameters.AddWithValue("@Premium", my_data.System_Premium);
            cmd.Parameters.AddWithValue("@Original_Amount", my_data.Original_Amount);
            cmd.Parameters.AddWithValue("@EM_Percent", my_uw_co.EM_Percent);
            cmd.Parameters.AddWithValue("@EM_Premium", my_uw_co.EM_Premium);
            cmd.Parameters.AddWithValue("@EM_Amount", my_uw_co.EM_Amount);
            cmd.Parameters.AddWithValue("@EF_Rate", my_uw_co.EF_Rate); //no modi
            cmd.Parameters.AddWithValue("@EF_Premium", my_uw_co.EF_Premium);
            cmd.Parameters.AddWithValue("@Total_EF_Year", my_uw_co.Total_EF_Year);
            cmd.Parameters.AddWithValue("@Discount_Amount", discount_amount);
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now); 
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@Created_Note", "");
            cmd.Parameters.AddWithValue("@Round_Status_ID", my_uw_co.Round_Status_ID == null ? "" : my_uw_co.Round_Status_ID);

            //New Parameter
            //cmd.Parameters.AddWithValue("@Old_Sum_Insure", my_data.Old_Sum_Insure == my_data.Old_Sum_Insure == null ? 0 : my_data.Old_Sum_Insure);
            if (my_data.Old_Sum_Insure != 0)
                cmd.Parameters.AddWithValue("@Old_Sum_Insure", my_data.Old_Sum_Insure); 
            else 
                cmd.Parameters.AddWithValue("@Old_Sum_Insure", ""); 

            if (my_data.Old_Premium != 0)
                cmd.Parameters.AddWithValue("@Old_Premium", my_data.Old_Premium); 
            else  
                cmd.Parameters.AddWithValue("@Old_Premium", "");

            if (my_data.Old_Original_Amount != 0)
                cmd.Parameters.AddWithValue("@Old_Original_Amount", my_data.Old_Original_Amount);
            else 
                cmd.Parameters.AddWithValue("@Old_Original_Amount", "");

            if (my_uw_co.EM_Premium != 0)
                cmd.Parameters.AddWithValue("@Old_EM_Premium", EM_Premium); 
            else  
                cmd.Parameters.AddWithValue("@Old_EM_Premium", "");

            if (my_uw_co.EM_Amount != 0)
                cmd.Parameters.AddWithValue("@Old_EM_Amount", EM_Amount);
            else 
                cmd.Parameters.AddWithValue("@Old_EM_Amount", "");

            if (discount_amount != 0)
                cmd.Parameters.AddWithValue("@Old_Discount_Amount", discount_amount);
            else 
                cmd.Parameters.AddWithValue("@Old_Discount_Amount", "");

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyPremium] in class [da_policy]. Details: " + ex.Message);
            }
        }

    }

    #region By Meas Sun On 2019-12-03
    //Insert new row into table policy premium
    public static bool InsertPolicyPremium(bl_policy_premium policy_prem)
    {
        bool status = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Premium";

            //params: policy_id, sum_id, premium, em_percent, em_premium, ef_rate, ef_premium, total_ef_year, created_on, created_by, created_note, original_amount, em_amount, discount

            cmd.Parameters.AddWithValue("@Policy_ID", policy_prem.Policy_ID);
            cmd.Parameters.AddWithValue("@Sum_Insure", policy_prem.Sum_Insure);
            cmd.Parameters.AddWithValue("@Premium", policy_prem.Premium);
            cmd.Parameters.AddWithValue("@Original_Amount", policy_prem.Original_Amount);
            cmd.Parameters.AddWithValue("@EM_Percent", policy_prem.EM_Percent);
            cmd.Parameters.AddWithValue("@EM_Premium", policy_prem.EM_Premium);
            cmd.Parameters.AddWithValue("@EM_Amount", policy_prem.EM_Amount);
            cmd.Parameters.AddWithValue("@EF_Rate", policy_prem.EF_Rate);
            cmd.Parameters.AddWithValue("@EF_Premium", policy_prem.EF_Premium);
            cmd.Parameters.AddWithValue("@Total_EF_Year", policy_prem.Total_EF_Year);
            cmd.Parameters.AddWithValue("@Discount_Amount", policy_prem.Discount_Amount);
            cmd.Parameters.AddWithValue("@Created_On", policy_prem.Created_On); 
            cmd.Parameters.AddWithValue("@Created_By", policy_prem.Created_By);
            cmd.Parameters.AddWithValue("@Created_Note", "");
            cmd.Parameters.AddWithValue("@Round_Status_ID", "");

            //New Parameter
            //cmd.Parameters.AddWithValue("@Old_Sum_Insure", my_data.Old_Sum_Insure == my_data.Old_Sum_Insure == null ? 0 : my_data.Old_Sum_Insure);
            if (policy_prem.Sum_Insure != 0)
                cmd.Parameters.AddWithValue("@Old_Sum_Insure", policy_prem.Sum_Insure); //Old_Sum_Insure
            else
                cmd.Parameters.AddWithValue("@Old_Sum_Insure", "");

            if (policy_prem.Premium != 0)
                cmd.Parameters.AddWithValue("@Old_Premium", policy_prem.Premium); //Old_Premium
            else
                cmd.Parameters.AddWithValue("@Old_Premium", "");

            if (policy_prem.Original_Amount != 0)
                cmd.Parameters.AddWithValue("@Old_Original_Amount", policy_prem.Original_Amount); //Old_Original_Amount
            else
                cmd.Parameters.AddWithValue("@Old_Original_Amount", "");

            if (policy_prem.EM_Premium != 0)
                cmd.Parameters.AddWithValue("@Old_EM_Premium", policy_prem.EM_Premium); //Old_EM_Premium
            else
                cmd.Parameters.AddWithValue("@Old_EM_Premium", ""); 

            if (policy_prem.EM_Amount != 0)
                cmd.Parameters.AddWithValue("@Old_EM_Amount", policy_prem.EM_Amount); //Old_EM_Amount
            else
                cmd.Parameters.AddWithValue("@Old_EM_Amount", "");

            if (policy_prem.Discount_Amount != 0)
                cmd.Parameters.AddWithValue("@Old_Discount_Amount", policy_prem.Discount_Amount); //Old_Discount_Amount
            else
                cmd.Parameters.AddWithValue("@Old_Discount_Amount", "");

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                status = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyPremium] in class [da_policy]. Details: " + ex.Message);
                status = false;
            }
        }

        return status;

    }

    #endregion
    //Insert new row into table policy premium rider
    /// <summary>
    /// Funtion InsertPolicyPremiumRider[4 Arguments]
    /// Insert new row into table policy premium rider and policy_status at the same time
    /// This function also insert record into table policy status for rider (Ct_Policy_Status_Rider)
    /// </summary>
    /// <param name="app_register_id"></param>
    /// <param name="policy_id"></param>
    /// <param name="created_by"></param>
    /// <param name="created_on"></param>
    public static void InsertPolicyPremiumRider(string app_register_id, string policy_id, string created_by, DateTime created_on)
    {

        try
        {
            List<bl_underwriting_co_rider> myCo = new List<bl_underwriting_co_rider>();
            myCo = GetUnderwritingCORiderData(app_register_id);

            //Get discount
            double discount_amount = Convert.ToDouble(da_underwriting.GetDiscount(app_register_id));

            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                foreach (bl_underwriting_co_rider co in myCo)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_Insert_Policy_Premium_Rider";

                    cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
                    cmd.Parameters.AddWithValue("@Sum_Insure", co.System_Sum_Insure);
                    cmd.Parameters.AddWithValue("@Premium", co.System_Premium);
                    cmd.Parameters.AddWithValue("@Original_Amount", co.Original_Amount);
                    cmd.Parameters.AddWithValue("@EM_Percent", co.EM_Percent);
                    cmd.Parameters.AddWithValue("@EM_Premium", co.EM_Premium);
                    cmd.Parameters.AddWithValue("@EM_Amount", co.EM_Amount);
                    cmd.Parameters.AddWithValue("@EF_Rate", co.EF_Rate);
                    cmd.Parameters.AddWithValue("@EF_Premium", co.EF_Premium);
                    cmd.Parameters.AddWithValue("@Total_EF_Year", co.Total_EF_Year);
                    cmd.Parameters.AddWithValue("@Discount_Amount", discount_amount);
                    cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Created_By", created_by);
                    cmd.Parameters.AddWithValue("@Created_Note", "");
                    cmd.Parameters.AddWithValue("@Level", co.Level);


                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    con.Close();


                    //save rider status
                    InsertPolicyStatusRider(policy_id, created_by, DateTime.Now + "", co.Rider_Type, co.Level);

                }
            }

        }

        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertPolicyPremiumRider] in class [da_policy]. Details: " + ex.Message);
        }
    }
    /// <summary>
    /// Funtion InsertPolicyPremiumRider[5 Arguments]
    /// Insert new row into table policy premium rider and policy_status at the same time but base on level
    /// this function is usefull when user inforce application in underwriting for add riders.
    /// </summary>
    /// <param name="app_register_id"></param>
    /// <param name="policy_id"></param>
    /// <param name="created_by"></param>
    /// <param name="created_on"></param>
    /// <param name="level">1=Spouse, 2=Kids, 12=TPD, 13=ADB</param>
    public static void InsertPolicyPremiumRider(string app_register_id, string policy_id, string created_by, DateTime created_on, int level)
    {
        try
        {
            List<bl_underwriting_co_rider> myCo = new List<bl_underwriting_co_rider>();
            myCo = GetUnderwritingCORiderData(app_register_id);

            //Get discount
            double discount_amount = Convert.ToDouble(da_underwriting.GetDiscount(app_register_id));

            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                foreach (bl_underwriting_co_rider co in myCo)
                {
                    if (co.Level == level)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_Insert_Policy_Premium_Rider";

                        cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
                        cmd.Parameters.AddWithValue("@Sum_Insure", co.System_Sum_Insure);
                        cmd.Parameters.AddWithValue("@Premium", co.System_Premium);
                        cmd.Parameters.AddWithValue("@Original_Amount", co.Original_Amount);
                        cmd.Parameters.AddWithValue("@EM_Percent", co.EM_Percent);
                        cmd.Parameters.AddWithValue("@EM_Premium", co.EM_Premium);
                        cmd.Parameters.AddWithValue("@EM_Amount", co.EM_Amount);
                        cmd.Parameters.AddWithValue("@EF_Rate", co.EF_Rate);
                        cmd.Parameters.AddWithValue("@EF_Premium", co.EF_Premium);
                        cmd.Parameters.AddWithValue("@Total_EF_Year", co.Total_EF_Year);
                        cmd.Parameters.AddWithValue("@Discount_Amount", discount_amount);
                        cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Created_By", created_by);
                        cmd.Parameters.AddWithValue("@Created_Note", "");
                        cmd.Parameters.AddWithValue("@Level", co.Level);

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        con.Close();
                        //save rider status
                        InsertPolicyStatusRider(policy_id, created_by, DateTime.Now + "", co.Rider_Type, co.Level);
                    }

                }
            }

        }

        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertPolicyPremiumRider] in class [da_policy] [5 arguments]. Details: " + ex.Message);
        }
    }

    #region By Meas Sun On 2019-12-06
    /// <summary>
    /// Funtion InsertPolicyPremiumRider[2 Arguments]
    /// Insert new row into table policy premium rider base on level
    /// </summary>
    /// <param name="prem_rider"></param>
    public static bool InsertPolicyPremiumRider(bl_policy_premium_riders prem_rider)
    {
        bool status = false;
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Insert_Policy_Premium_Rider";

                cmd.Parameters.AddWithValue("@Policy_ID", prem_rider.Policy_ID);
                cmd.Parameters.AddWithValue("@Sum_Insure", prem_rider.Sum_Insure);
                cmd.Parameters.AddWithValue("@Premium", prem_rider.Premium);
                cmd.Parameters.AddWithValue("@Original_Amount", prem_rider.Original_Amount);
                cmd.Parameters.AddWithValue("@EM_Percent", prem_rider.EM_Percent);
                cmd.Parameters.AddWithValue("@EM_Premium", prem_rider.EM_Premium);
                cmd.Parameters.AddWithValue("@EM_Amount", prem_rider.EM_Amount);
                cmd.Parameters.AddWithValue("@EF_Rate", prem_rider.EF_Rate);
                cmd.Parameters.AddWithValue("@EF_Premium", prem_rider.EF_Premium);
                cmd.Parameters.AddWithValue("@Total_EF_Year", prem_rider.Total_EF_Year);
                cmd.Parameters.AddWithValue("@Discount_Amount", prem_rider.Discount_Amount);
                cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
                cmd.Parameters.AddWithValue("@Created_By", prem_rider.Created_By);
                cmd.Parameters.AddWithValue("@Created_Note", "");
                cmd.Parameters.AddWithValue("@Level", prem_rider.Level);
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                    status = true;
                }
                catch (Exception ex)
                {
                    //Add error to log 
                    Log.AddExceptionToLog("Error in function [InsertPolicyPremiumRider] in class [da_policy]. Details: " + ex.Message);
                    status = false;
                }
            }

        }

        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertPolicyPremiumRider] in class [da_policy] [2 arguments]. Details: " + ex.Message);
        }

        return status;
    }

    #endregion

    public static bl_underwriting_co GetUnderwritingCOData(string app_register_id)
    {
        bl_underwriting_co my_data = new bl_underwriting_co();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_Original_Data"; //SP_Get_UW_Details_By_App_Register_ID

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
                        my_data.UW_Life_Product_ID = rdr.GetString(rdr.GetOrdinal("UW_Life_Product_ID"));

                        //Get uw_co by uw_life_product_id
                        bl_underwriting_co my_uw_co = GetUWCOByUWLifeProductID(my_data.UW_Life_Product_ID);

                        my_data.EM_Percent = my_uw_co.EM_Percent;
                        my_data.EM_Premium = my_uw_co.EM_Premium;
                        my_data.EM_Amount = my_uw_co.EM_Amount;
                        my_data.EM_Amount = my_uw_co.EM_Amount;
                        my_data.Round_Status_ID = my_uw_co.Round_Status_ID;

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUnderwritingData] in class [da_policy]. Details: " + ex.Message);
            }

        }

        return my_data;

    }

    //Get data from table UW_CO by it uw_co_prod_id
    public static bl_underwriting_co GetUWCOByUWLifeProductID(string uw_life_product_id)
    {
        bl_underwriting_co my_uw_co = new bl_underwriting_co();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_By_Life_Prod_ID";

            cmd.Parameters.AddWithValue("@UW_Life_Product_ID", uw_life_product_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_uw_co.EM_Percent = rdr.GetDouble(rdr.GetOrdinal("EM_Percent"));
                        my_uw_co.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        my_uw_co.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));

                        if (rdr["Round_Status_ID"] is DBNull)
                        {
                            my_uw_co.Round_Status_ID = "";
                        }
                        else
                        {
                            my_uw_co.Round_Status_ID = rdr.GetString(rdr.GetOrdinal("Round_Status_ID"));
                        }
                        

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUWCOByUWLifeProductID] in class [da_policy]. Details: " + ex.Message);
            }

        }

        return my_uw_co;

    }
    #region Policy Contact
    //Insert new row into table policy contact
    public static void InsertPolicyContact(string app_register_id, string policy_id)
    {
        //get underwriting counter offer object        
        bl_app_info_contact my_contact = GetAppInfoContact(app_register_id);

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Contact";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Mobile_Phone1", my_contact.Mobile_Phone1);
            cmd.Parameters.AddWithValue("@Mobile_Phone2", my_contact.Mobile_Phone2);
            cmd.Parameters.AddWithValue("@Home_Phone1", my_contact.Home_Phone1);
            cmd.Parameters.AddWithValue("@Home_Phone2", my_contact.Home_Phone2);
            cmd.Parameters.AddWithValue("@Office_Phone1", my_contact.Office_Phone1);
            cmd.Parameters.AddWithValue("@Office_Phone2", my_contact.Office_Phone2);
            cmd.Parameters.AddWithValue("@Fax1", my_contact.Fax1);
            cmd.Parameters.AddWithValue("@Fax2", my_contact.Fax2);
            cmd.Parameters.AddWithValue("@EMail", my_contact.EMail);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyPremium] in class [da_policy]. Details: " + ex.Message);
            }
        }

    }
  /// <summary>
  /// Update Policy contact by policy ID
  /// </summary>
  /// <param name="contact">Policy contact class object</param>
  /// <returns></returns>
    public static bool UpdatePolicyContact(bl_app_info_contact contact)
    {
        bool status = false;
        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_POLICY_CONTACT_UPDATE", new string[,] { 
            {"@policy_id", contact.PolicyID},
            {"@mobile_phone1", contact.Mobile_Phone1},
            {"@mobile_phone2", contact.Mobile_Phone2},
            {"@home_phone1", contact.Home_Phone1},
            {"@home_phone2", contact.Home_Phone2},
            {"@office_phone1", contact.Office_Phone1},
            {"@office_phone2", contact.Office_Phone2},
            {"@fax1", contact.Fax1},
            {"@fax2", contact.Fax2},
            {"@email", contact.EMail}
            }, "");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [UpdatePolicyContact(string policy_id)], detail" + ex.Message + "=>" + ex.StackTrace + "=>" + ex.InnerException);
        }
        return status;
    }
    /// <summary>
    /// Get contact object by policy ID
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bl_app_info_contact GetPolicyContact(string policy_id)
    {
        bl_app_info_contact contact = new bl_app_info_contact();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_POLICY_CONTACT_GET", new string[,] {
            {"@policy_id", policy_id}
            }, "da_policy=>GetPolicyContact(string policy_id)");
            foreach(DataRow row in tbl.Rows)
            {
                contact.PolicyID= row["policy_id"].ToString();
                contact.Mobile_Phone1 = row["mobile_phone1"].ToString();
                contact.Mobile_Phone2 = row["mobile_phone2"].ToString();
                contact.Home_Phone1 = row["home_phone1"].ToString();
                contact.Home_Phone2 = row["home_phone2"].ToString();
                contact.Office_Phone1 = row["office_phone1"].ToString();
                contact.Office_Phone2 = row["office_phone2"].ToString();
                contact.Fax1 = row["fax1"].ToString();
                contact.Fax2 = row["fax2"].ToString();
                contact.EMail = row["email"].ToString();
                break;
            }
        }
        catch (Exception ex)
        {
            contact = new bl_app_info_contact();
            Log.AddExceptionToLog("Error function [GetPolicyContact(string policy_id)], detail" + ex.Message + "=>" + ex.StackTrace + "=>" + ex.InnerException);

        }
        return contact;
    }
    #endregion Policy Contact
    #region By Meas Sun On 2019-11-28
    /// <summary>
    /// This function is usefull for CI Product
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public static bool InsertPolicyContact(bl_app_info_contact contact)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Insert_Policy_Contact", new string[,] { 
            {"@Policy_ID", contact.PolicyID},
            {"@Mobile_Phone1", contact.Mobile_Phone1},
            {"@Mobile_Phone2", contact.Mobile_Phone2},
            {"@Home_Phone1", ""},
            {"@Home_Phone2", ""},
            {"@Office_Phone1", ""},
            {"@Office_Phone2", ""},
            {"@Fax1", contact.Fax1},
            {"@Fax2", ""},
            {"@EMail", contact.EMail}
            }, "function [InsertPolicyContact(bl_app_info_contact contact) , class [da_policy]");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [InsertPolicyContact(bl_app_info_contact contact) in class [da_policy], detail: ]" + ex.Message);
        }
        return result;


    }

    #endregion
    //Insert new row into table policy premium pay
    public static void InsertPolicyPremiumPay(string app_register_id, string policy_id, string sale_agent_id, string office_id, string created_by, DateTime created_on)
    {

        string policy_prem_pay_id = "";

        bl_policy_prem_pay my_app_prem_pay = GetAppPremPay(app_register_id);

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Premium_Pay";

            //get new primary key for the row to be inserted
            policy_prem_pay_id = Helper.GetNewGuid("SP_Check_Policy_Premium_Pay_ID", "@Policy_Prem_Pay_ID").ToString();

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Prem_Pay_ID", policy_prem_pay_id);
            cmd.Parameters.AddWithValue("@Due_Date", my_app_prem_pay.Due_Date);
            cmd.Parameters.AddWithValue("@Pay_Date", my_app_prem_pay.Pay_Date);
            cmd.Parameters.AddWithValue("@Prem_Year", 1);
            cmd.Parameters.AddWithValue("@Prem_Lot", 1);
            cmd.Parameters.AddWithValue("@Sale_Agent_ID", sale_agent_id);
            cmd.Parameters.AddWithValue("@Office_ID", office_id);
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now); // Further change 
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@Created_Note", "");

            cmd.Parameters.AddWithValue("@Pay_Mode_ID", my_app_prem_pay.Pay_Mode_ID);


            #region Total Rider
            //TODO :For new family protection 13102016
            List<bl_policy_premium> listPremiumRider = new List<bl_policy_premium>();
            double premium_rider = 0.0;
            double rider_em_amount = 0.0;
            double total_amount_rider = 0.0;
            listPremiumRider = GetPolicyPremiumRider(policy_id);
            if (listPremiumRider.Count > 0)
            {
                foreach (bl_policy_premium preRider in listPremiumRider)
                {
                    premium_rider = premium_rider + preRider.Premium;
                    rider_em_amount = rider_em_amount + preRider.EM_Amount;
                }
                total_amount_rider = premium_rider + rider_em_amount;
            }


            ////GET ADB Rider premium from ct_app_rider by app_register_id
            //List<da_application_fp6.bl_app_rider> rider = new List<da_application_fp6.bl_app_rider>();
            //double adb_premium = 0.0;

            //rider = da_application_fp6.GetAppRiderList(app_register_id);
            //if (rider.Count > 0)
            //{
            //    foreach (da_application_fp6.bl_app_rider adb in rider)
            //    {
            //        if (adb.Rider_Type.Trim().ToUpper() == "ADB")
            //        {
            //            adb_premium = adb.Premium;
            //        }
            //    }
            //}

            //total_amount_rider = total_amount_rider + adb_premium;

            #endregion

            //Calculate total amount to pay based on pay mode
            //bl_policy_premium my_po_prem_pay = GetPolicyPremium(policy_id);
            #region CL24 Amount By Meas Sun On 03-04-2020
            List<bl_policy_premium> prem_pay_list = GetPolicyPremiumList(policy_id);

            bl_policy_premium po_prem_pay = new bl_policy_premium();

            if (prem_pay_list.Count > 0)
            {
                for (int i = 0; i < prem_pay_list.Count; i++)
                {
                    po_prem_pay.Premium += prem_pay_list[i].Premium;
                    po_prem_pay.EM_Amount += prem_pay_list[i].EM_Amount;
                    po_prem_pay.Discount_Amount += prem_pay_list[i].Discount_Amount;
                }

                cmd.Parameters.AddWithValue("@Amount", (po_prem_pay.Premium + po_prem_pay.EM_Amount + total_amount_rider) - po_prem_pay.Discount_Amount); //Amount = (Premium + EM_Amount) - Discount
            }
            else
            {
                cmd.Parameters.AddWithValue("@Amount", (po_prem_pay.Premium + po_prem_pay.EM_Amount + total_amount_rider) - po_prem_pay.Discount_Amount); //Amount = (Premium + EM_Amount) - Discount
            }
            #endregion
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyAddress] in class [da_policy]. Details: " + ex.Message);
            }
        }

    }

    public static bool InsertPolicyPremiumPay(bl_policy_prem_pay premPay)
    {
        bool status = false;
        try
        {

            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Insert_Policy_Premium_Pay",
                new string[,] { 
                    {"@Policy_ID", premPay.Policy_ID },
                    {"@Policy_Prem_Pay_ID", premPay.Policy_Prem_Pay_ID},
                    {"@Due_Date", premPay.Due_Date+""},
                    {"@Pay_Date", premPay.Pay_Date+""},
                    {"@Prem_Year", premPay.Prem_Year+""},
                    {"@Prem_Lot", premPay.Prem_Lot+""},
                    {"@Office_ID", premPay.Office_ID},
                    {"@Created_On", premPay.Created_On+""},
                    {"@Created_By", premPay.Created_By},
                    {"@Created_Note", premPay.Created_Note},
                    {"@Amount", premPay.Amount+""},
                    {"@Pay_Mode_ID", premPay.Pay_Mode_ID+""},
                    {"@Sale_Agent_ID", premPay.Sale_Agent_ID+""}
                    
                },
                "da_policy >> InsertPolicyPremiumPay");
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [InsertPolicyPremiumPay] in class [da_policy], Detial: " + ex.InnerException + "=>>" + ex.Message + "=>>" + ex.StackTrace);

        }
        return status;
    }

    //Insert new row into table policy premium
    public static void InsertPolicyNumber(string policy_id)
    {

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Number";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Number", GetPolicyNumber());

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyNumber] in class [da_policy]. Details: " + ex.Message);
            }
        }

    }
    public static bool UpdatePolicyPremPay(string policy_prem_pay_id, string product_id, string plan_code, string policy_status, double sum_insured)
    {
        bool result= false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(),"SP_UPDATE_POLICY_PREM_PAY", new string[,]{
            {"@POLICY_PREM_PAY_ID",policy_prem_pay_id}, 
            {"@PRODUCT_ID", product_id}, 
            {"@PLAN_CODE",plan_code},
            {"@POLICY_STATUS",policy_status},
            {"@SUM_INSURED",sum_insured+""}
            },"Function [UpdatePolicyPremPay(string policy_prem_pay_id, string product_id, string plan_code, string policy_status, double sum_insured)] class [da_policy]");
        

        }
        catch(Exception ex)
        {
            
        }
        return result;
    }
    //Insert new row into table policy premium
    /// <summary>
    /// Design for Flate Rate
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="policy_number"></param>
    public static bool InsertPolicyNumber(string policy_id, string policy_number)
    {
        bool result = false;
        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Number";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Number", policy_number);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyNumber(string policy_id, string policy_number)] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return result;
    }

    //Insert new row into table app policy
    public static bool InsertAppPolicy(string app_register_id, string policy_id)
    {
        bool status = false;
        string app_policy_id = "";

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_App_Policy";

            //get new primary key for the row to be inserted
            app_policy_id = Helper.GetNewGuid("SP_Check_App_Policy_ID", "@App_Policy_ID").ToString();

            cmd.Parameters.AddWithValue("@App_Policy_ID", app_policy_id);
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                status = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertAppPolicy] in class [da_policy]. Details: " + ex.Message);
                status = false;
            }
        }
        return status;

    }

    //Insert new row into table app policy
    public static void InsertPolicyPayMode(string app_register_id, string policy_id, int pay_mode, DateTime due_date, string created_by, DateTime created_on)
    {
        string app_policy_id = "";

        int first_day, first_month;

        first_day = Convert.ToInt16(due_date.Day);
        first_month = Convert.ToInt16(due_date.Month);

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Pay_Mode";

            //get new primary key for the row to be inserted
            app_policy_id = Helper.GetNewGuid("SP_Check_App_Policy_ID", "@App_Policy_ID").ToString();

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Pay_Mode", pay_mode);
            cmd.Parameters.AddWithValue("@First_Due_Day", first_day);
            cmd.Parameters.AddWithValue("@First_Due_Month", first_month);

            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@Created_Note", "");

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertAppPolicy] in class [da_policy]. Details: " + ex.Message);
            }
        }

    }
    /// <summary>
    /// This function is usefull for CI product
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="pay_mode"></param>
    /// <param name="due_date"></param>
    /// <param name="created_by"></param>
    /// <param name="created_on"></param>
    public static bool InsertPolicyPayMode(string policy_id, int pay_mode, DateTime due_date, string created_by, DateTime created_on)
    {
      
        int first_day, first_month;
        bool result = false;
        first_day = Convert.ToInt16(due_date.Day);
        first_month = Convert.ToInt16(due_date.Month);
        try
        {
           result= Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Insert_Policy_Pay_Mode", new string[,] {
                {"@Policy_ID", policy_id},
                {"@Pay_Mode", pay_mode+""},
                {"@First_Due_Day", first_day+""},
                {"@First_Due_Month", first_month+""},
                {"@Created_On", created_on+""},
                {"@Created_By", created_by},
                {"@Created_Note", ""}
             }, "[InsertPolicyPayMode(string policy_id, int pay_mode, DateTime due_date, string created_by, DateTime created_on)], class [da_policy]"
        );
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [InsertPolicyPayMode(string policy_id, int pay_mode, DateTime due_date, string created_by, DateTime created_on)] in class [da_policy], detail:" + ex.Message);
        }

        return result;
    }

    //Insert new row into table policy benefit
    public static void InsertPolicyBenefit(string benefit_note, string policy_id, string created_by, DateTime created_on)
    {


        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Benefit";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Benefit_Note", benefit_note);
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@Created_Note", "");

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [InsertPolicyBenefit] in class [da_Policy]. Details: " + ex.Message);
            }
        }
    }

    //Get relationship in Khmer by passing English word
    public static string GetKhmerRelationship(string eng_relationship)
    {
        string kh_relationship = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Relationship_Khmer";

            cmd.Parameters.AddWithValue("@Relationship", eng_relationship);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        kh_relationship = rdr.GetString(rdr.GetOrdinal("Relationship_Khmer")); ;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetKhmerRelationship] in class [da_policy]. Details: " + ex.Message);
            }



            return kh_relationship;
        }
    }

    //Insert new row into table policy benefit item
    public static void InsertPolicyBenefitItem(string app_register_id, string policy_id)
    {
        string policy_benefit_item_id = "";

        List<bl_app_benefit_item> benefits = new List<bl_app_benefit_item>();

        benefits = GetAppBenefitItemList(app_register_id);

        for (int i = 0; i < benefits.Count; i++)
        {
            //Get app benefit item by app register id
            bl_app_benefit_item app_benefit_item = benefits[i];

            string connString = AppConfiguration.GetConnectionString();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_Insert_Policy_Benefit_Item";

                //get new primary key for the row to be inserted
                policy_benefit_item_id = Helper.GetNewGuid("SP_Check_Policy_Benefit_Item_ID", "@Policy_Benefit_Item_ID").ToString();

                cmd.Parameters.AddWithValue("@Policy_Benefit_Item_ID", policy_benefit_item_id);
                cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
                cmd.Parameters.AddWithValue("@Full_Name", app_benefit_item.Full_Name);
                cmd.Parameters.AddWithValue("@ID_Type", app_benefit_item.ID_Type);
                cmd.Parameters.AddWithValue("@ID_Card", app_benefit_item.ID_Card);
                cmd.Parameters.AddWithValue("@Percentage", app_benefit_item.Percentage);
                cmd.Parameters.AddWithValue("@Relationship", app_benefit_item.Relationship); //Get relationshp in English   
                cmd.Parameters.AddWithValue("@Seq_Number", app_benefit_item.Seq_Number);
                cmd.Parameters.AddWithValue("@Relationship_Khmer", GetKhmerRelationship(app_benefit_item.Relationship)); //Get relationshp in Khmer   
                cmd.Parameters.AddWithValue("@Remarks", app_benefit_item.Remarks);
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    //Add error to log 
                    con.Close();
                    Log.AddExceptionToLog("Error in function [InsertPolicyBenefitItem] in class [da_Policy]. Details: " + ex.Message);
                }
            }
        }



    }

    #region By Meas Sun On 2019-12-11
    //Insert new row into table policy benefit item
    public static bool InsertPolicyBenefitItem(bl_policy_benefit_item benefit_item)
    {
        bool status = false;
        string policy_benefit_item_id = "";

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Benefit_Item";

            //get new primary key for the row to be inserted
            policy_benefit_item_id = Helper.GetNewGuid("SP_Check_Policy_Benefit_Item_ID", "@Policy_Benefit_Item_ID").ToString();

            cmd.Parameters.AddWithValue("@Policy_Benefit_Item_ID", policy_benefit_item_id);
            cmd.Parameters.AddWithValue("@Policy_ID", benefit_item.Policy_ID);
            cmd.Parameters.AddWithValue("@Full_Name", benefit_item.Full_Name);
            cmd.Parameters.AddWithValue("@ID_Type", benefit_item.ID_Type);
            cmd.Parameters.AddWithValue("@ID_Card", benefit_item.ID_Card);
            cmd.Parameters.AddWithValue("@Percentage", benefit_item.Percentage);
            cmd.Parameters.AddWithValue("@Relationship", benefit_item.Relationship); //Get relationshp in English   
            cmd.Parameters.AddWithValue("@Seq_Number", benefit_item.Seq_Number);
            cmd.Parameters.AddWithValue("@Relationship_Khmer", GetKhmerRelationship(benefit_item.Relationship)); //Get relationshp in Khmer   
            cmd.Parameters.AddWithValue("@Remarks", "");
            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                status = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                con.Close();
                Log.AddExceptionToLog("Error in function [InsertPolicyBenefitItem] in class [da_Policy]. Details: " + ex.Message);
                status = false;
            }
        }
        return status;

    }

    #endregion
    //Get benefit item from app_benefit_item table
    public static List<bl_app_benefit_item> GetAppBenefitItemList(string app_id)
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
                    benefit_item.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                    benefit_item.Relationship_Khmer = rdr.GetString(rdr.GetOrdinal("Relationship_Khmer"));
                    benefit_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));
                    benefit_item.Remarks = rdr["remarks"].ToString();
                    benefit_items.Add(benefit_item);
                }
            }
            con.Close();
        }
        return benefit_items;
    }

    //Insert new row into table policy premium
    /// <summary>
    /// Save "IF" policy status
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="created_by"></param>
    /// <param name="created_on"></param>
    public static void InsertPolicyStatus(string policy_id, string created_by, DateTime created_on)
    {

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_Status";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", "IF");
            cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Created_By", created_by);
            cmd.Parameters.AddWithValue("@Created_Note", "");

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyStatus] in class [da_policy]. Details: " + ex.Message);
            }
        }

    }
   /// <summary>
   /// Save others policy status
   /// </summary>
   /// <param name="policy_id"></param>
   /// <param name="policy_sataus"></param>
   /// <param name="created_by"></param>
   /// <param name="created_on"></param>
    public static bool InsertPolicyStatus(string policy_id, string policy_sataus, string created_by, DateTime created_on)
    {
        bool result = false; 
        
        {
           
            try
            {
              
                result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_Insert_Policy_Status", new string[,] { 
                {"@Policy_ID", policy_id},
                {"@Policy_Status_Type_ID", policy_sataus.Trim().ToUpper()},
                {"@Created_On", created_on+""},
                {"@Created_By", created_by},
                {"@Created_Note", ""}
                }, "Function [InsertPolicyStatus(string policy_id, string policy_sataus, string created_by, DateTime created_on)] in class [da_policy]");
            }
            catch (Exception ex)
            {
                result = false;
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyStatus] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }
    //------------------------------------------------------------------------//
    public static void InsertPolicyStatusRider(string policy_id, string created_by, string created_on, String rider_type, int level)
    {



        try
        {
            string connString = AppConfiguration.GetConnectionString();
            string[,] para = new string[,] { { "@Policy_ID", policy_id }, {"@Policy_Status_Type_ID", "IF"},{"@Created_On", created_on}, 
                                                {"@Created_By", created_by}, {"@Created_Note", ""}, {"@Rider_Type", rider_type},
                                                {"@Level", level+""}

                                                };
            Helper.ExecuteProcedure(connString, "SP_Insert_Policy_Status_Rider", para, "da_policy => InsertPolicyStatusRider");
        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [InsertPolicyStatusRider] in class [da_policy]. Details: " + ex.Message);
        }

        //using (SqlConnection con = new SqlConnection(connString))
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "SP_Insert_Policy_Status_Rider";

        //    cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
        //    cmd.Parameters.AddWithValue("@Policy_Status_Type_ID", "IF");
        //    cmd.Parameters.AddWithValue("@Created_On", DateTime.Now);
        //    cmd.Parameters.AddWithValue("@Created_By", created_by);
        //    cmd.Parameters.AddWithValue("@Created_Note", "");
        //    cmd.Parameters.AddWithValue("@Rider_Type", rider_type);
        //    cmd.Parameters.AddWithValue("@Level", level);

        //    cmd.Connection = con;
        //    con.Open();
        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Add error to log 
        //        Log.AddExceptionToLog("Error in function [InsertPolicyStatusRider] in class [da_policy]. Details: " + ex.Message);
        //    }
        //}

    }
    //Get app info contact object

    public static bl_app_info_contact GetAppInfoContact(string app_register_id)
    {

        bl_app_info_contact my_data = new bl_app_info_contact();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_App_Info_Contact";

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
                        my_data.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                        my_data.Mobile_Phone1 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone1"));
                        my_data.Mobile_Phone2 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone2"));
                        my_data.Home_Phone1 = rdr.GetString(rdr.GetOrdinal("Home_Phone1"));
                        my_data.Home_Phone2 = rdr.GetString(rdr.GetOrdinal("Home_Phone2"));
                        my_data.Office_Phone1 = rdr.GetString(rdr.GetOrdinal("Office_Phone1"));
                        my_data.Office_Phone2 = rdr.GetString(rdr.GetOrdinal("Office_Phone2"));
                        my_data.Fax1 = rdr.GetString(rdr.GetOrdinal("Fax1"));
                        my_data.Fax2 = rdr.GetString(rdr.GetOrdinal("Fax2"));
                        my_data.EMail = rdr.GetString(rdr.GetOrdinal("EMail"));

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetAppInfoContact] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return my_data;
    }

    //To be deleted


    public static int GetPayModeByAppRegisterID(string app_register_id)
    {

        int pay_mode_id = 0;


        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Payment_Mode_By_App_Register_ID";

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
                        pay_mode_id = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPayModeByAppRegisterID] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return pay_mode_id;

    }


    //Get policy premium pay object
    public static bl_policy_prem_pay GetAppPremPay(string app_register_id)
    {

        bl_policy_prem_pay mydata = new bl_policy_prem_pay();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_App_Prem_Pay";

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
                        mydata.Pay_Date = rdr.GetDateTime(rdr.GetOrdinal("Pay_Date"));
                        mydata.Amount = rdr.GetDouble(rdr.GetOrdinal("Amount"));

                        mydata.Pay_Mode_ID = GetPayModeByAppRegisterID(app_register_id);

                        //Get due date from effective date
                        mydata.Due_Date = GetPolicyEffectiveDate(app_register_id);

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetAppPremPay] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return mydata;
    }


    //Get policy effective date by app register id from table Ct_UW_Effective_Date
    public static DateTime GetPolicyEffectiveDate(string app_register_id)
    {


        DateTime mydatetime = new DateTime();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_Effective_Date";

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
                        mydatetime = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyEffectiveDate] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return mydatetime;
    }


    //Get last policy number
    public static string GetPolicyNumber()
    {
        string policy_number = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Last_Policy_Number";

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        policy_number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));

                        int strConvert = Convert.ToInt16(policy_number) + 1;
                        policy_number = strConvert.ToString("D8");

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyNumber] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return policy_number;
    }


    //Get app benefit item object by app register id
    public static bl_app_benefit_item GetAppBenefitItem(string app_register_id)
    {

        bl_app_benefit_item my_data = new bl_app_benefit_item();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_App_Benefit_Item";

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

                        my_data.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                        my_data.App_Benefit_Item_ID = rdr.GetString(rdr.GetOrdinal("App_Benefit_Item_ID"));
                        my_data.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                        my_data.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                        my_data.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                        my_data.Percentage = rdr.GetDouble(rdr.GetOrdinal("Percentage"));
                        my_data.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                        my_data.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetAppBenefitItem] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return my_data;
    }


    //Get policy pay mode object
    public static bl_policy_premium GetPolicyPremium(string policy_id)
    {
        bl_policy_premium mydata = new bl_policy_premium();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_Premium_By_Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        mydata.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                        mydata.Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));
                        mydata.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        mydata.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                        mydata.Discount_Amount = rdr.GetDouble(rdr.GetOrdinal("Discount_Amount"));
                        mydata.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyPremium] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return mydata;
    }

    //Get Policy Policy Prem Pay List
    public static List<bl_policy_premium> GetPolicyPremiumList(string Policy_ID) 
    {
        List<bl_policy_premium> pol_prem_pay = new List<bl_policy_premium>();
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Policy_Premium_By_Policy_ID";
            cmd.Parameters.AddWithValue("@Policy_ID", Policy_ID);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        bl_policy_premium policy_premium = new bl_policy_premium();

                        policy_premium.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                        policy_premium.Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                        policy_premium.Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));
                        policy_premium.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        policy_premium.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                        policy_premium.Discount_Amount = rdr.GetDouble(rdr.GetOrdinal("Discount_Amount"));

                        pol_prem_pay.Add(policy_premium);
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyPremiumList] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return pol_prem_pay;
    }

    //TODO: Get policy Premium rider
    public static List<bl_policy_premium> GetPolicyPremiumRider(string policy_id)
    {
        bl_policy_premium mydata;
        List<bl_policy_premium> listPre = new List<bl_policy_premium>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_Premium_Rider_By_Policy_ID";
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        mydata = new bl_policy_premium();
                        mydata.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                        mydata.Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));
                        mydata.EM_Premium = rdr.GetDouble(rdr.GetOrdinal("EM_Premium"));
                        mydata.EM_Amount = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                        mydata.Discount_Amount = rdr.GetDouble(rdr.GetOrdinal("Discount_Amount"));
                        listPre.Add(mydata);
                        Log.AddExceptionToLog(mydata.Policy_ID);
                    }
                }
                con.Close();

            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyPremiumRider] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return listPre;
    }

    //Get policy pay mode object
    public static bl_policy_pay_mode GetPolicyPayMode(string policy_id)
    {
        bl_policy_pay_mode mydata = new bl_policy_pay_mode();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_Pay_Mode";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        mydata.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                        mydata.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));
                        mydata.First_Due_Day = rdr.GetInt32(rdr.GetOrdinal("First_Due_Day"));
                        mydata.First_Due_Month = rdr.GetInt32(rdr.GetOrdinal("First_Due_Month"));

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyPayMode] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return mydata;
    }

    /// <summary>
    /// Return payment mode as text
    /// </summary>
    /// <param name="payment_mode">Payment mode ID</param>
    /// <returns></returns>
    public static string GetPaymentModeText(int payment_mode)
    {
        string pay_mode = "";
        switch (payment_mode)
        {
            case 0: //Single
                pay_mode = "Single";
                break;
            case 1: //Annually
                pay_mode = "Annually";
                break;
            case 2:
                pay_mode = "Semi-annually";
                break;
            case 3:
                pay_mode = "Quarterly";
                break;
            case 4:
                pay_mode = "Monthly";
                break;
        }
        return pay_mode;
    }

    //Get all data about policy for printing
    /// <summary>
    /// Get main and sub policies
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bl_policy_detail GetPolicyDetail(string policy_id)
    {
        bl_policy_detail policy_detail = new bl_policy_detail();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_By_Policy_ID";

            //bind parameter            
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        #region //<Start: Sub policy list ,updated by: Maneth, updated date: 17 Oct 2018>
                        
                        bl_sub_policy_premium subPolicy = new bl_sub_policy_premium();
                        subPolicy = da_sub_policy.GetSubPoliciesTotalPremium(policy_id);
                        #endregion //<End: Sub policy list ,updated by: Maneth, updated date: 17 Oct 2018>


                        policy_detail.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                        if (rdr["Policy_Number"] is DBNull)
                        {
                            policy_detail.Policy_Number = "";
                        }
                        else
                        {
                            policy_detail.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                        }

                        //Customer info
                        policy_detail.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID")).ToUpper();
                        policy_detail.Customer_Fullname = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name")) + " " + rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));

                        #region customer english name by maneth 14032017
                        policy_detail.First_Name = rdr.GetString(rdr.GetOrdinal("first_name"));
                        policy_detail.Last_Name = rdr.GetString(rdr.GetOrdinal("last_name"));
                        #endregion
                        policy_detail.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));

                        policy_detail.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        policy_detail.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));

                        if ((rdr.GetInt32(rdr.GetOrdinal("Gender")) == 1))
                            policy_detail.Gender = "ប្រុស";
                        else
                            policy_detail.Gender = "ស្រី";


                        //Contact
                        policy_detail.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                        policy_detail.Address2 = rdr.GetString(rdr.GetOrdinal("Address2"));
                        policy_detail.Province = rdr.GetString(rdr.GetOrdinal("Province"));
                        policy_detail.Customer_Address = policy_detail.Address1 + " " + policy_detail.Address2 + " " + policy_detail.Province;
                        policy_detail.Mobile_Phone1 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone1"));

                        //Product
                        policy_detail.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                        policy_detail.En_Title = rdr.GetString(rdr.GetOrdinal("En_Title"));
                        policy_detail.Kh_Title = rdr.GetString(rdr.GetOrdinal("Kh_Title"));
                        policy_detail.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                        policy_detail.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                        policy_detail.System_Sum_Insure += subPolicy.SumInsure;//sum sub policies
                        policy_detail.User_Sum_Insure += subPolicy.SumInsure;
                        policy_detail.Assure_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                        policy_detail.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));
                        #region //Discount amount by Maneth 06 September 2018
                        policy_detail.System_Premium_Discount = rdr.GetDouble(rdr.GetOrdinal("discount_amount"));
                        policy_detail.System_Premium_Discount += subPolicy.DiscountAmount;
                        #endregion
                        //Premium & extra premium & total premium
                        policy_detail.System_Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));

                        policy_detail.System_Premium = (policy_detail.System_Premium + subPolicy.Premium) - policy_detail.System_Premium_Discount;

                        policy_detail.Extra_Premium = da_underwriting.GetUWCOExtraAmount(policy_detail.App_Register_ID);
                        policy_detail.Total_Premium = policy_detail.System_Premium + policy_detail.Extra_Premium;
                       
                        policy_detail.TPD = 0;


                        policy_detail.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));
                        policy_detail.Status_Code = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));

                        policy_detail.Payment_Mode = da_underwriting.GetPaymentMode(policy_detail.Pay_Mode.ToString());


                        //get pay date n month
                        bl_policy_pay_mode my_policy_pay_mode = GetPolicyPayMode(policy_id);
                        int pay_day = my_policy_pay_mode.First_Due_Day;
                        int pay_month = my_policy_pay_mode.First_Due_Month;

                        switch (policy_detail.Payment_Mode)
                        {
                            case "Annually":
                                policy_detail.Payment_Mode = "ប្រចាំឆ្នាំ";
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " ខែ " + GetMonthName(pay_month) + " រៀងរាល់ឆ្នាំ";
                                break;
                            case "Semi-Annual":
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " ខែ " + GetMonthName(pay_month) + ", ថ្ងៃទី " + pay_day + " ខែ " + GetMonthName(pay_month + 6);
                                policy_detail.Payment_Mode = "ប្រចាំឆមាស";
                                break;
                            case "Quarterly":
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " " + GetMonthName(pay_month) + ", " + pay_day + " " + GetMonthName(pay_month + 3) + ", " + pay_day + " " + GetMonthName(pay_month + 6) + ", " + pay_day + " " + GetMonthName(pay_month + 9);
                                policy_detail.Payment_Mode = "ប្រចាំត្រីមាស";
                                break;
                            case "Monthly":
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " នៃខែនីមួយៗ";
                                policy_detail.Payment_Mode = "ប្រចាំខែ";
                                break;
                            case "Single":
                                policy_detail.Next_Premium_Payment_Due_Date = "-";
                                policy_detail.Payment_Mode = "បង់តែម្តង";
                                break;
                        }

                        //Policy detail
                        policy_detail.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                        dtfi.ShortDatePattern = "dd/MM/yyyy";
                        dtfi.DateSeparator = "/";

                        policy_detail.Maturity_Date = Convert.ToDateTime(policy_detail.Effective_Date.Day.ToString() + "/" + policy_detail.Effective_Date.Month.ToString() + "/" + Convert.ToString(policy_detail.Effective_Date.Year + policy_detail.Assure_Year), dtfi);
                        policy_detail.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));

                        //policy_detail.Issue_Date = DateTime.Now;

                        policy_detail.Expiry_Date = policy_detail.Maturity_Date.AddDays(-1);

                        #region //sale agent id by maneth <03032017>
                        policy_detail.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("sale_agent_id"));
                        //20122017
                        policy_detail.Product_ID = rdr["product_id"].ToString();
                        #endregion


                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyDetail] in class [da_policy]. Details: " + ex.Message);
            }

        }


        return policy_detail;


    }

    /// <summary>
    /// Get split policy
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bl_policy_detail GetPolicyDetailSplit(string policy_id)
    {
        bl_policy_detail policy_detail = new bl_policy_detail();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_By_Policy_ID";

            //bind parameter            
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        policy_detail.App_Register_ID = rdr.GetString(rdr.GetOrdinal("App_Register_ID"));
                        if (rdr["Policy_Number"] is DBNull)
                        {
                            policy_detail.Policy_Number = "";
                        }
                        else
                        {
                            policy_detail.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));
                        }

                        //Customer info
                        policy_detail.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID")).ToUpper();
                        policy_detail.Customer_Fullname = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name")) + " " + rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));

                        #region customer english name by maneth 14032017
                        policy_detail.First_Name = rdr.GetString(rdr.GetOrdinal("first_name"));
                        policy_detail.Last_Name = rdr.GetString(rdr.GetOrdinal("last_name"));
                        #endregion
                        policy_detail.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));

                        policy_detail.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        policy_detail.Age_Insure = rdr.GetInt32(rdr.GetOrdinal("Age_Insure"));

                        if ((rdr.GetInt32(rdr.GetOrdinal("Gender")) == 1))
                            policy_detail.Gender = "ប្រុស";
                        else
                            policy_detail.Gender = "ស្រី";


                        //Contact
                        policy_detail.Address1 = rdr.GetString(rdr.GetOrdinal("Address1"));
                        policy_detail.Address2 = rdr.GetString(rdr.GetOrdinal("Address2"));
                        policy_detail.Province = rdr.GetString(rdr.GetOrdinal("Province"));
                        policy_detail.Customer_Address = policy_detail.Address1 + " " + policy_detail.Address2 + " " + policy_detail.Province;
                        policy_detail.Mobile_Phone1 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone1"));

                        //Product
                        policy_detail.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                        policy_detail.En_Title = rdr.GetString(rdr.GetOrdinal("En_Title"));
                        policy_detail.Kh_Title = rdr.GetString(rdr.GetOrdinal("Kh_Title"));
                        //policy_detail.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("System_Sum_Insure"));
                        policy_detail.System_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("sum_insure"));
                        policy_detail.User_Sum_Insure = rdr.GetDouble(rdr.GetOrdinal("Sum_Insure"));
                     
                        policy_detail.Assure_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                        policy_detail.Pay_Year = rdr.GetInt32(rdr.GetOrdinal("Pay_Year"));
                       
                        policy_detail.System_Premium_Discount = rdr.GetDouble(rdr.GetOrdinal("discount_amount"));
                      
                        #endregion
                        //Premium & extra premium & total premium
                        policy_detail.System_Premium = rdr.GetDouble(rdr.GetOrdinal("Premium"));

                        policy_detail.System_Premium = (policy_detail.System_Premium) - policy_detail.System_Premium_Discount;

                        policy_detail.Extra_Premium = da_underwriting.GetUWCOExtraAmount(policy_detail.App_Register_ID);
                        policy_detail.Total_Premium = policy_detail.System_Premium + policy_detail.Extra_Premium;

                        policy_detail.TPD = 0;


                        policy_detail.Pay_Mode = rdr.GetInt32(rdr.GetOrdinal("Pay_Mode"));
                        policy_detail.Status_Code = rdr.GetString(rdr.GetOrdinal("Policy_Status_Type_ID"));

                        policy_detail.Payment_Mode = da_underwriting.GetPaymentMode(policy_detail.Pay_Mode.ToString());


                        //get pay date n month
                        bl_policy_pay_mode my_policy_pay_mode = GetPolicyPayMode(policy_id);
                        int pay_day = my_policy_pay_mode.First_Due_Day;
                        int pay_month = my_policy_pay_mode.First_Due_Month;

                        switch (policy_detail.Payment_Mode)
                        {
                            case "Annually":
                                policy_detail.Payment_Mode = "ប្រចាំឆ្នាំ";
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " ខែ " + GetMonthName(pay_month) + " រៀងរាល់ឆ្នាំ";
                                break;
                            case "Semi-Annual":
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " ខែ " + GetMonthName(pay_month) + ", ថ្ងៃទី " + pay_day + " ខែ " + GetMonthName(pay_month + 6);
                                policy_detail.Payment_Mode = "ប្រចាំឆមាស";
                                break;
                            case "Quarterly":
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " " + GetMonthName(pay_month) + ", " + pay_day + " " + GetMonthName(pay_month + 3) + ", " + pay_day + " " + GetMonthName(pay_month + 6) + ", " + pay_day + " " + GetMonthName(pay_month + 9);
                                policy_detail.Payment_Mode = "ប្រចាំត្រីមាស";
                                break;
                            case "Monthly":
                                policy_detail.Next_Premium_Payment_Due_Date = "ថ្ងៃទី " + pay_day + " នៃខែនីមួយៗ";
                                policy_detail.Payment_Mode = "ប្រចាំខែ";
                                break;
                            case "Single":
                                policy_detail.Next_Premium_Payment_Due_Date = "-";
                                policy_detail.Payment_Mode = "បង់តែម្តង";
                                break;
                        }

                        //Policy detail
                        policy_detail.Effective_Date = rdr.GetDateTime(rdr.GetOrdinal("Effective_Date"));

                        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                        dtfi.ShortDatePattern = "dd/MM/yyyy";
                        dtfi.DateSeparator = "/";

                        policy_detail.Maturity_Date = Convert.ToDateTime(policy_detail.Effective_Date.Day.ToString() + "/" + policy_detail.Effective_Date.Month.ToString() + "/" + Convert.ToString(policy_detail.Effective_Date.Year + policy_detail.Assure_Year), dtfi);
                        policy_detail.Issue_Date = rdr.GetDateTime(rdr.GetOrdinal("Issue_Date"));

                        //policy_detail.Issue_Date = DateTime.Now;

                        policy_detail.Expiry_Date = policy_detail.Maturity_Date.AddDays(-1);

                        #region //sale agent id by maneth <03032017>
                        policy_detail.Sale_Agent_ID = rdr.GetString(rdr.GetOrdinal("sale_agent_id"));
                        //20122017
                        policy_detail.Product_ID = rdr["product_id"].ToString();
                        #endregion


                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyDetail] in class [da_policy]. Details: " + ex.Message);
            }

        }


        return policy_detail;


    }


    //Get all data about policy info
    public static bl_policy_detail GetPolicyDetailByPolicyID(string policy_id) 
    {
        bl_policy_detail policy_detail = new bl_policy_detail();
        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_Detail_By_Policy_ID";

            //bind parameter            
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        //Customer info
                        policy_detail.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID")).ToUpper();
                        policy_detail.First_Name = rdr.GetString(rdr.GetOrdinal("first_name"));
                        policy_detail.Last_Name = rdr.GetString(rdr.GetOrdinal("last_name"));
                        policy_detail.Khmer_First_Name = rdr.GetString(rdr.GetOrdinal("Khmer_First_Name"));
                        policy_detail.Khmer_Last_Name = rdr.GetString(rdr.GetOrdinal("Khmer_Last_Name"));
                        policy_detail.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                        policy_detail.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                        policy_detail.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        if ((rdr.GetInt32(rdr.GetOrdinal("Gender")) == 1))
                            policy_detail.Gender = "1";
                        else
                            policy_detail.Gender = "0";

                        policy_detail.Country_ID = rdr.GetString(rdr.GetOrdinal("Country_ID"));
                        
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyDetailByID] in class [da_policy]. Details: " + ex.Message);
            }

        }

        return policy_detail;
    }
    //Get all data about policy for WING 
    public static bl_policy_detail GetPolicyDetailByPolicyNumber(string policy_number)
    {
        bl_policy_detail policy_detail = new bl_policy_detail();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_Detail_By_Policy_Number";

            //bind parameter            
            cmd.Parameters.AddWithValue("@Policy_Number", policy_number);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        policy_detail.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                        policy_detail.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));

                        //Customer info
                        policy_detail.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID")).ToUpper();
                        policy_detail.Customer_Fullname = rdr.GetString(rdr.GetOrdinal("Last_Name")) + " " + rdr.GetString(rdr.GetOrdinal("First_Name"));

                        policy_detail.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));

                        policy_detail.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));

                        policy_detail.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));

                        if ((rdr.GetInt32(rdr.GetOrdinal("Gender")) == 1))
                            policy_detail.Gender = "Male";
                        else
                            policy_detail.Gender = "Female";

                        //Contact                        
                        policy_detail.Mobile_Phone1 = rdr.GetString(rdr.GetOrdinal("Mobile_Phone1"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyDetailByPolicyNumber] in class [da_policy]. Details: " + ex.Message);
            }

        }


        return policy_detail;


    }

    //Get policy wing
    public static bl_policy_wing GetPolicyWINGByID(string policy_wing_id)
    {
        bl_policy_wing policy_wing = new bl_policy_wing();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_WING_Detail_By_ID";

            //bind parameter            
            cmd.Parameters.AddWithValue("@Policy_WING_ID", policy_wing_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        policy_wing.Policy_WING_ID = rdr.GetString(rdr.GetOrdinal("Policy_WING_ID"));
                        policy_wing.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                        policy_wing.Policy_Number = rdr.GetString(rdr.GetOrdinal("Policy_Number"));

                        //Customer info
                        policy_wing.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                        policy_wing.Customer_Name = rdr.GetString(rdr.GetOrdinal("Customer_Name"));
                        policy_wing.Gender = rdr.GetString(rdr.GetOrdinal("Gender"));
                        policy_wing.ID_Type = rdr.GetString(rdr.GetOrdinal("ID_Type"));
                        policy_wing.ID_Number = rdr.GetString(rdr.GetOrdinal("ID_Number"));
                        policy_wing.Birth_Date = rdr.GetDateTime(rdr.GetOrdinal("Birth_Date"));
                        policy_wing.Contact_Number = rdr.GetString(rdr.GetOrdinal("Contact_Number"));
                        policy_wing.WING_SK = rdr.GetString(rdr.GetOrdinal("WING_SK"));
                        policy_wing.WING_Number = rdr.GetString(rdr.GetOrdinal("WING_Number"));

                        policy_wing.Created_On = rdr.GetDateTime(rdr.GetOrdinal("Created_On"));
                        policy_wing.Created_By = rdr.GetString(rdr.GetOrdinal("WING_Number"));


                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyWINGByID] in class [da_policy]. Details: " + ex.Message);
            }

        }


        return policy_wing;


    }


    //Get List of Benefit_Items by policy id
    public static List<bl_policy_benefit_item> GetPolicyBenefitItem(string policy_id)
    {

        List<bl_policy_benefit_item> benefit_items = new List<bl_policy_benefit_item>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            SqlCommand cmd = new SqlCommand("SP_Get_Policy_Benefit_Item_By_Policy_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramName = new SqlParameter();
            paramName.ParameterName = "@Policy_ID";
            paramName.Value = policy_id;
            cmd.Parameters.Add(paramName);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    bl_policy_benefit_item benefit_item = new bl_policy_benefit_item();

                    benefit_item.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                    benefit_item.Policy_Benefit_Item_ID = rdr.GetString(rdr.GetOrdinal("Policy_Benefit_Item_ID"));
                    benefit_item.Full_Name = rdr.GetString(rdr.GetOrdinal("Full_Name"));
                    benefit_item.ID_Card = rdr.GetString(rdr.GetOrdinal("ID_Card"));
                    benefit_item.ID_Type = rdr.GetInt32(rdr.GetOrdinal("ID_Type"));
                    benefit_item.Percentage = rdr.GetDouble(rdr.GetOrdinal("Percentage"));
                    benefit_item.Relationship = rdr.GetString(rdr.GetOrdinal("Relationship"));
                    benefit_item.Seq_Number = rdr.GetInt32(rdr.GetOrdinal("Seq_Number"));
                    benefit_item.Relationship_Khmer = rdr.GetString(rdr.GetOrdinal("Relationship_Khmer"));


                    benefit_items.Add(benefit_item);
                }
            }
            con.Close();
        }
        return benefit_items;
    }


    //Get List of cv item by product id, gender and age
    public static List<bl_prod_life_cv_item> GetProdLifeCVItem(string product_id, int gender, int age, double sum_insured, int assure_year)
    {

        List<bl_prod_life_cv_item> cv_items = new List<bl_prod_life_cv_item>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {

            SqlCommand cmd = new SqlCommand("SP_Get_Product_Life_CV_Item", con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (product_id == "MRTA" || product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36")
            {
                cmd.CommandText = "SP_Get_Product_Life_CV_Item_MRTA";
                cmd.Parameters.AddWithValue("@Product_ID", product_id);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Assure_year", assure_year);
            }

            else
            {
                cmd.CommandText = "SP_Get_Product_Life_CV_Item";
                cmd.Parameters.AddWithValue("@Product_ID", product_id);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Gender", gender);

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
                        bl_prod_life_cv_item item = new bl_prod_life_cv_item();

                        item.Product_ID = rdr.GetString(rdr.GetOrdinal("Product_ID"));
                        item.Prod_Life_CV_Item_ID = rdr.GetString(rdr.GetOrdinal("Prod_Life_CV_Item_ID"));
                        item.Gender = rdr.GetInt32(rdr.GetOrdinal("Gender"));
                        item.Age = rdr.GetInt32(rdr.GetOrdinal("Age"));
                        item.Assure_Year = rdr.GetInt32(rdr.GetOrdinal("Assure_Year"));
                        item.Policy_Year = rdr.GetInt32(rdr.GetOrdinal("Policy_Year"));
                        item.Cash_Value = rdr.GetDouble(rdr.GetOrdinal("Cash_Value"));

                        //formula for surrender value
                        //item.CV_Amount = Math.Round((item.Cash_Value * sum_insured) / 1000);

                        item.CV_Amount = Math.Round(((item.Cash_Value * sum_insured) / 1000), 0, MidpointRounding.AwayFromZero);

                        cv_items.Add(item);
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetProdLifeCVItem] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return cv_items;
    }


    //Check for existing policy address
    public static bool CheckExistingPolicyAddress(string address1, string address2, string address3)
    {
        bool check_address = false;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Check_Duplicate_Policy_Address";

            cmd.Parameters.AddWithValue("@Address1", address1);
            cmd.Parameters.AddWithValue("@Address2", address2);
            cmd.Parameters.AddWithValue("@Address3", address3);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        check_address = true;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [CheckExistingPolicyAddress] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return check_address;
    }


    //Check for existing policy address
    public static string GetExistingPolicyAddress(string address1, string address2, string address3)
    {
        string address_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_Address_ID";

            cmd.Parameters.AddWithValue("@Address1", address1);
            cmd.Parameters.AddWithValue("@Address2", address2);
            cmd.Parameters.AddWithValue("@Address3", address3);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        address_id = rdr.GetString(rdr.GetOrdinal("Address_ID")); ;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetExistingPolicyAddress] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return address_id;
    }



    //MISC
    /// <summary>
    /// Return Month in Khmer
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    public static string GetMonthName(int month)
    {
        string month_name = "";

        switch (month)
        {
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



    //Insert Ct_Policy_ID
    /// <summary>
    /// 
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="policy_type_id">1:Individual, 2:Group, 3:Micro, 4:Flexi Term, 5: Miscellaneous, 6:family Protection</param>
    /// <returns></returns>
    public static bool InsertPolicyID(string policy_id, int policy_type_id)
    {
        bool result = false;

        string connString = AppConfiguration.GetConnectionString();
        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Insert_Policy_ID";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Type_ID", policy_type_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [InsertPolicyID] in class [da_policy]. Details: " + ex.Message);
            }
        }
        return result;
    }


    //Count number of policy payment
    public static int GetNumberOfPayment(string policy_id, int policy_id_type, DateTime due_date)
    {
        int number_of_payment = 0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Number_Of_Policy_Payment";

            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Parameters.AddWithValue("@Policy_Type_ID", policy_id_type);
            cmd.Parameters.AddWithValue("@Due_Date", policy_id_type);

            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        number_of_payment = rdr.GetInt32(rdr.GetOrdinal("Number_of_Payment")); ;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetNumberOfPayment] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return number_of_payment;
    }


    public static bl_policy GetPolicyByPolicyNumber(string policy_number)
    {
        bl_policy my_policy = new bl_policy();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Policy_By_Policy_Number";

            cmd.Parameters.AddWithValue("@Policy_Number", policy_number);


            cmd.Connection = con;
            con.Open();

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        my_policy.Policy_ID = rdr.GetString(rdr.GetOrdinal("Policy_ID"));
                        my_policy.Customer_ID = rdr.GetString(rdr.GetOrdinal("Customer_ID"));
                        my_policy.Product_ID = rdr["product_id"].ToString();
                        my_policy.Address_ID = rdr["address_id"].ToString();
                        my_policy.Policy_ID = rdr["policy_id"].ToString();
                        my_policy.Effective_Date = Convert.ToDateTime(rdr["effective_date"].ToString()).Date;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetPolicyByPolicyNumber] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return my_policy;
    }

    //Get sale agent id by policy id
    public static string GetSaleAgentIDByPolicyID(string policy_id)
    {
        string sale_agent_id = "";

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_Sale_Agent_ID_By_Policy_ID";

            //bind parameter            
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        sale_agent_id = rdr.GetString(rdr.GetOrdinal("Sale_Agent_ID"));

                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetSaleAgentIDByPolicyID] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return sale_agent_id;
    }

    //TODO: Get under write CO Rider Data 13102016
    public static List<bl_underwriting_co_rider> GetUnderwritingCORiderData(string app_register_id)
    {
        bl_underwriting_co_rider my_data;
        List<bl_underwriting_co_rider> listCo = new List<bl_underwriting_co_rider>();

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_Get_UW_CO_Rider_Original_Data"; //SP_Get_UW_Details_By_App_Register_ID

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
                        my_data = new bl_underwriting_co_rider();
                        my_data.UW_Life_Product_ID = rdr.GetString(rdr.GetOrdinal("UW_Life_Product_ID"));

                        //Get uw_co by uw_life_product_id
                        // bl_underwriting_co_rider my_uw_co = GetUWCOByUWLifeProductID(my_data.UW_Life_Product_ID);
                        my_data.System_Sum_Insure = Convert.ToDouble(rdr["system_sum_insure"].ToString());
                        my_data.System_Premium = Convert.ToDouble(rdr["system_premium"].ToString());
                        my_data.EM_Percent = Convert.ToDouble(rdr["EM_Percent"].ToString());
                        my_data.EM_Premium = Convert.ToDouble(rdr["EM_Premium"].ToString());
                        my_data.EM_Amount = Convert.ToDouble(rdr["EM_Amount"].ToString());
                        my_data.EM_Rate = Convert.ToDouble(rdr["EM_Rate"].ToString());
                        my_data.Level = Convert.ToInt32(rdr["Level"].ToString());
                        my_data.Original_Amount = Convert.ToDouble(rdr["original_amount"].ToString());
                        my_data.EF_Rate = Convert.ToDouble(rdr["EF_Rate"].ToString());
                        my_data.EF_Premium = Convert.ToDouble(rdr["EF_Premium"].ToString());
                        my_data.Total_EF_Year = Convert.ToInt32(rdr["Total_EF_Year"].ToString());
                        my_data.Rider_Type = rdr["Rider_Type"].ToString();

                        listCo.Add(my_data);
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetUnderwritingCORiderData] in class [da_policy]. Details: " + ex.Message);
            }

        }

        return listCo;

    }

    /// <summary>
    /// Delete record in table ct_policy_premium_rider base on policy id and level
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static bool DeletePolicyPremiumRider(string policy_id, int level)
    {

        bool status = false;
        try
        {
            string con_string = AppConfiguration.GetConnectionString();
            status = Helper.ExecuteProcedure(con_string, "SP_DELETE_POLICY_PREMIUM_RIDER_BY_POLICY_ID_LEVEL", new string[,] { { "@Policy_ID", policy_id }, { "@Level", level + "" } }, "da_policy => DeletePolicyPremiumRider");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeletePolicyPremiumRider] in class [da_policy], Detail: " + ex.Message);
        }
        return status;
    }
    /// <summary>
    /// Delete record in table ct_policy_status_rider base on policy id and level
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static bool DeletePolicyStatusRider(string policy_id, int level)
    {

        bool status = false;
        try
        {
            string con_string = AppConfiguration.GetConnectionString();
            status = Helper.ExecuteProcedure(con_string, "SP_DELETE_POLICY_STATUS_RIDER_BY_POLICY_ID_LEVEL", new string[,] { { "@Policy_ID", policy_id }, { "@Level", level + "" } }, "da_policy => DeletePolicyStatusRider");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeletePolicyStatusRider] in class [da_policy] Detail: " + ex.Message);
            status = false;

        }
        return status;
    }
    public static bool UpdatePolicyStatus(bl_policy_status objPolicy)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_UPDATE_POLICY_STATUS_BY_POLICY_ID", new string[,] { 
                {"@POLICY_ID", objPolicy.Policy_ID},
                {"@POLICY_STATUS_TYPE_ID", objPolicy.Policy_Status_Type_ID},
                {"@UPDATED_BY", objPolicy.Updated_By},
                {"@UPDATED_ON", objPolicy.Updated_On+""},
                {"@UPDATED_NOTE", objPolicy.Updated_Note}
            
            }, "da_policy => UpdatePolicyStatus(bl_policy_status objPolicy)");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdatePolicyStatus(bl_policy_status objPolicy)] in class [da_policy], detail:" + ex.Message);
            result = false;
        }
        return result;
    }
  

    #region Get Channel ID By <Meas Sun ON 16-03-2020>
    public static string GetChannelItemID(string app_register_id)
    {
        string ChannelItemID = "";
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Channel_Item_ID_By_App_Register_ID";
            cmd.Parameters.AddWithValue("@App_Register_ID", app_register_id);
            cmd.Connection = con;

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ChannelItemID = dr[0].ToString();
            }

            con.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetChannelID] in class [da_policy], Detail: " + ex.Message);
        }

        return ChannelItemID;
    }
    #endregion

    #region new bl_underwriting_co_rider class
    public class bl_underwriting_co_rider : bl_underwriting_co
    {
        public int Level { get; set; }
        public string Rider_Type { get; set; }
    }

    public static DateTime GetIssueDate(string policy_id)
    {

        DateTime issue = new DateTime();
        try
        {
            string connString = AppConfiguration.GetConnectionString();
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Get_Policy_Issue_Date";
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);
            cmd.Connection = con;

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {

                string str = dr[0].ToString();
                issue = Convert.ToDateTime(dr[0].ToString());
            }

            con.Close();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetIssueDate] in class [da_policy], Detail: " + ex.Message);
        }
        return issue;
    }
    #endregion

    /// <summary>
    /// Get Policy by policy ID
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bl_policy GetPolicy(string policy_id)
    {
        bl_policy pol = new bl_policy();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_POLICY_GET_BY_POLICY_ID", new string[,] {
            {"@policy_id", policy_id}
            }, "da_policy=>GetPolicy(string policy_id)");
            foreach (DataRow row in tbl.Rows)
            {
                pol.Policy_ID = row["policy_id"].ToString();
                pol.Customer_ID = row["customer_id"].ToString();
                pol.Product_ID = row["product_id"].ToString();
                pol.Effective_Date = DateTime.Parse(row["effective_date"].ToString());
                pol.Maturity_Date = DateTime.Parse(row["matuiry_date"].ToString());
                pol.Agreement_Date = DateTime.Parse(row["agreement_date"].ToString());
                pol.Issue_Date = DateTime.Parse(row["issue_date"].ToString());
                pol.Assure_Year = Int32.Parse(row["assure_year"].ToString());
                pol.Assure_Up_To_Age = Int32.Parse(row["assure_up_to_age"].ToString());
                pol.Pay_Year = Int32.Parse(row["pay_year"].ToString());
                pol.Pay_Up_To_Age = Int32.Parse(row["pay_up_to_age"].ToString());
                pol.Age_Insure = Int32.Parse(row["age_insure"].ToString());
                pol.Address_ID = row["address_id"].ToString();
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog(Log.GenerateLog(ex));
            pol = new bl_policy();
        }
        return pol;
    }
    /// <summary>
    /// Get policy list by Customer ID
    /// </summary>
    /// <param name="customer_id"></param>
    /// <returns></returns>
    public static List<bl_policy> GetPolicyByCustomerID(string customer_id)
    {
        List<bl_policy> poList = new List<bl_policy>();
        try
        {
            bl_policy pol;
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_POLICY_GET_BY_CUSTOMER_ID", new string[,] {
            {"@customer_id", customer_id}
            }, "da_policy=>GetPolicy(string policy_id)");

            foreach (DataRow row in tbl.Rows)
            {
                pol = new bl_policy();
                pol.Policy_ID = row["policy_id"].ToString();
                pol.Customer_ID = row["customer_id"].ToString();
                pol.Product_ID = row["product_id"].ToString();
                pol.Effective_Date = DateTime.Parse(row["effective_date"].ToString());
                pol.Maturity_Date = DateTime.Parse(row["maturity_date"].ToString());
                pol.Agreement_Date = DateTime.Parse(row["agreement_date"].ToString());
                pol.Issue_Date = DateTime.Parse(row["issue_date"].ToString());
                pol.Assure_Year = Int32.Parse(row["assure_year"].ToString());
                pol.Assure_Up_To_Age = Int32.Parse(row["assure_up_to_age"].ToString());
                pol.Pay_Year = Int32.Parse(row["pay_year"].ToString());
                pol.Pay_Up_To_Age = Int32.Parse(row["pay_up_to_age"].ToString());
                pol.Age_Insure = Int32.Parse(row["age_insure"].ToString());
                pol.Address_ID = row["address_id"].ToString();
                poList.Add(pol);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog(Log.GenerateLog(ex));
            poList = new List<bl_policy>();
        }
        return poList;
    }

    /// <summary>
    /// Get policy by Policy ID
    /// </summary>
    /// <param name="Policy_ID"></param>
    /// <returns></returns>
    public static string GetEMAmountByPolicyID(string policy_id)
    {
        double extra_premium = 0.0;

        string connString = AppConfiguration.GetConnectionString();

        using (SqlConnection con = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //call store procedure by name
            cmd.CommandText = "SP_GET_EM_PREM_BY_POLICY_ID";

            //bind parameter            
            cmd.Parameters.AddWithValue("@Policy_ID", policy_id);

            cmd.Connection = con;
            con.Open();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.HasRows)
                    {
                        extra_premium = rdr.GetDouble(rdr.GetOrdinal("EM_Amount"));
                    }
                }
            }
            catch (Exception ex)
            {
                //Add error to log 
                Log.AddExceptionToLog("Error in function [GetEMPremByPolicyID] in class [da_policy]. Details: " + ex.Message);
            }

        }
        return String.Format("{0:N0}", extra_premium);
    }

}