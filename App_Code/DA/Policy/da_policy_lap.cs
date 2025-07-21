using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_policy_lap
/// </summary>
public class da_policy_lap
{
    public da_policy_lap()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current_date"></param>
    /// <returns></returns>
    public static List<bl_policy_lap> GetPolicyLapList(DateTime current_date)
    {
        List<bl_policy_lap> listLap = new List<bl_policy_lap>();
        try
        {
            bl_policy_lap policyLap;
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "[SP_GET_CUSTOMER_BEFORE_DUE_NOTIFICATION]", new string[,] { },

                   "Function [GetPolicyLapList()] class [da_policy_lap]");
            foreach (DataRow row in tbl.Rows)
            {
                policyLap = new bl_policy_lap();
                policyLap.EffectiveDate = Convert.ToDateTime(row["effective_date"].ToString()).Date;
                policyLap.DueDate = Convert.ToDateTime(row["due_date"].ToString()).Date;
                policyLap.PayMode = Convert.ToInt32(row["pay_mode"].ToString());
                policyLap.CurrentDate = current_date;
                if (policyLap.CalculatedGracePeriod > policyLap.GracePeriod) //calculated grace period>30 days
                {
                    policyLap.PolicyID = row["policy_id"].ToString();
                    policyLap.CustomerID = row["customer_id"].ToString();
                    policyLap.PolicyNumber = row["policy_number"].ToString();
                    policyLap.CustomerNameEn = row["en_name"].ToString();
                    policyLap.CustomerNameKh = row["kh_name"].ToString();
                    policyLap.CustomerGender = row["gender"].ToString();
                    policyLap.CustomerDOB = Convert.ToDateTime(row["birth_date"].ToString());
                    policyLap.CustomerPhoneNumber = row["phone"].ToString();
                    policyLap.CustomerEmail = row["email"].ToString();
                    policyLap.ProductID = row["product_id"].ToString();
                    listLap.Add(policyLap);
                }
            }
        }
        catch (Exception ex)
        {
            listLap = new List<bl_policy_lap>();
            Log.AddExceptionToLog("Error function  [GetPolicyLapList()] in class [da_policy_lap], detail:" + ex.Message);
        }
        return listLap;
    }

    public static bool SavePolicyLap(bl_policy_lap polLap)
    {
        bool result = false;
        try
        {
            result = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_POLICY_LAP", new string[,] { 
            {"@POLICY_ID", polLap.PolicyID}, 
            {"@POLICY_NUMBER",polLap.PolicyNumber},
            {"@CUSTOMER_ID",polLap.CustomerID},
            {"@CUSTOMER_NAME_EN",polLap.CustomerNameEn},
            {"@CUSTOMER_NAME_KH",polLap.CustomerNameKh},
            {"@GENDER",polLap.CustomerGender},
            {"@DOB",polLap.CustomerDOB+""},
            {"@CUSTOMER_PHONE_NUMBER",polLap.CustomerPhoneNumber}, 
	        {"@CUSTOMER_EMAIL",polLap.CustomerEmail},
            {"@PRODUCT_ID",polLap.ProductID},
            {"@EFFECTIVE_DATE",polLap.EffectiveDate+""},
            {"@DUE_DATE",polLap.DueDate+""},
            {"@NEXT_DUE_DATE",polLap.NextDueDate+""},
            {"@GRACE_PERIOD",polLap.GracePeriod+""},
            {"@CALCULATED_GRACE_PERIOD",polLap.CalculatedGracePeriod+""},
            {"@POLICY_STATUS",polLap.PolicyStatus},
            {"@CURRENT_DATE",polLap.CurrentDate+""},
            {"@CREATED_ON",polLap.CreatedOn+""},
            {"@CREATED_BY", polLap.CreatedBy},
            {"@REMARKS", polLap.Remarks}
            }, "da_policy_lap => SavePolicyLap(bl_policy_lap polLap)");
        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [SavePolicyLap(bl_policy_lap)] in class [da_policy_lap], detail:" + ex.Message);
        }
        return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current_date"></param>
    /// <returns></returns>
    public static List<bl_policy_lap> UpdatePolicyToLap(DateTime current_date, string updated_by, DateTime updated_on, string updated_note)
    {
        List<bl_policy_lap> updatedList = new List<bl_policy_lap>();
        bl_policy_status polStatus;
        try
        {
            List<bl_policy_lap> getLapPolicyList = da_policy_lap.GetPolicyLapList(current_date);
            string saveLog = "";
            foreach (bl_policy_lap lap in getLapPolicyList)
            {

                polStatus = new bl_policy_status() { Policy_ID = lap.PolicyID, Policy_Status_Type_ID = "LAP", Updated_By = updated_by, Updated_On = updated_on, Updated_Note = updated_note };
                if (da_policy.UpdatePolicyStatus(polStatus))
                {
                    lap.CreatedBy = updated_by;
                    lap.CreatedOn = DateTime.Now;
                    lap.Remarks = updated_note;
                    lap.PolicyStatus = polStatus.Policy_Status_Type_ID;
                    updatedList.Add(lap);
                    da_policy_lap.SavePolicyLap(lap);
                    saveLog += "System had been updated policy number [" + lap.PolicyNumber + "] status to [LAP] successfully." + System.Environment.NewLine;
                }
                else
                {
                    saveLog += "System had been updated policy number [" + lap.PolicyNumber + "] status to [LAP] fail." + System.Environment.NewLine;
                }
            }
            saveLog = saveLog == "" ? "There is no policy lap." : saveLog;
            Log.CreateLog("SYS_UPDATE_POLICY_STATUS_TO_LAP", saveLog);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [UpdatePolicyToLap(DateTime current_date)] in class [da_policy_lap], detail:" + ex.Message);

        }
        return updatedList;
    }

}