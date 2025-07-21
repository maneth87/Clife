<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="add_role.aspx.cs" Inherits="Pages_Admin_add_role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Add New Role</h3>
        </div>
        <div class="panel-body">

            <table>
                <tr>
                    <td style="padding-left: 7px; width: auto">Role Name:&nbsp;<span style="color:red;">*</span></td>

                    <td style="width:auto; vertical-align: calc()">
                        <asp:TextBox ID="txtRoleName" placeholder="Role name ..." runat="server" Width="90%" ClientIDMode="Static" MaxLength="100"></asp:TextBox>
                    </td>
                   
                     <td style="width:auto;">Description:&nbsp;&nbsp;</td>
                    <td style="width: auto; vertical-align: calc()">
                         <asp:TextBox ID="txtDescription" placeholder="Description ..." runat="server" Width="90%" ClientIDMode="Static" MaxLength="100"></asp:TextBox>
                    </td>
                    
                    
                </tr>
                <tr>
                    
                    <td colspan="4" style="width: 42%; vertical-align: top;">
                        <asp:Button ID="btnAdd" runat="server" CssClass="btn" Style="border: 1pt groove #d5d5d5; Width: 100px" Text="Add" ValidationGroup="Add" OnClick="btnAdd_Click" ToolTip="Add Role" />
                        <asp:Button ID="btnClear" runat="server" CssClass="btn" Style="border: 1pt groove #d5d5d5; Width: 100px" Text="Clear" ValidationGroup="Clear" OnClick="btnClear_Click" ToolTip="Clear" />
                    </td>
                </tr>
            </table>

            <br />
            <asp:Label ID="resultSearch" runat="server" Text=""></asp:Label>
            <%-- Grid View --%>
            <asp:GridView ID="gvroles" PageSize="20"  CssClass="grid-layout; grid" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="Black" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" HorizontalAlign="Center" PagerStyle-HorizontalAlign="Center" OnSelectedIndexChanging="gvroles_SelectedIndexChanging" >

                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID ="ibntSelect" CommandName="select" ImageUrl="~/App_Themes/functions/active.png" />
                          
                            <asp:HiddenField ID="hdfOldRoleID" runat="server" Value='<%# Eval("RoleId")%>' />

                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" VerticalAlign="Middle" HorizontalAlign="Center" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Role Name">
                        <ItemTemplate>
                            <asp:Label ID="lblRoleName" runat="server" Text ='<%# Eval("rolename") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Lowered Role Name">
                        <ItemTemplate>
                            <asp:Label ID="lblLoweredRoleName" runat="server" Text ='<%# Eval("loweredrolename") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text ='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
                <RowStyle HorizontalAlign="Center" Height="25" />
                <SelectedRowStyle BackColor="#e0e0e0" />
            </asp:GridView>
            <br /><br />
        </div>
    </div>
</asp:Content>

