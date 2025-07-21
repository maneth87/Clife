<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="view_registration_doc.aspx.cs" Inherits="Pages_Documents_view_registration_doc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <style>
        .CheckboxList input {
            float: left;
            clear: both;
        }

        .GridPager a, .GridPager span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background-color: #21275b; /*#A1DCF2;*/
            color: white; /*#000;*/
            border: 1px solid #3AC0F2;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div id="dv_main" runat="server" class="panel panel-default">
                <div class="panel panel-default" id="dvSearch" runat="server">

                    <div class="panel-heading">
                        <h3 class="panel-title">Search Registration Documents</h3>
                    </div>
                    <div class="panel-body">
                        <table>
                            <tr>
                                <td>
                                    <table>

                                        <tr>

                                            <td>Upload Date From :
                                            </td>
                                            <td>

                                                <asp:TextBox ID="txtFromDate" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>

                                            </td>
                                            <td>To :                                      
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDate" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td style="padding-right: 20px;">
                                                <asp:Label ID="lblDescription" runat="server" Text="File Description :"></asp:Label>
                                            </td>
                                            <td style="padding-right: 20px;" colspan="2">
                                                <asp:TextBox ID="txtFileDescription" runat="server" CssClass="span6"></asp:TextBox>
                                            </td>
                                           <td>
                                               <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                           </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="panel panel-default" id="dv_grid" runat="server">
                    <div class="panel-heading">

                        <h3 class="panel-title">Searching Result
                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                        </h3>

                    </div>
                    <asp:GridView ID="gv_view" CssClass="grid-layout" AllowPaging="true" PageSize="25" runat="server" AutoGenerateColumns="false" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                        OnRowCommand="gv_view_RowCommand" OnPageIndexChanging="gv_view_PageIndexChanging">
                        <SelectedRowStyle BackColor="#EEFFAA" />

                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Application ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblapplicationID" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocName" runat="server" Text='<%#Eval("DocName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Path" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocPath" runat="server" Text='<%#Eval("DocPath") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Description" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocDescription" runat="server" Text='<%#Eval("DocDescription") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Uploaded By" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblUploadedBy" runat="server" Text='<%#Eval("UploadedBy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Uploaded On" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblUploadedOn" runat="server" Text='<%#Eval("UploadedOn")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblPreview" Text="View" Style="color: blue; padding: 2px 2px 2px 2px; font-weight: 800" runat="server" CommandName="CMD_PREVIEW" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
                   
                              
            </div>
            </div>     

            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
        <%--    <asp:PostBackTrigger ControlID="btnUpdate" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>



