<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="banca_micro_payment_upload.aspx.cs" Inherits="Pages_Business_banca_micro_payment_upload" %>

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
  <%--  <script>
        function pageLoad(sender, args) {
            $('.datepicker').datepicker();
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div id="div_message" runat="server" style="text-align: center; vertical-align: middle; margin-top: 10px; margin-bottom: 10px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 10px; height: 20px;"></div>
 <div id="dv_main" runat="server">
        <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Daily Payment Upload</h3>

        </div>
        <div class="panel-body">
            <table>
               
                <tr>
                    <td style="padding-right:20px;">
                        <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" >
                            <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                             <asp:ListItem Value="0025E613-4A0D-43E5-B6B8-BDE9A4A005EE" Text="Individual"></asp:ListItem>
                             <asp:ListItem Value="0152DF80-BA95-46A9-BB7A-E71966A34089" Text="Corporate"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-right:20px;">
                        <asp:Label ID="lblChannelItem" runat="server" Text="Campany:"></asp:Label>
                    </td>
                    <td  style="padding-right:20px;">
                        <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged" ></asp:DropDownList>
                    </td>
                  <%--   <td style="padding-right:20px;">
                        <asp:Label ID="lblChannelLocation" runat="server" Text="Branch:"></asp:Label>
                    </td>--%>
                   <%-- <td  style="padding-right:20px;">
                        <asp:DropDownList ID="ddlChannelLocation" runat="server"></asp:DropDownList>
                    </td>
                   --%>
                    <td rowspan="2">
                        
                        <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click" CssClass="btn btn-primary" style="margin-left:30px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblfile" Text="Select File:" runat="server"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:FileUpload ID="fUpload" runat="server" />
                    </td>
                    <td >
                        <a style="color: blue;" href="banca_payment_list_upload_template.xlsx"><span>Download Template</span></a>
                    </td>
                </tr>
               
            </table>
        </div>
        <div class="panel-heading">
            <h3 class="panel-title">[<asp:Label ID="lblRecords" runat="server"></asp:Label>]</h3>
            
        </div>
        <div style="overflow-x: scroll;height: 100%; width: 100%;">
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
                <%--   <asp:TemplateField HeaderText="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("id") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Branch_Code" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("Branch_Code") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch_Name"   Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchName" Width="150px" runat="server" Text='<%#Eval("Branch_Name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Product_Code"   Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblTranType" Width="150px" runat="server" Text='<%#Eval("Transaction_type") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Consumer_Num">
                        <ItemTemplate>
                            <asp:Label ID="lblApplicationNumber" Width="100px" runat="server" Text='<%# Eval("Insurance_Application_Number") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Customer_Name">
                        <ItemTemplate>
                            <asp:Label ID="lblClientName"  Width="100px" runat="server" Text='<%# Eval("Client_Name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     
                    <asp:TemplateField HeaderText="Paid_Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount"  Width="100px" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Paid_CCY">
                        <ItemTemplate>
                            <asp:Label ID="lblCurrency"  Width="110px" runat="server" Text='<%# Eval("Currency") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TXN_Date">
                        <ItemTemplate>
                            <asp:Label ID="lblTranDate"  Width="100px" runat="server" Text='<%# Eval("Tran_Date") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="COD_REFERENCE_NO">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentReferenceNo" Width="100px" runat="server" Text='<%# Eval("Payment_Reference_No") %>'></asp:Label>
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
           <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
   </div> 
</asp:Content>

