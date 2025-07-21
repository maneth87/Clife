using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Reports_Premium_Report : System.Web.UI.Page
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

        //Premium List from Individual Prem Pay
        List<bl_product_premium_report> product_premium_list = new List<bl_product_premium_report>();

        product_premium_list = da_product_premium_report.GetRenewalPremiumReport(from_date, to_date);

        #region

        double total_amount_paid = 0;
        double total_sum_insure = 0;
        double total_term_life = 0; // FT013, T10, T10002, T1011, T3, T3002, T5, T5002, 
        double total_endowment = 0; // PP15/10, PP200
        double total_whole_life = 0;// W10, W15, W20, W9010, W9015, W9020
        double total_mrta = 0; // MRTA, MRTA12, MRTA24, MRTA36
        double total_family_protection = 0; //FP,FPP10/10, 'FPP15/15','FP','FPP10/10','FPP15/15','FPP5/5','NFP10/10','NFP15/15','NFP5/5'
        double total_study_save = 0; //'SDS10/10','SDS12/12','SDS15/15','SDSPK10/10/5300','SDSPK12/12/6300','SDSPK15/15/6600','SDSPKM10/10/10630','SDSPKM12/12/12560',
        double total_AP = 0; //AP (Annual Premium)
        double total_CL = 0;

        double total_term_life_policy = 0;
        double total_endowment_policy = 0;
        double total_whole_life_policy = 0;
        double total_mrta_policy = 0;
        double total_family_protection_policy = 0;
        double total_study_save_policy = 0;
        double total_CL_policy = 0;

        double total_term_life_fyp = 0;
        double total_endowment_fyp = 0;
        double total_whole_life_fyp = 0;
        double total_mrta_fyp = 0;
        double total_family_protection_fyp = 0;
        double total_study_save_fyp = 0;
        double total_CL_fyp = 0;

        double total_term_life_ryp = 0;
        double total_endowment_ryp = 0;
        double total_whole_life_ryp = 0;
        double total_mrta_ryp = 0;
        double total_family_protection_ryp = 0;
        double total_study_save_ryp = 0;
        double total_CL_ryp = 0;

        double total_term_life_sum_insure = 0;
        double total_endowment_sum_insure = 0;
        double total_whole_life_sum_insure = 0;
        double total_mrta_sum_insure = 0;
        double total_family_protection_sum_insure = 0;
        double total_study_save_sum_insure = 0;
        double total_CL_sum_insure = 0;

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
            strTable += "<tr border=\"1\"><th styple=\"text-align: center; width:90px; \">No</th><th styple=\"text-align: center; \">Policy Number</th><th styple=\"text-align: center; \">Effective Date</th><th style=\"text-align: center;\">Due Date</th><th styple=\"text-align: center; \">Product</th><th style=\"text-align: center;\">Mode</th><th styple=\"text-align: center; \">Prem Year</th><th styple=\"text-align: center; \">Prem Lot</th><th styple=\"text-align: center; \">SI ($)</th><th style=\"text-align: center;\">AP ($)</th><th style=\"text-align: center;\">Amount Paid ($)</th></tr>";

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

            //Get total from individual policies
            #region

            total_amount_paid += product_premium.Amount_Paid;
            total_sum_insure += product_premium.Sum_Insure;
            total_AP += product_premium.AP;

            switch (product_premium.Product_ID)
            {
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
            }

            #endregion
            //Draw Row Indiviudal
            #region

            if (product_premium_list.Count > 0)
            {
                strTable += "<tr>";
                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (i + 1) + "</td>";

                //Policy Number Column
                strTable += "<td  style=\"text-align: center; mso-number-format:\\@; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Policy_Number + "</td>";

                //Effective Date Column
                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Effective_Date.ToString("d-MMM-yyyy") + "</td>";

                //Pay Date Column
                strTable += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + product_premium.Due_Date.ToString("d-MMM-yyyy") + "</td>";


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
                    strTable += "<td style=\"text-align: right; padding:5px 5px 5px 5px; width=\"0px;\" > - </td>";
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
            }

            #endregion

            row_no = i + 1;
        }//End loop product premium list
        #endregion
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


            if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) == 0)
            {

                strTable += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";

            }
            else
            {
                //Negative
                if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) < 0)
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) + ")</td>";
                }
                else //Positive
                {
                    strTable += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) + "</td>";
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

        //Draw Summary Table Product Premium (Term Life, Endowment, Whole Life, MRTA)
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
        for (int j = 1; j <= 4; j++)
        {
            if (product_premium_list.Count > 0)
            {
                strTableSummray += "<tr>";
                strTableSummray += "<td  style=\"text-align: center; padding:5px 5px 5px 5px; width=\"90px;\"  >" + (j) + "</td>";

                switch (j)
                {
                    case 1: //Term Life
                        strTableSummray += "<td  style=\"text-align: left; padding:5px 5px 5px 5px; width=\"0px;\"  > Term Life </td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life_policy + total_family_protection_policy + total_CL_policy) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life_sum_insure + total_family_protection_sum_insure + total_CL_sum_insure) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life_fyp + total_family_protection_fyp + total_CL_fyp) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life_ryp + total_family_protection_ryp + total_CL_ryp) + "</td>";
                        strTableSummray += "<td style=\"text-align: center; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_term_life + total_family_protection + total_CL) + "</td>";
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

            if ((total_mrta_policy + total_term_life_policy + total_whole_life_policy + total_endowment_policy + total_family_protection_policy + total_study_save_policy + total_CL_policy) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_policy + total_term_life_policy + total_whole_life_policy + total_endowment_policy + total_family_protection_policy + total_study_save_policy + total_CL_policy) + "</td>";
            }

            if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if ((total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_sum_insure + total_term_life_sum_insure + total_whole_life_sum_insure + total_endowment_sum_insure + total_family_protection_sum_insure + total_study_save_sum_insure + total_CL_sum_insure) + "</td>";
                }
            }

            if ((total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_family_protection_fyp + total_study_save_fyp + total_CL_fyp) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if ((total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_family_protection_fyp + total_study_save_fyp + total_CL_fyp) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_family_protection_fyp + total_study_save_fyp + total_CL_fyp) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_fyp + total_term_life_fyp + total_whole_life_fyp + total_endowment_fyp + total_family_protection_fyp + total_study_save_fyp + total_CL_fyp) + "</td>";
                }

            }

            if ((total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_family_protection_ryp + total_study_save_ryp + total_CL_ryp) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative 
                if ((total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_family_protection_ryp + total_study_save_ryp + total_CL_ryp) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_family_protection_ryp + total_study_save_ryp + total_CL_ryp) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta_ryp + total_term_life_ryp + total_whole_life_ryp + total_endowment_ryp + total_family_protection_ryp + total_study_save_ryp + total_CL_ryp) + "</td>";
                }

            }

            if ((total_mrta + total_term_life + total_whole_life + total_endowment + total_family_protection + total_study_save + total_CL) == 0)
            {
                strTableSummray += "<td style=\"text-align: center; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  > - </td>";
            }
            else
            {
                //Negative
                if ((total_mrta + total_term_life + total_whole_life + total_endowment + total_family_protection + total_study_save + total_CL) < 0)
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >(" + (total_mrta + total_term_life + total_whole_life + total_endowment + total_family_protection + total_study_save + total_CL) + ")</td>";
                }
                else //Positive
                {
                    strTableSummray += "<td style=\"text-align: center; text-decoration:underline; font-weight: bold; padding:5px 5px 5px 5px; width=\"0px;\"  >" + (total_mrta + total_term_life + total_whole_life + total_endowment + total_family_protection + total_study_save + total_CL) + "</td>";
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