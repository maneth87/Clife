using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_created_policy_list : System.Web.UI.Page
{
    bl_gtli_policy_search gtli_policy = new bl_gtli_policy_search();
        
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
        gtli_policy.Company_Name = txtCompanyName.Text.Trim();
        ArrayList list_of_gtli_policy = new ArrayList();

        //Empty GridView
        gvGTLI.DataSource = list_of_gtli_policy;
        gvGTLI.DataBind();

        //Populate created list
        list_of_gtli_policy =  da_gtli_policy.GetCreatedPolicyList(gtli_policy);

        gvGTLI.DataSource = list_of_gtli_policy;
        gvGTLI.DataBind();

        lblCount.Text = "Total Rows: " + list_of_gtli_policy.Count.ToString();
    }

    //Row Index Changing
    protected void gvGTLI_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvGTLI.PageIndex = e.NewPageIndex;
    }

    //Row Index Chanted
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
            Response.Redirect("../GTLI/created_policy_detail.aspx?pid=" + premium_id);
        }        
    }

    //Image button Search Click
    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Load_Grid();
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
            if (e.Row.Cells[11].Text == "$0.00" | e.Row.Cells[10].Text == "$0")
            {
                e.Row.Cells[11].Text = " - ";
            }
        }
    }

    //Search Created List by company
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Load_Grid();
    }
}