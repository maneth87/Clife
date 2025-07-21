using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.Security;
public partial class Pages_Business_banca_micro_payment_upload : System.Web.UI.Page
{
    class my_session
    {
        public  DataTable DATA = new DataTable();
        public  String FileName = "";
        public  bool isValid = true;
        public  string Message = "";
    }
    my_session session = new my_session();

    string userID = "";
    string userName = "";
    private bl_sys_user_role UserRole { get { return (bl_sys_user_role)ViewState["V_USER_ROLE"]; } set { ViewState["V_USER_ROLE"] = value; } }

    public void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        da_sys_activity_log.Save(new bl_sys_activity_log(UserRole.UserName, UserRole.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        userID = Membership.GetUser().ProviderUserKey.ToString();
        userName = Membership.GetUser().UserName;

        lblError.Text = "";

        string ObjCode = Path.GetFileName(Request.Url.AbsolutePath);
        List<bl_sys_user_role> Lobj = (List<bl_sys_user_role>)Session["SS_UR_LIST"];
        if (Lobj != null)
        {
            bl_sys_user_role ur = new bl_sys_user_role();
            bl_sys_user_role u = ur.GetSysUserRole(Lobj, ObjCode);
            if (u.UserName != null)
            {
                UserRole = u;
            }
            if (!Page.IsPostBack)
            {
                //string page_name = Path.GetFileName(Request.Url.AbsolutePath);
                //da_user_access user_acc = new da_user_access();

                //if (user_acc.GetActiveUserAccessPage(page_name, userID).UserId != userID)
                //{
                //    dv_main.Attributes.CssStyle.Add("display", "none");
                //    showMessage("No permission to access this page!", "1");
                //}
                //else
                //{
                //    showMessage("", "");
                //    Helper.BindChannelItem(ddlChannelItem, "0152DF80-BA95-46A9-BB7A-E71966A34089");
                //}

                showMessage("", "");
                Helper.BindChannelItem(ddlChannelItem, "0152DF80-BA95-46A9-BB7A-E71966A34089");
            }
        }
        else
        {
            Response.Redirect("../../unauthorize.aspx");
        }

     
    }
    protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlChannelItem_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["SS_DATA"] != null)
        {
            gv_valid.PageIndex = e.NewPageIndex;

            DataTable tbl = (DataTable)Session["SS_DATA"];

            gv_valid.DataSource = tbl;
            gv_valid.DataBind();
        }

      
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {

        if (ddlChannelItem.SelectedValue == "791D3296-82D0-4F07-AC62-B5C358742E2B")//Hattha
        {
            validateFile();
        }
        else if (ddlChannelItem.SelectedValue == "C6F8A033-548E-45BB-BA16-98D4792B7A62")//Wing 
        {
            validateFileWingBanca();
        }
        
    
        DataTable tblSuccess = new DataTable();
     
        DataTable tblFail = new DataTable();
        if (session.isValid)
        {
          
            bool save=false;
            if (session.DATA.Rows.Count > 0)
            {
             
                try
                {

                    save = da_banca.Payment.UploadPaymentList(session.DATA, ddlChannelItem.SelectedItem.Text, userName, DateTime.Now, ddlChannelItem.SelectedValue );
                    if (save)
                    {
                      
                        //load imported data
                        DataTable tbl = da_banca.Payment.GetTempPaymentList(userName, DateTime.Now, ddlChannelItem.SelectedValue);
                        if (da_banca.Payment.SUCCESS)
                        {
                            try
                            {
                                gv_valid.DataSource = tbl;
                                gv_valid.DataBind();
                                lblRecords.Text = tbl.Rows.Count + " Record(s) imported.";
                                Helper.Alert(false, "Imported successfully.", lblError);
                                Session["SS_DATA"] = tbl;//store to do other transaction in selected page index

                                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User uploads payment record of ", ddlChannelItem.SelectedItem.Text," [Total record(s):", tbl.Rows.Count,"]."));

                            }
                            catch (Exception ex)
                            {
                                lblRecords.Text = "";
                                gv_valid.DataSource = null;
                                gv_valid.DataBind();
                                Helper.Alert(false, "Imported successfully. </br>Error while system is trying to display data, </br>Detail: "+ ex.Message, lblError);
                            }
                        }
                        else
                        {
                            Helper.Alert(false, "Imported successfully. </br>Error while system is trying to display data, please contact your system administrator.", lblError);
                        }
                    }
                    else
                    {
                        Helper.Alert(true,  da_banca.Payment.MESSAGE, lblError);
                    }

                }
                catch (Exception ex)
                {
                    Helper.Alert(true, "Imported fail, detail error:"  + ex.Message, lblError);
                }
              

            }
            else
            {
                Helper.Alert(true, "No record upload.", lblError);
            }
        }
        else

        {
            Helper.Alert(true, session.Message, lblError);
        }
    }

    void validateFile()
    {
        //reset some value in session
        //my_session.Message = "";
        //my_session.isValid = true;
        //my_session.DATA = null;
        
        session.Message = "";
        session.isValid = true;
        session.DATA = null;
        bool saved_file = false;
        if (ddlChannel.SelectedIndex == 0)
        {
            session.isValid = false;
            session.Message = "Please select Channel.";
        }
        else if (ddlChannelItem.SelectedIndex == 0)
        {
            session.isValid = false;
            session.Message = "Please select Company.";
        }
        else if ((fUpload.PostedFile != null) && !string.IsNullOrEmpty(fUpload.PostedFile.FileName))
        {

            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(fUpload.PostedFile.FileName);
            string extension = Path.GetExtension(file_name);
            file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
            string file_path = "";
            if (extension.Trim().ToLower() == ".xls" || extension.Trim().ToLower() == ".xlsx")
            {
                file_path = save_path + file_name;
                fUpload.SaveAs(Server.MapPath(file_path));//save file 
                saved_file = true;
                DataTable myData = new DataTable();
                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "select * from [Data$]";

                if (my_excel.GetSheetName().ToLower()=="data$")
                {
                    myData = my_excel.GetData();
                    if (my_excel.Status)
                    {
                        session.DATA = myData;
                        
                        if (myData.Columns.Count == 9)
                        {
                            if(myData.Rows.Count>0)
                            {
                                string branchCode = "";
                                string branchName = "";

                            string appNo="";
                            string premium ="";
                            int index = -1;
                            string payDate = "";
                            string err = "";
                            try
                            {
                                foreach (DataRow r in myData.Rows)
                                {
                                    branchCode = r[0].ToString();
                                    branchName = r[1].ToString();
                                    appNo = r[3].ToString();
                                    index =myData.Rows.IndexOf(r)+1;

                                    if (branchCode == "")
                                    {
                                        err = "[Branch Code can not be blank.]";
                                    }
                                    else if (branchName == "")
                                    {
                                        err = "[Branch Name can not be blank.]";
                                    }

                                    #region validate application number
                                    if (appNo == "")
                                    {
                                        err="[Insurance Application Number can not be blank.]";
                                    }
                                    else if (appNo.Length < 10 || appNo.Length >11)
                                    {
                                        err = "[Insurance Application Number must be in format APP{8 digits} digits.]";
                                    }
                                    else if (appNo.Substring(0, 3) != "APP")
                                    {
                                        err = "[Insurance Application Number must be in format APP{8 digits} digits.]";
                                    }
                                    #endregion
                                    #region premium
                                    premium = r[5].ToString();
                                    if (premium == "")
                                    {
                                        err = err != "" ? err  + " / [Premium cannot be blank.]" : "[Premium cannot be blank.]";
                                    }
                                    else if (!Helper.IsAmount(premium))
                                    {
                                        err = err != "" ? err + " / [Premium is allowed only number.]" : "[Premium is allowed only number.]";
                                    }
                                    #endregion
                                    #region pay date
                                    payDate = r[7].ToString();
                                    if (payDate == "")
                                    {
                                        err = err != "" ? err + " / [Transaction Date cannot be blank.]" : "[Payment Date cannot be blank.]";
                                    }
                                    else if (!Helper.IsDate(payDate))
                                    {
                                        err = err != "" ? err + " / [Transaction Date is invalid format.]" : "[Payment Date is invalid format.]";
                                    }
                                    #endregion

                                    if(err!="")
                                    {
                                        session.Message = "Row [" + index+ "] " + err;
                                        session.isValid = false;
                                    break;
                                    }
                                }
                           
                            }
                            catch (Exception ex)
                            {
                                session.isValid = false;
                                session.Message ="Unexpected Error: " + ex.Message;
                            }
                            }
                            else// no row 
                            {
                                session.isValid = false;
                                session.Message = "No record to upload.";
                            }
                        }
                        else//invalid column numbers
                        {

                            session.isValid = false;
                            session.Message = "Please check your file, make sure total columns number = 9.";
                        }

                       
                    }
                    else //get data from excel error
                    {
                        session.Message = my_excel.Message;
                        session.isValid = false;
                    }
                }
                else//invalid sheet name
                {
                    session.Message = "Invalid sheet name, please download file template from system.";
                    session.isValid = false;
                }
            }//invalid file 
            else
            {
                session.isValid = false;
                session.Message = "[" + extension + "] is not supported.";

            }
            //delete file
            if (saved_file)
            {
                File.Delete(Server.MapPath(file_path));
            }
        }
        else
        {
            session.isValid = false;
            session.Message = "Please select an excel file.";
        }

    }
    void validateFileWingBanca()
    {
        session.Message = "";
        session.isValid = true;
        session.DATA = null;
        bool saved_file = false;
        if (ddlChannel.SelectedIndex == 0)
        {
            session.isValid = false;
            session.Message = "Please select Channel.";
        }
        else if (ddlChannelItem.SelectedIndex == 0)
        {
            session.isValid = false;
            session.Message = "Please select Company.";
        }
        else if ((fUpload.PostedFile != null) && !string.IsNullOrEmpty(fUpload.PostedFile.FileName))
        {

            string save_path = "~/Upload/";
            string file_name = Path.GetFileName(fUpload.PostedFile.FileName);
            string extension = Path.GetExtension(file_name);
            file_name = file_name.Replace(extension, DateTime.Now.ToString("yyyymmddhhmmss") + extension);
            string file_path = "";
            if (extension.Trim().ToLower() == ".xls" || extension.Trim().ToLower() == ".xlsx")
            {
                file_path = save_path + file_name;
                fUpload.SaveAs(Server.MapPath(file_path));//save file 
                saved_file = true;
                DataTable myData = new DataTable();
                ExcelConnection my_excel = new ExcelConnection();
                my_excel.FileName = Server.MapPath(file_path);
                my_excel.CommandText = "select * from [Data$]";

                if (my_excel.GetSheetName().ToLower() == "data$")
                {
                    myData = my_excel.GetData();
                    if (my_excel.Status)
                    {
                        session.DATA = myData;

                        if (myData.Columns.Count == 9)
                        {
                            if (myData.Rows.Count > 0)
                            {
                                string branchCode = "";
                                string branchName = "";

                                string appNo = "";
                                string premium = "";
                                int index = -1;
                                string payDate = "";
                                string err = "";
                                try
                                {
                                    foreach (DataRow r in myData.Rows)
                                    {
                                        branchCode = r[0].ToString();
                                        branchName = r[1].ToString();
                                        appNo = r[3].ToString();
                                        index = myData.Rows.IndexOf(r) + 1;

                                        if (branchCode == "")
                                        {
                                            err = "[Branch Code can not be blank.]";
                                        }
                                        else if (branchName == "")
                                        {
                                            err = "[Branch Name can not be blank.]";
                                        }

                                      
                                        #region premium
                                        premium = r[5].ToString();
                                        if (premium == "")
                                        {
                                            err = err != "" ? err + " / [Premium cannot be blank.]" : "[Premium cannot be blank.]";
                                        }
                                        else if (!Helper.IsAmount(premium))
                                        {
                                            err = err != "" ? err + " / [Premium is allowed only number.]" : "[Premium is allowed only number.]";
                                        }
                                        #endregion
                                        #region pay date
                                        payDate = r[7].ToString();
                                        if (payDate == "")
                                        {
                                            err = err != "" ? err + " / [Transaction Date cannot be blank.]" : "[Payment Date cannot be blank.]";
                                        }
                                        else if (!Helper.IsDate(payDate))
                                        {
                                            err = err != "" ? err + " / [Transaction Date is invalid format.]" : "[Payment Date is invalid format.]";
                                        }
                                        #endregion

                                        if (err != "")
                                        {
                                            session.Message = "Row [" + index + "] " + err;
                                            session.isValid = false;
                                            break;
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    session.isValid = false;
                                    session.Message = "Unexpected Error: " + ex.Message;
                                }
                            }
                            else// no row 
                            {
                                session.isValid = false;
                                session.Message = "No record to upload.";
                            }
                        }
                        else//invalid column numbers
                        {

                            session.isValid = false;
                            session.Message = "Please check your file, make sure total columns number = 9.";
                        }


                    }
                    else //get data from excel error
                    {
                        session.Message = my_excel.Message;
                        session.isValid = false;
                    }
                }
                else//invalid sheet name
                {
                    session.Message = "Invalid sheet name, please download file template from system.";
                    session.isValid = false;
                }
            }//invalid file 
            else
            {
                session.isValid = false;
                session.Message = "[" + extension + "] is not supported.";

            }
            //delete file
            if (saved_file)
            {
                File.Delete(Server.MapPath(file_path));
            }
        }
        else
        {
            session.isValid = false;
            session.Message = "Please select an excel file.";
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">Text message to show user</param>
    /// /// <param name="type">0=Success, 1=Error, 2=warning</param>
    void showMessage(string message, string type)
    {
        if (message.Trim() != "")
        {
            if (type == "0")
            {

                div_message.Attributes.CssStyle.Add("background-color", "#228B22");
            }
            else if (type == "1")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#f00");

            }
            else if (type == "2")
            {
                div_message.Attributes.CssStyle.Add("background-color", "#ffcc00");
            }
            div_message.Attributes.CssStyle.Add("display", "block");
            div_message.InnerHtml = message;
        }
        else
        {
            div_message.Attributes.CssStyle.Add("display", "none");
        }
    }
}