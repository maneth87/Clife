<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="edit_contact.aspx.cs" Inherits="Pages_GTLI_edit_contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>
  
   <%--Auto Complete--%>
    <link href="../../Scripts/themes/base/jquery.ui.all.css" rel="stylesheet" />    
    <script src="../../Scripts/ui/jquery.ui.core.js"></script>
    <script src="../../Scripts/ui/jquery.ui.widget.js"></script>
    <script src="../../Scripts/ui/jquery.ui.position.js"></script>
    <script src="../../Scripts/ui/jquery.ui.autocomplete.js"></script>
    <script src="../../Scripts/ui/jquery.ui.menu.js"></script>
    <%--End Auto Complete--%>
   
    <ul class="toolbar">
        <li>
            <input type="button"  onclick="EditContact()" style="background: url('../../App_Themes/functions/edit.png') no-repeat; border: none; height: 40px; width: 90px;" />
        </li>
        <li>
            <!-- Button Search-->
            <input type="button"  onclick="ShowSearchForm();" style="background: url('../../App_Themes/functions/search.png') no-repeat; border: none; height: 40px; width: 90px;" />      
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
        .ui-autocomplete {
            z-index: 5000;
        }
    </style>
    <script type="text/javascript">
        //Validation Section
        function ValidateNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }

        //Company Name autocomplete
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

        //Edit Contact
        function EditContact() {
            var contact_id = $('#Main_hdfContactID').val();
            if (contact_id == "") {
                alert("No Contact to edit.");
            } else {
                $('#myModalEditContact').modal('show');
            }
        }

        //Fucntion to check only one checkbox
        function SelectSingleCheckBox(ckb, contact_id, contact_name, contact_phone, contact_email) {
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

                $('#Main_hdfContactID').val(contact_id);

                /// Set text to textboxes for edition                
                $('#Main_txtContactNameEdit').val(contact_name);
                $('#Main_txtContactPhoneEdit').val(contact_phone);
                $('#Main_txtContactEmailEdit').val(contact_email);
                             
            } else {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "white");

                $('#Main_hdfContactID').val("");

                //Remove text from textboxes in edit form                                         
                $('#Main_txtContactNameEdit').val("");
                $('#Main_txtContactPhoneEdit').val("");
                $('#Main_txtContactEmailEdit').val("");
                
            }
        }

        //Show Search Form
        function ShowSearchForm() {
            $("#txtCompanyName").val("");

            $("#myModalSearchContact").modal("show");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
    <%-- Form Design Section--%>
    <br />
    <br />
    <br />
    
    <div class="panel panel-default">
        <div class="panel-heading">

            <h3 class="panel-title">Edit Company Contact</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>               
        
            <asp:GridView ID="gvContact" runat="server" Font-Names="Arial" Width="100%"
                AutoGenerateColumns="False" DataKeyNames="GTLI_Contact_ID">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="IndentBottom" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <%-- <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("GTLI_Contact_ID" ) + "\", \"" + Eval("Contact_Name" ) + "\", \"" + Eval("Contact_Phone" ) + "\", \"" + Eval("Contact_Email" ) + "\");" %>' />
                            
                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            No.
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Name" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblcontact_name" Text='<%# Bind("Contact_Name")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtcontact_name" Text='<%# Bind("Contact_Name")%>' runat="server" Width="90%"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorContactName" ForeColor="Red" runat="server" ErrorMessage="Require Contact Name" ControlToValidate="txtcontact_name" Text="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Phone" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="250" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblcontact_phone" Text='<%# Bind("Contact_Phone")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtcontact_phone" Text='<%# Bind("Contact_Phone")%>' runat="server" Width="85%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorContactPhone" ForeColor="Red" runat="server" ErrorMessage="Require Contact Phone" ControlToValidate="txtcontact_phone" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="AmountNumberRegularExpressionValidator" runat="server"
                                ControlToValidate="txtcontact_phone" ErrorMessage="Please enter numerical value only."
                                ValidationExpression="^[0-9]*$" ForeColor="Red" Font-Size="9pt" Font-Names="Arial" Text="*"></asp:RegularExpressionValidator>

                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Email" ItemStyle-CssClass="IndentLeft" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblcontact_email" Text='<%# Bind("Contact_Email")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtcontact_email" Text='<%# Bind("Contact_Email")%>' runat="server" Width="90%"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorContactEmail" ForeColor="Red" runat="server" ErrorMessage="Require Contact Email" ControlToValidate="txtcontact_email" Text="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>

                    </asp:TemplateField>                   

                </Columns>
            </asp:GridView>
           
        </div>
    </div>

     <div style="text-align: center; width: 100%">        
         <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ValidationGroup="1" ShowSummary="False" />
     </div>

     <!-- Modal Edit Contact Form -->
    <div id="myModalEditContact" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalContactEditHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="H1">Update Contact Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td width="100px">Contact Name:</td>
                     <td>
                        <asp:TextBox Width="60%" ID="txtContactNameEdit" runat="server" Height="15" MaxLength="50" ValidationGroup="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorContactNameEdit" runat="server" ErrorMessage="Rquire Contact" Text="*" ControlToValidate="txtContactNameEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>               
                <tr>
                    <td width="100px">Contact Phone:</td>
                     <td>
                        <asp:TextBox Width="60%" ID="txtContactPhoneEdit" runat="server" Height="15" MaxLength="25" onkeyup="ValidateNumber(this)" ValidationGroup="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorContactPhoneEdit" runat="server" ErrorMessage="Rquire Contact Phone" Text="*" ControlToValidate="txtContactPhoneEdit" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>  
                <tr>
                    <td width="100px">Contact Email:</td>
                     <td>
                        <asp:TextBox Width="60%" ID="txtContactEmailEdit" runat="server" Height="15" MaxLength="50" ValidationGroup="2"></asp:TextBox>
                         &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ControlToValidate="txtContactEmailEdit" ErrorMessage="Invalid email format."  ValidationGroup="2" ForeColor="Red" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>      
            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnUpdate" class="btn btn-primary" Style="height: 27px;" TabIndex="9" runat="server" Text="Update" OnClick="btnUpdate_Click" ValidationGroup="2" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal Edit Contact Form-->

    <%--Modal Search Contact Form --%>
    <div id="myModalSearchContact" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalSearchContactHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 class="panel-title">GTLI Contact Search</h3>
        </div>
        <div class="modal-body">
            <%--Modal Body--%>
           <table width="100%">
                <tr>
                    <td width="60px" >Company:</td>
                    <td width="87%">
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="96%" Font-Size="10pt" TabIndex="1" ClientIDMode="Static" MaxLength="255"></asp:TextBox>
                
                    </td>  
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSearchContact" runat="server" Text="*" ForeColor="Red" ValidationGroup="3" ControlToValidate="txtCompanyName"  ErrorMessage="RequiredFieldValidator" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                          
                </tr>       
            </table>
        </div>

        <div class="modal-footer">
          
            <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Search" OnClick="btnSearch_Click"  ValidationGroup="3"  />
            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <%--End Modal GTLI Contact Search--%>

     <%--Hidden Fields--%>
    <asp:HiddenField ID="hdfContactID" runat="server" />
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
</asp:Content>

