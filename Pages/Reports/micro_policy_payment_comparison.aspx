<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_policy_payment_comparison.aspx.cs" Inherits="Pages_Reports_micro_policy_payment_comparison" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />

    <style>
        .star {
            color: red;
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

        .CheckboxList input {
            float: left;
            clear: both;
        }

        .my_progress {
            width: 100%;
            height: 100%;
            color: #FFFFFF;
            position: absolute;
            float: left;
            overflow: hidden;
            top: 0px;
            left: 0px;
        }

            .my_progress div.tr {
                background-color: black;
                -moz-opacity: 0.7;
                opacity: 0.7;
                filter: alpha(opacity=70);
                position: absolute;
                top: 0px;
                left: 0px;
                z-index: 998;
                width: 100%;
                height: 100%;
            }

            .my_progress div.main {
                position: relative;
                top: 30%;
                width: 400px;
                z-index: 999;
                margin: auto;
                border: 2px solid #2b0557;
                border-radius: 5px;
                -moz-border-radius: 5px;
                -webkit-border-radius: 5px;
                -khtml-border-radius: 5px;
                -moz-box-shadow: 0 0 50px #fff;
                -webkit-box-shadow: 0 0 50px #fff;
                text-align: center;
                background-color: White;
                color: Black;
            }

                .my_progress div.main div.dhead {
                    text-align: left;
                    background-image: url(../images/top_nav_bg.png);
                }
    </style>
    <%--<script>
         function pageLoad(sender, args) {
             $('.datepicker').datepicker();
         }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="div_message" runat="server" style="text-align: center; vertical-align: middle; margin-top: 10px; margin-bottom: 10px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 10px; height: 20px;"></div>

    <div id="dv_main" runat="server">
        <asp:UpdatePanel ID="UP" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Policy Issued & Payment Comparison Report</h3>

                    </div>
                    <div class="panel-body">
                        <table style="width: auto;">
                            <tr>
                                <td>
                                    Channel:
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlChannelItem" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblDateF" runat="server" Text="Issued Date From:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                    <span class="star">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblDateTo" runat="server" Text="To:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtDateTo" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                    <span class="star">*</span>
                                </td>

                                <td rowspan="2">
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClick="btnGenerate_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                                   

                                </td>
                                <td>
                                   
                                </td>
                            </tr>


                        </table>

                    </div>
                      <div style="float: right; position: static; padding-right: 15px; margin-top: 10px;">
                     <asp:CheckBox ID="ckbIsNo" Width="200px" runat="server" Text="Export missed match only?" CssClass="CheckboxList" />
                           <asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary btn-small" Style="margin-left: 20px;" />
                </div>
                    <div class="panel-heading">
                        <h3 class="panel-title">Result [<asp:Label ID="lblRecords" runat="server"></asp:Label>]</h3>

                    </div>
                    <div style="overflow-x: scroll; height: 100%; width: 100%;">
                        <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowSorting="true" AllowPaging="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                            OnPageIndexChanging="gv_valid_PageIndexChanging" OnRowDataBound="gv_valid_RowDataBound" OnSorting="gv_valid_Sorting">
                            <SelectedRowStyle BackColor="#EEFFAA" />
                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="No" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("Branch_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Branch Name" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchName" Width="150px" runat="server" Text='<%#Eval("branch_name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application Number" Visible="true" SortExpression="application_number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApplicationNumber" Width="90px" runat="server" Text='<%#Eval("application_number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Policy Number" Visible="true" SortExpression="Policy_Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPolicyNumber" Width="90px" runat="server" Text='<%#Eval("Policy_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issued Date" Visible="true" SortExpression="issued_date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssuedDate" Width="90px" runat="server" Text='<%# Convert.ToDateTime( Eval("issued_date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Package" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPackage" Width="60px" runat="server" Text='<%#Eval("Package") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount (USD)" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" Width="50px" runat="server" Text='<%#Eval("amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" Width="150px" runat="server" Text='<%#Eval("customer_name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gender" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGender" Width="50px" runat="server" Text='<%#Eval("Gender") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Agent Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgentCode" Width="60px" runat="server" Text='<%#Eval("sale_agent_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Agent Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgentName" Width="150px" runat="server" Text='<%#Eval("agent_name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="App_Number Bank" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAppNumberBank" Width="90px" runat="server" Text='<%#Eval("application_number_bank") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name Bank" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerBank" Width="150px" runat="server" Text='<%#Eval("CUSTOMER_NAME_BANK") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transaction Type" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransactionType" Width="100px" runat="server" Text='<%#Eval("transaction_type") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid Amount" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaidAmount" Width="50px" runat="server" Text='<%#Eval("paid_amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" Width="50px" runat="server" Text='<%#Eval("currency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transaction Date" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransactionDate" Width="90px" runat="server" Text='<%# Convert.ToDateTime( Eval("tran_date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Reference No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentReferenceNo" Width="110px" runat="server" Text='<%# Eval("PAYMENT_REFERENCE_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Reference No.Bank">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentReferenceNoBank" Width="110px" runat="server" Text='<%# Eval("PAYMENT_REFERENCE_NO_BANK") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Correct?" SortExpression="remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" Width="110px" runat="server" Text='<%# Eval("remarks") %>'></asp:Label>
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
        <asp:UpdateProgress ID="upg" runat="server" AssociatedUpdatePanelID="UP">
            <ProgressTemplate>
                <div class="my_progress">
                    <div class="tr"></div>
                    <div class="main">
                        <div class="dhead">
                            <h2>FETCHING DATA</h2>
                        </div>
                        <p>
                            <img id="loader" src="../../App_Themes/images/loader.gif" alt="Progressing" />
                        </p>
                        <p>Please wait...</p>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</asp:Content>

