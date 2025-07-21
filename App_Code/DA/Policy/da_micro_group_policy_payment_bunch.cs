using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_group_policy_payment_bunch
/// </summary>
public class da_micro_group_policy_payment_bunch
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }

    private static DB db = new DB();
    public da_micro_group_policy_payment_bunch()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public class summary
    {
        public summary()
        {

        }
        public static bool Save(bl_micro_group_policy_payment_bunch.summary bunch)
        {
            bool result = false;
            try
            {
                var a = bunch;
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_SUMMARY_INSERT", new string[,] {
                    {"@bunch_id", a.BunchId},
            {"@bunch_number", a.BunchNumber+""},
            {"@group_master_code",a.GroupMasterCode},
            {"@amount", a.Amount+""}, 
            {"@discount", a.DisountAmount+""},
            {"@total_amount",a.TotalAmount+""},
            {"@return_amount", a.ReturnAmount+""},
           {"@status", a.Status+""},{"@NUMBER_POLICY", a.NumberPolicy+""},{"@report_date", a.ReportDate+""},{"@payment_type", a.PaymentType},
            {"@CREATED_BY", a.CreatedBy},
            {"@CREATED_ON", a.CreatedOn+""},
            {"@REMARKS",a.Remarks }
            }, "da_micro_group_policy_payment_bunch=>summary=>Save(bl_micro_group_policy_payment_bunch.summary bunch)");

                if (db.RowEffect == -1)
                {

                }
                if (result)
                {
                    _MESSAGE = "Success";
                    _SUCCESS = true;
                }
                else
                {
                    _MESSAGE = db.Message;
                    _SUCCESS = false;
                }

            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                result = false;
                Log.AddExceptionToLog("Error function [Save(bl_micro_group_policy_payment_bunch.summary bunch)] in class [da_micro_group_policy_payment_bunch => summary], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }
        /// <summary>
        /// Update Policy Payment Bunch Summary by Bunch Number
        /// </summary>
        /// <param name="bunch"></param>
        /// <returns></returns>
        public static bool Update(bl_micro_group_policy_payment_bunch.summary bunch)
        {
            bool result = false;
            try
            {
                var a = bunch;
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_SUMMARY_UPDATE", new string[,] {
           
            {"@bunch_number", a.BunchNumber+""},
            {"@amount", a.Amount+""}, 
            {"@discount", a.DisountAmount+""},
            {"@total_amount",a.TotalAmount+""},
            {"@return_amount", a.ReturnAmount+""},
           {"@status", a.Status+""},{"@NUMBER_POLICY", a.NumberPolicy+""},{"@report_date", a.ReportDate+""},
            {"@updated_by", a.UpdatedBy},
            {"@updated_on", a.UpdatedOn+""},
            {"@REMARKS",a.Remarks }
            }, "da_micro_group_policy_payment_bunch => summary=>Update(bl_micro_group_policy_payment_bunch.summary bunch)");

                if (db.RowEffect == -1)
                {

                }
                if (result)
                {
                    _MESSAGE = "Success";
                    _SUCCESS = true;
                }
                else
                {
                    _MESSAGE = db.Message;
                    _SUCCESS = false;
                }

            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                result = false;
                Log.AddExceptionToLog("Error function [Update(bl_micro_group_policy_payment_bunch.summary bunch)] in class [da_micro_group_policy_payment_bunch => summary], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bunchId"></param>
        /// <param name="status">[0=Pending, 1=Invoiced, 2=Paid]</param>
        /// <param name="remarks"></param>
        /// <param name="updatedBy"></param>
        /// <param name="updatedOn"></param>
        /// <returns></returns>
        public static bool UpdateStatus(string bunchId, Int32 status, string remarks, string updatedBy, DateTime updatedOn)
        {
            bool result = false;
            try
            {

                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_SUMMARY_UPDATE_STATUS", new string[,] {
           
            {"@bunch_id", bunchId},
            {"@status", status+""}, 
            {"@updated_by", updatedBy},
            {"@updated_on", updatedOn+""},
            {"@REMARKS",remarks }
            }, "da_micro_group_policy_payment_bunch => summary=>UpdateStatus(string bunchId, Int32 status, string remarks, string updatedBy, DateTime updatedOn)");

                if (db.RowEffect == -1)
                {

                }
                if (result)
                {
                    _MESSAGE = "Success";
                    _SUCCESS = true;
                }
                else
                {
                    _MESSAGE = db.Message;
                    _SUCCESS = false;
                }

            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                result = false;
                Log.AddExceptionToLog("Error function [UpdateStatus(string bunchId, Int32 status, string remarks, string updatedBy, DateTime updatedOn)] in class [da_micro_group_policy_payment_bunch => summary], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }
    }
   

    public class detail
    {
        public detail()
        {

        }
        public static bool Save(bl_micro_group_policy_payment_bunch.detail detail)
        {
            bool result = false;
            try
            {
                var a = detail;
                result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_PAYMENT_BUNCH_DETAIL_INSERT", new string[,] {
                    {"@bunch_detail_id", a.BunchDetailId},
            {"@bunch_id", a.BunchId},
            {"@policy_payment_id", a.PolicyPaymentId}
          
            }, "da_micro_group_policy_payment_bunch=>detail=>Save(bl_micro_group_policy_payment_bunch.detail detail)");

                if (db.RowEffect == -1)
                {

                }
                if (result)
                {
                    _MESSAGE = "Success";
                    _SUCCESS = true;
                }
                else
                {
                    _MESSAGE = db.Message;
                    _SUCCESS = false;
                }

            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                result = false;
                Log.AddExceptionToLog("Error function [Save(bl_micro_group_policy_payment_bunch.detail detail)] in class [da_micro_group_policy_payment_bunch => detail], detail: " + ex.Message + "==>" + ex.StackTrace);
            }
            return result;
        }
    }

    public class SummaryReport
    {
        public SummaryReport()
        {

        }

        public static List<bl_micro_group_policy_payment_bunch.SummaryReport> GetSummary()
        {
            List<bl_micro_group_policy_payment_bunch.SummaryReport> ls = new List<bl_micro_group_policy_payment_bunch.SummaryReport>();
            try
            {
                DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_BUNCH_PAYMENT_SUMMARRY", new string[,] { },

                    "da_micro_group_policy_payment_bunch => SummaryReport =>GetSummary()");

                foreach (DataRow r in tbl.Rows)
                {
                    ls.Add(new bl_micro_group_policy_payment_bunch.SummaryReport()
                    {
                        ChannelId = r["channel_id"].ToString(),
                        ChannelItemId = r["channel_item_id"].ToString(),
                        ChannelLocationId = r["channel_location_id"].ToString(),
                        GroupMasterCode=r["group_master_code"].ToString(),
                        BunchId = r["bunch_id"].ToString(),
                        ChannelName = r["channel_name"].ToString(),
                        BunchNumber = Convert.ToInt32(r["bunch_number"].ToString()),
                        Amount = Convert.ToDouble(r["amount"].ToString()),
                        Discount = Convert.ToDouble(r["discount"].ToString()),
                        TotalAmount = Convert.ToDouble(r["total_amount"].ToString()),
                        ReturnAmount = Convert.ToDouble(r["return_amount"].ToString()),
                        Remarks = r["remarks"].ToString(),
                        Status = Convert.ToInt32(r["status"].ToString()),
                        CreatedBy = r["created_by"].ToString(),
                        CreatedOn = Convert.ToDateTime(r["created_on"].ToString())
                    });
                }
                _SUCCESS = true;
            }
            catch (Exception ex)
            {

                _SUCCESS = false;
                _MESSAGE = ex.Message;
                ls = new List<bl_micro_group_policy_payment_bunch.SummaryReport>();
                Log.AddExceptionToLog("Error function [List<bl_micro_group_policy_payment_bunch.SummaryReport> GetSummary()] in class [da_micro_group_policy_payment_bunch => SummaryReport], detail: " + ex.Message + "==>" + ex.StackTrace);

            }
            return ls;
        }
        /// <summary>
        /// Get Summary report by filtering, channel id, channel item id and status
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="channelItemId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<bl_micro_group_policy_payment_bunch.SummaryReport> GetSummary(string channelId, string channelItemId, int status)
        {
            List<bl_micro_group_policy_payment_bunch.SummaryReport> ls = new List<bl_micro_group_policy_payment_bunch.SummaryReport>();
            try
            {
                foreach(bl_micro_group_policy_payment_bunch.SummaryReport sm in GetSummary().Where(_ => _.ChannelId == channelId && _.ChannelItemId == channelItemId && _.Status == status))
                {
                    ls.Add(sm);
                }
            }
            catch (Exception ex)
            {
                _SUCCESS = false;
                _MESSAGE = ex.Message;
                ls = new List<bl_micro_group_policy_payment_bunch.SummaryReport>();
                Log.AddExceptionToLog("Error function [List<bl_micro_group_policy_payment_bunch.SummaryReport>GetSummary(string channelId, string channelItemId, int status)] in class [da_micro_group_policy_payment_bunch => SummaryReport], detail: " + ex.Message + "==>" + ex.StackTrace);

            }
            return ls;
        }
    }
}