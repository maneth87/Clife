<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="05frmChannelLocationAgentMapping.aspx.cs" Inherits="Pages_Channel_05frmChannelLocationAgentMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <style>
        .tip {
            color: red;
            font-style: italic;
            font-weight: normal;
        }

       .CheckboxList input {
            float: left;
            clear: both;
        }
    </style>
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
                    <h2 class="panel-title">CHANNEL LOCATION AGENT MAPPING</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH AGENT MAPPING</h3>
                        </div>
                        <div class="panel-body">
                            <table>

                                <tr>

                                    <td>User Name: 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSearchInfo" runat="server" placeholder="User Name"></asp:TextBox>

                                    </td>


                                    <td style="vertical-align: middle; float: right;">
                                        <asp:Button ID="btnSearch" Text="SEARCH" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>


                            </table>

                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div style="float: right; position: static; padding-right: 15px; margin-top: 10px;">
                            <asp:LinkButton ID="ibtnAdd" runat="server" Text="ADD NEW" OnClick="ibtnAdd_Click"></asp:LinkButton>
                        </div>
                        <div class="panel-heading">
                            <h3 class="panel-title">AGENT MAPPING LIST</h3>
                        </div>

                        <div class="panel-body" id="divList" runat="server" style="overflow-x: scroll;">
                            <asp:GridView ID="gvParam" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowEditing="gvParam_RowEditing" PageSize="10" AllowPaging="true" OnPageIndexChanging="gvParam_PageIndexChanging" AlternatingRowStyle-BackColor="#C2D69B">
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" Width="120px" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CAHNNEL NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelItemName" Width="120px" runat="server" Text='<%# Eval("ChannelItemName")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CAHNNEL NAME" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelItemId" Width="150px" runat="server" Text='<%# Eval("ChannelItemId")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CHANNEL LOCATION ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelLocationId" Width="80px" runat="server" Text='<%#Eval("ChannelLocationId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="BRANCH CODE" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficeCode" Width="80px" runat="server" Text='<%#Eval("OfficeCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="BRANCH NAME" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficeName" Width="300px" runat="server" Text='<%#Eval("OfficeName") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AGENT NAME" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgentName" Width="180px" runat="server" Text='<%#Eval("SaleAgentName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="AGENT CODE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgentCode" Width="50px" runat="server" Text='<%# Eval("SaleAgentId")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="USER NAME" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" Width="200px" runat="server" Text='<%# Eval("UserName")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PRODUCT ID" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductId" Width="200px" runat="server" Text='<%# Eval("ProductId")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="STATUS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Width="80px" runat="server" Text='<%# Eval("Status").ToString()=="1"? "ACTIVE":"INACTIVE"  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="REMARKS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" Width="250px" runat="server" Text='<%# Eval("Remarks")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px">
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

                        <div class="panel-body" id="divAddContent">

                            <table id="tblAddContent">
                                <tr>
                                    <td>Channel Name:<span style="color: red; font-weight: bold;">*</span></td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlChannelName" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top;">Branch:<span style="color: red; font-weight: bold;">*</span></td>
                                    <td colspan="5" style="border:1px solid gray; border-radius:10px;">
                                      <%--  <asp:DropDownList ID="ddlBranch" Width="100%" runat="server">
                                        </asp:DropDownList>--%>
                                         <asp:CheckBoxList ID="cblBranch" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" CssClass="CheckboxList" Width="100%" CellSpacing="4" AutoPostBack="true" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"></asp:CheckBoxList>

                                    </td>
                                </tr>
                                  <tr>
                                    <td style="vertical-align:top;">Product:<span style="color: red; font-weight: bold;">*</span></td>
                                    <td colspan="5" style="border:1px solid gray; border-radius:10px;">
                                     
                                         <asp:CheckBoxList ID="cblProduct" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" CssClass="CheckboxList" Width="100%" CellSpacing="4" AutoPostBack="true" ></asp:CheckBoxList>

                                    </td>
                                </tr>
                                <tr>
                                    <td>User Name: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Agent Code:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgentCode" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Status<span style="color: red; font-weight: bold;">*</span></td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                            <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                            <asp:ListItem Text="ACTIVE" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="INACTIVE" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Remarks:</td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtRemarks" Width="97%" runat="server">

                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="vertical-align: central; text-align: center;">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="SAVE" />
                                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" OnClick="btnDelete_Click" Text="DELETE" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="CANCEL" />
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