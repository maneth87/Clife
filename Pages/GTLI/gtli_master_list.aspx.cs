using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;


public partial class Pages_GTLI_gtli_master_list : System.Web.UI.Page
{
    bl_gtli_master_list gtli_master = new bl_gtli_master_list();
    List<bl_gtli_master_list> master_list = new List<bl_gtli_master_list>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //gtli_master.SortDir = "ASC";
            //gtli_master.SortColum = "Company_Name";

            //assign sort expression for issue date
            gtli_master.SortDir = "DESC";
            gtli_master.SortColum = "Issue_Date";

            Load_Grid();
        }
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
        gtli_master.GTLI_Company = txtCompanyName.Text.Trim();
       
        //Search all company
        if (string.IsNullOrEmpty(gtli_master.GTLI_Company))
        {
            //get list of company_id in gtli_policy table
            ArrayList list_of_company_id = new ArrayList();
            list_of_company_id = da_gtli_policy.GetListOfCompanyID();

            for (int i = 0; i <= list_of_company_id.Count - 1; i++)
            {
                if (da_gtli_policy.CheckCompanyId(list_of_company_id[i].ToString()))
                {
                    //found transaction 
                    ArrayList master_list_by_company = new ArrayList();
                    master_list_by_company = da_gtli_policy.GetMasterListByCompanyID(list_of_company_id[i].ToString());

                    for (int j = 0; j <= master_list_by_company.Count - 1; j++)
                    {
                        master_list.Add((bl_gtli_master_list) master_list_by_company[j]);
                    }                    
                    
                }
            }
        }
        else //search my specific company
        {
            //check company
            string company_id = da_gtli_company.GetCompanyIDByCompanyName(gtli_master.GTLI_Company);
           
            if (da_gtli_policy.CheckCompanyId(company_id))
            {
                //found transaction              
                ArrayList master_list_by_company = new ArrayList();
                master_list_by_company = da_gtli_policy.GetMasterListByCompanyID(company_id);

                for (int j = 0; j <= master_list_by_company.Count - 1; j++)
                {
                    master_list.Add((bl_gtli_master_list)master_list_by_company[j]);
                }                    
            }
        }


        //Sorting
        if (!string.IsNullOrEmpty(gtli_master.SortColum))
        {
            //if (gtli_master.SortDir == "ASC")
            //{
            //    master_list = master_list.OrderBy(a => a.GTLI_Company).ToList();
            //}
            //else
            //{
            //    master_list = master_list.OrderByDescending(a => a.GTLI_Company).ToList();
            //}

            //first sort by issue date
            if (gtli_master.SortDir == "ASC")
            {
                master_list = master_list.OrderBy(a => a.Issue_Date).ToList();
            }
            else
            {
                master_list = master_list.OrderByDescending(a => a.Issue_Date).ToList();
            }
        }

        gvGTLI.DataSource = master_list;
        gvGTLI.DataBind();

        lblCount.Text = "Total Rows: " + master_list.Count.ToString();
    }       

 
    //Row Index Changed
    protected void gvGTLI_PageIndexChanged(object sender, EventArgs e)
    {
        Load_Grid();
    }

    //Row Index Changing
    protected void gvGTLI_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvGTLI.PageIndex = e.NewPageIndex;
    }

    //Row Command
    protected void gvGTLI_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DetailSale")
        {
            //Get policy ID
            string policy_id = Convert.ToString(e.CommandArgument);
            Response.Redirect("../GTLI/gtli_master_detail.aspx?pid=" + policy_id);
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

            if (e.Row.Cells[6].Text == "$0.00" | e.Row.Cells[6].Text == "$0")
            {
                e.Row.Cells[6].Text = " - ";

            }

            if (e.Row.Cells[5].Text == "$0.00" | e.Row.Cells[5].Text == "$0")
            {
                e.Row.Cells[5].Text = " - ";

            }

            if (e.Row.Cells[4].Text == "$0.00" | e.Row.Cells[4].Text == "$0")
            {
                e.Row.Cells[4].Text = " - ";
                
            }

            if (e.Row.Cells[7].Text == "$0.00" | e.Row.Cells[4].Text == "$0")
            {
                e.Row.Cells[7].Text = " - ";

            }

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

        }
    }

    //Sorting
    protected void gvGTLI_Sorting(object sender, GridViewSortEventArgs e)
    {
        gtli_master.SortColum = e.SortExpression;
        gtli_master.SortDir = GetSortDirection(e.SortExpression);

        Load_Grid();
    }

    //Get sort direction
    private string GetSortDirection(string column)
    {

        // By default, set the sort direction to ascending.
        dynamic sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        dynamic sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                dynamic lastDirection = ViewState["SortDirection"] as string;

                if (lastDirection != null && lastDirection == "ASC")
                {
                    sortDirection = "DESC";

                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        return sortDirection;

    }

    //Search Master List by company
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Load_Grid();
    }
}