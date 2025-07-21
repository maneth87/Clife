using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GTLI_gtli_issue_policy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Load_Grid();
        }
    }

    //Load Grid
    protected void Load_Grid()
    {

        List<bl_gtli_premium> list_of_gtli_premium_temporary = new List<bl_gtli_premium>();
        
        //Empty GridView
        gvPolicyTemp.DataSource = list_of_gtli_premium_temporary;
        gvPolicyTemp.DataBind();

        //Populate  List
        list_of_gtli_premium_temporary = da_gtli_premium_temporary.GetGTLPremiumTemporayList();

        //list only renew status, Transaction_Type=1
        List<bl_gtli_premium> list_of_gtli_premium = new List<bl_gtli_premium>();
        for (int i = 0; i <= list_of_gtli_premium_temporary.Count - 1; i++)
        {
            if (list_of_gtli_premium_temporary[i].Transaction_Type == 1) {
                list_of_gtli_premium.Add(list_of_gtli_premium_temporary[i]);
            }
           
        }


        //gvPolicyTemp.DataSource = list_of_gtli_premium_temporary;
        //gvPolicyTemp.DataBind();
        gvPolicyTemp.DataSource = list_of_gtli_premium;
        gvPolicyTemp.DataBind();
              
    }

    //Cancel premium temporary
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Delete temporary data
        da_gtli_employee_premium_temporary.DeleteEmployeePremiumTemporary(hdfPremiumID.Value);
        da_gtli_employee_temporary.DeleteGTLIEmployeeTemporary(hdfPremiumID.Value);
        da_gtli_policy_temporary.DeleteGTLIPolicyTemporary(hdfPolicyID.Value);
        da_gtli_certificate_temporary.DeleteGTLICertificateTemporary(hdfPremiumID.Value);
        da_gtli_premium_temporary.DeleteGTLIPremiumTemporary(hdfPremiumID.Value);
        da_gtli_policy_prem_pay_temporary.DeleteGTLIPolicyPremPayTemporary(hdfPremiumID.Value);
        da_gtli_policy_prem_return_temporary.DeleteGTLIPolicyPremReturnTemporary(hdfPremiumID.Value);

        //Reload Grid
        Load_Grid();
    }

    //Issue this premium
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool is_new_policy = false;

        //temporary variables
        string policy_temporary_id = "";
        string certificate_temporary_id = "";
        
        //get premium id
        string premium_temporay_id = hdfPremiumID.Value;

        //get gtli premium temporary by id
        bl_gtli_premium premium_temporary = new bl_gtli_premium();
        premium_temporary = da_gtli_premium_temporary.GetGTLIPremiumTemporayByID(premium_temporay_id);

        bl_gtli_policy my_policy = new bl_gtli_policy();

        if (!da_gtli_policy_temporary.CheckPolicyID(premium_temporary.GTLI_Policy_ID))
        {
            my_policy = da_gtli_policy.GetGTLIPolicyByID(premium_temporary.GTLI_Policy_ID);
            policy_temporary_id = my_policy.GTLI_Policy_ID;
        }
        else
        {
            my_policy = da_gtli_policy_temporary.GetGTLIPolicyTemporayByID(premium_temporary.GTLI_Policy_ID);

            policy_temporary_id = my_policy.GTLI_Policy_ID; //Save policy id of temporary

            //Get new policy number
            //my_policy.Policy_Number = da_gtli_policy.GetPolicyNumber();

            my_policy.Maturity_Date = my_policy.Expiry_Date.AddDays(1);

            //Insert new policy
            my_policy.GTLI_Policy_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_ID", "@GTLI_Policy_ID"); //Acquire new unique real policy id

            if (!da_gtli_policy.InsertPolicy(my_policy))
            {

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                return;
            }
        }          
           

        //Set new gtli policy id to gtli premium
        premium_temporary.GTLI_Policy_ID = my_policy.GTLI_Policy_ID;
        premium_temporary.GTLI_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Premium_ID", "@GTLI_Premium_ID"); //Acquire new unique real premium id

        //Insert GTLI Premium
        if (da_gtli_premium.InsertPremium(premium_temporary))
        {
            //Get Employee Temporary list by premium_id
            ArrayList employee_temporary_list = new ArrayList();
            employee_temporary_list = da_gtli_employee_temporary.GetGTLIEmployeeTemporayListByPremiumID(premium_temporay_id);

            //loop through employee temporary list
            for (int i = 0; i <= employee_temporary_list.Count - 1; i++)
            {
                bl_gtli_employee employee_temporary = new bl_gtli_employee();
                employee_temporary = (bl_gtli_employee)employee_temporary_list[i];

                //Get Certificate by certificate_id
                bl_gtli_certificate certificate = new bl_gtli_certificate();
                certificate = da_gtli_certificate_temporary.GetGTLICertificateTemporayByID(employee_temporary.GTLI_Certificate_ID);

                //temporary certificate_id
                string certificate_id = certificate.GTLI_Certificate_ID;

                if (premium_temporary.Transaction_Type != 3) //New policy or add new member -> Insert new certificate and employee
                {
                    //Get last certificate number by this company and policy number
                    //certificate.Certificate_Number = da_gtli_employee.GetLastCertificateNoByPolicyID(my_policy.GTLI_Policy_ID);
                    // certificate.Certificate_Number = certificate.Certificate_Number + 1;
                   
                    //get certificate number from tamp table, by maneth
                    certificate.Certificate_Number = certificate.Certificate_Number;

                    certificate_temporary_id = certificate.GTLI_Certificate_ID; //Save id of temporary for later use

                    certificate.GTLI_Certificate_ID = Helper.GetNewGuid("SP_Check_GTLI_Certificate_ID", "@GTLI_Certificate_ID"); //Acquire new unique real certificate id

                    //Insert Certificate
                    if (da_gtli_certificate.InsertCertificate(certificate))
                    {
                        //Insert Employee   

                        //Set new gtli certificate id to gtli employee
                        employee_temporary.GTLI_Certificate_ID = certificate.GTLI_Certificate_ID;

                        if (da_gtli_employee.InsertGTLIEmployee(employee_temporary))
                        {

                            //check if this customer exist in general policy customer (Clife)
                            char[] space = new char[] { ' ' };
                            string[] name = employee_temporary.Employee_Name.Split(space);

                            string first_name = "";
                            string last_name = name[0];

                            for(int n = 1; n < name.Count(); n++){
                                if (n == 1)
                                {
                                    first_name += name[n];
                                }
                                else
                                {
                                    first_name += " " + name[n];
                                }
                            }


                            if (da_gtli_employee.CheckGTLIEmployeeInCtCustomer(first_name, last_name, employee_temporary.Gender, employee_temporary.DOB))
                            {
                                //Insert into Ct_Customer_GTLI_Customer table
                                string ct_customer_id = da_gtli_employee.GetCtCustomerID(first_name, last_name, employee_temporary.Gender, employee_temporary.DOB);
                                bl_ct_customer_gtli_employee ct_customer_gtli_employee = new bl_ct_customer_gtli_employee();

                                ct_customer_gtli_employee.Customer_ID = ct_customer_id;
                                ct_customer_gtli_employee.GTLI_Certificate_ID = employee_temporary.GTLI_Certificate_ID;

                                if (!da_ct_customer_gtli_employee.InsertCtCustomerGTLIEmployee(ct_customer_gtli_employee))
                                { 
                                    //Failed ct_customer_gtli_employee
                                    da_gtli_employee.DeleteCtCustomerGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee.DeleteGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee_premium.DeleteEmployeePremium(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                                    //delete just added policy transaction
                                    if (is_new_policy)
                                    {
                                        bl_gtli_policy last_policy = new bl_gtli_policy();
                                        last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(my_policy.GTLI_Company_ID);
                                        
                                        string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                                        if (policy_status != "IF") //Case last policy status is not infoce -> delete just add policy row
                                        {
                                            da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                                        }
                                    }

                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                                    return;
                                }
                            }

                            //Get gtli employee premium list by this employee
                            List<bl_gtli_employee_premium> employee_premium_list = new List<bl_gtli_employee_premium>();

                            employee_premium_list = da_gtli_employee_premium_temporary.GetPremiumList(certificate_id, premium_temporay_id);

                            for (int j = 0; j <= employee_premium_list.Count - 1; j++)
                            {
                                bl_gtli_employee_premium premium = new bl_gtli_employee_premium();
                                premium = employee_premium_list[j];

                                premium.GTLI_Employee_Premium_ID = Helper.GetNewGuid("SP_Check_GTLI_Employee_Premium_ID", "@GTLI_Employee_Premium_ID"); //Acquire new unique real employee premium id
                                premium.GTLI_Certificate_ID = employee_temporary.GTLI_Certificate_ID;
                                premium.GTLI_Premium_ID = premium_temporary.GTLI_Premium_ID;

                                //Insert employee premium
                                if (!da_gtli_employee_premium.InsertEmployeePremium(premium))
                                { //Failed Employee Premium
                                    //delete
                                    da_gtli_employee.DeleteCtCustomerGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee.DeleteGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_employee_premium.DeleteEmployeePremium(premium_temporary.GTLI_Premium_ID);
                                    da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                                    //delete just added policy transaction
                                    if (is_new_policy)
                                    {
                                        bl_gtli_policy last_policy = new bl_gtli_policy();
                                        last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(my_policy.GTLI_Company_ID);

                                        string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                                        if (policy_status != "IF") //Case last policy status is not infoce -> delete just add policy row
                                        {
                                            da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                                        }
                                    }

                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                                    return;
                                }
                            } //end loop employee premium temporary list
                                                        
                        }
                        else
                        { //Failed Employee
                            //delete
                            da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                            da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                            //delete just added policy transaction
                            if (is_new_policy)
                            {
                                bl_gtli_policy last_policy = new bl_gtli_policy();
                                last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(my_policy.GTLI_Company_ID);

                                string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                                if (policy_status != "IF") //Case last policy status is not infoce -> delete just add policy row
                                {
                                    da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                                }
                            }

                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                            return;
                        }


                    }
                    else
                    { //Failed Certificate

                        //delete
                        da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                        //delete just added policy transaction
                        if (is_new_policy)
                        {
                            bl_gtli_policy last_policy = new bl_gtli_policy();
                            last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(my_policy.GTLI_Company_ID);

                            string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                            if (policy_status != "IF") //Case last policy status is not infoce -> delete just add policy row
                            {
                                da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                            }
                        }

                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                        return;
                    }
                                       

                }
            

            } //End loop employee temporary list

            if (premium_temporary.Transaction_Type != 3) //New policy or add new member -> Insert prem pay
            {
                //Get policy prem pay temporary
                bl_gtli_policy_prem_pay prem_pay = new bl_gtli_policy_prem_pay();
                prem_pay = da_gtli_policy_prem_pay_temporary.GetGTLIPolicyPremPayByGTLIPremiumID(premium_temporay_id);

                prem_pay.Status = 2;           
                                

                //Insert policy prem pay
                prem_pay.GTLI_Premium_ID = premium_temporary.GTLI_Premium_ID;
                prem_pay.GTLI_Policy_Prem_Pay_ID = Helper.GetNewGuid("SP_Check_GTLI_Policy_Prem_Pay_ID", "@GTLI_Policy_Prem_Pay_ID");

                if (da_gtli_policy_prem_pay.InsertPolicyPremPay(prem_pay))
                {
                    //Update premium in ct_gtli_policy
                    da_gtli_policy.UpdatePremium(premium_temporary.Life_Premium, premium_temporary.TPD_Premium, premium_temporary.DHC_Premium, premium_temporary.Accidental_100Plus_Premium, premium_temporary.Accidental_100Plus_Premium_Discount, premium_temporary.Life_Premium_Discount, premium_temporary.TPD_Premium_Discount, premium_temporary.DHC_Premium_Discount, premium_temporary.Original_Accidental_100Plus_Premium, premium_temporary.Original_Life_Premium, premium_temporary.Original_TPD_Premium, premium_temporary.Original_DHC_Premium, premium_temporary.Accidental_100Plus_Premium_Tax_Amount, premium_temporary.Life_Premium_Tax_Amount, premium_temporary.TPD_Premium_Tax_Amount, premium_temporary.DHC_Premium_Tax_Amount, premium_temporary.GTLI_Policy_ID);
                   
                        
                    bl_gtli_policy_status gtli_policy_status = new bl_gtli_policy_status();
                    gtli_policy_status.Created_By = premium_temporary.Created_By;
                    gtli_policy_status.Created_Note = "";
                    gtli_policy_status.Created_On = premium_temporary.Created_On;
                    gtli_policy_status.GTLI_Policy_ID = my_policy.GTLI_Policy_ID;
                    gtli_policy_status.Policy_Status_Type_ID = "IF";

                    da_gtli_policy_status.InsertGTLIPolicyStatus(gtli_policy_status);

                    //Reach here all save successful

                    //Delete temporary data
                    da_gtli_employee_premium_temporary.DeleteEmployeePremiumTemporary(premium_temporay_id);
                    da_gtli_employee_temporary.DeleteGTLIEmployeeTemporary(premium_temporay_id);
                    da_gtli_policy_temporary.DeleteGTLIPolicyTemporary(policy_temporary_id);
                    da_gtli_certificate_temporary.DeleteGTLICertificateTemporary(premium_temporay_id);
                    da_gtli_premium_temporary.DeleteGTLIPremiumTemporary(premium_temporay_id);
                    da_gtli_policy_prem_pay_temporary.DeleteGTLIPolicyPremPayTemporary(premium_temporay_id);
                    da_gtli_policy_prem_return_temporary.DeleteGTLIPolicyPremReturnTemporary(premium_temporay_id);

                    Response.Redirect("add_new_transaction_detail.aspx?pid=" + premium_temporary.GTLI_Premium_ID);
                }
                else
                { //Failed Prem Pay
                    //delete
                    da_gtli_employee.DeleteCtCustomerGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                    da_gtli_employee.DeleteGTLIEmployeeByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                    da_gtli_certificate.DeleteGTLICertificateByGTLIPremiumID(premium_temporary.GTLI_Premium_ID);
                    da_gtli_employee_premium.DeleteEmployeePremium(premium_temporary.GTLI_Premium_ID);
                    da_gtli_premium.DeleteGTLIPremium(premium_temporary.GTLI_Premium_ID);

                    //delete just added policy transaction
                    if (is_new_policy)
                    {
                        bl_gtli_policy last_policy = new bl_gtli_policy();
                        last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(my_policy.GTLI_Company_ID);

                        string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                        if (policy_status != "IF") //Case last policy status is not infoce -> delete just add policy row
                        {
                            da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                        }
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
                    return;

                }
            }
           
          
        }
        else
        { //Failed Premium
                    
            //delete just added policy transaction
            if (is_new_policy)
            {
                bl_gtli_policy last_policy = new bl_gtli_policy();
                last_policy = da_gtli_policy.GetLastGTLIPolicyBYCompanyID(my_policy.GTLI_Company_ID);

                string policy_status = da_gtli_policy.GetGTLIPolicyStatusBYGTLIPolicyID(last_policy.GTLI_Policy_ID);

                if (policy_status != "IF") //Case last policy status is not infoce -> delete just add policy row
                {
                    da_gtli_policy.DeleteGTLIPolicy(premium_temporary.GTLI_Policy_ID);
                }
            }
                       
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Failed. Please check it again.')", true);
            return;
        }
            
       
    }

    //View Details
    protected void gvPolicyTemp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Get premium ID
        string premium_id = Convert.ToString(e.CommandArgument);

        if (e.CommandName == "DetailSale")
        {
            
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            HiddenField hdfTransactionType = row.FindControl("hdfTransactionType") as HiddenField;

            switch (hdfTransactionType.Value)
            {
                case "1":               
                    Response.Redirect("../GTLI/create_new_transaction_underwrite_detail.aspx?pid=" + premium_id);
                    break;
              
            }
            
        }
    }

    //Grid Row Data Bound
    protected void gvPolicyTemp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Cell Life Premium
            if (e.Row.Cells[7].Text.Trim() == "$0.00")
            {
                e.Row.Cells[7].Text = "-";
            }

            //Cell 100Plus Premium
            if (e.Row.Cells[8].Text.Trim() == "$0.00")
            {
                e.Row.Cells[8].Text = "-";
            }

            //Cell TPD Premium
            if (e.Row.Cells[9].Text.Trim() == "$0.00")
            {
                e.Row.Cells[9].Text = "-";
            }

            //Cell DHC Premium
            if (e.Row.Cells[10].Text.Trim() == "$0.00")
            {
                e.Row.Cells[10].Text = "-";
            }

            //Cell Total Premium
            if (e.Row.Cells[11].Text.Trim() == "$0.00")
            {
                e.Row.Cells[11].Text = "-";
            }
        }
    }
}