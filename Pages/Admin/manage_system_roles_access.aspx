<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="manage_system_roles_access.aspx.cs" Inherits="Pages_Admin_manage_system_roles_access" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  


    <div id="div_main" runat="server" class="panel panel-default">
        <asp:UpdateProgress ID="upg" runat="server" AssociatedUpdatePanelID="UP">
            <ProgressTemplate>
                <div class="my_progress">
                    <div class="tr"></div>
                    <div class="main">
                        <div class="dhead">
                            <h2>PROCESSING</h2>
                        </div>
                        <p>
                            <img id="loader" src="../../App_Themes/images/loader.gif" alt="Progressing" />
                        </p>
                        <p>Please wait...</p>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                                    <div id="div_message" runat="server" style="text-align: center; vertical-align: middle; margin-top: 0px; margin-bottom: 5px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 3px 3px 0px 0px; height: 20px;"></div>

                <div class="panel-heading">
                    <h3 class="panel-title">Manage System Roles Access</h3>
                </div>
                <div class="panel-body">
                    <div>
                        <table>
                            <tr>
                                <td>Action :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                                        <asp:ListItem Text="Search" Value="SEARCH" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Add" Value="ADD"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Role Name: <span style="color: red;">*</span>
                                </td>
                                <td>

                                    <asp:DropDownList ID="ddlRoleName" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlRoleId_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>Role Id: <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRoleId" runat="server"></asp:TextBox>
                                </td>
                                <td>Module: <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlModule" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"></asp:DropDownList>
                                </td>

                                <td style="vertical-align: middle; float: right;">
                                    <asp:Button ID="btnAdd" Text="GO" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:GridView ID="gv_objects" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                        <Columns>

                                            <asp:TemplateField HeaderText="No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ObjectId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObjectId" runat="server" Text='<%#Eval("ObjId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Object Code" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObjCode" runat="server" Text='<%#Eval("ObjCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Object Name" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObjectName" runat="server" Text='<%#Eval("ObjName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsView" Visible="true">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbView" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsAdd" Visible="true">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbAdd" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsUpdate" Visible="true">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbUpdate" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsApprove" Visible="true">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbApprove" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsAdmin" Visible="true">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbAdmin" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>
                                </td>
                            </tr>

                        </table>
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">System Roles Access List
                <asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label></h3>
                        </div>

                        <div class="panel-body">
                            <asp:GridView ID="gv_pages" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowDataBound="gv_pages_RowDataBound">
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role Access Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoleAccessId" runat="server" Text='<%#Eval("RoleAccessId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role Id" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoleId" runat="server" Text='<%#Eval("RoleId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoleName" runat="server" Text='<%#Eval("RoleName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Module" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModule" runat="server" Text='<%#Eval("Module") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectId" runat="server" Text='<%#Eval("ObjectId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object Code" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectCode" runat="server" Text='<%#Eval("ObjectCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectName" runat="server" Text='<%#Eval("ObjectName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsView">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsView" runat="server" Text='<%# Eval("IsView").ToString()=="1"?"YES":"NO"  %>'></asp:Label>
                                            <asp:HiddenField ID="hdfIsView" runat="server" Value='<%# Eval("IsView") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsAdd">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsAdd" runat="server" Text='<%# Eval("IsAdd").ToString()=="1"?"YES":"NO"  %>'></asp:Label>
                                            <asp:HiddenField ID="hdfIsAdd" runat="server" Value='<%# Eval("IsAdd") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsUpdate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsUpdate" runat="server" Text='<%# Eval("IsUpdate").ToString()=="1"?"YES":"NO"  %>'></asp:Label>
                                            <asp:HiddenField ID="hdfIsUpdate" runat="server" Value='<%# Eval("IsUpdate") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsApprove">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsApprove" runat="server" Text='<%# Eval("IsApprove").ToString()=="1"?"YES":"NO"  %>'></asp:Label>
                                            <asp:HiddenField ID="hdfIsApprove" runat="server" Value='<%# Eval("IsApprove") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsAdmin">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsAdmin" runat="server" Text='<%# Eval("IsAdmin").ToString()=="1"?"YES":"NO"  %>'></asp:Label>
                                            <asp:HiddenField ID="hdfIsAdmin" runat="server" Value='<%# Eval("IsAdmin") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

                        </div>

                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

