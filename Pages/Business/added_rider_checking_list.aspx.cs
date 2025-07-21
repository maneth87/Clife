using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Business_added_rider_checking_list : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable tbl = new DataTable();
            tbl = DataSetGenerator.Get_Data_Soure("Select * from V_Data_Check_List_premium_Detail_Add_Rider where Rider_type<>'basic' order by App_number");
            gvApplication.DataSource = tbl;
            gvApplication.DataBind();

        }
    }
    protected void gvApplication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "checking_list")
        {
            string app_id = "";
            GridViewRow grow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int index = grow.RowIndex;
            //Label lbl = (Label)gvApplication.Rows[index].FindControl("lblLevel");

            Label lbl = (Label)grow.FindControl("lblAppID");
            app_id = lbl.Text;
           // Response.Redirect("Reports/data_check_life_RP_new.aspx?app_register_id=" + app_id + "&action=" + app_id);
            string url = "Reports/data_check_life_RP_new.aspx?app_register_id=" + app_id + "&action=" + app_id;
          
            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url  +"');</script>", false);
            
        }
    }
}