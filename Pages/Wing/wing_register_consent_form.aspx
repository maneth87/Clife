<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="wing_register_consent_form.aspx.cs" Inherits="Pages_Wing_wing_register_consent_form" %>

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
                $('#<%=ibtnSave.ClientID%>').click();
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
            <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/App_Themes/functions/save.png" OnClick="ibtnSave_Click" style="display:none;" />
                        <input type="button" id="btnSave" style="background: url('../../App_Themes/functions/save.png') no-repeat; border: none; height: 40px; width: 90px;" onclick="confirm_save();"  />

        </li>
        <li>
            <asp:ImageButton ID="ibtnClear" runat="server" ImageUrl="~/App_Themes/functions/clear.png" OnClick="ibtnClear_Click" />
        </li>
    </ul>
    <h1>Upload Consent Form List</h1>


    <div>
        <table class="table-layout">
            <tr>
                <td>
                    <div class="container">
                        <div style="position: absolute; margin-left: 500px; margin-top: 10px;">
                            <a style="color: blue;" href="UPLOAD_CONSENT_FORM_LIST_TEMPLATE.xlsx"><span>Download Template</span></a>
                        </div>
                        <table style="margin-left: 10px; margin-top: 10px;">

                            <tr>
                                <td>
                                    <asp:FileUpload ID="flUpload" runat="server" />
                                    <asp:Button Text="Load Data" runat="server" OnClick="btnLoadData_Click" ID="btnLoadData" OnClientClick="loading();" Style="border-radius: 10px;" />
                                </td>
                            </tr>

                        </table>




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
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Consent No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConsentNo" runat="server" Text='<%# Eval("ConsentNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Full Name(En)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFullNameEn" runat="server" Text='<%# Eval("FullNameEn")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Full Name(Kh)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFullNameKh" runat="server" Text='<%# Eval("FullNameKh")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DOB">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bank Account">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBankAccount" runat="server" Text='<%# Eval("WingAccNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Phone Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company/Factory">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFactory" runat="server" Text='<%# Eval("FactoryName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%#  Eval("Gender")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID Type" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIDType" runat="server" Text='<%#  Eval("IDTypeText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID Card" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>


                                </div>
                                <div class="tab-pane" id="div_invalid">
                                    <div style="text-align: right;">

                                        <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_Click" Visible="false" />
                                    </div>
                                    <div id="div_invalid_data" runat="server"></div>
                                    <asp:GridView ID="gv_invalid" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                        <SelectedRowStyle BackColor="#EEFFAA" />

                                         <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Consent No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConsentNo" runat="server" Text='<%# Eval("ConsentNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Full Name(En)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFullNameEn" runat="server" Text='<%# Eval("FullNameEn")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Full Name(Kh)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFullNameKh" runat="server" Text='<%# Eval("FullNameKh")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DOB">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%#  Eval("DOB") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bank Account">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBankAccount" runat="server" Text='<%# Eval("WingAccNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Phone Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company/Factory">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFactory" runat="server" Text='<%# Eval("FactoryName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%#  Eval("Gender")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID Type" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIDType" runat="server" Text='<%#  Eval("IDTypeText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID Card" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
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
