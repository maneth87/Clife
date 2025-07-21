using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CL24_Renewal_Premium_List : System.Web.UI.Page
{
    DataTable list_renewal_prem = new DataTable();
    string message = "";
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

            for (int i = 0; i <= 10; i++)
            {
                ListItem list = new ListItem(); string year = " year"; string[] ordinal_number = {"st", "nd", "rd", "th" };

                if (i != 0)
                {
                    switch (i)
                    {
                        case 1 :
                            list.Text = i.ToString() + ordinal_number[0] + year;
                            list.Value = list.Text;
                            break;
                        case 2:
                            list.Text = i.ToString() + ordinal_number[1] + year;
                            list.Value = list.Text;
                            break;
                        case 3:
                            list.Text = i.ToString() + ordinal_number[2] + year;
                            list.Value = list.Text;
                            break;
                        case 4:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 5:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 6:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 7:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 8:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 9:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 10:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 11:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;
                        case 12:
                            list.Text = i.ToString() + ordinal_number[3] + year;
                            list.Value = list.Text;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    list.Text = "-- Select --";
                    list.Value = "";
                }

                ddlPolicyYearSearch.Items.Add(list);
            }

            GetData(false);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetData(true);
    }

    void GetData(bool get_data_type)
    {
        if (get_data_type == true)
        {
            if (ddlPolicyYearSearch.Text != "" && txtPolicyNumberSearch.Text != "")
            {
                list_renewal_prem = da_policy_cl24.GetRenewalPremiumByPolicyYearList(ddlPolicyYearSearch.Text, txtPolicyNumberSearch.Text);
                txtPolicyNumberSearch.Text = ""; ddlPolicyYearSearch.Text = "";
            } 
            else if (ddlPolicyYearSearch.Text != "")
            {
                list_renewal_prem = da_policy_cl24.GetRenewalPremiumByPolicyYearList(ddlPolicyYearSearch.Text, "");
                txtPolicyNumberSearch.Text = ""; ddlPolicyYearSearch.Text = "";

            }
            else if (txtPolicyNumberSearch.Text != "")
            {
                list_renewal_prem = da_policy_cl24.GetRenewalPremiumByPolicyYearList("",txtPolicyNumberSearch.Text);
                txtPolicyNumberSearch.Text = ""; ddlPolicyYearSearch.Text = "";
            }
            else
            {
                message = "Please check your input!!!";
                list_renewal_prem = new DataTable();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alertMesg('" + message + "');", true);
            }

            gv_report.DataSource = list_renewal_prem;
            gv_report.DataBind();

            /*show record count*/
            lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + list_renewal_prem.Rows.Count;
            ViewState["RENEWAL_PREM_DATA"] = list_renewal_prem;
                
        }
        else // Load Page
        {
            list_renewal_prem = da_policy_cl24.GetRenewalPremiumByPolicyYearList("", "");

            gv_report.DataSource = list_renewal_prem;
            gv_report.DataBind();

            /*show record count*/
            lblCount.Text = "Record Display: " + gv_report.Rows.Count + " Of " + list_renewal_prem.Rows.Count;
            ViewState["RENEWAL_PREM_DATA"] = list_renewal_prem;
        }
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

    void ExportExcel()
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            DataTable tbl = (DataTable)ViewState["RENEWAL_PREM_DATA"];
            row_count = tbl.Rows.Count;
            if (row_count > 0)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheets = (HSSFSheet)hssfworkbook.CreateSheet("Renewal Premium");

                Helper.excel.Sheet = sheets;
                //design row header
                Helper.excel.HeaderText = new string[] {
                                                            "No", "Policy Number","Insured Name", "Insurance Plan", "Start Date", "End Date", "Sum Insure", "Payment Mode", "Total Premium", "Effective Date", "Issue Date",
                                                            "Renewal Year", "Last Due Date", "Due Date", "Report Date", "Created By"
                                                        };
                Helper.excel.generateHeader();

                // Set Font Bold and Color
                HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();

                IFont font = hssfworkbook.CreateFont();

                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DARK_GREEN.index;

                for (int i = 0; i <= 15; i++)
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
                    string Policy_Number = row[1].ToString().Trim();
                    string Insured_Name = row[3].ToString().Trim();
                    string Insurance_Plan = row[14].ToString().Trim();
                    string Start_Date = Convert.ToDateTime(row[7]).ToString("dd-MM-yyyy").Trim();
                    string End_Date = Convert.ToDateTime(row[8]).ToString("dd-MM-yyyy").Trim();
                    string Sum_Insure = row[9].ToString().Trim();
                    string Payment_Mode = row[24].ToString().Trim();
                    string Total_Premium = row[26].ToString().Trim();
                    string Effective_Date = Convert.ToDateTime(row[10]).ToString("dd-MM-yyyy").Trim();
                    string Issue_Date = Convert.ToDateTime(row[11]).ToString("dd-MM-yyyy").Trim();
                    string Due_Date = Convert.ToDateTime(row[17]).ToString("dd-MM-yyyy").Trim();
                    string Renewal_Year = row[22].ToString().Trim();
                    string Last_Due_Date = Convert.ToDateTime(row[12]).ToString("dd-MM-yyyy").Trim();
                    string Report_Date = Convert.ToDateTime(row[35]).ToString("dd-MM-yyyy").Trim();
                    string Created_By = row[31].ToString().Trim();

                    #endregion

                    #region set value
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheets.CreateRow(row_no);
                    rowCell.RowStyle = style;

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(Policy_Number);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(Insured_Name);

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(Insurance_Plan);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(Start_Date);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(End_Date);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(Sum_Insure);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(Payment_Mode);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(Total_Premium);

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Effective_Date);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Issue_Date);

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(Due_Date);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(Renewal_Year);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(Last_Due_Date);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(Report_Date);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(Created_By);

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
        }
    }

    protected void export_excel_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }

}