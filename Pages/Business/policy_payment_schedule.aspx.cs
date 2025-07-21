using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Web.UI.HtmlControls;



public partial class Pages_Business_policy_payment_schedule : System.Web.UI.Page
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

    //Draw Policy Payment Schedule according to each policy
    protected void btnCreatePaymentScheduleTable_Click(object sender, EventArgs e)
    {
        string policy_number = txtPolicyNumber.Text.Trim();

        dvContent.Attributes.Clear();

        if (policy_number != "")
        {
            //Check policy number in table Ct_Policy_Payment_Schedule
            //if (da_policy_payment_schedule.CheckPolicyNumber(Convert.ToInt64(policy_number).ToString()))
            if (da_policy_payment_schedule.CheckPolicyNumber(policy_number))
            {
               // Alert already created     
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Policy payment schedule already created.')", true);

               // ClientScript.RegisterStartupScript(this.GetType(), "", "confirm ('Policy payment schedule already created.')", true);
                //load exist data
               // loadExistPolicyPaymentSchedule(policy_number);
                
            }
            else
            {
                //Create new payment schedule table
                bl_policy_payment_schedule policy = da_policy_payment_schedule.GetPolicyDetails(policy_number);

                string pay_mode = da_payment_mode.GetPaymentModeByPayModeID(policy.Pay_Mode).Mode;

                DateTime effective_date = policy.Due_Date;

                #region Policy Riders
                double total_rider_premium = 0;
                List<bl_policy_premium_riders> list = da_policy_premium_riders.GetPolicyPremiumRidersList(policy.Policy_ID);
                foreach (bl_policy_premium_riders riders in list)
                {
                    total_rider_premium = total_rider_premium + Math.Ceiling(riders.Original_Amount);
                }
               
                #endregion

                //Draw schedule table
                #region
                    int n = 0;
                    int period = 0;
                    switch (policy.Pay_Mode)
                    {
                        case 0:
                        case 1:
                            n = 1;
                            period = 1;
                            break;
                        case 2:
                            n = 2;
                            period = 2;
                            break;
                        case 3:
                            n = 4;
                            period = 4;
                            break;
                        case 4:
                            n = 12;
                            period = 12;
                            break;
                    }

                    n = 1;
            
                    Table table = new Table();
                    table.ID = "tblPaymentTable";
                    table.Style.Add("width", "100%");
                    table.Style.Add("border", "1px solid #000000");

                    #region Grid header

                    TableRow header = new TableRow();                    

                    //Year
                    TableCell cell1 = new TableCell();
                    cell1.Style.Add("width", "40px");
                    cell1.Style.Add("border", "1px solid #000000");
                    cell1.Style.Add("text-align", "center");
                    cell1.Style.Add("font-weight", "bold");
                    cell1.Text = "Year";

                    ////Time
                    //TableCell cell2 = new TableCell();
                    //cell2.Style.Add("width", "40px");
                    //cell2.Style.Add("border", "1px solid #000000");
                    //cell2.Style.Add("text-align", "center");
                    //cell2.Style.Add("font-weight", "bold");
                    //cell2.Text = "Time";

                    //Product
                    TableCell cell3 = new TableCell();                    
                    cell3.Text = "Product";
                    cell3.Style.Add("width", "80px");
                    cell3.Style.Add("border", "1px solid #000000");
                    cell3.Style.Add("text-align", "center");
                    cell3.Style.Add("font-weight", "bold");

                    //Pay Mode
                    TableCell cell4 = new TableCell();
                    cell4.Text = "Pay Mode";
                    cell4.Style.Add("border", "1px solid #000000");
                    cell4.Style.Add("text-align", "center");
                    cell4.Style.Add("font-weight", "bold");

                    ////modified by maneth : 5th Feb 2016
                    ////Due Date
                    //TableCell cell5 = new TableCell();
                    //cell5.Text = "Due Date";
                    //cell5.Style.Add("border", "1px solid #000000");
                    //cell5.Style.Add("text-align", "center");
                    //cell5.Style.Add("font-weight", "bold");

                    //Sum Insure
                    TableCell cell6 = new TableCell();
                    cell6.Text = "Sum Insure";
                    cell6.Style.Add("border", "1px solid #000000");
                    cell6.Style.Add("text-align", "center");
                    cell6.Style.Add("font-weight", "bold");

                    //Premium
                    TableCell cell7 = new TableCell();
                    //cell7.Text = "Premium";
                    cell7.Text = "Premium/Year";
                    cell7.Style.Add("border", "1px solid #000000");
                    cell7.Style.Add("text-align", "center");
                    cell7.Style.Add("font-weight", "bold");

                    //Discount
                    TableCell cell8 = new TableCell();
                    //cell8.Text = "Discount";
                    cell8.Text = "Discount/Year";
                    cell8.Style.Add("border", "1px solid #000000");
                    cell8.Style.Add("text-align", "center");
                    cell8.Style.Add("font-weight", "bold");

                    //Premium After Discount
                    TableCell cell9 = new TableCell();
                    cell9.Text = "Premium After Discount";
                    cell9.Style.Add("border", "1px solid #000000");
                    cell9.Style.Add("text-align", "center");
                    cell9.Style.Add("font-weight", "bold");

                    //EM Premium
                    TableCell cell10 = new TableCell();
                    cell10.Text = "EM Premium";
                    cell10.Style.Add("border", "1px solid #000000");
                    cell10.Style.Add("text-align", "center");
                    cell10.Style.Add("font-weight", "bold");

                    //Total Premium
                    TableCell cell11 = new TableCell();
                    cell11.Text = "Total Premium";
                    cell11.Style.Add("border", "1px solid #000000");
                    cell11.Style.Add("text-align", "center");
                    cell11.Style.Add("font-weight", "bold");

                    //Remark
                    TableCell cell12 = new TableCell();
                    cell12.Text = "Remark";
                    cell12.Style.Add("border", "1px solid #000000");
                    cell12.Style.Add("text-align", "center");
                    cell12.Style.Add("font-weight", "bold");

                    //discount period
                    TableCell cell13 = new TableCell();
                    cell13.Text = "Period";
                    cell13.Style.Add("border", "1px solid #000000");
                    cell13.Style.Add("text-align", "center");
                    cell13.Style.Add("font-weight", "bold");
                
                    header.Cells.Add(cell1);
                    //hide column time
                   // header.Cells.Add(cell2);
                    header.Cells.Add(cell3);
                    header.Cells.Add(cell4);
                //modified by: maneth : 5th Feb 2016
                //hide column due date
                  //  header.Cells.Add(cell5);
                //
                    header.Cells.Add(cell6);
                    header.Cells.Add(cell7);
                    header.Cells.Add(cell8);
                    header.Cells.Add(cell9);
                    header.Cells.Add(cell10);
                    header.Cells.Add(cell11);
                    header.Cells.Add(cell12);
                    header.Cells.Add(cell13);

                    table.Rows.Add(header);
                    #endregion

                    int rows = 0;
                    //Loop Year
                    for (int i = 1; i <= policy.Year; i++)
                    {
                                             
                        //Loop Pay Mode
                        for (int j = 1; j <= n; j++)
                        {
                            rows += 1;

                            DateTime due_date = policy.Due_Date;
                            DateTime my_date = new DateTime();
                                                     

                            if (i == 1 && j == 1)
                            {
                                my_date = due_date;
                            }
                            else
                            {
                                switch (policy.Pay_Mode)
                                {
                                    case 0:
                                    case 1:
                                        policy.Due_Date = policy.Due_Date.AddYears(1);
                                        break;
                                    case 2:
                                        policy.Due_Date = policy.Due_Date.AddMonths(6);
                                        break;
                                    case 3:
                                        policy.Due_Date = policy.Due_Date.AddMonths(3);
                                        break;
                                    case 4:
                                        policy.Due_Date = policy.Due_Date.AddMonths(1);
                                        break;
                                }

                                my_date = Calculation.GetNext_Due(policy.Due_Date, due_date, effective_date);

                                policy.Due_Date = my_date;
                            }                           

                            if (j == 1)
                            {
                                TableRow row = new TableRow();

                                //Year
                                TableCell row_cell1 = new TableCell();
                                row_cell1.RowSpan = n;
                                row_cell1.Style.Add("text-align","center");
                                row_cell1.Style.Add("border", "1px solid #000000");
                                row_cell1.Text = i.ToString();

                                ////Time
                                //TableCell row_cell2 = new TableCell();
                                //row_cell2.Style.Add("text-align", "center");
                                //row_cell2.Style.Add("border", "1px solid #000000");

                                HiddenField hdf_Year = new HiddenField();
                                hdf_Year.ID = "hdfYear_" + rows + "_" + 2;
                                hdf_Year.Value = i.ToString();

                                //Label lbl_Time = new Label();
                                //lbl_Time.ID = "lblTime_" + rows + "_" + 2;
                                //lbl_Time.Text = j.ToString();

                                //HiddenField hdf_Time = new HiddenField();
                                //hdf_Time.ID = "hdfTime_" + rows + "_" + 2;
                                //hdf_Time.Value = j.ToString();

                               // row_cell2.Controls.Add(hdf_Year);
                                //row_cell2.Controls.Add(lbl_Time);
                                //row_cell2.Controls.Add(hdf_Time);

                                //Product
                                TableCell row_cell3 = new TableCell();
                                row_cell3.Style.Add("text-align", "center");
                                row_cell3.Style.Add("border", "1px solid #000000");
                                row_cell3.Text = policy.Product_ID.ToString();

                                //Pay Mode
                                TableCell row_cell4 = new TableCell();
                                row_cell4.Style.Add("text-align", "center");
                                row_cell4.Style.Add("border", "1px solid #000000");
                                row_cell4.Text = pay_mode.ToString();

                             
                                ////Due Date
                                //TableCell row_cell5 = new TableCell();
                                //row_cell5.Style.Add("text-align", "center");
                                //row_cell5.Style.Add("border", "1px solid #000000");

                                //Label lblDue_Date = new Label();
                                //lblDue_Date.Text = my_date.ToString("dd-MM-yyyy");
                                //lblDue_Date.ID = "lblDueDate_" + rows + "_" + 5;

                                //HiddenField hdfDue_Date = new HiddenField();
                                //hdfDue_Date.Value = my_date.ToString("dd/MM/yyyy");
                                //hdfDue_Date.ID = "hdfDueDate_" + rows + "_" + 5;

                                //row_cell5.Controls.Add(lblDue_Date);
                                //row_cell5.Controls.Add(hdfDue_Date);

                                //Sum Insure
                                TableCell row_cell6 = new TableCell();
                                row_cell6.Style.Add("text-align", "right");
                                row_cell6.Style.Add("padding-right", "5px");
                                row_cell6.Style.Add("border", "1px solid #000000");
                                row_cell6.Text = policy.Sum_Insure.ToString("C0");

                                //Premium
                                TableCell row_cell7 = new TableCell();
                                row_cell7.Style.Add("border", "1px solid #000000");
                                row_cell7.Style.Add("text-align", "center");
                                row_cell7.Style.Add("padding-top", "5px");

                                TextBox premium_textbox = new TextBox();

                                premium_textbox.ID = "txtPremium_" + rows + "_" + 7;
                                premium_textbox.MaxLength = 50;
                                premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                premium_textbox.Width = 100;
                                //premium_textbox.Text = policy.Premium.ToString("C0");
                                premium_textbox.Text = (policy.Original_Amount + total_rider_premium).ToString("C0");
                                premium_textbox.ReadOnly = true;
                                premium_textbox.Enabled = false;

                                HiddenField premium_hdf = new HiddenField();
                                premium_hdf.ID = "hdfPremium_" + rows + "_" + 7;
                               // premium_hdf.Value = policy.Premium.ToString();  
                                premium_hdf.Value = (policy.Original_Amount + total_rider_premium).ToString();     

                                row_cell7.Controls.Add(premium_textbox);
                                row_cell7.Controls.Add(premium_hdf);

                                //Discount
                                TableCell row_cell8 = new TableCell();
                                row_cell8.Style.Add("border", "1px solid #000000");
                                row_cell8.Style.Add("text-align", "center");
                                row_cell8.Style.Add("padding-top", "5px");

                                TextBox discount_textbox = new TextBox();

                                discount_textbox.ID = "txtDiscount_" + rows + "_" + 8;
                                discount_textbox.MaxLength = 50;
                                discount_textbox.Attributes.Add("onkeyup", "ValidateNumber(this); GetPremium(this.value, \"" + rows + "col" + 8 + "\");");                                
                                discount_textbox.Width = 100;
                                discount_textbox.Attributes.Add("runat", "server");

                                if (i == 1)
                                {
                                    //discount_textbox.Text = policy.Discount.ToString();
                                    discount_textbox.Text = (policy.Discount * period).ToString();
                                }
                                else
                                {
                                    discount_textbox.Text = "0";
                                }
                                ////show all all discount
                                //discount_textbox.Text = policy.Discount.ToString();

                                row_cell8.Controls.Add(discount_textbox);


                                //Premium After Discount
                                TableCell row_cell9 = new TableCell();
                                row_cell9.Style.Add("border", "1px solid #000000");
                                row_cell9.Style.Add("text-align", "center");
                                row_cell9.Style.Add("padding-top", "5px");

                                TextBox premium_after_discount_textbox = new TextBox();

                                premium_after_discount_textbox.ID = "txtPremiumAfterDiscount_" + rows + "_" + 9;
                                premium_after_discount_textbox.MaxLength = 50;
                                premium_after_discount_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                premium_after_discount_textbox.Width = 120;
                                premium_after_discount_textbox.ReadOnly = true;
                                premium_after_discount_textbox.Enabled = false;

                                HiddenField premium_after_discount_hdf = new HiddenField();
                                premium_after_discount_hdf.ID = "hdfPremiumAfterDiscount_" + rows + "_" + 9;

                                if (i == 1)
                                {
                                    //premium_after_discount_textbox.Text = (policy.Premium - policy.Discount).ToString("C0");
                                    //premium_after_discount_hdf.Value = (policy.Premium - policy.Discount).ToString();

                                    premium_after_discount_textbox.Text = (policy.Original_Amount - (policy.Discount * period) + total_rider_premium).ToString("C0");
                                    premium_after_discount_hdf.Value = (policy.Original_Amount - (policy.Discount * period) + total_rider_premium).ToString();
                                }
                                else
                                {
                                    //premium_after_discount_textbox.Text = policy.Premium.ToString("C0");
                                    //premium_after_discount_hdf.Value = policy.Premium.ToString();

                                    premium_after_discount_textbox.Text = (policy.Original_Amount + total_rider_premium).ToString("C0");
                                    premium_after_discount_hdf.Value = (policy.Original_Amount+ total_rider_premium).ToString();
                                }                                                                
                              
                                row_cell9.Controls.Add(premium_after_discount_textbox);
                                row_cell9.Controls.Add(premium_after_discount_hdf);

                                //EM Premium
                                TableCell row_cell10 = new TableCell();
                                row_cell10.Style.Add("border", "1px solid #000000");
                                row_cell10.Style.Add("text-align", "center");
                                row_cell10.Style.Add("padding-top", "5px");

                                TextBox em_premium_textbox = new TextBox();

                                em_premium_textbox.ID = "txtEMPremium_" + rows + "_" + 10;
                                em_premium_textbox.MaxLength = 50;
                                em_premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                em_premium_textbox.Width = 100;
                                em_premium_textbox.ReadOnly = true;
                                em_premium_textbox.Enabled = false;
                                em_premium_textbox.Text = policy.Extra_Premium.ToString("C0");

                                HiddenField em_premium_hdf = new HiddenField();
                                em_premium_hdf.ID = "hdfEMPremium_" + rows + "_" + 10;
                                em_premium_hdf.Value = policy.Extra_Premium.ToString();

                                row_cell10.Controls.Add(em_premium_textbox);
                                row_cell10.Controls.Add(em_premium_hdf);
                                
                                //Total Premium
                                TableCell row_cell11 = new TableCell();
                                row_cell11.Style.Add("border", "1px solid #000000");
                                row_cell11.Style.Add("text-align", "center");
                                row_cell11.Style.Add("padding-top", "5px");

                                TextBox total_premium_textbox = new TextBox();

                                total_premium_textbox.ID = "txtTotalPremium_" + rows + "_" + 11;
                                total_premium_textbox.MaxLength = 50;
                                total_premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                total_premium_textbox.Width = 100;                              
                                total_premium_textbox.ReadOnly = true;
                                total_premium_textbox.Enabled = false;
                              
                                HiddenField total_premium_hdf = new HiddenField();
                                total_premium_hdf.ID = "hdfTotalPremium_" + rows + "_" + 11;

                                if (i == 1)//first year
                                {
                                    //total_premium_textbox.Text = (policy.Premium - policy.Discount + policy.Extra_Premium).ToString("C0");
                                    //total_premium_hdf.Value = (policy.Premium - policy.Discount + policy.Extra_Premium).ToString();

                                    total_premium_textbox.Text = (policy.Original_Amount - (policy.Discount * period) + policy.Extra_Premium + total_rider_premium).ToString("C0");
                                    total_premium_hdf.Value = (policy.Original_Amount - (policy.Discount * period) + policy.Extra_Premium + total_rider_premium).ToString();
                                }
                                else
                                {
                                    //total_premium_textbox.Text = (policy.Premium + policy.Extra_Premium).ToString("C0");
                                    //total_premium_hdf.Value = (policy.Premium + policy.Extra_Premium).ToString();

                                    total_premium_textbox.Text = (policy.Original_Amount + policy.Extra_Premium + total_rider_premium).ToString("C0");
                                    total_premium_hdf.Value = (policy.Original_Amount + policy.Extra_Premium + total_rider_premium).ToString();
                                }

                                ////all total premium
                                //total_premium_textbox.Text = (policy.Premium - policy.Discount + policy.Extra_Premium).ToString("C0");
                                //total_premium_hdf.Value = (policy.Premium - policy.Discount + policy.Extra_Premium).ToString();

                                row_cell11.Controls.Add(total_premium_textbox);
                                row_cell11.Controls.Add(total_premium_hdf);

                                //Remark
                                TableCell row_cell12 = new TableCell();
                                row_cell12.Style.Add("border", "1px solid #000000");
                                row_cell12.Style.Add("text-align", "center");
                                row_cell12.Style.Add("padding-top", "5px");

                                TextBox remark_textbox = new TextBox();

                                remark_textbox.ID = "txtRemark_" + rows + "_" + 12;
                                remark_textbox.MaxLength = 255;
                                remark_textbox.Width = 100;

                                row_cell12.Controls.Add(remark_textbox);

                                //Discount period
                                TableCell row_cell13 = new TableCell();
                                row_cell13.Style.Add("border", "1px solid #000000");
                                row_cell13.Style.Add("text-align", "center");
                                row_cell13.Style.Add("padding-top", "5px");
                                DropDownList ddlDisPeriod = new DropDownList();
                                ddlDisPeriod.ID = "ddlDisPeriod_" + rows + "_" + 13;
                                //add period by pay mode
                                for (int month = 0; month <= period; month++)
                                {
                                    ddlDisPeriod.Items.Add(new ListItem(month+"", month+""));
                                }
                                ddlDisPeriod.Width = 50;
                                //selected period in first year
                                if (i == 1)
                                {
                                    ddlDisPeriod.SelectedIndex = 1;
                                }
                                row_cell13.Controls.Add(ddlDisPeriod);

                                row.Cells.Add(row_cell1);
                                //hide column time
                                //row.Cells.Add(row_cell2);
                                row.Cells.Add(row_cell3);
                                row.Cells.Add(row_cell4);
                                //hide column due date
                               // row.Cells.Add(row_cell5);
                                row.Cells.Add(row_cell6);
                                row.Cells.Add(row_cell7);
                                row.Cells.Add(row_cell8);
                                row.Cells.Add(row_cell9);
                                row.Cells.Add(row_cell10);
                                row.Cells.Add(row_cell11);
                                row.Cells.Add(row_cell12);
                                row.Cells.Add(row_cell13);
                                table.Controls.Add(row);
                            }
                            else
                            {                               

                                TableRow row2 = new TableRow();                                                              

                                ////Time
                                //TableCell row2_cell2 = new TableCell();
                                //row2_cell2.Style.Add("text-align", "center");
                                //row2_cell2.Style.Add("border", "1px solid #000000");                               

                                //Label lbl_Time = new Label();
                                //lbl_Time.ID = "lblTime_" + rows + "_" + 2;
                                //lbl_Time.Text = j.ToString();

                                //HiddenField hdf_Year = new HiddenField();
                                //hdf_Year.ID = "hdfYear_" + rows + "_" + 2;
                                //hdf_Year.Value = i.ToString();

                                //HiddenField hdf_Time = new HiddenField();
                                //hdf_Time.ID = "hdfTime_" + rows + "_" + 2;
                                //hdf_Time.Value = j.ToString();

                                //row2_cell2.Controls.Add(hdf_Year);
                                //row2_cell2.Controls.Add(lbl_Time);
                                //row2_cell2.Controls.Add(hdf_Time);

                                //Product
                                TableCell row2_cell3 = new TableCell();
                                row2_cell3.Style.Add("text-align", "center");
                                row2_cell3.Style.Add("border", "1px solid #000000");
                                row2_cell3.Text = policy.Product_ID.ToString();

                                //Pay Mode
                                TableCell row2_cell4 = new TableCell();
                                row2_cell4.Style.Add("text-align", "center");
                                row2_cell4.Style.Add("border", "1px solid #000000");
                                row2_cell4.Text = pay_mode.ToString();

                                ////hide due date
                                ////Due Date
                                //TableCell row2_cell5 = new TableCell();
                                //row2_cell5.Style.Add("text-align", "center");
                                //row2_cell5.Style.Add("border", "1px solid #000000");

                                //Label lblDue_Date = new Label();
                                //lblDue_Date.Text = my_date.ToString("dd-MM-yyyy");
                                //lblDue_Date.ID = "lblDueDate_" + rows + "_" + 5;

                                //HiddenField hdfDue_Date = new HiddenField();
                                //hdfDue_Date.Value = my_date.ToString("dd/MM/yyyy");
                                //hdfDue_Date.ID = "hdfDueDate_" + rows + "_" + 5;

                                //row2_cell5.Controls.Add(lblDue_Date);
                                //row2_cell5.Controls.Add(hdfDue_Date);

                                //Sum Insure
                                TableCell row2_cell6 = new TableCell();
                                row2_cell6.Style.Add("text-align", "right");
                                row2_cell6.Style.Add("border", "1px solid #000000");
                                row2_cell6.Style.Add("padding-right", "5px");
                                row2_cell6.Text = policy.Sum_Insure.ToString("C0");

                                //Premium
                                TableCell row2_cell7 = new TableCell();
                                row2_cell7.Style.Add("border", "1px solid #000000");
                                row2_cell7.Style.Add("text-align", "center");
                                row2_cell7.Style.Add("padding-top", "5px");

                                TextBox premium_textbox = new TextBox();

                                premium_textbox.ID = "txtPremium_" + rows + "_" + 7;
                                premium_textbox.MaxLength = 50;
                                premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                premium_textbox.Width = 100;                              
                                //premium_textbox.Text = policy.Premium.ToString("C0");
                                premium_textbox.Text = (policy.Original_Amount+ total_rider_premium).ToString("C0");
                                premium_textbox.ReadOnly = true;
                                premium_textbox.Enabled = false;

                                HiddenField premium_hdf = new HiddenField();
                                premium_hdf.ID = "hdfPremium_" + rows + "_" + 7;
                                premium_hdf.EnableViewState = true;
                                //premium_hdf.Value = policy.Premium.ToString(); 
                                premium_hdf.Value = (policy.Original_Amount + total_rider_premium).ToString();                              

                                row2_cell7.Controls.Add(premium_textbox);
                                row2_cell7.Controls.Add(premium_hdf);

                                //Discount
                                TableCell row2_cell8 = new TableCell();
                                row2_cell8.Style.Add("border", "1px solid #000000");
                                row2_cell8.Style.Add("text-align", "center");
                                row2_cell8.Style.Add("padding-top", "5px");

                                TextBox discount_textbox = new TextBox();

                                discount_textbox.ID = "txtDiscount_" + rows + "_" + 8;
                                discount_textbox.MaxLength = 50;
                                discount_textbox.Attributes.Add("onkeyup", "ValidateNumber(this); GetPremium(this.value, \"" + rows + "col" + 8 + "\");");
                                discount_textbox.Width = 100;
                                discount_textbox.Attributes.Add("runat", "server");

                                if (i == 1)
                                {
                                    discount_textbox.Text = (policy.Discount * period).ToString();
                                }
                                else
                                {
                                    discount_textbox.Text = "0";
                                }

                                //show all discount
                                //discount_textbox.Text = policy.Discount.ToString(); 
                                row2_cell8.Controls.Add(discount_textbox);


                                //Premium After Discount
                                TableCell row2_cell9 = new TableCell();
                                row2_cell9.Style.Add("border", "1px solid #000000");
                                row2_cell9.Style.Add("text-align", "center");
                                row2_cell9.Style.Add("padding-top", "5px");

                                TextBox premium_after_discount_textbox = new TextBox();

                                premium_after_discount_textbox.ID = "txtPremiumAfterDiscount_" + rows + "_" + 9;
                                premium_after_discount_textbox.MaxLength = 50;
                                premium_after_discount_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                premium_after_discount_textbox.Width = 120;                             
                                premium_after_discount_textbox.ReadOnly = true;
                                premium_after_discount_textbox.Enabled = false;

                                HiddenField premium_after_discount_hdf = new HiddenField();
                                premium_after_discount_hdf.ID = "hdfPremiumAfterDiscount_" + rows + "_" + 9;

                                if (i == 1)
                                {
                                    //premium_after_discount_textbox.Text = (policy.Premium - policy.Discount).ToString("C0");
                                    //premium_after_discount_hdf.Value = (policy.Premium - policy.Discount).ToString();

                                    premium_after_discount_textbox.Text = (policy.Original_Amount - (policy.Discount * period) + total_rider_premium).ToString("C0");
                                    premium_after_discount_hdf.Value = (policy.Original_Amount - (policy.Discount * period) + total_rider_premium).ToString();
                                }
                                else
                                {
                                    //premium_after_discount_textbox.Text = policy.Premium.ToString("C0");
                                    //premium_after_discount_hdf.Value = policy.Premium.ToString();

                                    premium_after_discount_textbox.Text = (policy.Original_Amount + total_rider_premium).ToString("C0");
                                    premium_after_discount_hdf.Value = (policy.Original_Amount + total_rider_premium).ToString();
                                }
                               
                                //all premium after discount
                                //premium_after_discount_textbox.Text = (policy.Premium - policy.Discount).ToString("C0");
                                //premium_after_discount_hdf.Value = (policy.Premium - policy.Discount).ToString(); 

                                row2_cell9.Controls.Add(premium_after_discount_textbox);
                                row2_cell9.Controls.Add(premium_after_discount_hdf);

                                //EM Premium
                                TableCell row2_cell10 = new TableCell();
                                row2_cell10.Style.Add("border", "1px solid #000000");
                                row2_cell10.Style.Add("text-align", "center");
                                row2_cell10.Style.Add("padding-top", "5px");

                                TextBox em_premium_textbox = new TextBox();

                                em_premium_textbox.ID = "txtEMPremium_" + rows + "_" + 10;
                                em_premium_textbox.MaxLength = 50;
                                em_premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                em_premium_textbox.Width = 100;
                                em_premium_textbox.Text = policy.Extra_Premium.ToString("C0");
                                em_premium_textbox.ReadOnly = true;
                                em_premium_textbox.Enabled= false;
                                
                                HiddenField em_premium_hdf = new HiddenField();
                                em_premium_hdf.ID = "hdfEMPremium_" + rows + "_" + 10;                                                    
                              
                                em_premium_hdf.Value = policy.Extra_Premium.ToString();
                              
                                row2_cell10.Controls.Add(em_premium_textbox);
                                row2_cell10.Controls.Add(em_premium_hdf);


                                //Total Premium
                                TableCell row2_cell11 = new TableCell();
                                row2_cell11.Style.Add("border", "1px solid #000000");
                                row2_cell11.Style.Add("text-align", "center");
                                row2_cell11.Style.Add("padding-top", "5px");

                                TextBox total_premium_textbox = new TextBox();

                                total_premium_textbox.ID = "txtTotalPremium_" + rows + "_" + 11;
                                total_premium_textbox.MaxLength = 50;
                                total_premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                                total_premium_textbox.Width = 100;
                                total_premium_textbox.ReadOnly = true;
                                total_premium_textbox.Enabled = false;  

                                HiddenField total_premium_hdf = new HiddenField();
                                total_premium_hdf.ID = "hdfTotalPremium_" + rows + "_" + 11;
                                
                                if (i == 1)
                                {
                                    //total_premium_textbox.Text = (policy.Premium - policy.Discount + policy.Extra_Premium).ToString("C0");
                                    //total_premium_hdf.Value = (policy.Premium - policy.Discount + policy.Extra_Premium).ToString();

                                    total_premium_textbox.Text = (policy.Original_Amount - (policy.Discount * period) + policy.Extra_Premium + total_rider_premium).ToString("C0");
                                    total_premium_hdf.Value = (policy.Original_Amount - (policy.Discount * period) + policy.Extra_Premium + total_rider_premium).ToString();
                                }
                                else
                                {
                                    //total_premium_textbox.Text = (policy.Premium  + policy.Extra_Premium).ToString("C0");
                                    //total_premium_hdf.Value = (policy.Premium + policy.Extra_Premium).ToString();

                                    total_premium_textbox.Text = (policy.Original_Amount + policy.Extra_Premium + total_rider_premium).ToString("C0");
                                    total_premium_hdf.Value = (policy.Original_Amount + policy.Extra_Premium + total_rider_premium).ToString();
                                }



                                row2_cell11.Controls.Add(total_premium_textbox);
                                row2_cell11.Controls.Add(total_premium_hdf);

                                //Remark
                                TableCell row2_cell12 = new TableCell();
                                row2_cell12.Style.Add("border", "1px solid #000000");
                                row2_cell12.Style.Add("text-align", "center");
                                row2_cell12.Style.Add("padding-top", "5px");

                                TextBox remark_textbox = new TextBox();

                                remark_textbox.ID = "txtRemark_" + rows + "_" + 12;
                                remark_textbox.MaxLength = 255;
                                remark_textbox.Width = 100;
                              
                                row2_cell12.Controls.Add(remark_textbox);

                                //Discount period
                                TableCell row2_cell13 = new TableCell();
                                row2_cell13.Style.Add("border", "1px solid #000000");
                                row2_cell13.Style.Add("text-align", "center");
                                row2_cell13.Style.Add("padding-top", "5px");
                                DropDownList ddlDisPeriod = new DropDownList();
                                ddlDisPeriod.ID = "ddlDisPeriod_" + rows + "_" + 13;
                                //add period by pay mode
                                for (int month = 0; month <= period; month++)
                                {
                                    ddlDisPeriod.Items.Add(new ListItem(month + "", month + ""));
                                }
                                ddlDisPeriod.Width = 50;
                                //selected period in first year
                                if (i == 1)
                                {
                                    ddlDisPeriod.SelectedIndex = 1;
                                }
                                row2_cell13.Controls.Add(ddlDisPeriod);
                                                               
                               // row2.Cells.Add(row2_cell2);
                                row2.Cells.Add(row2_cell3);
                                row2.Cells.Add(row2_cell4);
                               // row2.Cells.Add(row2_cell5);
                                row2.Cells.Add(row2_cell6);
                                row2.Cells.Add(row2_cell7);
                                row2.Cells.Add(row2_cell8);
                                row2.Cells.Add(row2_cell9);
                                row2.Cells.Add(row2_cell10);
                                row2.Cells.Add(row2_cell11);
                                row2.Cells.Add(row2_cell12);
                                row2.Cells.Add(row2_cell13);

                                table.Controls.Add(row2);
                             
                            }      
                          
                        }

                        dvContent.Controls.Add(table);
                    }
            
                    hdfRows.Value = rows.ToString();

                    hdfPayMode.Value = policy.Pay_Mode.ToString();
                    hdfProductID.Value = policy.Product_ID;
                    hdfSumInsure.Value = policy.Sum_Insure.ToString();
                    hdfPolicyID.Value = policy.Policy_ID;

                #endregion
            }

        }
    } 

    //sub generate list for save
    private void saveList()
    {
        string policy_number = txtPolicyNumber.Text.Trim();

        if (policy_number != "")
        {

            //Create new payment schedule table
            bl_policy_payment_schedule policy = da_policy_payment_schedule.GetPolicyDetails(policy_number);

            string pay_mode = da_payment_mode.GetPaymentModeByPayModeID(policy.Pay_Mode).Mode;

            DateTime effective_date = policy.Due_Date;

            int n = 0;
           
            switch (policy.Pay_Mode)
            {
                case 0:
                case 1:
                    n = 1;
                    break;
                case 2:
                    n = 2;
                    break;
                case 3:
                    n = 4;
                   break;
                case 4:
                    n = 12;
                    break;
            }


            //Loop Year
            for (int i = 1; i <= policy.Year; i++)
            {

                //Loop Pay Mode
                for (int j = 1; j <= n; j++)
                {
                    switch (policy.Pay_Mode)
                    {
                        case 0:
                        case 1:
                            policy.Due_Date = policy.Due_Date.AddYears(1);
                            break;
                        case 2:
                            policy.Due_Date = policy.Due_Date.AddMonths(6);
                            break;
                        case 3:
                            policy.Due_Date = policy.Due_Date.AddMonths(3);
                            break;
                        case 4:
                            policy.Due_Date = policy.Due_Date.AddMonths(1);
                            break;
                    }
                    DateTime due_date = policy.Due_Date;
                    DateTime my_date = new DateTime();
                   my_date= Calculation.GetNext_Due(policy.Due_Date, due_date, effective_date);

                   policy.Due_Date = my_date;

                    //get data from html table
                    Table tbl =new Table();
                    TableCell cell = new TableCell();
                    
                   tbl= (Table) this.FindControl("tblPaymentTable");

                   //for (int cellindex = 0; cellindex<=tbl.Rows[i].Cells.Count - 1; cellindex++)
                   //{
                       Double premium;
                       Double discount;
                       Double premium_after_discount;
                       Double em_premium;
                       Double total_premium;
                       string remark;
                       string createdby;

                       premium = Convert.ToDouble(tbl.Rows[i].Cells[4].ToString());
                       discount = Convert.ToDouble(tbl.Rows[i].Cells[5].ToString());
                       premium_after_discount = Convert.ToDouble(tbl.Rows[i].Cells[6].ToString());
                       em_premium = Convert.ToDouble(tbl.Rows[i].Cells[7].ToString());
                       total_premium = Convert.ToDouble(tbl.Rows[i].Cells[8].ToString());
                       remark = tbl.Rows[i].Cells[9].ToString();
                       createdby = policy.Created_By;
                       
                   //
                   

                  // RegisterStartupScript("alert", "alert ('" + tbl.Rows.Count + "');");

                   string result;
                    result = SavePolicyPaymentSchedule1(hdfPolicyID.Value.ToString(), hdfProductID.Value.ToString(), Convert.ToInt32(hdfPayMode.Value), 
                    Convert.ToDouble(hdfSumInsure.Value), policy.Due_Date, i, j, premium , discount, premium_after_discount, em_premium, total_premium, remark, createdby);
                   
                }
               
            }

        }
    }

    public string SavePolicyPaymentSchedule1(string policy_id, string product_id, Int32 pay_mode, Double sum_insure, DateTime due_date, Int32 year, Int32 time, Double premium, 
        Double discount, Double premium_after_discount, Double em_premium, Double total_premium, string remark, string user_name)
    {
        string result = "1";

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        //Delete By Policy ID
       // da_policy_payment_schedule.DeletePolicyPaymentScheduleByPolicyID(policy_id);

       
            try
            {
                string new_guid = Helper.GetNewGuid("SP_Check_Policy_Payment_Schedule_ID", "@Policy_Payment_Schedule_ID").ToString();

                bl_policy_payment_schedule policy_payment_schedule = new bl_policy_payment_schedule();

                policy_payment_schedule.Created_By = user_name;
                policy_payment_schedule.Created_Note = remark;
                policy_payment_schedule.Created_On = DateTime.Now;
                policy_payment_schedule.Discount = Convert.ToDouble(discount);
                policy_payment_schedule.Due_Date = Convert.ToDateTime(due_date, dtfi);
                policy_payment_schedule.Extra_Premium = Convert.ToDouble(em_premium);
                policy_payment_schedule.Pay_Mode = Convert.ToInt32(pay_mode);
                policy_payment_schedule.Policy_ID = policy_id;
                policy_payment_schedule.Policy_Payment_Schedule_ID = new_guid;
                policy_payment_schedule.Premium = Convert.ToDouble(premium);
                policy_payment_schedule.Premium_After_Discount = Convert.ToDouble(premium_after_discount);
                policy_payment_schedule.Product_ID = product_id;
                policy_payment_schedule.Sum_Insure = Convert.ToDouble(sum_insure);
                policy_payment_schedule.Time = Convert.ToInt32(time);
                policy_payment_schedule.Total_Premium = Convert.ToDouble(total_premium);
                policy_payment_schedule.Year = Convert.ToInt32(year);

                if (!da_policy_payment_schedule.InsertPolicyPaymentSchedule(policy_payment_schedule))
                {
                    result = "0";
                }

            }
            catch (Exception ex)
            {
                
                result = "0";
                //Add error to log 
                Log.AddExceptionToLog("Error in function [SavePolicyPaymentSchedule] in Page [policy_payment_schedule.aspx]. Details: " + ex.Message);
            }
       

        return result;
    }
    protected void save_Click(object sender, EventArgs e)
    {
        //saveList();

        int row;
      
        string str;
        //HtmlTable tbl = (HtmlTable)this.FindControl("tblPaymentTable");
        HtmlTable tbl = (HtmlTable)dvContent.FindControl("tblPaymentTable");
        Label lbl = (Label)dvContent.FindControl("lblmessage");

        HiddenField h = (HiddenField)dvContent.FindControl("hdfPremiumAfterDiscount_1_9");

        //Control myControl = dvContent.FindControl("save");

        //if (myControl.ID != null)
        //{
         Response.Write("<script>alert('" + h.ID + "');</script>");

        //}
        //else
        //{
        //    Response.Write("<script>alert('Nothing');</script>");
        //}
        //row = tbl.Rows.Count;
        // string script = "<script type='text/javascript'>alert('" + row + "');</script>";
        // Page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
       
    }
    //make data table
    public void loadExistPolicyPaymentSchedule(string policy_number)
    {

        //draw table
        Table table = new Table();
        table.ID = "tblPaymentTable";
        table.Style.Add("width", "100%");
        table.Style.Add("border", "1px solid #000000");


        TableRow header = new TableRow();

        //Year
        TableCell cell1 = new TableCell();
        cell1.Style.Add("width", "40px");
        cell1.Style.Add("border", "1px solid #000000");
        cell1.Style.Add("text-align", "center");
        cell1.Style.Add("font-weight", "bold");
        cell1.Text = "Year";

        //Product
        TableCell cell2 = new TableCell();
        cell2.Text = "Product";
        cell2.Style.Add("width", "80px");
        cell2.Style.Add("border", "1px solid #000000");
        cell2.Style.Add("text-align", "center");
        cell2.Style.Add("font-weight", "bold");

        //Pay Mode
        TableCell cell3 = new TableCell();
        cell3.Text = "Pay Mode";
        cell3.Style.Add("border", "1px solid #000000");
        cell3.Style.Add("text-align", "center");
        cell3.Style.Add("font-weight", "bold");

        //Sum Insure
        TableCell cell4 = new TableCell();
        cell4.Text = "Sum Insure";
        cell4.Style.Add("border", "1px solid #000000");
        cell4.Style.Add("text-align", "center");
        cell4.Style.Add("font-weight", "bold");

        //Premium
        TableCell cell5 = new TableCell();
        //cell7.Text = "Premium";
        cell5.Text = "Premium/Year";
        cell5.Style.Add("border", "1px solid #000000");
        cell5.Style.Add("text-align", "center");
        cell5.Style.Add("font-weight", "bold");

        //Discount
        TableCell cell6 = new TableCell();
        cell6.Text = "Discount/Year";
        cell6.Style.Add("border", "1px solid #000000");
        cell6.Style.Add("text-align", "center");
        cell6.Style.Add("font-weight", "bold");

        //Premium After Discount
        TableCell cell7 = new TableCell();
        cell7.Text = "Premium After Discount";
        cell7.Style.Add("border", "1px solid #000000");
        cell7.Style.Add("text-align", "center");
        cell7.Style.Add("font-weight", "bold");

        //EM Premium
        TableCell cell8 = new TableCell();
        cell8.Text = "EM Premium";
        cell8.Style.Add("border", "1px solid #000000");
        cell8.Style.Add("text-align", "center");
        cell8.Style.Add("font-weight", "bold");

        //Total Premium
        TableCell cell9 = new TableCell();
        cell9.Text = "Total Premium";
        cell9.Style.Add("border", "1px solid #000000");
        cell9.Style.Add("text-align", "center");
        cell9.Style.Add("font-weight", "bold");

        //Remark
        TableCell cell10 = new TableCell();
        cell10.Text = "Remark";
        cell10.Style.Add("border", "1px solid #000000");
        cell10.Style.Add("text-align", "center");
        cell10.Style.Add("font-weight", "bold");

        //Action
        TableCell cell11 = new TableCell();
        cell11.Text = "Action";
        cell11.Style.Add("border", "1px solid #000000");
        cell11.Style.Add("text-align", "center");
        cell11.Style.Add("font-weight", "bold");

        header.Cells.Add(cell1);
        header.Cells.Add(cell2);
        header.Cells.Add(cell3);
        header.Cells.Add(cell4);
        header.Cells.Add(cell5);
        header.Cells.Add(cell6);
        header.Cells.Add(cell7);
        header.Cells.Add(cell8);
        header.Cells.Add(cell9);
        header.Cells.Add(cell10);
        header.Cells.Add(cell11);

        table.Rows.Add(header);

        DataTable tbl = new DataTable();
        tbl = da_policy_payment_schedule.GetExistPolicyPaymentSchedule(policy_number);
        int rows;
        rows = tbl.Rows.Count;

        string strPolicy="";
        string strProductid = "";
        Double strPremium = 0;
        Double strSumInSure = 0;
        Double strExtra_Premium = 0;
        int strPay_Mode = 0;
        Double strdiscount_amount = 0;
        String strNote = "";
        Double strTotalPremium = 0;
        Double strPremium_after_dis = 0;

        int j=0;
        try
        {
            for (int i = 0; i <= rows - 1; i++)
            {

                strPolicy = tbl.Rows[i]["Policy_id"].ToString();
                strProductid = tbl.Rows[i]["Product_id"].ToString();
                strPremium = Convert.ToDouble(tbl.Rows[i]["Original_Amount"]);
                //round up
                strPremium = System.Math.Ceiling(strPremium);
                strSumInSure = Convert.ToDouble(tbl.Rows[i]["Sum_Insure"]);
                strExtra_Premium = Convert.ToDouble(tbl.Rows[i]["Extra_Premium"]);
                //roundup
                strExtra_Premium = System.Math.Ceiling(strExtra_Premium);

                strPay_Mode =Convert.ToInt32(tbl.Rows[i]["Pay_mode"]);
                strdiscount_amount = Convert.ToDouble(tbl.Rows[i]["Discount_amount"]);
                strNote = Convert.ToString(tbl.Rows[i]["Created_note"]);
                strPremium_after_dis = strPremium - strdiscount_amount;
                strTotalPremium = strPremium_after_dis + strExtra_Premium;

                j += 1;

                //Year
                TableCell row_cell1 = new TableCell();
                TableRow row = new TableRow();
          
                row_cell1.Style.Add("text-align", "center");
                row_cell1.Style.Add("border", "1px solid #000000");
                row_cell1.Text = Convert.ToString(j);

                //Product
                TableCell row_cell2 = new TableCell();
                row_cell2.Style.Add("text-align", "center");
                row_cell2.Style.Add("border", "1px solid #000000");
                row_cell2.Text = strProductid.ToString();

                //Pay Mode
                TableCell row_cell3 = new TableCell();
                row_cell3.Style.Add("text-align", "center");
                row_cell3.Style.Add("border", "1px solid #000000");
                row_cell3.Text = strPay_Mode.ToString();

                //Sum Insure
                TableCell row_cell4 = new TableCell();
                row_cell4.Style.Add("text-align", "right");
                row_cell4.Style.Add("padding-right", "5px");
                row_cell4.Style.Add("border", "1px solid #000000");
                row_cell4.Text = strSumInSure.ToString("C0");

                //Premium
                TableCell row_cell5 = new TableCell();
                row_cell5.Style.Add("border", "1px solid #000000");
                row_cell5.Style.Add("text-align", "center");
                row_cell5.Style.Add("padding-top", "5px");
                TextBox premium_textbox = new TextBox();

                premium_textbox.ID = "txtPremium_" + i + "_" + 5;
                premium_textbox.MaxLength = 50;
                premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                premium_textbox.Width = 100;
                premium_textbox.Text =strPremium.ToString("C0");
                premium_textbox.ReadOnly = true;
                premium_textbox.Enabled = false;

                HiddenField premium_hdf = new HiddenField();
                premium_hdf.ID = "hdfPremium_" + i + "_" + 5;
                premium_hdf.Value = strPremium.ToString();

                HiddenField hdf_Year = new HiddenField();
                hdf_Year.ID = "hdfYear_" + i + "_" + 1;
                hdf_Year.Value = j.ToString();

                HiddenField hdf_Policy_ID = new HiddenField();
                hdf_Policy_ID.ID = "hdfPolicy_ID_" + i + "_" + 1;
                hdf_Policy_ID.Value = strPolicy.ToString();

                HiddenField hdf_Product_ID = new HiddenField();
                hdf_Product_ID.ID = "hdfProduct_ID_" + i + "_" + 1;
                hdf_Product_ID.Value = strProductid.ToString();

                HiddenField hdf_Pay_Mode = new HiddenField();
                hdf_Pay_Mode.ID = "hdfPay_Mode_" + i + "_" + 1;
                hdf_Pay_Mode.Value = strPay_Mode.ToString();


                row_cell5.Controls.Add(hdf_Year);
                row_cell5.Controls.Add(premium_textbox);
                row_cell5.Controls.Add(premium_hdf);
                row_cell5.Controls.Add(hdf_Policy_ID);
                row_cell5.Controls.Add(hdf_Product_ID);
                row_cell5.Controls.Add(hdf_Pay_Mode);

                //Discount
                TableCell row_cell6 = new TableCell();
                row_cell6.Style.Add("border", "1px solid #000000");
                row_cell6.Style.Add("text-align", "center");
                row_cell6.Style.Add("padding-top", "5px");

                TextBox discount_textbox = new TextBox();
                discount_textbox.Text = strdiscount_amount.ToString();
                discount_textbox.ID = "txtDiscount_" + i + "_" + 6;
                discount_textbox.MaxLength = 50;
                discount_textbox.Attributes.Add("onkeyup", "ValidateNumber(this); UpdatePremium(this.value, \"" + i + "col" + 6 + "\");");
                discount_textbox.Width = 100;
                discount_textbox.Attributes.Add("runat", "server");

                row_cell6.Controls.Add(discount_textbox);

                //Premium After Discount
                TableCell row_cell7 = new TableCell();
                row_cell7.Style.Add("border", "1px solid #000000");
                row_cell7.Style.Add("text-align", "center");
                row_cell7.Style.Add("padding-top", "5px");

                TextBox premium_after_discount_textbox = new TextBox();
                premium_after_discount_textbox.Text = strPremium_after_dis.ToString("C0");
                premium_after_discount_textbox.ID = "txtPremiumAfterDiscount_" + i + "_" + 7;
                premium_after_discount_textbox.MaxLength = 50;
                premium_after_discount_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                premium_after_discount_textbox.Width = 120;
                premium_after_discount_textbox.ReadOnly = true;
                premium_after_discount_textbox.Enabled = false;

                HiddenField premium_after_discount_hdf = new HiddenField();
                premium_after_discount_hdf.ID = "hdfPremiumAfterDiscount_" + i + "_" + 7;
                premium_after_discount_hdf.Value = strPremium_after_dis.ToString();

                row_cell7.Controls.Add(premium_after_discount_textbox);
                row_cell7.Controls.Add(premium_after_discount_hdf);

                //EM Premium
                TableCell row_cell8 = new TableCell();
                row_cell8.Style.Add("border", "1px solid #000000");
                row_cell8.Style.Add("text-align", "center");
                row_cell8.Style.Add("padding-top", "5px");

                TextBox em_premium_textbox = new TextBox();

                em_premium_textbox.ID = "txtEMPremium_" + i + "_" + 8;
                em_premium_textbox.MaxLength = 50;
                em_premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                em_premium_textbox.Width = 100;
                em_premium_textbox.ReadOnly = true;
                em_premium_textbox.Enabled = false;
                em_premium_textbox.Text = strExtra_Premium.ToString();

                HiddenField em_premium_hdf = new HiddenField();
                em_premium_hdf.ID = "hdfEMPremium_" + i + "_" + 8;
                em_premium_hdf.Value = strExtra_Premium.ToString();

                row_cell8.Controls.Add(em_premium_textbox);
                row_cell8.Controls.Add(em_premium_hdf);

                //Total Premium
                TableCell row_cell9 = new TableCell();
                row_cell9.Style.Add("border", "1px solid #000000");
                row_cell9.Style.Add("text-align", "center");
                row_cell9.Style.Add("padding-top", "5px");

                TextBox total_premium_textbox = new TextBox();

                total_premium_textbox.ID = "txtTotalPremium_" + i + "_" + 9;
                total_premium_textbox.MaxLength = 50;
                total_premium_textbox.Attributes.Add("onkeyup", "ValidateNumber(this);");
                total_premium_textbox.Width = 100;
                total_premium_textbox.ReadOnly = true;
                total_premium_textbox.Enabled = false;

                HiddenField total_premium_hdf = new HiddenField();
                total_premium_hdf.ID = "hdfTotalPremium_" + i + "_" + 9;

                total_premium_textbox.Text = strTotalPremium.ToString("C0");
                total_premium_hdf.Value = strTotalPremium.ToString();
                    
                row_cell9.Controls.Add(total_premium_textbox);
                row_cell9.Controls.Add(total_premium_hdf);

                //Remark
                TableCell row_cell10 = new TableCell();
                row_cell10.Style.Add("border", "1px solid #000000");
                row_cell10.Style.Add("text-align", "center");
                row_cell10.Style.Add("padding-top", "5px");

                TextBox remark_textbox = new TextBox();
                remark_textbox.Text = strNote.ToString();
                remark_textbox.ID = "txtRemark_" + i + "_" + 10;
                remark_textbox.MaxLength = 255;
                remark_textbox.Width = 100;

                row_cell10.Controls.Add(remark_textbox);

                //Action
                TableCell row_cell11 = new TableCell();
                row_cell11.Style.Add("border", "1px solid #000000");
                row_cell11.Style.Add("text-align", "center");
                row_cell11.Style.Add("padding-top", "5px");

                Button btnUpdate = new Button();
                btnUpdate.Style.Add("background-image", "url('../../App_Themes/functions/Update.png')");
                btnUpdate.ID = "btnUpdate" + i + "_" + 11;
                btnUpdate.Width = 25;
                btnUpdate.Attributes.Add("onclick", "UpdatePolicyPaymentSchedule(" + i + ");");
                row_cell11.Controls.Add(btnUpdate);

                row.Cells.Add(row_cell1);
                row.Cells.Add(row_cell2);
                row.Cells.Add(row_cell3);
                row.Cells.Add(row_cell4);
                row.Cells.Add(row_cell5);
                row.Cells.Add(row_cell6);
                row.Cells.Add(row_cell7);
                row.Cells.Add(row_cell8);
                row.Cells.Add(row_cell9);
                row.Cells.Add(row_cell10);
                row.Cells.Add(row_cell11);

                table.Controls.Add(row);

            }
            dvContent.Controls.Add(table);
            string ip = "";
            ip = GetIPAddress();
            Log.AddExceptionToLog(ip);
        }
        catch (Exception ex)
        {
           
            Log.AddExceptionToLog("Error in function [loadExistPolicyPaymentSchedule] in form [policy_payment_schedule]. Details: " + ex.Message);
        }
    }

    protected string GetIPAddress()
    {
        //string strHostName = "";
        //string clientIPAddress = "";
        //try
        //{
        //    strHostName = System.Net.Dns.GetHostName();
        //    clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();

        //}
        //catch (Exception ex)
        //{

        //}
        //return "[Computer Name: " + strHostName + "]  [IP Address: " + clientIPAddress + "]";
        
        //Browser info

        string strUserAgent="";
        string strBrowser = "";
        string strBrowserVersion = "";
               strUserAgent = Request.UserAgent;
        strBrowser = Request.Browser.Browser;
        strBrowserVersion = Request.Browser.Version;
      
        string ipaddress;
        ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (ipaddress == "" || ipaddress == null)
            ipaddress = Request.ServerVariables["REMOTE_ADDR"];
        return "[Client IP Address: " + ipaddress + "] [Browser Info: " + strUserAgent + ";  " + strBrowser +";   " + strBrowserVersion+"]";
    }
}