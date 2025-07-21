<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="micro_policy_insurance_rp.aspx.cs" Inherits="Pages_Reports_micro_policy_insurance_rp" %>

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
    <%-- <script>
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
                    <h3 class="panel-title">Search Micro Policy Insurance</h3>

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
                                <asp:Label ID="lblPolicyStatus" runat="server" Text="Policy Status:" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblApplicationNumber" runat="server" Text="Application Number:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlPolicyStatus" runat="server" CssClass="span2" Style="display: none;">
                                    <asp:ListItem Text="---Select---" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Inforce" Value="IF"></asp:ListItem>
                                    <asp:ListItem Text="Termiate" Value="TER"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtApplicationNumber" runat="server" placeholder="APPxxxxxxx" CssClass="span2"></asp:TextBox>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" CssClass="span2">
                                </asp:DropDownList>

                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannelItem" runat="server" Text="Company:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged" CssClass="span2"></asp:DropDownList>
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
                            <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                            <asp:Button ID="btnExport" Text="Export" runat="server" OnClick="btnExport_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" /></div>
                        <h3></h3>
                    </h3>

                </div>
                                <div style="overflow-x: scroll; height: 400px; width: 100%;">
                    <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="50" runat="server" AutoGenerateColumns="False" Width="50%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                        OnRowDataBound="gv_valid_RowDataBound" OnPageIndexChanging="gv_valid_PageIndexChanging" OnSorting="gv_valid_Sorting" OnRowCommand="gv_valid_RowCommand" AllowSorting="true" >
                        <SelectedRowStyle BackColor="#EEFFAA" />
                        <PagerStyle HorizontalAlign="Left" CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationID" runat="server" Text='<%#Eval("application_id") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Company" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("channel_name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Code" Visible="true" SortExpression="office_code">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchCode" Width="50px" runat="server" Text='<%#Eval("office_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranch" Width="150px" runat="server" Text='<%#Eval("office_name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerNo" Width="100px" runat="server" Text='<%# Eval("customer_number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Full Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblFullNameKh" Width="100px" runat="server" Text='<%# Eval("full_name_in_khmer") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Full Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblFullNameEN" Width="100px" runat="server" Text='<%# Eval("full_name_in_english") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" Width="50px" runat="server" Text='<%# Eval("gender") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Type" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDType" Width="110px" runat="server" Text='<%# Eval("id_type") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDNo" Width="100px" runat="server" Text='<%# Eval("ID_number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DOB">
                                <ItemTemplate>
                                    <asp:Label ID="lblDOB" runat="server" Width="70px" Text='<%# Convert.ToDateTime( Eval("DATE_OF_BIRTH")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issued Age">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuedAge" Width="60px" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhoneNumber" Style="width: auto;" runat="server" Text='<%# Eval("Phone_number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" Width="100px" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Province">
                                <ItemTemplate>
                                    <asp:Label ID="lblProvince" Width="100px" runat="server" Text='<%# Eval("Province_kh") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%-- <asp:TemplateField HeaderText="Application No.">
                        <ItemTemplate>
                            <asp:Label ID="lblApplicationNo"  Width="100px" runat="server" Text='<%# Eval("application_number")  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Application No.">
                                <ItemTemplate>

                                    <asp:LinkButton runat="server" ID="lbtApplication" Style="color: blue;" Text='<%# Eval("application_number")  %>' CommandName="CMD_EDIT" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Policy No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyNo" Width="100px" runat="server" Text='<%#  Eval("policy_number")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductID" runat="server" Width="70px" Text='<%# Eval("product_id") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductName" runat="server" Width="50px" Text='<%# Eval("product_name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sum Assure">
                                <ItemTemplate>
                                    <asp:Label ID="lblSumAssure" runat="server" Width="50px" Text='<%# Eval("sum_assure") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Premium" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremium" runat="server" Width="50px" Text='<%# Eval("premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Discount Amount" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscountAmount" runat="server" Width="50px" Text='<%# Eval("discount_amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DHC" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDHC" runat="server" Width="50px" Text='<%# Eval("rider_sum_assure") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DHC Premium" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblRider" runat="server" Width="50px" Text='<%# Eval("rider_premium") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DHC Discount Amount" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblRiderDiscountAmount" runat="server" Width="50px" Text='<%# Eval("rider_discount_amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Amount" Visible="true" SortExpression="total_amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Width="50px" Text='<%# Eval("Total_AMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Paymode" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymode" runat="server" Width="50px" Text='<%# Eval("mode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issued Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuedDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("ISSUED_DATE")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("EFFECTIVE_DATE")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Maturity Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaturity" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("MATURITY_DATE")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expiry Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" runat="server" Width="100px" Text='<%# Convert.ToDateTime( Eval("expiry_DATE")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Policyholder" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyholder" runat="server"  Text='<%# Eval("POLICYHOLDER_NAME") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Primary Beneficiary's Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrimaryBenName" runat="server"  Text='<%# Eval("PRIMARY_BENEFICIARY_NAME") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Contingent Benefitciary's Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenName" runat="server"  Text='<%# Eval("BENEFICIARY_NAME") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyStatus" runat="server" Width="50px" Text='<%# Eval("POLICY_STATUS").ToString()=="IF" ? "Inforce":Eval("POLICY_STATUS").ToString()=="TER" ? "Terminate":Eval("POLICY_STATUS").ToString()=="CAN" ? "Cancel":Eval("POLICY_STATUS").ToString()=="EXP" ? "Expire":"" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                               <asp:TemplateField HeaderText="Payment Reference No." Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentReferenceNo" runat="server"  Text='<%# Eval("TRANSACTION_REFERRENCE_NO") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Agent Code" Visible="true" SortExpression="Agent_code">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgentcode" runat="server" Width="50px" Text='<%# Eval("Agent_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agent Name" Visible="true" SortExpression="Agent_Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgentName" runat="server" Width="100px" Text='<%# Eval("Agent_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Remarks" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblreamrks" runat="server" Width="50px" Text='<%# Eval("POLICY_STATUS_REMARKS") %>'></asp:Label>
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



