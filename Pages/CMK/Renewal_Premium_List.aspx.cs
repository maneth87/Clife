using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CMK_Renewal_Premium_List : System.Web.UI.Page
{
    List<bl_cmk.CMK_Group_Premium> list_renewal_prem = new List<bl_cmk.CMK_Group_Premium>();
    string sortEx = "";
    string sortCol = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            //bind user name and user id to hiddenfield
            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

            GetData(false);
        }
    }

    void GetData(bool get_data_type)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        string message = ""; DateTime fromDate, toDate; 

        if (get_data_type != true) // Load Page
        {
            fromDate = new DateTime(1900, 1, 01); toDate = DateTime.Now;

            list_renewal_prem = da_cmk.Policy.GetRenewalPremiumList(fromDate, toDate);
        }
        else // 
        {
            if (txtFrom_date.Text != "" && txtTo_date.Text != "")
            {
                if (DateTime.Parse(txtFrom_date.Text, dtfi).Date < DateTime.Parse(txtTo_date.Text, dtfi).Date)
                {
                    list_renewal_prem = da_cmk.Policy.GetRenewalPremiumList(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi));
                }
                else
                {
                    message = "To_Date is smaller than From_Date!!!";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alertMesg('" + message + "');", true);
                }
            }
            else
            {
                message = "Please check your search date!!!";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alertMesg('" + message + "');", true);
            }
        }

        if (list_renewal_prem.Count > 0)
        {
            gv_report.DataSource = list_renewal_prem;
            gv_report.DataBind();

            /*show record count*/
            lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + list_renewal_prem.Count;
        }
        
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetData(true);
    }

    //Row Index Changing
    protected void gv_report_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_report.PageIndex = e.NewPageIndex;

        DataTable tbl;
        tbl = (DataTable)ViewState["RENEWAL_PREM_DATA"];

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_report.DataSource = dview;
        gv_report.DataBind();

        /*show record count*/
        lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + tbl.Rows.Count;

    }

}