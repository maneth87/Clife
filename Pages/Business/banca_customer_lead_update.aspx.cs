using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
public partial class Pages_Business_banca_customer_lead_update : System.Web.UI.Page
{
    //class my_session
    //{
    //    public static string USER_NAME { get; set; }
    //    public static string ID { get; set; }
    //    public static string APP_NO { get; set; }
    //    public static string ApplicationID { get; set; }
    //}
    string user = "";
    private bl_sys_user_role UserRole { get { return (bl_sys_user_role)ViewState["V_USER_ROLE"]; } set { ViewState["V_USER_ROLE"] = value; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        string ID = "";
        string appNo = "";
        string appID = "";
       // my_session.USER_NAME = Membership.GetUser().UserName;
        user = Membership.GetUser().UserName;
        lblError.Text = "";

        string ObjCode = Path.GetFileName(Request.Url.AbsolutePath);
        List<bl_sys_user_role> Lobj = (List<bl_sys_user_role>)Session["SS_UR_LIST"];
        bl_sys_user_role ur = new bl_sys_user_role();
        bl_sys_user_role u = ur.GetSysUserRole(Lobj, ObjCode);

        UserRole = u;

        if (!Page.IsPostBack)
        {

            if (Request.QueryString.Count <2)
            {
                Alert("URL paramater is invalid.", true);
                ibtnSave.Attributes.Add("disabled", "disabled");

            }
            else
            {
                Helper.BindLeadStatus(ddlStatus);
               // my_session.ID = Request.QueryString["ID"].ToString();
              ID = Request.QueryString["ID"].ToString();
                //my_session.APP_NO = Request.QueryString["APP_NO"].ToString();
               appNo = Request.QueryString["APP_NO"].ToString();
               ViewState["VS_ID"] = ID;
               ViewState["VS_APP_NO"] = appNo;
                bl_customer_lead_app_temp cus = new bl_customer_lead_app_temp();
                cus = da_customer_lead_app_temp.GetCustomerLeadAppTempByID(ID);// (my_session.ID);
                if (da_customer_lead_app_temp.SUCCESS)
                {
                    if (cus.ApplicationID!= null)
                    {
                        //check application
                        bl_micro_application app = new bl_micro_application();
                        app = da_micro_application.GetApplication(appNo);// (my_session.APP_NO);

                        //my_session.ApplicationID = cus.ApplicationID;
                        ViewState["VS_APP_ID"] = cus.ApplicationID;

                        Helper.SelectedDropDownListIndex("TEXT", ddlIDType, cus.DocumentType);
                        txtIDNumber.Text = cus.DocumentId;
                        txtClientNameEn.Text = cus.ClientNameENG;
                        txtClientNameKh.Text = cus.ClientNameKHM;
                        txtDateOfBirth.Text = cus.ClientDoB.ToString("dd-MM-yyyy");
                        txtPhoneNumber.Text = cus.ClientPhoneNumber;
                        Helper.SelectedDropDownListIndex("TEXT", ddlGender, cus.ClientGender);
                        txtVillageEn.Text = cus.ClientVillage;
                        txtCommuneEn.Text = cus.ClientCommune;
                        txtDistrictEn.Text = cus.ClientDistrict;
                        txtProvinceEn.Text = cus.ClientProvince;

                        Helper.SelectedDropDownListIndex("VALUE", ddlStatus, cus.Status);
                        Helper.BindLeadStatusRemarks(ddlStatusRemarks, ddlStatus.SelectedValue);
                        //txtStatusRemarks.Text = cus.StatusRemarks;
                        Helper.SelectedDropDownListIndex("VALUE", ddlStatusRemarks, cus.StatusRemarks);

                        bl_micro_policy pol = new bl_micro_policy();
                        pol = da_micro_policy.GetPolicyByApplicationID(app.APPLICATION_ID);
                        if (pol.POLICY_NUMBER == null)
                        {
                            ibtnSave.Attributes.Remove("disabled");

                        }
                        else
                        {
                            ddlStatus.Items.Clear();
                            ddlStatus.Items.Add(new ListItem("Approved", "Approved"));
                            ddlStatusRemarks.Items.Clear();
                            ddlStatusRemarks.Items.Add(new ListItem("Issued", "Issued"));
                            ddlStatus.Attributes.Add("disabled", "disabled");
                            ddlStatusRemarks.Attributes.Add("disabled", "disabled");
                            ibtnSave.Attributes.Add("disabled", "disabled");
                            Alert("This record had been issued already. You cannot make change information.", false);
                        }
                    }
                    else
                    {
                        Alert("No Record Found.", true);
                        ibtnSave.Attributes.Add("disabled", "disabled");
                        ddlStatus.Attributes.Add("disabled", "disabled");
                       // txtStatusRemarks.Attributes.Add("disabled", "disabled");
                        ddlStatusRemarks.Attributes.Add("disabled", "disabled");
                    }
                }
                else
                {
                    Alert(da_customer_lead_app_temp.MESSAGE, true);
                    ibtnSave.Attributes.Add("disabled", "disabled");
                }
                
            }
            //disabled customer information
            DisabledControl();
        }
    }
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlStatus.SelectedIndex == 0)
        {
            Alert("Status is required.", true);
        }
        else if (ddlStatusRemarks.SelectedIndex == 0)
        {
            Alert("Remarks is required.", true);
        }
        else
        {
           if(user!="" && ViewState["VS_ID"]+"" !="")//  if (my_session.USER_NAME != null && my_session.ID != null)
            {
                //da_customer_lead.UpdateCustomerLeadStatus(ddlStatus.SelectedValue, txtStatusRemarks.Text.Trim(), my_session.ID, my_session.USER_NAME, DateTime.Now);
                da_customer_lead.UpdateCustomerLeadStatus(ddlStatus.SelectedValue, ddlStatusRemarks.SelectedValue, ViewState["VS_ID"] + "", user, DateTime.Now);
 
               if (da_customer_lead.SUCCESS)
                {
                   SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE.UPDATE,string.Concat("User was updated lead status to [",ddlStatus.SelectedItem.Text," , lead id:",ViewState["VS_ID"] + "]"));
                   //insert status lead history
                   bl_customer_lead_status_history objHis=new bl_customer_lead_status_history();
                   objHis.LeadID= ViewState["VS_ID"]+"";
                   objHis.Status = ddlStatus.SelectedValue;
                   objHis.StatusRemarks = ddlStatusRemarks.SelectedValue;
                   objHis.CreatedBy = user;
                   objHis.CreatedOn = DateTime.Now;
                   da_customer_lead_status_history.InsertLeadStatus(objHis);

                    da_customer_lead_app_temp.DeleteCustomerLeadAppTempByID(ViewState["VS_ID"]+"");// (my_session.ID);


                    //LoadExistData(my_session.ApplicationID);
                    LoadExistData(ViewState["VS_APP_ID"]+"");
                    Alert("Customer status is updated successfully.", false);

                }
                else
                {
                    Alert(da_customer_lead.MESSAGE,true);
                }
            }
            else
            {
                Alert("Page is expired, please reload the page.", false);
            }
        }
    }
    void Alert(string MESSEGE, bool ERROR)
    {
        Helper.Alert(ERROR, MESSEGE, lblError);
    }
    void DisabledControl()
    {
        ddlIDType.Attributes.Add("disabled", "disabled");
        txtIDNumber.Attributes.Add("disabled", "disabled");
        txtClientNameEn.Attributes.Add("disabled", "disabled");
        txtClientNameKh.Attributes.Add("disabled", "disabled");
        ddlGender.Attributes.Add("disabled", "disabled");
        txtDateOfBirth.Attributes.Add("disabled", "disabled");
        txtVillageEn.Attributes.Add("disabled", "disabled");
        txtCommuneEn.Attributes.Add("disabled", "disabled");
        txtDistrictEn.Attributes.Add("disabled", "disabled");
        txtProvinceEn.Attributes.Add("disabled", "disabled");
        txtPhoneNumber.Attributes.Add("disabled", "disabled"); 
    }

    void LoadExistData(string ID)
    {
        bl_customer_lead cus = new bl_customer_lead();
        cus = da_customer_lead.GetCustomerLeadByApplicationID(ID);
        Helper.SelectedDropDownListIndex("TEXT", ddlIDType, cus.DocumentType);
        txtIDNumber.Text = cus.DocumentId;
        txtClientNameEn.Text = cus.ClientNameENG;
        txtClientNameKh.Text = cus.ClientNameKHM;
        txtDateOfBirth.Text = cus.ClientDoB.ToString("dd-MM-yyyy");
        txtPhoneNumber.Text = cus.ClientPhoneNumber;
        Helper.SelectedDropDownListIndex("TEXT", ddlGender, cus.ClientGender);
        txtVillageEn.Text = cus.ClientVillage;
        txtCommuneEn.Text = cus.ClientCommune;
        txtDistrictEn.Text = cus.ClientDistrict;
        txtProvinceEn.Text = cus.ClientProvince;

        Helper.SelectedDropDownListIndex("VALUE", ddlStatus, cus.Status);
        Helper.BindLeadStatusRemarks(ddlStatusRemarks, ddlStatus.SelectedValue);
       // txtStatusRemarks.Text = cus.StatusRemarks;
        Helper.SelectedDropDownListIndex("VALUE", ddlStatusRemarks, cus.StatusRemarks);
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        Helper.BindLeadStatusRemarks(ddlStatusRemarks, ddlStatus.SelectedValue);
    }
    void SaveActivity(bl_sys_activity_log.ACTIVITY_LOG_TYPE.TYPE activity, string description)
    {
        // string logDesc = "User inquiries Lead with criteria [Channel:" + ddlChannel.SelectedValue + ", Branch Name:" + ddlBranchName.SelectedValue + ", Customer Name:" + txtCustomerNameEnglish.Text + ", ID Number:" + txtIDNumber.Text + ", Gender:" + ddlGender.SelectedValue + ", Dob:" + txtDateOfBirth.Text + "]";
        da_sys_activity_log.Save(new bl_sys_activity_log(user, UserRole.ObjectId, activity, DateTime.Now, description, Membership.ApplicationName));

    }
}