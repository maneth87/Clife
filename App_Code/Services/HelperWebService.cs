using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

/// <summary>
/// Summary description for HelperWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class HelperWebService : System.Web.Services.WebService {

    public HelperWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    
    [WebMethod]
    public  List<string> GetTables()
    {
        DataTable tbl_name = DataSetGenerator.Get_Data_Soure("SELECT [NAME], [OBJECT_ID] FROM SYS.TABLES ORDER BY [NAME]");
        List<string> table_list = new List<string>();
        foreach (DataRow row in tbl_name.Rows)
        {
            table_list.Add(row["NAME"].ToString().Trim());
        }

        return table_list;
    }

    [WebMethod]
    public List<string> GetColumns(string table_name)
    {
        DataTable col_name = DataSetGenerator.Get_Data_Soure("SELECT COLUMN_NAME FROM  INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME  = '" + table_name +"'");
        List<string> col_list = new List<string>();
        foreach (DataRow row in col_name.Rows)
        {
            col_list.Add(row["COLUMN_NAME"].ToString().Trim());
        }

        return col_list;
    }
    [WebMethod]
    public string GetGuid(string table_name, string column_name)
    {
        string guid = "";
        guid = Helper.GetNewGuid(new string[,] { 
            {"TABLE",table_name},
            {"COLUMN",column_name}
        });
        return guid;
    }
    [WebMethod]
    public string GetNextDue(string effective_date,string due_date, int paymode)
{
        DateTime next_due = new DateTime();
        switch (paymode)
        {
            case 1:
                next_due = Helper.FormatDateTime(due_date).Date.AddYears(1);
                break;
            case 2:
                next_due = Helper.FormatDateTime(due_date).Date.AddMonths(6);  
                break;
            case 3:
                next_due = Helper.FormatDateTime(due_date).Date.AddMonths(3);
                break;
            case 4: 
                next_due = Helper.FormatDateTime (due_date).AddMonths(1);
                break;
            default:
                next_due=Helper.FormatDateTime(due_date).AddMonths(0);
                break;
        }
    string result = "";
        DateTime sys_next_due=    Calculation.GetNext_Due(next_due, Helper.FormatDateTime(due_date).Date, Helper.FormatDateTime(effective_date).Date);
        result = "Effective Date=" + effective_date + "\tDue Date=" + due_date + "\tNext Due Date=" + sys_next_due.ToString("dd/MM/yyyy");
        return result;
}
}
