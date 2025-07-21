using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Business_print_policy_payment_schedule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser myUser = Membership.GetUser();
            string user_id = myUser.ProviderUserKey.ToString();
            string user_name = myUser.UserName;

            hdfUserName.Value = user_name;
        }
    }

    //Get policy payment schedule
    protected void btnSearchPaymentSchedule_Click(object sender, EventArgs e)
    {
        string policy_number = txtPolicyNumber.Text.Trim();

        dvContent.Attributes.Clear();

       
        if (policy_number != "")
        {
            List<bl_policy_payment_schedule> policy_payment_schedule_list = new List<bl_policy_payment_schedule>();
            policy_payment_schedule_list = da_policy_payment_schedule.GetPolicyPaymentScheduleDetails(policy_number, hdfSelectYear.Value);

            if (policy_payment_schedule_list.Count > 0)
            {
                dvLogo.Style.Remove("display");
                dvLogo.Style.Add("display", "block");
                dvDetail.Style.Remove("display"); 
                dvDetail.Style.Add("display", "block");
                dvSignature.Style.Remove("display");
                dvSignature.Style.Add("display", "block");
                dvTitle.Style.Remove("display");
                dvTitle.Style.Add("display", "block");

                bl_policy_detail policy = new bl_policy_detail();
                policy = da_policy.GetPolicyDetail(policy_payment_schedule_list[0].Policy_ID.ToString());
                string sale_agent_id = da_policy.GetSaleAgentIDByPolicyID(policy_payment_schedule_list[0].Policy_ID.ToString());

                lblEffectiveDate.Text = policy.Effective_Date.ToString("dd-MM-yyy");
                //lblAgentTelNumber.Text = da_sale_agent.GetSaleAgent_By_SaleAgentCode(sale_agent_id).Rows[0]["Mobile_Phone1"].ToString();

                #region by Maneth 06 03 2017
                sale_agent_id = policy.Sale_Agent_ID;
                System.Data.DataTable tbl = da_sale_agent.GetSaleAgent_By_SaleAgentCode(sale_agent_id);
                var my_row = tbl.Rows[0];
                if (my_row["Mobile_Phone1"].ToString().Trim() == "")
                {
                    lblAgentTelNumber.Text = my_row["home_phone1"].ToString();
                }
                else
                {
                    lblAgentTelNumber.Text = my_row["Mobile_Phone1"].ToString();
                }
                
                #endregion
                #region Add "Agent's name: " before name of agent by maneth 14032017
                //lblAgentName.Text = da_sale_agent.GetSaleAgentNameByID(sale_agent_id).ToUpper().Trim();
                lblAgentName.Text = "Agent's name: " +  da_sale_agent.GetSaleAgentNameByID(sale_agent_id).ToUpper().Trim();
                #endregion
               
                lblInsurancePlan.Text = da_product.GetProductByProductID(policy.Product_ID).En_Title.Trim();

                #region show english name of insured name by maneth 14032017
                //lblInsuredName.Text = policy.Customer_Fullname.ToUpper().Trim();
                lblInsuredName.Text = policy.Last_Name + " " + policy.First_Name;
                #endregion
               

                //lblSumInsure.Text =​Helper.FormatDec( policy.System_Sum_Insure);
                lblSumInsure.Text = Helper.FormatDec(policy.User_Sum_Insure);

                switch(da_policy.GetPolicyPayMode(policy_payment_schedule_list[0].Policy_ID.ToString()).Pay_Mode){
                    case 0: //Single
                        lblPayMode.Text = "Single";
                        break;
                    case 1: //Annually
                        lblPayMode.Text = "Annually";
                        break;
                    case 2:
                        lblPayMode.Text = "Semi-annually";
                        break;
                    case 3:
                        lblPayMode.Text = "Quarterly";
                        break;
                    case 4:
                        lblPayMode.Text = "Monthly";
                        break;
                }

                lblPolicyNumber.Text = policy.Policy_Number;

                #region not show user name
                //string[] arr_user_name = hdfUserName.Value.Split('.');

                //if (arr_user_name.Count() > 1)
                //{
                //    lblName.Text = arr_user_name[1].ToUpper().Trim() + " " + arr_user_name[0].ToUpper().Trim();
                //}
                //else
                //{
                //    lblName.Text = arr_user_name[0].ToUpper().Trim();
                //}
                #endregion
                //lblPosition.Text = "Underwriter";
                //<updated: 06062017 by: maneth>
                lblPosition.Text = "Life Operation Department";

                Table table = new Table();
                table.ID = "tblPaymentTable";
                table.Style.Add("width", "100%");
                table.Style.Add("border", "1px solid #000000");
                
                TableRow header = new TableRow();

                //No
                TableCell cell1 = new TableCell();
                cell1.Style.Add("width", "40px");
               
                cell1.Style.Add("border", "1px solid #000000");
                cell1.Style.Add("text-align", "center");
                cell1.Style.Add("font-weight", "bold");
                cell1.Text = "No";
                cell1.Style.Add("font-size", "10px");

                //Due Date
                TableCell cell5 = new TableCell();
                cell5.Text = "Due Date";
                cell5.Style.Add("border", "1px solid #000000");
                cell5.Style.Add("text-align", "center");
                cell5.Style.Add("font-weight", "bold");
                cell5.Style.Add("font-size", "10px");

                //Year/Time
                TableCell cell2 = new TableCell();
                cell2.Style.Add("width", "40px");
                cell2.Style.Add("border", "1px solid #000000");
                cell2.Style.Add("text-align", "center");
                cell2.Style.Add("font-weight", "bold");
                cell2.Text = "Year/Time";
                cell2.Style.Add("font-size", "10px");

                //Premium
                TableCell cell7 = new TableCell();
                cell7.Text = "Standard Premium (USD)";
                cell7.Style.Add("border", "1px solid #000000");
                cell7.Style.Add("text-align", "center");
                cell7.Style.Add("font-weight", "bold");
                cell7.Style.Add("font-size", "10px");

                //Discount
                TableCell cell8 = new TableCell();
                cell8.Text = "Discount(USD)";
                cell8.Style.Add("border", "1px solid #000000");
                cell8.Style.Add("text-align", "center");
                cell8.Style.Add("font-weight", "bold");
                cell8.Style.Add("font-size", "10px");

                //Premium After Discount
                TableCell cell9 = new TableCell();
                cell9.Text = "Standard Premium After Discount (USD)";
                cell9.Style.Add("border", "1px solid #000000");
                cell9.Style.Add("text-align", "center");
                cell9.Style.Add("font-weight", "bold");
                cell9.Style.Add("font-size", "10px");

                //EM Premium
                TableCell cell10 = new TableCell();
                cell10.Text = "Extra Premium (USD)";
                cell10.Style.Add("border", "1px solid #000000");
                cell10.Style.Add("text-align", "center");
                cell10.Style.Add("font-weight", "bold");
                cell10.Style.Add("font-size", "10px");

                //Total Premium
                TableCell cell11 = new TableCell();
                cell11.Text = "Total Premium (USD)";
                cell11.Style.Add("border", "1px solid #000000");
                cell11.Style.Add("text-align", "center");
                cell11.Style.Add("font-weight", "bold");
                cell11.Style.Add("font-size", "10px");

                //Accumulated Premium Paid
                #region Hide Accumulated Premium Paid

                //TableCell cell12 = new TableCell();
                //cell12.Text = "Accumulated Premium Paid (USD)";
                //cell12.Style.Add("border", "1px solid #000000");
                //cell12.Style.Add("text-align", "center");
                //cell12.Style.Add("font-weight", "bold");
                //cell12.Style.Add("font-size", "8px");
                #endregion
                //Remark
                TableCell cell13 = new TableCell();
                cell13.Text = "Remarks";
                cell13.Style.Add("border", "1px solid #000000");
                cell13.Style.Add("text-align", "center");
                cell13.Style.Add("font-weight", "bold");
                cell13.Style.Add("font-size", "10px");

                header.Cells.Add(cell1);
                header.Cells.Add(cell2);

                header.Cells.Add(cell5);

                header.Cells.Add(cell7);
                header.Cells.Add(cell8);
                header.Cells.Add(cell9);
                header.Cells.Add(cell10);
                header.Cells.Add(cell11);

                //Accumulated Premium Paid
                //header.Cells.Add(cell12);

                header.Cells.Add(cell13);

                table.Rows.Add(header);

                double accumulated_premium = 0;

                for (int i = 0; i < policy_payment_schedule_list.Count; i++)
                {
                    bl_policy_payment_schedule policy_payment_schedule = new bl_policy_payment_schedule();

                    policy_payment_schedule = policy_payment_schedule_list[i];

                    TableRow row2 = new TableRow();

                    //No
                    TableCell row2_cell1 = new TableCell();
                    row2_cell1.Style.Add("text-align", "center");
                    row2_cell1.Style.Add("border", "1px solid #000000");
                    row2_cell1.Style.Add("height", "5px;");
                    row2_cell1.Text = (i + 1).ToString();
                    row2_cell1.Style.Add("font-size", "8px");
                    

                    //Due Date
                    TableCell row2_cell5 = new TableCell();
                    row2_cell5.Style.Add("text-align", "center");
                    row2_cell5.Style.Add("border", "1px solid #000000");
                    row2_cell5.Text = policy_payment_schedule.Due_Date.ToString("dd-MM-yyy");
                    row2_cell5.Style.Add("font-size", "8px");
                  
                    //Year/Time
                    TableCell row2_cell2 = new TableCell();
                    row2_cell2.Style.Add("text-align", "center");
                    row2_cell2.Style.Add("border", "1px solid #000000");
                    row2_cell2.Text = policy_payment_schedule.Year + "/" + policy_payment_schedule.Time;
                    row2_cell2.Style.Add("font-size", "8px");
                  
                    //Premium
                    TableCell row2_cell7 = new TableCell();
                    row2_cell7.Style.Add("border", "1px solid #000000");
                    row2_cell7.Style.Add("text-align", "center");
                    //row2_cell7.Style.Add("padding-top", "5px");
                    row2_cell7.Text = policy_payment_schedule.Premium.ToString("C0");
                    row2_cell7.Style.Add("font-size", "8px");
                 
                    //Discount
                    TableCell row2_cell8 = new TableCell();
                    row2_cell8.Style.Add("border", "1px solid #000000");
                    row2_cell8.Style.Add("text-align", "center");
                    //row2_cell8.Style.Add("padding-top", "5px");
                    row2_cell8.Text = policy_payment_schedule.Discount.ToString("C0");
                    row2_cell8.Style.Add("font-size", "8px");
              
                    //Premium After Discount
                    TableCell row2_cell9 = new TableCell();
                    row2_cell9.Style.Add("border", "1px solid #000000");
                    row2_cell9.Style.Add("text-align", "center");
                    //row2_cell9.Style.Add("padding-top", "5px");
                    row2_cell9.Text = policy_payment_schedule.Premium_After_Discount.ToString("C0");
                    row2_cell9.Style.Add("font-size", "8px");
                  
                    //EM Premium
                    TableCell row2_cell10 = new TableCell();
                    row2_cell10.Style.Add("border", "1px solid #000000");
                    row2_cell10.Style.Add("text-align", "center");
                    //row2_cell10.Style.Add("padding-top", "5px");
                    row2_cell10.Text = policy_payment_schedule.Extra_Premium.ToString("C0");
                    row2_cell10.Style.Add("font-size", "8px");
                    
                    //Total Premium
                    TableCell row2_cell11 = new TableCell();
                    row2_cell11.Style.Add("border", "1px solid #000000");
                    row2_cell11.Style.Add("text-align", "center");
                   // row2_cell11.Style.Add("padding-top", "5px");
                    row2_cell11.Text = policy_payment_schedule.Total_Premium.ToString("C0");
                    row2_cell11.Style.Add("font-size", "8px");
                   
                    accumulated_premium += policy_payment_schedule.Total_Premium;

                    #region//Accumulated Premium
                    //TableCell row2_cell12 = new TableCell();
                    //row2_cell12.Style.Add("border", "1px solid #000000");
                    //row2_cell12.Style.Add("text-align", "center");
                    //row2_cell12.Style.Add("padding-top", "5px");
                    //row2_cell12.Text = accumulated_premium.ToString("C0");
                    //row2_cell12.Style.Add("font-size", "8px");
                    #endregion
                    //Remarks
                    TableCell row2_cell13 = new TableCell();
                    row2_cell13.Style.Add("border", "1px solid #000000");
                    row2_cell13.Style.Add("text-align", "center");
                   // row2_cell13.Style.Add("padding-top", "5px");
                    row2_cell13.Text = policy_payment_schedule.Created_Note.Trim();
                    row2_cell13.Style.Add("font-size", "8px");
                  
                    row2.Cells.Add(row2_cell1);
                    row2.Cells.Add(row2_cell2);
                    row2.Cells.Add(row2_cell5);

                    row2.Cells.Add(row2_cell7);
                    row2.Cells.Add(row2_cell8);
                    row2.Cells.Add(row2_cell9);
                    row2.Cells.Add(row2_cell10);
                    row2.Cells.Add(row2_cell11);

                    //Accumulated Premium Paid
                    //row2.Cells.Add(row2_cell12);

                    row2.Cells.Add(row2_cell13);

                    table.Controls.Add(row2);
                }

                dvContent.Controls.Add(table);
            }
            else
            {
                dvLogo.Style.Remove("display");
                dvLogo.Style.Add("display", "none");
                dvDetail.Style.Remove("display");
                dvDetail.Style.Add("display", "none");
                dvSignature.Style.Remove("display");
                dvSignature.Style.Add("display", "none");
                dvTitle.Style.Remove("display");
                dvTitle.Style.Add("display", "none");
            }
           
        }
    }
   

    protected void btn_Print_Click(object sender, EventArgs e)
    {
        Session["SS_POLICY_NUMBER"] = txtPolicyNumber.Text;
        Session["SS_YEAR"] =  hdfSelectYear.Value;
        Session["SS_REPORT_TYPE"] = ddlReportType.SelectedValue;
        string url = "Reports/policy_payment_schedule_rp.aspx";

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "<script>window.open('" + url + "', '_blank');</script>", true);
        Response.Redirect(url);
    }
}