using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_add_new_transaction_list : System.Web.UI.Page
{
    bl_gtli_policy_search gtli_policy = new bl_gtli_policy_search();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //get autocomplete for company name
    [WebMethod()]
    public static string[] GetCompanyName()
    {
        ArrayList list_of_company = new ArrayList();
        list_of_company = da_gtli_company.GetListOfCompanyName();

        string[] companyArray = new string[list_of_company.Count];

        for (int i = 0; i <= list_of_company.Count - 1; i++)
        {
            companyArray[i] = list_of_company[i].ToString();
        }

        return companyArray;
    }


    //Load Grid
    protected void Load_Grid()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        if (!string.IsNullOrEmpty(txtFrom.Text.Trim()))
        {
            gtli_policy.From_Date = Convert.ToDateTime(txtFrom.Text, dtfi);
        }
        else
        {
            gtli_policy.From_Date = Convert.ToDateTime("01/01/1900", dtfi);
        }

        if (!string.IsNullOrEmpty(txtTo.Text.Trim()))
        {
            gtli_policy.To_Date = Convert.ToDateTime(txtTo.Text, dtfi);
        }
        else
        {
            gtli_policy.To_Date = Convert.ToDateTime("01/01/1900", dtfi);
        }

        gtli_policy.Date_Type = Convert.ToInt32(ddlDateType.SelectedValue);

        gtli_policy.Company_Name = txtCompanyName.Text.Trim();

        btnTotalAddNew.Visible = false;
        ArrayList list_of_gtli_policy = new ArrayList();

        //Empty GridView
        gvGTLI.DataSource = list_of_gtli_policy;
        gvGTLI.DataBind();

        //Populate Added List
        list_of_gtli_policy = da_gtli_policy.GetAddedPolicyList(gtli_policy);

        gvGTLI.DataSource = list_of_gtli_policy;
        gvGTLI.DataBind();

        if (da_gtli_company.CheckCompany(txtCompanyName.Text.Trim()))
        {
            //found company          
            
            if (list_of_gtli_policy.Count > 0)
            {
                //show button view total history detail
                btnTotalAddNew.Visible = true;

                bl_gtli_policy_search policy_search = new bl_gtli_policy_search();
                policy_search = (bl_gtli_policy_search)list_of_gtli_policy[0];
                string policy_id = policy_search.GTLI_Policy_ID;

                 for(int i = 1; i <list_of_gtli_policy.Count; i++){
                     bl_gtli_policy_search policy_search2 = new bl_gtli_policy_search();
                     policy_search2 = (bl_gtli_policy_search) list_of_gtli_policy[i];

                     if (policy_id != policy_search2.GTLI_Policy_ID)
                     {
                         btnTotalAddNew.Visible = false; //Don't show total add detail button when more than one policy id of same company
                     }
                 }

            }
        }

        lblCount.Text = "Total Rows: " + list_of_gtli_policy.Count.ToString();
    }


    //Row Index Changing
    protected void gvGTLI_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvGTLI.PageIndex = e.NewPageIndex;
    }

    //Row Index Changed
    protected void gvGTLI_PageIndexChanged(object sender, EventArgs e)
    {
        Load_Grid();
    }

    //Row Command
    protected void gvGTLI_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Get premium ID
        string premium_id = Convert.ToString(e.CommandArgument);

        if (e.CommandName == "DetailSale")
        {
            Response.Redirect("../GTLI/add_new_transaction_detail.aspx?pid=" + premium_id);
        }
        
    }

    //Grid view row data bound
    protected void gvGTLI_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdftransaction_type = (HiddenField)e.Row.FindControl("hdfTransactionType");
            LinkButton ViewDetailLink = (LinkButton)e.Row.FindControl("ViewDetailLink");
            LinkButton ViewNewPlanLink = (LinkButton)e.Row.FindControl("ViewNewPlanLink");
            LinkButton ViewAddMemberLink = (LinkButton)e.Row.FindControl("ViewAddMemberLink");
            LinkButton ViewRemoveLink = (LinkButton)e.Row.FindControl("ViewRemoveLink");
            Label lblExpiryDate = (Label)e.Row.FindControl("lblExpiryDate");

            if (e.Row.Cells[7].Text == "$0.00" | e.Row.Cells[7].Text == "$0")
            {
                e.Row.Cells[7].Text = " - ";
            }

            if (e.Row.Cells[8].Text == "$0.00" | e.Row.Cells[8].Text == "$0")
            {
                e.Row.Cells[8].Text = " - ";

            }

            if (e.Row.Cells[9].Text == "$0.00" | e.Row.Cells[9].Text == "$0")
            {
                e.Row.Cells[9].Text = " - ";

            }
            if (e.Row.Cells[10].Text == "$0.00" | e.Row.Cells[10].Text == "$0")
            {
                e.Row.Cells[10].Text = " - ";

            }
            if (e.Row.Cells[11].Text == "$0.00" | e.Row.Cells[11].Text == "$0")
            {
                e.Row.Cells[11].Text = " - ";

            }
          
        }
    }

    //button view added member total click
    protected void btnTotalAddNew_Click(object sender, EventArgs e)
    {
        string id_list = "";

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        if (!string.IsNullOrEmpty(txtFrom.Text.Trim()))
        {
            gtli_policy.From_Date = Convert.ToDateTime(txtFrom.Text, dtfi);
        }
        else
        {
            gtli_policy.From_Date = Convert.ToDateTime("01/01/1900", dtfi);
        }

        if (!string.IsNullOrEmpty(txtTo.Text.Trim()))
        {
            gtli_policy.To_Date = Convert.ToDateTime(txtTo.Text, dtfi);
        }
        else
        {
            gtli_policy.To_Date = Convert.ToDateTime("01/01/1900", dtfi);
        }
        gtli_policy.Date_Type = Convert.ToInt32(ddlDateType.SelectedValue);

        gtli_policy.Company_Name = txtCompanyName.Text.Trim();

        btnTotalAddNew.Visible = false;
        ArrayList list_of_gtli_policy = da_gtli_policy.GetAddedPolicyList(gtli_policy);

        //loop through list of policy
        for (int i = 0; i <= list_of_gtli_policy.Count - 1; i++)
        {
            bl_gtli_policy_search my_policy = (bl_gtli_policy_search)list_of_gtli_policy[i];
            if (i == list_of_gtli_policy.Count - 1)
            {
                id_list += my_policy.GTLI_Premium_ID;
            }
            else
            {
                id_list += my_policy.GTLI_Premium_ID + ",";
            }
        }

        Response.Redirect("../GTLI/total_added_member_detail.aspx?pid=" + id_list);
    }

    //Search Added List by company, type and date
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Load_Grid();
    }
}