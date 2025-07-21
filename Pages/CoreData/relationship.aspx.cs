using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Drawing;

public partial class Pages_CoreData_relationship : System.Web.UI.Page
{
    string userid, user_name, sale_type;
    bl_relationship relationship = new bl_relationship();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    
    void ClearText()
    {
        txtRelationshipModal.Text="";
        chbCleanCase.Checked=false;
        chbReserved.Checked=false;
        txtNote.Text="";
        txtRep_Kh.Text="";
    }

    /// <summary>
    /// Insert
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        relationship.Relationship = txtRelationshipModal.Text;
        relationship.Is_Clean_Case = chbCleanCase.Checked ;
        relationship.Is_Reserved = chbReserved.Checked; 

        relationship.Created_On = DateTime.Now;
        relationship.Created_By = user_name;
        relationship.Created_Note = txtNote.Text;
        relationship.Relationship_Khmer = txtRep_Kh.Text;

        //if (da_relationship.GetRelationship_By_Relationship(relationship) == false)
        //{
        //    da_relationship.InsertRelationship(relationship);
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Relationship (" + relationship.Relationship + ") has already existed. Please check it again.')", true);
        //}

        da_relationship.InsertRelationship(relationship);

        ClearText();

        GvORelationship.DataBind();
    }

    /// <summary>
    /// Update
    /// </summary>

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        relationship.Relationship = hdfRelationship.Value.ToUpper();
        relationship.Is_Clean_Case = chbEditCleanCase.Checked ;
        relationship.Is_Reserved = chbEditReserved.Checked; 

        relationship.Created_On = DateTime.Now;
        relationship.Created_By = user_name;
        relationship.Created_Note = txtEditNote.Text;
        relationship.Relationship_Khmer = txtEditRep_Kh.Text;

        da_relationship.UpdateRelationship(relationship);

        GvORelationship.DataBind();
    }

    /// <summary>
    /// Delete
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        relationship.Relationship = hdfDeleteRelationship.Value;
        da_relationship.DeleteRelationship(relationship);

        GvORelationship.DataBind();
    }

   
}