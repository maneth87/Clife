using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class Pages_CoreData_sale_agent : System.Web.UI.Page
{
    bl_sale_agent sale_agent = new bl_sale_agent();
    bl_sale_agent_contact sale_agent_contact = new bl_sale_agent_contact();
    bl_sale_agent_ordinary sale_agent_ordinary = new bl_sale_agent_ordinary();
    int check_sale_id = 0, check_sale_type = 0, check_id_card = 0;
    string userid, user_name,sale_type;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            ViewState["SortExpression"] = "Sale_Agent_ID";

        }

        if (txtSearchSaleAgentCode.Text == "")
        {
            GvSaleAgent.DataSource = da_sale_agent.GetSaleAgentList();
            GvSaleAgent.DataBind();
        }
        else
        {
            GvSaleAgent.DataSource = da_sale_agent.GetSaleAgent_By_SaleAgentCode(txtSearchSaleAgentCode.Text);
            GvSaleAgent.DataBind();
        }
    }

    void ClearTextSave()
    {
        ///Sale Agent
        txtSaleAgentID.Text = "";
        txtLastName.Text = "";
        txtFirstName.Text = "";
        txtKhmerFirstName.Text = "";
        txtKhmerLastName.Text = "";
        txtNote.Text = "";

        /// Sale Agent Contact
        txtMobileNo.Text = "";
        txtPhoneNo.Text = "";
        txtFaxNo.Text = "";
        txtEmail.Text = "";

        /// Sale Agetn Ordinary
        txtIDNo.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtBirth_Date.Text = "";

        /// Sale Agent
        txtBankSaleID.Text = "";
        txtBankFullName.Text = "";
        txtKhmerBankFullName.Text = "";
        txtBankNote.Text = "";

        ///  Sale Agent Contact
        txtBankMobile.Text = "";
        txtBankPhone.Text = "";
        txtBankFax.Text = "";
        txtBankEmail.Text = "";

        /// Sale Agent
        txtBrokerSaleID.Text = "";
        txtBrokerFullName.Text = "";
        txtKhmerBrokerFullName.Text = "";
        txtBrokerNote.Text = "";

        ///  Sale Agent Contact
        txtBrokerMobile.Text = "";
        txtBrokerPhone.Text = "";
        txtBrokerFax.Text = "";
        txtBrokerEmail.Text = "";


    }

    /// <summary>
    /// Insert
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (hdfEditSaleID.Value == "" && hdfEditBankSaleID.Value == "" && hdfEditBrokerSaleID.Value == "")
        {
            MembershipUser myUser = Membership.GetUser();
            userid = myUser.ProviderUserKey.ToString();
            user_name = myUser.UserName;


            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            if (int.Parse(ddlSaleAgentType.SelectedValue) == 0) /// Ordinary Agent
            {
                ///Sale Agent
                sale_type = "Ordinary Agent";
                sale_agent.Sale_Agent_ID = txtSaleAgentID.Text.ToUpper();
                sale_agent.Sale_Agent_Type = 0;
                sale_agent.Full_Name = txtLastName.Text.ToUpper() + ' ' + txtFirstName.Text.ToUpper();
                sale_agent.Khmer_Full_Name = txtKhmerLastName.Text + " " + txtKhmerFirstName.Text;
                sale_agent.Status = 0; /// New 0, delete -1
                sale_agent.Created_On = DateTime.Now;
                sale_agent.Created_By = user_name;
                sale_agent.Created_Note = txtNote.Text;

                /// Sale Agent Contact
                sale_agent_contact.Mobile_Phone1 = txtMobileNo.Text;
                sale_agent_contact.Mobile_Phone2 = "";
                sale_agent_contact.Home_Phone1 = txtPhoneNo.Text;
                sale_agent_contact.Home_Phone2 = "";
                sale_agent_contact.Office_Phone1 = "";
                sale_agent_contact.Office_Phone2 = "";
                sale_agent_contact.Fax1 = txtFaxNo.Text;
                sale_agent_contact.Fax2 = "";
                sale_agent_contact.EMail = txtEmail.Text;

                /// Sale Agetn Ordinary
                sale_agent_ordinary.ID_Card = txtIDNo.Text.ToUpper();
                sale_agent_ordinary.ID_Type = int.Parse(ddlIDType.SelectedValue);
                sale_agent_ordinary.First_Name = txtFirstName.Text.ToUpper();
                sale_agent_ordinary.Last_Name = txtLastName.Text.ToUpper();
                sale_agent_ordinary.Khmer_First_Name = txtKhmerFirstName.Text.Trim();
                sale_agent_ordinary.Khmer_Last_Name = txtKhmerLastName.Text.Trim();

                sale_agent_ordinary.Gender = int.Parse(ddlSex.SelectedValue);
                sale_agent_ordinary.Birth_Date = DateTime.Parse(txtBirth_Date.Text,dtfi);
                sale_agent_ordinary.Country_ID = ddlNationality.Text;
            }
            else if (int.Parse(ddlSaleAgentType.SelectedValue) == 1)///  Bank Agent 
            {
                sale_type = "Bank Agent";
                /// Sale Agent
                sale_agent.Sale_Agent_ID = txtBankSaleID.Text.ToUpper();
                sale_agent.Sale_Agent_Type = 1;
                sale_agent.Full_Name = txtBankFullName.Text.ToUpper();
                sale_agent.Khmer_Full_Name = txtKhmerBankFullName.Text.Trim();
                sale_agent.Status = 0; /// New 0, delete -1
                sale_agent.Created_On = DateTime.Now;
                sale_agent.Created_By = user_name;
                sale_agent.Created_Note = txtBankNote.Text;

                ///  Sale Agent Contact
                sale_agent_contact.Mobile_Phone1 = txtBankMobile.Text;
                sale_agent_contact.Mobile_Phone2 = "";
                sale_agent_contact.Home_Phone1 = txtBankPhone.Text;
                sale_agent_contact.Home_Phone2 = "";
                sale_agent_contact.Office_Phone1 = "";
                sale_agent_contact.Office_Phone2 = "";
                sale_agent_contact.Fax1 = txtBankFax.Text;
                sale_agent_contact.Fax2 = "";
                sale_agent_contact.EMail = txtBankEmail.Text;
            }
            else ///  Broker Agent
            {
                sale_type = "Broker Agent";
                /// Sale Agent
                sale_agent.Sale_Agent_ID = txtBrokerSaleID.Text.ToUpper();
                sale_agent.Sale_Agent_Type = 2;
                sale_agent.Full_Name = txtBrokerFullName.Text.ToUpper();
                sale_agent.Khmer_Full_Name = txtKhmerBrokerFullName.Text.Trim();
                sale_agent.Status = 0; /// New 0, delete -1
                sale_agent.Created_On = DateTime.Now;
                sale_agent.Created_By = user_name;
                sale_agent.Created_Note = txtBrokerNote.Text;

                ///  Sale Agent Contact
                sale_agent_contact.Mobile_Phone1 = txtBrokerMobile.Text;
                sale_agent_contact.Mobile_Phone2 = "";
                sale_agent_contact.Home_Phone1 = txtBrokerPhone.Text;
                sale_agent_contact.Home_Phone2 = "";
                sale_agent_contact.Office_Phone1 = "";
                sale_agent_contact.Office_Phone2 = "";
                sale_agent_contact.Fax1 = txtBrokerFax.Text;
                sale_agent_contact.Fax2 = "";
                sale_agent_contact.EMail = txtBrokerEmail.Text;
            }

            /////  Check Duplicate Sale ID
            //if (da_sale_agent.Check_Duplicate(sale_agent, sale_agent_ordinary, 0) == true)
            //{
            //    check_sale_id = 1;
            //}
            ///// Check Duplicate Sale Type
            //if (da_sale_agent.Check_Duplicate(sale_agent, sale_agent_ordinary, 1) == true)
            //{
            //    check_sale_type = 1;
            //}
            ///// Check Duplicate ID Card
            //if (da_sale_agent.Check_Duplicate(sale_agent, sale_agent_ordinary, 2) == true)
            //{
            //    check_id_card = 1;
            //}

            //if (check_sale_type == 1 && check_sale_id == 0 && check_id_card == 0)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Sale Agent Type (" + sale_type + ") has already existed. Please check it again.')", true);
            //}

            //if (check_sale_type == 0 && check_sale_id == 1 && check_id_card == 0)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Sale Agent ID (" + sale_agent.Sale_Agent_ID + ") has already existed. Please check it again.')", true);
            //}

            //if (check_sale_type == 0 && check_sale_id == 0 && check_id_card == 1)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The ID Card (" + sale_agent_ordinary.ID_Card + ") has already existed. Please check it again.')", true);
            //}

            //if (check_sale_type == 1 && check_sale_id == 1 && check_id_card == 1)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Sale Agent Type (" + sale_type + "), Sale Agend ID (" + sale_agent.Sale_Agent_ID + ") , and ID Card (" + sale_agent_ordinary.ID_Card + ") have already existed. Please check them again.')", true);
            //}

            //if (check_sale_type == 1 && check_sale_id == 1 && check_id_card == 0)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Sale Agent Type (" + sale_type + ") and Sale Agend ID (" + sale_agent.Sale_Agent_ID + ") have already existed. Please check them again.')", true);
            //}

            //if (check_sale_type == 0 && check_sale_id == 1 && check_id_card == 1)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Sale Agend ID (" + sale_agent.Sale_Agent_ID + ") , and ID Card (" + sale_agent_ordinary.ID_Card + ") have already existed. Please check them again.')", true);
            //}

            //if (check_sale_type == 1 && check_sale_id == 0 && check_id_card == 1)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Sale Agend Type (" + sale_agent.Sale_Agent_Type + ") , and ID Card (" + sale_agent_ordinary.ID_Card + ") have already existed. Please check them again.')", true);
            //}

            ///Insert into Ct_Sale_Agent, Ct_Sale_Agent_Contact, Ct_Sale_Agent_Ordinary

            if (da_sale_agent.InsertSale_Agent(sale_agent, sale_agent_contact, sale_agent_ordinary) == false)
            {
                if (sale_agent.Sale_Agent_Type == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The process of Add New Sale Ordinary Agent is unsuccessful. Please check it again.')", true);
                }
                else if (sale_agent.Sale_Agent_Type == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The process of Add New Sale Bank Agent is unsuccessful. Please check it again.')", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The process of Add New Sale Broker Agent is unsuccessful. Please check it again.')", true);
                }
            }

            ClearTextSave();

            GvSaleAgent.DataBind();
        }
        else { btnEdit_Click(sender, e); }
    }

    void ClearTextEdit()
    {
        /// Sale Agent
        txtEditLastName.Text = "";
        txtEditFirstName.Text = "";
        txtEditNote.Text = "";

        /// Sale Agent Contact
        txtEditMobileNo.Text = "";
        txtEditPhoneNo.Text = "";
        txtEditFaxNo.Text = "";
        txtEditEmail.Text = "";

        /// Sale Agetn Ordinary
        txtEditIDNo.Text = "";
        txtEditFirstName.Text = "";
        txtEditLastName.Text = "";
        txtEditBirth_Date.Text = "";

        /// Sale Agent
        txtEditBankFullName.Text = "";
        txtBankNote.Text = "";

        /// Sale Agent Contact
        txtEditBankMobile.Text = "";
        txtEditBankPhone.Text = "";
        txtEditBankFax.Text = "";
        txtEditBankEmail.Text = "";

        /// Sale Agent
        txtEditBrokerFullName.Text = "";
        txtEditBrokerNote.Text = "";

        /// Sale Agent Contact
        txtEditBrokerMobile.Text = "";
        txtEditBrokerPhone.Text = "";
        txtEditBrokerFax.Text = "";
        txtEditBrokerEmail.Text = "";

    }

    /// <summary>
    /// Update
    /// </summary>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";


        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        if (int.Parse(hdfDDLEditSaleType.Value) == 0) /// Ordinary Agent
        {
            /// Sale Agent
            sale_agent.Sale_Agent_ID = hdfEditSaleID.Value.ToUpper(); //txtEditSaleAgentID.Text;
            sale_agent.Sale_Agent_Type = 0;
            sale_agent.Full_Name = txtEditLastName.Text.ToUpper() + ' ' + txtEditFirstName.Text.ToUpper();
            sale_agent.Khmer_Full_Name = txtEditKhmerLastName.Text.Trim() + " " + txtEditKhmerFirstName.Text.Trim();
            sale_agent.Status = 0; /// New 0, delete -1
            sale_agent.Created_On = DateTime.Now;
            sale_agent.Created_By = user_name;
            sale_agent.Created_Note = txtEditNote.Text;

            /// Sale Agent Contact
            sale_agent_contact.Mobile_Phone1 = txtEditMobileNo.Text;
            sale_agent_contact.Mobile_Phone2 = "";
            sale_agent_contact.Home_Phone1 = txtEditPhoneNo.Text;
            sale_agent_contact.Home_Phone2 = "";
            sale_agent_contact.Office_Phone1 = "";
            sale_agent_contact.Office_Phone2 = "";
            sale_agent_contact.Fax1 = txtEditFaxNo.Text;
            sale_agent_contact.Fax2 = "";
            sale_agent_contact.EMail = txtEditEmail.Text;

            /// Sale Agetn Ordinary
            sale_agent_ordinary.ID_Card = txtEditIDNo.Text.ToUpper();
            sale_agent_ordinary.ID_Type = int.Parse(ddlEditIDType.SelectedValue);
            sale_agent_ordinary.First_Name = txtEditFirstName.Text.ToUpper();
            sale_agent_ordinary.Last_Name = txtEditLastName.Text.ToUpper();
            sale_agent_ordinary.Khmer_First_Name = txtEditKhmerFirstName.Text.Trim();
            sale_agent_ordinary.Khmer_Last_Name = txtEditKhmerLastName.Text.Trim();
            sale_agent_ordinary.Gender = int.Parse(ddlEditSex.SelectedValue);
            sale_agent_ordinary.Birth_Date = DateTime.Parse(txtEditBirth_Date.Text, dtfi);
            sale_agent_ordinary.Country_ID = ddlEditNationality.Text;

        }
        else if (int.Parse(hdfDDLEditSaleType.Value) == 1) ///  Bank Agent 
        {
            /// Sale Agent
            sale_agent.Sale_Agent_ID = hdfEditBankSaleID.Value.ToUpper();//txtEditBankSaleID.Text;
            sale_agent.Sale_Agent_Type = 1;
            sale_agent.Full_Name = txtEditBankFullName.Text.ToUpper();
            sale_agent.Khmer_Full_Name = txtKhmerEditBankFullName.Text.Trim();
            sale_agent.Status = 0; /// New 0, delete -1
            sale_agent.Created_On = DateTime.Now;
            sale_agent.Created_By = user_name;
            sale_agent.Created_Note = txtBankNote.Text;

            /// Sale Agent Contact
            sale_agent_contact.Mobile_Phone1 = txtEditBankMobile.Text;
            sale_agent_contact.Mobile_Phone2 = "";
            sale_agent_contact.Home_Phone1 = txtEditBankPhone.Text;
            sale_agent_contact.Home_Phone2 = "";
            sale_agent_contact.Office_Phone1 = "";
            sale_agent_contact.Office_Phone2 = "";
            sale_agent_contact.Fax1 = txtEditBankFax.Text;
            sale_agent_contact.Fax2 = "";
            sale_agent_contact.EMail = txtEditBankEmail.Text;
        }
        else ///&& Broker Agent
        {
            /// Sale Agent
            sale_agent.Sale_Agent_ID = hdfEditBrokerSaleID.Value.ToUpper(); //txtEditBrokerSaleID.Text;
            sale_agent.Sale_Agent_Type = 2;
            sale_agent.Full_Name = txtEditBrokerFullName.Text.ToUpper();
            sale_agent.Khmer_Full_Name = txtKhmerEditBrokerFullName.Text.Trim();
            sale_agent.Status = 0; /// New 0, delete -1
            sale_agent.Created_On = DateTime.Now;
            sale_agent.Created_By = user_name;
            sale_agent.Created_Note = txtEditBrokerNote.Text;

            /// Sale Agent Contact
            sale_agent_contact.Mobile_Phone1 = txtEditBrokerMobile.Text;
            sale_agent_contact.Mobile_Phone2 = "";
            sale_agent_contact.Home_Phone1 = txtEditBrokerPhone.Text;
            sale_agent_contact.Home_Phone2 = "";
            sale_agent_contact.Office_Phone1 = "";
            sale_agent_contact.Office_Phone2 = "";
            sale_agent_contact.Fax1 = txtEditBrokerFax.Text;
            sale_agent_contact.Fax2 = "";
            sale_agent_contact.EMail = txtEditBrokerEmail.Text;
        }

        ///// Check Duplicate ID Card when update
        //if (da_sale_agent.Check_Duplicate(sale_agent, sale_agent_ordinary, 3) == true)
        //{
        //    check_id_card = 1;
        //}

        //if (check_id_card == 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The ID Card (" + sale_agent_ordinary.ID_Card + ") has already existed. Please check it again.')", true);
        //}
        /// Insert into Ct_Sale_Agent, Ct_Sale_Agent_Contact, Ct_Sale_Agent_Ordinary

        if (da_sale_agent.UpdateSale_Agent(sale_agent, sale_agent_contact, sale_agent_ordinary) == false)
        {
            if (sale_agent.Sale_Agent_Type == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The process of Add New Sale Ordinary Agent is unsuccessful. Please check it again.')", true);
            }
            else if (sale_agent.Sale_Agent_Type == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The process of Add New Sale Bank Agent is unsuccessful. Please check it again.')", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The process of Add New Sale Broker Agent is unsuccessful. Please check it again.')", true);
            }
        }

        if (txtSearchSaleAgentCode.Text == "")
        {
            GvSaleAgent.DataSource = da_sale_agent.GetSaleAgentList();
            GvSaleAgent.DataBind();
        }
        else
        {
            GvSaleAgent.DataSource = da_sale_agent.GetSaleAgent_By_SaleAgentCode(txtSearchSaleAgentCode.Text);
            GvSaleAgent.DataBind();
        }

        // Clear
        hdfEditSaleID.Value = "";
        hdfEditBankSaleID.Value = "";
        hdfEditBrokerSaleID.Value = "";

        ClearTextEdit();
    }

    /// <summary>
    ///  Delete
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string sale_agent_id = "";

        if (int.Parse(hdfDeleteSaleAgent.Value) == 0)
        {
            sale_agent_id = hdfDeleteSaleID.Value;
        }
        else if (int.Parse(hdfDeleteSaleAgent.Value) == 1)
        {
            sale_agent_id = hdfDeleteSaleIDBank.Value;
        }
        else
        {
            sale_agent_id = hdfDeleteSaleIDBroker.Value;
        }


        if (da_sale_agent.GetSaleAgent_IsUsed_By_Sale_ID(sale_agent_id) == false)
        {
            sale_agent.Sale_Agent_ID = sale_agent_id;
            da_sale_agent.DeleteSaleAgent_Record(sale_agent);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The Sale Agent Code (" + sale_agent_id + ") is already used. You cannot delete this agent from the system!')", true);
        }
        GvSaleAgent.DataBind();
    }

    /// <summary>
    /// Cancel Sale Agent
    /// </summary>
    protected void btnCancelSaleAgent_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;
        
        bl_sale_agent_cancel sale_agent_cancel = new bl_sale_agent_cancel();
        sale_agent_cancel.Created_On = DateTime.Now;
        sale_agent_cancel.Created_By = user_name;

        //if (int.Parse(hdfCancelSaleType.Value) == 0)
        //{
            sale_agent_cancel.Sale_Agent_ID = hdfCancelSaleID.Value;
            sale_agent_cancel.Created_Note = txtCancelNote.Text;
        //}
        //else if (int.Parse(hdfCancelSaleType.Value) == 1)
        //{
        //    sale_agent_cancel.Sale_Agent_ID = hdfCancelSaleIDBank.Value;
        //    sale_agent_cancel.Created_Note = txtCancelSaleNoteBank.Text;
        //}
        //else 
        //{ 
        //  sale_agent_cancel.Sale_Agent_ID = hdfCancelSaleIDBroker.Value;
        //  sale_agent_cancel.Created_Note = txtCancelNoteBroker.Text;  
        
        //}
       

        da_sale_agent_cancel.InsertSale_Agent_Cancel(sale_agent_cancel);

        GvSaleAgent.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtSearchSaleAgentCode.Text != "")
        {
            GvSaleAgent.DataSource = da_sale_agent.GetSaleAgent_By_SaleAgentCode(txtSearchSaleAgentCode.Text);
            GvSaleAgent.DataBind();
        }
        else
        {
            if (txtLastNameSearch.Text != "" || txtFirstNameSearch.Text != "")
            {
                GvSaleAgent.DataSource = da_sale_agent.GetSaleAgent_By_Name(txtFirstNameSearch.Text, txtLastNameSearch.Text);
            }
            else
            {
                GvSaleAgent.DataSource = da_sale_agent.GetSaleAgentList();
            }
            GvSaleAgent.DataBind();
        }

        txtSearchSaleAgentCode.Text = "";
        txtFirstNameSearch.Text = "";
        txtLastNameSearch.Text = "";
    }

    protected void Sorting(object sender, GridViewSortEventArgs e)
    {
        sale_agent.SortDir = GetSortDirection(e.SortExpression);

        Load_Grid();

    }

    //Get sort direction
    private string GetSortDirection(string column)
    {
        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;

                if (lastDirection != null && lastDirection == "ASC")
                {
                    sortDirection = "DESC";

                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        sale_agent.SortColum = column;
        sale_agent.SortDir = sortDirection;

        return sortDirection;

    }

    //Load Grid view
    protected void Load_Grid()
    {
        DataTable dt = new DataTable(); DataTable dt11 = new DataTable();

        if (txtSearchSaleAgentCode.Text == "")
        {
            dt = da_sale_agent.GetSaleAgentList();
        }
        else
        {
            if (txtLastNameSearch.Text != "" || txtFirstNameSearch.Text != "")
            {
                dt = da_sale_agent.GetSaleAgent_By_SaleAgentCode(txtSearchSaleAgentCode.Text);
            }
            else { dt = da_sale_agent.GetSaleAgentList(); }
        }

        if (dt.Rows.Count > 0)
        {
            if (sale_agent.SortColum == "Sale_Agent_ID")
            {
                if (sale_agent.SortDir == "ASC")
                {
                    DataView view = dt.DefaultView;
                    view.Sort = "Sale_Agent_ID ASC";
                    dt11 = view.ToTable();
                }
                else
                {
                    DataView view = dt.DefaultView;
                    view.Sort = "Sale_Agent_ID DESC";
                    dt11 = view.ToTable();
                }

            }
            else if (sale_agent.SortColum == "Full_Name")
            {
                if (sale_agent.SortDir == "ASC")
                {
                    DataView view = dt.DefaultView;
                    view.Sort = "Full_Name ASC";
                    dt11 = view.ToTable();
                }
                else
                {
                    DataView view = dt.DefaultView;
                    view.Sort = "Full_Name DESC";
                    dt11 = view.ToTable();
                }
            }

            GvSaleAgent.DataSource = dt11;
            GvSaleAgent.DataBind();
        }

    }

    protected void GvSaleAgent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
    {

    }
}