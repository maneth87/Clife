using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_manage_system_roles_access : System.Web.UI.Page
{
    bl_portal_sys_object obj = new bl_portal_sys_object();
    bl_portal_sys_role Role = new bl_portal_sys_role();
    bl_portal_role_access RolesAcccess = new bl_portal_role_access();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Initial();
           // LoadRolesAccess();
        }

    }
    void LoadRolesAccess()
    {

        List<bl_portal_role_access> Lobj = new List<bl_portal_role_access>();
        Lobj = RolesAcccess.GetSysRolesAccess();
        gv_pages.DataSource = Lobj;
        gv_pages.DataBind();
        lblCount.Text = "[ " + Lobj.Count + " Found. ]";
    }
    void LoadRolesAccess(int RoleId, string Module)
    {
        List<bl_portal_role_access> Lobj = new List<bl_portal_role_access>();
        Lobj = RolesAcccess.GetSysRolesAccess(RoleId,Module);
        gv_pages.DataSource = Lobj;
        gv_pages.DataBind();
        lblCount.Text = "[ " + Lobj.Count + " Found. ]";
    }

    void Initial()
    {
        Globle.showMessage(div_message, "", 0);

        //bind role
        List<bl_portal_sys_role> Lrole = Role.GetSysRoles(MYSQL_MEMBERSHIP.ApplicationName);
        ddlRoleName.Items.Add(new ListItem("--SELECT--", ""));
        foreach (bl_portal_sys_role role in Lrole)
        {
            ddlRoleName.Items.Add(new ListItem(role.RoleName, role.RoleId+""));
        }

        //bind module

        List<bl_portal_sys_object> Lobj = obj.GetSystObjectsModule();
     ddlModule.Items.Add(new ListItem("--SELECT--", ""));
     foreach (bl_portal_sys_object var in Lobj)
     {
         ddlModule.Items.Add(new ListItem(var.Module, var.Module));
     }
     ddlModule.SelectedIndex = 0;

        
    }
    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
    {
     
        if (ddlAction.SelectedValue == "ADD")
        {
          
            ddlModule.SelectedIndex = 0;
           
            ddlRoleName.SelectedIndex = 0;
        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
         
            gv_objects.DataSource = null;
            gv_objects.DataBind();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
      
        if (ddlAction.SelectedValue == "ADD")
        {
                      
            int RoleId = 0;
            RoleId = Convert.ToInt32(ddlRoleName.SelectedValue);
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
                    CheckBox CbDelete = (CheckBox)Grow.FindControl("ckbDelete");
                    CheckBox CbAdmin = (CheckBox)Grow.FindControl("ckbAdmin");

                    if (CbView.Checked || CbAdd.Checked || CbUpdate.Checked || CbDelete.Checked || CbAdmin.Checked)
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
                        CheckBox CbDelete = (CheckBox)Grow.FindControl("ckbDelete");
                        CheckBox CbAdmin = (CheckBox)Grow.FindControl("ckbAdmin");

                       

                        if (CbView.Checked || CbAdd.Checked || CbUpdate.Checked || CbDelete.Checked || CbAdmin.Checked)
                        {
                           bl_portal_role_access accObj = new bl_portal_role_access()
                            {
                                RoleId = RoleId,
                                ObjectId = Convert.ToInt32(LObjectId.Text),
                                IsView = CbView.Checked,
                                IsAdd = CbAdd.Checked ,
                                IsUpdate =  CbUpdate.Checked,
                                IsDelete = CbDelete.Checked ,
                                IsAdmin = CbAdmin.Checked
                            };
                            if (RolesAcccess.AddSysRolesAccess(accObj))
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
                        Globle.showMessage(div_message, "Added objects in role ["+ ddlRoleName.SelectedItem.Text +"] successfully.", 0);
                    }
                    else if (CountAddFail > 0 && CountAddSuccess == 0)
                    {
                        Globle.showMessage(div_message, "Added objects in role [" + ddlRoleName.SelectedItem.Text + "] fail.", 1);
                    }
                    else if (CountAddFail > 0 && CountAddSuccess > 0)
                    {
                        Globle.showMessage(div_message, "Some objects are not added in role [" + ddlRoleName.SelectedItem.Text + "] successfully.", 2);
                    }

                    //load recent added
                    LoadRolesAccess(RoleId,  ddlModule.SelectedValue);

                }
                else
                {
                    Globle.showMessage(div_message, "Please select objects.", 2);
                }
            }
        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
          
            LoadRolesAccess(Convert.ToInt32( ddlRoleName.SelectedValue), ddlModule.SelectedValue);
        }
    }
    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAction.SelectedValue == "ADD")
        {
            if (ddlModule.SelectedIndex > 0)
            {
                List<bl_portal_sys_object> Lobj = obj.GetSystObjects(ddlModule.SelectedValue);
                gv_objects.DataSource = Lobj;
                gv_objects.DataBind();

                List<bl_portal_role_access> Lrole = RolesAcccess.GetSysRolesAccess(Convert.ToInt32(ddlRoleName.SelectedValue), ddlModule.SelectedValue);

                foreach (GridViewRow Grow in gv_objects.Rows)
                {
                    Label LObjectId = (Label)Grow.FindControl("lblObjectId");
                    CheckBox CbView = (CheckBox)Grow.FindControl("ckbView");
                    CheckBox CbAdd = (CheckBox)Grow.FindControl("ckbAdd");
                    CheckBox CbUpdate = (CheckBox)Grow.FindControl("ckbUpdate");
                    CheckBox CbDelete = (CheckBox)Grow.FindControl("ckbDelete");
                    CheckBox CbAdmin = (CheckBox)Grow.FindControl("ckbAdmin");

                    foreach (bl_portal_role_access Role in Lrole)
                    {
                        if (Convert.ToInt32( LObjectId.Text) == Role.ObjectId)
                        {
                            CbView.Checked = Role.IsView ;
                            CbAdd.Checked = Role.IsAdd ;
                            CbUpdate.Checked = Role.IsUpdate ;
                            CbDelete.Checked = Role.IsDelete ;
                            CbAdmin.Checked = Role.IsAdmin ; 

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
    
    }
    protected void gv_pages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

            Label Lview = (Label)e.Row.FindControl("lblIsView");
            Label Ladd = (Label)e.Row.FindControl("lblIsAdd");
            Label Lupdate = (Label)e.Row.FindControl("lblIsUpdate");
            Label Lapprove = (Label)e.Row.FindControl("lblIsDelete");
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