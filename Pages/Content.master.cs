using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
public partial class Pages_Content : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
     ////get username
     //       MembershipUser myUser = Membership.GetUser();      

     //       string username = myUser.UserName;
	  
     //           //FormsAuthentication.RedirectFromLoginPage(username, chkRememberMe.Checked);
     //       if (Roles.IsUserInRole(username, "Administrator"))
     //           {
     //               admin.Visible = true;
     //           }
     //       else if (Roles.IsUserInRole(username, "IA"))
     //       {
     //           CoreDataSub.Visible = false;
     //           NewBusinessModuleSubIndividualPolicy.Visible = false;
     //           NewBusinessModuleSubGTLIPolicy.Visible = false;
     //           NewBusinessModuleSubFlexiTermPolicy.Visible = false;
     //           NewBusinessModuleSubSimpleOne.Visible = false;
     //           NewBusinessModuleSubWingAl.Visible = false;
     //           NewBusinessModuleSubCMKPolicy.Visible = false;
     //           NewBusinessModuleSubFirstFinancePolicy.Visible = false;
     //           NewBusinessModuleSubFlateRate.Visible = false;
     //           NewBusinessModuleSubMicroInsuranceApplicationIND.Visible = false;
     //           NewBusinessModuleSubMicroInsuranceUploadPayment.Visible = false;

     //           FinanceModuleSub.Visible = false;

     //           ReportModuleSubApplication.Visible = false;
     //           ReportModuleSubIndividualPolicy.Visible = false;
     //           ReportModuleSubGTLIPolicy.Visible = false;
     //           ReportModuleSubGTLIPremium.Visible = false;
     //           ReportModuleSubWingAccount.Visible = false;
     //           ReportModuleSubPremium.Visible = false;
     //           ReportModuleSubSale.Visible = false;
     //           ReportModuleSubExportPolicy.Visible = false;
     //           ReportModuleSubNewPolicyInMonth.Visible = false;
     //           ReportModuleSubMicroInsurancePaymentList.Visible = false;
     //           ReportModuleSubMicroInsurancePolicyPaymentComparison.Visible = false;
     //           ReportModuleSubMicroInsuranceChart.Visible = false;
     //           ReportModuleSubGroupMicroInsurance.Visible = false;
     //           //policy servicing
     //           //PolicyServiceSub.Visible = false;
     //           PolicyServiceSubCalculatePolicyLap.Visible = false;
     //           PolicyServiceSubPolicyStatusCheck.Visible = false;
     //           PolicyServiceSubPolicyStatusAmendment.Visible = false;
     //           PolicyServiceSubPolicyPaymentHistory.Visible = false;
     //           PolicyServiceSubPolicyReinstatement.Visible = false;
              
     //           //end policy servicing
               
     //           BroadcastMessageSub.Visible = false;
     //           CardSub.Visible = false;
     //           admin.Visible = false;
     //           NewBusinessModuleSubGroupMicroInsurance.Visible = false;
     //           AlterationModuleSub.Visible = false;
     //       }
     //       else if (Roles.IsUserInRole(username, "IA-Manager"))
     //       {
     //           CoreDataSub.Visible = false;
     //           NewBusinessModuleSubMicroInsurance.Visible = false;
     //           NewBusinessModuleSubIndividualPolicy.Visible = false;
     //           NewBusinessModuleSubGTLIPolicy.Visible = false;
     //           NewBusinessModuleSubFlexiTermPolicy.Visible = false;
     //           NewBusinessModuleSubSimpleOne.Visible = false;
     //           NewBusinessModuleSubWingAl.Visible = false;
     //           NewBusinessModuleSubCMKPolicy.Visible = false;
     //           NewBusinessModuleSubFirstFinancePolicy.Visible = false;
     //           NewBusinessModuleSubFlateRate.Visible = false;
     //           NewBusinessModuleSubMicroInsuranceApplicationIND.Visible = false;
     //           NewBusinessModuleSubMicroInsuranceUploadPayment.Visible = false;
     //           FinanceModuleSub.Visible = false;
     //           //policy servicing
     //           //PolicyServiceSub.Visible = false;
     //           PolicyServiceSubCalculatePolicyLap.Visible = false;
     //           PolicyServiceSubPolicyStatusCheck.Visible = false;
     //           PolicyServiceSubPolicyStatusAmendment.Visible = false;
     //           PolicyServiceSubPolicyPaymentHistory.Visible = false;
     //           PolicyServiceSubPolicyReinstatement.Visible = false;
             
     //           //end policy servicing
     //           BroadcastMessageSub.Visible = false;
     //           CardSub.Visible = false;
     //           admin.Visible = false;
     //           ReportModuleSubGroupMicroInsurance.Visible = false;
     //           ReportModuleSubApplication.Visible = false;
     //           ReportModuleSubIndividualPolicy.Visible = false;
     //           ReportModuleSubGTLIPolicy.Visible = false;
     //           ReportModuleSubGTLIPremium.Visible = false;
     //           ReportModuleSubWingAccount.Visible = false;
     //           ReportModuleSubPremium.Visible = false;
     //           ReportModuleSubSale.Visible = false;
     //           ReportModuleSubExportPolicy.Visible = false;
     //           ReportModuleSubNewPolicyInMonth.Visible = false;
     //           ReportModuleSubMicroInsurancePaymentList.Visible = false;
     //           ReportModuleSubMicroInsurancePolicyPaymentComparison.Visible = false;
     //           NewBusinessModuleSubGroupMicroInsurance.Visible = false;
     //           AlterationModuleSub.Visible = false;
     //       }
     //       else
     //       {
     //           admin.Visible = false;
     //       }       

        if (!Page.IsPostBack)
        {
            string ObjCode = Path.GetFileName(Request.Url.AbsolutePath);
            bl_sys_user_role ur = new bl_sys_user_role();


            List<bl_sys_user_role> Lobj = (List<bl_sys_user_role>)Session["SS_UR_LIST"];
            if (Lobj != null)
            {
                bl_sys_user_role u = ur.GetSysUserRole(Lobj, ObjCode);
                Session["SS_PERMISSION"] = u;
                if (u.ObjectCode == "" || u.ObjectCode == null)
                {
                    Response.Redirect("../../unauthorize.aspx");
                }
            }
            else
            {
                Response.Redirect("../../unauthorize.aspx");

            }
        }
    }
    protected void LgStatus_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(Membership.GetUser().UserName, "Content.master", bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.LOGOUT, DateTime.Now, "User logout", Membership.ApplicationName));

        Session.Clear();
        Response.Redirect("../../login.aspx");
    }
}
