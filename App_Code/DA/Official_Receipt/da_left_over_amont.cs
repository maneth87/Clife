using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for da_left_over_amont
/// </summary>
public class da_left_over_amont
{
	public da_left_over_amont()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bool SaverOverAmount(bl_left_over_amount obj)
    {
        bool result = false;

        try
        {
            result = new DB().Execute(AppConfiguration.GetConnectionString(), "SP_Insert_Left_Over", new string[,] {
            {"@Left_Over_Amount_ID", obj.Left_Over_Amount_ID},
            {"@Policy_ID", obj.Policy_ID},
            {"@Received_Amount", obj.Received_Amount+""},
            {"@Prem_Amount", obj.Prem_Amount+""},
            {"@Prem_Amount_Paid", obj.Prem_Amount_Paid+""},
             {"@Prem_Amount_Left_Over", obj.Prem_Amount_Left_Over+""},
             {"@Left_Over_Substract", obj.Left_Over_Substract+""},
             {"@Received_Date", obj.Received_Date+""},
             {"@Created_By", obj.Created_By},
            {"@Created_On", obj.Created_On+""},
             {"@Status_Used", obj.Status_Used+""},
             {"@Official_Receipt_ID", obj.Official_Receipt_ID}
            }, "da_left_over_amount=>SaverOverAmount(bl_left_over_amount obj)");
         

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [SaverOverAmount(bl_left_over_amount obj)] in class [da_left_over_amount]. Details: " + ex.Message);
        }

        return result;
    }
}