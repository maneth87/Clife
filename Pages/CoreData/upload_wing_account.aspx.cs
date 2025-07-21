using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_upload_wing_account : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        string user_id = myUser.ProviderUserKey.ToString();
        string user_name = myUser.UserName;

        //bind user name and user id to hiddenfield
        hdfuserid.Value = user_id;
        hdfusername.Value = user_name;

        lblMessage.Text = "";

        // Search Account Wing

        string wing_sk = txtSkNumberSearch.Text.Trim();
        string wing_num = txtWingNumberSearch.Text.Trim();

        LoadData(wing_sk, wing_num);
      
    }


    private void LoadData(string wing_sk, string wing_num)
    {
        IList<bl_wing_account> wing_account = new List<bl_wing_account>();

        wing_account = da_wing_account.GetAllWingAccount(wing_sk, wing_num);

        if (wing_account.Count == 0)
        {
            if (wing_sk != "" || wing_num != "")
            {

                result.Text = "Search Not Found !";
                result.ForeColor = System.Drawing.Color.Red;

            
            }else{

            result.Text = "Empty Data !";
            result.ForeColor = System.Drawing.Color.Red;

            }
        }
        else {

            if(wing_sk !="" || wing_num !=""){


                result.Text = "Total Search = ( " + wing_account.Count.ToString() + " )";
                result.ForeColor = System.Drawing.Color.Black;

            }else{

            result.Text ="Total = ( " + wing_account.Count.ToString() + " )";
            result.ForeColor = System.Drawing.Color.Black;

            }

        }

        GvWing.DataSource = wing_account;
        GvWing.DataBind();

    }

    protected void ImgBtnUpload_Click(object sender, ImageClickEventArgs e)
    {

        int insert_result = 0;

        lblMessage.Text = "";
          
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi2 = new DateTimeFormatInfo();
        dtfi2.ShortDatePattern = "dd/MM/yyyy";
        dtfi2.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";

        try
        {

            //check if file upload contain any file                      
            if ((FileUploadWingAccount.PostedFile != null) && !string.IsNullOrEmpty(FileUploadWingAccount.PostedFile.FileName))
            {

                string save_path = "~/Upload/Wing/";
                string file_name = Path.GetFileName(FileUploadWingAccount.PostedFile.FileName);
                string content_type = FileUploadWingAccount.PostedFile.ContentType;
                int content_length = FileUploadWingAccount.PostedFile.ContentLength;

                FileUploadWingAccount.PostedFile.SaveAs(Server.MapPath(save_path + file_name));
                string version = Path.GetExtension(file_name);

                string file_path = Server.MapPath(save_path + file_name).ToString();

                //verify if the file has been save           
                if (version == ".xls")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" + file_path + "';Extended Properties=Excel 8.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[Sheet1$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //Date Request
                        if (dt.Rows[i][0].ToString().Trim() != "")
                        {
                            if (!Helper.CheckDateFormat(dt.Rows[i][0].ToString()))
                            {
                                lblMessage.Text = "Please check your input for Date of Request field then try again. Row number: '" + (i + 1);
                                
                                return;
                            }

                        }
                        else
                        {
                            lblMessage.Text = "Please check your input for Date of Request field then try again. Row number: '" + (i + 1);
                            return;
                        }

                        //SK
                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for SK field then try again. Row number: '" + (i + 1);
                            return;
                        }

                        //Wing Number
                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Wing Number field then try again. Row number: '" + (i + 1);
                            return;
                        }                                                                          
                    }

                 ArrayList  result = new ArrayList();
                    
                    //loop excel file rows to save wing account
                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {

                        DateTime date_request = Convert.ToDateTime(dt.Rows[k][0].ToString().Trim(), dtfi2);
                        string sk = dt.Rows[k][1].ToString().Trim();
                        string wing_num = dt.Rows[k][2].ToString().Trim();                              

                         //Add wing account

                        bl_wing_account wing_account = new bl_wing_account();
                         
                        wing_account.Date_Request = date_request;
                        wing_account.Sk =sk ;
                        wing_account.Wing_Number =wing_num;
                        wing_account.Created_On =DateTime.Now;
                        wing_account.Created_By = hdfusername.Value;
                        wing_account.Status =1;
                       bool   insert =da_wing_account.InsertWingAccount(wing_account);     
               
                       if(insert){

                          result.Add(1);
                          insert_result += 1;
                       }else {

                          result.Add(0);

                       }

                    }//end loop

                    if (insert_result == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('All new wing accounts saving failed.')", true);

                    }
                    else if (insert_result < result.Count)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Some wing accounts saved successfully.')", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('All new wing accounts saved successfully.')", true);
                    }

                    ShowResult(result,dt);
                
                }
                else if (version == ".xlsx")
                {
                    System.Data.OleDb.OleDbConnection MyConnection = null;
                    System.Data.DataSet DtSet = null;
                    System.Data.OleDb.OleDbDataAdapter MyCommand = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + file_path + "';Extended Properties=Excel 12.0;");
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                    DtSet = new System.Data.DataSet();
                    MyCommand.Fill(DtSet, "[Sheet1$]");
                    MyConnection.Close();

                    DataTable dt = null;
                    dt = DtSet.Tables[0];
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //Date Request
                        
                        if (dt.Rows[i][0].ToString().Trim() != "")
                        {
                            if (!Helper.CheckDateFormat(dt.Rows[i][0].ToString()))
                            {
                                lblMessage.Text = "Please check your input for Date of Request field then try again. Row number: '" + (i + 1);
                                return;
                            }

                        }
                        else
                        {
                            lblMessage.Text = "Please check your input for Date of Request field then try again. Row number: '" + (i + 1);
                            return;
                        }

                        //SK
                        if (dt.Rows[i][1].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for SK field then try again. Row number: '" + (i + 1);
                            return;
                        }

                        //Wing Number
                        if (dt.Rows[i][2].ToString().Trim().Equals(""))
                        {
                            lblMessage.Text = "Please check your input for Wing Number field then try again. Row number: '" + (i + 1);
                            return;
                        }                                                                          
                    }

                 ArrayList  result = new ArrayList();
                    
                
                    //loop excel file rows to save wing account
                    for (int k = 0; k <= dt.Rows.Count - 1; k++)
                    {

                        DateTime date_request = Convert.ToDateTime(dt.Rows[k][0].ToString().Trim(), dtfi2);
                        string sk = dt.Rows[k][1].ToString().Trim();
                        string wing_num = dt.Rows[k][2].ToString().Trim();                              

                         //Add wing account

                        bl_wing_account wing_account = new bl_wing_account();
                         
                        wing_account.Date_Request = date_request;
                        wing_account.Sk =sk ;
                        wing_account.Wing_Number =wing_num;
                        wing_account.Created_On =DateTime.Now;
                        wing_account.Created_By = hdfusername.Value;
                        wing_account.Status =1;

                       bool   insert =da_wing_account.InsertWingAccount(wing_account);     
               
                       if(insert){

                          result.Add(1);
                          insert_result += 1;
                       }else {

                          result.Add(0);                          
                       }

                    }//end loop

                    
                    if (insert_result == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('All new wing accounts saving failed.')", true);

                    }
                    else if (insert_result < result.Count)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Some wing accounts saved successfully.')", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('All new wing accounts saved successfully.')", true);
                    }

                    ShowResult(result,dt);     
                }
              }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please upload an excel file that contains wing account data.')", true);
         
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Please contact system admin for problem diagnosis.')", true);

            Log.AddExceptionToLog("Error in function [ImgBtnUpload_Click], page [upload_wing_account_]. Details: " + ex.Message);
        }

    }

    private void ShowResult(ArrayList result, DataTable mydt)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "MM/dd/yyyy";
        dtfi.DateSeparator = "/";

        int re_row = 0;

        for (int i = 1; i <= result.Count; i++)
        {

            TableRow row = new TableRow();

            TableCell cell1 = new TableCell();
            cell1.Style.Add("text-align", "center");
            cell1.Text = i.ToString();

            TableCell cell2 = new TableCell();
            cell2.Style.Add("text-align", "left");
            cell2.Style.Add("padding-left", "5px");
            cell2.Text = mydt.Rows[re_row][0].ToString().Trim();

            TableCell cell3 = new TableCell();
            cell3.Style.Add("text-align", "left");
            cell3.Style.Add("padding-left", "5px");
            cell3.Text = mydt.Rows[re_row][1].ToString().Trim();

            TableCell cell4 = new TableCell();
            cell4.Style.Add("text-align", "left");
            cell4.Style.Add("padding-left", "5px");
            cell4.Text = mydt.Rows[re_row][2].ToString().Trim();

            TableCell cell5 = new TableCell();

            cell5.Style.Add("text-align", "center");
           
            if (result[re_row].ToString() == "0") //no row saved
            {
                cell5.Style.Add("color", "red");
                cell5.Text = "X";
            }
            else
            {
                
                    cell5.Style.Add("color", "green");
                    cell5.Text = "\u221A"; 
                
            }

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
            row.Cells.Add(cell4);
            row.Cells.Add(cell5);

            tblResult.Rows.Add(row);

            re_row += 1;

        }

    }

    // Function Search Wing Account
    protected void btnSearchWing_Click(object sender, EventArgs e)
    {
        string wing_sk = txtSkNumberSearch.Text.Trim();
        string wing_num = txtWingNumberSearch.Text.Trim();

        LoadData(wing_sk, wing_num);
    }
    protected void GvWing_PageIndexChanged(object sender, EventArgs e)
    {
        string wing_sk = txtSkNumberSearch.Text.Trim();
        string wing_num = txtWingNumberSearch.Text.Trim();

        LoadData(wing_sk, wing_num);
    }
    protected void GvWing_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvWing.PageIndex = e.NewPageIndex;
    }

    protected void btnEditWing_Click(object sender, EventArgs e)
    {
        string wing_sk = txtEditWingSk.Text.Trim();

        string wing_num = txtEditWingNum.Text.Trim();

        string hdf_Sk= hdftxtEditWingSk.Value;
        string hdf_Num = hdftxtEditWingNum.Value;
     
        if(hdf_Sk == wing_sk){

            //update wing number
            bool update_num = da_wing_account.UpdateWingNumber(wing_sk, wing_num);
            
            if (update_num)
            {
                Response.Redirect("~/Pages/CoreData/upload_wing_account.aspx");
            }

        }
        else if (hdf_Num == wing_num)
        {

            //update wing sk
            bool update_sk = da_wing_account.UpdateWingSK(wing_sk, hdf_Num);

            if (update_sk)
            {

                Response.Redirect("~/Pages/CoreData/upload_wing_account.aspx");
               
            }
        }
            // when update both wing sk and wing number
        else {

            //update wing sk and wing number
            bool update_sk=  da_wing_account.UpdateWingSK(wing_sk, hdf_Num);

             if(update_sk){

            bool update_num = da_wing_account.UpdateWingNumber(wing_sk, wing_num);

            if (update_num)
            {

                Response.Redirect("~/Pages/CoreData/upload_wing_account.aspx");
               
                 }
              }
        
        }
    
    }
}