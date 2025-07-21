using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Reflection;

/// <summary>
/// Summary description for Convertor
/// </summary>
public static class Convertor
{
	

    /// <summary>
    /// Convert list object to Datatable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IList<T> data)
    {
        System.ComponentModel.PropertyDescriptorCollection properties =
            System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        foreach (System.ComponentModel.PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (System.ComponentModel.PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }
        return table;
    }

    public static  DataTable ObjectListToDataTable<T>(this IList<T> data)
    {
        DataTable myTbl = new DataTable();
        DataTable dataTable = new DataTable();
        try
        {
            //columns
            int col = 0;

            col = data.Count;

            //Add columns
            for (int i = 0; i < col; i++)
            {
                myTbl.Columns.Add();
            }
            //Add rows
            foreach (var arr in data)
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
            foreach (T obj in data)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(obj, null);
                }
                dataTable.Rows.Add(values);
            }


        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ObjectListToDataTable<T>(this IList<T> data)] in class [Helper], Detail " + ex.Message);
        }

        return dataTable;
    }
}