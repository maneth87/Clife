using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using NReco.PdfGenerator;
using System.IO;
using System.Net;
/// <summary>
/// Summary description for Report_Generator
/// </summary>
public class Report_Generator
{
    public ReportDocument Report_Class { get; set; }
    public DataTable Report_Source { get; set; }
    public string Report_Name { get; set; }
    
    public string[,] Report_Parameters { get; set; }
    public string[,] Sub_Report_Parameters { get; set; }
    public List<string[]> SubReport { get; set; }

    public Array Sub_Report{get;set;}

	public Report_Generator()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void GenerateReport()
    {
       // Report.Load(Report_Name);
        Report_Class.SetDataSource(Report_Source);
        InitialParameters(Report_Parameters);
        InitialParametersInSubReport(Sub_Report_Parameters, "data_check_list_premium_detail.rpt");
       // Report.SetParameterValue("Report_Title", Report_Paramater);

    }
    public  DataTable Get_Data_Soure(string procedure_name, string[,] paramaters)
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
            Log.AddExceptionToLog("Error function [Get_Data_Soure] in Class [Report_Generator], Detail: "+ ex.Message);
        }
        return myDataTable;

    }

    public void InitialParameters(string[,] para)
    {
        int j = para.GetUpperBound(0);

        for (int i = 0; i <= para.GetUpperBound(0); i++)
        {
            Report_Class.SetParameterValue(para[i, 0], para[i, 1]);
        }

    }
    public void InitialParametersInSubReport(string[,] para, string sub_report_name)
    {
        int j = para.GetUpperBound(0);

        for (int i = 0; i <= para.GetUpperBound(0); i++)
        {
            Report_Class.SetParameterValue(para[i, 0], para[i, 1], sub_report_name);
            
        }

    }
    public void Add_Sub_Report(string[] sub_report_name)
    {
        SubReport.Add(sub_report_name);
     
    }
    /// <summary>
    /// Convert RDCL Report to PDF
    /// </summary>
    /// <param name="context"></param>
    /// <param name="my_report_viewer"></param>
    /// <param name="file_name">Only file name without extension</param>
    /// <param name="download">Todownload file set vaule = True</param>
    public static void ExportToPDF(HttpContext context, ReportViewer my_report_viewer, string file_name, bool download)
    {
        //pdf view
        Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;

        byte[] bytes = my_report_viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        context.Response.Buffer = true;
        context.Response.Clear();
        context.Response.ContentType = mimeType;

        if (download)
        {
            //client download
            context.Response.AddHeader("content-disposition", "attachment; filename=" + file_name + ".pdf");
        }
        context.Response.BinaryWrite(bytes); // create the file
        context.Response.Flush();
      
    }
 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="my_report_viewer"></param>
    /// <param name="file_name"></param>
    /// <param name="download"></param>
    /// <param name="type">[PDF, WORD]</param>
    public static void Export(HttpContext context, ReportViewer my_report_viewer, string file_name, bool download, string type)
    {
        //pdf view
        Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;

        byte[] bytes = my_report_viewer.LocalReport.Render(type, null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        context.Response.Buffer = true;
        context.Response.Clear();
        context.Response.ContentType = mimeType;

        if (download)
        {
            //client download
            string ext = type == "PDF" ? ".pdf" : type == "WORD" ? ".docx" : type == "EXCEL" ? ".xlsx": "";
            context.Response.AddHeader("content-disposition", "attachment; filename=" + file_name + type);
        }
        context.Response.BinaryWrite(bytes); // create the file
        context.Response.Flush();

    }
    /// <summary>
    /// Convert Html page to PDF
    /// </summary>
    /// <param name="context"></param>
    /// <param name="html"></param>
    public static void ExportToPDF(HttpContext context, string html)
    {

        HtmlToPdfConverter pdf = new HtmlToPdfConverter();
        byte[] pdf_bye = pdf.GeneratePdf(html);
      
        context.Response.ContentType = "application/pdf";// contentType;
        context.Response.AddHeader("", pdf_bye.Length.ToString());

        context.Response.BinaryWrite(pdf_bye);
    }

    public static void ExportToPDF(HttpContext context, string path, string type)
    {

        HtmlToPdfConverter pdf = new HtmlToPdfConverter();
        byte[] pdf_bye = pdf.GeneratePdfFromFile(path, "");

        context.Response.ContentType = "application/pdf";// contentType;
        context.Response.AddHeader("", pdf_bye.Length.ToString());

        context.Response.BinaryWrite(pdf_bye);
    }

    public static void DownloadPDF( HttpContext context, string fileName, string contentType, byte[] content)
    {
        HttpContext.Current.Response.Clear();
        context.Response.ContentType = contentType;
        context.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        context.Response.CacheControl = "No-cache";
        context.Response.BinaryWrite(content);
        context.Response.Flush();
        context.Response.SuppressContent = true;
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    public static void CrystallToPDF(HttpContext context, ReportDocument reportDocument )
    {
       
        context. Response.ClearContent();
        context.Response.ClearHeaders();
        context.Response.ContentType = "application/PDF";
        context.Response.AddHeader("Content-Disposition", "inline; filename=list.pdf");
        MemoryStream mem;// = new MemoryStream();
        Stream s = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
        var sr = new BinaryReader(s);

        mem = new MemoryStream(sr.ReadBytes((int)s.Length));

       context. Response.BinaryWrite(mem.ToArray());
       context.Response.Flush();
       context. Response.Close();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="reportDocument"></param>
    /// <param name="download"></param>
    public static void CrystallToPDF(HttpContext context, ReportDocument reportDocument, bool download, string fileName)
    {

        context.Response.ClearContent();
        context.Response.ClearHeaders();
        context.Response.ContentType = "application/PDF";
        context.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        MemoryStream mem;// = new MemoryStream();
        Stream s = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
        var sr = new BinaryReader(s);

        mem = new MemoryStream(sr.ReadBytes((int)s.Length));
        context.Response.CacheControl = "No-cache";
        context.Response.BinaryWrite(mem.ToArray());
        context.Response.Flush();
        context.Response.Close();
        context.Response.SuppressContent = true;
        HttpContext.Current.ApplicationInstance.CompleteRequest();

    }
    /// <summary>
    /// Convert Crystal Report to PDF and save it in disk.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="reportDocument"></param>
    /// <param name="fileName"></param>
    public static void CrystallToPDF(ReportDocument reportDocument, string fileName)
    {
        reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, fileName);

    }
    public static void ReadPDF(HttpContext context, string path)
    {
        WebClient client = new WebClient();
        Byte[] buffer = client.DownloadData(path);
       context. Response.ContentType = "application/pdf";
        context.Response.AddHeader("", buffer.Length.ToString());
        context.Response.BinaryWrite(buffer);

        File.Delete(path);
    }
}