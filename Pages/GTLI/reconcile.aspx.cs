using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_reconcile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           
        }
    }

    //Load Grid
    protected void Load_Grid()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        List<bl_gtli_premium> list_of_gtli_premium = new List<bl_gtli_premium>();

        //Empty GridView
        gvGTLIPolicy.DataSource = list_of_gtli_premium;
        gvGTLIPolicy.DataBind();
     
        string company_id = da_gtli_company.GetCompanyIDByCompanyName(txtCompanyName.Text.Trim());
        DateTime to_date = Convert.ToDateTime(txtToDate.Text, dtfi);

            //list_of_gtli_premium = da_gtli_premium.GetGTLIPremiumListByStatusAndCompanyID(1, company_id, to_date, ddlType.SelectedValue);
        list_of_gtli_premium = da_gtli_premium.GetGTLIPremiumListByStatusAndCompanyID(1, company_id, to_date, "0"); //2=add, 3=resigned , i put "0" because transaction type will compute by store procedure
        

        gvGTLIPolicy.DataSource = list_of_gtli_premium;
        gvGTLIPolicy.DataBind();


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

    //Load Grid where policy has status == 1 in prem pay or prem return

    //Grid Row Data Bound  
    protected void gvGTLIPolicy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Cell Life Premium
            if (e.Row.Cells[6].Text.Trim() == "$0.00")
            {
                e.Row.Cells[6].Text = "-";
            }

            //Cell 100Plus Premium
            if (e.Row.Cells[7].Text.Trim() == "$0.00")
            {
                e.Row.Cells[7].Text = "-";
            }

            //Cell TPD Premium
            if (e.Row.Cells[8].Text.Trim() == "$0.00")
            {
                e.Row.Cells[8].Text = "-";
            }

            //Cell DHC Premium
            if (e.Row.Cells[9].Text.Trim() == "$0.00")
            {
                e.Row.Cells[9].Text = "-";
            }

          
            //Cell Transaction Type
            if (e.Row.Cells[12].Text.Trim() == "1")
            {
                e.Row.Cells[11].Text = "Add Plan";
            }
            else if (e.Row.Cells[12].Text.Trim() == "2")
            {
                e.Row.Cells[11].Text = "Add Member";
            }
            else if (e.Row.Cells[12].Text.Trim() == "3")
            {
                e.Row.Cells[11].Text = "Resign Member";
                e.Row.Cells[5].Text = "(" + e.Row.Cells[5].Text + ")";
                e.Row.Cells[6].Text = "(" + e.Row.Cells[6].Text + ")";
            }
        }
    }

    protected void gvGTLIPolicy_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Get premium ID
        string premium_id = Convert.ToString(e.CommandArgument);

        if (e.CommandName == "DetailSale")
        {

            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            HiddenField hdfTransactionType = row.FindControl("hdfTransactionType") as HiddenField;

            switch (hdfTransactionType.Value)
            {
                case "1":
                case "2":
                    Response.Redirect("../GTLI/add_new_transaction_detail.aspx?pid=" + premium_id);
                    break;
                case "3":
                    Response.Redirect("../GTLI/resign_member_transaction_detail.aspx?pid=" + premium_id);
                    break;
            }

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
        Load_Grid();
    }

   
    protected void btnReconcile_Click(object sender, EventArgs e)
    {
       
        string premium_id_list = "";
        foreach (GridViewRow row in gvGTLIPolicy.Rows)
         {
             if (row.RowType == DataControlRowType.DataRow)
             {                
                 CheckBox chk = row.Cells[0].Controls[1] as CheckBox;
                 string premium_id = gvGTLIPolicy.DataKeys[row.RowIndex]["GTLI_Premium_ID"].ToString();
                
                 HiddenField hdfTransactionType = row.FindControl("hdfTransactionType") as HiddenField;
                 if (chk.Checked)              
                 {
                     //Get payment code                     
                     bl_payment_code payment_code = new bl_payment_code();

                     string policy_id = da_gtli_policy.GetGTLIPolicyIDByPremiumID(premium_id);

                     string prem_pay_payment_code = da_gtli_policy_prem_pay.GetPaymentCodeByPolicyID(policy_id);
                     string prem_return_payment_code = da_gtli_policy_prem_return.GetPaymentCodeByPolicyID(policy_id);

                     if (prem_pay_payment_code == "" && prem_return_payment_code == "")
                     {
                         payment_code.payment_code = da_payment_code.GetNewPaymentCode();
                     }
                     else
                     {
                         payment_code.payment_code = prem_pay_payment_code;
                     }
                     
                     payment_code.quotation_number = "";
                     payment_code.quotation_date = DateTime.Now;

                     if (hdfTransactionType.Value != "3")
                     {
                        
                         if (da_gtli_policy_prem_pay.UpdateGTLIPolicyPremPayStatus(2, payment_code.payment_code, premium_id))
                         {
                             da_payment_code.InsertPaymentCode(payment_code);

                             if (premium_id_list == "")
                             {
                                 premium_id_list = premium_id;
                             }
                             else
                             {
                                 premium_id_list += "," + premium_id;

                             }

                         }

                     }
                     else
                     {
                         if (da_gtli_policy_prem_return.UpdateGTLIPolicyPremReturnStatus(2, payment_code.payment_code, premium_id))
                         {
                             da_payment_code.InsertPaymentCode(payment_code);

                             if (premium_id_list == "")
                             {
                                 premium_id_list = premium_id;
                             }
                             else
                             {
                                 premium_id_list += "," + premium_id;

                             }

                         }

                     }
                 }
             }
           
        }//End loop grid

        if (premium_id_list != "")
        { 
            string strUrl = "../GTLI/reconcile_detail.aspx?pid=" + premium_id_list;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
           
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please select premium transaction for reconcile.')", true);
        }
    }

    //View Reconcile Details
    protected void btnView_Click(object sender, EventArgs e)
    {
        string premium_id_list = "";
        foreach (GridViewRow row in gvGTLIPolicy.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = row.Cells[0].Controls[1] as CheckBox;
                string premium_id = gvGTLIPolicy.DataKeys[row.RowIndex]["GTLI_Premium_ID"].ToString();
               
                if (chk.Checked)
                {                    
                                        
                    if (premium_id_list == "")
                    {
                        premium_id_list = premium_id;
                    }
                    else
                    {
                        premium_id_list += "," + premium_id;

                    }                                             
                                     
                }
            }

        }//End loop grid

        if (premium_id_list != "")
        {
            string strUrl = "../GTLI/view_reconcile_detail.aspx?pid=" + premium_id_list;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            
        }
        else
        {
            
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please select premium transaction for view.')", true);
        }
    }
}