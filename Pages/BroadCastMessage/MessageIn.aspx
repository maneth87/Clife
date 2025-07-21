<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="MessageIn.aspx.cs" Inherits="Pages_BroadCastMessage_MessageIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
     <ul class="toolbar" id="ul" runat="server">
        <li>
            <!-- Button to trigger modal new business form -->
            <input type="button" id="btnClear" style="background: url('../../App_Themes/functions/clear.png') no-repeat; border: none; height: 40px; width: 90px;" />
           <asp:Button  ID="btnClear1" runat="server" OnClick="btnClear1_Click" style="display:none;"/>
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
        
        var from_date;
        var to_date
        $(document).ready(function () {
            $('.datepicker').datepicker();

            $('#btnSearch').click(function () {
                btn_Search_Click();
            });
         
            $('#btnClear').click(function () {
                $('#<%=btnClear1.ClientID%>').click();
                
            });

            from_date = $('#<%=txtFromDate.ClientID%>');
            to_date = $('#<%=txtToDate.ClientID%>');

            from_date.val  ('<%=DateTime.Now.ToString("dd/MM/yyyy")%>');
           to_date.val ('<%=DateTime.Now.ToString("dd/MM/yyyy")%>');
        });

        function btn_Search_Click()
        {
            if (from_date.val() == '') {
                alert('Please Select From Date');
                return false;
            }
            else if (to_date.val() == '') {
                alert('Please Select To Date');
                return false;
            }
            else {
                var btnSearch = $('#<%=btnSearch1.ClientID%>');
                btnSearch.click();
            }
        }
        function loading() {
            $("#modal").show();
            $("#fade").show();
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
     <br />
    <br />
    <br />

    <div id="fade"></div>
    <div id="modal">
        <img id="loader" src="../../App_Themes/functions/loading.gif" alt="" />
        <span>Loading...</span>
    </div>
    <div id="div_main" runat="server" class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Message In</h3>
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
                    <td style="padding-left:20px;">
                        <input type="button" id="btnSearch" class="btn  btn-primary" value="Search" />
                        
                        <asp:Button ID="btnSearch1" runat="server" style="display:none;" OnClick="btnSearch1_Click" OnClientClick="loading();" />
                        
                    </td>
                    <td style="padding-left:20px;">
                        <asp:Button ID="btnExport" runat="server"  class="btn  btn-primary"  text="Export To Excel" style="display:none;"  OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
            <div id="div_result" runat="server"></div>
        </div>
    </div>
</asp:Content>

