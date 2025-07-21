<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="reinsurance_report.aspx.cs" Inherits="Pages_Reports_reinsurance_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar" style="text-align: left">

        <li>
            <asp:ImageButton ID="ImgExcel" runat="server" ImageUrl="~/App_Themes/functions/download_excel.png" Style="display: none;" OnClick="ImgExcel_Click" />
            <asp:ImageButton ID="ImgSearch" runat="server" ImageUrl="~/App_Themes/functions/download_excel.png" Style="display: none;" OnClick="ImgSearch_Click" />
            <input type="button"  onclick="excel();" style="background: url('../../App_Themes/functions/download_excel.png') no-repeat; border: none; height: 40px; width: 100px; "  />
            
        </li>
        <li>
            <input type="button" onclick="search();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 100px;" />
        </li>


    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('.datepicker').datepicker();
           
        });

        function excel() {

            var btn = $('#<%=ImgExcel.ClientID%>');
            btn.click();

        }
        function search()
        {
            var btn = $('#<%=ImgSearch.ClientID%>');
            var date = $('#<%=txtToDate.ClientID%>');
            if (date.val() != '') {
                btn.click();
                //$('#excel').show();
            } else {
                alert('Please select date!');
            }
        }
    </script>


    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Export To Excel</h3>

                </div>
                <div class="panel-body">
                    <table>
                        <tr>
                            <td>To Date: </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Product Type:</td>
                            <td>
                                <asp:DropDownList ID="ddlProductType" runat="server" CssClass="dropdown">

                                </asp:DropDownList>
                            </td>
                        </tr>

                    </table>
                    <div style="margin-top: 20px;">
                        <p class="text-info" style="font-size: small;"></p>
                    </div>
                    <div id="message" runat="server" style="font-family:Arial; font-size:12px;">
                    </div>
                </div>

            </div>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:PostBackTrigger ControlID="ImgPrint" />--%>
        </Triggers>
    </asp:UpdatePanel>
   <table class="table-bordered"></table>
</asp:Content>

