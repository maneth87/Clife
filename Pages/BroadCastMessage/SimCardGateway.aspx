<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="SimCardGateway.aspx.cs" Inherits="Pages_BroadCastMessage_SimCardGateway" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script>
        $(document).ready(function () {

            load_data();

            current_gateway = $('#<%=txtCurrentUsingGateway.ClientID%>');
            change_to = $('#<%=ddlChangeTo.ClientID%>');

            current_gateway.prop('disabled', true);


            //button save
            $('#btnSave').click(function () {

                if (current_gateway.val() == '')
                {
                    return (alert('Current gateway is required.'), false);
                }
                else if (change_to.val() == '')
                {
                    return (alert('Change to is required.'), false);
                }
                $.ajax({
                    type: "POST",
                    url: "../../MessageWebService.asmx/UpdateGateway",
                    data: "{old_number:'" + current_gateway.val() + "', new_number:'" + change_to.val() +"'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (data) {
                           
                            alert('Saved successfully.');
                            $('#div_change_gateway').modal('hide');

                            //reload data
                            $('#tblSimCardsGateway tr').each(function (index, element) {
                                $('#row_id' + index).remove();
                            });
                            load_data();
                        }
                        else {
                            //some code here
                            alert('Saved fail.');
                        }
                    },
                    error: function (err) {
                        alert("Page Save Error: " + err);
                    }


                });
                
            });

        });

        function load_data()
        {
            var row_id = 0;
           

            $.ajax({
                type: "POST",
                url: "../../MessageWebService.asmx/GetPrefixNumber",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d.length > 0) {
                       
                        $.each(data.d, function (index, item) {

                            row_id += 1;
                            var row = "<tr id='row_id" + row_id + "'>" +
                                        "<td><label id='lblNo" + row_id + "'>" + row_id + "</label></td>" +
                                        "<td style='display:block;'><label id='lblCompanyPhoneTypeID" + row_id + "'>" + item.CompanyPhoneTypeID + "</label></td>" +
                                        "<td><label id='lblGatewayName" + row_id + "'>" + item.Gateway + "</label></td>" +
                                        "<td><label id='lblPhoneNumber" + row_id + "'>" + item.MessageFrom + "</label></td>" +
                                        "<td style='padding:3px; height:20px; width:50px; vertical-align:middle; text-align:center;'>" +
                                        "<button type='button' id='btnDelete" + row_id + "' disabled='true'><span class='icon-remove'></span></button>" +
                                        "<button type='button' id='btnEdit" + row_id + "' onclick='open_edit(" + row_id + ");'> <span class='icon-edit'></span></button></td>" +
                                     "</tr>";
                           
                            $('#tblSimCardsGateway').append(row);
                           
                        });
                        
                    }
                    else {
                        //some code here
                        alert('No record(s) found.');
                    }
                },
                error: function (err) {
                    alert(err);
                }


            });
        }

        function bind_gateway()
        {
           
            $.ajax({
                type: "POST",
                url: "../../MessageWebService.asmx/GetGateway",
                data: "{phone_company_type_id:'" + phone_company_type_id +"', phone_number:'" + current_gateway.val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    change_to.empty();
                    change_to.append($('<option>',{text:'--Select--', value:''}));
                    if (data.d.length > 0) {
                       
                        $.each(data.d, function (index, item) {

                            change_to.append($('<option>', {
                                text: item.PhoneNumber,
                                value: item.PhoneNumber
                            }));

                        });
                        $('#btnSave').prop('disabled', false);
                    }
                    else {
                        //some code here
                        $('#btnSave').prop('disabled', true);
                    }
                },
                error: function (err) {
                    alert("Page error: " + err);
                    $('#btnSave').prop('disabled', true);
                }


            });
        }

        var current_gateway;
        var change_to;
        var phone_company_type_id = 0;
        function open_edit(row_index) {
            var phone = $('#lblPhoneNumber' + row_index).text();
             phone_company_type_id =  $('#lblCompanyPhoneTypeID' + row_index).text();
            current_gateway.val(phone);
            bind_gateway();

            //show form
            $('#div_change_gateway').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <div id="main" class="panel panel-default" runat="server">
        <div class="panel-heading">
            <h3 class="panel-title">SIM Cards Gateway</h3>
        </div>
        <div class="panel-body">
            <table id="tblSimCardsGateway" class='table table-bordered'>
                <tr>
                    <th>No.</th>
                    <th style="display: block;">Company Phone Type ID</th>
                    <th>Gateway Name</th>
                    <th>SIM Cards Number</th>
                    <th style="width: 100px;"></th>
                </tr>
            </table>
        </div>

        <div id="div_change_gateway" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="div_change_gateway" aria-hidden="true">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 class="panel-title">Change Gateway</h3>
            </div>
            <div class="modal-body">
                <table>
                    <tr>
                        <td style="width: 150px;">
                            <asp:Label ID="lblCurrentUsingGateway" runat="server" Text="Current Using Gateway"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrentUsingGateway" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblChangeTo" runat="server" Text="Change To"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlChangeTo" runat="server"></asp:DropDownList>
                        </td>
                    </tr>

                </table>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" id="btnSave" >Save</button>
            </div>
        </div>
    </div>
     <div id="div_err" runat="server" style="text-align: center; vertical-align: middle; margin-top: 30px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 10px; height: 20px;">
    </div>
</asp:Content>

