<%@ Page Title="Clife System | Setting => Change Password" Language="C#" MasterPageFile="~/Pages/Content.master" AutoEventWireup="true" CodeFile="change_password.aspx.cs" Inherits="Pages_Setting_change_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Toolbar" runat="Server">

    <ul class="toolbar" style="text-align: left">


    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
  
   <%-- Form Design Section--%>
    <h1>Change Password</h1>

    <div style="height:30px"></div>           
            <div class="panel panel-default">

                <div class="panel-body">

                    <div style="text-align: left;">
                        
                        <table width="100%" class="tbstyle">
                            <tr>
                                <td width="100%">
                                    <br />
                                    Current Password<span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:TextBox ID="txtOldPassword" runat="server" Width="94%" TabIndex="1" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtOldPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">New Password<span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:TextBox ID="txtNewPassword" runat="server" Width="94%" TabIndex="2" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtNewPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">Confirm New Password<span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" Width="94%" TabIndex="3" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtConfirmNewPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" align="left">
                                    <br />

                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 100%">
                                    <asp:Label ID="lblmessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width: "100%">
                                    <asp:Button ID="ImgBtnChange" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Change" onClick="ImgBtnChange_Click" />
                                </td>
                            </tr>
                        </table>

                    </div>

                </div>
            </div>

      </asp:Content>

