using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_policy_premium_riders
/// </summary>
public class da_policy_premium_riders
{
	public da_policy_premium_riders()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static List<bl_policy_premium_riders> GetPolicyPremiumRidersList(string policy_id)
    {
        List<bl_policy_premium_riders> list = new List<bl_policy_premium_riders>();
        try
        {
            bl_policy_premium_riders riders;
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_Get_Policy_Premium_Rider_By_Policy_ID", new string[,] { { "@Policy_ID", policy_id } });
            foreach (DataRow row in tbl.Rows)
            {
                riders = new bl_policy_premium_riders();
                riders.Premium = Convert.ToDouble(row["premium"].ToString());
                riders.Original_Amount = Convert.ToDouble(row["original_amount"].ToString());
                riders.Discount_Amount = Convert.ToDouble(row["discount_amount"].ToString());
                riders.EM_Premium = Convert.ToDouble(row["em_premium"].ToString());
                riders.EM_Amount = Convert.ToDouble(row["em_amount"].ToString());

                list.Add(riders);
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPolicyPremiumRidersList] in class [da_policy_premium_riders], Detail: " + ex.Message);
        }
       

        return list;
    }
}