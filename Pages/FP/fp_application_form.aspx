<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fp_application_form.aspx.cs" EnableEventValidation="false" Inherits="Pages_FP_fp_application_form" MasterPageFile="~/Pages/Content.master" %>

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
           

        </li>
        <li>
            <asp:ImageButton ID="ImgBtnClear" runat="server"  Visible="true" ImageUrl="~/App_Themes/functions/clear.png" CausesValidation="False" OnClick="ImgBtnClear_Click" />
        </li>
        <li>
            <div style="display:none;">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
            </div>
            <asp:ImageButton runat="server" ID="ibtSave" ImageUrl="~/App_Themes/functions/save.png" OnClick="ibtSave_Click" OnClientClick="validpage();" />
           
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
        //mameth
        var planId;
        var age1 =0;
        var age2 = 0;
        var paymentMode ;
        var gender1;
        var gender2;

        //Tab
        jQuery(document).ready(function ($) {
            $('#tabs').tab();
           // $("#tblAppContent *").attr('disabled', 'disabled');
        });

        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

            $('#Main_txtDateBirthEdit').datepicker().on('changeDate', function (dob_edit) {
                CalculateInsuranceEdit();
            });

            //policy owner date of birth
            $('#Main_txtDateBirth').datepicker().on('changeDate', function (dob) {

                if ($('#Main_ddlPlan').val() != "0" && $('#Main_ddlPaymentMode').val() > -1) {
                    
                    // getPlan($('#Main_ddlPlan').val());
                    getPlan();
                   
                    GetPremium();
                }

            });

            $('#Main_txtDateEntry').datepicker().on('changeDate', function (entry) {
                $('#Main_hdfDateEntry').val($('#Main_txtDateEntry').val());
                CalculateInsuranceNoAlert();

            });

            $('#Main_txtDOBLife2').datepicker().on('changeDate', function () {
            });

            $('#Main_txtDOBLife3').datepicker().on('changeDate', function () {
            });

            $('#Main_txtDOBLife4').datepicker().on('changeDate', function () {
            });

            //on select changed plan
            $('#Main_ddlPlan1').change(function () {
                var str = $(this).find(":selected").text();
                planId= $(this).val();

                age1 = $('#Main_hflAgeLife1').val();
                age2 = $('#Main_hflAgeLife2').val();
               
                $('#Main_hflPaymentMode').val($('#Main_ddlPaymentMode').val());
                $('#Main_hflGender').val($('#Main_ddlGender').val());
              
                $('#Main_hflGenderLife2').val($('#Main_ddlGenderLife2').val());
               
                var paymentMode = $('#Main_hflPaymentMode').val();
                var gender1 = $('#Main_hflGender').val();
                var gender2 = $('#Main_hflGenderLife2').val();

                if (this.selectedIndex > 0) {
                  
                    //check required fields
                    if ($('#Main_txtDateEntry').val() == "") {
                        alert('Date of Entry is required.');
                        //reset dropdownlist to select index 0
                        $('#Main_ddlPlan').prop('selectedIndex', 0);
                        return;
                    }
                    else if ($('#Main_txtDateBirth').val() == "")
                    {
                        alert('Date of Birth is required.');
                        $('#Main_ddlPlan').prop('selectedIndex', 0);
                        return;
                    }
                    else if ($('#Main_txtDOBLife2').val() == "") {
                        alert('Date of Birth Insured Life 2 is required.');
                        $('#Main_ddlPlan').prop('selectedIndex', 0);
                        return;
                    }
                    else {
                            //get plan detail 
                        // getPlan(planId);
                        getPlan();

                        var a = $('#Main_ddlPaymentMode').val();
                        
                        if (a > -1)
                        {
                            alert(a);
                           // alert('Age1:' +age1 + ' Age2:' + age2 + '  Gender1:' + gender1 + '  Gender2:' + gender2 +  'PaymentMode:' + paymentMode );
                           // GetPremium("FP", age1, gender1, paymentMode, "1"); //premuim life 1
                           // GetPremium("FP", age2, gender2, paymentMode, "2");//premium life 2
                            GetPremium();
                            //total premium
                            totalPremium();
                        }
                            
                    }
                   
                }
                else {
                    //reset 
                    $('#Main_txtSumInsuredLife1').val("");
                    $('#Main_txtSumInsuredLife2').val("");
                    $('#Main_txtSumInsuredLife3').val("");
                    $('#Main_txtSumInsuredLife4').val("");

                    $('#Main_ddlPaymentMode').prop('selectedIndex', 0);

                    //display coverage peroid
                    $('#Main_txtCoverageYearLife1').val("");
                    $('#Main_txtCoverageYearLife2').val("");
                    $('#Main_txtCoverageYearLife3').val("");
                    $('#Main_txtCoverageYearLife4').val("");

                    //display payment peroid
                    $('#Main_txtPaymentPeroidLife1').val("");
                    $('#Main_txtPaymentPeroidLife2').val("");
                    $('#Main_txtPaymentPeroidLife3').val("");
                    $('#Main_txtPaymentPeroidLife4').val("");

                    //display age
                    $('#Main_txtAgeLife1').val("");
                    $('#Main_txtAgeLife2').val("");

                    $('#Main_hflAgeLife1').val("");
                    $('#Main_hflAgeLife2').val("");

                    //dispay premium
                    $('#Main_txtPremiumLife1').val("");
                    $('#Main_txtPremiumLife2').val("");
                    $('#Main_txtOriginalPremiumLife1').val("");
                    $('#Main_txtOriginalPremiumLife2').val("");
                    $('#Main_txtTotalPremium').val("");
                    $('#Main_txtTotalOriginalPremium').val("");

                    $('#hflPremiumLife1').val('');
                    $('#hflPremiumLife2').val('');
                    $('#hflOriginalPremiumLife1').val('');
                    $('#hflOriginalPremiumLife2').val('');
                    $('#hflTotalPremiumLife1').val('');
                    $('#hflTotalPremiumLife2').val('');
                    $('#hflTotalOriginalPremiumLife1').val('');
                    $('#hflTotalOriginalPremiumLife2').val('');
                }
               
            });

            //ddl PaymentMode
            $('#Main_ddlPaymentMode').change(function () {
                var str = $(this).find(":selected").text();
                var planId = $(this).val();
                var gender1 = $('#Main_hflGender').val();
                var gender2 = $('#Main_hflGenderLife2').val();

                $('#Main_hflPaymentMode').val($(this).val());
                if (this.selectedIndex > 0) {

                    //check ddlplanmode
                    if ($('#Main_ddlPlan').val() !="0") {
                        //dispay premium
                       // GetPremium("FP", age1, gender1, paymentMode, "1"); //premuim life 1
                        // GetPremium("FP", age2, gender2, paymentMode, "2");//premium life 2
                        GetPremium();
                       // alert('Age1:' + age1 + ' Age2:' + age2 + '  Gender1:' + gender1 + '  Gender2:' + gender2 + 'PaymentMode:' + paymentMode);
                        //total premium
                        totalPremium();
                    }
                    else {
                        alert('Plan is required.');
                        return;
                    }
                   
                }
                else {
                    //clear
                    //text box
                    $('#txtPremiumLife1').val('');
                    $('#txtPremiumLife2').val('');
                    $('#txtOriginalPremiumLife1').val('');
                    $('#txtOriginalPremiumLife2').val('');
                    $('#txtTotalPremiumLife1').val('');
                    $('#txtTotalPremiumLife2').val('');
                    $('#txtTotalOriginalPremiumLife1').val('');
                    $('#txtTotalOriginalPremiumLife2').val('');
                    //hidenfield
                    $('#hflPremiumLife1').val('');
                    $('#hflPremiumLife2').val('');
                    $('#hflOriginalPremiumLife1').val('');
                    $('#hflOriginalPremiumLife2').val('');
                    $('#hflTotalPremiumLife1').val('');
                    $('#hflTotalPremiumLife2').val('');
                    $('#hflTotalOriginalPremiumLife1').val('');
                    $('#hflTotalOriginalPremiumLife2').val('');

                }
            });


        });

        
       
        //validate required fields
        function validpage() {
            //application detail section
            var appNumber = $('#Main_txtApplicationNumber').val();
      
            alert(appNumber);
                
        }

      
        //get plan information
       function getPlan()
        {
           var planid = $('#Main_ddlPlan').val();
           if (planid != "0") {
               $.ajax({
                   type: "POST",
                   url: "../../FPWebService.asmx/getPlanList",
                   data: { planId: planid },
                   dataType: "json",
                   success: function (data) {
                       $(data).each(function (index, plan) {
                           //display sum insured
                           $('#Main_txtSumInsuredLife1').val(plan.Sum_insured1);
                           $('#Main_txtSumInsuredLife2').val(plan.Sum_insured2);
                           $('#Main_txtSumInsuredLife3').val(plan.Sum_insured3);
                           $('#Main_txtSumInsuredLife4').val(plan.Sum_insured4);
                           //display coverage peroid
                           $('#Main_txtCoverageYearLife1').val(plan.Coverage_peroid);
                           $('#Main_txtCoverageYearLife2').val(plan.Coverage_peroid);
                           $('#Main_txtCoverageYearLife3').val(plan.Coverage_peroid);
                           $('#Main_txtCoverageYearLife4').val(plan.Coverage_peroid);
                           //display payment peroid
                           $('#Main_txtPaymentPeroidLife1').val(plan.Payment_peroid);
                           $('#Main_txtPaymentPeroidLife2').val(plan.Payment_peroid);
                           $('#Main_txtPaymentPeroidLife3').val(plan.Payment_peroid);
                           $('#Main_txtPaymentPeroidLife4').val(plan.Payment_peroid);

                           //display age
                           GetCustomerAge();

                       });

                   },
                   error: function (err) {
                       alert('Page error, please contact your system administrator. Error Detail: ' + err);
                   }

               });
           
           }
         
           
       }

        //total premium and original premium
       function totalPremium()
       {
           //total premium and original premium
           var premium1 = $('#Main_hflPremiumLife1').val();
           var premium2 = $('#Main_hflPremiumLife2').val();

           var originalPremium1 = $('#Main_hflOriginalPremiumLife1').val();
           var originalPremium2 = $('#Main_hflOriginalPremiumLife2').val();

           var totalOriginalPremium = parseFloat(originalPremium1) + parseFloat(originalPremium2);
           var totalPremium = parseFloat(premium1) + parseFloat(premium2);
         
           $('#Main_txtTotalPremium').val('');
           $('#Main_txtTotalOriginalPremium').val('');

           $('#Main_txtTotalPremium').val(totalPremium);
           $('#Main_txtTotalOriginalPremium').val(totalOriginalPremium);
          
       }
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

            var application_number = $('#Main_txtApplicationNumberModal').val();

            if (application_number == "") {
                alert('Please fill in Application Number');
                return;
            }

            //format application number 
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
                        $('#Main_lblApplicationNumber').text(application_number);
                        $('#Main_txtDataEntryBy').val(user_name);

                        $('#Main_lblApplicationNumberModalValidate').hide();

                        //Close New Application Modal
                        $('#myModalNewApplicationForm').modal('hide');
                    } else {

                        check_result = "1";
                        $('#Main_lblApplicationNumberModalValidate').show();
                    }

                }

            });

        }

        //Get Insurance Plan
        function GetInsurancePlan(product_type, product_id) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetInsurancePlans",
                data: "{product_type:" + JSON.stringify(product_type) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_ddlTypeInsurancePlan').setTemplate($("#jTemplateInsurancePlan").html());
                    $('#Main_ddlTypeInsurancePlan').processTemplate(data);

                    if (product_id != "") {

                        $('#Main_ddlTypeInsurancePlan').val(product_id);
                        $('#Main_hdfTypeInsurancePlan').val(product_id);
                    }

                }

            });
        };

        //Get Insurance Plan (Edit)
        function GetInsurancePlanEdit(product_type, product_id) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetInsurancePlansEdit",
                data: "{product_type:'" + product_type + "',product_id:'" + product_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_ddlTypeInsurancePlanEdit').setTemplate($("#jTemplateInsurancePlanEdit").html());
                    $('#Main_ddlTypeInsurancePlanEdit').processTemplate(data);
                }

            });
        };

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


        //Calculate Insurance (From Selection of Date Entry) : Purpose to stop displaying alert ('Please fill in date of birth')
        function CalculateInsuranceNoAlert() {
            var product_id = $('#Main_ddlTypeInsurancePlan').val();
            var dob = $('#Main_txtDateBirth').val();

            if (dob != "" && dob != " " && dob != null) {
                if (product_id != " " && product_id != "" && product_id != "0" && product_id != "null" && product_id != null) {
                    GetCustomerAge();
                    var customer_age = $('#Main_txtAssureeAge').val();
                    GetAssureYear(product_id, customer_age);
                    GetPayYear(product_id);

                    GetPremium(customer_age);

                } else {
                    alert('Please select insurance plan');
                }
            }

        }

        //Calculate Insurance (Edit)
        function CalculateInsuranceEdit() {
            var product_id = $('#Main_ddlTypeInsurancePlanEdit').val();
            var dob = $('#Main_txtDateBirthEdit').val();

            if (dob != "" && dob != " " && dob != null) {
                if (product_id != " " && product_id != "" && product_id != "0" && product_id != "null" && product_id != null) {
                    GetCustomerAgeEdit();
                    var customer_age = $('#Main_txtAssureeAgeEdit').val();
                    GetAssureYearEdit(product_id, customer_age);
                    GetPayYearEdit(product_id);

                    GetPremiumEdit(customer_age);

                } else {
                    alert('Please select insurance plan');
                }
            } else {
                alert('Please fill in date of birth');
            }

        }

        //Insurance Plan Section
        function ProcessInsuranceType(product_id) {
            var dob = $('#Main_txtDateBirth').val();

            if (dob != "" && dob != " " && dob != null) {
                GetCustomerAge();
                var customer_age = $('#Main_txtAssureeAge').val();
                GetAssureYear(product_id, customer_age);
                GetPayYear(product_id);

            }
            else {
                alert('Please fill in date of birth');
            }

        };

        //Insurance Plan Section (Edit)
        function ProcessInsuranceTypeEdit(product_id) {
            var dob = $('#Main_txtDateBirthEdit').val();

            if (dob != "" && dob != " " && dob != null) {
                GetCustomerAgeEdit();
                var customer_age = $('#Main_txtAssureeAgeEdit').val();
                GetAssureYearEdit(product_id, customer_age);
                GetPayYearEdit(product_id);

            }
            else {
                alert('Please fill in date of birth');
            }

        };


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


        //Get Pay Year for (payment period) (Edit)
        function GetPayYearEdit(product_id) {
            $.ajax({
                type: "POST",
                url: "../../ProductWebService.asmx/GetPayYear",
                data: "{product_id:" + JSON.stringify(product_id) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtPaymentPeriodEdit').val(data.d);

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

                }

            });
        };

        //Beneficiary Section     
        function GetTotalBenefitShare(obj) {
            total_share = 0;
            ValidateTextDecimal(obj);

            //recalculate total share
            for (var i = 2; i <= row_index_benefit; i++) {
                var benefit_share = $('#Benefit_Share' + i).val();

                if (benefit_share != null && benefit_share != "") {
                    total_share = +total_share + +benefit_share;
                }


                if (total_share > 0) {
                    $('#Main_lblTotalBenefitPercentage').text("Total: " + total_share + "%");
                } else {
                    $('#Main_lblTotalBenefitPercentage').text("Total: 0%");
                }

            }

            if (total_share > 100) {
                alert("Sum of total share exceeds 100%");
            }

        };

        //Delete Benefit Row
        function DeleteBenefitRow(row_index_benefit_delete) {
            var remove_share = $('#Benefit_Share' + row_index_benefit_delete).val();
            total_share = total_share - remove_share;
            if (total_share > 0) {
                $('#Main_lblTotalBenefitPercentage').text("Total: " + total_share + "%");
            } else {
                $('#Main_lblTotalBenefitPercentage').text("Total: 0%");
            }

            $('#Benefit' + row_index_benefit_delete).remove()
        }

        //Beneficiary Section (Edit)  
        function GetTotalBenefitShareEdit(obj) {
            total_share_edit = 0;
            ValidateTextDecimal(obj);

            //recalculate total share
            for (var i = 2; i <= row_index_benefit_edit; i++) {
                var share = $('#Benefit_Share_Edit' + i).val();

                if (share != null && share != "") {
                    total_share_edit = +total_share_edit + +share;
                }

                if (total_share_edit > 0) {
                    $('#Main_lblTotalBenefitPercentageEdit').text("Total: " + total_share_edit + "%");
                } else {
                    $('#Main_lblTotalBenefitPercentageEdit').text("Total: 0%");
                }

            }

            if (total_share_edit > 100) {
                alert("Sum of total share exceeds 100%");
            }

        };

        //Delete Benefit Row (Edit)
        function DeleteBenefitRowEdit(row_index_benefit_edit_delete) {
            var remove_share = $('#Benefit_Share_Edit' + row_index_benefit_edit_delete).val();
            total_share_edit = total_share_edit - remove_share;
            if (total_share_edit > 0) {
                $('#Main_lblTotalBenefitPercentageEdit').text("Total: " + total_share_edit + "%");
            } else {
                $('#Main_lblTotalBenefitPercentageEdit').text("Total: 0%");
            }

            $('#Benefit_Edit' + row_index_benefit_edit_delete).remove()
        }

        var row_index_benefit = 1;
        function AddNewBenefitRow() {
            var id_type = "";

            var id_type_value = $('#Main_ddlBenefitIDType option:selected').val();

            switch (id_type_value) {
                case '0':
                    id_type = "I.D Card";
                    break;
                case '1':
                    id_type = "Passpord";
                    break;
                case '2':
                    id_type = "Visa";
                    break;
                case '3':
                    id_type = "Birth Certificate";
                    break;
            }

            var id_no = $('#Main_txtBenefitIDNo').val();
            var benefit_name = $('#Main_txtBenefitName').val();

            var relation_value = $('#Main_ddlBenefitRelation option:selected').val();

            var share = $('#Main_txtBenefitSharePercentage').val();
            var number_share = share;

            if (share === "") {
                share = 0;
            }

            if (+total_share + +number_share <= 100) {


                total_share = +total_share + +number_share;

                $('#Main_lblTotalBenefitPercentage').text("Total: " + total_share + "%");

                row_index_benefit += 1;

                var new_row = "<tr id='Benefit" + row_index_benefit + "' style='background-color: lightgray;'>"
                new_row += "<td>&nbsp;</td>"
                new_row += "<td>"
                new_row += "<Label id='ID_Type" + row_index_benefit + "'>" + id_type + "</Label><input type='hidden' id='ID_Type_Value" + row_index_benefit + "' value='" + id_type_value + "' />"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='ID_No" + row_index_benefit + "' value='" + id_no + "' style='width: 93%; height: 30px' maxlength='30' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Name" + row_index_benefit + "' value='" + benefit_name + "' style='width: 93%; height: 30px' maxlength='100' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<Label id='Benefit_Relation" + row_index_benefit + "'>" + relation_value + "</Label>"
                new_row += "</td> "
                new_row += "<td>"


                if ($('#Main_hdfProductType').val() === '2') {

                    if (relation_value === 'CREDITOR') {

                        new_row += "<input type='text' id='Benefit_Share" + row_index_benefit + "' value='" + share + "' readonly='readonly' onkeyup='GetTotalBenefitShare(this)' onkeypress='return (event.keyCode != 13)' style='width: 93%; height: 30px' maxlength='5'/>"
                    } else {
                        new_row += "<input type='text' id='Benefit_Share" + row_index_benefit + "' value='" + share + "' onkeyup='GetTotalBenefitShare(this)' onkeypress='return (event.keyCode != 13)' style='width: 93%; height: 30px' maxlength='5'/>"
                    }
                } else {
                    new_row += "<input type='text' id='Benefit_Share" + row_index_benefit + "' value='" + share + "' onkeyup='GetTotalBenefitShare(this)' onkeypress='return (event.keyCode != 13)' style='width: 93%; height: 30px' maxlength='5'/>"
                }


                new_row += "</td> "
                new_row += "<td style='text-align:center'>"
                new_row += "<button type='button' class='btn btn-default btn-lg' style='height:40px' onclick='DeleteBenefitRow(" + row_index_benefit + ")'><span class='icon-trash'></span> Del</button>"

                new_row += "</td>"
                new_row += "<td width='2%'>&nbsp;</td></tr>"

                $('#tblBenefitTable').append(new_row);

                //Clear beneficary
                $('#Main_txtBenefitSharePercentage').val("");
                $('#Main_txtBenefitName').val("");
                $('#Main_txtBenefitIDNo').val("");
                $('#Main_ddlBenefitIDType').prop('selectedIndex', 0);
                $('#Main_ddlBenefitRelation').prop('selectedIndex', 0);
                $('#Main_txtBenefitSharePercentage').prop('readonly', false);
            }
            else {
                alert("Sum of total share exceeds 100%");
            }

        };

        var row_index_benefit_edit = 1;
        function AddNewBenefitRowEdit() {
            var id_type = "";

            var id_type_value = $('#Main_ddlBenefitIDTypeEdit option:selected').val();
            var id_no = $('#Main_txtBenefitIDNoEdit').val();
            var benefit_name = $('#Main_txtBenefitNameEdit').val();
            var relation = $('#Main_ddlBenefitRelationEdit').text();
            var relation_value = $('#Main_ddlBenefitRelationEdit option:selected').val();
            var share = $('#Main_txtBenefitSharePercentageEdit').val();
            var number_share = share;

            switch (id_type_value) {
                case "0":
                    id_type = "I.D Card";
                    break;
                case "1":
                    id_type = "Passpord";
                    break;
                case "2":
                    id_type = "Visa";
                    break;
                case "3":
                    id_type = "Birth Certificate";
                    break;
            }

            if (share === "") {
                share = 0;
            }

            if (+total_share_edit + +number_share <= 100) {


                total_share_edit = +total_share_edit + +number_share;

                $('#Main_lblTotalBenefitPercentageEdit').text("Total: " + total_share_edit + "%");

                row_index_benefit_edit += 1;

                var new_row = "<tr id='Benefit_Edit" + row_index_benefit_edit + "' style='background-color: lightgray'>"
                new_row += "<td>&nbsp;</td>"
                new_row += "<td>"
                new_row += "<Label id='ID_Type_Edit" + row_index_benefit_edit + "'>" + id_type + "</Label><input type='hidden' id='ID_Type_Value_Edit" + row_index_benefit_edit + "' value='" + id_type_value + "' />"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='ID_No_Edit" + row_index_benefit_edit + "' value='" + id_no + "' style='width: 83%; height: 30px' maxlength='30' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<input type='text' id='Benefit_Name_Edit" + row_index_benefit_edit + "' value='" + benefit_name + "' style='width: 83%; height: 30px' maxlength='100' onkeypress='return (event.keyCode != 13)'/>"
                new_row += "</td> "
                new_row += "<td>"
                new_row += "<Label id='Benefit_Relation_Edit" + row_index_benefit_edit + "'>" + relation_value + "</Label>"
                new_row += "</td> "
                new_row += "<td>"

                if ($('#Main_hdfProductType').val() === '2') {
                    if (relation_value === 'CREDITOR') {
                        new_row += "<input type='text' id='Benefit_Share_Edit" + row_index_benefit_edit + "' value='" + share + "' readonly='readonly' onkeyup='GetTotalBenefitShareEdit(this)' onkeypress='return (event.keyCode != 13)' style='width: 83%; height: 30px' maxlength='5'/>"
                    } else {
                        new_row += "<input type='text' id='Benefit_Share_Edit" + row_index_benefit_edit + "' value='" + share + "' onkeyup='GetTotalBenefitShareEdit(this)' onkeypress='return (event.keyCode != 13)' style='width: 83%; height: 30px' maxlength='5'/>"
                    }
                } else {
                    new_row += "<input type='text' id='Benefit_Share_Edit" + row_index_benefit_edit + "' value='" + share + "' onkeyup='GetTotalBenefitShareEdit(this)' onkeypress='return (event.keyCode != 13)' style='width: 83%; height: 30px' maxlength='5'/>"
                }

                new_row += "</td> "
                new_row += "<td style='text-align:center'>"
                new_row += "<button type='button' class='btn btn-default btn-lg' style='height:40px' onclick='DeleteBenefitRowEdit(" + row_index_benefit_edit + ")'><span class='icon-trash'></span> Del</button>"

                new_row += "</td>"
                new_row += "<td width='2%'>&nbsp;</td></tr>"

                $('#tblBenefitTableEdit').append(new_row);

                //Clear beneficary
                $('#Main_txtBenefitSharePercentageEdit').val("");
                $('#Main_txtBenefitNameEdit').val("");
                $('#Main_txtBenefitIDNoEdit').val("");
                $('#Main_ddlBenefitIDTypeEdit').prop('selectedIndex', 0);
                $('#Main_ddlBenefitRelationEdit').prop('selectedIndex', 0);
                $('#Main_txtBenefitSharePercentageEdit').prop('readonly', false);

            }
            else {
                alert("Sum of total share exceeds 100%");
            }

        };

        //Get customer Age
        function GetCustomerAge() {

            var dob = $('#Main_txtDateBirth').val();
            var dob1 = $('#Main_txtDOBLife2').val();
            var date_of_entry = $('#Main_txtDateEntry').val();

            //age of Insured life 1
            if (dob != "" && dob != " " && dob != null) {
                //calculate customer age base on dob
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetCustomerAge",
                    data: "{dob:'" + dob + "',date_of_entry:'" + date_of_entry + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtAgeLife1').val(data.d);
                        $('#Main_hflAgeLife1').val(data.d);
                        age1 = data.d;
                    },
                    error: function (msg) {
                        alert("Problem in calculating age");
                    }
                });
            
            }

            //age of Insured life 2
            if (dob1 != "" && dob1 != " " && dob1 != null) {
                //calculate customer age base on dob
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetCustomerAge",
                    data: "{dob:'" + dob1 + "',date_of_entry:'" + date_of_entry + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtAgeLife2').val(data.d);
                        $('#Main_hflAgeLife2').val(data.d);
                        age2 = data.d;
                    },
                    error: function (msg) {
                        alert("Problem in calculating age");
                    }
                });

            }
            
        }


        //Get customer Age (Edit)
        function GetCustomerAgeEdit() {
            var dob = $('#Main_txtDateBirthEdit').val();
            var date_of_entry = $('#Main_txtDateEntry').val();

            if (dob != "" && dob != " " && dob != null) {
                //calculate customer age base on dob
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetCustomerAge",
                    data: "{dob:'" + dob + "',date_of_entry:'" + date_of_entry + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtAssureeAgeEdit').val(data.d);

                        GetPremiumEdit(data.d);
                    },
                    error: function (msg) {
                        alert("Problem in calculating age");
                    }
                });
            }
        }

        //Get Premium
        function GetPremium() {
            var productId, age1, gender1, age2, gender2, paymentMode;

            productId = 'FP';
            age1 = $('#Main_hflAgeLife1').val();
            age2 = $('#Main_hflAgeLife2').val();
            gender1 = $('#Main_hflGender').val();
            gender2 = $('#Main_hflGenderLife2').val();
            paymentMode = $('#Main_hflPaymentMode').val();
          

            //premium life 1
            if (productId != "" && age1 != "" && gender1 != "" && paymentMode != "") {
                alert('Age1:' + age1 + '  Gender1:' + gender1 +  'PaymentMode:' + paymentMode);
                $.ajax({
                    type: "POST",
                    url: "../../FPWebService.asmx/getPremiumLife1",
                    data: { product_id: productId, customer_age: age1, gender: gender1, paymentMode: paymentMode },
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var arr = data.split(",");
                        $('#Main_txtPremiumLife1').val(arr[0]);
                        $('#Main_txtOriginalPremiumLife1').val(arr[1]);
                        $('#Main_hflPremiumLife1').val(arr[0]);
                        $('#Main_hflOriginalPremiumLife1').val(arr[1]);

                    },
                    errror: function (err) {
                        alert(err);
                    }

                });
            }
            else {
                //nothing
            }
            //premium life 2
            if (productId != "" && age2 != "" && gender2 != "" && paymentMode != "") {
                alert(' Age2:' + age2 + '  Gender2:' + gender2 + 'PaymentMode:' + paymentMode);
                $.ajax({
                    type: "POST",
                    url: "../../FPWebService.asmx/getPremiumLife2",
                    data: { product_id: productId, customer_age: age2, gender: gender2, paymentMode: paymentMode },
                    dataType: "json",
                    //contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var arr = data.split(",");
                        $('#Main_txtPremiumLife2').val(arr[0]);
                        $('#Main_txtOriginalPremiumLife2').val(arr[1]);
                        $('#Main_hflPremiumLife2').val(arr[0]);
                        $('#Main_hflOriginalPremiumLife2').val(arr[1]);
                    }

                });
            }
            else {
                //nothing
            }
            /*
            if (option == "1") //premium life 1
            {
                $.ajax({
                    type: "POST",
                    url: "../../FPWebService.asmx/getPremiumLife1",
                    data: { product_id: productId, customer_age: age, gender: gender, paymentMode: paymentMode },
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var arr = data.split(",");
                       $('#Main_txtPremiumLife1').val(arr[0]);
                       $('#Main_txtOriginalPremiumLife1').val(arr[1]);
                       $('#Main_hflPremiumLife1').val(arr[0]);
                       $('#Main_hflOriginalPremiumLife1').val(arr[1]);

                    },
                    errror: function (err) {
                        alert(err);
                    }

                });
            }
            else if (option == "2")//premium life 2
            {
                $.ajax({
                    type: "POST",
                    url: "../../FPWebService.asmx/getPremiumLife2",
                    data: { product_id: productId, customer_age: age, gender: gender, paymentMode: paymentMode },
                    dataType: "json",
                    //contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var arr = data.split(",");
                        $('#Main_txtPremiumLife2').val(arr[0]);
                        $('#Main_txtOriginalPremiumLife2').val(arr[1]);
                        $('#Main_hflPremiumLife2').val(arr[0]);
                        $('#Main_hflOriginalPremiumLife2').val(arr[1]);
                    }

                });
            }*/

          
        }

        //Function auto select dropdownlist
        function selectedChange(obj)
        {
            var str = $(obj).find(":selected").text();
            var va = $(obj).val();
            if (va == 0)
            {
                alert(v);
            }
        }

        //Get Premium Edit
        function GetPremiumEdit(customer_age) {

            if (customer_age == "0") {
                var customer_age = $('#Main_txtAssureeAgeEdit').val();
            }

            var sum_insured = $('#Main_txtInsuranceAmountRequiredEdit').val();
            var pay_mode = $('#Main_ddlPremiumModeEdit').val();
            var product_id = $('#Main_ddlTypeInsurancePlanEdit').val();

            var coverage_year = $('#Main_txtTermInsuranceEdit').val();
            var gender = $('#Main_ddlGenderEdit').val();
            var product_type = $('#Main_hdfProductType').val();

            //var is_pre_premium_discount = 1;

            //if ($('#Main_ckbIsPrePremiumDiscountEdit').is(':checked')) {
            //    is_pre_premium_discount = 1;
            //} else {
            //    is_pre_premium_discount = 0;
            //}



            if (sum_insured != "") {
                if (customer_age != "" && customer_age != " " && customer_age != null) {
                    if (product_id != " " && product_id != "" && product_id != "0" && product_id != "null" && product_id != null) {
                        //culculate premium
                        $.ajax({
                            type: "POST",
                            url: "../../CalculationWebService.asmx/GetPremium",
                            data: "{product_id:'" + product_id + "',sum_insured:'" + sum_insured + "',customer_age:'" + customer_age + "',coverage_period:'" + coverage_year + "',gender:'" + gender + "',product_type:'" + product_type + "',pay_mode:'" + pay_mode + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {

                                var arr = data.d.split(',');

                                $('#Main_txtPremiumAmountSystemEdit').val(arr[0]);
                                $('#Main_txtAnnualOriginalPremiumAmountSystemEdit').val(arr[1]);

                                $('#Main_hdfOriginalPremiumAmountSystem').val(arr[1]);

                                //get premium after discount edit
                                ProcessDiscountAmountEdit();

                            }

                        });
                    } else {
                        alert('Please select type of insurance plan and type insurance amount again.');
                    }
                }
            }
            else {
                $('#Main_txtPremiumAmountSystemEdit').val("");
            }

        }

        //Save new application
        function SaveApplication() {
            Page_ClientValidate();

            if (Page_IsValid) {

                if (check_result === "0") {

                    //Save Application alert
                    if (confirm('Are your sure you want to save this application form?')) {

                        //Register application

                        var app_number = $('#Main_hdfAppNumber').val();
                        var sale_agent_id = $('#Main_hdfMarketingCode').val();
                        var date_signature = $('#Main_txtDateSignature').val();
                        var created_by = $('#Main_hdfDataEntryBy').val();
                        var created_note = $('#Main_txtNote').val();
                        
                        var created_on = $('#Main_hdfDateEntry').val();
                        var user_id = $('#Main_hdfuserid').val();
                        var payment_code = $('#Main_txtPaymentCode').val();
                        var channel_id = $('#Main_ddlChannel').val();
                        var channel_item_id = $('#Main_ddlCompany').val();

                        $.ajax({
                            type: "POST",
                            url: "../../AppWebService.asmx/RegisterApplication",
                            data: "{app_number:'" + app_number + "',sale_agent_id:'" + sale_agent_id + "',date_signature:'" + date_signature + "',created_by:'" + created_by + "',created_note:'" + created_note + "',created_on:'" + created_on + "',benefit_note:'" + benefit_note + "',user_id:'" + user_id + "',payment_code:'" + payment_code + "',channel_id:'" + channel_id + "',channel_item_id:'" + channel_item_id + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {

                                //Save Discount
                                SaveDiscount(data.d);


                            },
                            error: function (msg) {
                                alert('Registration Failed. Please check it again. Register');
                                return; //exit function
                            }
                        });


                    }
                    else {
                        //no action            
                    }
                    //}
                } else {
                    alert("Application Number already exist");
                }

            } else {
                alert("Input is invalid. Please check for red *");
            }
        }


            //Search Application By App No
            function SearchApplicationByAppNo() {

                var app_number = $('#Main_txtApplicationNumberSearch').val();

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/SearchApplicationByAppNo",
                    data: "{app_number:'" + app_number + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#dvApplicationList').setTemplate($("#jTemplateApplication").html());
                        $('#dvApplicationList').processTemplate(data);

                    }

                });
            }

            //Search Application By Customer Number
            function SearchApplicationByCustomerName() {
                var first_name = $('#Main_txtFirstNameSearch').val();
                var last_name = $('#Main_txtLastNameSearch').val();

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/SearchApplicationByCustomerName",
                    data: "{last_name:'" + last_name + "',first_name:'" + first_name + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#dvApplicationList').setTemplate($("#jTemplateApplication").html());
                        $('#dvApplicationList').processTemplate(data);

                    }

                });
            }

            //Search Application By ID Card No
            function SearchApplicationByIDCardNo() {
                var id_type = $('#Main_ddlIDTypeSearch').val();
                var id_card_no = $('#Main_txtIDCardNoSearch').val();

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/SearchApplicationByIDCard",
                    data: "{id_type:'" + id_type + "',id_card_no:'" + id_card_no + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#dvApplicationList').setTemplate($("#jTemplateApplication").html());
                        $('#dvApplicationList').processTemplate(data);

                    }

                });

            }

            //Get Application By ID
            var search_app_id;
            function GetApplication(app_id, index, total) {

                if ($('#Ch' + index).is(':checked')) {
                    search_app_id = app_id;
                } else {
                    search_app_id = "";
                }

                //Uncheck all other checkboxes
                for (var i = 0; i < total  ; i++) {
                    if (i != index) {
                        $('#Ch' + i).prop('checked', false);
                    }

                }
            }

            //Fill Searched Application Data
            function FillSearchedData() {

                if (search_app_id != "") {
                    Clear();

                    //Get App_Register single row info
                    GetAppSingleRow();

                    $("#tblAppContent *").attr("disabled", "disabled");


                } else {
                    alert("Please select a checkbox");
                }

            }

            //Populate Application Single Row Data
            function GetAppSingleRow() {
                //Disabled Edit button first
                $('#btnAppEdit').prop('disabled', true);

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/GetAppSingleRow",
                    data: "{app_id:'" + search_app_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Main_hdfAppRegisterID').val(data.d.App_Register_ID);
                        $('#Main_txtApplicationNumber').val(data.d.App_Number);
                        $('#Main_hdfAppNumber').val(data.d.App_Number);
                        $('#Main_txtPaymentCode').val(data.d.Payment_Code);
                        $('#Main_ddlChannel').val(data.d.Channel_ID);
                        $('#Main_ddlCompany').val(data.d.Channel_Item_ID);
                        $('#Main_txtPolicyNumber').val(data.d.Policy_Number);
                        $('#Main_txtUnderwritingStatus').val(data.d.Underwriting_Status);
                        $('#Main_hdfUnderwritingStatus').val(data.d.Underwriting_Status);

                        $('#Main_txtDateEntry').val(formatJSONDate(data.d.Created_On));
                        $('#Main_hdfDateEntry').val(formatJSONDate(data.d.Created_On));
                        $('#Main_txtDataEntryBy').val(data.d.Created_By);
                        $('#Main_hdfDataEntryBy').val(data.d.Created_By);
                        $('#Main_txtDateSignature').val(formatJSONDate(data.d.App_Date));
                        $('#Main_hdfDateSignature').val(formatJSONDate(data.d.App_Date));
                        $('#Main_txtMarketingCode').val(data.d.Sale_Agent_ID);
                        $('#Main_hdfMarketingCode').val(data.d.Sale_Agent_ID);
                        $('#Main_hdfPreviousMarketingCode').val(data.d.Sale_Agent_ID);
                        $('#Main_txtMarketingName').val(data.d.Sale_Agent_Full_Name);
                        $('#Main_hdfMarketingName').val(data.d.Sale_Agent_Full_Name);
                        $('#Main_hdfPreviousMarketingName').val(data.d.Sale_Agent_Full_Name);
                        $('#Main_txtNote').val(data.d.Created_Note);
                        $('#Main_hdfNote').val(data.d.Created_Note);
                        $('#Main_ddlIDType').val(data.d.ID_Type);
                        $('#Main_hdfIDType').val(data.d.ID_Type);
                        $('#Main_txtIDNumber').val(data.d.ID_Card);
                        $('#Main_txtSurnameKh').val(data.d.Khmer_Last_Name);
                        $('#Main_hdfSurnameKh').val(data.d.Khmer_Last_Name);
                        $('#Main_txtFirstNameKh').val(data.d.Khmer_First_Name);
                        $('#Main_hdfFirstNameKh').val(data.d.Khmer_First_Name);
                        $('#Main_txtSurnameEng').val(data.d.Last_Name);
                        $('#Main_hdfSurnameEng').val(data.d.Last_Name);
                        $('#Main_txtFirstNameEng').val(data.d.First_Name);
                        $('#Main_hdfFirstNameEng').val(data.d.First_Name);
                        $('#Main_txtSurnameFather').val(data.d.Father_Last_Name);
                        $('#Main_hdfSurnameFather').val(data.d.Father_Last_Name);
                        $('#Main_txtFirstNameFather').val(data.d.Father_First_Name);
                        $('#Main_hdfFirstNameFather').val(data.d.Father_First_Name);
                        $('#Main_txtSurnameMother').val(data.d.Mother_Last_Name);
                        $('#Main_hdfSurnameMother').val(data.d.Mother_Last_Name);
                        $('#Main_txtFirstNameMother').val(data.d.Mother_First_Name);
                        $('#Main_hdfFirstNameMother').val(data.d.Mother_First_Name);
                        $('#Main_txtPreviousSurname').val(data.d.Prior_Last_Name);
                        $('#Main_hdfPreviousSurname').val(data.d.Prior_Last_Name);
                        $('#Main_txtPreviousFirstname').val(data.d.Prior_First_Name);
                        $('#Main_hdfPreviousFirstName').val(data.d.Prior_First_Name);
                        $('#Main_ddlNationality').val(data.d.Nationality);
                        $('#Main_hdfNationality').val(data.d.Nationality);
                        $('#Main_ddlGender').val(data.d.Gender);
                        $('#Main_hdfGender').val(data.d.Gender);
                        $('#Main_txtDateBirth').val(formatJSONDate(data.d.Birth_Date));
                        $('#Main_hdfDateBirth').val(formatJSONDate(data.d.Birth_Date));
                        $('#Main_txtAddress1').val(data.d.Address1);
                        $('#Main_hdfAddress1').val(data.d.Address1);
                        $('#Main_txtAddress2').val(data.d.Address2);
                        $('#Main_hdfAddress2').val(data.d.Address2);
                        $('#Main_txtCity').val(data.d.Province);
                        $('#Main_hdfCity').val(data.d.Province);
                        $('#Main_txtZipCode').val(data.d.Zip_Code);
                        $('#Main_hdfZipCode').val(data.d.Zip_Code);
                        $('#Main_hdfPreviousZipCode').val(data.d.Zip_Code);
                        $('#Main_ddlCountry').val(data.d.Country);
                        $('#Main_hdfCountry').val(data.d.Country);
                        $('#Main_txtTelephone').val(data.d.Home_Phone1);
                        $('#Main_hdfTelephone').val(data.d.Home_Phone1);
                        $('#Main_txtMobilePhone').val(data.d.Mobile_Phone1);
                        $('#Main_hdfMobilePhone').val(data.d.Mobile_Phone1);
                        $('#Main_txtEmail').val(data.d.EMail);
                        $('#Main_hdfEmail').val(data.d.EMail);
                        $('#Main_txtNameEmployer').val(data.d.Employer_Name);
                        $('#Main_hdfNameEmployer').val(data.d.Employer_Name);
                        $('#Main_txtNatureBusiness').val(data.d.Nature_Of_Business);
                        $('#Main_hdfNatureBusiness').val(data.d.Nature_Of_Business);
                        $('#Main_txtRoleAndResponsibility').val(data.d.Job_Role);
                        $('#Main_hdfRoleAndResponsibility').val(data.d.Job_Role);
                        $('#Main_txtCurrentPosition').val(data.d.Current_Position);
                        $('#Main_hdfCurrentPosition').val(data.d.Current_Position);
                        $('#Main_txtAnualIncome').val(data.d.Anual_Income);
                        $('#Main_hdfAnualIncome').val(data.d.Anual_Income);
                        $('#Main_hdfProductType').val(data.d.Product_Type_ID);

                        //$('#Main_hdfPreviousInsurancePlan').val(data.d.Product_ID);

                        $('#Main_hdfInsurancePlan').val(data.d.Product_ID);

                        GetInsurancePlan(data.d.Product_Type_ID, data.d.Product_ID);

                        GetInsurancePlanEdit(data.d.Product_Type_ID, data.d.Product_ID);

                        $('#Main_txtInsuranceAmountRequired').val(data.d.User_Sum_Insure);
                        $('#Main_hdfInsuranceAmountRequired').val(data.d.User_Sum_Insure);
                        $('#Main_txtTermInsurance').val(data.d.Assure_Year);
                        $('#Main_hdfTermInsurance').val(data.d.Assure_Year);
                        $('#Main_hdfAssureeAge').val(data.d.Age_Insure);
                        $('#Main_txtAssureeAge').val(data.d.Age_Insure);
                        $('#Main_hdfPreviousAssureeAge').val(data.d.Age_Insure);
                        $('#Main_txtPaymentPeriod').val(data.d.Pay_Year);
                        $('#Main_hdfPaymentPeriod').val(data.d.Pay_Year);
                        $('#Main_hdfPreviousPaymentPeriod').val(data.d.Pay_Year);
                        $('#Main_txtInsuranceAmountRequired').val(data.d.User_Sum_Insure);
                        $('#Main_hdfInsuranceAmountRequired').val(data.d.User_Sum_Insure);

                        $('#Main_txtPremiumAmountSystem').val(data.d.System_Premium);
                        $('#Main_hdfPremiumAmountSystem').val(data.d.System_Premium);
                        $('#Main_hdfPreviousPremiumAmountSystem').val(data.d.System_Premium);
                        $('#Main_ddlPremiumMode').val(data.d.Pay_Mode);

                        //if (data.d.Is_Pre_Premium_Discount == 1) {
                        //    $('#Main_ckbPrePremiumDiscount').prop('checked', true);

                        //} else {
                        //    $('#Main_ckbPrePremiumDiscount').prop('checked', false);
                        //}

                        //Discount Amount 
                        $('#Main_txtDiscountAmount').val(data.d.Discount_Amount);
                        $('#Main_txtInsurancePlanNote').val(data.d.Insurance_Plan_Note);
                        ProcessDiscountAmount();


                        $('#Main_hdfPremiumMode').val(data.d.Pay_Mode);
                        $('#Main_txtPremiumAmount').val(data.d.User_Premium);
                        $('#Main_hdfPremiumAmount').val(data.d.User_Premium);
                        $('#Main_txtBenefitNote').val(data.d.Benefit_Note);
                        $('#Main_hdfBenefitNote').val(data.d.Benefit_Note);
                        $('#Main_txtHeight').val(data.d.Height);
                        $('#Main_hdfHeight').val(data.d.Height);
                        $('#Main_txtWeight').val(data.d.Weight);
                        $('#Main_hdfWeight').val(data.d.Weight);

                        $('#Main_rbtnlWeightChange_' + data.d.Is_Weight_Changed).prop('checked', true);
                        $('#Main_hdfWeightChange').val(data.d.Is_Weight_Changed);

                        $('#Main_txtWeightChangeReason').val(data.d.Reason);

                        if (data.d.Is_Weight_Changed != "0") {
                            $('#Main_txtWeightChangeReason').show();
                            $('#Main_lblWeightChangeReason').show();
                        } else {
                            $('#Main_txtWeightChangeReason').hide();
                            $('#Main_lblWeightChangeReason').hide();
                        }

                        $('#Main_txtAnnualOriginalPremiumAmountSystem').val(data.d.Original_Amount);
                        $('#Main_hdfOriginalPremiumAmountSystem').val(data.d.Original_Amount);
                        $('#Main_hdfPreviousOriginalPremiumAmountSystem').val(data.d.Original_Amount);

                        GetBenefits();
                        GetPremiumDiscount();

                        //hide and show Loan
                        if (data.d.Product_Type_ID == "2") {

                            $('#dvLoan').show();

                            //populate loan
                            $('#Main_txtInterest').val(data.d.Interest_Rate);
                            $('#Main_hdfInterest').val(data.d.Interest_Rate);

                            $('#Main_hdfLoanType').val(data.d.Loan_Type);
                            $('#Main_ddlLoanType').val(data.d.Loan_Type);

                            $('#Main_txtTermLoan').val(data.d.Term_Year);
                            $('#Main_hdfTermLoan').val(data.d.Term_Year);

                            $('#Main_txtLoanEffectiveDate').val(formatJSONDate(data.d.Loan_Effiective_Date));
                            $('#Main_hdfLoanEffectiveDate').val(formatJSONDate(data.d.Loan_Effiective_Date));

                            $('#Main_txtOutstandingLoanAmount').val(data.d.Out_Std_Loan);
                            $('#Main_hdfOutstandingLoanAmount').val(data.d.Out_Std_Loan);

                            $('#Main_txtTermInsuranceEdit').prop('readonly', false);
                            $('#Main_ddlPremiumModeEdit').prop('disabled', true);

                        } else {
                            $('#dvLoan').hide();

                            //clear loan
                            $('#Main_txtInterest').val("");
                            $('#Main_hdfInterest').val("");

                            $('#Main_hdfLoanType').val("");
                            $('#Main_ddlLoanType').val("");

                            $('#Main_txtTermLoan').val("");
                            $('#Main_hdfTermLoan').val("");

                            $('#Main_txtLoanEffectiveDate').val("");
                            $('#Main_hdfLoanEffectiveDate').val("");

                            $('#Main_txtOutstandingLoanAmount').val("");
                            $('#Main_hdfOutstandingLoanAmount').val("");

                            $('#Main_txtTermInsuranceEdit').prop('readonly', true);
                            $('#Main_ddlPremiumModeEdit').prop('disabled', false);
                        }


                    }

                });

            }

            //Populate Edit Address Contact Modal
            function PopulateEditAddressContactModal() {

                $('#Main_txtAddress1Edit').val($('#Main_txtAddress1').val());
                $('#Main_txtAddress2Edit').val($('#Main_txtAddress2').val());
                $('#Main_txtProvinceEdit').val($('#Main_txtCity').val());
                $('#Main_txtZipCodeEdit').val($('#Main_txtZipCode').val());
                $('#Main_hdfZipCodeEdit').val($('#Main_txtZipCode').val());
                $('#Main_ddlCountryEdit').val($('#Main_ddlCountry option:selected').val());
                $('#Main_txtTelephoneEdit').val($('#Main_txtTelephone').val());
                $('#Main_txtMobilePhoneEdit').val($('#Main_txtMobilePhone').val());
                $('#Main_txtEmailEdit').val($('#Main_txtEmail').val());
            }

            //Populate Edit Personal Details Modal
            function PopulateEditPersonalDetailsModal() {

                $('#Main_ddlIDTypeEdit').val($('#Main_ddlIDType').val());
                $('#Main_txtIDNoEdit').val($('#Main_txtIDNumber').val());
                $('#Main_txtSurnameKhEdit').val($('#Main_txtSurnameKh').val());
                $('#Main_txtFirstNameKhEdit').val($('#Main_txtFirstNameKh').val());
                $('#Main_txtSurnameEdit').val($('#Main_txtSurnameEng').val());
                $('#Main_txtFirstNameEdit').val($('#Main_txtFirstNameEng').val());
                $('#Main_txtFatherSurnameEdit').val($('#Main_txtSurnameFather').val());
                $('#Main_txtFatherFirstNameEdit').val($('#Main_txtFirstNameFather').val());
                $('#Main_txtMotherSurnameEdit').val($('#Main_txtSurnameMother').val());
                $('#Main_txtMotherFirstNameEdit').val($('#Main_txtFirstNameMother').val());
                $('#Main_txtPreviousSurnameEdit').val($('#Main_txtPreviousSurname').val());
                $('#Main_txtPreviousFirstNameEdit').val($('#Main_txtPreviousFirstname').val());
                $('#Main_ddlNationalityEdit').val($('#Main_ddlNationality option:selected').val());
            }

            //Populate Edit Insurance Plan Modal
            function PopulateEditInsurancePlanModal() {
                $('#Main_txtDateBirthEdit').val($('#Main_txtDateBirth').val());
                $('#Main_ddlTypeInsurancePlanEdit').val($('#Main_ddlTypeInsurancePlan').val());
                $('#Main_txtTermInsuranceEdit').val($('#Main_txtTermInsurance').val());
                $('#Main_txtInsuranceAmountRequiredEdit').val($('#Main_txtInsuranceAmountRequired').val());
                $('#Main_txtAssureeAgeEdit').val($('#Main_txtAssureeAge').val());
                $('#Main_txtPaymentPeriodEdit').val($('#Main_txtPaymentPeriod').val());
                $('#Main_txtInsuranceAmountRequiredEdit').val($('#Main_txtInsuranceAmountRequired').val());
                $('#Main_txtPremiumAmountSystemEdit').val($('#Main_txtPremiumAmountSystem').val());
                $('#Main_ddlPremiumModeEdit').val($('#Main_ddlPremiumMode option:selected').val());
                $('#Main_txtPremiumAmountEdit').val($('#Main_txtPremiumAmount').val());
                $('#Main_ddlGenderEdit').val($('#Main_hdfGender').val());
                $('#Main_txtAnnualOriginalPremiumAmountSystemEdit').val($('#Main_txtAnnualOriginalPremiumAmountSystem').val());

                //if ($('#Main_ckbPrePremiumDiscount').is(':checked')) {
                //    $('#Main_ckbIsPrePremiumDiscountEdit').prop('checked', true);
                //} else {
                //    $('#Main_ckbIsPrePremiumDiscountEdit').prop('checked', false);
                //}

                //Discount Amount
                $('#Main_txtDiscountAmountEdit').val($('#Main_txtDiscountAmount').val());
                $('#Main_txtInsurancePlanNoteEdit').val($('#Main_txtInsurancePlanNote').val());

                ProcessDiscountAmountEdit();
            }


            //Populate Q&A Gridview selected answer
            function GetQAndA() {

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/GetAnswers",
                    data: "{app_id:'" + search_app_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        SetSelectionRadioButtonList(data);
                    }

                });
            };

            //Set Gridview Radio Button List value
            function SetSelectionRadioButtonList(data) {
                var answer = "";
                var question = "";
                for (var i = 0; i < data.d.length; i++) {

                    var item = data.d[i];

                    //String collection of answers                  
                    answer = $('#Main_hdfAnswers').val();
                    answer += item.Answer + ",";
                    $('#Main_hdfQAndA').val(answer);

                    //String collection of question id
                    question = $('#Main_hdfQuestionIDs').val();
                    question += item.Question_ID + ",";
                    $('#Main_hdfQuestionIDs').val(question);

                    var myGrid = document.getElementById("<%= GvQA.ClientID %>");
                    for (var j = 0; j < myGrid.rows.length; j++) {

                        var question_id = $("input[id$=Main_GvQA_hdfQuestionID_" + j + "]").val();

                        if (item.Question_ID === question_id) {

                            for (var k = 0; k < 3; k++) {
                                var rdbl = $("input[id$=Main_GvQA_rbtnlAnswer_" + j + "_" + k + "_" + j + "]").val();

                                if (rdbl == item.Answer) {
                                    $("input[id$=Main_GvQA_rbtnlAnswer_" + j + "_" + k + "_" + j + "]").prop('checked', true);
                                }
                            }
                        }
                    }

                }
            }

            //End Application Search Section..................................................................................................

            //Edit Application Section........................................................................................................

            function EditAddressContact() {
                var app_id = $('#Main_hdfAppRegisterID').val();
                var address1 = $('#Main_txtAddress1Edit').val();
                var address2 = $('#Main_txtAddress2Edit').val();
                var province = $('#Main_txtProvinceEdit').val();
                var zip_code = $('#Main_hdfZipCodeEdit').val();
                var country = $('#Main_ddlCountryEdit option:selected').val();
                var tel = $('#Main_txtTelephoneEdit').val();
                var mobile = $('#Main_txtMobilePhoneEdit').val();
                var email = $('#Main_txtEmailEdit').val();

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/EditAddressAndContact",
                    data: "{app_id:'" + app_id + "',address1:'" + address1 + "',address2:'" + address2 + "',province:'" + province + "',zip_code:'" + zip_code + "',country:'" + country + "',tel:'" + tel + "',mobile:'" + mobile + "',email:'" + email + "'}",
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
                            $('#Main_txtZipCode').val($('#Main_hdfZipCodeEdit').val());
                            $('#Main_ddlCountry').val(country);
                            $('#Main_txtTelephone').val($('#Main_txtTelephoneEdit').val());
                            $('#Main_txtMobilePhone').val($('#Main_txtMobilePhoneEdit').val());
                            $('#Main_txtEmail').val($('#Main_txtEmailEdit').val());

                            $('#myEditAddressAndContactModal').modal('hide');

                        }
                    }

                });
            }

            function EditPersonalDetails() {

                var app_id = $('#Main_hdfAppRegisterID').val();
                var id_type = $('#Main_ddlIDTypeEdit option:selected').val();
                var id_no = $('#Main_txtIDNoEdit').val();
                var surname_kh = $('#Main_txtSurnameKhEdit').val();
                var first_name_kh = $('#Main_txtFirstNameKhEdit').val();
                var surname_en = $('#Main_txtSurnameEdit').val();
                var first_name_en = $('#Main_txtFirstNameEdit').val();
                var father_surname = $('#Main_txtFatherSurnameEdit').val();
                var father_first_name = $('#Main_txtFatherFirstNameEdit').val();
                var Mother_surname = $('#Main_txtMotherSurnameEdit').val();
                var Mother_first_name = $('#Main_txtMotherFirstNameEdit').val();
                var previous_surname = $('#Main_txtPreviousFirstNameEdit').val();
                var previous_first_name = $('#Main_txtPreviousFirstNameEdit').val();
                var nationality = $('#Main_ddlNationalityEdit option:selected').val();
                var dob = $('#Main_txtDateBirth').val();
                var gender = $('#Main_ddlGender option:selected').val();

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/EditPersonalDetails",
                    data: "{app_id:'" + app_id + "',id_type:'" + id_type + "',id_no:'" + id_no + "',surname_kh:'" + surname_kh + "',first_name_kh:'" + first_name_kh + "',surname_en:'" + surname_en + "',first_name_en:'" + first_name_en + "',father_surname:'" + father_surname + "',father_first_name:'" + father_first_name + "',Mother_surname:'" + Mother_surname + "',Mother_first_name:'" + Mother_first_name + "',previous_surname:'" + previous_surname + "',previous_first_name:'" + previous_first_name + "',nationality:'" + nationality + "',dob:'" + dob + "',gender:'" + gender + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.d === "0") {
                            alert("Edit Personal Details failed. Please try again.");

                        } else {
                            alert("Edit Personal Details successful.");

                            $('#Main_ddlIDTypeEdit').val(id_type);
                            $('#Main_txtIDNumber').val($('#Main_txtIDNoEdit').val());
                            $('#Main_txtSurnameKh').val($('#Main_txtSurnameKhEdit').val());
                            $('#Main_txtFirstNameKh').val($('#Main_txtFirstNameKhEdit').val());
                            $('#Main_txtSurnameEng').val($('#Main_txtSurnameEdit').val());
                            $('#Main_txtFirstNameEng').val($('#Main_txtFirstNameEdit').val());
                            $('#Main_txtSurnameFather').val($('#Main_txtFatherSurnameEdit').val());
                            $('#Main_txtFirstNameFather').val($('#Main_txtFatherFirstNameEdit').val());
                            $('#Main_txtSurnameMother').val($('#Main_txtMotherFirstNameEdit').val());
                            $('#Main_txtFirstNameMother').val($('#Main_txtMotherFirstNameEdit').val());
                            $('#Main_txtPreviousSurname').val($('#Main_txtPreviousFirstNameEdit').val());
                            $('#Main_txtPreviousFirstname').val($('#Main_txtPreviousFirstNameEdit').val());
                            $('#Main_ddlNationality').val(nationality);

                            $('#myEditPersonalDetailsModal').modal('hide');

                        }
                    }

                });
            }
            function EditInsurancePlan() {
                var app_id = $('#Main_hdfAppRegisterID').val();
                var dob = $('#Main_txtDateBirthEdit').val();
                var gender = $('#Main_ddlGenderEdit').val();

                var customer_age = $('#Main_txtAssureeAgeEdit').val();
                var product_id = $('#Main_ddlTypeInsurancePlanEdit').val();
                var term_insurance = $('#Main_txtTermInsuranceEdit').val();
                var pay_year = $('#Main_txtPaymentPeriodEdit').val();
                var sum_insured = $('#Main_txtInsuranceAmountRequiredEdit').val();
                var premium_system = $('#Main_txtPremiumAmountSystemEdit').val();
                var pay_mode = $('#Main_ddlPremiumModeEdit').val();
                var premium = $('#Main_txtPremiumAmountEdit').val();

                //discount amount, annual premium
                var discount_amount = $('#Main_txtDiscountAmountEdit').val();
                var annual_premium = $('#Main_txtAnnualOriginalPremiumAmountSystemEdit').val();
                var insurance_plan_note = $('#Main_txtInsurancePlanNoteEdit').val();

                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/EditInsurancePlan",
                    data: "{app_id:'" + app_id + "',dob:'" + dob + "',gender:'" + gender + "',customer_age:'" + customer_age + "',product_id:'" + product_id + "',term_insurance:'" + term_insurance + "',pay_year:'" + pay_year + "',sum_insured:'" + sum_insured + "',premium_system:'" + premium_system + "',pay_mode:'" + pay_mode + "',premium:'" + premium + "',discount_amount:'" + discount_amount + "',annual_premium:'" + annual_premium + "',insurance_plan_note:'" + insurance_plan_note + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.d === "0") {
                            alert("Edit Insurance Plan failed. Please try again.");

                        } else {

                            var original_amount = $('#Main_txtAnnualOriginalPremiumAmountSystemEdit').val();

                            var amount = $('#Main_txtPremiumAmountEdit').val();

                            if (original_amount == "") {
                                original_amount = 0;
                            }

                            $.ajax({
                                type: "POST",
                                url: "../../AppWebService.asmx/EditOriginalAmount",
                                data: "{app_id:'" + app_id + "',original_amount:'" + original_amount + "',amount:'" + amount + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {

                                    if (data.d == "0") {
                                        alert("Edit Insurance Plan failed. Please try again.");
                                    } else {
                                        alert("Edit Insurance Plan successfull.");
                                    }

                                }

                            });


                            GetAppSingleRow();

                            $('#myEditInsurancePlanModal').modal('hide');

                        }
                    }

                });
            }

            function ShowEditAddressContact() {
                if ($('#Main_hdfAppRegisterID').val() != "") {
                    PopulateEditAddressContactModal();

                    $('#myEditAddressAndContactModal').modal('show');
                } else {
                    alert("No application to edit");
                }
            }

            function ShowEditPersonalDetails() {
                if ($('#Main_hdfAppRegisterID').val() != "") {
                    PopulateEditPersonalDetailsModal();

                    $('#myEditPersonalDetailsModal').modal('show');
                } else {
                    alert("No application to edit");
                }
            }

        //Clear Content...................................................................................................................

            function Clear() {
                $('#Main_hdfAppRegisterID').val("");
                $('#Main_txtApplicationNumber').val("");
                $('#Main_txtPolicyNumber').val("");
                $('#Main_txtPaymentCode').val("");
              

                $('#Main_txtDateEntry').val("");
                $('#Main_txtDataEntryBy').val("");
              
                $('#Main_txtDateSignature').val("");
                $('#Main_txtMarketingCode').val("");
              
                $('#Main_txtMarketingName').val("");
                $('#Main_hdfMarketingName').val("");
                $('#Main_txtNote').val("");
                $('#Main_ddlIDType').prop('selectedIndex', 0);
                $('#Main_txtIDNumber').val("");
                $('#Main_txtSurnameKh').val("");
                $('#Main_txtFirstNameKh').val("");
                $('#Main_txtSurnameEng').val("");
                $('#Main_txtFirstNameEng').val("");
                $('#Main_txtSurnameFather').val("");
                $('#Main_txtFirstNameFather').val("");
                $('#Main_txtSurnameMother').val("");
                $('#Main_txtFirstNameMother').val("");
                $('#Main_txtPreviousSurname').val("");
                $('#Main_txtPreviousFirstname').val("");
                $('#Main_ddlNationality').prop('selectedIndex', 0);
                $('#Main_ddlGender').prop('selectedIndex', 0);
                $('#Main_txtDateBirth').val("");
                $('#Main_txtAddress1').val("");
                $('#Main_txtAddress2').val("");
             
                $('#Main_ddlCountry').prop('selectedIndex', 0);
                $('#Main_txtTelephone').val("");
                $('#Main_txtMobilePhone').val("");
                $('#Main_txtEmail').val("");
                $('#Main_txtNameEmployer').val("");
                $('#Main_txtNatureBusiness').val("");
                $('#Main_txtRoleAndResponsibility').val("");
                $('#Main_txtCurrentPosition').val("");
                $('#Main_txtAnualIncome').val("");

                $('#Main_ddlTypeInsurancePlan >option').remove();
                $('#Main_txtInsuranceAmountRequired').val("");
                $('#Main_txtTermInsurance').val("");
                $('#Main_hdfTermInsurance').val("");
                $('#Main_hdfAssureeAge').val("");
                $('#Main_txtAssureeAge').val("");
                $('#Main_txtPaymentPeriod').val("");
                $('#Main_hdfPaymentPeriod').val("");
                $('#Main_txtInsuranceAmountRequired').val("");
                $('#Main_txtPremiumAmountSystem').val("");
                $('#Main_hdfPremiumAmountSystem').val("");
                $('#Main_ddlPremiumMode').prop('selectedIndex', 0);
                $('#Main_txtPremiumAmount').val("");
          
                //Remove Premium Discount
                RemoveDiscount();
                RemoveDiscountEdit();

                var myGrid = document.getElementById("<%= GvQA.ClientID %>");
                for (var j = 0; j < myGrid.rows.length; j++) {

                    $("input[id$=Main_GvQA_rbtnlAnswer_" + j + "_" + 0 + "_" + j + "]").prop('checked', true);

                }
            }
          
            //Show Cancel Modal
            function ShowCancelModal() {
                var app_id = $('#Main_hdfAppRegisterID').val();
                if (app_id != "") {
                    $('#Main_lblAppNumber').text($('#Main_hdfAppNumber').val());
                    $('#myCancelAppModal').modal('show');
                } else {
                    alert("No application to cancel");
                }
            }

            //Cancel Application
            function CancelApplication() {
                var app_id = $('#Main_hdfAppRegisterID').val();
                if (app_id != "") {
                    if (confirm("Are you sure, you want to cancel this applicaton")) {
                        $('#myCancelAppModal').modal('hide');
                        var btnCancel = document.getElementById('<%= btnCancel.ClientID %>'); //dynamically click button
                        btnCancel.click();
                    }
                } else {
                    alert("No application to cancel");
                }
            }

            //End Cancel Application Section..................................................................................................

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
                    url: "../../AppWebService.asmx/GetZipCode",
                    data: "{country_value:'" + country_value + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Main_txtZipCode').val(data.d);
                        $('#Main_hdfZipCode').val(data.d);

                    }

                });
            }

            //Get Zip Code in edit
            function GetZipCodeEdit() {
                var country_value = $('#Main_ddlCountryEdit').val();
                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/GetZipCode",
                    data: "{country_value:'" + country_value + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Main_txtZipCodeEdit').val(data.d);
                        $('#Main_hdfZipCodeEdit').val(data.d);
                    }

                });
            }
            //Input Textbox txtTermInsurance
            function InputTermInsurance() {

                if ($('#Main_txtTermInsurance').val() == "") {
                    $('#Main_txtTermInsurance').val(1) //at least one year
                }

                $('#Main_hdfTermInsurance').val($('#Main_txtTermInsurance').val());


                GetPremium($('#Main_txtAssureeAge').val());
            }

            //Input Textbox txtTermInsurance (Edit)
            function InputTermInsuranceEdit() {

                if ($('#Main_txtTermInsuranceEdit').val() == "") {
                    $('#Main_txtTermInsuranceEdit').val(1) //at least one year
                }

                GetPremiumEdit($('#Main_txtAssureeAgeEdit').val());
            }

            //Check Relation (Creditor case MRTA => share = 0%)
            function CheckRelation(value) {

                if (value === 'CREDITOR') {
                    if ($('#Main_hdfProductType').val() === '2') {
                        $('#Main_txtBenefitSharePercentage').val(0);
                        $('#Main_txtBenefitSharePercentage').prop('readonly', true);
                    }
                } else {
                    $('#Main_txtBenefitSharePercentage').prop('readonly', false);
                    $('#Main_txtBenefitSharePercentage').val("");
                }
            }

            //Check Relation (Creditor case MRTA => share = 0%) (Edit)
            function CheckRelationEdit(value) {

                if (value === 'CREDITOR') {
                    if ($('#Main_hdfProductType').val() === '2') {
                        $('#Main_txtBenefitSharePercentageEdit').val(0);
                        $('#Main_txtBenefitSharePercentageEdit').prop('readonly', true);
                    }
                } else {
                    $('#Main_txtBenefitSharePercentageEdit').prop('readonly', false);
                    $('#Main_txtBenefitSharePercentageEdit').val("");
                }
            }
           
            //Get Premium With discount by index
            function GetPremiumWithDiscountByIndex(discount, discount_row_index) {
                var annual_premium = $('#Main_hdfOriginalPremiumAmountSystem').val();

                if (annual_premium == "") {
                    annual_premium = 0;
                }

                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetPremiumDiscount",
                    data: "{annual_premium:'" + annual_premium + "',discount:'" + discount + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Premium_Discount' + discount_row_index).val(data.d);

                        var premium_discount = $('#Premium_Discount' + discount_row_index).val();

                        if (premium_discount == "") {
                            premium_discount = 0;
                            $('#Premium_Discount' + discount_row_index).val("0");
                        }

                        var annual_premium2 = $('#Main_hdfOriginalPremiumAmountSystem').val();
                        if (annual_premium2 == "") {
                            annual_premium2 = 0;
                        }

                        $.ajax({
                            type: "POST",
                            url: "../../CalculationWebService.asmx/GetPremiumAfterDiscount",
                            data: "{annual_premium:'" + annual_premium2 + "',premium_discount:'" + premium_discount + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {

                                $('#Premium_After_Discount' + discount_row_index).val(data.d);
                            }

                        });
                    }

                });
            }

            //Populate All Discount for this application
            function GetPremiumDiscount() {
                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/GetPremiumDiscount",
                    data: "{app_id:'" + search_app_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        row_index_discount = 1;

                        //Remove previous discount
                        RemoveDiscount();

                        for (var i = 0; i < data.d.length; i++) {

                            var discount_item = data.d[i];


                            $('#Main_txtDiscountYear').val(discount_item.Year);


                            $('#Main_txtDiscountRate').val(discount_item.Discount_Rate);


                            $('#Main_txtPremiumDiscount').val(discount_item.Premium_Discount);


                            $('#Main_txtPremiumAfterDiscount').val(discount_item.Premium_After_Discount);


                            AddNewDiscountRow();
                        }


                        $("#tblDiscountTable *").attr("disabled", "disabled");

                        //Clear                   
                        $('#Main_txtDiscountYear').val(data.d.length + 1);
                        $('#Main_txtDiscountRate').val("");
                        $('#Main_txtPremiumDiscount').val("");
                        $('#Main_txtPremiumAfterDiscount').val("");

                    }

                });
            }

            //Populate All Discount for this application (Edit)
            function GetPremiumDiscountEdit() {
                $.ajax({
                    type: "POST",
                    url: "../../AppWebService.asmx/GetPremiumDiscount",
                    data: "{app_id:'" + search_app_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        row_index_discount_edit = 1;

                        //Remove previous discount
                        RemoveDiscountEdit();

                        for (var i = 0; i < data.d.length; i++) {

                            var discount_item = data.d[i];

                            $('#Main_txtDiscountYearEdit').val(discount_item.Year);


                            $('#Main_txtDiscountRateEdit').val(discount_item.Discount_Rate);


                            $('#Main_txtPremiumDiscountEdit').val(discount_item.Premium_Discount);


                            $('#Main_txtPremiumAfterDiscountEdit').val(discount_item.Premium_After_Discount);

                            AddNewDiscountRowEdit();
                        }


                        //Clear                   
                        $('#Main_txtDiscountYearEdit').val(data.d.length + 1);
                        $('#Main_txtDiscountRateEdit').val("");
                        $('#Main_txtPremiumDiscountEdit').val("");
                        $('#Main_txtPremiumAfterDiscountEdit').val("");

                    }

                });
            }

            // Discount on pay mode
            function ProcessDiscountAmount() {

                var discount_amount = $('#Main_txtDiscountAmount').val();

                var system_premium = $('#Main_txtPremiumAmountSystem').val();

                if (system_premium == "") {
                    system_premium = 0;
                }

                if (discount_amount == "") {
                    $('#Main_txtDiscountAmount').val("0");
                    discount_number = 0;
                }

                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetDiscountAfterDiscountByPayMode",
                    data: "{system_premium:'" + system_premium + "',discount_amount:'" + discount_amount + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtTotalPremium').val(data.d);
                    }

                });


            }

            //Discount edit on pay mode
            function ProcessDiscountAmountEdit() {

                var discount_amount_edit = $('#Main_txtDiscountAmountEdit').val();

                var system_premium_edit = $('#Main_txtPremiumAmountSystemEdit').val();

                if (system_premium_edit == "") {
                    system_premium_edit = 0;
                }

                if (discount_amount_edit == "") {
                    $('#Main_txtDiscountAmountEdit').val(0);
                    discount_amount_edit = 0;
                }

                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetDiscountAfterDiscountByPayMode",
                    data: "{system_premium:'" + system_premium_edit + "',discount_amount:'" + discount_amount_edit + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        $('#Main_txtTotalPremiumEdit').val(data.d);
                    }

                });

            }

            //Get Company
            function GetCompany(channel_id) {

                $.ajax({
                    type: "POST",
                    url: "../../ChannelWebService.asmx/GetChannelItem",
                    data: "{channel_id:'" + channel_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Main_ddlCompany').setTemplate($("#jTemplateCompany").html());
                        $('#Main_ddlCompany').processTemplate(data);
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
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Application Details</h3>

            </td>
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <h3 style="width: 100%; color: black; margin: 0; height: 25px;">Personal details of proposed insured</h3>
            </td>

        </tr>
        <tr>
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
                <table id="tblAppRegister" width="99%" style="margin-top: 15px; margin-bottom: 10px;">
                    <tr>
                        <td style="width: 30%; text-align: right">Application No.:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtApplicationNumber" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="width: 35%">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorAppNumber" runat="server" ErrorMessage="Require Applicaiton No" ControlToValidate="txtApplicationNumber" ForeColor="Red" Text="*">*</asp:RequiredFieldValidator>
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
                        <td style="text-align: right">Date of Entry:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtDateEntry" runat="server" MaxLength="15" onkeypress="return false;"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Data Entry By:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtDataEntryBy" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDataEntryBy" ErrorMessage="Require Entry By" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Date of Signature:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtDateSignature" runat="server" CssClass="datepicker" MaxLength="15"></asp:TextBox>
                        </td>
                        <td>
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
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Require Maketing Code" ControlToValidate="txtMarketingCode" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>--%>
                            <input type="button" data-toggle="modal" data-target="#myMarketingCodeList"  style="background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Name of Marketing:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtMarketingName" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMarketingName" ErrorMessage="Require Name of Marketing" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Note:</td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtNote" TextMode="MultiLine" Height="40" runat="server" MaxLength="255"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                </table>

            </td>
            <td style="width: 50%; vertical-align: top; top: 5px; border-right: 1pt solid #d5d5d5; border-bottom: 1pt solid #d5d5d5; background-color: #f6f6f6;">
               <asp:UpdatePanel runat="server" id="UpdatePanel2">
                   <ContentTemplate>
                       <fieldset>
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Require I.D No." ControlToValidate="txtIDNumber" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Surname in Khmer:</td>
                        <td>
                            <asp:TextBox ID="txtSurnameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                        </td>
                        <td style="text-align: right" class="auto-style3">First Name in Khmer:</td>
                        <td>
                            <asp:TextBox ID="txtFirstNameKh" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Surname:</td>
                        <td>
                            <asp:TextBox ID="txtSurnameEng" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Require Surname" ControlToValidate="txtSurnameEng" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td style="text-align: right" class="auto-style3">First Name:</td>
                        <td>
                            <asp:TextBox ID="txtFirstNameEng" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Require First Name" ControlToValidate="txtFirstNameEng" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                   
                    <tr>
                        <td style="text-align: right">Nationality:</td>
                        <td>
                            <asp:DropDownList ID="ddlNationality" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlNationality" ErrorMessage="Require Nationality" ForeColor="Red" InitialValue="0" Text="*">*</asp:RequiredFieldValidator>
                        </td>
                        <td class="auto-style3"></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="auto-style2">Gender:</td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="ddlGender" class="span2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGender_SelectedIndexChanged">
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="0">Female</asp:ListItem>
                            </asp:DropDownList>
                             <asp:HiddenField ID="hflGender" runat="server" />
                        </td>
                        <td class="auto-style5"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Date of Birth:</td>
                        <td>

                            <asp:TextBox ID="txtDateBirth" runat="server" MaxLength="15" CssClass="span2" onkeypress="return false;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtDateBirth" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                            (DD-MM-YYYY)
                        </td>
                        <td class="auto-style3" style="padding-top: 2px">

                            <input type="button" onclick="CalculateInsurance()"  style="background: url('../../App_Themes/functions/calculator.png') no-repeat; border: none; height: 25px; width: 25px;" />

                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Date" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" ControlToValidate="txtDateBirth" ForeColor="Red"></asp:RegularExpressionValidator>
                        </td>
                        <td></td>
                    </tr>
                </table>

                       </fieldset>
                   </ContentTemplate>
               </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="container">
                    <div id="tab-content" style="padding-top: 10px;">
                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                            <li class="active"><a href="#mailling" data-toggle="tab">Mailling Address</a></li>
                            <li><a href="#relative" data-toggle="tab">Relative</a></li>
                            <li><a href="#insuranceplan" data-toggle="tab">Insurance Plan Details</a></li>
                            <li><a href="#health" data-toggle="tab">Health Details</a></li>
                        </ul>
                        <div id="my-tab-content" class="tab-content">
                            <div class="tab-pane active" id="mailling">
                                <%--Mailling Address--%>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 14.8%; text-align: right">Address:</td>
                                        <td style="width: 85.2%">
                                            <asp:TextBox ID="txtAddress1" Width="96%" runat="server" MaxLength="50"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                             <asp:TextBox ID="txtAddress2" Width="96%" runat="server" MaxLength="50"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Mobile Phone1:</td>
                                        <td>
                                            <asp:TextBox ID="txtMobilePhone1" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="Require Mobile Phone" ControlToValidate="txtMobilePhone1" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Mobile Phone2:</td>
                                        <td>
                                            <asp:TextBox ID="txtMobilePhone2" Width="96%" runat="server" MaxLength="50" ></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">E-mail:</td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" Width="96%" runat="server" MaxLength="125"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Invalid E-mail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="tab-pane" id="relative">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                    <ContentTemplate>
                                        <fieldset>
                                            <table width="99%" style="margin-top: 15px; margin-bottom: 10px;">
                                     <tr>
                                         <td></td>
                                         <td><b>Life Insured 2</b></td>
                                         <td><b>Life Insured 3</b></td>
                                         <td><b>Life Insured 4</b></td>
                                         
                                     </tr>
                                    <tr>
                                        <td style="width: 25%; text-align: right">I.D Type:</td>
                                            <td style="width: 25%">
                                                <asp:DropDownList ID="ddlIDTypeLife2" runat="server" class="span2">
                                                    <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                    <asp:ListItem Value="1">Passport</asp:ListItem>
                                                    <asp:ListItem Value="2">Visa</asp:ListItem>
                                                    <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hfdIDTypeLife2" runat="server" />
                                            </td>
                                            <td style="width: 25%">
                                                <asp:DropDownList ID="ddlIDTypeLife3" runat="server" class="span2">
                                                    <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                    <asp:ListItem Value="1">Passport</asp:ListItem>
                                                    <asp:ListItem Value="2">Visa</asp:ListItem>
                                                    <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hfdIDTypeLife3" runat="server" />
                                            </td>
                                        <td style="width: 25%">
                                                <asp:DropDownList ID="ddlIDTypeLife4" runat="server" class="span2">
                                                    <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                    <asp:ListItem Value="1">Passport</asp:ListItem>
                                                    <asp:ListItem Value="2">Visa</asp:ListItem>
                                                    <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hfdIDTypeLife4" runat="server" />
                                            </td>
                                            
                                    </tr>
                                     <tr>
                                         <td style="width: 25%; text-align: right">ID.No:</td>
                                          <td>
                                                <asp:TextBox ID="txtIDLife2" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                            </td>
                                          <td>
                                                <asp:TextBox ID="txtIDLife3" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                            </td>
                                          <td>
                                                <asp:TextBox ID="txtIDLife4" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                            </td>

                                     </tr>
                                        <tr>
                                            <td style="text-align: right">Surname in Khmer:</td>
                                            <td>
                                                <asp:TextBox ID="txtSureNameLife2" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                <asp:HiddenField ID="hfdSureNameLife2" runat="server" />
                                            </td>
                                             <td>
                                                <asp:TextBox ID="txtSureNameLife3" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                                <asp:HiddenField ID="hfdSureNameLife3" runat="server" />
                                            </td>
                                             <td>
                                                <asp:TextBox ID="txtSureNameLife4" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                            </td>
                                            
                                           
                                        </tr>
                                      <tr>
                                         <td style="text-align: right" class="auto-style3">First Name in Khmer:</td>
                                          <td>
                                                <asp:TextBox ID="txtFirstNameLife2" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                            </td>
                                          <td>
                                                <asp:TextBox ID="txtFirstNameLife3" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                            </td>
                                          <td>
                                                <asp:TextBox ID="txtFirstNameLife4" class="span2" runat="server" MaxLength="30" Height="25px"></asp:TextBox>
                                            </td>

                                     </tr>
                                     <tr>
                                          <td style="text-align: right">Surname:</td>
                                            <td>
                                                <asp:TextBox ID="txtSureNameENLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Require Surname" ControlToValidate="txtSurnameEng" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </td>
                                          <td>
                                                <asp:TextBox ID="txtSureNameENLife3" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                          <td>
                                                <asp:TextBox ID="txtSureNameENLife4" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                     </tr>
                                    
                                        <tr>
                                           
                                            <td style="text-align: right" class="auto-style3">First Name:</td>
                                            <td>
                                                <asp:TextBox ID="txtFirstNameENLife2" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Require First Name" ControlToValidate="txtFirstNameEng" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFirstNameENLife3" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFirstNameENLife4" class="span2" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                        </tr>
                   
                                        <tr>
                                            <td style="text-align: right">Nationality:</td>
                                            <td>
                                                <asp:DropDownList ID="ddlNationalityLife2" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                    <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="ddlNationality" ErrorMessage="Require Nationality" ForeColor="Red" InitialValue="0" Text="*">*</asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hfdNationalityLife2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlNationalityLife3" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                    <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hfdNationalityLife3" runat="server" />
                                            </td>
                                             <td>
                                                <asp:DropDownList ID="ddlNationalityLife4" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourceNationality" DataTextField="Nationality" DataValueField="Country_ID">
                                                    <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hfdNationalityLife4" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right" class="auto-style2">Gender:</td>
                                            <td class="auto-style2">
                                                <asp:DropDownList ID="ddlGenderLife2" class="span2" runat="server">
                                                    <asp:ListItem Value="1">Male</asp:ListItem>
                                                    <asp:ListItem Value="0">Female</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hflGenderLife2" runat="server" />
                                            </td>
                                            <td class="auto-style2">
                                                <asp:DropDownList ID="ddlGenderLife3" class="span2" runat="server">
                                                    <asp:ListItem Value="1">Male</asp:ListItem>
                                                    <asp:ListItem Value="0">Female</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hflGenderLife3" runat="server" />
                                            </td>
                                            <td class="auto-style2">
                                                <asp:DropDownList ID="ddlGenderLife4" class="span2" runat="server">
                                                    <asp:ListItem Value="1">Male</asp:ListItem>
                                                    <asp:ListItem Value="0">Female</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hflGenderLife4" runat="server" />
                                            </td>
                                           
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">Date of Birth:</td>
                                            <td>
                                                <asp:TextBox ID="txtDOBLife2" runat="server" MaxLength="15" CssClass="span2" onkeypress="return false;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Require Date of Birth" ControlToValidate="txtDateBirth" ForeColor="Red" Text="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                (DD-MM-YYYY)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDOBLife3" runat="server" MaxLength="15" CssClass="span2" onkeypress="return false;"></asp:TextBox>
                                                (DD-MM-YYYY)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDOBLife4" runat="server" MaxLength="15" CssClass="span2" onkeypress="return false;"></asp:TextBox>
                                                (DD-MM-YYYY)
                                            </td>

                                      </tr>
                                     <tr>
                                         <td style="text-align: right">Relationship to Policy owner</td>
                                         <td>
                                             <asp:RadioButtonList ID="rblSpouse" runat="server" RepeatDirection="Horizontal" CssClass="span2">
                                                 <asp:ListItem Text ="Wife" Value="WIFE"></asp:ListItem>
                                                 <asp:ListItem Text ="Husband" Value="HUSBAND"></asp:ListItem>
                                             </asp:RadioButtonList>
                                         </td>
                                         <td >
                                             <asp:RadioButtonList ID="rblChildren" runat="server" RepeatDirection="Horizontal" CssClass="span2">
                                                 <asp:ListItem Text ="Son" Value="SON"></asp:ListItem>
                                                 <asp:ListItem Text ="Daughter" Value="DAUGHTER"></asp:ListItem>
                                             </asp:RadioButtonList>
                                         </td>
                                         <td >
                                             <asp:RadioButtonList ID="rblChildren1" runat="server" RepeatDirection="Horizontal"  CssClass="span2">
                                                 <asp:ListItem Text ="Son" Value="SON"></asp:ListItem>
                                                 <asp:ListItem Text ="Daughter" Value="DAUGHTER"></asp:ListItem>
                                             </asp:RadioButtonList>
                                         </td>
                                     </tr>

                                </table>

                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                 

                            </div>
                            <div class="tab-pane" id="insuranceplan">

                                <%--Insurance Plan--%>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                            <ContentTemplate>
                                                                <fieldset>
                                <table width="100%">
                                                 <tr>
                                                    <td style="width: 20%; text-align: right">Plan:</td>
                                                    <td colspan="4">
                                                        
                                                        
                                                                     <asp:DropDownList ID="ddlPlan" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourcePlan" DataTextField="Plan_name" DataValueField="Plan_id" AutoPostBack="true" OnSelectedIndexChanged="ddlPlan_SelectedIndexChanged" >
                                                                        <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                                                     </asp:DropDownList>
                                                               
                                                        <asp:HiddenField ID="hflPlan" runat="server" />

                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: right">Payment Mode:</td>
                                                    <td colspan="4">
                                                         <asp:DropDownList ID="ddlPaymentMode" class="span2" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSourcePaymentMode" DataTextField="Mode" DataValueField="Pay_Mode_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                                                              <asp:ListItem Selected="True" Value="-1">.</asp:ListItem>
                                                         </asp:DropDownList>
                                                        <asp:HiddenField ID="hflPaymentMode" runat="server" />
                                                    </td>
                                                   
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                     <td style="width: 20%; text-align: left">Policy Ownder:</td>
                                                     <td style="width: 20%; text-align: left">Life Insured 2:</td>
                                                     <td style="width: 20%; text-align: left">Life Insured 3:</td>
                                                     <td style="width: 20%; text-align: left">Life Insured 4:</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: right">Sum Insured (USD):</td>
                                                     <td><asp:TextBox ID="txtSumInsuredLife1" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtSumInsuredLife2" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtSumInsuredLife3" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtSumInsuredLife4" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>

                                                </tr>
                                                <tr>
                                                     <td style="width: 20%; text-align: right">Coverage Peroid:</td>
                                                     <td><asp:TextBox ID="txtCoverageYearLife1" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtCoverageYearLife2" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtCoverageYearLife3" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtCoverageYearLife4" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                    <asp:HiddenField ID="hfdCoverageYearLife1" runat="server" />
                                                    <asp:HiddenField ID="hfdCoverageYearLife2" runat="server" />
                                                    <asp:HiddenField ID="hfdCoverageYearLife3" runat="server" />
                                                    <asp:HiddenField ID="hfdCoverageYearLife4" runat="server" />
                                                </tr>
                                                <tr>
                                                     <td style="width: 20%; text-align: right">Premium Payment Peroid:</td>
                                                    <td><asp:TextBox ID="txtPaymentPeroidLife1" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtPaymentPeroidLife2" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtPaymentPeroidLife3" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtPaymentPeroidLife4" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                    
                                                     <asp:HiddenField ID="hfdPaymentPeroidLife1" runat="server" />
                                                    <asp:HiddenField ID="hfdPaymentPeroidLife2" runat="server" />
                                                    <asp:HiddenField ID="hfdPaymentPeroidLife3" runat="server" />
                                                    <asp:HiddenField ID="hfdPaymentPeroidLife4" runat="server" />
                                                </tr>
                                                 <tr>
                                                    <td style="width: 20%; text-align: right">Age:</td>
                                                     <td><asp:TextBox ID="txtAgeLife1" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox><asp:HiddenField ID="hflAgeLife1" runat="server" /></td>
                                                     <td><asp:TextBox ID="txtAgeLife2" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox><asp:HiddenField ID="hflAgeLife2" runat="server" /></td>
                                                     <td></td>
                                                     <td></td>

                                                </tr>
                                                <tr>
                                                     <td style="width: 20%; text-align: right">Premium (USD):</td>
                                                     <td><asp:TextBox ID="txtPremiumLife1" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtPremiumLife2" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td></td>
                                                     <td></td>

                                                     <asp:HiddenField ID="hflPremiumLife1" runat="server" />
                                                     <asp:HiddenField ID="hflPremiumLife2" runat="server" />
                                               
                                                </tr>
                                                 <tr>
                                                     <td style="width: 20%; text-align: right">Original Premium (USD):</td>
                                                     <td><asp:TextBox ID="txtOriginalPremiumLife1" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td><asp:TextBox ID="txtOriginalPremiumLife2" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td></td>
                                                     <td></td>
                                                  
                                                     <asp:HiddenField ID="hflOriginalPremiumLife1" runat="server" />
                                                     <asp:HiddenField ID="hflOriginalPremiumLife2" runat="server" />
                                                </tr>
                                                
                                                <tr>
                                                     <td style="width: 20%; text-align: right">Total Premium (USD):</td>
                                                     <td><asp:TextBox ID="txtTotalPremium" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td></td>
                                                     <td></td>
                                                     <td></td>
                                                </tr>
                                                 <tr>
                                                     <td style="width: 20%; text-align: right">Total Original Premium (USD):</td>
                                                     <td><asp:TextBox ID="txtTotalOriginalPremium" runat="server" CssClass="span2" ReadOnly="true" ></asp:TextBox></td>
                                                     <td></td>
                                                     <td></td>
                                                     <td></td>
                                                </tr>
                                            </table>
                                                                     </fieldset>
                                                                
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                            </div>
                           
                            <div class="tab-pane" id="health">
                                <%--Miscellaneous Health--%>
                                <table width="100%">
                                    <tr>
                                        <td width="2%"></td>
                                        <td colspan="3">
                                            <%--Gridview Question--%>
                                            <asp:GridView ID="GvQA" Width="100%" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceQuestion" OnRowDataBound="GvQA_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="Question" HeaderText="Question" ItemStyle-Width="60%" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAnswerLife1" Text="Policy Owner" runat="server"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rbtnlAnswerLife1" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:HiddenField ID="hdfSeqNumberLife1" Value='<%# Eval("Seq_Number") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfQuestionIDLife1" Value='<%# Eval("Question_ID") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAnswerLife2" Text="Life Insured 2" runat="server"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rbtnlAnswerLife2" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:HiddenField ID="hdfSeqNumberLife2" Value='<%# Eval("Seq_Number") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfQuestionIDLife2" Value='<%# Eval("Question_ID") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAnswerLife3" Text="Life Insured 3" runat="server"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rbtnlAnswerLife3" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:HiddenField ID="hdfSeqNumberLife3" Value='<%# Eval("Seq_Number") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfQuestionIDLife3" Value='<%# Eval("Question_ID") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAnswerLife4" Text="Life Insured 4" runat="server"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rbtnlAnswerLife4" runat="server" CssClass="radio" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:HiddenField ID="hdfSeqNumberLife4" Value='<%# Eval("Seq_Number") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfQuestionIDLife4" Value='<%# Eval("Question_ID") %>' runat="server" />
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
            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillData();" value="OK" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
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
    <asp:HiddenField ID="hdfAppRegisterID" runat="server" />
    <%-- End Section Hidenfields Initialize  --%>

    <!--- Section Sqldatasource--->
    <asp:SqlDataSource ID="SqlDataSourceProductType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_Type_ID, CONVERT(varchar,Product_Type_ID + replicate(' ', 50 - len(Product_Type_ID))) + '. ' + Product_Type AS myproducttype FROM dbo.Ct_Product_Type where Product_Type_ID != 4 ORDER BY Product_Type_ID"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceCountry" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Country_Name FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceNationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Nationality "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceRelationship" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM [V_Relationship_List]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_PIP_Question ORDER BY Seq_Number ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" ConnectionString="<%$  ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Pay_Mode_ID, Mode FROM Ct_Payment_Mode ORDER BY Pay_Mode_ID "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceChannel" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Channel where Status = 1 order by Created_On ASC"></asp:SqlDataSource>
     <asp:SqlDataSource ID="SqlDataSourcePlan" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Plan_ID, Plan_name FROM ct_policy_fp_plan"></asp:SqlDataSource>
      <!--- End Section Sqldatasource--->
</asp:Content>