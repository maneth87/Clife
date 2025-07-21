<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmActivityLog.aspx.cs" Inherits="Pages_Setting_frmActivityLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <%-- Search in dropdownlist --%>
    <script src="../../Scripts/jquery.1.8.3.jquery.min.js"></script>
    <script src="../../Scripts/jquery.searchabledropdown-1.0.8.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <%--  <meta http-equiv="refresh" content="60" />--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    

    <ul class="toolbar">
    </ul>
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
    <asp:UpdatePanel ID="UP" runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Search Activity Log</h3>

                </div>
                <div class="panel-body">
                    <div style="border: 2px solid #21275b; border-radius: 15px; padding: 10px;">
                        <table style="width: auto;" border="0">

                            <tr>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblActivityDateF" runat="server" Text="Activity Date From:"></asp:Label><span class="star">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtDateF" CssClass="span2" runat="server" placeHolder="DD/MM/YYYY"></asp:TextBox>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="Label1" runat="server" Text="To:"></asp:Label><span class="star">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtDateT" CssClass="span2" runat="server" placeHolder="DD/MM/YYYY"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblUser" runat="server" Text="User Name:"></asp:Label>
                                </td>
                                <td colspan="3" style="padding-right: 20px;">
                                    <asp:TextBox ID="txtUserName" TextMode="MultiLine" runat="server" CssClass="span6" placeHolder="user1,user2,user3"></asp:TextBox>
                                    <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />

                                </td>
                               
                            </tr>

                        </table>
                    </div>
                </div>
                <div style="float: right; position: static; padding-right: 15px; margin-top: 10px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
                </div>
                <div class="panel-heading">

                    <h3 class="panel-title">Activity List  (<asp:Label ID="lblRecords" runat="server"></asp:Label>)</h3>

                </div>

                <div style="overflow-x: scroll; height: auto; width: 100%;" id="divList" runat="server">
                    <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="30" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                        OnPageIndexChanging="gv_valid_PageIndexChanging">
                        <SelectedRowStyle BackColor="#EEFFAA" />
                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />

                        <Columns>
                            <asp:TemplateField HeaderText="No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="GridRowCenter" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("LogId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Activity Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblActivityDate" runat="server" Text='<%# Eval("ActivityDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UserName" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Page Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblObjectName" runat="server" Text='<%#Eval("ObjectName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Page Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblObjectId" runat="server" Text='<%# Eval("ObjectId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:Label ID="lblActivity" runat="server" Text='<%# Eval("ActivityType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                           
                             <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ApplicationName" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationName" runat="server" Text='<%# Eval("ApplicationName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                           
                        </Columns>

                    </asp:GridView>
                </div>
                <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

