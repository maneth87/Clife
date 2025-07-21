using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using System.IO;
public partial class Pages_PolicyServicing_micro_policy_expiring : System.Web.UI.Page
{
    string userName = "";
    string[] userRole = new string[] { };
    bool roleIsIA = false;
    private DataTable MyData { get { return (DataTable)ViewState["VS_MYDATA"]; } set {  ViewState["VS_MYDATA"]=value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
         MembershipUser myUser = Membership.GetUser();
        lblError.Text = "";
     
        userName = myUser.UserName;
       
        try
        {

            List<bl_sys_user_role> Lrole = (List<bl_sys_user_role>)Session["SS_UR_ROLE"];
            if (Lrole[0].RoleId == "RCOM10" || Lrole[0].RoleId == "RCOM12")
            {
                roleIsIA = true;
            }
            else
            {
                roleIsIA = false;
            }
            //LoadDefault();
            if (!Page.IsPostBack)
            {

                if (roleIsIA)
                {
                    showMessage("", "");
                    txtBranchCode.Attributes.Add("disabled", "disabled");
                }
                else
                {


                    showMessage("", "");
                }

                LoadChannel();
                //Load status             
                Options.Bind(ddlStatus, new DB().GetData(AppConfiguration.GetConnectionString(), "SP_CT_MICRO_POLICY_REPAYMENT_STATUS_GET", new string[,] { }, "micro_policy_expiring.aspx.cs => Page_Load"), "Status", "Status", 0);

                ShowUpdateForm(false);
            }
        }
        catch (Exception ex)
        {
            Helper.Alert(true, ex.Message, lblError);
        }


    }
    void LoadChannel()
    {
        try
        {
            List<bl_channel_location> chList = new List<bl_channel_location>();
            if (roleIsIA) // if (my_sesstion.IsIA)
            {

                #region V2
                bl_channel_sale_agent channel_sale_agent = new bl_channel_sale_agent();
                channel_sale_agent = da_channel.GetChannelSaleAgentByUserName(userName);

                chList = new List<bl_channel_location>();
                chList = da_channel.GetChannelLocationByUser(userName);

                if (chList.Count > 0)
                {
                    
                    foreach (bl_channel_location ch in chList)
                    {
                        ddlBranchName.Items.Add(new ListItem() { Text = ch.Office_Code + " : " + ch.Office_Name, Value = ch.Channel_Location_ID });
                    }

                    string agent_name = da_sale_agent.GetSaleAgentName(channel_sale_agent.SALE_AGENT_ID);
                    hdfSaleAgentID.Value = channel_sale_agent.SALE_AGENT_ID;
                    hdfSaleAgentName.Value = agent_name;

                    ViewState["VS_SALE_AGENT_ID"] = channel_sale_agent.SALE_AGENT_ID;
                    ViewState["VS_SALE_AGENT_NAME"] = agent_name;

                    

                #endregion V2
                }
                else
                {
                    Helper.Alert(false, "Branch location is not found.", lblError);
                    return;
                }
                if (chList.Count > 1)
                {
                    ddlBranchName.Attributes.Remove("disabled");
                }
                else
                {
                    ddlBranchName.Attributes.Add("disabled", "disabled");
                }
            }
            else
            {
                chList = new List<bl_channel_location>();
                chList=  da_channel.GetChannelLocationListByChannelItemID("791D3296-82D0-4F07-AC62-B5C358742E2B");//HTB
            foreach (bl_channel_location ch in chList)
            {
                ddlBranchName.Items.Add(new ListItem() { Text = ch.Office_Name, Value = ch.Channel_Location_ID });
            }
                ddlBranchName.Attributes.Remove("disabled");
               // Helper.BindChanneLocation(ddlBranchName, "791D3296-82D0-4F07-AC62-B5C358742E2B", "Office_Name", "Office_Code");
            }

            ddlBranchName_SelectedIndexChanged(null, null);
        }
        catch (Exception ex)
        {
           // Log.AddExceptionToLog("Error function [LoadChannel()] in class [banca_customer_load.aspx.cs], detial: " + ex.Message + "=>" + ex.StackTrace);

            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = "micro_policy_exipring.aspx.cs",
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName

            });
            Helper.Alert(true, "Loaded channel error. Please contact your system administrator.", lblError);

        }

    }

  

    void LoadData()
    {
        //System.Threading.Thread.Sleep(1000);
        try
        {
            string filter = "";

            filter = txtCustomerNameEnglish.Text.Trim() != "" ? "full_name_en like'%" + txtCustomerNameEnglish.Text.Trim() + "%'" : "";
            filter += txtCustomerNumber.Text.Trim() != "" ? (filter == "" ? " customer_number='" + txtCustomerNumber.Text.Trim() + "'" : " and customer_number='" + txtCustomerNumber.Text.Trim() + "'") : "";
            filter += txtCertificateNumber.Text.Trim() != "" ? (filter == "" ? "Policy_Number='" + txtCertificateNumber.Text.Trim() + "'" : " and Policy_Number='" + txtCertificateNumber.Text.Trim() + "'") : "";

            DataTable exList = new DataTable();

            exList = da_micro_policy_expiring.GetPolicyExpiring("791D3296-82D0-4F07-AC62-B5C358742E2B", hdfChannelLocationID.Value);

            DataTable tblFilter = exList.Clone();
            if (filter != "")
            {

                foreach (DataRow r in exList.Select(filter))
                {
                    tblFilter.ImportRow(r);
                }
                exList = tblFilter;
            }
            if (exList.Rows.Count > 0)
            {
                gv_valid.DataSource = exList;
                Session["SS_EX_POL"] = exList;
                gv_valid.DataBind();
                lblRecords.Text = gv_valid.Rows.Count + " Of " + exList.Rows.Count;

                btnExport.Enabled = true;
               
            }
            else
            {
                gv_valid.DataSource = null;
                gv_valid.DataBind();
                lblRecords.Text = "";
                btnExport.Enabled = false;
            }
            MyData = exList;
        }
        catch (Exception ex)
        {
            gv_valid.DataSource = null;
            gv_valid.DataBind();

            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = "micro_policy_exipring.aspx.cs",
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName

            });
            Helper.Alert(true, ex.Message, lblError);
        }
    }
    protected void gv_valid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label lPolId;
        Label lChannelItemId;
        Label lChannelLocationId;
        Label lNewApplicationNumber;
        Label lPolicyStatus;
        Label lRemark;
        Label lRenewStatus;
        Label lRferrerName;
        Label lExpiryDate;
        Label lCustomerAge;

        HiddenField hProductId;

        string polId = "";
        string channelItemId = "";
        string channelLocationId = "";
        string newApplicationNumber = "";
        string policyStatus = "";
        string renewStatus = "";
        string remarks = "";
        string referrerName="";
        DateTime expiryDate;
        int age = 0;
        string productId = "";
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow g_row;
            GridView g = sender as GridView;
            g_row = g.Rows[index];
            lPolId = (Label)g_row.FindControl("lblPolicyID");
            lChannelItemId = (Label)g_row.FindControl("lblChannelItemId");
            lChannelLocationId = (Label)g_row.FindControl("lblChannelLocationId");
            lNewApplicationNumber = (Label)g_row.FindControl("lblNewApplicationNumber");
            lPolicyStatus = (Label)g_row.FindControl("lblPolicyStatus");
            lRemark = (Label)g_row.FindControl("lblRemarks");
            lRenewStatus = (Label)g_row.FindControl("lblStatus");
            lRferrerName = (Label)g_row.FindControl("lblReferrer");
            lExpiryDate = (Label)g_row.FindControl("lblExpiryDate");
            lCustomerAge = (Label)g_row.FindControl("lblCustomerAge");
            hProductId = (HiddenField)g_row.FindControl("hdfProductId");

            polId = lPolId.Text;
            channelItemId = lChannelItemId.Text;
            channelLocationId = lChannelLocationId.Text;
            newApplicationNumber = lNewApplicationNumber.Text;
            policyStatus = lPolicyStatus.Text;
            hdfPolicyId.Value = polId;
            remarks = lRemark.Text;
            renewStatus = lRenewStatus.Text;
            referrerName = lRferrerName.Text;
            hdfOldStatus.Value = lRenewStatus.Text;

            expiryDate = Helper.FormatDateTime(lExpiryDate.Text);
            age = Convert.ToInt32(lCustomerAge.Text);
            productId = hProductId.Value;
            ViewState["VS_POLICY_ID"] = polId;
            //if (e.CommandName == "CMD_RENEW")
            //{
            //    bl_micro_product_config proConf = new bl_micro_product_config();
            //    proConf = da_micro_product_config.GetProductMicroProduct(productId);

            //    if (hdfChannelItemID.Value.Trim() == "" || hdfChannelLocationID.Value.Trim() == "")
            //    {
            //        Helper.Alert(true, "Branch CODE/NAME is not found.", lblError);
            //    }
            //    else if (hdfSaleAgentID.Value.Trim() == "" || hdfSaleAgentName.Value.Trim() == "")
            //    {
            //        Helper.Alert(true, "Sale agent CODE/NAME is not found.", lblError);
            //    }
            //    else if (age < proConf.Age_Min || age > proConf.Age_Max)
            //    {
            //        Helper.Alert(true, "Age [" + age + "] is not allow.", lblError);
            //    }
            //    //else if (policyStatus.Trim() != "" && policyStatus =="IF")
            //    //{
            //    //    Helper.Alert(false, "Policy status is still In-force, cannot process renewal.", lblError);
            //    //}
            //    else if (hdfOldStatus.Value.Trim().ToUpper() != "AGREE")
            //    {
            //        Helper.Alert(false, "Please get confirmation from cusotmer, before processing renewal.", lblError);
            //    }
            //    else
            //    {

            //        Session["SS_SESSION_PARA"] = new SESSION_PARA()
            //        {
            //            ChannelItemId = hdfChannelItemID.Value,
            //            ChannelLocationId = hdfChannelLocationID.Value,
            //            AgentCode = hdfSaleAgentID.Value,
            //            AgentName = hdfSaleAgentName.Value,
            //            BranchCode = txtBranchCode.Text,
            //            BranchName = ddlBranchName.SelectedItem.Text,
            //            ApplicationNumber = newApplicationNumber,
            //            PolicyStatus = policyStatus,
            //            PolicyExpiryDate = expiryDate
            //        };


            //        string url = "../Business/banca_micro_application_renew.aspx";
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "?p_id=" + polId + "');</script>", false);

            //    }
            //}
            //else if (e.CommandName == "CMD_UPDATE")
            if (e.CommandName == "CMD_UPDATE")
            {
                bl_micro_application app = new bl_micro_application();
                bl_micro_policy pol = new bl_micro_policy();
                try
                {

               
                if (newApplicationNumber != "")
                {
                    

                    Helper.Alert(false, "This policy was coverted to new application [" + newApplicationNumber + "], system is not allowed to change status.", lblError);
                    btnUpdate.Enabled = false;
                    txtRemarks.Enabled = false;
                    ddlStatus.Enabled = false;

                }
                else
                {
                    btnUpdate.Enabled = true;
                    txtRemarks.Enabled = true;
                    ddlStatus.Enabled = true;
                }
                   
                    txtRemarks.Text = remarks;
                    Helper.SelectedDropDownListIndex("VALUE", ddlStatus, remarks == "Over Age" && renewStatus==""  ? "Over Age": renewStatus);
                    ddlStatus.Enabled = remarks == "Over Age" ? false : true;
                    ShowUpdateForm(true);
                }
                catch (Exception ex)
                {
                    Helper.Alert(true, ex.Message, lblError);
                } 
              
            }
        }
        catch (Exception ex)
        {
           
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = "micro_policy_exipring.aspx.cs",
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName

            });
            Helper.Alert(true, ex.Message+ ", please contact your system administrator.", lblError);

        }

    }

    protected void gv_valid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            // when mouse is over the row, save original color to new attribute, and change it to highlight color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

            // when mouse leaves the row, change the bg color to its original value  
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

            Label lbl = (Label)e.Row.FindControl("lblCustomerAge");
            Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
            if (Convert.ToInt32(lbl.Text)>60)
            {
                //lbl.ForeColor = System.Drawing.Color.Red;
                //lbl.Font.Bold = true;
                e.Row.BackColor = System.Drawing.Color.Red;
               // e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#21275b");
                lblRemarks.Text = "Over Age";
            }
            //else
            //{
            //    btn.Enabled = true;
            //}
        }
    }
    protected void gv_valid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_valid.PageIndex = e.NewPageIndex;

        DataTable tbl;
        tbl = (DataTable)Session["SS_EX_POL"];
        gv_valid.DataSource = tbl;
        gv_valid.DataBind();
        /*show record count*/
        lblRecords.Text = gv_valid.Rows.Count + " Of " + tbl.Rows.Count;

    }
   
    protected void ddlBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string[] arr = ddlBranchName.SelectedItem.Text.Split(':');
           
            txtBranchCode.Text = arr[0].Count() > 0 ? arr[0] : "";

            hdfChannelItemID.Value = da_channel.GetChannelItemIDByChannelLocationID(ddlBranchName.SelectedValue);
            hdfChannelLocationID.Value = ddlBranchName.SelectedValue;
            
            LoadData();

        }
        catch (Exception ex)
        {
            Log.SaveLog(new bl_log()
            {
                LogDate = DateTime.Now,
                Class = "micro_policy_exipring.aspx.cs",
                FunctionName = MethodBase.GetCurrentMethod().Name,
                LogType = "ERROR",
                ErrorLine = Log.GetLineNumber(ex),
                Message = ex.Message + "=>" + ex.StackTrace,
                UserName = userName

            });
            Helper.Alert(true, ex.Message, lblError);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData();
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
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedIndex == 0)
        {
            Helper.Alert(true, "Status is required.", lblError);
        }
        else if (txtRemarks.Text.Trim() == "")
        {
            Helper.Alert(true, "Remarks is required.", lblError);
        }
        else
        {
            bl_micro_policy_expiring_status objStatus = new bl_micro_policy_expiring_status();
            objStatus.PolicyId = hdfPolicyId.Value;
            objStatus.Status = ddlStatus.SelectedValue;
            objStatus.Remarks = txtRemarks.Text;
            bool status = false;
            string message="";
            if (hdfOldStatus.Value == "")
            {
                objStatus.CreatedBy = userName;
                objStatus.CreatedOn = DateTime.Now;

                status= da_micro_policy_expiring_status.Save(objStatus, userName);
                message= da_micro_policy_expiring_status.MESSAGE;
                if (status)
                {
                    status = da_micro_policy_expiring_status.SaveHistory(objStatus, userName);
                    message = da_micro_policy_expiring_status.MESSAGE;
                }
               
            }
            else
            {
                objStatus.UpdatedBy = userName;
                objStatus.UpdatedOn = DateTime.Now;
                status= da_micro_policy_expiring_status.Update(objStatus, userName);
                 message= da_micro_policy_expiring_status.MESSAGE;
                 if (status)
                 {
                     objStatus.CreatedBy = objStatus.UpdatedBy;
                     objStatus.CreatedOn = objStatus.UpdatedOn;
                     status = da_micro_policy_expiring_status.SaveHistory(objStatus, userName);
                     message = da_micro_policy_expiring_status.MESSAGE;
                 }

            }
            if(status)
            {
                
             Helper.Alert(false, da_micro_policy_expiring_status.MESSAGE, lblError);
                    ShowUpdateForm(false);
                    hdfPolicyId.Value = "";
                    hdfOldStatus.Value = "";
                    LoadData();
            }
            else
            {
             Helper.Alert(true, message, lblError);
            }
        }
    }
    void ShowUpdateForm(bool status)
    {
        divUpdate.Visible = status;
        divUpdateContent.Visible = status;
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ShowUpdateForm(false);
        hdfPolicyId.Value = "";
        hdfOldStatus.Value = "";
    }
    protected void ibtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (MyData.Rows.Count > 0)
            {

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();

                Response.Clear();
                HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet 1");


                Helper.excel.Sheet = sheet1;
                Helper.excel.Title = new string[] { "Policy Expiring List" };
                Helper.excel.HeaderText = new string[]
                        {
                        "No","Channel","BranchCode","ApplicationID ","CIF","PolicyNo.","CustomerNo.","CustomerName","Gender","Dob","Age","ContactNo.","Product","SumAssure","TotalAmount", "EffectiveDate", "ExpiryDate","ExpiryIn (DAYS)", "PolicyStatus", "RenewalStatus","Remarks","ApplicationNo.","AgentName", "Referrer"
                        };
                Helper.excel.generateHeader();
                int row_no = 0;
                row_no = Helper.excel.NewRowIndex - 1;
                foreach (DataRow r in MyData.Rows)
                {

                    row_no += 1;
                    HSSFRow rowCell = (HSSFRow)sheet1.CreateRow(row_no);

                    HSSFCell Cell1 = (HSSFCell)rowCell.CreateCell(0);
                    Cell1.SetCellValue(row_no - 1);

                    HSSFCell Cell2 = (HSSFCell)rowCell.CreateCell(1);
                    Cell2.SetCellValue(r["Company"].ToString());

                    HSSFCell Cell3 = (HSSFCell)rowCell.CreateCell(2);
                    Cell3.SetCellValue(r["Branch_code"].ToString());

                    HSSFCell Cell4 = (HSSFCell)rowCell.CreateCell(3);
                    Cell4.SetCellValue(r["Application_id"].ToString());

                    HSSFCell Cell5 = (HSSFCell)rowCell.CreateCell(4);
                    Cell5.SetCellValue(r["CIF"].ToString());

                    HSSFCell Cell6 = (HSSFCell)rowCell.CreateCell(5);
                    Cell6.SetCellValue(r["Policy_number"].ToString());

                    HSSFCell Cell7 = (HSSFCell)rowCell.CreateCell(6);
                    Cell7.SetCellValue(r["Customer_number"].ToString());

                    HSSFCell Cell8 = (HSSFCell)rowCell.CreateCell(7);
                    Cell8.SetCellValue(r["Full_name_kh"].ToString());

                    HSSFCell Cell9 = (HSSFCell)rowCell.CreateCell(8);
                    Cell9.SetCellValue(r["Gender"].ToString());

                    HSSFCell Cell10 = (HSSFCell)rowCell.CreateCell(9);
                    Cell10.SetCellValue(Convert.ToDateTime(r["date_of_birth"].ToString()).ToString("dd-MM-yyyy"));

                    HSSFCell Cell11 = (HSSFCell)rowCell.CreateCell(10);
                    Cell11.SetCellValue(Calculation.Culculate_Customer_Age(Convert.ToDateTime(r["date_of_birth"].ToString()).ToString("dd-MM-yyyy"), DateTime.Now));


                    HSSFCell Cell12 = (HSSFCell)rowCell.CreateCell(11);
                    Cell12.SetCellValue(r["Contact_number"].ToString());

                    HSSFCell Cell13 = (HSSFCell)rowCell.CreateCell(12);
                    Cell13.SetCellValue(r["Product_name"].ToString());

                    HSSFCell Cell14 = (HSSFCell)rowCell.CreateCell(13);
                    Cell14.SetCellValue(r["Sum_assure"].ToString());

                    HSSFCell Cell15 = (HSSFCell)rowCell.CreateCell(14);
                    Cell15.SetCellValue(r["total_amount"].ToString());

                    HSSFCell Cell16 = (HSSFCell)rowCell.CreateCell(15);
                    Cell16.SetCellValue(Convert.ToDateTime(r["effective_date"].ToString()).ToString("dd-MM-yyyy"));

                    HSSFCell Cell17 = (HSSFCell)rowCell.CreateCell(16);
                    Cell17.SetCellValue(Convert.ToDateTime(r["expiry_date"].ToString()).ToString("dd-MM-yyyy"));

                    HSSFCell Cell18 = (HSSFCell)rowCell.CreateCell(17);
                    Cell18.SetCellValue(r["expiry_in_current"].ToString());

                    HSSFCell Cell19 = (HSSFCell)rowCell.CreateCell(18);
                    Cell19.SetCellValue(r["Policy_status"].ToString());

                    HSSFCell Cell20 = (HSSFCell)rowCell.CreateCell(19);
                    Cell20.SetCellValue(r["status"].ToString());

                    HSSFCell Cell21 = (HSSFCell)rowCell.CreateCell(20);
                    Cell21.SetCellValue(r["Remarks"].ToString());

                    HSSFCell Cell22 = (HSSFCell)rowCell.CreateCell(21);
                    Cell22.SetCellValue(r["application_number"].ToString());

                    HSSFCell Cell23 = (HSSFCell)rowCell.CreateCell(22);
                    Cell23.SetCellValue(r["agent_name"].ToString());

                    HSSFCell Cell24 = (HSSFCell)rowCell.CreateCell(23);
                    Cell24.SetCellValue(r["referrer"].ToString());


                }
                string filename = "Policy_Expiry_List_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                MemoryStream file = new MemoryStream();
                hssfworkbook.Write(file);


                Response.BinaryWrite(file.GetBuffer());

                Response.End();
            }
        }
        catch (Exception Ex)
        {
            Helper.Alert(true, "Export Data error. Detail:" + Ex.Message, lblError);
        }
    }
}

