<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="banca_customer_lead.aspx.cs" Inherits="Pages_Business_banca_customer_lead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
      <%-- Search in dropdownlist --%>
    <script src="../../Scripts/jquery.1.8.3.jquery.min.js"></script>
    <script src="../../Scripts/jquery.searchabledropdown-1.0.8.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <%--  <meta http-equiv="refresh" content="60" />--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <style>
        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 250%;
            background-color: #ababab;
            z-index: 9999999;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=90);
        }

        #modal {
            display: none;
            position: absolute;
            top: 40%;
            left: 45%;
            width: 100px;
            height: 100px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 9999999;
            text-align: center;
            overflow: auto;
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
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }

        .CheckboxList {
            float: left;
            clear: both;
            display: inline;
        }

        .row_header {
            background-color: #21275b;
            color: white;
            font-weight: bold;
          
        }
        .col_space {
            margin-left:20px;
        }
        .gridrow-align {
            vertical-align:middle; text-align:center; background-color:red;
        }
    </style>

    <script>
       

        function loading() {

            $("#modal").show();
            $("#fade").show();

        };
        function Endloading() {

            $("#modal").hide();
            $("#fade").hide();

        };
        function confirm_save() {
            if (confirm('Confirm to save!')) {
                loading();
            }
            {
                return false;
            }
        };
        function pageLoad(sender, args) {
            $(document).ready(function () {
                $("#<%=ddlProvince.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });

                $("#<%=ddlDistrict.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });

                $("#<%=ddlCommune.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });

                $("#<%=ddlVillage.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });

                $("#<%=ddlNationality.ClientID%>").searchable({
                    maxListSize: 200, // if list size are less than maxListSize, show them all
                    maxMultiMatch: 300, // how many matching entries should be displayed
                    exactMatch: false, // Exact matching on search
                    wildcards: true, // Support for wildcard characters (*, ?)
                    ignoreCase: true, // Ignore case sensitivity
                    latency: 200, // how many millis to wait until starting search
                    warnMultiMatch: 'top {0} matches ...',
                    warnNoMatch: 'no matches ...',
                    zIndex: 'auto'
                });
            });

        }
    </script>

    <div id="fade"></div>
    <div id="modal">
        <img id="loader" src="../../App_Themes/functions/loading.gif" alt="" /><br />
        <span>Processing data...</span>
    </div>

    <ul class="toolbar">

        <%--<li>
            <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="ibtnSave_Click" OnClientClick="loading();" />
        </li>
        <li>
            <asp:ImageButton ID="ibtnClear" runat="server" ImageUrl="~/App_Themes/functions/clear.png" OnClick="ibtnClear_Click" />
        </li>--%>
    </ul>
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
    <asp:UpdatePanel ID="UP" runat="server" >
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Search Client Lead</h3>

                </div>
                <div class="panel-body">
                    <div style="border:2px solid #21275b; border-radius:15px; padding:10px; ">
                    <table style="width: auto;  " border="0" >
                            <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblChannel" runat="server" Text="Channel:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;" colspan="5">
                                <asp:DropDownList ID="ddlChannel" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                           
                        </tr>
                        <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblBranchName" runat="server" Text="Branch Name:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;" colspan="5">
                                <asp:DropDownList ID="ddlBranchName" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchName_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblBranchcode" runat="server" Text="Branch Code:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtBranchCode" CssClass="span2" runat="server"></asp:TextBox>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblCustomerNameEnglish" runat="server" Text="Customer Name (English):"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtCustomerNameEnglish" CssClass="span3" runat="server"></asp:TextBox>
                            </td>
                             <td style="padding-right: 20px;">
                                <asp:Label ID="lblIDNumber" runat="server" Text="ID Number:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtIDNumber" CssClass="span2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblGender" runat="server" Text="Gender:"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:DropDownList ID="ddlGender" CssClass="span2" runat="server">
                                    <asp:ListItem Value="" Text=".">                        
                                    </asp:ListItem>
                                    <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
                                    <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:Label ID="lblDateOfBirth" runat="server" Text="Date of Birth (DD-MM-YYYY):"></asp:Label>
                            </td>
                            <td style="padding-right: 20px;">
                                <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="span2"></asp:TextBox>

                            </td>
                           
                            <td colspan="2" style="text-align:right; padding-right:20px;">
                                 <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                                <asp:Button ID="btnRefresh" Text="Refresh" runat="server" OnClick="btnRefresh_Click" CssClass="btn btn-primary" Style="margin-left: 30px;" />
                            </td>
                        </tr>

                    </table>
                        </div>
                </div>
                <div style="float: right; position: static; padding-right: 15px; margin-top: 10px;">
                    <asp:CheckBox ID="ckbShowAddress" runat="server" Style="margin-right: 5px; margin-bottom: 20px;" Text="" /><span">Show Address</span>
                    <span style="margin-right:20px; margin-left:20px;">|</span>
                    <asp:Label ID="lblTopTow" runat="server" Text="Show Last Records:"></asp:Label>
                    <asp:TextBox ID="txtTopRow" Width="30px" runat="server"></asp:TextBox>
                      <span style="margin-right:20px; margin-left:20px;">|</span>
                     <asp:LinkButton ID="ibtnAdd" runat="server" Text="ADD NEW" OnClick="ibtnAdd_Click"></asp:LinkButton>
                </div>
                <div class="panel-heading">

                    <h3 class="panel-title">List of Client Lead  (<asp:Label ID="lblRecords" runat="server"></asp:Label>)</h3>

                </div>

                <div style="overflow-x: scroll; height: auto; width: 100%;" id="divList" runat="server">
                    <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                        OnRowCommand="gv_valid_RowCommand" OnRowDataBound="gv_valid_RowDataBound" OnPageIndexChanging="gv_valid_PageIndexChanging" OnSelectedIndexChanged="gv_valid_SelectedIndexChanged">
                        <SelectedRowStyle BackColor="#EEFFAA" />
                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                       
                        <Columns>
                            <asp:TemplateField HeaderText="No" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="GridRowCenter"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Code" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchCode" runat="server" Text='<%#Eval("BranchCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchName" runat="server" Text='<%#Eval("BranchName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationID" runat="server" Text='<%# Eval("ApplicationId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffID" runat="server" Text='<%# Eval("ReferralStaffId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffName" runat="server" Text='<%# Eval("ReferralStaffName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referral Staff Position">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferralStaffPosition" runat="server" Text='<%# Eval("ReferralStaffPosition") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientType" runat="server" Text='<%# Eval("ClientType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CIF">
                                <ItemTemplate>
                                    <asp:Label ID="lblCIF" runat="server" Text='<%# Eval("ClientCIF") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Client Name (English)">
                                <ItemTemplate>
                                    <asp:Label ID="lblClientNameEnglish" runat="server" Text='<%# Eval("ClientNameENG") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client Name (Khmer)" ControlStyle-Font-Names="Khmer OS Content" ControlStyle-Font-Size="10">
                                <ItemTemplate >
                                    <asp:Label ID="lblClientNameKhmer" runat="server" Text='<%# Eval("ClientNameKHM") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("ClientGender") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nationality">
                                <ItemTemplate>
                                    <asp:Label ID="lblNationality" runat="server" Text='<%# Eval("ClientNationality") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of Birth">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfBirth" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("ClientDoB")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Village">
                                <ItemTemplate>
                                    <asp:Label ID="lblVillage" runat="server" Text='<%# Eval("ClientVillage") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Commune">
                                <ItemTemplate>
                                    <asp:Label ID="lblCommune" runat="server" Text='<%# Eval("ClientCommune") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="District">
                                <ItemTemplate>
                                    <asp:Label ID="lblDistrict" runat="server" Text='<%# Eval("ClientDistrict") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Provice">
                                <ItemTemplate>
                                    <asp:Label ID="lblProvince" runat="server" Text='<%# Eval("ClientProvince") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDType" runat="server" Text='<%# Eval("DocumentType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDNumber" runat="server" Text='<%# Eval("DocumentId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("ClientPhoneNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interest" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblInterest" runat="server" Text='<%# Eval("Interest") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referred Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferredDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("ReferredDate")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Width="100px" Text='<%# Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status Remarks" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusRemarks" runat="server" Text='<%# Eval("StatusRemarks") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Remarks" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Updated By" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdatedBy" runat="server" Text='<%# Eval("UpdatedBy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Updated On" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdatedOn" runat="server" Text='<%# Eval("UpdatedOn") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="App No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuranceApplicationNumber" runat="server" Text='<%# Eval("InsuranceApplicationId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lead Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblLeadType" runat="server" Text='<%# Eval("LeadType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="API USER ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblApiUserId" runat="server" Text='<%# Eval("ApiUser") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ACTION" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="ibtnApp" ToolTip="Convert To Application" runat="server" Text="C" Style="color: blue; font-weight:bolder; " CommandName="CMD_COPY" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ></asp:LinkButton>
                                    <span style="margin-left:2px; margin-right:2px;">|</span>
                                    <asp:LinkButton ID="ibtnUpdate" ToolTip="Update Lead Status"  runat="server" Text="U" Style="color: red;  font-weight:bolder;" CommandName="CMD_UPDATE" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                    <span style="margin-left:2px; margin-right:2px;">|</span>
                                    <asp:LinkButton ID="ibtnEdit" ToolTip="Edit Lead"  runat="server" Text="E" Style="color:green; font-weight:bolder; " CommandName="CMD_EDIT" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
                 <div class="panel panel-default" id="divAdd" runat="server">

                        <div class="panel-heading">
                            <h3 class="panel-title">ADD NEW / UPDATE </h3>

                        </div>

                        <div class="panel-body"  id="divAddContent">
                           
                                <table id="tblAddContent" border="0" style="border:1px solid #21275b; " >
                                    <tr>
                                        <td colspan="8" class="row_header">Branch Information</td>
                                    </tr>
                                    <tr>
                                        <td>Channel:</td>
                                        <td colspan="2">
                                        <asp:DropDownList ID="ddlChannelNameAdd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelNameAdd_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                        <td>Branch Name:</td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlBranchNameAdd" CssClass="span6" runat="server" AutoPostBack ="true" OnSelectedIndexChanged="ddlBranchNameAdd_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td></td>
                                   
                                            </tr>
                                    <tr>
                                        <td colspan="8" class="row_header">Referral Information</td>
                                    </tr>
                                    <tr>
                                        
                                        <td>Referral Staff ID: <span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox  runat="server" CssClass="span2"  ID="txtReferralStaffId" placeholder="Referral ID / No."></asp:TextBox>
                                        </td>
                                         <td>Referral Staff Name:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferralStaffName"  CssClass="span2"  placeholder="Referrer Name" runat="server"></asp:TextBox>
                                        </td>
                                          <td> <span  class="col_space">Referral Staff Position: </span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReferralStaffPosition"  CssClass="span2"  runat="server"  placeholder="Referrer Position" ></asp:TextBox>
                                        </td>
                                         <td>
                                           <span  class="col_space">Referrerd Date:</span><span style="color: red; font-weight: bold;">*</span>
                                      </td>
                                      <td><asp:TextBox ID="txtReferredDate" CssClass="span2"  runat="server" placeholder="DD/MM/YYYY" ></asp:TextBox></td>
                                    </tr>
                                     <tr>
                                        <td colspan="8" class="row_header">Client Information</td>
                                    </tr>
                                    <tr>
                                       <td>Application ID: <span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtApplicationId" CssClass="span2"  runat="server" placeHolder="Auto"></asp:TextBox>
                                        </td>
                                         <td> <span  class="col_space">Client Type: </span><span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:DropdownList ID="ddlClientType"  CssClass="span2"  runat="server">
                                                <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Borrower" Value="Borrower"></asp:ListItem>
                                                <asp:ListItem Text="Saver" Value="Saver"></asp:ListItem>
                                                <asp:ListItem Text="Family" Value="Family"></asp:ListItem>
                                            </asp:DropdownList>
                                        </td>
                                         <td> <span  class="col_space">CIF:</span>
                                        </td>
                                         <td>
                                            <asp:TextBox ID="txtCif" placeholder="Customer Identify No."  CssClass="span2"  runat="server" ></asp:TextBox>
                                        </td>
                                       <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        
                                       
                                         <td>Last Name (EN):<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtClientLastNameEN"  placeHolder="Last name in English" CssClass="span2"  runat="server" ></asp:TextBox>
                                            </td>
                                       
                                            <td>
                                            <span class="col_space">First Name (EN):</span><span style="color: red; font-weight: bold;">*</span>
                                                </td>
                                            <td>
                                            <asp:TextBox ID="txtClientFirstNameEn" placeHolder="First name in English" CssClass="span2" runat="server"></asp:TextBox>
                                                
                                            </td>
                                            <td>
                                                <span class="col_space">Last Name (KH):</span> <span style="color: red; font-weight: bold;">*</span>
                                            </td>
                                            <td>
                                            <asp:TextBox ID="txtClientLastNameKh" placeHolder="Last name in khmer" CssClass="span2"  runat="server" style="font-family:'Khmer OS Content'; font-size:9pt; height:20px; " ></asp:TextBox>
                                                    
                                            </td>
                                        <td>
                                            <span class="col_space">First Name (KH):</span><span style="color: red; font-weight: bold;">*</span>
                                            </td>
                                        <td>
                                            <asp:TextBox ID="txtClientFirstNameKh" placeHolder="First name in khmer"  CssClass="span2"  runat="server" style="font-family:'Khmer OS Content'; font-size:9pt; height:20px; "></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                      
                                         <td> <span  class="col_space">Gender: </span><span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:DropdownList ID="ddlGenderAdd"  CssClass="span2"  runat="server" >
                                                <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
                                                <asp:ListItem Text="F" Value="Female"></asp:ListItem>
                                                <asp:ListItem Text="M" Value="Male"></asp:ListItem>
                                            </asp:DropdownList>
                                        </td>
                                          <td> <span  class="col_space">Nationality:</span><span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td>
                                            <asp:DropdownList ID="ddlNationality"  CssClass="span2"  runat="server" ></asp:DropdownList>
                                        </td>
                                         <td> <span  class="col_space">Date Of Birth:</span><span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtDateOfBirthAdd"  CssClass="span2" placeholder="DD/MM/YYYY" runat="server" ></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>ID Type:<span style="color: red; font-weight: bold;">*</span>
                                        </td>
                                        <td >
                                            <asp:DropdownList ID="ddlIdType"  CssClass="span2"  runat="server" >
                                                   <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="0">ID Card</asp:ListItem>
                                        <asp:ListItem Value="1">Passport</asp:ListItem>
                                        <asp:ListItem Value="3">Birth Certificate</asp:ListItem>
                                        <asp:ListItem Value="4">Family Book</asp:ListItem>
                                        <asp:ListItem Value="5">Proof of Resident</asp:ListItem>
                                        <asp:ListItem Value="6">Certificate of Identity</asp:ListItem>
                                            </asp:DropdownList>
                                        </td>
                                       <td><span class="col_space" >ID No.:</span><span style="color: red; font-weight: bold;">*</span></td>
                                        <td><asp:TextBox ID="txtIdNo"  CssClass="span2" runat="server"></asp:TextBox></td>
                                         <td>
                                         <span class="col_space">Phone Number:</span> 
                                      </td>
                                      <td><asp:TextBox ID="txtPhoneNumber"  CssClass="span2"  runat="server"></asp:TextBox></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Address Provice:<span style="color: red; font-weight: bold;">*</span></td>
                                        <td ><asp:DropDownList ID="ddlProvince" CssClass="span2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" style="font-family:'Khmer OS Content'; font-size:9pt; "></asp:DropDownList>
                                            </td>
                                        <td>
                                            <span  class="col_space">District:</span>
                                            </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDistrict"  CssClass="span2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" style="font-family:'Khmer OS Content'; font-size:9pt; "></asp:DropDownList>
                                            </td>
                                        <td>
                                            <span class="col_space">
                                                Commune:

                                            </span>
                                            </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCommune" CssClass="span2"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCommune_SelectedIndexChanged" style="font-family:'Khmer OS Content'; font-size:9pt; "></asp:DropDownList>
                                            </td>
                                        <td>
                                            <span class="col_space">
                                                Village:
                                            </span>
                                            </td>
                                        <td>
                                            <asp:DropDownList ID="ddlVillage" CssClass="span2"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVillage_SelectedIndexChanged" style="font-family:'Khmer OS Content'; font-size:9pt; "></asp:DropDownList>
                                        </td>
                                         
                                    </tr>
                                    <tr>
                                        <td>Remarks:</td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="span10"></asp:TextBox>
                                        </td>
                                    </tr>
                                  <tr>
                                      <td colspan="8" style="height:1px;border-bottom: 1px solid #21275b; "></td>
                                  </tr>
                                    <tr>

                                        <td style="vertical-align: central; text-align: center; padding-top:10px; padding-bottom:10px; " colspan="8">
                                            <asp:Button ID="btnSave" Text="SAVE" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                             <asp:Button ID="btnDelete" Text="DELETE" runat="server" CssClass="btn btn-primary" OnClick="btnDelete_Click" />
                                             <asp:Button ID="btnCancel" Text="CLOSE" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>


                                </table>
                            
                        </div>

                    </div>
                <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>


