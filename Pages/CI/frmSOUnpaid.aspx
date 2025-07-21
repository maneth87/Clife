<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmSOUnpaid.aspx.cs" Inherits="Pages_CI_frmSOUnpaid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <script>

        $(document).ready(function () {


            $('#tbl_filter').css('margin-left', ($('#tbl_content').width() / 2) - ($('#tbl_filter').width() / 2));
            $('#tbl_filter').css('border', 'solid 2px black');

        });


    </script>
    <h1>Upload Unpaid</h1>
    <div>
        <table class="table-layout" id="tbl_content">
            <tr>
                <td>
                    <table id="tbl_filter" style="width: 500px; height: 150px;">

                        <tr>
                            <td style="padding-left: 10px; padding-right: 10px;">Choose File</td>
                            <td style="padding-left: 10px; padding-right: 10px;" colspan="2">
                                <asp:FileUpload ID="UnpaidFile" runat="server" />
                            </td>

                        </tr>
                        <tr>
                            <td colspan="3" style="padding-left: 10px; padding-right: 10px;">
                                <a href="SIMPLE_ONE_UNPAID_TEMPLATE.xlsx" style="color: blue;"><u>Download Template File</u> </a>
                            </td>
                        </tr>
                        <tr>

                            <td colspan="3" style="text-align: center;">

                                <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClientClick="return confirm('Confirm to process upload unpaid.');" OnClick="btnUpload_Click" Style="margin-left: 100px; border-radius: 5px;" />
                            </td>
                        </tr>
                    </table>
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
                                    <div style="text-align: right;">

                                        <asp:Button ID="btnExportValid" runat="server" Text="Export" class="btn btn-primary" OnClick="btnExportValid_Click" Visible="false" />
                                    </div>
                                    <asp:GridView ID="gv_policy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                        <SelectedRowStyle BackColor="#EEFFAA" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="No" Visible="true">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            
                                            <asp:TemplateField HeaderText="Policy Number" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyNumber" runat="server" Text='<%#Eval("policy_number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Due Date" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDueDate" runat="server" Text='<%#Eval("Due_Date") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Premium" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremium" runat="server" Text='<%#Eval("Premium") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Pay Year" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayYear" runat="server" Text='<%#Eval("Pay_Year") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Pay Lot" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayLot" runat="server" Text='<%#Eval("Pay_Lot") %>'></asp:Label>
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
                                <div class="tab-pane" id="div_invalid">
                                    <div style="text-align: right;">

                                        <asp:Button ID="export_excel" runat="server" Text="Export" class="btn btn-primary" OnClick="export_excel_Click" Visible="false" />
                                    </div>
                                    <div id="div_invalid_data" runat="server"></div>
                                                                        <asp:GridView ID="gvErr" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                        <SelectedRowStyle BackColor="#EEFFAA" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="No" Visible="true">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Policy Number" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPolicyNumber" runat="server" Text='<%#Eval("policy_number") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Due Date" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDueDate" runat="server" Text='<%#Eval("Due_Date") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Premium" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremium" runat="server" Text='<%#Eval("Premium") %>'></asp:Label>
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

