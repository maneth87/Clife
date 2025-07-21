using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using System.Text;
using System.IO;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;

public partial class Pages_Reports_report_policy_complete : System.Web.UI.Page
{
    HSSFWorkbook hssfworkbook = new HSSFWorkbook();

    int check_form_load_or_search = 0, order_by = 0, payment_mode = 0;
    string policy_number = "", product_id = "";
    string status_code = "";

    //Public property for passing data to other pages
    #region
    public string Check_Form_Load_Or_Search
    {
        get
        {
            return hdfCheckFormLoadOrSearch.Value;
        }
    }

    public string Status_Code
    {
        get
        {
            return hdfPolicyStatus.Value;
        }
    }

    public string Order_By
    {
        get
        {
            return hdfOrderBy.Value;
        }
    }

    public string From_Date
    {
        get
        {
            return hdfFromDate.Value;
        }
    }

    public string To_Date
    {
        get
        {
            return hdfToDate.Value;
        }
    }

    public string Product
    {
        get
        {
            return hdfProduct.Value;
        }
    }

    public string Policy_Number
    {
        get
        {
            return hdfPolicyNum.Value;
        }
    }

    public string Chart_Type
    {
        get
        {
            return hdfChartType.Value;
        }
    }

    public string Chart_Data
    {
        get
        {
            return hdfChartData.Value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PolicyDataSource.SelectCommand = "";

            gvPolicy.DataBind();

            

        }

        lblNumberOfPolicy.Text = "Please filter your search...";
    }




    protected string GetNumberOfPayment(string policy_id, int policy_type_id, DateTime due_date)
    {
        int number_of_payment = da_policy.GetNumberOfPayment(policy_id, policy_type_id, due_date);
        return number_of_payment.ToString();
    }

    //Get extra amount
    protected string GetExtraPremium(string app_register_id)
    {
        string extra_premium = da_underwriting.GetEMAmount(app_register_id);
        return extra_premium;
    }

    //Get discount
    protected string GetDiscount(string app_register_id)
    {
        string discount = da_underwriting.GetDiscount(app_register_id);
        return discount.ToString();
    }

    //Get extra amount
    protected string GetTotalPremium(string app_register_id, string system_premium)
    {
        double total_premium = (Convert.ToDouble(system_premium) + Convert.ToDouble(GetExtraPremium(app_register_id)) - Convert.ToDouble(GetDiscount(app_register_id)));
        return String.Format("{0:N0}", total_premium);
    }

    protected void gvPolicy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


        }
    }

    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        gvPolicy.DataBind();
    }

    protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filename = "Complete_Policy_Report_" + DateTime.Now.Date + ".xls";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Charset = "";
            Response.Clear();

            // If you want the option to open the Excel file without saving than comment out the line below
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Excel 2003
            //Response.ContentType = "application/vnd.xls";

            //Excel 2007
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            //Apply style to header
            for (int i = 0; i < gvPolicy.HeaderRow.Cells.Count; i++)
            {
                gvPolicy.HeaderRow.Cells[i].Style.Add("background-color", "#f2f2f2");
                gvPolicy.HeaderRow.Cells[i].Style.Add("text-transform", "uppercase");
            }

            //Apply number format and text format to each cell
            foreach (GridViewRow r in gvPolicy.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    for (int columnIndex = 0; columnIndex < r.Cells.Count; columnIndex++)                        
                    {
                        if (columnIndex == 0 || columnIndex == 1)
                        {
                            r.Cells[columnIndex].Style.Add("mso-number-format", @"\@");
                        }
                        //else
                        //{
                        //    r.Cells[columnIndex].Style.Add("mso-number-format", "0");
                        //}                        
                    }
                }
            }

            gvPolicy.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());
            
            Response.End();
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('There is no data, please check it again.')", true);
        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        /* Verifies that the control is rendered */
    }

    

    protected void btnOk_Click(object sender, EventArgs e)
    {

        string order_by = "Policy_Number";

        //Get sort by parameter; default value is policy number
        if (RdbOrderBy.SelectedIndex > -1)
        {
            order_by = RdbOrderBy.SelectedItem.Value;
        }
        
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy HHMMSS";
        
        dtfi.DateSeparator = "/";
        
        TimeSpan start_time = new TimeSpan(00, 00, 00);
        TimeSpan end_time = new TimeSpan(23, 59, 59);

        //Create from_date and to_date searching param
        DateTime from_date_time = DateTime.Parse(txtFrom_date.Text, dtfi).Date + start_time;
        DateTime to_date_time = DateTime.Parse(txtTo_date.Text, dtfi).Date + end_time;

        //Get policy status
        string policy_status = hdfPolicy_Status_ID.Value;

        if (DateTime.Parse(txtFrom_date.Text, dtfi).Date > DateTime.Parse(txtTo_date.Text, dtfi).Date)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('From Date (" + txtFrom_date.Text + ") cannot be bigger than To Date (" + txtTo_date.Text + "), please check it again.')", true);
            txtFrom_date.Text = "";
            txtTo_date.Text = "";
            return;
        }
        else //Get gridview data based on selection date and product ID
        {
            switch (ddlProduct.SelectedValue) //select by product ID
            {
                case "MRTA": 
                case "MRTA12": 
                case "MRTA24": 
                case "MRTA36": 
                case "PP200": 
                case "PP15/10": 
                case "T3002": 
                case "T5002": 
                case "T10002": 
                case "T3":
                case "T5": 
                case "T10":
                case "T1011":
                case "W10": 
                case "W15":
                case "W20": 
                case "W9010": 
                case "W9015":
                case "W9020":
                case "FT013":
                    if (policy_status != "")
                    {
                        PolicyDataSource.SelectCommand = "SELECT * FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date_time + "' AND '" + to_date_time + "' AND Product_ID = '" + ddlProduct.SelectedValue + "' AND Status = '" + policy_status + "' ORDER BY " + order_by + " DESC";
                    }
                    else
                    {
                        PolicyDataSource.SelectCommand = "SELECT * FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date_time + "' AND '" + to_date_time + "' AND Product_ID = '" + ddlProduct.SelectedValue + "' ORDER BY " + order_by + " DESC";

                    }
                    break;
                case "-1": //select from_date and to_date only
                    if (policy_status != "")
                    {
                        PolicyDataSource.SelectCommand = "SELECT * FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date_time + "' AND '" + to_date_time + "' AND Status = '" + policy_status + "' ORDER BY " + order_by + " DESC";
                    }
                    else
                    {
                        PolicyDataSource.SelectCommand = "SELECT * FROM V_Policy_Complete WHERE Effective_Date BETWEEN '" + from_date_time + "' AND '" + to_date_time + "' ORDER BY "+ order_by +" DESC";       
                    }

                    break;
                default:
                    PolicyDataSource.SelectCommand = "";
                    break;
                    
            }            
        }

        gvPolicy.DataBind();

        lblNumberOfPolicy.Text = gvPolicy.Rows.Count.ToString() + " record(s) found!";

        //Set value to hidden fields for passing data to other pages
        hdfFromDate.Value = txtFrom_date.Text.Trim();
        hdfToDate.Value = txtTo_date.Text.Trim();
        hdfPolicyNum.Value = txtPolicyNumber.Text.Trim();

        if (check_form_load_or_search != 1)
        {
            hdfPolicyNum.Value = txtPolicyNumber.Text;
            hdfProduct.Value = ddlProduct.SelectedValue;
        }

        hdfOrderBy.Value = RdbOrderBy.SelectedValue;
        hdfPolicyStatus.Value = policy_status;
        hdfCheckFormLoadOrSearch.Value = check_form_load_or_search.ToString();
    }

    void CheckOrderBy()
    {
        if (RdbOrderBy.SelectedValue != "")
        {
            if (int.Parse(RdbOrderBy.SelectedValue) == 0)
            {
                order_by = 1;

            }
            else if (int.Parse(RdbOrderBy.SelectedValue) == 1)
            {
                order_by = 2;
            }
            else if (int.Parse(RdbOrderBy.SelectedValue) == 2)
            {
                order_by = 3;
            }
            else
            {
                order_by = 4;
            }
        }
    }

    //Get payment mode from Payment Mode table
    protected string GetPayMode(string paymode)
    {
        string payment_mode = da_underwriting.GetPaymentMode(paymode);
        return payment_mode;
    }

    //Calculate API
    protected string CalculateNPI(string premium, string no_of_payment) //NPI: Net Premium Income
    {
        double npi = Convert.ToDouble(premium) * Convert.ToDouble(no_of_payment);
        return npi.ToString();
    }

    //Calculate NPI
    protected string CalculateAPI(string premium, string paymode) //API: Annualized Premium Income
    {
        double api = 0;
        switch (paymode)
        {
            case "0":
                api = Convert.ToDouble(premium) * 1;
                break;
            case "1":
                api = Convert.ToDouble(premium) * 1;
                break;
            case "2":
                api = Convert.ToDouble(premium) * 2;
                break;
            case "3":
                api = Convert.ToDouble(premium) * 4;
                break;
            case "4":
                api = Convert.ToDouble(premium) * 12;
                break;

        }
        return api.ToString();
    }

    protected string GetDisplayName(string first_name, string last_name, string khmer_first_name, string khmer_last_name, string product_id)
    {
        string full_name = "";

        if (product_id == "T1011")
        {
            if (first_name == "" || last_name == "")
            {
                full_name = khmer_last_name + " " + khmer_first_name;
            }
            else if (first_name != "" || last_name != "" || khmer_first_name != "" || khmer_last_name != "")
            {
                full_name = last_name + " " + first_name;
            }

        }
        else
            full_name = last_name + " " + first_name;

        
        return full_name;
    }

    private MemoryStream WriteToStream()
    {
        //Write the stream data of workbook to the root directory
        MemoryStream file = new MemoryStream();
        hssfworkbook.Write(file);
        return file;
    }

    protected void btnSearchPolicy_Click(object sender, EventArgs e)
    {
        try
        {
            string policy_number = txtPolicyNumberSearch.Text;
            string surname = txtSurnameSearch.Text;
            string firstname = txtFirstnameSearch.Text;

            if (policy_number != "") //Search by policy number CONTAINS(Column, 'test')
            {
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                PolicyDataSource.SelectCommand = "SELECT * FROM V_Policy_Complete WHERE Policy_Status_Type_ID = 'IF' AND Policy_Number LIKE '%" + policy_number + "%' ORDER BY Effective_Date DESC";
            }
            else //Search by customer name
            {
                txtPolicyNumberSearch.Text = "";
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                PolicyDataSource.SelectCommand = "SELECT * FROM V_Policy_Complete WHERE Policy_Status_Type_ID = 'IF' AND First_Name LIKE '%" + firstname + "%' AND Last_Name LIKE '%" + surname + "%' ORDER BY Effective_Date DESC";
            }

            gvPolicy.DataBind();

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [ImageBtnSearch_Click] in page [policy_printing.aspx.cs]. Details: " + ex.Message);
        }
    }

    //Create Chart
    protected void btnCreateChart_Click(object sender, EventArgs e)
    {
        hdfChartType.Value = ddlType.SelectedValue;
        hdfChartData.Value = rbtnlData.SelectedValue;

        Server.Transfer("~/Pages/Chart/policy_complete_chart.aspx");
    }

}