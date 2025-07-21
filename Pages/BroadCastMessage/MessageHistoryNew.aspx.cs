using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Configuration;
public partial class Pages_BroadCastMessage_MessageHistoryNew : System.Web.UI.Page
{
    string user_name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        user_name = System.Web.Security.Membership.GetUser().UserName;
      
        if (!Page.IsPostBack)
        {
            SqlMessageCate.ConnectionString = AppConfiguration.GetCellCardMessageDbConnectionString();
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtFromDate.Text.Trim() == "" || txtToDate.Text.Trim() == "")
        {
            AlertMessage("From Date and To Date are required.");
            return;
        }
        else
        {
            BindData();
            //Button1.Visible = true;
            btnExport_Success.Visible = true;
            btnResent.Visible = true;
        }
    }
    void BindSuccess(List<History> listHistory)
    {
        List<History> listSuccess = new List<History>();
        int record = 0;
        string[] phoneNumber;
        string htm = "<table border='1'><th>No</th><th>Phone Number</th><th>Message</th><th>Sent DateTime</th>";
        //linq filter
        foreach (History myHis in listHistory.Where(__ => __.Status.Trim().ToUpper() == "SUCCESS"))
        {
            phoneNumber = myHis.PhoneNumber.Split(';');
            foreach (string myPhone in phoneNumber)
            {
                record += 1;
                listSuccess.Add(new History()
                {
                    No = record,
                    PhoneNumber = myPhone,
                    Status = myHis.Status,
                    SendDateTime = myHis.SendDateTime
                });
                htm += "<tr><td>" + record + "</td><td>" + myPhone + "</td><td>" + myHis.MessageText + "</td><td>" + myHis.SendDateTime + "</td></tr>";
            }
        }
        htm += "</table>";
        dvSuccessRecord.InnerHtml = listSuccess.Count + " Record(s).";
        if (listSuccess.Count > 0)
        {
            dvSuccessContent.InnerHtml = htm;
        }
        else
        {
            dvSuccessContent.InnerHtml = "";
        }
    }
    void BindFail(List<History> listHistory)
    {
        List<History> listFail = new List<History>();
      
        int record = 0;
        string[] phoneNumber;
        #region v1
        /* 
        string htm = "<table  border='1'><th>No</th><th>Phone Number</th><th>Message</th><th>Sent DateTime</th>";
        //linq filter
        foreach (History myHis in listHistory.Where(__ => __.Status.Trim().ToUpper() == "FAIL"))
        {
            phoneNumber = myHis.PhoneNumber.Split(';');
            lblIdForUpdate.Text += myHis.ID + ";";
            foreach (string myPhone in phoneNumber)
            {
                record += 1;
                listFail.Add(new History()
                {
                    No = record,
                    PhoneNumber = myPhone,
                    Status = myHis.Status,
                    SendDateTime = myHis.SendDateTime,
                    MessageText = myHis.MessageText
                });

                htm += "<tr><td>" + record + "</td><td>" + myPhone + "</td><td>" + myHis.MessageText + "</td><td>" + myHis.SendDateTime + "</td></tr>";
            }
        }
        htm += "</table>";
        
        
        dvFailRecord.InnerHtml = listFail.Count + " Record(s).";
        if (listFail.Count > 0)
        {
            dvFailContent.InnerHtml = htm;
            btnResent.Enabled = true;
        }
        else
        {
            dvFailContent.InnerHtml = "";
            btnResent.Enabled = false;
        }
        //trim ;
*/
        #endregion v1
        #region v2
        foreach (History myHis in listHistory.Where(__ => __.Status.Trim().ToUpper() == "FAIL")) 
        {
            phoneNumber = myHis.PhoneNumber.Split(';');
           
            foreach (string myPhone in phoneNumber)
            {
                record += 1;
                listFail.Add(new History()
                {
                    No = record,
                    PhoneNumber = myPhone,
                    Status = myHis.Status,
                    SendDateTime = myHis.SendDateTime,
                    MessageText = myHis.MessageText,
                    ID=myHis.ID,
                    MessageCate=myHis.MessageCate
                });

            }
        }
        gvFail.DataSource = listFail;
        gvFail.DataBind();
        if (listFail.Count > 0)
        {
           
            btnResent.Enabled = true;
        }
        else
        {
            
            btnResent.Enabled = false;
        }
       
        dvFailRecord.InnerHtml = listFail.Count + " Record(s).";
        #endregion v2
        if (lblIdForUpdate.Text.Trim() != "")
        {
            lblIdForUpdate.Text = lblIdForUpdate.Text.Trim().Substring(0, lblIdForUpdate.Text.Trim().Length - 1);
        }

    }

    void BindData()
    {
        try
        {
            da_cellcard_message cellCard = new da_cellcard_message();
            List<History> listHistory = new List<History>();
            foreach (bl_cellcard_message message in cellCard.GetMessageList(Helper.FormatDateTime(txtFromDate.Text.Trim()), Helper.FormatDateTime(txtToDate.Text.Trim()), ddlMessageCate.SelectedValue, ddlStatus.SelectedValue,""))
            {
                listHistory.Add(new History()
                {
                    PhoneNumber = message.MessageTo,
                    Status = message.Status,           
                    SendDateTime = message.SendDateTime,
                    MessageText = message.MessageText,
                    ID = message.IDForUpdated,
                    MessageCate=message.MessageCate
                });
            }

            BindSuccess(listHistory);
            BindFail(listHistory);
            dvError.InnerHtml = "";
        }
        catch (Exception ex)
        {
            dvError.InnerHtml = "Ooop! Page getting error!";
            Log.AddExceptionToLog("Error function [BindData] in Page [UploadCellcardMessageHistory], Detail:" + ex.Message + "==>" + ex.InnerException + "==>" + ex.StackTrace);
        }
    }

    class History
    {

        public int No { get; set; }
        public DateTime SendDateTime { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }
        public string MessageText { get; set; }
        public string ID { get; set; }
        public string MessageCate { get; set; }
    }
   
    void AlertMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + message + "')", true);
    }
    protected void btnResent_Click(object sender, EventArgs e)
    {
      
        #region V1
        /*
        if (lblIdForUpdate.Text.Trim() != "")
        {

            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetCellCardMessageDbConnectionString(), "SP_GET_BROADCAST_MESSAGE_BY_ID", new string[,] { 
                {"@ID", lblIdForUpdate.Text.Trim() } 
            }, "[BroadCastMessage_UploadCellcardMessageHistory ==> btnResent_Click]");


          //  DataTable tblCustNotification = da_before_due_notificaton.GetCustomerNotification(Convert.ToDateTime(row["createddatetime"].ToString()), Convert.ToDateTime(row["createddatetime"].ToString()));

            bl_cellcard_message cellcard;
            da_cellcard_message cellcardTran = new da_cellcard_message();
            string[] sentStatus = new string[] { };
            //int index = 0;
            //SentBunchMessages sentMessageObj;
            //List<SentBunchMessages> listSentMessages = new List<SentBunchMessages>();
            foreach (DataRow row in tbl.Rows)
            {
                cellcard = new bl_cellcard_message();
                cellcard.SenderName = "Camlife";
                cellcard.MessageFrom = "+85589951000";
                cellcard.MessageTo = row["messageTo"].ToString();
                cellcard.MessageText = row["messageText"].ToString();
                cellcard.SendDateTime = DateTime.Now;
                cellcard.Status = "queue";
                cellcard.CreatedBy = user_name;
                cellcard.CreatedDateTime = DateTime.Now;
                cellcard.Remarks = "";
                cellcard.IDForUpdated = row["id"].ToString();
                cellcard.MessageCate = row["MessageCate"].ToString();
                if (cellcardTran.SaveMessage(cellcard))
                {
                    //index += 1;
                    cellcardTran.DeleteMessage(cellcard.IDForUpdated);// delete previous sent fail message
                    sentStatus = cellcard.SendMessage(cellcard);
                    // UPDATE MESSAGE STATUS
                    cellcardTran.UpdateMessageStatus(cellcard.ID, sentStatus[1].ToString(), sentStatus[0].ToString(), user_name, DateTime.Now);
                }

                System.Threading.Thread.Sleep(1000);//delay 1 second.
            }

            lblIdForUpdate.Text = "";
            AlertMessage("Messages had been resent.");
            return;
        }
        else
        {
            AlertMessage("No messages to send.");
            return;
        }
         * */
        #endregion V1
            #region V2
        SENDSMS sms;
        da_cellcard_message cellcardTran = new da_cellcard_message();
       
        int recordSent = 0;
        int recordSelected = 0;
        foreach (GridViewRow row in gvFail.Rows)
        {
            CheckBox ckb = (CheckBox)row.FindControl("ckb");
            string id = ((Label)row.FindControl("lblID")).Text.Trim();
            string message = ((Label)row.FindControl("lblMessage")).Text.Trim();
            string cate = ((Label)row.FindControl("lblMessageCate")).Text.Trim();
            string phone = ((Label)row.FindControl("lblPhoneNumber")).Text.Trim();
            DateTime sendDate = Convert.ToDateTime(((Label)row.FindControl("lblSentDateTime")).Text.Trim());
            if (ckb.Checked)
            {
              //  phone = "+85577588098";
                sms = new SENDSMS();
                sms.Message = message;
                sms.MessageCate = cate;
                sms.PhoneNumber = "0"+ phone.Substring(4, phone.Length - 4);
               
                recordSelected += 1;

                if (sms.Send())
                {
                   
                    recordSent += 1;
                   // cellcardTran.DeleteMessage(id);// delete previous sent fail message
                    cellcardTran.UpdateMessageStatus(id, "TRY-RESENT", "Try to resend in another new sms.", user_name, DateTime.Now);
                    if (sms.MessageCate.ToUpper() == "7DAY_BEFORE_DUE")
                    {
                        DataTable tblCustNotification = da_before_due_notificaton.GetCustomerNotification(sendDate, sendDate);
                       
                        foreach (DataRow rowCust in tblCustNotification.Select("next_due_day=7 and phone='" + sms.PhoneNumber + "'"))
                        {
                            da_before_due_notificaton.UpdateNotificationStatus(Convert.ToInt32(rowCust["ID"].ToString().Trim()), user_name, DateTime.Now, "SUCCESS");
                            da_before_due_notificaton.Delete(Convert.ToInt32(rowCust["ID"].ToString().Trim()), user_name);
                        }
                    }
                }
               
                System.Threading.Thread.Sleep(1000);//delay 1 second.
            }
        }
        if (recordSelected > 0)
        {
            if (recordSent == 0)
            {
                //no record sent
            
                ScriptManager.RegisterStartupScript(this.up, this.up.GetType(), "", "alert('Messages has been fail.')", true);
            }
            else if (recordSent == recordSelected)
            {
                //all selected records sent successfully
                ScriptManager.RegisterStartupScript(this.up, this.up.GetType(), "", "alert(' Messages has been sent.')", true);
                //AlertMessage("Messages has been sent.");
               // return;
            }
            else
            {
                //some records sent fail
         
                ScriptManager.RegisterStartupScript(this.up, this.up.GetType(), "", "alert('Some messages were sent fail.')", true);
            }
        }
        else
        {
        
            ScriptManager.RegisterStartupScript(this.up, this.up.GetType(), "", "alert('No messages to send.')", true);
        }
        btnSearch_Click(null, null);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "clear_loading();", true);
        
            #endregion V2
    }

    protected void ckbAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ckb_header = (CheckBox)gvFail.HeaderRow.FindControl("ckbAll");
        foreach (GridViewRow row in gvFail.Rows)
        {
            CheckBox ckb = (CheckBox)row.FindControl("ckb");
          
            if (ckb_header.Checked)
            {
                ckb.Checked = true;
                
            }
            else
            {
                ckb.Checked = false;

            }
        }
    }

    protected void btnExport_Success_Click(object sender, EventArgs e)
    {
       
        ExportExcel(sender,e);
    }    

    protected void ExportExcel(object sender, EventArgs e)
    {
        try
        {
            DataTable tbl = DataSetGenerator.Get_Data_Soure(AppConfiguration.GetConnectionString(), "SP_GET_BROADCAST_MESSAGE_WITH_CUSTOMER", new string[,] 
            { 
                {"@FROM", Helper.FormatDateTime(txtFromDate.Text.Trim()) +""},
                {"@TO", Helper.FormatDateTime(txtToDate.Text.Trim()) +""} 
            },

                "[da_cellcard_message ==> GetMessageList]");

            using (XLWorkbook wb = new XLWorkbook())
            {
                
                wb.Worksheets.Add(tbl);               

                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=BroadcaseMessage.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            dvError.InnerHtml = "Ooop! Page getting error!";
            Log.AddExceptionToLog("Error function [Export Excel] in Page [MessageHistoryNew], Detail:" + ex.Message + "==>" + ex.InnerException + "==>" + ex.StackTrace);
        }
      }   
}