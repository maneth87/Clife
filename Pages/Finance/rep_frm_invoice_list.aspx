<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="rep_frm_invoice_list.aspx.cs" Inherits="Pages_Finance_rep_frm_invoice_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <style>
        .star {
            color: red;
        }

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
    <h1>Invoices - List</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Input Criteria</h3>

                </div>
                <div class="panel-body">
                    <table style="width: auto;">
                        <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblInvoiceDateFrom" runat="server" Text="Invoice Date From:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtInvoiceDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                <span class="star">*</span>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblInvoiceDateTo" runat="server" Text="To:"></asp:Label>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtInvoiceDateTo" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                <span class="star">*</span>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                           
                           

                        </tr>
                        <tr>
                             <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No.:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                            </td>
                             <td style="vertical-align:middle;">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />

                            </td>
                        </tr>
                    </table>
                </div>
                <div class="panel-heading">
                    <h3 class="panel-title">Result</h3>
                </div>
                <table class="table-bordered" id="tblResult" runat="server">

                    <tr>
                        <td style="width: 100%; vertical-align: top;">
                            <div style="float: left; padding: 5px 5px 5px 5px; height: 300px; overflow-y: scroll; ">
                                <asp:GridView ID="gv_valid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b" OnRowCommand="gv_valid_RowCommand" OnRowDataBound="gv_valid_RowDataBound">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" Width="30px" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelId" Width="150px" runat="server" Text='<%#Eval("channel_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel Item Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelItemId" Width="150px" runat="server" Text='<%#Eval("channel_item_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Invoice Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNumber" Width="100px" runat="server" Text='<%#Eval("invoice_number") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Policy Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicyNumer" Width="100px" runat="server" Text='<%#Eval("Group_master_code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" Width="250px" runat="server" Text='<%#Eval("channel_name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                       

                                        <asp:TemplateField HeaderText="Amount USD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" Width="80px" runat="server" Text='<%# Eval("amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscountAmount" Width="50px" runat="server" Text='<%# Eval("discount_amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" Width="100px" runat="server" Text='<%# Eval("total_amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax Change Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExchangeRate" Width="50px" runat="server" Text='<%# Eval("exchange_rate_tax") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amount KHR">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmountKHR" Width="100px" runat="server" Text='<%# Eval("total_amount_kh") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Number of Policies">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNumberPolicy" Width="50px" runat="server" Text='<%# Eval("number_policy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" Width="80px" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoiced Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoicedDate" Width="80px" runat="server" Text='<%# Convert.ToDateTime( Eval("invoice_date")).ToString("dd-MMM-yy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Invoice Due Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoicDueDate" Width="80px" runat="server" Text='<%# Convert.ToDateTime( Eval("invoice_due_date")).ToString("dd-MMM-yy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="100px">
                                            <ItemTemplate>

                                                <asp:LinkButton ID="ibtnUpdate" runat="server" Text="Print" Style="color: red; padding: 5px 5px 5px 5px; width: 100px;" CommandName="CMD_PRINT" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                            </div>

                        </td>

                    </tr>
                </table>

            </div>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
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
</asp:Content>

