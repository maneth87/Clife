<%@ Page Title="Clife | Business => Policy Issuance" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_issuance.aspx.cs" Inherits="Pages_Business_policy_issuance" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <%--<link href="../../App_Themes/data-checklist.css" rel="stylesheet" />--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script src="../../Scripts/date.format.js"></script>
    <script src="../../Scripts/print.js"></script>

    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImgBtnRefresh" runat="server" ImageUrl="~/App_Themes/functions/refresh.png" OnClick="ImgBtnRefresh_Click" />
        </li>

        <%--<li>
            <asp:ImageButton ID="ImageBtnSelectAll" runat="server" ImageUrl="~/App_Themes/functions/select_all.png" />
        </li>--%>

        <li>            
            <input type="button" onclick="CheckUW();" style="background: url('../../App_Themes/functions/print_data_checklist.png') no-repeat; border: none; height: 40px; width: 130px;" />
        </li>
        <li>
            <input type="button" onclick="CheckUndo()" style="background: url('../../App_Themes/functions/undo_underwrite.png') no-repeat; border: none; height: 40px; width: 90px;" />            
        </li>
        <li>
            <input type="button" onclick="CheckSelection();" style="background: url('../../App_Themes/functions/issue_policy.png') no-repeat; border: none; height: 40px; width: 100px;" />
            
           <%-- <asp:ImageButton ID="ImageBtnIssuePolicy" data-toggle="modal" data-target="#IssuePolicyModal" runat="server" ImageUrl="~/App_Themes/functions/issue_policy.png" />
        --%></li>


    </ul>

    <script type="text/javascript">

        //Fucntion to check only one checkbox and highlight textbox
        function SelectSingleCheckBox(ckb, app_register_id, product_id, system_sum_insured, system_premium, pay_mode, gender, age, status_code, pay_up_to_age, assure_up_to_age, pay_year, assure_year, effective_date) {
            var GridView = ckb.parentNode.parentNode.parentNode;


            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                    
                }
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#f5f5f5");

                //Bind value to Hidden Fields    
                $('#Main_hdfAppRegisterID').val(app_register_id);

                //Pass data all labels before print
                var app_register_id = $('#Main_hdfAppRegisterID').val();

                $('#Main_hdfEffectiveDate').val(effective_date);
                $('#Main_hdfProductID').val(product_id);

                $('#Main_hdfSystemPremium').val(system_premium);
               
                
                $.ajax({
                    type: "POST",
                    url: "../../UnderwritingWebService.asmx/GetBenefits",
                    data: "{app_id:'" + app_register_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var rows = $('#tblBenefiterTable tbody').children().length;

                        //Delete previous benefits
                        while (rows > 1) {
                            $('#tblBenefiterTable tbody tr:last').remove();
                            rows = $('#tblBenefiterTable tbody').children().length;
                        }

                        for (var i = 0; i < data.d.length; i++) {

                            var item = data.d[i];
                            var new_row = "";
                            new_row += "<tr>"
                            new_row += "<td class=\"khmer\">"
                            new_row += item.Full_Name
                            new_row += "</td> "
                            new_row += "<td class=\"khmer\">&nbsp;&nbsp;"
                            new_row += item.Relationship
                            new_row += "</td> "
                            new_row += "<td>&nbsp;&nbsp;"
                            new_row += item.Percentage + "%"
                            new_row += "</td> "
                            new_row += "</tr>"

                            $('#tblBenefiterTable').append(new_row);

                        }
                    }

                });

                $.ajax({
                    type: "POST",
                    url: "../../UnderwritingWebService.asmx/GetUWObject",
                    data: "{app_register_id:'" + app_register_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {                        
                        //pass data to hidden div for print
                        $('#Main_lblAppNo').text(data.d.App_Number);
                        $('#Main_lblKeyInBy').text(data.d.Created_By);
                        $('#Main_lblAgentName').text(data.d.Sale_Agent_Full_Name);
                        $('#Main_lblAgentCode').text(data.d.Sale_Agent_ID);

                        //------------------------------------------------------------
                        $('#Main_lblFullName').text(data.d.Last_Name + " " + data.d.First_Name);
                        $('#Main_lblIDNumber').text(data.d.ID_Card);
                        $('#Main_lblNationality').text(data.d.Nationality);
                        $('#Main_lblDateOfBirth').text(formatJSONDate(data.d.Birth_Date));

                        $('#Main_lblAge').text(data.d.Age_Insure);
                        $('#Main_lblGender').text(data.d.Gender);

                        $('#Main_lblFatherFullName').text(data.d.Father_Last_Name + " " + data.d.Father_First_Name);
                        $('#Main_lblMotherFullName').text(data.d.Mother_Last_Name + " " + data.d.Mother_First_Name);

                        $('#Main_lblAddress').text(data.d.Address1 + " " + data.d.Address2 + " " + data.d.Province);
                        $('#Main_lblPhoneNumber').text(data.d.Mobile_Phone1);
                        $('#Main_lblEmailAddress').text(data.d.EMail);

                        //------------------------------------------------------------
                        $('#Main_lblInsurancePlan').text(data.d.Kh_Title);
                        $('#Main_lblTermOfCoveragePeriod').text(data.d.Assure_Year);
                        $('#Main_lblTermOfPayment').text(data.d.Pay_Year);

                        $('#Main_lblSumInsured').text(formatCurrency(data.d.System_Sum_Insure));
                        $('#Main_lblModeOfPayment').text(data.d.Payment_Mode);

                        $('#Main_lblAnnualPremium').text(formatCurrency(data.d.Rounded_Amount));


                        $('#Main_lblAnnualPremium').text(formatCurrency(data.d.Rounded_Amount));
                        $('#Main_lblAnnualEM').text(formatCurrency(data.d.Extra_Premium));
                        $('#Main_lblTotalAnnualPremium').text(formatCurrency(data.d.Total_Yearly_Premium));


                        $('#Main_lblPremium').text(formatCurrency(data.d.System_Premium));
                        $('#Main_lblEM').text(formatCurrency(data.d.Extra_Amount));
                        $('#Main_lblDiscount').text(formatCurrency(data.d.User_Premium_Discount));
                        $('#Main_lblTotalPremium').text(formatCurrency(data.d.Total_Premium));
                        $('#Main_lblTPD').text(formatCurrency(data.d.TPD));

                        //------------------------------------------------------------
                        $('#Main_lblEffectiveDate').text(formatJSONDate(data.d.Effective_Date));                        
                        $('#Main_lblMaturityDate').text(formatJSONDate(data.d.Maturity_Date));
                        $('#Main_lblDateforPremiumPayment').text(data.d.Complete_Due_Day_Month);

                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });


            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");
            }
        }

        //Check whether user make any selection before printing       
        function CheckSelection() {

            var GridView = document.getElementById('<%= gvApplication.ClientID %>');

            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {                   
                    $('#IssuePolicyModal').modal('show');
                    return;
                }
            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }


        //Function to handle when user click undo underwrite button
        function CheckUW() {

            var GridView = document.getElementById('<%= gvApplication.ClientID %>');

            var ckbList = GridView.getElementsByTagName("input");

            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {

                    var pro_id = $('#Main_hdfProductID').val();
                    pro_id = pro_id.substring(0, 3);

                    if (pro_id == "NFP" || pro_id == "FPP" || pro_id == "SDS" || pro_id == "CL2") {
                        // alert("New family Protection" + $('#Main_hdfPolicyID').val());
                        window.open("Reports/data_check_life_RP_new.aspx?app_register_id=" + $('#Main_hdfAppRegisterID').val(), "_blank");
                        
                        return;
                    }
                    else {
                        PrintDetails();
                        return;
                    }

                    //ResultModal
                   // PrintDetails();
                   // return;
                }
            }

            $('#Main_txtNote').val("Please make your selection first!");
            $('#ResultModal').modal('show');

        }

        //undo underwriting
        function CheckUndo() {

            var GridView = document.getElementById('<%= gvApplication.ClientID %>');

             var ckbList = GridView.getElementsByTagName("input");

             for (i = 0; i < ckbList.length; i++) {
                 if (ckbList[i].type == "checkbox" && ckbList[i].checked == true) {
                     //ResultModal
                     $('#UndoUnderwriteModal').modal('show');
                     return;
                 }
             }

             $('#Main_txtNote').val("Please make your selection first!");
             $('#ResultModal').modal('show');
         }
        function open_uw_rider()
        {
            
            //window.open("Reports/data_check_life_RP_new.aspx?app_register_id=" + $('#Main_hdfAppRegisterID').val());
            window.location = "underwriting_rider.aspx?rid=" + $('#Main_hdfAppRegisterID').val();
        }
        function PrintDetails() {            
            //Open hidden div and print
            var printContent = document.getElementById('<%= printarea.ClientID %>');

            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, '');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.getElementById('print-content').style.display = 'block';
            printWindow.document.write('<link rel="stylesheet" href="../../App_Themes/data-checklist.css" type="text/css" />');

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

            return false;
        }
      
        //$('#print-content').toggle(300);

        function formatJSONDate(jsonDate) {
            var value = jsonDate;
            if (value.substring(0, 6) == "/Date(") {
                var dt = new Date(parseInt(value.substring(6, value.length - 2)));
                var en_month = "";

                //get month in word
                switch (dt.getMonth() + 1) {
                    case 1:
                        en_month = "January";
                        break;
                    case 2:
                        en_month = "February";
                        break;
                    case 3:
                        en_month = "March";
                        break;
                    case 4:
                        en_month = "April";
                        break;
                    case 5:
                        en_month = "May";
                        break;
                    case 6:
                        en_month = "June";
                        break;
                    case 7:
                        en_month = "July";
                        break;
                    case 8:
                        en_month = "August";
                        break;
                    case 9:
                        en_month = "September";
                        break;
                    case 10:
                        en_month = "October";
                        break;
                    case 11:
                        en_month = "November";
                        break;
                    default:
                        en_month = "December";
                        break;
                }

                var dtString = dt.getDate() + " " + en_month + ", " + dt.getFullYear();
                value = dtString;
            }

            return value;

        }


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

        //Display date picker
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });

    </script>

    <style>
        @page {
            size: 8.27in 11.69in;
            margin: .0in .0in .0in .0in;
            mso-header-margin: .0in;
            mso-footer-margin: .0in;
            mso-paper-source: 0;
        }
        body {
            /* this affects the margin on the content before sending to printer */
            margin: 0px;            
        }
        #print-content {
            
            font-size: 8pt;
        } 
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--Operation here--%>
            <br />
            <br />
            <br />
            <div id="printarea" runat="server">
                <div id="print-content" style="display: none; position: relative;">
                    <div align="Center">
                        <h1>CAMBODIA LIFE INSURANCE COMPANY PLC.</h1>
                        <h2 style="text-decoration: underline;">DATA CHECKING LIST</h2>
                    </div>
                    <table width="100%">
                        <tr>
                            <td width="80px">App. No:</td>
                            <td width="360px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblAppNo" CssClass="label-font" runat="server"></asp:Label>
                            </td>
                            <td width="120px">&nbsp;&nbsp;Keyed in by:</td>

                            <td width="340px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblKeyInBy" CssClass="label-font" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td width="70px">Agent:</td>
                            <td width="430px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblAgentName" CssClass="label-font" runat="server"></asp:Label>
                            </td>

                            <td width="60px">&nbsp;&nbsp;&nbsp;Code:</td>

                            <td width="340px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblAgentCode" CssClass="label-font" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr style="font-weight: 100;" />
                    <h2 style="font-weight: bold;">INSURED'S INFORMATION</h2>
                    <table width="100%">

                        <tr>
                            <td width="230px">Surname and Name:</td>
                            <td width="370px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblFullName" CssClass="khmer" runat="server"></asp:Label>
                            </td>

                            <td width="110px">&nbsp;&nbsp;&nbsp;ID No:</td>

                            <td width="130px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblIDNumber" CssClass="label-font" runat="server"></asp:Label>
                            </td>

                            <td width="100px">&nbsp;&nbsp;&nbsp;&nbsp;Nationality:</td>

                            <td width="60px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblNationality" CssClass="label-font" runat="server"></asp:Label>
                            </td>

                        </tr>
                    </table>
                    <table>

                        <tr>
                            <td width="180px">Date of Birth:</td>
                            <td width="420px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblDateOfBirth" runat="server"></asp:Label>
                            </td>

                            <td width="80px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Age:</td>

                            <td width="200px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblAge" runat="server"></asp:Label>
                            </td>

                            <td width="100px">&nbsp;&nbsp;&nbsp;&nbsp;Gender:
                        &nbsp;</td>

                            <td width="180px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblGender" CssClass="khmer" runat="server"></asp:Label></td>
                    </table>
                    <table width="100%">
                        <tr>
                            <td colspan="2" width="220px">Father&#39;s Surname and Name: </td>

                            <td colspan="4" width="480px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblFatherFullName" CssClass="khmer" runat="server"></asp:Label>
                            </td>

                        </tr>

                        <tr>
                            <td colspan="2">Mother&#39;s Surname and Name:</td>

                            <td colspan="4" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label CssClass="khmer" ID="lblMotherFullName" runat="server"></asp:Label>
                            </td>

                        </tr>

                    </table>
                    <table width="100%">
                        <tr>
                            <td width="60px">Address: </td>

                            <td width="720px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label CssClass="khmer" ID="lblAddress" runat="server"></asp:Label>
                            </td>

                        </tr>

                    </table>

                    <table width="100%">
                        <tr>
                            <td width="110px">Phone No.: </td>

                            <td width="250px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblPhoneNumber" runat="server"></asp:Label>
                            </td>

                            <td width="170px">&nbsp;&nbsp;&nbsp;Email Address: </td>

                            <td width="370px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
                            </td>

                        </tr>

                    </table>

                    <table width="100%">
                        <tr>
                            <td width="170px">Insurance Plan: </td>

                            <td width="320px" style="border-bottom: 1px dotted black;">&nbsp;<asp:Label CssClass="khmer" ID="lblInsurancePlan" runat="server"></asp:Label>
                            </td>

                            <td width="180px">&nbsp;Coverage Period: </td>

                            <td width="50px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblTermOfCoveragePeriod" runat="server"></asp:Label>
                            </td>

                            <td width="200px">&nbsp;Term of Payment: </td>
                            <td width="50px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblTermOfPayment" runat="server"></asp:Label>
                            </td>
                        </tr>

                    </table>

                    <table width="100%">
                        <tr>
                            <td width="140px">Sum Insured: </td>

                            <td width="330px" style="border-bottom: 1px dotted black; padding-left: 60px;">&nbsp;&nbsp;<asp:Label ID="lblSumInsured" runat="server"></asp:Label>
                            </td>

                            <td width="230px">&nbsp;&nbsp;&nbsp;Mode of Payment: </td>

                            <td width="340px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblModeOfPayment" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <table width="100%">
                        <tr>
                            <td width="180px">Annual Premium: </td>
                            <td width="140px" style="border-bottom: 1px dotted black; padding-left: 20px;">&nbsp;&nbsp;<asp:Label ID="lblAnnualPremium" runat="server"></asp:Label>
                            </td>

                            <td width="140px">&nbsp;&nbsp;Annual EM: </td>
                            <td width="100px" style="border-bottom: 1px dotted black; padding-left: 10px;">&nbsp;&nbsp;<asp:Label ID="lblAnnualEM" runat="server"></asp:Label>
                            </td>

                            <td width="280px">&nbsp;&nbsp;Total Annual Premium: </td>
                            <td width="120px" style="border-bottom: 1px dotted black; padding-left:10px;">&nbsp;&nbsp;<asp:Label ID="lblTotalAnnualPremium" runat="server"></asp:Label>
                            </td>
                            
                        </tr>
                    </table>

                    <table width="100%">
                        <tr>
                            <td width="160px">System Premium: </td>
                            <td width="180px" style="border-bottom: 1px dotted black; padding-left: 20px;">&nbsp;&nbsp;<asp:Label ID="lblPremium" runat="server"></asp:Label>
                            </td>

                            <td width="120px">&nbsp;&nbsp;System EM: </td>
                            <td width="140px" style="border-bottom: 1px dotted black; padding-left: 20px;">&nbsp;&nbsp;<asp:Label ID="lblEM" runat="server"></asp:Label>
                            </td>

                            <td width="100px">&nbsp;&nbsp;Discount: </td>
                            <td width="140px" style="border-bottom: 1px dotted black; padding-left: 20px;">&nbsp;&nbsp;<asp:Label ID="lblDiscount" runat="server"></asp:Label>
                            </td>
                            
                        </tr>
                    </table>

                    <table width="100%">
                        <tr>                                                      

                            <td width="120px">Total Premium: </td>
                            <td width="620px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotalPremium" runat="server"></asp:Label>
                            </td>
                        </tr>

                    </table>
                    
                    <%--When TPD is used, place this back--%>
                    
                    <%--<table width="100%">
                        <tr>
                            <td width="40px">TPD: </td>

                            <td width="720px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblTPD" runat="server"></asp:Label>
                            </td>

                        </tr>

                    </table>--%>

                    <table width="100%">
                        <tr>
                            <td width="140px">Effective Date: </td>

                            <td width="380px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblEffectiveDate" runat="server"></asp:Label>
                            </td>

                            <td width="160px">&nbsp;&nbsp;&nbsp;Maturity Date: </td>

                            <td width="380px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblMaturityDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <table width="100%">
                        <tr>
                            <td width="280px">Due Date for Premium Payment: </td>

                            <td width="520px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblDateforPremiumPayment" CssClass="khmer" runat="server"></asp:Label>
                            </td>

                        </tr>

                    </table>
                    <br />
                    <hr style="font-weight: 100;" />
                    <table id="tblBenefiterTable" width="100%">
                        <tr>
                            <td width="400px" style="border-bottom: 1px dotted black;">Beneficiary(ies)
                            </td>

                            <td width="300px" style="border-bottom: 1px dotted black; margin-left: 10px;">&nbsp;&nbsp;Relation
                            </td>
                            <td width="200px" style="border-bottom: 1px dotted black; margin-left: 10px;">&nbsp;&nbsp;Proportion </td>
                        </tr>
                    </table>
                    <br />
                    <hr style="font-weight: 100;" />
                    <br />
                    <table width="100%">
                        <tr>
                            <td width="100px">Printed by: </td>

                            <td width="260px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblPrintedBy" runat="server"></asp:Label>
                            </td>

                            <td width="70px">&nbsp;&nbsp;Checker: </td>

                            <td width="230px" style="border-bottom: 1px dotted black;">&nbsp;
                            </td>

                            <td width="70px">&nbsp;&nbsp;&nbsp;Editor: </td>
                            <td width="230px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;
                            </td>
                        </tr>

                    </table>

                    <table width="100%">
                        <tr>
                            <td width="130px">Date of Printing: </td>

                            <td width="720px" style="border-bottom: 1px dotted black;">&nbsp;&nbsp;<asp:Label ID="lblDateOfPrinting" runat="server"></asp:Label>
                            </td>

                        </tr>

                    </table>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Issue Policy</h3>
                   
                </div>
                <div class="panel-body">

                    <asp:GridView ID="gvApplication" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" DataSourceID="AppDatasource" Width="100%" HorizontalAlign="Center" OnRowDataBound="gvApplication_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("App_Register_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("System_Sum_Insure" ) + "\", \"" + Eval("System_Premium" ) + "\", \"" + Eval("Pay_Mode" ) + "\", \"" + Eval("Gender" ) + "\", \"" + Eval("Age_Insure" ) + "\", \"" + Eval("Status_Code" ) + "\", \"" + Eval("Pay_Up_To_Age" ) + "\", \"" + Eval("Assure_Up_To_Age" ) + "\", \"" + Eval("Pay_Year" ) + "\", \"" + Eval("Assure_Year" ) + "\", \"" + Eval("Effective_Date" ) + "\");" %>' />--%>
                                    <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("App_Register_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("System_Sum_Insure" ) + "\", \"" + Eval("System_Premium" ) + "\", \"" + Eval("Pay_Mode_ID" ) + "\", \"" + Eval("Gender" ) + "\", \"" + Eval("Age_Insure" ) + "\", \"" + Eval("Status_Code" ) + "\", \"" + Eval("Pay_Up_To_Age" ) + "\", \"" + Eval("Assure_Up_To_Age" ) + "\", \"" + Eval("Pay_Year" ) + "\", \"" + Eval("Assure_Year" ) + "\", \"" + Eval("Effective_Date" ) + "\");" %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="30" />
                                <ItemStyle Width="30" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="App_Number" HeaderText="Application no." SortExpression="App_Number" ItemStyle-HorizontalAlign="Center" >
                             <ItemStyle HorizontalAlign="Center" />
                             </asp:BoundField>
                            <asp:BoundField DataField="App_Date" HeaderText="Signature date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="App_Date" ItemStyle-HorizontalAlign="Center" >
                             <ItemStyle HorizontalAlign="Center" />
                             </asp:BoundField>
                            <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" SortExpression="Effective_Date">
                                 <ItemStyle HorizontalAlign="Center" />
                             </asp:BoundField>
                            <asp:BoundField DataField="Last_Name" HeaderText="Surname" SortExpression="Last_Name">
                                <ItemStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="First_Name" HeaderText="First name" SortExpression="First_Name">
                                <ItemStyle CssClass="GridRowLeft" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Age_Insure" HeaderText="Age" SortExpression="Age_Insure" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Country_ID" HeaderText="Nationality" SortExpression="Country_ID" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Product_ID" HeaderText="Product" SortExpression="Product_ID" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="System_Sum_Insure" HeaderText="Sum Insure" SortExpression="System_Sum_Insure" DataFormatString="{0:N0}"  ItemStyle-HorizontalAlign="Right" > 
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                             </asp:BoundField>
                            
                            <asp:BoundField DataField="Rounded_Amount" HeaderText="Annual Premium" SortExpression="Rounded_Amount" DataFormatString="{0:N0}"  ItemStyle-HorizontalAlign="Right" > 
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="System_Premium" HeaderText="System Premium" SortExpression="System_Premium" DataFormatString="{0:N0}"  ItemStyle-HorizontalAlign="Right" > 
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>

                             <asp:BoundField DataField="User_Premium" HeaderText="User Premium" SortExpression="User_Premium" DataFormatString="{0:N0}"  ItemStyle-HorizontalAlign="Right" > 
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="Extra Premium">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblExtraPremium" runat="server" Text='<%# GetExtraPremium(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString())%>'></asp:Label>--%>
                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EXTRA_PREMIUM").ToString()%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Discount">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblDiscount" runat="server" Text='<%# GetDiscount(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString())%>'></asp:Label>--%>
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DISCOUNT").ToString()%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Premium">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblTotalPremium" runat="server" Text='<%# GetTotalPremium(DataBinder.Eval(Container.DataItem, "App_Register_ID").ToString(), DataBinder.Eval(Container.DataItem, "System_Premium").ToString())%>'></asp:Label>--%>
                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "System_Premium").ToString()) + Convert.ToDouble(DataBinder.Eval(Container.DataItem, "EXTRA_PREMIUM").ToString()) - Convert.ToDouble(DataBinder.Eval(Container.DataItem, "DISCOUNT").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>

                           <%-- <asp:TemplateField HeaderText="Returned Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblReturnPremium" runat="server" Text='<%# GetReturnedPremium(DataBinder.Eval(Container.DataItem, "System_Premium").ToString(), DataBinder.Eval(Container.DataItem, "User_Premium").ToString())%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GridRowRight" HorizontalAlign="Right" />
                            </asp:TemplateField>--%>

                            <asp:BoundField DataField="Pay_Year" HeaderText="Payment" SortExpression="Pay_Year" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Payment Mode">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblPaymode" runat="server" Text='<%# GetPayMode(DataBinder.Eval(Container.DataItem, "Pay_Mode").ToString())%>'></asp:Label>--%>
                                    <asp:Label ID="lblPaymode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Pay_Mode").ToString()%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Assure_Year" HeaderText="Coverage" SortExpression="Assure_Year" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Sale_Agent_ID" HeaderText="Agent Code" SortExpression="Sale_Agent_ID" ItemStyle-HorizontalAlign="Center" />


                        </Columns>
                    </asp:GridView>

                    <%--<asp:SqlDataSource ID="AppDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number],​ [Khmer_First_Name], [Khmer_Last_Name], [Status_Code], [App_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [Rounded_Amount], [System_Sum_Insure], [System_Premium], [User_Premium], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age], [Effective_Date] FROM [Cv_Basic_App_Form] WHERE (App_Register_ID <> App_Number) AND ([Status_Code] = 'IF') AND [Is_Policy_Issued] = 0 ORDER BY [App_Date] DESC"></asp:SqlDataSource>--%>
                    <asp:SqlDataSource ID="AppDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="EXEC SP_GET_APP_TO_ISSUE_POLICY;"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="ReportDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT [App_Register_ID], [App_Number], [Status_Code], [App_Date], [Sale_Agent_ID], [Product_ID], [Age_Insure], [Pay_Year], [Pay_Mode], [Assure_Year], [System_Sum_Insure], [System_Premium], [First_Name], [Last_Name], [Gender], [Country_ID], [Pay_Up_To_Age], [Assure_Up_To_Age] FROM [Cv_Basic_App_Form] WHERE (App_Register_ID <> App_Number) AND ([Status_Code] = 'IF') AND [Is_Policy_Issued] = 0 ORDER BY [App_Number] ASC"></asp:SqlDataSource>


                    <br />
                </div>
            </div>

             <!-- Modal Result -->
            <div id="ResultModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalResult" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">System Alert</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNote" Width="88%" TextMode="MultiLine" Height="60" runat="server"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnCancelNote" runat="server" Text="Close" class="btn" data-dismiss="modal" aria-hidden="true" />
                    <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                </div>
            </div>
            <!--End Modal Result -->

            <!-- Modal Issue Policy -->
            <div id="IssuePolicyModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalIssue" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Confirmation</h3>
                </div>
                <br />
                
                <table style="width: 100%; text-align: left;">
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>Issued Date: &nbsp;
                                <asp:TextBox ID="txtIssueDate" Width="250" runat="server" CssClass="datepicker"></asp:TextBox>&nbsp;&nbsp;(DD-MM-YYYY)
                            </td>
                        </tr>
                        <br />
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>Split Policy: &nbsp; &nbsp;
                                <asp:CheckBox ID="ckb2" runat="server" /> &nbsp; Yes  &nbsp; (If yes split each policy by 5000 USD)
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnIssuePolicy" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Issue" OnClick="btnIssuePolicy_Click" />
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Issue Policy Modal -->

             <!-- Modal Undo Underwrite -->
            <div id="UndoUnderwriteModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalUndoUW" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">Confirmation</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Are you sure you want to undo underwrite this application?<br />
                <br />
                <div class="modal-footer">
                    <asp:Button ID="btnUndoUW"  class="btn btn-primary" Style="height: 27px;" runat="server" Text="Yes" OnClick="btnUndoUW_Click" />                    

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Undo Underwrite -->
            
            <%--Hidden Fields--%>
            <asp:HiddenField ID="hdfProductID" runat="server" />
            <asp:HiddenField ID="hdfGender" runat="server" />
            <asp:HiddenField ID="hdfAge" runat="server" />
            <asp:HiddenField ID="hdfStatusCode" runat="server" />
            <asp:HiddenField ID="hdfAppRegisterID" runat="server" />
            <asp:HiddenField ID="hdfPayYear" runat="server" />
            <asp:HiddenField ID="hdfPayUpToAge" runat="server" />
            <asp:HiddenField ID="hdfAssureYear" runat="server" />
            <asp:HiddenField ID="hdfAssureUpToAge" runat="server" />
            <asp:HiddenField ID="hdfSumInsure" runat="server" />
            <asp:HiddenField ID="hdfSystemPremium" runat="server" />
            <asp:HiddenField ID="hdfPayMode" runat="server" />
            <asp:HiddenField ID="hdfEffectiveDate" runat="server" />

        
               <!-- Modal Message -->
            <div id="ModalMessage" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="ModalMessage" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="panel-title">System Message Alert!</h3>
                </div>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtMessage" Width="88%" TextMode="MultiLine" Height="60" runat="server" style="color:red; font-weight:normal;"></asp:TextBox>&nbsp;&nbsp;<br />
                <br />
                
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton runat="server" ID="lbtnUWRider" Text="Click here to do underwriting." OnClientClick="open_uw_rider();" CssClass="btn-link"></asp:LinkButton>

                <div class="modal-footer">
                    <%--<button class="btn btn-primary" data-dismiss="modal"  aria-hidden="true" onclick="open_uw_rider();">Ok</button>--%>
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
            <!--End Modal Message -->
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnIssuePolicy" />                        
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

