using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

public partial class Pages_Business_policy_number_reserve : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser user = Membership.GetUser();
            string user_id = user.ProviderUserKey.ToString();
            string username = user.UserName;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        MembershipUser user = Membership.GetUser();
        string user_id = user.ProviderUserKey.ToString();
        string username = user.UserName;
        string application_number = ""; 

        try
        {
            bl_app_reserve_policy reserve_policy = new bl_app_reserve_policy();

            application_number = txtApplicationNumber.Text.Trim();
            application_number = da_application.GetCompleteAppNumber(application_number);

            reserve_policy.App_Number = application_number;
            reserve_policy.Customer_ID = txtCustomerID.Text.Trim();
            reserve_policy.Policy_Number = txtPolicyNumber.Text.Trim();
            reserve_policy.Created_On = DateTime.Now;
            reserve_policy.Created_By = username;

            if (reserve_policy.App_Number != "" && reserve_policy.Customer_ID != "" && reserve_policy.Policy_Number != "" )
            {
                bool existInApplication = da_policy.CheckExistingAppNumber(reserve_policy.App_Number); 
                bool existInMasterPolicy = da_policy.CheckExistingGroupMasterPolicy(reserve_policy.Policy_Number);
                bool existInCustomer = da_policy.CheckExistingCustomerID(reserve_policy.Customer_ID); 
                bool existInReserved = da_policy.CheckExistingReservePolicy(reserve_policy.Policy_Number); 
                  
                //Check in Master Policy
                if (existInApplication != true && existInMasterPolicy != true && existInCustomer != true && existInReserved != true)
                {
                    //Insertion Reserve Policy
                    if (da_application.InsertReservePolicy(reserve_policy))
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + "Created Reserve Policy Number Successfully." + "', event,'success', 'Successfully');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + "Create Reserved Policy Number Failed. Please check it again!" + "', event,'warning', 'Failed');", true);
                }
                else
                {
                    string my_message = "";

                    string[, ,] exists = new string[,,] { };
                    List<string[, ,]> exist_list = new List<string[, ,]>();

                    if (existInApplication != false)
                    {
                        exists = new string[,,] { { { "", application_number } } };
                        exist_list.Add(exists);
                    }
                    if (existInMasterPolicy != false || existInReserved != false)
                    {
                        exists = new string[,,] { { { "", reserve_policy.Policy_Number } } };
                        exist_list.Add(exists);
                    }
                    if (existInCustomer != false )
                    {
                        exists = new string[,,] { { { "", reserve_policy.Customer_ID } } };
                        exist_list.Add(exists);
                    }

                    try
                    {
                        foreach (string[, ,] text in exist_list)
                        {
                            for (int index = 0; index <= text.GetUpperBound(0); index++)
                            {
                                my_message += my_message.Trim() == "" ? "[" + text[index, 0, 1] + "]" : "," + "[" + text[index, 0, 1] + "]";
                            }
                        }

                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + "#Error. Please contact administrator." + "', event,'warning', 'Failed');", true);
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + my_message + " is already exist. Please check it again." + "', event,'warning', 'Failed');", true);
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('All input is required! Please check it again.')", true);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + "#Error. Please contact administrator." + "', event,'warning', 'Failed');", true);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        loadDataView(txtApplicationNumber.Text.Trim(), txtCustomerID.Text.Trim(), txtPolicyNumber.Text.Trim());
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtApplicationNumber.Text = "";
        txtCustomerID.Text = "";
        txtPolicyNumber.Text = "";
        lblCount.Text = "";
    }
    protected void gvCL24_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    void loadDataView(string app_number, string customer_id, string policy_number) 
    {
        string[,,] condition = new string[,,] { };
        List<string[, ,]> condition_list = new List<string[, ,]>();
        string application_number = "";
        if (app_number.Trim() != "")
        {
            application_number = da_application.GetCompleteAppNumber(app_number.Trim());
            condition = new string[,,] { { { "APP_NUMBER", "=", application_number } } };
            condition_list.Add(condition);
        }
        if (customer_id.Trim() != "")
        {
            condition = new string[,,] { { { "CUSTOMER_ID", "=", customer_id.Trim() } } };
            condition_list.Add(condition);
        }
        if (policy_number.Trim() != "")
        {
            condition = new string[,,] { { { "Policy_Number", "=", policy_number.Trim() } } };
            condition_list.Add(condition);
        }

        //Get Data By Condition
        DataTable tbl = da_application.GetReservedDataList(condition_list);

        if (tbl.Rows.Count > 0)
        {
            string sortEx = "";
            string sortCol = "";
            sortEx = Session["SORTEX"] + "";
            sortCol = Session["SORTCOL"] + "";
            DataView dview = new DataView(tbl);
            if (sortEx != "")
            {
                dview.Sort = sortCol + " " + sortEx;
            }

            gvCL24.DataSource = dview;
            gvCL24.DataBind();
            ViewState["DATA"] = tbl;
            lblCount.Text = (tbl.Rows.Count).ToString() + " Record (s)";

        }
        else
        {
            gvCL24.DataSource = null;
            gvCL24.DataBind();
            ViewState["DATA"] = null;
            lblCount.Text = "No Record Found";

        }

    }

    protected void Edit_Click(object sender, EventArgs e)
    {

    }

    protected void Del_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton ReservePolicyID = sender as LinkButton;
            if (ReservePolicyID.CommandArgument != "")
            {
                string id = ReservePolicyID.CommandArgument;
                if (da_application.DeleteReservePolicy(id))
                {
                    loadDataView(txtApplicationNumber.Text.Trim(), txtCustomerID.Text.Trim(), txtPolicyNumber.Text.Trim());
                }
            }
        }
        catch (Exception)
        {
        }

    }

}