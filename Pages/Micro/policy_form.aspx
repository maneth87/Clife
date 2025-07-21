<%@ Page Title="Clife | Micro => Policy" Language="C#" MasterPageFile="~/Pages/Content.master" EnableEventValidation="false" CodeFile="policy_form.aspx.cs" Inherits="Pages_Business_policy_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <ul class="toolbar">

        <li>
            <!-- Button to trigger modal new miro policy form -->
            <input type="button" onclick="ShowNewPolicyModal()"  style="background: url('../../App_Themes/functions/add.png') no-repeat; border: none; height: 40px; width: 90px;" />

        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" runat="server"  data-toggle="modal" data-target="#mySearchPolicy" ImageUrl="~/App_Themes/functions/search.png" />
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
                    <a href="#" onclick="ShowEditInsurancePlan();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Insurance Plan</a>
                </li>
                <li style="float: left; width: 100%">
                    <a href="#" onclick="ShowEditBeneficiaries();" style="text-decoration: none; font-family: Arial; font-size: 10pt">Edit Beneficiaries</a>
                </li>

            </ul>
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnClear" runat="server"  Visible="true" ImageUrl="~/App_Themes/functions/clear.png" CausesValidation="False" OnClick="ImgBtnClear_Click" />
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
            </div>
            <input type="button" onclick="SavePolicy()"  style="background: url('../../App_Themes/functions/issue_policy.png') no-repeat; border: none; height: 40px; width: 100px;" />
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

        .modal.larger {
            width: 85%; /* desired relative width */
            left: 8%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
        }

        .auto-style2 {
            height: 37px;
        }

        .auto-style3 {
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
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }
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

        var total_share_benefit = 0;
        var total_share_benefit_edit = 0;


        //Tab
        jQuery(document).ready(function ($) {
            $('#tabs').tab();
            $("#tblPolicyContent *").attr('disabled', 'disabled');
        });

        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

            $('#Main_txtDateBirthEdit').datepicker().on('changeDate', function (dob_edit) {
                GetCustomerAgeEdit();
            });

            $('#Main_txtDateBirth').datepicker().on('changeDate', function (dob) {

                GetCustomerAge();

            });

            $('#Main_txtEntryDate').datepicker().on('changeDate', function (dob) {

                $('#Main_hdfEntryDate').val($('#Main_txtEntryDate').val());

            });

        });

        //Add New Policy Section//.....................................................................................................

        //Fill Data for new policy (OK click)
        function NewPolicy() {
            //Check barcode number
            var barcode = $('#Main_txtBarcode').val();
            var card = $('#Main_ddlCard option:selected').val();                     
            $('#Main_hdfSearchingView').val("");

            if (card == "0") {
                alert("Please select card.");
                $('#Main_lblCardValidate').show();
                return;
            } else {
                $('#Main_lblCardValidate').hide();
            }

            if (barcode == "") {
                alert("Please input barcode.");
                $('#Main_lblBarcodeValidate').show();
                return;
            } else {
                $('#Main_lblBarcodeValidate').hide();
            }

            Clear();

            $('#Main_txtBarcodeNumber').val(barcode);
            $('#Main_hdfBarcodeNumber').val(barcode);
                        
            //Default values
            $('#Main_txtInsuranceAmountRequired').val("1000");
            $('#Main_txtPremiumAmount').val("10");
            $('#Main_txtBenefitSharePercentage').val("100");
            $('#Main_lblTotalBenefitSharePercentage').text("Total: 0%");

            $('#Main_ddlCountry').val("KH");
            $('#Main_txtZipCode').val("855");
            $('#Main_hdfZipCode').val("855");
         
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/CheckBarcode",
                data: "{card:'" + card + "', barcode:'" + barcode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d != "1") {
                        alert("This card is invalid. Please select another card and try again.");
                        $('#Main_txtBarcodeNumber').val("");
                        $('#Main_hdfBarcodeNumber').val("");
                    } else {
                                              
                        $("#tblPolicyContent *").removeAttr('disabled');

                        $.ajax({
                            type: "POST",
                            url: "../../ProductWebService.asmx/GetProductMicro",
                            data: "{card:'" + card + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {

                                $('#Main_txtProduct').val(data.d.En_Title);
                                $('#Main_hdfProduct').val(data.d.Product_ID);
                                GetPayYear(data.d.Product_ID);
                                GetAssureYear(data.d.Product_ID, 0);
                                //GetNewPolicyNumber();
                                //GetMarketingCode();
                                $('#myModalNewPolicyForm').modal("hide");

                            }

                        });
                    }
                }

            });


        }

        //........................................Policy Search......................................................................................................

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
                    $('#Main_hdftotalagentrow').val(data.d.length);

                    $('#dvAgentList').setTemplate($("#jTemplateSaleAgent").html());
                    $('#dvAgentList').processTemplate(data);

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

            var total_agent_row = $('#Main_hdftotalagentrow').val();

            //Uncheck all other checkboxes
            for (var i = 1; i <= total_agent_row  ; i++) {
                if (i != row_index_agent_list) {
                    $('#' + i).prop('checked', false);
                }

            }

        };

        //Fill Selected Sale Agent to Textboxes
        function FillMarketingCode() {

            if (marketing_code == "") {
                alert('Please select a checkbox');
                return;
            }

            $('#Main_txtMarketingCode').val(marketing_code);
            $('#Main_txtMarketingName').val(marketing_name);
            $('#Main_hdfMarketingCode').val(marketing_code);
            $('#Main_hdfMarketingName').val(marketing_name);
        };


        //Search Policy By Barcode No
        function SearchPolicyByBarcodeNo() {
         
            var barcode_number = $('#Main_txtBarcodeNumberSearch').val();

            $('#dvPolicyList').empty();
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/SearchPolicyByBarcodeNoInternal",
                data: "{barcode_number:'" + barcode_number + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                  
                    $('#dvPolicyList').setTemplate($("#jTemplatePolicy").html());
                    $('#dvPolicyList').processTemplate(data);

                }

            });
        }

        //Search Policy By Policy No
        function SearchPolicyByPolicyNo() {
           
            var policy_number = $('#Main_txtPolicyNumberSearch').val();

            $('#dvPolicyList').empty();
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/SearchPolicyByPolicyNoInternal",
                data: "{policy_number:'" + policy_number + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#dvPolicyList').setTemplate($("#jTemplatePolicy").html());
                    $('#dvPolicyList').processTemplate(data);

                }

            });
        }

        //Search Policy By Customer Name
        function SearchPolicyByCustomerName() {
            var first_name = $('#Main_txtFirstNameSearch').val();
            var last_name = $('#Main_txtLastNameSearch').val();
           
            $('#dvPolicyList').empty();

            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/SearchPolicyByCustomerNameInternal",
                data: "{last_name:'" + last_name + "',first_name:'" + first_name + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#dvPolicyList').setTemplate($("#jTemplatePolicy").html());
                    $('#dvPolicyList').processTemplate(data);

                }

            });
        }

        //Search Policy By ID Card No
        function SearchPolicyByIDCardNo() {
            var id_type = $('#Main_ddlIDTypeSearch').val();
            var id_card_no = $('#Main_txtIDCardNoSearch').val();
           
            $('#dvPolicyList').empty();
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/SearchPolicyByIDCardInternal",
                data: "{id_type:'" + id_type + "',id_card_no:'" + id_card_no + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#dvPolicyList').setTemplate($("#jTemplatePolicy").html());
                    $('#dvPolicyList').processTemplate(data);

                }

            });

        }

        //Get Policy By ID
        var search_policy_id;
        function GetPolicy(policy_id, index, total) {

            if ($('#P' + index).is(':checked')) {
                search_policy_id = policy_id;
            } else {
                search_policy_id = "";
            }

            //Uncheck all other checkboxes
            for (var i = 0; i < total  ; i++) {
                if (i != index) {
                    $('#P' + i).prop('checked', false);
                }

            }
        }

        //Fill Searched Policy Data
        function FillSearchedData() {

            if (search_policy_id != "") {
                Clear();

                $("#Main_hdfPolicyID").val(search_policy_id);

                $('#Main_hdfSearchingView').val("Search Mode");

                //Get Policy single row info
                GetPolicySingleRow();

                $("#tblPolicyContent *").attr("disabled", "disabled");

                $('#Main_txtBarcodeNumberSearch').val("");

            } else {
                alert("Please select a checkbox");
            }

        }

        //Populate Policy Single Row Data
        function GetPolicySingleRow() {

            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/GetPolicySingleRow",
                data: "{policy_id:'" + search_policy_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_hdfPolicyID').val(data.d.Policy_Micro_ID);
                    $('#Main_txtPolicyNumber').val(data.d.Policy_Number);
                    $('#Main_hdfPolicyNumber').val(data.d.Policy_Number);
                  
                    $('#Main_txtMarketingCode').val(data.d.Sale_Agent_ID);
                    $('#Main_hdfMarketingCode').val(data.d.Sale_Agent_ID);
                    $('#Main_hdfBarcodeNumber').val(data.d.Barcode);
                    $('#Main_txtBarcodeNumber').val(data.d.Barcode);
                    $('#Main_txtMarketingName').val(data.d.Sale_Agent_Full_Name);
                    $('#Main_hdfMarketingName').val(data.d.Sale_Agent_Full_Name);

                    $('#Main_txtEntryDate').val(formatJSONDate(data.d.Issue_Date));
                    $('#Main_ddlCompanyMicro').val(data.d.Channel_Item_ID);

                    SetOffice(data.d.Channel_Item_ID, data.d.Channel_Location_ID);

                   
                    $('#Main_ddlIDType').val(data.d.ID_Type);
                
                    $('#Main_txtIDNumber').val(data.d.ID_Card);
                    $('#Main_txtSurnameKh').val(data.d.Khmer_Last_Name);

                    $('#Main_txtFirstNameKh').val(data.d.Khmer_First_Name);

                    $('#Main_txtSurnameEng').val(data.d.Last_Name);

                    $('#Main_txtFirstNameEng').val(data.d.First_Name);

                    $('#Main_ddlCountry').val(data.d.Country);

                    $('#Main_ddlGender').val(data.d.Gender);

                    $('#Main_txtDateBirth').val(formatJSONDate(data.d.Birth_Date));
                    $('#Main_hdfDateBirth').val(formatJSONDate(data.d.Birth_Date));
                    $('#Main_txtAddress1').val(data.d.Address1);
                    $('#Main_txtAddress2').val(data.d.Address2);
                    $('#Main_txtCity').val(data.d.Province);

                    $('#Main_txtZipCode').val(data.d.Zip_Code);
                    $('#Main_hdfZipCode').val(data.d.Zip_Code);

                    $('#Main_txtMobilePhone').val(data.d.Mobile_Phone1);

                    $('#Main_txtEmail').val(data.d.EMail);

                    $('#Main_txtProduct').val(data.d.Product);
                    $('#Main_hdfProduct').val(data.d.Product_ID);

                    $('#Main_txtTermInsurance').val(data.d.Assure_Year);
                    $('#Main_hdfTermInsurance').val(data.d.Assure_Year);
                    $('#Main_hdfAssureeAge').val(data.d.Age_Insure);
                    $('#Main_txtAssureeAge').val(data.d.Age_Insure);
                    $
                    $('#Main_txtPaymentPeriod').val(data.d.Pay_Year);
                    $('#Main_hdfPaymentPeriod').val(data.d.Pay_Year);

                    $('#Main_txtInsuranceAmountRequired').val(data.d.User_Sum_Insure);

                    $('#Main_txtPremiumAmount').val(data.d.User_Premium);

                    GetBenefits();

                }

            });

        }

        //Populate All Benefits for this policy
        function GetBenefits() {
           
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/GetBenefits",
                data: "{policy_id:'" + search_policy_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    row_index = 1;
                    total_share_benefit = 0;

                    //Remove previous beneficaries
                    RemoveBeneficaries();
                    var benefit_dob = "";              
                    var benefit_name = "";
                    var benefit_relation = "";
                    var benefit_share = "";

                    for (var i = 0; i < data.d.length; i++) {

                        var item = data.d[i];

                        $('#Main_txtBenefitDOB').val(formatJSONDate(item.Birth_Date));
                        $('#Main_hdfBenefitDOB').val("}" + $('#Main_hdfBenefitDOB').val() + item.Birth_Date);

                        $('#Main_txtBenefitName').val(item.Full_Name);
                        $('#Main_hdfBenefitName').val("}" + $('#Main_hdfBenefitName').val() + item.Full_Name);

                        $('#Main_ddlBenefitRelation').val(item.Relationship);
                        $('#Main_hdfBenefitRelation').val("}" + $('#Main_hdfBenefitRelation').val() + item.Relationship);

                        $('#Main_txtBenefitSharePercentage').val(item.Percentage);
                        $('#Main_hdfBenefitSharePercentage').val("}" + $('#Main_hdfBenefitSharePercentage').val() + item.Percentage);

                        AddNewBenefitRow();
                    }


                    $("#tblBenefitTable *").attr("disabled", "disabled");

                    //Clear                   
                    $('#Main_txtBenefitDOB').val("");                
                    $('#Main_txtBenefitName').val("");
                    $('#Main_ddlBenefitRelation').val($('#Main_ddlBenefitRelation option:first').val());
                    $('#Main_txtBenefitSharePercentage').val("");

                }

            });
        }


        //....................................End Policy Search......................................................................

        //Format Date in Jtemplate
        function formatJSONDate(jsonDate) {
            var value = jsonDate;
            if (value.substring(0, 6) == "/Date(") {
                var dt = new Date(parseInt(value.substring(6, value.length - 2)));
                var dtString = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                value = dtString;
            }

            return value;
        }

        //Get Zip Code
        function GetZipCode() {
            var country_value = $('#Main_ddlCountry').val();
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/GetZipCode",
                data: "{country_value:'" + country_value + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtZipCode').val(data.d);
                    $('#Main_hdfZipCode').val(data.d);

                }

            });
        }

        function GetOffice(company_id) {
            
            $.ajax({
                type: "POST",
                url: "../../ChannelWebService.asmx/GetChannelLocation",
                data: "{channel_item_id:'" + company_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_ddlOffice').setTemplate($("#jTemplateOffice").html());
                    $('#Main_ddlOffice').processTemplate(data);

                }

            });
        }


        function SetOffice(company_id, office_id) {

            $.ajax({
                type: "POST",
                url: "../../ChannelWebService.asmx/GetChannelLocation",
                data: "{channel_item_id:'" + company_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_ddlOffice').setTemplate($("#jTemplateOffice").html());
                    $('#Main_ddlOffice').processTemplate(data);

                    $('#Main_ddlOffice').val(office_id);
                }

            });
        }

        function GetOfficeID(office_id) {
            $('#Main_hdfChannelLocation').val(office_id);
        }

        //........................................New Policy Modal............................................................................

        //Show New Policy Modal
        function ShowNewPolicyModal() {
            $('#Main_ddlCard').val("1");
            $('#Main_lblCardValidate').hide();
            $('#Main_txtBarcode').val("");
            $('#Main_lblBarcodeValidate').hide();           
            $('#myModalNewPolicyForm').modal("show");
            
        }

        //Get next policy number
        //function GetNewPolicyNumber() {
        //    $.ajax({
        //        type: "POST",
        //        url: "../../PolicyWebService.asmx/GetNewPolicyNumber",
        //        data: "{}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (data) {
        //            $('#Main_txtPolicyNumber').val(data.d);
        //            $('#Main_hdfPolicyNumber').val(data.d);

        //        }

        //    });
        //}


        //Get marketing code by id
        function GetMarketingCode() {
            var id = $('#Main_hdfSaleAgentID').val();
            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetSaleAgent",
                data: "{id:'" + id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtMarketingCode').val(data.d.Sale_Agent_ID);
                    $('#Main_hdfMarketingCode').val(data.d.Sale_Agent_ID);
                    $('#Main_txtMarketingName').val(data.d.Full_Name);
                    $('#Main_hdfMarketingName').val(data.d.Full_Name);
                }

            });
        }
        
        //.......................................End New Policy Modal................................................................

        //.......................................Insurance Plan......................................................................
        //Get customer Age
        function GetCustomerAge() {
            var dob = $('#Main_txtDateBirth').val();
            $('#Main_hdfDateBirth').val(dob);

            if (dob != "" && dob != " " && dob != null) {
                //calculate customer age base on dob
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetCustomerAgeMicro",
                    data: "{dob:" + JSON.stringify(dob) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtAssureeAge').val(data.d);
                        $('#Main_hdfAssureeAge').val(data.d);
                    },
                    error: function (msg) {
                        alert("Problem in calculating age");
                    }
                });
            }
        }

        //Get customer Age Edit
        function GetCustomerAgeEdit() {
            var dob = $('#Main_txtDateBirthEdit').val();

            if (dob != "" && dob != " " && dob != null) {
                //calculate customer age base on dob
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetCustomerAgeMicro",
                    data: "{dob:" + JSON.stringify(dob) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtAssureeAgeEdit').val(data.d);

                    },
                    error: function (msg) {
                        alert("Problem in calculating age");
                    }
                });
            }
        }

        //Get Pay Year for (payment period)
        function GetPayYear(product_id) {
            
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetPayYear",
                data: "{product_id:" + JSON.stringify(product_id) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtPaymentPeriod').val(data.d);
                    $('#Main_hdfPaymentPeriod').val(data.d);
                }

            });
        };

        //Get Assure Year for (Term of Insurance)
        function GetAssureYear(product_id, customer_age) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetAssureYear",
                data: "{product_id:'" + product_id + "',customer_age:'" + customer_age + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtTermInsurance').val(data.d);
                    $('#Main_hdfTermInsurance').val(data.d);
                }

            });
        };

        //Get Premium
        function GetPremium() {

            var value = $('#Main_txtInsuranceAmountRequired').val();

            $.ajax({
                type: "POST",
                url: "../../CalculationWebService.asmx/GetPremiumMicro",
                data: "{amount:'" + value + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtPremiumAmount').val(data.d);

                }

            });
        };

        //Get Premium Edit
        function GetPremiumEdit() {

            var value = $('#Main_txtInsuranceAmountRequiredEdit').val();

            $.ajax({
                type: "POST",
                url: "../../CalculationWebService.asmx/GetPremiumMicro",
                data: "{amount:'" + value + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtPremiumAmountEdit').val(data.d);

                }

            });
        };

        //........................................End Insurance Plan.................................................................

        //..................................Beneficiaries............................................................................
        //Beneficiary Total Share     
        function GetTotalBenefitShare(obj) {
            total_share_benefit = 0;

            ValidateTextDecimal(obj);

            //recalculate total share benefit
            for (var i = 2; i <= row_index_benefit; i++) {
                var share = $('#Benefit_Share' + i).val();

                if (share != null) {
                    total_share_benefit = +total_share_benefit + +share;

                }

                if (total_share_benefit > 0) {
                    $('#Main_lblTotalBenefitSharePercentage').text("Total: " + total_share_benefit + "%");
                } else {
                    $('#Main_lblTotalBenefitSharePercentage').text("Total: 0%");
                }

            }

            if (total_share_benefit > 100) {
                alert("Sum of total share exceeds 100%");
            }

        };

        //Delete Benefit Row
        function DeleteBenefitRow(row_index) {
            var remove_share = $('#Benefit_Share' + row_index).val();

            total_share_benefit = total_share_benefit - remove_share;

            if (total_share_benefit > 0) {
                $('#Main_lblTotalBenefitSharePercentage').text("Total: " + total_share_benefit + "%");
            } else {
                $('#Main_lblTotalBenefitSharePercentage').text("Total: 0%");
            }

            $('#' + row_index).remove()
        }

        //Add Benefit Row
        var row_index_benefit = 1;
        function AddNewBenefitRow() {

            var benefit_name = $('#Main_txtBenefitName').val();

            var birth_date = $('#Main_txtBenefitDOB').val();

            if (birth_date === "") {
                alert("Require Date of Birth");
                return;
            }

            var relation_value = $('#Main_ddlBenefitRelation option:selected').val();

            var share = $('#Main_txtBenefitSharePercentage').val();

            var number_share = share;

            if (share === "") {
                share = 0;
            }

            if (+total_share_benefit + +number_share <= 100) {

                total_share_benefit = +total_share_benefit + +number_share;

                $('#Main_lblTotalBenefitSharePercentage').text("Total: " + total_share_benefit + "%");

                row_index_benefit += 1;

                var new_row = "<tr id='" + row_index_benefit + "' style='background-color: lightgray;'>"
                new_row += "<td>&nbsp;</td>"
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Name" + row_index_benefit + "' value='" + benefit_name + "' style='width: 93%; text-transform: uppercase; height: 30px' maxlength='100' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_DOB" + row_index_benefit + "' readonly='true' value='" + birth_date + "'  style='width: 86%; height: 30px' maxlength='30' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Relation" + row_index_benefit + "' readonly='true' value='" + relation_value + "' style='width: 92%; height: 30px' maxlength='100' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "               
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Share" + row_index_benefit + "' value='" + share + "' onkeyup='GetTotalBenefitShare(this)' onkeypress='return (event.keyCode != 13)' style='width: 85%; height: 30px' maxlength='5'/>"
                new_row += "</td> "
                new_row += "<td style='text-align:center'>"
                new_row += "<button type='button' class='btn btn-default btn-lg' style='height:40px' onclick='DeleteBenefitRow(" + row_index_benefit + ")'><span class='icon-trash'></span> Del</button>"

                new_row += "</td>"
                new_row += "<td width='2%'>&nbsp;</td></tr>"

                $('#tblBenefitTable').append(new_row);

                //Clear beneficary
                $('#Main_txtBenefitSharePercentage').val("");
                $('#Main_txtBenefitName').val("");
                $('#Main_txtBenefitDOB').val("");
                $('#Main_ddlBenefitRelation').prop('selectedIndex', 0);
            }
            else {
                alert("Sum of total benefit share exceeds 100%");
            }

        };

        //Remove Beneficaries
        function RemoveBeneficaries() {
            var rows = $('#tblBenefitTable tbody').children().length;

            //Delete  benefits
            while (rows > 1) {

                $('#tblBenefitTable tbody tr:last').remove();
                rows = $('#tblBenefitTable tbody').children().length;
            }
        }


        //..................................End Beneficiaries........................................................................

        //Save new policy
        function SavePolicy() {
            Page_ClientValidate();

            if (Page_IsValid) {

                if ($('#Main_hdfSearchingView').val() != "") {
                    alert("System can not save existing policy. Please fill new policy before saving.");
                    return;
                }

                //not save if share total benefit and proposed share not equal 100%
                if (total_share_benefit != 100) {
                    alert("This policy can not be saved. Total benefit share less than or greater than 100%.")
                    return;
                }

                //Save alert
                if (confirm('Are your sure you want to save this policy form?')) {


                    var benefit_names = "";
                    var benefit_relations = "";
                    var benefit_shares = "";
                    var benefit_dobs = "";                 
                    var count = 0;

                    for (i = 2, j = row_index_benefit; i <= j; i++) {

                        var bname = $('#Benefit_Name' + i).val();

                        if (bname == "") {
                            bname = "N/A";
                        }

                        var dob = $('#Benefit_DOB' + i).val();
                        var relation = $('#Benefit_Relation' + i).val();
                       
                        var share = $('#Benefit_Share' + i).val();

                        if (count != 0) {
                            benefit_names += bname + "}";
                            benefit_relations += relation + "}";
                            benefit_shares += share + "}";
                            benefit_dobs += dob + "}";
                       

                        } else {
                            benefit_names = bname + "}";
                            benefit_relations = relation + "}";
                            benefit_shares = share + "}";
                            benefit_dobs = dob + "}";
                            
                        }

                        count += 1;
                    }


                    //Benefit array         
                    $('#Main_hdfBenefitName').val(benefit_names);
                    $('#Main_hdfBenefitDOB').val(benefit_dobs);
                    $('#Main_hdfBenefitRelation').val(benefit_relations);                   
                    $('#Main_hdfBenefitShare').val(benefit_shares);

                    var btnSave = document.getElementById('<%= btnSave.ClientID %>'); //dynamically click button

                    btnSave.click();
                }




            } else {
                alert("Input is invalid. Please check for red *");
            }
        }

        //.......................................Edit Policy..........................................................................
        //Populate Edit Address Contact Modal
        function PopulateEditAddressContactModal() {

            $('#Main_txtAddress1Edit').val($('#Main_txtAddress1').val());
            $('#Main_txtAddress2Edit').val($('#Main_txtAddress2').val());
            $('#Main_txtProvinceEdit').val($('#Main_txtCity').val());
            $('#Main_txtZipCodeEdit').val($('#Main_txtZipCode').val());
            $('#Main_ddlCountryEdit').val($('#Main_ddlCountry option:selected').val());
            $('#Main_txtMobilePhoneEdit').val($('#Main_txtMobilePhone').val());
            $('#Main_txtEmailEdit').val($('#Main_txtEmail').val());
        }

        //Populate Edit Personal Details Modal
        function PopulateEditPersonalDetailsModal() {

            $('#Main_ddlIDTypeEdit').val($('#Main_ddlIDType option:selected').val());
            $('#Main_txtIDNoEdit').val($('#Main_txtIDNumber').val());
            $('#Main_txtSurnameKhEdit').val($('#Main_txtSurnameKh').val());
            $('#Main_txtFirstNameKhEdit').val($('#Main_txtFirstNameKh').val());
            $('#Main_txtSurnameEdit').val($('#Main_txtSurnameEng').val());
            $('#Main_txtFirstNameEdit').val($('#Main_txtFirstNameEng').val());
        
        }

        //Populate Edit Insurance Plan Modal
        function PopulateEditInsurancePlanModal() {

            $('#Main_txtProductEdit').val($('#Main_txtProduct').val());
            $('#Main_hdfProductEdit').val($('#Main_hdfProduct').val());
            $('#Main_txtTermInsuranceEdit').val($('#Main_txtTermInsurance').val());
            $('#Main_txtInsuranceAmountRequiredEdit').val($('#Main_txtInsuranceAmountRequired').val());
            $('#Main_txtAssureeAgeEdit').val($('#Main_txtAssureeAge').val());
            $('#Main_txtPaymentPeriodEdit').val($('#Main_txtPaymentPeriod').val());
            $('#Main_txtPremiumAmountEdit').val($('#Main_txtPremiumAmount').val());
            $('#Main_txtDateBirthEdit').val($('#Main_txtDateBirth').val());
            $('#Main_ddlGenderEdit').val($('#Main_ddlGender option:selected').val());
        }

        //Populate All Benefits (Edit)
        function GetBenefitsEdit() {

            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/GetBenefits",
                data: "{policy_id:'" + search_policy_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    row_index_benefit_edit = 1;
                    total_share_benefit_edit = 0;
                    $('#Main_lblTotalBenefitSharePercentageEdit').text("Total: 0%");

                    //Remove previous beneficaries
                    RemoveBeneficariesEdit();
                    var benefit_dob = "";                   
                    var benefit_name = "";
                    var benefit_relation = "";
                    var benefit_share = "";

                    for (var i = 0; i < data.d.length; i++) {

                        var item = data.d[i];

                        $('#Main_txtBenefitDOBEdit').val(formatJSONDate(item.Birth_Date));
                                              
                        $('#Main_txtBenefitNameEdit').val(item.Full_Name);

                        $('#Main_ddlBenefitRelationEdit').val(item.Relationship);

                        $('#Main_txtBenefitSharePercentageEdit').val(item.Percentage);

                        AddNewBenefitRowEdit();
                    }


                    //Clear                   
                    $('#Main_txtBenefitDOBEdit').val("");                
                    $('#Main_txtBenefitNameEdit').val("");
                    $('#Main_ddlBenefitRelationEdit').val($('#Main_ddlBenefitRelationEdit option:first').val());
                    $('#Main_txtBenefitSharePercentageEdit').val("");

                }

            });
        }

        //Beneficiary Total Share (Edit)  
        function GetTotalBenefitShareEdit(obj) {
            total_share_benefit_edit = 0;

            ValidateTextDecimal(obj);

            //recalculate total share benefit
            for (var i = 2; i <= row_index_benefit_edit; i++) {
                var share = $('#Benefit_Share_Edit' + i).val();

                if (share != null) {
                    total_share_benefit_edit = +total_share_benefit_edit + +share;

                }

                if (total_share_benefit_edit > 0) {
                    $('#Main_lblTotalBenefitSharePercentageEdit').text("Total: " + total_share_benefit_edit + "%");
                } else {
                    $('#Main_lblTotalBenefitSharePercentageEdit').text("Total: 0%");
                }

            }

            if (total_share_benefit_edit > 100) {
                alert("Sum of total share exceeds 100%");
            }

        };

        //Delete Benefit Row (Edit)
        function DeleteBenefitRowEdit(row_index) {
            var remove_share = $('#Benefit_Share_Edit' + row_index).val();

            total_share_benefit_edit = total_share_benefit_edit - remove_share;

            if (total_share_benefit_edit > 0) {
                $('#Main_lblTotalBenefitSharePercentageEdit').text("Total: " + total_share_benefit_edit + "%");
            } else {
                $('#Main_lblTotalBenefitSharePercentageEdit').text("Total: 0%");
            }

            $('#BenefitEdit' + row_index).remove()
        }

        //Add Benefit Row (Edit)
        var row_index_benefit_edit = 1;
        function AddNewBenefitRowEdit() {

            var benefit_name = $('#Main_txtBenefitNameEdit').val();

            var birth_date = $('#Main_txtBenefitDOBEdit').val();

            if (birth_date === "") {
                alert("Require Date of Birth");
                return;
            }

            var relation_value = $('#Main_ddlBenefitRelationEdit option:selected').val();

           
            var share = $('#Main_txtBenefitSharePercentageEdit').val();


            var number_share = share;

            if (share === "") {
                share = 0;
            }

            if (+total_share_benefit_edit + +number_share <= 100) {

                total_share_benefit_edit = +total_share_benefit_edit + +number_share;

                $('#Main_lblTotalBenefitSharePercentageEdit').text("Total: " + total_share_benefit_edit + "%");

                row_index_benefit_edit += 1;

                var new_row = "<tr id='BenefitEdit" + row_index_benefit_edit + "' style='background-color: lightgray;'>"
                new_row += "<td>&nbsp;</td>"
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Name_Edit" + row_index_benefit_edit + "' value='" + benefit_name + "' style='width: 93%; text-transform: uppercase; height: 30px' maxlength='100' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_DOB_Edit" + row_index_benefit_edit + "' readonly='true' value='" + birth_date + "'  style='width: 86%; height: 30px' maxlength='30' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Relation_Edit" + row_index_benefit_edit + "' readonly='true' value='" + relation_value + "' style='width: 92%; height: 30px' maxlength='100' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "               
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Share_Edit" + row_index_benefit_edit + "' value='" + share + "' onkeyup='GetTotalBenefitShareEdit(this)' onkeypress='return (event.keyCode != 13)' style='width: 85%; height: 30px' maxlength='5'/>"
                new_row += "</td> "
                new_row += "<td style='text-align:center'>"
                new_row += "<button type='button' class='btn btn-default btn-lg' style='height:40px' onclick='DeleteBenefitRowEdit(" + row_index_benefit_edit + ")'><span class='icon-trash'></span> Del</button>"

                new_row += "</td>"
                new_row += "<td width='2%'>&nbsp;</td></tr>"

                $('#tblBenefitTableEdit').append(new_row);

                //Clear beneficary
                $('#Main_txtBenefitSharePercentageEdit').val("");
                $('#Main_txtBenefitNameEdit').val("");             
                $('#Main_txtBenefitDOBEdit').val("");
                $('#Main_ddlBenefitRelationEdit').prop('selectedIndex', 0);
            }
            else {
                alert("Sum of total benefit share exceeds 100%");
            }

        };

        //Remove Beneficaries (Edit)
        function RemoveBeneficariesEdit() {
            var rows = $('#tblBenefitTableEdit tbody').children().length;

            //Delete  benefits
            while (rows > 1) {

                $('#tblBenefitTableEdit tbody tr:last').remove();
                rows = $('#tblBenefitTableEdit tbody').children().length;
            }
        }


        //Get Pay Year for (payment period)
        function GetPayYearEdit(product_id) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetPayYear",
                data: "{product_id:" + JSON.stringify(product_id) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtPaymentPeriodEdit').val(data.d);
                    $('#Main_hdfPaymentPeriodEdit').val(data.d);
                }

            });
        };

        //Get Assure Year for (Term of Insurance) (Edit)
        function GetAssureYearEdit(product_id, customer_age) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetAssureYear",
                data: "{product_id:'" + product_id + "',customer_age:'" + customer_age + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtTermInsuranceEdit').val(data.d);
                    $('#Main_hdfTermInsuranceEdit').val(data.d);
                }

            });
        };

        //ddlInsurancePlan Click (Edit)
        function ProcessInsurancePlanEdit(value) {

            var customer_age = $('#Main_hdfAssureeAgeEdit').val();

            GetAssureYearEdit(value, customer_age);
            GetPayYearEdit(value);
        };

        function EditAddressContact() {

            var policy_id = $('#Main_hdfPolicyID').val();
            var address1 = $('#Main_txtAddress1Edit').val();
            var address2 = $('#Main_txtAddress2Edit').val();
            var province = $('#Main_txtProvinceEdit').val();
            var zip_code = $('#Main_txtZipCodeEdit').val();
            var country = $('#Main_ddlCountryEdit option:selected').val();
            var mobile = $('#Main_txtMobilePhoneEdit').val();
            var email = $('#Main_txtEmailEdit').val();

            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/EditAddressAndContact",
                data: "{policy_id:'" + policy_id + "',address1:'" + address1 + "',address2:'" + address2 + "',province:'" + province + "',zip_code:'" + zip_code + "',country:'" + country + "',mobile:'" + mobile + "',email:'" + email + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d === "0") {
                        alert("Edit Address & Contact failed. Please try again.");

                    } else {
                        alert("Edit Address & Contact successful.");

                        $('#Main_txtAddress1').val($('#Main_txtAddress1Edit').val());
                        $('#Main_txtAddress2').val($('#Main_txtAddress2Edit').val());
                        $('#Main_txtCity').val($('#Main_txtProvinceEdit').val());
                        $('#Main_txtZipCode').val($('#Main_txtZipCodeEdit').val());
                        $('#Main_ddlCountry').val($('#Main_ddlCountryEdit option:selected').val());
                        $('#Main_txtMobilePhone').val($('#Main_txtMobilePhoneEdit').val());
                        $('#Main_txtEmail').val($('#Main_txtEmailEdit').val());

                        $('#myEditAddressAndContactModal').modal('hide');

                    }
                }

            });
        }

        function EditPersonalDetails() {

            var policy_id = $('#Main_hdfPolicyID').val();
            var id_type = $('#Main_ddlIDTypeEdit option:selected').val();
            var id_no = $('#Main_txtIDNoEdit').val();
            var surname_kh = $('#Main_txtSurnameKhEdit').val();
            var first_name_kh = $('#Main_txtFirstNameKhEdit').val();
            var surname_en = $('#Main_txtSurnameEdit').val();
            var first_name_en = $('#Main_txtFirstNameEdit').val();
       
                     
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/EditPersonalDetails",
                data: "{policy_id:'" + policy_id + "',id_type:'" + id_type + "',id_no:'" + id_no + "',surname_kh:'" + surname_kh + "',first_name_kh:'" + first_name_kh + "',surname_en:'" + surname_en + "',first_name_en:'" + first_name_en + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d === "0") {
                        alert("Edit Personal Details failed. Please try again.");

                    } else {
                        alert("Edit Personal Details successful.");

                        $('#Main_ddlIDType').val($('#Main_ddlIDTypeEdit option:selected').val());
                        $('#Main_txtIDNumber').val($('#Main_txtIDNoEdit').val());
                        $('#Main_txtSurnameKh').val($('#Main_txtSurnameKhEdit').val());
                        $('#Main_txtFirstNameKh').val($('#Main_txtFirstNameKhEdit').val());
                        $('#Main_txtSurnameEng').val($('#Main_txtSurnameEdit').val());
                        $('#Main_txtFirstNameEng').val($('#Main_txtFirstNameEdit').val());
                      
                        $('#myEditPersonalDetailsModal').modal('hide');

                    }
                }

            });
        }

        function EditBeneficiaries() {
            var policy_id = $('#Main_hdfPolicyID').val();
          
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/DeleteBenefitItems",
                data: "{policy_id:'" + policy_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    SaveEditBeneficiaries(policy_id);                  
                }

            });

        }


        //Save Beneficiary (Edit)
        function SaveEditBeneficiaries(policy_id) {
            
            var dobs = "";
            var names = "";
            var relations = "";
            var shares = "";

            //Loop Beneficiary Table
            var table = document.getElementById('tblBenefitTableEdit');

            rows = table.getElementsByTagName('tr');
            var i, j;
            var dob, name, relation, share;

            for (i = 1, j = row_index_benefit_edit; i <= j; i++) {
            
                dob = $('#Benefit_DOB_Edit' + i).val();
                name = $('#Benefit_Name_Edit' + i).val();
                relation = $('#Benefit_Relation_Edit' + i).val();
                share = $('#Benefit_Share_Edit' + i).val();
                               
                names += name + "}";
                relations += relation + "}";
                shares += share + "}";
                dobs += dob + "}";

            }
          

            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/SaveBenefit",
                data: "{policy_id:'" + policy_id + "',dob:'" + dobs + "',name:'" + names + "',relation:'" + relations + "',share:'" + shares + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d != "1") {
                        save_beneficiaries = "failed";
                        alert('Edit Benefit Failed. Please check it again. Benefit');
                        return;
                    } else {
                        alert("Edit Beneficiaries successfull.");
                        GetBenefits();
                        $('#myEditBeneficiariesModal').modal('hide');
                    }


                },
                error: function (msg) {
                    alert('Registration Failed. Please check it again. Benefit');

                    save_beneficiaries = "failed";
                    return; //exit function

                }
            });
        }

        function EditInsurancePlan() {
            var policy_id = $('#Main_hdfPolicyID').val();
            var dob = $('#Main_txtDateBirthEdit').val();
            var gender = $('#Main_ddlGenderEdit option:selected').val();
            var customer_age = $('#Main_txtAssureeAgeEdit').val();
            var term_insurance = $('#Main_txtTermInsuranceEdit').val();
            var pay_year = $('#Main_txtPaymentPeriodEdit').val();
            var sum_insured = $('#Main_txtInsuranceAmountRequiredEdit').val();
            var pay_mode = 0;
            var premium = $('#Main_txtPremiumAmountEdit').val();

            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/EditInsurancePlan",
                data: "{policy_id:'" + policy_id + "',customer_age:'" + customer_age + "',term_insurance:'" + term_insurance + "',pay_year:'" + pay_year + "',sum_insured:'" + sum_insured + "',pay_mode:'" + pay_mode + "',premium:'" + premium + "',dob:'" + dob + "',gender:'" + gender + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d === "0") {
                        alert("Edit Insurance Plan failed. Please try again.");

                    } else {
                        alert("Edit Insurance Plan successfull.");


                        $('#Main_txtDateBirth').val($('#Main_txtDateBirthEdit').val());
                        $('#Main_ddlGender').val($('#Main_ddlGenderEdit option:selected').val());
                        $('#Main_txtAssureeAge').val($('#Main_txtAssureeAgeEdit').val());

                        $('#Main_txtInsuranceAmountRequired').val($('#Main_txtInsuranceAmountRequiredEdit').val());

                        $('#Main_txtPremiumAmount').val($('#Main_txtPremiumAmountEdit').val());

                        $('#myEditInsurancePlanModal').modal('hide');

                    }
                }

            });
        }

        function ShowEditAddressContact() {
            if ($('#Main_hdfPolicyID').val() != "") {
                PopulateEditAddressContactModal();

                $('#myEditAddressAndContactModal').modal('show');
            } else {
                alert("No policy to edit");
            }
        }

        function ShowEditPersonalDetails() {
            if ($('#Main_hdfPolicyID').val() != "") {
                PopulateEditPersonalDetailsModal();

                $('#myEditPersonalDetailsModal').modal('show');
            } else {
                alert("No policy to edit");
            }
        }

        function ShowEditBeneficiaries() {
            if ($('#Main_hdfPolicyID').val() != "") {
                GetBenefitsEdit();

                $('#myEditBeneficiariesModal').modal('show');
            } else {
                alert("No policy to edit");
            }
        }


        function ShowEditInsurancePlan() {

            if ($('#Main_hdfPolicyID').val() != "") {

                PopulateEditInsurancePlanModal();

                var policy_id = $('#Main_hdfPolicyID').val();

                $('#myEditInsurancePlanModal').modal('show');
              
               
            } else {
                alert("No policy to edit");
            }
        }

        //Get Zip Code (edit)
        function GetZipCodeEdit() {
            var country_value = $('#Main_ddlCountryEdit').val();
            $.ajax({
                type: "POST",
                url: "../../MicroWebService.asmx/GetZipCode",
                data: "{country_value:'" + country_value + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtZipCodeEdit').val(data.d);
                }

            });
        }

        //.................................................End Edit Policy...........................................................

        function Clear() {
            $("#Main_hdfPolicyID").val("");

            $('#Main_txtPolicyNumber').val("");
            $('#Main_txtBarcodeNumber').val("");
            $('#Main_hdfBarcodeNumber').val("");
            $('#Main_hdfPolicyNumber').val("");
            
            $('#Main_txtMarketingCode').val("");
            $('#Main_hdfMarketingCode').val("");
            $('#Main_txtMarketingName').val("");
            $('#Main_hdfMarketingName').val("");

            $('#Main_ddlIDType').prop('selectedIndex', 0);
            $('#Main_txtIDNumber').val("");
            $('#Main_txtSurnameKh').val("");
            $('#Main_txtFirstNameKh').val("");
            $('#Main_txtSurnameEng').val("");
            $('#Main_txtFirstNameEng').val("");
            $('#Main_ddlGender').prop('selectedIndex', 0);
            $('#Main_txtDateBirth').val("");
            $('#Main_txtAddress1').val("");
            $('#Main_txtAddress2').val("");
            $('#Main_txtCity').val("");

            $('#Main_txtZipCode').val("");
            $('#Main_hdfZipCode').val("");

            $('#Main_ddlCountry').prop('selectedIndex', 0);

            $('#Main_txtMobilePhone').val("");
            $('#Main_txtEmail').val("");


            $('#Main_txtProduct').val("");
            $('#Main_txtInsuranceAmountRequired').val("");
            $('#Main_txtTermInsurance').val("");
            $('#Main_hdfTermInsurance').val("");
            $('#Main_hdfAssureeAge').val("");
            $('#Main_txtAssureeAge').val("");
            $('#Main_txtPaymentPeriod').val("");
            $('#Main_hdfPaymentPeriod').val("");

            $('#Main_txtPremiumAmount').val("");

            total_share_benefit = 0;
            total_share_benefit_edit = 0;

            //Remove beneficaries
            RemoveBeneficaries();
            RemoveBeneficariesEdit();
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

    <script id="jTemplatePolicy" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">No</th>
                    <th style="border-width: thin; border-style: solid;">Barcode</th>
                    <th style="border-width: thin; border-style: solid;">Policy No</th>
                    <th style="border-width: thin; border-style: solid;">Effective Date</th>
                    <th style="border-width: thin; border-style: solid;">ID. Type</th>
                    <th style="border-width: thin; border-style: solid;">ID. Card No</th>
                    <th style="border-width: thin; border-style: solid;">Last Name</th>
                    <th style="border-width: thin; border-style: solid;">First Name</th>
                    <th style="border-width: thin; border-style: solid;">Gender</th>
                    <th style="border-width: thin; border-style: solid;">Birth Date</th>

                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td>{ $T.row$index + 1 }</td>
                        <td>{ $T.row.Barcode }</td>
                        <td>{ $T.row.Policy_Number }</td>
                        <td>{ formatJSONDate($T.row.Effective_Date) }</td>
                        <td>{ $T.row.ID_Type }</td>
                        <td>{ $T.row.ID_Card }</td>
                        <td>{ $T.row.Last_Name }</td>
                        <td>{ $T.row.First_Name }</td>
                        <td>{ $T.row.Gender }</td>
                        <td>{ formatJSONDate($T.row.Birth_Date) }</td>
                        <td>
                            <input id="P{ $T.row$index }" type="checkbox" onclick="GetPolicy('{ $T.row.Policy_Micro_ID }', '{ $T.row$index }', '{ $T.row$total }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>

     <script id="jTemplateOffice" type="text/html">
         <option selected="selected" value='0'>.</option>
        {#foreach $T.d as record}          
             <option value='{ $T.record.Channel_Location_ID }'>{ $T.record.Office_Name }</option>
        {#/for}
           
    </script>

    <%-- Form Design Section--%>
    <h1>Policy Form</h1>

    <table id="tblPolicyContent" class="table-layout">
        <tr>
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Application Details</h3>

            </td>
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Personal Details of Proposed Insured</h3>

            </td>

        </tr>
        <tr>
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <table id="tblPolicyDetail" width="99%" style="margin-top: 15px; margin-bottom: 10px;">
                    <tr>
                        <td style="width: 21%; text-align: right">Barcode No.:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtBarcodeNumber" runat="server" ReadOnly="true" MaxLength="9" Width="95%"></asp:TextBox>
                        </td>
                        <td style="width: 15%">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorBarcode" runat="server" ErrorMessage="Require Barcode No" ControlToValidate="txtBarcodeNumber" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>                    
                    <tr>
                        <td style="text-align: right">Policy No.:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtPolicyNumber" runat="server" ReadOnly="True" Width="95%"></asp:TextBox>
                            <asp:HiddenField ID="hdfPolicyNumber" runat="server" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Payment Code:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtPaymentCode" runat="server" MaxLength="50" Width="95%"></asp:TextBox>
                            
                        </td>
                        <td></td>
                    </tr>
                   <%-- <tr>
                        <td style="text-align: right">Date of Signature:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtDateSignature" runat="server" ReadOnly="true" MaxLength="15"></asp:TextBox>
                            <asp:HiddenField ID="hdfDateSignature" runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDateSignature" runat="server" ControlToValidate="txtDateSignature" ErrorMessage="Require Date of Signature" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            (DD-MM-YYYY)
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorDateSignature" runat="server" ErrorMessage="Invalid Date" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" ForeColor="Red" ControlToValidate="txtDateSignature"></asp:RegularExpressionValidator>
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="text-align: right">Date of Entry:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtEntryDate" runat="server" onkeypress="return false;" Width="95%"></asp:TextBox>
                            <asp:HiddenField ID="hdfEntryDate" runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEntryDate" runat="server" ErrorMessage="Require Date of Entry" ControlToValidate="txtEntryDate" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align:middle;">Marketing Code:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtMarketingCode" runat="server" ReadOnly="True" Height="25px" Width="95%"></asp:TextBox>
                            <asp:HiddenField ID="hdfMarketingCode" runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorMarketingCode" runat="server" ErrorMessage="Require Maketing Code" ControlToValidate="txtMarketingCode" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align:middle;">&nbsp;Name of Marketing:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtMarketingName" runat="server" ReadOnly="True" Height="25px" Width="95%"></asp:TextBox>
                            <asp:HiddenField ID="hdfMarketingName" runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorNameOfMarketing" runat="server" ControlToValidate="txtMarketingName" ErrorMessage="Require Name of Marketing" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <input type="button" data-toggle="modal" data-target="#myMarketingCodeList"  style="background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />
                        </td>
                    </tr>
                     <tr>
                        <td style="text-align: right; vertical-align:middle;">Company (Micro):</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlCompanyMicro" Height="35px" Width="99.3%" AppendDataBoundItems="true" runat="server" onchange="GetOffice(this.value);" DataSourceID="SqlDataSourceCompany" DataTextField="Channel_Name" DataValueField="Channel_Item_ID">
                                <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                            </asp:DropDownList>                           
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompanyMicro" runat="server" ControlToValidate="ddlCompanyMicro" InitialValue="0" ErrorMessage="Require Company" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                           
                        </td>
                    </tr>
                     <tr>
                        <td style="text-align: right; vertical-align:middle;">Office:</td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlOffice" Height="35px" Width="99.3%" runat="server" AppendDataBoundItems="true" onchange="GetOfficeID(this.value);">
                                <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                            </asp:DropDownList>                           
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorOffice" runat="server" ControlToValidate="ddlOffice"  ErrorMessage="Require Office" ForeColor="Red" Text="*" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                            
                        </td>
                    </tr>
                </table>

            </td>
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">

                <table width="99%" style="margin-top: 15px; margin-bottom: 10px;">
                    <tr>
                        <td style="width: 25%; text-align: right">I.D Type:</td>
                        <td style="width: 25%">
                            <asp:DropDownList ID="ddlIDType" runat="server" class="span2">
                                <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                <asp:ListItem Value="1">Passport</asp:ListItem>
                                <asp:ListItem Value="2">Visa</asp:ListItem>
                                <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td style="text-align: right" class="auto-style3">I.D No.:</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtIDNumber" class="span2" runat="server" MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorIDNumber" runat="server" ErrorMessage="Require I.D No." ControlToValidate="txtIDNumber" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align:middle">Surname in Khmer:</td>
                        <td>
                            <asp:TextBox ID="txtSurnameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>

                        </td>
                        <td style="text-align: right; vertical-align:middle;" class="auto-style3">First Name in Khmer:</td>
                        <td>
                            <asp:TextBox ID="txtFirstNameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align:middle;">Surname in English:</td>
                        <td>
                            <asp:TextBox ID="txtSurnameEng" class="span2" runat="server" Height="25px" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorSurnameEng" runat="server" ErrorMessage="Require Surname" ControlToValidate="txtSurnameEng" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>

                        </td>
                        <td style="text-align: right; vertical-align:middle;" class="auto-style3">First Name in English:</td>
                        <td>
                            <asp:TextBox ID="txtFirstNameEng" class="span2" runat="server" Height="25px" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorFirstNameEng" runat="server" ErrorMessage="Require First Name" ControlToValidate="txtFirstNameEng" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="auto-style2">Gender:</td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="ddlGender" class="span2" runat="server">
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="0">Female</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td class="auto-style5"></td>
                    </tr>
                    <%--<tr>
                        <td style="text-align: right" class="auto-style2">Marital Status:</td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="ddlMaritalStatus" class="span2" runat="server">
                                <asp:ListItem Value="1">Single</asp:ListItem>
                                <asp:ListItem Value="2">Widowed</asp:ListItem>
                                <asp:ListItem Value="3">Married</asp:ListItem>
                                <asp:ListItem Value="4">Divorced</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td class="auto-style5"></td>
                    </tr>--%>
                    <tr>
                        <td style="text-align: right">Date of Birth:</td>
                        <td>

                            <asp:TextBox ID="txtDateBirth" runat="server" MaxLength="15" CssClass="span2" onkeypress="return false;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDateOfBirth" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtDateBirth" ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            (DD-MM-YYYY)
                            <asp:HiddenField ID="hdfDateBirth" runat="server" />
                        </td>
                        <td class="auto-style3" style="padding-top: 2px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Date" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" ControlToValidate="txtDateBirth" ForeColor="Red"></asp:RegularExpressionValidator>
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
                            <li class="active"><a href="#mailling" data-toggle="tab">Mailling Address</a></li>
                            <li><a href="#insuranceplan" data-toggle="tab">Insurance Plan</a></li>
                            <li><a href="#beneficiaries" data-toggle="tab">Beneficiaries</a></li>

                        </ul>
                        <div id="my-tab-content" class="tab-content">
                            <div class="tab-pane active" id="mailling">
                                <%--Mailling Address--%>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 14.8%; text-align: right;vertical-align:middle;">Address:</td>
                                        <td style="width: 85.2%">
                                            <asp:TextBox ID="txtAddress1" Width="96%" runat="server" MaxLength="50" Height="25px"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="width: 14.8%; text-align: right;vertical-align:middle;">:</td>
                                        <td style="width: 85.2%">
                                        
                                            <asp:TextBox ID="txtAddress2" Width="96%" runat="server" MaxLength="50" Height="25px"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; vertical-align:middle;">Province/City:</td>
                                        <td>
                                            <asp:TextBox ID="txtCity" Width="96%" runat="server" MaxLength="50" Height="25px"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Zip Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtZipCode" Width="96%" runat="server" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                            <asp:HiddenField ID="hdfZipCode" runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Country:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlCountry" Width="97.3%" Height="25px" onchange="GetZipCode();" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceCountry" DataTextField="Country_Name" DataValueField="Country_ID">
                                                <asp:ListItem Value="0">.</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" ErrorMessage="Require Country" ControlToValidate="ddlCountry" InitialValue="0" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="text-align: right">Mobile Phone:</td>
                                        <td>
                                            <asp:TextBox ID="txtMobilePhone" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">E-mail:</td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" Width="96%" runat="server" MaxLength="125"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ErrorMessage="Invalid E-mail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>

                            <div class="tab-pane" id="insuranceplan">
                                <%--Insurance Plan--%>

                                <table width="100%" style="border-right: 1pt solid #d5d5d5;">
                                    <tr>
                                        <td style="width: 14.8%; text-align: right">Assuree Age:</td>
                                        <td style="width: 85.2%">

                                            <asp:TextBox ID="txtAssureeAge" Width="94%" runat="server" ReadOnly="True"></asp:TextBox>&nbsp;Years
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorAssureeAge" runat="server" ErrorMessage="Require Assuree Age" Text="*" ControlToValidate="txtAssureeAge" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfAssureeAge" runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Type of Insurance Plan:</td>
                                        <td>
                                            <asp:TextBox ID="txtProduct" Width="94%" ReadOnly="true" runat="server">                                              
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfProduct" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Term of Insurance:</td>
                                        <td>
                                            <asp:TextBox ID="txtTermInsurance" runat="server" Width="94%" ReadOnly="True" onkeyup="ValidateNumber(this); InputTermInsurance();" MaxLength="3"></asp:TextBox>&nbsp;Years
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorTermOfInsurance" runat="server" ErrorMessage="Require Term of Insurance" ControlToValidate="txtTermInsurance" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfTermInsurance" runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Payment Period:</td>
                                        <td>
                                            <asp:TextBox ID="txtPaymentPeriod" Width="94%" runat="server" ReadOnly="True"></asp:TextBox>&nbsp;Years
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPaymentPeriod" runat="server" ErrorMessage="Require Payment Period" ControlToValidate="txtPaymentPeriod" Text="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfPaymentPeriod" runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Insurance Amount Required:</td>
                                        <td>
                                            <asp:TextBox ID="txtInsuranceAmountRequired" Width="94%" runat="server" MaxLength="5" onkeyup="ValidateTextDecimal(this); GetPremium();" onchange="ValidateTextDecimal(this); GetPremium();"></asp:TextBox>&nbsp;USD.
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorInsruanceAmountRequired" runat="server" ErrorMessage="Require Insurance Amount Required" ControlToValidate="txtInsuranceAmountRequired" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Premium Amount:</td>
                                        <td>
                                            <asp:TextBox ID="txtPremiumAmount" Width="94%" runat="server" MaxLength="12" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>&nbsp;USD.
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPremiumAmount" runat="server" ErrorMessage="Require Premium Amount" ControlToValidate="txtPremiumAmount" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>

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
                                                    <td width="2%"></td>
                                                    <td style="width: 18%">
                                                        <div style="padding-bottom: 5px">Surname and Name</div>
                                                        <asp:TextBox ID="txtBenefitName" Style="width: 93%; text-transform: uppercase;" runat="server" MaxLength="100" Height="30"></asp:TextBox>

                                                    </td>
                                                    <td style="width: 10%">
                                                        <div style="padding-bottom: 5px">Date of Birth</div>
                                                        <asp:TextBox ID="txtBenefitDOB" Style="width: 86%" runat="server" onkeypress="return false;" CssClass="datepicker" MaxLength="30" Height="30"></asp:TextBox>

                                                    </td>
                                                    <td style="width: 12%">
                                                        <div style="padding-bottom: 5px">Relation</div>
                                                        <asp:DropDownList ID="ddlBenefitRelation" Style="width: 99%" Height="40" runat="server" DataSourceID="SqlDataSourceRelationship" DataTextField="Relationship" DataValueField="Relationship"></asp:DropDownList>

                                                    </td>
                                                   <%-- <td style="width: 20%">
                                                        <div style="padding-bottom: 5px">Address</div>
                                                        <asp:TextBox ID="txtBenefitAddress" Style="width: 97%" runat="server" MaxLength="100" Height="30"></asp:TextBox>

                                                    </td>--%>
                                                    <td style="width: 10%">
                                                        <div style="padding-bottom: 5px">Share (%)</div>
                                                        <asp:TextBox ID="txtBenefitSharePercentage" Style="width: 85%" MaxLength="5" runat="server" Height="30" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>&nbsp;
                                                    
                                                    </td>
                                                    <td style="vertical-align: middle; padding-bottom: 8px; text-align: center; width: 5%">
                                                        <button type="button" class="btn btn-default btn-lg" style="width: 69px; height: 40px" onclick="AddNewBenefitRow();">
                                                            <span class="icon-ok"></span>Add
                                                        </button>
                                                    </td>
                                                    <td width="2%"></td>
                                                </tr>

                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 60%"></td>
                                                    <td style="width: 8%; text-align: center;">
                                                        <asp:Label ID="lblTotalBenefitSharePercentage" runat="server" Text="Total: 0%"></asp:Label>
                                                    </td>
                                                    <td width="12%"></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>

    </table>

    <!-- Modal New Policy Form -->
    <div id="myModalNewPolicyForm" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelNewPolicyHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="myModalLabelNewPolicyHeader">New Policy Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="width: 100%; text-align: left;">

                <tr>
                    <td style="vertical-align: middle; width: 80px;">Card:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlCard" runat="server" Width="96%">
                            <asp:ListItem Value="0">.</asp:ListItem>
                            <asp:ListItem Value="1">Term One Card</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:Label ID="lblCardValidate" Text="*" ForeColor="Red" Style="display: none;" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle">Barcode:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtBarcode" runat="server" Width="93%"></asp:TextBox>
                        &nbsp;
                        <asp:Label ID="lblBarcodeValidate" Text="*" ForeColor="Red" Style="display: none;" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="NewPolicy();" value="OK" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal New Policy Form-->


    <!-- Modal Search Policy -->
    <div id="mySearchPolicy" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPolicyHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H2">Search Policy Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <ul class="nav nav-tabs" id="myTabPolicySearch">
                <li class="active"><a href="#SBarcodeNo" data-toggle="tab" style="text-decoration: none;">Search By Barcode No</a></li>
                <li><a href="#SPolicyNo" data-toggle="tab" style="text-decoration: none;">Search By Policy No</a></li>
                <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none;">Search By Customer Name</a></li>
                <li><a href="#SIDCardNo" data-toggle="tab" style="text-decoration: none;">Search By ID. Card No</a></li>
            </ul>

            <div class="tab-content" style="height: 60px; overflow: hidden;">
                <div class="tab-pane active" id="SBarcodeNo">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 13%; vertical-align: middle">Barcode No:</td>
                            <td style="width: 30%; vertical-align: bottom">
                                <asp:TextBox ID="txtBarcodeNumberSearch" Width="90%" runat="server"></asp:TextBox>

                            </td>
                            <td style="width: 56%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchPolicyByBarcodeNo();" value="Search" />
                            </td>

                        </tr>
                    </table>
                    <hr />
                </div>
                <div class="tab-pane active" id="SPolicyNo">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 13%; vertical-align: middle">Policy No:</td>
                            <td style="width: 30%; vertical-align: bottom">
                                <asp:TextBox ID="txtPolicyNumberSearch" Width="90%" runat="server"></asp:TextBox>

                            </td>
                            <td style="width: 56%; vertical-align: top">
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchPolicyByPolicyNo();" value="Search" />
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
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchPolicyByCustomerName();" value="Search" />
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
                                <input type="button" class="btn" style="height: 27px;" onclick="SearchPolicyByIDCardNo();" value="Search" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>

            </div>
            <div id="dvPolicyList"></div>
        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillSearchedData();" data-dismiss="modal" value="Select" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Search Application-->

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
                    <td style="vertical-align: middle; text-align: right;">Mobile no:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtMobilePhoneEdit" Width="90%" runat="server" onkeyup="ValidateNumber(this);" MaxLength="50"></asp:TextBox>
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
                    <td style="vertical-align: middle; width: 120px; text-align: right;">I.D. No:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtIDNoEdit" Width="90%" runat="server" MaxLength="12"></asp:TextBox>
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
                    <td style="vertical-align: middle; width: 130px; text-align: right;">Surname in English:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtSurnameEdit" Width="90%" runat="server" MaxLength="50" Style="text-transform: uppercase"></asp:TextBox>
                    </td>
                    <td style="vertical-align: middle; width: 130px; text-align: right;">First Name in English:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtFirstNameEdit" Width="90%" runat="server" MaxLength="50" Style="text-transform: uppercase"></asp:TextBox>
                    </td>
                </tr>

                <%--<tr>
                    <td style="vertical-align: middle; width: 120px; text-align: right;">Marital Status:</td>
                    <td style="vertical-align: bottom">
                        <asp:DropDownList ID="ddlMaritalStatusEdit" class="span2" runat="server">
                            <asp:ListItem Value="1">Single</asp:ListItem>
                            <asp:ListItem Value="2">Widowed</asp:ListItem>
                            <asp:ListItem Value="3">Married</asp:ListItem>
                            <asp:ListItem Value="4">Divorced</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>--%>

            </table>

        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditPersonalDetails();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>

        </div>
    </div>
    <!--End Modal Edit Personal Details-->

    <!-- Modal Edit Benefitciaries -->
    <div id="myEditBeneficiariesModal" class="modal hide fade larger" tabindex="-1" role="dialog" aria-labelledby="myModalEditBeneficiariesHeader" aria-hidden="true">
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
                                <td width="1%"></td>
                                <td style="width: 18%">
                                    <div style="padding-bottom: 5px">Surname and Name</div>
                                    <asp:TextBox ID="txtBenefitNameEdit" Style="width: 93%; text-transform: uppercase;" runat="server" MaxLength="100" Height="30"></asp:TextBox>

                                </td>
                                <td style="width: 10%">
                                    <div style="padding-bottom: 5px">Date of Birth</div>
                                    <asp:TextBox ID="txtBenefitDOBEdit" Style="width: 86%" runat="server" onkeypress="return false;" CssClass="datepicker" MaxLength="30" Height="30"></asp:TextBox>

                                </td>
                                <td style="width: 12%">
                                    <div style="padding-bottom: 5px">Relation</div>
                                    <asp:DropDownList ID="ddlBenefitRelationEdit" Style="width: 99%" Height="40" runat="server" DataSourceID="SqlDataSourceRelationship" DataTextField="Relationship" DataValueField="Relationship"></asp:DropDownList>

                                </td>
                             <%--   <td style="width: 20%">
                                    <div style="padding-bottom: 5px">Address</div>
                                    <asp:TextBox ID="txtBenefitAddressEdit" Style="width: 97%" runat="server" MaxLength="100" Height="30"></asp:TextBox>

                                </td>--%>
                                <td style="width: 10%">
                                    <div style="padding-bottom: 5px">Share (%)</div>
                                    <asp:TextBox ID="txtBenefitSharePercentageEdit" Style="width: 85%" MaxLength="5" runat="server" Height="30" onkeyup="ValidateTextDecimal(this);"></asp:TextBox>&nbsp;
                                                    
                                </td>
                                <td style="vertical-align: middle; padding-bottom: 8px; text-align: center; width: 5%">
                                    <button type="button" class="btn btn-default btn-lg" style="width: 69px; height: 40px" onclick="AddNewBenefitRowEdit();">
                                        <span class="icon-ok"></span>Add
                                    </button>
                                </td>
                                <td width="1%"></td>
                            </tr>

                        </table>
                        <table width="100%">
                            <tr>
                                <td style="width: 60%"></td>
                                <td style="width: 8%; text-align: center;">
                                    <asp:Label ID="lblTotalBenefitSharePercentageEdit" runat="server" Text="Total: 0%"></asp:Label>
                                </td>
                                <td width="12%"></td>
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
    <!--End Modal Edit Benefitiaries-->

    <!-- Modal Edit Insurance Plan -->
    <div id="myEditInsurancePlanModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalEditInsurancePlanHeader" aria-hidden="true">
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
                        <asp:DropDownList ID="ddlGenderEdit" Width="88.1%" runat="server">
                            <asp:ListItem Value="1">Male</asp:ListItem>
                            <asp:ListItem Value="0">Female</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Date of Birth:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtDateBirthEdit" runat="server" Width="85%"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Assuree Age:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtAssureeAgeEdit" ReadOnly="true" Width="85%" runat="server" ValidationGroup="3"></asp:TextBox>

                        &nbsp;Years
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Type of Insurance Plan:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtProductEdit" Width="85%" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Term of Insurance:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtTermInsuranceEdit" runat="server" Width="85%" ReadOnly="True" MaxLength="3" ValidationGroup="3"></asp:TextBox>

                        &nbsp;Years
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Payment Period:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPaymentPeriodEdit" Width="85%" runat="server" ReadOnly="true" ValidationGroup="3"></asp:TextBox>

                        &nbsp;Years
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Insurance Amount Required:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtInsuranceAmountRequiredEdit" Width="85%" runat="server" MaxLength="5" onkeyup="ValidateTextDecimal(this); GetPremiumEdit()" ValidationGroup="3"></asp:TextBox>

                        &nbsp;USD
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle; width: 160px; text-align: right;">Premium Amount:</td>
                    <td style="vertical-align: bottom">
                        <asp:TextBox ID="txtPremiumAmountEdit" Width="85%" runat="server" MaxLength="6" onkeyup="ValidateTextDecimal(this);" ValidationGroup="3"></asp:TextBox>
                        &nbsp;USD
                        
                    </td>
                </tr>

            </table>
        </div>
        <div class="modal-footer">
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="EditInsurancePlan();" value="OK" />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Edit Insurance Plan -->

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

    <%-- Section Hidenfields Initialize  --%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
    <asp:HiddenField ID="hdfPolicyID" runat="server" />
    <asp:HiddenField ID="hdfBarcodeNumber" runat="server" />
    <asp:HiddenField ID="hdfSaleAgentID" runat="server" />
    <asp:HiddenField ID="hdfSearchingView" runat="server" />

    <asp:HiddenField ID="hdftotalagentrow" runat="server" />
    <asp:HiddenField ID="hdfBenefitCount" runat="server" />

    <asp:HiddenField ID="hdfChannelChannelItem" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelLocation" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelItem" runat="server" Value="0" />

    <asp:HiddenField ID="hdfBenefitSeqNumber" runat="server" />
    <asp:HiddenField ID="hdfBenefitName" runat="server" />
    <asp:HiddenField ID="hdfBenefitDOB" runat="server" />
    <asp:HiddenField ID="hdfBenefitRelation" runat="server" />
    <asp:HiddenField ID="hdfBenefitAddress" runat="server" />
    <asp:HiddenField ID="hdfBenefitShare" runat="server" />

    <%-- End Section Hidenfields Initialize  --%>

    <!--- Section Sqldatasource--->

    <asp:SqlDataSource ID="SqlDataSourceCountry" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Country_Name FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceRelationship" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Relationship_List]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceInsurancePlan" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_ID, En_Title FROM dbo.Ct_Product WHERE Product_ID = 'T1011' "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceCompany" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand=" select Ct_Channel_Item.Channel_Name, Ct_Channel_Item.Channel_Item_ID from Ct_Channel_Item INNER JOIN Ct_Channel_Channel_Item ON Ct_Channel_Channel_Item.Channel_Item_ID = Ct_Channel_Item.Channel_Item_ID where Ct_Channel_Channel_Item.Channel_Sub_ID = 4"></asp:SqlDataSource>

    <!--- End Section Sqldatasource--->
</asp:Content>
