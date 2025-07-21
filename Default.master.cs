using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
public partial class _Default : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //get username
        MembershipUser myUser = Membership.GetUser();

        string username = myUser.UserName;

        //FormsAuthentication.RedirectFromLoginPage(username, chkRememberMe.Checked);
        if (Roles.IsUserInRole(username, "Administrator") || Roles.IsUserInRole(username, "ICT"))
        {
            admin.Visible = true;
        }
        else if (Roles.IsUserInRole(username, "IA"))
        {
            CoreDataSub.Visible = false;
            NewBusinessModuleSubIndividualPolicy.Visible = false;
            NewBusinessModuleSubGTLIPolicy.Visible = false;
            NewBusinessModuleSubFlexiTermPolicy.Visible = false;
            NewBusinessModuleSubSimpleOne.Visible = false;
            NewBusinessModuleSubWingAl.Visible = false;
            NewBusinessModuleSubCMKPolicy.Visible = false;
            NewBusinessModuleSubFirstFinancePolicy.Visible = false;
           // NewBusinessModuleSubFlateRate.Visible = false;
            NewBusinessModuleSubMicroInsuranceApplicationIND.Visible = false;
            NewBusinessModuleSubMicroInsuranceUploadPayment.Visible = false;
            FinanceModuleSub.Visible = false;

            ReportModuleSubApplication.Visible = false;
            ReportModuleSubIndividualPolicy.Visible = false;
            ReportModuleSubGTLIPolicy.Visible = false;
            ReportModuleSubGTLIPremium.Visible = false;
            ReportModuleSubWingAccount.Visible = false;
            ReportModuleSubPremium.Visible = false;
            ReportModuleSubSale.Visible = false;
            ReportModuleSubExportPolicy.Visible = false;
            ReportModuleSubNewPolicyInMonth.Visible = false;
            ReportModuleSubMicroInsurancePaymentList.Visible = false;
            ReportModuleSubMicroInsurancePolicyPaymentComparison.Visible = false;
            //ReportModuleSubMicroInsuranceChart.Visible = false;
            ReportModuleSubGroupMicroInsurance.Visible = false;
            NewBusinessModuleSubGroupMicroInsurance.Visible = false;
            //policy servicing
            //PolicyServiceSub.Visible = false;
            PolicyServiceSubCalculatePolicyLap.Visible = false;
            PolicyServiceSubPolicyStatusCheck.Visible = false;
            PolicyServiceSubPolicyStatusAmendment.Visible = false;
            PolicyServiceSubPolicyPaymentHistory.Visible = false;
            PolicyServiceSubPolicyReinstatement.Visible = false;

            //end policy servicing
            BroadcastMessageSub.Visible = false;
            CardSub.Visible = false;
            admin.Visible = false;
            AlterationModuleSub.Visible = false;
        }
        else if (Roles.IsUserInRole(username, "IA-Manager"))
        {
            CoreDataSub.Visible = false;
            NewBusinessModuleSubMicroInsurance.Visible = false;
            NewBusinessModuleSubIndividualPolicy.Visible = false;
            NewBusinessModuleSubGTLIPolicy.Visible = false;
            NewBusinessModuleSubFlexiTermPolicy.Visible = false;
            NewBusinessModuleSubSimpleOne.Visible = false;
            NewBusinessModuleSubWingAl.Visible = false;
            NewBusinessModuleSubCMKPolicy.Visible = false;
            NewBusinessModuleSubFirstFinancePolicy.Visible = false;
            //NewBusinessModuleSubFlateRate.Visible = false;
            NewBusinessModuleSubMicroInsuranceApplicationIND.Visible = false;
            NewBusinessModuleSubMicroInsuranceUploadPayment.Visible = false;
            NewBusinessModuleSubGroupMicroInsurance.Visible = false;
            FinanceModuleSub.Visible = false;
            //policy servicing
            //PolicyServiceSub.Visible = false;
            PolicyServiceSubCalculatePolicyLap.Visible = false;
            PolicyServiceSubPolicyStatusCheck.Visible = false;
            PolicyServiceSubPolicyStatusAmendment.Visible = false;
            PolicyServiceSubPolicyPaymentHistory.Visible = false;
            PolicyServiceSubPolicyReinstatement.Visible = false;

            //end policy servicing
            BroadcastMessageSub.Visible = false;
            CardSub.Visible = false;
            admin.Visible = false;

            ReportModuleSubApplication.Visible = false;
            ReportModuleSubIndividualPolicy.Visible = false;
            ReportModuleSubGTLIPolicy.Visible = false;
            ReportModuleSubGTLIPremium.Visible = false;
            ReportModuleSubWingAccount.Visible = false;
            ReportModuleSubPremium.Visible = false;
            ReportModuleSubSale.Visible = false;
            ReportModuleSubExportPolicy.Visible = false;
            ReportModuleSubNewPolicyInMonth.Visible = false;
            ReportModuleSubMicroInsurancePaymentList.Visible = false;
            ReportModuleSubMicroInsurancePolicyPaymentComparison.Visible = false;
            ReportModuleSubGroupMicroInsurance.Visible = false;
            AlterationModuleSub.Visible = false;
        }
        else
        {
            admin.Visible = false;
        }         
        
    }
    protected void LgStatus_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(Membership.GetUser().UserName, "Default.master", bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.LOGOUT, DateTime.Now, "User logout", Membership.ApplicationName));

        Session.Clear();
        Response.Redirect("login.aspx");
    }
}
