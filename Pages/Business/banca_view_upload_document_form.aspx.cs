using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using iTextSharp.text;
using System.Configuration;
using System.Web.Security;
public partial class Pages_Business_banca_view_upload_document_form : System.Web.UI.Page
{
    string userID = "";
    string userName = "";
    Label application_id;
    Label TC;
    Label lblApp;
    Label lblCert;
    Label lblIDCard;
    Label lblPayslip;
    Label lblTC;
    // private List<bl_sys_user_role> permission { get { return (List<bl_sys_user_role>)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
        userID = Membership.GetUser().ProviderUserKey.ToString();
        userName = Membership.GetUser().UserName;
        lblError.Text = "";
        dv_preview.Visible = false;

       

        if (!Page.IsPostBack)
        {
            List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];

            if (Lrole != null)
            {
                if (Lrole[0].RoleId == "RCOM10" || Lrole[0].RoleId == "RCOM12")
                {
                    ViewState["VS_ROLE_IS_IA"] = true;// roleIsIA = true;
                    Helper.BindChannel(ddlChannel);
                    ddlChannel.SelectedIndex = 2;//cooperate
                    Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
                    bl_channel_sale_agent agentChannel = da_channel.GetChannelSaleAgentByUserName(userName);
                    string channelItem = da_channel.GetChannelItemIDByChannelLocationID(agentChannel.CHANNEL_LOCATION_ID);
                    Helper.SelectedDropDownListIndex("VALUE", ddlChannelItem, channelItem);
                    List<bl_channel_location> chList = da_channel.GetChannelLocationByUser(userName);
                    //bind channel location by user
                    Options.Bind(cblChannelLocation, chList, "Office_Code", "Channel_Location_ID", 0);
                    for (int i = 0; i < cblChannelLocation.Items.Count; i++)
                    {
                        cblChannelLocation.Items[i].Selected = true;
                    }
                    ddlChannel.Attributes.Add("disabled", "disabled");
                    ddlChannelItem.Attributes.Add("disabled", "disabled");

                    if (chList.Count > 1)
                    {
                        cblChannelLocation.Enabled = true;
                    }
                    else
                    {
                        cblChannelLocation.Enabled = false;
                    }

                }
                else
                {
                    ViewState["VS_ROLE_IS_IA"] = false;
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
                    cblChannelLocation.Enabled = true;
                }
            }
            else
            {
                Response.Redirect("../../login.aspx");
            }

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string ch_location_id = "";
        if (cblChannelLocation.Items.Count > 0)
        {
            for (int i = 0; i <= cblChannelLocation.Items.Count - 1; i++)
            {
                if (cblChannelLocation.Items[i].Selected)
                {
                    ch_location_id += ch_location_id == "" ? cblChannelLocation.Items[i].Value : "," + cblChannelLocation.Items[i].Value;
                }
            }
            loadGrid(ddltype.SelectedValue, txtFromDate.Text.Trim() == "" ? new DateTime(1900, 1, 1) : Helper.FormatDateTime(txtFromDate.Text), txtToDate.Text.Trim() == "" ? new DateTime(1900, 1, 1) : Helper.FormatDateTime(txtToDate.Text), txtcertNO.Text, ch_location_id);


        }

    }
    //private void loadGrid(string date_type, DateTime date_from, DateTime date_to, string policy_number)
    //{
    //    try
    //    {
    //        DataTable tbl = new DataTable();

    //        DB db = new DB();

    //        tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POL_CONTRACT_DOC_GET", new string[,] { 
    //    { "@DATE_TYPE", date_type},
    //    { "@DATE_F", date_from+""}, 
    //    {"@DATE_T", date_to+"" } ,
    //    {"@POLICY_NUMBER", policy_number} ,    
    //    }, "banca_view_upload_document_form.aspx.cs => loadGrid(string date_type,DateTime date_from,DateTime date_to,string policy_number)");
    //        //Show data on Gridview
    //        DataTable tbl_filter = tbl.Clone();

    //        if (permission.IsAdmin == 0)
    //        {
    //            foreach (DataRow r in tbl.Select("created_by='" + userName + "'"))
    //            {
    //                tbl_filter.ImportRow(r);
    //            }
    //        }
    //        else
    //        {
    //            tbl_filter = tbl;
    //        }

    //        gv_view.DataSource = tbl_filter;
    //        gv_view.DataBind();

    //        if (tbl_filter.Rows.Count == 0)// when search number is not found
    //        {
    //            Helper.Alert(true, "No record(s) found.", lblError);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Helper.Alert(true, ex.Message, lblError);
    //    }
    //}
    private void loadGrid(string date_type, DateTime date_from, DateTime date_to, string policy_number, string channel_location_id)
    {
        try
        {
            DataTable tbl = new DataTable();

            DB db = new DB();

            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POL_CONTRACT_DOC_GET_CHANEL_LOCATION_V1", new string[,] { 
        { "@DATE_TYPE", date_type},
        { "@DATE_F", date_from+""}, 
        {"@DATE_T", date_to+"" } ,
        {"@POLICY_NUMBER", policy_number} ,   
        {"@CHANNEL_LOCATION_ID",channel_location_id}
        }, "banca_view_upload_document_form.aspx.cs => loadGrid(string date_type, DateTime date_from, DateTime date_to, string policy_number,string channel_location_id)");
            //Show data on Gridview
            //DataTable tbl_filter = tbl.Clone();

            //if (permission.IsAdmin == 0)
            //{
            //    foreach (DataRow r in tbl.Select("created_by='" + userName + "'"))
            //    {
            //        tbl_filter.ImportRow(r);
            //    }
            //}
            //else
            //{
            //    tbl_filter = tbl;
            //}

            //gv_view.DataSource = tbl_filter;
            //gv_view.DataBind();

            //if (tbl_filter.Rows.Count == 0)// when search number is not found
            //{
            //    Helper.Alert(true, "No record(s) found.", lblError);
            //}


            if (tbl.Rows.Count == 0)// when search number is not found
            {
                gv_view.DataSource = null;
                gv_view.DataBind();

                Helper.Alert(true, "No record(s) found.", lblError);
            }
            else
            {
                gv_view.DataSource = tbl;
                gv_view.DataBind();

                string desc = txtcertNO.Text.Trim()=="" ?"" : string.Concat("Cert No:", txtcertNO.Text.Trim());
                desc +=  ddltype.SelectedIndex == 0 ? "": desc=="" ? string.Concat("Date Type:", ddltype.SelectedItem.Text) : string.Concat(" Date Type:", ddltype.SelectedItem.Text);
                desc +=  txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "" ? "" : desc=="" ? string.Concat("From:", txtFromDate.Text.Trim(), " To:", txtFromDate.Text.Trim()) : string.Concat(" From:", txtFromDate.Text.Trim(), " To:", txtFromDate.Text.Trim());
                desc += ddlChannel.SelectedIndex == 0 ? "" : desc=="" ? string.Concat("Channel:", ddlChannel.SelectedItem.Text) : string.Concat(" Channel:", ddlChannel.SelectedItem.Text);
                desc +=  ddlChannelItem.SelectedIndex == 0 ? "" : desc =="" ? string.Concat("Company:", ddlChannelItem.SelectedItem.Text) : string.Concat(" Company:", ddlChannelItem.SelectedItem.Text);


                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries certificate information with criteria [", desc,"]."));

            }
            Session["SSL_DATA"] = tbl;
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void gv_view_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        string app_id;
        //string doc_id="";
        try
        {

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow g_row;
            GridView g = sender as GridView;
            g_row = g.Rows[index];
            application_id = (Label)g_row.FindControl("lblapplicationID");
            Label ddlstu = (Label)g_row.FindControl("lblreviewstatus");
            app_id = application_id.Text;

            string policyNumber = ((Label)g_row.FindControl("lblpolicyNo")).Text;

            if (e.CommandName == "CMD_PREVIEW")
            {
                DataTable tbl = new DataTable();
                tbl = documents.PolicyContract.GetDocName(app_id);

                if (tbl.Rows.Count > 0)
                {

                    gv_preview.DataSource = tbl;
                    gv_preview.DataBind();
                    dv_preview.Visible = true;

                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat( "User click preview [Pol No. ", policyNumber,"]." ));

                }
                else
                {
                    gv_preview.DataSource = null;
                    gv_preview.DataBind();
                    Helper.Alert(true, "No Record (s) Found!", lblError);
                }
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        gv_view.DataSource = null;
        gv_view.DataBind();
        txtcertNO.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
    }
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Helper.BindChanneLocation(ddlChannelLocation, ddlChannelItem.SelectedValue);
        if (ddlChannelItem.SelectedIndex == 0)
        {
            cblChannelLocation.Items.Clear();
        }
        else
        {
            Helper.BindChanneLocation(cblChannelLocation, ddlChannelItem.SelectedValue);
        }
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindChannelItem(ddlChannelItem, ddlChannel.SelectedValue);
        if (ddlChannel.SelectedIndex == 1)
        {
            ddlChannelItem.SelectedIndex = 1;
            ddlChannelItem_SelectedIndexChanged(null, null);

            // ddlChannelLocation.SelectedIndex = 1;
            cblChannelLocation.SelectedIndex = 1;

            ddlChannelItem.Attributes.Add("disabled", "disabled");
            // ddlChannelLocation.Attributes.Add("disabled", "disabled");
            // cblChannelLocation.Enabled = false;
        }
        else
        {
            ddlChannelItem.Attributes.Remove("disabled");
            // ddlChannelLocation.Attributes.Remove("disabled");
            // ddlChannelLocation.Items.Clear();
            cblChannelLocation.Enabled = true;
            cblChannelLocation.Items.Clear();
        }
    }

    protected void cblChannelLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedCount = cblChannelLocation.Items.Cast<System.Web.UI.WebControls.ListItem>().Count(li => li.Selected);
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
    protected void gv_view_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_view.PageIndex = e.NewPageIndex;
        DataTable tbl = (DataTable)Session["SSL_DATA"];
        gv_view.DataSource = tbl;
        gv_view.DataBind();
    }
    public static bool BindChannel(DropDownList DDL)
    {
        bool result = false;
        List<bl_channel> ch = new List<bl_channel>();
        try
        {
            ch = da_channel.GetChannelListActive();
            DDL.Items.Clear();
            Options.Bind(DDL, ch, "Type", "Channel_ID", -1);
            result = true;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [BindChannel(DropDownList DDL)] in class [Helper], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
        return result;
    }
    public static bool BindChannelItem(DropDownList DDL, string CHANNEL_ID)
    {
        bool result = false;
        List<bl_channel_item> ch = new List<bl_channel_item>();
        try
        {
            // ch = da_channel.GetChannelItemListByChannel(CHANNEL_ID);
            ch = da_channel.GetChannelItemListMicroByChannel(CHANNEL_ID);
            DDL.Items.Clear();
            Options.Bind(DDL, ch, "Channel_Name", "Channel_Item_ID", 0);

            result = true;
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [BindChannelItem(DropDownList DDL, string CHANNEL_ID)] in class [Helper], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
        return result;
    }
    protected void gv_preview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label lblDocName;
        try
        {
            string app_path = "";
            string save_path = "";
            string doc_name = "";
            if (e.CommandName == "CMD_VIEW")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow g_row;
                GridView g = sender as GridView;
                g_row = g.Rows[index];

                lblDocName = (Label)g_row.FindControl("lblDocName");
                app_path = ((HiddenField)g_row.FindControl("hdfDocPath")).Value;
                doc_name = lblDocName.Text;
                save_path = Server.MapPath("~/Temp/" + doc_name);

                File.Copy(documents.PolicyContract.MainPath + app_path, save_path, true);


                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(doc_name) + "&view_type=multy');</script>", false);

                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views document [Doc Name: ", doc_name, "]."));

            }
            else if (e.CommandName == "CMD_VIEW_ALL")
            {
                byte[] mergedPdf;
                List<Byte[]> myFile = new List<byte[]>();
                string combineDocName = "";
                foreach (GridViewRow row in gv_preview.Rows)
                {

                    lblDocName = (Label)row.FindControl("lblDocName");
                    app_path = ((HiddenField)row.FindControl("hdfDocPath")).Value;
                    doc_name = lblDocName.Text;
                    save_path = Server.MapPath("~/Temp/" + doc_name);
                    File.Copy(documents.PolicyContract.MainPath + app_path, save_path, true);
                    myFile.Add(File.ReadAllBytes(Server.MapPath("~/Temp/" + doc_name)));
                    File.Delete(save_path);
                    combineDocName += doc_name=="" ? doc_name: doc_name + ",";
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    iTextSharp.text.Document Mydocument = new iTextSharp.text.Document();
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(Mydocument, ms);
                    Mydocument.Open();
                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.PdfImportedPage page;
                    iTextSharp.text.pdf.PdfReader reader;
                    int file_number = 0;
                    file_number = myFile.Count;
                    int file_index = 0;

                    foreach (byte[] file in myFile)
                    {
                        file_index += 1;
                        reader = new iTextSharp.text.pdf.PdfReader(file);
                        int pages = reader.NumberOfPages;

                        for (int i = 1; i <= pages; i++)
                        {

                            Mydocument.SetPageSize(PageSize.A4);
                            Mydocument.NewPage();
                            page = writer.GetImportedPage(reader, i);
                            cb.AddTemplate(page, 0, 0);
                        }
                    }

                    Mydocument.Close();
                    mergedPdf = ms.GetBuffer();
                    ms.Flush();
                    ms.Close();
                }
                string strGenerateFileName = userName + DateTime.Now.ToString("yyMMddhhmmss");
                File.WriteAllBytes(Server.MapPath("~/Temp/" + strGenerateFileName), mergedPdf);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(strGenerateFileName) + "&view_type=multy');</script>", false);
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views all documents [Doc Name: ", combineDocName, "]."));
            }
            dv_preview.Visible = true;
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string reviewed_remarks = "";
        string oldstatus = "";
        string doc_ID = "";
        string app_id = "";

        foreach (GridViewRow ros in gv_preview.Rows) //loop in grid view
        {
            DropDownList ddlstatus = (DropDownList)ros.FindControl("ddlcomfirm");
            TextBox txtremarks = (TextBox)ros.FindControl("txtremarks");
            Label docID = (Label)ros.FindControl("lbldocID");
            Label lblappID = (Label)ros.FindControl("lblappID");
            HiddenField hdfoldstatus = (HiddenField)ros.FindControl("hdfoldStatus");
            doc_ID = docID.Text;
            app_id = lblappID.Text;
            reviewed_remarks = txtremarks.Text;
            oldstatus = hdfoldstatus.Value;
            string docName= ((Label)ros.FindControl("lblDocName")).Text;
            if (ddlstatus.SelectedIndex > 0)
            {
                if (oldstatus != ddlstatus.SelectedValue)
                {
                    bool result = false;
                    result = documents.PolicyContract.UpdateReviewedStatus(doc_ID, ddlstatus.SelectedValue, userName, DateTime.Now, reviewed_remarks);
                    if (result)
                    {
                        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPDATE, string.Concat("User updates document status of [Doc Name:", docName, " to ", ddlstatus.SelectedItem.Text, "]."));

                        Helper.Alert(false, "Update successfully!", lblError);
                    }
                    else
                    {
                        Helper.Alert(true, "Update Fail!", lblError);
                    }
                }
            }
        }
       // loaddata(app_id);
        btnSearch_Click(null, null);
        dv_preview.Visible = true;
    }


    protected void gv_preview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string oldstatus = "";
        int confrim = 0;
        int reject = 0;
        foreach (GridViewRow row in gv_preview.Rows)
        {
            HiddenField hdfstatus = (HiddenField)row.FindControl("hdfoldStatus");
            DropDownList ddlsta = (DropDownList)row.FindControl("ddlcomfirm");
            oldstatus = hdfstatus.Value;
            if (oldstatus == "confirm")
            {
                ddlsta.SelectedValue = oldstatus;
                ddlsta.ForeColor = System.Drawing.Color.Blue;
                confrim += 1;

            }
            else if (oldstatus == "reject")
            {
                ddlsta.SelectedValue = oldstatus;
                ddlsta.ForeColor = System.Drawing.Color.Red;
                reject += 1;
            }

        }
        if (gv_preview.Rows.Count == confrim)
        {
            lblCount.Text = "completed!";
        }
        else
        {
            lblCount.Text = "Total files [" + gv_preview.Rows.Count + "] Confirm [" + confrim + "] Reject[" + reject + "] Pending[" + (gv_preview.Rows.Count - (confrim + reject)) + "]";
        }
        //else if (confrim > 0 && reject>0) {
        //    lblCount.Text = "Application confrim "+ confrim + ",reject "+ reject;
        //}
        //else if (confrim == 0 && reject == 0) {
        //    lblCount.Text = " All Application not update status yet";
        //}
        //else if (confrim + reject<gv_preview.Rows.Count)
        //{
        //    lblCount.Text = "Some application not update status yet";
        //}
        //(gv_preview.Rows.Count == reject) 

        //lblCount.Text = "Applications have status reject!" + reject+","+confrim; 
    }

    private void loaddata(string app_id)
    {
        DataTable tbl = new DataTable();
        tbl = documents.PolicyContract.GetDocName(app_id);

        if (tbl.Rows.Count > 0)
        {
            gv_preview.DataSource = tbl;
            gv_preview.DataBind();
            dv_preview.Visible = true;

        }
        else
        {
            gv_preview.DataSource = null;
            gv_preview.DataBind();
            Helper.Alert(true, "No Record (s) Found!", lblError);
        }
    }
}
