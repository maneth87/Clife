using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
public partial class Pages_Setting_frmActivityLog : System.Web.UI.Page
{
    private bl_sys_user_role UserRole { get { return (bl_sys_user_role)ViewState["V_USER_ROLE"]; } set { ViewState["V_USER_ROLE"] = value; } }
    private List<bl_sys_activity_log> ActivityLogList { get { return (List<bl_sys_activity_log>)ViewState["V_ACTIVITY_LOG"]; } set { ViewState["V_ACTIVITY_LOG"] = value; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";

       // UserName = Membership.GetUser().UserName;

        string ObjCode = Path.GetFileName(Request.Url.AbsolutePath);
        List<bl_sys_user_role> Lobj = (List<bl_sys_user_role>)Session["SS_UR_LIST"];
        if (Lobj != null)
        {
            bl_sys_user_role ur = new bl_sys_user_role();
            bl_sys_user_role u = ur.GetSysUserRole(Lobj, ObjCode);
            if (u.UserName != null)
            {
                UserRole = u;
            }
            if (!Page.IsPostBack)
            {
                PageTransaction(Helper.FormTransactionType.FIRST_LOAD);
            }
        }
        else
        {
            Response.Redirect("../../unauthorize.aspx");
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        PageTransaction(Helper.FormTransactionType.SAVE);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        PageTransaction(Helper.FormTransactionType.SEARCH);
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_valid.PageIndex = e.NewPageIndex;

        gv_valid.DataSource = ActivityLogList;
        gv_valid.DataBind();
        /*show record count*/
        lblRecords.Text = gv_valid.PageCount == e.NewPageIndex + 1 ? string.Concat("Record(s): ", gv_valid.PageSize * (e.NewPageIndex) + gv_valid.Rows.Count, " of ", ActivityLogList.Count) :  string.Concat("Record(s): ", gv_valid.PageSize * (e.NewPageIndex + 1), " of ", ActivityLogList.Count);
    }

    void PageTransaction(Helper.FormTransactionType tranType)
    {
        string message = "";
        #region first load
        if (tranType == Helper.FormTransactionType.FIRST_LOAD)
        {
            txtDateF.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateT.Text = DateTime.Now.ToString("dd/MM/yyyy");
            if (UserRole.IsView != 1 && UserRole.IsAdmin!=1)
            {
                EnableControls(false);
                Helper.Alert(true, "User do not have permission to access this page.", lblError);
            }
            else
            {
                EnableControls();
            }

        }
        #endregion 
        #region search
        else if (tranType == Helper.FormTransactionType.SEARCH)
        {
            if (UserRole.IsView != 1 && UserRole.IsAdmin != 1)
            {
                EnableControls(false);
                Helper.Alert(true, "User do not have permission to view any information.", lblError);
            }
            else
            {
                Search(out message);
                if (message == "")/*nothing error save activity log*/
                {
                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat( "User inquiries user activity log with criteria [ Date From:",txtDateF.Text.Trim(),", To:", txtDateT.Text.Trim(),txtUserName.Text.Trim()​​!="" ?", User Name:"+ txtUserName.Text.Trim():"","]"));
                }
            }
        }
        #endregion 
        #region Export
        else if (tranType == Helper.FormTransactionType.SAVE)
        {
            if (ActivityLogList.Count > 0)
            {

                try
                {
                   
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                        Response.Clear();
                        HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

                        Helper.excel.Sheet = sheet1;
                        Helper.excel.Title = new string[] { "User Activity Log", string.Concat( "Activity Date :", txtDateF.Text.Trim()," To ",txtDateT.Text.Trim() ) };
                        Helper.excel.HeaderText = new string[]
                        {
                        "No.", "Activity Date", "User Name", "Page Name","Page Code","Activity", "Description"
                        };

                        Helper.excel.generateHeader();

                        int row_no = 0;
                        row_no = Helper.excel.NewRowIndex - 1;
                        foreach (var obj in ActivityLogList)
                        {

                            row_no += 1;
                            HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                            HSSFCell Cell0 = (HSSFCell)rowCell.CreateCell(0);
                            Cell0.SetCellValue(row_no- Helper.excel.Title.Count());

                            HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(1);
                            Cell1.SetCellValue(obj.ActivityDate.ToString("dd-MM-yyyy"));

                            HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(2);
                            Cell2.SetCellValue(obj.UserName);

                            HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(3);
                            Cell3.SetCellValue(obj.ObjectName);

                            HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(4);
                            Cell4.SetCellValue(obj.ObjectId);

                            HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(5);
                            Cell5.SetCellValue(obj.ActivityType.ToString());

                            HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(6);
                            Cell6.SetCellValue(obj.Description);

                        }
                        string filename = "UserActivityLog_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                        Response.Clear();
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                        MemoryStream file = new MemoryStream();
                        hssfworkbook.Write(file);


                        Response.BinaryWrite(file.GetBuffer());
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, "User exported user activity log.");
                        Response.End();
                       
                }
                catch (Exception ex)
                {
                    Helper.Alert(true, ex.Message, lblError);
                }
            }
            else {
                Helper.Alert(false, "No record found.", lblError);
            }
        }
        #endregion
    }
    void Search(out string message)
    {
        message = "";
        if (!Helper.IsDate(txtDateF.Text.Trim()))
        {
            message = "Activity Date From is required as date [dd/MM/yyyy].";
        }
        else if (!Helper.IsDate(txtDateT.Text.Trim()))
        {
            message = "Activity Date To is required as date [dd/MM/yyyy].";
        }
        else
        {
       List<bl_sys_activity_log> lActivity=     da_sys_activity_log.GetList(Helper.FormatDateTime(txtDateF.Text.Trim()), Helper.FormatDateTime(txtDateT.Text.Trim()), txtUserName.Text.Trim());
            if (da_sys_activity_log.Transaction)
            {
                ActivityLogList = lActivity;
                gv_valid.DataSource = lActivity;
                gv_valid.DataBind();
                /*show record count*/
                lblRecords.Text = gv_valid.Rows.Count + " Of " + ActivityLogList.Count + " Records";

                btnExport.Enabled = lActivity.Count == 0 ? false : true;
            }
            else
            {
                Helper.Alert(true, da_sys_activity_log.Message, lblError);
            }
        }

    }
    void EnableControls(bool t = true)
    {
        txtDateF.Enabled = t;
        txtDateT.Enabled = t;
        btnSearch.Enabled = t;
        btnExport.Enabled = t;
        txtUserName.Enabled = t;
    }

    void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(UserRole.UserName, UserRole.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
}