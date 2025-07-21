using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CoreData_underwrite_code : System.Web.UI.Page
{
    string userid, user_name; int check_status_code = 0, check_detail = 0;
    bl_underwrite_code underwrite_code = new bl_underwrite_code();


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    void ClearText()
    {
        txtStatusCodeModal.Text = "";
        txtDetail.Text = "";
        chbInforce.Checked = false;
        chbReserved.Checked = false;
        txtNote.Text = "";
    }

    /// <summary>
    /// Insert into Ct_Underwrite_Table
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        underwrite_code.Status_Code = txtStatusCodeModal.Text;
        underwrite_code.Detail = txtDetail.Text;
        underwrite_code.Is_Inforce = chbInforce.Checked;
        underwrite_code.Is_Reserved = chbReserved.Checked;
        underwrite_code.Created_On = DateTime.Now;
        underwrite_code.Created_By = user_name;
        underwrite_code.Created_Note = txtNote.Text;

        //if(da_underwrite_code.GetUnderwrite_By_Status_Code(underwrite_code,0)==true)
        //{
        //    check_status_code = 1;
        //}

        //if (da_underwrite_code.GetUnderwrite_By_Status_Code(underwrite_code, 1) == true)
        //{
        //    check_detail = 1;
        //}

        //if (check_status_code == 1 && check_detail == 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Status Code (" + underwrite_code.Status_Code + ") has already existed. Please check it again.')", true);
        //}

        //if (check_status_code == 0 && check_detail == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Detail (" + underwrite_code.Detail + ") has already existed. Please check it again.')", true);
        //}

        //if (check_status_code == 1 && check_detail == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Status Code (" + underwrite_code.Status_Code + ") and The Detail (" + underwrite_code.Detail + ") have already existed, Please check them again.')", true);
        //}

        //if (check_detail == 0 && check_status_code == 0)
        //{
        //    da_underwrite_code.InsertUnderwrite_Code(underwrite_code);
        //}

        da_underwrite_code.InsertUnderwrite_Code(underwrite_code);

        ClearText();

        GvOUderwrite.DataBind();

    }

    /// <summary>
    /// Update Ct_Underwrite_Table
    /// </summary>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        underwrite_code.Status_Code = hdfStatusCode.Value;
        underwrite_code.Detail = txtEditDetail.Text;
        underwrite_code.Is_Inforce = chbEditInforce.Checked;
        underwrite_code.Is_Reserved = chbEditReserved.Checked;
        underwrite_code.Created_On = DateTime.Now;
        underwrite_code.Created_By = user_name;
        underwrite_code.Created_Note = txtEditNote.Text;

        //if (da_underwrite_code.GetUnderwrite_By_Status_Code(underwrite_code, 2) == false)
        //{
        //    da_underwrite_code.UpdateUnderwrite_Code(underwrite_code);
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Detail (" + underwrite_code.Detail + ") has already existed. Please check it again.')", true);
        //}

        da_underwrite_code.UpdateUnderwrite_Code(underwrite_code);

        GvOUderwrite.DataBind();
    }

    /// <summary>
    /// Delete from Ct_Underwrite_Table
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        underwrite_code.Status_Code = hdfDeleteStatusCode.Value;

        if (da_underwrite_code.DeleteUnderwrite_Code(underwrite_code) == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Status Code (" + underwrite_code.Status_Code + ") is being used. Please check it again.')", true);
        }

        GvOUderwrite.DataBind();
    }
}