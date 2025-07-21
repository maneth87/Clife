<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_banca_insurance_booking_rp.aspx.cs" Inherits="Pages_Reports_banca_micro_insurance_booking_rp" %>

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
                    <h3 class="panel-title">Search Insurance Booking</h3>

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
                                <asp:Label ID="lblPackage" runat="server" Text="Package:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlPackage" runat="server" CssClass="span2">
                                </asp:DropDownList>
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
                            <td style="padding-right: 20px;" colspan="9">
                                <%-- <asp:DropDownList ID="ddlChannelLocation" runat="server"></asp:DropDownList>--%>
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
                        <asp:Label ID="lblRecords" runat="server"></asp:Label><asp:Label ID="lblTotalPremium" runat="server"></asp:Label>
                        ]
                        <div style="float: right;">
                            <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" /><asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" /></div>
                    </h3>

                </div>
                <div style="overflow-x: scroll; height: 450px; width: 100%;">
                    <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="50" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                        OnRowDataBound="gv_valid_RowDataBound" OnPageIndexChanging="gv_valid_PageIndexChanging" AllowSorting="true" OnSorting="gv_valid_Sorting">
                        <SelectedRowStyle BackColor="#EEFFAA" />

                        <PagerStyle HorizontalAlign="Left" CssClass="GridPager" />


                        <Columns>
                            <asp:TemplateField HeaderText="No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Branch Code" Visible="true" SortExpression="office_code">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("office_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchName" Width="150px" runat="server" Text='<%#Eval("office_name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationID" Width="100px" runat="server" Text='<%# Eval("APPLICATION_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientType" Width="100px" runat="server" Text='<%# Eval("CLIENT_TYPE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Certificate No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyNumber" Width="100px" runat="server" Text='<%# Eval("POLICY_NUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment Reference No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentReferenceNo" Width="110px" runat="server" Text='<%# Eval("PAYMENT_REFERENCE_NO") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffID" Width="100px" runat="server" Text='<%# Eval("REFERRAL_STAFF_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffName" Width="100px" runat="server" Text='<%# Eval("REFERRAL_STAFF_NAME") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff Position">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffPosition" Width="100px" runat="server" Text='<%# Eval("REFERRAL_STAFF_POSITION") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CIF">
                                <ItemTemplate>
                                    <asp:Label ID="lblCIF" runat="server" Width="60px" Text='<%# Eval("CIF") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Client Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientNameEnglish" Width="150px" runat="server" Text='<%# Eval("LAST_NAME_IN_ENGLISH") + " " +Eval("FIRST_NAME_IN_ENGLISH") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientNameKhmer" Width="150px" runat="server" Text='<%#  Eval("LAST_NAME_IN_KHMER") + " " +Eval("FIRST_NAME_IN_KHMER") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date of Birth">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfBirth" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("DATE_OF_BIRTH")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhoneNumber" runat="server" Width="100px" Text='<%# Eval("Phone_Number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Width="50px" Text='<%# Eval("GENDER").ToString()=="0" ? "Female" : "Male" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDType" runat="server" Width="100px" Text='<%# Eval("ID_TYPE_TEXT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDNumber" runat="server" Width="100px" Text='<%# Eval("ID_NUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Village">
                                <ItemTemplate>
                                    <asp:Label ID="lblVillage" runat="server" Width="100px" Text='<%# Eval("VILLAGE_EN") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Commune">
                                <ItemTemplate>
                                    <asp:Label ID="lblCommune" runat="server" Width="100px" Text='<%# Eval("COMMUNE_EN") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="District">
                                <ItemTemplate>
                                    <asp:Label ID="lblDistrict" runat="server" Width="100px" Text='<%# Eval("DISTRICT_EN") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Provice">
                                <ItemTemplate>
                                    <asp:Label ID="lblProvince" runat="server" Width="100px" Text='<%# Eval("PROVINCE_EN") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("EFFECTIVE_DATE")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Maturity Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaturity" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("MATURITY_DATE")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Insurance Tenor (Y)">
                                <ItemTemplate>
                                    <asp:Label ID="lblCoverYear" runat="server" Width="50px" Text='<%# Eval("TERM_OF_COVER") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Premium" Visible="true" SortExpression="amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Width="50px" Text='<%# Eval("AMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Width="50px" Text='<%# Eval("Currency") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Insurance Type" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuranceType" Width="100px" runat="server" Text='Micro Insurance'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Package Type" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPackage" runat="server" Width="60px" Text='<%# Eval("PACKAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Insurance Status" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuranceStatus" runat="server" Width="60px" Text='<%# Eval("POLICY_STATUS").ToString()=="IF" ? "Approved":"" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Fee" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralFee" runat="server" Width="50px" Text='<%# Eval("REFERRAL_FEE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Incentive" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralIncentive" runat="server" Width="50px" Text='<%# Eval("REFERRAL_INCENTIVE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IA Name" Visible="true" SortExpression="agent_name_en">
                                <ItemTemplate>
                                    <asp:Label ID="lblIAName" runat="server" Width="100px" Text='<%# Eval("AGENT_NAME_EN") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("REFERRED_DATE")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issued Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuedDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("ISSUED_DATE")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Policy Status" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblpolicyStatus" runat="server" Width="60px" Text='<%# Eval("POLICY_STATUS").ToString()=="IF" ? "Inforce":Eval("POLICY_STATUS").ToString()=="TER" ? "Terminate":Eval("POLICY_STATUS").ToString()=="CAN" ? "Cancel":Eval("POLICY_STATUS").ToString()=="EXP" ? "Expire":"" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Remarks" >
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyRemarks" runat="server" Width="100px" Text='<%#  Eval("POLICY_STATUS_REMARKS") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Policy Status Remarks" >
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatusRemarks" runat="server" Width="100px" Text='<%#  Eval("POLICY_STATUS_REMARK") %>'></asp:Label>
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

