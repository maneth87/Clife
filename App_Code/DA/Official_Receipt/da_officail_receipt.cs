using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_officail_receipt
/// </summary>
public class da_officail_receipt
{
    #region Properties
    public string Official_Receipt_Rider_ID { get; set; }
    public string Rider_ID { get; set; }
    #endregion
    public da_officail_receipt()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public da_officail_receipt(string official_receipt_rider_id, string rider_id)
    {
        this.Official_Receipt_Rider_ID = official_receipt_rider_id;
        this.Rider_ID = rider_id;
    }
    public static List<da_officail_receipt> GetOfficialReceiptRider(string rider_id)
    {
        List<da_officail_receipt> myList = new List<da_officail_receipt>();
        try
        {
            da_officail_receipt rec = new da_officail_receipt();
            DataTable tbl = DataSetGenerator.Get_Data_Soure("SP_GET_OFFICIAL_RECEIPT_RIDER_BY_RIDER_ID", new string[,] { { "@Rider_ID", rider_id } });
            foreach (DataRow row in tbl.Rows)
            {
                rec.Official_Receipt_Rider_ID = row["Official_Receipt_Rider_ID"].ToString().Trim();
                rec.Rider_ID = row["Rider_ID"].ToString().Trim();
                myList.Add(rec);
            }
            
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetOfficialReceiptRider] in class [da_officail_receipt], Detail: " + ex.Message);
        }
        return myList;
    }
    public static DateTime GetEffectiveDate(string policy_id)
    {
        DateTime effective_Date = DateTime.Now;

        try
        {
           
            string sql = @"select Effective_Date from Ct_Policy where Policy_ID=@Policy_ID  ";
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), sql, new string[,] { { "@Policy_ID", policy_id } });
            if (tbl.Rows.Count > 0)
            {
                effective_Date = Convert.ToDateTime( tbl.Rows[0]["Effective_Date"].ToString());
            }


        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [GetEffectiveDate] in class [da_official_receipt]. Details: " + ex.Message);
        }

        return effective_Date;
    }
    public static bool Insert_Official_Receipt(bl_official_receipt official_receipt)
    {
        bool result = false;

        try
        {
          
            string sql = @"Insert into Ct_Official_Receipt 
                                (Official_Receipt_ID,Policy_ID,Customer_ID,Receipt_No,Policy_Type,Method_Payment,Amount,Created_By,Created_On,Created_Note,Interest_Amount,Entry_Date)
                                Values(@Official_Receipt_ID,@Policy_ID,@Customer_ID,@Receipt_No,@Policy_Type,@Method_Payment,@Amount,@Created_By,@Created_On,@Created_Note,@Interest_Amount,@Entry_Date)";


           result=Helper.ExecuteCommand(AppConfiguration.GetConnectionString(),sql,new string[,]{

            {"@Official_Receipt_ID", official_receipt.Official_Receipt_ID},
            {"@Policy_ID", official_receipt.Policy_ID},
            {"@Customer_ID", official_receipt.Customer_ID},
            {"@Receipt_No", official_receipt.Receipt_No},
            {"@Policy_Type", official_receipt.Policy_Type+""},
            {"@Method_Payment", official_receipt.Method_Payment+""},
            {"@Amount", official_receipt.Amount+""},
            {"@Created_On", official_receipt.Created_On+""},
            {"@Created_By", official_receipt.Created_By},
            {"@Created_Note", official_receipt.Created_Note},
            {"@Interest_Amount", official_receipt.Interest_Amount+""},
            {"@Entry_Date", official_receipt.Entry_Date+""}

        },"function [Insert_Official_Receipt(bl_official_receipt official_receipt)] class [da_official_receipt]");

        }
        catch (Exception ex)
        {
            //Add error to log 
            Log.AddExceptionToLog("Error in function [Insert_Official_Receipt] in class [da_Official_Receipt]. Details: " + ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Get Group Micro Official Receipt by Official Receipt Id
    /// </summary>
    /// <param name="officialReceiptId"></param>
    /// <returns></returns>
    public static DataTable GetGroupMicroOfficialReceipt(string officialReceiptId)
    {
        DB db = new DB();
        DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_OFFICIAL_RECEIPT_GET", new string[,] {{"@official_receipt_id",officialReceiptId} }, "Report => GroupMicro =>GetGroupMicroOfficialReceipt(string officialReceiptId)");
        return tbl;
    }
    /// <summary>
    /// Get Group Micro Official Receipt List by Pay Date range
    /// </summary>
    /// <param name="payDateF">From Pay Date</param>
    /// <param name="payDateT">To Pay Date</param>
    /// <returns></returns>
    public static DataTable GetGroupMicroOfficialReceiptList(DateTime payDateF, DateTime payDateT)
    {
        DB db = new DB();
        DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_OFFICIAL_RECEIPT_GET_LIST", new string[,] { 
        { "@pay_date_f", payDateF+"" },{"@pay_date_t",payDateT+""}
        }, "Report => GroupMicro =>GetGroupMicroOfficialReceipt(string officialReceiptId)");
        return tbl;
    }

    /// <summary>
    /// Get official receipt report by pay date ranges
    /// </summary>
    /// <param name="payDateF"> from pay date</param>
    /// <param name="payDateT">to pay date</param>
    /// <returns></returns>
    public static DataTable GetGroupMicroOfficialReceiptReport(DateTime payDateF, DateTime payDateT)
    {
        DB db = new DB();
        DataTable tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_GROUP_MICRO_OFFICIAL_RECEIPT_REPORT", new string[,] { 
        { "@pay_date_f", payDateF+"" },{"@pay_date_t",payDateT+""}
        }, "Report => GroupMicro =>GetGroupMicroOfficialReceiptReport(string officialReceiptId)");
        return tbl;
    }
}