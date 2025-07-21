<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="manage_system_objects.aspx.cs" Inherits="Pages_Admin_manage_system_objects" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    
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
                    <h3 class="panel-title">Manage System Objects</h3>
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
                                <td>Module: <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtModule" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfObjectId" runat="server" />
                                </td>
                                <td>Object Code: <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObjectCode" runat="server"></asp:TextBox>
                                </td>
                                <td>Object Name: <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObjectName" runat="server"></asp:TextBox>
                                </td>
                              <td>
                                  Path:<span style="color: red;">*</span>
                              </td>
                                <td>
                                    <asp:TextBox ID="txtPath" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                  <td>Is Active: <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlActive" runat="server">
                                        <asp:ListItem Text="YES" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td style="vertical-align: middle; float: right;">
                                    <asp:Button ID="btnAdd" Text="GO" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                </td>
                            </tr>

                        </table>
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">System Objects List
                <asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label></h3>
                        </div>

                        <div class="panel-body">
                            <asp:GridView ID="gv_pages" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center" OnRowEditing="gv_pages_RowEditing">
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectId" runat="server" Text='<%#Eval("ObjId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Module" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModule" runat="server" Text='<%#Eval("Module") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object code" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectCode" runat="server" Text='<%#Eval("ObjCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectName" runat="server" Text='<%#Eval("ObjName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Path" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPath" runat="server" Text='<%#Eval("Path") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsActive">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsActive" runat="server" Text='<%# Eval("IsActive").ToString()=="1"?"YES":"NO"  %>'></asp:Label>
                                            <asp:HiddenField ID="hdfIsActive" runat="server" Value='<%# Eval("IsActive") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="~/App_Themes/functions/edit.png" CommandName="edit" />
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

