using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Globalization;
using System.Data;

public partial class Pages_Business_underwriting_rider : System.Web.UI.Page
{
#region Global Variables Declaration
    string g_app_register_id = "";
    string g_type = "";
    string g_policy_id = "";
#endregion Global Variables Declaration

    protected void Page_Load(object sender, EventArgs e)
    {
       g_app_register_id = Request.Params["rid"];
       g_type = Request.Params["action"];

       if (g_type != "" && g_type !=null)
       {
           DataTable tbl = DataSetGenerator.Get_Data_Soure("Select Policy_ID from Ct_App_Policy where App_Register_ID='" + g_app_register_id + "'");
           g_policy_id = tbl.Rows[0][0].ToString().Trim();
       }

        if (!Page.IsPostBack)
        {
            
            if (g_app_register_id != null && g_app_register_id != "")
            {
               
                    BindGridView();
            }
            else
            {
                Response.Redirect("../Business/underwriting.aspx");
            }
        }
       
    }


    //Get discount
    protected string GetDiscount(string app_register_id)
    {
        string discount = da_underwriting.GetDiscount(app_register_id);
        return discount.ToString();

    }

    //Get payment mode from Payment Mode table
    protected string GetPayMode(string paymode)
    {
        string payment_mode = da_underwriting.GetPaymentMode(paymode);
        return payment_mode;
    }

    protected string GetStatusCode(string app_register_id, int level)
    {
        string status_code = "";
        status_code = da_underwriting.GetRiderStatusCode(app_register_id, level);
        return status_code;
    }


    //When inforce button is clicked
    protected void btnSaveInforce_Click(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        //Check effective date
        if (!Helper.CheckDateFormat(txtDateEffectiveDate.Text.Trim()))
        {
            MessageAlert("Effective date is not validate.");
            return;
        }
       
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            SingleRow row = single_row[0];
            //Get Effective_Date
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            DateTime effective_date = Convert.ToDateTime(txtDateEffectiveDate.Text, dtfi); 

            //get logged-in username
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;

            double user_premium_discount = 0;

            //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
            //{
            //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
            //}

            //Check inforce status of life insured 1 if not yet inforced not allow to inforce for rider
           
            string lifeInsured1Status = da_underwriting.GetStatusCode(row.AppID);
            if (lifeInsured1Status != "IF")
            {
                MessageAlert("System is not allow to inforce, because the life insured is not yet inforced.");
                return;
            }
            else if (lifeInsured1Status == "NT" ||  lifeInsured1Status == "CC" || lifeInsured1Status == "DC")
            {
                MessageAlert("System is not allow to inforce, because the life insured's status is " + lifeInsured1Status);
                return;
            }
            //allow inforce
            else
            {

                // status MO, AP, CO update to IF
                if (row.Status_Code == "MO" || row.Status_Code == "AP" || row.Status_Code == "CO")
                {
                    bool updateMORecord = false;
                    updateMORecord = da_underwriting.UpdateUnderwritingRiderRecord(row.AppID, row.Level, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                    if (updateMORecord)
                    {
                        updateMORecord = da_underwriting.UpdateRiderUWLifeProductRecord(row.AppID, row.Level, row.Product_ID, row.Age_Insure, row.Pay_Year, row.Pay_Up_To_Age, row.Assure_Year, row.Assure_Up_To_Age, row.System_Sum_Insure, row.System_Sum_Insure, row.System_Premium, row.System_Premium, user_premium_discount, row.Pay_Mode, effective_date, user_name, "", row.Original_Amount, row.Rounded_Amount, user_premium_discount, row.DOB + "", row.Gender, row.Assure_Year);
                    }
                    if (updateMORecord)
                    {
                        MessageAlert("Application was inforced successfully.");
                    }
                    else
                    {
                        MessageAlert("Application was inforced fail.");
                    }

                }
                //Status NT, PP, CC, DC not allow to update to IF
                else if (row.Status_Code == "NT" || row.Status_Code == "PP" || row.Status_Code == "CC" || row.Status_Code == "DC")
                {
                    MessageAlert("System not allow to inforce with " + row.Status_Code + " status.");
                    return;
                }
                //Statuse IF
                else if (row.Status_Code == "IF")
                {

                    MessageAlert("System was already inforced.");
                    return;
                }
                //new status code
                else if (row.Status_Code == "")
                {
                    //add IF row to table underwriting                        
                    bool addRecord = false;// da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date); //result IF = 2, MO = 0, NT = 1, CO = 1
                    addRecord = da_underwriting.AddUnderwritingRecordRider(row.AppID, row.Level, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                    if (addRecord)
                    {
                        //add row to table UW_Life_Product
                        //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(row.AppID, row.Level, row.Product_ID, row.Age_Insure, row.Pay_Year, row.Pay_Up_To_Age, row.Assure_Year, row.Assure_Up_To_Age, row.System_Sum_Insure, row.System_Sum_Insure, row.System_Premium, row.System_Premium, user_premium_discount, row.Pay_Mode, effective_date, user_name, "", row.Original_Amount, row.Rounded_Amount, user_premium_discount, row.DOB + "", row.Gender, row.Assure_Year);

                        #region If underwrite for add riders
                        //g_type variable get from query string when page load
                        if (g_type != "" && g_type!=null)
                        {
                            //DataTable tbl = DataSetGenerator.Get_Data_Soure("Select Policy_ID from Ct_App_Policy where App_Register_ID='" + g_app_register_id + "'");
                            //if (tbl.Rows.Count > 0)
                            //{
                            //    da_policy.InsertPolicyPremiumRider(g_app_register_id, tbl.Rows[0][0].ToString().Trim(), user_name, DateTime.Now, row.Level);
                            //}
                            da_policy.InsertPolicyPremiumRider(g_app_register_id, g_policy_id, user_name, DateTime.Now, row.Level);
                        }
                       
                        #endregion

                    }

                    if (addRecord)
                    {
                      
                        MessageAlert("Application was inforced successfully.");

                    }
                    else
                    {
                        MessageAlert("Application was inforced fail.");
                    }
                }
            }

            //TODO: Save effective date for rider 11102016
            da_underwriting.AddUWRiderEffectiveDate(row.AppID, effective_date, user_name, row.Level);
            
        }
        else
        {
            MessageAlert("No row selected in rider underwriting applications list.");
            return;
        }
        #region old
        /*
        foreach (GridViewRow row in gvApplication.Rows)
        {
            CheckBox myChkBox = (CheckBox)row.FindControl("ckb1");

            //Get row that check is true
            if (myChkBox.Checked)
            {
                //Get Effective_Date
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd/MM/yyyy";
                dtfi.DateSeparator = "/";

                DateTime effective_date = Convert.ToDateTime(txtDateEffectiveDate.Text, dtfi); ////To be deleted after inserting data

                //DateTime effective_date = DateTime.Now;    

                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }


                switch (hdfStatusCode.Value)
                {
                    case "MO":
                        //Update status code from "MO" to "IF" in table underwriting
                        bool updateMORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                        updateMORecord = da_underwriting.UpdateUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                        break;
                    case "CO":
                        //Update status code from "CO" to "IF" in table underwriting
                        bool updateCORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                        updateCORecord = da_underwriting.UpdateUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                        break;
                    case "AP":
                        //Update status code from "AP" to "IF" in table underwriting
                        bool updateAPRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                        updateAPRecord = da_underwriting.UpdateUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                        break;
                    default:
                        //add IF row to table underwriting                        
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date); //result IF = 2, MO = 0, NT = 1, CO = 1

                        //add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                        break;
                }


                bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);

                //refresh gridview
                gvApplication.DataBind();
            }


        }*/
        #endregion

        //Refresh gridview
        BindGridView();
    }

    //When CO button is clicked
    protected void btnSaveCO_Click(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            #region User selected row in gridview 
            SingleRow row = single_row[0];

            //Get Effective_Date
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";

            DateTime effective_date = DateTime.Now;

            //get logged-in username
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;

            string updated_note = txtCONote.Text.Trim();

            double user_premium_discount = 0;

            //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
            //{
            //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
            //}

            //check for existing record in underwriting table; if existed, update; if not add new row

            bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(row.AppID, row.Level);

            if (checkUnderwriting)
            {
                
                try // try to catch error while update data into database
                {
                    //Update status code  to CO in table underwriting rider
                    bool updateRecord = false;// da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);
                    updateRecord = da_underwriting.UpdateUnderwritingRiderRecord(row.AppID, row.Level, 1, "CO", user_name, updated_note, effective_date);
                    if (updateRecord)
                    {
                        //Update CO
                        bl_underwriting_co co = new bl_underwriting_co();
                        co.UW_Life_Product_ID = da_underwriting.GetUWLifeProductID(row.AppID, row.Level);
                        co.EM_Rate = Convert.ToDouble(txtEMRate.Text);
                        co.EM_Amount = Convert.ToDouble(txtEMAmount.Text);
                        co.EM_Premium = Convert.ToDouble(txtEMPremium.Text);
                        co.EM_Percent = Convert.ToDouble(ddlEMPercentage.SelectedItem.Value);
                        updateRecord = da_underwriting.UpdateUWCORider(co);
                    }

                    if (updateRecord)
                    {
                        MessageAlert("CO was updated successfully.");
                    }
                    else
                    {
                        MessageAlert("CO updated fail, please contact your system administrator.");
                    }
                }
                catch (Exception ex)
                {
                    Log.AddExceptionToLog("System error while user click btnSaveCO to update CO, Detail: " + ex.Message);
                    MessageAlert("CO updated fail, please contact your system administrator.");
                }
                
            }

            else
            {
                try // try to catch error while save data into database
                {
                    //add row to table underwriting     
                    bool addRecord = false;// da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);
                    addRecord = da_underwriting.AddUnderwritingRecordRider(row.AppID, row.Level, 1, "CO", user_name, updated_note, effective_date);
                    ////add row to table UW_Life_Product
                    // addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                    if (addRecord)
                    {
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(row.AppID, row.Level, row.Product_ID, row.Age_Insure, row.Pay_Year, row.Pay_Up_To_Age, row.Assure_Year, row.Assure_Up_To_Age, row.System_Sum_Insure, row.System_Sum_Insure, row.System_Premium, row.System_Premium, user_premium_discount, row.Pay_Mode, effective_date, user_name, updated_note, row.Original_Amount, row.Rounded_Amount, user_premium_discount, row.DOB + "", row.Gender, row.Assure_Year);
                        if (addRecord)
                        {
                            //add row to table UW_CO; 
                            double em_percentage, em_premium, em_amount, em_rate;
                            string uw_life_product_id = "";
                            uw_life_product_id = da_underwriting.GetUWLifeProductID(row.AppID, row.Level);
                            em_percentage = Convert.ToDouble(ddlEMPercentage.SelectedItem.Value);
                            em_premium = Convert.ToDouble(txtEMPremium.Text);
                            em_amount = Convert.ToDouble(txtEMAmount.Text);
                            em_rate = Convert.ToDouble(txtEMRate.Text);

                            addRecord = da_underwriting.AddUWCO(uw_life_product_id, row.AppID, row.System_Sum_Insure, row.System_Premium, em_percentage, em_rate, em_premium, em_amount, 0, 0, 0, user_name, updated_note);

                        }  
                    }

                    if (addRecord)
                    {
                        MessageAlert("CO was saved successfully.");
                    }
                    else
                    {
                        //Delete CO while Error
                        da_underwriting.DeleteCORiderByAppIdAndLevel(row.AppID, row.Level);
                        MessageAlert("CO saved fail, please contact your system administrator.");
                    }
                }
                catch (Exception ex)
                {
                    da_underwriting.DeleteCORiderByAppIdAndLevel(row.AppID, row.Level);
                    Log.AddExceptionToLog("System error while user click btnSaveCO to Save CO, Detail: " + ex.Message);
                    MessageAlert("CO Saved fail, please contact your system administrator.");
                }
            }
           
            //refresh gridview
            BindGridView();

            #endregion  User selected row in gridview
        }
        else
        {
            MessageAlert("No row selected from rider underwriting list.");
        }

    }

    //When save memo button is clicked
    protected void btnSaveMemo_Click(object sender, EventArgs e)
    {

        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        foreach (GridViewRow row in gvApplication.Rows)
        {

            #region get single row from grid view

            //Get row that check is true
            CheckBox myChkBox = (CheckBox)row.FindControl("ckb1");

            #endregion get single row from grid view


            if (myChkBox.Checked)
            {

                #region Get application id from grid view

                Label lblLevel = (Label)row.FindControl("lblLevel");
                int level = -1;
                level = Convert.ToInt32(lblLevel.Text);

                Label lblAppID = (Label)row.FindControl("lblAppID");
                string app_register_id = "";
                app_register_id = lblAppID.Text;

                Label lblProduct_ID = (Label)row.FindControl("lblProduct_ID");
                string product_id = "";
                product_id = lblProduct_ID.Text;

                Label lblAge_Insure = (Label)row.FindControl("lblAge_Insure");
                int age_insure = 0;
                age_insure = Convert.ToInt32(lblAge_Insure.Text);

                Label lblPay_Year = (Label)row.FindControl("lblPay_Year");
                int pay_year = 0;
                pay_year = Convert.ToInt32(lblPay_Year.Text);

                Label lblPay_Up_To_Age = (Label)row.FindControl("lblPay_Up_To_Age");
                int pay_up_to_age = 0;
                pay_up_to_age = Convert.ToInt32(lblPay_Up_To_Age.Text);

                Label lblAssure_Year = (Label)row.FindControl("lblAssure_Year");
                int assure_year = 0;
                assure_year = Convert.ToInt32(lblAssure_Year.Text);

                Label lblAssure_Up_To_Age = (Label)row.FindControl("lblAssure_Up_To_Age");
                int assure_up_to_age = 0;
                assure_up_to_age = Convert.ToInt32(lblAssure_Up_To_Age.Text);

                Label lblSystem_Sum_Insure = (Label)row.FindControl("lblSystem_Sum_Insure");
                double system_sum_insure = 0.0;
                system_sum_insure = Convert.ToDouble(lblSystem_Sum_Insure.Text);

                Label lblSystem_Premium = (Label)row.FindControl("lblSystem_Premium");
                double system_premium = 0.0;
                system_premium = Convert.ToDouble(lblSystem_Premium.Text);

                Label lblPay_Mode_Code = (Label)row.FindControl("lblPay_Mode_Code");
                int pay_mode_code = -1;
                pay_mode_code = Convert.ToInt32(lblPay_Mode_Code.Text);

                Label lblOriginal_Amount = (Label)row.FindControl("lblOriginal_Amount");
                double original_amount = 0.0;
                original_amount = Convert.ToDouble(lblOriginal_Amount.Text);

                Label lblRounded_Amount = (Label)row.FindControl("lblRounded_Amount");
                double rounded_amount = 0.0;
                rounded_amount = Convert.ToDouble(lblRounded_Amount.Text);

                Label lblGender = (Label)row.FindControl("lblGender");
                int gender = -1;

                Label lblDOB = (Label)row.FindControl("lblBirth_Date");
                DateTime dob;

                dob = Convert.ToDateTime(lblDOB.Text);
                #endregion Get application id from grid view

                //Get Effective_Date
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "MM/dd/yyyy";
                dtfi.DateSeparator = "/";

                //DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                DateTime effective_date = DateTime.Now;

                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtMemoNote.Text.Trim();

                double user_premium_discount = 0;

                if (lblGender.Text == "M")
                {
                    gender = 1;
                }
                else
                {
                    gender = 0;
                }

                //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                //{
                //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                //}

                //Get single row from grid view
                List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
                if (single_row.Count == 0)
                {
                    MessageAlert("No row selected in rider underwriting applications list.");
                    return;
                }
                else
                {
                    SingleRow myRow = new SingleRow();
                    myRow = single_row[0];
                    
                    //check for existing record in underwriting table; if existed, update; if not add new
                    //bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(app_register_id, level);

                    bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(myRow.AppID, myRow.Level);

                    if (checkUnderwriting)
                    {
                        //Update status code to "MO" in table underwriting
                        //bool updateCORecord = da_underwriting.UpdateUnderwritingRiderRecord(app_register_id, level, 0, "MO", user_name, updated_note, effective_date);
                        bool updateCORecord = da_underwriting.UpdateUnderwritingRiderRecord(myRow.AppID, myRow.Level, 0, "MO", user_name, updated_note, effective_date);
                        MessageAlert("Memo saved successfully.");
                    }
                    else
                    {
                       
                        ////add MO row to table underwriting     
                        //bool addRecord = da_underwriting.AddUnderwritingRecordRider(app_register_id, level, 0, "MO", user_name, updated_note, effective_date);

                        //////add row to table UW_Life_Product
                        //addRecord = da_underwriting.AddUWRiderLifeProductRecord(app_register_id, level, product_id, age_insure,
                        //                                                     pay_year, pay_up_to_age,
                        //                                                     assure_year, assure_up_to_age,
                        //                                                     system_sum_insure, system_sum_insure,
                        //                                                     system_premium, system_premium,
                        //                                                     0, pay_mode_code, effective_date, user_name, updated_note,
                        //                                                     original_amount, rounded_amount,
                        //                                                     user_premium_discount, dob + "", gender, assure_year);

                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecordRider(myRow.AppID, myRow.Level, 0, "MO", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(myRow.AppID, myRow.Level, myRow.Product_ID, myRow.Age_Insure,
                                                                             myRow.Pay_Year, myRow.Pay_Up_To_Age,
                                                                             myRow.Assure_Year, myRow.Assure_Up_To_Age,
                                                                             myRow.System_Sum_Insure, myRow.System_Sum_Insure,
                                                                             myRow.System_Premium, myRow.System_Premium,
                                                                             0, myRow.Pay_Mode, effective_date, user_name, updated_note,
                                                                             myRow.Original_Amount, myRow.Rounded_Amount,
                                                                             user_premium_discount, myRow.DOB + "", myRow.Gender, myRow.Assure_Year);
                        if (!addRecord)
                        {
                            RollBack(myRow.AppID, myRow.Level);

                            //Alert message
                            //ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "alert", "alert('Save memo fail, please contact your system administrator.');", true);
                            //return;
                            MessageAlert("Memo saved fail, please contact your system administrator.     :-(");

                            return;
                        }
                        else
                        {
                            MessageAlert("Memo saved successfully.");
                        }

                    }
                    //Clear memo textbox
                    txtMemoNote.Text = "";

                    //refresh gridview
                    BindGridView();
                    break;
                }
                
            }

        }
    }


    protected void btnSaveNotTaken_Click(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        //get single row from grid view
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count == 0)
        {
            MessageAlert("No row selected in rider underwriting applications list.");
            return;
        }
        else
        {
            //Get Effective_Date
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";

            //DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
            DateTime effective_date = DateTime.Now;

            //get logged-in username
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;

            string updated_note = txtNotTakenNote.Text.Trim();

            double user_premium_discount = 0;

            //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
            //{
            //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
            //}

            SingleRow myRow = new SingleRow();
            myRow = single_row[0];

            //check for existing record in underwriting table; if existed, update; if not add new
            bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(myRow.AppID, myRow.Level);

            if (checkUnderwriting)
            {
                //Update status code to NT in table underwriting
                bool updateRecord = false;// da_underwriting.UpdateUnderwritingRecord(myRow.AppID, 1, "NT", user_name, updated_note, effective_date);
                updateRecord = da_underwriting.UpdateUnderwritingRiderRecord(myRow.AppID, myRow.Level, 1, "NT", user_name, updated_note, effective_date);
                //alert message to user
                if (updateRecord)
                {
                    MessageAlert("[Not Taken] saved successfully.");
                }
                else
                {
                    MessageAlert("[Not Taken] saved fail, please contact your system administrator.");
                    return;
                }
            }
            //save new record 
            else
            {
                //add NT row to table underwriting     
                bool addRecord = false;
                //addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "NT", user_name, updated_note, effective_date);
                addRecord = da_underwriting.AddUnderwritingRecordRider(myRow.AppID, myRow.Level, 1, "NT", user_name, updated_note, effective_date);
                ////add row to table UW_Life_Product
                // addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                addRecord = da_underwriting.AddUWLifeProductRecord(myRow.AppID, myRow.Product_ID, myRow.Age_Insure, myRow.Pay_Year, myRow.Pay_Up_To_Age, myRow.Assure_Year, myRow.Assure_Up_To_Age, myRow.System_Sum_Insure, myRow.System_Sum_Insure, myRow.System_Premium, myRow.System_Premium, user_premium_discount, myRow.Pay_Mode, effective_date, user_name, updated_note, myRow.Original_Amount, myRow.Rounded_Amount, user_premium_discount, myRow.DOB + "", myRow.Gender, myRow.Assure_Year);
            }

            //add row to table UW_Life_Prod_Cancel
            bool addNTRecord = false;
            //addNTRecord = da_underwriting.AddUWLifeProductCancel(hdfAppRegisterID.Value, user_name, updated_note);
            addNTRecord = da_underwriting.AddUWLifeProductCancel(myRow.AppID, user_name, updated_note);
        }
        #region old code
        /*
        foreach (GridViewRow row in gvApplication.Rows)
        {
            //Get row that check is true
            CheckBox myChkBox = (CheckBox)row.FindControl("ckb1");

            if (myChkBox.Checked)
            {

                //Get Effective_Date
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "MM/dd/yyyy";
                dtfi.DateSeparator = "/";

                //DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                DateTime effective_date = DateTime.Now;

                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtNotTakenNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //get single row from grid view
                List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
                if (single_row.Count == 0)
                {
                    MessageAlert("No row selected in rider underwriting applications list.");
                    return;

                }
                else
                {
                    SingleRow myRow = new SingleRow();
                    myRow = single_row[0];

                    //check for existing record in underwriting table; if existed, update; if not add new
                    bool checkUnderwriting = da_underwriting.CheckUnderwriting(myRow.AppID);

                    if (checkUnderwriting)
                    {
                        //Update status code to NT in table underwriting
                        bool updateRecord = false;// da_underwriting.UpdateUnderwritingRecord(myRow.AppID, 1, "NT", user_name, updated_note, effective_date);
                        updateRecord = da_underwriting.UpdateUnderwritingRiderRecord(myRow.AppID, myRow.Level, 1, "NT", user_name, updated_note, effective_date);
                        //alert message to user
                        if (updateRecord)
                        {
                            MessageAlert("[Not Taken] saved successfully.");
                        }
                        else
                        {
                            MessageAlert("[Not Taken] saved fail, please contact your system administrator.");
                            return;
                        }
                    }
                        //save new record 
                    else
                    {
                        //add NT row to table underwriting     
                        bool addRecord = false;
                        //addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "NT", user_name, updated_note, effective_date);
                        addRecord = da_underwriting.AddUnderwritingRecordRider(myRow.AppID, myRow.Level, 1, "NT", user_name, updated_note, effective_date);
                        ////add row to table UW_Life_Product
                       // addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                        addRecord = da_underwriting.AddUWLifeProductRecord(myRow.AppID, myRow.Product_ID, myRow.Age_Insure, myRow.Pay_Year, myRow.Pay_Up_To_Age, myRow.Assure_Year, myRow.Assure_Up_To_Age, myRow.System_Sum_Insure, myRow.System_Sum_Insure, myRow.System_Premium, myRow.System_Premium, user_premium_discount, myRow.Pay_Mode, effective_date, user_name, updated_note, myRow.Original_Amount, myRow.Rounded_Amount, user_premium_discount, myRow.DOB + "", myRow.Gender, myRow.Assure_Year);
                    }

                    //add row to table UW_Life_Prod_Cancel
                    bool addNTRecord = false; 
                    //addNTRecord = da_underwriting.AddUWLifeProductCancel(hdfAppRegisterID.Value, user_name, updated_note);
                    addNTRecord = da_underwriting.AddUWLifeProductCancel(myRow.AppID, user_name, updated_note); 
                }
            }
            
        }*/
        #endregion

        //refresh gridview
        BindGridView();
    }


    protected void btnSavePostpone_Click(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            SingleRow row = single_row[0];
            //Get Effective_Date
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";

            //DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
            DateTime effective_date = DateTime.Now;

            //get logged-in username
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;

            string updated_note = txtPostponeNote.Text.Trim();

            double user_premium_discount = 0;

            //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
            //{
            //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
            //}

            //check for existing record in underwriting table; if existed, update; if not add new
            bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(row.AppID, row.Level);

            if (checkUnderwriting)
            {
                //Update status code to PP
                bool updateRecord = false;// da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "PP", user_name, updated_note, effective_date);
                updateRecord = da_underwriting.UpdateUnderwritingRiderRecord(row.AppID, row.Level, 1, "PP", user_name, updated_note, effective_date);
                if (updateRecord)
                {
                    MessageAlert("[Postpone] updated successfully.");
                }
                else
                {
                    MessageAlert("[Postpone] updated fail, please contact your system administrator.");
                }
            }
            else
            {
                //add MO row to table underwriting     
                bool addRecord = false;// da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "PP", user_name, updated_note, effective_date);
                addRecord = da_underwriting.AddUnderwritingRecordRider(row.AppID, row.Level, 1, "PP", user_name, updated_note, effective_date);
                //if add record into table underwriting successfully, continue adding record into table UW_Life_Product
                if (addRecord)
                {
                    ////add row to table UW_Life_Product
                    // addRecord = false;// da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                    addRecord = da_underwriting.AddUWRiderLifeProductRecord(row.AppID, row.Level, row.Product_ID, row.Age_Insure, row.Pay_Year, row.Pay_Up_To_Age, row.Assure_Year, row.Assure_Up_To_Age, row.System_Sum_Insure, row.System_Sum_Insure, row.System_Premium, row.System_Premium, user_premium_discount, row.Pay_Mode, effective_date, user_name, updated_note, row.Original_Amount, row.Rounded_Amount, user_premium_discount, row.DOB + "", row.Gender, row.Assure_Year);
                }
                if (addRecord)
                {
                    MessageAlert("[Postpone] saved successfully.");
                }
                else
                {
                    MessageAlert("[Postpone] saved fail, please contact your system administrator.");
                }
            }

        }
        else
        {
            MessageAlert("No row selected in rider underwriting applications list.");
            return;
        }
        //refresh grid view
        BindGridView();
      
    }

    protected void btnSaveCancel_Click(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            SingleRow row = new SingleRow();
            row = single_row[0];

            //Get Effective_Date
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";

            //DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
            DateTime effective_date = DateTime.Now;

            //get logged-in username
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;

            string updated_note = txtCancelNote.Text.Trim();

            double user_premium_discount = 0;

            //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
            //{
            //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
            //}

            //check for existing record in underwriting table; if existed, update; if not add new
            bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(row.AppID, row.Level);

            if (checkUnderwriting)
            {
                bool updateRecord = da_underwriting.UpdateUnderwritingRiderRecord(row.AppID,row.Level, 1, "CC", user_name, updated_note, effective_date);
                if (updateRecord)
                {
                    MessageAlert("[Cancel] updated successfully.");
                }
                else
                {
                    MessageAlert("[Cancel] updated fail, please contact your system administrator.");
                    return;
                }
            }
            else
            {
                //add MO row to table underwriting     
                bool addRecord = da_underwriting.AddUnderwritingRecordRider(row.AppID, row.Level, 1, "CC", user_name, updated_note, effective_date);
                if (addRecord)
                {

                    ////add row to table UW_Life_Product
                    addRecord = da_underwriting.AddUWRiderLifeProductRecord(row.AppID, row.Level, row.Product_ID, row.Age_Insure, row.Pay_Year, row.Pay_Up_To_Age, row.Assure_Year, row.Assure_Up_To_Age, row.System_Sum_Insure, row.System_Sum_Insure, row.System_Premium, row.System_Premium, user_premium_discount, row.Pay_Mode, effective_date, user_name, updated_note, row.Original_Amount, row.Rounded_Amount, user_premium_discount, row.DOB + "", row.Gender, row.Assure_Year);
                    if (addRecord)
                    {

                        //add row to table UW_Life_Prod_Cancel
                        addRecord = da_underwriting.AddUWLifeProductCancel(row.AppID, user_name, updated_note);
                    }
                }
                if (addRecord)
                {
                    MessageAlert("[Cancel] saved successfully.");
                }
                else
                {
                    MessageAlert("[Cancel] saved fail, please contact your system administrator.");
                    return;
                }
            }
        }
        else
        {
            MessageAlert("No row selected in rider underwriting applications list.");
            return;
        }
        //refresh grid view
        BindGridView();
    }

    protected void btnSaveDecline_Click(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            SingleRow row = single_row[0];

            //Get Effective_Date
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";

            //DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
            DateTime effective_date = DateTime.Now;

            //get logged-in username
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;

            string updated_note = txtDeclineNote.Text.Trim();

            double user_premium_discount = 0;

            //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
            //{
            //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
            //}
            //check for existing record in underwriting table; if existed, update; if not add new

            bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(row.AppID, row.Level);

            if (checkUnderwriting)
            {
                //Update status code from "DC"
                bool updateRecord = false;// da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "DC", user_name, updated_note, effective_date);

                updateRecord = da_underwriting.UpdateUnderwritingRiderRecord(row.AppID, row.Level, 1, "DC", user_name, updated_note, effective_date);
                
                if (updateRecord)
                {
                    MessageAlert("[Decline] updated successfully.");
                }
                else
                {
                    MessageAlert("[Decline] updated fail, please contact your system administrator.");
                    return;
                }
            }
            else
            {
                //add MO row to table underwriting     
                bool addRecord = false;// da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "DC", user_name, updated_note, effective_date);

                addRecord = da_underwriting.AddUnderwritingRecordRider(row.AppID, row.Level, 1, "DC", user_name, updated_note, effective_date);
                if (addRecord)
                {
                    ////add row to table UW_Life_Product
                    //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                    addRecord = da_underwriting.AddUWRiderLifeProductRecord(row.AppID, row.Level, row.Product_ID, row.Age_Insure, row.Pay_Year, row.Pay_Up_To_Age, row.Assure_Year, row.Assure_Up_To_Age, row.System_Sum_Insure, row.System_Sum_Insure, row.System_Premium, row.System_Premium, user_premium_discount, row.Pay_Mode, effective_date, user_name, updated_note, row.Original_Amount, row.Rounded_Amount, user_premium_discount, row.DOB + "", row.Gender, row.Assure_Year);
                    if (addRecord)
                    {
                        //add row to table UW_Life_Prod_Cancel
                        //addRecord = da_underwriting.AddUWLifeProductCancel(hdfAppRegisterID.Value, user_name, updated_note);
                        addRecord = da_underwriting.AddUWLifeProductCancel(row.AppID, user_name, updated_note);
                    }
                }
                //Message alert to user
                if (addRecord)
                {
                    MessageAlert("[Decline] saved successfully.");
                }
                else
                {
                    MessageAlert("[Decline] saved fail, please contact your system administrator.");
                    return;
                }
              
            }
         
        }
        else
        {
            MessageAlert("No row selected in rider underwriting applications list.");
            return;
        }
        //refresh gridview
        BindGridView();

    }


    protected void btnSaveApprove_Click(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }

        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            SingleRow row = single_row[0];

            //Get Effective_Date
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";

            //DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
            DateTime effective_date = DateTime.Now;

            //get logged-in username
            MembershipUser myUser = Membership.GetUser();
            string user_name = myUser.UserName;

            string updated_note = txtApproveNote.Text.Trim();

            double user_premium_discount = 0;

            //if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
            //{
            //    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
            //}

            //check for existing record in underwriting table; if existed, update; if not add new
            bool checkUnderwriting = da_underwriting.CheckUnderwritingRider(row.AppID, row.Level);

            if (checkUnderwriting)
            {
                //Update status code to "AP"
                bool updateRecord = false;// da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);
                updateRecord = da_underwriting.UpdateUnderwritingRiderRecord(row.AppID, row.Level, 1, "AP", user_name, updated_note, effective_date);
                if (updateRecord)
                {
                    MessageAlert("[Approve] updated successfully.");
                }
                else
                {
                    MessageAlert("[Approve] updated fail, please contact your system administrator.");
                    return;
                }
            }
            else
            {
                //add MO row to table underwriting     
                bool addRecord = false;// da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);
                addRecord = da_underwriting.AddUnderwritingRecordRider(row.AppID, row.Level, 1, "AP", user_name, updated_note, effective_date);
                if (addRecord)
                {
                    ////add row to table UW_Life_Product
                    //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value),
                                                                        //Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value),
                                                                       // Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value),
                                                                        //Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note,
                                                                       // Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(),
                                                                        //Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                    addRecord = da_underwriting.AddUWRiderLifeProductRecord(row.AppID, row.Level, row.Product_ID, row.Age_Insure, row.Pay_Year, row.Pay_Up_To_Age, row.Assure_Year, row.Assure_Up_To_Age, row.System_Sum_Insure, row.System_Sum_Insure, row.System_Premium, row.System_Premium, user_premium_discount, row.Pay_Mode, effective_date, user_name, updated_note, row.Original_Amount, row.Rounded_Amount, user_premium_discount, row.DOB + "", row.Gender, row.Assure_Year);
                }
                //Message alert to user
                if (addRecord)
                {
                    MessageAlert("[Approve] saved successfully.");
                }
                else
                {
                    MessageAlert("[Approve] saved fail, please contact your system administrator.");
                    return;
                }

            }
        }
        else
        {

            MessageAlert("No row selected in rider underwriting applications list.");
            return;
        }

        //Refresh grid view
        BindGridView();
    }

    protected void btnUndoUW_Click(object sender, EventArgs e)
    {
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
           
            if (g_type != "" & g_type!=null)//add rider
            {
              
               List<da_officail_receipt> listRec = new List<da_officail_receipt>();

               if (single_row[0].Rider_Type.Trim().ToUpper() == "ADB" || single_row[0].Rider_Type.Trim().ToUpper() == "TPD")
               {
                   List<da_application_fp6.bl_app_rider> riders = da_application_fp6.GetAppRiderList(single_row[0].AppID);
                   //get rider id from rider list base on app id and rider type
                   foreach (da_application_fp6.bl_app_rider rider in riders)
                   {
                       if (rider.App_Register_ID.Trim() == single_row[0].AppID.Trim() && rider.Rider_Type.Trim().ToUpper() == single_row[0].Rider_Type.Trim().ToUpper() &&
                           rider.Action.ToUpper() == "ADD")
                       {
                           //get payment of rider in table official receipt rider
                           listRec = da_officail_receipt.GetOfficialReceiptRider(rider.Rider_ID);
                           break;
                       }
                   }
               }
               else 
               {
                   List<da_application_fp6.bl_app_life_product_Sub> riders = da_application_fp6.GetLifeProductSubList(single_row[0].AppID.Trim());
                   //get rider id from rider list base on app id and rider type
                   foreach (da_application_fp6.bl_app_life_product_Sub rider in riders)
                   {
                       if (rider.App_Register_ID.Trim() == single_row[0].AppID.Trim() && rider.Level == single_row[0].Level &&
                           rider.Action.ToUpper() == "ADD")
                       {
                           //get payment of rider in table official receipt rider
                           listRec = da_officail_receipt.GetOfficialReceiptRider(rider.Rider_ID);
                           break;
                       }
                   }
               }
               
               //check payment of rider in table official receipt rider
               //if it was already paid cannot undo          
                if (listRec.Count > 0)
                {
                    MessageAlert("This rider was paid, system cannot undo.");
                    return;
                }
                else
                {
                    UndoUWAddRider(single_row[0].AppID, single_row[0].Level);
                    BindGridView();
                }
               
            }
            else
            {
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;
                da_underwriting.UndoUnderwriting(single_row[0].AppID, single_row[0].Level);
                //refresh grid view
                BindGridView();
            }
        }
        else
        {
            MessageAlert("No row is seleted.");
        }
    }

    //Refresh gridview and load data
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        //refresh gridview
        BindGridView();
    }


    protected void gvApplication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
        }
    }
   

    private void BindGridView()
    {
        if (g_type != "" & g_type!=null)
        {
            DataTable tblUW = da_application_fp6.GetDataTable("SP_Get_UW_Rider_Add_By_App", g_app_register_id);
            gvApplication.DataSource = tblUW;
            gvApplication.DataBind();
            foreach (GridViewRow row in gvApplication.Rows)
            {

                row.Enabled = true;
                row.Style.Add("color", "black");

            }
        }
        else
        {
            DataTable tblUW = da_application_fp6.GetDataTable("SP_Get_UW_Rider_By_App", g_app_register_id);
            gvApplication.DataSource = tblUW;
            gvApplication.DataBind();

            //disable for IF status
            foreach (GridViewRow row in gvApplication.Rows)
            {
                CheckBox ckb1 = (CheckBox)row.FindControl("ckb1");
                LinkButton lbtn1 = (LinkButton)row.FindControl("lbtn1");
                if (lbtn1.Text == "IF")
                {
                    //lbtn1.Enabled = false;
                    //ckb1.Enabled = false;
                    row.Enabled = false;
                    row.Style.Add("color", "red");
                }

            }
        }

    }
    //private void BindGridViewAddRider()
    //{

    //    DataTable tblUW = da_application_fp6.GetDataTable("SP_Get_UW_Rider_Add_By_App", g_app_register_id);
    //    gvApplication.DataSource = tblUW;
    //    gvApplication.DataBind();
    //    foreach (GridViewRow row in gvApplication.Rows)
    //    {
           
    //            row.Enabled = true;
    //            row.Style.Add("color", "black");

    //    }

    //}
    private void MessageAlert(string message)
    {
        txtMessage.Text = message;
        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
          "<script> $('#ModalMessage').modal('show'); </script>", false);
    }
    private void HideModal(string modalId)
    {
        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
              "<script> $('#" + modalId + "').modal('hide'); </script>", false);
    }
    private void RollBack(string app_register_id, int level)
    { 
        //Delete all data in table ct_underwriting_rider while erro insert data into table ct_uw_life_product
        da_underwriting.DeleteUnderwritingRiderByAppID(app_register_id, level);
    }
    /// <summary>
    /// Rollback data in table ct_policy_premium_rider and ct_policy_status_rider while data process error.
    /// </summary>
    /// <param name="policy_id"></param>
    /// <param name="level"></param>
    private void RollBackPolicy(string policy_id, int level)
    {
        da_policy.DeletePolicyPremiumRider(policy_id, level);
        da_policy.DeletePolicyStatusRider(policy_id, level);
    }

    private void UndoUWAddRider(string app_register_id, int level)
    {
        da_underwriting.UndoUnderwriting(app_register_id, level);
        //delete record in table ct_policy_premium_rider, ct_policy_status_rider
        
        da_policy.DeletePolicyPremiumRider(g_policy_id, level);
        da_policy.DeletePolicyStatusRider(g_policy_id, level);

    }

    protected void gvApplication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "view")
        {
            string status_code = Convert.ToString(e.CommandArgument);
            string update_status = "";
            GridViewRow grow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int index = grow.RowIndex;
            Label lbl = (Label)gvApplication.Rows[index].FindControl("lblLevel");
            int level = -1;

            level = Convert.ToInt32(lbl.Text);
            //Get Status detail
            update_status = da_underwriting.GetRiderMemoStatusDetail(g_app_register_id, level);

            //memo
            if (status_code.Trim().ToLower() == "mo")
            {
                txtNote.Text = update_status;
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                "<script> $('#ResultModal').modal('show'); </script>", false);
            }
            //not taken
            else if (status_code.Trim().ToLower() == "nt")
            {
                txtNotTakenNote.Text = update_status;
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                "<script> $('#NotTakenModal').modal('show'); </script>", false);
            }
            // postpone
            else if (status_code.Trim().ToLower() == "pp")
            {
                txtPostponeNote.Text = update_status;
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
               "<script> $('#PostponeModal').modal('show'); </script>", false);
            }
            //cancel
            else if (status_code.Trim().ToLower() == "cc")
            {
                txtCancelNote.Text = update_status;
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                "<script> $('#CancelModal').modal('show'); </script>", false);
            }
            //decline
            else if (status_code.Trim().ToLower() == "dc")
            {
                txtDeclineNote.Text = update_status;
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                "<script> $('#DeclineModal').modal('show'); </script>", false);
            }
            //approve
            else if (status_code.Trim().ToLower() == "ap")
            {
                txtApproveNote.Text = update_status;
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                "<script> $('#ApproveModal').modal('show'); </script>", false);
            }
            //Counter offer
            else if (status_code.Trim().ToLower() == "co")
            {
               // txtCONote.Text = update_status;
                //ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                //"<script> $('#CODisplayModal').modal('show'); </script>", false);
                Label lbl_app_id = (Label)gvApplication.Rows[index].FindControl("lblAppID");
                string app_register_id = "";
                app_register_id = lbl_app_id.Text;
                ShowCODetail(app_register_id, level);
            }
        }
        else if (e.CommandName == "check")
        {
              GridViewRow grow = (GridViewRow)(((CheckBox)e.CommandSource).NamingContainer);
            //high light row
              gvApplication.Rows[grow.RowIndex].Style.Add("background-color", "f5f5f5");
            //row.Style.Add("background-color", "f5f5f5");
        }
        
    }
    private void ShowCODetail(string app_register_id, int level)
    {
        try
        {
           

            bl_underwriting_co co = new bl_underwriting_co();
            co = da_underwriting.GetCORiderByAppID_Level(app_register_id, level);
           
            txtDisplayCONote.Text = co.Benefit_Note;
            txtSumInsuredDisplayCO.Text = co.System_Sum_Insure + "";
            txtPremiumDisplayCO.Text = co.System_Premium + "";
            Helper.SelectedDropDownListIndex("VALUE", ddlPayModeDisplayCO, co.Pay_Mode + "");
            Helper.SelectedDropDownListIndex("VALUE", ddlEMPercentageDisplayCO, co.EM_Percent + "");
           
            txtEMRateDisplayCO.Text = co.EM_Rate + "";
            txtEMPremiumDisplayCO.Text = co.EM_Premium + "";
            txtEMAmountDisplayCO.Text = co.EM_Amount + "";

            //open CO pop form
            ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
               "<script> $('#CODisplayModal').modal('show'); </script>", false);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ShowCODetail] in page[underwriting_rider.aspx.cs], Detail: "+ ex.Message);
        }
    }
    
    class SingleRow
    {
        #region Property
        public int Level{get;set;}
        public string AppID { get; set; }
        public String Product_ID { get; set; }
        public int Age_Insure { get; set; }
        public int Pay_Year { get; set; }
        public int Pay_Up_To_Age { get; set; }
        public int Assure_Year { get; set; }
        public int Assure_Up_To_Age { get; set; }
        public double System_Sum_Insure { get; set; }
        public double System_Premium { get; set; }
        public int Pay_Mode { get; set; }
        public double Original_Amount { get; set; }
        public double Rounded_Amount { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Status_Code { get; set; }
        public DateTime Effective_Date { get; set; }
        public string Rider_Type { get; set; }
        #endregion Property

        public static List<SingleRow> GetRow(GridView myGridView)
        {
            SingleRow myRow = new SingleRow();
            List<SingleRow> singal_row = new List<SingleRow>();
            try
            {
                //get single row from grid view
                foreach (GridViewRow row in myGridView.Rows)
                {
                    CheckBox ckb = (CheckBox)row.FindControl("ckb1");
                    if (ckb.Checked)
                    {
                        Label lblLevel = (Label)row.FindControl("lblLevel");
                        int level = -1;
                        level = Convert.ToInt32(lblLevel.Text);
                        myRow.Level = level;

                        Label lblAppID = (Label)row.FindControl("lblAppID");
                        string app_register_id = "";
                        app_register_id = lblAppID.Text;
                        myRow.AppID = app_register_id;

                        Label lblProduct_ID = (Label)row.FindControl("lblProduct_ID");
                        string product_id = "";
                        product_id = lblProduct_ID.Text;
                        myRow.Product_ID = product_id;

                        Label lblAge_Insure = (Label)row.FindControl("lblAge_Insure");
                        int age_insure = 0;
                        age_insure = Convert.ToInt32(lblAge_Insure.Text);
                        myRow.Age_Insure = age_insure;

                        Label lblPay_Year = (Label)row.FindControl("lblPay_Year");
                        int pay_year = 0;
                        pay_year = Convert.ToInt32(lblPay_Year.Text);
                        myRow.Pay_Year = pay_year;

                        Label lblPay_Up_To_Age = (Label)row.FindControl("lblPay_Up_To_Age");
                        int pay_up_to_age = 0;
                        pay_up_to_age = Convert.ToInt32(lblPay_Up_To_Age.Text);
                        myRow.Pay_Up_To_Age = pay_up_to_age;

                        Label lblAssure_Year = (Label)row.FindControl("lblAssure_Year");
                        int assure_year = 0;
                        assure_year = Convert.ToInt32(lblAssure_Year.Text);
                        myRow.Assure_Year = assure_year;

                        Label lblAssure_Up_To_Age = (Label)row.FindControl("lblAssure_Up_To_Age");
                        int assure_up_to_age = 0;
                        assure_up_to_age = Convert.ToInt32(lblAssure_Up_To_Age.Text);
                        myRow.Assure_Up_To_Age = assure_up_to_age;

                        Label lblSystem_Sum_Insure = (Label)row.FindControl("lblSystem_Sum_Insure");
                        double system_sum_insure = 0.0;
                        system_sum_insure = Convert.ToDouble(lblSystem_Sum_Insure.Text);
                        myRow.System_Sum_Insure = system_sum_insure;

                        Label lblSystem_Premium = (Label)row.FindControl("lblSystem_Premium");
                        double system_premium = 0.0;
                        system_premium = Convert.ToDouble(lblSystem_Premium.Text);
                        myRow.System_Premium = system_premium;

                        Label lblPay_Mode_Code = (Label)row.FindControl("lblPay_Mode_Code");
                        int pay_mode_code = -1;
                        pay_mode_code = Convert.ToInt32(lblPay_Mode_Code.Text);
                        myRow.Pay_Mode = pay_mode_code;

                        Label lblOriginal_Amount = (Label)row.FindControl("lblOriginal_Amount");
                        double original_amount = 0.0;
                        original_amount = Convert.ToDouble(lblOriginal_Amount.Text);
                        myRow.Original_Amount = original_amount;

                        Label lblRounded_Amount = (Label)row.FindControl("lblRounded_Amount");
                        double rounded_amount = 0.0;
                        rounded_amount = Convert.ToDouble(lblRounded_Amount.Text);
                        myRow.Rounded_Amount = rounded_amount;

                        Label lblGender = (Label)row.FindControl("lblGender");
                        int gender = -1;
                        if (lblGender.Text.Trim() == "M")
                        {
                            gender = 1;
                        }
                        else
                        {
                            gender = 0;
                        }
                        myRow.Gender = gender;

                        Label lblDOB = (Label)row.FindControl("lblBirth_Date");
                        DateTime dob;
                        dob = Convert.ToDateTime(lblDOB.Text);
                        myRow.DOB = dob;

                        LinkButton lbtnStatus = (LinkButton)row.FindControl("lbtn1");
                        string status_code = "";
                        status_code = lbtnStatus.Text;
                        myRow.Status_Code = status_code;

                        Label lblEffeciveDate = (Label)row.FindControl("lblSignature");
                        DateTime effective_date;
                        effective_date = Helper.FormatDateTime(lblEffeciveDate.Text);
                        myRow.Effective_Date = effective_date;

                        Label lblRiderType = (Label)row.FindControl("lbl_rider_type");
                        myRow.Rider_Type = lblRiderType.Text;

                        //high light row
                        row.Style.Add("background-color", "f5f5f5");

                        singal_row.Add(myRow);
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error Function [GetRow] in class[SingleRow] in page [underwriting_rider.aspx.cs], Detail: " + ex.Message);
            }
            

             return singal_row;
        }
   
    }

    private bool RowSeleted()
    {
            
        bool seleted = false;
            
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            seleted = true;
        }
                
        return seleted;
    }

    protected void btnSaveInforce_Click1(object sender, EventArgs e)
    {
        if (ApplicatonIsInforce())
        {
            MessageAlert("Application was already inforce");
            return;
        }
        //check effective date
        if (Helper.CheckDateFormat(txtDateEffectiveDate.Text.Trim()))
        {
            List<SingleRow> single_row = SingleRow.GetRow(gvApplication);

            if (single_row.Count > 0)
            {
                SingleRow row = single_row[0];
                //Get age insure by effective date
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd/MM/yyyy";
                dtfi.DateSeparator = "/";
                DateTime efdate;
                efdate = Convert.ToDateTime(txtDateEffectiveDate.Text, dtfi);
                int my_age_insure = da_underwriting.GetCustomerAge(row.DOB + "", efdate + "");
                double new_premium = 0.0;
                double new_original_amount = 0.0;
                bool age_change = false;

                //If age increase, get new premium calculation based on new age
                if (my_age_insure > row.Age_Insure)
                {
                    age_change = true;
                    bl_product product = da_product.GetProductByProductID(row.Product_ID);
                    row.Age_Insure = my_age_insure;

                    //get plan assure from life product
                    da_application_fp6.ProductFP6 life_product = new da_application_fp6.ProductFP6();
                    List<da_application_fp6.ProductFP6> list_product = new List<da_application_fp6.ProductFP6>();
                    list_product = da_application_fp6.ProductFP6.GetProductFP6List();
                    int assure_plan = 0;
                    for (int i = 0; i < list_product.Count; i++)
                    {
                        var life = list_product[i];
                        if (row.Product_ID == life.ProductID)
                        {
                            life_product = life;
                            assure_plan = life.AssureYear;
                            break;
                        }
                    }
                    //calculate new assure year base on new ages
                    int new_assure_age = 0;
                    if (life_product.ProductID.Substring(0, 3).Trim().ToUpper() == "NFP")
                    {
                        //Spouse
                        if (row.Level == 2)
                        {
                            new_assure_age = row.Age_Insure + assure_plan;
                            if (new_assure_age <= 70) //70 is expired ages
                            {
                                row.Assure_Year = assure_plan;

                            }
                            else
                            {
                                // row.Assure_Year = assure_plan - row.Age_Insure;
                                row.Assure_Year = assure_plan - (new_assure_age - 70);
                            }
                            //new_premium = da_application_fp6.getPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 0);
                            //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Assure_Year, 0);
                            double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 0);
                            new_premium = arr_premium[0, 0];
                            new_original_amount = arr_premium[0, 1];
                        }
                        //Kids
                        else if (row.Level > 2 && row.Level < 5)
                        {
                            new_assure_age = row.Age_Insure + assure_plan;
                            if (new_assure_age <= 21)//21 is expired ages
                            {
                                row.Assure_Year = assure_plan;
                            }
                            else
                            {
                                row.Assure_Year = assure_plan - (new_assure_age - 21);
                            }
                            //new_premium = da_application_fp6.getPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 1);
                            //new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Assure_Year, 1);
                            double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 1);
                            new_premium = arr_premium[0, 0];
                            new_original_amount = arr_premium[0, 1];
                        }
                        else if (row.Level == 12)//TPD
                        {
                            new_assure_age = row.Age_Insure + assure_plan;
                            if (new_assure_age <= 65)// 65 is expired ages
                            {
                                row.Assure_Year = assure_plan;
                            }
                            else
                            {
                                row.Assure_Year = assure_plan - (new_assure_age - 65);
                            }
                           // new_premium = da_application_fp6.getPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 1);
                           // new_original_amount = da_application_fp6.getAnnualPremiumFP6Sub(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Assure_Year, 1);
                            string str_premium = da_application_fp6.GetTPDPremium(row.System_Sum_Insure,row.Product_ID, row.Gender,row.Age_Insure,row.Assure_Year,row.Pay_Mode);
                        }
                        
                    }

                    else if (life_product.ProductID.Substring(0, 3).Trim().ToUpper() == "FPP")//family protection package
                    {
                        //Spouse
                        if (row.Level == 2)
                        {
                            new_assure_age = row.Age_Insure + assure_plan;
                            if (new_assure_age <= 65) //70 is expired ages
                            {
                                row.Assure_Year = assure_plan;

                            }
                            else
                            {
                                // row.Assure_Year = assure_plan - row.Age_Insure;
                                row.Assure_Year = assure_plan - (new_assure_age - 65);
                            }
                            new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(row.Product_ID, row.Gender, 0,  row.Assure_Year, 0);
                            new_original_amount = new_premium;
                        }
                        //Kids
                        else if (row.Level > 2 && row.Level < 5)
                        {
                            new_assure_age = row.Age_Insure + assure_plan;
                            if (new_assure_age <= 21)//21 is expired ages
                            {
                                row.Assure_Year = assure_plan;
                            }
                            else
                            {
                                row.Assure_Year = assure_plan - (new_assure_age - 21);
                            }
                            new_premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(row.Product_ID, row.Gender, 0, row.Assure_Year, 1);
                            new_original_amount = new_premium;
                        }
                        else if (row.Level == 12)//TPD
                        {
                            new_assure_age = row.Age_Insure + assure_plan;
                            if (new_assure_age <= 65)// 65 is expired ages
                            {
                                row.Assure_Year = assure_plan;
                            }
                            else
                            {
                                row.Assure_Year = assure_plan - (new_assure_age - 65);
                            }
                            if (row.Age_Insure >= 18 && row.Age_Insure <= 60)
                            {
                                new_premium = da_application_fp6.GetTPDPremiumFPPackage(life_product.ProductID, row.Assure_Year, row.Gender, 0);
                                new_original_amount = new_premium;
                            }
                            
                        }
                        //else if (row.Level == 13)//ADB
                        //{
                        //    new_assure_age = row.Age_Insure + assure_plan;
                        //    if (new_assure_age <= 65)// 65 is expired ages
                        //    {
                        //        row.Assure_Year = assure_plan;
                        //    }
                        //    else
                        //    {
                        //        row.Assure_Year = assure_plan - (new_assure_age - 65);
                        //    }

                        //    if (row.Age_Insure >= 18 && row.Age_Insure <= 60)
                        //    {
                        //        string str_class = "";
                        //        string[] str;
                        //        str = row.Product_ID.Split('/');
                        //        str_class = "Class " + str[1] + "/" + str[1];

                        //        new_premium = da_application_fp6.GetADBPremiumFPPackage(str_class);
                        //        new_original_amount = new_premium;
                        //    }

                           
                        //}
                    }

                    #region study save
                    if (life_product.ProductID.Substring(0, 3).Trim().ToUpper() == "SDS")
                    {
                        string[] arry_product = life_product.ProductID.Split('/');
                        #region study save normal
                        if (arry_product.Length == 2)
                        {
                            //Spouse
                            if (row.Level == 2)
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 65) //65 is expired ages
                                {
                                    row.Assure_Year = assure_plan;

                                }
                                else
                                {
                                    // row.Assure_Year = assure_plan - row.Age_Insure;
                                    row.Assure_Year = assure_plan - (new_assure_age - 65);
                                }
                                double[,] arr_premium = da_application_study_save.study_save.GetPremiumRider(row.System_Sum_Insure,row.Product_ID,row.Gender,row.Age_Insure,row.Pay_Mode,row.Assure_Year,0);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                            //Kids
                            else if (row.Level > 2 && row.Level < 5)
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 21)//21 is expired ages
                                {
                                    row.Assure_Year = assure_plan;
                                }
                                else
                                {
                                    row.Assure_Year = assure_plan - (new_assure_age - 21);
                                }
                         
                                double[,] arr_premium = da_application_study_save.study_save.GetPremiumRider(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 1);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                            else if (row.Level == 12)//TPD
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 65)// 65 is expired ages
                                {
                                    row.Assure_Year = assure_plan;
                                }
                                else
                                {
                                    row.Assure_Year = assure_plan - (new_assure_age - 65);
                                }
                                double[,] arr_premium = da_application_study_save.study_save.GetTPDPremium(row.Age_Insure, row.Gender, row.Product_ID, row.Assure_Year, row.Pay_Mode, row.System_Sum_Insure);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                            else if (row.Level == 13)//ADB
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 65)// 65 is expired ages
                                {
                                    row.Assure_Year = assure_plan;
                                }
                                else
                                {
                                    row.Assure_Year = assure_plan - (new_assure_age - 65);
                                }
                                double str_class_rate = 0;
                                int rate_id = da_application_fp6.GetADBRate(row.AppID);
                                str_class_rate = da_application_fp6.GetClassRate("Class " + rate_id + row.Product_ID);
                                double[,] arr_premium = da_application_study_save.study_save.GetADBPremium(row.Product_ID,row.Pay_Mode,row.System_Sum_Insure, str_class_rate);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                        }
                      
                        #endregion
                        #region study save package
                        else if (arry_product.Length > 2)
                        {
                            //Spouse
                            if (row.Level == 2)
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 65) //65 is expired ages
                                {
                                    row.Assure_Year = assure_plan;

                                }
                                else
                                {
                                    // row.Assure_Year = assure_plan - row.Age_Insure;
                                    row.Assure_Year = assure_plan - (new_assure_age - 65);
                                }
                                double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 0);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                            //Kids
                            else if (row.Level > 2 && row.Level < 5)
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 21)//21 is expired ages
                                {
                                    row.Assure_Year = assure_plan;
                                }
                                else
                                {
                                    row.Assure_Year = assure_plan - (new_assure_age - 21);
                                }

                                double[,] arr_premium = da_application_study_save.study_save_package.GetPremiumRider(row.System_Sum_Insure, row.Product_ID, row.Gender, row.Age_Insure, row.Pay_Mode, row.Assure_Year, 1);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                            else if (row.Level == 12)//TPD
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 65)// 65 is expired ages
                                {
                                    row.Assure_Year = assure_plan;
                                }
                                else
                                {
                                    row.Assure_Year = assure_plan - (new_assure_age - 65);
                                }
                                double[,] arr_premium = da_application_study_save.study_save_package.GetTPDPremium(row.Age_Insure, row.Gender, row.Product_ID, row.Assure_Year, row.Pay_Mode);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                            else if (row.Level == 13)//ADB
                            {
                                new_assure_age = row.Age_Insure + assure_plan;
                                if (new_assure_age <= 65)// 65 is expired ages
                                {
                                    row.Assure_Year = assure_plan;
                                }
                                else
                                {
                                    row.Assure_Year = assure_plan - (new_assure_age - 65);
                                }
                                string str_class_rate = "";
                               
                                str_class_rate ="Class " +  row.Product_ID;
                                double[,] arr_premium = da_application_study_save.study_save_package.GetADBPremium(str_class_rate, row.Pay_Mode);
                                new_premium = arr_premium[0, 0];
                                new_original_amount = arr_premium[0, 1];
                            }
                        }
                        #endregion

                    }
                    #endregion

                    row.System_Premium = new_premium;
                    row.Original_Amount = new_original_amount;

                }

                txtCustomerAge.Text = row.Age_Insure + "";
                txtPremiumPreview.Text = row.System_Premium + "";
                txtAnnuallyPremiumPreview.Text = row.Original_Amount + "";

                txtEffectiveDate.Text = txtDateEffectiveDate.Text;
                if (age_change)
                {
                    dConfirmMessage.InnerHtml = "System has detected that customer's ages are different from application form.";
                }
                else
                {
                    dConfirmMessage.InnerText = "";
                }
                ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none", "<script>$('#InforceConfirmModal').modal('show'); </script>", false);
               // ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none", "<script> $('#DateSelectionModal').modal('hide'); $('#InforceConfirmModal').modal('show'); </script>", false);

            }
            else
            {
                MessageAlert("No record was selected.");
            }

        }
        else 
        {
            MessageAlert("Effective date format is not varlidate.");
        }
    }
    public double GetEMRate()
    {
        double rate = 0.0;
        try
        {
            List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
            if (single_row.Count > 0)
            {
                SingleRow row = single_row[0];
               
                double premium = 0.0;
                double percentage = 0.0;
                int paymentMode = -1;
                int insured_type = -1;

                if (row.Level == 2)
                {
                    insured_type = 0;//spouse
                }
                else if(row.Level>2 && row.Level<5)
                {
                    insured_type = 1;//kids
                }

                percentage = Convert.ToDouble(ddlEMPercentage.SelectedValue);
                //get rate , insured_type =1 is for kid
                #region Family protection package use rider (spouse, kid) extra premium rate (gender = male, age = 39) both male and female
                if (row.Product_ID.Substring(0, 3).ToUpper() == "FPP")
                {
                    rate = da_underwriting.GetEMRiderRate(insured_type, "", 1, 39, percentage, row.Assure_Year);
                }
                else
                {
                    rate = da_underwriting.GetEMRiderRate(insured_type, "", row.Gender, row.Age_Insure, percentage, row.Assure_Year);
                }
                #endregion
                // rate = 1.5;
                premium = (rate * row.System_Sum_Insure)/1000;
                premium = Math.Ceiling(premium);

               //select ddlpaymode index base on value
                Helper.SelectedDropDownListIndex("VALUE", ddlPayMode, row.Pay_Mode+"");
                
                paymentMode = Convert.ToInt32(ddlPayMode.SelectedValue);
                
                switch (paymentMode)
                {
                    case 0:
                        premium = premium * 1;
                        break;
                    case 1:
                        premium = premium * 1;
                        break;
                    case 2:
                        premium = premium * 0.52;//not used 0.54
                        break;
                    case 3:
                        premium = premium * 0.27;
                        break;
                    case 4:
                        premium = premium * 0.09;
                        break;

                }

                txtSumInsured.Text = row.System_Sum_Insure + "";
                txtPremium.Text = row.System_Premium + "";
                txtEMRate.Text = rate + "";
                txtEMPremium.Text = premium + "";
                txtEMAmount.Text = Math.Ceiling(premium) + "";

            }
            else
            {
                rate = 0.0;
            }
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetEMRate] in page [underwriting_rider.aspx.cs], Detail: " + ex.Message);
            rate = 0.0;
        }

        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
             "<script> $('#CounterOfferModal').modal('show'); </script>", false);
        return rate;

    }

    public double GetTPDEMRate()
    {
        double rate = 0.0;
        try
        {
            List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
            if (single_row.Count > 0)
            {
                SingleRow row = single_row[0];

                double premium = 0.0;
                double percentage = 0.0;
                int paymentMode = -1;
               


                percentage = Convert.ToDouble(ddlEMPercentage.SelectedValue);
                //get rate , insured_type =1 is for kid
                #region Family protection package use TPD extra premium rate (gender = male, age = 39) both male and female
                if (row.Product_ID.Substring(0, 3).ToUpper() == "FPP")
                {
                    
                    rate = da_underwriting.GetTPDEMRate("", 1, 39, percentage, row.Assure_Year);
                }
                else if (row.Product_ID.Substring(0, 3).ToUpper() == "NFP")
                {
                    rate = da_underwriting.GetTPDEMRate("", row.Gender, row.Age_Insure, percentage, row.Assure_Year);
                }
                #endregion
                #region Study save
                else if (row.Product_ID.Substring(0, 3).ToUpper() == "SDS")
                {
                   
                    rate = da_underwriting.GetTPDEMRate("", row.Gender, row.Age_Insure, percentage, row.Assure_Year);
                   
                }
                #endregion
                // rate = 1.5;
                premium = (rate * row.System_Sum_Insure) / 1000;
                premium = Math.Ceiling(premium);

                //select ddlpaymode index base on value
                Helper.SelectedDropDownListIndex("VALUE", ddlPayMode, row.Pay_Mode + "");

                paymentMode = Convert.ToInt32(ddlPayMode.SelectedValue);

                switch (paymentMode)
                {
                    case 0:
                        premium = premium * 1;
                        break;
                    case 1:
                        premium = premium * 1;
                        break;
                    case 2:
                        premium = premium * 0.52;//not used 0.54
                        break;
                    case 3:
                        premium = premium * 0.27;
                        break;
                    case 4:
                        premium = premium * 0.09;
                        break;

                }

                txtSumInsured.Text = row.System_Sum_Insure + "";
                txtPremium.Text = row.System_Premium + "";
                txtEMRate.Text = rate + "";
                txtEMPremium.Text = premium + "";
                txtEMAmount.Text = Math.Ceiling(premium) + "";

            }
            else
            {
                rate = 0.0;
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetTPDEMRate] in page [underwriting_rider.aspx.cs], Detail: " + ex.Message);
            rate = 0.0;
        }

        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
             "<script> $('#CounterOfferModal').modal('show'); </script>", false);
        return rate;

    }
    public Double GetEMRateByProductType()
    {
        double rate = 0.0;
         List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
         if (single_row.Count > 0)
         {
             SingleRow row = single_row[0];
             if (row.Level == 12)//life assure it is tpd
             {
                rate= GetTPDEMRate();
             }
             else
             { //spouse and kids 
                 rate = GetEMRate();
             }
         }
         return rate;
    }
    protected void ddlPayMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetEMRate();
        GetEMRateByProductType();
       
    }
    private void OpenPopUp(string formName)
    {
        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
              "<script> $('#" + formName + "').modal('show'); </script>", false);
    }
    private void ClosePopUp(string formName)
    {
        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
                 "<script> $('#" + formName + "').modal('hide'); </script>", false);
    }
    protected void ImageBtnApprove_Click(object sender, ImageClickEventArgs e)
    {
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            //SingleRow row = single_row[0];
            //ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
            //    "<script> $('#ApproveModal').modal('show'); </script>", false);
            OpenPopUp("ApproveModal");
        }
        else 
        {
            MessageAlert("No record was selected.");
        }
       
        
    }
    protected void ImageBtnInforce_Click(object sender, ImageClickEventArgs e)
    {
      
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            if (g_type != "" & g_type!=null)
            {

                txtDateEffectiveDate.Text = string.Format("{0:dd/MM/yyyy}", single_row[0].Effective_Date);

                txtDateEffectiveDate.Enabled = false;
            }
            else
            {
                txtDateEffectiveDate.Enabled = true;
            }
            OpenPopUp("DateSelectionModal");
        }
        else
        {
            MessageAlert("No record was selected.");
        }
    }

    protected void ImageBtnDecline_Click(object sender, ImageClickEventArgs e)
    {

        if (RowSeleted())
        {
            OpenPopUp("DeclineModal");
        }
        else
        {
            MessageAlert("No record was selected.");
        }
       
    }
    protected void ImgBtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        if (RowSeleted())
        {
            OpenPopUp("CancelModal");
        }
        else
        {
            MessageAlert("No record was selected.");
        }
    }
    protected void ImageBtnPostpone_Click(object sender, ImageClickEventArgs e)
    {
        if (RowSeleted())
        {
            OpenPopUp("PostponeModal");
        }
        else
        {
            MessageAlert("No record was selected.");
        }
    }
    protected void ImageBtnNotTaken_Click(object sender, ImageClickEventArgs e)
    {
        if (RowSeleted())
        {
            OpenPopUp("NotTakenModal");
        }
        else
        {
            MessageAlert("No record was selected.");
        }
    }
    protected void ImageBtnCounterOffer_Click(object sender, ImageClickEventArgs e)
    {
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            txtSumInsured.Text = single_row[0].System_Sum_Insure + "";
            Helper.SelectedDropDownListIndex("VALUE", ddlPayMode, single_row[0].Pay_Mode + "");
            txtPremium.Text = single_row[0].System_Premium + "";

            OpenPopUp("CounterOfferModal");
        }
       
        else
        {
            MessageAlert("No record was selected.");
        }
    }
    protected void ImageBtnMemo_Click(object sender, ImageClickEventArgs e)
    {
        if (RowSeleted())
        {
            OpenPopUp("MemoModal");
        }
        else
        {
            MessageAlert("No record was selected.");
        }
    }
    protected void ImageBtnUndo_Click(object sender, ImageClickEventArgs e)
    {
        if (RowSeleted())
        {
            OpenPopUp("UndoUnderwriteModal");
        }
        else
        {
            MessageAlert("No record was selected.");
        }
    }

    private bool ApplicatonIsInforce()
    {
        bool status = false;
        List<SingleRow> single_row = SingleRow.GetRow(gvApplication);
        if (single_row.Count > 0)
        {
            var row = single_row[0];
            if (row.Status_Code == "IF")
            {
                status = true;
            }
            else
            {
                status = false;
            }
        }
        return status;
    }
}
                       