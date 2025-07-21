﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for bl_sys_para
/// </summary>
public class bl_system
{
    public bl_system()
	{
		//
		// TODO: Add constructor logic here
		//
      
       
	}

    public class SYSTEM_PARAMATER
    {
        public SYSTEM_PARAMATER() { _errorMessage = ""; }
        public string ApplicationId { get; set; }
        public int Id { get; set; }
        public string ParamaterName { get; set; }
        public string ParamaterVal { get; set; }
        public string ParamaterDesc { get; set; }
        public bool Isactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedRemarks { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedRemarks { get; set; }
        public string ErrorMessage { get { return _errorMessage; } }
        private string _errorMessage = "";
        public bool Save(string applicationId, int id, string paramName, string paramVal, string paramDesc, bool isActive, string createdBy, DateTime createdOn, string createdRemarks = "")
        {
            bool result = false;
            try {
                DB db = new DB();
                result = db.Execute(AppConfiguration.GetAccountConnectionString(), "CT_SYS_PARAM_INSERT", new string[,] { 
                    {"@application_id", applicationId},
                {"@ID",id+""},{ "@PARAM_NAME",paramName}, {"@PARAM_VAL",paramVal},{"@IS_ACTIVE", isActive==true ? "1":"0"} , {"@PARAM_DESC",paramDesc}, {"@CREATED_BY",createdBy}, 
                {"@CREATED_ON", createdOn+""}, {"@CREATED_REMARKS",createdRemarks}
                }, "bl_system=>SYSTEM_PARAMATER=>Save(string applicationId, int id, string paramName, string paramVal, string paramDesc, bool isActive, string createdBy, DateTime createdOn, string createdRemarks = \"\")");
                if (db.RowEffect == -1)
                {
                    _errorMessage = db.Message;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = "Saved Error, please contact your system administrator.";
                result = false;
                Log.AddExceptionToLog("Error function [Save(string applicationId, int id, string paramName, string paramVal, string paramDesc, bool isActive, string createdBy, DateTime createdOn, string createdRemarks = \"\")] in class [bl_system=>SYSTEM_PARAMATER], detail: " + ex.Message + "=>" + ex.StackTrace);
            }
            return result;
        }
        public bool Update(string applicationId, int id, string paramName, string paramVal, string paramDesc, bool isActive, string updatedBy, DateTime updatedOn, string updatedRemarks = "")
        {
            bool result = false;
            try
            {
                DB db = new DB();
                result = db.Execute(AppConfiguration.GetAccountConnectionString(), "CT_SYS_PARAM_UPDATE", new string[,] { 
                    {"@application_id", applicationId},
                {"@ID",id+"" }, {"@PARAM_NAME",paramName}, {"@PARAM_VAL",paramVal},{"@IS_ACTIVE", isActive==true ? "1":"0"} , {"@PARAM_DESC",paramDesc}, {"@UPDATED_BY",updatedBy}, 
                {"@UPDATED_ON", updatedOn+""}, {"@UPDATED_REMARKS",updatedRemarks}
                }, "bl_system=>SYSTEM_PARAMATER=>Update(string applicationId, int id, string paramName, string paramVal, string paramDesc, bool isActive, string updatedBy, DateTime updatedOn, string updatedRemarks = \"\")");
                if (db.RowEffect == -1)
                {
                    _errorMessage = db.Message;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = "Updated Error, please contact your system administrator.";
                result = false;
                Log.AddExceptionToLog("Error function [Update(string applicationId, int id, string paramName, string paramVal, string paramDesc, bool isActive, string updatedBy, DateTime updatedOn, string updatedRemarks = \"\")] in class [bl_system=>SYSTEM_PARAMATER], detail: " + ex.Message + "=>" + ex.StackTrace);
            }
            return result;
        }
        /// <summary>
        /// Get all paramaters list
        /// </summary>
        /// <returns></returns>
        public List<SYSTEM_PARAMATER> GetParamaterList( string applicationId, string paramaterName="")
        {
            List<SYSTEM_PARAMATER> listParam = new List<SYSTEM_PARAMATER>();
            try
            {
                string procName = paramaterName == "" ? "CT_SYS_PARAM_GET_LIST" : "CT_SYS_PARAM_GET_LIST_BY_PARAM_NAME";
                DB db = new DB();
                DataTable tbl;
                if (paramaterName != "")
                {
                    tbl = db.GetData(AppConfiguration.GetAccountConnectionString(), "CT_SYS_PARAM_GET_LIST_BY_PARAM_NAME", new string[,] { { "@application_id", applicationId } ,{"@param_name",paramaterName}}, "bl_system=>SYSTEM_PARAMATER=>GetParamaterList(string applicationId, string paramaterName=\"\")");

                }
                else
                {
                    tbl = db.GetData(AppConfiguration.GetAccountConnectionString(), "CT_SYS_PARAM_GET_LIST", new string[,] { { "@application_id", applicationId } }, "bl_system=>SYSTEM_PARAMATER=>GetParamaterList(string applicationId, string paramaterName=\"\")");

                }
                foreach (DataRow r in tbl.Rows)
                {
                    listParam.Add(new SYSTEM_PARAMATER() {
                    Id= Convert.ToInt32(r["id"].ToString()),
                    ParamaterName=r["param_name"].ToString(),
                    ParamaterVal=r["param_val"].ToString(),
                    ParamaterDesc=r["param_desc"].ToString(),
                     Isactive = (r["is_active"].ToString()=="1"? true:false),
                      CreatedBy = r["created_by"].ToString(),
                      CreatedOn=Convert.ToDateTime(r["created_on"].ToString()),
                      CreatedRemarks = r["created_remarks"].ToString(),
                      UpdatedBy=r["updated_by"].ToString(),
                      UpdatedOn=Convert.ToDateTime(r["updated_on"].ToString()),
                      UpdatedRemarks=r["updated_remarks"].ToString()

                    });
                }
            }
            catch (Exception ex)
            {
                listParam = new List<SYSTEM_PARAMATER>();
                Log.AddExceptionToLog("Error function [GetParamaterList(string applicationId)] in class [bl_system=>SYSTEM_PARAMATER], detail: " + ex.Message + "=>" + ex.StackTrace);

            }
            return listParam;
        }
        /// <summary>
        /// Get active paramaters list
        /// </summary>
        /// <returns></returns>
        public List<SYSTEM_PARAMATER> GetParamaterListActive(string applicationId)
        {
            List<SYSTEM_PARAMATER> listParam = new List<SYSTEM_PARAMATER>();
            try
            {
                DB db = new DB();
                DataTable tbl = db.GetData(AppConfiguration.GetAccountConnectionString(), "CT_SYS_PARAM_GET_LIST_ACTIVE", new string[,] { { "@application_id", applicationId } }, "bl_system=>SYSTEM_PARAMATER=>GetParamaterListActive(string applicationId)");
                foreach (DataRow r in tbl.Rows)
                {
                    listParam.Add(new SYSTEM_PARAMATER()
                    {
                        Id = Convert.ToInt32(r["id"].ToString()),
                        ParamaterName = r["param_name"].ToString(),
                        ParamaterVal = r["param_val"].ToString(),
                        ParamaterDesc = r["param_desc"].ToString(),
                        Isactive = (r["is_active"].ToString() == "1" ? true : false),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                        CreatedRemarks = r["created_remarks"].ToString(),
                        UpdatedBy = r["updated_by"].ToString(),
                        UpdatedOn = Convert.ToDateTime(r["updated_on"].ToString()),
                        UpdatedRemarks = r["updated_remarks"].ToString()

                    });
                }
            }
            catch (Exception ex)
            {
                listParam = new List<SYSTEM_PARAMATER>();
                Log.AddExceptionToLog("Error function [GetParamaterListActive(string ApplicationId)] in class [bl_system=>SYSTEM_PARAMATER], detail: " + ex.Message + "=>" + ex.StackTrace);

            }
            return listParam;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="paramName"></param>
       /// <param name="isActive"></param>
       /// <returns></returns>
        public SYSTEM_PARAMATER GetParamater(string paramName, string applicationId, bool isActive = true)
        {
            SYSTEM_PARAMATER param = new SYSTEM_PARAMATER();
            try
            {
                DB db = new DB();
                string procName = isActive == true ? "CT_SYS_PARAM_GET_PARAM_NAME_ACTIVE" : "CT_SYS_PARAM_GET_PARAM_NAME";
                DataTable tbl = db.GetData(AppConfiguration.GetAccountConnectionString(), procName, new string[,] { { "@param_name", paramName }, { "@application_id", applicationId } }, "bl_system=>SYSTEM_PARAMATER=>GetParamater(string paramName, string applicationId, bool isActive = true)");
                
                    foreach (DataRow r in tbl.Rows)
                    {
                        param = new SYSTEM_PARAMATER()
                        {
                            Id = Convert.ToInt32(r["id"].ToString()),
                            ParamaterName = r["param_name"].ToString(),
                            ParamaterVal = r["param_val"].ToString(),
                            ParamaterDesc = r["param_desc"].ToString(),
                            Isactive = (r["is_active"].ToString() == "1" ? true : false),
                            CreatedBy = r["created_by"].ToString(),
                            CreatedOn = Convert.ToDateTime(r["created_on"].ToString()),
                            CreatedRemarks = r["created_remarks"].ToString(),
                            UpdatedBy = r["updated_by"].ToString(),
                            UpdatedOn = Convert.ToDateTime(r["updated_on"].ToString()),
                            UpdatedRemarks = r["updated_remarks"].ToString()

                        };
                        break;
                    }
                
            }
            catch (Exception ex)
            {
                param = new SYSTEM_PARAMATER();
                Log.AddExceptionToLog("Error function [GetParamater(string paramName, string applicationId, bool isActive = true] in class [bl_system=>SYSTEM_PARAMATER], detail: " + ex.Message + "=>" + ex.StackTrace);

            }
            return param;
        }

        public bool Delete(string applicationId, string paramName)
        {
            try
            {
                DB db = new DB();
                bool result = false;
                result = db.Execute(AppConfiguration.GetAccountConnectionString(), "CT_SYS_PARAM_DELETE", new string[,] { 
                    {"@application_id", applicationId},
                { "@param_name", paramName } 
                }, "bl_system=>SYSTEM_PARAMATER=>Delete(string applicationId, string paramName)");

                if (db.RowEffect == -1)
                    _errorMessage = "Deleted fail, please contact your system administrator.";
                return result;
            }
            catch (Exception ex)
            {
                _errorMessage = "Deleted fail, please contact your system administrator.";
                Log.AddExceptionToLog("Error function [Delete(string applicationId, string paramName)] in class [bl_system=>SYSTEM_PARAMATER], detail: " + ex.Message + "=>" + ex.StackTrace);

                return false;
            }
        }
    }

    public class SYSTEM_SETTING
    {
        public SYSTEM_SETTING() { }

        public static string APPLICATION_APPROVER { get { return "APPLICATION_APPROVER"; } }
        public static string SO_POLICY_INSURANCE_FILE_PATH { get { return "SO_POLICY_INSURANCE_FILE_PATH"; } }
        public static string SO_DHC_POLICY_INSURANCE_FILE_PATH { get { return "SO_DHC_POLICY_INSURANCE_FILE_PATH"; } }
        public static class SEND_EMAIL
        {
            public static string EMAIL_HOST { get { return "EMAIL-HOST"; } }
            public static string EMAIL_PORT { get { return "EMAIL-PORT"; } }
            public static string EMAIL_PASSWORD { get { return "EMAIL-PASSWORD"; } }
            public static string EMAIL_SENDER { get { return "EMAIL-SENDER"; } }
            public static class SEND_EMMAIL_ISSUE_POLICY_OPTION
            {
                public static string SEND_EMMAIL_ISSUE_POLICY { get { return "SEND-EMAIL-ISSUE-POLICY"; } }
                public static string SEND_EMMAIL_TO { get { return "SEND-EMAIL-ISSUE-POLICY-TO"; } } 
                public static class OPTION
                {
                    public static string YES { get { return "YES"; } }
                    public static  string NO { get { return "NO"; } }
                }
            }

            public static class SEND_EMAIL_RENEWAL_POLICY_OPTION
            {
                public static string SEND_EMMAIL_RENEWAL_TO { get { return "EMAIL-RENEWAL-TO"; } }
                public static string SEND_EMMAIL_RENEWAL_BCC { get { return "EMAIL-RENEWAL-BCC"; } }
            }
           
        }
       
        public static class MULTI_POLICY_OPTION
        {
            public static class CHECK_MULTI_POLICY_OPTION
            {
                public static string CHECK_MULTI_POLICY_BY { get { return "CHECK-MULTI-POLICY-BY"; } }
                public static class OPTION
                {
                    public static string NEW { get { return "NEW"; } }
                    public static string REPAYMENT { get { return "REPAYMENT"; } }
                }
               
            }
           
            public static class NEW_POLICY
            {
                public static class ALLOW_MULTI_NEW_POLICY_OPTION
                {
                    public static string ALLOW_MULTI_NEW_POLICY_PER_LIFE { get { return "ALLOW-MULTI-NEW-POLICY-PER-LIFE"; } }
                   
                    public static class OPTION
                    {
                        public static string YES { get { return "YES"; } }
                        public static string NO { get { return "NO"; } }
                    }
                }
                
            }
            public static class REPAYMENT_POLICY
            {
                public static class ALLOW_MULTI_REPAYMENT_OPTION
                {

                    public static string ALLOW_MULTI_REPAYMENT_POLICY_PER_LIFE { get { return "ALLOW-MULTI-REPAYMENT-POLICY-PER-LIFE"; } }
                    public static string ALLOW_REPAYMENT_BEFORE_POLICY_EXPIRE { get { return "ALLOW-REPAYMENT-POLICY-BEFORE-EXPIRE-DAYS"; } }
                    public static string ALLOW_REPAYMENT_AFTER_POLICY_EXPIRE { get { return "ALLOW-REPAYMENT-POLICY-AFTER-EXPIRE-DAYS"; } }
                    public class OPTION
                    {
                        public static string YES { get { return "YES"; } }
                        public static string NO { get { return "NO"; } }
                    }
                }

            }
        }

        public static class CELLCARD_ECERT_LINK
        {
           
            public static string CELLCARD_CERT_LINK { get { return "CELLCARD-CERT-LINK"; } }
            public static string CELLCARD_CERT_PARAS { get { return "CELLCARD-CERT-PARAS"; } }
        }
        public static class DOCUMENT
        {
            public static string DOCUMENT_TYPE { get { return "UPLOAD-DOC-TYPE"; } }
            public static string DOCUMENT_SIZE { get { return "UPLOAD-DOC-SIZE"; } }

            public static class LOCATION_OPTION
            {
                public static string LOCATION { get { return "DOC-LOCATION"; } }
                public static class OPTION
                {
                    public static string LOCAL { get { return "LOCAL";}  }
                    public static string REMOTE { get { return "REMOTE"; } }
                }
                public static string PATH { get { return "UPLOAD-DOC-PATH"; } }
            }
            public static class TRANSACTION_FILES
            {
                public static string PATH { get { return "TRANS_FILE_PATH"; } }
            }
        }

        public static class PASSWORD_POLICY_OPTION
        {
            public static string PASSWORD_POLICY { get { return "PASSWORD-POLICY"; } }
        }

        public static class CONNECTION
        {
            public static string CLIFE { get { return "CLIFE-SYSTEM-CONNECTION"; } }
            public static string CALLCENTER { get { return "CALLCENTER-SYSTEM-CONNECTION"; } }
            public static string SMS { get { return "SMS-SYSTEM-CONNECTION"; } }
            public static string INTRANET { get { return "INTRANET-SYSTEM-CONNECTION"; } }
            public static string CAMLIFE_MIDDLE_WARE { get { return "MIDDLE-WARE-SYSTEM-CONNECTION"; } }
        }

        public static class SEND_SMS_URL_KEY
        {
            public static string URL { get { return "SEND-SMS-URL"; } }
            public static string PARAMS { get { return "SEND-SMS-URL-PARAS"; } }
        }

        public static class API
        {
            public static class CAMLIFE
            {
                public static string TOKEN_URL { get { return "CAMLIFE-TOKEN-URL"; } }
                public static string TOKEN_USER { get { return "CAMLIFE-TOKEN-USER"; } }
                public static string TOKEN_PWD { get { return "CAMLIFE-TOKEN-PWD"; } }
                public static string API_URL { get { return "CAMLIFE-API-URL"; } }
            }
        }
    }

}

