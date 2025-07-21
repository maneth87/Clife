<%@ Page Title="PRODUCT CONFIG" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="05frmProductConfig.aspx.cs" Inherits="Pages_Products_05frmProductConfig" %>


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
                    <h2 class="panel-title">PRODUCT CONFIGURATION</h2>
                </div>
                <div class="panel-body">
                    <div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">SEARCH PRODUCT CONFIGURATION</h3>
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
                            <asp:LinkButton ID="ibtnAdd" runat="server" Text="ADD NEW" OnClick="ibtnAdd_Click1"></asp:LinkButton>
                        </div>
                        <div class="panel-heading">
                            <h3 class="panel-title">PRODUCT CONFIGURATION LIST
                            </h3>

                        </div>

                        <div class="panel-body" style="overflow-x:scroll; width:auto;">
                            <asp:GridView ID="gvParam" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowEditing="gvParam_RowEditing" PageSize="10" AllowPaging="true" OnPageIndexChanging="gvParam_PageIndexChanging"  AlternatingRowStyle-BackColor = "#C2D69B">
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
                                            <asp:Label ID="lblProductId" Width="80px" runat="server" Text='<%#Eval("Product_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="PRODUCT NAME" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" Width="80px" runat="server" Text='<%#Eval("En_Title") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RIDER ID" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRiderId" Width="150px" runat="server" Text='<%#Eval("RiderProductID") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BASIC SA RANGE" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBasicSaRange" Width="60px" runat="server" Text='<%#Eval("BasicSumAssured") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RIDER SA RANGE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRiderSaRange" Width="100px" runat="server" Text='<%# Eval("RiderSumAssured").ToString()  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PAYMODE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymode" Width="20px" runat="server" Text='<%# Eval("PayModeString")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BUSINUSS TYPE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBusinessType" Width="100px" runat="server" Text='<%# Eval("BusinessType")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ALLOW REFER">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllowRefer" Width="20px" runat="server" Text='<%# Eval("AllowRefer").ToString()=="True"? "YES":"NO"   %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="REQUIRED RIDER">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequiredRider" Width="20px" runat="server" Text='<%# Eval("IsRequiredRider").ToString()=="True"? "YES":"NO"   %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="VALIDATE REFERRAL ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValidateReferralId" Width="20px" runat="server" Text='<%# Eval("IsValidateReferralId").ToString()=="True"? "YES":"NO"   %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="COVER PERIOD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCoverPeriod" Width="150px" runat="server" Text='<%# Eval("CoverPeriodTypeString")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="PAY PERIOD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayPeriod" Width="150px" runat="server" Text='<%# Eval("PayPeriodTypeString")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="MARKETING NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMarketingName" Width="150px" runat="server" Text='<%# Eval("MarketingName")  %>'></asp:Label>
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
                                        <asp:DropDownList ID="ddlChannelItem" Width="100%" runat="server">

                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Product ID: <span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProduct" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>Basic SA Range:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBasicSaRange" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblBasicSaRangeTip" runat="server" CssClass="tip"></asp:Label>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>Rider ID:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlRiderId" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>Rider SA Range:</td>
                                    <td>
                                        <asp:TextBox ID="txtRiderSaRange" runat="server"></asp:TextBox>
                                    </td>
                                     <td colspan="2">
                                        <asp:Label ID="lblRiderSaRangeTip" runat="server" CssClass="tip"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Required Rider:</td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlRequiredRider" runat="server">
                                            <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                            <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>Paymode:<span style="color: red; font-weight: bold;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPayMode" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                          <asp:Label ID="lblPayModeTip" runat="server" CssClass="tip"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Business Type:<span style="color: red; font-weight: bold;">*</span> </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBusinessType" runat="server">
                                          <%--  <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                            <asp:ListItem Text="INDIVIDUAL" Value="INDIVIDUAL"></asp:ListItem>
                                            <asp:ListItem Text="REFERRAL" Value="REFERRAL"></asp:ListItem>
                                            <asp:ListItem Text="BUNDLE" Value="BUNDLE"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Allow Refer: </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAllowRefer" runat="server">
                                            <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                            <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                   <td>
                                       Validate Referral Id:
                                   </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRequiredReferralId" runat="server">
                                           
                                            <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                     <td>Cover Period:<span style="color: red; font-weight: bold;">*</span> </td>
                                    <td>
                                        <asp:TextBox ID="txtCoverPeriod" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="lblCoverPeriodTip" runat="server" CssClass="tip"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                     <td>Pay Period:<span style="color: red; font-weight: bold;">*</span> </td>
                                    <td>
                                        <asp:TextBox ID="txtPayPeriod" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="lblPayPeriodTip" runat="server" CssClass="tip"></asp:Label>
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
                                   <td>Marketing Name:<span style="color: red; font-weight: bold;">*</span></td>
                                   <td>
                                       <asp:TextBox ID="txtMarketingName" runat="server"></asp:TextBox>
                                   </td>
                                   <td>
                                       Remarks:
                                   </td>
                                   <td colspan="3">
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



