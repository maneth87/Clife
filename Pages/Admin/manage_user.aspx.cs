using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Pages_admin_manage_user : System.Web.UI.Page
{

    bl_user user = new bl_user();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            List<bl_role> role_list = da_user.GetRole(Membership.ApplicationName);
            //bind roles in drop down list
            ddlRole.Items.Clear();
            ddlRole.Items.Add(new ListItem(".", "0"));
            ddlRole.DataSource = role_list;
            ddlRole.DataTextField = "roleName";
            ddlRole.DataValueField = "roleId";
            ddlRole.DataBind();

            ddlUserRole.Items.Clear();
            ddlUserRole.Items.Add(new ListItem(".", "0"));
            ddlUserRole.DataSource = role_list;
            ddlUserRole.DataTextField = "roleName";
            ddlUserRole.DataValueField = "roleId";
            ddlUserRole.DataBind();

            editddlUserRole.Items.Clear();
            editddlUserRole.Items.Add(new ListItem(".", "0"));
            editddlUserRole.DataSource = role_list;
            editddlUserRole.DataTextField = "roleName";
            editddlUserRole.DataValueField = "roleId";
            editddlUserRole.DataBind();



            //bind roles in drop down list in create user winzard panel
            //DropDownList user_role = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlUserRole");
            //user_role.Items.Clear();
            //user_role.Items.Add(new ListItem(".", "0"));
            //user_role.DataSource = role_list;
            //user_role.DataTextField = "roleName";
            //user_role.DataValueField = "roleId";
            //user_role.DataBind();

            //get username
            MembershipUser myUser = Membership.GetUser();

            string username = myUser.UserName;


            //Store username in hidenfield when load paeg

            hdfUserName.Value = username;

            //FormsAuthentication.RedirectFromLoginPage(username, chkRememberMe.Checked);
            if (!Roles.IsUserInRole(username, "Administrator") && !Roles.IsUserInRole(username, "ICT"))
            {
                Response.Redirect("~/default.aspx");
            }

            string user_serch = txtUsername.Text.Trim();
            string user_type = ddlRole.SelectedValue.ToString();
            int status = Convert.ToInt32(ddlStatus.SelectedValue);

            // Page load for all user
            Load_Data(user_serch, user_type, status);
        }
    }

    // Load_Data

    private void Load_Data(string username, string user_type, int status)
    {

        List<bl_user> user_list = new List<bl_user>();

        user_list = da_user.GetAllUserList(username, user_type,status, Membership.ApplicationName);

        if (user_list.Count == 0)
        {

            resultSearch.Text = "&nbsp;&nbsp; Not Found !";
            resultSearch.ForeColor = System.Drawing.Color.Red;
        }
        else {
            resultSearch.Text = "Total User : ( " + user_list.Count.ToString() + " )";
            resultSearch.ForeColor = System.Drawing.Color.Black;
          }

       // CreateUserWizard.ActiveStepIndex = 0;

        // After CreateUserWizard completed  

        //TextBox name = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
        //TextBox pwd = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Password");
        //TextBox re_pwd = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmPassword");
        //TextBox email = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");

        //// After Created Successfull
        //name.Text =".";
        //pwd.Text = " ";
        //re_pwd.Text = " ";
        //email.Text = " ";

        GvUser.DataSource = user_list;
        GvUser.DataBind();       

    }

    // Function search user

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        string user_serch= txtUsername.Text.Trim();
        string user_type = ddlRole.SelectedValue.ToString();
        int    status = Convert.ToInt32(ddlStatus.SelectedValue);

        Load_Data(user_serch,user_type,status);

    }

   protected void GvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        GvUser.PageIndex = e.NewPageIndex;

    }

    protected void GvUser_PageIndexChanged(object sender, EventArgs e)
    {

        string user_serch = txtUsername.Text.Trim();
        string user_type = ddlRole.SelectedValue.ToString();
        int status = Convert.ToInt32(ddlStatus.SelectedValue);

        Load_Data(user_serch, user_type,status);

    }

    protected void CreateUserWizard_CreatedUser(object sender, EventArgs e)
    {
        //try
        //{
        //    //Add new User account
        //    DropDownList user_role = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlUserRole");

        //    string role = user_role.SelectedItem.Text;

        //    string userName = CreateUserWizard.UserName;
        //    string pass = CreateUserWizard.Password;


        //    //Check password policy
        //    bl_password pwd = new bl_password();
        //    string Message = "";
        //    if (pwd.PasswordIsValid(pass, out Message))
        //    {
        //        Roles.AddUserToRole(userName, role);
        //        // Load Gridview

        //        string user_serch = txtUsername.Text.Trim();
        //        string user_type = ddlRole.SelectedValue.ToString();
        //        int status = Convert.ToInt32(ddlStatus.SelectedValue);

        //        // Page load for all user
        //        Load_Data(user_serch, user_type, status);

        //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('User Account Created Successfully !.')", true);
        //    }
        //    else
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + Message + "')", true);
        //    }

        //}
        //catch (Exception ex)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + ex.Message + "')", true);
        //}

    }

    //Function Edit User account
    protected void btnEditUser_Click(object sender, EventArgs e)
    {

        string user_id = hdfUserId.Value;

         user.UserId = user_id;
        
         user.Email = editEmail.Text.Trim();
         user.IsApproved = editddlStatus.SelectedValue.ToString();
         user.IsLockedOut  = editddlLocked.SelectedValue.ToString();

         bool update_user_membership = da_user.UpdateUserAccountMembership(user);

        if(update_user_membership){

          string role_id = editddlUserRole.SelectedValue.ToString();

          bool update_role = da_user.UpdateUserAccountUserInRole(role_id, user_id);

          if (update_role)
          {

              Response.Redirect("~/Pages/Admin/manage_user.aspx");
             
         }

        }

    }
    protected void btnresetpwd_Click(object sender, EventArgs e)
    {

        string username = hdfResettxtUsername.Value;
        string newPassword = ResetPassword.Text.Trim();
        bl_password pwd = new bl_password();
        string Message = "";
        if (pwd.PasswordIsValid(newPassword, out Message))
        {
            MembershipUser usr = Membership.GetUser(username);
            string resetPwd = usr.ResetPassword();
            usr.ChangePassword(resetPwd, newPassword);

           // ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Password Has Been Changed!.')", true);
            lblResetMessage.Text = "Password Has Been Changed!.";
        }
        else
        {
           // ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The password you supplied is not meet requirement. Please check your input again.')", true);
           
            lblResetMessage.Text= Message;
           
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + lblResetMessage.Text + "')", true);

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
        else if (Email.Text.Trim() == "")
        {
            lblMessage.Text = "Please input email.";
        }
        else if (!Regex.IsMatch(Email.Text.Trim(), @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))//check email format
        {
            lblMessage.Text = "Please input valid email format. [Ex: username@camlife.com.kh]";
        }
        else if (string.Compare(txtPassword.Text, txtConfirmPassword.Text) != 0)
        {
            lblMessage.Text = "Password and Re-password are not matched.";
        }
        else
        {
            bl_user user = new bl_user();
            user = da_user.GetUserByUser(txtUserNameC.Text, Membership.ApplicationName);
            RoleProvider role = Roles.Providers["SqlRoleProvider"];
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

                    MembershipUser userCreated = Membership.CreateUser(txtUserNameC.Text, txtPassword.Text, Email.Text.Trim());

                    if (userCreated.UserName != null)
                    {
                        role.AddUsersToRoles(new string[] { txtUserNameC.Text }, new string[] { ddlUserRole.SelectedItem.Text });
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