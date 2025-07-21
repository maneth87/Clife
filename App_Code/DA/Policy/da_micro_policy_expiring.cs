using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
/// <summary>
/// Summary description for da_micro_policy_expiring
/// </summary>
public class da_micro_policy_expiring
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
    private static DB db = new DB();
    /// <summary>
    /// Class Name
    /// </summary>
    private static string MYNAME = "da_micro_policy_expiring";
	public da_micro_policy_expiring()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static List<bl_micro_policy_expiring> GetPolicyExpiringList(string channel_item_id, string channel_location_id, string userName="")
    {
        List<bl_micro_policy_expiring> exList = new List<bl_micro_policy_expiring>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_GET", new string[,] { 
            {"@channel_item_id", channel_item_id},
            {"@channel_location_id", channel_location_id}
            }, "da_micro_policy_expiring =>  GetPolicyExpiringList(string channel_item_id, string channel_location_id, string userName)");
            foreach (DataRow r in tbl.Rows)
            {
            exList.Add(new bl_micro_policy_expiring() {
            BranchCode=r["branch_code"].ToString(),
            PolicyID=r["policy_id"].ToString(),
            PolicyNumber=r["policy_number"].ToString(),
            PolicyStatus=r["policy_status"].ToString(),
            CustNo=r["customer_number"].ToString(),
            CustNameEn=r["full_name_en"].ToString(),
            CustNamekh=r["full_name_kh"].ToString(),
            CustGender=r["gender"].ToString(),
            CustIdNo=r["id_number"].ToString(),
            CustDob=Convert.ToDateTime(r["date_of_birth"].ToString()),
            CustContactNo=r["contact_number"].ToString(),
            IssuedDate=Convert.ToDateTime(r["issued_date"].ToString()),
            EffectiveDate=Convert.ToDateTime(r["effective_date"].ToString()),
            ExpiryDate=Convert.ToDateTime(r["expiry_date"].ToString()),
            NextDueDate=Convert.ToDateTime(r["next_due_date"].ToString()),
            ProductID=r["product_id"].ToString(),
            ProductName=r["product_name"].ToString(),
            ProductRemarks=r["product_remarks"].ToString(),
            Premium=Convert.ToDouble(r["premium"].ToString()),
            PaymentMode=r["payment_mode"].ToString(),
            SumAssure=Convert.ToDouble(r["sum_assure"].ToString()),
            DHC=Convert.ToDouble(r["dhc"].ToString()),
            Amount=Convert.ToDouble(r["amount"].ToString()),
            DiscountAmount=Convert.ToDouble(r["discount_amount"].ToString()),
            TotalAmount=Convert.ToDouble(r["total_amount"].ToString()),
            Channel=r["channel"].ToString(),
            Company=r["company"].ToString(),
            AgentCode=r["agent_code"].ToString(),
            AgentName=r["agent_name"].ToString(),
            GeneratedDate=Convert.ToDateTime(r["generated_date"].ToString()),
            ExpiryIn=Convert.ToInt32(r["expiry_in"].ToString()),
            ExpiryInCurrent=Convert.ToInt32(r["expiry_in_current"].ToString()),
            ChannelItemId=r["channel_item_id"].ToString(),
            ChannelLocationId=r["channel_location_id"].ToString()
            });
            }
        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log() {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            }); 
        }
        return exList;
    }
    public static bl_micro_policy_expiring GetPolicyExpiringObject(string policyId, string userName = "")
    {
        bl_micro_policy_expiring obj = new bl_micro_policy_expiring();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_GET_BY_POLICY_ID", new string[,] { 
            {"@POLICY_ID", policyId}
            }, "da_micro_policy_expiring => GetPolicyExpiring(string policyId, string userName)");
            foreach (DataRow r in tbl.Rows)
            {
                obj=(new bl_micro_policy_expiring()
                {
                    BranchCode = r["branch_code"].ToString(),
                    PolicyID = r["policy_id"].ToString(),
                    PolicyNumber = r["policy_number"].ToString(),
                    PolicyStatus = r["policy_status"].ToString(),
                    CustNo = r["customer_number"].ToString(),
                    CustNameEn = r["full_name_en"].ToString(),
                    CustNamekh = r["full_name_kh"].ToString(),
                    CustGender = r["gender"].ToString(),
                    CustIdNo = r["id_number"].ToString(),
                    CustDob = Convert.ToDateTime(r["date_of_birth"].ToString()),
                    CustContactNo = r["contact_number"].ToString(),
                    IssuedDate = Convert.ToDateTime(r["issued_date"].ToString()),
                    EffectiveDate = Convert.ToDateTime(r["effective_date"].ToString()),
                    ExpiryDate = Convert.ToDateTime(r["expiry_date"].ToString()),
                    NextDueDate = Convert.ToDateTime(r["next_due_date"].ToString()),
                    ProductID = r["product_id"].ToString(),
                    ProductName = r["product_name"].ToString(),
                    ProductRemarks = r["product_remarks"].ToString(),
                    Premium = Convert.ToDouble(r["premium"].ToString()),
                    PaymentMode = r["payment_mode"].ToString(),
                    SumAssure = Convert.ToDouble(r["sum_assure"].ToString()),
                    DHC = Convert.ToDouble(r["dhc"].ToString()),
                    Amount = Convert.ToDouble(r["amount"].ToString()),
                    DiscountAmount = Convert.ToDouble(r["discount_amount"].ToString()),
                    TotalAmount = Convert.ToDouble(r["total_amount"].ToString()),
                    Channel = r["channel"].ToString(),
                    Company = r["company"].ToString(),
                    AgentCode = r["agent_code"].ToString(),
                    AgentName = r["agent_name"].ToString(),
                    GeneratedDate = Convert.ToDateTime(r["generated_date"].ToString()),
                    ExpiryIn = Convert.ToInt32(r["expiry_in"].ToString()),
                    ExpiryInCurrent = Convert.ToInt32(r["expiry_in_current"].ToString()),
                    ChannelItemId = r["channel_item_id"].ToString(),
                    ChannelLocationId = r["channel_location_id"].ToString(),
                    NewApplicationNumber=r["new_application_number"].ToString()
                });
            }
        }
        catch (Exception ex)
        {
            obj = new bl_micro_policy_expiring();
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return obj;
    }

    public static DataTable GetPolicyExpiring(string channel_item_id, string channel_location_id, string userName="")
    {
        DataTable tbl = new DataTable(); ;
        try
        {
             tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_GET", new string[,] { 
            {"@channel_item_id", channel_item_id},
            {"@channel_location_id", channel_location_id}
            }, "da_micro_policy_expiring =>  GetPolicyExpiring(string channel_item_id, string channel_location_id)");
           
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
                UserName = userName
            });
        }
        return tbl;
    }

    public static DataTable GetPolicyExpiringSummary(string channel_item_id, string channel_location_id, DateTime generatedDate, string userName = "")
    {
        DataTable tbl = new DataTable();

        try
        {
              tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_SUMMARY_GET", new string[,] { 
            {"@channel_item_id", channel_item_id},
            {"@channel_location_id", channel_location_id},
             {"@generated_date", generatedDate+""}
            }, "da_micro_policy_expiring =>  GetPolicyExpiringSummary(string channel_item_id, string channel_location_id, DateTime generatedDate, string userName)");
              if (db.RowEffect < 0)
              {
                  //error generate
                  _MESSAGE = db.Message;
                  _SUCCESS = false;

              }
              else
              {
                  _SUCCESS = true;
              }
        }
        catch (Exception ex)
        {
            _MESSAGE = ex.Message;
            tbl = new DataTable();
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return tbl;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationNumber"> New application number</param>
    /// <param name="policyId"></param>
    /// <param name="status"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedOn"></param>
    /// <param name="Remarks"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public static bool Update(string applicationNumber, string policyId, string status, string updatedBy, DateTime updatedOn, string Remarks, string userName = "")
    {
        
        try
        {
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_UPDATE", new string[,] { 
            {"@policy_id", policyId},
            {"@application_number", applicationNumber},
            {"@status", status},
            {"@updated_by", updatedBy},
            {"@updated_on", updatedOn+""},
            {"@remarks", Remarks}
            }, "da_micro_policy_expiring => Update(string applicationNumber, string policyId, string status, string updatedBy, DateTime updatedOn, string Remarks, string userName)");
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return _SUCCESS;
    }

    public static bool Delete( string policyId, string userName = "")
    {

        try
        {
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_EXPIRING_DELETE", new string[,] { 
            {"@policy_id", policyId}
            }, "da_micro_policy_expiring => Delete( string policyId, string userName)");
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });
        }
        return _SUCCESS;
    }
    public static bool SendSummaryNotification(string channel_item_id, string channel_location_id, DateTime generatedDate, string userName = "")
    {
        string body = "";
        EmailSender mail;
        mail = new EmailSender();
        mail.From = AppConfiguration.GetEmailFrom();
        mail.To = AppConfiguration.GetEmailRenewalTo();
        mail.BCC = AppConfiguration.GetEmailRenewalBCC();
        mail.Subject = "Policy expire in 30 days";
      
        mail.Host = AppConfiguration.GetEmailHost();

        mail.Port = AppConfiguration.GetEmailPort();
        mail.Password = AppConfiguration.GetEmailPassword();
        try
        {
            DataTable tbl = GetPolicyExpiringSummary(channel_item_id, channel_location_id, generatedDate, userName);
          
           
            if (da_micro_policy_expiring.SUCCESS)
            {
                int rcount = 0;

                if (tbl.Rows.Count > 0)
                {
                    body += "Dear Respective Team, <br /> Here is the summary list of policy expire in 30 days.<br />";
                    body += "<table border='1'> <th>No.</th><th>Brand Code</th><th>No. Polices</th><th>Agent Code</th><th>Agent Name</th><th>BTL</th>";
                    foreach (DataRow r in tbl.Rows)
                    {
                        rcount += 1;
                        body += "<tr><td>" + rcount + "</td><td>" + r["Branch_Code"].ToString() + "</td><td>" + r["NOPOLICY"].ToString() + "</td><td>" + r["agent_code"].ToString() + "</td><td>" + r["agent_name"].ToString() + "</td><td>" + r["btl"].ToString() + "</td></tr>";
                    }
                    body += "</table>";
                    #region send mail
                    mail.Message = body;
                    _SUCCESS = mail.SendMail(mail);

                    #endregion send mail
                    
                }
                else// no data to send
                { 
                    // send email alert to IT
                    mail.To = "ict@camlife.com.kh";
                    mail.BCC = "";
                    body = "There is no expiring policy in 30 days, system processes on " + DateTime.Now.ToString("dd-MM-yyyy");
                    mail.Message = body;
                    _SUCCESS = mail.SendMail(mail);
                }

                Log.SaveLog(new bl_log()
                {
                    LogDate = DateTime.Now,
                    Class = MYNAME,
                    FunctionName = MethodBase.GetCurrentMethod().Name,
                    LogType = _SUCCESS == true ? "INFO": "ERROR",
                    ErrorLine = 0,
                    Message = tbl.Rows.Count + " Record(s) were generated and " + (tbl.Rows.Count > 0 ? (_SUCCESS == true ? " send email successfully" : "send email fail") : " skip sending."),
                    UserName = userName
                });

            }
            
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = MYNAME,
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName
            });

            // send email alert to IT
            mail.To = "ict@camlife.com.kh";
            mail.BCC = "";
            body = "System got error while generate policies expire in 30 days, system processes on " + DateTime.Now.ToString("dd-MM-yyyy") + " <br/> Error Detail:<br/>" + ex.Message;
            mail.Message = body;
            _SUCCESS = mail.SendMail(mail);
        }
        return _SUCCESS;
    }
}