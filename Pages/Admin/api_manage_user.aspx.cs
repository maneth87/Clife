using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_api_manage_user : System.Web.UI.Page
{
    bl_user user = new bl_user();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bl_portal_sys_role roleObj = new bl_portal_sys_role();

            List<bl_portal_sys_role> roleList = roleObj.GetSysRoles(MYSQL_MEMBERSHIP.ApplicationName);
            //bind roles in drop down list
            ddlRole.Items.Clear();
            ddlRole.Items.Add(new ListItem(".", "0"));
            ddlRole.DataSource = roleList;
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataBind();

            ddlUserRole.Items.Clear();
            ddlUserRole.Items.Add(new ListItem(".", "0"));
            ddlUserRole.DataSource = roleList;
            ddlUserRole.DataTextField = "RoleName";
            ddlUserRole.DataValueField = "RoleId";
            ddlUserRole.DataBind();

            editddlUserRole.Items.Clear();
            editddlUserRole.Items.Add(new ListItem(".", "0"));
            editddlUserRole.DataSource = roleList;
            editddlUserRole.DataTextField = "RoleName";
            editddlUserRole.DataValueField = "RoleId";
            editddlUserRole.DataBind();


            //get username
            MembershipUser myUser = Membership.GetUser();

            string username = myUser.UserName;


            //Store username in hidenfield when load paeg

            hdfUserName.Value = username;



            string user_serch = txtUsername.Text.Trim();
            int user_type = Convert.ToInt32(ddlRole.SelectedValue.ToString());
            int status = Convert.ToInt32(ddlStatus.SelectedValue);

            // Page load for all user
            Load_Data(user_serch, user_type, status);
        }
    }

    // Load_Data

    private void Load_Data(string username, int roleId, int status)
    {

        List<bl_user> user_list = new List<bl_user>();

        user_list = MYSQL_MEMBERSHIP.GetUser(username, roleId, status);

        if (user_list.Count == 0)
        {

            resultSearch.Text = "&nbsp;&nbsp; Not Found !";
            resultSearch.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            resultSearch.Text = "Total User : ( " + user_list.Count.ToString() + " )";
            resultSearch.ForeColor = System.Drawing.Color.Black;
        }


        GvUser.DataSource = user_list;
        GvUser.DataBind();

    }

    // Function search user

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        string user_serch = txtUsername.Text.Trim();
        int user_type = Convert.ToInt32(ddlRole.SelectedValue.ToString());
        int status = Convert.ToInt32(ddlStatus.SelectedValue);

        Load_Data(user_serch, user_type, status);

    }

    protected void GvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        GvUser.PageIndex = e.NewPageIndex;

    }

    protected void GvUser_PageIndexChanged(object sender, EventArgs e)
    {

        string user_serch = txtUsername.Text.Trim();
        int user_type = Convert.ToInt32(ddlRole.SelectedValue.ToString());
        int status = Convert.ToInt32(ddlStatus.SelectedValue);

        Load_Data(user_serch, user_type, status);

    }


    //Function Edit User account
    protected void btnEditUser_Click(object sender, EventArgs e)
    {

        string user_id = hdfUserId.Value;
        int oldRoleId = Convert.ToInt32(hdfOldRoleId.Value);

        bool update = MYSQL_MEMBERSHIP.UpdateUserRole(Convert.ToInt32(user_id), oldRoleId, editEmail.Text.Trim(), Convert.ToInt32(editddlStatus.SelectedValue), Convert.ToInt32(editddlLocked.SelectedValue), Convert.ToInt32(editddlUserRole.SelectedValue));


        if (update)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('User has been updated successfully.')", true);
            btnSearch_Click(null, null);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update user fail.')", true);
        }

    }
    protected void btnresetpwd_Click(object sender, EventArgs e)
    {

        //string username = hdfResettxtUsername.Value;
        //string newPassword = ResetPassword.Text.Trim();
        //bl_password pwd = new bl_password();
        //string Message = "";
        //if (pwd.PasswordIsValid(newPassword, out Message))
        //{

        //    if (MYSQL_MEMBERSHIP.ResetPassword(username, newPassword))
        //    {
        //        lblResetMessage.Text = "Password Has Been Changed!.";
        //    }
        //    else
        //    {
        //        lblResetMessage.Text = "Reset password fail.";
        //    }
        //}
        //else
        //{
        //    lblResetMessage.Text = Message;

        //}
        //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + lblResetMessage.Text + "')", true);

       string a= API.CAMLIFE.GetToken( "","").access_token;

    }
    protected void bntCreate_Click(object sender, EventArgs e)
    {
        if (txtUserNameC.Text.Trim() == "")
        {
            lblMessage.Text = "Please input username.";
        }
        else if (ddlUserRole.SelectedIndex == 0)
        {
            lblMessage.Text = "Please select role.";
        }
        else if (txtPassword.Text.Trim() == "")
        {
            lblMessage.Text = "Please input password.";
        }
        else if (txtConfirmPassword.Text.Trim() == "")
        {
            lblMessage.Text = "Please input re-password.";
        }


        else if (string.Compare(txtPassword.Text, txtConfirmPassword.Text) != 0)
        {
            lblMessage.Text = "Password and Re-password are not matched.";
        }
        //else if (!Regex.IsMatch(Email.Text.Trim(), @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))//check email format
        //{
        //    lblMessage.Text = "Please input valid email format. [Ex: username@camlife.com.kh]";
        //}

        else
        {
            if (Email.Text.Trim() != "")
            {

                if (!Regex.IsMatch(Email.Text.Trim(), @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))//check email format
                {
                    lblMessage.Text = "Please input valid email format. [Ex: username@camlife.com.kh]";
                    return;
                }
            }


            bl_user user = new bl_user();
            var uList = MYSQL_MEMBERSHIP.GetUser(txtUserNameC.Text, -1, -1);
            if (uList.Count > 0)
            {
                user = uList[0];
            }


            string Message = "";
            if (user.UserName != null)
            {
                lblMessage.Text = "User is already exist.";
            }
            else
            {
                bl_password pwd = new bl_password();
                if (pwd.PasswordIsValid(txtPassword.Text, out Message))
                {
                    bool cUser = MYSQL_MEMBERSHIP.CreateUser(txtUserNameC.Text.Trim(), txtPassword.Text, Email.Text, ddlUserRole.SelectedItem.Text);
                    if (cUser)
                    {
                        lblMessage.Text = "New user account is created.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        //create user fail
                        lblMessage.Text = "Create user account fail.";
                    }
                }
                else
                {
                    //password is not requirement
                    lblMessage.Text = Message;
                }


            }
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + lblMessage.Text + "')", true);
    }
}