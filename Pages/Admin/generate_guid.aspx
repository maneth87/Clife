<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="generate_guid.aspx.cs" Inherits="Pages_Admin_generate_guid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <script>
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "../../HelperWebService.asmx/GetTables",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //$.each(data, function () {
                    //    $('#Main_ddlTable').append($('<option></option>').val(data.d).html(data.d));
                    //});
                    $('#Main_ddlTable').append($('<option>', {
                        value: '',
                        text: '---Select---'
                    }));
                    $.each(data.d, function (i, item) {
                        //$('#Main_ddlTable').append(new Option(item, item));
                        $('#Main_ddlTable').append($('<option>', {
                            value: item,
                            text: item
                        }));
                    });
                }
            });
        });

        function get_columns_name(table) {
            $.ajax({
                type: "POST",
                url: "../../HelperWebService.asmx/GetColumns",
                data: "{table_name:'" + table + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var ddl_col = $('#Main_ddlColumn');

                    ddl_col.empty();
                    ddl_col.append($('<option>', {
                        value: '',
                        text: '---Select---'
                    }));
                    $.each(data.d, function (i, item) {
                        //$('#Main_ddlTable').append(new Option(item, item));
                        ddl_col.append($('<option>', {
                            value: item,
                            text: item
                            
                        }));
                      
                    });
                }
            });
        }

        function get_guid()
        {
            var tbl = $('#Main_ddlTable').val();
            var col = $('#Main_ddlColumn').val();
            if (tbl == '' || col == '') {
                alert('Please Select.');
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "../../HelperWebService.asmx/GetGuid",
                    data: "{table_name:'" + tbl + "',column_name:'" + col + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Main_txtGuid').val(data.d);
                    }
                });
            }
           
        }
    </script>



            <h1>Generate Guid</h1>
            <table class="table-layout" style="background-color: #f6f6f6; padding: 40px;">

                <tr>
                    <td style="padding-left: 10px; padding-top: 10px; padding-bottom: 10px; vertical-align: middle;">
                        <table>
                            <tr>
                                <td style="vertical-align: middle;">
                                    <asp:Label runat="server" ID="lblTable" Text="Select Table"></asp:Label>
                                </td>
                                <td style="vertical-align: middle;">
                                    <asp:DropDownList runat="server" ID="ddlTable" onchange="get_columns_name(this.value);"></asp:DropDownList>
                                </td>
                                <td style="vertical-align: middle;"></td>
                            </tr>
                            <tr>
                                <td style="vertical-align: middle;">
                                    <asp:Label runat="server" ID="lblColumn" Text="Select Column Name"></asp:Label>
                                </td>
                                <td style="vertical-align: middle;">
                                    <asp:DropDownList runat="server" ID="ddlColumn"></asp:DropDownList>
                                </td>
                                <td style="vertical-align: middle;">
                                    <input type="button" value="Generate" onclick="get_guid();" class="btn btn-primary" />
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: middle; padding-top: 20px; padding-right: 10px;" colspan="3">
                                    <asp:TextBox runat="server" Text="" ID="txtGuid" Width="100%"></asp:TextBox>
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
       
</asp:Content>

