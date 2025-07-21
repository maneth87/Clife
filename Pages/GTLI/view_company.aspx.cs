using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_view_company : System.Web.UI.Page
{

    bl_gtli_company blcompany = new bl_gtli_company();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {            
            ViewState["SortDirection"] = "ASC";
            ViewState["SortExpression"] = "Company_Name";
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


    //Function to load grid view
    protected void Load_Grid()
    {

        if (ddlBusiness.SelectedValue == "0")
        {
            blcompany.Type_Of_Business = "";
        }
        else
        {
            blcompany.Type_Of_Business = ddlBusiness.SelectedItem.Text;

        }

        if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
        {
            blcompany.Company_Name = txtCompany.Text.Trim();
        }
        else
        {
            blcompany.Company_Name = "";
        }

        List<bl_gtli_company> company_list = new List<bl_gtli_company>();
      
        //Empty GridView
        gvCompany.DataSource = company_list;
        gvCompany.DataBind();

        //Populate Company List
        company_list = da_gtli_company.GetCompanyListSorting(blcompany);

        if (blcompany.SortColum == "Type_Of_Business")
        {
            if (blcompany.SortDir == "ASC")
            {
                company_list.Sort(delegate(bl_gtli_company c1, bl_gtli_company c2)
                {
                    return c1.Type_Of_Business.CompareTo(c2.Type_Of_Business);
                });
               
            }
            else
            {
                company_list.Sort(delegate(bl_gtli_company c1, bl_gtli_company c2)
                {
                    return c2.Type_Of_Business.CompareTo(c1.Type_Of_Business);
                });
            }
        }

        if (blcompany.SortColum == "Company_Name")
        {
            if (blcompany.SortDir == "ASC")
            {
                company_list.Sort(delegate(bl_gtli_company c1, bl_gtli_company c2)
                {
                    return c1.Company_Name.CompareTo(c2.Company_Name);
                });
                
            }
            else
            {
                company_list.Sort(delegate(bl_gtli_company c1, bl_gtli_company c2)
                {
                    return c2.Company_Name.CompareTo(c1.Company_Name);
                });
            }
        }

        if (blcompany.SortColum == "Latest_Contact_Name")
        {
            if (blcompany.SortDir == "ASC")
            {
                company_list.Sort(delegate(bl_gtli_company c1, bl_gtli_company c2)
                {
                    return c1.Latest_Contact_Name.CompareTo(c2.Latest_Contact_Name);
                });
            }
            else
            {
                company_list.Sort(delegate(bl_gtli_company c1, bl_gtli_company c2)
                {
                    return c2.Latest_Contact_Name.CompareTo(c1.Latest_Contact_Name);
                });
               
            }
        }

        gvCompany.DataSource = company_list;
        gvCompany.DataBind();

        lblCount.Text = "Total Companies: " + gvCompany.Rows.Count.ToString();

        if (gvCompany.Rows.Count > 0)
        {
            lblCompanyName.Visible = true;
            lblTitle.Visible = true;
        }
        else
        {
            lblCompanyName.Visible = false;
            lblTitle.Visible = false;
        }

    }


    //Gridview page index changing
    protected void gvCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCompany.PageIndex = e.NewPageIndex;
    }

    //Gridview index changed
    protected void gvCompany_PageIndexChanged(object sender, EventArgs e)
    {
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

    //Gridview sorting
    protected void gvCompany_Sorting(object sender, GridViewSortEventArgs e)
    {
        blcompany.SortColum = e.SortExpression;
        blcompany.SortDir = GetSortDirection(e.SortExpression);

        Load_Grid();
    }

    //Search Company List
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Load_Grid();
    }
}