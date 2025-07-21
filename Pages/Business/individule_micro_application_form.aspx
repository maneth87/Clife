<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="individule_micro_application_form.aspx.cs" Inherits="Pages_Business_MiroIndviduleApp_individule_micro_application_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.7.1.min.js"></script>
    <%-- Search in dropdownlist --%>
    <script src="../../Scripts/jquery.1.8.3.jquery.min.js"></script>
    <script src="../../Scripts/jquery.searchabledropdown-1.0.8.min.js"></script>
    <%-- Jtemplate --%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <style>
        .GridPager a, .GridPager span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }

        .th_address {
            text-align: center;
            border: 1pt solid #d5d5d5;
            background-color: #21275b;
            color: white;
        }

        .my_progress {
            width: 100%;
            height: 100%;
            color: #FFFFFF;
            position: absolute;
            float: left;
            overflow: hidden;
            top: 0px;
            left: 0px;
        }

            .my_progress div.tr {
                background-color: black;
                -moz-opacity: 0.7;
                opacity: 0.7;
                filter: alpha(opacity=70);
                position: absolute;
                top: 0px;
                left: 0px;
                z-index: 998;
                width: 100%;
                height: 100%;
            }

            .my_progress div.main {
                position: relative;
                top: 30%;
                width: 400px;
                z-index: 999;
                margin: auto;
                border: 2px solid #2b0557;
                border-radius: 5px;
                -moz-border-radius: 5px;
                -webkit-border-radius: 5px;
                -khtml-border-radius: 5px;
                -moz-box-shadow: 0 0 50px #fff;
                -webkit-box-shadow: 0 0 50px #fff;
                text-align: center;
                background-color: White;
                color: Black;
            }

                .my_progress div.main div.dhead {
                    text-align: left;
                    background-image: url(../images/top_nav_bg.png);
                }

        #tblHolder tr td:nth-child(1) {
            vertical-align: middle;
            text-align: right;
        }

        #tblIssue {
            width: auto;
            margin-top: 10px;
        }

            #tblIssue td input[type="text"] {
                width: 120px;
                text-align: center;
            }

            #tblIssue td input[type="button"] {
                margin-left: 200px;
            }

 
    </style>

    <script>
        function pageLoad(sender, args) {
            $(document).ready(function () {
                $("#<%=ddlProvinceKh.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });

                $("#<%=ddlDistrictKh.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });

                $("#<%=ddlCommuneKh.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });

                $("#<%=ddlVillageKh.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });


                $('#Main_txtBasicDiscount').on('keypress', function (e) {
                    var charCode = e.which;
                    var value = $(this).val();

                    // Allow digits (0–9)
                    if (charCode >= 48 && charCode <= 57) {
                        return;
                    }

                    // Allow one dot (.)
                    if (charCode === 46 && value.indexOf('.') === -1) {
                        return;
                    }

                    // Block all other characters
                    e.preventDefault();
                });

                $('#txtRiderDiscount').on('keypress', function (e) {
                    var charCode = e.which;
                    var value = $(this).val();

                    // Allow digits (0–9)
                    if (charCode >= 48 && charCode <= 57) {
                        return;
                    }

                    // Allow one dot (.)
                    if (charCode === 46 && value.indexOf('.') === -1) {
                        return;
                    }

                    // Block all other characters
                    e.preventDefault();
                });


                $('#btnConfirmYes').click(function () {
                    var polId = $('#<%=hdfPolicyID.ClientID%>').val();
                      confirmAction(polId, 'Y');
                  });
                $('#btnConfirmNo').click(function () {
                    var polId = $('#<%=hdfPolicyID.ClientID%>').val();
                    confirmAction(polId, 'N');
                });

            });

        }

        var marketing_code;
        var marketing_name;
        var total_agent_row;

        //Search Marketing Code (Search click)
        function SearchMarketingCode() {
            var sale_agent_name = $('#Main_txtAgentName').val();

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetSaleAgents",
                data: "{sale_agent_name:" + JSON.stringify(sale_agent_name) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_hdfTotalAgentRow').val(data.d.length);

                    $('#dvAgentList').setTemplate($("#jTemplateSaleAgent").html());
                    $('#dvAgentList').processTemplate(data);

                }

            });

        }

        //Search Marketing Code (Search click)
        function SearchApplication() {
            var appNumber = $('#Main_txtSearchApplicationNumber').val();
            var cusName = $('#Main_txtSearchCustomerName').val();
            var cusGender = $('#Main_ddlSearchCustomerGender').val();
            var cusDob = $('#Main_txtSearchCustomerDOB').val();
            var agentCode = $('#Main_txtSearchAgentCode').val();
            var agentName = $('#Main_txtSearchAgentName').val();

            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/SearchApplication",
                data: "{applicationNumber:" + JSON.stringify(appNumber) + ',customerName:' + JSON.stringify(cusName) + ',customerGender:' + JSON.stringify(cusGender) + ',customerDOB:' + JSON.stringify(cusDob) + ',agentName:' + JSON.stringify(agentName) + ',agentCode:' + JSON.stringify(agentName) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_hdfTotalApplicationRow').val(data.d.length);
                    $('#dvSearchApplication').setTemplate($("#jTemplateSearchApplication").html());
                    $('#dvSearchApplication').processTemplate(data);

                }
            });

        }

        //Get Selected Sale Agent

        function GetAgent(AgentID, AgentName, row_index_agent_list) {

            if ($('#' + row_index_agent_list).is(':checked')) {
                marketing_code = AgentID;
                marketing_name = AgentName;


            } else {
                marketing_code = "";
                marketing_name = "";
            }

            var total_agent_row = $('#Main_hdfTotalAgentRow').val();
            // alert(total_agent_row);
            //Uncheck all other checkboxes
            for (var i = 1; i <= total_agent_row  ; i++) {
                if (i != row_index_agent_list) {
                    $('#' + i).prop('checked', false);
                }

            }

        };


        function GetApplicationID(applicationID, index) {

            if ($('#' + index).is(':checked')) {
                appID = applicationID;
                //$('#Main_hdfApplicationID').val(applicationID);
                $('#Main_hdfApplicationNumber').val(applicationID);

            } else {
                appID = "";
            }

            var total_app_row = $('#Main_hdfTotalApplicationRow').val();

            //Uncheck all other checkboxes
            for (var i = 1; i <= total_app_row  ; i++) {
                if (i != index) {
                    $('#' + i).prop('checked', false);
                }

            }

        };
        function Search() {

            $('#<%=btnSearch.ClientID%>').click();
        }

        function FillMarketingCode() {

            if (marketing_code == "") {
                alert('Please select a checkbox');
                return;
            }

            $('#Main_txtSaleAgentID').val(marketing_code);
            $('#Main_txtSaleAgentName').val(marketing_name);
            $('#Main_hdfSaleAgentID').val(marketing_code);
            $('#Main_hdfSaleAgentName').val(marketing_name);
        };

        function Reload() {
            location.reload();
        }
        function SearchReferralId() {
            var referralInfo = $('#<%=txtReferralIdSearch.ClientID  %>').val();
            var channelItemId = $('#<%=ddlCompany.ClientID%>').val();
            var channelLocationId = $('#<%=ddlBranch.ClientID%>').val();

            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/SearchReferral",
                data: "{channelItemId:" + JSON.stringify(channelItemId) + ", channelLocationId:" + JSON.stringify(channelLocationId) + ", referralInfo:" + JSON.stringify(referralInfo) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // $('#Main_hdfTotalAgentRow').val(data.d.length);
                    totalReferralRow = data.d.length;

                    $('#dvReferer').setTemplate($("#jTemplateReferral").html());
                    $('#dvReferer').processTemplate(data);
                }
            });

        }
        var referralId;
        var referralName;
        var totalReferralRow;
        function GetReferral(referralStaffId, referralStaffName, index) {

            if ($('#' + index).is(':checked')) {
                referralId = referralStaffId;
                referralName = referralStaffName;

            } else {
                referralId = "";
                referralName = "";

            }

            //Uncheck all other checkboxes
            for (var i = 1; i <= totalReferralRow  ; i++) {
                if (i != index) {
                    $('#' + i).prop('checked', false);
                }

            }

        };
        function FillReferral() {

            if (referralId == "") {
                alert('Please select a checkbox');
                return;
            }

            $('#<%=txtReferrerId.ClientID%>').val(referralId);
            $('#<%=txtReferrerName.ClientID%>').val(referralName);

        };

      

        function confirmAction(policyId, attachedPolicyInsurance) {
            $.ajax({
                type: "POST",
                url: "micro_application_form_edit.aspx/ConfirmAction",
                data: JSON.stringify({ policyId: policyId, attachedPolicyInsurance: attachedPolicyInsurance }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var certUrl = response.d;
                    window.open(certUrl, "_blank"); // opens the certificate page
                },
                error: function (xhr) {
                    console.error("Error:", xhr.responseText);
                    alert("An error occurred.");
                }
            });
        }
    </script>
    <%--Jtemplate Sections--%>
    <script id="jTemplateSaleAgent" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">Sale Agent ID</th>
                    <th style="border-width: thin; border-style: solid;">Sale Agent Name</th>
                    <th style="border-width: thin; border-style: solid;"></th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as record}
                    <tr>
                        <td>{ $T.record.Sale_Agent_ID }</td>
                        <td>{ $T.record.Full_Name }</td>
                        <td>
                            <input id="{ $T.record.row_index }" type="checkbox" onclick="GetAgent('{ $T.record.Sale_Agent_ID }', '{ $T.record.Full_Name }', '{ $T.record.row_index }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>

    <%--Jtemplate search application--%>
    <script id="jTemplateSearchApplication" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid; display: none;">Application ID</th>
                    <th style="border-width: thin; border-style: solid;">Application No.</th>
                    <th style="border-width: thin; border-style: solid;">Customer Name</th>
                    <th style="border-width: thin; border-style: solid;"></th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as record}
                    <tr>
                        <td style="display: none;">{ $T.record.ApplicationID }</td>
                        <td>{ $T.record.ApplicationNumber }</td>
                        <td>{ $T.record.CustomerLastName  + ' ' + $T.record.CustomerFirstName}</td>
                        <td>
                            <input id="{ $T.record.No }" type="checkbox" onclick="GetApplicationID('{ $T.record.ApplicationNumber }', '{$T.record.No }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>
    <script id="jTemplateReferral" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">Referral ID</th>
                    <th style="border-width: thin; border-style: solid;">Referral Name</th>
                    <th style="border-width: thin; border-style: solid;"></th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as record}
                    <tr>
                        <td>{ $T.record.ReferralStaffId }</td>
                        <td>{ $T.record.ReferralStaffName }</td>
                        <td>
                            <input id="Checkbox1" type="checkbox" onclick="GetReferral('{ $T.record.ReferralStaffId }', '{ $T.record.ReferralStaffName }', '{ $T.record.No }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>
    <asp:UpdatePanel ID="UP" runat="server">
        <ContentTemplate>
            <%-- <script type="text/javascript">
                Sys.Application.add_load(LoadScript);
            </script>--%>
            <ul class="toolbar">

                <li>
                    <asp:ImageButton ID="ibtnPrintApplication" runat="server" ImageUrl="~/App_Themes/functions/print_application.png" OnClick="ibtnPrintApplication_Click" />
                </li>
                <li>
                    <asp:ImageButton ID="ibtnPrintCertificate" runat="server" ImageUrl="~/App_Themes/functions/print_certificate.png" OnClick="ibtnPrintCertificate_Click" style="display:none;" />
                     <input type="button" id="btnPrintCert" runat="server"  data-toggle="modal" data-target="#modalConfirmPrinting" style="background: url('../../App_Themes/functions/print_certificate.png') no-repeat; border: none; width:130px;" />
                </li>
                <li>
                    <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="ibtnSave_Click" />
                </li>
                <li>
                    <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" data-toggle="modal" data-target="#searchApplication" />
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Style="display: none;" />
                </li>
                <li>
                    <asp:ImageButton ID="ibtnClear" runat="server" ImageUrl="~/App_Themes/functions/clear.png" OnClientClick="Reload();" />

                </li>
            </ul>

            <h1>Application Form</h1>
            <div id="dvApplicationInfo" runat="server">
                <table id="tblApplicationInfo" class="table-layout">
                    <tr>
                        <td colspan="8" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Application Details</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblClientType" runat="server" Text="Client Type:" class="span2"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:DropDownList ID="ddlClientType" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlClientType_SelectedIndexChanged">
                                <asp:ListItem Text="SELF" Value="SELF"></asp:ListItem>
                                <asp:ListItem Text="CLIENT_FAMILY" Value="CLIENT_FAMILY"></asp:ListItem>
                                <asp:ListItem Text="BANK_STAFF" Value="BANK_STAFF"></asp:ListItem>
                                <asp:ListItem Text="BANK_STAFF_FAMILY" Value="BANK_STAFF_FAMILY"></asp:ListItem>
                                <asp:ListItem Text="REPAYMENT" Value="REPAYMENT"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblApplicationNumber" runat="server" Text="Application Number:" Width="130px"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtApplicationNumber" runat="server" class="span2" placeholder="Auto Generate"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:DropDownList ID="ddlChannel" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged">
                                <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                <asp:ListItem Value="0025E613-4A0D-43E5-B6B8-BDE9A4A005EE" Text="Individual"></asp:ListItem>
                                <asp:ListItem Value="0152DF80-BA95-46A9-BB7A-E71966A34089" Text="Corporate"></asp:ListItem>
                            </asp:DropDownList>

                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCompany" runat="server" Text="Company:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:DropDownList ID="ddlCompany" runat="server" class="span3" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblBranch" runat="server" Text="Branch Code:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:DropDownList ID="ddlBranch" runat="server" class="span2" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblSaleAgentName" runat="server" Text="Sale Agent Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSaleAgentName" runat="server" class="span2"></asp:TextBox>
                            <span style="color: red;">*</span>
                            <input type="button" data-toggle="modal" data-target="#myMarketingCodeList" style="background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblSageAgentID" runat="server" Text="Sale Agent ID:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtSaleAgentID" runat="server" class="span2"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblApplicationDate" runat="server" Text="Application Date:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtApplicationDate" runat="server" placeholder="DD-MM-YYYY" class=" datepicker span2" AutoPostBack="true" OnTextChanged="txtApplicationDate_TextChanged"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblPolicyNumber" runat="server" Text="Certificate No.:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtPolicyNumber" runat="server" class="span2" placeholder="Auto Generate"></asp:TextBox>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblPolicyStatus" runat="server" Text="PolicyStatus:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtPolicyStatus" runat="server" class="span2"></asp:TextBox>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblLoanNumber" runat="server" Text="Loan Number:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtLoanNumber" runat="server" class="span2" OnTextChanged="txtLoanNumber_TextChanged" AutoPostBack="true"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblBankStaffName" runat="server" Text="Bank Staff Name:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtBankStaffName" runat="server" class="span2 font-khmer"></asp:TextBox>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblClientTypeRelation" runat="server" Text="Relation:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:DropDownList ID="ddlClientTypeRelation" runat="server" class="span2 font-khmer">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right; margin-top: 30px;">
                            <asp:Label ID="lblReferrerId" runat="server" Text="Referral Id:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtReferrerId" runat="server" class="span2"></asp:TextBox>
                            <input type="button" data-toggle="modal" data-target="#myReferralList" style="background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblReferrerName" runat="server" Text="Referral Name:"></asp:Label>
                        </td>
                        <td style="padding-right: 20px;">
                            <asp:TextBox ID="txtReferrerName" runat="server" class="span2"></asp:TextBox>

                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvCustomerInfo">
                <table id="tblCustomerInfo" class="table-layout">
                    <tr>
                        <td colspan="8" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Customer Information</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblIDType" Text="ID Type:" CssClass="span2"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlIDType" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlIDType_SelectedIndexChanged">
                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                <asp:ListItem Value="0">ID Card</asp:ListItem>
                                <asp:ListItem Value="1">Passport</asp:ListItem>
                                <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                <asp:ListItem Value="4">Family Book</asp:ListItem>
                                <asp:ListItem Value="5">Proof of Resident</asp:ListItem>
                                <asp:ListItem Value="6">Certificate of Identity</asp:ListItem>
                            </asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblIDNo" Text="ID No.:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIDNumber" class="span2" runat="server" AutoPostBack="true" OnTextChanged="txtIDNumber_TextChanged"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblSurnameInKhmer" Text="Surname In Khmer:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurnameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblFirstNameInKhmer" Text="First Name In Khmer:" Style="margin-left: 20px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstNameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblSurnameInEnglish" Text="Surname In English:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurnameEng" class="span2" runat="server" MaxLength="30"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblFirstInEnglish" Text="First Name In English:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstNameEng" class="span2" runat="server" MaxLength="30"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblNationality" Text="Nationality:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNationality" class="span2 font-khmer" Height="30px" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblGender" Text="Gender:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGender" class="span2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGender_SelectedIndexChanged">
                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="0">Female</asp:ListItem>
                            </asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right">

                            <asp:Label runat="server" ID="lblDateOfBirth" Text="Date of Birth (DD-MM-YYYY):"></asp:Label>
                        </td>
                        <td>

                            <asp:TextBox ID="txtDateOfBirth" placeHolder="DD-MM-YYYY" runat="server" CssClass="span2 datepicker" AutoPostBack="true" OnTextChanged="txtDateOfBirth_TextChanged"></asp:TextBox>

                            <span style="color: red;">*</span>

                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblCustomerAge" Text="Age:"></asp:Label>

                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerAge" class="span2" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>

                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblMaritalStatus" Text="Marital Status:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMaritalStatus" class="span2" runat="server">
                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                <asp:ListItem Value="Married">Married</asp:ListItem>
                                <asp:ListItem Value="Single">Single</asp:ListItem>
                                <asp:ListItem Value="Windower">Windower</asp:ListItem>
                                <asp:ListItem Value="Window">Window</asp:ListItem>
                            </asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblOccupation" Text="Occupation:"></asp:Label>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOccupation" runat="server" class="span2"></asp:DropDownList>
                            <span style="color: red;">*</span>

                        </td>
                    </tr>

                    <tr>

                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblPhoneNumber" Text="PhoneNumber:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhoneNumber" class="span2" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblEmail" Text="Email:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" class="span2" runat="server"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td class="th_address">
                            <asp:Label ID="lblAddress" class="span1" runat="server" Text="Address"></asp:Label>
                        </td>
                        <td class="th_address">
                            <asp:Label ID="lblHouseNo" runat="server" Text="House No."></asp:Label>
                        </td>
                        <td class="th_address">
                            <asp:Label ID="lblStreet" runat="server" Text="Street"></asp:Label>
                        </td>

                        <td class="th_address">
                            <asp:Label ID="lblVillage" runat="server" Text="Village"></asp:Label>
                        </td>
                        <td class="th_address">
                            <asp:Label ID="lblCommune" runat="server" Text="Commune"></asp:Label>
                        </td>
                        <td class="th_address">
                            <asp:Label ID="lblDistrict" runat="server" Text="District"></asp:Label>
                        </td>
                        <td class="th_address">
                            <asp:Label ID="lblProvince" runat="server" Text="Province"></asp:Label>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:Label runat="server" ID="lblAddressInKhmer" Text="Khmer:"></asp:Label>
                        </td>

                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtHouseNoKh" OnTextChanged="txtHouseNoKh_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="ផ្ទះលេខ"></asp:TextBox>
                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtStreetKh" OnTextChanged="txtStreetKh_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="ផ្លូវលេខ"></asp:TextBox>
                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:DropDownList ID="ddlVillageKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlVillageKh_SelectedIndexChanged">
                                <asp:ListItem Value="" Text="--ភូមិ--"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:DropDownList ID="ddlCommuneKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlCommuneKh_SelectedIndexChanged">
                                <asp:ListItem Value="" Text="--ឃុំ/សង្កាត់--"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:DropDownList ID="ddlDistrictKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrictKh_SelectedIndexChanged">
                                <asp:ListItem Value="" Text="--ស្រុក/ខណ្ឌ--"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:DropDownList ID="ddlProvinceKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlProvinceKh_SelectedIndexChanged1">
                                <asp:ListItem Value="" Text="--ខេត្ត/ក្រុង--"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:Label runat="server" ID="lblAddressInEnglish" Text="English:"></asp:Label>
                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtHouseNoEn" class="span2" runat="server" Height="25px" placeholder="House No."></asp:TextBox>
                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtStreetEn" AutoPostBack="false" class="span2" runat="server" Height="25px" placeholder="Street No."></asp:TextBox>
                        </td>

                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtVillageEn" class="span2" runat="server" Height="25px" placeholder="Village"></asp:TextBox>

                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtCommuneEn" class="span2" runat="server" Height="25px" placeholder="Commune"></asp:TextBox>
                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtDistrictEn" class="span2" runat="server" Height="25px" placeholder="Distict"></asp:TextBox>

                        </td>
                        <td style="text-align: center; border: 1pt solid #d5d5d5;">
                            <asp:TextBox ID="txtProvinceEn" class="span2" runat="server" Height="25px" placeholder="Province"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvProductDetail">
                <table class="table-layout" style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td colspan="6" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Insurance Product and Detail</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblPackage" Text="Select Product:"></asp:Label>

                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlProduct" runat="server" class="span4" Style="text-align: center;" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value=""> </asp:ListItem>

                            </asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblProductName" Text="Product Name:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtProductName" class="span5" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblTermCover" Text="Term of Cover:"></asp:Label>

                        </td>
                        <td>
                            <asp:DropdownList ID="ddlCoverType"  class="span1" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCoverType_SelectedIndexChanged"></asp:DropdownList>
                            <asp:DropdownList ID="ddlTermOfCover" class="span1" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlTermOfCover_SelectedIndexChanged"></asp:DropdownList>
                            
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblSumAssure" Text="Sum Assure (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSumAssure" class="span2" runat="server" AutoPostBack="true" OnTextChanged="txtSumAssure_TextChanged"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblPaymentMode" Text="Pay Mode:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPaymentMode" CssClass="span2" runat="server" Style="text-align: center;" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged"></asp:DropDownList>

                            <span style="color: red;">*</span>
                            <asp:Label runat="server" ID="lblPremiumPaymentPeriod" Visible="false" Text="Payment Period:"></asp:Label>
                             <asp:TextBox ID="txtPremiumPaymentPeriod" class="span2" Visible="false" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblPremium" Text="Premium (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPremium" class="span2" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblBasicDiscount" Text="Discount Amount (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBasicDiscount" class="span2" runat="server" AutoPostBack="true" OnTextChanged="txtBasicDiscount_TextChanged"></asp:TextBox>

                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblBasicAfterDiscount" Text="Premium After Discount (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBasicAfterDiscount" class="span2" runat="server"></asp:TextBox>
                            <asp:Label runat="server" ID="lblAnnualPremium" Text="Annual Premium (USD):"></asp:Label>
                            <asp:TextBox ID="txtAnnualPremium" class="span2" runat="server"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTotalSumAssure" runat="server">Total Sum Assure (USD):</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalSumAssure" CssClass="span2" runat="server" AutoPostBack="true" OnTextChanged="txtTotalSumAssure_TextChanged"></asp:TextBox>
                             <span style="color: red;">*</span>
                        </td>
                        <td>
                            <asp:Label ID="lblNumberOfApplication" runat="server"> Number of Application:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNumberofApplication" runat="server" AppendDataBoundItems="true" CssClass="span2">
                                <asp:ListItem Value="0" Text="---Select---"></asp:ListItem>
                            </asp:DropDownList></td>

                        <td style="text-align:right;">
                            <asp:Label ID="Label3" runat="server"> Number of Year:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNumberofYear" runat="server" AppendDataBoundItems="true" CssClass="span2">
                                <asp:ListItem Value="0" Text="---Select---"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                    </tr>
                </table>

            </div>
            <div id="dvRider" runat="server">
                <table class="table-layout" style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td colspan="8" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Rider</h3>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRiderProduct" Text="Rider Product:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRiderProduct" class="span2" runat="server" Style="text-align: center;" AutoPostBack="true" OnSelectedIndexChanged="ddlRiderProduct_SelectedIndexChanged" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="---Select---"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRiderProductname" Text="Product Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRiderProductName" class="span2" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRiderSumAssure" Text="Sum Assure (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRiderSumAssure" class="span2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRiderSumAssure_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRiderPremium" Text="Rider Premium (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRiderPremium" class="span2" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRiderDiscount" Text="Discount Amount (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRiderDiscount" class="span2" runat="server" AutoPostBack="true" OnTextChanged="txtRiderDiscount_TextChanged"></asp:TextBox>

                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRiderAfterDiscount" Text="Premium After Discount (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRiderAfterDiscount" class="span2" runat="server"></asp:TextBox>

                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblRiderAnnualPremium" Text="Rider Annual Premium:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRiderAnnualPremium" class="span2" runat="server"></asp:TextBox>

                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvTotalPremium">
                <table class="table-layout" style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td colspan="8" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Total Premium</h3>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblTotalPremium" Text="Total Amount (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPremium" runat="server" class="span2" Style="font-weight: bold;"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblDiscountAmount" Text="Discount Amount (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalDiscountAmount" class="span2" runat="server"></asp:TextBox>

                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblTotalPremiumAfterDiscount" Text="Premium After Discount (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPremiumAfterDiscount" class="span2" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblPaymentCode" Text="Payment Code."></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaymentCode" class="span2" runat="server" Style="text-align: center;"></asp:TextBox>
                           
                        </td>
                    </tr>

                </table>

            </div>
            <div id="dvPolicyHolder" runat="server">
                <table id="tblPolicyHolder" class="table-layout">
                    <tr id="rowHeaderPolicyHolder" runat="server">
                        <td colspan="6" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Policy Holder</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblHolderName" runat="server">Name:</asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtHolderName" runat="server" Style="height: 30px;" CssClass="span8 font-khmer" ReadOnly="true"></asp:TextBox>
                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lblHolderDOB" runat="server">DOB:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHolderDOB" placeholder="DD/MM/YYYY" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblHolderGender" CssClass="span2" runat="server">Gender:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlHolderGender" CssClass="span2" ReadOnly="true">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="0">Female</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblHolderIdType" CssClass="span2" runat="server">ID Type:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlHolderIdType" CssClass="span3" ReadOnly="true">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="0">ID Card</asp:ListItem>
                                <asp:ListItem Value="1">Passport</asp:ListItem>
                                <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                <asp:ListItem Value="4">Family Book</asp:ListItem>
                                <asp:ListItem Value="5">Proof of Resident</asp:ListItem>
                                <asp:ListItem Value="6">Certificate of Identity</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lblHolderIdNo" CssClass="span2" runat="server">ID NO.:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHolderIdNo" runat="server" CssClass="span2" ReadOnly="true"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblHolderAddress" runat="server">Address:</asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtHolderAddress" runat="server" Width="98%" CssClass="font-khmer" Height="30px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvPrimaryBeneficiary" runat="server">
                <table class="table-layout" id="tblPrimaryBeneficiary" style="width: 100%; margin-top: 10px;">
                    <tr id="rowHeaderPrimaryBeneficiary" runat="server">
                        <td colspan="6" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Primary Beneficiary</h3>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblPrimaryBenName" Text="Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrimaryBenName" class="span4 font-khmer" Height="30px" runat="server" ReadOnly="true"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblPrimaryBenLoan" Text="Loan Number:"></asp:Label>
                        </td>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txtPrimaryBenLoan" class="span2" runat="server" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblPrimaryBenAddress" Text="Address"></asp:Label>
                        </td>

                        <td>
                            <asp:TextBox ID="txtPrimaryBenAddress" class="span6 font-khmer" runat="server" Style="text-align: center; height: 30px;" ReadOnly="true"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>

                    </tr>

                </table>

            </div>
            <div id="dvBeneficiary" runat="server">
                <table class="table-layout" id="tblBeneficiary" style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td colspan="8" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;" id="benTitle" runat="server">Beneficiary(s)</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblBenFullName" Text="Full Name:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtFullName" class="span4 font-khmer" Style="height: 30px;" runat="server"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblBenDOB" Text="DOB:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtBenDOB" placeHolder="DD/MM/YYYY" class="span2" runat="server"></asp:TextBox>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblBenGender" runat="server">Gender</asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlBenGender" CssClass="span2" OnSelectedIndexChanged="ddlBenGender_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="0">Female</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblBenIDType" CssClass="span2" runat="server">ID Type</asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlBenIdType" CssClass="span2">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="0">ID Card</asp:ListItem>
                                <asp:ListItem Value="1">Passport</asp:ListItem>
                                <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                <asp:ListItem Value="4">Family Book</asp:ListItem>
                                <asp:ListItem Value="5">Proof of Resident</asp:ListItem>
                                <asp:ListItem Value="6">Certificate of Identity</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblBenIDNo" Text="ID No."></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtBenIdNo" class="span2" runat="server" Style="text-align: center;"></asp:TextBox>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblBenAge" Text="Age"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtAge" class="span2" runat="server" Style="text-align: center;"></asp:TextBox>

                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblBenRelation" Text="Relation"></asp:Label>
                        </td>
                        <td style="text-align: left;">

                            <asp:DropDownList runat="server" ID="ddlRelation" class="span2 font-khmer"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblBenPercentageOfShare" Text="Percentage of Share(%):"></asp:Label>

                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtPercentageOfShare" class="span2" runat="server" Style="text-align: center;"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="lblBenAddress" Text="Address"></asp:Label>
                        </td>
                        <td colspan="7" style="text-align: left;">
                            <asp:TextBox ID="txtBenAddress" class="span8 font-khmer" runat="server" Style="height: 30px;" ReadOnly="true"></asp:TextBox>
                            <asp:Button ID="tblAddBen" runat="server" Text="ADD" CssClass="btn btn-primary" Style="margin-left: 30px;" OnClick="tblAddBen_Click" />
                        </td>

                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:GridView ID="gvBen" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowEditing="gvBen_RowEditing" OnRowDeleting="gvBen_RowDeleting" PageSize="10" AlternatingRowStyle-BackColor="#C2D69B">
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true" ItemStyle-Width="10px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BenID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBenId" Width="80px" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Name" ItemStyle-Width="150px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFullName" Width="150px" runat="server" Text='<%# Eval("FULL_NAME")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Age" Visible="true" ItemStyle-Width="10px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAge" Width="10px" runat="server" Text='<%#Eval("AGE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="DOB" Visible="true" ItemStyle-Width="80px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDOB" Width="80px" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).Year==1900 ? "" : Convert.ToDateTime( Eval("DOB")).ToString("dd-MM-yyyy") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIdType" Width="60px" runat="server" Text='<%#Eval("IdTypeString") %>'></asp:Label>
                                            <asp:HiddenField ID="hdfIdType" runat="server" Value='<%#Eval("IdType") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ID No." ItemStyle-Width="20px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIdNo" Width="100%" runat="server" Text='<%# Eval("IdNo").ToString()  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gender" ItemStyle-Width="10px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGender" Width="10px" runat="server" Text='<%# Eval("GenderString")  %>'></asp:Label>
                                            <asp:HiddenField ID="hdfGender" runat="server" Value='<%# Eval("Gender")  %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" Width="20px" runat="server" Text='<%# Eval("ADDRESS")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relation" ItemStyle-Width="100px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRelation" Width="100px" runat="server" CssClass="font-khmer" Text='<%# Eval("RELATION")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="%" ItemStyle-Width="20px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPercentage" Width="20px" runat="server" Text='<%# Eval("PERCENTAGE_OF_SHARE")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px" HeaderStyle-CssClass="gridView-header">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="~/App_Themes/functions/edit.png" CommandName="edit" />
                                            <asp:ImageButton ID="ibtDelete" runat="server" ImageUrl="~/App_Themes/functions/delete.png" CommandName="delete" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvHealthQuestion">
                <table class="table-layout" style="width: 100%;">
                    <tr>
                        <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Health Question of the Assured</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 70%;">
                            <asp:Label runat="server" ID="lblQuestionkh" Style="font-family: 'Khmer Kampot'" Text="១. តើអ្នកត្រូវបានធានារ៉ាប់រងកំពុងស្ថិតក្នុងលក្ខខណ្ឌណាមួយដែលមានស្រាប់មុនពេលបណ្ណសន្យារ៉ាប់រងមានសុពលភាពហើយត្រូវបានកំណត់ដូចជា៖ជំងឺមហារីក ជំងឺទឹកនោមផ្អែម ជំងឺបេះដូង ជំងឺថ្លើម ជំងឺដាច់សរសៃឈាមក្នុងខួរក្បាល ជំងឺសួត ជំងឺតំរងនោម ជំងឺរ៉ាំរ៉ៃ ជំងឺផ្សេងៗដែលកំពុងកើតមានឡើងឬកំពុងព្យាបាលមិនទាន់ជាសះស្បើយ ឬរបួសស្នាមរ៉ាំរ៉ៃដែរឬទេ? ប្រសិនបើប្រែប្រួលសូមបញ្ជាក់មូលហេតុ។"></asp:Label><br />
                            <asp:Label runat="server" ID="lblQuestion" Text="1. Has the Life Assured is in the  conditions which existed before effective date of insurance policy and defined as: Cancer, diabetes, heart disease, liver disease, stroke, lung disease, kidney disease or chronic injuries? If yes, please state the reason.:"></asp:Label>
                        </td>
                        <td style="border-left: 1px solid #21275b; padding-left: 20px;">
                            <asp:DropDownList ID="ddlAnswer" runat="server" class="span2">
                                <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="color: red;">*</span><br />
                            <asp:TextBox ID="txtAnswerRemarks" class="span5" placeholder="Please give details" runat="server" TextMode="MultiLine"></asp:TextBox>
                            <asp:HiddenField ID="hdfQuestionID" runat="server" Value="6D44FA76-6B00-42D6-A4D7-ACD85986DC7C" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvIssuePolicy">
                <table id="tblIssue" class="table-layout" style="width: 100%;">
                    <tr>
                        <td colspan="6" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                            <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Issue Policy</h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div id="dvApplicationToIssue" runat="server" style="border:2px solid #21275b; margin:10px 10px 10px 10px; text-align:center; padding:10px 10px 10px 10px;background-color:#dff0d8;">
                                <div id="dvAppList" runat="server" style="text-align:left;  margin:0 auto;  width:350px;" ></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblIssueDate" Text="Issue Date:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIssueDate" class="span3" runat="server" placeholder="DD-MM-YYYY" AutoPostBack="true" OnTextChanged="txtIssueDate_TextChanged"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="Label1" Text="Effective Date:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEffectiveDate" class="span3" runat="server" placeholder="DD-MM-YYYY" AutoPostBack="true" OnTextChanged="txtEffectiveDate_TextChanged" ></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="Label2" Text="Pay Date:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaydate" class="span3" runat="server" placeholder="DD-MM-YYYY" AutoPostBack="true" OnTextChanged="txtPaydate_TextChanged"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblUserPremium" Text="Collected Premium (USD):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserPremium" class="span3" runat="server" Style="text-align: center;"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right">
                            <asp:Label runat="server" ID="lblPaymentRefNo" Text="Payment Ref. No.:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaymentRefNo" class="span3" runat="server" Style="text-align: center;"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td style="text-align: right"></td>
                        <td>
                            <asp:Button runat="server" ID="btnIssue" class="btn btn-primary" Text="Issue Policy" OnClick="btnIssue_Click" OnClientClick="return confirm('Do you want to process issue policy?');" />

                        </td>
                    </tr>

                </table>

            </div>

            <asp:HiddenField ID="hdfChannelID" runat="server" Value="" />
            <asp:HiddenField ID="hdfChannelItemID" runat="server" Value="" />
            <asp:HiddenField ID="hdfChannelLocationID" runat="server" Value="" />
            <asp:HiddenField ID="hdfSaleAgentID" runat="server" Value="" />
            <asp:HiddenField ID="hdfSaleAgentName" runat="server" Value="" />
            <asp:HiddenField ID="hdfProductID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyNumber" runat="server" Value="" />
            <asp:HiddenField ID="hdfRiderProductID" runat="server" Value="" />
            <asp:HiddenField ID="hdfRiderProductIDOld" runat="server" Value="" />
            <asp:HiddenField ID="hdfApplicationNumber" runat="server" Value="" />
            <asp:HiddenField ID="hdfApplicationID" runat="server" Value="" />
            <asp:HiddenField ID="hdfApplicationCustomerID" runat="server" Value="" />
            <asp:HiddenField ID="hdfExistCustomerID" runat="server" Value="" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyDetailID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyPaymentID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyBenID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyRiderID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyAddressID" runat="server" Value="" />
            <asp:HiddenField ID="hdfTotalAgentRow" runat="server" Value="" />
            <asp:HiddenField ID="hdfTotalApplicationRow" runat="server" Value="" />
            <asp:HiddenField ID="hdfBeneficiaryId" runat="server" Value="" />
            <asp:Label ID="lblError" runat="server"></asp:Label>
                        <div id="myReferralList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelMarketingCodeHeader" data-keyboard="false" data-backdrop="static">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" >×</button>
                    <h3 id="H3">Select Referral</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->

                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td style="width: 25%; vertical-align: middle">Referral Id/Name:</td>
                            <td style="width: 57%; vertical-align: bottom">
                                <asp:TextBox ID="txtReferralIdSearch" Width="90%" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 18%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchReferralId();" value="Search" />
                            </td>

                        </tr>

                    </table>
                    <hr />
                    <div id="dvReferer"></div>

                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillReferral();" data-dismiss="modal" value="OK" />

                    <button class="btn" data-dismiss="modal" >Cancel</button>
                </div>
            </div>

             <!---Popup share Referral--->
                <div id="modalConfirmPrinting" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalConfirmPrinting"  data-keyboard="false" data-backdrop="static">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" >×</button>
                        <h3 id="H4">CONFIRM!</h3>
                    </div>
                    <div class="modal-body">
                        <!---Modal Body--->

                        <div id="dvConfirmContent">Do you wish to include insurance policy with the Certificate Printing?</div>

                    </div>
                    <div class="modal-footer" style="text-align:center;">
                       <%-- <button class="btn btn-primary" id="btnConfirmYes"  >Yes, include policy</button>--%>
                        <input type="button" class="btn btn-primary" id="btnConfirmYes" value="Yes, include policy" />
                        <input type="button"class="btn btn-success" id="btnConfirmNo" value="No, just the certificate" />
                        <button class="btn btn-warning " data-dismiss="modal" >CANCEL</button>
                    </div>
                </div>

            <!-- Modal Marketing Code -->
            <div id="myMarketingCodeList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelMarketingCodeHeader"  data-keyboard="false" data-backdrop="static">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" >×</button>
                    <h3 id="H1">Select Marketing Code</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->

                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td style="width: 25%; vertical-align: middle">Agent Code/Name:</td>
                            <td style="width: 57%; vertical-align: bottom">
                                <asp:TextBox ID="txtAgentName" Width="90%" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 18%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchMarketingCode();" value="Search" />
                            </td>

                        </tr>

                    </table>
                    <hr />
                    <div id="dvAgentList"></div>


                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillMarketingCode();" data-dismiss="modal" value="OK" />

                    <button class="btn" data-dismiss="modal" >Cancel</button>
                </div>
            </div>
            <!--End ModalMarketing Code-->

            <!-- Modal Application -->
            <div id="searchApplication" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelMarketingCodeHeader"  data-keyboard="false" data-backdrop="static">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" >×</button>
                    <h3 id="H2">Select Application</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->

                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td style="width: 25%; vertical-align: middle">Application Number:</td>
                            <td style="width: 57%; vertical-align: bottom">
                                <asp:TextBox ID="txtSearchApplicationNumber" Width="90%" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 18%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchApplication();" value="Search" />
                            </td>

                        </tr>
                        <tr>

                            <td style="width: 25%; vertical-align: middle">Agent Code:</td>

                            <td>
                                <asp:TextBox ID="txtSearchAgentCode" Width="90%" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>

                            <td style="width: 25%; vertical-align: middle">Agent Name:</td>

                            <td>
                                <asp:TextBox ID="txtSearchAgentName" Width="90%" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>

                            <td style="width: 25%; vertical-align: middle">Customer Name:</td>

                            <td>
                                <asp:TextBox ID="txtSearchCustomerName" Width="90%" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>

                            <td style="width: 25%; vertical-align: middle">Customer Gender:</td>

                            <td>
                                <asp:DropDownList ID="ddlSearchCustomerGender" runat="server">
                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Female" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                </asp:DropDownList>

                            </td>
                            <td></td>
                        </tr>
                        <tr>

                            <td style="width: 25%; vertical-align: middle">Customer DOB:</td>

                            <td>
                                <asp:TextBox ID="txtSearchCustomerDOB" Width="90%" runat="server" placeholder="DD-MM-YYYY"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <hr />
                    <div id="dvSearchApplication"></div>


                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-primary" style="height: 27px;" onclick="Search();" data-dismiss="modal" value="OK" />

                    <button class="btn" data-dismiss="modal" >Cancel</button>
                </div>
            </div>
            <!--End Application-->
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="upg" runat="server" AssociatedUpdatePanelID="UP">
        <ProgressTemplate>
            <div class="my_progress">
                <div class="tr"></div>
                <div class="main">
                    <div class="dhead">
                        <h2>PROCESSING</h2>
                    </div>
                    <p>
                        <img id="loader" src="../../App_Themes/images/loader.gif" alt="Progressing" />
                    </p>
                    <p>Please wait...</p>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
