<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frm_gm_issue_invoice.aspx.cs" Inherits="Pages_Finance_frm_gm_issue_invoice" %>

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
    <h1>Issue Invoices</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Search Bunch - Pending</h3>

                </div>
                <div class="panel-body">
                    <table style="width: auto;">
                        <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblReportDateFrom" runat="server" Text="Report Date From:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtReportDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                <span class="star">*</span>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblReportDateTo" runat="server" Text="To:"></asp:Label>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtReportDateTo" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                <span class="star">*</span>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"></asp:DropDownList>
                                <span class="star">*</span>
                            </td>

                        </tr>
                        <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true"></asp:DropDownList>
                                <span class="star">*</span>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="panel-heading">

                    <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                </div>
                <table class="table-bordered" id="tblResult" runat="server">
                    <tr>
                        <td>
                            <h3 class="panel-title">Bunch Summary</h3>
                        </td>
                        <td>
                            <h3 class="panel-title">Bunch Details</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%; vertical-align: top;">
                            <div style="float: left; padding: 0px 5px 5px 5px; height: 300px; overflow-y: scroll;">
                                <asp:GridView ID="gv_valid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelId" runat="server" Text='<%#Eval("Channel_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel Item Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelItemId" Width="150px" runat="server" Text='<%#Eval("channel_item_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bunch ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBunchId" Width="100px" runat="server" Text='<%# Eval("bunch_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bunch Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBunchNumber" Width="10px" runat="server" Text='<%# Eval("bunch_number") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel Name" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelName" Width="200px" runat="server" Text='<%#Eval("Channel_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupCode" Width="80px" runat="server" Text='<%# Eval("group_master_code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Number Policies">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNumberPolicies" Width="50px" runat="server" Text='<%# Eval("number_policy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" Width="50px" runat="server" Text='<%# Eval("amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" Width="50px" runat="server" Text='<%# Eval("discount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" Width="50px" runat="server" Text='<%# Eval("total_amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" Width="60px" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentType" Width="60px" runat="server" Text='<%# Eval("payment_type") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Report Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReportDate" Width="60px" runat="server" Text='<%# Convert.ToDateTime( Eval("Report_Date")).ToString("dd-MMM-yy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Check">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckb" runat="server" OnCheckedChanged="ckb_CheckedChanged" AutoPostBack="true" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                            </div>
                            <div>
                                <table style="margin: 0 5px 5px 5px; border: 2px solid #21275b; width: auto;">
                                    <tr>
                                        <td>Invoice Date<span class="star">*</span>:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" placeHolder="DD-MM-YYYY" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>Invoice No.<span class="star">*</span>:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtInvoiceNo" AutoPostBack="true" placeHolder="Input Number" Enabled="false" OnTextChanged="txtInvoiceNo_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total Policies:</td>
                                        <td>
                                            <asp:TextBox ID="txtTotalPolicies" runat="server" Enabled="false" Font-Bold="true"></asp:TextBox>
                                        </td>
                                        <td>Total Amount USD:</td>
                                        <td>
                                            <asp:TextBox ID="txtTotalAmount" runat="server" Enabled="false" Font-Bold="true"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Total Discount USD:</td>
                                        <td>
                                            <asp:TextBox ID="txtTotalDiscount" runat="server" Enabled="false" Font-Bold="true"></asp:TextBox>
                                        </td>
                                        <td>Total Amount After Dis. USD:</td>
                                        <td>
                                            <asp:TextBox ID="txtTotalAmountAfterDiscount" runat="server" Enabled="false" Font-Bold="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Exchange Rate Tax<span class="star">*</span>:</td>
                                        <td>
                                            <asp:TextBox ID="txtExchangeRateTax" runat="server" AutoPostBack="true" OnTextChanged="txtExchangeRateTax_TextChanged" Enabled="false" Font-Bold="true"></asp:TextBox>
                                        </td>
                                        <td>Total Amount After Dis. KHR:</td>
                                        <td>
                                            <asp:TextBox ID="txtTotalAmountKh" runat="server" Enabled="false" Font-Bold="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div style="float: right; padding: 0px 5px 5px 0px;">
                                                <asp:Button ID="btnIssue" Text="Issue Invoice" runat="server" OnClick="btnIssue_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" OnClientClick="return confirm('Do you want to issue invoice?');" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width: 60%; float: none; vertical-align: top;">
                            <div style="overflow-x: scroll; overflow-y: scroll; height: 400px; width: 100%; padding: 0px 0px 5px;">
                                <asp:GridView ID="gvDetail" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Height="400px" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BunchID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBunchId" runat="server" Text='<%#Eval("bunch_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BunchDetailID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBunchDetailId" runat="server" Text='<%#Eval("bunch_detail_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bunch Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBunchNumber" Width="50px" runat="server" Text='<%# Eval("bunch_number") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Customer ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerID" Width="160px" runat="server" Text='<%# Eval("Customer_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Policy Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicyNumber" Width="160px" runat="server" Text='<%#Eval("Policy_number") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Premium">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPremium" Width="70px" runat="server" Text='<%# Eval("premium") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" Width="70px" runat="server" Text='<%# Eval("Discount_amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" Width="120px" runat="server" Text='<%# Eval("total_amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Width="50px" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
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


