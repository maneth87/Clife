<%@ Page Title="Clife | CMK => Upload Policy" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Upload_Policy_Cmk.aspx.cs" Inherits="Pages_CMK_Upload_Policy_Cmk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        
        <li>
            <asp:ImageButton ID="ImgBtnClear" runat="server" ValidationGroup="2"  Visible="true" ImageUrl="~/App_Themes/functions/clear.png" CausesValidation="False" OnClick="ImgBtnClear_Click" />
        </li>
        <li>
            <div style="display: none;">
                <asp:Button ID="btnSave" runat="server" />
            </div>
            <asp:ImageButton ID="ImgBtnSave" runat="server" Enabled="False"  Visible="true" ImageUrl="~/App_Themes/functions/save.png" OnClick="ImgBtnSave_Click" />

        </li>

    </ul>
    <script type="text/javascript">
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        function ValidateTextDecimal(j) {

            var msg = j.value;
            var w;
            var nokta = msg.indexOf(".");
            var ind;

            for (w = 0; w < msg.length; w++) {

                ind = msg.substring(w, w + 1);
                if (ind < "0" || ind > "9") {

                    if (nokta > 0)
                        if (w == nokta) continue;

                    msg = msg.substring(0, w);
                    j.value = msg;
                    break;
                }

            }

        }

        $(document).ready(function () {
            $('.datepicker').datepicker();
        });

        function ProcessChannelProductType(Channel_Sub_ID) {
            if (Channel_Sub_ID != "0") {

                $.ajax({
                    type: "POST",
                    url: "../../ChannelWebService.asmx/GetChannelLocationID",
                    data: "{channel_sub_id:'" + Channel_Sub_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var channel_location = data.d[0];

                        if (channel_location.Channel_Location_ID != "") {
                            $('#Main_hdfChannelSubID').val(Channel_Sub_ID);
                            $('#Main_hdfChannelLocationID').val(channel_location.Channel_Location_ID);
                            $('#Main_hdfChannelItemID').val(channel_location.Channel_Item_ID);

                        }

                    }

                });

                $.ajax({
                    type: "POST",
                    url: "../../ProductWebService.asmx/GetProductCMK",
                    data: "{card:'" + Channel_Sub_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#Main_ddlProduct').setTemplate($("#jTemplateProduct").html());
                        $('#Main_ddlProduct').processTemplate(data);
                    }

                });
            } else {
                //$('#Main_ddlProduct').setTemplate($("#jTemplateProduct").html());
                //$('#Main_ddlProduct').processTemplate(0);
            }
        }

        function GetProduct(product_id)
        {
            if (product_id != "" && product_id != "0") {
                $('#Main_ddlProduct').val(product_id);
                $('#Main_hdfProductID').val(product_id);
            } else {
                $('#Main_ddlProduct').val("");
                $('#Main_hdfProductID').val("");
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>

    <%-- date picker jquery and css--%>
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <script id="jTemplateProduct" type="text/html">
        <option selected="selected" value='0'>.</option>
        {#foreach $T.d as record}          
             <option value='{ $T.record.Product_ID }'>{ $T.record.En_Title }</option>
        {#/for}
           
    </script>
    
    <br />
    <br />
    <br />

    <%-- Upload Form Design Section--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">CMK : Upload Policies</h3>
        </div>
        <div class="panel-body">
            <table>
                <tr>
                    <td style="text-align: right; vertical-align:middle;">Bank Name:</td>
                    <td>
                        <asp:DropDownList ID="ddlChannel" Width="95%" Height="35px" AppendDataBoundItems="true" runat="server" onchange="ProcessChannelProductType(this.value)"  DataSourceID="SqlDataSourceChannel" DataTextField="Channel_Name" DataValueField="Channel_Sub_ID">
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td></td>
                </tr>
                <tr>

                    <td style="text-align: right;">Product:</td>
                    <td style="vertical-align: bottom; width:350px; vertical-align:middle;">
                        <asp:DropDownList ID="ddlProduct" Width="95%" Height="35px" onchange="GetProduct(this.value)" AppendDataBoundItems="true" runat="server" >
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td style="text-align: right;">Report Date:</td>
                    <td>
                        <asp:TextBox ID="txtReport_date" runat="server" PlaceHolder="DD/MM/YYYY" CssClass="datepicker" width="91%" TabIndex="4" ></asp:TextBox>
                    </td>
                    
                </tr>

                <tr>
                    <td  class="colsub">
                        Approver Name : 
                    </td>
                    <td  class="colsub">
                        <asp:DropDownList ID="ddlFullName" runat="server" AutoPostBack="false" CssClass="ddl"></asp:DropDownList>
                    </td>
                    <td></td>
                    <td>
                        <a href="Templates/CMK_POLICIES_TEMPLATE.xlsx" style="color: blue;"><u>Download Template File</u> </a>
                    </td>
                </tr>

                <tr>

                    <td style="text-align: right">Upload:</td>
                    <td>
                        <asp:FileUpload ID="FileUploadCmkPolicy" runat="server" Width="91%" />
                    </td>
                    <td></td>
                    <td>
                        <asp:Button Text="Load Data" runat="server" OnClick="btnLoadData_Click" ID="btnLoadData" OnClientClick="loading();" />
                    </td>
                    
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

        </div>
    </div>
  

    <%--UPLOAD TAB--%>
    <div id="DIV_UPLOAD_TAB" runat="server">
        <table class="table-layout">
            <tr style="vertical-align: middle;">
                <td style="vertical-align: middle;">
                    <div class="container">
                        <div id="Div2" style="padding-top: 10px;">
                            <ul id="Ul1" class="nav nav-tabs" data-tabs="tabs">
                                <li class="active"><a href="#div_valid" data-toggle="tab" style="color: green;">Valid Data</a></li>
                                <li><a href="#div_invalid" data-toggle="tab" style="color: red;">Invalid Data</a></li>
                            </ul>
                            <div id="my-div-valid" class="tab-content">
                                <div class="tab-pane active" id="div_valid">
                                    <div id="div_valid_data" runat="server"></div>
                                    <b><asp:Label ID="lblCountValid" runat="server" ForeColor="Red"></asp:Label></b>
                                    <asp:GridView ID="gv_valid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                        RowStyle-BackColor="#ffffff" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000" BorderColor="#B9B9B9" AllowPaging="True" PageSize="20"   OnPageIndexChanging="gv_valid_PageIndexChanging">
                                        <SelectedRowStyle BackColor="#EEFFAA" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Eval("Row_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("CMKCustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Certificate No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loan ID" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="First Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Convert.ToDateTime(Eval("DateOfBirth")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Group">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Outstanding Balance">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutstandingBalance" runat="server" Text='<%# Eval("OutstandingBalance") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Assured Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssuredAmount" runat="server" Text='<%# Eval("AssuredAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Effective Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("EffectiveDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#99ccff" ForeColor="White"></HeaderStyle>

                                        <PagerSettings PageButtonCount="15" />

                                        <RowStyle BackColor="#CCCCCC"></RowStyle>
                                    </asp:GridView>
                                </div>
                                <div class="tab-pane" id="div_invalid">
                                    <div id="div_invalid_data" runat="server"></div>
                                    <b><asp:Label ID="lblCountInvalid" runat="server" ForeColor="Red"></asp:Label></b>
                                    <asp:GridView ID="gv_invalid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                        RowStyle-BackColor="#ffffff" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000" BorderColor="#B9B9B9" AllowPaging="True" PageSize="20"   OnPageIndexChanging="gv_invalid_PageIndexChanging">
                                        <SelectedRowStyle BackColor="#EEFFAA" />

                                        <Columns>

                                            <asp:TemplateField HeaderText="No#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Eval("Row_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Branch" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranch" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Customer ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("CMKCustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Certificate No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan ID" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Last Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="First Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Eval("DateOfBirth") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("LoanType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OpenedDate" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOpenedDate" runat="server" Text='<%# Eval("OpenedDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DateOfEntry" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateOfEntry" runat="server" Text='<%# Eval("DateOfEntry") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Duration" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("LoanDuration") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CoverYear" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbCoverYear" runat="server" Text='<%# Eval("CoveredYear") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outstanding Balance" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutstandingBalance" runat="server" Text='<%# Eval("OutstandingBalance") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Assured Amount" Visible="false" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssuredAmount" runat="server" Text='<%# Eval("AssuredAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Currancy" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrancy" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Group">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Age">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAge" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PaymentMode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("PaymodeID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PolicyStatus" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="MonthlyPremium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthlyPremium" runat="server" Text='<%# Eval("MonthlyPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PremiumAfterDiscount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAfterDiscount" runat="server" Text='<%# Eval("PremiumAfterDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="ExtraPremium" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# Eval("ExtraPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Effective Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Paid Off In Month" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaid" runat="server" Text='<%# Eval("PaidOffInMonth") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Terminate Date" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTerminateDate" runat="server" Text='<%# Eval("TerminateDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-ForeColor="Red">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("CreatedNoted") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>

                                        <HeaderStyle BackColor="#99ccff" ForeColor="White"></HeaderStyle>

                                        <PagerSettings PageButtonCount="15" />

                                        <RowStyle BackColor="#CCCCCC"></RowStyle>
                                    </asp:GridView>

                                    <div id="Div_Export_Invalid" style="margin-left: 1%" runat="server">
                                        <asp:Button ID="export_excel_invalid" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_invalid_Click" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </td>
            </tr>
        </table>
    </div>

    <%--STATUS TAB--%>
    <div id="DIV_RESULT" runat="server">
        <table class="table-layout">
            <tr style="vertical-align: middle;">
                <td style="vertical-align: middle;">
                    <div class="container">
                        <div id="tab-content" style="padding-top: 10px;">
                            <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                                <li id="success" class="active" runat="server"><a href="#div_success" data-toggle="tab" style="color: green;">Success</a></li>
                                <li id="fail" runat="server"><a href="#div_fail" data-toggle="tab" style="color: red;">Fail</a></li>
                            </ul>
                            <div id="my-tab-content" class="tab-content">
                                <div class="tab-pane active" id="div_success">
                                    <div id="div_success_data" runat="server"></div>
                                    <b><asp:Label ID="lblCountSuccess" runat="server" ForeColor="Red"></asp:Label></b>
                                    <asp:GridView ID="gv_success" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                        RowStyle-BackColor="#ffffff" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000" BorderColor="#B9B9B9" AllowPaging="True" PageSize="20"   OnPageIndexChanging="gv_success_PageIndexChanging">
                                        <SelectedRowStyle BackColor="#EEFFAA" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="No#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Eval("Row_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Branch" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranch" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Customer ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("CMKCustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Certificate No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan ID" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Last Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="First Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Convert.ToDateTime(Eval("DateOfBirth")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("LoanType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OpenedDate" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOpenedDate" runat="server" Text='<%# Convert.ToDateTime(Eval("OpenedDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DateOfEntry" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateOfEntry" runat="server" Text='<%# Convert.ToDateTime(Eval("DateOfEntry")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Duration" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("LoanDuration") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CoverYear" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbCoverYear" runat="server" Text='<%# Eval("CoveredYear") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outstanding Balance" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutstandingBalance" runat="server" Text='<%# Eval("OutstandingBalance") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Assured Amount" Visible="false" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssuredAmount" runat="server" Text='<%# Eval("AssuredAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Currancy" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrancy" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Group">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Age">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAge" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PaymentMode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("PaymodeID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="MonthlyPremium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthlyPremium" runat="server" Text='<%# Eval("MonthlyPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PremiumAfterDiscount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAfterDiscount" runat="server" Text='<%# Eval("PremiumAfterDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="ExtraPremium" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# Eval("ExtraPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Effective Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("EffectiveDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle BackColor="#99ccff" ForeColor="White"></HeaderStyle>

                                        <PagerSettings PageButtonCount="15" />

                                        <RowStyle BackColor="#CCCCCC"></RowStyle>
                                    </asp:GridView>
                                </div>
                                <div class="tab-pane" id="div_fail">
                                    <div id="div_fail_data" runat="server"></div>
                                    <b><asp:Label ID="lblCountFail" runat="server" ForeColor="Red"></asp:Label></b>
                                    <asp:GridView ID="gv_fail" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                        RowStyle-BackColor="#ffffff" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000" BorderColor="#B9B9B9" AllowPaging="True" PageSize="20"   OnPageIndexChanging="gv_fail_PageIndexChanging">
                                        <SelectedRowStyle BackColor="#EEFFAA" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="No#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Eval("Row_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Branch" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranch" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Customer ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("CMKCustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Certificate No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan ID" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Last Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="First Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# Convert.ToDateTime(Eval("DateOfBirth")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("LoanType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OpenedDate" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOpenedDate" runat="server" Text='<%# Convert.ToDateTime(Eval("OpenedDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DateOfEntry" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateOfEntry" runat="server" Text='<%# Convert.ToDateTime(Eval("DateOfEntry")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Duration" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("LoanDuration") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CoverYear" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbCoverYear" runat="server" Text='<%# Eval("CoveredYear") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outstanding Balance" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutstandingBalance" runat="server" Text='<%# Eval("OutstandingBalance") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Assured Amount" Visible="false" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssuredAmount" runat="server" Text='<%# Eval("AssuredAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Currancy" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrancy" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Group">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Age">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAge" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PaymentMode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("PaymodeID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PolicyStatus" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="MonthlyPremium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthlyPremium" runat="server" Text='<%# Eval("MonthlyPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PremiumAfterDiscount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAfterDiscount" runat="server" Text='<%# Eval("PremiumAfterDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="ExtraPremium" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraPremium" runat="server" Text='<%# Eval("ExtraPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPremium" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Effective Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(Eval("EffectiveDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Paid Off In Month" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaid" runat="server" Text='<%# Eval("PaidOffInMonth") != "" ? Convert.ToDateTime(Eval("PaidOffInMonth")).ToString("MM/yyyy") : "-" %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Terminate Date" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTerminateDate" runat="server" Text='<%# Eval("TerminateDate") != "" ? Convert.ToDateTime(Eval("TerminateDate")).ToString("dd/MM/yyyy") : "-" %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-ForeColor="Red">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("CreatedNoted") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle BackColor="#99ccff" ForeColor="White"></HeaderStyle>

                                        <PagerSettings PageButtonCount="15" />

                                        <RowStyle BackColor="#CCCCCC"></RowStyle>
                                    </asp:GridView>

                                    <div id="Div_Export" style="margin-left: 1%" runat="server">
                                        <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_Click" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </td>
            </tr>
        </table>
    </div>

  <asp:SqlDataSource ID="SqlDataSourceChannel" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT DISTINCT Ct_Channel_Item.Channel_Item_ID, Ct_Channel_Channel_Item.Channel_Sub_ID, Ct_Channel_Item.Channel_Name FROM Ct_Channel_Item 
    INNER JOIN Ct_Channel_Location ON Ct_Channel_Item.Channel_Item_ID = Ct_Channel_Location.Channel_Item_ID
    INNER JOIN Ct_Channel_Channel_Item ON Ct_Channel_Channel_Item.Channel_Item_ID = Ct_Channel_Location.Channel_Item_ID WHERE Ct_Channel_Channel_Item.Channel_Sub_ID=6
    ORDER BY Ct_Channel_Item.Channel_Name ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select * from Ct_Product WHERE Product_ID ='CLC' "></asp:SqlDataSource>

    <%--Hidden fields of User--%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />


    <%--Hidden fields of Channel--%>
    <asp:HiddenField ID="hdfChannelChannelItemID" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelLocationID" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelItemID" runat="server" Value="0" />
    <asp:HiddenField ID="hdfChannelSubID" runat="server" Value="0" />

    <asp:HiddenField ID="hdfProductID" runat="server" />
</asp:Content>


