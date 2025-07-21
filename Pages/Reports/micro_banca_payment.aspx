<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_banca_payment.aspx.cs" Inherits="Pages_Reports_banca_micro_payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />

    <style>
        .star {
            color:red;
        }

     
         .GridPager a, .GridPager span
    {
        display: block;
        height: 15px;
        width: 15px;
        font-weight: bold;
        text-align: center;
        text-decoration: none;
       
    }
    .GridPager a
    {
        background-color: #f5f5f5;
        color: #969696;
        border: 1px solid #969696;
    }
    .GridPager span
    {
        background-color:#21275b; /*#A1DCF2;*/ 
        color:white;/*#000;*/ 
        border: 1px solid #3AC0F2;
    }
    </style>
     <%--<script>
         function pageLoad(sender, args) {
             $('.datepicker').datepicker();
         }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Search Payment Report</h3>

        </div>
        <div class="panel-body">
            <table>
                 <tr>
                    <td  style="padding-right:20px;">
                        <asp:Label ID="lblDateF" runat="server" Text="Payment Date From:"></asp:Label>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:TextBox ID="txtDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:Label ID="lblDateTo" runat="server" Text="To:"></asp:Label>
                    </td >
                    <td style="padding-right:20px;">
                        <asp:TextBox ID="txtDateTo" runat="server" placeholder="DD-MM-YYYY"  CssClass="datepicker"></asp:TextBox>
                        <span class="star">*</span>
                    </td>
                    <td style="padding-right:20px;">
                        
                    </td>
                    <td style="padding-right:20px;">
                       
                    </td>
                </tr>
                <tr>
                                        
                    <td style="padding-right:20px;">
                        <asp:Label ID="lblChannelItem" runat="server" Text="Campany:"></asp:Label>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged" ></asp:DropDownList>
                    </td>
                     <td style="padding-right:20px;">
                        <asp:Label ID="lblChannelLocation" runat="server" Text="Branch:"></asp:Label>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:DropDownList ID="ddlChannelLocation" runat="server"></asp:DropDownList>
                    </td>
                   
                    <td rowspan="2">
                         <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" style="margin-left:30px;" />
                        <asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary" style="margin-left:30px;" />
                    </td>
                </tr>
               
               
            </table>
        </div>
        <div class="panel-heading">
            <h3 class="panel-title">Result (<asp:Label ID="lblRecords" runat="server"></asp:Label>)</h3>
            
        </div>
        <div >
            <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center"  HeaderStyle-ForeColor="White"  HeaderStyle-BackColor="#21275b"
                  OnPageIndexChanging="gv_valid_PageIndexChanging">
                <SelectedRowStyle BackColor="#EEFFAA"   />
                    <PagerStyle HorizontalAlign = "Center" CssClass = "GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="No" Visible="true" >
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Name" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("channel_name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch Code" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("branch_Code") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch Name"   Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchName" Width="150px" runat="server" Text='<%#Eval("branch_name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                   
                     <asp:TemplateField HeaderText="Payment Reference No.">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentReferenceNo"  Width="110px" runat="server" Text='<%# Eval("PAYMENT_REFERENCE_NO") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Transaction Type">
                        <ItemTemplate>
                            <asp:Label ID="lblTransactionType"  Width="110px" runat="server" Text='<%# Eval("transaction_type") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Insurance Application Number">
                        <ItemTemplate>
                            <asp:Label ID="lblInsuranceApplicationNumber"  Width="150px" runat="server" Text='<%# Eval("insurance_application_number")  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client Name">
                        <ItemTemplate>
                            <asp:Label ID="lblClientName"  Width="150px" runat="server" Text='<%# Eval("client_name")  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Premium"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server"  Width="50px" Text='<%# Eval("premium") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Currency"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblCurrency" runat="server" Width="50px"  Text='<%# Eval("Currency") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Date">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentdate" runat="server"  Width="100px" Text='<%# Convert.ToDateTime( Eval("payment_date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                </Columns>

            </asp:GridView>
        </div>
    

    </div>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    
</asp:Content>

