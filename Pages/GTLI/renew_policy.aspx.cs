using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_renew_policy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //Search Last Year Active Member for renew
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtCompanyName.Text.Trim() != "")
        {
            //Check last year policy status
            #region
                
            #endregion
            bl_gtli_member_list last_year_active_member = new bl_gtli_member_list();
        }
    }
}