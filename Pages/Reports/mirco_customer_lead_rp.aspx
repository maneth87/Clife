<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="mirco_customer_lead_rp.aspx.cs" Inherits="Pages_Reports_mirco_customer_lead_rp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
      <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
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
                    <h3 class="panel-title">Search Client Lead</h3>

                </div>
                <div class="panel-body">
                    <table style="width: 100%;" border="0">
                        <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblReferredDateF" runat="server" Text="Referred Date From:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtReferredDateFrom" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                <span class="star">*</span>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblReferredDateTo" runat="server" Text="To:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtReferredDateTo" runat="server" placeholder="DD-MM-YYYY" CssClass="datepicker"></asp:TextBox>
                                <span class="star">*</span>
                            </td>

                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="0025E613-4A0D-43E5-B6B8-BDE9A4A005EE" Text="Individual"></asp:ListItem>
                                    <asp:ListItem Value="0152DF80-BA95-46A9-BB7A-E71966A34089" Text="Corporate"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                           <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblPolStatus" runat="server" Text="Policy Status:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;" colspan="9">                             
                                <table style="width: 100%; border: 1px solid gray" id="tblpolicy" runat="server">
                                    <tr>
                                        <td style="padding:10px;">

                                            <asp:CheckBoxList ID="cblpolicystatus" runat="server" RepeatDirection="Horizontal" RepeatColumns="12" CssClass="CheckboxList" Width="100%" CellSpacing="5" AutoPostBack="true" OnSelectedIndexChanged="cblpolicystatus_SelectedIndexChanged">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>                      
                                                <asp:ListItem Text="Inforce" Value="IF"></asp:ListItem>
                                                <asp:ListItem Text="Terminate" Value="TER"></asp:ListItem>
                                                <asp:ListItem Text="Cancel" Value="CAN"></asp:ListItem>
                                                <asp:ListItem Text="Expire" Value="EXP"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                        <tr>

                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelLocation" runat="server" Text="Branch Code:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;" colspan="7">
                                <%--<asp:DropDownList ID="ddlChannelLocation" runat="server"></asp:DropDownList>--%>
                                <table style="width: 100%; border: 1px solid gray;" id="tblUser" runat="server">
                                    <tr>
                                        <td style="padding: 5px;">

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
                        <asp:Label ID="lblRecords" runat="server"></asp:Label>
                        ]
                        <div style="float: right;">
                            <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" /><asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" /></div>
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
                            <asp:TemplateField HeaderText="Company" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCompany" runat="server" Text='<%#Eval("channel_name") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Code" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("BRANCH_CODE") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchName" runat="server" Text='<%#Eval("BRANCH_NAME") %>' Width="150px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationID" runat="server" Text='<%# Eval("APPLICATION_ID") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffID" runat="server" Text='<%# Eval("REFERRAL_STAFF_ID") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffName" runat="server" Text='<%# Eval("REFERRAL_STAFF_NAME") %>' Width="120px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff Position">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffPosition" runat="server" Text='<%# Eval("REFERRAL_STAFF_POSITION") %>' Width="120px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientType" runat="server" Text='<%# Eval("CLIENT_TYPE") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CIF">
                                <ItemTemplate>
                                    <asp:Label ID="lblCIF" runat="server" Text='<%# Eval("CIF") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Client Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientNameEnglish" runat="server" Text='<%# Eval("CLIENT_NAME_IN_ENGLISH") %>' Width="120px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientNameKhmer" runat="server" Text='<%# Eval("CLIENT_NAME_IN_KHMER") %>' Width="120px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("GENDER") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nationality">
                                <ItemTemplate>
                                    <asp:Label ID="lblNationality" runat="server" Text='<%# Eval("NATIONALITY") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of Birth">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Convert.ToDateTime( Eval("DATE_OF_BIRTH")).ToString("dd-MMM-yyyy") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Village">
                                <ItemTemplate>
                                    <asp:Label ID="lblVillage" runat="server" Text='<%# Eval("VILLAGE") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Commune">
                                <ItemTemplate>
                                    <asp:Label ID="lblCommune" runat="server" Text='<%# Eval("COMMUNE") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="District">
                                <ItemTemplate>
                                    <asp:Label ID="lblDistrict" runat="server" Text='<%# Eval("DISTRICT") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Province">
                                <ItemTemplate>
                                    <asp:Label ID="lblProvince" runat="server" Text='<%# Eval("PROVINCE") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDType" runat="server" Text='<%# Eval("ID_TYPE") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDNumber" runat="server" Text='<%# Eval("ID_NUMBER") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PHONE_NUMBER") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interest" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblInterest" runat="server" Text='<%# Eval("INTEREST") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referred Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferredDate" runat="server" Text='<%# Convert.ToDateTime( Eval("REFERRED_DATE")).ToString("dd-MM-yyyy") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issued Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuedDate" runat="server" Text='<%# Convert.ToDateTime( Eval("ISSUED_DATE")).ToString("dd-MM-yyyy")=="01-01-1900"? "": Convert.ToDateTime( Eval("ISSUED_DATE")).ToString("dd-MM-yyyy") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("STATUS") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusRemarks" runat="server" Text='<%# Eval("STATUS_REMARKS") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Insurance Application No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuranceApplicationNumber" runat="server" Text='<%# Eval("INSURANCE_APPLICATION_NUMBER") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Policy Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblpolicyStatus" runat="server" Text='<%# Eval("POLICY_STATUS").ToString()=="IF" ? "Inforce":Eval("POLICY_STATUS").ToString()=="TER" ? "Terminate":Eval("POLICY_STATUS").ToString()=="CAN" ? "Cancel":Eval("POLICY_STATUS").ToString()=="EXP" ? "Expire":"" %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agent Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("agent_code") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agent Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("agent_name_en") %>' Width="100px"></asp:Label>
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


