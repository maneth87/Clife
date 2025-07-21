using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_policy_printed_records
/// </summary>
public class da_inform_letter_printed_records
{
    public da_inform_letter_printed_records()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bool InsertRecord(bl_inform_letter_printed_records printed)
    {
        bool status = false;

        try
        {
            status = Helper.ExecuteProcedure(AppConfiguration.GetConnectionString(), "SP_INSERT_INFORM_LETTER_PRINTED_RECORD", new string[,] { { "@Policy_ID", printed.Policy_ID }, { "@Printed_Date", printed.Printed_Date + "" }, { "@Printed_By", printed.Printed_By }, { "@Report_Type", printed.Report_Type } }, "da_policy_printed_records => InsertRecord");
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [InsertRecord] in class [da_inform_letter_printed_records], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }
    public static List<bl_inform_letter_printed_records> GetPrintedInformLetter(string[] policy_id, string report_type)
    {
        List<bl_inform_letter_printed_records> listInfo = new List<bl_inform_letter_printed_records>();

        try
        {
            DataTable tbl = new DataTable();
            string str_policy_id = "";
            for (int i = 0; i < policy_id.Length; i++)
            {
                str_policy_id = str_policy_id + policy_id[i].ToString()+ ",";
            }
            str_policy_id = ( str_policy_id.Substring(0, str_policy_id.Length - 1));
           
            tbl = DataSetGenerator.Get_Data_Soure("SP_GET_INFORM_LETTER_BY_POLICY_ID", new string[,] { { "@Policy_ID",  str_policy_id }, {"@Report_Type", report_type} });
            foreach(DataRow row in tbl.Rows)
            {
                listInfo.Add(new bl_inform_letter_printed_records {No=Convert.ToInt32(row["no"].ToString()), Policy_ID= row["policy_id"].ToString(), Printed_By=row["printed_by"].ToString(), Printed_Date=Convert.ToDateTime(row["printed_date"].ToString()), Report_Type=row["report_type"].ToString() });
            }
           
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetPrintedInformLetter] in class [da_inform_letter_printed_records], Detail: " + ex.Message);
        }
        return listInfo;
    }
}