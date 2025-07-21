using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;


public partial class Pages_Business_Reports_approver_form : System.Web.UI.Page
{
    List<da_report_approver.bl_report_approver> ApproverList;
    string policy_id = "";
    string user_name = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        ApproverList = da_report_approver.GetApproverList();
       // ApproverList = new List<da_report_approver.bl_report_approver>();
        policy_id = Request.QueryString["policy_id"];
        MembershipUser myUser = Membership.GetUser();
        user_name = myUser.UserName;
        if (!Page.IsPostBack)
        {
          
            BindPosition();
           
        }
    }
    private void BindPosition()
    {
        ddlPosition.Items.Clear();
        ddlPosition.Items.Add(".");
        ddlFullName.Items.Add(".");
        foreach( da_report_approver.bl_report_approver approver in ApproverList)
        {
            ddlPosition.Items.Add(new ListItem(approver.PositionEn, approver.PositionKh + "/" + approver.Remarks));
        }

    }
    private void BindNameByPosition(string position)
    {
        ddlFullName.Items.Clear();
        ddlFullName.Items.Add(".");
         foreach( da_report_approver.bl_report_approver approver in ApproverList)
        {
            if (position.ToString().ToUpper().Trim() == approver.PositionEn.Trim().ToUpper())
            {

                ddlFullName.Items.Add(new ListItem(approver.NameEn, approver.NameKh));
            }
            
        }
    }
    protected void ddlPosition_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindNameByPosition(ddlPosition.SelectedItem.Text.Trim());
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        message.InnerHtml = "";
        ddlPosition.Items.Clear();
        ddlFullName.Items.Clear();
        BindPosition();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ddlPosition.SelectedIndex == 0)
        {
            message.InnerHtml = "Position is required.";
            return;
        }
        else if (ddlFullName.SelectedIndex == 0)
        {
            message.InnerHtml = "Full name is required.";
            return;
        }
        else
        {
            message.InnerHtml = "";
            if (policy_id != null && policy_id != "null" && policy_id != "")

            {
               
                string position = "";
                string[] str;
                position = ddlPosition.SelectedValue.Trim();
                str=position.Split(new char[] {'/'});

                Session["NAME_EN"] = ddlFullName.SelectedItem.Text.Trim();
                Session["NAME_KH"] = ddlFullName.SelectedValue.Trim();
                Session["POSITION_EN"] = str[1].ToString();
                Session["POSITION_KH"] = str[0].ToString();
                string url = "";

                #region Save approver
                List<da_report_approver.bl_report_approver> list_approver= da_report_approver.GetApproverList();
                 da_report_approver.bl_report_approver_policy approver_policy ;
                 da_report_approver.bl_report_approver approver_obj;
                foreach (da_report_approver.bl_report_approver approve in list_approver)
                {
                    if (approve.NameEn.Trim() == ddlFullName.SelectedItem.Text.Trim() && approve.NameKh.Trim() == ddlFullName.SelectedValue.Trim())
                    {
                       
                        approver_obj = da_report_approver.GetAproverInfo(policy_id);
                        if (approver_obj.ID == 0) // new policy approved
                        {
                            //da_report_approver.bl_report_approver_policy approver_policy = new da_report_approver.bl_report_approver_policy();
                            approver_policy = new da_report_approver.bl_report_approver_policy();
                            approver_policy.Approver_ID = approve.ID;
                            approver_policy.Policy_ID = policy_id;
                            approver_policy.Created_On = DateTime.Now;
                            approver_policy.Created_By = user_name;
                            da_report_approver.InsertApproverPolicy(approver_policy);
                            break;
                        }
                    }

                }
                #endregion


                url = "policy_schedule_RP_new.aspx?policy_id=" + policy_id;

                Response.Redirect(url);

            }
            else
            {
                message.InnerHtml = "Record(S) Not Found.";

            }
        }
    }
}