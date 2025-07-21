using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for API
/// </summary>
public class API
{
	public API()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public class CAMLIFE
    {
        public CAMLIFE() { }

        public static string TokenURL { get { return GenerateTokenURL(); } }
        public static string TokenUser { get { return GenerateTokeUser(); } }
        public static string TokenPWD { get { return GenerateTokenPwd(); } }
        public static string APIURL { get { return GenerateAPIUrl(); } }


        private static string GenerateTokenURL()
        {
            return AppConfiguration.GetTokenURL();
        }
        private static string GenerateTokeUser()
        {
            return AppConfiguration.GetTokenUser();
        }
        private static string GenerateTokenPwd()
        {
            return AppConfiguration.GetTokenPWD();
        }
        private static string GenerateAPIUrl()
        {
            return AppConfiguration.GetCamlifeApiURL();
        }
        public class Response
        {
           public class Token
            {
                public string access_token { get; set; }
                public string token_type { get; set; }
                public int expires_in { get; set; }
                public string userName { get; set; }
            }
            public class RegistrationDocument
            {
                public string Status { get; set; }
                public int StatusCode { get; set; }
                public string Message { get; set; }
                public List<ObjectDetail> Detail { get; set; }

                public class ObjectDetail
                {
                    public string Id { get; set; }
                    public string DocName { get; set; }
                    public string DocPath { get; set; }
                    public string DocDescription { get; set; }
                    public string Remarks { get; set; }
                    public string UploadedBy { get; set; }
                    public DateTime UploadedOn { get; set; }
                }
            }
        }

        public class Reqeust
        {
            public class Document {
                public class RegistrationDocument
                {
                    public string DateFrom { get; set; }
                    public string DateTo { get; set; }
                    public string FileDescription { get; set; }
                }
            }
        }

        public static CAMLIFE.Response.Token GetToken()
        {
            CAMLIFE.Response.Token token = new Response.Token();
            try
            {
                System.Net.WebClient web = new WebClient();
                web.Headers.Add("content-type", "application/x-www-form-urlencoded");
                web.Encoding = System.Text.Encoding.UTF8;
                string strResponse = web.UploadString(TokenURL, "userName=" + TokenUser + "&password=" + TokenPWD + "&grant_type=password");

                token = JsonConvert.DeserializeObject<Response.Token>(strResponse);
            }
            catch (Exception ex)
            {
                token = null;
                Log.AddExceptionToLog("Error function [GetToken()] in class [API.CAMLIFE], detail: " + ex.Message);
            }
            return token;
        }

        public static CAMLIFE.Response.Token GetToken(string userName, string password)
        {
            CAMLIFE.Response.Token token = new Response.Token();
            try
            {
                System.Net.WebClient web = new WebClient();
                web.Headers.Add("content-type", "application/x-www-form-urlencoded");
                web.Encoding = System.Text.Encoding.UTF8;
                string strResponse = web.UploadString(TokenURL, "userName=" + userName + "&password=" + password + "&grant_type=password");

                token = JsonConvert.DeserializeObject<Response.Token>(strResponse);
            }
            catch (Exception ex)
            {
                token = null;
                Log.AddExceptionToLog("Error function [GetToken()] in class [API.CAMLIFE], detail: " + ex.Message);
            }
            return token;
        }

    }
}