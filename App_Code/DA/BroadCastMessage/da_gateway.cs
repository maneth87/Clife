using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for da_gateway
/// </summary>
public class da_gateway
{
	public da_gateway()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static List<bl_gateway> GatewayList()
    {
        List<bl_gateway> arr_obj = new List<bl_gateway>();
        try
        {
          
            foreach (DataRow row in DataSetGenerator.Get_Data_Soure( "select * from ct_message_gateway").Rows)
            {
                arr_obj.Add(new bl_gateway() { 
                     CompanyPhoneTypeID = Convert.ToInt32(row["company_phone_type_id"].ToString()),
                     PhoneNumber = row["phone_number"].ToString()
                });
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GatewayList] in class [da_gateway], Detail: " + ex.Message);
        }
        return arr_obj;
    }
}