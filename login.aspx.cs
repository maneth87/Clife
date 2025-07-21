using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    private bool PasswordExpired { get { return (bool)ViewState["V_PWD_EX"]; } set { ViewState["V_PWD_EX"] = value; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            sPwdAlter.Visible = false;
            sChangePwd.Visible = false;

           
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string username, password;
            username = txtusername.Text.Trim();
            password = txtpassword.Text.Trim();
            bl_user user = new bl_user();
            user = da_user.GetUserByUser(username, Membership.ApplicationName);
            bl_password pwd = new bl_password();// AppConfiguration.PasswordPolicy();
            // check account status
            if (user.LockedOut) // account is locked
            {
                lblResult.Text = "Account is locked.";
            }
            else // account is not locked
            {
                PasswordExpired = false;
                    if (Membership.ValidateUser(username, password))
                    {
                        txtUserNameView.Text = username;
                        // check password age
                        int age = pwd.PasswordAge - user.PasswordAge;
                        if (age <= 0) //expired
                        {
                            lblPasswordAlert.Text = "Password was expired. Please change your password.";
                            slogin.Visible = false;
                            sPwdAlter.Visible = true;
                            PasswordExpired = true;
                            btnContinue.Visible = false;

                        }
                        else if (age > 0 && age <= 7)
                        {
                           
                            lblPasswordAlert.Text = "Password will expire in " + age + " day(s).";
                            slogin.Visible = false;
                            sPwdAlter.Visible = true;
                        }
                        else
                        {
                         da_sys_activity_log.Save(new bl_sys_activity_log( username,"Login.aspx",bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.LOGIN, DateTime.Now, "User login", Membership.ApplicationName));
                            FormsAuthentication.RedirectFromLoginPage(username, chkRememberMe.Checked);
                        }
                    }
                    else
                    {
                        lblResult.Text = "Login failed. Please try again";
                    }

                

            }
          
           
        }

        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error occured on page login. Details: " + ex.Message);
        }

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        string url = "";
        string user_name = "";

        user_name = txtusername.Text.Trim();

        if (user_name !="")
        {
            lblResult.Text = "";

            url = "ResetPassword.aspx?Username=" + user_name;

            Response.Redirect(url);
        }
        else
        {
            lblResult.Text = "Reset failed. Please try again!";
        }

    }
    protected void btnContinue_Click(object sender, EventArgs e)
    {
        if (PasswordExpired)
        {
            sPwdAlter.Visible = false;
            sChangePwd.Visible = true;
            txtUserNameView.Text = txtusername.Text;
        }
        else
        {
            FormsAuthentication.RedirectFromLoginPage(txtusername.Text, chkRememberMe.Checked);
        }
    }
    protected void btnChange_Click(object sender, EventArgs e)
    {
        bl_password pwd = new bl_password();
        string  Message="";
        MembershipUser myuser = Membership.GetUser(txtUserNameView.Text);
        if (btnChange.Text == "Change")
        {
            if (txtOldPassword.Text.Trim() == "" && txtNewPassword.Text.Trim() == "" && txtConfirmNewPassword.Text.Trim()=="")
            {
                lblChangePwdMessage.Text = "Please old password, new password & confirm new password";
            }
            else
            {
               
                if (string.Compare(txtNewPassword.Text.Trim(), txtConfirmNewPassword.Text.Trim()) == 0)
                {
                    if (Membership.ValidateUser(txtUserNameView.Text.Trim(), txtOldPassword.Text.Trim()))
                    {
                        pwd.PasswordIsValid(txtNewPassword.Text, out Message);

                        if (pwd.PasswordIsValid(txtNewPassword.Text, out Message))
                        {

                            if (myuser.ChangePassword(txtOldPassword.Text, txtNewPassword.Text))
                            {
                                da_sys_activity_log.Save(new bl_sys_activity_log(txtUserNameView.Text.Trim(), "Login.aspx", bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPDATE, DateTime.Now, "User was changed password.", Membership.ApplicationName));

                                lblChangePwdMessage.Text = "Your password has been changed.";
                                btnChange.Text = "Login";
                            }
                            else
                            {
                                lblChangePwdMessage.Text = "Changed password fail.";
                            }
                        }
                        else
                        {
                            lblChangePwdMessage.Text = Message;
                        }
                    }
                    else
                    {
                        lblChangePwdMessage.Text = "The old password you supplied is incorrect. Please check your input again.";
                    }
                   
                } 
                else
                {
                    lblChangePwdMessage.Text = "New password and confirm password mismatched. Please check your input again."; 
                }
                

            }
        }
        else if (btnChange.Text == "Login")
        {
            sChangePwd.Visible = false;
            slogin.Visible = true;
        }
      
       
    }
    protected void btnChange1_Click(object sender, EventArgs e)
    {
        sChangePwd.Visible = true;
        sPwdAlter.Visible = false;
       
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        sChangePwd.Visible = false;
        sPwdAlter.Visible = true;
       
    }
}