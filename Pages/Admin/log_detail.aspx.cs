using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_log_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnRead_Click(object sender, EventArgs e)
    {
        try
        {
            string log = Log.ReadLog(Helper.FormatDateTime(txtdate.Text.Trim()));
            message.InnerHtml = log;
        }
        catch
        {
            message.InnerHtml = "<p style='color:red;'>Oooooop!</p>";
        }
    }
}