using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_micro_group_policy_beneficiary
/// </summary>
public class da_micro_group_policy_beneficiary
{
    private static bool _SUCCESS = false;
    private static string _MESSAGE = "";

    public static bool SUCCESS { get { return _SUCCESS; } }
    public static string MESSAGE { get { return _MESSAGE; } }
    private static DB db = new DB();
	public da_micro_group_policy_beneficiary()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bool Save(bl_micro_policy_beneficiary BENEFICIARY)
    {
        bool result = false;
        try
        {
            
            //DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_BENEFICIARY_INSERT", new string[,] {
            {"@BENEFICIARY_ID", BENEFICIARY.ID},
            {"@POLICY_ID", BENEFICIARY.POLICY_ID}, 
            {"@FULL_NAME",BENEFICIARY.FULL_NAME}, 
            {"@gender",  BENEFICIARY.Gender},
            {"@BIRTH_DATE", BENEFICIARY.BirthDate+""},
            {"@AGE", BENEFICIARY.AGE}, 
            {"@RELATION", BENEFICIARY.RELATION}, 
            {"@PERCENTAGE_OF_SHARE", BENEFICIARY.PERCENTAGE_OF_SHARE+""}, 
            {"@ADDRESS", BENEFICIARY.ADDRESS}, 
            {"@CREATED_BY", BENEFICIARY.CREATED_BY}, 
            {"@CREATED_ON", BENEFICIARY.CREATED_ON+""}, 
            {"@REMARKS", BENEFICIARY.REMARKS}
            }, "da_micro_group_policy_benificiary=>Save(bl_micro_policy_beneficiary BENEFICIARY)");

            if (db.RowEffect == -1) //error
            {
                _MESSAGE = db.Message;
            }
            else
            {
                _MESSAGE = "Success";
            }
            _SUCCESS = result;
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            result = false;
            Log.AddExceptionToLog("Error function [Save(bl_micro_policy_beneficiary BENEFICIARY)] in class [da_micro_group_policy_benificiary], detail: " + ex.Message );

        }

        return result;
    }

    public static bool Update(bl_micro_policy_beneficiary BENEFICIARY, string BeneficiaryId)
    {
        bool result = false;
        try
        {

            //DB db = new DB();
            result = db.Execute(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_BENEFICIARY_UPDATE_BY_BEN_ID", new string[,] {
            {"@BENEFICIARY_ID",BeneficiaryId},
            {"@FULL_NAME",BENEFICIARY.FULL_NAME}, 
            {"@gender",  BENEFICIARY.Gender},
            {"@BIRTH_DATE", BENEFICIARY.BirthDate+""},
            {"@AGE", BENEFICIARY.AGE}, 
            {"@RELATION", BENEFICIARY.RELATION}, 
            {"@PERCENTAGE_OF_SHARE", BENEFICIARY.PERCENTAGE_OF_SHARE+""}, 
            {"@ADDRESS", BENEFICIARY.ADDRESS}, 
            {"@UPDATED_BY", BENEFICIARY.UPDATED_BY}, 
            {"@UPDATED_ON", BENEFICIARY.UPDATED_ON+""}, 
            {"@REMARKS", BENEFICIARY.REMARKS}
            }, "da_micro_group_policy_benificiary=>Update(bl_micro_policy_beneficiary BENEFICIARY, string BeneficiaryId)");

            if (db.RowEffect == -1) //error
            {
                _MESSAGE = db.Message;
            }
            else
            {
                _MESSAGE = "Success";
            }
            _SUCCESS = result;
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            result = false;
            Log.AddExceptionToLog("Error function [Update(bl_micro_policy_beneficiary BENEFICIARY, string BeneficiaryId)] in class [da_micro_group_policy_benificiary], detail: " + ex.Message);

        }

        return result;
    }


    /// <summary>
    /// Get beneficiary object by policy id
    /// </summary>
    /// <param name="policyId"></param>
    /// <returns></returns>
    public static bl_micro_policy_beneficiary Get(string policyId)
    {
        bl_micro_policy_beneficiary ben = new bl_micro_policy_beneficiary();
        try {
            DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_GROUP_MICRO_POLICY_BENEFICIARY_GET_BY_POL_ID", new string[,] { 
                {"@POLICY_ID", policyId}
            }, "da_micro_group_policy_benificiary=>Get(string policyId)");
            if (db.RowEffect < 0)
            {
                ben = null;
                _MESSAGE = "Get Beneficiary is getting error, please contact your system administrator.";
                _SUCCESS = false;
            }
            else if (db.RowEffect == 0)
            {
                ben = null;
                _MESSAGE = "No record found.";
                _SUCCESS = true;
            }
            else
            {
                _SUCCESS = true;
                _MESSAGE = "Get beneficiary successfully.";
                var r = tbl.Rows[0];
                ben = new bl_micro_policy_beneficiary() { 
                 ID=r["beneficiary_id"].ToString(),
                  POLICY_ID=r["policy_id"].ToString(),
                  FULL_NAME=r["full_name"].ToString(),
                  Gender=r["gender"].ToString(),
                   RELATION=r["relation"].ToString(),
                   PERCENTAGE_OF_SHARE=Convert.ToDouble(r["percentage_shared"].ToString()),
                   ADDRESS=r["address"].ToString(),
                   REMARKS=r["remarks"].ToString()

                };
            }
        }
        catch (Exception ex)
        {
            _SUCCESS = false;
            _MESSAGE = ex.Message;
            ben = null;
            Log.AddExceptionToLog("Error function [Get(string policyId)] in class [da_micro_group_policy_benificiary], detail: " + ex.Message );

        }


        return ben;
    }
}