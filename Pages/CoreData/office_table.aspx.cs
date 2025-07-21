using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;


public partial class Pages_CoreData_office_table : System.Web.UI.Page
{
    int check_office_id=0, check_detail=0;
    bl_office office_table = new bl_office();
    string userid, user_name;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    void ClearText()
    {
        txtOfficeCodeModal.Text = "";
        txtOfficeDetailModal.Text = "";
        txtNoteModal.Text = "";
    }

    /// <summary>
    /// Insert
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;


        office_table.Office_ID = txtOfficeCodeModal.Text.ToUpper();
        office_table.Detail = txtOfficeDetailModal.Text.ToUpper();
        office_table.Status = 0; // new 0, delete -1;
        office_table.Created_On = DateTime.Now;
        office_table.Created_By = user_name;
        office_table.Created_Note = txtNoteModal.Text;
 
        //if (da_office.GetOffice_By_Office_ID(office_table,0) == true) /// 0, check Office ID
        //{
        //    check_office_id = 1;
        //}

        //if (da_office.GetOffice_By_Office_ID(office_table,1) == true) /// 1, check Office Detail
        //{
        //    check_detail = 1;
        //}

        //if(check_office_id==1 && check_detail==0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Office Code (" + office_table.Office_ID + ") has already existed. Please check it again.')", true);
        //}

        //if (check_office_id == 0 && check_detail == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Office Detail (" + office_table.Detail + ") has already existed. Please check it again.')", true);
        //}

        //if (check_office_id == 1 && check_detail == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Office Code (" + office_table.Office_ID + ") and The Office Detail ("+ office_table.Detail +") have already existed, Please check them again.')", true);
        //}

        //if (check_detail == 0 && check_office_id == 0)
        //{
        //    da_office.InsertOffice(office_table);
        //}

        da_office.InsertOffice(office_table);

        ClearText();

        GvOfficeTable.DataBind();
    }

    /// <summary>
    /// Update
    /// </summary>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        office_table.Old_Office_ID=hdOldOfficeID.Value.ToUpper();
        office_table.Office_ID = hdOldOfficeID.Value.ToUpper(); //txtEditOfficeID.Text;
        office_table.Detail = txtEditDetail.Text.ToUpper();
        office_table.Status = 0; // new 0, delete -1;
        office_table.Created_On = DateTime.Now;
        office_table.Created_By = user_name;
        office_table.Created_Note = txtEditNote.Text;

        //if (da_office.GetOffice_By_Office_ID(office_table,2) == false) /// Check Duplicate  Detail
        //{
        //    da_office.UpdateOfficeTable(office_table);
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Office Detail (" + office_table.Detail + ") has already existed. Please check it again.')", true);
        //}

        da_office.UpdateOfficeTable(office_table);

        GvOfficeTable.DataBind();
    }

    /// <summary>
    /// Delete
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        office_table.Office_ID = hdDeleteOfficeID.Value;

        if (da_office.GetOffice_ID_IsUsed_By_Office_ID(office_table) == false) 
        {
            da_office.DeleteOffice_Record(office_table);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Office Code ("+ office_table.Office_ID +") is being used. Please check it again.')", true);
        }

        GvOfficeTable.DataBind();
    }
}