using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.Security;

public partial class Administrator_Underwriting_underwriting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

                DateTime effective_date = Convert.ToDateTime(txtDateEffectiveDate.Text, dtfi);

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


                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtCONote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

                //check for existing record in underwriting table; if existed, update; if not add new row
                bool checkUnderwriting = da_underwriting.CheckUnderwriting(hdfAppRegisterID.Value);

                if (checkUnderwriting)
                {
                    //Update status code from blank to CO in table underwriting
                    bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);

                }
                else
                {
                    //add row to table underwriting     
                    bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "CO", user_name, updated_note, effective_date);

                    ////add row to table UW_Life_Product
                    addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

                }

                //add row to table UW_CO; assuming EF is not applicable
                double em_percentage, em_premium, em_amount, em_rate;
                em_percentage = Convert.ToDouble(ddlEMPercentage.SelectedItem.Value);
                em_premium = Convert.ToDouble(hdfEMPremium.Value);
                em_amount = Convert.ToDouble(hdfEMAmount.Value);
                em_rate = Convert.ToDouble(hdfEMRate.Value);

                bool addCORecord = da_underwriting.AddUWCO(hdfAppRegisterID.Value, Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), em_percentage, em_rate, em_premium, em_amount, 0, 0, 0, user_name, updated_note);

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

                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtMemoNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

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

                //Commented on 24/4/14

                //add row to table UW_Life_Product; Assume discount = 0
                //string created_note = txtMemoNote.Text;                
                //addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSystemPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), user_name, created_note);

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

                //get logged-in username
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;

                string updated_note = txtNotTakenNote.Text.Trim();

                double user_premium_discount = 0;

                if (Convert.ToDouble(hdfUserPremiumDiscount.Value) != 0)
                {
                    user_premium_discount = Convert.ToDouble(hdfUserPremiumDiscount.Value);
                }

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

                if (checkUnderwriting)
                {
                    //Update status code from "MO" to "IF" in table underwriting
                    bool updateRecord = da_underwriting.UpdateUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);

                }
                else
                {
                    //add MO row to table underwriting     
                    bool addRecord = da_underwriting.AddUnderwritingRecord(hdfAppRegisterID.Value, 1, "AP", user_name, updated_note, effective_date);

                    ////add row to table UW_Life_Product
                    addRecord = da_underwriting.AddUWLifeProductRecord(hdfAppRegisterID.Value, hdfProductID.Value, Convert.ToInt32(hdfAge.Value), Convert.ToInt32(hdfPayYear.Value), Convert.ToInt32(hdfPayUpToAge.Value), Convert.ToInt32(hdfAssureYear.Value), Convert.ToInt32(hdfAssureUpToAge.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfSumInsure.Value), Convert.ToDouble(hdfUserPremium.Value), Convert.ToDouble(hdfSystemPremium.Value), 0, Convert.ToInt16(hdfPayMode.Value), effective_date, user_name, updated_note, Convert.ToDouble(hdfOriginalAmount.Value), Convert.ToDouble(hdfRoundedAmount.Value), user_premium_discount, hdfBirthDate.Value.ToString(), Convert.ToInt32(hdfGender.Value), Convert.ToInt32(hdfAssureYear.Value));

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


}