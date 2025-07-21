using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for HttpPost
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class HttpPost : System.Web.Services.WebService {

    public HttpPost () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    /// <summary>
    /// This function will call web service in http://192.168.1.9:12793
    /// </summary>
    /// <param name="phoneNumber">[phone number format +85512xxxxxx]</param>
    /// <param name="messageText"></param>
    /// <param name="messageCate"></param>
    [WebMethod]
    public void SendMessage(string phoneNumber, string messageText, string messageCate)
    {
        string strResponse="";
      
        try
        {
                System.Net.WebClient web = new System.Net.WebClient();
                web.Headers.Add("cache-control", "no-cache");
                web.Headers.Add("content-type", "application/x-www-form-urlencoded");
        
                    web.Encoding = System.Text.Encoding.UTF8;
                    strResponse = web.UploadString("http://192.168.1.9:12793/HttpPost.asmx/SendMessage", "phoneNumber=" + phoneNumber + "&messageText=" + messageText + "&messageCate=" + messageCate);
               
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [SendMessage(string phoneNumber, string messageText, string messageCate)] in class [HttpPost.cs], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
            strResponse = "fail";
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(strResponse));


    }
    
}
