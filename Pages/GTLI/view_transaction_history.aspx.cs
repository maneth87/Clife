using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_view_transaction_history : System.Web.UI.Page
{
    bl_gtli_policy_search policy_premium = new bl_gtli_policy_search();
    double grand_total_premium;
    List<bl_transaction_history> list_of_policy = new List<bl_transaction_history>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            ViewState["SortExpression"] = "Effective_Date";

            lblCombodianLife.Visible = false;
            lblRortTitle.Visible = false;
            lblCount.Text = "";
            btnTotalHistory.Visible = false;
        }
    }

  
    //Load Grid view
    protected void Load_Grid()
    {
        //clear gridview
        ArrayList empty_list = new ArrayList();
        gvHistory.DataSource = empty_list;
        gvHistory.DataBind();

        //hide button view total history detail
        btnTotalHistory.Visible = false;

        string company_id = "0";
        //check company name
        if (da_gtli_company.CheckCompany(txtCompany.Text.Trim()))
        {
            //found company

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            if (!string.IsNullOrEmpty(txtFrom.Text.Trim()))
            {
                policy_premium.From_Date = Convert.ToDateTime(txtFrom.Text, dtfi);
            }
            else
            {
                policy_premium.From_Date = Convert.ToDateTime("01/01/1900", dtfi);
            }
           
            if (!string.IsNullOrEmpty(txtTo.Text.Trim()))
            {
                policy_premium.To_Date = Convert.ToDateTime(txtTo.Text, dtfi);
            }else{
                policy_premium.To_Date = Convert.ToDateTime("01/01/1900", dtfi);
            }
           
            //get company_id by company_name
            company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompany.Text.Trim());

            if (da_gtli_policy.CheckCompanyId(company_id))
            {
                string policy_number = da_gtli_policy.GetPolicyNumberByCompanyID(company_id);
                              
                //list of all policy for this company
                list_of_policy = da_gtli_policy.GetPolicyHistoryByPolicyNumber(policy_number, policy_premium.SortColum, policy_premium.SortDir, policy_premium.From_Date, policy_premium.To_Date);


                if (policy_premium.SortColum == "Transaction_Staff_Number")
                {
                    if (policy_premium.SortDir == "ASC")
                    {
                        list_of_policy.Sort(delegate(bl_transaction_history c1, bl_transaction_history c2)
                        {
                            return c1.Transaction_Staff_Number.CompareTo(c2.Transaction_Staff_Number);
                        });                       
                    }
                    else
                    {
                        list_of_policy.Sort(delegate(bl_transaction_history c1, bl_transaction_history c2)
                        {
                            return c2.Transaction_Staff_Number.CompareTo(c1.Transaction_Staff_Number);
                        });
                        
                    }                 
                    
                }
                else if (policy_premium.SortColum == "Sum_Insured")
                {
                    if (policy_premium.SortDir == "ASC")
                    {
                        list_of_policy.Sort(delegate(bl_transaction_history c1, bl_transaction_history c2)
                        {
                            return c1.Sum_Insured.CompareTo(c2.Sum_Insured);
                        });  
                        
                    }
                    else
                    {
                        list_of_policy.Sort(delegate(bl_transaction_history c1, bl_transaction_history c2)
                        {
                            return c2.Sum_Insured.CompareTo(c1.Sum_Insured);
                        });
                       
                    }   
                }
                else if (policy_premium.SortColum == "Total_Premium")
                {
                    if (policy_premium.SortDir == "ASC")
                    {
                        list_of_policy.Sort(delegate(bl_transaction_history c1, bl_transaction_history c2)
                        {
                            return c2.Total_Premium.CompareTo(c1.Total_Premium);
                        });
                       
                    }
                    else
                    {
                        list_of_policy.Sort(delegate(bl_transaction_history c1, bl_transaction_history c2)
                        {
                            return c1.Total_Premium.CompareTo(c2.Total_Premium);
                        });
                       
                    }   
                }
                else if (policy_premium.SortColum == "Effective_Date")
                {
                    if (policy_premium.SortDir == "ASC")
                    {
                        list_of_policy = list_of_policy.OrderBy(item => item.Effective_Date).ToList();
                    }
                    else
                    {
                        list_of_policy = list_of_policy.OrderByDescending(item => item.Effective_Date).ToList();
                    }   
                }
                else if (policy_premium.SortColum == "Expiry_Date")
                {
                    if (policy_premium.SortDir == "ASC")
                    {
                        list_of_policy = list_of_policy.OrderBy(item => item.Expiry_Date).ToList();
                    }
                    else
                    {
                        list_of_policy = list_of_policy.OrderByDescending(item => item.Expiry_Date).ToList();
                    }   
                }
                else
                {

                    list_of_policy = list_of_policy.OrderBy(item => item.Effective_Date).ToList();
                   
                }

                if (list_of_policy.Count > 0)
                {
                    //bind to gridview
                    gvHistory.DataSource = list_of_policy;
                    gvHistory.DataBind();
                    lblCombodianLife.Visible = true;
                    lblCount.Text = "Total Record: " + list_of_policy.Count;
                    lblRortTitle.Visible = true;


                    //show button view total history detail
                    btnTotalHistory.Visible = true;

                }
                else
                {
                    lblCombodianLife.Visible = false;
                    lblRortTitle.Visible = false;
                    lblCount.Text = "";
                }
            }

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

    //Get sort direction
    private string GetSortDirection(string column)
    {

        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;

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

    //Gridview page index changing
    protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHistory.PageIndex = e.NewPageIndex;
    }

    //Gridview page index changed
    protected void gvHistory_PageIndexChanged(object sender, EventArgs e)
    {
        Load_Grid();
    }

    //Gridview sorting

    protected void gvHistory_Sorting(object sender, GridViewSortEventArgs e)
    {
        policy_premium.SortColum = e.SortExpression;
        policy_premium.SortDir = GetSortDirection(e.SortExpression);

        Load_Grid();
    }

    //Gridview row data bound
    protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton ViewAddNewDetailLink = (LinkButton)e.Row.FindControl("ViewAddNewDetailLink");
            LinkButton ViewResignDetailLink = (LinkButton)e.Row.FindControl("ViewResignDetailLink");
            LinkButton ViewCreatePolicyLink = (LinkButton)e.Row.FindControl("ViewCreatePolicyLink");
            switch (e.Row.Cells[6].Text)
            {
                case "1":
                    e.Row.Cells[6].Text = "Create Policy";
                    ViewCreatePolicyLink.Visible = true;
                    ViewAddNewDetailLink.Visible = false;
                    ViewResignDetailLink.Visible = false;
                    break;
                case "2":
                    e.Row.Cells[6].Text = "Add Member";
                    ViewCreatePolicyLink.Visible = false;
                    ViewAddNewDetailLink.Visible = true;
                    ViewResignDetailLink.Visible = false;
                    break;
                case "3":
                    e.Row.Cells[6].Text = "Resign Member";
                    e.Row.Cells[11].Text = "(" + e.Row.Cells[11].Text + ")";
                    ViewCreatePolicyLink.Visible = false;
                    ViewAddNewDetailLink.Visible = false;
                    ViewResignDetailLink.Visible = true;
                    break;
            }

            if (e.Row.Cells[7].Text == "$0")
            {
                e.Row.Cells[7].Text = " - ";
            }

            if (e.Row.Cells[2].Text == " ")
            {
                e.Row.Cells[2].Text = " - ";
            }

            if (e.Row.Cells[8].Text == "$0" | e.Row.Cells[8].Text == "$0.00")
            {
                e.Row.Cells[8].Text = " - ";
            }
            if (e.Row.Cells[9].Text == "$0" | e.Row.Cells[9].Text == "$0.00")
            {
                e.Row.Cells[9].Text = " - ";
            }
            if (e.Row.Cells[10].Text == "$0" | e.Row.Cells[10].Text == "$0.00")
            {
                e.Row.Cells[10].Text = " - ";
            }
            if (e.Row.Cells[11].Text == "$0" | e.Row.Cells[11].Text == "$0.00")
            {
                e.Row.Cells[11].Text = " - ";
            }

        }
    }

    //Gridview row command
    protected void gvHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddNewDetailSale")
        {
            //Get premium ID
            string premium_id = Convert.ToString(e.CommandArgument);
            Response.Redirect("../GTLI/add_new_transaction_detail.aspx?pid=" + premium_id);
        }

        if (e.CommandName == "CreatePolicyDetailSale")
        {
            //Get premium ID
            string premium_id = Convert.ToString(e.CommandArgument);
            Response.Redirect("../GTLI/created_policy_detail.aspx?pid=" + premium_id);
        }

        if (e.CommandName == "ResignDetailSale")
        {
            //Get premium ID
            string premium_id = Convert.ToString(e.CommandArgument);
            Response.Redirect("../GTLI/resign_member_transaction_detail.aspx?pid=" + premium_id);
        }
    }

    //view total history detail click
    protected void btnTotalHistory_Click(object sender, EventArgs e)
    {
        string id_list = "";

        //get list of policy
        string company_id = "0";
        //check company name
        if (da_gtli_company.CheckCompany(txtCompany.Text.Trim()))
        {
            //found company

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            if (!string.IsNullOrEmpty(txtFrom.Text.Trim()))
            {
                policy_premium.From_Date = Convert.ToDateTime(txtFrom.Text, dtfi);
            }
            else
            {
                policy_premium.From_Date = Convert.ToDateTime("01/01/1900", dtfi);
            }

            if (!string.IsNullOrEmpty(txtTo.Text.Trim()))
            {
                policy_premium.To_Date = Convert.ToDateTime(txtTo.Text, dtfi);
            }
            else
            {
                policy_premium.To_Date = Convert.ToDateTime("01/01/1900", dtfi);
            }
          
            //get company_id by company_name
            company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompany.Text.Trim());

            if (da_gtli_policy.CheckCompanyId(company_id))
            {
                string policy_number = da_gtli_policy.GetPolicyNumberByCompanyID(company_id);
                               
                //list of all policy for this company
                list_of_policy = da_gtli_policy.GetPolicyHistoryByPolicyNumber(policy_number, policy_premium.SortColum, policy_premium.SortDir, policy_premium.From_Date, policy_premium.To_Date);

                //loop through list of policy
                for (int i = 0; i <= list_of_policy.Count - 1; i++)
                {
                    bl_transaction_history my_policy = (bl_transaction_history) list_of_policy[i];

                    if (i == list_of_policy.Count - 1)
                    {
                        id_list += my_policy.GTLI_Premium_ID;
                    }
                    else
                    {
                        id_list += my_policy.GTLI_Premium_ID + ",";
                    }
                }

                Response.Redirect("../GTLI/transaction_history_total_detail.aspx?pid=" + id_list);

            }
        }
    }

    //Search Transaction History
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Load_Grid();
    }
}