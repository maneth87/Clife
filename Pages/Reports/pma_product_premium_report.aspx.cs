using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Data;


public partial class Pages_Reports_pma_product_premium_report : System.Web.UI.Page
{
    string user_name = "";
    string user_id = "";
    //First Page Load Event    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            System.Web.Security.MembershipUser myUser = System.Web.Security.Membership.GetUser();
            user_id = myUser.ProviderUserKey.ToString();
            user_name = myUser.UserName;

            //string page_name = Path.GetFileName(Request.Url.AbsolutePath);
            //da_user_access user_acc = new da_user_access();
            //if (user_acc.GetActiveUserAccessPage(page_name, user_id).UserId != user_id)
            //{
            //    div_main_Toolbar.Attributes.CssStyle.Add("display", "none");
            //    div_main.Attributes.CssStyle.Add("display", "none");
            //    showMessage("No permission to access this page!", "1");
            //}
            //else
            //{
            //    showMessage("", "");
            //    //Default Table Detail
            //    dvReportDetail.Style.Clear();
            //    dvReportDetail.Controls.Add(new LiteralControl("Please filter your search...."));
            //    dvReportDetail.Style.Add("color", "#3399ff");
            //    dvReportDetail.Style.Add("Font-Weight", "bold");
            //}

            showMessage("", "");
            //Default Table Detail
            dvReportDetail.Style.Clear();
            dvReportDetail.Controls.Add(new LiteralControl("Please filter your search...."));
            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");

        }
    }

    //Generate Product Premium Report
    protected void ReportProductPremium()
    {
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";

        DateTimeFormatInfo dtfi3 = new DateTimeFormatInfo();
        dtfi3.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
        dtfi3.DateSeparator = "/";

        DateTime from_date = DateTime.Parse(txtFrom_date.Text, dtfi);
        DateTime to_date = DateTime.Parse(txtTo_date.Text + " 23:59:59", dtfi3);


        //Get list of product premium details
        #region
        //Premium List from Individual Prem Pay
        List<bl_product_premium_report> product_premium_list = new List<bl_product_premium_report>();

        product_premium_list = da_product_premium_report.GetPMAProductPremiumDetailsByDates(from_date, to_date);

        //Premium List from Group Prem Pay
        List<bl_product_premium_report> group_prem_pay_list = new List<bl_product_premium_report>();

        group_prem_pay_list = da_product_premium_report.GetPMAGTLIPremiumDetailsFromPremPayByDates(from_date, to_date);

        //Premium List from Group Prem Return
        List<bl_product_premium_report> group_prem_return_list = new List<bl_product_premium_report>();

        group_prem_return_list = da_product_premium_report.GetPMAGTLIPremiumDetailsFromPremReturnByDates(from_date, to_date);

        #endregion

        //Declare total premium by group of products variables
        #region

        double total_amount_paid = 0;
        double total_sum_insure = 0;
        double total_term_life = 0; // FT013, T10, T10002, T1011, T3, T3002, T5, T5002, 
        double total_endowment = 0; // PP15/10, PP200
        double total_whole_life = 0;// W10, W15, W20, W9010, W9015, W9020
        double total_mrta = 0; // MRTA, MRTA12, MRTA24, MRTA36
        double total_gtli = 0; //GTLI (Group Term Life)
        double total_family_protection = 0; //FP,FPP10/10, 'FPP15/15','FP','FPP10/10','FPP15/15','FPP5/5','NFP10/10','NFP15/15','NFP5/5'
        double total_study_save = 0; //'SDS10/10','SDS12/12','SDS15/15','SDSPK10/10/5300','SDSPK12/12/6300','SDSPK15/15/6600','SDSPKM10/10/10630','SDSPKM12/12/12560',
        double total_so_ci = 0; //SO, CI
        double total_AL = 0; //AL WING
        double total_AP = 0; //AP (Annual Premium)
        double total_CL = 0;
        double total_CMK = 0;

        double total_term_life_policy = 0;
        double total_endowment_policy = 0;
        double total_whole_life_policy = 0;
        double total_mrta_policy = 0;
        double total_gtli_policy = 0;
        double total_family_protection_policy = 0;
        double total_study_save_policy = 0;
        double total_so_ci_policy = 0;
        double total_AL_policy = 0;
        double total_CL_policy = 0;
        double total_CMK_policy = 0;

        double total_term_life_fyp = 0;
        double total_endowment_fyp = 0;
        double total_whole_life_fyp = 0;
        double total_mrta_fyp = 0;
        double total_gtli_fyp = 0;
        double total_family_protection_fyp = 0;
        double total_study_save_fyp = 0;
        double total_so_ci_fyp = 0;
        double total_AL_fyp = 0;
        double total_CL_fyp = 0;
        double total_CMK_fyp = 0;

        double total_term_life_ryp = 0;
        double total_endowment_ryp = 0;
        double total_whole_life_ryp = 0;
        double total_mrta_ryp = 0;
        double total_gtli_ryp = 0;
        double total_family_protection_ryp = 0;
        double total_study_save_ryp = 0;
        double total_so_ci_ryp = 0;
        double total_AL_ryp = 0;
        double total_CL_ryp = 0;
        double total_CMK_ryp = 0;

        double total_term_life_sum_insure = 0;
        double total_endowment_sum_insure = 0;
        double total_whole_life_sum_insure = 0;
        double total_mrta_sum_insure = 0;
        double total_gtli_sum_insure = 0;
        double total_family_protection_sum_insure = 0;
        double total_study_save_sum_insure = 0;
        double total_so_ci_sum_insure = 0;
        double total_AL_sum_insure = 0;
        double total_CL_sum_insure = 0;
        double total_CMK_sum_insure = 0;

        #endregion

        //Draw Header
        #region

        if (product_premium_list.Count > 0)
        {
            lblfrom.Text = "<div style='text-align:center; class='PrintPD''><h1 style=\"color: blue; text-align:center;\">Premium Report</h1> <input type=\"Label\" value=\"From: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + " " + txtFrom_date.Text + "<input type=\"Label\" value=\"To: \" style=\"color:black; border:none; text-align:right; width:35px; font-size:12px;\"/>" + txtTo_date.Text + "</div>";

            lblfrom1.Text = txtFrom_date.Text;
            lblto1.Text = txtTo_date.Text;
        }
        else
        {
            lblfrom.Text = "";
            lblfrom1.Text = "";
            lblto1.Text = "";
        }

        string strTable = "";

        //Draw Header
        if (product_premium_list.Count > 0)
        {

            strTable = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
            strTable += "<tr border=\"1\"><th styple=\"text-align: center; width:90px; \">No</th><th styple=\"text-align: center; \">Policy Number</th><th styple=\"text-align: center; \">Effective Date</th><th style=\"text-align: center;\">Pay Date</th><th styple=\"text-align: center; \">Product</th><th style=\"text-align: center;\">Mode</th><th styple=\"text-align: center; \">Prem Year</th><th styple=\"text-align: center; \">Prem Lot</th><th styple=\"text-align: center; \">SI ($)</th><th style=\"text-align: center;\">AP ($)</th><th style=\"text-align: center;\">Amount Paid ($)</th></tr>";

            dvReportDetail.Controls.Add(new LiteralControl(strTable));

            strTable = "";
        }
        else
        {
            strTable = "";
        }


        #endregion

        int row_no = 0;
        /*
         total sum assure = sum insure FYP + sum insure renew
         * total number of policies = number of policies in first year + renewal
         * FYP is the fist year premium ( prem_year =1 && prem_lot =1)
         * RYP is the renewal premium ( prem_year =2 && prem_lot =1)
         */

        //Loop through product premium list (Individual)
        #region
        for (int i = 0; i < product_premium_list.Count; i++)
        {
            //Get obj product premium of this index[i]
            bl_product_premium_report product_premium = new bl_product_premium_report();
            product_premium = product_premium_list[i];

            DataTable my_sub = new DataTable(); // RESERVED FOR FIRST FINANCE

            //Get total from individual policies
            #region

            total_amount_paid += product_premium.Amount_Paid;
            total_sum_insure += product_premium.Sum_Insure;
            total_AP += product_premium.AP;

            switch (product_premium.Product_ID)
            {
                #region GROUP 
                case "FT013": //Flexi Term
                case "T10": //Term 10 Old
                case "T10002": //Term 10 New
                case "T1011": //Micro
                case "T3": //Term 3 Old
                case "T3002": //Term 3 New
                case "T5": //Term 5 Old
                case "T5002": //Term 5 New
                    total_term_life += product_premium.Amount_Paid;

                    //Total Term Life policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_term_life_policy += 1;
                        total_term_life_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                       total_term_life_policy += 1;
                        total_term_life_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total Term Life FYP
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_term_life_fyp += product_premium.Amount_Paid;
                    }
                    else //Total Term Life RYP
                    {
                        total_term_life_ryp += product_premium.Amount_Paid;
                    }

                    break;
                case "PP15/10":
                case "PP200":
                    total_endowment += product_premium.Amount_Paid;

                    //Total Endowment policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_endowment_policy += 1;
                        total_endowment_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                        total_endowment_policy += 1;
                        total_endowment_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total Endowment FYP
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_endowment_fyp += product_premium.Amount_Paid;
                    }
                    else //Total Endowment RYP
                    {
                        total_endowment_ryp += product_premium.Amount_Paid;
                    }
                    break;
                case "W10": //Whole Life 10 Old
                case "W15": //Whole Life 15 Old 
                case "W20": //Whole Life 20 Old
                case "W9010": //Whole Life 10 New
                case "W9015": //Whole Life 15 New
                case "W9020": //Whole Life 20 New
                    total_whole_life += product_premium.Amount_Paid;

                    //Total Whole Life policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_whole_life_policy += 1;
                        total_whole_life_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                       total_whole_life_policy += 1;
                        total_whole_life_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total Whole Life FYP
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_whole_life_fyp += product_premium.Amount_Paid;
                    }
                    else //Total Whole Life RYP
                    {
                        total_whole_life_ryp += product_premium.Amount_Paid;
                    }

                    break;
                case "MRTA": // MRTA Old
                case "MRTA12": //MRTA12
                case "MRTA24": //MRTA24
                case "MRTA36": //MRTA36
                    total_mrta += product_premium.Amount_Paid;

                    //Total MRTA policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_mrta_policy += 1;
                        total_mrta_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                        total_mrta_policy += 1;
                        total_mrta_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total MRTA FYP
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_mrta_fyp += product_premium.Amount_Paid;
                    }
                    else //Total MRTA RYP
                    {
                        total_mrta_ryp += product_premium.Amount_Paid;
                    }
                    break;

                case "FP":
                case "FPP10/10": // Family Protection 
                case "FPP15/15":
                case "FPP5/5":
                case "NFP10/10":
                case "NFP15/15":
                case "NFP5/5":
                    total_family_protection += product_premium.Amount_Paid;

                    //Total FP Policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_family_protection_policy += 1;
                        total_family_protection_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                       total_family_protection_policy += 1;
                        total_family_protection_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total FP FYP
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_family_protection_fyp += product_premium.Amount_Paid;
                    }
                    else //Total FP RYP
                    {
                        total_family_protection_ryp += product_premium.Amount_Paid;
                    }
                    break;

                case "SDS10/10": //Study Save
                case "SDS12/12":
                case "SDS15/15":
                case "SDSPK10/10/5300":
                case "SDSPK12/12/6300":
                case "SDSPK15/15/6600":
                case "SDSPKM10/10/10630":
                case "SDSPKM12/12/12560":
                case "SDSPKM15/15/13300":
                case "SDSPKP10/10/10000":
                case "SDSPKP10/10/5000":
                case "SDSPKP12/12/10000":
                case "SDSPKP12/12/5000":
                case "SDSPKP15/15/10000":
                case "SDSPKP15/15/5000":
                    total_study_save += product_premium.Amount_Paid;

                    //Total SD Policy 
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_study_save_policy += 1;
                        total_study_save_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                       total_study_save_policy += 1;
                        total_study_save_sum_insure += product_premium.Sum_Insure;
                    }
                    //Total SD FYP
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_study_save_fyp += product_premium.Amount_Paid;
                    }
                    else //Total SD RYP
                    {
                        total_study_save_ryp += product_premium.Amount_Paid;
                    }
                    break;
                #endregion

                #region SIMPLE ONE
                case "SO": //Sample One Insurance
                case "CI":
                    total_so_ci += product_premium.Amount_Paid;

                    //Total SO Policy sum insure & policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_so_ci_policy += 1;
                        total_so_ci_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                        total_so_ci_policy += 1;
                        total_so_ci_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total SO FYP 
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_so_ci_fyp += product_premium.Amount_Paid;
                    }
                    else //Total SO RYP 
                    {
                        total_so_ci_ryp += product_premium.Amount_Paid;
                    }
                    break;
                case "SO2022005":
                     total_so_ci += product_premium.Amount_Paid;

                    //Total SO Policy sum insure & policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_so_ci_policy += product_premium.NumberPolicy;
                        total_so_ci_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                        total_so_ci_policy += 1;
                        total_so_ci_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total SO FYP 
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_so_ci_fyp += product_premium.Amount_Paid;
                    }
                    else //Total SO RYP 
                    {
                        total_so_ci_ryp += product_premium.Amount_Paid;
                    }
                    break;

                #endregion

                #region WING
                case "AL": // 'AL' WING
                    total_AL += product_premium.Amount_Paid;

                    //AL Policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_AL_policy += 1;
                        total_AL_sum_insure += product_premium.Sum_Insure;
                    }
                    //Re-new policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                        total_AL_policy += 1;
                        total_AL_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total AL FYP
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_AL_fyp += product_premium.Amount_Paid;
                    }
                    else //Total AL RYP
                    {
                        total_AL_ryp += product_premium.Amount_Paid;
                    }
                    break;
                #endregion

                #region FIRST FINANCE
                case "CL24": // Credit Life 24 //New Update By MEASSUN On 12-05-2020 
                    total_CL += product_premium.Amount_Paid;

                    my_sub = da_policy_cl24.GetSubPolicyByMainPolicyID(product_premium.Policy_Number);
                    if (my_sub.Rows.Count > 0)
                    {
                        foreach (DataRow row in my_sub.Rows)
                        {
                            //CL Policy
                            if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                            {
                                total_CL_policy += 1;
                                total_CL_sum_insure += row["Sum_Insure"].ToString() != "" ? float.Parse(row["Sum_Insure"].ToString()) : 0;
                            }

                            //Re-new policy
                            if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                            {
                                total_CL_policy += 1;
                                total_CL_sum_insure += row["Sum_Insure"].ToString() != "" ? float.Parse(row["Sum_Insure"].ToString()) : 0;
                            }

                            //Total CL FYP
                            if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                            {
                                total_CL_fyp += row["Total_Premium"].ToString() != "" ? float.Parse(row["Total_Premium"].ToString()) : 0;
                            }
                            else //Total CL RYP
                            {
                                total_CL_policy += 1;
                                total_CL_sum_insure += row["Sum_Insure"].ToString() != "" ? float.Parse(row["Sum_Insure"].ToString()) : 0;
                                total_CL_ryp += row["Total_Premium"].ToString() != "" ? float.Parse(row["Total_Premium"].ToString()) : 0;
                            }
                        }
                        
                    }
                    
                    break;
                #endregion

                #region CMK
                case "CLC":
                    total_CMK += product_premium.Amount_Paid;

                    //CMK Policy
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_CMK_policy += 1;
                        total_CMK_sum_insure += product_premium.Sum_Insure;
                    }

                    //Re-new Policy
                    if (product_premium.Prem_Year == 2 && product_premium.Prem_Lot == 1)
                    {
                        total_CMK_policy += 1;
                        total_CMK_sum_insure += product_premium.Sum_Insure;
                    }

                    //Total CMK FYP 
                    if (product_premium.Prem_Year == 1 && product_premium.Prem_Lot == 1)
                    {
                        total_CMK_fyp += product_premium.Amount_Paid;
                    }
                    else // Total CMK RYP
                    {
                        total_CMK_policy += 1;
                        total_CMK_sum_insure += product_premium.Sum_Insure;
                        total_CMK_ryp += product_premium.Amount_Paid;
                    }
                    break;
                #endregion
            }

            #endregion
            //Draw Row Indiviudal
            #region WRITING

            if (product_premium_list.Count > 0)
            {
                if (product_premium.Product_ID != "CL24")
                {
                    row_no += 1;
                    #region OTHER PRODUCT

                    strTable += "<tr>";
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (row_no) + "</td>";

                    //Policy Number Column
                    strTable += "<td  style=\"text-align: center; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Policy_Number + "</td>";

                    //Effective Date Column
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";

                    //Pay Date Column
                    strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Pay_Date.ToString("d-MMM-yyyy") + "</td>";


                    //Product Column
                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Product + "</td>";


                    //Pay Mode Column
                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Pay_Mode + "</td>";

                    //Prem Year Column
                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Prem_Year + "</td>";

                    //Prem Lot Column 
                    strTable += "<td style=\"text-align: center; padding:5px 5px 5px; width=\"0px;\" >" + product_premium.Prem_Lot + "</td>";

                    //Sum Insure Column
                    if (product_premium.Sum_Insure == 0)
                    {
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Sum_Insure + "</td>";
                    }

                    //Annual Premium Column
                    if (product_premium.AP == 0)
                    {
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + product_premium.AP + "</td>";
                    }

                    //Amount Paid Column
                    if (product_premium.Amount_Paid == 0)
                    {
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                    }
                    else
                    {
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + product_premium.Amount_Paid + "</td>";
                    }


                    strTable += "</tr>";

                    dvReportDetail.Controls.Add(new LiteralControl(strTable));

                    strTable = "";

#endregion
                }
                else // FIRST FINANCE
                {
                    #region FIRST FINANCE

                    foreach (DataRow row in my_sub.Rows)
                    {
                        row_no++;

                        strTable += "<tr>";
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (row_no) + "</td>";

                        //Policy Number Column
                        strTable += "<td  style=\"text-align: center; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + row["Policy_Number"].ToString() + "</td>";

                        //Effective Date Column
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + Convert.ToDateTime(row["Effective_Date"]).ToString("dd-MMM-yyyy") + "</td>";

                        //Pay Date Column
                        strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Pay_Date.ToString("d-MMM-yyyy") + "</td>";


                        //Product Column
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Product + "</td>";


                        //Pay Mode Column
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Pay_Mode + "</td>";

                        //Prem Year Column
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Prem_Year + "</td>";

                        //Prem Lot Column 
                        strTable += "<td style=\"text-align: center; padding:5px 5px 5px; width=\"0px;\" >" + product_premium.Prem_Lot + "</td>";

                        //Sum Insure Column
                        if (product_premium.Sum_Insure == 0)
                        {
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                        }
                        else
                        {
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + row["Sum_Insure"].ToString() + "</td>";
                        }

                        //Annual Premium Column
                        if (product_premium.AP == 0)
                        {
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                        }
                        else
                        {
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + row["Total_Premium"].ToString() + "</td>";
                        }

                        //Amount Paid Column
                        if (product_premium.Amount_Paid == 0)
                        {
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                        }
                        else
                        {
                            strTable += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + row["Total_Premium"].ToString() + "</td>";
                        }

                        strTable += "</tr>";

                        dvReportDetail.Controls.Add(new LiteralControl(strTable));

                        strTable = "";


                    }

                    #endregion
                }
                
            }

            #endregion

        }//End loop product premium list
        #endregion

        //Loop through group prem pay list (GTLI)
        #region
        for (int j = 0; j < group_prem_pay_list.Count; j++)
        {
            row_no += 1;

            //Get obj product prem pay of this index[j]
            bl_product_premium_report group_prem_pay = new bl_product_premium_report();
            group_prem_pay = group_prem_pay_list[j];

            //Get total from group prem pay policies
            #region

            total_amount_paid += group_prem_pay.Amount_Paid;
            total_sum_insure += group_prem_pay.Sum_Insure;
            total_gtli_sum_insure += group_prem_pay.Sum_Insure;
            total_AP += group_prem_pay.AP;

            if (group_prem_pay.Transaction_Type == 1)
            {
                total_gtli_policy += 1;
            }

            total_gtli += group_prem_pay.Amount_Paid;

            total_gtli_fyp += group_prem_pay.Amount_Paid;

            #endregion

            //Draw Row GTLI Prem Pay
            #region

            if (group_prem_pay_list.Count > 0)
            {
                string strTableGroupPremPay = "<tr>";
                strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (row_no) + "</td>";

                //Policy Number Column
                strTableGroupPremPay += "<td  style=\"text-align: center; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Policy_Number + "</td>";

                //Effective Date Column
                strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Effective_Date.ToString("d-MMM-yyyy") + "</td>";

                //Pay Date Column
                strTableGroupPremPay += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Pay_Date.ToString("d-MMM-yyyy") + "</td>";

                //Product Column
                strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Product + "</td>";

                //Pay Mode Column
                strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Pay_Mode + "</td>";

                //Prem Year Column
                strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Prem_Year + "</td>";

                //Prem Lot Column 
                strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + group_prem_pay.Prem_Lot + "</td>";
                //Sum Insure Column
                if (group_prem_pay.Sum_Insure == 0)
                {
                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                }
                else
                {
                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_pay.Sum_Insure + "</td>";
                }

                //Annual Column
                if (group_prem_pay.AP == 0)
                {
                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + group_prem_pay.AP + "</td>";
                }

                //Amount Paid Column
                if (group_prem_pay.Amount_Paid == 0)
                {
                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTableGroupPremPay += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + group_prem_pay.Amount_Paid + "</td>";
                }


                strTableGroupPremPay += "</tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTableGroupPremPay));

                strTableGroupPremPay = "";
            }

            #endregion

        }//End loop group prem pay list
        #endregion

        //Loop through group prem return list (GTLI)
        #region
        for (int k = 0; k < group_prem_return_list.Count; k++)
        {
            row_no += 1;

            //Get obj product prem return of this index[k]
            bl_product_premium_report group_prem_return = new bl_product_premium_report();
            group_prem_return = group_prem_return_list[k];

            //Get total from group prem return policies
            #region

            total_amount_paid -= group_prem_return.Amount_Paid;
            total_sum_insure -= group_prem_return.Sum_Insure;
            total_gtli_sum_insure -= group_prem_return.Sum_Insure;
            total_AP -= group_prem_return.AP;
            total_gtli -= group_prem_return.Amount_Paid;

            total_gtli_fyp -= group_prem_return.Amount_Paid;

            #endregion

            //Draw Row GTLI Prem Return
            #region

            if (group_prem_return_list.Count > 0)
            {
                string strTableGroupPremReturn = "<tr>";
                strTableGroupPremReturn += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (row_no) + "</td>";

                //Policy Number Column
                strTableGroupPremReturn += "<td  style=\"text-align: center; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Policy_Number + "</td>";

                //Effective Date Column
                strTableGroupPremReturn += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Effective_Date.ToString("d-MMM-yyyy") + "</td>";

                //Pay Date Column
                strTableGroupPremReturn += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Pay_Date.ToString("d-MMM-yyyy") + "</td>";

                //Product Column
                strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Product + "</td>";

                //Pay Mode Column
                strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Pay_Mode + "</td>";

                //Prem Year Column
                strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + group_prem_return.Prem_Year + "</td>";

                //Prem Lot Column 
                strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >" + group_prem_return.Prem_Lot + "</td>";

                //Sum Insure Column
                if (group_prem_return.Sum_Insure == 0)
                {
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
                }
                else
                {
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + group_prem_return.Sum_Insure + ")</td>";
                }

                //Annual Premium Column
                if (group_prem_return.AP == 0)
                {
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >(" + group_prem_return.AP + ")</td>";
                }

                //Amount Paid Column
                if (group_prem_return.Amount_Paid == 0)
                {
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
                }
                else
                {
                    strTableGroupPremReturn += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\" >(" + group_prem_return.Amount_Paid + ")</td>";
                }


                strTableGroupPremReturn += "</tr>";

                dvReportDetail.Controls.Add(new LiteralControl(strTableGroupPremReturn));

                strTableGroupPremReturn = "";
            }

            #endregion

        }//End loop group prem return list
        #endregion

        //Draw Total
        #region

        if (product_premium_list.Count > 0)
        {
            strTable += "<tr>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\" >&nbsp;</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";

            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
            strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >&nbsp;</td>";
            strTable += "<td  style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >Total:</td>";


            if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_sum_insure) == 0)
            {

                strTable += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";

            }
            else
            {
                //Negative
                if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_sum_insure) < 0)
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_sum_insure) + ")</td>";
                }
                else //Positive
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_sum_insure) + "</td>";
                }
            }

            if (total_AP == 0)
            {
                strTable += "<td style=\"text-align: center;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if (total_AP < 0)
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_AP + ")</td>";
                }
                else //Positive
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_AP + "</td>";
                }
            }

            if (total_amount_paid == 0)
            {
                strTable += "<td style=\"text-align: center;  font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if (total_amount_paid < 0)
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_amount_paid + ")</td>";
                }
                else //Positive
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_amount_paid + "</td>";
                }
            }


            strTable += "</tr>";

            dvReportDetail.Controls.Add(new LiteralControl(strTable));

            strTable = "";
        }

        #endregion

        //Draw Summary Table Product Premium (Term Life, Endowment, Whole Life, MRTA, Group Term Life)
        #region
        string strTableSummray = "";

        //Draw Header Summary
        #region
        if (product_premium_list.Count > 0)
        {

            strTableSummray = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"1\">";
            strTableSummray += "<tr border=\"1\"><th styple=\"text-align: center; width:90px; \">No</th><th styple=\"text-align: center; \">Product</th><th styple=\"text-align: center; \">No of Policy</th><th styple=\"text-align: center; \">SI ($)</th><th styple=\"text-align: center; \">FYP ($)</th><th styple=\"text-align: center; \">RYP ($)</th><th styple=\"text-align: center; \">Total Amount ($)</th></tr>";

            dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));

            strTableSummray = "";
        }
        else
        {
            strTableSummray = "";
        }
        #endregion

        //Draw Row Summary
        #region
        for (int j = 1; j <= 5; j++)
        {
            if (product_premium_list.Count > 0)
            {
                strTableSummray += "<tr>";
                strTableSummray += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (j) + "</td>";

                switch (j)
                {  
                    case 1: //Term Life
                        strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Term Life </td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life_policy + total_family_protection_policy + total_so_ci_policy + total_AL_policy + total_CL_policy + total_CMK_policy) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life_sum_insure + total_family_protection_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_sum_insure) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (float)Math.Round((total_term_life_fyp + total_family_protection_fyp + total_so_ci_fyp + total_AL_fyp + total_CL_fyp + total_CMK_fyp), 2, MidpointRounding.AwayFromZero) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (float)Math.Round((total_term_life_ryp + total_family_protection_ryp + total_so_ci_ryp + total_AL_ryp + total_CL_ryp + total_CMK_ryp), 2, MidpointRounding.AwayFromZero) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life + total_family_protection + total_so_ci + total_AL + total_CL + total_CMK) + "</td>";
                        break;
                    case 2: //Endowment
                        strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Endowment </td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_endowment_policy + total_study_save_policy) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_endowment_sum_insure + total_study_save_sum_insure) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_endowment_fyp + total_study_save_fyp) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_endowment_ryp + total_study_save_ryp) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_endowment + total_study_save) + "</td>";
                        break;
                    case 3: //Whole Life
                        strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Whole Life </td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life_policy + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life_sum_insure + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life_fyp + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life_ryp + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_whole_life + "</td>";
                        break;
                    case 4: //MRTA
                        strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > MRTA </td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_policy + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_sum_insure + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_fyp + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta_ryp + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_mrta + "</td>";
                        break;
                    case 5: //Group Term Life (GTLI)
                        strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Group </td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_policy + "</td>";

                        //Negative
                        if (total_gtli_sum_insure < 0)
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli_sum_insure + ")</td>";
                        }
                        else //Positive
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_sum_insure + "</td>";
                        }

                        //Negative 
                        if (total_gtli_fyp < 0)
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli_fyp + ")</td>";
                        }
                        else //Positive
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_fyp + "</td>";
                        }

                        //Negative
                        if (total_gtli_ryp < 0)
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli_ryp + ")</td>";
                        }
                        else //Positive
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli_ryp + "</td>";
                        }

                        //Negative
                        if (total_gtli < 0)
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + total_gtli + ")</td>";
                        }
                        else //Positive
                        {
                            strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_gtli + "</td>";
                        }
                        break;

                    //case 6:
                    //    strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Family Protection </td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_family_protection_policy + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_family_protection_sum_insure + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_family_protection_fyp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_family_protection_ryp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_family_protection + "</td>";
                    //    break;
                    //case 7:
                    //    strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Study Save </td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_study_save_policy + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_study_save_sum_insure + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_study_save_fyp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_study_save_ryp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_study_save + "</td>";
                    //    break;
                    //case 8:
                    //    strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > SO & CI </td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_so_ci_policy + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_so_ci_sum_insure + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_so_ci_fyp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_so_ci_ryp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_so_ci + "</td>";
                    //    break;
                    //case 9:
                    //    strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > AL </td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_AL_policy + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_AL_sum_insure + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_AL_fyp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_AL_ryp + "</td>";
                    //    strTableSummray += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\"  >" + total_AL + "</td>";
                    //    break;
                }

                strTableSummray += "</tr>";

                dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));

                strTableSummray = "";

            }
        }

        #endregion

        //Draw Total
        #region

        if (product_premium_list.Count > 0)
        {
            strTableSummray += "<tr>";

            strTableSummray += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\" >&nbsp;</td>";

            strTableSummray += "<td  style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >Total:</td>";

            if ((total_mrta_policy + total_term_life_policy + total_whole_life_policy + total_endowment_policy + total_gtli_policy + total_family_protection_policy + total_study_save_policy + total_so_ci_policy + total_AL_policy + total_CL_policy + total_CMK_policy) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_policy + total_term_life_policy + total_whole_life_policy + total_endowment_policy + total_family_protection_policy + total_study_save_policy + total_so_ci_policy + total_AL_policy + total_CL_policy + total_CMK_policy) + "</td>";
            }

            if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_policy) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_policy) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_policy) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (float)Math.Round((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_gtli_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_so_ci_sum_insure + total_AL_sum_insure + total_CL_sum_insure + total_CMK_policy), 2, MidpointRounding.AwayFromZero) + "</td>";
                }
            }

            if ((total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp + total_family_protection_fyp + total_study_save_fyp + total_so_ci_fyp + total_AL_fyp + total_CL_fyp + total_CMK_fyp) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if ((total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp + total_family_protection_fyp + total_study_save_fyp + total_so_ci_fyp + total_AL_fyp + total_CL_fyp + total_CMK_fyp) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp + total_family_protection_fyp + total_study_save_fyp + total_so_ci_fyp + total_AL_fyp + total_CL_fyp + total_CMK_fyp) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (float)Math.Round((total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_gtli_fyp + total_family_protection_fyp + total_study_save_fyp + total_so_ci_fyp + total_AL_fyp + total_CL_fyp + total_CMK_fyp), 2, MidpointRounding.AwayFromZero) + "</td>";
                }

            }

            if ((total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp + total_family_protection_ryp + total_study_save_ryp + total_so_ci_ryp + total_AL_ryp + total_CL_ryp + total_CMK_ryp) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative 
                if ((total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp + total_family_protection_ryp + total_study_save_ryp + total_so_ci_ryp + total_AL_ryp + total_CL_ryp + total_CMK_ryp) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp + total_family_protection_ryp + total_study_save_ryp + total_so_ci_ryp + total_AL_ryp + total_CL_ryp + total_CMK_ryp) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (float)Math.Round((total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_gtli_ryp + total_family_protection_ryp + total_study_save_ryp + total_so_ci_ryp + total_AL_ryp + total_CL_ryp + total_CMK_ryp), 2, MidpointRounding.AwayFromZero) + "</td>";
                }

            }

            if ((total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli + total_family_protection + total_study_save + total_so_ci + total_AL + total_CL + total_CMK) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if ((total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli + total_family_protection + total_study_save + total_so_ci + total_AL + total_CL + total_CMK) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli + total_family_protection + total_study_save + total_so_ci + total_AL + total_CL + total_CMK) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta + total_term_life + total_whole_life + total_endowment + total_gtli + total_family_protection + total_study_save + total_so_ci + total_AL + total_CL + total_CMK) + "</td>";
                }

            }

            strTableSummray += "</tr>";

            dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));

            strTableSummray = "";
        }

        #endregion

        #endregion

        dvReportDetail.Style.Clear();
        dvReportSummary.Style.Clear();

        if (product_premium_list.Count > 0)
        {
            //Table Summary End
            strTableSummray += "</table>";
            dvReportSummary.Controls.Add(new LiteralControl(strTableSummray));
            strTable = "";

            //Table Detail End
            strTable += "</table>";
            dvReportDetail.Controls.Add(new LiteralControl(strTable));
            strTable = "";
        }
        else
        {

            //Default Text in Table Detail
            dvReportDetail.Controls.Add(new LiteralControl("No data found. Please filter your search...."));

            strTable = "";

            dvReportDetail.Style.Add("color", "#3399ff");
            dvReportDetail.Style.Add("Font-Weight", "bold");
        }

    }

    //Button Search Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        ReportProductPremium();
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
}