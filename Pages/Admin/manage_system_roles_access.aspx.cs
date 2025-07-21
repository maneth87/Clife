using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_manage_system_roles_access : System.Web.UI.Page
{
    bl_sys_objects obj = new bl_sys_objects();
    bl_sys_roles Role = new bl_sys_roles();
    bl_sys_roles_access RolesAcccess = new bl_sys_roles_access();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Initial();
            LoadRolesAccess();
        }

    }
    void LoadRolesAccess()
    {
        
        List<bl_sys_roles_access> Lobj = new List<bl_sys_roles_access>();
        Lobj = RolesAcccess.GetSysRolesAccess();
        gv_pages.DataSource = Lobj;
        gv_pages.DataBind();
        lblCount.Text = "[ " + Lobj.Count + " Found. ]";
    }
    void LoadRolesAccess(string RoleId, string RoleName, string Module)
    {
        List<bl_sys_roles_access> Lobj = new List<bl_sys_roles_access>();
        Lobj = RolesAcccess.GetSysRolesAccess(RoleId, RoleName,Module);
        gv_pages.DataSource = Lobj;
        gv_pages.DataBind();
        lblCount.Text = "[ " + Lobj.Count + " Found. ]";
    }

    void Initial()
    {
        Globle.showMessage(div_message, "", 0);

        //bind role
        List<bl_sys_roles> Lrole = Role.GetSysRoles();
        ddlRoleName.Items.Add(new ListItem("--SELECT--", ""));
        foreach (bl_sys_roles role in Lrole)
        {
            ddlRoleName.Items.Add(new ListItem(role.RoleName, role.RoleId));
        }

        //bind module
             
     List<bl_sys_objects> Lobj=   obj.GetSystObjectsModule();
     ddlModule.Items.Add(new ListItem("--SELECT--", ""));
     foreach (bl_sys_objects var in Lobj)
     {
         ddlModule.Items.Add(new ListItem(var.Module, var.Module));
     }
     ddlModule.SelectedIndex = 0;

        
    }
    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
    {
     
        if (ddlAction.SelectedValue == "ADD")
        {
            txtRoleId.Enabled = false;
            ddlModule.SelectedIndex = 0;
            txtRoleId.Text = "";
            ddlRoleName.SelectedIndex = 0;
        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
            txtRoleId.Enabled = true;
            gv_objects.DataSource = null;
            gv_objects.DataBind();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
      
        if (ddlAction.SelectedValue == "ADD")
        {
            txtRoleId.Enabled = false;

           
            string RoleId = "";
            RoleId = txtRoleId.Text;
            Int32 CountSelectedObject = 0;
            Int32 CountAddSuccess = 0;
            Int32 CountAddFail = 0;
            if (ddlRoleName.SelectedIndex == 0)
            {
                Globle.showMessage(div_message, "Please select role name.", 2);
            }
            else if (ddlModule.SelectedIndex == 0)
            {
                Globle.showMessage(div_message, "Please select module.", 2);
            }
            else
            {
                foreach (GridViewRow Grow in gv_objects.Rows)
                {
                    Label LObjectId = (Label)Grow.FindControl("lblObjectId");
                    CheckBox CbView = (CheckBox)Grow.FindControl("ckbView");
                    CheckBox CbAdd = (CheckBox)Grow.FindControl("ckbAdd");
                    CheckBox CbUpdate = (CheckBox)Grow.FindControl("ckbUpdate");
                    CheckBox CbApprove = (CheckBox)Grow.FindControl("ckbApprove");
                    CheckBox CbAdmin = (CheckBox)Grow.FindControl("ckbAdmin");

                    if (CbView.Checked || CbAdd.Checked || CbUpdate.Checked || CbApprove.Checked || CbAdmin.Checked)
                    {
                        CountSelectedObject += 1;
                    }

                }
                if (CountSelectedObject > 0)
                {

                    //Delete all existing role access
                    RolesAcccess.DeleteSysRolesAccess(RoleId, ddlModule.SelectedValue);
                    //ADD ROLE ACCESS
                    foreach (GridViewRow Grow in gv_objects.Rows)
                    {
                        Label LObjectId = (Label)Grow.FindControl("lblObjectId");
                        CheckBox CbView = (CheckBox)Grow.FindControl("ckbView");
                        CheckBox CbAdd = (CheckBox)Grow.FindControl("ckbAdd");
                        CheckBox CbUpdate = (CheckBox)Grow.FindControl("ckbUpdate");
                        CheckBox CbApprove = (CheckBox)Grow.FindControl("ckbApprove");
                        CheckBox CbAdmin = (CheckBox)Grow.FindControl("ckbAdmin");

                        if (CbView.Checked || CbAdd.Checked || CbUpdate.Checked || CbApprove.Checked || CbAdmin.Checked)
                        {
                            if (RolesAcccess.AddSysRolesAccess(RoleId, LObjectId.Text, CbView.Checked ? true : false, CbAdd.Checked ? true : false, CbUpdate.Checked ? true : false, CbApprove.Checked ? true : false, CbAdmin.Checked ? true : false))
                            {
                                CountAddSuccess += 1;
                            }
                            else
                            {
                                CountAddFail += 1;
                            }
                        }

                    }
                    if (CountAddFail == 0 && CountAddSuccess > 0)
                    {
                        Globle.showMessage(div_message, "Added objects in role ["+ txtRoleId.Text +"] successfully.", 0);
                    }
                    else if (CountAddFail > 0 && CountAddSuccess == 0)
                    {
                        Globle.showMessage(div_message, "Added objects in role [" + txtRoleId.Text + "] fail.", 1);
                    }
                    else if (CountAddFail > 0 && CountAddSuccess > 0)
                    {
                        Globle.showMessage(div_message, "Some objects are not added in role [" + txtRoleId.Text + "] successfully.", 2);
                    }

                    //load recent added
                    LoadRolesAccess(txtRoleId.Text, ddlRoleName.SelectedIndex == 0 ? "" : ddlRoleName.SelectedItem.Text, ddlModule.SelectedValue);
                    //ddlAction.SelectedIndex = 0;
                    //gv_objects.DataSource = null;
                    //gv_objects.DataBind();

                }
                else
                {
                    Globle.showMessage(div_message, "Please select objects.", 2);
                }
            }
        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
            txtRoleId.Enabled = true;
            LoadRolesAccess(txtRoleId.Text, ddlRoleName.SelectedIndex==0 ? "" : ddlRoleName.SelectedItem.Text, ddlModule.SelectedValue);
        }
    }
    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAction.SelectedValue == "ADD")
        {
            if (ddlModule.SelectedIndex > 0)
            {
                List<bl_sys_objects> Lobj = obj.GetSystObjects(ddlModule.SelectedValue);
                gv_objects.DataSource = Lobj;
                gv_objects.DataBind();

             List<bl_sys_roles_access> Lrole  = RolesAcccess.GetSysRolesAccess(txtRoleId.Text, "", ddlModule.SelectedValue);

                foreach (GridViewRow Grow in gv_objects.Rows)
                {
                    Label LObjectId = (Label)Grow.FindControl("lblObjectId");
                    CheckBox CbView = (CheckBox)Grow.FindControl("ckbView");
                    CheckBox CbAdd = (CheckBox)Grow.FindControl("ckbAdd");
                    CheckBox CbUpdate = (CheckBox)Grow.FindControl("ckbUpdate");
                    CheckBox CbApprove = (CheckBox)Grow.FindControl("ckbApprove");
                    CheckBox CbAdmin = (CheckBox)Grow.FindControl("ckbAdmin");

                    foreach (bl_sys_roles_access Role in Lrole)
                    {
                        if (LObjectId.Text == Role.ObjectId)
                        {
                            CbView.Checked = Role.IsView == 1 ? true : false;
                            CbAdd.Checked = Role.IsAdd == 1 ? true : false;
                            CbUpdate.Checked = Role.IsUpdate == 1 ? true : false;
                            CbApprove.Checked = Role.IsApprove == 1 ? true : false;
                            CbAdmin.Checked = Role.IsAdmin == 1 ? true : false; 

                            break;
                        }
                    }

                }
            }
            else
            {
                gv_objects.DataSource = null;
                gv_objects.DataBind();
            }
        }
        else
        {
            gv_objects.DataSource = null;
            gv_objects.DataBind();
        }
    }
    protected void ddlRoleId_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRoleId.Text = ddlRoleName.SelectedValue;
    }
    protected void gv_pages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            Label Lview = (Label)e.Row.FindControl("lblIsView");
            Label Ladd = (Label)e.Row.FindControl("lblIsAdd");
            Label Lupdate = (Label)e.Row.FindControl("lblIsUpdate");
            Label Lapprove = (Label)e.Row.FindControl("lblIsApprove");
            Label Ladmin = (Label)e.Row.FindControl("lblIsAdmin");

            Lview.BackColor = Lview.Text == "YES" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            Ladd.BackColor = Ladd.Text == "YES" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            Lupdate.BackColor = Lupdate.Text == "YES" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            Lapprove.BackColor = Lapprove.Text == "YES" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            Ladmin.BackColor = Ladmin.Text == "YES" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            Lview.ForeColor = System.Drawing.Color.White;
            Ladd.ForeColor = System.Drawing.Color.White;
            Lupdate.ForeColor = System.Drawing.Color.White;
            Lapprove.ForeColor = System.Drawing.Color.White;
            Ladmin.ForeColor = System.Drawing.Color.White;

        }
    }
}