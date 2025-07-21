using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.Security;
using System.Data.SqlClient;
using System.Collections;

//SVN

public partial class Pages_Business_add_rider : System.Web.UI.Page
{
    #region public variables
    string str_policy_id = "";
    
    bl_app_add_riders rider;
    List<bl_app_add_riders> riderslist = da_app_add_riders.riders_list;
    DateTime life_assure_birth_date;
    string sub_product_id = "";
    string pub_prouct_id = "";

    bl_policy_detail policy;
    bl_app_info_person app_person;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        //Session["SS_POLICY_ID"] = "C8FE18AE-7087-4DDC-8769-47761DAFE5C9";

        if (Session["SS_POLICY_ID"] != null)
        {
            str_policy_id = Session["SS_POLICY_ID"] + "";

        }
        else
        {
            Response.Redirect("application_form_fp6.aspx");
            divmessage.InnerHtml = "Page was dead.";
            return;
        }
        if (!Page.IsPostBack)
        {
            //load data
            riderslist.Clear();
            //clear rider to delete
            My_View_State.Rider_To_Delete = "";
            My_View_State.Selected_Rider_Index = -1;

            My_View_State.Page_Is_Edited = false;
            LoadData();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        
        if (Add_Rider())
        {
           
            //divmessage.Style.Value = "success";


            My_View_State.Page_Is_Edited = true;

            //riderslist = da_app_add_riders.riders_list;
            Clear();
            divmessage.InnerHtml = "Added sussess.";
        }
        else 
        {
            divmessage.InnerHtml = "Added fail.";
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }
    protected void ImgBtnSave_Click(object sender, ImageClickEventArgs e)
    {
        if (My_View_State.Page_Is_Edited)
        {
            if (SaveRider())
            {
                DeleteRider();
                //divmessage.InnerHtml = "Saved successfully.";
                MessageAlert("Saved successfully.");
            }
            else
            {
                //divmessage.InnerHtml = "Saved fail.";
                MessageAlert("Saved fail.");
            }
        }
        else
        {
            //divmessage.InnerHtml = "Nothing to update.";
            MessageAlert("Nothing to update.");
        }
        
    }
  
    void LoadData()
    {
        try
        {
            //get policy detail       
            policy = new bl_policy_detail();
            policy = da_policy.GetPolicyDetail(str_policy_id);
            txtPolicy_Number.Text = policy.Policy_Number + "";

            app_person = da_customer.GetAppInfoPerson(policy.App_Register_ID);
            ddlApply_To.Items.Add(new ListItem(app_person.Last_Name + " " + app_person.First_Name, "1"));


            life_assure_birth_date = policy.Birth_Date;

            #region Store data in ViewState
            //ViewState["VS_OBJECT_POLICY"] = policy.Product_ID ;
            My_View_State.Product_ID = policy.Product_ID;
            My_View_State.Policy = policy;
            My_View_State.Person = app_person;
            My_View_State.Policy_ID = str_policy_id;
            My_View_State.Policy_Premium = da_policy.GetPolicyPremium(str_policy_id);
            My_View_State.prem_lot = da_policy_prem_lot.Get_Policy_Prem_Lot(str_policy_id);

            #endregion
            txtLast_Due_Date.Text = string.Format("{0:dd/MM/yyyy}", My_View_State.prem_lot.Due_Date);

            txtRider_SumInsured.Text = My_View_State.Policy_Premium.Sum_Insure + "";

            //bind all family protection product into dropdown list
            foreach (da_application_fp6.ProductFP6 pro in da_application_fp6.ProductFP6.GetProductFP6List())
            {
                //get product title 
                bl_product bl_pro = da_product.GetProductByProductID(policy.Product_ID);

                ddlInsurance_Plan.Items.Add(new ListItem(bl_pro.En_Title, bl_pro.Product_ID));
            }

            //select product by production id
            Helper.SelectedDropDownListIndex("VALUE", ddlInsurance_Plan, policy.Product_ID);

            pub_prouct_id = policy.Product_ID;
            sub_product_id = policy.Product_ID.Substring(0, 3).ToUpper().Trim();

            //get assure and pay year

            bl_product_life bl_life = da_product.GetProductLifeByProductID(policy.Product_ID);
            txtTerm_Insurance.Text = bl_life.Assure_Year + "";
            txtPayment_Period.Text = bl_life.Pay_Year + "";


            //select pay mode

            ddlPremiumMode.Items.Clear();
            List<da_application_fp6.PayMentMode> payMode = da_application_fp6.PayMentMode.GetPaymentModeList();
            foreach (da_application_fp6.PayMentMode mode in payMode)
            {
                ddlPremiumMode.Items.Add(new ListItem(mode.Mode, mode.PayMentModeID + ""));
            }

            Helper.SelectedDropDownListIndex("VALUE", ddlPremiumMode, policy.Pay_Mode + "");

            //get appliction info
            List<bl_app_register> app = da_application_fp6.GetApplication(policy.App_Register_ID);

            foreach (bl_app_register app_register in app)
            {
                txtApplication_Number.Text = app_register.App_Number;
                break;
            }


            if (sub_product_id == "FPP")//family protection package
            {
                ddlPosition.Enabled = false;
                ddlCategories.Enabled = false;
                ddlPremiumMode.Enabled = false;
            }
            else if (sub_product_id == "NFP")
            {
                ddlPremiumMode.Enabled = true;
                ddlPosition.Enabled = true;
                ddlCategories.Enabled = true;
                //bind Categories
                BindCategories(ddlCategories);
            }

            //load exist riders
            if (!LoadExistRider(str_policy_id))
            {
                divmessage.InnerHtml = "Load existing riders fail, please contact your system administrator.";
            }
        }
        catch (Exception ex)
        {
            MessageAlert("Page load error.");
            ImgBtnSave.Enabled = false;
        }
      
    }
    private bool Add_Rider()
    {
       
        int level = 0;
        double sum_insure = 0;
        double premium = 0;
        double original_amount = 0;
        double original_rounded_amount = 0;
        try
        {

            if (My_View_State.Selected_Rider_Index >= 0)
            {
                #region Update Old Rider
                if (riderslist.Count > 0)
                {
                    var rider = riderslist[My_View_State.Selected_Rider_Index];

                    sum_insure = Convert.ToDouble(txtRider_SumInsured.Text.Trim());
                    premium = Convert.ToDouble(txtRider_Premium.Text.Trim());
                    original_amount = Convert.ToDouble(txtOriginal_Amount.Text.Trim());
                    original_rounded_amount = Convert.ToDouble(txtRounded_Amount.Text.Trim());

                    rider.Rider_Type = ddlRider_Type.SelectedValue.Trim().ToUpper();
                    /*
                    rider.Rider_ID = "";
                    rider.Rate_ID = 0;
                    rider.Rate = 0;

                    rider.App_Register_ID = My_View_State.Person.App_Register_ID;
                    rider.Product_ID = My_View_State.Policy.Product_ID;
                    rider.Level = rider.Level;
                     */
                    if (My_View_State.Product_ID.Substring(0, 3).ToUpper() == "NFP")
                    {

                        rider.Rate_ID = Convert.ToInt32(ddlPosition.SelectedValue);
                        rider.Rate = Convert.ToDouble(txtRate.Text.Trim());
                    }

                    rider.Sum_Insured = sum_insure;
                    rider.Premium = premium;
                    rider.Discount = 0;
                    rider.Original_Amount = original_amount;
                    rider.Annual_Rounded_Amount = original_rounded_amount;
                    

                    rider.Age_Insure = My_View_State.age_assure_pay.Age_Insure;
                    rider.Pay_Year = My_View_State.age_assure_pay.Pay_Year;
                    rider.Pay_Up_To_Age = My_View_State.age_assure_pay.Pay_Up_To_Age;
                    rider.Assure_Year = My_View_State.age_assure_pay.Assure_Year;
                    rider.Assure_Up_To_Age = My_View_State.age_assure_pay.Assure_Up_To_Age;
                    rider.Effective_Date = Helper.FormatDateTime(txtEffective_Date_System.Text.Trim());

                }
                #endregion

            }
            else
            {
                #region Add New Rider
                sum_insure = Convert.ToDouble(txtRider_SumInsured.Text.Trim());
                premium = Convert.ToDouble(txtRider_Premium.Text.Trim());
                original_amount = Convert.ToDouble(txtOriginal_Amount.Text.Trim());
                original_rounded_amount = Convert.ToDouble(txtRounded_Amount.Text.Trim());

                level = 1;
                rider = new bl_app_add_riders();
                rider.Rider_Type = ddlRider_Type.SelectedValue.Trim().ToUpper();
                rider.Rider_ID = "";
                rider.App_Register_ID = My_View_State.Person.App_Register_ID;
                rider.Product_ID = My_View_State.Policy.Product_ID;
                rider.Level = level;
                rider.Sum_Insured = sum_insure;
                rider.Premium = premium;
                rider.Discount = 0;
                rider.Original_Amount = original_amount;
                rider.Annual_Rounded_Amount = original_rounded_amount;
                
                if (My_View_State.Product_ID.Substring(0, 3).ToUpper() == "NFP")
                {

                    rider.Rate_ID = Convert.ToInt32(ddlPosition.SelectedValue);
                    rider.Rate = Convert.ToDouble(txtRate.Text.Trim());
                }
                else if (My_View_State.Product_ID.Substring(0, 3).ToUpper() == "FPP") 
                {
                    rider.Rate_ID = 0;
                    rider.Rate = 0;
                }

                rider.Age_Insure = My_View_State.age_assure_pay.Age_Insure;
                rider.Pay_Year = My_View_State.age_assure_pay.Pay_Year;
                rider.Pay_Up_To_Age = My_View_State.age_assure_pay.Pay_Up_To_Age;
                rider.Assure_Year = My_View_State.age_assure_pay.Assure_Year;
                rider.Assure_Up_To_Age = My_View_State.age_assure_pay.Assure_Up_To_Age;
                rider.Effective_Date = Helper.FormatDateTime(txtEffective_Date_System.Text.Trim());
                
                da_app_add_riders.AddRiders(rider);
                #endregion
            }

            //show in grid view
            BindGridView();
           
            return true;
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error funcaton [Add_Rider] in class [add_rider.aspx.cs], Detail: " + ex.Message);
            return false;   
        }
        
    }
    private void BindCategories(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(".", ""));

        ArrayList arrList = da_application_fp6.GetADBCategories();
        if (arrList.Count > 0)
        {
            for (int i = 0; i < arrList.Count; i++)
            {
                ddl.Items.Add(new ListItem(arrList[i].ToString(), arrList[i].ToString()));
            }

        }
    }

    private void BindPosition(string category, DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem(".", ""));

        ArrayList arrList = da_application_fp6.GetPosition(category);
        if (arrList.Count > 0)
        {
            for (int i = 0; i < arrList.Count; i++)
            {
                int value = 0;
                string text = "";
                char[] c = new char[] { ',' };
                string[] str = arrList[i].ToString().Split(c);
                value = Convert.ToInt32(str[1]);
                text = str[0];
                ddl.Items.Add(new ListItem(text, value + ""));


            }

        }
    }
    void  ADBPremium()
    {
        string pre_annual = "";

        double rate = 0.0;
        try
        {
            if (ddlCategories.SelectedIndex > 0 && ddlPosition.SelectedIndex > 0 && ddlRider_Type.SelectedIndex>0)
            {
                string class_name = "";
                //get class name
                class_name = "Class " + da_application_fp6.GetADBRate(ddlCategories.SelectedValue.Trim(), ddlPosition.SelectedItem.Text.Trim());
                //get rate base on class name
                rate = da_application_fp6.GetClassRate(class_name);

                txtRate.Text = rate+"";

                //get premium
                double sum_insured = Convert.ToDouble(txtRider_SumInsured.Text.Trim());
                int pay_mode =Convert.ToInt32( ddlPremiumMode.SelectedValue.Trim());
                pre_annual = da_application_fp6.GetADBPremium(sum_insured, ddlInsurance_Plan.SelectedItem.Text.Trim(), rate, pay_mode);

               string[] prem = pre_annual.Split('/');
               txtRider_Premium.Text = prem[0] + "";
               txtOriginal_Amount.Text = prem[1]+"";
               txtRounded_Amount.Text = Math.Ceiling(Convert.ToDouble(prem[1]))+"";

            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ADBPremium] in page [add_rider.aspx.cs], Detail: " + ex.Message);
           
        }
       
    }
   

    //protected void gvRider_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    string appID = ViewState["appID"] + "";
    //    DataTable tblRider = (DataTable)ViewState["tblRider"];

    //    if (tblRider.Rows.Count > 0)
    //    {

    //        int index = e.NewSelectedIndex;
    //        if (tblRider.Rows[index]["level"].ToString().Trim() == "1")
    //        {

    //            btnAdd.Text = "Update";
    //            Helper.SelectedDropDownListIndex("VALUE", ddlRider, tblRider.Rows[index]["rider_type"].ToString().Trim());
    //            Helper.SelectedDropDownListIndex("VALUE", ddlRiderPerson, tblRider.Rows[index]["level"].ToString().Trim());
    //            txtRiderSumInsured.Text = tblRider.Rows[index]["sumInsured"].ToString().Trim();
    //            txtRiderPremium.Text = tblRider.Rows[index]["premium"].ToString().Trim();

    //            txtOriginalAmount.Text = tblRider.Rows[index]["original_amount"].ToString().Trim();
    //            txtRoundedAmount.Text = tblRider.Rows[index]["rounded_amount"].ToString().Trim();

    //            //ADB
    //            if (tblRider.Rows[index]["rider_type"].ToString().Trim() == "ADB")
    //            {
    //                string cate = "";
    //                int rateID = 0;
    //                double rate = 0.0;

    //                ddlClassRate.Enabled = true;
    //                ddlPosition.Enabled = true;

    //                rateID = Convert.ToInt32(tblRider.Rows[index]["rate_id"].ToString().Trim());
    //                rate = Convert.ToDouble(tblRider.Rows[index]["rate"].ToString().Trim());
    //                cate = da_application_fp6.GetCategoryByNo(rateID);

    //                txtADBRate.Text = rate + "";

    //                BindPosition(cate, ddlPosition);

    //                Helper.SelectedDropDownListIndex("VALUE", ddlClassRate, cate);
    //                Helper.SelectedDropDownListIndex("VALUE", ddlPosition, rateID + "");
    //            }
    //            //TPD
    //            else
    //            {
    //                txtADBRate.Text = "";
    //                ddlClassRate.Enabled = false;
    //                ddlPosition.Enabled = false;
    //                ddlPosition.SelectedIndex = 0;
    //                ddlClassRate.SelectedIndex = 0;
    //            }


    //            ViewState["tblRiderRowIndex"] = index;
    //        }
    //    }
    //    else
    //    {
    //        lblMessageInsurancePlan.Text = "No row selected.";
    //    }
    //}
    //protected void gvRider_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    string appID = ViewState["appID"] + "";
    //    DataTable tblRider = (DataTable)ViewState["tblRider"];

    //    if (tblRider.Rows.Count > 0)
    //    {

    //        int index = e.RowIndex;
    //        if (tblRider.Rows[index]["rider_id"].ToString().Trim() != "")
    //        {
    //            ViewState["tblRiderID"] += tblRider.Rows[index]["rider_id"].ToString().Trim() + ",";
    //        }
    //        tblRider.Rows[index].Delete();
    //        tblRider.AcceptChanges();
    //        ViewState["tblRider"] = tblRider;

    //        // gvRider.DataSource = tblRider;
    //        //gvRider.DataBind();
    //        TotalPremiumAndRider();
    //    }
    //    else
    //    {
    //        lblMessageInsurancePlan.Text = "No row selected.";
    //    }
    //}
    //protected void gvPersonalInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
    //    {

    //        // when mouse is over the row, save original color to new attribute, and change it to highlight color
    //        e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#00BFFF'");

    //        // when mouse leaves the row, change the bg color to its original value  
    //        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

    //    }
    //}

    protected void ddlRider_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            sub_product_id = My_View_State.Policy.Product_ID.Substring(0, 3).ToUpper().Trim();
            pub_prouct_id = My_View_State.Policy.Product_ID;

            Session["SS_POLICY_ID"] = My_View_State.Policy_ID;

            if (sub_product_id == "NFP")
            {
                if (ddlRider_Type.SelectedItem.Text.Trim().ToUpper() == "ADB")
                {
                    ddlPosition.Enabled = true;
                    ddlCategories.Enabled = true;
                    BindCategories(ddlCategories);
                }
                else if (ddlRider_Type.SelectedItem.Text.Trim().ToUpper() == "TPD")
                {
                    ddlPosition.Enabled = false;
                    ddlCategories.Enabled = false;
                    //ddlCategories.Items.Clear();
                    //ddlPosition.Items.Clear();
                    CalcPremium(pub_prouct_id);
                }
                else if (ddlRider_Type.SelectedItem.Text.Trim().ToUpper() == "SPOUSE" || ddlRider_Type.SelectedItem.Text.Trim().ToUpper() == "KID")
                {
                    Response.Redirect("application_form_fp6_rider.aspx?application_register_id=" + My_View_State.Person.App_Register_ID);
                }
            }
            else if (sub_product_id == "FPP")
            {


                ddlPosition.Enabled = false;

                ddlCategories.Enabled = false;

                if (ddlRider_Type.SelectedItem.Text.Trim().ToUpper() == "SPOUSE" || ddlRider_Type.SelectedItem.Text.Trim().ToUpper() == "KID")
                {
                    Response.Redirect("application_form_fp6_rider.aspx?application_register_id=" + My_View_State.Person.App_Register_ID);
                }
                else
                {
                    CalcPremium(pub_prouct_id);
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ddlRider_Type_SelectedIndexChanged] in class [add_rider.aspx.cs], Detial: " + ex.Message);
            divmessage.InnerHtml = "Select rider error.";
        }

        
       
    }
    protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPosition(ddlCategories.SelectedValue.Trim(), ddlPosition);
        //ADBPremium();
    }
    protected void ddlPosition_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ADBPremium();
        CalcPremium(str_policy_id);
    }
    protected void gvRider_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = -1;
        index = e.RowIndex;
       
        My_View_State.Rider_To_Delete = My_View_State.Rider_To_Delete + riderslist[index].Rider_ID + ",";
        My_View_State.Page_Is_Edited = true;

        da_app_add_riders.RemoveRider(index);
        BindGridView();
        divmessage.InnerHtml = My_View_State.Rider_To_Delete;

       
    }

    void BindGridView()
    {
        gvRider.DataSource =riderslist;
        gvRider.DataBind();
        gvRider.SelectedIndex = -1;

        //if riders was already underwriten user cannot do anything
        foreach (GridViewRow row in gvRider.Rows)
        {
            string app_id = "";
            string rider_type = "";
            int level = 0;
            app_id = ((Label)row.FindControl("lblApplicationNumber")).Text.Trim();
            rider_type = ((Label)row.FindControl("lblRiderType")).Text.Trim();
            if (rider_type == "ADB")
            {
                level = 13;
            }
            else if (rider_type == "TPD")
            {
                level = 12;
            }
            if (da_application_fpp.GetUWRiderStatus(app_id, level) != "")
            {
                row.ForeColor = System.Drawing.Color.Red;
                row.Enabled = false;
            }
        }
    }

    void CalcPremium(string product_id)
    {
        double premium = 0;
        double original_amount = 0;
        double original_rounded_amount = 0;
        int customer_age = 0;
        int gender = -1;
        int assure_year = 0;
        string rider_type = "";
        int policy_year_insured = 0;

        //Calculate customer age
        DateTime effective_date;
        DateTime dob;
                

        dob = My_View_State.Person.Birth_Date;
        effective_date = Helper.FormatDateTime(txtEffective_Date.Text.Trim());

        //Compare the effective date and current system due date
        //bl_policy_prem_lot prem_lot = da_policy_prem_lot.Get_Policy_Prem_Lot(My_View_State.Policy_ID);
       
        policy_year_insured = My_View_State.prem_lot.Prem_Year;

        if (effective_date != My_View_State.prem_lot.Due_Date)
        {
            effective_date = da_policy_prem_pay.Get_Next_Due_by(My_View_State.prem_lot.Pay_Mod, My_View_State.prem_lot.Due_Date);
            
        }
        //txtLast_Due_Date.Text = My_View_State.prem_lot.Due_Date + "";
        txtEffective_Date_System.Text = String.Format("{0:dd/MM/yyyy}", effective_date);

        customer_age = Calculation.Culculate_Customer_Age(string.Format("{0:dd/MM/yyyy}",dob), effective_date);
        gender = My_View_State.Person.Gender;
        assure_year = My_View_State.Policy.Assure_Year;

        rider_type = ddlRider_Type.SelectedValue.Trim().ToUpper();

        sub_product_id = My_View_State.Policy.Product_ID.Substring(0, 3).ToUpper().Trim();
        pub_prouct_id = My_View_State.Policy.Product_ID;
        if (sub_product_id == "NFP")// Family protection
        {
            #region Family protection
 
            string pre_annual = "";
                        string class_name = "";
             double sum_insured = Convert.ToDouble(txtRider_SumInsured.Text.Trim());
                        int pay_mode = Convert.ToInt32(ddlPremiumMode.SelectedValue.Trim());
                       

            if (rider_type == "ADB")
            {
                double rate = 0.0;
                try
                {
                    if (ddlCategories.SelectedIndex > 0 && ddlPosition.SelectedIndex > 0 && ddlRider_Type.SelectedIndex > 0)
                    {
                       
                        //get class name
                        class_name = "Class " + da_application_fp6.GetADBRate(ddlCategories.SelectedValue.Trim(), ddlPosition.SelectedItem.Text.Trim());
                        //get rate base on class name
                        rate = da_application_fp6.GetClassRate(class_name);

                        txtRate.Text = rate + "";

                        //get premium
                        pre_annual = da_application_fp6.GetADBPremium(sum_insured, ddlInsurance_Plan.SelectedItem.Text.Trim(), rate, pay_mode);

                        string[] prem = pre_annual.Split('/');
                        premium = Convert.ToDouble(prem[0]);
                        original_amount = Convert.ToDouble(prem[1]);
                        original_rounded_amount = Math.Ceiling(Convert.ToDouble(prem[1]));

                        //store age_insure, pay_year, pay_up_to_age, assure_year, assure_up_to_age 
                        My_View_State.age_assure_pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(customer_age, assure_year, 65, policy_year_insured);
                        

                    }

                }
                catch (Exception ex)
                {
                    Log.AddExceptionToLog("Error function [ADBPremium] in page [add_rider.aspx.cs], Detail: " + ex.Message);

                }            
            }
            else if(rider_type=="TPD")
            {
                My_View_State.age_assure_pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(customer_age, assure_year, 65, policy_year_insured);
               
                pre_annual = da_application_fp6.GetTPDPremium(sum_insured,"", gender, customer_age, My_View_State.age_assure_pay.Assure_Year , pay_mode);
                string[] prem = pre_annual.Split('/');
                premium = Convert.ToDouble(prem[0]);
                original_amount = Convert.ToDouble(prem[1]);
                original_rounded_amount = Math.Ceiling(Convert.ToDouble(prem[1]));
               

            }
            #endregion
        }
        else if (sub_product_id == "FPP")// Family protection package
        {
            #region Family protection package
            string varlid_age_range = "";
            varlid_age_range = da_application_fpp.Varlid_Life_Insured_Age_Range(customer_age);
            #region old code
            /*
            if (varlid_age_range == "")//age is varlid
            {
                               
                if (rider_type == "TPD")
                {
                    //TPD 
                    premium = da_application_fp6.GetTPDPremiumFPPackage("", assure_year, gender, 0);
                    original_amount = premium;
                    original_rounded_amount = premium;
                }
                else if (rider_type == "ADB")
                {
                    //ADB
                    string str_class = "";
                    string[] str;
                    str = product_id.Split('/');
                    str_class = "Class " + str[1] + "/" + str[1];
                    premium = da_application_fp6.GetADBPremiumFPPackage(str_class);
                    original_amount = premium;
                    original_rounded_amount = premium;
                }

                //store age_insure, pay_year, pay_up_to_age, assure_year, assure_up_to_age 
                My_View_State.age_assure_pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(customer_age, assure_year, 65);


            }
            else
            {
                divmessage.InnerHtml = varlid_age_range;
                return;
            }
             */ 
            #endregion
            #region new code
            if (rider_type == "TPD")
            {
                //TPD 
                premium = da_application_fp6.GetTPDPremiumFPPackage("", assure_year, gender, 0);
                original_amount = premium;
                original_rounded_amount = premium;
            }
            else if (rider_type == "ADB")
            {
                //ADB
                string str_class = "";
                string[] str;
                str = product_id.Split('/');
                str_class = "Class " + str[1] + "/" + str[1];
                premium = da_application_fp6.GetADBPremiumFPPackage(str_class);
                original_amount = premium;
                original_rounded_amount = premium;
            }

            //store age_insure, pay_year, pay_up_to_age, assure_year, assure_up_to_age 
            My_View_State.age_assure_pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(customer_age, assure_year, 65, policy_year_insured);
            #endregion


            #endregion
        }

        txtRider_Premium.Text = premium + "";
        txtOriginal_Amount.Text = original_amount + "";
        txtRounded_Amount.Text = original_rounded_amount + "";
        
    }
    /// <summary>
    /// System filter only riders which has the action =add
    /// </summary>
    /// <param name="policy_id"></param>
    bool LoadExistRider(string policy_id)
    {
        bool result = false;
        try
        {
            List<da_application_fp6.bl_app_rider> li = da_application_fp6.GetAppRiderList(My_View_State.Person.App_Register_ID);

            for(int i =0 ;i<li.Count;i++)
            {
                var  rider = li[i];
                bl_app_add_riders add_rider = new bl_app_add_riders();
                add_rider.App_Register_ID = rider.App_Register_ID;
                add_rider.Age_Insure = rider.Age_Insure ;
                add_rider.Annual_Rounded_Amount = rider.Rounded_Amount;
                add_rider.Assure_Up_To_Age = rider.Assure_Up_To_Age;
                add_rider.Assure_Year = rider.Assure_Year;
                add_rider.Discount = 0;
                add_rider.Level = rider.Level;
                add_rider.Original_Amount = rider.Original_Amount;
                add_rider.Pay_Up_To_Age = rider.Pay_Up_To_Age;
                add_rider.Pay_Year = rider.Pay_Year;
                add_rider.Premium = rider.Premium;
                add_rider.Product_ID = rider.Product_ID;
                add_rider.Rate = rider.Rate;
                add_rider.Rate_ID = rider.Rate_ID;
                add_rider.Rider_Type = rider.Rider_Type;
                add_rider.Rider_ID = rider.Rider_ID;
                add_rider.Sum_Insured = rider.SumInsured;
                add_rider.Effective_Date = rider.Effective_Date;

                da_app_add_riders.AddRiders(add_rider);
            }
            result = true;
           
            BindGridView();

            

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [LoadExistRider] in class [add_rider.aspx.cs], Detail: " + ex.Message);
            result= false;
        }

        return result;
      

    }
    /// <summary>
    /// Save Riders
    /// This function use for save new and update existing riders
    /// </summary>
    private bool SaveRider()
    {
        bool result = true;
        try
        {
            foreach (bl_app_add_riders riders in riderslist)
            {
                //check underwriting status
                int level = 0;
                if (riders.Rider_Type == "TPD")
                {
                    level = 12;
                }
                else if (riders.Rider_Type == "ADB")
                {
                    level = 13;
                }
                
                if (da_application_fpp.GetUWRiderStatus(riders.App_Register_ID, level)=="")//it means rider is not yet underwrite
                {
                    if (riders.Rider_ID == "")
                    {
                        #region Save New Riders
                        da_application_fp6.bl_app_rider obj_rider = new da_application_fp6.bl_app_rider();
                        obj_rider.App_Register_ID = riders.App_Register_ID;
                        obj_rider.Rider_ID = Helper.GetNewGuid("SP_Check_App_Rider_ID", "@Rider_ID"); ;
                        obj_rider.Product_ID = riders.Product_ID;
                        obj_rider.Rider_Type = riders.Rider_Type;
                        obj_rider.SumInsured = riders.Sum_Insured;
                        obj_rider.Premium = riders.Premium;
                        obj_rider.Original_Amount = riders.Original_Amount;
                        obj_rider.Rounded_Amount = riders.Annual_Rounded_Amount;
                        obj_rider.Discount = riders.Discount;
                        obj_rider.Level = riders.Level;
                        obj_rider.Rate = riders.Rate;
                        obj_rider.Rate_ID = riders.Rate_ID;
                        obj_rider.Action = "add";
                        obj_rider.Created_On = DateTime.Now;
                        obj_rider.Age_Insure = riders.Age_Insure;
                        obj_rider.Pay_Year = riders.Pay_Year;
                        obj_rider.Pay_Up_To_Age = riders.Pay_Up_To_Age;
                        obj_rider.Assure_Year = riders.Assure_Year;
                        obj_rider.Assure_Up_To_Age = riders.Assure_Up_To_Age;
                        obj_rider.Effective_Date = riders.Effective_Date;

                        if (da_application_fp6.InsertAppRider(obj_rider))
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }

                        #endregion
                    }
                    else
                    {
                        #region Update Riders
                        da_application_fp6.bl_app_rider obj_rider = new da_application_fp6.bl_app_rider();
                        obj_rider.App_Register_ID = riders.App_Register_ID;
                        obj_rider.Rider_ID = riders.Rider_ID;
                        obj_rider.Product_ID = riders.Product_ID;
                        obj_rider.Rider_Type = riders.Rider_Type;
                        obj_rider.SumInsured = riders.Sum_Insured;
                        obj_rider.Premium = riders.Premium;
                        obj_rider.Original_Amount = riders.Original_Amount;
                        obj_rider.Rounded_Amount = riders.Annual_Rounded_Amount;
                        obj_rider.Discount = riders.Discount;
                        obj_rider.Level = riders.Level;
                        obj_rider.Rate = riders.Rate;
                        obj_rider.Rate_ID = riders.Rate_ID;
                        obj_rider.Age_Insure = riders.Age_Insure;
                        obj_rider.Pay_Year = riders.Pay_Year;
                        obj_rider.Pay_Up_To_Age = riders.Pay_Up_To_Age;
                        obj_rider.Assure_Year = riders.Assure_Year;
                        obj_rider.Assure_Up_To_Age = riders.Assure_Up_To_Age;
                        obj_rider.Effective_Date = riders.Effective_Date;

                        if (da_application_fp6.UpdateAppRider(obj_rider))
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        #endregion
                    }
                }
                
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SaveRiders] in class [add_rider.aspx.cs], Detail:" + ex.Message);
            result = false;
        }
        return result;
    }
    /// <summary>
    /// Delete Riders while user click delete button in gridview
    /// </summary>
    /// <returns></returns>
    bool DeleteRider()
    {
        bool result = false;
        try
        {
            string[] rider_id = My_View_State.Rider_To_Delete.Split(',');
            for (int i = 0; i < rider_id.Length; i++)
            {
                if (rider_id[i].ToString().Trim() != "")
                {
                    da_application_fp6.DeleteAppRiderByID(rider_id[i].Trim());
                    result = true;
               } 
            }
            BindGridView();

            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [DeleteRider] in class [add_rider_aspx.cs], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    protected void gvRider_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        
        int row_index = e.NewSelectedIndex;
        if (riderslist.Count > 0)
        {
            var rider = riderslist[row_index];
            txtEffective_Date.Text = string.Format("{0:dd/MM/yyyy}", rider.Effective_Date);
            txtEffective_Date_System.Text = string.Format("{0:dd/MM/yyyy}", rider.Effective_Date); 
            Helper.SelectedDropDownListIndex("VALUE", ddlRider_Type, rider.Rider_Type);
            txtRider_SumInsured.Text = rider.Sum_Insured + "";
            txtRider_Premium.Text = rider.Premium + "";
            txtRounded_Amount.Text = rider.Annual_Rounded_Amount + "";
            txtOriginal_Amount.Text = rider.Original_Amount + "";

            string sub_pro = "";
            sub_pro = rider.Product_ID.Substring(0, 3).Trim().ToUpper();

            Helper.SelectedDropDownListIndex("VALUE",ddlPremiumMode, My_View_State.Policy.Pay_Mode+"");

            if (sub_pro == "NFP")
            {
                //enable , disable ddlCategories and position
                if (rider.Rider_Type.ToUpper() == "ADB")
                {
                    ddlCategories.Enabled = true;
                    ddlPosition.Enabled = true;
                    txtRate.Text = rider.Rate + "";
                    int rate_id = rider.Rate_ID;
                    string cat = da_application_fp6.GetCategoryByNo(rate_id);
                   

                    Helper.SelectedDropDownListIndex("VALUE", ddlCategories, cat);
                    BindPosition(cat, ddlPosition);
                    Helper.SelectedDropDownListIndex("VALUE", ddlPosition, rate_id + "");
                }
                else
                {
                    ddlCategories.Enabled = false;
                    ddlPosition.Enabled = false;
                    ddlCategories.SelectedIndex = 0;
                    ddlPosition.SelectedIndex = 0;
                    txtRate.Text = "";
                }
        
            }
            My_View_State.Selected_Rider_Index = row_index;

        }
    }
    protected void imgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ddlRider_Type_SelectedIndexChanged(sender, e);

    }
    void Clear()
    {
        txtEffective_Date.Text = "";
        txtEffective_Date_System.Text = "";
        ddlRider_Type.SelectedIndex = 0;
        ddlApply_To.SelectedIndex = 0;

        if (My_View_State.Product_ID.Substring(0, 3).ToUpper() == "NFP")
        {
            ddlCategories.SelectedIndex = 0;
            ddlPosition.SelectedIndex = 0;
        }
        txtRate.Text = "";
        txtRider_Premium.Text = "";
        txtRounded_Amount.Text = "";
        txtOriginal_Amount.Text = "";

        //clear My_View_State
        My_View_State.Selected_Rider_Index = -1;
        gvRider.SelectedIndex = -1;

        //clear message
        divmessage.Style.Clear();
        divmessage.InnerHtml = "";

    }

    /// <summary>
    /// Class My_View_State
    /// Use for store data while first page load
    /// 
    /// </summary>
    class My_View_State 
    {
        public static string Product_ID { get; set; }
        public static string Policy_ID { get; set; }
        public static bl_policy_detail Policy { get; set; }
        public static bl_app_info_person Person { get; set; }
        //public static List<bl_app_add_riders> riderslist = da_app_add_riders.riders_list;
        public static bl_policy_premium Policy_Premium { get; set; }
        public static da_application_fpp.Cal_Age_Assure_Pay_Year age_assure_pay { get; set; }
        public static bl_policy_prem_lot prem_lot { get; set; }
        public static int Selected_Rider_Index { get; set; }
        public static string Rider_To_Delete { get; set; }
        public static bool Page_Is_Edited { get; set; }
        
    }

    private void MessageAlert(string message)
    {
        txtMessage.Text = message;
        ScriptManager.RegisterStartupScript(upContent, upContent.GetType(), "none",
          "<script> $('#ModalMessage').modal('show'); </script>", false);
    }
}