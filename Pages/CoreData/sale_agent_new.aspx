<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="sale_agent_new.aspx.cs" Inherits="Pages_CoreData_sale_agent_new" %>



<%@ Register assembly="System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI" tagprefix="cc1" %>



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
                    <h2 class="panel-title">SALE AGENT</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH SALE AGENT</h3>
                        </div>
                        <div class="panel-body">
                            <table>
                            
                                <tr>

                                    <td>Sale Agent Information: 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSaleAgentSearch" runat="server" placeholder="Full Name or Sale Agent ID"></asp:TextBox>
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
                            <h3 class="panel-title">SALE AGENT LIST</h3>
                        </div>

                        <div class="panel-body" id="divList" runat="server">
                            <asp:GridView ID="gvParam" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowEditing="gvParam_RowEditing" PageSize="10" AllowPaging="true" OnPageIndexChanging="gvParam_PageIndexChanging" AlternatingRowStyle-BackColor = "#C2D69B">
                                <Columns>

                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SALE AGENT ID" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaleAgentId" Width="30px" runat="server" Text='<%#Eval("SaleAgentId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="AGENT TYPE" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaleAgentType" Width="30px" runat="server" Text='<%#Eval("SaleAgentType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="FULL NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFullNameEn" Width="120px" runat="server" Text='<%# Eval("FullNameEn")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FULL NAME" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFullNameKh" Width="120px" runat="server" Text='<%#Eval("FullNameKh") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="POSITION" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPosition" Width="100px" runat="server" Text='<%#Eval("Position") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EMAIL" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" Width="100px" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                   
                                    <asp:TemplateField HeaderText="STATUS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Width="80px" runat="server" Text='<%# Eval("Status").ToString()=="1"? "ACTIVE":"INACTIVE"  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="VALID FROM">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValidFrom" Width="80px" runat="server" Text='<%# Convert.ToDateTime( Eval("ValidFrom").ToString()).Year == 1900 ? "" : Convert.ToDateTime( Eval("ValidFrom").ToString()).ToString("dd/MM/yyyy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="VALID TO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValidTo" Width="80px" runat="server" Text='<%# Convert.ToDateTime( Eval("ValidTo").ToString()).Year == 1900 ? "" : Convert.ToDateTime( Eval("ValidTo").ToString()).ToString("dd/MM/yyyy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="REMARKS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" Width="150px" runat="server" Text='<%# Eval("CreatedNote").ToString()  %>'></asp:Label>
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
                                    <td>Sale Agent Id: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtSaleAgentId" runat="server"></asp:TextBox>
                                    </td>
                                    
                                    <td>Full Name (EN):<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFullNameEn" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                       Full Name (KH):<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFullNameKh" style="font-family:'Khmer OS Content'; font-size:9pt; " ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                     <td>Sale Agent Type: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                         <asp:DropDownList ID="ddlSaleAgentType" runat="server"></asp:DropDownList>
                                    </td>
                                     <td>Valid From: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtValidFrom" runat="server" placeHolder="DD/MM/YYYY"></asp:TextBox>
                                    </td>
                                     <td>Valid To: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtValidTo" runat="server" placeHolder="DD/MM/YYYY"></asp:TextBox>
                                    </td>
                                <tr>
                                    <td>Position:</td>
                                    <td >
                                      <asp:TextBox ID="txtPosition" CssClass="span3"  runat="server"></asp:TextBox>
                                    </td>
                                    <td>Status:<span style="color: red; font-weight: bold;">*</span></td>
                                      <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                            <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                            <asp:ListItem Text="ACTIVE" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="INACTIVE" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Email:</td>
                                    <td>
                                          <asp:TextBox ID="txtEmail" CssClass="span3"  runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                
                               </tr>
                              <tr>
                                    <td>Remarks:
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtRemarks"  CssClass="span12" runat="server"></asp:TextBox>
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