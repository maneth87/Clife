using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.IO;

/// <summary>
/// Summary description for ExcelConnection
/// </summary>
public class PDFConnection
{
    public PDFConnection()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string FileName { get; set; }
    public string CommandText { get; set; }
    //string SheetName = "";
    public string Extension { get { return _extension; } }
    public bool IspdfFile { get { return _ispdf; } }
    public bool Status { get { return _status; } }
    public string Message { get { return _message; } }

    private string _extension = "";
    private bool _ispdf = false;
    private bool _status = false;
    private string _message = "";
    private  OleDbConnection GetConnection()
    {
        OleDbConnection my_connection = null;
        try
        {
            if (FileName != "")
            {
                string extension = "";
                extension = Path.GetExtension(FileName);
                if (extension == ".pdf")
                {
                    //my_connection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + FileName + "';Extended Properties=Excel 8.0;");
                    my_connection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + FileName + "';Extended Properties=Excel 12.0;");
                    _ispdf = true;
                }
                _extension = extension;
            }
    
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetConnection] in class [PDFConnection], Detial: " + ex.Message);
        }
       
        return my_connection;
    }
    public DataTable GetData()
    {
        DataTable my_data = new DataTable();
        OleDbDataAdapter my_ad;
        OleDbConnection my_conn = new OleDbConnection();

        try
        {
            
                my_conn = GetConnection();
                if (IspdfFile)
                {
                my_conn.Open();


                my_ad = new OleDbDataAdapter(CommandText, my_conn);
                my_ad.Fill(my_data);
                my_conn.Close();
                _status = true;
                _message = "Success";
            }
            else
            {
                _status = false;
                _message = "File "+ FileName + " is not PDF FILE";
            }
        }
        catch (Exception ex)
        {
            _status = false;
            _message = ex.Message;
            Log.AddExceptionToLog("Error function [GetData] in class [PDFConnection], Detial: " + ex.Message);
            my_conn.Close();
        }
        return my_data;
    }
    
}