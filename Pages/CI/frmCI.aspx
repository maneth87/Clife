<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmCI.aspx.cs" Inherits="Pages_Business_CI_frmCI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

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
    </style>

    <script>
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });

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

    </script>

    <div id="fade"></div>
    <div id="modal">
        <img id="loader" src="../../App_Themes/functions/loading.gif" alt="" /><br />
        <span>Processing data...</span>
    </div>

    <ul class="toolbar">

        <li>
            <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="ibtnSave_Click" OnClientClick="loading();" />
        </li>
        <li>
            <asp:ImageButton ID="ibtnClear" runat="server" ImageUrl="~/App_Themes/functions/clear.png" OnClick="ibtnClear_Click" />
        </li>
    </ul>
    <h1>Simple One Data Uploading</h1>


    <div>
        <table class="table-layout">
            <tr>
                <td>
                    <div class="container">
                        <div style="position: absolute; margin-left: 500px; margin-top: 5px;">
                            <a style="color: blue;" href="SIMPLE_ONE_DATA_TEMPLATE.xlsx"><span>Download Template</span></a>
                        </div>
                        <div>
                            <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date:"></asp:Label><span style="color: red;">*</span>
                            <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass=" datepicker" onkeypress="return false;"></asp:TextBox>
                        </div>
                        <asp:FileUpload ID="flUpload" runat="server" />
                        <asp:Button Text="Load Data" runat="server" OnClick="btnLoadData_Click" ID="btnLoadData" OnClientClick="loading();" />
                        <div style="margin-top: 20px; margin-bottom: 20px; vertical-align: middle;">
                            <asp:Label ID="lblApprover" runat="server" Text="Approver"></asp:Label>
                            <asp:DropDownList ID="ddlApprover" runat="server" ></asp:DropDownList>
                        </div>
                        <div style="margin-top: 20px; margin-bottom: 20px; vertical-align: middle;">
                            <asp:Label ID="lblSendSMS" runat="server" Text="Send SMS"></asp:Label>
                            <asp:CheckBox ID="ckbSendSMS" runat="server" Checked="true" Text="" TextAlign="Left" Width="200px" />
                        </div>
                    </div>

                </td>
            </tr>
            <tr style="vertical-align: middle;">
                <td style="vertical-align: middle;">
                    <div class="container">
                        <div id="tab-content" style="padding-top: 10px;">
                            <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
                                <li class="active"><a href="#div_valid" data-toggle="tab" style="color: green;">Success</a></li>
                                <li><a href="#div_invalid" data-toggle="tab" style="color: red;">Fail</a></li>

                            </ul>
                            <div id="my-tab-content" class="tab-content">
                                <div class="tab-pane active" id="div_valid">
                                    <div id="div_valid_data" runat="server"></div>
                                    <asp:GridView ID="gv_valid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                        <SelectedRowStyle BackColor="#EEFFAA" />

                                        <Columns>
                                            <%-- <asp:TemplateField HeaderText="No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Eval("No") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="ID Type" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTypeID" runat="server" Text='<%#Eval("IDType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID Type" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTypeIDText" runat="server" Text='<%#Eval("IDTypeText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Surname(En)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnLastName" runat="server" Text='<%# Eval("ENLastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Given Name(En)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnFirstName" runat="server" Text='<%# Eval("ENFirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Surname(Kh)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKHLastName" runat="server" Text='<%# Eval("KHLastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Given Name(Kh)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKHFirstName" runat="server" Text='<%# Eval("KHFirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DOB">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Age">
                                                <ItemTemplate>
                                                    <asp:Label ID="Age" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Country Code" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCountryCode" runat="server" Text='<%# Eval("CountryCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Province">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProvince" runat="server" Text='<%# Eval("Province") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sum Assured USD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSumAssured" runat="server" Text='<%# Eval("SA") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Premium Paid USD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumPaid" runat="server" Text='<%# Eval("UserPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Syestem Premium USD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSystemPremium" runat="server" Text='<%# Eval("SystemPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Annual Premium USD" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOriginalPremium" runat="server" Text='<%# Eval("OriginalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment Mode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("PayMode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment Mode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentModeText" runat="server" Text='<%# Eval("PayModeText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Policy Number" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyNumber" runat="server" Text='<%# Eval("PolicyNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Payment Code" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentCode" runat="server" Text='<%# Eval("PaymentCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentBy" runat="server" Text='<%# Eval("PaymentBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Agent Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("Agent_code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>

                                </div>
                                <div class="tab-pane" id="div_invalid">
                                    <div style="text-align: right;">

                                        <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_Click" />
                                    </div>
                                    <div id="div_invalid_data" runat="server"></div>
                                    <asp:GridView ID="gv_invalid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                        <SelectedRowStyle BackColor="#EEFFAA" />

                                        <Columns>
                                            <%-- <asp:TemplateField HeaderText="No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Eval("No") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="ID Type" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTypeID" runat="server" Text='<%#Eval("IDType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID Type" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTypeIDText" runat="server" Text='<%#Eval("IDTypeText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Surname(En)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnLastName" runat="server" Text='<%# Eval("ENLastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Given Name(En)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnFirstName" runat="server" Text='<%# Eval("ENFirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Surname(Kh)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKHLastName" runat="server" Text='<%# Eval("KHLastName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Given Name(Kh)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKHFirstName" runat="server" Text='<%# Eval("KHFirstName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DOB">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Age">
                                                <ItemTemplate>
                                                    <asp:Label ID="Age" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Country Code" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCountryCode" runat="server" Text='<%# Eval("CountryCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Province">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProvince" runat="server" Text='<%# Eval("Province") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sum Assured USD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSumAssured" runat="server" Text='<%# Eval("SA") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Premium Paid USD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumPaid" runat="server" Text='<%# Eval("UserPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Syestem Premium USD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSystemPremium" runat="server" Text='<%# Eval("SystemPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Annual Premium USD" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOriginalPremium" runat="server" Text='<%# Eval("OriginalPremium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment Mode" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("PayMode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment Mode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentModeText" runat="server" Text='<%# Eval("PayModeText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Policy Number" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyNumber" runat="server" Text='<%# Eval("PolicyNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Payment Code" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentCode" runat="server" Text='<%# Eval("PaymentCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaymentBy" runat="server" Text='<%# Eval("PaymentBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Agent Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("Agent_code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                </div>

                            </div>
                        </div>
                    </div>

                </td>
            </tr>

        </table>

    </div>
</asp:Content>


