<%@ Page Title="PRODUCT DISCOUNT CONFIG" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="06frmProductDiscountConfig.aspx.cs" Inherits="Pages_Products_06frmProductDiscountConfig" %>

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
                    <h2 class="panel-title">PRODUCT DISCOUNT CONFIGURATION</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH PRODUCT DISCOUNT CONFIGURATION</h3>
                        </div>
                        <div class="panel-body">
                            <table>

                                <tr>

                                    <td>Product ID: 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductNameSearch" placeholder="ANY WORDS OF PRODUCT ID" runat="server" Width="300px"></asp:TextBox>
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
                            <h3 class="panel-title">PRODUCT DISCOUNT CONFIGURATION LIST
                            </h3>

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
                                            <asp:Label ID="lblId" Width="80px" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="CAHNNEL NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" Width="150px" runat="server" Text='<%# Eval("ChannelName")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PRODUCT ID" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductId" Width="80px" runat="server" Text='<%#Eval("ProductID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="CLIENT TYPE" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientType" Width="80px" runat="server" Text='<%#Eval("ClientType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIDER ID" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRiderId" Width="150px" runat="server" Text='<%#Eval("ProductRiderID") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BASIC SA" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBasicSa" Width="60px" runat="server" Text='<%#Eval("BasicSumAssured") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RIDER SA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRiderSa" Width="20px" runat="server" Text='<%# Eval("RiderSumAssured").ToString()  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BASIC DIS. AMOUNT">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBasicDisAmount" Width="20px" runat="server" Text='<%# Eval("BasicDiscountAmount")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIDER DIS. AMOUNT">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRiderDisAmount" Width="20px" runat="server" Text='<%# Eval("RiderDiscountAmount")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EFFECTIVE DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveDate" Width="20px" runat="server" Text='<%# Convert.ToDateTime( Eval("EffectiveDate").ToString()).ToString("dd/MM/yyyy")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="EXPIRY DATE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpiryDate" Width="150px" runat="server" Text='<%# Convert.ToDateTime( Eval("ExpiryDate").ToString()).ToString("dd/MM/yyyy")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="STATUS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Width="150px" runat="server" Text='<%# Eval("Status").ToString()=="True"? "ACTIVE":"INACTIVE"  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="REMARKS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" Width="150px" runat="server" Text='<%# Eval("Remarks")  %>'></asp:Label>
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
                                    <td> Channel Item Name:<span style="color: red; font-weight: bold;">*</span></td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlChannelItem" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged">

                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Product ID: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>Basic SA:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBasicSa" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        Basic Dis. Amount:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtBasicDisAmount"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Rider ID:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlRiderId" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>Rider SA:</td>
                                    <td>
                                        <asp:TextBox ID="txtRiderSa" runat="server"></asp:TextBox>
                                    </td>
                                     <td>
                                        Rider Dis. Amount:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtRiderDisAmount"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Effective Date:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEffectiveDate" placeholder="DD/MM/YYYY" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Expiry Date:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExpiryDate" placeholder="DD/MM/YYYY" runat="server"></asp:TextBox>
                                    </td>
                                   <td>Status:<span style="color: red; font-weight: bold;">*</span></td>
                                      <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                            <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                            <asp:ListItem Text="ACTIVE" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="INACTIVE" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               
                               <tr>
                                  <td>Client Type:<span style="color: red; font-weight: bold;">*</span></td>
                                   <td>
                                       <asp:DropDownlist ID="ddlClientType" runat="server" class="span3">
                                        <asp:ListItem Text="SELF" Value="SELF"></asp:ListItem>
                                        <asp:ListItem Text="CLIENT_FAMILY" Value="CLIENT_FAMILY"></asp:ListItem>
                                        <asp:ListItem Text="BANK_STAFF" Value="BANK_STAFF"></asp:ListItem>
                                        <asp:ListItem Text="BANK_STAFF_FAMILY" Value="BANK_STAFF_FAMILY"></asp:ListItem>
                                         <asp:ListItem Text="REPAYMENT" Value="REPAYMENT"></asp:ListItem>
                                    </asp:DropDownlist>
                                   </td>
                                   <td>
                                       Remarks:
                                   </td>
                                   <td colspan="4">
                                       <asp:TextBox ID="txtRemarks" runat="server" Width="97%"></asp:TextBox>
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



