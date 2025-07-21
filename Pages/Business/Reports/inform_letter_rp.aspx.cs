using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Text;
using System.Web.Security;
using System.IO;
using NReco.PdfGenerator;
public partial class Pages_Business_Reports_inform_letter_rp : System.Web.UI.Page
{
 #region Variables declaration
        string do_date = "";
        string policy_number = "";
        string customer_name = "";
        string customer_number = "";
        string contact_number = "";
        string address = "";
        double premium = 0;
        double user_premium = 0;
        double remain_premium = 0;
        string agent_name = "";
        string agent_phone = "";
        string title = "";
        string str_prem_text = "";
        string policy_id = "";
        string paper_type = "";
        #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            policy_id = Request.QueryString["policy_id"];
            paper_type = Request.QueryString["p_type"];
            //policy_id="AFE33161-C8C2-42FC-AF6A-A288DDD9A655";

            ReportViewer my_report = new ReportViewer();
            DataTable tbl = new DataTable();
            tbl.Columns.Add("report_code");
            tbl.Columns.Add("report_name");
            tbl.Columns.Add("report_desc");
            DataRow r;
            //assign blank row into data table
            r = tbl.NewRow();
            r[0] = "";
            r[1] = "";
            r[2] = "";
            tbl.Rows.Add(r);

            //tbl = DataSetGenerator.Get_Data_Soure("Select report_code, report_name, report_desc from tbl_reports");
            ReportDataSource datasource = new ReportDataSource("inform_letter", tbl);
            my_report.LocalReport.DataSources.Clear();
            my_report.LocalReport.DataSources.Add(datasource);

            //get policy detail by policy number
            bl_policy_detail po_detail = new bl_policy_detail();
            po_detail = da_policy.GetPolicyDetail(policy_id);

            string report_title = "លិខិតទទួលបណ្ណសន្យារ៉ាប់រង";
            string policy_label = "បណ្ណសន្យារ៉ាប់រង";
            string owner_sign = "ម្ចាស់បណ្ណ";
            string gender = "";
            string marital_status = "";
            DateTime issued_date = new DateTime();

            if (po_detail.Product_ID == null || po_detail.Product_ID == "")//flate rate
            {
                #region Flate Rate
                List<bl_policy_flat_rate> arr_obj = da_policy_flat_rate.GetPolicyByParameters("POLICY.Policy_ID ='" + policy_id + "'");
               
                if (arr_obj.Count > 0)
                {
                    var po_detail1 = arr_obj[0];
                    List<bl_contact> arr_contact = da_customer.GetContactList(po_detail1.CustomerID);
                    List<bl_customer> arr_cust = da_customer.GetCustomerList("", "", "", po_detail1.CustomerID, "");
                    policy_number = po_detail1.PolicyNumber;

                    customer_number = po_detail1.CustomerNumber;
                    if (po_detail1.CustomerNameKh.Trim() != "")
                    {
                        customer_name = po_detail1.CustomerNameKh;
                    }
                    else
                    {
                        customer_name = po_detail1.CustomerName;
                    }

                    if (po_detail1.Gender.ToUpper() == "MALE")
                    {
                        title = "លោក";
                    }
                    else
                    {
                        if (arr_cust[0].Marital_Status.Trim().ToLower() == "single")
                        {
                            title = "កញ្ញា";
                        }
                        else
                        {
                            title = "លោកស្រី";
                        }
                    }
                    
                    if (arr_contact.Count > 0)
                    {
                        contact_number = arr_contact[0].Mobile;
                        address = arr_cust[0].Address + System.Environment.NewLine + da_customer.GetKhanName(arr_cust[0].Khan) + " " + da_customer.GetSangkatName(arr_cust[0].Sangkat) + " " + new da_province().GetProvinceName(arr_cust[0].Province);
                    }

                    issued_date = po_detail1.IssuedDate;

                    //get sale agent info
                    DataTable tbl_agent = da_sale_agent.GetSaleAgent_By_SaleAgentCode(po_detail1.SaleAgentID);
                    var row = tbl_agent.Rows[0];

                    agent_name = row["full_name_kh"].ToString() + "   ​​​​           ";

                    if (row["mobile_phone1"].ToString().Trim() == "")
                    {
                        agent_phone = row["home_phone1"].ToString();
                    }
                    else
                    {
                        agent_phone = row["mobile_phone1"].ToString();
                    }

                    gender = po_detail1.Gender.ToUpper().Trim();
                    marital_status = arr_cust[0].Marital_Status.Trim().ToUpper();
                    premium = po_detail1.PremiumByMode;

                    user_premium = po_detail1.ActualPremium;
                    remain_premium = po_detail1.ReturnPremium;

                    if (remain_premium > 0)
                    {
                        str_prem_text = "សូមជំរាបជូនថាលោកអ្នកបានបង់បុព្វលាភចំនួន " + user_premium + " ដុល្លារអាមេរិកដែលច្រើនជាងបុព្វលាភជាក់ស្តែងចំនួន " + premium + " ដុល្លារ" + System.Environment.NewLine +
                                               "អាមេរិកដូច្នេះយើងខ្ញុំនឹងប្រគល់ជូនបុព្វលាភដែលនៅសល់ចំនួន " + remain_premium + " ដុល្លារអាមេរិកតាមរយៈភ្នាក់ងារធានារ៉ាប់រងរបស់យើងខ្ញុំ។";
                    }
                    else
                    {
                        str_prem_text = "";
                    }

                    #region Check credit life 24
                    List<bl_group_master_product> mMasterList = da_group_master_product.GetGroupMasterProductList(po_detail1.ProductID, "");
                    if (mMasterList.Count > 0)
                    {
                        var master = mMasterList[0];
                        List<bl_group_master_policy> mPolicyList = da_group_master_policy.GetGroupMasterPolicyList(policy_id, master.GroupMasterID);
                        po_detail.Policy_Number = master.GroupCode + "-" + mPolicyList[0].SeqNumber;

                        report_title = "លិខិតទទួលវិញ្ញាបនប័ត្រធានារ៉ាប់រង";
                        policy_label = "វិញ្ញាបនប័ត្រធានារ៉ាប់រង";
                        owner_sign = "ម្ចាស់វិញ្ញាបនប័ត្រ";
                    }
                    #endregion
                }

            }
                #endregion
            else
            {

                policy_number = po_detail.Policy_Number;
                customer_name = po_detail.Customer_Fullname;
                customer_number = po_detail.Customer_ID;
                contact_number = po_detail.Mobile_Phone1;
                issued_date = po_detail.Issue_Date;

                address = po_detail.Address1 + System.Environment.NewLine + po_detail.Address2 + " " + po_detail.Province;
                //get sale agent info
                DataTable tbl_agent = da_sale_agent.GetSaleAgent_By_SaleAgentCode(po_detail.Sale_Agent_ID);
                var row = tbl_agent.Rows[0];

                agent_name = row["full_name_kh"].ToString() + "   ​​​​           ";

                if (row["mobile_phone1"].ToString().Trim() == "")
                {
                    agent_phone = row["home_phone1"].ToString();
                }
                else
                {
                    agent_phone = row["mobile_phone1"].ToString();
                }

                if (po_detail.Gender.ToUpper() == "ប្រុស")
                {
                    title = "លោក";
                }
                else
                {
                    //get marital status from application form only life assure.
                    DataTable tbl_person = new DataTable();
                    tbl_person = da_application_fp6.GetDataTable("SP_Get_App_Info_Person_FP6_By_App_Register_ID", po_detail.App_Register_ID);

                    foreach (DataRow r_person in tbl_person.Rows)
                    {
                        if (r_person["level"].ToString().Trim() == "1")
                        {
                            marital_status = r_person["marital_status"].ToString().Trim().ToUpper();
                            break;
                        }
                    }
                    if (marital_status.Trim() != "")
                    {
                        if (marital_status.Trim().ToLower() == "single")
                        {
                            title = "កញ្ញា";
                        }
                        else
                        {
                            title = "លោកស្រី";
                        }
                    }
                    else
                    {
                        title = "កញ្ញា";
                    }
                }

                DataTable tbl_pre = da_policy_prem_pay.Get_Prem(policy_id);
                premium = Convert.ToDouble(tbl_pre.Rows[0]["Amount"].ToString());

               

                #region Get user premium
                DataTable tbl_user_premium = DataSetGenerator.Get_Data_Soure("SP_GET_USER_PREMIUM_POLICY_ID", new string[,] { { "@Policy_ID", policy_id } });
                foreach (DataRow user_prem_row in tbl_user_premium.Rows)
                {
                    user_premium = Convert.ToDouble(user_prem_row["user_premium"].ToString());
                    break;
                }
                #endregion

                string newPolicyNumber = "";
                if (policy_number.Length <= 8)//normal policy number format
                {
                    newPolicyNumber = Convert.ToDouble(policy_number) + "";
                }
                else
                {
                    newPolicyNumber = policy_number;
                }

                List<bl_policy_payment_schedule> listPayment = da_policy_payment_schedule.GetPolicyPaymentScheduleDetails(newPolicyNumber, "0");
                double totalPrePaid = 0;
                foreach (bl_policy_payment_schedule obj in listPayment.Where(_ => _.Created_Note.Trim().ToUpper() == "PAID"))
                {
                    totalPrePaid += obj.Total_Premium;
                }

                if (user_premium > 0 && user_premium > totalPrePaid)
                {
                    remain_premium = user_premium - totalPrePaid;
                    str_prem_text = "សូមជំរាបជូនថាលោកអ្នកបានបង់បុព្វលាភចំនួន " + user_premium + " ដុល្លារអាមេរិកដែលច្រើនជាងបុព្វលាភជាក់ស្តែងចំនួន " + totalPrePaid + " ដុល្លារ" + System.Environment.NewLine +
                                           "អាមេរិកដូច្នេះយើងខ្ញុំនឹងប្រគល់ជូនបុព្វលាភដែលនៅសល់ចំនួន " + remain_premium + " ដុល្លារអាមេរិកតាមរយៈភ្នាក់ងារធានារ៉ាប់រងរបស់យើងខ្ញុំ។";
                }
                else
                {
                    str_prem_text = "";
                }


                #region Check credit life 24
                List<bl_group_master_product> mMasterList = da_group_master_product.GetGroupMasterProductList(po_detail.Product_ID, "");
                if (mMasterList.Count > 0)
                {
                    var master = mMasterList[0];
                    List<bl_group_master_policy> mPolicyList = da_group_master_policy.GetGroupMasterPolicyList(policy_id, master.GroupMasterID);
                    po_detail.Policy_Number = master.GroupCode + "-" + mPolicyList[0].SeqNumber;

                    report_title = "លិខិតទទួលបណ្ណសន្យារ៉ាប់រង";
                    policy_label = "បណ្ណសន្យារ៉ាប់រង";
                    owner_sign = "ម្ចាស់បណ្ណ";
                }


                #endregion
            }

            string day = issued_date.Day.ToString("00");
            string month = da_policy.GetMonthName(issued_date.Month);
            string year = issued_date.Year + "";

            do_date = "រាជធានីភ្នំពេញ ថ្ងៃទី  " + day + " ខែ " + month + " ឆ្នាំ " + year;


            #region Approver information
            string approver_name_kh = "";
            string approver_position_kh = "";
            da_report_approver.bl_report_approver approver = new da_report_approver.bl_report_approver();
            approver = da_report_approver.GetAproverInfo(policy_id);
            approver_name_kh = approver.NameKh;
            approver_position_kh = approver.PositionKh;
            
            #endregion


            ReportParameter[] paras = new ReportParameter[] { 
             new ReportParameter("Do_Date", do_date),
             new ReportParameter("TO_WHOM", customer_name),
             new ReportParameter("Title",title),
             new ReportParameter("Customer_Number", customer_number),
             new ReportParameter("Address",address),
             new ReportParameter("Contact_Number", contact_number),
             new ReportParameter("Text_For_Return_Premium", str_prem_text),
             new ReportParameter("Agent_Phone", agent_phone),
             new ReportParameter("Agent_Name", agent_name),
            new ReportParameter("Policy_Number",  policy_number),
            //new ReportParameter("Paper_Type","LETTER_HEAD")
            new ReportParameter("Paper_Type",paper_type),
            new ReportParameter("Approver_Name_Kh", approver_name_kh),
             new ReportParameter("Approver_Position_Kh", approver_position_kh),
             new ReportParameter("Report_Title",report_title),
             new ReportParameter("Policy_label",policy_label),
             new ReportParameter("Owner_Sign",owner_sign)
         };

            my_report.LocalReport.ReportPath = Server.MapPath("inform_letter.rdlc");
            my_report.LocalReport.SetParameters(paras);
            my_report.LocalReport.Refresh();
            Report_Generator.ExportToPDF(this.Context, my_report, "Inform_letter" + DateTime.Now.ToString("yyyyMMddhhmmss"), false);

            #region save printed records
            bool save = false;
            //save = da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records { Policy_ID=policy_id, Printed_By= Membership.GetUser().UserName , Printed_Date= DateTime.Now});
            save = da_inform_letter_printed_records.InsertRecord(new bl_inform_letter_printed_records(policy_id, DateTime.Now, Membership.GetUser().UserName, "Acknowledge"));
            #endregion

        }
        catch (Exception ex)
        {
            Log.AddExceptionToLog("Error [Page_Load] in page [inform_letter_rp.aspx.cs], Detail: " + ex.Message);
            message.InnerHtml = "Load report error.";
        }
    }

}