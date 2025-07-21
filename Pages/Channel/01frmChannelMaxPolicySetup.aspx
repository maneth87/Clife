<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="01frmChannelMaxPolicySetup.aspx.cs" Inherits="Pages_Channel_01frmChannelMaxPolicySetup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <style>
        .tip {
            color:red;
            font-style:italic;
            font-weight:normal;
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
                    <h2 class="panel-title">MAXIMUM POLICY SETUP</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH CHANNEL SETUP</h3>
                        </div>
                        <div class="panel-body">
                            <table>
                            
                                <tr>

                                    <td>Channel Name: 
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlChannelNameSearch" runat="server"></asp:DropDownList>
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
                            <h3 class="panel-title">CHANNEL SETUP LIST</h3>
                        </div>

                        <div class="panel-body">
                            <asp:GridView ID="gvParam" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowEditing="gvParam_RowEditing" PageSize="10" AllowPaging="true" OnPageIndexChanging="gvParam_PageIndexChanging" AlternatingRowStyle-BackColor = "#C2D69B">
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" Width="80px" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="CHANNEL ITEM ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelItemId" Width="80px" runat="server" Text='<%#Eval("ChannelItemId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="CHANNEL NAME" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" Width="150px" runat="server" Text='<%#Eval("ChannelItemName") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="PRODUCT ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductId" Width="100px" runat="server" Text='<%# Eval("ProductId")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MAX POLICY PER LIFE" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaxPolNo" Width="60px" runat="server" Text='<%#Eval("MaxPolicyPerLife") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="STATUS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Width="100px" runat="server" Text='<%# Eval("Status").ToString()=="1"? "ACTIVE":"INACTIVE"  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="REMARKS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" Width="200px" runat="server" Text='<%# Eval("Remarks")  %>'></asp:Label>
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
                                    <td> Channel Name:<span style="color: red; font-weight: bold;">*</span></td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlChannelName" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelName_SelectedIndexChanged" >

                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Product Id: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProduct" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>Max Policy Per Life:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxPolNo" runat="server"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                               
                               <tr>
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
                                  <td colspan="3">
                                      <asp:TextBox ID="txtRemarks" runat="server" style="width:97%;"></asp:TextBox>
                                  </td>
                              </tr>
                                <tr>
                                    <td colspan="4" style="vertical-align: central; text-align: center;">
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
