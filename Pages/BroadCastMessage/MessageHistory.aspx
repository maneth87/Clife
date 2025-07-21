<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="MessageHistory.aspx.cs" Inherits="Pages_BroadCastMessage_MessageHistory" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <ul class="toolbar" id="ul" runat="server">
        <li>
            <!-- Button to trigger modal new business form -->
            <input type="button" id="btnClear" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
    </ul>
    <style>
        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 250%;
            background-color: #ababab;
            z-index: 9999999;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=90);
        }

        #modal {
            display: none;
            position: absolute;
            top: 40%;
            left: 45%;
            width: 64px;
            height: 64px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 9999999;
            text-align: center;
            overflow: auto;
            
        }
    </style>
    <script>


        $(document).ready(function () {
            $('.datepicker').datepicker();

            $('#btnSearch').click(function () {
                btn_Search_Click();
            });

            $('#btnClear').click(function () {
                var from_date = $('#<%=txtFromDate.ClientID%>');
                var to_date = $('#<%=txtToDate.ClientID%>');
                var message_cate = $('#<%=ddlMessageCate.ClientID%>');
                var status_code = $('#<%=ddlStatusCode.ClientID%>');

                from_date.val('<%=DateTime.Now.ToString("dd/MM/yyyy")%>');
                to_date.val('<%=DateTime.Now.ToString("dd/MM/yyyy")%>');

                //from_date.val('');
                //to_date.val('');
                message_cate.val('');
                status_code.val('');
                $('#div_result').html('');
                $('#div_records').html('');
                $('#btnResend').css('display', 'none');
                $('#btnExport').css('display', 'none');
            });

            $('#btnResend').css('display', 'none');
            $('#btnExport').css('display', 'none');

            $('#btnResend').click(function () {
                if (!confirm('Do you want send fail messages?')) {
                    return false;
                }
                var total_row = 0;
                var log_ids = new Array();
                var history_ids = new Array();
                var messages = new Array();
                //$('#tblMessage tr').each(function (index, element) {
                $('#tblMessage tr:gt(0)').each(function (index, element) {
                  
                    var i = index + 1;

                    history_ids[index] = $('#lblHistoryRecordID' + (i)).text();
                    log_ids[index] = $('#lblLogID' + (i)).text();
                    //messages[index] = $('#tdMessageFrom' + (index)).text() + ',' + $('#tdMessageTo' + (index)).text() + ',' + $('#tdMessageCate' + (index)).text() + ',' + $('#tdMessageText' + (index)).text();
                    messages[index] = $('#tdMessageFrom' + (i)).text() + '___' + $('#tdMessageTo' + (i)).text() + '___' + $('#tdMessageCate' + (i)).text() + '___' + $('#tdMessageText' + (i)).text();
                });
                $.ajax({
                    type: "POST",
                    url: "../../MessageWebService.asmx/SendMessage",
                    data: JSON.stringify({ message_info: messages, history_id: history_ids, log_id: log_ids }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d != '') {

                            alert('Resent successfully.');
                        }
                        else {
                            alert('Resent fail.');
                        }
                    }

                });
                $('#btnClear').click();
            });

            $('#btnExport').click(function () {
                if (confirm('Do you want to export data to excel?')) {
                   
                     $('#<%=btnExport1.ClientID%>').click();

                }
            });

            $('#<%=txtFromDate.ClientID%>').val('<%=DateTime.Now.ToString("dd/MM/yyyy")%>');
            $('#<%=txtToDate.ClientID%>').val('<%=DateTime.Now.ToString("dd/MM/yyyy")%>');
        });

        function btn_Search_Click() {
            var from_date = $('#<%=txtFromDate.ClientID%>');
            var to_date = $('#<%=txtToDate.ClientID%>');
            var message_cate = $('#<%=ddlMessageCate.ClientID%>');
            var status_code = $('#<%=ddlStatusCode.ClientID%>');

            if (from_date.val() == '') {
                alert('Please Select From Date');
                return false;
            }
            else if (to_date.val() == '') {
                alert('Please Select To Date');
                return false;
            }
            else {
                loading();
                var tbl = "<table id='tblMessage' class='table table-bordered'><th>No.</th><th>Message Cate.</th><th>Message From</th><th>Message To</th><th>Message Text</th><th>Send Time</th><th>Status</th>";
                var status_stye = '';
                var no = 0;
                $.ajax({
                    type: "POST",
                    url: "../../MessageWebService.asmx/GetMessageList",
                    //data: JSON.stringify({ from_date: from_date.val(), to_date: to_date.val(), message_cate: message_cate.val() , status_code: status_code.val()}),
                    data: "{from_date:'" + from_date.val() + "', to_date:'" + to_date.val() + "', message_cate:'" + message_cate.val() + "', status_code:'" + status_code.val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.length > 0) {
                            $.each(data.d, function (index, item) {
                                no += 1;
                                if (item.StatusCode.trim() == 'SUCCESS') {
                                    tbl += "<tr id='tr" + no + "' style='height:25px;'><td>" + item.No + "<label style='display:none;' id='lblHistoryRecordID" + no + "'>" + item.HistoryRecordID + "</label><label style='display:none;' id='lblLogID" + no + "'>" + item.LogID + "</label></td><td id='tdMessageCate" + no + "'>" + item.MessageCate + "</td><td id='tdMessageFrom" + no + "'>" + item.MessageFrom + "</td><td id='tdMessageTo" + no + "'>" + item.MessageTo + "</td><td id='tdMessageText" + no + "'>" + item.MessageText + '</td><td>' + formatJSONDate(item.SendTime) + "</td><td style='color:green;'>" + item.StatusCode + '</td></tr>';

                                }
                                else if (item.StatusCode.trim() == 'FAIL') {
                                    tbl += "<tr id='tr" + no + "' style='height:25px;'><td>" + item.No + "<label style='display:none;' id='lblHistoryRecordID" + no + "'>" + item.HistoryRecordID + "</label><label  style='display:none;' id='lblLogID" + no + "'>" + item.LogID + "</label></td><td id='tdMessageCate" + no + "'>" + item.MessageCate + "</td><td id='tdMessageFrom" + no + "'>" + item.MessageFrom + "</td><td id='tdMessageTo" + no + "'>" + item.MessageTo + "</td><td id='tdMessageText" + no + "'>" + item.MessageText + '</td><td>' + formatJSONDate(item.SendTime) + "</td><td style='color:red;'>" + item.StatusCode + '</td></tr>';
                                   
                                }

                            });

                            $('#div_result').html(tbl + '</table>');
                            $('#div_records').html(no + ' Record(s) Found.');

                            //show butten resent
                            if (no > 0 && status_code.val() == '300') {
                                $('#btnResend').css('display', 'block');
                            }
                            else {
                                $('#btnResend').css('display', 'none');

                            }
                            if (no > 0) {
                                $('#btnExport').css('display', 'block');
                            }

                        }
                        else {
                            $('#div_result').html('');
                            $('#div_records').html('Not Found.');
                            $('#btnExport').css('display', 'none');
                        }
                        clear_loading();
                    }

                });

            }
        }

        function loading() {
            $("#modal").show();
            $("#fade").show();
        }
        function clear_loading() {
            $("#modal").hide();
            $("#fade").hide();
        }

        

        //Format Date in Jtemplate
        function formatJSONDate(jsonDate) {
            var value = jsonDate;
            if (value.substring(0, 6) == "/Date(") {
                var dt = new Date(parseInt(value.substring(6, value.length - 2)));
                var day, month;

                if (dt.getDate() <= 9) {
                    day = '0' + dt.getDate();

                }
                else {
                    day = dt.getDate();
                }
                if ((dt.getMonth() + 1) <= 9) {
                    month = '0' + (dt.getMonth() + 1);
                }
                else {
                    month = dt.getMonth() + 1;
                }
                //var dtString = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                var dtString = day + "/" + month + "/" + dt.getFullYear();

                value = dtString;
            }
            return value;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <br />
    <br />
    <br />

    <div id="fade"></div>
    <div id="modal">
        <img id="loader" src="../../App_Themes/functions/loading.gif" alt="" />
        <span id="process">Loading...</span>
    </div>
    <div id="div_main" runat="server" class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Sent Message History</h3>
        </div>
        <div class="panel-body">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblFromDate" Text="From Date" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker"></asp:TextBox>
                        <span class="icon-calendar"></span>
                    </td>
                    <td>
                        <asp:Label ID="lblToDate" Text="To Date" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker"></asp:TextBox>
                        <span class="icon-calendar"></span>
                    </td>
                    <td style="padding-left: 20px;">
                        <button type="button" id="btnSearch" class="btn  btn-primary">Search <span class="icon-search"></span></button>

                    </td>
                    <td>
                        <button type="button" id="btnExport" class="btn btn-primary">Export to Excel <span class="icon-arrow-up"></span></button>
                        <asp:Button ID="btnExport1" runat="server" OnClick="btnExport1_Click" style="display:none;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessageCate" Text="Message Cate." runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMessageCate" runat="server" DataSourceID="ds_message_cate" DataTextField="cate" DataValueField="cate" AppendDataBoundItems="true">
                            <asp:ListItem Value="" Text="ALL"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblStatusCode" Text="Status" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatusCode" runat="server">
                            <asp:ListItem Value="" Text="ALL"></asp:ListItem>
                            <asp:ListItem Value="200" Text="SUCCESS"></asp:ListItem>
                            <asp:ListItem Value="300" Text="FAIL"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 20px;">
                        <%--<input type="button" id="btnResend" class="btn  btn-primary" value="Resend" />--%>
                        <button type="button" id="btnResend" class="btn  btn-primary">
                            Resend 
                             <span class="icon-refresh"></span>
                        </button>
                    </td>
                    <td></td>
                </tr>
            </table>
            <div style="color: red;" id="div_records"></div>
            <div id="div_result"></div>
        </div>
    </div>
    <asp:SqlDataSource ID="ds_message_cate" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" runat="server" SelectCommand="select cate from MessageCate order by cate asc;"></asp:SqlDataSource>
</asp:Content>

