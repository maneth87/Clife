using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Globalization;

public partial class _default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Request.IsAuthenticated)
        {
            Response.Redirect("login.aspx");
        }
        else

        {
            try
            {
                MembershipUser myUser = Membership.GetUser();
                string user_name = myUser.UserName;
                bl_sys_user_role ur = new bl_sys_user_role();
                if (myUser.UserName != null)
                {
                    List<bl_sys_user_role> Lobj = ur.GetSysUserRole(user_name);
                    List<bl_sys_user_role> Lrole = ur.GetSysRole(user_name);
                    if (Lobj != null && Lrole != null)
                    {
                        Session["SS_UR_LIST"] = Lobj;
                        Session["SS_UR_ROLE"] = Lrole;
                    }
                    else
                    {
                        Response.Redirect("unauthorize.aspx");
                    }
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Write("Unexpected error.");
            }
        }
    }
}