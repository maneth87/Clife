using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
public partial class Pages_Reports_micro_policy_payment_comparison : System.Web.UI.Page
{
    string userID = "";
    string userName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        userID = Membership.GetUser().ProviderUserKey.ToString();
        userName = Membership.GetUser().UserName;
        if (!Page.IsPostBack)
        {
            Helper.BindChannelItem(ddlChannelItem, "0152DF80-BA95-46A9-BB7A-E71966A34089");
            showMessage("", "");
            btnExport.Attributes.Add("disabled", "disabled");
            ckbIsNo.Enabled = false;

            Session["SORTEX"] = "ASC";
            Session["SORTCOL"] = "remarks";

        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDateFrom.Text.Trim() != "" && txtDateTo.Text.Trim() != "")
            {
                if (Helper.IsDate(txtDateFrom.Text.Trim()) && Helper.IsDate(txtDateTo.Text.Trim()))
                {
                    DataTable tbl = da_banca.Payment.GetPolicyPaymentComparison(Helper.FormatDateTime(txtDateFrom.Text.Trim()), Helper.FormatDateTime(txtDateTo.Text.Trim()), userName,ddlChannelItem.SelectedValue);

                    int duplicate = 0;
                    //exist = tbl.AsEnumerable().Count(r => r.Field(""));
                    string remarks = "";
                    foreach (DataRow row in tbl.Rows)
                    {
                        remarks = "";
                        duplicate = tbl.AsEnumerable().Count(r => r.Field<String>("application_number") == row["application_number"].ToString().Trim() && r.Field<String>("application_number").ToString().Trim()!="");
                        if (duplicate > 1)
                        {
                            remarks = "NO - DUPLICATE APPLICATION NUMBER";
                        }
                        else
                        {
                            if (row["application_number"].ToString().Trim() == "")
                            {
                                remarks = "NO - MISSED RECORD IN CAMLIFE";

                            }
                            else if (row["application_number_bank"].ToString().Trim() == "")
                            {
                                remarks = "NO - MISSED RECORD IN BANK";
                            }
                            else
                            {
                                if (row["issued_date"].ToString().Trim() != row["tran_date"].ToString().Trim())
                                {
                                    remarks += remarks == "" ? "NO - DIFFERENT ISSUED & TRANSACTION DATE" : " | DIFFERENT ISSUED & TRANSACTION DATE";
                                }
                                if (row["amount"].ToString().Trim() != row["paid_amount"].ToString().Trim())
                                {
                                    remarks += remarks == "" ? "NO - AMOUNT IS DIFFERENT" : " | AMOUNT IS DIFFERENT";
                                }
                                if (row["customer_name"].ToString().Trim().ToUpper().Replace(" ", "") != row["customer_name_bank"].ToString().Trim().ToUpper().Replace(" ", ""))
                                {
                                    remarks += remarks == "" ? "NO - CUSTOMER NAME IS DIFFERENT" : " | CUSTOMER NAME IS DIFFERENT";
                                }
                                if (row["PAYMENT_REFERENCE_NO"].ToString().Trim().ToUpper() != row["PAYMENT_REFERENCE_NO_BANK"].ToString().Trim().ToUpper())
                                {
                                    remarks += remarks == "" ? "NO - PAYMENT REFERENCE NO. IS DIFFERENT" : " | PAYMENT REFERENCE NO. IS DIFFERENT";
                                }
                            }
                        }
                       
                        row["remarks"] = remarks.Trim()=="" ? "YES": remarks;
                    }

                    string sortEx = "";
                    string sortCol = "";
                    sortEx = Session["SORTEX"] + "";
                    sortCol = Session["SORTCOL"] + "";
                    DataView dview = new DataView(tbl);
                    if (sortEx != "")
                    {
                        dview.Sort = sortCol + " " + sortEx;

                    }
                    gv_valid.DataSource = dview;
                    gv_valid.DataBind();
                    // lblRecords.Text = tbl.Rows.Count > 0 ? tbl.Rows.Count + " Record(s)" : "";
                    if (tbl.Rows.Count > 0)
                    {
                        btnExport.Attributes.Remove("disabled");
                        lblRecords.Text = tbl.Rows.Count + " Record(s)";
                        ckbIsNo.Enabled = true;

                    }
                    else
                    {
                        Helper.Alert(false, "No record found.", lblError);
                        btnExport.Attributes.Add("disabled", "disabled");
                        lblRecords.Text = "";
                        ckbIsNo.Enabled = false;
                    }
                    Session["SS_DATA"] = dview.ToTable();
                }
                else
                {
                    Helper.Alert(true, "Issued Date From & To must be in formating DD-MM-YYYY.", lblError);
                }
            }
            else
            {
                Helper.Alert(true, "Issued Date From & To are required.", lblError);
            }


        }
        catch (Exception ex)
        {
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            Helper.Alert(true, ex.Message, lblError);
        }
    }

    private bool GenerateWingBancaPayment()
    {
        bool result = false;
        try
        {
            if (txtDateFrom.Text.Trim() != "" && txtDateTo.Text.Trim() != "")
            {
                if (Helper.IsDate(txtDateFrom.Text.Trim()) && Helper.IsDate(txtDateTo.Text.Trim()))
                {
                    DataTable tbl = da_banca.Payment.GetPolicyPaymentComparison(Helper.FormatDateTime(txtDateFrom.Text.Trim()), Helper.FormatDateTime(txtDateTo.Text.Trim()), userName, ddlChannelItem.SelectedValue);

                    int duplicate = 0;
                    //exist = tbl.AsEnumerable().Count(r => r.Field(""));
                    string remarks = "";
                    foreach (DataRow row in tbl.Rows)
                    {
                        remarks = "";
                        duplicate = tbl.AsEnumerable().Count(r => r.Field<String>("application_number") == row["application_number"].ToString().Trim() && r.Field<String>("application_number").ToString().Trim() != "");
                        if (duplicate > 1)
                        {
                            remarks = "NO - DUPLICATE APPLICATION NUMBER";
                        }
                        else
                        {
                            if (row["application_number"].ToString().Trim() == "")
                            {
                                remarks = "NO - MISSED RECORD IN CAMLIFE";

                            }
                            else if (row["application_number_bank"].ToString().Trim() == "")
                            {
                                remarks = "NO - MISSED RECORD IN BANK";
                            }
                            else
                            {
                                if (row["issued_date"].ToString().Trim() != row["tran_date"].ToString().Trim())
                                {
                                    remarks += remarks == "" ? "NO - DIFFERENT ISSUED & TRANSACTION DATE" : " | DIFFERENT ISSUED & TRANSACTION DATE";
                                }
                                if (row["amount"].ToString().Trim() != row["paid_amount"].ToString().Trim())
                                {
                                    remarks += remarks == "" ? "NO - AMOUNT IS DIFFERENT" : " | AMOUNT IS DIFFERENT";
                                }
                                if (row["customer_name"].ToString().Trim().ToUpper().Replace(" ", "") != row["customer_name_bank"].ToString().Trim().ToUpper().Replace(" ", ""))
                                {
                                    remarks += remarks == "" ? "NO - CUSTOMER NAME IS DIFFERENT" : " | CUSTOMER NAME IS DIFFERENT";
                                }
                                if (row["PAYMENT_REFERENCE_NO"].ToString().Trim().ToUpper() != row["PAYMENT_REFERENCE_NO_BANK"].ToString().Trim().ToUpper())
                                {
                                    remarks += remarks == "" ? "NO - PAYMENT REFERENCE NO. IS DIFFERENT" : " | PAYMENT REFERENCE NO. IS DIFFERENT";
                                }
                            }
                        }

                        row["remarks"] = remarks.Trim() == "" ? "YES" : remarks;
                    }

                    string sortEx = "";
                    string sortCol = "";
                    sortEx = Session["SORTEX"] + "";
                    sortCol = Session["SORTCOL"] + "";
                    DataView dview = new DataView(tbl);
                    if (sortEx != "")
                    {
                        dview.Sort = sortCol + " " + sortEx;

                    }
                    gv_valid.DataSource = dview;
                    gv_valid.DataBind();
                    // lblRecords.Text = tbl.Rows.Count > 0 ? tbl.Rows.Count + " Record(s)" : "";
                    if (tbl.Rows.Count > 0)
                    {
                        btnExport.Attributes.Remove("disabled");
                        lblRecords.Text = tbl.Rows.Count + " Record(s)";
                        ckbIsNo.Enabled = true;

                    }
                    else
                    {
                        Helper.Alert(false, "No record found.", lblError);
                        btnExport.Attributes.Add("disabled", "disabled");
                        lblRecords.Text = "";
                        ckbIsNo.Enabled = false;
                    }
                    Session["SS_DATA"] = dview.ToTable();
                }
                else
                {
                    Helper.Alert(true, "Issued Date From & To must be in formating DD-MM-YYYY.", lblError);
                }
            }
            else
            {
                Helper.Alert(true, "Issued Date From & To are required.", lblError);
            }


        }
        catch (Exception ex)
        {
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            Helper.Alert(true, ex.Message, lblError);
        }

        return result;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

            Helper.excel.Sheet = sheet1;
            Helper.excel.HeaderText = new string[] {
        "Branch Code", "Brand Name" ,"Application Number", "Policy Number", "Issued Date", "Package",  "Amount (USD)","Customer Name","Gender", "Agent Code", "Agent Name", "App_Number Bank","Cusotmer Name Bank","Transaction Type","Paid Amount", "Currency", "Transaction Date","Payment Reference No","Payment Reference No Bank", "Correct?"
       
        };

            Helper.excel.generateHeader();
            int row_no = 0;
            DataTable tbl = (DataTable)Session["SS_DATA"];
          
            if (ckbIsNo.Checked)
            {
                DataTable tblFiltered = tbl.AsEnumerable()
                    .Where(row => row.Field<String>("remarks") != "YES")
                    .CopyToDataTable();

                tbl = new DataTable();
                tbl = tblFiltered;
            }

            foreach (DataRow r in tbl.Rows)//foreach (DataRow r in my_session.DATA.Rows)
            {
                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(r["branch_code"].ToString());

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(r["branch_name"].ToString());

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(r["Application_Number"].ToString());

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(r["Policy_number"].ToString());

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue(Convert.ToDateTime(r["issued_date"].ToString()).ToString("dd-MM-yyyy") == "01-01-1900" ? "" : Convert.ToDateTime(r["issued_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(r["Package"].ToString());

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(Convert.ToDouble(r["Amount"].ToString()) == 0 ? "" : r["Amount"].ToString());

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue(r["Customer_name"].ToString());

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                Cell9.SetCellValue(r["Gender"].ToString());

                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                Cell10.SetCellValue(r["sale_agent_id"].ToString());

                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                Cell11.SetCellValue(r["agent_name"].ToString());

                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                Cell12.SetCellValue(r["Application_Number_Bank"].ToString());

                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                Cell13.SetCellValue(r["Customer_Name_Bank"].ToString());

                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                Cell14.SetCellValue(r["transaction_type"].ToString());

                HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                Cell15.SetCellValue(Convert.ToDouble(r["Paid_Amount"].ToString()) == 0 ? "" : r["Paid_Amount"].ToString());

                HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                Cell16.SetCellValue(r["currency"].ToString());

                HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                Cell17.SetCellValue(Convert.ToDateTime(r["tran_date"].ToString()).ToString("dd-MM-yyyy") == "01-01-1900" ? "" : Convert.ToDateTime(r["tran_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                Cell18.SetCellValue(r["payment_reference_no"].ToString());

                HSSFCell Cell19= (HSSFCell)rowCell.CreateCell(18);
                Cell19.SetCellValue(r["PAYMENT_REFERENCE_NO_BANK"].ToString());

                HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                Cell20.SetCellValue(r["remarks"].ToString());


            }
            string filename = "Miro_policy_issued_payment_comparison_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);


            Response.BinaryWrite(file.GetBuffer());

            Response.End();
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["SS_DATA"] != null)
        {
            gv_valid.PageIndex = e.NewPageIndex;

            DataTable tbl = (DataTable)Session["SS_DATA"];

            gv_valid.DataSource = tbl;
            gv_valid.DataBind();
        }

    }
    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'; ");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

            var obj = e.Row;
            Label lbl = (Label)obj.FindControl("lblRemarks");
            Label lblAmount = (Label)obj.FindControl("lblAmount");
            Label lblPaidAmount = (Label)obj.FindControl("lblPaidAmount");
            Label lblIssuedDate = (Label)obj.FindControl("lblIssuedDate");
            Label lblTranDate = (Label)obj.FindControl("lblTransactionDate");

            lblAmount.Text = Convert.ToDouble(lblAmount.Text) == 0 ? "" : lblAmount.Text;
            lblPaidAmount.Text = Convert.ToDouble(lblPaidAmount.Text) == 0 ? "" : lblPaidAmount.Text;
            lblIssuedDate.Text = lblIssuedDate.Text == "01-01-1900" ? "" : lblIssuedDate.Text;
            lblTranDate.Text = lblTranDate.Text == "01-01-1900" ? "" : lblTranDate.Text;
            if (lbl.Text.Trim().ToUpper() == "YES")
            {

                lbl.ForeColor = System.Drawing.Color.White;
                lbl.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                lbl.ForeColor = System.Drawing.Color.White;
                lbl.BackColor = System.Drawing.Color.Red;
            }

        }
    }
    protected void gv_valid_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable tbl = (DataTable)Session["SS_DATA"];
        if (Session["SORTEX"] + "" == "ASC")
        {
            Session["SORTEX"] = "DESC";
        }
        else
        {
            Session["SORTEX"] = "ASC";
        }
        Session["SORTCOL"] = e.SortExpression;


        string sortEx = "";
        string sortCol = "";
        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_valid.DataSource = dview;
        gv_valid.DataBind();

        //Data table after sorting
        Session["SS_DATA"] = dview.ToTable();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">Text message to show user</param>
    /// /// <param name="type">0=Success, 1=Error, 2=warning</param>
    void showMessage(string message, string type)
    {
        if (message.Trim() != "")
        {
            if (type == "0")
            {

                div_message.Attributes.CssStyle.Add("background-color", "#228B22");
            }
            else if (type == "1")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#f00");

            }
            else if (type == "2")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#ffcc00");
            }
            div_message.Attributes.CssStyle.Add("display", "block");
            div_message.InnerHtml = message;
        }
        else
        {
            div_message.Attributes.CssStyle.Add("display", "none");
        }
    }
}