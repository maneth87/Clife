using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Pages_Business_banca_document_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            if (Request.QueryString.Count == 0)
            {
                Response.Redirect("../../unknow_url.aspx");
            }
            else
            {
                string docName = Request.QueryString["doc_name"].ToString();
                string viewType = Request.QueryString["view_type"].ToString();
                string path = Server.MapPath("~/Temp/" + docName);
                if (System.IO.File.Exists(path))
                {

                    WebClient client = new WebClient();
                    Byte[] buffer = client.DownloadData(path);

                    string fileExt = Path.GetExtension(path);
                    string conType = "";
                    string[] img = new string[] { ".JPG", ".JPEG", ".PNG" };

                    if (img.Contains(fileExt.ToUpper()))
                    {
                        conType = "image/jpeg";
                    }
                    else if (fileExt.ToUpper() == ".PDF")
                    {
                        conType = "application/pdf";
                    }
                    if (buffer != null)
                    {
                        Response.Clear();
                        Response.ContentType = conType;// "application/pdf";
                        Response.AddHeader("content-length", buffer.Length.ToString());
                        Response.BinaryWrite(buffer);
                        if (viewType == "multy")
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
                else
                {
                    Response.Redirect("../../unknow_url.aspx");

                }
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }

      
    }

}