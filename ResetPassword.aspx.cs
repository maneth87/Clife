using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ResetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = "";
        string username = "", surname = "", lastname = "";
        if (Request.QueryString["Username"] != "" && Request.QueryString["Username"] != null)
        {
            username = Request.QueryString["Username"].ToString().Trim();
        }

        if (!Page.IsPostBack)
        {
            if (username != "")
            {
                MembershipUser myuser = Membership.GetUser(username);
                if (myuser != null)
                {
                    string[] str_username = username.Split('.');
                    if (str_username.Length > 0 && str_username.Length == 1)
                    {
                        surname = (str_username[0].ToString().Trim()).ToUpperInvariant();
                    }
                    else
                    {
                        surname = (str_username[0].ToString().Trim()).ToUpperInvariant();
                        lastname = ((str_username[1].ToString().Trim())).ToUpperInvariant();
                    }
                    
                    txtEmail.Text = myuser.Email;
                    txtUserName.Text = username;
                    txtFullName.Text = surname + " " + lastname;

                    btnLogin.Enabled = false;
                }
                else
                {
                    //reset bland textbox
                    txtEmail.Text = "";
                    txtUserName.Text = "";
                    txtFullName.Text = "";

                    url = "login.aspx";
                    Response.Redirect(url);
                }
            }
            else
            {
                url = "login.aspx";
                Response.Redirect(url);
            }
        }
        
    }

    //Button Reset password click
    protected void btnReset_Click(object sender, EventArgs e)
    {
        //string message = "";
        //try
        //{
        //    string strNewPass = null;
        //    string user_name = null;

        //    strNewPass = "";

        //    if (txtUserName.Text != "")
        //    {
        //        user_name = txtUserName.Text;
        //        strNewPass = Membership.GeneratePassword(12, 1);

        //        MembershipProvider mvp = Membership.Providers["SqlProvider"];
        //        MembershipUser cLifeUser = mvp.GetUser(user_name, true);

        //        if (cLifeUser.IsLockedOut != false)
        //        {
        //            Helper.IsLockedOut(cLifeUser.UserName);
        //        }

        //        string strPassword = cLifeUser.ResetPassword();
        //        cLifeUser.ChangePassword(strPassword, strNewPass);

        //        EmailSender mail = new EmailSender();
        //        string emailtopic = null;
        //        string emailmessage = null;

        //        emailtopic = "Password Reset on " + System.DateTime.Now.ToLongDateString() + " at " + System.DateTime.Now.ToLongTimeString();

        //        emailmessage = "<span style=\"font-family:Calibri; font-size: 15px;\">Dear <span style=\"font-weight: bold;\">" + txtFullName.Text + "</span>" + ", <br /><br />";
        //        emailmessage += "Your password has been reset. Your new password is <span style=\"font-weight: bold;\">" + strNewPass + "</span><br /><br />";

        //        emailmessage += "Thank you, <br/>System Administrator</span>";

        //        mail.From = "camlifesys@camlife.com.kh";
        //        mail.To = cLifeUser.Email; 
        //        mail.Subject = emailtopic;
        //        mail.Message = emailmessage;
        //        mail.Host = "mail.camlife.com.kh";
        //        mail.Port = 587;
        //        mail.Password = "admin1!2cdef";

        //        if (mail.SendMail(mail))
        //        {
        //            btnLogin.Enabled = true;
        //            message = "New password was sent to your email.";
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "showSuccessMessage('" + message + "');", true);
        //        }
        //        else
        //        {
        //            Log.AddExceptionToLog("Error in function [btnReset_Click], class [ResetPassword.aspx.cs]. Details: Send email failed!");

        //            message = "Reset new password failed!!";
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "showFailMessage('" + message + "');", true);
        //        }
        //    }

        //}
        //catch (Exception ex)
        //{
        //    Log.AddExceptionToLog("Error in function [btnReset_Click], class [ResetPassword.aspx.cs]. Details: " + ex.Message);

        //    message = "Contact system administrator!!";
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "showFailMessage('" + message + "');", true);
        //}

        #region V2
        string message = "";
        try
        {
            string strNewPass = null;
            string user_name = null;

            strNewPass = "";

            if (txtUserName.Text != "")
            {
                user_name = txtUserName.Text;
                strNewPass = Membership.GeneratePassword(20, 1);

                MembershipProvider mvp = Membership.Providers["SqlProvider"];
                MembershipUser cLifeUser = mvp.GetUser(user_name, true);

                if (cLifeUser.IsLockedOut != false)
                {
                    Helper.IsLockedOut(cLifeUser.UserName);//unlock
                }

                string strPassword = cLifeUser.ResetPassword();
                cLifeUser.ChangePassword(strPassword, strNewPass);

                EmailSender mail = new EmailSender();
                string emailtopic = null;
                string emailmessage = null;

               

                emailtopic = "CLIFE SYSTEM - PASSWORD RESET";

                emailmessage = "<span style=\"font-family:Calibri; font-size: 15px;\">Dear <span style=\"font-weight: bold;\">" + txtFullName.Text + "</span>" + ", <br /><br />";
                emailmessage += "Your password has been reset. Your new password is <span style=\"font-weight: bold;\">" + strNewPass + "</span><br /><br />";

                emailmessage += "Thank you, <br/>System Administrator</span>";

                mail.From = AppConfiguration.GetEmailFrom();
                mail.To = cLifeUser.Email;
                mail.Subject = emailtopic;
                mail.Message = emailmessage;
                mail.Host = AppConfiguration.GetEmailHost();
                mail.Port = AppConfiguration.GetEmailPort();
                mail.Password = AppConfiguration.GetEmailPassword();

                if (mail.SendMail(mail))
                {
                    btnLogin.Enabled = true;
                    message = "New password was sent to your email.";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "showSuccessMessage('" + message + "');", true);
                }
                else
                {
                    Log.AddExceptionToLog("Error in function [btnReset_Click], class [ResetPassword.aspx.cs]. Details: Send email failed!");

                    message = "Reset new password failed!!";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "showFailMessage('" + message + "');", true);
                }
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [btnReset_Click], class [ResetPassword.aspx.cs]. Details: " + ex.Message);

            message = "Contact system administrator!!";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "showFailMessage('" + message + "');", true);
        }
        #endregion V2
    }


    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string url = "";

            //reset bland textbox
            txtEmail.Text = "";
            txtUserName.Text = "";
            txtFullName.Text = "";

            url = "login.aspx";
            Response.Redirect(url);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in function [btnLogin_Click], class [ResetPassword.aspx.cs]. Details: " + ex.Message);
        }
    
    }
}