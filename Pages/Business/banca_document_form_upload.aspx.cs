using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using iTextSharp.text;


public partial class Pages_Business_banca_document_form_upload : System.Web.UI.Page
{
    string userID = "";
    string userName = "";
    Label application_id;
    Label application_no;
    Label policy_No;
    Label doc_name;
    private bool FileDeleted { get { return (bool)ViewState["FILE_IS_DELETED"]; } set { ViewState["FILE_IS_DELETED"] = value; } }
    private string ErrorMessage { get { return ViewState["ERR_SMS"] + ""; } set { ViewState["ERR_SMS"] = value; } }
    private List<documents.PolicyContract.Preview> docList { get { return (List<documents.PolicyContract.Preview>)ViewState["DOC_LIST"]; } set { ViewState["DOC_LIST"] = value; } }
    private List<string[,]> oldDocList { get { return (List<string[,]>)ViewState["old_Doc_list"]; } set { ViewState["old_Doc_list"] = value; } }
    private List<documents.PolicyContract> existingFile { get { return (List<documents.PolicyContract>)ViewState["exist_file"]; } set { ViewState["exist_file"] = value; } }
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
                txtApplicationNO.Text = "";
                txtCusName.Text = "";
                txtPolicyno.Text = "";

                dvUpload.Attributes.CssStyle.Add("Display", "None");
                dv_show.Attributes.CssStyle.Add("Display", "None");
                dv_preview.Attributes.CssStyle.Add("Display", "None");
            }
        }
        else
        {
            Response.Redirect("../../unauthorize.aspx");
        }


       
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            string app_no = txtApplicationNO.Text.Trim();
            string cus_name = txtCusName.Text.Trim();
            string policy_no = txtPolicyno.Text.Trim();

            if (app_no.Trim() == "" && cus_name.Trim() == "" && policy_no.Trim() == "")
            {
                Helper.Alert(true, "Please Input Application No, Customer Name and Policy No!", lblError);
            }
            else
            {
                Load_Grid(app_no, cus_name, policy_no);

            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }

    }
    protected void Load_Grid(string application_no, string customer_name_eng, string policy_number)
    {

        DataTable tbl = new DataTable();
        DB db = new DB();

        tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POL_GET_SEARCH_BY_APPLICATON_NUMBER", new string[,] { 
        { "@APPLICATON_NUMBER", application_no},
        { "@CUSTOMER_NAME_ENG", customer_name_eng}, 
        {"@POLICY_NO", policy_number } 
        }, "banca_document_from_upload.aspx.cs => Load_Grid(string application_no,string customer_name_eng,string policy_number)");
        //Show data on Gridview
        gv_form_app.DataSource = tbl;
        gv_form_app.DataBind();

        if (tbl.Rows.Count == 0)// when search number is not found
        {
            Helper.Alert(true, "No record(s) found.", lblError);
        }
        else
        { 
        /*save activity log*/
            string desc = txtApplicationNO.Text.Trim() == "" ? "" : string.Concat("App No:", txtApplicationNO.Text.Trim());
            desc += txtCusName.Text.Trim() == "" ? "" : desc == "" ? string.Concat("Customer Name:", txtCusName.Text.Trim()) : string.Concat(", Customer Name:", txtCusName.Text.Trim());
            desc += txtPolicyno.Text.Trim() == "" ? "" : desc == "" ? string.Concat("Pol No:", txtPolicyno.Text.Trim()) : string.Concat(", Pol No:", txtPolicyno.Text.Trim());
           // SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries application for uploading document with criteria [",txtApplicationNO.Text.Trim()=="" ? "": "App No." + txtApplicationNO.Text.Trim(), txtCusName.Text.Trim()=="" ? "" : ", Customer Name:"+ txtCusName.Text.Trim(), txtPolicyno.Text.Trim()=="" ? "": ", Pol No.:"+ txtPolicyno.Text.Trim(),"]."));
            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.INQUIRY, string.Concat("User inquiries application for uploading document with criteria [",desc,"]."));

        }

    }

    protected void gv_form_app_RowCommand(object sender, GridViewCommandEventArgs e)
    {      
        string app_id = "";
        string app_No = "";
        string policy_no = "";

        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow g_row;
            GridView g = sender as GridView;
            g_row = g.Rows[index];
            application_id = (Label)g_row.FindControl("lblapplicationID");
            application_no = (Label)g_row.FindControl("lblApplicationNo");
            policy_No = (Label)g_row.FindControl("lblpolicyNo");
            app_id = application_id.Text;
            policy_no = policy_No.Text;
            app_No = application_no.Text;

            if (e.CommandName == "CMD_UPLORD")
            {
                List<documents.PolicyContract> doc = documents.PolicyContract.GetDocumentList(app_id);
                if (documents.PolicyContract.ErrorMessage != "")
                {
                    Helper.Alert(true, documents.PolicyContract.ErrorMessage, lblError);
                }
                else
                {
                    if (doc.Count > 0)
                    {
                        Helper.Alert(false, "This application is already exist.", lblError);
                        existingFile = doc;
                        foreach (documents.PolicyContract obj in doc)
                        {
                            if (obj.DocCode == "APP")
                            {

                                lbtApp.Text = obj.DocName;

                                hdfPathAPP.Value = documents.PolicyContract.MainPath + obj.DocPath;
                            }

                            if (obj.DocCode == "CERT")
                            {
                                lblCert.Text = obj.DocName;
                                hdfPathCERT.Value = documents.PolicyContract.MainPath + obj.DocPath;
                            }
                            if (obj.DocCode == "ID_CARD")
                            {
                                lblIDCard.Text = obj.DocName;
                                hdfPahtID.Value = documents.PolicyContract.MainPath + obj.DocPath;
                            }
                            if (obj.DocCode == "PAY_SLIP")
                            {
                                lblPaySlip.Text = obj.DocName;
                                hdfPathPaySlip.Value = documents.PolicyContract.MainPath + obj.DocPath;
                            }
                        }

                    }
                    else
                    {
                        existingFile = new List<documents.PolicyContract>();
                    }
                    txtPolicyNumber.Text = policy_no;
                    txtApp_ID.Text = app_id;
                    hdfappid.Value = app_id;
                    hdfappNumber.Value = app_No;
                    dvUpload.Attributes.CssStyle.Remove("Display");
                    txtPolicyNumber.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }

    //function check if not select file
    private bool Valid { get { return (bool)ViewState["V_VALID"]; } set { ViewState["V_VALID"] = value; } }
    private string ErrorSMS { get { return (string)ViewState["V_SMS"]; } set { ViewState["V_SMS"] = value; } }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isUpdat">Set true to validate on update</param>
    private void validate(bool isUpdat = false)
    {
        if (!isUpdat)
        {


            if (!fUploadappform.HasFile || !fUploadcert.HasFile || !fUploadidcard.HasFile || !fUploadpayslip.HasFile)
            {
                Valid = false;
                ErrorSMS = "Application Form,Certificate,ID card,Payslip are Require";

            }
            else if (!documents.PolicyContract.IsCorrectFileType(fUploadappform.PostedFile.FileName))
            {
                Valid = false;
                ErrorSMS = "Application Form, " + documents.PolicyContract.ErrorMessage;
            }
            else if (!documents.PolicyContract.IsCorrectFileType(fUploadcert.PostedFile.FileName))
            {
                Valid = false;
                ErrorSMS = "Certificate, " + documents.PolicyContract.ErrorMessage;
            }
            else if (!documents.PolicyContract.IsCorrectFileType(fUploadidcard.PostedFile.FileName))
            {
                Valid = false;
                ErrorSMS = "ID Card, " + documents.PolicyContract.ErrorMessage;
            }
            else if (!documents.PolicyContract.IsCorrectFileType(fUploadpayslip.PostedFile.FileName))
            {
                Valid = false;
                ErrorSMS = "Payslip, " + documents.PolicyContract.ErrorMessage;
            }
            else if (!documents.PolicyContract.IsCorrectFileSize(fUploadappform.PostedFile.ContentLength, false))
            {
                Valid = false;
                ErrorSMS = "Application Form, " + documents.PolicyContract.ErrorMessage;
            }
            else if (!documents.PolicyContract.IsCorrectFileSize(fUploadcert.PostedFile.ContentLength, false))
            {
                Valid = false;
                ErrorSMS = "Certificate, " + documents.PolicyContract.ErrorMessage;
            }
            else if (!documents.PolicyContract.IsCorrectFileSize(fUploadidcard.PostedFile.ContentLength, false))
            {
                Valid = false;
                ErrorSMS = "ID Card, " + documents.PolicyContract.ErrorMessage;
            }
            else if (!documents.PolicyContract.IsCorrectFileSize(fUploadpayslip.PostedFile.ContentLength, false))
            {
                Valid = false;
                ErrorSMS = "Payslip, " + documents.PolicyContract.ErrorMessage;
            }
            else // Those file for control have file
            {
                Valid = true;
                ErrorSMS = string.Empty;
            }
        }
        else
        {   
       
            if (!fUploadappform.HasFile & !fUploadcert.HasFile & !fUploadidcard.HasFile & !fUploadpayslip.HasFile)
            {
                Valid = false;
                ErrorSMS = "Please choose file Application Form or Certificate or ID card or Payslip to update.";
            }

                else{

                    if (!documents.PolicyContract.IsCorrectFileType(fUploadappform.PostedFile.FileName) && fUploadappform.HasFile)
                    {
                        Valid = false;
                        ErrorSMS = "Application Form, " + documents.PolicyContract.ErrorMessage;

                    }
                    else if (!documents.PolicyContract.IsCorrectFileType(fUploadcert.PostedFile.FileName) && fUploadcert.HasFile)
                    {
                        Valid = false;
                        ErrorSMS = "Certificate, " + documents.PolicyContract.ErrorMessage;
                    }
                    else if (!documents.PolicyContract.IsCorrectFileType(fUploadidcard.PostedFile.FileName)&& fUploadidcard.HasFile)
                    {
                        Valid = false;
                        ErrorSMS = "ID Card, " + documents.PolicyContract.ErrorMessage;
                    }
                    else if (!documents.PolicyContract.IsCorrectFileType(fUploadpayslip.PostedFile.FileName)&& fUploadpayslip.HasFile)
                    {
                        Valid = false;
                        ErrorSMS = "Payslip, " + documents.PolicyContract.ErrorMessage;
                    }
                    else
                    {
                        Valid = true;
                        ErrorSMS = string.Empty;
                    }
            }

            
        }

    }

    private bool updateFile()
    {
        bool result = false;
        string save_path = Server.MapPath("~/Temp/");

        try
        {
            docList = new List<documents.PolicyContract.Preview>();

            string Fname = "";
            string ext = "";
            string FnameNew = "";

            if (fUploadappform.HasFile)
            {
                Fname = Path.GetFileName(fUploadappform.PostedFile.FileName);
                ext = Path.GetExtension(Fname);
                FnameNew = Fname.Replace(Fname, txtPolicyNumber.Text + "-APP" + ext);
                fUploadappform.SaveAs(save_path + FnameNew);
                docList.Add(new documents.PolicyContract.Preview(1, "APP", "PDF", fUploadappform.PostedFile.ContentLength.ToString(), Fname, FnameNew, save_path + FnameNew));
            }

            if (fUploadcert.HasFile)
            {
                Fname = Path.GetFileName(fUploadcert.PostedFile.FileName);
                ext = Path.GetExtension(Fname);
                FnameNew = Fname.Replace(Fname, txtPolicyNumber.Text + "-CERT" + ext);
                fUploadcert.SaveAs(save_path + FnameNew);
                docList.Add(new documents.PolicyContract.Preview(2, "CERT", "PDF", fUploadcert.PostedFile.ContentLength.ToString(), Fname, FnameNew, save_path + FnameNew));
            }
            if (fUploadidcard.HasFile)
            {
                Fname = Path.GetFileName(fUploadidcard.PostedFile.FileName);
                ext = Path.GetExtension(Fname);
                FnameNew = Fname.Replace(Fname, txtPolicyNumber.Text + "-ID" + ext);
                fUploadidcard.SaveAs(save_path + FnameNew);
                docList.Add(new documents.PolicyContract.Preview(3, "ID_CARD", "PDF", fUploadidcard.PostedFile.ContentLength.ToString(), Fname, FnameNew, save_path + FnameNew));
            }

            if (fUploadpayslip.HasFile)
            {
                Fname = Path.GetFileName(fUploadpayslip.PostedFile.FileName);
                ext = Path.GetExtension(Fname);
                FnameNew = Fname.Replace(Fname, txtPolicyNumber.Text + "-PAYSLIP" + ext);
                fUploadpayslip.SaveAs(save_path + FnameNew);
                docList.Add(new documents.PolicyContract.Preview(4, "PAY_SLIP", "PDF", fUploadpayslip.PostedFile.ContentLength.ToString(), Fname, FnameNew, save_path + FnameNew));
            }

            gv_preview.DataSource = docList;
            gv_preview.DataBind();
            dv_preview.Attributes.CssStyle.Remove("Display");
            dv_gird.Attributes.CssStyle.Add("Display", "None");
            //dvSearch.Attributes.CssStyle.Add("Display", "None");
            dvSearch.Attributes.CssStyle.Remove("Display");
                                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User previews the uploading files of Pol No:[",txtPolicyNumber.Text.Trim(),"]."));

        }
        catch (Exception ex)
        {
            result = false;
            Log.AddExceptionToLog("Error function [updateFile()] in page [banca_document_form_upload.aspx.cs], detail:" + ex.Message);
        }
        return result;
    }

    private string GetExtension(string path)
    {
        return Path.GetExtension(path);
    }
    protected void btnPreview_Click(object sender, EventArgs e)
     {
         validate(existingFile.Count > 0 ? true : false);
        if (Valid)
        {
            if (existingFile.Count > 0)
            {
                
                    updateFile();               
            }
            else
            {
                try
                {
                    string save_path = Server.MapPath("~/Temp/");
                    string FnameApp = Path.GetFileName(fUploadappform.PostedFile.FileName);
                    string FnameCert = Path.GetFileName(fUploadcert.PostedFile.FileName);
                    string FnameIdCard = Path.GetFileName(fUploadidcard.PostedFile.FileName);
                    string FnamePayslip = Path.GetFileName(fUploadpayslip.PostedFile.FileName);

                    string extApp = Path.GetExtension(FnameApp);
                    string extCert = Path.GetExtension(FnameCert);
                    string extIdCard = Path.GetExtension(FnameIdCard);
                    string extPayslip = Path.GetExtension(FnamePayslip);

                    string FnameAppNew = FnameApp.Replace(FnameApp, txtPolicyNumber.Text + "-APP" + extApp);
                    string FnameCertNew = FnameCert.Replace(FnameCert, txtPolicyNumber.Text + "-CERT" + extCert);
                    string FnameIdCardNew = FnameIdCard.Replace(FnameIdCard, txtPolicyNumber.Text + "-ID" + extIdCard);
                    string FnamePayslipNew = FnamePayslip.Replace(FnamePayslip, txtPolicyNumber.Text + "-PAYSLIP" + extPayslip);
                    string FnameTc = txtPolicyNumber.Text + "-TC.PDF";


                    // fUploadappform.SaveAs(Server.MapPath(file_path));//save file 
                    fUploadappform.SaveAs(save_path + FnameAppNew);//save file 
                    fUploadcert.SaveAs(save_path + FnameCertNew);
                    fUploadidcard.SaveAs(save_path + FnameIdCardNew);
                    fUploadpayslip.SaveAs(save_path + FnamePayslipNew);

                    //COPY TC to temp folder
                    File.Copy(Server.MapPath("~/upload/tc/SO_DHC_Insurance_Policy.pdf"), save_path + FnameTc, true);
                    //show on grid view
                    docList = new List<documents.PolicyContract.Preview>();

                    documents.PolicyContract.Preview preview;
                    preview = new documents.PolicyContract.Preview(1, "APP", "PDF", fUploadappform.PostedFile.ContentLength.ToString(), FnameApp, FnameAppNew, save_path + FnameAppNew);
                    docList.Add(preview);
                    preview = new documents.PolicyContract.Preview(2, "CERT", "PDF", fUploadcert.PostedFile.ContentLength.ToString(), FnameCert, FnameCertNew, save_path + FnameCertNew);
                    docList.Add(preview);
                    preview = new documents.PolicyContract.Preview(3, "ID_CARD", "PDF", fUploadidcard.PostedFile.ContentLength.ToString(), FnameIdCard, FnameIdCardNew, save_path + FnameIdCardNew);
                    docList.Add(preview);
                    preview = new documents.PolicyContract.Preview(4, "PAY_SLIP", "PDF", fUploadpayslip.PostedFile.ContentLength.ToString(), FnamePayslip, FnamePayslipNew, save_path + FnamePayslipNew);
                    docList.Add(preview);
                    preview = new documents.PolicyContract.Preview(5, "TC", "PDF", "TC.PDF", fUploadappform.PostedFile.ContentLength.ToString(), FnameTc, save_path + FnameTc);
                    docList.Add(preview);

                    gv_preview.DataSource = docList;
                    gv_preview.DataBind();
                    dv_preview.Attributes.CssStyle.Remove("Display");
                    dv_gird.Attributes.CssStyle.Add("Display", "None");
                    dvSearch.Attributes.CssStyle.Remove("Display");

                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User previews the uploading files of Pol No:[",txtPolicyNumber.Text.Trim(),"]."));
                }
                catch (Exception ex)
                {
                    Helper.Alert(true, ex.Message, lblError);
                }
            }
        }

        else
        {
            Helper.Alert(true, ErrorSMS, lblError);
        }
        // }
    }
    protected void gv_preview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label lbDocName;
        try
        {

            if (e.CommandName == "CMD_VIEW")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow g_row;
                GridView g = sender as GridView;
                g_row = g.Rows[index];

                lbDocName = (Label)g_row.FindControl("lblDocName");
              
                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views document [Doc Name:", lbDocName.Text,"]."));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(lbDocName.Text) + "&view_type=single');</script>", false);


            }
            else if (e.CommandName == "CMD_VIEW_ALL")
            {
                byte[] mergedPdf;
                List<Byte[]> myFile = new List<byte[]>();
                string combineDocName = "";
                foreach (documents.PolicyContract.Preview pre in docList)
                {
                    myFile.Add(File.ReadAllBytes(Server.MapPath("~/Temp/" + pre.DocumentName)));
                    combineDocName += combineDocName == "" ? pre.DocumentName : string.Concat(", ", pre.DocumentName);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    iTextSharp.text.Document Mydocument = new iTextSharp.text.Document();
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(Mydocument, ms);
                    Mydocument.Open();
                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.PdfImportedPage page;
                    iTextSharp.text.pdf.PdfReader reader;
                    int file_number = 0;
                    file_number = myFile.Count;
                    int file_index = 0;

                    foreach (byte[] file in myFile)
                    {
                        file_index += 1;
                        reader = new iTextSharp.text.pdf.PdfReader(file);
                        int pages = reader.NumberOfPages;

                        for (int i = 1; i <= pages; i++)
                        {

                            Mydocument.SetPageSize(PageSize.A4);
                            Mydocument.NewPage();
                            page = writer.GetImportedPage(reader, i);
                            cb.AddTemplate(page, 0, 0);
                        }
                    }

                    Mydocument.Close();
                    mergedPdf = ms.GetBuffer();
                    ms.Flush();
                    ms.Close();
                }

                SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.VIEW, string.Concat("User views all documents of Pol No:[", txtPolicyNumber.Text.Trim(), "] at the same time [Doc Name:", combineDocName, "]."));

                string strGenerateFileName = userName + DateTime.Now.ToString("yyMMddhhmmss");
                File.WriteAllBytes(Server.MapPath("~/Temp/" + strGenerateFileName), mergedPdf);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(strGenerateFileName) + "&view_type=multy');</script>", false);

            }

        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string errMessage = "";
        foreach (documents.PolicyContract.Preview doc in docList)
        {
            try
            {
                File.Delete(doc.DocumentPath);
            }
            catch (Exception ex)
            {
                errMessage += ex.Message + "<br />";
            }
        }
        if (errMessage != "")
        {
            Helper.Alert(true, errMessage, lblError);
        }
        else
        {
            docList.Clear();
            gv_preview.DataSource = docList;
            gv_preview.DataBind();
        }
        dv_preview.Attributes.CssStyle.Add("Display", "None");
        SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.CANCEL, string.Concat("User cancels uploading documents of Pol No:[", txtPolicyNumber.Text.Trim(), "]."));
    }

    //block comfirm
    protected void btnUpload_Click(object sender, EventArgs e)
    {

        try
        {
            bool success = true;
            string sms = "";
            documents.PolicyContract.CreateFolder();
            string save_path = documents.PolicyContract.FullPath;
            string combindDocName = "";
            //Update exist file pdf
            if (existingFile.Count > 0)
            {

                try
                {
                    foreach (documents.PolicyContract file in existingFile)
                    {
                        foreach (documents.PolicyContract.Preview doc in docList)
                        {

                            if (file.DocCode == doc.DocumentCode)
                            {
                                File.Copy(doc.DocumentPath, save_path + "\\" + doc.DocumentName, true);
                                documents.PolicyContract.UpdateDocument(file.DocID, doc.DocumentName, doc.DocumentSize, documents.PolicyContract.Path + "\\" + doc.DocumentName, DateTime.Now, userName, "Update File exist by" + " " + userName);
                                //Delete from Temp folder
                                File.Delete(doc.DocumentPath);
                                //delete from real folder or path
                                if(string.Compare(save_path + "\\" + doc.DocumentName,documents.PolicyContract.MainPath + file.DocPath)>0)
                                {
                                    File.Delete(documents.PolicyContract.MainPath + file.DocPath);
                                }
                                //if (!File.Exists(save_path + "\\" + doc.DocumentName))
                                //{
                                //    File.Delete(documents.PolicyContract.MainPath + file.DocPath);
                                    
                                //}
                                combindDocName += combindDocName=="" ?  string.Concat( doc.DocumentName):string.Concat(", ", doc.DocumentName);
                                break;

                            }
                        }
                    }                   
                    sms = "Files Update Successfully!.";
                    SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPDATE, string.Concat("User updates ", docList.Count, " documents. of [Pol No:", txtPolicyNumber.Text.Trim(), " Doc Name:", combindDocName, "] "));
                }
                catch (Exception ex)
                {
                    success = false;
                    sms = ex.Message;
                }
            }
            else // new data
            {
                if (documents.PolicyContract.CreatedFolder)
                {
                    if (docList.Count > 0)
                    {
                        //copy file
                        try
                        {
                            //Log.AddExceptionToLog("start transection");
                            foreach (documents.PolicyContract.Preview doc in docList)
                            {

                                //Log.AddExceptionToLog("Ready...");
                                File.Copy(doc.DocumentPath, save_path + "\\" + doc.DocumentName, true);
                                InsertData(doc.Seq, hdfappid.Value, doc.DocumentCode, doc.DocumentName, doc.DocumentType, doc.DocumentSize, documents.PolicyContract.Path + "\\" + doc.DocumentName, DateTime.Now, userName);
                                File.Delete(doc.DocumentPath);
                                //Log.AddExceptionToLog("=>" + doc.DocumentCode);
                                combindDocName += string.Concat(", ", doc.DocumentName);
                            }
                            sms = "Files Upload Successfully!.";
                            SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.SAVE, string.Concat("User saves ", docList.Count, " documents. of [Pol No:", txtPolicyNumber.Text.Trim(), " Doc Name:", combindDocName, "] "));

                        }
                        catch (Exception ex)
                        {         
                            success=false;
                            sms = ex.Message;
                            Log.AddExceptionToLog("Erro In function [btnUpload_Click] in page [banca_document_form_upload.aspx.cs],detail: " + ex.StackTrace + "=>" + ex.Message);
                        }
                    }
                }
                else
                {
                    Log.AddExceptionToLog("Error Create folder.");
                    Helper.Alert(true, documents.PolicyContract.ErrorMessage, lblError);
                }
            }

            if (success)
            {
                ShowDataUpload(hdfappid.Value);
                dv_show.Attributes.CssStyle.Remove("display");
                dvSearch.Attributes.CssStyle.Remove("display");
                dvUpload.Attributes.CssStyle.Add("Display", "None");
                dv_preview.Attributes.CssStyle.Add("Display", "None");


                //clear data in preview
                gv_preview.DataSource = null;
                gv_preview.DataBind();

                Helper.Alert(false, sms, lblError);
            }
            else
            {
                Helper.Alert(true, sms, lblError);
            }
        }

        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);

        }
    }

    public bool InsertData(Int32 seq, string app_id, string doc_code, string doc_name, string doc_type, string doc_size, string doc_path, DateTime created_on, string created_by, string created_remarks = "")
    {
        bool result = false;
        result = documents.PolicyContract.InsertData(seq, app_id, doc_code, doc_name, doc_type, doc_size, doc_path, created_on, created_by, created_remarks);
        return result;
    }
    protected void ShowDataUpload(string application_id)
    {

        DataTable tbl = new DataTable();
        DB db = new DB();

        try
        {
            tbl = db.GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POL_CONTRACT_DOC_LOADDATA", new string[,]{
        {"@APPLICATION_ID",application_id},    
        }, "banca_document_form_upload=>ShowData(string application_id)");

            if (tbl.Rows.Count > 0)
            {
                gv_show_upload.DataSource = tbl;
                gv_show_upload.DataBind();
            }
            else
            {
                gv_show_upload.DataSource = null;
                gv_show_upload.DataBind();
            }

        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }

    protected void lbtApp_Click(object sender, EventArgs e)
    {

        try
        {

            File.Copy((hdfPathAPP.Value), Server.MapPath("~/Temp/" + lbtApp.Text), true);      
            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(lbtApp.Text) + "&view_type=multy');</script>", false);
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void lblCert_Click(object sender, EventArgs e)
    {
        try
        {

            File.Copy((hdfPathCERT.Value), Server.MapPath("~/Temp/" + lblCert.Text), true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(lblCert.Text) + "&view_type=multy');</script>", false);
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void lblIDCard_Click(object sender, EventArgs e)
    {
        try
        {

            File.Copy((hdfPahtID.Value), Server.MapPath("~/Temp/" + lblIDCard.Text), true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(lblIDCard.Text) + "&view_type=multy');</script>", false);


        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void lblPaySlip_Click(object sender, EventArgs e)
    {
        try
        {
            File.Copy((hdfPathPaySlip.Value), Server.MapPath("~/Temp/" + lblPaySlip.Text), true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('banca_document_view.aspx?doc_name=" + ResolveClientUrl(lblPaySlip.Text) + "&view_type=multy');</script>", false);
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
 
        gv_form_app.DataSource = null;
        gv_form_app.DataBind();
        gv_show_upload.DataSource = null;
        gv_show_upload.DataBind();     
        gv_preview.DataSource = null;
        gv_preview.DataBind();
        txtApplicationNO.Text = "";
        txtCusName.Text = "";
        txtPolicyno.Text = "";
        dv_show.Attributes.CssStyle.Add("Display", "None");
        dvUpload.Attributes.CssStyle.Add("Display", "None");
        dv_preview.Attributes.CssStyle.Add("Display", "None");
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);

    }
}