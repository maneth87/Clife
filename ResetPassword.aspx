<%@ Title="Clife System | Reset Password" Language="C#" CodeFile="ResetPassword.aspx.cs" Inherits="ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Clife System | Login</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <%-- stylesheet--%>
    <link href="App_Themes/style.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/jmenu.css" rel="stylesheet" type="text/css" />
    <link href="scripts/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />

    <%-- javascript--%>
    <script src="scripts/jquery-1.9.0.min.js"></script>
    <script src="scripts/jmenu.js"></script>
    <script type="text/javascript" src="scripts/bootstrap/js/bootstrap.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>

    <script type="text/javascript">

        function showSuccessMessage(message) {
            swal("Success!", message, "success", {
                button: "Ok",
            });
        }


        function showFailMessage(message) {
            swal("Failed!", message, "error", {
                button: "Ok",
            });
        }

    </script>
</head>
<body>
    <div class="container">
        <form id="form2" runat="server">
        <h1 style="padding:5px">Reset Password</h1>

            <div class="panel panel-default">

                <div class="panel-body">

                    <div style="text-align: left;">
                        
                        <table width="20%" class="tbstyle">
                            <tr>
                                <td width="100%">
                                    <br />
                                    Email<span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:TextBox ID="txtEmail" runat="server" Enabled="false" Width="94%" TabIndex="1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">User Name<span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:TextBox ID="txtUserName" runat="server" Enabled="false" Width="94%" TabIndex="2" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtUserName" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">Full Name<span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:TextBox ID="txtFullName" runat="server" Enabled="false" Width="94%" TabIndex="3"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtFullName" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 100%">
                                    <br />

                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 100%">
                                    <asp:Label ID="lblmessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 100%">
                                    <asp:Button ID="btnLogin" class="btn btn-success" runat="server" Style="height: 27px;" Text="Login" onClick="btnLogin_Click" />
                                    <asp:Button ID="btnReset" class="btn btn-primary" runat="server" Style="height: 27px;" Text="Reset" onClick="btnReset_Click" />
                                </td>
                            </tr>
                        </table>

                    </div>

                </div>
            </div>
        </form>
    </div>

    
    

</body>
</html>

