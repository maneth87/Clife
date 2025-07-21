using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Web.Security;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
public partial class Pages_CI_frmSOUnpaid : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    string user_name = "";
    string product_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        
        if (Request.QueryString.Count > 0)
        {
            product_id = Request.QueryString["pro_type"];
        }
        else
        {
            product_id = "SO,CI";
        }
       
        user_name = Membership.GetUser().UserName;
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        gv_policy.DataSource = null;
        gv_policy.DataBind();
        gvErr.DataSource = null;
        gvErr.DataBind();

        try
        {
            /*create sub folder*/
            string subFolder = "SOUnpaid";
            documents.TrascationFiles.CreateSubFolder(subFolder);
            string newFileName = Guid.NewGuid().ToString().ToUpper();
            string newSavedFilePath = "";
            if ((UnpaidFile.PostedFile != null) && !string.IsNullOrEmpty(UnpaidFile.PostedFile.FileName))
            {

                string save_path = "~/Upload/";
                string file_name = Path.GetFileName(UnpaidFile.PostedFile.FileName);
                string extension = Path.GetExtension(file_name);
                file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
                string file_path = save_path + file_name;
                int row_number = 0;

                UnpaidFile.SaveAs(Server.MapPath(file_path));//save file 

                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "SELECT * FROM [Upload$]";
                DataTable my_data = new DataTable();
                my_data = my_excel.GetData();

                row_number = my_data.Rows.Count;

                /*SAVE DOCUMMENT INFORMATION*/
                //documents.TrascationFiles.SaveDoc(new documents.TrascationFiles() {
                // DocName= string.Concat(newFileName,extension),
                //  UploadedBy= user_name,
                //  UploadedOn=DateTime.Now,
                //   DocPath= string.Concat(subFolder,"/",newFileName,extension),
                //    DocDescription="SO unpaid premium"
                
                //});

                /*copy file*/
                 newSavedFilePath=string.Concat(documents.TrascationFiles.Path, subFolder, "/", newFileName, extension);
                File.Copy(my_excel.FileName, newSavedFilePath );

                #region //All IF SO
                string filter = "";
                //get all S.O policies
                DataTable tbl = da_ci.GetPolicyCIDetailReport(new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), "", "", product_id);

                filter += filter.Trim() == "" ? " policy_status in ('IF','LAP')" : " and policy_status in ('IF','LAP')";//get only IF status
                DataTable tbl_filter = my_data.Clone();//clone propertity from table tbl such columns name
                //Add new column Remarks
                tbl_filter.Columns.Add("Pay_Year");
                tbl_filter.Columns.Add("Pay_Lot");
                DataTable tbl_err = my_data.Clone();

                DataRow row_filter;
                DataRow row_err;
                int pay_year = 0;
                int pay_lot = 0;
                bool isSavePolicyPremPay = false;
                DateTime transaction_date = DateTime.Now;
                DateTime last_due = new DateTime(1900, 1, 1);
                int no_rows = 0;
                foreach (DataRow row_upload in my_data.Rows)
                {
                    try
                    {
                        #region Loop filter
                        foreach (DataRow row in tbl.Select(filter))//filter data in table tbl
                        {
                            #region existing policy
                            if (row["policy_number"].ToString().Trim() == row_upload["policy_number"].ToString().Trim())
                            {
                                #region Check duplicate due date
                               last_due=  da_policy_prem_pay.GetLast_Due_Date(row["policy_id"].ToString());
                               if (last_due == Helper.FormatDateTime(row_upload["due_date"].ToString()))
                               {
                                   row_err = tbl_err.NewRow();
                                   row_err["Policy_number"] = row_upload["policy_number"];
                                   row_err["due_date"] = row_upload["due_date"];
                                   row_err["premium"] = row_upload["premium"];
                                   row_err["Remarks"] = "Due date is duplicated.";
                                   tbl_err.Rows.Add(row_err);
                                   break;
                               }
                               else
                               {
                                   #region new record
                                   pay_year = da_policy_prem_pay.GetLast_Prem_Year(row["policy_id"].ToString().Trim());
                                   pay_year = pay_year == 0 ? 1 : pay_year;
                                   pay_lot = da_policy_prem_pay.GetPrem_Lot(row["policy_id"].ToString().Trim(), Helper.GetPayModeID(row["mode"].ToString()), pay_year);

                                   bl_policy_prem_pay pol_prem_pay = new bl_policy_prem_pay();
                                   pol_prem_pay.Policy_Prem_Pay_ID = Helper.GetNewGuid(new string[,] { { "TABLE", "CT_POLICY_PREM_PAY" }, { "FIELD", "POLICY_PREM_PAY_ID" } });
                                   pol_prem_pay.Policy_ID = row["policy_id"].ToString().Trim();
                                   pol_prem_pay.Due_Date = Helper.FormatDateTime(row_upload["due_date"].ToString());
                                   pol_prem_pay.Pay_Date = pol_prem_pay.Due_Date;
                                   pol_prem_pay.Prem_Year = pay_year;
                                   pol_prem_pay.Prem_Lot = pay_lot;
                                   pol_prem_pay.Amount = Convert.ToDouble(row_upload["premium"].ToString());
                                   pol_prem_pay.Sale_Agent_ID = row["agent_code"].ToString().Trim();
                                   pol_prem_pay.Office_ID = "HQ";
                                   pol_prem_pay.Created_By = user_name;
                                   pol_prem_pay.Created_On = transaction_date;
                                   pol_prem_pay.Created_Note = row_upload["Remarks"].ToString().Trim();
                                   pol_prem_pay.Pay_Mode_ID = Helper.GetPayModeID(row["mode"].ToString());

                                   isSavePolicyPremPay = da_policy.InsertPolicyPremiumPay(pol_prem_pay);
                                   if (isSavePolicyPremPay)
                                   {
                                       row_filter = tbl_filter.NewRow();

                                       row_filter["Policy_number"] = row_upload["policy_number"];
                                       row_filter["due_date"] = row_upload["due_date"];
                                       row_filter["premium"] = row_upload["premium"];
                                       row_filter["pay_year"] = pay_year;
                                       row_filter["pay_lot"] = pay_lot;
                                       row_filter["Remarks"] = "Saved successfully.";

                                       tbl_filter.Rows.Add(row_filter);
                                       break;
                                   }
                                   else
                                   {
                                       row_err = tbl_err.NewRow();
                                       row_err["Policy_number"] = row_upload["policy_number"];
                                       row_err["due_date"] = row_upload["due_date"];
                                       row_err["premium"] = row_upload["premium"];
                                       row_err["Remarks"] = "Saved fail.";
                                       tbl_err.Rows.Add(row_err);
                                       break;
                                   }
                                   #endregion new record
                               }
                                #endregion Check duplicate due date

                            }
                            #endregion Existing policy
                            #region Policy not yet registered
                            else
                            {
                                no_rows += 1;
                                if (tbl.Select(filter).Distinct().Count() == no_rows)// not exist from the beginning till end
                                {
                                    row_err = tbl_err.NewRow();
                                    row_err["Policy_number"] = row_upload["policy_number"];
                                    row_err["due_date"] = row_upload["due_date"];
                                    row_err["premium"] = row_upload["premium"];
                                    row_err["Remarks"] = "Policy is not exist.";
                                    tbl_err.Rows.Add(row_err);
                                    // break;
                                }

                            }
                            #endregion Policy not yet registered
                        }
                        #endregion Loop filter
                        //reset
                        no_rows = 0;
                        last_due = new DateTime(1900, 1, 1);
                    }
                    catch (Exception ex)
                    {
                        no_rows = 0;
                        Log.AddExceptionToLog("Error function [upload unpaid] in page [frmSOUnpaid], [POLICY_NUMBER:"+row_upload["policy_number"].ToString()+"], Detail:" + ex.Message);
                        row_err = tbl_err.NewRow();
                        row_err["Policy_number"] = row_upload["policy_number"];
                        row_err["due_date"] = row_upload["due_date"];
                        row_err["premium"] = row_upload["premium"];
                        row_err["Remarks"] = "Error.";
                        tbl_err.Rows.Add(row_err);
                    }

                }

                /*save activity log*/
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPLOAD, string.Concat( "User uploads SO unpaid [File:", newSavedFilePath, ", success:", tbl_filter.Rows.Count,", fial:", tbl_err.Rows.Count,"]." ));
              
                if (tbl_filter.Rows.Count > 0)
                {
                    gv_policy.DataSource = tbl_filter;
                    gv_policy.DataBind();

                    btnExportValid.Visible = true;
                }
                else
                {
                    btnExportValid.Visible = false;
                    gv_policy.DataSource = null;
                    gv_policy.DataBind();
                }

                if (tbl_err.Rows.Count > 0)
                {
                    gvErr.DataSource = tbl_err;
                    gvErr.DataBind();
                    export_excel.Visible = true;
                }
                else
                {
                    gvErr.DataSource = null;
                    gvErr.DataBind();
                    export_excel.Visible = false;
                }

                //message alert
                if (tbl_err.Rows.Count == 0 && tbl_filter.Rows.Count > 0)
                { 
                    //all success
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('All data were uploaded successfully.');", true);
                }
                else if (tbl_err.Rows.Count > 0 && tbl_filter.Rows.Count == 0)
                {
                    //all fail
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('All data were uploaded fail, please see in tab [Fail].');", true);
                }
                else
                { 
                    //few success and fail
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Some data were uploaded fail, please see in tab [Fail].');", true);
                }
                #endregion //ALL IF SO

                //delete file
                try
                {
                    File.Delete(Server.MapPath(file_path));
                }
                catch (Exception ex)
                {
                    Log.AddExceptionToLog("Delete file [" + file_path + "] error, Detail:" + ex.Message);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Please upload unpaid policies file.');", true);

            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Unpaid policies [UPLOAD DATA], detail:" + ex.Message, user_name);
        }
    }
    protected void export_excel_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }
    void ExportExcel()
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            row_count = gvErr.Rows.Count;
            if (row_count > 0)
            {

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                Helper.excel.Sheet = sheet1;
                //design row header
                Helper.excel.HeaderText = new string[] {"No", "Policy_Number","Due_Date", "Premium", "Remarks"};
                Helper.excel.generateHeader();
                //disign rows
                foreach (GridViewRow row in gvErr.Rows)
                {
                    #region //Variable
                    Label policy_number = (Label)row.FindControl("lblPolicyNumber");
                    Label due_date = (Label)row.FindControl("lblDueDate");
                    Label premium = (Label)row.FindControl("lblPremium");
                    Label remarks = (Label)row.FindControl("lblRemarks");
                   
                    #endregion
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(policy_number.Text);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(due_date.Text);
                   

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(premium.Text);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(remarks.Text);


                }

                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports fail data to excel file [Total record(s):", row_count, "]."));

                string filename = "SO_Unpaid_Records_Err_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Export data fail, please contact your system administrator.');", true);
            Log.AddExceptionToLog("Error function [ExportExcel()] in page [frmSOUnpaid.aspx.cs], detail:" + ex.Message);
        }
    }
    protected void btnExportValid_Click(object sender, EventArgs e)
    {
        try
        {
            int row_count = 0;
            int row_no = 0;
            row_count =gv_policy.Rows.Count;
            if (row_count > 0)
            {

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                Helper.excel.Sheet = sheet1;
                //design row header
                Helper.excel.HeaderText = new string[] { "No", "Policy_Number", "Due_Date", "Premium", "Pay Year", "Pay Lot", "Remarks" };
                Helper.excel.generateHeader();
                //disign rows
                foreach (GridViewRow row in gv_policy.Rows)
                {
                    #region //Variable
                    Label policy_number = (Label)row.FindControl("lblPolicyNumber");
                    Label due_date = (Label)row.FindControl("lblDueDate");
                    Label premium = (Label)row.FindControl("lblPremium");
                    Label payYear = (Label)row.FindControl("lblPayYear");
                    Label payLot = (Label)row.FindControl("lblPayLot");
                    Label remarks = (Label)row.FindControl("lblRemarks");

                    #endregion
                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(policy_number.Text);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(due_date.Text);

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(premium.Text);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(payYear.Text);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(payLot.Text);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(remarks.Text);


                }
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports success data to excel file [Total record(s):", row_count, "]."));
                string filename = "SO_Unpaid_Records_Success_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Export data fail, please contact your system administrator.');", true);
            Log.AddExceptionToLog("Error function [btnExportValid_Click(object sender, EventArgs e)] in page [frmSOUnpaid.aspx.cs], detail:" + ex.Message);
        }
    }
}