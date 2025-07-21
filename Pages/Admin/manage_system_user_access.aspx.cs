using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_manage_system_user_access : System.Web.UI.Page
{
    bl_sys_objects obj = new bl_sys_objects();
    bl_sys_roles Role = new bl_sys_roles();
    bl_sys_roles_access RolesAcccess = new bl_sys_roles_access();
    bl_sys_user_access UserAccess = new bl_sys_user_access();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Initial();
            LoadUserAccess();
        }

    }
    void LoadUserAccess()
    {
        List<bl_sys_user_access> uList = new List<bl_sys_user_access>();
        uList = UserAccess.GetUserAccessList();

        gv_pages.DataSource = uList;
        gv_pages.DataBind();
        lblCount.Text = "[ " + uList.Count + " Found. ]";
    }
    void LoadUserAccess(string roleId, string module, string userName)
    {
        List<bl_sys_user_access> uList = new List<bl_sys_user_access>();
        uList = UserAccess.GetUserAccessList(roleId, module, userName);
        gv_pages.DataSource = uList;
        gv_pages.DataBind();
        lblCount.Text = "[ " + uList.Count + " Found. ]";
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

        List<bl_sys_objects> Lobj = obj.GetSystObjectsModule();
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
            ddlRoleName.Enabled = false;
            txtRoleId.Enabled = false;
            ddlModule.Enabled = false;
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
        string userName=txtUserName.Text.Trim();
        string roleId = txtRoleId.Text.Trim();
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
                            UserAccess.Delete(userName, LObjectId.Text);
                            if (UserAccess.Save(new bl_sys_user_access(UserAccess.GetNewId(), userName, roleId, LObjectId.Text, CbView.Checked ? 1 : 0, CbAdd.Checked ? 1 : 0, CbUpdate.Checked ? 1 : 0, CbApprove.Checked ? 1 : 0, CbAdmin.Checked ? 1 : 0)))
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
                        Globle.showMessage(div_message, "Added User Access successfully.", 0);
                       
                    }
                    else if (CountAddFail > 0 && CountAddSuccess == 0)
                    {
                        Globle.showMessage(div_message, "Added User Access  fail.", 1);
                    }
                    else if (CountAddFail > 0 && CountAddSuccess > 0)
                    {
                        Globle.showMessage(div_message, "Some User Access were added successfully.", 2);
                    }

                    //load recent added
                    LoadUserAccess(txtRoleId.Text.Trim(), ddlModule.SelectedValue, userName);
                 

                }
                else
                {
                    Globle.showMessage(div_message, "Please select objects.", 2);
                }
            }
        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
            if (ddlRoleName.SelectedValue != "")
            {
                LoadUserAccess(ddlRoleName.SelectedValue, ddlModule.SelectedValue, userName);
            }
            else
            {
                LoadUserAccess();
            }
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

               
                List<bl_sys_user_access> uList = UserAccess.GetUserAccessList(txtRoleId.Text.Trim(), ddlModule.SelectedValue, txtUserName.Text.Trim());

                foreach (GridViewRow Grow in gv_objects.Rows)
                {
                    Label LObjectId = (Label)Grow.FindControl("lblObjectId");
                    CheckBox CbView = (CheckBox)Grow.FindControl("ckbView");
                    CheckBox CbAdd = (CheckBox)Grow.FindControl("ckbAdd");
                    CheckBox CbUpdate = (CheckBox)Grow.FindControl("ckbUpdate");
                    CheckBox CbApprove = (CheckBox)Grow.FindControl("ckbApprove");
                    CheckBox CbAdmin = (CheckBox)Grow.FindControl("ckbAdmin");

                    foreach (bl_sys_user_access u in uList)
                    {
                        if (LObjectId.Text == u.ObjId)
                        {
                            CbView.Checked = u.IsView == 1 ? true : false;
                            CbAdd.Checked = u.IsAdd == 1 ? true : false;
                            CbUpdate.Checked = u.IsUpdate == 1 ? true : false;
                            CbApprove.Checked = u.IsApprove == 1 ? true : false;
                            CbAdmin.Checked = u.IsAdmin == 1 ? true : false;

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
        ddlModule_SelectedIndexChanged(null, null);
        
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
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
        ddlRoleName.Enabled = true;
        ddlModule.Enabled = true;
    }
    protected void gv_pages_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Label lId;
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gRow;
            GridView g = sender as GridView;
            gRow = g.Rows[index];

            lId = (Label)gRow.FindControl("lblId");
            if (UserAccess.Delete(lId.Text))
            {
                Globle.showMessage(div_message, "Deleted successfully.", 0);
                if (ddlRoleName.SelectedValue != "")
                {
                    LoadUserAccess(ddlRoleName.SelectedValue, ddlModule.SelectedValue, txtUserName.Text.Trim());
                }
                else
                {
                    LoadUserAccess();
                }
            }
            else
            {
                Globle.showMessage(div_message, "Deleted fail.", 1);
            }
        }
        catch (Exception ex)
        {
            Globle.showMessage(div_message, ex.Message, 1);
        }
    }
}