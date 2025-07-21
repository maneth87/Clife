using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
/// <summary>
/// Summary description for da_app_add_riders
/// </summary>
public class da_app_add_riders
{
    //store all bl_app_add_riders elements
    public static List<bl_app_add_riders> riders_list = new List<bl_app_add_riders>();

	public da_app_add_riders()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //add rider into list
    public static bool AddRiders(bl_app_add_riders rider)
    { 
        bool result=true;
        try
        {
            //riders_list = new List<bl_app_add_riders>();
            riders_list.Add(rider);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [AddRiders] in class [da_app_add_riders], Detail: " + ex.Message);
            result = false;
        }
        return result;
    }
    public static List<bl_app_add_riders> LoadExistRiders()
    {
        try
        {
            DataTable tbl = new DataTable();
            tbl = DataSetGenerator.Get_Data_Soure("", new string[,] { { "", "" } });

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [LoadExistRiders] in class [da_app_add_riders], Detail: " + ex.Message);
        }
        return riders_list;
    }
    //remove rider
    public static void RemoveRider(int index)
    {
        riders_list.RemoveAt(index);
    }

    //Convert to Datatable
    public static DataTable ConvertListToDataTable(List<bl_app_add_riders> riders_list)
    {
        DataTable myTbl = new DataTable();
        DataTable dataTable = new DataTable();
        try
        {
            //columns
            int col = 0;

            col = riders_list.Count;

            //Add columns
            for (int i = 0; i < col; i++)
            {
                myTbl.Columns.Add();
            }
            //Add rows
            foreach (var arr in riders_list)
            {
                myTbl.Rows.Add(arr);
            }


            //-------------------

           

            //Get all the properties
            PropertyInfo[] Props = typeof(bl_app_add_riders).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (bl_app_add_riders rider in riders_list)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(rider, null);
                }
                dataTable.Rows.Add(values);
            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ConvertListToDataTable] in class [da_app_add_riders], Detail " + ex.Message);
        }

        return dataTable;
    }
}