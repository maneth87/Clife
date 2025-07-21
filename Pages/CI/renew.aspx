<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="renew.aspx.cs" Inherits="Pages_CI_renew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <link href="../../Scripts/bootstrap/datepicker/css/datepicker.css" rel="stylesheet" />
    <script src="../../Scripts/bootstrap/datepicker/js/bootstrap-datepicker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <script>
        var obj_remarks;
        var obj_policy_id;
        var obj_new_effective_date;
        var obj_new_maturity_date;
        var obj_new_expiry_date;
        var obj_dob;
        var obj_customer_age;
        $(document).ready(function () {
            $('.datepicker').datepicker();

            $('#tbl_filter').css('margin-left', ($('#tbl_content').width() / 2) - ($('#tbl_filter').width() / 2));
            $('#tbl_filter').css('border', 'solid 2px black');

            $('#btn_client_search').click(function () {
                $('#<%=btnSearch.ClientID%>').click();
            });

            obj_remarks = $('#<%=txtRemarks.ClientID%>');
            obj_policy_id = $('#<%=hfdPolicyID.ClientID%>');
            obj_new_effective_date = $('#<%=txtNewEffectiveDate.ClientID%>');
            obj_new_maturity_date = $('#<%=txtNewMaturityDate.ClientID%>');
            obj_new_expiry_date = $('#<%=txtNewExpiryDate.ClientID%>');
            obj_dob = $('#<%=hfdDob.ClientID%>');
            obj_customer_age = $('#<%=txtCustomerAge.ClientID%>');


            //obj_new_effective_date.change( function () {
            //   // changed_effective();
            //    return alert('hello world');
            //});
            $('#<%=txtNewEffectiveDate.ClientID%>').datepicker().on('changeDate',function () {
                changed_effective();
            });
        });
        function open_renew_dialog(policy_id, new_effective_date, dob) {
            $('#modal_policy_search').modal('show');
            if (policy_id != null && policy_id != "") {
                /*
                recalculate maturity date, expiry date and customer age

                */
                obj_new_effective_date.val(new_effective_date);
                obj_dob.val(dob);
                obj_policy_id.val(policy_id);

                $.ajax({
                    type: "POST",
                    url: "../../PolicyWebService.asmx/CI_Calculate_Renew_Policy",
                    data: "{policy_id:'" + policy_id + "', new_effecive_date:'" + new_effective_date + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d.length > 0) {

                            
                            var obj = data.d;
                            obj_customer_age.val(obj[0]);
                            obj_new_maturity_date.val(obj[1]);
                            obj_new_expiry_date.val(obj[2]);
                           
                            /*disable controls*/
                            obj_customer_age.prop('disabled', 'false');
                            obj_new_maturity_date.prop('disabled', 'false');
                            obj_new_expiry_date.prop('disabled', 'false');
                           // obj_new_effective_date.prop('disabled', 'false');
                           
                        } else {
                            alert('Renew calculation fial.');
                        }
                       
                    }
                });

                $('#modal_policy_search').modal('show');
            }

            else {
                alert('No policy is selected.');
            }
        }
        function renew() {
            if (obj_policy_id.val() != "" && obj_policy_id != null) {

                $('#<%=btnRenew.ClientID%>').click();
            }
            else {
                alert('Please select policy to terminate.');
            }
        }

        function changed_effective()
        {
          
            $.ajax({
                type: "POST",
                url: "../../PolicyWebService.asmx/CI_Calculate_Renew_Policy",
                data: "{policy_id:'" + obj_policy_id.val() + "', new_effecive_date:'" + obj_new_effective_date.val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {


                        var obj = data.d;
                        obj_customer_age.val(obj[0]);
                        obj_new_maturity_date.val(obj[1]);
                        obj_new_expiry_date.val(obj[2]);

                        /*disable controls*/
                        obj_customer_age.prop('disabled', 'false');
                        obj_new_maturity_date.prop('disabled', 'false');
                        obj_new_expiry_date.prop('disabled', 'false');
                        // obj_new_effective_date.prop('disabled', 'false');

                    } else {
                        alert('Renew calculation fial.');
                    }

                }
            });
        }
    </script>
    <h1>Renew Policies</h1>
    <div>
        <table class="table-layout" id="tbl_content">
            <tr>
                <td>
                    <table id="tbl_filter">
                        <tr>
                            <td colspan="3" style="font-weight: bold;">Filter</td>

                        </tr>
                        <tr>
                            <td>Customer Name</td>
                            <td>
                                <asp:TextBox ID="txtCustomerName" runat="server" ></asp:TextBox>
                            </td>
                            <td rowspan="3" style="vertical-align: middle;">
                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/App_Themes/functions/search.png" Style="display: none;" OnClick="btnSearch_Click" />
                                <input type="button" class="btn btn-primary" value="Search" id="btn_client_search" />
                            </td>
                        </tr>
                        <tr>
                            <td>Expiry Date</td>
                            <td>
                                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="datepicker"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Policy Number</td>
                            <td>
                                <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="vertical-align: middle;">
                <td style="vertical-align: middle;">
                    <div class="container">
                        <div id="tab-content" style="padding-top: 10px;">
                            <asp:GridView ID="gv_policy" CssClass="grid-layout" runat="server" AutoGenerateColumns="False" Width="100%" HorizontalAlign="Center" >
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
                                            <asp:Label ID="lblDOB" runat="server" Text='<%#  Convert.ToDateTime (Eval("birth_date").ToString()).ToString("dd-MM-yyyy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveDate" runat="server" Text='<%#  Convert.ToDateTime (Eval("Effective_date").ToString()).ToString("dd-MM-yyyy HH:mm:ss") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Phone Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("mobile_phone1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Expiry Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpiryDate" runat="server" Text='<%#  Convert.ToDateTime (Eval("expiry_date").ToString()).ToString("dd-MM-yyyy HH:mm:ss") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maturity Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Convert.ToDateTime( Eval("maturity_date").ToString()).ToString("dd/MM/yyyy") %>'></asp:Label>
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

                                            <a href="#" style="color: green; font-weight: bold;" onclick='<%# "open_renew_dialog(\"" + Eval("policy_id") + "\", \""+ Convert.ToDateTime( Eval("maturity_date")).ToString("dd/MM/yyyy") + "\", \""+ Convert.ToDateTime( Eval("birth_date")).ToString("dd/MM/yyyy") +"\");"%>'>Renew</a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

                        </div>
                    </div>

                </td>
            </tr>

        </table>

    </div>
    <div id="modal_policy_search" class="modal hide fade " tabindex="-1" role="dialog" aria-labelledby="SearchCustomer" aria-hidden="true" data-keyboard="false" data-backdrop="static">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Renew Policy</h3>
        </div>
        <div class="modal-body ">
            <asp:HiddenField runat="server" ID="hfdPolicyID" />
             <asp:HiddenField runat="server" ID="hfdDob" />
            <div id="search_policy" class="tab-pane active">
                <table style="width: 100%;">
                    <tr>
                        <td>New Effective Date
                            <span style="color:red;">*</span>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewEffectiveDate" CssClass="datepicker" Width="98%" ></asp:TextBox>
                        </td>

                    </tr>
                     <tr>
                        <td>New Maturity Date
                             <span style="color:red;">*</span>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewMaturityDate" CssClass="datepicker" Width="98%"></asp:TextBox>
                        </td>

                    </tr>
                     <tr>
                        <td>New Expiry Date
                             <span style="color:red;">*</span>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewExpiryDate" CssClass="datepicker" Width="98%"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>Customer Age
                             
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCustomerAge"  Width="98%"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>Remarks
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRemarks" Width="98%"></asp:TextBox>
                        </td>

                    </tr>
                     <tr>
                        <td>Approver
                        </td>
                        <td>
                            
                            <asp:DropDownList ID="ddlApprover" runat="server"  Width="98%"></asp:DropDownList>
                        </td>

                    </tr>
                </table>
            </div>

        </div>
        <div class="modal-footer">
            <asp:Button ID="btnRenew" runat="server" OnClick="btnRenew_Click" Style="display: none;" />
            <input type="button" id="btnClientRenew" value="Renew" onclick="renew();" class="btn btn-primary" />
            <input type="button" id="btnCancel" value="Cancel" class="btn" data-dismiss="modal" />
        </div>
    </div>


</asp:Content>

