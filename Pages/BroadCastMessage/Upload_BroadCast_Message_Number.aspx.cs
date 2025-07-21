using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Web.Security;
using System.IO;

public partial class Pages_BroadCastMessage_Upload_BroadCast_Message_Number : System.Web.UI.Page
{
    DataTable my_data;
    string user_name;
    protected void Page_Load(object sender, EventArgs e)
    {
         user_name = Membership.GetUser().UserName;
        if (!Page.IsPostBack)
        {
            //string page_name = "Upload_BroadCast_Message_Number";
            
            //string user_id = Membership.GetUser().ProviderUserKey.ToString();

            //da_user_access uaccess = new da_user_access();
            ////check user acccess page.
            //if (uaccess.GetActiveUserAccessPage(page_name, user_id).UserId == user_id.ToUpper())
            //{
            //    div_err.Attributes.CssStyle.Add("display", "none");

            //}
            //else
            //{
            //    ul.Attributes.CssStyle.Add("display", "none");
            //    div_main.Attributes.CssStyle.Add("display", "none");
            //    div_err.InnerHtml = "No permission to access this page!";
            //}
            div_err.Attributes.CssStyle.Add("display", "none");

        }
    }
     bool Validate( out string message)
    {
        bool status = true;
        message = "";
        string file_path = "";
        //check sheet name
        if ((fupload.PostedFile != null) && !string.IsNullOrEmpty(fupload.PostedFile.FileName))
        {
            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(fupload.PostedFile.FileName);
            string extension = Path.GetExtension(file_name);
            file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
            file_path = save_path + file_name;

            fupload.SaveAs(Server.MapPath(file_path));//save file 

            ExcelConnection my_excel = new ExcelConnection();
            my_excel.FileName = Server.MapPath(file_path);

            if (my_excel.GetSheetName() != "Upload$")
            {
                message = "File is not correct format, please donwload file template from the system.";
                status = false;
            }
            else
            {
                my_excel.CommandText = "Select * from [Upload$]";
                DataTable tbl= my_excel.GetData();
                int col_count = 0;
                col_count = tbl.Columns.Count;
                if (col_count > 3)//check number of columns
                {
                    message = "File is not correct format, please donwload file template from the system.";
                    status = false;
                }
                else
                {//check column name.
                    if (tbl.Columns[0].ColumnName != "No" || tbl.Columns[1].ColumnName != "Name" || tbl.Columns[2].ColumnName != "Phone_Number")
                    {
                        message = "File is not correct format, please donwload file template from the system.";
                        status = false;
                    }
                }
                
            }
        }
        //delete file
        File.Delete(Server.MapPath(file_path));
        return status;
    }
     bool IsNumber(string phone_number)
     {
         
         if (phone_number.Trim().All(c=> Char.IsNumber(c)))
         {
             return true;
         }
         else
         {
             return false;
         }
     }
     void EnabledControl(bool t)
     {
         btnPreview.Enabled = t;
         btnSend.Enabled = t;
         fupload.Enabled = t;
     }
    
    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message +"')", true);
    }
  
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if ((fupload.PostedFile != null) && !string.IsNullOrEmpty(fupload.PostedFile.FileName))
        {
            //check valid
            string message;
            if (Validate(out message) == true)
            {
                string save_path = "~/Upload/";
                string file_name = Path.GetFileName(fupload.PostedFile.FileName);
                string extension = Path.GetExtension(file_name);
                file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
                string file_path = save_path + file_name;
                int row_number = 0;

                fupload.SaveAs(Server.MapPath(file_path));//save file 

                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "SELECT * FROM [Upload$]";
                my_data = my_excel.GetData();
                ViewState["my_data"] = my_data;
                row_number = my_data.Rows.Count;
                string str = "<table class='table table-bordered'><th>No</th><th>Name</th><th>Phone Number</th>";
                    
                foreach (DataRow row in my_data.Rows)
                {
                    str += "<tr><td>" + row["no"].ToString() + "</td><td>" + row["name"].ToString() + "</td><td>" + row["phone_number"].ToString() + "</td></tr>";
                }
                str += "<tr><td colspan='3' style='text-align:right;'><span id='spanHide' style='color:blue; font-size:12px;'>Hide</span></td></tr></table>";
                div_preview_count_records.InnerHtml = "<h3>Preview " + row_number + " Record(s). <span id='spanShow' style='color:blue; font-size:12px;'>Show</span></h3> ";
                div_preview_records.InnerHtml = str;
                if (row_number > 0)
                {
                    btnSend.Enabled = true;
                }
                else
                {
                    btnSend.Enabled = false;
                }

            }
            else
            {
                AlertMessage(message);
                return;
            }
        }
        else
        {
            AlertMessage("Please select your file.");
        }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        DataTable tbl = (DataTable)ViewState["my_data"];
    
        if (txtScript.Text.Trim() == "")
        {
            AlertMessage("Script is required.");
            return;
        }
        else if (tbl.Rows.Count == 0)
        {
            AlertMessage("No records uploaded.");
            return;
        }
        else if (ddlMessageCate.SelectedIndex == 0)
        {
            AlertMessage("Please select message cate.");
            return;
        }
        else
        {
            string str_row = "";
            //string str_row_fail = "";
            int row_number = 0;
            int err_record = 0;
            int success_record = 0;
            string str_phone_number = "";
            string message_cate = "";
            message_cate = ddlMessageCate.SelectedItem.Text.Trim();

            foreach (DataRow row in tbl.Rows)
            {
               
                row_number += 1;
                if (row["phone_number"].ToString() != "")
                {
                    str_phone_number = row["phone_number"].ToString().Trim().Replace(" ", "");//delete space

                    bl_prefix_number prefix = da_broadcast_message.PrefixNumber(row["phone_number"].ToString());
                    //send message to device
                    bool save_message = da_broadcast_message.SendMessage(new bl_send_message()
                    {
                        MessageTo = str_phone_number,
                        MessageFrom = prefix.MessageFrom,
                        MessageText = txtScript.Text.Trim(),
                        MessageType = "sms.tex",
                        Gateway = prefix.Gateway,
                        UserInfo = "",
                        UserId = user_name,
                        Priority = 0,
                        Scheduled = DateTime.Now,
                        IsRead = 0,
                        IsSent = 0,
                        MessageCate = message_cate
                    });
                    string sent_status = "";
                    if (save_message)
                    {
                        sent_status = "Success";
                        success_record += 1;
                    }
                    else
                    {
                        sent_status = "Fail";
                    }

                    str_row += "<tr><td>" + row_number + "</td>" +
                           "<td>" + row["name"].ToString() + "</td>" +
                           "<td>" + str_phone_number + "</td>" +
                           "<td style='color:Green;'>Sent " + sent_status + " </td>" +
                       "</tr>";

                }
                else
                {
                    err_record += 1;
                    str_row += "<tr><td>" + row_number + "</td>" +
                                "<td>" + row["name"].ToString() + "</td>" +
                                "<td>" + str_phone_number + "</td>" +
                                "<td style='color:red;'>Rejected</td>" +
                            "</tr>";

                }
            }
            string str_header = "<h3>Total Records: " + row_number + ", Sent: " + success_record + ", Fail: " + err_record + " </h3>" +
                    "<table class='table table-bordered'><th>No</th><th>Name</th><th>Phone Number</th><th>Status</th>";

            div_upload_records.InnerHtml = str_header + str_row + "</table>";
            //div_fail.InnerHtml = str_header + str_row_fail  + "</table>";
        }
    }
    

}