using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Finance_frm_gm_official_receipt_list : System.Web.UI.Page
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
        if (Helper.IsDate(txtPayDateFrom.Text.Trim()) && Helper.IsDate(txtPayDateTo.Text.Trim()))
        {
            DataTable tbl = da_officail_receipt.GetGroupMicroOfficialReceiptList(Helper.FormatDateTime(txtPayDateFrom.Text.Trim()), Helper.FormatDateTime(txtPayDateTo.Text.Trim()));

            //FILTER
            string criteria = "";
            criteria = ddlChannel.SelectedValue != "" ? "channel_id='" + ddlChannel.SelectedValue + "'" : "";
            criteria += ddlChannelItem.SelectedValue != "" ? (criteria.Trim() != "" ? " and channel_item_id='" + ddlChannelItem.SelectedValue + "'" : " channel_item_id='" + ddlChannelItem.SelectedValue + "'") : "";
            criteria += txtReceiptNo.Text.Trim() != "" ? (criteria.Trim() != "" ? " and receipt_no like '%" + txtReceiptNo.Text.Trim() + "%'" : " receipt_no like '%" + txtReceiptNo.Text.Trim() + "%'") : "";
          
            if (criteria != "")
            {
                DataTable tblFilter = tbl.Clone();
                foreach (DataRow r in tbl.Select(criteria))
                {
                    tblFilter.ImportRow(r);
                }

                gv_valid.DataSource = tblFilter;
                gv_valid.DataBind();
                if (tblFilter.Rows.Count > 0)
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
                gv_valid.DataSource = tbl;
                gv_valid.DataBind();
                if (tbl.Rows.Count > 0)
                {
                    tblResult.Attributes.CssStyle.Remove("display");
                }
                else{
                    tblResult.Attributes.CssStyle.Add("display", "none");
                    Helper.Alert(false, "No Record(s) found with selected criterias", lblError);
                }
            }
        }
        else
        {
            Helper.Alert(true, "Pay Date From & To are required with format [DD-MM-YYYY].", lblError);
        }
    }
   
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
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
         
            id = (Label)g_row.FindControl("lblOfficialReceiptId");
         
            if (e.CommandName == "CMD_PRINT")
            {
                string url = "frm_receipt_print_rp.aspx" ;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?rec_id=" + id.Text + "');</script>", false);
            }
           
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Ooop! system is error. Detail:"+ ex.Message, lblError);

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
}