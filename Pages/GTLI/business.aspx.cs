using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_business : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            //bind user name and user id to hiddenfield
            hdfuserid.Value = user_id;
            hdfusername.Value = user_name;

            Load_Data();
        }
    }

    //Load Data to gridview
    private void Load_Data()
    {
        //refresh hdfBusinessID
        hdfBusinessID.Value = "";

        gvBusiness.DataSource = da_gtli_business.GetListOfBusiness();
        gvBusiness.DataBind();
    }

    //Insert New Business
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try{
            if(da_gtli_business.CheckExistingBusinessName(txtBusinessName.Text.Trim())){
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Business already exist.')", true);
                return;
            }

            bl_gtli_business business = new bl_gtli_business();
            business.GTLI_Business_ID = Helper.GetNewGuid("SP_Check_GTLI_Business_ID", "@GTLI_Business_ID");
            business.Business_Name = txtBusinessName.Text.Trim();
            business.Created_By = hdfusername.Value;
            business.Created_On = System.DateTime.Today;

            txtBusinessName.Text = "";

            if (da_gtli_business.InsertBusiness(business))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new business successfull.')", true);
                Load_Data();
            }else{
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new business failed.Please try again.')", true);
            }
        }catch(Exception ex){
             Log.AddExceptionToLog("Error function [btnSave_Click] in class [Pages_GTLI_business]. Details: " + ex.Message);
             ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Save new business failed.Please try again.')", true);
        }

    }

    //Delete Business
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string business_id = hdfBusinessID.Value;

            if (da_gtli_business.DeleteBusiness(business_id))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete business successfull.')", true);
                Load_Data();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete business failed.Please try again.')", true);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnDelete_Click] in class [Pages_GTLI_business]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Delete business failed.Please try again.')", true);
        }
    }
        
    //Update Business Name
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            bl_gtli_business business = new bl_gtli_business();
            business.GTLI_Business_ID =hdfBusinessID.Value;
            business.Business_Name = txtBusinessNameEdit.Text.Trim();

            if (da_gtli_business.UpdateBusiness(business))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update business successfull.')", true);
                Load_Data();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update business failed.Please try again.')", true);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [btnDelete_Click] in class [Pages_GTLI_business]. Details: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Update business failed.Please try again.')", true);
        }
    }
}