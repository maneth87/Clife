<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frm_sys_paramaters.aspx.cs" Inherits="Pages_Admin_frm_sys_paramaters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
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

                <div class="panel-heading">
                    <h2 class="panel-title">SYSTEM PARAMATERS</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH PARAMATERS</h3>
                        </div>
                        <div class="panel-body">
                            <table>

                                <tr>

                                    <td>Paramater Name: 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtParamaterNameSearch" runat="server"></asp:TextBox>
                                    </td>


                                    <td style="vertical-align: middle; float: right;">
                                        <asp:Button ID="btnSearch" Text="SEARCH" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>


                            </table>

                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div style="float:right; position:static; padding-right:15px; margin-top:10px;">
                            <asp:LinkButton ID="ibtnAdd" runat="server" Text="ADD NEW" OnClick="ibtnAdd_Click"></asp:LinkButton>
                        </div>
                        <div class="panel-heading">
                            <h3 class="panel-title">PARAMATER LIST
                <asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label>

                            </h3>

                        </div>

                        <div class="panel-body">
                            <asp:GridView ID="gvParam" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowEditing="gvParam_RowEditing" >
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NO" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NAME" Visible="true" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblParamaterName" Width="300px" runat="server" Text='<%#Eval("ParamaterName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VALUE" Visible="true" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblParamaterVal"  Width="200px" runat="server" Text='<%#Eval("ParamaterVal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DESCRIPTION" Visible="true" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblParamaterDesc"  Width="300px" runat="server" Text='<%#Eval("ParamaterDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ACTIVE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsActive" runat="server" Text='<%# Eval("IsActive").ToString()=="True"?"YES":"NO"  %>'></asp:Label>
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

                    <div class="panel panel-default" id="divAdd" runat="server">

                        <div class="panel-heading">
                            <h3 class="panel-title">ADD NEW / UPDATE </h3>

                        </div>

                        <div class="panel-body">
                           
                                <table>
                                    <tr>
                                        <td>ID: <span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIdAdd" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Paramater Name: <span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParamaterNameAdd" runat="server" style="text-transform:uppercase;"></asp:TextBox>
                                        </td>
                                        <td>Active?<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlActive" runat="server">
                                                <asp:ListItem Value="1" Text="YES" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="NO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Paramater Value:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtParamaterValueAdd" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Paramater Description:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtParamaterDesc" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>

                                        <td style="vertical-align: central; text-align: center;" colspan="6">
                                            <asp:Button ID="btnSave" Text="SAVE" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                             <asp:Button ID="btnDelete" Text="DELETE" runat="server" CssClass="btn btn-primary" OnClick="btnDelete_Click" />
                                             <asp:Button ID="btnCancel" Text="CANCEL" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>


                                </table>
                            
                        </div>

                    </div>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


