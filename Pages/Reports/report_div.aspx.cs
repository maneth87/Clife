using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Reports_report_div : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblPolicyNo.InnerText = " 00000251";
        lblCustomerID.InnerText = " 00000245";
        lblInusredName.InnerText = " គឹម កល្យាណ";
        lblPassport.InnerText = " 160163822";
        lblDOB.InnerText = " 14/01/1988";
        lblAge.InnerText = " 25 ឆ្នាំ";
        lblSex.InnerText = " ស្រី";
        lblPhone.InnerText = " 016 339 499";
        lblAddress.InnerText = " ផ្ទះលេខ 213AE2 ផ្លូវលេខ 139 សង្កាត់អូរឬស្សី 3 ក្រុងភ្នំពេញ";
        lblTypeInsurancePlan.InnerText = " មូលធនសុភមង្គលគ្រួសារយើង 200";
        lblStandardPrem.InnerText = "236.00";
        lblSum_Insured.InnerText = " USD 10,000";
        lblExtra_Prem.InnerText = "0.00";
        lblCoverage_period.InnerText = "15 ឆ្នាំ";
        lblPayment_period.InnerText = "10 ឆ្នាំ";
        lblTotalPremium.InnerText = "236.00";
        lblEffective_date.InnerText = "09/01/2014";
        lblMode_payment.InnerText = "ប្រចាំឆ្នាំ";
        lblExpriy_date.InnerText = "08/01/2029";
        lblDue_date.InnerText = "ថ្ងៃទី 09 ខែមករា";
        lblMaturity_date.InnerText = "09/01/2029";

        /// Get Benefitciary 

        StringBuilder strDeviceList = new StringBuilder();
        string strTable = "<table width=\"1070px\" >"; //border=\"1\"
        //strTable += "<tr><th styple=\"text-align: center; \">ឈ្មោះ/Name</th><th styple=\"text-align: center; \">ទំនាក់ទំនង/Relationship</th><th style=\"text-align: center;\">ភាគរយនៃអត្ថប្រយោជន៍/Percentage Shares</th><th style=\"text-align: center;\">ចំណាំ/Remarks</th></tr>";

        foreach(DataRow dr in da_report_certificate_policy.Get_Benefitciary().Rows) 
        {
            //<td class="tright "> 
            //<span class='is_present'>Yes</span><br/> 

            //</td> 

            strTable += "<tr>";  //padding:5px 0px 5px 5px

            strTable += "<td style=\"text-align: center;   width=\"560px;\"  class=\"tb-schedule-td-right\">" + "<span>" + dr["Full_Name"].ToString() + "</span>" + "</td>";
            strTable += "<td style=\"text-align: left;   width=\"10px;\"   class=\"tb-schedule-td-right\">" + "<span>" + dr["Relationship"].ToString() + "<span" + "</td>";
            strTable += "<td style=\"text-align: left; width=\"360px;\"  class=\"tb-schedule-td-right\">" + "<span>" + dr["Percentage"].ToString() + "<span>" + "</td>";
            strTable += "<td style=\"text-align: center; width=\"10px;\"   class=\"tb-schedule-td-right\">" + "<span>" + "---------------" + "<span>" + "</td>";

            strTable += "</tr>";
        }

        strTable += "</table>";

        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append(strTable);

        Beneficiary_name.InnerHtml = strBuilder.ToString();

        /// End of Benefitciary
        
        /// Surrender Value
        lblEndPolicy_year1.InnerText = "1";
        lblSurrenderValue1.InnerText = "10";

        lblEndPolicy_year2.InnerText = "2";
        lblSurrenderValue2.InnerText = "20";

        lblEndPolicy_year3.InnerText = "3";
        lblSurrenderValue3.InnerText = "30";

        lblEndPolicy_year4.InnerText = "4";
        lblSurrenderValue4.InnerText = "40";

        lblEndPolicy_year5.InnerText = "5";
        lblSurrenderValue5.InnerText = "50";

        lblEndPolicy_year6.InnerText = "6";
        lblSurrenderValue6.InnerText = "60";

        lblEndPolicy_year7.InnerText = "7";
        lblSurrenderValue7.InnerText = "70";

        lblEndPolicy_year8.InnerText = "8";
        lblSurrenderValue8.InnerText = "80";

        lblEndPolicy_year9.InnerText = "9";
        lblSurrenderValue9.InnerText = "90";

        lblEndPolicy_year10.InnerText = "10";
        lblSurrenderValue10.InnerText = "100";

        lblEndPolicy_year11.InnerText = "11";
        lblSurrenderValue11.InnerText = "110";

        lblEndPolicy_year12.InnerText = "12";
        lblSurrenderValue12.InnerText = "120";

        lblEndPolicy_year13.InnerText = "13";
        lblSurrenderValue13.InnerText = "130";

        lblEndPolicy_year14.InnerText = "14";
        lblSurrenderValue14.InnerText = "140";

        lblEndPolicy_year15.InnerText = "15";
        lblSurrenderValue15.InnerText = "150";

        lblEndPolicy_year16.InnerText = "16";
        lblSurrenderValue16.InnerText = "160";

        lblEndPolicy_year17.InnerText = "17";
        lblSurrenderValue17.InnerText = "170";

        lblEndPolicy_year18.InnerText = "18";
        lblSurrenderValue18.InnerText = "180";

        lblEndPolicy_year19.InnerText = "19";
        lblSurrenderValue19.InnerText = "190";

        lblEndPolicy_year20.InnerText = "20";
        lblSurrenderValue20.InnerText = "200";

        lblEndPolicy_year21.InnerText = "21";
        lblSurrenderValue21.InnerText = "210";

        lblEndPolicy_year22.InnerText = "22";
        lblSurrenderValue22.InnerText = "220";

        lblEndPolicy_year23.InnerText = "23";
        lblSurrenderValue23.InnerText = "230";

        lblEndPolicy_year24.InnerText = "24";
        lblSurrenderValue24.InnerText = "240";

       
    }
}