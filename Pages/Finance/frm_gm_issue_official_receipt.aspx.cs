using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
public partial class Pages_Finance_frm_gm_issue_official_receipt : System.Web.UI.Page
{
    string userName = "";
    string userId = "";
    string pageName = "";
    private string GroupCode { get { return ViewState["gCode"] + ""; } set { ViewState["gCode"] = value; } }
    private string GroupId { get { return ViewState["gId"] + ""; } set { ViewState["gId"] = value; } }
    private double TotalAmount { get { return Convert.ToDouble(ViewState["tAmount"] + ""); } set { ViewState["tAmount"] = value; } }
    private double TotalDiscount { get { return Convert.ToDouble(ViewState["tDiscount"] + ""); } set { ViewState["tDiscount"] = value; } }
    private double GrandTotal { get { return Convert.ToDouble(ViewState["gAmount"] + ""); } set { ViewState["gAmount"] = value; } }
    private Int32 TotalPolicyNumber { get { return Convert.ToInt32(ViewState["tNumberPolicy"] + ""); } set { ViewState["tNumberPolicy"] = value; } }
    private List<string> InvoiceIdList { get { return (List<string>)ViewState["lInvoiceId"]; } set { ViewState["lInvoiceId"] = value; } }
    private string Message { get { return ViewState["message"] + ""; } set { ViewState["message"] = value; } }
    private string BunchId { get { return ViewState["lBunchId"] + ""; } set { ViewState["lBunchId"] = value; } }

    private DataTable DistinctBunchId { get { return (DataTable)Session["DATA_DISTINCT"]; } set { Session["DATA_DISTINCT"] = value; } }

    public class myList
    {
        public string BunchId { get; set; }
        public string InvoiceId { get; set; }
    }
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
        if (!Helper.IsDate(txtInvoiceDateFrom.Text.Trim()) || !Helper.IsDate(txtInvoiceDateTo.Text.Trim()))
        {
            Helper.Alert(true, "Invoice Date From & To are required with format [DD-MM-YYYY].", lblError);
        }
        else if (ddlChannel.SelectedIndex == 0)
        {
            Helper.Alert(true, "Channel is required.", lblError);
        }
        else if (ddlChannelItem.SelectedIndex == 0)
        {
            Helper.Alert(true, "Company is required.", lblError);
        }
        else
        {
            SearchData(true);
        }
    }

    private void SearchData(bool firstSearch)
    {
        try
        {


            DataTable tbl = Report.GroupMicro.GetInvoiceSummaryForOfficialReceipt();
            gvDetail.DataSource = null;
            gvDetail.DataBind();

            txtReceiveAmount.Text = "";
            txtPayDate.Text = "";
            txtTotalAmount.Text = "";
            txtTotalPolicies.Text = "";
            txtTotalDiscount.Text = "";
            txtTotalAmountAfterDiscount.Text = "";


            DataTable tblFilter = new DataTable();// = tbl.Clone();

            var result = tbl
        .AsEnumerable()
        .Where(myRow => myRow.Field<DateTime>("invoice_date") >= Helper.FormatDateTime(txtInvoiceDateFrom.Text.Trim()) && myRow.Field<DateTime>("invoice_date") <= Helper.FormatDateTime(txtInvoiceDateTo.Text.Trim()) && myRow.Field<string>("Channel_item_id") == ddlChannelItem.SelectedValue);


            if (result.Count<DataRow>() > 0)
            {
                tblFilter = result.CopyToDataTable();
            }


            gv_valid.DataSource = tblFilter;
            gv_valid.DataBind();
            if (tblFilter.Rows.Count > 0)
            {
                //show result session
                tblResult.Attributes.CssStyle.Remove("display");

            }
            else
            {

                //hide result session
                tblResult.Attributes.CssStyle.Add("display", "none");
                if (firstSearch)
                {
                    Helper.Alert(false, "Pending Invoice is not found.", lblError);
                }
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error in function [SearchData], detail:" + ex.Message, lblError);
        }
    }

    protected void ckb_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string invoiceId = "";
            double tAmount = 0;
            double gAmount = 0;
            double tDiscount = 0;
            Int32 tNumberPolicies = 0;
            InvoiceIdList = new List<string>();
            BunchId = "";

            foreach (GridViewRow r in gv_valid.Rows)
            {

                CheckBox c = (CheckBox)(r.FindControl("ckb"));
                Label lbl = (Label)(r.FindControl("lblInvoiceId"));
                Label lblGcode = (Label)(r.FindControl("lblGroupCode"));
                Label lblGId = (Label)(r.FindControl("lblGroupId"));
                Label lblBuchId = new Label();//= (Label)(r.FindControl("lblBunchId"));
                Label lblTotalAmount = (Label)(r.FindControl("lblTotalAmount"));
                Label lblAmount = (Label)r.FindControl("lblAmount");
                Label lblDiscount = (Label)r.FindControl("lblDiscount");
                Label lblNumberPolicy = (Label)r.FindControl("lblNumberPolicies");

                if (c.Checked)
                {
                    GroupCode = lblGcode.Text.Trim();
                    GroupId = lblGId.Text.Trim();
                    invoiceId += invoiceId.Trim() == "" ? lbl.Text : "," + lbl.Text;
                    tAmount += Convert.ToDouble(lblAmount.Text.Trim());
                    gAmount += Convert.ToDouble(lblTotalAmount.Text.Trim());
                    tDiscount += Convert.ToDouble(lblDiscount.Text.Trim());
                    tNumberPolicies += Convert.ToInt32(lblNumberPolicy.Text.Trim());

                    InvoiceIdList.Add(lbl.Text);
                    BunchId += BunchId == "" ? lbl.Text + "/" + lblBuchId.Text : "$" + lbl.Text + "/" + lblBuchId.Text;

                }
            }

            TotalAmount = tAmount;
            GrandTotal = gAmount;
            TotalDiscount = tDiscount;
            TotalPolicyNumber = tNumberPolicies;
            btnIssue.Enabled = TotalAmount > 0 ? true : false;
            txtReceiveAmount.Enabled = TotalAmount > 0 ? true : false;
            txtPayDate.Enabled = TotalAmount > 0 ? true : false;
            txtRemarks.Enabled = TotalAmount > 0 ? true : false;
            rdlMethod.Enabled = TotalAmount > 0 ? true : false;
            txtTotalAmount.Text = TotalAmount + "";
            txtTotalDiscount.Text = TotalDiscount + "";
            txtTotalPolicies.Text = TotalPolicyNumber + "";
            txtTotalAmountAfterDiscount.Text = GrandTotal + "";

            // Helper.Alert(false, "bunch_id in('" + bunchId.Replace(",", "','")+ "')", lblError);
            DataTable tbl = Report.GroupMicro.GetInvoiceDetailForOfficialReceipt();
            DataTable tblFilter = tbl.Clone();

            foreach (DataRow rd in tbl.Select("invoice_id in('" + invoiceId.Replace(",", "','") + "')"))
            {

                tblFilter.ImportRow(rd);
            }

            //distinct bunch id
            DataView dv = new DataView(tblFilter);
            DataTable distinct = dv.ToTable(true, "invoice_id", "Bunch_id");
            //STORE DISTINCT DATA IN SESSION TO US IN ISSUE OFFICIAL RECEIPT SECTION
            //Session["DATA_DISTINCT"] = distinct;
            DistinctBunchId = distinct;
            gvDetail.DataSource = tblFilter;
            gvDetail.DataBind();

        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
    protected void btnIssue_Click1(object sender, EventArgs e)
    {


        DateTime tranDate = DateTime.Now;
        bool isSuccess = true;
        string receiptNo = "";
        if (ValidateIssue())
        {

            try
            {
                //GET LAST RECEIPT NUMBER
                receiptNo = da_policy_prem_pay.Auto_Receipt_Number();
                //INSERT RECEIPT
                bl_official_receipt receipt = new bl_official_receipt()
                {
                    Official_Receipt_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "Ct_Official_Receipt" }, { "FIELD", "OFFICIAL_RECEIPT_ID" } }),
                    Policy_ID = GroupId,
                    Policy_Type = 2,
                    Method_Payment = Convert.ToInt32(rdlMethod.SelectedValue),
                    Amount = Convert.ToDouble(txtReceiveAmount.Text.Trim()),
                    Customer_ID = GroupCode,
                    Created_On = Helper.FormatDateTime(txtPayDate.Text.Trim()),
                    Created_By = userName,
                    Entry_Date = tranDate,
                    Receipt_No = receiptNo,
                    Created_Note = TotalPolicyNumber + " Policies"
                };


                if (da_officail_receipt.Insert_Official_Receipt(receipt))
                {
                    //check and save receive over amount      
                    double totalAmount = Convert.ToDouble(txtTotalAmountAfterDiscount.Text);
                    if (receipt.Amount > totalAmount)
                    {
                        da_left_over_amont.SaverOverAmount(
                            new bl_left_over_amount()
                            {
                                Left_Over_Amount_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "Ct_Left_Over_Amount" }, { "FIELD", "LEFT_OVER_AMOUNT_ID" } }),
                                Policy_ID = receipt.Policy_ID,
                                Received_Amount = receipt.Amount,
                                Prem_Amount = totalAmount,
                                Prem_Amount_Paid = totalAmount,
                                Prem_Amount_Left_Over = receipt.Amount - totalAmount,
                                Left_Over_Substract = 0,
                                Official_Receipt_ID = receipt.Official_Receipt_ID,
                                Received_Date = receipt.Created_On,
                                Created_By = userName,
                                Created_On = tranDate,
                                Status_Used = 0

                            }
                            );
                    }

                    if (Transaction.GroupMirco.Official_receipt.Backup(new Transaction.GroupMirco.Official_receipt.Tran()
                    {
                        OfficialReceiptId = receipt.Official_Receipt_ID,
                        TranBy = userName,
                        TranDate = tranDate,
                        TranType = "INSERT"
                    }))
                    {
                        Message = "Save Officail Receipt - Successfully </br>";

                        //INSERT METHOD PAYMENT
                        bl_method_payment method = new bl_method_payment()
                        {
                            Method_ID = Convert.ToInt32(rdlMethod.SelectedValue),
                            Method_Name = rdlMethod.SelectedValue == "0" ? "សាច់ប្រាក់/Cash" : rdlMethod.SelectedValue == "1" ? "មូលប្បទានបត្រ/Cheque" : rdlMethod.SelectedValue == "2" ? "ផ្សេងៗ/Others" : "",
                            Official_Receipt_ID = receipt.Official_Receipt_ID,
                            Transaction_ID = "",
                            Created_By = userName,
                            Created_On = Helper.FormatDateTime(txtPayDate.Text.Trim()),
                            Created_Note = txtRemarks.Text.Trim()
                        };
                        if (da_method_payment.Insert_Method_Payment(method))
                        {

                            Message += "Save Method Payment - Successfully </br>";

                            if (Transaction.GroupMirco.MethodPayment.Backup(new Transaction.GroupMirco.MethodPayment.Tran()
                            {
                                OfficialReceiptId = receipt.Official_Receipt_ID,
                                TranBy = userName,
                                TranDate = tranDate,
                                TranType = "INSERT"
                            }))
                            {
                                // int row = DistinctBunchId.Rows.Count;
                                //distinct invoice id
                                DataView inDv = new DataView(DistinctBunchId);
                                DataTable tblInvoice = inDv.ToTable(true, "invoice_id");
                                foreach (DataRow row in tblInvoice.Rows)
                                {
                                    var inId = row["invoice_id"].ToString();
                                    if (Transaction.GroupMirco.InvoiceSummary.Backup(new Transaction.GroupMirco.InvoiceSummary.Tran()
                                    {
                                        InvoiceId = inId,
                                        TranBy = userName,
                                        TranDate = tranDate,
                                        TranType = "UPDATE"
                                    }))
                                    {

                                        if (da_micro_group_invoice.summary.UpdateOfficialReceipt(inId, receipt.Official_Receipt_ID, "PAID", userName, tranDate))
                                        {
                                            Message += "Update Invoice Summary - Successfully </br>";
                                            //update status payment bunch summary
                                            foreach (DataRow r in DistinctBunchId.Select("invoice_id='" + inId + "'"))
                                            {
                                                var bId = r["bunch_id"].ToString();
                                                Transaction.GroupMirco.PaymentBunchSummary.Backup(new Transaction.GroupMirco.PaymentBunchSummary.Tran()
                                                {
                                                    BunchId = bId,
                                                    TranBy = userName,
                                                    TranDate = tranDate,
                                                    TranType = "UPDATE"
                                                });
                                                if (Transaction.SUCCESS)
                                                {
                                                    if (da_micro_group_policy_payment_bunch.summary.UpdateStatus(bId, 2, "PAID", userName, tranDate))
                                                    {
                                                        Message += "Update Payment Bunch Summary - Successfully </br>";

                                                        //UPDATE PAY DATE 

                                                        if (da_micro_group_policy_payment.UpdatePaymentDate(bId, Helper.FormatDateTime(txtPayDate.Text.Trim())))
                                                        {
                                                            Message += "Update Paydate - Successfully </br>";
                                                        }
                                                        else
                                                        {
                                                            isSuccess = false;
                                                            Message += "Update Paydate - Fail </br>";
                                                            break;
                                                        }


                                                    }
                                                    else
                                                    {
                                                        isSuccess = false;
                                                        Message += "Update Payment Bunch Summary - Fail </br>";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    //backup fail
                                                    isSuccess = false;
                                                    Message += "Backup Payment Bunch Summary - Fail </br>";
                                                    break;
                                                }


                                            }

                                        }
                                        else
                                        {
                                            isSuccess = false;
                                            Message += "Update Invoice Summary - Fail </br>";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //backup invoice summary fail
                                        isSuccess = false;
                                        Message += "Backup Invoice Summary - Fail </br>";
                                        break;
                                    }

                                }//end loop invoices

                            }
                            else
                            {
                                //backup Method Payment fail
                                isSuccess = false;
                                Message += "Backup Method Payment - Fail </br>";

                            }

                        }
                        else
                        {
                            isSuccess = false;
                            // Helper.Alert(true, "Save Method Payment Fail.", lblError);
                            Message += "Save Method Payment - Fail </br>";
                        }

                    }
                    else
                    {
                        // backup official receipt fail
                        isSuccess = false;
                        // Helper.Alert(true, "Save Method Payment Fail.", lblError);
                        Message += "Backup Official Receipt - Fail </br>";
                    }

                }
                else
                {
                    isSuccess = false;
                    //Helper.Alert(true, "Save Receipt Fail.", lblError);
                    Message += "Save Receipt - Fail </br>";
                }


                if (isSuccess)
                {

                    //clear backup transaction
                    if (Transaction.GroupMirco.ClearBackupTransactionIssueOfficialReceiptRecords(userName, tranDate, ""))
                    {
                        string urlPrint = "<a href='../Finance/frm_receipt_print_rp.aspx?rec_id=" + receipt.Official_Receipt_ID + "' target='_blank'><span>Click here to print</span></a>";
                        Helper.Alert(false, "Issue Official Receipt Successfully.<br/><b>Receipt No." + receipt.Receipt_No + "</b><br/>" + urlPrint, lblError);
                        //relead data
                        SearchData(false);
                    }
                    else
                    {

                        Helper.Alert(false, "Issue Official Receipt Successfully.<br/> System clear backup transaction fail.", lblError);

                    }

                }
                else
                {
                    if (Transaction.GroupMirco.RollBackIssueOfficialReceipt(userName, tranDate))
                    {
                        Transaction.GroupMirco.ClearBackupTransactionIssueOfficialReceiptRecords(userName, tranDate, "");
                        Helper.Alert(true, Message + " System rollback transaction successfully, please try again.", lblError);
                    }
                    else
                    {
                        Helper.Alert(true, Message + " System rollback transaction fial, please contact your system administrator.", lblError);
                    }

                }
            }
            catch (Exception ex)
            {
                Helper.Alert(true, ex.Message, lblError);
            }
        }
        else
        {
            //invalid input
            Helper.Alert(true, Message, lblError);
        }

    }
    private bool ValidateIssue()
    {
        bool valid = true;
        if (!Helper.IsAmount(txtReceiveAmount.Text.Trim()))
        {
            valid = false;
            Message = "Received Amount is not valid number format.";
        }
        else if (Convert.ToDouble(txtReceiveAmount.Text) < Convert.ToDouble(txtTotalAmountAfterDiscount.Text))
        {
            valid = false;
            Message = "Received Amount must be equal or grather than Totatl Amount After Discount.";

        }
        else if (!Helper.IsDate(txtPayDate.Text))
        {
            valid = false;
            Message = "Paydate is not valid format.";
        }

        return valid;

    }
}