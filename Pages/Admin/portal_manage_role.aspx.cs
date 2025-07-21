using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_manage_system_role : System.Web.UI.Page
{
    bl_portal_sys_role role = new bl_portal_sys_role( AppConfiguration.GetCamlifePortalConnectionString());

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Initial();

            LoadRole(MYSQL_MEMBERSHIP.ApplicationName);
        }
    }

    void LoadRole( string ApplicaitonName,string RoleName="")
    {

        List<bl_portal_sys_role> ListRole = new List<bl_portal_sys_role>();
        ListRole = role.GetSysRoles(ApplicaitonName, RoleName);
        gv_pages.DataSource = ListRole;
        gv_pages.DataBind();
        lblCount.Text = "[ " + ListRole.Count + " Found.]";
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddlAction.SelectedValue == "ADD")
        {
            //if (txtRoleId.Text.Trim() == "")
            //{
            //    Globle.showMessage(div_message, "Role Id is required.", 2);
            //}
            //else
            if (txtRoleName.Text.Trim() == "")
            {
                Globle.showMessage(div_message, "Role Name is required.", 2);
            }
            else
            {
            if (hdfRoleId.Value == "")
            {
                if (role.AddRole(new bl_portal_sys_role() { ApplicationId=MYSQL_MEMBERSHIP.ApplicationId,  RoleName = txtRoleName.Text.Trim(), IsActive = Convert.ToInt32(ddlActive.SelectedValue) }))
                {
                    txtRoleId.Text = "";
                    Globle.showMessage(div_message, "Added new role successfully.", 0);
                    LoadRole(MYSQL_MEMBERSHIP.ApplicationName, txtRoleName.Text);
                }
                else
                {
                    Globle.showMessage(div_message, "Added new role fail.", 1);
                }
            }
            else
            { 
                //update
                if (role.UpdateRole(Convert.ToInt32(hdfRoleId.Value),txtRoleName.Text.Trim()))
                {
                    Globle.showMessage(div_message, "Updated role successfully.", 0);
                    LoadRole(MYSQL_MEMBERSHIP.ApplicationName, txtRoleName.Text);

                    hdfRoleId.Value = "";
                    txtRoleId.Text = "";
                  
                    ddlActive.SelectedIndex = 0;
                    
                }
                else
                {
                    Globle.showMessage(div_message, "Updated role fail.", 1);
                }
            }

        }
        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
      
            LoadRole( MYSQL_MEMBERSHIP.ApplicationName, txtRoleName.Text);

        }
    }

    void Initial()
    {
        Globle.showMessage(div_message, "", 0);
     //if (ddlAction.SelectedValue == "ADD")
     //   {
     //       txtRoleId.Enabled = true;
     //       ddlActive.Enabled = true;
     //   }
     //   else if (ddlAction.SelectedValue == "SEARCH")
     //   {
     //       txtRoleId.Enabled = false;
     //       ddlActive.Enabled = false;
           
     //   }
             txtRoleId.Enabled = false;
            ddlActive.Enabled = false;
    }

    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        Initial();
    }
    protected void gv_pages_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridViewRow r = gv_pages.Rows[e.NewEditIndex];
            Label lRoleId = (Label)r.FindControl("lblRoleId");
            Label lRoleName = (Label)r.FindControl("lblRoleName");
            HiddenField lActive = (HiddenField)r.FindControl("hdfIsActive");
            txtRoleId.Text = lRoleId.Text;
            txtRoleName.Text = lRoleName.Text;
            ddlActive.SelectedIndex = (lActive.Value == "1" ? 0 : 1);
            hdfRoleId.Value = lRoleId.Text;

            ddlAction.SelectedIndex = 1;//select add action
            txtRoleId.Enabled = false;
            ddlActive.Enabled = true;
        }
        catch (Exception ex)
        {
            Globle.showMessage(div_message, ex.Message, 1);
        }

    }
}