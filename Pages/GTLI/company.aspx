<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="company.aspx.cs" Inherits="Pages_GTLI_company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" Runat="Server">
     <ul class="toolbar">  
        <li>
            <input type="button"  onclick="DeleteCompany()" style="background: url('../../App_Themes/functions/delete.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>

        <li>
            <input type="button"  onclick="EditCompany()" style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>    
        <li>
            <asp:ImageButton ID="ImgBtnSearch" runat="server"  ImageUrl="~/App_Themes/functions/search.png" ValidationGroup="1" OnClick="ImgBtnSearch_Click"  />
        </li>      
       
    </ul>
   <style>
        .IndentLeft {
            padding-left: 5px;          
        }

        .IndentRight {
            padding-right: 5px;            
        }
        .IndentBottom {
            padding-bottom: 7px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

    <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />    
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>


    <%--Javascript--%>
    <script type="text/javascript">
        PageMethods.GetCompanyName(function (results) {

            $("#txtCompanyName").autocomplete({
                source: function (request, response) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                    response($.grep(results, function (item) {
                        return matcher.test(item);
                    }));
                }
            });
        });

        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb, company_id, company_name, type_of_business, company_email, company_address) {
            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");
                }
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e5e5e5");

                $('#Main_hdfCompanyID').val(company_id);

                /// Set text to textboxes for edition                
                $('#Main_txtCompanyNameEdit').val(company_name);
                $('#Main_txtCompanyEmailEdit').val(company_email);
                $('#Main_txtCompanyAddressEdit').val(company_address);
                $('#Main_ddlBusinessTypeEdit').val(type_of_business);

                /// Delete Company
                $('#Main_txtDeleteCompanyName').val(company_name + ' ?');

            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                $('#Main_hdfCompanyID').val("");

                //Remove text from textboxes in edit form                                         
                $('#Main_txtCompanyNameEdit').val("");
                $('#Main_txtCompanyEmailEdit').val("");
                $('#Main_txtCompanyAddressEdit').val("");
                $('#Main_ddlBusinessTypeEdit').selectedIndex = 0;

                //Remove Delete Company
                $('#Main_txtDeleteCompanyName').val("");
            }
        }

        //Edit Company
        function EditCompany() {
            var company_id = $('#Main_hdfCompanyID').val();
            if (company_id == "") {
                alert("No company to edit.");
            } else {
                $('#myModalEditCompany').modal('show');
            }
        }

        //Delete Company
        function DeleteCompany() {
            var company_id = $('#Main_hdfCompanyID').val();
            if (company_id == "") {
                alert("No company to delete.");
            } else {
                $('#myModalDeleteCompany').modal('show');
            }
        }
      </script>

    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
    <table width="100%">
        <tr>
            <td width="60px" >Company:</td>
            <td width="30%">
                <asp:TextBox ID="txtCompanyName" runat="server" Width="100%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static" MaxLength="255"></asp:TextBox>
                
            </td>  
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompany" Text="*" runat="server" ControlToValidate="txtCompanyName" ErrorMessage="Require Company" ForeColor="Red" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>         
        </tr>       
    </table>
     <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Group Term Life Insurance: Company</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <asp:GridView ID="gvCompany" runat="server" Font-Names="Arial" Width="100%"
                AutoGenerateColumns="False" DataKeyNames="GTLI_Company_ID">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="IndentBottom" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("GTLI_Company_ID" ) + "\", \"" + Eval("Company_Name" ) + "\", \"" + Eval("Type_Of_Business" ) + "\", \"" + Eval("Company_Email" ) + "\", \"" + Eval("Company_Address" ) + "\");" %>' />
                            
                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Name" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblCompany" Text='<%# Bind("Company_Name")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type of Business" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblTypeOfBusiness" Text='<%# Bind("Type_Of_Business")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Email" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyEmail" Text='<%# Bind("Company_Email")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Address" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyAddress" Text='<%# Bind("Company_Address")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                  
                </Columns>
            </asp:GridView>

        </div>
    </div>

    <!-- Modal Edit Company Form -->
    <div id="myModalEditCompany" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalCompanyEditHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Update Company Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td width="120px">Company Name:<span style="color:red">*</span></td>
                     <td>
                        <asp:TextBox Width="86%" ID="txtCompanyNameEdit" runat="server" Height="15" MaxLength="255"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompanyNameEdit" runat="server" ErrorMessage="Rquire Company" Text="*" ControlToValidate="txtCompanyNameEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>               
                <tr>
                    <td width="120px"> Type of Business:</td>
                     <td>
                         <asp:DropDownList ID="ddlBusinessTypeEdit" Width="89.2%" runat="server"></asp:DropDownList>                                    
                         
                    </td>
                </tr>    
                <tr>
                    <td width="120px">Company Email:<span style="color:red">*</span></td>
                     <td>
                        <asp:TextBox Width="86%" ID="txtCompanyEmailEdit" runat="server" Height="15" MaxLength="255"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompanyEmailEdit" runat="server" ErrorMessage="Rquire Company Email" Text="*" ControlToValidate="txtCompanyEmailEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>   
                <tr>
                    <td width="120px">Company Address:<span style="color:red">*</span></td>
                     <td>
                        <asp:TextBox Width="86%" ID="txtCompanyAddressEdit" runat="server" Height="15" MaxLength="255"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompanyAddressEdit" runat="server" ErrorMessage="Rquire Company Address" Text="*" ControlToValidate="txtCompanyAddressEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>      
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnUpdate" class="btn btn-primary" Style="height: 27px;" TabIndex="9" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="2" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Edit Company Form-->

    <!-- Modal Delete Company Form -->
    <div id="myModalDeleteCompany" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalDeleteCompanyHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="myModalDeleteCompanyHeader">Delete Company Confirm</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table style="text-align: left;">
                <tr>
                    <td>Are you sure, you want to delete company
                        <asp:TextBox ID="txtDeleteCompanyName" runat="server" BorderStyle="None" Enabled="True" ReadOnly="True" BackColor="White" ForeColor="Red"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnDelete" class="btn btn-primary" Style="height: 27px;" runat="server" Text="Delete" OnClick="btnDelete_Click" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Delete Company Form-->

    <%--Hidden Fields--%>
    <asp:HiddenField ID="hdfCompanyID" runat="server" />
</asp:Content>

