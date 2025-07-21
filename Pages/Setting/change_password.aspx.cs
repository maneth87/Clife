
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

public partial class Pages_Setting_change_password : System.Web.UI.Page
{

    //Button Change password click
    protected void ImgBtnChange_Click(object sender, EventArgs e)
    {
        #region v1
        //try
        //{
        //    MembershipUser myUser = Membership.GetUser();
        //    string user_id = myUser.ProviderUserKey.ToString();
           
        //    string strOldPass = null;
        //    string strNewPass = null;
        //    string strConfirmPass = null;
        //    string user_name = User.Identity.Name;

        //    strOldPass = txtOldPassword.Text;

        //    //check if old pass provided is correct
        //    if (Membership.ValidateUser(user_name, strOldPass))
        //    {
        //        strNewPass = txtNewPassword.Text;
        //        strConfirmPass = txtConfirmNewPassword.Text;

        //        int result = 0;
        //        result = string.Compare(strNewPass, strConfirmPass);


        //        if (result == 0)
        //        {
        //            if (strNewPass.Length < 8)
        //            {
        //                lblmessage.Text = "New password requires at least 8 characters.";
        //                lblmessage.ForeColor = System.Drawing.Color.Red;
        //                return;
        //            }

        //            MembershipUser myuser = Membership.GetUser(user_name);
        //            myuser.ChangePassword(strOldPass, strNewPass);
                   
        //            lblmessage.Text = "Your password has been changed.";
        //            lblmessage.ForeColor = System.Drawing.Color.Green;


        //            //Start send mail
        //            DateTime date_created = default(DateTime);
        //            date_created = System.DateTime.Now;
        //            MembershipUser u = Membership.GetUser(user_name, false);
        //            string senderEmail = "webmaster@camlife.com.kh";

        //            // get the email address from the web.conf
        //            Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~\\Web.config");
        //            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
        //            if (mailSettings != null)
        //            {
        //                int port = mailSettings.Smtp.Network.Port;
        //                string host = mailSettings.Smtp.Network.Host;
        //                string password = mailSettings.Smtp.Network.Password;
        //                string username = mailSettings.Smtp.Network.UserName;
        //                senderEmail = mailSettings.Smtp.From;
        //            }

        //            string emailtopic = null;
        //            string emailmessage = null;

        //            emailtopic = "Password Reset on " + System.DateTime.Now.ToLongDateString() + " at " + System.DateTime.Now.ToLongTimeString();

        //            emailmessage = "<span style=\"font-family:Calibri; font-size: 15px;\">Dear " + user_name + ", <br /><br />";
        //            emailmessage += "Your password has been changed. Your new password is <span style=\"font-weight: bold;\">" + strNewPass + "</span><br /><br />";

        //            emailmessage += "Thank you, <br/>Banc System</span>";

        //            //Send mail to sales and service

        //            EmailSender.SendMailMessage(senderEmail, u.Email, "maneth.som@camlife.com.kh", emailtopic, emailmessage);

        //            //Send mail to the right person

        //            txtOldPassword.Text = "";
        //            txtNewPassword.Text = "";
        //            txtConfirmNewPassword.Text = "";
        //        }
        //        else
        //        {
        //            lblmessage.Text = "New password and confirm password mismatched. Please check your input again.";
        //            lblmessage.ForeColor = System.Drawing.Color.Red;
        //        }

        //        //old password provided mismatched password in db
        //    }
        //    else
        //    {
        //        lblmessage.Text = "The old password you supplied is incorrect. Please check your input again.";
        //        lblmessage.ForeColor = System.Drawing.Color.Red;
        //    }

        //}
        //catch (Exception ex)
        //{

        //    Log.AddExceptionToLog("Error in function [btnChangePassword_Click], class [change_password.aspx.cs]. Details: " + ex.Message);
        //}
        #endregion v1
        #region V2
        try
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();

            string strOldPass = null;
            string strNewPass = null;
            string strConfirmPass = null;
            string user_name = User.Identity.Name;

            strOldPass = txtOldPassword.Text;
            bl_password pwd = new bl_password();
            string Message = "";
           

            //check if old pass provided is correct
            if (Membership.ValidateUser(user_name, strOldPass))
            {
                strNewPass = txtNewPassword.Text;
                strConfirmPass = txtConfirmNewPassword.Text;
                pwd.PasswordIsValid(strNewPass, out Message);

                if (pwd.PasswordIsValid(txtNewPassword.Text, out Message))
                {
                    if (string.Compare(strNewPass, strConfirmPass) == 0)
                    {

                        MembershipUser myuser = Membership.GetUser(user_name);
                        myuser.ChangePassword(strOldPass, strNewPass);

                        lblmessage.Text = "Your password has been changed.";
                        lblmessage.ForeColor = System.Drawing.Color.Green;


                        txtOldPassword.Text = "";
                        txtNewPassword.Text = "";
                        txtConfirmNewPassword.Text = "";
                    }
                    else
                    {
                        lblmessage.Text = "New password and confirm password mismatched. Please check your input again.";
                        lblmessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblmessage.Text = Message;
                }

              
            }
            else
            {
                lblmessage.Text = "The old password you supplied is incorrect. Please check your input again.";
                lblmessage.ForeColor = System.Drawing.Color.Red;
            }

        }
        catch (Exception ex)
        {

            Log.AddExceptionToLog("Error in function [btnChangePassword_Click], class [change_password.aspx.cs]. Details: " + ex.Message);
        }
        #endregion V2
    }
}