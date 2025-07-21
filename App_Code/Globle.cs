using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for Globle
/// </summary>
public class Globle
{
	public Globle()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">Text message to show user</param>
    /// /// <param name="type">0=Success, 1=Error, 2=warning</param>
    public static void showMessage(HtmlGenericControl div,  string message, int type)
    {
        
        if (message.Trim() != "")
        {
            if (type == 0)
            {

                div.Attributes.CssStyle.Add("background-color", "#228B22");
            }
            else if (type == 1)
            {
                div.Attributes.CssStyle.Add("background-color", "#f00");

            }
            else if (type == 2)
            {
                div.Attributes.CssStyle.Add("background-color", "#ffcc00");
            }
            div.Attributes.CssStyle.Add("display", "block");
            div.InnerHtml = message;
        }
        else
        {
            div.Attributes.CssStyle.Add("display", "none");
        }
    }
}