using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Business_customer_list : System.Web.UI.Page
{
    DataTable tbl_customer;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            ViewState["sortdir"] = "desc";
            ViewState["sortcmd"] = "created_datetime";
        }
        loadData("", "", "", "");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       // ScriptManager.RegisterStartupScript(up, up.GetType(), "alert", "alert('Hello world.');$('#modal_search_customer').modal('hide');", true);
        loadData(txtSearchCustName.Text.Trim(), txtSearchIDCard.Text.Trim(),"", txtSearchCustNo.Text.Trim());
    }

    void loadData(string customer_name, string id_card, string customer_id, string customer_number)
    {
        tbl_customer = DataSetGenerator.Get_Data_Soure("SP_GET_CUSTOMER_ALL_TYPE1", new string[,] { {"@FULL_NAME",customer_name },
                                                                                                    {"@ID_CARD", id_card}, 
                                                                                                    {"@CUSTOMER_ID", customer_id}, 
                                                                                                    {"@CUSTOMER_NUMBER", customer_number} });
        gvCustomer.DataSource = tbl_customer;
        gvCustomer.DataBind();
    }
    protected void gvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCustomer.PageIndex = e.NewPageIndex;
        gvCustomer.DataSource = tbl_customer;
        gvCustomer.DataBind();
    }
    protected void gvCustomer_Sorting(object sender, GridViewSortEventArgs e)
    {
        
        if(ViewState["sortdir"].ToString().ToLower() == "asc")
        {
            ViewState["sortdir"] = "desc";
        }
                
            else
        {
               ViewState["sortdir"] = "asc";
        }
        ViewState["sortcmd"] = e.SortExpression + " " + ViewState["sortdir"];

        tbl_customer.DefaultView.Sort = ViewState["sortcmd"] + "";
        DataTable tbl_out  = tbl_customer.DefaultView.ToTable();
        gvCustomer.DataSource = tbl_out;
        gvCustomer.DataBind();
     
    }
    protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "cmdLink")
        {
            GridViewRow grow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int index = grow.RowIndex;
            
            Label lbl = new Label();
            lbl.Text = "";
            lbl.Text = ((Label)gvCustomer.Rows[index].FindControl("lblCustID")).Text;
            lblMessage.InnerText = index + "  " + lbl.Text;
            //Response.Redirect("customers.aspx?cust_id=" + lbl.Text);
           
        }
    }
    protected string GetGenderText(int gender)
    {
        string str_gender = "";
        if (gender == 1) {
            str_gender= "Male";
        }
        else if (gender == 0)
        {
            str_gender= "Female";
        }
        else 
        {
            str_gender = "-";
        }
        return str_gender;
    }
  
}