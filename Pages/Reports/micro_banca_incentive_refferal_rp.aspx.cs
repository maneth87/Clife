using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
using System.Web.Security;
public partial class Pages_Reports_micro_banca_incentive_refferal_rp : System.Web.UI.Page
{
    string user = "";
    string[] userRole = new string[] { };
    private bool RoleIsIA { get { return (bool)ViewState["VS_ROLE_IS_IA"]; } set { ViewState["VS_ROLE_IS_IA"] = value; } }

    int LoadChannelItem()
    {
        ddlChannelItem.Items.Clear();
        List<bl_channel_item> chList = new List<bl_channel_item>();
        List<bl_channel_item> chListFilter = new List<bl_channel_item>();

        if (RoleIsIA)
        {

            /*bind channel by user name*/

            chList = da_channel.GetChannelItemByUserName(user);

            //channel search
            Options.Bind(ddlChannelItem, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select ---");
            if (chList.Count == 1)
            {
                ddlChannelItem.SelectedIndex = 1;
                ddlChannelItem.Enabled = false;
                ddlBranchName_SelectedIndexChanged(null, null);

            }
            else
            {
                ddlChannelItem.Enabled = true;
            }
        }
        else
        {
            chList = da_channel.GetChannelItemListByChannel(ddlChannel.SelectedValue);
            Options.Bind(ddlChannelItem, chList, bl_channel_item.NAME.Channel_Name, bl_channel_item.NAME.Channel_Item_ID, 0, "--- Select --");

        }
        return chList.Count;
    }
    int LoadChannelLocation(string channelId)
    {
        cblChannelLocation.Items.Clear();

        List<bl_channel_location> locList = new List<bl_channel_location>();
        if (RoleIsIA)
        {
            locList = da_channel.GetChannelLocationByChannelItemIdUser(channelId, user);
            Options.Bind(cblChannelLocation, locList, bl_channel_location.NAME.OfficeCode, bl_channel_location.NAME.ChannelItemId, -1);
            if (locList.Count() == 1)
            {
                cblChannelLocation.SelectedIndex = 1;
                cblChannelLocation.Enabled = false;
                // cblChannelLocation_SelectedIndexChanged(null, null);
            }
            else
            {
                cblChannelLocation.Enabled = true;
            }
        }
        else
        {
            locList = da_channel.GetChannelLocationListByChannelItemID(channelId);
            Options.Bind(cblChannelLocation, locList, bl_channel_location.NAME.OfficeCode, bl_channel_location.NAME.ChannelItemId, -1);
        }

        return locList.Count;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        string user_id = "";
        MembershipUser myUser = Membership.GetUser();
        user = myUser.UserName;
        user_id = myUser.ProviderUserKey.ToString();
        userRole = Roles.GetRolesForUser(user);
        List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];

        if (!Page.IsPostBack)
        {
            if (Lrole != null)
            {
                showMessage("", "");
                txtIssuedDateFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtIssuedDateTo.Text = DateTime.Now.ToString("dd-MM-yyyy");

               

                //if (ddlChannel.Items.Count > 0)
                //{
                //    ddlChannel.SelectedIndex = 2;
                //    ddlChannel_SelectedIndexChanged(null, null);
                //    ddlChannelItem.SelectedIndex = 1;
                //    ddlBranchName_SelectedIndexChanged(null, null);
                //    for (int i = 0; i < cblChannelLocation.Items.Count; i++)
                //    {
                //        cblChannelLocation.Items[i].Selected = true;
                //    }
                //}
                //ddlChannel.Attributes.Remove("disabled");
                //ddlChannelItem.Attributes.Remove("disabled");

                //cblChannelLocation.Enabled = true;
                //btnExport.Attributes.Add("disabled", "disabled");

                if (Helper.BindChannel(ddlChannel))
                {
                    if (Lrole[0].RoleId == "RCOM10" || Lrole[0].RoleId == "RCOM12")
                    {
                        //my_session.IsIA = true;
                        RoleIsIA = true;
                        ddlChannel.SelectedIndex = 2;
                        ddlChannel.Enabled = false;
                    }
                    else
                    {
                        //my_session.IsIA = false;
                        RoleIsIA = false;

                    }
                    LoadChannelItem();
                }
                else
                {
                    Helper.Alert(true, "Load channel error.", lblError);
                }
            }
            else
            {
                Response.Redirect("../../unauthorize.aspx");
            }

           
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtIssuedDateFrom.Text.Trim() == "")
        {
            Alert(true, "Issued Date From is required.");
        }
        else if (txtIssuedDateTo.Text.Trim() == "")
        {
            Alert(true, "Issued Date To is required.");
        }
        else if
        (!Helper.IsDate(txtIssuedDateFrom.Text.Trim()))
        {
            Alert(true, "Issued Date From is invalid format.");
        }
        else if (!Helper.IsDate(txtIssuedDateTo.Text.Trim()))
        {
            Alert(true, "Issued Date To is invalid format.");
        }
        else if (Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim()) > Helper.FormatDateTime(txtIssuedDateTo.Text.Trim()))
        {
            Alert(true, "Issued Date From must be smaller than Issued To Date.");
        }
        else if (ddlChannelItem.SelectedValue == "")
        {
            Alert(true, "Company is required.");
        }
        else
        {
            BindData();
        }
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_valid.PageIndex = e.NewPageIndex;

        DataTable tbl;
      
        tbl = (DataTable)Session["SS_DATA"];

        gv_valid.DataSource = tbl;
        gv_valid.DataBind();
    
    }
    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    void BindChannelItem()
    {
       
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
    void BindChannelLocation()
    {
        
        if (ddlChannelItem.SelectedIndex == 0)
        {
            cblChannelLocation.Items.Clear();
        }
        else
        {
            Helper.BindChanneLocation(cblChannelLocation, ddlChannelItem.SelectedValue);
        }
    }
    void BindData()
    {
        try
        {
            DateTime f_d = Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim());
            DateTime t_d = Helper.FormatDateTime(txtIssuedDateTo.Text.Trim());
            string ch_id = ddlChannel.SelectedIndex == 0 ? "" : ddlChannel.SelectedValue;
            string ch_item_id = ddlChannelItem.SelectedIndex == 0 ? "" : ddlChannelItem.SelectedValue;
         
            string ch_location_id = "";


            if (cblChannelLocation.Items.Count > 0)
            {
                if ((cblChannelLocation.SelectedIndex == 0 && cblChannelLocation.Items[0].Text.ToUpper() == "ALL") || cblChannelLocation.SelectedIndex == -1)
                {
                    ch_location_id = "";
                }
                else
                {
                    for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
                    {
                        if (cblChannelLocation.Items[i].Selected)
                        {
                            ch_location_id += ch_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
                        }
                    }
                }
            }
            DataTable tbl = da_banca.GetIncentiveReferral(f_d, t_d, ch_id, ch_item_id, ch_location_id);
            if (da_banca.SUCCESS)
            {

                //gv_valid.DataSource = tbl;
                //gv_valid.DataBind();

                gvIncentive.DataSource = tbl;
                gvIncentive.DataBind();

                lblRecords.Text = "Record(s): " + tbl.Rows.Count;

                if (tbl.Rows.Count > 0)
                {
                    btnExport.Attributes.Remove("disabled");

                  
                }
                else
                {
                    btnExport.Attributes.Add("disabled", "disabled");
                    Helper.Alert(false, "No record found.", lblError);
                }

               
            }
            else
            {
              
                //gv_valid.DataSource = null;
                //gv_valid.DataBind();


                gvIncentive.DataSource = null;
                gvIncentive.DataBind();
                lblRecords.Text = "";
            }
           
            Session["SS_DATA"] = tbl;
        }
        catch (Exception ex)
        {
           
            Alert(true, ex.Message);
        }
    }

    void Alert(bool is_error, string message)
    {
        Helper.Alert(is_error, message, lblError);
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region V1
        //BindChannelItem();
        //if (ddlChannel.SelectedIndex == 1)//selected individual
        //{
        //    // BindChannelLocation();
        //    ddlChannelItem.SelectedIndex = 1;
        //    ddlBranchName_SelectedIndexChanged(null, null);

        //    cblChannelLocation.SelectedIndex = 1;
        //    ddlChannelItem.Attributes.Add("disabled", "disabled");
          
        //}
        //else if (ddlChannel.SelectedIndex == 0)
        //{
        //    ddlChannelItem.Items.Clear();
           
        //    cblChannelLocation.Items.Clear();
        //}
        //else
        //{
        //    ddlChannelItem.Attributes.Remove("disabled");
           
        //    cblChannelLocation.Items.Clear();
        //    cblChannelLocation.Enabled = true;
        //}
        #endregion

        //V2
        LoadChannelItem();
    }
    protected void ddlBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
       // BindChannelLocation();
        //V2
        LoadChannelLocation(ddlChannelItem.SelectedValue);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tbl = (DataTable)Session["SS_DATA"];

            int colNo = tbl.Columns.Count;
            string colName = "";
           
            foreach (DataColumn c in tbl.Columns)
            {
                colName += colName != "" ? "," +c.ColumnName.ToString() : c.ColumnName.ToString();
            }
            string[] arrColName = colName.Split(',');
           
            //Helper.Alert(false, colName, lblError);

            //return;

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            Response.Clear();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

            Helper.excel.Sheet = sheet1;
        //    Helper.excel.HeaderText = new string[] {
        //"Branch Code", "Brand Name", "Staff ID", "Staff Name", "Policy Package1", "Policy Package2", "Incentive Package1", "Incentive Package2", "Total Incentive"
        //};
            Helper.excel.HeaderText = arrColName;
            Helper.excel.generateHeader();
            int row_no = 0;

            #region 
            //int totalPolicyPackage1 = 0;
            //int totalPolicyPackage2 = 0;
            //double incentivePackage1 = 0;
            //double incentivePackage2 = 0;
            //double totalIncentive = 0;
            //foreach (DataRow r in tbl.Rows)
            //{
            //    row_no += 1;
            //    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

            //    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
            //    Cell1.SetCellValue(r["office_code"].ToString());

            //    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
            //    Cell2.SetCellValue(r["office_name"].ToString());

            //    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
            //    Cell3.SetCellValue(r["referral_staff_id"].ToString());

            //    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
            //    Cell4.SetCellValue(r["referral_staff_Name"].ToString());

            //    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
            //    Cell5.SetCellValue(r["policy_package1"].ToString());

            //    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
            //    Cell6.SetCellValue(r["policy_package2"].ToString());

            //    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
            //    Cell7.SetCellValue(r["incentive_package1"].ToString());

            //    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
            //    Cell8.SetCellValue(r["incentive_package2"].ToString());

            //    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
            //    Cell9.SetCellValue(r["total_incentive"].ToString());
            //    totalPolicyPackage1 += Convert.ToInt32(r["policy_package1"].ToString());
            //    totalPolicyPackage2 += Convert.ToInt32(r["policy_package2"].ToString());
            //    incentivePackage1 += Convert.ToDouble(r["incentive_package1"].ToString());
            //    incentivePackage2 += Convert.ToDouble(r["incentive_package2"].ToString());
            //    totalIncentive += Convert.ToDouble(r["total_incentive"].ToString());
            //}
          
            //HSSFRow rowCell1 = (HSSFRow)sheet1.CreateRow(row_no + 1);
            //HSSFCell Celltotal = (HSSFCell)rowCell1.CreateCell(3);
            //Celltotal.SetCellValue("TOTAL");

            //HSSFCell CellpolicyPackage1 = (HSSFCell)rowCell1.CreateCell(4);
            //CellpolicyPackage1.SetCellValue(totalPolicyPackage1);

            //HSSFCell CellpolicyPackage2 = (HSSFCell)rowCell1.CreateCell(5);
            //CellpolicyPackage2.SetCellValue(totalPolicyPackage2);

            //HSSFCell CellIncentivePackage1 = (HSSFCell)rowCell1.CreateCell(6);
            //CellIncentivePackage1.SetCellValue(incentivePackage1);

            //HSSFCell CellIncentivePackage2 = (HSSFCell)rowCell1.CreateCell(7);
            //CellIncentivePackage2.SetCellValue(Convert.ToDouble(incentivePackage2));

            //HSSFCell CellTotalIncentive = (HSSFCell)rowCell1.CreateCell(8);
            //CellTotalIncentive.SetCellValue(Convert.ToDouble(totalIncentive));


            //string filename = "Referral_Insurance_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            //Response.Clear();
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            //MemoryStream file = new MemoryStream();
            //hssfworkbook.Write(file);


            //Response.BinaryWrite(file.GetBuffer());

            //Response.End();

            #endregion

            foreach (DataRow r in tbl.Rows)
            {
                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                for (int i = 0; i < colNo; i++)
                {
                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(i);
                    Cell1.SetCellValue(r[i].ToString());
                }

               
            }
            string filename = "Referral_Insurance_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);


            Response.BinaryWrite(file.GetBuffer());

            Response.End();
        }
        catch (Exception ex)
        {
            Alert(true, ex.Message);
        }
    }
    protected void cblChannelLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedCount = cblChannelLocation.Items.Cast<ListItem>().Count(li => li.Selected);
        int channelCount = cblChannelLocation.Items.Count;

        string result = Request.Form["__EVENTTARGET"];

        string[] checkedBox = result.Split('$'); ;

        int index = int.Parse(checkedBox[checkedBox.Length - 1]);//get check box index

        if (index == 0)
        {
            if (cblChannelLocation.Items[0].Selected)
            {
                for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
                {
                    cblChannelLocation.Items[i].Selected = true;
                }
            }
            else
            {
                cblChannelLocation.ClearSelection();
            }
        }
        else
        {
            if (selectedCount < channelCount && cblChannelLocation.Items[0].Selected == true)
            {
                cblChannelLocation.Items[0].Selected = false;
            }
            else if (selectedCount == channelCount - 1)
            {
                cblChannelLocation.Items[0].Selected = true;
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">Text message to show user</param>
    /// /// <param name="type">0=Success, 1=Error, 2=warning</param>
    void showMessage(string message, string type)
    {
        if (message.Trim() != "")
        {
            if (type == "0")
            {

                div_message.Attributes.CssStyle.Add("background-color", "#228B22");
            }
            else if (type == "1")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#f00");

            }
            else if (type == "2")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#ffcc00");
            }
            div_message.Attributes.CssStyle.Add("display", "block");
            div_message.InnerHtml = message;
        }
        else
        {
            div_message.Attributes.CssStyle.Add("display", "none");
        }
    }
}