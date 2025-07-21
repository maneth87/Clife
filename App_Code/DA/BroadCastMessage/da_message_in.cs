using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_message_in
/// </summary>
public class da_message_in
{
	public da_message_in()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static List<bl_message_in> GetMessageIn(DateTime from_date, DateTime to_date)
    {
        List<bl_message_in> arr_message = new List<bl_message_in>();
        try
        {
            DataTable tbl;
           tbl = DataSetGenerator.Get_Data_Soure("SP_GET_MESSAGE_IN_BY_DATE_RANGE", new string[,] { { "@FROM_DATE", from_date + "" }, { "@TO_DATE", to_date + "" } });
           foreach (DataRow row in tbl.Rows)
           {
               arr_message.Add(new bl_message_in(Convert.ToInt32(row["no"].ToString()), Convert.ToDateTime(row["sendtime"].ToString()), Convert.ToDateTime(row["receivetime"].ToString()), row["messagefrom"].ToString(), row["messageto"].ToString(), row["messagetext"].ToString()));
           }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetMessageIn] in class [da_message_in], Detail: " + ex.Message);
        }
        return arr_message;
    }
}