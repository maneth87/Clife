using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_reduced_sum_assured
/// </summary>
public class da_reduced_sum_assured
{
	public da_reduced_sum_assured()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// Get Rate by coverage year
    /// </summary>
    /// <param name="coverageYear"></param>
    /// <returns></returns>
    public static List<bl_reduced_sum_assured> GetTableRate(string productId, int coverageYear)
    {
        List<bl_reduced_sum_assured> arrList = new List<bl_reduced_sum_assured>();
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_REDUCED_SUM_ASSURED_RATE", new string[,] { { "@COVERAGE_YEAR", coverageYear + "" }, {"@PRODUCT_ID", productId } });
            foreach (DataRow row in tbl.Rows)
            {
                arrList.Add(new bl_reduced_sum_assured() { 
                     CoverageYear = Convert.ToInt32(row["coverageyear"].ToString()), 
                     Year = Convert.ToInt32(row["year"].ToString()),
                     Month=Convert.ToInt32(row["month"].ToString()),
                     ProductId = row["productid"].ToString(),
                     Rate=Convert.ToDouble(row["rate"].ToString())
                });
            }
        }
        catch (Exception ex)
        { 
            Log.AddExceptionToLog("Error function [GetTableRate] in class [da_reduced_sum_assured], Detail: " + ex.InnerException + "==>" + ex.Message + "==>" + ex.StackTrace);
        }
        return arrList;
    }
}