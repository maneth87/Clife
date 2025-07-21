<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="new_policy_in_month.aspx.cs" Inherits="Pages_Business_Reports_new_policy_in_month" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <script type="text/javascript">

        function open_report(opt) {
            if (fdate.val() == '' && tdate.val() == '') {


                alert('Report filter is required.');
            }
            else {

                btnexcel.click();

            }

        };
        function cls() {
            fdate.val('');
            tdate.val('');
        };
        var fdate;
        var tdate;
        var btnexcel;
        $(document).ready(function () {
            $('.datepicker').datepicker();

            fdate = $('#<%=txtFromDate.ClientID%>');
            tdate = $('#<%=txtTodate.ClientID%>');

            btnexcel = $('#<%=btnExcel.ClientID%>');

            $('#tbl_filter').css('margin-left', ($('#div_content').width() / 2) - ($('#tbl_filter').width() / 2));
            $('#tbl_filter').css('border', 'solid 2px black');
        });
    </script>
    <ul class="toolbar">
        <li>
            <input type="button" style="border-style: none; border-color: inherit; border-width: medium; background: url('../../App_Themes/functions/download_excel.png') no-repeat; height: 40px; width: 90px;" onclick="open_report('excel');" />
            <asp:Button ID="btnExcel" runat="server" Style="display: none;" OnClick="btnExcel_Click" />
        </li>

        <li>
            <input type="button" style="border-style: none; border-color: inherit; border-width: medium; background: url('../../App_Themes/functions/clear.png') no-repeat; height: 40px; width: 90px;" onclick="cls();" />
        </li>
    </ul>
    <h1>New Policy In Month Report</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <br />
    <div class="container" id="div_content">

        <table style="margin: 10px; border: solid 2px;" id="tbl_filter">
            <tr>
                <td colspan="4">
                    <h3>Report Filter</h3>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <asp:Label ID="lblFromDate" runat="server" Text="Date From"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" PlaceHolder="DD/MM/YYYY" CssClass=" datepicker"></asp:TextBox></td>
                <td>
                    <asp:Label ID="lblToDate" runat="server" Text="To"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" PlaceHolder="DD/MM/YYYY" CssClass="datepicker"></asp:TextBox></td>
            </tr>

        </table>

    </div>
</asp:Content>


