<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="application_form_fp.aspx.cs" Inherits="Pages_Business_application_form_fp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <ul class="toolbar">
        <li>
            <!-- Button to trigger modal new application form -->
            <asp:ImageButton ID="ImgBtnAdd" runat="server"  data-toggle="modal" data-target="#myModalNewApplicationForm" ImageUrl="~/App_Themes/functions/add.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" runat="server"  data-toggle="modal" data-target="#mySearchApplication" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
        <li>
            <a data-toggle="dropdown">
                <input type="button" style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 90px;" /></a>
            <ul class="dropdown-menu">
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditPersonalDetails();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Personal Details</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditAddressContact();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Address & Contact</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditPremiumDiscount();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Premium Discount</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditBeneficiaries();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Beneficiaries</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditInsurancePlan();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Insurance Plan</a>
                </li>
            </ul>
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
            </div>
            <input type="button" onclick="ShowCancelModal()"  style="background: url('../../App_Themes/functions/cancel.png') no-repeat; border: none; height: 40px; width: 90px;" />

        </li>
        <li>
            <asp:ImageButton ID="ImgBtnClear" runat="server"  Visible="true" ImageUrl="~/App_Themes/functions/clear.png" CausesValidation="False" OnClick="ImgBtnClear_Click" />
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
            </div>
            <input type="button" onclick="SaveApplication()"  style="background: url('../../App_Themes/functions/save.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>

   
    <style>
        body .modal.large {
            width: 70%; /* desired relative width */
            left: 15%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
        }

        .auto-style1 {
            height: 42px;
        }

        .auto-style2 {
            height: 37px;
        }

        .auto-style3 {
            width: 25%;
        }

        .auto-style4 {
            height: 42px;
            width: 25%;
        }

        .auto-style5 {
            height: 37px;
            width: 25%;
        }

        .auto-style6 {
            width: 35%;
        }
    </style>

    <%--Javascript for bootstrap--%>
    <script type="text/javascript">

        //Validation Section
     
        //Validate integer
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        //Validate decimal number
        function ValidateTextDecimal(j) {

            var msg = j.value;
            var w;
            var nokta = msg.indexOf(".");
            var ind;

            for (w = 0; w < msg.length; w++) {

                ind = msg.substring(w, w + 1);
                if (ind < "0" || ind > "9") {

                    if (nokta > 0)
                        if (w == nokta) continue;

                    msg = msg.substring(0, w);
                    j.value = msg;
                    break;
                }

            }

        }
        //End Validation

        //Global Variables
        var marketing_code;
        var marketing_name;
        var register_id;
        var total_share = 0;
        var total_share_edit = 0;
        var exceed_premium_input = false;
        var check_result;
        var save_beneficiaries; //for checking saving failed
        var change_discount_rate_row_index = 0; // for use in GetPremiumWithDiscountByIndex and GetPremiumWithDiscountEditByIndex

        //Tab
        jQuery(document).ready(function ($) {
            $('#tabs').tab();
           // $("#tblAppContent *").attr('disabled', 'disabled');
        });

        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

            $('#Main_txtDateBirthEdit').datepicker().on('changeDate', function (dob_edit) {
              //  CalculateInsuranceEdit();
            });

            $('#Main_txtDateBirthLife1').datepicker().on('changeDate', function (dob) {

                $('#Main_hfdDateBirthLife1').val($('#Main_txtDateBirthLife1').val());
                
               CalculateInsurance();

            });
            $('#Main_txtDateBirthLife2').datepicker().on('changeDate', function (dob) {

                $('#Main_hfdDateBirthLife2').val($('#Main_txtDateBirthLife2').val());
               CalculateInsurance();

            });

            $('#Main_txtDateEntry').datepicker().on('changeDate', function (entry) {
                $('#<%=hdfDateEntry.ClientID%>').val($('#<%=txtDateEntry.ClientID%>').val());
               // $('#Main_hdfDateEntry').val($('#Main_txtDateEntry').val());
             //   CalculateInsuranceNoAlert();

            });

            

              
        });


        //Add New Applicaiton Section.....................................................................................................

        //Get complete format of application number with A + 8 digits number
        function GetCompleteAppNumber(app_number) {

            var complete_number = "A";

            var count = app_number.length;

            for (var i = count; i < 8; i++) {
                complete_number += "0";
            }

            complete_number += app_number;

            return complete_number;
        }

        //Fill Data for new application (OK click)
        function FillData() {

            //Clear all data first
            Clear();

            $("#tblAppContent *").removeAttr('disabled');

            total_share = 0;
            total_share_edit = 0;

            var application_number = $('#Main_txtApplicationNumberModal').val();

            if (application_number == "") {
                alert('Please fill in Application Number');
                return;
            }

            application_number = GetCompleteAppNumber(application_number);

            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/CheckApplicationNumber",
                data: "{app_number:" + JSON.stringify(application_number) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d === "0") {

                        check_result = "0";

                        var user_name = $('#Main_hdfusername').val();
                        $('#Main_txtApplicationNumber').val(application_number);
                        $('#Main_txtDataEntryBy').val(user_name);
                        $('#Main_txtDateEntry').val($('#Main_hdfDateEntry').val());
                        $('#Main_lblApplicationNumberModalValidate').hide();

                        //Get Insurance Plan according to selection of Product Type
                        var product_type = $('#Main_ddlProductTypeModal').val();
                        GetInsurancePlan(product_type, "");

                        //hide and show Loan for
                        if (product_type == "2") {

                            $('#dvLoan').show();
                            //enable term of insurance
                            $('#Main_txtTermInsurance').prop('readonly', false);

                            //select single payment mode
                            $('#Main_ddlPremiumMode').val(0);

                            //diabled Premium Mode
                            $('#Main_ddlPremiumMode').prop('disabled', true);

                        } else {
                            $('#dvLoan').hide();
                            //diabled term of insurance
                            $('#Main_txtTermInsurance').prop('readonly', true);

                            //select first option
                            $('#Main_ddlPremiumMode').val(1);

                            //enabled Premium Mode
                            $('#Main_ddlPremiumMode').prop('disabled', false);

                        }

                        //Close New Application Modal
                        $('#myModalNewApplicationForm').modal('hide');
                    } else {

                        check_result = "1";
                        $('#Main_lblApplicationNumberModalValidate').show();
                    }

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


    <script id="jTemplateInsurancePlan" type="text/html">
        {#foreach $T.d as record}
          
                    <option value='{ $T.record.Product_ID }'>{ $T.record.En_Title }</option>
        {#/for}
           
    </script>

    <script id="jTemplateInsurancePlanEdit" type="text/html">
        {#foreach $T.d as record}
          
                    <option value='{ $T.record.Product_ID }'>{ $T.record.En_Title }</option>
        {#/for}
           
    </script>

    <script id="jTemplateApplication" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">No</th>
                    <th style="border-width: thin; border-style: solid;">Application No</th>
                    <th style="border-width: thin; border-style: solid;">ID. Type</th>
                    <th style="border-width: thin; border-style: solid;">ID. Card No</th>
                    <th style="border-width: thin; border-style: solid;">Last Name</th>
                    <th style="border-width: thin; border-style: solid;">First Name</th>
                    <th style="border-width: thin; border-style: solid;">Gender</th>
                    <th style="border-width: thin; border-style: solid;">Birth Date</th>
                    <th style="border-width: thin; border-style: solid;">Nationality</th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td>{ $T.row$index + 1 }</td>
                        <td>{ $T.row.App_Number }</td>
                        <td>{ $T.row.ID_Type }</td>
                        <td>{ $T.row.ID_Card }</td>
                        <td>{ $T.row.Last_Name }</td>
                        <td>{ $T.row.First_Name }</td>
                        <td>{ $T.row.Gender }</td>
                        <td>{ formatJSONDate($T.row.Birth_Date) }</td>
                        <td>{ $T.row.Nationality }</td>
                        <td>
                            <input id="Ch{ $T.row$index }" type="checkbox" onclick="GetApplication('{ $T.row.App_Register_ID }', '{ $T.row$index }', '{ $T.row$total }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>

    
    <script id="jTemplateCompany" type="text/html">
        <option selected="selected" value='0'>.</option>
        {#foreach $T.d as record}          
             <option value='{ $T.record.Channel_Item_ID }'>{ $T.record.Channel_Name }</option>
        {#/for}
           
    </script>

    <%-- Form Design Section--%>
    <h1>Application Form</h1>

    <table id="tblAppContent" class="table-layout">
        <tr>
            <td style="vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Application Details</h3>

            </td>
           

        </tr>
        <tr>
            <td style="vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <table id="tblAppRegister" width="99%" style="margin-top: 15px; margin-bottom: 10px;">
                    <tr>
                        <td style="width: 30%; text-align: right">Application No.:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtApplicationNumber" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                            <asp:HiddenField ID="hdfAppNumber" runat="server" />
                        </td>
                        <td style="width: 35%">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorAppNumber" runat="server" ErrorMessage="Require Applicaiton No" ControlToValidate="txtApplicationNumber" ForeColor="Red" Text="*">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Channel:</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlChannel" DataSourceID="SqlDataSourceChannel" DataTextField="Type" DataValueField="Channel_ID" AppendDataBoundItems="true" runat="server" onchange="GetCompany(this.value);" >
                                <asp:ListItem Value="0">.</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Require Channel" InitialValue="0" ControlToValidate="ddlChannel" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                     </tr>
                    <tr>
                        <td style="text-align:right">Company:</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Require Company" InitialValue="0" ControlToValidate="ddlCompany" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                     <tr>
                        <td style="text-align: right">Payment Code:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtPaymentCode" runat="server" MaxLength="50" onkeyup="ValidateNumber(this);" ></asp:TextBox>
                        </td>
                        <td>
                           
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Policy No.:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtPolicyNumber" runat="server" ReadOnly="True"></asp:TextBox>

                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Underwrite Status:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtUnderwritingStatus" runat="server" ReadOnly="True"></asp:TextBox>
                            <asp:HiddenField ID="hdfUnderwritingStatus" runat="server" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Date of Entry:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtDateEntry" runat="server" MaxLength="15" onkeypress="return false;"></asp:TextBox>
                            <asp:HiddenField ID="hdfDateEntry" runat="server" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Data Entry By:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtDataEntryBy" runat="server" ReadOnly="True"></asp:TextBox>
                            <asp:HiddenField ID="hdfDataEntryBy" runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDataEntryBy" ErrorMessage="Require Entry By" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Date of Signature:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtDateSignature" runat="server" CssClass="datepicker" MaxLength="15"></asp:TextBox>
                            <asp:HiddenField ID="hdfDateSignature" runat="server" />
                        </td>
                        <td style="text-align:left;">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDateSignature" ErrorMessage="Require Date of Signature" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            (DD-MM-YYYY)
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid Date" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" ForeColor="Red" ControlToValidate="txtDateSignature"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Marketing Code:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtMarketingCode" runat="server" ReadOnly="True"></asp:TextBox>
                            <asp:HiddenField ID="hdfMarketingCode" runat="server" />
                            <asp:HiddenField ID="hdfPreviousMarketingCode" runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Require Maketing Code" ControlToValidate="txtMarketingCode" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                            <input type="button" data-toggle="modal" data-target="#myMarketingCodeList"  style="background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Name of Marketing:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtMarketingName" runat="server" ReadOnly="True"></asp:TextBox>
                            <asp:HiddenField ID="hdfMarketingName" runat="server" />
                            <asp:HiddenField ID="hdfPreviousMarketingName" runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMarketingName" ErrorMessage="Require Name of Marketing" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Note:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtNote" TextMode="MultiLine" Height="40" runat="server" MaxLength="255"></asp:TextBox>
                            <asp:HiddenField ID="hdfNote" runat="server" />
                        </td>
                        <td></td>
                    </tr>
                </table>

            </td>
           
        </tr>
        <tr>
            <td colspan="2">
                <div class="container">
                    <div id="tab-content" style="padding-top: 10px;">
                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                            <li class="active"><a href="#personalinformation" data-toggle="tab">Personal Information</a></li>
                            <li><a href="#mailling" data-toggle="tab">Mailling Address</a></li>
                            <li><a href="#jobhistory" data-toggle="tab">Job History</a></li>
                            <li><a href="#insuranceplan" data-toggle="tab">Insurance Plan</a></li>
                            <%--<li><a href="#discount" data-toggle="tab">Discount</a></li>--%>
                            <li><a href="#beneficiaries" data-toggle="tab">Beneficiaries</a></li>
                            <li><a href="#health" data-toggle="tab">Miscellaneolus and Health Details</a></li>
                        </ul>
                        <div id="my-tab-content" class="tab-content">
                            <div class="tab-pane " id="mailling">
                                <%--Mailling Address--%>
                                <table style= " width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                     <tr>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Policy owner</h3>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Life Insured 1</h3></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 14.8%; text-align: right">Address:</td>
                                                    <td style="width: 85.2%">
                                                        <asp:TextBox ID="txtAddress1Life1" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                                        <asp:TextBox ID="txtAddress2Life1" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Province/City:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtCityLife1" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Zip Code:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtZipCodeLife1" Width="96%" runat="server" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Country:</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCountryLife1" Width="97.3%" Height="25px" onchange="GetZipCode();" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceCountry" DataTextField="Country_Name" DataValueField="Country_ID">
                                                            <asp:ListItem Value="0">.</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Require Country" ControlToValidate="ddlCountryLife1" InitialValue="0" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Tel.:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtTelephoneLife1" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Mobile Phone:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtMobilePhoneLife1" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="Require Mobile Phone" ControlToValidate="txtMobilePhoneLife1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">E-mail:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmailLife1" Width="96%" runat="server" MaxLength="125"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Invalid E-mail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmailLife1"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <table width="100%" >
                                                <tr>
                                                    <td style="width: 14.8%; text-align: right">Address:</td>
                                                    <td style="width: 85.2%">
                                                        <asp:TextBox ID="txtAddress1Life2" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                                        <asp:TextBox ID="txtAddress2Life2" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Province/City:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtCityLife2" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Zip Code:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtZipCodeLife2" Width="96%" runat="server" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Country:</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCountryLife2" Width="97.3%" Height="25px" onchange="GetZipCode();" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceCountry" DataTextField="Country_Name" DataValueField="Country_ID">
                                                            <asp:ListItem Value="0">.</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="Require Country" ControlToValidate="ddlCountryLife2" InitialValue="0" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Tel.:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtTelephoneLife2" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Mobile Phone:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtMobilePhoneLife2" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="Require Mobile Phone" ControlToValidate="txtMobilePhoneLife2" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">E-mail:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmailLife2" Width="96%" runat="server" MaxLength="125"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Invalid E-mail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmailLife2"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                
                            </div>
                            <div class="tab-pane" id="personalinfo">

                            </div>
                            <%--life insured 2 --%>
                                <div class="tab-pane active" id="personalinformation">
                                <table  style="border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                   
                                    <tr>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Policy owner</h3>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Life Insured 1</h3></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <table width="99%" >
                                                 <tr>
                                                    <td style="width: 25%; text-align: right">I.D Type:</td>
                                                    <td style="width: 25%">
                                                        <asp:DropDownList ID="ddlIDTypeLife1" runat="server" class="span2">
                                                            <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                            <asp:ListItem Value="1">Passport</asp:ListItem>
                                                            <asp:ListItem Value="2">Visa</asp:ListItem>
                                                            <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hfdIDTypeLife1" runat="server" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">I.D No.:</td>
                                                    <td style="width: 25%">
                                                        <asp:TextBox ID="txtIDNumberLife1" class="span2" runat="server" MaxLength="30"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Require I.D No." ControlToValidate="txtIDNumberLife1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hfdIDNumberLife1" runat="server" />
                                                    </td>
                                                 </tr>
                           
                                                <tr>
                                                    <td style="text-align: right">Surname in Khmer:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurnameKhLife1" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                        <asp:HiddenField ID="hfdSurnameKhLife1" runat="server" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name in Khmer:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameKhLife1" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                        <asp:HiddenField ID="hfdFirstNameKhLife1" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Surname:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurnameEngLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Require Surname" ControlToValidate="txtSurnameEngLife1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hfdSurnameEngLife1" runat="server" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameEngLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Require First Name" ControlToValidate="txtFirstNameEngLife1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hfdFirstNameEngLife1" runat="server" />
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Surname of Father:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurnameFatherLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField ID="hfdSurnameFatherLife1" runat="server" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name of Father:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameFatherLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField ID="hfdFirstNameFatherLife1" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Surname Mother:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurnameMotherLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField ID="hfdSurnameMotherLife1" runat="server" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name of Mother:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameMotherLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField ID="hfdFirstNameMotherLife1" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right" class="auto-style1">Previous Surname:</td>
                                                    <td class="auto-style1">
                                                        <asp:TextBox ID="txtPreviousSurnameLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdPreviousSurnameLife1" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style4">Previous First Name:</td>
                                                    <td class="auto-style1">
                                                        <asp:TextBox ID="txtPreviousFirstnameLife1" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdPreviousFirstnameLife1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Nationality:</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlNationalityLife1" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlNationalityLife1" ErrorMessage="Require Nationality" ForeColor="Red" InitialValue="0" Text="*">*</asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hfdNationalityLife1" runat="server" />
                                                    </td>
                                                    <td class="auto-style3"></td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right" class="auto-style2">Gender:</td>
                                                    <td class="auto-style2">
                                                        <asp:DropDownList ID="ddlGenderLife1" class="span2" runat="server">
                                                            <asp:ListItem Value="1">Male</asp:ListItem>
                                                            <asp:ListItem Value="0">Female</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField runat="server" ID="hfdGenderLife1" />
                                                    </td>
                                                    <td class="auto-style5"></td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Date of Birth:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtDateBirthLife1" runat="server" MaxLength="15" CssClass="span2" onkeypress="return false;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtDateBirthLife1" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                            (DD-MM-YYYY)
                                                        <asp:HiddenField runat="server" ID="hfdDateBirthLife1" />
                                                    </td>
                                                    <td class="auto-style3" style="padding-top: 2px">
                                                        <input type="button" onclick="CalculateInsurance()"  style="background: url('../../App_Themes/functions/calculator.png') no-repeat; border: none; height: 25px; width: 25px;" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Date" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" ControlToValidate="txtDateBirthLife1" ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td></td>
                                               </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <table width="99%" style="margin-top: 15px; margin-bottom: 10px;">
                                                <tr>
                                                    <td style="width: 25%; text-align: right">I.D Type:</td>
                                                    <td style="width: 25%">
                                                        <asp:DropDownList ID="ddlIDTypeLife2" runat="server" class="span2">
                                                            <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                            <asp:ListItem Value="1">Passport</asp:ListItem>
                                                            <asp:ListItem Value="2">Visa</asp:ListItem>
                                                            <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField runat="server"  ID="hfdIDTypeLife2"/>
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">I.D No.:</td>
                                                    <td style="width: 25%">
                                                        <asp:TextBox ID="txtIDNumberLife2" class="span2" runat="server" MaxLength="30"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Require I.D No." ControlToValidate="txtIDNumberLife2" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField runat="server" ID="hfdIDNumberLife2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Surname in Khmer:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurNameKhLife2" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdSurNameKhLife2" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name in Khmer:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameKhLife2" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdFirstNameKhLife2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Surname:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurNameEngLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Require Surname" ControlToValidate="txtSurNameEngLife2" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField runat="server"  ID="hfdSurNameEngLife2"/>
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameEngLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="Require First Name" ControlToValidate="txtFirstNameEngLife2" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField runat="server" ID="hfdFirstNameEngLife2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Surname of Father:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurNameFatherLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdSurNameFatherLife2" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name of Father:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameFatherLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdFirstNameFatherLife2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Surname Mother:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSurNameMotherLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdSurNameMotherLife2" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style3">First Name of Mother:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtFirstNameMotherLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdFirstNameMotherLife2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right" class="auto-style1">Previous Surname:</td>
                                                    <td class="auto-style1">
                                                        <asp:TextBox ID="txtPreviousSurNameLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdPreviousSurNameLife2" />
                                                    </td>
                                                    <td style="text-align: right" class="auto-style4">Previous First Name:
                                                    </td>
                                                    <td class="auto-style1">
                                                        <asp:TextBox ID="txtPreviousFirstNameLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfdPreviousFirstNameLife2" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Nationality:</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlNationalityLife2" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="ddlNationalityLife2" ErrorMessage="Require Nationality" ForeColor="Red" InitialValue="0" Text="*">*</asp:RequiredFieldValidator>
                                                        <asp:HiddenField runat="server" ID="hfdNationalityLife2" />
                                                    </td>
                                                    <td class="auto-style3"></td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right" class="auto-style2">Gender:</td>
                                                    <td class="auto-style2">
                                                        <asp:DropDownList ID="ddlGenderLife2" class="span2" runat="server">
                                                            <asp:ListItem Value="1">Male</asp:ListItem>
                                                            <asp:ListItem Value="0">Female</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField runat="server" ID="hfdGenderLife2" />
                                                    </td>
                                                    <td class="auto-style5"></td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Date of Birth:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtDateBirthLife2" runat="server" MaxLength="15" CssClass="span2" onkeypress="return false;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtDateBirthLife2" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                        (DD-MM-YYYY)
                                                        <asp:HiddenField runat="server" ID="hfdDateBirthLife2" />
                                                    </td>
                                                    <td class="auto-style3" style="padding-top: 2px">
                                                        <input type="button" onclick="CalculateInsurance(2)"  style="background: url('../../App_Themes/functions/calculator.png') no-repeat; border: none; height: 25px; width: 25px;" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Invalid Date" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" ControlToValidate="txtDateBirthLife2" ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                

                            </div>
                            <div class="tab-pane" id="jobhistory">
                                <%--Job History--%>
                                 <table style= " width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                     <tr>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Policy owner</h3>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Life Insured 1</h3></td>
                                    </tr>
                                    <tr>
                                         <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                             <table width="100%">
                                                <tr>
                                                    <td style="width: 35%; text-align: right">Name of Employer:</td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtNameEmployerLife1" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Nature of Business:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtNatureBusinessLife1" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Role and Responsibility:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtRoleAndResponsibilityLife1" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Current Position:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtCurrentPositionLife1" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Anual Income (USD):</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnualIncomeLife1" Width="96%" runat="server" onkeyup="ValidateNumber(this);" MaxLength="12"></asp:TextBox>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>

                                            </table>
                                         </td>
                                         <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                             <table width="100%">
                                                <tr>
                                                    <td style="width: 35%; text-align: right">Name of Employer:</td>
                                                    <td style="width: 65%">
                                                        <asp:TextBox ID="txtNameEmployerLife2" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Nature of Business:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtNatureBusinessLife2" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Role and Responsibility:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtRoleAndResponsibilityLife2" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Current Position:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtCurrentPositionLife2" Width="96%" runat="server" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">Anual Income (USD):</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnualIncomeLife2" Width="96%" runat="server" onkeyup="ValidateNumber(this);" MaxLength="12"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>

                                            </table>
                                         </td>
                                     </tr>
                                </table>
                            </div>

                            <div class="tab-pane" id="insuranceplan">
                                <%--Insurance Plan--%>
                                
                                        <table width="100%">
                                            <tr>
                                                <td width="70%" style="vertical-align: top;">
                                                    <table style="border-right: 1pt solid #d5d5d5;">
                                                        <tr>
                                                            <td style="text-align: right">
                                                                Calculate Premium
                                                            </td>
                                                            <td>
                                                                <input type="button" onclick="CalculateInsurance(1); CalculateInsurance(2);"  style="background: url('../../App_Themes/functions/calculator.png') no-repeat; border: none; height: 25px; width: 25px;" />
                                                            </td>
                                                            <td></td>
                                                        </tr>       
                                                         <tr>
                                                            <td style="text-align: right">Type of Insurance Plan:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlTypeInsurancePlan" onchange="ProcessInsuranceType(this.value);" runat="server"></asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Require Type of Insurance Plan" ControlToValidate="ddlTypeInsurancePlan" InitialValue="0" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                <asp:HiddenField runat="server" ID="hfdTypeInsurancePlan" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Term of Insurance:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtTermInsurance" runat="server" ReadOnly="True" onkeyup="ValidateNumber(this); InputTermInsurance();" MaxLength="3"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Require Term of Insurance" ControlToValidate="txtTermInsurance" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;years
                                                                <asp:HiddenField runat="server" ID="hfdTermInsurance" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Payment Period:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtPaymentPeriod" runat="server" ReadOnly="True"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Require Payment Period" ControlToValidate="txtPaymentPeriod" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;years
                                                                <asp:HiddenField runat="server" ID="hfdPaymentPeriod" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Insurance Amount Required:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtInsuranceAmountRequired" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this); GetPremium(0);"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Require Insurance Amount Required" ControlToValidate="txtInsuranceAmountRequired" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>USD.
                                                                <asp:HiddenField runat="server" ID="hfdInsuranceAmountRequired" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Premium Mode:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPremiumMode" runat="server" DataSourceID="SqlDataSourcePaymentMode" DataTextField="Mode" DataValueField="Pay_Mode_ID"  >
                                                                </asp:DropDownList>
                                                                <asp:HiddenField runat="server" ID="hfdPremiumMode" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Discount:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtDiscountAmount" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this); ProcessDiscountAmount();" Text="0"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDiscountAmount" runat="server" ErrorMessage="Require Discount Amount" ControlToValidate="txtDiscountAmount" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>USD.
                                                                <asp:HiddenField runat="server" ID="hfdDiscountAmount" />
                                                            </td>
                                                            <td></td>
                                                        </tr>

                                                        <tr>
                                                            <td></td>
                                                            <td><h3 style="text-align:left;">Policyowner</h3></td>
                                                            <td><h3 style="text-align:left;">Life Insured 1</h3></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 29.3%; text-align: right">Assuree Age:</td>
                                                            <td>
                                                                 <asp:TextBox ID="txtAssureeAgeLife1" runat="server" ReadOnly="True"></asp:TextBox>
                                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Require Assuree Age" Text="*" ControlToValidate="txtAssureeAgeLife1" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;years
                                                                <asp:HiddenField runat="server" ID="hfdAssureeAgeLife1" />
                                                            </td>
                                                             <td>
                                                                 <asp:TextBox ID="txtAssureeAgeLife2" runat="server" ReadOnly="True"></asp:TextBox>
                                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="Require Assuree Age" Text="*" ControlToValidate="txtAssureeAgeLife2" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;years
                                                                 <asp:HiddenField runat="server" ID="hfdAssureeAgeLife2" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right">Annual Premium (System):</td>
                                                            <td>
                                                                <asp:TextBox ID="txtAnnualOriginalPremiumAmountSystemLife1" runat="server" ReadOnly="True"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAnnualOriginalPremiumAmountSystem" runat="server" ErrorMessage="Require Annual Premium Amount (System)" ControlToValidate="txtAnnualOriginalPremiumAmountSystemLife1" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;USD.
                                                                 <asp:HiddenField runat="server" ID="hfdAnnualOriginalPremiumAmountSystemLife1" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAnnualOriginalPremiumAmountSystemLife2" runat="server" ReadOnly="True"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="Require Annual Premium Amount (System)" ControlToValidate="txtAnnualOriginalPremiumAmountSystemLife2" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;USD.
                                                                 <asp:HiddenField runat="server" ID="hfdAnnualOriginalPremiumAmountSystemLife2" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td style="text-align: right">Premium (System):</td>
                                                            <td>
                                                                <asp:TextBox ID="txtPremiumAmountSystemLife1" runat="server" ReadOnly="True"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Require Premium Amount (System)" ControlToValidate="txtPremiumAmountSystemLife1" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;USD.
                                                                  <asp:HiddenField runat="server" ID="hfdPremiumAmountSystemLife1" />
                                                            </td>
                                                           <td>
                                                                <asp:TextBox ID="txtPremiumAmountSystemLife2" runat="server" ReadOnly="True"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ErrorMessage="Require Premium Amount (System)" ControlToValidate="txtPremiumAmountSystemLife2" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;USD.
                                                               <asp:HiddenField runat="server" ID="hfdPremiumAmountSystemLife2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style=" text-align: right;">Total Premium:</td>
                                                            <td >
                                                                <asp:TextBox ID="txtTotalPremium" runat="server" ReadOnly="true"></asp:TextBox>
                                                                &nbsp;USD
                                                                <asp:HiddenField runat="server" ID="hfdTotalPremium" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">Premium (User):</td>
                                                            <td>
                                                                <asp:TextBox ID="txtPremiumAmount" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Require Premium Amount" ControlToValidate="txtPremiumAmount" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>USD.
                                                            </td>
                                                        </tr>  
                                                         <tr>
                                                            <td style="vertical-align: top; text-align: right;">Note:</td>
                                                            <td >
                                                                <asp:TextBox ID="txtInsurancePlanNote" runat="server"  MaxLength="254" TextMode="MultiLine" Height="50"></asp:TextBox>                        
                                                            </td>
                                                            <td></td>
                                                        </tr>  
                                                        
                                                    </table>
                                           
                                                </td>
                                                <td width="50%" style="vertical-align: top;">
                                                    <div id="dvLoan">
                                                        <%--Loan Information--%>
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 20.3%; text-align: right">Loan Type:</td>
                                                                <td style="width: 79.7%">
                                                                    <asp:DropDownList ID="ddlLoanType" runat="server">
                                                                        <asp:ListItem Value="0">Home</asp:ListItem>
                                                                        <asp:ListItem Value="1">Business</asp:ListItem>
                                                                        <asp:ListItem Value="2">Others</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:HiddenField ID="hdfLoanType" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Interest:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtInterest" runat="server" MaxLength="5" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>
                                                                    <asp:Label ID="lblInterestValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label>&nbsp;%
                                                                    <asp:HiddenField ID="hdfInterest" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Term:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTermLoan" runat="server" MaxLength="3" onkeyup="ValidateNumber(this);"></asp:TextBox>
                                                                    <asp:Label ID="lblTermLoanValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label>&nbsp;years
                                                                    <asp:HiddenField ID="hdfTermLoan" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Loan Effective Date:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLoanEffectiveDate" CssClass="datepicker" runat="server" MaxLength="15"></asp:TextBox>
                                                                    <asp:Label ID="lblLoanEffectiveDateValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label>&nbsp;(DD-MM-YYYY)
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtLoanEffectiveDate" runat="server" ErrorMessage="Invalid Date" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                    <asp:HiddenField ID="hdfLoanEffectiveDate" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right">Outstanding Loan:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOutstandingLoanAmount" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>
                                                                    <asp:Label ID="lblOutstandingLoadAmountValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label>&nbsp;USD.
                                                                    <asp:HiddenField ID="hdfOutstandingLoanAmount" runat="server" />
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                               
                            </div>
                           
                            <div class="tab-pane" id="beneficiaries">
                                <%--Benefitciary--%>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table id="tblBenefitTable" class="table table-bordered">

                                                <tr>
                                                    <td width="2%">&nbsp;</td>

                                                    <td colspan="5">
                                                        <div style="padding-bottom: 5px">Note</div>
                                                        <asp:TextBox ID="txtBenefitNote" runat="server" Width="98.7%" MaxLength="255"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfBenefitNote" runat="server" />
                                                    </td>
                                                    <td style="text-align: center; vertical-align: middle;">
                                                        <asp:Label ID="lblTotalBenefitPercentage" runat="server" Text="Total: 0%"></asp:Label>
                                                    </td>
                                                    <td width="2%"></td>
                                                </tr>
                                                <tr>
                                                    <td width="2%"></td>
                                                    <td>
                                                        <div style="padding-bottom: 5px">ID. Type</div>
                                                        <asp:DropDownList ID="ddlBenefitIDType" runat="server" Style="width: 99%; height: 40px">
                                                            <asp:ListItem Value="0"> I.D Card</asp:ListItem>
                                                            <asp:ListItem Value="1">Passport</asp:ListItem>
                                                            <asp:ListItem Value="2">Visa</asp:ListItem>
                                                            <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdfBenefitIDType" runat="server" />
                                                    </td>
                                                    <td>
                                                        <div style="padding-bottom: 5px">ID. No<asp:Label ID="lblBenefitIDNoValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                                        <asp:TextBox ID="txtBenefitIDNo" Style="width: 93%" runat="server" MaxLength="30" Height="30"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfBenefitIDNo" runat="server" />
                                                    </td>

                                                    <td>
                                                        <div style="padding-bottom: 5px">Surname and Name<asp:Label ID="lblBenefitNameValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                                        <asp:TextBox ID="txtBenefitName" Style="width: 93%" runat="server" MaxLength="100" Height="30"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfBenefitName" runat="server" />
                                                    </td>

                                                    <td>
                                                        <div style="padding-bottom: 5px">Relation</div>
                                                        <asp:DropDownList ID="ddlBenefitRelation" Style="width: 99%" Height="40" runat="server" DataSourceID="SqlDataSourceRelationship" DataTextField="Relationship" DataValueField="Relationship" onchange="CheckRelation(this.value)"></asp:DropDownList>
                                                        <asp:HiddenField ID="hdfBenefitRelation" runat="server" />
                                                    </td>
                                                    <td>
                                                        <div style="padding-bottom: 5px">Share (%)<asp:Label ID="lblBenefitSharePercentageValidate" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                                        <asp:TextBox ID="txtBenefitSharePercentage" Style="width: 93%" MaxLength="5" runat="server" Height="30" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfBenefitSharePercentage" runat="server" />
                                                    </td>
                                                    <td style="vertical-align: middle; padding-top: 10px; text-align: center">
                                                        <button type="button" class="btn btn-default btn-lg" style="width: 69px; height: 40px" onclick="AddNewBenefitRow();">
                                                            <span class="icon-ok"></span>Add
                                                        </button>
                                                    </td>
                                                    <td width="2%"></td>
                                                </tr>

                                            </table>

                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <div class="tab-pane" id="health">
                                <%--Miscellaneous Health--%>
                                        
                                 <table style= " width:100%; border-top:1pt solid #d5d5d5; border-left:1pt solid #d5d5d5; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                     <tr>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Policy owner</h3>
                                        </td>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Life Insured 2</h3></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            
                                             <table border="0">
                                                 
                                                <tr>
                                                    <td width="19%" style="text-align: right">Height:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHeightLife1" Width="90%" runat="server" MaxLength="3" onkeyup="ValidateNumber(this);"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="Require Height" ControlToValidate="txtHeightLife1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>cm.</td>
                                                </tr>
                                                <tr>
                                                    <td width="19%" style="text-align: right">Weight:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtWeightLife1" Width="90%" runat="server" MaxLength="3" onkeyup="ValidateNumber(this);"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="Require Weight" ControlToValidate="txtWeightLife1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>kg.</td>
                                                   
                                                </tr>
                                                <tr>
                                               
                                                    <td width="50%" style="text-align: right;">Weight change in pass 6 months?</td>
                                                    <td colspan="2">
                                                       

                                                                
                                                        <asp:RadioButtonList ID="rblWeightChangeLife1" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal" CellSpacing="-1"  OnSelectedIndexChanged="rblWeightChangeLife1_SelectedIndexChanged" >
                                                            <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Increase" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Descrease" Value="2"></asp:ListItem>
                                                        </asp:RadioButtonList>

                                                                 
                                                    </td>
                                                  
                                                </tr>
                                                <tr>
                                                    <td width="19%" style="text-align: right;">
                                                        <asp:Label ID="lblWeightChangeReasonLife1" Text="Reason:"  Visible="false" runat="server"></asp:Label></td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtWeightChangeReasonLife1"  Visible="false" runat="server"  Width="83.6%" Height="30px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                                  
                                            </table>
                                             
                                        </td>
                                        <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                                            <table border="0">
                                                <tr>
                                                    <td width="19%" style="text-align: right">Height:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHeightLife2" Width="90%" runat="server" MaxLength="3" onkeyup="ValidateNumber(this);"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ErrorMessage="Require Height" ControlToValidate="txtHeightLife2" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>cm.</td>
                                                </tr>
                                                <tr>
                                                    <td width="19%" style="text-align: right">Weight:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtWeightLife2" Width="90%" runat="server" MaxLength="3" onkeyup="ValidateNumber(this);"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ErrorMessage="Require Weight" ControlToValidate="txtWeightLife2" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>kg.</td>
                                                   
                                                </tr>
                                                <tr>
                                               
                                                    <td width="50%" style="text-align: right;">Weight change in pass 6 months?</td>
                                                    <td colspan="2">
                                                        <asp:RadioButtonList ID="rblWeightChangeLife2" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal" CellSpacing="-1" OnSelectedIndexChanged="rblWeightChangeLife2_SelectedIndexChanged" >
                                                            <asp:ListItem Selected="True" Text="No" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Increase" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Descrease" Value="2"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                  
                                                </tr>
                                                <tr>
                                                    <td width="19%" style="text-align: right;">
                                                        <asp:Label ID="lblWeightChangeReasonLife2" Text="Reason:" Visible="false" runat="server"></asp:Label></td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtWeightChangeReasonLife2"  Visible="false" runat="server"  Width="83.6%" Height="30px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                   
                                    <tr>
                                        <td width="2%"></td>
                                        <td colspan="3">
                                            <%--Gridview Question--%>
                                            <asp:GridView ID="GvQA" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="GvQA_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="Question" HeaderText="Question" ItemStyle-Width="84%" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAnswerLife1" Text="Owner" runat="server" Width="180px"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rbtnlAnswerLife1" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                <asp:ListItem Selected="True" Text="(None)" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:HiddenField ID="hdfSeqNumberLife1" Value='<%# Eval("Seq_Number") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfQuestionIDLife1" Value='<%# Eval("Question_ID") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAnswerLife2" Text="Life1" runat="server"  Width="180px"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rbtnlAnswerLife2" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                <asp:ListItem Selected="True" Text="(None)" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:HiddenField ID="hdfSeqNumberLife2" Value='<%# Eval("Seq_Number") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfQuestionIDLife2" Value='<%# Eval("Question_ID") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                        <td width="2%"></td>
                                    </tr>
                                </table>
                             
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <!-- Modal New Application Form -->
               
                <div id="myModalNewApplicationForm" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelNewApplicationHeader" aria-hidden="true">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h3 id="myModalLabelNewApplicationHeader">New Application Form</h3>
                    </div>
                    <div class="modal-body">
                        <!---Modal Body--->
                        <table style="width: 100%; text-align: left;">
                            <tr>
                                <td style="vertical-align: middle">Application No.:</td>
                                <td style="vertical-align: bottom">
                                    <asp:TextBox ID="txtApplicationNumberModal" runat="server" onkeyup="ValidateNumber(this);"></asp:TextBox>
                                    <asp:Label ID="lblApplicationNumberModalValidate" runat="server" Text="Application No already exist." ForeColor="Red" Style="display: none;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: middle">Product Type:</td>
                                <td style="vertical-align: bottom">
                                    <asp:DropDownList ID="ddlProductTypeModal" runat="server" DataSourceID="SqlDataSourceProductType" DataTextField="myproducttype" DataValueField="product_type_id"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>



                    </div>
                    <div class="modal-footer">
                      
                      <input type="button" id="mybtn" class="btn btn-primary" style="height: 27px;" onclick="FillData();" value="OK" />
                      <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                    <%--  <input type="button" id="mytest" class="btn btn-primary" onclick="FillData();" value="OK" />--%>
                    </div>
                </div>
            
    <!--End Modal New Application Form-->

    <!-- Modal Marketing Code -->
    <div id="myMarketingCodeList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelMarketingCodeHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
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
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillMarketingCode();" data-dismiss="modal" value="Select" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End ModalMarketing Code-->

    <!-- Modal Search Application -->
    <div id="mySearchApplication" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchApplicationHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H2">Search Application Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <ul class="nav nav-tabs" id="myTabApplicationSearch">
                <li class="active"><a href="#SAppNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Application No</a></li>
                <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By Customer Name</a></li>
                <li><a href="#SIDCardNo" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Search By ID. Card No</a></li>
            </ul>

            <div class="tab-content" style="height: 60px; overflow: hidden;">
                <div class="tab-pane active" id="SAppNo">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 13%; vertical-align: middle">Application No:</td>
                            <td style="width: 30%; vertical-align: bottom">
                                <asp:TextBox ID="txtApplicationNumberSearch" Width="90%" runat="server"></asp:TextBox>

                            </td>
                            <td style="width: 56%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchApplicationByAppNo();" value="Search" />
                            </td>

                        </tr>
                    </table>
                    <hr />
                </div>
                <div class="tab-pane" id="SCustomerName">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 10%; vertical-align: middle">Last Name:</td>
                            <td style="width: 26%; vertical-align: bottom">
                                <asp:TextBox ID="txtLastNameSearch" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 10%; vertical-align: middle">First Name:</td>
                            <td style="width: 26%; vertical-align: bottom">
                                <asp:TextBox ID="txtFirstNameSearch" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 18%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchApplicationByCustomerName();" value="Search" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>
                <div class="tab-pane" id="SIDCardNo">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 8%; vertical-align: middle">ID Type:</td>
                            <td style="width: 28%; vertical-align: bottom">
                                <asp:DropDownList ID="ddlIDTypeSearch" Height="27px" runat="server">
                                    <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                    <asp:ListItem Value="1">Passport</asp:ListItem>
                                    <asp:ListItem Value="2">Visa</asp:ListItem>
                                    <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 10%; vertical-align: middle">ID Card No:</td>
                            <td style="width: 26%; vertical-align: bottom">
                                <asp:TextBox ID="txtIDCardNoSearch" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 18%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchApplicationByIDCardNo();" value="Search" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>

            </div>
            <div id="dvApplicationList"></div>
        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillSearchedData();" data-dismiss="modal" value="Select" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Search Application-->

    <!-- Modal Cancel Application Form -->
    <div id="myCancelAppModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalCancelApplicationHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H3">Cancel Application Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="vertical-align: middle; width: 150px;">Application Number:</td>
                    <td style="vertical-align: bottom">
                        <asp:Label ID="lblAppNumber" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle">Cancel Note:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtCancelNote" Width="90%" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>

        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="CancelApplication();" value="OK" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Cancel Application Form-->

    <!-- Modal Edit Address & Contact Application Form -->
    <div id="myEditAddressAndContactModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditAddress&ContactApplicationHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H4">Edit Address & Contact</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="vertical-align: middle; width: 100px; text-align: right;">Address:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtAddress1Edit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 100px; text-align: right;"></td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtAddress2Edit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; text-align: right;">Province:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtProvinceEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; text-align: right;">Zip Code:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtZipCodeEdit" Width="90%" runat="server" ReadOnly="true" MaxLength="10"></asp:TextBox>
                        <asp:HiddenField ID="hdfZipCodeEdit" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; text-align: right;">Country:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlCountryEdit" Width="93%" onchange="GetZipCodeEdit();" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="Country_Name" DataValueField="Country_ID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; text-align: right;">Phone no:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtTelephoneEdit" Width="90%" runat="server"  MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; text-align: right;">Mobile no:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtMobilePhoneEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; text-align: right;">Email:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtEmailEdit" Width="90%" runat="server" MaxLength="125"></asp:TextBox>
                    </td>
                </tr>

            </table>

        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditAddressContact();" value="OK" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>

        </div>
    </div>
    <!--End Modal Edit Address & Contact-->

    <!-- Modal Edit Personal Details -->
    <div id="myEditPersonalDetailsModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditPersonalDetailsApplicationHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H5">Edit Personal Details</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">I.D. Type:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlIDTypeEdit" runat="server" class="span2">
                            <asp:ListItem Value="0">I.D Card</asp:ListItem>
                            <asp:ListItem Value="1">Passport</asp:ListItem>
                            <asp:ListItem Value="2">Visa</asp:ListItem>
                            <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;">I.D. No:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtIDNoEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">Surname in Khmer:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtSurnameKhEdit" Width="90%" Height="25px" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;">First Name in Khmer:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtFirstNameKhEdit" Width="90%" Height="25px" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">Surname:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;">First Name:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">Surname of Father:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtFatherSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;">First Name of Father:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtFatherFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">Surname of Mother:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtMotherSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;">First Name of Mother:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtMotherFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">Previous Surname:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPreviousSurnameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;">Previouse First Name:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPreviousFirstNameEdit" Width="90%" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">Nationality:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlNationalityEdit" class="span2" runat="server" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                        </asp:DropDownList>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;"></td>
                    <td style="vertical-align: bottom"></td>
                </tr>
            </table>

        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditPersonalDetails();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>

        </div>
    </div>
    <!--End Modal Edit Personal Details-->

    <!-- Modal Edit Benefitciaries -->
    <div id="myEditBeneficiariesModal" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalEditBeneficiariesApplicationHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H6">Edit Beneficiaries</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td>
                        <table id="tblBenefitTableEdit" class="table table-bordered">

                            <tr>
                                <td width="2%">&nbsp;</td>

                                <td colspan="5">
                                    <div style="padding-bottom: 5px">Note</div>
                                    <asp:TextBox ID="txtBenefitNoteEdit" runat="server" Width="96.6%" MaxLength="255"></asp:TextBox>

                                </td>
                                <td style="text-align: center; vertical-align: middle;">
                                    <asp:Label ID="lblTotalBenefitPercentageEdit" runat="server"></asp:Label>
                                </td>
                                <td width="2%"></td>
                            </tr>
                            <tr>
                                <td width="2%"></td>
                                <td>
                                    <div style="padding-bottom: 5px">ID. Type</div>
                                    <asp:DropDownList ID="ddlBenefitIDTypeEdit" runat="server" Height="40" Style="width: 90%">
                                        <asp:ListItem Value="0"> I.D Card</asp:ListItem>
                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                        <asp:ListItem Value="2">Visa</asp:ListItem>
                                        <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td>
                                    <div style="padding-bottom: 5px">ID. No</div>
                                    <asp:TextBox ID="txtBenefitIDNoEdit" Style="width: 83%" runat="server" Height="30" MaxLength="30"></asp:TextBox>&nbsp;
                                                       
                                </td>

                                <td>
                                    <div style="padding-bottom: 5px">Surname and Name</div>
                                    <asp:TextBox ID="txtBenefitNameEdit" Style="width: 83%" runat="server" Height="30" MaxLength="100"></asp:TextBox>&nbsp;
                                                        <asp:Label ID="lblBenefitNameValidateEdit" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label>

                                </td>

                                <td>
                                    <div style="padding-bottom: 5px">Relation</div>
                                    <asp:DropDownList ID="ddlBenefitRelationEdit" Style="width: 90%" Height="40" runat="server" onchange="CheckRelationEdit(this.value);" DataSourceID="SqlDataSourceRelationship" DataTextField="Relationship" DataValueField="Relationship"></asp:DropDownList>

                                </td>
                                <td>
                                    <div style="padding-bottom: 5px">Share (%)</div>
                                    <asp:TextBox ID="txtBenefitSharePercentageEdit" Style="width: 83%" MaxLength="5" Height="30" runat="server" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>&nbsp;
                                                        <asp:Label ID="lblBenefitSharePercentageValidateEdit" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label>

                                </td>
                                <td style="vertical-align: middle; padding-top: 10px; text-align: center">
                                    <button type="button" class="btn btn-default btn-lg" style="width: 69px; height: 40px" onclick="AddNewBenefitRowEdit();">
                                        <span class="icon-ok"></span>Add
                                    </button>
                                </td>
                                <td width="2%"></td>
                            </tr>

                        </table>

                    </td>
                </tr>
            </table>

        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditBeneficiaries();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>

        </div>
    </div>
    <!--End Modal Edit Beneficiaries-->

    <!-- Modal Edit Insurance Plan -->
    <div id="myEditInsurancePlanModal" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalEditInsurancePlanApplicationHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H7">Edit Insurance Plan</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Gender:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlGenderEdit" Width="88.1%" runat="server" onchange="CalculateInsuranceEdit();">
                            <asp:ListItem Value="1">Male</asp:ListItem>
                            <asp:ListItem Value="0">Female</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Date of Birth:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtDateBirthEdit" runat="server" onkeypress="return false;" Width="85%"></asp:TextBox>

                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Assuree Age:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtAssureeAgeEdit" ReadOnly="true" Width="85%" runat="server" ValidationGroup="3"></asp:TextBox>

                        &nbsp;Years
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Type of Insurance Plan:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlTypeInsurancePlanEdit" Width="88.1%" onchange="ProcessInsuranceTypeEdit(this.value);" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <%--<asp:CheckBox ID="ckbIsPrePremiumDiscountEdit" Text="Is Pre-Premium Discount" Checked="true" runat="server" onchange="GetPremiumEdit(0);"/>--%>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Term of Insurance:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtTermInsuranceEdit" runat="server" Width="85%" ReadOnly="True" onkeyup="ValidateNumber(this); InputTermInsuranceEdit();" MaxLength="3" ValidationGroup="3"></asp:TextBox>

                        &nbsp;Years
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Payment Period:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPaymentPeriodEdit" Width="85%" runat="server" ReadOnly="true" ValidationGroup="3"></asp:TextBox>

                        &nbsp;Years
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Insurance Amount Required:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtInsuranceAmountRequiredEdit" Width="85%" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this); GetPremiumEdit(0);" ValidationGroup="3"></asp:TextBox>

                        &nbsp;USD
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Annual Premium (System):</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtAnnualOriginalPremiumAmountSystemEdit" Width="85%" runat="server" ReadOnly="True" ValidationGroup="3"></asp:TextBox>

                        &nbsp;USD
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Premium (System):</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPremiumAmountSystemEdit" Width="85%" runat="server" ReadOnly="True" ValidationGroup="3"></asp:TextBox>

                        &nbsp;USD
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Premium Mode:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlPremiumModeEdit" runat="server" Width="88.1%" DataSourceID="SqlDataSourcePaymentMode" DataTextField="Mode" DataValueField="Pay_Mode_ID" onchange="GetCustomerAgeEdit();">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                 <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Discount:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtDiscountAmountEdit" Width="85%" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this); ProcessDiscountAmountEdit();" ValidationGroup="3"></asp:TextBox>
                        &nbsp;USD
                        
                    </td>
                     <td></td>
                </tr>
                 <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Total Premium:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtTotalPremiumEdit" Width="85%" runat="server" ValidationGroup="3" ReadOnly="true"></asp:TextBox>
                        &nbsp;USD
                        
                    </td>
                     <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Premium (User):</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPremiumAmountEdit" Width="85%" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this);" ValidationGroup="3"></asp:TextBox>
                        &nbsp;USD
                        
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Note:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtInsurancePlanNoteEdit" Width="85%" runat="server" ValidationGroup="3"  MaxLength="254" TextMode="MultiLine" Height="50"></asp:TextBox>                        
                        
                    </td>
                    <td></td>
                </tr>

            </table>
        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditInsurancePlan();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Edit Product -->

    <!-- Modal Edit Premium Discount -->
    <div id="myEditPremiumDiscountModal" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalEditPremiumDiscountApplicationHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H8">Edit Premium Discount</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td>
                        <table id="tblDiscountTableEdit" class="table table-bordered">
                            <tr>
                                <td width="2%"></td>
                                <td width="22%">
                                    <div style="padding-bottom: 5px">Year</div>
                                    <asp:TextBox ID="txtDiscountYearEdit" runat="server" Height="30" Text="1" ReadOnly="true" Width="94%"></asp:TextBox>

                                </td>
                                <td width="22%">
                                    <div style="padding-bottom: 5px">Discount Rate (%)<asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red" Style="display: none;"></asp:Label></div>
                                    <asp:TextBox ID="txtDiscountRateEdit" runat="server" MaxLength="5" Height="30" Width="94%" onkeyup="ValidateTextDecimal(this); GetPremiumWithDiscountEdit(this.value);"></asp:TextBox>
                               
                                </td>

                                <td width="22%">
                                    <div style="padding-bottom: 5px">Premium Discount (Annual)</div>
                                    <asp:TextBox ID="txtPremiumDiscountEdit" runat="server" Height="30" onkeyup="ValidateTextDecimal(this); GetPremiumWithDiscountAmountEdit(this.value);" MaxLength="11" Width="94%"></asp:TextBox>

                                </td>
                                <td width="22%">
                                    <div style="padding-bottom: 5px">Premium After Discount (Annual)</div>
                                    <asp:TextBox ID="txtPremiumAfterDiscountEdit" runat="server" Height="30" ReadOnly="true" Width="94%"></asp:TextBox>

                                </td>
                                <td style="vertical-align: middle; padding-top: 10px; text-align: center; width: 8%">
                                    <button type="button" class="btn btn-default btn-lg" style="width: 69px; height: 40px" onclick="AddNewDiscountRowEdit();">
                                        <span class="icon-ok"></span>Add
                                    </button>
                                </td>
                                <td width="2%"></td>
                            </tr>

                        </table>

                    </td>
                </tr>
            </table>

        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditPremiumDiscount();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>

        </div>
    </div>
    <!--End Modal Edit Premium Discount-->


    <%-- Section Hidenfields Initialize  --%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
    <asp:HiddenField ID="hdfTotalAgentRow" runat="server" />
    <asp:HiddenField ID="hdfInsurancePlan" runat="server" />
    <asp:HiddenField ID="hdfPreviousInsurancePlan" runat="server" />
    <asp:HiddenField ID="hdfBenefitCount" runat="server" />
    <asp:HiddenField ID="hdfAppRegisterID" runat="server" />
    <asp:HiddenField ID="hdfProductType" runat="server" />
    <asp:HiddenField ID="hdfAnswers" runat="server" />
    <asp:HiddenField ID="hdfQuestionIDs" runat="server" />
    <asp:HiddenField ID="hdfOriginalPremiumAmountSystem" runat="server" />
    <asp:HiddenField ID="hdfPreviousOriginalPremiumAmountSystem" runat="server" />
    <%-- End Section Hidenfields Initialize  --%>

    <!--- Section Sqldatasource--->
    <asp:SqlDataSource ID="SqlDataSourceProductType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_Type_ID, CONVERT(varchar,Product_Type_ID + replicate(' ', 50 - len(Product_Type_ID))) + '. ' + Product_Type AS myproducttype FROM dbo.Ct_Product_Type where Product_Type_ID != 4 ORDER BY Product_Type_ID"></asp:SqlDataSource>
    <%--<asp:SqlDataSource ID="SqlDataSourceUnderwritingStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Status_Code, Detail FROM dbo.Ct_Underwrite_Table ORDER BY Status_Code"></asp:SqlDataSource>--%>
    <asp:SqlDataSource ID="SqlDataSourceCountry" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Country_Name FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceNationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Nationality "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Payment_Mode_List]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceRelationship" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Relationship_List]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Question ORDER BY Seq_Number ASC "></asp:SqlDataSource>
  
    <asp:SqlDataSource ID="SqlDataSourceChannel" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Channel where Status = 1 order by Created_On ASC"></asp:SqlDataSource>
      <!--- End Section Sqldatasource--->
</asp:Content>

