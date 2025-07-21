using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
/// <summary>
/// Summary description for da_sl
/// </summary>
public class da_sl : System.Web.UI.Page
{
	public da_sl()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone_number">[85512xxxxxx]</param>
    /// <param name="dob">DD/MM/YYYY</param>
    /// <returns></returns>
    public  eCertificate GetEcertificate(string phone_number, string dob)
    { 
        eCertificate cert= new eCertificate();
        try
        {
            WebClient web = new WebClient();
            string strResponse = "";
            ResponseError responseError = new ResponseError();
            web.Headers.Add("cache-control", "no-cache");
            web.Headers.Add("content-type", "application/x-www-form-urlencoded");
            web.Encoding = System.Text.Encoding.UTF8;
            phone_number = Server.UrlEncode(phone_number.Trim());
            dob = Server.UrlEncode(dob.Trim());
            string link = AppConfiguration.GetCellcardCertificateLink();
            string paras = AppConfiguration.GetCellcardCertificateLinkParas();
            if (link != "" && paras != "")
            {
                paras = paras.Replace(",", "&");
                paras=string.Format(paras,phone_number, dob);

             
                strResponse = web.UploadString(link, paras);

                responseError = JsonConvert.DeserializeObject<ResponseError>(strResponse);
                if (responseError.Error != null)
                {
                    cert = new eCertificate();//set blank object
                    cert.ErrorMessage = "Policy is not found.";
                }
                else
                {
                    cert = JsonConvert.DeserializeObject<eCertificate>(strResponse);
                }
            }
            else
            {
                cert = new eCertificate();//set blank object
                cert.ErrorMessage = "System cannot find the specific link or paramaters.";
            }

           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetEcertificate(string phone_number, string dob)] in class [da_sl], detail:" + ex.Message);
            cert = new eCertificate();//set blank object
            cert.ErrorMessage = "Ooop! something is going wrong.";
       }
        return cert;
    }
  /// <summary>
  /// 
  /// </summary>
  /// <param name="phone_number">85512xxxxxx</param>
  /// <returns></returns>
    public eCertificate GetPolicy(string phone_number)
    {
        eCertificate cert = new eCertificate();
        try
        {
            WebClient web = new WebClient();
            string strResponse = "";
            ResponseError responseError = new ResponseError();
            web.Headers.Add("cache-control", "no-cache");
            web.Headers.Add("content-type", "application/x-www-form-urlencoded");
            web.Encoding = System.Text.Encoding.UTF8;
            phone_number = Server.UrlEncode(phone_number.Trim());

            strResponse = web.UploadString("http://192.168.1.9:171/Certificate.asmx/GetPolicy", "phone_number=" + phone_number );
            //strResponse = web.UploadString("http://localhost:52408/CallCenterSystem-SVN/Certificate.asmx/GetPolicy", "phone_number=" + phone_number );
            responseError = JsonConvert.DeserializeObject<ResponseError>(strResponse);
            if (responseError.Error != null)
            {
                cert = new eCertificate();//set blank object
            }
            else
            {
                cert = JsonConvert.DeserializeObject<eCertificate>(strResponse);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicy(string phone_number)] in class [da_sl], detail:" + ex.Message);
            cert = new eCertificate();//set blank object
        }
        return cert;
    }
    public class ResponseError
    {
      public  string Error { get; set; }
    }
}