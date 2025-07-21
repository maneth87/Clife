using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Data.SqlClient;
using System.IO;

/// <summary>
/// Summary description for Report_Properties
/// </summary>
public class Report_Properties : System.Web.UI.Page
{
    public ReportDocument Report_Class { get; set; }

	public Report_Properties()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void InitialParameters(string[,] para)
    {
        int j = para.GetUpperBound(0);

        for (int i = 0; i <= para.GetUpperBound(0); i++)
        {
            Report_Class.SetParameterValue(para[i, 0], para[i, 1]);
        }

    }
    public void InitialParameters(string[,] para, string sub_report_name)
    {
        int j = para.GetUpperBound(0);

        for (int i = 0; i <= para.GetUpperBound(0); i++)
        {
            Report_Class.SetParameterValue(para[i, 0], para[i, 1], sub_report_name);
        }

    }
    public DataTable Get_Data_Soure(string procedure_name, string[,] paramaters)
    {
        DataTable myDataTable = new DataTable();
        DataSet myDataSet = new DataSet();
        try
        {
            string connString = AppConfiguration.GetConnectionString();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(procedure_name, con);
                SqlDataAdapter da;
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i <= paramaters.GetUpperBound(0); i++)
                {
                    cmd.Parameters.AddWithValue(paramaters[i, 0], paramaters[i, 1]);
                }
                //SqlParameter paramName = new SqlParameter();
                //paramName.ParameterName = "@App_Register_ID";
                //paramName.Value = paramater;
                //cmd.Parameters.Add(paramName);

                con.Open();
                da = new SqlDataAdapter(cmd);
                da.Fill(myDataSet);
                SqlDataReader rdr = cmd.ExecuteReader();
                da.Dispose();
                cmd.Dispose();
                con.Close();
                myDataTable = myDataSet.Tables[0];
            }

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [Get_Data_Soure] in Class [Report_Generator], Detail: " + ex.Message);
        }
        return myDataTable;

    }
    public void ExportReport(object obj)
    {
        //view report in pdf
        BinaryReader reader = new BinaryReader(Report_Class.ExportToStream(ExportFormatType.PortableDocFormat));
        
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";
        
        Response.BinaryWrite(reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length)));
        Response.Flush();
        Response.Close();
    }
}