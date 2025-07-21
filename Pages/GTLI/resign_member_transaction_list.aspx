<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="resign_member_transaction_list.aspx.cs" Inherits="Pages_GTLI_resign_member_transaction_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />

    <%--Auto Complete--%>
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <script src="../../Scripts/ui/jquery.ui.datepicker.js"></script>
    <%--End Auto Complete--%>

    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>        

    <ul class="toolbar">
        <li>
            <!-- Button Search-->
            <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>
    </ul>
     <style>
         .ui-autocomplete {
            z-index: 5000;
        }
    </style>
    <script type="text/javascript">
        PageMethods.GetCompanyName(function (results) {

            $("#txtCompanyName").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(results, function (item) {
                        return matcher.test(item);
                    }));
                }
            });
        });

        //Date picker
        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

        //Show Search Form
        function ShowSearchForm() {
            $("#txtCompanyName").val("");
            $("#Main_txtFrom").val("");
            $("#Main_txtTo").val("");

            $("#myModalSearchResignList").modal("show");
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
            <h3 class="panel-title">Resigned Members List</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <asp:Label ID="lblCount" runat="server"></asp:Label>
            <br />
            <div>
                <asp:GridView ID="gvGTLI" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="GTLI_Premium_ID" BorderColor="#B9B9B9" AllowPaging="True" PageSize="30" OnPageIndexChanged="gvGTLI_PageIndexChanged" OnPageIndexChanging="gvGTLI_PageIndexChanging" OnRowCommand="gvGTLI_RowCommand" OnRowDataBound="gvGTLI_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />

                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number" SortExpression="Policy_Number" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="110px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" SortExpression="Effective_Date" DataFormatString="{0:d-MMM-yyyy}" HtmlEncode="false" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="80">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Expiry Date">
                            <ItemTemplate>
                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Eval("Expiry_Date", "{0:dd-MMM-yyyy}")%>'></asp:Label>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center" />

                            <ItemStyle Width="100px" HorizontalAlign="Center" />

                        </asp:TemplateField>
                        <asp:BoundField DataField="Sum_Insured" HeaderText="Total Sum Insured" SortExpression="Sum_Insured" DataFormatString="{0:C0}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="120" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Life_Premium" HeaderText="Life Return Premium" SortExpression="Life_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="120" />
                        </asp:BoundField>
                          <asp:BoundField DataField="Accidental_100Plus_Premium" HeaderText="100Plus Return Premium" SortExpression="Accidental_100Plus_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="120" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TPD_Premium" HeaderText="TPD Return Premium" SortExpression="TPD_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="120" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DHC_Premium" HeaderText="DHC Return Premium" SortExpression="DHC_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="120" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Company_Name" HeaderText="Customer" SortExpression="Company_Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="300px"></ItemStyle>
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Detail">
                            <ItemTemplate>
                                <asp:LinkButton ID="ViewDetailLink" runat="server" CommandArgument='<%# Eval("GTLI_Premium_ID")%>'
                                    CommandName="DetailSale">View</asp:LinkButton>
                                <asp:HiddenField ID="hdfTransactionType" Value='<%# Eval("Transaction_Type")%>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hdfCurrentSearchOption" runat="server" />
            </div>
            <br />
            <br />
            <div style="text-align: right; width: 100%">
                <asp:Button ID="btnTotalResign" runat="server" Text="View Total Resigned Member" Height="35px" Visible="False" OnClick="btnTotalResign_Click" />
            </div>
        </div>
    </div>

    <%--Modal Search Resign List Form --%>
    <div id="myModalSearchResignList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchResignListHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Resign List Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
           <table>
                <tr>
                    <td style="width: 70px">Company:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCompanyName" Width="97%" runat="server" ValidationGroup="1" ClientIDMode="Static" ></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td >From:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server" ValidationGroup="3" Width="200px" CssClass="datepicker" onkeypress="return false;"></asp:TextBox>
               
                    </td>
                    <td style="width: 50px">To:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server" ValidationGroup="3" Width="200px" CssClass="datepicker" onkeypress="return false;"></asp:TextBox>
                
                    </td>
                </tr>              
                <tr>
                    <td>
                        Type:</td>
           
                    <td colspan="3">
                        <asp:DropDownList ID="ddlDateType" runat="server" Width="200px">
                            <asp:ListItem Selected="True" Value="0">Effective Date</asp:ListItem>
                             <asp:ListItem  Value="1">Transaction Date</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>           
         
        </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search"  ValidationGroup="3" OnClick="btnSearch_Click"  />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Resign List Search--%>

</asp:Content>

