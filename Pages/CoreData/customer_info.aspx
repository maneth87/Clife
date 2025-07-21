<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="customer_info.aspx.cs" Inherits="Pages_Business_customer_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
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
            width: 70%; /* desired relative width */
            left: 20%; /* (100%-width)/2 */
            /* place center */
            margin-left: auto;
            margin-right: auto;
            height: auto;
        }

        .g_row {
            padding: 5px;
            vertical-align: middle;
        }

        .g_header {
            background-color: #ebe8e8;
        }

        .g_row_center {
            text-align: center;
        }

        .div_button {
            float: right;
            padding-right: 10px;
        }

        .btn_save_change {
            background-color: #36bdd7;
            border-radius: 6px;
        }
       
    </style>
    <script>
      
        var cust_id_obj;

        $(document).ready(function () {
        
            cust_id_obj = $('#<%=hdfCustomerID.ClientID%>');


        });
        function search_customer() {
            // $('#btnSearchCustomer').click(function () {

            // alert('Name=' + cust_name.val() + ', cust no=' + cust_no.val() + ', id card=' + id_card.val() + ', cust type=' + cust_type.val());
            if (cust_name.val() != '' || cust_no.val() != '' || id_card.val() != '' || cust_type.val() != '') {

                $('#modal_search_customer').modal('hide');
                $('#<%=btnSearch.ClientID%>').click();

            }

            // });
        }
        function add_new() {
            //  window.open("customers.aspx", true);
            window.location.href = 'customers.aspx';
            return;
        }
        function get_cust_id(cust_id) {
            window.location.href = 'customers.aspx?cust_id=' + cust_id;

        }
        function openModal() {
            $('#<%=pnSearch.ClientID%>').modal();
        }

        function check(ckb, cust_id)
        {
            var GridView = ckb.parentNode.parentNode.parentNode;
            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                   

                }
                
            }
            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#36bdd7");
                $('#<%=hdfCustomerID.ClientID%>').val(cust_id);
            }
            else {
                $('#<%=hdfCustomerID.ClientID%>').val('');
            }
        }
        function confirmOK()
        {
           
            if ($('#<%=hdfCustomerID.ClientID%>').val() == '' || $('#<%=hdfCustomerID.ClientID%>').val() == null) {
                alert('Please select any records.');
            }
            else {
             
                $('#<%=btnOK.ClientID%>').click();
                $('#<%= pnSearch.ClientID%>').modal('hide');
            }
        }
    </script>
    <div id="div_message" runat="server" style="width:100%; height:30px; border-radius:10px; color:white; background-color:red; text-align:center;vertical-align:middle; font-weight:bolder; padding: 5px 5px 5px 5px; margin-top:200px;"></div>
    <div id="div_body" runat="server">
    <ul class="toolbar" style="float: right;">
      
        <li>
           
            <input type="button" onclick="openModal();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px; margin: 0 0 0 0;" />

        </li>
    </ul>
    <div style="padding: 0 0 0 0; margin: 0 0 0 0; height: 45px; border: 0px solid red;"></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="ContentPanel" runat="server">
        <ContentTemplate>
            <%--Operation here--%>

            <div class="panel panel-default">

                <div class="panel-heading" style="vertical-align:middle;">
                     <h3 class="panel-title">Personal Detail </h>
                  
                    <%--<ul class="toolbar" style="float:right;">
                        <li>
                            <input type="button" onclick="add_new();" style="background: url('../../App_Themes/functions/add.png') no-repeat; border: none; height: 40px; width: 90px;" />
                        </li>
                        <li>
                            <input type="button" data-toggle="modal" data-target="#modal_search_customer" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />
                        </li>
                    </ul>--%>
                </div>
                <div class="panel-body">
                    <table id="tblPersonalDetail" style="width: 100%;">
                        <tr>
                            <td>Cusotmer No.</td>
                            <td>
                                <asp:TextBox ID="txtCustNo" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td>First Name</td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                            </td>
                            <td>Last Name</td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                            </td>
                            <td>Gender</td>
                            <td>
                                <asp:TextBox ID="txtGender" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td>Birth Date</td>
                            <td>
                                <asp:TextBox ID="txtBirthDate" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td>Khmer First Name</td>
                            <td>
                                <asp:TextBox ID="txtKhmerFirstName" runat="server"></asp:TextBox>
                            </td>
                            <td>Khmer Last Name</td>
                            <td>
                                <asp:TextBox ID="txtKhmerLastName" runat="server" ></asp:TextBox>
                            </td>
                            <td>ID Type</td>
                            <td>
                                <asp:DropDownList ID="ddlIDType" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            
                            <td>ID Number</td>
                            <td>
                                <asp:TextBox ID="txtIDNo" runat="server"></asp:TextBox>
                            </td>
                            
                            <td>Natinality</td>
                            <td>
                                <asp:DropDownList ID="ddlNationality" runat="server"></asp:DropDownList>
                            </td>
                             <td>Prior First Name</td>
                            <td>
                                <asp:TextBox ID="txtpriorFirstName" runat="server"></asp:TextBox>
                            </td>
                             <td>Prior Last Name</td>
                            <td>
                                <asp:TextBox ID="txtPriorLastName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mother First Name</td>
                            <td>
                                 <asp:TextBox ID="txtMotherFirstName" runat="server"></asp:TextBox>
                            </td>
                            <td>Mother Last Name</td>
                            <td>
                                <asp:TextBox ID="txtMotherLastName" runat="server"></asp:TextBox>
                            </td>
                            <td>Fater First Name</td>
                            <td>
                                <asp:TextBox ID="txtFatherFirstName" runat="server" ></asp:TextBox>
                            </td>
                            <td>Father Last Name</td>
                            <td>
                                 <asp:TextBox ID="txtFatherLastName" runat="server" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <div class="div_button">
                                    <asp:Button ID="btnEditCustomer" runat="server" CssClass="btn" Text="Edit" OnClick="btnEditCustomer_Click" />
                                    <asp:Button ID="btnSaveCustPersonal" runat="server" Text="Save Change" class="btn_save_change" OnClick="btnSaveCustPersonal_Click" OnClientClick="return confirm('Click OK to update customer\'s personal detail.');"/>
                                </div>

                            </td>

                        </tr>
                    </table>
                </div>
                <div class="panel-heading">
                    <h3 class="panel-title">Contact</h3>
                </div>
                <div class="panel-body">
                    <table id="Table1" style="width: 100%;">
                        <tr>
                            <td>Mobile 1</td>
                            <td>
                                <asp:HiddenField ID="hdfpolicyID" runat="server" Value="" />
                                <asp:TextBox ID="txtMobile1" runat="server"></asp:TextBox>
                            </td>
                            <td>Mobile 2</td>
                            <td>
                                <asp:TextBox ID="txtMobile2" runat="server"></asp:TextBox>
                            </td>
                            <td>Home 1</td>
                            <td>
                                <asp:TextBox ID="txtHomePhone1" runat="server"></asp:TextBox>
                            </td>
                            <td>Home 2</td>
                            <td>
                                <asp:TextBox ID="txtHomePhone2" runat="server"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>

                            <td>Office 1</td>
                            <td>
                                <asp:TextBox ID="txtOfficePhone1" runat="server"></asp:TextBox>
                            </td>
                            <td>Office 2</td>
                            <td>
                                <asp:TextBox ID="txtOfficePhone2" runat="server"></asp:TextBox>
                            </td>
                            <td>Fax 1</td>
                            <td>
                                <asp:TextBox ID="txtFax1" runat="server"></asp:TextBox>
                            </td>
                            <td>Fax 2</td>
                            <td>
                                <asp:TextBox ID="txtFax2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>E-mail</td>
                            <td colspan="7">
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <div class="div_button">
                                    <asp:Button ID="btnEditContact" runat="server" CssClass="btn" Text="Edit" OnClick="btnEditContact_Click" />
                                    <asp:Button ID="btnSaveContact" runat="server" Text="Save Change" class="btn_save_change" OnClick="btnSaveContact_Click" OnClientClick="return confirm('Click OK to update contact.');"/>
                                </div>

                            </td>

                        </tr>
                    </table>
                </div>
                <div class="panel-heading">
                    <h3 class="panel-title">Address</h3>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="upAddress" runat="server">
                        <ContentTemplate>
                            <table id="Table2" style="width: 100%;">
                                <tr>
                                    <td>Address 1</td>
                                    <td colspan="5">
                                        <asp:HiddenField ID="hdfAddressID" runat="server" Value="" />
                                        <asp:TextBox ID="txtAddress1" runat="server" Style="width: 100%;"></asp:TextBox>
                                    </td>


                                </tr>
                                <tr>
                                    <td>Address 2</td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtAddress2" runat="server" Style="width: 100%;"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>Address 3</td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtAddress3" runat="server" Style="width: 100%;"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>Province</td>
                                    <td>
                                        <asp:TextBox ID="txtProvice" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Country</td>
                                    <td>
                                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>Zip Code</td>
                                    <td>
                                        <asp:TextBox ID="txtZipCode" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td colspan="6">
                                        <div class="div_button">
                                            <asp:Button ID="btnEditAddress" runat="server" CssClass="btn" Text="Edit" OnClick="btnEditAddress_Click" />
                                            <asp:Button ID="btnSaveAddress" runat="server" Text="Save Change" class="btn_save_change" OnClick="btnSaveAddress_Click" OnClientClick="return confirm('Click OK to update address.');" />
                                        </div>

                                    </td>

                                </tr>
                            </table>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>

                <label id="lblMessage" runat="server"></label>
            </div>

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <!---Modal Search--->
    <asp:Panel runat="server" ID="pnSearch" class="modal hide fade large" TabIndex="-1" role="dialog" aria-labelledby="pnSearch" aria-hidden="true" data-keyboard="false" data-backdrop="static">
        <asp:UpdatePanel runat="server" ID="upSearch" UpdateMode="Conditional">
            <ContentTemplate>
<%--                <div id="modal_search_customer" class="modal hide fade large" tabindex="-1" role="dialog" aria-labelledby="modal_search_customer" aria-hidden="true" data-keyboard="false" data-backdrop="static">--%>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h3 id="H2">Search Customer</h3>
                    </div>
                    <div class="modal-body">

                        <div class="tab-content" style="height: 80px; overflow: hidden;">

                            <table style="width: auto;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustNo" Text="Customer Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchCustNo"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchCustName" Text="Customer Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchCustName"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchGender" Text="Gender"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSearchGender">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchIDCard" Text="ID Card"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchIDCard"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblSearchBirthDate" Text="Birth Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearchBirthDate" placeholder="DD/MM/YYYY"></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="text-align:center;">
                                         
                                    </td>
                                </tr>
                            </table>

                        </div>


                        <div class="tab-content" style="height: auto; max-height: 250px; overflow: auto;">
                            <asp:GridView ID="gvCustomer" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" OnRowCommand="gvCustomer_RowCommand" >
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("App_Register_ID" ) + "\", \"" + Eval("Product_ID" ) + "\", \"" + Eval("System_Sum_Insure" ) + "\", \"" + Eval("System_Premium" ) + "\", \"" + Eval("Pay_Mode" ) + "\", \"" + Eval("Gender" ) + "\", \"" + Eval("Age_Insure" ) + "\", \"" + Eval("Status_Code" ) + "\", \"" + Eval("Pay_Up_To_Age" ) + "\", \"" + Eval("Assure_Up_To_Age" ) + "\", \"" + Eval("Pay_Year" ) + "\", \"" + Eval("Assure_Year" ) + "\", \"" + Eval("Effective_Date" ) + "\");" %>' />--%>
                                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "check(this,\"" + DataBinder.Eval(Container.DataItem,"Customer_ID").ToString() + "\");" %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="30" />
                                        <ItemStyle Width="30" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   

                                     <asp:TemplateField HeaderText="Customer No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Customer_ID").ToString()  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Last_Name").ToString() + " " + DataBinder.Eval(Container.DataItem,"First_Name").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gender">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGender" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Gender").ToString() == "1" ? "M" : "F" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Birth Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBirthDate" runat="server" Text='<%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"Birth_Date").ToString()).ToString("dd/MMM/yyyy")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>


                    </div>
                    <div class="modal-footer">
                        <%--<input type="button" id="btnSearchCustomer" onclick="search_customer();" value="Search" class="btn btn-primary" />--%>
                        <asp:HiddenField ID="hdfCustomerID" runat="server" Value="" />
                        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" />
                        <asp:Button runat="server" ID="btnOK" OnClick="btnOK_Click" CssClass="btn btn-primary" Text="OK" style="display:none;"  />
                        <input type="button" class="btn btn-primary" value="OK"  onclick="confirmOK();" />
                    </div>
                <%--</div>--%>
            </ContentTemplate>
            <Triggers>
             <%-- <asp:PostBackTrigger  ControlID="gvCustomer" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    </div>
</asp:Content>

