using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_policy_prem_lot
/// </summary>
public class da_policy_prem_lot
{
	public da_policy_prem_lot()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    
    //Get policy prem year and lot
    /// <summary>
    /// Get policy premium lot by policy id
    /// </summary>
    /// <returns></returns>
    public static bl_policy_prem_lot Get_Policy_Prem_Lot(string policy_id)
    {
        bl_policy_prem_lot prem_lot = new bl_policy_prem_lot();
        try
        {
            DataTable my_data_table = new DataTable();
            my_data_table = DataSetGenerator.Get_Data_Soure("SP_GET_POLICY_PREM_LOT", new string[,] { { "@Policy_ID", policy_id } });
            
            foreach (DataRow row in my_data_table.Rows)
            {
                prem_lot.Policy_ID = row["policy_id"].ToString();
                prem_lot.Prem_Year = Convert.ToInt32(row["prem_year"].ToString());
                prem_lot.Prem_Lot = Convert.ToInt32(row["prem_lot"].ToString());
                prem_lot.Pay_Mod = Convert.ToInt32(row["pay_mode"].ToString());
                prem_lot.Due_Date = Convert.ToDateTime(row["due_date"].ToString());
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Get_Policy_Prem_Lot] in class [da_policy_prem_lot], Detail: " + ex.Message);
        }
        

        return prem_lot;
    }
}