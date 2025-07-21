using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Finance_rep_frm_invoice_list : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            tblResult.Attributes.CssStyle.Add("display", "none");
            Helper.BindChannel(ddlChannel);
            ddlChannel.SelectedIndex = 2;
            ddlChannel_SelectedIndexChanged(null, null);
            ddlChannel.Enabled = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (Helper.IsDate(txtInvoiceDateFrom.Text.Trim()) && Helper.IsDate(txtInvoiceDateTo.Text.Trim()))
        {
            DataTable tbl = Report.GroupMicro.GetInvoiceSummaryList(Helper.FormatDateTime(txtInvoiceDateFrom.Text.Trim()), Helper.FormatDateTime(txtInvoiceDateTo.Text.Trim()),ddlChannel.SelectedValue, ddlChannelItem.SelectedValue, txtInvoiceNo.Text.Trim());

            gv_valid.DataSource = tbl;
            gv_valid.DataBind();
          
                if (tbl.Rows.Count > 0)
                {
                    tblResult.Attributes.CssStyle.Remove("display");
                    
                }
                else
                {
                    tblResult.Attributes.CssStyle.Add("display", "none");
                    Helper.Alert(false, "No Record(s) found with selected criterias", lblError);
                }
                     
        }
        else
        {
            Helper.Alert(true, "Invoice Date From & To are required with format [DD-MM-YYYY].", lblError);
        }
    }
    protected void gv_valid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label id;

        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow g_row;
            GridView g = sender as GridView;
            g_row = g.Rows[index];

            id = (Label)g_row.FindControl("lblInvoiceNumber");

            if (e.CommandName == "CMD_PRINT")
            {
                string url = "rep_frm_invoice_print_rp.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?inv_number=" + id.Text + "');</script>", false);
            }

        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Ooop! system is error. Detail:" + ex.Message, lblError);

        }
    }
    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


        }
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
}