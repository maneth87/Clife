<%@ Page Title="Clife | Business => Reserve Policy Number" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="policy_number_reserve.aspx.cs" Inherits="Pages_Business_policy_number_reserve" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>
    <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
    <%--End Auto Complete--%>
    <style>
        .ui-autocomplete {
            z-index: 5000;
        }
    </style>

    <%--Javascript--%>
    <script type="text/javascript">

        //Validation Section
        //Validate integer
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }
        function successalert(message, event, type, title) {
            swal({
                title: title,
                text: message,
                type: type
            });
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
            <h3 class="panel-title">Credit Life Policy Number Reserve</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <table>
                <tr>
                    <td style="width:40%;">Application Number <span style="color:red;">*</span></td>
                    <td>
                        <asp:TextBox ID="txtApplicationNumber" runat="server" Width="96%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static" MaxLength="255" onkeyup="ValidateNumber(this);"></asp:TextBox>
                        <asp:HiddenField ID="hdfApplicationNumber" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width:100px;">Customer ID<span style="color:red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerID" runat="server" placeholder="XXXXXXXX"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:100px;">Policy Number<span style="color:red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPolicyNumber" runat="server" placeholder="XXXXX" onkeyup="ValidateNumber(this);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                   <td></td>
                    <td>
                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                           
                        <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="btn" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="btn" OnClick="btnClear_Click" />
                        
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:GridView ID="gvCL24" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="#B9B9B9" OnRowDataBound="gvCL24_RowDataBound">

                <Columns>
                    <asp:TemplateField HeaderText="Application Number">
                        <ItemTemplate>
                           <asp:Label ID="lblAppNumber" runat="server" Text='<%#Eval("App_Number") %>'>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Policy Number">
                        <ItemTemplate>
                           <asp:Label ID="lblPolicyNumber" runat="server" Text='<%#Eval("Policy_Number") %>'>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Customer ID">
                        <ItemTemplate>
                           <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("Customer_ID") %>'>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtEdit" runat="server" CommandArgument='<%# Eval("Reserve_Policy_ID")%>'
                                CommandName="Select"  OnClick="Edit_Click" style="height:auto"><i class="fa fa-pencil-square-o"></i>  Edit</asp:LinkButton>
                            <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%# Eval("Reserve_Policy_ID")%>'
                                CommandName="Remove" OnClick="Del_Click" OnClientClick="return confirm('Do you want to delete this selected record?');" style="height:auto"><i class="fa fa-trash"></i> Delete</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="45px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>

                </Columns>
                <HeaderStyle BackColor="#99ccff" ForeColor="#383434"></HeaderStyle>
            </asp:GridView>

            <asp:HiddenField ID="hdfCurrentSearchOption" runat="server" />
        </div>
    </div>


</asp:Content>
