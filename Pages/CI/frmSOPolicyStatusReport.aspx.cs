using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_CI_frmTermiateSOReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        { 
            LoadData(txtCustomerName.Text.Trim(),ddlGender.SelectedValue.Trim(),"","");
        }
    }
    void LoadData(string customer_name, string gender, string policy_number, string status)
    {
        
        string filter = "";
        DateTime fDate = new DateTime();
        DateTime tDate = new DateTime();

        if (status.Trim().ToUpper() != "TER")
        {
            fDate = Helper.FormatDateTime(txtFDate.Text);
            tDate = Helper.FormatDateTime(txtTDate.Text);

        }
        else
        {
            fDate = new DateTime(1900, 1, 1);
            tDate = new DateTime(1900, 1, 1);
        }

        //get all S.O policies
        DataTable tbl = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), customer_name, policy_number,"CI,SO");
        filter = gender.Trim() == "" ? "" : " gender='" + gender + "'";

        filter += filter.Trim() == "" ? " policy_status='IF'" : " and policy_status='IF'";//get only IF status
        DataTable tbl_filter = tbl.Clone();//clone propertity from table tbl such columns name
        int a = tbl_filter.Rows.Count;
        DataRow row_filter;
        foreach (DataRow row in tbl.Select(filter))//filter data in table tbl
        {
            row_filter = tbl_filter.NewRow();
            //copy data from tbl to tbl_filter base on codition
            for (int col = 0; col < tbl_filter.Columns.Count; col++)
            {
                row_filter[col] = row[col].ToString();
            }

            tbl_filter.Rows.Add(row_filter);
        }

        if (tbl_filter.Rows.Count > 0)
        {
            gv_policy.DataSource = tbl_filter;
            gv_policy.DataBind();
        }
    }

}