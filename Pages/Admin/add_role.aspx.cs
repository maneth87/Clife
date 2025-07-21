using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
public partial class Pages_Admin_add_role : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            loadRole();
           
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

       
        if (txtRoleName.Text.Trim() == "")
        {
            alert("Role Name Is Required.");
            return;
        }
        bl_role role = new bl_role();
        role.ApplicationID = da_user.GetApplicationID(Membership.ApplicationName);
        role.RoleName = txtRoleName.Text.Trim();
        role.Description = txtDescription.Text.Trim();

        if (ViewState["V_OLD_ROLE_ID"]+"" == "")
        {
          
            if (!Roles.RoleExists(txtRoleName.Text.Trim()))
            {
               

                if (da_user.AddRole(role))
                {
                    alert("Role was added successfully.");
                    clear();
                    loadRole();
                }
                else
                {
                    alert("Role was added fail.");
                }
            }
            else
            {
                alert("Role is already exist.");
            }
        }
        else
        { 
            //update role
            role.RoleID = ViewState["V_OLD_ROLE_ID"] + "";
            if (da_user.UpdateRole(role))
            {
                alert("Role was updated successfully.");
                clear();
                loadRole();
            }
            else
            {
                alert("Role was updated fail.");
            }
        }
      
    }

    void loadRole()
    {
        List<bl_role> role_list = new List<bl_role>();

        role_list = da_user.GetRole(Membership.ApplicationName);
        gvroles.DataSource = role_list;
        gvroles.DataBind();
    }
   
    protected void gvroles_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        int index = e.NewSelectedIndex;
        GridViewRow gRow = gvroles.Rows[index];
      HiddenField hdfOldRoleId= (HiddenField)  gRow.FindControl("hdfOldRoleID");
      txtRoleName.Text = ((Label)gRow.FindControl("lblRoleName")).Text;
     
     txtDescription.Text = ((Label)gRow.FindControl("lblDescription")).Text;
    
     ViewState["V_OLD_ROLE_ID"] = hdfOldRoleId.Value;
     btnAdd.Text = "Update";
     btnAdd.Attributes.Add("onclick", "confirm('Do you want to update role?');");

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

        clear();
    }

    void clear()
    {
        ViewState.Clear();
        txtDescription.Text = "";
       
        txtRoleName.Text = "";
        btnAdd.Text = "Add";
        btnAdd.Attributes.Remove("onclick");
    }

    void alert(string message)
    {
         
         Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');",true);
    }
}