using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;

public partial class Pages_Card_frmupload_g_card : System.Web.UI.Page
{
    private DataTable MyUploadData { get { return (DataTable)ViewState["VS_G_CARD_DATA"]; } set { ViewState["VS_G_CARD_DATA"] = value; } }
    private List<bl_g_card> ValidData { get { return (List<bl_g_card>)ViewState["VS_G_CARD_VALIDE_DATA"]; } set { ViewState["VS_G_CARD_VALIDE_DATA"] = value; } }
    private List<bl_g_card> InvalidData { get { return (List<bl_g_card>)ViewState["VS_G_CARD_INVALIDE_DATA"]; } set { ViewState["VS_G_CARD_INVALIDE_DATA"] = value; } }
    string Username = "";
    authentication auth = new authentication();
    protected void Page_Load(object sender, EventArgs e)
    {
       // string PageName = Path.GetFileName(Request.Url.AbsolutePath);
        Username = Membership.GetUser().UserName;
        lblError.Text = "";
        //if (!Page.IsPostBack)
        //{
        //     auth.RequestPage = PageName;
        //    if (!auth.Authorize)
        //    {
        //        Response.Redirect("../../unauthorize.aspx");
        //    }
        //}
        
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        // System.Threading.Thread.Sleep(50000);
        try
        {

            string message;
            if (Validate(out message) == true)
            {
                if (MyUploadData.Rows.Count > 0)
                {

                    foreach (DataRow r in MyUploadData.Rows)
                    {
                        da_g_card.SaveCardTemp(new bl_g_card()
                        {
                            CustomerNameKh = r[0].ToString(),
                            CustomerNameEn = r[1].ToString(),
                            DOB = Helper.FormatDateTime(r[2].ToString()).Date,
                            Gender = r[3].ToString(),
                            CertificateNumber = r[4].ToString(),
                            EffectiveDate = Helper.FormatDateTime(r[5].ToString()).Date,
                            ExpiryDate = Helper.FormatDateTime(r[6].ToString()).Date,
                            MaturityDate = Helper.FormatDateTime(r[7].ToString()).Date,
                            Status = r[8].ToString().Trim().ToUpper() == "ADD" ? 1 : r[8].ToString().Trim().ToUpper() == "DELETE" ? 0 : -1,
                            CreatedBy = Membership.GetUser().UserName,
                            CreatedOn = DateTime.Now,
                            Owner = r[1].ToString().Trim().Replace(' ', '.').ToLower()
                        });
                        if (da_g_card.TransactionStatus == false)
                        {
                            break;
                        }
                    }
                    if (da_g_card.TransactionStatus == true)
                    {
                        //get data
                        int CountValid = 0; int CountInvalid = 0;
                        List<bl_g_card> VList = da_g_card.GetCardListTempForInsertUpdate(Username, DateTime.Now.Date);
                        if (da_g_card.GetDataStatus == true)
                        {
                            CountValid = da_g_card.CountData;

                            ValidData = VList;
                            gv_valideList.DataSource = ValidData;
                            gv_valideList.DataBind();

                            List<bl_g_card> InVlist = da_g_card.GetCardListTempNotAllowInsertUpdate(Username, DateTime.Now.Date);
                            if (da_g_card.GetDataStatus == true)
                            {
                                CountInvalid = da_g_card.CountData;
                                gv_invalid.DataSource = InVlist;
                                gv_invalid.DataBind();
                            }

                            div_record_found.InnerText = "Total Data Upload = " + (CountValid + CountInvalid) + " | Data Valid = " + CountValid + " | Data Invalid = " + CountInvalid;

                            btnSave.Visible = CountValid > 0 ? true : false;
                            export_excel.Visible = CountInvalid > 0 ? true : false;
                            ClearTempData();

                            if (CountValid + CountInvalid == CountValid)
                            {
                                Helper.Alert(false, "All Data are valid.", lblError);
                            }
                            else if (CountValid + CountInvalid == CountInvalid)
                            {
                                Helper.Alert(true, "All Data are invalid. Please see invalid detail in [Invalid Data]", lblError);
                            }
                            else
                            {
                                Helper.Alert(true, "Some Data are valid and some data are invalid. Please see invalid detail in [Invalid Data]", lblError);
                            }
                        }
                        else
                        {

                            if (ClearTempData())
                            {
                                Helper.Alert(true, "Uploaded fail, system clear temporary data successfully.", lblError);
                            }
                            else
                            {
                                Helper.Alert(true, "Uploaded fail, system cannot clear temporary data successfully. Please contact your system administrator.", lblError);
                            }
                        }
                    }
                    else
                    {
                        // div_record_found.InnerText = da_g_card.Exception;
                        if (ClearTempData())
                        {
                            Helper.Alert(true, "Uploaded fail, system clear temporary data successfully.", lblError);
                        }
                        else
                        {

                            Helper.Alert(true, "Uploaded fail, system cannot clear temporary data successfully. Please contact your system administrator.", lblError);
                        }
                    }

                }
            }
            else
            {

                Helper.Alert(true, message, lblError);
            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error Function [btnUpload_Click(object sender, EventArgs e)] in Class [frmupload_g_card.aspx.cs], detail: " + ex.Message + " => " + ex.StackTrace);
            Helper.Alert(true, ex.Message, lblError);
            if (ClearTempData())
            {
                Helper.Alert(true, "Uploaded fail," + ex.Message + ". System clear temporary data successfully.", lblError);
            }
            else
            {

                Helper.Alert(true, "Uploaded fail," + ex.Message + ". System cannot clear temporary data successfully. Please contact your system administrator.", lblError);
            }
        }
    }
    private bool ClearTempData()
    {
        da_g_card.DeleteCardTemp(Username, DateTime.Now.Date);
        return da_g_card.TransactionStatus;

    }
    bool Validate(out string message)
    {
        bool status = true;
        message = "";
        string file_path = "";
        //check sheet name
        if ((fupload.PostedFile != null) && !string.IsNullOrEmpty(fupload.PostedFile.FileName))
        {
            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(fupload.PostedFile.FileName);
            string extension = Path.GetExtension(file_name);
            file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
            file_path = save_path + file_name;

            fupload.SaveAs(Server.MapPath(file_path));//save file 

            ExcelConnection my_excel = new ExcelConnection();
            my_excel.FileName = Server.MapPath(file_path);

            if (my_excel.GetSheetName() != "Upload$")
            {
                message = "File is not correct format, please donwload file template from the system.";
                status = false;
            }
            else
            {
                my_excel.CommandText = "Select * from [Upload$]";
                DataTable tbl = my_excel.GetData();
                int col_count = 0;
                col_count = tbl.Columns.Count;
                if (col_count > 9 || col_count < 9)//check number of columns
                {
                    message = "File is not correct format, please donwload file template from the system.";
                    status = false;
                }
                else
                {//check column name.
                    //if (tbl.Columns[0].ColumnName != "No" || tbl.Columns[1].ColumnName != "Name" || tbl.Columns[2].ColumnName != "Phone_Number")
                    //{
                    //    message = "File is not correct format, please donwload file template from the system.";
                    //    status = false;
                    //}
                    int rIndex = 0;
                    string strStatus = "";
                    string strNameKh = "";
                    string strName = "";
                    string strDob = "";
                    string strGender = "";
                    string strCert = "";
                    string strEffective = "";
                    string strExpiry = "";
                    string strMaturity = "";

                    foreach (DataRow row in tbl.Rows)
                    {
                        rIndex = tbl.Rows.IndexOf(row);

                        strNameKh = row[0].ToString().Trim();
                        strName = row[1].ToString().Trim();
                        strDob = row[2].ToString().Trim();
                        strGender = row[3].ToString().Trim();
                        strCert = row[4].ToString().Trim();
                        strEffective = row[5].ToString().Trim();
                        strExpiry = row[6].ToString().Trim();
                        strMaturity = row[7].ToString().Trim();
                        strStatus = row[8].ToString().Trim().ToUpper();
                        if (strStatus != "DELETE" && strStatus != "ADD")
                        {
                            //INVALID INPUT COLUMN STATUS;
                            message = string.Format("Row [{0}] Column [Status] input invalid value [{1}]", rIndex + 1, strStatus);
                            status = false;
                            break;
                        }
                        else
                        {
                            if (strStatus == "ADD")
                            {
                                if (strName == "")
                                {
                                    message = string.Format("Row [{0}] Column [Customer_name_en] is required.", rIndex + 1);
                                    status = false;
                                    break;
                                }
                                else if (!Helper.IsDateTime(strDob))
                                {
                                    message = string.Format("Row [{0}] Column [Customer_Dob] is required in format[DD/MM/YYYY].", rIndex + 1);
                                    status = false;
                                    break;
                                }
                                else if (strGender == "")
                                {
                                    message = string.Format("Row [{0}] Column [Customer_Gender] is required.", rIndex + 1);
                                    status = false;
                                    break;
                                }
                                else if (strCert == "")
                                {
                                    message = string.Format("Row [{0}] Column [Certificate_Number] is required.", rIndex + 1);
                                    status = false;
                                    break;
                                }
                                else if (!Helper.IsDateTime(strEffective))
                                {
                                    message = string.Format("Row [{0}] Column [Effecive_Date] is required in format[DD/MM/YYYY].", rIndex + 1);
                                    status = false;
                                    break;
                                }
                                else if (!Helper.IsDateTime(strExpiry))
                                {
                                    message = string.Format("Row [{0}] Column [Expiry_Date] is required in format[DD/MM/YYYY].", rIndex + 1);
                                    status = false;
                                    break;
                                }
                                else if (!Helper.IsDateTime(strMaturity))
                                {
                                    message = string.Format("Row [{0}] Column [Maturity_Date] is required in format[DD/MM/YYYY].", rIndex + 1);
                                    status = false;
                                    break;
                                }
                            }
                            else if (strStatus == "DELETE")
                            {


                                if (!Helper.IsDateTime(strDob))
                                {
                                    tbl.Rows[rIndex][2] = "01-01-1900";
                                }
                                if (!Helper.IsDateTime(strEffective))
                                {
                                    tbl.Rows[rIndex][5] = "01-01-1900";
                                }
                                if (!Helper.IsDateTime(strExpiry))
                                {
                                    tbl.Rows[rIndex][6] = "01-01-1900";
                                }
                                if (!Helper.IsDateTime(strMaturity))
                                {
                                    tbl.Rows[rIndex][7] = "01-01-1900";
                                }
                                if (strName == "")
                                {
                                    message = string.Format("Row [{0}] Column [Customer_name_en] is required.", rIndex + 1);
                                    status = false;
                                    break;
                                }
                                else if (strCert == "")
                                {
                                    message = string.Format("Row [{0}] Column [Certificate_Number] is required.", rIndex + 1);
                                    status = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (status)
                {
                    MyUploadData = tbl;
                }
            }
            //delete file
            File.Delete(Server.MapPath(file_path));

        }
        else
        {
            status = false;
            message = "Please select a file.";
        }
        return status;

    }
    protected void export_excel_Click(object sender, EventArgs e)
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            row_count = gv_invalid.Rows.Count;
            if (row_count > 0)
            {

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                Helper.excel.Sheet = sheet1;
                //design row header
                Helper.excel.HeaderText = new string[] { "No", "Customer_Name_Kh", "Customer_Name_En", "DOB", "Gender", "Certificate_Number", "Effective_Date", "Expiry_Date", "Maturity_Date", "Status", "Remarks" };
                Helper.excel.generateHeader();
                //disign rows
                foreach (GridViewRow row in gv_invalid.Rows)
                {
                    #region //Variable
                    Label NameKh = (Label)row.FindControl("lblCustomerNameKh");
                    Label NameEn = (Label)row.FindControl("lblCustomerNameEn");
                    Label Dob = (Label)row.FindControl("lblDob");
                    Label Gender = (Label)row.FindControl("lblGender");
                    Label Certificate = (Label)row.FindControl("lblCertificateNumber");
                    Label EffectiveDate = (Label)row.FindControl("lblEffectiveDate");
                    Label ExpiryDate = (Label)row.FindControl("lblExpiryDate");
                    Label MaturityDate = (Label)row.FindControl("lblMaturityDate");
                    Label Status = (Label)row.FindControl("lblStatus");
                    Label Remarks = (Label)row.FindControl("lblRemarks");

                    #endregion
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(NameKh.Text);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(NameEn.Text);

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(Dob.Text);
                    HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                    style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd-MM-yyyy");
                    Cell4.CellStyle = style;

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(Gender.Text);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(Certificate.Text);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(EffectiveDate.Text);

                    // style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd-MM-yyyy");
                    Cell7.CellStyle = style;

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(ExpiryDate.Text);
                    // HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                    // style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd-MM-yyyy");
                    Cell8.CellStyle = style;

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(MaturityDate.Text);
                    // HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                    // style.DataFormat = hssfworkbook.GetCreationHelper().CreateDataFormat().GetFormat("dd-MM-yyyy");
                    Cell9.CellStyle = style;

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Status.Text);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Remarks.Text);

                }

                string filename = "Invalid_Records_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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

            Log.AddExceptionToLog("Error function [ExportExcel()] in page [frmUpload_g_card.aspx.cs], detail:" + ex.Message + " => " + ex.StackTrace);
            Helper.Alert(true, "Export data fail, please contact your system administrator.", lblError);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        MembershipProvider provider;
        provider = Membership.Providers["MySqlMembershipProvider"];
        RoleProvider role = Roles.Providers["MySqlRoleProvider"];
        List<bl_g_card> SuccessList = new List<bl_g_card>();
        List<bl_g_card> FailList = new List<bl_g_card>();
        bl_g_card fail = new bl_g_card();
        System.Guid proKey;
        MembershipUser MemUser ;

        try
        {
            foreach (bl_g_card card in ValidData)
            {
                if (card.Status == 1)
                {
                    da_g_card.SaveCard(card);
                    if (da_g_card.TransactionStatus == true)
                    {
                      
                        SuccessList.Add(card);
                        System.Web.Security.MembershipCreateStatus status;
                        proKey = System.Guid.NewGuid();

                        MemUser = provider.GetUser(card.Owner, false);

                        if (MemUser != null)//exist user name
                        {
                            if (MemUser.IsApproved == false)
                            {
                                MemUser.IsApproved = true;
                                da_g_card.UpdateMembershipApproval(1, MemUser.UserName);
                                if (da_g_card.TransactionStatus == true)
                                {
                                    card.Remarks = "ADDED";
                                }
                                else
                                {
                                    card.Remarks = "ADDED, CHANGE APPROVAL STATUS USER FAIL.";
                                  
                                }

                            }
                        }
                        else //new user name
                        {
                            provider.CreateUser(card.Owner, card.DOB.ToString("ddMMyyyy"), "", "", "", true, proKey, out status);
                            if (status == System.Web.Security.MembershipCreateStatus.Success)
                            {
                                //string[] user = new string[] { "som.maneth4" };
                                //string[] r = new string[] { "ExternalUser" };
                                role.AddUsersToRoles(new string[] { card.Owner }, new string[] { "ExternalUser" });
                            }
                            else
                            {
                                card.Remarks = "CREATED USER FAIL";
                                FailList.Add(card);
                            }
                        }

                                               

                    }
                    else
                    {
                        fail = new bl_g_card();
                        fail = card;
                        fail.Remarks = da_g_card.Exception;
                        FailList.Add(fail);
                    }

                }
                else
                {
                    da_g_card.UpdateCard(0, Username, DateTime.Now, "DELETE", card.CertificateNumber, card.Owner);
                    if (da_g_card.TransactionStatus)
                    {
                        card.Remarks = "UPDATED";
                        SuccessList.Add(card);

                        da_g_card.UpdateMembershipApproval(0, card.Owner);
                        if (!da_g_card.TransactionStatus == true)
                        {
                            card.Remarks = "LOCKED USER FAIL";
                            FailList.Add(card);
                        }


                    }
                    else
                    {
                        card.Remarks = da_g_card.Exception;
                        FailList.Add(card);
                    }
                }
            }

            gv_valideList.DataSource = SuccessList;
            gv_valideList.DataBind();

            gv_invalid.DataSource = FailList;
            gv_invalid.DataBind();

            div_record_found.InnerText = "Total Data = " + ValidData.Count + " | Data Success = " + SuccessList.Count + " | Data Fail = " + FailList.Count;
            btnSave.Visible = SuccessList.Count > 0 ? true : false;
            export_excel.Visible = FailList.Count > 0 ? true : false;

            if (ValidData.Count == SuccessList.Count)
            {
                Helper.Alert(false, "All Data are saved successfully.", lblError);
            }
            else if (ValidData.Count == FailList.Count)
            {
                Helper.Alert(true, "All Data are saved fail. Please see fail detail in [Invalid Data]", lblError);
            }
            else
            {
                Helper.Alert(true, "Some Data are saved successfully and some data are saved fail. Please see fail detail in [Invalid Data]", lblError);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error Function [btnSave_Click(object sender, EventArgs e)] in Class [frmupload_g_card_aspx.cs], detail: " + ex.Message + " => " + ex.StackTrace);
            Helper.Alert(true, ex.Message, lblError);
        }
    }
}