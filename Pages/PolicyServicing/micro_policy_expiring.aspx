<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_policy_expiring.aspx.cs" Inherits="Pages_PolicyServicing_micro_policy_expiring" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <style>
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
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>

    <ul class="toolbar">
    </ul>
    <div id="div_message" runat="server" style="text-align: center; vertical-align: middle; margin-top: 10px; margin-bottom: 10px; color: #fcfcfc; font-family: Arial; font-size: 16px; font-weight: bold; background-color: #f00; padding: 10px; border-radius: 10px; height: 20px;"></div>
    <div id="div_main" runat="server">
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
                        <h3 class="panel-title">Search Criterias</h3>

                    </div>
                    <div class="panel-body">
                        <table>
                            <tr>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblBranchName" runat="server" Text="Branch Name:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;" colspan="5">
                                    <asp:DropDownList ID="ddlBranchName" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchName_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td rowspan="3">
                                    <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />

                                </td>
                            </tr>
                            <tr>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblBranchcode" runat="server" Text="Branch Code:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtBranchCode" runat="server"></asp:TextBox>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblCustomerNameEnglish" runat="server" Text="Customer Name (English):"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtCustomerNameEnglish" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>

                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblDateOfBirth" runat="server" Text="Customer No:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtCustomerNumber" runat="server"></asp:TextBox>

                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblCertificateNumber" runat="server" Text="Certificate No:"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtCertificateNumber" runat="server"></asp:TextBox>
                                </td>
                            </tr>

                        </table>
                    </div>

                    <div class="panel-heading" id="divUpdate" runat="server">

                        <h3 class="panel-title">Update Status</h3>

                    </div>
                    <div class="panel-body" style="background-color: #21275b;" runat="server" id="divUpdateContent">
                        <table style="width: 50%; text-align: left;" border="0">
                            <tr>
                                <td style="width: 10%; vertical-align: middle; color: white;">Status:<span style="color: red;">*</span></td>
                                <td style="width: 57%; vertical-align: bottom">

                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="100%">
                                        <%-- <asp:ListItem Value="Agree" Text="Agree"></asp:ListItem>
                                    <asp:ListItem Value="Follow up" Text="Follow up"></asp:ListItem>
                                    <asp:ListItem Value="Reject" Text="Reject"></asp:ListItem>
                                      <asp:ListItem Value="Over Age" Text="Over age"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td rowspan="2" style="vertical-align: bottom;">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass=" btn btn-primary" OnClick="btnUpdate_Click" Style="margin-left: 30px; margin-bottom: 10px;" />
                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass=" btn btn-primary" OnClick="btnClose_Click" Style="margin-left: 30px; margin-bottom: 10px;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; vertical-align: middle; color: white;">Remarks:<span style="color: red;">*</span></td>
                                <td style="width: 57%; vertical-align: bottom">
                                    <asp:TextBox ID="txtRemarks" runat="server" placeHolder="Input remarks" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                </td>

                            </tr>
                        </table>
                    </div>
                    <div style="float: right; position: static; padding-right: 15px; margin-top: 10px;">

                        <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Enabled="false" Text="Export" OnClick="ibtnAdd_Click"></asp:Button>
                    </div>
                    <div class="panel-heading">
                        <h3 class="panel-title">List of Policies Expiring [<asp:Label ID="lblRecords" runat="server"></asp:Label>]</h3>

                    </div>
                    <div style="overflow-x: scroll; height: 400px; width: 100%;">
                        <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                            OnRowCommand="gv_valid_RowCommand" OnRowDataBound="gv_valid_RowDataBound" OnPageIndexChanging="gv_valid_PageIndexChanging">
                            <SelectedRowStyle BackColor="#EEFFAA" />

                            <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />

                            <Columns>
                                <asp:TemplateField HeaderText="No" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPolicyID" runat="server" Text='<%#Eval("Policy_ID") %>'></asp:Label>
                                        <asp:Label ID="lblChannelItemId" runat="server" Text='<%#Eval("Channel_item_id") %>'></asp:Label>
                                        <asp:Label ID="lblChannelLocationId" runat="server" Text='<%#Eval("Channel_location_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("Branch_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application ID" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApplicationID" runat="server" Text='<%#Eval("application_id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CIF" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCIF" runat="server" Text='<%#Eval("CIF") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Policy No." Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPolicyNumber" runat="server" Text='<%#Eval("Policy_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cust. No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustNo" runat="server" Text='<%# Eval("customer_number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustNameEn" runat="server" Text='<%# Eval("full_name_en") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustNameKh" runat="server" Text='<%# Eval("Full_name_kh") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gender">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DOB" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOB" runat="server" Text='<%# Convert.ToDateTime( Eval("date_of_birth")).ToString("dd-MM-yy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Age">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerAge" runat="server" Text='<%# Calculation.Culculate_Customer_Age( Convert.ToDateTime( Eval("date_of_birth")).ToString("dd/MM/yyyy"),DateTime.Now) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustContactNo" runat="server" Text='<%# Eval("contact_number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Product">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("Product_Name") %>'></asp:Label>
                                        <asp:HiddenField ID="hdfProductId" runat="server" Value='<%# Eval("Product_id") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sum Assure">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSumAssure" runat="server" Text='<%# Eval("Sum_Assure") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Effective Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime( Eval("Effective_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Convert.ToDateTime( Eval("Expiry_Date")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry In" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpiryIn" runat="server" Text='<%# Eval("Expiry_In") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry In">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpiryInCurrent" runat="server" Text='<%# Eval("Expiry_In_Current") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Policy Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("policy_status") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Renewal Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="App. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewApplicationNumber" runat="server" Text='<%# Eval("new_application_number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Channel">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChannel" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Agent Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("Agent_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Agent Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("Agent_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Referrer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReferrer" runat="server" Text='<%# Eval("Referrer") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ibtnApp" runat="server" Text="Renew" Style="display: none; color: blue; padding: 2px 2px 2px 2px; font-family: Calibri; font-size: 14px;" CommandName="CMD_RENEW" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                        <asp:LinkButton ID="ibtnUpdate" runat="server" Text="Update" Style="color: blue; padding: 2px 2px 2px 2px; font-family: Calibri; font-size: 14px;" CommandName="CMD_UPDATE" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </div>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:HiddenField ID="hdfChannelItemID" runat="server" Value="" />
                    <asp:HiddenField ID="hdfChannelLocationID" runat="server" Value="" />
                    <asp:HiddenField ID="hdfSaleAgentID" runat="server" Value="" />
                    <asp:HiddenField ID="hdfSaleAgentName" runat="server" Value="" />
                    <asp:HiddenField ID="hdfPolicyId" runat="server" Value="" />
                    <asp:HiddenField ID="hdfOldStatus" runat="server" Value="" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>


        </asp:UpdatePanel>

    </div>
</asp:Content>

