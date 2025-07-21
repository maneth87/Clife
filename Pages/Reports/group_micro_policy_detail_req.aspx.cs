using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
using System.Data;
using System.Web.Security;

public partial class Pages_Reports_group_micro_policy_detail_req : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));
    }

    private List<Report.GroupMicro.PolicyDetail> MyPolicyList { get { return (List<Report.GroupMicro.PolicyDetail>)ViewState["VS_MY_POL_LIST"]; } set { ViewState["VS_MY_POL_LIST"] = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        if (!Page.IsPostBack)
        {
            DataTable tblStatus = new DB().GetData(AppConfiguration.GetConnectionString(), "SP_GET_POLICY_STATUS", new string[,] { }, "micro_policy_update_status.aspx => Page load");
            Options.Bind(cblStatus, tblStatus, "Detail", "Policy_Status_Code", 0);

            for (int i = 0; i < cblStatus.Items.Count; i++)
            {
                cblStatus.Items[i].Selected = true;
            }

            btnExport.Enabled = false;
            if (!Helper.BindChannel(ddlChannel))
            {
                Helper.Alert(true, "Bind Channel Error", lblError);
            }
            else
            {
                ddlChannel.SelectedIndex = 2;
                ddlChannel_SelectedIndexChanged(null, null);
                ddlChannel.Enabled = false;
            }

            if (Request.QueryString.Count > 0)
            {
                string channelItemId = Request.QueryString[0].ToString();
                txtReportDateFrom.Text = Request.QueryString[1].ToString();
                txtReportDateTo.Text = Request.QueryString[2].ToString();


                ddlChannelItem.SelectedValue = channelItemId;
                ddlChannelItem_SelectedIndexChanged(null, null);
                Helper.SelectedDropDownListIndex("VALUE", ddlProduct, Request.QueryString[3].ToString());
                btnSearch_Click(null, null);
            }
        }
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }

    private List<string> GetPolicyStatus()
    {
        List<string> lStr = new List<string>();
        if (cblStatus.Items.Count > 0)
        {
            for (int i = 0; i < cblStatus.Items.Count; i++)
            {
                if (cblStatus.Items[i].Selected)
                {
                    lStr.Add(cblStatus.Items[i].Value);
                }
            }
        }
        return lStr;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            MyPolicyList = (List<Report.GroupMicro.PolicyDetail>)Report.GroupMicro.GetPolicyDetail(Helper.FormatDateTime(txtReportDateFrom.Text), Helper.FormatDateTime(txtReportDateTo.Text), "", "", "", "", GetPolicyStatus(), new List<string>() { ddlProduct.SelectedValue }, Report.GroupMicro.ReportType.ListObject);
            if (MyPolicyList.Count > 0)
            {
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries policy detail with criteria [Payment Date From:", txtReportDateFrom.Text.Trim(), " To:", txtReportDateTo.Text.Trim(), "; Policy Status:", string.Join(",", GetPolicyStatus()), "; Product Id:", ddlProduct.SelectedValue, "]."));

                gv_valid.DataSource = MyPolicyList;
                gv_valid.DataBind();
                btnExport.Enabled = true;
                lblRecords.Text = "Record(s): " + gv_valid.Rows.Count + " of " + MyPolicyList.Count;
            }
            else
            {
                MyPolicyList = null;
                gv_valid.DataSource = null;
                gv_valid.DataBind();

                btnExport.Enabled = false;
                lblRecords.Text = "No Record Found.";
            }
        }
        catch (Exception ex)
        {
            MyPolicyList = null;
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            Log.AddExceptionToLog("Error function [btnSearch_Click(object sender, EventArgs e)] in class [group_micro_policy_detail_req.aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);

            Helper.Alert(true, "Error:" + ex.Message, lblError);

        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            List<Report.GroupMicro.PolicyDetail> filterObj = new List<Report.GroupMicro.PolicyDetail>();
            filterObj = MyPolicyList;
            if (filterObj.Count > 0)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                Response.Clear();
                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");
                var pre = filterObj[0];
                Helper.excel.Sheet = sheet1;
                Helper.excel.Title = new string[] { "Policy Details", pre.ChannelName };
                Helper.excel.HeaderText = new string[]
                        {
                        "Application Number", "Policy Number", "Agent Code", "Agent Name","Customer Id","Customer Name","Customer Name", "ID Type", "ID Number","Gender", "DOB", 
                        "Product ID","Package", "Sum Assue","Premium","Annual Premium", "Discount Amount", "Amount" ,"Pay Mode", "Issue Age",  
                        "Effective Date","Expiry Date", "Ben. Name", "Ben. Age", "Relation", "Percentage", "PolicyStatus","PolicyStatusDate"
                        };

                Helper.excel.generateHeader();

                int row_no = 0;
                row_no = Helper.excel.NewRowIndex - 1;
                foreach (Report.GroupMicro.PolicyDetail obj in filterObj)//foreach (DataRow r in my_session.DATA.Rows)
                {

                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(obj.ApplicationNumber);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(obj.PolicyNumber);

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(obj.SaleAgentId);

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(obj.SaleAgentNameEn);

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(obj.CustomerNumber);

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(obj.FullNameEn);

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(obj.FullNameKh);

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(obj.IdTypeEn);

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(obj.IdNumber);

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(obj.GenderEn);

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(obj.DateOfBirth.ToString("dd-MMM-yyyy"));

                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(obj.ProductId);

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(obj.Package);

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(obj.SumAssure);

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(obj.Premium);

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(obj.AnnualPremium);

                    HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                    Cell17.SetCellValue(obj.BasicDiscountAmount);

                    HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                    Cell18.SetCellValue(obj.BasicTotalAmount);

                    HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                    Cell19.SetCellValue(obj.PayModeEn);

                    HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                    Cell20.SetCellValue(obj.IssueAge);

                    HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);
                    Cell21.SetCellValue(obj.EffectiveDate.ToString("dd-MMM-yyyy"));

                    HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(21);
                    Cell22.SetCellValue(obj.ExpiryDate.ToString("dd-MMM-yyyy"));

                    HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(22);
                    Cell23.SetCellValue(obj.BenFullName);

                    HSSFCell Cell24 = (HSSFCell)rowCell.CreateCell(23);
                    Cell24.SetCellValue(obj.BenAge);

                    HSSFCell Cell25 = (HSSFCell)rowCell.CreateCell(24);
                    Cell25.SetCellValue(obj.Relation);

                    HSSFCell Cell26 = (HSSFCell)rowCell.CreateCell(25);
                    Cell26.SetCellValue(obj.PercentageShared);

                    HSSFCell Cell27 = (HSSFCell)rowCell.CreateCell(26);
                    Cell27.SetCellValue(obj.PolicyStatus);

                    HSSFCell Cell28 = (HSSFCell)rowCell.CreateCell(27);
                    Cell28.SetCellValue(obj.PolicyStatusDate.Year == 2000 ? "" : obj.PolicyStatusDate.ToString("dd-MMM-yyyy"));
                }

                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.EXPORT, string.Concat("User exports policy detail [Total record(s):", filterObj.Count, "]."));


                string filename = "Group_Miro_Policy_Detail_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                MemoryStream file = new MemoryStream();
                hssfworkbook.Write(file);


                Response.BinaryWrite(file.GetBuffer());

                Response.End();
            }
            else
            {
                Helper.Alert(false, "No data to export.", lblError);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnExport_Click(object sender, EventArgs e)] in class [group_micro_policy_detail_req.aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);
            Helper.Alert(true, ex.Message, lblError);
        }
    }

    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            int countRow = 0;

            gv_valid.PageIndex = e.NewPageIndex;
            gv_valid.DataSource = MyPolicyList;
            gv_valid.DataBind();

            if (gv_valid.PageCount == e.NewPageIndex + 1)//last page
            {
                countRow = gv_valid.PageSize * (e.NewPageIndex) + gv_valid.Rows.Count;
            }
            else
            {
                countRow = gv_valid.PageSize * (e.NewPageIndex + 1);
            }
            lblRecords.Text = "Record(s): " + countRow + " of " + MyPolicyList.Count;
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error:" + ex.Message, lblError);

        }
    }
    protected void gv_valid_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    protected void gv_valid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label lAppId;
        Label lPolId;
        LinkButton lAppNo;
        LinkButton lPolNo;
        string appId = "";
        string polId = "";
        int index = -1;
        string url = "";
        try
        {
            index = Convert.ToInt32(e.CommandArgument);
            GridViewRow g_row;
            GridView g = sender as GridView;

            g_row = g.Rows[index];
            if (e.CommandName == "CMD_PRINT_APP")
            {
                lAppId = (Label)g_row.FindControl("lblApplicationId");
                lAppNo = (LinkButton)g_row.FindControl("lbtApplication");

                appId = lAppId.Text;

                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views application form of [App No:", lAppNo.Text,"]."));

                url = string.Format("../Business/banca_micro_application_print.aspx?APP_ID={0}&A_TYPE=BDL", appId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);

            }
            else if (e.CommandName == "CMD_PRINT_POL")
            {
                lPolId = (Label)g_row.FindControl("lblPolicyId");
                lPolNo = (LinkButton)g_row.FindControl("lbtPolicy");
                polId = lPolId.Text;
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views ceritificate of [Pol No:", lPolNo.Text, "]."));


                url = string.Format("../Business/banca_micro_cert.aspx?P_ID={0}&P_TYPE=BDL", polId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Error:" + ex.Message, lblError);
        }
    }
    protected void cblStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedCount = cblStatus.Items.Cast<ListItem>().Count(li => li.Selected);
        int itemCount = cblStatus.Items.Count;

        string result = Request.Form["__EVENTTARGET"];

        string[] checkedBox = result.Split('$'); ;

        int index = int.Parse(checkedBox[checkedBox.Length - 1]);//get check box index

        if (index == 0)
        {
            if (cblStatus.Items[0].Selected)
            {
                for (int i = 0; i <= cblStatus.Items.Count - 1; i++)
                {
                    cblStatus.Items[i].Selected = true;
                }
            }
            else
            {
                cblStatus.ClearSelection();
            }
        }
        else
        {
            if (selectedCount < itemCount && cblStatus.Items[0].Selected == true)
            {
                cblStatus.Items[0].Selected = false;
            }
            else if (selectedCount == itemCount - 1)
            {
                cblStatus.Items[0].Selected = true;
            }
        }
    }
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*bind saleable product list */
        ddlProduct.Items.Clear();
        List<bl_micro_product_config> proConList = da_micro_product_config.ProductConfig.GetMicroProductConfigListByChannelItemId(ddlChannelItem.SelectedValue, true);
        Options.Bind(ddlProduct, proConList, bl_micro_product_config.NAME.MarketingName, bl_micro_product_config.NAME.Product_ID, 0, "--- Select ---");

        if (ddlProduct.Items.Count == 2)
        {
            ddlProduct.SelectedIndex = 1;
            ddlProduct.Enabled = false;

        }
        else
        {
            ddlProduct.Enabled = true;
            ddlProduct.SelectedIndex = 0;
        }
    }
}