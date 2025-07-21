using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Pages_Admin_manage_system_user_role : System.Web.UI.Page
{
    bl_sys_roles Roles = new bl_sys_roles();
    bl_sys_user_role UserRole = new bl_sys_user_role();

     string UserName { get { return ViewState["VS_USER"] + ""; } set { ViewState["VS_USER"] = value; } }
     List<string> LRoleId { get { return (List<string>)ViewState["VS_L_ROLE"]; } set { ViewState["VS_L_ROLE"] = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Initial();
        }
    }
  
    protected void btnAdd_Click(object sender, EventArgs e)
    {
     

        Int32 CountSaveSuccess = 0;
      
        this.LRoleId = new List<string>();
        foreach (GridViewRow r in GvRole.Rows)
        {
            CheckBox cb = (CheckBox)r.FindControl("ckbAdd");
            Label LroleId = (Label)r.FindControl("lblRoleId");
            if (cb.Checked)
            {


                this.LRoleId.Add(LroleId.Text);
            }
        }
  
        if (this.UserName == "")
        {
            Globle.showMessage(div_message, "Please select user.", 2);
        }
        else if (this.LRoleId.Count == 0)
        {
            Globle.showMessage(div_message, "Please select roles.", 2);
        }
        else
        {
           //delete all existing role of the user
            UserRole.DeleteSysUserRole(this.UserName);
            foreach (string str in LRoleId)
            {
                if (UserRole.AddSysUserRole(UserName, str))
                {
                    CountSaveSuccess += 1;
                }
            }
            if (CountSaveSuccess == LRoleId.Count)
            {
               
                Globle.showMessage(div_message, "Added roles to [" + UserName + "] successfully.", 0);
                //LoadExistingRoles(this.UserName);
            }
            else if (CountSaveSuccess > 0 && CountSaveSuccess != LRoleId.Count)
            {
                Globle.showMessage(div_message, "Some roles were added  to [" + UserName + "]  are not successfully.", 2);
                LoadExistingRoles(this.UserName);
            }
            else
            {
                Globle.showMessage(div_message, "Added roles to [" + UserName + "] fail.", 1);
                //LoadExistingRoles(this.UserName);
            }

          
        }
    }
    protected void gv_pages_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    void Initial()
    {
        Globle.showMessage(div_message, "", 0);
        List<bl_sys_roles> LRole = Roles.GetSysRoles();
        GvRole.DataSource = LRole;
        GvRole.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
     List<bl_user> Luser=   da_user.GetAllUserList(txtUserName.Text,"0", 1, Membership.ApplicationName);
     GvUserName.DataSource = Luser;
     GvUserName.DataBind();
    
    }
    protected void ckbAdd_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ckBox = sender as CheckBox;
        bool MustCheck = ckBox.Checked;

        // uncheck all rows
        Unchecked(GvUserName, "ckbAdd");
       
        // now set the current check box
        ckBox.Checked = MustCheck;
        
        foreach (GridViewRow row in GvUserName.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (CheckBox)row.FindControl("ckbAdd");
                Label Luser = (Label)row.FindControl("lblUserName");

                ckBox = chkRow;
                
                if (chkRow.Checked)
                {
                  this.UserName = Luser.Text;
                    LoadExistingRoles(Luser.Text);

                    break;
                }
                else
                {
                    Initial();
                }
            }
        }

        
       
    }
    void LoadExistingRoles(string UserName)
    {
        Unchecked(GvRole, "ckbAdd");
     List<bl_sys_user_role> LUrole=   UserRole.GetSysRole(UserName);
        foreach (GridViewRow row in GvRole.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (CheckBox)row.FindControl("ckbAdd");
                Label LRoleId = (Label)row.FindControl("lblRoleId");
                foreach (bl_sys_user_role role in LUrole)
                {
                    if (LRoleId.Text == role.RoleId)
                    {
                        chkRow.Checked = true;
                        break;
                    }
                }
                
                
            }
        }
    }
    void Unchecked(GridView Gv, string CheckBoxId)
    {
        foreach (GridViewRow row in Gv.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (CheckBox)row.FindControl(CheckBoxId);
                chkRow.Checked = false;
            }
        }
    }
}