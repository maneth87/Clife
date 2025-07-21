<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frm_gm_official_receipt_rp.aspx.cs" Inherits="Pages_Finance_frm_gm_official_receipt_rp" %>

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
    <h1>Official Receipt - Report</h1>
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
                                <asp:Label ID="lblPayDateFrom" runat="server" Text="Pay Date From:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtPayDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                <span class="star">*</span>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblPayDateTo" runat="server" Text="To:"></asp:Label>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtPayDateTo" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
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
                                <asp:Label ID="lblReceipNo" runat="server" Text="Receipt No.:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtReceiptNo" runat="server"></asp:TextBox>
                            </td>
                            <td colspan="2" >
                                <div style="float:right; vertical-align:middle;">
                                     <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                                <asp:Button id="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" Enabled="false" OnClick="btnExport_Click"/>
                                </div>
                               
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
                            <div style="float: left; padding: 5px 5px 5px 5px; height: 300px; overflow-y: scroll; overflow-x:scroll; width:60%;">
                                <asp:GridView ID="gv_valid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"  OnRowDataBound="gv_valid_RowDataBound">
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
                                          <asp:TemplateField HeaderText="Policy Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicyNumber" Width="150px" runat="server" Text='<%# Eval("Group_code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Customer" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" Width="200px" runat="server" Text='<%#Eval("customer_name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Customer ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerId" Width="150px" runat="server" Text='<%# Eval("customer_id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProduct" Width="150px" runat="server" Text='<%# Eval("en_title") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Receipt No." Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceiptNo" Width="150px" runat="server" Text='<%#Eval("Receipt_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="API">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAPI" Width="150px" runat="server" Text='<%# Eval("annual_premium") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                      

                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" Width="150px" runat="server" Text='<%# Eval("amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Interest">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInterest" Width="150px" runat="server" Text='<%# Eval("interest_amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Sum Insured">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSumInsured" Width="150px" runat="server" Text='<%# Eval("sum_assured") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Date Payment">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDatePayment" Width="150px" runat="server" Text='<%# Convert.ToDateTime( Eval("Pay_Date")).ToString("dd-MMM-yy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Period Pay">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPeriodPay" Width="150px" runat="server" Text=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created Note">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedNote" Width="150px" runat="server" Text='<%# Eval("Created_note") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Mode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayMode" Width="150px" runat="server" Text='<%# Eval("Pay_Mode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Entry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryDate" Width="150px" runat="server" Text='<%# Convert.ToDateTime( Eval("entry_Date")).ToString("dd-MMM-yy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Back Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBackDate" Width="150px" runat="server" Text=''></asp:Label>
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
            <asp:PostBackTrigger ControlID="btnExport" />
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
