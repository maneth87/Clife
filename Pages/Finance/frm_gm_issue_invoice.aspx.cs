using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.IO;
public partial class Pages_Finance_frm_gm_issue_invoice : System.Web.UI.Page
{
    private string GroupCode { get { return ViewState["gCode"] + ""; } set { ViewState["gCode"]=value; } }
    private double TotalAmount { get { return Convert.ToDouble( ViewState["tAmount"] + ""); } set { ViewState["tAmount"] = value; } }
    private double TotalDiscount { get { return Convert.ToDouble(ViewState["tDiscount"] + ""); } set { ViewState["tDiscount"] = value; } }
    private double GrandTotal { get { return Convert.ToDouble(ViewState["gAmount"] + ""); } set { ViewState["gAmount"] = value; } }
    private Int32 TotalPolicyNumber { get { return Convert.ToInt32(ViewState["tNumberPolicy"] + ""); } set { ViewState["tNumberPolicy"] = value; } }
    private List<string> BunchIdList { get { return (List<string>)ViewState["lBunchId"]; } set { ViewState["lBunchId"] = value; } }
    private List<string> BunchDetailIdList { get { return (List<string>)ViewState["lBunchDetailId"]; } set { ViewState["lBunchDetailId"] = value; } }

    private string Message { get { return ViewState["message"] + ""; } set { ViewState["message"] = value; } }

    string userName = "";
    string userId = "";
    string pageName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        userId = Membership.GetUser().ProviderUserKey.ToString();
        userName = Membership.GetUser().UserName;
        lblError.Text = "";
        if (!Page.IsPostBack)
        {
            btnIssue.Enabled = false;
            //hide result session
            tblResult.Attributes.CssStyle.Add("display", "none");
            if (!Helper.BindChannel(ddlChannel))
            {
                Helper.Alert(true, "Bind Channel Error", lblError);
            }
            else
            { 
                //default selected channel
                ddlChannel.SelectedIndex = 2;
                ddlChannel_SelectedIndexChanged(null, null);
                ddlChannel.Enabled = false;
               
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //validate require fields
        bool isValide = true; string message = "";
        if(!Helper.IsDate(txtReportDateFrom.Text.Trim()) || !Helper.IsDate( txtReportDateTo.Text.Trim()))
        {
            isValide = false;
            message = "Report Date FROM & TO are required with format [DD-MM-YYYY].";
        }
        else if (ddlChannel.SelectedIndex == 0)
        {
            isValide = false;
            message = "Channel is required.";
        }
        else if (ddlChannelItem.SelectedIndex == 0)
        {
            isValide = false;
            message = "Company is required..";
        }
        if (isValide)
        {
            //DataTable tbl = SearchData();// Report.GroupMicro.GetBunchSummaryForInvoice();

            //if (tbl.Rows.Count > 0)
            //{
            //    //show result session
            //    tblResult.Attributes.CssStyle.Remove("display");
            //    gv_valid.DataSource = tbl;
            //    gv_valid.DataBind();

            //}
            //else
            //{
            //    //hide result session
            //    tblResult.Attributes.CssStyle.Add("display", "none");
            //    Helper.Alert(false, "Bunch record(s) not found.", lblError);
            //}
            SearchData(true);
        }
        else
        {
            Helper.Alert(true, message, lblError);
        }
       
    }
    private void SearchData( bool firstSearch)
    {
        try
        {
            gvDetail.DataSource = null;
            gvDetail.DataBind();


            txtInvoiceDate.Text = "";
            txtTotalAmount.Text = "";
            txtTotalPolicies.Text = "";
            txtTotalDiscount.Text = "";
            txtTotalAmountAfterDiscount.Text = "";
            txtExchangeRateTax.Text = "";
            txtTotalAmountKh.Text = "";
            txtInvoiceNo.Text = "";
            DataTable tbl = Report.GroupMicro.GetBunchSummaryForInvoice();

            DataTable tblFilter = new DataTable();// = tbl.Clone();

            var result = tbl
        .AsEnumerable()
        .Where(myRow => myRow.Field<DateTime>("Report_date") >= Helper.FormatDateTime(txtReportDateFrom.Text.Trim()) && myRow.Field<DateTime>("Report_date") <= Helper.FormatDateTime(txtReportDateTo.Text.Trim()) && myRow.Field<string>("Channel_item_id") == ddlChannelItem.SelectedValue);

            if (result.Count<DataRow>() > 0)
            {
                tblFilter = result.CopyToDataTable();

                //show result session
                tblResult.Attributes.CssStyle.Remove("display");
                gv_valid.DataSource = tblFilter;
                gv_valid.DataBind();
            }
            else
            {
                //hide result session
                tblResult.Attributes.CssStyle.Add("display", "none");
                if (firstSearch)
                {
                    Helper.Alert(false, "Bunch record(s) not found.", lblError);
                }
            }

        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error function [SearchData], detail:" + ex.Message, lblError);
        }
        
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
    protected void ckb_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string bunchId = "";
            double tAmount = 0;
            double gAmount = 0;
            double tDiscount = 0;
            Int32 tNumberPolicies = 0;
            BunchIdList = new List<string>();
            foreach (GridViewRow r in gv_valid.Rows)
            {
                
                CheckBox c = (CheckBox)(r.FindControl("ckb"));
                Label lbl = (Label)(r.FindControl("lblBunchId"));
                Label lblGcode = (Label)(r.FindControl("lblGroupCode"));
                Label lblTotalAmount = (Label)(r.FindControl("lblTotalAmount"));
                Label lblAmount = (Label)r.FindControl("lblAmount");
                Label lblDiscount = (Label)r.FindControl("lblDiscount");
                Label lblNumberPolicy = (Label)r.FindControl("lblNumberPolicies");
                GroupCode = lblGcode.Text.Trim();
                if (c.Checked)
                {
                   
                    bunchId += bunchId.Trim()=="" ? lbl.Text : ","+lbl.Text;
                    tAmount += Convert.ToDouble(lblAmount.Text.Trim());
                    gAmount += Convert.ToDouble(lblTotalAmount.Text.Trim());
                    tDiscount += Convert.ToDouble(lblDiscount.Text.Trim());
                    tNumberPolicies += Convert.ToInt32(lblNumberPolicy.Text.Trim());

                    BunchIdList.Add(lbl.Text);
                }
            }

            TotalAmount =tAmount;
            GrandTotal = gAmount;
            TotalDiscount = tDiscount;
            TotalPolicyNumber = tNumberPolicies;
            btnIssue.Enabled = TotalAmount > 0 ? true : false;
            txtInvoiceDate.Enabled = TotalAmount > 0 ? true : false;
            txtExchangeRateTax.Enabled = TotalAmount > 0 ? true : false;
            txtInvoiceNo.Enabled = TotalAmount > 0 ? true : false;
            txtTotalAmount.Text = TotalAmount + "";
            txtTotalDiscount.Text = TotalDiscount + "";
            txtTotalPolicies.Text = TotalPolicyNumber + "";
            txtTotalAmountAfterDiscount.Text = GrandTotal + "";

           // Helper.Alert(false, "bunch_id in('" + bunchId.Replace(",", "','")+ "')", lblError);
            DataTable tbl  =Report.GroupMicro.GetBunchDetailForInvoice();
            DataTable tblFilter = tbl.Clone();
          
            foreach (DataRow rd in tbl.Select("bunch_id in('" + bunchId.Replace(",","','") +"')"))
            {
             
                tblFilter.ImportRow(rd);
            }

            gvDetail.DataSource = tblFilter;
            gvDetail.DataBind();
           
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    private bool ValidateIssue()
    {
        bool isValid=true;
        if (!Helper.IsDate(txtInvoiceDate.Text.Trim()))
        {
            isValid=false;
            Message = "Invoice Date is require with format [DD-MM-YYYY].";
        }
        else if(!Helper.IsAmount(txtExchangeRateTax.Text.Trim()))
        {
            isValid = false;
            Message = "Exchange rate is incorrect format.";
        }
        else if (txtInvoiceNo.Text.Trim().Length != 18)
        {
            isValid = false;
            Message = "Invoice Number is incorrect format.";
        }
        return isValid;
    }
    protected void btnIssue_Click(object sender, EventArgs e)
    {
        
        try
        {
            if (ValidateIssue())
            {
                bool isSuccess = true;
                string message = "";
                DateTime tranDate = DateTime.Now;
                bl_micro_group_invoice.summary inSummary = new bl_micro_group_invoice.summary()
                {
                    InvoiceNumber= txtInvoiceNo.Text.Trim(),
                    Amount = TotalAmount,
                    DiscountAmount = TotalDiscount,
                    TotalAmount = GrandTotal,
                    GroupMasterCode = GroupCode,
                    NumberPolicy = TotalPolicyNumber,
                    InvoiceDate= Helper.FormatDateTime(txtInvoiceDate.Text.Trim()),
                    ExchangeRateTax = Convert.ToDouble(txtExchangeRateTax.Text.Trim()),
                    TotalAmountKh = Convert.ToDouble(txtTotalAmountKh.Text.Trim()),
                    CreatedBy = userName,
                    CreatedOn = tranDate,
                    Remarks = "Invoiced"
                };

                if (da_micro_group_invoice.summary.Save(inSummary))
                {
                    //backup invoice summary
                    Transaction.GroupMirco.InvoiceSummary.Backup(new Transaction.GroupMirco.InvoiceSummary.Tran()
                    {
                        InvoiceId = inSummary.InvoiceId,
                        TranBy = userName,
                        TranDate = tranDate,
                        TranType = "INSERT"
                    });

                    bl_micro_group_invoice.detail inDetail;
                    bool isSaveDetail = true;
                    foreach (GridViewRow grow in gvDetail.Rows)
                    {
                        Label lblBunchId = (Label)grow.FindControl("lblBunchId");
                        Label lblBunchDetailId = (Label)grow.FindControl("lblBunchDetailId");

                        inDetail = new bl_micro_group_invoice.detail() { InvoiceId = inSummary.InvoiceId, BunchId = lblBunchId.Text, BunchDetailId = lblBunchDetailId.Text };
                        isSaveDetail = da_micro_group_invoice.detail.Save(inDetail);
                        //backup invoice detail
                        Transaction.GroupMirco.InvoiceDetail.Backup(new Transaction.GroupMirco.InvoiceDetail.Tran()
                        {
                            InvoiceDetailId = inDetail.InvoiceDetailId,
                            TranBy = userName,
                            TranDate = tranDate,
                            TranType = "INSERT"
                        });
                        if (!isSaveDetail)
                        {
                            isSuccess = false;
                            message = da_micro_group_invoice.MESSAGE;
                            break;
                        }
                    }

                    if (isSaveDetail)
                    {

                        foreach (string bId in BunchIdList)
                        {

                            Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran()
                            {
                                BunchId = bId,
                                TranBy = userName,
                                TranDate = tranDate,
                                TranType = "UPDATE"
                            });

                            if (!da_micro_group_policy_payment_bunch.summary.UpdateStatus(bId, 1, "Invoiced", userName, tranDate))
                            {
                                isSuccess = false;
                                message = da_micro_group_policy_payment_bunch.MESSAGE;
                                break;

                            }

                        }
                    }

                }
                else
                {
                    isSuccess = false;

                }

                if (isSuccess)
                {
                   // Helper.Alert(false, "Saved Successfully.", lblError);
                    //clear backup
                    Transaction.GroupMirco.ClearBackupTransactionIssueInvoiceRecords(userName, tranDate, "");
                    string urlPrint = "<a href='../Finance/rep_frm_invoice_print_rp.aspx?inv_number=" + inSummary.InvoiceNumber + "' target='_blank'><span>Click here to print</span></a>";
                    Helper.Alert(false, "Issue Invoice Successfully.<br/><b>Invoice No." + inSummary.InvoiceNumber + "</b><br/>" + urlPrint, lblError);
                   // btnSearch_Click(null, null);
                    SearchData(false);
                }
                else
                {
                    bool isRollBack = Transaction.GroupMirco.RollBackIssueInvoice(userName, tranDate);
                    if (isRollBack)
                    {
                        Transaction.GroupMirco.ClearBackupTransactionIssueInvoiceRecords(userName, tranDate, "");
                        Helper.Alert(true, message + "<br />System Rollback: <b>Successfully</b>", lblError);
                    }
                    else
                    {
                        Helper.Alert(true, message + "<br />System Rollback: <b>Successfully</b><br/> Detail:" + Transaction.MESSAGE, lblError);
                    }

                }
            }
            else
            { 
            //message for validate false
                Helper.Alert(true, Message, lblError);
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }


    protected void txtExchangeRateTax_TextChanged(object sender, EventArgs e)
    {
        if (txtExchangeRateTax.Text.Trim() == "")
        {
            Helper.Alert(true, "Exchange rate tax is required. Please input.", lblError);
        }
       else if (!Helper.IsAmount(txtExchangeRateTax.Text.Trim()))
        {
            Helper.Alert(true, "Exchange rate tax is incorrect format, please check your input.", lblError);
        }
       
        else
        {
            txtTotalAmountKh.Text = (Convert.ToDouble(txtTotalAmountAfterDiscount.Text.Trim()) * Convert.ToDouble(txtExchangeRateTax.Text.Trim())) + "";
        }
    }
    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (Helper.IsDate(txtInvoiceDate.Text))
        {
            txtInvoiceNo.Text = bl_micro_group_invoice.GenerateInvoiceNumber(Convert.ToInt32(txtInvoiceNo.Text), Helper.FormatDateTime(txtInvoiceDate.Text));
        }
        else
        {
            Helper.Alert(true, "To generate invioce number is required Invoice Date.", lblError);
        }
    }
}