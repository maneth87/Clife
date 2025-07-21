<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="manage_system_user_role.aspx.cs" Inherits="Pages_Admin_manage_system_user_role" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />

    <script>
       

    </script>
    <style>
        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 250%;
            background-color: #ababab;
            z-index: 9999999;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=90);
        }

        #modal {
            display: none;
            position: absolute;
            top: 40%;
            left: 45%;
            width: 100px;
            height: 100px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 9999999;
            text-align: center;
            overflow: auto;
        }

        .CheckboxList input {
            float: left;
            clear: both;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="fade"></div>
    <div id="modal">
        <img id="loader" src="../../App_Themes/functions/loading.gif" alt="" /><br />
        <span>Saving data...</span>
    </div>

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
                    <h3 class="panel-title">Manage System Users Roles</h3>
                </div>
                <div class="panel-body">
                    <div>
                        <table style="width: 100%;">

                            <tr>

                                <td colspan="3" style="background-color: #21275b; color: white; border-left: 2px solid #21275b;">User Name: <span style="color: red;">*</span>
                                    <asp:TextBox runat="server" ID="txtUserName"></asp:TextBox>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                </td>
                                <td colspan="4" style="background-color: #21275b; color: white; border-left: 2px solid #21275b; border-right: 2px solid #21275b;">Roles: <span style="color: red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="vertical-align: top; border-left: 2px solid #21275b; border-bottom: 2px solid #21275b;">
                                    <asp:GridView ID="GvUserName" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="80%" HorizontalAlign="Center">
                                        <Columns>

                                            <asp:TemplateField HeaderText="No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User Name" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("Username") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Add">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbAdd" runat="server" AutoPostBack="true" OnCheckedChanged="ckbAdd_CheckedChanged" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>

                                </td>
                                <td colspan="4" style="vertical-align: top; border-left: 2px solid #21275b; border-right: 2px solid #21275b; border-bottom: 2px solid #21275b; text-align: center;">
                                    <asp:GridView ID="GvRole" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                        <Columns>

                                            <asp:TemplateField HeaderText="No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Role Id" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoleID" runat="server" Text='<%#Eval("RoleId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Role Name" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoleName" runat="server" Text='<%#Eval("RoleName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Add">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbAdd" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>
                                    <asp:Button ID="btnAdd" Text="GO" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                </td>
                            </tr>

                        </table>
                    </div>


                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
