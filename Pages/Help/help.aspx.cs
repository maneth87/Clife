using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Help_help : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count > 0)
        {
            try
            {
                string fileName = Request.QueryString["fName"].ToString();
                string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"600px\">";
                //embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                //embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                embed += "</object>";
                //ltFile.Text = string.Format(embed, ResolveUrl("~/Upload/Help/How-to-upload-payment-list.pdf"));
                ltFile.Text = string.Format(embed, ResolveUrl("~/Upload/Help/" + fileName));
                div_message.Attributes.CssStyle.Add("display", "none");
            }
            catch 
            {
                div_message.InnerHtml = "Oooop! parameter is not correct format." ;
            }
        }
        else
        {
            div_message.Attributes.CssStyle.Add("display", "block");
            div_message.InnerHtml = "Parameter is required.";
        }
    }
}