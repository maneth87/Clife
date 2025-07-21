using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class Pages_PolicyPayment_update_policy_status : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           // Search_Policy();
        }
    }


    void Search_Policy()
    {
        TimeSpan start_time = new TimeSpan(00, 00, 00);
      
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        if (txtFrom_date.Text == "" && txtTo_date.Text == "")
        {
            DataTable dt = da_policy_prem_pay.GetPolicy_List(DateTime.Parse("01/01/1900"), DateTime.Parse("01/01/1900"), txtLastName_Search.Text, txtFirstName_Search.Text, txtPolicyNum.Text,0);
            GvOPolicy.DataSource = dt;

            lbltotal.Text = dt.Rows.Count.ToString();
        }
        else  
        {
            DataTable dt = da_policy_prem_pay.GetPolicy_List(DateTime.Parse(txtFrom_date.Text, dtfi).Date + start_time, DateTime.Parse(txtTo_date.Text, dtfi).Date +start_time, txtLastName_Search.Text, txtFirstName_Search.Text, txtPolicyNum.Text,0);
            GvOPolicy.DataSource = dt;
        
            lbltotal.Text = dt.Rows.Count.ToString();
        }

        GvOPolicy.DataBind();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        da_policy_prem_pay.Update_Policy_Status(hdfPolicy_ID.Value, ddlPolicyStatus.SelectedValue, txtNote.Text);

        TimeSpan start_time = new TimeSpan(00, 00, 00);

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DataTable dt = da_policy_prem_pay.GetPolicy_List(DateTime.Now.Date + start_time, DateTime.Now.Date + start_time, txtLastName_Search.Text, txtFirstName_Search.Text, txtPolicyNum.Text,1);
        GvOPolicy.DataSource = dt;
        GvOPolicy.DataBind();

        lbltotal.Text = dt.Rows.Count.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search_Policy();

        txtFirstName_Search.Text = "";
        txtLastName_Search.Text = "";
        txtFrom_date.Text = "";
        txtTo_date.Text = "";
        txtPolicyNum.Text = "";

    }
}