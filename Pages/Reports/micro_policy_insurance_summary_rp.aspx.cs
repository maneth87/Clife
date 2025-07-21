using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
public partial class Pages_Reports_micro_policy_insurance_summary_rp : System.Web.UI.Page
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
                ddlChannelItem_SelectedIndexChanged(null, null);
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
        
        user = Membership.GetUser().UserName;
        userRole = Roles.GetRolesForUser(user);

        if (!Page.IsPostBack)
        {
            txtIssuedDateFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtIssuedDateTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
            btnExport.Attributes.Add("disabled", "disabled");
            Session["SORTEX"] = "DESC";
            Session["SORTCOL"] = "total_policy";

                    
            List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];
            if (Lrole!=null)
            {
                if (Helper.BindChannel(ddlChannel))
                {
                    for (int i = 0; i < cblpolicystatus.Items.Count; i++)
                    {
                        cblpolicystatus.Items[i].Selected = true;
                    }

                    if (Lrole[0].RoleId == "RCOM10" || Lrole[0].RoleId == "RCOM12")
                    {
                        #region V1
                        //roleIsIA = true;
                        //Helper.BindChannel(ddlChannel);
                        //ddlChannel.SelectedIndex = 2;//cooperate
                        //Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
                        //bl_channel_sale_agent agentChannel = da_channel.GetChannelSaleAgentByUserName(user);
                        //string channelItem = da_channel.GetChannelItemIDByChannelLocationID(agentChannel.CHANNEL_LOCATION_ID);
                        ////channel
                        //Helper.SelectedDropDownListIndex("VALUE", ddlChannelItem, channelItem);
                        ////channel location
                        //List<bl_channel_location> chList = da_channel.GetChannelLocationByUser(user);
                        ////bind channel location by user
                        //Options.Bind(cblChannelLocation, chList, "Office_Code", "Channel_Location_ID", 0);
                        //for (int i = 0; i < cblChannelLocation.Items.Count; i++)
                        //{
                        //    cblChannelLocation.Items[i].Selected = true;
                        //}

                        //ddlChannel.Attributes.Add("disabled", "disabled");
                        //ddlChannelItem.Attributes.Add("disabled", "disabled");

                        //ddlGroup.Enabled = false;
                        //if (chList.Count > 1)
                        //{
                        //    cblChannelLocation.Enabled = true;
                        //}
                        //else
                        //{
                        //    cblChannelLocation.Enabled = false;
                        //}
                        #endregion V1
                        RoleIsIA = true;
                        ddlChannel.SelectedIndex = 2;
                        ddlChannel.Enabled = false;
                    }
                    else
                    {
                        RoleIsIA = false;
                        //if (!Helper.BindChannel(ddlChannel))
                        //{
                        //    Alert("Bind Channel Error", true);
                        //}
                        if (ddlChannel.Items.Count > 0)
                        {
                            ddlChannel.SelectedIndex = 2;
                            ddlChannel_SelectedIndexChanged(null, null);
                            ddlChannelItem.SelectedIndex = 0;
                            ddlChannelItem_SelectedIndexChanged(null, null);
                            for (int i = 0; i < cblChannelLocation.Items.Count; i++)
                            {
                                cblChannelLocation.Items[i].Selected = true;
                            }
                        }
                        ddlChannel.Attributes.Remove("disabled");
                        ddlChannelItem.Attributes.Remove("disabled");
                        // ddlChannelLocation.Attributes.Remove("disabled");
                        cblChannelLocation.Enabled = true;

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
        bool ckbChecked=false;
        foreach (ListItem user in cblChannelLocation.Items)
        {
            if (user.Selected)
            {
                ckbChecked = true;
                break;
            }
        }

        if (!Helper.IsDate(txtIssuedDateFrom.Text.Trim()) || !Helper.IsDate(txtIssuedDateTo.Text.Trim()))
        {
            Helper.Alert(true, "Issued Date From & To are requried with format [DD-MM-YYYY].", lblError);
        }
        else if (!ckbChecked)
        {
            Helper.Alert(true, "Please select branch code.", lblError);
        }
        else
        {
            BindData();
        }
       
    }
    void Alert(string MESSEGE, bool ERROR)
    {
        Helper.Alert(ERROR, MESSEGE, lblError);
    }
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region V1

        //if (ddlChannelItem.SelectedIndex == 0)
        //{
        //    cblChannelLocation.Items.Clear();
        //}
        //else
        //{
        //    ddlGroup.Items.Clear();
        //    Helper.BindChanneLocationGroup(ddlGroup, ddlChannelItem.SelectedValue);
        //    if (ddlGroup.Items.Count <= 1)
        //    {
        //        Helper.BindChanneLocation(cblChannelLocation, ddlChannelItem.SelectedValue);

        //    }

        //    ddlGroup_SelectedIndexChanged(null, null);
        //}
        #endregion V1
        LoadChannelLocation(ddlChannelItem.SelectedValue);
        if (!RoleIsIA)
        {
            ddlGroup.Items.Clear();
            Helper.BindChanneLocationGroup(ddlGroup, ddlChannelItem.SelectedValue);
            if (ddlGroup.Items.Count <= 1)
            {
                Helper.BindChanneLocation(cblChannelLocation, ddlChannelItem.SelectedValue);

            }

            ddlGroup_SelectedIndexChanged(null, null);
        }
    }
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGroup.SelectedIndex > 0)
        {
            //bind channel location by gorup
            Helper.BindChanneLocationByGroup(cblChannelLocation, ddlChannelItem.SelectedValue, ddlGroup.SelectedValue);
        }
        else
        {
            Helper.BindChanneLocation(cblChannelLocation, ddlChannelItem.SelectedValue);
        }
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
        //if (ddlChannel.SelectedIndex == 1)
        //{
        //    ddlChannelItem.SelectedIndex = 1;
        //    ddlChannelItem_SelectedIndexChanged(null, null);

        //    //ddlChannelLocation.SelectedIndex = 1;
        //    cblChannelLocation.SelectedIndex = 1;

        //    ddlChannelItem.Attributes.Add("disabled", "disabled");
        //    //ddlChannelLocation.Attributes.Add("disabled", "disabled");
        //    //cblChannelLocation.Enabled = false;
        //    ddlChannelItem.Attributes.Add("disabled", "disabled");
        //}
        //else
        //{
        //    ddlChannelItem.Attributes.Remove("disabled");
        //    //ddlChannelLocation.Attributes.Remove("disabled");
        //    //ddlChannelLocation.Items.Clear();
        //    cblChannelLocation.Attributes.Remove("disabled");
        //    cblChannelLocation.Items.Clear();
        //}

        LoadChannelItem();
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {

       
        HSSFWorkbook hssfworkbook = new HSSFWorkbook();

        Response.Clear();
        HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");

        Helper.excel.Sheet = sheet1;
        Helper.excel.HeaderText = new string[] {
         "Channel", "Company","Branch Code", "Branch Name", "Policy Package1", "Policy Package2", "Total Policy",  "SO (USD)","SO Discount (USD)", "SO After Discount (USD)", "DHC (USD)", "DHC Discount (USD)", "DHC After Discount (USD)", "Total Amount (USD)", "Total Discount (USD)", "Premium Package1 (USD)","Premium Package2 (USD)","Total Amount After Discount (USD)","Policy Status","Remarks"
        };

        Helper.excel.generateHeader();
        int row_no = 0;
            DataTable tbl = (DataTable)Session["SS_DATA"];
           // int totalPolicy = 0;
            //double totalSO, totalDHC, totalDiscount, totalAmount, totalAmountAfterDiscount;
            //totalSO = 0;
            //totalDHC = 0;
            //totalDiscount = 0;
            //totalAmount = 0;
            //totalAmountAfterDiscount = 0;
            int totalPolicyPackage1 = tbl.AsEnumerable().Sum(r => r.Field<int>("policy_package1"));
            int totalPolicyPackage2 = tbl.AsEnumerable().Sum(r => r.Field<int>("policy_package2"));
            int totalPolicy = tbl.AsEnumerable().Sum(r => r.Field<int>("total_policy"));
            decimal totalSO = tbl.AsEnumerable().Sum(r => r.Field<decimal>("so"));
            decimal totalSODiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("so_discount"));
            decimal totalSOAfterDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("so_after_discount"));
            decimal totalDHC = tbl.AsEnumerable().Sum(r => r.Field<decimal>("dhc"));
            decimal totalDHCDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("dhc_discount"));
            decimal totalDHCAfterDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("dhc_after_discount"));
            decimal totalAmount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("total_amount"));
            decimal totalDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("total_discount"));
            decimal totalAmountAfterDis = tbl.AsEnumerable().Sum(r => r.Field<decimal>("total_amount_after_discount"));
            decimal totalPremiumPackage1 = tbl.AsEnumerable().Sum(r => r.Field<decimal>("premium_package1"));
            decimal totalPremiumPackage2 = tbl.AsEnumerable().Sum(r => r.Field<decimal>("premium_package2"));

        foreach(DataRow r in tbl.Rows)//foreach (DataRow r in my_session.DATA.Rows)
        {
            row_no += 1;
            HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

            //totalPolicy += Convert.ToInt32(r["POLICY_ISSUED"].ToString());
            //totalSO += Convert.ToDouble(r["SO"].ToString());
            //totalDHC += Convert.ToDouble(r["Dhc"].ToString());
            //totalDiscount += Convert.ToDouble(r["discount"].ToString());
            //totalAmount += Convert.ToDouble(r["total_amount"].ToString());
            //totalAmountAfterDiscount += Convert.ToDouble(r["TOTAL_AMOUNT_AFTER_DISCOUNT"].ToString());

            HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
            Cell1.SetCellValue(r["DETAILS"].ToString());

            HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
            Cell2.SetCellValue(r["CHANNEL_NAME"].ToString());

            HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
            Cell3.SetCellValue(r["office_CODE"].ToString());

            HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
            Cell4.SetCellValue(r["office_name"].ToString());

            HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
            Cell5.SetCellValue(r["POLICY_Package1"].ToString());

            HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
            Cell6.SetCellValue(r["POLICY_Package2"].ToString());

            HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
            Cell7.SetCellValue(r["total_policy"].ToString());

            HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
            Cell8.SetCellValue(r["SO"].ToString());

            HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
            Cell9.SetCellValue(r["SO_DISCOUNT"].ToString());

            HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
            Cell10.SetCellValue(r["SO_AFTER_DISCOUNT"].ToString());

            HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
            Cell11.SetCellValue(r["DHC"].ToString());

            HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
            Cell12.SetCellValue(r["DHC_DISCOUNT"].ToString());

            HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
            Cell13.SetCellValue(r["DHC_AFTER_DISCOUNT"].ToString());

            HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
            Cell14.SetCellValue(r["TOTAL_AMOUNT"].ToString());

            HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
            Cell15.SetCellValue(r["TOTAL_Discount"].ToString());

            HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
            Cell16.SetCellValue(r["premium_package1"].ToString());

            HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
            Cell17.SetCellValue(r["premium_package2"].ToString());

            HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
            Cell18.SetCellValue(r["TOTAL_AMOUNT_AFTER_DISCOUNT"].ToString());

            HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
            Cell19.SetCellValue(r["policy_status"].ToString() == "IF" ? "Inforce" : r["policy_status"].ToString() == "TER" ? "Terminate" : r["policy_status"].ToString() == "CAN" ? "Cancel" : r["policy_status"].ToString() == "EXP" ? "Expire" : "");

            HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
            Cell20.SetCellValue(r["POLICY_STATUS_REMARKS"].ToString());

        }

        HSSFRow rowCell1 = (HSSFRow)sheet1.CreateRow(row_no+1);
        HSSFCell Celltotal = (HSSFCell)rowCell1.CreateCell(3);
        Celltotal.SetCellValue("TOTAL");

        HSSFCell CellpolicyPackage1 = (HSSFCell)rowCell1.CreateCell(4);
        CellpolicyPackage1.SetCellValue(totalPolicyPackage1);

        HSSFCell CellpolicyPackage2 = (HSSFCell)rowCell1.CreateCell(5);
        CellpolicyPackage2.SetCellValue(totalPolicyPackage2);

        HSSFCell Cellpolicy = (HSSFCell)rowCell1.CreateCell(6);
        Cellpolicy.SetCellValue(totalPolicy);

        HSSFCell Cellso = (HSSFCell)rowCell1.CreateCell(7);
        Cellso.SetCellValue(Convert.ToDouble(totalSO));

        HSSFCell CellsoDiscount = (HSSFCell)rowCell1.CreateCell(8);
        CellsoDiscount.SetCellValue(Convert.ToDouble(totalSODiscount));

        HSSFCell CellsoAfterDiscount = (HSSFCell)rowCell1.CreateCell(9);
        CellsoAfterDiscount.SetCellValue(Convert.ToDouble( totalSOAfterDiscount));

        HSSFCell Celldhc = (HSSFCell)rowCell1.CreateCell(10);
        Celldhc.SetCellValue(Convert.ToDouble(totalDHC));

        HSSFCell CelldhcDiscount = (HSSFCell)rowCell1.CreateCell(11);
        CelldhcDiscount.SetCellValue(Convert.ToDouble(totalDHCDiscount));

        HSSFCell CelldhcAfterDiscount= (HSSFCell)rowCell1.CreateCell(12);
        CelldhcAfterDiscount.SetCellValue(Convert.ToDouble(totalDHCAfterDiscount));

        HSSFCell CellTotalAmount = (HSSFCell)rowCell1.CreateCell(13);
        CellTotalAmount.SetCellValue(Convert.ToDouble(totalAmount));

        HSSFCell CellTotaldiscount = (HSSFCell)rowCell1.CreateCell(14);
        CellTotaldiscount.SetCellValue(Convert.ToDouble(totalDiscount));

        HSSFCell CellTotalPremiumPackage1 = (HSSFCell)rowCell1.CreateCell(15);
        CellTotalPremiumPackage1.SetCellValue(Convert.ToDouble(totalPremiumPackage1));

        HSSFCell CellTotalPremiumPackage2 = (HSSFCell)rowCell1.CreateCell(16);
        CellTotalPremiumPackage2.SetCellValue(Convert.ToDouble(totalPremiumPackage2));

        HSSFCell CellTotalAmountAfterDiscount = (HSSFCell)rowCell1.CreateCell(17);
        CellTotalAmountAfterDiscount.SetCellValue(Convert.ToDouble(totalAmountAfterDis));

        string filename = "Miro_policy_insurance_summary_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
            Alert( ex.Message,true);
        }
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_valid.PageIndex = e.NewPageIndex;

        DataTable tbl = (DataTable)Session["SS_DATA"];
       // tbl = my_session.DATA;

        gv_valid.DataSource = tbl;
        gv_valid.DataBind();
    }
    void BindData()
    {
       // System.Threading.Thread.Sleep(1000);
        try
        {
            DateTime f_d = Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim());
            DateTime t_d = Helper.FormatDateTime(txtIssuedDateTo.Text.Trim());
            string ch_id = ddlChannel.SelectedIndex == 0 ? "" : ddlChannel.SelectedValue;
            string ch_item_id = ddlChannelItem.SelectedIndex == 0 ? "" : ddlChannelItem.SelectedValue;
            string pol_status = cblpolicystatus.SelectedIndex == 0 ? "" : cblpolicystatus.SelectedValue;       
            //string ch_location_id = ddlChannelLocation.SelectedIndex == 0 ? "" : ddlChannelLocation.SelectedValue;
            string ch_location_id = "";


            //if (cblChannelLocation.Items.Count > 0)
            //{
            //    if ((cblChannelLocation.SelectedIndex == 0 && cblChannelLocation.Items[0].Text.ToUpper()=="ALL") || cblChannelLocation.SelectedIndex == -1)
            //    {
            //        ch_location_id = "";
            //    }
            //    else
            //    {
            //        for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
            //        {
            //            if (cblChannelLocation.Items[i].Selected)
            //            {
            //                ch_location_id += ch_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
            //            }
            //        }
            //    }
            //}

            //build channel location filter
            for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
            {
                if (cblChannelLocation.Items[i].Selected)
                {
                    ch_location_id += ch_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
                }
            }
            if (ch_location_id != "")
            {
                //DataTable tbl = da_micro_policy.GetPolicyInsuranceSummaryReport(f_d, t_d, ch_id, ch_item_id, ch_location_id);
                //kehong: get all policy status
                DataTable tbl = da_micro_policy.GetPolicyInsuranceSummaryReportV1(f_d, t_d, ch_id, ch_item_id, ch_location_id,pol_status);

                if (da_micro_policy.SUCCESS)
                {

                    gv_valid.DataSource = tbl;
                    gv_valid.DataBind();
                    lblRecords.Text = "Branch(s): " + tbl.Rows.Count;

                    int totalPolicy = tbl.AsEnumerable().Sum(r => r.Field<int>("total_policy"));
                    decimal totalSO = tbl.AsEnumerable().Sum(r => r.Field<decimal>("so"));
                    // decimal totalSODiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("so_discount"));
                    // decimal totalSOAfterDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("so_after_discount"));
                    decimal totalDHC = tbl.AsEnumerable().Sum(r => r.Field<decimal>("dhc"));
                    // decimal totalDHCDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("dhc_discount"));
                    // decimal totalDHCAfterDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("dhc_after_discount"));
                    // decimal totalAmount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("total_amount"));
                    decimal totalDiscount = tbl.AsEnumerable().Sum(r => r.Field<decimal>("total_discount"));
                    decimal totalAmountAfterDis = tbl.AsEnumerable().Sum(r => r.Field<decimal>("total_amount_after_discount"));

                    lblTotalPremium.Text = "   |   Policy : " + totalPolicy + "  |   SO: $" + totalSO.ToString("N") + "  |   DHC: $" + totalDHC.ToString("N") + "   |   Discount: $" + totalDiscount.ToString("N") + "   |   Total Amount After Discount: $" + totalAmountAfterDis.ToString("N");
                    if (tbl.Rows.Count > 0)
                    {
                        btnExport.Attributes.Remove("disabled");
                    }
                    else
                    {
                        btnExport.Attributes.Add("disabled", "disabled");
                    }
                }
                else
                {
                    Alert(da_micro_policy.MESSAGE, true);
                    gv_valid.DataSource = null;
                    gv_valid.DataBind();
                    lblRecords.Text = "";
                }
                //my_session.DATA = tbl;
                Session["SS_DATA"] = tbl;
            }
            else
            {
                Alert("Branch code is required.", false);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Erro function [BindData()] in page [micro_policy_insurance_summary_rp.aspx.cs], detail: " + ex.Message + "=>" + ex.StackTrace);
            Alert(ex.Message, true);
        }
    }

    protected void gv_valid_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable tbl = (DataTable)Session["SS_DATA"];
        if (Session["SORTEX"] + "" == "ASC")
        {
            Session["SORTEX"] = "DESC";
        }
        else
        {
            Session["SORTEX"] = "ASC";
        }
        Session["SORTCOL"] = e.SortExpression;


        string sortEx = "";
        string sortCol = "";
        sortEx = Session["SORTEX"] + "";
        sortCol = Session["SORTCOL"] + "";
        DataView dview = new DataView(tbl);
        if (sortEx != "")
        {
            dview.Sort = sortCol + " " + sortEx;

        }
        gv_valid.DataSource = dview;
        gv_valid.DataBind();
        /*show record count*/
       // div_count_record.InnerHtml = "Record Display:" + gvCustomer.Rows.Count + " Of " + tbl.Rows.Count;
    }
    protected void cblChannelLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedCount = cblChannelLocation.Items.Cast<ListItem>().Count(li => li.Selected);
        int channelCount=cblChannelLocation.Items.Count;
      
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
    protected void cblpolicystatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedCount = cblpolicystatus.Items.Cast<ListItem>().Count(li => li.Selected);
        int policycount = cblpolicystatus.Items.Count;

        string result = Request.Form["__EVENTTARGET"];

        string[] checkedBox = result.Split('$'); ;

        int index = int.Parse(checkedBox[checkedBox.Length - 1]);//get check box index

        if (index == 0)
        {
            if (cblpolicystatus.Items[0].Selected)
            {
                for (int i = 0; i <= cblpolicystatus.Items.Count - 1; i++)
                {
                    cblpolicystatus.Items[i].Selected = true;
                }
            }
            else
            {
                cblpolicystatus.ClearSelection();
            }
        }
        else
        {
            if (selectedCount < policycount && cblpolicystatus.Items[0].Selected == true)
            {
                cblpolicystatus.Items[0].Selected = false;
            }
            else if (selectedCount == policycount - 1)
            {
                cblpolicystatus.Items[0].Selected = true;
            }
        }
    }
}