<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frm_gmCustomer_upload.aspx.cs" Inherits="Pages_Business_frm_gmCustomer_upload" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <link href="../../App_Themes/msg.css" rel="stylesheet" />
    <link href="../../App_Themes/progress_dialog.css" rel="stylesheet" />
    <%-- Jtemplate --%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
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
    <script>

        //$(document).ready(function () {
        //    setInterval(function () { Counter() }, 1000);
        //});
        //var count = 0;
        //function Counter() {
        //    count++;
        //    $('#myTimeer').html(count);
        //}


        var marketing_code;
        var marketing_name;
        var total_agent_row;

        //Search Marketing Code (Search click)
        function SearchMarketingCode() {
            var sale_agent_name = $('#Main_txtAgentName').val();

            $.ajax({
                type: "POST",
                url: "../../SaleAgentWebService.asmx/GetSaleAgents",
                data: "{sale_agent_name:" + JSON.stringify(sale_agent_name) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#Main_hdfTotalAgentRow').val(data.d.length);

                    $('#dvAgentList').setTemplate($("#jTemplateSaleAgent").html());
                    $('#dvAgentList').processTemplate(data);
                }

            });

        }

        function FillMarketingCode() {

            if (marketing_code == "") {
                alert('Please select a checkbox');
                return;
            }

            $('#Main_txtSaleAgentID').val(marketing_code);
            $('#Main_txtSaleAgentName').val(marketing_name);
            $('#Main_hdfSaleAgentID').val(marketing_code);
            $('#Main_hdfSaleAgentName').val(marketing_name);
        };

        //Get Selected Sale Agent
        function GetAgent(AgentID, AgentName, row_index_agent_list) {

            if ($('#' + row_index_agent_list).is(':checked')) {
                marketing_code = AgentID;
                marketing_name = AgentName;
            } else {
                marketing_code = "";
                marketing_name = "";
            }

            var total_agent_row = $('#Main_hdfTotalAgentRow').val();
            // alert(total_agent_row);
            //Uncheck all other checkboxes
            for (var i = 1; i <= total_agent_row  ; i++) {
                if (i != row_index_agent_list) {
                    $('#' + i).prop('checked', false);
                }
            }
        };


        function ReConfirm()
        {
            var btn = $('#Main_btnUpload');
            
            return confirm('Confirm ' + btn.val() + '!');
          
        }
    </script>
    <%--Jtemplate Sections--%>
    <script id="jTemplateSaleAgent" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th style="border-width: thin; border-style: solid;">Sale Agent ID</th>
                    <th style="border-width: thin; border-style: solid;">Sale Agent Name</th>
                    <th style="border-width: thin; border-style: solid;"></th>
                </tr>
            </thead>
            <tbody>
                {#foreach $T.d as record}
                    <tr>
                        <td>{ $T.record.Sale_Agent_ID }</td>
                        <td>{ $T.record.Full_Name }</td>
                        <td>
                            <input id="{ $T.record.row_index }" type="checkbox" onclick="GetAgent('{ $T.record.Sale_Agent_ID }', '{ $T.record.Full_Name }', '{ $T.record.row_index }');" /></td>
                    </tr>
                {#/for}
            </tbody>
        </table>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="15000"></asp:ScriptManager>
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
                            <%--<span style="font-size:8pt; font-style:normal;"><span id="myTimeer"></span><span>.second(s)</span></span>--%>
                        </p>


                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UP" runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Upload Bundle Data</h3>
                    </div>
                    <div class="panel-body">
                        <table border="0">
                            <tr>
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
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlChannelItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelItem_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblChannelLocation" runat="server" Text="Branch:"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlChannelLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelLocation_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="lblProduct" runat="server" Text="Product:"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td rowspan="4" style="vertical-align: top;">
                                    <div style="border: solid 2px gray; padding: 10px; text-align: center; float: right;">
                                        <p style="font-style: italic;"><u>Download File Template</u></p>
                                        <a style="color: blue;" href="UPLOAD_LOAN_TEMPLATE.xlsx"><span>1.Loan</span></a><br />
                                         <a style="color: blue;" href="UPLOAD_INTERNATIONAL_MONEY_TRANSFTER_TEMPLATE.xlsx"><span>2.Internation Money Transfer</span></a><br />
                                         <a style="color: blue;" href="UPLOAD_PAYROLL_TEMPLATE.xlsx"><span>3.Payroll</span></a><br />
                                         <a style="color: blue;" href="UPLOAD_WING_DIGITAL_LOAN_TEMPLATE.xlsx"><span>4.Wing Digital Loan</span></a>
                                    </div>

                                </td>
                            </tr>
                            <tr>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="Label1" runat="server" Text="Payment Mode:"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlPaymentMode" runat="server"></asp:DropDownList>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="Label3" runat="server" Text="Sum Assure"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlBasicSumAssure" runat="server"></asp:DropDownList>
                                </td>

                                <td style="padding-right: 20px;">
                                    <asp:Label ID="Label4" runat="server" Text="Rider"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlProductRider" runat="server"></asp:DropDownList>

                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:Label ID="Label5" runat="server" Text="Rider Sum Assure"></asp:Label>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:DropDownList ID="ddlRiderSumAssure" runat="server"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" Text="Pay Period Type:" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPayPeriodType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPayPeriodType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" Text="Pay Period :" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPayPeriod" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" Text="Cover Period Type:" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCoverPeriodType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCoverPeriodType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" Text="Cover Period :" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCoverPeriod" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblProjectType" Text="Project Type:" runat="server"></asp:Label>

                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProjectType" runat="server"></asp:DropDownList>
                                </td>
                                <td style="text-align: left;" colspan="6">
                                    <asp:Label ID="lblSaleAgentName" runat="server" Text="Sale Agent Code / Name:"></asp:Label>
                                    <span style="color: red;">*</span>
                                    <asp:TextBox ID="txtSaleAgentID" runat="server" class="span2" Enabled="true" placeHolder="Input Agent Code" AutoPostBack="true" OnTextChanged="txtSaleAgentID_TextChanged"></asp:TextBox>
                                    <span>/ </span>
                                    <asp:TextBox ID="txtSaleAgentName" runat="server" class="span3" Enabled="false"></asp:TextBox>

                                    <input type="button" data-toggle="modal" data-target="#myMarketingCodeList" style="display: none; background: url('../../App_Themes/functions/search_icon.png') no-repeat; border: none; height: 25px; width: 25px;" />
                                </td>

                            </tr>
                            <tr>

                                <td>
                                    <asp:Label ID="lblfile" Text="Payment Report Date:" runat="server"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td style="padding-right: 20px;">
                                    <asp:TextBox ID="txtPaymentReportDate" runat="server" placeHolder="DD-MM-YYYY"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" Text="Select File:" runat="server"></asp:Label>
                                    <span style="color: red;">*</span>
                                </td>
                                <td colspan="3">
                                    <asp:FileUpload ID="fUpload" runat="server" />
                                </td>
                                <td colspan="2">
                                    <div style="float: right;">

                                        <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClientClick="ReConfirm();" OnClick="btnUpload_Click" CssClass="btn btn-primary" Style="margin-right: 5px; margin-top: 0px;" />
                                        <asp:Button runat="server" ID="btnSave" OnClientClick="return confirm('Convert to policy!');" Text="Convert to Policy" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                    </div>
                                </td>

                            </tr>

                        </table>


                    </div>
                    <div style="float: right; position: static; padding-right: 15px; margin-top: 10px;">
                        <asp:CheckBox ID="ckbShow" runat="server" Width="200px" AutoPostBack="true" OnCheckedChanged="ckbShow_CheckedChanged" Text="Show uploaded Records" CssClass="CheckboxList" />

                    </div>
                    <div class="panel-heading">
                        <h3 class="panel-title"> Result - 
                            <asp:Label ID="lblRecords" runat="server"></asp:Label></h3>
                    </div>
                    <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                        <li class="active"><a href="#div_valid" data-toggle="tab" style="color: green;">Valid</a></li>
                        <li><a href="#div_invalid" data-toggle="tab" style="color: red;">Invalid</a></li>

                    </ul>
                    <div id="my-tab-content" class="tab-content">
                        <div class="tab-pane active" id="div_valid">
                            <div style="overflow-x: scroll; height: 100%; width: 100%;">
                                <asp:GridView ID="gv_valid" CssClass="grid-layout" AllowPaging="true" AllowSorting="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                                    OnPageIndexChanging="gv_valid_PageIndexChanging" OnSorting="gv_valid_Sorting" OnRowDataBound="gv_valid_RowDataBound">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("LoanId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranch" Width="150px" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Client Name" Visible="true" SortExpression="ClientName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" Width="150px" runat="server" Text='<%#Eval("ClientName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gender" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGender" Width="50px" runat="server" Text='<%#Eval("GenderText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DOB">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDOB" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdType" Width="100px" runat="server" Text='<%# Eval("IdTypeText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdNumber" Width="100px" runat="server" Text='<%# Eval("IdNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Agreement Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgreementNumber" Width="110px" runat="server" Text='<%# Eval("AgreementNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Occupation">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOccupation" Width="100px" runat="server" Text='<%# Eval("Occupation") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" Width="100px" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contact Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContact" Width="100px" runat="server" Text='<%# Eval("ContactNumber") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Loan Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanAmount" Width="100px" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Disbursement Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDisbursementDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("DisbursementDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Loan Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanPeriod" Width="100px" runat="server" Text='<%# Eval("LoanPeriod") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Coverable Year">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCoverableYear" Width="100px" runat="server" Text='<%# Eval("CoverableYear") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Insurance Cost">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInsuranceCost" Width="100px" runat="server" Text='<%# Eval("InsuranceCost") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Beneficiary">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBeneficiary" Width="100px" runat="server" Text='<%# Eval("BeneficiaryName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Relation">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRelation" Width="100px" runat="server" Text='<%# Eval("Relation") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" Width="100px" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                                <asp:GridView ID="gvMoneyTransfer" CssClass="grid-layout" AllowPaging="true" AllowSorting="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                                    OnPageIndexChanging="gvMoneyTransfer_PageIndexChanging" OnSorting="gvMoneyTransfer_Sorting" OnRowDataBound="gvMoneyTransfer_RowDataBound">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Client Name" Visible="true" SortExpression="ClientName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" Width="150px" runat="server" Text='<%#Eval("ClientName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gender" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGender" Width="50px" runat="server" Text='<%#Eval("GenderText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DOB">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDOB" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdType" Width="100px" runat="server" Text='<%# Eval("IdTypeText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdNumber" Width="100px" runat="server" Text='<%# Eval("IdNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccountNumber" Width="110px" runat="server" Text='<%# Eval("AccountNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Contact Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContact" Width="100px" runat="server" Text='<%# Eval("ContactNumber") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Sum Assured">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSumAssured" Width="100px" runat="server" Text='<%# Eval("SumAssured") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("EffectiveDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Premium">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInsuranceCost" Width="100px" runat="server" Text='<%# Eval("Premium") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" Width="100px" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                                  <asp:GridView ID="gvPayroll" CssClass="grid-layout" AllowPaging="true" AllowSorting="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                                    OnPageIndexChanging="gvPayroll_PageIndexChanging">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Issue Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssuedDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("IssuedDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Last Name" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastName" Width="150px" runat="server" Text='<%#Eval("LastName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="First Name" Visible="true" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFirstName" Width="150px" runat="server" Text='<%#Eval("FirstName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client Name" Visible="true" SortExpression="ClientName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" Width="150px" runat="server" Text='<%#Eval("ClientName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gender" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGender" Width="50px" runat="server" Text='<%#Eval("GenderText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DOB">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDOB" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdType" Width="100px" runat="server" Text='<%# Eval("IdTypeText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdNumber" Width="100px" runat="server" Text='<%# Eval("IdNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                     

                                        <asp:TemplateField HeaderText="Phone Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContact" Width="100px" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                      
                                      
                                        <asp:TemplateField HeaderText="Sum Assured">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSumAssured" Width="100px" runat="server" Text='<%# Eval("SumAssured") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                       

                                        <asp:TemplateField HeaderText="Premium">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInsuranceCost" Width="100px" runat="server" Text='<%# Eval("Premium") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Effective Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("EffectiveDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyName" Width="100px" runat="server" Text='<%# Eval("SubCompanyName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                                <asp:GridView ID="gv_digital_loan" CssClass="grid-layout" AllowPaging="true" AllowSorting="true" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b"
                                    OnPageIndexChanging="gv_digital_loan_PageIndexChanging">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SEQ" Visible="true">
                                            <ItemTemplate>
                                              <%--  <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>--%>
                                                 <asp:Label ID="lblNo" runat="server" Text='<%#Eval("Seq") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Loan ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Account Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccountNumber" runat="server" Text='<%#Eval("AccountNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Last Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastName" Width="150px" runat="server" Text='<%#Eval("LastName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="First Name" Visible="false" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFirstName" Width="150px" runat="server" Text='<%#Eval("FirstName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client Name" Visible="true" SortExpression="ClientName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" Width="150px" runat="server" Text='<%#Eval("ClientName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Gender" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGender" Width="50px" runat="server" Text='<%#Eval("GenderText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="ID Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdType" Width="100px" runat="server" Text='<%# Eval("IdTypeText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdNumber" Width="100px" runat="server" Text='<%# Eval("IdNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="DOB">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDOB" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("DOB")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Phone Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone_number" Width="100px" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        
                                        <asp:TemplateField HeaderText="Province">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProvince" Width="100px" runat="server" Text='<%# Eval("Province") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="District">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDistrict" Width="100px" runat="server" Text='<%# Eval("District") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Commune">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommune" Width="100px" runat="server" Text='<%# Eval("Commune") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Village">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVillage" Width="100px" runat="server" Text='<%# Eval("Village") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Applied Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAppliedDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("AppliedDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Issue Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssueDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("IssueDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Effective Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("EffectiveDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Expiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpiryDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("ExpiryDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Maturity Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaturityDate" Width="100px" runat="server" Text='<%# Convert.ToDateTime( Eval("MaturityDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCoverPeriod" Width="100px" runat="server" Text='<%# Eval("LoanPeriodType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Cover Period">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCoverPeriod" Width="100px" runat="server" Text='<%# Eval("LoanPeriod") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="First Policy?">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFirstPolicy" Width="100px" runat="server" Text='<%# Convert.ToBoolean( Eval("IsFirstPolicy"))==true ? "Y":"N" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Policy Status Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicyStatusRemarks" Width="100px" runat="server" Text='<%#  Eval("PolicyStatusRemarks") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Premium">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPremium" Width="100px" runat="server" Text='<%# Eval("Premium") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="SumAssure">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSumAssure" Width="100px" runat="server" Text='<%# Eval("SumAssure") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </div>

                           
                        </div>
                        <!-- active tab -->
                         <div class="tab-pane" id="div_invalid">
                                <asp:Label ID="lblInvalideText" Text="NOTHING IS INVALID." runat="server" Style="color: red; font-size: 20pt; margin-left: 10px;" Visible="false"></asp:Label>
                                <div style="float: right; padding-right: 10px; padding-bottom: 5px;">
                                    <asp:Button ID="btnExportInvalide" runat="server" Text="Export" OnClick="btnExportInvalide_Click" CssClass="btn btn-primary" Visible="false" />
                                </div>

                                <asp:GridView ID="gvInvalid" runat="server" CssClass="grid-layout" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b">
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                </asp:GridView>
                            </div>
                    </div>
                    <!-- end tab -->
                    <asp:Label ID="lblError" runat="server"></asp:Label>

                    <!-- Modal Marketing Code -->
                    <div id="myMarketingCodeList" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelMarketingCodeHeader" aria-hidden="true" data-keyboard="false" data-backdrop="static">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="H1">Select Agent Code</h3>
                        </div>
                        <div class="modal-body">
                            <!---Modal Body--->

                            <table style="width: 100%; text-align: left;">
                                <tr>
                                    <td style="width: 25%; vertical-align: middle">Agent Code/Name:</td>
                                    <td style="width: 57%; vertical-align: bottom">
                                        <asp:TextBox ID="txtAgentName" Width="90%" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 18%; vertical-align: top">
                                        <input type="button" class="btn" style="height: 27px;" onclick="SearchMarketingCode();" value="Search" />
                                        <asp:Button ID="btnSearch" runat="server" Text="Search1" OnClick="btnSearch_Click" />
                                    </td>

                                </tr>

                            </table>
                            <hr />
                            <div id="dvAgentList"></div>

                            <div style="overflow-x: scroll; height: 100%; width: 100%;">
                                <asp:GridView ID="GridView1" CssClass="grid-layout" PageSize="20" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#21275b">
                                    <SelectedRowStyle BackColor="#EEFFAA" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="Sale Agent ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSaleAgentId" runat="server" Text='<%#Eval("SaleAgentId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sale Agent Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSaleAgentName" Width="150px" runat="server" Text='<%#Eval("FullNameEn") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary" style="height: 27px;" onclick="FillMarketingCode();" data-dismiss="modal" value="OK" />

                            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                        </div>
                    </div>
                    <!--End ModalMarketing Code-->
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
                <asp:PostBackTrigger ControlID="btnExportInvalide" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
</asp:Content>
