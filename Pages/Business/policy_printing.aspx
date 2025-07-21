<%@ Page Title="Clife | Business => Policy Printing" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_printing.aspx.cs" Inherits="Pages_Business_policy_printing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <script src="../../Scripts/print.js"></script>
   <%-- <link href="../../App_Themes/print.css" rel="stylesheet" />--%>
    <%-- <link href="../../Scripts/bootstrap/css/bootstrap.css" rel="stylesheet" />--%>

    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnSelectAll" data-toggle="modal" data-target="#DateSelectionModal" runat="server" ImageUrl="~/App_Themes/functions/select_all.png" />
        </li>

        <li>
            <asp:ImageButton ID="ImageBtnSearch" runat="server" data-toggle="modal" data-target="#mySearchPolicy" ImageUrl="~/App_Themes/functions/search.png" />
        </li>

        <li>
            <input type="button" onclick="CheckSelection();" style="background: url('../../App_Themes/functions/print_policy_schedule.png') no-repeat; border: none; height: 40px; width: 144px;" />
        </li>

        <li>

           <input type="button" onclick="check_already_print();" style="background: url('../../App_Themes/functions/print_info_ack_letter.png') no-repeat; border: none; height: 40px; width: 144px;" />
             <%--<input type="button" data-toggle="modal" data-target="#paper_type" style="background: url('../../App_Themes/functions/print_info_ack_letter.png') no-repeat; border: none; height: 40px; width: 144px;" />--%>


        </li>
        <li>

           <input type="button" onclick="print_non_forfeiture();" style="background: url('../../App_Themes/functions/print_non_forfeiture.png') no-repeat; border: none; height: 40px; width: 144px;" />
             <%--<input type="button" data-toggle="modal" data-target="#paper_type" style="background: url('../../App_Themes/functions/print_info_ack_letter.png') no-repeat; border: none; height: 40px; width: 144px;" />--%>
        </li>
        <li>

           <input type="button" onclick="PrintReducedSI();"  style="background: url('../../App_Themes/functions/print_reduced_si.png') no-repeat; border: none; height: 40px; width: 144px;" />
             <%--<input type="button" data-toggle="modal" data-target="#paper_type" style="background: url('../../App_Themes/functions/print_info_ack_letter.png') no-repeat; border: none; height: 40px; width: 144px;" />--%>
        </li>
    </ul>

    <script type="text/javascript">
        //Format currency in database
        function formatCurrency(num) {
            num = num.toString();
            num = num.replace(/,/gi, "")

            if (num.length > 3) {
                var dec = 0
                var sep = ","
                var decChar = ","
                var pre = ""
                var post = ""
                num = isNaN(num) || num === '' || num === null ? 0.00 : num;
                var n = num.toString().split(decChar);
                return (pre || '') + n[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1" + sep) + (n.length > 1 ? decChar + n[1].substr(0, dec) : '') + (post || '');
            } else {
                return num;
            }

        }


        //Fucntion to check only one checkbox and highlight textbox
        function SelectSingleCheckBox(ckb, policy_id, product_id, gender) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");

                }

            }

            if (ckb.checked) {
                //Change style
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#f5f5f5");

                //Bind value to Hidden Fields    
                $('#Main_hdfPolicyID').val(policy_id);

                $('#Main_lblBenefit1').text("");
                $('#Main_lblBenefit2').text("");
                $('#Main_lblBenefit3').text("");
                $('#Main_lblBenefit4').text("");
                $('#Main_lblBenefit5').text("");
                $('#Main_lblRelationship1').text("");
                $('#Main_lblRelationship2').text("");
                $('#Main_lblRelationship3').text("");
                $('#Main_lblRelationship4').text("");
                $('#Main_lblRelationship5').text("");
                $('#Main_lblPercentage1').text("");
                $('#Main_lblPercentage2').text("");
                $('#Main_lblPercentage3').text("");
                $('#Main_lblPercentage4').text("");
                $('#Main_lblPercentage5').text("");
                $('#Main_lblBenefitRemark1').text("");
                $('#Main_lblBenefitRemark2').text("");
                $('#Main_lblBenefitRemark3').text("");
                $('#Main_lblBenefitRemark4').text("");
                $('#Main_lblBenefitRemark5').text("");

                $('#Main_hdfProductID').val(product_id);

                if (product_id == "PP200") {
                    $('#provision_pp_200').show();
                    $('#provision_mrta').hide();
                    $('#provision_term').hide();
                    $('#provision_whole').hide();
                }
                else if (product_id == "MRTA" || product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36") {
                    $('#provision_pp_200').hide();
                    $('#provision_mrta').show();
                    $('#provision_term').hide();
                    $('#provision_whole').hide();
                }

                else {
                    $('#provision_pp_200').hide();
                    $('#provision_mrta').hide();
                    $('#provision_term').show();
                    $('#provision_whole').hide();
                }

                //Get benefit payment clause
                //$.ajax({
                //    type: "POST",
                //    url: "../../ProductWebService.asmx/GetBenefitClause",
                //    data: "{product_id:'" + product_id + "'}",
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    success: function (data) {

                //        if (returned_product_id == "PP200") {
                //            $('#provision_pp_200').show();
                //            $('#provision_mrta').hide();                           
                //        }
                //        else if (returned_product_id == "MRTA" || returned_product_id == "MRTA12" || returned_product_id == "MRTA24" || returned_product_id == "MRTA36") {
                //            $('#provision_pp_200').hide();
                //            $('#provision_mrta').show();                           
                //        }
                //        else {
                //            $('#provision_pp_200').show();
                //            $('#provision_mrta').hide();                           
                //        }

                //    }
                //});

                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/GetBenefits",
                    data: "{policy_id:'" + policy_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        for (var i = 0; i < data.d.length; i++) {

                            var item = data.d[i];
                            if (i == 0) {
                                $('#Main_lblBenefit1').text(item.Full_Name);
                                $('#Main_lblRelationship1').text(item.Relationship_Khmer);
                                $('#Main_lblPercentage1').text(item.Percentage + '%');

                                if (product_id == "MRTA" || product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36") {
                                    $('#Main_lblBenefitRemark1').text("អ្នកទទួលផលចម្បង");
                                    $('#Main_lblPercentage1').text("ប្រាក់កម្ចីដែលនៅសល់");

                                }
                            }
                            if (i == 1) {
                                $('#Main_lblBenefit2').text(item.Full_Name);
                                $('#Main_lblRelationship2').text(item.Relationship_Khmer);
                                $('#Main_lblPercentage2').text(item.Percentage + '%');

                                if (product_id == "MRTA" || product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36") {
                                    $('#Main_lblBenefitRemark2').text("អ្នកទទួលផលបន្ទាប់បន្សំ");
                                }

                            }
                            if (i == 2) {
                                $('#Main_lblBenefit3').text(item.Full_Name);
                                $('#Main_lblRelationship3').text(item.Relationship_Khmer);
                                $('#Main_lblPercentage3').text(item.Percentage + '%');

                                if (product_id == "MRTA" || product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36") {
                                    $('#Main_lblBenefitRemark3').text("អ្នកទទួលផលបន្ទាប់បន្សំ");
                                }

                            }
                            if (i == 3) {
                                $('#Main_lblBenefit4').text(item.Full_Name);
                                $('#Main_lblRelationship4').text(item.Relationship_Khmer);
                                $('#Main_lblPercentage4').text(item.Percentage + '%');

                                if (product_id == "MRTA" || product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36") {
                                    $('#Main_lblBenefitRemark4').text("អ្នកទទួលផលបន្ទាប់បន្សំ");
                                }

                            }
                            if (i == 4) {
                                $('#Main_lblBenefit5').text(item.Full_Name);
                                $('#Main_lblRelationship5').text(item.Relationship_Khmer);
                                $('#Main_lblPercentage5').text(item.Percentage + '%');

                                if (product_id == "MRTA" || product_id == "MRTA12" || product_id == "MRTA24" || product_id == "MRTA36") {
                                    $('#Main_lblBenefitRemark5').text("អ្នកទទួលផលបន្ទាប់បន្សំ");
                                }

                            }
                        }
                    }

                });

                //Get policy detail from web service
                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/GetPolicyDetail",
                    data: "{policy_id:'" + policy_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        //pass data to hidden div for print
                        $('#Main_lblPolicyNumber').text(data.d.Policy_Number);
                        $('#Main_lblCustomerID').text(data.d.Customer_ID);

                        $('#Main_lblInsuredName').text(data.d.Customer_Fullname);
                        $('#Main_lblDateOfBirth').text(formatJSONDate(data.d.Birth_Date));
                        $('#Main_lblAge').text(data.d.Age_Insure);


                        $('#Main_lblPhoneNumber').text(data.d.Mobile_Phone1);
                        $('#Main_lblAddress').text(data.d.Customer_Address);

                        $('#Main_lblID').text(data.d.ID_Card);
                        $('#Main_lblGender').text(data.d.Gender);

                        $('#Main_lblInsurancePlan').text(data.d.Kh_Title);
                        $('#Main_lblStandardPremium').text(formatDollar(data.d.System_Premium));

                        $('#Main_lblSumInsured').text('USD  ' + formatCurrency(data.d.User_Sum_Insure));


                        $('#Main_lblExtraPremium').text(formatDollar(data.d.Extra_Premium));

                        $('#Main_lblCoveragePeriod').text(data.d.Assure_Year);
                        $('#Main_lblPaymentPeriod').text(data.d.Pay_Year);

                        $('#Main_lblTotalPremium').text(formatDollar(data.d.Total_Premium));

                        $('#Main_lblEffectiveDate').text(formatJSONDate(data.d.Effective_Date));
                        $('#Main_lblModeOfPayment').text(data.d.Payment_Mode);
                        $('#Main_lblExpiryDate').text(formatJSONDate(data.d.Expiry_Date));
                        $('#Main_lblDueDate').text(data.d.Next_Premium_Payment_Due_Date);
                        $('#Main_lblMaturityDate').text(formatJSONDate(data.d.Maturity_Date));
                        $('#Main_lblIssueDate').text(formatJSONDate(data.d.Issue_Date));

                        //Assign value to hidden field to calculate surrender value
                        //$('#Main_hdfAge').val(data.d.Age_Insure);
                        //$('#Main_hdfSumInsured').val(data.d.System_Sum_Insure);
                        //$('#Main_hdfAssureYear').val(data.d.Assure_Year);

                        //Get surrender value 
                        GetSurrenderValue(product_id, gender, data.d.Age_Insure, data.d.System_Sum_Insure, data.d.Assure_Year);

                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });


                //GetSurrenderValue(product_id, gender, $('#Main_hdfAge').val(), $('#Main_hdfSumInsured').val(), $('#Main_hdfAssureYear').val());


            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");
            }
        }

        function GetSurrenderValue(product_id, gender, age, sum_insured, assure_year) {

            //Get surrender value
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetSurrenderValue",
                data: "{product_id:'" + product_id + "',gender:'" + gender + "',customer_age:'" + age + "',sum_insured:'" + sum_insured + "',assure_year:'" + assure_year + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    switch (product_id) {

                        case "PP200":
                            $('#Main_lblPolicy1').text("1");
                            $('#Main_lblPolicy2').text("2");
                            $('#Main_lblPolicy3').text("3");
                            $('#Main_lblPolicy4').text("4");
                            $('#Main_lblPolicy5').text("5");
                            $('#Main_lblPolicy6').text("6");
                            $('#Main_lblPolicy7').text("7");
                            $('#Main_lblPolicy8').text("8");
                            $('#Main_lblPolicy9').text("9");
                            $('#Main_lblPolicy10').text("10");
                            $('#Main_lblPolicy11').text("11");
                            $('#Main_lblPolicy12').text("12");
                            $('#Main_lblPolicy13').text("13");
                            $('#Main_lblPolicy14').text("14");
                            $('#Main_lblPolicy15').text("15");

                            $('#Main_lblPolicy16').text("-");
                            $('#Main_lblPolicy17').text("-");
                            $('#Main_lblPolicy18').text("-");
                            $('#Main_lblPolicy19').text("-");
                            $('#Main_lblPolicy20').text("-");
                            $('#Main_lblPolicy21').text("-");
                            $('#Main_lblPolicy22').text("-");
                            $('#Main_lblPolicy23').text("-");
                            $('#Main_lblPolicy24').text("-");

                            $('#Main_lblSurrender16').text("-");
                            $('#Main_lblSurrender17').text("-");
                            $('#Main_lblSurrender18').text("-");
                            $('#Main_lblSurrender19').text("-");
                            $('#Main_lblSurrender20').text("-");
                            $('#Main_lblSurrender21').text("-");
                            $('#Main_lblSurrender22').text("-");
                            $('#Main_lblSurrender23').text("-");
                            $('#Main_lblSurrender24').text("-");

                            $('#Main_lblSurrenderValueTable').text("");

                            for (var j = 0; j < data.d.length; j++) {

                                //assign object to local variable
                                var cv_item = data.d[j];

                                switch (cv_item.Policy_Year) {
                                    case 1:
                                        $('#Main_lblSurrender1').text(formatCurrency(cv_item.CV_Amount));

                                    case 2:
                                        $('#Main_lblSurrender2').text(formatCurrency(cv_item.CV_Amount));
                                    case 3:
                                        $('#Main_lblSurrender3').text(formatCurrency(cv_item.CV_Amount));
                                    case 4:
                                        $('#Main_lblSurrender4').text(formatCurrency(cv_item.CV_Amount));
                                    case 5:
                                        $('#Main_lblSurrender5').text(formatCurrency(cv_item.CV_Amount));
                                    case 6:
                                        $('#Main_lblSurrender6').text(formatCurrency(cv_item.CV_Amount));
                                    case 7:
                                        $('#Main_lblSurrender7').text(formatCurrency(cv_item.CV_Amount));
                                    case 8:
                                        $('#Main_lblSurrender8').text(formatCurrency(cv_item.CV_Amount));
                                    case 9:
                                        $('#Main_lblSurrender9').text(formatCurrency(cv_item.CV_Amount));
                                    case 10:
                                        $('#Main_lblSurrender10').text(formatCurrency(cv_item.CV_Amount));
                                    case 11:
                                        $('#Main_lblSurrender11').text(formatCurrency(cv_item.CV_Amount));
                                    case 12:
                                        $('#Main_lblSurrender12').text(formatCurrency(cv_item.CV_Amount));
                                    case 13:
                                        $('#Main_lblSurrender13').text(formatCurrency(cv_item.CV_Amount));
                                    case 14:
                                        $('#Main_lblSurrender14').text(formatCurrency(cv_item.CV_Amount));
                                    case 15:
                                        $('#Main_lblSurrender15').text(formatCurrency(cv_item.CV_Amount));
                                }

                            }
                            break;

                        case "W9010":
                        case "W9015":
                        case "W9020": //when the product is PP200

                            $('#Main_lblSurrenderValueTable').text("- Please refer to attached document");

                            $('#Main_lblPolicy1').text("");
                            $('#Main_lblPolicy2').text("");
                            $('#Main_lblPolicy3').text("");
                            $('#Main_lblPolicy4').text("");
                            $('#Main_lblPolicy5').text("");
                            $('#Main_lblPolicy6').text("");
                            $('#Main_lblPolicy7').text("");
                            $('#Main_lblPolicy8').text("");
                            $('#Main_lblPolicy9').text("");
                            $('#Main_lblPolicy10').text("");
                            $('#Main_lblPolicy11').text("");
                            $('#Main_lblPolicy12').text("");
                            $('#Main_lblPolicy13').text("");
                            $('#Main_lblPolicy14').text("");
                            $('#Main_lblPolicy15').text("");
                            $('#Main_lblPolicy16').text("");
                            $('#Main_lblPolicy17').text("");
                            $('#Main_lblPolicy18').text("");
                            $('#Main_lblPolicy19').text("");
                            $('#Main_lblPolicy20').text("");
                            $('#Main_lblPolicy21').text("");
                            $('#Main_lblPolicy22').text("");
                            $('#Main_lblPolicy23').text("");
                            $('#Main_lblPolicy24').text("");
                            $('#Main_lblSurrender1').text("");
                            $('#Main_lblSurrender2').text("");
                            $('#Main_lblSurrender3').text("");
                            $('#Main_lblSurrender4').text("");
                            $('#Main_lblSurrender5').text("");
                            $('#Main_lblSurrender6').text("");
                            $('#Main_lblSurrender7').text("");
                            $('#Main_lblSurrender8').text("");
                            $('#Main_lblSurrender9').text("");
                            $('#Main_lblSurrender10').text("");
                            $('#Main_lblSurrender11').text("");
                            $('#Main_lblSurrender12').text("");
                            $('#Main_lblSurrender13').text("");
                            $('#Main_lblSurrender14').text("");
                            $('#Main_lblSurrender15').text("");
                            $('#Main_lblSurrender16').text("");
                            $('#Main_lblSurrender17').text("");
                            $('#Main_lblSurrender18').text("");
                            $('#Main_lblSurrender19').text("");
                            $('#Main_lblSurrender20').text("");
                            $('#Main_lblSurrender21').text("");
                            $('#Main_lblSurrender22').text("");
                            $('#Main_lblSurrender23').text("");
                            $('#Main_lblSurrender24').text("");

                            break;

                        case "MRTA": //when the product is MRTA
                        case "MRTA12":
                        case "MRTA24":
                        case "MRTA36":

                            $('#Main_lblPolicy1').text("0");
                            $('#Main_lblPolicy2').text("1");
                            $('#Main_lblPolicy3').text("2");
                            $('#Main_lblPolicy4').text("3");
                            $('#Main_lblPolicy5').text("4");
                            $('#Main_lblPolicy6').text("5");
                            $('#Main_lblPolicy7').text("6");
                            $('#Main_lblPolicy8').text("7");
                            $('#Main_lblPolicy9').text("8");
                            $('#Main_lblPolicy10').text("9");
                            $('#Main_lblPolicy11').text("10");
                            $('#Main_lblPolicy12').text("11");
                            $('#Main_lblPolicy13').text("12");
                            $('#Main_lblPolicy14').text("13");
                            $('#Main_lblPolicy15').text("14");
                            $('#Main_lblPolicy16').text("15");
                            $('#Main_lblPolicy17').text("16");
                            $('#Main_lblPolicy18').text("17");
                            $('#Main_lblPolicy19').text("18");
                            $('#Main_lblPolicy20').text("19");
                            $('#Main_lblPolicy21').text("20");
                            $('#Main_lblPolicy22').text("");
                            $('#Main_lblPolicy23').text("");
                            $('#Main_lblPolicy24').text("");

                            $('#Main_lblSurrender1').text("-");
                            $('#Main_lblSurrender2').text("-");
                            $('#Main_lblSurrender3').text("-");
                            $('#Main_lblSurrender4').text("-");
                            $('#Main_lblSurrender5').text("-");
                            $('#Main_lblSurrender6').text("-");
                            $('#Main_lblSurrender7').text("-");
                            $('#Main_lblSurrender8').text("-");
                            $('#Main_lblSurrender9').text("-");
                            $('#Main_lblSurrender10').text("-");
                            $('#Main_lblSurrender11').text("-");
                            $('#Main_lblSurrender12').text("-");
                            $('#Main_lblSurrender13').text("-");
                            $('#Main_lblSurrender14').text("-");
                            $('#Main_lblSurrender15').text("-");
                            $('#Main_lblSurrender16').text("-");
                            $('#Main_lblSurrender17').text("-");
                            $('#Main_lblSurrender18').text("-");
                            $('#Main_lblSurrender19').text("-");
                            $('#Main_lblSurrender20').text("-");
                            $('#Main_lblSurrender21').text("-");
                            $('#Main_lblSurrender22').text("-");
                            $('#Main_lblSurrender23').text("-");
                            $('#Main_lblSurrender24').text("-");

                            $('#Main_lblSurrenderValueTable').text("");

                            for (var k = 0; k < data.d.length; k++) {

                                //assign object to local variable
                                var cv_item = data.d[k];

                                switch (cv_item.Policy_Year) {
                                    case 0:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender1').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 1:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender2').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 2:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender3').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 3:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender4').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 4:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender5').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 5:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender6').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 6:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender7').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 7:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender8').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 8:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender9').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 9:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender10').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 10:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender11').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 11:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender12').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 12:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender13').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 13:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender14').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 14:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender15').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 15:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender16').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 16:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender17').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 17:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender18').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 18:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender19').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 19:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender20').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                    case 20:
                                        if (cv_item.CV_Amount > 0) {
                                            $('#Main_lblSurrender21').text(formatCurrency(cv_item.CV_Amount));
                                            cv_item.CV_Amount = 0;
                                        }
                                }

                            }

                            break;

                        default:
                            $('#Main_lblPolicy1').text("-");
                            $('#Main_lblPolicy2').text("-");
                            $('#Main_lblPolicy3').text("-");
                            $('#Main_lblPolicy4').text("-");
                            $('#Main_lblPolicy5').text("-");
                            $('#Main_lblPolicy6').text("-");
                            $('#Main_lblPolicy7').text("-");
                            $('#Main_lblPolicy8').text("-");
                            $('#Main_lblPolicy9').text("-");
                            $('#Main_lblPolicy10').text("-");
                            $('#Main_lblPolicy11').text("-");
                            $('#Main_lblPolicy12').text("-");
                            $('#Main_lblPolicy13').text("-");
                            $('#Main_lblPolicy14').text("-");
                            $('#Main_lblPolicy15').text("-");
                            $('#Main_lblPolicy16').text("-");
                            $('#Main_lblPolicy17').text("-");
                            $('#Main_lblPolicy18').text("-");
                            $('#Main_lblPolicy19').text("-");
                            $('#Main_lblPolicy20').text("-");
                            $('#Main_lblPolicy21').text("-");
                            $('#Main_lblPolicy22').text("-");
                            $('#Main_lblPolicy23').text("-");
                            $('#Main_lblPolicy24').text("-");
                            $('#Main_lblSurrender1').text("-");
                            $('#Main_lblSurrender2').text("-");
                            $('#Main_lblSurrender3').text("-");
                            $('#Main_lblSurrender4').text("-");
                            $('#Main_lblSurrender5').text("-");
                            $('#Main_lblSurrender6').text("-");
                            $('#Main_lblSurrender7').text("-");
                            $('#Main_lblSurrender8').text("-");
                            $('#Main_lblSurrender9').text("-");
                            $('#Main_lblSurrender10').text("-");
                            $('#Main_lblSurrender11').text("-");
                            $('#Main_lblSurrender12').text("-");
                            $('#Main_lblSurrender13').text("-");
                            $('#Main_lblSurrender14').text("-");
                            $('#Main_lblSurrender15').text("-");
                            $('#Main_lblSurrender16').text("-");
                            $('#Main_lblSurrender17').text("-");
                            $('#Main_lblSurrender18').text("-");
                            $('#Main_lblSurrender19').text("-");
                            $('#Main_lblSurrender20').text("-");
                            $('#Main_lblSurrender21').text("-");
                            $('#Main_lblSurrender22').text("-");
                            $('#Main_lblSurrender23').text("-");
                            $('#Main_lblSurrender24').text("-");
                            break;
                    }

                }

            });
        }

        //Check whether user make any selection before printing       
        function CheckSelection() {

            var GridView = document.getElementById('<%= gvPolicy.ClientID %>');
            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                    //alert($('#Main_hdfProductID').val());
                    var pro_id = $('#Main_hdfProductID').val();
                    pro_id = pro_id.substring(0, 3);

                    if (pro_id == "NFP" || pro_id == "FPP" || pro_id == "SDS" || pro_id == "CL2") {
                        // alert("New family Protection" + $('#Main_hdfPolicyID').val());
                        //window.open("Reports/policy_schedule_RP_new.aspx?policy_id=" + $('#Main_hdfPolicyID').val(), "_newtab");
                        window.open("Reports/approver_form.aspx?policy_id=" + $('#Main_hdfPolicyID').val(), "_blank");
                        return;
                    }
                    else {
                        PrintDetails();
                        return;
                    }
                }

            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }

        //Print reduced sum assured   
        function PrintReducedSI() {

            var GridView = document.getElementById('<%= gvPolicy.ClientID %>');
            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                   
                    window.open("Reports/reduced_sum_assured_cl24_rp.aspx?policy_id=" + $('#Main_hdfPolicyID').val(), "_blank");
                    return;
                }

            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }
        //Check whether user make any selection before print inform letter       
        function CheckSelection_inform_letter()
        {

            var GridView = document.getElementById('<%= gvPolicy.ClientID %>');
                var ckbList = GridView.getElementsByTagName("input");

                for (i = 0; i < ckbList.length; i++) {
                    if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {

                        //check existing approver
                        //var approver = get_approver($('#Main_hdfPolicyID').val());
                        //alert(approver.PositionEN);

                        $.ajax({
                            type: "POST",
                            url: "../../ReportApprover.asmx/GetApproverInfo",
                            data: "{policy_id:'" + $('#Main_hdfPolicyID').val() + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.d.PositionEn != '' && data.d.PositionEn != 'null' && data.d.PositionEn != null) {
                                    //check printed inform letter
                                    var paper = $('#<%= ddlPaper_Type.ClientID%>').val();
                                    window.open("Reports/inform_letter_rp.aspx?policy_id=" + $('#Main_hdfPolicyID').val() + "&p_type=" + paper, "_blank");
                                    return;
                                }
                                else {
                                   
                                    //save approver policy
                                   
                                    $('#Approver').modal('show');
                                    return;
                                }

                            },
                            error: function ( message) {
                                alert(message);
                            }

                        });
                        return;
                }
                    
            }
            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }
        //print non-forfeiture value table
        function print_non_forfeiture() {

            var GridView = document.getElementById('<%= gvPolicy.ClientID %>');
            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                    //alert($('#Main_hdfProductID').val());
                    var pro_id = $('#Main_hdfProductID').val();
                    pro_id = pro_id.substring(0, 3);

                    if (pro_id == "NFP" || pro_id == "FPP" || pro_id == "SDS") {
                        // alert("New family Protection" + $('#Main_hdfPolicyID').val());
                        //window.open("Reports/policy_schedule_RP_new.aspx?policy_id=" + $('#Main_hdfPolicyID').val(), "_newtab");
                        window.open("Reports/non_forfeiture_rp.aspx?policy_id=" + $('#Main_hdfPolicyID').val(), "_blank");
                        return;
                    }
                   
                }

            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }

        function select_paper() {
            var paper = $('#<%= ddlPaper_Type.ClientID%>').val();
            if (paper != '') {
                $('#div_message').html('Please make sure you have already input letter head in the tray of the printer');
            }
            else {
                $('#div_message').html('');
            }
        }

       
        //<function check already printed inform letter by: maneth 17032017>
        function check_already_print() {
            var GridView = document.getElementById('<%= gvPolicy.ClientID %>');
            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                    //check printed inform letter
                    $.ajax({
                        type: "POST",
                        url: "../../PolicyWebService.asmx/GetInformLetter",
                        data: "{policy_id:'" + $('#Main_hdfPolicyID').val() + "', report_type:'Acknowledge'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d.length > 0) {
                                $('#mgs_reprint').html('This policy had already printed.');
                                $('#reprint').modal('show');
                                return;
                            }
                            else {
                                $('#paper_type').modal('show');
                                return;

                            }
                        }
                    });
                    return;
                }
            }
            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');
        }
        //<end function check already printed inform letter by: maneth 17032017>

        //<function open modal paper type by: maneth 17032017>
        function open_paper_type()
        {
            $('#paper_type').modal('show');
        }
        //<end function open modal paper type by: maneth 17032017>

        function select_position()
        {
            var position_en = $('#<%=ddlPosition.ClientID%>');
           // alert(position_en.val());
            //get approver list
                $.ajax({
                    type: "POST",
                    url: "../../ReportApprover.asmx/GetApproverName",
                    data: "{position_en:'" + position_en.val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var name = $('#<%=ddlFullName.ClientID%>');
                        name.empty();
                        $.each(data.d, function (i, item) {
                            //alert(item.ID);
                            name.append($('<option>',

                                {
                                    value: item.ID,
                                     text: item.NameEn
                                }));
                            $('#div_approver_message').html('');

                        });
                       
                    },
                    error: function (err)
                    {
                        alert('error:' +err);
                    }

                });
        }

        function save_approver() {
            var name = $('#<% =ddlFullName.ClientID%>');
            var position = $('#<%=ddlPosition.ClientID%>');

            if (name.val() != '' & position.val() != '') {
                // alert(position_en.val());
                //get approver list
                $.ajax({
                    type: "POST",
                    url: "../../ReportApprover.asmx/SaveApprover",
                    data: "{policy_id:'" + $('#Main_hdfPolicyID').val() + "', approver_id:'" + name.val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d) {
                            var paper = $('#<%= ddlPaper_Type.ClientID%>').val();
                            window.open("Reports/inform_letter_rp.aspx?policy_id=" + $('#Main_hdfPolicyID').val() + "&p_type=" + paper, "_blank");
                            return;
                        }
                        else {
                            alert('Approver had been saved fail, pleasce contact your system administrator.');
                            return;
                        }

                    },
                    error: function (err) {
                        alert('error:' + err);
                    }

                });
                $('#div_approver_message').html('');
            }
            else {
                $('#div_approver_message').html('Position and Full Name are required.');
                $('#Approver').modal('show');
                return;
            }

        }

        //Function to print policy
        function PrintDetails() {
            //Open hidden div and print
            var printContent = document.getElementById('<%= printarea.ClientID %>');

            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, '');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.getElementById('print-content').style.display = 'block';
            printWindow.document.write('<link rel="stylesheet" href="../../App_Themes/print.css" type="text/css" />');

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

            return false;
        }

        function formatJSONDate(jsonDate) {
            var value = jsonDate;
            if (value.substring(0, 6) == "/Date(") {
                var dt = new Date(parseInt(value.substring(6, value.length - 2)));
                var kh_month = "";

                //get month in word
                switch (dt.getMonth() + 1) {
                    case 1:
                        kh_month = "មករា";
                        break;
                    case 2:
                        kh_month = "កុម្ភៈ";
                        break;
                    case 3:
                        kh_month = "មីនា";
                        break;
                    case 4:
                        kh_month = "មេសា";
                        break;
                    case 5:
                        kh_month = "ឧសភា";
                        break;
                    case 6:
                        kh_month = "មិថុនា";
                        break;
                    case 7:
                        kh_month = "កក្កដា";
                        break;
                    case 8:
                        kh_month = "សីហា";
                        break;
                    case 9:
                        kh_month = "កញ្ញា";
                        break;
                    case 10:
                        kh_month = "តុលា";
                        break;
                    case 11:
                        kh_month = "វិច្ឆិកា";
                        break;
                    default:
                        kh_month = "ធ្នូ";
                        break;
                }

                var dtString = dt.getDate() + " " + kh_month + " " + dt.getFullYear();
                value = dtString;


            }

            return value;

        }

        function formatDollar(num) {
            var p = num.toFixed(2).split(".");
            return "" + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
                return num + (i && !(i % 3) ? "," : "") + acc;
            }, "") + "." + p[1];
        }

       
    </script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">


    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--Operation here--%>
            <br />
            <br />
            <br />

            <div id="printarea" runat="server" class="print_content">

                <div id="print-content" class="print-content" style="display: none;">

                    <asp:Label ID="lblPolicyNumber" runat="server" Text="" CssClass="policy_number"></asp:Label>
                    <asp:Label ID="lblCustomerID" runat="server" Text="" CssClass="customer_id"></asp:Label>

                    <br />
                    <br />
                    <br />
                    <br />
                    <br />

                    <asp:Label ID="lblInsuredName" runat="server" Text="" CssClass="insured_name"></asp:Label>
                    <asp:Label ID="lblID" runat="server" Text="" CssClass="id_card"></asp:Label>
                    <asp:Label ID="lblDateOfBirth" runat="server" Text="" CssClass="dob"></asp:Label>
                    <asp:Label ID="lblAge" runat="server" Text="" CssClass="age"></asp:Label>
                    <asp:Label ID="lblGender" runat="server" Text="" CssClass="gender"></asp:Label>
                    <asp:Label ID="lblPhoneNumber" runat="server" Text="" CssClass="phone"></asp:Label>
                    <asp:Label ID="lblAddress" runat="server" Text="" CssClass="address"></asp:Label>

                    <%---------------------------------------------------------------------------------------------------------%>

                    <asp:Label ID="lblInsurancePlan" runat="server" Text="" CssClass="insurance_plan"></asp:Label>

                    <asp:Label ID="lblUSDStandardPremium" runat="server" Text="USD" CssClass="standard_premium_usd"></asp:Label>
                    <asp:Label ID="lblStandardPremium" runat="server" Text="" CssClass="standard_premium"></asp:Label>
                    <%--<asp:TextBox ID="txtStandardPremium" runat="server" Text="abcde" CssClass="standard_premium"></asp:TextBox>--%>

                    <asp:Label ID="lblSumInsured" runat="server" Text="" CssClass="sum_insured"></asp:Label>

                    <asp:Label ID="lblUSDExtraPremium" runat="server" Text="USD" CssClass="extra_premium_usd"></asp:Label>
                    <asp:Label ID="lblExtraPremium" runat="server" Text="" CssClass="extra_premium"></asp:Label>


                    <asp:Label ID="lblCoveragePeriod" runat="server" Text="" CssClass="coverage_period"></asp:Label>
                    <asp:Label ID="lblPaymentPeriod" runat="server" Text="" CssClass="payment_period"></asp:Label>

                    <asp:Label ID="lblUSDTotalPremium" runat="server" Text="USD" CssClass="total_premium_usd"></asp:Label>
                    <asp:Label ID="lblTotalPremium" runat="server" Text="" CssClass="total_premium"></asp:Label>

                    <asp:Label ID="lblEffectiveDate" runat="server" Text="" CssClass="effective_date"></asp:Label>
                    <asp:Label ID="lblModeOfPayment" runat="server" Text="" CssClass="mode_of_payment"></asp:Label>
                    <asp:Label ID="lblExpiryDate" runat="server" Text="" CssClass="expiry_date"></asp:Label>
                    <asp:Label ID="lblDueDate" runat="server" Text="" CssClass="due_date"></asp:Label>
                    <asp:Label ID="lblMaturityDate" runat="server" Text="" CssClass="maturity_date"></asp:Label>

                    <%---------------------------------------------------------------------------------------------------------%>

                    <%-- <asp:Label ID="lblPaymentClause" runat="server" Text="-" CssClass="payment_clause"></asp:Label>--%>

                    <div id="provision_pp_200" class="payment_clause" style="display: none;">
                        ២.១ អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព 
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;២.១.១	ប្រសិនបើមរណភាពបណ្តាលមកពីជម្ងឺ ឬ ជរាពាធ ក្រុមហ៊ុននឹងទូទាត់ ៖<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ក) ១០០% នៃចំនួនទឹកប្រាក់ធានារ៉ាបរងសរុប និង
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ខ) ១១៥% នៃបុព្វលាភរ៉ាប់រងប្រចាំឆ្នាំសរុបដែលបានបង់រួច ដែលគិតតាមរបៀបទូរទាត់ប្រចាំឆ្នាំ (មិនគិតបញ្ចូលការប្រាក់ បុព្វលាភរ៉ាប់រងបន្ថែម តម្លៃអប្បហារ បុព្វលាភលើផលប្រយោជន៍បន្ថែម
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ពន្ធលើបុព្វលាភរ៉ាប់រង និង/ឬ ថ្លៃសេវាកម្មផ្សេងៗដែលត្រូវដាក់បន្ទុកលើប័ណ្ណសន្យារ៉ាប់រង)<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;២.១.២	ប្រសិនបើមរណភាពបណ្តាលពីគ្រោះថ្នាក់ចៃដន្យ ក្រុមហ៊ុននឹងទូទាត់ ៖<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ក) ២០០% នៃចំនួនទឹកប្រាក់ធានារ៉ាបរងសរុប និង<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ខ) ១១៥% នៃបុព្វលាភរ៉ាប់រងប្រចាំឆ្នាំសរុបដែលបានបង់រួច ដែលគិតតាមរបៀបទូរទាត់ប្រចាំឆ្នាំ (មិនគិតបញ្ចូលការប្រាក់ បុព្វលាភរ៉ាប់រងបន្ថែម តម្លៃអប្បហារ បុព្វលាភលើផលប្រយោជន៍បន្ថែម<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ពន្ធលើបុព្វលាភរ៉ាប់រង និង/ឬ ថ្លៃសេវាកម្មផ្សេងៗដែលត្រូវដាក់បន្ទុកលើប័ណ្ណសន្យារ៉ាប់រង)<br />
                        ២.២ អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងមានជីវិតរស់នៅ  
                        <br />
                        ប្រសិនបើអ្នកត្រូវបានធានារ៉ាប់រងនៅមានជីវិតរស់នៅរហូតដល់កាលបរិច្ឆេទផុតកំណត់ធានាក្រុមហ៊ុននឹងទូទាត់ ១១៥% នៃបុព្វលាភរ៉ាប់រងប្រចាំឆ្នាំសរុបដែលបានបង់រួច ដែលគិតតាមរបៀបទូរទាត់ប្រចាំឆ្នាំ<br />
                        (មិនគិតបញ្ចូលការប្រាក់ បុព្វលាភរ៉ាប់រងបន្ថែម តម្លៃអប្បហារ បុព្វលាភលើផលប្រយោជន៍បន្ថែម ពន្ធលើបុព្វលាភរ៉ាប់រង និង/ឬ ថ្លៃសេវាកម្មផ្សេងៗ ដែលត្រូវដាក់បន្ទុកលើប័ណ្ណសន្យារ៉ាប់រង) ទៅកាន់អ្នកត្រូវបាន<br />
                        ធានារ៉ាប់រងនៅកាលបរិច្ឆេទផុតកំណត់ធានា។

                    </div>

                    <div id="provision_mrta" class="payment_clause_mrta" style="display: none;">
                        ៥.១ អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព<br />
                        ក្នុងករណីដែលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព ហើយក្រុមហ៊ុនទទួលបានភស្តុតាងនៃការទទួលមរណភាពនេះដូចដែលក្រុមហ៊ុនបានស្នើសុំ នោះក្រុមហ៊ុននឹងទូទាត់អត្ថប្រយោជន៍
                        <br />
                        ទៅកាន់អ្នកទទួលផលចម្បង (Major Beneficiary) ស្ថិតក្រោមតារាងកាត់បន្ថយទឹកប្រាក់ធានារ៉ាប់រង (Reduced Sum Insured Table) ក្នុងខែដែលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព។
                        <br />
                        ចំនួនដែលត្រូវទូទាត់នឹងមិនច្រើនជាងបំណុលដែលនៅជំពាក់អ្នកទទួលផលចម្បង (Major Beneficiary) ឡើយ។ អត្ថប្រយោជន៍ដែលនៅសល់បន្ទាប់ពីទូទាត់បំណុលហើយ (ប្រសិនបើមាន)
                        <br />
                        ក្រុមហ៊ុននឹងទូទាត់ទៅកាន់អ្នកទទួលផលបន្ទាប់បន្សំ (Minor Beneficiary) ។
                        <br />
                        ៥.២ អត្ថប្រយោជន៍ករណីពិការភាពទាំងស្រុង និងជាអចិន្ត្រៃយ៍<br />
                        ក្នុងករណី អ្នកត្រូវបានធានារ៉ាប់រងក្លាយទៅជាមនុស្សពិការទាំងស្រុងនិងជាអចិន្ត្រៃយ៍ ហើយពិការភាពនោះកើតឡើងជាបន្តបន្ទាប់ក្នុងរយៈពេលមិនតិចជាង ១៨០ថ្ងៃ  ឬ ក្នុងករណីដែលអ្នកត្រូវបាន<br />
                        ធានារ៉ាប់រងក្លាយទៅជាមនុស្សពិការទាំងស្រុង និងជាអចិន្ត្រៃយ៍ដោយមានភស្តុតាងជាក់លាក់ ឬ មានការបញ្ជាក់វេជ្ជសាស្ត្រច្បាស់លាស់ នោះក្រុមហ៊ុននឹងទូទាត់ផលប្រយោជន៍ 
                        <br />
                        ទៅកាន់អ្នកទទួលផលចម្បង (Major Beneficiary) ស្ថិតក្រោមតារាងកាត់បន្ថយទឹកប្រាក់ធានារ៉ាប់រង (Reduced Sum Insured Table) ក្នុងខែដែលអ្នកត្រូវបានធានារ៉ាប់រងក្លាយជា
                        <br />
                        មនុស្សពិការទាំងស្រុងនិងជាអចិន្ត្រៃយ៍ ។ ចំនួនដែលត្រូវទូទាត់នឹងមិនច្រើនជាងបំណុលដែលនៅជំពាក់ អ្នកទទួលផលចម្បង (Major Beneficiary) ឡើយ។ ផលប្រយោជន៍
                        <br />
                        ដែលនៅសល់បន្ទាប់ពីទូទាត់បំណុលហើយ (ប្រសិនបើមាន) ក្រុមហ៊ុននឹងទូទាត់ទៅកាន់អ្នកត្រូវបានធានារ៉ាប់រង។
                    </div>

                    <div id="provision_term" class="payment_clause_term" style="display: none;">
                        អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព:<br />
                        ​ប្រសិនបើ​អ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាពក្នុងកំឡុងពេលធានា ក្រុមហ៊ុននឹងធ្វើការទូទាត់អត្ថប្រយោជន៍ទៅកាន់អ្នកទទួលផល ឬ
                        <br />
                        ទៅកាន់អ្នកទទួលមរតកស្របច្បាប់របស់អ្នកត្រូវបានធានារ៉ាប់រង ទៅតាមករណីណាមួយដែលអាចកើតឡើង ។

                    </div>
                    <div id="provision_whole" class="payment_clause_term" style="display: none;">
                        អត្ថប្រយោជន៏ពេលអ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាព:<br />
                        ​ប្រសិនបើ​អ្នកត្រូវបានធានារ៉ាប់រងទទួលមរណភាពក្នុងកំឡុងពេលធានា ក្រុមហ៊ុននឹងធ្វើការទូទាត់អត្ថប្រយោជន៍ទៅកាន់អ្នកទទួលផល ឬ
                        <br />
                        ទៅកាន់អ្នកទទួលមរតកស្របច្បាប់របស់អ្នកត្រូវបានធានារ៉ាប់រង ទៅតាមករណីណាមួយដែលអាចកើតឡើង ។
                    
                    </div>

                    <%---------------------------------------------------------------------------------------------------------%>

                    <asp:Label ID="lblBenefit1" runat="server" Text="" CssClass="benefit1"></asp:Label>
                    <asp:Label ID="lblBenefit2" runat="server" Text="" CssClass="benefit2"></asp:Label>
                    <asp:Label ID="lblBenefit3" runat="server" Text="" CssClass="benefit3"></asp:Label>
                    <asp:Label ID="lblBenefit4" runat="server" Text="" CssClass="benefit4"></asp:Label>
                    <asp:Label ID="lblBenefit5" runat="server" Text="" CssClass="benefit5"></asp:Label>

                    <asp:Label ID="lblRelationship1" runat="server" Text="" CssClass="relationship1"></asp:Label>
                    <asp:Label ID="lblRelationship2" runat="server" Text="" CssClass="relationship2"></asp:Label>
                    <asp:Label ID="lblRelationship3" runat="server" Text="" CssClass="relationship3"></asp:Label>
                    <asp:Label ID="lblRelationship4" runat="server" Text="" CssClass="relationship4"></asp:Label>
                    <asp:Label ID="lblRelationship5" runat="server" Text="" CssClass="relationship5"></asp:Label>

                    <asp:Label ID="lblBenefitRemark1" runat="server" Text="" CssClass="remark1"></asp:Label>
                    <asp:Label ID="lblBenefitRemark2" runat="server" Text="" CssClass="remark2"></asp:Label>
                    <asp:Label ID="lblBenefitRemark3" runat="server" Text="" CssClass="remark3"></asp:Label>
                    <asp:Label ID="lblBenefitRemark4" runat="server" Text="" CssClass="remark4"></asp:Label>
                    <asp:Label ID="lblBenefitRemark5" runat="server" Text="" CssClass="remark5"></asp:Label>

                    <asp:Label ID="lblPercentage1" runat="server" Text="" CssClass="percentage1"></asp:Label>
                    <asp:Label ID="lblPercentage2" runat="server" Text="" CssClass="percentage2"></asp:Label>
                    <asp:Label ID="lblPercentage3" runat="server" Text="" CssClass="percentage3"></asp:Label>
                    <asp:Label ID="lblPercentage4" runat="server" Text="" CssClass="percentage4"></asp:Label>
                    <asp:Label ID="lblPercentage5" runat="server" Text="" CssClass="percentage5"></asp:Label>

                    <%---------------------------------------------------------------------------------------------------------%>



                    <%---------------------------------------------------------------------------------------------------------%>
                    <asp:Label ID="lblSurrenderValueTable" runat="server" Text="" CssClass="surrender_value_table"></asp:Label>

                    <asp:Label ID="lblPolicy1" runat="server" Text="" CssClass="policy_year_1"></asp:Label>
                    <asp:Label ID="lblPolicy2" runat="server" Text="" CssClass="policy_year_2"></asp:Label>
                    <asp:Label ID="lblPolicy3" runat="server" Text="" CssClass="policy_year_3"></asp:Label>
                    <asp:Label ID="lblPolicy4" runat="server" Text="" CssClass="policy_year_4"></asp:Label>
                    <asp:Label ID="lblPolicy5" runat="server" Text="" CssClass="policy_year_5"></asp:Label>
                    <asp:Label ID="lblPolicy6" runat="server" Text="" CssClass="policy_year_6"></asp:Label>

                    <asp:Label ID="lblSurrender1" runat="server" Text="" CssClass="surrender_value_1"></asp:Label>
                    <asp:Label ID="lblSurrender2" runat="server" Text="" CssClass="surrender_value_2"></asp:Label>
                    <asp:Label ID="lblSurrender3" runat="server" Text="" CssClass="surrender_value_3"></asp:Label>
                    <asp:Label ID="lblSurrender4" runat="server" Text="" CssClass="surrender_value_4"></asp:Label>
                    <asp:Label ID="lblSurrender5" runat="server" Text="" CssClass="surrender_value_5"></asp:Label>
                    <asp:Label ID="lblSurrender6" runat="server" Text="" CssClass="surrender_value_6"></asp:Label>

                    <%---------------------------------------------------------------------------------------------------------%>

                    <asp:Label ID="lblPolicy7" runat="server" Text="" CssClass="policy_year_7"></asp:Label>
                    <asp:Label ID="lblPolicy8" runat="server" Text="" CssClass="policy_year_8"></asp:Label>
                    <asp:Label ID="lblPolicy9" runat="server" Text="" CssClass="policy_year_9"></asp:Label>
                    <asp:Label ID="lblPolicy10" runat="server" Text="" CssClass="policy_year_10"></asp:Label>
                    <asp:Label ID="lblPolicy11" runat="server" Text="" CssClass="policy_year_11"></asp:Label>
                    <asp:Label ID="lblPolicy12" runat="server" Text="" CssClass="policy_year_12"></asp:Label>

                    <asp:Label ID="lblSurrender7" runat="server" Text="" CssClass="surrender_value_7"></asp:Label>
                    <asp:Label ID="lblSurrender8" runat="server" Text="" CssClass="surrender_value_8"></asp:Label>
                    <asp:Label ID="lblSurrender9" runat="server" Text="" CssClass="surrender_value_9"></asp:Label>
                    <asp:Label ID="lblSurrender10" runat="server" Text="" CssClass="surrender_value_10"></asp:Label>
                    <asp:Label ID="lblSurrender11" runat="server" Text="" CssClass="surrender_value_11"></asp:Label>
                    <asp:Label ID="lblSurrender12" runat="server" Text="" CssClass="surrender_value_12"></asp:Label>


                    <%---------------------------------------------------------------------------------------------------------%>

                    <asp:Label ID="lblPolicy13" runat="server" Text="" CssClass="policy_year_13"></asp:Label>
                    <asp:Label ID="lblPolicy14" runat="server" Text="" CssClass="policy_year_14"></asp:Label>
                    <asp:Label ID="lblPolicy15" runat="server" Text="" CssClass="policy_year_15"></asp:Label>
                    <asp:Label ID="lblPolicy16" runat="server" Text="" CssClass="policy_year_16"></asp:Label>
                    <asp:Label ID="lblPolicy17" runat="server" Text="" CssClass="policy_year_17"></asp:Label>
                    <asp:Label ID="lblPolicy18" runat="server" Text="" CssClass="policy_year_18"></asp:Label>

                    <asp:Label ID="lblSurrender13" runat="server" Text="" CssClass="surrender_value_13"></asp:Label>
                    <asp:Label ID="lblSurrender14" runat="server" Text="" CssClass="surrender_value_14"></asp:Label>
                    <asp:Label ID="lblSurrender15" runat="server" Text="" CssClass="surrender_value_15"></asp:Label>
                    <asp:Label ID="lblSurrender16" runat="server" Text="" CssClass="surrender_value_16"></asp:Label>
                    <asp:Label ID="lblSurrender17" runat="server" Text="" CssClass="surrender_value_17"></asp:Label>
                    <asp:Label ID="lblSurrender18" runat="server" Text="" CssClass="surrender_value_18"></asp:Label>

                    <%---------------------------------------------------------------------------------------------------------%>

                    <asp:Label ID="lblPolicy19" runat="server" Text="" CssClass="policy_year_19"></asp:Label>
                    <asp:Label ID="lblPolicy20" runat="server" Text="" CssClass="policy_year_20"></asp:Label>
                    <asp:Label ID="lblPolicy21" runat="server" Text="" CssClass="policy_year_21"></asp:Label>
                    <asp:Label ID="lblPolicy22" runat="server" Text="" CssClass="policy_year_22"></asp:Label>
                    <asp:Label ID="lblPolicy23" runat="server" Text="" CssClass="policy_year_23"></asp:Label>
                    <asp:Label ID="lblPolicy24" runat="server" Text="" CssClass="policy_year_24"></asp:Label>

                    <asp:Label ID="lblSurrender19" runat="server" Text="" CssClass="surrender_value_19"></asp:Label>
                    <asp:Label ID="lblSurrender20" runat="server" Text="" CssClass="surrender_value_20"></asp:Label>
                    <asp:Label ID="lblSurrender21" runat="server" Text="" CssClass="surrender_value_21"></asp:Label>
                    <asp:Label ID="lblSurrender22" runat="server" Text="" CssClass="surrender_value_22"></asp:Label>
                    <asp:Label ID="lblSurrender23" runat="server" Text="" CssClass="surrender_value_23"></asp:Label>
                    <asp:Label ID="lblSurrender24" runat="server" Text="" CssClass="surrender_value_24"></asp:Label>

                    <asp:Label ID="lblIssueDate" runat="server" Text="-" CssClass="issue_date"></asp:Label>

                </div>
            </div>


            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Print Policy</h3>

                </div>
                <div class="panel-body">

                    <asp:GridView ID="gvPolicy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False"  Width="100%" HorizontalAlign="Center" OnRowDataBound="gvPolicy_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("Policy_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("Gender" ) + "\");" %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Policy_Number" HeaderText="Policy no." SortExpression="Policy_Number" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Customer_ID" HeaderText="Customer ID" SortExpression="Customer_ID" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="App_Number" HeaderText="Application no." SortExpression="App_Number" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Last_Name" HeaderText="Last Name" SortExpression="Last_Name">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="First_Name" HeaderText="First name" SortExpression="First_Name">
                                <ItemStyle CssClass="GridRowCenter" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Age_Insure" HeaderText="Age" SortExpression="Age_Insure" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Agreement_Date" HeaderText="Signature date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Agreement_Date" ItemStyle-HorizontalAlign="Center" >
                             <ItemStyle HorizontalAlign="Center" />
                             </asp:BoundField>--%>
                            <asp:BoundField DataField="Product_ID" HeaderText="Product" SortExpression="Product_ID" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sum_Insure" HeaderText="Sum Insure" SortExpression="Sum_Insure" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Rounded_Amount" HeaderText="Annual Premium" SortExpression="Rounded_Amount" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Premium" HeaderText="Premium" SortExpression="Premium" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <%--Call function to get total premium = (system + extra premium) - discount--%>

                            <asp:TemplateField HeaderText="Extra Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# GetExtraPremium(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString(),DataBinder.Eval(Container.DataItem, "Policy_ID").ToString(), DataBinder.Eval(Container.DataItem, "Product_ID").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Discount">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# GetDiscount(DataBinder.Eval(Container.DataItem, "Policy_ID").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# GetTotalPremium(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString(), DataBinder.Eval(Container.DataItem, "Premium").ToString(), DataBinder.Eval(Container.DataItem, "Policy_ID").ToString(), DataBinder.Eval(Container.DataItem, "Product_ID").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>


                            <asp:BoundField DataField="Issue_Date" HeaderText="Issue Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Issue_Date" ItemStyle-HorizontalAlign="Center">

                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Effective_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Maturity_Date" HeaderText="Maturity Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Maturity_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <%--<asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAnswer" Text="Round Premium" runat="server"></asp:Label>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:RadioButtonList ID="rbtnlAnswer" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Up" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Down" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                                <HeaderStyle Width="10%" />
                                <ItemStyle Width="10%" HorizontalAlign="center" />
                            </asp:TemplateField>
                            --%>

                        </Columns>
                    </asp:GridView>

                    <%--<asp:SqlDataSource ID="AppDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age] FROM [Cv_Basic_App_Form] WHERE (App_Register_ID <> App_Number) AND ([Status_Code] = 'IF') AND [Is_Policy_Issued] = 0 ORDER BY [App_Number] ASC"></asp:SqlDataSource>

                    <asp:SqlDataSource ID="ReportDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age] FROM [Cv_Basic_App_Form] WHERE (App_Register_ID <> App_Number) AND ([Status_Code] = 'IF') AND [Is_Policy_Issued] = 0 ORDER BY [App_Number] ASC"></asp:SqlDataSource>--%>

                    <%--SELECT * FROM Cv_Basic_Policy WHERE Policy_Status_Type_ID = 'IF' ORDER BY Policy_Number DESC--%>

                    <asp:SqlDataSource ID="PolicyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT TOP 100 * FROM Cv_Basic_Policy WHERE Policy_Status_Type_ID = 'IF' ORDER BY Policy_Number DESC; SELECT * FROM V_PRINT_POLICY_FLAT_RATE_SCHEDULE;"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="ApproverPosition" runat="server" ConnectionString ="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand ="select distinct position_en from ct_report_approver" ></asp:SqlDataSource>


                </div>
            </div>

            <!-- Modal Result -->
            <div id="ResultModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalResult" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Note</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnCancelNote" runat="server" Text="Close" class="btn" data-dismiss="modal" aria-hidden="true" />
                    <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                </div>
            </div>

            
            <!-- Modal Approver -->
            <div id="Approver" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalResult" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Approver</h3>
                </div>
                <br />
                 <div style="padding:10px;">
                     <table>
                         <tr>
                             <td>Position</td>
                             <td>
                                 <asp:DropDownList ID="ddlPosition" runat="server"  DataSourceID="ApproverPosition" DataTextField="position_en" DataValueField="position_en" AppendDataBoundItems="true" onchange="select_position();">
                                    <asp:ListItem Text="Please Select" Value=""></asp:ListItem>

                    </asp:DropDownList>
                             </td>
                             
                         </tr>
                         <tr>
                             <td>Full Name</td>
                             <td>
                                 <asp:DropDownList ID="ddlFullName" runat="server"></asp:DropDownList>
                             </td>
                         </tr>
                     </table>
                    
                   <div id="div_approver_message" class=" text-error"></div>
                </div>
                <br />
                <div class="modal-footer">
                    <%--<asp:Button ID="btnOk" runat="server" Text="OK" class="btn btn-primary"   OnClientClick="save_approver();" />--%>
                    <input type="button" class="btn btn-primary" style="height: 27px;" onclick="save_approver();" value="OK" />
                    <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                </div>
            </div>

            <!--End Modal Select paper -->

            <!-- Modal Select paper -->
            <div id="paper_type" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="paper_type" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Select Paper Type</h3>
                </div>
                <br />
                <div style="text-align: center;">
                    <asp:DropDownList ID="ddlPaper_Type" runat="server" Width="90%" onchange="select_paper();">
                        <asp:ListItem Text="Normal Paper" Value=""></asp:ListItem>
                        <asp:ListItem Text="Letter Head" Value="LETTER_HEAD"></asp:ListItem>

                    </asp:DropDownList>
                    <div id="div_message" class="text-warning"></div>
                </div>

                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnPrint" runat="server" OnClientClick="CheckSelection_inform_letter();" Text="Print" class="btn btn-primary" data-dismiss="modal" aria-hidden="true" />
                    <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                </div>
            </div>
            <!--End Modal Select paper -->

             <!-- Modal Confirm Reprint -->
            <div id="reprint" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="reprint" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">System Alert!</h3>
                </div>
                <br />
                <div style="text-align: center;">
                    <div id="mgs_reprint" class=" text-info"></div>
                </div>

                <br />
                <div class="modal-footer">
                    <asp:Button ID="Button1" runat="server" OnClientClick="open_paper_type();" Text="Re-Print" class="btn btn-primary" data-dismiss="modal" aria-hidden="true" />
                    <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                </div>
            </div>
            <!--End Modal Select paper -->

            <!-- Modal Search Policy -->
            <div id="mySearchPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPolicyHeader" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Policy</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SAppNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Policy No</a></li>
                        <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Customer Name</a></li>                       
                    </ul>

                    <div class="tab-content" style="height: 60px; overflow: hidden;">
                        <div class="tab-pane active" id="SAppNo">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Policy No:</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNumberSearch" Width="95%" runat="server"></asp:TextBox>

                                    </td>
                                    

                                </tr>
                            </table>

                        </div>
                        <div class="tab-pane" style="height: 70px;" id="SCustomerName">
                            
                            <table style="width: 99%; padding-top: 4px;">
                                <tr>
                                    <td width="70px;">&nbsp;&nbsp;Surname:</td>
                                    <td>
                                        <asp:TextBox ID="txtSurnameSearch" Width="95%" runat="server"></asp:TextBox>

                                    </td>
                                   
                                </tr>
                                 <tr>
                                    <td width="70px;">&nbsp;&nbsp;Firstname:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstnameSearch" Width="95%" runat="server"></asp:TextBox>

                                    </td>
                                    

                                </tr>
                            </table>

                        </div>
                        

                    </div>
                   
                </div>
                <div class="modal-footer">
                    <%--<input type="button" class="btn btn-primary" style="height: 27px;" data-dismiss="modal" value="Search"  />--%>
                    <asp:Button ID="btnSearchPolicy" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Search" OnClick="btnSearchPolicy_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Search Policy-->


            <%--Hidden Fields--%>
            <asp:HiddenField ID="hdfPolicyID" runat="server" />
            <asp:HiddenField ID="hdfAge" runat="server" />
            <asp:HiddenField ID="hdfSumInsured" runat="server" />
            <asp:HiddenField ID="hdfAssureYear" runat="server" />
            <asp:HiddenField ID="hdfProductID" runat="server" />


        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearchPolicy" />

        </Triggers>

    </asp:UpdatePanel>


</asp:Content>

