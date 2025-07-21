using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Reflection;

/// <summary>
/// Get connection string from web.config
/// </summary>
public class AppConfiguration
{
    public AppConfiguration()
    {

    }
    private static string MYNAME = "AppConfiguration";
    private static string ApplicationID = da_user.GetApplicationID(System.Web.Security.Membership.ApplicationName);
    public static string GetConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["ApplicationDBContext"].ConnectionString.ToString();
        return connString;

      
    }

    public static string GetAccountConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
        return connString;
    }

    //ConnectionString "DefaultConnection" of database's name CAMLIFEAUTH
    public static string GetConnectionString_User()
    {
        string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
        return connString;
    }

    //Flexi Term Temp Database Connection String
    public static string GetFlexiTermConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["FlexiTermDBContext"].ConnectionString.ToString();
        return connString;
    }

    //Quotation Database Connection String
    public static string GetQuotationConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["QuotationDBContext"].ConnectionString.ToString();
        return connString;
    }
    //CellCard Message Database
    public static string GetCellCardMessageDbConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["CellCardMessageDb"].ConnectionString.ToString();
        return connString;
      
    }
    //CellCard Message Database
    public static string GetCallCenterConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["CallCenterDB"].ConnectionString.ToString();
        return connString;
    }
    //CAN Database
    public static string GetCanDbConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["CAN"].ConnectionString.ToString();
        return connString;
    }
    //Camlife Intranet
    public static string GetCamlifeIntranetConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["CamlifeIntranet"].ConnectionString.ToString();
        return connString;
    }

    //Camlife Midleware
    public static string GetCamlifeMidleWareConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["CamlifeMidleWare"].ConnectionString.ToString();
        return connString;
        
    }
    public static string GetCamlifePortalConnectionString()
    {
        string connString = ConfigurationManager.ConnectionStrings["CAMLIFEPORTAL"].ConnectionString.ToString();
        return connString;

    }

    //get cellcard certificate link
    public static string GetCellcardCertificateLink()
    {
       // return ConfigurationManager.AppSettings["CELLCARD-CERT-LINK"].ToString();
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj= sysObj.GetParamater(bl_system.SYSTEM_SETTING.CELLCARD_ECERT_LINK.CELLCARD_CERT_LINK, ApplicationID);
        return sysObj.ParamaterVal;

    }
    //get cellcard certificate link
    public static string GetCellcardCertificateLinkParas()
    {
        //return ConfigurationManager.AppSettings["CELLCARD-CERT-PARAS"].ToString();
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.CELLCARD_ECERT_LINK.CELLCARD_CERT_PARAS, ApplicationID);
        return sysObj.ParamaterVal;

    }
    /// <summary>
    /// all system to send email or not
    /// </summary>
    /// <returns></returns>
    public static bool GetSendEmailOption()
    {
       // string value = ConfigurationManager.AppSettings["SEND-EMAIL"].ToString().ToUpper();
       // return value == "YES" ? true : false;
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.SEND_EMMAIL_ISSUE_POLICY_OPTION.SEND_EMMAIL_ISSUE_POLICY, ApplicationID);
        return sysObj.ParamaterVal == bl_system.SYSTEM_SETTING.SEND_EMAIL.SEND_EMMAIL_ISSUE_POLICY_OPTION.OPTION.YES ? true : false;
    }
    public static string GetEmailHost()
    {
        //return ConfigurationManager.AppSettings["EMAIL-HOST"].ToString();
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.EMAIL_HOST, ApplicationID);
        return sysObj.ParamaterVal;
    }
    public static Int32 GetEmailPort()
    {
        // return Convert.ToInt32(ConfigurationManager.AppSettings["EMAIL-PORT"].ToString());
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.EMAIL_PORT, ApplicationID);
        return Convert.ToInt32(sysObj.ParamaterVal);

    }
    public static string GetEmailPassword()
    {
       // return ConfigurationManager.AppSettings["EMAIL-PASSWORD"].ToString();
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.EMAIL_PASSWORD, ApplicationID);
        return sysObj.ParamaterVal;
    }

    public static string GetEmailFrom()
    {
       // return ConfigurationManager.AppSettings["EMAIL-FROM"].ToString();
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.EMAIL_SENDER, ApplicationID);
        return sysObj.ParamaterVal;
    }
    public static string GetEmailTo()
    {
       // return ConfigurationManager.AppSettings["EMAIL-TO"].ToString();
        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.SEND_EMMAIL_ISSUE_POLICY_OPTION.SEND_EMMAIL_TO, ApplicationID);
        return sysObj.ParamaterVal;
    }
    
   
    public static string GetEmailRenewalTo()
    {
       // return ConfigurationManager.AppSettings["EMAIL-RENEWAL-TO"].ToString();

        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.SEND_EMAIL_RENEWAL_POLICY_OPTION.SEND_EMMAIL_RENEWAL_TO, ApplicationID);
        return sysObj.ParamaterVal;

    }
    public static string GetEmailRenewalBCC()
    {
        //return ConfigurationManager.AppSettings["EMAIL-RENEWAL-BCC"].ToString();

        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_EMAIL.SEND_EMAIL_RENEWAL_POLICY_OPTION.SEND_EMMAIL_RENEWAL_BCC, ApplicationID);
        return sysObj.ParamaterVal;

    }
    public static bool AllowMultiRepaymentPolicyPerLife()
    {

        try
        {
            //string para = ConfigurationManager.AppSettings["ALLOW-MULTI-POLICY-PER-LIFE"].ToString();
            //return para.ToUpper() == "YES" ? true : false;
            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.REPAYMENT_POLICY.ALLOW_MULTI_REPAYMENT_OPTION.ALLOW_MULTI_REPAYMENT_POLICY_PER_LIFE, ApplicationID);
            return sysObj.ParamaterVal == bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.REPAYMENT_POLICY.ALLOW_MULTI_REPAYMENT_OPTION.OPTION.YES ? true:false;


        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = ""

            });
            return false;
        }

    }
    public static int AllowMultiRepaymentPolicyBeforeExpireDays()
    {

        try
        {
            //string para = ConfigurationManager.AppSettings["ALLOW-MULTI-POLICY-BEFORE-EXPIRE-DAYS"].ToString();
            //return Convert.ToInt32(para);
            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.REPAYMENT_POLICY.ALLOW_MULTI_REPAYMENT_OPTION.ALLOW_REPAYMENT_BEFORE_POLICY_EXPIRE, ApplicationID);
            return Convert.ToInt32(sysObj.ParamaterVal);


        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = ""

            });
            return -1;
        }

    }
    public static int AllowMRepaymentPolicyAfterExpireDays()
    {

        try
        {
           // string para = ConfigurationManager.AppSettings["ALLOW-REPAYMENT-POLICY-AFTER-EXPIRE-DAYS"].ToString();
           // return Convert.ToInt32(para);

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.REPAYMENT_POLICY.ALLOW_MULTI_REPAYMENT_OPTION.ALLOW_REPAYMENT_AFTER_POLICY_EXPIRE, ApplicationID);
            return Convert.ToInt32(sysObj.ParamaterVal);
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = ""

            });
            return -1;
        }

    }

   // public enum MultiPolicyType { NEW, REPAYMENT };
    /// <summary>
    /// Value set in webconfig file: New or Repayment
    /// New: check on new policy creation
    /// Repayment: Check on policy repayment
    /// </summary>
    /// <returns></returns>
    public static string CheckMultiPolicy()
    {
        //string para = ConfigurationManager.AppSettings["CHECK-MULTI-POLICY-BY"].ToString();
        //return para;

        bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
        sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.CHECK_MULTI_POLICY_OPTION.CHECK_MULTI_POLICY_BY, ApplicationID);
        return sysObj.ParamaterVal;

    }
    /// <summary>
    /// This setting is use for control new policy
    /// </summary>
    /// <returns></returns>
    public static bool AllowMultiNewPolicyPerLife()
    {

        try
        {
            //string para = ConfigurationManager.AppSettings["ALLOW-MULTI-NEW_POLICY-PER-LIFE"].ToString();
            //return para.ToUpper() == "YES" ? true : false;

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.NEW_POLICY.ALLOW_MULTI_NEW_POLICY_OPTION.ALLOW_MULTI_NEW_POLICY_PER_LIFE, ApplicationID);
            return sysObj.ParamaterVal== bl_system.SYSTEM_SETTING.MULTI_POLICY_OPTION.NEW_POLICY.ALLOW_MULTI_NEW_POLICY_OPTION.OPTION.YES ? true : false ;

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [AllowMultiNewPolicyPerLife()] in class [AppConfiguration], detail: " + ex.Message + " ==> " + ex.StackTrace);

            return false;
        }

    }

    public static string GetUploadDocumentPath()
    {

        try
        {
            //string para = ConfigurationManager.AppSettings["UPLOAD-DOC-PATH"].ToString();
            //return para;

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.DOCUMENT.LOCATION_OPTION.PATH, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = ""

            });
            return "";
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GetDocumentLocation()
    {
        try
        {
            //string para = ConfigurationManager.AppSettings["DOC-LOCATION"].ToString();
            //return para;

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.DOCUMENT.LOCATION_OPTION.LOCATION, ApplicationID);
            return sysObj.ParamaterVal;

        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }
    }
    public static double GetDocumentSize()
    {
        try
        {
            //string para = ConfigurationManager.AppSettings["UPLOAD-DOC-SIZE"].ToString();
            //return Convert.ToDouble(para);

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.DOCUMENT.DOCUMENT_SIZE, ApplicationID);
            return Convert.ToDouble(sysObj.ParamaterVal);

        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return 0;
        }
    }
    public static string GetDocumentType()
    {
        try
        {
            //string para = ConfigurationManager.AppSettings["UPLOAD-DOC-TYPE"].ToString();
            //return para;
            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.DOCUMENT.DOCUMENT_TYPE, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }

    public static string TransactionFilesPath()
    {
        try
        {
            //string para = ConfigurationManager.AppSettings["UPLOAD-DOC-TYPE"].ToString();
            //return para;
            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.DOCUMENT.TRANSACTION_FILES.PATH, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }

    public static string GetPasswordPolicy()
    {
        try
        {
           
            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.PASSWORD_POLICY_OPTION.PASSWORD_POLICY, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }
    public static string GetSendSMSUrl()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_SMS_URL_KEY.URL, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }
    public static string GetSendSMSUrlParams()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SEND_SMS_URL_KEY.PARAMS, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }

    public static string GetTokenURL()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.API.CAMLIFE.TOKEN_URL, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }
   
    public static string GetTokenUser()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.API.CAMLIFE.TOKEN_USER, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }
    public static string GetTokenPWD()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.API.CAMLIFE.TOKEN_PWD, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }
    public static string GetCamlifeApiURL()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.API.CAMLIFE.API_URL, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }


    public static string GetApplicationApprover()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.APPLICATION_APPROVER, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }

    public static string GetSoPolicyInsuranceFilePath()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SO_POLICY_INSURANCE_FILE_PATH, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }
    public static string GetSoDhcPolicyInsuranceFilePath()
    {
        try
        {

            bl_system.SYSTEM_PARAMATER sysObj = new bl_system.SYSTEM_PARAMATER();
            sysObj = sysObj.GetParamater(bl_system.SYSTEM_SETTING.SO_DHC_POLICY_INSURANCE_FILE_PATH, ApplicationID);
            return sysObj.ParamaterVal;
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = System.Web.Security.Membership.GetUser().UserName

            });
            return "";
        }

    }

    public static string GetApplicationFormURL()
    {
        return ConfigurationManager.AppSettings["APP-FORM-URL"].ToString();
    }

    public static string GetCertificateURL()
    {
        return ConfigurationManager.AppSettings["CERT-URL"].ToString();
    }
}