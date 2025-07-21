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
using System.Threading;
using System.Threading.Tasks;
public partial class Pages_Reports_micro_policy_insurance_rp : System.Web.UI.Page
{
  
    string user = "";
    string[] userRole =  new string []{};
    //bool roleIsIA = false;

    private bool _IsAdmin { get { return (bool)ViewState["V_ADMIN"]; } set { ViewState["V_ADMIN"] = value; } }

    private bool _IsUpdate { get { return (bool)ViewState["V_UPDATED"]; } set { ViewState["V_UPDATED"] = value; } }

    int LoadChannelItem()
    {
        ddlChannelItem.Items.Clear();
        List<bl_channel_item> chList = new List<bl_channel_item>();
        List<bl_channel_item> chListFilter = new List<bl_channel_item>();
        bool RoleIsIA = (bool)ViewState["VS_ROLE_IS_IA"];
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
        bool RoleIsIA = (bool)ViewState["VS_ROLE_IS_IA"];

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
            Session["SORTEX"] = "ASC";
            Session["SORTCOL"] = "office_code";
            txtIssuedDateFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtIssuedDateTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
            btnExport.Attributes.Add("disabled", "disabled");

            List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];
            bl_sys_user_role u = (bl_sys_user_role)Session["SS_PERMISSION"];
            Helper.BindChannel(ddlChannel);
            if (Lrole != null && u!=null)
            {
                for (int i = 0; i < cblpolicystatus.Items.Count; i++)
                {
                    cblpolicystatus.Items[i].Selected = true;
                }
              
              
                _IsAdmin = u.IsAdmin == 1 ? true : false ;
                _IsUpdate = u.IsUpdate == 1 ? true : false;

             //   if (Lrole[0].RoleId == "RCOM10" || Lrole[0].RoleId == "RCOM12")
                if(u.IsView==1 && !_IsUpdate)
                {
                
                    #region V1
                    //ViewState["VS_ROLE_IS_IA"] = true;// roleIsIA = true;
                    //Helper.BindChannel(ddlChannel);
                    //ddlChannel.SelectedIndex = 2;//cooperate
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
                    ////ddlChannelLocation.Attributes.Add("disabled", "disabled");
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
                    ViewState["VS_ROLE_IS_IA"] = true;// roleIsIA = true;

                    ddlChannel.SelectedIndex = 2;
                    ddlChannel.Enabled = false;
                    #endregion V2
                    ViewState["VS_APP_LINK"] = "../Business/banca_micro_application_print.aspx?APP_ID={0}&A_TYPE={1}";
                }
                else if(_IsAdmin || _IsUpdate)
                {
                    ViewState["VS_ROLE_IS_IA"] = false;
                    if (!Helper.BindChannel(ddlChannel))
                    {
                        Alert("Bind Channel Error", true);
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
                    //ddlChannelLocation.Attributes.Remove("disabled");
                    cblChannelLocation.Enabled = true;
                }
                LoadChannelItem();
            }
            else
            {
                Response.Redirect("../../unauthorize.aspx");
            }
        }
    }
    void Alert(string MESSEGE, bool ERROR)
    { 
        Helper.Alert(ERROR ,MESSEGE,lblError);
    }

    void BindData()
    {
       // System.Threading.Thread.Sleep(1000);
        DateTime f_d = Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim());
        DateTime t_d = Helper.FormatDateTime(txtIssuedDateTo.Text.Trim());
        string ch_id= ddlChannel.SelectedIndex==0 ? "" : ddlChannel.SelectedValue;
        string ch_item_id=ddlChannelItem.SelectedIndex==0?"": ddlChannelItem.SelectedValue;
        string pol_status = cblpolicystatus.SelectedIndex == 0 ? "" : cblpolicystatus.SelectedValue;
        //string ch_location_id=ddlChannelLocation.SelectedIndex==0? "": ddlChannelLocation.SelectedValue;
                string ch_location_id = "";


        if (cblChannelLocation.Items.Count > 0)
        {
            #region V1
            //if ((cblChannelLocation.SelectedIndex == 0 && cblChannelLocation.Items[0].Text.ToUpper() == "ALL") || cblChannelLocation.SelectedIndex == -1)
            //{
            //    ch_location_id = "";
            //}
            //else
            //{
            //    for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
            //    {
            //        if (cblChannelLocation.Items[i].Selected)
            //        {
            //            ch_location_id += ch_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
            //        }
            //    }
            //}
            #endregion V1
            #region V2
            for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
            {
                if (cblChannelLocation.Items[i].Selected)
                {
                    ch_location_id += ch_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
                }
            }
            #endregion V2


        }
        //DataTable tbl = da_micro_policy.GetPolicyInsuranceReport(f_d, t_d, ch_id, ch_item_id, ch_location_id);
        //get all policy status
        DataTable tbl = da_micro_policy.GetPolicyInsuranceReportListing(f_d, t_d, ch_id, ch_item_id, ch_location_id, pol_status);
        DataTable tbl_filter = new DataTable();
        if (da_micro_policy.SUCCESS)
        {
           
                string condi = "";
                #region V1
                //if ((bool) ViewState["VS_ROLE_IS_IA"]==true)
                //{

                //    if (ddlPolicyStatus.SelectedIndex > 0)
                //    {

                //        condi = "created_by='" + user + "' and policy_status='" + ddlPolicyStatus.SelectedValue + "'";

                //    }
                //    else
                //    {
                       
                //        condi = "created_by='" + user + "'";
                //    }
                //}
                //else
                //{
                    
                //    ViewState["VS_ROLE_IS_IA"] = false;
                //    if (ddlPolicyStatus.SelectedIndex > 0)
                //    {

                //        condi = "policy_status='" + ddlPolicyStatus.SelectedValue + "'";

                //    }
                //}
                #endregion V1
                #region V2
                if (ddlPolicyStatus.SelectedIndex > 0)
                {

                    condi = "policy_status='" + ddlPolicyStatus.SelectedValue + "'";

                }
                #endregion V2


                //Condition to filter application number
            if(txtApplicationNumber.Text.Trim()!="")
            {
                condi += condi != "" ? " and application_number='" + txtApplicationNumber.Text.Trim() + "'" : "application_number='" + txtApplicationNumber.Text.Trim() + "'"; 

            }

                tbl_filter = tbl.Clone();


                foreach (DataRow r in tbl.Select(condi))
                {
                    tbl_filter.ImportRow(r);

                }
                tbl = tbl_filter;
            
                //gv_valid.DataSource = tbl_filter;
                //gv_valid.DataBind();

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
            

                lblRecords.Text = "Issued Policies : " + tbl.Rows.Count;

                double totalPremium = tbl.AsEnumerable().Sum(r => r.Field<double>("Total_AMOUNT"));
                lblTotalPremium.Text = "   |   Total Premium : $" + totalPremium.ToString("N");
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
            Alert(da_banca.MESSAGE,true);
            gv_valid.DataSource = null;
            gv_valid.DataBind();
            lblRecords.Text = "";
        }
        //my_session.DATA = tbl;
        Session["SS_DATA"] = tbl;
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
        //        Helper.BindChanneLocation(cblChannelLocation, ddlChannelItem.SelectedValue);
        //}
        #endregion v1
        #region v2
        LoadChannelLocation(ddlChannelItem.SelectedValue);
        #endregion v2
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtIssuedDateFrom.Text.Trim() == "")
        {
            Alert("Issued Date From is required.",true);
        }
        else if (txtIssuedDateTo.Text.Trim() == "")
        {
            Alert("Issued Date To is required.",true);
        }
        else if
        (!Helper.IsDate(txtIssuedDateFrom.Text.Trim()))
        {
            Alert("Issued Date From is invalid format.",true);
        }
        else if (!Helper.IsDate(txtIssuedDateTo.Text.Trim()))
        {
            Alert("Issued Date To is invalid format.",true);
        }
        else if (Helper.FormatDateTime(txtIssuedDateFrom.Text.Trim()) > Helper.FormatDateTime(txtIssuedDateTo.Text.Trim()))
        {
            Alert("Issued Date From must be smaller than Issued To Date.",true);
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
       "Company",
        "Brand Code",
        "Branch Name",
        "Customer No.",
        "Full Name KH",
        "Full Name EN",
        "Gender",
        "ID Type",
        "ID Number",
        "Date of Birth",
        "Issued Age",
        "Phone Number",
        "Email",
        "Province",
        "Application No.",
        "Policy No.",
        "Product ID",
        "Product Name",
        "Sum Assure",
        "Premium",
        "Discount Amount",
        "DHC",
        "DHC Premium",
        "DHC Discount Amount",
        "Total Amount",
        "Paymode",
        "Issued Date",
        "Effective Date",
        "Maturity Date",
        "Expiry Date",
        "Policyholder",
        "Primary Beneficiary's Name",
        "Contingent Benefitciary's Name",
        "Agent Code",
        "Agent Name",
        "Policy Status",
        "Payment Reference No.",
        "Remarks"
        };

        Helper.excel.generateHeader();
        int rownum = 0;
            DataTable tbl = (DataTable)Session["SS_DATA"];
            foreach (DataRow row1 in tbl.Rows)//foreach (DataRow r in my_session.DATA.Rows)
        {
            ++rownum;
            HSSFRow row2 = (HSSFRow)sheet1.CreateRow(rownum);
            ((HSSFCell)row2.CreateCell(0)).SetCellValue(row1["channel_name"].ToString());
            ((HSSFCell)row2.CreateCell(1)).SetCellValue(row1["office_code"].ToString());
            ((HSSFCell)row2.CreateCell(2)).SetCellValue(row1["office_name"].ToString());
            ((HSSFCell)row2.CreateCell(3)).SetCellValue(row1["customer_number"].ToString());
            ((HSSFCell)row2.CreateCell(4)).SetCellValue(row1["full_name_in_khmer"].ToString());
            ((HSSFCell)row2.CreateCell(5)).SetCellValue(row1["full_name_in_english"].ToString());
            ((HSSFCell)row2.CreateCell(6)).SetCellValue(row1["gender"].ToString());
            ((HSSFCell)row2.CreateCell(7)).SetCellValue(row1["id_type"].ToString());
            ((HSSFCell)row2.CreateCell(8)).SetCellValue(row1["id_number"].ToString());
            ((HSSFCell)row2.CreateCell(9)).SetCellValue(Convert.ToDateTime(row1["date_of_birth"].ToString()).ToString("dd-MM-yyyy"));
            ((HSSFCell)row2.CreateCell(10)).SetCellValue(row1["age"].ToString());
            ((HSSFCell)row2.CreateCell(11)).SetCellValue(row1["phone_number"].ToString());
            ((HSSFCell)row2.CreateCell(12)).SetCellValue(row1["email"].ToString());
            ((HSSFCell)row2.CreateCell(13)).SetCellValue(row1["province_kh"].ToString());
            ((HSSFCell)row2.CreateCell(14)).SetCellValue(row1["application_number"].ToString());
            ((HSSFCell)row2.CreateCell(15)).SetCellValue(row1["policy_number"].ToString());
            ((HSSFCell)row2.CreateCell(16 /*0x10*/)).SetCellValue(row1["product_id"].ToString());
            ((HSSFCell)row2.CreateCell(17)).SetCellValue(row1["product_name"].ToString());
            ((HSSFCell)row2.CreateCell(18)).SetCellValue(row1["sum_assure"].ToString());
            ((HSSFCell)row2.CreateCell(19)).SetCellValue(row1["premium"].ToString());
            ((HSSFCell)row2.CreateCell(20)).SetCellValue(row1["discount_amount"].ToString());
            ((HSSFCell)row2.CreateCell(21)).SetCellValue(row1["rider_sum_assure"].ToString());
            ((HSSFCell)row2.CreateCell(22)).SetCellValue(row1["rider_premium"].ToString());
            ((HSSFCell)row2.CreateCell(23)).SetCellValue(row1["rider_discount_amount"].ToString());
            ((HSSFCell)row2.CreateCell(24)).SetCellValue(row1["total_amount"].ToString());
            ((HSSFCell)row2.CreateCell(25)).SetCellValue(row1["mode"].ToString());
            ((HSSFCell)row2.CreateCell(26)).SetCellValue(Convert.ToDateTime(row1["issued_date"].ToString()).ToString("dd-MM-yyyy"));
            ((HSSFCell)row2.CreateCell(27)).SetCellValue(Convert.ToDateTime(row1["effective_date"].ToString()).ToString("dd-MM-yyyy"));
            ((HSSFCell)row2.CreateCell(28)).SetCellValue(Convert.ToDateTime(row1["maturity_date"].ToString()).ToString("dd-MM-yyyy"));
            ((HSSFCell)row2.CreateCell(29)).SetCellValue(Convert.ToDateTime(row1["expiry_date"].ToString()).ToString("dd-MM-yyyy"));
            ((HSSFCell)row2.CreateCell(30)).SetCellValue(row1["POLICYHOLDER_NAME"].ToString());
            ((HSSFCell)row2.CreateCell(31 /*0x1F*/)).SetCellValue(row1["PRIMARY_BENEFICIARY_NAME"].ToString());
            ((HSSFCell)row2.CreateCell(32 /*0x20*/)).SetCellValue(row1["BENEFICIARY_NAME"].ToString());
            ((HSSFCell)row2.CreateCell(33)).SetCellValue(row1["Agent_code"].ToString());
            ((HSSFCell)row2.CreateCell(34)).SetCellValue(row1["Agent_name"].ToString());
            ((HSSFCell)row2.CreateCell(35)).SetCellValue(row1["policy_status"].ToString() == "IF" ? "Inforce" : (row1["policy_status"].ToString() == "TER" ? "Terminate" : (row1["policy_status"].ToString() == "CAN" ? "Cancel" : (row1["policy_status"].ToString() == "EXP" ? "Expire" : ""))));
            ((HSSFCell)row2.CreateCell(36)).SetCellValue(row1["TRANSACTION_REFERRENCE_NO"].ToString());
            ((HSSFCell)row2.CreateCell(37)).SetCellValue(row1["POLICY_STATUS_REMARKS"].ToString());
        }
        string filename = "Miro_policy_insurance_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
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

        gv_valid.DataSource = tbl;
        gv_valid.DataBind();



    }
    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

            //LinkButton btn = (LinkButton)e.Row.FindControl("lbtApplication");
            //if ((bool)ViewState["VS_ROLE_IS_IA"])
            //{

            //   btn.Enabled = false;
            //}
            //else
            //{
            //    btn.Enabled = true;
            //}

        }
        
        
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region V1
        //Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
        //if (ddlChannel.SelectedIndex == 1)
        //{
        //    ddlChannelItem.SelectedIndex = 1;
        //    ddlChannelItem_SelectedIndexChanged(null, null);
        //    cblChannelLocation.SelectedIndex = 1;
        //    ddlChannelItem.Attributes.Add("disabled", "disabled");
        //}
        //else
        //{
        //    ddlChannelItem.Attributes.Remove("disabled");
        //    cblChannelLocation.Enabled = true;
        //    cblChannelLocation.Items.Clear();
        //}
        #endregion V1

        #region V2
        LoadChannelItem();
        #endregion V2
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

    protected void gv_valid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label app_id;
        string application_id = "";
        int index = -1;
        string url = "";
        try
        {

            if (e.CommandName == "CMD_EDIT")
            {
                index = Convert.ToInt32(e.CommandArgument);//% gv_valid.PageSize;
                GridViewRow g_row;
                GridView g = sender as GridView;
                g_row = g.Rows[index];
                app_id = (Label)g_row.FindControl("lblApplicationID");

                application_id = app_id.Text;
                if (ViewState["VS_APP_LINK"] + "" != "")
                {
                    url = string.Format(ViewState["VS_APP_LINK"] + "", application_id,"IND");
                }
                else
                {
                    url = "../Business/micro_application_form_edit.aspx?APP_ID=" + application_id;
                }
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "');</script>", false);

            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [gv_valid_RowCommand(object sender, GridViewCommandEventArgs e)] in class [banca_customer_lead.aspx.cs], detail: " + ex.Message + " => " + ex.StackTrace);
            Helper.Alert(true, "Ooop! system cannot convert lead to application, please contact your system administrator.", lblError);

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