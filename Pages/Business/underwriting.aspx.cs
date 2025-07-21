using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Globalization;


public partial class Pages_Business_underwriting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region Blocked Code

            //string query = "	SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Birth_Date], [Sale_Agent_ID], " +
            //               " [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], " +
            //               " [User_Premium], [Rounded_Amount], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], " +
            //                " [Amount], [Original_Amount], [level] FROM (" +
            //                "SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Birth_Date], [Sale_Agent_ID], " +
            //                "[Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], " +
            //                "[User_Premium], [Rounded_Amount], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], " +
            //                " [Amount], [Original_Amount], [level] FROM [Cv_Basic_App] " +
            //                " WHERE [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_Underwriting WHERE Status_Code = 'NT' OR Status_Code = 'DC' OR Status_Code = 'CC' OR Status_Code = 'PP') AND ([level]=1 OR [level] IS NULL) AND [App_Register_ID] NOT IN (SELECT App_Register_ID FROM Ct_App_Register_Cancel) AND [Original_Amount] > 0 And (App_Register_ID <> App_Number) AND ([Status_Code] IS NULL) OR ([Status_Code] = 'AP') OR ([Status_Code] = 'CO') OR ([Status_Code] = 'MO') " +
            //                " UNION ALL " +
            //                " select '' AS 'App_Register_ID', APP_NUMBER, 'CO' AS 'STATUS', App_Date, BIRTH_DATE, SALE_AGENT_ID, " +
            //                " PRODUCT_ID, AGE_INSURE, Pay_Year, Pay_Mode_ID AS 'PAY_MODE', Assured_Year AS 'ASSURE_YEAR', Sum_Insure AS 'SYSTEM_SUM_INSURE', PREMIUM AS 'SYSTEM_PREMIUM', " +
            //                 " ACTUAL_PREMIUM AS 'USER_PREMIUM', ROUNDED_AMOUNT, FIRST_NAME, LAST_NAME, GENDER, COUNTRY_ID, [Pay_Up_To_Age], [Assure_Up_To_Age], " +
            //                 " PREMIUM AS 'AMOUNT', ORIGINAL_AMOUNT, NULL AS 'LEVEL' " +
            //                 " from [V_PRINT_POLICY_FLAT_RATE_SCHEDULE] WHERE Policy_Status_ID = 2 AND Extra_Premium>0) " +//FILTER BY STATUS NEW AND EXTRA_PREMIUM>0 
            //                 " #TEMP ORDER BY #TEMP.App_Date DESC";

            //Response.Write(query);
            //return;
            #endregion
            string query = "EXEC SP_GET_APP_TO_UNDERWRITING;";
            AppDatasource.SelectCommand = query;
            
            foreach (GridViewRow row in gvApplication.Rows)
            {
                //allow link to open underwriting for rider
                LinkButton lbtn = (LinkButton)row.FindControl("ViewRiderLink");
                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;

                //for new family protection 
                product_id = product_id.Substring(0, 3).ToUpper().Trim();
                if (product_id != "NFP" || product_id != "FPP" || product_id == "SDS")
                {
                    //lbtn.Enabled = false;
                    //lbtn.Style.Add("color", "gray");
                    lbtn.Text = "";
                }
                else
                {
                    lbtn.Enabled = true;
                    lbtn.Style.Add("color", "blue");
                }
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

    protected string GetStatusCode(string app_register_id)
    {
        string status_code = "";
        status_code = da_underwriting.GetStatusCode(app_register_id);
        return status_code;
    }


    //When inforce button is clicked
    protected void btnSaveInforce_Click(object sender, EventArgs e)
    {
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

                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                string product_id_to_store = "";
                product_id_to_store = product_id;
                product_id = product_id.Trim().Substring(0, 3).ToUpper().Trim();
                

                switch (hdfStatusCode.Value)
                {
                    case "MO":
                        //Update status code from "MO" to "IF" in table underwriting
                         //get product id from grid view
                       
                    //new family protection
                        //NFP : family protection, FPP : Family protection package
                        if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                        {
                            bool updateMORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                            updateMORecord = da_underwriting.UpdateRiderUWLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                            
                            //Open page for riders
                            string register_id = hdfAppRegisterID.Value;
                            Response.Redirect("../Business/underwriting_rider.aspx?rid=" + register_id);
                        }
                        else
                        {
                            bool updateMORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                            updateMORecord = da_underwriting.UpdateUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                        }
                        
                        break;
                    case "CO":
                        //Update status code from "CO" to "IF" in table underwriting
                        if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                        {
                            bool updateCORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                            updateCORecord = da_underwriting.UpdateRiderUWLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                            //Open page for riders
                            string register_id = hdfAppRegisterID.Value;
                            Response.Redirect("../Business/underwriting_rider.aspx?rid=" + register_id);
                        }
                        else
                        {
                            bool updateCORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                            updateCORecord = da_underwriting.UpdateUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                        }
                        
                        break;
                    case "AP":
                        //Update status code from "AP" to "IF" in table underwriting
                        if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                        {
                            bool updateAPRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                            updateAPRecord = da_underwriting.UpdateRiderUWLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                            //Open page for riders
                            string register_id = hdfAppRegisterID.Value;
                            Response.Redirect("../Business/underwriting_rider.aspx?rid=" + register_id);
                        }
                        else
                        {
                            bool updateAPRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date);
                            updateAPRecord = da_underwriting.UpdateUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                        }
                       
                        break;
                    default:
                        //add IF row to table underwriting         
                        if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                        {
                            bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date); //result IF = 2, MO = 0, NT = 1, CO = 1

                            //add row to table UW_Life_Product
                            addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                            //Open page for riders
                            string register_id = hdfAppRegisterID.Value;
                            Response.Redirect("../Business/underwriting_rider.aspx?rid=" + register_id);
                        }
                        else
                        {
                            bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 2, "IF", user_name, "Inforced by " + user_name + " on " + System.DateTime.Now, effective_date); //result IF = 2, MO = 0, NT = 1, CO = 1

                            //add row to table UW_Life_Product
                            addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, "", Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                            bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);
                        }
                        
                        break;  
                }

                
               // bool addEffectiveDate = da_underwriting.AddUWEffectiveDate(hdfAppRegisterID.Value, effective_date, user_name);

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }

    //When CO button is clicked
    protected void btnSaveCO_Click(object sender, EventArgs e)
    {
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

                DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value.ToString(), dtfi); //To be deleted after inserting data                

                //DateTime effective_date = DateTime.Now;
                
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtCONote.Text.Trim();

                string round_status_id = "";

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //check for existing record in underwriting table; if existed, update; if not add new row
                bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                 //get product id from grid view
                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                string product_id_to_store = "";
                product_id_to_store = product_id;

             
                product_id = product_id.Trim().Substring(0, 3).ToUpper().Trim();

                //new family protection
                if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from blank to CO in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);

                        //add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }

                    //add row to table UW_CO; assuming EF is not applicable
                    double em_percentage, em_premium, em_amount, em_rate;
                    em_percentage = Convert.ToDouble(ddlEMPercentage.SelectedItem.Value);
                    em_premium = Convert.ToDouble(hdfEMPremium.Value);
                    em_amount = Convert.ToDouble(hdfEMAmount.Value);
                    em_rate = Convert.ToDouble(hdfEMRate.Value);

                    int level = Convert.ToInt32(((Label)row.FindControl("lbl_level")).Text);
                    string uw_life_product_id = "";
                    uw_life_product_id = da_underwriting.GetUWLifeProductID(hdfAppRegisterID.Value, level);

                    bool addCORecord = da_underwriting.AddUWCO(uw_life_product_id, hdfAppRegisterID.Value, Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), em_percentage, em_rate, em_premium, em_amount, 0, 0, 0, user_name, updated_note);                                

                }
                //old product
                else 
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from blank to CO in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);

                        //add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }

                    //add row to table UW_CO; assuming EF is not applicable
                    double em_percentage, em_premium, em_amount, em_rate;
                    em_percentage = Convert.ToDouble(ddlEMPercentage.SelectedItem.Value);
                    em_premium = Convert.ToDouble(hdfEMPremium.Value);
                    em_amount = Convert.ToDouble(hdfEMAmount.Value);
                    em_rate = Convert.ToDouble(hdfEMRate.Value);
                    round_status_id = hdfRoundStatusID.Value;

                    bool addCORecord = da_underwriting.AddUWCO(hdfAppRegisterID.Value, Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), em_percentage, em_rate, em_premium, em_amount, 0, 0, 0, user_name, updated_note, round_status_id);                                

                }

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }
    
    //When save memo button is clicked
    protected void btnSaveMemo_Click(object sender, EventArgs e)
    {
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

                DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                //DateTime effective_date = DateTime.Now;
                
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtMemoNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //get product id from grid view
                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                string product_id_to_store = "";
                product_id_to_store = product_id;

                #region for new family protection 
                product_id = product_id.Trim().Substring(0, 3).ToUpper().Trim();

                if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                {
                    //check for existing record in underwriting table; if existed, update; if not add new
                    bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                    if (checkUnderwriting)
                    {
                        //update status code to MO in table underwriting (family protection using UpdateUnderwritingRiderRecord)
                        bool updateCORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 0, "MO", user_name, updated_note, effective_date);
                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 0, "MO", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value),
                                                                         Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value),
                                                                        Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value),
                                                                         Convert.ToDouble(hdfSumInsure.Value),  Convert.ToDouble(hdfSumInsure.Value),
                                                                         Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value),
                                                                        0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note,
                                                                        Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value),
                                                                        user_premium_discount, hdfBirthDate.Value.ToString() , Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));
                    }

                }

                #endregion

                #region For old product
                else
                {
                    //check for existing record in underwriting table; if existed, update; if not add new
                    bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                    if (checkUnderwriting)
                    {
                        //Update status code to "MO" in table underwriting
                        bool updateCORecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 0, "MO", user_name, updated_note, effective_date);
                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 0, "MO", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }

                }

                    #endregion For old product

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }


    protected void btnSaveNotTaken_Click(object sender, EventArgs e)
    {
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

                DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                //DateTime effective_date = DateTime.Now;
                
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtNotTakenNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //get product id from grid view
                string product_id = "";
                string product_id_to_store = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                product_id_to_store = product_id;
                product_id = product_id.Trim().Substring(0, 3).ToUpper().Trim();

                if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                {
                    //check for existing record in underwriting table; if existed, update; if not add new
                    bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                    if (checkUnderwriting)
                    {
                        //Update status code to NT in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "NT", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add NT row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "NT", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }
                else
                {
                    //check for existing record in underwriting table; if existed, update; if not add new
                    bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                    if (checkUnderwriting)
                    {
                        //Update status code to NT in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "NT", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add NT row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "NT", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }

                


                //Commented on 24/4/14

                //add CO row to table underwriting     
                //bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "NT", user_name, updated_note);

                ////add row to table UW_Life_Product
                //string created_note = txtNotTakenNote.Text;
                //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), user_name, created_note);

                //add row to table UW_Life_Prod_Cancel
                bool addNTRecord = da_underwriting.AddUWLifeProductCancel(hdfAppRegisterID.Value, user_name, updated_note);
                
                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }


    protected void btnSavePostpone_Click(object sender, EventArgs e)
    {
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

                DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                //DateTime effective_date = DateTime.Now;
                
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtPostponeNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }
                //check for existing record in underwriting table; if existed, update; if not add new
                bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                string product_id_to_store = "";
                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                product_id_to_store = product_id;
                product_id = product_id.Trim().Substring(0, 3).Trim().ToUpper();

                // new product family protection
                if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                {
                    if (checkUnderwriting)
                    {
                        //Update status code to PP
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "PP", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "PP", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }
                    //old product
                else
                {
                    if (checkUnderwriting)
                    {
                        //Update status code to PP
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "PP", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "PP", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }

               
                


                //Commented on 24/4/14

                ////add postpone row to table underwriting     
                //bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "PP", user_name, updated_note);

                ////add row to table UW_Life_Product
                //string created_note = txtPostponeNote.Text;
                //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), user_name, created_note);
                

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }


    protected void btnSaveCancel_Click(object sender, EventArgs e)
    {
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

                DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                //DateTime effective_date = DateTime.Now;
                
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtCancelNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //check for existing record in underwriting table; if existed, update; if not add new
                bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                string product_id_to_store = "";
                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                product_id_to_store = product_id;
                product_id = product_id.Trim().Substring(0, 3).Trim().ToUpper();

                //new family protection
                if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from "MO" to "IF" in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "CC", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "CC", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }
                //old product
                else
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from "MO" to "IF" in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "CC", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "CC", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }
               

                
                //Commented on 24/4/14

                //add cancel row to table underwriting     
                //bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "CC", user_name, updated_note);

                ////add row to table UW_Life_Product
                //string created_note = txtCancelNote.Text;
                //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), user_name, created_note);



                //add row to table UW_Life_Prod_Cancel
                bool addCCRecord = da_underwriting.AddUWLifeProductCancel(hdfAppRegisterID.Value, user_name, updated_note);

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }

    protected void btnSaveDecline_Click(object sender, EventArgs e)
    {
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

                DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                //DateTime effective_date = DateTime.Now;
                
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtDeclineNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //check for existing record in underwriting table; if existed, update; if not add new
                bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                string product_id_to_store = "";
                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                product_id_to_store = product_id;
                product_id = product_id.Trim().Substring(0, 3).Trim().ToUpper();

                //new family protection
                if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from "MO" to "IF" in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "DC", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "DC", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }
                //old product
                else
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from "MO" to "IF" in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "DC", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "DC", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }
                }
                //add decline row to table underwriting     
                //bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "DC", user_name, updated_note);

                //add row to table UW_Life_Product
                //string created_note = txtDeclineNote.Text;
                //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), user_name, created_note);

                //add row to table UW_Life_Prod_Cancel
                bool addDCRecord = da_underwriting.AddUWLifeProductCancel(hdfAppRegisterID.Value, user_name, updated_note);

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }

    protected void btnSaveApprove_Click(object sender, EventArgs e)
    {
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

                DateTime effective_date = Convert.ToDateTime(hdfAppDate.Value, dtfi); //To be deleted after inserting data
                //DateTime effective_date = DateTime.Now;
                
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtApproveNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //check for existing record in underwriting table; if existed, update; if not add new
                bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);
                string product_id_to_store = "";
                string product_id = "";
                product_id = gvApplication.Rows[row.RowIndex].Cells[5].Text;
                product_id_to_store = product_id;
                product_id = product_id.Trim().Substring(0, 3).Trim().ToUpper();

                //new family protection
                if (product_id == "NFP" || product_id == "FPP" || product_id == "SDS")
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from "MO" or "CO" to "IF" in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWRiderLifeProductRecord(hdfAppRegisterID.Value, 1, product_id_to_store, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value),
                                                                            Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value),
                                                                            Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value),
                                                                            Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note,
                                                                            Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(),
                                                                            Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }

                }
                //old product
                else
                {
                    if (checkUnderwriting)
                    {
                        //Update status code from "MO" or "CO" to "IF" in table underwriting
                        bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);

                    }
                    else
                    {
                        //add MO row to table underwriting     
                        bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);

                        ////add row to table UW_Life_Product
                        addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value),
                                                                            Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value),
                                                                            Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value),
                                                                            Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note,
                                                                            Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(),
                                                                            Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                    }

                }
               
                ////add CO row to table underwriting     
                //bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note);

                ////add row to table UW_Life_Product
                //string created_note = txtApproveNote.Text;
                //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), user_name, created_note);
                

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }

    protected void btnUndoUW_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvApplication.Rows)
        {
            //Get row that check is true
            CheckBox myChkBox = (CheckBox)row.FindControl("ckb1");

            if (myChkBox.Checked)
            {
                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                //Delete rows from table underwriting, uw_life_product, uw_co
                da_underwriting.UndoUnderwriting(hdfAppRegisterID.Value);

                //refresh gridview
                gvApplication.DataBind();
            }

        }
    }
    //Refresh gridview and load data
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        //refresh gridview
        gvApplication.DataBind();
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
    protected void gvApplication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            //int index = row.RowIndex;

            LinkButton lbtn1 = ((LinkButton)row.FindControl("lbtn1"));
            if (lbtn1.Text != "")
            {
                //Get policy ID
                string register_id = Convert.ToString(e.CommandArgument);
                Response.Redirect("../Business/underwriting_rider.aspx?rid=" + register_id);
            }
           
        }
    }
    //TODO: Get TPD by Application_id 11102016
    protected string GetTPD(string app_register_id)
    {
        string strTpd = "";
        double dTpt = 0.0;
        dTpt = da_underwriting.GetADB_TPD(app_register_id, "TPD");
        if (dTpt == 0)
        {
            strTpd = "";
        }
        else
        {
            strTpd = dTpt + "";
        }
        return strTpd;
       
    }
    //TODO: Get ADB by Application_id 11102016
    protected string GetADB(string app_register_id)
    {
        string strAdb = "";
        double dAdb = 0.0;
        dAdb = da_underwriting.GetADB_TPD(app_register_id, "ADB");
        if (dAdb == 0)
        {
            strAdb = "";
        }
        else
        {
            strAdb = dAdb + "";
        }
        return strAdb;
    }

    protected void btnCOPrint_Click(object sender, EventArgs e)
    {
        Session["SS_APP_ID"] = hdfAppRegisterID.Value;
        Session["SS_APP_NUMBER"] =hdfAppNumber.Value; // use for flat rate
        Session["SS_DOCUMENT_DATE"] = Helper.FormatDateTime(txtDocDate.Text);
        Session["SS_WRITER_NAME"] = ddlDocWriter.SelectedItem.Text;
        Session["SS_WRITER_POSITION"] = ddlDocWriter.SelectedValue;
        Session["SS_DOCUMENT_NUMBER"] = txtDocNumber.Text;
        Session["SS_REASON"] = txtReason.Text;

        ScriptManager.RegisterStartupScript(ContentPanel, ContentPanel.GetType(), "none",
          "<script> window.open('Reports/co_inform_letter.aspx', '_blank'); </script>", false);

        
    }


    //code behind
    protected void ddp_selectIndexchanged(object sender, EventArgs e)
    {
        if (rbtnlRoundAmount.SelectedValue == "0") //Round Up
        {
            txtEMAmount.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(hdfEMPremium.Value)));
            hdfEMAmount.Value = txtEMAmount.Text;
        }
        else if (rbtnlRoundAmount.SelectedValue == "1") // Midpoint Rounding
        {
            txtEMAmount.Text = Convert.ToString(Math.Round(Convert.ToDouble(hdfEMPremium.Value), MidpointRounding.AwayFromZero));
            hdfEMAmount.Value = txtEMAmount.Text;
        }
        else 
        {
            txtEMAmount.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(hdfEMPremium.Value)));
            hdfEMAmount.Value = txtEMAmount.Text;
        }
    }

    //Clear CO input 
    //protected void ClearInput()
    //{
    //    txtSumInsured.Text = "";
    //    ddlPayMode.Text = "0";
    //    txtPremium.Text = "";
    //    ddlEMPercentage.Text = "0";
    //    txtEMRate.Text = "";
    //    txtEMPremium.Text = "";
    //    txtEMAmount.Text = "";
    //    txtCONote.Text = "";
    //}
}