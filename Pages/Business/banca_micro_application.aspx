<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="banca_micro_application.aspx.cs" Inherits="Pages_Business_banca_micro_application" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />

    <%-- Search in dropdownlist --%>
    <script src="../../Scripts/jquery.1.8.3.jquery.min.js"></script>
    <script src="../../Scripts/jquery.searchabledropdown-1.0.8.min.js"></script>
    <%-- Jtemplate --%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
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
            });

        }
        function SearchReferralId() {
            var referralInfo = $('#<%=txtReferralIdSearch.ClientID  %>').val();
            var channelItemId = $('#<%=hdfChannelItemID.ClientID%>').val();
            var channelLocationId = $('#<%=hdfChannelLocationID.ClientID%>').val();

            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/SearchReferral",
                data: "{channelItemId:" + JSON.stringify(channelItemId) + ", channelLocationId:" + JSON.stringify(channelLocationId) + ", referralInfo:" + JSON.stringify(referralInfo) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // $('#Main_hdfTotalAgentRow').val(data.d.length);
                    totalReferralRow = data.d.length;

                    $('#dvAgentList').setTemplate($("#jTemplateReferral").html());
                    $('#dvAgentList').processTemplate(data);
                }
            });

        }

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
                            <input id="{ $T.record.No }" type="checkbox" onclick="GetReferral('{ $T.record.ReferralStaffId }', '{ $T.record.ReferralStaffName }', '{ $T.record.No }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>
    <asp:UpdatePanel ID="UP" runat="server">
        <ContentTemplate>
            <ul class="toolbar">

                <li>
                    <asp:ImageButton ID="ibtnPrintApplication" runat="server" ImageUrl="~/App_Themes/functions/print_application.png" OnClick="ibtnPrintApplication_Click" />
                </li>
                <li>
                    <asp:ImageButton ID="ibtnPrintCertificate" runat="server" ImageUrl="~/App_Themes/functions/print_certificate.png" OnClick="ibtnPrintCertificate_Click" />
                </li>
                <li>
                    <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="ibtnSave_Click" />
                </li>
            </ul>

            <h1>Application Form</h1>
            <table id="tblAppContent" class="table-layout">
                <tr>
                    <td style="width: 35%; vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                        <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Application Details</h3>

                    </td>
                    <td style="width: 75%; vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                        <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Customer Information</h3>

                    </td>

                </tr>
                <tr>
                    <td style="width: 35%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                        <table style="margin-top: 15px; margin-bottom: 10px; margin-left: 10px;">
                             <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblClientType" runat="server" Text="Client Type:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownlist ID="ddlClientType" runat="server" class="span3" AutoPostBack="true" OnSelectedIndexChanged="ddlClientType_SelectedIndexChanged">
                                        <asp:ListItem Text="SELF" Value="SELF"></asp:ListItem>
                                        <asp:ListItem Text="CLIENT_FAMILY" Value="CLIENT_FAMILY"></asp:ListItem>
                                        <asp:ListItem Text="BANK_STAFF" Value="BANK_STAFF"></asp:ListItem>
                                        <asp:ListItem Text="BANK_STAFF_FAMILY" Value="BANK_STAFF_FAMILY"></asp:ListItem>
                                         <asp:ListItem Text="REPAYMENT" Value="REPAYMENT"></asp:ListItem>
                                    </asp:DropDownlist>
                                    <asp:HiddenField id="hdfOldClientType" runat="server"/>
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblBankStaffName" runat="server" Text="Bank Staff Name:"></asp:Label>
                                </td>
                                <td  style="padding-right: 20px;">
                                    <asp:TextBox ID="txtBankStaffName" runat="server" ></asp:TextBox>
                                      <asp:HiddenField id="hdfOldBankStaffName" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblClientTypeRelation" runat="server" Text="Relation:"></asp:Label>
                                </td>
                                <td  style="padding-right: 20px;">
                                    <asp:DropDownlist ID="ddlClientTypeRelation" runat="server" class="span3" >
                                       
                                    </asp:DropDownlist>
                                      <asp:HiddenField id="hdfOldClientTypeRelation" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblReletedCertificate" runat="server" Text="Releted Certificate No.:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtReletedCertificate" runat="server" class="span3" Width="150px"></asp:TextBox>
                                    <asp:Label ID="lblApplicationType" runat="server" ForeColor="Red"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblApplicationNumber" runat="server" Text="Application Number:" Width="130px"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtApplicationNumber" runat="server" class="span3" placeholder="Auto Generate"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblBranchName" runat="server" Text="Branch Name:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtBranchName" runat="server" class="span3"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblBranchcode" runat="server" Text="Branch Code:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtBranchCode" runat="server" class="span3"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblSageAgentID" runat="server" Text="Sale Agent ID:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtSaleAgentID" runat="server" class="span3"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblSaleAgentName" runat="server" Text="Sale Agent Name:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSaleAgentName" runat="server" class="span3"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblApplicationDate" runat="server" Text="Application Date:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtApplicationDate" runat="server" placeholder="DD-MM-YYYY" class=" datepicker span3" AutoPostBack="true" OnTextChanged="txtApplicationDate_TextChanged"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                    <asp:HiddenField ID="hdfOldApplicationDate" runat="server" Value="" />
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblPolicyNumber" runat="server" Text="Certificate No.:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtPolicyNumber" runat="server" class="span3" placeholder="Auto Generate"></asp:TextBox>

                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblPolicyStatus" runat="server" Text="PolicyStatus:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtPolicyStatus" runat="server" class="span3"></asp:TextBox>

                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right; margin-top: 30px;">
                                    <asp:Label ID="lblReferrerId" runat="server" Text="Referral Id:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtReferrerId" runat="server" class="span3"></asp:TextBox>
                                    <asp:HiddenField ID="hdfOldReferrerId" runat="server" Value="" />
                                    <input type="button" data-toggle="modal" data-target="#myReferralList" style="background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblReferrerName" runat="server" Text="Referral Name:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtReferrerName" runat="server" class="span3"></asp:TextBox>

                                </td>

                            </tr>
                              <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblLoanNumber" runat="server" Text="Loan Number:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtLoanNumber" runat="server" class="span3"></asp:TextBox>

                                </td>

                            </tr>
                        </table>

                    </td>
                    <td style="width: 75%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                        <table style="margin-top: 15px; margin-bottom: 10px;">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblIDType" Text="ID Type:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlIDType" runat="server" class="span3">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="0">ID Card</asp:ListItem>
                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                        <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                        <asp:ListItem Value="4">Family Book</asp:ListItem>
                                        <asp:ListItem Value="5">Proof of Resident</asp:ListItem>
                                        <asp:ListItem Value="6">Certificate of Identity</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfOldIdType" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblIDNo" Text="ID No.:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIDNumber" class="span3" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfOldIdNumber" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblSurnameInKhmer" Text="Surname In Khmer:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSurnameKh" class="span3" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                    <asp:HiddenField ID="hdfSurnameKh" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblFirstNameInKhmer" Text="First Name In Khmer:" Style="margin-left: 20px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFirstNameKh" class="span3" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                    <asp:HiddenField ID="hdfFirstnameKh" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblSurnameInEnglish" Text="Surname In English:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSurnameEng" class="span3" runat="server" MaxLength="30"></asp:TextBox>
                                    <asp:HiddenField ID="hdfSurnameEng" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblFirstInEnglish" Text="First Name In English:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFirstNameEng" class="span3" runat="server" MaxLength="30"></asp:TextBox>
                                    <asp:HiddenField ID="hdfFirstNameEng" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblNationality" Text="Nationality:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNationality" class="span3" runat="server" MaxLength="30"></asp:TextBox>
                                    <asp:HiddenField ID="hdfNationlity" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblGender" Text="Gender:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGender" class="span3" runat="server">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Male</asp:ListItem>
                                        <asp:ListItem Value="0">Female</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfGender" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">

                                    <asp:Label runat="server" ID="lblDateOfBirth" Text="Date of Birth (DD-MM-YYYY):"></asp:Label>
                                </td>
                                <td>

                                    <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="span3 datepicker" AutoPostBack="true" OnTextChanged="txtDateOfBirth_TextChanged"></asp:TextBox>
                                    <asp:HiddenField ID="hdfDateOfBirth" runat="server" Value="" />
                                    <span style="color: red;">*</span>

                                </td>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblCustomerAge" Text="Age:"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerAge" class="span3" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfCustomerAge" runat="server" Value="" />
                                    <span style="color: red;">*</span>

                                </td>
                            </tr>

                            <tr>

                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblMaritalStatus" Text="Marital Status:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMaritalStatus" class="span3" runat="server">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="Married">Married</asp:ListItem>
                                        <asp:ListItem Value="Single">Single</asp:ListItem>
                                        <asp:ListItem Value="Widower">Widower</asp:ListItem>
                                        <asp:ListItem Value="Widow">Widow</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfMaritalStatus" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblOccupation" Text="Occupation:"></asp:Label>

                                </td>
                                <td>
                                    <%-- <asp:TextBox ID="txtOccupation" class="span3" runat="server"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddlOccupation" runat="server" class="span3"></asp:DropDownList>
                                    <asp:HiddenField ID="hdfOccupation" runat="server" Value="" />
                                    <span style="color: red;">*</span>

                                </td>
                            </tr>
                            <tr>

                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblPhoneNumber" Text="PhoneNumber:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhoneNumber" class="span3" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfPhoneNumber" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblEmail" Text="Email:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" class="span3" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfEmail" runat="server" Value="" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table style="border: 1pt solid #d5d5d5;">
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
                                                <asp:HiddenField ID="hdfHouseNoKh" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:TextBox ID="txtStreetKh" OnTextChanged="txtStreetKh_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="ផ្លូវលេខ"></asp:TextBox>
                                                <asp:HiddenField ID="hdfStreetKh" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:DropDownList ID="ddlVillageKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlVillageKh_SelectedIndexChanged">
                                                    <asp:ListItem Value="" Text="--ភូមិ--"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfVillagekh" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:DropDownList ID="ddlCommuneKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlCommuneKh_SelectedIndexChanged">
                                                    <asp:ListItem Value="" Text="--ឃុំ/សង្កាត់--"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfCommuneKh" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:DropDownList ID="ddlDistrictKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrictKh_SelectedIndexChanged">
                                                    <asp:ListItem Value="" Text="--ស្រុក/ខណ្ឌ--"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfDistrictKh" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <%--                                    <asp:TextBox ID="txtProvinceKh" class="span2" runat="server" Height="25px" OnTextChanged="txtProvinceKh_TextChanged" AutoPostBack="true" placeholder="ខេត្ត/ក្រុង"></asp:TextBox>--%>
                                                <asp:DropDownList ID="ddlProvinceKh" runat="server" class="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlProvinceKh_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:HiddenField ID="hdfProvinceKh" runat="server" Value="" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:Label runat="server" ID="lblAddressInEnglish" Text="English:"></asp:Label>
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:TextBox ID="txtHouseNoEn" OnTextChanged="txtHouseNoEn_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="House No."></asp:TextBox>
                                                <asp:HiddenField ID="hdfHouseNoEn" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:TextBox ID="txtStreetEn" OnTextChanged="txtStreetEn_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="Street No."></asp:TextBox>
                                                <asp:HiddenField ID="hdfStreetEn" runat="server" Value="" />
                                            </td>

                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:TextBox ID="txtVillageEn" OnTextChanged="txtVillageEn_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="Village"></asp:TextBox>
                                                <asp:HiddenField ID="hdfVillageEn" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:TextBox ID="txtCommuneEn" OnTextChanged="txtCommuneEn_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="Commune"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCommuneEn" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:TextBox ID="txtDistrictEn" OnTextChanged="txtDistrictEn_TextChanged" AutoPostBack="true" class="span2" runat="server" Height="25px" placeholder="Distict"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDistrictEn" runat="server" Value="" />
                                            </td>
                                            <td style="text-align: center; border: 1pt solid #d5d5d5;">
                                                <asp:TextBox ID="txtProvinceEn" class="span2" runat="server" Height="25px" OnTextChanged="txtProvinceEn_TextChanged" AutoPostBack="true" placeholder="Province"></asp:TextBox>
                                                <asp:HiddenField ID="hdfProvinceEn" runat="server" Value="" />
                                            </td>
                                        </tr>

                                    </table>

                                </td>
                            </tr>


                        </table>
                    </td>
                </tr>
                <%--Insurance product and detail--%>
                <tr>
                    <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                        <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Insurance Product and Detail</h3>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                        <table style="width: auto; margin-top: 10px;">
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblPackage" Text="Package:"></asp:Label>

                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPackage" runat="server" class="span2" Style="text-align: center;" AutoPostBack="true" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged">
                                        <%--   <asp:ListItem Text="--Select--" Value="0"> </asp:ListItem>
                                        <asp:ListItem Text="Package1" Value="2000"> </asp:ListItem>
                                        <asp:ListItem Text="Package2" Value="5000"> </asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfPackage" runat="server" Value="" />
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblProductName" Text="Product Name:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProductName" class="span3" runat="server"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblTermCover" Text="Term of Cover (Year):"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtTermOfCover" class="span2" runat="server"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblPremiumPaymentPeriod" Text="Payment Period:"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtPremiumPaymentPeriod" class="span2" runat="server"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>


                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblSumAssure" Text="Sum Assure (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBasicSa" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlBasicSa_SelectedIndexChanged" CssClass="span2">
                                        <asp:ListItem Text="--- Select ---" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblPaymentMode" Text="Pay Mode:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPaymentMode" runat="server" Style="text-align: center;" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged"></asp:DropDownList>

                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblPremium" Text="Premium (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPremium" class="span2" runat="server"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblBasicDiscount" Text="Discount Amount (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBasicDiscount" class="span2" runat="server"></asp:TextBox>

                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblBasicAfterDiscount" Text="Premium After Discount (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBasicAfterDiscount" class="span2" runat="server"></asp:TextBox>

                                </td>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblAnnualPremium" Text="Annual Premium (USD):"></asp:Label>

                                </td>
                                <td>
                                    <asp:TextBox ID="txtAnnualPremium" class="span3" runat="server"></asp:TextBox>
                                    <%--  <span style="color:red;">*</span>--%>
                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--Rider--%>
                <tr>
                    <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                        <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Rider</h3>
                    </td>

                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                        <table style="width: auto; margin-top: 10px;">
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblRiderProduct" Text="Rider Product:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRiderProduct" class="span3" runat="server"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblRiderSumAssure" Text="Sum Assure (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRiderSa" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRiderSa_SelectedIndexChanged" CssClass="span2"></asp:DropDownList>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblRiderPremium" Text="Rider Premium (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRiderPremium" class="span2" runat="server"></asp:TextBox>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
                                    <asp:Label runat="server" ID="lblRiderDiscount" Text="Discount Amount (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRiderDiscount" class="span2" runat="server"></asp:TextBox>

                                </td>
                                <td style="text-align: right">
                                     <span class="span05"></span>
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
                                    <%--<span style="color:red;">*</span>--%>
                                </td>
                                <td></td>
                                <td>
                            </tr>

                        </table>
                    </td>

                </tr>

                <%--TotalPremium--%>
                <tr>
                    <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                        <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Total Premium</h3>
                    </td>

                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                        <table style="width:auto; margin-top: 10px;">
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblTotalPremium" Text="Total Amount (USD):"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalPremium" runat="server" class="span2" Style="font-weight: bolder;"></asp:TextBox>
                                    <span style="color: red;">*</span> </td>
                    </td>
                    <td style="text-align: right">
                        <span class="span05"></span>
                        <asp:Label runat="server" ID="lblTotalDiscountAmount" Text="Total Discount Amount (USD):"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalDiscountAmount" class="span2" runat="server"></asp:TextBox>

                    </td>
                    <td style="text-align: right">
                         <span class="span05"></span>
                        <asp:Label runat="server" ID="lblTotalPremiumAfterDiscount" Text="Total Amount After Discount (USD):"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalPremiumAfterDiscount" class="span2" runat="server" Style="font-weight: bolder;"></asp:TextBox>
                        <span style="color: red;">*</span>
                    </td>

                </tr>

            </table>
            </td>

            </tr>

            <%--Beneficiary--%>
            <tr>
                <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                    <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Beneficiary(s)</h3>
                </td>


            </tr>
            <tr>
                <td colspan="2" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                    <table style="width: auto; margin-top: 10px;">
                        <tr>
                            <td style="text-align: right">
                                <asp:Label runat="server" ID="lblBenFullName" Text="Full Name:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFullName" class="span3" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hdfFullName" runat="server" Value="" />
                                <span style="color: red;">*</span>
                            </td>
                            <td style="text-align: right;">
                                <span class="span05"></span>
                                <asp:Label runat="server" ID="lblBenAge" Text="Age:"></asp:Label>
                            </td>
                            <td style="text-align: center;">
                                <asp:TextBox ID="txtAge" class="span2" runat="server" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdfAge" runat="server" Value="" />
                                <span style="color: red;">*</span>
                            </td>
                            <td style="text-align: right">
                                 <span class="span05"></span>
                                <asp:Label runat="server" ID="lblBenRelation" Text="Relation:"></asp:Label>
                            </td>
                            <td>
                                <%-- <asp:TextBox ID="txtRelation" class="span3" runat="server" Style="text-align: center;"></asp:TextBox>--%>
                                <asp:DropDownList runat="server" ID="ddlRelation" class="span3"></asp:DropDownList>
                                <asp:HiddenField ID="hdfRelation" runat="server" Value="" />
                                <span style="color: red;">*</span>
                            </td>
                            <td style="text-align: right">
                                 <span class="span05"></span>
                                <asp:Label runat="server" ID="lblBenPercentageOfShare" Text="Percentage of Share(%):"></asp:Label>

                            </td>
                            <td>
                                <asp:TextBox ID="txtPercentageOfShare" class="span2" runat="server" Style="text-align: center;"></asp:TextBox>
                                <span style="color: red;">*</span>
                            </td>


                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label runat="server" ID="lblBenAddress" Text="Address:"></asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtBenAddress" class="span7" runat="server" Height="25px"></asp:TextBox>
                                <asp:HiddenField ID="hdfBenAddress" runat="server" Value="" />
                                <span style="color: red;">*</span>
                            </td>

                            <td style="text-align: right">&nbsp;</td>

                        </tr>
                    </table>
                </td>
            </tr>

            <%--Question--%>
            <tr>
                <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                    <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Health Question of the Assured</h3>
                </td>


            </tr>
            <tr>
                <td colspan="2" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: left; width: 70%;">
                                <asp:Label runat="server" ID="lblQuestionkh" Style="font-family: 'Khmer OS Content'; font-size: 9.8pt;" Text="១. តើអ្នកត្រូវបានធានារ៉ាប់រងកំពុងស្ថិតក្នុងលក្ខខណ្ឌណាមួយដែលមានស្រាប់មុនពេលបណ្ណសន្យារ៉ាប់រងមានសុពលភាពហើយត្រូវបានកំណត់ដូចជា៖ ជំងឺមហារីក ជំងឺទឹកនោមផ្អែម ជំងឺបេះដូង ជំងឺថ្លើម ជំងឺដាច់សរសៃឈាមក្នុងខួរក្បាល ជំងឺសួត ជំងឺតំរងនោម ជំងឺរ៉ាំរ៉ៃ ជំងឺផ្សេងៗដែលកំពុងកើតមានឡើងឬកំពុងព្យាបាលមិនទាន់ជាសះស្បើយ ឬរបួសស្នាមរ៉ាំរ៉ៃដែរឬទេ? ប្រសិនបើប្រែប្រួលសូមបញ្ជាក់មូលហេតុ។"></asp:Label><br />

                                <asp:Label runat="server" ID="lblQuestion" Text="1. Has the Life Assured is in the  conditions which existed before effective date of insurance policy and defined as: Cancer, diabetes, heart disease, liver disease, stroke, lung disease, kidney disease or chronic injuries? If yes, please state the reason.:"></asp:Label>
                            </td>
                            <td style="border-left: 1px solid #21275b; padding-left: 20px;">
                                <asp:DropDownList ID="ddlAnswer" runat="server" class="span2">
                                    <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdfAnswer" runat="server" Value="" />
                                <span style="color: red;">*</span><br />
                                <asp:TextBox ID="txtAnswerRemarks" class="span5" placeholder="Please give details" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <asp:HiddenField ID="hdfQuestionID" runat="server" Value="6D44FA76-6B00-42D6-A4D7-ACD85986DC7C" />
                                <asp:HiddenField ID="hdfAnswerRemarks" runat="server" Value="" />
                            </td>

                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                    <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Payment</h3>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                    <table style="width: auto; margin-top: 10px; text-align: left;">
                        <tr>
                            <td style="text-align: right">
                                <asp:Label runat="server" ID="lblUserPremium" Text="Input Collected Premium (USD):"></asp:Label>
                            </td>
                            <td style="text-align: center;">
                                <asp:TextBox ID="txtUserPremium" class="span2" runat="server" Style="text-align: center;"></asp:TextBox>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="text-align: right">
                                <asp:Label runat="server" ID="lblPaymentReferenceNo" Text="Payment Reference No."></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentReferenceNo" class="span3" runat="server" Style="text-align: center;"></asp:TextBox>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="width: 20px;"></td>
                            <td>
                                <asp:Button runat="server" ID="btnGetPayment" class="btn btn-primary" Text="Payment" OnClick="btnGetPayment_Click" />

                            </td>

                        </tr>
                        <tr>
                            <td colspan="6">
                                <span style="color: red;">** Click Payment to get payment information automatically.</span>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align: top; top: 5px; border-right: 1pt solid #21275b; border-bottom: 1pt solid #21275b; background-color: #21275b;">
                    <h3 style="width: 100%; color: white; margin: 0; height: 25px;">Issue Policy</h3>
                </td>


            </tr>
            <%--Issue Policy--%>
            <tr>
                <td colspan="2" style="width: 100%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                    <table style="width: auto; margin-top: 10px;">
                        <tr>
                            <td style="text-align: right">
                                <asp:Label runat="server" ID="lblIssueDate" Text="Issue Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIssueDate" class="span2" runat="server" placeholder="DD-MM-YYYY"></asp:TextBox>
                                <span style="color: red;">*</span>
                            </td>
                            <td style="text-align: right">
                                <asp:Label runat="server" ID="lblEffectiveDate" Text="Effective Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEffectiveDate" class="span2" runat="server" placeholder="DD-MM-YYYY"></asp:TextBox>
                                <span style="color: red;">*</span>
                            </td>

                            <td style="width: 20px;"></td>
                            <td>

                                <asp:Button runat="server" ID="btnIssue" class="btn btn-primary" Text="Issue Policy" OnClick="bntIssue_Click" OnClientClick="return confirm('Do you want to process issue policy?');" />

                            </td>


                        </tr>
                        <tr>
                            <td colspan="6">
                                <span style="color: red;">** Issue and effective date format [DD-MM-YYYY]</span>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            </table>

            <asp:HiddenField ID="hdfChannelItemID" runat="server" Value="" />
            <asp:HiddenField ID="hdfChannelLocationID" runat="server" Value="" />
            <asp:HiddenField ID="hdfSaleAgentID" runat="server" Value="" />
            <asp:HiddenField ID="hdfSaleAgentName" runat="server" Value="" />
            <asp:HiddenField ID="hdfProductID" runat="server" Value="" />
            <asp:HiddenField ID="hdfRiderProductID" runat="server" Value="" />
            <asp:HiddenField ID="hdfApplicationNumber" runat="server" Value="" />
            <asp:HiddenField ID="hdfApplicationID" runat="server" Value="" />

            <asp:HiddenField ID="hdfExistCustomerID" runat="server" Value="" />
            <asp:HiddenField ID="hdfCustomerID" runat="server" Value="" />
            <asp:HiddenField ID="hdfApplicationCustomerID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyDetailID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyPaymentID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyAddressID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyBenID" runat="server" Value="" />
            <asp:HiddenField ID="hdfPolicyRiderID" runat="server" Value="" />

            <asp:Label ID="lblError" runat="server"></asp:Label>
            <!---Popup share Referral--->
            <div id="myReferralList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelMarketingCodeHeader" aria-hidden="true" data-keyboard="false" data-backdrop="static">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H1">Select Referral</h3>
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
                    <div id="dvAgentList"></div>

                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillReferral();" data-dismiss="modal" value="OK" />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
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

