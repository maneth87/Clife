using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_group_policy
/// </summary>
/// 

public class da_micro_group_policy
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";
    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
    private static DB db = new DB();
    public da_micro_group_policy()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool Save(bl_micro_group_policy policy)
    {

        try
        {
            var a = policy;
            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_INSERT", new string[,] 
            {
            {"@policy_id", a.PolicyId},
            {"@group_master_id",a.GroupMasterId},
            {"@customer_id",a.CustomerId},
            {"@loaN_id",a.LoanId},
            {"@application_id", a.ApplicationId},
            {"@seq", a.SEQ+""},
            {"@policy_number", a.PolicyNumber},
            {"@product_id", a.ProductId},
            {"@currency",a.Currency},
            {"@exchange_rate",a.ExchangeRate+""},
            {"@policy_status", a.PolicyStatus},
            {"@policy_status_date",a.PolicyStatusDate+""},
            {"@is_reduce_sa", a.IsReduceSA+""},
            {"@year_of_reduce_sa",a.YearOfReduceSA+""},
            {"@agent_code", a.AgentCode+""},
            {"@channel_id",a.ChannelId},
            {"@channel_item_id", a.ChannelItemId},
            {"@Channel_location_id", a.ChannelLocationId},
            {"@created_by", a.createdBy},
            {"@created_on",a.CreatedOn+""},
            {"@remarks",a.Remarks},
            {"@is_first_policy",a.IsFirstPolicy == true ? "1" :"0"}
            }, "da_micro_group_policy=>Save(bl_micro_group_policy policy)");
            if (!_SUCCESS)
            {
                _MESSAGE = db.Message;
            }
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [Save(bl_micro_group_policy policy) in class [da_micro_group_policy], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return _SUCCESS;
    }

    public static List<bl_micro_group_policy> GetPolicyList(string customerNumber)
    {
        List<bl_micro_group_policy> poList = new List<bl_micro_group_policy>();
        try
        {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_GET_BY_CUST_NO", new string[,] { 
                {"@CUSTOMER_NUMBER", customerNumber}
            }, "da_micro_group_policy=>GetPolicyList(string customerNumber)");
            if (db.RowEffect < 0)
            {
                poList = null;
                _MESSAGE = "Get Policy List is getting error, please contact your system administrator.";
                _SUCCESS = false;
            }
            else if (db.RowEffect == 0)
            {
                poList = null;
                _MESSAGE = "No record found.";
                _SUCCESS = true;
            }
            else
            {
                _SUCCESS = true;
                _MESSAGE = "Get policy list successfully.";
                foreach (DataRow r in tbl.Rows)
                {

                    poList.Add(new bl_micro_group_policy()
                    {
                        PolicyId = r["policy_id"].ToString(),
                        PolicyNumber = r["policy_number"].ToString(),
                        CustomerId = r["customer_id"].ToString(),
                        ProductId = r["product_id"].ToString(),
                        Currency = r["currency"].ToString(),
                        ExchangeRate = Convert.ToDouble(r["exchange_rate"].ToString()),
                        PolicyStatus = r["policy_status"].ToString(),
                        PolicyStatusDate = Convert.ToDateTime(r["policy_status_date"].ToString()),
                        AgentCode = r["agent_code"].ToString(),
                        ChannelId = r["channel_id"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ChannelLocationId = r["channel_location_id"].ToString(),
                        ApplicationId = r["application_id"].ToString()

                    });
                }
            }
        }
        catch (Exception ex)
        {
            poList = null;
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [GetPolicyList(string customerNumber)] in class [da_micro_group_policy], Detail:" + ex.Message + "=>" + ex.StackTrace);
        }
        return poList;
    }

    public static bool UpdatePolicyStatus(string policyId, string policyStatus, DateTime policyStatusDate, string policyStatusRemarks, string updatedBy, DateTime updatedOn)
    {

        try
        {

            _SUCCESS = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_UPDATE_POLICY_STATUS", new string[,] 
            {
            {"@policy_id", policyId},
            {"@policy_status",policyStatus},
            {"@policy_status_date",policyStatusDate+""},
            {"@policy_status_remarks",policyStatusRemarks},
            {"@updated_by", updatedBy},
            {"@updated_on", updatedOn+""}
            }, "da_micro_group_policy=>UpdatePolicyStatus(string policyId, string policyStatus, DateTime policyStatusDate, string policyStatusRemarks, string updatedBy, DateTime updatedOn)");
            if (!_SUCCESS)
            {
                _MESSAGE = db.Message;
            }
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [UpdatePolicyStatus(string policyId, string policyStatus, DateTime policyStatusDate, string policyStatusRemarks, string updatedBy, DateTime updatedOn)] in class [da_micro_group_policy], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return _SUCCESS;
    }


    public static DataTable GetPolicyDetail(string policyId)
    {
        DataTable tbl = new DataTable();
        try
        {
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_GET_POL_DETAIL_BY_POL_ID", new string[,] 
            {
            {"@policy_id",policyId}
           
            }, "da_micro_group_policy=>GetPolicyDetail(string policyId)");
            if (!_SUCCESS)
            {
                _MESSAGE = db.Message;
            }
            if (db.RowEffect > 0)
            {
                _SUCCESS = true;
                _MESSAGE = tbl.Rows.Count + " Record(s) found.";
            }
            else if (db.RowEffect == 0)
            {
                _SUCCESS = true;
                _MESSAGE = "No Record(s) found.";
            }
            else
            {
                _SUCCESS = false;
                _MESSAGE = "Get data error.";
            }
        }
        catch (Exception ex)
        {
            tbl = new DataTable();
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [GetPolicyDetail(string policyId)] in class [da_micro_group_policy], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return tbl;
    }


    /// <summary>
    /// Get number of policies effective in the year by customer id type and id number
    /// </summary>
    /// <param name="idType"></param>
    /// <param name="idNumber"></param>
    /// <param name="year"></param>
    /// <returns>[0]: is policy counted, [1]: is total sum assured</returns>
    public static double[] GetPolicyEffective(int idType, string idNumber, int year)
    {
        double[] result = new double[] { 0, 0 };
        DataTable tbl = new DataTable();
        try
        {
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_COUNT_POLICY_EFFECTIVE_IN_Y_BY_CUST_INFO", new string[,] 
            {
            {"@id_type",idType+""},{"@id_number", idNumber},{"@year",year+""}
           
            }, "da_micro_group_policy=>GetPolicyEffective(int idType, string idNumber, int year)");
            if (!_SUCCESS)
            {
                _MESSAGE = db.Message;
            }
            if (db.RowEffect > 0)
            {
                var r = tbl.Rows[0];
                _SUCCESS = true;
                _MESSAGE = tbl.Rows.Count + " Record(s) found.";
                result = new double[] { Convert.ToDouble(r["POLICY_COUNT"].ToString()), Convert.ToDouble(r["TOTAL_SUM_ASSURE"].ToString()) };
            }
            else if (db.RowEffect == 0)
            {
                _SUCCESS = true;
                _MESSAGE = "No Record(s) found.";
            }
            else
            {
                _SUCCESS = false;
                _MESSAGE = "Get data error.";
            }
        }
        catch (Exception ex)
        {
            result = new double[] { 0, 0 };
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [GetPolicyEffective(int idType, string idNumber, int year)] in class [da_micro_group_policy], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return result;
    }
    /// <summary>
    /// Get number of policies effective in the year by customer id 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public static double[] GetPolicyEffective(string customerId, int year)
    {
        double[] result = new double[] { 0, 0 };
        DataTable tbl = new DataTable();
        try
        {
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_COUNT_POLICY_EFFECTIVE_IN_Y_BY_CUST_ID", new string[,] 
            {
            {"@CUSTOMER_ID",customerId},{"@year",year+""}
           
            }, "da_micro_group_policy=>GetPolicyEffective(string customerId, int year)");
            if (!_SUCCESS)
            {
                _MESSAGE = db.Message;
            }
            if (db.RowEffect > 0)
            {
                var r = tbl.Rows[0];
                _SUCCESS = true;
                _MESSAGE = tbl.Rows.Count + " Record(s) found.";
                result = new double[] { Convert.ToDouble(r["POLICY_COUNT"].ToString()), Convert.ToDouble(r["TOTAL_SUM_ASSURE"].ToString()) };
            }
            else if (db.RowEffect == 0)
            {
                _SUCCESS = true;
                _MESSAGE = "No Record(s) found.";
            }
            else
            {
                _SUCCESS = false;
                _MESSAGE = "Get data error.";
            }
        }
        catch (Exception ex)
        {
            result = new double[] { 0, 0 };
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [GetPolicyEffective(string customerId, int year)] in class [da_micro_group_policy], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return result;
    }


    public static bl_micro_group_policy.LastSequence GetLastSEQ(string groupCode)
    {
        bl_micro_group_policy.LastSequence returnObj = new bl_micro_group_policy.LastSequence();
        try
        {
            Int32 seq = 0;
            string[] polNo = new string[] { };
            DB db = new DB();
            string prefix = "";

            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_GET_LAST_SEQ", new string[,] {
        {"@GROUP_CODE",groupCode}
        }, "bl_micro_group_policy => GetLastSEQ()");
            if (db.RowEffect == -1)//error
            {
                returnObj = new bl_micro_group_policy.LastSequence() { LastSequenceNumber = -1, Prefix = "" };
            }
            else
            {
                if (tbl.Rows.Count > 0)
                {
                    seq = Convert.ToInt32(tbl.Rows[0]["seq"].ToString());

                    polNo = tbl.Rows[0]["POLICY_NUMBER"].ToString().Split('-');
                    if (polNo.Length > 0)
                    {
                        prefix = polNo[polNo.Length - 1].ToString().Substring(0, 2);
                        returnObj = new bl_micro_group_policy.LastSequence() { LastSequenceNumber = seq, Prefix = prefix };
                    }
                }
                else
                {
                    returnObj = new bl_micro_group_policy.LastSequence() { LastSequenceNumber = 0, Prefix = "" };
                }
            }
        }
        catch (Exception ex)
        {
            returnObj = new bl_micro_group_policy.LastSequence() { LastSequenceNumber = -1, Prefix = "" };
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            Log.AddExceptionToLog("Error function [GetLastSEQ(string groupCode)] in class [da_micro_group_policy], Detail:" + ex.Message + "=>" + ex.StackTrace);

        }
        return returnObj;
    }
}