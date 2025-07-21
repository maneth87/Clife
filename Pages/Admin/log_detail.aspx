<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="log_detail.aspx.cs" Inherits="Pages_Admin_log_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script>
        function readlog()
        {
            var date = "";
            date = $('#<%= txtdate.ClientID%>').val();
            if (date != '') {
                // $('#message').html('Test');
               
                var btn = $('#<%=btnRead.ClientID%>');
                btn.click();
            }
            else {
                alert('Please input date');
            }
        }
        $(document).ready(function () {
            $('.datepicker').datepicker();
           // readlog();
        });
        
      
    </script>
    <div class="panel panel-default">
        <div class="panel-body">
            <table>
                <tr>
                    <td>Date:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtdate" CssClass=" datepicker"></asp:TextBox>
                    </td>
                    <td style="vertical-align:middle;">
                        <input type="button" value="OK" class=" btn btn-primary" onclick="readlog();" />
                        <asp:Button ID="btnRead" runat="server" Text ="OK" CssClass="btn-primary" OnClick="btnRead_Click" style="display:none;" />
                    </td>
                </tr>
            </table>
            <p style="font-family:Arial; font-size:12px;"></p>
        </div>
        <div class="panel-heading">
            <h3 class="panel-title">System Log Detail</h3>
        </div>
        <div class="panel-body">
            <div id="message" runat="server" style=" padding:5px;"></div>
        </div>
    </div>
</asp:Content>

