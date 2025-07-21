<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="renew_policy.aspx.cs" Inherits="Pages_GTLI_renew_policy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
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

         
         //Show Search Form
         function ShowSearchForm() {
             $("#txtCompanyName").val("");

             $("#myModalSearchActiveMemberList").modal("show");
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
            <h3 class="panel-title">Renew Policy</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <asp:Label ID="lblCount" runat="server"></asp:Label>
            <br />
            <div>
                  <asp:GridView ID="gvActiveMember" runat="server" AutoGenerateColumns="False" DataKeyNames="GTLI_Certificate_ID,GTLI_Premium_ID" >
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="50px">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="GTLI_Plan" HeaderText="Plan">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="60px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Certificate_Number" HeaderText="Certificate No.">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Employee_Name" HeaderText="Employee Name">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="190px" />
                        </asp:BoundField>                       
                        <asp:BoundField DataField="Gender" HeaderText="Gender">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:dd-MM-yyyy}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Position" HeaderText="Position">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="200px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sum_Insured" HeaderText="Sum Insured" DataFormatString="{0:C0}">
                            <ItemStyle Font-Names="Arial" Font-Size="10pt" Width="120px" HorizontalAlign="right" />
                        </asp:BoundField>
                    
                        <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="IndentBottom" HeaderStyle-CssClass="IndentBottom">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk1" runat="server" onclick="Check_Click(this)" />
                                <asp:HiddenField ID="hdfEmpId" runat="server" Value='<%# Bind("GTLI_Certificate_ID")%>' />
                                <asp:HiddenField ID="hdfPremiumID" runat="server" Value='<%# Bind("GTLI_Premium_ID")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>                
                <br />
                <br />

            </div>
        </div>
    </div>

     <%--Modal Search Active Member List Form --%>
    <div id="myModalSearchCreatedList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchCreatedListHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Active Member List Search</h3>
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
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search"  ValidationGroup="3" OnClick="btnSearch_Click"    />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Active Member List Search--%>
</asp:Content>

