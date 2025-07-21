<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="reconcile.aspx.cs" Inherits="Pages_GTLI_reconcile" %>

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

    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <ul class="toolbar">
      
        <li>
            <!-- Button Search-->
            <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />      
        </li>
        <li>
            <!-- Button View-->
            <input type="button"  onclick="ViewDeails();" style="background: url('../../App_Themes/functions/view.png') no-repeat; border: none; height: 40px; width: 90px;" /> 
            <div style="display:none">
                <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" />
            </div>
        </li>
        <li>
            <input type="button"  onclick="Reconcile();" style="background: url('../../App_Themes/functions/approve.png') no-repeat; border: none; height: 40px; width: 90px;" />      
            <div style="display:none">
                <asp:Button ID="btnReconcile" runat="server" OnClick="btnReconcile_Click" />
            </div>
        </li>

    </ul>
    <style>
         .ui-autocomplete {
            z-index: 5000;
        }
        .IndentBottom {
            padding-bottom:5px;
        }
    </style>
    <script type="text/javascript">

        //Section date picker
        $(document).ready(function () {

            $('.datepicker').datepicker();

        });

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

            $("#myModalSearchPolicyList").modal("show");
        }

        //View Reconcile Details
        function ViewDeails() {
            var btnView = document.getElementById("<%= btnView.ClientID %>");
            btnView.click();
        }
       
        //Reconcile
        function Reconcile() {
            if (confirm('Are you sure, you want to reconcile?')) {
                var btnSave = document.getElementById("<%= btnReconcile.ClientID %>");
                btnSave.click();
            }
        }

        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");         

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e5e5e5");

               

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

            }
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
            <h3 class="panel-title">Reconcile</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <asp:Label ID="lblCount" runat="server"></asp:Label>
            <br />
            <div>
                <asp:GridView Width="100%" ID="gvGTLIPolicy" runat="server" AutoGenerateColumns="False" DataKeyNames="GTLI_Premium_ID" BorderColor="#B9B9B9" AllowPaging="false" OnRowDataBound="gvGTLIPolicy_RowDataBound" OnRowCommand="gvGTLIPolicy_RowCommand"  >
                    <Columns>
                     <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="IndentBottom" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server"  onclick='<%# "SelectSingleCheckBox(this);" %>'/>
                            
                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" HorizontalAlign="Center" CssClass="IndentBottom"/>
                      </asp:TemplateField>                      
                     
                        <asp:BoundField DataField="Transaction_Staff_Number" HeaderText="Employees" SortExpression="Transaction_Staff_Number" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="70">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="70px" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number" SortExpression="Policy_Number" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="130">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="130px"></ItemStyle>
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
                        <asp:BoundField DataField="Sum_Insured" HeaderText="Sum Assured" SortExpression="Sum_Insured" DataFormatString="{0:C0}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="130" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Life_Premium" HeaderText="Life Premium" SortExpression="Life_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Accidental_100Plus_Premium" HeaderText="100Plus Premium" SortExpression="Accidental_100Plus_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                         <asp:BoundField DataField="Accidental_100Plus_Premium" HeaderText="100Plus Premium" SortExpression="Accidental_100Plus_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TPD_Premium" HeaderText="TPD Premium" SortExpression="TPD_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DHC_Premium" HeaderText="DHC Premium" SortExpression="DHC_Premium" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" Width="110" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Policy_Year" HeaderText="Policy Year"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="130">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="130px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        </asp:BoundField>
                        
                         <asp:BoundField DataField="Transaction_Type" HeaderText="Transaction Type"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="130">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Width="130px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
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
              
                <br />
                <br />

            </div>
        </div>
    </div>

     <%--Modal Search Policy List Form --%>
    <div id="myModalSearchPolicyList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchPolicyListHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Policy List Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
           <table width="100%">
                <tr>
                    <td width="60px" >Company:</td>
                    <td width="40%">
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="85%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static" MaxLength="255"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*" ForeColor="Red" ControlToValidate="txtCompanyName" ValidationGroup="3" ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>         
                    <td width="60px">To Date:</td>             
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="85%" CssClass="datepicker" onkeypress="return false;" Font-Size="10pt" TabIndex="2" MaxLength="25"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ForeColor="Red" ControlToValidate="txtToDate" ValidationGroup="3" ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>    
                    <td></td>  
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" Visible="false">
                            <asp:ListItem Text="Add Member" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Resign Member" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>       
            </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search"  ValidationGroup="3" OnClick="btnSearch_Click"    />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Policy List Search--%>

    
    <asp:HiddenField ID="hdfPremiumID" runat="server" />
    <asp:HiddenField ID="hdfTransactionThpe" runat="server" />
</asp:Content>

