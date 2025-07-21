<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="customer_list1.aspx.cs" Inherits="Pages_Business_customer_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        <li>
            <input type="button" onclick="add_new();" style="background: url('../../App_Themes/functions/add.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <input type="button" data-toggle="modal" data-target="#modal_search_customer" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>

    <%--jTemplate--%>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/date.format.js"></script>
    <style>
        #tbl_customer_list {
            background-color: green;
        }

            #tbl_customer_list th {
                width: 200px;
                padding: 5px;
                font-weight: bold;
            }

        .modal.large {
            width: 60%; /* desired relative width */
            left: 20%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
        }
        .g_row {
            padding:5px;
            vertical-align:middle;
        }
        .g_header {
            background-color:#ebe8e8;
        }
        .g_row_center {
            text-align:center;
        }
    </style>
    <script>
        var cust_name;
        var cust_no;
        var id_card;
        var cust_type;
        $(document).ready(function () {
            cust_name = $('#<%=txtSearchCustName.ClientID%>');
            cust_no = $('#<%=txtSearchCustNo.ClientID%>');
            id_card = $('#<%=txtSearchIDCard.ClientID%>');
            cust_type = $('#<%=ddlSearchCustType.ClientID%>');

           
        });
        function search_customer()
        {
           // $('#btnSearchCustomer').click(function () {

               // alert('Name=' + cust_name.val() + ', cust no=' + cust_no.val() + ', id card=' + id_card.val() + ', cust type=' + cust_type.val());
                if (cust_name.val() != '' || cust_no.val() != '' || id_card.val() != '' || cust_type.val() != '') {

                    $('#modal_search_customer').modal('hide');
                    $('#<%=btnSearch.ClientID%>').click();

                }

            // });
        }
        function add_new()
        {
          //  window.open("customers.aspx", true);
            window.location.href = 'customers.aspx';
            return;
        }
        function get_cust_id(cust_id)
        {
            window.location.href = 'customers.aspx?cust_id=' + cust_id;

        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--Operation here--%>
            <br />
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Customers</h3>
                </div>
                <div class="panel-body">
                   <asp:GridView ID="gvCustomer" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gvCustomer_Sorting" OnPageIndexChanging="gvCustomer_PageIndexChanging"
                                AutoGenerateColumns="False" CssClass=" table-hover" Width="770px" PageSize="10" >
                                <SelectedRowStyle BackColor="#EEFFAA" />
                       
                                <Columns>
                                    <asp:TemplateField HeaderText="Customer Number" SortExpression="cust_no">
                                        <ItemStyle CssClass="g_row" />
                                        <HeaderStyle CssClass="g_header" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCustNo" Text='<%# Eval("cust_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name" SortExpression="last_name_en">
                                        <ItemStyle CssClass="g_row" />
                                        <HeaderStyle CssClass="g_header" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFirstNameEn" Text='<%# Eval("last_name_en") + " " + Eval("first_name_en") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gender" SortExpression="gender">
                                         <ItemStyle CssClass="g_row g_row_center" />
                                        <HeaderStyle CssClass="g_header" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblGender" Text='<%# GetGenderText(Convert.ToInt32(Eval("Gender").ToString()))%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Marital Status" SortExpression="marital_status">
                                        <ItemStyle CssClass="g_row g_row_center" />
                                        <HeaderStyle CssClass="g_header" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMaritalStatus" Text='<%# Eval("marital_status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="status">
                                        <ItemStyle CssClass="g_row g_row_center" />
                                        <HeaderStyle CssClass="g_header" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust ID" Visible="false">
                                        <ItemStyle CssClass=" g_row_center" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCustID" Text='<%# Eval("cust_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                         <ItemStyle CssClass="g_row g_row_center" />
                                        <HeaderStyle CssClass="g_header" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lbtDetail" CommandName="cmdLink" OnClientClick='<%# "get_cust_id(\"" + Eval("cust_id")  + "\");" %>'><span class="icon-eye-open" style="margin-right:5px; margin-left:5px;"></span> Details </asp:LinkButton>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                </div>
                <label id="lblMessage" runat="server"></label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
         <!---Modal Search--->
            <div id="modal_search_customer" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="modal_search_customer" aria-hidden="true" data-keyboard="false" data-backdrop="static">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="H2">Search Customer</h3>
                </div>
                <div class="modal-body">
                    <!---Modal Body--->
                    <ul class="nav nav-tabs" id="my_tab_customer">
                        <li class="active"><a href="#tab_customer_number" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Customer Number</a></li>
                        <li><a href="#tab_customer_info" data-toggle="tab" style="text-decoration: none; font-size: 9pt;">Customer Info.</a></li>                       
                    </ul>
                    <div class="tab-content" style="height: 80px; overflow: hidden;">
                        <div class="tab-pane active" id="tab_customer_number">
                            <table style="width: 70%;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustNo" Text="Customer Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchCustNo"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="tab-pane" style="height: 80px;" id="tab_customer_info">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustType" Text="Customer Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSearchCustType" DataSourceID="ds_customer_type" DataTextField="customer_type" DataValueField="customer_type_id" AppendDataBoundItems="true">
                                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustName" Text="Customer Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchCustName"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchIDCard" Text="ID Card"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchIDCard"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <input type="button" id="btnSearchCustomer" onclick="search_customer();" value="Search"  class="btn btn-primary" />
                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Style="display: none;" />
                </div>
            </div>
    <asp:SqlDataSource ID="ds_Customer_Type" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationDBContext %>" SelectCommand="SELECT Customer_Type_ID, Customer_Type  FROM Ct_Customer_Type ORDER BY Customer_Type"></asp:SqlDataSource>
</asp:Content>

