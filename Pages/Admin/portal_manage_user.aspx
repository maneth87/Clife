<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="portal_manage_user.aspx.cs" Inherits="Pages_Admin_portal_manage_user" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">
    <%--  <script src="../../Scripts/bootstrap/js/bootstrap.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">

    <script type="text/javascript">

        //Function Check Box when Edit user
        function SelectSingleCheckBox(ckb, user_id, user_name, isapproved, roleid, email, locked) {

            $('#Main_editUserName').val(user_name);
            $('#Main_editddlUserRole').val(roleid);
            $('#Main_editddlStatus').val(isapproved=="True" ? 1: 0);
            $('#Main_editEmail').val(email);
            $('#Main_hdfUserId').val(user_id);
            $('#Main_hdfOldRoleId').val(roleid);
          
            var user_name_login = $('#Main_hdfUserName').val();
                      

            $('#Main_editddlLocked').val(locked == "True" ? 1 : 0);

            var GridView = ckb.parentNode.parentNode.parentNode;

            var ckbList = GridView.getElementsByTagName("input");
            for (i = 0; i < ckbList.length; i++) {
                if (ckbList[i].type == "checkbox" && ckbList[i].id != ckb.id) {

                    ckbList[i].checked = false;
                    $("#" + ckbList[i].id).parent("td").parent("tr").css("background-color", "white");

                }
            }

            if (ckb.checked) {
                $("#" + ckb.id).parent("td").parent("tr").css("background-color", "#e0e0e0");

            }
        }

        //Function Update User Account

        function EditUser() {

            var btnEditUser = document.getElementById('<%= btnEditUser.ClientID %>'); //dynamically click button
            btnEditUser.click();

        }

        //Function Reset Password

        function ResetPwd(obj, user_id, username, email) {

            $('#Main_ResettxtUsername').val(username);
            $('#Main_ResettxtEmail').val(email);

            $('#Main_hdfResettxtUserId').val(user_id);
            $('#Main_hdfResettxtUsername').val(username);
        }

        //Function Reset Password Click

        function ResetPwdUser() {

            var btnResetPwd = document.getElementById('<%= btnresetpwd.ClientID%>');

            btnResetPwd.click();
        }

        //Function validate length input

        function ClientValidate(source, clientside_arguments) {
            //Test whether the length of the value is more than 6 characters
            if (clientside_arguments.Value.length >= 6) {
                clientside_arguments.IsValid = true;
            }
            else { clientside_arguments.IsValid = false }
        }

    </script>


    <ul class="toolbar">

        <%-- <li>
            <asp:ImageButton ID="ImgBtnSearch" runat="server" data-toggle="modal" data-target="#myModalSearchSaleAgentCode" ImageUrl="~/App_Themes/functions/search.png" />
        </li>--%>
        <li>
            <asp:ImageButton ID="ImgBtnEdit" runat="server" data-toggle="modal" data-target="#myModelEditUser" ImageUrl="~/App_Themes/functions/edit.png" />
        </li>
        <li>
            <asp:ImageButton ID="ImgBtnAdd" data-toggle="modal" data-target="#myModalAddUser" runat="server" ImageUrl="~/App_Themes/functions/add.png" />
        </li>

    </ul>

    <br />
    <br />
    <br />

    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Manage User</h3>
        </div>
        <div class="panel-body">

            <table>
                <tr>
                    <td style="padding-left: 7px; width: 8%;">User Name:&nbsp;</td>

                    <td style="width: 30%; vertical-align: calc()">
                        <asp:TextBox ID="txtUsername" placeholder="User name ..." runat="server" Width="90%" ClientIDMode="Static" MaxLength="100"></asp:TextBox>
                    </td>
                    <td>Role:&nbsp;&nbsp;</td>
                    <td style="vertical-align: calc()">
                        <asp:DropDownList ID="ddlRole" runat="server" AppendDataBoundItems="true" class="span2">
                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;Approved:&nbsp;&nbsp;</td>
                    <td style="vertical-align: calc()">
                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" class="span2">
                            <asp:ListItem Selected="True" Value="-1">.</asp:ListItem>
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 3%">&nbsp;</td>
                    <td style="width: 42%; vertical-align: top">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn" Style="border: 1pt groove #d5d5d5; Width: 70px" Text="Search" ValidationGroup="search" OnClick="btnSearch_Click" ToolTip="Search User" />
                    </td>
                </tr>
            </table>

            <br />
            <asp:Label ID="resultSearch" runat="server" Text=""></asp:Label>
            <%-- Grid View --%>
            <asp:GridView ID="GvUser" PageSize="20" OnPageIndexChanged="GvUser_PageIndexChanged" OnPageIndexChanging="GvUser_PageIndexChanging" CssClass="grid-layout; grid" Width="100%" runat="server" AutoGenerateColumns="False" BorderColor="Black" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" HorizontalAlign="Center" PagerStyle-HorizontalAlign="Center">

                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckb1" runat="server" onclick='<%# "SelectSingleCheckBox(this, \"" + Eval("UserId" ) + "\",\"" + Eval("UserName" ) + "\",\""  + Eval("IsApproved") + "\",\"" + Eval("RoleId" ) + "\",\"" + Eval("Email") + "\",\"" + Eval("IsLockedOut") + "\");" %>' />
                            <asp:HiddenField ID="hdfOldSaleAgentID" runat="server" Value='<%# Bind("UserId")%>' />
                          
                        </ItemTemplate>
                        <HeaderStyle Width="30" />
                        <ItemStyle Width="30" VerticalAlign="Middle" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="Username" HeaderText="User Name" />
                    <asp:BoundField DataField="RoleName" HeaderText="Role" />
                    <asp:BoundField DataField="CreateDate" HeaderText="Updated on" DataFormatString="{0:dd-MM-yyyy  [HH:mm]}" />
                    <asp:BoundField DataField="LastActivityDate" HeaderText="LastActivityDate" DataFormatString="{0:dd-MM-yyyy  [HH:mm]}" />
                    <asp:BoundField DataField="LastLoginDate" HeaderText="LastLoginDate" DataFormatString="{0:dd-MM-yyyy  [HH:mm]}" />
                    <asp:TemplateField HeaderText="Forgot Password" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>

                            <a href="#" style="color: #64CA64;" data-toggle="modal" data-target="#myModelResetPassword" onclick='<%# "ResetPwd(this, \"" + Eval("UserId" )  + "\",\"" + Eval("UserName" ) + "\",\"" + Eval("Email" ) + "\");" %>' title="Reset Password">Reset Password</a>

                        </ItemTemplate>
                        <HeaderStyle Width="100" Height="25" />
                        <ItemStyle Width="100" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Locked" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <img src='<%# Eval("IsLockedOut").ToString().Trim() == "True" ? "../../App_Themes/functions/locked.png" :"../../App_Themes/functions/unlocked.png" %>' alt="-" width="16px" height="16px" />
                        </ItemTemplate>
                        <HeaderStyle Width="50" Height="25" />
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approved" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                           
                            <img src='<%# Eval("IsApproved").ToString().Trim() == "True" ? "../../App_Themes/functions/active.png" :"../../App_Themes/functions/inactive.png" %>' alt="Photo" />
                        </ItemTemplate>
                        <HeaderStyle Width="50" Height="25" />
                        <ItemStyle Width="30" HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Left" />
                <RowStyle HorizontalAlign="Left" Height="25" />
            </asp:GridView>
            <br /><br />
        </div>

        <div class="panel-body">

            <table>
                <tr>
                    <td style="padding-left: 7px; width: 8%;">File:</td>

                    <td style="width: 30%; vertical-align: calc()">
                        <asp:FileUpload ID="fUpload" runat="server" />
                    </td>
                    <td>Password:</td>
                    <td style="vertical-align: calc()">
                        <asp:TextBox ID="txtPasswordImport" runat="server"></asp:TextBox>
                    </td>
                   
                    <td style="width: 42%; vertical-align: top">
                        <asp:Button ID="btnImport" runat="server" CssClass="btn" Style="border: 1pt groove #d5d5d5; Width: 70px" Text="Import" OnClick="btnImport_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="vertical-align:central;">
                        <asp:Label ID="lblImportMessage" runat="server" Text="" CssClass="star"></asp:Label>
                    </td>
                </tr>
            </table>

        </div>
    </div>

    <%--  Model Add new User--%>

    <div id="myModalAddUser" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalAddUserHeader" aria-hidden="true"  >
        <div class="panel panel-default">
            <div class="panel-heading">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 class="panel-title">Add New User Account</h3>
            </div>
            <div class="panel-body" style="background-color: #e5e3e3">
             
                                <table class="table table-bordered" style="width: 100%">

                                    <tr>
                                        <td align="right" style="width: 150px;">
                                            <asp:Label ID="lblUserName" runat="server" >User Name: 
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUserNameC" runat="server" Width="96%" Height="10%"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <td>&nbsp;
                                           <asp:Label ID="lblRole" runat="server">Role: </asp:Label>
                                    </td>
                                    <td style="vertical-align: calc()">
                                        <asp:DropDownList ID="ddlUserRole"  runat="server" AppendDataBoundItems="true" class="span5">
                                            <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <tr>
                                        <td align="right" style="width: 150px;">&nbsp;
                                            <asp:Label ID="lblPassword" runat="server" >Password:
                                               
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="96%" Height="10%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 150px;">&nbsp;
                                            <asp:Label ID="lblConfirmPassword" runat="server" >Re-Password:
                                               
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Width="96%" Height="10%"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" style="width: 150px;">&nbsp;
                                            <asp:Label ID="lblEmail" runat="server" >E-mail:
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Email" runat="server" Width="96%" Height="10%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:center;" colspan="2">
                                            <asp:Label ID="lblMessage" runat="server" style="color:red;"></asp:Label>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="text-align:right;" colspan="2">
                                           <asp:Button ID="bntCreate" runat="server" Text="Create" OnClick="bntCreate_Click" />
                                        </td>
                                    </tr>
                                </table>
                           
            </div>
            
        </div>
    </div>

    <%--Edit User account--%>

    <div id="myModelEditUser" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModelEditUserHeader" aria-hidden="true">
        <div class="panel panel-default">
            <div class="panel-heading">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 class="panel-title">Edit User Account</h3>
            </div>
            <div class="panel-body" style="background-color: #e5e3e3">
                <%--Create new account--%>

                <table class="table table-bordered" style="width: 100%">

                    <tr>
                        <td align="right" style="width: 150px;">&nbsp;  
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="editUserName">User Name:      
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="red" ControlToValidate="editUserName" ErrorMessage="Username is required." ToolTip="Username is required." ValidationGroup="edit">*</asp:RequiredFieldValidator>
                            </asp:Label>
                        </td>
                        <td>

                            <asp:TextBox ID="editUserName" runat="server" ReadOnly="true" Width="96%" Height="10%"></asp:TextBox>
                            <asp:HiddenField ID="hdfUserId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Label ID="RoleLable" runat="server" AssociatedControlID="editddlUserRole">Role:<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="red" ControlToValidate="editddlUserRole" ErrorMessage="Role is required." ToolTip="Role is required." ValidationGroup="edit" InitialValue="0">*</asp:RequiredFieldValidator></asp:Label>
                        </td>
                        <td style="vertical-align: calc()">
                            <asp:DropDownList ID="editddlUserRole" runat="server" AppendDataBoundItems="true" class="span5">
                                <asp:ListItem Selected="True" Value="0">.</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" style="width: 150px;">&nbsp;
                             <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="editEmail">E-mail:
   <%--                             <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ForeColor="red" ControlToValidate="editEmail" ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="edit">*</asp:RequiredFieldValidator>
                                 <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="editEmail" ErrorMessage="Format!" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                             </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="editEmail" runat="server" Width="96%" Height="10%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="editddlUserRole">Approved:</asp:Label>
                        </td>
                        <td style="vertical-align: calc()">
                            <asp:DropDownList ID="editddlStatus" runat="server" AppendDataBoundItems="true" class="span5">
                                <asp:ListItem Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="editddlLocked">Locked:</asp:Label>
                        </td>
                        <td style="vertical-align: calc()">
                            <asp:DropDownList ID="editddlLocked" runat="server" AppendDataBoundItems="true" class="span5">
                                <asp:ListItem Value="1">Locked</asp:ListItem>
                                <asp:ListItem Value="0">Unlocked</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">


                            <div align="right">
                                <div style="display: none;">
                                    <asp:Button ID="btnEditUser" runat="server" OnClick="btnEditUser_Click" ValidationGroup="edit" />
                                </div>
                                <input id="Button1" type="button" runat="server" class="btn btn-primary" style="height: 27px;" onclick="EditUser();" value="Update" title="Update" />

                                <button class="btn" data-dismiss="modal" aria-hidden="true" title="Cancel">Cancel</button>
                            </div>
                        </td>
                    </tr>

                </table>

            </div>
        </div>
    </div>

    <%--Reset Password User account--%>

    <div id="myModelResetPassword" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModelResetPasswordHeader" aria-hidden="true">

        <div class="panel panel-default">
            <div class="panel-heading">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h3 class="panel-title">Reset Password User Account</h3>
            </div>
            <div class="panel-body" style="background-color: #e5e3e3">
                <%--Reset Password account--%>

                <table class="table table-bordered" style="width: 100%">

                    <tr>
                        <td align="right" style="width: 150px;">&nbsp;  
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="ResettxtUsername">User Name:      
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="red" ControlToValidate="ResettxtUsername" ErrorMessage="Username is required." ToolTip="Username is required." ValidationGroup="reset">*</asp:RequiredFieldValidator>
                            </asp:Label>
                        </td>
                        <td>

                            <asp:TextBox ID="ResettxtUsername" runat="server" ReadOnly="true" Width="96%" Height="10%"></asp:TextBox>
                            <asp:HiddenField ID="hdfResettxtUserId" runat="server" />
                            <asp:HiddenField ID="hdfResettxtUsername" runat="server" />

                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 150px;">&nbsp;  
                            <asp:Label ID="Label4" runat="server" AssociatedControlID="ResettxtUsername">Email:                                         
                            </asp:Label>
                        </td>
                        <td>

                            <asp:TextBox ID="ResettxtEmail" runat="server" ReadOnly="true" Width="96%" Height="10%"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 150px;">&nbsp;
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="ResetPassword">New-Password:
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ForeColor="red" ControlToValidate="ResetPassword" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="reset">*</asp:RequiredFieldValidator>
                                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ResetPassword" runat="server" TextMode="Password" Width="96%" Height="10%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 150px;">&nbsp;
                                            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ResetConfirmPassword">Re-Password:
                                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" ForeColor="red" runat="server" ControlToValidate="ResetConfirmPassword" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required." ValidationGroup="reset">*</asp:RequiredFieldValidator>
                                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ResetConfirmPassword" runat="server" TextMode="Password" Width="96%" Height="10%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:CompareValidator ID="PasswordCompare" runat="server" ForeColor="red" ControlToCompare="ResetPassword" ControlToValidate="ResetConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match." ValidationGroup="reset"></asp:CompareValidator>
                            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False" ></asp:Literal>

                            <asp:CustomValidator ID="CustomValidator1"
                                ClientValidationFunction="ClientValidate"
                                ControlToValidate="ResetPassword" runat="server"
                                ErrorMessage="The password must be at least 8 characters."
                                Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                            <asp:Label ID="lblResetMessage" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">


                            <div align="right">
                                <div style="display: none;">
                                    <asp:Button ID="btnresetpwd" runat="server" OnClick="btnresetpwd_Click" ValidationGroup="reset" />
                                </div>
                                <input id="Button2" type="button" runat="server" class="btn btn-primary" style="height: 27px;" onclick="ResetPwdUser();" value="Reset Password" title="Reset Password" />

                                <button class="btn" data-dismiss="modal" aria-hidden="true" title="Cancel">Cancel</button>
                            </div>
                        </td>
                    </tr>

                </table>

            </div>
        </div>
    </div>

    <%--  Initailize value--%>

    <asp:HiddenField ID="hdfUserName" runat="server" />
    <asp:HiddenField ID="hdfOldRoleId" runat="server" />
</asp:Content>


