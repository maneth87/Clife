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
public partial class Pages_Reports_mirco_customer_lead_rp : System.Web.UI.Page
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
         

                txtReferredDateFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtReferredDateTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
                btnExport.Attributes.Add("disabled", "disabled");
                List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];

                
                    if (Lrole != null)
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
                                //RoleIsIA = true;
                                //Helper.BindChannel(ddlChannel);
                                //ddlChannel.SelectedIndex = 2;//cooperate
                                //Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
                                //bl_channel_sale_agent agentChannel = da_channel.GetChannelSaleAgentByUserName(user);// (my_session.User);
                                //string channelItem = da_channel.GetChannelItemIDByChannelLocationID(agentChannel.CHANNEL_LOCATION_ID);
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
                                //cblChannelLocation.Enabled = false;
                                //if (chList.Count > 1)
                                //{
                                //    cblChannelLocation.Enabled = true;
                                //}
                                //else
                                //{
                                //    cblChannelLocation.Enabled = false;
                                //}
                                #endregion V1

                                #region V2
                                RoleIsIA = true;
                                ddlChannel.SelectedIndex = 2;
                                ddlChannel.Enabled = false;
                                #endregion V2
                            }
                            else
                            {
                                RoleIsIA = false;
                                if (!Helper.BindChannel(ddlChannel))
                                {

                                    Helper.Alert(true, "Bind Channel Error", lblError);
                                }
                                if (ddlChannel.Items.Count > 0)
                                {
                                    ddlChannel.SelectedIndex = 2;
                                    ddlChannel_SelectedIndexChanged(null, null);
                                    ddlChannelItem.SelectedIndex = 1;
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
    void BindData()
    {
       // System.Threading.Thread.Sleep(1000);
        try
        {
            //DataTable tbl = da_customer_lead.GetLeadReport(Helper.FormatDateTime(txtReferredDateFrom.Text.Trim()), Helper.FormatDateTime(txtReferredDateTo.Text.Trim()));GetLeadReportByPolicyStatus
            DataTable tbl = da_customer_lead.GetLeadReportByPolicyStatus(Helper.FormatDateTime(txtReferredDateFrom.Text.Trim()), Helper.FormatDateTime(txtReferredDateTo.Text.Trim()));
            DataTable filter = tbl.Clone();
            string condi = "";
            string channel_id = "";
            string channel_item_id = "";
            string channel_location_id = "";
            string policy_status = "";
            channel_id = ddlChannel.SelectedValue;
            channel_item_id = ddlChannelItem.SelectedValue;
            policy_status = cblpolicystatus.SelectedIndex == 0 ? "": cblpolicystatus.SelectedValue;
            // channel_location_id = ddlChannelLocation.SelectedValue;

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
            condi += condi == "" ? (channel_location_id == "" ? "" : " channel_location_id ='" + channel_location_id + "'") : (channel_location_id == "" ? "" : " and channel_location_id ='" + channel_location_id + "'");
            condi += condi == "" ? (policy_status == "" ? "" : " policy_status ='" + policy_status + "'") : (policy_status == "" ? "" : " and policy_status ='" + policy_status + "'");

            foreach (DataRow r in tbl.Select(condi))
            {
                filter.ImportRow(r);
            }
            if (da_customer_lead.SUCCESS)
            {
                gv_valid.DataSource = filter;// list;
                gv_valid.DataBind();

                double countIssuedPolicy = filter.AsEnumerable().Count(r => r.Field<string>("status")=="Approved");
                double countLead = filter.Rows.Count;
                double successPercent = 0;
                if (countLead > 0)
                {
                    successPercent = Math.Round( (countIssuedPolicy / countLead)*100,2);
                }
                lblRecords.Text = "Leads: " + countLead + " | Issued Policies: " + countIssuedPolicy + " | Success Rate: " + successPercent + "%";
                if (filter.Rows.Count > 0)
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
                Helper.Alert(true, da_customer_lead.MESSAGE, lblError);
                gv_valid.DataSource = null;
                gv_valid.DataBind();
                lblRecords.Text = "No Record Found.";
            }
          //  my_session.DATA = filter;
            Session["SS_DATA"] = filter;
        }
        catch (Exception ex)
        {
            Helper.Alert(true, "Load Data Error: "+ ex.Message, lblError);
            Log.AddExceptionToLog("Error function [BindData()] in class [micro_customer_lead_rp_aspx.cs], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
    }

    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region V1
        //Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
        //if (ddlChannel.SelectedIndex == 1)//selected individual
        //{
        //    ddlChannelItem.SelectedIndex = 1;
        //    ddlChannelItem_SelectedIndexChanged(null, null);
        //    //ddlChannelLocation.SelectedIndex = 1;
        //    cblChannelLocation.SelectedIndex = 1;

        //    ddlChannelItem.Attributes.Add("disabled", "disabled");
        //   // ddlChannelLocation.Attributes.Add("disabled", "disabled");
        //}
        //else if (ddlChannel.SelectedIndex == 0)
        //{
        //    ddlChannelItem.Items.Clear();
        //   // ddlChannelLocation.Items.Clear();
        //    cblChannelLocation.Items.Clear();
        //}
        //else
        //{
        //    ddlChannelItem.Attributes.Remove("disabled");
        //   // ddlChannelLocation.Attributes.Remove("disabled");
        //    //ddlChannelLocation.Items.Clear();
        //    cblChannelLocation.Items.Clear();
        //    cblChannelLocation.Enabled = true;
        //}
        #endregion V1
        //V2
        LoadChannelItem();

    }

    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region V1
        ////Helper.BindChanneLocation(ddlChannelLocation, ddlChannelItem.SelectedValue);
        //if (ddlChannelItem.SelectedIndex == 0)
        //{
        //    cblChannelLocation.Items.Clear();
        //}
        //else
        //{
        //    Helper.BindChanneLocation(cblChannelLocation, ddlChannelItem.SelectedValue);
        //}
        #endregion v1
        //V2
        LoadChannelLocation(ddlChannelItem.SelectedValue);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
  
        if (txtReferredDateFrom.Text.Trim() == "" )
        {
           Helper.  Alert(true, "Referred Date From is required.", lblError);
        }
        else if (txtReferredDateTo.Text.Trim() == "")
        {
        Helper.    Alert(true, "Referred Date To is required.", lblError);
        }
        else if
        (!Helper.IsDate(txtReferredDateFrom.Text.Trim()))
        {
          Helper.  Alert(true, "Referred Date From is invalid format.",lblError);
        }
        else if (!Helper.IsDate(txtReferredDateTo.Text.Trim()))
        {
         Helper.   Alert(true, "Referred Date To is invalid format.", lblError);
        }
        else if (Helper.FormatDateTime(txtReferredDateFrom.Text.Trim()) > Helper.FormatDateTime(txtReferredDateTo.Text.Trim()))
        {
            Helper.Alert(true, "Referred Date From must be smaller than Transaction To Date.", lblError);
        }
        else
        {
            BindData();
        }
    
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
        "Company","Branch Code", "Brand Name", "Application ID",   "Referral Staff ID", "Referral Staff Name", "Referral Staff Position", "Client Type","CIF", 
        "Client Name (English)", "Client Name (Khmer)",  "Gender", "Nationality","Date of Birth",  "Village", "Commune", "District", "Province","ID Type", "ID Number","Phone Number",
        "Referred Date","Issued Date", "Status","Status Remarks", "Insurance Application No.","Policy Status","Agent Code", "Agent Name"
        };

            Helper.excel.generateHeader();
            int row_no = 0;
            DataTable tbl = (DataTable)Session["SS_DATA"];
            foreach(DataRow r in tbl.Rows)// (DataRow r in my_session.DATA.Rows)
            {
                row_no += 1;
                HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                Cell1.SetCellValue(r["channel_name"].ToString());

                HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                Cell2.SetCellValue(r["branch_code"].ToString());

                HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                Cell3.SetCellValue(r["branch_name"].ToString());

                HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                Cell4.SetCellValue(r["application_id"].ToString());

                HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                Cell5.SetCellValue(r["referral_staff_id"].ToString());

                HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                Cell6.SetCellValue(r["referral_staff_name"].ToString());

                HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                Cell7.SetCellValue(r["referral_staff_position"].ToString());

                HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                Cell8.SetCellValue(r["client_type"].ToString());

                HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                Cell9.SetCellValue(r["cif"].ToString());

                HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(9);
                Cell11.SetCellValue(r["client_name_in_english"].ToString() );

                HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(10);
                Cell12.SetCellValue(r["client_name_in_khmer"].ToString() );

                HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(11);
                Cell13.SetCellValue(r["gender"].ToString());

                HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(12);
                Cell14.SetCellValue(r["nationality"].ToString());

                HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(13);
                Cell15.SetCellValue(Convert.ToDateTime( r["date_of_birth"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(14);
                Cell16.SetCellValue(r["village"].ToString());

                HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(15);
                Cell17.SetCellValue(r["commune"].ToString());

                HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(16);
                Cell18.SetCellValue(r["district"].ToString());

                HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(17);
                Cell19.SetCellValue(r["province"].ToString());

                HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(18);
                Cell20.SetCellValue(r["id_type"].ToString());

                HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(19);
                Cell21.SetCellValue(r["id_number"].ToString());

                HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(20);
                Cell22.SetCellValue(r["phone_number"].ToString());

                HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(21);
                Cell23.SetCellValue(Convert.ToDateTime(r["referred_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell24 = (HSSFCell)rowCell.CreateCell(22);
                Cell24.SetCellValue(Convert.ToDateTime(r["issued_date"].ToString()).ToString("dd-MM-yyyy") == "01-01-1900" ? "" : Convert.ToDateTime(r["issued_date"].ToString()).ToString("dd-MM-yyyy"));

                HSSFCell Cell25 = (HSSFCell)rowCell.CreateCell(23);
                Cell25.SetCellValue(r["status"].ToString());

                HSSFCell Cell26 = (HSSFCell)rowCell.CreateCell(24);
                Cell26.SetCellValue(r["status_remarks"].ToString());

                HSSFCell Cell27 = (HSSFCell)rowCell.CreateCell(25);
                Cell27.SetCellValue(r["insurance_application_number"].ToString());

                HSSFCell Cell28 = (HSSFCell)rowCell.CreateCell(26);
                Cell28.SetCellValue(r["policy_status"].ToString() == "IF" ? "Inforce" : r["policy_status"].ToString() == "TER" ? "Terminate" : r["policy_status"].ToString() == "CAN" ? "Cancel" : r["policy_status"].ToString() == "EXP" ? "Expire" : "");

                HSSFCell Cell29 = (HSSFCell)rowCell.CreateCell(27);
                Cell29.SetCellValue(r["agent_code"].ToString());

                HSSFCell Cell30 = (HSSFCell)rowCell.CreateCell(28);
                Cell30.SetCellValue(r["agent_name_en"].ToString());


            }
            string filename = "client_lead_report_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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
            Helper.Alert(true, ex.Message,lblError);
        }
    }
    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        foreach (GridViewRow row in gv_valid.Rows)
        {
            Label lAgentCode = (Label)row.FindControl("lblAgentCode");
           
            Label lAgentname = (Label)row.FindControl("lblAgentName");

            if (lAgentCode.Text.Trim()=="" )
            {
                lAgentCode.Text = "";
            }

            
        }
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_valid.PageIndex = e.NewPageIndex;

        DataTable tbl;
       // tbl = my_session.DATA;

        tbl = (DataTable)Session["SS_DATA"];
        gv_valid.DataSource = tbl;
        gv_valid.DataBind();
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