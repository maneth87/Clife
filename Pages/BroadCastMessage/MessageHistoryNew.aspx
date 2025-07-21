<%@ Page Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="MessageHistoryNew.aspx.cs" Inherits="Pages_BroadCastMessage_MessageHistoryNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.js"></script>
    <script src="../../Scripts/jquery.battatech.excelexport.min.js"></script>
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

            //$('#btnSearch').click(function () {
            //    btn_Search_Click();
            //});

            $('#btnClear').click(function () {
                window.location.replace('MessageHistoryNew.aspx');
            });

            $('#btnResend').css('display', 'none');
            $('#btnExport').css('display', 'none');
            $('#btnExport_Fail').css('display', 'none');
            $('#btnExport_Success').css('display', 'none');


        });

        function plConfirm()
        {
            //if (!confirm('Do you want send fail messages?')) {
            //    return false;
            //}
            //else {
            //    loading();
            //}
            loading();
        }

        function loading() {
            $("#modal").show();
            $("#fade").show();
        }
        function clear_loading() {
            $("#modal").hide();
            $("#fade").hide();
        }

        //Function Export report to excel
        function ExportExcel() {

            $("#Success2").btechco_excelexport({
                containerid: "Success2"
               , datatype: $datatype.Table
            });
        }

        function ExportExcelFail() {

            $("#Fail").btechco_excelexport({
                containerid: "Fail"
               , datatype: $datatype.Table
            });
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
            <table id="tblContent" class="table-layout" style="background-color: #f6f6f6; padding: 10px;">
                <tr>
                    <td style="padding-left: 10px; padding-top: 10px; vertical-align: middle;">
                        <asp:Label runat="server" ID="lblFromDate" Text="From Date"></asp:Label><span style="color: red;">*</span>
                    </td>
                    <td style="padding-top: 10px; vertical-align: middle;">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" datepicker"></asp:TextBox>
                    </td>
                    <td style="padding-top: 10px; vertical-align: middle;">
                        <asp:Label runat="server" ID="lblToDate" Text="To Date"></asp:Label><span style="color: red;">*</span>
                    </td>
                    <td style="padding-top: 10px; vertical-align: middle;">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass=" datepicker"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td style="text-align: left; vertical-align: middle; width: 100px; padding-left: 10px;">
                        <asp:Label runat="server" ID="lblMessageCate" Text="Message Cate."></asp:Label>
                    </td>
                    <td style="vertical-align: middle;">
                        <asp:DropDownList runat="server" ID="ddlMessageCate" AppendDataBoundItems="true" DataSourceID="SqlMessageCate" DataTextField="cate" DataValueField="cate">
                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td style="vertical-align: middle;">
                        <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                    </td>
                    <td style="vertical-align: middle;">
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Success" Value="Success"></asp:ListItem>
                            <asp:ListItem Text="Fail" Value="Fail"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding-left: 10px; padding-bottom: 10px; vertical-align: middle;">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn-small btn-primary" Text="Search" OnClick="btnSearch_Click" OnClientClick="loading();" Style="border-radius: 10px;" />
                        <asp:Button ID="btnExport_Success" runat="server" CssClass="btn-small btn-primary" Text="Export Excel" Visible="false" Style="border-radius: 10px;" Enabled="true"  OnClick="btnExport_Success_Click"  />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding: 10px;">
                        <div id="dvError" runat="server" style="color: red; vertical-align: middle"></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding-left: 10px;">
                        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                            <li class="active"><a href="#Success" data-toggle="tab" style="color: green;">Success</a></li>
                            <li><a href="#Fail" data-toggle="tab" style="color: red;">Fail</a></li>

                        </ul>
                        <div class="container">
                            <div class="tab-content">

                                <div id="Success" class="tab-pane active" style="padding-left: 10px;">

                                    
                                     
                                    <div id="dvSuccessRecord" runat="server" style="color: red"></div>                                    
                                    <div id="dvSuccessContent" runat="server"></div>
                                
                                </div>
                                <div id="Fail" class="tab-pane" style="padding-left: 10px;">
                                    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
                                    <asp:UpdatePanel runat="server" ID="up">
                                        <ContentTemplate>

                                           <%-- <asp:Button ID="Button1" runat="server" CssClass="btn-small btn-primary" Text="Export Excel" Visible="false" Style="border-radius: 10px;" Enabled="true"  OnClick="btnExport_Success_Click"  />--%>
                                            <asp:Button ID="btnResent" runat="server" CssClass="btn-small btn-primary" Text="Resent" Visible="false" Style="border-radius: 10px;" Enabled="false" OnClick="btnResent_Click" OnClientClick="plConfirm();" />
                                            
                                            <div id="dvFailRecord" runat="server" style="color: red"></div>
                                            <div>
                                                <asp:GridView ID="gvFail" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                                    <SelectedRowStyle BackColor="#EEFFAA" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Justify"  >
                                                            <HeaderTemplate >
                                                                
                                                                <asp:CheckBox ID="ckbAll" runat="server"  OnCheckedChanged="ckbAll_CheckedChanged" AutoPostBack="true" TextAlign="Left"  />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ckb" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval("No") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Phone Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Message" HeaderStyle-Width="60%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMessage" runat="server" Text='<%# Eval("MessageText") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sent DateTime" HeaderStyle-Width="12%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSentDateTime" runat="server" Text='<%# Eval("SendDateTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Message Cate" HeaderStyle-Width="12%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMessageCate" runat="server" Text='<%# Eval("MessageCate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>
                                            </div>
                                            <div>
                                                <asp:Label ID="lblIdForUpdate" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div id="dvFailContent" runat="server"></div>
                                        </ContentTemplate>

                                    </asp:UpdatePanel>

                                </div>

                            </div>

                        </div>
                    </td>
                </tr>

            </table>

        </div>
    </div>

   <%-- <asp:SqlDataSource ID="SqlMessageCate" runat="server" ConnectionString="<%$ ConnectionStrings:CellCardMessageDb %>" SelectCommand="EXEC SP_GET_CC_MESSAGE_CATE"></asp:SqlDataSource>--%>
     <asp:SqlDataSource ID="SqlMessageCate" runat="server"  SelectCommand="EXEC SP_GET_CC_MESSAGE_CATE"></asp:SqlDataSource>
</asp:Content>
