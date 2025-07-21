<%@ Page Language="C#" Title="Clife | Accounting => Policy Payment" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_payment.aspx.cs" Inherits="Pages_Policy_Payment_policy_payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <ul class="toolbar" style="text-align: left">
        <%--   <li>
            <asp:ImageButton ID="ImgPrint_Auto"  runat="server" ImageUrl="~/App_Themes/functions/print.png"  OnClientClick="Print_default();"/>
       </li>--%>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" />
            </div>
            <input type="button" runat="server" onclick="GetReceipNo_Edit()" style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
            </div>
            <input type="button" runat="server" style="background: url('../../App_Themes/functions/save.png') no-repeat; border: none; height: 40px; width: 90px;" onclick="GetReceipNo()" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnSearch" data-toggle="modal" data-target="#myModalSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" />
        </li>
    </ul>

    <script>

        //Format Date in Jtemplate
        function formatJSONDate(jsonDate) {
            var date = eval(jsonDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
            return dateFormat(date, "dd/mm/yyyy");
        }

        var Result_Remaining_Month;

        //Get Policy Num
        var search_poli_num;
        var prem_amount;
        var cus_name;
        var product;
        var sum_insured;
        var premYear;
        var due_date;
        var normal_due_date;
        var prem_mode;
        //var period_pay;
        var next_due;
        var method_payment;
        var Policy_Status;

        var Policy_Prem_Pay_ID_Value;
        var Receipt_ID;
        var Receipt_Num;
        var Normal_Prem;
        var Product_Type;



        function GetPreList() {
            var Policy_Number_search = $('#Main_txtPoliNumberSearch').val();
            var Customer_ID_search = "";

            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetPreValue",
                data: "{Policy_Number:'" + Policy_Number_search + "',Customer_ID:'" + Customer_ID_search + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_txtPolicyNum').val(data.d.Policy_Number);
                    $('#Main_hdfPlicyID').val(data.d.Policy_ID);

                    $('#Main_txtCustomerID').val(data.d.Customer_ID);
                    $('#Main_hdfCusID').val(data.d.Customer_ID);

                    $('#Main_ddlPayMode').val(data.d.Pay_Mode_ID);
                    $('#Main_hdfPayMode').val(data.d.Pay_Mode_ID);

                    $('#Main_txtPrem').val(data.d.Amount);
                    $('#Main_hdfPrem').val(data.d.Amount);
                    $('#Main_lblAmount').val(data.d.Amount);

                    $('#Main_hdfOfficeID').val(data.d.Office_ID);
                    $('#Main_hdfSaleAgentID').val(data.d.Sale_Agent_ID);

                    $('#Main_txtDue').val(formatJSONDate(data.d.Due_Date));
                    $('#Main_hdfDue').val(formatJSONDate(data.d.Normal_Due_Date));


                    $('#Main_txtPeriod').val(1);

                    // Next Due
                    Calculate_Next_Due();

                },
                error: function (msg) {

                    alert(msg);
                }
            });
        }

        function GetPre_PolNum_Mode() {
            var Policy_Number_search = $('#Main_hdfPolicyNo').val();
            var ddl_Mode = $('#Main_ddlPayMode').val();
            var period = $('#Main_txtPeriod').val();
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetPreValue_Poli_Num_Pay_Mode",
                data: "{Policy_Number:'" + Policy_Number_search + "',Pay_Mode_ID:'" + ddl_Mode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    $('#Main_lblAmount').val(data.d);
                    var period = $('#Main_txtPeriod').val();

                    if ($('#Main_txtPeriod').val() != "") {

                        var pre = $('#Main_lblAmount').val();

                        $('#Main_txtPrem').val(+period * +pre);
                        $('#Main_hdfPrem').val(+period * +pre);

                        $('#Main_hdfNormal_Prem').val(+period * +pre);

                        var pre_amount = +period * +pre;
                        var interest_amount = $('#Main_hdf_interest').val();
                        var total_pre = +pre_amount + +interest_amount;

                        $('#Main_txtTotal_Prem').val(total_pre);

                    }
                    else {
                        $('#Main_txtPrem').val(data.d);
                        $('#Main_hdfPrem').val(data.d);
                        $('#Main_txtTotal_Prem').val(data.d);
                    }

                    // Get Total Month
                    GetTotal_Month();

                    // Next Due
                    Calculate_Next_Due();

                },
                error: function (msg) {

                    alert(msg);
                }
            });
        }

        function GetTotal_Month() {
            var Policy_ID = $('#Main_hdfPlicyID').val();
            var ddl_Mode = $('#Main_ddlPayMode').val();
            var period = $('#Main_txtPeriod').val();

            var ddl_hdf = $('#Main_hdfPayMode').val();

            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/GetMonth_Paid",
                data: "{Policy_ID:'" + Policy_ID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var total_month = data.d;
                    var remaining_month = 12 - +total_month;

                    if (remaining_month == 1 || remaining_month == 2) {

                        if (ddl_Mode != 4) {
                            alert("Only Monthly can use, please check it again");

                            $('#Main_ddlPayMode').val(ddl_hdf);

                            $('#Main_hdf_PayMode_OK').val(1);
                        }
                        else { $('#Main_hdf_PayMode_OK').val(""); }
                    }
                    else if (remaining_month == 3) {
                        if (ddl_Mode != 3 && ddl_Mode != 4) {
                            alert("Only Monthly and Quarterly can use, please check it again");

                            $('#Main_ddlPayMode').val(ddl_hdf);

                            $('#Main_hdf_PayMode_OK').val(1);
                        }
                        else { $('#Main_hdf_PayMode_OK').val(""); }
                    }
                    else if (remaining_month == 6 && remaining_month < 12) {
                        if (ddl_Mode == 1) {
                            alert("Annually cannot use, please check it again");

                            $('#Main_ddlPayMode').val(ddl_hdf);

                            $('#Main_hdf_PayMode_OK').val(1);
                        }
                        else { $('#Main_hdf_PayMode_OK').val(""); }
                    }
                },
                error: function (msg) {

                    alert(msg);
                }
            });
        }

        function Calculate_Next_Due() {

            Date.isLeapYear = function (year) {
                return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
            };

            Date.getDaysInMonth = function (year, month) {
                return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
            };

            Date.prototype.isLeapYear = function () {
                var y = this.getFullYear();
                return (((y % 4 === 0) && (y % 100 !== 0)) || (y % 400 === 0));
            };

            Date.prototype.getDaysInMonth = function () {
                return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
            };

            Date.prototype.addMonths = function (value) {
                var n = this.getDate();
                this.setDate(1);
                this.setMonth(this.getMonth() + value);
                this.setDate(Math.min(n, this.getDaysInMonth()));
                return this;
            };

            var ddl_Mode = $('#Main_ddlPayMode').val();
            var period = $('#Main_txtPeriod').val();
            var string_due = $('#Main_hdfDue').val();
            var arr = string_due.split('/');

            var due_date = arr[1] + '/' + arr[0] + '/' + arr[2]

            var myDate = new Date(due_date);

            var next_due;

            //Annual
            if (ddl_Mode == 1) {

                next_due = myDate.addMonths(12 * +period); //run
            }
                // Semi
            else if (ddl_Mode == 2) {

                next_due = myDate.addMonths(6 * +period);
            }
                // Quarter
            else if (ddl_Mode == 3) {

                next_due = myDate.addMonths(3 * +period);
            }
                //Month
            else if (ddl_Mode == 4) {

                next_due = myDate.addMonths(1 * +period);
            }

            var next_due1 = formattedDate(next_due);

            $('#Main_txtNextDue').val(next_due1);
            $('#Main_hdfNextDue').val(formattedDate_hfdNext_Due(next_due1));

        }

        function formattedDate(date) {
            var d = new Date(date || Date.now()),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [day, month, year].join('/');
        }

        function formattedDate_hfdNext_Due(date) {
            var d = new Date(date || Date.now()),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [month, day, year].join('/');
        }

        function CalculatePre() {

            var period_pay = $('#Main_txtPeriod').val();
            var pre_val = $('#Main_lblAmount').val();
            var first_preriod_pay = $('#Main_hdfperiod_pay').val();

            var row_count = $('#tbl_PolicyLapsed tr').length - 1;
            row_count = +row_count - +1;

            if (+row_count < 1) {

                $('#Main_txtPrem').val(+period_pay * +pre_val);
                $('#Main_hdfPrem').val(+period_pay * +pre_val);
                $('#Main_txtTotal_Prem').val(+period_pay * +pre_val);

                Calculate_Next_Due();
            }
            else {

                $('#Main_txtPeriod').val(first_preriod_pay);
            }

        }

        function Remaining_Month() {
            var Policy_ID = $('#Main_hdfPlicyID').val();
            var ddl_Mode = $('#Main_ddlPayMode').val();
            var pro_type = Product_Type;

            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/Result_Remaining_Month",
                data: "{Policy_ID:'" + Policy_ID + "', Pay_Mode_ID:'" + ddl_Mode + "', Product_Type:'" + pro_type + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    Result_Remaining_Month = data.d;

                },
                error: function (msg) {

                    alert(msg);
                }
            });
        }

        function Check_PolicyLapsed() {

            // $("#Main_ddlPayMode").attr("disabled", true); // disable ddlPayMode

            Remaining_Month();

            var current_paymode = $('#Main_ddlPayMode').val();
            var totalmonth_left = Result_Remaining_Month;

            if (Remaining_Month == -1) {
                alert("This Product can use only Singal Payment");
            }
            else {

                if (totalmonth_left >= 6 && totalmonth_left <= 11) {

                    if (current_paymode == 0 || current_paymode == 1) {

                        alert("Singal & Annually Payment are not available");

                        $('#Main_ddlPayMode').val(2);

                        GetPre_PolNum_Mode();
                    }
                }
                else if (totalmonth_left >= 3 && totalmonth_left <= 5) {

                    if (current_paymode != 3 || current_paymode != 4) {
                        alert("Only Quarterly & Monthly are available");

                        $('#Main_ddlPayMode').val(3);

                        GetPre_PolNum_Mode();
                    }
                }
                else if (totalmonth_left > 0 && totalmonth_left <= 2) {

                    if (current_paymode != 4) {
                        alert("Only Monthly Payment Mode is available");

                        $('#Main_ddlPayMode').val(4);

                        GetPre_PolNum_Mode();
                    }
                }

                var first_pay_mode = $('#Main_hdfPayMode').val();

                var first_preriod_pay = $('#Main_hdfperiod_pay').val();

                var row_count = $('#tbl_PolicyLapsed tr').length - 1;
                row_count = +row_count - +1;

                if (+row_count < 1) {

                    GetPre_PolNum_Mode();
                }
                else {

                    $('#Main_ddlPayMode').val(first_pay_mode);
                }
            }
        }

        function onclick_ok() {
            var check_Receipt = $('#Main_txtReceiptNo').val();
            var check_Paydate = $('#Main_txtPayDate').val();

            if (check_Receipt != "" && check_Paydate != "") {
                var btnOk = document.getElementById('<%= btnOk.ClientID %>'); //dynamically click button

            btnOk.click();

            ClearText();
        }
        else {
            if (check_Paydate == "" && check_Receipt != "") {
                alert("Please select Pay Date");
            }
            else if (check_Paydate != "" && check_Receipt == "") {
                alert("Please input Receipt Number");
            }
            else {
                alert("Receipt Number and Pay Date cannot be blank");
            }
        }
    }

    function onclick_edit() {
        var check_Receipt = $('#Main_txtReceiptNo').val();
        var check_Paydate = $('#Main_txtPayDate').val();

        if (check_Receipt != "" && check_Paydate != "") {
            var btnEdit = document.getElementById('<%= btnEdit.ClientID %>'); //dynamically click button

            btnEdit.click();

            ClearText();
        }
        else {
            if (check_Paydate == "" && check_Receipt != "") {
                alert("Please select Pay Date");
            }
            else if (check_Paydate != "" && check_Receipt == "") {
                alert("Please input Receipt Number");
            }
            else {
                alert("Receipt Number and Pay Date cannot be blank");
            }
        }
    }

    function ClearText() {
        $('#Main_txtPolicyNum').val("");
        $('#Main_hdfPlicyID').val("");

        $('#Main_txtCustomerID').val("");
        $('#Main_hdfCusID').val("");

        $('#Main_ddlPayMode').val("");
        $('#Main_hdfPayMode').val("");

        $('#Main_txtPrem').val("");
        $('#Main_hdfPrem').val("");

        $('#Main_hdfOfficeID').val("");
        $('#Main_hdfSaleAgentID').val("");

        $('#Main_txtDue').val("");
        $('#Main_hdfDue').val("");

        $('#Main_txtReceiptNo').val("");
        $('#Main_txtPayDate').val("");
        $('#Main_txtPeriod').val("");

        $('#Main_txtPolicyNum_Search').val("");
        $('#Main_txtCustomerID_Search').val("");

        $('#Main_txtTotal_Prem').val("");
        $('#Main_txtInterestPrem').val("");

        $('#Main_lblAmount').val("");
    }

    function ValidateTextDecimal(j) {

        var msg = j.value;
        var w;
        //var nokta = msg.indexOf(".");
        var nokta;

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

    /**************/

    function GetPreList_by_PoliID() {

        $('#dvPolicyList').val("");

        var Policy_Number_search = $('#Main_txtPoliNumberSearch').val();
        var Customer_ID_search = $('#Main_txtCustIDSearch').val();
        var Cus_Last = $('#Main_txtLastNameSearch').val();
        var Cus_First = $('#Main_txtFirstNameSearch').val();


        $.ajax({
            type: "POST",
            url: "../../PolicyWebService.asmx/SearchPolicy_Num",
            data: "{Policy_Number:'" + Policy_Number_search + "', Customer_ID:'" + Customer_ID_search + "', Last_Name:'" + Cus_Last + "', First_Name:'" + Cus_First + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.d != "" || data.d != null) {

                    $('#dvPolicyList').setTemplate($("#jTemplatePolicy").html());
                    $('#dvPolicyList').processTemplate(data);
                }

                // Next Due
                //Calculate_Next_Due();
            },
            error: function (msg) {

                alert(msg);
            }
        });
    }

    function GetPreLapsedList_by_PoliID() {

        $('#dvPolicyLapsed').val("");

        var Policy_Number_search = $('#Main_txtPoliNumberSearch').val();
        var Customer_ID_search = $('#Main_txtCustIDSearch').val();
        var Cus_Last = $('#Main_txtLastNameSearch').val();
        var Cus_First = $('#Main_txtFirstNameSearch').val();

        $.ajax({
            type: "POST",
            url: "../../PolicyWebService.asmx/Search_Lapsed_Policy",
            data: "{Policy_Number:'" + Policy_Number_search + "', Customer_ID:'" + Customer_ID_search + "', Last_Name:'" + Cus_Last + "', First_Name:'" + Cus_First + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                ///Policy Lapsed
                $('#Main_txtInterestPrem').val(0);

                $('#dvPolicyLapsed').setTemplate($("#JTemplatePolicyLapsed").html());
                $('#dvPolicyLapsed').processTemplate(data);

                GetDetailValue(data);

            },
            error: function (msg) {

                alert(msg);
            }
        });

        //Clear text after searching
        $('#Main_txtPoliNumberSearch').val("");
        $('#Main_txtCustIDSearch').val("");
        $('#Main_txtLastNameSearch').val("");
        $('#Main_txtFirstNameSearch').val("");
    }

    function GetDetailValue(data) {
        var string_value = data.d[0].interest_amount + ",";
        var total_interest = data.d[0].interest_amount;

        for (var i = 1; i < data.d.length; i++) {
            var item = data.d[i];
            string_value += item.interest_amount + "/";
            total_interest += item.interest_amount;
        }

        if (total_interest > 0) {

            RounInterestUpDown(total_interest);
        }
        else {

            $('#Main_txtInterestPrem').val(0);
        }

        $('#Main_hdf_interest').val(string_value);
    }

    function RounInterestUpDown(total_interest) {

        var interest_round_up_down = total_interest;

        $.ajax({
            type: "POST",
            url: "../../PolicyWebService.asmx/RounInterestUp_Down",
            data: "{interest_total:'" + interest_round_up_down + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.d != "") {

                    $('#Main_txtInterestPrem').val(data.d);
                    $('#Main_hdf_Totalinterest').val(data.d);
                } else {

                    $('#Main_txtInterestPrem').val(0);
                    $('#Main_hdf_Totalinterest').val(0);
                }
            },
            error: function (msg) {

                alert(msg);
            }
        });
    }

    function GetPolicy(last_name, first_name, prem, poli_num, product_name, sumInsured, dueDate, premMode
                        , poli_id, office_id, sale_id, Policy_Prem_Pay_ID, ReceiptID, ReceiptNum, NormalDueDate, NormalPrem, PolicyStatus, ProductType, index, total) {

        if ($('#Ch' + index).is(':checked')) {

            Policy_Status = PolicyStatus;
            Product_Type = ProductType;

            search_poli_num = poli_num;
            prem_amount = prem;
            cus_name = last_name + '' + first_name;
            product = product_name;
            sum_insured = sumInsured;
            due_date = dueDate;
            normal_due_date = NormalDueDate;
            prem_mode = premMode;
            Receipt_ID = ReceiptID;
            Receipt_Num = ReceiptNum;
            Policy_ID = poli_id;
            Normal_Prem = NormalPrem

            $('#Main_hdfNormal_Prem').val(Normal_Prem);

            $('#Main_hdfPlicyID').val(poli_id);
            $('#Main_hdfPolicyNo').val(search_poli_num);

            $('#Main_ddlPayMode').val(premMode);
            $('#Main_hdfPayMode').val(premMode);

            $('#Main_hdfPrem').val(prem);
            $('#Main_lblAmount').val(prem);

            $('#Main_hdfOfficeID').val(office_id);
            $('#Main_hdfSaleAgentID').val(sale_id);

            $('#Main_hdfDue').val(formatJSONDate(normal_due_date));

            $('#Main_hdfReceipt_ID').val(Receipt_ID);
            $('#Main_hdfReceipt_Num').val(Receipt_Num);


        } else {
            search_poli_num = "";
        }


        //Uncheck all other checkboxes
        for (var i = 0; i < total  ; i++) {
            if (i != index) {
                $('#Ch' + i).prop('checked', false);
            }

        }
    }

    function GetPrem_Year() {
        var Policy_ID = $('#Main_hdfPlicyID').val();

        $.ajax({
            type: "POST",
            url: "../../PolicyWebService.asmx/GetPrem_Year",
            data: "{Policy_ID:'" + Policy_ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                premYear = data.d;

                var period = $('#Main_txtPeriod').val();
                var mode = $('#Main_ddlPayMode').val();

                if (mode == 0) {
                    if (period == 1) {

                        $('#Main_txtYear_Prem').val(+premYear);
                    }
                    els
                    {
                        premYear = +premYear - 1;
                    }
                }
                else if (mode == 1) {

                    if (period == 1) {

                        $('#Main_txtYear_Prem').val(premYear);
                    }
                    els
                    {
                        premYear = +premYear - 1;

                        $('#Main_txtYear_Prem').val(+premYear + +period);
                    }
                }
                else if (mode == 2) {
                    if (period == 1) {

                        $('#Main_txtYear_Prem').val(+premYear);
                    }
                    else {
                        period = period / 2;

                        premYear = +premYear + +period;

                        $('#Main_txtYear_Prem').val(+premYear);
                    }
                }
                else if (mode == 3) {

                    if (period == 1) {

                        $('#Main_txtYear_Prem').val(+premYear);
                    }
                    else {
                        period = period / 4;

                        premYear = +premYear + +period;

                        $('#Main_txtYear_Prem').val(+premYear);
                    }
                }
                else if (mode == 4) {

                    if (period == 1) {

                        $('#Main_txtYear_Prem').val(+premYear);
                    }
                    else {
                        period = period / 12;

                        premYear = +premYear + +period;

                        $('#Main_txtYear_Prem').val(+premYear);
                    }
                }
            },
            error: function (msg) {

                alert(msg);
            }
        });
    }

    function FillSearchedData() {

        if (search_poli_num != "") {

            var total_interest_minus = $('#Main_hdf_Totalinterest').val();

            $('#Main_txtReceived').val(cus_name);

            $('#Main_txtTotal_Prem').val(prem_amount);

            $('#Main_txtPrem').val(+prem_amount - +total_interest_minus);
            $('#Main_hdfPrem').val(prem_amount);
            $('#Main_lblAmount').val(prem_amount);

            $('#Main_txtPolicyNo').val(search_poli_num);
            $('#Main_txtInsurance_Plan').val(product);
            $('#Main_txtSum_Insured').val(sum_insured);

            GetPrem_Year();

            $('#Main_txtDue').val(formatJSONDate(due_date));
            $('#Main_hdfDue').val(formatJSONDate(normal_due_date));

            // Check Available Pay Mode
            $('#Main_ddlPayMode').val(prem_mode);
            $('#Main_hdfPayMode').val(prem_mode);

            var row_count = $('#tbl_PolicyLapsed tr').length - 1;
            row_count = +row_count - +1;


            if (row_count > 0) {
                $('#Main_txtPeriod').val(row_count);
                $('#Main_hdfperiod_pay').val(row_count);

            } else {

                $('#Main_txtPeriod').val(1);
                $('#Main_hdfperiod_pay').val(1);

            }

            $('#Main_ddlMethod_Payment').val(0);

            Calculate_Next_Due();

        } else {
            alert("Plese select a checkbox");
        }
    }

    //Move cursor to next row textbox on enter press
    function EnterPress(obj, event) {
        var keyCode;
        if (event.keyCode > 0) {
            keyCode = event.keyCode;
        }
        else if (event.which > 0) {
            keyCode = event.which;
        }
        else {
            keycode = event.charCode;
        }
        if (keyCode == 13) {
            document.getElementById(obj).focus();
            return false;
        }
        else {
            return true;
        }
    }

    function GetReceipNo() {

        var Receipt_Num = $('#Main_txtReceiptNo').val();
        var Due_Date = $('#Main_hdfDue').val();
        var Policy_ID = $('#Main_hdfPlicyID').val();

        $.ajax({
            type: "POST",
            url: "../../PolicyWebService.asmx/GetReceiptNo",
            data: "{Receipt_Num:'" + Receipt_Num + "', Due_Date:'" + Due_Date + "', Policy_ID:'" + Policy_ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.d != "") {
                    alert(data.d);
                }
                else {
                    onclick_ok();
                }
            },
            error: function (msg) {

                alert(msg);
            }
        });
    }

    function GetReceipNo_Edit() {
        var receipt_no = $('#Main_txtReceiptNo').val();
        var Policy_Prem_Pay_ID = Policy_Prem_Pay_ID_Value;

        $.ajax({
            type: "POST",
            url: "../../PolicyWebService.asmx/GetReceiptNo_Edit",

            data: "{Receipt_Num:'" + receipt_no + "', Policy_Prem_Pay_ID:'" + Policy_Prem_Pay_ID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.d != "") {
                    alert("Duplication Receipt No (" + receipt_no + "), please check it again");
                }
                else {
                    onclick_edit();
                }
            },
            error: function (msg) {

                alert(msg);
            }
        });
    }

    $(document).ready(function () {

        $('.datepicker').datepicker()
        .on('changeDate', function (pay_date) {

            var input_pay_date = (pay_date.date);
            input_pay_date = formattedDate(input_pay_date);

            var due_date = $('#Main_hdfDue').val();

            var Policy_ID = $('#Main_hdfPlicyID').val();
            var Pay_Mode_ID = $('#Main_ddlPayMode').val();
            var Prem_Paid = $('#Main_hdfNormal_Prem').val();
            var Pol_Num = $('#Main_hdfPolicyNo').val();

            // Check duration whether it is Lapsed or not
            Calculate_Lapsed_Prem(Policy_ID, Pay_Mode_ID, input_pay_date, due_date, Prem_Paid, Pol_Num);

        });
    });

    function Calculate_Lapsed_Prem(Policy_ID, Pay_Mode_ID, Pay_Date, Due_Date, Pre_Paid, Policy_Num) {

        var interest_amount = $('#Main_hdf_Totalinterest').val();
        var get_period_pay = $('#Main_txtPeriod').val();

        $.ajax({
            type: "POST",
            url: "../../PolicyWebService.asmx/Calculate_Lapsed_Prem",

            data: "{Policy_ID:'" + Policy_ID + "', Pay_Mode_ID:'" + Pay_Mode_ID + "', Pay_Date:'" + Pay_Date + "', Due_Date:'" + Due_Date + "', Prem_Paid:'" + Pre_Paid + "', interest_amount:'" + interest_amount + "', Policy_Num:'" + Policy_Num + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.d != "") {

                    var get_data = data.d; Calculate_Lapsed_Prem
                    get_data = get_data.split(',');

                    var check_period_pay = get_data[0];
                    var prem_amount = get_data[1];

                    if (check_period_pay > 0) {
                        $('#Main_txtPeriod').val(check_period_pay);
                    }


                    $('#Main_txtPrem').val(+prem_amount * +get_period_pay);
                    $('#Main_hdfPrem').val(+prem_amount * +get_period_pay);

                    $('#Main_hdfNextDue').val(formattedDate(get_data[2]));

                    var inter_prem = get_data[3];

                    if (inter_prem == "") {

                        $('#Main_txtInterestPrem').val(0);
                    }
                    else {
                        $('#Main_txtInterestPrem').val(get_data[3]);
                    }

                    $('#Main_hdf_interest').val(get_data[4]);

                    $('#Main_txtTotal_Prem').val(+(+prem_amount * +get_period_pay) + +inter_prem);

                }
            },
            error: function (msg) {

                alert(msg);
            }
        });
    }


    </script>

    <%--Jtemplate Sections--%>

    <script id="jTemplatePolicy" type="text/html">
        <table class="table table-bordered">
            <thead style="text-align: center">
                <tr>
                    <th style="border-width: thin; border-style: solid;">No</th>
                    <th style="border-width: thin; border-style: solid;">Policy No</th>
                    <th style="border-width: thin; border-style: solid;">Product</th>
                    <th style="border-width: thin; border-style: solid;">Last Name</th>
                    <th style="border-width: thin; border-style: solid;">First Name</th>
                    <th style="border-width: thin; border-style: solid;">Gender</th>
                    <th style="border-width: thin; border-style: solid;">Prem Year </th>
                    <th style="border-width: thin; border-style: solid;">Prem Lot </th>
                    <th style="border-width: thin; border-style: solid;">Pay Mode</th>
                    <th style="border-width: thin; border-style: solid;">Sum Insure</th>
                    <th style="border-width: thin; border-style: solid;">Amount</th>
                    <th style="border-width: thin; border-style: solid;">Due Date</th>
                    <th style="border-width: thin; border-style: solid;">Sale Agent ID</th>


                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as row}
                    <tr>
                        <td style="text-align: center">{ $T.row$index + 1 }</td>
                        <td style="text-align: center">{ $T.row.Policy_Number }</td>
                        <td style="text-align: center">{ $T.row.En_Abbr }</td>
                        <td>{ $T.row.Last_Name }</td>
                        <td>{ $T.row.First_Name }</td>
                        <td style="text-align: center">{ $T.row.Gender }</td>
                        <td style="text-align: center">{ $T.row.Prem_Year }</td>
                        <td style="text-align: center">{ $T.row.Prem_Lot }</td>
                        <td style="text-align: center">{ $T.row.Mode }</td>
                        <td style="text-align: right">${ $T.row.Sum_Insure }</td>
                        <td style="text-align: right">${ $T.row.Amount }</td>
                        <td style="text-align: right">{ formatJSONDate($T.row.Due_Date) }</td>
                        <td style="text-align: center">{ $T.row.Sale_Agent_ID }</td>
                        <td>
                            <input id="Ch{ $T.row$index }" type="checkbox" onclick="GetPolicy('{$T.row.Last_Name}', '{$T.row.First_Name}', '{$T.row.Amount}',
                                                                                              '{$T.row.Policy_Number}', '{$T.row.En_Abbr}', '{$T.row.Sum_Insure}',
                                                                                              '{$T.row.Due_Date}', '{$T.row.Pay_Mode_ID}', '{$T.row.Policy_ID}',
                                                                                              '{$T.row.Office_ID}', '{$T.row.Sale_Agent_ID}', '{$T.row.Policy_Prem_Pay_ID}',
                                                                                              '{$T.row.Receipt_ID}', '{$T.row.Receipt_Num}', '{$T.row.Normal_Due_Date}',
                                                                                              '{$T.row.Normal_Prem}', '{$T.row.Policy_Status_Type_ID}', '{$T.row.Product_Type_ID}', '{$T.row$index}', '{$T.row$total}');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>

    <%-- Form Design Section--%>

    <%-- Lapsed Records --%>
    <script id="JTemplatePolicyLapsed" type="text/html">
        <table id="tbl_PolicyLapsed" class="table table-bordered">
            <thead style="text-align: center">
                <tr>
                    <th style="border-width: thin; border-style: solid; text-align: center;">No</th>
                    <th style="border-width: thin; border-style: solid; text-align: center;">Due Date</th>
                    <th style="border-width: thin; border-style: solid; text-align: center;">Interest Amount</th>
                    <th style="border-width: thin; border-style: solid; text-align: center;">Prem Amount</th>
                    <th style="border-width: thin; border-style: solid; text-align: center">Duration Days</th>
                    <th style="border-width: thin; border-style: solid; text-align: center">Duration Months</th>
                    <th style="border-width: thin; border-style: solid; text-align: center">Status</th>

                </tr>
            </thead>
            <tbody>
                {#param name=Total_Interest value=0}
                {#param name=Total_Prem value=0}
                {#param name=Total_DurationDay value=0}

                {#foreach $T.d as row}
                    <tr>
                        <td style="text-align: center">{ $T.row$index + 1 }</td>
                        <td style="text-align: center">{ formatJSONDate($T.row.due_date)}</td>
                        <td style="text-align: center">{ $T.row.interest_amount }</td>
                        <td style="text-align: center">{ $T.row.prem_amount }</td>
                        <td style="text-align: center">{ $T.row.duration }</td>
                        <td style="text-align: center">{ $T.row.duration_month }</td>
                        <td style="text-align: center">{ $T.row.policy_status }</td>
                    </tr>
                {#param name=Total_Interest value=$P.Total_Interest + ($T.row.interest_amount)}
                {#param name=Total_Prem value=$P.Total_Prem + ($T.row.prem_amount)}
                {#param name=Total_DurationDay value=$P.Total_DurationDay + ($T.row.duration)}

                {#/for}
               
            </tbody>

            <tr>
                <td class="header"></td>
                <td class="header" style="text-align: right">Total:</td>
                <td class="header">{$P.Total_Interest}</td>
                <td class="header">{$P.Total_Prem}</td>
                <td class="header">{$P.Total_DurationDay}</td>
                <td class="header"></td>
                <td class="header"></td>
            </tr>
        </table>
    </script>
    <%-- End of Lapsed Records--%>

    <style>
        .pickter_size {
            width: 222px;
        }

        .text_size {
            width: 125px;
            text-align: right;
        }

        /*---------*/

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


        /*-- mine --*/
        .text_pre {
            width: 412px;
        }

        .text_pre_only {
            width: 398px;
        }

        .com_box {
            width: 425px;
        }

        .check_box {
            margin-top: 50px;
        }

        .text_method {
            width: 250px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>

    <asp:ScriptManager ID="MainScriptManager" runat="server" />

    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--here is the block code--%>

            <!-- Modal Search Policy -->
            <div id="myModalSearch" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="myModalSearch" aria-hidden="true">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Policy Form</h3>
                </div>
                <div class="modal-body">

                    <asp:HiddenField ID="hdfProduct" runat="server" />

                    <asp:HiddenField ID="hdf_interest" runat="server" />
                    <asp:HiddenField ID="hdf_Totalinterest" runat="server" />

                    <%--value="{ $T.row.Policy_Number }--%>
                    <asp:HiddenField ID="hdfSaleAgentID" runat="server" />
                    <asp:HiddenField ID="hdfPlicyID" runat="server" />
                    <asp:HiddenField ID="hdfCusID" runat="server" />

                    <asp:HiddenField ID="hdfReceipt_ID" runat="server" />
                    <asp:HiddenField ID="hdfReceipt_Num" runat="server" />

                    <asp:Label ID="lblAmount" runat="server" Text="Label" ForeColor="White"></asp:Label>

                    <asp:HiddenField ID="hdfOfficeID" runat="server" />
                    <asp:HiddenField ID="hdf_PayMode_OK" runat="server" />

                    <asp:HiddenField ID="hdfperiod_pay" runat="server" />
                    <asp:HiddenField ID="hdfNormal_Prem" runat="server" />

                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="myTabPolicySearch">
                        <li class="active"><a href="#SPoliNo" data-toggle="tab" style="text-decoration: none;">Search By Policy Num</a></li>
                        <li><a href="#SCustomerName" data-toggle="tab" style="text-decoration: none;">Search By Customer Name</a></li>
                        <li><a href="#SCusID" data-toggle="tab" style="text-decoration: none;">Search By Customer ID</a></li>
                    </ul>

                    <div class="tab-content" style="height: 60px; overflow: hidden;">
                        <div class="tab-pane active" id="SPoliNo">
                            <table style="width: 98%">
                                <tr>
                                    <td style="width: 13%; vertical-align: middle">Policy Num:</td>
                                    <td style="width: 30%; vertical-align: bottom">
                                        <asp:TextBox ID="txtPoliNumberSearch" Width="90%" runat="server"></asp:TextBox>

                                    </td>
                                    <td style="width: 56%; vertical-align: top">
                                        <input type="button" class="btn" style="height: 27px;" onclick="GetPreList_by_PoliID(); GetPreLapsedList_by_PoliID();" value="Search" />
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
                                        <input type="button" class="btn" style="height: 27px;" onclick="GetPreList_by_PoliID();" value="Search" />
                                    </td>
                                </tr>
                            </table>
                            <hr />
                        </div>

                        <div class="tab-pane active" id="SCusID">
                            <table style="width: 98%">
                                <tr>
                                    <td style="width: 13%; vertical-align: middle">Customer ID:</td>
                                    <td style="width: 30%; vertical-align: bottom">
                                        <asp:TextBox ID="txtCustIDSearch" Width="90%" runat="server"></asp:TextBox>

                                    </td>
                                    <td style="width: 56%; vertical-align: top">
                                        <input type="button" class="btn" style="height: 27px;" onclick="GetPreList_by_PoliID();" value="Search" />
                                    </td>
                                </tr>
                            </table>
                            <hr />
                        </div>
                    </div>
                    <div id="dvPolicyLapsed"></div>
                    <div id="dvPolicyList"></div>
                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillSearchedData();" data-dismiss="modal" value="Select" />

                    <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                </div>
            </div>
            <!--End Modal Search -->

            <%-- Form Design Section--%>

            <br />
            <br />
            <br />

            <div class="panel panel-default">
                <div class="panel-heading">

                    <h3 class="panel-title">Policy Payment</h3>
                </div>

                <div class="panel-body">

                    <table width="100%">                        
                        <tr>
                            <td>
                                <table id="tblPolicy" width="100%" style="margin-left: 10px; margin-bottom: 10px;">
                                    <tr>
                                        <td style="width: 140px;">No:</td>

                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtReceiptNo" runat="server" MaxLength="50" Width="96%"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Pay Date:</td>

                                        <td>
                                            <asp:TextBox ID="txtPayDate" runat="server" MaxLength="50" Width="96%" CssClass="datepicker span2; text_pre; text_pre"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Received from:</td>

                                        <td>
                                            <asp:TextBox ID="txtReceived" runat="server" MaxLength="50" Width="96%" Enabled="false"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Gross Premium: &nbsp;<asp:Label ID="Label1" runat="server" Text="($)" ForeColor="Red"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtPrem" runat="server" MaxLength="50" Width="96%" Enabled="false"></asp:TextBox>
                                           

                                            <asp:HiddenField ID="hdfPrem" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Interest Premium:&nbsp;<asp:Label ID="Label3" runat="server" Text="($)" ForeColor="Red"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtInterestPrem" runat="server" MaxLength="50" Width="96%" Enabled="false"></asp:TextBox>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total Premium:&nbsp;<asp:Label ID="Label4" runat="server" Text="($)" ForeColor="Red"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtTotal_Prem" runat="server" MaxLength="50" Width="96%" Enabled="false" ForeColor="Red"></asp:TextBox>
                                            

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Policy No:</td>
                                        <td>
                                            <asp:TextBox ID="txtPolicyNo" runat="server" MaxLength="50" Width="96%" Enabled="false"></asp:TextBox></td>
                                        <asp:HiddenField ID="hdfPolicyNo" runat="server" />
                                    </tr>
                                    <tr>
                                        <td>Insurance Plan:</td>
                                        <td>
                                            <asp:TextBox ID="txtInsurance_Plan" runat="server" MaxLength="50" Width="96%" Enabled="false"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Sum Insured:&nbsp;<asp:Label ID="Label2" runat="server" Text="($)" ForeColor="Red"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtSum_Insured" runat="server" MaxLength="50" Width="96%" Enabled="false"></asp:TextBox>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Year of Premium:</td>
                                        <td>
                                            <asp:TextBox ID="txtYear_Prem" runat="server" MaxLength="50" Width="96%" Enabled="false"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>Due Date:</td>
                                        <td>
                                            <asp:TextBox ID="txtDue" runat="server" MaxLength="50" Width="96%" CssClass="datepicker span2; text_pre" Enabled="false"></asp:TextBox></td>

                                        <asp:HiddenField ID="hdfDue" runat="server" />
                                    </tr>
                                    <tr>
                                        <td>Premium Mode:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlPayMode" class="span2" runat="server" Width="97.2%" TabIndex="5" onchange="Check_PolicyLapsed()" CssClass="com_box">
                                                <asp:ListItem Value="0">Single</asp:ListItem>
                                                <asp:ListItem Value="1">Annually</asp:ListItem>
                                                <asp:ListItem Value="2">Semi</asp:ListItem>
                                                <asp:ListItem Value="3">Quarterly</asp:ListItem>
                                                <asp:ListItem Value="4">Monthly</asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:HiddenField ID="hdfPayMode" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Period Pay:</td>
                                        <td>
                                            <asp:TextBox ID="txtPeriod" runat="server" MaxLength="49" TabIndex="4" onkeyup="CalculatePre(); ValidateTextDecimal(this);" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Next Due:</td>
                                        <td>
                                            <asp:TextBox ID="txtNextDue" runat="server" MaxLength="49" TabIndex="7" Width="96%" CssClass="datepicker span2; text_pre" Enabled="false"></asp:TextBox>

                                            <asp:HiddenField ID="hdfNextDue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Method of payment:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlMethod_Payment" class="span2" runat="server" TabIndex="5" Width="97.2%" CssClass="com_box">
                                                <asp:ListItem Value="0">Cash</asp:ListItem>
                                                <asp:ListItem Value="1">Cheque</asp:ListItem>
                                                <asp:ListItem Value="2">Others</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </div>

            </div>

            <%-- Section Hidenfields Initialize  --%>
            <asp:HiddenField ID="hdfuserid" runat="server" />
            <asp:HiddenField ID="hdfusername" runat="server" />

            <%-- End Section Hidenfields Initialize  --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
