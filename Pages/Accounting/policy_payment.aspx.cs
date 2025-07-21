using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Windows.Forms;
using System.Globalization;

public partial class Pages_Policy_Payment_policy_payment : System.Web.UI.Page
{
    string userid, user_name;

    protected void Page_Load(object sender, EventArgs e)
    {
        txtPoliNumberSearch.Attributes.Add("onkeypress", "return EnterPress('" + txtPoliNumberSearch.ClientID + "', event)");
    }

    void ClearText()
    {
        txtPeriod.Text = "";
        txtReceiptNo.Text = "";
        txtPayDate.Text = "";
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        /// Insert into Ct_Policy_Prem_Pay

        int i = 1, n = 0; double prem = 0; string unique_id = "";

        if (txtPeriod.Text != "") { n = int.Parse(txtPeriod.Text); } else { n = 0; }

        TimeSpan end_date = new TimeSpan(00, 00, 00);

        /// Calculate Interest
        double amount_interest = 0;

        string[] interest_value = hdf_Totalinterest.Value.Split(',');

        if (interest_value.Count() > 0 && interest_value[0] !="")
        {
            amount_interest = double.Parse(interest_value[0]);

            amount_interest = Math.Round(amount_interest);
        }
        
        /// 

        if (n != 0) { prem = double.Parse(hdfPrem.Value) / n; } //{ prem = (double.Parse(hdfPrem.Value) - amount_interest) / n; }

        while (i<=n)
        {
            string new_guid = Helper.GetNewGuid("Check_Duplicate_Prem_Pay_ID", "@Policy_Prem_Pay_ID").ToString();

            unique_id += new_guid + ",";

            bl_policy_prem_pay policy_prem_pay = new bl_policy_prem_pay();

            policy_prem_pay.Policy_Prem_Pay_ID = new_guid;
            policy_prem_pay.Policy_ID = hdfPlicyID.Value;

            if (i == n)
            {
                policy_prem_pay.Due_Date = DateTime.Parse(hdfNextDue.Value,dtfi).Date + end_date;
            }
            else {
                if (i == 1)
                {
                    DateTime due_date =DateTime.Parse(hdfDue.Value,dtfi); //due_date = DateTime.Parse(due_date.Year + "/" + due_date.Day + "/" + due_date.Month );
                    policy_prem_pay.Due_Date = da_policy_prem_pay.GetNext_Due(int.Parse(ddlPayMode.SelectedValue), due_date).Date + end_date;
                }
                else
                {
                    policy_prem_pay.Due_Date = policy_prem_pay.Due_Date = da_policy_prem_pay.GetNext_Due(int.Parse(ddlPayMode.SelectedValue), da_policy_prem_pay.GetLast_Due_Date(hdfPlicyID.Value)).Date + end_date;
                }
            }

            policy_prem_pay.Pay_Date = DateTime.Parse(txtPayDate.Text,dtfi).Date + end_date;
            policy_prem_pay.Prem_Year = da_policy_prem_pay.GetPrem_Year(hdfPlicyID.Value); 
            policy_prem_pay.Prem_Lot = da_policy_prem_pay.GetPrem_Lot(hdfPlicyID.Value, int.Parse(ddlPayMode.SelectedValue),policy_prem_pay.Prem_Year);
            policy_prem_pay.Amount = prem;
            policy_prem_pay.Sale_Agent_ID = hdfSaleAgentID.Value;
            policy_prem_pay.Office_ID = hdfOfficeID.Value;
            policy_prem_pay.Created_On = DateTime.Now;
            policy_prem_pay.Created_By = user_name;
            policy_prem_pay.Created_Note = "";
            policy_prem_pay.Pay_Mode_ID = int.Parse(ddlPayMode.SelectedValue);


            if (da_policy_prem_pay.InsertPolicy_Pre_Pay(policy_prem_pay) == true)
            {

                /// If Pay Mode is difference, update Ct_Policy_Pay_Mode
                if (da_policy_prem_pay.GetPay_Mode_by_PolicyID(hdfPlicyID.Value) != int.Parse(ddlPayMode.SelectedValue))
                {
                    da_policy_prem_pay.Update_Pay_Mode(hdfPlicyID.Value, int.Parse(ddlPayMode.SelectedValue));
                }

                /// Insert into table Ct_Payment_Receipt

                string Receipt_ID = Helper.GetNewGuid("SP_Check_Payment_Receipt_ID", "@Receipt_ID").ToString();

                bl_payment_receipt payment_receipt = new bl_payment_receipt();
                payment_receipt.Receipt_ID = Receipt_ID;
                payment_receipt.Policy_Prem_Pay_ID = new_guid;
                payment_receipt.Receipt_Num = txtReceiptNo.Text.ToUpper();
                payment_receipt.Payment_Mode =int.Parse(ddlMethod_Payment.SelectedValue);
                payment_receipt.Created_By = user_name;
                payment_receipt.Created_On = DateTime.Now;
                payment_receipt.Rate_Lapsed = 0;

                da_payment_receipt.Insert_Payment_Receipt(payment_receipt);

                if (da_payment_receipt.Check_Policy_Status(hdfPlicyID.Value) == true) // If LAP ?
                {
                    da_payment_receipt.Update_Policy_Status(hdfPlicyID.Value, "IF"); /// Update to IF
                }
                else
                {
                    /// Check Pay Year
                    if (da_payment_receipt.Check_Pay_Year(hdfPlicyID.Value) == true)
                    {
                        da_payment_receipt.Update_Policy_Status(hdfPlicyID.Value, "MAT"); /// Update to MATURITY STATUS
                    }
                }
            }

            i = i + 1;
        } // End of while loop


        if (unique_id != "")
        {
            decimal interest_amount=0;

            string[] u_id =unique_id.Split(',');

            string[] aa = hdf_interest.Value.Split('/');

            for (int iii = 0; iii <= u_id.Count() -1; iii++)
            {
                for (int nn = iii; nn <= iii ; nn++)
                {
                    if (u_id[iii] != "")
                    {
                        if(aa[nn] !="")
                        {
                            interest_amount = decimal.Parse(aa[nn]);
                        }

                        da_payment_receipt.Update_Interest_Amount(u_id[iii], interest_amount);
                   }
                }
            }
        }

        ClearText();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        MembershipUser myUser = Membership.GetUser();
        userid = myUser.ProviderUserKey.ToString();
        user_name = myUser.UserName;

        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        /// Insert into Ct_Policy_Prem_Pay
        int i = 1, n = 0;
        if (txtPeriod.Text != "") { n = int.Parse(txtPeriod.Text); } else { n = 0; }

        TimeSpan end_date = new TimeSpan(00, 00, 00);

        double prem = 0;

        if (n != 0) { prem = double.Parse(hdfPrem.Value) / n; }

        while (i <= n)
        {
            string new_guid = Helper.GetNewGuid("Check_Duplicate_Prem_Pay_ID", "@Policy_Prem_Pay_ID").ToString();

            bl_policy_prem_pay policy_prem_pay = new bl_policy_prem_pay();

            policy_prem_pay.Policy_Prem_Pay_ID = new_guid;
            policy_prem_pay.Policy_ID = hdfPlicyID.Value;

            if (i == n)
            {
                policy_prem_pay.Due_Date = DateTime.Parse(hdfNextDue.Value,dtfi).Date + end_date;
            }
            else
            {
                if (i == 1)
                {
                    DateTime due_date = DateTime.Parse(hdfDue.Value,dtfi); //due_date = DateTime.Parse(due_date.Year + "/" + due_date.Day + "/" + due_date.Month);
                    policy_prem_pay.Due_Date = da_policy_prem_pay.GetNext_Due(int.Parse(ddlPayMode.SelectedValue), due_date).Date + end_date;
                }
                else
                {
                    policy_prem_pay.Due_Date = da_policy_prem_pay.GetNext_Due(int.Parse(ddlPayMode.SelectedValue), da_policy_prem_pay.GetLast_Due_Date(hdfPlicyID.Value)).Date + end_date;
                }
            }

            policy_prem_pay.Pay_Date = DateTime.Parse(txtPayDate.Text, dtfi).Date + end_date;
            policy_prem_pay.Prem_Year = da_policy_prem_pay.GetPrem_Year(hdfPlicyID.Value);
            policy_prem_pay.Prem_Lot = da_policy_prem_pay.GetPrem_Lot(hdfPlicyID.Value, int.Parse(ddlPayMode.SelectedValue), policy_prem_pay.Prem_Year);
            policy_prem_pay.Amount = prem;
            policy_prem_pay.Sale_Agent_ID = hdfSaleAgentID.Value;
            policy_prem_pay.Office_ID = hdfOfficeID.Value;
            policy_prem_pay.Created_On = DateTime.Now;
            policy_prem_pay.Created_By = user_name;
            policy_prem_pay.Created_Note = "";
            policy_prem_pay.Pay_Mode_ID = int.Parse(ddlPayMode.SelectedValue);

            if (da_policy_prem_pay.InsertPolicy_Pre_Pay(policy_prem_pay) == true)
            {
                /// If Pay Mode is difference, update Ct_Policy_Pay_Mode
                if (da_policy_prem_pay.GetPay_Mode_by_PolicyID(hdfPlicyID.Value) != int.Parse(ddlPayMode.SelectedValue))
                {
                    da_policy_prem_pay.Update_Pay_Mode(hdfPlicyID.Value, int.Parse(ddlPayMode.SelectedValue));
                }

                /// Insert into table Ct_Payment_Receipt

                bl_payment_receipt payment_receipt = new bl_payment_receipt();
               
                payment_receipt.Policy_Prem_Pay_ID = new_guid;
                payment_receipt.Receipt_Num = txtReceiptNo.Text.ToUpper();
                payment_receipt.Payment_Mode = int.Parse(ddlMethod_Payment.SelectedValue);
                payment_receipt.Created_By = user_name;
                payment_receipt.Created_On = DateTime.Now;

                if (da_payment_receipt.Check_Duplicate_Payment_Receipt(payment_receipt.Receipt_Num) != "")
                {
                    payment_receipt.Receipt_ID=hdfReceipt_ID.Value;
                    da_payment_receipt.Update_Payment_Receipt(payment_receipt); 
                }
                else {
                    string Receipt_ID = Helper.GetNewGuid("SP_Check_Payment_Receipt_ID", "@Receipt_ID").ToString();
                    
                    payment_receipt.Receipt_ID = Receipt_ID;

                    da_payment_receipt.Insert_Payment_Receipt(payment_receipt); 
                }

                ClearText();
            }

            i = i + 1;
        } // End of while loop
    }

}