using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Wing_report_wing_account : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected string GetIDType(string id_type)
    {
        
        string return_type = "";
        
        switch (id_type)
        {
            case "0":
                return_type = "ID Card";
                break;

            case "1":
                return_type = "Passport";
                break;

            case "2":
                return_type = "Visa";
                break;
            case "3":
                return_type = "Birth Certificate";
                break;


        }

        return return_type;

    }

    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        gvWingAccount.DataBind();
    }
    protected void ImgExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filename = "Camlife_WING_Account_" + DateTime.Now.Date + ".xls";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Charset = "";
            Response.Clear();

            // If you want the option to open the Excel file without saving than comment out the line below
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Excel 2003
            Response.ContentType = "application/vnd.xls";

            //Excel 2007
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            //Apply style to header
            for (int i = 0; i < gvWingAccount.HeaderRow.Cells.Count; i++)
            {
                gvWingAccount.HeaderRow.Cells[i].Style.Add("background-color", "#f2f2f2");
                gvWingAccount.HeaderRow.Cells[i].Style.Add("text-transform", "uppercase");
            }

            //Apply number format and text format to each cell
            foreach (GridViewRow r in gvWingAccount.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    for (int columnIndex = 0; columnIndex < r.Cells.Count; columnIndex++)
                    {
                        r.Cells[columnIndex].Style.Add("mso-number-format", @"\@");
                            
                    }
                }
            }

            gvWingAccount.RenderControl(htmlWrite);

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

    protected void btnSearchWING_Click(object sender, EventArgs e)
    {

        try
        {
            string policy_number = txtPolicyNumberSearch.Text;
            string customer_name = txtSurnameSearch.Text.ToUpper() + " " + txtFirstnameSearch.Text.ToUpper();
           
            string wing_sk = txtWingSK.Text.Trim();
            string wing_number = txtWingNo.Text.Trim();

            
            //DateTime effective_date = Convert.ToDateTime(txtDateEffectiveDate.Text, dtfi); ////To be deleted after inserting data

            if (policy_number != "") //Search by policy number 
            {              
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.Policy_Number = " + policy_number;
                
                //Clear
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtWingSK.Text = "";
                txtWingNo.Text = "";
                txtSearchToDate.Text = "";
                txtSearchFromDate.Text = "";
            }
            else if (wing_sk != "") //Search by wing serial key 
            {               
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.WING_SK = " + wing_sk;

                //Clear
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtWingSK.Text = "";
                txtWingNo.Text = "";
                txtSearchToDate.Text = "";
                txtSearchFromDate.Text = "";
            }
            else if (wing_number != "") //Search by wing number
            {
                
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.WING_Number = " + wing_number;

                //Clear
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtWingSK.Text = "";
                txtWingNo.Text = "";
                txtSearchToDate.Text = "";
                txtSearchFromDate.Text = "";
            }
           

            else if (txtSearchToDate.Text != "") //Search by date
            {
                //Set date format for SQL Server
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd/MM/yyyy";
                dtfi.DateSeparator = "/";

                DateTime from_date = Convert.ToDateTime(txtSearchFromDate.Text, dtfi);
                DateTime to_date = Convert.ToDateTime(txtSearchToDate.Text, dtfi);


                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.Created_On BETWEEN '" + from_date + "' AND '" + to_date + "' ORDER BY Created_On DESC, Ct_Wing_Account.SK DESC";

                //Clear
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtWingSK.Text = "";
                txtWingNo.Text = "";
                txtSearchToDate.Text = "";
                txtSearchFromDate.Text = "";
            
            }

            else if (customer_name != "") //Search by customer name
            {
                WINGAccountDataSource.SelectCommand = "SELECT Ct_Policy_Wing.*, Ct_Wing_Account.Date_Request FROM Ct_Policy_Wing INNER JOIN Ct_Wing_Account ON Ct_Policy_Wing.WING_SK = Ct_Wing_Account.SK WHERE Ct_Policy_Wing.Customer_Name LIKE '%" + customer_name + "%' ORDER BY Created_On DESC, Ct_Wing_Account.SK DESC";

                //Clear
                txtFirstnameSearch.Text = "";
                txtSurnameSearch.Text = "";
                txtPolicyNumberSearch.Text = "";
                txtWingSK.Text = "";
                txtWingNo.Text = "";
                txtSearchToDate.Text = "";
                txtSearchFromDate.Text = "";

            }

            

            gvWingAccount.DataBind();

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [btnSearchWING_Click] in page [policy_printing.aspx.cs]. Details: " + ex.Message);
        }
    }

    protected void gvWingAccount_RowDataBound(object sender, GridViewRowEventArgs e)
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