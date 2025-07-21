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
using Excel = Microsoft.Office.Interop.Excel;

public partial class Pages_CL24_Premium_List : System.Web.UI.Page
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
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        DateTime fromDate, toDate, blank_date;
        blank_date = new DateTime(1900, 1, 01);

        if (get_data_type == true)
        {
            if (txtFrom_date.Text.Trim() != "" && txtTo_date.Text.Trim() != "")
            {
                fromDate = DateTime.Parse(txtFrom_date.Text, dtfi); toDate = DateTime.Parse(txtTo_date.Text, dtfi);
            }
            else
            {
                fromDate = new DateTime(1900, 1, 01); toDate = fromDate;
            }

            if (fromDate.Date != new DateTime(1900, 1, 01) && toDate.Date != new DateTime(1900, 1, 01))
            {
                if (rbtnlData.SelectedValue != "")
                {
                    list_renewal_prem = da_policy_cl24.GetPremiumByDateRangeList(fromDate, toDate, int.Parse(rbtnlData.SelectedValue));
                    txtPolicyNumberSearch.Text = ""; ddlPolicyYearSearch.Text = ""; txtFrom_date.Text = ""; txtTo_date.Text = ""; fromDate = blank_date; toDate = blank_date;
                }
                else
                {
                    message = "Date is required!!";
                    list_renewal_prem = new DataTable();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alertMesg('" + message + "');", true);
                    return;
                }
            }
            else if (ddlPolicyYearSearch.Text != "" && txtPolicyNumberSearch.Text != "")
            {
                list_renewal_prem = da_policy_cl24.GetRenewalPremiumByPolicyYearList(ddlPolicyYearSearch.Text, txtPolicyNumberSearch.Text);
                txtPolicyNumberSearch.Text = ""; ddlPolicyYearSearch.Text = ""; txtFrom_date.Text = ""; txtTo_date.Text = ""; fromDate = blank_date; toDate = blank_date;

            } 
            else if (ddlPolicyYearSearch.Text != "")
            {
                list_renewal_prem = da_policy_cl24.GetRenewalPremiumByPolicyYearList(ddlPolicyYearSearch.Text, "");
                txtPolicyNumberSearch.Text = ""; ddlPolicyYearSearch.Text = ""; txtFrom_date.Text = ""; txtTo_date.Text = ""; fromDate = blank_date; toDate = blank_date;

            }
            else if (txtPolicyNumberSearch.Text != "")
            {
                list_renewal_prem = da_policy_cl24.GetRenewalPremiumByPolicyYearList("",txtPolicyNumberSearch.Text);
                txtPolicyNumberSearch.Text = ""; ddlPolicyYearSearch.Text = ""; txtFrom_date.Text = ""; txtTo_date.Text = ""; fromDate = blank_date; toDate = blank_date;

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
                HSSFSheet sheets = (HSSFSheet)hssfworkbook.CreateSheet("Premium List");

                Helper.excel.Sheet = sheets;
                //design row header
                Helper.excel.HeaderText = new string[] {
                                                            "No", "Policy Number","Insured Name", "Insurance Plan", "Sum Insure", "Payment Mode", "Total Premium", "Effective Date", "Issue Date",
                                                            "Renewal Year", "Start Date", "End Date", "Last Due Date", "Next Due Date", "Report Date", "Created By"
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
                    string Next_Due_Date = Convert.ToDateTime(row[17]).ToString("dd-MM-yyyy").Trim();
                    string Renewal_Year = row[22].ToString().Trim();
                    string Last_Due_Date = Convert.ToDateTime(row[12]).ToString("dd-MM-yyyy").Trim();
                    string Report_Date = Convert.ToDateTime(row[36]).ToString("dd-MM-yyyy").Trim();
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
                    Cell5.SetCellValue(Sum_Insure);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(Payment_Mode);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(Total_Premium);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(Effective_Date);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(Issue_Date);

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Renewal_Year);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Start_Date);

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(End_Date);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(Last_Due_Date);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(Next_Due_Date);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(Report_Date);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(Created_By);

                    #endregion

                }

                string filename = "PREMIUM_LIST_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
        //if (rbtnlData.SelectedValue != "")
        //{
        //    if (rbtnlData.SelectedValue == "1")
        //    {
        //        DownloadExcelFile();
        //    }
        //    else if (rbtnlData.SelectedValue == "2")
        //    {
        //        ExportExcel();
        //    }
        //}
        //else
        //{
        //    ExportExcel();
        //}

        ExportExcel();
            
    }

    //void DownloadExcelFile()
    //{
    //    DataTable my_tbl = (DataTable)ViewState["RENEWAL_PREM_DATA"];

    //    if (my_tbl.Rows.Count > 0)
    //    {
    //        var app = new Excel.Application();

    //        app.Workbooks.Add();
    //        Excel._Worksheet workSheet = (Excel.Worksheet)app.ActiveSheet;

    //        DataTable table = new DataTable();

    //        #region DEFINE COLUMN
    //        // Create a DataTable with four columns.   
    //        string[] columnNames = new string[] { "No.", "Pol No.", "Insured Name", "Ins. Plan", "Start Date", "End Date", "SA ($)", "Mode Of Payment", "Basic Premium", "Total Premium", "Banks", 
    //                                              "Agent", "Agent Code", "Issue Date", "Policy Year", "Branch" };

    //        workSheet.Name = "FF Policyholder"; ;
    //        #endregion

    //        #region RESERVE VARIABLES
    //        int count = 1; int NewPolicy = 0, RenewalPolicy = 0; int manual_row_add_count = 0;
    //        float Total_Sum_Insure_New = 0, Total_Basic_Premium_New = 0, Total_Total_Premium_New = 0;
    //        float Total_Sum_Insure_Renew = 0, Total_Basic_Premium_Renew = 0, Total_Total_Premium_Renew = 0;
    //        #endregion

    //        for (int i = 0; i < columnNames.Length; i++)
    //        {
    //            table.Columns.Add(columnNames[i]);
    //        }

    //        //Add Header to worksheet with the Datatable 
    //        for (int i = 1; i < table.Columns.Count + 1; i++)
    //        {
    //            workSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
    //        }

    //        #region ADD NEW ROW
    //        //Add row to worksheet with the Datatable 
    //        foreach (DataRow row in my_tbl.Rows)
    //        {
    //            table.Rows.Add(count,
    //                           row["Policy_Number"].ToString(),
    //                           row["Insured_Name"].ToString(),
    //                           row["Product_Name"].ToString(),
    //                           row["Start_Date"].ToString(),
    //                           row["End_Date"].ToString(),
    //                           row["Sum_Insure"].ToString(),
    //                           row["Payment_Mode"].ToString(),
    //                           row["Premium"].ToString(),
    //                           row["Total_Premium"].ToString(),
    //                           "First Finance",
    //                           row["Agent_Code"].ToString(),
    //                           row["Agent_Name"].ToString(),
    //                           row["Issue_Date"].ToString(),
    //                           row["Policy_Year"].ToString(),
    //                           row["Branch"].ToString()
    //                           );

    //            count++;

    //        }
    //        #endregion

    //        #region NEW POLICY
    //        // Split Policy New
    //        int new_index = 0;
    //        for (int j = 0; j < table.Rows.Count; j++)
    //        {
    //            if (table.Rows[j].ItemArray[14].ToString() == "1st year") // NEW
    //            {
    //                NewPolicy++;
    //                new_index += 1; // ADD (1) = 1 row after header
    //                if (j == 0)
    //                {
    //                    new_index += 1;
    //                    manual_row_add_count++;
    //                    for (int k = 0; k < table.Columns.Count; k++)
    //                    {
    //                        workSheet.Cells[j + new_index, k + 1] = ""; // New Row for New Policy Label
    //                    }

    //                    var range = workSheet.Range[workSheet.Cells[j + new_index, j + 1], workSheet.Cells[j + new_index, table.Columns.Count]];
    //                    range.Merge();

    //                    workSheet.Cells[j + new_index, j + 1] = "I. New Policy In " + Convert.ToDateTime(my_tbl.Rows[my_tbl.Rows.Count - 1]["Report_Date"].ToString()).ToString("MMMM yyyy");
    //                    Excel.Font font1 = workSheet.Range[workSheet.Cells[j + new_index, j + 1], workSheet.Cells[j + new_index, table.Columns.Count]].Font;
    //                    font1.Bold = Excel.XlLineStyle.xlContinuous;

    //                }

    //                for (int k = 0; k < table.Columns.Count; k++) // WRITE NEW ROW TO EXCEL
    //                {
    //                    if (k == 4 || k == 5 || k == 13)
    //                    {
    //                        workSheet.Cells[new_index + manual_row_add_count, k + 1] = Convert.ToDateTime(table.Rows[j].ItemArray[k].ToString()).ToString("dd/MMM/yyyy");
    //                    }
    //                    else
    //                    {
    //                        workSheet.Cells[new_index + manual_row_add_count, k + 1] = table.Rows[j].ItemArray[k].ToString();
    //                    }

    //                    // SUM Sum Insure Basic Premium and Total Premium
    //                    if (k == 6) // TOTAL Sum Insure
    //                    {
    //                        Total_Sum_Insure_New += table.Rows[j].ItemArray[k].ToString() != "" ? float.Parse(table.Rows[j].ItemArray[k].ToString()) : 0;
    //                    }
    //                    else if (k == 8) // TOTAL Basic Premium
    //                    {
    //                        Total_Basic_Premium_New += table.Rows[j].ItemArray[k].ToString() != "" ? float.Parse(table.Rows[j].ItemArray[k].ToString()) : 0;
    //                    }
    //                    else if (k == 9) // TOTAL Total Premium
    //                    {
    //                        Total_Total_Premium_New += table.Rows[j].ItemArray[k].ToString() != "" ? float.Parse(table.Rows[j].ItemArray[k].ToString()) : 0;
    //                    }
    //                }

    //            }
    //            else
    //            {
    //                RenewalPolicy++;
    //            }
    //        }

    //        if (NewPolicy > 0) // SUM COLUMN
    //        {
    //            manual_row_add_count++; // manual_row_add_count INCREASE 1 CUZ 1 BLANK ROW AFTER NEW_POLICY
    //            new_index += manual_row_add_count;
    //            for (int k = 0; k < table.Columns.Count; k++) // 
    //            {
    //                if (k == 6) // TOTAL Sum Insure
    //                {
    //                    workSheet.Cells[new_index + 1, k + 1] = Total_Sum_Insure_New.ToString();
    //                }
    //                else if (k == 8) // TOTAL Basic Premium
    //                {
    //                    workSheet.Cells[new_index + 1, k + 1] = Total_Basic_Premium_New.ToString();
    //                }
    //                else if (k == 9) // TOTAL Total Premium
    //                {
    //                    workSheet.Cells[new_index + 1, k + 1] = Total_Total_Premium_New.ToString();
    //                }
    //                else
    //                {
    //                    workSheet.Cells[new_index + 1, k + 1] = "";
    //                }
    //            }

    //            var rangeTotal = workSheet.Range[workSheet.Cells[new_index + 1, 1], workSheet.Cells[new_index + 1, 6]];
    //            rangeTotal.Merge();

    //            workSheet.Cells[new_index + 1, 1] = "TOTAL";
    //            Excel.Font myBold1 = workSheet.Range[workSheet.Cells[new_index + 1, 1], workSheet.Cells[new_index + 1, table.Columns.Count]].Font;
    //            myBold1.Bold = Excel.XlLineStyle.xlContinuous;

    //            workSheet.Range[workSheet.Cells[new_index + 1, 1], workSheet.Cells[new_index + 1, table.Columns.Count]].Interior.Color = Excel.XlRgbColor.rgbDarkOrange;
    //        }
    //        #endregion

    //        #region RENEWAL POLICY

    //        // Split Policy Renewal
    //        int renew_index = 0;
    //        if (RenewalPolicy > 0)
    //        {
    //            renew_index = new_index + manual_row_add_count;
    //            if (renew_index == 0)
    //            {
    //                renew_index = 2;
    //            }

    //            for (int k = 0; k < table.Columns.Count; k++)
    //            {
    //                workSheet.Cells[renew_index, k + 1] = "";
    //            }

    //            var range = workSheet.Range[workSheet.Cells[renew_index, 1], workSheet.Cells[renew_index, table.Columns.Count]];
    //            range.Merge();

    //            workSheet.Cells[renew_index, 1] = "II. Renewal Policy In " + Convert.ToDateTime(my_tbl.Rows[my_tbl.Rows.Count - 1]["Report_Date"].ToString()).ToString("MMMM yyyy");
    //            Excel.Font font1 = workSheet.Range[workSheet.Cells[renew_index, 1], workSheet.Cells[renew_index, table.Columns.Count]].Font;
    //            font1.Bold = Excel.XlLineStyle.xlContinuous;

    //            for (int j = 0; j < table.Rows.Count; j++)
    //            {
    //                if (table.Rows[j].ItemArray[14].ToString() != "1st year") // RENEWAL
    //                {
    //                    renew_index++;
    //                    for (int k = 0; k < table.Columns.Count; k++)
    //                    {
    //                        if (k == 4 || k == 5 || k == 13)
    //                        {
    //                            workSheet.Cells[renew_index, k + 1] = Convert.ToDateTime(table.Rows[j].ItemArray[k].ToString()).ToString("dd/MMM/yyyy");
    //                        }
    //                        else
    //                        {
    //                            workSheet.Cells[renew_index, k + 1] = table.Rows[j].ItemArray[k].ToString();
    //                        }

    //                        // SUM Sum Insure Basic Premium and Total Premium
    //                        if (k == 6) // TOTAL Sum Insure
    //                        {
    //                            Total_Sum_Insure_Renew += table.Rows[j].ItemArray[k].ToString() != "" ? float.Parse(table.Rows[j].ItemArray[k].ToString()) : 0;
    //                        }
    //                        else if (k == 8) // TOTAL Basic Premium
    //                        {
    //                            Total_Basic_Premium_Renew += table.Rows[j].ItemArray[k].ToString() != "" ? float.Parse(table.Rows[j].ItemArray[k].ToString()) : 0;
    //                        }
    //                        else if (k == 9) // TOTAL Total Premium
    //                        {
    //                            Total_Total_Premium_Renew += table.Rows[j].ItemArray[k].ToString() != "" ? float.Parse(table.Rows[j].ItemArray[k].ToString()) : 0;
    //                        }
    //                    }

    //                }
    //            }

    //            manual_row_add_count++;  // add 1 row blank after renewal_policy

    //            for (int k = 0; k < table.Columns.Count; k++)
    //            {
    //                if (k == 6) // TOTAL Sum Insure
    //                {
    //                    workSheet.Cells[renew_index + 2, k + 1] = Total_Sum_Insure_Renew.ToString();
    //                }
    //                else if (k == 8) // TOTAL Basic Premium
    //                {
    //                    workSheet.Cells[renew_index + 2, k + 1] = Total_Basic_Premium_Renew.ToString();
    //                }
    //                else if (k == 9) // TOTAL Total Premium
    //                {
    //                    workSheet.Cells[renew_index + 2, k + 1] = Total_Total_Premium_Renew.ToString();
    //                }
    //                else
    //                {
    //                    workSheet.Cells[renew_index + 2, k + 1] = "";
    //                }

    //                workSheet.Cells[renew_index + 3, k + 1] = ""; // Grand Total
    //            }

    //            var rangeTotal = workSheet.Range[workSheet.Cells[renew_index + 2, 1], workSheet.Cells[renew_index + 2, 6]];
    //            rangeTotal.Merge();

    //            workSheet.Cells[renew_index + 2, 1] = "TOTAL";
    //            Excel.Font myBold1 = workSheet.Range[workSheet.Cells[renew_index + 2, 1], workSheet.Cells[renew_index + 2, table.Columns.Count]].Font;
    //            myBold1.Bold = Excel.XlLineStyle.xlContinuous;

    //            workSheet.Range[workSheet.Cells[renew_index + 2, 1], workSheet.Cells[renew_index + 2, table.Columns.Count]].Interior.Color = Excel.XlRgbColor.rgbDarkOrange;
    //        }

    //        #endregion

    //        #region GRAND TOTAL PREMIUM
    //        if (NewPolicy > 0 && RenewalPolicy > 0)
    //        {
    //            manual_row_add_count = manual_row_add_count + 4;
    //            var rangeGrandTotal = workSheet.Range[workSheet.Cells[renew_index + 3, 1], workSheet.Cells[renew_index + 3, 9]];
    //            rangeGrandTotal.Merge();
    //            workSheet.Cells[renew_index + 3, 1] = "TOTAL ALL PREMIUM";

    //            Excel.Font myBoldGrand = workSheet.Range[workSheet.Cells[renew_index + 3, 1], workSheet.Cells[renew_index + 3, table.Columns.Count]].Font;
    //            myBoldGrand.Bold = Excel.XlLineStyle.xlContinuous;

    //            workSheet.Cells[renew_index + 3, 10] = (Total_Total_Premium_New + Total_Total_Premium_Renew).ToString();
    //        }
    //        else
    //        {
    //            manual_row_add_count += 2;
    //        }

    //        #endregion

    //        #region EXCEL DECORATION
    //        Excel.Range excelCellrange;
    //        Excel.Range excelCellHeader;

    //        // DECLARE VARIABLE
    //        excelCellHeader = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, table.Columns.Count]];
    //        excelCellrange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[count + manual_row_add_count, table.Columns.Count]];

    //        // HEADER FONT BOLD
    //        Excel.Font myBold = excelCellHeader.Font;
    //        myBold.Bold = Excel.XlLineStyle.xlContinuous;

    //        // TABLE STYLE
    //        excelCellrange.EntireColumn.AutoFit();
    //        Excel.Borders border = excelCellrange.Borders;
    //        border.LineStyle = Excel.XlLineStyle.xlContinuous;
    //        border.Weight = 2d;
    //        #endregion

    //        // OPEN EXCEL 
    //        app.Visible = true;
    //    }
    
    //}

}