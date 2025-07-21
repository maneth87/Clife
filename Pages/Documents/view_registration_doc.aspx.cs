using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
public partial class Pages_Documents_view_registration_doc : System.Web.UI.Page
{
    private bl_sys_user_role permission { get { return (bl_sys_user_role)ViewState["VS_PERMISSION"]; } set { ViewState["VS_PERMISSION"] = value; } }

    private void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(permission.UserName, permission.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        permission = (bl_sys_user_role)Session["SS_PERMISSION"];
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (Helper.IsDate(txtFromDate.Text) && Helper.IsDate(txtToDate.Text))
        {

            var obj = documents.TrascationFiles.GetDocList(txtFromDate.Text.Trim(), txtToDate.Text.Trim(), txtFileDescription.Text.Trim());
            if (obj.Count > 0)
            {
                gv_view.DataSource = obj;
                gv_view.DataBind();
                Session["SSL_DATA"] = obj;
                string desc = string.Concat("Upload Date from:", txtFromDate.Text.Trim(), " to:", txtToDate.Text.Trim());
                desc += txtFileDescription.Text.Trim() == "" ? "" : string.Concat( " File Description:", txtFileDescription.Text.Trim());
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat( "User inquiries registration documents with criteria [", desc,"]."));
            }
            else
            {
                Helper.Alert(false, "No record found.", lblError);
            }
        }
        else
        {
            Helper.Alert(true, "Upload Date From & To are required in format [DD/MM/YYYY].", lblError);
        }
    }
    protected void gv_view_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      
        try
        {

            string path = "";
            string save_path = "";
            string doc_name = "";
            string newFileName = "";
            if (e.CommandName == "CMD_PREVIEW")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow g_row;
                GridView g = sender as GridView;
                g_row = g.Rows[index];
                newFileName = Guid.NewGuid().ToString();
               Label lblDocName = (Label)g_row.FindControl("lblDocName");
                path = ((Label)g_row.FindControl("lblDocPath")).Text;
                doc_name = lblDocName.Text;
              //  save_path = Server.MapPath("~/Temp/" + doc_name);
                if (File.Exists(documents.TrascationFiles.Path + path))
                {
                    newFileName += Path.GetExtension(documents.TrascationFiles.Path + path);
                    save_path = Server.MapPath("~/Temp/" + newFileName);
                    File.Copy(documents.TrascationFiles.Path + path, save_path, true);
                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views registration document [Doc Name:", doc_name, "]."));

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('../Business/banca_document_view.aspx?doc_name=" + ResolveClientUrl(newFileName) + "&view_type=multy');</script>", false);

                }
                else
                {
                    Helper.Alert(true, "File is not exist or removed.", lblError);
                }
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void gv_view_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_view.PageIndex = e.NewPageIndex;
      
      gv_view.DataSource = (List < documents.TrascationFiles >) Session["SSL_DATA"];
       gv_view.DataBind();
    }
}