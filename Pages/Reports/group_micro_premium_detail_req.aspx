<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="group_micro_premium_detail_req.aspx.cs" Inherits="Pages_Reports_group_micro_premium_detail_req" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="10000"></asp:ScriptManager>
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
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Group Micro Policy Premium Detail</h3>
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
                            </td>
                          
                            
                        </tr>
                         <tr>
                               <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblProduct" runat="server" Text="Product:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProduct" runat="server"></asp:DropDownList>
                            </td>
                             <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                         <tr>
                            <td colspan="6">
                                <span class="star">* This report show only policies status [in-force & expired].</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: right; position: static; padding-right: 15px; margin-top: 10px;">
                    <asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />

                </div>
                <div class="panel-heading">
                    <h3 class="panel-title">Result [<asp:Label ID="lblRecords" runat="server"></asp:Label><asp:Label ID="lblTotalPremium" runat="server"></asp:Label>]
                    </h3>
                </div>
                <div style="overflow-x: scroll; height: 100%; width: 100%;">
                    <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" AllowSorting="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                        OnPageIndexChanging="gv_valid_PageIndexChanging" OnSorting="gv_valid_Sorting">
                        <SelectedRowStyle BackColor="#EEFFAA" />
                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ChannelId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblChannelId" runat="server" Text='<%#Eval("ChannelId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ChannelItemId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblChannelItemId" Width="150px" runat="server" Text='<%#Eval("ChannelItemId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ChannelName" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblChannelName" Width="130px" runat="server" Text='<%#Eval("ChannelName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IssuedDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("SubmittedDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CustomerID">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerId" Width="130px" runat="server" Text='<%# Eval("CustomerNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Policy Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyNumber" Width="130px" runat="server" Text='<%# Eval("PolicyNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agreement Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgreementNumber" Width="110px" runat="server" Text='<%# Eval("AgreementNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CustomerName">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" Width="150px" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" Width="60px" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DOB">
                                <ItemTemplate>
                                    <asp:Label ID="lblDob" Width="90px" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).ToString("dd-MMM-yyyy")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Age">
                                <ItemTemplate>
                                    <asp:Label ID="lblAge" Width="50px" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EffectiveDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("EffectivedDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExpiryDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("ExpireDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period(Days)">
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriod" Width="100px" runat="server" Text='<%# Eval("CoveragePeriod") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" Width="100px" runat="server" Text='<%# Eval("Currency") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exchange Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblExchange" Width="100px" runat="server" Text='<%# Eval("ExchangeRate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LoanAmountKHR">
                                <ItemTemplate>
                                    <asp:Label ID="lblLoanAmount" Width="100px" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LoanAmount(USD)">
                                <ItemTemplate>
                                    <asp:Label ID="lblLoanAmountUSD" Width="100px" runat="server" Text='<%# Eval("LoanAmountUSD") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" Width="100px" runat="server" Text='<%# Eval("PremiumRate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PremiumAmountKhr">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremiumAmountKh" Width="100px" runat="server" Text='<%# Eval("PremiumKh") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PremiumAmountUSD">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremiumAmountUSD" Width="100px" runat="server" Text='<%# Eval("Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="PolicyStatus">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatus" Width="100px" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                              <asp:TemplateField HeaderText="PolicyStatusDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatusDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("PolicyStatusDate")).Year==2000? "":Convert.ToDateTime( Eval("PolicyStatusDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
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


