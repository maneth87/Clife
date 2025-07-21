<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="wing_policy_uploaded.aspx.cs" Inherits="Pages_Wing_wing_policy_uploaded" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <script type="text/javascript">

        function open_report(opt) {
            if (fdate.val() == '' && tdate.val() == '' && customer_name.val() == '' && policy_number.val() == '') {


                alert('Report filter is required.');
            }
            else {
                if (opt == 'pdf') {
                    btn.click();
                }
                else if (opt == 'excel') {
                    btnexcel.click();
                }
            }

        };

        $(document).ready(function () {
            $('.datepicker').datepicker();


            $('#tbl_filter').css('margin-left', ($('#div_content').width() / 2) - ($('#tbl_filter').width() / 2));
            $('#tbl_filter').css('border', 'solid 2px black');
        });
    </script>

    <h1>Wing Policies Uploaded Data</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <br />
    <div class="container" id="div_content">

        <table style="margin: 10px; border: solid 2px;" id="tbl_filter">
            <tr>
                <td colspan="4">
                    <h3>Filter</h3>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <asp:Label ID="lblFromDate" runat="server" Text="Issued Date From"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" PlaceHolder="DD/MM/YYYY" CssClass=" datepicker"></asp:TextBox></td>
                <td>
                    <asp:Label ID="lblToDate" runat="server" Text="To"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" PlaceHolder="DD/MM/YYYY" CssClass="datepicker"></asp:TextBox></td>
            </tr>
            <tr>

                <td style="padding-left: 10px;">
                    <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number"></asp:Label></td>
                <td colspan="3">
                    <asp:TextBox ID="txtPolicyNumber" runat="server" placeholder="Policy No.1, Policy No.2" Width="97%"></asp:TextBox></td>
            </tr>
            <tr>
                 <td style="padding-left: 10px;">
                    <asp:Label ID="lblPolicyStatus" runat="server" Text="Policy Status"></asp:Label></td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlPolicyStatus" runat ="server" DataSourceID="sqlPolicyStatus" DataValueField="Policy_Status_Type_ID" DataTextField="Detail" AppendDataBoundItems="true">
                        <asp:ListItem  Selected="True" Text="---Select---" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sqlPolicyStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="select Policy_Status_Type_ID, Detail from Ct_Policy_Status_type order by Policy_Status_Type_ID;"></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                    <asp:Button ID="btnClear" Text="Clear" CssClass="btn " runat="server"  OnClick="btnClear_Click"/>
                    <asp:Button ID="btnSearch" Text="Search" CssClass="btn btn-primary" runat="server" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
        <div style="float:right;">
            <asp:Button ID="btnExcel" runat="server" Text ="Export" CssClass="btn btn-primary" OnClick="btnExcel_Click" />
            <asp:Button ID="btnPDF" runat="server" Text ="PDF" CssClass="btn btn-primary" OnClick="btnPDF_Click" />
        </div>
        <div>
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
                            <asp:Label ID="lblConsentNo" runat="server" Text='<%# Eval("Consent_Number") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Full Name(En)">
                        <ItemTemplate>
                            <asp:Label ID="lblFullNameEn" runat="server" Text='<%# Eval("Last_Name") + " " + Eval("First_Name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Full Name(Kh)">
                        <ItemTemplate>
                            <asp:Label ID="lblFullNameKh" runat="server" Text='<%# Eval("Khmer_Last_Name") + " " + Eval("Khmer_First_Name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="DOB">
                        <ItemTemplate>
                            <asp:Label ID="lblDOB" runat="server" Text='<%# Convert.ToDateTime( Eval("birth_date").ToString()).ToString("dd/MM/yyyy") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Policy Number" >
                        <ItemTemplate>
                            <asp:Label ID="lblPolicyNumber" runat="server" Text='<%# Eval("Policy_Number") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Phone Number">
                        <ItemTemplate>
                            <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("Phone_Number") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Company/Factory">
                        <ItemTemplate>
                            <asp:Label ID="lblFactory" runat="server" Text='<%# Eval("Factory_Name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Gender">
                        <ItemTemplate>
                            <asp:Label ID="lblGender" runat="server" Text='<%# Helper.GetGenderText(Convert.ToInt32( Eval("Gender").ToString()),false)  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="ID Type" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblIDType" runat="server" Text='<%# Helper.GetIDCardTypeText(Convert.ToInt32( Eval("ID_Type").ToString())) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="ID Card" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID_Card") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product Name">
                        <ItemTemplate>
                            <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("PRODUCT_ID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sum Assured USD">
                        <ItemTemplate>
                            <asp:Label ID="lblSumAssured" runat="server" Text='<%# Eval("SUM_ASSURED") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Premium Paid USD">
                        <ItemTemplate>
                            <asp:Label ID="lblPremiumPaid" runat="server" Text='<%# Eval("Premium") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Payment Mode" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("Mode") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                  

                    <asp:TemplateField HeaderText="Payment Code" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentCode" runat="server" Text='<%# Eval("Payment_Code") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment By">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentBy" runat="server" Text='<%# Eval("Payment_By") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                   <asp:TemplateField HeaderText="Age">
                        <ItemTemplate>
                            <asp:Label ID="Age" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Agent Code">
                        <ItemTemplate>
                            <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("Agent_code") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("Policy_Status") %>'></asp:Label>
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
</asp:Content>



