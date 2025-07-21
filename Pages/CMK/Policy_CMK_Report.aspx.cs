using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CMK_Policy_CMK_Report : System.Web.UI.Page
{
    List<bl_cmk_policy_report> list_report_policy = new List<bl_cmk_policy_report>();
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

            if (txtFrom_date.Text == "" && txtTo_date.Text == "")
            {
                ReportPolicy(true);
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        if (txtFrom_date.Text == "" || txtTo_date.Text == "")
        {
            gv_report.DataSource = null;
            gv_report.DataBind();
            ReportPolicy(false);
            txtFrom_date.Text = "";
            txtTo_date.Text = "";
            lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + 0;
        }
        else
        {
            if (DateTime.Parse(txtFrom_date.Text, dtfi).Date > DateTime.Parse(txtTo_date.Text, dtfi).Date)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('From Date (" + txtFrom_date.Text + ") cannot be bigger than To Date (" + txtTo_date.Text + "), please check it again.')", true);
                txtFrom_date.Text = "";
                txtTo_date.Text = "";
            }
            else
            {
                ReportPolicy(false);
                txtFrom_date.Text = "";
                txtTo_date.Text = "";
                txtCertificateNumberSearch.Text = "";
                txtLastnameSearch.Text = "";
                txtFirstnameSearch.Text = "";
            }
        }

    }

    void ReportPolicy(bool load)
    {
        //Clear report div
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        gv_report.DataSource = null;
        gv_report.DataBind();

        DataTable tbl = new DataTable();
        Report_Type.Text = "Policy Report ";

        if (load != true)
        {
            if (txtFrom_date.Text != "" && txtTo_date.Text != "" && (RdbSearchBy.SelectedValue == "1" || RdbSearchBy.SelectedValue == "2"))
            {
                txtCertificateNumberSearch.Text = ""; txtLastnameSearch.Text = ""; txtFirstnameSearch.Text = "";


                if (chkInforceInMonth.Checked)
                {
                    list_report_policy = da_cmk.Policy.GetPolicyReportListInMonth(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), Convert.ToInt32(rbtnlPolicyStatus.SelectedValue));
                    Report_Type.Text = "Report In Month ";
                }
                else
                {
                    list_report_policy = da_cmk.Policy.GetPolicyReportListByParams(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), Convert.ToInt32(RdbSearchBy.SelectedValue), Convert.ToInt32(rbtnlPolicyStatus.SelectedValue));
                }
            }
            else if (txtFrom_date.Text != "" && txtTo_date.Text != "" && (RdbSearchBy.SelectedValue == "3"))
            {
                txtCertificateNumberSearch.Text = ""; txtLastnameSearch.Text = ""; txtFirstnameSearch.Text = "";
                list_report_policy = da_cmk.Policy.GetPolicyLastReportByParams(DateTime.Parse(txtFrom_date.Text, dtfi), DateTime.Parse(txtTo_date.Text, dtfi), Convert.ToInt32(rbtnlPolicyStatus.SelectedValue));
            }
            else if (txtCertificateNumberSearch.Text != "")
            {
                txtFrom_date.Text = ""; txtTo_date.Text = ""; txtLastnameSearch.Text = ""; txtFirstnameSearch.Text = "";
                list_report_policy = da_cmk.Policy.GetPolicyReportListByParams(txtCertificateNumberSearch.Text, "", "");
            
            }
            else if (txtLastnameSearch.Text != "" || txtFirstnameSearch.Text != "")
            {
                txtFrom_date.Text = ""; txtTo_date.Text = ""; txtCertificateNumberSearch.Text = ""; 
                list_report_policy = da_cmk.Policy.GetPolicyReportListByParams("", txtLastnameSearch.Text, txtFirstnameSearch.Text);
            }
        }
        else
        {
            list_report_policy = da_cmk.Policy.GetPolicyReportListByParams(DateTime.Parse("01/01/1900", dtfi), DateTime.Parse("01/01/1900", dtfi), 1, Convert.ToInt32(rbtnlPolicyStatus.SelectedValue));
        }

        if (list_report_policy.Count > 0)
        {
            #region TABLE

            gv_report.DataSource = list_report_policy;
            gv_report.DataBind();

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(list_report_policy);
            tbl = JsonConvert.DeserializeObject<DataTable>(json);

            ViewState["CMK_POLICY_DATA"] = tbl;

            lblfrom.Visible = true;
            lblto.Visible = true;
            lblfrom.Text = txtFrom_date.Text;
            lblto.Text = txtTo_date.Text;

            /*show record count*/
            lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + tbl.Rows.Count;
            #endregion
        }
        else
        {
            lblfrom.Text = txtFrom_date.Text;
            lblto.Text = txtTo_date.Text;
            lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + tbl.Rows.Count;
        }

        txtCertificateNumberSearch.Text = ""; txtLastnameSearch.Text = ""; txtFirstnameSearch.Text = "";

    }

    //Row Index Changing
    protected void gv_report_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_report.PageIndex = e.NewPageIndex;

        DataTable tbl;
        tbl = (DataTable)ViewState["CMK_POLICY_DATA"];

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

    void ExportExcel()
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            DataTable tbl = (DataTable)ViewState["CMK_POLICY_DATA"];
            row_count = tbl.Rows.Count;
            if (row_count > 0)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheets = (HSSFSheet)hssfworkbook.CreateSheet("Monthly Upload Report");

                Helper.excel.Sheet = sheets;
                //design row header
                Helper.excel.HeaderText = new string[] {
                                                            "No", "Branch","Customer ID", "CMK Customer ID", "Loan ID", "Certificate No", "Last Name", "First Name", "DOB", "Age", "Gender",
                                                            "Opened Date", "Duration", "Amount", "Outstanding Balance", "Date Of Entry", "Cover Year", "Effective Date", "Policy Status",
                                                            "Assured Amount", "Currancy", "Group", "Payment Mode", "Monthly Premium", "Premium After Discount", "Extra Premium", "Total Premium", 
                                                            "Report Date", "Paid Off In Month", "Terminate Date", "Remarks"
                                                        };
                Helper.excel.generateHeader();

                // Set Font Bold and Color
                HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();

                IFont font = hssfworkbook.CreateFont();

                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DARK_GREEN.index;

                for (int i = 0; i <= 29; i++)
                {
                    ICell cell = hssfworkbook.GetSheetAt(0).GetRow(0).GetCell(i);
                    cell.CellStyle = style;
                    font.Boldweight = (short)FontBoldWeight.BOLD;

                    cell.CellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.AQUA.index;
                    style.SetFont(font);
                }

                foreach (DataRow row in tbl.Rows)
                {
                    #region VARIABLE 
                    string Customer_ID = row[0].ToString().Trim();
                    string First_Name = row[3].ToString().Trim();
                    string Last_Name = row[4].ToString().Trim();
                    string Gender = row[5].ToString().Trim();
                    string DOB = Convert.ToDateTime(row[6]).ToString("dd-MM-yyyy").Trim();
                    string CMK_Customer_ID = row[17].ToString().Trim();
                    string Certificate_No = row[18].ToString().Trim();
                    string Loan_ID = row[19].ToString().Trim();
                    string Group = row[21].ToString().Trim();
                    string Opened_Date = Convert.ToDateTime(row[23]).ToString("dd-MM-yyyy").Trim();
                    string Effective_Date = Convert.ToDateTime(row[24]).ToString("dd-MM-yyyy").Trim();
                    string Policy_Status = row[25].ToString().Trim();
                    string Date_Of_Entry = Convert.ToDateTime(row[26]).ToString("dd-MM-yyyy").Trim();
                    string Currancy = row[28].ToString().Trim();
                    string Age = row[29].ToString().Trim();
                    string Duration = row[30].ToString().Trim();
                    string Cover_Year = row[31].ToString().Trim();
                    string Branch = row[32].ToString().Trim();
                    string remarks = row[37].ToString().Trim();
                    double Amount = Convert.ToDouble(row[39].ToString().Trim());
                    double Outstanding_Balance = Convert.ToDouble(row[41].ToString().Trim());
                    double Assured_Amount = Convert.ToDouble(row[43].ToString().Trim());
                    double Monthly_Premium = Convert.ToDouble(row[45].ToString().Trim());
                    double Extra_Premium = Convert.ToDouble(row[46].ToString().Trim());
                    double Premium_After_Discount = Convert.ToDouble(row[48].ToString().Trim());
                    double Total_Premium = Convert.ToDouble(row[49].ToString().Trim());
                    string Report_Date = Convert.ToDateTime(row[50]).ToString("dd-MM-yyyy").Trim();
                    string Payment_Mode = row[51].ToString().Trim();
                    string Paid_Off_In_Month = Convert.ToDateTime(row[52].ToString().Trim()) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(row[52]).ToString("MM-yyyy").Trim() : "-";
                    string Terminate_Date = Convert.ToDateTime(row[53].ToString().Trim()) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(row[53]).ToString("dd-MM-yyyy").Trim() : "-"; 

                    #endregion

                    #region set value
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheets.CreateRow(row_no);
                    rowCell.RowStyle = style;

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(Branch);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(Customer_ID); 
                    
                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(CMK_Customer_ID);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(Loan_ID);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(Certificate_No);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(Last_Name);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(First_Name);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(DOB);

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Age);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Gender);

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(Opened_Date);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(Duration);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(Amount);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(Outstanding_Balance);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(Date_Of_Entry);

                    HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                    Cell17.SetCellValue(Cover_Year);

                    HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                    Cell18.SetCellValue(Effective_Date);

                    HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                    Cell19.SetCellValue(Policy_Status);

                    HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                    Cell20.SetCellValue(Assured_Amount);

                    HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);
                    Cell21.SetCellValue(Currancy);

                    HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(21);
                    Cell22.SetCellValue(Group);

                    HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(22);
                    Cell23.SetCellValue(Payment_Mode);

                    HSSFCell Cell24 = (HSSFCell)rowCell.CreateCell(23);
                    Cell24.SetCellValue(Monthly_Premium);

                    HSSFCell Cell25 = (HSSFCell)rowCell.CreateCell(24);
                    Cell25.SetCellValue(Premium_After_Discount);

                    HSSFCell Cell26 = (HSSFCell)rowCell.CreateCell(25);
                    Cell26.SetCellValue(Extra_Premium);

                    HSSFCell Cell27 = (HSSFCell)rowCell.CreateCell(26);
                    Cell27.SetCellValue(Total_Premium);

                    HSSFCell Cell28 = (HSSFCell)rowCell.CreateCell(27);
                    Cell28.SetCellValue(Report_Date);

                    HSSFCell Cell29 = (HSSFCell)rowCell.CreateCell(28);
                    Cell29.SetCellValue(Paid_Off_In_Month);

                    HSSFCell Cell30 = (HSSFCell)rowCell.CreateCell(29);
                    Cell30.SetCellValue(Terminate_Date);

                    HSSFCell Cell31 = (HSSFCell)rowCell.CreateCell(30);
                    Cell31.SetCellValue(remarks);

                    #endregion

                }

                string filename = "POLICY_REPORT_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                MemoryStream file = new MemoryStream();
                hssfworkbook.Write(file);

                Response.BinaryWrite(file.GetBuffer());

                Response.End();

            }

        }
        catch (Exception ex)
        {
            AlertMessage("Export data fail, please contact your system administrator.");
            Log.AddExceptionToLog("Error function [ExportExcel()] in page [Upload_Policy_Cmk.aspx.cs], detail:" + ex.Message);
        }
    }


    protected void export_excel_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }


    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }
}