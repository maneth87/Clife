<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="register_company.aspx.cs" Inherits="Pages_GTLI_register_company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <ul class="toolbar">
        <li>
            <asp:ImageButton ID="ImageBtnEdit" runat="server" ImageUrl="~/App_Themes/functions/edit.png" CausesValidation="false"  PostBackUrl="~/Pages/GTLI/edit_company.aspx" />
        </li>
        <li>
            <!-- Button to trigger modal register company form -->
            <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="~/App_Themes/functions/add.png"  ValidationGroup="1" OnClick="ImgBtnAdd_Click" />
            
        </li>
      
    </ul>
    <style>
        .linkbutton {
            cursor: pointer;
            text-decoration:none;
        }
    </style>
    <%--Javascript--%>
    <script type="text/javascript">
        //Add new business
        function AddNewBusiness() {
            $("#Main_txtBusinessName").val("");
            $("#myModalNewBusiness").modal('show');
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

            <h3 class="panel-title">Register Company</h3>
        </div>
        <div class="panel-body">
            <%--Content--%>
            <table width="100%">
                <tr>
                    <td>Company Name<span style="color: red">*</span></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCompany" runat="server" Width="97%" Font-Size="10pt" TabIndex="1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCompany" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtCompany" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Type of Business<span style="color: red">*</span> &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="hplAddNewBusiness" onclick="AddNewBusiness();" runat="server" Text="Add New Business" CssClass="linkbutton"></asp:HyperLink>                    
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlBusinessType" Width="98%" Height="36px" TabIndex="2" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorBusiness" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="ddlBusinessType" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Company Address<span style="color: red">*</span></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Width="97%" Font-Size="10pt" TabIndex="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorAddress" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtCompany" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Company Email
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ControlToValidate="txtCompanyEmail" ValidationGroup="1" ErrorMessage="- Invalid Email Address" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCompanyEmail" runat="server" Width="97%" Font-Size="10pt" TabIndex="4"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td width="100%">Contact Person<span style="color: red">*</span>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:TextBox ID="txtContactPerson" runat="server" Width="97%" Font-Size="10pt" TabIndex="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorContact" runat="server" ErrorMessage=" * " ValidationGroup="1"
                            ControlToValidate="txtCompany" Font-Names="Arial" Font-Size="9pt" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%">Phone Contact 
                 <asp:RegularExpressionValidator ID="RegularExpressionValidatorPhone" runat="server" ControlToValidate="txtPhone"
                     ErrorMessage=" - Please Enter Numbers Only" ValidationExpression="^\d+$" ForeColor="Red" ValidationGroup="1"
                     Font-Size="9pt" Font-Names="Arial"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%" class="auto-style1">
                        <asp:TextBox ID="txtPhone" runat="server" Width="97%" Font-Size="10pt" TabIndex="6"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td width="100%">Email Contact
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmailContact" runat="server" ValidationGroup="1" ControlToValidate="txtContactEmail" ErrorMessage="- Invalid Email Address" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:TextBox ID="txtContactEmail" runat="server" Width="97%" Font-Size="10pt" TabIndex="7"></asp:TextBox>

                    </td>
                </tr>           
                
            </table>
        </div>
    </div>

    <!-- Modal New Business Form -->
    <div id="myModalNewBusiness" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalBusinessHeader" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h3 id="myModalBusinessHeader">New Business Form</h3>
        </div>
        <div class="modal-body">
            <!---Modal Body--->
            <table width="100%">
                <tr>
                    <td width="100px">Business Name:</td>
                    <td>
                        <asp:TextBox Width="86%" ID="txtBusinessName" runat="server" Height="15" MaxLength="255"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorBusinessName" runat="server" ErrorMessage="Rquire Business" Text="*" ControlToValidate="txtBusinessName" ValidationGroup="2" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>                

            </table>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSave" class="btn btn-primary" Style="height: 27px;" TabIndex="9" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="2" />

            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
        </div>
    </div>
    <!--End Modal New Business Form-->

    <%-- Section Hidenfields Initialize  --%>
    <asp:HiddenField ID="hdfuserid" runat="server" />
    <asp:HiddenField ID="hdfusername" runat="server" />
</asp:Content>

