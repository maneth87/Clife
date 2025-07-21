<%@ Page Title="Clife | CMK => Upload Monthly Premium" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Upload_Monthly_Premium.aspx.cs" Inherits="Pages_CMK_Upload_Monthly_Premium" %>

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

        function ConfirmDel()
        {
            var report_date = $("#Main_txtReport_date").val();

            if (report_date != "") {

                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm("Are you sure want to delete this report?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
                
            } else {
                alert("Choose report date!!");
            }
        }

        $(document).ready(function () {
            $('.datepicker').datepicker();
        });


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
            <h3 class="panel-title">CMK : Upload Renewal Premium</h3>
        </div>
        <div class="panel-body">
            <table>
                <tr>
                    <td style="text-align: right;">Report Date:</td>
                    <td>
                        <asp:TextBox ID="txtReport_date" runat="server" PlaceHolder="DD/MM/YYYY" CssClass="datepicker" width="40%" TabIndex="4" ></asp:TextBox>
                        <asp:DropDownList ID="ActionList" AutoPostBack="false" width="25%" TabIndex="4" runat="server">
                          <asp:ListItem Value="1"> NEW </asp:ListItem>
                          <asp:ListItem Value="2"> ADD To Exist </asp:ListItem>
                       </asp:DropDownList>
                    </td>
                    <td>
                        <a href="Templates/UPLOAD_MONTHLY_PREMIUM_TEMPLATE.xlsx" style="color: blue;"><u>Download Template File</u> </a>
                    </td>
                </tr>

                <tr>

                    <td style="text-align: right">Upload:</td>
                    <td>
                        <asp:FileUpload ID="FileUploadCmkPolicy" runat="server" Width="40%" />
                    </td>
                    <td>
                        <asp:Button Text="Load Data" runat="server" OnClick="btnLoadData_Click" ID="btnLoadData" />
                    </td>

                    <td>
                        <asp:Button Text="Delete" runat="server" CssClass="btn btn-danger" OnClick="btnDelReport_Click" ID="DeleteReport" OnClientClick="ConfirmDel()" />

                        <%--<input type="button" name="DeleteReport" value="Delete" class="btn btn-danger" onclick="DelReport()"  />--%>
                    </td>
                    
                </tr>
                <tr>
                    <td><span style="font-weight:bold; color:red;">Note:</span></td>
                    <td style="color:red;">Once upload will be count as 1 invoice premium, in case fail, please contact System Admin </td>
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

                                            <asp:TemplateField HeaderText="Certificate No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
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

                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Policy Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Monthly Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthlyPremium" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Premium After Discount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAfterDiscount" runat="server" Text='<%# Eval("PremiumAfterDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Extra Premium">
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

                                            <asp:TemplateField HeaderText="Report Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ReportDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Paid Off Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidOffDate" runat="server" Text='<%# Eval("PaidOffInMonth") !="" ? Convert.ToDateTime(Eval("PaidOffInMonth")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Terminated Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTerminatedDate" runat="server" Text='<%# Eval("TerminateDate") != "" ? Convert.ToDateTime(Eval("TerminateDate")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
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
                                    <asp:GridView ID="gv_invalid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
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

                                            <asp:TemplateField HeaderText="Certificate No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanID" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
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

                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Policy Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Monthly Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthlyPremium" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Premium After Discount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAfterDiscount" runat="server" Text='<%# Eval("PremiumAfterDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Extra Premium">
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

                                            <asp:TemplateField HeaderText="Report Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ReportDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Paid Off Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidOffDate" runat="server" Text='<%# Eval("PaidOffInMonth") !="" ? Convert.ToDateTime(Eval("PaidOffInMonth")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Terminated Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTerminatedDate" runat="server" Text='<%# Eval("TerminateDate") != "" ? Convert.ToDateTime(Eval("TerminateDate")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
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
                                <li class="active"><a href="#div_success" data-toggle="tab" style="color: green;">Success</a></li>
                                <li><a href="#div_fail" data-toggle="tab" style="color: red;">Fail</a></li>
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
                                                    <asp:Label ID="lblNoSuccess" runat="server" Text='<%# Eval("Row_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerIDSuccess" runat="server" Text='<%#Eval("CMKCustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Certificate No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNoSuccess" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loan ID" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanIDSuccess" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountSuccess" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Outstanding Balance">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutstandingBalanceSuccess" runat="server" Text='<%# Eval("OutstandingBalance") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Assured Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssuredAmountSuccess" runat="server" Text='<%# Eval("AssuredAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrencySuccess" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Policy Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyStatusSuccess" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Monthly Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthlyPremiumSuccess" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Premium After Discount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAfterDiscountSuccess" runat="server" Text='<%# Eval("PremiumAfterDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Extra Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraPremiumSuccess" runat="server" Text='<%# Eval("ExtraPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPremiumSuccess" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Report Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportDateSuccess" runat="server" Text='<%# Convert.ToDateTime(Eval("ReportDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Paid Off Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidOffDateSuccess" runat="server" Text='<%# Eval("PaidOffInMonth") !="" ? Convert.ToDateTime(Eval("PaidOffInMonth")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Terminated Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTerminatedDateSuccess" runat="server" Text='<%# Eval("TerminateDate") != "" ? Convert.ToDateTime(Eval("TerminateDate")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
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
                                                    <asp:Label ID="lblNoFailed" runat="server" Text='<%# Eval("Row_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Customer ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerIDFailed" runat="server" Text='<%#Eval("CMKCustomerID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Certificate No" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCertificateNoFailed" runat="server" Text='<%#Eval("CertificateNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan ID" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanIDFailed" runat="server" Text='<%#Eval("LoanID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountFailed" runat="server" Text='<%# Eval("LoanAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outstanding Balance">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOutstandingBalanceFailed" runat="server" Text='<%# Eval("OutstandingBalance") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Assured Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssuredAmountFailed" runat="server" Text='<%# Eval("AssuredAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrancyFailed" runat="server" Text='<%# Eval("Currancy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Policy Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyStatusFailed" runat="server" Text='<%# Eval("PolicyStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Payment Mode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentModeFailed" runat="server" Text='<%# Eval("PaymodeID") == "4" ? "Monthly" : " - "%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Monthly Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthlyPremiumFailed" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Premium After Discount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAfterDiscountFailed" runat="server" Text='<%# Eval("PremiumAfterDiscount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Extra Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraPremiumFailed" runat="server" Text='<%# Eval("ExtraPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Premium">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPremiumFailed" runat="server" Text='<%# Eval("TotalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Report Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportDateFailed" runat="server" Text='<%# Convert.ToDateTime(Eval("ReportDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Paid Off Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidOffDateFailed" runat="server" Text='<%# Eval("PaidOffInMonth") !="" ? Convert.ToDateTime(Eval("PaidOffInMonth")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                    
                                            <asp:TemplateField HeaderText="Terminated Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTerminateDateFailed" runat="server" Text='<%# Eval("TerminateDate") != "" ? Convert.ToDateTime(Eval("TerminateDate")).ToString("dd/MM/yyyy") : " - " %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="System Remarks" ItemStyle-ForeColor="Red">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarkFailed" runat="server" Text='<%# Eval("CreatedNoted")%>'></asp:Label>
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

    <%--Hidden fields of User--%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />


    <%--Hidden fields of Channel--%>
</asp:Content>