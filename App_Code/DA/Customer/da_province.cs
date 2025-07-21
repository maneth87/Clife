using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for da_province
/// </summary>
public class da_province
{
    List<bl_province> province_list;
	public da_province()
	{
		//
		// TODO: Add constructor logic here
		//
        //Get all provinces
        province_list = GetProvinceList("");
	}

    public static List<bl_province> GetProvinceList(string pro_name)
    {
        List<bl_province> arr_obj = new List<bl_province>();
        foreach (DataRow row in DataSetGenerator.Get_Data_Soure("SP_GET_PROVINCE", new string[,] { { "@PRO_NAME", pro_name } }).Rows)
        {
            arr_obj.Add(new bl_province(row["pro_id"].ToString(), row["pro_name"].ToString(), row["pro_name_kh"].ToString(), row["remarks"].ToString(), row["pro_post_code"].ToString(), row["created_by"].ToString(), Convert.ToDateTime(row["created_datetime"].ToString()), row["updated_by"].ToString(), Convert.ToDateTime(row["updated_datetime"].ToString())));
        }
        return arr_obj;
    }

    /// <summary>
    /// Get province by province id
    /// </summary>
    /// <param name="province_id"></param>
    /// <returns></returns>
    public string GetProvinceName(string province_id)
    {
        string province_name = "";
        try
        {
            foreach (bl_province pro in province_list.Where(_ => _.ProID == province_id))
            {
                if (pro.ProNameKH.Trim() != "")
                {
                    province_name = pro.ProNameKH;
                }
                else
                {
                    province_name = pro.ProNAME;
                }
            }
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetProvinceName] in class [da_province], Detail: " + ex.Message);
            province_name = "";
        }
        return province_name;
    }

    
}