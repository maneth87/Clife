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

public partial class Pages_Business_application_form_fp6_rider : System.Web.UI.Page
{
   
    //sub add benifit in to data table
    private void generateBenifitTable()
    {
        //variable
        DataTable tblBenifit = new DataTable();
        var a = tblBenifit.Columns;
        a.Add("ID_Type");
        a.Add("ID_Card");
        a.Add("full_Name");
        a.Add("relationship");
        a.Add("Percentage");
        a.Add("app_benefit_item_id");
        ViewState["tblBenifit"] = tblBenifit;
    }
    //sub add Job history in to data table
    private void generateJobHistoryTable()
    {
        //variable
        DataTable tblJob = new DataTable();
        var a = tblJob.Columns;
        a.Add("app_register_id");
        a.Add("job_id");
        a.Add("employer_name");
        a.Add("nature_of_business");
        a.Add("current_position");
        a.Add("job_role");
        a.Add("anual_income");
        a.Add("level");
        a.Add("address");
        ViewState["tblJobHistory"] = tblJob;
    }
    //sub add Job history in to data table
    private void generateBodyTable()
    {
        //variable
        DataTable tblBody = new DataTable();
        var a = tblBody.Columns;
        a.Add("app_register_id");
        a.Add("weight");
        a.Add("height");
        a.Add("reason");
        a.Add("is_weight_changed");
        a.Add("level");
        a.Add("id");
        ViewState["tblBody"] = tblBody;
    }
    //generate rider data table
    private void geneateRiderTable()
    {
        DataTable tblRider = new DataTable();
        var a = tblRider.Columns;
        a.Add("rider_id");
        a.Add("app_register_id");
        a.Add("product_id");
        a.Add("level");
        a.Add("rider_type");
        a.Add("sumInsured");
        a.Add("premium");
        a.Add("discount");
        a.Add("Rate");
        ViewState["tblRider"] = tblRider;
    }

    //Generate premium detail table
    private void GeneratePremiumDetailTable()
    {
        DataTable tblPremiumDetail = new DataTable();
        var col = tblPremiumDetail.Columns;
        col.Add("record_id");
        col.Add("level");
        col.Add("person_id");
        col.Add("app_register_id");
        col.Add("full_name");
        col.Add("gender");
        col.Add("birth_date");
        col.Add("age_insure");
        col.Add("assure_year");
        col.Add("assure_up_to_age");
        col.Add("pay_year");
        col.Add("pay_up_to_age");
        col.Add("pay_mode");
        col.Add("product_id");
        col.Add("sum_insure");
        col.Add("premium");
        col.Add("original_premium");
        col.Add("effective_date");
        col.Add("created_on");

        ViewState["tblPermiumDetail"] = tblPremiumDetail;

    }
  

    #region Database transection
   
    //save person info
    private bool insertPersonalInfoSub(string app_id)
    {
        bool result = false;
        try
        {
            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int rowCount = tblPersonal.Rows.Count;

            for (int i = 0; i < rowCount; i++)
            {

                var row = tblPersonal.Rows[i];
                if (row["level"].ToString().Trim() != "0")
                {
                    da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                    person.App_Register_ID = My_View_State.App_Register_ID;
                    person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                    person.Country_ID = row["nationality"].ToString();
                    person.Father_First_Name = row["fatherFirstName"].ToString();
                    person.Father_Last_Name = row["fatherSurName"].ToString();
                    person.First_Name = row["firstEnName"].ToString();
                    person.Last_Name = row["surEnName"].ToString();
                    person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                    person.Khmer_Last_Name = row["surKhName"].ToString();
                    person.Gender = Convert.ToInt32(row["gender"].ToString());
                    person.ID_Card = row["idNumber"].ToString();
                    person.ID_Type = Convert.ToInt32(row["idtype"].ToString());

                    person.Level = Convert.ToInt32(row["level"].ToString().Trim());
                    person.Mother_First_Name = row["motherFirstName"].ToString();
                    person.Mother_Last_Name = row["motherSurName"].ToString();
                    person.Person_ID = row["id"].ToString().Trim();
                    person.Prior_First_Name = row["previousFirstName"].ToString();
                    person.Prior_Last_Name = row["previousSurName"].ToString();

                    person.Marital_Status = row["Marital_Status"].ToString();
                    person.Relationship = row["Relationship"].ToString();

                    if (da_application_fp6.InsertAppInfoPersonSub(person))
                    {
                        result = true;
                    }
                    else
                    {
                      
                        result = false;
                    }
                }

            }

        }
        catch (Exception ex)
        {
           
            Log.AddExceptionToLog("Error insert insertPersonalInfoSub function in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            result = false;
        }
        return result;

    }
   
    //save job history list
    private bool InsertAppJobHistorySubList(string app_id)
    {
        bool status = false;
        try
        {

            DataTable tblJob = (DataTable)ViewState["tblJobHistory"];
            foreach (DataRow row in tblJob.Rows)
            {
                if (row["job_id"].ToString().Trim() == "")
                { //save
                    da_application_fp6.bl_app_job_history_sub app_job_history = new da_application_fp6.bl_app_job_history_sub();

                    app_job_history.App_Register_ID = app_id;
                    app_job_history.Anual_Income = Convert.ToDouble(row["anual_income"].ToString().Trim());
                    app_job_history.Current_Position = row["current_position"].ToString().Trim();
                    app_job_history.Employer_Name = row["employer_name"].ToString().Trim();
                    app_job_history.Job_Role = row["anual_income"].ToString().Trim();
                    app_job_history.Nature_Of_Business = row["anual_income"].ToString().Trim();
                    app_job_history.Level = Convert.ToInt32(row["Level"].ToString().Trim());
                    //app_job_history.Job_ID = row["job_id"].ToString().Trim();
                    app_job_history.Job_ID = Helper.GetNewGuid("SP_Check_App_Info_Job_Sub_ID", "@Job_ID");
                    app_job_history.Address = row["address"].ToString().Trim();

                    //save into database
                    if (da_application_fp6.InsertAppJobHistorySub(app_job_history))
                    {
                        status = true;
                    }
                    else
                    {

                        status = false;
                    }
                }
                else
                { //update

                    da_application_fp6.bl_app_job_history_sub job = new da_application_fp6.bl_app_job_history_sub();
                    job.App_Register_ID = app_id;
                    job.Nature_Of_Business = row["nature_of_business"].ToString().Trim();
                    job.Employer_Name = row["employer_name"].ToString().Trim();
                    job.Current_Position = row["current_position"].ToString().Trim();
                    job.Job_Role = row["job_role"].ToString().Trim();
                    job.Anual_Income = Convert.ToDouble(row["anual_income"].ToString().Trim());
                    job.Level = Convert.ToInt32(row["level"].ToString().Trim());
                    job.Job_ID = row["job_id"].ToString().Trim();
                    job.Address = row["address"].ToString().Trim();

                    if (da_application_fp6.UpdateAppJobHistorySub(job))
                    { //update success
                        status = true;
                    }
                    else
                    { //update fail
                        status = false;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppJobHistorySub] in class [application_form_fp6.aspx.cs]. Details: " + ex.Message);
           
            status = false;

        }
        return status;
    }
   
    //save body change
    private bool InsertAppInfoBody(string app_id)
    {
        bool status = false;
        try
        {
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            for (int i = 0; i < tblBody.Rows.Count; i++)
            {
                var drow = tblBody.Rows[i];
                //level>0 is life insured
                da_application_fp6.bl_app_info_body_sub app_info_body = new da_application_fp6.bl_app_info_body_sub();

                app_info_body.App_Register_ID = app_id;

                app_info_body.Height = Convert.ToInt32(drow["height"].ToString().Trim());
                app_info_body.Weight_Change = 0;
                app_info_body.Weight = Convert.ToInt32(drow["weight"].ToString().Trim());
                app_info_body.Is_Weight_Changed = Convert.ToInt32(drow["is_weight_changed"].ToString().Trim());
                app_info_body.Reason = drow["reason"].ToString().Trim();
                app_info_body.Level = Convert.ToInt32(drow["level"].ToString().Trim());
                app_info_body.Id = Helper.GetNewGuid("SP_Check_App_Info_Body_Sub_ID", "@Id");

                if (da_application_fp6.InsertAppInfoBodySub(app_info_body))
                {
                    status = true;
                }
                else
                {

                    status = false;
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppInfoBody] in class [Pages_Business_application_form_fp6.aspx.cs]. Details: " + ex.Message);
           
            status = false;
        }
        return status;
    }

   

    //Save Rider
    private bool InsertAppRider(string app_id)
    {
        bool result = false;
        try
        {

            DataTable tblRider = (DataTable)ViewState["tblRider"];
            string productID = "";
            productID = My_View_State.Product_ID ;

            if (tblRider.Rows.Count > 0)
            { //Save
                da_application_fp6.bl_app_rider rider;
                foreach (DataRow row in tblRider.Rows)
                {//Loop in table rider
                    rider = new da_application_fp6.bl_app_rider();

                    rider.App_Register_ID = app_id;
                    rider.Rider_ID = Helper.GetNewGuid("SP_Check_App_Rider_ID", "@Rider_ID");
                    rider.Product_ID = productID;
                    rider.Rider_Type = row["rider_type"].ToString().Trim();
                    rider.SumInsured = Convert.ToDouble(row["sumInsured"].ToString().Trim());
                    rider.Premium = Convert.ToDouble(row["premium"].ToString().Trim());
                    rider.Discount = Convert.ToDouble(row["discount"].ToString().Trim());
                    rider.Level = Convert.ToInt32(row["Level"].ToString().Trim());

                    if (da_application_fp6.InsertAppRider(rider))
                    {
                        result = true;
                    }
                    else
                    {
                        //Rollback
                       
                        result = false;
                    }
                }

            }
            else
            {//alert message
                //lblMessageApplication.Text = "";
            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppRider] in class [Pages_Business_application_form_fp6]. Details: " + ex.Message);
           
            result = false;

        }
        return result;
    }

    //save answer
    private bool InsertAppAnswerItem(string app_id)
    {
        bool status = true;
        try
        {
            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int rowCount = tblPersonal.Rows.Count;
            int level = -1;

            for (int i = 0; i < rowCount; i++)//loop in personal list
            {
                    var rowIndex = tblPersonal.Rows[i];
                    level = Convert.ToInt32(rowIndex["level"].ToString().Trim());
                
                     #region save spouse
                    if (level == 2)
                    {
                        foreach (GridViewRow row in GvQA.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {

                                RadioButtonList rbtnlAnswer1 = (RadioButtonList)row.FindControl("rbtnlAnswerLife1");
                                Label hdfSeqNumber1 = (Label)row.FindControl("lblSeqNumberLife1");
                                Label hdfQuestionID1 = (Label)row.FindControl("lblQuestionIDLife1");
                                string new_guid = "";

                                da_application_fp6.bl_app_answer_item_fp6 app_answer_item = new da_application_fp6.bl_app_answer_item_fp6();
                                app_answer_item.App_Register_ID = app_id;

                                    if (rbtnlAnswer1.SelectedIndex > 0)
                                    {
                                        new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                        app_answer_item.App_Answer_Item_ID = new_guid;
                                        app_answer_item.App_Register_ID = app_id;
                                        app_answer_item.Question_ID = hdfQuestionID1.Text.Trim();
                                        app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber1.Text.Trim());
                                        app_answer_item.Answer = Convert.ToInt32(rbtnlAnswer1.SelectedValue);
                                        app_answer_item.Level = level;
                                        //Insert into Ct_App_Answer_Item

                                        if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                        {
                                            status = true;
                                        }
                                        else
                                        {

                                            status = false;
                                            break;
                                        }
                                    }

                                }

                            }
                        }
                    #endregion

                      #region save kid 1
            
                    else if(level==3)
                    {
                        foreach (GridViewRow kid1 in gvkid1.Rows)
                        {
                            if (kid1.RowType == DataControlRowType.DataRow)
                            {
                                RadioButtonList rbtnlAnswer2 = (RadioButtonList)kid1.FindControl("rbtnlAnswerLife2");
                                Label hdfSeqNumber2 = (Label)kid1.FindControl("lblSeqNumberLife2");
                                Label hdfQuestionID2 = (Label)kid1.FindControl("lblQuestionIDLife2");
                                da_application_fp6.bl_app_answer_item_fp6 app_answer_item = new da_application_fp6.bl_app_answer_item_fp6();
                                app_answer_item.App_Register_ID = app_id;
                                string new_guid = "";
                                if (rbtnlAnswer2.SelectedIndex > 0)
                                {
                                    new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                    app_answer_item.App_Answer_Item_ID = new_guid;
                                    app_answer_item.App_Register_ID = app_id;
                                    app_answer_item.Question_ID = hdfQuestionID2.Text.Trim();
                                    app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber2.Text.Trim());
                                    app_answer_item.Answer = Convert.ToInt32(rbtnlAnswer2.SelectedValue);
                                    app_answer_item.Level = level;
                                    //Insert into Ct_App_Answer_Item

                                    if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                    {
                                        status = true;
                                    }
                                    else
                                    {

                                        status = false;
                                        break;
                                    }
                                }
                                
                            }
                    }
                    }
                    #endregion
                    #region save kid 2
                    else if(level==4)
                    {
                        foreach (GridViewRow kid2 in gvkid2.Rows)
                        {
                            if (kid2.RowType == DataControlRowType.DataRow)
                            {
                                RadioButtonList rbtnlAnswer3 = (RadioButtonList)kid2.FindControl("rbtnlAnswerLife3");
                                Label hdfSeqNumber3 = (Label)kid2.FindControl("lblSeqNumberLife3");
                                Label hdfQuestionID3 = (Label)kid2.FindControl("lblQuestionIDLife3");
                                da_application_fp6.bl_app_answer_item_fp6 app_answer_item = new da_application_fp6.bl_app_answer_item_fp6();
                                app_answer_item.App_Register_ID = app_id;
                                string new_guid = "";
                                if (rbtnlAnswer3.SelectedIndex > 0)
                                {
                                    new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                    app_answer_item.App_Answer_Item_ID = new_guid;
                                    app_answer_item.App_Register_ID = app_id;
                                    app_answer_item.Question_ID = hdfQuestionID3.Text.Trim();
                                    app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber3.Text.Trim());
                                    app_answer_item.Answer = Convert.ToInt32(rbtnlAnswer3.SelectedValue);
                                    app_answer_item.Level = level;
                                    //Insert into Ct_App_Answer_Item

                                    if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                    {
                                        status = true;
                                    }
                                    else
                                    {

                                        status = false;
                                        break;
                                    }
                                }
                                
                        
                            }
                        }
                    }
                    #endregion

                    #region save kid 3
                    else if(level==5)
                    {
                        foreach (GridViewRow kid3 in gvkid3.Rows)
                        {
                            if (kid3.RowType == DataControlRowType.DataRow)
                            {
                                RadioButtonList rbtnlAnswer4 = (RadioButtonList)kid3.FindControl("rbtnlAnswerLife4");
                                Label hdfSeqNumber4 = (Label)kid3.FindControl("lblSeqNumberLife4");
                                Label hdfQuestionID4 = (Label)kid3.FindControl("lblQuestionIDLife4");
                                da_application_fp6.bl_app_answer_item_fp6 app_answer_item = new da_application_fp6.bl_app_answer_item_fp6();
                                app_answer_item.App_Register_ID = app_id;
                                string new_guid = "";
                                if (rbtnlAnswer4.SelectedIndex > 0)
                                {
                                    new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                    app_answer_item.App_Answer_Item_ID = new_guid;
                                    app_answer_item.App_Register_ID = app_id;
                                    app_answer_item.Question_ID = hdfQuestionID4.Text.Trim();
                                    app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber4.Text.Trim());
                                    app_answer_item.Answer = Convert.ToInt32(rbtnlAnswer4.SelectedValue);
                                    app_answer_item.Level = level;
                                    //Insert into Ct_App_Answer_Item

                                    if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                    {
                                        status = true;
                                    }
                                    else
                                    {

                                        status = false;
                                        break;
                                    }
                                }
                                
                            }
                        }
                    }
                    #endregion

                    #region save kid 4
                    else if(level==6)
                    {
                        foreach (GridViewRow kid4 in gvkid4.Rows)
                        {
                            if (kid4.RowType == DataControlRowType.DataRow)
                            {
                                RadioButtonList rbtnlAnswer5 = (RadioButtonList)kid4.FindControl("rbtnlAnswerLife5");
                                Label hdfSeqNumber5 = (Label)kid4.FindControl("lblSeqNumberLife5");
                                Label hdfQuestionID5 = (Label)kid4.FindControl("lblQuestionIDLife5");
                                da_application_fp6.bl_app_answer_item_fp6 app_answer_item = new da_application_fp6.bl_app_answer_item_fp6();
                                app_answer_item.App_Register_ID = app_id;
                                string new_guid = "";
                                if (rbtnlAnswer5.SelectedIndex > 0)
                                {
                                    new_guid = Helper.GetNewGuid("SP_Check_App_Answer_Item_ID", "@App_Answer_Item_ID").ToString();
                                    app_answer_item.App_Answer_Item_ID = new_guid;
                                    app_answer_item.App_Register_ID = app_id;
                                    app_answer_item.Question_ID = hdfQuestionID5.Text.Trim();
                                    app_answer_item.Seq_Number = Convert.ToInt32(hdfSeqNumber5.Text.Trim());
                                    app_answer_item.Answer = Convert.ToInt32(rbtnlAnswer5.SelectedValue);
                                    app_answer_item.Level = level;
                                    //Insert into Ct_App_Answer_Item

                                    if (da_application_fp6.InsertAppAnswerItemFp6(app_answer_item))
                                    {
                                        status = true;
                                    }
                                    else
                                    {

                                        status = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

            }

            //if reach here return true
        
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [InsertAppAnswerItem] in class [Pages_Business_application_form_fp6]. Details: " + ex.Message);
            status = false;
           
        }
        return status;
    }
 
   
    //load person
    private void LoadPerson(string appID)
    {
        try
        {
            DataTable tbl = new DataTable();
            tbl = da_application_fp6.GetDataTable("SP_Get_App_Info_Person_Sub_FP6_By_App_Register_ID", appID);

            ViewState["tblPersonal"] = tbl;
            gvPersonalInfo.DataSource = tbl;
            gvPersonalInfo.DataBind();

            //bind data into drop downlist
            ddlJobPolicyOwner.Items.Clear();
            ddlJobPolicyOwner.Items.Add(new ListItem(".", ""));

            ddlBodyPerson.Items.Clear();
            ddlBodyPerson.Items.Add(new ListItem(".", ""));

            //if riders was already underwriten user cannot do anything
            foreach (GridViewRow row in gvPersonalInfo.Rows)
            {
                int level = 0;
                level = Convert.ToInt32(((Label)row.FindControl("lblLevel")).Text.Trim());
                if (da_application_fpp.GetUWRiderStatus(My_View_State.App_Register_ID, level) != "")
                {
                    row.ForeColor = System.Drawing.Color.Red;
                    row.Enabled = false;
                }
            }

            BindPersonInDrowDownList();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [LoadPerson] in page [application_form_fp6_rider.aspx.cs], Detail: "+ ex.Message);
        }
       
    
    }

    //bind person list into dropdownlist
    void BindPersonInDrowDownList()
    {
        DataTable tblPerson = (DataTable)ViewState["tblPersonal"];
        ddlJobPolicyOwner.Items.Clear();
        ddlBodyPerson.Items.Clear();
        ddlJobPolicyOwner.Items.Add(new ListItem(".", ""));
        ddlBodyPerson.Items.Add(new ListItem(".", ""));
        foreach (DataRow row in tblPerson.Rows)
        {
            ddlJobPolicyOwner.Items.Add(new ListItem(row["level"].ToString() + " - " + row["surEnName"].ToString() + " " + row["firstEnName"].ToString(), row["level"].ToString()));
            ddlBodyPerson.Items.Add(new ListItem(row["level"].ToString() + " - " + row["surEnName"].ToString() + " " + row["firstEnName"].ToString(), row["level"].ToString()));
      
        }
    }

  //load premium detail
    private void LoadPremiumDetail(string appID)
    {
        try
        {
            DataTable tblPremiumDetail = new DataTable();
            tblPremiumDetail = da_application_fp6.GetDataTable("SP_Get_App_Premium_Detail_By_App_Register_ID", appID);
            ViewState["tblPremiumDetail"] = tblPremiumDetail;
            gvPremiumDetail.DataSource = tblPremiumDetail;
            gvPremiumDetail.DataBind();

            //total premium 
            double totalPremium = 0.0;
            foreach (DataRow row in tblPremiumDetail.Rows)
            {
                totalPremium = totalPremium + Convert.ToDouble(row["premium"].ToString());
            }
            //initialize to textbox
            txtTotalPremium.Text = totalPremium + "";
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [LoadPremiumDetail] in page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
        }
        
    }

    //Load Rider
    private void LoadRider(string appID)
    {
        try
        {
            List<da_application_fp6.bl_app_rider> myRider = new List<da_application_fp6.bl_app_rider>();
            myRider = da_application_fp6.GetAppRiderList(appID);
            if (myRider.Count > 0)
            {
                DataTable tblRider = (DataTable)ViewState["tblRider"];
                tblRider.Clear();

                double discountAmount = 0.0;
                double riderPremium = 0.0;
                double amountAfterDiscount = 0.0;


                for (int i = 0; i < myRider.Count; i++)
                {
                    var rider = myRider[i];
                    DataRow row = tblRider.NewRow();
                    row["rider_id"] = rider.Rider_ID;
                    row["app_register_id"] = rider.App_Register_ID;
                    row["level"] = rider.Level;
                    row["rider_type"] = rider.Rider_Type;
                    row["sumInsured"] = rider.SumInsured;
                    row["premium"] = rider.Premium;
                    row["product_id"] = rider.Product_ID;
                    row["discount"] = rider.Discount;

                    discountAmount = rider.Discount;
                    riderPremium = riderPremium + rider.Premium;

                    tblRider.Rows.Add(row);
                }

                amountAfterDiscount = riderPremium - discountAmount;


                gvPremiumDetail.DataSource = tblRider;

                gvPremiumDetail.DataBind();

                ViewState["tblRider"] = tblRider;
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [LoadRider] in page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
        }

        
    }
    //load job history
    private void LoadJobHistory(string appID)
    {
        try
        {
            DataTable tblJobHistory = new DataTable();
            tblJobHistory = da_application_fp6.GetApplicationJobHistorySub(appID);

            ViewState["tblJobHistory"] = tblJobHistory;
            gvJobHistory.DataSource = tblJobHistory;
            gvJobHistory.DataBind();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [LoadJobHistory] in page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
        }


        

    }
   

    //Load Body
    private void LoadBody(string appID)
    {
        try
        {
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            tblBody.Clear();

            //Life insured 
            List<da_application_fp6.bl_app_info_body_sub> myList1 = new List<da_application_fp6.bl_app_info_body_sub>();
            myList1 = da_application_fp6.GetAppInfoBodySub(appID);
            //check has record or not
            if (myList1.Count > 0)
            {
                for (int j = 0; j < myList1.Count; j++)
                {
                    var i = myList1[j];
                    if (i.Level > 0)
                    {//except level 0 because it's policy owner
                        DataRow row;
                        row = tblBody.NewRow();
                        row["level"] = i.Level;
                        row["app_register_id"] = i.App_Register_ID;
                        row["weight"] = i.Weight;
                        row["height"] = i.Height;
                        row["is_weight_changed"] = i.Is_Weight_Changed;
                        row["reason"] = i.Reason;
                        row["id"] = i.Id;

                        tblBody.Rows.Add(row);
                    }

                }
            }
            gvBody.DataSource = tblBody;
            gvBody.DataBind();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [LoadBody] in page [application_form_fp6_rider_aspx.cs], Detail: " + ex.Message);
        }
       

    }

    //Load Answers
    void LoadAnswers(string appID)
    {
        try
        {
            GvQA.DataBind();
            gvkid1.DataBind();
            gvkid2.DataBind();
            gvkid3.DataBind();
            gvkid4.DataBind();
            #region load spouse
            List<da_application_fp6.bl_app_answer_item_fp6> myList = new List<da_application_fp6.bl_app_answer_item_fp6>();
            myList = da_application_fp6.GetAppAnswerItem(appID);

            if (myList.Count > 0)
            {

                //loop questions in gridview
                int rowCount = GvQA.Rows.Count;

                //foreach (GridViewRow gRow in GvQA.Rows)
                //{
                //    Label lblQuestionID = (Label)gRow.FindControl("lblQuestionIDLife1");

                //    string questionID = lblQuestionID.Text.Trim();

                   
                    //RadioButtonList rbtnAnswer2 = (RadioButtonList)gRow.FindControl("rbtnlAnswerLife2");
                    //RadioButtonList rbtnAnswer3 = (RadioButtonList)gRow.FindControl("rbtnlAnswerLife3");
                    //RadioButtonList rbtnAnswer4 = (RadioButtonList)gRow.FindControl("rbtnlAnswerLife4");
                    //RadioButtonList rbtnAnswer5 = (RadioButtonList)gRow.FindControl("rbtnlAnswerLife5");

                    //loop answers from database
                    for (int i = 0; i < myList.Count; i++)
                    {
                        var answer = myList[i];

                        int answerID = Convert.ToInt32(answer.Answer);//from database

                        #region load spouse
                        if (answer.Level == 2)//spouse
                        {
                            foreach (GridViewRow spouse_row in GvQA.Rows)
                            {
                                RadioButtonList rbtnAnswer1 = (RadioButtonList)spouse_row.FindControl("rbtnlAnswerLife1");
                                Label lblQuestionID_spouse = (Label)spouse_row.FindControl("lblQuestionIDLife1");
                               // string questionID_spouse = lblQuestionID_spouse.Text.Trim();
                                if (lblQuestionID_spouse.Text.Trim() == answer.Question_ID.Trim())
                                {
                                    //rbtnAnswer1.SelectedIndex = answerID - 1;
                                    Helper.SelectedRadioListIndex(rbtnAnswer1, answerID + "");
                                }
                            }
                            
                        }
                        #endregion

                        #region kid 1
                        else if (answer.Level == 3)//kid 1
                        {
                            foreach (GridViewRow kid1_row in gvkid1.Rows)
                            {
                                RadioButtonList rbtnAnswer2 = (RadioButtonList)kid1_row.FindControl("rbtnlAnswerLife2");
                                Label lblQuestionID_kid1 = (Label)kid1_row.FindControl("lblQuestionIDLife2");
                                if (lblQuestionID_kid1.Text.Trim() == answer.Question_ID.Trim())
                                {
                                    Helper.SelectedRadioListIndex(rbtnAnswer2, answerID + "");
                                }
                            }
                            
                        }

                        #endregion

                        #region kid 2
                        else if (answer.Level == 4)//kid 2
                        {
                            foreach (GridViewRow kid2_row in gvkid2.Rows)
                            {
                                RadioButtonList rbtnAnswer3 = (RadioButtonList)kid2_row.FindControl("rbtnlAnswerLife3");
                                Label lblQuestionID_kid2 = (Label)kid2_row.FindControl("lblQuestionIDLife3");
                                if (lblQuestionID_kid2.Text.Trim() == answer.Question_ID.Trim())
                                {
                                    Helper.SelectedRadioListIndex(rbtnAnswer3, answerID + "");
                                }
                            }
                           
                        }
                        #endregion

                        #region kid 3
                        else if (answer.Level == 5)//kid 3
                        {
                            foreach (GridViewRow kid3_row in gvkid3.Rows)
                            {
                                RadioButtonList rbtnAnswer4 = (RadioButtonList)kid3_row.FindControl("rbtnlAnswerLife4");
                                Label lblQuestionID_kid3 = (Label)kid3_row.FindControl("lblQuestionIDLife4");
                                if (lblQuestionID_kid3.Text.Trim() == answer.Question_ID.Trim())
                                {
                                    Helper.SelectedRadioListIndex(rbtnAnswer4, answerID + "");
                                }
                            }
                            
                        }
                        #endregion

                        #region kid 4
                        else if (answer.Level == 6)//kid 4
                        {
                            foreach (GridViewRow kid4_row in gvkid4.Rows)
                            {
                                RadioButtonList rbtnAnswer5 = (RadioButtonList)kid4_row.FindControl("rbtnlAnswerLife5");
                                Label lblQuestionID_kid4 = (Label)kid4_row.FindControl("lblQuestionIDLife5");
                                if (lblQuestionID_kid4.Text.Trim() == answer.Question_ID.Trim())
                                {
                                    Helper.SelectedRadioListIndex(rbtnAnswer5, answerID + "");
                                }
                            }
                        }
                        #endregion
                       
                    }
               // }
            #endregion
               

            }
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Load answer Error:" + ex.Message);
        }

    }

    //call all load data
    private void LoadData(string appID)
    {
        try
        {
          
            //load person
            LoadPerson(appID);

            //load Job history
            LoadJobHistory(appID);
          
            //Load Rider
            LoadRider(appID);

            //Load Body
            LoadBody(appID);

            //Load Answers
            LoadAnswers(appID);

            //enable button update
            EnableButtonsUpdate(true);
        }
        catch (Exception ex)
        {
            //disable button update
            // EnableButtonsUpdate(false);
            lblMessageApplication.Text = "System load data error, please contact your system administrator.";
        }


    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        string appID = "";
        
        appID = Request.Params["application_register_id"];

        //if (Session["SS_POLICY_ID"] == null)
        //{
        //    lblMessageApplication.Text = "Page expired.";
        //    Response.Redirect("application_form_fp6.aspx");
        //    return;
        //}

        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            if (appID !="" && appID !=null)
            {
                
                #region Check Exist application ID 
                List<da_application_fp6.AppPlanInfo> planList = new List<da_application_fp6.AppPlanInfo>();
                   
                planList= da_application_fp6.GetAppPlanInfo(appID);
                if (planList.Count > 0)
                {
                
                    //Store data in class My_View_State to use everywhere of this page
                    My_View_State.User_ID = user_id;
                    My_View_State.User_Name = user_name;
                    My_View_State.App_Register_ID = planList[0].ApplicationID;
                    My_View_State.Pay_Mode = planList[0].PayMode;
                    My_View_State.Sum_Insured = planList[0].SumInsure;
                    My_View_State.Assure_Plan = planList[0].AssureYear;
                    My_View_State.Product_ID = planList[0].ProductID;

                    My_View_State.Policy_ID = Session["SS_POLICY_ID"] + "";
                  
                    My_View_State.Policy =  da_policy.GetPolicyDetail(My_View_State.Policy_ID);


                    if (My_View_State.Policy.Policy_Number != "" && My_View_State.Policy.Policy_Number != null)
                    {
                        My_View_State.prem_lot = da_policy_prem_lot.Get_Policy_Prem_Lot(My_View_State.Policy_ID);

                        txtPolicyNumber.Text = My_View_State.Policy.Policy_Number;

                        txtDateOfEntry.Text = String.Format("{0:dd/MM/yyyy}", System.DateTime.Today);
                        txtDateOfEntry.Enabled = false;
                        txtEffectiveDate.Text = String.Format("{0:dd/MM/yyyy}", My_View_State.prem_lot.Due_Date);
                        txtEffectiveDate.Enabled = true;

                        imgDelete.Enabled = true;
                        imgSave.Enabled = true;
                        lblMessageApplication.Text = "";

                    }
                    else
                    {
                        //My_View_State.prem_lot.Prem_Lot = 0;
                        txtDateOfEntry.Text = String.Format("{0:dd/MM/yyyy}", System.DateTime.Today);
                        txtDateOfEntry.Enabled = true;
                        txtEffectiveDate.Enabled = false;

                        if (CheckUnderWrite(appID))
                        {
                            imgDelete.Enabled = false;
                            imgSave.Enabled = false;
                            lblMessageApplication.Text = "Application was underwritten already, system is not allow to do any update.";
                            //return;
                            //disable buttons update
                            EnableButtonsUpdate(false);
                        }
                        else
                        {
                            imgDelete.Enabled = true;
                            imgSave.Enabled = true;
                            lblMessageApplication.Text = "";

                            //disable buttons update
                            EnableButtonsUpdate(true);
                        }
                    }
                    
                    
                    generatePersonalTable();

                    //generate benifit table
                    generateBenifitTable();

                    //Generate job history table
                    generateJobHistoryTable();

                    //Generate body table
                    generateBodyTable();

                    //Generate Rider table
                    geneateRiderTable();

                    //Generate Premium Detail table
                    GeneratePremiumDetailTable();


                    //alert to confirm message
                    imgSave.Attributes.Add("onclick", "return confirm('Do you want to save this application?');");
                    imgDelete.Attributes.Add("onclick", "return confirm('Do you want to delete this rider application?');");

                    btnSaveJob.Attributes.Add("onclick", "return confirm('Confirm update!');");

                    //disable buttons update
                   // EnableButtonsUpdate(true);

                    //Load Person
                    LoadPerson(appID);

                    //Load premium detail
                    LoadPremiumDetail(appID);


                    //Load job history
                    LoadJobHistory(appID);

                    //Load body
                    LoadBody(appID);

                    //Load answer
                    LoadAnswers(appID);

                   

                }
                else
                { //if not exist in ct_app_register show message to user
                    lblMessageApplication.Text = "Application number is not exist in the system.";
                }
                #endregion End Check Application id

            }
            else
            {
                lblMessageApplication.Text = "System not allow to do anything.";
                DisableControls(this.Page, false);
                btnGoBack.Enabled = true;
                ibtnBack.Enabled = true;
            }
            
        }
    }


    #region Personal information
    //sub add personal information in to data table
    private void generatePersonalTable()
    {
        //variable
        DataTable tblPersonal = new DataTable();
        var a = tblPersonal.Columns;
      
        a.Add("level");
        a.Add("idType");
        a.Add("idNumber");
        a.Add("surKhName");
        a.Add("firstKhName");
        a.Add("surEnName");
        a.Add("firstEnName");
        a.Add("nationality");
        a.Add("gender");
        a.Add("dob");
        a.Add("id");
        a.Add("Marital_Status");
        a.Add("Relationship");
        ViewState["tblPersonal"] = tblPersonal;
    }
    //clear control in personal information
    private void ClearPerson()
    {

        ddlRiderType.SelectedIndex = 0;
        ddlIDType.SelectedIndex = 0;
        txtIDNumber.Text = "";
        txtSurnameEng.Text = "";
        txtFirstNameEng.Text = "";
        txtSurnameKh.Text = "";
        txtFirstNameKh.Text = "";
       
        ddlNationality.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        txtDateBirth.Text = "";

        txtEffectiveDate.Text = "";

        lblMessagePersonalInfo.Text = "";

        ddlRelationShip.SelectedIndex = 0;

        txtRiderSumInsured.Text = "";

        btnPersonalAdd.Text = "Add";
        ViewState["tblPersonalRowIndex"] = null;
    }

    protected void btnClearPersonInfo_Click(object sender, EventArgs e)
    {
        ClearPerson();
    }

    protected void btnPersonalAdd_Click(object sender, EventArgs e)
    {
        DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
        DataRow row;

        DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
        
        #region //varlidat required fields

        if (txtIDNumber.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "I.D NO is required.";
            return;
        }
        else if (txtSurnameEng.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "Surname is required.";
            return;
        }
        else if (txtFirstNameEng.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "First name is required.";
            return;
        }
        else if (ddlNationality.SelectedIndex == 0)
        {

            lblMessagePersonalInfo.Text = "Nationality is required.";
            return;
        }
        else if (txtDateBirth.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "Date of Birth is required.";
            return;

        }
        else if (txtDateOfEntry.Text.Trim() == "")
        {
            lblMessagePersonalInfo.Text = "Date of Entry is required.";
            return;
        }
        #endregion

        string effectiveDate = "";
        string gender = "";
        int level = 0;

        if (ddlGender.SelectedValue.Trim() == "0")
        {
            gender = "Female";
        }
        else
        {
            gender = "Male";
        }

        if (ddlRiderType.SelectedValue.Trim() != "")
        {
            level = Convert.ToInt32(ddlRiderType.SelectedValue.Trim());
        }

        if (txtEffectiveDate.Text.Trim() != "")
        {
            effectiveDate = txtEffectiveDate.Text.Trim();
        }
        else
        {
            effectiveDate = txtDateOfEntry.Text.Trim();
        }

        
        int count = tblPersonal.Rows.Count;
        
        if (count > 0)
        {
            int limit = 0;
            //loop to count riders maximum riders is 5 , spouse 1 kids 4
            for (int i = 0; i < count; i++)
            {
                //Label lblOwner = (Label)gvPersonalInfo.Rows[i].FindControl("lblOwnerType");
                var tr = tblPersonal.Rows[i];

                limit += 1;

            }

            if (limit > 5)
            {
                //not allow input policy owner more than one
                lblMessagePersonalInfo.Text = "Max rider is 5.";
                return;
            }
        }
            string sub_product = "";
            string dob = "";
            dob = txtDateBirth.Text.Trim();

            //check effective date user input with system effecitve that
            //if not the same effective date will automatic use the next year policy due date
            if (My_View_State.Policy_ID != null & My_View_State.Policy_ID != "")
            {
                if (My_View_State.prem_lot.Prem_Year > 0)
                {
                    if (Helper.FormatDateTime(effectiveDate) != My_View_State.prem_lot.Due_Date)
                    {
                        effectiveDate = string.Format("{0:dd/MM/yyyy}", da_policy_prem_pay.Get_Next_Due_by(My_View_State.prem_lot.Pay_Mod, My_View_State.prem_lot.Due_Date) + "");
                    }
                }
            }
            

            int age = Calculation.Culculate_Customer_Age(dob, Helper.FormatDateTime(string.Format("{0:dd/MM/yyyy}",effectiveDate)));
            //int new_age = Helper.GetAge(Helper.FormatDateTime(dob), Helper.FormatDateTime(string.Format("{0:dd/MM/yyyy}", effectiveDate)));

            sub_product = My_View_State.Product_ID.Substring(0, 3).ToUpper().Trim();

            if (ddlRiderType.SelectedValue.Trim() == "2")
            { //spouse
                //Check age

                if (sub_product == "NFP")
                {
                    if (!CheckVarlidAge(age, 1))
                    {
                        lblMessagePersonalInfo.Text = "Age is over. Age must be in rage [18 - 55].";
                        return;
                    }
                }
                else if (sub_product == "FPP")//family protection package
                {
                    string age_varlid = "";
                    age_varlid = da_application_fpp.Varlid_Spouse_Age_Range(age);
                    if (age_varlid != "")
                    {
                        lblMessagePersonalInfo.Text = age_varlid;
                        return;
                    }
                }

            }
            else if (ddlRiderType.SelectedValue.Trim() == "3" || ddlRiderType.SelectedValue.Trim() == "4" || ddlRiderType.SelectedValue.Trim() == "5" || ddlRiderType.SelectedValue.Trim() == "6")
            {//kids
                //Check age


                if (sub_product == "NFP")
                {
                    if (!CheckVarlidAge(age, 2))
                    {
                        lblMessagePersonalInfo.Text = "Age is over. Age must be in rage [1 - 17].";
                        return;
                    }
                }
                else if (sub_product == "FPP")
                {
                    string age_varlid = "";
                    age_varlid = da_application_fpp.Varlid_Kid_Age_Range(age);
                    if (age_varlid != "")
                    {
                        lblMessagePersonalInfo.Text = age_varlid;
                        return;
                    }
                }
            }

            //get sum insured
            double new_sum_insured = 0.0;
            double recal_sum_insured = 0.0;

            if (My_View_State.Sum_Insured > 0 && My_View_State.Sum_Insured != null)
            {
                recal_sum_insured = My_View_State.Sum_Insured;
            }

            if (ddlRiderType.SelectedValue.Trim() == "2")
            {
               

                #region Enable user input riders suminsured
                //new_sum_insured = da_application_fp6.GetSpouseSumInsured(recal_sum_insured);
                //txtRiderSumInsured.Text = new_sum_insured + "";
                new_sum_insured = Convert.ToDouble(txtRiderSumInsured.Text.Trim());
                #endregion

                //check varlid range sum insured
                string str = "";
                str = da_application_fp6.VarlidSpouseSumInsured(new_sum_insured);
                if (str!="")
                {
                    lblMessagePersonalInfo.Text = str;
                    return;
                }
            }
            else if (ddlRiderType.SelectedValue.Trim() == "3" || ddlRiderType.SelectedValue.Trim() == "4" || ddlRiderType.SelectedValue.Trim() == "5" || ddlRiderType.SelectedValue.Trim() == "6")
            {
                string str = "";
              

                #region Enable user input riders suminsured
                //new_sum_insured = da_application_fp6.GetKidSumInsured(recal_sum_insured);
                //txtRiderSumInsured.Text = new_sum_insured + "";
                new_sum_insured = Convert.ToDouble(txtRiderSumInsured.Text.Trim());
                #endregion

                str = da_application_fp6.VarlidKidSumInsured(new_sum_insured);
                if (str != "")
                {
                    lblMessagePersonalInfo.Text = str;
                    return;
                }
            
            }
        
        #region Save data in data table

        //get sum insured user input
        double suminsured = 0.0;
        if (txtRiderSumInsured.Text.Trim() != "")
        {
            suminsured = Convert.ToDouble(txtRiderSumInsured.Text.Trim());
        }

        if (ViewState["tblPersonalRowIndex"] == null)
        { //save

            try
            {
                //check exist data in datatable personal
                if (tblPersonal.Rows.Count > 0)
                {
                    foreach (DataRow trow in tblPersonal.Rows)
                    {
                        if (ddlRiderType.SelectedValue.Trim() == trow["level"].ToString().Trim())
                        {
                            lblMessagePersonalInfo.Text = "Record is already exist.";
                            return;
                        }
                    }
                }
                //initialize value to data row
                row = tblPersonal.NewRow();
                
                row["level"] = level;
                row["idType"] = ddlIDType.SelectedValue.Trim();
                row["idNumber"] = txtIDNumber.Text.Trim();
                row["surKhName"] = txtSurnameKh.Text.Trim();
                row["firstKhName"] = txtFirstNameKh.Text.Trim();
                row["surEnName"] = txtSurnameEng.Text.Trim().ToUpper();
                row["firstEnName"] = txtFirstNameEng.Text.Trim().ToUpper() ;
                row["nationality"] = ddlNationality.SelectedValue.Trim();
                row["gender"] = gender;
                row["dob"] = txtDateBirth.Text.Trim();
                row["id"] = "";
                row["Marital_Status"] = ddlMaritalStatus.SelectedValue;
                row["Relationship"] = ddlRelationShip.SelectedValue;

                //add row into datatable
                tblPersonal.Rows.Add(row);

                gvPersonalInfo.DataSource = tblPersonal;
                gvPersonalInfo.DataBind();

                //if riders was already underwriten user cannot do anything
                foreach (GridViewRow row22 in gvPersonalInfo.Rows)
                {
                    int level22 = 0;
                    level22 = Convert.ToInt32(((Label)row22.FindControl("lblLevel")).Text.Trim());
                    if (da_application_fpp.GetUWRiderStatus(My_View_State.App_Register_ID, level22) != "")
                    {
                        row22.ForeColor = System.Drawing.Color.Red;
                        row22.Enabled = false;
                    }
                }


                //add row into premium detail datatable 
               
                //add row into table tblPremiumDetail
                if (AddPremiumDetailRow(level, txtSurnameEng.Text.Trim() + " " + txtFirstNameEng.Text.Trim(), gender, txtDateBirth.Text.Trim(), effectiveDate, suminsured))
                {
                    lblMessagePersonalInfo.Text = "AddPremiumDetailRow success";
                }

                //clear text control
                ClearPerson();

                BindPersonInDrowDownList();

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add Personal information. Detail: " + ex.Message);

                lblMessagePersonalInfo.Text = "Added fail, please contact your system administrator.";

            }

        }
        #endregion End Save data in data table

        #region Update data in data table
        else
        { //update data in datatable

            try
            {
                int rowIndex = Convert.ToInt16(ViewState["tblPersonalRowIndex"]);
                int oldLevel=0;
               
                oldLevel=Convert.ToInt32(tblPersonal.Rows[rowIndex]["level"] .ToString());

                //initialize value to data row
                tblPersonal.Rows[rowIndex]["level"] = level; 
                tblPersonal.Rows[rowIndex]["idType"] = ddlIDType.SelectedValue.Trim();
                tblPersonal.Rows[rowIndex]["idNumber"] = txtIDNumber.Text.Trim();
                tblPersonal.Rows[rowIndex]["surKhName"] = txtSurnameKh.Text.Trim();
                tblPersonal.Rows[rowIndex]["firstKhName"] = txtFirstNameKh.Text.Trim();
                tblPersonal.Rows[rowIndex]["surEnName"] = txtSurnameEng.Text.Trim(); ;
                tblPersonal.Rows[rowIndex]["firstEnName"] = txtFirstNameEng.Text.Trim();
                tblPersonal.Rows[rowIndex]["nationality"] = ddlNationality.SelectedValue.Trim();
                tblPersonal.Rows[rowIndex]["gender"] = gender;
                tblPersonal.Rows[rowIndex]["dob"] = txtDateBirth.Text.Trim();
                //tblPersonal.Rows[rowIndex]["id"] = 0;
                tblPersonal.Rows[rowIndex]["Marital_Status"] = ddlMaritalStatus.SelectedValue;
                tblPersonal.Rows[rowIndex]["Relationship"] = ddlRelationShip.SelectedValue;

                tblPersonal.AcceptChanges();

                gvPersonalInfo.DataSource = tblPersonal;
                gvPersonalInfo.DataBind();

                //if riders was already underwriten user cannot do anything
                foreach (GridViewRow row1 in gvPersonalInfo.Rows)
                {
                    int level1 = 0;
                    level1 = Convert.ToInt32(((Label)row1.FindControl("lblLevel")).Text.Trim());
                    if (da_application_fpp.GetUWRiderStatus(My_View_State.App_Register_ID, level) != "")
                    {
                        row1.ForeColor = System.Drawing.Color.Red;
                        row1.Enabled = false;
                    }
                }


                //update row into premium detail datatable 
                if(UpdatePremiumDetailRow(oldLevel,level,txtSurnameEng.Text.Trim() + " " + txtFirstNameEng.Text.Trim(),gender,txtDateBirth.Text.Trim(), effectiveDate, suminsured))
                {

                }

                if (gvPersonalInfo.Rows.Count > 0)
                {
                    //call calculation premium
                    //CalculatePremium();
                }

                //clear text control
                ClearPerson();

                BindPersonInDrowDownList();

            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add Personal information. Detail: " + ex.Message);

                lblMessagePersonalInfo.Text = "Update fail, please contact your system administrator.";

            }

        }
        #endregion Update data in data table

    }

    protected void gvPersonalInfo_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];

        //check data in data table has or not?
        if (tblPersonal.Rows.Count > 0)
        {

            int rowIndex = e.NewSelectedIndex;
            ViewState["tblPersonalRowIndex"] = rowIndex;
            string gender = "";
            int level = 0;

            if (tblPersonal.Rows[rowIndex]["gender"].ToString().Trim() == "Male")
            {
                gender = "1";
            }
            else
            {
                gender = "0";
            }

            level =Convert.ToInt32( tblPersonal.Rows[rowIndex]["level"].ToString());
            //change add button to Update
            btnPersonalAdd.Text = "Update";

            //ddlRiderType.SelectedIndex = level;
            Helper.SelectedDropDownListIndex("VALUE", ddlRiderType, level+"");

            Helper.SelectedDropDownListIndex("VALUE", ddlIDType, tblPersonal.Rows[rowIndex]["idType"].ToString());
            txtIDNumber.Text = tblPersonal.Rows[rowIndex]["idNumber"].ToString();
            Helper.SelectedDropDownListIndex("VALUE", ddlNationality, tblPersonal.Rows[rowIndex]["nationality"].ToString());
            Helper.SelectedDropDownListIndex("VALUE", ddlGender, gender);

            txtDateBirth.Text = tblPersonal.Rows[rowIndex]["dob"].ToString();
            txtFirstNameKh.Text = tblPersonal.Rows[rowIndex]["firstKhName"].ToString();
            txtSurnameKh.Text = tblPersonal.Rows[rowIndex]["surKhName"].ToString();
            txtFirstNameEng.Text = tblPersonal.Rows[rowIndex]["firstEnName"].ToString();
            txtSurnameEng.Text = tblPersonal.Rows[rowIndex]["surEnName"].ToString();

            Helper.SelectedDropDownListIndex("VALUE", ddlRelationShip, tblPersonal.Rows[rowIndex]["Relationship"].ToString());

            Helper.SelectedDropDownListIndex("VALUE", ddlMaritalStatus, tblPersonal.Rows[rowIndex]["Marital_Status"].ToString());

            //get effective date from premium detail datatable by level
            DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
            foreach(DataRow row in tblPremiumDetail.Rows)
            {
                if(row["level"].ToString().Trim()==tblPersonal.Rows[rowIndex]["level"].ToString().Trim())
                {
                    //txtEffectiveDate.Text = row["effective_date"].ToString();
                    txtDateOfEntry.Text = row["effective_date"].ToString();
                    break;
                }
            }
            //get premium from premium detail datatable by level
            double sumInsure = 0.0;
            sumInsure = GetRiderSumInsured(level);
            txtRiderSumInsured.Text = sumInsure + "";

        }

    }
    #endregion
   
    #region Health
    //select changed: weight changed 6 months for life 1
    protected void rblWeightChangeLife1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblWeightChangeLife1.SelectedIndex >= 1)
        {
            lblWeightChangeReasonLife1.Visible = true;
            txtWeightChangeReasonLife1.Visible = true;
        }
        else
        { //hide control

            lblWeightChangeReasonLife1.Visible = false;
            txtWeightChangeReasonLife1.Visible = false;
        }
    }


    #endregion


    protected void imgSave_Click(object sender, ImageClickEventArgs e)
    {
        #region Old code
        /*
        string appID = "";

        bool status = false;
        try
        {
            appID = ViewState["application_register_id"] + "";
            if (appID == "" || appID == null)
            {
                lblMessageApplication.Text = "Application number is not exsit.";
                return;
            }

            if (!da_application.CheckAppRegisterID(appID))
            {
                lblMessageApplication.Text = "Application number is not exist.";
                return;
            }
            else
            { //save
                //save personal info sub
                if (insertPersonalInfoSub(appID))
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                //save job history

                if (InsertAppJobHistorySubList(appID))
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                //save Rider
                if (InsertAppRider(appID))
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                //save body
                if (InsertAppInfoBody(appID))
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                //save answer
                if (InsertAppAnswerItem(appID))
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            if (status == true)
            {
                lblMessageApplication.Text = "Saved successfully.";
            }
            else
            {
                lblMessageApplication.Text = "Saved fail, please contact your system administrator.";
            }
        }
        catch (Exception ex)
        {
            // lblMessageApplication.Text = "Saved fail, please contact your system administrator.";
            Log.AddExceptionToLog("Error click save button in page [application_form_fp6.aspx], Detail: " + ex.Message);
        }
        */
        #endregion

        #region New Code
         string appID = "";
        string status = "";
        appID =My_View_State.App_Register_ID;
        if (appID == "" || appID == null)
        {
            lblMessageApplication.Text = "Application number is not exsit.";
            return;
        }
        else
        {
            //check policy is issued or not
            if (My_View_State.prem_lot!=null)
            {
                if (My_View_State.prem_lot.Prem_Year > 0)
                { 
                
                }
                else
                {
                    //check underwriting

                    if (CheckUnderWrite(appID))
                    {
                        lblMessageApplication.Text = "Application was underwritten already, saved fail.";
                        return;
                    }
                }
            }
           

            //save personal data
            status = SavePersonal();
            if (status == "")
            {
                status = "";
            }
            else
            {
                lblMessageApplication.Text = status;
                return;
            }
            //save job history
            status = SaveJobHistory();
            if (status == "")
            {
                status = "";
            }
            else
            {
                lblMessageApplication.Text = status;
                return;
            }
            //save health
            status = SaveHealth();
            if (status == "")
            {
                status = "";
            }
            else
            {
                lblMessageApplication.Text = status;
                return;
            }

            //update application discount
            double premium = 0;
            if (txtTotalPremium.Text.Trim() != "")
            {
                premium = Convert.ToDouble(txtTotalPremium.Text.Trim());
            }
            status = UpdateApplicationDiscount(appID);
            if (status == "")
            {
                status = "";
            }
            else
            {
                lblMessageApplication.Text = status;
                return;
            }

            //final message 
            if (status == "")
            {
                lblMessageApplication.Text = "Application was saved successfully.";
            }
            else
            {
                lblMessageApplication.Text = "Application was saved fail. Please contact your system administrator.";
            }
        }
        #endregion

    }
  
   
    protected void gvPersonalInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable tbl = (DataTable)ViewState["tblPersonal"];
        int rowIndex = e.RowIndex;
        int level = 0;

        string id = "";
        id = tbl.Rows[rowIndex]["id"].ToString();
        level =Convert.ToInt32( tbl.Rows[rowIndex]["level"].ToString());

        if (id != "0" & id != "")
        {// store id for deleting from database
            ViewState["personID"] += id + ",";
            ViewState["oldLevel"] += level +",";
            ViewState["DELETE"] += level + ",";
        }
        //delete row from table tblPersonal
        tbl.Rows[rowIndex].Delete();
        tbl.AcceptChanges();

        //delete row from table tblPremiumDetail
        DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
        int count = tblPremiumDetail.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            var row = tblPremiumDetail.Rows[i];
            string strLevel=row["level"].ToString().Trim();
            if (strLevel == level + "")
            {
                row.Delete();
                tblPremiumDetail.AcceptChanges();
                break;
            }
        }

        // lblMessageApplication.Text ="Row index="+ rowIndex;
        ViewState["tblPersonal"] = tbl;
        //reload personal detail
        gvPersonalInfo.DataSource = tbl;
        gvPersonalInfo.DataBind();

        ViewState["tblPremiumDetail"] = tblPremiumDetail;
        //reload premium detail
        gvPremiumDetail.DataSource = tblPremiumDetail;
        gvPremiumDetail.DataBind();

        txtTotalPremium.Text = GetTotalPremium() + "";

        //CalculatePremium();


    }

    //update person
    protected void btnUpdatePerson_Click(object sender, EventArgs e)
    {
        //update and delete some rows of person
        DataTable tbl = (DataTable)ViewState["tblPersonal"];
        if (tbl.Rows.Count > 0)
        {
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var row = tbl.Rows[i];
               
                {// life insured
                    #region Update into database

                    if (row["id"].ToString().Trim() != "")
                    { //update database
                        try
                        {
                            da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                            person.App_Register_ID = My_View_State.App_Register_ID;
                            person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                            person.Country_ID = row["nationality"].ToString();
                            person.Father_First_Name = "";
                            person.Father_Last_Name = "";
                            person.First_Name = row["firstEnName"].ToString();
                            person.Last_Name = row["surEnName"].ToString();
                            person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                            person.Khmer_Last_Name = row["surKhName"].ToString();
                            if (row["gender"].ToString().Trim() == "Male")
                            {
                                person.Gender = 1;
                            }
                            else
                            {
                                person.Gender = 0;
                            }
                            person.ID_Card = row["idNumber"].ToString();
                            person.ID_Type = Convert.ToInt32(row["idtype"].ToString());
                            person.Level =Convert.ToInt32(row["level"].ToString().Trim());
                            person.Mother_First_Name = "";
                            person.Mother_Last_Name ="";
                            person.Person_ID = row["id"].ToString().Trim();
                            person.Prior_First_Name = "";
                            person.Prior_Last_Name = "";
                            person.Marital_Status = row["Marital_Status"].ToString();
                            person.Relationship = row["Relationship"].ToString();

                            if (da_application_fp6.UpdateAppInfoPersonSub(person))
                            {
                                lblMessagePersonalInfo.Text = "Updated successfully.";
                            }
                            else
                            {
                                lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.AddExceptionToLog("Error update person [btnUpdatePerson_Click] page [application_form_fp6], Detail: " + ex.Message);
                            return;
                        }

                    }
                    #endregion

                    #region Save into database

                    else
                    { //save into database
                        try
                        {
                            da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                            person.App_Register_ID = My_View_State.App_Register_ID;
                            person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                            person.Country_ID = row["nationality"].ToString();
                            person.Father_First_Name = "";
                            person.Father_Last_Name = "";
                            person.First_Name = row["firstEnName"].ToString();
                            person.Last_Name = row["surEnName"].ToString();
                            person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                            person.Khmer_Last_Name = row["surKhName"].ToString();
                            if (row["gender"].ToString().Trim() == "Male")
                            {
                                person.Gender = 1;
                            }
                            else
                            {
                                person.Gender = 0;
                            }
                            person.ID_Card = row["idNumber"].ToString();
                            person.ID_Type = Convert.ToInt32(row["idtype"].ToString());

                            person.Level =Convert.ToInt32(row["level"].ToString().Trim());
                            person.Mother_First_Name = "";
                            person.Mother_Last_Name = "";

                            string id = Helper.GetNewGuid("SP_Check_App_Info_Person_Sub_ID", "Person_ID");

                            person.Person_ID = id;
                            person.Prior_First_Name = "";
                            person.Prior_Last_Name = "";

                            person.Marital_Status = row["Marital_Status"].ToString();
                            person.Relationship = row["Relationship"].ToString();

                            if (da_application_fp6.InsertAppInfoPersonSub(person))
                            {
                                lblMessagePersonalInfo.Text = "Updated successfully.";
                            }
                            else
                            {
                                lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.AddExceptionToLog("Error saved person [btnUpdatePerson_Click] page [application_form_fp6], Detail: " + ex.Message);

                        }

                    }
                    #endregion
                }
            }
            #region Delete data in database base on records deleted from gridview
            try
            {
                string personIDs = "";
                if (ViewState["personID"] != null)
                {
                    personIDs = ViewState["personID"] + "";

                    personIDs = personIDs.Substring(0, personIDs.Length - 1);

                    char[] a = { ',' };
                    string[] delete = personIDs.Split(a);
                    for (int i = 0; i < delete.Length; i++)
                    {
                        if (!da_application_fp6.DeleteAppInfoPersonSubByPersonIDs(delete[i]))
                        {
                            lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                        }
                        else
                        {

                            lblMessagePersonalInfo.Text = "Updated successfully.";
                            ViewState["personID"] = null;
                        }
                    }
                }

                if (SaveProductLife(My_View_State.App_Register_ID))
                {

                }
                #region Save Premium Pay
                DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
                string appID="";
                appID = My_View_State.App_Register_ID;
                //delete old records
               
                foreach (DataRow rowDel in tblPremiumDetail.Rows)
                {
                    int level = 0;
                    level=Convert.ToInt32(rowDel["level"].ToString());
                    if (level>1)//delete spouse and kids
                    {
                        if (da_application_fp6.DeleteAppPremPayByLevel(appID, level))
                        {

                        }
                    }
                }
                foreach (DataRow row in tblPremiumDetail.Rows)
                {
                   
                    ViewState["tblPermiumDetail"] = tblPremiumDetail;
                    if (SavePremPay(Helper.GetNewGuid("SP_Check_App_Prem_Pay_ID", "@App_Prem_Pay_ID"), 
                                    appID, 
                                    Helper.FormatDateTime(row["effective_date"].ToString()),
                                    1,
                                    Convert.ToDouble(row["premium"].ToString()),
                                    Convert.ToDouble(row["original_premium"].ToString()),
                                    Math.Ceiling(Convert.ToDouble(row["original_premium"].ToString())),
                                    ViewState["userName"]+"",
                                    "",//created note
                                    DateTime.Now,
                                    Convert.ToInt32(row["level"].ToString())))
                    { 
                    
                    }
                }
                #endregion End Save Premium Pay
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Delete personal data in function [btnUpdatePerson_Click] in page [application_form_fp6.aspx.cs], Detail: " + ex.Message);
            }
            #endregion
            //reload data
            //call load person

            LoadPerson(My_View_State.App_Register_ID);
        }
        else
        {
            lblMessagePersonalInfo.Text = "List of Person is empty cannot be updated.";
        }


    }
   

    //update job history
    protected void btnSaveJob_Click(object sender, EventArgs e)
    {
        //update job history
        string appID = "";
        bool success = false;
        try
        {
                appID = My_View_State.App_Register_ID;

            if (appID.Trim() != "")
            {
                if (InsertAppJobHistorySubList(appID))
                { //success
                    success = true;
                    //delete data
                    if (ViewState["jobID"] != null)
                    {
                        string jobId = "";
                        jobId = ViewState["jobID"] + "";
                        jobId = jobId.Substring(0, jobId.Length - 1);

                        char[] a = { ',' };
                        string[] delete = jobId.Split(a);
                        for (int i = 0; i < delete.Length; i++)
                        {
                            da_application_fp6.bl_app_job_history_sub jobhistory = new da_application_fp6.bl_app_job_history_sub();
                            jobhistory.App_Register_ID = appID;
                            jobhistory.Job_ID = delete[i];
                            if (da_application_fp6.DeleteAppJobHistoryByJobIDs(jobhistory))
                            { //inserted success
                                success = true;
                            }
                            else
                            { //inserted fail
                                success = false;
                                break;
                            }
                        }

                    }

                   
                }
                else
                { //fail
                    success = false;
                    
                }

                if (success)
                {
                    lblMessageJobHistory.Text = "Saved successfully.";
                }
                else
                {
                    lblMessageJobHistory.Text = "Saved fail, please contact your system administrator.";
                }

                //reload data
                LoadJobHistory(appID);
            }
        }

        catch (Exception ex)
        {
            Log.AddExceptionToLog("Update Job history for policy owner was fail  in function [btnSaveJob_Click] in page [application_form_fp6.aspx.cs]. Details: " + ex.Message);
            return;
        }

    }
   
    //Enable buttons for update 
    void EnableButtonsUpdate(bool status)
    {
        
       
        //personal
        btnUpdatePerson.Visible = status;
        //job history
        btnSaveJob.Visible = status;
      
        //health
        btnBodySave.Visible = status;
    }

    #region Clear form
  

    //Clear Personal Information
    void ClearPersonalInformation()
    {
        ClearPerson();
        //Clear record in datatable
        DataTable tbl = (DataTable)ViewState["tblPersonal"];

        tbl.Clear();

        ViewState["tblPersonal"] = tbl;

        gvPersonalInfo.DataSource = tbl;
        gvPersonalInfo.DataBind();

        // btnUpdatePerson.Enabled = false;
    }
    
    //Clear Job History
    void ClearJobHistory()
    {
        lblMessageJobHistory.Text = "";
        //policy owner
        txtNameEmployerLife1.Text = "";
        txtNatureBusinessLife1.Text = "";
        txtRoleAndResponsibilityLife1.Text = "";
        txtCurrentPositionLife1.Text = "";
        txtAnualIncomeLife1.Text = "";

        DataTable tblJobHistory = (DataTable)ViewState["tblJobHistory"];
        tblJobHistory.Clear();
        gvJobHistory.DataSource = tblJobHistory;
        gvJobHistory.DataBind();

        // btnSaveJob.Enabled = false;
    }

   
    //Clear Body
    void ClearBody()
    {
        lblMessageHealth.Text = "";

        txtHeightLife1.Text = "";
        txtWeightLife1.Text = "";
        rblWeightChangeLife1.SelectedIndex = 0;
        txtWeightChangeReasonLife1.Text = "";
        lblWeightChangeReasonLife1.Visible = false;

        DataTable tblBody = (DataTable)ViewState["tblBody"];
        tblBody.Clear();

        gvBody.DataSource = tblBody;
        gvBody.DataBind();

    }

    //Clear Answers
    void ClearAnswer()
    {
        foreach (GridViewRow row in GvQA.Rows)
        {
            RadioButtonList rbtnlAnswerLife1 = (RadioButtonList)row.FindControl("rbtnlAnswerLife1");
            RadioButtonList rbtnlAnswerLife2 = (RadioButtonList)row.FindControl("rbtnlAnswerLife2");
            RadioButtonList rbtnlAnswerLife3 = (RadioButtonList)row.FindControl("rbtnlAnswerLife3");
            RadioButtonList rbtnlAnswerLife4 = (RadioButtonList)row.FindControl("rbtnlAnswerLife4");
            RadioButtonList rbtnlAnswerLife5 = (RadioButtonList)row.FindControl("rbtnlAnswerLife5");
            RadioButtonList rbtnlAnswerLife6 = (RadioButtonList)row.FindControl("rbtnlAnswerLife6");
            rbtnlAnswerLife1.SelectedIndex = -1;
            rbtnlAnswerLife2.SelectedIndex = -1;
            rbtnlAnswerLife3.SelectedIndex = -1;
            rbtnlAnswerLife4.SelectedIndex = -1;
            rbtnlAnswerLife5.SelectedIndex = -1;
            rbtnlAnswerLife6.SelectedIndex = -1;
        }
    }


    //Clear All
    void ClearForm()
    {
       
        ClearPersonalInformation();
        ClearJobHistory();
       
        ClearBody();
        ClearAnswer();



    }
    #endregion

    protected void ImgBtnClear_Click(object sender, ImageClickEventArgs e)
    {
        ClearForm();
        //Clear Viewstate

        ViewState.Clear();
    }
  
    //add job history into gridview
    protected void btnAddJobHistory_Click(object sender, EventArgs e)
    {
        string appID = "";

        appID = My_View_State.App_Register_ID;
        try
        {
            if (appID != "")
            {

                DataTable tblJob = (DataTable)ViewState["tblJobHistory"];
                DataRow row;
               
                if (ViewState["tblJobHistoryRowIndex"] == null)
                {//save record into datatable

                    //check owner 
                    int count = tblJob.Rows.Count;
                    bool exist = false;
                   
                    if (ddlJobPolicyOwner.SelectedValue.Trim() != "")
                    { 
                        if (count >= 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                string strLevel = tblJob.Rows[i]["Level"].ToString().Trim();
                                if (strLevel.Trim() == ddlJobPolicyOwner.SelectedValue.Trim())
                                {//check douplicate data
                                    
                                    exist = true;
                                    break;

                                }
                            }
                            if (exist)
                            {
                                lblMessageJobHistory.Text = "Record is already exist.";
                                return;
                            }
                            //add data in data table
                            row = tblJob.NewRow();
                            row["app_register_id"] = appID;
                            row["job_id"] = "";
                            row["employer_name"] = txtNameEmployerLife1.Text.Trim();
                            row["nature_of_business"] = txtNatureBusinessLife1.Text.Trim();
                            row["current_position"] = txtCurrentPositionLife1.Text.Trim();
                            row["job_role"] = txtRoleAndResponsibilityLife1.Text.Trim();
                            row["anual_income"] = txtAnualIncomeLife1.Text.Trim();
                            row["level"] = ddlJobPolicyOwner.SelectedValue.Trim();
                            row["address"] = txtAddress.Text.Trim();

                            tblJob.Rows.Add(row);
                            ViewState["tblJobHistory"] = tblJob;
                            gvJobHistory.DataSource = tblJob;
                            gvJobHistory.DataBind();

                            //clear message
                            lblMessageJobHistory.Text = "";
                           
                        }
                        else
                        {
                            lblMessageJobHistory.Text = "Record Not found.";
                            return;
                        }

                    }

                }
                else// if gridview row seleted update record into datatable
                {
                    try
                    {
                        int rowIndex = Convert.ToInt32(ViewState["tblJobHistoryRowIndex"] + "");
                        var r = tblJob.Rows[rowIndex];

                        r["employer_name"] = txtNameEmployerLife1.Text.Trim();
                        r["nature_of_business"] = txtNatureBusinessLife1.Text.Trim();
                        r["current_position"] = txtCurrentPositionLife1.Text.Trim();
                        r["job_role"] = txtRoleAndResponsibilityLife1.Text.Trim();
                        r["anual_income"] = txtAnualIncomeLife1.Text.Trim();
                        r["address"] = txtAddress.Text.Trim();
                        r["level"] = ddlJobPolicyOwner.SelectedValue.Trim();

                        tblJob.AcceptChanges();
                        ViewState["tblJobHistory"] = tblJob;
                        gvJobHistory.DataSource = tblJob;
                        gvJobHistory.DataBind();

                    }
                    catch (Exception ex)
                    {
                        Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add Job History. Detail: " + ex.Message);

                        lblMessagePersonalInfo.Text = "Update fail, please Job History your system administrator.";

                    }
                }

                //Clear text box
                ClearJobHistoryInfo();

            }
            else // if application number is empty
            {
                //alert message
                lblMessageJobHistory.Text = "Application number is required.";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function btnAddJobHistory_Click in page application_form_fp6.aspx, Detail: " + ex.Message);
            lblMessageJobHistory.Text = "Add job history fail, please contact your system administrator.";
        }


    }
    void ClearJobHistoryInfo()
    {
        //Clear text box

        txtNameEmployerLife1.Text = "";
        txtNatureBusinessLife1.Text = "";
        txtRoleAndResponsibilityLife1.Text = "";
        txtCurrentPositionLife1.Text = "";
        txtAnualIncomeLife1.Text = "";
        txtAddress.Text = "";

        btnAddJobHistory.Text = "Add";
        ViewState["tblJobHistoryRowIndex"] = null;
    }
    protected void gvJobHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblJobHistory = (DataTable)ViewState["tblJobHistory"];
            int rowIndex = e.RowIndex;

            string id = "";
            id = tblJobHistory.Rows[rowIndex]["job_id"].ToString().Trim();
            if (id != "" && id != "0")
            {

                ViewState["jobID"] += id + ",";
                
            }

            tblJobHistory.Rows[rowIndex].Delete();
            tblJobHistory.AcceptChanges();

            ViewState["tblJobHistory"] = tblJobHistory;
            gvJobHistory.DataSource = tblJobHistory;
            gvJobHistory.DataBind();


        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Delete Job history error in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }
    }
    protected void gvJobHistory_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataTable tblJob = (DataTable)ViewState["tblJobHistory"];
        if (tblJob.Rows.Count > 0)
        {
            int row = e.NewSelectedIndex;
            var a = tblJob.Rows[row];

            ViewState["tblJobHistoryRowIndex"] = row;
            btnAddJobHistory.Text = "Update";

            txtNameEmployerLife1.Text = a["employer_name"].ToString().Trim();
            txtNatureBusinessLife1.Text = a["nature_of_business"].ToString().Trim();
            txtCurrentPositionLife1.Text = a["current_position"].ToString().Trim();
            txtRoleAndResponsibilityLife1.Text = a["job_role"].ToString().Trim();
            txtAnualIncomeLife1.Text = a["anual_income"].ToString().Trim();
            txtAddress.Text = a["address"].ToString().Trim();

            //select item in dropdown list
            Helper.SelectedDropDownListIndex("VALUE", ddlJobPolicyOwner, a["level"].ToString().Trim());

        }
        else //if no record in data table
        {
            //alret message
            lblMessageJobHistory.Text = "Record not found.";
        }
    }
    protected void btnAddWeight_Click(object sender, EventArgs e)
    {
        string appID = "";
        appID = My_View_State.App_Register_ID;
        try
        {
            if (appID != "")
            {

                DataTable tblBody = (DataTable)ViewState["tblBody"];
                DataRow row;
               
                if (ViewState["tblBodyRowIndex"] == null)
                {//save record into datatable

                    //check owner 
                    int count = tblBody.Rows.Count;
                    string reason = "";

                    if (txtWeightChangeReasonLife1.Text.Trim() != "")
                    {
                        reason = txtWeightChangeReasonLife1.Text.Trim();
                    }

                    row = tblBody.NewRow();
                    row["app_register_id"] = appID;
                    row["weight"] = txtWeightLife1.Text.Trim();
                    row["height"] = txtHeightLife1.Text.Trim();
                    row["is_weight_changed"] = rblWeightChangeLife1.SelectedValue;
                    row["reason"] = reason;
                    row["level"] = ddlBodyPerson.SelectedValue.Trim();
                    row["id"] = "";

                    tblBody.Rows.Add(row);
                    ViewState["tblBody"] = tblBody;
                    gvBody.DataSource = tblBody;
                    gvBody.DataBind();

                    //clear message
                    lblMessageJobHistory.Text = "";
                }
                else// if gridview row seleted update record into datatable
                {
                    try
                    {
                        int rowIndex = Convert.ToInt32(ViewState["tblBodyRowIndex"] + "");
                        var r = tblBody.Rows[rowIndex];
                        string reason = "";

                        if (txtWeightChangeReasonLife1.Text.Trim() != "")
                        {
                            reason = txtWeightChangeReasonLife1.Text.Trim();
                        }
                        r["app_register_id"] = appID;
                        r["weight"] = txtWeightLife1.Text.Trim();
                        r["height"] = txtHeightLife1.Text.Trim();
                        r["is_weight_changed"] = rblWeightChangeLife1.SelectedValue;
                        r["reason"] = reason;
                        r["level"] = ddlBodyPerson.SelectedValue.Trim();

                        tblBody.AcceptChanges();
                        ViewState["tblBody"] = tblBody;
                        gvBody.DataSource = tblBody;
                        gvBody.DataBind();

                    }
                    catch (Exception ex)
                    {
                        Log.AddExceptionToLog("Error in page [application_form_fp6.aspx.cs] while user clicks button Add body. Detail: " + ex.Message);

                        lblMessagePersonalInfo.Text = "Update fail, please contact your system administrator.";

                    }
                }

                //Clear text box
                ClearBodyInfo();

            }
            else // if application number is empty
            {
                //alert message
                lblMessageJobHistory.Text = "Application number is required.";
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function btnAddJobHistory_Click in page application_form_fp6.aspx, Detail: " + ex.Message);
            lblMessageJobHistory.Text = "Add job history fail, please contact your system administrator.";
        }

    }

    protected void gvBody_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            int rowIndex = e.RowIndex;

            string id = "";
            id = tblBody.Rows[rowIndex]["id"].ToString().Trim();
            if (id != "")
            {

                ViewState["id"] += id + ",";
            }

            tblBody.Rows[rowIndex].Delete();
            tblBody.AcceptChanges();

            ViewState["tblBody"] = tblBody;
            gvBody.DataSource = tblBody;
            gvBody.DataBind();

            //lblMessageHealth.Text = "table body rows=" + tblBody.Rows.Count + "";

        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Delete Body error in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }
    }
    protected void gvBody_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        DataTable tblBody = (DataTable)ViewState["tblBody"];
        if (tblBody.Rows.Count > 0)
        {
            int row = e.NewSelectedIndex;
            var a = tblBody.Rows[row];

            ViewState["tblBodyRowIndex"] = row;
            btnAddWeight.Text = "Update";
            txtWeightLife1.Text = a["weight"].ToString().Trim();
            txtHeightLife1.Text = a["height"].ToString().Trim();
            rblWeightChangeLife1.SelectedIndex = Convert.ToInt32(a["is_weight_changed"].ToString().Trim());
            if (Convert.ToInt32(a["is_weight_changed"].ToString().Trim()) > 0)
            {
                lblWeightChangeReasonLife1.Visible = true;
                txtWeightChangeReasonLife1.Visible = true;
                txtWeightChangeReasonLife1.Text = a["reason"].ToString().Trim();

            }
            else
            {
                lblWeightChangeReasonLife1.Visible = false;
                txtWeightChangeReasonLife1.Visible = false;
                txtWeightChangeReasonLife1.Text = "";

            }

            //select item in dropdown list
            Helper.SelectedDropDownListIndex("VALUE", ddlBodyPerson, a["level"].ToString().Trim());

        }
        else //if no record in data table
        {
            //alret message
            lblMessageHealth.Text = "Record not found.";
        }
    }
    void ClearBodyInfo()
    {
        ViewState["tblBodyRowIndex"] = null;
        btnAddWeight.Text = "Add";
        txtWeightLife1.Text = "";
        txtHeightLife1.Text = "";
        rblWeightChangeLife1.SelectedIndex = 0;
        lblWeightChangeReasonLife1.Visible = false;
        txtWeightChangeReasonLife1.Visible = false;
        txtWeightChangeReasonLife1.Text = "";
    }
    protected void btnClearWeight_Click(object sender, EventArgs e)
    {
        ClearBodyInfo();
        gvBody.SelectedIndex = -1;
    }
    protected void btnBodySave_Click(object sender, EventArgs e)
    {
        try
        {
            #region Save body
            DataTable tblBody = (DataTable)ViewState["tblBody"];
            int rowCount = tblBody.Rows.Count;
            string appID ="";
            //store appid for saving answer and delete some record 
            appID = tblBody.Rows[0]["app_register_id"].ToString();

            for (int i = 0; i < rowCount; i++)
            {
                var row = tblBody.Rows[i];

                // level >0 is life insured
                if (row["id"].ToString().Trim() == "")
                { //id=="" save new

                    da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                    body.App_Register_ID = row["app_register_id"].ToString().Trim();
                    body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                    body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                    body.Level = Convert.ToInt32(row["level"].ToString().Trim());
                    body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                    body.Reason = row["reason"].ToString().Trim();
                    body.Id = Helper.GetNewGuid("SP_Check_App_Info_Body_Sub_ID", "@Id");



                    if (da_application_fp6.InsertAppInfoBodySub(body))
                    {
                        lblMessageHealth.Text = "Updated successfully.";
                    }
                    else
                    {
                        lblMessageHealth.Text = "Health updated fail, please contact your system administrator.";
                        return;
                    }
                }
                else
                { //id!="" update
                    da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                    body.App_Register_ID = row["app_register_id"].ToString().Trim();
                    body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                    body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                    body.Level = Convert.ToInt32(row["level"].ToString().Trim());
                    body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                    body.Id = row["id"].ToString().Trim();
                    body.Reason = row["reason"].ToString().Trim();

                    if (da_application_fp6.UpdateAppInfoBodySub(body))
                    {
                        lblMessageHealth.Text = "Updated successfully.";
                    }
                    else
                    {
                        lblMessageHealth.Text = "Health updated fail, please contact your system administrator.";
                        return;
                    }

                }

            }
            #endregion End Save body

            #region Delete some record
            if (ViewState["id"] != null || ViewState["id"] + "" != "")
            {
                string id = "";
                id = ViewState["id"] + "";
                id = id.Substring(0, id.Length - 1);

                char[] a = { ',' };
                string[] delete = id.Split(a);
                for (int i = 0; i < delete.Length; i++)
                {
                    da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                    body.App_Register_ID =appID;
                    body.Id = delete[i];

                    if (da_application_fp6.DeleteAppInfoBodySubByIDs(body))
                    {
                        lblMessageHealth.Text = "Updated successfully.";
                        //ViewState["id"] = null;
                    }
                    else
                    {
                        lblMessageHealth.Text = "Questions Deleted fail, please contact your system administrator.";
                    }
                }


            }
            #endregion End Delete some record

            #region Save Answer

            DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
            int Count = tblPersonal.Rows.Count;
           
            //delete existing answer
            for (int ex = 0; ex < Count; ex++)
            {
                var row = tblPersonal.Rows[ex];
                if (row["level"].ToString().Trim() != "0")
                {
                    da_application_fp6.DeleteAppAnswerByAppIDAndLevel(appID, Convert.ToInt32(row["level"].ToString()));
                }
                
            }

            if (InsertAppAnswerItem(appID))
            {
                lblMessageHealth.Text = "Updated successfully.";
            }
            else
            {
                lblMessageHealth.Text = "Questions updated fail, please contact your system administrator.";
            }

            #endregion End Save Answer


            //reload data
            LoadBody(appID);
            LoadAnswers(appID);
        }

        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function btnBodySave_Click in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        }
    }

    protected void btnClearJobHistory_Click(object sender, EventArgs e)
    {
        ClearJobHistoryInfo();
    }
    
    private bool SavePremPay(string prem_pay_id, string app_register_id, DateTime pay_date, int is_init_pay, double amount, double original_amount, double rounded_amount, string created_by, string created_note, DateTime created_on, int level)
    {

        bool status = false;
        try
        {
            da_application_fp6.bl_app_prem_pay_fp6 app_prem_pay = new da_application_fp6.bl_app_prem_pay_fp6();

            app_prem_pay.App_Prem_Pay_ID = prem_pay_id;
            app_prem_pay.App_Register_ID = app_register_id;
            app_prem_pay.Pay_Date = pay_date;
            app_prem_pay.Is_Init_Payment = is_init_pay;
            app_prem_pay.Amount = amount;
            app_prem_pay.Original_Amount = original_amount;
            app_prem_pay.Created_By = created_by;
            app_prem_pay.Created_Note = created_note;
            app_prem_pay.Created_On = created_on;
            app_prem_pay.Rounded_Amount = rounded_amount;
            app_prem_pay.Level = level;

            if (da_application_fp6.InsertAppPremPayFP6(app_prem_pay))
            {
                status = true;
            }
            else
            {
                status = false;
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function SavePremPay in page application_form_fp6.aspx. Detail:" + ex.Message);
            status = false;
        }

        return status;
    }


    private bool CheckVarlidAge(int age, int level)
    {
        bool varlid = false;

        if (level == 1) //spouse
        {
            if (age > 55 || age < 18)
            {
                varlid = false;
            }
            else
            {
                varlid = true;
            }
        }
        else
        {//life 2 - 5 (child)
            if (age > 17 || age < 1)
            {
                varlid = false;
            }
            else
            {
                varlid = true;
            }
        }

        return varlid;

    }
    private bool Check_ADB_TPD_Varlid_Age(int age)
    {
        bool varlid = false;

        if (age > 60 && age < 18)
        {
            varlid = false;
        }
        else
        {
            varlid = true;
        }

        return varlid;

    }
    protected void DisableControls(Control parent, bool State)
    {
        foreach (Control c in parent.Controls)
        {
            if (c is DropDownList)
            {
                ((DropDownList)(c)).Enabled = State;
            }
            else if (c is TextBox)
            {
                ((TextBox)(c)).Enabled = State;
            }
            else if (c is Button)
            {
                ((Button)(c)).Enabled = State;
            }
            else if (c is GridView)
            {
                ((GridView)(c)).Enabled = State;
            }

            DisableControls(c, State);
        }
    }

    private bool AddPremiumDetailRow(int level, string fullName, string gender, string dob, string effectiveDate, double sumInsure)
    {
        bool result = false;
        int age = 0;
        double newSumInsure = 0.0;
        double premium = 0.0;
        double originalPremium = 0.0;
        int paymentMode=-1;
        //double sumInsure=0.0;
        int assurePlan=0;
        string productId="";
        int intGender = -1;

        try
        {
            //double totalOriginalPremium = 0.0;
            //double totalPremium = 0.0;
            DataRow row;
                           
            paymentMode = My_View_State.Pay_Mode ;
            assurePlan = My_View_State.Assure_Plan;
            productId = My_View_State.Product_ID;

            DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];

            if (dob.Trim() != "" && gender + "".Trim() != "" && paymentMode > -1 && sumInsure != 0 && assurePlan > 0)
            {
                #region If policy is already issued
                My_View_State.prem_lot = da_policy_prem_lot.Get_Policy_Prem_Lot(My_View_State.Policy_ID);
                int policy_insured_year = 0;
                effectiveDate = Helper.FormatDateTime(effectiveDate) + "";
                if (My_View_State.prem_lot.Prem_Year > 0)
                {
                    policy_insured_year = My_View_State.prem_lot.Prem_Year;
                    //compare system effective date with user input effective date
                   
                    if (Convert.ToDateTime(effectiveDate) != My_View_State.prem_lot.Due_Date)
                    {
                        effectiveDate = da_policy_prem_pay.Get_Next_Due_by(My_View_State.prem_lot.Pay_Mod, My_View_State.prem_lot.Due_Date) + "";
                    }
                }
               
                #endregion

                //get age
                age = Calculation.Culculate_Customer_Age(dob, Convert.ToDateTime(effectiveDate));
                if (gender == "Male")
                {
                    intGender = 1;
                }
                else
                {
                    intGender = 0;
                }
                // product id 
                string sub_product_id = "";
                sub_product_id = productId.Trim().Substring(0, 3).ToUpper();
                #region Family Protection
                if (sub_product_id == "NFP")
                {
                    if (level == 2)
                    {//spouse 
                        My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 70, policy_insured_year);
                        //newSumInsure = sumInsure * 0.5;
                        newSumInsure = sumInsure;

                        //premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse
                        //originalPremium = da_application_fp6.getAnnualPremiumFP6Sub(newSumInsure, productId, intGender, age, My_View_State.Age_Assure_Pay.Assure_Year, 0);
                        double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse
                        premium = arr_premium[0, 0];
                        originalPremium = arr_premium[0, 1];
                    }
                    else if (level > 2)
                    {//kids 
                       
                        My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);
                        //newSumInsure = sumInsure * 0.25;
                        newSumInsure = sumInsure;

                        //premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1); //for child
                        //originalPremium = da_application_fp6.getAnnualPremiumFP6Sub(newSumInsure, productId, intGender, age, My_View_State.Age_Assure_Pay.Assure_Year, 1);
                        double [,] arr_premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1); //for child
                        premium = arr_premium[0, 0];
                        originalPremium = arr_premium[0, 1];
                    }

                }
                #endregion

                #region Family Protection Package
                else if (sub_product_id == "FPP")//family protection package
                {
                    double life_sum_insure = 0;
                    life_sum_insure = My_View_State.Sum_Insured;
                    if (level == 2)
                    {//spouse 
                        newSumInsure = sumInsure;
                        if (age >= 18 && age <= 60)
                        {
                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 65, policy_insured_year);
                            premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(productId, intGender, 0, assurePlan, 0);//for spouse
                            if (life_sum_insure == 5000)
                            {
                                premium = Math.Ceiling(premium / 2);
                            }
                            originalPremium = premium;
                        }
                    }
                    else if (level > 2)
                    {//kids 
                        newSumInsure = sumInsure;
                        if (age >= 1 && age <= 17)
                        {
                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);
                            premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(productId, intGender, 0, assurePlan, 1);//for kids
                            
                            if (life_sum_insure == 5000)
                            {
                                premium = Math.Ceiling(premium / 2);
                            }
                            originalPremium = premium;
                        }
                    }
                }
                #endregion

                #region Study save
                else if (sub_product_id == "SDS")
                {
                    string[] arr_product = productId.Split('/');
                    double[,] arr_premium = new double[,]{{0,0}};
                    newSumInsure = sumInsure;
                    #region study save normal
                    if (arr_product.Length == 2)
                    {

                        if (level == 2)
                        {//spouse 
                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 70, policy_insured_year);
                            arr_premium = da_application_study_save.study_save.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse
                          
                        }
                        else if (level > 2)
                        {//kids 

                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);
                            arr_premium = da_application_study_save.study_save.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1); //for child
                        }

                    }
                    #endregion

                    #region study save package
                    else if (arr_product.Length > 2)
                    {
                        if (level == 2)
                        {//spouse 
                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 70, policy_insured_year);
                            //arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse
                            arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, assurePlan, 0);//for spouse
                          
                        }
                        else if (level > 2)
                        {//kids 

                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);
                            //arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1);//for child
                            arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, assurePlan , 1);//for child
   
                        }
                    }
                    #endregion
                    premium = arr_premium[0, 0];
                    originalPremium = arr_premium[0, 1];
                }
                #endregion

                //totalPremium += premium;
                //totalOriginalPremium += originalPremium;
                
                row = tblPremiumDetail.NewRow();
                row["level"] = level;
                row["full_name"] = fullName;
                row["gender"] = gender;
                row["birth_date"] = dob;
                row["effective_date"] = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(effectiveDate));
                row["age_insure"] = age;
                row["assure_year"] = My_View_State.Age_Assure_Pay.Assure_Year;
                row["assure_up_to_age"] = My_View_State.Age_Assure_Pay.Assure_Up_To_Age;
                row["pay_year"] = My_View_State.Age_Assure_Pay.Pay_Year;
                row["pay_up_to_age"] = My_View_State.Age_Assure_Pay.Pay_Up_To_Age;
                row["pay_mode"] = paymentMode;
                row["product_id"] = productId;
                row["sum_insure"] = newSumInsure;
                row["premium"] = premium;
                row["original_premium"] = originalPremium;
                row["created_on"] = DateTime.Now;
                row["record_id"] = "";
                
                tblPremiumDetail.Rows.Add(row);

                gvPremiumDetail.DataSource = tblPremiumDetail;
                gvPremiumDetail.DataBind();

                txtTotalPremium.Text = GetTotalPremium() + "";
            }
            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function AddPremiumDetailRow in page application_form_fp6_rider.aspx.cs, Detail: "+ ex.Message);
            result = false;
        }
        return result;

    }
    private bool UpdatePremiumDetailRow(int oldLevel, int level, string fullName, string gender, string dob, string effectiveDate, double sumInsure)
    {
        bool result = false;
        int age = 0;
        double newSumInsure = 0.0;
        double premium = 0.0;
        double originalPremium = 0.0;
        int paymentMode = -1;
        //double sumInsure = 0.0;
        int assurePlan = 0;
        string productId = "";
        int intGender = -1;

        double life_sum_insure = 0;
        life_sum_insure = My_View_State.Sum_Insured;

        try
        {
            paymentMode = My_View_State.Pay_Mode;
            assurePlan = My_View_State.Assure_Plan;
            productId = My_View_State.Product_ID;

            DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];

            if (dob.Trim() != "" && gender + "".Trim() != "" && paymentMode > -1 && sumInsure != 0 && assurePlan > 0)
            {
                #region If policy is already issued
                My_View_State.prem_lot = da_policy_prem_lot.Get_Policy_Prem_Lot(My_View_State.Policy_ID);
                int policy_insured_year = 0;
                if (My_View_State.prem_lot.Prem_Year > 0)
                {
                    policy_insured_year = My_View_State.prem_lot.Prem_Year;
                }
                //compare system effective date with user input effective date
                effectiveDate = Helper.FormatDateTime(effectiveDate)+"";
                if (Convert.ToDateTime(effectiveDate) != My_View_State.prem_lot.Due_Date)
                {
                    effectiveDate = da_policy_prem_pay.Get_Next_Due_by(My_View_State.prem_lot.Pay_Mod, My_View_State.prem_lot.Due_Date) + "";
                }
                #endregion

                //get age
                //age = Calculation.Culculate_Customer_Age(dob, Helper.FormatDateTime(effectiveDate));
                age = Calculation.Culculate_Customer_Age(dob, Convert.ToDateTime(effectiveDate));
                if (gender == "Male")
                {
                    intGender = 1;
                }
                else
                {
                    intGender = 0;
                }

                //get premium

                if (productId.Substring(0, 3).ToUpper().Trim() == "NFP")
                {
                    if (level == 2)
                    {//spouse 
                        My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 70, policy_insured_year);

                        //newSumInsure = sumInsure * 0.5;
                        newSumInsure = sumInsure;

                        //premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse
                        //originalPremium = da_application_fp6.getAnnualPremiumFP6Sub(newSumInsure, productId, intGender, age, My_View_State.Age_Assure_Pay.Assure_Year, 0);
                        double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse
                        premium = arr_premium[0, 0];
                        originalPremium = arr_premium[0, 1];

                    }
                    else if (level > 2)
                    {//kids 

                        My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);

                        //newSumInsure = sumInsure * 0.25;
                        newSumInsure = sumInsure;

                        //premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1); //for child
                        //originalPremium = da_application_fp6.getAnnualPremiumFP6Sub(newSumInsure, productId, intGender, age, My_View_State.Age_Assure_Pay.Assure_Year, 1);
                       double[,] arr_premium = da_application_fp6.getPremiumFP6Sub(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1); //for child
                       premium = arr_premium[0, 0];
                       originalPremium = arr_premium[0, 1];
                    }
                }
                else if (productId.Substring(0, 3).ToUpper().Trim() == "FPP")
                {
                    if (level == 2)
                    {//spouse 

                        My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 65, policy_insured_year);
                        newSumInsure = sumInsure;
                        if (age >= 18 && age <= 60)
                        {
                            premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(productId, intGender, 0, assurePlan, 0);
                            if(life_sum_insure==5000)
                            {
                                premium = Math.Ceiling(premium / 2);
                            }
                            originalPremium = premium;
                        }
                    }
                    else if (level > 2)
                    {//kids 
                        newSumInsure = sumInsure;

                        if (age >= 1 && age <= 17)
                        {
                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);
                            premium = da_application_fp6.getPremiumSpouseKidsFamilyProtectionPackage(productId, intGender, 0, assurePlan, 1);
                            if (life_sum_insure == 5000)
                            {
                                premium = Math.Ceiling(premium / 2);
                            }
                            originalPremium = premium;
                        }
                    }
                }
                #region Study save
                else if (productId.Substring(0, 3).ToUpper().Trim() == "SDS")
                {
                    string[] arr_product = productId.Split('/');
                    double[,] arr_premium = new double[,] { { 0, 0 } };
                    newSumInsure = sumInsure;
                    #region study save normal
                    if (arr_product.Length == 2)
                    {

                        if (level == 2)
                        {//spouse 
                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 70, policy_insured_year);
                            arr_premium = da_application_study_save.study_save.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse

                        }
                        else if (level > 2)
                        {//kids 

                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);
                            arr_premium = da_application_study_save.study_save.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1); //for child
                        }

                    }
                    #endregion

                    #region study save package
                    else if (arr_product.Length > 2)
                    {
                        if (level == 2)
                        {//spouse 
                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 70, policy_insured_year);
                            //arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 0);//for spouse
                            arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, assurePlan , 0);//for spouse

                        }
                        else if (level > 2)
                        {//kids 

                            My_View_State.Age_Assure_Pay = new da_application_fpp.Cal_Age_Assure_Pay_Year(age, assurePlan, 21, policy_insured_year);
                            //arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, My_View_State.Age_Assure_Pay.Assure_Year, 1);//for child
                            arr_premium = da_application_study_save.study_save_package.GetPremiumRider(newSumInsure, productId, intGender, age, paymentMode, assurePlan, 1);//for child

                        }
                    }
                    #endregion
                    premium = arr_premium[0, 0];
                    originalPremium = arr_premium[0, 1];
                }
                #endregion

               
                for (int i = 0; i < tblPremiumDetail.Rows.Count; i++)
                {
                    var row = tblPremiumDetail.Rows[i];
                    if (row["level"].ToString().Trim() == oldLevel + "")
                    {
                        row["level"] = level;
                        row["full_name"] = fullName;
                        row["gender"] = gender;
                        row["birth_date"] = dob;
                        //row["effective_date"] = effectiveDate;
                        row["effective_date"] = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(effectiveDate));
                        row["age_insure"] = age;
                        row["assure_year"] = My_View_State.Age_Assure_Pay.Assure_Year;
                        row["assure_up_to_age"] = My_View_State.Age_Assure_Pay.Assure_Up_To_Age;
                        row["pay_year"] = My_View_State.Age_Assure_Pay.Pay_Year;
                        row["pay_up_to_age"] = My_View_State.Age_Assure_Pay.Pay_Up_To_Age;
                        row["pay_mode"] = paymentMode;
                        row["product_id"] = productId;
                        row["sum_insure"] = newSumInsure;
                        row["premium"] = premium;
                        row["original_premium"] = originalPremium;
                        tblPremiumDetail.AcceptChanges();
                        break;
                    }
                }

                ViewState["tblPremiumDetail"] = tblPremiumDetail;
                gvPremiumDetail.DataSource = tblPremiumDetail;
                gvPremiumDetail.DataBind();

                txtTotalPremium.Text = GetTotalPremium() + "";
            }
            result = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function AddPremiumDetailRow in page application_form_fp6_rider.aspx.cs, Detail: " + ex.Message);
            result = false;
        }
        return result;

    }
    private double GetTotalPremium(){

        double total = 0.0;

        try
        {
            DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
            foreach (DataRow row in tblPremiumDetail.Rows)
            {
                total = total + Convert.ToDouble(row["premium"].ToString());
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function GetTotalPremium in page application_form_fp6_rider.aspx.cs, Detail: " + ex.Message);
        }
        return total;
    }
    private bool SaveProductLife(string appID)
    {
        bool status = false;
        try
        {
            //get old premium rider spouse + kids before delete
            double old_premium = 0.0;
            try
            {
                DataTable tblPremiumDetail1 = new DataTable();
                tblPremiumDetail1 = da_application_fp6.GetDataTable("SP_Get_App_Premium_Detail_By_App_Register_ID", appID);

                //total premium 
                double totalPremium = 0.0;
                foreach (DataRow row in tblPremiumDetail1.Rows)
                {
                    totalPremium = totalPremium + Convert.ToDouble(row["premium"].ToString());
                }
                //initialize to textbox
                old_premium = totalPremium;
            }
            catch (Exception ex)
            {
                Log.AddExceptionToLog("Error function SaveProductLife in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
            }
            

                DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
                da_application_fp6.bl_app_life_product_Sub life;
                

                //save
                foreach (DataRow row in tblPremiumDetail.Rows)
                {
                 
                    //check new or not
                    
                    if (row["record_id"].ToString().Trim()=="")
                    {
                        #region Save New Record
                        life = new da_application_fp6.bl_app_life_product_Sub();
                        life.App_Register_ID = appID;
                        life.Level = Convert.ToInt32(row["level"].ToString().Trim());
                        life.Product_ID = row["product_id"].ToString().Trim();
                        life.Assure_Year = Convert.ToInt32(row["assure_year"].ToString());
                        life.Pay_Year = Convert.ToInt32(row["pay_year"].ToString());
                        life.Age_Insure = Convert.ToInt32(row["age_insure"].ToString());
                        life.Assure_Up_To_Age = Convert.ToInt32(row["assure_up_to_age"].ToString());
                        life.Pay_Up_To_Age = Convert.ToInt32(row["pay_up_to_age"].ToString());
                        life.System_Sum_Insure = Convert.ToDouble(row["sum_insure"].ToString());
                        life.System_Premium = Convert.ToDouble(row["premium"].ToString());
                        life.Pay_Mode = Convert.ToInt32(row["pay_mode"].ToString());
                        life.EffectiveDate = Helper.FormatDateTime(row["effective_date"].ToString());
                        //life.EffectiveDate =Convert.ToDateTime(row["effective_date"].ToString());
                        life.Created_On = DateTime.Now;
                        life.Rider_ID = Helper.GetNewGuid("SP_Check_App_Life_Product_Sub_ID", "@Rider_ID");

                        if (My_View_State.prem_lot.Prem_Year > 0)
                        {
                            life.Action = "add";
                        }
                        else
                        {
                            life.Action = "new";
                        }


                        if (da_application_fp6.InsertAppLifeProductSub(life))
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Update Old Records
                        life = new da_application_fp6.bl_app_life_product_Sub();
                        life.App_Register_ID = appID;
                        life.Level = Convert.ToInt32(row["level"].ToString().Trim());
                        life.Product_ID = row["product_id"].ToString().Trim();
                        life.Assure_Year = Convert.ToInt32(row["assure_year"].ToString());
                        life.Pay_Year = Convert.ToInt32(row["pay_year"].ToString());
                        life.Age_Insure = Convert.ToInt32(row["age_insure"].ToString());
                        life.Assure_Up_To_Age = Convert.ToInt32(row["assure_up_to_age"].ToString());
                        life.Pay_Up_To_Age = Convert.ToInt32(row["pay_up_to_age"].ToString());
                        life.System_Sum_Insure = Convert.ToDouble(row["sum_insure"].ToString());
                        life.System_Premium = Convert.ToDouble(row["premium"].ToString());
                        life.Pay_Mode = Convert.ToInt32(row["pay_mode"].ToString());
                        life.EffectiveDate = Helper.FormatDateTime(row["effective_date"].ToString());
                        //life.EffectiveDate =Convert.ToDateTime(row["effective_date"].ToString());

                        if (da_application_fp6.UpdateAppLifeProductSub(life))
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                        #endregion
                    }

                }

            //Delete some record when user delete from grid view
                string del = "";
                del = ViewState["DELETE"] + "";
                if (del != "")
                {
                    string[] a = del.Split(',');
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ToString().Trim()!="")
                        {
                            status = da_application_fp6.DeleteAppLifeProductSubByLevel(Convert.ToInt32(a[i]), appID);
                            status = da_application_fp6.DeleteAppPremPayByLevel(appID, Convert.ToInt32(a[i]));
                        }
                    }
                }
                

                // Update application discount
                double premium_rider_new = 0.0;
                if (txtTotalPremium.Text.Trim() != "")
                {
                    premium_rider_new = Convert.ToDouble(txtTotalPremium.Text.Trim());
                }
                UpdateApplicationDiscount(appID);

           
            
        }
        catch(Exception ex)
        {
            Log.AddExceptionToLog("Error function SaveProductLife in page application_form_fp6_rider.aspx.cs, Detail: " + ex.Message);
            status = false;
        }
        return status;
    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("application_form_fp6.aspx?application_register_id=" + My_View_State.App_Register_ID);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error on click GoBack button, Detail: " + ex.Message);
        }
       
    }
    protected void GvQA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#00BFFF'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

        }
    }
    protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("application_form_fp6.aspx?application_register_id=" + My_View_State.App_Register_ID);
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            //Log.AddExceptionToLog("Error on click GoBack button, Detail: " + ex.Message);
        }
    }

    //Get rider suminsured by level from tblPremiumDetail to show personal detail form
    private double GetRiderSumInsured(int level)
    {
        double suminsured = 0.0;
        try
        {
            DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
            foreach (DataRow row in tblPremiumDetail.Rows)
            {
                int preLevel = 0;
                double preSuminsured = 0.0;
                //get level from premium detail table
                preLevel = Convert.ToInt32( row["level"].ToString());
                //get suminsured from premium detail table
                preSuminsured = Convert.ToDouble(row["sum_insure"].ToString());
                if (level == preLevel)
                {
                    suminsured = preSuminsured;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            suminsured = 0.0;
            Log.AddExceptionToLog("Error function [GetRiderSumInsured] in page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
        }
        return suminsured;
    }

  
protected void ddlRiderType_SelectedIndexChanged(object sender, EventArgs e)
{
    double new_sum_insured = 0.0;
    double recal_sum_insured = 0.0;
  
    if (My_View_State.Sum_Insured > 0 && My_View_State.Sum_Insured != null)
    {
        recal_sum_insured = My_View_State.Sum_Insured;
    }
    if (ddlRiderType.SelectedValue.Trim() == "2")
    {
     
        new_sum_insured = da_application_fp6.GetSpouseSumInsured(recal_sum_insured);
        txtRiderSumInsured.Text = new_sum_insured + "";
        
    }
    else if (ddlRiderType.SelectedValue.Trim() == "3" || ddlRiderType.SelectedValue.Trim() == "4" || ddlRiderType.SelectedValue.Trim() == "5" || ddlRiderType.SelectedValue.Trim() == "6")
    {

        new_sum_insured = da_application_fp6.GetKidSumInsured(recal_sum_insured);
        txtRiderSumInsured.Text = new_sum_insured + "";
      
    }
}
private string SavePersonal()
{
    string save_status = "";

    //update and delete some rows of person
    DataTable tbl = (DataTable)ViewState["tblPersonal"];
    if (tbl.Rows.Count > 0)
    {
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            var row = tbl.Rows[i];

            {// life insured
                #region Update into database

                if (row["id"].ToString().Trim() != "")
                { //update database
                    try
                    {
                        da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                        person.App_Register_ID = My_View_State.App_Register_ID;
                        person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                        person.Country_ID = row["nationality"].ToString();
                        person.Father_First_Name = "";
                        person.Father_Last_Name = "";
                        person.First_Name = row["firstEnName"].ToString();
                        person.Last_Name = row["surEnName"].ToString();
                        person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                        person.Khmer_Last_Name = row["surKhName"].ToString();
                        if (row["gender"].ToString().Trim() == "Male")
                        {
                            person.Gender = 1;
                        }
                        else
                        {
                            person.Gender = 0;
                        }
                        person.ID_Card = row["idNumber"].ToString();
                        person.ID_Type = Convert.ToInt32(row["idtype"].ToString());
                        person.Level = Convert.ToInt32(row["level"].ToString().Trim());
                        person.Mother_First_Name = "";
                        person.Mother_Last_Name = "";
                        person.Person_ID = row["id"].ToString().Trim();
                        person.Prior_First_Name = "";
                        person.Prior_Last_Name = "";
                        person.Marital_Status = row["Marital_Status"].ToString();
                        person.Relationship = row["Relationship"].ToString();

                        if (da_application_fp6.UpdateAppInfoPersonSub(person))
                        {
                            //lblMessagePersonalInfo.Text = "Updated successfully.";
                            save_status = "";
                        }
                        else
                        {
                            //lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            save_status = "[Personal Information] was updated fail please contact your system administrator.";
                            return save_status;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.AddExceptionToLog("Error update person, funcation [SavePersonal] page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
                       // return;
                        save_status = "[Personal Information] was updated fail please contact your system administrator.";
                        return save_status;
                    }

                }
                #endregion

                #region Save into database

                else
                { //save into database
                    try
                    {
                        da_application_fp6.bl_app_info_person_sub person = new da_application_fp6.bl_app_info_person_sub();
                        person.App_Register_ID = My_View_State.App_Register_ID;
                        person.Birth_Date = Helper.FormatDateTime(row["dob"].ToString());
                        person.Country_ID = row["nationality"].ToString();
                        person.Father_First_Name = "";
                        person.Father_Last_Name = "";
                        person.First_Name = row["firstEnName"].ToString();
                        person.Last_Name = row["surEnName"].ToString();
                        person.Khmer_First_Name = row["firstKhName"].ToString(); ;
                        person.Khmer_Last_Name = row["surKhName"].ToString();
                        if (row["gender"].ToString().Trim() == "Male")
                        {
                            person.Gender = 1;
                        }
                        else
                        {
                            person.Gender = 0;
                        }
                        person.ID_Card = row["idNumber"].ToString();
                        person.ID_Type = Convert.ToInt32(row["idtype"].ToString());

                        person.Level = Convert.ToInt32(row["level"].ToString().Trim());
                        person.Mother_First_Name = "";
                        person.Mother_Last_Name = "";

                        string id = Helper.GetNewGuid("SP_Check_App_Info_Person_Sub_ID", "Person_ID");

                        person.Person_ID = id;
                        person.Prior_First_Name = "";
                        person.Prior_Last_Name = "";

                        person.Marital_Status = row["Marital_Status"].ToString();
                        person.Relationship = row["Relationship"].ToString();

                        if (da_application_fp6.InsertAppInfoPersonSub(person))
                        {
                            //lblMessagePersonalInfo.Text = "Updated successfully.";
                            save_status = "";
                        }
                        else
                        {
                            //lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                            save_status = "[Personal Information] was saved fail please contact your system administrator.";
                            return save_status;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.AddExceptionToLog("Error saved person, function [SavePersonal] page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
                        save_status = "[Personal Information] was saved fail please contact your system administrator.";
                        return save_status;
                    }

                }
                #endregion
            }
        }
        #region Delete data in database base on records deleted from gridview
        try
        {
            string personIDs = "";
            if (ViewState["personID"] != null)
            {
                personIDs = ViewState["personID"] + "";

                personIDs = personIDs.Substring(0, personIDs.Length - 1);

                char[] a = { ',' };
                string[] delete = personIDs.Split(a);
                for (int i = 0; i < delete.Length; i++)
                {
                    if (!da_application_fp6.DeleteAppInfoPersonSubByPersonIDs(delete[i]))
                    {
                       // lblMessagePersonalInfo.Text = "Updated fail please contact your system administrator.";
                        Log.AddExceptionToLog("Delete record of Personal sub fail[" + delete[i] + "] function [SavePersonal] page [application_form_fp6_rider.aspx.cs]");
                    }
                    else
                    {

                        //lblMessagePersonalInfo.Text = "Updated successfully.";
                        //ViewState["personID"] = null;
                        Log.AddExceptionToLog("Record(s) of personal sub has been deleted [" + delete[i]+"]");

                    }
                }
            }
            // reset personID
            ViewState["personID"] = null;

            if (SaveProductLife(My_View_State.App_Register_ID))
            {
                save_status = "";
            }
            else
            {
                save_status = "Premium detail was saved fail, please contact your system administrator.";
                return save_status;
            }

            #region Save Premium Pay
            DataTable tblPremiumDetail = (DataTable)ViewState["tblPremiumDetail"];
            string appID = "";
            appID = My_View_State.App_Register_ID;
            //delete old records

            foreach (DataRow rowDel in tblPremiumDetail.Rows)
            {
                int level = 0;
                level = Convert.ToInt32(rowDel["level"].ToString());
                if (level > 1 & level<=6)//delete spouse and kids
                {
                    if (da_application_fp6.DeleteAppPremPayByLevel(appID, level))
                    {

                    }
                }
            }

            foreach (DataRow row in tblPremiumDetail.Rows)
            {

                ViewState["tblPermiumDetail"] = tblPremiumDetail;
                if (SavePremPay(Helper.GetNewGuid("SP_Check_App_Prem_Pay_ID", "@App_Prem_Pay_ID"),
                                appID,
                                Helper.FormatDateTime(row["effective_date"].ToString()),
                                1,
                                Convert.ToDouble(row["premium"].ToString()),
                                Convert.ToDouble(row["original_premium"].ToString()),
                                Math.Ceiling(Convert.ToDouble(row["original_premium"].ToString())),
                               My_View_State.User_Name,
                                "",//created note
                                DateTime.Now,
                                Convert.ToInt32(row["level"].ToString())))
                {
                    save_status = "";

                }
                else
                {
                    save_status = "Premium pay was saved fail, please contact your system administrator.";
                    return save_status;
                }
            }
            #endregion End Save Premium Pay
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Delete personal data in function [SavePersonal] in page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
            save_status = "Premium pay was saved fail, please contact your system administrator.";
            return save_status;
        }
        #endregion
        //reload data
        //call load person

        LoadPerson(My_View_State.App_Register_ID);
    }
    else
    {
        //lblMessagePersonalInfo.Text = "List of Person is empty cannot be updated.";
        save_status = "List of Person is empty cannot be saved or updated.";


    }
    return save_status;
}
private string SaveJobHistory()
{
    #region old code
    string save_status = "";
    //update job history
    string appID = "";
   
    try
    {
        if (My_View_State.App_Register_ID != null)
        {

            appID = My_View_State.App_Register_ID;
        }

        if (appID.Trim() != "")
        {

            if (InsertAppJobHistorySubList(appID))
            { //success
                save_status="";

                //delete data
                if (ViewState["jobID"] != null)
                {
                    string jobId = "";
                    jobId = ViewState["jobID"] + "";
                    jobId = jobId.Substring(0, jobId.Length - 1);

                    char[] a = { ',' };
                    string[] delete = jobId.Split(a);
                    for (int i = 0; i < delete.Length; i++)
                    {
                        da_application_fp6.bl_app_job_history_sub jobhistory = new da_application_fp6.bl_app_job_history_sub();
                        jobhistory.App_Register_ID = appID;
                        jobhistory.Job_ID = delete[i];
                        if (da_application_fp6.DeleteAppJobHistoryByJobIDs(jobhistory))
                        { //inserted success
                            save_status = "" ;
                        }
                        else
                        { //inserted fail
                            save_status = "Job History was saved fail, please contact your system administrator.";
                            break;
                            
                        }
                    }

                }


            }
            else
            { //fail
                save_status = "Job History was saved fail, please contact your system administrator.";
                return save_status;

            }

            //reload data
            LoadJobHistory(appID);
        }
    }

    catch (Exception ex)
    {
        Log.AddExceptionToLog("Update Job history for policy owner was fail  in function [SaveJobHistory] in page [application_form_fp6_rider.aspx.cs]. Details: " + ex.Message);
        save_status = "Job History was saved fail, please contact your system administrator.";
        
    }
    return save_status;
    #endregion
    
}
private string SaveHealth()
{
    string save_status = "";
    try
    {
        #region Save body
        DataTable tblBody = (DataTable)ViewState["tblBody"];
        int rowCount = tblBody.Rows.Count;
        string appID = "";
        //store appid for saving answer and delete some record 
        appID = My_View_State.App_Register_ID;

        for (int i = 0; i < rowCount; i++)
        {
            var row = tblBody.Rows[i];

            // level >0 is life insured
            if (row["id"].ToString().Trim() == "")
            { //id=="" save new

                da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                body.App_Register_ID = row["app_register_id"].ToString().Trim();
                body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                body.Level = Convert.ToInt32(row["level"].ToString().Trim());
                body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                body.Reason = row["reason"].ToString().Trim();
                body.Id = Helper.GetNewGuid("SP_Check_App_Info_Body_Sub_ID", "@Id");

                if (da_application_fp6.InsertAppInfoBodySub(body))
                {
                    save_status="";
                }
                else
                {
                    save_status = "Health was saved fail, please contact your system administrator.";
                    return save_status;
                }
            }
            else
            { //id!="" update
                da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                body.App_Register_ID = row["app_register_id"].ToString().Trim();
                body.Height = Convert.ToInt32(row["height"].ToString().Trim());
                body.Weight = Convert.ToInt32(row["weight"].ToString().Trim());
                body.Level = Convert.ToInt32(row["level"].ToString().Trim());
                body.Is_Weight_Changed = Convert.ToInt32(row["is_weight_changed"].ToString().Trim());
                body.Id = row["id"].ToString().Trim();
                body.Reason = row["reason"].ToString().Trim();

                if (da_application_fp6.UpdateAppInfoBodySub(body))
                {
                    save_status = "";
                }
                else
                {
                    save_status = "Health updated fail, please contact your system administrator.";
                    return save_status;
                }

            }

        }
        #endregion End Save body

        #region Delete some record
        if (ViewState["id"] != null || ViewState["id"] + "" != "")
        {
            string id = "";
            id = ViewState["id"] + "";
            id = id.Substring(0, id.Length - 1);

            char[] a = { ',' };
            string[] delete = id.Split(a);
            for (int i = 0; i < delete.Length; i++)
            {
                da_application_fp6.bl_app_info_body_sub body = new da_application_fp6.bl_app_info_body_sub();
                body.App_Register_ID = appID;
                body.Id = delete[i];

                if (da_application_fp6.DeleteAppInfoBodySubByIDs(body))
                {
                   // lblMessageHealth.Text = "Updated successfully.";
                    //ViewState["id"] = null;
                }
                else
                {
                    //lblMessageHealth.Text = "Questions Deleted fail, please contact your system administrator.";
                }
            }


        }
        #endregion End Delete some record

        #region Save Answer

        DataTable tblPersonal = (DataTable)ViewState["tblPersonal"];
        int Count = tblPersonal.Rows.Count;

        //delete existing answer
        for (int ex = 0; ex < Count; ex++)
        {
            var row = tblPersonal.Rows[ex];
            if (row["level"].ToString().Trim() != "0")
            {
                da_application_fp6.DeleteAppAnswerByAppIDAndLevel(appID, Convert.ToInt32(row["level"].ToString()));
            }

        }

        if (InsertAppAnswerItem(appID))
        {
            save_status="";
        }
        else
        {
            save_status = "Question(s) was saved fail, please contact your system administrator.";
            return save_status;
        }

        #endregion End Save Answer


        //reload data
        LoadBody(appID);
        LoadAnswers(appID);
    }

    catch (Exception ex)
    {
        Log.AddExceptionToLog("Error function [SaveHealth] in page application_form_fp6.aspx.cs, Detail: " + ex.Message);
        save_status = "Health was saved fail, please contact your system administrator.";
       
    }
    return save_status;
}
private string UpdateApplicationDiscount(string app_register_id)
{
    string  status = "";
    try
    {

        if (da_application_fp6.UpdateApplicationDiscountTotalAmount(app_register_id))
        {
            status = "";
        }
        else
        {
            status = "update application discount total amount fail.";
        }
    }
    catch (Exception ex)
    {
        Log.AddExceptionToLog("Error function [UpdateApplicationDiscount] in page [application_form_fp6_rider.aspx.cs], Detail " + ex.Message);
        status = "update application discount total amount fail.";
    }
    return status;
}
protected void imgDelete_Click(object sender, ImageClickEventArgs e)
{
    string appID = "";
    //store appid for saving answer and delete some record 
    appID = My_View_State.App_Register_ID;
    if (appID != "")
    {
        //check underwriting 
        if (CheckUnderWrite(appID))
        {
            lblMessageApplication.Text = "Application was underwritten already, deleted fail.";
            return;
        }
        else
        {
            if (da_application_fp6.DeleteAppRiderForm(appID))
            {
                UpdateApplicationDiscount(appID);
                LoadData(appID);
                lblMessageApplication.Text = "Application was deleted successfully.";

            }
            else
            {
                lblMessageApplication.Text = "Application was deleted fail.";
            }
        }
        
    }
    else
    {
        lblMessageApplication.Text = "Application number is not exist.";
    }
    
}
private bool DeleteRiderApplication(string app_register_id)
{
    bool status = true;
    try
    {

    }
    catch (Exception ex)
    {
        Log.AddExceptionToLog("Error function [DeleteRiderApplication] in page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
        status = false;
    }
    return status;
}
private bool CheckUnderWrite(string app_register_id)
{
    bool status = true;
    try
    {
        status = da_underwriting.CheckUnderwritingRider(app_register_id);
    }
    catch (Exception ex)
    {
        Log.AddExceptionToLog("Error function [CheckUnderWrite] in page [application_form_fp6_rider.aspx.cs], Detail: " + ex.Message);
        status = false;
    }
    return status;
}
class My_View_State
{ 
    public static string User_ID{get;set;}
    public static string User_Name{get;set;}
    public static string App_Register_ID{get;set;}
    public static int Pay_Mode{get;set;}
    public static double Sum_Insured{get;set;}
    public static int Assure_Plan{get;set;}
    public static string Product_ID{get;set;}
    public static bl_policy_prem_lot prem_lot { get; set; }
    public static da_application_fpp.Cal_Age_Assure_Pay_Year Age_Assure_Pay { get; set; }
    public static string Policy_ID { get; set; }
    public static bl_policy_detail Policy { get; set; }
}
}