<%@ Page Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_flat.aspx.cs" Inherits="Pages_Flate_Rate_Policy_Flat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        <li>
            <input type="button" onclick="add_new();" style="background: url('../../App_Themes/functions/add.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
       <li>
           <input type="button" onclick="clear_all();" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;"/>
       </li>
       <li>
            <input type="button" data-toggle="modal" data-target="#modal_search_customer" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
       </li>
        <li style="display: none">
            <input type="button" id="btnDelete" onclick="delete_customer();" style="background: url('../../App_Themes/functions/delete.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
       
        <li>
            <input type="button" id="btnSave" onclick="check_existing_customer();" style="background: url('../../App_Themes/functions/save.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
    </ul>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
    <script>
        //Public variable declaration
        //#Customer information variable

        var policy_state = '';

        var customer_number;
        var first_name_en;
        var last_name_en;
        var first_name_kh;
        var last_name_kh;
        var gender;
        var birth_date;
        var id_card;
        var id_type;
        var nationality;
        var mother_first_name;
        var mother_last_name;
        var mother_first_name_kh;
        var mother_last_name_kh;
        var father_first_name;
        var father_last_name;
        var father_first_name_kh;
        var father_last_name_kh;

        //#end customer information

        //#Contact variable
        var tel;
        var mobile;
        var fax;
        var email;
        //#end Contact 

        //#Address Variable
        var address1;
        var address2;
        var country;
        var zip_code;
        var province;
        //#end Address

        //#Policy Information variable
        var policy_id;
        var policy_number;
        var policy_customer;
        var policy_customer_id;
        var policy_customer_type;
        var policy_channel;
        var policy_company;
        var product;
        var product_type;
        var policy_pay_mode;
        var policy_type;
        var policy_status_type_id;
        var policy_sum_insured;
        var policy_cover_year;
        var policy_pay_year;
        var policy_effective_date;
        var policy_maturity_date;
        var policy_issued_date;
        var policy_agreement_date;
        var policy_age_insure;
        var policy_pay_up_to_age;
        var policy_assure_up_to_age;
        var policy_created_note;
        
        var policy_premium;
        var policy_discount;
        var policy_annaul_premium;
        var policy_extra_amount;
        var address_id;
        var premium_system;

        //#end Policy info

        //#Rider Information variable
        var rider_type;
        var rider_name;
        var rider_first_name_en;
        var rider_last_name_en;
        var rider_first_name_kh;
        var rider_last_name_kh;
        var rider_gender;
        var rider_birth_date;
        var rider_age = 0;
        var rider_nationality;
        var rider_relationship;
        var rider_id_type;
        var rider_id_number;

        var rider_si;
        var rider_premium;
        var rider_original_amount;
        var rider_EM_Amount;
        var rider_dis_Amount;
        //#end Rider info

        //#Beneficiary Information variable
        var ben_fullname;
        var ben_relationship;
        var ben_id_type;
        var ben_id_card;
        var ben_percentage;
        var ben_remarks;

        var total_percentage =0;
        var selected_percentage=0;
        //#end Beneficiary info
        
        //Global Variables
        var customer_search_id;
        //var exist_customer_number;
        var save_address_status = false;

        var save_complete_status = false;

        var check_result;
        var app_register_id;
        var sale_agent_code;
        var sale_agent_name;

        //Application 
        var app_number;
        var sale_agent_id;
        var date_signature;
        var created_by;
        var created_note;
        var benefit_note;
        var user_id;
        var payment_code;
        var channel_id;
        var channel_item_id;

        $(document).ready(function () {
            
            customer_number = $('#Main_txtCustNumber');
            first_name_en = $('#Main_txtFirstName');
            last_name_en = $('#Main_txtLastName');
            first_name_kh = $('#Main_txtKhFirstName');
            last_name_kh = $('#Main_txtKhLastName');
            gender = $('#<%=ddlGender.ClientID%>');
            birth_date = $('#Main_txtBirthDate');
            id_card = $('#Main_txtIDCard');
            id_type = $('#<%=ddlIDType.ClientID%>');
            nationality = $('#<%=ddlNationality.ClientID%>');
            mother_first_name = $('#Main_txtMotherFirstName');
            mother_last_name = $('#Main_txtMotherLastName');
            mother_first_name_kh = $('#Main_txtMotherKhFirstName');
            mother_last_name_kh = $('#Main_txtMotherKhLastName');
            father_first_name = $('#Main_txtFatherFirstName');
            father_last_name = $('#Main_txtFatherLastName');
            father_first_name_kh = $('#Main_txtFatherKhFirstName');
            father_last_name_kh = $('#Main_txtFatherKhLastName');

            mobile = $('#Main_txtMobile'); 
            tel = $('#Main_txtTel'); 
            fax = $('#Main_txtFax'); 
            email = $('#Main_txtEmail'); 


            address1 = $('#Main_txtAddress1');
            address2 = $('#Main_txtAddress2');
            country = $('#<%=ddlCountry.ClientID%>');
            zip_code = $('#Main_txtZipCodeLife2');
            province = $('#<%=ddlProvince.ClientID%>');

            //Application 
            app_number = $('#<%=txtApplicationNumber.ClientID%>');
            sale_agent_id =  $('#<%=txtSaleAgentCode.ClientID%>');
            date_signature = $('#<%=txtDateOfSignature.ClientID%>');
            $('#Main_hdfDataEntryBy').val($('#Main_hdfusername').val())
            created_by = $('#Main_hdfDataEntryBy');
            created_note = $('#<%=txtNote.ClientID%>');
            benefit_note = "";
            
            user_id = $('#Main_hdfuserid');
            payment_code = $('#<%=txtPaymentCode.ClientID%>');
            channel_id = $('#<%=ddlChannel.ClientID%>');
            channel_item_id = $('#<%=ddlCompany.ClientID%>');

            //policy controls
            policy_id = $('#<%=hdfPolicyID.ClientID%>');
            policy_number = $('#<%=txtPolicyNumber.ClientID%>');
            policy_channel = $('#<%=ddlChannel.ClientID%>');
            policy_company = $('#<%=ddlCompany.ClientID%>');
            product = $('#<%=ddlProduct.ClientID%>');
            policy_pay_mode = $('#<%=ddlPayMode.ClientID%>');
            policy_type = $('#<%=ddlPolicyType.ClientID%>');
            policy_status_type_id = $('#<%=ddlPolicyStatusTypeID.ClientID%>');
            policy_sum_insured = $('#<%=txtPolicySI.ClientID%>');
            policy_cover_year = $('#<%=txtCoverYear.ClientID%>');
            policy_pay_year = $('#<%=txtPayYear.ClientID%>');
            policy_effective_date = $('#<%=txtPolicyEffectiveDate.ClientID%>');
            policy_maturity_date = $('#<%=txtPolicyMaturityDate.ClientID%>');
            policy_issued_date = $('#<%=txtPolicyIssuedDate.ClientID%>');
            policy_agreement_date = $('#<%=txtPolicyAgreementDate.ClientID%>');
            policy_age_insure = $('#<%=txtPolicyAgeIssure.ClientID%>');
            policy_premium = $('#<%=txtPolicyPremium.ClientID%>');
            policy_discount = $('#<%=txtPolicyDiscountAmount.ClientID%>');
            policy_annaul_premium = $('#<%=txtPolicyOriginalAmount.ClientID%>');
            policy_extra_amount = $('#<%=txtPolicyExtraAmount.ClientID%>');
            premium_system = $('#<%=txtPolicySystemPrem.ClientID%>');
            policy_created_note = $('#<%=txtPolicyCreatedNote.ClientID%>');
            //rider
            rider_type = $('#<%=ddlRiderType.ClientID%>');
            rider_first_name_en = $('#<%=txtRiderFirstName.ClientID%>');
            rider_last_name_en = $('#<%=txtRiderLastName.ClientID%>');
            rider_first_name_kh = $('#<%=txtRiderkhFirstName.ClientID%>');
            rider_last_name_kh = $('#<%=txtRiderkhLastName.ClientID%>');
            rider_gender = $('#<%=txtRiderGender.ClientID%>');
            rider_birth_date = $('#<%=txtRiderDOB.ClientID%>');
            rider_nationality = $('#<%=txtRiderNationality.ClientID%>');
            rider_relationship = $('#<%=txtRiderRelationship.ClientID%>');
            rider_id_type = $('#<%=txtRiderIDType.ClientID%>'); 
            rider_id_number = $('#<%=txtRiderIDNumber.ClientID%>');
            rider_si = $('#<%=txtRiderSI.ClientID%>');
            rider_premium = $('#<%=txtRiderPrem.ClientID%>');
            rider_original_amount = $('#<%=txtRiderOP.ClientID%>');
            rider_EM_Amount = $('#<%=txtRiderEMAmount.ClientID%>');
            rider_dis_Amount = $('#<%=txtRiderDisAmount.ClientID%>');

            //ben
            ben_fullname = $('#<%=txtBenFullName.ClientID%>');
            ben_relationship = $('#<%=ddlBenRelationship.ClientID%>');
            ben_id_type = $('#<%=ddlBenIDType.ClientID%>');
            ben_id_card = $('#<%=txtBenIDCard.ClientID%>');
            ben_percentage = $('#<%=txtBenPercentage.ClientID%>');
            ben_remarks = $('#<%=txtBenRemarks.ClientID%>');

            //sale agent
            sale_agent_code = $('#<%=txtSaleAgentCode.ClientID%>');
            sale_agent_name = $('#<%=txtSaleAgentName.ClientID%>');
            $('#application_status').html('*');
            $(".icon-search").hide();
            $('.datepicker').datepicker();
            $('#tblContent *').attr('disabled', 'disabled');
            $('#btnSave').attr('disabled', 'disabled');
            
            
            load_exist_customer(); // load from request form call
            perminent_blank();

            birth_date.datepicker().on('changeDate', function () {
                calculate_age($(this).val(), policy_effective_date.val());
            });

            //calculate cover year
            policy_cover_year.change(function () {
                if (policy_effective_date.val() == "") {
                    alert('Effective date is required.');
                    policy_effective_date.focus();
                    return false;
                } else {
                    calc_maturiry_date(policy_effective_date.val(), $(this).val());
                }
            });

            //calculate maturity date
            policy_effective_date.datepicker().on('changeDate', function () {
                calc_maturiry_date($(this).val(), policy_cover_year.val());

                if (birth_date.val() != '') {
                    calculate_age(birth_date.val(), $(this).val());
                }else {
                    alert('Birth date is required.');
                    birth_date.focus();
                }
                
            });

            //get rider age 
            rider_birth_date.datepicker().on('changeDate', function () {
                if (policy_effective_date.val() != '') {
                    calculate_rider_age($(this).val(), policy_effective_date.val());
                }else {
                    alert('Effective date is required.');
                    policy_effective_date.focus();
                }
                
            });

            rider_type.change(function () {
                if (rider_type.val() != '')
                {
                    if (rider_type.val() != '12' && rider_type.val() != '13')
                    {
                        rider_disabled(false, false);
                    }
                    else
                    {
                        rider_disabled(false, true);
                        rider_gender.val('');
                        rider_nationality.prop('selectedIndex', 0);
                        rider_relationship.prop('selectedIndex', 0);
                        rider_id_type.prop('selectedIndex', 0);
                        rider_first_name_en.val('');
                        rider_last_name_en.val('');
                        rider_first_name_kh.val('');
                        rider_last_name_kh.val('');
                        rider_birth_date.val('');
                        rider_id_number.val('');
                    }

                    $('#btnAddRider').removeAttr('disabled');
                }
                else
                {
                    rider_disabled(true, true);
                    $('#btnAddRider').attr('disabled', true);
                }
                
            });
            
            // calcu policy maturity
            function calc_maturiry_date(effective_date, cover_year) {
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetMaturityDateByMode",
                    data: "{effective_date:'" + policy_effective_date.val() + "',cover_year:'" + cover_year + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        policy_maturity_date.val(formatJSONDate(data.d));
                    },
                    error: function (msg) {
                        alert("Problem in calculating Maturity Date");
                    }
                });
            }
            
        });

        function rider_disabled(enable, disabled) {
            //Disabled some controls, permanent disabled
            rider_si.prop('disabled', enable);
            rider_original_amount.prop('disabled', enable);
            rider_premium.prop('disabled', enable);
            rider_EM_Amount.prop('disabled', enable);
            rider_dis_Amount.prop('disabled', enable);

            rider_first_name_en.prop('disabled', disabled);
            rider_last_name_en.prop('disabled', disabled);
            rider_first_name_kh.prop('disabled', disabled);
            rider_last_name_kh.prop('disabled', disabled);
            rider_gender.prop('disabled', disabled);
            rider_birth_date.prop('disabled', disabled);
            rider_nationality.prop('disabled', disabled);
            rider_relationship.prop('disabled', disabled);
            rider_id_type.prop('disabled', disabled);
            rider_id_number.prop('disabled', disabled);

        }

        function customer_disabled(status) {
            customer_number.prop('disabled', status);
            first_name_en.prop('disabled', status);
            last_name_en.prop('disabled', status);
            first_name_kh.prop('disabled', status);
            last_name_kh.prop('disabled', status);
            nationality.prop('disabled', status);
            gender.prop('disabled', status);
            birth_date.prop('disabled', status);
            id_type.prop('disabled', status);
            id_card.prop('disabled', status);
            mother_first_name.prop('disabled', status);
            mother_last_name.prop('disabled', status);
            mother_first_name_kh.prop('disabled', status);
            mother_last_name_kh.prop('disabled', status);
            father_first_name.prop('disabled', status);
            father_last_name.prop('disabled', status);
            father_first_name_kh.prop('disabled', status);
            father_last_name_kh.prop('disabled', status);
        }

        function perminent_blank() {
            if (save_complete_status != false) {
                save_complete_status = false;
                clear_form();
            }
            
        }


        function add_new() {
            policy_state = 'NEW';
            $(".icon-search").show();
            $('#tblContent *').removeAttr('disabled');
            $('#btnSave').removeAttr('disabled');
            $('#application_status').html('<b><u> New</u></b>');

            if (rider_type.val() == '') {
                rider_disabled(true, true);
                $('#btnAddRider').attr('disabled', true);
            }
        }
        function showCompleteMessage(){
            if (save_complete_status == true) {
                swal("Done!", "You saved information successfully!", "success", {
                    button: "Ok",
                });
            }
        }

        function showFailMessage(msg)
        {
            if (save_complete_status != true) {
                swal("Failed!", ' ' + msg + ' ', "error", {
                    button: "Ok",
                });    
            }
        }

        //get parameters
        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

        //load exist customer information by query string ['cust_id']
        function load_exist_customer() {
            var cust_id_param = getParameterByName('customer_id');
            if (cust_id_param != null && cust_id_param != '') {
                customer_search_id = cust_id_param;
                load_customer(cust_id_param);
            }
            else {
                //add_new();

            }
        }

        function clear_all() {
            clear_form();
            policy_id.val('');
        }

        function clear_all_controls(){
            clear_application();
            //clear customer
            clear_customer_controls();
            //clear contact
            clear_contact_controls();
            //clear addr
            clear_address_controls();
            //clear policy
            clear_policy_controls();
            //clear rider tab
            clear_rider_controls();
            clear_rider_session();
            //clear beneficiary tab
            clear_beneficiary_controls();
            clear_ben_session();
        }

        function clear_rider_session() {
            var tbl = $('#tbl_rider_list tr:gt(0)');
            if (tbl.length > 0) {
                $('#tbl_rider_list tr').each(function (index, element) {
                    $('#row_rider_list' + index).remove();
                });
            }

            row_rider_no = 0;
            selected_rider_row_index = 0;
        }

        function clear_ben_session() {
            var tbl = $('#tbl_ben_list tr:gt(0)');
            if (tbl.length > 0) {
                $('#tbl_ben_list tr').each(function (index, element) {
                    $('#row_ben_list' + index).remove();
                });
            }

            row_ben_no = 0;
            selected_ben_row_index = 0;
            total_percentage = 0;
        }
        //load exist customer
        function load_customer(customer_no) {
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/GetCustomerByCustID",
                data: "{customer_name:'', customer_type_id:'', id_card:'', customer_id:'', customer_number:'" + customer_no + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        var customer = data.d[0];
                        customer_number.val(customer.Customer_Number);
                        first_name_en.val(customer.First_Name);
                        last_name_en.val(customer.Last_Name);
                        first_name_kh.val(customer.Khmer_First_Name);
                        last_name_kh.val(customer.Khmer_Last_Name);
                        mother_first_name.val(customer.Mother_First_Name);
                        gender.val(customer.Gender);
                        id_type.val(customer.ID_Type);
                        id_card.val(customer.ID_Card);
                        birth_date.val(formatJSONDate(customer.Birth_Date));

                        nationality.val(customer.Nationality);
                        //exist_customer_number.val(customer.Customer_Number); //purpose for using while user update record

                        //contact
                        tel.val(customer.tel);
                        mobile.val(customer.mobile);
                        email.val(customer.email);
                        fax.val(customer.fax);
                        
                        //address
                        address1.val(customer.address1);
                        address2.val(customer.address2);
                        country.val(customer.country);
                        zip_code.val(customer.zip_code);
                        
                        if (customer.Country_ID != 'KH') {
                            province.prop('disabled', true);
                        }
                        else {
                            province.prop('disabled', false);
                            province.val(customer.province);
                        }

                    } else {//not record found
                        //reset customer id from search form
                        customer_search_id = '';
                        alert('Customer not found.');
                    }
                },
                error: function (err) { alert(err); }

            });

            $('#modal_search_customer').modal('hide');
            var btn = $('#btnSearchCustomer');
            btn.val('Search');
        }

        //check existing customer number
        function check_existing_customer() {
            if (validate_form() == false) {
                return false;
            }
            if (customer_number.val() != '') { //key in customer_number
                get_customer_list(customer_number.val());

                //if (customer_number.val() != exist_customer_number.val()) {
                //    get_customer_list(customer_number.val());
                //}
                //else {
                //    save_all('update');
                //}
            }
            else { 
                get_exist_customer(first_name_en.val(), last_name_en.val(), gender.val(), id_card.val(), birth_date.val());
            }

        }

        //6.Get customer list
        function get_customer_list(customer_number) {
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/GetCustomerList",
                data: "{customer_name:'', customer_type_id:'', id_card:'', customer_id:'', customer_number:'" + customer_number + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        var record = data.d[0];
                        //alert('Customer Number [' + record.Customer_Number + '] is already exist. Do you want to add new policy on exist customer?');
                        //get_customer_info_list(data.d);
                        //policy_state = 'NEW_POL_ON_EXIST_CUSTOMER'
                        //exist_customer_info = true;
                        //customer_number = data.d;
                        //save_all('NEW_POL_ON_EXIST_CUSTOMER');

                        if (confirm('Customer Number [' + record.Customer_Number + '] is already exist! Do you want to add new policy on exist customer?')) {
                            get_customer_info_list(data.d);
                            policy_state = 'NEW_POL_ON_EXIST_CUSTOMER'
                            exist_customer_info = true;
                            customer_number.val(data.d);
                            save_all('NEW_POL_ON_EXIST_CUSTOMER');
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        get_exist_customer(first_name_en.val(), last_name_en.val(), gender.val(), id_card.val(), birth_date.val());
                        if (exist_customer_info != true) {
                            save_all('save');
                        }
                    }
                }
            });
        }

        //7.Get exist customer
        var exist_customer_info = false;
        function get_exist_customer(first_name_en, last_name_en, gender, id_card, birth_date) {
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/GetExistCustomer",
                data: "{first_name_en:'" + first_name_en + "', last_name_en:'" + last_name_en + "', gender:'" + gender + "', id_card:'" + id_card + "', birth_date:'" + birth_date + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (validate_form() == false) {
                        return false;
                    } else {
                        if (data.d != '') { // exist
                            if (confirm('Customer is already exist! Do you want to add new policy on exist customer?')) {
                                get_customer_info_list(data.d);
                                policy_state = 'NEW_POL_ON_EXIST_CUSTOMER'
                                exist_customer_info = true;
                                customer_number.val(data.d);
                                save_all('NEW_POL_ON_EXIST_CUSTOMER');
                            }
                            else {
                                return false;
                            }
                        } else {
                            if (customer_number.val() == '')
                            {
                                save_all('generate_to_save');
                            } 
                        }
                    }
                }
            });
        }

        function get_customer_info_list(cust_number)
        {
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/GetCustomerList",
                data: "{customer_name:'', customer_type_id:'', id_card:'', customer_id:'', customer_number:'" + cust_number + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d.length > 0) {
                        var customer = data.d[0];
                        customer_number.val(customer.Customer_Number);
                        first_name_en.val(customer.First_Name);
                        last_name_en.val(customer.Last_Name);
                        first_name_kh.val(customer.Khmer_First_Name);
                        last_name_kh.val(customer.Khmer_Last_Name);
                        nationality.val(customer.Nationality);
                        gender.val(customer.Gender);
                        birth_date.val(formatJSONDate(customer.Birth_Date))
                        id_type.val(customer.ID_Type);
                        id_card.val(customer.ID_Card);
                        mother_first_name.val(customer.Mother_First_Name);
                        mother_last_name.val(customer.Mother_Last_Name);
                        customer_disabled(true);
                    }
                    else {
                        
                    }
                }
            });
        }
        //Save Customers 'Main Saving'
        function save_all(option) {
            if (option == 'save') {
                if (confirm('Do you want to save this customer?')) {

                    if (save_customer() == true) { //save customer

                        if(save_address() == true){ //save address

                            if(save_app_register() == true){ //save app

                                if(save_policy_info() == true){ //save policy

                                    if(save_contact() != true){ //save contact
                                        roll_back_all();
                                        return false;
                                    }

                                    if (row_rider_no > 0) {
                                        if(save_rider() != true){ //save rider
                                            roll_back_all();
                                            return false;
                                        }
                                    }

                                    if (row_ben_no > 0) {
                                        if(save_beneficiary() != true){ //save beneficiary
                                            roll_back_all();
                                            return false;
                                        }
                                    }

                                    if(save_uw_record() != true){ // save uw
                                        roll_back_all();
                                        return false;
                                    }
                                    else 
                                    {
                                        save_complete_status = true;
                                    }

                                } else {
                                    roll_back_all();
                                }
                            }
                            else 
                            {
                                if (policy_state != 'NEW_POL_ON_EXIST_CUSTOMER') {
                                    roll_back_customer(customer_number.val());
                                    roll_back_address(address_id);
                                }
                            }

                        } 
                        else 
                        {
                            if (policy_state != 'NEW_POL_ON_EXIST_CUSTOMER') {
                                roll_back_customer(customer_number.val());
                            }
                        }
                    } 
                   
                }
            }
            else if (option == 'generate_to_save') {
                generate_new_customer_number();
            }
            else if (option == 'NEW_POL_ON_EXIST_CUSTOMER')
            {
                if(save_address() == true){ //save address

                    if(save_app_register() == true){ //save app

                        if(save_policy_info() == true){ //save policy

                            if(save_contact() != true){ //save contact
                                roll_back_all();
                                return false;
                            }

                            if (row_rider_no > 0) {
                                if(save_rider() != true){ //save rider
                                    roll_back_all();
                                    return false;
                                }
                            }

                            if (row_ben_no > 0) {
                                if(save_beneficiary() != true){ //save beneficiary
                                    roll_back_all();
                                    return false;
                                }
                            }

                            if(save_uw_record() != true){ // save uw
                                roll_back_all();
                                return false;
                            }
                            else 
                            {
                                save_complete_status = true;
                            }

                        } else {
                            alert("POLICY_ID" + policy_id.val());
                            roll_back_all();
                        }
                    }
                    else 
                    {
                        if (policy_state != 'NEW_POL_ON_EXIST_CUSTOMER') {
                            roll_back_customer(customer_number.val());
                            roll_back_address(address_id);
                        }
                    }

                } 
                else 
                {
                    if (policy_state != 'NEW_POL_ON_EXIST_CUSTOMER') {
                        roll_back_customer(customer_number.val());
                    }
                }
            }
            else if(option == 'update') {
                if (confirm("Do you update this customer?")) {
                    update_customer();
                }
            }
            showCompleteMessage(); //Saved Completed
            perminent_blank(); // Set perminant blank

        }

        //1.Save Customer
        var customer_save_status = false;
        function save_customer() {
            var customer_info = '';
            customer_info = customer_number.val() + ';' + first_name_en.val() + ';' + last_name_en.val() + ';' + last_name_kh.val() + ';' + first_name_kh.val() + ';' +
                                gender.val() + ';' + id_type.val() + ';' + id_card.val() + ';' + birth_date.val() + ';' + nationality.val() + ';' +
                                mother_first_name.val() + ';' + mother_last_name.val() + ';' + mother_first_name_kh.val() + ';' + mother_last_name_kh.val() + ';' +
                                father_first_name.val() + ';' + father_last_name.val() + ';' + father_first_name_kh.val() + ';' + father_last_name_kh.val();
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/SaveCustomer",
                data: "{customer_info:'" + customer_info + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != '') {
                        customer_save_status = true;
                    }
                    else {
                        customer_save_status = false;
                        showFailMessage('Customer was saved failed!');
                    }
                },
                error: function (err) {
                    customer_save_status = false;
                    showFailMessage('Save customer error: ' + err);
                }

            });

            return customer_save_status;
        }

        //2.Generate new customer adn save customer
        function generate_new_customer_number() {
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/GenerateNewCustomerNumber",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != '') {
                        customer_number.val(data.d);
                        save_all('save');
                    }
                    else {
                        alert('System cannot generate new customer number, please contact your system administrator.');
                        return;
                    }
                }
            });
        }

        //5.Update customer
        function update_customer() {
            customer_info = customer_number.val() + ';' + first_name_en.val() + ';' + last_name_en.val() + ';' + last_name_kh.val() + ';' + first_name_kh.val() + ';' +
                                  gender.val() + ';' + id_type.val() + ';' + id_card.val() + ';' + birth_date.val() + ';' + nationality.val() + ';' +
                                  mother_first_name.val() + ';' + mother_last_name.val() + ';' + mother_first_name_kh.val() + ';' + mother_last_name_kh.val() + ';' +
                                  father_first_name.val() + ';' + father_last_name.val() + ';' + father_first_name_kh.val() + ';' + father_last_name_kh.val();

            $.ajax({  
                type: "POST",
                url: "../../CustomerWebService.asmx/UpdateCustomer",
                data: "{customer_info:'" + customer_info + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d) {
                        alert('Updated [' + customer_number.val() + '] successfully.');
                    } else {
                        alert('Updated failed!');
                    }
                }
            });
        }

        var exist_policy = false;
        function checking_policy_by_policy_number(pol_number) {
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/CheckPolicyByPolicyNumber",
                data: "{policy_number:'" + pol_number + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        alert('Policy Number [' + pol_number + '] is existing!');
                        exist_policy = true;
                    } else {
                        exist_policy = false;
                    }
                }
            });
        }

        //Register application
        var app_register_save_status = false;
        function save_app_register() {
            if (payment_code == "") {
                payment_code.val(0);
            }
            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/RegisterApplication",
                data: "{app_number:'" + app_number.val() + "',sale_agent_id:'" + sale_agent_id.val() + "',date_signature:'" + date_signature.val() + "',created_by:'" + created_by.val() + "',created_note:'" + created_note.val() + "',created_on:'" + '' + "',benefit_note:'" + benefit_note + "',user_id:'" + user_id.val() + "',payment_code:'" + payment_code.val() + "',channel_id:'" + channel_id.val() + "',channel_item_id:'" + channel_item_id.val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != 0) {
                        app_register_save_status = true; 
                        app_register_id = data.d;
                        save_app_life_product();
                    }
                    else {
                        showFailMessage('App Registration Failed. Please check it again!');
                        return; //exit function
                    }
                        
                },
                error: function (msg) {
                    showFailMessage('Registration Error. Please contact system administrator.');
                    return; //exit function
                }
            });

            return app_register_save_status;
        }

        //save app life pro
        function save_app_life_product() {
            
            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/InsertAppLifeProduct",
                data: "{app_id:'" + app_register_id + "',customer_age:'" + policy_age_insure.val() + "',product_id:'" + product.val() + "',assure_year:'" + policy_cover_year.val() + "',pay_year:'" + policy_pay_year.val() + "',sum_insured:'" + policy_sum_insured.val() + "',premium_system:'" + premium_system.val() + "',pay_mode:'" + policy_pay_mode.val() + "',premium:'" + policy_premium.val() + "',discount_amount:'" + policy_discount.val() + "',annual_premium:'" + "" + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d == false) {
                        showFailMessage('Registration Error. Please contact system administrator.');
                        return; //exit function
                    }
                },
                error: function (msg) {
                    return; //exit function
                }
            });
        }

        //Add Underwriting new record
        var uw_save_status = false;
        function save_uw_record()
        {
            var uwRecord = new Array();
            var index = 0;
            if (app_register_id != '') { //Policy flat rate
                if (row_rider_no > 0) {
                    for (i = 1; i <= row_rider_no; i++) {
                        uwRecord[index] = policy_id.val() + ';' +
                                        product.val() + ';' +
                                        rider_age + ';' +
                                        policy_pay_year.val() + ';' +
                                        policy_pay_up_to_age + ';' +
                                        policy_cover_year.val() + ';' +
                                        policy_assure_up_to_age + ';' +
                                        $('#lbl_rider_si' + i).text() + ';' +
                                        $('#lbl_rider_premium' + i).text() + ';' +
                                        premium_system.val() + ';' +
                                        policy_pay_mode.val() + ';' +
                                        $('#lbl_rider_original_amount' + i).text() + ';' +
                                        $('#lbl_rider_em_amount' + i).text() + ';' +
                                        $('#lbl_rider_dis_amount' + i).text() + ';' +
                                        $('#lbl_rider_type' + i).text() + ';' +
                                        $('#lbl_rider_birth_date' + i).text() + ';' +
                                        $('#lbl_rider_gender' + i).text() + ';' +
                                        policy_effective_date.val() + ';' +
                                        policy_extra_amount.val() + ';' +
                                        birth_date.val() 
                        index++;
                    }

                    $.ajax({
                        type: "POST",
                        url: "../../UnderwritingWebService.asmx/UnderwritingRecord",
                        data: JSON.stringify({ UWRecords: uwRecord, App_Register_ID: app_register_id }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data.d != false) {
                                uw_save_status = true;
                            }
                            else {
                                showFailMessage('Save Error. Please contact system administrator!');
                            }
                        },
                        error: function (msg) {
                            showFailMessage(msg);
                        }
                    });
                }
                else {
                    uw_save_status = true;
                }
            }

            return uw_save_status;
        }

        //3. Save policy_info
        var policy_save_status = false;
        function save_policy_info() {
            //Check existing Policy Number
            checking_policy_by_policy_number(policy_number.val());
            if (exist_policy != true) {

                var policies = new Array();

                if (policy_discount.val() == '') {
                    policy_discount.val(0);
                }
                if (policy_extra_amount.val() == '' ) {
                    policy_extra_amount.val(0);
                }
                if (premium_system.val() == '') {
                    premium_system.val(0);
                }
                if (policy_annaul_premium.val() == '') {
                    policy_annaul_premium.val(0);
                }
                
                calc_policy_pay_up_to_age(policy_pay_year.val(), policy_age_insure.val());
                calc_policy_assure_up_to_age(policy_cover_year.val(), policy_age_insure.val());

                //ct_policy
                policies[0] = customer_number.val();
                policies[1] = product.val();
                policies[2] = policy_effective_date.val();
                policies[3] = policy_maturity_date.val();
                policies[4] = policy_agreement_date.val();
                policies[5] = policy_issued_date.val();
                policies[6] = policy_age_insure.val();
                policies[7] = policy_pay_year.val();
                policies[8] = policy_pay_up_to_age; 
                policies[9] = policy_cover_year.val();
                policies[10] = policy_assure_up_to_age;
                policies[11] = address_id;
                policies[12] = policy_company.val();
                //ct_policy_premium
                policies[13] = policy_number.val();
                policies[14] = policy_sum_insured.val();
                policies[15] = premium_system.val();  

                //save ct_policy_number
                policies[16] = policy_pay_mode.val();
                policies[17] = policy_type.val();

                policies[18] = policy_discount.val();
                policies[19] = policy_annaul_premium.val(); // Original Amount
                policies[20] = policy_extra_amount.val(); // EM Amount
                policies[21] = app_register_id;
                policies[22] = policy_created_note.val();
                policies[23] = sale_agent_id.val();
                policies[24] = policy_status_type_id.val();
                policies[25] = date_signature.val();
                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/InsertPolicyFlatRate",
                    data: JSON.stringify({ policy: policies }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        var data_list = data.d;
                        if (data_list[0] != '') {
                            policy_save_status = true;
                            policy_id.val(data_list[1]);// reserve for saving contact, riders and beneficiary
                        }
                        else {
                            policy_save_status = false;
                            policy_id.val(data_list[1]) //policy reserve rollback
                            showFailMessage('Policy saved failed!');
                        }
                    },
                    error: function (err) {
                        policy_save_status = false;
                        showFailMessage('Save policy error: ' + err);
                    }

                });
            }
            else {
                showFailMessage('Policy Number is existing!');
            }

            return policy_save_status;
        }

        //5.Save Rider
        var rider_save_status = false;
        function save_rider() {

            var riders = new Array();
            var index = 0;
            if (row_rider_no > 0) {
                for (i = 1; i <= row_rider_no; i++) {
                    riders[index] = policy_id.val() + ';' +
                                    $('#lbl_rider_si' + i).text() + ';' +
                                    $('#lbl_rider_premium' + i).text() + ';' +
                                    $('#lbl_rider_em_amount' + i).text() + ';' +
                                    $('#lbl_rider_dis_amount' + i).text() + ';' +
                                    $('#lbl_rider_original_amount' + i).text() + ';' +
                                    $('#lbl_rider_type' + i).text() + ';' +

                                    $('#lbl_rider_id_number' + i).text() + ';' +
                                    $('#lbl_rider_id_type' + i).text() + ';' +
                                    $('#lbl_rider_firstname' + i).text() + ';' +
                                    $('#lbl_rider_lastname' + i).text() + ';' +
                                    $('#lbl_rider_gender' + i).text() + ';' +
                                    $('#lbl_rider_birth_date' + i).text() + ';' +
                                    $('#lbl_rider_nationality' + i).text() + ';' +
                                    $('#lbl_rider_firstname_kh' + i).text() + ';' +
                                    $('#lbl_rider_lastname_kh' + i).text() + ';' +
                                    $('#lbl_rider_relationship' + i).text() + ';' +
                                    app_register_id
                    index++;
                }

                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/SaveRiders",
                    data: JSON.stringify({ riders: riders }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d) {
                            rider_save_status = true;
                        }
                        else {
                            rider_save_status = false;
                            showFailMessage('Rider Saved failed!');
                        }
                    }
                });
            }

            return rider_save_status;
        }

        //6. Save Beneficiary
        var ben_save_status = false;
        function save_beneficiary() {
            var beneficiaries = new Array();
            var index = 0;
            if (row_ben_no > 0) {
                for (i = 1; i <= row_ben_no; i++){
                    beneficiaries[index] = policy_id.val() + ';' +
                                    $('#lbl_ben_fullname' + i).text() + ';' +
                                    $('#lbl_ben_relationship' + i).text() + ';' +
                                    $('#lbl_ben_id_type' + i).text() + ';' +
                                    $('#lbl_ben_id_card' + i).text() + ';' +
                                    $('#lbl_ben_percentage' + i).text() + ';' +
                                    $('#lbl_ben_remarks' + i).text()
                    index++;
                }

                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/SaveBeneficiary",
                    data: JSON.stringify({ beneficiaries: beneficiaries }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d) {
                            ben_save_status = true;
                        }
                        else {
                            ben_save_status = false;
                            showFailMessage('Beneficiary Saved failed!');
                        }
                    }

                });
            }

            return ben_save_status;
        }

        //4. Save Contact
        var con_save_status = false;
        function save_contact() {
            var contacts = new Array();

            if (policy_id.val() != '') {

                contacts[0] = policy_id.val();
                contacts[1] = mobile.val();
                contacts[2] = tel.val();
                contacts[3] = fax.val();
                contacts[4] = email.val();
                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/SaveContact",
                    data: JSON.stringify({ contact: contacts }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d) {
                            con_save_status = true;
                        }
                        else {
                            con_save_status = false;
                            showFailMessage('Customer was saved, but contact was saved failed!');
                        }
                    },
                    error: function (err) {
                        con_save_status = false;
                        showFailMessage('Save contact error: ' + err);
                    }

                });
            }

            return con_save_status;
        }

        //2. Save Address
        var address_save_status = false;
        function save_address() {

            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/SaveAddress",
                data: "{country_id:'" + country.val() + "',province:'" + province.val() + "', zip_code:'" + zip_code.val() + "', address1:'" + address1.val() + "', address2:'" + address2.val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != '') {
                        address_save_status = true;
                        address_id = data.d;
                    }
                    else {
                        address_save_status = false;
                        showFailMessage('Customer was saved, but address was saved failed!');
                    }
                },
                error: function (err) {
                    address_save_status = false;
                    showFailMessage('Save address error: ' + err);
                }

            });
                
            return address_save_status;
        }

        //add rider into rider list
        var row_rider_no = 0;
        function add_rider() {
            if (validate_rider() == false) {
                return false;
            }
            if (rider_type.val() == '13') {
                rider_name = 'AD';
            }else if(rider_type.val() == '12'){
                rider_name = 'TPD';
            }else if(rider_type.val() == '2'){
                rider_name = 'Spouse';
            }else if(rider_type.val() == '3'){
                rider_name = 'Kid 1';
            }else if(rider_type.val() == '4'){
                rider_name = 'Kid 2';
            }else if(rider_type.val() == '5'){
                rider_name = 'Kid 3';
            }
            else {
                rider_name = 'Kid 4';
            }
            if (rider_id_number.val() == '') {
                rider_id_number.val('N/A');
            }

            if (rider_si.val() == '') {
                rider_si.val(0);
            }
            if (rider_premium.val() == '') {
                rider_premium.val(0);
            }
            if (rider_original_amount.val() == '') {
                rider_original_amount.val(0);
            }
            if (rider_EM_Amount.val() == '') {
                rider_EM_Amount.val(0);
            }
            if (rider_dis_Amount.val() == '') {
                rider_dis_Amount.val(0);
            }
            if (selected_rider_row_index == 0) { //add new record
                row_rider_no += 1
                var tbl_rider_list;

                tbl_rider_list += "<tr id='row_rider_list" + row_rider_no + "'>" +
                            "<td style='display:none;'> <lable id='lbl_rider_no" + row_rider_no + "'>" + row_rider_no + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; '><label id='lbl_rider_fullname" + row_rider_no + "' style='width:100px;'>" + rider_first_name_en.val() + " " + rider_last_name_en.val() + "</label></td>" +
                            "<td style='display:none; padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_rider_firstname" + row_rider_no + "'>" + rider_first_name_en.val() + "</label></td>" +
                            "<td style='display:none; padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_rider_lastname" + row_rider_no + "'>" + rider_last_name_en.val() + "</label></td>" +
                            "<td style='display:none; padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_rider_firstname_kh" + row_rider_no + "'>" + rider_first_name_kh.val() + "</label></td>" +
                            "<td style='display:none; padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_rider_lastname_kh" + row_rider_no + "'>" + rider_last_name_kh.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_gender" + row_rider_no + "'>" + rider_gender.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_rider_birth_date" + row_rider_no + "'>" + rider_birth_date.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_nationality" + row_rider_no + "'>" + rider_nationality.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_rider_relationship" + row_rider_no + "'>" + rider_relationship.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_id_type" + row_rider_no + "'>" + rider_id_type.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_id_number" + row_rider_no + "'>" + rider_id_number.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_name" + row_rider_no + "'>" + rider_name + "</label></td>" +
                            "<td style='display:none; padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_type" + row_rider_no + "'>" + rider_type.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_si" + row_rider_no + "'>" + rider_si.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_premium" + row_rider_no + "'>" + rider_premium.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_original_amount" + row_rider_no + "'>" + rider_original_amount.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_em_amount" + row_rider_no + "'>" + rider_EM_Amount.val() + "</label></td>" +
                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center;'><label id='lbl_rider_dis_amount" + row_rider_no + "'>" + rider_dis_Amount.val() + "</label></td>" +
                            "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                            "<button type='button' class='btn btn-danger' id='btn_policy_delete" + row_rider_no + "' value='Delete' onclick='remove_rider(" + row_rider_no + ");' ><span class='icon-trash'></span></button>" +
                            "<button type='button' class='btn btn-success' id='btn_policy_select" + row_rider_no + "' value='Select' onclick='select_rider(" + row_rider_no + ");' ><span class='icon-edit'></span></button></td" +
                            "</tr>";

                $('#tbl_rider_list').append(tbl_rider_list);
                
            }
            else { //update list
                    
                if (rider_type.val() == '12' || rider_type.val() == '13') {
                    $('#lbl_rider_firstname' + selected_rider_row_index).text('');
                    $('#lbl_rider_fullname' + selected_rider_row_index).text('');
                    $('#lbl_rider_lastname' + selected_rider_row_index).text('');
                    $('#lbl_rider_lastname_kh' + selected_rider_row_index).text('');
                    $('#lbl_rider_lastname_kh' + selected_rider_row_index).text('');
                    $('#lbl_rider_gender' + selected_rider_row_index).text('');
                    $('#lbl_rider_birth_date' + selected_rider_row_index).text('');
                    $('#lbl_rider_nationality' + selected_rider_row_index).text('');
                    $('#lbl_rider_relationship' + selected_rider_row_index).text('');
                    $('#lbl_rider_id_type' + selected_rider_row_index).text('');
                    $('#lbl_rider_id_number' + selected_rider_row_index).text('');
                }
                else 
                {
                    $('#lbl_rider_firstname' + selected_rider_row_index).text(rider_first_name_en.val());
                    $('#lbl_rider_lastname' + selected_rider_row_index).text(rider_last_name_en.val());
                    $('#lbl_rider_fullname' + selected_rider_row_index).text(rider_first_name_en.val() + ' ' + rider_last_name_en.val());
                    
                    $('#lbl_rider_lastname_kh' + selected_rider_row_index).text(rider_first_name_kh.val());
                    $('#lbl_rider_lastname_kh' + selected_rider_row_index).text(rider_last_name_kh.val());
                    $('#lbl_rider_gender' + selected_rider_row_index).text(rider_gender.val());
                    $('#lbl_rider_birth_date' + selected_rider_row_index).text(rider_birth_date.val());
                    $('#lbl_rider_nationality' + selected_rider_row_index).text(rider_nationality.val());
                    $('#lbl_rider_relationship' + selected_rider_row_index).text(rider_relationship.val());
                    $('#lbl_rider_id_type' + selected_rider_row_index).text(rider_id_type.val());
                    $('#lbl_rider_id_number' + selected_rider_row_index).text(rider_id_number.val());       
                }

                $('#lbl_rider_type' + selected_rider_row_index).text(rider_type.val());
                $('#lbl_rider_name' + selected_rider_row_index).text(rider_name);
                $('#lbl_rider_si' + selected_rider_row_index).text(rider_si.val());
                $('#lbl_rider_premium' + selected_rider_row_index).text(rider_premium.val());
                $('#lbl_rider_original_amount' + selected_rider_row_index).text(rider_original_amount.val());
                $('#lbl_rider_em_amount' + selected_rider_row_index).text(rider_EM_Amount.val());
                $('#lbl_rider_dis_amount' + selected_rider_row_index).text(rider_dis_Amount.val());
            }

            clear_rider_controls();

        }

        ////select rider row
        var selected_rider_row_index = 0;
        function select_rider(row_index) {
            selected_rider_row_index = row_index;//use for updating contact
            $('#tbl_rider_list tr').each(function (index, element) {
                $('#row_rider_list' + index).removeClass('select_rider');
            });
            $('#row_rider_list' + row_index).addClass('select_rider');
            rider_type.val($('#lbl_rider_type' + row_index).text());
            rider_first_name_en.val($('#lbl_rider_firstname' + row_index).text());
            rider_last_name_en.val($('#lbl_rider_lastname' + row_index).text());
            rider_first_name_kh.val($('#lbl_rider_firstname_kh' + row_index).text());
            rider_last_name_kh.val($('#lbl_rider_lastname_kh' + row_index).text());
            rider_gender.val($('#lbl_rider_gender' + row_index).text());
            rider_birth_date.val($('#lbl_rider_birth_date' + row_index).text());
            rider_nationality.val($('#lbl_rider_nationality' + row_index).text());
            rider_relationship.val($('#lbl_rider_relationship' + row_index).text());
            rider_id_type.val($('#lbl_rider_id_type' + row_index).text());
            rider_id_number.val($('#lbl_rider_id_number' + row_index).text());
            rider_si.val($('#lbl_rider_si' + row_index).text());
            rider_premium.val($('#lbl_rider_premium' + row_index).text());
            rider_original_amount.val($('#lbl_rider_original_amount' + row_index).text());
            rider_EM_Amount.val($('#lbl_rider_em_amount' + row_index).text());
            rider_dis_Amount.val($('#lbl_rider_dis_amount' + row_index).text());
            if (rider_type.val() != '12' && rider_type.val() != '13')
            {
                rider_disabled(false, false);
            }
            else {
                rider_disabled(false, true);
            }

            $('#btnAddRider').removeAttr('disabled');
                
        }

        ////remove item from policy list
        function remove_rider(row_index) {
            if(confirm('Do you want to remove this rider?'))
            {
                $('#row_rider_list' + row_index).remove();
                //reset selected row index if user selectd record
                selected_rider_row_index = 0;
                row_rider_no -= 1;
            }
        }
        
        //add beneficiary
        var row_ben_no = 0;
        var share_total = 100;
        function add_beneficiary()
        {
            //check maximum percentage [max =100%]
            var max_percentage =(total_percentage + parseInt(ben_percentage.val())) - selected_percentage;
            
            if (max_percentage <= share_total) { // allow share percentage only 100%
                if (validate_ben() == false) {
                    return false;
                }
                if (id_card.val() == '') {
                    id_card.val('N/A');
                }

                total_percentage += parseInt(ben_percentage.val()) - selected_percentage;

                if (selected_ben_row_index == 0) { //add new record
                    row_ben_no += 1;
                    var tbl_ben_list;
                    tbl_ben_list += "<tr id='row_ben_list" + row_ben_no + "'>" +
                                    "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center'> <lable id='lbl_ben_no" + row_ben_no + "'>" + row_ben_no + "</label></td>" +
                                    "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center'><label id='lbl_ben_fullname" + row_ben_no + "'>" + ben_fullname.val() + "</label></td>" +
                                    "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center'><label id='lbl_ben_relationship" + row_ben_no + "'>" + ben_id_type.val() + "</label></td>" +
                                    "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center'><label id='lbl_ben_id_type" + row_ben_no + "'>" + ben_id_card.val() + "</label></td>" +
                                    "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center'><label id='lbl_ben_id_card" + row_ben_no + "'>" + ben_relationship.val() + "</label></td>" +
                                    "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center'><label id='lbl_ben_percentage" + row_ben_no + "'>" + ben_percentage.val() + "</label></td>" +
                                    "<td style='padding:5px; height:20px; width:100px; vertical-align:middle; text-align:center'><label id='lbl_ben_remarks" + row_ben_no + "'>" + ben_remarks.val() + "</label></td>" +
                                    "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                                    "<button type='button' class='btn btn-danger' id='btn_ben_delete" + row_ben_no + "' value='Delete' onclick='remove_ben(" + row_ben_no + ");' ><span class='icon-trash'></span></button>" +
                                    "<button type='button' class='btn btn-success' id='btn_ben_select" + row_ben_no + "' value='Select' onclick='select_ben(" + row_ben_no + ");' ><span class='icon-edit'></span></button></td" +
                                    "</tr>";
                    $('#tbl_ben_list').append(tbl_ben_list);
                }
                else //update
                {
                    $('#lbl_ben_fullname' + selected_ben_row_index).text(ben_fullname.val());
                    $('#lbl_ben_relationship' + selected_ben_row_index).text(ben_relationship.val());
                    $('#lbl_ben_id_type' + selected_ben_row_index).text(ben_id_type.val());
                    $('#lbl_ben_id_card' + selected_ben_row_index).text(ben_id_card.val());
                    $('#lbl_ben_percentage' + selected_ben_row_index).text(ben_percentage.val());
                    $('#lbl_ben_remarks' + selected_ben_row_index).text(ben_remarks.val());       
                }

            }else {
                alert ('Percentage is not allowed over 100%.');
            }
            

            clear_beneficiary_controls();
        }

        //selected ben row
        var selected_ben_row_index = 0;
        function select_ben(row_index) {
            selected_ben_row_index = row_index;//use for updating contact
            $('#tbl_ben_list tr').each(function (index, element) {
                $('#row_ben_list' + index).removeClass('select_ben');
            });
            $('#row_ben_list' + row_index).addClass('select_ben');
            ben_fullname.val($('#lbl_ben_fullname' + row_index).text());
            ben_relationship.val($('#lbl_ben_relationship' + row_index).text());
            ben_id_type.val($('#lbl_ben_id_type' + row_index).text());
            ben_id_card.val($('#lbl_ben_id_card' + row_index).text());
            //ben_percentage.val($('#lbl_ben_percentage' + row_index).text());
            ben_remarks.val($('#lbl_ben_remarks' + row_index).text());

            selected_percentage = $('#lbl_ben_percentage' + row_index).text();
            ben_percentage.val(selected_percentage);
        }

        //remove item from ben list
        function remove_ben(row_index) {
            if(confirm('Do you want to remove this beneficiary?'))
            {
                total_percentage -= parseInt($('#lbl_ben_percentage' + row_index).text());
                $('#row_ben_list' + row_index).remove();
                selected_ben_row_index = 0;
                row_ben_no -= 1;
            }
        }

        function get_zip_code(country_id) {

            if (country_id != 'KH') {
                province.attr('disabled', true);
                province.prop('selectedIndex', 0);
            }
            else {
                province.attr('disabled', false);
            }
            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/GetZipCode",
                data: "{country_value:'" + country_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtZipCodeLife2').val(data.d);
                }

            });

        }   
        var customer_age = 0;
        function calculate_age(birth_date, compare_date) {
            if (birth_date.trim() != '') {
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetCustomerAge",
                    data: "{dob:'" + birth_date + "',date_of_entry:'" + compare_date + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {

                        customer_age = data.d;
                        policy_age_insure.val(data.d);
                    },
                    error: function (msg) {
                        alert("Problem in calculating age");
                    }
                });
            }
            else {
                policy_age_insure.val(0);
            }
        }

        function calculate_rider_age(birth_date, compare_date) {
            if (birth_date.trim() != '') {
                $.ajax({
                    type: "POST",
                    url: "../../CalculationWebService.asmx/GetCustomerAge",
                    data: "{dob:'" + birth_date + "',date_of_entry:'" + compare_date + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        rider_age = data.d;
                    },
                    error: function (msg) {
                        alert("Problem in calculating age");
                    }
                });
            }
            else {
                rider_age = 0;
            }
        }

        // calcu pay up to age
        function calc_policy_pay_up_to_age(pay_year, age_insure) {
            if (pay_year == '') {
                policy_pay_up_to_age = 0;
            }
            else {
                policy_pay_up_to_age = parseInt(pay_year) + parseInt(age_insure);
            }
        }

        //calcu assure up to age
        function calc_policy_assure_up_to_age(cover_pay, age_insure) {
            if (cover_pay == '') {
                policy_assure_up_to_age = 0;
            }
            else {
                policy_assure_up_to_age = parseInt(cover_pay) + parseInt(age_insure);
            }
        }

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

        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        function validate_form() {
            //validate customer
            if (validate_customer() == false) {
                return false;
            }else if (validate_contact() == false) {
                return false
            }else if (validate_application() == false) {
                return false;
            } else if (validate_policy() == false) {
                return false
            } else if (validate_address() == false) {
                return false
            } else if (validate_rider() == false) {
                return false
                //} else if (validate_ben() == false) {
                //    return false
            } else {
                return true;
            }

        }

        //Validate customer 
        function validate_customer() {
            if (first_name_en.val() == '') {
                alert('[First Name] is required.');
                first_name_en.focus();
                return false;
            } else if (last_name_en.val() == '') {
                alert('[Last Name] is required.');
                last_name_en.focus();
                return false;
            } else if (first_name_kh.val() == '') {
                alert('[Khmer first Name] is required.');
                first_name_kh.focus();
                return false;
            } else if (last_name_kh.val() == '') {
                alert('[Last Name] is required.');
                last_name_kh.focus();
                return false;
            }
            else if (nationality.val() == '') {
                alert('[Nationality] is required.');
                nationality.focus();
                return false;
            }
            else if (gender.val() == '') {
                alert('[Gender] is required.');
                gender.focus();
                return false;
            }
            else if (birth_date.val() == '') {
                alert('[Birth Date] is required.');
                birth_date.focus();
                return false;
            }
            else if (id_card.val() == '') {
                alert('[ID Card] is required.');
                id_card.focus();
                return false;
            }
            else if (id_type.val() == '') {
                alert('[ID Type] is required.');
                id_type.focus();
                return false;
            } else {
                return true;
            }
        }

        //varlidate contact
        function validate_contact() {
            if (mobile.val() == '') {
                alert('[Mobile] is required.');
                mobile.focus();
                return false;
            } else {
                return true;
            }
        }

        //varlidate contact
        function validate_address() {
            if (country.val() == '') {
                alert('[Country] is required.');
                country.focus();
                return false;
            }else if (country.val() == 'KH') {
                if (province.val() == '') {
                    alert('[Province/City] is required.');
                    province.focus();
                    return false;
                }
            }
            if (zip_code.val() == '') {
                alert('[Zip Code] is required.');
                zip_code.focus();
                return false;
            } else {
                return true;
            }
        }

        //validation applicatiom
        function validate_application(){
            if (app_number.val() == '') {
                alert('Application Number is required!');
                app_number.focus();
                return false;
            }else if (date_signature.val() == '') {
                alert('Date Signature is required!');
                date_signature.focus();
                return false;
            }else if (sale_agent_name.val() == '') {
                alert('Sale Agent is required!');
                sale_agent_name.focus();
                return false;
            }else {
                return true;
            }
        }

        //validation policy
        function validate_policy() {
            if (policy_channel.val().trim() == '') {
                alert('Channel is required.');
                policy_channel.focus();
                return false;
            }else if (policy_sum_insured.val().trim() == '') {
                alert('Sum insured is required.');
                policy_sum_insured.focus();
                return false;
            }else if (policy_pay_mode.val().trim() == '') {
                alert('Pay mode is required.');
                policy_pay_mode.focus();
                return false;
            }else if (product.val().trim() == '') {
                alert('Product is required.');
                product.focus();
                return false;
            }else if (policy_premium.val().trim() == '') {
                alert('Premium is required.');
                policy_premium.focus();
                return false;
            }else if (policy_type.val().trim() == '') {
                alert('Policy type is required.');
                policy_type.focus();
                return false;
            }else if (policy_status_type_id.val().trim() == '') {
                alert('Policy Status is required.');
                policy_status_type_id.focus();
                return false;
            }else if (policy_cover_year.val().trim() == '') {
                alert('Cover Year is required.');
                policy_cover_year.focus();
                return false;
            }else if (policy_pay_year.val().trim() == '') {
                alert('Pay year is required.');
                policy_pay_year.focus();
                return false;
            }else if (policy_effective_date.val().trim() == '') {
                alert('Effective date is required.');
                policy_effective_date.focus();
                return false;
            }else if (policy_issued_date.val().trim() == '') {
                alert('issued date is required.');
                policy_issued_date.focus();
                return false;
            }else if (policy_agreement_date.val().trim() == '') {
                alert('Agreement date is required.');
                policy_agreement_date.focus();
                return false;
            }else {
                return true;
            }

        }

        //Validate rider
        function validate_rider() {
            if (rider_type.val() != '')
            {
                if (rider_type.val() != '12' && rider_type.val() != '13') {
                    if (rider_first_name_en.val() == '') {
                        alert('[First Name] is required.');
                        rider_first_name_en.focus();
                        return false;
                    }
                    else if (rider_last_name_en.val() == '') {
                        alert('[Last Name] is required.');
                        rider_last_name_en.focus();
                        return false;
                    }
                    else if (rider_gender.val() == '') {
                        alert('Gender is required.');
                        rider_gender.focus();
                        return false;
                    }
                    else if (rider_birth_date.val() == '') {
                        alert('Birth date is required.');
                        rider_birth_date.focus();
                        return false;
                    }
                    //else if (rider_id_number.val() == '') {
                    //    alert('ID Number is required.');
                    //    rider_id_number.focus();
                    //    return false;
                    //}
                    else if (rider_nationality.val() == '') {
                        alert('Nationality is required.');
                        rider_nationality.focus();
                        return false;
                    }
                    else if (rider_relationship.val() == '') {
                        alert('Relationship is required.');
                        rider_relationship.focus();
                        return false;
                    }
                    else if (rider_si.val() == '') {
                        alert('Sum insured is required.');
                        rider_si.focus();
                        return false;
                    }
                    else if (rider_premium.val() == '') {
                        alert('Premium is required.');
                        rider_premium.focus();
                        return false;
                    }
                    else if (rider_original_amount.val() == '') {
                        alert('Original Amount is required.');
                        rider_original_amount.focus();
                        return false;
                    }
                    else {
                        return true;
                    }
                } else {
                    if (rider_si.val() == '') {
                        alert('Sum insured is required.');
                        rider_si.focus();
                        return false;
                    }
                    else if (rider_premium.val() == '') {
                        alert('Premium is required.');
                        rider_premium.focus();
                        return false;
                    }
                    else if (rider_original_amount.val() == '') {
                        alert('Original Amount is required.');
                        rider_original_amount.focus();
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        //Validate rider
        function validate_ben() {
            if (ben_fullname.val() == '') {
                alert('[Full name] is required.');
                ben_fullname.focus();
                return false;
            }else if(ben_relationship.val() == ''){
                alert('[Relationship] is required.');
                ben_relationship.focus();
                return false;
            //} else if (ben_id_card.val() == '') {
            //    alert('[ID Card] is required.');
            //    ben_id_card.focus();
            //    return false;
            } else if (ben_id_type.val() == '') {
                alert('[ID Type] is required.');
                ben_id_type.focus();
                return false;
            } else if (ben_percentage.val() == '') {
                alert('[Share %] is required.');
                ben_percentage.focus();
                return false;
            } else {
                return true;
            }
        }

        function clear_form() {
            $('select').prop('selectedIndex', 0); //set all dropdown list select index 0
            $("input[type=text]").val(""); //clear all textboxes

            //exist_customer_number.val('');
            clear_application();
            //reset variables
            customer_search_id = '';

            $('#dvCustomerSearchList').html('');

            //clear customer
            clear_customer_controls();
            //clear contact
            clear_contact_controls();
            //clear addr
            clear_address_controls();
            //clear policy
            clear_policy_controls();
            //clear rider tab
            clear_rider_controls();
            clear_rider_session();
            //clear beneficiary tab
            clear_beneficiary_controls();
            clear_ben_session();
            $('#tblContent *').attr('disabled', 'disabled');
            $(".icon-search").hide();
            $('#btnSave').attr('disabled', 'disabled');

        }

        function clear_application(){
            app_register_id = '';
            $('#Main_txtApplicationNumberModal').val('');
            app_number.val('');
            save_complete_status = false;
            sale_agent_id.val('');
            sale_agent_name.val('');
            date_signature.val('');
            created_by.val('');
            created_note.val('');
            benefit_note = "";
            user_id.val('');
            payment_code.val('');
            sale_agent_code_search ='';
            sale_agent_name_search='';
        }

        function clear_customer_controls() {
            customer_number.val('');
            first_name_en.val('');
            last_name_en.val('');
            first_name_kh.val('');
            last_name_kh.val('');
            birth_date.val('');
            id_card.val('');
            mother_first_name.val('');
            mother_last_name.val('');
            mother_first_name_kh.val('');
            mother_last_name_kh.val('');
            father_first_name.val('');
            father_last_name.val('');
            father_first_name_kh.val('');
            father_last_name_kh.val('');
            id_type.prop('selectedIndex', 0);
            gender.val('');
            nationality.prop('selectedIndex', 0);
        }

        function clear_policy_controls() {
            policy_id.val('');
            policy_number.val('');
            policy_customer = '';
            policy_customer_id= '';
            policy_customer_type = '';
            product.prop('selectedIndex', 0);
            policy_channel.prop('selectedIndex', 0);
            policy_company.prop('selectedIndex', 0);
            policy_pay_mode.prop('selectedIndex', 0);
            policy_type.prop('selectedIndex', 0);
            policy_status_type_id.prop('selectedIndex', '');
            policy_sum_insured.val('');
            policy_cover_year.val('');
            policy_pay_year.val('');
            policy_effective_date.val('');
            policy_issued_date.val('');
            policy_agreement_date.val('');
            policy_age_insure.val('');
            policy_pay_up_to_age = '';
            policy_assure_up_to_age ='';
            policy_premium.val('');
            policy_discount.val('');
            policy_annaul_premium.val('');
            policy_extra_amount.val('');

        }

        function clear_contact_controls() {
            tel.val('');
            mobile.val('');
            fax.val('');
            email.val('');
        }

        function clear_address_controls() {
            address1.val('');
            address2.val('');
            country.prop('selectedIndex', 0);
            zip_code.prop('selectedIndex', 0);
            province.prop('selectedIndex', 0);
          
        }

        function clear_rider_controls() {

            rider_type.prop('selectedIndex', 0);
            rider_gender.val('');
            rider_nationality.prop('selectedIndex', 0);
            rider_relationship.prop('selectedIndex', 0);
            rider_id_type.prop('selectedIndex', 0);
            rider_first_name_en.val('');
            rider_last_name_en.val('');
            rider_first_name_kh.val('');
            rider_last_name_kh.val('');
            rider_birth_date.val('');
            rider_id_number.val('');
            rider_si.val('');
            rider_premium.val('');
            rider_original_amount.val('');
            rider_EM_Amount.val('');
            rider_dis_Amount.val('');
            $('#btnAddRider').attr('disabled', true);

            rider_disabled(true, true);

            selected_rider_row_index = 0;

        }

        function clear_beneficiary_controls() {
            //clear_ben_session();
            ben_fullname.val('');
            ben_relationship.prop('selectedIndex', 0);
            ben_id_card.val('');
            ben_id_type.prop('selectedIndex', 0);
            ben_percentage.val('');
            ben_remarks.val('');

            selected_ben_row_index = 0;
            selected_percentage=0;
            $('#tbl_ben_list tr').each(function (index, element) {
                $('#row_ben_list' + index).removeClass('select_ben');
            });

        }

        //clear ben boxes
        function clear_rider_boxes() {
            clear_rider_controls();
            clear_rider_session();
        }

        //clear ben boxes
        function clear_ben_boxes() {
            clear_beneficiary_controls();
            clear_ben_session();
        }
            

        function roll_back_all()
        {
            roll_back(policy_id.val());
            roll_back_address(address_id);
            if (policy_state != 'NEW_POL_ON_EXIST_CUSTOMER') {
                roll_back_customer(customer_number.val());
            }
        }

        //Roll back policy
        function roll_back(policy_id) {
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/RollBack",
                data: JSON.stringify({ policy_id: policy_id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    //alert('Rollback');
                },
                error: function (err) {
                    alert('System rollback error: ' + err);
                }
            });
        }

        //Roll back Customer
        function roll_back_customer(customer_id) {
            //alert('Rollback Customer ID:' + customer_id);
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/RollBack",
                data: JSON.stringify({ customer_id: customer_id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function (err) {
                    alert('System rollback error: ' + err);
                }
            });
        }

        //Roll back policy
        function roll_back_address(address_id) {
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/RollBackAddress",
                data: JSON.stringify({ address_id: address_id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                },
                error: function (err) {
                    alert('System rollback error: ' + err);
                }
            });
        }

        function GetCompany(channel_id) {

            $.ajax({
                type: "POST",
                url: "../../ChannelWebService.asmx/GetChannelItem",
                data: "{channel_id:'" + channel_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    policy_company.setTemplate($("#jTemplateCompany").html());
                    policy_company.processTemplate(data);
                }

            });
        }

        //search customers
        function search_customer() {
            var btn = $('#btnSearchCustomer');
            if (btn.val() == 'Search') {//first search

                var full_name = $('#<%=txtSearchCustName.ClientID%>').val();
                    var customer_type_id = $('#<%=ddlSearchCustType.ClientID%>').val();
                    var id_card_search = $('#<%=txtSearchIDCard.ClientID%>').val();
                    var customer_number_search = $('#<%=txtSearchCustNo.ClientID%>').val();

                    if (full_name == '' && customer_type_id == '' && id_card_search == '' && customer_number_search == '') {
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        url: "../../CustomerWebService.asmx/GetCustomerList",
                        data: "{customer_name:'" + full_name + "', customer_type_id:'" + customer_type_id + "', id_card:'" + id_card_search + "', customer_id:'', customer_number:'" + customer_number_search + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d.length > 0) {
                                $('#dvCustomerSearchList').setTemplate($("#jTemplateCustomer").html());
                                $('#dvCustomerSearchList').processTemplate(data);
                            }
                            else {
                                $('#dvCustomerSearchList').html('');
                            }
                        }

                    });

                
                }
                else { 
                    btn.val('Search');
                    $('#modal_search_customer').modal('hide');
                }
            }

        
            //get some information of customer in search form
            function get_customer_info(customer_id, customer_name, customer_type_id, customer_birth_date, customer_gender, index, total_row) {
                if ($('#Ch' + index).is(':checked')) {
                    customer_search_id = customer_id;
                    customer_search_name = customer_name;
                    customer_search_type_id = customer_type_id;
                    customer_search_birth_date = formatJSONDate( customer_birth_date);
                    if(customer_gender=='0')
                    {
                        customer_search_gender = "Female";
                    }
                    else if(customer_gender=='1')
                    {
                        customer_search_gender = "Male";
                    }
                
                    // alert(customer_search_id);
                    $('#Ch' + index).attr('background-color', 'red');

                    $('#btnSearchCustomer').val('Ok');
                } else {
                    customer_search_id = "";
                    $('#btnSearchCustomer').val('Search');
                }
                //Uncheck all other checkboxes
                for (var i = 0; i < total_row  ; i++) {
                    if (i != index) {
                        $('#Ch' + i).prop('checked', false);
                    }

                }
            }

            //Search Sale Agent Name
            function show_agent_search()
            {
                $('#modal_search_agent').modal('show');
            }
            function search_agent()
            {
                var btn= $('#btnSearchAgent');
                if(btn.val()=='Ok')
                {
                    //code here
                    sale_agent_code.val(sale_agent_code_search);
                    sale_agent_name.val(sale_agent_name_search);
                    $('#modal_search_agent').modal('hide');//hide modal agent search
                    btn.val('Search');
                }
                else if(btn.val()=='Search')
                {
                    //code here
                    var sale = $('#<%=txtSearchSaleAgentName.ClientID%>');
                        if(sale.val().trim()!='')
                        {
                            $.ajax({
                                type: "POST",
                                url: "../../SaleAgentWebService.asmx/GetSaleAgents",
                                data:JSON.stringify({sale_agent_name: sale.val()}),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function(data){
                                    $('#dvAgentList').setTemplate($("#jTemplateSaleAgent").html());
                                    $('#dvAgentList').processTemplate(data);
                           
                                },
                                error:function(err)
                                {
                                    alert('Get sale agent list error: ' + err);
                                }
                    
                            });
                   
                        }
                    }
            }

            var sale_agent_code_search='';
            var sale_agent_name_search='';
            function get_sale_agent_info(agent_code, agent_name, row_index, total_rows)
            {
                var btn= $('#btnSearchAgent');
                if ($('#SaleAgent' + row_index).is(':checked')) {
               
                    sale_agent_code_search = agent_code;
                    sale_agent_name_search=agent_name;
                    btn.val('Ok');
                } else {
                    sale_agent_code_search ='';
                    sale_agent_name_search='';
                    btn.val('Search');
                }
                //Uncheck all other checkboxes
                for (var i = 0; i < total_rows  ; i++) {
                    if (i != row_index) {
                        $('#SaleAgent' + i).prop('checked', false);
                    }
                }
            
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
                $("#tblAppContent *").removeAttr('disabled');

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
                            $('#Main_lblApplicationNumberModalValidate').hide();

                            //Get Product according to selection of Product Type
                            product_type = $('#Main_ddlProductTypeModal').val();
                            $('#Main_hdfProductType').val(product_type)
                            GetInsurancePlan(product_type, "");

                            //hide and show Loan for
                            if (product_type == "2") {
                                // some fields needed change

                            } else {

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

            //Get Insurance Plan
            function GetInsurancePlan(product_type, product_id) {
                $.ajax({
                    type: "POST",
                    url: "../../ProductWebService.asmx/GetInsurancePlans",
                    data: "{product_type:" + JSON.stringify(product_type) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Main_ddlProduct').setTemplate($("#jTemplateProduct").html());
                        $('#Main_ddlProduct').processTemplate(data);

                        if (product_id != "") {

                            $('#Main_ddlProduct').val(product_id);
                            $('#Main_ddlProduct').val(product_id);
                        }
                    }
                });
            };
   
            //End New Applicaiton Section.....................................................................................................

    </script>

    <style>
        .modal.large {
            width: 60%; /* desired relative width */
            left: 20%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
        }
        .td10 {
            padding-left: 10px;
            padding-top: 0px;
            padding-bottom: 0px;
            padding-right: 10px;
            vertical-align: middle;
        }

        .tdpading10 {
            padding-top: 10px;
        }

        .txt100 {
            width: 96%;
        }
        .txt50 {
            width:50%;
        }
        .td40 {
            width: 10%;
        }

        .td60 {
            width: 80%;
        }
        .td25 {
            width:25%;
        }
        .ddl100 {
            width: 97.2%;
        }

        .required {
            color: red;
        }
        .select_rider, .select_ben {
            background-color:#94D1FF;
        }
        #application_status, .icon-search {
            cursor: pointer;
        }
    </style>
    <script id="jTemplateCompany" type="text/html">
        <option selected="selected" value='0'>--Select--</option>
        {#foreach $T.d as record}          
             <option value='{ $T.record.Channel_Item_ID }'>{ $T.record.Channel_Name }</option>
        {#/for}
           
    </script>
    <script id="jTemplateCustomer" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">No</th>
                    <th style="border-width: thin; border-style: solid;">Full Name</th>
                    <th style="border-width: thin; border-style: solid;">ID Card</th>
                    <th style="border-width: thin; border-style: solid;">Gender</th>
                    <th style="border-width: thin; border-style: solid;">Birth Date</th>
                    <th style="border-width: thin; border-style: solid; display: none;">Customer ID</th>
                    <th style="border-width: thin; border-style: solid; text-align: center; vertical-align: middle;">Select</th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td style="vertical-align: middle;">{ $T.row$index + 1 }</td>
                        <td style="vertical-align: middle;">{ $T.row.Last_Name + ' ' + $T.row.First_Name }</td>
                        <td style="vertical-align: middle;">{ $T.row.ID_Card}</td>
                        {#if $T.row.Gender==0}
                            <td style="vertical-align: middle;">Female</td>
                        {#elseif $T.row.Gender==1}
                            <td style="vertical-align: middle;">Male</td>
                        {#else} 
                            <td style="vertical-align: middle;">-</td>
                        {#/if}
                        <td style="vertical-align: middle;">{formatJSONDate($T.row.Birth_Date)}</td>
                        <td style="display: none;">{  $T.row.Customer_ID }</td>
                        <td style="text-align: center; vertical-align: middle;">
                            <input id="Ch{ $T.row$index }" type="checkbox" onclick="get_customer_info('{ $T.row.Customer_ID }','{ $T.row.Last_Name + ' ' + $T.row.First_Name }', '{ $T.row.Customer_Type }', '{ $T.row.Birth_Date }', '{ $T.row.Gender }', '{ $T.row$index }', '{ $T.row$total }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>
    <script id="jTemplateSaleAgent" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;text-align: center; vertical-align: middle;">No</th>
                    <th style="border-width: thin; border-style: solid;text-align: center; vertical-align: middle;" colspan="2">Name</th>
                    <th style="border-width: thin; border-style: solid;text-align: center; vertical-align: middle;">Agent Code</th>
                    <th style="border-width: thin; border-style: solid; text-align: center; vertical-align: middle;">Select</th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td style="text-align: center; vertical-align: middle;">{ $T.row$index + 1 }</td>
                        <td style="text-align: center; vertical-align: middle;">{ $T.row.Full_Name }</td>
                        <td style="text-align: center; vertical-align: middle;">{ $T.row.Khmer_Full_Name}</td>
                        <td style="text-align: center; vertical-align: middle;">{$T.row.Sale_Agent_ID}</td>
                        <td style="text-align: center; vertical-align: middle;">
                            <input id="SaleAgent{ $T.row$index }"+ type="checkbox" onclick="get_sale_agent_info('{ $T.row.Sale_Agent_ID }','{ $T.row.Full_Name }', '{ $T.row$index }', '{ $T.row$total }');" />

                        </td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>
    <script id="jTemplateProduct" type="text/html">
        <option value=''>---Select---</option>
        {#foreach $T.d as record}
          
                    <option value='{ $T.record.Product_ID }'>{ $T.record.En_Title }</option>
        {#/for}
           
    </script>
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
            <h1>Customer Information</h1>
            <div class="test3"></div>
           <table  id="tblContent" class="table-layout" style="background-color: #f6f6f6; padding: 40px;">
               <div id="customer_info">
                    <tr>
                        <td class="td10 tdpading10">
                            <asp:Label ID="lblCustNumber" Text="Customer Number" runat="server"></asp:Label></td>
                        <td class="td10 tdpading10">
                            <asp:TextBox runat="server" ID="txtCustNumber"></asp:TextBox>
                            <%--<asp:HiddenField runat="server" ID="hdfCustomerNumber" Value="" />--%>
                        </td>

                        <td class="td10 tdpading10">
                            <asp:Label ID="lblNationality" Text="Nationality" runat="server"></asp:Label></td>
                        <td class="td10 tdpading10">
                            <asp:DropDownList runat="server" ID="ddlNationality" DataSourceID="ds_Nationality" DataTextField="Nationality" DataValueField="Country_ID" AppendDataBoundItems="true">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <span class="required">*</span>
                        </td>
                    
                    </tr>
                
                    <tr>
                        <td class="td10">
                            <asp:Label ID="lblFirstName" Text="First Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtFirstName"></asp:TextBox>
                            <span class="required">*</span>
                        </td>
                        <td class="td10">
                            <asp:Label ID="lblLastName" Text="Last Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtLastName"></asp:TextBox>
                            <span class="required">*</span>
                        </td>
                    </tr>

                   <tr>
                        <td class="td10">
                            <asp:Label ID="lblKhFirstName" Text="Khmer First Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtKhFirstName" Height="20px"></asp:TextBox>
                            <span class="required">*</span>
                        </td>
                        <td class="td10">
                            <asp:Label ID="lblKhLastName" Text="Khmer Last Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtKhLastName" Height="20px"></asp:TextBox>
                            <span class="required">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td10">
                            <asp:Label ID="lblGender" Text="Gender" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:DropDownList runat="server" ID="ddlGender">
                                <asp:ListItem Text="Female" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                            <span class="required">*</span>
                        </td>

                        <td class="td10">
                            <asp:Label ID="lblBirthDate" Text="Birth Date" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox ID="txtBirthDate" runat="server" CssClass="datepicker" ReadOnly="true" placeholder="[DD\MM\YYYY]"></asp:TextBox>
                             <span class="icon-calendar"></span>
                             <span class="required">*</span>
                        </td>
                        <td class="td10"></td>
                        <td class="td10"></td>
                    </tr>
                    <tr>
                        <td class="td10">
                            <asp:Label ID="lblIDType" Text="ID Type" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:DropDownList runat="server" ID="ddlIDType">
                                <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                <asp:ListItem Value="1">Passport</asp:ListItem>
                                <asp:ListItem Value="2">Visa</asp:ListItem>
                                <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                            </asp:DropDownList>
                            <span class="required">*</span>
                        </td>

                        <td class="td10">
                            <asp:Label ID="lblIDCard" Text="ID Card" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtIDCard"></asp:TextBox>
                            <span class="required">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td10">
                            <asp:Label ID="lblMotherKhFirstName" Text="Mother Khmer First Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtMotherKhFirstName" Height="20px"></asp:TextBox></td>
                        <td class="td10">
                            <asp:Label ID="lblMotherKhLastName" Text="Mother Khmer Last Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtMotherKhLastName" Height="20px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="td10">
                            <asp:Label ID="lblMotherFirstName" Text="Mother First Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtMotherFirstName"></asp:TextBox></td>
                        <td class="td10">
                            <asp:Label ID="lblMotherLastName" Text="Mother Last Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtMotherLastName"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="td10">
                            <asp:Label ID="lblFatherKhFirstName" Text="Father Khmer First Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtFatherKhFirstName" Height="20px"></asp:TextBox></td>
                        <td class="td10">
                            <asp:Label ID="lblFatherKhLastName" Text="Father Khmer Last Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtFatherKhLastName" Height="20px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="td10">
                            <asp:Label ID="lblFatherFirstName" Text="Father First Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtFatherFirstName"></asp:TextBox></td>
                        <td class="td10">
                            <asp:Label ID="lblFatherLastName" Text="Father Last Name" runat="server"></asp:Label></td>
                        <td class="td10">
                            <asp:TextBox runat="server" ID="txtFatherLastName"></asp:TextBox></td>
                    </tr>
               </div>
                
               <%--Address Tabs--%>
                <tr>
                    <td colspan="4" class="td10">
                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                            <li class="active" id="tab_contact"><a href="#contact" data-toggle="tab"><b>Contact</b></a></li>
                            <li ><a href="#address" data-toggle="tab"><b>Address</b></a></li>
                            <li ><a href="#application" data-toggle="tab"><b>Application Detail</b></a></li>
                            <li ><a href="#policyinfo" data-toggle="tab"><b>Policy Detail</b></a></li>
                            <li ><a href="#rider" data-toggle="tab"><b>Riders</b></a></li>
                            <li ><a href="#beneficiary" data-toggle="tab"><b>Beneficiary</b></a></li>
                        </ul>
                        <div class="container">
                            <div class="tab-content">
                                <div id="my-tab-content" class="tab-content">
                                    <%-- Tab contact --%>
                                    <div class="tab-pane active" id="contact">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTel" runat="server" Text="Tel"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTel" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMobile" runat="server" Text="Mobile"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFax" Text="Fax" runat="server"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
                                                    
                                                </td>

                                            </tr>
                                            <tr>
                                                
                                                <td>
                                                    <asp:Label ID="lblEmail" Text="Email" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                                   
                                                </td>
                                            </tr>
                                        
                                        </table>
                                        
                                    </div>
                                    <%-- end Tab contact --%>

                                    <%-- Tab Address --%>
                                    <div class="tab-pane" id="address">
                                        <table style= " width:100%; background-color: #f6f6f6;">
                                            <tr>
                                               <td style="width:10%;">
                                                    <asp:Label ID="lblAddress1" runat="server">Address <b>#1</b></asp:Label>
                                                </td>
                                                 <td style="width:40%;">
                                                    <asp:TextBox ID="txtAddress1" runat="server" Width="96%" Height="22px"></asp:TextBox>
                                                </td>

                                                <td style="width:10%;">
                                                    <asp:Label ID="lblAddress2" runat="server" >Address <b>#2</b></asp:Label>
                                                </td>
                                                 <td style="width:40%;">
                                                    <asp:TextBox ID="txtAddress2" runat="server" Width="96%" Height="22px"></asp:TextBox>
                                                </td>
                                                
                                            </tr>
                                           <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblCountry" Text="Country" runat="server"></asp:Label></td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCountry" Width="98%" Height="25px" DataSourceID="ds_Country" DataTextField="Country_Name" DataValueField="Country_ID" onchange="get_zip_code(this.value);" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>

                                               <td style="width:10%;">
                                                    <asp:Label ID="lblProvince" Text="Province/City" runat="server"></asp:Label></td>
                                                <td style="width:40%;">
                                                    <asp:DropDownList runat="server" ID="ddlProvince" Width="98%" Height="25px" DataSourceID="ds_province" DataTextField="PRO_NAME" DataValueField="PRO_ID" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <tr>
                                                    <td style="text-align: left">Zip Code:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtZipCodeLife2" Width="96%" runat="server" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </tr>
                                            
                                        </table>

                                    </div>
                                    <%-- end Tab Address --%>

                                    <%--Tab Application Detail--%>
                                    <div class="tab-pane" id="application">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAppNumber" runat="server" Text="Application Number"></asp:Label>
                                                </td>

                                                <td>
                                                    <asp:TextBox ID="txtApplicationNumber" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                    <span id="application_status" class="required" data-toggle="modal" data-target="#myModalNewApplicationForm"></span>
                                                    <%--<span class="required">*</span>--%>
                                                </td>
                                               
                                                <td>
                                                    <asp:Label ID="lblDateOfSignature" Text="Date of Signature"  runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDateOfSignature" CssClass="datepicker" ReadOnly="true" placeholder="[DD\MM\YYYY]" runat="server"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDateSignature" runat="server" />
                                                    <span class="required">*</span>
                                                </td>

                                                 <td>
                                                    <asp:Label ID="lblDataEntryBy" runat="server" Text="Data Entry By"></asp:Label>
                                                </td>

                                                <td>
                                                    <asp:TextBox ID="txtDataEntryBy" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDataEntryBy" runat="server" />
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSaleAgentName" Text="Sale Agent Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSaleAgentName" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                    <span class="icon-search" onclick="show_agent_search();"></span>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSaleAgentCode" Text="Sale Agent Code" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSaleAgentCode" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblPaymentCode" runat="server" Text="Payment Code"></asp:Label>
                                                </td>

                                                <td>
                                                    <asp:TextBox ID="txtPaymentCode" runat="server"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="text-align: right">Created Note:</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtNote" TextMode="MultiLine" Height="40" runat="server" MaxLength="255"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfNote" runat="server" />
                                                </td>

                                            </tr>

                                        </table>
                                        
                                    </div>
                                    <%--End Tab Application Detail--%>

                                    <%-- Tab Policy Info --%>
                                    <div class="tab-pane" id="policyinfo">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number"></asp:Label>
                                                </td>

                                                <td>
                                                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfPolicyID" runat="server" Value="" />
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblChannel" Text="Channel" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlChannel" DataSourceID="SqlDataSourceChannel" DataTextField="Type" DataValueField="Channel_ID" AppendDataBoundItems="true" runat="server" onchange="GetCompany(this.value);">
                                                        <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCompany" Text="Company" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompany" runat="server">
                                                        <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblProduct" Text="Product" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlProduct" runat="server"></asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPayMode" runat="server" Text="Pay Mode"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPayMode" runat="server" DataSourceID="ds_paymode" DataTextField="mode" DataValueField="Pay_Mode_ID" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPolicyType" runat="server" Text="Policy Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPolicyType" runat="server" DataSourceID="ds_policy_type" DataTextField="Type" DataValueField="Policy_Type_ID" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPolicySI" runat="server" Text="Sum Insured"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicySI" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox> <%--ReadOnly="true" Enabled="false"--%>
                                                    USD
                                            <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCoverYear" runat="server" Text="Cover Year"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCoverYear" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPayYear" runat="server" Text="Pay Year"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPayYear" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPolicyEffectiveDate" runat="server" Text="Effective Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyEffectiveDate" runat="server" CssClass="datepicker" ReadOnly="true"></asp:TextBox>
                                                    <span class="icon-calendar"></span>
                                                    <span class="required">*</span>
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblPolicyMaturityDate" runat="server" Text="Maturity Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyMaturityDate" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                    <span class="icon-calendar"></span>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPolicyIssuedDate" runat="server" Text="Issued Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyIssuedDate" runat="server" CssClass="datepicker" ReadOnly="true"></asp:TextBox>
                                                    <span class="icon-calendar"></span>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPolicyAgreementDate" Text="Agreement Date" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyAgreementDate" CssClass="datepicker" ReadOnly="true" runat="server" />
                                                    <span class="icon-calendar"></span>
                                                    <span class="required">*</span>
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblPolicyPremium" runat="server" Text="Premium (User)"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyPremium" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblPolicyStatusTypeID" runat="server" Text="Policy Status"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPolicyStatusTypeID" runat="server">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Inforce" Value="IF"></asp:ListItem>
                                                        <asp:ListItem Text="Lapse" Value="LAP"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPolicyDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyDiscountAmount" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    USD
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPolicySystemPrem" runat="server" Text="Premium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicySystemPrem" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                </td>
                                                
                                                <td>
                                                    <asp:Label ID="lblPolicyExtraAmount" runat="server" Text="Extra Amount"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyExtraAmount" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    USD
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPolicyOriginalAmount" runat="server" Text="Annual Premium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyOriginalAmount" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    USD
                                                </td>
                                                
                                                <td>
                                                    <asp:Label ID="lblPolicyAgeIssure" runat="server" Text="Age"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyAgeIssure" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                </td>
                                                
                                                <td>
                                                    <asp:Label ID="lblPolicyNote" runat="server" Text="Created Note"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPolicyCreatedNote" TextMode="MultiLine" Height="40" runat="server" MaxLength="255"></asp:TextBox>
                                                </td>

                                            </tr>

                                        </table>
                                        
                                    </div>
                                    <%-- end Tab Policy Info --%>

                                    <%-- Tab Rider --%>
                                    <div class="tab-pane" id="rider">
                                        <table>
                                            <tr>
                                                <td class="td10">
                                                    <asp:Label ID="lblRiderType" Text="Rider Type" runat="server"></asp:Label>
                                                </td>
                                                <td class="td10">
                                                    <asp:DropDownList ID="ddlRiderType" runat="server">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="AD" Value="13"></asp:ListItem>
                                                        <asp:ListItem Text="TPD" Value="12"></asp:ListItem>
                                                        <asp:ListItem Text="Spouse" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Kid 1" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="Kid 2" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="Kid 3" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="Kid 4" Value="6"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="td10 tdpading10">
                                                    <asp:Label ID="lblRiderFirstName" Text="First Name (EN)" runat="server"></asp:Label></td>
                                                <td class="td10 tdpading10">
                                                    <asp:TextBox runat="server" ID="txtRiderFirstName"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>

                                                <td class="td10 tdpading10">
                                                    <asp:Label ID="lblRiderLastName" Text="Last Name (EN)" runat="server"></asp:Label></td>
                                                <td class="td10 tdpading10">
                                                    <asp:TextBox runat="server" ID="txtRiderLastName"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>

                                                <td class="td10 tdpading10">
                                                    <asp:Label ID="lblRiderSI" runat="server" Text="Sum Insured"></asp:Label>
                                                </td>
                                                <td class="td10 tdpading10">
                                                    <asp:TextBox ID="txtRiderSI" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    USD 
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>

                                           <tr>
                                                <td class="td10">
                                                    <asp:Label ID="lblRiderkhFirstName" Text="Khmer First Name" runat="server"></asp:Label></td>
                                                <td class="td10">
                                                    <asp:TextBox runat="server" ID="txtRiderkhFirstName"></asp:TextBox>
                                                </td>

                                                <td class="td10">
                                                    <asp:Label ID="lblRiderkhLastName" Text="Khmer Last Name" runat="server"></asp:Label></td>
                                                <td class="td10">
                                                    <asp:TextBox runat="server" ID="txtRiderkhLastName" ></asp:TextBox>
                                                </td>

                                               <td class="td10">
                                                    <asp:Label ID="lblRiderPrem" runat="server" Text="Premium"></asp:Label>
                                                </td>
                                                <td class="td10">
                                                    <asp:TextBox ID="txtRiderPrem" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    USD
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>


                                            <tr>
                                               <td class="td10">
                                                    <asp:Label ID="lblRiderGender" Text="Gender" runat="server"></asp:Label>
                                               </td>
                                               <td class="td10">
                                                    <asp:DropDownList ID="txtRiderGender" runat="server">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Female" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                               </td>
                                               <td class="td10">
                                                    <asp:Label ID="lblRiderDOB" Text="Birth Date" runat="server"></asp:Label></td>
                                                <td class="td10">
                                                    <asp:TextBox runat="server" ID="txtRiderDOB" CssClass="datepicker" ReadOnly="true"></asp:TextBox>
                                                    <span class="icon-calendar"></span>
                                                </td>
                                                
                                                <td class="td10">
                                                    <asp:Label ID="lblRiderOP" runat="server" Text="Original Amount"></asp:Label>
                                                </td>
                                                <td class="td10"> 
                                                    <asp:TextBox ID="txtRiderOP" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    USD
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td10">
                                                    <asp:Label ID="lblRiderNationality" Text="Nationality" runat="server"></asp:Label></td>
                                                <td class="td10">
                                                    <asp:DropDownList runat="server" ID="txtRiderNationality" DataSourceID="ds_Nationality" DataTextField="Nationality" DataValueField="Country_ID" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                                <td class="td10">
                                                    <asp:Label ID="lblRiderRelationship" Text="Relationship" runat="server"></asp:Label></td>
                                                <td class="td10">
                                                    <asp:DropDownList ID="txtRiderRelationship" runat="server" DataSourceID="ds_relationship" DataTextField="relationship" DataValueField="relationship" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>

                                                <td class="td10">
                                                    <asp:Label ID="lblRiderEMAmount" runat="server" Text="EM Amount"></asp:Label>
                                                </td>
                                                <td class="td10">
                                                    <asp:TextBox ID="txtRiderEMAmount" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>

                                                <td rowspan="5" style="width: 2%; vertical-align: bottom; text-align: center; padding-right: 5px;">
                                                    <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="btnClearRider" class="btn btn-small btn-primary" onclick="clear_rider_boxes()" >Clear</button><br />
                                                    <br />
                                                    <button type="button" style="margin-left: 5px; margin-bottom: 5px;" id="btnAddRider" class="btn btn-small btn-primary" onclick="add_rider();"><span class="icon-plus"></span></button>
                                                </td>
                                            </tr>
                                           <tr>
                                               <td class="td10">
                                                    <asp:Label ID="lblRiderIDType" Text="ID Type" runat="server"></asp:Label>
                                               </td>
                                               <td class="td10">
                                                    <asp:DropDownList ID="txtRiderIDType" runat="server">
                                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                                        <asp:ListItem Value="2">Visa</asp:ListItem>
                                                        <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                               <td class="td10">
                                                    <asp:Label ID="lblRiderIDNumber" Text="ID Number" runat="server"></asp:Label></td>
                                                <td class="td10">
                                                    <asp:TextBox runat="server" ID="txtRiderIDNumber"></asp:TextBox>
                                                </td>

                                               <td class="td10">
                                                    <asp:Label ID="lblRiderDisAmount" runat="server" Text="Discount Amount"></asp:Label>
                                                </td>
                                                <td class="td10">
                                                    <asp:TextBox ID="txtRiderDisAmount" runat="server" onkeyup="ValidateNumber(this)"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>
                                               
                                            </tr>

                                        </table>
                                        <div id="div_rider_list">
                                            <table id="tbl_rider_list" class="table table-bordered">
                                                <tr style="background-color: lightgray;">
                                                    <td style="width: 10px; vertical-align: middle; display: none;">No.</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display: none;">Rider ID</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align: left;">Full Name</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display: none;">English Name</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display: none;">Kh First Name</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display: none;">Kh Last Name</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display: none;">En First Name</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; display: none;">En Last Name</td>
                                                    <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align: center;">Gender</td>
                                                    <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align: center;">Birth Date</td>
                                                    <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align: center;">Nationality</td>
                                                    <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align: center;">Relationship</td>
                                                    <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align: center;">ID Type</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align: center;">ID Number</td>
                                                    <td style="padding: 5px; height: 30px; width: 50px; vertical-align: middle; text-align: center;">Rider</td>
                                                    <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align: left;">Sum Insured</td>
                                                    <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; text-align: center;">Premium</td>
                                                    <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle;">Original Amount</td>
                                                    <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; text-align: center;">EM Amount</td>
                                                    <td style="padding: 5px; height: 30px; width: 70px; vertical-align: middle; text-align: center;">Discount Amount</td>
                                                    <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle;">Action</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <%-- end Tab Rider --%>

                                    <%-- Tab Beneficiary --%>
                                    <div id="beneficiary" class="tab-pane" style="padding-left: 10px; padding-right: 10px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblBenFullName" runat="server" Text="Full Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBenFullName" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBenRelationship" Text="Relationship" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBenRelationship" runat="server" DataSourceID="ds_relationship" DataTextField="relationship" DataValueField="relationship" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblBenIDType" Text="ID Type" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBenIDType" runat="server">
                                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="0">I.D Card</asp:ListItem>
                                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                                        <asp:ListItem Value="2">Visa</asp:ListItem>
                                                        <asp:ListItem Value="3">Birth certificate</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBenIDCard" Text="ID Card" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBenIDCard" runat="server" ></asp:TextBox>
                                                    <span class="required">*</span>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblBenPercentage" Text="Share(%)" runat="server"></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtBenPercentage" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBenRemarks" Text="Remarks" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBenRemarks" runat="server"></asp:TextBox>
                                                </td>

                                                <td rowspan="5" style="width: 2%; vertical-align: bottom; text-align: center; padding-right: 5px;">
                                                    <button type="button" style="margin-left: 5px; margin-bottom: 5px;" class="btn btn-small btn-primary" onclick="clear_ben_boxes()">Clear</button><br />
                                                    <br />
                                                    <button type="button" style="margin-left: 5px; margin-bottom: 5px;" class="btn btn-small btn-primary" onclick="add_beneficiary();"><span class="icon-plus"></span></button>
                                                </td>
                                            </tr>
                                        
                                        </table>
                                        <div id="beneficiary_list">
                                            <table id="tbl_ben_list" class="table table-bordered">
                                                <tr style="background-color: lightgray;">
                                                    <td style="width: 50px; vertical-align: middle; text-align: center;">No.</td>
                                                    <td style="width: 50px; vertical-align: middle; display: none;">Beneficiary ID</td>
                                                    <td style="padding: 5px; height: 30px; width: 200px; vertical-align: middle; text-align: center;">Full Name</td>
                                                    <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align: center;">ID Type</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align: center;">ID Card</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align: center;">Relationship</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align: center;">Share(%)</td>
                                                    <td style="padding: 5px; height: 30px; width: 100px; vertical-align: middle; text-align: center;">Remarks</td>
                                                    <td style="padding: 5px; height: 30px; width: 60px; vertical-align: middle; text-align: center;">Action</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <%-- end Tab Beneficiary --%>
                                </div>
                            </div>

                        </div>
                    </td>
                </tr>
               <%-- End Address Tabs--%>

            </table>
            <%--Modal Message Alert--%>
            <div id="modal_message" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="Message" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H1">System Alert!</h3>
                </div>
                <div class="modal-body">
                    <div id="message" runat="server" style="padding: 10px; text-align: center; color: red;">
                        <asp:Label runat="server" Text="" ID="lblMessage"></asp:Label>
                    </div>

                </div>

            </div>
            
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
                                    <asp:DropDownList ID="ddlProductTypeModal" runat="server" DataSourceID="ds_ProductType" DataTextField="myproducttype" DataValueField="product_type_id"></asp:DropDownList>
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
            <%--Modal Search Customer--%>
            <div id="modal_search_customer" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="SearchCustomer" aria-hidden="true" data-keyboard="false" data-backdrop="static">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Customer</h3>
                </div>
                <div class="modal-body ">
                    <ul id="Ul1" class="nav nav-tabs" data-tabs="tabs">
                        <li class="active"><a href="#search_customer_by_customer_number" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Customer Number</a></li>
                        <li><a href="#search_customer_by_customer_name" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Customer Info.</a></li>
                    </ul>
                    <div id="search_customer_tabcontent" class="tab-content" style="height: 70px; overflow: hidden;">

                        <div id="search_customer_by_customer_number" class="tab-pane active">
                            <table style="width: 70%;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustNo" Text="Customer Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchCustNo"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="search_customer_by_customer_name" class="tab-pane">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustType" Text="Customer Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSearchCustType" DataSourceID="ds_customer_type" DataTextField="customer_type" DataValueField="customer_type_id" AppendDataBoundItems="true" >
                                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustName" Text="Customer Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchCustName"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchIDCard" Text="ID Card"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchIDCard"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div id ="dvCustomerSearchList"></div>
                </div>
                <div class="modal-footer" >
                    <input type="button" id="btnSearchCustomer" value="Search" onclick="search_customer();" class="btn btn-primary" />
                </div>
            </div>

            <%--Modal Search Agent--%>
            <div id="modal_search_agent" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="SearchAgent" aria-hidden="true" data-keyboard="false" data-backdrop="static">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H3">Search Agent</h3>
                </div>
                <div class="modal-body ">
           
                        <div id="Div4" class="tab-pane active">
                            <table style="width: 70%;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchSaleAgentName" Text="Agent Name/Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchSaleAgentName"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
               
                    <div id="dvAgentList"></div>
                </div>
                <div class="modal-footer">
                    <input type="button" id="btnSearchAgent" value="Search" onclick="search_agent();" class="btn btn-primary" />
                </div>
            </div>
            <%--End Modal Search Agent--%>
        </ContentTemplate>
       
    </asp:UpdatePanel>
    <%-- Section Hidenfields Initialize  --%>
    <asp:HiddenField ID="hdlCustID" runat="server" Value="" />
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
    <asp:HiddenField ID="hdfAppRegisterID" runat="server" />
    <asp:HiddenField ID="hdfProductType" runat="server" />
    <asp:HiddenField ID="hdfOriginalPremiumAmountSystem" runat="server" />
    <%-- End Section Hidenfields Initialize  --%>

    <asp:SqlDataSource ID="ds_Country" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Country_Name FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_Nationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_Customer_Type" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Customer_Type_ID, Customer_Type  FROM Ct_Customer_Type ORDER BY Customer_Type"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_status" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT STATUS_ID, STATUS FROM TBL_STATUS"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_province" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT PRO_ID, PRO_NAME FROM TBL_PROVINCE ORDER BY PRO_NAME;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceChannel" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT * FROM Ct_Channel where Status = 1 order by Created_On ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_paymode" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Pay_Mode_ID, Mode FROM Ct_Payment_Mode ORDER BY MODE;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_policy_type" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Policy_Type_ID, Type FROM Ct_Policy_Type ORDER BY Type;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_product" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_ID, En_Title FROM Ct_Product WHERE En_Title <>'' ORDER BY En_Title;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_relationship" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select relationship from ct_relationship  order by Relationship;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_ProductType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Product_Type_ID, CONVERT(varchar,Product_Type_ID + replicate(' ', 50 - len(Product_Type_ID))) + '. ' + Product_Type AS myproducttype FROM dbo.Ct_Product_Type where Product_Type_ID != 4 ORDER BY Product_Type_ID"></asp:SqlDataSource>
    
</asp:Content>
