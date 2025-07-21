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
public partial class Pages_Reports_banca_micro_insurance_booking_rp : System.Web.UI.Page
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

        user = Membership.GetUser().UserName;
        userRole = Roles.GetRolesForUser(user);
        List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];

        //if(Roles.IsUserInRole(user,"IA"))
        //{
        if (Lrole != null)
        {

            if (Lrole[0].RoleId == "RCOM10" || Lrole[0].RoleId == "RCOM12")
            {
                //my_session.IsIA = true;
                RoleIsIA = true;
            }
            else
            {
                //my_session.IsIA = false;
                RoleIsIA = false;

            }

            if (!Page.IsPostBack)
            {
                for (int i = 0; i < cblpolicystatus.Items.Count; i++)
                {
                    cblpolicystatus.Items[i].Selected = true;
                }
                Session["SORTEX"] = "ASC";
                Session["SORTCOL"] = "office_code";
                txtIssuedDateFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtIssuedDateTo.Text = DateTime.Now.ToString("dd-MM-yyyy");

                ddlPackage.Items.Add(new ListItem("---Select---", ""));
                ddlPackage.Items.Add(new ListItem("Package1", "Package1"));
                ddlPackage.Items.Add(new ListItem("Package2", "Package2"));

                if (Helper.BindChannel(ddlChannel))
                {

                    if (RoleIsIA)//(my_session.IsIA)
                    {
                        #region V1
                        //bl_channel_sale_agent channel_sale_agent = new bl_channel_sale_agent();
                        //channel_sale_agent = da_channel.GetChannelSaleAgentByUserName(user);// (my_session.USER_NAME);
                        ////channel_sale_agent = new bl_channel_sale_agent();
                        //if (channel_sale_agent.ID != null)
                        //{

                        //bl_channel_location channel = da_channel.GetChannelLocationByChannelLocationID(channel_sale_agent.CHANNEL_LOCATION_ID);
                        //ddlChannel.SelectedIndex = 2;
                        //Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
                        //bl_channel_sale_agent agentChannel = da_channel.GetChannelSaleAgentByUserName(user);
                        //string channelItem = da_channel.GetChannelItemIDByChannelLocationID(agentChannel.CHANNEL_LOCATION_ID);
                        //Helper.SelectedDropDownListIndex("VALUE", ddlChannelItem, channelItem);
                        //List<bl_channel_location> chList = da_channel.GetChannelLocationByUser(user);
                        ////bind channel location by user
                        //Options.Bind(cblChannelLocation, chList, "Office_Code", "Channel_Location_ID", 0);
                        //for (int i = 0; i < cblChannelLocation.Items.Count; i++)
                        //{
                        //    cblChannelLocation.Items[i].Selected = true;
                        //}
                        //ddlChannel.Attributes.Add("disabled", "disabled");
                        //ddlChannelItem.Attributes.Add("disabled", "disabled");
                        //cblChannelLocation.Enabled = false;
                        //if (chList.Count > 1)
                        //{
                        //    cblChannelLocation.Enabled = true;
                        //}
                        //else
                        //{
                        //    cblChannelLocation.Enabled = false;
                        //}



                        //}
                        //else
                        //{
                        //    Helper.Alert(false, "Branch location is not found.", lblError);
                        //    return;
                        //}
                        #endregion V1

                        #region V2
                        ddlChannel.SelectedIndex = 2;
                        ddlChannel.Enabled = false;
                        #endregion V2
                    }
                    else
                    {
                        if (ddlChannel.Items.Count > 0)
                        {
                            ddlChannel.SelectedIndex = 2;
                            ddlChannel_SelectedIndexChanged(null, null);
                            ddlChannelItem.SelectedIndex = 1;
                            ddlBranchName_SelectedIndexChanged(null, null);
                            for (int i = 0; i < cblChannelLocation.Items.Count; i++)
                            {
                                cblChannelLocation.Items[i].Selected = true;
                            }
                        }
                        ddlChannel.Attributes.Remove("disabled");
                        ddlChannelItem.Attributes.Remove("disabled");
                        //ddlChannelLocation.Attributes.Remove("disabled");
                        cblChannelLocation.Enabled = true;
                    }
                    LoadChannelItem();
                }
                else
                {

                    Helper.Alert(true, "Load channel error.", lblError);
                }
                btnExport.Attributes.Add("disabled", "disabled");
            }
        }
        else
        {
            Response.Redirect("../../unauthorize.aspx");
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
        //List<bl_channel_item> ch=  da_channel.GetChannelItemListByChannel( ddlChannel.SelectedValue);
        //ddlChannelItem.Items.Clear();
        //Options.Bind(ddlChannelItem, ch, "Channel_Name", "Channel_Item_ID", 0);

        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
    }
    void BindChannelLocation()
    {
        // List<bl_channel_location> ch = da_channel.GetChannelLocationListByChannelItemID(ddlChannelItem.SelectedValue);
        // ddlChannelLocation.Items.Clear();
        //Options.Bind(ddlChannelLocation,ch,"Office_Name", "Channel_Location_ID",0);
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
        //System.Threading.Thread.Sleep(1000);
        try
        {
            string condi = "";
            string channel_id = "";
            string channel_item_id = "";
            string channel_location_id = "";
            string policy_status = "";
            channel_id = ddlChannel.SelectedValue;
            channel_item_id = ddlChannelItem.SelectedValue;
            policy_status = cblpolicystatus.SelectedIndex == 0 ? "" : cblpolicystatus.SelectedValue;
            //channel_location_id = ddlChannelLocation.SelectedValue;
            #region V1
            //if (cblChannelLocation.Items.Count > 0)
            //{
            //    if ((cblChannelLocation.SelectedIndex == 0 && cblChannelLocation.Items[0].Text.ToUpper() == "ALL") || cblChannelLocation.SelectedIndex == -1)
            //    {
            //        channel_location_id = "";
            //    }
            //    else
            //    {
            //        for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
            //        {
            //            if (cblChannelLocation.Items[i].Selected)
            //            {
            //                channel_location_id += channel_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
            //            }
            //        }
            //    }
            //}
            #endregion V1

            #region V2
            if (cblChannelLocation.Items.Count > 0)
            {
                for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
                {
                    if (cblChannelLocation.Items[i].Selected)
                    {
                        channel_location_id += channel_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
                    }
                }
            }
            #endregion V2

            channel_location_id = channel_location_id.Replace(",", "' or channel_location_id='");
            condi = channel_id == "" ? "" : " channel_id='" + channel_id + "'";
            condi += condi == "" ? (channel_item_id == "" ? "" : " channel_item_id='" + channel_item_id + "'") : (channel_item_id == "" ? "" : " and channel_item_id='" + channel_item_id + "'");
            //condi += condi == "" ? (channel_location_id == "" ? "" : " channel_location_id='" + channel_location_id + "'") : (channel_location_id == "" ? "" : " and channel_location_id='" + channel_location_id + "'");
            condi += condi == "" ? (channel_location_id == "" ? "" : " channel_location_id ='" + channel_location_id + "'") : (channel_location_id == "" ? "" : " and (channel_location_id ='" + channel_location_id + "')");

            condi += condi == "" ? (ddlPackage.SelectedValue == "" ? "" : " package='" + ddlPackage.SelectedValue + "'") : (ddlPackage.SelectedValue == "" ? "" : " and package='" + ddlPackage.SelectedValue + "'");
            condi += condi == "" ? (policy_status == "" ? "" : " policy_status ='" + policy_status + "'") : (policy_status == "" ? "" : " and (policy_status ='" + policy_status + "')");

            //DataTable tbl = da_banca.GetDataInsuranceBookingHTB(Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim()), Helper.FormatDateTime(txtIssuedDateTo.Text.Trim()));
            //kehong:new function specific policy status
            DataTable tbl = da_banca.GetDataInsuranceBookingHTBByPolicyStatus(Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim()), Helper.FormatDateTime(txtIssuedDateTo.Text.Trim()));

            //Helper.Alert(false, condi, lblError);

            DataTable tbl1 = tbl.Clone();

            foreach (DataRow r in tbl.Select(condi))
            {

                tbl1.ImportRow(r);

            }

            // List<bl_daily_insurance_booking_htb> list = da_banca.GetDailyInsuranceBookingHTB(Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim()), Helper.FormatDateTime(txtIssuedDateTo.Text.Trim()));
            if (da_banca.SUCCESS)
            {
                //gv_valid.DataSource = tbl1;// list;
                //gv_valid.DataBind();

                string sortEx = "";
                string sortCol = "";
                sortEx = Session["SORTEX"] + "";
                sortCol = Session["SORTCOL"] + "";
                DataView dview = new DataView(tbl1);
                if (sortEx != "")
                {
                    dview.Sort = sortCol + " " + sortEx;

                }
                gv_valid.DataSource = dview;
                gv_valid.DataBind();

                lblRecords.Text = "Issued Policies : " + tbl1.Rows.Count;
                double totalPremium = tbl1.AsEnumerable().Sum(r => r.Field<double>("Amount"));
                lblTotalPremium.Text = "   |   Total Premium : $" + totalPremium.ToString("N");

                if (tbl1.Rows.Count > 0)
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
                Alert(true, da_banca.MESSAGE);
                gv_valid.DataSource = null;
                gv_valid.DataBind();
                lblRecords.Text = "No Record Found.";
                lblTotalPremium.Text = "";
            }
            // my_session.DATA = tbl1;
            Session["SS_DATA"] = tbl1;
        }
        catch (Exception ex)
        {
            // Helper.ShowMsgBox("ERROR", ex.Message, lblError, "white", "red", "error");
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

        //    // ddlChannelLocation.SelectedIndex = 1;
        //    cblChannelLocation.SelectedIndex = 1;
        //    ddlChannelItem.Attributes.Add("disabled", "disabled");
        //    //ddlChannelLocation.Attributes.Add("disabled", "disabled");
        //    // cblChannelLocation.Enabled = false;
        //}
        //else if (ddlChannel.SelectedIndex == 0)
        //{
        //    ddlChannelItem.Items.Clear();
        //    //ddlChannelLocation.Items.Clear();
        //    cblChannelLocation.Items.Clear();
        //}
        //else
        //{
        //    ddlChannelItem.Attributes.Remove("disabled");
        //    //ddlChannelLocation.Attributes.Remove("disabled");
        //    // ddlChannelLocation.Items.Clear();
        //    cblChannelLocation.Items.Clear();
        //    cblChannelLocation.Enabled = true;
        //}
        #endregion v1
        //V2
        LoadChannelItem();

    }
    protected void ddlBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindChannelLocation();
        //V2
        LoadChannelLocation(ddlChannelItem.SelectedValue);
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
        "Branch Code", "Brand Name", "Application ID", "Client Type", "Certificate No.", "Payment Reference No.", "Referral Staff ID", "Referral Staff Name", "Referral Staff Position", "CIF", 
        "Client Name (English)", "Client Name (Khmer)", "Date of Birth", "Phone Number", "Gender", "ID Type", "ID Number", "Village", "Commune", "District", "Province","Effective Date", "Maturity Date",
        "Insurance Toner (Y)", "Permium", "Currency", "Insurance Type","Package Type","Insurance Status","Referral Fee", "Referral Incentive","IA Name","Referral Date", "Issued Date","Policy Status","Policy Remarks", "Insurance Application Number"
        };

            Helper.excel.generateHeader();
            int row_no = 0;
            DataTable tbl = (DataTable)Session["SS_DATA"];
            foreach (DataRow r in tbl.Rows)// (DataRow r in my_session.DATA.Rows)
            {
                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(r["office_code"].ToString());

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(r["office_name"].ToString());

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(r["application_id"].ToString());

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(r["client_type"].ToString());

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue(r["policy_number"].ToString());

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(r["payment_reference_no"].ToString());

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(r["referral_staff_id"].ToString());

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue(r["referral_staff_name"].ToString());

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                Cell9.SetCellValue(r["referral_staff_position"].ToString());

                HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                Cell10.SetCellValue(r["cif"].ToString());

                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                Cell11.SetCellValue(r["last_name_in_english"].ToString() + " " + r["first_name_in_english"].ToString());

                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                Cell12.SetCellValue(r["last_name_in_khmer"].ToString() + " " + r["first_name_in_khmer"].ToString());

                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                Cell13.SetCellValue(Convert.ToDateTime(r["date_of_birth"].ToString()).ToString("dd-MMM-yyyy"));

                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                Cell14.SetCellValue(r["phone_number"].ToString());

                HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                Cell15.SetCellValue(r["gender"].ToString() == "0" ? "Female" : "Male");

                HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                Cell16.SetCellValue(r["id_type_text"].ToString());

                HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                Cell17.SetCellValue(r["id_number"].ToString());

                HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                Cell18.SetCellValue(r["village_en"].ToString());

                HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                Cell19.SetCellValue(r["commune_en"].ToString());

                HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                Cell20.SetCellValue(r["district_en"].ToString());

                HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);
                Cell21.SetCellValue(r["province_en"].ToString());

                HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(21);
                Cell22.SetCellValue(Convert.ToDateTime(r["effective_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(22);
                Cell23.SetCellValue(Convert.ToDateTime(r["maturity_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell24 = (HSSFCell)rowCell.CreateCell(23);
                Cell24.SetCellValue(r["term_of_cover"].ToString());

                HSSFCell Cell25 = (HSSFCell)rowCell.CreateCell(24);
                Cell25.SetCellValue(r["amount"].ToString());

                HSSFCell Cell26 = (HSSFCell)rowCell.CreateCell(25);
                Cell26.SetCellValue(r["currency"].ToString());

                HSSFCell Cell27 = (HSSFCell)rowCell.CreateCell(26);
                Cell27.SetCellValue("Micro Insurance");

                HSSFCell Cell28 = (HSSFCell)rowCell.CreateCell(27);
                Cell28.SetCellValue(r["package"].ToString());

                HSSFCell Cell29 = (HSSFCell)rowCell.CreateCell(28);
                Cell29.SetCellValue(r["policy_status"].ToString());

                HSSFCell Cell30 = (HSSFCell)rowCell.CreateCell(29);
                Cell30.SetCellValue(r["referral_fee"].ToString());

                HSSFCell Cell31 = (HSSFCell)rowCell.CreateCell(30);
                Cell31.SetCellValue(r["referral_incentive"].ToString());

                HSSFCell Cell32 = (HSSFCell)rowCell.CreateCell(31);
                Cell32.SetCellValue(r["agent_name_en"].ToString());

                HSSFCell Cell33 = (HSSFCell)rowCell.CreateCell(32);
                Cell33.SetCellValue(Convert.ToDateTime(r["referred_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell34 = (HSSFCell)rowCell.CreateCell(33);
                Cell34.SetCellValue(Convert.ToDateTime(r["issued_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell35 = (HSSFCell)rowCell.CreateCell(34);
                Cell35.SetCellValue(r["policy_status"].ToString() == "IF" ? "Inforce" : r["policy_status"].ToString() == "TER" ? "Terminate" : r["policy_status"].ToString() == "CAN" ? "Cancel" : r["policy_status"].ToString() == "EXP" ? "Expire" : "");


                HSSFCell Cell36 = (HSSFCell)rowCell.CreateCell(35);
                Cell36.SetCellValue(r["POLICY_STATUS_REMARKS"].ToString());

                HSSFCell Cell37 = (HSSFCell)rowCell.CreateCell(36);
                Cell37.SetCellValue(r["application_number"].ToString());

            }
            string filename = "Insurance_Booking_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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

        //Data table after sorting
        Session["SS_DATA"] = dview.ToTable();
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