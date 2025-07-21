<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="group_micro_policy_detail_req.aspx.cs" Inherits="Pages_Reports_group_micro_policy_detail_req" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <style>
           .CheckboxList input {
            float: left;
            clear: both;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="15000"></asp:ScriptManager>
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
                    <h3 class="panel-title">Group Micro Policy Detail</h3>

                </div>
                <div class="panel-body">
                    <table style="width: auto;" border="0">
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
                        </tr>
                       
                    </table>
                    <table style="width:100%;" border="0">
                        <tr>
                             <td>Policy Status:</td>
                            <td  style="border: 1px solid #21275b; text-align:left;">
                               <asp:CheckBoxList runat="server" ID="cblStatus" AppendDataBoundItems="true"  RepeatDirection="Horizontal" RepeatColumns="6" CssClass="CheckboxList" Width="100%"  CellSpacing="5" AutoPostBack="true" OnSelectedIndexChanged="cblStatus_SelectedIndexChanged">
                               </asp:CheckBoxList>

                            </td>
                             <td> <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" /></td>
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
                        OnPageIndexChanging="gv_valid_PageIndexChanging" OnSorting="gv_valid_Sorting" OnRowCommand="gv_valid_RowCommand"> 
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
                            <asp:TemplateField HeaderText="App. Number">
                                <ItemTemplate>
                                 <%--   <asp:Label ID="lblApplicationNumber" Width="160px" runat="server" Text='<%# Eval("ApplicationNumber") %>'></asp:Label>--%>
                                    <asp:Label ID="lblApplicationId" Width="160px" Visible="false" runat="server" Text='<%# Eval("ApplicationId") %>'></asp:Label>
                                    <asp:LinkButton runat="server" Width="165px" ID="lbtApplication" Style="color: blue;" Text='<%# Eval("ApplicationNumber")  %>' CommandName="CMD_PRINT_APP" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  ToolTip="Click to view application form"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Policy Number">
                                <ItemTemplate>
                                   <%-- <asp:Label ID="lblPolicyNumber" Width="130px" runat="server" Text='<%# Eval("PolicyNumber") %>'></asp:Label>--%>
                                    <asp:Label ID="lblPolicyId" Width="160px" Visible="false" runat="server" Text='<%# Eval("PolicyId") %>'></asp:Label>
                                    <asp:LinkButton runat="server" Width="135px" ID="lbtPolicy" Style="color: blue;" Text='<%# Eval("PolicyNumber")  %>' CommandName="CMD_PRINT_POL" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Click to view certificate" ></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="AgentCode">
                                <ItemTemplate>
                                    <asp:Label ID="lblSaleAgentId" Width="110px" runat="server" Text='<%# Eval("SaleAgentId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AgentName">
                                <ItemTemplate>
                                    <asp:Label ID="lblSaleAgentName" Width="150px" runat="server" Text='<%# Eval("SaleAgentNameEn") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CustomerID">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerId" Width="135px" runat="server" Text='<%# Eval("CustomerNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CustomerName">
                                <ItemTemplate>
                                    <asp:Label ID="lblFullNameEn" Width="130px" runat="server" Text='<%# Eval("FullNameEn") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CustomerName">
                                <ItemTemplate>
                                    <asp:Label ID="lblFullNameKh" Width="130px" runat="server" Text='<%# Eval("FullNameKh") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IdType">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdType" Width="130px" runat="server" Text='<%# Eval("IdTypeEn") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IdNumber">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdNumber" Width="100px" runat="server" Text='<%# Eval("IdNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" Width="60px" runat="server" Text='<%# Eval("GenderEn") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DOB">
                                <ItemTemplate>
                                    <asp:Label ID="lblDob" Width="90px" runat="server" Text='<%# Convert.ToDateTime( Eval("DateOfBirth")).ToString("dd-MMM-yyyy")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="ProductId">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductId" Width="90px" runat="server" Text='<%#  Eval("ProductId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Package">
                                <ItemTemplate>
                                    <asp:Label ID="lblPackage" Width="90px" runat="server" Text='<%#  Eval("Package") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="SumAssure">
                                <ItemTemplate>
                                    <asp:Label ID="lblSumAssure" Width="80px" runat="server" Text='<%#  Eval("SumAssure") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Premium">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremium" Width="80px" runat="server" Text='<%#  Eval("Premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="AnnualPremium">
                                <ItemTemplate>
                                    <asp:Label ID="lblAnnualPremium" Width="90px" runat="server" Text='<%#  Eval("AnnualPremium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Basic Dist.">
                                <ItemTemplate>
                                    <asp:Label ID="lblBasicDiscountAmount" Width="90px" runat="server" Text='<%#  Eval("BasicDiscountAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Basic Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblBasicTotalAmount" Width="90px" runat="server" Text='<%#  Eval("BasicTotalAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="PayMode">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayMode" Width="100px" runat="server" Text='<%# Eval("PayModeEn") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IssueAge">
                                <ItemTemplate>
                                    <asp:Label ID="lblAge" Width="50px" runat="server" Text='<%# Eval("IssueAge") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EffectiveDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("EffectiveDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExpiryDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("ExpiryDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Ben.Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenName" Width="100px" runat="server" Text='<%# Eval("BenFullName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Relation">
                                <ItemTemplate>
                                    <asp:Label ID="lblRelation" Width="100px" runat="server" Text='<%# Eval("Relation") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Percentage">
                                <ItemTemplate>
                                    <asp:Label ID="lblPercentate" Width="70px" runat="server" Text='<%# Eval("PercentageShared") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                           
                              <asp:TemplateField HeaderText="PolicyStatus">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatus" Width="70px" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PolicyStatusDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatusDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("PolicyStatusDate")).Year==2000? "": Convert.ToDateTime( Eval("PolicyStatusDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
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

