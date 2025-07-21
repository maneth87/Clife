using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_group_master_policy
/// </summary>
public class da_group_master_policy
{
	public da_group_master_policy()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bool InsertGroupMasterPolicy(bl_group_master_policy gMasterPolicy)
    {
        bool status = false; ;
        try
        {
           status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_GROUP_MASTER_POLICY", new string[,] { 
                {"@OPTION", "INSERT"}, 
                {"@POLICY_ID", gMasterPolicy.PolicyID}, 
                {"@GROUP_POLICY_ID", gMasterPolicy.GroupPolicyID}, 
                {"@GROUP_MASTER_ID", gMasterPolicy.GroupMasterID}, 
                {"@SEQ_MUMBER", gMasterPolicy.SeqNumber}, 
                {"@CREATED_BY", gMasterPolicy.CreatedBY}, 
                {"@CREATED_DATETIME", gMasterPolicy.CreatedDateTime +""}, 
                {"@REMARKS", gMasterPolicy.Remarks},
                {"@UPDATED_BY", gMasterPolicy.CreatedBY},
                {"@UPDATED_DATETIME", DateTime.Now.ToString()},
                {"@MAIN_POLICY_NUMBER", gMasterPolicy.MainPolicyNumber}

            }, "[da_group_master_policy >> InsertGroupMasterPolicy] ");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [InsertGroupMasterPolicy] in class [da_group_master_policy], Detail: " + ex.Message);
        }

        return status;

    }
    /// <summary>
    /// Get group master policy list by two parameters
    /// </summary>
    /// <param name="policy_id">[Skip filt input '']</param>
    /// <param name="group_master_id">[Skip filt input '']</param>
    /// <returns></returns>
    public static List<bl_group_master_policy> GetGroupMasterPolicyList(string policy_id, string group_master_id)
    {
        List<bl_group_master_policy> mPolicyList = new List<bl_group_master_policy>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_GROUP_MASTER_POLICY",new string [,]{{"@POLICY_ID", policy_id}, {"@GROUP_MASTER_ID", group_master_id}});
            foreach(DataRow row in tbl.Rows)
            {
                mPolicyList.Add(new bl_group_master_policy() { PolicyID = row["policy_id"].ToString(), GroupMasterID = row["group_master_id"].ToString(), SeqNumber = row["seq_number"].ToString() });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetGroupMasterPolicyList] in class [da_group_master_policy], Detail: " + ex.Message);
        }
        return mPolicyList;
    }
    /// <summary>
    /// Get new sequence number by group master id.
    /// </summary>
    /// <param name="gMasterGroupID">Group Master ID</param>
    /// <returns></returns>
    public static string GetNewSeqNumber(string gMasterGroupID)
    {
        string seq_number = "";
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_GROUP_MASTER_POLICY_NEW_SEQUENCE_NUMBER", new string[,] { { "@GROUP_MASTER_ID", gMasterGroupID } });
            seq_number = tbl.Rows[0]["SEQ_NUMBER"].ToString();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetNewSeqNumber] in class [da_group_master_policy], Detail: " + ex.Message);
        }
        return seq_number;
    }

    //Created_On 19-06-2020 By Meas Sun
    public static string GetSubNewSeqNumber(string seqNumber)
    {
        string sub_seq_number = "";
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_GROUP_MASTER_POLICY_SUB_NEW_SEQUENCE_NUMBER", new string[,] { { "@SEQ_NUMBER", seqNumber } });
            sub_seq_number = tbl.Rows[0]["SUB_SEQ_NUMBER"].ToString();
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetSubNewSeqNumber] in class [da_group_master_policy], Detail: " + ex.Message);
        }
        return sub_seq_number;
    }

}