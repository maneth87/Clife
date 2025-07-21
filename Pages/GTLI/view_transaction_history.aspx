<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="view_transaction_history.aspx.cs" Inherits="Pages_GTLI_view_transaction_history" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

    <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>

    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--print--%>
    <script src="../../Scripts/jquery.print.js"></script>

    <ul class="toolbar">
        <li>
            <!-- Button to trigger modal new business form -->
            <input type="button" onclick="PrintHistoryList();"  style="background: url('../../App_Themes/functions/print.png') no-repeat; border: none; height: 40px; width: 90px;" />

        </li>
        <li>
            <!-- Button Search-->
           <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>
    </ul>
      <style>
        @media print {
            @page {
                size: A4;
                margin: 5px;
                /*font-family: Arial;*/
                font-size: 8pt;
                width: 210mm;
                height: 297mm;
            }

            html {
                margin: 10px;
            }

            body {
                font-family: Arial;
                font-size: 8pt;
            }

            .hideelement {
                visibility: hidden;
            }

        }

         .ui-autocomplete {
                z-index: 5000;
            }
    </style>

    <script type="text/javascript">
        //Date picker
        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        //Company Name autocomplete
        PageMethods.GetCompanyName(function (results) {

            $("#txtCompany").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(results, function (item) {
                        return matcher.test(item);
                    }));
                }
            });
        });

        //Print
        function PrintHistoryList() {

            $(".printable").print();
        }

        //Show Search Form
        function ShowSearchForm() {
            $("#txtCompanyName").val("");
            $("#Main_txtFrom").val("");
            $("#Main_txtTo").val("");

            $("#myModalSearchTransactionHistoryList").modal("show");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
 
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Transaction History</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <div class="printable" align="center">

                <table width="100%" style="font-weight: bold; font-size: 16px;">
                    <tr>
                        <td style="text-align: center">
                            
                                <span style="font-weight: bold">
                                    <asp:Label ID="lblCombodianLife" runat="server" Text="Cambodia Life Insurance PLC."></asp:Label></span>
                                <br />
                                <span style="font-weight: bold">
                                    <asp:Label ID="lblRortTitle" runat="server" Text="Group Term Life: Transaction History"></asp:Label></span>
                        </td>
                    </tr>

                </table>
                <asp:Label ID="lblCount" runat="server" CssClass="hideelement"></asp:Label>
                <asp:GridView ID="gvHistory" runat="server" HeaderStyle-Font-Size="8pt" AutoGenerateColumns="False" DataKeyNames="GTLI_Premium_ID" AllowSorting="True" ItemStyle-HorizontalAlign="Center" Width="100%" OnPageIndexChanged="gvHistory_PageIndexChanged" OnPageIndexChanging="gvHistory_PageIndexChanging" OnRowCommand="gvHistory_RowCommand" OnRowDataBound="gvHistory_RowDataBound" OnSorting="gvHistory_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-Width="50px">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />

                        </asp:TemplateField>

                        <asp:BoundField DataField="Created_On" HeaderText="Created Date" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="90px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="GTLI_plan" HeaderText="Plan" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" SortExpression="Effective_Date" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="90px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Expiry_Date" HeaderText="Expiry Date" SortExpression="Expiry_Date" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="90px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Transaction_Staff_Number" HeaderText="Employee Number" SortExpression="Transaction_Staff_Number" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Transaction_Type" HeaderText="Transaction Type">
                            <ItemStyle Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sum_insured" SortExpression="Sum_Insured" DataFormatString="{0:C0}" HeaderText="Sum Insured">
                            <ItemStyle HorizontalAlign="Right" Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Life_Premium" DataFormatString="{0:C2}" HeaderText="Total Life Premium">
                            <ItemStyle HorizontalAlign="Right" Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Accidental_100Plus_Premium" DataFormatString="{0:C2}" HeaderText="Total 100Plus Premium">
                            <ItemStyle HorizontalAlign="Right" Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TPD_Premium" DataFormatString="{0:C2}" HeaderText="Total TPD Premum">
                            <ItemStyle HorizontalAlign="Right" Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DHC_Premium" DataFormatString="{0:C2}" HeaderText="Total DHC Premium">
                            <ItemStyle HorizontalAlign="Right" Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Total_Premium" SortExpression="Total_Premium" DataFormatString="{0:C2}" HeaderText="Total Premium">
                            <ItemStyle HorizontalAlign="Right" Font-Names="Arial" Font-Size="8pt" Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField  HeaderText="Detail" ItemStyle-CssClass="hideelement" HeaderStyle-CssClass="hideelement" ControlStyle-CssClass="hideelement" FooterStyle-CssClass="hideelement">
                            <ItemTemplate>
                                <asp:LinkButton ID="ViewAddNewDetailLink" runat="server" CommandArgument='<%# Eval("GTLI_Premium_ID")%>'
                                    CommandName="AddNewDetailSale">View</asp:LinkButton>

                                <asp:LinkButton ID="ViewResignDetailLink" runat="server" CommandArgument='<%# Eval("GTLI_Premium_ID")%>'
                                    CommandName="ResignDetailSale">View</asp:LinkButton>
                                <asp:LinkButton ID="ViewCreatePolicyLink" runat="server" CommandArgument='<%# Eval("GTLI_Premium_ID")%>'
                                    CommandName="CreatePolicyDetailSale">View</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />

                            <ControlStyle CssClass="hideelement"></ControlStyle>

                            <FooterStyle CssClass="hideelement"></FooterStyle>

                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>

                    </Columns>

                    <HeaderStyle CssClass="header_color" Font-Size="8pt" />

                </asp:GridView>
            </div>
            <br />
            <br />
            <div style="text-align: right; width: 100%">
                <asp:Button ID="btnTotalHistory" runat="server" Text="View Total History" Height="35px" OnClick="btnTotalHistory_Click" />
            </div>
        </div>
    </div>

    <%--Modal Search Transaction History List Form --%>
    <div id="myModalSearchTransactionHistoryList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchTransactionHistoryListHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Transaction History List Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
             <table width="100%">
                <tr>
                    <td width="9%">Company:
                    </td>
                    <td width="81%" colspan="3">
                        <asp:TextBox ID="txtCompany" runat="server" Width="95.8%" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td >From:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server" Width="91%" CssClass="datepicker" onkeypress="return false;"></asp:TextBox>
                    </td>
                    <td >To:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server" Width="91%" CssClass="datepicker" onkeypress="return false;"></asp:TextBox>
                    </td>                    
                </tr>
      
            </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search"  ValidationGroup="3" OnClick="btnSearch_Click"  />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Transaction History List  Search--%>
</asp:Content>

