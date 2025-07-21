<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="resign_member.aspx.cs" Inherits="Pages_GTLI_resign_member" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

    <ul class="toolbar">
        <li>
            <!-- Button Search-->
             <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" /> 
        </li>
    </ul>
      

    <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>

    <style>
        .ui-autocomplete {
            z-index: 5000;
        }
    </style>

    <%--Javascript--%>
    <script type="text/javascript">
        //Company Name autocomplete
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

        //Show Search Form
        function ShowSearchForm() {
            $("#txtCompanyName").val("");

            $("#myModalSearchMasterList").modal("show");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
        <%-- Form Design Section--%>
    <br />
    <br />
    <br />

    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Resign Member</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>

            <asp:Label ID="lblCount" runat="server"></asp:Label>
            <br />
            <asp:GridView ID="gvGTLI" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="GTLI_Policy_ID" BorderColor="#B9B9B9" AllowPaging="True" PageSize="30" AllowSorting="True" OnPageIndexChanged="gvGTLI_PageIndexChanged" OnPageIndexChanging="gvGTLI_PageIndexChanging" OnRowCommand="gvGTLI_RowCommand" OnRowDataBound="gvGTLI_RowDataBound" OnSorting="gvGTLI_Sorting">

                <Columns>
                    <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="100">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="80">
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
                    <asp:BoundField DataField="Sum_Insured" HeaderText="Total Sum Insured" DataFormatString="{0:C0}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" Width="130" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Life_Premium" HeaderText="Total Life Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" Width="130" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Accidental_100Plus_Premium" HeaderText="Total 100Plus Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" Width="130" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TPD_Premium" HeaderText="Total TPD Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" Width="130" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DHC_Premium" HeaderText="Total DHC Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" Width="130" />
                    </asp:BoundField>

                    <asp:BoundField DataField="GTLI_Company" HeaderText="Customer" SortExpression="Company_Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="300">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                        <ItemStyle Width="300px"></ItemStyle>
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="ViewDetailLink" runat="server" CommandArgument='<%# Eval("GTLI_Policy_ID")%>'
                                CommandName="Resign">Resign</asp:LinkButton>
                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hdfCurrentSearchOption" runat="server" />
        </div>
    </div>

       <%--Modal Search Master List Form --%>
    <div id="myModalSearchMasterList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchMasterListHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
           <table width="100%">
                <tr>
                    <td width="60px" >Company:</td>
                    <td width="87%">
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="96%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static" MaxLength="255"></asp:TextBox>
                
                    </td>  
                  
                </tr>       
            </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search"  ValidationGroup="3" OnClick="btnSearch_Click"   />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Master List Search--%>
</asp:Content>

