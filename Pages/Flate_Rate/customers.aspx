<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="customers.aspx.cs" Inherits="Pages_Business_flate_rate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        <li>
            <input type="button" onclick="add_new();" style="background: url('../../App_Themes/functions/add.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
       <li>
           <input type="button" onclick="btn_clear();" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;"/>
       </li>
         <li style="display:none;">
            <input type="button" data-toggle="modal" data-target="#modal_search_customer" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li style="display:none;">
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

    <script>
        //public variables declaration
        var cust_type;
        var reg_name;
        var reg_number;
        var reg_date;
        var tin;

        var cust_id;
        var customer_status;
        var marital_status;
        var customer_number;
        var first_name_kh;
        var last_name_kh;
        var first_name_en;
        var last_name_en;
        var gender;
        var id_type;
        var id_card;
        var nationality;
        var birth_date;

        var mother_first_name;
        var mother_last_name;
        var mother_first_name_kh;
        var mother_last_name_kh;
        var father_first_name;
        var father_last_name;
        var father_first_name_kh;
        var father_last_name_kh;

        var address;
        var country;
        var zip_code;
        var province;
        var khan;
        var sangkat;

        //contact
        var tel;
        var mobile;
        var fax;
        var mail;
        var contact_name;
        var contact_address;
        var contact_remark;
        var responsibilty;

        var factory;
        var occupation;
        var remarks;

        var customer_search_id;

        var exist_customer_number;
      
        //end public variables declaration

        //get parameters
        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

        $(document).ready(function () {
           
            reg_name = $('#Main_txtRegName');
            reg_number = $('#Main_txtRegNumber');
            reg_date = $('#Main_txtRegDate');
            tin = $('#Main_txtTinNumber');

            cust_id = $('#<%=hdlCustID.ClientID%>');
            cust_type = $('#Main_ddlCustomerType');
            customer_number = $('#<%=txtCustNumber.ClientID%>');
            first_name_en = $('#Main_txtFirstName');
            last_name_en = $('#Main_txtLastName');
            first_name_kh = $('#<%=txtKhFirstName.ClientID%>');
            last_name_kh = $('#<%=txtKhLastName.ClientID%>');
            gender = $('#<%=ddlGender.ClientID%>');
            customer_status = $('#<%=ddlStatus.ClientID%>');
            marital_status = $('#<%=ddlMaritalStatus.ClientID%>');
            id_type = $('#<%=ddlIDType.ClientID%>');
            id_card = $('#<%=txtIDCard.ClientID%>');
            nationality = $('#<%=ddlNationality.ClientID%>');
            birth_date = $('#<%=txtBirthDate.ClientID%>');

            mother_first_name = $('#<%=txtMotherFirstName.ClientID%>');
            mother_last_name = $('#<%=txtMotherLastName.ClientID%>');
            mother_first_name_kh = $('#<%=txtMotherKhFirstName.ClientID%>');
            mother_last_name_kh = $('#<%=txtMotherKhLastName.ClientID%>');
            father_first_name = $('#<%=txtFatherFirstName.ClientID%>');
            father_last_name = $('#<%=txtFatherLastName.ClientID%>');
            father_first_name_kh = $('#<%=txtFatherKhFirstName.ClientID%>');
            father_last_name_kh = $('#<%=txtFatherKhLastName.ClientID%>');

            address = $('#<%=txtAddress.ClientID%>');
            country = $('#<%=ddlCountry.ClientID%>');
            zip_code = $('#<%=txtZipCode.ClientID%>');
            province = $('#<%=ddlProvince.ClientID%>');
            khan = $('#<%=ddlKhan.ClientID%>');
            sangkat = $('#<%=ddlSangkat.ClientID%>');

            mobile = $('#<%=txtMobile.ClientID%>');
            tel = $('#<%=txtTel.ClientID%>');
            fax = $('#<%=txtFax.ClientID%>');
            mail = $('#<%=txtEmail.ClientID%>');
            contact_name = $('#<%=txtContactName.ClientID%>');
            contact_address = $('#<%=txtContactAddress.ClientID%>');
            contact_remark = $('#<%=txtContactRemarks.ClientID%>');
            responsibility = $('#<%=txtResponsibility.ClientID%>');

            remarks = $('#<%=txtRemarks.ClientID%>');
            factory = $('#<%=ddlFactory.ClientID%>');
            occupation = $('#<%=txtOccupation.ClientID%>');

            exist_customer_number = $('#<%=hdfCustomerNumber.ClientID%>');

            $('.datepicker').datepicker();

           // enabled_company_registration(false);

            //disable all controls
            disabled_controls(true);
            
            customer_search_id = '';

            load_exist_customer();

        });

        //load exist customer information by query string ['cust_id']
        function load_exist_customer()
        {
            var cust_id_param = getParameterByName('cust_id');
            if (cust_id_param != null && cust_id_param != '') {
                customer_search_id = cust_id_param;
                load_customer(cust_id_param);
            }
            else {
                add_new();
                
            }
        }

        function paste_name_to_contact_name()
        {
            contact_name.val(last_name_en.val() + ' ' + first_name_en.val());
        }
        function get_zip_code(country_id) {

            if (country_id != 'KH') {
                province.attr('disabled', true);
                khan.attr('disabled', true);
                sangkat.attr('disabled', true);
                province.prop('selectedIndex', 0);
                khan.prop('selectedIndex', 0);
                sangkat.prop('selectedIndex', 0);

            }
            else {
                province.attr('disabled', false);
                khan.attr('disabled', false);
                sangkat.attr('disabled', false);
            }
            $.ajax({
                type: "POST",
                url: "../../AppWebService.asmx/GetZipCode",
                data: "{country_value:'" + country_id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_txtZipCode').val(data.d);
                }

            });

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
        function company() {

            //var cust_type = $('#Main_ddlCustomerType');
            //var reg_number = $('#Main_txtRegNumber');
            //var reg_date = $('#Main_txtRegDate');
            //var tin = $('#Main_txtTinNumber');

            if (cust_type.find('option:selected').text() != 'Company') {

                //reg_number.prop('disabled', true);
                //reg_date.prop('disabled', true);
                //tin.prop('disabled', true);
                enabled_company_registration(false);
            }
            else {
                //reg_number.val('');
                //reg_date.val('');
                //tin.val('');
                ////reg_number.prop('disabled', false);
                //reg_number.removeAttr('disabled');
                //reg_date.removeAttr('disabled');
                //tin.removeAttr('disabled');
                enabled_company_registration(true);
            }
        }
        function enabled_company_registration(bool) {
            //var cust_type = $('#Main_ddlCustomerType');
            //var reg_number = $('#Main_txtRegNumber');
            //var reg_date = $('#Main_txtRegDate');
            //var tin = $('#Main_txtTinNumber');

            var prop_disabled = 'disabled';

            if (!bool) { //disabled
                reg_name.prop('disabled', true);
                reg_number.prop('disabled', true);
                reg_date.prop('disabled', true);
                tin.prop('disabled', true);

                marital_status.prop('disabled', false);

                first_name_en.prop(prop_disabled, false);
                last_name_en.prop(prop_disabled, false);
                first_name_kh.prop(prop_disabled, false);
                last_name_kh.prop(prop_disabled, false);
                gender.prop(prop_disabled, false);
                id_type.prop(prop_disabled, false);
                id_card.prop(prop_disabled, false);
                nationality.prop(prop_disabled, false);
                birth_date.prop(prop_disabled, false);

                mother_first_name.prop('disabled', false);
                mother_last_name.prop('disabled', false);
                mother_first_name_kh.prop('disabled', false);
                mother_last_name_kh.prop('disabled', false);
                father_first_name.prop('disabled', false);
                father_last_name.prop(prop_disabled, false);
                father_first_name_kh.prop(prop_disabled, false);
                father_last_name_kh.prop(prop_disabled, false);

                reg_name.val('');
                reg_number.val('');
                reg_date.val('');
                tin.val('');

            } else {

                //reg_number.prop('disabled', false);
                reg_name.removeAttr('disabled');
                reg_number.removeAttr('disabled');
                reg_date.removeAttr('disabled');
                tin.removeAttr('disabled');

                //
                marital_status.prop('disabled', true);

                first_name_en.prop(prop_disabled, false);
                last_name_en.prop(prop_disabled, true);
                first_name_kh.prop(prop_disabled, false);
                last_name_kh.prop(prop_disabled, true);

                gender.prop(prop_disabled, true);
                id_type.prop(prop_disabled, true);
                id_card.prop(prop_disabled, true);
                nationality.prop(prop_disabled, true);
                birth_date.prop(prop_disabled, true);

                mother_first_name.prop(prop_disabled, true);
                mother_last_name.prop(prop_disabled, true);
                mother_first_name_kh.prop(prop_disabled, true);
                mother_last_name_kh.prop(prop_disabled, true);
                father_first_name.prop(prop_disabled, true);
                father_last_name.prop(prop_disabled, true);
                father_first_name_kh.prop(prop_disabled, true);
                father_last_name_kh.prop(prop_disabled, true);

                mother_first_name.val('');
                mother_first_name_kh.val('');
                mother_last_name.val('');
                mother_last_name_kh.val('');
                father_first_name.val('');
                father_first_name_kh.val('');
                father_last_name.val('');
                father_last_name_kh.val('');

            }
        }

        function save_customer(option) {
            ////validate form
            //if (validate_form() == false) {
            //    return;
            //}
            var customer_info = '';
            if (option == 'save')
            {
                //save
                //alert(cust_type.val() + '  ' + last_name_en.val() + '  ' + first_name_en.val() + '  ' + last_name_kh.val() + '  ' + first_name_kh.val() + '  ' + gender.val());
                if (confirm('Do you save this customer?')) {
                    //generate customer number
                    $.ajax({
                        type: "POST",
                        url: "../../CustomerWebService.asmx/GenerateNewCustomerNumber",
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != '') {
                                customer_number.val(data.d);
                                if (cust_type.find('option:selected').text() == 'Company') {
                                    customer_info = cust_type.val() + ';' + customer_number.val() + ';;' + first_name_en.val() + ';;' + first_name_kh.val() + ';;;;;' + customer_status.val() + ';;;;;;;;;;' + reg_name.val() + ';' + reg_number.val() + ';' + reg_date.val() + ';' + tin.val() + ';' +
                                 address.val() + ';' + country.val() + ';' + zip_code.val() + ';' + province.val() + ';' +
                                  khan.val() + ';' + sangkat.val() + ';' + factory.val() + ';' + occupation.val() + ';' + remarks.val() + ';;';
                                }
                                else {
                                    customer_info = cust_type.val() + ';' + customer_number.val() + ';' + last_name_en.val() + ';' + first_name_en.val() + ';' + last_name_kh.val() + ';' + first_name_kh.val() + ';' +
                                   gender.val() + ';' + id_type.val() + ';' + id_card.val() + ';' + nationality.val() + ';' + customer_status.val() + ';' + marital_status.val() + ';' +
                                   mother_first_name.val() + ';' + mother_last_name.val() + ';' + mother_first_name_kh.val() + ';' + mother_last_name_kh.val() + ';' +
                                   father_first_name.val() + ';' + father_last_name.val() + ';' + father_first_name_kh.val() + ';' + father_last_name_kh.val() + ';' +
                                   reg_name.val() + ';' + reg_number.val() + ';' + reg_date.val() + ';' + tin.val() + ';' +
                                   address.val() + ';' + country.val() + ';' + zip_code.val() + ';' + province.val() + ';' +
                                   khan.val() + ';' + sangkat.val() + ';' + factory.val() + ';' + occupation.val() + ';' + remarks.val() + ';' + birth_date.val();
                                }

                                $.ajax({
                                    type: "POST",
                                    url: "../../CustomerWebService.asmx/SaveCustomer",
                                    data: "{customer_info:'" + customer_info + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (data) {
                                        if (data.d != '') {
                                            cust_id.val(data.d);
                                            save_contact();//save contact list
                                            alert('Saved successfully.');
                                            clear_form();
                                        } else {
                                            alert('Saved fial');
                                        }
                                    }
                                });
                            }
                            else {
                                alert('System cannot generate new customer number, please contact your system administrator.');
                                return;
                            }
                        }
                    });

                }
            }
            else if (option == 'update')
            {
               
                if (confirm('Do you update this customer?')) {

                    if (cust_type.find('option:selected').text() == 'Company') {
                        customer_info = cust_type.val() + ';' + customer_number.val() + ';;' + first_name_en.val() + ';;' + first_name_kh.val() + ';;;;;' + customer_status.val() + ';;;;;;;;;;' + reg_name.val() + ';' + reg_number.val() + ';' + reg_date.val() + ';' + tin.val() + ';' +
                       address.val() + ';' + country.val() + ';' + zip_code.val() + ';' + province.val() + ';' +
                       khan.val() + ';' + sangkat.val() + ';' + factory.val() + ';' + occupation.val() + ';'  + remarks.val() + ';;' + customer_search_id;

                    }
                    else
                    {
                        customer_info = cust_type.val() + ';' + customer_number.val() + ';' + last_name_en.val() + ';' + first_name_en.val() + ';' + last_name_kh.val() + ';' + first_name_kh.val() + ';' +
                       gender.val() + ';' + id_type.val() + ';' + id_card.val() + ';' + nationality.val() + ';' + customer_status.val() + ';' + marital_status.val() + ';' +
                       mother_first_name.val() + ';' + mother_last_name.val() + ';' + mother_first_name_kh.val() + ';' + mother_last_name_kh.val() + ';' +
                       father_first_name.val() + ';' + father_last_name.val() + ';' + father_first_name_kh.val() + ';' + father_last_name_kh.val() + ';' +
                       reg_name.val() + ';' + reg_number.val() + ';' + reg_date.val() + ';' + tin.val() + ';' +
                       address.val() + ';' + country.val() + ';' + zip_code.val() + ';' + province.val() + ';' +
                        khan.val() + ';' + sangkat.val() + ';' + factory.val() + ';'  + occupation.val() + ';' + remarks.val() + ';' + birth_date.val() + ';' + customer_search_id;

                    }
                   
                    $.ajax({
                        type: "POST",
                        url: "../../CustomerWebService.asmx/UpdateCustomer",
                        data: "{customer_info:'" + customer_info + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d) {
                                alert('Updated successfully.');
                                save_contact();//save contact list
                                clear_form();
                            } else {
                                alert('Updated fial');
                            }
                        }
                    });
                }
            }
            
        }

        function check_existing_customer() {
            //check existing customer number
            if (validate_form() == false) {
                return false;
            }
            if (customer_search_id != '') {
                //code update old record
                //update
                    if (customer_number.val() != exist_customer_number.val()) {
                        $.ajax({
                            type: "POST",
                            url: "../../CustomerWebService.asmx/GetCustomerList",
                            data: "{customer_name:'', customer_type_id:'', id_card:'', customer_id:'', customer_number:'" + customer_number.val() + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.d.length > 0) {
                                    var record = data.d[0];
                                    alert('Customer Number [' + record.Customer_Number + '] is already exist. Updated fail.');
                                    return false;
                                }
                                else
                                {
                                    save_customer('update');
                                }
                            }
                        });
                    }
                    else
                    {
                        save_customer('update');
                    }
            }
            else
            {
                //code save new record
                /*
                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/GetCustomerList",
                    data: "{customer_name:'', customer_type_id:'', id_card:'', customer_id:'', customer_number:'" + customer_number.val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.length > 0) {
                            var record = data.d[0];
                            alert('Customer Number [' + record.Customer_Number +'] is already exist. Saved fail.');
                        }
                        else {//start processing save record
                            $.ajax({
                                type: "POST",
                                url: "../../CustomerWebService.asmx/GetExistCustomer",
                                data: "{first_name_en:'" + first_name_en.val() + "', last_name_en:'" + last_name_en.val() + "', gender:'" + gender.val() + "', id_card:'" + id_card.val() + "', birth_date:'" + birth_date.val() + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    //validate form
                                    if (validate_form() == false) {
                                        return false;
                                    } else {
                                        if (data.d != '') {
                                            alert('Customer is already exist. Saved fail.');
                                            return false;
                                        } else {
                                            save_customer('save');
                                        }
                                    }
                                }
                            });
                        }
                    }
                });
                */

                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/GetExistCustomer",
                    data: "{first_name_en:'" + first_name_en.val() + "', last_name_en:'" + last_name_en.val() + "', gender:'" + gender.val() + "', id_card:'" + id_card.val() + "', birth_date:'" + birth_date.val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //validate form
                        if (validate_form() == false) {
                            return false;
                        } else {
                            if (data.d != '') {
                                alert('Customer is already exist. Saved fail.');
                                return false;
                            } else {
                                save_customer('save');
                            }
                        }
                    }
                });
            }
        }

        function validate_form() {
            if (cust_type.val() == '') {
                alert('[Customer Type] is required.');
                cust_type.focus();
                return false;
            }
            //if (customer_number.val() == '') {
            //    alert('[Customer Number] is required.');
            //    customer_number.focus();
            //    return false;
            //}
            if (customer_status.val() == '') {
                alert('[Status] is required.');
                customer_status.focus();
                return false;
            }

            if (cust_type.find('option:selected').text() == 'Company') {
                if (reg_name.val() == '') {
                    alert('[Register Name] is required.');
                    reg_name.focus();
                    return false;
                } else if (reg_number.val() == '') {
                    alert('[Register Number] is required.');
                    reg_number.focus();
                    return false;
                } else if (reg_date.val() == '') {
                    alert('[Register Date] is required.');
                    reg_date.focus();
                    return false;
                }
            }
            else {// Individual and others
                if (marital_status.val() == '') {
                    alert('[Marital Status] is required.');
                    marital_status.focus();
                    return false;
                } else if (first_name_en.val() == '') {
                    alert('[First Name] is required.');
                    first_name_en.focus();
                    return false;
                } else if (last_name_en.val() == '') {
                    alert('[Last Name] is required.');
                    last_name_en.focus();
                    return false;
                } else if (id_card.val() == '') {
                    alert('[ID Card] is required.');
                    id_card.focus();
                    return false;
                } else if (nationality.val() == '') {
                    alert('[Nationality] is required.');
                    nationality.focus();
                    return false;
                } else if (birth_date.val() == '') {
                    alert('[Birth Date] is required.');
                    birth_date.focus();
                    return false;
                }

            }
            //Occupation
            if (factory.val() == '') {
                alert('[Factory] is required.');
                factory.focus();
                return false;
            }
            if (occupation.val() == '') {
                alert('[Occupation] is required.');
                occupation.focus();
                return false;
            }
            //Address
            
            if (address.val() == '') {
                alert('[Address] is required.');
                address.focus();
                return false;
            }
            if (country.val() == '') {
                alert('[Country] is required.');
                country.focus();
                return false;
            }
            else if (country.val() == 'KH') {
                if (province.val() == '') {
                    alert('[Province/City] is required.');
                    province.focus();
                    return false;
                } else if (khan.val() == '')
                {
                    alert('[Khan] is required.');
                    khan.focus();
                    return false;
                }
                else if (sangkat.val() == '') {
                    alert('[Sangkat] is required.');
                    sangkat.focus();
                    return false;
                }
            }
            if (zip_code.val() == '') {
                alert('[Zip Code] is required.');
                zip_code.focus();
                return false;
            }

            //Contact
            //if (mobile.val() == '') {
            //    alert('[Mobile] is required.');
            //    mobile.focus();
            //    return false;
            //}
            if (row_contact_no == 0)
            {
                alert('[Contact list] is required.');
               // $('#tab_contact').addClass('active');
                return false;
            }
        }
        //enable all control
        function disabled_controls(bool) {
            $('#tblContent *').prop('disabled', bool);
            $('#tabs *').prop('disabled', bool);
           
            //disable button save and delete
            //button 
            var btn_save;
            var btn_delete;
            btn_save = $('#btnSave');
            btn_delete = $('#btnDelete');
            
            btn_save.prop('disabled', bool);
            btn_delete.prop('disabled', bool);
        }
        //Clear form
        function clear_form()
        {
            $('select').prop('selectedIndex', 0); //set all dropdown list select index 0
            $("input[type=text]").val("");//clear all textboxes
            enabled_company_registration(false);

            exist_customer_number.val('');
            cust_id.val('');

            //reset variables
            customer_search_id = '';
            contact_id_deleted = '';
            row_contact_no = 0;

            //clear tbl_contact_list
            clear_contact_list();
            
            //select default value
            cust_type.prop('selectedIndex', 3);//individual
            customer_status.prop('selectedIndex', 1);//Active
            customer_status.prop('disabled', 'disabled');
            marital_status.prop('selectedIndex', 2);
            nationality.val('KH');
            country.val('KH');
            get_zip_code('KH');

            //clear customer list in search form

            $('#dvCustomerSearchList').html('');

        }
        //add new
        function add_new()
        {
            //window.location.href = 'customers.aspx';
            disabled_controls(false);
            clear_form();
        }
        //button clear
        function btn_clear()
        {
            // clear_form();
            clear_form();
            disabled_controls(true);
          
        }
        //Search
        function search_customer()
        {
            var btn = $('#btnSearchCustomer');
            if (btn.val() == 'Search') {//first search

                var full_name = $('#<%=txtSearchCustName.ClientID%>').val();
                var customer_type_id = $('#<%=ddlSearchCustType.ClientID%>').val();
                var id_card_search = $('#<%=txtSearchIDCard.ClientID%>').val();
                var customer_number_search = $('#<%=txtSearchCustNo.ClientID%>').val();

                if (full_name == '' && customer_type_id == '' && id_card_search == '' && customer_number_search == '')
                {
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
            else { //load existing customer after search it means when you click button ok
                load_customer(customer_search_id);
            }
        }

        //load exist customer
        function load_customer(customer_id)
        {
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/GetCustomerByCustID",
                data: "{customer_name:'', customer_type_id:'', id_card:'', customer_id:'" + customer_id + "', customer_number:''}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {


                    if (data.d.length > 0) {//has record
                        var customer = data.d[0];
                        //alert(data.d[0].Last_Name);
                        cust_id.val(customer.Customer_ID);
                        first_name_en.val(customer.First_Name);
                        last_name_en.val(customer.Last_Name);
                        first_name_kh.val(customer.Khmer_First_Name);
                        last_name_kh.val(customer.Khmer_Last_Name);
                        mother_first_name.val(customer.Mother_First_Name);
                        cust_type.val(customer.Customer_Type);
                        gender.val(customer.Gender);
                        id_type.val(customer.ID_Type);
                        id_card.val(customer.ID_Card);
                        birth_date.val(formatJSONDate(customer.Birth_Date));
                        marital_status.val(customer.Marital_Status);
                        customer_status.val(customer.Status);
                        customer_number.val(customer.Customer_Number);
                        nationality.val(customer.Nationality);
                        exist_customer_number.val(customer.Customer_Number); //purpose for using while user update record
                        //address
                        address.val(customer.Address);
                        country.val(customer.Country_ID);
                        zip_code.val(customer.Zip_Code);
                        province.val(customer.Province);
                        //get khan list
                        get_khan_list(customer.Province, customer.Khan);
                        //alert(customer.Khan);

                        //get sangkat list
                        get_sangkat_list(customer.Khan, customer.Sangkat);
                        //alert(customer.Sangkat);

                        //occupation
                        factory.val(customer.Sector);
                        occupation.val(customer.Occupation);
                        //company
                        reg_name.val(customer.Register_Name);
                        reg_number.val(customer.Register_Number);
                        reg_date.val(formatJSONDate(customer.Register_Date));
                        tin.val(customer.TIN_NO);

                        remarks.val(customer.Remarks);
                        //enable all controls
                        disabled_controls(false);
                        //enabled_company_registration(false);
                        company();

                        if (customer.Country_ID != 'KH') {
                            province.prop('disabled', true);
                            khan.prop('disabled', true);
                            sangkat.prop('disabled', true);
                        }
                        else {
                            province.prop('disabled', false);
                            khan.prop('disabled', false);
                            sangkat.prop('disabled', false);
                        }

                        //load contact list
                        clear_contact_list();
                        get_contact_list();

                        //reset 
                        contact_id_deleted = '';
                        row_contact_no = 0;

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
       


        //get customer id from search form
       
        function get_customer_id(customer_id, index, total_row) {
            if ($('#Ch' + index).is(':checked')) {
                customer_search_id = customer_id;
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

        //delete custoemr
        function delete_customer()
        {
            if (customer_search_id != '') {
                if (confirm("Click ok to delete customer.")) {
                    $.ajax({
                        type: "POST",
                        url: "../../CustomerWebService.asmx/DeleteCustomer",
                        data: "{customer_id:'" + customer_search_id + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            if (data.d) {
                                alert('Customer was deleted.');
                                clear_form();
                            } else {
                                alert('Customer could not be deleted.');
                            }
                        }

                    });
                }
                
            }
            else {
                alert("No customer to delete.");
            }
        }
        //varlidate contact
        function varlidate_contact()
        {
            if (contact_name.val() == '') {
                alert('Name is required.');
                contact_name.focus();
                return false;
            } else if (mobile.val() == '') {
                alert('Mobile is required.');
                mobile.focus();
                return false;
            }
        }
        //clear contact
        function clear_contact() {
            contact_name.val('');
            contact_address.val('');
            contact_remark.val('');
            mobile.val('');
            fax.val('');
            mail.val('');
            responsibility.val('');
            tel.val('');
         }
        //add contact into contact list
        var row_contact_no = 0;
        function add_contact()
        {
            if (varlidate_contact() == false)
            {
                return false;
            }
            if (selected_row_index == 0) {//add new record
                row_contact_no += 1;
               // alert(row_contact_no);
                var tbl_contact = '';
                tbl_contact += "<tr id='row_contact_list" + row_contact_no + "'>" +
                                "<td style='display:none;'> <lable id='lbl_contact_no" + row_contact_no + "'>" + row_contact_no + "</label><label id='lbl_contact_id" + row_contact_no + "'></label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_name" + row_contact_no + "' style='width:100px;'> " + contact_name.val() + "</label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id ='lbl_contact_responsibility" + row_contact_no + "' style='width:100px;'>" + responsibility.val() + "</label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_tel" + row_contact_no + "'>" + tel.val() + "</label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_mobile" + row_contact_no + "' style='width:100px;'>" + mobile.val() + "</label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_fax" + row_contact_no + "'>" + fax.val() + "</label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_mail" + row_contact_no + "'>" + mail.val() + "</label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_address" + row_contact_no + "'>" + contact_address.val() + "</label></td>" +
                                "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_remarks" + row_contact_no + "'>" + contact_remark.val() + "</label></td>" +
                                "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                                "<button type='button' id='btn_contact_delete" + row_contact_no + "' value='Delete' onclick='remove_contact(" + row_contact_no + ");' ><span class='icon-trash'></span></button>" +
                                "<button type='button' id='btn_contact_select" + row_contact_no + "' value='Select' onclick='select_contact(" + row_contact_no + ");' ><span class='icon-ok'></span></button></td" +
                                "</tr>";

                $('#tbl_contact_list').append(tbl_contact);
            } else {//update old record

                $('#lbl_contact_name' + selected_row_index).text(contact_name.val());
                $('#lbl_contact_responsibility' + selected_row_index).text(responsibility.val());
                $('#lbl_contact_tel' + selected_row_index).text(tel.val());
                $('#lbl_contact_mobile' + selected_row_index).text(mobile.val());
                $('#lbl_contact_fax' + selected_row_index).text(fax.val());
                $('#lbl_contact_mail' + selected_row_index).text(mail.val());
                $('#lbl_contact_address' + selected_row_index).text(contact_address.val());
                $('#lbl_contact_remarks' + selected_row_index).text(contact_remark.val());

                //reset selected row index
                $('#row_contact_list' + selected_row_index).removeClass('select_contact');
                selected_row_index = 0;
            }
            //clear contact fields.
            clear_contact();
        }
        //remove item from contact list
        function remove_contact(row_index)
        {
            if (row_index > 0) {
                contact_id_deleted += $('#lbl_contact_id' + row_index).text() + ";";
                alert(contact_id_deleted);
                $('#row_contact_list' + row_index).remove();

                //reset selected row index if user selectd record
                selected_row_index = 0;
                row_contact_no -= 1;
            }
        }
        //clear all row in tbl_contact_list
        function clear_contact_list()
        {
            $('#tbl_contact_list tr').each(function (index, element) {

                $('#row_contact_list' + index).remove();
            });
        }

        //select contact row
        var selected_row_index = 0;
        function select_contact(row_index) {
            selected_row_index = row_index;//use for updating contact
            $('#tbl_contact_list tr').each(function (index, element) {
                
                $('#row_contact_list' + index).removeClass('select_contact');
            });
            //$('#row_contact_list' + row_index).css('background-color', '#9974');
            $('#row_contact_list' + row_index).addClass('select_contact');
            contact_name.val($('#lbl_contact_name' + row_index).text());
            mobile.val($('#lbl_contact_mobile' + row_index).text());
            responsibility.val($('#lbl_contact_responsibility' + row_index).text());
            tel.val($('#lbl_contact_tel' + row_index).text());
            fax.val($('#lbl_contact_fax' + row_index).text());
            mail.val($('#lbl_contact_mail' + row_index).text());
            contact_address.val($('#lbl_contact_address' + row_index).text());
            contact_remark.val($('#lbl_contact_remarks' + row_index).text());
        }
        //save contact
        
        function save_contact()
        {
            if (row_contact_no > 0)//has record(s) in contact list
            {
                var id_arr = '',name_arr = '', mobile_arr = '', tel_arr = '', fax_arr='', mail_arr='', address_arr='', responsibility_arr='', remarks_arr='';
                
                for (i = 1; i <= row_contact_no; i++) {
                    // alert($('#lbl_contact_name' + i).text());
                    id_arr += $('#lbl_contact_id' + i).text()+';';
                    name_arr += $('#lbl_contact_name' + i).text() + ';';
                    mobile_arr += $('#lbl_contact_mobile' + i).text() + ';';
                    tel_arr += $('#lbl_contact_tel' + i).text() + ';';
                    fax_arr += $('#lbl_contact_fax' + i).text() + ';';
                    mail_arr += $('#lbl_contact_mail' + i).text() + ';';
                    address_arr += $('#lbl_contact_address' + i).text() + ';';
                    responsibility_arr += $('#lbl_contact_responsibility' + i).text() + ';';
                    remarks_arr += $('#lbl_contact_remarks' + i).text() + ';';
                }
                //save
                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/SaveContact",
                    data: "{cust_id:'" + cust_id.val() + "',id:'" + id_arr + "', name:'" + name_arr + "', responsibility:'" +  responsibility_arr + "', mobile:'" + mobile_arr + "', tel:'" + tel_arr + "', fax:'" + fax_arr + "', mail:'" +mail_arr+"', address:'" + address_arr+"', remarks:'" + remarks_arr+"'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.d) {
                            //alert('Contact was saved.');
                            clear_form();
                        } else {
                            alert('Customer was saved, but contact list was saved faile.');
                        }
                    }

                });

                //delete some contacts
                if (contact_id_deleted != '')
                {
                    delete_contact();
                }
            }
            
        }

        //delect contact
        var contact_id_deleted='';
        function delete_contact()
        {
            if (contact_id_deleted != '')
            {
                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/DeleteContact",
                    data: "{id:'" + contact_id_deleted + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (!data.d) {
                            alert('Contact was deleted fail.');
                        } 
                    }

                });
            }
        }

        //get contact list
        function get_contact_list()
        {
           
            $.ajax({
                type: "POST",
                url: "../../CustomerWebService.asmx/GetContactList",
                data: "{cust_id:'" + cust_id.val() +"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d.length > 0) {
                        // row_contact_no = data.d.length;
                        
                        $.each(data.d, function (i, item) {
                            //alert(item.ID);
                            row_contact_no += 1;
                            var row_contact = '';
                            row_contact += "<tr id='row_contact_list" + row_contact_no + "'>" +
                                            "<td style='display:none;'> <lable id='lbl_contact_no" + row_contact_no + "'>" + row_contact_no + "</label><label id='lbl_contact_id" + row_contact_no + "'>" + item.ID+ "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_name" + row_contact_no + "' style='width:100px;'> " + item.Name + "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id ='lbl_contact_responsibility" + row_contact_no + "' style='width:100px;'>" + item.Responsibility + "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_tel" + row_contact_no + "'>" + item.Tel + "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_mobile" + row_contact_no + "' style='width:100px;'>" + item.Mobile + "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_fax" + row_contact_no + "'>" + item.Fax + "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_mail" + row_contact_no + "'>" + item.Mail + "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_address" + row_contact_no + "'>" + item.Address + "</label></td>" +
                                            "<td style='padding:5px; height:20px; width:100px; vertical-align:middle;'><label id='lbl_contact_remarks" + row_contact_no + "'>" + item.Remarks + "</label></td>" +
                                            "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                                            "<button type='button' id='btn_contact_delete" + row_contact_no + "' value='Delete' onclick='remove_contact(" + row_contact_no + ");' ><span class='icon-trash'></span></button>" +
                                            "<button type='button' id='btn_contact_select" + row_contact_no + "' value='Select' onclick='select_contact(" + row_contact_no + ");' ><span class='icon-ok'></span></button></td>" +
                                            "</tr>";

                            $('#tbl_contact_list').append(row_contact);
                        });
                    } else {
                        alert('Contact not found.');
                    }
                }

            });
        }

        function get_khan_list(pro_id,khan_id)
        {
            if (pro_id != '') {
                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/GetKhanListByProID",
                    data: "{pro_id:'" + pro_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.d.length > 0) {
                            khan.empty();
                            khan.append($('<option>', {
                                value: '',
                                text: '--Select--'
                            }));

                            $.each(data.d, function (index, item) {
                                khan.append($('<option>', {
                                    text: item.Khan_Name,
                                    value: item.Khan_ID
                                }));
                            });
                            khan.val(khan_id);
                        } else {
                            alert('Khan not found.');
                        }
                    }

                });
            }
            else {
                khan.empty();
                khan.append($('<option>', {
                    value: '',
                    text: '--Select--'
                }));
                sangkat.empty();
                sangkat.append($('<option>', {
                    value: '',
                    text: '--Select--'
                }));
            }
        }

        function get_sangkat_list(khan_id, sangkat_id) {
            if (khan_id != '') {
                $.ajax({
                    type: "POST",
                    url: "../../CustomerWebService.asmx/GetSangkatListByKhanID",
                    data: "{khan_id:'" + khan_id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data.d.length > 0) {
                            sangkat.empty();
                            sangkat.append($('<option>', {
                                value: '',
                                text: '--Select--'
                            }));
                            $.each(data.d, function (index, item) {
                                sangkat.append($('<option>', {
                                    text: item.Sangkat_Name,
                                    value: item.Sangkat_ID
                                }));
                            });
                            sangkat.val(sangkat_id);
                        } else {
                            alert('Sangkat not found.');
                        }
                    }

                });
            }
        }
    </script>
    <script id="jTemplateCustomer" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">No</th>
                    <th style="border-width: thin; border-style: solid;">Full Name</th>
                    <th style="border-width: thin; border-style: solid;">ID Card</th>
                    <th style="border-width: thin; border-style: solid;">Gender</th>
                    <th style="border-width: thin; border-style: solid; display:none;">Customer ID</th>
                    <th style="border-width: thin; border-style: solid; text-align:center; vertical-align:middle;">Select</th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td style="vertical-align:middle;">{ $T.row$index + 1 }</td>
                        <td style="vertical-align:middle;">{ $T.row.Last_Name + ' ' + $T.row.First_Name }</td>
                        <td style="vertical-align:middle;">{ $T.row.ID_Card}</td>
                        {#if $T.row.Gender==0}
                            <td style="vertical-align:middle;">Female</td>
                        {#elseif $T.row.Gender==1}
                            <td style="vertical-align:middle;">Male</td>
                        {#else} 
                            <td style="vertical-align:middle;">-</td>
                        {#/if}
                        <td style="display:none;">{ $T.row.Customer_ID }</td>
                        <td style="text-align:center; vertical-align:middle;">
                            <input id="Ch{ $T.row$index }" type="checkbox" onclick="get_customer_id('{ $T.row.Customer_ID }', '{ $T.row$index }', '{ $T.row$total }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
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
        .select_contact {
            background-color:#24f1b2;
        }
    </style>
    <asp:ScriptManager ID="MainScriptManager" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <h1>Customer Details</h1>

           <table  id="tblContent" class="table-layout" style="background-color: #f6f6f6; padding: 40px;">
                <tr>
                    <td class="td10 tdpading10">
                        <asp:Label ID="lblCustType" Text="Customer Type" runat="server"></asp:Label>
                        
                    </td>
                    <td class="td10 tdpading10">
                        <asp:DropDownList runat="server" ID="ddlCustomerType" DataSourceID="ds_Customer_Type" DataTextField="Customer_Type" DataValueField="Customer_Type_ID" AppendDataBoundItems="true" onchange="company();">
                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <span class="required">*</span>
                    </td>
                    <td class="td10 tdpading10">
                        <asp:Label ID="lblStatus" Text="Status" runat="server"></asp:Label></td>
                    <td class="td10 tdpading10">
                        <asp:DropDownList runat="server" ID="ddlStatus" DataSourceID="ds_status" DataTextField="status" DataValueField="status_id" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="td10">
                        <asp:Label ID="lblCustNumber" Text="Customer Number" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtCustNumber"  ReadOnly="true" Enabled="false"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdfCustomerNumber" Value="" />
                        <span class="required">*</span>
                    </td>
                    <td class="td10">
                        <asp:Label ID="lblMaritalStatus" Text="Marital Status" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:DropDownList ID="ddlMaritalStatus" runat="server">
                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                            <asp:ListItem Text="Single" Value="SINGLE"></asp:ListItem>
                             <asp:ListItem Text="Married" Value="MARRIED"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="td10">
                        <asp:Label ID="lblKhFirstName" Text="Khmer First Name" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtKhFirstName" Height="20px"></asp:TextBox></td>
                    <td class="td10">
                        <asp:Label ID="lblKhLastName" Text="Khmer Last Name" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtKhLastName" Height="20px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="td10">
                        <asp:Label ID="lblFirstName" Text="First Name" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtFirstName" onchange="paste_name_to_contact_name();"></asp:TextBox>
                        <span class="required">*</span>
                    </td>
                    <td class="td10">
                        <asp:Label ID="lblLastName" Text="Last Name" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtLastName" onchange="paste_name_to_contact_name();"></asp:TextBox>
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
                        <asp:Label ID="lblRegName" Text="Register Name" runat="server"></asp:Label>
                    </td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtRegName"></asp:TextBox>
                        <span class="required">*</span>
                    </td>
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
                        <asp:Label ID="lblRegNumber" Text="Register Number" runat="server"></asp:Label>
                    </td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtRegNumber"></asp:TextBox>
                        <span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="td10">
                        <asp:Label ID="lblIDCard" Text="ID Card" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtIDCard"></asp:TextBox>
                        <span class="required">*</span>
                    </td>
                    <td class="td10">
                        <asp:Label ID="lblRegDate" Text="Register Date" runat="server"></asp:Label>
                    </td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtRegDate" CssClass="datepicker" placeholder="[DD\MM\YYYY]"></asp:TextBox>
                        <span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="td10">
                        <asp:Label ID="lblNationality" Text="Nationality" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:DropDownList runat="server" ID="ddlNationality" DataSourceID="ds_Nationality" DataTextField="Nationality" DataValueField="Country_ID" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <span class="required">*</span>
                    </td>
                    <td class="td10">
                        <asp:Label ID="lblTinNumber" Text="TIN Number" runat="server"></asp:Label>

                    </td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtTinNumber"></asp:TextBox>
                        <span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="td10">
                        <asp:Label ID="lblBirthDate" Text="Birth Date" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="datepicker" placeholder="[DD\MM\YYYY]"></asp:TextBox>
                         <span class="required">*</span>
                    </td>
                    <td class="td10"></td>
                    <td class="td10"></td>
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
                <tr>
                    <td class="td10">
                        <asp:Label ID="lblFactory" runat="server" Text="Company Sector" ></asp:Label>
                    </td>
                    <td class="td10">
                        <asp:DropDownList ID="ddlFactory" runat="server" DataSourceID="ds_business_type" DataTextField="business_name" DataValueField="id" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <span class="required">*</span>
                    </td>
                    <td class="td10">
                        <asp:Label ID="lblOccupation" Text="Occupation" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtOccupation"></asp:TextBox>
                        <span class="required">*</span>
                    </td>
                   
                </tr>
                <tr>
                     <td class="td10">
                        <asp:Label ID="lblRemarks" Text="Remarks" runat="server"></asp:Label></td>
                    <td class="td10">
                        <asp:TextBox runat="server" ID="txtRemarks"></asp:TextBox>

                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4" class="td10"><%--Tabs--%>
                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                            <li class="active"><a href="#address" data-toggle="tab">Address</a></li>
                            <li id="tab_contact"><a href="#contact" data-toggle="tab">Contact</a></li>
                        </ul>
                        <div class="container">
                            <div class="tab-content">

                                <div id="my-tab-content" class="tab-content">
                                    <div class="tab-pane active" id="address">
                                        <%//Address Form %>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td class="td40">
                                                    <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                                                </td>
                                                <td class="td60">
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="txt100" Height="20px"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td class="td40">
                                                    <asp:Label ID="lblProvince" runat="server" Text="Province/City"></asp:Label>
                                                </td>
                                                <td class="td60">
                                                    <%-- <asp:DropDownList ID="ddlProvince" runat="server" CssClass="ddl100"></asp:DropDownList>--%>
                                                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="ddl100" DataSourceID="ds_province" DataValueField="pro_id" DataTextField="pro_name" AppendDataBoundItems="true" onchange="get_khan_list(this.value);">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td class="td40">
                                                    <asp:Label ID="lblKhan" runat="server" Text="Khan"></asp:Label>
                                                </td>
                                                <td class="td60">
                                                   
                                                    <asp:DropDownList ID="ddlKhan" runat="server"  CssClass="ddl100" AppendDataBoundItems="true" onchange="get_sangkat_list(this.value);">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td class="td40">
                                                    <asp:Label ID="lblSangkat" runat="server" Text="Sangkat"></asp:Label>
                                                </td>
                                                <td class="td60">
                                                   
                                                    <asp:DropDownList ID="ddlSangkat" runat="server"  CssClass="ddl100" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td40">
                                                    <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                                                </td>
                                                <td class="td60">
                                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="ddl100" DataSourceID="ds_Country" DataTextField="Country_Name" DataValueField="Country_ID" onchange="get_zip_code(this.value);" AppendDataBoundItems="true" >
                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="required">*</span>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td40">
                                                    <asp:Label ID="lblZipCode" runat="server" Text="Zip Code"></asp:Label>
                                                </td>
                                                <td class="td60">
                                                    <asp:TextBox ID="txtZipCode" runat="server" CssClass="txt100" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                    <div class="tab-pane" id="contact">
                                        <table style="width: 100%;" border="0">
                                            
                                            <tr>
                                                <td style="width:10%;">
                                                    <asp:Label ID="lblContactName" runat="server" Text="Name"></asp:Label>
                                                </td>
                                                <td style="width:40%">
                                                    <asp:TextBox ID="txtContactName" runat="server" style="width:95%;"></asp:TextBox>
                                                     <span class="required">*</span>
                                                </td>
                                               <td style="width:10%; padding-left:20px;">
                                                    <asp:Label ID="lblResponsibility" runat="server" Text="Responsibility"></asp:Label>
                                                </td>
                                                 <td style="width:40%;">
                                                    <asp:TextBox ID="txtResponsibility" runat="server" style="width:95%;"></asp:TextBox>

                                                </td>
                                                 <td rowspan="5" style="width:5%; vertical-align:bottom; text-align:center; padding-right:10px;">
                                                     <%--<input type="button"  style="margin-left:10px; margin-bottom:5px;" onclick="add_contact();" />--%>
                                                     <button type="button" style="margin-left:10px; margin-bottom:5px;" id="btn_add" class="btn btn-large btn-primary" onclick="add_contact();"><span class="icon-plus"></span></button>
                                                     
                                                </td>
                                              
                                            </tr>
                                            <tr>
                                               <td style="width:10%;">
                                                    <asp:Label ID="lblTel" runat="server" Text="Tel"></asp:Label>
                                                </td>
                                                 <td style="width:40%;">
                                                    <asp:TextBox ID="txtTel" runat="server" style="width:95%;"></asp:TextBox>

                                                </td>
                                                <td style="width:10%; padding-left:20px;">
                                                    <asp:Label ID="lblMobile" runat="server" Text="Mobil"></asp:Label>
                                                </td>
                                                 <td style="width:40%;">
                                                    <asp:TextBox ID="txtMobile" runat="server" style="width:95%;"></asp:TextBox>
                                                    <span class="required">*</span>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td style="width:10%;">
                                                    <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                                                </td>
                                                <td style="width:40%;">
                                                    <asp:TextBox ID="txtFax" runat="server" style="width:95%;"></asp:TextBox>
                                                </td>
                                                <td style="width:10%; padding-left:20px;">
                                                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                                                </td>
                                                 <td style="width:40%;">
                                                    <asp:TextBox ID="txtEmail" runat="server" style="width:95%;"></asp:TextBox>
                                                </td>
                                               
                                            </tr>
                                           
                                            <tr>
                                               <td style="width:10%;">
                                                    <asp:Label ID="lblContactAddress" runat="server" Text="Address"></asp:Label>
                                                </td>
                                                 <td style="width:40%;">
                                                    <asp:TextBox ID="txtContactAddress" runat="server" style="width:95%;"></asp:TextBox>
                                                </td>
                                              <td style="width:10%; padding-left:20px;">
                                                    <asp:Label ID="lblContactRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td style="width:40%;">
                                                    <asp:TextBox ID="txtContactRemarks" runat="server" style="width:95%;"></asp:TextBox>
                                                    
                                                </td>
                                            </tr>
                                           
                                        </table>
                                        <div id="div_contact_list">
                                            <table id="tbl_contact_list" class="table table-bordered">
                                                <tr style="background-color: lightgray;">
                                                    <td style="display:none;">No</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Name</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Responsibility</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Tel</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Mobile</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Fax</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Mail</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Address</td>
                                                    <td style="padding:5px; height:30px; width:100px; vertical-align:middle; ">Remarks</td>
                                                    <td style="padding:5px; height:30px; width:50px; vertical-align:middle; "></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </td>
                    <%-- End Tabs--%>
                </tr>
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
        </ContentTemplate>
       
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdlCustID" runat="server" Value="" />
    <asp:SqlDataSource ID="ds_Country" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Country_Name FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_Nationality" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Country_ID, Nationality FROM dbo.Ct_Country ORDER BY Country_Name "></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_Customer_Type" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Customer_Type_ID, Customer_Type  FROM Ct_Customer_Type ORDER BY Customer_Type"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_status" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT STATUS_ID, STATUS FROM TBL_STATUS"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_province" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT PRO_ID, PRO_NAME FROM TBL_PROVINCE ORDER BY PRO_NAME;"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ds_business_type" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT GTLI_Business_ID AS ID, Business_Name FROM Ct_GTLI_Business ORDER BY Business_Name;"></asp:SqlDataSource>
</asp:Content>
