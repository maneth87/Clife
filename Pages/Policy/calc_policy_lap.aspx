<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="calc_policy_lap.aspx.cs" Inherits="Pages_Policy_calc_policy_lap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <script>
        $(document).ready(function () {

            $('.datepicker').datepicker();

            $('#tbl_search').css('margin-left', ($('#tbl_main').width() / 2) - ($('#tbl_search').width() / 2));
            $('#tbl_search').css('border', 'solid 2px black');
         
            $('#tbl_footer').css('margin-left', ($('#tbl_main').width() / 2) - ($('#tbl_footer').width() / 2));
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <h1>Calculate Policy Lap</h1>


    <div>
        <table class="table-layout" id="tbl_main">
            <tr>
                <td style="vertical-align: middle;">
                    <table id="tbl_search" style="margin:5px;">
                        <tr>

                            <td>
                                <asp:Label ID="lblPolcyNumber" runat="server" Text="Policy Number"></asp:Label>
                                <span style="color:red;">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                            </td>
                            <td rowspan="3" style="text-align: left; vertical-align: middle;">
                                <ul class="toolbar">

                                    <li>
                                        <asp:ImageButton ID="ibtnCalc" runat="server" ImageUrl="~/App_Themes/functions/search.png" OnClick="ibtnCalc_Click"  />
                                    </li>
                                    <li>
                                        <asp:ImageButton ID="ibtnClear" runat="server" ImageUrl="~/App_Themes/functions/clear.png" OnClick="ibtnClear_Click" />
                                    </li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPayDate" runat="server" Text="Pay Date"></asp:Label>
                                <span style="color:red;">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayDate" runat="server" CssClass="datepicker"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPolicyStatus" runat="server" Text="Policy Status"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPolicyStatus" runat="server" Style="font-weight: bold;" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        <div>
            <asp:GridView ID="gv_data" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                <SelectedRowStyle BackColor="#EEFFAA" />

                <Columns>

                    <asp:TemplateField HeaderText="No">
                        <ItemTemplate>
                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Policy Number" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblPolicyNumber" runat="server" Text='<%#Eval("Polno") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Full Name" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Customer") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("Product") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Year/Time">
                        <ItemTemplate>
                            <asp:Label ID="lblYearTime" runat="server" Text='<%# Eval("YearTimes").ToString()  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Mode">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentMode" runat="server" Text='<%# Eval("Mode") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sum Insured">
                        <ItemTemplate>
                            <asp:Label ID="lblSumInsured" runat="server" Text='<%# Eval("SumIn") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Due Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval("Due_Date") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Interest">
                        <ItemTemplate>
                            <asp:Label ID="lblInterest" runat="server" Text='<%# Eval("interest") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Days">
                        <ItemTemplate>
                            <asp:Label ID="lblDays" runat="server" Text='<%# Eval("duration_day") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Months">
                        <ItemTemplate>
                            <asp:Label ID="lblMonths" runat="server" Text='<%# Eval("month") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </div>
        <div id="div_footer">
            <table id="tbl_footer">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPremium" Text="Premium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPremium" Style="font-weight: bold; text-align: center;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblInterest" Text="Interest"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtInterest" Style="font-weight: bold; text-align: center;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblTotalAmount" Text="Total Amount"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTotalAmount" Style="font-weight: bold; text-align: center;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export" style="border-radius:10px;" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>

        </div>
    </div>
</asp:Content>

