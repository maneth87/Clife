<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="04frmChannelLocation.aspx.cs" Inherits="Pages_Channel_04frmChannelLocation" %>


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
                    <h2 class="panel-title">CHANNEL LOCATION</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH CHANNEL LOCATION</h3>
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
                            <h3 class="panel-title">CHANNEL LOCATION LIST</h3>
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
                                    <asp:TemplateField HeaderText="CHANNEL LOCATION ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelLocationId" Width="80px" runat="server" Text='<%#Eval("Channel_Location_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="CAHNNEL NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" Width="150px" runat="server" Text='<%# Eval("ChannelName")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BRANCH CODE" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchCode" Width="80px" runat="server" Text='<%#Eval("Office_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="BRANCH NAME" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchName" Width="150px" runat="server" Text='<%#Eval("Office") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PHONE" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhone" Width="60px" runat="server" Text='<%#Eval("PhoneNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ADDRESS EN">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddressEn" Width="20px" runat="server" Text='<%# Eval("Address").ToString()  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ADDRESS KH">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddressKh" Width="20px" runat="server" Text='<%# Eval("AddressKh")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="STATUS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Width="150px" runat="server" Text='<%# Eval("Status").ToString()=="1"? "ACTIVE":"INACTIVE"  %>'></asp:Label>
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
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlChannelName" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelName_SelectedIndexChanged" >

                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Branch Code: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtBranchCode" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Branch Name:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBranchName" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        Phone Number:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPhoneNumber"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Address (EN):</td>
                                    <td colspan="5">
                                      <asp:TextBox ID="txtAddressEn" CssClass="span12" placeholder="Write in english" runat="server"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>Address (KH):
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtAddressKh"  CssClass="span12" style="font-family:'Khmer OS Content'; font-size:10pt;" placeholder="Write in khmer" runat="server"></asp:TextBox>
                                    </td>
                                  
                                </tr>
                               <tr>
                                   <td>Status:</td>
                                      <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                            <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                            <asp:ListItem Text="ACTIVE" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="INACTIVE" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
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