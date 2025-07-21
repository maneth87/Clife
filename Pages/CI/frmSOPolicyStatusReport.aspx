<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="frmSOPolicyStatusReport.aspx.cs" Inherits="Pages_CI_frmTermiateSOReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
    <script>
        var obj_remarks;
        var obj_policy_id;
        $(document).ready(function () {

            $('.datepicker').datepicker();
            $('#tbl_filter').css('margin-left', ($('#tbl_content').width() / 2) - ($('#tbl_filter').width() / 2));
            $('#tbl_filter').css('border', 'solid 2px black');

            $('#btn_client_search').click(function () {
                $('#<%=btnSearch.ClientID%>').click();
            });

        });
        function open_terminate_dialog(policy_id) {
            if (policy_id != null && policy_id != "") {
                $('#modal_policy_search').modal('show');


                obj_policy_id.val(policy_id);
            }
            else {
                alert('No policy is selected.');
            }
        }
       
    </script>
    <h1>Simple One Policies Status Report </h1>
    <div>
        <table class="table-layout" id="tbl_content">
            <tr>
                <td>
                    <table id="tbl_filter">
                        <tr>
                            <td colspan="3" style="font-weight: bold;">Report Filter</td>

                        </tr>
                        <tr>
                            <td>
                                From Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtFDate" runat="server" CssClass=" datepicker"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtTDate" runat="server" CssClass=" datepicker"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td>Customer Name</td>
                            <td>
                                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                            </td>
                            <td rowspan="3" style="vertical-align: middle;">
                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" Style="display: none;"  />
                                <input type="button" class="btn btn-primary" value="Search" id="btn_client_search" />
                            </td>
                        </tr>
                        <tr>
                            <td>Gender</td>
                            <td>
                                <asp:DropDownList ID="ddlGender" runat="server">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="F" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="M" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Policy Number</td>
                            <td>
                                <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Policy Status</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPolicyStatus" DataSourceID="sds_policy_status_type" DataTextField="detail" DataValueField="policy_status_type_id" AppendDataBoundItems="true">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                                                <asp:GridView ID="gv_policy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center">
                                <SelectedRowStyle BackColor="#EEFFAA" />
                                <Columns>
                                  
                                    <asp:TemplateField HeaderText="No" Visible="true">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOlicyID" runat="server" Text='<%#Eval("policy_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy Number" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPolicyNumber" runat="server" Text='<%#Eval("policy_number") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Card" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIDCard" runat="server" Text='<%#Eval("ID_Card") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Surname(En)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("last_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Given Name(En)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("first_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Surname(Kh)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblKHLastName" runat="server" Text='<%# Eval("khmer_last_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Given Name(Kh)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblKHFirstName" runat="server" Text='<%# Eval("khmer_first_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Gender">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender").ToString()=="0"? "F":Eval("Gender").ToString()=="1"?"M" : "N/A" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="DOB">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("birth_date") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Phone Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("mobile_phone1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPolicyStatus" runat="server" Text='<%# Eval("policy_status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>

                                            <a href="#" style="color: red; font-weight: bold;" onclick='<%# "open_terminate_dialog(\"" + Eval("policy_id") + "\");"%>'>Terminate</a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

                </td>
            </tr>
        </table>

    </div>

    <asp:SqlDataSource ID="sds_policy_status_type" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="exec SP_GET_ALL_POLICY_STATUS_TYPE;"></asp:SqlDataSource>
</asp:Content>

