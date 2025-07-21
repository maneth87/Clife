using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


/// <summary>
/// Summary description for da_sub_policy
/// </summary>
public class da_sub_policy
{
	public da_sub_policy()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// Get all sub policies premium by policy id
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static List<bl_sub_policy_premium> GetSubPolicyPremiumList(string policy_id)
    {
        List<bl_sub_policy_premium> preList = new List<bl_sub_policy_premium>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_SUB_POLICY_PREMIUM_BY_POLICY_ID", new string[,] { { "@POLICY_ID", policy_id } }, "[da_sub_policy ==> GetSubPolicyPremiumList(string policy_id)]");
            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    //Add sub policies premium in list
                    preList.Add(new bl_sub_policy_premium()
                    {
                        PolicyID = row["policy_id"].ToString(),
                        PolicyNumber = row["policy_number"].ToString(),
                        Premium = Convert.ToDouble(row["premium"].ToString()),
                        OriginalAmount = Convert.ToDouble(row["original_amount"].ToString()),
                        EmPremium = Convert.ToDouble(row["em_premium"].ToString()),
                        EmAmount = Convert.ToDouble(row["em_amount"].ToString()),
                        DiscountAmount = Convert.ToDouble(row["discount_amount"].ToString()),
                        SumInsure = Convert.ToDouble(row["sum_insure"].ToString())

                    });
                }
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error: class [da_sub_policy], function [GetSubPolicyPremiumList(string policy_id)]. Details: " + ex.Message);
        }
        return preList;
    }

    /// <summary>
    /// This function return sub policy premium object, but premium, em, original_amount and SI, and discount show as total
    /// </summary>
    /// <param name="policy_id"></param>
    /// <returns></returns>
    public static bl_sub_policy_premium GetSubPoliciesTotalPremium(string policy_id)
    {
        bl_sub_policy_premium objSub = new bl_sub_policy_premium();
        try
        {
            List<bl_sub_policy_premium> listSubPrem = GetSubPolicyPremiumList(policy_id);
            if (listSubPrem.Count > 0)
            {
                foreach (bl_sub_policy_premium obj in listSubPrem)
                {
                    objSub.Premium += obj.Premium;
                    objSub.OriginalAmount += obj.OriginalAmount;
                    objSub.EmAmount += obj.EmAmount;
                    objSub.EmPremium += obj.EmPremium;
                    objSub.DiscountAmount += obj.DiscountAmount;
                    objSub.SumInsure += obj.SumInsure;
                    
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error in class [da_sub_policy], function [GetSubPoliciesTotalPremium(string policy_id)], Detail: " + ex.Message);
        }
        return objSub;
    }
}