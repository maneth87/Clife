using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CoreData_policy_status_code : System.Web.UI.Page
{
    string userid, user_name; int check_status_code = 0, check_detail = 0;
    bl_policy_status_type policy_status_type = new bl_policy_status_type();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Insert into Ct_Policy_Status_Type
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        policy_status_type.Policy_Status_Type_ID = txtPolicyStatusIDModal.Text.ToUpper();
        policy_status_type.Policy_Status_Code = txtPolicyCode.Text.ToUpper();
        policy_status_type.Detail = txtDetail.Text.ToUpper();
        policy_status_type.Terminated = chbTerminated.Checked;
        policy_status_type.Disabled = chbDisable.Checked;
        policy_status_type.Is_Reserved = chbReserved.Checked;
        policy_status_type.Created_On = DateTime.Now;
        policy_status_type.Created_By = user_name;
        policy_status_type.Created_Note = txtNote.Text;


        //if (da_policy_status_type.GetPolicy_By_Policy_ID(policy_status_type, 0) == true)
        //{
        //    check_status_code = 1;
        //}

        //if (da_policy_status_type.GetPolicy_By_Policy_ID(policy_status_type, 1) == true)
        //{
        //    check_detail = 1;
        //}

        //if (check_status_code == 1 && check_detail == 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Policy ID (" + policy_status_type.Policy_Status_Type_ID + ") has already existed. Please check it again.')", true);
        //}

        //if (check_status_code == 0 && check_detail == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Policy Code (" + policy_status_type.Policy_Status_Code + ") has already existed. Please check it again.')", true);
        //}

        //if (check_status_code == 1 && check_detail == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Policy ID (" + policy_status_type.Policy_Status_Type_ID + ") and The Policy Code (" + policy_status_type.Policy_Status_Code + ") have already existed, Please check them again.')", true);
        //}

        //if (check_detail == 0 && check_status_code == 0)
        //{
        //    da_policy_status_type.InsertPolicyStatus(policy_status_type);
        //}

        da_policy_status_type.InsertPolicyStatus(policy_status_type);

        ClearText();

        GvOPolicy.DataBind();
    }

    void ClearText()
    {
        txtPolicyStatusIDModal.Text = "";
        txtPolicyCode.Text = "";
        txtDetail.Text = "";
        chbTerminated.Checked = false;
        chbDisable.Checked = false;
        chbReserved.Checked = false;
        txtNote.Text = "";
    }


    /// <summary>
    /// Update Ct_Policy_Status_Type
    /// </summary>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        policy_status_type.Policy_Status_Type_ID = hdfPolicyID.Value.ToUpper();
        policy_status_type.Policy_Status_Code = txtEditPolicyCode.Text.ToUpper();
        policy_status_type.Detail = txtEditDetail.Text.ToUpper();
        policy_status_type.Terminated = chbEditTerminated.Checked;
        policy_status_type.Disabled = chbEditDisable.Checked;
        policy_status_type.Is_Reserved = chbEditReserved.Checked;
        policy_status_type.Created_On = DateTime.Now;
        policy_status_type.Created_By = user_name;
        policy_status_type.Created_Note = txtEditNote.Text;

        //if (da_policy_status_type.GetPolicy_By_Policy_ID(policy_status_type, 2) == false)
        //{
        //    da_policy_status_type.UpdatePolicyStatus(policy_status_type);
        //}
        //else { ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Policy Code (" + policy_status_type.Policy_Status_Code + ") has already existed. Please check it again.')", true); }

        da_policy_status_type.UpdatePolicyStatus(policy_status_type);

        GvOPolicy.DataBind();
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
       policy_status_type.Policy_Status_Type_ID= hdfDeletePolicyID.Value;

       if (da_policy_status_type.DeletePolicyStatus(policy_status_type) == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Policy Status Type ID (" + policy_status_type.Policy_Status_Type_ID + ") is being used. Please check it again.')", true);
        }

        GvOPolicy.DataBind();
    }
}