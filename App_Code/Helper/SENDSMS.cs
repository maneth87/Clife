using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

/// <summary>
/// Summary description for SENDSMS
/// </summary>
public class SENDSMS:System.Web.UI.Page
{
    /// <summary>
    /// Phone number: 012xxxxxx
    /// </summary>
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string MessageCate { get; set; }

	public SENDSMS()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public SENDSMS(string phone_number, string message, string message_cate)
    {
        PhoneNumber = phone_number;
        Message = message;
        MessageCate = message_cate;
    }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="phone_number">012xxxxxx</param>
   /// <param name="message"></param>
   /// <param name="message_cate"></param>
   /// <returns></returns>
    public bool Send(string phone_number, string message, string message_cate)
    {
        bool result = false;
        try
        {

            System.Net.WebClient web = new System.Net.WebClient();
            web.Headers.Add("cache-control", "no-cache");
            web.Headers.Add("content-type", "application/x-www-form-urlencoded");

            message_cate = Server.UrlEncode(message_cate);
            phone_number = "855" + phone_number.Substring(1, phone_number.Length - 1);
            phone_number = Server.UrlEncode(phone_number.Trim());
            message = Server.UrlEncode(message.Trim());
            string strResponse = "";
            web.Encoding = System.Text.Encoding.UTF8;

            // strResponse = web.UploadString("http://192.168.1.31:14387/HttpPost.asmx/SendMessage", "phoneNumber=" + phone_number + "&messageText=" + message + "&messageCate=" + message_cate);
            string url = AppConfiguration.GetSendSMSUrl();
            string param = AppConfiguration.GetSendSMSUrlParams().Replace(",", "&");
            param = string.Format(param, phone_number, message, message_cate);
            strResponse = web.UploadString(url, param);

            XDocument doc = XDocument.Parse(strResponse);

            foreach (var attr in doc.Descendants().Attributes())
            {
                var elem = attr.Parent;
                attr.Remove();
                elem.Add(new XAttribute(attr.Name.LocalName, attr.Value));
                strResponse = elem.Value;
            }

           
            if (strResponse.Trim().ToLower() == "true")
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [Send(string phone_number, string message)] in class [SENDSMS], detail:" + ex.Message + "=>" + ex.StackTrace);
        }
        return result;
    }
    public bool Send()
    {
        return Send(PhoneNumber, Message, MessageCate);
    }
}