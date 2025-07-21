using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Net;
/// <summary>
/// Summary description for bl_cellcard_message
/// </summary>
public class bl_cellcard_message : System.Web.UI.Page
{
    #region Public Variables
    private string __MessageToAPI = "";
    private string __MessageID = "";
    private string __ClientCorrelator = "";
    #endregion

    public bl_cellcard_message()
	{
		//
		// TODO: Add constructor logic here
		//
        __MessageID = Helper.GetNewGuid(AppConfiguration.GetCellCardMessageDbConnectionString(), new string[,] { { "TABLE", "tbl_BroadCastMessage" }, { "FIELD", "ID" } });
        __ClientCorrelator = GenerateClientCorrelator();
    }
    #region Properties
    public string ID
    { 
        get 
        {
            //return Helper.GetNewGuid(AppConfiguration.GetCellCardMessageDbConnectionString(),new string[,]{{"TABLE", "TBL_BROADCAST_MESSAGE"},{"FIELD","ID"}});
            return __MessageID;
        }
    }
    public string SenderName { get; set; }
    public string MessageFrom { get; set; }
    public string MessageFromAPI
    { 
        get 
        {
            return "tel" + Server.UrlEncode(":"+MessageFrom);
        }
    
    }
    public string MessageTo { get; set; }
    public string MessageText { get; set; }
    public string MessageToAPI //text format form API
    {
        get { return FormatMessageTo(); } 
        //set
        //{
        //    value =FormatMessageTo();
        //    __MessageToAPI = value;
        //}
    
    }
    public string MessageTextAPI //text format form API
    {
        get
        {
            return FormatMessageText();
        }
    }
    public DateTime SendDateTime { get; set; }
    public string Status { get; set; }
    public string Remarks { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string MessageCate { get; set; }
    public String IDForUpdated { get; set; }
    public string ClientCorrelator
    {
        get
        {
            return __ClientCorrelator;
        }
    }
    #endregion

    private string FormatMessageTo()
    {
        string formatedMessageTo = "";
        //formatedMessageTo = MessageTo.Replace(";", "&");
        formatedMessageTo = "address=tel"  + Server.UrlEncode(":") + MessageTo.Replace(";", "&address=tel" + Server.UrlEncode(":"));
        formatedMessageTo = formatedMessageTo.Replace("+", Server.UrlEncode("+"));
        //formatedMessageTo = MessageTo;
        //formatedMessageTo = MessageTo.Replace(" ", "%20");
        return formatedMessageTo;
    }
    private string FormatMessageText()
    {
        string formatedMessageText = "";
        //formatedMessageText = MessageText.Replace(":", "%3A");
        formatedMessageText = Server.UrlPathEncode(MessageText);
        return formatedMessageText;
    }

    /// <summary>
    /// Return [0]= "TRUE" or "FAIL" , [1] = PhoneNumber
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public string[] ValidatePhone(string phoneNumber)
    {
        string strPhoneNumber = "";
        int phoneNumberLength=0;
       
        string formatedNumber = "";
        string[] validate = new string[] { };

        //trim space
        strPhoneNumber = phoneNumber.Replace(" ", "").Trim();
        //trim sign '+'
        strPhoneNumber = strPhoneNumber.Replace("+", "").Trim();
        
        //check phone number length
        phoneNumberLength = strPhoneNumber.Length;
        if (phoneNumberLength >= 9 && phoneNumberLength <= 12)
        {
            //check number without 855
            //012363466 || 0763132168
            if (phoneNumberLength == 9 || phoneNumberLength == 10)
            {
                //check first degit with zero 
                if (strPhoneNumber.Substring(0, 1).Trim() == "0")
                {
                    formatedNumber = "+855" + strPhoneNumber.Substring(1, phoneNumberLength - 1);
                    validate = new string[] { "TRUE", formatedNumber};
                }
                else
                {
                    validate = new string[] { "FAIL", phoneNumber };//return with original phone number
                }
                
            }
            else //check number start with 855
                //85512363466
            {
                if (strPhoneNumber.Substring(0, 3).Trim() == "855")
                {
                    formatedNumber = "+" + strPhoneNumber;
                    validate = new string[] { "TRUE", formatedNumber};
                }
                else
                {
                    validate = new string[] { "FAIL", phoneNumber };//return with original phone number
                }

            }
        }
        else //reject
        {
            validate = new string[] { "FAIL", phoneNumber };//return with original phone number
        }

        return validate;
    }
    private string GenerateClientCorrelator()
    {
        string code = "";
        code = "C" + DateTime.Now.ToString("yyyyMMddhhmmss");
        return code;
    }



    public string[] SendMessage(bl_cellcard_message cellCard)
    {
        //cellcard id address: 172.16.100.180
        string[] result = new string[] { };
        string strResponse = "";
        try
        {

            System.Net.WebClient web = new WebClient();
            web.Headers.Add("cache-control", "no-cache");
            web.Headers.Add("content-type", "application/x-www-form-urlencoded");
            web.Headers.Add("authorization", "Basic Q2FtRmlmZTpjQE0xOExmZQ==");
            web.Encoding = System.Text.Encoding.UTF8;
            strResponse = web.UploadString("https://api.cellcard.com.kh:8244/oneapi/1/smsmessaging/outbound/tel%3A%2BCamLife/requests", "clientCorrelator=" + cellCard.ClientCorrelator + "&message=" + cellCard.MessageTextAPI + " &senderName=" + cellCard.SenderName + "&senderAddress=" + cellCard.MessageFromAPI + "&" + cellCard.MessageToAPI);

            //Check response error 
            ResponseError dataResponseError = JsonConvert.DeserializeObject<ResponseError>(strResponse);
            if (dataResponseError.requestError != null)
            {
                var messageID = dataResponseError.requestError.policyException.messageID;
                result = new string[] { strResponse, "Fail" };
            }
            else
            {
                ResponseSuccess dataResponse = JsonConvert.DeserializeObject<ResponseSuccess>(strResponse);
                if (dataResponse.resourceReference != null && dataResponse.resourceReference.resourceURL != "")
                {
                    result = new string[] { strResponse, "Success" };
                }
                else
                {
                    result = new string[] { strResponse, "Fail" };
                }


            }

        }
        catch (Exception ex)
        {
            result = new string[] { "Error", "Fail" };
            Log.AddExceptionToLog("Error function SendMessage(string messageId, string clientCorrelator, string message, string senderName, string senderAddress, string toAddress ) in page [UploadCellCardMessage], Detail:" + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return result;
    }
    #region Get Response Error
    class ResponseError
    {
        // public  policyException policyException{get;set;}
        public requestError requestError { get; set; }
    }
    class requestError
    {
        public policyException policyException { get; set; }
    }
    class policyException
    {
        public string messageID { get; set; }
        public string text { get; set; }
        public string variables { get; set; }
    }
    #endregion
    #region Get Response
    class ResponseSuccess
    {
        //public string resourceURL { get; set; }
        //public string resourceReference { get; set; }
        public resourceReference resourceReference { get; set; }
    }
    class resourceReference
    {
        public string resourceURL { get; set; }
    }
    #endregion

}