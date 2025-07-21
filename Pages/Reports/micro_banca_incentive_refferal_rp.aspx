<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_banca_incentive_refferal_rp.aspx.cs" Inherits="Pages_Reports_micro_banca_incentive_refferal_rp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
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
    </style>
    <%--  <script>
        function pageLoad(sender, args) {
            $('.datepicker').datepicker();
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="div_message" runat="server" style="text-align: center; vertical-align: middle; margin-top: 10px; margin-bottom: 10px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 10px; height: 20px;"></div>

    <div id="dv_main" runat="server">
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
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Generate Refferal Incentive</h3>

                    </div>
                    <div class="panel-body">
                        <table style="width: 100%;">
                            <tr>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblIssuedDateF" runat="server" Text="Issued Date From:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtIssuedDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker span2"></asp:TextBox>
                                    <span class="star">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblIssuedDateTo" runat="server" Text="To:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtIssuedDateTo" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker span2"></asp:TextBox>
                                    <span class="star">*</span>
                                </td>

                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlChannel" runat="server" CssClass="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged">
                                        <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Value="0025E613-4A0D-43E5-B6B8-BDE9A4A005EE" Text="Individual"></asp:ListItem>
                                        <asp:ListItem Value="0152DF80-BA95-46A9-BB7A-E71966A34089" Text="Corporate"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlChannelItem" runat="server" CssClass="span2" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchName_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>

                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblChannelLocation" runat="server" Text="Branch Code:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;" colspan="9">
                                    <%-- <asp:DropDownList ID="ddlChannelLocation" runat="server"></asp:DropDownList>--%>
                                    <table style="width: 100%; border: 1px solid gray;" id="tblUser" runat="server">
                                        <tr>
                                            <td style="padding: 3px;">

                                                <asp:CheckBoxList ID="cblChannelLocation" runat="server" RepeatDirection="Horizontal" RepeatColumns="12" CssClass="CheckboxList" Width="100%" CellSpacing="5" AutoPostBack="true" OnSelectedIndexChanged="cblChannelLocation_SelectedIndexChanged"></asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>


                            </tr>


                        </table>
                    </div>
                    <div class="panel-heading">
                        <h3 class="panel-title">Result [
                        <asp:Label ID="lblRecords" runat="server"></asp:Label><asp:Label ID="lblTotalPremium" runat="server"></asp:Label>
                            ]
                        <div style="float: right; margin-bottom: 30px;">
                            <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" /><asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                            </div>
                        </h3>

                    </div>
                    <div style="overflow-x: scroll; height: 450px; width: 100%;">
                        <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="50" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                            OnRowDataBound="gv_valid_RowDataBound" OnPageIndexChanging="gv_valid_PageIndexChanging">
                            <SelectedRowStyle BackColor="#EEFFAA" />

                            <PagerStyle HorizontalAlign="Left" CssClass="GridPager" />


                            <Columns>
                                <asp:TemplateField HeaderText="No" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationID" runat="server" Text='<%#Eval("channel_location_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Channel ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChannelID" runat="server" Text='<%#Eval("channel_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Channel Item ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChannelItemID" runat="server" Text='<%#Eval("channel_item_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("office_code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchName" Width="250px" runat="server" Text='<%#Eval("office_name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Referral Staff ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReferralStaffID" runat="server" Width="60px" Text='<%# Eval("Referral_staff_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Referral Staff Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReferralStaffName" runat="server" Width="120px" Text='<%# Eval("Referral_staff_name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Policy Package1">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPolicyPackage1" runat="server" Width="70px" Text='<%# Eval("policy_package1") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Policy Package2">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPolicyPackage2" runat="server" Width="70px" Text='<%# Eval("policy_package2") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Incentive Package1">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIncentivePackage1" runat="server" Width="70px" Text='<%# Eval("Incentive_package1") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Incentive Package2">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIncentivePackage2" runat="server" Width="70px" Text='<%# Eval("Incentive_package2") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Incentive">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalIncentive" runat="server" Width="70px" Text='<%# Eval("Total_Incentive") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>

                        <asp:GridView runat="server" AutoGenerateColumns="true" ID="gvIncentive" CssClass="grid-layout" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b">
                        </asp:GridView>
                    </div>


                </div>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>


