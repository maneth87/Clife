using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;

using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;

/// <summary>
/// Summary description for MessageWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class MessageWebService : System.Web.Services.WebService {

    public MessageWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public List<bl_message_history> GetMessageList(string from_date, string to_date, string message_cate, string status_code) {
        List<bl_message_history> message_list = new List<bl_message_history>();
        try
        {
            message_list= da_message_history.GetMessageHistory(Helper.FormatDateTime(from_date), Helper.FormatDateTime(to_date), message_cate, status_code);
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [GetMessageList] in class [MessageWebService], Detail: " + ex.Message);
        }
        return message_list;
    }
    [WebMethod]
    public bool ExportMessage(HttpContext context, string from_date, string to_date, string message_cate, string status_code)
    {
        bool status = false;
        HSSFWorkbook workbook = new HSSFWorkbook();
        List<bl_message_history> message_list = new List<bl_message_history>();
        try
        {
            message_list = da_message_history.GetMessageHistory(Helper.FormatDateTime(from_date), Helper.FormatDateTime(to_date), message_cate, status_code);
            string filename = "Message_History" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            context.Response.Clear();
            
            HSSFSheet sheet1 = (HSSFSheet)workbook.CreateSheet("Data");

            //header 
            HSSFRow row = (HSSFRow)sheet1.CreateRow(0);
            HSSFCell h_cell0 = (HSSFCell)row.CreateCell(0);
            h_cell0.SetCellValue("No.");

            HSSFCell h_cell1 = (HSSFCell)row.CreateCell(1);
            h_cell1.SetCellValue("Message Cate.");

            HSSFCell h_cell2 = (HSSFCell)row.CreateCell(2);
            h_cell2.SetCellValue("Message From");

            HSSFCell h_cell3 = (HSSFCell)row.CreateCell(3);
            h_cell3.SetCellValue("Message To");

            HSSFCell h_cell4 = (HSSFCell)row.CreateCell(4);
            h_cell4.SetCellValue("Message Text");

            HSSFCell h_cell5 = (HSSFCell)row.CreateCell(5);
            h_cell5.SetCellValue("Send Time");

            HSSFCell h_cell6 = (HSSFCell)row.CreateCell(6);
            h_cell6.SetCellValue("Status");

            //rows
            int index = 1;
            foreach (bl_message_history message in message_list)
            {
                HSSFRow row1 = (HSSFRow)sheet1.CreateRow(index);

                //cells
                HSSFCell row_cell0 = (HSSFCell)row1.CreateCell(0);
                row_cell0.SetCellValue(message.No);

                HSSFCell row_cell1 = (HSSFCell)row1.CreateCell(1);
                row_cell1.SetCellValue(message.MessageCate);

                HSSFCell row_cell2 = (HSSFCell)row1.CreateCell(2);
                row_cell2.SetCellValue(message.MessageFrom);

                HSSFCell row_cell3 = (HSSFCell)row1.CreateCell(3);
                row_cell3.SetCellValue(message.MessageTo);

                HSSFCell row_cell4 = (HSSFCell)row1.CreateCell(4);
                row_cell4.SetCellValue(message.MessageText);

                HSSFCell row_cell5 = (HSSFCell)row1.CreateCell(5);
                row_cell5.SetCellValue(message.SendTime);

                HSSFCell row_cell6 = (HSSFCell)row1.CreateCell(6);
                row_cell6.SetCellValue(message.StatusCode);
                row_cell6.CellStyle.FillForegroundColor = 13;

                index += 1;
            }

            MemoryStream file = new MemoryStream();
            workbook.Write(file);

            context.Response.BinaryWrite(file.GetBuffer());

            context.Response.End();

            status = true;
        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error function [ExportMessage] in class [MessageWebService], Detail: " + ex.Message);
            status = false;
        }
        return status;
    }

    [WebMethod]
    public bool SendMessage(List<string> message_info, List<string>history_id, List<string>log_id)
    {
        bool status = false;
        try
        {

            if (message_info.Count > 0)
            {
                string user_name = Membership.GetUser().UserName;
                bl_send_message send;
                foreach (string message in message_info)
                {
                    string[] separatingChars = { "___" };//under score 3 times  
                    //string[] arr_message = message.Split(',');
                    string[] arr_message = message.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                    if (arr_message[0].ToString().Trim() != "" && arr_message[1].ToString().Trim() != "" && arr_message[2].ToString().Trim() != "" && arr_message[3].ToString().Trim() != "")
                    {
                        send = new bl_send_message();
                        bl_prefix_number prefix = da_broadcast_message.PrefixNumber(arr_message[1].ToString().Trim());
                        send.MessageFrom = arr_message[0].ToString().Trim();
                        send.MessageTo = arr_message[1].ToString().Trim();
                        send.MessageCate = arr_message[2].ToString().Trim();
                        send.MessageText = arr_message[3].ToString().Trim();
                        send.MessageType = "sms.tex";
                        send.Gateway = prefix.Gateway;
                        send.UserInfo = "";
                        send.UserId = user_name;
                        send.Priority = 0;
                        send.Scheduled = DateTime.Now;
                        send.IsRead = 0;
                        send.IsSent = 0;

                       status = da_broadcast_message.SaveMessage(send);
                    }

                }
                if (status == true)
                {
                    //delete records in MessageOut_History
                   
                    string str_history_id = "";
                    foreach (string str_his in history_id)
                    {
                        if (str_his.Trim() != "")
                        {
                            str_history_id += str_his.Trim() + ",";
                        }
                    }
                    str_history_id = str_history_id.Substring(0, str_history_id.Length - 1);

                    //Delete record in MessageLog
                    string str_log_id = "";
                    foreach (string str_id in log_id)
                    {
                        if (str_id.Trim() != "")
                        {
                            str_log_id += str_id.Trim() + ",";
                        }
                    }
                    str_log_id = str_log_id.Substring(0, str_log_id.Length - 1);

                   Helper.ExecuteCommand(AppConfiguration.GetConnectionString(), "Delete MessageOut_History where RecordId in (" + str_history_id + "); Delete MessageLog where id in (" + str_log_id + ");");
                    
                }

               
            }
        }
        catch (Exception ex)
        {
            status = false;
            Log.AddExceptionToLog("Error function [SendMessage] in class [MessageWebService], Detail: " + ex.Message);
        }
        return status;
    }

    [WebMethod]
    public List<bl_prefix_number> GetPrefixNumber()
    {
        //List<bl_prefix_number> gateway_list = da_broadcast_message.GetPrefixNumberList();
        List<bl_prefix_number> gateway_list = new List<bl_prefix_number>();

        var test = da_broadcast_message.GetPrefixNumberList().GroupBy(a => a.MessageFrom).Select(g => g.First()).ToList();// distinct message from
        gateway_list=((List<bl_prefix_number>)test);
        return gateway_list;

    }
    [WebMethod]
    public List<bl_gateway> GetGateway(int phone_company_type_id, string phone_number)
    {

        List<bl_gateway> arr_obj = new List<bl_gateway>();

        foreach (bl_gateway obj in da_gateway.GatewayList().Where(a => a.CompanyPhoneTypeID == phone_company_type_id && a.PhoneNumber != phone_number))
        {
            //arr_obj = new List<bl_gateway>();
            arr_obj.Add(obj);
        }

        //var obj = arr_obj.Where(a => a.PhoneNumber != phone_number);// && a.CompanyPhoneTypeID == phone_company_type_id);

        return arr_obj;
    }
    [WebMethod]
    public bool UpdateGateway(string old_number, string new_number)
    {
        bool status = false;
        status = da_broadcast_message.UpdateGateway(old_number, new_number);
        return status;
    }
}
