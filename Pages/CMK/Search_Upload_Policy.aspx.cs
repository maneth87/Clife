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

public partial class Pages_CMK_Search_Upload_Policy : System.Web.UI.Page
{
    List<bl_cmk_policy_report> list_report_policy = new List<bl_cmk_policy_report>();
    DataTable tbl_result = new DataTable();
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

            if ((FileUploadSearchPolicy.PostedFile != null) && !string.IsNullOrEmpty(FileUploadSearchPolicy.PostedFile.FileName))
            {
                SearchUploadPolicy();
            }
            else
            {
                gvSearchResult.DataSource = tbl_result;
                gvSearchResult.DataBind();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        string message = "";

        //check sheet name
        if ((FileUploadSearchPolicy.PostedFile != null) && !string.IsNullOrEmpty(FileUploadSearchPolicy.PostedFile.FileName))
        {
            if (Validate(out message) != false)
            {
                SearchUploadPolicy();
            }
            else
            {
                AlertMessage("Please select your file.");
                return;
            }
        }
        else
        {
            AlertMessage("Please select your file.");
            return;
        }

    }

    void SearchUploadPolicy()
    {
        List<bl_cmk_policy_report> list_search_report = new List<bl_cmk_policy_report>();

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        BindUploadData();

        list_search_report = (List<bl_cmk_policy_report>)Session["upload_data"];

        if (list_search_report.Count > 0 || list_search_report != null)
        {
            // Add Items into List String
            List<string[, ,]> search_items_list = new List<string[, ,]>();
            string[, ,] condition = new string[,,] { };

            string customer_list_str = "", loan_id_list_str = "", certifi_list_str = "";

            for (int i = 0; i < list_search_report.Count; i++)
            {
                customer_list_str += customer_list_str.Trim() == "" ? list_search_report[i].CMK_Customer_ID.ToString() : "," + list_search_report[i].CMK_Customer_ID.ToString();
                loan_id_list_str += loan_id_list_str.Trim() == "" ? list_search_report[i].Loan_ID.ToString() : "," + list_search_report[i].Loan_ID.ToString();
                certifi_list_str += certifi_list_str.Trim() == "" ? list_search_report[i].Certificate_No.ToString() : "," + list_search_report[i].Certificate_No.ToString();
            }

            condition = new string[,,] { { { "CMK_CUSTOMER_ID", "IN", customer_list_str } } };
            search_items_list.Add(condition);

            condition = new string[,,] { { { "LOAN_ID", "IN", loan_id_list_str } } };
            search_items_list.Add(condition);

            condition = new string[,,] { { { "CERTIFICATE_NO", "IN", certifi_list_str } } };
            search_items_list.Add(condition);

            // PASSING DATA TO GET SEARCH RESULT
            tbl_result = da_cmk.Policy.GetPolicyReportListByUpload(search_items_list);
        }
        
        if (tbl_result.Rows.Count > 0)
        {
            #region TABLE

            gvSearchResult.DataSource = tbl_result;
            gvSearchResult.DataBind();

            ViewState["SEARCH_UPLOAD_POLICY"] = tbl_result;

            ////Calculate Sum and display in Footer Row
            //double total = tbl_result.AsEnumerable().Sum(row => row.Field<double>("Total_Premium"));
            //gvSearchResult.FooterRow.Cells[25].Text = "Grand Total";
            //gvSearchResult.FooterRow.Cells[25].Font.Bold = true;
            //gvSearchResult.FooterRow.Cells[25].HorizontalAlign = HorizontalAlign.Center;
            //gvSearchResult.FooterRow.Cells[26].ForeColor = System.Drawing.Color.Red;
            //gvSearchResult.FooterRow.Cells[26].Font.Bold = true;
            //gvSearchResult.FooterRow.Cells[26].HorizontalAlign = HorizontalAlign.Center;
            //gvSearchResult.FooterRow.Cells[26].Text = total.ToString("N2");

            #endregion
        }

        lblCount.Text = tbl_result.Rows.Count + " Policy(S) found.";
        
    }

    void BindUploadData()
    {
        string save_path = "~/Upload/";
        string file_name = Path.GetFileName(FileUploadSearchPolicy.PostedFile.FileName);
        string extension = Path.GetExtension(file_name);
        file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
        string file_path = save_path + file_name;
        int row_number = 0;

        FileUploadSearchPolicy.SaveAs(Server.MapPath(file_path));//save file 
        ExcelConnection my_excel = new ExcelConnection();
        my_excel.FileName = Server.MapPath(file_path);
        my_excel.CommandText = "SELECT * FROM [Upload_Format$]";
        DataTable my_data = new DataTable();
        my_data = my_excel.GetData();

        ViewState["my_data"] = my_data;
        row_number = my_data.Rows.Count;
        //Short Loan_ID and Customer_ID

        List<bl_cmk_policy_report> upload_list = new List<bl_cmk_policy_report>();

        foreach (DataRow row in my_data.Rows)
        {
            if (row[1].ToString().Trim() != "" && row[2].ToString().Trim() != "" && row[3].ToString().Trim() != "")
            {
                if (validRow(row, my_data.Rows.IndexOf(row)))
                {
                    bl_cmk_policy_report upload_data = new bl_cmk_policy_report();

                    //Policy Props
                    upload_data.CMK_Customer_ID = row[1].ToString().Trim();
                    upload_data.Loan_ID = row[2].ToString();
                    upload_data.Certificate_No = row[3].ToString();

                    upload_list.Add(upload_data);
                }

            }

        }

        Session["upload_data"] = upload_list;

    }

    //Row Index Changing
    protected void gvSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSearchResult.PageIndex = e.NewPageIndex;

        DataTable tbl;
        tbl = (DataTable)ViewState["SEARCH_UPLOAD_POLICY"];

        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gvSearchResult.DataSource = dview;
        gvSearchResult.DataBind();

        /*show record count*/
        lblCount.Text = "Record Display: " + gvSearchResult.Rows.Count + " Of " + tbl.Rows.Count;

    }

    bool Validate(out string message)
    {
        bool status = true;
        message = "";
        string file_path = "";
        //check sheet name
        if ((FileUploadSearchPolicy.PostedFile != null) && !string.IsNullOrEmpty(FileUploadSearchPolicy.PostedFile.FileName))
        {
            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(FileUploadSearchPolicy.PostedFile.FileName);
            string extension = Path.GetExtension(file_name);
            file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
            file_path = save_path + file_name;

            FileUploadSearchPolicy.SaveAs(Server.MapPath(file_path));//save file 

            ExcelConnection my_excel = new ExcelConnection();
            my_excel.FileName = Server.MapPath(file_path);

            if (my_excel.GetSheetName() != "Upload_Format$")
            {
                message = "File is not correct format, please donwload file template from the system.";
            }
            else
            {
                my_excel.CommandText = "Select * from [Upload_Format$]";
                DataTable tbl = my_excel.GetData();
                int col_count = 0;
                col_count = tbl.Columns.Count;
                if (col_count > 5 || col_count < 0)//check number of columns
                {
                    message = "File is not correct format, please donwload file template from the system.";
                }
                else
                {//check column name.
                    if (tbl.Columns[0].ColumnName.Trim() != "No#" || tbl.Columns[2].ColumnName.Trim() != "Customer ID" || tbl.Columns[3].ColumnName.Trim() != "Loan ID" || tbl.Columns[4].ColumnName.Trim() != "Certificate No")
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                    }
                }

            }
        }
        //delete file
        File.Delete(Server.MapPath(file_path));
        return status;
    }

    bool validRow(DataRow row, int row_index)
    {
        bool valid = true;

        Invalidation invalid_data = new Invalidation();
        try
        {
            #region //Check invalid data
            if (row[1].ToString().Trim() == "") //CMK Customer ID
            {
                AlertMessage("Customer ID is required.");
            }
            else if (row[2].ToString().Trim() == "") //Loan ID
            {
                AlertMessage("Loan ID is required.");
            }
            else if (row[3].ToString().Trim() == "") // Certificate No
            {
               AlertMessage("Certificate No is required.");
            }

            #endregion
        }
        catch (Exception ex)
        {
            valid = false;
            Log.AddExceptionToLog("Error function [validRow(DataRow row)] in class [Search_Upload_Policy.aspx.cs], row index [" + row_index + "], detail:" + ex.Message);
        }
        return valid;
    }

    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
        return;
    }

    void ExportExcel()
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            DataTable tbl = (DataTable)ViewState["SEARCH_UPLOAD_POLICY"];
            row_count = tbl.Rows.Count;
            if (row_count > 0)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheets = (HSSFSheet)hssfworkbook.CreateSheet("Search Monthly Upload Report");

                Helper.excel.Sheet = sheets;
                //design row header
                Helper.excel.HeaderText = new string[] {
                                                            "No", "Branch","Customer ID", "Loan ID", "Certificate No", "Last Name", "First Name", "DOB", "Age", "Gender",  "Product",
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

                    string Customer_ID = row[1].ToString().Trim();
                    string Last_Name = row[3].ToString().Trim();
                    string First_Name = row[4].ToString().Trim();
                    string Gender = row[5].ToString().Trim();
                    string DOB = Convert.ToDateTime(row[6]).ToString("dd-MM-yyyy").Trim();
                    string Certificate_No = row[7].ToString().Trim();
                    string Loan_ID = row[8].ToString().Trim();
                    string Group = row[10].ToString().Trim();
                    string Product = row[11].ToString().Trim();
                    string Opened_Date = Convert.ToDateTime(row[12]).ToString("dd-MM-yyyy").Trim();
                    string Effective_Date = Convert.ToDateTime(row[13]).ToString("dd-MM-yyyy").Trim();
                    string Date_Of_Entry = Convert.ToDateTime(row[14]).ToString("dd-MM-yyyy").Trim();
                    string Currancy = row[16].ToString().Trim();
                    string Age = row[17].ToString().Trim();
                    string Duration = row[18].ToString().Trim();
                    string Cover_Year = row[19].ToString().Trim();
                    string remarks = row[22].ToString().Trim();
                    string Branch = row[23].ToString().Trim();
                    double Amount = Convert.ToDouble(row[26].ToString().Trim());
                    double Outstanding_Balance = Convert.ToDouble(row[27].ToString().Trim());
                    double Assured_Amount = Convert.ToDouble(row[28].ToString().Trim());
                    string Policy_Status = row[32].ToString().Trim();
                    string Payment_Mode = row[33].ToString().Trim();
                    double Monthly_Premium = Convert.ToDouble(row[34].ToString().Trim());
                    double Extra_Premium = Convert.ToDouble(row[35].ToString().Trim());
                    double Premium_After_Discount = Convert.ToDouble(row[37].ToString().Trim());
                    double Total_Premium = Convert.ToDouble(row[38].ToString().Trim());
                    string Report_Date = Convert.ToDateTime(row[39]).ToString("dd-MM-yyyy").Trim();
                    string Paid_Off_In_Month = Convert.ToDateTime(row[40].ToString().Trim()) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(row[40]).ToString("MM-yyyy").Trim() : "-";
                    string Terminate_Date = Convert.ToDateTime(row[41].ToString().Trim()) != new DateTime(1900, 1, 01) ? Convert.ToDateTime(row[41]).ToString("dd-MM-yyyy").Trim() : "-";


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
                    Cell4.SetCellValue(Loan_ID);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(Certificate_No);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(Last_Name);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(First_Name);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(DOB);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(Age);

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Gender);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Product);

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

                //Calculate Sum and display in Footer Row
                double total = tbl_result.AsEnumerable().Sum(row => row.Field<double>("Total_Premium"));

                ICell footerCell = hssfworkbook.GetSheetAt(0).CreateRow(row_count + 1).CreateCell(26);
                footerCell.SetCellValue(total.ToString("N2"));
                font.Boldweight = (short)FontBoldWeight.BOLD;
                footerCell.CellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RED.index;
                style.SetFont(font);
                footerCell.CellStyle = style;

                string filename = "UPLOAD_SEARCH_MONTHLY_REPORT_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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

    class Invalidation : bl_cmk_load_data
    {
        public string Message { get; set; }
    }
}