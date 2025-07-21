using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Pages_Admin_manage_system_objects : System.Web.UI.Page
{
    bl_sys_objects obj = new bl_sys_objects();
    string UserName;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserName = Membership.GetUser().UserName;
        if (!Page.IsPostBack)
        {
            Initial();
            LoadObject();
        }
    }
    void LoadObject()
    {
       
        hdfObjectId.Value = "";
        ddlAction.SelectedIndex = 0;
        txtObjectCode.Text = "";
        bl_sys_objects obj = new bl_sys_objects();
        List<bl_sys_objects> Lobj = new List<bl_sys_objects>();
        Lobj = obj.GetSystObjects(txtModule.Text, txtObjectName.Text);

        gv_pages.DataSource = Lobj;
        gv_pages.DataBind();
        lblCount.Text = "[ " + Lobj.Count + " Found. ]";

        
    }

    void Initial()
    {
        Globle.showMessage(div_message, "", 0);
        if (ddlAction.SelectedValue == "ADD")
        {
            txtModule.Enabled = true;
            txtObjectCode.Enabled = true;
            txtObjectName.Enabled = true;
            ddlActive.Enabled = true;
            txtPath.Enabled = true;
        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
            txtObjectCode.Enabled = false;
            ddlActive.Enabled = false;
            txtPath.Enabled = false;

        }
    }

    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        Initial();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddlAction.SelectedValue == "ADD")
        {
            if (txtModule.Text.Trim() == "")
            {
                Globle.showMessage(div_message, "Module is required", 2);
            }
            else if (txtObjectCode.Text.Trim() == "")
            {
                Globle.showMessage(div_message, "Object Code is required", 2);
            }
            else if (txtObjectName.Text.Trim() == "")
            {
                Globle.showMessage(div_message, "Object Name is required", 2);
            }
            else if (txtPath.Text.Trim() == "")
            {
                Globle.showMessage(div_message, "Path is required", 2);
            }
            else
            {
                if (hdfObjectId.Value == "")
                {
                    //save new
                    if (obj.AddObject(new bl_sys_objects()
                    {
                        ObjId = Helper.GetNewGuid(new string[,] { { "TABLE", "TBL_SYS_OBJECTS" }, { "FIELD", "OBJ_ID" } }),
                        Module = txtModule.Text.Trim(),
                        ObjCode = txtObjectCode.Text.Trim(),
                        ObjName = txtObjectName.Text.Trim(),
                       Path=txtPath.Text.Trim(),
                        IsActive = Convert.ToInt32(ddlActive.SelectedValue),
                        Remarks = "",
                        TransactionBy = UserName,
                        TransactionDate = DateTime.Now

                    }))
                    {
                        Globle.showMessage(div_message, "Added new Object successfully.", 0);
                        txtObjectCode.Text = "";
                        LoadObject();

                    }
                    else
                    {
                        Globle.showMessage(div_message, "Added new Object fail.", 0);
                    }
                }
                else
                {
                    //update
                    if (obj.UpdateObject(new bl_sys_objects()
                     {
                         ObjId = hdfObjectId.Value,
                         Module = txtModule.Text.Trim(),
                         ObjCode = txtObjectCode.Text.Trim(),
                         ObjName = txtObjectName.Text.Trim(),
                         Path=txtPath.Text.Trim(),
                         IsActive = Convert.ToInt32(ddlActive.SelectedValue),
                         Remarks = "",
                         TransactionBy = UserName,
                         TransactionDate = DateTime.Now
                     }))
                    {
                        Globle.showMessage(div_message, "Updated Object successfully.", 0);
                        LoadObject();
                    }
                    else
                    {
                        Globle.showMessage(div_message, "Updated Object fail.", 1);
                    }
                }
            }

        }
        else if (ddlAction.SelectedValue == "SEARCH")
        {
            //txtObjectCode.Enabled = false;
            //ddlActive.Enabled = false;

            //List<bl_sys_objects> Lobj = obj.GetSystObjects(txtModule.Text, txtObjectName.Text);
            //gv_pages.DataSource = Lobj;
            //gv_pages.DataBind();

            LoadObject();
        }
    }

    protected void gv_pages_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridViewRow r = gv_pages.Rows[e.NewEditIndex];
            Label lObjId = (Label)r.FindControl("lblObjectId");
            Label lObjCode = (Label)r.FindControl("lblObjectCode");
            Label lObjName = (Label)r.FindControl("lblObjectName");
            Label lModule = (Label)r.FindControl("lblModule");
            HiddenField lActive = (HiddenField)r.FindControl("hdfIsActive");
            Label lPath = (Label)r.FindControl("lblPath");
            txtObjectCode.Text = lObjCode.Text;
            txtObjectName.Text = lObjName.Text;
            txtModule.Text = lModule.Text;
            txtPath.Text=lPath.Text;
            ddlActive.SelectedIndex = (lActive.Value == "1" ? 0 : 1);
            hdfObjectId.Value = lObjId.Text;
            ddlAction.SelectedIndex = 1;//select add action
            txtObjectCode.Enabled = true;
            ddlActive.Enabled = true;
            txtPath.Enabled = true;
        }
        catch (Exception ex)
        {
            Globle.showMessage(div_message, ex.Message, 1);
        }

    }
}